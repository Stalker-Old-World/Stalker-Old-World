using Robust.Shared.Serialization;

namespace Content.Shared._Stalker_EN.BulletinBoard;

/// <summary>
/// A single offer on a bulletin board. Immutable record sent over the network
/// as part of <see cref="STBulletinUiState"/>.
/// </summary>
[Serializable, NetSerializable]
public sealed class STBulletinOffer
{
    /// <summary>Formats an offer ID as a bracketed reference string (e.g. "[MB#3]").</summary>
    public static string FormatRef(string prefix, uint id) => $"[{prefix}{id}]";

    /// <summary>Unique offer ID (globally unique across all board types within the round).</summary>
    public readonly uint Id;

    /// <summary>Whether this is a primary or secondary category offer.</summary>
    public readonly STBulletinCategory Category;

    /// <summary>The offer reference prefix stored at creation time (e.g. "MB#", "TB#").</summary>
    public readonly string OfferRefPrefix;

    /// <summary>In-game character name of the poster.</summary>
    public readonly string PosterName;

    /// <summary>
    /// The poster's unique messenger ID (e.g. "472-819").
    /// Null if the poster has no messenger ID.
    /// </summary>
    public readonly string? PosterMessengerId;

    /// <summary>
    /// Faction name of the poster (resolved at post time via BandsComponent).
    /// Null if the poster has no faction.
    /// </summary>
    public readonly string? PosterFaction;

    /// <summary>
    /// JobIcon prototype ID for the poster's rank icon (resolved at post time).
    /// Null if the poster has no rank.
    /// </summary>
    public readonly string? PosterRankIcon;

    /// <summary>Free-text description of the offer.</summary>
    public readonly string Description;

    /// <summary>Server CurTime when the offer was posted. Used client-side for live elapsed time.</summary>
    public readonly TimeSpan Timestamp;

    public STBulletinOffer(
        uint id,
        STBulletinCategory category,
        string offerRefPrefix,
        string posterName,
        string? posterMessengerId,
        string? posterFaction,
        string? posterRankIcon,
        string description,
        TimeSpan timestamp)
    {
        Id = id;
        Category = category;
        OfferRefPrefix = offerRefPrefix;
        PosterName = posterName;
        PosterMessengerId = posterMessengerId;
        PosterFaction = posterFaction;
        PosterRankIcon = posterRankIcon;
        Description = description;
        Timestamp = timestamp;
    }
}
