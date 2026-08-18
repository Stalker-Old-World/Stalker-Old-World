namespace Content.Shared._Stalker.ZoneAnomaly.Effects.Components;

[RegisterComponent]
public sealed partial class ZoneAnomalyEffectRemoveItemComponent : Component
{
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public int Count = 1;
}
