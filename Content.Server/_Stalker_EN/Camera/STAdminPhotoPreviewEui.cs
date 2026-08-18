using Content.Server.EUI;
using Content.Shared._Stalker_EN.Camera;
using Content.Shared.Eui;

namespace Content.Server._Stalker_EN.Camera;

/// <summary>
/// Server-side EUI that sends photo bytes to an admin's client for preview.
/// </summary>
public sealed class STAdminPhotoPreviewEui : BaseEui
{
    private readonly Guid _photoId;
    private readonly byte[] _imageData;

    public STAdminPhotoPreviewEui(Guid photoId, byte[] imageData)
    {
        _photoId = photoId;
        _imageData = imageData;
    }

    public override void Opened()
    {
        StateDirty();
    }

    public override EuiStateBase GetNewState()
    {
        return new STAdminPhotoPreviewEuiState(_photoId, _imageData);
    }
}
