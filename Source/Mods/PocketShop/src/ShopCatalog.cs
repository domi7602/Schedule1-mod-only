using System;
using System.Collections;
using System.Collections.Generic;
using Il2CppScheduleOne.ItemFramework;
using Il2CppScheduleOne.Levelling;
using Il2CppScheduleOne.UI.Shop;
using MelonLoader;
using PocketShop.Config;
using S1Mods.Shared;
using UnityEngine;
using EPaymentType = Il2CppScheduleOne.UI.Shop.ShopInterface.EPaymentType;

namespace PocketShop.Services;

/// <summary>
/// Snapshot of a single purchasable item, aggregating vanilla <see cref="ShopListing"/>
/// data into a POCO for easy binding to the phone-app UI. Holds a reference back
/// to the live <see cref="ShopListing"/> so <see cref="PurchaseService"/> can
/// decrement stock after a successful buy.
/// </summary>
public sealed class ItemPOCO
{
    public string Name { get; set; } = string.Empty;
    public float Price { get; set; }
    public int CurrentStock { get; set; }
    public bool IsInStock { get; set; }
    public StockState Stock { get; set; }
    public string ShopName { get; set; } = string.Empty;
    public string ShopCode { get; set; } = string.Empty;
    public string StableItemId { get; set; } = string.Empty;
    public int CategoryValue { get; set; }
    public StorableItemDefinition Definition { get; set; } = null!;
    public ShopListing SourceListing { get; set; } = null!;
    public ShopInterface SourceShop { get; set; } = null!;
    public Sprite? Icon { get; set; }

    /// <summary>
    /// v0.3.1: Payment rule of the shop this item is bought from (vanilla EPaymentType).
    /// Cached at catalog refresh — PurchaseService charges Cash or Card accordingly.
    /// </summary>
    public EPaymentType ShopPaymentType { get; set; } = EPaymentType.Online;

    /// <summary>Bug-Audit 2026-09-12: stable identity for stock-change dispatch.</summary>
    public string ItemId
    {
        get
        {
            try
            {
                if (Definition != null && Definition.Pointer != IntPtr.Zero && !Definition.WasCollected)
                    return Definition.ID ?? string.Empty;
            }
            catch { }
            return StableItemId ?? string.Empty;
        }
    }

    /// <summary>
    /// Whether the item is unlocked according to the player's rank/level.
    /// Evaluated dynamically against the live definition.
    /// </summary>
    public bool IsUnlocked
    {
        get
        {
            try
            {
                if (Definition != null && Definition.Pointer != IntPtr.Zero && !Definition.WasCollected)
                {
                    if (Definition.RequiresLevelToPurchase)
                    {
                        return Definition.IsUnlocked;
                    }
                }
            }
            catch { }
            return true;
        }
    }

    /// <summary>
    /// Whether this item requires a certain level/rank to purchase.
    /// </summary>
    public bool RequiresLevelToPurchase
    {
        get
        {
            try
            {
                if (Definition != null && Definition.Pointer != IntPtr.Zero && !Definition.WasCollected)
                {
                    return Definition.RequiresLevelToPurchase;
                }
            }
            catch { }
            return false;
        }
    }

    /// <summary>
    /// Clean display string of the required rank (e.g. "Hoodlum II").
    /// </summary>
    public string RequiredRankString
    {
        get
        {
            try
            {
                if (Definition != null && Definition.Pointer != IntPtr.Zero && !Definition.WasCollected)
                {
                    if (Definition.RequiresLevelToPurchase)
                    {
                        return FullRank.GetString(Definition.RequiredRank);
                    }
                }
            }
            catch { }
            return string.Empty;
        }
    }

    /// <summary>
    /// Numeric required rank (lower = earlier unlock); -1 when the item has no level gate.
    /// Used for the vanilla-like display order (unlocked first, then by rank).
    /// </summary>
    public int RequiredRankValue
    {
        get
        {
            try
            {
                if (Definition != null && Definition.Pointer != IntPtr.Zero && !Definition.WasCollected
                    && Definition.RequiresLevelToPurchase)
                {
                    return Definition.RequiredRank.GetRankIndex();
                }
            }
            catch { }
            return -1;
        }
    }

    /// <summary>
    /// Checks whether this item is currently available for purchase by the player,
    /// considering the EnforceLevelRequirements configuration.
    /// </summary>
    public bool IsAvailableToPlayer => !PocketShopConfig.EnforceLevelRequirementsStatic || IsUnlocked;
}

/// <summary>
/// Snapshot of a vanilla shop, aggregated from <see cref="ShopInterface.AllShops"/>.
/// Used for the drill-down shop list shown by <see cref="PocketShop.UI.ShopCatalogPane"/>.
/// </summary>
public sealed class ShopPOCO
{
    public string Name { get; set; } = string.Empty;
    public string ShopCode { get; set; } = string.Empty;
    public int ItemCount { get; set; }
    /// <summary>
    /// v0.3.1: Vanilla payment rule of this shop (EPaymentType: Cash / Online / PreferCash / PreferOnline).
    /// Black-market shops charge Cash, clean/legal shops charge by card (Online).
    /// </summary>
    public EPaymentType PaymentType { get; set; } = EPaymentType.Online;

    /// <summary>The live vanilla shop this tile was built from (used to re-resolve the gate).</summary>
    public ShopInterface SourceShop { get; set; } = null!;

    /// <summary>Unlock + opening-hours state, resolved per catalog refresh.</summary>
    public ShopGate Gate { get; set; } = new ShopGate();

    public bool IsLocked => Gate?.IsKnown == true && Gate.IsLocked;
    public bool IsOpen => Gate?.IsKnown == true && Gate.IsOpen;
    public bool IsAvailable => Gate?.CanPurchase == true;
    public bool GateKnown => Gate?.IsKnown == true;
    public bool HasSchedule => Gate?.HasSchedule ?? false;
    public string HoursText => Gate?.HoursText ?? string.Empty;
}

/// <summary>Verified live values read from the current registered shop and listing.</summary>
public readonly record struct LiveShopOffer(
    ShopInterface Shop,
    ShopListing Listing,
    StorableItemDefinition Definition,
    StockState Stock,
    float Price,
    EPaymentType PaymentType,
    ShopGate Gate);

/// <summary>
/// Aggregates items + shops from all vanilla shops via <see cref="ShopInterface.AllShops"/>.
/// Handles save-load timing by retrying until at least one live shop is registered.
/// </summary>
public static class ShopCatalog
{
    private static List<ItemPOCO> _itemCache = new();
    private static List<ShopPOCO> _shopCache = new();
    private static bool _initialised;

    private static int _retryCount;

    private static object? _retryHandle;
    public static event Action? OnCatalogChanged;

    public static IReadOnlyList<ItemPOCO> Items => _itemCache;
    public static IReadOnlyList<ShopPOCO> Shops => _shopCache;

    /// <summary>True after at least one registered shop has been read successfully, even if it has no listings.</summary>
    public static bool IsInitialized => _initialised;
    /// <summary>Compatibility property: true when the cached catalog has no purchasable item rows.</summary>
    public static bool IsEmpty => _itemCache.Count == 0;

    public static List<ItemPOCO> GetItemsOfShop(string shopCode)
    {
        var result = new List<ItemPOCO>();
        foreach (var item in _itemCache)
        {
            if (item.ShopCode == shopCode) result.Add(item);
        }
        return result;
    }

    /// <summary>Resolves a shop gate only from the currently registered live shop.</summary>
    public static bool TryGetGate(string shopCode, out ShopGate gate)
    {
        gate = new ShopGate();
        if (string.IsNullOrEmpty(shopCode)) return false;

        for (int i = 0; i < _shopCache.Count; i++)
        {
            var snapshot = _shopCache[i];
            if (snapshot == null || !string.Equals(snapshot.ShopCode, shopCode, StringComparison.Ordinal)) continue;
            if (!TryFindRegisteredShop(snapshot.SourceShop, snapshot.ShopCode, out var liveShop, out _)) return false;
            gate = ShopGateResolver.Resolve(liveShop, snapshot.ShopCode, snapshot.Name);
            return true;
        }
        return false;
    }

    /// <summary>
    /// Reads the current listing, owning shop, visibility, stock, price, payment rule, and gate.
    /// A cached POCO is never sufficient authority for a purchase.
    /// </summary>
    public static bool TryResolveLiveOffer(ItemPOCO item, out LiveShopOffer offer, out string failure)
    {
        offer = default;
        failure = "The listing is no longer available.";
        if (item == null || !NetworkGuard.IsAlive(item.SourceShop) || !IsListingAlive(item.SourceListing))
        {
            failure = "The shop listing has expired. Refresh the shop and try again.";
            return false;
        }

        string expectedItemId = item.StableItemId;
        if (string.IsNullOrEmpty(expectedItemId))
        {
            try
            {
                var snapshotDefinition = item.SourceListing.Item;
                if (NetworkGuard.IsAlive(snapshotDefinition)) expectedItemId = snapshotDefinition!.ID ?? string.Empty;
            }
            catch { expectedItemId = string.Empty; }
        }
        if (string.IsNullOrEmpty(expectedItemId))
        {
            failure = "The item identity could not be verified.";
            return false;
        }

        if (!TryFindRegisteredShop(item.SourceShop, item.ShopCode, out var shop, out failure)) return false;

        try
        {
            var listings = shop.Listings;
            if (listings == null || listings.Pointer == IntPtr.Zero || listings.WasCollected)
            {
                failure = "The shop listing list is unavailable.";
                return false;
            }

            ShopListing? listing = null;
            for (int i = 0; i < listings.Count; i++)
            {
                var candidate = listings[i];
                if (IsListingAlive(candidate) && candidate!.Pointer == item.SourceListing.Pointer)
                {
                    listing = candidate;
                    break;
                }
            }
            if (listing == null)
            {
                failure = "This item is no longer offered by the selected shop.";
                return false;
            }

            var owner = listing.Shop;
            if (!NetworkGuard.IsAlive(owner) || owner!.Pointer != shop.Pointer)
            {
                failure = "The listing no longer belongs to the selected shop.";
                return false;
            }

            var definition = listing.Item;
            if (!NetworkGuard.IsAlive(definition))
            {
                failure = "The item definition is no longer available.";
                return false;
            }
            string liveItemId = definition!.ID ?? string.Empty;
            if (!string.Equals(liveItemId, expectedItemId, StringComparison.OrdinalIgnoreCase))
            {
                failure = "The item changed while the shop was open. Refresh and try again.";
                return false;
            }

            bool shouldShow = listing.ShouldShow();
            bool limitedStock = listing.LimitedStock;
            int currentStock = listing.CurrentStock;
            var stock = PurchaseRules.ResolveStock(true, true, limitedStock, currentStock);
            // Vanilla may hide a sold-out limited listing. Keep it visible as OUT when the
            // selected live listing itself is still registered; purchases remain blocked by stock.CanBuy.
            if (!shouldShow && stock.Kind != StockKind.LimitedEmpty)
            {
                failure = "This item is no longer being offered.";
                return false;
            }

            float price = listing.Price;
            var paymentType = shop.PaymentType;
            var gate = ShopGateResolver.Resolve(shop, item.ShopCode, shop.ShopName);

            item.Name = ResolveCleanItemName(definition, listing);
            item.Price = price;
            item.CurrentStock = stock.Quantity;
            item.IsInStock = stock.CanBuy(1);
            item.Stock = stock;
            item.Definition = definition;
            item.ShopPaymentType = paymentType;

            offer = new LiveShopOffer(shop, listing, definition, stock, price, paymentType, gate);
            failure = string.Empty;
            return true;
        }
        catch (Exception ex)
        {
            failure = $"Live shop data could not be read: {ex.Message}";
            return false;
        }
    }

    private static bool TryFindRegisteredShop(ShopInterface source, string expectedCode, out ShopInterface liveShop, out string failure)
    {
        liveShop = null!;
        failure = "The shop is no longer registered.";
        if (!NetworkGuard.IsAlive(source)) return false;

        try
        {
            var allShops = ShopInterface.AllShops;
            if (allShops == null || allShops.Pointer == IntPtr.Zero || allShops.WasCollected) return false;
            for (int i = 0; i < allShops.Count; i++)
            {
                var candidate = allShops[i];
                if (!NetworkGuard.IsAlive(candidate) || candidate!.Pointer != source.Pointer) continue;

                string currentCode = ResolveShopCode(candidate, i);
                if (!string.Equals(currentCode, expectedCode, StringComparison.Ordinal))
                {
                    failure = "The shop identity changed while the app was open.";
                    return false;
                }

                liveShop = candidate;
                failure = string.Empty;
                return true;
            }
        }
        catch (Exception ex)
        {
            failure = $"The live shop registry could not be read: {ex.Message}";
            return false;
        }
        return false;
    }

    private static bool IsListingAlive(ShopListing? listing)
    {
        if (listing == null) return false;
        try { return listing.Pointer != IntPtr.Zero && !listing.WasCollected; }
        catch { return false; }
    }

    private static string ResolveShopCode(ShopInterface shop, int index)
    {
        if (!string.IsNullOrEmpty(shop.ShopCode)) return shop.ShopCode;
        if (!string.IsNullOrEmpty(shop.ShopName)) return shop.ShopName;
        return shop.name ?? $"shop_{index}";
    }

    public static void Refresh()
    {
        try
        {
            var allShops = ShopInterface.AllShops;
            if (allShops == null || allShops.Count == 0)
            {
                // The game is still registering shops (or is unloading). Keep the last good
                // snapshot intact; scene teardown explicitly resets it when appropriate.
                return;
            }

            var nextItems = new List<ItemPOCO>();
            var nextShops = new List<ShopPOCO>();
            int registeredCount = 0;
            bool refreshHadShopReadError = false;

            for (int s = 0; s < allShops.Count; s++)
            {
                var shop = allShops[s];
                if (!NetworkGuard.IsAlive(shop)) continue;
                registeredCount++;

                try
                {
                    var listings = shop!.Listings;
                    if (listings == null || listings.Pointer == IntPtr.Zero || listings.WasCollected)
                    {
                        refreshHadShopReadError = true;
                        MelonLogger.Warning($"PocketShop skipped shop with unreadable listings: {shop.ShopName}");
                        continue;
                    }

                    string code = ResolveShopCode(shop, s);
                    EPaymentType paymentType;
                    try { paymentType = shop.PaymentType; }
                    catch { paymentType = (EPaymentType)(-1); }

                    var gate = ShopGateResolver.Resolve(shop, code, shop.ShopName);
                    int availableCount = 0;

                    for (int i = 0; i < listings.Count; i++)
                    {
                        var listing = listings[i];
                        if (!IsListingAlive(listing)) continue;

                        try
                        {
                            var definition = listing!.Item;
                            if (!NetworkGuard.IsAlive(definition)) continue;
                            if (!listing.ShouldShow()) continue;

                            StockState stock;
                            try
                            {
                                stock = PurchaseRules.ResolveStock(true, true, listing.LimitedStock, listing.CurrentStock);
                            }
                            catch
                            {
                                stock = new StockState(StockKind.Unknown, 0, 0);
                            }

                            float price;
                            try { price = listing.Price; }
                            catch { price = float.NaN; }

                            string itemId;
                            try { itemId = definition!.ID ?? string.Empty; }
                            catch { itemId = string.Empty; }
                            if (string.IsNullOrEmpty(itemId)) continue;

                            nextItems.Add(new ItemPOCO
                            {
                                Name = ResolveCleanItemName(definition!, listing),
                                Price = price,
                                CurrentStock = stock.Quantity,
                                IsInStock = stock.CanBuy(1),
                                Stock = stock,
                                ShopName = shop.ShopName,
                                ShopCode = code,
                                StableItemId = itemId,
                                CategoryValue = (int)definition!.Category,
                                Definition = definition,
                                SourceListing = listing,
                                SourceShop = shop,
                                Icon = definition.Icon,
                                ShopPaymentType = paymentType
                            });

                            if (stock.CanBuy(1)) availableCount++;
                        }
                        catch (Exception ex)
                        {
                            MelonLogger.Warning($"PocketShop skipped an invalid listing in '{code}': {ex.Message}");
                        }
                    }

                    nextShops.Add(new ShopPOCO
                    {
                        Name = shop.ShopName,
                        ShopCode = code,
                        ItemCount = availableCount,
                        PaymentType = paymentType,
                        SourceShop = shop,
                        Gate = gate
                    });
                }
                catch (Exception ex)
                {
                    refreshHadShopReadError = true;
                    // A broken shop entry is isolated; do not discard the last fully committed snapshot.
                    MelonLogger.Warning($"PocketShop skipped an unreadable shop: {ex.Message}");
                }
            }

            if (refreshHadShopReadError || registeredCount == 0 || nextShops.Count == 0) return;

            // Commit only after the candidate snapshot was built. A failed refresh leaves the
            // previous good snapshot available for display, but purchases still revalidate live.
            _itemCache = nextItems;
            _shopCache = nextShops;
            _initialised = true;
            _retryCount = 0;

            var handlers = OnCatalogChanged?.GetInvocationList();
            if (handlers != null)
            {
                foreach (var handler in handlers)
                {
                    try { ((Action)handler)(); }
                    catch (Exception ex) { MelonLogger.Warning($"ShopCatalog subscriber failed: {ex.Message}"); }
                }
            }
        }
        catch (Exception ex)
        {
            // Preserve the last committed snapshot and allow the retry loop to try again.
            MelonLogger.Warning($"ShopCatalog refresh failed: {ex.Message}");
        }
    }

    /// <summary>
    /// Retries until at least one valid registered shop catalog is read. A shop with zero
    /// visible listings is still a successfully initialized catalog.
    /// </summary>
    public static void RetryIfEmpty()
    {
        if (_initialised) return;
        if (_retryHandle != null) return;
        if (_retryCount >= 20) return;

        _retryHandle = MelonCoroutines.Start(RetryLoop());
    }

    private static IEnumerator RetryLoop()
    {
        while (!_initialised && _retryCount < 20)
        {
            _retryCount++;
            Refresh();
            if (_initialised) break;
            yield return new WaitForSeconds(1.5f);
        }
        _retryHandle = null;
    }

    public static void StopRetryLoop()
    {
        if (_retryHandle != null)
        {
            MelonCoroutines.Stop(_retryHandle);
            _retryHandle = null;
        }
    }

    public static void ResetForSceneReload()
    {
        StopRetryLoop();
        _itemCache = new List<ItemPOCO>();
        _shopCache = new List<ShopPOCO>();
        _initialised = false;
        _retryCount = 0;
    }

    private static string ResolveCleanItemName(StorableItemDefinition def, ShopListing listing)
    {
        if (def != null && !string.IsNullOrEmpty(def.Name))
        {
            return def.Name;
        }
        if (def != null && !string.IsNullOrEmpty(def.ID))
        {
            return def.ID;
        }
        string raw = listing != null ? listing.name : string.Empty;
        if (string.IsNullOrEmpty(raw)) return "Item";

        int idx = raw.IndexOfAny(new[] { '(', '[', '$' });
        if (idx > 0)
        {
            return raw.Substring(0, idx).Trim();
        }
        return raw.Trim();
    }
}
