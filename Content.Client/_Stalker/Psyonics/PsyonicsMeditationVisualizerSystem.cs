using Content.Client.Atmos.Components;
using Content.Shared._Stalker.Psyonics.Actions.Meditation;
using Content.Shared.Atmos;
using Robust.Client.GameObjects;
using Robust.Shared.Map;
using System.Numerics;

namespace Content.Client.Atmos.EntitySystems;

/// <summary>
/// This handles the display of fire effects on meditation entities.
/// </summary>
public sealed class PsyonicsMeditationVisualizerSystem : VisualizerSystem<PsyonicsMeditationVisualsComponent>
{
    [Dependency] private readonly PointLightSystem _lights = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<PsyonicsMeditationVisualsComponent, ComponentInit>(OnComponentInit);
        SubscribeLocalEvent<PsyonicsMeditationVisualsComponent, ComponentShutdown>(OnShutdown);
    }

    private void OnShutdown(EntityUid uid, PsyonicsMeditationVisualsComponent component, ComponentShutdown args)
    {
        if (component.LightEntity != null)
        {
            Del(component.LightEntity.Value);
            component.LightEntity = null;
        }

        // Need LayerMapTryGet because Init fails if there's no existing sprite / appearancecomp
        // which means in some setups (most frequently no AppearanceComp) the layer never exists.
        if (TryComp<SpriteComponent>(uid, out var sprite) &&
            sprite.LayerMapTryGet(MeditationVisualLayers.Meditation, out var layer))
        {
            sprite.RemoveLayer(layer);
        }
    }

    private void OnComponentInit(EntityUid uid, PsyonicsMeditationVisualsComponent component, ComponentInit args)
    {
        if (!TryComp<SpriteComponent>(uid, out var sprite) || !TryComp(uid, out AppearanceComponent? appearance))
            return;

        sprite.LayerMapReserveBlank(MeditationVisualLayers.Meditation);
        sprite.LayerSetVisible(MeditationVisualLayers.Meditation, false);
        sprite.LayerSetShader(MeditationVisualLayers.Meditation, "unshaded");
        if (component.Sprite != null)
            sprite.LayerSetRSI(MeditationVisualLayers.Meditation, component.Sprite);

        UpdateAppearance(uid, component, sprite, appearance);
    }

    protected override void OnAppearanceChange(EntityUid uid, PsyonicsMeditationVisualsComponent component, ref AppearanceChangeEvent args)
    {
        if (args.Sprite != null)
            UpdateAppearance(uid, component, args.Sprite, args.Component);
    }

    private void UpdateAppearance(EntityUid uid, PsyonicsMeditationVisualsComponent component, SpriteComponent sprite, AppearanceComponent appearance)
    {
        if (!sprite.LayerMapTryGet(MeditationVisualLayers.Meditation, out var index))
            return;

        AppearanceSystem.TryGetData<bool>(uid, PsyonicsMeditationVisuals.IsMeditating, out var hasMeditation, appearance);
        sprite.LayerSetVisible(index, hasMeditation);
        sprite.LayerSetOffset(index, new Vector2(0, -0.1f));

        if (!hasMeditation)
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
        _lights.SetRadius(component.LightEntity.Value, Math.Clamp(component.LightRadius, 1.5f, 1.5f + component.LightRadius), light);
        _lights.SetEnergy(component.LightEntity.Value, Math.Clamp(component.LightEnergy, 1f, 1f + component.LightEnergy), light);

    }
}

public enum MeditationVisualLayers : byte
{
    Meditation
}
