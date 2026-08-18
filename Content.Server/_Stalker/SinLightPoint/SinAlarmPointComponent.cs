namespace Content.Server._Stalker.SinLightPoint;

[RegisterComponent]
public sealed partial class SinAlarmPointComponent : Component
{
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public float Distance = 25f;

    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public string Side = "nothing";

    [DataField]
    public HashSet<EntityUid> LightPoints = new();

    [DataField]
    public int Count;
}
