using Content.Shared.Chemistry.Components;
using Robust.Shared.Prototypes;

namespace Content.Shared._Stalker.Psyonics.Actions.Smoke;

[RegisterComponent]
public sealed partial class PsyonicsActionSmokeComponent : BasePsyonicsActionComponent
{
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public override EntProtoId ActionId { get; set; } = "ActionPsyonicsSmoke";

    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public override int Cost { get; set; } = 25;

    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public EntProtoId SmokePrototype = "Smoke";

    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public Solution Solution = new();

    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public TimeSpan Duration = TimeSpan.FromSeconds(10f);

    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public float Quantity = 50f;

    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public int SpreadAmount = 30;
}
