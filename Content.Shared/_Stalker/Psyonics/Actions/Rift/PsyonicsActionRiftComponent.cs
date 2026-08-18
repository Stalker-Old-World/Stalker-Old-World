using Robust.Shared.Prototypes;

namespace Content.Shared._Stalker.Psyonics.Actions.Rift;

[RegisterComponent]
public sealed partial class PsyonicsActionRiftComponent : BasePsyonicsActionComponent
{
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public override EntProtoId ActionId { get; set; } = "ActionPsyonicsRift";

    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public override int Cost { get; set; } = 100;
}
