using Content.Shared.Eui;
using Robust.Shared.Serialization;

namespace Content.Shared._Stalker_EN.Camera;

/// <summary>
/// EUI state carrying photo bytes for admin preview.
/// </summary>
[Serializable, NetSerializable]
public sealed class STAdminPhotoPreviewEuiState : EuiStateBase
{
    public Guid PhotoId { get; }
    public byte[] ImageData { get; }

    public STAdminPhotoPreviewEuiState(Guid photoId, byte[] imageData)
    {
        PhotoId = photoId;
        ImageData = imageData;
    }
}
