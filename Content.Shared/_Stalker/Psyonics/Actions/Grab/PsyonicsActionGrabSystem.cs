using Content.Shared.Hands.EntitySystems;
using Content.Shared.Item;

namespace Content.Shared._Stalker.Psyonics.Actions.Grab;

public sealed class PsyonicsActionGrabSystem : BasePsyonicsActionSystem<PsyonicsActionGrabComponent, PsyonicsActionGrabEvent>
{
    [Dependency] private readonly SharedHandsSystem _hands = default!;

    protected override void OnAction(Entity<PsyonicsActionGrabComponent> entity, ref PsyonicsActionGrabEvent args)
    {
        base.OnAction(entity, ref args);

        if (!HasComp<ItemComponent>(args.Target))
        {
            args.Handled = false;
            return;
        }

        if (!_hands.TryPickupAnyHand(entity, args.Target))
        {
            args.Handled = false;
            return;
        }

        args.Handled = true;
    }
}
