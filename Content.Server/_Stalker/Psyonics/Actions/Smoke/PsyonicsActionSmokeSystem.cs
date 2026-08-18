using Content.Server.Fluids.EntitySystems;
using Content.Shared._Stalker.Psyonics.Actions;
using Content.Shared._Stalker.Psyonics.Actions.Smoke;
using Content.Shared.Chemistry.Components;

namespace Content.Server._Stalker.Psyonics.Actions.Smoke;

public sealed class PsyonicsActionSmokeSystem : BasePsyonicsActionSystem<PsyonicsActionSmokeComponent, PsyonicsActionSmokeEvent>
{
    [Dependency] private readonly SmokeSystem _smoke = default!;

    protected override void OnAction(Entity<PsyonicsActionSmokeComponent> entity, ref PsyonicsActionSmokeEvent args)
    {
        base.OnAction(entity, ref args);

        var ent = Spawn(entity.Comp.SmokePrototype, args.Target);
        if (!TryComp<SmokeComponent>(ent, out var smoke))
        {
            Log.Error($"Smoke prototype {entity.Comp.SmokePrototype} was missing SmokeComponent");
            Del(ent);

            args.Handled = false;
            return;
        }

        _smoke.StartSmoke(ent, entity.Comp.Solution, (float)entity.Comp.Duration.TotalSeconds, entity.Comp.SpreadAmount, smoke);
        args.Handled = true;
    }
}
