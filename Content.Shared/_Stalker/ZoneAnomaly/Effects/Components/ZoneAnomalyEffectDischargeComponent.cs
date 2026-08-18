using Content.Shared.Whitelist;

namespace Content.Shared._Stalker.ZoneAnomaly.Effects.Components;

[RegisterComponent]
public sealed partial class ZoneAnomalyEffectDischargeComponent : Component
{
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public EntityWhitelist Whitelist = new();

    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public float DischargePercentage = 1f;

    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public TimeSpan DischargeUpdateTime;

    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public TimeSpan DischargeUpdateDelay = TimeSpan.FromSeconds(1f);
}
