namespace Content.Server.TrashDetector.Components
{
    [RegisterComponent]
    public sealed partial class TrashDetectorComponent : Component
    {
        [DataField]
        public float SearchTime = 5;

        [DataField]
        public float Probability = 0.5f;

        [DataField]
        public string Loot = "RandomTrashDetectorSpawner";
        
        [DataField]
        public int RollsMin = 1;

        [DataField]
        public int RollsMax = 1;

        [DataField]
        public int RollsHardCap = 6;
    }
}
