using Content.Server._Stalker.Anomaly.Triggers.Systems;
using Content.Shared._Stalker.Anomaly.Triggers.Events;
using Robust.Shared.Timing;

namespace Content.Server._Stalker.Anomaly.Triggers.TimeDelay;

public sealed class STAnomalyTriggerTimeDelaySystem : EntitySystem
{
    [Dependency] private readonly IGameTiming _timing = default!;
    [Dependency] private readonly STAnomalyTriggerSystem _anomalyTrigger = default!;

    private readonly List<ActivationQueueElement> _activationQueue = new();

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<STAnomalyTriggerTimeDelayComponent, STAnomalyChangedStateEvent>(OnChangesState);
    }

    public override void Update(float frameTime)
    {
        base.Update(frameTime);

        var removeFromQuery = new List<ActivationQueueElement>();
        foreach (var element in _activationQueue)
        {
            if (_timing.CurTime < element.Time)
                continue;

            removeFromQuery.Add(element);
            _anomalyTrigger.Trigger(element.Anomaly, element.Options.Group);
        }

        foreach (var element in removeFromQuery)
        {
            _activationQueue.Remove(element);
        }
    }

    private void OnChangesState(Entity<STAnomalyTriggerTimeDelayComponent> trigger, ref STAnomalyChangedStateEvent args)
    {
        if (!trigger.Comp.Options.TryGetValue(args.State, out var options))
            return;

        var time = _timing.CurTime + options.Delay;
        _activationQueue.Add(new ActivationQueueElement(trigger, time, options));
    }

    private struct ActivationQueueElement(EntityUid anomaly, TimeSpan time, STAnomalyTriggerRimeDelayOptions options)
    {
        public readonly EntityUid Anomaly = anomaly;
        public readonly TimeSpan Time = time;
        public readonly STAnomalyTriggerRimeDelayOptions Options = options;
    }
}
