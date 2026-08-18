using Robust.Shared.Audio;

namespace Content.Server._Stalker.SinLightPoint;

[RegisterComponent]
public sealed partial class SinLightPointComponent : Component
{
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public string Side = "nothing";

    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public SoundSpecifier Sound = new SoundPathSpecifier("/Audio/Effects/radpulse1.ogg");
}
