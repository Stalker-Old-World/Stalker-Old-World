using System.Numerics;

namespace Content.Shared._Stalker.Throwing;

[ByRefEvent]
public struct STBeforeThrowedEvent(EntityUid target, EntityUid user, Vector2 direction, float strength)
{
    public readonly EntityUid Target = target;
    public readonly EntityUid User = user;

    public Vector2 Direction = direction;
    public float Strength = strength;

    public bool Cancelled;
}
