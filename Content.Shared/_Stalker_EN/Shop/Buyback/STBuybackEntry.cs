using Robust.Shared.Serialization;

namespace Content.Shared._Stalker_EN.Shop.Buyback;

/// <summary>
/// Represents a single item available for buyback after being sold to a shop.
/// Stored per-player in the shop component's buyback dictionary.
/// </summary>
[Serializable, NetSerializable]
public readonly record struct STBuybackEntry(
    uint Id,
    string PrototypeId,
    string Name,
    string Description,
    int OriginalSellPrice,
    int BuybackPrice);
