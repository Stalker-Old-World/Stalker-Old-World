namespace Content.Server._Stalker.ZoneArtifact.Components.Spawner;

[RegisterComponent]
public sealed partial class ZoneArtifactSpawnerMapTierComponent : Component
{
    [DataField]
    public int MinTier = int.MinValue;

    [DataField]
    public int MaxTier = int.MaxValue;
}
