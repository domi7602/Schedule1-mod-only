using Il2CppScheduleOne.ItemFramework;
using UnityEngine;
using S1API.Items.Storable;
using System;
using System.Collections;
using MelonLoader;
using Il2CppScheduleOne.UI.Shop;
using GameItemDef = Il2CppScheduleOne.ItemFramework.StorableItemDefinition;

namespace BackpackMod
{
    public static class BackpackDefinitions
    {
        // Gatekeeper-fix 2026-08-29: S1API's ClothingSlot wrapper enum uses index 10 for backpack.
        // This is a different type than Il2CppScheduleOne.Clothing.EClothingSlot — do NOT unify.
        // If S1API renumbers the wrapper enum, re-check this constant.
        private const S1API.Items.Clothing.ClothingSlot BackpackSlotId = (S1API.Items.Clothing.ClothingSlot)10;

        public static GameItemDef Tier1Backpack { get; private set; }
        public static GameItemDef Tier2Backpack { get; private set; }
        public static GameItemDef Tier3Backpack { get; private set; }

        public static void Initialize(AssetBundle? bundle)
        {
            try
            {
                // Check each tier individually — previous early return prevented t2/t3 creation if t1 already existed
                bool hasT1 = Il2CppScheduleOne.Registry.ItemExists("backpack_t1");
                bool hasT2 = Il2CppScheduleOne.Registry.ItemExists("backpack_t2");
                bool hasT3 = Il2CppScheduleOne.Registry.ItemExists("backpack_t3");
                if (hasT1 && hasT2 && hasT3) return;

                var reg = Il2CppScheduleOne.Registry.Instance;
                if (reg == null || reg.Pointer == IntPtr.Zero || reg.ItemDictionary == null)
                {
                    Mod.Log?.Msg("Registry not ready yet, deferring backpack registration.");
                    return;
                }

                // Find a real clothing item in the registry to clone from
                string? baseClothingId = null;
                foreach (var entry in reg.ItemDictionary.Values)
                {
                    if (entry != null && entry.Definition != null)
                    {
                        var clothingDef = entry.Definition.TryCast<Il2CppScheduleOne.Clothing.ClothingDefinition>();
                        if (clothingDef != null && clothingDef.Pointer != IntPtr.Zero)
                        {
                            baseClothingId = entry.ID;
                            break;
                        }
                    }
                }

                if (string.IsNullOrEmpty(baseClothingId))
                {
                    Mod.Log?.Msg("No clothing items found in Registry yet, waiting for next lifecycle stage...");
                    return;
                }

                Mod.Log?.Msg($"Registering backpacks by cloning clothing template '{baseClothingId}'...");

                if (!hasT1) Tier1Backpack = CreateBackpackDef(baseClothingId, bundle, 1, "Tier1_Backpack", "backpack_t1", "Small Backpack", 100);
                else Tier1Backpack = Il2CppScheduleOne.Registry.GetItem("backpack_t1")?.TryCast<GameItemDef>();
                if (!hasT2) Tier2Backpack = CreateBackpackDef(baseClothingId, bundle, 2, "Tier2_Backpack", "backpack_t2", "Hiking Bag", 250);
                else Tier2Backpack = Il2CppScheduleOne.Registry.GetItem("backpack_t2")?.TryCast<GameItemDef>();
                if (!hasT3) Tier3Backpack = CreateBackpackDef(baseClothingId, bundle, 3, "Tier3_Backpack", "backpack_t3", "Duffle Bag", 600);
                else Tier3Backpack = Il2CppScheduleOne.Registry.GetItem("backpack_t3")?.TryCast<GameItemDef>();

                Mod.Log?.Msg("Backpack definitions created and registered successfully!");
            }
            catch (Exception ex)
            {
                Mod.Log?.Error($"Failed to initialize Backpack definitions: {ex}");
            }
        }

        private static GameItemDef? CreateBackpackDef(string baseClothingId, AssetBundle? bundle, int tier, string prefabName, string id, string displayName, int price)
        {
            try
            {
                var builder = S1API.Items.Clothing.ClothingItemCreator.CloneFrom(baseClothingId);
                if (builder == null)
                {
                    Mod.Log?.Error($"ClothingItemCreator.CloneFrom('{baseClothingId}') returned null!");
                    return null;
                }

                builder.WithBasicInfo(id, displayName, "A wearable backpack that provides extra storage space. Drop it to access its inventory.", S1API.Items.ItemCategory.Clothing)
                    .WithSlot(BackpackSlotId)
                    .WithPricing(price, 0.5f)
                    .WithIcon(BackpackIconGenerator.GetOrCreateIcon(tier))
                    .WithColorable(false)
                    .WithBlockedSlots(new S1API.Items.Clothing.ClothingSlot[0]);

                builder.Build();

                var def = Il2CppScheduleOne.Registry.GetItem(id)?.TryCast<GameItemDef>();
                if (def != null)
                {
                    Mod.Log?.Msg($"Registered clothing item '{id}' ({displayName})");
                }
                else
                {
                    Mod.Log?.Warning($"Could not find registered item '{id}' in Registry after build.");
                }
                return def;
            }
            catch (Exception ex)
            {
                Mod.Log?.Error($"Error creating backpack definition '{id}': {ex.Message}");
                return null;
            }
        }

        private static int _injectRetryCount = 0;
        private const int MaxInjectRetries = 10;

        /// <summary>
        /// Injects all 3 backpack tiers into both hardware stores (Handy Hank's + Dan's Hardware).
        /// Idempotent — skips already-injected listings. Uses same pattern as AutoPackagingStation/HomelessMod.
        /// </summary>
        public static void InjectHardwareStoreListing()
        {
            try
            {
                var shops = ShopInterface.AllShops;
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

                string[] backpackIds = new[] { "backpack_t1", "backpack_t2", "backpack_t3" };
                float[] prices = new[] { 100f, 250f, 600f };

                for (int i = 0; i < shops.Count; i++)
                {
                    var shop = shops[i];
                    if (shop == null || shop.Pointer == IntPtr.Zero) continue;
                    string code = (shop.ShopCode ?? string.Empty).ToLowerInvariant();
                    string name = (shop.ShopName ?? string.Empty).ToLowerInvariant();
                    if (!code.Contains("hardware") && !name.Contains("hardware") && !code.Contains("dan") && !name.Contains("dan") && !code.Contains("hank") && !name.Contains("hank")) continue;

                    var listings = shop.Listings;
                    if (listings == null) continue;

                    for (int b = 0; b < backpackIds.Length; b++)
                    {
                        string itemId = backpackIds[b];
                        var def = Il2CppScheduleOne.Registry.GetItem(itemId);
                        if (def == null || def.Pointer == IntPtr.Zero) continue;
                        var storable = def.TryCast<GameItemDef>();
                        if (storable == null || storable.Pointer == IntPtr.Zero) continue;

                        bool alreadyIn = false;
                        for (int j = 0; j < listings.Count; j++)
                        {
                            var existing = listings[j];
                            if (existing != null && existing.Item != null && existing.Item.ID == itemId)
                            {
                                alreadyIn = true;
                                break;
                            }
                        }
                        if (alreadyIn) continue;

                        var listing = new ShopListing
                        {
                            name = $"Backpack_{itemId}_Listing",
                            Item = storable,
                            OverridePrice = true,
                            OverriddenPrice = prices[b],
                            LimitedStock = false,
                            DefaultStock = 10,
                            CanBeDelivered = true
                        };
                        listings.Add(listing);
                        Mod.Log?.Msg($"Injected '{itemId}' into hardware shop listing ('{shop.ShopName}').");
                    }
                }
            }
            catch (Exception ex)
            {
                MelonLoader.MelonLogger.Warning($"[BackpackMod] Failed to inject backpack store listings: {ex.Message}");
            }
        }

        private static IEnumerator DelayedShopRetry()
        {
            yield return new WaitForSeconds(2.0f);
            InjectHardwareStoreListing();
        }
    }
}
