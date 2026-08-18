namespace Content.Server._Stalker.Anomaly.TargetGroups.Weight;

[RegisterComponent]
public sealed partial class STAnomalyTriggerStartCollideWeightComponent : Component
{
    [DataField]
    public Dictionary<float, string> WeightTriggerGroup = new()
    {
        { 10, "WeightSmall" },
        { 20, "WeightNormal" },
    };
}
