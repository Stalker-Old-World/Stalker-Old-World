namespace Content.Shared._Stalker.Anomaly.Triggers.Events;

public sealed class STAnomalyTriggerStartCollideGetAdditionalGroupsEvent(EntityUid target) : EntityEventArgs
{
    public readonly EntityUid Target = target;
    public IReadOnlySet<string> Groups => _groups;

    private readonly HashSet<string> _groups = new();

    public void Add(string group)
    {
        _groups.Add(group);
    }

    public void Add(IEnumerable<string> groups)
    {
        _groups.UnionWith(groups);
    }
}
