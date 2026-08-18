using Content.Shared._Stalker.ZoneAnomaly.Components;
using Content.Shared._Stalker.ZoneAnomaly.Effects.Components;
using Content.Shared.Throwing;
using Content.Shared.Whitelist;
using Robust.Server.GameObjects;

namespace Content.Server._Stalker.ZoneAnomaly.Effects.Systems;

public sealed class ZoneAnomalyEffectThrowSystem : EntitySystem
{
    [Dependency] private readonly ThrowingSystem _throwing = default!;
    [Dependency] private readonly TransformSystem _transform = default!;
    [Dependency] private readonly EntityWhitelistSystem _whitelist = default!;

    public override void Initialize()
    {
        SubscribeLocalEvent<ZoneAnomalyEffectThrowComponent, ZoneAnomalyActivateEvent>(OnActivate);
    }

    private void OnActivate(Entity<ZoneAnomalyEffectThrowComponent> effect, ref ZoneAnomalyActivateEvent args)
    {
        if (!TryComp<ZoneAnomalyComponent>(effect, out var anomaly))
            return;

        foreach (var entity in anomaly.InAnomaly)
        {
            if (effect.Comp.Whitelist is { } whitelist && _whitelist.IsWhitelistFail(whitelist, entity))
                continue;

            var direction = _transform.GetWorldPosition(entity) - _transform.GetWorldPosition(effect);
            if (direction.Length() < effect.Comp.MinDistance)
                continue;

            _throwing.TryThrow(entity, direction * effect.Comp.Distance, effect.Comp.Force, effect, 0);
        }
    }
}
