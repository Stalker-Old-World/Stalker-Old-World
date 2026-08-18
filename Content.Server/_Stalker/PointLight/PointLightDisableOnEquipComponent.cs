namespace Content.Server._Stalker.PointLight;

[RegisterComponent]
public sealed partial class PointLightDisableOnEquipComponent : Component
{
    [DataField]
    public bool Enabled;
}
