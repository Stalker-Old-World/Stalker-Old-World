using Robust.Shared.GameStates;

namespace Content.Shared._Stalker.ZoneArtifact.Components;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class ZoneArtifactDetectorDistanceIndicatorComponent : Component
{
    [DataField, AutoNetworkedField]
    public bool Enabled;

    [DataField, AutoNetworkedField]
    public float? Distance;
}
