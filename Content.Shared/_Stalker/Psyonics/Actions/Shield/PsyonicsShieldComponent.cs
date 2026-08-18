using Content.Shared.Damage.Prototypes;
using Content.Shared.FixedPoint;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype.List;

namespace Content.Shared._Stalker.Psyonics.Actions.Shield;

[RegisterComponent]
public sealed partial class PsyonicsActionShieldComponent : BasePsyonicsActionComponent
{
    public TimeSpan LastDecay = TimeSpan.FromSeconds(0f);
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public override EntProtoId ActionId { get; set; } = "ActionPsyonicsShield";

    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public override int Cost { get; set; } = 20;

    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public FixedPoint2 Health { get; set; } = 500f;

    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public FixedPoint2 MaxHealth { get; set; } = 500f;

    [DataField, ViewVariables(VVAccess.ReadOnly)]
    public bool IsActive = false;

    [DataField("ignoredDamageTypes", customTypeSerializer: typeof(PrototypeIdListSerializer<DamageTypePrototype>))]
    public List<string> IgnoredDamageTypes { get; private set; } = new List<string>() { "Asphyxiation", "Bloodloss", "Cellular", "Psy" };

    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public int PricePerPeriod { get; set; } = 1;

    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public int PeriodSeconds { get; set; } = 5;
}
