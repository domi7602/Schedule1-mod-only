using System;
using System.Collections.Generic;
using Il2CppScheduleOne.UI.Phone.Messages;
using S1API.UI;
using S1API.Utils;
using S1Mods.Shared;
using UnityEngine;
using UnityEngine.UI;

namespace MessagesPlus;

/// <summary>
/// Injects the MessagesPlus UI into the VANILLA MessagesApp canvas (patch-only
/// mod — no separate PhoneApp, no homescreen icon). Two anchors in the existing
/// app page (MessagesApp.homePage):
///
///   1. Toolbar row (top-right): [trash icon] toggles the trash section,
///      [Clear All] moves every visible thread to the trash (confirmation).
///   2. Trash section (bottom): collapsible "[Trash (N)]" header, one row
///      per deleted thread with a restore button, and [Empty trash]
///      (permanent delete, second confirmation).
///
/// All clicks go through S1API ButtonUtils (IL2CPP-safe EventHelper), every
/// update handler is WasCollected-guarded, and rows are rebuilt only when the
/// trash content changes (no per-frame allocations). In multiplayer clients
/// the mutation buttons are disabled (read-only trash, host-only mutations).
/// </summary>
public static class TrashUI
{
    // --- Palette ---
    private static readonly Color SectionBg = new(0.07f, 0.08f, 0.11f, 0.92f);
    private static readonly Color HeaderBg = new(0.10f, 0.12f, 0.16f, 0.95f);
    private static readonly Color RowBg = new(1f, 1f, 1f, 0.06f);
    private static readonly Color DestructiveBg = new(0.62f, 0.18f, 0.18f, 0.95f);
    private static readonly Color RestoreBg = new(0.20f, 0.38f, 0.62f, 0.95f);
    private static readonly Color NeutralBg = new(0.16f, 0.19f, 0.25f, 0.95f);
    private static readonly Color TextLight = new(0.95f, 0.95f, 0.95f, 1f);
    private static readonly Color TextDim = new(0.68f, 0.74f, 0.84f, 1f);

    // --- References ---
    private static MessagesApp? _app;
    private static GameObject? _toolbarRoot;
    private static GameObject? _sectionRoot;
    private static GameObject? _rowsContainer;
    private static GameObject? _emptyButtonRoot;
    private static GameObject? _modalRoot;
    private static Text? _headerLabel;
    private static Text? _modalTitle;
    private static Text? _modalMessage;
    private static Text? _modalConfirmLabel;
    private static Button? _modalConfirmButton;

    private static bool _expanded = true;
    private static bool _rowsDirty = true;
    private static bool _trashEventsSubscribed;
    private static int _headerCountCache = -1;
    private static bool _headerExpandedCache;
    private static Action? _pendingConfirm;
    private static float _nextTickTime;

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

            // C2: rebuild the row list whenever the trash state changes
            // (Clear All / Restore / Empty trash / slot load). Static event —
            // subscribe exactly once, never unsubscribe (Rule 10 analogue).
            SubscribeOnce();

            bool sameApp = ReferenceEquals(_app, app);
            bool alive = NetworkGuard.IsAlive(_toolbarRoot)
                && NetworkGuard.IsAlive(_sectionRoot)
                && NetworkGuard.IsAlive(_modalRoot);
            if (sameApp && alive)
            {
                _rowsDirty = true;
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
                S1Mods.Shared.UITheme.InitializeForDashboard(homeRt);
            }

            BuildToolbar(home.transform);
            BuildTrashSection(home.transform);
            BuildConfirmModal(home.transform);
            _rowsDirty = true;
            Refresh();
            Mod.Log?.Info("UI injected into vanilla MessagesApp.");
        }
        catch (Exception ex)
        {
            Mod.Log?.Error($"EnsureBuilt failed: {ex.Message}");
        }
    }

    /// <summary>
    /// Periodic, throttled refresh (driven by Mod's single MelonEvents.OnUpdate
    /// dispatcher). Keeps the "(N)" counter and row list in sync even when the
    /// vanilla app rebuilt its conversation list without a trash mutation, and
    /// re-applies trash/purge state (purged threads stay alive and can receive
    /// new messages — vanilla callbacks may otherwise re-show the entry).
    /// </summary>
    public static void Tick()
    {
        try
        {
            if (Time.unscaledTime < _nextTickTime) return;
            _nextTickTime = Time.unscaledTime + 1.0f;

            // W5: re-apply purge/trash state (cheap — early-returns when both
            // lists are empty) so a purged thread with new messages cannot
            // resurrect its inbox entry.
            TrashService.ApplyToConversations();

            if (_app == null || !NetworkGuard.IsAlive(_app)) return;
            if (!NetworkGuard.IsAlive(_toolbarRoot) || !NetworkGuard.IsAlive(_sectionRoot) || !NetworkGuard.IsAlive(_modalRoot))
            {
                // Vanilla rebuilt its page (scene reload) or a previous build
                // failed part-way — re-inject.
                EnsureBuilt(_app);
                return;
            }

            Refresh();
        }
        catch (Exception ex)
        {
            Mod.Log?.Warn($"Tick failed: {ex.Message}");
        }
    }

    /// <summary>Clears managed UI references on scene unload (objects die with the scene — never Destroy).</summary>
    public static void TearDownForSceneUnload()
    {
        _app = null;
        _toolbarRoot = null;
        _sectionRoot = null;
        _rowsContainer = null;
        _emptyButtonRoot = null;
        _modalRoot = null;
        _headerLabel = null;
        _modalTitle = null;
        _modalMessage = null;
        _modalConfirmLabel = null;
        _modalConfirmButton = null;
        _pendingConfirm = null;
        _rowsDirty = true;
        _headerCountCache = -1;
    }

    /// <summary>
    /// Hides the confirmation dialog and drops the pending action — called when
    /// the app closes so a half-finished dialog cannot reappear (with a stale
    /// pending action) on the next open.
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
    /// W4: destroys our managed UI roots while they are still alive. Only called
    /// on the rebuild path (a mid-build exception can leave earlier roots in the
    /// scene; dropping the refs without Destroy would orphan them and the next
    /// build would inject a second toolbar). These are our own injected objects —
    /// distinct from the scene-unload case, where objects die with the scene and
    /// must never be Destroyed.
    /// </summary>
    private static void DestroyManagedRoots()
    {
        DestroyIfAlive(_toolbarRoot);
        DestroyIfAlive(_sectionRoot); // rows container + empty button are its children
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

    private static void BuildToolbar(Transform parent)
    {
        float dp8 = S1Mods.Shared.UITheme.Dp(8f);

        GameObject toolbar = UIFactory.Panel("MessagesPlus_Toolbar", parent, Color.clear,
            anchorMin: new Vector2(1f, 1f), anchorMax: new Vector2(1f, 1f));
        _toolbarRoot = toolbar; // keep the ref immediately: a mid-build exception must not orphan the root

        // W9: the toolbar panel is invisible — let clicks pass through to the
        // vanilla page underneath instead of swallowing them.
        SetRaycastTarget(toolbar, false);

        RectTransform rt = (RectTransform)toolbar.transform;
        rt.pivot = new Vector2(1f, 1f);
        rt.sizeDelta = new Vector2(S1Mods.Shared.UITheme.Dp(180f), S1Mods.Shared.UITheme.Dp(30f));
        rt.anchoredPosition = new Vector2(-dp8, -dp8);

        var hlg = toolbar.AddComponent<HorizontalLayoutGroup>();
        hlg.childControlWidth = false;
        hlg.childControlHeight = true;
        hlg.childForceExpandWidth = false;
        hlg.childForceExpandHeight = false;
        hlg.spacing = S1Mods.Shared.UITheme.Dp(6f);
        hlg.childAlignment = TextAnchor.MiddleRight;

        // W10: mutation buttons are disabled for multiplayer clients
        // (read-only trash — mutations are host-only).
        bool canMutate = NetworkGuard.IsHostOrSingleplayer();

        var (_, trashBtn, trashLabel) = UIFactory.ButtonWithLabel(
            "MessagesPlus_TrashToggle", "\U0001F5D1", toolbar.transform,
            HeaderBg, S1Mods.Shared.UITheme.Dp(44f), S1Mods.Shared.UITheme.Dp(28f));
        ButtonUtils.AddListener(trashBtn, OnTrashToggleClicked);
        SetRaycastTarget(trashLabel, false);

        var (_, clearBtn, clearLabel) = UIFactory.ButtonWithLabel(
            "MessagesPlus_ClearAll", "Clear All", toolbar.transform,
            HeaderBg, S1Mods.Shared.UITheme.Dp(110f), S1Mods.Shared.UITheme.Dp(28f));
        clearBtn.interactable = canMutate;
        ButtonUtils.AddListener(clearBtn, OnClearAllClicked);
        SetRaycastTarget(clearLabel, false);
    }

    private static void BuildTrashSection(Transform parent)
    {
        GameObject section = UIFactory.Panel("MessagesPlus_TrashSection", parent, SectionBg,
            anchorMin: new Vector2(0f, 0f), anchorMax: new Vector2(1f, 0f));
        _sectionRoot = section; // keep the ref immediately (see BuildToolbar)

        RectTransform rt = (RectTransform)section.transform;
        rt.pivot = new Vector2(0.5f, 0f);
        rt.anchoredPosition = Vector2.zero;
        rt.sizeDelta = new Vector2(0f, S1Mods.Shared.UITheme.Dp(120f));

        var vlg = section.AddComponent<VerticalLayoutGroup>();
        vlg.childControlWidth = true;
        vlg.childControlHeight = true;
        vlg.childForceExpandWidth = true;
        vlg.childForceExpandHeight = false;
        vlg.spacing = S1Mods.Shared.UITheme.Dp(4f);
        vlg.padding = new RectOffset(
            (int)S1Mods.Shared.UITheme.Dp(8f), (int)S1Mods.Shared.UITheme.Dp(8f),
            (int)S1Mods.Shared.UITheme.Dp(6f), (int)S1Mods.Shared.UITheme.Dp(6f));

        var csf = section.AddComponent<ContentSizeFitter>();
        csf.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
        csf.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;

        // 1. Collapsible header "[Trash (N)]"
        var (headerRoot, headerBtn, headerText) = UIFactory.ButtonWithLabel(
            "MessagesPlus_TrashHeader", "Trash (0)", section.transform,
            HeaderBg, S1Mods.Shared.UITheme.Dp(360f), S1Mods.Shared.UITheme.Dp(30f));
        AddPreferredHeight(headerRoot, 30f);
        ButtonUtils.AddListener(headerBtn, OnTrashToggleClicked);
        SetRaycastTarget(headerText, false);
        _headerLabel = headerText;

        // 2. Rows container (restored per trashed thread)
        GameObject rows = UIFactory.Panel("MessagesPlus_TrashRows", section.transform, Color.clear);
        SetRaycastTarget(rows, false); // W9: invisible container must not block vanilla UI
        var rowsVlg = rows.AddComponent<VerticalLayoutGroup>();
        rowsVlg.childControlWidth = true;
        rowsVlg.childControlHeight = true;
        rowsVlg.childForceExpandWidth = true;
        rowsVlg.childForceExpandHeight = false;
        rowsVlg.spacing = S1Mods.Shared.UITheme.Dp(3f);
        var rowsCsf = rows.AddComponent<ContentSizeFitter>();
        rowsCsf.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
        rowsCsf.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
        _rowsContainer = rows;

        // 3. Permanent delete "[Empty trash]"
        var (emptyRoot, emptyBtn, emptyLabel) = UIFactory.ButtonWithLabel(
            "MessagesPlus_EmptyTrash", "Empty trash", section.transform,
            DestructiveBg, S1Mods.Shared.UITheme.Dp(360f), S1Mods.Shared.UITheme.Dp(28f));
        AddPreferredHeight(emptyRoot, 28f);
        emptyBtn.interactable = NetworkGuard.IsHostOrSingleplayer(); // W10: read-only trash for MP clients
        ButtonUtils.AddListener(emptyBtn, OnEmptyTrashClicked);
        SetRaycastTarget(emptyLabel, false);
        _emptyButtonRoot = emptyRoot;
    }

    private static void BuildConfirmModal(Transform parent)
    {
        // Full-stretch backdrop inside the vanilla home page (renders above the
        // list; hidden by default). Backdrop click cancels.
        GameObject modal = UIFactory.Panel("MessagesPlus_ConfirmModal", parent, new Color(0f, 0f, 0f, 0.72f), fullAnchor: true);
        _modalRoot = modal; // keep the ref immediately (see BuildToolbar)
        modal.SetActive(false);
        Button backdropBtn = modal.AddComponent<Button>();
        backdropBtn.transition = Selectable.Transition.None;
        ButtonUtils.AddListener(backdropBtn, HideConfirm);

        GameObject card = UIFactory.Panel("MessagesPlus_ConfirmCard", modal.transform, new Color(0.12f, 0.15f, 0.20f, 1f));
        RectTransform cardRt = (RectTransform)card.transform;
        cardRt.anchorMin = new Vector2(0.5f, 0.5f);
        cardRt.anchorMax = new Vector2(0.5f, 0.5f);
        cardRt.sizeDelta = new Vector2(S1Mods.Shared.UITheme.Dp(320f), S1Mods.Shared.UITheme.Dp(180f));

        // W8: clicks on the card (background / title / message) bubble up via
        // ExecuteEvents.ExecuteHierarchy and would hit the backdrop Button —
        // dismissing the dialog accidentally. A no-op Button on the card
        // consumes them here.
        Button cardBtn = card.AddComponent<Button>();
        cardBtn.transition = Selectable.Transition.None;
        ButtonUtils.AddListener(cardBtn, () => { });

        var vlg = card.AddComponent<VerticalLayoutGroup>();
        vlg.childControlWidth = true;
        vlg.childControlHeight = true;
        vlg.childForceExpandWidth = true;
        vlg.childForceExpandHeight = false;
        vlg.spacing = S1Mods.Shared.UITheme.Dp(8f);
        vlg.padding = new RectOffset(
            (int)S1Mods.Shared.UITheme.Dp(14f), (int)S1Mods.Shared.UITheme.Dp(14f),
            (int)S1Mods.Shared.UITheme.Dp(14f), (int)S1Mods.Shared.UITheme.Dp(14f));

        _modalTitle = UIFactory.Text("MessagesPlus_ConfirmTitle", "Confirm", card.transform,
            S1Mods.Shared.UITheme.Sp(14), TextAnchor.MiddleCenter, FontStyle.Bold);
        _modalTitle.color = TextLight;
        SetRaycastTarget(_modalTitle, false); // W8: modal texts must not take/bubble clicks

        _modalMessage = UIFactory.Text("MessagesPlus_ConfirmMessage", string.Empty, card.transform,
            S1Mods.Shared.UITheme.Sp(11), TextAnchor.UpperLeft);
        _modalMessage.color = TextDim;
        SetRaycastTarget(_modalMessage, false); // W8
        AddPreferredHeight(_modalMessage.gameObject, 56f);

        GameObject buttonRow = UIFactory.Panel("MessagesPlus_ConfirmButtons", card.transform, Color.clear);
        SetRaycastTarget(buttonRow, false); // W9: invisible row must not block the card
        var hlg = buttonRow.AddComponent<HorizontalLayoutGroup>();
        hlg.childControlWidth = true;
        hlg.childControlHeight = true;
        hlg.childForceExpandWidth = true;
        hlg.childForceExpandHeight = false;
        hlg.spacing = S1Mods.Shared.UITheme.Dp(10f);
        AddPreferredHeight(buttonRow, 30f);

        var (_, cancelBtn, cancelLabel) = UIFactory.ButtonWithLabel(
            "MessagesPlus_ConfirmCancel", "Cancel", buttonRow.transform,
            NeutralBg, S1Mods.Shared.UITheme.Dp(120f), S1Mods.Shared.UITheme.Dp(28f));
        ButtonUtils.AddListener(cancelBtn, HideConfirm);
        SetRaycastTarget(cancelLabel, false);

        var (_, confirmBtn, confirmLabel) = UIFactory.ButtonWithLabel(
            "MessagesPlus_ConfirmOk", "Confirm", buttonRow.transform,
            DestructiveBg, S1Mods.Shared.UITheme.Dp(120f), S1Mods.Shared.UITheme.Dp(28f));
        ButtonUtils.AddListener(confirmBtn, OnConfirmClicked);
        SetRaycastTarget(confirmLabel, false);
        _modalConfirmButton = confirmBtn;
        _modalConfirmLabel = confirmLabel;
    }

    // ------------------------------------------------------------------
    // Refresh / rows
    // ------------------------------------------------------------------

    private static void Refresh()
    {
        if (_app == null || !NetworkGuard.IsAlive(_app)) return;
        int count = TrashService.TrashedCount;

        // S4: only interpolate the header when the visible content changed.
        if (count != _headerCountCache || _expanded != _headerExpandedCache)
        {
            _headerCountCache = count;
            _headerExpandedCache = _expanded;
            if (_headerLabel != null && NetworkGuard.IsAlive(_headerLabel))
            {
                _headerLabel.text = $"Trash ({count}) {(_expanded ? "\u25B2" : "\u25BC")}";
            }
        }

        if (_rowsDirty)
        {
            RebuildRows();
            _rowsDirty = false;
        }

        bool showRows = _expanded && count > 0;
        if (NetworkGuard.IsAlive(_rowsContainer) && _rowsContainer!.activeSelf != showRows)
        {
            _rowsContainer.SetActive(showRows);
        }

        bool showEmpty = _expanded && count > 0;
        if (NetworkGuard.IsAlive(_emptyButtonRoot) && _emptyButtonRoot!.activeSelf != showEmpty)
        {
            _emptyButtonRoot.SetActive(showEmpty);
        }
    }

    private static void RebuildRows()
    {
        GameObject? rows = _rowsContainer;
        if (!NetworkGuard.IsAlive(rows)) return;

        // Mutations are rare (user-triggered) — full rebuild is simple and safe.
        UIFactory.ClearChildren(rows!.transform);

        bool canMutate = NetworkGuard.IsHostOrSingleplayer();
        foreach (TrashEntry entry in TrashService.Trashed)
        {
            if (entry == null) continue;
            BuildRow(entry, canMutate);
        }
    }

    private static void BuildRow(TrashEntry entry, bool canMutate)
    {
        GameObject row = UIFactory.Panel("MessagesPlus_TrashRow", _rowsContainer!.transform, RowBg);
        SetRaycastTarget(row, false); // W9: non-interactive row background
        AddPreferredHeight(row, 34f);

        var hlg = row.AddComponent<HorizontalLayoutGroup>();
        hlg.childControlWidth = true;
        hlg.childControlHeight = true;
        hlg.childForceExpandWidth = true;
        hlg.childForceExpandHeight = false;
        hlg.spacing = S1Mods.Shared.UITheme.Dp(6f);
        hlg.padding = new RectOffset(
            (int)S1Mods.Shared.UITheme.Dp(8f), (int)S1Mods.Shared.UITheme.Dp(6f), 0, 0);
        hlg.childAlignment = TextAnchor.MiddleLeft;

        Text label = UIFactory.Text("MessagesPlus_TrashRowLabel", entry.ContactName, row.transform,
            S1Mods.Shared.UITheme.Sp(11), TextAnchor.MiddleLeft);
        label.color = TextLight;
        SetRaycastTarget(label, false); // W9
        var labelLe = label.gameObject.AddComponent<LayoutElement>();
        labelLe.flexibleWidth = 1f;
        labelLe.minWidth = S1Mods.Shared.UITheme.Dp(40f);

        var (_, restoreBtn, restoreLabel) = UIFactory.ButtonWithLabel(
            "MessagesPlus_TrashRowRestore", "\u21A9", row.transform,
            RestoreBg, S1Mods.Shared.UITheme.Dp(40f), S1Mods.Shared.UITheme.Dp(24f));
        restoreBtn.interactable = canMutate; // W10: read-only trash for MP clients
        SetRaycastTarget(restoreLabel, false);
        var btnLe = restoreBtn.gameObject.AddComponent<LayoutElement>();
        btnLe.flexibleWidth = 0f;
        btnLe.preferredWidth = S1Mods.Shared.UITheme.Dp(40f);
        btnLe.preferredHeight = S1Mods.Shared.UITheme.Dp(24f);

        TrashEntry captured = entry;
        ButtonUtils.AddListener(restoreBtn, () => OnRestoreClicked(captured));
    }

    private static void AddPreferredHeight(GameObject go, float dp)
    {
        var le = go.AddComponent<LayoutElement>();
        le.preferredHeight = S1Mods.Shared.UITheme.Dp(dp);
        le.minHeight = S1Mods.Shared.UITheme.Dp(dp);
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
    // Trash-change subscription (C2)
    // ------------------------------------------------------------------

    private static void SubscribeOnce()
    {
        if (_trashEventsSubscribed) return;
        _trashEventsSubscribed = true;
        TrashService.OnTrashChanged += OnTrashChangedHandler;
    }

    private static void OnTrashChangedHandler()
    {
        try
        {
            _rowsDirty = true;
            Refresh();
        }
        catch (Exception ex)
        {
            Mod.Log?.Warn($"OnTrashChanged handler failed: {ex.Message}");
        }
    }

    // ------------------------------------------------------------------
    // Click handlers
    // ------------------------------------------------------------------

    private static void OnTrashToggleClicked()
    {
        _expanded = !_expanded;
        _rowsDirty = true;
        Refresh();
    }

    private static void OnClearAllClicked()
    {
        int count = CountVisibleConversations();
        if (count <= 0)
        {
            Mod.Log?.Info("ClearAll: nothing to do — no visible conversations.");
            return;
        }

        ShowConfirm(
            "Clear All",
            $"Move all {count} conversation(s) to the trash?\nThey can be restored from the Trash.",
            "Clear All",
            () => TrashService.ClearAll(_app));
    }

    private static void OnEmptyTrashClicked()
    {
        int count = TrashService.TrashedCount;
        if (count <= 0) return;

        // Second confirmation — permanent delete needs the stronger warning.
        ShowConfirm(
            "Empty trash?",
            $"Permanently remove {count} conversation(s) from the Messages app?\n\nThis cannot be undone.",
            "Empty trash",
            () => TrashService.EmptyTrash(_app));
    }

    private static void OnRestoreClicked(TrashEntry entry)
    {
        TrashService.Restore(entry, _app);
    }

    private static int CountVisibleConversations()
    {
        int count = 0;
        try
        {
            var conversations = Il2CppScheduleOne.UI.Phone.Messages.MessagesApp.Conversations;
            if (conversations == null) return 0;
            for (int i = 0; i < conversations.Count; i++)
            {
                var conv = conversations[i];
                if (!TrashService.IsAlive(conv)) continue; // S5: shared IL2CPP liveness helper
                if (conv.EntryVisible && !TrashService.IsTrashed(conv) && !TrashService.IsPurged(conv))
                {
                    count++;
                }
            }
        }
        catch (Exception ex)
        {
            Mod.Log?.Warn($"CountVisibleConversations failed: {ex.Message}");
        }
        return count;
    }

    // ------------------------------------------------------------------
    // Confirmation modal
    // ------------------------------------------------------------------

    private static void ShowConfirm(string title, string message, string confirmLabel, Action onConfirm)
    {
        if (!NetworkGuard.IsAlive(_modalRoot))
        {
            // W3: log the bail instead of silently no-op'ing (dead Clear All
            // buttons). The modal is part of the EnsureBuilt liveness check, so
            // the next open/tick rebuilds the UI.
            Mod.Log?.Warn("ShowConfirm: modal root missing — dialog skipped (UI rebuilds on next open/tick).");
            return;
        }

        _pendingConfirm = onConfirm;
        if (_modalTitle != null && NetworkGuard.IsAlive(_modalTitle)) _modalTitle.text = title;
        if (_modalMessage != null && NetworkGuard.IsAlive(_modalMessage)) _modalMessage.text = message;
        if (_modalConfirmLabel != null && NetworkGuard.IsAlive(_modalConfirmLabel)) _modalConfirmLabel.text = confirmLabel;

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
