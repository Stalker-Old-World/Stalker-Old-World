namespace Content.Server._Stalker.DelayOnTriggerComponent;

[RegisterComponent]
public sealed partial class DelayOnTriggerComponent : Component
{
    [DataField("delay")]
    public float Delay;
}
