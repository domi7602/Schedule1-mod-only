using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Diagnostics.CodeAnalysis;
using MelonLoader;
using S1API.PhoneApp;
using S1API.UI;
using S1API.Utils;
using S1Mods.Shared;
using UnityEngine;
using UnityEngine.UI;

namespace TaxiDriver;

/// <summary>
/// "Taxi" phone app — landscape ride dashboard in the PotScanner/BankApp design language
/// (GamePalette surfaces, UISprites two-layer cards, filter chips, search band,
/// banded layout: hero / actions / filters / search / flexible destination list).
///
/// The service is operated through the phone app: CALL TAXI runs
/// <see cref="SpikeCommands.CallTaxi"/>, STOP runs <see cref="SpikeCommands.Stop"/>, and tapping a destination row runs the
/// shared <see cref="SpikeCommands.SetRideDestination"/>. The hero mirrors the live
/// <see cref="SpikeState"/> (state, drop-off, fare from <see cref="FareMeter"/>) and is
/// the only status surface — button outcomes appear there for a few seconds.
///
/// Lifecycle follows the golden rules: explicit Horizontal orientation, isolated
/// background panel (starts hidden, never destroyed), <c>OnUpdate</c> wired exactly once,
/// and an immediate close in the same frame as every other phone app (the deferred-close
/// experiment was removed 2026-10 - it made this app's ESC close lag ~0.75 s behind them).
/// One text input (the session-only destination search, typing guard per phoneapp
/// Rule 5), no persistence.
/// </summary>
public sealed class TaxiApp : S1API.PhoneApp.PhoneApp
{
    // ---- identity -----------------------------------------------------------

    /// <summary>Internal S1API app key.</summary>
    protected override string AppName => "Taxi";

    /// <summary>App title shown by the phone.</summary>
    protected override string AppTitle => "Taxi";

    /// <summary>Icon caption on the home screen.</summary>
    protected override string IconLabel => "Taxi";

    /// <summary>
    /// No filename shortcut — the icon is resolved in <see cref="IconSprite"/>
    /// (the PotScanner pattern), so a missing PNG can never make the phone log
    /// load errors.
    /// </summary>
    protected override string IconFileName => string.Empty;

    /// <summary>Landscape dashboard (the game turns the phone sideways, like PocketShop/PotScanner).</summary>
    protected override EOrientation Orientation => EOrientation.Horizontal;

    /// <summary>The app icon (shared with the fare notification — see <see cref="TaxiIcon"/>).</summary>
    protected override Sprite? IconSprite => TaxiIcon.Get();

    // ---- palette (GamePalette semantics: green = primary action, blue = selection,
    // teal = value ink, red = destructive, orange = deals) ----------------------
    private static readonly Color BgColor = GamePalette.Bg;
    private static readonly Color CardFill = GamePalette.Card;
    private static readonly Color CardFillSoft = GamePalette.CardAlt;
    private static readonly Color CardBorder = GamePalette.Border;
    private static readonly Color PrimaryAction = GamePalette.Green;
    private static readonly Color DestructiveAction = GamePalette.Red;
    private static readonly Color SelectionAccent = GamePalette.Blue;
    private static readonly Color ValueInk = GamePalette.Teal;
    private static readonly Color TextPrimary = GamePalette.TextPrimary;
    private static readonly Color TextMuted = GamePalette.TextMuted;
    private static readonly Color TextDim = GamePalette.TextDim;

    private const float CardRadius = 10f;
    private const float ChipRadius = 6f;

    // ---- destination filter keys --------------------------------------------
    private const string FilterAll = "all";
    private const string FilterHomes = "homes";
    private const string FilterDeals = "deals";
    private const string FilterPlaces = "places";
    private static readonly string[] FilterKeys = { FilterAll, FilterHomes, FilterDeals, FilterPlaces };
    private static readonly string[] FilterLabels = { "All", "Homes", "Deals", "Places" };

    // ---- UI refs ------------------------------------------------------------
    private GameObject? _mainBG;
    private RectTransform? _listContent;
    private Text? _heroTitle;
    private Text? _heroMeta;
    private Text? _heroValue;

    private Button? _callButton;
    private Image? _callFill;
    private Text? _callLabel;
    private Text? _callSub;
    private Button? _stopButton;
    private float _stopArmedUntil;
    private const float StopConfirmSeconds = 3f;
    private Image? _stopFill;
    private Text? _stopLabel;
    private Text? _stopSub;

    private readonly Dictionary<string, (Image Fill, Text Label, Button Button)> _filterChips = new();
    private Text? _countText;
    private Button? _clearButton;
    private Image? _clearFill;
    private Text? _clearLabel;

    // ---- destination search (session-only, never persisted) --------------------
    private InputField? _searchInput;
    private string _searchQuery = string.Empty;
    private GameObject? _searchClearGo;

    /// <summary>Destination rows keyed by their click key (catalog index / "STAND"), each carrying
    /// the name the highlight compares against.</summary>
    private readonly Dictionary<string, (Image Rim, Image Fill, Text Label, Text Tag, string Highlight)> _destRows = new();

    /// <summary>
    /// Package 7: catalog snapshot held per list build — taps resolve against
    /// THIS, never against a fresh scene walk with shifted indexes.
    /// </summary>
    private readonly Dictionary<string, TaxiDestinations.Destination> _destSnapshot = new();

    /// <summary>Stable row identity: kind + name + 1 m goal grid.</summary>
    private static string RowKey(TaxiDestinations.Destination destination)
    {
        Vector3 g = destination.Goal;
        return $"{destination.Kind}|{destination.Name}|{(int)Math.Round(g.x)}|{(int)Math.Round(g.y)}|{(int)Math.Round(g.z)}";
    }

    private string _activeFilter = FilterAll;

    /// <summary>One-shot per session: first app open writes the complete destination table into the log.</summary>
    private static bool _catalogDumpedThisSession;

    // ---- live state ---------------------------------------------------------
    private string _statusOverride = string.Empty;
    private float _statusOverrideUntil;
    private const float StatusOverrideSeconds = 4f;

    // Change guard: the Update loop refreshes only when the snapshot changed.
    private HeroSnapshot _heroSnapshot;
    private int _actionSignature = int.MinValue;
    private int _clearSignature = int.MinValue;

    private float _nameColumnWidth;
    private int _refitFrames;
    private int _closeTraceFrames;
    private readonly List<(string Name, float Width)> _rowWidths = new();

    // ---- lifecycle ----------------------------------------------------------

    /// <summary>
    /// Defensive, idempotent update-hook wiring. Closing the phone only hides the
    /// app; the hook is removed when S1API destroys the app instance.
    /// </summary>
    protected override void OnCreated()
    {
        base.OnCreated();

        MelonEvents.OnUpdate.Unsubscribe(Update);
        MelonEvents.OnUpdate.Subscribe(Update);
    }

    protected override void OnDestroyed()
    {
        MelonEvents.OnUpdate.Unsubscribe(Update);
        base.OnDestroyed();
    }

    /// <summary>Builds the landscape dashboard (background panel starts hidden).</summary>
    protected override void OnCreatedUI(GameObject container)
    {
        var containerRt = container.GetComponent<RectTransform>();
        if (containerRt != null)
        {
            UITheme.InitializeForDashboard(containerRt);
            TaxiLog.Verbose($"[ui] canvas {UITheme.ActualWidth:F0}x{UITheme.ActualHeight:F0} scale={UITheme.Scale:F2}");
        }

        // Golden Rule 3: isolated background panel, starts hidden.
        _mainBG = UIFactory.Panel("TaxiApp_MainBG", container.transform, BgColor, fullAnchor: true);
        _mainBG.SetActive(false);

        // Banded stack (PotScanner idiom): hero / action row / filter toolbar / flexible list.
        var vlg = _mainBG.AddComponent<VerticalLayoutGroup>();
        vlg.childControlHeight = true;
        vlg.childControlWidth = true;
        vlg.childForceExpandHeight = false;
        vlg.childForceExpandWidth = true;
        vlg.spacing = UITheme.Dp(8f);
        int pad = (int)UITheme.Dp(8f);
        vlg.padding = new RectOffset(pad, pad, pad, pad);

        CreateHero(_mainBG.transform);
        CreateActionRow(_mainBG.transform);
        CreateFilterToolbar(_mainBG.transform);
        CreateSearchBand(_mainBG.transform);
        CreateList(_mainBG.transform);

        RefreshHero();
        RefreshActionButtons();
        RefreshClearButton();
    }

    /// <summary>IL2CPP liveness: managed wrappers survive scene unload while native objects are dead.</summary>
    private static bool IsAlive([NotNullWhen(true)] UnityEngine.Object? obj)
    {
        if (obj == null) return false;
        try { return obj.Pointer != IntPtr.Zero && !obj.WasCollected && (UnityEngine.Object)obj != null; }
        catch { return false; }
    }

    protected override void OnPhoneClosed()
    {
        TaxiLog.Verbose($"[close] OnPhoneClosed f={Time.frameCount} t={Time.unscaledTime:0.000}");
        // Hide immediately, like every other phone app. The deferred-close experiment
        // (patched S1API SetAppOpen, HideDelayFrames=45) made the Taxi app linger ~0.75 s
        // behind the others on ESC - removed, the close path is now the plain S1API one.
        base.OnPhoneClosed();
        if (IsAlive(_mainBG))
            _mainBG.SetActive(false);
    }

    /// <summary>
    /// Per-frame sync: background visibility follows <see cref="IsOpen"/>, Escape closes
    /// the app, and the cheap change-guarded refreshes keep hero/actions/list in sync
    /// with <see cref="SpikeState"/>.
    /// </summary>
    private void Update()
    {
        if (!IsAlive(_mainBG))
            return;

        bool open = IsOpen();
        // Visibility follows IsOpen() in BOTH directions. _mainBG starts inactive
        // (Golden Rule 3) and this sync is the only thing that ever shows it, so the
        // OPEN direction must never be gated by a close-path experiment.
        if (_mainBG.activeSelf != open)
        {
            _mainBG.SetActive(open);

            // The destination catalog walks the scene (deal locations + lots), so it
            // is built on open — never per frame.
            if (open)
            {
                RebuildDestinationList();
                _refitFrames = 5;   // labels settle one layout pass after the rebuild
            }
            else
            {
                _closeTraceFrames = 8;   // [close] frame-gap trace (verbose)
            }
        }

        if (_refitFrames > 0 && --_refitFrames == 0)
        {
            FitHeroTitle();
            RefitLabelsAtRender();
        }

        if (_closeTraceFrames > 0)
        {
            _closeTraceFrames--;
            TaxiLog.Verbose($"[close] trace f={Time.frameCount} t={Time.unscaledTime:0.000} open={open} mainBG={_mainBG.activeSelf}");
        }

        if (!open)
            return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Search feature: the first Escape clears the search, the second closes.
            if (_searchQuery.Trim().Length > 0)
            {
                SetSearchQuery(string.Empty);
                return;
            }

            CloseApp();
            return;
        }

        RefreshHero();
        RefreshActionButtons();
        RefreshClearButton();
        RefreshDestinationHighlight();
    }

    // =====================================================================
    // UI construction
    // =====================================================================

    /// <summary>Hero band (Dp96): live state, context, fare, app icon.</summary>
    private void CreateHero(Transform parent)
    {
        var band = UIFactory.Panel("HeroBand", parent, Color.clear);
        var bandImg = band.GetComponent<Image>();
        if (bandImg != null) bandImg.raycastTarget = false;
        var le = band.AddComponent<LayoutElement>();
        le.minHeight = UITheme.Dp(96f);
        le.preferredHeight = UITheme.Dp(96f);
        le.flexibleHeight = 0f;

        var card = CreateCard("HeroCard", band.transform, CardFill, out var fill);

        var overline = UIFactory.Text("HeroOverline", "TAXI", fill.transform, UITheme.Sp(12), TextAnchor.MiddleLeft, FontStyle.Bold);
        overline.color = TextMuted;
        overline.raycastTarget = false;
        AnchorRect(overline.rectTransform, 0.05f, 0.76f, 0.68f, 0.94f);

        _heroTitle = UIFactory.Text("HeroTitle", "CALL A TAXI", fill.transform, UITheme.Sp(26), TextAnchor.MiddleLeft, FontStyle.Bold);
        _heroTitle.color = TextPrimary;
        _heroTitle.raycastTarget = false;
        _heroTitle.horizontalOverflow = HorizontalWrapMode.Overflow;
        _heroTitle.verticalOverflow = VerticalWrapMode.Truncate;
        AnchorRect(_heroTitle.rectTransform, 0.05f, 0.46f, 0.68f, 0.78f);

        _heroMeta = UIFactory.Text("HeroMeta", "Orders a cab to your position", fill.transform, UITheme.Sp(11), TextAnchor.MiddleLeft);
        _heroMeta.color = TextMuted;
        _heroMeta.raycastTarget = false;
        _heroMeta.horizontalOverflow = HorizontalWrapMode.Overflow;
        _heroMeta.verticalOverflow = VerticalWrapMode.Truncate;
        AnchorRect(_heroMeta.rectTransform, 0.05f, 0.25f, 0.70f, 0.44f);

        _heroValue = UIFactory.Text("HeroValue", string.Empty, fill.transform, UITheme.Sp(12), TextAnchor.MiddleLeft, FontStyle.Bold);
        _heroValue.color = ValueInk;
        _heroValue.raycastTarget = false;
        _heroValue.horizontalOverflow = HorizontalWrapMode.Overflow;
        _heroValue.verticalOverflow = VerticalWrapMode.Truncate;
        AnchorRect(_heroValue.rectTransform, 0.05f, 0.05f, 0.70f, 0.24f);

        // Right side: the app icon (brand anchor) in the slot PotScanner uses for its donut.
        float iconSide = Mathf.Min(UITheme.Dp(96f) * 0.62f, UITheme.ActualWidth * 0.14f);
        var iconGo = UIFactory.Panel("HeroIcon", fill.transform, Color.white);
        var iconImg = iconGo.GetComponent<Image>();
        if (iconImg != null)
        {
            iconImg.sprite = TaxiIcon.Get();
            iconImg.preserveAspect = true;
            iconImg.raycastTarget = false;
        }
        var iconRt = iconGo.GetComponent<RectTransform>();
        iconRt.anchorMin = new Vector2(0.85f, 0.5f);
        iconRt.anchorMax = iconRt.anchorMin;
        iconRt.sizeDelta = new Vector2(iconSide, iconSide);
    }

    /// <summary>Action band (Dp48): CALL TAXI (primary, green) + STOP (destructive, red).</summary>
    private void CreateActionRow(Transform parent)
    {
        var panel = UIFactory.Panel("ActionRow", parent, Color.clear);
        var img = panel.GetComponent<Image>();
        if (img != null) img.raycastTarget = false;
        var le = panel.AddComponent<LayoutElement>();
        le.minHeight = UITheme.Dp(48f);
        le.preferredHeight = UITheme.Dp(48f);
        le.flexibleHeight = 0f;

        var hlg = panel.AddComponent<HorizontalLayoutGroup>();
        hlg.spacing = UITheme.Dp(8f);
        hlg.childControlWidth = true;
        hlg.childControlHeight = true;
        hlg.childForceExpandWidth = true;
        hlg.childForceExpandHeight = true;

        var callCard = CreateCard("CallPanel", panel.transform, PrimaryAction, out _callFill, raycastTarget: true, radius: ChipRadius);
        _callButton = callCard.AddComponent<Button>();
        _callButton.transition = Selectable.Transition.None;
        _callLabel = UIFactory.Text("CallLbl", "CALL TAXI", _callFill.transform, UITheme.Sp(13), TextAnchor.MiddleCenter, FontStyle.Bold);
        _callLabel.color = Color.white;
        _callLabel.raycastTarget = false;
        AnchorRect(_callLabel.rectTransform, 0.06f, 0.46f, 0.94f, 0.95f);
        _callSub = UIFactory.Text("CallSubLbl", "to your position", _callFill.transform, UITheme.Sp(10), TextAnchor.MiddleCenter, FontStyle.Bold);
        _callSub.color = Color.white;
        _callSub.raycastTarget = false;
        AnchorRect(_callSub.rectTransform, 0.06f, 0.06f, 0.94f, 0.42f);
        ButtonUtils.AddListener(_callButton, OnCallTaxiPressed);

        var stopCard = CreateCard("StopPanel", panel.transform, CardFillSoft, out _stopFill, raycastTarget: true, radius: ChipRadius);
        _stopButton = stopCard.AddComponent<Button>();
        _stopButton.transition = Selectable.Transition.None;
        _stopLabel = UIFactory.Text("StopLbl", "STOP", _stopFill.transform, UITheme.Sp(13), TextAnchor.MiddleCenter, FontStyle.Bold);
        _stopLabel.color = TextDim;
        _stopLabel.raycastTarget = false;
        AnchorRect(_stopLabel.rectTransform, 0.06f, 0.46f, 0.94f, 0.95f);
        _stopSub = UIFactory.Text("StopSubLbl", "cancel & despawn", _stopFill.transform, UITheme.Sp(10), TextAnchor.MiddleCenter, FontStyle.Bold);
        _stopSub.color = TextDim;
        _stopSub.raycastTarget = false;
        AnchorRect(_stopSub.rectTransform, 0.06f, 0.06f, 0.94f, 0.42f);
        ButtonUtils.AddListener(_stopButton, OnStopPressed);
    }

    /// <summary>Filter band (Dp28): 4 category chips + place count + clear chip.</summary>
    private void CreateFilterToolbar(Transform parent)
    {
        var panel = UIFactory.Panel("FilterToolbar", parent, Color.clear);
        var img = panel.GetComponent<Image>();
        if (img != null) img.raycastTarget = false;
        var le = panel.AddComponent<LayoutElement>();
        le.minHeight = UITheme.Dp(28f);
        le.preferredHeight = UITheme.Dp(28f);
        le.flexibleHeight = 0f;

        var hlg = panel.AddComponent<HorizontalLayoutGroup>();
        hlg.spacing = UITheme.Dp(8f);
        hlg.childControlWidth = true;
        hlg.childControlHeight = true;
        hlg.childForceExpandWidth = false;
        hlg.childForceExpandHeight = true;

        _filterChips.Clear();
        for (int i = 0; i < FilterKeys.Length; i++)
        {
            string key = FilterKeys[i];
            var chip = CreateCard($"Filter_{key}", panel.transform, CardFill, out var chipFill, raycastTarget: true, radius: ChipRadius);
            var chipLe = chip.AddComponent<LayoutElement>();
            chipLe.flexibleWidth = 1f;   // the four chips divide the free width
            chipLe.minWidth = UITheme.Dp(44f);
            chipLe.flexibleHeight = 1f;

            var btn = chip.AddComponent<Button>();
            btn.transition = Selectable.Transition.None;
            var lbl = UIFactory.Text($"Filter_{key}_Lbl", FilterLabels[i], chipFill.transform, UITheme.Sp(11), TextAnchor.MiddleCenter, FontStyle.Bold);
            lbl.color = TextMuted;
            lbl.raycastTarget = false;
            AnchorRect(lbl.rectTransform, 0.06f, 0f, 0.94f, 1f);

            _filterChips[key] = (chipFill, lbl, btn);
            string captured = key;
            ButtonUtils.AddListener(btn, () => SetFilter(captured));
        }

        _countText = UIFactory.Text("PlaceCount", string.Empty, panel.transform, UITheme.Sp(10), TextAnchor.MiddleRight);
        _countText.color = TextMuted;
        _countText.raycastTarget = false;
        var countLe = _countText.gameObject.AddComponent<LayoutElement>();
        countLe.preferredWidth = UITheme.Dp(64f);
        countLe.minWidth = UITheme.Dp(52f);
        countLe.flexibleWidth = 0f;

        var clearCard = CreateCard("ClearPanel", panel.transform, CardFill, out _clearFill, raycastTarget: true, radius: ChipRadius);
        var clearLe = clearCard.AddComponent<LayoutElement>();
        clearLe.preferredWidth = UITheme.Dp(58f);
        clearLe.minWidth = UITheme.Dp(48f);
        clearLe.flexibleWidth = 0f;
        _clearButton = clearCard.AddComponent<Button>();
        _clearButton.transition = Selectable.Transition.None;
        _clearLabel = UIFactory.Text("ClearLbl", "✕ clear", _clearFill.transform, UITheme.Sp(10), TextAnchor.MiddleCenter, FontStyle.Bold);
        _clearLabel.color = TextDim;
        _clearLabel.raycastTarget = false;
        AnchorRect(_clearLabel.rectTransform, 0.03f, 0f, 0.97f, 1f);
        ButtonUtils.AddListener(_clearButton, OnClearPressed);

        RefreshFilterChips();
    }

    /// <summary>
    /// Search band (Dp32): live destination search over name and type tag
    /// (<see cref="DestinationFilter"/>), combined with the filter chips above.
    /// Session-only (never persisted); typing is guarded by
    /// <see cref="TaxiAppInputFocus"/> so player movement is not hijacked while typing.
    /// TaxiDriver itself installs no keyboard bindings (phoneapp Rule 5/17).
    /// </summary>
    private void CreateSearchBand(Transform parent)
    {
        var band = UIFactory.Panel("SearchBand", parent, Color.clear);
        var bandImg = band.GetComponent<Image>();
        if (bandImg != null) bandImg.raycastTarget = false;
        var bandLe = band.AddComponent<LayoutElement>();
        bandLe.minHeight = UITheme.Dp(32f);
        bandLe.preferredHeight = UITheme.Dp(32f);
        bandLe.flexibleHeight = 0f;

        var hlg = band.AddComponent<HorizontalLayoutGroup>();
        hlg.spacing = UITheme.Dp(8f);
        hlg.childControlWidth = true;
        hlg.childControlHeight = true;
        hlg.childForceExpandWidth = false;
        hlg.childForceExpandHeight = true;

        var card = CreateCard("SearchPanel", band.transform, CardFill, out Image fill, raycastTarget: true, radius: ChipRadius);
        var cardLe = card.AddComponent<LayoutElement>();
        cardLe.flexibleWidth = 1f;
        cardLe.minWidth = UITheme.Dp(120f);
        cardLe.flexibleHeight = 1f;

        // Rule 17: the field surface must stay clickable - the card image (created
        // with raycastTarget: true) is the InputField's target graphic.
        _searchInput = card.AddComponent<InputField>();
        _searchInput.transition = Selectable.Transition.None;
        _searchInput.targetGraphic = card.GetComponent<Graphic>();
        _searchInput.lineType = InputField.LineType.SingleLine;
        _searchInput.caretWidth = 2;
        _searchInput.caretColor = TextPrimary;
        _searchInput.selectionColor = new Color(0.22f, 0.45f, 0.90f, 0.30f);

        var text = UIFactory.Text("SearchText", string.Empty, fill.transform, UITheme.Sp(11), TextAnchor.MiddleLeft);
        text.color = TextPrimary;
        text.supportRichText = false;
        text.raycastTarget = false;
        AnchorRect(text.rectTransform, 0.05f, 0f, 0.95f, 1f);

        var placeholder = UIFactory.Text("SearchPlaceholder", "Search destinations (name or type)", fill.transform, UITheme.Sp(11), TextAnchor.MiddleLeft);
        placeholder.color = TextDim;
        placeholder.supportRichText = false;
        placeholder.raycastTarget = false;
        AnchorRect(placeholder.rectTransform, 0.05f, 0f, 0.95f, 1f);

        _searchInput.textComponent = text;
        _searchInput.placeholder = placeholder;

        // Clear chip beside the field (hidden while the query is empty).
        var clearCard = CreateCard("SearchClearPanel", band.transform, CardFill, out Image clearFill, raycastTarget: true, radius: ChipRadius);
        var clearLe = clearCard.AddComponent<LayoutElement>();
        clearLe.preferredWidth = UITheme.Dp(40f);
        clearLe.minWidth = UITheme.Dp(36f);
        clearLe.flexibleWidth = 0f;
        var clearButton = clearCard.AddComponent<Button>();
        clearButton.transition = Selectable.Transition.None;
        var clearLabel = UIFactory.Text("SearchClearLbl", "X", clearFill.transform, UITheme.Sp(10), TextAnchor.MiddleCenter, FontStyle.Bold);
        clearLabel.color = TextDim;
        clearLabel.raycastTarget = false;
        AnchorRect(clearLabel.rectTransform, 0.03f, 0f, 0.97f, 1f);
        ButtonUtils.AddListener(clearButton, OnSearchClearPressed);
        _searchClearGo = clearCard;
        _searchClearGo.SetActive(false);

        EventHelper.RemoveListener(OnSearchQueryChanged, _searchInput.onValueChanged);
        EventHelper.AddListener(OnSearchQueryChanged, _searchInput.onValueChanged);

        // Rule 5: the typing guard lives on the band and dies with it (OnDisable
        // resets Controls.IsTyping).
        var focusHook = band.AddComponent<TaxiAppInputFocus>();
        focusHook.searchInput = _searchInput;
    }

    /// <summary>Search text changed (live filter).</summary>
    private void OnSearchQueryChanged(string value) => SetSearchQuery(value, fromField: true);

    /// <summary>Clear chip: empties the search field and shows the full list again.</summary>
    private void OnSearchClearPressed() => SetSearchQuery(string.Empty);

    /// <summary>
    /// Single write path for the search query: updates the field (unless the
    /// change came FROM the field), toggles the clear chip and rebuilds the list.
    /// Setting <c>.text</c> re-fires onValueChanged - the equality guard makes
    /// that a no-op.
    /// </summary>
    private void SetSearchQuery(string value, bool fromField = false)
    {
        string normalized = value ?? string.Empty;
        if (string.Equals(_searchQuery, normalized, StringComparison.Ordinal))
            return;
        _searchQuery = normalized;

        if (!fromField && _searchInput != null && IsAlive(_searchInput))
            _searchInput.text = normalized;
        if (_searchClearGo != null)
            _searchClearGo.SetActive(normalized.Trim().Length > 0);

        RebuildDestinationList();
    }

    /// <summary>Flexible list band: scrollable destination rows (RectMask2D-clipped).</summary>
    private void CreateList(Transform parent)
    {
        var listPanel = UIFactory.Panel("DestList", parent, Color.clear);
        var listImg = listPanel.GetComponent<Image>();
        if (listImg != null) listImg.raycastTarget = false;
        var listLe = listPanel.AddComponent<LayoutElement>();
        listLe.flexibleHeight = 1f;

        if (listPanel.GetComponent<RectMask2D>() == null)
            listPanel.AddComponent<RectMask2D>();

        _listContent = UIFactory.ScrollableVerticalList("DestListScroll", listPanel.transform, out var scroll);
        UIFactory.FitContentHeight(_listContent);
        _listContent.sizeDelta = new Vector2(0f, _listContent.sizeDelta.y);

        // Content width = VIEWPORT width (2026-09-29 fix, kept in the landscape rebuild):
        // row math must match the visible width exactly.
        RectTransform contentRt = _listContent;
        contentRt.anchorMin = new Vector2(0f, contentRt.anchorMin.y);
        contentRt.anchorMax = new Vector2(1f, contentRt.anchorMax.y);
        contentRt.offsetMin = new Vector2(0f, contentRt.offsetMin.y);
        contentRt.offsetMax = new Vector2(0f, contentRt.offsetMax.y);

        var vlg = _listContent.GetComponent<VerticalLayoutGroup>();
        if (vlg != null)
        {
            vlg.spacing = UITheme.Dp(5f);
            vlg.padding = new RectOffset(0, 0, (int)UITheme.Dp(2f), (int)UITheme.Dp(6f));
            vlg.childControlWidth = true;
            vlg.childForceExpandWidth = true;
        }

        if (scroll != null)
        {
            scroll.vertical = true;
            scroll.movementType = ScrollRect.MovementType.Clamped;
        }

        _nameColumnWidth = UITheme.ActualWidth - 2f * UITheme.Dp(8f) - 2f * UITheme.Dp(10f) - UITheme.Dp(60f) - UITheme.Dp(8f);
    }

    // =====================================================================
    // Destination list
    // =====================================================================

    /// <summary>
    /// Rebuilds the destination list. Called once per app open (the catalog walks the
    /// scene — never per frame) and on every filter change. The Taxi-Stand stays pinned
    /// first in every filter (proven routable — the taxi spawns on that street entry).
    /// </summary>
    private void RebuildDestinationList()
    {
        if (_listContent == null)
            return;

        UIFactory.ClearChildren(_listContent);
        _destRows.Clear();
        _destSnapshot.Clear();
        _rowWidths.Clear();

        string query = _searchQuery.Trim();
        string standName = $"Taxi-Stand ({TaxiStand.StandName})";
        int places = 0;
        int visible = 0; // rows the active chips show WITHOUT the search query

        // The stand row is chip-independent; only the search can hide it.
        visible++;
        if (DestinationFilter.Matches(query, standName, "STAND"))
        {
            MakeDestinationRow("STAND", standName, "STAND", "Taxi-Stand");
            places++;
        }

        if (!TaxiDestinations.BuildCatalog(out List<TaxiDestinations.Destination> catalog, "TaxiApp list"))
        {
            MakeDestinationRow("NONE", "No places found — is a save loaded?", string.Empty, string.Empty);
            if (IsAlive(_countText))
                _countText.text = "—";
        }
        else
        {
            if (!_catalogDumpedThisSession)
            {
                _catalogDumpedThisSession = true;
                TaxiDestinations.DumpPois("TaxiApp first open — full destination dump (one-shot per session)");
            }

            // Package 7: the row key is a stable identity (kind + name + 1 m goal
            // grid) into the snapshot held above — never a catalog index, which
            // shifts when the scene changes between list build and tap.
            foreach (TaxiDestinations.Destination destination in catalog)
            {
                if (!MatchesFilter(destination))
                    continue;

                visible++;
                if (!DestinationFilter.Matches(query, destination.Name, destination.Tag))
                    continue;

                string rowKey = RowKey(destination);
                if (_destSnapshot.ContainsKey(rowKey))
                    rowKey += "#" + destination.Index; // twins within 1 m stay tappable
                _destSnapshot[rowKey] = destination;
                MakeDestinationRow(rowKey, destination.Name, destination.Tag, destination.Name);
                places++;
            }

            if (places == 0 && query.Length > 0)
                MakeDestinationRow("NOMATCH", $"No destination matches '{query}'", string.Empty, string.Empty);

            if (IsAlive(_countText))
                _countText.text = query.Length > 0 ? $"{places} of {visible}" : $"{places} places";
        }

        UIFactory.FitContentHeight(_listContent);
        _refitFrames = 3;
        ReportLabelWidths();
        RefreshDestinationHighlight();
    }

    private bool MatchesFilter(TaxiDestinations.Destination destination)
    {
        switch (_activeFilter)
        {
            case FilterHomes: return destination.Kind == TaxiDestinations.KindProperty;
            case FilterDeals: return destination.Kind == TaxiDestinations.KindDeal;
            case FilterPlaces: return destination.Kind == TaxiDestinations.KindLot;
            default: return true;
        }
    }

    /// <summary>
    /// One destination row (tap = pick): two-layer card, name (computed fit) + type tag.
    /// Selection restyles the card from the highlight pass — the row is never rebuilt for it.
    /// </summary>
    private void MakeDestinationRow(string key, string name, string tag, string highlight)
    {
        if (_listContent == null)
            return;

        var row = CreateCard($"Dest_{key}", _listContent, CardFill, out var fill, raycastTarget: true, radius: CardRadius);
        var le = row.AddComponent<LayoutElement>();
        le.preferredHeight = UITheme.Dp(44f);
        le.minHeight = UITheme.Dp(30f);
        var btn = row.AddComponent<Button>();
        btn.transition = Selectable.Transition.None;

        var hlg = fill.gameObject.AddComponent<HorizontalLayoutGroup>();
        hlg.padding = new RectOffset((int)UITheme.Dp(10f), (int)UITheme.Dp(10f), 0, 0);
        hlg.spacing = UITheme.Dp(8f);
        hlg.childAlignment = TextAnchor.MiddleLeft;
        hlg.childControlWidth = true;
        hlg.childControlHeight = true;
        hlg.childForceExpandWidth = false;
        hlg.childForceExpandHeight = true;

        int nameSize = UITheme.Sp(15);
        var nameTxt = UIFactory.Text($"Dest_{key}_Name", name, fill.transform, nameSize, TextAnchor.MiddleLeft, FontStyle.Bold);
        nameTxt.color = TextPrimary;
        nameTxt.raycastTarget = false;
        nameTxt.horizontalOverflow = HorizontalWrapMode.Overflow;
        nameTxt.verticalOverflow = VerticalWrapMode.Truncate;
        nameTxt.gameObject.AddComponent<LayoutElement>().flexibleWidth = 1f;   // name takes the rest
        MakeFixedFitting(nameTxt, nameSize, UITheme.Sp(11), _nameColumnWidth);
        _rowWidths.Add((name, nameTxt.preferredWidth));

        int tagSize = UITheme.Sp(10);
        var tagTxt = UIFactory.Text($"Dest_{key}_Tag", tag, fill.transform, tagSize, TextAnchor.MiddleRight, FontStyle.Bold);
        tagTxt.color = TagColor(tag);
        tagTxt.raycastTarget = false;
        var tagLe = tagTxt.gameObject.AddComponent<LayoutElement>();
        tagLe.preferredWidth = UITheme.Dp(60f);
        tagLe.minWidth = UITheme.Dp(44f);
        tagLe.flexibleWidth = 0f;   // tag keeps its width, the name gets the rest

        string captured = key;
        ButtonUtils.AddListener(btn, () => OnDestinationPressed(captured));
        _destRows[captured] = (row.GetComponent<Image>(), fill, nameTxt, tagTxt, highlight);
    }

    /// <summary>Type tag ink (GamePalette hues; selection never changes it).</summary>
    private static Color TagColor(string tag) => tag switch
    {
        "STAND" => SelectionAccent,
        "HOME" => PrimaryAction,
        "PROP" => ValueInk,
        "DEAL" => GamePalette.Orange,
        _ => TextMuted,
    };

    /// <summary>Highlight pass: the picked destination gets the blue selection rim + raised fill.</summary>
    private void RefreshDestinationHighlight()
    {
        string selected = SpikeState.RideDestinationName;

        foreach (KeyValuePair<string, (Image Rim, Image Fill, Text Label, Text Tag, string Highlight)> entry in _destRows)
        {
            bool isSelected = entry.Value.Highlight.Length > 0 &&
                              string.Equals(entry.Value.Highlight, selected, StringComparison.Ordinal);

            // Change guards only: an unconditional Set forces a canvas rebuild for every row.
            Color rim = isSelected ? SelectionAccent : CardBorder;
            Color fill = isSelected ? CardFillSoft : CardFill;
            if (IsAlive(entry.Value.Rim) && entry.Value.Rim.color != rim)
                entry.Value.Rim.color = rim;
            if (IsAlive(entry.Value.Fill) && entry.Value.Fill.color != fill)
                entry.Value.Fill.color = fill;
        }
    }

    // =====================================================================
    // Filters / clear / actions
    // =====================================================================

    private void SetFilter(string key)
    {
        if (_activeFilter == key)
            return;

        _activeFilter = key;
        RefreshFilterChips();
        RebuildDestinationList();
        RefreshHero();
    }

    private void RefreshFilterChips()
    {
        foreach (KeyValuePair<string, (Image Fill, Text Label, Button Button)> chip in _filterChips)
        {
            bool isActive = chip.Key == _activeFilter;

            // BankApp precedent: selected = AccentBlue fill with WHITE ink, unselected =
            // neutral surface with muted ink.
            Color fillColor = isActive ? SelectionAccent : CardFill;
            Color labelColor = isActive ? Color.white : TextMuted;
            if (IsAlive(chip.Value.Fill) && chip.Value.Fill.color != fillColor)
                chip.Value.Fill.color = fillColor;
            if (IsAlive(chip.Value.Label) && chip.Value.Label.color != labelColor)
                chip.Value.Label.color = labelColor;
        }
    }

    /// <summary>Clear chip: enabled exactly while a destination is picked.</summary>
    private void RefreshClearButton()
    {
        if (!IsAlive(_clearFill))
            return;

        bool picked = SpikeState.RideDestinationPicked;
        int signature = picked ? 1 : 0;
        if (signature == _clearSignature)
            return;
        _clearSignature = signature;

        if (_clearButton != null)
            _clearButton.interactable = picked;
        Color fill = picked ? CardFillSoft : CardFill;
        if (_clearFill!.color != fill)
            _clearFill.color = fill;
        if (_clearLabel != null)
        {
            Color labelColor = picked ? TextPrimary : TextDim;
            if (_clearLabel.color != labelColor)
                _clearLabel.color = labelColor;
        }
    }

    /// <summary>Action band states: CALL is the primary action (blocked outside gameplay);
    /// STOP is enabled exactly while a taxi exists or a ride runs.</summary>
    private void RefreshActionButtons()
    {
        if (!IsAlive(_callFill) || !IsAlive(_stopFill))
            return;

        bool inScene = SceneGate.IsInMainScene;
        bool carExists = SpikeState.Vehicle != null || SpikeState.PendingSpawnCode != null;
        bool stopEnabled = inScene && (carExists || SpikeState.RideActive);

        int signature = (inScene ? 1 : 0) | (carExists ? 2 : 0) | (stopEnabled ? 4 : 0);
        if (signature == _actionSignature)
            return;
        _actionSignature = signature;

        SetActionVisual(_callButton, _callFill, _callLabel, _callSub, inScene, PrimaryAction,
            "CALL TAXI", inScene ? "to your position" : "only in gameplay");
        SetActionVisual(_stopButton, _stopFill, _stopLabel, _stopSub, stopEnabled, DestructiveAction,
            "STOP", stopEnabled ? "cancel & despawn" : "no active taxi");
    }

    private static void SetActionVisual(Button? btn, Image? fill, Text? label, Text? sub,
        bool enabled, Color enabledFill, string labelText, string subText)
    {
        if (btn != null && btn.interactable != enabled)
            btn.interactable = enabled;
        if (fill != null)
        {
            Color color = enabled ? enabledFill : CardFillSoft;
            if (fill.color != color)
                fill.color = color;
        }
        if (label != null)
        {
            if (label.text != labelText) label.text = labelText;
            Color color = enabled ? Color.white : TextDim;
            if (label.color != color) label.color = color;
        }
        if (sub != null)
        {
            if (sub.text != subText) sub.text = subText;
            Color color = enabled ? Color.white : TextDim;
            if (sub.color != color) sub.color = color;
        }
    }

    // =====================================================================
    // Hero (live status)
    // =====================================================================

    /// <summary>
    /// Live <see cref="SpikeState"/> (and the fare) → hero texts. Change-guarded by a
    /// cheap signature, so an idle screen costs one comparison per frame.
    /// </summary>
    private void RefreshHero()
    {
        if (!IsAlive(_heroTitle) || !IsAlive(_heroMeta) || !IsAlive(_heroValue))
            return;

        bool overrideActive = _statusOverride.Length > 0 && Time.unscaledTime < _statusOverrideUntil;
        string overrideText = overrideActive ? _statusOverride : string.Empty;
        string destination = SpikeState.RideDestinationName;

        var snapshot = new HeroSnapshot(
            SpikeState.RideActive,
            SpikeState.RideAwaitingDestination,
            SpikeState.RideArrived,
            SpikeState.NavGaveUp,
            SpikeState.RideDestinationPicked,
            SpikeState.RideAwaitingBoard,
            SpikeState.AutoToPlayer,
            SpikeState.AutoRunning,
            SpikeState.PendingSpawnCode != null,
            SpikeState.Vehicle != null,
            SpikeState.NavToPlayer && SpikeState.PollingActive,
            destination,
            FareMeter.CalculatedTotal,
            overrideText);
        if (snapshot == _heroSnapshot)
            return;
        _heroSnapshot = snapshot;

        string title;
        string meta;
        string value = string.Empty;
        Color valueColor = TextMuted;

        if (SpikeState.RideActive)
        {
            if (SpikeState.RideAwaitingDestination)
            {
                title = "ON BOARD";
                meta = "Pick a destination below";
            }
            else if (SpikeState.RideArrived)
            {
                title = SpikeState.NavGaveUp ? "COULDN'T GET THERE" : "ARRIVED";
                meta = SpikeState.NavGaveUp
                    ? "Press E to get out — or STOP to despawn"
                    : "Press E to exit the taxi";
                value = $"FARE ${FareMeter.CalculatedTotal}";
                valueColor = ValueInk;
            }
            else
            {
                // Package 7: while driving, the hero names the ACTIVE trip —
                // the picker may have been cleared or re-picked since dispatch.
                string trip = SpikeState.ActiveTripPoint.HasValue && SpikeState.ActiveTripName.Length > 0
                    ? SpikeState.ActiveTripName
                    : destination;
                bool showing = SpikeState.RideDestinationPicked || trip != destination;
                title = showing ? $"RIDING TO {trip.ToUpperInvariant()}" : "RIDING";
                string dropOff = ShortDropOff(SpikeState.RideDropOff);
                meta = showing
                    ? (string.IsNullOrEmpty(dropOff) ? "On the way" : $"Drop-off: {dropOff}")
                    : "On the way";
                value = $"FARE ${FareMeter.CalculatedTotal}";
                valueColor = ValueInk;
            }
        }
        else if (SpikeState.RideAwaitingBoard)
        {
            title = "BOARD NOW";
            meta = "Press E to get in";
            value = SpikeState.RideDestinationPicked ? $"TO {destination}" : "Or pick a destination below";
        }
        else if (SpikeState.AutoToPlayer || (SpikeState.NavToPlayer && SpikeState.PollingActive))
        {
            title = "TAXI ON THE WAY";
            meta = "Driving to your position — wait outside";
            if (SpikeState.RideDestinationPicked)
                value = $"NEXT: {destination}";
        }
        else if (SpikeState.AutoRunning || SpikeState.PendingSpawnCode != null)
        {
            title = "SPAWNING…";
            meta = "The taxi is being dispatched";
        }
        else if (SpikeState.Vehicle != null)
        {
            title = "TAXI WAITING";
            meta = "Press STOP to despawn it";
            value = SpikeState.RideDestinationPicked ? $"TO {destination}" : "No destination picked";
        }
        else
        {
            title = "CALL A TAXI";
            meta = "Orders a cab to your position";
            value = SpikeState.RideDestinationPicked ? $"TO {destination}" : "No destination picked";
        }

        if (overrideText.Length > 0)
            meta = overrideText;

        if (_heroTitle!.text != title)
        {
            _heroTitle.text = title;
            FitHeroTitle();
        }
        if (_heroMeta!.text != meta)
            _heroMeta.text = meta;
        if (_heroValue!.text != value)
        {
            _heroValue.text = value;
            _heroValue.color = valueColor;
        }
    }

    /// <summary>Drops the parenthesised detail from a drop-off sentence ("lot entry (nearest…)").</summary>
    private static string ShortDropOff(string? dropOff)
    {
        if (string.IsNullOrEmpty(dropOff))
            return string.Empty;
        int paren = dropOff.IndexOf('(');
        return paren > 0 ? dropOff.Substring(0, paren).Trim() : dropOff;
    }

    /// <summary>Shrinks the hero title to its band (computed once per text change, no per-frame best-fit).</summary>
    private void FitHeroTitle()
    {
        if (!IsAlive(_heroTitle))
            return;

        int maxSize = UITheme.Sp(26);
        int minSize = UITheme.Sp(14);
        _heroTitle!.fontSize = maxSize;
        float available = _heroTitle.rectTransform.rect.width;
        float natural = _heroTitle.preferredWidth;
        if (available > 1f && natural > available)
            _heroTitle.fontSize = Mathf.Clamp(Mathf.FloorToInt(maxSize * available / natural), minSize, maxSize);
    }

    /// <summary>Shows a button outcome in the hero meta line for a few seconds.</summary>
    private void SetStatusOverride(string text)
    {
        _statusOverride = text;
        _statusOverrideUntil = Time.unscaledTime + StatusOverrideSeconds;
        RefreshHero();
    }

    // =====================================================================
    // Button handlers
    // =====================================================================

    private void OnCallTaxiPressed()
    {
        if (!SceneGate.IsInMainScene)
        {
            SetStatusOverride("Only available in gameplay.");
            return;
        }

        bool started = SpikeCommands.CallTaxi("TaxiApp");
        SetStatusOverride(started
            ? "Taxi ordered — spawning at the stand, then driving to you."
            : "Could not order the taxi — see the log.");
    }

    /// <summary>
    /// STOP = cancel the ride AND despawn the taxi (Dominik: "Kündigen soll das
    /// Taxi despawnen lassen") — the proven cleanup order, with everyone verified
    /// out of the car before it is destroyed.
    /// </summary>
    private void OnStopPressed()
    {
        if (!SceneGate.IsInMainScene)
        {
            SetStatusOverride("Only available in gameplay.");
            return;
        }

        // A ride in progress is paid for by the meter: STOP there needs a second press.
        if (SpikeState.RideActive && Time.unscaledTime > _stopArmedUntil)
        {
            _stopArmedUntil = Time.unscaledTime + StopConfirmSeconds;
            SetStatusOverride($"Ride running — press STOP again within {StopConfirmSeconds:0} s to cancel.");
            return;
        }
        _stopArmedUntil = 0f;

        bool stopped = SpikeCommands.Stop();
        if (stopped)
        {
            SetStatusOverride("Ride cancelled — taxi removed safely.");
        }
        else if (SpikeState.VehicleOwnership.DestroyPending)
        {
            SetStatusOverride("Taxi removal is being verified — wait for the result before pressing STOP again.");
        }
        else if (SpikeState.VehicleOwnership.DestroyUnknown &&
                 SpikeState.VehicleOwnership.DestroyAttempts < VehicleOwnershipLedger.MaxDestroyAttempts)
        {
            SetStatusOverride("Taxi removal is uncertain — it remains tracked; press STOP once more for the bounded retry.");
        }
        else if (SpikeState.VehicleOwnership.DestroyUnknown)
        {
            SetStatusOverride("Taxi removal is still uncertain — no more retries; taxi remains tracked. See the log.");
        }
        else
        {
            SetStatusOverride("Taxi could not be removed safely and remains tracked. Fix the logged cause before retrying.");
        }
    }

    /// <summary>Destination row press → the shared SpikeCommands ride destination.</summary>
    private void OnDestinationPressed(string key)
    {
        if (!SceneGate.IsInMainScene)
        {
            SetStatusOverride("Only available in gameplay.");
            return;
        }

        bool ok;
        if (string.Equals(key, "STAND", StringComparison.Ordinal))
        {
            ok = SpikeCommands.SetRideDestination(key, "TaxiApp");
        }
        else if (_destSnapshot.TryGetValue(key, out TaxiDestinations.Destination? destination) && destination != null)
        {
            ok = SpikeCommands.ToDestination(destination, "TaxiApp");
        }
        else
        {
            SetStatusOverride($"'{key}' is no longer listed — reopening the list.");
            RebuildDestinationList();
            return;
        }
        SetStatusOverride(ok
            ? $"Destination: {SpikeState.RideDestinationName}"
            : $"Could not set '{key}' — see the log.");
        RefreshDestinationHighlight();
        RefreshClearButton();
    }

    /// <summary>Clear chip → drop the pick (the waiting ride keeps waiting).</summary>
    private void OnClearPressed()
    {
        SpikeState.ClearDestination();
        SetStatusOverride("Destination cleared — tap a place to pick a new one.");
        RefreshDestinationHighlight();
        RefreshClearButton();
    }

    // =====================================================================
    // Fit + diagnostics
    // =====================================================================

    /// <summary>
    /// Single-line label with a computed-once font size: the rect stays exactly one
    /// line tall, the font shrinks when the natural width exceeds the column. No
    /// per-canvas-rebuild best-fit passes (those caused the fold flicker).
    /// </summary>
    private static void MakeFixedFitting(Text txt, int maxSize, int minSize, float availableWidth)
    {
        txt.resizeTextForBestFit = false;
        txt.verticalOverflow = VerticalWrapMode.Truncate;
        txt.horizontalOverflow = HorizontalWrapMode.Overflow;
        float natural = txt.preferredWidth;
        if (natural > availableWidth && natural > 1f)
            txt.fontSize = Mathf.Clamp(Mathf.FloorToInt(maxSize * availableWidth / natural), minSize, maxSize);
    }

    /// <summary>Render-time fit verification 3 frames after a rebuild (real rect widths).</summary>
    private void RefitLabelsAtRender()
    {
        int over = 0;
        float worst = 0f;
        foreach (KeyValuePair<string, (Image Rim, Image Fill, Text Label, Text Tag, string Highlight)> entry in _destRows)
            over += RefitLabel(entry.Value.Label, UITheme.Sp(11), ref worst);

        TaxiLog.Verbose($"[ui] render-fit: {over} label(s) were wider than their rect (worst +{worst:0}u) and were shrunk to fit.");
    }

    private static int RefitLabel(Text txt, int minSize, ref float worst)
    {
        if (!IsAlive(txt))
            return 0;
        float available = txt.rectTransform.rect.width;
        if (available < 1f)
            return 0;
        float natural = txt.preferredWidth;
        float overBy = natural - available;
        if (overBy <= 0f)
            return 0;
        if (overBy > worst)
            worst = overBy;
        txt.fontSize = Mathf.Clamp(Mathf.FloorToInt(txt.fontSize * available / natural), minSize, txt.fontSize);
        return 1;
    }

    /// <summary>Verbose width audit: does any destination name need shrinking at the built size?</summary>
    private void ReportLabelWidths()
    {
        TaxiLog.Verbose(
            $"[ui] canvas {UITheme.ActualWidth:0}x{UITheme.ActualHeight:0} units, scale {UITheme.Scale:0.00}; " +
            $"name column {_nameColumnWidth:0} units, name font {UITheme.Sp(15)} pt (min {UITheme.Sp(11)} pt).");

        if (_rowWidths.Count == 0)
            return;

        int over = _rowWidths.Count(row => row.Width > _nameColumnWidth);
        TaxiLog.Verbose($"[ui] {_rowWidths.Count} rows measured: {over} wider than the name column (auto-shrunk).");
        foreach ((string name, float width) in _rowWidths.OrderByDescending(r => r.Width).Take(3))
            TaxiLog.Verbose($"[ui]   widest: '{name}' needs {width:0} units (column {_nameColumnWidth:0}).");
    }

    // =====================================================================
    // Shared helpers (PotScanner idiom)
    // =====================================================================

    /// <summary>
    /// Two-layer card: outer rounded rect in <see cref="GamePalette.Border"/> with an inner
    /// fill inset by 1 Dp — a 1 px outline around an opaque surface. Returns the outer card
    /// GameObject; content parents to <paramref name="fillImage"/>.transform.
    /// </summary>
    private static GameObject CreateCard(string name, Transform parent, Color fill, out Image fillImage,
        bool raycastTarget = false, float radius = CardRadius)
    {
        var card = UIFactory.Panel(name, parent, CardBorder, fullAnchor: true);
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

// ---------------------------------------------------------------------------
// Merged from TaxiIcon.cs (2026-10-02) — shared app/notification icon.
// ---------------------------------------------------------------------------

/// <summary>
/// The taxi icon, shared by the phone app and the fare notification: <c>taxi_icon.png</c>
/// from the game's <c>Mods</c> folder (deployed from <c>assets/</c>), with the procedural
/// yellow "T" fallback so a missing or broken file never logs load errors and never
/// renders as an empty white square.
/// </summary>
internal static class TaxiIcon
{
    private static Sprite? _cachedIconSprite;

    /// <summary>Loads and caches the icon (never returns null - the fallback is generated if needed).</summary>
    internal static Sprite Get()
    {
        if (_cachedIconSprite != null)
            return _cachedIconSprite;

        try
        {
            string path = Path.Combine(MelonLoader.Utils.MelonEnvironment.ModsDirectory, "taxi_icon.png");
            if (File.Exists(path))
            {
                var tex = new Texture2D(2, 2, TextureFormat.RGBA32, false);
                if (ImageConversion.LoadImage(tex, File.ReadAllBytes(path)))
                {
                    tex.name = "TaxiApp_Icon";
                    _cachedIconSprite = Sprite.Create(
                        tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
                    return _cachedIconSprite;
                }

                MelonLogger.Warning($"[TaxiApp] Icon '{path}' is not a decodable texture — using the procedural fallback.");
            }
            else
            {
                MelonLogger.Warning($"[TaxiApp] Icon '{path}' not found — using the procedural fallback.");
            }
        }
        catch (Exception ex)
        {
            MelonLogger.Warning($"[TaxiApp] Icon load failed ({ex.Message}) — using the procedural fallback.");
        }

        _cachedIconSprite = CreateFallbackIconSprite();
        return _cachedIconSprite;
    }

    /// <summary>Procedural yellow "T" icon — used when the PNG cannot be loaded.</summary>
    private static Sprite CreateFallbackIconSprite()
    {
        const int size = 64;
        var tex = new Texture2D(size, size, TextureFormat.RGBA32, false) { name = "TaxiApp_Icon_Fallback" };
        var yellow = new Color32(242, 194, 48, 255);
        var dark = new Color32(30, 34, 44, 255);
        var px = new Color32[size * size];
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                bool inT = (y >= 42 && y < 52 && x >= 14 && x < 50) ||   // top bar
                           (x >= 27 && x < 37 && y >= 14 && y < 52);    // stem
                px[y * size + x] = inT ? dark : yellow;
            }
        }

        tex.SetPixels32(px);
        tex.Apply(false, true);
        return Sprite.Create(tex, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f));
    }
}

