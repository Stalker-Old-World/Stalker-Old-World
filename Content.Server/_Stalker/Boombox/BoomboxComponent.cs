using Content.Shared.Containers.ItemSlots;
using Robust.Shared.Audio;
using Robust.Shared.Audio.Components;

namespace Content.Server._Stalker.Boombox;

[RegisterComponent]
public sealed partial class BoomboxComponent : Component
{

    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public float Volume = -7f;

    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public float MaxDistance = 7f;

    [DataField, ViewVariables(VVAccess.ReadOnly)]
    public (EntityUid, AudioComponent)? CurrentPlaying;

    [DataField, ViewVariables(VVAccess.ReadOnly)]
    public TimeSpan SoundTime = TimeSpan.Zero;

    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public bool RepeatOn = false;
    public TimeSpan SoundEnd = TimeSpan.Zero;
}
