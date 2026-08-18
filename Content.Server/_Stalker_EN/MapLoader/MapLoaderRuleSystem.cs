using Content.Server._Stalker_EN.Trash;
using Content.Server.StationEvents.Events;
using Content.Shared.GameTicking.Components;
using Robust.Shared.EntitySerialization;
using Robust.Shared.EntitySerialization.Systems;
using Robust.Shared.Prototypes;
using Robust.Shared.Utility;

namespace Content.Server._Stalker_EN.MapLoader;

/// <summary>
/// This handles adding a bunch of extra maps
/// </summary>
public sealed class MapLoaderRuleSystem : StationEventSystem<MapLoaderRuleComponent>
{
    [Dependency] private readonly IPrototypeManager _protoMan = default!;
    [Dependency] private readonly MapLoaderSystem _mapLoader = default!;

    protected override void Added(EntityUid uid, MapLoaderRuleComponent component, GameRuleComponent gameRule, GameRuleAddedEvent args)
    {
        base.Added(uid, component, gameRule, args);

        var prototype = _protoMan.Index(component.prototype);
        foreach (var path in prototype.MapPaths.Values)
        {
            if (_mapLoader.TryLoadMap(new ResPath(path), out var mapEntity, out _, DeserializationOptions.Default with { InitializeMaps = true }))
                AddComp<CertifiedOrganicMapComponent>(mapEntity.Value);
        }
    }
}
