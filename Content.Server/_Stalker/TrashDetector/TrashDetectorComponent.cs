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
    }
}
