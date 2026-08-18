using Content.Shared.Doors.Systems;
using Content.Shared.Interaction;
using Content.Shared.Doors.Components;
using Content.Server.CkeyAccess.Components;
using Robust.Shared.Player;

namespace Content.Server.CkeyAccessSystem;

public sealed class CkeyAccessSystem : EntitySystem
{
    [Dependency] private readonly SharedDoorSystem _door = default!;

    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<CkeyAccessComponent, BeforeRangedInteractEvent>(OnUseInHand);
    }

    public override void Update(float frameTime)
    {
        base.Update(frameTime);

    }

    public void OnUseInHand(EntityUid uid, CkeyAccessComponent comp, BeforeRangedInteractEvent args)
    {
        if (!args.CanReach)
            return;
        OnUse(args.Target, args.User);
    }

    public void OnUse(EntityUid? target, EntityUid user)
    {
        if (target == null)
            return;
        if (TryComp<DoorComponent>(target, out var doorcomp) && doorcomp != null && TryComp<CkeyAccessComponent>(target, out var acces) && acces != null)
        {
            var door = target.Value;
            if (TryComp<ActorComponent>(user, out var actor))
            {
                if (actor.PlayerSession.Name == acces.OwnerName)
                {
                    if (doorcomp.State == DoorState.Open)
                    {
                        _door.StartClosing(door, doorcomp, user, false);
                    }
                    if (doorcomp.State == DoorState.Closed)
                    {
                        _door.StartOpening(door, doorcomp, user, false);
                    }

                }

            }
        }
    }

}
