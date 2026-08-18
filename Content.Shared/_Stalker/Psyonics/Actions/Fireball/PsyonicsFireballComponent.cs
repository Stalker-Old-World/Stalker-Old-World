using Content.Shared.Chemistry.Components;
using Robust.Shared.Prototypes;

namespace Content.Shared._Stalker.Psyonics.Actions.Fireball;

[RegisterComponent]
public sealed partial class PsyonicsFireballComponent : BasePsyonicsActionComponent
{
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public override EntProtoId ActionId { get; set; } = "ActionPsyonicsFireball";

    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public override int Cost { get; set; } = 25;

    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public EntProtoId Prototype = "STPsyonicsProjectileFireball";

}
