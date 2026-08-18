namespace Content.Server._Stalker.Trash;

[RegisterComponent, AutoGenerateComponentPause]
public sealed partial class TrashComponent : Component
{
    /// <summary>
    /// This field was created to set up specific time for this item to delete
    /// seconds
    /// </summary>
    [DataField("time")]
    public int TimeToDelete;

    /// <summary>
    /// This field was created to figure out when to delete this item
    /// </summary>
    [ViewVariables(VVAccess.ReadOnly), AutoPausedField]
    public TimeSpan? DeletingTime;

    [DataField("ignore"), ViewVariables(VVAccess.ReadWrite)]
    public bool IgnoreConditions;
}
