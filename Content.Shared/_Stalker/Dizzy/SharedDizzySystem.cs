using Content.Shared._Stalker.Psyonics.Actions.Dizzy;
using Content.Shared.Speech.EntitySystems;
using Content.Shared.StatusEffect;
using Robust.Shared.Timing;

namespace Content.Shared._Stalker.Dizzy;
public abstract class SharedDizzySystem : EntitySystem
{
    [ValidatePrototypeId<StatusEffectPrototype>]
    public const string DizzyKey = "Dizzy";

    [Dependency] private readonly StatusEffectsSystem _statusEffectsSystem = default!;
    [Dependency] private readonly SharedSlurredSystem _slurredSystem = default!;
    [Dependency] private readonly IGameTiming _gameTiming = default!;

    public override void Initialize()
    {
        base.Initialize();
    }

    public void TryApplyDizziness(EntityUid uid, float power, StatusEffectsComponent? status = null)
    {
        if (!Resolve(uid, ref status, false))
            return;

        if (!_statusEffectsSystem.HasStatusEffect(uid, DizzyKey, status))
        {
            _statusEffectsSystem.TryAddStatusEffect<DizzyComponent>(uid, DizzyKey, TimeSpan.FromSeconds(power), true, status);
        }
        else
        {
            _statusEffectsSystem.TryAddTime(uid, DizzyKey, TimeSpan.FromSeconds(power), status);
        }
    }

    public void TryRemoveDizziness(EntityUid uid)
    {
        _statusEffectsSystem.TryRemoveStatusEffect(uid, DizzyKey);
    }
}
