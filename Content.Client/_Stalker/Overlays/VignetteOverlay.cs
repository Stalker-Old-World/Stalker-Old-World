using Robust.Client.Graphics;
using Robust.Client.Player;
using Robust.Shared.Enums;
using Robust.Shared.Prototypes;
using Content.Shared.Eye.Blinding.Components;

namespace Content.Client._Stalker.Overlays
{
    public sealed class VignetteOverlay : Overlay
    {
        [Dependency] private readonly IPrototypeManager _prototypeManager = default!;
        [Dependency] private readonly IPlayerManager _playerManager = default!;
        [Dependency] IEntityManager _entityManager = default!;

        public override OverlaySpace Space => OverlaySpace.WorldSpace;
        private readonly ShaderInstance _vignetteNoisedShader;
        private readonly ShaderInstance _vignetteBaseShader;

        public VignetteOverlay()
        {
            IoCManager.InjectDependencies(this);
            _vignetteNoisedShader = _prototypeManager.Index<ShaderPrototype>("VignetteNoised").InstanceUnique();
            _vignetteBaseShader = _prototypeManager.Index<ShaderPrototype>("VignetteBase").InstanceUnique();
        }
        protected override bool BeforeDraw(in OverlayDrawArgs args)
        {
            if (!_entityManager.TryGetComponent(_playerManager.LocalSession?.AttachedEntity, out EyeComponent? eyeComp))
                return false;

            if (args.Viewport.Eye != eyeComp.Eye)
                return false;

            var playerEntity = _playerManager.LocalSession?.AttachedEntity;

            if (playerEntity == null)
                return false;

            if (!_entityManager.TryGetComponent<BlindableComponent>(playerEntity, out var blindComp))
                return false;

            return true;
        }

        protected override void Draw(in OverlayDrawArgs args)
        {
            var worldHandle = args.WorldHandle;
            var viewport = args.WorldBounds;
            worldHandle.UseShader(_vignetteBaseShader);
            worldHandle.DrawRect(viewport, Color.White);
            worldHandle.UseShader(_vignetteNoisedShader);
            worldHandle.DrawRect(viewport, Color.White);
            worldHandle.UseShader(null);
        }
    }
}
