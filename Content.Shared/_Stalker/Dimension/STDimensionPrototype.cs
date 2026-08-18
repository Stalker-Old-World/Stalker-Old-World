using Robust.Shared.Prototypes;
using Robust.Shared.Utility;

namespace Content.Shared._Stalker.Dimension;

[Prototype("stDimension")]
public sealed class STDimensionPrototype : IPrototype
{
    [IdDataField]
    public string ID { get; } = string.Empty;

    [DataField]
    public ResPath MapPath = new("/Maps/_ST/Anomaly/bubble_small.yml");
}
