using Content.Server._Stalker.ZoneArtifact.Components.Detector;
using Content.Server._Stalker.ZoneArtifact.Components.Spawner;
using Content.Server._Stalker.ZoneArtifact.Systems;
using Content.Shared._Stalker.ZoneAnomaly.Components;
using Content.Shared._Stalker_EN.Devices.Radar;
using Content.Shared._Stalker_EN.Devices.Radar.Components;
using Robust.Server.GameObjects;
using Robust.Shared.Map.Components;

namespace Content.Server._Stalker_EN.Devices.Radar.EntitySystems;

/// <summary>
/// System that provides artifact targets to radar displays.
/// Listens for RadarTargetSourceUpdateEvent and adds artifact blips.
/// </summary>
public sealed class ArtifactRadarTargetSourceSystem : EntitySystem
{
    [Dependency] private readonly EntityLookupSystem _entityLookup = default!;
    [Dependency] private readonly SharedMapSystem _map = default!;
    [Dependency] private readonly TransformSystem _transform = default!;
    [Dependency] private readonly ZoneArtifactSpawnerSystem _artifactSpawner = default!;

    private readonly HashSet<Entity<ZoneArtifactDetectorTargetComponent>> _artifactBuffer = new();
    private EntityQuery<ZoneAnomalyComponent> _anomalyQuery;

    public override void Initialize()
    {
        base.Initialize();

        _anomalyQuery = GetEntityQuery<ZoneAnomalyComponent>();
        SubscribeLocalEvent<ArtifactRadarTargetSourceComponent, RadarTargetSourceUpdateEvent>(OnRadarUpdate);
    }

    private void OnRadarUpdate(Entity<ArtifactRadarTargetSourceComponent> entity, ref RadarTargetSourceUpdateEvent args)
    {
        var xformQuery = GetEntityQuery<TransformComponent>();

        // Get the grid the user is on for consistent angle calculation
        MapGridComponent? userGrid = null;
        if (args.UserGridUid != null)
            TryComp(args.UserGridUid.Value, out userGrid);

        _artifactBuffer.Clear();
        _entityLookup.GetEntitiesInRange(args.UserMapCoords, entity.Comp.DetectionRange, _artifactBuffer, LookupFlags.Uncontained);

        foreach (var target in _artifactBuffer)
        {
            if (!target.Comp.Detectable)
                continue;

            if (target.Comp.DetectedLevel > entity.Comp.Level)
                continue;

            var targetXform = xformQuery.GetComponent(target);
            var targetWorldPos = _transform.GetWorldPosition(targetXform, xformQuery);
            var distance = (targetWorldPos - args.UserWorldPos).Length();

            if (distance > entity.Comp.DetectionRange)
                continue;

            // Process spawner activation for ALL targets (including anomalies)
            if (TryComp<ZoneArtifactSpawnerComponent>(target, out var spawner))
            {
                if (!_artifactSpawner.Ready((target, spawner)))
                    continue; // No artifact ready — skip

                if (distance <= entity.Comp.ActivationDistance)
                {
                    _artifactSpawner.TrySpawn((target, spawner));
                    continue; // Spawned — actual artifact appears next update
                }
                // Ready spawner: falls through to add blip (even if it's an anomaly)
            }
            else if (_anomalyQuery.HasComponent(target))
            {
                // Non-spawner anomaly — skip from artifact blips
                // These show via AnomalyRadarTargetSourceSystem instead
                continue;
            }

            var radarAngle = RadarAngleHelper.CalculateRadarAngle(
                _map, args.UserGridUid, userGrid, args.UserWorldPos, targetWorldPos);

            args.Blips.Add(new RadarBlip(
                GetNetEntity(target),
                radarAngle,
                distance,
                target.Comp.DetectedLevel,
                RadarBlipType.Artifact));
        }
    }
}
