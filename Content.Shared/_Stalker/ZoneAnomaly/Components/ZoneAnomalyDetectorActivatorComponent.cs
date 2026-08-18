using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom;

namespace Content.Shared._Stalker.ZoneAnomaly.Components;

[RegisterComponent]
public sealed partial class ZoneAnomalyDetectorActivatorComponent : Component
{
    [DataField]
    public int Level;

    [DataField]
    public float Distance = 5f;

    [DataField]
    public int MaxCount;

    [DataField]
    public bool Enabled;

    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public TimeSpan ActivationDelay = TimeSpan.FromMinutes(1f);

    [DataField(customTypeSerializer: typeof(TimeOffsetSerializer)), ViewVariables(VVAccess.ReadWrite)]
    public TimeSpan NexActivationTime;
}
