namespace Content.Server._Stalker.Restart;

[RegisterComponent]
public partial class RestartComponent : Component
{
    [DataField]
    public TimeSpan Time;

    [DataField]
    public TimeSpan IntervalDelay = TimeSpan.FromMinutes(1f);

    [DataField]
    public TimeSpan IntervalLast;
}
