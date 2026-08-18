using Content.Shared.CartridgeLoader;
using Robust.Shared.Serialization;

namespace Content.Shared._Stalker.PdaMessenger;

[Serializable, NetSerializable]
public sealed class MessengerUiMessageEvent : CartridgeMessageEvent
{
    public readonly PdaMessage Message;

    public MessengerUiMessageEvent(PdaMessage message)
    {
        Message = message;
    }
}

[Serializable, NetSerializable]
public sealed class MessengerUiSetLoginEvent : CartridgeMessageEvent
{
    public readonly string Owner;

    public MessengerUiSetLoginEvent(string owner)
    {
        Owner = owner;
    }
}

[Serializable, NetSerializable]
public sealed class MessengerUiState : BoundUserInterfaceState
{
    public readonly List<PdaChat> Chats;

    public MessengerUiState(List<PdaChat> chats)
    {
        Chats = chats;
    }
}
