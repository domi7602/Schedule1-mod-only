using System;
using MelonLoader;
using UnityEngine;
using S1API.Items.Buildable;
using SnackVendor.World;
using BuildableItemCreator = S1API.Items.Buildable.BuildableItemCreator;
using BuildableItemDefinitionBuilder = S1API.Items.Buildable.BuildableItemDefinitionBuilder;
using GameRegistry = Il2CppScheduleOne.Registry;
using NativeBuildableItemDef = Il2CppScheduleOne.ItemFramework.BuildableItemDefinition;
using NativeStorableItemDef = Il2CppScheduleOne.ItemFramework.StorableItemDefinition;

namespace SnackVendor.Items;

/// <summary>
/// Registers a 1×1 player-stocked snack-vending-machine BuildableItem with S1API,
/// injects the hardware-store catalogue listing (so the player can buy and place
/// it), and provides a Ghost-Visual for the place-mode. Mirrors AutoPack's
/// ItemFactory but leans on the simpler bases (`storage` / `pot`) so the
/// inherited GridItem footprint stays at 1×1.
/// </summary>
public static class SnackVendorItemFactory
{
    private static bool _isRegistered = false;

    public static void RegisterItem()
    {
        // Gatekeeper-fix (mirrors AutoPack): re-verify in Registry so a patch-day
        // or save-slot switch cannot leave us with a stale registration flag.
        string itemId = Mod.CurrentConfig.StationItemId;
        if (_isRegistered && GameRegistry.ItemExists(itemId)) return;
        _isRegistered = false;

        try
        {
            if (GameRegistry.ItemExists(itemId))
            {
                Mod.Log.Info($"Item '{itemId}' already exists in Registry.");
                _isRegistered = true;
                return;
            }

            Mod.Log.Info($"Registering custom buildable item '{itemId}'...");

            // Bases that give us a sane 1×1 GridItem + small footprint.
            // 'storage' is a chest (small), 'pot' is the smallest plant pot,
            // both are typical 1×1 station anchors.
            string[] candidateBases = new[] { "storage", "pot", "dryingrack", "packagingstation" };
            string? baseId = null;
            foreach (var candidate in candidateBases)
            {
                if (GameRegistry.ItemExists(candidate))
                {
                    var def = GameRegistry.GetItem(candidate);
                    if (def != null && def.Pointer != IntPtr.Zero && def.TryCast<NativeBuildableItemDef>() != null)
                    {
                        baseId = candidate;
                        break;
                    }
                }
            }

            BuildableItemDefinitionBuilder builder;
            if (!string.IsNullOrEmpty(baseId))
            {
                Mod.Log.Info($"Cloning SnackVendor definition from base item '{baseId}' (inherits GridItem & footprint)...");
                builder = BuildableItemCreator.CloneFrom(baseId);
            }
            else
            {
                Mod.Log.Warn("No candidate base item found in Registry — falling back to CreateBuilder (grid frame still applied below).");
                builder = BuildableItemCreator.CreateBuilder();
            }

            builder.WithBasicInfo(
                       itemId,
                       Mod.CurrentConfig.StationDisplayName,
                       Mod.CurrentConfig.StationDescription,
                       S1API.Items.ItemCategory.Furniture)
                   .WithPricing(Mod.CurrentConfig.StationPurchasePrice, Mod.CurrentConfig.StationResellMultiplier)
                   .WithIcon(BuildOrLoadGhostPrefab.GetOrCreateIconSprite())
                   .WithGhostVisual((Transform parent) =>
                   {
                       var ghost = BuildOrLoadGhostPrefab.CreateGhostPrefab();
                       ghost.transform.SetParent(parent, false);
                       ghost.transform.localPosition = Vector3.zero;
                       ghost.transform.localRotation = Quaternion.identity;
                       ghost.name = "SnackVendor_Ghost";
                       return ghost;
                   }, replaceExistingVisual: true);

            var defWrapper = builder.Build();
            defWrapper.RequiresLevelToPurchase = false;

            _isRegistered = true;
            Mod.Log.Info($"Successfully registered item '{itemId}' (unlocked for all ranks).");
        }
        catch (Exception ex)
        {
            Mod.Log.Error($"Failed to register SnackVendor item", ex);
        }
    }

    public static void OnSaveInfoLoaded() => RegisterItem();

    /// <summary>
    /// Configures an already-placed station GameObject for placement paths
    /// that instantiate the BuiltItem prefab directly and bypass the vanilla
    /// BuildableItem.Start flow — currently the HomelessMod street-placement
    /// path. Mirrors AutoPack's SetupPlacedStation contract: attach the
    /// controller, run the full setup (vanilla VendingMachine clone + mesh
    /// swap + stock restore). No-op when a controller already exists.
    /// Invoked from HomelessMod via reflection (type is resolved by name).
    /// </summary>
    public static void SetupPlacedStation(GameObject go, string? guid = null)
    {
        if (go == null || go.Pointer == IntPtr.Zero) return;

        try
        {
            var existing = go.GetComponent<SnackVendorController>();
            if (existing != null)
            {
                // Restore path / double entry: keep the live controller, only
                // backfill the GUID when the restore data carries one.
                if (!string.IsNullOrEmpty(guid) && string.IsNullOrEmpty(existing.InstanceGuid))
                {
                    existing.InstanceGuid = guid;
                }
                Mod.Log.Info("SetupPlacedStation: controller already present — reusing it.");
                return;
            }

            var ctrl = go.AddComponent<SnackVendorController>();
            if (!string.IsNullOrEmpty(guid)) ctrl.InstanceGuid = guid;
            ctrl.SetupAfterPlacementExternal();
            Mod.Log.Info("SetupPlacedStation: controller attached via street placement.");
        }
        catch (Exception ex)
        {
            Mod.Log.Error("SetupPlacedStation failed", ex);
        }
    }

    private static int _injectRetryCount = 0;
    private const int MaxInjectRetries = 10;

    public static void InjectHardwareStoreListing()
    {
        if (!Mod.CurrentConfig.InjectIntoHardwareShop) return;

        try
        {
            string itemId = Mod.CurrentConfig.StationItemId;
            var def = GameRegistry.GetItem(itemId);
            if (def == null || def.Pointer == IntPtr.Zero) return;

            var storable = def.TryCast<NativeStorableItemDef>();
            if (storable == null || storable.Pointer == IntPtr.Zero) return;

            var shops = Il2CppScheduleOne.UI.Shop.ShopInterface.AllShops;
            if (shops == null || shops.Count == 0)
            {
                if (_injectRetryCount < MaxInjectRetries)
                {
                    _injectRetryCount++;
                    MelonCoroutines.Start(DelayedShopRetry());
                }
                return;
            }
            _injectRetryCount = 0;

            for (int i = 0; i < shops.Count; i++)
            {
                var shop = shops[i];
                if (shop == null || shop.Pointer == IntPtr.Zero) continue;

                string code = (shop.ShopCode ?? string.Empty).ToLowerInvariant();
                string name = (shop.ShopName ?? string.Empty).ToLowerInvariant();

                if (!code.Contains("hardware") && !name.Contains("hardware") && !code.Contains("dan") && !name.Contains("dan") && !code.Contains("hank") && !name.Contains("hank")) continue;

                var listings = shop.Listings;
                if (listings == null) continue;

                bool alreadyIn = false;
                for (int j = 0; j < listings.Count; j++)
                {
                    var existing = listings[j];
                    if (existing != null && existing.Item != null && existing.Item.ID == itemId)
                    {
                        S1Mods.Shared.ShopListingSync.OnListingExists(shop, existing, 10, msg => Mod.Log.Warn(msg));
                        alreadyIn = true;
                        break;
                    }
                }
                if (alreadyIn) continue;

                var listing = new Il2CppScheduleOne.UI.Shop.ShopListing
                {
                    name = "SnackVendor_ShopListing",
                    Item = storable,
                    OverridePrice = true,
                    OverriddenPrice = Mod.CurrentConfig.StationPurchasePrice,
                    LimitedStock = false,
                    DefaultStock = 10,
                    CanBeDelivered = true
                };

                listings.Add(listing);
                S1Mods.Shared.ShopListingSync.OnListingAdded(shop, listing, 10, msg => Mod.Log.Info(msg), msg => Mod.Log.Warn(msg));
                Mod.Log.Info($"Injected '{itemId}' into hardware shop listing ('{shop.ShopName}').");
            }
        }
        catch (Exception ex)
        {
            Mod.Log.Warn($"Failed to inject hardware store listing: {ex.Message}");
        }
    }

    private static System.Collections.IEnumerator DelayedShopRetry()
    {
        yield return new UnityEngine.WaitForSeconds(2.0f);
        InjectHardwareStoreListing();
    }
}
