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
/// </summary>
public sealed class WeatherApp : PhoneApp
{
    // --- Dynamic version badge from MelonInfo ---
    private static readonly string _versionString = GetVersionString();

    private static string GetVersionString()
    {
        var attr = (MelonInfoAttribute?)Attribute.GetCustomAttribute(typeof(Mod).Assembly, typeof(MelonInfoAttribute));
        return attr != null ? "v" + attr.Version : "v?";
    }

    // --- Relative Layout Anchors (fractions of the phone canvas) ---
    private const float HeaderHeight = 0.10f;
    private const float HeroTop = 0.885f;
    private const float HeroBottom = 0.595f;
    private const float SectionRowTop = 0.582f;
    private const float SectionRowBottom = 0.552f;
    private const float ListTop = 0.545f;
    private const float ListBottom = 0.062f;
    private const float FooterTop = 0.055f;
    private const float FooterBottom = 0.012f;
    private const float RowSpacing = 4f;

    // --- Component table (order matches WeatherState / the nine native components) ---
    private const int ComponentCount = 9;
    private const float ActiveEpsilon = 0.0005f;

    private static readonly string[] ComponentNames =
    {
        "Sunny", "Cloudy", "Rainy", "Stormy", "Snowy", "Foggy", "Windy", "Hail", "Sleet"
    };

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

    // --- UI Color Palette (dark panel, consistent with NotesApp) ---
    private static readonly Color BgColor = new(0.08f, 0.09f, 0.12f, 1f);
    private static readonly Color HeaderBgColor = new(0.09f, 0.10f, 0.14f, 1f);
    private static readonly Color CardBgColor = new(0.12f, 0.13f, 0.18f, 1f);
    private static readonly Color RowBgColor = new(0.135f, 0.15f, 0.20f, 1f);
    private static readonly Color RowBgDimColor = new(0.10f, 0.11f, 0.145f, 1f);
    private static readonly Color BarBgColor = new(1f, 1f, 1f, 0.10f);
    private static readonly Color DividerColor = new(1f, 1f, 1f, 0.08f);
    private static readonly Color SubtitleColor = new(0.65f, 0.72f, 0.82f, 1f);
    private static readonly Color DimColor = new(1f, 1f, 1f, 0.28f);
    private static readonly Color ClearAccentColor = new(0.45f, 0.52f, 0.62f, 1f);

    // --- UI References ---
    private GameObject _mainBG = null!;
    private GameObject _contentRoot = null!;
    private GameObject _emptyRoot = null!;
    private Text _heroName = null!;
    private Text _heroPct = null!;
    private Text _heroState = null!;
    private Image _heroAccent = null!;
    private Image _heroBarFill = null!;
    private RectTransform _heroBarFillRt = null!;
    private Text _activeLabel = null!;
    private readonly Text[] _rowName = new Text[ComponentCount];
    private readonly Text[] _rowPct = new Text[ComponentCount];
    private readonly Image[] _rowBg = new Image[ComponentCount];
    private readonly Image[] _rowFill = new Image[ComponentCount];
    private readonly RectTransform[] _rowFillRt = new RectTransform[ComponentCount];

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

        var title = UIFactory.Text("Title", "WEATHER", headerBar.transform, UITheme.Sp(34), TextAnchor.MiddleCenter, FontStyle.Bold);
        var titleRt = title.rectTransform;
        titleRt.anchorMin = Vector2.zero;
        titleRt.anchorMax = Vector2.one;
        titleRt.offsetMin = Vector2.zero;
        titleRt.offsetMax = Vector2.zero;
        title.raycastTarget = false;

        var versionBadge = UIFactory.Text("VersionBadge", _versionString, headerBar.transform, UITheme.Sp(15), TextAnchor.MiddleRight, FontStyle.Normal);
        var vbRt = versionBadge.rectTransform;
        vbRt.anchorMin = new Vector2(0.70f, 0f);
        vbRt.anchorMax = new Vector2(0.95f, 1f);
        vbRt.offsetMin = Vector2.zero;
        vbRt.offsetMax = Vector2.zero;
        versionBadge.color = new Color(1f, 1f, 1f, 0.40f);
        versionBadge.raycastTarget = false;

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
        BuildFooter(_contentRoot.transform);
    }

    private void BuildHero(Transform parent)
    {
        var hero = UIFactory.Panel("HeroCard", parent, CardBgColor);
        var heroRt = hero.GetComponent<RectTransform>();
        heroRt.anchorMin = new Vector2(0.04f, HeroBottom);
        heroRt.anchorMax = new Vector2(0.96f, HeroTop);
        heroRt.offsetMin = Vector2.zero;
        heroRt.offsetMax = Vector2.zero;
        hero.GetComponent<Image>().raycastTarget = false;

        // Left accent strip, tinted with the dominant condition's colour while rendering.
        _heroAccent = UIFactory.Panel("Accent", hero.transform, ClearAccentColor).GetComponent<Image>();
        _heroAccent.raycastTarget = false;
        var accentRt = _heroAccent.rectTransform;
        accentRt.anchorMin = new Vector2(0f, 0f);
        accentRt.anchorMax = new Vector2(0f, 1f);
        accentRt.offsetMin = Vector2.zero;
        accentRt.offsetMax = new Vector2(UITheme.Dp(5f), 0f);

        _heroName = UIFactory.Text("DominantName", "", hero.transform, UITheme.Sp(54), TextAnchor.MiddleLeft, FontStyle.Bold);
        var nameRt = _heroName.rectTransform;
        nameRt.anchorMin = new Vector2(0.055f, 0.50f);
        nameRt.anchorMax = new Vector2(0.62f, 0.92f);
        nameRt.offsetMin = Vector2.zero;
        nameRt.offsetMax = Vector2.zero;
        _heroName.horizontalOverflow = HorizontalWrapMode.Overflow;
        _heroName.verticalOverflow = VerticalWrapMode.Truncate;
        _heroName.raycastTarget = false;

        _heroPct = UIFactory.Text("DominantPercent", "", hero.transform, UITheme.Sp(54), TextAnchor.MiddleRight, FontStyle.Bold);
        var pctRt = _heroPct.rectTransform;
        pctRt.anchorMin = new Vector2(0.62f, 0.50f);
        pctRt.anchorMax = new Vector2(0.955f, 0.92f);
        pctRt.offsetMin = Vector2.zero;
        pctRt.offsetMax = Vector2.zero;
        _heroPct.raycastTarget = false;

        // Dominant-condition progress bar.
        var barBg = UIFactory.Panel("BarBg", hero.transform, BarBgColor);
        barBg.GetComponent<Image>().raycastTarget = false;
        var barRt = barBg.GetComponent<RectTransform>();
        barRt.anchorMin = new Vector2(0.055f, 0.335f);
        barRt.anchorMax = new Vector2(0.955f, 0.455f);
        barRt.offsetMin = Vector2.zero;
        barRt.offsetMax = Vector2.zero;

        _heroBarFill = UIFactory.Panel("BarFill", barBg.transform, ClearAccentColor).GetComponent<Image>();
        _heroBarFill.raycastTarget = false;
        _heroBarFillRt = _heroBarFill.rectTransform;
        _heroBarFillRt.anchorMin = Vector2.zero;
        _heroBarFillRt.anchorMax = new Vector2(0f, 1f);
        _heroBarFillRt.offsetMin = Vector2.zero;
        _heroBarFillRt.offsetMax = Vector2.zero;

        _heroState = UIFactory.Text("Intensity", "", hero.transform, UITheme.Sp(28), TextAnchor.MiddleLeft, FontStyle.Bold);
        var stateRt = _heroState.rectTransform;
        stateRt.anchorMin = new Vector2(0.055f, 0.05f);
        stateRt.anchorMax = new Vector2(0.955f, 0.30f);
        stateRt.offsetMin = Vector2.zero;
        stateRt.offsetMax = Vector2.zero;
        _heroState.raycastTarget = false;
    }

    private void BuildSectionRow(Transform parent)
    {
        var label = UIFactory.Text("SectionLabel", "ALL CONDITIONS", parent, UITheme.Sp(18), TextAnchor.MiddleLeft, FontStyle.Bold);
        var lRt = label.rectTransform;
        lRt.anchorMin = new Vector2(0.05f, SectionRowBottom);
        lRt.anchorMax = new Vector2(0.55f, SectionRowTop);
        lRt.offsetMin = Vector2.zero;
        lRt.offsetMax = Vector2.zero;
        label.color = SubtitleColor;
        label.raycastTarget = false;

        _activeLabel = UIFactory.Text("ActiveCount", "", parent, UITheme.Sp(18), TextAnchor.MiddleRight, FontStyle.Bold);
        var aRt = _activeLabel.rectTransform;
        aRt.anchorMin = new Vector2(0.55f, SectionRowBottom);
        aRt.anchorMax = new Vector2(0.95f, SectionRowTop);
        aRt.offsetMin = Vector2.zero;
        aRt.offsetMax = Vector2.zero;
        _activeLabel.color = SubtitleColor;
        _activeLabel.raycastTarget = false;
    }

    private void BuildConditionList(Transform parent)
    {
        var list = UIFactory.Panel("ConditionList", parent, Color.clear);
        list.GetComponent<Image>().raycastTarget = false;
        var listRt = list.GetComponent<RectTransform>();
        listRt.anchorMin = new Vector2(0.04f, ListBottom);
        listRt.anchorMax = new Vector2(0.96f, ListTop);
        listRt.offsetMin = Vector2.zero;
        listRt.offsetMax = Vector2.zero;

        // Even height distribution instead of scrolling: all nine conditions stay visible at any
        // resolution, so the active ones are noticeable at a glance.
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
        var row = UIFactory.Panel($"Condition_{ComponentNames[index]}", parent, RowBgColor);
        var rowImage = row.GetComponent<Image>();
        rowImage.raycastTarget = false;
        _rowBg[index] = rowImage;

        var le = row.AddComponent<LayoutElement>();
        le.minHeight = UITheme.Dp(28f);
        le.preferredHeight = UITheme.Dp(34f);
        le.flexibleWidth = 1f;
        le.flexibleHeight = 1f;

        var name = UIFactory.Text("Name", ComponentNames[index], row.transform, UITheme.Sp(19), TextAnchor.MiddleLeft, FontStyle.Bold);
        var nameRt = name.rectTransform;
        nameRt.anchorMin = new Vector2(0.035f, 0f);
        nameRt.anchorMax = new Vector2(0.32f, 1f);
        nameRt.offsetMin = Vector2.zero;
        nameRt.offsetMax = Vector2.zero;
        name.raycastTarget = false;
        _rowName[index] = name;

        var barBg = UIFactory.Panel("BarBg", row.transform, BarBgColor);
        barBg.GetComponent<Image>().raycastTarget = false;
        var barRt = barBg.GetComponent<RectTransform>();
        barRt.anchorMin = new Vector2(0.33f, 0.32f);
        barRt.anchorMax = new Vector2(0.775f, 0.68f);
        barRt.offsetMin = Vector2.zero;
        barRt.offsetMax = Vector2.zero;

        var fill = UIFactory.Panel("BarFill", barBg.transform, ComponentColors[index]).GetComponent<Image>();
        fill.raycastTarget = false;
        var fillRt = fill.rectTransform;
        fillRt.anchorMin = Vector2.zero;
        fillRt.anchorMax = new Vector2(0f, 1f);
        fillRt.offsetMin = Vector2.zero;
        fillRt.offsetMax = Vector2.zero;
        _rowFill[index] = fill;
        _rowFillRt[index] = fillRt;

        var pct = UIFactory.Text("Percent", "0%", row.transform, UITheme.Sp(19), TextAnchor.MiddleRight, FontStyle.Bold);
        var pctRt = pct.rectTransform;
        pctRt.anchorMin = new Vector2(0.79f, 0f);
        pctRt.anchorMax = new Vector2(0.965f, 1f);
        pctRt.offsetMin = Vector2.zero;
        pctRt.offsetMax = Vector2.zero;
        pct.raycastTarget = false;
        _rowPct[index] = pct;
    }

    private void BuildFooter(Transform parent)
    {
        var footer = UIFactory.Text("Footer", "Live \u00b7 updates automatically", parent, UITheme.Sp(15), TextAnchor.MiddleCenter);
        var fRt = footer.rectTransform;
        fRt.anchorMin = new Vector2(0.04f, FooterBottom);
        fRt.anchorMax = new Vector2(0.96f, FooterTop);
        fRt.offsetMin = Vector2.zero;
        fRt.offsetMax = Vector2.zero;
        footer.color = new Color(1f, 1f, 1f, 0.35f);
        footer.raycastTarget = false;
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
        card.GetComponent<Image>().raycastTarget = false;
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

        // --- Hero: dominant condition ---
        if (max <= ActiveEpsilon)
        {
            _heroName.text = "CLEAR";
            _heroPct.text = "0%";
            _heroState.text = "Clear";
            _heroPct.color = SubtitleColor;
            _heroState.color = SubtitleColor;
            _heroAccent.color = ClearAccentColor;
            _heroBarFill.color = ClearAccentColor;
            SetBarFill(_heroBarFillRt, 0f);
        }
        else
        {
            _heroName.text = ComponentNames[dominant].ToUpperInvariant();
            _heroPct.text = PercentText(max);
            _heroState.text = max > 0.66f ? "Heavy" : (max >= 0.33f ? "Moderate" : "Light");
            _heroPct.color = ComponentColors[dominant];
            _heroState.color = ComponentColors[dominant];
            _heroAccent.color = ComponentColors[dominant];
            _heroBarFill.color = ComponentColors[dominant];
            SetBarFill(_heroBarFillRt, max);
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

            _rowName[i].text = ComponentNames[i];
            _rowName[i].color = active ? Color.white : DimColor;
            _rowPct[i].text = PercentText(value);
            _rowPct[i].color = active ? ComponentColors[i] : DimColor;
            _rowBg[i].color = active ? RowBgColor : RowBgDimColor;
            _rowFill[i].color = ComponentColors[i];
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
