using Robust.Shared.GameObjects;

namespace Content.Server.CartridgeLoader.Events;

/// <summary>
/// Event raised to get the current UI state of a cartridge program synchronously.
/// This allows the server to include the program's state in the same message as the cartridge loader state.
/// </summary>
public sealed class CartridgeGetStateEvent : EntityEventArgs
{
    public EntityUid LoaderUid;
    public BoundUserInterfaceState? State;

    public CartridgeGetStateEvent(EntityUid loaderUid)
    {
        LoaderUid = loaderUid;
    }
}
