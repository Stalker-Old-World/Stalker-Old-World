using Content.Shared.Damage.Prototypes;
using Content.Shared.FixedPoint;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype.List;

namespace Content.Shared._Stalker.Psyonics.Actions.Light;

[RegisterComponent]
public sealed partial class PsyonicsActionLightComponent : BasePsyonicsActionComponent
{
    public TimeSpan LastDecay = TimeSpan.FromSeconds(0f);
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public override EntProtoId ActionId { get; set; } = "ActionPsyonicsLight";

    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public override int Cost { get; set; } = 0;

    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public int PricePerPeriod { get; set; } = 1;

    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public int PeriodSeconds { get; set; } = 10;

    [DataField, ViewVariables(VVAccess.ReadOnly)]
    public bool IsActive = false;
}
