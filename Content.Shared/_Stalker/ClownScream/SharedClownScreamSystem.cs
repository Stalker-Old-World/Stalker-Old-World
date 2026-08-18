using Robust.Shared.Audio;
using Robust.Shared.Audio.Components;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Serialization;
using Robust.Shared.Utility;

namespace Content.Shared._Stalker.ClownScream;

public abstract class SharedClownScreamSystem : EntitySystem
{
    [Dependency] private readonly SharedAudioSystem _audio = default!;

    protected void PlaySound(Entity<ClownScreamComponent> entity)
    {
        var comp = entity.Comp;
        var audioParams = new AudioParams().WithVolume(comp.Volume).WithLoop(true);
        var audioEntity = _audio.PlayPvs(comp.ScreamSound, entity.Owner, audioParams);
        if (!audioEntity.HasValue)
            return;
        comp.SoundEntity = audioEntity.Value.Entity;
    }

    protected void StopSound(Entity<ClownScreamComponent> entity)
    {
        var comp = entity.Comp;
        if (comp.SoundEntity == null ||
            !TryComp<AudioComponent>(comp.SoundEntity, out var sound))
            return;

        comp.SoundEntity = _audio.Stop(comp.SoundEntity.Value, sound);
    }
}

[Serializable, NetSerializable]
public sealed class ToggleClownScreamMessage : EntityEventArgs
{
    public NetEntity Entity;
    public SpriteSpecifier Sprite;
    public bool Enable;

    public ToggleClownScreamMessage(NetEntity entity, SpriteSpecifier sprite, bool enable)
    {
        Entity = entity;
        Sprite = sprite;
        Enable = enable;
    }
}
