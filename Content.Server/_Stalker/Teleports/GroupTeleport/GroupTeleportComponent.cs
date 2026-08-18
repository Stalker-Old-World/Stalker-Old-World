using Content.Shared.Damage;

namespace Content.Server._Stalker.Teleports.GroupTeleport;

[RegisterComponent, AutoGenerateComponentPause]
public sealed partial class GroupTeleportComponent : Component
{
    [DataField, ViewVariables(VVAccess.ReadOnly)]
    public string Group;

    [DataField, ViewVariables(VVAccess.ReadOnly)]
    public string TargetGroup;

    [DataField]
    public bool AllowAll;

    [DataField]
    public bool CooldownEnabled;

    [DataField]
    public float CooldownTime;

    [DataField]
    public float DecreasedTime;

    [DataField]
    public DamageModifierSet? ModifierSet;

    /// <summary>
    /// Relinking time in seconds
    /// </summary>
    [DataField("time")]
    public float ReLinkTime;

    [AutoPausedField, ViewVariables(VVAccess.ReadOnly)]
    public TimeSpan? ReLink;
}
