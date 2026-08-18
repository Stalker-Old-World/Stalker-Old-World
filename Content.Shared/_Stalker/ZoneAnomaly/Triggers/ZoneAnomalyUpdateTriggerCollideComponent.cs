namespace Content.Shared._Stalker.ZoneAnomaly.Triggers;

[RegisterComponent]
public sealed partial class ZoneAnomalyUpdateTriggerCollideComponent : ZoneAnomalyTriggerCollideComponent
{
    [DataField]
    public HashSet<EntityUid> InAnomaly = new();
}
