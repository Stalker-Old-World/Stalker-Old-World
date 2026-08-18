using Robust.Shared.Audio;

namespace Content.Server._Stalker.Boombox.Cassetes;
[RegisterComponent]
public sealed partial class CasseteComponent : Component
{
    [DataField("sound")]
    public SoundSpecifier Music;
}
