using Content.Shared._Stalker_EN.Camera;
using JetBrains.Annotations;
using Robust.Client.UserInterface;

namespace Content.Client._Stalker_EN.Camera;

/// <summary>
/// BoundUserInterface for the photo viewer window.
/// </summary>
[UsedImplicitly]
public sealed class STPhotoBoundUserInterface : BoundUserInterface
{
    [Dependency] private readonly IEntityManager _entManager = default!;

    private STPhotoWindow? _window;
    private STPhotoSystem? _photoSystem;
    private Guid _photoId;

    public STPhotoBoundUserInterface(EntityUid owner, Enum uiKey) : base(owner, uiKey)
    {
        IoCManager.InjectDependencies(this);
    }

    protected override void Open()
    {
        base.Open();

        _photoSystem = _entManager.System<STPhotoSystem>();
        _photoSystem.PhotoReceived += OnPhotoReceived;

        _window = new STPhotoWindow();
        _window.OnClose += Close;
    }

    protected override void UpdateState(BoundUserInterfaceState state)
    {
        base.UpdateState(state);

        if (state is not STPhotoBoundUiState photoState)
            return;

        _photoId = photoState.PhotoId;

        if (_photoSystem != null && _photoSystem.TryGetCachedTexture(photoState.PhotoId, out var texture))
        {
            _window?.SetTexture(texture);
        }
        else if (_photoSystem != null)
        {
            var netEntity = _entManager.GetNetEntity(Owner);
            _photoSystem.RequestPhoto(netEntity, photoState.PhotoId);
            _window?.StartLoading();
        }

        // Show window after first state update
        if (_window is { IsOpen: false })
        {
            _window.OpenCentered();
        }
    }

    private void OnPhotoReceived(Guid photoId)
    {
        if (photoId != _photoId)
            return;

        if (_photoSystem != null && _photoSystem.TryGetCachedTexture(photoId, out var texture))
        {
            _window?.SetTexture(texture);
        }
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);

        if (!disposing)
            return;

        if (_photoSystem != null)
            _photoSystem.PhotoReceived -= OnPhotoReceived;

        _window?.Close();
    }
}
