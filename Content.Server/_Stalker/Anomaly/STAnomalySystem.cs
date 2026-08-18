using Content.Shared._Stalker.Anomaly.Triggers.Events;

namespace Content.Server._Stalker.Anomaly;

public sealed class STAnomalySystem : EntitySystem
{
    private EntityQuery<STAnomalyComponent> _anomalyQuery;

    public override void Initialize()
    {
        base.Initialize();

        _anomalyQuery = GetEntityQuery<STAnomalyComponent>();

        SubscribeLocalEvent<STAnomalyComponent, STAnomalyTriggerEvent>(OnTrigger);
    }

    private void OnTrigger(Entity<STAnomalyComponent> anomaly, ref STAnomalyTriggerEvent args)
    {
        if (!args.StateChanger)
            return;

        var transitions = anomaly.Comp.States[anomaly.Comp.State];

        foreach (var transition in transitions)
        {
            if (!args.Groups.Contains(transition.Group))
                continue;

            SetState(anomaly, transition.State);
            break;
        }
    }

    private void SetState(Entity<STAnomalyComponent> anomaly, string state)
    {
        var previousState = anomaly.Comp.State;
        anomaly.Comp.State = state;

        var ev = new STAnomalyChangedStateEvent(previousState, state);
        RaiseLocalEvent(anomaly, ev);
    }
}
