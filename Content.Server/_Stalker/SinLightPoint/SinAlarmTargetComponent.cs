namespace Content.Server._Stalker.SinLightPoint;

[RegisterComponent]
public sealed partial class SinAlarmTargetComponent : Component

{
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public bool Triggering = true;
}
