using Content.Shared._Stalker.Characteristics;

namespace Content.Server._Stalker.Characteristics.Training
{
    [RegisterComponent, Access(typeof(CharacteristicTrainingComponent))]
    public sealed partial class CharacteristicTrainingComponent : Component
    {
        [DataField]
        public CharacteristicType Characteristic = CharacteristicType.Strength;

        [DataField]
        public int Increase = 1;

        [DataField]
        public int MinValue = 0;

        [DataField]
        public int MaxValue = 20;

        [DataField]
        public int Delay = 5;
    }
}
