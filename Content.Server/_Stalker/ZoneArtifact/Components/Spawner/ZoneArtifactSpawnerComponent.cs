using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom;

namespace Content.Server._Stalker.ZoneArtifact.Components.Spawner;

[RegisterComponent]
public sealed partial class ZoneArtifactSpawnerComponent : Component
{
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public EntProtoId? Artifact;

    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public List<EntityArtifactSpawnEntry> Artifacts = new();

    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public TimeSpan ResumptionDelay = TimeSpan.FromMinutes(15f);

    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public TimeSpan ResumptionTime;

    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public bool RestoreOnReady = true;
}

[Serializable, DataDefinition]
public partial struct EntityArtifactSpawnEntry
{
    [DataField("id"), ViewVariables(VVAccess.ReadWrite)]
    public EntProtoId? PrototypeId = null;

    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public int Tier = 1;

    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public float Ratio = 1;

    public EntityArtifactSpawnEntry() { }
}
