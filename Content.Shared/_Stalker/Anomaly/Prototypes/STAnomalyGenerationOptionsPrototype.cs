using Content.Shared._Stalker.Anomaly.Data;
using Robust.Shared.Prototypes;

namespace Content.Shared._Stalker.Anomaly.Prototypes;

[Prototype("stAnomalyGenerationOptions")]
public sealed class STAnomalyGenerationOptionsPrototype : IPrototype
{
    [IdDataField]
    public string ID { get; } = string.Empty;

    [DataField]
    public STAnomalyGenerationOptions Options;
}
