using Content.Server.Polymorph.Systems;
using Content.Shared._Stalker.ZoneAnomaly.Components;
using Content.Shared._Stalker.ZoneAnomaly.Effects.Components;
using Content.Shared.Mobs.Components;
using Robust.Shared.Random;

namespace Content.Server._Stalker.ZoneAnomaly.Effects.Systems;

public sealed class ZoneAnomalyEffectPolymorphSystem : EntitySystem
{
    [Dependency] private readonly IRobustRandom _random = default!;
    [Dependency] private readonly PolymorphSystem _polymorph = default!;

    public override void Initialize()
    {
        SubscribeLocalEvent<ZoneAnomalyEffectPolymorphComponent, ZoneAnomalyActivateEvent>(OnActivate);
    }

    private void OnActivate(Entity<ZoneAnomalyEffectPolymorphComponent> effect, ref ZoneAnomalyActivateEvent args)
    {
        foreach (var trigger in args.Triggers)
        {
            if (!HasComp<MobStateComponent>(trigger))
                continue;

            _polymorph.PolymorphEntity(trigger, _random.Pick(effect.Comp.Polymorph));
        }
    }
}
