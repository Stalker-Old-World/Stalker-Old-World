using Robust.Shared.Map;

namespace Content.Server._Stalker.Anomaly.Generation.Components;

[RegisterComponent]
public sealed partial class STAnomalyGeneratorComponent : Component
{
    [ViewVariables]
    public Dictionary<MapId, HashSet<EntityUid>> MapGeneratedAnomalies = new();
}
