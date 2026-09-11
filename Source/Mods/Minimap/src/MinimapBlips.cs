using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Il2CppInterop.Runtime;
using Il2CppScheduleOne.Economy;
using Il2CppScheduleOne.Map;
using Il2CppScheduleOne.NPCs;
using Il2CppScheduleOne.PlayerScripts;
using Il2CppScheduleOne.Police;
using Il2CppScheduleOne.Property;
using Il2CppScheduleOne.Quests;
using Il2CppScheduleOne.Vehicles;
using UnityEngine;
using UnityEngine.UI;

namespace Minimap;

public enum BlipType
{
    Police,
    ActiveDeal,
    PotentialCustomer,
    KnownCustomer,
    Property,
    SleepingBag,
    Shop,
    Quest,
    Vehicle,
    GenericDot
}

public sealed class MinimapBlips
{
    public struct BlipInfo
    {
        public Vector3 WorldPos;
        public BlipType Type;
        public Color Color;
        public string Label;
        public float CustomScale;
        public bool IsPulsing;
        public bool IsCritical; // Whether it should be edge-clamped when out of view
    }

    private sealed class PooledBlip
    {
        public GameObject Root;
        public RectTransform Rect;
        public Image Icon;
        public CanvasGroup Group;

        public PooledBlip(GameObject root, RectTransform rect, Image icon, CanvasGroup group)
        {
            Root = root;
            Rect = rect;
            Icon = icon;
            Group = group;
        }
    }

    private const int PoolSize = 64;
    private readonly List<PooledBlip> _pool = new(PoolSize);
    private readonly List<BlipInfo> _activeBlips = new(PoolSize);
    private Transform? _blipContainer;

    // Cache static landmarks so we only search them occasionally
    private readonly List<BlipInfo> _staticLandmarks = new(16);
    private float _lastLandmarkScanTime = -999f;
    private const float LandmarkScanInterval = 10.0f; // Cache properties/shops every 10s

    private float _lastDynamicScanTime = -999f;
    private const float DynamicScanInterval = 1.0f; // Scan NPCs every 1.0s

    public void Initialize(Transform blipContainer)
    {
        _blipContainer = blipContainer;
        _pool.Clear();

        for (int i = 0; i < PoolSize; i++)
        {
            GameObject obj = new GameObject($"Blip_{i}", Il2CppType.Of<RectTransform>());
            obj.transform.SetParent(_blipContainer, false);

            RectTransform rt = obj.GetComponent<RectTransform>();
            rt.anchorMin = new Vector2(0.5f, 0.5f);
            rt.anchorMax = new Vector2(0.5f, 0.5f);
            rt.pivot = new Vector2(0.5f, 0.5f);
            rt.sizeDelta = new Vector2(16f, 16f);

            Image img = obj.AddComponent<Image>();
            img.sprite = MinimapTextures.GetBlipDot();
            img.raycastTarget = false;

            CanvasGroup group = obj.AddComponent<CanvasGroup>();
            group.blocksRaycasts = false;
            group.interactable = false;

            obj.SetActive(false);
            _pool.Add(new PooledBlip(obj, rt, img, group));
        }
    }

    public void InvalidateLandmarkCache()
    {
        _lastLandmarkScanTime = -999f;
    }

    private void UpdateStaticLandmarks(MinimapConfig config, float now)
    {
        if (now - _lastLandmarkScanTime < LandmarkScanInterval)
            return;

        _lastLandmarkScanTime = now;
        _staticLandmarks.Clear();

        // 1. Owned Properties
        if (config.ShowPropertyBlips)
        {
            try
            {
                var owned = Property.OwnedProperties;
                if (owned != null)
                {
                    int count = owned.Count;
                    for (int i = 0; i < count; i++)
                    {
                        var prop = owned[i];
                        if (prop != null && (UnityEngine.Object)prop != null)
                        {
                            _staticLandmarks.Add(new BlipInfo
                            {
                                WorldPos = prop.transform.position,
                                Type = BlipType.Property,
                                Color = new Color(0.95f, 0.61f, 0.07f, 1f), // Amber
                                Label = prop.PropertyName ?? "Property",
                                CustomScale = 1.15f,
                                IsPulsing = false,
                                IsCritical = true // Always clamp owned bases
                            });
                        }
                    }
                }
            }
            catch { }
        }

        // 2. Fixed Shops & Stations
        if (config.ShowShopBlips)
        {
            try
            {
                if (Map.Instance != null && (UnityEngine.Object)Map.Instance != null)
                {
                    var policeStation = Map.Instance.PoliceStation;
                    if (policeStation != null && (UnityEngine.Object)policeStation != null && config.ShowPoliceBlips)
                    {
                        _staticLandmarks.Add(new BlipInfo
                        {
                            WorldPos = policeStation.transform.position,
                            Type = BlipType.Police,
                            Color = new Color(0.20f, 0.55f, 0.95f, 1f),
                            Label = "Police Station",
                            CustomScale = 1.10f,
                            IsPulsing = false,
                            IsCritical = false
                        });
                    }

                    var med = Map.Instance.MedicalCentre;
                    if (med != null && (UnityEngine.Object)med != null)
                    {
                        _staticLandmarks.Add(new BlipInfo
                        {
                            WorldPos = med.transform.position,
                            Type = BlipType.Shop,
                            Color = new Color(0.91f, 0.30f, 0.24f, 1f),
                            Label = "Medical Centre",
                            CustomScale = 1.10f,
                            IsPulsing = false,
                            IsCritical = false
                        });
                    }
                }
            }
            catch { }
        }
    }

    public void UpdateEntities(MinimapConfig config, float now, Vector3 playerWorldPos)
    {
        UpdateStaticLandmarks(config, now);

        if (now - _lastDynamicScanTime < DynamicScanInterval)
            return;

        _lastDynamicScanTime = now;
        _activeBlips.Clear();

        // Add static landmarks
        for (int s = 0; s < _staticLandmarks.Count; s++)
        {
            _activeBlips.Add(_staticLandmarks[s]);
        }

        float maxRangeSqr = config.MaxEntityRange * config.MaxEntityRange;

        // 3. Fast Scan Active NPCs via NPCManager.NPCRegistry (0-allocation List iteration)
        try
        {
            var registry = NPCManager.NPCRegistry;
            if (registry != null)
            {
                int npcCount = registry.Count;
                for (int i = 0; i < npcCount; i++)
                {
                    var npc = registry[i];
                    if (npc == null || (UnityEngine.Object)npc == null || !npc.gameObject.activeInHierarchy)
                        continue;

                    Vector3 pos = npc.transform.position;
                    float dx = pos.x - playerWorldPos.x;
                    float dz = pos.z - playerWorldPos.z;
                    float distSqr = dx * dx + dz * dz;

                    // Range Check (Cull distant NPCs immediately)
                    if (distSqr > maxRangeSqr)
                        continue;

                    // A: Police Officer check
                    var cop = npc.TryCast<PoliceOfficer>();
                    if (cop != null)
                    {
                        if (!config.ShowPoliceBlips) continue;

                        bool isAggro = cop.PursuitTarget != null && (UnityEngine.Object)cop.PursuitTarget != null;
                        Color copColor = isAggro ? new Color(0.95f, 0.20f, 0.20f, 1f) : new Color(0.20f, 0.55f, 0.95f, 1f);

                        _activeBlips.Add(new BlipInfo
                        {
                            WorldPos = pos,
                            Type = BlipType.Police,
                            Color = copColor,
                            Label = isAggro ? "Police (Pursuit!)" : "Police",
                            CustomScale = isAggro ? 1.25f : 1.05f,
                            IsPulsing = isAggro,
                            IsCritical = isAggro // Only clamp if in pursuit
                        });
                        continue;
                    }

                    // B: Customer check (use TryCast first to avoid GetComponent alloc where possible)
                    var cust = npc.TryCast<Customer>();
                    if (cust == null) cust = npc.GetComponent<Customer>();
                    if (cust != null)
                    {
                        bool isAwaiting = cust.IsAwaitingDelivery;
                        bool isAtLocation = cust.IsAtDealLocation();
                        bool awaitingSample = cust.awaitingSample;
                        bool recommended = cust.HasBeenRecommended;

                        // Active Deal (Critical!)
                        if (isAwaiting || isAtLocation)
                        {
                            if (config.ShowActiveDeals)
                            {
                                _activeBlips.Add(new BlipInfo
                                {
                                    WorldPos = pos,
                                    Type = BlipType.ActiveDeal,
                                    Color = new Color(0.98f, 0.77f, 0.06f, 1f), // Gold
                                    Label = "Deal Meeting",
                                    CustomScale = 1.30f,
                                    IsPulsing = true,
                                    IsCritical = true // Always clamp active deals
                                });
                            }
                        }
                        // Potential Customer for Free Samples
                        else if (awaitingSample || recommended)
                        {
                            if (config.ShowPotentialCustomers)
                            {
                                _activeBlips.Add(new BlipInfo
                                {
                                    WorldPos = pos,
                                    Type = BlipType.PotentialCustomer,
                                    Color = new Color(0.10f, 0.74f, 0.61f, 1f), // Turquoise
                                    Label = "Potential Customer",
                                    CustomScale = 1.05f,
                                    IsPulsing = false,
                                    IsCritical = false
                                });
                            }
                        }
                        // Known Customer
                        else if (config.ShowKnownCustomers)
                        {
                            _activeBlips.Add(new BlipInfo
                            {
                                WorldPos = pos,
                                Type = BlipType.KnownCustomer,
                                Color = new Color(0.18f, 0.80f, 0.44f, 1f), // Emerald Green
                                Label = "Customer",
                                CustomScale = 0.95f,
                                IsPulsing = false,
                                IsCritical = false
                            });
                        }
                    }
                }
            }
        }
        catch { }

        // 4. Vehicles (cached scan via VehicleManager.AllVehicles)
        if (config.ShowVehicleBlips)
        {
            try
            {
                var vm = VehicleManager.Instance;
                if (vm != null && (UnityEngine.Object)vm != null && vm.AllVehicles != null)
                {
                    int vCount = vm.AllVehicles.Count;
                    for (int v = 0; v < vCount; v++)
                    {
                        var vehicle = vm.AllVehicles[v];
                        if (vehicle == null || (UnityEngine.Object)vehicle == null || !vehicle.gameObject.activeInHierarchy)
                            continue;

                        Vector3 vPos = vehicle.transform.position;
                        float vdx = vPos.x - playerWorldPos.x;
                        float vdz = vPos.z - playerWorldPos.z;
                        float vDistSqr = vdx * vdx + vdz * vdz;
                        if (vDistSqr > maxRangeSqr)
                            continue;

                        _activeBlips.Add(new BlipInfo
                        {
                            WorldPos = vPos,
                            Type = BlipType.Vehicle,
                            Color = new Color(0.20f, 0.60f, 0.86f, 1f),
                            Label = "Vehicle",
                            CustomScale = 1.10f,
                            IsPulsing = false,
                            IsCritical = false
                        });
                    }
                }
            }
            catch { }
        }

        // 5. Active Quests (derived from Quest.Quests + QuestEntry.PoILocation)
        if (config.ShowQuestBlips)
        {
            try
            {
                var allQuests = Quest.Quests;
                if (allQuests != null)
                {
                    int qCount = allQuests.Count;
                    for (int q = 0; q < qCount; q++)
                    {
                        var quest = allQuests[q];
                        if (quest == null || (UnityEngine.Object)quest == null)
                            continue;

                        var entries = quest.Entries;
                        if (entries == null)
                            continue;

                        int eCount = entries.Count;
                        for (int e = 0; e < eCount; e++)
                        {
                            var entry = entries[e];
                            if (entry == null || (UnityEngine.Object)entry == null)
                                continue;

                            if (entry.State != EQuestState.Active)
                                continue;

                            var poi = entry.PoILocation;
                            if (poi == null || (UnityEngine.Object)poi == null)
                                continue;

                            Vector3 qPos = poi.position;
                            float qdx = qPos.x - playerWorldPos.x;
                            float qdz = qPos.z - playerWorldPos.z;
                            float qDistSqr = qdx * qdx + qdz * qdz;
                            if (qDistSqr > maxRangeSqr)
                                continue;

                            _activeBlips.Add(new BlipInfo
                            {
                                WorldPos = qPos,
                                Type = BlipType.Quest,
                                Color = new Color(0.90f, 0.30f, 0.24f, 1f), // Ruby red
                                Label = quest.Title ?? "Quest",
                                CustomScale = 1.20f,
                                IsPulsing = true,
                                IsCritical = true // Always clamp quest POIs
                            });
                        }
                    }
                }
            }
            catch { }
        }

        // 6. Sleeping Bags (cross-mod reflection — cached to avoid per-tick reflection + GC)
        try
        {
            var activeList = GetStreetObjectsCached();
            if (activeList != null)
            {
                int sc = activeList.Count;
                for (int si = 0; si < sc; si++)
                {
                    var go = activeList[si];
                    if (go == null || (UnityEngine.Object)go == null)
                        continue;
                    if (go.name != "SleepingBag_Root")
                        continue;

                    Vector3 bPos = go.transform.position;
                    float bdx = bPos.x - playerWorldPos.x;
                    float bdz = bPos.z - playerWorldPos.z;
                    float bDistSqr = bdx * bdx + bdz * bdz;
                    if (bDistSqr > maxRangeSqr)
                        continue;

                    _activeBlips.Add(new BlipInfo
                    {
                        WorldPos = bPos,
                        Type = BlipType.SleepingBag,
                        Color = new Color(0.15f, 0.68f, 0.38f, 1f),
                        Label = "Sleeping Bag",
                        CustomScale = 1.10f,
                        IsPulsing = false,
                        IsCritical = false
                    });
                }
            }
        }
        catch { }

        // Pool is fixed at 64: stable-partition critical blips (quests, deals,
        // aggro police, owned bases) to the front so overflow drops expendable
        // dots first instead of quest POIs collected last.
        StableCriticalFirst(_activeBlips);
    }

    private static void StableCriticalFirst(List<BlipInfo> blips)
    {
        int write = 0;
        for (int read = 0; read < blips.Count; read++)
        {
            if (!blips[read].IsCritical) continue;
            if (read != write)
            {
                var tmp = blips[write];
                blips[write] = blips[read];
                blips[read] = tmp;
            }
            write++;
        }
    }

    public void RenderBlips(Vector3 playerWorldPos, float playerYaw, Vector2 playerMapPos, MinimapConfig config, float mapRadius, float zoomFactor)
    {
        int blipCount = _activeBlips.Count;
        int poolCount = _pool.Count;

        float halfW = mapRadius - 8f;
        float halfH = mapRadius - 8f;
        float clampRadius = mapRadius - 8f;
        float clampRadiusSqr = clampRadius * clampRadius;

        float timeSin = Mathf.Sin(Time.time * 6f) * 0.15f;

        for (int i = 0; i < poolCount; i++)
        {
            var pooled = _pool[i];
            if (i >= blipCount)
            {
                if (pooled.Root.activeSelf)
                    pooled.Root.SetActive(false);
                continue;
            }

            var info = _activeBlips[i];
            Vector2 itemMapPos = Vector2.zero;

            // Map world pos to 2D Map coordinate
            if (MapPositionUtility.Instance != null && (UnityEngine.Object)MapPositionUtility.Instance != null)
            {
                itemMapPos = MapPositionUtility.Instance.GetMapPosition(info.WorldPos);
            }
            else
            {
                itemMapPos = new Vector2(info.WorldPos.x * 2.5f, info.WorldPos.z * 2.5f);
            }

            Vector2 offset = (itemMapPos - playerMapPos) * zoomFactor;

            // Rotate offset if FollowPlayer mode
            if (config.RotationMode == MinimapRotation.FollowPlayer)
            {
                float rad = playerYaw * Mathf.Deg2Rad;
                float cos = Mathf.Cos(rad);
                float sin = Mathf.Sin(rad);
                offset = new Vector2(offset.x * cos - offset.y * sin, offset.x * sin + offset.y * cos);
            }

            bool isOutOfBounds = false;
            Vector2 finalPos = offset;

            // Shape-dependent Clamping
            if (config.Shape == MinimapShape.Circle)
            {
                float distSqr = offset.sqrMagnitude;
                if (distSqr > clampRadiusSqr)
                {
                    isOutOfBounds = true;
                    if (config.EnableEdgeClamping && (info.IsCritical || !config.ClampOnlyCritical))
                    {
                        finalPos = offset.normalized * clampRadius;
                    }
                }
            }
            else // Square / RoundedSquare
            {
                if (Mathf.Abs(offset.x) > halfW || Mathf.Abs(offset.y) > halfH)
                {
                    isOutOfBounds = true;
                    if (config.EnableEdgeClamping && (info.IsCritical || !config.ClampOnlyCritical))
                    {
                        finalPos = new Vector2(
                            Mathf.Clamp(offset.x, -halfW, halfW),
                            Mathf.Clamp(offset.y, -halfH, halfH)
                        );
                    }
                }
            }

            // Cull non-critical blips if they are outside viewport
            if (isOutOfBounds && (!config.EnableEdgeClamping || (config.ClampOnlyCritical && !info.IsCritical)))
            {
                if (pooled.Root.activeSelf)
                    pooled.Root.SetActive(false);
                continue;
            }

            if (!pooled.Root.activeSelf)
                pooled.Root.SetActive(true);

            pooled.Rect.anchoredPosition = finalPos;

            // Select matching sprite
            Sprite sprite = info.Type switch
            {
                BlipType.Police => MinimapTextures.GetBlipPolice(),
                BlipType.ActiveDeal => MinimapTextures.GetBlipDeal(),
                BlipType.PotentialCustomer => MinimapTextures.GetBlipPotential(),
                BlipType.KnownCustomer => MinimapTextures.GetBlipDealer(),
                BlipType.Property => MinimapTextures.GetBlipProperty(),
                BlipType.SleepingBag => MinimapTextures.GetBlipSleepingBag(),
                BlipType.Shop => MinimapTextures.GetBlipShop(),
                BlipType.Quest => MinimapTextures.GetBlipQuest(),
                BlipType.Vehicle => MinimapTextures.GetBlipVehicle(),
                _ => MinimapTextures.GetBlipDot()
            };

            pooled.Icon.sprite = sprite;
            pooled.Icon.color = info.Color;

            float pulseMult = info.IsPulsing ? (1.0f + timeSin) : 1.0f;
            float baseSize = (info.Type == BlipType.GenericDot) ? 10f : 15f;
            float finalDimension = baseSize * info.CustomScale * pulseMult;

            pooled.Rect.sizeDelta = new Vector2(finalDimension, finalDimension);
            pooled.Group.alpha = isOutOfBounds ? 0.80f : 1.0f;
        }
    }

    // --- Cached reflection for HomelessMod interop (avoids per-tick Type.GetType + PropertyInfo alloc) ---
    private static Type? _cachedStreetType;
    private static PropertyInfo? _cachedActiveProp;
    private static bool _streetReflectionTried = false;

    private static System.Collections.Generic.IReadOnlyList<GameObject>? GetStreetObjectsCached()
    {
        if (!_streetReflectionTried)
        {
            _streetReflectionTried = true;
            try
            {
                _cachedStreetType = Type.GetType("HomelessMod.Building.StreetPropertyManager, HomelessMod");
                if (_cachedStreetType != null)
                    _cachedActiveProp = _cachedStreetType.GetProperty("ActiveStreetObjects", BindingFlags.Public | BindingFlags.Static);
            }
            catch { }
        }
        if (_cachedStreetType == null || _cachedActiveProp == null) return null;
        try
        {
            return _cachedActiveProp.GetValue(null) as System.Collections.Generic.IReadOnlyList<GameObject>;
        }
        catch { return null; }
    }
}
