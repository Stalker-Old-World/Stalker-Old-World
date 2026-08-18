using Robust.Shared.Prototypes;

namespace Content.Shared._Stalker.Psyonics.Actions.Grab;

[RegisterComponent]
public sealed partial class PsyonicsActionGrabComponent : BasePsyonicsActionComponent
{
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public override EntProtoId ActionId { get; set; } = "ActionPsyonicsGrab";

    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public override int Cost { get; set; } = 25;
}
