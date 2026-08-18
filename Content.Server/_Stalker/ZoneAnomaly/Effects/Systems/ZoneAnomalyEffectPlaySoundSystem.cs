using Content.Shared._Stalker.ZoneAnomaly.Components;
using Content.Shared._Stalker.ZoneAnomaly.Effects.Components;
using Robust.Server.Audio;
using Robust.Shared.Audio;

namespace Content.Server._Stalker.ZoneAnomaly.Effects.Systems;

public sealed class ZoneAnomalyEffectPlaySoundSystem : EntitySystem
{
    [Dependency] private readonly AudioSystem _audio = default!;

    public override void Initialize()
    {
        SubscribeLocalEvent<ZoneAnomalyEffectPlaySoundComponent, ZoneAnomalyActivateEvent>(OnActivate);
    }

    private void OnActivate(Entity<ZoneAnomalyEffectPlaySoundComponent> effect, ref ZoneAnomalyActivateEvent args)
    {
        _audio.PlayPredicted(effect.Comp.Sound, Transform(effect).Coordinates, effect, new AudioParams()
        {
            MaxDistance = effect.Comp.Range,
            Volume = effect.Comp.Volume,
        });
    }
}
