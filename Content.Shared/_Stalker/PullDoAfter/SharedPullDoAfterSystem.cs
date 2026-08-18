using Content.Shared.DoAfter;
using Robust.Shared.Containers;

namespace Content.Shared._Stalker.PullDoAfter;
//TODO: REMOVE THIS SHIT (or refactor...)
public sealed class SharedPullDoAfterSystem : EntitySystem
{
    [Dependency] private readonly SharedDoAfterSystem _doAfter = default!;

    public void StartRemoveDoAfter(Entity<PullDoAfterComponent> used, EntityUid user, EntityUid container)
    {
        var args = new DoAfterArgs(EntityManager,
            user, used.Comp.PullTime,
            new PullDoAfterEvent(GetNetEntity(container)), container, used, used)
        {
            BlockDuplicate = true,
            BreakOnHandChange = true,
            Hidden = used.Comp.Hidden,
            NeedHand = true
        };
        _doAfter.TryStartDoAfter(args);
    }

    public void StartInteractDoAfter(Entity<PullDoAfterComponent> used, EntityUid user, EntityUid container)
    {
        var args = new DoAfterArgs(EntityManager,
            user, used.Comp.PullTime,
            new PullDoAfterEvent(GetNetEntity(container), true), container, used, used)
        {
            BlockDuplicate = true,
            BreakOnHandChange = true,
            Hidden = used.Comp.Hidden,
            NeedHand = true
        };
        _doAfter.TryStartDoAfter(args);
    }
}
