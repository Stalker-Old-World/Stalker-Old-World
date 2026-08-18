using Content.Shared.Damage;

namespace Content.Shared._Stalker.ZoneAnomaly.Effects.Components;

[RegisterComponent]
public sealed partial class ZoneAnomalyEffectDamageComponent : Component
{
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public DamageSpecifier? Damage;

    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public float FireStacks;

    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public bool DamageUpdate = false;

    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public bool IgnoreResistances;

    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public bool InterruptsDoAfters = true;

    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public TimeSpan DamageUpdateTime;

    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public TimeSpan DamageUpdateDelay = TimeSpan.FromSeconds(0.05f);
}
