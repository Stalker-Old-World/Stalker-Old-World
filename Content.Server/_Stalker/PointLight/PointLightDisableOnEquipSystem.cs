using Content.Shared.Inventory.Events;
using Robust.Server.GameObjects;

namespace Content.Server._Stalker.PointLight;

public sealed class PointLightDisableOnEquipSystem : EntitySystem
{
    [Dependency] private readonly PointLightSystem _pointLight = default!;

    public override void Initialize()
    {
        SubscribeLocalEvent<PointLightDisableOnEquipComponent, GotEquippedEvent>(OnGotEquipped);
        SubscribeLocalEvent<PointLightDisableOnEquipComponent, GotUnequippedEvent>(OnGotUnequipped);
    }

    private void OnGotEquipped(Entity<PointLightDisableOnEquipComponent> disabler, ref GotEquippedEvent args)
    {
        if (!_pointLight.TryGetLight(disabler, out var pointLight))
            return;

        disabler.Comp.Enabled = pointLight.Enabled;
        _pointLight.SetEnabled(disabler, false);
    }

    private void OnGotUnequipped(Entity<PointLightDisableOnEquipComponent> disabler, ref GotUnequippedEvent args)
    {
        _pointLight.SetEnabled(disabler, disabler.Comp.Enabled);
    }
}
