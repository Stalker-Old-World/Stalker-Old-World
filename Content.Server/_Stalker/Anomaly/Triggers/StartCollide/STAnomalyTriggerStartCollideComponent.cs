using Content.Shared.Whitelist;

namespace Content.Server._Stalker.Anomaly.Triggers.StartCollide;

[RegisterComponent]
public sealed partial class STAnomalyTriggerStartCollideComponent : Component
{
    [DataField]
    public EntityWhitelist? Blacklist;

    [DataField]
    public string MainTriggerGroup = "StartCollide";
}
