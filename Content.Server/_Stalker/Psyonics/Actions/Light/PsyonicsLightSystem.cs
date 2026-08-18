using Content.Server.DoAfter;
using Content.Shared._Stalker.Psyonics;
using Content.Shared._Stalker.Psyonics.Actions;
using Content.Shared._Stalker.Psyonics.Actions.Light;
using Content.Shared.ActionBlocker;
using Robust.Shared.Timing;

namespace Content.Server._Stalker.Psyonics.Actions.Light;
public sealed class PsyonicsLightSystem : BasePsyonicsActionSystem<PsyonicsActionLightComponent, PsyonicsActionLightEvent>
{
    [Dependency] private readonly IGameTiming _timing = default!;
    [Dependency] private readonly PsyonicsSystem _psy = default!;

    public override void Initialize()
    {
        base.Initialize();
    }

    public override void Update(float frameTime)
    {
        base.Update(frameTime);

        var query = EntityQueryEnumerator<PsyonicsActionLightComponent>();
        while (query.MoveNext(out var uid, out var comp))
        {
            if (!comp.IsActive)
                continue;
            var newPeriodStartsAt = comp.LastDecay + TimeSpan.FromSeconds(comp.PeriodSeconds);
            if (_timing.CurTime > newPeriodStartsAt)
            {
                comp.LastDecay = _timing.CurTime;

                if (TryComp<PsyonicsComponent>(uid, out var psyComponent))
                {
                    int remainingPsy = _psy.RemovePsy((uid, psyComponent), comp.PricePerPeriod);
                    if (remainingPsy <= 0)
                    {
                        comp.IsActive = false;
                        UpdateAppearance(uid, comp);
                    }
                }
            }
        }
    }

    [Dependency] private readonly SharedAppearanceSystem _appearance = default!;
    protected override void OnAction(Entity<PsyonicsActionLightComponent> entity, ref PsyonicsActionLightEvent args)
    {
        entity.Comp.IsActive = !entity.Comp.IsActive;
        if (!TryComp<PsyonicsComponent>(entity.Owner, out var psy))
            return;
        float remainingPsy = _psy.GetPsy((entity.Owner, psy));
        if (remainingPsy < entity.Comp.PricePerPeriod)
            return;
        UpdateAppearance(entity.Owner, entity.Comp);
        args.Handled = true;
    }

    public void UpdateAppearance(EntityUid uid, PsyonicsActionLightComponent? meditation = null)
    {
        if (!TryComp(uid, out AppearanceComponent? appearance))
            return;
        if (!Resolve(uid, ref meditation, ref appearance))
            return;

        _appearance.SetData(uid, PsyonicsLightVisuals.IsActive, meditation.IsActive, appearance);
    }
}
