using Robust.Shared.Audio;

namespace Content.Shared._Stalker.ZoneAnomaly.Effects.Components;

[RegisterComponent]
public sealed partial class ZoneAnomalyEffectPlaySoundComponent : Component
{
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public SoundSpecifier Sound = new SoundPathSpecifier("/Audio/_Stalker/Effects/springboard_blowout.ogg");

    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public float Range = 10f;

    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public float Volume;
}
