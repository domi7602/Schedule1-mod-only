using System;
using MelonLoader;
using S1API.PhoneApp;
using S1API.UI;
using S1API.Weather;
using S1Mods.Shared;
using UnityEngine;
using UnityEngine.UI;
using UITheme = S1Mods.Shared.UITheme;

namespace Weather;

/// <summary>
/// Read-only status dashboard for the live in-game weather (S1API.Weather.WeatherManager).
/// Shows the dominant condition (largest weight) prominently plus all nine condition
/// components with progress bars. Pure view — no persistence, no gameplay influence.
///
/// Visual design (0.3.0, minimal dark/glass): neutral charcoal canvas with a plain dark
/// glass hero card (white 6% fill + white 12% rim, radius 12) holding the overline, the
/// dominant condition name/percentage, a dark glass intensity pill and the radial ring
/// gauge around the shape-drawn condition icon (no glyphs — legacy Arial Text cannot
/// render emoji). Type hierarchy is white/alpha only; the single splash of colour on the
/// whole screen is the ring-gauge arc (dominant condition colour at ~62% alpha). Nine
/// glass condition rows with white shape icons, white bar fills and white percentages.
/// No footer/status row — the redesign deliberately omits the old LIVE indicator and
/// version badge.
/// </summary>
public sealed class WeatherApp : PhoneApp
{
    // --- Relative Layout Anchors (fractions of the phone canvas) ---
    private const float HeaderHeight = 0.10f;
    private const float HeroTop = 0.885f;
    private const float HeroBottom = 0.595f;
    private const float SectionRowTop = 0.582f;
    private const float SectionRowBottom = 0.552f;
    private const float ListTop = 0.545f;
    private const float ListBottom = 0.012f; // extended into the removed footer band
    private const float RowSpacing = 5f;

    // --- Hero card child bands (fractions of the hero card; x right, y up) ---
    // Layout math — overlap-proof by construction (the 0.2.0 review caught a pct-vs-icon
    // collision, so every neighbouring span keeps an explicit clear gap). At the 400x750
    // reference canvas the hero card is 368x218 px, so a 0.02 gap is already ~4 px of
    // clear space and spans never intersect:
    //   y spans, left text column (x 0.055-0.660):
    //     overline   y 0.84-0.97  (Sp13 label)           gap 0.03 v
    //     hero name  y 0.55-0.81  (Sp38 bold uppercase)  gap 0.03 v
    //     hero pct   y 0.25-0.52  (Sp40 bold)            gap 0.04 v
    //     chip/meta  y 0.06-0.21  (glass pill, Dp24 tall)
    //   x spans (non-intersecting wherever the y spans intersect):
    //     overline/name/pct x 0.055-0.660 (text pad starts 0.055 clear of the card rim;
    //                   widest name "STORMY" ~150 of 223 px at Sp38 and
    //                   widest pct "100%" ~102 px, both left-aligned — no overflow into the
    //                   right-hand ring band)
    //     chip pill     x 0.055-0.360 | meta label x 0.380-0.660 (gap 0.020)
    //     ring gauge    x 0.710-0.970 — box centred at HeroRingCenterX with side
    //                   min(0.60*heroH, 0.26*heroW), so the box can never leave x 0.71-0.97
    //                   nor y 0.26-0.86; the icon inside is <= 0.56*ringSide, keeping even
    //                   the widest shape geometry (sun rays reach ~0.22 of ringSide from
    //                   centre) inside the ring bore (inner radius 0.40*ringSide).
    //     -> the text column ends at x 0.660 and the ring box starts at x 0.710: 0.05 of
    //        clear space replaces the old pct-vs-icon collision zone.
    private const float HeroPadL = 0.055f;
    private const float HeroColRight = 0.66f;
    private const float HeroChipRight = 0.36f;
    private const float HeroMetaLeft = 0.38f;
    private const float HeroRingCenterX = 0.84f;
    private const float HeroRingCenterY = 0.56f;

    // --- Component table (order matches WeatherState / the nine native components) ---
    private const int ComponentCount = 9;
    private const float ActiveEpsilon = 0.0005f;

    private static readonly string[] ComponentNames =
    {
        "Sunny", "Cloudy", "Rainy", "Stormy", "Snowy", "Foggy", "Windy", "Hail", "Sleet"
    };

    // --- Condition colours (semantic only): the ring-gauge arc is the single place in the
    // whole app where a condition colour is allowed to show (at reduced alpha). Every
    // other surface uses the monochrome white/alpha tokens below.
    private static readonly Color[] ComponentColors =
    {
        new(1.00f, 0.83f, 0.35f, 1f), // Sunny  — warm yellow
        new(0.76f, 0.79f, 0.84f, 1f), // Cloudy — light grey
        new(0.35f, 0.60f, 0.96f, 1f), // Rainy  — blue
        new(0.63f, 0.42f, 0.96f, 1f), // Stormy — violet
        new(0.95f, 0.97f, 1.00f, 1f), // Snowy  — white
        new(0.62f, 0.64f, 0.68f, 1f), // Foggy  — grey
        new(0.33f, 0.84f, 0.80f, 1f), // Windy  — turquoise
        new(0.55f, 0.80f, 1.00f, 1f), // Hail   — light blue
        new(0.38f, 0.90f, 0.92f, 1f)  // Sleet  — cyan
    };

    // --- UI Color Palette (neutral charcoal panels — no dominant-colour tint anywhere) ---
    private static readonly Color BgColor = new(0.07f, 0.08f, 0.11f, 1f);
    private static readonly Color HeaderBgColor = new(0.09f, 0.10f, 0.14f, 1f);
    private static readonly Color CardBgColor = new(0.115f, 0.125f, 0.17f, 1f);
    private static readonly Color BarBgColor = new(1f, 1f, 1f, 0.08f);
    private static readonly Color DividerColor = new(1f, 1f, 1f, 0.08f);
    private static readonly Color SubtitleColor = new(1f, 1f, 1f, 0.55f);

    // --- Glass palette (white-alpha fills + rims over the neutral background) ---
    private static readonly Color GlassFillColor = new(1f, 1f, 1f, 0.06f);
    private static readonly Color GlassBorderColor = new(1f, 1f, 1f, 0.12f);
    private static readonly Color ChipGlassColor = new(1f, 1f, 1f, 0.10f);
    private static readonly Color RingTrackColor = new(1f, 1f, 1f, 0.10f);
    private static readonly Color ShadowColor = new(0f, 0f, 0f, 0.25f);
    private static readonly Color OverlineColor = new(1f, 1f, 1f, 0.48f);   // labels / overline
    private static readonly Color MetaColor = new(1f, 1f, 1f, 0.58f);        // secondary meta lines
    private static readonly Color HeaderTickColor = new(1f, 1f, 1f, 0.50f);  // header tick (mono)
    private static readonly Color BarFillColor = new(1f, 1f, 1f, 0.65f);     // row bar fills
    private static readonly Color RowIconColor = new(1f, 1f, 1f, 0.70f);     // row shape icons
    private static readonly Color PctColor = new(1f, 1f, 1f, 0.85f);         // row percentages
    private static readonly Color ChipTextColor = new(1f, 1f, 1f, 0.90f);    // intensity chip text
    private const float RingFillAlpha = 0.62f; // ring gauge arc (only colour on screen)
    private const float RowDimAlpha = 0.38f;   // zero-weight rows (CanvasGroup alpha)

    // --- Shape-icon palette (flat white alpha ramps; drawn from Images — legacy Arial
    // cannot render glyphs, so every icon stays pure geometry, monochrome like the rest) ---
    private static readonly Color SunCoreColor = new(1f, 1f, 1f, 0.95f);
    private static readonly Color SunRayColor = new(1f, 1f, 1f, 0.70f);
    private static readonly Color CloudLightColor = new(1f, 1f, 1f, 0.85f);
    private static readonly Color CloudDarkColor = new(1f, 1f, 1f, 0.45f);
    private static readonly Color RainDropColor = new(1f, 1f, 1f, 0.75f);
    private static readonly Color BoltColor = new(1f, 1f, 1f, 0.95f);
    private static readonly Color SnowDotColor = new(1f, 1f, 1f, 0.90f);
    private static readonly Color FogBarColor = new(1f, 1f, 1f, 0.55f);
    private static readonly Color WindBarColor = new(1f, 1f, 1f, 0.75f);
    private static readonly Color HailDotColor = new(1f, 1f, 1f, 0.85f);
    private static readonly Color SleetDropColor = new(1f, 1f, 1f, 0.75f);

    // --- UI References ---
    private GameObject _mainBG = null!;
    private Image _mainBgImage = null!;
    private GameObject _contentRoot = null!;
    private GameObject _emptyRoot = null!;
    private Text _heroName = null!;
    private Text _heroPct = null!;
    private Text _heroState = null!;
    private Image _ringFill = null!;
    private GameObject _heroIconArea = null!;
    private readonly GameObject[] _heroIconRoots = new GameObject[ComponentCount];
    private Text _activeLabel = null!;
    private readonly Text[] _rowName = new Text[ComponentCount];
    private readonly Text[] _rowPct = new Text[ComponentCount];
    private readonly Image[] _rowFill = new Image[ComponentCount];
    private readonly RectTransform[] _rowFillRt = new RectTransform[ComponentCount];
    private readonly CanvasGroup[] _rowGroup = new CanvasGroup[ComponentCount];

    // --- Generated sprite cache (created once, reused by every card/bar/shape) ---
    private static Sprite? _roundedSprite;
    private static Sprite? _capsuleSprite;
    private static Sprite? _circleSprite;
    private static Sprite? _glassSprite;
    private static Sprite? _donutSprite;

    // --- Runtime state ---
    // One app instance is discovered per Main-scene load. Static handlers are subscribed exactly
    // once and always dispatch through _active, so a scene reload never stacks subscribers.
    private static WeatherApp? _active;
    private static bool _staticSubscribed;

    private readonly float[] _values = new float[ComponentCount];
    private WeatherState? _state;
    private bool _dirty;

    protected override string AppName => "Weather";
    protected override string AppTitle => "Weather";
    protected override string IconLabel => "Weather";
    protected override string IconFileName => "weather_icon.png";
    protected override EOrientation Orientation => EOrientation.Vertical;

    // =====================================================================
    // Lifecycle
    // =====================================================================

    protected override void OnCreated()
    {
        base.OnCreated();
        _active = this;

        if (_staticSubscribed) return;
        // Defensive Unsubscribe-before-Subscribe: idempotent across scene reloads.
        MelonEvents.OnUpdate.Unsubscribe(DispatchUpdate);
        MelonEvents.OnUpdate.Subscribe(DispatchUpdate);
        // Only latch the guard once the update loop is actually wired, so a failed
        // Subscribe can be retried on the next OnCreated instead of leaving the app blank.
        _staticSubscribed = true;

        try
        {
            WeatherManager.OnWeatherChanged -= DispatchWeatherChanged;
            WeatherManager.OnWeatherChanged += DispatchWeatherChanged;
        }
        catch (Exception ex)
        {
            MelonLogger.Warning($"[{AppName}] Could not subscribe to WeatherManager.OnWeatherChanged: {ex.Message}");
        }

        MelonLogger.Msg($"[{AppName}] Registered with S1API PhoneApp system.");
    }

    /// <summary>Called from Mod.OnSceneWasUnloaded so a torn-down scene does not keep a dead instance alive.</summary>
    internal static void TearDownForSceneUnload() => _active = null;

    private static void DispatchUpdate()
    {
        var app = _active;
        if (app == null) return;
        try { app.Update(); } catch { }
    }

    private static void DispatchWeatherChanged(WeatherState state)
    {
        var app = _active;
        if (app == null) return;
        try
        {
            // Closed app: cache only (rendered on next open). Open app: re-render immediately.
            app._state = state;
            app._dirty = true;
            if (app.IsOpen()) app.RenderInternal();
        }
        catch { }
    }

    protected override void OnPhoneClosed()
    {
        base.OnPhoneClosed();
        if (_mainBG != null) _mainBG.SetActive(false);
        // Rule 2/10: never destroy UI elements here, never unsubscribe MelonEvents/statically
        // subscribed game events here (OnCreated fires only once per scene — the app would stay blank).
    }

    private void Update()
    {
        bool open = IsOpen();
        if (NetworkGuard.IsAlive(_mainBG) && _mainBG.activeSelf != open)
        {
            _mainBG.SetActive(open);
            if (open) OnAppOpened();
        }

        if (!open) return;

        // Cheap per-frame poll of the immutable snapshot: covers a weather change that happened
        // before this instance subscribed or an edge case where the change event was missed.
        WeatherState? current = SafeCurrent();
        if (_dirty || !SameState(current, _state))
        {
            _state = current;
            RenderInternal();
        }

        if (Input.GetKeyDown(KeyCode.Escape)) CloseApp();
    }

    private void OnAppOpened()
    {
        _state = SafeCurrent();
        RenderInternal();
    }

    private static WeatherState? SafeCurrent()
    {
        try { return WeatherManager.Current; }
        catch { return null; }
    }

    private static bool SameState(WeatherState? a, WeatherState? b)
    {
        if (!a.HasValue || !b.HasValue) return !a.HasValue && !b.HasValue;
        return a.GetValueOrDefault() == b.GetValueOrDefault();
    }

    // =====================================================================
    // UI Construction
    // =====================================================================

    protected override void OnCreatedUI(GameObject container)
    {
        var containerRt = container.GetComponent<RectTransform>();
        if (containerRt != null)
        {
            UITheme.InitializeForTextApp(containerRt);
            MelonLogger.Msg($"[{AppName}] Canvas initialized: {UITheme.ActualWidth:F0}x{UITheme.ActualHeight:F0} (Scale={UITheme.Scale:F2}).");
        }

        // Isolated background panel (Rule 3): full-anchored, starts hidden.
        _mainBG = UIFactory.Panel("Background", container.transform, BgColor, fullAnchor: true);
        _mainBgImage = _mainBG.GetComponent<Image>();
        _mainBG.SetActive(false);

        BuildHeader(_mainBG.transform);
        BuildContent(_mainBG.transform);
        BuildEmptyState(_mainBG.transform);

        RenderInternal();
    }

    private void BuildHeader(Transform parent)
    {
        var headerBar = UIFactory.Panel("HeaderBackground", parent, HeaderBgColor);
        var hbRt = headerBar.GetComponent<RectTransform>();
        hbRt.anchorMin = new Vector2(0f, 1f - HeaderHeight);
        hbRt.anchorMax = new Vector2(1f, 1f);
        hbRt.offsetMin = Vector2.zero;
        hbRt.offsetMax = Vector2.zero;
        headerBar.GetComponent<Image>().raycastTarget = false;

        // Short monochrome tick left of the title (white 50% capsule — no accent colour).
        var headerAccent = UIFactory.Panel("HeaderTick", headerBar.transform, HeaderTickColor).GetComponent<Image>();
        headerAccent.sprite = GetCapsuleSprite();
        headerAccent.type = Image.Type.Sliced;
        headerAccent.raycastTarget = false;
        var haRt = headerAccent.rectTransform;
        haRt.anchorMin = new Vector2(0.055f, 0.28f);
        haRt.anchorMax = new Vector2(0.055f, 0.72f);
        haRt.offsetMin = new Vector2(-UITheme.Dp(3f), 0f);
        haRt.offsetMax = new Vector2(UITheme.Dp(3f), 0f);

        var title = UIFactory.Text("Title", "WEATHER", headerBar.transform, UITheme.Sp(30), TextAnchor.MiddleLeft, FontStyle.Bold);
        var titleRt = title.rectTransform;
        titleRt.anchorMin = new Vector2(0.075f, 0f);
        titleRt.anchorMax = new Vector2(0.70f, 1f);
        titleRt.offsetMin = Vector2.zero;
        titleRt.offsetMax = Vector2.zero;
        title.horizontalOverflow = HorizontalWrapMode.Overflow;
        title.verticalOverflow = VerticalWrapMode.Truncate;
        title.raycastTarget = false;

        var subtitle = UIFactory.Text("HeaderSubtitle", "LIVE CONDITIONS", headerBar.transform, UITheme.Sp(13), TextAnchor.MiddleRight, FontStyle.Normal);
        var subRt = subtitle.rectTransform;
        subRt.anchorMin = new Vector2(0.55f, 0f);
        subRt.anchorMax = new Vector2(0.945f, 1f);
        subRt.offsetMin = Vector2.zero;
        subRt.offsetMax = Vector2.zero;
        subtitle.color = OverlineColor;
        subtitle.horizontalOverflow = HorizontalWrapMode.Overflow;
        subtitle.verticalOverflow = VerticalWrapMode.Truncate;
        subtitle.raycastTarget = false;

        var headerLine = UIFactory.Panel("HeaderLine", parent, DividerColor);
        var hlRt = headerLine.GetComponent<RectTransform>();
        hlRt.anchorMin = new Vector2(0.04f, 1f - HeaderHeight);
        hlRt.anchorMax = new Vector2(0.96f, 1f - HeaderHeight);
        hlRt.offsetMin = Vector2.zero;
        hlRt.offsetMax = new Vector2(0f, UITheme.Dp(1.5f));
        headerLine.GetComponent<Image>().raycastTarget = false;
    }

    private void BuildContent(Transform parent)
    {
        _contentRoot = new GameObject("Content");
        _contentRoot.transform.SetParent(parent, false);
        var rootRt = _contentRoot.AddComponent<RectTransform>();
        rootRt.anchorMin = Vector2.zero;
        rootRt.anchorMax = Vector2.one;
        rootRt.offsetMin = Vector2.zero;
        rootRt.offsetMax = Vector2.zero;

        BuildHero(_contentRoot.transform);
        BuildSectionRow(_contentRoot.transform);
        BuildConditionList(_contentRoot.transform);
    }

    private void BuildHero(Transform parent)
    {
        // Child spans are defined by the "Hero card child bands" comment block above;
        // this method implements exactly those spans (no element leaves its band).
        float heroW = 0.92f * UITheme.ActualWidth;                   // hero card pixel width  (0.96-0.04)
        float heroH = (HeroTop - HeroBottom) * UITheme.ActualHeight;  // hero card pixel height (0.29)

        // Fake elevation: a slightly larger dark rounded rect peeking out behind the card.
        var shadow = UIFactory.Panel("HeroShadow", parent, ShadowColor);
        var shadowImg = shadow.GetComponent<Image>();
        shadowImg.sprite = GetRoundedSprite();
        shadowImg.type = Image.Type.Sliced;
        shadowImg.raycastTarget = false;
        var shRt = shadow.GetComponent<RectTransform>();
        shRt.anchorMin = new Vector2(0.04f, HeroBottom);
        shRt.anchorMax = new Vector2(0.96f, HeroTop);
        shRt.offsetMin = new Vector2(-UITheme.Dp(3f), -UITheme.Dp(3f));
        shRt.offsetMax = new Vector2(UITheme.Dp(3f), UITheme.Dp(3f));

        // Glass hero card: outer rounded rim in the border colour, plain dark base
        // inset 2 Dp (two-layer technique => a thin light rim at any size). No gradient
        // sprite, no glow — the fill is a single dark tone for the minimal look.
        var hero = new GameObject("HeroCard");
        hero.transform.SetParent(parent, false);
        var heroRt = hero.AddComponent<RectTransform>();
        heroRt.anchorMin = new Vector2(0.04f, HeroBottom);
        heroRt.anchorMax = new Vector2(0.96f, HeroTop);
        heroRt.offsetMin = Vector2.zero;
        heroRt.offsetMax = Vector2.zero;

        var rim = UIFactory.Panel("Rim", hero.transform, GlassBorderColor, fullAnchor: true);
        var rimImg = rim.GetComponent<Image>();
        rimImg.sprite = GetRoundedSprite();
        rimImg.type = Image.Type.Sliced;
        rimImg.raycastTarget = false;

        var basePanel = UIFactory.Panel("Base", hero.transform, CardBgColor, fullAnchor: true);
        var baseRt = basePanel.GetComponent<RectTransform>();
        baseRt.offsetMin = new Vector2(UITheme.Dp(2f), UITheme.Dp(2f));
        baseRt.offsetMax = new Vector2(-UITheme.Dp(2f), -UITheme.Dp(2f));
        var baseImg = basePanel.GetComponent<Image>();
        baseImg.sprite = GetRoundedSprite();
        baseImg.type = Image.Type.Sliced;
        baseImg.raycastTarget = false;

        // --- Ring gauge (signature element): donut track + radial fill around the icon. ---
        _heroIconArea = new GameObject("IconArea");
        _heroIconArea.transform.SetParent(hero.transform, false);
        var iconRt = _heroIconArea.AddComponent<RectTransform>();
        float ringSide = Mathf.Min(heroH * 0.60f, heroW * 0.26f); // box stays within x 0.71-0.97 / y 0.26-0.86
        float iconSide = ringSide * 0.56f;                        // shapes stay inside the ring bore
        iconRt.anchorMin = new Vector2(HeroRingCenterX, HeroRingCenterY);
        iconRt.anchorMax = new Vector2(HeroRingCenterX, HeroRingCenterY);
        iconRt.sizeDelta = new Vector2(ringSide, ringSide);

        var ringTrack = UIFactory.Panel("RingTrack", _heroIconArea.transform, RingTrackColor, fullAnchor: true);
        var trackImg = ringTrack.GetComponent<Image>();
        trackImg.sprite = GetDonutSprite();
        trackImg.raycastTarget = false;

        var ringFill = UIFactory.Panel("RingFill", _heroIconArea.transform, RingTrackColor, fullAnchor: true);
        _ringFill = ringFill.GetComponent<Image>();
        _ringFill.sprite = GetDonutSprite();
        _ringFill.type = Image.Type.Filled;
        _ringFill.fillMethod = Image.FillMethod.Radial360;
        _ringFill.fillOrigin = (int)Image.Origin360.Top;
        _ringFill.fillClockwise = true;
        _ringFill.fillAmount = 0f;
        _ringFill.raycastTarget = false;

        // Shape-drawn condition icons (sun disc, cloud, drops, ...) centred inside the ring.
        for (int i = 0; i < ComponentCount; i++)
        {
            var root = new GameObject("Icon_" + ComponentNames[i]);
            root.transform.SetParent(_heroIconArea.transform, false);
            var rootRt = root.AddComponent<RectTransform>();
            rootRt.anchorMin = new Vector2(0.5f, 0.5f);
            rootRt.anchorMax = new Vector2(0.5f, 0.5f);
            rootRt.sizeDelta = new Vector2(iconSide, iconSide);
            BuildConditionIcon(root.transform, i, iconSide);
            root.SetActive(false);
            _heroIconRoots[i] = root;
        }

        // Overline label (y 0.84-0.97).
        var overline = UIFactory.Text("Overline", "DOMINANT CONDITION", hero.transform, UITheme.Sp(13), TextAnchor.MiddleLeft, FontStyle.Bold);
        var ovRt = overline.rectTransform;
        ovRt.anchorMin = new Vector2(HeroPadL, 0.84f);
        ovRt.anchorMax = new Vector2(HeroColRight, 0.97f);
        ovRt.offsetMin = Vector2.zero;
        ovRt.offsetMax = Vector2.zero;
        overline.color = OverlineColor;
        overline.horizontalOverflow = HorizontalWrapMode.Overflow;
        overline.verticalOverflow = VerticalWrapMode.Truncate;
        overline.raycastTarget = false;

        // Big condition name (y 0.55-0.81) and big percentage (y 0.25-0.52) stacked in the
        // left column — side by side they cannot both fit their spans with clear gaps.
        _heroName = UIFactory.Text("DominantName", "", hero.transform, UITheme.Sp(38), TextAnchor.MiddleLeft, FontStyle.Bold);
        var nameRt = _heroName.rectTransform;
        nameRt.anchorMin = new Vector2(HeroPadL, 0.55f);
        nameRt.anchorMax = new Vector2(HeroColRight, 0.81f);
        nameRt.offsetMin = Vector2.zero;
        nameRt.offsetMax = Vector2.zero;
        _heroName.horizontalOverflow = HorizontalWrapMode.Overflow;
        _heroName.verticalOverflow = VerticalWrapMode.Truncate;
        _heroName.raycastTarget = false;

        _heroPct = UIFactory.Text("DominantPercent", "", hero.transform, UITheme.Sp(40), TextAnchor.MiddleLeft, FontStyle.Bold);
        var pctRt = _heroPct.rectTransform;
        pctRt.anchorMin = new Vector2(HeroPadL, 0.25f);
        pctRt.anchorMax = new Vector2(HeroColRight, 0.52f);
        pctRt.offsetMin = Vector2.zero;
        pctRt.offsetMax = Vector2.zero;
        _heroPct.horizontalOverflow = HorizontalWrapMode.Overflow;
        _heroPct.verticalOverflow = VerticalWrapMode.Truncate;
        _heroPct.raycastTarget = false;

        // Intensity chip as a glass pill (x 0.055-0.360, y band 0.06-0.21).
        var chipBg = UIFactory.Panel("IntensityChip", hero.transform, ChipGlassColor);
        chipBg.GetComponent<Image>().sprite = GetCapsuleSprite();
        chipBg.GetComponent<Image>().type = Image.Type.Sliced;
        chipBg.GetComponent<Image>().raycastTarget = false;
        var chipRt = chipBg.GetComponent<RectTransform>();
        chipRt.anchorMin = new Vector2(HeroPadL, 0.135f);
        chipRt.anchorMax = new Vector2(HeroChipRight, 0.135f);
        chipRt.offsetMin = new Vector2(0f, -UITheme.Dp(12f));
        chipRt.offsetMax = new Vector2(0f, UITheme.Dp(12f));

        _heroState = UIFactory.Text("Intensity", "", chipBg.transform, UITheme.Sp(15), TextAnchor.MiddleCenter, FontStyle.Bold);
        var stateRt = _heroState.rectTransform;
        stateRt.anchorMin = Vector2.zero;
        stateRt.anchorMax = Vector2.one;
        stateRt.offsetMin = Vector2.zero;
        stateRt.offsetMax = Vector2.zero;
        _heroState.horizontalOverflow = HorizontalWrapMode.Overflow;
        _heroState.verticalOverflow = VerticalWrapMode.Truncate;
        _heroState.raycastTarget = false;

        // Meta info right of the chip ("N OF 9 ACTIVE", x 0.38-0.66).
        _activeLabel = UIFactory.Text("ActiveCount", "", hero.transform, UITheme.Sp(13), TextAnchor.MiddleRight, FontStyle.Normal);
        var aRt = _activeLabel.rectTransform;
        aRt.anchorMin = new Vector2(HeroMetaLeft, 0.06f);
        aRt.anchorMax = new Vector2(HeroColRight, 0.21f);
        aRt.offsetMin = Vector2.zero;
        aRt.offsetMax = Vector2.zero;
        _activeLabel.color = MetaColor;
        _activeLabel.horizontalOverflow = HorizontalWrapMode.Overflow;
        _activeLabel.verticalOverflow = VerticalWrapMode.Truncate;
        _activeLabel.raycastTarget = false;
    }

    /// <summary>
    /// Builds one shape-drawn icon per condition from plain Image rectangles and circles
    /// (legacy UI Text cannot render emoji, so the visuals are pure geometry). Every shape
    /// is positioned in unit coordinates of a square icon area of side <paramref name="side"/>.
    /// </summary>
    private void BuildConditionIcon(Transform parent, int index, float side)
    {
        switch (index)
        {
            case 0: // Sunny — warm core disc + eight rays
                AddIconCircle(parent, "Core", SunCoreColor, new Vector2(0.5f, 0.52f), 0.40f, side);
                for (int r = 0; r < 8; r++)
                {
                    float angle = r * 45f;
                    float rad = angle * Mathf.Deg2Rad;
                    var center = new Vector2(0.5f + Mathf.Cos(rad) * 0.36f, 0.52f + Mathf.Sin(rad) * 0.36f);
                    AddIconRect(parent, "Ray" + r, SunRayColor, center, new Vector2(0.16f, 0.055f), angle, side);
                }
                break;

            case 1: // Cloudy — darker back cloud + lighter front cloud
                AddIconCircle(parent, "Back1", CloudDarkColor, new Vector2(0.36f, 0.62f), 0.30f, side);
                AddIconCircle(parent, "Back2", CloudDarkColor, new Vector2(0.60f, 0.64f), 0.30f, side);
                AddIconRect(parent, "BackBase", CloudDarkColor, new Vector2(0.48f, 0.52f), new Vector2(0.44f, 0.16f), 0f, side);
                AddIconCircle(parent, "Front1", CloudLightColor, new Vector2(0.42f, 0.46f), 0.28f, side);
                AddIconCircle(parent, "Front2", CloudLightColor, new Vector2(0.64f, 0.48f), 0.26f, side);
                AddIconRect(parent, "FrontBase", CloudLightColor, new Vector2(0.53f, 0.38f), new Vector2(0.44f, 0.15f), 0f, side);
                break;

            case 2: // Rainy — cloud + three blue drop bars
                AddCloud(parent, side);
                AddIconRect(parent, "Drop1", RainDropColor, new Vector2(0.32f, 0.16f), new Vector2(0.07f, 0.20f), 8f, side);
                AddIconRect(parent, "Drop2", RainDropColor, new Vector2(0.50f, 0.12f), new Vector2(0.07f, 0.20f), 8f, side);
                AddIconRect(parent, "Drop3", RainDropColor, new Vector2(0.68f, 0.16f), new Vector2(0.07f, 0.20f), 8f, side);
                break;

            case 3: // Stormy — cloud + violet zigzag bolt (rotated bars)
                AddCloud(parent, side);
                AddIconRect(parent, "Bolt1", BoltColor, new Vector2(0.55f, 0.26f), new Vector2(0.26f, 0.075f), -50f, side);
                AddIconRect(parent, "Bolt2", BoltColor, new Vector2(0.43f, 0.13f), new Vector2(0.26f, 0.075f), -50f, side);
                AddIconRect(parent, "Bolt3", BoltColor, new Vector2(0.58f, 0.05f), new Vector2(0.14f, 0.075f), -50f, side);
                break;

            case 4: // Snowy — cloud + scattered white dots
                AddCloud(parent, side);
                AddIconCircle(parent, "Flake1", SnowDotColor, new Vector2(0.32f, 0.16f), 0.09f, side);
                AddIconCircle(parent, "Flake2", SnowDotColor, new Vector2(0.50f, 0.10f), 0.09f, side);
                AddIconCircle(parent, "Flake3", SnowDotColor, new Vector2(0.68f, 0.16f), 0.09f, side);
                AddIconCircle(parent, "Flake4", SnowDotColor, new Vector2(0.41f, 0.01f), 0.08f, side);
                AddIconCircle(parent, "Flake5", SnowDotColor, new Vector2(0.59f, 0.01f), 0.08f, side);
                break;

            case 5: // Foggy — layered translucent bars of varying width
                AddIconRect(parent, "Fog1", FogBarColor, new Vector2(0.50f, 0.80f), new Vector2(0.52f, 0.09f), 0f, side);
                AddIconRect(parent, "Fog2", FogBarColor, new Vector2(0.52f, 0.62f), new Vector2(0.76f, 0.09f), 0f, side);
                AddIconRect(parent, "Fog3", FogBarColor, new Vector2(0.48f, 0.44f), new Vector2(0.64f, 0.09f), 0f, side);
                AddIconRect(parent, "Fog4", FogBarColor, new Vector2(0.52f, 0.26f), new Vector2(0.82f, 0.09f), 0f, side);
                AddIconRect(parent, "Fog5", FogBarColor, new Vector2(0.48f, 0.08f), new Vector2(0.50f, 0.09f), 0f, side);
                break;

            case 6: // Windy — sweeping turquoise bars with curled ends
                AddIconRect(parent, "Wind1", WindBarColor, new Vector2(0.42f, 0.76f), new Vector2(0.56f, 0.08f), 0f, side);
                AddIconCircle(parent, "Curl1", WindBarColor, new Vector2(0.74f, 0.72f), 0.14f, side);
                AddIconRect(parent, "Wind2", WindBarColor, new Vector2(0.48f, 0.50f), new Vector2(0.72f, 0.08f), 0f, side);
                AddIconRect(parent, "Wind3", WindBarColor, new Vector2(0.40f, 0.24f), new Vector2(0.48f, 0.08f), 0f, side);
                AddIconCircle(parent, "Curl2", WindBarColor, new Vector2(0.68f, 0.20f), 0.13f, side);
                break;

            case 7: // Hail — cloud + solid light-blue hail stones
                AddCloud(parent, side);
                AddIconCircle(parent, "Stone1", HailDotColor, new Vector2(0.33f, 0.14f), 0.11f, side);
                AddIconCircle(parent, "Stone2", HailDotColor, new Vector2(0.51f, 0.08f), 0.11f, side);
                AddIconCircle(parent, "Stone3", HailDotColor, new Vector2(0.69f, 0.14f), 0.11f, side);
                break;

            case 8: // Sleet — cloud + mix of drop bars and dots
                AddCloud(parent, side);
                AddIconRect(parent, "Mix1", SleetDropColor, new Vector2(0.33f, 0.15f), new Vector2(0.07f, 0.19f), 8f, side);
                AddIconCircle(parent, "Mix2", SleetDropColor, new Vector2(0.51f, 0.10f), 0.09f, side);
                AddIconRect(parent, "Mix3", SleetDropColor, new Vector2(0.68f, 0.15f), new Vector2(0.07f, 0.19f), 8f, side);
                AddIconCircle(parent, "Mix4", SleetDropColor, new Vector2(0.42f, 0.01f), 0.08f, side);
                break;
        }
    }

    /// <summary>Shared cloud silhouette (three circles + a rounded base bar) for precipitation icons.</summary>
    private static void AddCloud(Transform parent, float side)
    {
        AddIconCircle(parent, "Cloud1", CloudLightColor, new Vector2(0.38f, 0.68f), 0.32f, side);
        AddIconCircle(parent, "Cloud2", CloudLightColor, new Vector2(0.62f, 0.68f), 0.32f, side);
        AddIconCircle(parent, "Cloud3", CloudLightColor, new Vector2(0.50f, 0.78f), 0.30f, side);
        AddIconRect(parent, "CloudBase", CloudLightColor, new Vector2(0.50f, 0.58f), new Vector2(0.52f, 0.16f), 0f, side);
    }

    /// <summary>Rounded rectangle shape at unit-space center/size within a square icon area.</summary>
    private static void AddIconRect(Transform parent, string name, Color color, Vector2 center01, Vector2 size01, float rotationDeg, float side)
    {
        var go = new GameObject(name);
        go.transform.SetParent(parent, false);
        var rt = go.AddComponent<RectTransform>();
        rt.anchorMin = new Vector2(0.5f, 0.5f);
        rt.anchorMax = new Vector2(0.5f, 0.5f);
        rt.sizeDelta = new Vector2(size01.x * side, size01.y * side);
        rt.anchoredPosition = new Vector2((center01.x - 0.5f) * side, (center01.y - 0.5f) * side);
        rt.localRotation = Quaternion.Euler(0f, 0f, rotationDeg);
        var img = go.AddComponent<Image>();
        img.color = color;
        img.sprite = GetCapsuleSprite();
        img.type = Image.Type.Sliced;
        img.raycastTarget = false;
    }

    /// <summary>Circle shape at unit-space center/diameter within a square icon area.</summary>
    private static void AddIconCircle(Transform parent, string name, Color color, Vector2 center01, float diameter01, float side)
    {
        var go = new GameObject(name);
        go.transform.SetParent(parent, false);
        var rt = go.AddComponent<RectTransform>();
        rt.anchorMin = new Vector2(0.5f, 0.5f);
        rt.anchorMax = new Vector2(0.5f, 0.5f);
        float d = diameter01 * side;
        rt.sizeDelta = new Vector2(d, d);
        rt.anchoredPosition = new Vector2((center01.x - 0.5f) * side, (center01.y - 0.5f) * side);
        var img = go.AddComponent<Image>();
        img.color = color;
        img.sprite = GetCircleSprite();
        img.raycastTarget = false;
    }

    /// <summary>Recolours every shape of a built condition icon to one flat white tint, capping each shape's own alpha at the tint's alpha (rows render the icon calmer than the hero).</summary>
    private static void TintIconMono(Transform iconRoot, Color tint)
    {
        for (int i = 0; i < iconRoot.childCount; i++)
        {
            var img = iconRoot.GetChild(i).GetComponent<Image>();
            if (img == null) continue;
            img.color = new Color(tint.r, tint.g, tint.b, Mathf.Min(img.color.a, tint.a));
        }
    }

    private void BuildSectionRow(Transform parent)
    {
        var label = UIFactory.Text("SectionLabel", "ALL CONDITIONS", parent, UITheme.Sp(16), TextAnchor.MiddleLeft, FontStyle.Bold);
        var lRt = label.rectTransform;
        lRt.anchorMin = new Vector2(0.05f, SectionRowBottom);
        lRt.anchorMax = new Vector2(0.52f, SectionRowTop);
        lRt.offsetMin = Vector2.zero;
        lRt.offsetMax = Vector2.zero;
        label.color = SubtitleColor;
        label.horizontalOverflow = HorizontalWrapMode.Overflow;
        label.verticalOverflow = VerticalWrapMode.Truncate;
        label.raycastTarget = false;

        // Thin rule that continues the label line to the right edge (visual section spacing).
        var rule = UIFactory.Panel("SectionRule", parent, DividerColor);
        rule.GetComponent<Image>().raycastTarget = false;
        var rRt = rule.GetComponent<RectTransform>();
        rRt.anchorMin = new Vector2(0.56f, 0.567f);
        rRt.anchorMax = new Vector2(0.95f, 0.567f);
        rRt.offsetMin = new Vector2(0f, -UITheme.Dp(1f));
        rRt.offsetMax = new Vector2(0f, UITheme.Dp(1f));
    }

    private void BuildConditionList(Transform parent)
    {
        var list = UIFactory.Panel("ConditionList", parent, Color.clear);
        list.GetComponent<Image>().raycastTarget = false;
        var listRt = list.GetComponent<RectTransform>();
        listRt.anchorMin = new Vector2(0.045f, ListBottom);
        listRt.anchorMax = new Vector2(0.955f, ListTop);
        listRt.offsetMin = Vector2.zero;
        listRt.offsetMax = Vector2.zero;

        // Even height distribution instead of scrolling: all nine conditions stay visible at any
        // resolution, so the active ones are noticeable at a glance.
        // Overflow math: 9 rows x minHeight Dp(24) + 8 x spacing Dp(5) = Dp(256) worst-case
        // minimum (218px at the 0.85 scale floor), while the list band (0.545 down to 0.012 —
        // extended into the former footer band, since the redesign has no footer/status row)
        // is 0.533 of the canvas height (~400px at the 750px reference height, ~340px at the
        // 0.85 minimum scale of a 637px canvas) — ~120px of slack at the tightest supported
        // size. Flexible-height children share the exact band height, so rows can never spill
        // below the canvas.
        var vlg = list.AddComponent<VerticalLayoutGroup>();
        vlg.spacing = UITheme.Dp(RowSpacing);
        vlg.padding = new RectOffset(0, 0, 0, 0);
        vlg.childAlignment = TextAnchor.UpperCenter;
        vlg.childControlWidth = true;
        vlg.childControlHeight = true;
        vlg.childForceExpandWidth = true;
        vlg.childForceExpandHeight = true;

        for (int i = 0; i < ComponentCount; i++) BuildRow(list.transform, i);
    }

    private void BuildRow(Transform parent, int index)
    {
        // Glass card: white 6% fill (radius 12) inset 2 Dp inside a white 12% rounded rim.
        var row = UIFactory.Panel($"Condition_{ComponentNames[index]}", parent, GlassBorderColor);
        var rowImage = row.GetComponent<Image>();
        rowImage.sprite = GetGlassSprite();
        rowImage.type = Image.Type.Sliced;
        rowImage.raycastTarget = false;

        var glass = UIFactory.Panel("Glass", row.transform, GlassFillColor, fullAnchor: true);
        var glassRt = glass.GetComponent<RectTransform>();
        glassRt.offsetMin = new Vector2(UITheme.Dp(2f), UITheme.Dp(2f));
        glassRt.offsetMax = new Vector2(-UITheme.Dp(2f), -UITheme.Dp(2f));
        var glassImg = glass.GetComponent<Image>();
        glassImg.sprite = GetGlassSprite();
        glassImg.type = Image.Type.Sliced;
        glassImg.raycastTarget = false;

        // Zero-weight rows dim as a whole (icons included) via CanvasGroup alpha.
        _rowGroup[index] = row.AddComponent<CanvasGroup>();

        var le = row.AddComponent<LayoutElement>();
        le.minHeight = UITheme.Dp(24f);
        le.preferredHeight = UITheme.Dp(36f);
        le.flexibleWidth = 1f;
        le.flexibleHeight = 1f;

        // Row band spans (fractions of the row; every gap >= 0.02, nothing intersects):
        // icon x 0.048-0.108 (Dp22 box centred at 0.078) | name x 0.15-0.38 |
        // bar x 0.40-0.79 (Dp8 capsule) | pct x 0.81-0.97 (right-aligned).
        // "STORMY" at Sp17 is ~71 of 84 px in the name band, "100%" ~43 of 58 px in the pct
        // band — both fit with clear space, so no text can reach its neighbour's span.

        // Small shape icon of the row's condition, flat white (alpha-capped, no tint).
        var iconRoot = new GameObject("RowIcon");
        iconRoot.transform.SetParent(row.transform, false);
        var iconRt = iconRoot.AddComponent<RectTransform>();
        iconRt.anchorMin = new Vector2(0.078f, 0.5f);
        iconRt.anchorMax = new Vector2(0.078f, 0.5f);
        float rowIconSide = UITheme.Dp(22f);
        iconRt.sizeDelta = new Vector2(rowIconSide, rowIconSide);
        BuildConditionIcon(iconRoot.transform, index, rowIconSide);
        TintIconMono(iconRoot.transform, RowIconColor);

        var name = UIFactory.Text("Name", ComponentNames[index], row.transform, UITheme.Sp(17), TextAnchor.MiddleLeft, FontStyle.Bold);
        var nameRt = name.rectTransform;
        nameRt.anchorMin = new Vector2(0.15f, 0f);
        nameRt.anchorMax = new Vector2(0.38f, 1f);
        nameRt.offsetMin = Vector2.zero;
        nameRt.offsetMax = Vector2.zero;
        name.horizontalOverflow = HorizontalWrapMode.Overflow;
        name.verticalOverflow = VerticalWrapMode.Truncate;
        name.raycastTarget = false;
        _rowName[index] = name;

        // Capsule progress bar: white 8% track, white 65% fill (no condition colour).
        var barBg = UIFactory.Panel("BarBg", row.transform, BarBgColor);
        barBg.GetComponent<Image>().sprite = GetCapsuleSprite();
        barBg.GetComponent<Image>().type = Image.Type.Sliced;
        barBg.GetComponent<Image>().raycastTarget = false;
        var barRt = barBg.GetComponent<RectTransform>();
        barRt.anchorMin = new Vector2(0.40f, 0.5f);
        barRt.anchorMax = new Vector2(0.79f, 0.5f);
        barRt.offsetMin = new Vector2(0f, -UITheme.Dp(4f));
        barRt.offsetMax = new Vector2(0f, UITheme.Dp(4f));

        var fill = UIFactory.Panel("BarFill", barBg.transform, BarFillColor).GetComponent<Image>();
        fill.sprite = GetCapsuleSprite();
        fill.type = Image.Type.Sliced;
        fill.raycastTarget = false;
        var fillRt = fill.rectTransform;
        fillRt.anchorMin = Vector2.zero;
        fillRt.anchorMax = new Vector2(0f, 1f);
        fillRt.offsetMin = Vector2.zero;
        fillRt.offsetMax = Vector2.zero;
        _rowFill[index] = fill;
        _rowFillRt[index] = fillRt;

        var pct = UIFactory.Text("Percent", "0%", row.transform, UITheme.Sp(17), TextAnchor.MiddleRight, FontStyle.Bold);
        var pctRt = pct.rectTransform;
        pctRt.anchorMin = new Vector2(0.81f, 0f);
        pctRt.anchorMax = new Vector2(0.97f, 1f);
        pctRt.offsetMin = Vector2.zero;
        pctRt.offsetMax = Vector2.zero;
        pct.horizontalOverflow = HorizontalWrapMode.Overflow;
        pct.verticalOverflow = VerticalWrapMode.Truncate;
        pct.raycastTarget = false;
        _rowPct[index] = pct;
    }

    private void BuildEmptyState(Transform parent)
    {
        _emptyRoot = new GameObject("EmptyState");
        _emptyRoot.transform.SetParent(parent, false);
        var rootRt = _emptyRoot.AddComponent<RectTransform>();
        rootRt.anchorMin = Vector2.zero;
        rootRt.anchorMax = Vector2.one;
        rootRt.offsetMin = Vector2.zero;
        rootRt.offsetMax = Vector2.zero;

        var card = UIFactory.Panel("Card", _emptyRoot.transform, CardBgColor);
        var cardImg = card.GetComponent<Image>();
        cardImg.sprite = GetRoundedSprite();
        cardImg.type = Image.Type.Sliced;
        cardImg.raycastTarget = false;
        var cardRt = card.GetComponent<RectTransform>();
        cardRt.anchorMin = new Vector2(0.05f, 0.30f);
        cardRt.anchorMax = new Vector2(0.95f, 0.68f);
        cardRt.offsetMin = Vector2.zero;
        cardRt.offsetMax = Vector2.zero;

        var headline = UIFactory.Text("Headline", "No weather data available", card.transform, UITheme.Sp(28), TextAnchor.MiddleCenter, FontStyle.Bold);
        var hlRt = headline.rectTransform;
        hlRt.anchorMin = new Vector2(0.06f, 0.58f);
        hlRt.anchorMax = new Vector2(0.94f, 0.90f);
        hlRt.offsetMin = Vector2.zero;
        hlRt.offsetMax = Vector2.zero;
        headline.raycastTarget = false;

        var hint = UIFactory.Text(
            "Hint",
            "The weather is provided by the in-game weather system and becomes available once you are in-game.",
            card.transform,
            UITheme.Sp(20),
            TextAnchor.UpperCenter);
        var hintRt = hint.rectTransform;
        hintRt.anchorMin = new Vector2(0.08f, 0.14f);
        hintRt.anchorMax = new Vector2(0.92f, 0.54f);
        hintRt.offsetMin = Vector2.zero;
        hintRt.offsetMax = Vector2.zero;
        hint.horizontalOverflow = HorizontalWrapMode.Wrap;
        hint.verticalOverflow = VerticalWrapMode.Overflow;
        hint.color = SubtitleColor;
        hint.raycastTarget = false;
    }

    // =====================================================================
    // Generated sprites (rounded rect / capsule / circle / glass card / donut)
    // =====================================================================

    private static Sprite GetRoundedSprite()
    {
        if (_roundedSprite != null) return _roundedSprite;
        const int size = 32;
        const float radius = 8f;
        var tex = new Texture2D(size, size, TextureFormat.ARGB32, false);
        var pixels = new Color32[size * size];
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                // Distance to the nearest inner corner; alpha ramps over ~1px for soft edges.
                float cx = Mathf.Clamp(x + 0.5f, radius, size - radius);
                float cy = Mathf.Clamp(y + 0.5f, radius, size - radius);
                float dist = Vector2.Distance(new Vector2(x + 0.5f, y + 0.5f), new Vector2(cx, cy));
                float a = Mathf.Clamp01(radius - dist + 0.5f);
                pixels[y * size + x] = new Color32(255, 255, 255, (byte)Mathf.RoundToInt(a * 255f));
            }
        }
        tex.SetPixels32(pixels);
        tex.Apply();
        _roundedSprite = Sprite.Create(tex, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f), 100f, 0,
            SpriteMeshType.FullRect, new Vector4(radius, radius, radius, radius));
        return _roundedSprite;
    }

    private static Sprite GetCapsuleSprite()
    {
        if (_capsuleSprite != null) return _capsuleSprite;
        const int size = 32;
        const float radius = 15.5f;
        var tex = new Texture2D(size, size, TextureFormat.ARGB32, false);
        var pixels = new Color32[size * size];
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                float cx = Mathf.Clamp(x + 0.5f, radius, size - radius);
                float cy = Mathf.Clamp(y + 0.5f, radius, size - radius);
                float dist = Vector2.Distance(new Vector2(x + 0.5f, y + 0.5f), new Vector2(cx, cy));
                float a = Mathf.Clamp01(radius - dist + 0.5f);
                pixels[y * size + x] = new Color32(255, 255, 255, (byte)Mathf.RoundToInt(a * 255f));
            }
        }
        tex.SetPixels32(pixels);
        tex.Apply();
        _capsuleSprite = Sprite.Create(tex, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f), 100f, 0,
            SpriteMeshType.FullRect, new Vector4(radius, radius, radius, radius));
        return _capsuleSprite;
    }

    private static Sprite GetCircleSprite()
    {
        if (_circleSprite != null) return _circleSprite;
        const int size = 48;
        var tex = new Texture2D(size, size, TextureFormat.ARGB32, false);
        var pixels = new Color32[size * size];
        float half = size * 0.5f;
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                float dist = Vector2.Distance(new Vector2(x + 0.5f, y + 0.5f), new Vector2(half, half));
                float a = Mathf.Clamp01(half - 1f - dist + 0.5f);
                pixels[y * size + x] = new Color32(255, 255, 255, (byte)Mathf.RoundToInt(a * 255f));
            }
        }
        tex.SetPixels32(pixels);
        tex.Apply();
        _circleSprite = Sprite.Create(tex, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f), 100f);
        return _circleSprite;
    }

    /// <summary>Glass-card rounded rect (radius 12) for the condition rows.</summary>
    private static Sprite GetGlassSprite()
    {
        if (_glassSprite != null) return _glassSprite;
        const int size = 48;
        const float radius = 12f;
        var tex = new Texture2D(size, size, TextureFormat.ARGB32, false);
        var pixels = new Color32[size * size];
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                float cx = Mathf.Clamp(x + 0.5f, radius, size - radius);
                float cy = Mathf.Clamp(y + 0.5f, radius, size - radius);
                float dist = Vector2.Distance(new Vector2(x + 0.5f, y + 0.5f), new Vector2(cx, cy));
                float a = Mathf.Clamp01(radius - dist + 0.5f);
                pixels[y * size + x] = new Color32(255, 255, 255, (byte)Mathf.RoundToInt(a * 255f));
            }
        }
        tex.SetPixels32(pixels);
        tex.Apply();
        _glassSprite = Sprite.Create(tex, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f), 100f, 0,
            SpriteMeshType.FullRect, new Vector4(radius, radius, radius, radius));
        return _glassSprite;
    }

    /// <summary>
    /// Anti-aliased donut (annulus, ring thickness ~9% of the diameter) for the hero ring
    /// gauge; drawn as Image.Type.Filled / Radial360 the arc sweeps clockwise from the top.
    /// </summary>
    private static Sprite GetDonutSprite()
    {
        if (_donutSprite != null) return _donutSprite;
        const int size = 128;
        const float outer = 63f;
        const float inner = 51.5f;
        var tex = new Texture2D(size, size, TextureFormat.ARGB32, false);
        var pixels = new Color32[size * size];
        float half = size * 0.5f;
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                float dist = Vector2.Distance(new Vector2(x + 0.5f, y + 0.5f), new Vector2(half, half));
                float a = Mathf.Clamp01(Mathf.Min(dist - inner, outer - dist) + 0.5f);
                pixels[y * size + x] = new Color32(255, 255, 255, (byte)Mathf.RoundToInt(a * 255f));
            }
        }
        tex.SetPixels32(pixels);
        tex.Apply();
        _donutSprite = Sprite.Create(tex, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f), 100f);
        return _donutSprite;
    }

    // =====================================================================
    // Rendering
    // =====================================================================

    private void RenderInternal()
    {
        // _mainBG is null before OnCreatedUI and a dead wrapper after a scene unload.
        if (!NetworkGuard.IsAlive(_mainBG)) return;
        _dirty = false;

        WeatherState? snapshot = _state;
        bool hasData = snapshot.HasValue;

        if (NetworkGuard.IsAlive(_contentRoot) && _contentRoot.activeSelf != hasData) _contentRoot.SetActive(hasData);
        if (NetworkGuard.IsAlive(_emptyRoot) && _emptyRoot.activeSelf != !hasData) _emptyRoot.SetActive(!hasData);
        if (!hasData)
        {
            if (_mainBgImage != null) _mainBgImage.color = BgColor;
            return;
        }

        WeatherState state = snapshot!.Value;
        FillValues(state, _values);

        int dominant = 0;
        int activeCount = 0;
        float max = -1f;
        for (int i = 0; i < ComponentCount; i++)
        {
            float value = Mathf.Clamp01(_values[i]);
            if (value > ActiveEpsilon) activeCount++;
            if (value > max)
            {
                max = value;
                dominant = i;
            }
        }

        bool clear = max <= ActiveEpsilon;

        // Neutral charcoal canvas: the dominant colour never tints a surface — it feeds the
        // ring-gauge arc below (the app's single accent) and nothing else.
        if (_mainBgImage != null) _mainBgImage.color = BgColor;

        // Shape icon selection (one geometric icon per condition, no glyphs).
        for (int i = 0; i < ComponentCount; i++)
        {
            var root = _heroIconRoots[i];
            if (root != null) root.SetActive(!clear && i == dominant);
        }

        // --- Hero: dominant condition (type is white/alpha only) ---
        _heroName.color = Color.white;
        _heroPct.color = Color.white;
        _heroState.color = ChipTextColor;
        if (clear)
        {
            _heroName.text = "CLEAR";
            _heroPct.text = "0%";
            _heroState.text = "CLEAR";
            if (_ringFill != null)
            {
                _ringFill.color = RingTrackColor;
                _ringFill.fillAmount = 0f;
            }
        }
        else
        {
            _heroName.text = ComponentNames[dominant].ToUpperInvariant();
            _heroPct.text = PercentText(max);
            _heroState.text = max > 0.66f ? "HEAVY" : (max >= 0.33f ? "MODERATE" : "LIGHT");
            if (_ringFill != null)
            {
                Color ringColor = ComponentColors[dominant];
                _ringFill.color = new Color(ringColor.r, ringColor.g, ringColor.b, RingFillAlpha);
                _ringFill.fillAmount = max;
            }
        }

        if (NetworkGuard.IsAlive(_activeLabel))
        {
            _activeLabel.text = $"{activeCount} OF {ComponentCount} ACTIVE";
        }

        // --- All nine components ---
        for (int i = 0; i < ComponentCount; i++)
        {
            float value = Mathf.Clamp01(_values[i]);
            bool active = value > ActiveEpsilon;

            _rowName[i].text = ComponentNames[i].ToUpperInvariant();
            _rowName[i].color = Color.white;
            _rowPct[i].text = PercentText(value);
            _rowPct[i].color = PctColor;
            _rowFill[i].color = BarFillColor;
            if (_rowGroup[i] != null) _rowGroup[i].alpha = active ? 1f : RowDimAlpha;
            SetBarFill(_rowFillRt[i], active ? value : 0f);
        }
    }

    private static void SetBarFill(RectTransform fill, float value)
    {
        if (fill == null) return;
        fill.anchorMin = Vector2.zero;
        fill.anchorMax = new Vector2(Mathf.Clamp01(value), 1f);
        fill.offsetMin = Vector2.zero;
        fill.offsetMax = Vector2.zero;
    }

    private static string PercentText(float value) => Mathf.RoundToInt(Mathf.Clamp01(value) * 100f) + "%";

    private static void FillValues(WeatherState s, float[] dst)
    {
        dst[0] = s.Sunny;
        dst[1] = s.Cloudy;
        dst[2] = s.Rainy;
        dst[3] = s.Stormy;
        dst[4] = s.Snowy;
        dst[5] = s.Foggy;
        dst[6] = s.Windy;
        dst[7] = s.Hail;
        dst[8] = s.Sleet;
    }
}
