using Content.Shared.Whitelist;

namespace Content.Shared._Stalker.ZoneAnomaly.Effects.Components;

[RegisterComponent]
public sealed partial class ZoneAnomalyEffectDestroyComponent : Component
{
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public EntityWhitelist Whitelist = new();

    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public float Lifetime = 0.01f;
}
