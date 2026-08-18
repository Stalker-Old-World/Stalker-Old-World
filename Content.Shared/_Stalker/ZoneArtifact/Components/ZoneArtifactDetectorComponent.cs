using Robust.Shared.GameStates;

namespace Content.Shared._Stalker.ZoneArtifact.Components;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class ZoneArtifactDetectorComponent : Component
{
    [DataField, AutoNetworkedField, ViewVariables(VVAccess.ReadWrite)]
    public bool Available = true;

    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public float ActivationDistance = 2f;

    [DataField, AutoNetworkedField, ViewVariables(VVAccess.ReadWrite)]
    public float? ClosestDistance = null;

    [DataField, AutoNetworkedField, ViewVariables(VVAccess.ReadWrite)]
    public EntityUid? ClosestEntity = null;

    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public float DetectionDistance = 10f;

    [DataField, AutoNetworkedField, ViewVariables(VVAccess.ReadWrite)]
    public bool Enabled;

    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public int Level;

    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public TimeSpan UpdateInterval = TimeSpan.FromSeconds(0.05f);

    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public TimeSpan UpdateTime;
}
