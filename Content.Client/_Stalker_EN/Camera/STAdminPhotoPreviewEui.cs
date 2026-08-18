using System.IO;
using Content.Client.Eui;
using Content.Shared._Stalker_EN.Camera;
using Content.Shared.Eui;
using Robust.Client.Graphics;
using Robust.Shared.Log;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace Content.Client._Stalker_EN.Camera;

/// <summary>
/// Client-side EUI that displays a photo preview for admins.
/// </summary>
public sealed class STAdminPhotoPreviewEui : BaseEui
{
    [Dependency] private readonly IClyde _clyde = default!;
    [Dependency] private readonly ILogManager _log = default!;

    private ISawmill _sawmill = default!;
    private STPhotoWindow? _window;

    /// <inheritdoc />
    public override void Opened()
    {
        _sawmill = _log.GetSawmill("st.camera.eui");
        _window = new STPhotoWindow();
        _window.OpenCentered();
        _window.StartLoading();
        _window.OnClose += () => SendMessage(new CloseEuiMessage());
    }

    /// <inheritdoc />
    public override void HandleState(EuiStateBase state)
    {
        if (state is not STAdminPhotoPreviewEuiState photoState)
            return;

        if (_window == null)
            return;

        if (photoState.ImageData.Length == 0)
        {
            _window.ShowUnavailable();
            return;
        }

        try
        {
            using var stream = new MemoryStream(photoState.ImageData);
            using var image = Image.Load<Rgba32>(stream);
            var texture = _clyde.LoadTextureFromImage(image);
            _window.SetTexture(texture);
        }
        catch (Exception ex)
        {
            _sawmill.Warning($"Failed to load admin photo preview image: {ex.Message}");
            _window.ShowUnavailable();
        }
    }

    /// <inheritdoc />
    public override void Closed()
    {
        _window?.Close();
        _window?.Dispose();
        _window = null;
    }
}
