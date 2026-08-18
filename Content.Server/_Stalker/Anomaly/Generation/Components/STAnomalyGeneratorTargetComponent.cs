using Content.Shared._Stalker.Anomaly.Prototypes;
using Robust.Shared.Prototypes;

namespace Content.Server._Stalker.Anomaly.Generation.Components;

[RegisterComponent]
public sealed partial class STAnomalyGeneratorTargetComponent : Component
{
    [DataField]
    public ProtoId<STAnomalyGenerationOptionsPrototype> OptionsId;
}
