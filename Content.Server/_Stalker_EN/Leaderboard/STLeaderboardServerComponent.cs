using Robust.Shared.GameObjects;
using Robust.Shared.Serialization.Manager.Attributes;
using Robust.Shared.ViewVariables;

namespace Content.Server._Stalker_EN.Leaderboard;

/// <summary>
/// Server-side leaderboard state for a PDA cartridge.
/// Not networked — client receives data via <see cref="STLeaderboardUiState"/> through the BUI system.
/// Stores owner information to avoid having to search for ActorComponent on every update.
/// </summary>
[RegisterComponent, AutoGenerateComponentPause]
[Access(typeof(STLeaderboardSystem))]
public sealed partial class STLeaderboardServerComponent : Component
{
    /// <summary>
    /// The player's account user ID (from NetUserId). Used as part of the composite identity key.
    /// </summary>
    [ViewVariables]
    public Guid OwnerUserId;

    /// <summary>
    /// Character name of the PDA's original owner.
    /// Stored as string so it survives entity deletion (e.g. body cleanup after death).
    /// </summary>
    [ViewVariables]
    public string OwnerCharacterName = string.Empty;
}
