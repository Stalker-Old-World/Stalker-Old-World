using Robust.Shared.Audio;
using Robust.Shared.Utility;

namespace Content.Shared._Stalker.ClownScream;

[RegisterComponent]
public sealed partial class ClownScreamComponent : Component
{
    [DataField("sound")]
    public SoundSpecifier ScreamSound = default!;

    [DataField]
    public float Volume = 5f;

    [DataField("texture")]
    public SpriteSpecifier Sprite = default!;

    [DataField]
    public string Slot = default!;

    public EntityUid? SoundEntity;
}
