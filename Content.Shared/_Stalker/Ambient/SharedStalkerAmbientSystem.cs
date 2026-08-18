using Robust.Shared.Audio.Systems;
using Robust.Shared.Map.Components;
using Robust.Shared.Network;
using Robust.Shared.Player;
using Robust.Shared.Random;
using Robust.Shared.Timing;

namespace Content.Shared._Stalker.Ambient;
//TODO: Сделать проверку на повторение звука ибо неприятно ушкам.
public sealed class SharedStalkerAmbientSystem : EntitySystem
{
    [Dependency] private readonly IRobustRandom _random = default!;
    [Dependency] private readonly IGameTiming _timing = default!;
    [Dependency] private readonly SharedAudioSystem _audio = default!;
    [Dependency] private readonly INetManager _netMan = default!;
    private TimeSpan _updateTime = TimeSpan.FromSeconds(1);

    public override void Update(float frameTime)
    {
        if (_netMan.IsClient)
            return;
        base.Update(frameTime);

        if (_updateTime > _timing.CurTime)
            return;
        _updateTime = _timing.CurTime + TimeSpan.FromSeconds(1);
        var query = EntityQueryEnumerator<StalkerAmbientComponent>();
        while (query.MoveNext(out var uid, out var ambient))
        {
            if (!TryComp<MapComponent>(uid, out var mapComp))
                continue;

            if (ambient.CooldownTime > _timing.CurTime ||
                ambient.CurrentlyPlaying is { comp.Playing: true })
                continue;

            if (!_random.Prob(ambient.Probability))
                continue;

            var mapId = mapComp.MapId;
            var soundToPlay = _random.Pick(ambient.Sounds);

            ambient.CooldownTime = _timing.CurTime + _audio.GetAudioLength(_audio.GetSound(soundToPlay)) + TimeSpan.FromSeconds(ambient.Cooldown);

            // Wizards' cringe
            soundToPlay.Params = soundToPlay.Params.WithVolume(ambient.Volume);
            // Play sound to all players who are currently on this map
            ambient.CurrentlyPlaying = _audio.PlayGlobal(soundToPlay, Filter.BroadcastMap(mapId), false);
        }
    }
}
