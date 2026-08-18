using Robust.Shared.Serialization;

namespace Content.Shared._Stalker_EN.BulletinBoard;

/// <summary>
/// Generic offer category for bulletin boards.
/// Primary and Secondary meanings are defined per board type via localization.
/// </summary>
[Serializable, NetSerializable]
public enum STBulletinCategory : byte
{
    /// <summary>Primary category (e.g. "Services" on merc board, "Selling" on trade board).</summary>
    Primary = 0,

    /// <summary>Secondary category (e.g. "Jobs" on merc board, "Buying" on trade board).</summary>
    Secondary = 1,
}
