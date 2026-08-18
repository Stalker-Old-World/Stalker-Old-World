using Content.Server.Weapons.Ranged.Systems;
using Content.Shared._Stalker.Psyonics.Actions;
using Content.Shared._Stalker.Psyonics.Actions.Fireball;
using Content.Shared.Hands.EntitySystems;
using Content.Shared.Magic;
using Robust.Server.GameObjects;
using Robust.Shared.Map;

namespace Content.Server._Stalker.Psyonics.Actions.Fireball;

public sealed class PsyonicsFireballSystem : BasePsyonicsActionSystem<PsyonicsFireballComponent, PsyonicsActionFireballEvent>
{
    [Dependency] private readonly SharedHandsSystem _hands = default!;
    [Dependency] private readonly PhysicsSystem _physics = default!;
    [Dependency] private readonly IMapManager _mapManager = default!;
    [Dependency] private readonly SharedTransformSystem _transformSystem = default!;
    [Dependency] private readonly GunSystem _gunSystem = default!;

    protected override void OnAction(Entity<PsyonicsFireballComponent> entity, ref PsyonicsActionFireballEvent args)
    {
        base.OnAction(entity, ref args);

        var comp = entity.Comp;
        var xform = Transform(args.Performer);
        var userVelocity = _physics.GetMapLinearVelocity(args.Performer);
        var coords = new List<EntityCoordinates>(1) { xform.Coordinates };
        foreach (var pos in coords)
        {
            // If applicable, this ensures the projectile is parented to grid on spawn, instead of the map.
            var mapPos = pos.ToMap(EntityManager, _transformSystem);
            var spawnCoords = _mapManager.TryFindGridAt(mapPos, out var gridUid, out _)
                ? pos.WithEntityId(gridUid, EntityManager)
                : new(_mapManager.GetMapEntityId(mapPos.MapId), mapPos.Position);

            var ent = Spawn(comp.Prototype, spawnCoords);
            var direction = args.Target.ToMapPos(EntityManager, _transformSystem) -
                            spawnCoords.ToMapPos(EntityManager, _transformSystem);
            _gunSystem.ShootProjectile(ent, direction, userVelocity, args.Performer, args.Performer);
        }
        args.Handled = true;
    }
}
