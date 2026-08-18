namespace Content.Shared._Stalker.ZoneArtifact.Events;

public sealed class ZoneArtifactDetectorSetTargetEvent : EntityEventArgs
{
    public EntityUid? Detector;
    public EntityUid? Target;

    public ZoneArtifactDetectorSetTargetEvent(EntityUid detector, EntityUid? target = null)
    {
        Detector = detector;
        Target = target;
    }
}
