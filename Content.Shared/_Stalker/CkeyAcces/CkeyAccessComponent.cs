using Robust.Shared.GameStates;

namespace Content.Server.CkeyAccess.Components;

[RegisterComponent, NetworkedComponent]
public sealed partial class CkeyAccessComponent : Component
{
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public string OwnerName = "nobody";

}
