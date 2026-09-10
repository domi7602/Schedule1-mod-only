using System;
using System.Collections.Generic;
using System.Linq;
using MelonLoader;
using PotScanner.Services;
using PotScanner.Utils;
using S1API.PhoneApp;
using S1API.UI;
using S1API.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace PotScanner;

/// <summary>
/// Thin wrapper delegating to Shared UITheme (Single Source of Truth) — Dashboard-Mode.
/// </summary>
public static class UITheme
{
    public const float RefHeight = S1Mods.Shared.UITheme.RefHeight;
    public const float RefWidth = S1Mods.Shared.UITheme.RefWidth;
    public static float ActualWidth => S1Mods.Shared.UITheme.ActualWidth;
    public static float ActualHeight => S1Mods.Shared.UITheme.ActualHeight;
    public static float Scale => S1Mods.Shared.UITheme.Scale;
    public static void Initialize(RectTransform containerRt) => S1Mods.Shared.UITheme.InitializeForDashboard(containerRt);
    public static int Sp(float pt) => S1Mods.Shared.UITheme.Sp(pt);
    public static float Dp(float px) => S1Mods.Shared.UITheme.Dp(px);
}

/// <summary>
/// Filter categories for the quick filter tabs toolbar.
/// </summary>
public enum PotFilter
{
    All,
    Thirsty,
    Ready,
    Empty
}

/// <summary>
/// Phone app that lists all placed pots, grouped by property, with water/soil/growth/quality.
/// Refreshed by Mod.OnUpdate every 2s when the app is open (cheap when IsOpen() is false).
/// </summary>
public sealed class PotScannerApp : PhoneApp
{
    private GameObject _mainBG = null!;
    private RectTransform _listContent = null!;
    private Button _waterAllButton = null!;
    private Text _waterAllLabel = null!;
    private Text _waterAllSubLabel = null!;
    private Button _autoWaterButton = null!;
    private Text _autoWaterLabel = null!;
    private Text _autoWaterSubLabel = null!;
    private float _lastRefreshRealtime;
    private string? _activePropertyKey;
    private PotFilter _activeFilter = PotFilter.All;
    private readonly Dictionary<PotFilter, (Image bg, Text label, Button btn)> _filterButtons = new();

    private sealed class PotRowEntry
    {
        public PotInfo PotInfo = null!;
        public GameObject RowObject = null!;
    }

    private sealed class PotRowUIRef
    {
        public PotInfo PotInfo = null!;
        public GameObject RowObject = null!;
        public Text TitleText = null!;
        public RectTransform WaterFillRt = null!;
        public Text? WaterBadgeText;
        public RectTransform GrowthFillRt = null!;
        public Image? GrowthFillImage;
        public Text? GrowthBadgeText;
        public RectTransform SoilFillRt = null!;
        public Text? SoilBadgeText;
    }

    private sealed class PropertyGroupUIRef
    {
        public string PropertyKey = "";
        public GameObject HeaderObject = null!;
        public Image HeaderImage = null!;
        public Text TitleText = null!;
        public Text BadgeText = null!;
        public Button HeaderButton = null!;
        public readonly List<PotRowEntry> Rows = new();
    }

    private readonly Dictionary<IntPtr, PotRowUIRef> _rowCache = new();
    private readonly Dictionary<string, PropertyGroupUIRef> _propertyGroups = new();
    private bool _initialScanComplete;

    protected override string AppName => "PotScanner";
    protected override string AppTitle => "PotScanner";
    protected override string IconLabel => "Pots";
    protected override string IconFileName => string.Empty;
    protected override EOrientation Orientation => EOrientation.Horizontal;

    private Sprite? _cachedIconSprite;

    protected override Sprite IconSprite
    {
        get
        {
            if (_cachedIconSprite != null) return _cachedIconSprite;

            string path = System.IO.Path.Combine(MelonLoader.Utils.MelonEnvironment.ModsDirectory, "PotScannerIcon.png");
            if (System.IO.File.Exists(path))
            {
                try
                {
                    byte[] data = System.IO.File.ReadAllBytes(path);
                    var tex = new Texture2D(256, 256, TextureFormat.RGBA32, false);
                    if (ImageConversion.LoadImage(tex, data))
                    {
                        _cachedIconSprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
                        _cachedIconSprite.name = "PotScannerIcon";
                        return _cachedIconSprite;
                    }
                }
                catch (Exception ex)
                {
                    MelonLogger.Warning($" Failed to load PotScannerIcon.png: {ex.Message}");
                }
            }

            const int size = 64;
            var fallbackTex = new Texture2D(size, size, TextureFormat.RGBA32, false);
            _cachedIconSprite = Sprite.Create(fallbackTex, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f));
            _cachedIconSprite.name = "PotScannerIcon";
            return _cachedIconSprite;
        }
    }

    // Fix (Bug-Audit 2026-09-10): the phone re-instantiates this app per scene load and the old
    // per-instance handlers stayed in the static invocation lists forever (MelonEvents.OnUpdate,
    // PotTracker.OnPotsScanned). Both now dispatch through _active, subscribed exactly once;
    // Mod.OnSceneWasUnloaded clears _active when the gameplay scene tears down.
    private static PotScannerApp? _active;
    private static bool _staticSubscribed;

    protected override void OnCreated()
    {
        base.OnCreated();
        _active = this;
        if (!_staticSubscribed)
        {
            _staticSubscribed = true;
            MelonEvents.OnUpdate.Subscribe(DispatchUpdate);
            PotTracker.Instance.OnPotsScanned += DispatchPotsScanned;
        }
        MelonLogger.Msg("PotScanner app created and registered.");
        PotTracker.Instance.RefreshNow();
    }

    internal static void TearDownForSceneUnload() => _active = null;

    private static void DispatchUpdate() => _active?.Update();

    private static void DispatchPotsScanned() => _active?.OnPotsScannedHandler();

    protected override void OnCreatedUI(GameObject container)
    {
        var containerRt = container.GetComponent<RectTransform>();
        if (containerRt != null)
        {
            UITheme.Initialize(containerRt);
            MelonLogger.Msg($"Responsive canvas initialized: {UITheme.ActualWidth:F0}x{UITheme.ActualHeight:F0} (Scale={UITheme.Scale:F2})");
        }

        _mainBG = UIFactory.Panel("MainBG", container.transform, new Color(0.08f, 0.09f, 0.12f, 1f), fullAnchor: true);
        _mainBG.SetActive(false);

        var vlg = _mainBG.AddComponent<VerticalLayoutGroup>();
        vlg.childControlHeight = true;
        vlg.childControlWidth = true;
        vlg.childForceExpandHeight = false;
        vlg.childForceExpandWidth = true;
        vlg.spacing = UITheme.Dp(4f);
        vlg.padding = new RectOffset((int)UITheme.Dp(6f), (int)UITheme.Dp(6f), (int)UITheme.Dp(6f), (int)UITheme.Dp(6f));

        // --- ActionRow (2 Buttons) - Kompakte Höhe 36px ---
        var actionPanel = UIFactory.Panel("ActionRow", _mainBG.transform, Color.clear);
        var actionLE = actionPanel.AddComponent<LayoutElement>();
        actionLE.minHeight = UITheme.Dp(36f);
        actionLE.preferredHeight = UITheme.Dp(36f);
        actionLE.flexibleHeight = 0f;

        var actionHlg = actionPanel.AddComponent<HorizontalLayoutGroup>();
        actionHlg.spacing = UITheme.Dp(6f);
        actionHlg.childControlWidth = true;
        actionHlg.childControlHeight = true;
        actionHlg.childForceExpandWidth = true;
        actionHlg.childForceExpandHeight = true;

        // --- Water All button ---
        var waterPanel = UIFactory.Panel("WaterAllPanel", actionPanel.transform, new Color(0.16f, 0.36f, 0.60f));
        _waterAllButton = waterPanel.AddComponent<Button>();
        _waterAllButton.transition = Selectable.Transition.None;

        var wVlg = waterPanel.AddComponent<VerticalLayoutGroup>();
        wVlg.childControlHeight = true; wVlg.childControlWidth = true;
        wVlg.childForceExpandHeight = false; wVlg.childForceExpandWidth = true;
        wVlg.childAlignment = TextAnchor.MiddleCenter;

        _waterAllLabel = UIFactory.Text("WaterAllLbl", "Water All", waterPanel.transform, UITheme.Sp(13), TextAnchor.MiddleCenter, FontStyle.Bold);
        _waterAllLabel.color = Color.white;
        _waterAllLabel.raycastTarget = false;
        _waterAllLabel.gameObject.AddComponent<LayoutElement>().flexibleHeight = 1f;

        _waterAllSubLabel = UIFactory.Text("WaterAllSubLbl", "\u2014", waterPanel.transform, UITheme.Sp(10), TextAnchor.MiddleCenter, FontStyle.Bold);
        _waterAllSubLabel.raycastTarget = false;
        _waterAllSubLabel.gameObject.AddComponent<LayoutElement>().minHeight = UITheme.Dp(13f);

        ButtonUtils.AddListener(_waterAllButton, OnWaterAllClicked);

        // --- Auto-Water toggle button ---
        var autoPanel = UIFactory.Panel("AutoWaterPanel", actionPanel.transform, new Color(0.21f, 0.33f, 0.27f));
        _autoWaterButton = autoPanel.AddComponent<Button>();
        _autoWaterButton.transition = Selectable.Transition.None;

        var aVlg = autoPanel.AddComponent<VerticalLayoutGroup>();
        aVlg.childControlHeight = true; aVlg.childControlWidth = true;
        aVlg.childForceExpandHeight = false; aVlg.childForceExpandWidth = true;
        aVlg.childAlignment = TextAnchor.MiddleCenter;

        _autoWaterLabel = UIFactory.Text("AutoWaterLbl", "Auto-Water", autoPanel.transform, UITheme.Sp(13), TextAnchor.MiddleCenter, FontStyle.Bold);
        _autoWaterLabel.color = Color.white;
        _autoWaterLabel.raycastTarget = false;
        _autoWaterLabel.gameObject.AddComponent<LayoutElement>().flexibleHeight = 1f;

        _autoWaterSubLabel = UIFactory.Text("AutoWaterSubLbl", "\u2014", autoPanel.transform, UITheme.Sp(10), TextAnchor.MiddleCenter, FontStyle.Bold);
        _autoWaterSubLabel.raycastTarget = false;
        _autoWaterSubLabel.gameObject.AddComponent<LayoutElement>().minHeight = UITheme.Dp(13f);

        ButtonUtils.AddListener(_autoWaterButton, OnAutoWaterClicked);

        // --- Filter Tabs Toolbar (4 Pills: All, Thirsty, Ready, Empty) ---
        CreateFilterToolbar(_mainBG.transform);

        // --- Pot-Liste (scrollable) ---
        var listPanel = UIFactory.Panel("PotList", _mainBG.transform, new Color(0.08f, 0.08f, 0.08f, 0.95f));
        var listLE = listPanel.AddComponent<LayoutElement>();
        listLE.flexibleHeight = 1f;

        if (listPanel.GetComponent<RectMask2D>() == null)
        {
            listPanel.AddComponent<RectMask2D>();
        }

        _listContent = UIFactory.ScrollableVerticalList("PotListScroll", listPanel.transform, out var potScrollRect);
        UIFactory.FitContentHeight(_listContent);
        _listContent.sizeDelta = new Vector2(0f, _listContent.sizeDelta.y);

        if (potScrollRect != null)
        {
            potScrollRect.vertical = true;
            potScrollRect.movementType = ScrollRect.MovementType.Clamped;
        }

        _initialScanComplete = false;
        ApplyLoadingState();
        PotTracker.Instance.RefreshNow();
        RefreshList();
        RefreshWaterAllButton();
        RefreshAutoWaterButton();
    }

    private void CreateFilterToolbar(Transform parent)
    {
        var filterPanel = UIFactory.Panel("FilterToolbar", parent, Color.clear);
        var filterLE = filterPanel.AddComponent<LayoutElement>();
        filterLE.minHeight = UITheme.Dp(26f);
        filterLE.preferredHeight = UITheme.Dp(26f);
        filterLE.flexibleHeight = 0f;

        var filterHlg = filterPanel.AddComponent<HorizontalLayoutGroup>();
        filterHlg.spacing = UITheme.Dp(4f);
        filterHlg.childControlWidth = true;
        filterHlg.childControlHeight = true;
        filterHlg.childForceExpandWidth = true;
        filterHlg.childForceExpandHeight = true;

        _filterButtons.Clear();

        CreateFilterPill(filterPanel.transform, PotFilter.All, "All");
        CreateFilterPill(filterPanel.transform, PotFilter.Thirsty, "\U0001F4A7 Thirsty");
        CreateFilterPill(filterPanel.transform, PotFilter.Ready, "\u2605 Ready");
        CreateFilterPill(filterPanel.transform, PotFilter.Empty, "\u2B21 Empty");

        UpdateFilterButtonStyles();
    }

    private void CreateFilterPill(Transform parent, PotFilter filter, string label)
    {
        var pillPanel = UIFactory.Panel($"Filter_{filter}", parent, new Color(0.12f, 0.15f, 0.20f, 1f));
        var btn = pillPanel.AddComponent<Button>();
        btn.transition = Selectable.Transition.None;

        var vlg = pillPanel.AddComponent<VerticalLayoutGroup>();
        vlg.childControlHeight = true;
        vlg.childControlWidth = true;
        vlg.childForceExpandHeight = true;
        vlg.childForceExpandWidth = true;
        vlg.childAlignment = TextAnchor.MiddleCenter;

        var txt = UIFactory.Text($"Lbl_{filter}", label, pillPanel.transform, UITheme.Sp(10), TextAnchor.MiddleCenter, FontStyle.Bold);
        txt.color = new Color(0.65f, 0.70f, 0.78f, 1f);
        txt.raycastTarget = false;

        var img = pillPanel.GetComponent<Image>();
        _filterButtons[filter] = (img, txt, btn);

        ButtonUtils.AddListener(btn, () => SetFilter(filter));
    }

    private void SetFilter(PotFilter filter)
    {
        if (_activeFilter == filter) return;
        _activeFilter = filter;
        UpdateFilterButtonStyles();
        UpdatePropertyVisibilities();
    }

    private void UpdateFilterButtonStyles()
    {
        foreach (var kvp in _filterButtons)
        {
            var filter = kvp.Key;
            var (img, txt, _) = kvp.Value;
            bool isActive = filter == _activeFilter;

            if (isActive)
            {
                txt.color = Color.white;
                img.color = filter switch
                {
                    PotFilter.Thirsty => new Color(0.16f, 0.36f, 0.60f, 1f),
                    PotFilter.Ready => new Color(0.55f, 0.45f, 0.15f, 1f),
                    PotFilter.Empty => new Color(0.30f, 0.30f, 0.35f, 1f),
                    _ => new Color(0.20f, 0.30f, 0.45f, 1f)
                };
            }
            else
            {
                txt.color = new Color(0.65f, 0.70f, 0.78f, 1f);
                img.color = new Color(0.12f, 0.15f, 0.20f, 1f);
            }
        }
    }

    private static bool MatchesFilter(PotInfo p, PotFilter filter)
    {
        return filter switch
        {
            PotFilter.All => true,
            PotFilter.Thirsty => p.WaterPercent < Constants.WaterAllSkipThreshold && !p.IsFullyGrown && !string.IsNullOrEmpty(p.PlantName),
            PotFilter.Ready => p.IsFullyGrown,
            PotFilter.Empty => string.IsNullOrEmpty(p.PlantName),
            _ => true
        };
    }

    private void ApplyLoadingState()
    {
        if (_waterAllButton != null)
        {
            _waterAllButton.interactable = false;
            _waterAllLabel.text = "Water All";
            _waterAllSubLabel.text = "\u2026";
            var wImg = _waterAllButton.GetComponent<Image>();
            if (wImg != null) wImg.color = new Color(0.12f, 0.20f, 0.32f, 1f);
        }
        if (_autoWaterButton != null)
        {
            _autoWaterButton.interactable = false;
            _autoWaterLabel.text = "Auto-Water";
            _autoWaterSubLabel.text = "\u2026";
            var aImg = _autoWaterButton.GetComponent<Image>();
            if (aImg != null) aImg.color = new Color(0.16f, 0.22f, 0.20f, 1f);
        }
    }

    private void OnWaterAllClicked()
    {
        var result = WaterAllService.WaterAll();
        MelonLogger.Msg($"WaterAll \u2192 {result.Message}");
    }

    private void OnAutoWaterClicked()
    {
        bool newState = !AutoWaterService.IsEnabled;
        AutoWaterService.SetEnabled(newState);
        S1Mods.Shared.ModConfig<PotScannerConfig>.SetAndSave(nameof(PotScannerConfig.AutoWaterEnabled), newState);
        RefreshAutoWaterButton();
    }

    private void RefreshWaterAllButton()
    {
        if (_waterAllButton == null || _waterAllLabel == null || _waterAllSubLabel == null) return;

        if (!_initialScanComplete)
        {
            _waterAllButton.interactable = false;
            _waterAllLabel.text = "Water All";
            _waterAllSubLabel.text = "\u2026";
            _waterAllButton.transition = Selectable.Transition.None;
            var wImg = _waterAllButton.GetComponent<Image>();
            if (wImg != null) wImg.color = new Color(0.12f, 0.20f, 0.32f, 1f);
            return;
        }

        var (count, cost, canEnable, reason, minWater) = WaterAllService.GetButtonState();

        _waterAllLabel.text = "Water All";
        if (canEnable)
        {
            _waterAllSubLabel.text = $"{count} x {cost:0}g";
        }
        else
        {
            if (reason == "All pots watered" || (count == 0 && reason != "No owned pots"))
            {
                _waterAllSubLabel.text = "All Moist";
            }
            else if (reason == "No owned pots")
            {
                _waterAllSubLabel.text = "No Pots";
            }
            else if (reason.StartsWith("Not enough cash"))
            {
                _waterAllSubLabel.text = "No Cash";
            }
            else
            {
                _waterAllSubLabel.text = "All Moist";
            }
        }

        _waterAllButton.interactable = canEnable;
        _waterAllButton.transition = Selectable.Transition.None;

        var img = _waterAllButton.GetComponent<Image>();
        if (img != null)
        {
            img.color = canEnable
                ? new Color(0.16f, 0.36f, 0.60f, 1f)
                : new Color(0.12f, 0.20f, 0.32f, 1f);
        }
    }

    private void RefreshAutoWaterButton()
    {
        if (_autoWaterButton == null || _autoWaterLabel == null || _autoWaterSubLabel == null) return;

        if (!_initialScanComplete)
        {
            _autoWaterButton.interactable = false;
            _autoWaterLabel.text = "Auto-Water";
            _autoWaterSubLabel.text = "\u2026";
            _autoWaterButton.transition = Selectable.Transition.None;
            var aImg = _autoWaterButton.GetComponent<Image>();
            if (aImg != null) aImg.color = new Color(0.16f, 0.22f, 0.20f, 1f);
            return;
        }

        bool on = AutoWaterService.IsEnabled;
        _autoWaterLabel.text = "Auto-Water";
        _autoWaterSubLabel.text = on ? "ON" : "OFF";
        _autoWaterButton.transition = Selectable.Transition.None;
        _autoWaterButton.interactable = true;

        var img = _autoWaterButton.GetComponent<Image>();
        if (img != null)
        {
            img.color = on
                ? new Color(0.16f, 0.55f, 0.30f, 1f)
                : new Color(0.20f, 0.30f, 0.26f, 1f);
        }
    }

    public void RefreshList()
    {
        if (_listContent == null) return;
        _lastRefreshRealtime = Time.realtimeSinceStartup;
        _initialScanComplete = true;

        var pots = PotTracker.Instance.Pots;

        // Non-allocating check for in-place updates (0 GC allocations)
        bool canInPlaceUpdate = _rowCache.Count > 0 && _rowCache.Count == pots.Count;
        if (canInPlaceUpdate)
        {
            for (int i = 0; i < pots.Count; i++)
            {
                if (!_rowCache.ContainsKey(pots[i].NativePtr))
                {
                    canInPlaceUpdate = false;
                    break;
                }
            }
        }

        if (canInPlaceUpdate)
        {
            for (int i = 0; i < pots.Count; i++)
            {
                var p = pots[i];
                if (!_rowCache.TryGetValue(p.NativePtr, out var refUI)) continue;
                refUI.PotInfo = p;

                string statusIcon = p.IsFullyGrown ? "\u2605" : !string.IsNullOrEmpty(p.PlantName) ? "\u25b6" : "\u2022";
                string plantName = string.IsNullOrEmpty(p.PlantName) ? "Pot" : p.PlantName;
                Color iconColor = p.IsFullyGrown
                    ? new Color(1f, 0.85f, 0.3f, 1f)
                    : !string.IsNullOrEmpty(p.PlantName)
                        ? new Color(0.4f, 0.9f, 0.4f, 1f)
                        : new Color(0.5f, 0.5f, 0.55f, 1f);

                string titleTextStr = !string.IsNullOrEmpty(p.PlantName)
                    ? $"{statusIcon} {plantName}\nQ: {Mathf.RoundToInt(p.Quality * 100f)}%"
                    : $"{statusIcon} {plantName}";

                refUI.TitleText.text = titleTextStr;
                refUI.TitleText.color = iconColor;

                refUI.WaterFillRt.anchorMax = new Vector2(Mathf.Clamp01(p.WaterPercent), 1f);
                if (refUI.WaterBadgeText != null)
                    refUI.WaterBadgeText.text = $"\U0001F4A7 {Mathf.RoundToInt(p.WaterPercent * 100f)}%";

                refUI.GrowthFillRt.anchorMax = new Vector2(Mathf.Clamp01(p.GrowthPercent), 1f);
                if (refUI.GrowthFillImage != null)
                    refUI.GrowthFillImage.color = p.IsFullyGrown ? new Color(0.10f, 0.95f, 0.45f, 1f) : new Color(0.18f, 0.82f, 0.35f, 1f);
                if (refUI.GrowthBadgeText != null)
                    refUI.GrowthBadgeText.text = $"{Mathf.RoundToInt(p.GrowthPercent * 100f)}%";

                refUI.SoilFillRt.anchorMax = new Vector2(Mathf.Clamp01(p.SoilPercent), 1f);
                if (refUI.SoilBadgeText != null)
                    refUI.SoilBadgeText.text = p.SoilPercent > 0.01f ? $"{Mathf.RoundToInt(p.SoilPercent * 100f)}%" : "No Soil";
            }

            UpdatePropertyVisibilities();
            return;
        }

        _propertyGroups.Clear();
        _rowCache.Clear();
        UIFactory.ClearChildren(_listContent);

        var grouped = pots
            .GroupBy(p => string.IsNullOrEmpty(p.PropertyName)
                ? (string.IsNullOrEmpty(p.PropertyCode) ? "(unknown)" : p.PropertyCode)
                : p.PropertyName)
            .ToList();

        var byProperty = grouped
            .OrderBy(g => g.Min(p => GetUrgency(p)))
            .ThenBy(g => g.Key)
            .ToList();

        if (_activePropertyKey != null && !byProperty.Any(g => g.Key == _activePropertyKey))
        {
            _activePropertyKey = null;
        }

        foreach (var group in byProperty)
        {
            string propKey = group.Key;
            var groupPots = group
                .OrderBy(p => string.IsNullOrEmpty(p.PlantName) ? "zzz" : p.PlantName)
                .ThenBy(p => p.NativePtr.ToInt64())
                .ToList();

            var groupRef = new PropertyGroupUIRef { PropertyKey = propKey };
            _propertyGroups[propKey] = groupRef;

            CreatePropertyHeaderRow(propKey, groupPots, groupRef);

            foreach (var pot in groupPots)
            {
                var potRowGo = CreatePotRow(pot);
                groupRef.Rows.Add(new PotRowEntry { PotInfo = pot, RowObject = potRowGo });
            }
        }

        UpdatePropertyVisibilities();
    }

    private int GetUrgency(PotInfo p)
    {
        if (p.WaterPercent < Constants.WaterAllSkipThreshold && !string.IsNullOrEmpty(p.PlantName) && !p.IsFullyGrown) return 1;
        if (p.IsFullyGrown) return 2;
        if (!string.IsNullOrEmpty(p.PlantName)) return 3;
        return 4;
    }

    private void CreatePropertyHeaderRow(string propKey, List<PotInfo> pots, PropertyGroupUIRef groupRef)
    {
        Color normalBg = new Color(0.11f, 0.15f, 0.22f, 1f);
        var row = UIFactory.Panel($"Header_{propKey}", _listContent, normalBg, fullAnchor: true);
        var rowImg = row.GetComponent<Image>();
        if (rowImg != null) rowImg.raycastTarget = true;

        var le = row.AddComponent<LayoutElement>();
        le.minHeight = UITheme.Dp(30f);
        le.preferredHeight = UITheme.Dp(30f);
        le.flexibleWidth = 1f;
        le.flexibleHeight = 0f;

        var btn = row.AddComponent<Button>();
        btn.transition = Selectable.Transition.ColorTint;
        if (rowImg != null) btn.targetGraphic = rowImg;

        var colors = btn.colors;
        colors.normalColor = normalBg;
        colors.highlightedColor = new Color(0.18f, 0.26f, 0.38f, 1f);
        colors.pressedColor = new Color(0.25f, 0.38f, 0.52f, 1f);
        colors.selectedColor = normalBg;
        btn.colors = colors;

        var content = UIFactory.Panel("Content", row.transform, Color.clear, fullAnchor: true);
        var contentImg = content.GetComponent<Image>();
        if (contentImg != null) contentImg.raycastTarget = false;
        var contentLE = content.AddComponent<LayoutElement>();
        contentLE.ignoreLayout = true;

        int total = pots.Count;
        int ready = pots.Count(p => p.IsFullyGrown);
        int dry = pots.Count(p => p.WaterPercent < Constants.WaterAllSkipThreshold && !p.IsFullyGrown && !string.IsNullOrEmpty(p.PlantName));

        string badgeText = ready > 0 ? $"{ready} ready" : dry > 0 ? $"{dry} thirsty" : $"{total} pots";
        Color badgeColor = ready > 0
            ? new Color(1f, 0.85f, 0.3f, 1f)
            : dry > 0
                ? new Color(0.9f, 0.4f, 0.4f, 1f)
                : new Color(0.7f, 0.7f, 0.7f, 1f);

        var titleText = UIFactory.Text($"Header_{propKey}_Title", $"\u25b6  {propKey}",
            content.transform, UITheme.Sp(13), TextAnchor.MiddleLeft, FontStyle.Bold);
        titleText.color = Color.white;
        titleText.raycastTarget = false;
        var titleRt = titleText.rectTransform;
        titleRt.anchorMin = new Vector2(0f, 0f);
        titleRt.anchorMax = new Vector2(0.68f, 1f);
        titleRt.offsetMin = new Vector2(UITheme.Dp(8f), 0f);
        titleRt.offsetMax = new Vector2(0f, 0f);

        var subText = UIFactory.Text($"Header_{propKey}_Badge", badgeText,
            content.transform, UITheme.Sp(10), TextAnchor.MiddleRight, FontStyle.Bold);
        subText.color = badgeColor;
        subText.raycastTarget = false;
        var subRt = subText.rectTransform;
        subRt.anchorMin = new Vector2(0.68f, 0f);
        subRt.anchorMax = new Vector2(1f, 1f);
        subRt.offsetMin = new Vector2(0f, 0f);
        subRt.offsetMax = new Vector2(-UITheme.Dp(8f), 0f);

        groupRef.HeaderObject = row;
        groupRef.HeaderImage = rowImg!;
        groupRef.TitleText = titleText;
        groupRef.BadgeText = subText;
        groupRef.HeaderButton = btn;

        string captureKey = propKey;
        ButtonUtils.AddListener(btn, () => TogglePropertyExpanded(captureKey));
    }

    private void TogglePropertyExpanded(string propKey)
    {
        if (_activePropertyKey == propKey)
        {
            _activePropertyKey = null;
        }
        else
        {
            _activePropertyKey = propKey;
        }

        UpdatePropertyVisibilities();
    }

    private void UpdatePropertyVisibilities()
    {
        foreach (var kvp in _propertyGroups)
        {
            string key = kvp.Key;
            var groupRef = kvp.Value;

            int totalCount = groupRef.Rows.Count;
            int readyCount = 0;
            int dryCount = 0;
            int emptyCount = 0;
            int matchingCount = 0;

            for (int i = 0; i < groupRef.Rows.Count; i++)
            {
                var p = groupRef.Rows[i].PotInfo;
                if (p.IsFullyGrown) readyCount++;
                if (p.WaterPercent < Constants.WaterAllSkipThreshold && !p.IsFullyGrown && !string.IsNullOrEmpty(p.PlantName)) dryCount++;
                if (string.IsNullOrEmpty(p.PlantName)) emptyCount++;

                if (MatchesFilter(p, _activeFilter))
                    matchingCount++;
            }

            // Determine badge text and color based on active filter
            string badgeText;
            Color badgeColor;

            switch (_activeFilter)
            {
                case PotFilter.Thirsty:
                    badgeText = $"{matchingCount} thirsty";
                    badgeColor = new Color(0.9f, 0.4f, 0.4f, 1f);
                    break;
                case PotFilter.Ready:
                    badgeText = $"{matchingCount} ready";
                    badgeColor = new Color(1f, 0.85f, 0.3f, 1f);
                    break;
                case PotFilter.Empty:
                    badgeText = $"{matchingCount} empty";
                    badgeColor = new Color(0.7f, 0.7f, 0.75f, 1f);
                    break;
                default: // All
                    if (readyCount > 0)
                    {
                        badgeText = $"{readyCount} ready";
                        badgeColor = new Color(1f, 0.85f, 0.3f, 1f);
                    }
                    else if (dryCount > 0)
                    {
                        badgeText = $"{dryCount} thirsty";
                        badgeColor = new Color(0.9f, 0.4f, 0.4f, 1f);
                    }
                    else
                    {
                        badgeText = $"{totalCount} pots";
                        badgeColor = new Color(0.7f, 0.7f, 0.7f, 1f);
                    }
                    break;
            }

            if (groupRef.BadgeText != null)
            {
                groupRef.BadgeText.text = badgeText;
                groupRef.BadgeText.color = badgeColor;
            }

            // Hide group if no pots match the active filter
            if (matchingCount == 0)
            {
                if (groupRef.HeaderObject != null)
                    groupRef.HeaderObject.SetActive(false);

                for (int i = 0; i < groupRef.Rows.Count; i++)
                {
                    if (groupRef.Rows[i].RowObject != null)
                        groupRef.Rows[i].RowObject.SetActive(false);
                }
                continue;
            }

            // Group has matching pots
            if (_activePropertyKey == null)
            {
                if (groupRef.HeaderObject != null)
                    groupRef.HeaderObject.SetActive(true);

                if (groupRef.TitleText != null)
                    groupRef.TitleText.text = $"\u25b6  {key}";

                Color normalBg = new Color(0.11f, 0.15f, 0.22f, 1f);
                if (groupRef.HeaderImage != null) groupRef.HeaderImage.color = normalBg;
                if (groupRef.HeaderButton != null)
                {
                    var colors = groupRef.HeaderButton.colors;
                    colors.normalColor = normalBg;
                    colors.highlightedColor = new Color(0.18f, 0.26f, 0.38f, 1f);
                    colors.pressedColor = new Color(0.25f, 0.38f, 0.52f, 1f);
                    colors.selectedColor = normalBg;
                    groupRef.HeaderButton.colors = colors;
                }

                for (int i = 0; i < groupRef.Rows.Count; i++)
                {
                    if (groupRef.Rows[i].RowObject != null)
                        groupRef.Rows[i].RowObject.SetActive(false);
                }
            }
            else if (_activePropertyKey == key)
            {
                if (groupRef.HeaderObject != null)
                    groupRef.HeaderObject.SetActive(true);

                if (groupRef.TitleText != null)
                    groupRef.TitleText.text = $"\u25bc  {key}";

                Color activeBg = new Color(0.15f, 0.22f, 0.32f, 1f);
                if (groupRef.HeaderImage != null) groupRef.HeaderImage.color = activeBg;
                if (groupRef.HeaderButton != null)
                {
                    var colors = groupRef.HeaderButton.colors;
                    colors.normalColor = activeBg;
                    colors.highlightedColor = new Color(0.20f, 0.30f, 0.42f, 1f);
                    colors.pressedColor = new Color(0.25f, 0.38f, 0.52f, 1f);
                    colors.selectedColor = activeBg;
                    groupRef.HeaderButton.colors = colors;
                }

                for (int i = 0; i < groupRef.Rows.Count; i++)
                {
                    var rowEntry = groupRef.Rows[i];
                    if (rowEntry.RowObject != null)
                    {
                        bool match = MatchesFilter(rowEntry.PotInfo, _activeFilter);
                        rowEntry.RowObject.SetActive(match);
                    }
                }
            }
            else
            {
                if (groupRef.HeaderObject != null)
                    groupRef.HeaderObject.SetActive(false);

                for (int i = 0; i < groupRef.Rows.Count; i++)
                {
                    if (groupRef.Rows[i].RowObject != null)
                        groupRef.Rows[i].RowObject.SetActive(false);
                }
            }
        }
    }

    private GameObject CreatePotRow(PotInfo p)
    {
        var row = UIFactory.Panel($"Pot_{p.NativePtr:X}", _listContent,
            new Color(0.08f, 0.11f, 0.15f, 1f),
            fullAnchor: true);
        var rowImg = row.GetComponent<Image>();
        if (rowImg != null) rowImg.raycastTarget = false;

        var le = row.AddComponent<LayoutElement>();
        le.minHeight = UITheme.Dp(46f);
        le.preferredHeight = UITheme.Dp(46f);
        le.flexibleWidth = 1f;
        le.flexibleHeight = 0f;

        var content = UIFactory.Panel("Content", row.transform, Color.clear, fullAnchor: true);
        var contentImg = content.GetComponent<Image>();
        if (contentImg != null) contentImg.raycastTarget = false;
        var contentLE = content.AddComponent<LayoutElement>();
        contentLE.ignoreLayout = true;

        string statusIcon = p.IsFullyGrown ? "\u2605" : !string.IsNullOrEmpty(p.PlantName) ? "\u25b6" : "\u2022";
        string plantName = string.IsNullOrEmpty(p.PlantName) ? "Pot" : p.PlantName;

        Color iconColor = p.IsFullyGrown
            ? new Color(1f, 0.85f, 0.3f, 1f)
            : !string.IsNullOrEmpty(p.PlantName)
                ? new Color(0.4f, 0.9f, 0.4f, 1f)
                : new Color(0.5f, 0.5f, 0.55f, 1f);

        string titleTextStr = !string.IsNullOrEmpty(p.PlantName)
            ? $"{statusIcon} {plantName}\nQ: {Mathf.RoundToInt(p.Quality * 100f)}%"
            : $"{statusIcon} {plantName}";

        var titleText = UIFactory.Text($"Pot_{p.NativePtr:X}_Title", titleTextStr,
            content.transform, UITheme.Sp(11), TextAnchor.MiddleLeft, FontStyle.Bold);
        titleText.color = iconColor;
        titleText.raycastTarget = false;
        titleText.horizontalOverflow = HorizontalWrapMode.Wrap;
        titleText.verticalOverflow = VerticalWrapMode.Truncate;
        var titleRt = titleText.rectTransform;
        titleRt.anchorMin = new Vector2(0f, 0.05f);
        titleRt.anchorMax = new Vector2(0.40f, 0.95f);
        titleRt.offsetMin = new Vector2(UITheme.Dp(6f), 0f);
        titleRt.offsetMax = Vector2.zero;

        string waterPctText = $"\U0001F4A7 {Mathf.RoundToInt(p.WaterPercent * 100f)}%";
        string growthPctText = $"{Mathf.RoundToInt(p.GrowthPercent * 100f)}%";
        string soilPctText = p.SoilPercent > 0.01f ? $"{Mathf.RoundToInt(p.SoilPercent * 100f)}%" : "No Soil";

        // Dynamic click listener: always attach single-pot watering handler
        IntPtr ptr = p.NativePtr;
        Action onWaterClick = () => WaterAllService.WaterSinglePot(ptr);

        // 1. Water (Top)
        var (wFillRt, wFillImg, wBadge) = AddProgressBar(content.transform, "W", p.WaterPercent, new Color(0.23f, 0.51f, 0.96f, 1f), 0.68f, 0.26f, onWaterClick, waterPctText);
        // 2. Growth (Middle)
        var (gFillRt, gFillImg, gBadge) = AddProgressBar(content.transform, "G", p.GrowthPercent, p.IsFullyGrown ? new Color(0.10f, 0.95f, 0.45f, 1f) : new Color(0.18f, 0.82f, 0.35f, 1f), 0.36f, 0.26f, null, growthPctText);
        // 3. Soil (Bottom)
        var (sFillRt, sFillImg, sBadge) = AddProgressBar(content.transform, "S", p.SoilPercent, new Color(0.62f, 0.42f, 0.22f, 1f), 0.04f, 0.26f, null, soilPctText);

        _rowCache[p.NativePtr] = new PotRowUIRef
        {
            PotInfo = p,
            RowObject = row,
            TitleText = titleText,
            WaterFillRt = wFillRt,
            WaterBadgeText = wBadge,
            GrowthFillRt = gFillRt,
            GrowthFillImage = gFillImg,
            GrowthBadgeText = gBadge,
            SoilFillRt = sFillRt,
            SoilBadgeText = sBadge
        };

        return row;
    }

    private (RectTransform fillRt, Image? fillImg, Text? badgeText) AddProgressBar(Transform parent, string label, float percent, Color fillCol, float anchorYOffset, float barHeight = 0.26f, Action? onClick = null, string? badgeText = null)
    {
        var lbl = UIFactory.Text("Lbl", label, parent, UITheme.Sp(9), TextAnchor.MiddleRight, FontStyle.Bold);
        lbl.color = new Color(0.8f, 0.8f, 0.8f, 1f);
        lbl.raycastTarget = false;
        var lblRt = lbl.rectTransform;
        lblRt.anchorMin = new Vector2(0.41f, anchorYOffset);
        lblRt.anchorMax = new Vector2(0.46f, anchorYOffset + barHeight);
        lblRt.offsetMin = Vector2.zero; lblRt.offsetMax = Vector2.zero;

        if (onClick != null)
        {
            var border = UIFactory.Panel("Border", parent, new Color(0.30f, 0.65f, 0.95f, 0.4f), fullAnchor: true);
            var borderRt = border.GetComponent<RectTransform>();
            borderRt.anchorMin = new Vector2(0.465f, anchorYOffset - 0.015f);
            borderRt.anchorMax = new Vector2(0.985f, anchorYOffset + barHeight + 0.015f);
            borderRt.offsetMin = Vector2.zero; borderRt.offsetMax = Vector2.zero;
            var borderImg = border.GetComponent<Image>();
            if (borderImg != null) borderImg.raycastTarget = false;
        }

        var track = UIFactory.Panel("Track", parent, new Color(0.12f, 0.14f, 0.18f, 1f), fullAnchor: true);
        var trackRt = track.GetComponent<RectTransform>();
        trackRt.anchorMin = new Vector2(0.47f, anchorYOffset);
        trackRt.anchorMax = new Vector2(0.98f, anchorYOffset + barHeight);
        trackRt.offsetMin = Vector2.zero; trackRt.offsetMax = Vector2.zero;

        var fill = UIFactory.Panel("Fill", track.transform, fillCol, fullAnchor: true);
        var fillImage = fill.GetComponent<Image>();
        if (fillImage != null) fillImage.raycastTarget = false;

        var fillRt = fill.GetComponent<RectTransform>();
        fillRt.anchorMin = new Vector2(0, 0);
        fillRt.anchorMax = new Vector2(Mathf.Clamp01(percent), 1);
        fillRt.offsetMin = Vector2.zero; fillRt.offsetMax = Vector2.zero;

        Text? badge = null;
        if (!string.IsNullOrEmpty(badgeText))
        {
            badge = UIFactory.Text("Badge", badgeText, track.transform, UITheme.Sp(9), TextAnchor.MiddleCenter, FontStyle.Bold);
            badge.color = Color.white;
            badge.raycastTarget = false;
            var badgeRt = badge.rectTransform;
            badgeRt.anchorMin = Vector2.zero;
            badgeRt.anchorMax = Vector2.one;
            badgeRt.offsetMin = Vector2.zero; badgeRt.offsetMax = Vector2.zero;
        }

        if (onClick != null)
        {
            var trackImage = track.GetComponent<Image>();
            if (trackImage != null) trackImage.raycastTarget = true;

            var btn = track.AddComponent<Button>();
            btn.transition = Selectable.Transition.None;
            ButtonUtils.AddListener(btn, onClick);
        }

        return (fillRt, fillImage, badge);
    }

    private void OnPotsScannedHandler()
    {
        if (IsOpen())
        {
            RefreshList();
            RefreshWaterAllButton();
            RefreshAutoWaterButton();
        }
    }

    protected override void OnPhoneClosed()
    {
        if (_mainBG != null) _mainBG.SetActive(false);
        // Update + OnPotsScanned bleiben lebenslang subscribed (defensives Unsubscribe-Subscribe in OnCreated).
    }

    private void Update()
    {
        bool open = IsOpen();
        if (_mainBG != null && _mainBG.activeSelf != open)
        {
            _mainBG.SetActive(open);
            if (open)
            {
                RefreshList();
                RefreshWaterAllButton();
                RefreshAutoWaterButton();
            }
        }
    }
}
