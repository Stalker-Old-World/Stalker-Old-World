using Robust.Shared.Serialization;

namespace Content.Shared._Stalker_EN.Camera;

/// <summary>
/// Visual effect applied to photos taken by a camera.
/// Different camera items produce different effects.
/// </summary>
[Serializable, NetSerializable]
public enum STPhotoEffect : byte
{
    None,
    Polaroid,
    Glitch,
}
