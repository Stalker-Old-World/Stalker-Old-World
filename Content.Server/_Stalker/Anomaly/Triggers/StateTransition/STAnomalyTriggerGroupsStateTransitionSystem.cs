using Content.Server._Stalker.Anomaly.Triggers.Systems;
using Content.Shared._Stalker.Anomaly.Triggers.Events;

namespace Content.Server._Stalker.Anomaly.Triggers.StateTransition;

public sealed class STAnomalyTriggerGroupsStateTransitionSystem : EntitySystem
{
    [Dependency] private readonly STAnomalyTriggerSystem _trigger = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<STAnomalyTriggerGroupsStateTransitionComponent, STAnomalyChangedStateEvent>(OnChangedState);
    }

    private void OnChangedState(Entity<STAnomalyTriggerGroupsStateTransitionComponent> trigger, ref STAnomalyChangedStateEvent args)
    {
        var group = trigger.Comp.Prefix == string.Empty ? args.State : $"{trigger.Comp.Prefix}{args.State}";
        _trigger.Trigger(trigger, group, false);
    }
}
