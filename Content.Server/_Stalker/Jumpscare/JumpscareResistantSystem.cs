using Content.Shared._Stalker.Jumpscare;

namespace Content.Server._Stalker.Jumpscare;

public sealed class JumpscareResistantSystem : EntitySystem
{
    public override void Update(float frameTime)
    {
        base.Update(frameTime);
        var query = EntityQueryEnumerator<JumpscareResistantComponent>();
        while (query.MoveNext(out var uid, out var resist))
        {
            resist.TimeBeforeRemove -= frameTime;
            if (resist.TimeBeforeRemove <= 0)
            {
                RemComp<JumpscareResistantComponent>(uid);
            }
        }
    }
}
