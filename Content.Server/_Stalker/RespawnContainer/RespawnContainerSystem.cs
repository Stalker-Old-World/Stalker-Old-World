using Content.Server.Administration.Commands;
using Content.Shared._Stalker.RespawnContainer;

namespace Content.Server._Stalker.RespawnContainer;

public sealed class RespawnContainerSystem : SharedRespawnContainerSystem
{
    public override void Initialize()
    {
        SubscribeLocalEvent<RespawnContainerComponent, RespawnedByCommandEvent>(OnRespawn);
    }

    private void OnRespawn(Entity<RespawnContainerComponent> entity, ref RespawnedByCommandEvent args)
    {
        var oldEnt = GetEntity(args.OldEntity);
        var comp = EnsureComp<RespawnContainerComponent>(oldEnt);
        TransferTo((oldEnt, comp), entity.Owner);
    }
}

