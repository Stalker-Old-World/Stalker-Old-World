using Robust.Shared.Serialization;

namespace Content.Shared._Stalker.ZoneArtifact;

[Serializable, NetSerializable]
public enum ZoneArtifactDetectorVisuals : byte
{
    Enabled,
    Disabled,
    Detected,
    Layer,
}

[Serializable, NetSerializable]
public enum ZoneArtifactDetectorAngleIndicatorVisuals : byte
{
    Layer,
}
