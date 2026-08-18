using Content.Shared._Stalker.Dizzy;
using Content.Shared.StatusEffect;
using Robust.Client.Graphics;
using Robust.Client.Player;
using Robust.Shared.Enums;
using Robust.Shared.Prototypes;
using Robust.Shared.Timing;

namespace Content.Client._Stalker.Dizzy;

public sealed class DizzyOverlay : Overlay
{
    [Dependency] private readonly IEntityManager _entityManager = default!;
    [Dependency] private readonly IPrototypeManager _prototypeManager = default!;
    [Dependency] private readonly IPlayerManager _playerManager = default!;
    [Dependency] private readonly IGameTiming _timing = default!;

    public override OverlaySpace Space => OverlaySpace.WorldSpace;
    public override bool RequestScreenTexture => true;
    private readonly ShaderInstance _dizzyShader;

    private bool _isActive;

    public DizzyOverlay()
    {
        IoCManager.InjectDependencies(this);
        _dizzyShader = _prototypeManager.Index<ShaderPrototype>("STDizzy").InstanceUnique();
    }

    public void Stop()
    {
        _isActive = false;
    }

    protected override void FrameUpdate(FrameEventArgs args)
    {
        var playerEntity = _playerManager.LocalEntity;

        if (playerEntity == null || !_entityManager.TryGetComponent<StatusEffectsComponent>(playerEntity, out var status))
        {
            _isActive = false;
            return;
        }

        if (_entityManager.TryGetComponent<DizzyComponent>(playerEntity, out var dizzyComp))
        {
            _isActive = true;
        }
        else
        {
            _isActive = false;
        }
    }

    protected override bool BeforeDraw(in OverlayDrawArgs args)
    {
        if (!_entityManager.TryGetComponent(_playerManager.LocalEntity, out EyeComponent? eyeComp))
            return false;

        if (args.Viewport.Eye != eyeComp.Eye)
            return false;

        return _isActive;
    }

    protected override void Draw(in OverlayDrawArgs args)
    {
        if (ScreenTexture == null)
            return;

        var handle = args.WorldHandle;
        _dizzyShader.SetParameter("SCREEN_TEXTURE", ScreenTexture);
        _dizzyShader.SetParameter("boozePower", 10.0f);  // Always at max power
        handle.UseShader(_dizzyShader);
        handle.DrawRect(args.WorldBounds, Color.White);
        handle.UseShader(null);
    }
}
