using Il2CppScheduleOne.DevUtilities;
using Il2CppScheduleOne.ItemFramework;
using Il2CppScheduleOne.Persistence;
using Il2CppScheduleOne.PlayerScripts;
using Il2CppScheduleOne.Storage;
using Il2CppScheduleOne.UI;
using UnityEngine;
using System;
using System.IO;
using System.Text.Json;
using System.Collections.Generic;
using S1Mods.Shared;

namespace BackpackMod
{
    public static class BackpackStorageManager
    {
        private static StorageEntity? _storageEntity;
        private static GameObject? _storageEntityObj;
        private static int _currentSlotCount = 0;
        private static string _lastKnownSlot = "default";
        private static bool _isBackpackStorageOpen = false;

        public static void ToggleStorage()
        {
            try
            {
                var storageMenu = Singleton<StorageMenu>.Instance;
                if (storageMenu == null || storageMenu.Pointer == IntPtr.Zero)
                {
                    Mod.Log?.Warning("StorageMenu instance is not ready yet.");
                    return;
                }

                if (_isBackpackStorageOpen || storageMenu.IsOpen)
                {
                    if (_isBackpackStorageOpen)
                    {
                        SaveStorage();
                    }
                    if (storageMenu.IsOpen)
                    {
                        storageMenu.Close();
                    }
                    _isBackpackStorageOpen = false;
                    return;
                }

                // Check active backpack tier
                int slotCount = GetActiveBackpackSlotCount(out string title);
                if (slotCount <= 0)
                {
                    Mod.Log?.Msg("No backpack equipped to open storage.");
                    return;
                }

                EnsureStorageEntity(slotCount);
                if (_storageEntity != null && _storageEntity.Pointer != IntPtr.Zero && !_storageEntity.WasCollected)
                {
                    _isBackpackStorageOpen = true;
                    storageMenu.Open(_storageEntity);
                    Mod.Log?.Msg($"Opened Backpack Storage ({title}, {slotCount} slots)!");
                }
            }
            catch (Exception ex)
            {
                Mod.Log?.Error($"Error toggling backpack storage: {ex}");
            }
        }

        public static void OnStorageMenuClosed()
        {
            try
            {
                if (_isBackpackStorageOpen)
                {
                    SaveStorage();
                    _isBackpackStorageOpen = false;
                }
            }
            catch (Exception ex)
            {
                Mod.Log?.Error($"Error in OnStorageMenuClosed: {ex.Message}");
            }
        }

        public static void OnUpdate()
        {
            try
            {
                if (!_isBackpackStorageOpen) return;

                // Immediate save hook and close when ESC is pressed while backpack storage is open
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    SaveStorage();
                    var storageMenu = Singleton<StorageMenu>.Instance;
                    if (storageMenu != null && storageMenu.Pointer != IntPtr.Zero && storageMenu.IsOpen)
                    {
                        storageMenu.Close();
                    }
                    _isBackpackStorageOpen = false;
                    return;
                }

                var menu = Singleton<StorageMenu>.Instance;
                if (menu != null && menu.Pointer != IntPtr.Zero)
                {
                    bool isOpen = menu.IsOpen;
                    if (_isBackpackStorageOpen && !isOpen)
                    {
                        // Menu was closed externally (e.g. via Escape or clicking close button)
                        SaveStorage();
                        _isBackpackStorageOpen = false;
                    }
                }
                else if (_isBackpackStorageOpen)
                {
                    _isBackpackStorageOpen = false;
                    SaveStorage();
                }
            }
            catch { }
        }

        public static int GetActiveBackpackSlotCount(out string title)
        {
            title = "Backpack";
            var wornSlot = Patches.PlayerClothingPatch.BackpackSlot;

            if (wornSlot == null)
            {
                var playerMovement = PlayerSingleton<PlayerMovement>.Instance;
                var pc = playerMovement != null && playerMovement.Pointer != IntPtr.Zero
                    ? playerMovement.GetComponent<Il2CppScheduleOne.PlayerScripts.PlayerClothing>()
                    : null;
                if (pc != null && pc.ClothingSlots != null && pc.ClothingSlots.ContainsKey((Il2CppScheduleOne.Clothing.EClothingSlot)10))
                {
                    wornSlot = pc.ClothingSlots[(Il2CppScheduleOne.Clothing.EClothingSlot)10];
                }
            }

            string? id = null;
            if (wornSlot != null && wornSlot.ItemInstance != null && wornSlot.ItemInstance.Definition != null)
            {
                id = wornSlot.ItemInstance.Definition.ID;
            }

            return id switch
            {
                "backpack_t3" => (title = "Duffle Bag", 24).Item2,
                "backpack_t2" => (title = "Hiking Bag", 16).Item2,
                "backpack_t1" => (title = "Small Backpack", 8).Item2,
                _ => 0
            };
        }

        private static void EnsureStorageEntity(int targetSlotCount)
        {
            if (_storageEntityObj == null || _storageEntity == null || _storageEntity.Pointer == IntPtr.Zero || _storageEntity.WasCollected || _currentSlotCount != targetSlotCount)
            {
                // H2: Capture overflow before shrinking — prevents silent loss of SlotIndex >= targetSlotCount
                List<(string ItemId, int Quantity)> overflow = new();
                if (_storageEntity != null && _storageEntity.Pointer != IntPtr.Zero && !_storageEntity.WasCollected && _storageEntity.ItemSlots != null && targetSlotCount < _currentSlotCount)
                {
                    for (int i = targetSlotCount; i < _storageEntity.ItemSlots.Count; i++)
                    {
                        var s = _storageEntity.ItemSlots[i];
                        if (s != null && s.Pointer != IntPtr.Zero && !s.WasCollected && s.ItemInstance != null && s.ItemInstance.Pointer != IntPtr.Zero && !s.ItemInstance.WasCollected && s.ItemInstance.Definition != null && s.Quantity > 0)
                        {
                            overflow.Add((s.ItemInstance.Definition.ID, s.Quantity));
                        }
                    }
                    if (overflow.Count > 0) Mod.Log?.Warning($"Backpack downgrade {_currentSlotCount}->{targetSlotCount}: {overflow.Count} overflow stacks will be returned to inventory.");
                }

                if (_storageEntityObj != null || (_storageEntity != null && _storageEntity.Pointer != IntPtr.Zero && !_storageEntity.WasCollected))
                {
                    SaveStorage();
                }

                if (_storageEntityObj != null)
                {
                    UnityEngine.Object.Destroy(_storageEntityObj);
                    _storageEntityObj = null;
                }
                _storageEntity = null;

                _storageEntityObj = new GameObject("Backpack_StorageEntity");
                UnityEngine.Object.DontDestroyOnLoad(_storageEntityObj);

                _storageEntity = _storageEntityObj.AddComponent<StorageEntity>();
                if (_storageEntity.ItemSlots != null)
                {
                    _storageEntity.ItemSlots.Clear();
                    for (int i = 0; i < targetSlotCount; i++)
                    {
                        var slot = new ItemSlot(false);
                        _storageEntity.ItemSlots.Add(slot);
                    }
                }

                _currentSlotCount = targetSlotCount;

                // Load saved contents
                LoadStorage();

                // Return overflow to player inventory (or warn if full)
                if (overflow.Count > 0)
                {
                    var inv = PlayerInventory.Instance;
                    foreach (var entry in overflow)
                    {
                        try
                        {
                            var def = Il2CppScheduleOne.Registry.GetItem(entry.ItemId);
                            if (def == null || def.Pointer == IntPtr.Zero) continue;
                            var inst = def.GetDefaultInstance(entry.Quantity);
                            if (inst == null || inst.Pointer == IntPtr.Zero) continue;
                            if (inv != null && inv.Pointer != IntPtr.Zero && !inv.WasCollected && inv.CanItemFitInInventory(inst, entry.Quantity))
                            {
                                inv.AddItemToInventory(inst);
                                Mod.Log?.Msg($"Returned overflow {entry.Quantity}x '{entry.ItemId}' to inventory after downgrade.");
                            }
                            else
                            {
                                Mod.Log?.Warning($"Overflow {entry.Quantity}x '{entry.ItemId}' could not be returned — inventory full. Item lost! Free space and re-equip backpack.");
                            }
                        }
                        catch (Exception ex) { Mod.Log?.Warning($"Overflow return failed for '{entry.ItemId}': {ex.Message}"); }
                    }
                }
            }
        }

        private static string GetActiveSlotSuffix()
        {
            try
            {
                var loadMgr = PersistentSingleton<LoadManager>.Instance;
                if (loadMgr != null && loadMgr.Pointer != IntPtr.Zero && !loadMgr.WasCollected)
                {
                    var saveInfo = loadMgr.ActiveSaveInfo;
                    if (saveInfo != null && saveInfo.Pointer != IntPtr.Zero && !saveInfo.WasCollected)
                    {
                        string slot = $"slot_{saveInfo.SaveSlotNumber}";
                        _lastKnownSlot = slot;
                        return slot;
                    }
                }
            }
            catch { }
            return _lastKnownSlot;
        }

        private static string GetSaveFilePath()
        {
            string suffix = GetActiveSlotSuffix();
            string path = SafeStorage.GetUserDataPath("BackpackMod", $"backpack_items_{suffix}.json");
            TryMigrateLegacy(path);
            return path;
        }

        private static void TryMigrateLegacy(string slotPath)
        {
            try
            {
                string legacy = SafeStorage.GetUserDataPath("BackpackMod", "backpack_items.json");
                // Also handle legacy in old BaseDirectory location
                string legacyOld = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "UserData", "BackpackMod", "backpack_items.json");
                foreach (var cand in new[] { legacy, legacyOld })
                {
                    if (!File.Exists(cand)) continue;
                    if (File.Exists(slotPath)) { try { File.Delete(cand); } catch { } continue; }
                    try
                    {
                        var data = SafeStorage.LoadTextSafe(cand, "", null);
                        if (!string.IsNullOrWhiteSpace(data))
                        {
                            SafeStorage.SaveTextAtomic(slotPath, data, null);
                            try { File.Delete(cand); } catch { }
                        }
                        else
                        {
                            File.Move(cand, slotPath);
                        }
                    }
                    catch { try { File.Move(cand, slotPath); } catch { } }
                    break;
                }
            }
            catch { }
        }

        public static void ResetCache(bool keepSlot = false)
        {
            SaveStorage();
            if (_storageEntityObj != null)
            {
                UnityEngine.Object.Destroy(_storageEntityObj);
                _storageEntityObj = null;
                _storageEntity = null;
                _currentSlotCount = 0;
            }
            if (!keepSlot) _lastKnownSlot = "default";
            _isBackpackStorageOpen = false;
        }

        public static void ResetForSceneUnload()
        {
            ResetCache(keepSlot: true);
        }

        public static void SaveStorage()
        {
            try
            {
                if (_storageEntity == null || _storageEntity.Pointer == IntPtr.Zero || _storageEntity.WasCollected || _storageEntity.ItemSlots == null) return;

                var items = new List<SavedItemData>();
                for (int i = 0; i < _storageEntity.ItemSlots.Count; i++)
                {
                    var slot = _storageEntity.ItemSlots[i];
                    if (slot != null && slot.Pointer != IntPtr.Zero && !slot.WasCollected && slot.ItemInstance != null && slot.ItemInstance.Pointer != IntPtr.Zero && !slot.ItemInstance.WasCollected && slot.ItemInstance.Definition != null)
                    {
                        items.Add(new SavedItemData
                        {
                            SlotIndex = i,
                            ItemId = slot.ItemInstance.Definition.ID,
                            Quantity = slot.Quantity
                        });
                    }
                }

                string json = JsonSerializer.Serialize(items, new JsonSerializerOptions { WriteIndented = true });
                SafeStorage.SaveTextAtomic(GetSaveFilePath(), json, null);
                Mod.Log?.Msg($"Saved {items.Count} items in backpack storage.");
            }
            catch (Exception ex)
            {
                Mod.Log?.Error($"Error saving backpack storage: {ex}");
            }
        }

        public static void LoadStorage()
        {
            try
            {
                if (_storageEntity == null || _storageEntity.ItemSlots == null || _storageEntity.Pointer == IntPtr.Zero || _storageEntity.WasCollected) return;
                string path = GetSaveFilePath();
                // Don't early-return on File.Exists — let LoadTextSafe handle .bak recovery (H1)

                string json = SafeStorage.LoadTextSafe(path, "", null);
                var items = JsonSerializer.Deserialize<List<SavedItemData>>(json);
                if (items == null) return;

                foreach (var item in items)
                {
                    if (item.SlotIndex >= 0 && item.SlotIndex < _storageEntity.ItemSlots.Count)
                    {
                        var def = Il2CppScheduleOne.Registry.GetItem(item.ItemId);
                        if (def != null)
                        {
                            var instance = def.GetDefaultInstance(item.Quantity);
                            if (instance != null)
                            {
                                _storageEntity.ItemSlots[item.SlotIndex].SetStoredItem(instance, true);
                            }
                        }
                    }
                }

                Mod.Log?.Msg($"Loaded {items.Count} items into backpack storage.");
            }
            catch (Exception ex)
            {
                Mod.Log?.Error($"Error loading backpack storage: {ex}");
            }
        }

        private class SavedItemData
        {
            public int SlotIndex { get; set; }
            public string ItemId { get; set; } = string.Empty;
            public int Quantity { get; set; } = 1;
        }
    }
}
