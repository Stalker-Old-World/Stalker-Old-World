using Robust.Shared.GameObjects;
using Robust.Shared.Serialization;

namespace Content.Shared._Stalker_EN.BulletinBoard;

/// <summary>
/// Full UI state for a bulletin board cartridge, sent via the CartridgeLoader BUI system.
/// </summary>
[Serializable, NetSerializable]
public sealed class STBulletinUiState : BoundUserInterfaceState
{
    /// <summary>Board configuration (display text, colors, permissions).</summary>
    public readonly STBulletinBoardConfig Config;

    /// <summary>Active primary-category offers visible to this player.</summary>
    public readonly List<STBulletinOffer> PrimaryOffers;

    /// <summary>Active secondary-category offers visible to this player.</summary>
    public readonly List<STBulletinOffer> SecondaryOffers;

    /// <summary>Character name of the PDA owner (used to identify own offers).</summary>
    public readonly string OwnerCharacterName;

    /// <summary>Number of active primary offers posted by this player.</summary>
    public readonly int MyPrimaryCount;

    /// <summary>Number of active secondary offers posted by this player.</summary>
    public readonly int MySecondaryCount;

    /// <summary>
    /// One-shot search pre-fill from the server (e.g. when navigating from an offer link).
    /// Null means no search pre-fill requested.
    /// </summary>
    public readonly string? SearchQuery;

    /// <summary>
    /// One-shot tab switch from the server (e.g. when navigating from an offer link).
    /// Null means keep the current tab.
    /// </summary>
    public readonly STBulletinCategory? ActiveCategory;

    /// <summary>
    /// Whether this board cartridge's notifications are muted (ringer suppressed, badge still appears).
    /// </summary>
    public readonly bool IsMuted;

    public STBulletinUiState(
        STBulletinBoardConfig config,
        List<STBulletinOffer> primaryOffers,
        List<STBulletinOffer> secondaryOffers,
        string ownerCharacterName,
        int myPrimaryCount,
        int mySecondaryCount,
        bool isMuted,
        string? searchQuery = null,
        STBulletinCategory? activeCategory = null)
    {
        Config = config;
        PrimaryOffers = primaryOffers;
        SecondaryOffers = secondaryOffers;
        OwnerCharacterName = ownerCharacterName;
        MyPrimaryCount = myPrimaryCount;
        MySecondaryCount = mySecondaryCount;
        IsMuted = isMuted;
        SearchQuery = searchQuery;
        ActiveCategory = activeCategory;
    }
}
