using System;
using System.Collections.Generic;
using Il2CppScheduleOne.Messaging;
using Il2CppScheduleOne.UI.Phone.Messages;
using S1API.UI;
using S1API.Utils;
using S1Mods.Shared;
using UnityEngine;
using UnityEngine.UI;

namespace MessagesPlus;

/// <summary>
/// Injects the MessagesPlus UI into the VANILLA MessagesApp canvas (patch-only
/// mod — no separate PhoneApp, no homescreen icon). v0.4.0 surface:
///
///   1. Sticky search band directly under the vanilla title. The band takes its
///      height from the top of the vanilla conversation viewport (see
///      TryMakeRoom), so it overlaps neither the title nor the first
///      conversation:
///      row 1: light search field (drawn magnifier, placeholder, clear "x")
///             plus the vertical "..." overflow button;
///      row 2: category chips [All][Customer][Dealer][Supplier] on the left,
///             the unread counter on the right.
///      The two destructive actions live behind the "..." menu (Clear Read /
///      Clear All) instead of sitting permanently in the open.
///   2. A confirmation dialog (backdrop + card + Cancel/Confirm) shared by the
///      two destructive actions; its text states that only CUSTOMER
///      conversations are hidden.
///
/// The styling is deliberately LIGHT (white field, hairline borders, dark ink)
/// so the band reads as part of the vanilla Messages app rather than a dark
/// overlay. It is also glyph-free: magnifier, "x" and "..." are drawn from
/// <see cref="S1Mods.Shared.UISprites"/> shapes, because the built-in Arial
/// font renders emoji/icon glyphs as blanks.
///
/// Clear All / Clear Read hide conversations via
/// MSGConversation.SetEntryVisibility(false) — supplier and dealer threads and
/// threads with unknown/empty categories are NEVER touched. The search/filter
/// VIEW (InboxView) only toggles entry GameObjects and never mutates save state.
///
/// All clicks go through S1API ButtonUtils (IL2CPP-safe EventHelper) with a
/// defensive Remove-before-Add: S1API's EventHelper dedupes globally per
/// delegate instance, so after a UI rebuild the stale dict entry would make
/// every new control dead (WireClick/WireValueChanged). In multiplayer clients
/// the mutation rows are disabled (host-only mutations).
/// </summary>
public static class InboxUI
{
    // --- Band geometry (Dp). The band height is also exactly what TryMakeRoom
    //     takes from the top of the vanilla conversation viewport. ---
    private const float BandPadDp = 8f;
    private const float SearchHeightDp = 36f;
    private const float RowGapDp = 8f;
    private const float ChipsHeightDp = 30f;
    private const float MenuButtonDp = 36f;
    private const float BandTopFallbackDp = 64f;   // only used when the vanilla list cannot be located

    private static float BandHeightPx =>
        UITheme.Dp(BandPadDp) * 2f + UITheme.Dp(SearchHeightDp) + UITheme.Dp(RowGapDp) + UITheme.Dp(ChipsHeightDp);

    // --- Palette: light (native Messages look) and dark. The dark side reuses the
    //     shared S1Mods.Shared.GamePalette (the BankApp-verified dark look) so the
    //     app matches its sibling apps. Colors are read at BUILD time; dark mode is
    //     PERMANENT since v0.4.1 (config defaults to ON, no in-app toggle). ---
    private static bool DarkMode => ModConfig<MessagesPlusConfig>.Instance?.DarkMode ?? false;

    private static Color BandBg => DarkMode
        ? GamePalette.Header                                                   // sticky band
        : new Color(0.98f, 0.98f, 0.99f, 0.97f);
    private static Color HairLine => DarkMode
        ? GamePalette.Border                                                   // 1 px borders/separators
        : new Color(0.86f, 0.87f, 0.89f, 1f);
    private static Color FieldBg => DarkMode
        ? GamePalette.Card                                                     // search field
        : new Color(1f, 1f, 1f, 1f);
    private static Color FieldShadow => DarkMode
        ? new Color(0f, 0f, 0f, 0.35f)                                         // soft drop below the field
        : new Color(0f, 0f, 0f, 0.07f);
    private static Color Ink => DarkMode
        ? GamePalette.TextPrimary                                              // primary text
        : new Color(0.11f, 0.12f, 0.14f, 1f);
    private static Color InkDim => DarkMode
        ? GamePalette.TextMuted                                                // placeholder/meta/icons
        : new Color(0.47f, 0.49f, 0.53f, 1f);
    private static Color ChipBg => DarkMode
        ? GamePalette.Card                                                     // inactive chip
        : new Color(0.93f, 0.94f, 0.95f, 1f);
    private static Color ChipInk => DarkMode
        ? GamePalette.TextMuted
        : new Color(0.28f, 0.30f, 0.34f, 1f);
    private static Color ChipActiveBg => DarkMode
        ? GamePalette.Blue                                                     // active chip = selected accent
        : new Color(0.19f, 0.22f, 0.27f, 1f);
    private static Color MenuBg => DarkMode
        ? GamePalette.Card                                                     // "..." popup card
        : new Color(1f, 1f, 1f, 1f);
    private static Color ModalBackdrop => DarkMode
        ? new Color(0f, 0f, 0f, 0.55f)
        : new Color(0f, 0f, 0f, 0.45f);
    private static Color ModalCard => DarkMode
        ? GamePalette.Card
        : new Color(0.99f, 0.99f, 1f, 1f);
    private static Color NeutralBg => DarkMode
        ? GamePalette.CardAlt                                                  // secondary button
        : new Color(0.90f, 0.91f, 0.93f, 1f);
    private static Color NeutralInk => DarkMode
        ? GamePalette.TextPrimary
        : new Color(0.18f, 0.19f, 0.22f, 1f);
    private static Color DestructiveBg => DarkMode
        ? GamePalette.Red
        : new Color(0.85f, 0.25f, 0.25f, 1f);
    private static Color DestructiveInk => DarkMode
        ? GamePalette.Red                                                      // "Clear All" row text
        : new Color(0.86f, 0.22f, 0.20f, 1f);

    // --- Filter chip definitions (display order) ---
    private static readonly InboxFilter[] ChipFilters =
    {
        InboxFilter.All, InboxFilter.Customer, InboxFilter.Dealer, InboxFilter.Supplier
    };
    private static readonly string[] ChipLabels = { "All", "Customer", "Dealer", "Supplier" };
    private static readonly float[] ChipWidths = { 44f, 82f, 62f, 76f };

    // --- References ---
    private static MessagesApp? _app;
    private static GameObject? _toolbarRoot;
    private static GameObject? _modalRoot;
    private static Text? _modalTitle;
    private static Text? _modalMessage;
    private static InputField? _searchInput;
    private static Text? _unreadLabel;
    private static GameObject? _searchClear;           // drawn "x" inside the field (hidden while empty)
    private static readonly Button?[] _chipButtons = new Button?[4];
    private static readonly Text?[] _chipLabels = new Text?[4];
    private static GameObject? _menuRoot;              // "..." popup (Clear Read / Clear All)
    private static RectTransform? _menuCard;           // positioned under the "..." button on open
    private static InboxFilter _currentFilter = InboxFilter.All;
    private static int _lastUnreadShown = -1;
    private static Action? _pendingConfirm;
    private static float _nextTickTime;

    // Vanilla list surgery: the band takes its height off the top of the vanilla
    // conversation viewport so the two can never overlap. Restored on rebuild.
    private static RectTransform? _listViewport;
    private static Vector2 _listViewportOffsetMax;
    private static bool _listShifted;

    /// <summary>
    /// Builds the injected UI exactly once per MessagesApp instance. Called from
    /// the Start/SetOpen/Loaded postfixes; re-entrant and self-healing (rebuilds
    /// when the vanilla page was recreated by a scene reload or a previous build
    /// failed part-way).
    /// </summary>
    public static void EnsureBuilt(MessagesApp app)
    {
        try
        {
            if (!NetworkGuard.IsAlive(app)) return;

            bool sameApp = ReferenceEquals(_app, app);
            bool alive = NetworkGuard.IsAlive(_toolbarRoot) && NetworkGuard.IsAlive(_modalRoot);
            if (sameApp && alive)
            {
                UpdateUnreadLabel();
                return;
            }
            GameObject home = app.homePage;
            if (!NetworkGuard.IsAlive(home))
            {
                Mod.Log?.Debug("EnsureBuilt: homePage not ready yet — retrying on next open.");
                return;
            }

            // W4: destroy OUR old managed roots before rebuilding — a mid-build
            // exception can leave earlier roots alive in the scene; dropping the
            // references without Destroy would orphan them and the next build
            // would inject a second toolbar. These are our own injected objects
            // (distinct from the scene-unload case, where they die with the
            // scene and must never be Destroyed).
            DestroyManagedRoots();

            // Drop stale references from a previous scene before rebuilding.
            TearDownForSceneUnload();

            _app = app;
            RectTransform homeRt = home.GetComponent<RectTransform>();
            if (homeRt != null)
            {
                UITheme.InitializeForDashboard(homeRt);
            }

            BuildToolbar(home.transform);
            BuildMenu(home.transform);
            BuildConfirmModal(home.transform);
            AppTheme.SetOwnRoots(
                _toolbarRoot != null ? _toolbarRoot.transform : null,
                _menuRoot != null ? _menuRoot.transform : null,
                _modalRoot != null ? _modalRoot.transform : null);
            TryMakeRoom();
            RestyleChips();
            UpdateUnreadLabel();
            if (DarkMode) AppTheme.Apply(app); // whole-app dark mode (vanilla surfaces)
            Mod.Log?.Info("UI injected into vanilla MessagesApp.");
        }
        catch (Exception ex)
        {
            Mod.Log?.Error($"EnsureBuilt failed: {ex.Message}");
        }
    }

    /// <summary>
    /// Throttled self-healing tick (driven by Mod.OnUpdate, 1 s): re-applies the
    /// search/filter view (vanilla callbacks can re-show entries — v0.1.x TrashUI
    /// W5 lesson), refreshes the unread counter, and re-injects the UI when the
    /// vanilla page was rebuilt. Free when the phone is closed.
    /// </summary>
    public static void Tick()
    {
        try
        {
            if (Time.unscaledTime < _nextTickTime) return;
            _nextTickTime = Time.unscaledTime + 1.0f;

            if (_app == null || !NetworkGuard.IsAlive(_app)) return;
            if (!NetworkGuard.IsAlive(_toolbarRoot) || !NetworkGuard.IsAlive(_modalRoot))
            {
                EnsureBuilt(_app); // vanilla rebuilt its page or a build failed part-way
                return;
            }

            ReassertRoom();
            InboxView.Reapply();
            UpdateUnreadLabel();
            if (DarkMode) AppTheme.Apply(_app); // theme vanilla surfaces that appeared since the last tick
        }
        catch (Exception ex)
        {
            Mod.Log?.Warn($"Tick failed: {ex.Message}");
        }
    }

    /// <summary>Clears managed UI references on scene unload (objects die with the scene — never Destroy).</summary>
    public static void TearDownForSceneUnload()
    {
        AppTheme.ClearOwnRoots();
        _app = null;
        _toolbarRoot = null;
        _modalRoot = null;
        _modalTitle = null;
        _modalMessage = null;
        _menuRoot = null;
        _menuCard = null;
        _searchInput = null;
        _searchClear = null;
        _unreadLabel = null;
        Array.Clear(_chipButtons, 0, _chipButtons.Length);
        Array.Clear(_chipLabels, 0, _chipLabels.Length);
        _pendingConfirm = null;
        _lastUnreadShown = -1;
        _listViewport = null;   // the scene died together with its list — nothing left to restore
        _listShifted = false;
        // View state references entries of the dying scene — drop without touching them.
        InboxView.DropState();
    }

    /// <summary>
    /// W12 analogue for the whole app surface: called when the app closes so a
    /// half-finished dialog cannot reappear (with a stale pending action) and no
    /// stale search/filter view survives into the next open.
    /// </summary>
    public static void OnAppClosed()
    {
        ResetModal();
        HideMenu();
        _currentFilter = InboxFilter.All;
        RestyleChips();
        if (_searchInput != null && NetworkGuard.IsAlive(_searchInput))
        {
            // Fires onValueChanged → SetSearch("") (idempotent).
            _searchInput.text = string.Empty;
        }
        InboxView.ResetView();
        UpdateUnreadLabel();
    }

    /// <summary>
    /// Hides the confirmation dialog and drops the pending action.
    /// </summary>
    public static void ResetModal()
    {
        _pendingConfirm = null;
        if (NetworkGuard.IsAlive(_modalRoot))
        {
            _modalRoot!.SetActive(false);
        }
    }

    // ------------------------------------------------------------------
    // Builders
    // ------------------------------------------------------------------

    /// <summary>
    /// W4: destroys our managed UI roots while they are still alive (rebuild
    /// path only — see EnsureBuilt).
    /// </summary>
    private static void DestroyManagedRoots()
    {
        // The band took its height off the vanilla list — hand that space back
        // before rebuilding, otherwise every rebuild would shorten it again.
        UndoMakeRoom();
        DestroyIfAlive(_toolbarRoot);
        DestroyIfAlive(_menuRoot);
        DestroyIfAlive(_modalRoot);
    }

    private static void DestroyIfAlive(GameObject? go)
    {
        if (!NetworkGuard.IsAlive(go)) return;
        try
        {
            UnityEngine.Object.Destroy(go);
        }
        catch (Exception ex)
        {
            Mod.Log?.Warn($"DestroyManagedRoots failed: {ex.Message}");
        }
    }

    /// <summary>
    /// Sticky search band under the vanilla title: search field + "..." menu on
    /// the first row, category chips + unread counter on the second. The band is
    /// parked by <see cref="TryMakeRoom"/> directly above the vanilla list; the
    /// fallback offset only applies when that list cannot be located.
    /// </summary>
    private static void BuildToolbar(Transform parent)
    {
        GameObject root = UIFactory.Panel("MessagesPlus_Band", parent, BandBg,
            anchorMin: new Vector2(0f, 1f), anchorMax: new Vector2(1f, 1f));
        _toolbarRoot = root; // keep the ref immediately: a mid-build exception must not orphan the root

        RectTransform rt = root.GetComponent<RectTransform>();
        rt.pivot = new Vector2(0.5f, 1f);
        rt.sizeDelta = new Vector2(0f, BandHeightPx);
        rt.anchoredPosition = new Vector2(0f, -UITheme.Dp(BandTopFallbackDp));
        root.transform.SetAsLastSibling(); // render on top of the vanilla list

        AddHairLine(rt); // sticky-header separator along the bottom edge

        // Row 1: search field (flexes) + "..." overflow menu
        GameObject row1 = BuildRow("MessagesPlus_RowSearch", rt,
            SearchHeightDp, BandPadDp, TextAnchor.MiddleLeft);
        _searchInput = BuildSearchField(row1.transform);
        WireValueChanged(_searchInput, OnSearchQueryChanged);
        var focusHook = root.AddComponent<MessagesPlusInputFocus>();
        focusHook.searchInput = _searchInput;
        BuildMenuButton(row1.transform);

        // Row 2: chips on the left, unread counter on the right
        GameObject row2 = BuildRow("MessagesPlus_RowFilter", rt,
            ChipsHeightDp, BandPadDp + SearchHeightDp + RowGapDp, TextAnchor.MiddleLeft);
        for (int i = 0; i < ChipFilters.Length; i++)
        {
            InboxFilter filter = ChipFilters[i];
            var (mask, btn, label) = UIFactory.RoundedButtonWithLabel(
                $"MessagesPlus_Chip{i}", ChipLabels[i], row2.transform,
                ChipBg, UITheme.Dp(ChipWidths[i]), UITheme.Dp(ChipsHeightDp),
                (int)UITheme.Sp(17), ChipInk);
            btn.GetComponent<Image>().raycastTarget = true;
            WireClick(btn, () => OnChipClicked(filter));
            SetRaycastTarget(label, false);
            _chipButtons[i] = btn;
            _chipLabels[i] = label;
        }

        _unreadLabel = UIFactory.Text("MessagesPlus_Unread", "0 unread", row2.transform,
            (int)UITheme.Sp(16), TextAnchor.MiddleRight);
        _unreadLabel.color = InkDim;
        _unreadLabel.raycastTarget = false;
        _unreadLabel.horizontalOverflow = HorizontalWrapMode.Overflow;
        var unreadLe = _unreadLabel.gameObject.AddComponent<LayoutElement>();
        unreadLe.flexibleWidth = 1f; // eats the leftover space → the counter hugs the right edge
        unreadLe.preferredHeight = UITheme.Dp(ChipsHeightDp);
        unreadLe.minHeight = UITheme.Dp(ChipsHeightDp);
    }

    /// <summary>
    /// Transparent, manually anchored band row (full width, fixed height, own
    /// child widths). The row itself never takes raycasts (W9: invisible gaps
    /// must not swallow clicks meant for the vanilla UI).
    /// </summary>
    private static GameObject BuildRow(string name, Transform parent, float heightDp, float topDp, TextAnchor align)
    {
        GameObject row = UIFactory.Panel(name, parent, Color.clear,
            anchorMin: new Vector2(0f, 1f), anchorMax: new Vector2(1f, 1f));
        SetRaycastTarget(row.GetComponent<Image>(), false); // W9

        RectTransform rt = row.GetComponent<RectTransform>();
        rt.pivot = new Vector2(0.5f, 1f);
        rt.sizeDelta = new Vector2(0f, UITheme.Dp(heightDp));
        rt.anchoredPosition = new Vector2(0f, -UITheme.Dp(topDp));

        var hlg = row.AddComponent<HorizontalLayoutGroup>();
        hlg.childControlWidth = true;
        hlg.childControlHeight = true;
        hlg.childForceExpandWidth = false;
        hlg.childForceExpandHeight = false;
        hlg.spacing = UITheme.Dp(6f);
        hlg.childAlignment = align;
        hlg.padding = new RectOffset((int)UITheme.Dp(10f), (int)UITheme.Dp(10f), 0, 0);
        return row;
    }

    /// <summary>1 px separator along the parent's bottom edge (sticky-header look).</summary>
    private static void AddHairLine(RectTransform parent)
    {
        GameObject line = UIFactory.Panel("HairLine", parent, HairLine);
        RectTransform rt = line.GetComponent<RectTransform>();
        rt.anchorMin = new Vector2(0f, 0f);
        rt.anchorMax = new Vector2(1f, 0f);
        rt.pivot = new Vector2(0.5f, 0f);
        rt.sizeDelta = new Vector2(0f, 1f);
        rt.anchoredPosition = Vector2.zero;
        SetDecorSprite(line, null); // keeps the flat colour, disables raycasts
    }

    /// <summary>Sliced sprite for a panel; decoration never takes raycasts (W9).</summary>
    private static void SetDecorSprite(GameObject go, Sprite? sprite)
    {
        Image? img = go.GetComponent<Image>();
        if (img == null || !NetworkGuard.IsAlive(img)) return;
        if (sprite != null)
        {
            img.sprite = sprite;
            img.type = Image.Type.Sliced;
        }
        img.raycastTarget = false;
    }

    /// <summary>Stretches a rect to its parent with optional Dp insets.</summary>
    private static void Stretch(RectTransform rt, float leftDp = 0f, float rightDp = 0f,
        float topDp = 0f, float bottomDp = 0f)
    {
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = new Vector2(UITheme.Dp(leftDp), UITheme.Dp(bottomDp));
        rt.offsetMax = new Vector2(-UITheme.Dp(rightDp), -UITheme.Dp(topDp));
    }

    /// <summary>
    /// Light single-line search field: white capsule, 1 px hairline border, soft
    /// drop shadow, and shape-drawn magnifier + clear "x" (Arial renders icon
    /// glyphs as blanks, so nothing here relies on a font glyph).
    /// </summary>
    private static InputField BuildSearchField(Transform parent)
    {
        GameObject go = new GameObject("MessagesPlus_Search");
        go.transform.SetParent(parent, false);
        go.AddComponent<RectTransform>();
        var le = go.AddComponent<LayoutElement>();
        le.flexibleWidth = 1f;
        le.preferredHeight = UITheme.Dp(SearchHeightDp);
        le.minHeight = UITheme.Dp(SearchHeightDp);

        // Soft drop shadow — created first, so it renders behind the field.
        GameObject shadow = UIFactory.Panel("Shadow", go.transform, FieldShadow, fullAnchor: true);
        var shadowRt = shadow.GetComponent<RectTransform>();
        shadowRt.offsetMin = new Vector2(-UITheme.Dp(1f), -UITheme.Dp(3f));
        shadowRt.offsetMax = new Vector2(UITheme.Dp(1f), -UITheme.Dp(1f));
        SetDecorSprite(shadow, UISprites.Capsule());

        // 1 px border ring, covered by the white surface except at the rim.
        GameObject border = UIFactory.Panel("Border", go.transform, HairLine, fullAnchor: true);
        SetDecorSprite(border, UISprites.Capsule());

        GameObject bg = UIFactory.Panel("Background", go.transform, FieldBg, fullAnchor: true);
        var bgRt = bg.GetComponent<RectTransform>();
        bgRt.offsetMin = new Vector2(1f, 1f);
        bgRt.offsetMax = new Vector2(-1f, -1f);
        // The white surface IS the field's click surface and must stay a raycast
        // target: the click bubbles up to the InputField on this object. The
        // generic SetDecorSprite (W9) would disable it and make the field dead.
        Image? bgImg = bg.GetComponent<Image>();
        if (bgImg != null && NetworkGuard.IsAlive(bgImg))
        {
            bgImg.sprite = UISprites.Capsule();
            bgImg.type = Image.Type.Sliced;
            bgImg.raycastTarget = true;
        }

        var inputField = go.AddComponent<InputField>();
        inputField.targetGraphic = bg.GetComponent<Graphic>();

        BuildMagnifier(go.transform);

        var text = UIFactory.Text("Text", string.Empty, go.transform, (int)UITheme.Sp(20), TextAnchor.MiddleLeft);
        Stretch(text.rectTransform, leftDp: 36f, rightDp: 34f);
        text.color = Ink;
        text.supportRichText = false;
        text.raycastTarget = false;

        var placeholder = UIFactory.Text("Placeholder", "Search", go.transform,
            (int)UITheme.Sp(20), TextAnchor.MiddleLeft);
        Stretch(placeholder.rectTransform, leftDp: 36f, rightDp: 34f);
        placeholder.supportRichText = false;
        placeholder.color = InkDim;
        placeholder.raycastTarget = false;

        inputField.textComponent = text;
        inputField.placeholder = placeholder;
        inputField.caretWidth = 2;
        inputField.caretColor = Ink;
        inputField.selectionColor = new Color(0.22f, 0.45f, 0.90f, 0.30f);

        BuildClearButton(go.transform);
        return inputField;
    }

    /// <summary>Drawn magnifier (donut ring + rotated handle capsule).</summary>
    private static void BuildMagnifier(Transform parent)
    {
        GameObject box = UIFactory.Panel("Magnifier", parent, Color.clear,
            anchorMin: new Vector2(0f, 0.5f), anchorMax: new Vector2(0f, 0.5f));
        RectTransform rt = box.GetComponent<RectTransform>();
        rt.pivot = new Vector2(0f, 0.5f);
        rt.sizeDelta = new Vector2(UITheme.Dp(22f), UITheme.Dp(22f));
        rt.anchoredPosition = new Vector2(UITheme.Dp(11f), 0f);

        GameObject glass = UIFactory.Panel("Glass", box.transform, Ink,
            anchorMin: new Vector2(0f, 1f), anchorMax: new Vector2(0f, 1f));
        var glassRt = glass.GetComponent<RectTransform>();
        glassRt.pivot = new Vector2(0f, 1f);
        glassRt.sizeDelta = new Vector2(UITheme.Dp(14f), UITheme.Dp(14f));
        glassRt.anchoredPosition = Vector2.zero;
        SetDecorSprite(glass, UISprites.Donut());

        GameObject handle = UIFactory.Panel("Handle", box.transform, Ink,
            anchorMin: new Vector2(0f, 1f), anchorMax: new Vector2(0f, 1f));
        var handleRt = handle.GetComponent<RectTransform>();
        handleRt.pivot = new Vector2(0f, 0.5f);
        handleRt.sizeDelta = new Vector2(UITheme.Dp(8f), UITheme.Dp(2.4f));
        handleRt.anchoredPosition = new Vector2(UITheme.Dp(11.5f), -UITheme.Dp(11.5f));
        handleRt.localRotation = Quaternion.Euler(0f, 0f, 45f);
        SetDecorSprite(handle, UISprites.Capsule());
    }

    /// <summary>Drawn "x" inside the field; hidden while the search text is empty.</summary>
    private static void BuildClearButton(Transform parent)
    {
        GameObject hit = UIFactory.Panel("Clear", parent, Color.clear,
            anchorMin: new Vector2(1f, 0.5f), anchorMax: new Vector2(1f, 0.5f));
        RectTransform hitRt = hit.GetComponent<RectTransform>();
        hitRt.pivot = new Vector2(1f, 0.5f);
        hitRt.sizeDelta = new Vector2(UITheme.Dp(26f), UITheme.Dp(26f));
        hitRt.anchoredPosition = new Vector2(-UITheme.Dp(6f), 0f);

        var btn = hit.AddComponent<Button>();
        btn.transition = Selectable.Transition.None;
        hit.GetComponent<Image>().raycastTarget = true;
        WireClick(btn, OnClearSearchClicked);

        for (int i = 0; i < 2; i++)
        {
            GameObject bar = UIFactory.Panel($"Bar{i}", hit.transform, InkDim, fullAnchor: true);
            var barRt = bar.GetComponent<RectTransform>();
            barRt.offsetMin = new Vector2(UITheme.Dp(7f), UITheme.Dp(12.4f));
            barRt.offsetMax = new Vector2(-UITheme.Dp(7f), -UITheme.Dp(12.4f));
            barRt.localRotation = Quaternion.Euler(0f, 0f, i == 0 ? 45f : -45f);
            SetDecorSprite(bar, UISprites.Capsule());
        }

        _searchClear = hit;
        hit.SetActive(false);
    }

    /// <summary>Vertical "..." overflow button (three drawn dots).</summary>
    private static void BuildMenuButton(Transform parent)
    {
        GameObject root = UIFactory.Panel("MessagesPlus_MenuButton", parent, Color.clear);
        var le = root.AddComponent<LayoutElement>();
        le.preferredWidth = UITheme.Dp(MenuButtonDp);
        le.minWidth = UITheme.Dp(MenuButtonDp);
        le.preferredHeight = UITheme.Dp(MenuButtonDp);
        le.minHeight = UITheme.Dp(MenuButtonDp);

        var btn = root.AddComponent<Button>();
        btn.transition = Selectable.Transition.None;
        root.GetComponent<Image>().raycastTarget = true;
        WireClick(btn, ToggleMenu);

        for (int i = -1; i <= 1; i++)
        {
            GameObject dot = UIFactory.Panel($"Dot{i + 1}", root.transform, Ink);
            var dotRt = dot.GetComponent<RectTransform>();
            dotRt.sizeDelta = new Vector2(UITheme.Dp(4f), UITheme.Dp(4f));
            dotRt.anchoredPosition = new Vector2(0f, UITheme.Dp(7f) * i);
            SetDecorSprite(dot, UISprites.Circle());
        }
    }

    /// <summary>
    /// White popup card holding the two destructive actions (host-only, disabled
    /// rows on a multiplayer client). A full-page backdrop closes it again.
    /// </summary>
    private static void BuildMenu(Transform parent)
    {
        GameObject root = UIFactory.Panel("MessagesPlus_Menu", parent, Color.clear, fullAnchor: true);
        _menuRoot = root;

        GameObject backdrop = UIFactory.Panel("Backdrop", root.transform, new Color(0f, 0f, 0f, 0.02f), fullAnchor: true);
        var backdropBtn = backdrop.AddComponent<Button>();
        backdropBtn.transition = Selectable.Transition.None;
        WireClick(backdropBtn, HideMenu);

        GameObject border = UIFactory.Panel("CardBorder", root.transform, HairLine,
            anchorMin: new Vector2(1f, 1f), anchorMax: new Vector2(1f, 1f));
        _menuCard = border.GetComponent<RectTransform>();
        _menuCard.pivot = new Vector2(1f, 1f);
        _menuCard.sizeDelta = new Vector2(UITheme.Dp(212f), UITheme.Dp(120f)); // 3 rows + 2 separators
        SetDecorSprite(border, UISprites.Rounded(10f));

        GameObject card = UIFactory.Panel("Card", border.transform, MenuBg, fullAnchor: true);
        var cardRt = card.GetComponent<RectTransform>();
        cardRt.offsetMin = new Vector2(1f, 1f);
        cardRt.offsetMax = new Vector2(-1f, -1f);
        SetDecorSprite(card, UISprites.Rounded(9f));

        var vlg = card.AddComponent<VerticalLayoutGroup>();
        vlg.childControlWidth = true;
        vlg.childControlHeight = true;
        vlg.childForceExpandWidth = true;
        vlg.childForceExpandHeight = false;
        vlg.spacing = 0f;
        vlg.padding = new RectOffset(
            (int)UITheme.Dp(2f), (int)UITheme.Dp(2f),
            (int)UITheme.Dp(2f), (int)UITheme.Dp(2f));

        bool canMutate = NetworkGuard.IsHostOrSingleplayer();

        BuildMenuRow(card.transform, "MessagesPlus_MenuClearRead", "Clear Read",
            canMutate ? Ink : InkDim, canMutate ? OnClearReadClicked : null);
        AddMenuSeparator(card.transform);
        BuildMenuRow(card.transform, "MessagesPlus_MenuClearAll", "Clear All",
            canMutate ? DestructiveInk : InkDim, canMutate ? OnClearAllClicked : null);
        root.SetActive(false); // opened by the "..." button
    }

    private static void BuildMenuRow(Transform parent, string name, string label, Color ink, Action? onClick)
    {
        GameObject row = UIFactory.Panel(name, parent, Color.clear);
        var le = row.AddComponent<LayoutElement>();
        le.preferredHeight = UITheme.Dp(38f);
        le.minHeight = UITheme.Dp(38f);

        var btn = row.AddComponent<Button>();
        btn.transition = Selectable.Transition.None;
        btn.interactable = onClick != null;
        row.GetComponent<Image>().raycastTarget = true; // the row itself is the hit area
        if (onClick != null) WireClick(btn, onClick);

        Text txt = UIFactory.Text(name + "_Label", label, row.transform, (int)UITheme.Sp(20), TextAnchor.MiddleLeft);
        Stretch(txt.rectTransform, leftDp: 14f, rightDp: 14f);
        txt.color = ink;
        txt.raycastTarget = false;
    }

    private static void AddMenuSeparator(Transform parent)
    {
        GameObject sep = UIFactory.Panel("Separator", parent, HairLine);
        var le = sep.AddComponent<LayoutElement>();
        le.preferredHeight = 1f;
        le.minHeight = 1f;
        SetDecorSprite(sep, null);
    }

    private static void ToggleMenu()
    {
        if (_menuRoot != null && NetworkGuard.IsAlive(_menuRoot) && _menuRoot.activeSelf) HideMenu();
        else ShowMenu();
    }

    private static void ShowMenu()
    {
        try
        {
            if (_menuRoot == null || !NetworkGuard.IsAlive(_menuRoot)) return;

            // Park the card right under the "..." button (band top + row 1 + gap).
            if (_menuCard != null && NetworkGuard.IsAlive(_menuCard) &&
                _toolbarRoot != null && NetworkGuard.IsAlive(_toolbarRoot))
            {
                // IL2CPP: a (RectTransform) cast on .transform throws (the wrapper is
                // typed Transform) — fetch the rect via GetComponent instead.
                RectTransform? bandRt = _toolbarRoot.GetComponent<RectTransform>();
                if (bandRt != null)
                {
                    float bandTopY = bandRt.anchoredPosition.y;
                    _menuCard.anchoredPosition = new Vector2(
                        -UITheme.Dp(10f),
                        bandTopY - UITheme.Dp(BandPadDp + SearchHeightDp + 4f));
                }
            }

            _menuRoot.SetActive(true);
            _menuRoot.transform.SetAsLastSibling(); // above the band and the vanilla list
        }
        catch (Exception ex)
        {
            Mod.Log?.Warn($"ShowMenu failed: {ex.Message}");
        }
    }

    private static void HideMenu()
    {
        try
        {
            if (_menuRoot != null && NetworkGuard.IsAlive(_menuRoot) && _menuRoot.activeSelf)
            {
                _menuRoot.SetActive(false);
            }
        }
        catch (Exception ex)
        {
            Mod.Log?.Debug($"HideMenu failed: {ex.Message}");
        }
    }

    private static void OnClearSearchClicked()
    {
        try
        {
            if (_searchInput != null && NetworkGuard.IsAlive(_searchInput))
            {
                _searchInput.text = string.Empty; // fires onValueChanged → InboxView.SetSearch("")
            }
        }
        catch (Exception ex)
        {
            Mod.Log?.Warn($"ClearSearch failed: {ex.Message}");
        }
    }

    // ------------------------------------------------------------------
    // Making room in the vanilla layout
    // ------------------------------------------------------------------

    /// <summary>
    /// Takes the band's height off the top of the vanilla conversation viewport
    /// and parks the band exactly in that slot, so it can cover neither the title
    /// nor the first conversation. When the vanilla list cannot be located
    /// nothing is shifted and the fixed fallback offset stays in place.
    /// </summary>
    private static void TryMakeRoom()
    {
        try
        {
            _listViewport = null;
            _listShifted = false;
            if (_app == null || !NetworkGuard.IsAlive(_app)) return;

            GameObject home = _app.homePage;
            if (!NetworkGuard.IsAlive(home)) return;
            RectTransform homeRt = home.GetComponent<RectTransform>();
            if (homeRt == null) return;

            ScrollRect? scroll = home.GetComponentInChildren<ScrollRect>();
            if (scroll == null)
            {
                Mod.Log?.Info("Band: no vanilla ScrollRect found — keeping the fallback offset (no shift).");
                return;
            }

            RectTransform vp = scroll.viewport != null ? scroll.viewport : scroll.GetComponent<RectTransform>();
            if (vp == null || !NetworkGuard.IsAlive(vp)) return;

            float band = BandHeightPx;
            Vector3 vpTopWorld = vp.TransformPoint(new Vector3(0f, vp.rect.height * (1f - vp.pivot.y), 0f));
            Vector3 vpTopLocal = homeRt.InverseTransformPoint(vpTopWorld);
            float homeTop = homeRt.rect.height * (1f - homeRt.pivot.y);
            float listTopFromHomeTop = Mathf.Max(band, homeTop - vpTopLocal.y);

            RectTransform? bandRt = _toolbarRoot != null && NetworkGuard.IsAlive(_toolbarRoot)
                ? _toolbarRoot.GetComponent<RectTransform>()
                : null;
            if (bandRt != null)
            {
                bandRt.anchoredPosition = new Vector2(0f, -(listTopFromHomeTop - band));
            }

            _listViewport = vp;
            _listViewportOffsetMax = vp.offsetMax;
            vp.offsetMax = new Vector2(_listViewportOffsetMax.x, _listViewportOffsetMax.y - band);
            _listShifted = true;
            Mod.Log?.Info($"Band: list shortened by {band:0.#} px (band top {listTopFromHomeTop - band:0.#} px below the page top).");
        }
        catch (Exception ex)
        {
            _listViewport = null;
            _listShifted = false;
            Mod.Log?.Warn($"Band: could not make room ({ex.Message}) — fallback offset kept.");
        }
    }

    /// <summary>Hands the borrowed space back to the vanilla list.</summary>
    private static void UndoMakeRoom()
    {
        try
        {
            if (_listShifted && _listViewport != null && NetworkGuard.IsAlive(_listViewport))
            {
                _listViewport.offsetMax = _listViewportOffsetMax;
            }
        }
        catch (Exception ex)
        {
            Mod.Log?.Debug($"Band: viewport restore failed: {ex.Message}");
        }
        finally
        {
            _listShifted = false;
            _listViewport = null;
        }
    }

    /// <summary>
    /// Vanilla re-lays out the list on its own events (message arrival, entry
    /// rebuilds) — re-assert the borrowed space from the throttled tick.
    /// </summary>
    private static void ReassertRoom()
    {
        if (!_listShifted) return;
        if (_listViewport == null || !NetworkGuard.IsAlive(_listViewport))
        {
            _listShifted = false;
            return;
        }

        try
        {
            Vector2 expected = new(_listViewportOffsetMax.x, _listViewportOffsetMax.y - BandHeightPx);
            if ((_listViewport.offsetMax - expected).sqrMagnitude > 0.01f)
            {
                _listViewport.offsetMax = expected;
                Mod.Log?.Debug("Band: vanilla re-laid out the list — borrowed space re-asserted.");
            }
        }
        catch (Exception ex)
        {
            Mod.Log?.Debug($"Band: room re-assert failed: {ex.Message}");
        }
    }

    private static void BuildConfirmModal(Transform parent)
    {
        // Full-stretch backdrop inside the vanilla home page (renders above the
        // list; hidden by default). Backdrop click cancels.
        GameObject modal = UIFactory.Panel("MessagesPlus_ConfirmModal", parent, ModalBackdrop, fullAnchor: true);
        _modalRoot = modal; // keep the ref immediately (see BuildToolbar)
        modal.SetActive(false);
        Button backdropBtn = modal.AddComponent<Button>();
        backdropBtn.transition = Selectable.Transition.None;
        WireClick(backdropBtn, HideConfirm);

        GameObject card = UIFactory.Panel("MessagesPlus_ConfirmCard", modal.transform, ModalCard);
        Image? cardImg = card.GetComponent<Image>();
        if (cardImg != null)
        {
            cardImg.sprite = UISprites.Rounded(8f);
            cardImg.type = Image.Type.Sliced;
        }
        RectTransform cardRt = card.GetComponent<RectTransform>();
        cardRt.anchorMin = new Vector2(0.5f, 0.5f);
        cardRt.anchorMax = new Vector2(0.5f, 0.5f);
        cardRt.sizeDelta = new Vector2(UITheme.Dp(380f), UITheme.Dp(190f));

        // W8: clicks on the card (background / title / message) bubble up via
        // ExecuteEvents.ExecuteHierarchy and would hit the backdrop Button —
        // dismissing the dialog accidentally. A no-op Button on the card
        // consumes them here.
        Button cardBtn = card.AddComponent<Button>();
        cardBtn.transition = Selectable.Transition.None;
        WireClick(cardBtn, () => { });

        var csf = card.AddComponent<ContentSizeFitter>();
        csf.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
        csf.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;

        var vlg = card.AddComponent<VerticalLayoutGroup>();
        vlg.childControlWidth = true;
        vlg.childControlHeight = true;
        vlg.childForceExpandWidth = true;
        vlg.childForceExpandHeight = false;
        vlg.spacing = UITheme.Dp(6f);
        vlg.padding = new RectOffset(
            (int)UITheme.Dp(12f), (int)UITheme.Dp(12f),
            (int)UITheme.Dp(12f), (int)UITheme.Dp(12f));

        _modalTitle = UIFactory.Text("MessagesPlus_ConfirmTitle", "Confirm", card.transform,
            UITheme.Sp(32), TextAnchor.MiddleCenter, FontStyle.Bold);
        _modalTitle.color = Ink;
        SetRaycastTarget(_modalTitle, false); // W8: modal texts must not take/bubble clicks
        AddPreferredHeight(_modalTitle.gameObject, 42f);

        _modalMessage = UIFactory.Text("MessagesPlus_ConfirmMessage", string.Empty, card.transform,
            UITheme.Sp(24), TextAnchor.MiddleCenter);
        _modalMessage.color = InkDim;
        SetRaycastTarget(_modalMessage, false); // W8
        // Review-minor 2026-09-26: 80 Dp clipped a wrapped 4-line message into the
        // button row — 120 Dp fits the longest confirm text without overlap.
        AddPreferredHeight(_modalMessage.gameObject, 120f);

        GameObject buttonRow = UIFactory.Panel("MessagesPlus_ConfirmButtons", card.transform, Color.clear);
        SetRaycastTarget(buttonRow.GetComponent<Image>(), false); // W9: invisible row must not block the card
        var hlg = buttonRow.AddComponent<HorizontalLayoutGroup>();
        hlg.childControlWidth = true;
        hlg.childControlHeight = true;
        hlg.childForceExpandWidth = true;
        hlg.childForceExpandHeight = false;
        hlg.spacing = UITheme.Dp(8f);
        AddPreferredHeight(buttonRow, 52f);

        var (cancelMask, cancelBtn, cancelLabel) = UIFactory.RoundedButtonWithLabel(
            "MessagesPlus_ConfirmCancel", "Cancel", buttonRow.transform,
            NeutralBg, UITheme.Dp(130f), UITheme.Dp(50f), (int)UITheme.Sp(26), NeutralInk);
        cancelBtn.GetComponent<Image>().raycastTarget = true;
        WireClick(cancelBtn, HideConfirm);
        SetRaycastTarget(cancelLabel, false);

        var (confirmMask, confirmBtn, confirmLabel) = UIFactory.RoundedButtonWithLabel(
            "MessagesPlus_ConfirmOk", "Confirm", buttonRow.transform,
            DestructiveBg, UITheme.Dp(130f), UITheme.Dp(50f), (int)UITheme.Sp(26), Color.white);
        confirmBtn.GetComponent<Image>().raycastTarget = true;
        WireClick(confirmBtn, OnConfirmClicked);
        SetRaycastTarget(confirmLabel, false);
    }

    private static void AddPreferredHeight(GameObject go, float dp)
    {
        var le = go.AddComponent<LayoutElement>();
        le.preferredHeight = UITheme.Dp(dp);
        le.minHeight = UITheme.Dp(dp);
    }

    /// <summary>W9: non-interactive graphics must not swallow clicks meant for vanilla UI underneath.</summary>
    private static void SetRaycastTarget(Graphic? graphic, bool value)
    {
        if (graphic != null && NetworkGuard.IsAlive(graphic))
        {
            graphic.raycastTarget = value;
        }
    }

    // ------------------------------------------------------------------
    // Listener wiring (rebuild-safe)
    // ------------------------------------------------------------------

    /// <summary>
    /// S1API's EventHelper dedupes listeners GLOBALLY per delegate instance —
    /// a rebuilt button would silently get no listener (static method groups
    /// compile to cached delegate instances). Defensive Remove-before-Add
    /// clears the stale dict entry first.
    /// </summary>
    private static void WireClick(Button btn, Action handler)
    {
        ButtonUtils.RemoveListener(btn, handler);
        ButtonUtils.AddListener(btn, handler);
    }

    /// <summary>Same rebuild-safe wiring for generic Unity events (search onValueChanged).</summary>
    private static void WireValueChanged(InputField input, Action<string> handler)
    {
        EventHelper.RemoveListener(handler, input.onValueChanged);
        EventHelper.AddListener(handler, input.onValueChanged);
    }

    // ------------------------------------------------------------------
    // View handlers (search + filter — no save mutation)
    // ------------------------------------------------------------------

    private static void OnSearchQueryChanged(string value)
    {
        try
        {
            InboxView.SetSearch(value);
            UpdateUnreadLabel();
            if (_searchClear != null && NetworkGuard.IsAlive(_searchClear))
            {
                bool hasText = !string.IsNullOrEmpty(value);
                if (_searchClear.activeSelf != hasText) _searchClear.SetActive(hasText);
            }
        }
        catch (Exception ex)
        {
            Mod.Log?.Warn($"OnSearchQueryChanged failed: {ex.Message}");
        }
    }

    private static void OnChipClicked(InboxFilter filter)
    {
        try
        {
            _currentFilter = filter;
            RestyleChips();
            InboxView.SetFilter(filter);
            UpdateUnreadLabel();
        }
        catch (Exception ex)
        {
            Mod.Log?.Warn($"OnChipClicked failed: {ex.Message}");
        }
    }

    private static void RestyleChips()
    {
        for (int i = 0; i < ChipFilters.Length; i++)
        {
            bool active = ChipFilters[i] == _currentFilter;

            Button? btn = _chipButtons[i];
            if (btn != null && NetworkGuard.IsAlive(btn))
            {
                Image? img = btn.GetComponent<Image>();
                if (img != null && NetworkGuard.IsAlive(img))
                {
                    img.color = active ? ChipActiveBg : ChipBg;
                }
            }

            Text? label = _chipLabels[i];
            if (label != null && NetworkGuard.IsAlive(label))
            {
                label.color = active ? Color.white : ChipInk;
            }
        }
    }

    private static void UpdateUnreadLabel()
    {
        if (_unreadLabel == null || !NetworkGuard.IsAlive(_unreadLabel)) return;
        int unread = InboxView.CountUnread();
        if (unread == _lastUnreadShown) return;
        _lastUnreadShown = unread;
        _unreadLabel.text = unread + " unread";
    }

    // ------------------------------------------------------------------
    // Destructive actions (Clear Read / Clear All — host-only, customer-only)
    // ------------------------------------------------------------------

    private static void OnClearAllClicked()
    {
        int count = CountVisibleCustomerConversations();
        if (count <= 0)
        {
            Mod.Log?.Info("ClearAll: nothing to do — no visible customer conversations.");
            return;
        }

        ShowConfirm(
            "Clear All",
            $"Hide all {count} customer conversation(s) from the inbox? Supplier and dealer threads are kept.",
            RunClearAll);
    }

    private static void OnClearReadClicked()
    {
        int count = CollectReadCustomerConversations().Count;
        if (count <= 0)
        {
            Mod.Log?.Info("ClearRead: nothing to do — no visible read customer conversations.");
            return;
        }

        ShowConfirm(
            "Clear Read",
            $"Hide {count} read customer conversation(s) from the inbox? Unread threads are kept. Supplier and dealer threads are kept.",
            RunClearRead);
    }

    /// <summary>
    /// Clear All semantics: iterate MessagesApp.ActiveConversations (the visible
    /// inbox subset — Conversations also contains background NPCs like "Sewer
    /// Goblin" that never appear in the inbox); for every alive conversation
    /// with EntryVisible == true that IS a customer (conservative filter — see
    /// ConversationUtils.IsCustomer) call SetEntryVisibility(false). No trash
    /// records are written. Afterwards RepositionEntries() + RefreshNotifications().
    /// Host-only (NetworkGuard) — guarded again at execution time.
    /// </summary>
    private static void RunClearAll()
    {
        if (!NetworkGuard.IsHostOrSingleplayer())
        {
            Mod.Log?.Warn("ClearAll: ignored — mutations are host-only (multiplayer client).");
            return;
        }

        try
        {
            List<MSGConversation> targets = CollectVisibleCustomerConversations();
            int hidden = HideConversations(targets, "ClearAll");
            RefreshApp(_app);
            UpdateUnreadLabel();
            Mod.Log?.Info($"ClearAll: {hidden} customer conversation(s) hidden from the inbox.");
        }
        catch (Exception ex)
        {
            Mod.Log?.Error($"ClearAll failed: {ex.Message}");
        }
    }

    /// <summary>
    /// Clear Read semantics: like Clear All but only conversations whose
    /// MSGConversation.Read flag is TRUE (access errors are never read — such
    /// threads are kept). Unread threads, suppliers and dealers are untouched.
    /// Host-only (NetworkGuard) — guarded again at execution time.
    /// </summary>
    private static void RunClearRead()
    {
        if (!NetworkGuard.IsHostOrSingleplayer())
        {
            Mod.Log?.Warn("ClearRead: ignored — mutations are host-only (multiplayer client).");
            return;
        }

        try
        {
            List<MSGConversation> targets = CollectReadCustomerConversations();
            int hidden = HideConversations(targets, "ClearRead");
            RefreshApp(_app);
            UpdateUnreadLabel();
            Mod.Log?.Info($"ClearRead: {hidden} read customer conversation(s) hidden from the inbox.");
        }
        catch (Exception ex)
        {
            Mod.Log?.Error($"ClearRead failed: {ex.Message}");
        }
    }

    /// <summary>Plan-then-commit hide: SetEntryVisibility can rebuild the conversation lists under a live walk.</summary>
    private static int HideConversations(List<MSGConversation> targets, string tag)
    {
        int hidden = 0;
        for (int i = 0; i < targets.Count; i++)
        {
            MSGConversation conv = targets[i];
            if (!ConversationUtils.IsAlive(conv)) continue;
            try
            {
                conv.SetEntryVisibility(false);
                hidden++;
            }
            catch (Exception ex)
            {
                Mod.Log?.Warn($"{tag}: hide failed for '{ConversationUtils.SafeName(conv)}': {ex.Message}");
            }
        }
        return hidden;
    }

    private static int CountVisibleCustomerConversations() =>
        CollectVisibleCustomerConversations().Count;

    private static List<MSGConversation> CollectVisibleCustomerConversations()
    {
        List<MSGConversation> targets = new();
        try
        {
            var conversations = MessagesApp.ActiveConversations;
            if (conversations == null) return targets;
            int count = conversations.Count;
            for (int i = 0; i < count; i++)
            {
                MSGConversation? conv = conversations[i];
                if (!ConversationUtils.IsAlive(conv)) continue;
                try
                {
                    if (!conv!.EntryVisible) continue;
                    if (!ConversationUtils.IsCustomer(conv)) continue; // conservative: unknown/empty categories are never hidden
                    targets.Add(conv);
                }
                catch (Exception ex)
                {
                    Mod.Log?.Warn($"ClearAll: conversation {i} skipped: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Mod.Log?.Warn($"CollectVisibleCustomerConversations failed: {ex.Message}");
        }
        return targets;
    }

    private static List<MSGConversation> CollectReadCustomerConversations()
    {
        List<MSGConversation> targets = new();
        try
        {
            var conversations = MessagesApp.ActiveConversations;
            if (conversations == null) return targets;
            int count = conversations.Count;
            for (int i = 0; i < count; i++)
            {
                MSGConversation? conv = conversations[i];
                if (!ConversationUtils.IsAlive(conv)) continue;
                try
                {
                    if (!conv!.EntryVisible) continue;
                    if (!ConversationUtils.IsCustomer(conv)) continue; // conservative: unknown/empty categories are never hidden
                    if (!ConversationUtils.TryGetRead(conv, out bool read) || !read) continue; // unknown read state = keep
                    targets.Add(conv);
                }
                catch (Exception ex)
                {
                    Mod.Log?.Warn($"ClearRead: conversation {i} skipped: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Mod.Log?.Warn($"CollectReadCustomerConversations failed: {ex.Message}");
        }
        return targets;
    }

    private static void RefreshApp(MessagesApp? app)
    {
        try
        {
            if (app == null || !NetworkGuard.IsAlive(app)) return;
            app.RepositionEntries();
            app.RefreshNotifications();
        }
        catch (Exception ex)
        {
            Mod.Log?.Warn($"RefreshApp failed: {ex.Message}");
        }
    }

    // ------------------------------------------------------------------
    // Confirmation modal
    // ------------------------------------------------------------------

    private static void ShowConfirm(string title, string message, Action onConfirm)
    {
        if (!NetworkGuard.IsAlive(_modalRoot))
        {
            // W3: log the bail instead of silently no-op'ing (dead button). The
            // modal is part of the EnsureBuilt liveness check, so the next open
            // rebuilds the UI.
            Mod.Log?.Warn("ShowConfirm: modal root missing — dialog skipped (UI rebuilds on next open).");
            return;
        }

        _pendingConfirm = onConfirm;
        if (_modalTitle != null && NetworkGuard.IsAlive(_modalTitle)) _modalTitle.text = title;
        if (_modalMessage != null && NetworkGuard.IsAlive(_modalMessage)) _modalMessage.text = message;

        _modalRoot!.SetActive(true);
        _modalRoot.transform.SetAsLastSibling();
    }

    private static void HideConfirm()
    {
        ResetModal();
    }

    private static void OnConfirmClicked()
    {
        Action? pending = _pendingConfirm;
        HideConfirm();
        try
        {
            pending?.Invoke();
        }
        catch (Exception ex)
        {
            Mod.Log?.Error($"Confirm action failed: {ex.Message}");
        }
    }
}
