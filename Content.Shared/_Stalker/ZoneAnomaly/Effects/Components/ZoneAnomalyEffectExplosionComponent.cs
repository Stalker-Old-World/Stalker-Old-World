using Content.Shared.Explosion;
using Robust.Shared.Prototypes;

namespace Content.Shared._Stalker.ZoneAnomaly.Effects.Components;

[RegisterComponent]
public sealed partial class ZoneAnomalyEffectExplosionComponent : Component
{
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public ProtoId<ExplosionPrototype> ProtoId = "Son";

    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public float TotalIntensity = 500f;

    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public float Slope = 4f;

    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public float MaxTileIntensity = 1000f;
}
