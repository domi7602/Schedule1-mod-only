using System;
using MelonLoader;
using HarmonyLib;
using S1Mods.Shared;
using System.IO;
using UnityEngine;
using S1API.Lifecycle;

[assembly: MelonInfo(typeof(BackpackMod.Mod), "BackpackMod", "1.0.2", "Dominik")]
[assembly: MelonGame("TVGS", "Schedule I")]

namespace BackpackMod;

public class Mod : MelonMod
{
    public static Mod Instance { get; private set; }
    public static MelonLogger.Instance Log { get; private set; }
    private static AssetBundle? _bundle;
    public static AssetBundle? BackpackBundle => _bundle;

    public override void OnInitializeMelon()
    {
        Instance = this;
        Log = LoggerInstance;

        // Apply Harmony patches via PatchGuard (graceful degradation on game update)
        try
        {
            var harmony = new HarmonyLib.Harmony("com.s1mods.backpackmod");
            var logger = new ModLogger("BackpackMod");
            PatchGuard.TryPatch(harmony, typeof(Il2CppScheduleOne.PlayerScripts.PlayerClothing), "Awake", postfix: new HarmonyMethod(typeof(Patches.PlayerClothingPatch), nameof(Patches.PlayerClothingPatch.Postfix)), log: logger);
            // Hotfix 2026-08-30: guard vanilla ClothingItemUI.UpdateUI NRE when backpack clothing item is added via AddItemToInventory (console/MCP)
            PatchGuard.TryPatch(harmony, typeof(Il2CppScheduleOne.UI.Items.ClothingItemUI), "UpdateUI", finalizer: new HarmonyMethod(typeof(Patches.ClothingItemUIPatch), nameof(Patches.ClothingItemUIPatch.Finalizer)), log: logger);
            PatchGuard.TryPatch(harmony, typeof(Il2CppScheduleOne.UI.StorageMenu), "Close", prefix: new HarmonyMethod(typeof(Patches.StorageMenuPatch), nameof(Patches.StorageMenuPatch.Close_Prefix)), log: logger);
            PatchGuard.TryPatch(harmony, typeof(Il2CppScheduleOne.UI.CharacterInterface), "Open", postfix: new HarmonyMethod(typeof(Patches.CharacterUIPatch), nameof(Patches.CharacterUIPatch.Open_Postfix)), log: logger);
            PatchGuard.TryPatch(harmony, typeof(Il2CppScheduleOne.UI.CharacterInterface), "Close", postfix: new HarmonyMethod(typeof(Patches.CharacterUIPatch), nameof(Patches.CharacterUIPatch.Close_Postfix)), log: logger);
            PatchGuard.TryPatch(harmony, typeof(Il2CppScheduleOne.UI.CharacterInterface), "LateUpdate", postfix: new HarmonyMethod(typeof(Patches.CharacterUIPatch), nameof(Patches.CharacterUIPatch.LateUpdate_Postfix)), log: logger);
            PatchGuard.Report(logger);
        }
        catch (Exception ex) { Log?.Warning($"Harmony patch failed: {ex.Message}"); }

        GameLifecycle.OnPreLoad += EnsureDefinitions;
        GameLifecycle.OnSaveInfoLoaded += EnsureDefinitions;
        GameLifecycle.OnSaveInfoLoaded += BackpackDefinitions.InjectHardwareStoreListing;
        GameLifecycle.OnLoadComplete += EnsureDefinitions;
        GameLifecycle.OnLoadComplete += BackpackDefinitions.InjectHardwareStoreListing;
        GameLifecycle.OnSaveComplete += OnSaveComplete;
        GameLifecycle.OnPreLoad += OnPreLoadReset;

        Log?.Msg("[BackpackMod] initialized.");
    }

    public override void OnDeinitializeMelon()
    {
        GameLifecycle.OnPreLoad -= EnsureDefinitions;
        GameLifecycle.OnSaveInfoLoaded -= EnsureDefinitions;
        GameLifecycle.OnSaveInfoLoaded -= BackpackDefinitions.InjectHardwareStoreListing;
        GameLifecycle.OnLoadComplete -= EnsureDefinitions;
        GameLifecycle.OnLoadComplete -= BackpackDefinitions.InjectHardwareStoreListing;
        GameLifecycle.OnSaveComplete -= OnSaveComplete;
        GameLifecycle.OnPreLoad -= OnPreLoadReset;
    }

    private static void OnSaveComplete()
    {
        try { BackpackStorageManager.SaveStorage(); } catch { }
    }

    private static void OnPreLoadReset()
    {
        try { BackpackStorageManager.ResetCache(); } catch { }
    }

    private static void EnsureDefinitions()
    {
        if (_bundle == null)
        {
            LoadBundle();
        }
        BackpackDefinitions.Initialize(_bundle);
    }

    private static void LoadBundle()
    {
        string[] candidatePaths = new[]
        {
            Path.Combine(MelonLoader.Utils.MelonEnvironment.MelonBaseDirectory, "Mods", "backpacks.bundle"),
            Path.Combine(MelonLoader.Utils.MelonEnvironment.MelonBaseDirectory, "Mods", "BackpackMod", "assets", "backpacks.bundle"),
            Path.Combine(Directory.GetCurrentDirectory(), "Mods", "backpacks.bundle"),
            Path.Combine(Directory.GetCurrentDirectory(), "Mods", "BackpackMod", "assets", "backpacks.bundle")
        };

        foreach (var path in candidatePaths)
        {
            if (File.Exists(path))
            {
                _bundle = AssetBundle.LoadFromFile(path);
                if (_bundle != null)
                {
                    Log?.Msg($"Successfully loaded AssetBundle from: {path}");
                    return;
                }
            }
        }

        Log?.Warning("Could not find or load backpacks.bundle from any standard path.");
    }

    public override void OnSceneWasUnloaded(int buildIndex, string sceneName)
    {
        BackpackVisualManager.Clear();
        Patches.PlayerClothingPatch.ResetForSceneUnload();
        if (sceneName == "Main") BackpackStorageManager.ResetForSceneUnload();
    }

    public override void OnUpdate()
    {
        BackpackVisualManager.UpdateVisuals(_bundle);
        BackpackStorageManager.OnUpdate();

        bool isTyping = false;
        try { isTyping = S1Mods.Shared.HotkeyManager.IsInputFieldFocused(); } catch { }
        if (isTyping) return;

        if (Input.GetKeyDown(KeyCode.B))
        {
            // Focus guard already above; also require in-game save
            var lm = Il2CppScheduleOne.DevUtilities.PersistentSingleton<Il2CppScheduleOne.Persistence.LoadManager>.Instance;
            if (lm == null || lm.Pointer == IntPtr.Zero || lm.WasCollected || lm.ActiveSaveInfo == null || lm.ActiveSaveInfo.Pointer == IntPtr.Zero || lm.ActiveSaveInfo.WasCollected)
            {
                // Allow toggle only in Main scene with active save
                if (!S1Mods.Shared.NetworkGuard.IsInMainScene) return;
            }
            BackpackStorageManager.ToggleStorage();
        }

#if DEBUG
        if (Input.GetKeyDown(KeyCode.F8))
        {
            Log.Msg("F8 pressed! Attempting to spawn backpacks...");
            EnsureDefinitions();

            var pInv = Il2CppScheduleOne.PlayerScripts.PlayerInventory.Instance;
            if (pInv == null || pInv.Pointer == IntPtr.Zero || pInv.WasCollected)
            {
                Log.Warning("PlayerInventory.Instance is null (are you in an active save game?)");
                return;
            }

            SpawnBackpack(pInv, "backpack_t1");
            SpawnBackpack(pInv, "backpack_t2");
            SpawnBackpack(pInv, "backpack_t3");
        }
#endif
    }

    private static void SpawnBackpack(Il2CppScheduleOne.PlayerScripts.PlayerInventory pInv, string id)
    {
        var itemDef = Il2CppScheduleOne.Registry.GetItem(id);
        if (itemDef == null || itemDef.Pointer == IntPtr.Zero)
        {
            Log.Error($"Item '{id}' not found in Registry!");
            return;
        }

        var instance = itemDef.GetDefaultInstance(1);
        if (instance == null || instance.Pointer == IntPtr.Zero)
        {
            Log.Error($"Could not create ItemInstance for '{id}'!");
            return;
        }

        if (!pInv.CanItemFitInInventory(instance, 1))
        {
            Log.Warning($"Cannot add '{id}' — inventory full.");
            return;
        }

        pInv.AddItemToInventory(instance);
        Log.Msg($"Successfully added '{id}' ({itemDef.name}) to player inventory!");
    }
}
