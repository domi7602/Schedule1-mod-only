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
    AcceptedDealClock,
    PotentialCustomer,
    KnownCustomer,
    PurchaseCheck,
    RegionLock,
    Dealer,
    Waypoint,
    Property,
    SleepingBag,
    Shop,
    Quest,
    Vehicle,
    GenericDot,
    SewerKey
}

public sealed class MinimapBlips
{
    public struct BlipInfo
    {
        public Vector3 WorldPos;
        public BlipType Type;
        public Color Color;
        public string Label;
        public bool ShowLabel;
        public string BadgeGlyph;
        public Sprite? Portrait;
        public float CustomScale;
        public bool IsPulsing;
        public bool IsCritical; // Whether it should be edge-clamped when out of view
    }

    private sealed class PooledBlip
    {
        public GameObject Root;
        public RectTransform Rect;
        public Image Icon;
        public Text NameLabel;
        public Text BadgeGlyph;
        public Image PortraitImage;
        public CanvasGroup Group;
        public readonly RectTransform NameRect;
        public readonly RectTransform BadgeRect;
        public readonly RectTransform PortraitRect;
        public readonly GameObject NameObject;
        public readonly GameObject BadgeObject;
        public readonly GameObject PortraitObject;
        // IL2CPP Color/Vector2 proxies lack managed IEquatable<T>; scalar tuple
        // equality avoids boxing/native Equals calls in the per-frame policy.
        public AppliedValue<BlipType> IconType;
        public AppliedValue<(float, float, float, float)> IconColor;
        public AppliedValue<(float, float)> Position;
        public AppliedValue<float> Dimension;
        public AppliedValue<bool> IconEnabled;
        public AppliedValue<bool> PortraitActive;
        public AppliedValue<IntPtr> PortraitSprite;
        public AppliedValue<float> PortraitDimension;
        public AppliedValue<bool> BadgeActive;
        public AppliedValue<string> BadgeText;
        public AppliedValue<(float, float)> BadgeSize;
        public AppliedValue<(float, float)> BadgePosition;
        public AppliedValue<bool> BadgeOverPortrait;
        public AppliedValue<bool> NameActive;
        public AppliedValue<string> NameText;
        public AppliedValue<(float, float, float, float)> NameColor;
        public AppliedValue<(float, float)> NamePosition;
        public AppliedValue<float> Alpha;

        public PooledBlip(GameObject root, RectTransform rect, Image icon, Text nameLabel, Text badgeGlyph, Image portraitImage, CanvasGroup group)
        {
            Root = root;
            Rect = rect;
            Icon = icon;
            NameLabel = nameLabel;
            BadgeGlyph = badgeGlyph;
            PortraitImage = portraitImage;
            Group = group;
            // Resolve wrapper-safe components once, never through a Transform cast.
            NameRect = nameLabel.GetComponent<RectTransform>();
            BadgeRect = badgeGlyph.GetComponent<RectTransform>();
            PortraitRect = portraitImage.transform.parent.GetComponent<RectTransform>();
            NameObject = nameLabel.gameObject;
            BadgeObject = badgeGlyph.gameObject;
            PortraitObject = portraitImage.gameObject;
        }
    }

    private const int PoolSize = 64;
    private readonly List<PooledBlip> _pool = new(PoolSize);
    private readonly List<BlipInfo> _activeBlips = new(PoolSize);
    private Transform? _blipContainer;
    private long _snapshotGeneration;
    private readonly Vector2[] _mapPositions = new Vector2[PoolSize];
    // Scalar calibration key: do not retain gameplay wrappers across scene changes.
    private readonly AppliedValue<(long Snapshot, int Converter,
        (float, float, float) Origin, (float, float, float) Edge,
        float Dimensions, float Factor)>[] _projectionKeys = new AppliedValue<(long, int,
        (float, float, float), (float, float, float), float, float)>[PoolSize];

    // ── M6 palette: maps category key → hex; consulted at blip build time ──
    private static System.Collections.Generic.Dictionary<string, string>? _palette;

    /// <summary>M6: install/refresh the user palette (from MinimapSettingsApp).</summary>
    public void SetPalette(System.Collections.Generic.Dictionary<string, string>? palette)
    {
        _palette = palette;
    }

    /// <summary>
    /// Hides every pooled blip. Called while the player is not yet resolved
    /// (spawn/loading) so no stale-position dot flashes at the map centre.
    /// </summary>
    public void HideAll()
    {
        for (int i = 0; i < _pool.Count; i++)
        {
            var pooled = _pool[i];
            if (pooled.Root != null && pooled.Root.activeSelf)
                pooled.Root.SetActive(false);
        }
    }

    /// <summary>
    /// Adds the un-collected world sewer keys as pulsing gold key blips.
    /// <para>
    /// The keys are tiny world pickups — nearly impossible to find by eye — so this is
    /// the whole point of the feature: a pulsing, distance-independent landmark for
    /// each one still out there. Vanilla spawns the key at the position chosen by
    /// <c>SewerManager.RandomSewerKeyLocationIndex</c>; the actual live pickup object
    /// (RandomWorldSewerKeyPickup) is preferred when present, because that is the
    /// object that disappears when the key is taken.
    /// </para>
    /// </summary>
    private void UpdateSewerKeys(MinimapConfig config)
    {
        if (!config.ShowSewerKeyBlips) return;

        SewerManager? manager;
        try { manager = SewerManager.Instance; }
        catch { return; }
        if (manager == null || manager.Pointer == IntPtr.Zero || manager.WasCollected) return;

        // 1. Live pickup object — the authoritative position while it is still out there.
        try
        {
            if (manager.IsRandomWorldKeyCollected) return;
            var pickup = manager.RandomWorldSewerKeyPickup;
            if (pickup != null && pickup.Pointer != IntPtr.Zero && !pickup.WasCollected)
            {
                Vector3 keyPos = pickup.transform.position;
                _activeBlips.Add(new BlipInfo
                {
                    WorldPos = keyPos,
                    Type = BlipType.SewerKey,
                    Color = PaletteColor("SewerKey", new Color(0.99f, 0.85f, 0.20f, 1f)), // Gold
                    Label = "Sewer Key",
                    CustomScale = 1.45f,
                    IsPulsing = true,
                    IsCritical = true // Edge-clamp: it must stay visible even from across the map
                });
            }
        }
        catch { }
    }

    private static Color PaletteColor(string key, Color fallback)
    {
        if (_palette != null && _palette.TryGetValue(key, out var hex)
            && ColorUtility.TryParseHtmlString(hex, out var c))
            return c;
        return fallback;
    }

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
        Array.Clear(_projectionKeys, 0, _projectionKeys.Length);

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

            Font? font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            Text nameLabel = CreatePoolText(obj.transform, "CustomerName", new Vector2(108f, 18f), new Vector2(0f, -13f), font, 9, FontStyle.Normal);
            Text badgeGlyph = CreatePoolText(obj.transform, "BadgeGlyph", new Vector2(20f, 20f), Vector2.zero, font, 13, FontStyle.Bold);
            Image portrait = CreatePoolPortrait(obj.transform, 26f);
            nameLabel.gameObject.SetActive(false);
            badgeGlyph.gameObject.SetActive(false);
            portrait.gameObject.SetActive(false);

            obj.SetActive(false);
            _pool.Add(new PooledBlip(obj, rt, img, nameLabel, badgeGlyph, portrait, group));
        }
    }

    private static Text CreatePoolText(Transform parent, string name, Vector2 size, Vector2 position, Font? font, int fontSize, FontStyle style)
    {
        GameObject obj = new GameObject(name, Il2CppType.Of<RectTransform>());
        obj.transform.SetParent(parent, false);
        RectTransform rect = obj.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.pivot = new Vector2(0.5f, 0.5f);
        rect.anchoredPosition = position;
        rect.sizeDelta = size;
        Text text = obj.AddComponent<Text>();
        if (font != null) text.font = font;
        text.fontSize = fontSize;
        text.fontStyle = style;
        text.alignment = TextAnchor.MiddleCenter;
        text.color = Color.white;
        text.supportRichText = false;
        text.horizontalOverflow = HorizontalWrapMode.Overflow;
        text.verticalOverflow = VerticalWrapMode.Overflow;
        text.raycastTarget = false;
        return text;
    }

    private static string ResolveCustomerName(Customer cust)
    {
        try
        {
            var npc = cust.NPC;
            if (npc != null && (UnityEngine.Object)npc != null)
            {
                string full = npc.FullName;
                if (!string.IsNullOrWhiteSpace(full)) return full;
            }
        }
        catch { }
        return "Customer";
    }

    /// <summary>Mugshot sprite for the customer's NPC, or null when unavailable (early save load).</summary>
    private static Sprite? ResolveCustomerPortrait(Customer cust)
    {
        try
        {
            var npc = cust.NPC;
            if (npc != null && (UnityEngine.Object)npc != null)
            {
                var mug = npc.MugshotSprite;
                if (mug != null && mug.Pointer != IntPtr.Zero && !mug.WasCollected)
                    return mug;
            }
        }
        catch { }
        return null;
    }

    /// <summary>
    /// Builds a round NPC portrait badge: a circular mask root whose child Image carries the
    /// mugshot sprite. The portrait is centered on the marker position and REPLACES the
    /// normal icon when shown (single visual per customer). Returns the inner Image
    /// (the part that is shown/hidden per frame); the mask root stays active but draws
    /// nothing on its own (showMaskGraphic = false).
    /// </summary>
    private static Image CreatePoolPortrait(Transform parent, float diameter)
    {
        GameObject rootObj = new GameObject("Portrait", Il2CppType.Of<RectTransform>());
        rootObj.transform.SetParent(parent, false);
        RectTransform rootRt = rootObj.GetComponent<RectTransform>();
        rootRt.anchorMin = new Vector2(0.5f, 0.5f);
        rootRt.anchorMax = new Vector2(0.5f, 0.5f);
        rootRt.pivot = new Vector2(0.5f, 0.5f);
        rootRt.anchoredPosition = Vector2.zero;
        rootRt.sizeDelta = new Vector2(diameter, diameter);

        Image maskImg = rootObj.AddComponent<Image>();
        maskImg.sprite = MinimapTextures.GetCircleMask();
        maskImg.color = Color.white;
        maskImg.raycastTarget = false;
        Mask mask = rootObj.AddComponent<Mask>();
        mask.showMaskGraphic = false;

        GameObject imgObj = new GameObject("Mugshot", Il2CppType.Of<RectTransform>());
        imgObj.transform.SetParent(rootObj.transform, false);
        RectTransform imgRt = imgObj.GetComponent<RectTransform>();
        imgRt.anchorMin = Vector2.zero;
        imgRt.anchorMax = Vector2.one;
        imgRt.offsetMin = Vector2.zero;
        imgRt.offsetMax = Vector2.zero;
        Image portrait = imgObj.AddComponent<Image>();
        portrait.raycastTarget = false;
        portrait.preserveAspect = true;
        portrait.color = Color.white;
        return portrait;
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
                                Color = PaletteColor("Property", new Color(0.95f, 0.61f, 0.07f, 1f)), // Amber
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

        // 1b. Green purchase checks — unowned but purchasable properties
        if (config.ShowPurchaseChecks)
        {
            try
            {
                var unowned = Property.UnownedProperties;
                if (unowned != null)
                {
                    int count = unowned.Count;
                    for (int i = 0; i < count; i++)
                    {
                        var prop = unowned[i];
                        if (prop == null || (UnityEngine.Object)prop == null)
                            continue;

                        bool canBuy;
                        try { canBuy = prop.CanBePurchased(); }
                        catch { canBuy = false; }
                        bool isOwned;
                        try { isOwned = prop.IsOwned; }
                        catch { isOwned = false; }

                        if (!MinimapFeatureRules.ShouldShowPurchaseCheck(canBuy, isOwned))
                            continue;

                        _staticLandmarks.Add(new BlipInfo
                        {
                            WorldPos = prop.transform.position,
                            Type = BlipType.PurchaseCheck,
                            Color = PaletteColor("PurchaseCheck", new Color(0.18f, 0.80f, 0.44f, 1f)), // Green check
                            Label = prop.PropertyName ?? "Property",
                            BadgeGlyph = "\u2713", // check mark
                            CustomScale = 1.15f,
                            IsPulsing = false,
                            IsCritical = true
                        });
                    }
                }
            }
            catch { }
        }

        // 1c. Gray regional locks — regions the player has not unlocked yet
        if (config.ShowRegionalLocks)
        {
            try
            {
                if (Map.Instance != null && (UnityEngine.Object)Map.Instance != null)
                {
                    var regions = Map.Instance.Regions;
                    if (regions != null)
                    {
                        int rCount = regions.Length;
                        for (int i = 0; i < rCount; i++)
                        {
                            var region = regions[i];
                            if (region == null)
                                continue;
                            if (!MinimapFeatureRules.ShouldShowRegionLock(region.IsUnlocked))
                                continue;

                            var bounds = region.RegionBounds;
                            if (bounds == null || (UnityEngine.Object)bounds == null)
                                continue;

                            _staticLandmarks.Add(new BlipInfo
                            {
                                WorldPos = bounds.transform.position,
                                Type = BlipType.RegionLock,
                                Color = PaletteColor("RegionLock", new Color(0.60f, 0.63f, 0.65f, 1f)), // Gray
                                Label = region.Name ?? "Locked Region",
                                BadgeGlyph = "\uD83D\uDD12", // lock
                                CustomScale = 1.10f,
                                IsPulsing = false,
                                IsCritical = false
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
                            Color = PaletteColor("Shop", new Color(0.91f, 0.30f, 0.24f, 1f)),
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

    public void UpdateEntities(MinimapConfig config, float now, Vector3 playerWorldPos, MinimapWaypoints? waypoints = null)
    {
        UpdateStaticLandmarks(config, now);

        if (now - _lastDynamicScanTime < DynamicScanInterval)
            return;

        _lastDynamicScanTime = now;
        _activeBlips.Clear();
        _snapshotGeneration++;

        // Add static landmarks
        for (int s = 0; s < _staticLandmarks.Count; s++)
        {
            _activeBlips.Add(_staticLandmarks[s]);
        }

        // World sewer keys (tiny pickups — pulsing gold key blips, edge-clamped)
        UpdateSewerKeys(config);

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
                        Color copColor = isAggro ? new Color(0.95f, 0.20f, 0.20f, 1f) : PaletteColor("Police", new Color(0.20f, 0.55f, 0.95f, 1f));

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
                        string custName = ResolveCustomerName(cust);
                        var marker = MinimapFeatureRules.ResolveCustomerMarker(
                            isAwaiting, isAtLocation, awaitingSample || recommended, config.ShowKnownCustomers);

                        // Simplified customer look: portrait when available, else normal symbol.
                        // No extra name rendering — one visual per customer.
                        Sprite? custPortrait = ResolveCustomerPortrait(cust);

                        if (marker == MinimapMarkerKind.AcceptedDealClock)
                        {
                            if (config.ShowActiveDeals)
                            {
                                _activeBlips.Add(new BlipInfo
                                {
                                    WorldPos = pos,
                                    Type = BlipType.AcceptedDealClock,
                                    Color = PaletteColor("DealClock", new Color(0.30f, 0.64f, 1f, 1f)), // Blue clock
                                    Label = custName,
                                    ShowLabel = false,
                                    Portrait = custPortrait,
                                    BadgeGlyph = "\u23F1", // stopwatch
                                    CustomScale = 1.30f,
                                    IsPulsing = true,
                                    IsCritical = true // Always clamp active deals
                                });
                            }
                        }
                        // Potential Customer for Free Samples — purple highlight
                        else if (marker == MinimapMarkerKind.PotentialCustomer)
                        {
                            if (config.ShowPotentialCustomers)
                            {
                                _activeBlips.Add(new BlipInfo
                                {
                                    WorldPos = pos,
                                    Type = BlipType.PotentialCustomer,
                                    Color = PaletteColor("Potential", new Color(0.64f, 0.30f, 0.89f, 1f)), // Purple highlight
                                    Label = custName,
                                    ShowLabel = false,
                                    Portrait = custPortrait,
                                    CustomScale = 1.05f,
                                    IsPulsing = false,
                                    IsCritical = false
                                });
                            }
                        }
                        // Known Customer
                        else if (marker == MinimapMarkerKind.KnownCustomer)
                        {
                            _activeBlips.Add(new BlipInfo
                            {
                                WorldPos = pos,
                                Type = BlipType.KnownCustomer,
                                Color = PaletteColor("Customer", new Color(0.18f, 0.80f, 0.44f, 1f)), // Emerald Green
                                Label = custName,
                                ShowLabel = false,
                                Portrait = custPortrait,
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

        // 3b. Dealer scan (M1) — recruited dealers are ALWAYS shown, clamped to the
        // minimap edge regardless of MaxEntityRange. Source: DealerManagementApp.dealers
        // (the same list the in-game Dealer Management app iterates).
        if (config.ShowDealerBlips)
        {
            try
            {
                var dealerApp = Il2CppScheduleOne.UI.Phone.Messages.DealerManagementApp.Instance;
                if (dealerApp != null && (UnityEngine.Object)dealerApp != null && dealerApp.dealers != null)
                {
                    int dCount = dealerApp.dealers.Count;
                    for (int d = 0; d < dCount; d++)
                    {
                        var dealer = dealerApp.dealers[d];
                        if (dealer == null || (UnityEngine.Object)dealer == null || !dealer.gameObject.activeInHierarchy)
                            continue;
                        if (!dealer.IsRecruited)
                            continue;

                        Vector3 dPos = dealer.transform.position;

                        _activeBlips.Add(new BlipInfo
                        {
                            WorldPos = dPos,
                            Type = BlipType.Dealer,
                            Color = PaletteColor("Dealer", new Color(0.64f, 0.30f, 0.89f, 1f)), // Purple
                            Label = dealer.FullName ?? "Dealer",
                            CustomScale = 1.20f,
                            IsPulsing = false,
                            IsCritical = true // always clamp — dealer positions matter at any range
                        });
                    }
                }
            }
            catch { }
        }

        // 3c. User waypoints (M4) — always clamped, never range-culled, drawn as
        // diamonds. Rendered AFTER all scans so they sit at the tail of the list;
        // StableCriticalFirst keeps them in front (IsCritical = true).
        if (waypoints != null && config.ShowWaypoints)
        {
            var all = waypoints.All;
            for (int w = 0; w < all.Count; w++)
            {
                var wp = all[w];
                if (wp == null) continue;

                ColorUtility.TryParseHtmlString(wp.ColorHex, out Color wc);
                _activeBlips.Add(new BlipInfo
                {
                    WorldPos = wp.ToVector(),
                    Type = BlipType.Waypoint,
                    Color = wc,
                    Label = wp.Name,
                    CustomScale = 1.25f,
                    IsPulsing = true,
                    IsCritical = true // always clamp — waypoints exist for far navigation
                });
            }
        }

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
                            Color = PaletteColor("Vehicle", new Color(0.20f, 0.60f, 0.86f, 1f)),
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
                                Color = PaletteColor("Quest", new Color(0.90f, 0.30f, 0.24f, 1f)), // Ruby red
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
        bool followPlayer = config.RotationMode == MinimapRotation.FollowPlayer;
        float rad = playerYaw * Mathf.Deg2Rad;
        float cos = followPlayer ? Mathf.Cos(rad) : 1f;
        float sin = followPlayer ? Mathf.Sin(rad) : 0f;

        var converter = MapPositionUtility.Instance;
        if (converter == null || converter.Pointer == IntPtr.Zero || converter.WasCollected
            || (UnityEngine.Object)converter == null)
            converter = null;
        int converterId = converter != null ? converter.GetInstanceID() : 0;
        Vector3 origin = Vector3.zero;
        Vector3 edge = Vector3.zero;
        float dimensions = 0f;
        float factor = 0f;
        if (converter != null)
        {
            // Recalculate/calibration can change on the same utility instance.
            // Observe once per frame, rather than pinning stale live converter state.
            var originPoint = converter.OriginPoint;
            var edgePoint = converter.EdgePoint;
            if (originPoint != null) origin = originPoint.position;
            if (edgePoint != null) edge = edgePoint.position;
            dimensions = converter.MapDimensions;
            factor = converter.conversionFactor;
        }
        var projectionKey = (_snapshotGeneration, converterId,
            (origin.x, origin.y, origin.z), (edge.x, edge.y, edge.z), dimensions, factor);

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
            // The world position is already a 1s snapshot. Project each slot once
            // per rebuild/calibration change, including fallback -> real converter.
            if (_projectionKeys[i].NeedsUpdate(projectionKey))
            {
                _mapPositions[i] = converter != null
                    ? converter.GetMapPosition(info.WorldPos)
                    : new Vector2(info.WorldPos.x * 2.5f, info.WorldPos.z * 2.5f);
                _projectionKeys[i].Commit(projectionKey);
            }
            Vector2 itemMapPos = _mapPositions[i];

            Vector2 offset = (itemMapPos - playerMapPos) * zoomFactor;

            // Rotate offset if FollowPlayer mode
            if (followPlayer)
            {
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

            // Caches belong to UI slots, not entities: reuse/cull/hide/reopen compares
            // against what was actually applied. Never commit before a native setter.
            var positionKey = (finalPos.x, finalPos.y);
            if (pooled.Position.NeedsUpdate(positionKey))
            {
                pooled.Rect.anchoredPosition = finalPos;
                pooled.Position.Commit(positionKey);
            }

            if (pooled.IconType.NeedsUpdate(info.Type))
            {
                Sprite sprite = info.Type switch
                {
                    BlipType.Police => MinimapTextures.GetBlipPolice(),
                    BlipType.ActiveDeal => MinimapTextures.GetBlipDeal(),
                    BlipType.AcceptedDealClock => MinimapTextures.GetBlipDeal(),
                    BlipType.PotentialCustomer => MinimapTextures.GetBlipPotential(),
                    BlipType.KnownCustomer => MinimapTextures.GetBlipDealer(),
                    BlipType.PurchaseCheck => MinimapTextures.GetBlipProperty(),
                    BlipType.RegionLock => MinimapTextures.GetBlipDot(),
                    BlipType.Dealer => MinimapTextures.GetBlipDealer(),
                    BlipType.Waypoint => MinimapTextures.GetBlipQuest(),
                    BlipType.Property => MinimapTextures.GetBlipProperty(),
                    BlipType.SleepingBag => MinimapTextures.GetBlipSleepingBag(),
                    BlipType.Shop => MinimapTextures.GetBlipShop(),
                    BlipType.Quest => MinimapTextures.GetBlipQuest(),
                    BlipType.Vehicle => MinimapTextures.GetBlipVehicle(),
                    BlipType.SewerKey => MinimapTextures.GetBlipSewerKey(),
                    _ => MinimapTextures.GetBlipDot()
                };
                pooled.Icon.sprite = sprite;
                pooled.IconType.Commit(info.Type);
            }
            var colorKey = (info.Color.r, info.Color.g, info.Color.b, info.Color.a);
            if (pooled.IconColor.NeedsUpdate(colorKey))
            {
                pooled.Icon.color = info.Color;
                pooled.IconColor.Commit(colorKey);
            }

            float pulseMult = info.IsPulsing ? (1.0f + timeSin) : 1.0f;
            float baseSize = (info.Type == BlipType.GenericDot) ? 10f : 15f;
            float finalDimension = baseSize * info.CustomScale * pulseMult;
            if (pooled.Dimension.NeedsUpdate(finalDimension))
            {
                pooled.Rect.sizeDelta = new Vector2(finalDimension, finalDimension);
                pooled.Dimension.Commit(finalDimension);
            }

            // Portrait replaces the normal icon (one visual per customer).
            bool hasPortrait = info.Portrait != null;
            if (pooled.IconEnabled.NeedsUpdate(!hasPortrait))
            {
                pooled.Icon.enabled = !hasPortrait;
                pooled.IconEnabled.Commit(!hasPortrait);
            }
            if (pooled.PortraitActive.NeedsUpdate(hasPortrait))
            {
                pooled.PortraitObject.SetActive(hasPortrait);
                pooled.PortraitActive.Commit(hasPortrait);
            }
            if (hasPortrait)
            {
                IntPtr portraitPointer = info.Portrait!.Pointer;
                if (pooled.PortraitSprite.NeedsUpdate(portraitPointer))
                {
                    pooled.PortraitImage.sprite = info.Portrait;
                    pooled.PortraitSprite.Commit(portraitPointer);
                }
                if (pooled.PortraitDimension.NeedsUpdate(finalDimension))
                {
                    pooled.PortraitRect.sizeDelta = new Vector2(finalDimension, finalDimension);
                    pooled.PortraitDimension.Commit(finalDimension);
                }
            }

            // Optional glyph badge (clock / check / lock), above the portrait.
            bool hasGlyph = !string.IsNullOrEmpty(info.BadgeGlyph);
            if (pooled.BadgeActive.NeedsUpdate(hasGlyph))
            {
                pooled.BadgeObject.SetActive(hasGlyph);
                pooled.BadgeActive.Commit(hasGlyph);
            }
            bool badgeOverPortrait = hasGlyph && hasPortrait;
            if (pooled.BadgeOverPortrait.NeedsUpdate(badgeOverPortrait))
            {
                if (badgeOverPortrait) pooled.BadgeRect.SetAsLastSibling();
                pooled.BadgeOverPortrait.Commit(badgeOverPortrait);
            }
            if (hasGlyph)
            {
                if (pooled.BadgeText.NeedsUpdate(info.BadgeGlyph))
                {
                    pooled.BadgeGlyph.text = info.BadgeGlyph;
                    pooled.BadgeText.Commit(info.BadgeGlyph);
                }
                Vector2 badgeSize = hasPortrait
                    ? new Vector2(finalDimension * 0.65f, finalDimension * 0.65f)
                    : new Vector2(finalDimension, finalDimension);
                Vector2 badgePosition = hasPortrait
                    ? new Vector2(finalDimension * 0.35f, -finalDimension * 0.35f)
                    : Vector2.zero;
                var badgeSizeKey = (badgeSize.x, badgeSize.y);
                var badgePositionKey = (badgePosition.x, badgePosition.y);
                if (pooled.BadgeSize.NeedsUpdate(badgeSizeKey))
                {
                    pooled.BadgeRect.sizeDelta = badgeSize;
                    pooled.BadgeSize.Commit(badgeSizeKey);
                }
                if (pooled.BadgePosition.NeedsUpdate(badgePositionKey))
                {
                    pooled.BadgeRect.anchoredPosition = badgePosition;
                    pooled.BadgePosition.Commit(badgePositionKey);
                }
            }

            // Optional name label under the marker (hidden while a portrait is shown).
            bool hasLabel = !hasPortrait && info.ShowLabel && !string.IsNullOrWhiteSpace(info.Label);
            if (pooled.NameActive.NeedsUpdate(hasLabel))
            {
                pooled.NameObject.SetActive(hasLabel);
                pooled.NameActive.Commit(hasLabel);
            }
            if (hasLabel)
            {
                if (pooled.NameText.NeedsUpdate(info.Label))
                {
                    pooled.NameLabel.text = info.Label;
                    pooled.NameText.Commit(info.Label);
                }
                if (pooled.NameColor.NeedsUpdate(colorKey))
                {
                    pooled.NameLabel.color = info.Color;
                    pooled.NameColor.Commit(colorKey);
                }
                Vector2 labelPosition = new Vector2(0f, -(finalDimension * 0.5f + 9f));
                var labelPositionKey = (labelPosition.x, labelPosition.y);
                if (pooled.NamePosition.NeedsUpdate(labelPositionKey))
                {
                    pooled.NameRect.anchoredPosition = labelPosition;
                    pooled.NamePosition.Commit(labelPositionKey);
                }
            }

            float alpha = isOutOfBounds ? 0.80f : 1.0f;
            if (pooled.Alpha.NeedsUpdate(alpha))
            {
                pooled.Group.alpha = alpha;
                pooled.Alpha.Commit(alpha);
            }
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
