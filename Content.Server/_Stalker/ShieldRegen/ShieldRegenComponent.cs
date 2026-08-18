using Content.Shared.Damage;

namespace Content.Server._Stalker.ShieldRegen;

[RegisterComponent]
public sealed partial class ShieldRegenComponent : Component
{
    public TimeSpan StartTime = TimeSpan.FromSeconds(0f);

    public TimeSpan EndTime = TimeSpan.FromSeconds(0f);
    public TimeSpan ReloadTime = TimeSpan.FromSeconds(0.1f);
    public TimeSpan RegenStartTime = TimeSpan.FromSeconds(0f);

    public TimeSpan RegenEndTime = TimeSpan.FromSeconds(0f);
    public TimeSpan RegenReloadTime = TimeSpan.FromSeconds(5f);

    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public DamageSpecifier HealDamage = new()
    {
        DamageDict = new()
        {
            { "Asphyxiation", 5 },
            { "Bloodloss", 5 },
            { "Blunt", 5 },
            { "Heat", 5 },
            { "Piercing", 5 },
            { "Poison", 5 },
            { "Slash", 5 },
            { "Cellular", 5 }
        }
    };

    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public float Health = 200f;

}
