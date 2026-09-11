using System;
using System.Collections;
using System.Collections.Generic;
using Il2CppScheduleOne.ItemFramework;
using Il2CppScheduleOne.UI.Shop;
using MelonLoader;
using UnityEngine;

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
    public string ShopName { get; set; } = string.Empty;
    public string ShopCode { get; set; } = string.Empty;
    public int CategoryValue { get; set; }
    public StorableItemDefinition Definition { get; set; } = null!;
    public ShopListing SourceListing { get; set; } = null!;
    public Sprite? Icon { get; set; }
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
}

/// <summary>
/// Aggregates items + shops from all vanilla shops via <see cref="ShopInterface.AllShops"/>.
/// Handles the save-load timing race condition by retrying until at least one shop is registered.
/// </summary>
public static class ShopCatalog
{
    private static readonly List<ItemPOCO> _itemCache = new();
    private static readonly List<ShopPOCO> _shopCache = new();
    private static bool _initialised;

    private static int _retryCount;

    private static object? _retryHandle;
    public static event Action? OnCatalogChanged;

    public static IReadOnlyList<ItemPOCO> Items => _itemCache;
    public static IReadOnlyList<ShopPOCO> Shops => _shopCache;

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

    public static void Refresh()
    {
        try
        {
            _itemCache.Clear();
            _shopCache.Clear();

            var allShops = ShopInterface.AllShops;
            if (allShops == null || allShops.Count == 0)
            {
                _initialised = false;
                return;
            }

            var perShopCount = new Dictionary<string, int>();

            for (int s = 0; s < allShops.Count; s++)
            {
                var shop = allShops[s];
                if (shop == null) continue;

                var listings = shop.Listings;
                if (listings == null) continue;

                // Fallback-Kette: ShopCode → ShopName → GameObject-Name → Slot-Index (nie null).
                var code = !string.IsNullOrEmpty(shop.ShopCode) ? shop.ShopCode
                    : !string.IsNullOrEmpty(shop.ShopName) ? shop.ShopName
                    : shop.name ?? $"shop_{s}";
                int availableCount = 0;

                for (int i = 0; i < listings.Count; i++)
                {
                    var listing = listings[i];
                    if (listing == null) continue;
                    if (listing.Item == null) continue;
                    // Unlimited listings (LimitedStock=false, z.B. mod-injiziert) gelten als In-Stock,
                    // auch wenn IsInStock/CurrentStock nie initialisiert wurden (Vanilla-Semantik).
                    // CurrentStock-Fallback deckt stale Saves ab (LimitedStock=true, IsInStock nie gesetzt,
                    // aber Stock > 0 vorhanden) — solche Listings waeren sonst unsichtbar.
                    bool inStock = listing.IsInStock || !listing.LimitedStock || listing.CurrentStock > 0;
                    if (!inStock) continue;

                    availableCount++;
                    var def = listing.Item;

                    _itemCache.Add(new ItemPOCO
                    {
                        Name = ResolveCleanItemName(def, listing),
                        Price = listing.Price,
                        CurrentStock = listing.CurrentStock,
                        IsInStock = listing.IsInStock,
                        ShopName = shop.ShopName,
                        ShopCode = code,
                        CategoryValue = (int)def.Category,
                        Definition = def,
                        SourceListing = listing,
                        Icon = def.Icon
                    });
                }

                // Shop-Tile immer anzeigen — auch bei 0 verfügbaren Items (Empty-State "OUT").
                // Vorher verschwand der Shop komplett aus dem Katalog, sobald alles ausverkauft war.
                perShopCount[code] = availableCount;
                _shopCache.Add(new ShopPOCO
                {
                    Name = shop.ShopName,
                    ShopCode = code,
                    ItemCount = availableCount
                });
            }

            _initialised = _itemCache.Count > 0;
            if (_initialised)
            {
                _retryCount = 0;
                // Per-Handler-Invoke: ein werfender Subscriber (z.B. tote UI nach
                // Szenen-Reload) darf weder die restlichen Handler noch Refresh()
                // abbrechen — sonst _initialised=false + StopRetryLoop = Katalog tot.
                var handlers = OnCatalogChanged?.GetInvocationList();
                if (handlers != null)
                {
                    foreach (var h in handlers)
                    {
                        try { ((Action)h)(); }
                        catch (Exception ex) { MelonLogger.Warning($"ShopCatalog subscriber failed: {ex.Message}"); }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            MelonLogger.Warning($"ShopCatalog refresh failed: {ex.Message}");
            _initialised = false;
            StopRetryLoop();
            return;
        }
    }

    /// <summary>
    /// Retries the refresh if the catalog is still empty. Safe to call every frame.
    /// Bails out after 20 attempts (~30s at 1.5s interval) to avoid spinning forever.
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
        _itemCache.Clear();
        _shopCache.Clear();
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
