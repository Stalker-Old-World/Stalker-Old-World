namespace Content.Server._Stalker.MemoryLost;

[RegisterComponent]
public sealed partial class MemoryLostComponent : Component
{
    [DataField]
    public float CoolDownTime = 2f;

    [DataField]
    public int PopupCount = 8;
}
