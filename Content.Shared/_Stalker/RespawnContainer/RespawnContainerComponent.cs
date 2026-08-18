namespace Content.Shared._Stalker.RespawnContainer;

/// <summary>
/// Contains data that should be transferred between entities with mind
/// </summary>
[RegisterComponent]
public sealed partial class RespawnContainerComponent : Component
{
    /// <summary>
    /// This field is used for storing unpredicted amount of data records
    /// </summary>
    [DataField]
    public Dictionary<string, object> Data = new();
}
