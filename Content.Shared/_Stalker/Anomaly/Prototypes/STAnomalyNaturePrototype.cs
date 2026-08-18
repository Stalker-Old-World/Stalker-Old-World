using Robust.Shared.Prototypes;

namespace Content.Shared._Stalker.Anomaly.Prototypes;

[Prototype("stAnomalyNature")]
public sealed class STAnomalyNaturePrototype : IPrototype
{
    [IdDataField]
    public string ID { get; } = string.Empty;
}
