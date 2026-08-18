namespace Content.Server._Stalker.Anomaly.TargetGroups;

[RegisterComponent]
public sealed partial class STAnomalyAdditionalGroupsComponent : Component
{
    [DataField]
    public HashSet<string> Groups = new();
}
