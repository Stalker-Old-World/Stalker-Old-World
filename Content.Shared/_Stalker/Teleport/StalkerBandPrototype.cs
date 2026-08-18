using Robust.Shared.Prototypes;

namespace Content.Shared._Stalker.Teleport;

[Prototype("bandLoader")]
public sealed class StalkerBandPrototype : IPrototype
{
    [IdDataField]
    [ViewVariables]
    public string ID { get; } = default!;

    [DataField(serverOnly: true)]
    public HashSet<string> BandTeleports = new();
}
