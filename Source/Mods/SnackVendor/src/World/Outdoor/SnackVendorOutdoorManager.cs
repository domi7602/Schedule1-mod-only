using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Il2CppInterop.Runtime;
using Il2CppScheduleOne.Building;
using Il2CppScheduleOne.DevUtilities;
using MelonLoader.Utils;
using S1API.Lifecycle;
using S1Mods.Shared;
using SnackVendor.Items;
using UnityEngine;
using NativeBuildableItemDef = Il2CppScheduleOne.ItemFramework.BuildableItemDefinition;

namespace SnackVendor.World.Outdoor;

public sealed class OutdoorStationRecord
{
    public string Guid { get; set; } = string.Empty;
    public float PosX { get; set; }
    public float PosY { get; set; }
    public float PosZ { get; set; }
    public float RotX { get; set; }
    public float RotY { get; set; }
    public float RotZ { get; set; }
    public float RotW { get; set; }
}

public sealed class OutdoorStationsSaveData
{
    public int SaveVersion { get; set; } = 1;
    public List<OutdoorStationRecord> Stations { get; set; } = new();
}

public sealed class ActiveOutdoorStation
{
    public GameObject GameObject { get; }
    public string Guid { get; }

    public ActiveOutdoorStation(GameObject go, string guid)
    {
        GameObject = go;
        Guid = guid;
    }
}

/// <summary>
/// Manages virtual outdoor root container, runtime tracking, and save-slot isolated
/// persistence for standalone outdoor SnackVendors.
/// </summary>
public static class SnackVendorOutdoorManager
{
    private static GameObject? _outdoorRoot;
    private static readonly List<ActiveOutdoorStation> _activeStations = new();
    private const string FilePattern = "outdoor_stations_slot_{0}.json";
    private const string DirName = "SnackVendor";

    public static GameObject OutdoorRoot
    {
        get
        {
            if (_outdoorRoot == null || _outdoorRoot.Pointer == IntPtr.Zero)
            {
                _outdoorRoot = new GameObject("SnackVendor_OutdoorRoot");
                GameObject.DontDestroyOnLoad(_outdoorRoot);
            }
            return _outdoorRoot;
        }
    }

    public static bool HasOutdoorRoot => _outdoorRoot != null && _outdoorRoot.Pointer != IntPtr.Zero;

    public static void RegisterOutdoorStation(GameObject go, string guid)
    {
        if (go == null || go.Pointer == IntPtr.Zero) return;
        if (string.IsNullOrEmpty(guid)) guid = System.Guid.NewGuid().ToString();

        _activeStations.RemoveAll(s => s.GameObject == null || s.GameObject.Pointer == IntPtr.Zero || s.GameObject.WasCollected || s.GameObject == go);
        _activeStations.Add(new ActiveOutdoorStation(go, guid));
        Mod.Log.Info($"Registered outdoor station (GUID: {guid}, Active: {_activeStations.Count})");
    }

    public static void UnregisterOutdoorStation(GameObject go)
    {
        if (go == null) return;
        int removed = _activeStations.RemoveAll(s => s.GameObject == go);
        if (removed > 0)
        {
            Mod.Log.Info($"Unregistered outdoor station. Remaining active: {_activeStations.Count}");
        }
    }

    /// <summary>
    /// Checks if a GameObject or any of its parents is a registered outdoor SnackVendor station.
    /// Used by Harmony guards to prevent vanilla property culling and destruction.
    /// </summary>
    public static bool IsOutdoorStation(GameObject? go)
    {
        if (go == null || go.Pointer == IntPtr.Zero || go.WasCollected) return false;

        try
        {
            int instanceId = go.GetInstanceID();
            for (int i = 0; i < _activeStations.Count; i++)
            {
                var stationGo = _activeStations[i].GameObject;
                if (stationGo != null && stationGo.Pointer != IntPtr.Zero && !stationGo.WasCollected)
                {
                    if (stationGo.GetInstanceID() == instanceId) return true;
                }
            }

            Transform? curr = go.transform;
            while (curr != null && curr.Pointer != IntPtr.Zero && !curr.WasCollected)
            {
                var currGo = curr.gameObject;
                if (currGo != null && currGo.Pointer != IntPtr.Zero && !currGo.WasCollected)
                {
                    if (HasOutdoorRoot && currGo == _outdoorRoot)
                        return true;
                    if (currGo.GetComponent<SnackVendorOutdoorInteractable>() != null)
                        return true;
                }
                curr = curr.parent;
            }
        }
        catch { }

        return false;
    }

    private static string GetSaveFilePath(int slot)
    {
        return Path.Combine(MelonEnvironment.UserDataDirectory, DirName, string.Format(FilePattern, slot));
    }

    public static void ResetState()
    {
        try
        {
            for (int i = 0; i < _activeStations.Count; i++)
            {
                var go = _activeStations[i].GameObject;
                if (go != null && go.Pointer != IntPtr.Zero && !go.WasCollected)
                {
                    UnityEngine.Object.Destroy(go);
                }
            }
            _activeStations.Clear();
            Mod.Log.Info("ResetState: cleared outdoor snack vendor stations.");
        }
        catch (Exception ex)
        {
            Mod.Log.Warn("ResetState failed", ex);
        }
    }

    public static void SaveOutdoorStations()
    {
        if (!Mod.CurrentConfig.EnableStandaloneOutdoorPlacement) return;

        int slot = SaveSlots.GetActiveSlotNumber();
        if (slot < 0) return;

        try
        {
            var data = new OutdoorStationsSaveData();
            for (int i = 0; i < _activeStations.Count; i++)
            {
                var entry = _activeStations[i];
                if (entry.GameObject == null || entry.GameObject.Pointer == IntPtr.Zero || entry.GameObject.WasCollected)
                    continue;

                var t = entry.GameObject.transform;
                data.Stations.Add(new OutdoorStationRecord
                {
                    Guid = entry.Guid,
                    PosX = t.position.x,
                    PosY = t.position.y,
                    PosZ = t.position.z,
                    RotX = t.rotation.x,
                    RotY = t.rotation.y,
                    RotZ = t.rotation.z,
                    RotW = t.rotation.w
                });
            }

            string path = GetSaveFilePath(slot);
            string json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
            SafeStorage.SaveAtomic(path, json);
            Mod.Log.Info($"Saved {data.Stations.Count} outdoor snack vendor(s) to slot {slot}.");
        }
        catch (Exception ex)
        {
            Mod.Log.Error("SaveOutdoorStations failed", ex);
        }
    }

    public static void LoadAndSpawnOutdoorStations()
    {
        if (!Mod.CurrentConfig.EnableStandaloneOutdoorPlacement) return;

        int slot = SaveSlots.GetActiveSlotNumber();
        if (slot < 0) return;

        string path = GetSaveFilePath(slot);
        if (!File.Exists(path)) return;

        try
        {
            string json = File.ReadAllText(path);
            var data = JsonSerializer.Deserialize<OutdoorStationsSaveData>(json);
            if (data?.Stations == null || data.Stations.Count == 0) return;

            var def = Il2CppScheduleOne.Registry.GetItem(Mod.CurrentConfig.StationItemId);
            var bDef = def?.TryCast<NativeBuildableItemDef>();
            if (bDef == null || bDef.BuiltItem == null)
            {
                Mod.Log.Warn("LoadAndSpawnOutdoorStations: BuiltItem prefab not found in Registry!");
                return;
            }

            foreach (var rec in data.Stations)
            {
                if (string.IsNullOrEmpty(rec.Guid)) continue;
                if (_activeStations.Any(s => s.Guid.Equals(rec.Guid, StringComparison.OrdinalIgnoreCase)))
                    continue;

                Vector3 pos = new Vector3(rec.PosX, rec.PosY, rec.PosZ);
                Quaternion rot = new Quaternion(rec.RotX, rec.RotY, rec.RotZ, rec.RotW);

                bool wasActive = bDef.BuiltItem.gameObject.activeSelf;
                GameObject placedObj;
                try
                {
                    if (wasActive) bDef.BuiltItem.gameObject.SetActive(false);
                    placedObj = GameObject.Instantiate(bDef.BuiltItem.gameObject, pos, rot);
                }
                finally
                {
                    if (wasActive) bDef.BuiltItem.gameObject.SetActive(true);
                }

                placedObj.transform.SetParent(OutdoorRoot.transform, true);

                var buildMgr = NetworkSingleton<BuildManager>.Instance;
                if (buildMgr != null && buildMgr.Pointer != IntPtr.Zero)
                {
                    buildMgr.DisableNetworking(placedObj);
                    buildMgr.DisableNavigation(placedObj);
                }

                var interactable = placedObj.AddComponent<SnackVendorOutdoorInteractable>();
                interactable.Guid = rec.Guid;

                SnackVendorItemFactory.SetupPlacedStation(placedObj, rec.Guid);
                RegisterOutdoorStation(placedObj, rec.Guid);
                placedObj.SetActive(true);
            }

            Mod.Log.Info($"Restored {data.Stations.Count} outdoor snack vendor(s) from slot {slot}.");
        }
        catch (Exception ex)
        {
            Mod.Log.Error("LoadAndSpawnOutdoorStations failed", ex);
        }
    }
}
