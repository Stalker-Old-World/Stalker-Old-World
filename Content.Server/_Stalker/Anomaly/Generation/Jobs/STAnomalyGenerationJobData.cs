namespace Content.Server._Stalker.Anomaly.Generation.Jobs;

public sealed class STAnomalyGenerationJobData
{
    [DataField]
    public HashSet<EntityUid> SpawnedAnomalies = new();
}
