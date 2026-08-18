namespace Content.Server.MemoryVaccine.Components
{
    [RegisterComponent]
    public sealed partial class MemoryVaccineComponent : Component
    {
        [DataField]
        public float UseTime = 5f;

    }
}
