using Content.Server.Database;

namespace Content.Server._Stalker_EN.PdaMessenger;

/// <summary>
/// Database integration methods for the messenger system.
/// All async void methods are wrapped in try-catch to prevent silent exception swallowing.
/// </summary>
public sealed partial class STMessengerSystem
{
    /// <summary>
    /// Bulk-loads all messenger IDs from the database into the in-memory cache.
    /// Called once during system initialization.
    /// </summary>
    private async void LoadMessengerIdCacheAsync()
    {
        try
        {
            var ids = await _db.GetAllStalkerMessengerIdsAsync();
            foreach (var entry in ids)
            {
                var key = (entry.UserId, entry.CharacterName);
                _messengerIdCache[entry.MessengerId] = key;
                _characterToMessengerId[key] = entry.MessengerId;
            }

            Log.Info($"Loaded {ids.Count} messenger IDs into cache.");
        }
        catch (Exception ex)
        {
            Log.Error($"Failed to load messenger ID cache from DB: {ex}");
        }
    }

    /// <summary>
    /// Loads or generates a messenger ID for the given (userId, characterName) identity.
    /// If the identity already has an ID in the database, it is used.
    /// Otherwise a new unique ID is generated and persisted.
    /// </summary>
    private async void LoadOrGenerateMessengerIdAsync(
        EntityUid cartridgeUid,
        Guid userId,
        string characterName)
    {
        try
        {
            var identityKey = (userId, characterName);

            // Fast path: already cached from bulk load or prior spawn
            if (_characterToMessengerId.TryGetValue(identityKey, out var cachedId))
            {
                if (!Deleted(cartridgeUid) && TryComp<STMessengerServerComponent>(cartridgeUid, out var cachedServer))
                    cachedServer.MessengerId = cachedId;

                return;
            }

            var existing = await _db.GetStalkerMessengerIdAsync(userId, characterName);

            // Entity may have been deleted while we were waiting for DB
            if (Deleted(cartridgeUid) || !TryComp<STMessengerServerComponent>(cartridgeUid, out var server))
                return;

            if (existing is not null)
            {
                server.MessengerId = existing.MessengerId;
                _messengerIdCache[existing.MessengerId] = identityKey;
                _characterToMessengerId[identityKey] = existing.MessengerId;
                return;
            }

            string newId;
            var attempts = 0;
            do
            {
                newId = GenerateMessengerId(_random);
                attempts++;
            }
            while (_messengerIdCache.ContainsKey(newId) && attempts < MaxRetryCollision);

            if (attempts >= MaxRetryCollision)
            {
                Log.Error($"Failed to generate unique messenger ID for {characterName} after {MaxRetryCollision} attempts.");
                return;
            }

            // Persist to DB first, then update local state (avoids stale state on DB failure)
            await _db.SetStalkerMessengerIdAsync(userId, characterName, newId);

            // Re-check entity validity after second await
            if (Deleted(cartridgeUid) || !TryComp<STMessengerServerComponent>(cartridgeUid, out server))
            {
                // Entity gone, but cache the ID so it persists across rounds
                _messengerIdCache[newId] = identityKey;
                _characterToMessengerId[identityKey] = newId;
                return;
            }

            server.MessengerId = newId;
            _messengerIdCache[newId] = identityKey;
            _characterToMessengerId[identityKey] = newId;

            Log.Debug($"Generated messenger ID {newId} for {characterName}.");
        }
        catch (Exception ex)
        {
            Log.Error($"Failed to load/generate messenger ID for {characterName}: {ex}");
        }
    }

    /// <summary>
    /// Loads contacts from the database for the given identity and populates the component.
    /// </summary>
    private async void LoadContactsAsync(
        EntityUid cartridgeUid,
        Guid userId,
        string characterName)
    {
        try
        {
            var contacts = await _db.GetStalkerMessengerContactsAsync(userId, characterName);

            // Entity may have been deleted while we were waiting for DB
            if (Deleted(cartridgeUid) || !TryComp<STMessengerServerComponent>(cartridgeUid, out var server))
                return;

            foreach (var contact in contacts)
            {
                var contactKey = (contact.ContactUserId, contact.ContactCharacterName);

                // Look up the contact's messenger ID from cache
                if (!_characterToMessengerId.TryGetValue(contactKey, out var contactMessengerId))
                    continue; // Skip contacts whose messenger ID isn't cached yet

                server.Contacts[contactMessengerId] = new STContactEntry(
                    contact.ContactUserId, contact.ContactCharacterName, contact.FactionName);
            }

            Log.Debug($"Loaded {contacts.Count} contacts for {characterName}.");

            // Refresh all active messenger UIs so contacts see this player's current faction.
            // Done here (after DB loads complete) rather than in InitializeMessengerForPda
            // to avoid a wasted broadcast before contacts are populated.
            MarkBroadcastPending(null);
        }
        catch (Exception ex)
        {
            Log.Error($"Failed to load contacts for {characterName}: {ex}");
        }
    }

    /// <summary>
    /// Persists a new contact to the database.
    /// </summary>
    private async void AddContactAsync(
        Guid ownerUserId, string ownerName,
        Guid contactUserId, string contactName,
        string? factionName)
    {
        try
        {
            await _db.AddStalkerMessengerContactAsync(ownerUserId, ownerName, contactUserId, contactName, factionName);
        }
        catch (Exception ex)
        {
            Log.Error($"Failed to persist contact {contactName} for {ownerName}: {ex}");
        }
    }

    /// <summary>
    /// Persists an updated faction name for an existing contact.
    /// </summary>
    private async void UpdateContactFactionAsync(
        Guid ownerUserId, string ownerName,
        Guid contactUserId, string contactName,
        string factionName)
    {
        try
        {
            await _db.UpdateStalkerMessengerContactFactionAsync(ownerUserId, ownerName, contactUserId, contactName, factionName);
        }
        catch (Exception ex)
        {
            Log.Error($"Failed to update faction for contact {contactName} of {ownerName}: {ex}");
        }
    }

    /// <summary>
    /// Removes a contact from the database.
    /// </summary>
    private async void RemoveContactAsync(
        Guid ownerUserId, string ownerName,
        Guid contactUserId, string contactName)
    {
        try
        {
            await _db.RemoveStalkerMessengerContactAsync(ownerUserId, ownerName, contactUserId, contactName);
        }
        catch (Exception ex)
        {
            Log.Error($"Failed to remove contact {contactName} for {ownerName}: {ex}");
        }
    }
}
