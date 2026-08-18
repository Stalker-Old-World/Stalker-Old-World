using Robust.Shared.Prototypes;

namespace Content.Shared._Stalker.Pack;

[Prototype("stPack")]
public sealed class STPackPrototype : IPrototype
{
    [IdDataField]
    public string ID { get; } = string.Empty;

    [DataField]
    public EntProtoId Head;

    [DataField]
    public HashSet<EntProtoId> Members = new();

    [DataField]
    public int MinMemberCount = 4;

    [DataField]
    public int MaxMemberCount = 8;
}
