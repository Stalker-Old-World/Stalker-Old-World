namespace Content.Server._Stalker.AddCustomComponent;

/// <summary>
/// This is used for...
/// </summary>
[RegisterComponent]
public sealed partial class AddOrDelOnCollideSafeZoneComponent : Component
{
    [DataField("MustAdd")]
    public bool MustAdd = true;
}
