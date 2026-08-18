using Content.Server.DoAfter;
using Content.Shared._Stalker.Psyonics;
using Content.Shared._Stalker.Psyonics.Actions;
using Content.Shared._Stalker.Psyonics.Actions.Meditation;
using Content.Shared.ActionBlocker;
using Content.Shared.Damage;
using Content.Shared.DoAfter;
using Content.Shared.FixedPoint;
using Content.Shared.Interaction.Events;
using Content.Shared.Movement.Events;
using Robust.Shared.Timing;

namespace Content.Server._Stalker.Psyonics.Actions.Meditation;
public sealed class PsyonicsMeditationSystem : BasePsyonicsActionSystem<PsyonicsActionMeditationComponent, PsyonicsActionMeditationEvent>
{
    [Dependency] private readonly IGameTiming _timing = default!;
    [Dependency] private readonly ActionBlockerSystem _blocker = default!;
    [Dependency] private readonly PsyonicsSystem _psy = default!;

    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<PsyonicsActionMeditationComponent, UpdateCanMoveEvent>(OnActionAttempt);
        SubscribeLocalEvent<PsyonicsActionMeditationComponent, AttackAttemptEvent>(OnActionAttempt);
    }

    private void OnActionAttempt(EntityUid uid, PsyonicsActionMeditationComponent component, CancellableEntityEventArgs args)
    {
        if (component.IsActive)
            args.Cancel();
    }

    public override void Update(float frameTime)
    {
        base.Update(frameTime);

        var query = EntityQueryEnumerator<PsyonicsActionMeditationComponent>();
        while (query.MoveNext(out var uid, out var comp))
        {
            if (!comp.IsActive)
                continue;
            var newPeriodStartsAt = comp.LastRecovery + TimeSpan.FromSeconds(comp.PeriodSeconds);
            if (_timing.CurTime > newPeriodStartsAt)
            {
                comp.LastRecovery = _timing.CurTime;

                if (TryComp<PsyonicsComponent>(uid, out var psyComponent))
                {
                    _psy.RegenPsy((uid, psyComponent), comp.RecoverPerPeriod);
                }
            }
        }
    }

    [Dependency] private readonly SharedAppearanceSystem _appearance = default!;
    protected override void OnAction(Entity<PsyonicsActionMeditationComponent> entity, ref PsyonicsActionMeditationEvent args)
    {
        entity.Comp.IsActive = !entity.Comp.IsActive;
        _blocker.UpdateCanMove(entity.Owner);
        _blocker.CanAttack(entity.Owner);
        UpdateAppearance(entity.Owner, entity.Comp);
        args.Handled = true;
    }

    public void UpdateAppearance(EntityUid uid, PsyonicsActionMeditationComponent? meditation = null)
    {
        if (!TryComp(uid, out AppearanceComponent? appearance))
            return;
        if (!Resolve(uid, ref meditation, ref appearance))
            return;

        _appearance.SetData(uid, PsyonicsMeditationVisuals.IsMeditating, meditation.IsActive, appearance);
    }
}
