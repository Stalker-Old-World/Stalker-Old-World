using Content.Shared.GameTicking;
using Content.Shared.Light.Components;
using Content.Shared.Random.Rules;
using Robust.Shared.Timing;

namespace Content.Shared._Stalker_EN.Rules;

/// <summary>
/// Checks if the grid being stood on is at daytime or night time.
/// </summary>
public sealed partial class IsDayRule : RulesRule
{

    public override bool Check(EntityManager entManager, EntityUid uid)
    {
        if (entManager.TryGetComponent<TransformComponent>(uid, out var xform) &&
            entManager.TryGetComponent<SunShadowCycleComponent>(xform.GridUid, out var cycle))
        {
            // stupid chud code
            var meta = entManager.EntitySysManager.GetEntitySystem<MetaDataSystem>();
            var ticker = entManager.EntitySysManager.GetEntitySystem<SharedGameTicker>();
            var timing = IoCManager.Resolve<IGameTiming>();
            var pausedTime = meta.GetPauseTime(xform.GridUid.Value);

            var time = (float)timing.CurTime
                .Add(cycle.Offset)
                .Subtract(ticker.RoundStartTimeSpan)
                .Subtract(pausedTime)
                .TotalSeconds;

            var timePercent = time / cycle.Duration.TotalSeconds;
            if (0.15 > timePercent || timePercent > 0.80) // if the daylight variables are ever changed then this breaks and will have to be tuned again
            {
                return Inverted;
            }
            return !Inverted;
        }
        return false;
    }
}
