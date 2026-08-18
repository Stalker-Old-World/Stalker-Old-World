using Robust.Shared.Audio;
using Robust.Shared.Audio.Components;

namespace Content.Shared._Stalker.Ambient;

[RegisterComponent, AutoGenerateComponentPause]
public sealed partial class StalkerAmbientComponent : Component
{
    [DataField]
    public List<SoundSpecifier> Sounds;

    [DataField]
    public float Volume;

    [DataField]
    public float Probability;

    [DataField]
    public float Cooldown;

    public (EntityUid uid, AudioComponent comp)? CurrentlyPlaying;

    [AutoPausedField]
    public TimeSpan? CooldownTime;
}
