namespace Content.Shared._Stalker.ZoneArtifact.Events;

public sealed class ZoneArtifactDetectorUpdateEvent : EntityEventArgs
{
    public EntityUid? Detector;
    public EntityUid? Detected;
    public float? Distance;

    public ZoneArtifactDetectorUpdateEvent(EntityUid detector, EntityUid? detected = null, float? distance = null)
    {
        Detector = detector;
        Detected = detected;
        Distance = distance;
    }
}
