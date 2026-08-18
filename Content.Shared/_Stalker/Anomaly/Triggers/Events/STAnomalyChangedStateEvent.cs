namespace Content.Shared._Stalker.Anomaly.Triggers.Events;

public sealed class STAnomalyChangedStateEvent(string previousState, string state) : EntityEventArgs
{
    public readonly string PreviousState = previousState;
    public readonly string State = state;
}
