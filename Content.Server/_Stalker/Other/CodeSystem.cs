using Content.Server.Decals;
using Content.Shared.Decals;
using Robust.Shared.Map;

namespace Content.Server._Stalker.Other;

/// <summary>
/// This handles...
/// </summary>
public sealed class CodeSystem : EntitySystem
{

    [Dependency] private readonly DecalSystem _decals = default!;

    /// <inheritdoc/>
    public override void Initialize()
    {

    }


    public TransformComponent CSTransform(EntityUid uid)
    {
        return Transform(uid);
    }

    public void RaiseNetworkEvent(EntityEventArgs message)
    {
        EntityManager.EntityNetManager?.SendSystemNetworkMessage(message);
    }


    public NetCoordinates GetNetCoordinates(EntityCoordinates coordinates, MetaDataComponent? metadata = null)
    {
        return EntityManager.GetNetCoordinates(coordinates, metadata);
    }


    public void SpawnDecal(Decal Input,EntityCoordinates coordinates)
    {
        _decals.TryAddDecal(Input,coordinates,out uint decalId);
    }

}

