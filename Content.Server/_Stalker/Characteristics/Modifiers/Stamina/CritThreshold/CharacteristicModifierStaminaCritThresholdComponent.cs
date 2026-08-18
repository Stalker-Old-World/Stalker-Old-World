using Content.Shared._Stalker.Characteristics;

namespace Content.Server._Stalker.Characteristics.Modifiers.Stamina.CritThreshold;

[RegisterComponent]
public sealed partial class CharacteristicModifierStaminaCritThresholdComponent : BaseCharacteristicFloatModifierComponent
{
    [DataField]
    public override CharacteristicType AllowedType { get; set; } = CharacteristicType.Endurance;

    [DataField]
    public override float MinModifier { get; set; } = 0.1f;

    [DataField]
    public override float MaxModifier { get; set; } = 1.5f;

    [DataField]
    public override float Modifier { get; set; } = 0.05f;
}
