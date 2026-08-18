using System.Collections.Immutable;
using Robust.Shared.Serialization;

namespace Content.Shared.CartridgeLoader;

[Virtual]
[Serializable, NetSerializable]
public class CartridgeLoaderUiState : BoundUserInterfaceState
{
    public NetEntity? ActiveUI;
    public List<NetEntity> Programs;

    public BoundUserInterfaceState? ActiveProgramState; //stalker-en-change

    public CartridgeLoaderUiState(List<NetEntity> programs, NetEntity? activeUI, BoundUserInterfaceState? activeProgramState = null)
    {
        Programs = programs;
        ActiveUI = activeUI;
        ActiveProgramState = activeProgramState;
    }
}
