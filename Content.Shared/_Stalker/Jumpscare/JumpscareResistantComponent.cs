namespace Content.Shared._Stalker.Jumpscare;

[RegisterComponent]
public sealed partial class     JumpscareResistantComponent : Component
{
    [DataField]
    public float TimeBeforeRemove = 3;
}
