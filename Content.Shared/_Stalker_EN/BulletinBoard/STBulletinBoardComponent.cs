using Robust.Shared.GameObjects;
using Robust.Shared.Maths;
using Robust.Shared.Serialization.Manager.Attributes;

namespace Content.Shared._Stalker_EN.BulletinBoard;

/// <summary>
/// Shared component for a generic bulletin board PDA cartridge.
/// All configuration is via DataFields — new board types need only a new YAML prototype.
/// </summary>
[RegisterComponent]
public sealed partial class STBulletinBoardComponent : Component
{
    /// <summary>Board type ID for offer storage grouping. All cartridges with same ID share one pool.</summary>
    [DataField]
    public string BoardTypeId = "default";

    // Localization keys (LocIds, resolved client-side)
    [DataField] public string HeaderTitle = "st-bulletin-header";
    [DataField] public string PrimaryTabName = "st-bulletin-tab-primary";
    [DataField] public string SecondaryTabName = "st-bulletin-tab-secondary";
    [DataField] public string PrimaryPostButton = "st-bulletin-post-primary";
    [DataField] public string SecondaryPostButton = "st-bulletin-post-secondary";
    [DataField] public string PrimaryInfoLabel = "st-bulletin-info-primary";
    [DataField] public string SecondaryInfoLabel = "st-bulletin-info-secondary";
    [DataField] public string SecondaryHint = "";
    [DataField] public string NewPrimaryTitle = "st-bulletin-new-primary";
    [DataField] public string NewSecondaryTitle = "st-bulletin-new-secondary";
    [DataField] public string SearchPlaceholder = "st-bulletin-search";

    /// <summary>Offer reference prefix (e.g. "MB#", "TB#").</summary>
    [DataField]
    public string OfferRefPrefix = "BB#";

    // Card colors
    [DataField] public Color PrimaryBorderColor = Color.FromHex("#4488CC");
    [DataField] public Color PrimaryOwnBorderColor = Color.FromHex("#66AAEE");
    [DataField] public Color SecondaryBorderColor = Color.FromHex("#CCAA44");
    [DataField] public Color SecondaryOwnBorderColor = Color.FromHex("#EEDD66");

    // Limits
    [DataField] public int MaxOffersPerPlayer = 1;
    [DataField] public int MaxTotalOffers = 100;
    [DataField] public int MaxDescriptionLength = 300;
    [DataField] public TimeSpan ContactCooldown = TimeSpan.FromSeconds(1);
}
