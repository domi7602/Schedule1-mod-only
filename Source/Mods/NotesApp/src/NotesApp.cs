using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.Json;
using Il2CppScheduleOne.Persistence;
using MelonLoader;
using MelonLoader.Utils;
using S1API.GameTime;
using S1API.PhoneApp;
using S1API.UI;
using S1API.Utils;
using S1Mods.Shared;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object; // disambiguate from System.Object

namespace NotesApp;

public sealed class Note
{
    public string Title { get; set; } = string.Empty;
    public string Text { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public bool IsPinned { get; set; } = false;
}

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

public sealed class NotesApp : PhoneApp
{
    private const string SaveFileName = "notes.json";

    // --- Dynamic version badge from MelonInfo ---
    private static readonly string _versionString = GetVersionString();
    private static string GetVersionString()
    {
        var attr = (MelonInfoAttribute?)Attribute.GetCustomAttribute(typeof(Mod).Assembly, typeof(MelonInfoAttribute));
        return attr != null ? "v" + attr.Version : "v?";
    }

    // --- Relative Layout Anchors ---
    private const float HeaderHeight = 0.09f;
    private const float SearchBarTop = 0.905f;
    private const float SearchBarBottom = 0.840f;
    private const float ListTop = 0.835f;
    private const float ListBottom = 0.105f;
    private const float BottomBarHeight = 0.10f;
    private const float RowSpacing = 8f;

    // --- UI Color Palette ---
    private static readonly Color BgColor = new(0.08f, 0.09f, 0.12f, 1f);
    private static readonly Color HeaderBgColor = new(0.09f, 0.10f, 0.14f, 1f);
    private static readonly Color CardBgColor = new(0.12f, 0.13f, 0.18f, 1f);
    private static readonly Color CardPinnedBgColor = new(0.14f, 0.16f, 0.22f, 1f);
    private static readonly Color InputBgColor = new(0.12f, 0.14f, 0.20f, 1f);
    private static readonly Color PrimaryBtnColor = new(0.23f, 0.51f, 0.96f, 1f);
    private static readonly Color SecondaryBtnColor = new(0.20f, 0.23f, 0.30f, 1f);
    private static readonly Color DangerBtnColor = new(0.85f, 0.25f, 0.25f, 1f);
    private static readonly Color PinnedAmberColor = new(0.98f, 0.73f, 0.18f, 1f);
    private static readonly Color DividerColor = new(1f, 1f, 1f, 0.08f);
    private static readonly Color SubtitleColor = new(0.65f, 0.72f, 0.82f, 1f);

    // --- Data ---
    private readonly List<Note> _notes = new();
    private string _savePath = null!;
    private string _lastKnownSlot = "default";
    private string _searchQuery = string.Empty;

    private string GetSaveSlotSuffix()
    {
        try
        {
            // Use PersistentSingleton for robustness (matches BankApp/TransactionHistoryService)
            var loadMgr = Il2CppScheduleOne.DevUtilities.PersistentSingleton<LoadManager>.Instance;
            if (loadMgr != null && loadMgr.Pointer != IntPtr.Zero && !loadMgr.WasCollected)
            {
                var info = loadMgr.ActiveSaveInfo;
                if (info != null && info.Pointer != IntPtr.Zero && !info.WasCollected
                    && info.SaveSlotNumber >= 0)
                {
                    string slot = $"slot_{info.SaveSlotNumber}";
                    _lastKnownSlot = slot;
                    return slot;
                }
            }
            // Fallback to legacy LoadManager.Instance (in case PersistentSingleton not ready)
            var legacyInfo = LoadManager.Instance?.ActiveSaveInfo;
            if (legacyInfo != null && legacyInfo.Pointer != IntPtr.Zero && !legacyInfo.WasCollected
                && legacyInfo.SaveSlotNumber >= 0)
            {
                string slot = $"slot_{legacyInfo.SaveSlotNumber}";
                _lastKnownSlot = slot;
                return slot;
            }
        }
        catch { }
        return _lastKnownSlot;
    }

    private string GetNotesPath()
    {
        string suffix = GetSaveSlotSuffix();
        string path = SafeStorage.GetUserDataPath("NotesApp", $"notes_{suffix}.json");
        TryMigrateLegacyForPath(path);
        return path;
    }

    private void TryMigrateLegacyForPath(string slotPath)
    {
        try
        {
            string legacyPath = SafeStorage.GetUserDataPath("NotesApp", SaveFileName);
            if (!File.Exists(legacyPath)) return;
            if (File.Exists(slotPath)) { try { File.Delete(legacyPath); } catch { } return; }
            File.Move(legacyPath, slotPath);
        }
        catch { }
    }
    private int _editingIndex = -1;
    private int _detailIndex = -1;
    private int _pendingDeleteIndex = -1;

    // --- UI References ---
    private RectTransform _listContent = null!;
    private ScrollRect _listScrollRect = null!;
    private GameObject _mainBG = null!;
    private GameObject _listRoot = null!;
    private GameObject _detailRoot = null!;
    private GameObject _editorRoot = null!;
    private GameObject _confirmRoot = null!;
    private Text _detailTitle = null!;
    private Text _detailPinButtonText = null!;
    private RectTransform _detailBodyContent = null!;
    private Text _editorHeader = null!;
    private Text _confirmBody = null!;
    private Text _wordCountText = null!;
    private InputField _searchInput = null!;
    private InputField _titleInput = null!;
    private InputField _textInput = null!;
    private Text _copyButtonText = null!;
    private float _copyResetTimer = -1f;

    protected override string AppName => "NotesApp";
    protected override string AppTitle => "Notes";
    protected override string IconLabel => "Notes";
    protected override string IconFileName => "notiz_app_lowpoly_fancy.png";
    protected override EOrientation Orientation => EOrientation.Vertical;

    private void Update()
    {
        bool open = IsOpen();
        if (_mainBG != null && _mainBG.activeSelf != open)
        {
            _mainBG.SetActive(open);
        }

        if (!open) return;

        // Reset "Copied!" button feedback timer
        if (_copyResetTimer > 0f)
        {
            _copyResetTimer -= Time.unscaledDeltaTime;
            if (_copyResetTimer <= 0f && _copyButtonText != null)
            {
                _copyButtonText.text = "Copy";
            }
        }

        HandleKeyboardShortcuts();
    }

    private void HandleKeyboardShortcuts()
    {
        bool ctrl = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);

        // Escape: Back / Cancel
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_confirmRoot != null && _confirmRoot.activeSelf)
            {
                CancelConfirm();
            }
            else if (_editorRoot != null && _editorRoot.activeSelf)
            {
                CancelEditor();
            }
            else if (_detailRoot != null && _detailRoot.activeSelf)
            {
                ShowList();
            }
        }

        // Tab: Focus switch between Title and Body in Editor
        if (Input.GetKeyDown(KeyCode.Tab) && _editorRoot != null && _editorRoot.activeSelf)
        {
            if (_titleInput != null && _titleInput.isFocused && _textInput != null)
            {
                _textInput.Select();
                _textInput.ActivateInputField();
            }
            else if (_textInput != null && _textInput.isFocused && _titleInput != null)
            {
                _titleInput.Select();
                _titleInput.ActivateInputField();
            }
        }

        // Ctrl + S: Instant Save in Editor
        if (ctrl && Input.GetKeyDown(KeyCode.S))
        {
            if (_editorRoot != null && _editorRoot.activeSelf)
            {
                SaveEditor();
            }
        }

        // Ctrl + N: New Note from List
        if (ctrl && Input.GetKeyDown(KeyCode.N))
        {
            if (_listRoot != null && _listRoot.activeSelf)
            {
                OpenNewNote();
            }
        }

        // Ctrl + F: Focus Search
        if (ctrl && Input.GetKeyDown(KeyCode.F))
        {
            if (_listRoot != null && _listRoot.activeSelf && _searchInput != null)
            {
                _searchInput.Select();
                _searchInput.ActivateInputField();
            }
        }
    }

    protected override void OnPhoneClosed()
    {
        base.OnPhoneClosed();
        if (_mainBG != null) _mainBG.SetActive(false);
        // Update bleibt lebenslang subscribed (defensives Unsubscribe-Subscribe in OnCreated).
    }

    // Fix (Bug-Audit 2026-09-10): the phone re-instantiates this app per scene load and the old
    // per-instance handlers stayed in the static invocation lists forever (memory retention +
    // N redundant executions per event). Static events now dispatch through _active, subscribed
    // exactly once; Mod.OnSceneWasUnloaded clears _active when the gameplay scene tears down.
    private static NotesApp? _active;
    private static bool _staticSubscribed;

    protected override void OnCreated()
    {
        _savePath = GetNotesPath();
        Load();
        base.OnCreated();
        _active = this;
        if (_staticSubscribed) return;
        _staticSubscribed = true;
        MelonEvents.OnUpdate.Subscribe(DispatchUpdate);
        // Subscribe to slot changes so path stays fresh after save-slot switch without scene reload.
        try { S1API.Lifecycle.GameLifecycle.OnSaveInfoLoaded += DispatchSaveInfoLoaded; } catch { }
        try { S1API.Lifecycle.GameLifecycle.OnLoadComplete += DispatchLoadComplete; } catch { }
    }

    internal static void TearDownForSceneUnload() => _active = null;

    private static void DispatchUpdate() => _active?.Update();

    private static void DispatchSaveInfoLoaded()
    {
        try { _active?.OnSaveInfoLoaded(); } catch { }
    }

    private static void DispatchLoadComplete()
    {
        try { _active?.OnLoadComplete(); } catch { }
    }

    private void OnSaveInfoLoaded()
    {
        try
        {
            _savePath = GetNotesPath();
            Load();
            RefreshList();
        }
        catch { }
    }

    private void OnLoadComplete()
    {
        try
        {
            _savePath = GetNotesPath();
            Load();
            RefreshList();
        }
        catch { }
    }

    protected override void OnCreatedUI(GameObject container)
    {
        var containerRt = container.GetComponent<RectTransform>();
        if (containerRt != null)
        {
            UITheme.Initialize(containerRt);
            MelonLogger.Msg($"Responsive canvas initialized: {UITheme.ActualWidth:F0}x{UITheme.ActualHeight:F0} (Scale={UITheme.Scale:F2})");
        }

        _mainBG = UIFactory.Panel("Background", container.transform, BgColor, fullAnchor: true);
        _mainBG.SetActive(false);
        var panel = _mainBG;

        BuildListScreen(panel);
        BuildDetailScreen(panel);
        BuildEditorScreen(panel);
        BuildConfirmScreen(panel);

        var focusHook = container.AddComponent<NotesAppInputFocus>();
        focusHook.titleInput = _titleInput;
        focusHook.textInput = _textInput;
        focusHook.searchInput = _searchInput;

        RefreshList();
    }

    // =====================================================================
    // 1. List View (Search + Scrollable Notes + Bottom Bar)
    // =====================================================================

    private void BuildListScreen(GameObject panel)
    {
        _listRoot = new GameObject("ListScreen");
        _listRoot.transform.SetParent(panel.transform, false);
        var rootRt = _listRoot.AddComponent<RectTransform>();
        rootRt.anchorMin = Vector2.zero;
        rootRt.anchorMax = Vector2.one;
        rootRt.offsetMin = Vector2.zero;
        rootRt.offsetMax = Vector2.zero;

        // === Header Bar ===
        var headerBar = UIFactory.Panel("HeaderBackground", _listRoot.transform, HeaderBgColor);
        var hbRt = headerBar.GetComponent<RectTransform>();
        hbRt.anchorMin = new Vector2(0f, 1f - HeaderHeight);
        hbRt.anchorMax = new Vector2(1f, 1f);
        hbRt.offsetMin = Vector2.zero;
        hbRt.offsetMax = Vector2.zero;

        var title = UIFactory.Text("Title", AppTitle, headerBar.transform, UITheme.Sp(34), TextAnchor.MiddleCenter, FontStyle.Bold);
        var titleRt = title.rectTransform;
        titleRt.anchorMin = Vector2.zero;
        titleRt.anchorMax = Vector2.one;
        titleRt.offsetMin = Vector2.zero;
        titleRt.offsetMax = Vector2.zero;

        var versionBadge = UIFactory.Text("VersionBadge", _versionString, headerBar.transform, UITheme.Sp(15), TextAnchor.MiddleRight, FontStyle.Normal);
        var vbRt = versionBadge.rectTransform;
        vbRt.anchorMin = new Vector2(0.70f, 0f);
        vbRt.anchorMax = new Vector2(0.95f, 1f);
        vbRt.offsetMin = Vector2.zero;
        vbRt.offsetMax = Vector2.zero;
        versionBadge.color = new Color(1f, 1f, 1f, 0.40f);
        versionBadge.raycastTarget = false;

        var headerLine = UIFactory.Panel("HeaderLine", _listRoot.transform, DividerColor);
        var hlRt = headerLine.GetComponent<RectTransform>();
        hlRt.anchorMin = new Vector2(0.04f, 1f - HeaderHeight);
        hlRt.anchorMax = new Vector2(0.96f, 1f - HeaderHeight);
        hlRt.offsetMin = Vector2.zero;
        hlRt.offsetMax = new Vector2(0f, 1.5f);

        // === Search Bar ===
        _searchInput = CreateInputField("SearchInput", "\ud83d\udd0d Search notes...", _listRoot.transform, multiLine: false);
        var searchRt = _searchInput.gameObject.GetComponent<RectTransform>();
        searchRt.anchorMin = new Vector2(0.04f, SearchBarBottom);
        searchRt.anchorMax = new Vector2(0.96f, SearchBarTop);
        searchRt.offsetMin = Vector2.zero;
        searchRt.offsetMax = Vector2.zero;
        EventHelper.AddListener<string>(OnSearchQueryChanged, _searchInput.onValueChanged);

        // === Scrollable List ===
        var list = UIFactory.ScrollableVerticalList("NoteList", _listRoot.transform, out var scrollRect);
        // Fix 2026-09-02: responsive scroll feel — S1API defaults (scrollSensitivity untouched =
        // Unity default 10 with phone-scale content) felt clunky. Snappier sensitivity, lighter
        // elasticity, slightly higher deceleration for controlled glide.
        scrollRect.scrollSensitivity = 35f;
        scrollRect.elasticity = 0.08f;
        scrollRect.decelerationRate = 0.16f;
        scrollRect.inertia = true;
        _listScrollRect = scrollRect;
        scrollRect.vertical = true;
        scrollRect.movementType = ScrollRect.MovementType.Clamped;
        var listRt = (RectTransform)list.parent.parent;
        listRt.anchorMin = new Vector2(0.04f, ListBottom);
        listRt.anchorMax = new Vector2(0.96f, ListTop);
        listRt.offsetMin = Vector2.zero;
        listRt.offsetMax = Vector2.zero;
        _listContent = list;
        _listContent.sizeDelta = new Vector2(0f, _listContent.sizeDelta.y);

        var vlg = _listContent.GetComponent<VerticalLayoutGroup>();
        if (vlg != null)
        {
            vlg.spacing = RowSpacing;
            vlg.padding = new RectOffset(0, 0, 6, 6);
            vlg.childControlWidth = true;
            vlg.childControlHeight = true;
            vlg.childForceExpandWidth = true;
            vlg.childForceExpandHeight = false;
        }

        // === Bottom Action Bar ("+ New Note") ===
        var bottomBar = new GameObject("BottomBar");
        bottomBar.transform.SetParent(_listRoot.transform, false);
        var bottomRt = bottomBar.AddComponent<RectTransform>();
        bottomRt.anchorMin = new Vector2(0f, 0f);
        bottomRt.anchorMax = new Vector2(1f, BottomBarHeight);
        bottomRt.offsetMin = Vector2.zero;
        bottomRt.offsetMax = Vector2.zero;

        UIFactory.Panel("BottomBackground", bottomBar.transform, HeaderBgColor, fullAnchor: true);

        var bottomLine = UIFactory.Panel("BottomLine", bottomBar.transform, DividerColor);
        var blRt = bottomLine.GetComponent<RectTransform>();
        blRt.anchorMin = new Vector2(0.04f, 0.98f);
        blRt.anchorMax = new Vector2(0.96f, 0.98f);
        blRt.offsetMin = Vector2.zero;
        blRt.offsetMax = new Vector2(0f, 1.5f);

        var (addMask, addButton, _) = UIFactory.RoundedButtonWithLabel(
            "AddNote", "+ New Note", bottomBar.transform,
            PrimaryBtnColor, 340f, UITheme.Dp(48f), UITheme.Sp(23), Color.white);
        var addRt = addMask.GetComponent<RectTransform>();
        addRt.anchorMin = new Vector2(0.05f, 0.14f);
        addRt.anchorMax = new Vector2(0.95f, 0.86f);
        addRt.offsetMin = Vector2.zero;
        addRt.offsetMax = Vector2.zero;
        EventHelper.AddListener(OpenNewNote, addButton.onClick);
    }

    private void OnSearchQueryChanged(string query)
    {
        _searchQuery = query?.Trim() ?? string.Empty;
        RefreshList();
    }

    // =====================================================================
    // 2. Detail View (Header Pin/Back + Body + 4-Button Action Bar)
    // =====================================================================

    private void BuildDetailScreen(GameObject panel)
    {
        _detailRoot = new GameObject("DetailScreen");
        _detailRoot.transform.SetParent(panel.transform, false);
        var rootRt = _detailRoot.AddComponent<RectTransform>();
        rootRt.anchorMin = Vector2.zero;
        rootRt.anchorMax = Vector2.one;
        rootRt.offsetMin = Vector2.zero;
        rootRt.offsetMax = Vector2.zero;
        _detailRoot.SetActive(false);

        // === Header Bar ===
        var headerBar = UIFactory.Panel("HeaderBackground", _detailRoot.transform, HeaderBgColor);
        var hbRt = headerBar.GetComponent<RectTransform>();
        hbRt.anchorMin = new Vector2(0f, 1f - HeaderHeight);
        hbRt.anchorMax = new Vector2(1f, 1f);
        hbRt.offsetMin = Vector2.zero;
        hbRt.offsetMax = Vector2.zero;

        // Back button (left)
        var (backMask, backButton, _) = UIFactory.RoundedButtonWithLabel(
            "Back", "\u2039 Back", headerBar.transform,
            SecondaryBtnColor, 100f, UITheme.Dp(38f), UITheme.Sp(20), Color.white);
        var backRt = backMask.GetComponent<RectTransform>();
        backRt.anchorMin = new Vector2(0.04f, 0.16f);
        backRt.anchorMax = new Vector2(0.24f, 0.84f);
        backRt.offsetMin = Vector2.zero;
        backRt.offsetMax = Vector2.zero;
        EventHelper.AddListener(ShowList, backButton.onClick);

        // Detail Title (center)
        _detailTitle = UIFactory.Text("DetailTitle", "", headerBar.transform, UITheme.Sp(26), TextAnchor.MiddleLeft, FontStyle.Bold);
        var dtRt = _detailTitle.rectTransform;
        dtRt.anchorMin = new Vector2(0.26f, 0f);
        dtRt.anchorMax = new Vector2(0.74f, 1f);
        dtRt.offsetMin = Vector2.zero;
        dtRt.offsetMax = Vector2.zero;
        _detailTitle.horizontalOverflow = HorizontalWrapMode.Wrap;
        _detailTitle.verticalOverflow = VerticalWrapMode.Truncate;
        _detailTitle.raycastTarget = false;

        // Pin/Unpin Toggle Button (right)
        var (pinMask, pinButton, pinText) = UIFactory.RoundedButtonWithLabel(
            "PinToggle", "\ud83d\udccc Pin", headerBar.transform,
            SecondaryBtnColor, 90f, UITheme.Dp(38f), UITheme.Sp(19), Color.white);
        _detailPinButtonText = pinText;
        var pinRt = pinMask.GetComponent<RectTransform>();
        pinRt.anchorMin = new Vector2(0.76f, 0.16f);
        pinRt.anchorMax = new Vector2(0.96f, 0.84f);
        pinRt.offsetMin = Vector2.zero;
        pinRt.offsetMax = Vector2.zero;
        EventHelper.AddListener(ToggleDetailPin, pinButton.onClick);

        var headerLine = UIFactory.Panel("HeaderLine", _detailRoot.transform, DividerColor);
        var hlRt = headerLine.GetComponent<RectTransform>();
        hlRt.anchorMin = new Vector2(0.04f, 1f - HeaderHeight);
        hlRt.anchorMax = new Vector2(0.96f, 1f - HeaderHeight);
        hlRt.offsetMin = Vector2.zero;
        hlRt.offsetMax = new Vector2(0f, 1.5f);

        // === Scrollable Text Area ===
        var bodyScroll = UIFactory.ScrollableVerticalList("DetailBody", _detailRoot.transform, out var bodyScrollRect);
        // Fix 2026-09-02: same responsive scroll tuning as the note list.
        bodyScrollRect.scrollSensitivity = 35f;
        bodyScrollRect.elasticity = 0.08f;
        bodyScrollRect.decelerationRate = 0.16f;
        bodyScrollRect.inertia = true;
        var bodyRt = (RectTransform)bodyScroll.parent.parent;
        bodyRt.anchorMin = new Vector2(0.05f, 0.12f);
        bodyRt.anchorMax = new Vector2(0.95f, 0.89f);
        bodyRt.offsetMin = Vector2.zero;
        bodyRt.offsetMax = Vector2.zero;
        _detailBodyContent = bodyScroll;
        var bodyVlg = _detailBodyContent.GetComponent<VerticalLayoutGroup>();
        if (bodyVlg != null)
        {
            bodyVlg.padding = new RectOffset(8, 8, 8, 8);
            bodyVlg.childControlWidth = true;
            bodyVlg.childControlHeight = true;
            bodyVlg.childForceExpandWidth = true;
            bodyVlg.childForceExpandHeight = false;
        }

        // === Bottom Bar: Edit / Copy / Duplicate / Delete ===
        var bottomBar = new GameObject("BottomBar");
        bottomBar.transform.SetParent(_detailRoot.transform, false);
        var bRt = bottomBar.AddComponent<RectTransform>();
        bRt.anchorMin = new Vector2(0f, 0f);
        bRt.anchorMax = new Vector2(1f, BottomBarHeight);
        bRt.offsetMin = Vector2.zero;
        bRt.offsetMax = Vector2.zero;

        UIFactory.Panel("BottomBg", bottomBar.transform, HeaderBgColor, fullAnchor: true);

        // 1. Edit Button
        var (editMask, editButton, _) = UIFactory.RoundedButtonWithLabel(
            "EditDetail", "Edit", bottomBar.transform,
            PrimaryBtnColor, 80f, UITheme.Dp(46f), UITheme.Sp(20), Color.white);
        var editRt = editMask.GetComponent<RectTransform>();
        editRt.anchorMin = new Vector2(0.04f, 0.14f);
        editRt.anchorMax = new Vector2(0.25f, 0.86f);
        editRt.offsetMin = Vector2.zero;
        editRt.offsetMax = Vector2.zero;
        EventHelper.AddListener(OpenEditFromDetail, editButton.onClick);

        // 2. Copy Button
        var (copyMask, copyButton, copyLabel) = UIFactory.RoundedButtonWithLabel(
            "CopyDetail", "Copy", bottomBar.transform,
            SecondaryBtnColor, 80f, UITheme.Dp(46f), UITheme.Sp(20), Color.white);
        _copyButtonText = copyLabel;
        var copyRt = copyMask.GetComponent<RectTransform>();
        copyRt.anchorMin = new Vector2(0.27f, 0.14f);
        copyRt.anchorMax = new Vector2(0.48f, 0.86f);
        copyRt.offsetMin = Vector2.zero;
        copyRt.offsetMax = Vector2.zero;
        EventHelper.AddListener(CopyDetailToClipboard, copyButton.onClick);

        // 3. Duplicate Button
        var (dupMask, dupButton, _) = UIFactory.RoundedButtonWithLabel(
            "DupDetail", "Clone", bottomBar.transform,
            SecondaryBtnColor, 80f, UITheme.Dp(46f), UITheme.Sp(20), Color.white);
        var dupRt = dupMask.GetComponent<RectTransform>();
        dupRt.anchorMin = new Vector2(0.50f, 0.14f);
        dupRt.anchorMax = new Vector2(0.72f, 0.86f);
        dupRt.offsetMin = Vector2.zero;
        dupRt.offsetMax = Vector2.zero;
        EventHelper.AddListener(DuplicateCurrentNote, dupButton.onClick);

        // 4. Delete Button
        var (delMask, delButton, _) = UIFactory.RoundedButtonWithLabel(
            "DeleteDetail", "Delete", bottomBar.transform,
            DangerBtnColor, 80f, UITheme.Dp(46f), UITheme.Sp(20), Color.white);
        var delRt = delMask.GetComponent<RectTransform>();
        delRt.anchorMin = new Vector2(0.74f, 0.14f);
        delRt.anchorMax = new Vector2(0.96f, 0.86f);
        delRt.offsetMin = Vector2.zero;
        delRt.offsetMax = Vector2.zero;
        EventHelper.AddListener(() => ShowConfirm(_detailIndex), delButton.onClick);
    }

    // =====================================================================
    // 3. Editor View (Header Stamp/Back + Inputs + Save/Cancel)
    // =====================================================================

    private void BuildEditorScreen(GameObject panel)
    {
        _editorRoot = new GameObject("EditorScreen");
        _editorRoot.transform.SetParent(panel.transform, false);
        var rootRt = _editorRoot.AddComponent<RectTransform>();
        rootRt.anchorMin = Vector2.zero;
        rootRt.anchorMax = Vector2.one;
        rootRt.offsetMin = Vector2.zero;
        rootRt.offsetMax = Vector2.zero;
        _editorRoot.SetActive(false);

        // === Header Bar ===
        var headerBar = UIFactory.Panel("HeaderBackground", _editorRoot.transform, HeaderBgColor);
        var hbRt = headerBar.GetComponent<RectTransform>();
        hbRt.anchorMin = new Vector2(0f, 1f - HeaderHeight);
        hbRt.anchorMax = new Vector2(1f, 1f);
        hbRt.offsetMin = Vector2.zero;
        hbRt.offsetMax = Vector2.zero;

        var (backMask, backButton, _) = UIFactory.RoundedButtonWithLabel(
            "Back", "\u2039 Back", headerBar.transform,
            SecondaryBtnColor, 100f, UITheme.Dp(38f), UITheme.Sp(20), Color.white);
        var backRt = backMask.GetComponent<RectTransform>();
        backRt.anchorMin = new Vector2(0.04f, 0.16f);
        backRt.anchorMax = new Vector2(0.24f, 0.84f);
        backRt.offsetMin = Vector2.zero;
        backRt.offsetMax = Vector2.zero;
        EventHelper.AddListener(CancelEditor, backButton.onClick);

        _editorHeader = UIFactory.Text("EditorTitle", "", headerBar.transform, UITheme.Sp(26), TextAnchor.MiddleLeft, FontStyle.Bold);
        var ehRt = _editorHeader.rectTransform;
        ehRt.anchorMin = new Vector2(0.26f, 0f);
        ehRt.anchorMax = new Vector2(0.72f, 1f);
        ehRt.offsetMin = Vector2.zero;
        ehRt.offsetMax = Vector2.zero;
        _editorHeader.raycastTarget = false;

        // Quick Stamp Button (right)
        var (stampMask, stampButton, _) = UIFactory.RoundedButtonWithLabel(
            "StampBtn", "\ud83d\udd52 Stamp", headerBar.transform,
            SecondaryBtnColor, 95f, UITheme.Dp(38f), UITheme.Sp(18), Color.white);
        var stampRt = stampMask.GetComponent<RectTransform>();
        stampRt.anchorMin = new Vector2(0.74f, 0.16f);
        stampRt.anchorMax = new Vector2(0.96f, 0.84f);
        stampRt.offsetMin = Vector2.zero;
        stampRt.offsetMax = Vector2.zero;
        EventHelper.AddListener(InsertQuickStamp, stampButton.onClick);

        var headerLine = UIFactory.Panel("HeaderLine", _editorRoot.transform, DividerColor);
        var hlRt = headerLine.GetComponent<RectTransform>();
        hlRt.anchorMin = new Vector2(0.04f, 1f - HeaderHeight);
        hlRt.anchorMax = new Vector2(0.96f, 1f - HeaderHeight);
        hlRt.offsetMin = Vector2.zero;
        hlRt.offsetMax = new Vector2(0f, 1.5f);

        // === Section Label: TITLE ===
        var titleLabel = UIFactory.Text("TitleLabel", "TITLE", _editorRoot.transform, UITheme.Sp(18), TextAnchor.MiddleLeft, FontStyle.Bold);
        var tlRt = titleLabel.rectTransform;
        tlRt.anchorMin = new Vector2(0.05f, 0.850f);
        tlRt.anchorMax = new Vector2(0.95f, 0.885f);
        tlRt.offsetMin = Vector2.zero;
        tlRt.offsetMax = Vector2.zero;
        titleLabel.color = SubtitleColor;
        titleLabel.raycastTarget = false;

        // === Title Input Field ===
        _titleInput = CreateInputField("TitleInput", "Title (optional)", _editorRoot.transform, multiLine: false);
        var titleRt = _titleInput.gameObject.GetComponent<RectTransform>();
        titleRt.anchorMin = new Vector2(0.05f, 0.775f);
        titleRt.anchorMax = new Vector2(0.95f, 0.845f);
        titleRt.offsetMin = Vector2.zero;
        titleRt.offsetMax = Vector2.zero;

        // === Section Label: CONTENT ===
        var contentLabel = UIFactory.Text("ContentLabel", "CONTENT", _editorRoot.transform, UITheme.Sp(18), TextAnchor.MiddleLeft, FontStyle.Bold);
        var clRt = contentLabel.rectTransform;
        clRt.anchorMin = new Vector2(0.05f, 0.715f);
        clRt.anchorMax = new Vector2(0.95f, 0.750f);
        clRt.offsetMin = Vector2.zero;
        clRt.offsetMax = Vector2.zero;
        contentLabel.color = SubtitleColor;
        contentLabel.raycastTarget = false;

        // === Note Text Input Field (multiline, starts top-left) ===
        _textInput = CreateInputField("TextInput", "Type a note...", _editorRoot.transform, multiLine: true);
        var textRt = _textInput.gameObject.GetComponent<RectTransform>();
        textRt.anchorMin = new Vector2(0.05f, 0.120f);
        textRt.anchorMax = new Vector2(0.95f, 0.710f);
        textRt.offsetMin = Vector2.zero;
        textRt.offsetMax = Vector2.zero;

        // === Live Word Count (bottom-right inside text input) ===
        _wordCountText = UIFactory.Text("WordCount", "", _textInput.gameObject.transform, UITheme.Sp(15), TextAnchor.LowerRight);
        var wcRt = _wordCountText.rectTransform;
        wcRt.anchorMin = new Vector2(0f, 0f);
        wcRt.anchorMax = new Vector2(1f, 0f);
        wcRt.offsetMin = new Vector2(12f, 6f);
        wcRt.offsetMax = new Vector2(-12f, 26f);
        _wordCountText.color = new Color(1f, 1f, 1f, 0.40f);
        _wordCountText.raycastTarget = false;

        EventHelper.AddListener<string>(UpdateWordCount, _textInput.onValueChanged);
        UpdateWordCount(_textInput.text);

        // === Bottom Bar: Cancel / Save ===
        var bottomBar = new GameObject("BottomBar");
        bottomBar.transform.SetParent(_editorRoot.transform, false);
        var bRt = bottomBar.AddComponent<RectTransform>();
        bRt.anchorMin = new Vector2(0f, 0f);
        bRt.anchorMax = new Vector2(1f, BottomBarHeight);
        bRt.offsetMin = Vector2.zero;
        bRt.offsetMax = Vector2.zero;

        UIFactory.Panel("BottomBg", bottomBar.transform, HeaderBgColor, fullAnchor: true);

        var (cancelMask, cancelButton, _) = UIFactory.RoundedButtonWithLabel(
            "Cancel", "Cancel", bottomBar.transform,
            SecondaryBtnColor, 160f, UITheme.Dp(48f), UITheme.Sp(23), Color.white);
        var cancelRt = cancelMask.GetComponent<RectTransform>();
        cancelRt.anchorMin = new Vector2(0.05f, 0.14f);
        cancelRt.anchorMax = new Vector2(0.48f, 0.86f);
        cancelRt.offsetMin = Vector2.zero;
        cancelRt.offsetMax = Vector2.zero;
        EventHelper.AddListener(CancelEditor, cancelButton.onClick);

        var (saveMask, saveButton, _) = UIFactory.RoundedButtonWithLabel(
            "Save", "Save", bottomBar.transform,
            PrimaryBtnColor, 160f, UITheme.Dp(48f), UITheme.Sp(23), Color.white);
        var saveRt = saveMask.GetComponent<RectTransform>();
        saveRt.anchorMin = new Vector2(0.52f, 0.14f);
        saveRt.anchorMax = new Vector2(0.95f, 0.86f);
        saveRt.offsetMin = Vector2.zero;
        saveRt.offsetMax = Vector2.zero;
        EventHelper.AddListener(SaveEditor, saveButton.onClick);
    }

    // =====================================================================
    // 4. Confirm View (Delete Confirmation Dialog)
    // =====================================================================

    private void BuildConfirmScreen(GameObject panel)
    {
        _confirmRoot = new GameObject("ConfirmScreen");
        _confirmRoot.transform.SetParent(panel.transform, false);
        var rootRt = _confirmRoot.AddComponent<RectTransform>();
        rootRt.anchorMin = Vector2.zero;
        rootRt.anchorMax = Vector2.one;
        rootRt.offsetMin = Vector2.zero;
        rootRt.offsetMax = Vector2.zero;
        _confirmRoot.SetActive(false);

        // Semi-transparent Dimmer
        UIFactory.Panel("Dimmer", _confirmRoot.transform, new Color(0f, 0f, 0f, 0.65f), fullAnchor: true);

        // Compact Dialog Box
        var dialog = UIFactory.Panel("Dialog", _confirmRoot.transform, InputBgColor, fullAnchor: false);
        var dlgRt = dialog.GetComponent<RectTransform>();
        dlgRt.anchorMin = new Vector2(0.08f, 0.35f);
        dlgRt.anchorMax = new Vector2(0.92f, 0.65f);
        dlgRt.offsetMin = Vector2.zero;
        dlgRt.offsetMax = Vector2.zero;

        // Header: "Delete Note?"
        var header = UIFactory.Text("Header", "Delete Note?", dialog.transform, UITheme.Sp(28), TextAnchor.MiddleCenter, FontStyle.Bold);
        var headerRt = header.rectTransform;
        headerRt.anchorMin = new Vector2(0.05f, 0.68f);
        headerRt.anchorMax = new Vector2(0.95f, 0.94f);
        headerRt.offsetMin = Vector2.zero;
        headerRt.offsetMax = Vector2.zero;
        header.raycastTarget = false;

        // Body Text
        _confirmBody = UIFactory.Text("Body", "", dialog.transform, UITheme.Sp(22), TextAnchor.MiddleCenter);
        var bodyRt = _confirmBody.rectTransform;
        bodyRt.anchorMin = new Vector2(0.06f, 0.32f);
        bodyRt.anchorMax = new Vector2(0.94f, 0.68f);
        bodyRt.offsetMin = Vector2.zero;
        bodyRt.offsetMax = Vector2.zero;
        _confirmBody.horizontalOverflow = HorizontalWrapMode.Wrap;
        _confirmBody.verticalOverflow = VerticalWrapMode.Overflow;
        _confirmBody.color = SubtitleColor;
        _confirmBody.raycastTarget = false;
        _confirmBody.supportRichText = false;

        // Divider
        var divider = UIFactory.Panel("Divider", dialog.transform, DividerColor);
        var divRt = divider.GetComponent<RectTransform>();
        divRt.anchorMin = new Vector2(0.06f, 0.30f);
        divRt.anchorMax = new Vector2(0.94f, 0.30f);
        divRt.offsetMin = Vector2.zero;
        divRt.offsetMax = new Vector2(0f, 1f);

        // Cancel Button (left)
        var (cancelMask, cancelButton, _) = UIFactory.RoundedButtonWithLabel(
            "Cancel", "Cancel", dialog.transform,
            SecondaryBtnColor, 140f, UITheme.Dp(44f), UITheme.Sp(23), Color.white);
        var cancelRt = cancelMask.GetComponent<RectTransform>();
        cancelRt.anchorMin = new Vector2(0.06f, 0.05f);
        cancelRt.anchorMax = new Vector2(0.48f, 0.25f);
        cancelRt.offsetMin = Vector2.zero;
        cancelRt.offsetMax = Vector2.zero;
        EventHelper.AddListener(CancelConfirm, cancelButton.onClick);

        // Delete Button (right, red)
        var (delMask, delButton, _) = UIFactory.RoundedButtonWithLabel(
            "ConfirmDelete", "Delete", dialog.transform,
            DangerBtnColor, 140f, UITheme.Dp(44f), UITheme.Sp(23), Color.white);
        var delRt = delMask.GetComponent<RectTransform>();
        delRt.anchorMin = new Vector2(0.52f, 0.05f);
        delRt.anchorMax = new Vector2(0.94f, 0.25f);
        delRt.offsetMin = Vector2.zero;
        delRt.offsetMax = Vector2.zero;
        EventHelper.AddListener(ConfirmDelete, delButton.onClick);
    }

    private InputField CreateInputField(string name, string placeholderText, Transform parent, bool multiLine)
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

        var alignment = multiLine ? TextAnchor.UpperLeft : TextAnchor.MiddleLeft;

        var text = UIFactory.Text("Text", "", go.transform, UITheme.Sp(23), alignment);
        var textRt = text.rectTransform;
        textRt.anchorMin = Vector2.zero;
        textRt.anchorMax = Vector2.one;
        textRt.offsetMin = multiLine ? new Vector2(14f, 14f) : new Vector2(14f, 0f);
        textRt.offsetMax = multiLine ? new Vector2(-14f, -14f) : new Vector2(-14f, 0f);
        text.supportRichText = false;
        text.raycastTarget = false;
        if (multiLine)
        {
            text.verticalOverflow = VerticalWrapMode.Overflow;
        }

        var placeholder = UIFactory.Text("Placeholder", placeholderText, go.transform, UITheme.Sp(23), alignment);
        var phRt = placeholder.rectTransform;
        phRt.anchorMin = Vector2.zero;
        phRt.anchorMax = Vector2.one;
        phRt.offsetMin = multiLine ? new Vector2(14f, 14f) : new Vector2(14f, 0f);
        phRt.offsetMax = multiLine ? new Vector2(-14f, -14f) : new Vector2(-14f, 0f);
        placeholder.supportRichText = false;
        placeholder.color = new Color(1f, 1f, 1f, 0.40f);
        placeholder.raycastTarget = false;
        if (multiLine)
        {
            placeholder.verticalOverflow = VerticalWrapMode.Overflow;
        }

        inputField.targetGraphic = bg.GetComponent<Graphic>();
        inputField.textComponent = text;
        inputField.placeholder = placeholder;
        inputField.caretWidth = 2;
        inputField.caretColor = Color.white;
        inputField.selectionColor = new Color(0.23f, 0.51f, 0.96f, 0.45f);
        if (multiLine)
        {
            inputField.lineType = InputField.LineType.MultiLineNewline;
        }
        return inputField;
    }

    // =====================================================================
    // Navigation & Actions
    // =====================================================================

    private void ShowList()
    {
        _listRoot.SetActive(true);
        _detailRoot.SetActive(false);
        _editorRoot.SetActive(false);
        _confirmRoot.SetActive(false);
        RefreshList();
    }

    private void ShowDetail(int index)
    {
        if (index < 0 || index >= _notes.Count) return;
        _detailIndex = index;
        var note = _notes[index];

        string prefix = note.IsPinned ? "\ud83d\udccc " : "";
        string displayTitle = string.IsNullOrWhiteSpace(note.Title) ? "(Untitled)" : note.Title;
        _detailTitle.text = prefix + displayTitle;
        _detailPinButtonText.text = note.IsPinned ? "Unpin" : "\ud83d\udccc Pin";

        for (int i = _detailBodyContent.childCount - 1; i >= 0; i--)
        {
            Object.Destroy(_detailBodyContent.GetChild(i).gameObject);
        }
        var bodyText = UIFactory.Text("Body", note.Text, _detailBodyContent, UITheme.Sp(25), TextAnchor.UpperLeft);
        var btRt = bodyText.rectTransform;
        btRt.anchorMin = Vector2.zero;
        btRt.anchorMax = Vector2.one;
        btRt.offsetMin = Vector2.zero;
        btRt.offsetMax = Vector2.zero;
        bodyText.horizontalOverflow = HorizontalWrapMode.Wrap;
        bodyText.verticalOverflow = VerticalWrapMode.Overflow;
        bodyText.supportRichText = false;
        bodyText.raycastTarget = false;
        Canvas.ForceUpdateCanvases();
        var bodyLE = bodyText.gameObject.AddComponent<LayoutElement>();
        bodyLE.preferredHeight = bodyText.preferredHeight + 8f;
        UIFactory.FitContentHeight(_detailBodyContent);

        _listRoot.SetActive(false);
        _detailRoot.SetActive(true);
        _editorRoot.SetActive(false);
        _confirmRoot.SetActive(false);
    }

    private void ToggleDetailPin()
    {
        if (_detailIndex < 0 || _detailIndex >= _notes.Count) return;
        var note = _notes[_detailIndex];
        note.IsPinned = !note.IsPinned;
        Save();
        ShowDetail(_detailIndex);
    }

    private void CopyDetailToClipboard()
    {
        if (_detailIndex < 0 || _detailIndex >= _notes.Count) return;
        var note = _notes[_detailIndex];
        string content = !string.IsNullOrWhiteSpace(note.Title)
            ? $"{note.Title}\n\n{note.Text}"
            : note.Text;
        GUIUtility.systemCopyBuffer = content;

        if (_copyButtonText != null)
        {
            _copyButtonText.text = "\u2713 Copied!";
            _copyResetTimer = 1.5f;
        }
    }

    private void DuplicateCurrentNote()
    {
        if (_detailIndex < 0 || _detailIndex >= _notes.Count) return;
        var original = _notes[_detailIndex];
        string newTitle = string.IsNullOrWhiteSpace(original.Title) ? "Note (Copy)" : $"{original.Title} (Copy)";
        var clone = new Note
        {
            Title = newTitle,
            Text = original.Text,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            IsPinned = false
        };
        _notes.Insert(0, clone);
        Save();
        ShowDetail(0);
    }

    private void OpenNewNote()
    {
        _editingIndex = -1;
        _editorHeader.text = "New Note";
        _titleInput.text = "";
        _textInput.text = "";
        SwitchToEditor();
    }

    private void OpenEditor(int index)
    {
        if (index < 0 || index >= _notes.Count) return;
        _editingIndex = index;
        var note = _notes[index];
        _editorHeader.text = "Edit Note";
        _titleInput.text = note.Title;
        _textInput.text = note.Text;
        SwitchToEditor();
    }

    private void OpenEditFromDetail()
    {
        OpenEditor(_detailIndex);
    }

    private void SwitchToEditor()
    {
        _listRoot.SetActive(false);
        _detailRoot.SetActive(false);
        _editorRoot.SetActive(true);
        _confirmRoot.SetActive(false);
        _titleInput.Select();
        _titleInput.ActivateInputField();
        _titleInput.MoveTextEnd(false);
    }

    private void CancelEditor()
    {
        _editingIndex = -1;
        ShowList();
    }

    private void SaveEditor()
    {
        var title = _titleInput.text?.Trim() ?? "";
        var text = _textInput.text?.Trim() ?? "";
        if (string.IsNullOrEmpty(title) && string.IsNullOrEmpty(text)) return;

        if (_editingIndex >= 0 && _editingIndex < _notes.Count)
        {
            _notes[_editingIndex].Title = title;
            _notes[_editingIndex].Text = text;
            _notes[_editingIndex].UpdatedAt = DateTime.UtcNow;
        }
        else
        {
            _notes.Insert(0, new Note
            {
                Title = title,
                Text = text,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsPinned = false
            });
        }
        _editingIndex = -1;
        Save();
        ShowList();
    }

    private void InsertQuickStamp()
    {
        string stamp = GetInGameStamp();
        if (_textInput == null) return;

        if (string.IsNullOrEmpty(_textInput.text))
        {
            _textInput.text = stamp;
        }
        else
        {
            _textInput.text += (string.IsNullOrEmpty(_textInput.text) || _textInput.text.EndsWith("\n") ? "" : "\n") + stamp;
        }
        _textInput.MoveTextEnd(false);
        UpdateWordCount(_textInput.text);
    }

    private string GetInGameStamp()
    {
        try
        {
            int elapsedDays = TimeManager.ElapsedDays;
            int currentTime = TimeManager.CurrentTime;
            Day currentDay = TimeManager.CurrentDay;

            int hours = currentTime / 100;
            int mins = currentTime % 100;
            return $"[Day {elapsedDays} ({currentDay}), {hours:D2}:{mins:D2}] ";
        }
        catch
        {
            return $"[{DateTime.Now:yyyy-MM-dd HH:mm}] ";
        }
    }

    private void DeleteNote(int index)
    {
        if (index < 0 || index >= _notes.Count) return;
        _notes.RemoveAt(index);
        if (_editingIndex == index) _editingIndex = -1;
        else if (_editingIndex > index) _editingIndex--;
        if (_detailIndex == index) _detailIndex = -1;
        else if (_detailIndex > index) _detailIndex--;
        Save();
    }

    // =====================================================================
    // Confirm Dialog Handlers
    // =====================================================================

    private void ShowConfirm(int index)
    {
        if (index < 0 || index >= _notes.Count) return;
        _pendingDeleteIndex = index;
        var note = _notes[index];
        var displayTitle = string.IsNullOrWhiteSpace(note.Title) ? "(Untitled)" : note.Title;
        _confirmBody.text = $"Delete \u201c{displayTitle}\u201d?\nThis action cannot be undone.";

        _listRoot.SetActive(false);
        _detailRoot.SetActive(false);
        _editorRoot.SetActive(false);
        _confirmRoot.SetActive(true);
    }

    private void CancelConfirm()
    {
        _pendingDeleteIndex = -1;
        _confirmRoot.SetActive(false);
        _detailRoot.SetActive(true);
    }

    private void ConfirmDelete()
    {
        DeleteNote(_pendingDeleteIndex);
        _pendingDeleteIndex = -1;
        ShowList();
    }

    // =====================================================================
    // List Rendering
    // =====================================================================

    private void RefreshList()
    {
        // Lifecycle hooks (OnSaveInfoLoaded/OnLoadComplete) can fire before
        // OnCreatedUI built the list — and after scene unload _listContent is a
        // dead wrapper. Both cases: data is already loaded, UI refresh can wait.
        if (_listContent == null) return;
        try
        {
            if (_listContent.Pointer == IntPtr.Zero || _listContent.WasCollected) return;
        }
        catch { return; }
        for (int i = _listContent.childCount - 1; i >= 0; i--)
        {
            Object.Destroy(_listContent.GetChild(i).gameObject);
        }

        if (_notes.Count == 0)
        {
            RenderEmptyCard("No notes yet.\nTap \u201c+ New Note\u201d below to get started.");
            return;
        }

        // Build list of indices matching filter
        var matchingIndices = new List<int>();
        for (int i = 0; i < _notes.Count; i++)
        {
            var note = _notes[i];
            if (string.IsNullOrEmpty(_searchQuery)
                || (!string.IsNullOrEmpty(note.Title) && note.Title.IndexOf(_searchQuery, StringComparison.OrdinalIgnoreCase) >= 0)
                || (!string.IsNullOrEmpty(note.Text) && note.Text.IndexOf(_searchQuery, StringComparison.OrdinalIgnoreCase) >= 0))
            {
                matchingIndices.Add(i);
            }
        }

        if (matchingIndices.Count == 0)
        {
            RenderEmptyCard($"No notes matching \u201c{_searchQuery}\u201d");
            return;
        }

        // Sort: Pinned first, then newest (UpdatedAt / CreatedAt)
        matchingIndices.Sort((a, b) =>
        {
            var na = _notes[a];
            var nb = _notes[b];
            if (na.IsPinned != nb.IsPinned)
            {
                return nb.IsPinned.CompareTo(na.IsPinned);
            }
            var dateA = na.UpdatedAt != default ? na.UpdatedAt : na.CreatedAt;
            var dateB = nb.UpdatedAt != default ? nb.UpdatedAt : nb.CreatedAt;
            return dateB.CompareTo(dateA);
        });

        float scaledRowHeight = UITheme.Dp(96f);

        foreach (var originalIndex in matchingIndices)
        {
            var note = _notes[originalIndex];

            var row = new GameObject("Note_" + originalIndex);
            var rowRt = row.AddComponent<RectTransform>();
            rowRt.anchorMin = Vector2.zero;
            rowRt.anchorMax = Vector2.one;
            rowRt.offsetMin = Vector2.zero;
            rowRt.offsetMax = Vector2.zero;
            row.transform.SetParent(_listContent, false);
            var rowLE = row.AddComponent<LayoutElement>();
            rowLE.minHeight = scaledRowHeight;
            rowLE.preferredHeight = scaledRowHeight;
            rowLE.flexibleWidth = 1f;
            rowLE.flexibleHeight = 0f;

            var rowBg = UIFactory.Panel("Background", row.transform, note.IsPinned ? CardPinnedBgColor : CardBgColor, fullAnchor: true);
            var rowButton = rowBg.AddComponent<Button>();
            rowButton.transition = Selectable.Transition.None;
            var targetIdx = originalIndex;
            EventHelper.AddListener(() => ShowDetail(targetIdx), rowButton.onClick);

            // Left accent strip (4px)
            Color accentColor = note.IsPinned ? PinnedAmberColor : AccentColorFor(!string.IsNullOrWhiteSpace(note.Title) ? note.Title : note.Text);
            var accent = UIFactory.Panel("Accent", row.transform, accentColor, fullAnchor: false);
            var accRt = accent.GetComponent<RectTransform>();
            accRt.anchorMin = new Vector2(0f, 0f);
            accRt.anchorMax = new Vector2(0f, 1f);
            accRt.offsetMin = Vector2.zero;
            accRt.offsetMax = new Vector2(note.IsPinned ? 5f : 4f, 0f);

            // Title on top-left
            bool hasTitle = !string.IsNullOrWhiteSpace(note.Title);
            string titleDisplay = (note.IsPinned ? "\ud83d\udccc " : "") + (hasTitle ? note.Title : "(Untitled)");
            var titleText = UIFactory.Text(
                "Title",
                titleDisplay,
                row.transform,
                UITheme.Sp(26),
                TextAnchor.MiddleLeft,
                FontStyle.Bold);
            titleText.horizontalOverflow = HorizontalWrapMode.Wrap;
            titleText.verticalOverflow = VerticalWrapMode.Truncate;
            titleText.raycastTarget = false;
            titleText.color = note.IsPinned ? PinnedAmberColor : (hasTitle ? Color.white : new Color(1f, 1f, 1f, 0.70f));
            var titleRt = titleText.rectTransform;
            titleRt.anchorMin = new Vector2(0f, 0.50f);
            titleRt.anchorMax = new Vector2(0.68f, 0.95f);
            titleRt.offsetMin = new Vector2(16f, 0f);
            titleRt.offsetMax = new Vector2(-6f, 0f);

            // Timestamp on top-right (shows last modified/created)
            var displayDate = note.UpdatedAt != default ? note.UpdatedAt : note.CreatedAt;
            var dateText = UIFactory.Text(
                "Date",
                FormatRelativeDate(displayDate),
                row.transform,
                UITheme.Sp(18),
                TextAnchor.MiddleRight,
                FontStyle.Normal);
            dateText.raycastTarget = false;
            dateText.color = SubtitleColor;
            var dateRt = dateText.rectTransform;
            dateRt.anchorMin = new Vector2(0.68f, 0.50f);
            dateRt.anchorMax = new Vector2(1f, 0.95f);
            dateRt.offsetMin = new Vector2(0f, 0f);
            dateRt.offsetMax = new Vector2(-14f, 0f);

            // Text preview on bottom
            var preview = UIFactory.Text(
                "Preview",
                note.Text,
                row.transform,
                UITheme.Sp(22),
                TextAnchor.MiddleLeft);
            preview.horizontalOverflow = HorizontalWrapMode.Wrap;
            preview.verticalOverflow = VerticalWrapMode.Truncate;
            preview.raycastTarget = false;
            preview.color = new Color(0.72f, 0.78f, 0.86f, 0.80f);
            var previewRt = preview.rectTransform;
            previewRt.anchorMin = new Vector2(0f, 0.06f);
            previewRt.anchorMax = new Vector2(1f, 0.48f);
            previewRt.offsetMin = new Vector2(16f, 0f);
            previewRt.offsetMax = new Vector2(-14f, 0f);
        }

        UIFactory.FitContentHeight(_listContent);
        RebuildListLayout();
    }

    private void RenderEmptyCard(string message)
    {
        var empty = new GameObject("Empty");
        empty.AddComponent<RectTransform>();
        empty.transform.SetParent(_listContent, false);

        UIFactory.Panel("Background", empty.transform, CardBgColor, fullAnchor: true);

        var emptyText = UIFactory.Text(
            "Text",
            message,
            empty.transform,
            UITheme.Sp(23),
            TextAnchor.MiddleCenter);
        var emptyRt = emptyText.rectTransform;
        emptyRt.anchorMin = Vector2.zero;
        emptyRt.anchorMax = Vector2.one;
        emptyRt.offsetMin = new Vector2(16f, 16f);
        emptyRt.offsetMax = new Vector2(-16f, -16f);
        emptyText.color = SubtitleColor;
        emptyText.raycastTarget = false;

        var emptyLayout = empty.AddComponent<LayoutElement>();
        emptyLayout.minHeight = UITheme.Dp(160f);
        emptyLayout.preferredHeight = UITheme.Dp(160f);

        UIFactory.FitContentHeight(_listContent);
    }

    private void RebuildListLayout()
    {
        if (_listScrollRect == null || _listContent == null) return;

        float savedPos = _listScrollRect.verticalNormalizedPosition;
        bool hadValidPos = !float.IsNaN(savedPos) && !float.IsInfinity(savedPos);

        LayoutRebuilder.ForceRebuildLayoutImmediate(_listContent);

        var viewport = _listScrollRect.viewport;
        if (viewport == null) return;
        if (_listContent.rect.height <= viewport.rect.height)
        {
            _listScrollRect.verticalNormalizedPosition = 1f;
            return;
        }

        if (hadValidPos)
        {
            _listScrollRect.verticalNormalizedPosition = savedPos;
        }
    }

    // =====================================================================
    // Persistence (SafeStorage with fallback migration)
    // =====================================================================

    private void Load()
    {
        try
        {
            // Ensure path is always slot-fresh (covers slot switch without scene reload)
            _savePath = GetNotesPath();
            var loaded = SafeStorage.LoadSafe<List<Note>>(_savePath, fallback: new List<Note>());
            _notes.Clear();
            if (loaded != null && loaded.Count > 0)
            {
                foreach (var note in loaded)
                {
                    if (note == null) continue;
                    if (note.UpdatedAt == default)
                    {
                        note.UpdatedAt = note.CreatedAt != default ? note.CreatedAt : DateTime.UtcNow;
                    }
                    if (note.CreatedAt == default)
                    {
                        note.CreatedAt = note.UpdatedAt;
                    }
                    _notes.Add(note);
                }
                return;
            }

            // Migration from legacy format (List<string>) -> title from first line
            if (File.Exists(_savePath))
            {
                try
                {
                    var json = SafeStorage.LoadTextSafe(_savePath, fallback: "");
                    var legacy = JsonSerializer.Deserialize<List<string>>(json);
                    if (legacy != null && legacy.Count > 0)
                    {
                        _notes.Clear();
                        foreach (var entry in legacy)
                        {
                            var trimmed = entry?.Trim();
                            if (string.IsNullOrEmpty(trimmed)) continue;
                            _notes.Add(new Note
                            {
                                Title = FirstLine(trimmed),
                                Text = trimmed,
                                CreatedAt = DateTime.UtcNow,
                                UpdatedAt = DateTime.UtcNow
                            });
                        }
                        Save();
                    }
                }
                catch
                {
                }
            }
        }
        catch (System.Exception ex)
        {
            MelonLogger.Error("Error loading notes: " + ex.Message);
        }
    }

    private static string FirstLine(string text)
    {
        int nl = text.IndexOfAny(new[] { '\n', '\r' });
        var first = nl >= 0 ? text.Substring(0, nl) : text;
        if (first.Length > 24) first = first.Substring(0, 24);
        return first;
    }

    private void Save()
    {
        try
        {
            _savePath = GetNotesPath();
            SafeStorage.SaveAtomic(_savePath, _notes);
        }
        catch (System.Exception ex)
        {
            MelonLogger.Error("Error saving notes: " + ex.Message);
        }
    }

    // =====================================================================
    // Visual Helpers
    // =====================================================================

    private static string FormatRelativeDate(DateTime utc)
    {
        var local = utc.ToLocalTime();
        var now = DateTime.Now;
        var date = local.Date;
        if (date == now.Date) return "Today";
        if (date == now.Date.AddDays(-1)) return "Yesterday";
        if (now.Year == date.Year) return date.ToString("MMM d", CultureInfo.InvariantCulture);
        return date.ToString("MMM d, yyyy", CultureInfo.InvariantCulture);
    }

    private static Color AccentColorFor(string key)
    {
        int hash = 0;
        foreach (char c in key) hash = hash * 31 + c;
        if (hash < 0) hash = -hash;
        float hue = (hash % 360) / 360f;
        return Color.HSVToRGB(hue, 0.55f, 0.85f);
    }

    // =====================================================================
    // Editor Helpers
    // =====================================================================

    private void UpdateWordCount(string text)
    {
        if (_wordCountText == null) return;
        if (string.IsNullOrEmpty(text))
        {
            _wordCountText.text = "";
            return;
        }
        _wordCountText.text = $"{CountWords(text)} W \u00b7 {text.Length} C";
    }

    private static int CountWords(string text)
    {
        if (string.IsNullOrWhiteSpace(text)) return 0;
        int count = 0;
        bool inWord = false;
        foreach (char c in text)
        {
            if (char.IsWhiteSpace(c)) inWord = false;
            else if (!inWord) { inWord = true; count++; }
        }
        return count;
    }
}
