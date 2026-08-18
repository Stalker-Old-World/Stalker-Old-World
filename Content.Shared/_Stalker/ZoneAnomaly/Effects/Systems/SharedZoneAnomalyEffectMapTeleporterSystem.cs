using Robust.Shared.Map;

namespace Content.Shared._Stalker.ZoneAnomaly.Effects.Systems;

public class SharedZoneAnomalyEffectMapTeleporterSystem : EntitySystem
{
    [Dependency] private readonly IMapManager _map = default!;
    [Dependency] private readonly SharedTransformSystem _transform = default!;

    protected void TeleportEntity(EntityUid entity, MapCoordinates coords, bool reParent = true)
    {
        var map = _map.GetMapEntityId(coords.MapId);
        var position = new EntityCoordinates(map, coords.Position);

        _transform.SetCoordinates(entity, position);

        if (!reParent)
            return;

        _transform.SetParent(entity, map);
    }

}
