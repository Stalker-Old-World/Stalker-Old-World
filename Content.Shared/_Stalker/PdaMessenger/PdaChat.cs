using Robust.Shared.Serialization;

namespace Content.Shared._Stalker.PdaMessenger;

[Serializable, NetSerializable]
public sealed class PdaChat
{
    public readonly string Name;
    public readonly string? Receiver;
    public readonly string? Sender;

    public readonly List<PdaMessage> Messages = new();

    public PdaChat(string name, string? receiver = null, string? sender = null)
    {
        Name = name;
        Receiver = receiver;
        Sender = sender;
    }
}
