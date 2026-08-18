using Robust.Shared.Maths;
using Robust.Shared.Serialization;

namespace Content.Shared._Stalker_EN.BulletinBoard;

/// <summary>
/// Configuration record sent to clients as part of <see cref="STBulletinUiState"/>.
/// All display text fields are LocIds — resolved client-side via <c>Loc.GetString</c>.
/// Built server-side from <see cref="STBulletinBoardComponent"/> DataFields.
/// </summary>
[Serializable, NetSerializable]
public sealed class STBulletinBoardConfig
{
    // Localization keys
    public readonly string HeaderTitle;
    public readonly string PrimaryTabName;
    public readonly string SecondaryTabName;
    public readonly string PrimaryPostButton;
    public readonly string SecondaryPostButton;
    public readonly string PrimaryInfoLabel;
    public readonly string SecondaryInfoLabel;
    public readonly string SecondaryHint;
    public readonly string NewPrimaryTitle;
    public readonly string NewSecondaryTitle;
    public readonly string SearchPlaceholder;
    public readonly string OfferRefPrefix;

    // Colors
    public readonly Color PrimaryBorderColor;
    public readonly Color PrimaryOwnBorderColor;
    public readonly Color SecondaryBorderColor;
    public readonly Color SecondaryOwnBorderColor;

    // Permission flags (computed server-side from restriction components)
    public readonly bool CanPostPrimary;
    public readonly bool CanPostSecondary;
    public readonly bool ShowSecondaryCountBadge;

    // Limits
    public readonly int MaxDescriptionLength;
    public readonly int MaxOffersPerPlayer;

    public STBulletinBoardConfig(
        string headerTitle,
        string primaryTabName,
        string secondaryTabName,
        string primaryPostButton,
        string secondaryPostButton,
        string primaryInfoLabel,
        string secondaryInfoLabel,
        string secondaryHint,
        string newPrimaryTitle,
        string newSecondaryTitle,
        string searchPlaceholder,
        string offerRefPrefix,
        Color primaryBorderColor,
        Color primaryOwnBorderColor,
        Color secondaryBorderColor,
        Color secondaryOwnBorderColor,
        bool canPostPrimary,
        bool canPostSecondary,
        bool showSecondaryCountBadge,
        int maxDescriptionLength,
        int maxOffersPerPlayer)
    {
        HeaderTitle = headerTitle;
        PrimaryTabName = primaryTabName;
        SecondaryTabName = secondaryTabName;
        PrimaryPostButton = primaryPostButton;
        SecondaryPostButton = secondaryPostButton;
        PrimaryInfoLabel = primaryInfoLabel;
        SecondaryInfoLabel = secondaryInfoLabel;
        SecondaryHint = secondaryHint;
        NewPrimaryTitle = newPrimaryTitle;
        NewSecondaryTitle = newSecondaryTitle;
        SearchPlaceholder = searchPlaceholder;
        OfferRefPrefix = offerRefPrefix;
        PrimaryBorderColor = primaryBorderColor;
        PrimaryOwnBorderColor = primaryOwnBorderColor;
        SecondaryBorderColor = secondaryBorderColor;
        SecondaryOwnBorderColor = secondaryOwnBorderColor;
        CanPostPrimary = canPostPrimary;
        CanPostSecondary = canPostSecondary;
        ShowSecondaryCountBadge = showSecondaryCountBadge;
        MaxDescriptionLength = maxDescriptionLength;
        MaxOffersPerPlayer = maxOffersPerPlayer;
    }
}
