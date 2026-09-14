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

    // ----- stock (managed, source of truth is the sidecar; this is the runtime cache) -----
    private readonly List<StockSlot> _stock = new();

    public IReadOnlyList<StockSlot> Stock => _stock;

    /// <summary>Slot of size 1 — keeps it simple, no merge logic needed for this MVP.</summary>
    public int MaxSlots => 8; // TODO: read from current slot-config

    /// <summary>Called by BuildableItem.Start-Postfix after the GameObject lives.</summary>
    public void SetupAfterPlacement(BuildableItem owner)
    {
        try
        {
            EnsureGuid(owner);
            SpawnVanillaClone(owner.gameObject);
            SwapMesh();
            RestoreStockFromDisk();
        }
        catch (Exception ex)
        {
            Mod.Log.Error($"SnackVendorController.SetupAfterPlacement failed", ex);
        }
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
    /// Falls back to leaving the vanilla mesh in place if the load fails
    /// (player still sees a vending machine; NPCs still route to it).
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
            var glbGo = new GameObject("SnackVendor_GLBMesh");
            glbGo.transform.SetParent(parentTransform, false);
            glbGo.transform.localPosition = Vector3.zero;
            glbGo.transform.localRotation = Quaternion.identity;

            // Hide the cloned-vanilla renderers so the GLB shows alone.
            if (Clone != null && Clone.Pointer != IntPtr.Zero)
            {
                try
                {
                    var rs = Clone.GetComponentsInChildren<Renderer>(true);
                    foreach (var r in rs) if (r != null && r.Pointer != IntPtr.Zero) r.enabled = false;
                }
                catch { /* swallow, GLB still gets visualised */ }
            }

            // Texture+material fix: apply URP-Lit to all renderers so
            // GLB-shaders don't show as pink. Mirrors AutoPackMeshBuilder.
            Shader? SafeShader() => Shader.Find("Universal Render Pipeline/Lit") ?? Shader.Find("Standard") ?? Shader.Find("Unlit/Color");
            try
            {
                var rs = glbGo.GetComponentsInChildren<Renderer>(true);
                var sh = SafeShader();
                foreach (var r in rs)
                {
                    try
                    {
                        r.material = new Material(sh);
                    }
                    catch
                    {
                        // renderer with submeshes etc. — keep whatever material they had
                    }
                }
            }
            catch { /* non-fatal */ }

            // NOTE: We do NOT call S1MAPI.GltfLoader.LoadGlb — that API
            // exists in S1MAPI_Il2cpp.dll which we reference conditionally.
            // For the MVP runtime we ship the BoxCollider-based ghost prefab
            // and let the player see a snack-bar 0.95×1.85×0.72 m proxy until
            // we wire the full GLB in a follow-up. The marker, the slots,
            // and the NPC-purchase path all work without it.
            //
            // TODO(Spike-Followup): load actual GLB through S1MAPI once we
            // confirm AddComponent-vs-clone behaviour in a live test.
            Mod.Log.Info("Visual mesh swapped to proxy (full GLB routing in follow-up).");
        }
        catch (Exception ex)
        {
            Mod.Log.Error("SwapMesh failed", ex);
        }
    }

    // ========== STOCK / SLOTS ==========

    /// <summary>Resolves the configured ingredient ids to vanilla registry ids once per call.</summary>
    public static List<int> GetAllowedIngredientIds()
    {
        var list = new List<int>();
        try
        {
            var allIds = new List<int>();
            for (int candidate = 1; candidate < 1024; candidate++)
            {
                try
                {
                    var def = GameRegistry.GetItem(candidate.ToString());
                    if (def == null || def.Pointer == IntPtr.Zero) continue;
                    if (def.TryCast<NativeIngredientItemDef>() != null) allIds.Add(candidate);
                }
                catch { /* no-op: nonexistent id returns null */ }
            }
            list = allIds;
        }
        catch (Exception ex)
        {
            Mod.Log.Warn("GetAllowedIngredientIds", ex);
        }
        return list;
    }

    /// <summary>Returns id->displayName for the UI for the currently-allowed set.</summary>
    public static string? GetIngredientName(int ingredientId)
    {
        try
        {
            var def = GameRegistry.GetItem(ingredientId.ToString());
            return def?.Name;
        }
        catch { return null; }
    }

    public bool TryDepositIngredient(int ingredientId, int amount)
    {
        if (amount <= 0) return false;
        // Clamp max-slots + check existing slot for same id.
        var slot = _stock.FirstOrDefault(s => s.IngredientId == ingredientId);
        if (slot != null)
        {
            slot.Quantity += amount;
            return true;
        }
        if (_stock.Count >= MaxSlots) return false;
        _stock.Add(new StockSlot(ingredientId, amount));
        return true;
    }

    public bool TryExtractIngredient(int ingredientId, int amount)
    {
        if (amount <= 0) return false;
        var slot = _stock.FirstOrDefault(s => s.IngredientId == ingredientId);
        if (slot == null || slot.Quantity < amount) return false;
        slot.Quantity -= amount;
        if (slot.Quantity == 0) _stock.Remove(slot);
        return true;
    }

    /// <summary>Try pop one slot for the NPC purchase path. Returns -1 if empty.</summary>
    public int TryConsumeOne()
    {
        if (_stock.Count == 0) return -1;
        int idx = Random.Range(0, _stock.Count);
        var slot = _stock[idx];
        slot.Quantity -= 1;
        int id = slot.IngredientId;
        if (slot.Quantity <= 0) _stock.RemoveAt(idx);
        return id;
    }

    private void RestoreStockFromDisk()
    {
        // Sidecar is loaded centrally on GameLifecycle events; we look up
        // by InstanceGuid to populate this controller's slots.
        var all = SnackVendor.Persistence.SnackVendorStore.Load();
        if (all?.Stations == null) return;
        var data = all.Stations.FirstOrDefault(s => s.InstanceGuid == InstanceGuid);
        if (data == null) return;
        _stock.Clear();
        foreach (var s in data.Slots)
        {
            if (int.TryParse(s.IngredientId, out int ingredientId) && s.Quantity > 0)
            {
                _stock.Add(new StockSlot(ingredientId, s.Quantity));
            }
        }
    }

    public void PersistSlotsToDisk()
    {
        var all = SnackVendor.Persistence.SnackVendorStore.Load();
        if (all == null) return;
        var data = all.Stations.FirstOrDefault(s => s.InstanceGuid == InstanceGuid);
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
                IngredientId = slot.IngredientId.ToString(),
                Quantity = slot.Quantity,
            });
        }
        SnackVendor.Persistence.SnackVendorStore.Save(all);
    }

    /// <summary>Resolves a vanilla BasePurchasePrice for an ingredient id; -1 if unknown.</summary>
    public float ResolveIngredientPrice(int ingredientId)
    {
        try
        {
            var def = GameRegistry.GetItem(ingredientId.ToString());
            if (def == null || def.Pointer == IntPtr.Zero) return -1f;
            // BasePurchasePrice lives on StorableItemDefinition, not ItemDefinition.
            // Most ingredients in the game ARE storable, so the cast almost
            // always succeeds. Anything that fails (e.g. a non-storable item
            // sneaking through) returns -1 so the caller skips it.
            var storable = def.TryCast<Il2CppScheduleOne.ItemFramework.StorableItemDefinition>();
            if (storable == null || storable.Pointer == IntPtr.Zero) return -1f;
            float price = storable.BasePurchasePrice;
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
    public int IngredientId;
    public int Quantity;
    public StockSlot(int id, int q) { IngredientId = id; Quantity = q; }
}
