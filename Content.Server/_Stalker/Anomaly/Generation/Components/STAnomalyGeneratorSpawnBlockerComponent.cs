namespace Content.Server._Stalker.Anomaly.Generation.Components;

[RegisterComponent]
public sealed partial class STAnomalyGeneratorSpawnBlockerComponent : Component
{
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public int Size = 5;
}
