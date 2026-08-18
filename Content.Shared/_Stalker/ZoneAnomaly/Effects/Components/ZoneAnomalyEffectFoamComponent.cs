using Content.Shared.Chemistry.Reagent;
using Robust.Shared.Prototypes;

namespace Content.Shared._Stalker.ZoneAnomaly.Effects.Components;

[RegisterComponent]
public sealed partial class ZoneAnomalyEffectFoamComponent : Component
{
    [DataField]
    public ProtoId<ReagentPrototype> Reagent;

    [DataField]
    public float ReagentAmount;

    [DataField]
    public int Range;

    [DataField]
    public float Duration;
}
