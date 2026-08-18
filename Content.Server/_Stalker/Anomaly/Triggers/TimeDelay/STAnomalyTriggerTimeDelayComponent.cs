namespace Content.Server._Stalker.Anomaly.Triggers.TimeDelay;

[RegisterComponent]
public sealed partial class STAnomalyTriggerTimeDelayComponent : Component
{
    [DataField]
    public Dictionary<string, STAnomalyTriggerRimeDelayOptions> Options;
}

[Serializable, DataDefinition]
public partial struct STAnomalyTriggerRimeDelayOptions
{
    [DataField, ViewVariables]
    public string Group;

    [DataField, ViewVariables]
    public TimeSpan Delay = TimeSpan.FromSeconds(5f);
}
