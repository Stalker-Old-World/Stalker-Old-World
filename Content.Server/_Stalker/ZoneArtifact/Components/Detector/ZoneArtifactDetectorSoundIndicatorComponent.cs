using Robust.Shared.Audio;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom;

namespace Content.Server._Stalker.ZoneArtifact.Components.Detector;

[RegisterComponent]
public sealed partial class ZoneArtifactDetectorSoundIndicatorComponent : Component
{
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public SoundSpecifier Sound = new SoundPathSpecifier("/Audio/Items/locator_beep.ogg");

    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public TimeSpan MaxInterval = TimeSpan.FromSeconds(2.5f);

    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public TimeSpan MinInterval = TimeSpan.FromSeconds(0.05f);

    [DataField(customTypeSerializer: typeof(TimeOffsetSerializer)), ViewVariables(VVAccess.ReadWrite)]
    public TimeSpan NextTime;
}
