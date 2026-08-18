using Robust.Shared.GameStates;

namespace Content.Shared._Stalker.Psyonics;

[RegisterComponent, NetworkedComponent]
public sealed partial class PsyonicsAbsorbableComponent : Component
{
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public string VerbAction = "psy-absorb-artifact-action";
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public string VerbPopup = "psy-absorb-artifact-popup";
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public int PsyRecovery = 50;
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public bool IsPersistent = false;
}
