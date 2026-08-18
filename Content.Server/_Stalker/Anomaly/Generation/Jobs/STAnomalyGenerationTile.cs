using Content.Shared._Stalker.Anomaly.Prototypes;
using Robust.Shared.Map;
using Robust.Shared.Prototypes;

namespace Content.Server._Stalker.Anomaly.Generation.Jobs;

public sealed class STAnomalyGenerationTile
{
    public readonly TileRef TileRef;

    public float Weight = 1f;
    public ProtoId<STAnomalyNaturePrototype> Nature = string.Empty;

    public STAnomalyGenerationTile(TileRef tileRef)
    {
        TileRef = tileRef;
    }
}
