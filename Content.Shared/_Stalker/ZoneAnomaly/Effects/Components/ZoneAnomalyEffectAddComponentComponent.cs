using Robust.Shared.Prototypes;

namespace Content.Shared._Stalker.ZoneAnomaly.Effects.Components;

[RegisterComponent]
public sealed partial class ZoneAnomalyEffectAddComponentComponent : Component
{
    [DataField, AlwaysPushInheritance]
    public ComponentRegistry Components { get; private set; } = new();
}
