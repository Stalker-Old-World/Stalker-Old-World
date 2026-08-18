using Content.Shared._Stalker.Dizzy;
using Robust.Client.Graphics;
using Robust.Client.Player;
using Robust.Shared.Player;

namespace Content.Client._Stalker.Dizzy;

public sealed class DizzySystem : SharedDizzySystem
{
    [Dependency] private readonly IPlayerManager _player = default!;
    [Dependency] private readonly IOverlayManager _overlayMan = default!;

    private DizzyOverlay _overlay = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<DizzyComponent, ComponentInit>(OnDizzyInit);
        SubscribeLocalEvent<DizzyComponent, ComponentShutdown>(OnDizzyShutdown);

        SubscribeLocalEvent<DizzyComponent, LocalPlayerAttachedEvent>(OnPlayerAttached);
        SubscribeLocalEvent<DizzyComponent, LocalPlayerDetachedEvent>(OnPlayerDetached);

        _overlay = new();
    }

    private void OnPlayerAttached(EntityUid uid, DizzyComponent component, LocalPlayerAttachedEvent args)
    {
        _overlayMan.AddOverlay(_overlay);
    }

    private void OnPlayerDetached(EntityUid uid, DizzyComponent component, LocalPlayerDetachedEvent args)
    {
        _overlay.Stop();
        _overlayMan.RemoveOverlay(_overlay);
    }

    private void OnDizzyInit(EntityUid uid, DizzyComponent component, ComponentInit args)
    {
        if (_player.LocalEntity == uid)
            _overlayMan.AddOverlay(_overlay);
    }

    private void OnDizzyShutdown(EntityUid uid, DizzyComponent component, ComponentShutdown args)
    {
        if (_player.LocalEntity == uid)
        {
            _overlay.Stop();
            _overlayMan.RemoveOverlay(_overlay);
        }
    }
}
