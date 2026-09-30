using System;
using System.Collections.Generic;
using System.Linq;
using MelonLoader;
using PotScanner.Services;
using PotScanner.Utils;
using S1API.PhoneApp;
using S1API.UI;
using S1API.Utils;
using S1Mods.Shared;
using UnityEngine;
using UnityEngine.UI;
using UITheme = S1Mods.Shared.UITheme;

namespace PotScanner;

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
/// v0.7.0 view rewrite onto the shared BankApp palette (S1Mods.Shared.GamePalette): flat opaque
/// surfaces, 1 px outlines, semantic accents and teal value numbers. Pure view change — life
/// cycle, polling, filtering and all WaterAllService/AutoWaterService behaviour are unchanged.
/// </summary>
public sealed class PotScannerApp : PhoneApp
{
    // ============================ Design tokens (shared GamePalette) ============================
    // v0.7.0: every colour now comes from S1Mods.Shared.GamePalette — the screenshot-verified
    // BankApp v0.3.0 look, promoted to a shared palette so all phone apps read as one product.
    // The local names below keep the view code readable.
    //
    // Opacity (breaking change vs v0.6.0): surfaces are fully opaque. The old hierarchy came
    // from white-alpha "glass" fills (white 4-14%) over a near-black base; that cannot survive
    // an opaque palette and is replaced by steps of surface lightness
    // (Bg < Card < CardAlt < CardHover/Pressed < Border).
    //
    // Colour is semantic, never decorative: Blue = interactive/selected, Green = primary action,
    // Teal = value + progress-bar fill, Orange = ready, TextMuted = empty. Accent fills carry
    // WHITE ink (BankApp precedent), not dark ink.
    private const float CardCornerRadius = 8f;
    private const float ChipCornerRadius = 6f;

    private static readonly Color BgColor = GamePalette.Bg;
    private static readonly Color CardFill = GamePalette.Card;
    private static readonly Color CardFillSoft = GamePalette.CardAlt;
    private static readonly Color CardBorderColor = GamePalette.Border;
    private static readonly Color CardHover = GamePalette.CardHover;
    private static readonly Color CardPressed = GamePalette.CardPressed;
    private static readonly Color ActionStrong = GamePalette.Green;      // Water All, enabled
    private static readonly Color ActionActive = GamePalette.CardAlt;    // Auto-Water, ON
    private static readonly Color ActionIdle = GamePalette.Card;         // Auto-Water, OFF
    private static readonly Color PillActive = GamePalette.Blue;         // active filter chip
    private static readonly Color CardExpanded = GamePalette.CardAlt;    // expanded property header
    private static readonly Color ClickRing = GamePalette.Blue;          // water-bar click affordance
    private static readonly Color BarFill = GamePalette.Teal;            // progress-bar fills
    private static readonly Color ReadyAccent = GamePalette.Orange;      // status dot: fully grown
    private static readonly Color GrowthAccent = GamePalette.Green;      // status dot: planted
    private static readonly Color EmptyAccent = GamePalette.TextMuted;   // status dot: no plant
    private static readonly Color TextPrimary = GamePalette.TextPrimary;
    private static readonly Color TextMuted = GamePalette.TextMuted;
    private static readonly Color TextDim = GamePalette.TextDim;
    private static readonly Color LabelDim = GamePalette.TextDim;
    private static readonly Color BarTrack = GamePalette.Border;

    // ============================ Layout bands (overlap-safe) ============================
    // Main stack: VerticalLayoutGroup (top to bottom, spacing Dp(8), padding Dp(8) all sides):
    //   [1] Hero summary card   fixed  Dp(96)  - scan summary + donut moisture gauge
    //   [2] Action row          fixed  Dp(48)  - Water All + Auto-Water glass cards
    //   [3] Filter toolbar      fixed  Dp(28)  - 4 chips (All/Thirsty/Ready/Empty)
    //   [4] Pot list            flex 1         - scrollable cards (RectMask2D-clipped)
    // Fixed total = Dp(96+48+28) + 3x spacing Dp(8) + 2x padding Dp(8) = Dp(220).
    // (No footer band: the app label + version pill were removed on request, and the pot
    // list — the only flexible child — absorbs the freed Dp(24) plus one spacing step.)
    // Worst-case math vs available canvas height H (UITheme: Scale = clamp(H/900, 0.75, 1.2)):
    //   - Proportional regime (675 <= H <= 1080): fixed = 220 * H/900 = 0.244 * H ->
    //     the flexible list band always keeps >= 75% of H. Bands stack top-down and can
    //     never overlap (childForceExpandHeight = false, one flexible child absorbs slack).
    //   - Clamped-high regime (H > 1080): fixed = 220 * 1.2 = 264 px, shrinking relative
    //     to H as H grows. No overflow possible.
    //   - Clamped-low regime (H < 675): Scale clamps at 0.75 -> fixed = 220 * 0.75 = 165 px.
    //     Requires H >= 165 px; every supported phone canvas is >= 600 px tall (the old
    //     layout required only H >= 62 px, so this is strictly safer than before).
    //   The list band is the only flexible child and is RectMask2D-clipped, so long lists
    //   scroll instead of ever overlapping the bands above.
    //
    // Hero card internals (absolute anchors inside the hero card, x fractions of card width):
    //   overline  x 0.05-0.70  y 0.76-0.94   "SCAN SUMMARY"  (Sp 12, uppercase, white 55%)
    //   title     x 0.05-0.70  y 0.44-0.76   "N POTS"        (Sp 30, bold, white 85%)
    //   meta      x 0.05-0.72  y 0.24-0.42   counts line     (Sp 11, white 60%)
    //   avg label x 0.05-0.72  y 0.06-0.22   "AVG WATER N%"  (Sp 11, bold, white 85%)
    //   donut     center (0.835, 0.52), D = min(0.78 * cardH, 0.20 * cardW) ->
    //             x span 0.735-0.935, clear of the text column (right edge 0.72) at any width.
    // Pot row internals (row height Dp(52), x fractions of row width):
    //   status dot x 0.02-0.08 (circle sprite, semantic hue); title x 0.085-0.40 y 0.52-0.94;
    //   quality x 0.085-0.40 y 0.08-0.48; metric label x 0.40-0.455; capsule track x 0.465-0.855
    //   (Dp(7.5) high, centered in its band); value numbers x 0.87-0.985 RIGHT-ALIGNED.
    //   Metric bands (y fractions, non-overlapping): water 0.68-0.94, growth 0.36-0.62,
    //   soil 0.04-0.30. Text column (x <= 0.40) and metric column (x >= 0.40) never overlap.
    // Property header internals (Dp(32)): chevron x 0.022-0.082 (shape-drawn, rotates on
    //   expand/collapse), title x 0.10-0.58, badge x 0.58-0.97 right-aligned.
    // Action card internals: label y 0.46-0.95, sublabel y 0.06-0.42 (both centered).
    // Every element is anchor-positioned with non-overlapping bands; heights are Dp/Sp
    // scaled by UITheme.Scale so the composition holds at every supported canvas size.

    private GameObject _mainBG = null!;
    private RectTransform _listContent = null!;
    private Button _waterAllButton = null!;
    private Text _waterAllLabel = null!;
    private Text _waterAllSubLabel = null!;
    private Image _waterAllFill = null!;
    private Button _autoWaterButton = null!;
    private Text _autoWaterLabel = null!;
    private Text _autoWaterSubLabel = null!;
    private Image _autoWaterFill = null!;
    private float _lastRefreshRealtime;
    private string? _activePropertyKey;
    private PotFilter _activeFilter = PotFilter.All;
    private readonly Dictionary<PotFilter, (Image bg, Text label, Button btn)> _filterButtons = new();

    // Hero summary refs (view only).
    private Text? _heroTotalText;
    private Text? _heroMetaText;
    private Text? _heroAvgText;
    private Image? _heroRingFill;

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
        public Text? QualityText;
        public Image? StatusDot;
        public RectTransform WaterFillRt = null!;
        public Image? WaterFillImage;
        public Text? WaterBadgeText;
        public RectTransform GrowthFillRt = null!;
        public Image? GrowthFillImage;
        public Text? GrowthBadgeText;
        public RectTransform SoilFillRt = null!;
        public Image? SoilFillImage;
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
        public RectTransform? ChevronRt;
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

    internal static void TearDownForSceneUnload()
    {
        try
        {
            // Drop per-scene row caches: they hold wrappers of destroyed native
            // objects (retention + stale NativePtr on the next in-place check).
            _active?._rowCache.Clear();
            _active?._propertyGroups.Clear();
        }
        catch { }
        _active = null;
    }

    private static void DispatchUpdate() => _active?.Update();

    private static void DispatchPotsScanned() => _active?.OnPotsScannedHandler();

    protected override void OnCreatedUI(GameObject container)
    {
        var containerRt = container.GetComponent<RectTransform>();
        if (containerRt != null)
        {
            UITheme.InitializeForDashboard(containerRt);
            MelonLogger.Msg($"Responsive canvas initialized: {UITheme.ActualWidth:F0}x{UITheme.ActualHeight:F0} (Scale={UITheme.Scale:F2})");
        }

        _mainBG = UIFactory.Panel("MainBG", container.transform, BgColor, fullAnchor: true);
        _mainBG.SetActive(false);

        var vlg = _mainBG.AddComponent<VerticalLayoutGroup>();
        vlg.childControlHeight = true;
        vlg.childControlWidth = true;
        vlg.childForceExpandHeight = false;
        vlg.childForceExpandWidth = true;
        vlg.spacing = UITheme.Dp(8f);
        vlg.padding = new RectOffset((int)UITheme.Dp(8f), (int)UITheme.Dp(8f), (int)UITheme.Dp(8f), (int)UITheme.Dp(8f));

        // --- [1] Hero summary card (scan summary + donut moisture gauge) ---
        CreateHeroCard(_mainBG.transform);

        // --- [2] ActionRow (2 buttons) ---
        var actionPanel = UIFactory.Panel("ActionRow", _mainBG.transform, Color.clear);
        var actionImg = actionPanel.GetComponent<Image>();
        if (actionImg != null) actionImg.raycastTarget = false;
        var actionLE = actionPanel.AddComponent<LayoutElement>();
        actionLE.minHeight = UITheme.Dp(48f);
        actionLE.preferredHeight = UITheme.Dp(48f);
        actionLE.flexibleHeight = 0f;

        var actionHlg = actionPanel.AddComponent<HorizontalLayoutGroup>();
        actionHlg.spacing = UITheme.Dp(8f);
        actionHlg.childControlWidth = true;
        actionHlg.childControlHeight = true;
        actionHlg.childForceExpandWidth = true;
        actionHlg.childForceExpandHeight = true;

        // --- Water All button (dark glass card, white 10% fill when enabled) ---
        var waterCard = CreateCard("WaterAllPanel", actionPanel.transform, ActionStrong, out _waterAllFill,
            raycastTarget: true, radius: ChipCornerRadius);
        _waterAllButton = waterCard.AddComponent<Button>();
        _waterAllButton.transition = Selectable.Transition.None;

        _waterAllLabel = UIFactory.Text("WaterAllLbl", "Water All", _waterAllFill.transform, UITheme.Sp(13), TextAnchor.MiddleCenter, FontStyle.Bold);
        _waterAllLabel.color = Color.white;
        _waterAllLabel.raycastTarget = false;
        AnchorRect(_waterAllLabel.rectTransform, 0.06f, 0.46f, 0.94f, 0.95f);

        _waterAllSubLabel = UIFactory.Text("WaterAllSubLbl", "-", _waterAllFill.transform, UITheme.Sp(10), TextAnchor.MiddleCenter, FontStyle.Bold);
        _waterAllSubLabel.raycastTarget = false;
        AnchorRect(_waterAllSubLabel.rectTransform, 0.06f, 0.06f, 0.94f, 0.42f);

        ButtonUtils.AddListener(_waterAllButton, OnWaterAllClicked);

        // --- Auto-Water toggle button (dark glass card, hierarchy via fill alpha only) ---
        var autoCard = CreateCard("AutoWaterPanel", actionPanel.transform, ActionIdle, out _autoWaterFill,
            raycastTarget: true, radius: ChipCornerRadius);
        _autoWaterButton = autoCard.AddComponent<Button>();
        _autoWaterButton.transition = Selectable.Transition.None;

        _autoWaterLabel = UIFactory.Text("AutoWaterLbl", "Auto-Water", _autoWaterFill.transform, UITheme.Sp(13), TextAnchor.MiddleCenter, FontStyle.Bold);
        _autoWaterLabel.color = Color.white;
        _autoWaterLabel.raycastTarget = false;
        AnchorRect(_autoWaterLabel.rectTransform, 0.06f, 0.46f, 0.94f, 0.95f);

        _autoWaterSubLabel = UIFactory.Text("AutoWaterSubLbl", "-", _autoWaterFill.transform, UITheme.Sp(10), TextAnchor.MiddleCenter, FontStyle.Bold);
        _autoWaterSubLabel.raycastTarget = false;
        AnchorRect(_autoWaterSubLabel.rectTransform, 0.06f, 0.06f, 0.94f, 0.42f);

        ButtonUtils.AddListener(_autoWaterButton, OnAutoWaterClicked);

        // --- [3] Filter Tabs Toolbar (4 Pills: All, Thirsty, Ready, Empty) ---
        CreateFilterToolbar(_mainBG.transform);

        // --- [4] Pot list (scrollable) ---
        var listPanel = UIFactory.Panel("PotList", _mainBG.transform, Color.clear);
        var listImg = listPanel.GetComponent<Image>();
        if (listImg != null) listImg.raycastTarget = false;
        var listLE = listPanel.AddComponent<LayoutElement>();
        listLE.flexibleHeight = 1f;

        if (listPanel.GetComponent<RectMask2D>() == null)
        {
            listPanel.AddComponent<RectMask2D>();
        }

        _listContent = UIFactory.ScrollableVerticalList("PotListScroll", listPanel.transform, out var potScrollRect);
        UIFactory.FitContentHeight(_listContent);
        _listContent.sizeDelta = new Vector2(0f, _listContent.sizeDelta.y);

        // 4/8/12/16 Dp spacing rhythm: rows are cards separated by Dp(5).
        var listVlg = _listContent.GetComponent<VerticalLayoutGroup>();
        if (listVlg != null)
        {
            listVlg.spacing = UITheme.Dp(5f);
            listVlg.padding = new RectOffset(0, 0, (int)UITheme.Dp(2f), (int)UITheme.Dp(6f));
        }

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

    /// <summary>
    /// Hero/focus card: scan summary of data PotScanner already shows (pot counts and the
    /// average water level) with an anti-aliased donut ring gauge around a shape-drawn pot icon.
    /// </summary>
    private void CreateHeroCard(Transform parent)
    {
        // Band container for the hero card. The v0.6.0 fake elevation shadow is gone:
        // the BankApp look is flat (no drop shadows).
        var heroBand = UIFactory.Panel("HeroBand", parent, Color.clear);
        var bandImg = heroBand.GetComponent<Image>();
        if (bandImg != null) bandImg.raycastTarget = false;
        var bandLE = heroBand.AddComponent<LayoutElement>();
        bandLE.minHeight = UITheme.Dp(96f);
        bandLE.preferredHeight = UITheme.Dp(96f);
        bandLE.flexibleHeight = 0f;

        var card = CreateCard("HeroCard", heroBand.transform, CardFill, out var fillImg);

        var overline = UIFactory.Text("HeroOverline", "SCAN SUMMARY", fillImg.transform, UITheme.Sp(12), TextAnchor.MiddleLeft, FontStyle.Bold);
        overline.color = TextMuted;
        overline.raycastTarget = false;
        AnchorRect(overline.rectTransform, 0.05f, 0.76f, 0.70f, 0.94f);

        _heroTotalText = UIFactory.Text("HeroTotal", "0 POTS", fillImg.transform, UITheme.Sp(30), TextAnchor.MiddleLeft, FontStyle.Bold);
        _heroTotalText.color = TextPrimary;
        _heroTotalText.raycastTarget = false;
        _heroTotalText.horizontalOverflow = HorizontalWrapMode.Overflow;
        _heroTotalText.verticalOverflow = VerticalWrapMode.Truncate;
        AnchorRect(_heroTotalText.rectTransform, 0.05f, 0.44f, 0.70f, 0.76f);

        _heroMetaText = UIFactory.Text("HeroMeta", "0 ready \u00b7 0 thirsty \u00b7 0 empty", fillImg.transform, UITheme.Sp(11), TextAnchor.MiddleLeft);
        _heroMetaText.color = TextMuted;
        _heroMetaText.raycastTarget = false;
        AnchorRect(_heroMetaText.rectTransform, 0.05f, 0.24f, 0.72f, 0.42f);

        _heroAvgText = UIFactory.Text("HeroAvg", "AVG WATER 0%", fillImg.transform, UITheme.Sp(11), TextAnchor.MiddleLeft, FontStyle.Bold);
        _heroAvgText.color = TextPrimary;
        _heroAvgText.raycastTarget = false;
        AnchorRect(_heroAvgText.rectTransform, 0.05f, 0.06f, 0.72f, 0.22f);

        // Donut ring gauge (right side): neutral track + green radial fill around a teal pot
        // icon (green = the app's primary accent, teal = value ink, per GamePalette).
        float donutSide = Mathf.Min(UITheme.Dp(96f) * 0.78f, (UITheme.ActualWidth - 2f * UITheme.Dp(8f)) * 0.20f);

        var ringTrack = UIFactory.Panel("HeroRingTrack", fillImg.transform, BarTrack);
        var trackImg = ringTrack.GetComponent<Image>();
        if (trackImg != null)
        {
            trackImg.sprite = UISprites.Donut();
            trackImg.raycastTarget = false;
        }
        var trackRt = ringTrack.GetComponent<RectTransform>();
        trackRt.anchorMin = new Vector2(0.835f, 0.52f);
        trackRt.anchorMax = new Vector2(0.835f, 0.52f);
        trackRt.sizeDelta = new Vector2(donutSide, donutSide);

        var ringFill = UIFactory.Panel("HeroRingFill", fillImg.transform, BarFill);
        _heroRingFill = ringFill.GetComponent<Image>();
        if (_heroRingFill != null)
        {
            _heroRingFill.sprite = UISprites.Donut();
            _heroRingFill.type = Image.Type.Filled;
            _heroRingFill.fillMethod = Image.FillMethod.Radial360;
            _heroRingFill.fillOrigin = (int)Image.Origin360.Top;
            _heroRingFill.fillClockwise = true;
            _heroRingFill.fillAmount = 0f;
            _heroRingFill.raycastTarget = false;
        }
        var fillRingRt = ringFill.GetComponent<RectTransform>();
        fillRingRt.anchorMin = new Vector2(0.835f, 0.52f);
        fillRingRt.anchorMax = new Vector2(0.835f, 0.52f);
        fillRingRt.sizeDelta = new Vector2(donutSide, donutSide);

        // Shape-drawn pot icon in the gauge centre (rim capsule + rounded body).
        float iconSide = donutSide * 0.34f;
        var rim = UIFactory.Panel("HeroIconRim", fillImg.transform, BarFill);
        var rimImg = rim.GetComponent<Image>();
        if (rimImg != null)
        {
            rimImg.sprite = UISprites.Capsule();
            rimImg.type = Image.Type.Sliced;
            rimImg.raycastTarget = false;
        }
        var rimRt = rim.GetComponent<RectTransform>();
        rimRt.anchorMin = new Vector2(0.835f, 0.52f + 0.27f * iconSide / donutSide);
        rimRt.anchorMax = rimRt.anchorMin;
        rimRt.sizeDelta = new Vector2(iconSide, iconSide * 0.22f);

        var body = UIFactory.Panel("HeroIconBody", fillImg.transform, BarFill);
        var bodyImg = body.GetComponent<Image>();
        if (bodyImg != null)
        {
            bodyImg.sprite = UISprites.Rounded(2f);
            bodyImg.type = Image.Type.Sliced;
            bodyImg.raycastTarget = false;
        }
        var bodyRt = body.GetComponent<RectTransform>();
        bodyRt.anchorMin = new Vector2(0.835f, 0.52f - 0.17f * iconSide / donutSide);
        bodyRt.anchorMax = bodyRt.anchorMin;
        bodyRt.sizeDelta = new Vector2(iconSide * 0.78f, iconSide * 0.52f);
    }

    private void CreateFilterToolbar(Transform parent)
    {
        var filterPanel = UIFactory.Panel("FilterToolbar", parent, Color.clear);
        var filterImg = filterPanel.GetComponent<Image>();
        if (filterImg != null) filterImg.raycastTarget = false;
        var filterLE = filterPanel.AddComponent<LayoutElement>();
        filterLE.minHeight = UITheme.Dp(28f);
        filterLE.preferredHeight = UITheme.Dp(28f);
        filterLE.flexibleHeight = 0f;

        var filterHlg = filterPanel.AddComponent<HorizontalLayoutGroup>();
        filterHlg.spacing = UITheme.Dp(8f);
        filterHlg.childControlWidth = true;
        filterHlg.childControlHeight = true;
        filterHlg.childForceExpandWidth = true;
        filterHlg.childForceExpandHeight = true;

        _filterButtons.Clear();

        CreateFilterPill(filterPanel.transform, PotFilter.All, "All");
        CreateFilterPill(filterPanel.transform, PotFilter.Thirsty, "Thirsty");
        CreateFilterPill(filterPanel.transform, PotFilter.Ready, "Ready");
        CreateFilterPill(filterPanel.transform, PotFilter.Empty, "Empty");

        UpdateFilterButtonStyles();
    }

    private void CreateFilterPill(Transform parent, PotFilter filter, string label)
    {
        // Chip, not capsule (v0.6.0 used a full-radius capsule). The INNER fill carries the
        // active colour so the outline stays a constant 1 px frame in both states.
        var chip = CreateCard($"Filter_{filter}", parent, CardFill, out var fillImg,
            raycastTarget: true, radius: ChipCornerRadius);
        var btn = chip.AddComponent<Button>();
        btn.transition = Selectable.Transition.None;

        var txt = UIFactory.Text($"Lbl_{filter}", label, fillImg.transform, UITheme.Sp(11), TextAnchor.MiddleCenter, FontStyle.Bold);
        txt.color = TextMuted;
        txt.raycastTarget = false;
        AnchorRect(txt.rectTransform, 0.06f, 0f, 0.94f, 1f);

        _filterButtons[filter] = (fillImg, txt, btn);

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

            // BankApp precedent: selected = AccentBlue fill with WHITE ink, unselected =
            // neutral surface with muted ink. No alpha grades any more.
            if (isActive)
            {
                txt.color = Color.white;
                img.color = PillActive;
            }
            else
            {
                txt.color = TextMuted;
                img.color = CardFill;
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
            _waterAllSubLabel.text = "...";
            if (_waterAllFill != null) _waterAllFill.color = CardFillSoft;
        }
        if (_autoWaterButton != null)
        {
            _autoWaterButton.interactable = false;
            _autoWaterLabel.text = "Auto-Water";
            _autoWaterSubLabel.text = "...";
            if (_autoWaterFill != null) _autoWaterFill.color = CardFillSoft;
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
            _waterAllSubLabel.text = "...";
            _waterAllButton.transition = Selectable.Transition.None;
            if (_waterAllFill != null) _waterAllFill.color = CardFillSoft;
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

        // Primary action = solid AccentGreen with white ink (BankApp's DEPOSIT button);
        // inert = neutral surface with muted ink. Hierarchy is fill COLOUR now, not alpha.
        _waterAllLabel.color = canEnable ? Color.white : TextMuted;
        _waterAllSubLabel.color = canEnable ? Color.white : TextDim;
        if (_waterAllFill != null)
        {
            _waterAllFill.color = canEnable ? ActionStrong : CardFillSoft;
        }
    }

    private void RefreshAutoWaterButton()
    {
        if (_autoWaterButton == null || _autoWaterLabel == null || _autoWaterSubLabel == null) return;

        if (!_initialScanComplete)
        {
            _autoWaterButton.interactable = false;
            _autoWaterLabel.text = "Auto-Water";
            _autoWaterSubLabel.text = "...";
            _autoWaterButton.transition = Selectable.Transition.None;
            if (_autoWaterFill != null) _autoWaterFill.color = CardFillSoft;
            return;
        }

        bool on = AutoWaterService.IsEnabled;
        _autoWaterLabel.text = "Auto-Water";
        _autoWaterSubLabel.text = on ? "ON" : "OFF";
        _autoWaterButton.transition = Selectable.Transition.None;
        _autoWaterButton.interactable = true;

        // Toggle: ON carries the semantic green on the sublabel (the fill stays neutral so the
        // primary action keeps the only saturated surface in the row).
        _autoWaterLabel.color = on ? TextPrimary : TextMuted;
        _autoWaterSubLabel.color = on ? GrowthAccent : TextDim;
        if (_autoWaterFill != null)
        {
            _autoWaterFill.color = on ? ActionActive : ActionIdle;
        }
    }

    public void RefreshList()
    {
        if (_listContent == null) return;
        _lastRefreshRealtime = Time.realtimeSinceStartup;
        _initialScanComplete = true;

        var pots = PotTracker.Instance.Pots;
        RefreshHero(pots);

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

                bool planted = !string.IsNullOrEmpty(p.PlantName);
                string plantName = planted ? p.PlantName : "Pot";

                refUI.TitleText.text = plantName;
                refUI.TitleText.color = planted ? TextPrimary : TextDim;

                if (refUI.QualityText != null)
                {
                    refUI.QualityText.text = planted ? $"Q: {Mathf.RoundToInt(p.Quality * 100f)}%" : string.Empty;
                    refUI.QualityText.color = TextMuted;
                }

                if (refUI.StatusDot != null)
                {
                    refUI.StatusDot.color = p.IsFullyGrown
                        ? ReadyAccent
                        : planted ? GrowthAccent : EmptyAccent;
                }

                refUI.WaterFillRt.anchorMax = new Vector2(Mathf.Clamp01(p.WaterPercent), 1f);
                if (refUI.WaterBadgeText != null)
                {
                    refUI.WaterBadgeText.text = $"{Mathf.RoundToInt(p.WaterPercent * 100f)}%";
                    refUI.WaterBadgeText.color = p.WaterPercent > 0.01f ? BarFill : TextDim;
                }

                refUI.GrowthFillRt.anchorMax = new Vector2(Mathf.Clamp01(p.GrowthPercent), 1f);
                if (refUI.GrowthFillImage != null)
                    refUI.GrowthFillImage.color = BarFill;
                if (refUI.GrowthBadgeText != null)
                {
                    refUI.GrowthBadgeText.text = $"{Mathf.RoundToInt(p.GrowthPercent * 100f)}%";
                    refUI.GrowthBadgeText.color = p.GrowthPercent > 0.01f ? BarFill : TextDim;
                }

                refUI.SoilFillRt.anchorMax = new Vector2(Mathf.Clamp01(p.SoilPercent), 1f);
                if (refUI.SoilBadgeText != null)
                {
                    refUI.SoilBadgeText.text = p.SoilPercent > 0.01f ? $"{Mathf.RoundToInt(p.SoilPercent * 100f)}%" : "No Soil";
                    refUI.SoilBadgeText.color = p.SoilPercent > 0.01f ? BarFill : TextDim;
                }
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

    /// <summary>
    /// Hero summary refresh (view only): totals, per-state counts and average water level,
    /// all derived from the same PotTracker data the list already renders.
    /// </summary>
    private void RefreshHero(IReadOnlyList<PotInfo> pots)
    {
        int total = 0, ready = 0, thirsty = 0, empty = 0;
        int planted = 0;
        float waterSum = 0f;

        for (int i = 0; i < pots.Count; i++)
        {
            var p = pots[i];
            total++;
            bool hasPlant = !string.IsNullOrEmpty(p.PlantName);
            if (!hasPlant) empty++;
            if (p.IsFullyGrown) ready++;
            if (p.WaterPercent < Constants.WaterAllSkipThreshold && !p.IsFullyGrown && hasPlant) thirsty++;
            if (hasPlant)
            {
                planted++;
                waterSum += Mathf.Clamp01(p.WaterPercent);
            }
        }

        float avgWater = planted > 0 ? waterSum / planted : 0f;

        if (_heroTotalText != null)
        {
            _heroTotalText.text = $"{total} POTS";
            _heroTotalText.color = total > 0 ? TextPrimary : TextDim;
        }
        if (_heroMetaText != null)
        {
            _heroMetaText.text = $"{ready} ready \u00b7 {thirsty} thirsty \u00b7 {empty} empty";
            _heroMetaText.color = total > 0 ? TextMuted : TextDim;
        }
        if (_heroAvgText != null)
        {
            _heroAvgText.text = $"AVG WATER {Mathf.RoundToInt(avgWater * 100f)}%";
            _heroAvgText.color = planted > 0 ? TextPrimary : TextDim;
        }
        if (_heroRingFill != null)
        {
            _heroRingFill.fillAmount = Mathf.Clamp01(avgWater);
            _heroRingFill.color = planted > 0 ? BarFill : TextDim;
        }
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
        var row = CreateCard($"Header_{propKey}", _listContent, CardFill, out var rowImg, raycastTarget: true);

        var le = row.AddComponent<LayoutElement>();
        le.minHeight = UITheme.Dp(32f);
        le.preferredHeight = UITheme.Dp(32f);
        le.flexibleWidth = 1f;
        le.flexibleHeight = 0f;

        var btn = row.AddComponent<Button>();
        btn.transition = Selectable.Transition.ColorTint;
        btn.targetGraphic = rowImg;

        var colors = btn.colors;
        colors.normalColor = CardFill;
        colors.highlightedColor = CardHover;
        colors.pressedColor = CardPressed;
        colors.selectedColor = CardFill;
        btn.colors = colors;

        var content = rowImg.transform;

        // Shape-drawn expand/collapse chevron (two capsule bars; rotates instead of glyph swap).
        var chevronRoot = UIFactory.Panel("Chevron", content, Color.clear);
        var chevronRootImg = chevronRoot.GetComponent<Image>();
        if (chevronRootImg != null) chevronRootImg.raycastTarget = false;
        var chevronRt = chevronRoot.GetComponent<RectTransform>();
        chevronRt.anchorMin = new Vector2(0.022f, 0.5f);
        chevronRt.anchorMax = new Vector2(0.082f, 0.5f);
        chevronRt.sizeDelta = new Vector2(0f, UITheme.Dp(14f));

        var barL = UIFactory.Panel("ChevL", chevronRoot.transform, TextMuted);
        var barLImg = barL.GetComponent<Image>();
        if (barLImg != null)
        {
            barLImg.sprite = UISprites.Capsule();
            barLImg.type = Image.Type.Sliced;
            barLImg.raycastTarget = false;
        }
        var barLRt = barL.GetComponent<RectTransform>();
        barLRt.anchorMin = new Vector2(0.22f, 0.38f);
        barLRt.anchorMax = new Vector2(0.56f, 0.38f);
        barLRt.sizeDelta = new Vector2(0f, UITheme.Dp(2.2f));
        barLRt.localRotation = Quaternion.Euler(0f, 0f, -45f);

        var barR = UIFactory.Panel("ChevR", chevronRoot.transform, TextMuted);
        var barRImg = barR.GetComponent<Image>();
        if (barRImg != null)
        {
            barRImg.sprite = UISprites.Capsule();
            barRImg.type = Image.Type.Sliced;
            barRImg.raycastTarget = false;
        }
        var barRRt = barR.GetComponent<RectTransform>();
        barRRt.anchorMin = new Vector2(0.44f, 0.38f);
        barRRt.anchorMax = new Vector2(0.78f, 0.38f);
        barRRt.sizeDelta = new Vector2(0f, UITheme.Dp(2.2f));
        barRRt.localRotation = Quaternion.Euler(0f, 0f, 45f);

        int total = pots.Count;
        int ready = pots.Count(p => p.IsFullyGrown);
        int dry = pots.Count(p => p.WaterPercent < Constants.WaterAllSkipThreshold && !p.IsFullyGrown && !string.IsNullOrEmpty(p.PlantName));

        string badgeText = ready > 0 ? $"{ready} ready" : dry > 0 ? $"{dry} thirsty" : $"{total} pots";
        // Badge copy carries the state; the text itself stays monochrome (white 60%).
        Color badgeColor = TextMuted;

        var titleText = UIFactory.Text($"Header_{propKey}_Title", propKey,
            content, UITheme.Sp(13), TextAnchor.MiddleLeft, FontStyle.Bold);
        titleText.color = TextPrimary;
        titleText.raycastTarget = false;
        titleText.horizontalOverflow = HorizontalWrapMode.Overflow;
        titleText.verticalOverflow = VerticalWrapMode.Truncate;
        AnchorRect(titleText.rectTransform, 0.10f, 0f, 0.58f, 1f);

        var subText = UIFactory.Text($"Header_{propKey}_Badge", badgeText,
            content, UITheme.Sp(11), TextAnchor.MiddleRight, FontStyle.Bold);
        subText.color = badgeColor;
        subText.raycastTarget = false;
        AnchorRect(subText.rectTransform, 0.58f, 0f, 0.97f, 1f);

        groupRef.HeaderObject = row;
        groupRef.HeaderImage = rowImg;
        groupRef.TitleText = titleText;
        groupRef.BadgeText = subText;
        groupRef.HeaderButton = btn;
        groupRef.ChevronRt = chevronRt;

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
                    badgeColor = TextMuted;
                    break;
                case PotFilter.Ready:
                    badgeText = $"{matchingCount} ready";
                    badgeColor = TextMuted;
                    break;
                case PotFilter.Empty:
                    badgeText = $"{matchingCount} empty";
                    badgeColor = TextMuted;
                    break;
                default: // All
                    if (readyCount > 0)
                    {
                        badgeText = $"{readyCount} ready";
                        badgeColor = TextMuted;
                    }
                    else if (dryCount > 0)
                    {
                        badgeText = $"{dryCount} thirsty";
                        badgeColor = TextMuted;
                    }
                    else
                    {
                        badgeText = $"{totalCount} pots";
                        badgeColor = TextMuted;
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
                    groupRef.TitleText.text = key;

                if (groupRef.HeaderImage != null) groupRef.HeaderImage.color = CardFill;
                if (groupRef.HeaderButton != null)
                {
                    var colors = groupRef.HeaderButton.colors;
                    colors.normalColor = CardFill;
                    colors.highlightedColor = CardHover;
                    colors.pressedColor = CardPressed;
                    colors.selectedColor = CardFill;
                    groupRef.HeaderButton.colors = colors;
                }
                SetChevronExpanded(groupRef.ChevronRt, false);

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
                    groupRef.TitleText.text = key;

                if (groupRef.HeaderImage != null) groupRef.HeaderImage.color = CardExpanded;
                if (groupRef.HeaderButton != null)
                {
                    var colors = groupRef.HeaderButton.colors;
                    colors.normalColor = CardExpanded;
                    colors.highlightedColor = CardHover;
                    colors.pressedColor = CardPressed;
                    colors.selectedColor = CardExpanded;
                    groupRef.HeaderButton.colors = colors;
                }
                SetChevronExpanded(groupRef.ChevronRt, true);

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

    /// <summary>Chevron points down when expanded, right when collapsed (rotates the drawn shape).</summary>
    private static void SetChevronExpanded(RectTransform? chevronRt, bool expanded)
    {
        if (chevronRt == null) return;
        chevronRt.localRotation = Quaternion.Euler(0f, 0f, expanded ? 0f : -90f);
    }

    private GameObject CreatePotRow(PotInfo p)
    {
        bool planted = !string.IsNullOrEmpty(p.PlantName);
        string plantName = planted ? p.PlantName : "Pot";

        var row = CreateCard($"Pot_{p.NativePtr:X}", _listContent, CardFill, out var rowImg, raycastTarget: false);

        var le = row.AddComponent<LayoutElement>();
        le.minHeight = UITheme.Dp(52f);
        le.preferredHeight = UITheme.Dp(52f);
        le.flexibleWidth = 1f;
        le.flexibleHeight = 0f;

        var content = rowImg.transform;

        // Shape-drawn status dot (replaces the old star/arrow/bullet glyphs).
        var statusDot = UIFactory.Panel("StatusDot", content, EmptyAccent);
        var dotImg = statusDot.GetComponent<Image>();
        if (dotImg != null)
        {
            dotImg.sprite = UISprites.Circle();
            dotImg.raycastTarget = false;
            dotImg.color = p.IsFullyGrown ? ReadyAccent : planted ? GrowthAccent : EmptyAccent;
        }
        var dotRt = statusDot.GetComponent<RectTransform>();
        dotRt.anchorMin = new Vector2(0.045f, 0.5f);
        dotRt.anchorMax = new Vector2(0.045f, 0.5f);
        dotRt.sizeDelta = new Vector2(UITheme.Dp(9f), UITheme.Dp(9f));

        var titleText = UIFactory.Text($"Pot_{p.NativePtr:X}_Title", plantName,
            content, UITheme.Sp(12), TextAnchor.MiddleLeft, FontStyle.Bold);
        titleText.color = planted ? TextPrimary : TextDim;
        titleText.raycastTarget = false;
        titleText.horizontalOverflow = HorizontalWrapMode.Wrap;
        titleText.verticalOverflow = VerticalWrapMode.Truncate;
        AnchorRect(titleText.rectTransform, 0.085f, 0.52f, 0.40f, 0.94f);

        Text? qualityText = null;
        if (planted)
        {
            qualityText = UIFactory.Text($"Pot_{p.NativePtr:X}_Quality", $"Q: {Mathf.RoundToInt(p.Quality * 100f)}%",
                content, UITheme.Sp(10), TextAnchor.MiddleLeft);
            qualityText.color = TextMuted;
            qualityText.raycastTarget = false;
            AnchorRect(qualityText.rectTransform, 0.085f, 0.08f, 0.40f, 0.48f);
        }

        string waterPctText = $"{Mathf.RoundToInt(p.WaterPercent * 100f)}%";
        string growthPctText = $"{Mathf.RoundToInt(p.GrowthPercent * 100f)}%";
        string soilPctText = p.SoilPercent > 0.01f ? $"{Mathf.RoundToInt(p.SoilPercent * 100f)}%" : "No Soil";

        // Dynamic click listener: always attach single-pot watering handler
        IntPtr ptr = p.NativePtr;
        Action onWaterClick = () => WaterAllService.WaterSinglePot(ptr);

        // 1. Water (Top) — white 65% fill (monochrome bars)
        var (wFillRt, wFillImg, wBadge) = AddProgressBar(content, "W", p.WaterPercent, BarFill, 0.68f, 0.26f, onWaterClick, waterPctText);
        // 2. Growth (Middle)
        var (gFillRt, gFillImg, gBadge) = AddProgressBar(content, "G", p.GrowthPercent, BarFill, 0.36f, 0.26f, null, growthPctText);
        // 3. Soil (Bottom)
        var (sFillRt, sFillImg, sBadge) = AddProgressBar(content, "S", p.SoilPercent, BarFill, 0.04f, 0.26f, null, soilPctText);

        if (wBadge != null) wBadge.color = p.WaterPercent > 0.01f ? BarFill : TextDim;
        if (gBadge != null) gBadge.color = p.GrowthPercent > 0.01f ? BarFill : TextDim;
        if (sBadge != null) sBadge.color = p.SoilPercent > 0.01f ? BarFill : TextDim;

        _rowCache[p.NativePtr] = new PotRowUIRef
        {
            PotInfo = p,
            RowObject = row,
            TitleText = titleText,
            QualityText = qualityText,
            StatusDot = dotImg,
            WaterFillRt = wFillRt,
            WaterFillImage = wFillImg,
            WaterBadgeText = wBadge,
            GrowthFillRt = gFillRt,
            GrowthFillImage = gFillImg,
            GrowthBadgeText = gBadge,
            SoilFillRt = sFillRt,
            SoilFillImage = sFillImg,
            SoilBadgeText = sBadge
        };

        return row;
    }

    /// <summary>
    /// Capsule progress bar row segment: metric letter, Dp(7.5) capsule track in the border tone
    /// with a teal fill, and the value number right-aligned in teal (BankApp value ink). The band
    /// [anchorYOffset, anchorYOffset + barHeight] is a fraction of the row height; the bar is
    /// centered inside it so adjacent bands can never overlap.
    /// </summary>
    private (RectTransform fillRt, Image? fillImg, Text? badgeText) AddProgressBar(Transform parent, string label, float percent, Color fillCol, float anchorYOffset, float barHeight = 0.26f, Action? onClick = null, string? badgeText = null)
    {
        float bandMid = anchorYOffset + barHeight * 0.5f;

        var lbl = UIFactory.Text("Lbl", label, parent, UITheme.Sp(9), TextAnchor.MiddleRight, FontStyle.Bold);
        lbl.color = LabelDim;
        lbl.raycastTarget = false;
        AnchorRect(lbl.rectTransform, 0.40f, anchorYOffset, 0.455f, anchorYOffset + barHeight);

        if (onClick != null)
        {
            // Clickable affordance: blue capsule ring around the water track (blue = interactive).
            var border = UIFactory.Panel("Border", parent, ClickRing);
            var borderImg = border.GetComponent<Image>();
            if (borderImg != null)
            {
                borderImg.sprite = UISprites.Capsule();
                borderImg.type = Image.Type.Sliced;
                borderImg.raycastTarget = false;
            }
            var borderRt = border.GetComponent<RectTransform>();
            borderRt.anchorMin = new Vector2(0.458f, bandMid);
            borderRt.anchorMax = new Vector2(0.862f, bandMid);
            borderRt.sizeDelta = new Vector2(0f, UITheme.Dp(11f));
        }

        var track = UIFactory.Panel("Track", parent, BarTrack);
        var trackImg = track.GetComponent<Image>();
        if (trackImg != null)
        {
            trackImg.sprite = UISprites.Capsule();
            trackImg.type = Image.Type.Sliced;
            trackImg.raycastTarget = false;
        }
        var trackRt = track.GetComponent<RectTransform>();
        trackRt.anchorMin = new Vector2(0.465f, bandMid);
        trackRt.anchorMax = new Vector2(0.855f, bandMid);
        trackRt.sizeDelta = new Vector2(0f, UITheme.Dp(7.5f));

        var fill = UIFactory.Panel("Fill", track.transform, fillCol, fullAnchor: true);
        var fillImage = fill.GetComponent<Image>();
        if (fillImage != null)
        {
            fillImage.sprite = UISprites.Capsule();
            fillImage.type = Image.Type.Sliced;
            fillImage.raycastTarget = false;
        }

        var fillRt = fill.GetComponent<RectTransform>();
        fillRt.anchorMin = new Vector2(0, 0);
        fillRt.anchorMax = new Vector2(Mathf.Clamp01(percent), 1);
        fillRt.offsetMin = Vector2.zero; fillRt.offsetMax = Vector2.zero;

        Text? badge = null;
        if (!string.IsNullOrEmpty(badgeText))
        {
            // Value number right-aligned in its own column (never overlaid on the bar).
            badge = UIFactory.Text("Badge", badgeText, parent, UITheme.Sp(10), TextAnchor.MiddleRight, FontStyle.Bold);
            badge.color = BarFill;
            badge.raycastTarget = false;
            AnchorRect(badge.rectTransform, 0.87f, anchorYOffset, 0.985f, anchorYOffset + barHeight);
        }

        if (onClick != null)
        {
            if (trackImg != null) trackImg.raycastTarget = true;

            var btn = track.AddComponent<Button>();
            btn.transition = Selectable.Transition.None;
            ButtonUtils.AddListener(btn, onClick);
        }

        return (fillRt, fillImage, badge);
    }

    private void OnPotsScannedHandler()
    {
        if (!IsAlive(_mainBG)) return;
        if (IsOpen())
        {
            RefreshList();
            RefreshWaterAllButton();
            RefreshAutoWaterButton();
        }
    }

    /// <summary>IL2CPP liveness: managed wrappers survive scene unload while native objects are dead.</summary>
    private static bool IsAlive(UnityEngine.Object? obj)
    {
        if (obj == null) return false;
        try { return obj.Pointer != IntPtr.Zero && !obj.WasCollected && (UnityEngine.Object)obj != null; }
        catch { return false; }
    }

    protected override void OnPhoneClosed()
    {
        if (IsAlive(_mainBG)) _mainBG.SetActive(false);
        // Update + OnPotsScanned stay subscribed for the app's lifetime (defensive unsubscribe/subscribe in OnCreated).
    }

    private void Update()
    {
        bool open = IsOpen();
        if (IsAlive(_mainBG) && _mainBG.activeSelf != open)
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

    // ============================ View helpers ============================
    // Sprite generation lives in the shared kit (S1Mods.Shared.UISprites: Rounded / Capsule /
    // Circle / Donut). Both PotScannerApp and WeatherApp carried their own near-identical
    // signed-distance rasteriser; that duplication is gone as of v0.7.0.

    /// <summary>
    /// Two-layer card: outer rounded rect in <see cref="GamePalette.Border"/> with an inner fill
    /// inset by 1 Dp — i.e. a 1 px outline around an opaque surface. In v0.6.0 the same helper
    /// produced the white-alpha "glass" look (white 12% border / white 6% fill); both layers are
    /// now opaque and the colour comes from the caller.
    /// Returns the outer card GameObject; content parents to <paramref name="fillImage"/>.transform.
    /// Non-interactive graphics keep raycastTarget=false unless the card itself is a control.
    /// </summary>
    private static GameObject CreateCard(string name, Transform parent, Color fill, out Image fillImage,
        bool raycastTarget = false, float radius = CardCornerRadius)
    {
        var card = UIFactory.Panel(name, parent, CardBorderColor, fullAnchor: true);
        var borderImg = card.GetComponent<Image>();
        if (borderImg != null)
        {
            borderImg.sprite = UISprites.Rounded(radius);
            borderImg.type = Image.Type.Sliced;
            borderImg.raycastTarget = raycastTarget;
        }

        var fillGo = UIFactory.Panel("Fill", card.transform, fill, fullAnchor: true);
        var fillRt = fillGo.GetComponent<RectTransform>();
        float inset = UITheme.Dp(1f);
        fillRt.offsetMin = new Vector2(inset, inset);
        fillRt.offsetMax = new Vector2(-inset, -inset);

        fillImage = fillGo.GetComponent<Image>()!;
        fillImage.sprite = UISprites.Rounded(radius - 1f);
        fillImage.type = Image.Type.Sliced;
        fillImage.raycastTarget = false;

        return card;
    }

    /// <summary>Stretches an anchored rect over an anchor band (fractions of the parent).</summary>
    private static void AnchorRect(RectTransform rt, float xMin, float yMin, float xMax, float yMax)
    {
        rt.anchorMin = new Vector2(xMin, yMin);
        rt.anchorMax = new Vector2(xMax, yMax);
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;
    }
}
