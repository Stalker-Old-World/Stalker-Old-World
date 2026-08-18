using Content.Shared.Polymorph;
using Robust.Shared.Prototypes;

namespace Content.Shared._Stalker.ZoneAnomaly.Effects.Components;

[RegisterComponent]
public sealed partial class ZoneAnomalyEffectPolymorphComponent : Component
{
    [DataField]
    public List<ProtoId<PolymorphPrototype>> Polymorph = new();
}
