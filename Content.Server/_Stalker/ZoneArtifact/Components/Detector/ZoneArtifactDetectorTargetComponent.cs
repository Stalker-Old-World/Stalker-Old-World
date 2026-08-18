namespace Content.Server._Stalker.ZoneArtifact.Components.Detector;

[RegisterComponent]
public sealed partial class ZoneArtifactDetectorTargetComponent : Component
{
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public bool Detectable = true;

    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public int DetectedLevel = 0;
}
