using Content.Server._Stalker.ZoneArtifact.Components.Detector;
using Content.Shared._Stalker.ZoneArtifact;
using Content.Shared._Stalker.ZoneArtifact.Components;
using Robust.Server.GameObjects;

namespace Content.Server._Stalker.ZoneArtifact.Systems.Detector;

public sealed class ZoneArtifactDetectorAngleIndicatorSystem : EntitySystem
{
    [Dependency] private readonly AppearanceSystem _appearance = default!;
    [Dependency] private readonly ZoneArtifactDetectorSystem _detector = default!;
    [Dependency] private readonly TransformSystem _transform = default!;

    public override void Update(float frameTime)
    {
        base.Update(frameTime);

        var query = EntityQueryEnumerator<ZoneArtifactDetectorAngleIndicatorComponent, ZoneArtifactDetectorComponent>();
        while (query.MoveNext(out var uid, out var indicator, out var detector))
        {
            if (!_detector.Enabled((uid, detector)))
            {
                _appearance.SetData(uid, ZoneArtifactDetectorAngleIndicatorVisuals.Layer, -1);
                continue;
            }

            if (detector.ClosestEntity is not { } entity || detector.ClosestDistance is not { } distance)
            {
                _appearance.SetData(uid, ZoneArtifactDetectorAngleIndicatorVisuals.Layer, 9);
                continue;
            }

            if (distance <= indicator.CenterDistance)
            {
                _appearance.SetData(uid, ZoneArtifactDetectorAngleIndicatorVisuals.Layer, 8);
                continue;
            }

            var direction = _transform.GetWorldPosition(entity) - _transform.GetWorldPosition(uid);
            _appearance.SetData(uid, ZoneArtifactDetectorAngleIndicatorVisuals.Layer, (int) direction.ToAngle().GetDir());
        }
    }
}
