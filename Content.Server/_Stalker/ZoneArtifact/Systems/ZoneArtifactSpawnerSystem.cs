using Content.Server._Stalker.ZoneArtifact.Components.Detector;
using Content.Server._Stalker.ZoneArtifact.Components.Spawner;
using Robust.Server.GameObjects;
using Robust.Shared.Map;
using Robust.Shared.Prototypes;
using Robust.Shared.Random;
using Robust.Shared.Timing;

namespace Content.Server._Stalker.ZoneArtifact.Systems;

public sealed class ZoneArtifactSpawnerSystem : EntitySystem
{
    [Dependency] private readonly IGameTiming _timing = default!;
    [Dependency] private readonly IMapManager _mapManager = default!;
    [Dependency] private readonly IPrototypeManager _prototype = default!;
    [Dependency] private readonly IRobustRandom _random = default!;
    [Dependency] private readonly TransformSystem _transform = default!;

    private readonly TimeSpan _updateTimeDelay = TimeSpan.FromMinutes(1);
    private TimeSpan _nextUpdateTime;

    public override void Update(float frameTime)
    {
        base.Update(frameTime);

        // We reduce the frequency of updates to reduce lags
        if (_nextUpdateTime > _timing.CurTime)
            return;

        _nextUpdateTime = _timing.CurTime + _updateTimeDelay;

        // Updating spawns
        var query = EntityQueryEnumerator<ZoneArtifactSpawnerComponent>();
        while (query.MoveNext(out var uid, out var spawner))
        {
            UpdateResumption((uid, spawner));
        }
    }

    public bool Ready(Entity<ZoneArtifactSpawnerComponent> spawner)
    {
        return spawner.Comp.Artifact is not null;
    }

    public bool TrySpawn(Entity<ZoneArtifactSpawnerComponent> spawner)
    {
        if (!Ready(spawner))
            return false;

        Spawn(spawner.Comp.Artifact, _transform.GetMapCoordinates(spawner));
        spawner.Comp.Artifact = null;

        return true;
    }

    private void UpdateResumption(Entity<ZoneArtifactSpawnerComponent> spawner)
    {
        if (Ready(spawner) && !spawner.Comp.RestoreOnReady)
            return;

        if (spawner.Comp.ResumptionTime > _timing.CurTime)
            return;

        Restore(spawner);

        spawner.Comp.ResumptionTime = _timing.CurTime + spawner.Comp.ResumptionDelay;
    }

    private void Restore(Entity<ZoneArtifactSpawnerComponent> spawner)
    {
        // Fucking Linq
        var artifacts = GetPossibleArtifacts(spawner);

        var sumRation = 0f;
        foreach (var artifact in artifacts)
        {
            sumRation += artifact.Ratio;
        }

        var random = _random.NextFloat(0f, sumRation);

        sumRation = 0f;
        foreach (var artifact in artifacts)
        {
            if (random > (sumRation += artifact.Ratio))
                continue;

            SetArtifact(spawner, artifact.PrototypeId);
            break;
        }
    }

    private List<EntityArtifactSpawnEntry> GetPossibleArtifacts(Entity<ZoneArtifactSpawnerComponent> spawner)
    {
        var result = new List<EntityArtifactSpawnEntry>();
        var map = _mapManager.GetMapEntityId(Transform(spawner).MapID);

        if (!TryComp<ZoneArtifactSpawnerMapTierComponent>(map, out var mapTier))
            return spawner.Comp.Artifacts;

        foreach (var artifact in spawner.Comp.Artifacts)
        {
            if (artifact.Tier > mapTier.MaxTier)
                continue;

            if  (artifact.Tier < mapTier.MinTier)
                continue;

            result.Add(artifact);
        }

        return result;
    }

    private void SetArtifact(Entity<ZoneArtifactSpawnerComponent> spawner, EntProtoId? protoId)
    {
        spawner.Comp.Artifact = protoId;

        if (!TryComp<ZoneArtifactDetectorTargetComponent>(spawner, out var spawnerTarget))
            return;

        if (protoId is not { } prototype)
            return;

        if (!_prototype.Index(prototype).TryGetComponent<ZoneArtifactDetectorTargetComponent>(out var target))
            return;

        spawnerTarget.DetectedLevel = target.DetectedLevel;
        spawnerTarget.Detectable = target.Detectable;
    }
}
