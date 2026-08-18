using System.Numerics;
using Robust.Shared.Map;

namespace Content.Shared._Stalker.Dimension;

public abstract class STSharedDimensionSystem : EntitySystem
{
    [Dependency] private readonly IMapManager _map = default!;
    [Dependency] private readonly SharedTransformSystem _transform = default!;

    protected void EnterDimension(EntityUid target, MapId mapId, Vector2 worldPosition)
    {
        if (!_map.TryFindGridAt(mapId, worldPosition, out var gridUid, out _))
            return;

        var invMatrix = _transform.GetInvWorldMatrix(gridUid);
        var gridPos = Vector2.Transform(worldPosition, invMatrix);
        _transform.SetCoordinates(target, Transform(target), new EntityCoordinates(gridUid, gridPos));
    }
}
