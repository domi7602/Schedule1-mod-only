using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Il2CppFishNet;
using Il2CppFishNet.Managing.Object;
using Il2CppFishNet.Object;
using Il2CppScheduleOne.Persistence.Datas;
using Il2CppScheduleOne.Storage;
using MelonLoader;
using UnityEngine;

namespace TownHallInterior;

/// <summary>
/// Places the game's existing FilingCabinet_Built prefab where the decorative
/// filing cabinets stand. Does not create an item, a definition or a slot list.
/// </summary>
public sealed class TownHallCabinetStorage
{
    public const string PrefabName = "FilingCabinet_Built";

    private readonly List<NetworkObject> _spawned = new();
    private readonly Dictionary<string, GameObject> _hosts = new(StringComparer.Ordinal);
    private readonly HashSet<string> _restored = new(StringComparer.Ordinal);
    private List<HallCabinetRecord>? _records;
    private bool _restoreFailed;
    private bool _placed;
    private bool _missingLogged;

    public bool IsPlaced => _placed;

    public void Bind(List<HallCabinetRecord> records) => _records = records;

    public void CaptureContents()
    {
        if (_records == null || !_placed || _restoreFailed)
            throw new InvalidOperationException("Aktenschränke sind noch nicht vollständig geladen.");
        var captured = _records.FindAll(record => !_hosts.ContainsKey(record.Name));
        foreach (var host in _hosts)
            captured.Add(new HallCabinetRecord { Name = host.Key, ItemSetJson = NativeHallStorage.Read(host.Value) });
        _records.Clear();
        _records.AddRange(captured);
        MelonLogger.Msg($"TownHallInterior: captured {_records.Count} cabinet contents for the checkpoint.");
    }

    public void TryPlace()
    {
        if (_placed || _records == null || _restoreFailed) return;
        try
        {
            var manager = InstanceFinder.NetworkManager;
            if (manager == null || !manager.IsServer) return;
            if (!HallGrid.CheckpointSaveHookReady)
                throw new InvalidOperationException("Native save exclusion is unavailable for Rathaus cabinets.");

            var prefab = FindPrefab(manager);
            if (prefab == null)
            {
                if (!_missingLogged)
                {
                    _missingLogged = true;
                    MelonLogger.Warning("TownHallInterior: FilingCabinet_Built was not in the spawnable prefabs.");
                }
                return;
            }

            var hosts = Hosts();
            var hall = GameObject.Find("TownHallInterior.VanillaReception");
            foreach (var host in hosts)
            {
                if (!_hosts.ContainsKey(host.Name) && !SpawnOne(manager, prefab, host, hall)) return;
                if (_restored.Contains(host.Name)) continue;
                var record = _records.Find(value => value.Name == host.Name);
                if (record != null) NativeHallStorage.Restore(_hosts[host.Name], record.ItemSetJson);
                _restored.Add(host.Name);
            }

            _placed = true;
            MelonLogger.Msg("TownHallInterior: placed " + hosts.Count +
                " existing Filing Cabinet prefabs; decorative meshes hidden.");
        }
        catch (Exception ex)
        {
            _restoreFailed = true;
            foreach (var host in _hosts.Values)
                if (host != null) host.SetActive(false);
            MelonLogger.Warning("TownHallInterior filing cabinet place failed: " + ex.Message);
        }
    }

    public void Release()
    {
        var manager = InstanceFinder.NetworkManager;
        foreach (var net in _spawned)
        {
            try
            {
                if (net == null || net.Pointer == IntPtr.Zero) continue;
                if (manager != null && manager.IsServer)
                    manager.ServerManager.Despawn(net);
                else if (net.gameObject != null)
                    UnityEngine.Object.Destroy(net.gameObject);
            }
            catch (Exception ex)
            {
                MelonLogger.Warning("TownHallInterior filing cabinet despawn failed: " + ex.Message);
            }
        }
        _spawned.Clear();
        _hosts.Clear();
        _restored.Clear();
        _records = null;
        _restoreFailed = false;
        _placed = false;
    }

    private bool SpawnOne(Il2CppFishNet.Managing.NetworkManager manager, GameObject prefab, FurniturePlacement host, GameObject? hall)
    {
        bool wasActive = prefab.activeSelf;
        prefab.SetActive(false);
        var instance = UnityEngine.Object.Instantiate(prefab);
        prefab.SetActive(wasActive);
        if (instance == null) return false;

        instance.name = "TownHall-" + host.Name;
        HallGrid.RegisterCheckpointOwned(instance);
        var rotation = Quaternion.Euler(0f, FurniturePlan.FilingCabinetYaw(host.Yaw), 0f);
        instance.transform.SetPositionAndRotation(new Vector3(host.X, host.Y, host.Z), rotation);
        AssignStableGuid(instance, host.Name);

        var net = instance.GetComponent<NetworkObject>();
        if (net == null)
        {
            UnityEngine.Object.Destroy(instance);
            MelonLogger.Warning("TownHallInterior: FilingCabinet_Built has no NetworkObject.");
            return false;
        }

        manager.ServerManager.Spawn(net);
        if (hall != null)
            instance.transform.SetParent(hall.transform, true);
        instance.transform.rotation = rotation;
        instance.SetActive(true);
        var deco = hall == null ? null : hall.transform.Find(host.Name);
        if (deco != null) deco.gameObject.SetActive(false);
        _spawned.Add(net);
        _hosts[host.Name] = instance;
        return true;
    }

    private static GameObject? FindPrefab(Il2CppFishNet.Managing.NetworkManager manager)
    {
        var prefabs = manager.GetPrefabObjects<PrefabObjects>(0, false);
        if (prefabs == null) return null;
        int count = prefabs.GetObjectCount();
        for (int i = 0; i < count; i++)
        {
            var obj = prefabs.GetObject(true, i);
            if (obj != null && obj.gameObject != null && obj.gameObject.name == PrefabName)
                return obj.gameObject;
        }
        return null;
    }

    private static IReadOnlyList<FurniturePlacement> Hosts()
    {
        using var stream = typeof(TownHallCabinetStorage).Assembly.GetManifestResourceStream("TownHallInterior.NativeFurniture")
            ?? throw new InvalidOperationException("Local vanilla furniture extraction missing");
        using var bytes = new System.IO.MemoryStream();
        stream.CopyTo(bytes);
        return FurniturePlan.StorageHosts(FurniturePlan.Read(bytes.ToArray()));
    }

    private static void AssignStableGuid(GameObject instance, string name)
    {
        string guid = StableGuid(name);
        foreach (var component in instance.GetComponentsInChildren<MonoBehaviour>(true))
        {
            if (component == null) continue;
            foreach (var field in component.GetType().GetFields(
                         System.Reflection.BindingFlags.Public |
                         System.Reflection.BindingFlags.NonPublic |
                         System.Reflection.BindingFlags.Instance))
            {
                if (field.FieldType != typeof(string)) continue;
                if (!field.Name.Equals("GUID", StringComparison.OrdinalIgnoreCase) &&
                    !field.Name.Equals("guid", StringComparison.OrdinalIgnoreCase))
                    continue;
                field.SetValue(component, guid);
            }
        }
    }

    private static string StableGuid(string name)
    {
        byte[] hash = System.Security.Cryptography.SHA256.HashData(
            System.Text.Encoding.UTF8.GetBytes("townhall-filingcabinet:" + name));
        var bytes = new byte[16];
        Array.Copy(hash, bytes, 16);
        bytes[6] = (byte)((bytes[6] & 0x0F) | 0x50);
        bytes[8] = (byte)((bytes[8] & 0x3F) | 0x80);
        return new Guid(bytes).ToString();
    }
}

// One checkpoint owner for both storage types; preserve full native item data.
internal static class NativeHallStorage
{
    internal static string Read(GameObject placed)
    {
        var storage = placed.GetComponentInChildren<StorageEntity>(true)
            ?? throw new InvalidDataException("Rathausobjekt besitzt kein Lager.");
        var slots = storage.ItemSlots;
        if (slots == null || slots.Count == 0)
            throw new InvalidDataException("Rathaus-Lagerfächer sind noch nicht initialisiert.");
        string json = new ItemSet(slots).GetJSON();
        HallStorageSnapshot.Validate(json);
        using var document = JsonDocument.Parse(json);
        var saved = document.RootElement.GetProperty("Items");
        if (saved.GetArrayLength() != slots.Count || slots.Count != storage.SlotCount)
            throw new InvalidDataException("Rathaus-Lager ist nicht vollständig initialisiert oder serialisiert.");
        for (int i = 0; i < slots.Count; i++)
        {
            var slot = slots[i] ?? throw new InvalidDataException("Fehlendes Rathaus-Lagerfach.");
            var item = slot.ItemInstance ?? slot._ItemInstance_k__BackingField;
            string? value = saved[i].ValueKind == JsonValueKind.Null ? null : saved[i].GetString();
            if (item == null)
            {
                if (slot.Quantity != 0 || !HallStorageSnapshot.IsEmptyItem(value))
                    throw new InvalidDataException("Rathaus-Lagerfach und serialisierter Inhalt widersprechen sich.");
                continue;
            }
            if (HallStorageSnapshot.IsEmptyItem(value))
                throw new InvalidDataException("Belegtes Rathaus-Lagerfach wurde leer serialisiert.");
            using var data = JsonDocument.Parse(value!);
            if (data.RootElement.GetProperty("ID").GetString() != item.Definition?.ID ||
                data.RootElement.GetProperty("Quantity").GetInt32() != slot.Quantity)
                throw new InvalidDataException("Rathaus-Lageritem wurde nicht vollständig serialisiert.");
        }
        return json;
    }

    internal static void Restore(GameObject placed, string json)
    {
        HallStorageSnapshot.Validate(json);
        var slots = placed.GetComponentInChildren<StorageEntity>(true)?.ItemSlots;
        if (slots == null || slots.Count == 0 || !ItemSet.TryDeserialize(json, out var contents) || contents?.Items == null)
            throw new InvalidDataException("Rathaus-Lagerinhalt kann noch nicht geladen werden.");
        using var document = JsonDocument.Parse(json);
        var saved = document.RootElement.GetProperty("Items");
        if (saved.GetArrayLength() > slots.Count || contents.Items.Length != saved.GetArrayLength())
            throw new InvalidDataException("Rathaus-Lagergröße stimmt nicht mit dem Checkpoint überein.");
        for (int i = 0; i < saved.GetArrayLength(); i++)
        {
            string? value = saved[i].ValueKind == JsonValueKind.Null ? null : saved[i].GetString();
            if (!HallStorageSnapshot.IsEmptyItem(value) && contents.Items[i] == null)
                throw new InvalidDataException("Ein Rathaus-Lageritem konnte nicht rekonstruiert werden.");
        }
        contents.LoadTo(slots);
        using var observed = JsonDocument.Parse(Read(placed));
        var actual = observed.RootElement.GetProperty("Items");
        if (saved.GetArrayLength() != actual.GetArrayLength())
            throw new InvalidDataException("Rathaus-Lagerrestore konnte nicht bestätigt werden.");
        for (int i = 0; i < saved.GetArrayLength(); i++)
            if (saved[i].GetString() != actual[i].GetString())
                throw new InvalidDataException("Rathaus-Lagerrestore stimmt nicht mit dem gespeicherten Inhalt überein.");
    }
}
