namespace Content.Server._Stalker_EN.CharacterRank;

/// <summary>
/// Tracks in-memory state for a single character's rank progression.
/// </summary>
public sealed class CharacterRankTrackingData
{
    public Guid UserId;
    public string CharacterName = default!;
    public TimeSpan AccumulatedTime;
    public TimeSpan LastFlushTime;
    public bool IsActive;
}
