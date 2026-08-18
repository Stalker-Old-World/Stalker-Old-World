using Content.Shared._Stalker.ZoneAnomaly.Components;
using Content.Shared._Stalker_EN.Devices.Radar;
using Content.Shared._Stalker_EN.Devices.Radar.Components;
using Robust.Server.GameObjects;
using Robust.Shared.Map.Components;

namespace Content.Server._Stalker_EN.Devices.Radar.EntitySystems;

/// <summary>
/// System that provides anomaly targets to radar displays.
/// Listens for RadarTargetSourceUpdateEvent and adds anomaly blips.
/// </summary>
public sealed class AnomalyRadarTargetSourceSystem : EntitySystem
{
    [Dependency] private readonly EntityLookupSystem _entityLookup = default!;
    [Dependency] private readonly SharedMapSystem _map = default!;
    [Dependency] private readonly TransformSystem _transform = default!;

    private readonly HashSet<Entity<ZoneAnomalyComponent>> _anomalyBuffer = new();

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<AnomalyRadarTargetSourceComponent, RadarTargetSourceUpdateEvent>(OnRadarUpdate);
    }

    private void OnRadarUpdate(Entity<AnomalyRadarTargetSourceComponent> entity, ref RadarTargetSourceUpdateEvent args)
    {
        var xformQuery = GetEntityQuery<TransformComponent>();

        // Get the grid the user is on for consistent angle calculation
        MapGridComponent? userGrid = null;
        if (args.UserGridUid != null)
            TryComp(args.UserGridUid.Value, out userGrid);

        _anomalyBuffer.Clear();
        _entityLookup.GetEntitiesInRange(args.UserMapCoords, entity.Comp.DetectionRange, _anomalyBuffer, LookupFlags.Uncontained);

        foreach (var target in _anomalyBuffer)
        {
            if (!target.Comp.Detected)
                continue;

            if (target.Comp.DetectedLevel > entity.Comp.Level)
                continue;

            var targetXform = xformQuery.GetComponent(target);
            var targetWorldPos = _transform.GetWorldPosition(targetXform, xformQuery);

            var diff = targetWorldPos - args.UserWorldPos;
            var distance = diff.Length();

            if (distance > entity.Comp.DetectionRange)
                continue;

            var radarAngle = RadarAngleHelper.CalculateRadarAngle(
                _map, args.UserGridUid, userGrid, args.UserWorldPos, targetWorldPos);

            args.Blips.Add(new RadarBlip(
                GetNetEntity(target),
                radarAngle,
                distance,
                target.Comp.DetectedLevel,
                RadarBlipType.Anomaly));
        }
    }
}
