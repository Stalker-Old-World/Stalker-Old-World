using Robust.Shared.Timing;

namespace Content.Server._Stalker.SinMobController;

[RegisterComponent]
public sealed partial class SinMobResistantComponent : Component
{
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public string Faction = "Stalker";

    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public TimeSpan TimeBeforeRemove = TimeSpan.FromSeconds(120);
}
