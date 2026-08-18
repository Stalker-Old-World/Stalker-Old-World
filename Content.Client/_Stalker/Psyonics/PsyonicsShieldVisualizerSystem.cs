using Content.Client.Atmos.Components;
using Content.Shared._Stalker.Psyonics.Actions.Shield;
using Content.Shared.Atmos;
using Robust.Client.GameObjects;
using Robust.Shared.Map;

namespace Content.Client.Atmos.EntitySystems;

/// <summary>
/// This handles the display of fire effects on shielded entities.
/// </summary>
public sealed class PsyonicsShieldVisualizerSystem : VisualizerSystem<PsyonicsShieldVisualsComponent>
{
    [Dependency] private readonly PointLightSystem _lights = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<PsyonicsShieldVisualsComponent, ComponentInit>(OnComponentInit);
        SubscribeLocalEvent<PsyonicsShieldVisualsComponent, ComponentShutdown>(OnShutdown);
    }

    private void OnShutdown(EntityUid uid, PsyonicsShieldVisualsComponent component, ComponentShutdown args)
    {
        if (component.LightEntity != null)
        {
            Del(component.LightEntity.Value);
            component.LightEntity = null;
        }

        // Need LayerMapTryGet because Init fails if there's no existing sprite / appearancecomp
        // which means in some setups (most frequently no AppearanceComp) the layer never exists.
        if (TryComp<SpriteComponent>(uid, out var sprite) &&
            sprite.LayerMapTryGet(ShieldVisualLayers.Shield, out var layer))
        {
            sprite.RemoveLayer(layer);
        }
    }

    private void OnComponentInit(EntityUid uid, PsyonicsShieldVisualsComponent component, ComponentInit args)
    {
        if (!TryComp<SpriteComponent>(uid, out var sprite) || !TryComp(uid, out AppearanceComponent? appearance))
            return;

        sprite.LayerMapReserveBlank(ShieldVisualLayers.Shield);
        sprite.LayerSetVisible(ShieldVisualLayers.Shield, false);
        sprite.LayerSetShader(ShieldVisualLayers.Shield, "unshaded");
        if (component.Sprite != null)
            sprite.LayerSetRSI(ShieldVisualLayers.Shield, component.Sprite);

        UpdateAppearance(uid, component, sprite, appearance);
    }

    protected override void OnAppearanceChange(EntityUid uid, PsyonicsShieldVisualsComponent component, ref AppearanceChangeEvent args)
    {
        if (args.Sprite != null)
            UpdateAppearance(uid, component, args.Sprite, args.Component);
    }

    private void UpdateAppearance(EntityUid uid, PsyonicsShieldVisualsComponent component, SpriteComponent sprite, AppearanceComponent appearance)
    {
        if (!sprite.LayerMapTryGet(ShieldVisualLayers.Shield, out var index))
            return;

        AppearanceSystem.TryGetData<bool>(uid, ShieldVisuals.HasShield, out var hasShield, appearance);
        AppearanceSystem.TryGetData<float>(uid, ShieldVisuals.ShieldHealth, out var shieldHealth, appearance);
        sprite.LayerSetVisible(index, hasShield);

        if (!hasShield)
        {
            if (component.LightEntity != null)
            {
                Del(component.LightEntity.Value);
                component.LightEntity = null;
            }

            return;
        }

        sprite.LayerSetState(index, component.State);

        component.LightEntity ??= Spawn(null, new EntityCoordinates(uid, default));
        var light = EnsureComp<PointLightComponent>(component.LightEntity.Value);

        _lights.SetColor(component.LightEntity.Value, component.LightColor, light);
        _lights.SetRadius(component.LightEntity.Value, Math.Clamp(1.5f + component.LightRadius * shieldHealth, 1.5f, 1.5f + component.LightRadius), light);
        _lights.SetEnergy(component.LightEntity.Value, Math.Clamp(1 + component.LightEnergy * shieldHealth, 0f, 1 + component.LightEnergy), light);

    }
}

public enum ShieldVisualLayers : byte
{
    Shield
}
