using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using MelonLoader;
using MelonLoader.Utils;
using S1API.Input;
using S1API.Lifecycle;
using S1API.Money;
using S1API.PhoneApp;
using S1API.UI;
using S1API.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace CalculatorApp;

/// <summary>
/// Thin wrapper delegating to Shared UITheme (Single Source of Truth).
/// </summary>
public static class UITheme
{
    public const float RefHeight = S1Mods.Shared.UITheme.RefHeight;
    public const float RefWidth = S1Mods.Shared.UITheme.RefWidth;
    public static float ActualWidth => S1Mods.Shared.UITheme.ActualWidth;
    public static float ActualHeight => S1Mods.Shared.UITheme.ActualHeight;
    public static float Scale => S1Mods.Shared.UITheme.Scale;
    public static void Initialize(RectTransform containerRt) => S1Mods.Shared.UITheme.InitializeForTextApp(containerRt);
    public static int Sp(float pt) => S1Mods.Shared.UITheme.Sp(pt);
    public static float Dp(float px) => S1Mods.Shared.UITheme.Dp(px);
}

/// <summary>
/// Main PhoneApp implementation for the S1API Calculator with decimal precision,
/// in-game player balance quick-insert, clipboard copy/paste, and searchable history.
/// </summary>
public sealed class CalculatorApp : PhoneApp
{
    // --- UI Color Palette ---
    private static readonly Color BgColor = new(0.06f, 0.07f, 0.10f, 1f);
    private static readonly Color HeaderBgColor = new(0.09f, 0.11f, 0.15f, 1f);
    private static readonly Color DisplayBgColor = new(0.09f, 0.11f, 0.16f, 1f);
    private static readonly Color CardBgColor = new(0.11f, 0.13f, 0.18f, 1f);
    private static readonly Color InputBgColor = new(0.12f, 0.15f, 0.22f, 1f);
    private static readonly Color NumBtnColor = new(0.14f, 0.17f, 0.23f, 1f);
    private static readonly Color FuncBtnColor = new(0.22f, 0.27f, 0.36f, 1f);
    private static readonly Color QuickBtnColor = new(0.16f, 0.21f, 0.30f, 1f);
    private static readonly Color OpBtnColor = new(0.95f, 0.58f, 0.15f, 1f);    // Vibrant Amber / Orange
    private static readonly Color EqualsBtnColor = new(0.18f, 0.65f, 0.98f, 1f); // Accent Blue
    private static readonly Color DangerBtnColor = new(0.85f, 0.25f, 0.25f, 1f);
    private static readonly Color DividerColor = new(1f, 1f, 1f, 0.08f);
    private static readonly Color SubtitleColor = new(0.60f, 0.68f, 0.78f, 1f);
    private static readonly Color AccentSuccessColor = new(0.25f, 0.85f, 0.55f, 1f);

    // --- State & Engine ---
    private CalculatorState _state = null!;
    private CalculatorEngine _engine = null!;
    private string _currentSearchQuery = string.Empty;

    // --- UI References ---
    private GameObject _mainBG = null!;
    private GameObject _keypadRoot = null!;
    private GameObject _historyRoot = null!;
    private Text _mainDisplayText = null!;
    private Text _subDisplayText = null!;
    private Text _clearBtnLabel = null!;
    private Text _tabKeypadLabel = null!;
    private Text _tabHistoryLabel = null!;
    private Image _tabKeypadBg = null!;
    private Image _tabHistoryBg = null!;
    private InputField _historySearchInput = null!;
    private RectTransform _historyListContent = null!;
    private ScrollRect _historyScrollRect = null!;
    private Text _historyCountText = null!;
    private CalculatorAppInputFocus _inputFocusHook = null!;
    private Sprite? _cachedIconSprite;

    protected override string AppName => "CalculatorApp";
    protected override string AppTitle => "Calculator";
    protected override string IconLabel => "Calculator";
    protected override string IconFileName => "calculator_icon.png";
    protected override EOrientation Orientation => EOrientation.Vertical;

    protected override Sprite IconSprite
    {
        get
        {
            if (_cachedIconSprite != null) return _cachedIconSprite;

            string iconPath = Path.Combine(MelonEnvironment.ModsDirectory, "calculator_icon.png");
            if (File.Exists(iconPath))
            {
                try
                {
                    byte[] data = File.ReadAllBytes(iconPath);
                    var tex = new Texture2D(128, 128, TextureFormat.RGBA32, false);
                    if (ImageConversion.LoadImage(tex, data))
                    {
                        _cachedIconSprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
                        _cachedIconSprite.name = "CalculatorIcon";
                        return _cachedIconSprite;
                    }
                }
                catch (Exception ex)
                {
                    MelonLogger.Warning($"Failed to load icon from file: {ex.Message}");
                }
            }

            _cachedIconSprite = GenerateProgrammaticIcon();
            return _cachedIconSprite;
        }
    }

    // Fix (Bug-Audit 2026-09-10): the phone re-instantiates this app per scene load and the old
    // per-instance handlers stayed in the static invocation lists forever. Static events now
    // dispatch through _active, subscribed exactly once; Mod.OnSceneWasUnloaded clears _active
    // when the gameplay scene tears down.
    private static CalculatorApp? _active;
    private static bool _staticSubscribed;

    protected override void OnCreated()
    {
        _state = CalculatorState.Load();
        _engine = new CalculatorEngine(_state);
        _engine.OnStateChanged += UpdateDisplayUI;

        base.OnCreated();
        _active = this;
        if (_staticSubscribed) return;
        _staticSubscribed = true;
        MelonEvents.OnUpdate.Subscribe(DispatchUpdate);

        // Fix 1.1 (Bug-Audit 2026-09-02): subscribe to lifecycle events so the state reloads
        // on save-slot switch / new-game load. Without this, history from slot A stayed in
        // RAM and overwrote slot B's file on the next save.
        GameLifecycle.OnSaveInfoLoaded += DispatchSaveInfoLoaded;
        GameLifecycle.OnPreLoad += DispatchPreLoad;

        MelonLogger.Msg("Loaded state & initialized (v0.2.1).");
    }

    internal static void TearDownForSceneUnload() => _active = null;

    private static void DispatchUpdate() => _active?.OnUpdate();

    private static void DispatchSaveInfoLoaded() => _active?.HandleSaveInfoLoaded();

    private static void DispatchPreLoad() => _active?.HandlePreLoad();

    private void HandleSaveInfoLoaded()
    {
        // Slot changed (or save finished loading info): reload state from the new slot's file.
        try
        {
            _state = CalculatorState.Load();
            if (_engine != null) _engine.OnStateChanged -= UpdateDisplayUI;
            _engine = new CalculatorEngine(_state);
            _engine.OnStateChanged += UpdateDisplayUI;
            UpdateDisplayUI();
            RefreshHistoryList();
        }
        catch (Exception ex)
        {
            MelonLogger.Warning($"State reload on save-info load failed: {ex.Message}");
        }
    }

    private void HandlePreLoad()
    {
        // A new save/scene is about to load: flush the current slot's state before switching.
        try { _state?.Save(); }
        catch (Exception ex) { MelonLogger.Warning($"State flush on pre-load failed: {ex.Message}"); }
    }

    protected override void OnPhoneClosed()
    {
        base.OnPhoneClosed();
        if (_mainBG != null) _mainBG.SetActive(false);
        Controls.IsTyping = false;
        _state?.Save();
    }

    private void OnUpdate()
    {
        bool open = IsOpen();
        if (_mainBG != null && _mainBG.activeSelf != open)
        {
            _mainBG.SetActive(open);
            if (open)
            {
                UpdateDisplayUI();
            }
            else
            {
                Controls.IsTyping = false;
            }
        }

        if (!open) return;

        // Handle physical keyboard input when Keypad view is active
        if (_keypadRoot != null && _keypadRoot.activeSelf)
        {
            HandleKeyboardInput();
        }
    }

    protected override void OnCreatedUI(GameObject container)
    {
        var containerRt = container.GetComponent<RectTransform>();
        if (containerRt != null)
        {
            UITheme.Initialize(containerRt);
        }

        _inputFocusHook = container.AddComponent<CalculatorAppInputFocus>();

        _mainBG = UIFactory.Panel("CalculatorBG", container.transform, BgColor, fullAnchor: true);
        _mainBG.SetActive(false);

        BuildHeaderBar(_mainBG);
        BuildKeypadScreen(_mainBG);
        BuildHistoryScreen(_mainBG);

        // Assign search input to focus hook
        if (_inputFocusHook != null)
        {
            _inputFocusHook.searchInput = _historySearchInput;
        }

        // Show Keypad view by default
        ShowKeypadTab();
        UpdateDisplayUI();
    }

    // =====================================================================
    // 1. Header Bar & Tabs
    // =====================================================================

    private void BuildHeaderBar(GameObject parent)
    {
        var header = UIFactory.Panel("HeaderBar", parent.transform, HeaderBgColor);
        var hRt = header.GetComponent<RectTransform>();
        hRt.anchorMin = new Vector2(0f, 0.91f);
        hRt.anchorMax = new Vector2(1f, 1.0f);
        hRt.offsetMin = Vector2.zero;
        hRt.offsetMax = Vector2.zero;

        // Title
        var title = UIFactory.Text("AppTitle", "Calculator", header.transform, UITheme.Sp(24), TextAnchor.MiddleLeft, FontStyle.Bold);
        var tRt = title.rectTransform;
        tRt.anchorMin = new Vector2(0.05f, 0f);
        tRt.anchorMax = new Vector2(0.40f, 1f);
        tRt.offsetMin = Vector2.zero;
        tRt.offsetMax = Vector2.zero;
        title.raycastTarget = false;

        // Tab Switcher (Keypad / History)
        var tabContainer = new GameObject("TabContainer");
        tabContainer.transform.SetParent(header.transform, false);
        var tcRt = tabContainer.AddComponent<RectTransform>();
        tcRt.anchorMin = new Vector2(0.42f, 0.15f);
        tcRt.anchorMax = new Vector2(0.95f, 0.85f);
        tcRt.offsetMin = Vector2.zero;
        tcRt.offsetMax = Vector2.zero;

        // Keypad Tab Button
        var (keypadMask, keypadBtn, keypadLabel) = UIFactory.RoundedButtonWithLabel(
            "TabKeypad", "Calc", tabContainer.transform,
            EqualsBtnColor, 90f, UITheme.Dp(32f), UITheme.Sp(17), Color.white);
        var kpRt = keypadMask.GetComponent<RectTransform>();
        kpRt.anchorMin = new Vector2(0f, 0f);
        kpRt.anchorMax = new Vector2(0.48f, 1f);
        kpRt.offsetMin = Vector2.zero;
        kpRt.offsetMax = Vector2.zero;
        _tabKeypadLabel = keypadLabel;
        _tabKeypadBg = keypadMask.GetComponentInChildren<Image>();
        EventHelper.AddListener(ShowKeypadTab, keypadBtn.onClick);

        // History Tab Button
        var (histMask, histBtn, histLabel) = UIFactory.RoundedButtonWithLabel(
            "TabHistory", "History", tabContainer.transform,
            FuncBtnColor, 90f, UITheme.Dp(32f), UITheme.Sp(17), Color.white);
        var histRt = histMask.GetComponent<RectTransform>();
        histRt.anchorMin = new Vector2(0.52f, 0f);
        histRt.anchorMax = new Vector2(1.0f, 1f);
        histRt.offsetMin = Vector2.zero;
        histRt.offsetMax = Vector2.zero;
        _tabHistoryLabel = histLabel;
        _tabHistoryBg = histMask.GetComponentInChildren<Image>();
        EventHelper.AddListener(ShowHistoryTab, histBtn.onClick);

        // Divider line under header
        var div = UIFactory.Panel("HeaderDivider", parent.transform, DividerColor);
        var dRt = div.GetComponent<RectTransform>();
        dRt.anchorMin = new Vector2(0.04f, 0.908f);
        dRt.anchorMax = new Vector2(0.96f, 0.910f);
        dRt.offsetMin = Vector2.zero;
        dRt.offsetMax = Vector2.zero;
    }

    // =====================================================================
    // 2. Keypad Screen (Display, Quick Action Chips & 4x5 Buttons)
    // =====================================================================

    private void BuildKeypadScreen(GameObject parent)
    {
        _keypadRoot = new GameObject("KeypadScreen");
        _keypadRoot.transform.SetParent(parent.transform, false);
        var rootRt = _keypadRoot.AddComponent<RectTransform>();
        rootRt.anchorMin = Vector2.zero;
        rootRt.anchorMax = new Vector2(1f, 0.91f);
        rootRt.offsetMin = Vector2.zero;
        rootRt.offsetMax = Vector2.zero;

        // --- Quick Actions Bar (Cash, Bank, Copy, Sqrt) ---
        var quickBar = new GameObject("QuickBar");
        quickBar.transform.SetParent(_keypadRoot.transform, false);
        var qbRt = quickBar.AddComponent<RectTransform>();
        qbRt.anchorMin = new Vector2(0.04f, 0.895f);
        qbRt.anchorMax = new Vector2(0.96f, 0.975f);
        qbRt.offsetMin = Vector2.zero;
        qbRt.offsetMax = Vector2.zero;

        // 4 Quick action chips: Cash, Bank, Copy, √
        float[] qbMins = { 0.00f, 0.26f, 0.52f, 0.77f };
        float[] qbMaxs = { 0.24f, 0.50f, 0.75f, 1.00f };

        CreateChipButton("BtnQuickCash", "💵 Cash", quickBar.transform, qbMins[0], qbMaxs[0], QuickBtnColor, OnInsertCashClicked);
        CreateChipButton("BtnQuickBank", "💳 Bank", quickBar.transform, qbMins[1], qbMaxs[1], QuickBtnColor, OnInsertBankClicked);
        CreateChipButton("BtnQuickCopy", "📋 Copy", quickBar.transform, qbMins[2], qbMaxs[2], QuickBtnColor, OnCopyClicked);
        CreateChipButton("BtnQuickSqrt", "√ Sqrt", quickBar.transform, qbMins[3], qbMaxs[3], QuickBtnColor, () => _engine.InputSquareRoot());

        // --- Readout Display Card ---
        var displayBox = UIFactory.Panel("DisplayBox", _keypadRoot.transform, DisplayBgColor);
        var dbRt = displayBox.GetComponent<RectTransform>();
        dbRt.anchorMin = new Vector2(0.04f, 0.690f);
        dbRt.anchorMax = new Vector2(0.96f, 0.880f);
        dbRt.offsetMin = Vector2.zero;
        dbRt.offsetMax = Vector2.zero;

        // Sub-expression text (top-right)
        _subDisplayText = UIFactory.Text("SubDisplay", "", displayBox.transform, UITheme.Sp(17), TextAnchor.MiddleRight);
        var subRt = _subDisplayText.rectTransform;
        subRt.anchorMin = new Vector2(0.04f, 0.60f);
        subRt.anchorMax = new Vector2(0.96f, 0.95f);
        subRt.offsetMin = Vector2.zero;
        subRt.offsetMax = Vector2.zero;
        _subDisplayText.color = SubtitleColor;
        _subDisplayText.raycastTarget = false;

        // Main display value (bottom-right)
        _mainDisplayText = UIFactory.Text("MainDisplay", "0", displayBox.transform, UITheme.Sp(38), TextAnchor.MiddleRight, FontStyle.Bold);
        var mainRt = _mainDisplayText.rectTransform;
        mainRt.anchorMin = new Vector2(0.04f, 0.05f);
        mainRt.anchorMax = new Vector2(0.96f, 0.65f);
        mainRt.offsetMin = Vector2.zero;
        mainRt.offsetMax = Vector2.zero;
        _mainDisplayText.color = Color.white;
        _mainDisplayText.raycastTarget = false;

        // --- 4x5 Keypad Grid ---
        // Columns X anchors:
        float[] colMins = { 0.04f, 0.275f, 0.510f, 0.745f };
        float[] colMaxs = { 0.255f, 0.490f, 0.725f, 0.960f };

        // Rows Y anchors (5 rows from top to bottom):
        float[] rowMins = { 0.550f, 0.415f, 0.280f, 0.145f, 0.010f };
        float[] rowMaxs = { 0.665f, 0.530f, 0.395f, 0.260f, 0.125f };

        // Row 0: C/AC, ⌫, %, ÷
        var (_, _, clearLabel) = CreateGridButtonWithLabel("BtnAC", "AC", 0, 0, colMins, colMaxs, rowMins, rowMaxs, FuncBtnColor, () => _engine.InputClear());
        _clearBtnLabel = clearLabel;

        CreateGridButton("BtnBack", "⌫", 0, 1, colMins, colMaxs, rowMins, rowMaxs, FuncBtnColor, () => _engine.InputBackspace());
        CreateGridButton("BtnPercent", "%", 0, 2, colMins, colMaxs, rowMins, rowMaxs, FuncBtnColor, () => _engine.InputPercent());
        CreateGridButton("BtnDiv", "÷", 0, 3, colMins, colMaxs, rowMins, rowMaxs, OpBtnColor, () => _engine.InputOperator("÷"));

        // Row 1: 7, 8, 9, ×
        CreateGridButton("Btn7", "7", 1, 0, colMins, colMaxs, rowMins, rowMaxs, NumBtnColor, () => _engine.InputDigit(7));
        CreateGridButton("Btn8", "8", 1, 1, colMins, colMaxs, rowMins, rowMaxs, NumBtnColor, () => _engine.InputDigit(8));
        CreateGridButton("Btn9", "9", 1, 2, colMins, colMaxs, rowMins, rowMaxs, NumBtnColor, () => _engine.InputDigit(9));
        CreateGridButton("BtnMul", "×", 1, 3, colMins, colMaxs, rowMins, rowMaxs, OpBtnColor, () => _engine.InputOperator("×"));

        // Row 2: 4, 5, 6, -
        CreateGridButton("Btn4", "4", 2, 0, colMins, colMaxs, rowMins, rowMaxs, NumBtnColor, () => _engine.InputDigit(4));
        CreateGridButton("Btn5", "5", 2, 1, colMins, colMaxs, rowMins, rowMaxs, NumBtnColor, () => _engine.InputDigit(5));
        CreateGridButton("Btn6", "6", 2, 2, colMins, colMaxs, rowMins, rowMaxs, NumBtnColor, () => _engine.InputDigit(6));
        CreateGridButton("BtnSub", "-", 2, 3, colMins, colMaxs, rowMins, rowMaxs, OpBtnColor, () => _engine.InputOperator("-"));

        // Row 3: 1, 2, 3, +
        CreateGridButton("Btn1", "1", 3, 0, colMins, colMaxs, rowMins, rowMaxs, NumBtnColor, () => _engine.InputDigit(1));
        CreateGridButton("Btn2", "2", 3, 1, colMins, colMaxs, rowMins, rowMaxs, NumBtnColor, () => _engine.InputDigit(2));
        CreateGridButton("Btn3", "3", 3, 2, colMins, colMaxs, rowMins, rowMaxs, NumBtnColor, () => _engine.InputDigit(3));
        CreateGridButton("BtnAdd", "+", 3, 3, colMins, colMaxs, rowMins, rowMaxs, OpBtnColor, () => _engine.InputOperator("+"));

        // Row 4: ±, 0, ., =
        CreateGridButton("BtnNeg", "±", 4, 0, colMins, colMaxs, rowMins, rowMaxs, FuncBtnColor, () => _engine.InputNegate());
        CreateGridButton("Btn0", "0", 4, 1, colMins, colMaxs, rowMins, rowMaxs, NumBtnColor, () => _engine.InputDigit(0));
        CreateGridButton("BtnDot", ".", 4, 2, colMins, colMaxs, rowMins, rowMaxs, NumBtnColor, () => _engine.InputDecimal());
        CreateGridButton("BtnEquals", "=", 4, 3, colMins, colMaxs, rowMins, rowMaxs, EqualsBtnColor, () => _engine.InputEquals());
    }

    private void CreateChipButton(
        string name,
        string label,
        Transform parent,
        float minX,
        float maxX,
        Color bgColor,
        Action onClick)
    {
        var (mask, btn, _) = UIFactory.RoundedButtonWithLabel(
            name, label, parent,
            bgColor, 80f, UITheme.Dp(28f), UITheme.Sp(14), Color.white);

        var rt = mask.GetComponent<RectTransform>();
        rt.anchorMin = new Vector2(minX, 0f);
        rt.anchorMax = new Vector2(maxX, 1f);
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;

        EventHelper.AddListener(onClick, btn.onClick);
    }

    private void CreateGridButton(
        string name,
        string label,
        int row,
        int col,
        float[] colMins,
        float[] colMaxs,
        float[] rowMins,
        float[] rowMaxs,
        Color bgColor,
        Action onClick)
    {
        var (mask, btn, _) = UIFactory.RoundedButtonWithLabel(
            name, label, _keypadRoot.transform,
            bgColor, 80f, UITheme.Dp(58f), UITheme.Sp(26), Color.white);

        var rt = mask.GetComponent<RectTransform>();
        rt.anchorMin = new Vector2(colMins[col], rowMins[row]);
        rt.anchorMax = new Vector2(colMaxs[col], rowMaxs[row]);
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;

        EventHelper.AddListener(onClick, btn.onClick);
    }

    private (GameObject mask, Button btn, Text label) CreateGridButtonWithLabel(
        string name,
        string label,
        int row,
        int col,
        float[] colMins,
        float[] colMaxs,
        float[] rowMins,
        float[] rowMaxs,
        Color bgColor,
        Action onClick)
    {
        var (mask, btn, labelText) = UIFactory.RoundedButtonWithLabel(
            name, label, _keypadRoot.transform,
            bgColor, 80f, UITheme.Dp(58f), UITheme.Sp(26), Color.white);

        var rt = mask.GetComponent<RectTransform>();
        rt.anchorMin = new Vector2(colMins[col], rowMins[row]);
        rt.anchorMax = new Vector2(colMaxs[col], rowMaxs[row]);
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;

        EventHelper.AddListener(onClick, btn.onClick);
        return (mask, btn, labelText);
    }

    // =====================================================================
    // 3. History Screen ("Suchverlauf" with Search Bar & Direct Actions)
    // =====================================================================

    private void BuildHistoryScreen(GameObject parent)
    {
        _historyRoot = new GameObject("HistoryScreen");
        _historyRoot.transform.SetParent(parent.transform, false);
        var rootRt = _historyRoot.AddComponent<RectTransform>();
        rootRt.anchorMin = Vector2.zero;
        rootRt.anchorMax = new Vector2(1f, 0.91f);
        rootRt.offsetMin = Vector2.zero;
        rootRt.offsetMax = Vector2.zero;
        _historyRoot.SetActive(false);

        // --- Search Bar ---
        _historySearchInput = CreateSearchInputField("HistorySearchInput", "🔍 Search history...", _historyRoot.transform);
        var sRt = _historySearchInput.gameObject.GetComponent<RectTransform>();
        sRt.anchorMin = new Vector2(0.04f, 0.91f);
        sRt.anchorMax = new Vector2(0.96f, 0.985f);
        sRt.offsetMin = Vector2.zero;
        sRt.offsetMax = Vector2.zero;

        // Safe S1API listener for string event
        EventHelper.AddListener<string>(OnSearchQueryChanged, _historySearchInput.onValueChanged);

        // --- History Count / Status Header ---
        _historyCountText = UIFactory.Text("HistoryCount", "0 calculations", _historyRoot.transform, UITheme.Sp(16), TextAnchor.MiddleLeft);
        var hcRt = _historyCountText.rectTransform;
        hcRt.anchorMin = new Vector2(0.05f, 0.865f);
        hcRt.anchorMax = new Vector2(0.95f, 0.905f);
        hcRt.offsetMin = Vector2.zero;
        hcRt.offsetMax = Vector2.zero;
        _historyCountText.color = SubtitleColor;
        _historyCountText.raycastTarget = false;

        // --- Scrollable History List ---
        var list = UIFactory.ScrollableVerticalList("HistoryList", _historyRoot.transform, out var scrollRect);
        _historyScrollRect = scrollRect;
        // Fix 2026-09-02: responsive scroll feel (same tuning as NotesApp).
        scrollRect.scrollSensitivity = 35f;
        scrollRect.elasticity = 0.08f;
        scrollRect.decelerationRate = 0.16f;
        scrollRect.inertia = true;
        scrollRect.vertical = true;
        scrollRect.movementType = ScrollRect.MovementType.Clamped;

        var listRt = (RectTransform)list.parent.parent;
        listRt.anchorMin = new Vector2(0.04f, 0.12f);
        listRt.anchorMax = new Vector2(0.96f, 0.86f);
        listRt.offsetMin = Vector2.zero;
        listRt.offsetMax = Vector2.zero;
        _historyListContent = list;

        var vlg = _historyListContent.GetComponent<VerticalLayoutGroup>();
        if (vlg != null)
        {
            vlg.spacing = 8f;
            vlg.padding = new RectOffset(0, 0, 4, 4);
            vlg.childControlWidth = true;
            vlg.childControlHeight = true;
            vlg.childForceExpandWidth = true;
            vlg.childForceExpandHeight = false;
        }

        // --- Bottom Action Bar ("Clear History") ---
        var bottomBar = new GameObject("HistoryBottomBar");
        bottomBar.transform.SetParent(_historyRoot.transform, false);
        var bbRt = bottomBar.AddComponent<RectTransform>();
        bbRt.anchorMin = new Vector2(0f, 0f);
        bbRt.anchorMax = new Vector2(1f, 0.11f);
        bbRt.offsetMin = Vector2.zero;
        bbRt.offsetMax = Vector2.zero;

        UIFactory.Panel("BottomBg", bottomBar.transform, HeaderBgColor, fullAnchor: true);

        var (clearMask, clearBtn, _) = UIFactory.RoundedButtonWithLabel(
            "ClearHistoryBtn", "Clear All History", bottomBar.transform,
            DangerBtnColor, 340f, UITheme.Dp(42f), UITheme.Sp(19), Color.white);
        var clRt = clearMask.GetComponent<RectTransform>();
        clRt.anchorMin = new Vector2(0.06f, 0.15f);
        clRt.anchorMax = new Vector2(0.94f, 0.85f);
        clRt.offsetMin = Vector2.zero;
        clRt.offsetMax = Vector2.zero;
        EventHelper.AddListener(OnClearHistoryClicked, clearBtn.onClick);
    }

    private void OnSearchQueryChanged(string query)
    {
        _currentSearchQuery = query;
        RefreshHistoryList();
    }

    private InputField CreateSearchInputField(string name, string placeholderText, Transform parent)
    {
        var go = new GameObject(name);
        go.transform.SetParent(parent, false);
        go.AddComponent<RectTransform>();

        var inputField = go.AddComponent<InputField>();

        var bg = UIFactory.Panel("Background", go.transform, InputBgColor);
        var bgRt = bg.GetComponent<RectTransform>();
        bgRt.anchorMin = Vector2.zero;
        bgRt.anchorMax = Vector2.one;
        bgRt.offsetMin = Vector2.zero;
        bgRt.offsetMax = Vector2.zero;

        var text = UIFactory.Text("Text", "", go.transform, UITheme.Sp(18), TextAnchor.MiddleLeft);
        var textRt = text.rectTransform;
        textRt.anchorMin = Vector2.zero;
        textRt.anchorMax = Vector2.one;
        textRt.offsetMin = new Vector2(14f, 0f);
        textRt.offsetMax = new Vector2(-14f, 0f);
        text.supportRichText = false;
        text.raycastTarget = false;

        var placeholder = UIFactory.Text("Placeholder", placeholderText, go.transform, UITheme.Sp(18), TextAnchor.MiddleLeft);
        var phRt = placeholder.rectTransform;
        phRt.anchorMin = Vector2.zero;
        phRt.anchorMax = Vector2.one;
        phRt.offsetMin = new Vector2(14f, 0f);
        phRt.offsetMax = new Vector2(-14f, 0f);
        placeholder.supportRichText = false;
        placeholder.color = new Color(1f, 1f, 1f, 0.40f);
        placeholder.raycastTarget = false;

        inputField.targetGraphic = bg.GetComponent<Graphic>();
        inputField.textComponent = text;
        inputField.placeholder = placeholder;
        inputField.caretWidth = 2;
        inputField.caretColor = Color.white;
        inputField.selectionColor = new Color(0.18f, 0.65f, 0.98f, 0.45f);
        inputField.lineType = InputField.LineType.SingleLine;

        return inputField;
    }

    // =====================================================================
    // 4. View Switching & Rendering
    // =====================================================================

    private void ShowKeypadTab()
    {
        if (_keypadRoot != null) _keypadRoot.SetActive(true);
        if (_historyRoot != null) _historyRoot.SetActive(false);

        if (_tabKeypadBg != null) _tabKeypadBg.color = EqualsBtnColor;
        if (_tabHistoryBg != null) _tabHistoryBg.color = FuncBtnColor;
        Controls.IsTyping = false;
    }

    private void ShowHistoryTab()
    {
        if (_keypadRoot != null) _keypadRoot.SetActive(false);
        if (_historyRoot != null) _historyRoot.SetActive(true);

        if (_tabKeypadBg != null) _tabKeypadBg.color = FuncBtnColor;
        if (_tabHistoryBg != null) _tabHistoryBg.color = EqualsBtnColor;

        RefreshHistoryList();
    }

    private void UpdateDisplayUI()
    {
        if (_mainDisplayText == null || _subDisplayText == null) return;

        string display = _engine.DisplayText;
        _mainDisplayText.text = display;
        _subDisplayText.text = _engine.ExpressionText;

        // Auto-scale font size if text is long
        if (display.Length > 14)
        {
            _mainDisplayText.fontSize = UITheme.Sp(22);
        }
        else if (display.Length > 10)
        {
            _mainDisplayText.fontSize = UITheme.Sp(28);
        }
        else
        {
            _mainDisplayText.fontSize = UITheme.Sp(38);
        }

        // Dynamic C vs AC button label
        if (_clearBtnLabel != null)
        {
            _clearBtnLabel.text = _engine.IsDirtyEntry ? "C" : "AC";
        }
    }

    private void RefreshHistoryList()
    {
        if (_historyListContent == null) return;

        // Clear previous rows
        for (int i = _historyListContent.childCount - 1; i >= 0; i--)
        {
            UnityEngine.Object.Destroy(_historyListContent.GetChild(i).gameObject);
        }

        var items = _engine.GetFilteredHistory(_currentSearchQuery).ToList();

        if (_historyCountText != null)
        {
            if (string.IsNullOrEmpty(_currentSearchQuery))
            {
                _historyCountText.text = $"{items.Count} past calculation{(items.Count == 1 ? "" : "s")}";
            }
            else
            {
                _historyCountText.text = $"Found {items.Count} result{(items.Count == 1 ? "" : "s")} for \"{_currentSearchQuery}\"";
            }
        }

        if (items.Count == 0)
        {
            var empty = new GameObject("EmptyHistory");
            empty.AddComponent<RectTransform>();
            empty.transform.SetParent(_historyListContent, false);
            UIFactory.Panel("Background", empty.transform, CardBgColor, fullAnchor: true);

            string emptyMsg = string.IsNullOrEmpty(_currentSearchQuery)
                ? "No calculations in history yet.\nPerform some calculations on the Keypad!"
                : $"No calculations matched \"{_currentSearchQuery}\".";

            var emptyText = UIFactory.Text("EmptyText", emptyMsg, empty.transform, UITheme.Sp(18), TextAnchor.MiddleCenter);
            var etRt = emptyText.rectTransform;
            etRt.anchorMin = Vector2.zero;
            etRt.anchorMax = Vector2.one;
            etRt.offsetMin = new Vector2(16f, 16f);
            etRt.offsetMax = new Vector2(-16f, -16f);
            emptyText.color = SubtitleColor;
            emptyText.raycastTarget = false;

            var emptyLE = empty.AddComponent<LayoutElement>();
            emptyLE.minHeight = UITheme.Dp(130f);
            emptyLE.preferredHeight = UITheme.Dp(130f);

            UIFactory.FitContentHeight(_historyListContent);
            return;
        }

        float rowHeight = UITheme.Dp(76f);

        foreach (var record in items)
        {
            var row = new GameObject("HistRow_" + record.Id);
            var rowRt = row.AddComponent<RectTransform>();
            rowRt.anchorMin = Vector2.zero;
            rowRt.anchorMax = Vector2.one;
            rowRt.offsetMin = Vector2.zero;
            rowRt.offsetMax = Vector2.zero;
            row.transform.SetParent(_historyListContent, false);

            var rowLE = row.AddComponent<LayoutElement>();
            rowLE.minHeight = rowHeight;
            rowLE.preferredHeight = rowHeight;
            rowLE.flexibleWidth = 1f;

            var rowBg = UIFactory.Panel("RowBg", row.transform, CardBgColor, fullAnchor: true);
            var rowBtn = rowBg.AddComponent<Button>();
            rowBtn.transition = Selectable.Transition.None;

            // Clicking history item restores result into calculator display
            var capturedRecord = record;
            EventHelper.AddListener(() =>
            {
                _engine.RestoreHistoryResult(capturedRecord);
                ShowKeypadTab();
            }, rowBtn.onClick);

            // Left accent bar
            var accent = UIFactory.Panel("Accent", row.transform, EqualsBtnColor);
            var accRt = accent.GetComponent<RectTransform>();
            accRt.anchorMin = new Vector2(0f, 0f);
            accRt.anchorMax = new Vector2(0f, 1f);
            accRt.offsetMin = Vector2.zero;
            accRt.offsetMax = new Vector2(4f, 0f);

            // Expression on top-left (e.g., "120 × 5 =")
            var exprText = UIFactory.Text("Expr", $"{record.Expression} =", row.transform, UITheme.Sp(16), TextAnchor.MiddleLeft);
            var exprRt = exprText.rectTransform;
            exprRt.anchorMin = new Vector2(0f, 0.52f);
            exprRt.anchorMax = new Vector2(0.55f, 0.95f);
            exprRt.offsetMin = new Vector2(14f, 0f);
            exprRt.offsetMax = new Vector2(-4f, 0f);
            exprText.color = SubtitleColor;
            exprText.raycastTarget = false;

            // Timestamp on top (middle right)
            var timeText = UIFactory.Text("Time", FormatRelativeTimestamp(record.Timestamp), row.transform, UITheme.Sp(13), TextAnchor.MiddleRight);
            var timeRt = timeText.rectTransform;
            timeRt.anchorMin = new Vector2(0.55f, 0.52f);
            timeRt.anchorMax = new Vector2(0.76f, 0.95f);
            timeRt.offsetMin = Vector2.zero;
            timeRt.offsetMax = new Vector2(-6f, 0f);
            timeText.color = new Color(1f, 1f, 1f, 0.40f);
            timeText.raycastTarget = false;

            // Big Result on bottom (e.g. "600")
            var resText = UIFactory.Text("Result", record.Result, row.transform, UITheme.Sp(22), TextAnchor.MiddleLeft, FontStyle.Bold);
            var resRt = resText.rectTransform;
            resRt.anchorMin = new Vector2(0f, 0.05f);
            resRt.anchorMax = new Vector2(0.76f, 0.52f);
            resRt.offsetMin = new Vector2(14f, 0f);
            resRt.offsetMax = new Vector2(-4f, 0f);
            resText.color = AccentSuccessColor;
            resText.raycastTarget = false;

            // --- Direct Row Action Buttons ---
            // 1. Copy button [📋]
            var (copyMask, copyBtn, _) = UIFactory.RoundedButtonWithLabel(
                "CopyBtn_" + record.Id, "📋", row.transform,
                FuncBtnColor, 32f, UITheme.Dp(30f), UITheme.Sp(14), Color.white);
            var cpRt = copyMask.GetComponent<RectTransform>();
            cpRt.anchorMin = new Vector2(0.78f, 0.15f);
            cpRt.anchorMax = new Vector2(0.88f, 0.85f);
            cpRt.offsetMin = Vector2.zero;
            cpRt.offsetMax = Vector2.zero;
            EventHelper.AddListener(() =>
            {
                GUIUtility.systemCopyBuffer = capturedRecord.Result;
                MelonLogger.Msg($"Copied '{capturedRecord.Result}' to clipboard.");
            }, copyBtn.onClick);

            // 2. Delete button [🗑️]
            var (delMask, delBtn, _) = UIFactory.RoundedButtonWithLabel(
                "DelBtn_" + record.Id, "🗑️", row.transform,
                DangerBtnColor, 32f, UITheme.Dp(30f), UITheme.Sp(14), Color.white);
            var dlRt = delMask.GetComponent<RectTransform>();
            dlRt.anchorMin = new Vector2(0.89f, 0.15f);
            dlRt.anchorMax = new Vector2(0.98f, 0.85f);
            dlRt.offsetMin = Vector2.zero;
            dlRt.offsetMax = Vector2.zero;
            EventHelper.AddListener(() =>
            {
                _engine.DeleteHistoryRecord(capturedRecord.Id);
                RefreshHistoryList();
            }, delBtn.onClick);
        }

        UIFactory.FitContentHeight(_historyListContent);
    }

    private void OnClearHistoryClicked()
    {
        _engine.ClearHistory();
        RefreshHistoryList();
    }

    // =====================================================================
    // 5. Game Integration & Clipboard Helpers
    // =====================================================================

    private void OnInsertCashClicked()
    {
        try
        {
            float cash = Money.GetCashBalance();
            decimal cashDec = Convert.ToDecimal(cash);
            _engine.SetDisplayValue(cashDec, $"Cash (${CalculatorEngine.FormatNumber(cashDec)})");
        }
        catch (Exception ex)
        {
            MelonLogger.Warning($"Failed to read cash balance: {ex.Message}");
        }
    }

    private void OnInsertBankClicked()
    {
        try
        {
            float bank = Money.GetOnlineBalance();
            decimal bankDec = Convert.ToDecimal(bank);
            _engine.SetDisplayValue(bankDec, $"Bank (${CalculatorEngine.FormatNumber(bankDec)})");
        }
        catch (Exception ex)
        {
            MelonLogger.Warning($"Failed to read bank balance: {ex.Message}");
        }
    }

    private void OnCopyClicked()
    {
        string textToCopy = _engine.DisplayText;
        if (_engine.HasError || string.IsNullOrEmpty(textToCopy)) return;

        GUIUtility.systemCopyBuffer = textToCopy;
        MelonLogger.Msg($"Copied display value '{textToCopy}' to clipboard.");
    }

    private void OnPasteClicked()
    {
        string clip = GUIUtility.systemCopyBuffer;
        if (!string.IsNullOrWhiteSpace(clip))
        {
            if (_engine.TryPaste(clip))
            {
                MelonLogger.Msg($"Pasted '{clip}' into calculator.");
            }
        }
    }

    // =====================================================================
    // 6. Physical Keyboard Input Hook
    // =====================================================================

    private void HandleKeyboardInput()
    {
        // Don't intercept keyboard if user is typing in a search input
        if (_historySearchInput != null && _historySearchInput.isFocused)
        {
            return;
        }

        bool ctrl = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);

        // Ctrl+C (Copy)
        if (ctrl && Input.GetKeyDown(KeyCode.C))
        {
            OnCopyClicked();
            return;
        }

        // Ctrl+V (Paste)
        if (ctrl && Input.GetKeyDown(KeyCode.V))
        {
            OnPasteClicked();
            return;
        }

        // Ctrl+H or Tab (Toggle Tab)
        if ((ctrl && Input.GetKeyDown(KeyCode.H)) || Input.GetKeyDown(KeyCode.Tab))
        {
            if (_keypadRoot != null && _keypadRoot.activeSelf)
            {
                ShowHistoryTab();
            }
            else
            {
                ShowKeypadTab();
            }
            return;
        }

        // Digits 0-9
        if (Input.GetKeyDown(KeyCode.Keypad0) || Input.GetKeyDown(KeyCode.Alpha0)) _engine.InputDigit(0);
        else if (Input.GetKeyDown(KeyCode.Keypad1) || Input.GetKeyDown(KeyCode.Alpha1)) _engine.InputDigit(1);
        else if (Input.GetKeyDown(KeyCode.Keypad2) || Input.GetKeyDown(KeyCode.Alpha2)) _engine.InputDigit(2);
        else if (Input.GetKeyDown(KeyCode.Keypad3) || Input.GetKeyDown(KeyCode.Alpha3)) _engine.InputDigit(3);
        else if (Input.GetKeyDown(KeyCode.Keypad4) || Input.GetKeyDown(KeyCode.Alpha4)) _engine.InputDigit(4);
        else if (Input.GetKeyDown(KeyCode.Keypad5) || Input.GetKeyDown(KeyCode.Alpha5)) _engine.InputDigit(5);
        else if (Input.GetKeyDown(KeyCode.Keypad6) || Input.GetKeyDown(KeyCode.Alpha6)) _engine.InputDigit(6);
        else if (Input.GetKeyDown(KeyCode.Keypad7) || Input.GetKeyDown(KeyCode.Alpha7)) _engine.InputDigit(7);
        else if (Input.GetKeyDown(KeyCode.Keypad8) || Input.GetKeyDown(KeyCode.Alpha8)) _engine.InputDigit(8);
        else if (Input.GetKeyDown(KeyCode.Keypad9) || Input.GetKeyDown(KeyCode.Alpha9)) _engine.InputDigit(9);

        // Operators
        else if (Input.GetKeyDown(KeyCode.KeypadPlus) || Input.GetKeyDown(KeyCode.Plus)) _engine.InputOperator("+");
        else if (Input.GetKeyDown(KeyCode.KeypadMinus) || Input.GetKeyDown(KeyCode.Minus)) _engine.InputOperator("-");
        else if (Input.GetKeyDown(KeyCode.KeypadMultiply) || Input.GetKeyDown(KeyCode.Asterisk)) _engine.InputOperator("×");
        else if (Input.GetKeyDown(KeyCode.KeypadDivide) || Input.GetKeyDown(KeyCode.Slash)) _engine.InputOperator("÷");

        // Decimal
        else if (Input.GetKeyDown(KeyCode.KeypadPeriod) || Input.GetKeyDown(KeyCode.Period) || Input.GetKeyDown(KeyCode.Comma)) _engine.InputDecimal();

        // Equals / Enter
        else if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Equals)) _engine.InputEquals();

        // Backspace
        else if (Input.GetKeyDown(KeyCode.Backspace)) _engine.InputBackspace();

        // Clear / Delete (C only when not holding Ctrl)
        else if (Input.GetKeyDown(KeyCode.Delete) || (!ctrl && Input.GetKeyDown(KeyCode.C))) _engine.InputClear();
    }

    // =====================================================================
    // 7. Helpers & Icon Generation
    // =====================================================================

    private static string FormatRelativeTimestamp(DateTime utc)
    {
        var local = utc.ToLocalTime();
        var diff = DateTime.Now - local;

        if (diff.TotalMinutes < 1) return "Just now";
        if (diff.TotalMinutes < 60) return $"{Math.Max(1, (int)diff.TotalMinutes)}m ago";
        if (diff.TotalHours < 24 && local.Date == DateTime.Today) return local.ToString("HH:mm", CultureInfo.InvariantCulture);
        if (diff.TotalDays < 2) return "Yesterday";
        return local.ToString("MMM d", CultureInfo.InvariantCulture);
    }

    private static Sprite GenerateProgrammaticIcon()
    {
        const int size = 64;
        var tex = new Texture2D(size, size, TextureFormat.RGBA32, false);

        Color bodyBg = new(0.12f, 0.15f, 0.22f, 1f);
        Color screenColor = new(0.18f, 0.65f, 0.98f, 1f);
        Color btnFunc = new(0.95f, 0.58f, 0.15f, 1f);
        Color btnNum = new(0.30f, 0.36f, 0.48f, 1f);
        Color clear = new(0f, 0f, 0f, 0f);

        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                // Rounded outer rectangle
                if (x < 4 || x >= size - 4 || y < 4 || y >= size - 4)
                {
                    tex.SetPixel(x, y, clear);
                    continue;
                }

                // Calculator mini screen (top portion)
                if (y >= 44 && y <= 54 && x >= 10 && x <= size - 10)
                {
                    tex.SetPixel(x, y, screenColor);
                }
                // Right operator strip
                else if (x >= 42 && x <= 50 && y >= 12 && y <= 38)
                {
                    tex.SetPixel(x, y, btnFunc);
                }
                // Number button dots grid
                else if (((x >= 12 && x <= 20) || (x >= 24 && x <= 32)) &&
                         ((y >= 12 && y <= 18) || (y >= 22 && y <= 28) || (y >= 32 && y <= 38)))
                {
                    tex.SetPixel(x, y, btnNum);
                }
                else
                {
                    tex.SetPixel(x, y, bodyBg);
                }
            }
        }

        tex.Apply();
        var sprite = Sprite.Create(tex, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f));
        sprite.name = "CalculatorProgrammaticIcon";
        return sprite;
    }
}
