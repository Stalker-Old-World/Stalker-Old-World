using Content.Server._Stalker.ZoneArtifact.Components.Detector;
using Content.Shared._Stalker.ZoneArtifact.Events;
using Content.Shared.Examine;
using ZoneArtifactDetectorComponent = Content.Shared._Stalker.ZoneArtifact.Components.ZoneArtifactDetectorComponent;
using ZoneArtifactDetectorDistanceIndicatorComponent = Content.Shared._Stalker.ZoneArtifact.Components.ZoneArtifactDetectorDistanceIndicatorComponent;

namespace Content.Server._Stalker.ZoneArtifact.Systems.Detector;

public sealed class ZoneArtifactDetectorDistanceIndicatorSystem : EntitySystem
{
    [Dependency] private readonly ZoneArtifactDetectorSystem _artifactDetector = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<ZoneArtifactDetectorDistanceIndicatorComponent, ZoneArtifactDetectorUpdateEvent>(OnUpdate);
        SubscribeLocalEvent<ZoneArtifactDetectorDistanceIndicatorComponent, ExaminedEvent>(OnExamine);
    }

    private void OnUpdate(Entity<ZoneArtifactDetectorDistanceIndicatorComponent> indicator, ref ZoneArtifactDetectorUpdateEvent args)
    {
        if (!TryComp<ZoneArtifactDetectorComponent>(indicator, out var detector))
            return;

        indicator.Comp.Enabled = _artifactDetector.Enabled((indicator, detector));
        indicator.Comp.Distance = args.Distance;

        Dirty(indicator);
    }

    private void OnExamine(Entity<ZoneArtifactDetectorDistanceIndicatorComponent> indicator, ref ExaminedEvent args)
    {
        if (!TryComp<ZoneArtifactDetectorComponent>(indicator, out var detector))
            return;

        if (!_artifactDetector.Enabled((indicator, detector)))
            return;

        if (detector.ClosestDistance is not { } distance)
            return;

        var msg = Loc.GetString("distance-indicator-component-examine", ("distance", distance.ToString("N1")));
        args.PushMarkup(msg);
    }
}
