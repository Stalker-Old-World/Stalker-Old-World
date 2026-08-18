using Robust.Shared.GameStates;

namespace Content.Shared._Stalker.Psyonics;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class PsyonicsComponent : Component
{
    [DataField, ViewVariables(VVAccess.ReadWrite), AutoNetworkedField]
    public int Psy;

    [DataField, ViewVariables(VVAccess.ReadWrite), AutoNetworkedField]
    public int PsyMax = 300;
}
