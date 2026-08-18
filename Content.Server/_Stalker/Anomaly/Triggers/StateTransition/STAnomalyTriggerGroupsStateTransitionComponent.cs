namespace Content.Server._Stalker.Anomaly.Triggers.StateTransition;

[RegisterComponent]
public sealed partial class STAnomalyTriggerGroupsStateTransitionComponent : Component
{
    [DataField]
    public string Prefix = "State";
}
