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
    /// <summary>Selectable property entry for the filter chips.</summary>
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

    /// <summary>Reads owned properties and world storage containers straight from the game.</summary>
    internal sealed class GameStorageSource : IStorageSource
    {
        private readonly List<PropertyWrapper> _owned = new List<PropertyWrapper>();
        private readonly List<PropertyOption> _properties = new List<PropertyOption>();
        private readonly Dictionary<string, PropertyStorageCache> _itemCache = new Dictionary<string, PropertyStorageCache>(StringComparer.Ordinal);
        private readonly HashSet<IntPtr> _loggedContainers = new HashSet<IntPtr>();
        private int _lastSceneStorageCount = -1;
        private int _lastRegistryStorageCount = -1;

        public IReadOnlyList<PropertyOption> Properties => _properties;

        public void RefreshPropertyCache()
        {
            _owned.Clear();
            _properties.Clear();
            try
            {
                List<PropertyWrapper> ownedProperties = PropertyManager.GetOwnedProperties();
                if (ownedProperties == null)
                {
                    return;
                }
                for (int i = 0; i < ownedProperties.Count; i++)
                {
                    PropertyWrapper wrapper = ownedProperties[i];
                    if (wrapper == null)
                    {
                        continue;
                    }
                    string code = wrapper.PropertyCode;
                    if (string.IsNullOrEmpty(code))
                    {
                        continue;
                    }
                    _owned.Add(wrapper);
                    _properties.Add(new PropertyOption(code, string.IsNullOrEmpty(wrapper.PropertyName) ? code : wrapper.PropertyName));
                }
            }
            catch (Exception ex)
            {
                MelonLogger.Warning("[StorageScanner] PropertyManager.GetOwnedProperties failed: " + ex.Message);
            }
        }

        /// <summary>Drops cached per-property item data; call when a save is loaded (containers are slot-specific).</summary>
        public void ClearItemCache()
        {
            _itemCache.Clear();
            _loggedContainers.Clear();
        }

        public bool IsReadyToScan()
        {
            // Exactly one property-cache refresh per scan cycle: Capture() reuses the primed _owned list.
            RefreshPropertyCache();
            // FIX (0.1.1): do NOT gate on IsContentCulled here. The game culls property
            // contents whenever the player is not right there, so the old per-property
            // cull check blocked every scan permanently ("Loading storage..." forever,
            // zero "Scan:" log lines). Culled properties are handled in Capture() below:
            // they are served from the last-known cache or reported as incomplete.
            return _owned.Count > 0;
        }

        public StorageSnapshot Capture()
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            List<StorageItem> items = new List<StorageItem>();
            bool complete = true;
            List<string> notes = new List<string>();
            int containers = 0;

            // Pass 1: detect which owned properties are currently culled.
            HashSet<string> culled = new HashSet<string>(StringComparer.Ordinal);
            for (int i = 0; i < _owned.Count; i++)
            {
                PropertyWrapper wrapper = _owned[i];
                if (wrapper == null)
                {
                    continue;
                }
                try
                {
                    if (wrapper.IsContentCulled)
                    {
                        complete = false;
                        culled.Add(wrapper.PropertyCode);
                    }
                }
                catch
                {
                }
            }

            // Pass 2: read every container of every live (non-culled) property.
            Dictionary<string, List<StorageItem>> liveByProperty = new Dictionary<string, List<StorageItem>>(StringComparer.Ordinal);
            bool enumerationOk = true;
            int unassignedContainers = 0;
            try
            {
                // Fixed rack shelving lives inside PlaceableStorageEntity/SurfaceStorageEntity wrappers and is
                // NOT registered in WorldStorageEntity.All; only a scene scan reaches the inner StorageEntity.
                List<StorageEntity> found = DiscoverStorageEntities();
                if (found != null)
                {
                    for (int j = 0; j < found.Count; j++)
                    {
                        StorageEntity entity = found[j];
                        if (entity == null || entity.WasCollected)
                        {
                            continue;
                        }
                        Vector3 position;
                        string containerName;
                        string containerId;
                        try
                        {
                            position = entity.transform.position;
                            containerName = entity.StorageEntityName;
                            containerId = entity.Pointer.ToInt64().ToString("x");
                        }
                        catch (Exception)
                        {
                            complete = false;
                            if (notes.Count < 3)
                            {
                                notes.Add("unreadable storage transform");
                            }
                            continue;
                        }
                        PropertyWrapper? owner = FindOwnedProperty(position);
                        if (owner == null)
                        {
                            unassignedContainers++;
                            continue;
                        }
                        if (culled.Contains(owner.PropertyCode))
                        {
                            // Contents of a culled property cannot be read; its containers do not count.
                            continue;
                        }
                        StorageInstance? instance;
                        try
                        {
                            instance = StorageInstance.FromGameObject(entity.gameObject);
                        }
                        catch (Exception)
                        {
                            complete = false;
                            if (notes.Count < 3)
                            {
                                notes.Add(containerName + ": wrap failed");
                            }
                            continue;
                        }
                        if (instance == null)
                        {
                            complete = false;
                            if (notes.Count < 3)
                            {
                                notes.Add(containerName + ": not wrappable");
                            }
                            continue;
                        }
                        containers++;
                        LogContainerOnce(entity, containerName, position, owner);
                        try
                        {
                            foreach (KeyValuePair<ItemInstance, int> item in instance.GetContentsDictionary())
                            {
                                ItemInstance stack = item.Key;
                                if (stack == null)
                                {
                                    continue;
                                }
                                int quantity = item.Value;
                                if (quantity <= 0)
                                {
                                    continue;
                                }
                                ItemDefinition? definition = stack.Definition;
                                if (definition == null)
                                {
                                    continue;
                                }
                                string itemId = definition.ID;
                                if (string.IsNullOrEmpty(itemId))
                                {
                                    continue;
                                }
                                string itemName = string.IsNullOrEmpty(definition.Name) ? itemId : definition.Name;
                                StorageItem storageItem = new StorageItem(owner.PropertyCode, owner.PropertyName, containerId, containerName, itemId, itemName, quantity, categoryId: definition.Category.ToString());
                                items.Add(storageItem);
                                GetLiveBucket(liveByProperty, owner.PropertyCode).Add(storageItem);
                            }
                        }
                        catch (Exception)
                        {
                            complete = false;
                            if (notes.Count < 3)
                            {
                                notes.Add(containerName + ": contents failed");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                enumerationOk = false;
                complete = false;
                notes.Add("world storage enumeration failed");
                MelonLogger.Warning("[StorageScanner] Storage enumeration failed: " + ex.Message);
            }

            // Pass 3: remember what could be read. Live data wins over stale data, so a property
            // readable in this pass always overwrites its previous cache entry.
            DateTimeOffset now = DateTimeOffset.Now;
            if (enumerationOk)
            {
                for (int i = 0; i < _owned.Count; i++)
                {
                    PropertyWrapper wrapper = _owned[i];
                    if (wrapper == null)
                    {
                        continue;
                    }
                    string code = wrapper.PropertyCode;
                    if (string.IsNullOrEmpty(code) || culled.Contains(code))
                    {
                        continue;
                    }
                    liveByProperty.TryGetValue(code, out List<StorageItem>? liveStacks);
                    string name = wrapper.PropertyName;
                    _itemCache[code] = new PropertyStorageCache(code, string.IsNullOrEmpty(name) ? code : name, liveStacks ?? new List<StorageItem>(), now);
                }
            }

            // Pass 4: serve culled properties from the last-known cache, clearly marked as cached.
            int cachedProperties = 0;
            for (int i = 0; i < _owned.Count; i++)
            {
                PropertyWrapper wrapper = _owned[i];
                if (wrapper == null)
                {
                    continue;
                }
                string code = wrapper.PropertyCode;
                if (string.IsNullOrEmpty(code) || !culled.Contains(code))
                {
                    continue;
                }
                if (_itemCache.TryGetValue(code, out PropertyStorageCache? entry))
                {
                    cachedProperties++;
                    double ageSeconds = (now - entry.UpdatedAt).TotalSeconds;
                    MelonLogger.Msg($"[StorageScanner] [CACHE] {entry.PropertyName} culled; using cached data from {entry.UpdatedAt:HH:mm:ss} ({ageSeconds:F0}s old)");
                    for (int k = 0; k < entry.Items.Count; k++)
                    {
                        items.Add(entry.Items[k].AsCached());
                    }
                }
                else if (notes.Count < 3)
                {
                    notes.Add(wrapper.PropertyName + " content culled (no cached data)");
                }
            }

            List<string> statusDetails = new List<string>();
            if (cachedProperties > 0)
            {
                statusDetails.Add(cachedProperties == 1 ? "1 property using cached data" : cachedProperties + " properties using cached data");
            }
            if (notes.Count > 0)
            {
                statusDetails.Add("incomplete - " + string.Join(", ", notes));
            }
            string status = statusDetails.Count == 0
                ? containers + " container(s)"
                : containers + " container(s); " + string.Join("; ", statusDetails);

            int liveProperties = _owned.Count - culled.Count;
            stopwatch.Stop();
            MelonLogger.Msg($"[StorageScanner] [SCAN] props {liveProperties}/{_owned.Count} live ({cachedProperties} cached) | containers {containers} (unassigned {unassignedContainers}) | stacks {items.Count} | {stopwatch.ElapsedMilliseconds} ms | complete={complete}");
            return new StorageSnapshot(items, complete, status, containers, _owned.Count, liveProperties, cachedProperties);
        }

        private static List<StorageItem> GetLiveBucket(Dictionary<string, List<StorageItem>> map, string propertyCode)
        {
            if (!map.TryGetValue(propertyCode, out List<StorageItem>? bucket))
            {
                bucket = new List<StorageItem>();
                map.Add(propertyCode, bucket);
            }
            return bucket;
        }

        /// <summary>Collects every storage container in the loaded scene (scene scan + registry), de-duplicated by native pointer.</summary>
        private List<StorageEntity> DiscoverStorageEntities()
        {
            List<StorageEntity> result = new List<StorageEntity>();
            HashSet<IntPtr> seen = new HashSet<IntPtr>();
            int sceneCount = 0;
            int registryCount = 0;
            try
            {
                var sceneEntities = UnityEngine.Object.FindObjectsByType<StorageEntity>(FindObjectsSortMode.None);
                if (sceneEntities != null)
                {
                    sceneCount = sceneEntities.Length;
                    for (int i = 0; i < sceneEntities.Length; i++)
                    {
                        StorageEntity entity = sceneEntities[i];
                        if (entity != null && !entity.WasCollected && seen.Add(entity.Pointer))
                        {
                            result.Add(entity);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MelonLogger.Warning("[StorageScanner] Scene storage scan failed: " + ex.Message);
            }
            try
            {
                var registry = WorldStorageEntity.All;
                if (registry != null)
                {
                    registryCount = registry.Count;
                    for (int i = 0; i < registry.Count; i++)
                    {
                        WorldStorageEntity entity = registry[i];
                        if (entity != null && !entity.WasCollected && seen.Add(entity.Pointer))
                        {
                            result.Add(entity);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MelonLogger.Warning("[StorageScanner] Registry storage scan failed: " + ex.Message);
            }
            if (sceneCount != _lastSceneStorageCount || registryCount != _lastRegistryStorageCount)
            {
                _lastSceneStorageCount = sceneCount;
                _lastRegistryStorageCount = registryCount;
                MelonLogger.Msg($"[StorageScanner] [DISCOVER] scene storages {sceneCount}, registry {registryCount}, unique {result.Count}");
            }
            return result;
        }

        /// <summary>Logs each container once per session so the first in-game round can classify what was found.</summary>
        private void LogContainerOnce(StorageEntity entity, string containerName, Vector3 position, PropertyWrapper owner)
        {
            try
            {
                if (!_loggedContainers.Add(entity.Pointer) || _loggedContainers.Count > 40)
                {
                    return;
                }
                MelonLogger.Msg($"[StorageScanner] [CONTAINER] '{containerName}' prop '{owner.PropertyName}' pos ({position.x:F0},{position.y:F0},{position.z:F0}) id {entity.Pointer.ToInt64():x}");
            }
            catch (Exception)
            {
            }
        }

        private PropertyWrapper? FindOwnedProperty(Vector3 pos)
        {
            for (int i = 0; i < _owned.Count; i++)
            {
                PropertyWrapper wrapper = _owned[i];
                if (wrapper == null)
                {
                    continue;
                }
                try
                {
                    if (wrapper.IsPointInside(pos))
                    {
                        return wrapper;
                    }
                }
                catch
                {
                }
            }
            return null;
        }
    }
}
