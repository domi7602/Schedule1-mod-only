extern alias il2cpp;

using System;
using System.Collections.Generic;
using System.Linq;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.Attributes;
using Il2CppScheduleOne.EntityFramework;
using Il2CppScheduleOne.ItemFramework;
using Il2CppScheduleOne.ObjectScripts;
using MelonLoader;
using S1Mods.Shared;
using UnityEngine;
using GameObject = UnityEngine.GameObject;
using Random = UnityEngine.Random;
using NativeIngredientItemDef = Il2CppScheduleOne.ItemFramework.AdditiveDefinition;
using NativeBuildableItemDef = Il2CppScheduleOne.ItemFramework.BuildableItemDefinition;
using NativeStorableItemDef = Il2CppScheduleOne.ItemFramework.StorableItemDefinition;
using GameRegistry = Il2CppScheduleOne.Registry;

namespace SnackVendor.World;

/// <summary>
/// MonoBehaviour on the placed Snack-Vendor GameObject. Its only job is to:
///   1. Clone a vanilla VendingMachine from the live scene into a NetworkObject
///      so NPCs can route purchases at us (FishNet RPCs all pre-wired by vanilla),
///   2. Swap our GLB mesh onto the cloned body while keeping the vanilla
///      interactable/anim/audio/collision components,
///   3. Hold the Stock and route deposit/extract UI calls to it,
///   4. Persist stock + pin a stable BakedGUID for save/load.
///
/// This is the Spike-Gate component. If 1 fails, the buildable item still
/// appears but no NPC will ever buy from it; if 2 fails, the player sees the
/// raw Cuke-style box and we keep going (the GLB fallback in GLB-Load) — every
/// failure path is logged loud, no silent breakage.
/// </summary>
[RegisterTypeInIl2Cpp]
public sealed class SnackVendorController : MonoBehaviour
{
    public SnackVendorController(IntPtr ptr) : base(ptr) { }

    // ----- stable identity -----
    public string InstanceGuid = string.Empty;
    public SnackVendorMarker? Marker;
    public VendingMachine? Clone;

    // ----- visual / mesh -----
    private bool _meshSwapped = false;
    // Audit 0.0.3: keep explicit handles for cleanup. The GLB-swapped mesh
    // (built procedurally in SwapMesh) is parented to the cloned vanilla
    // machine, and its Renderer was given a brand-new Material(sh). If we
    // leave these dangling when the station is dismantled, the Material
    // (and the dynamically-allocated Sprite/texture pair on the ghost icon)
    // outlive the GameObject and accumulate GPU memory across saves.
    private GameObject? _glbMeshGo;
    private readonly List<Material> _ownedMaterials = new();

    // Marked true once OnDestroy has run; guards against double-cleanup in
    // the rare case Unity invokes the callback twice (scene reload mid-tear).
    private bool _destroyed;

    // ----- stock (managed, source of truth is the sidecar; this is the runtime cache) -----
    private readonly List<StockSlot> _stock = new();

    [HideFromIl2Cpp]
    public IReadOnlyList<StockSlot> Stock => _stock;

    /// <summary>Slot count from config (was hardcoded 8 until v0.0.5).</summary>
    public int MaxSlots => Mod.CurrentConfig.MaxIngredientSlots;

    // Set once the station wiring (clone + mesh + stock) has run for this
    // instance — guards the double-entry path where the vanilla Start postfix
    // AND the street-placement external setup could both fire on the same GO.
    private bool _placementDone = false;

    /// <summary>Called by BuildableItem.Start-Postfix after the GameObject lives.</summary>
    public void SetupAfterPlacement(BuildableItem owner)
    {
        try
        {
            EnsureGuid(owner);
            RunPlacementSetup();
        }
        catch (Exception ex)
        {
            Mod.Log.Error($"SnackVendorController.SetupAfterPlacement failed", ex);
        }
    }

    /// <summary>
    /// External entry for placement paths that instantiate the BuiltItem
    /// prefab directly (HomelessMod street placement) and bypass the vanilla
    /// BuildableItem.Start flow. Called via SnackVendorItemFactory.
    /// SetupPlacedStation. Safe to call multiple times: the wiring runs at
    /// most once per controller instance (_placementDone guard).
    /// </summary>
    public void SetupAfterPlacementExternal()
    {
        try
        {
            if (string.IsNullOrEmpty(InstanceGuid)) InstanceGuid = Guid.NewGuid().ToString();
            RunPlacementSetup();
        }
        catch (Exception ex)
        {
            Mod.Log.Error($"SnackVendorController.SetupAfterPlacementExternal failed", ex);
        }
    }

    private void RunPlacementSetup()
    {
        if (_placementDone)
        {
            Mod.Log.Debug("SnackVendor placement setup already done for this instance — skipping.");
            return;
        }
        _placementDone = true;
        Mod.Log.Info("SnackVendorController setup starting (vanilla clone + mesh + stock)...");
        SpawnVanillaClone(gameObject);
        SwapMesh();
        RestoreStockFromDisk();
    }

    private void EnsureGuid(BuildableItem owner)
    {
        if (!string.IsNullOrEmpty(InstanceGuid)) return;

        // Owner GUID first.
        string? ownerGuid = null;
        try
        {
            var raw = owner.GUID;
            if (raw != default)
            {
                ownerGuid = raw.ToString();
            }
        }
        catch
        {
            // property access threw — fall through to assignment below
        }
        InstanceGuid = ownerGuid ?? Guid.NewGuid().ToString();
        Mod.Log.Info($"SnackVendor '{name}' Guid = {InstanceGuid}");
    }

    /// <summary>
    /// Spawn-Gate (the Spike). Clones a vanilla VendingMachine out of the
    /// scene and parents the clone to the buildable GameObject so it inherits
    /// position/rotation. FishNet: the vanilla prefab owns its NetworkObject,
    /// which is what makes its RPCs magically registered — the clone picks
    /// up the same component family when Instantiate() runs Awake().
    /// </summary>
    private void SpawnVanillaClone(GameObject ownerGo)
    {
        try
        {
            var allMachines = VendingMachine.AllMachines;
            if (allMachines == null || allMachines.Count == 0)
            {
                Mod.Log.Warn("No VendingMachine available in the scene to clone — NPC shop path will be DEAD.");
                return;
            }

            // Pick the first live machine as the template. Any vanilla one will do
            // — we only need its component wiring (Interactable, Lights, Sound,
            // PhysicsDamageable, FishNet RPCs); the mesh we replace.
            var template = allMachines[0];
            if (template == null || template.Pointer == IntPtr.Zero || template.gameObject == null)
            {
                Mod.Log.Warn("Template VendingMachine not in a usable state — NPC shop path will be DEAD.");
                return;
            }

            var templateGo = template.gameObject;
            GameObject cloneGo = UnityEngine.Object.Instantiate(templateGo);
            cloneGo.name = $"SnackVendorClone_{InstanceGuid[..Math.Min(8, InstanceGuid.Length)]}";
            cloneGo.transform.SetParent(ownerGo.transform, false);
            cloneGo.transform.localPosition = Vector3.zero;
            cloneGo.transform.localRotation = Quaternion.identity;

            // Strip the *cloned* component, then re-fetch — Instantiate on a
            // NetworkBehaviour does the right thing for FishNet but requires
            // we look up the local-instance again.
            Clone = cloneGo.GetComponentInChildren<VendingMachine>();
            if (Clone == null || Clone.Pointer == IntPtr.Zero)
            {
                Mod.Log.Warn("Clone Instantiate did not yield a VendingMachine component — NPC shop path DEAD.");
                UnityEngine.Object.Destroy(cloneGo);
                return;
            }

            // Pin marker on the clone.
            Marker = cloneGo.AddComponent<SnackVendorMarker>();
            Marker.InstanceGuid = InstanceGuid;
            Marker.VanillaClone = Clone;
            // Try pinning a stable GUID for save/load via BakedGUID; if the
            // property is read-only we just leave it null and rely on
            // sidecar + scene persistence alone.
            try { Marker.BakedGuid = new il2cpp::Il2CppSystem.Guid(InstanceGuid); } catch { /* swallowed */ }

            Mod.Log.Info("Spawned vanilla VendingMachine clone for SnackVendor — NPC shop path LIVE.");
        }
        catch (Exception ex)
        {
            Mod.Log.Error("SpawnVanillaClone failed", ex);
        }
    }

    /// <summary>
    /// Replaces the cloned body's renderers with the SnackVendor GLB.
    /// Loads via <c>S1MAPI.Gltf.GltfLoader.LoadGlb</c> (same pipeline as
    /// AutoPackagingStation). Falls back to leaving the vanilla mesh in place
    /// if the load fails (player still sees a vending machine; NPCs still
    /// route to it). The vanilla clone renderers are hidden ONLY after a GLB
    /// GameObject actually exists — never before (invisible-machine trap,
    /// see 0.0.4 CHANGELOG).
    /// </summary>
    private void SwapMesh()
    {
        if (_meshSwapped) return;
        _meshSwapped = true; // single attempt; no retries — silent errors here are worse than visible ones
        try
        {
            var glb = BuildOrLoadGhostPrefab.GetGlbBytes();
            if (glb == null || glb.Length == 0)
            {
                Mod.Log.Warn("GLB bytes missing — keeping vanilla mesh.");
                return;
            }

            var parentTransform = (Clone != null && Clone.Pointer != IntPtr.Zero) ? Clone.transform : transform;

            GameObject? glbGo = null;
            try
            {
                glbGo = S1MAPI.Gltf.GltfLoader.LoadGlb(glb);
            }
            catch (Exception ex)
            {
                Mod.Log.Warn("GltfLoader.LoadGlb threw — falling back to vanilla clone mesh", ex);
            }

            if (glbGo == null || glbGo.Pointer == IntPtr.Zero)
            {
                Mod.Log.Warn("GLB load failed/empty — keeping vanilla clone mesh (NPC path unaffected).");
                return;
            }

            glbGo.name = "SnackVendor_GLBMesh";
            glbGo.transform.SetParent(parentTransform, false);
            glbGo.transform.localPosition = Vector3.zero;
            glbGo.transform.localRotation = Quaternion.identity;
            _glbMeshGo = glbGo; // keep handle for OnDestroy.

            // Strip colliders — collision comes from the buildable item itself.
            var cols = glbGo.GetComponentsInChildren<Collider>(true);
            for (int i = 0; i < cols.Length; i++)
            {
                try { if (cols[i] != null && cols[i].Pointer != IntPtr.Zero) UnityEngine.Object.Destroy(cols[i]); }
                catch { /* non-fatal */ }
            }

            // URP shader fix: mutate the GLB's own materials via sharedMaterial
            // (no per-renderer Material clones — leak-safe, AutoPack H12 pattern).
            Shader? SafeShader() => Shader.Find("Universal Render Pipeline/Lit") ?? Shader.Find("Standard") ?? Shader.Find("Unlit/Color");
            var sh = SafeShader();
            var rs = glbGo.GetComponentsInChildren<Renderer>(true);
            for (int i = 0; i < rs.Length; i++)
            {
                try
                {
                    if (rs[i] == null || rs[i].Pointer == IntPtr.Zero) continue;
                    var sm = rs[i].sharedMaterial;
                    if (sm == null || sm.Pointer == IntPtr.Zero) continue;
                    sm.shader = sh;
                    _ownedMaterials.Add(sm); // GLB materials are per-load instances — track for OnDestroy.
                }
                catch { /* renderer with submeshes etc. — keep whatever material they had */ }
            }

            // Hide the cloned-vanilla renderers ONLY now that a real GLB mesh
            // is rendered (0.0.4 lesson: never hide before the mesh exists).
            if (Clone != null && Clone.Pointer != IntPtr.Zero)
            {
                var vanillaRs = Clone.GetComponentsInChildren<Renderer>(true);
                for (int i = 0; i < vanillaRs.Length; i++)
                {
                    try { if (vanillaRs[i] != null && vanillaRs[i].Pointer != IntPtr.Zero) vanillaRs[i].enabled = false; }
                    catch { /* non-fatal */ }
                }
            }

            Mod.Log.Info("GLB mesh loaded via S1MAPI.GltfLoader — vanilla clone renderers hidden.");
        }
        catch (Exception ex)
        {
            Mod.Log.Error("SwapMesh failed", ex);
        }
    }

    // ========== LIFECYCLE / CLEANUP ==========

    /// <summary>
    /// Audit 0.0.3: explicit cleanup for the dynamically allocated resources
    /// we own. Without this, every dismantled station leaks the GLB mesh's
    /// brand-new Material instances (and the GLB GameObject stays in the
    /// scene under DontDestroyOnLoad semantics on its parent). Mirrors
    /// AutoPackStationController.OnDestroy.
    /// </summary>
    private void OnDestroy()
    {
        if (_destroyed) return;
        _destroyed = true;

        try
        {
            // Destroy owned materials first (each one was `new Material(sh)`
            // in SwapMesh). Try/catch per item so a single bad ref doesn't
            // starve the rest of the cleanup.
            for (int i = 0; i < _ownedMaterials.Count; i++)
            {
                var m = _ownedMaterials[i];
                if (m == null) continue;
                try { UnityEngine.Object.Destroy(m); }
                catch (Exception ex) { Mod.Log.Warn($"OnDestroy: material {i}", ex); }
            }
            _ownedMaterials.Clear();

            // Tear down the GLB mesh GameObject (parented to the clone, but
            // its lifetime was tied to this controller).
            if (_glbMeshGo != null && _glbMeshGo.Pointer != IntPtr.Zero)
            {
                try { UnityEngine.Object.Destroy(_glbMeshGo); }
                catch (Exception ex) { Mod.Log.Warn("OnDestroy: glbGo", ex); }
            }
            _glbMeshGo = null;

            // The Marker component rides on the clone; Unity destroys it
            // when the clone GameObject is destroyed. We just null the
            // managed reference so the GC sees it sooner.
            Marker = null;
            Clone = null;
        }
        catch (Exception ex)
        {
            Mod.Log.Warn("SnackVendorController.OnDestroy failed", ex);
        }
    }

    // ========== STOCK / SLOTS ==========

    // Cached allowed-ingredient set (v0.0.5: derived from live shop listings —
    // the old numeric registry scan 1..1023 never matched a single real item ID,
    // item IDs in Schedule I are strings like "cuke").
    private static readonly List<NativeStorableItemDef> _allowedIngredients = new();
    private static float _allowedIngredientsAt = -999f;
    private const float AllowedIngredientsRefreshSeconds = 30f;

    /// <summary>
    /// Resolves the sellable ingredient set from shop listings (gas-market
    /// shops first, any shop as fallback) where the listing item is an
    /// <c>AdditiveDefinition</c>. Cached for 30 s; 0 allocations when cached.
    /// </summary>
    [HideFromIl2Cpp]
    public static List<NativeStorableItemDef> GetAllowedIngredientDefs()
    {
        if (_allowedIngredients.Count > 0
            && Time.realtimeSinceStartup - _allowedIngredientsAt < AllowedIngredientsRefreshSeconds)
        {
            return _allowedIngredients;
        }

        _allowedIngredients.Clear();
        try
        {
            var shops = Il2CppScheduleOne.UI.Shop.ShopInterface.AllShops;
            if (shops != null && shops.Count > 0)
            {
                // Pass 1: gas-market shops, Pass 2: everything else (fallback).
                for (int pass = 0; pass < 2 && _allowedIngredients.Count == 0; pass++)
                {
                    var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                    for (int i = 0; i < shops.Count; i++)
                    {
                        var shop = shops[i];
                        if (shop == null || shop.Pointer == IntPtr.Zero) continue;

                        string code = (shop.ShopCode ?? string.Empty).ToLowerInvariant();
                        string name = (shop.ShopName ?? string.Empty).ToLowerInvariant();
                        bool isGas = code.Contains("gas") || name.Contains("gas");
                        if (pass == 0 && !isGas) continue;

                        var listings = shop.Listings;
                        if (listings == null) continue;
                        for (int j = 0; j < listings.Count; j++)
                        {
                            var listing = listings[j];
                            if (listing == null || listing.Pointer == IntPtr.Zero) continue;
                            var item = listing.Item;
                            if (item == null || item.Pointer == IntPtr.Zero) continue;
                            var additive = item.TryCast<NativeIngredientItemDef>();
                            if (additive == null || additive.Pointer == IntPtr.Zero) continue;
                            string id = item.ID ?? string.Empty;
                            if (id.Length == 0 || !seen.Add(id)) continue;
                            _allowedIngredients.Add(item);
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Mod.Log.Warn("GetAllowedIngredientDefs", ex);
        }
        _allowedIngredientsAt = Time.realtimeSinceStartup;
        if (_allowedIngredients.Count == 0)
            Mod.Log.Warn("No ingredient definitions resolved from shops — deposit panel will stay empty.");
        return _allowedIngredients;
    }

    /// <summary>Resolves a live ingredient definition by registry item ID; null when unknown/unalive.</summary>
    [HideFromIl2Cpp]
    public static NativeStorableItemDef? GetIngredientDef(string ingredientId)
    {
        if (string.IsNullOrEmpty(ingredientId)) return null;
        try
        {
            var def = GameRegistry.GetItem(ingredientId);
            if (def == null || def.Pointer == IntPtr.Zero) return null;
            var storable = def.TryCast<NativeStorableItemDef>();
            if (storable == null || storable.Pointer == IntPtr.Zero) return null;
            return storable;
        }
        catch { return null; }
    }

    /// <summary>Returns id->displayName for the UI; falls back to the raw id.</summary>
    [HideFromIl2Cpp]
    public static string GetIngredientName(string ingredientId)
    {
        try
        {
            var def = GameRegistry.GetItem(ingredientId);
            var n = def?.Name;
            return string.IsNullOrEmpty(n) ? ingredientId : n;
        }
        catch { return ingredientId; }
    }

    [HideFromIl2Cpp]
    public int GetStockQuantity(string ingredientId)
    {
        for (int i = 0; i < _stock.Count; i++)
        {
            if (string.Equals(_stock[i].IngredientId, ingredientId, StringComparison.OrdinalIgnoreCase))
            {
                return _stock[i].Quantity;
            }
        }
        return 0;
    }

    [HideFromIl2Cpp]
    public bool TryDepositIngredient(string ingredientId, int amount)
    {
        if (string.IsNullOrEmpty(ingredientId) || amount <= 0) return false;
        // Clamp max-slots + check existing slot for same id.
        StockSlot? slot = null;
        for (int i = 0; i < _stock.Count; i++)
        {
            if (string.Equals(_stock[i].IngredientId, ingredientId, StringComparison.OrdinalIgnoreCase))
            {
                slot = _stock[i];
                break;
            }
        }

        if (slot != null)
        {
            slot.Quantity += amount;
            return true;
        }
        if (_stock.Count >= MaxSlots) return false;
        _stock.Add(new StockSlot(ingredientId, amount));
        return true;
    }

    [HideFromIl2Cpp]
    public bool TryExtractIngredient(string ingredientId, int amount)
    {
        if (string.IsNullOrEmpty(ingredientId) || amount <= 0) return false;
        StockSlot? slot = null;
        for (int i = 0; i < _stock.Count; i++)
        {
            if (string.Equals(_stock[i].IngredientId, ingredientId, StringComparison.OrdinalIgnoreCase))
            {
                slot = _stock[i];
                break;
            }
        }

        if (slot == null || slot.Quantity < amount) return false;
        slot.Quantity -= amount;
        if (slot.Quantity == 0) _stock.Remove(slot);
        return true;
    }

    /// <summary>Try pop one slot for the NPC purchase path. Returns null when empty.</summary>
    [HideFromIl2Cpp]
    public string? TryConsumeOne()
    {
        if (_stock.Count == 0) return null;
        int idx = Random.Range(0, _stock.Count);
        var slot = _stock[idx];
        slot.Quantity -= 1;
        string id = slot.IngredientId;
        if (slot.Quantity <= 0) _stock.RemoveAt(idx);
        return id;
    }

    [HideFromIl2Cpp]
    private void RestoreStockFromDisk()
    {
        // Sidecar is loaded centrally on GameLifecycle events; we look up
        // by InstanceGuid to populate this controller's slots.
        var all = SnackVendor.Persistence.SnackVendorStore.Load();
        if (all?.Stations == null) return;
        SnackVendor.Persistence.SnackStationSaveData? data = null;
        for (int i = 0; i < all.Stations.Count; i++)
        {
            if (all.Stations[i].InstanceGuid == InstanceGuid)
            {
                data = all.Stations[i];
                break;
            }
        }
        if (data == null) return;
        _stock.Clear();
        for (int i = 0; i < data.Slots.Count; i++)
        {
            var s = data.Slots[i];
            if (s == null || string.IsNullOrEmpty(s.IngredientId) || s.Quantity <= 0) continue;
            // v0.0.5: IDs are registry strings now. Skip legacy numeric-scan
            // rows (they never resolved to a real item anyway).
            if (GetIngredientDef(s.IngredientId) == null)
            {
                Mod.Log.Warn($"RestoreStockFromDisk: dropping unknown ingredient id '{s.IngredientId}'.");
                continue;
            }
            _stock.Add(new StockSlot(s.IngredientId, s.Quantity));
        }
    }

    [HideFromIl2Cpp]
    public void PersistSlotsToDisk()
    {
        var all = SnackVendor.Persistence.SnackVendorStore.Load();
        if (all == null) return;
        SnackVendor.Persistence.SnackStationSaveData? data = null;
        for (int i = 0; i < all.Stations.Count; i++)
        {
            if (all.Stations[i].InstanceGuid == InstanceGuid)
            {
                data = all.Stations[i];
                break;
            }
        }
        if (data == null)
        {
            data = new SnackVendor.Persistence.SnackStationSaveData { InstanceGuid = InstanceGuid };
            all.Stations.Add(data);
        }
        data.Slots.Clear();
        foreach (var slot in _stock)
        {
            data.Slots.Add(new SnackVendor.Persistence.SnackSlotData
            {
                IngredientId = slot.IngredientId,
                Quantity = slot.Quantity,
            });
        }
        SnackVendor.Persistence.SnackVendorStore.Save(all);
    }

    /// <summary>Resolves a vanilla BasePurchasePrice for an ingredient id; -1 if unknown.</summary>
    public float ResolveIngredientPrice(string ingredientId)
    {
        try
        {
            var def = GetIngredientDef(ingredientId);
            if (def == null || def.Pointer == IntPtr.Zero) return -1f;
            float price = def.BasePurchasePrice;
            return price < 0f ? -1f : price;
        }
        catch (Exception ex)
        {
            Mod.Log.Warn($"ResolveIngredientPrice({ingredientId})", ex);
            return -1f;
        }
    }
}

/// <summary>Managed runtime record — never crosses into IL2CPP land.</summary>
public sealed class StockSlot
{
    public string IngredientId;
    public int Quantity;
    public StockSlot(string id, int q) { IngredientId = id; Quantity = q; }
}
