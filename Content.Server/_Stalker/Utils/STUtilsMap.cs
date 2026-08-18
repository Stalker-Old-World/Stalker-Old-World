using Robust.Shared.Map.Components;

namespace Content.Server._Stalker.Utils;

public static class STUtilsMap
{
    public static bool InWorld(EntityUid entityUid, IEntityManager? entityManager = null)
    {
        return InWorld((entityUid, null), entityManager);
    }

    public static bool InWorld(Entity<TransformComponent?> entity, IEntityManager? entityManager = null)
    {
        entityManager ??= IoCManager.Resolve<IEntityManager>();

        if (entity.Comp is null && !entityManager.TryGetComponent(entity, out entity.Comp))
            return false;

        var parent = entity.Comp.ParentUid;
        if (parent == EntityUid.Invalid)
            return false;

        return entityManager.HasComponent<MapComponent>(parent) || entityManager.HasComponent<MapGridComponent>(parent);
    }
}
