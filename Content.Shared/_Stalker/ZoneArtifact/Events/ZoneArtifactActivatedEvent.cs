namespace Content.Shared._Stalker.ZoneArtifact.Events;

public sealed class ZoneArtifactActivatedEvent : EntityEventArgs
{
    public EntityUid? Activator;

    public ZoneArtifactActivatedEvent(EntityUid activator)
    {
        Activator = activator;
    }
}
