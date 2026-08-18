using Content.Shared._Stalker_EN.BulletinBoard;
using Robust.Shared.GameObjects;
using Robust.Shared.ViewVariables;

namespace Content.Server._Stalker_EN.BulletinBoard;

/// <summary>
/// Server-side state for a bulletin board PDA cartridge instance.
/// Not networked — client receives data via <see cref="Content.Shared._Stalker_EN.BulletinBoard.STBulletinUiState"/>
/// through the BUI system.
/// </summary>
[RegisterComponent, AutoGenerateComponentPause]
[Access(typeof(STBulletinBoardSystem))]
public sealed partial class STBulletinServerComponent : Component
{
    /// <summary>
    /// The player's account user ID (from NetUserId). Used with character name as composite identity.
    /// </summary>
    [ViewVariables]
    public Guid OwnerUserId;

    /// <summary>
    /// Character name of the PDA's owner.
    /// </summary>
    [ViewVariables]
    public string OwnerCharacterName = string.Empty;

    /// <summary>
    /// The mob entity owning this PDA. Used for dynamic band membership checks.
    /// </summary>
    [ViewVariables]
    public EntityUid? OwnerMob;

    /// <summary>
    /// Next allowed contact time (absolute simulation time). Rate-limits contact button usage.
    /// </summary>
    [AutoPausedField]
    [ViewVariables]
    public TimeSpan NextContactTime;

    /// <summary>
    /// Whether notifications for this board cartridge are muted (badge still appears, ringer suppressed).
    /// </summary>
    [ViewVariables]
    public bool Muted;

    /// <summary>
    /// One-shot search pre-fill. Consumed and cleared by the next UI state update.
    /// Set by external systems (e.g. messenger offer link navigation).
    /// </summary>
    [ViewVariables]
    public string? PendingSearchQuery;

    /// <summary>
    /// One-shot category tab switch. Consumed and cleared by the next UI state update.
    /// Set alongside <see cref="PendingSearchQuery"/> when navigating from an offer link.
    /// </summary>
    [ViewVariables]
    public STBulletinCategory? PendingCategory;
}
