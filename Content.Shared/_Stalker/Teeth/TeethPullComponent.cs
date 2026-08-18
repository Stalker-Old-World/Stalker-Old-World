using Content.Shared.DoAfter;
using Robust.Shared.Audio;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;

namespace Content.Shared._Stalker.Teeth;

// TODO: Probably separate tooth info from teeth pulling
[RegisterComponent]
public sealed partial class TeethPullComponent : Component
{
    // Base teeth info

    // Current count of teeth
    // This one is just using on initialization to setup respawnContainer data
    [DataField, ViewVariables]
    public int TeethCount = 5;

    // Proto to spawn for teeth pulling (probably will be used for something other in future)
    [DataField, ViewVariables]
    public EntProtoId TeethProto;

    /*
     * Teeth pulling info
     */

    // Item tag to use for pulling
    [DataField, ViewVariables]
    public string PullingItemTag;

    // Accent component without "Component" part
    [DataField]
    public string AccentComp = "LizardAccent";

    [DataField]
    public SoundSpecifier? PullSound;

    // Time its taking user of the item to pull a tooth
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public float PullTime = 5f;

    // Useless now, possibly useful in future
    [DataField, ViewVariables]
    public bool Pulled;

    [ViewVariables(VVAccess.ReadOnly)]
    public TimeSpan? ReviveTime;

    [DataField]
    public int InitialTeeth = 5;
}

[Serializable, NetSerializable]
public sealed partial class TeethPulledEvent : SimpleDoAfterEvent
{
}
