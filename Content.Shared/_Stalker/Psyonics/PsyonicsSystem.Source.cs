using Content.Shared.IdentityManagement;
using Content.Shared.Popups;
using Content.Shared.Verbs;

namespace Content.Shared._Stalker.Psyonics;

public sealed partial class PsyonicsSystem : EntitySystem
{
    private void InitializeSource()
    {
        SubscribeLocalEvent<PsyonicsAbsorbableComponent, GetVerbsEvent<AlternativeVerb>>(OnGetVerbs);
    }

    private void OnGetVerbs(Entity<PsyonicsAbsorbableComponent> absorbable, ref GetVerbsEvent<AlternativeVerb> args)
    {
        if (!args.CanInteract)
            return;

        var user = args.User;
        if (!TryComp<PsyonicsComponent>(user, out var psionics))
            return;

        args.Verbs.Add(new AlternativeVerb()
        {
            Text = Loc.GetString(absorbable.Comp.VerbAction),
            Act = () =>
            {
                Absorb(absorbable, (user, psionics));
            },
        });
    }

    private void Absorb(Entity<PsyonicsAbsorbableComponent> absorbable, Entity<PsyonicsComponent> psionics)
    {
        if (_net.IsServer)
            _popup.PopupEntity(Loc.GetString(absorbable.Comp.VerbPopup, ("user", Identity.Entity(psionics, EntityManager))), psionics, PopupType.Large);

        RegenPsy(psionics, absorbable.Comp.PsyRecovery);
        if (!absorbable.Comp.IsPersistent)
            Del(absorbable);
    }
}
