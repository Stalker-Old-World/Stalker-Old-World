namespace Content.Shared._Stalker.ZoneAnomaly.Effects.Components;

[RegisterComponent]
public sealed partial class ZoneAnomalyEffectHeatComponent : Component
{
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public bool IgnoreResistance;

    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public float Heat = 10f;

    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public bool Update;

    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public TimeSpan UpdateTime;

    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public TimeSpan UpdateDelay = TimeSpan.FromSeconds(0.05f);
}
