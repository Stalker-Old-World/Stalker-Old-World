using Content.Shared._Stalker.Characteristics;

namespace Content.Server._Stalker.Characteristics.Modifiers.Stamina.Decay;

[RegisterComponent]
public sealed partial class CharacteristicModifierStaminaDecayComponent : BaseCharacteristicFloatModifierComponent
{
    [DataField]
    public override CharacteristicType AllowedType { get; set; } = CharacteristicType.Endurance;

    [DataField]
    public override float MinModifier { get; set; } = 0.1f;

    [DataField]
    public override float MaxModifier { get; set; } = 2f;

    [DataField]
    public override float Modifier { get; set; } = 0.02f;
}
