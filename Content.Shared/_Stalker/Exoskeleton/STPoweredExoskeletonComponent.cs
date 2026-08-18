using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;

namespace Content.Shared._Stalker.Exoskeleton;

/// <summary>
/// Increase Maximum and Overload of user <see cref="STWeightComponent"/> when enabled. Consumes power if enabled.
/// Can be enabled only if equiped.
/// Need <see cref="PowerCellDrawComponent"/> and <see cref="PowerCellSlotComponent"/> for proper work.
/// </summary>
[RegisterComponent, NetworkedComponent]
public sealed partial class STPoweredExoskeletonComponent : Component
{
    public bool Enabled = false;

    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public EntProtoId ToggleAction = "ActionToggleExoskeleton";

    public EntityUid? ToggleActionUid;

    /// <summary>
    /// Value that added to Overload of user.
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public float OverloadChange = 0;

    /// <summary>
    /// Value that added to Maximum of user.
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public float MaximumChange = 0;
}
