using Robust.Shared.Prototypes;

namespace Content.Shared._Stalker.Psyonics.Actions.Springboard;

[RegisterComponent]
public sealed partial class PsyonicsActionSpringboardComponent : BasePsyonicsActionComponent
{
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public override EntProtoId ActionId { get; set; } = "ActionPsyonicsSpringboard";

    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public override int Cost { get; set; } = 50;

    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public float Radius = 1.5f;

    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public float Strength = 10f;

    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public bool Stun = true;

    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public TimeSpan StunDuration = TimeSpan.FromSeconds(2f);
}
