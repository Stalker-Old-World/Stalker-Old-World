namespace Content.Server._Stalker.SinMobController;

[RegisterComponent]
public sealed partial class SinMobControllerComponent : Component
{
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public string ToggleOnAction = default!;

    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public string ToggleOffAction = default!;
}
