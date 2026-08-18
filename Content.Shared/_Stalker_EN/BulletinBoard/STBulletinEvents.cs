using Content.Shared.CartridgeLoader;
using Robust.Shared.GameObjects;
using Robust.Shared.Serialization;

namespace Content.Shared._Stalker_EN.BulletinBoard;

/// <summary>
/// Client requests posting a new offer to a bulletin board.
/// </summary>
[Serializable, NetSerializable]
public sealed class STBulletinPostOfferEvent : CartridgeMessageEvent
{
    public readonly STBulletinCategory Category;
    public readonly string Description;

    public STBulletinPostOfferEvent(STBulletinCategory category, string description)
    {
        Category = category;
        Description = description;
    }
}

/// <summary>
/// Client requests withdrawing one of their own offers from the board.
/// </summary>
[Serializable, NetSerializable]
public sealed class STBulletinWithdrawOfferEvent : CartridgeMessageEvent
{
    public readonly uint OfferId;

    public STBulletinWithdrawOfferEvent(uint offerId)
    {
        OfferId = offerId;
    }
}

/// <summary>
/// Client requests adding the poster of an offer as a messenger contact.
/// </summary>
[Serializable, NetSerializable]
public sealed class STBulletinContactPosterEvent : CartridgeMessageEvent
{
    public readonly string PosterMessengerId;
    public readonly uint OfferId;

    public STBulletinContactPosterEvent(string posterMessengerId, uint offerId)
    {
        PosterMessengerId = posterMessengerId;
        OfferId = offerId;
    }
}

/// <summary>
/// Client requests toggling mute on a bulletin board cartridge (suppresses ringer, badge still appears).
/// </summary>
[Serializable, NetSerializable]
public sealed class STBulletinToggleMuteEvent : CartridgeMessageEvent { }

/// <summary>
/// Local by-ref entity event raised on a bulletin board cartridge entity to request
/// opening a specific offer. Decouples messenger → bulletin board dependency.
/// </summary>
[ByRefEvent]
public record struct STOpenBulletinOfferEvent(EntityUid LoaderUid, uint OfferId)
{
    /// <summary>Set to true by the handler that activates its program, preventing other boards from also activating.</summary>
    public bool Handled;
}
