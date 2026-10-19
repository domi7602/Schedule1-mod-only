extern alias il2cpp;

using System;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppScheduleOne.UI.Shop;
using UnityEngine;

namespace S1Mods.Shared;

/// <summary>
/// Makes runtime-injected <see cref="ShopListing"/>s visible in the vanilla shop UI.
/// A raw <c>Listings.Add()</c> is not enough: the vanilla shop renders <c>ListingUI</c> rows
/// (plus buy/sell/hover event hooks) built around shop build/open, so listings injected
/// afterwards additionally need <c>Initialize()</c> and — if the row collection is already
/// built — a manually created row. Mirrors S1API's ShopIntegration.AddItemToShop without
/// taking a dependency on S1API. Every step is best-effort: failures degrade to
/// "listing present, row missing", never a crash.
/// </summary>
public static class ShopListingSync
{
    /// <summary>Call once right after adding a new listing to <c>shop.Listings</c>.</summary>
    /// <remarks>
    /// Order mirrors S1API's <c>ShopIntegration.AddItemToShop</c>: <c>Add</c> →
    /// <c>Initialize(shop)</c> → UI row. <c>Initialize</c> derives runtime stock from
    /// <c>DefaultStock</c>, so any explicit <c>SetStock</c> must run <i>after</i> it
    /// (a pre-Initialize SetStock is silently overwritten). Stock is only forced for
    /// <c>LimitedStock</c> listings — unlimited ones legitimately report
    /// <c>CurrentStock == 0</c> until first initialized.
    /// </remarks>
    public static void OnListingAdded(
        ShopInterface shop,
        ShopListing listing,
        int defaultStock,
        Action<string>? logInfo = null,
        Action<string>? logWarn = null)
    {
        if (!IsAlive(shop) || !IsAlive(listing)) return;
        try { listing.Initialize(shop); }
        catch (Exception ex) { logWarn?.Invoke($"ShopListingSync: Initialize failed: {ex.Message}"); }
        try
        {
            if (listing.LimitedStock && listing.CurrentStock <= 0)
                listing.SetStock(defaultStock, true);
        }
        catch (Exception ex) { logWarn?.Invoke($"ShopListingSync: SetStock failed: {ex.Message}"); }
        EnsureListingRow(shop, listing, logWarn);
        try
        {
            string id = listing.Item != null ? listing.Item.ID : "?";
            logInfo?.Invoke($"ShopListingSync: '{id}' ready in '{shop.ShopName}'.");
        }
        catch { }
    }

    /// <summary>Idempotent repair path for already-present listings: refreshes stock + row.</summary>
    public static void OnListingExists(
        ShopInterface shop,
        ShopListing listing,
        int defaultStock,
        Action<string>? logWarn = null)
    {
        if (!IsAlive(shop) || !IsAlive(listing)) return;
        try
        {
            if (listing.LimitedStock && listing.CurrentStock <= 0)
                listing.SetStock(defaultStock, true);
        }
        catch { }
        EnsureListingRow(shop, listing, logWarn);
    }

    private static bool IsAlive(UnityEngine.Object? obj)
    {
        if (obj == null) return false;
        try
        {
            if (obj.Pointer == IntPtr.Zero || obj.WasCollected) return false;
            return (UnityEngine.Object)obj != null;
        }
        catch { return false; }
    }

    /// <summary>Liveness check for plain Il2Cpp objects (e.g. ShopListing is NOT a UnityEngine.Object).</summary>
    private static bool IsAlive(Il2CppObjectBase? obj)
    {
        if (obj == null) return false;
        try { return obj.Pointer != IntPtr.Zero && !obj.WasCollected; }
        catch { return false; }
    }

    private static void EnsureListingRow(ShopInterface shop, ShopListing listing, Action<string>? logWarn)
    {
        try
        {
            var rows = shop.listingUI;
            // UI not built yet: vanilla builds rows at first open from Listings, and our listing
            // is already initialized above, so it will be picked up — nothing to do here.
            if (rows == null || rows.Count == 0) return;

            string itemId = listing.Item != null ? listing.Item.ID : string.Empty;
            for (int i = 0; i < rows.Count; i++)
            {
                var row = rows[i];
                if (!IsAlive(row) || !IsAlive(row.Listing) || !IsAlive(row.Listing.Item)) continue;
                if (!string.IsNullOrEmpty(itemId)
                    && string.Equals(row.Listing.Item.ID, itemId, StringComparison.OrdinalIgnoreCase)) return; // row present
            }

            if (!IsAlive(shop.ListingUIPrefab) || !IsAlive(shop.ListingContainer)) return;

            // Same creation path as S1API ShopIntegration.CreateListingUI.
            var uiObject = UnityEngine.Object.Instantiate(shop.ListingUIPrefab.gameObject, shop.ListingContainer);
            var listingUI = uiObject.GetComponent<ListingUI>();
            if (listingUI == null)
            {
                UnityEngine.Object.Destroy(uiObject);
                return;
            }

            listingUI.Initialize(listing);
            BindListingUIEvents(shop, listingUI);
            rows.Add(listingUI);
            try
            {
                var selectable = listingUI.Selectable;
                if (IsAlive(shop.listingPanel) && IsAlive(selectable))
                    shop.listingPanel.AddSelectable(selectable);
            }
            catch (Exception ex) { logWarn?.Invoke($"ShopListingSync: AddSelectable failed: {ex.Message}"); }
        }
        catch (Exception ex) { logWarn?.Invoke($"ShopListingSync: UI row failed: {ex.Message}"); }
    }

    private static void BindListingUIEvents(ShopInterface shop, ListingUI listingUI)
    {
        // NOTE: the il2cpp:: alias is mandatory here. Directory.Build.props aliases
        // Il2Cppmscorlib as il2cpp, while Il2CppSystem.Core is referenced WITHOUT alias —
        // a plain Il2CppSystem.Action resolves to the Core assembly and fails the build
        // (CS0305/CS0234). S1API can use the plain name only because its own build
        // aliases the assemblies differently. Same Combine sequence as S1API otherwise.
        listingUI.onAddItem = il2cpp::Il2CppSystem.Delegate.Combine(
            listingUI.onAddItem,
            DelegateSupport.ConvertDelegate<il2cpp::Il2CppSystem.Action>(
                new Action(() => shop.AddItem(listingUI))))
            .Cast<il2cpp::Il2CppSystem.Action>();
        listingUI.onRemoveItem = il2cpp::Il2CppSystem.Delegate.Combine(
            listingUI.onRemoveItem,
            DelegateSupport.ConvertDelegate<il2cpp::Il2CppSystem.Action>(
                new Action(() => shop.RemoveItem(listingUI))))
            .Cast<il2cpp::Il2CppSystem.Action>();
        listingUI.onSetAmount = il2cpp::Il2CppSystem.Delegate.Combine(
            listingUI.onSetAmount,
            DelegateSupport.ConvertDelegate<il2cpp::Il2CppSystem.Action<int>>(
                new Action<int>(amount => shop.SetAmount(listingUI, amount))))
            .Cast<il2cpp::Il2CppSystem.Action<int>>();
        listingUI.onAdjustAmount = il2cpp::Il2CppSystem.Delegate.Combine(
            listingUI.onAdjustAmount,
            DelegateSupport.ConvertDelegate<il2cpp::Il2CppSystem.Action<int>>(
                new Action<int>(amount => shop.AdjustAmount(listingUI, amount))))
            .Cast<il2cpp::Il2CppSystem.Action<int>>();
        listingUI.hoverStart = il2cpp::Il2CppSystem.Delegate.Combine(
            listingUI.hoverStart,
            DelegateSupport.ConvertDelegate<il2cpp::Il2CppSystem.Action>(
                new Action(() => shop.EntryHovered(listingUI))))
            .Cast<il2cpp::Il2CppSystem.Action>();
        listingUI.hoverEnd = il2cpp::Il2CppSystem.Delegate.Combine(
            listingUI.hoverEnd,
            DelegateSupport.ConvertDelegate<il2cpp::Il2CppSystem.Action>(
                new Action(() => shop.EntryUnhovered())))
            .Cast<il2cpp::Il2CppSystem.Action>();
    }
}
