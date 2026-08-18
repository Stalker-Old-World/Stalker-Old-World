using Content.Shared.Whitelist;

namespace Content.Shared._Stalker.ZoneAnomaly.Effects.Components;

[RegisterComponent]
public sealed partial class ZoneAnomalyEffectActivatorComponent : Component
{
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public EntityWhitelist Whitelist = new();

    [DataField]
    public float Distance = 8f;
}
