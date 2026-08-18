using Robust.Shared.Network;

namespace Content.Server._Stalker.Stagger;

[RegisterComponent]
public sealed partial class StaggerComponent : Component
{
    [DataField]
    public NetUserId? NetUserId;

    [DataField]
    public float SlownessDistanceMin = -2.5f;

    [DataField]
    public float SlownessDistanceMax = 3.5f;

    [DataField]
    public float MovementSpeedModifier = 1f;
}
