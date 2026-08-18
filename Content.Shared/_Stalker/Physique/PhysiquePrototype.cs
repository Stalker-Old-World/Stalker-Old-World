using Robust.Shared.Prototypes;

namespace Content.Shared._Stalker.Physique;

[Prototype("physique")]
public sealed partial class PhysiquePrototype : IPrototype
{
    [IdDataField]
    public string ID { get; } = default!;

    [DataField]
    public string Name = string.Empty;

    [DataField]
    public bool Selectable = true;
}
