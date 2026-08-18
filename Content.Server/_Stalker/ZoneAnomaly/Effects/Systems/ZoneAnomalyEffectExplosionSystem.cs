using Content.Server.Explosion.EntitySystems;
using Content.Shared._Stalker.ZoneAnomaly.Components;
using Content.Shared._Stalker.ZoneAnomaly.Effects.Components;

namespace Content.Server._Stalker.ZoneAnomaly.Effects.Systems;

public sealed class ZoneAnomalyEffectExplosionSystem : EntitySystem
{
    [Dependency] private readonly ExplosionSystem _explosion = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<ZoneAnomalyEffectExplosionComponent, ZoneAnomalyActivateEvent>(OnActivate);
    }

    private void OnActivate(Entity<ZoneAnomalyEffectExplosionComponent> effect, ref ZoneAnomalyActivateEvent args)
    {
        _explosion.QueueExplosion(effect, effect.Comp.ProtoId, effect.Comp.TotalIntensity, effect.Comp.Slope, effect.Comp.MaxTileIntensity, canCreateVacuum: false, addLog: false);
    }
}
