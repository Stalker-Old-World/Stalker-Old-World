namespace Content.Shared._Stalker.ZoneAnomaly.Effects.Components;

[RegisterComponent]
public sealed partial class ZoneAnomalyEffectNoGravityComponent : Component
{
    [DataField]
    public float WeightlessFriction = 0.08f;

    [DataField]
    public float WeightlessFrictionNoInput = 0.08f;

    [DataField]
    public float WeightlessAcceleration = 0f;
}
