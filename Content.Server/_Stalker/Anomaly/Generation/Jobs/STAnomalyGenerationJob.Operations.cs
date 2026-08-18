using System.Threading.Tasks;

namespace Content.Server._Stalker.Anomaly.Generation.Jobs;

public sealed partial class STAnomalyGenerationJob
{
    /// <summary>
    /// Default value, for all operations in generation.
    /// </summary>
    private const int Operations = 500;

    /// <summary>
    /// Used in method <see cref="MakeOperation"/>
    /// to maintain the number of committed to suspend Job activity,
    /// to maintain productivity.
    /// </summary>
    private int _operationCounter;

    private async Task MakeOperation(int operations = -1)
    {
        var maxOperations = operations == -1 ? Operations : operations;

        if (_operationCounter > maxOperations)
        {
            _operationCounter -= maxOperations;
            await SuspendNow();
        }

        _operationCounter++;
    }
}
