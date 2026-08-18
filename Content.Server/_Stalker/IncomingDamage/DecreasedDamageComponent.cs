using Content.Shared.Damage;

namespace Content.Server._Stalker.IncomingDamage;

[RegisterComponent]
public sealed partial class DecreasedDamageComponent : Component
{
    [ViewVariables(VVAccess.ReadOnly)]
    public DamageModifierSet? Modifiers;
    public bool Active;
    public TimeSpan? TimeToDelete;
}
