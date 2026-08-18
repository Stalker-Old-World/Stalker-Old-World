namespace Content.Shared._Stalker.Anomaly.Triggers.Events;

public sealed class STAnomalyTriggerEvent(HashSet<string> groups, bool stateChanger = true) : EntityEventArgs
{
    public IReadOnlySet<string> Groups = groups;
    public readonly bool StateChanger = stateChanger;
}
