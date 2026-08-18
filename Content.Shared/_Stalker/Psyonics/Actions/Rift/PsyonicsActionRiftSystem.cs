using Content.Shared._Stalker.Psyonics.Actions.Springboard;
using Content.Shared.Interaction;
using Content.Shared.Physics;
using Content.Shared.Stunnable;

namespace Content.Shared._Stalker.Psyonics.Actions.Rift;

public sealed class PsyonicsActionRiftSystem : BasePsyonicsActionSystem<PsyonicsActionRiftComponent, PsyonicsActionRiftEvent>
{
    [Dependency] private readonly SharedTransformSystem _transform = default!;
    [Dependency] private readonly SharedInteractionSystem _interaction = default!;
    [Dependency] private readonly SharedStunSystem _stun = default!;

    protected override void OnAction(Entity<PsyonicsActionRiftComponent> entity, ref PsyonicsActionRiftEvent args)
    {
        base.OnAction(entity, ref args);

        var origin = _transform.GetMapCoordinates(entity);
        var target = args.Target.ToMap(EntityManager, _transform);

        if (!_interaction.InRangeUnobstructed(origin, target, 0f, CollisionGroup.Opaque, uid => uid == entity.Owner))
        {
            // TODO: Popup
            args.Handled = false;
            return;
        }

        _transform.SetCoordinates(entity, args.Target);
        _transform.AttachToGridOrMap(entity);

        args.Handled = true;
    }
}
