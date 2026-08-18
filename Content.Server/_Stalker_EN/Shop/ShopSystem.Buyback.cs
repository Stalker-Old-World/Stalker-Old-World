using Content.Shared._Stalker.Shop;
using Content.Shared._Stalker.Shop.Prototypes;
using Content.Shared._Stalker_EN.Shop.Buyback;
using Content.Shared.FixedPoint;
using Content.Shared.GameTicking;
using Content.Shared.Store;
using Robust.Shared.Enums;
using Robust.Shared.Player;
using Robust.Shared.Prototypes;
namespace Content.Server._Stalker.Shop;

/// <summary>
/// Handles the buyback system: when players sell items, they can repurchase them
/// from a "Buyback" category at a configurable markup.
/// </summary>
public sealed partial class ShopSystem
{
    private const string BuybackCategoryLocId = "st-shop-buyback-category";
    private const int BuybackCategoryPriority = 999;

    private void InitializeBuyback()
    {
        SubscribeLocalEvent<ShopComponent, STBuybackPurchaseMessage>(OnBuybackPurchase);
        SubscribeLocalEvent<ShopComponent, ShopClosedMessage>(OnBuybackShopClosed);
        SubscribeLocalEvent<RoundRestartCleanupEvent>(OnBuybackRoundCleanup);
        _player.PlayerStatusChanged += OnBuybackPlayerStatusChanged;
    }

    private void ShutdownBuyback()
    {
        _player.PlayerStatusChanged -= OnBuybackPlayerStatusChanged;
    }

    /// <summary>
    /// Clears this player's buyback entries from the specific shop they closed.
    /// </summary>
    private void OnBuybackShopClosed(EntityUid uid, ShopComponent component, ShopClosedMessage msg)
    {
        if (msg.Actor is not { Valid: true } buyer)
            return;

        if (!TryComp<ActorComponent>(buyer, out var actor))
            return;

        component.BuybackItems.Remove(actor.PlayerSession.UserId);
    }

    /// <summary>
    /// Clears a disconnecting player's buyback entries from all shops.
    /// </summary>
    private void OnBuybackPlayerStatusChanged(object? sender, SessionStatusEventArgs args)
    {
        if (args.NewStatus != SessionStatus.Disconnected)
            return;

        var userId = args.Session.UserId;
        var query = EntityQueryEnumerator<ShopComponent>();
        while (query.MoveNext(out _, out var comp))
        {
            comp.BuybackItems.Remove(userId);
        }
    }

    private void AddBuybackEntry(
        EntityUid shop,
        EntityUid seller,
        ShopComponent component,
        string prototypeId,
        string name,
        string description,
        int perItemSellPrice,
        int count)
    {
        if (!TryComp<ActorComponent>(seller, out var actor))
            return;

        var userId = actor.PlayerSession.UserId;

        if (!component.BuybackItems.TryGetValue(userId, out var entries))
        {
            entries = new List<STBuybackEntry>();
            component.BuybackItems[userId] = entries;
        }

        var buybackPrice = (int) Math.Ceiling(perItemSellPrice * component.BuybackPriceMultiplier);

        for (var i = 0; i < count; i++)
        {
            entries.Add(new STBuybackEntry(
                component.BuybackNextId++,
                prototypeId,
                name,
                description,
                perItemSellPrice,
                buybackPrice));
        }

        if (entries.Count > component.BuybackMaxItems)
            entries.RemoveRange(0, entries.Count - component.BuybackMaxItems);
    }

    private CategoryInfo? GetBuybackCategory(EntityUid user, ShopComponent component)
    {
        if (!TryComp<ActorComponent>(user, out var actor))
            return null;

        var userId = actor.PlayerSession.UserId;

        if (!component.BuybackItems.TryGetValue(userId, out var entries) || entries.Count == 0)
            return null;

        var category = new CategoryInfo
        {
            Name = BuybackCategoryLocId,
            Priority = BuybackCategoryPriority,
        };

        foreach (var entry in entries)
        {
            var listing = new ListingData(
                name: entry.Name,
                discountCategory: null,
                description: entry.Description,
                conditions: null,
                icon: null,
                priority: 0,
                productEntity: entry.PrototypeId,
                productAction: null,
                productUpgradeId: null,
                productActionEntity: null,
                productEvent: null,
                raiseProductEventOnUser: false,
                purchaseAmount: 0,
                id: STBuybackConstants.IdPrefix + entry.Id,
                categories: new HashSet<ProtoId<StoreCategoryPrototype>>(),
                originalCost: new Dictionary<ProtoId<CurrencyPrototype>, FixedPoint2>
                {
                    [component.MoneyId] = entry.BuybackPrice,
                },
                restockTime: TimeSpan.Zero,
                dataDiscountDownTo: new Dictionary<ProtoId<CurrencyPrototype>, FixedPoint2>(),
                disableRefund: true,
                count: 1);

            category.ListingItems.Add(listing);
        }

        return category;
    }

    private void OnBuybackPurchase(EntityUid uid, ShopComponent component, STBuybackPurchaseMessage msg)
    {
        if (msg.Actor is not { Valid: true } buyer)
            return;

        if (!TryComp<ActorComponent>(buyer, out var actor))
            return;

        var userId = actor.PlayerSession.UserId;

        if (!component.BuybackItems.TryGetValue(userId, out var entries))
            return;

        var targetIndex = -1;
        for (var i = 0; i < entries.Count; i++)
        {
            if (entries[i].Id == msg.BuybackEntryId)
            {
                targetIndex = i;
                break;
            }
        }

        if (targetIndex < 0)
            return;

        var targetEntry = entries[targetIndex];

        var balance = GetMoneyFromList(GetContainersElements(buyer), component);
        if (balance < targetEntry.BuybackPrice)
            return;

        entries.RemoveAt(targetIndex);
        SubtractBalance(buyer, component, targetEntry.BuybackPrice);

        var product = Spawn(targetEntry.PrototypeId, Transform(buyer).Coordinates);
        _hands.PickupOrDrop(buyer, product);

        var newBalance = GetMoneyFromList(GetContainersElements(buyer), component);
        component.CurrentBalance = newBalance;
        UpdateShopUI(buyer, uid, newBalance, component);
    }

    private void OnBuybackRoundCleanup(RoundRestartCleanupEvent ev)
    {
        var query = EntityQueryEnumerator<ShopComponent>();
        while (query.MoveNext(out _, out var component))
        {
            component.BuybackItems.Clear();
            component.BuybackNextId = 0;
        }
    }
}
