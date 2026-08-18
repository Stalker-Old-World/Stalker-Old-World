namespace Content.Server._Stalker.TrashSerchable;

public sealed class TrashSerchableSystem : EntitySystem
{
    public override void Update(float frameTime)
    {
        base.Update(frameTime);
        var query = EntityQueryEnumerator<TrashSerchableComponent>();
        while (query.MoveNext(out var uid, out var resist))
        {
            resist.TimeBeforeNextSearch -= frameTime;
        }
    }
}
