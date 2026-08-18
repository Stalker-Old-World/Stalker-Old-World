using Content.Shared._Stalker.Bands;
using Robust.Shared.GameObjects;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.Manager.Attributes;

namespace Content.Shared._Stalker_EN.BulletinBoard;

/// <summary>
/// Optional marker component for merc-board-specific restrictions.
/// When present on a bulletin board cartridge, the system applies:
/// - Only members of the specified band can post Primary offers
/// - Non-members only see their own Secondary offers
/// </summary>
[RegisterComponent]
public sealed partial class STMercBoardRestrictionsComponent : Component
{
    [DataField]
    public ProtoId<STBandPrototype> RequiredBandForPrimary = "STMercenariesBand";
}
