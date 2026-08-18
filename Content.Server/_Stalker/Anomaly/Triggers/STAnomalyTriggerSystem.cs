using Content.Shared._Stalker.Anomaly.Triggers.Events;

namespace Content.Server._Stalker.Anomaly.Triggers.Systems;

public sealed class STAnomalyTriggerSystem : EntitySystem
{
    public void Trigger(EntityUid uid, string group, bool stateChanger = true)
    {
        Trigger(uid, new HashSet<string> { group }, stateChanger);
    }

    public void Trigger(EntityUid uid, HashSet<string> groups, bool stateChanger = true)
    {
        if (!TryComp<STAnomalyComponent>(uid, out var comp))
            return;

        Trigger((uid, comp), groups, stateChanger);
    }

    public void Trigger(Entity<STAnomalyComponent> anomaly, string group, bool stateChanger = true)
    {
        Trigger(anomaly, new HashSet<string> { group }, stateChanger);
    }

    public void Trigger(Entity<STAnomalyComponent> anomaly, HashSet<string> groups, bool stateChanger = true)
    {
        var ev = new STAnomalyTriggerEvent(groups, stateChanger);
        RaiseLocalEvent(anomaly, ev);
    }
}
