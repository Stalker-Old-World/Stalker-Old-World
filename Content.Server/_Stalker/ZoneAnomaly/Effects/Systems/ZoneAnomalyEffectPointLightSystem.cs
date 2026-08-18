using Content.Shared._Stalker.ZoneAnomaly;
using Content.Shared._Stalker.ZoneAnomaly.Components;
using Content.Shared._Stalker.ZoneAnomaly.Effects.Components;
using Robust.Server.GameObjects;

namespace Content.Server._Stalker.ZoneAnomaly.Effects.Systems;

public sealed class ZoneAnomalyEffectPointLightSystem : EntitySystem
{
    [Dependency] private readonly PointLightSystem _pointLight = default!;

    public override void Initialize()
    {
        SubscribeLocalEvent<ZoneAnomalyEffectPointLightComponent, ZoneAnomalyChangedState>(OnChangeState);
    }

    private void OnChangeState(Entity<ZoneAnomalyEffectPointLightComponent> effect, ref ZoneAnomalyChangedState args)
    {
        _pointLight.SetEnabled(effect, args.Current == ZoneAnomalyState.Activated);
    }
}
