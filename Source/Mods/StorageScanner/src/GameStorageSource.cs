using System;
using System.Collections.Generic;
using System.Diagnostics;
using Il2CppScheduleOne.Storage;
using MelonLoader;
using S1API.Items;
using S1API.Property;
using S1API.Storages;
using UnityEngine;

namespace StorageScanner
{
    public sealed class PropertyOption
    {
        public string Code { get; }
        public string Name { get; }

        public PropertyOption(string code, string name)
        {
            Code = code;
            Name = name;
        }
    }

    internal sealed class GameStorageSource : IStorageSource
    {
        private readonly List<PropertyWrapper> _owned = new List<PropertyWrapper>();
        private readonly List<PropertyOption> _properties = new List<PropertyOption>();
        private readonly PropertyStorageCache _cache = new PropertyStorageCache();

        public IReadOnlyList<PropertyOption> Properties => _properties;

        public void ResetForNewSave()
        {
            _owned.Clear();
            _properties.Clear();
            _cache.Clear();
        }

        public void RefreshPropertyCache()
        {
            _owned.Clear();
            _properties.Clear();

            try
            {
                List<PropertyWrapper> ownedProperties = PropertyManager.GetOwnedProperties();
                if (ownedProperties == null) return;

                for (int i = 0; i < ownedProperties.Count; i++)
                {
                    PropertyWrapper wrapper = ownedProperties[i];
                    if (wrapper == null) continue;

                    string code = wrapper.PropertyCode;
                    if (string.IsNullOrEmpty(code)) continue;

                    _owned.Add(wrapper);
                    _properties.Add(new PropertyOption(code,
                        string.IsNullOrEmpty(wrapper.PropertyName) ? code : wrapper.PropertyName));
                }
            }
            catch (Exception ex)
            {
                MelonLogger.Warning("[StorageScanner] [SCAN] Property refresh failed: " + ex.Message);
            }
        }

        public bool IsReadyToScan()
        {
            // This is the only property refresh in one controller scan cycle. Capture() reuses it.
            RefreshPropertyCache();
            return _owned.Count > 0;
        }

        public StorageSnapshot Capture()
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            DateTimeOffset capturedAt = DateTimeOffset.Now;
            var liveItemsByProperty = new Dictionary<string, List<StorageItem>>(StringComparer.Ordinal);
            var liveContainersByProperty = new Dictionary<string, HashSet<string>>(StringComparer.Ordinal);
            var isCulled = new Dictionary<string, bool>(StringComparer.Ordinal);
            var allItems = new List<StorageItem>();
            var states = new List<PropertyScanState>();
            var notes = new List<string>();

            for (int i = 0; i < _owned.Count; i++)
            {
                PropertyWrapper wrapper = _owned[i];
                string code = wrapper.PropertyCode;
                liveItemsByProperty[code] = new List<StorageItem>();
                liveContainersByProperty[code] = new HashSet<string>(StringComparer.Ordinal);

                bool culled = false;
                try { culled = wrapper.IsContentCulled; }
                catch { culled = true; }
                isCulled[code] = culled;
            }

            try
            {
                var world = WorldStorageEntity.All;
                if (world != null)
                {
                    for (int i = 0; i < world.Count; i++)
                    {
                        WorldStorageEntity entity = world[i];
                        if (entity == null || entity.Pointer == IntPtr.Zero || entity.WasCollected) continue;

                        Vector3 position;
                        string containerName;
                        string containerId;
                        try
                        {
                            position = entity.transform.position;
                            containerName = string.IsNullOrEmpty(entity.StorageEntityName) ? "Storage" : entity.StorageEntityName;
                            containerId = entity.Pointer.ToInt64().ToString("x");
                        }
                        catch
                        {
                            AddNote(notes, "unreadable storage transform");
                            continue;
                        }

                        PropertyWrapper? owner = FindOwnedProperty(position);
                        if (owner == null) continue;
                        string propertyId = owner.PropertyCode;
                        if (isCulled.TryGetValue(propertyId, out bool culled) && culled) continue;

                        StorageInstance? instance;
                        try { instance = StorageInstance.FromGameObject(entity.gameObject); }
                        catch
                        {
                            AddNote(notes, containerName + ": wrap failed");
                            continue;
                        }
                        if (instance == null)
                        {
                            AddNote(notes, containerName + ": not wrappable");
                            continue;
                        }

                        liveContainersByProperty[propertyId].Add(containerId);
                        try
                        {
                            foreach (KeyValuePair<ItemInstance, int> pair in instance.GetContentsDictionary())
                            {
                                ItemInstance stack = pair.Key;
                                if (stack == null || pair.Value <= 0) continue;
                                ItemDefinition? definition = stack.Definition;
                                if (definition == null || string.IsNullOrEmpty(definition.ID)) continue;

                                liveItemsByProperty[propertyId].Add(new StorageItem(
                                    propertyId,
                                    owner.PropertyName,
                                    containerId,
                                    containerName,
                                    definition.ID,
                                    string.IsNullOrEmpty(definition.Name) ? definition.ID : definition.Name,
                                    pair.Value));
                            }
                        }
                        catch
                        {
                            AddNote(notes, containerName + ": contents failed");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                AddNote(notes, "world storage enumeration failed");
                MelonLogger.Warning("[StorageScanner] [SCAN] Storage enumeration failed: " + ex.Message);
            }

            int totalContainers = 0;
            for (int i = 0; i < _owned.Count; i++)
            {
                PropertyWrapper wrapper = _owned[i];
                string propertyId = wrapper.PropertyCode;
                string propertyName = string.IsNullOrEmpty(wrapper.PropertyName) ? propertyId : wrapper.PropertyName;

                if (isCulled[propertyId])
                {
                    if (_cache.TryGet(propertyId, out IReadOnlyList<StorageItem> cached,
                        out int cachedContainers, out DateTimeOffset updatedAt))
                    {
                        for (int j = 0; j < cached.Count; j++) allItems.Add(cached[j]);
                        totalContainers += cachedContainers;
                        states.Add(new PropertyScanState(propertyId, propertyName, false, updatedAt, cachedContainers));
                        AddNote(notes, propertyName + " cached");
                        MelonLogger.Msg($"[StorageScanner] [CACHE] {propertyName} culled; using cached snapshot from {updatedAt:HH:mm:ss}.");
                    }
                    else
                    {
                        states.Add(new PropertyScanState(propertyId, propertyName, false, capturedAt, 0));
                        AddNote(notes, propertyName + " unavailable");
                    }
                    continue;
                }

                List<StorageItem> liveItems = liveItemsByProperty[propertyId];
                int liveContainers = liveContainersByProperty[propertyId].Count;
                for (int j = 0; j < liveItems.Count; j++) allItems.Add(liveItems[j]);
                totalContainers += liveContainers;
                states.Add(new PropertyScanState(propertyId, propertyName, true, capturedAt, liveContainers));
                _cache.Store(propertyId, liveItems, liveContainers, capturedAt);
            }

            stopwatch.Stop();
            bool complete = notes.Count == 0 && states.TrueForAll(x => x.IsLive);
            string status = complete
                ? $"{totalContainers} container(s)"
                : $"{totalContainers} container(s); partial - {string.Join(", ", notes)}";

            MelonLogger.Msg($"[StorageScanner] [SCAN] {allItems.Count} stack(s), {totalContainers} container(s), " +
                $"{states.Count} propertie(s), {stopwatch.ElapsedMilliseconds} ms, complete={complete}");

            return new StorageSnapshot(allItems, states, complete, status, totalContainers,
                stopwatch.ElapsedMilliseconds, capturedAt);
        }

        private PropertyWrapper? FindOwnedProperty(Vector3 position)
        {
            for (int i = 0; i < _owned.Count; i++)
            {
                PropertyWrapper wrapper = _owned[i];
                if (wrapper == null) continue;
                try
                {
                    if (wrapper.IsPointInside(position)) return wrapper;
                }
                catch { }
            }
            return null;
        }

        private static void AddNote(List<string> notes, string note)
        {
            if (notes.Count < 4 && !notes.Contains(note)) notes.Add(note);
        }
    }
}
