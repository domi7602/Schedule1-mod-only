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
/// Visual design (mockup build, 2026-10-01): flat near-black canvas with no header bar and no
/// divider rules. The hero card is an accent-bordered rounded panel with a soft accent halo,
/// holding the giant condition name, the big percentage, a solid accent pill with dark text
/// (HEAVY/MODERATE/LIGHT) and the "N OF 9 ACTIVE" meta line; the radial ring gauge on the right
/// carries the same accent arc (rounded cap) around the shape-drawn condition icon. Below it,
/// "ALL CONDITIONS" captions a stack of nine rounded rows whose icons always carry their
/// condition's theme colour while the accent tint, border and bar mark the active conditions;
/// inactive rows otherwise stay neutral grey-blue. A tie for the top
/// weight shows "MIXED". No footer/status row.
///
/// Every anchor is a canvas fraction measured off the reference design (aspect 400:750, so the
/// fractions map 1:1 onto the phone canvas at any resolution).
/// </summary>
public sealed class WeatherApp : PhoneApp
{
    // --- Canvas-level anchors (x across the canvas, y up from its bottom) ---
    private const float TitleLeft = 0.0546f;
    private const float TitleRight = 0.62f;        // safety edge only — the title is short
    private const float HeaderCenterY = 0.966f;    // title / live dot / live label share this line
    private const float LiveDotCenterX = 0.702f;
    private const float LiveLabelLeft = 0.700f;    // the label is right-aligned; this is its floor
    private const float LiveLabelRight = 0.944f;
    private const float CardLeft = 0.052f;         // hero card and condition rows share these edges
    private const float CardRight = 0.948f;
    private const float HeroTop = 0.9330f;
    private const float HeroBottom = 0.7723f;
    private const float SectionLeft = 0.0535f;
    private const float SectionRight = 0.60f;
    private const float SectionCenterY = 0.738f;
    private const float ListTop = 0.7164f;         // first row's top edge
    private const float ListBottom = 0.0606f;      // last row's bottom edge
    private const float RowSpacing = 6f;           // Dp between rows

    // --- Hero card child bands (fractions of the card; x across it, y up from its bottom) ---
    // Measured spans (card = 358x121 px at the 400x750 reference):
    //   name  glyph band y 0.62-0.81, centre 0.714 (Sp32 bold)   gap 0.09 v
    //   pct   glyph band y 0.40-0.53, centre 0.464 (Sp24 bold)   gap 0.11 v
    //   chip  y 0.138-0.290 (Dp19 pill)  |  meta line left x 0.279, same centre
    //   ring  box centred at x 0.833, side min(0.757*heroH, 0.2546*heroW) = 91 px — the icon box
    //         (0.56 of the ring) can never reach the left text column, whose widest member
    //         ("MIXED" at Sp32 = 101 px) ends around x 0.33 of the card.
    private const float HeroPadL = 0.052f;         // left text pad (name, pct)
    private const float HeroTextRight = 0.60f;
    private const float HeroNameCenterY = 0.714f;
    private const float HeroPctCenterY = 0.464f;
    private const float HeroChipLeft = 0.0487f;
    private const float HeroChipRight = 0.2424f;
    private const float HeroChipCenterY = 0.214f;
    private const float HeroMetaLeft = 0.279f;
    private const float HeroMetaRight = 0.62f;
    private const float HeroRingCenterX = 0.833f;
    private const float HeroRingCenterY = 0.496f;
    private const float HeroRingOfHeight = 0.757f;
    private const float HeroRingOfWidth = 0.2546f;
    private const float HeroIconOfRing = 0.56f;

    // --- Row bands (fractions of the row; x across it, y up from its bottom) ---
    //   icon x 0.061-0.123 (Dp22 box, centre 0.092, vertically centred)
    //   name y 0.55-0.76 (Sp12 bold, left x 0.1876 — left-aligned with the bar)
    //   bar  y 0.265-0.371 (Dp7.5 capsule, x 0.1864-0.8344), gap 0.09 v to the name
    //   pct  right-aligned at x 0.9513, vertically centred in the row (Sp13 bold)
    private const float RowIconCenterX = 0.092f;
    private const float RowTextLeft = 0.1876f;
    private const float RowBarLeft = 0.1864f;
    private const float RowBarRight = 0.8344f;
    private const float RowBarCenterY = 0.318f;
    private const float RowNameCenterY = 0.6525f;
    private const float RowPctRight = 0.955f;

    // --- Type scale (canvas px at the 400x750 reference) ---
    private const int TitleSp = 21;
    private const int LiveSp = 10;
    private const int SectionSp = 14;
    private const int HeroNameSp = 32;
    private const int HeroPctSp = 24;
    private const int HeroChipSp = 10;
    private const int HeroMetaSp = 10;
    private const int RowNameSp = 12;
    private const int RowPctSp = 13;

    // --- Metrics (Dp) ---
    private const float LiveDotSize = 6f;
    private const float HeroRadius = 12f;
    private const float RowRadius = 10f;
    private const float CardBorder = 2f;
    private const float HeroChipHeight = 19f;
    private const float HeroGlowSpread = 5f;
    private const float RowIconSize = 22f;
    private const float RowBarHeight = 7.5f;

    // --- Component table (order matches WeatherState / the nine native components) ---
    private const int ComponentCount = 9;
    private const float ActiveEpsilon = 0.0005f;

    private static readonly string[] ComponentNames =
    {
        "Sunny", "Cloudy", "Rainy", "Stormy", "Snowy", "Foggy", "Windy", "Hail", "Sleet"
    };

    // --- Condition accents: hero border/halo, chip, ring arc, every row icon plus the active
    //     rows' tint/border/bar ---
    // Sunny, Cloudy, Foggy and Windy are the accents measured off the reference design; the other
    // five keep the house palette (they sit inactive in the reference).
    private static readonly Color[] ComponentColors =
    {
        new(0.992f, 0.816f, 0.180f, 1f), // Sunny  — amber
        new(0.557f, 0.710f, 0.882f, 1f), // Cloudy — light blue
        new(0.376f, 0.647f, 0.980f, 1f), // Rainy  — blue
        new(0.639f, 0.435f, 0.960f, 1f), // Stormy — violet
        new(0.949f, 0.973f, 1.000f, 1f), // Snowy  — white
        new(0.675f, 0.643f, 0.588f, 1f), // Foggy  — warm grey
        new(0.133f, 0.651f, 0.725f, 1f), // Windy  — teal
        new(0.549f, 0.800f, 1.000f, 1f), // Hail   — light blue
        new(0.376f, 0.902f, 0.925f, 1f)  // Sleet  — cyan
    };

    // --- Palette (measured off the reference design) ---
    private static readonly Color BgColor = new(0.075f, 0.090f, 0.114f, 1f);          // canvas
    private static readonly Color HeroFillColor = new(0.098f, 0.110f, 0.114f, 1f);    // hero card base
    private static readonly Color RowFillColor = new(0.126f, 0.149f, 0.184f, 1f);     // inactive row base
    private static readonly Color RowBorderColor = new(0.424f, 0.455f, 0.514f, 1f);   // inactive row rim
    private const float AccentRowTint = 0.14f;                                        // accent over BgColor
    private static readonly Color LiveDotColor = new(0.996f, 0.812f, 0.145f, 1f);
    private const float HeroGlowAlpha = 0.17f;                                        // accent over canvas
    private static readonly Color ChipTextColor = new(0.043f, 0.051f, 0.071f, 1f);
    private static readonly Color TextPrimary = new(1f, 1f, 1f, 1f);
    private static readonly Color TextPct = new(1f, 1f, 1f, 0.94f);
    private static readonly Color TextSection = new(1f, 1f, 1f, 0.68f);
    private static readonly Color TextLive = new(1f, 1f, 1f, 0.62f);
    private static readonly Color TextMeta = new(1f, 1f, 1f, 0.58f);
    private static readonly Color BarTrackColor = new(1f, 1f, 1f, 0.10f);   // over the row base
    private static readonly Color RingTrackColor = new(1f, 1f, 1f, 0.14f);  // over the hero base

    // --- Shape-icon palette (flat white alpha ramps; the render pass tints them per state) ---
    private static readonly Color SunCoreColor = new(1f, 1f, 1f, 0.95f);
    private static readonly Color SunRayColor = new(1f, 1f, 1f, 0.80f);
    private static readonly Color CloudLightColor = new(1f, 1f, 1f, 0.90f);
    private static readonly Color CloudDarkColor = new(1f, 1f, 1f, 0.50f);
    private static readonly Color RainDropColor = new(1f, 1f, 1f, 0.80f);
    private static readonly Color BoltColor = new(1f, 1f, 1f, 0.95f);
    private static readonly Color SnowDotColor = new(1f, 1f, 1f, 0.90f);
    private static readonly Color FogBarColor = new(1f, 1f, 1f, 0.60f);
    private static readonly Color WindBarColor = new(1f, 1f, 1f, 0.85f);
    private static readonly Color HailDotColor = new(1f, 1f, 1f, 0.85f);
    private static readonly Color SleetDropColor = new(1f, 1f, 1f, 0.80f);

    // --- Donut geometry (kept in one place — the ring cap needs the same numbers) ---
    private const float DonutSize = 128f;
    private const float DonutOuter = 63f;
    private const float DonutInner = 50f;

    // --- UI references ---
    private GameObject _mainBG = null!;
    private GameObject _contentRoot = null!;
    private GameObject _emptyRoot = null!;
    private Text _heroName = null!;
    private Text _heroPct = null!;
    private Text _heroState = null!;
    private Text _activeLabel = null!;
    private Image _heroRim = null!;
    private Image _heroChip = null!;
    private Image _heroGlow = null!;
    private Image _liveDot = null!;
    private Image _ringFill = null!;
    private Image _ringCap = null!;
    private float _ringSide;
    private bool _ringCleared = true;
    private readonly GameObject[] _heroIconRoots = new GameObject[ComponentCount];
    private readonly Text[] _rowName = new Text[ComponentCount];
    private readonly Text[] _rowPct = new Text[ComponentCount];
    private readonly Image[] _rowFill = new Image[ComponentCount];
    private readonly RectTransform[] _rowFillRt = new RectTransform[ComponentCount];
    private readonly Image[] _rowRim = new Image[ComponentCount];
    private readonly Image[] _rowBase = new Image[ComponentCount];
    private readonly Transform[] _rowIconRoot = new Transform[ComponentCount];

    // --- Generated sprite cache (created once, reused by every card/bar/shape) ---
    private static Sprite? _capsuleSprite;
    private static Sprite? _circleSprite;
    private static Sprite? _heroSprite;
    private static Sprite? _rowSprite;
    private static Sprite? _glowSprite;
    private static Sprite? _donutSprite;

    // --- Runtime state ---
    // One app instance is discovered per Main-scene load. Static handlers are subscribed exactly
    // once and always dispatch through _active, so a scene reload never stacks subscribers.
    private static WeatherApp? _active;
    private static bool _staticSubscribed;

    private readonly float[] _values = new float[ComponentCount];
    private WeatherState? _state;
    private bool _dirty;
    private float _nextPollTime;   // fallback-poll throttle (unscaled seconds)

    // --- Lively animation state (targets are set by RenderInternal, eased in AnimateTick) ---
    private readonly float[] _target = new float[ComponentCount];
    private readonly float[] _shown = new float[ComponentCount];
    private float _ringTarget;
    private float _ringShown;
    private const float AnimSpeed = 8f;
    private const float PollInterval = 0.5f;

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

        // Fallback poll of the immutable snapshot, throttled to ~0.5 s: covers a weather change
        // that happened before this instance subscribed or an edge case where the change event
        // was missed. The WeatherManager.OnWeatherChanged event still re-renders immediately.
        if (_dirty || Time.unscaledTime >= _nextPollTime)
        {
            _nextPollTime = Time.unscaledTime + PollInterval;
            WeatherState? current = SafeCurrent();
            if (_dirty || !SameState(current, _state))
            {
                _state = current;
                RenderInternal();
            }
        }

        AnimateTick();

        if (Input.GetKeyDown(KeyCode.Escape)) CloseApp();
    }

    /// <summary>
    /// Eases bars/ring toward their targets and animates the live dot and the hero icon (the sun
    /// rotates, the other icons bob gently). Layout-affecting values are written only while they
    /// change, so an idle screen costs no layout rebuilds.
    /// </summary>
    private void AnimateTick()
    {
        float dt = Time.unscaledDeltaTime;
        float k = 1f - Mathf.Exp(-AnimSpeed * dt);
        for (int i = 0; i < ComponentCount; i++)
        {
            float next = Mathf.Lerp(_shown[i], _target[i], k);
            if (Mathf.Abs(_target[i] - next) < 0.0015f) next = _target[i];
            if (next != _shown[i])
            {
                _shown[i] = next;
                SetBarFill(_rowFillRt[i], _shown[i]);
            }
        }
        float ringNext = Mathf.Lerp(_ringShown, _ringTarget, k);
        if (Mathf.Abs(_ringTarget - ringNext) < 0.0015f) ringNext = _ringTarget;
        if (ringNext != _ringShown)
        {
            _ringShown = ringNext;
            if (_ringFill != null) _ringFill.fillAmount = _ringShown;
            UpdateRingCap(_ringShown);
        }

        // Soft pulse on the header dot — it labels the readout as live.
        if (_liveDot != null)
        {
            float a = 0.55f + 0.45f * (0.5f + 0.5f * Mathf.Sin(Time.unscaledTime * 2.2f));
            _liveDot.color = new Color(LiveDotColor.r, LiveDotColor.g, LiveDotColor.b, a);
        }

        for (int i = 0; i < ComponentCount; i++)
        {
            var root = _heroIconRoots[i];
            if (root == null || !root.activeSelf) continue;
            var rt = (RectTransform)root.transform;
            if (i == 0)
                rt.Rotate(0f, 0f, -12f * dt);
            else
            {
                rt.localRotation = Quaternion.identity;
                rt.anchoredPosition = new Vector2(0f, Mathf.Sin(Time.unscaledTime * 1.5f) * rt.sizeDelta.y * 0.03f);
            }
        }
    }

    /// <summary>Keeps the round cap of the ring arc at the arc's tip (the design uses rounded caps).</summary>
    private void UpdateRingCap(float fill)
    {
        if (_ringCap == null) return;
        bool show = !_ringCleared && fill > 0.012f;
        if (_ringCap.gameObject.activeSelf != show) _ringCap.gameObject.SetActive(show);
        if (!show) return;
        float radius = _ringSide * ((DonutOuter + DonutInner) * 0.5f / (DonutSize * 0.5f));
        float angle = 90f - fill * 360f; // 12 o'clock, sweeping clockwise
        float rad = angle * Mathf.Deg2Rad;
        ((RectTransform)_ringCap.transform).anchoredPosition = new Vector2(Mathf.Cos(rad) * radius, Mathf.Sin(rad) * radius);
    }

    private void OnAppOpened()
    {
        Array.Clear(_shown, 0, _shown.Length);
        _ringShown = 0f;
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
        _mainBG.GetComponent<Image>().raycastTarget = false;
        _mainBG.SetActive(false);

        BuildHeader(_mainBG.transform);
        BuildContent(_mainBG.transform);
        BuildEmptyState(_mainBG.transform);

        RenderInternal();
    }

    private void BuildHeader(Transform parent)
    {
        // Flat header line: title left, pulsing accent dot + muted live label right.
        // No bar, no tick, no divider rule — the design separates with spacing only.
        var title = PlaceText(parent, "Title", "WEATHER", UITheme.Sp(TitleSp), TextAnchor.MiddleLeft,
            TitleLeft, TitleRight, HeaderCenterY, FontStyle.Bold, TextPrimary);

        _liveDot = UIFactory.Panel("LiveDot", parent, LiveDotColor).GetComponent<Image>();
        _liveDot.sprite = GetCircleSprite();
        _liveDot.raycastTarget = false;
        var dotRt = _liveDot.rectTransform;
        dotRt.anchorMin = new Vector2(LiveDotCenterX, HeaderCenterY);
        dotRt.anchorMax = new Vector2(LiveDotCenterX, HeaderCenterY);
        dotRt.sizeDelta = new Vector2(UITheme.Dp(LiveDotSize), UITheme.Dp(LiveDotSize));

        PlaceText(parent, "LiveLabel", "LIVE CONDITIONS", UITheme.Sp(LiveSp), TextAnchor.MiddleRight,
            LiveLabelLeft, LiveLabelRight, HeaderCenterY, FontStyle.Normal, TextLive);
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
        float heroW = (CardRight - CardLeft) * UITheme.ActualWidth;
        float heroH = (HeroTop - HeroBottom) * UITheme.ActualHeight;

        // Soft accent halo behind the card (feathered sprite, spread by the same amount).
        var glow = UIFactory.Panel("HeroGlow", parent, new Color(1f, 1f, 1f, HeroGlowAlpha));
        _heroGlow = glow.GetComponent<Image>();
        _heroGlow.sprite = GetGlowSprite();
        _heroGlow.type = Image.Type.Sliced;
        _heroGlow.raycastTarget = false;
        var glowRt = _heroGlow.rectTransform;
        glowRt.anchorMin = new Vector2(CardLeft, HeroBottom);
        glowRt.anchorMax = new Vector2(CardRight, HeroTop);
        glowRt.offsetMin = new Vector2(-UITheme.Dp(HeroGlowSpread), -UITheme.Dp(HeroGlowSpread));
        glowRt.offsetMax = new Vector2(UITheme.Dp(HeroGlowSpread), UITheme.Dp(HeroGlowSpread));

        // Accent-bordered card: outer rounded rim plus a base inset by the border width.
        var hero = new GameObject("HeroCard");
        hero.transform.SetParent(parent, false);
        var heroRt = hero.AddComponent<RectTransform>();
        heroRt.anchorMin = new Vector2(CardLeft, HeroBottom);
        heroRt.anchorMax = new Vector2(CardRight, HeroTop);
        heroRt.offsetMin = Vector2.zero;
        heroRt.offsetMax = Vector2.zero;

        var rim = UIFactory.Panel("Rim", hero.transform, RowBorderColor, fullAnchor: true);
        _heroRim = rim.GetComponent<Image>();
        _heroRim.sprite = GetHeroSprite();
        _heroRim.type = Image.Type.Sliced;
        _heroRim.raycastTarget = false;

        var basePanel = UIFactory.Panel("Base", hero.transform, HeroFillColor, fullAnchor: true);
        var baseRt = basePanel.GetComponent<RectTransform>();
        baseRt.offsetMin = new Vector2(UITheme.Dp(CardBorder), UITheme.Dp(CardBorder));
        baseRt.offsetMax = new Vector2(-UITheme.Dp(CardBorder), -UITheme.Dp(CardBorder));
        var baseImg = basePanel.GetComponent<Image>();
        baseImg.sprite = GetHeroSprite();
        baseImg.type = Image.Type.Sliced;
        baseImg.raycastTarget = false;

        // --- Ring gauge: donut track + accent arc (rounded cap) around the condition icon ---
        var iconArea = new GameObject("IconArea");
        iconArea.transform.SetParent(hero.transform, false);
        var iconRt = iconArea.AddComponent<RectTransform>();
        _ringSide = Mathf.Min(heroH * HeroRingOfHeight, heroW * HeroRingOfWidth);
        float iconSide = _ringSide * HeroIconOfRing;
        iconRt.anchorMin = new Vector2(HeroRingCenterX, HeroRingCenterY);
        iconRt.anchorMax = new Vector2(HeroRingCenterX, HeroRingCenterY);
        iconRt.sizeDelta = new Vector2(_ringSide, _ringSide);

        var track = UIFactory.Panel("RingTrack", iconArea.transform, RingTrackColor, fullAnchor: true);
        var trackImg = track.GetComponent<Image>();
        trackImg.sprite = GetDonutSprite();
        trackImg.raycastTarget = false;

        var fill = UIFactory.Panel("RingFill", iconArea.transform, RingTrackColor, fullAnchor: true);
        _ringFill = fill.GetComponent<Image>();
        _ringFill.sprite = GetDonutSprite();
        _ringFill.type = Image.Type.Filled;
        _ringFill.fillMethod = Image.FillMethod.Radial360;
        _ringFill.fillOrigin = (int)Image.Origin360.Top;
        _ringFill.fillClockwise = true;
        _ringFill.fillAmount = 0f;
        _ringFill.raycastTarget = false;

        var cap = UIFactory.Panel("RingCap", iconArea.transform, RingTrackColor);
        _ringCap = cap.GetComponent<Image>();
        _ringCap.sprite = GetCircleSprite();
        _ringCap.raycastTarget = false;
        var capRt = _ringCap.rectTransform;
        capRt.anchorMin = new Vector2(0.5f, 0.5f);
        capRt.anchorMax = new Vector2(0.5f, 0.5f);
        float stroke = (DonutOuter - DonutInner) / DonutSize * _ringSide;
        capRt.sizeDelta = new Vector2(stroke, stroke);
        _ringCap.gameObject.SetActive(false);

        // Shape-drawn condition icons (sun disc, cloud, drops, ...) centred inside the ring.
        for (int i = 0; i < ComponentCount; i++)
        {
            var root = new GameObject("Icon_" + ComponentNames[i]);
            root.transform.SetParent(iconArea.transform, false);
            var rootRt = root.AddComponent<RectTransform>();
            rootRt.anchorMin = new Vector2(0.5f, 0.5f);
            rootRt.anchorMax = new Vector2(0.5f, 0.5f);
            rootRt.sizeDelta = new Vector2(iconSide, iconSide);
            BuildConditionIcon(root.transform, i, iconSide);
            TintIconMono(root.transform, ComponentColors[i]);
            root.SetActive(false);
            _heroIconRoots[i] = root;
        }

        // Dominant condition name and percentage, stacked in the left column.
        _heroName = PlaceText(hero.transform, "DominantName", string.Empty, UITheme.Sp(HeroNameSp),
            TextAnchor.MiddleLeft, HeroPadL, HeroTextRight, HeroNameCenterY, FontStyle.Bold, TextPrimary);
        _heroPct = PlaceText(hero.transform, "DominantPercent", string.Empty, UITheme.Sp(HeroPctSp),
            TextAnchor.MiddleLeft, HeroPadL, HeroTextRight, HeroPctCenterY, FontStyle.Bold, TextPrimary);

        // Solid accent pill (dark text) with the meta count to its right.
        var chip = UIFactory.Panel("IntensityChip", hero.transform, ComponentColors[0]);
        _heroChip = chip.GetComponent<Image>();
        _heroChip.sprite = GetCapsuleSprite();
        _heroChip.type = Image.Type.Sliced;
        _heroChip.raycastTarget = false;
        PlaceCentered(_heroChip.rectTransform, HeroChipLeft, HeroChipRight, HeroChipCenterY, UITheme.Dp(HeroChipHeight));

        _heroState = UIFactory.Text("Intensity", string.Empty, chip.transform, UITheme.Sp(HeroChipSp), TextAnchor.MiddleCenter, FontStyle.Bold);
        var stateRt = _heroState.rectTransform;
        stateRt.anchorMin = Vector2.zero;
        stateRt.anchorMax = Vector2.one;
        stateRt.offsetMin = Vector2.zero;
        stateRt.offsetMax = Vector2.zero;
        _heroState.color = ChipTextColor;
        _heroState.horizontalOverflow = HorizontalWrapMode.Overflow;
        _heroState.verticalOverflow = VerticalWrapMode.Truncate;
        _heroState.raycastTarget = false;

        _activeLabel = PlaceText(hero.transform, "ActiveCount", string.Empty, UITheme.Sp(HeroMetaSp),
            TextAnchor.MiddleLeft, HeroMetaLeft, HeroMetaRight, HeroChipCenterY, FontStyle.Normal, TextMeta);
    }

    private void BuildSectionRow(Transform parent)
    {
        // Plain muted caption — the design deliberately has no rule line here.
        PlaceText(parent, "SectionLabel", "ALL CONDITIONS", UITheme.Sp(SectionSp), TextAnchor.MiddleLeft,
            SectionLeft, SectionRight, SectionCenterY, FontStyle.Bold, TextSection);
    }

    private void BuildConditionList(Transform parent)
    {
        var list = UIFactory.Panel("ConditionList", parent, Color.clear);
        list.GetComponent<Image>().raycastTarget = false;
        var listRt = list.GetComponent<RectTransform>();
        listRt.anchorMin = new Vector2(CardLeft, ListBottom);
        listRt.anchorMax = new Vector2(CardRight, ListTop);
        listRt.offsetMin = Vector2.zero;
        listRt.offsetMax = Vector2.zero;

        // Even height distribution instead of scrolling: all nine conditions stay visible at any
        // resolution. Overflow math: 9 x minHeight Dp(24) + 8 x spacing Dp(6) = Dp(264) worst case
        // (224 px at the 0.85 scale floor) while the list band is 0.6558 of the canvas height
        // (418 px at that floor) — generous slack, so rows can never spill below the canvas.
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
        // Accent-bordered rounded card: outer rim plus a base inset by the border width.
        var row = UIFactory.Panel($"Condition_{ComponentNames[index]}", parent, RowBorderColor);
        var rowImage = row.GetComponent<Image>();
        rowImage.sprite = GetRowSprite();
        rowImage.type = Image.Type.Sliced;
        rowImage.raycastTarget = false;
        _rowRim[index] = rowImage;

        var basePanel = UIFactory.Panel("Base", row.transform, RowFillColor, fullAnchor: true);
        var baseRt = basePanel.GetComponent<RectTransform>();
        baseRt.offsetMin = new Vector2(UITheme.Dp(CardBorder), UITheme.Dp(CardBorder));
        baseRt.offsetMax = new Vector2(-UITheme.Dp(CardBorder), -UITheme.Dp(CardBorder));
        var baseImage = basePanel.GetComponent<Image>();
        baseImage.sprite = GetRowSprite();
        baseImage.type = Image.Type.Sliced;
        baseImage.raycastTarget = false;
        _rowBase[index] = baseImage;

        var le = row.AddComponent<LayoutElement>();
        le.minHeight = UITheme.Dp(24f);
        le.preferredHeight = UITheme.Dp(46f);
        le.flexibleWidth = 1f;
        le.flexibleHeight = 1f;

        // Shape icon of the row's condition (always in its own theme colour).
        var iconRoot = new GameObject("RowIcon");
        iconRoot.transform.SetParent(row.transform, false);
        var iconRt = iconRoot.AddComponent<RectTransform>();
        iconRt.anchorMin = new Vector2(RowIconCenterX, 0.5f);
        iconRt.anchorMax = new Vector2(RowIconCenterX, 0.5f);
        float rowIconSide = UITheme.Dp(RowIconSize);
        iconRt.sizeDelta = new Vector2(rowIconSide, rowIconSide);
        BuildConditionIcon(iconRoot.transform, index, rowIconSide);
        TintIconMono(iconRoot.transform, ComponentColors[index]);
        _rowIconRoot[index] = iconRoot.transform;

        // Name and bar form one left-aligned column; the percentage is centred on the row.
        _rowName[index] = PlaceText(row.transform, "Name", ComponentNames[index].ToUpperInvariant(),
            UITheme.Sp(RowNameSp), TextAnchor.MiddleLeft, RowTextLeft, RowBarRight, RowNameCenterY,
            FontStyle.Bold, TextPrimary);

        var barBg = UIFactory.Panel("BarBg", row.transform, BarTrackColor);
        var barImg = barBg.GetComponent<Image>();
        barImg.sprite = GetCapsuleSprite();
        barImg.type = Image.Type.Sliced;
        barImg.raycastTarget = false;
        PlaceCentered(barBg.GetComponent<RectTransform>(), RowBarLeft, RowBarRight, RowBarCenterY, UITheme.Dp(RowBarHeight));

        var fill = UIFactory.Panel("BarFill", barBg.transform, ComponentColors[index]).GetComponent<Image>();
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

        _rowPct[index] = PlaceText(row.transform, "Percent", "0%", UITheme.Sp(RowPctSp),
            TextAnchor.MiddleRight, RowBarRight, RowPctRight, 0.5f, FontStyle.Bold, TextPct);
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

        var card = UIFactory.Panel("Card", _emptyRoot.transform, RowFillColor);
        var cardImg = card.GetComponent<Image>();
        cardImg.sprite = GetRowSprite();
        cardImg.type = Image.Type.Sliced;
        cardImg.raycastTarget = false;
        var cardRt = card.GetComponent<RectTransform>();
        cardRt.anchorMin = new Vector2(CardLeft, 0.30f);
        cardRt.anchorMax = new Vector2(CardRight, 0.68f);
        cardRt.offsetMin = Vector2.zero;
        cardRt.offsetMax = Vector2.zero;

        var headline = UIFactory.Text("Headline", "No weather data available", card.transform, UITheme.Sp(28), TextAnchor.MiddleCenter, FontStyle.Bold);
        var hlRt = headline.rectTransform;
        hlRt.anchorMin = new Vector2(0.06f, 0.58f);
        hlRt.anchorMax = new Vector2(0.94f, 0.90f);
        hlRt.offsetMin = Vector2.zero;
        hlRt.offsetMax = Vector2.zero;
        headline.color = TextPrimary;
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
        hint.color = TextMeta;
        hint.raycastTarget = false;
    }

    // =====================================================================
    // Placement helpers
    // =====================================================================

    /// <summary>Anchors a rect by fractions (x left→right, y as a centre line) and gives it a fixed pixel height.</summary>
    private static void PlaceCentered(RectTransform rt, float left, float right, float centerY, float height)
    {
        rt.anchorMin = new Vector2(left, centerY);
        rt.anchorMax = new Vector2(right, centerY);
        rt.offsetMin = new Vector2(0f, -height * 0.5f);
        rt.offsetMax = new Vector2(0f, height * 0.5f);
    }

    private static Text PlaceText(Transform parent, string name, string content, int fontSize, TextAnchor anchor,
        float left, float right, float centerY, FontStyle style, Color color)
    {
        var text = UIFactory.Text(name, content, parent, fontSize, anchor, style);
        PlaceCentered(text.rectTransform, left, right, centerY, fontSize * 2f);
        text.color = color;
        text.horizontalOverflow = HorizontalWrapMode.Overflow;
        text.verticalOverflow = VerticalWrapMode.Truncate;
        text.raycastTarget = false;
        return text;
    }

    /// <summary>Blends an accent into the canvas colour — the tint used by the active row bases.</summary>
    private static Color AccentOverBg(Color accent) => new(
        BgColor.r + (accent.r - BgColor.r) * AccentRowTint,
        BgColor.g + (accent.g - BgColor.g) * AccentRowTint,
        BgColor.b + (accent.b - BgColor.b) * AccentRowTint,
        1f);

    // =====================================================================
    // Generated sprites (rounded rects / capsule / circle / donut / glow)
    // =====================================================================

    /// <summary>Rounded rect with the hero card's corner radius.</summary>
    private static Sprite GetHeroSprite() => _heroSprite ??= BuildRoundedSprite(48, HeroRadius);

    /// <summary>Rounded rect with the condition rows' corner radius.</summary>
    private static Sprite GetRowSprite() => _rowSprite ??= BuildRoundedSprite(48, RowRadius);

    /// <summary>Feathered rounded rect (soft outer edge) used for the hero halo.</summary>
    private static Sprite GetGlowSprite()
    {
        if (_glowSprite != null) return _glowSprite;
        const int size = 64;
        const float radius = 16f;
        const float feather = 6f;
        var tex = new Texture2D(size, size, TextureFormat.ARGB32, false);
        var pixels = new Color32[size * size];
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                float cx = Mathf.Clamp(x + 0.5f, radius, size - radius);
                float cy = Mathf.Clamp(y + 0.5f, radius, size - radius);
                float dist = Vector2.Distance(new Vector2(x + 0.5f, y + 0.5f), new Vector2(cx, cy));
                float a = Mathf.Clamp01((radius - dist) / feather);
                pixels[y * size + x] = new Color32(255, 255, 255, (byte)Mathf.RoundToInt(a * 255f));
            }
        }
        tex.SetPixels32(pixels);
        tex.Apply();
        float border = radius + feather;
        _glowSprite = Sprite.Create(tex, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f), 100f, 0,
            SpriteMeshType.FullRect, new Vector4(border, border, border, border));
        return _glowSprite;
    }

    /// <summary>Anti-aliased rounded rect of a given corner radius (bordered so it can be sliced).</summary>
    private static Sprite BuildRoundedSprite(int size, float radius)
    {
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
        return Sprite.Create(tex, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f), 100f, 0,
            SpriteMeshType.FullRect, new Vector4(radius, radius, radius, radius));
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

    /// <summary>
    /// Anti-aliased donut (annulus, ring thickness ~10% of the diameter) for the hero ring gauge;
    /// drawn as Image.Type.Filled / Radial360 the arc sweeps clockwise from the top.
    /// </summary>
    private static Sprite GetDonutSprite()
    {
        if (_donutSprite != null) return _donutSprite;
        const int size = (int)DonutSize;
        var tex = new Texture2D(size, size, TextureFormat.ARGB32, false);
        var pixels = new Color32[size * size];
        float half = size * 0.5f;
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                float dist = Vector2.Distance(new Vector2(x + 0.5f, y + 0.5f), new Vector2(half, half));
                float a = Mathf.Clamp01(Mathf.Min(dist - DonutInner, DonutOuter - dist) + 0.5f);
                pixels[y * size + x] = new Color32(255, 255, 255, (byte)Mathf.RoundToInt(a * 255f));
            }
        }
        tex.SetPixels32(pixels);
        tex.Apply();
        _donutSprite = Sprite.Create(tex, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f), 100f);
        return _donutSprite;
    }

    // =====================================================================
    // Shape icons (pure geometry — legacy Arial Text cannot render emoji)
    // =====================================================================

    /// <summary>
    /// Builds one shape-drawn icon per condition from plain Image rectangles and circles. Every
    /// shape is positioned in unit coordinates of a square icon area of side <paramref name="side"/>;
    /// the colours are flat white-alpha ramps that the render pass tints per state.
    /// </summary>
    private void BuildConditionIcon(Transform parent, int index, float side)
    {
        switch (index)
        {
            case 0: // Sunny — core disc + eight rays (centred on 0.5 so hero rotation stays wobble-free)
                AddIconCircle(parent, "Core", SunCoreColor, new Vector2(0.5f, 0.5f), 0.40f, side);
                for (int r = 0; r < 8; r++)
                {
                    float angle = r * 45f;
                    float rad = angle * Mathf.Deg2Rad;
                    var center = new Vector2(0.5f + Mathf.Cos(rad) * 0.36f, 0.5f + Mathf.Sin(rad) * 0.36f);
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

            case 2: // Rainy — cloud + three drop bars
                AddCloud(parent, side);
                AddIconRect(parent, "Drop1", RainDropColor, new Vector2(0.32f, 0.16f), new Vector2(0.07f, 0.20f), 8f, side);
                AddIconRect(parent, "Drop2", RainDropColor, new Vector2(0.50f, 0.12f), new Vector2(0.07f, 0.20f), 8f, side);
                AddIconRect(parent, "Drop3", RainDropColor, new Vector2(0.68f, 0.16f), new Vector2(0.07f, 0.20f), 8f, side);
                break;

            case 3: // Stormy — cloud + zigzag bolt (rotated bars)
                AddCloud(parent, side);
                AddIconRect(parent, "Bolt1", BoltColor, new Vector2(0.55f, 0.26f), new Vector2(0.26f, 0.075f), -50f, side);
                AddIconRect(parent, "Bolt2", BoltColor, new Vector2(0.43f, 0.13f), new Vector2(0.26f, 0.075f), -50f, side);
                AddIconRect(parent, "Bolt3", BoltColor, new Vector2(0.58f, 0.05f), new Vector2(0.14f, 0.075f), -50f, side);
                break;

            case 4: // Snowy — cloud + scattered dots
                AddCloud(parent, side);
                AddIconCircle(parent, "Flake1", SnowDotColor, new Vector2(0.32f, 0.16f), 0.09f, side);
                AddIconCircle(parent, "Flake2", SnowDotColor, new Vector2(0.50f, 0.10f), 0.09f, side);
                AddIconCircle(parent, "Flake3", SnowDotColor, new Vector2(0.68f, 0.16f), 0.09f, side);
                AddIconCircle(parent, "Flake4", SnowDotColor, new Vector2(0.41f, 0.01f), 0.08f, side);
                AddIconCircle(parent, "Flake5", SnowDotColor, new Vector2(0.59f, 0.01f), 0.08f, side);
                break;

            case 5: // Foggy — layered bars of varying width
                AddIconRect(parent, "Fog1", FogBarColor, new Vector2(0.50f, 0.80f), new Vector2(0.52f, 0.09f), 0f, side);
                AddIconRect(parent, "Fog2", FogBarColor, new Vector2(0.52f, 0.62f), new Vector2(0.76f, 0.09f), 0f, side);
                AddIconRect(parent, "Fog3", FogBarColor, new Vector2(0.48f, 0.44f), new Vector2(0.64f, 0.09f), 0f, side);
                AddIconRect(parent, "Fog4", FogBarColor, new Vector2(0.52f, 0.26f), new Vector2(0.82f, 0.09f), 0f, side);
                AddIconRect(parent, "Fog5", FogBarColor, new Vector2(0.48f, 0.08f), new Vector2(0.50f, 0.09f), 0f, side);
                break;

            case 6: // Windy — sweeping bars with curled ends
                AddIconRect(parent, "Wind1", WindBarColor, new Vector2(0.42f, 0.76f), new Vector2(0.56f, 0.08f), 0f, side);
                AddIconCircle(parent, "Curl1", WindBarColor, new Vector2(0.74f, 0.72f), 0.14f, side);
                AddIconRect(parent, "Wind2", WindBarColor, new Vector2(0.48f, 0.50f), new Vector2(0.72f, 0.08f), 0f, side);
                AddIconRect(parent, "Wind3", WindBarColor, new Vector2(0.40f, 0.24f), new Vector2(0.48f, 0.08f), 0f, side);
                AddIconCircle(parent, "Curl2", WindBarColor, new Vector2(0.68f, 0.20f), 0.13f, side);
                break;

            case 7: // Hail — cloud + solid stones
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

    /// <summary>Recolours every shape of a built condition icon to one flat tint, capping each shape's own alpha at the tint's alpha.</summary>
    private static void TintIconMono(Transform iconRoot, Color tint)
    {
        for (int i = 0; i < iconRoot.childCount; i++)
        {
            var img = iconRoot.GetChild(i).GetComponent<Image>();
            if (img == null) continue;
            img.color = new Color(tint.r, tint.g, tint.b, Mathf.Min(img.color.a, tint.a));
        }
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
        if (!hasData) return;

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
        Color accent = ComponentColors[dominant];

        // --- Hero: accent border, halo and pill are all derived from the dominant colour ---
        for (int i = 0; i < ComponentCount; i++)
        {
            var root = _heroIconRoots[i];
            if (root != null) root.SetActive(!clear && i == dominant);
        }

        if (clear)
        {
            _heroName.text = "CLEAR";
            _heroPct.text = "0%";
            _heroState.text = "CLEAR";
            SetRing(RingTrackColor, true, 0f);
            _heroChip.color = RowBorderColor;
            _heroRim.color = RowBorderColor;
            _heroGlow.color = new Color(BgColor.r, BgColor.g, BgColor.b, 0f);
        }
        else
        {
            int ties = 0;
            for (int i = 0; i < ComponentCount; i++)
                if (Mathf.Abs(Mathf.Clamp01(_values[i]) - max) < 0.001f) ties++;
            _heroName.text = ties > 1 ? "MIXED" : ComponentNames[dominant].ToUpperInvariant();
            _heroPct.text = PercentText(max);
            _heroState.text = max > 0.66f ? "HEAVY" : (max >= 0.33f ? "MODERATE" : "LIGHT");
            SetRing(accent, false, max);
            _heroChip.color = accent;
            _heroRim.color = accent;
            _heroGlow.color = new Color(accent.r, accent.g, accent.b, HeroGlowAlpha);
            TintIconMono(_heroIconRoots[dominant].transform, accent);
        }

        if (NetworkGuard.IsAlive(_activeLabel))
            _activeLabel.text = $"{activeCount} OF {ComponentCount} ACTIVE";

        // --- All nine components: icon always themed; accent tint/border/bar while active, neutral otherwise ---
        for (int i = 0; i < ComponentCount; i++)
        {
            float value = Mathf.Clamp01(_values[i]);
            bool active = value > ActiveEpsilon;
            Color cc = ComponentColors[i];

            _rowName[i].text = ComponentNames[i].ToUpperInvariant();
            _rowName[i].color = TextPrimary;
            _rowPct[i].text = PercentText(value);
            _rowPct[i].color = TextPct;
            _rowRim[i].color = active ? cc : RowBorderColor;
            _rowBase[i].color = active ? AccentOverBg(cc) : RowFillColor;
            _rowFill[i].color = new Color(cc.r, cc.g, cc.b, 1f);
            if (_rowIconRoot[i] != null)
                TintIconMono(_rowIconRoot[i], cc);
            _target[i] = active ? value : 0f;
        }
    }

    /// <summary>Colours the ring arc (and its round cap) and stores its target fill.</summary>
    private void SetRing(Color color, bool clear, float target)
    {
        _ringCleared = clear;
        if (_ringFill != null) _ringFill.color = color;
        if (_ringCap != null) _ringCap.color = color;
        _ringTarget = clear ? 0f : Mathf.Clamp01(target);
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
