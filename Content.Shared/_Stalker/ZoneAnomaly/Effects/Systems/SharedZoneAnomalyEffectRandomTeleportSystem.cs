using Robust.Shared.Map;

namespace Content.Shared._Stalker.ZoneAnomaly.Effects.Systems;

public class SharedZoneAnomalyEffectRandomTeleportSystem : EntitySystem
{
    [Dependency] protected readonly SharedTransformSystem _transform = default!;

    public void TeleportEntity(EntityUid entity, EntityCoordinates coords, bool reParent = true)
    {
        _transform.SetCoordinates(entity, coords);

        var map = coords.GetMapUid(EntityManager);
        if (map != null && reParent)
            _transform.SetParent(entity, map.Value);
    }
}
