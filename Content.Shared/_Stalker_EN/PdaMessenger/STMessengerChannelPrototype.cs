using Content.Shared._Stalker.Bands;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.Manager.Attributes;

namespace Content.Shared._Stalker_EN.PdaMessenger;

/// <summary>
/// Defines a public messenger channel available to all PDAs.
/// Channels are defined in YAML and displayed in the messenger's main page.
/// </summary>
[Prototype("stMessengerChannel")]
public sealed class STMessengerChannelPrototype : IPrototype
{
    [ViewVariables]
    [IdDataField]
    public string ID { get; } = string.Empty;

    /// <summary>
    /// Localized display name for this channel.
    /// </summary>
    [DataField(required: true)]
    public LocId Name { get; } = string.Empty;

    /// <summary>
    /// Display color for channel name in the messenger UI.
    /// </summary>
    [DataField]
    public Color Color { get; } = Color.White;

    /// <summary>
    /// Sort order in the channel list (lower = higher in list).
    /// </summary>
    [DataField]
    public int SortOrder { get; } = 0;

    /// <summary>
    /// Band prototype IDs that grant access to this channel.
    /// Empty list = open to all players.
    /// </summary>
    [DataField]
    public List<ProtoId<STBandPrototype>> RequiredBands { get; } = new();

    /// <summary>
    /// Whether users can send anonymous messages in this channel.
    /// </summary>
    [DataField]
    public bool AllowAnonymous { get; } = false;

    /// <summary>
    /// Whether messages in this channel are forwarded to the Discord webhook.
    /// </summary>
    [DataField]
    public bool BroadcastToDiscord { get; } = false;
}
