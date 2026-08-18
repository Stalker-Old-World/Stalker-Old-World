using Robust.Shared.Serialization;

namespace Content.Shared._Stalker.ZoneAnomaly;

[Serializable, NetSerializable]
public enum ZoneAnomalyState
{
    Idle,
    Activated,
    Charging,
    Preparing,
}
