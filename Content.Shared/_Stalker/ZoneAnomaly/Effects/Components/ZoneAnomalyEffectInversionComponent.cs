using Content.Shared.Whitelist;

namespace Content.Shared._Stalker.ZoneAnomaly.Effects.Components;

[RegisterComponent]
public sealed partial class ZoneAnomalyEffectInversionComponent : Component
{
    [DataField]
    public EntityWhitelist Whitelist = new();

    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public float Distance = 25f;

    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public float Speed = 25f;

    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public float Radial = 10f;

    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public float Tangential;
}
