using Content.Shared.CombatMode.Pacification;
using Robust.Server.GameObjects;
using Robust.Shared.Physics.Events;
using Robust.Shared.Player;

namespace Content.Server._Stalker.PacifiedZone;

public sealed class StalkerPacifiedZoneSystem : EntitySystem
{
    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<StalkerPacifiedZoneComponent, StartCollideEvent>(OnCollideStalkerPacifiedZone);
    }

    private void OnCollideStalkerPacifiedZone(EntityUid uid, StalkerPacifiedZoneComponent component, ref StartCollideEvent args)
    {
        if (!TryComp(args.OtherEntity, out ActorComponent? actor))
            return;

        if (actor.PlayerSession.AttachedEntity == null)
            return;

        if (component.Pacified)
        {
            EnsureComp<PacifiedComponent>(actor.PlayerSession.AttachedEntity.Value);
        }
        else
        {
            RemComp<PacifiedComponent>(actor.PlayerSession.AttachedEntity.Value);
        }

    }

}
