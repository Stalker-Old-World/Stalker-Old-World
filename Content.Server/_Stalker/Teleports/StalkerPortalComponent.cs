namespace Content.Server._Stalker;

/// <summary>
/// This is used for...
/// </summary>
[RegisterComponent]
public sealed partial class StalkerPortalComponent : Component
{
    //Имя телепорта сталкеров, например "Бандиты", "Долг" и т.д.
    [DataField("PortalName")]
    public string PortalName = string.Empty;

    [DataField]
    public bool AllowAll;
}
