using System;
using System.Collections.Generic;
using System.Text;
using MelonLoader;
using LightControl.Core;
using S1API.PhoneApp;
using S1API.UI;
using S1Mods.Shared;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace LightControl;

/// <summary>
/// Landscape phone list, same orientation as PotScanner. Lamps sit under a property header.
/// Color, brightness and apply stay in one bottom bar. Rows are pooled and never destroyed on close.
/// </summary>
public sealed class LightControlApp : PhoneApp
{
    protected override string AppName => "LightControl";

    protected override string AppTitle => "Light Control";

    protected override string IconLabel => "Lights";

    protected override string IconFileName => string.Empty;

    protected override EOrientation Orientation => EOrientation.Horizontal;

    protected override Sprite IconSprite => GetIcon();

    private readonly List<(UnityEvent Event, Action Handler)> _wired = new();
    private readonly List<SwatchButton> _swatches = new();
    private readonly List<LampRow> _rows = new();
    private readonly List<PropertyHeader> _headers = new();
    private readonly List<int> _visible = new();
    private readonly List<int> _ordered = new();
    private readonly List<string?> _rowCodes = new();
    private readonly List<string?> _rowNames = new();
    private readonly List<UiSelectionPolicy.PropertyGroup> _groups = new();
    private readonly System.Text.StringBuilder _signatureBuilder = new();

    private GameObject? _root;
    private GameObject? _actions;
    private GameObject? _palette;
    private Text _status = null!, _detailName = null!, _detailProperty = null!, _action = null!, _propertyApply = null!, _worldApply = null!, _empty = null!;
    private Transform? _listContent;
    private ScrollRect? _scroll;
    private string? _selectionKey;
    private Guid _seenSession;
    private long _seenCatalog = -1;
    private long _seenRevision = -1;
    private string _seenNotice = string.Empty;
    private double _actionPosted = double.NegativeInfinity;
    private LightRuntime.CopyPreview _propertyPreview;
    private LightRuntime.CopyPreview _worldPreview;
    private bool _hasPropertyPreview;
    private bool _hasWorldPreview;
    private readonly List<Image> _intensityMarks = new();
    private string _signature = string.Empty;
    private bool _destroyed;
    private bool _updateErrorLogged;
    private static Sprite? _icon;
    private static Texture2D? _iconTexture;

    protected override void OnCreated()
    {
        base.OnCreated();
        MelonEvents.OnUpdate.Unsubscribe(Update);
        MelonEvents.OnUpdate.Subscribe(Update);
    }

    protected override void OnCreatedUI(GameObject container)
    {
        UITheme.InitializeForDashboard(container.GetComponent<RectTransform>());

        // A single isolated, inactive root. Visibility is driven exclusively by IsOpen().
        _root = UIFactory.Panel("LightControl_Root", container.transform, GamePalette.Bg, fullAnchor: true);
        _root.SetActive(false);
        var stack = _root.AddComponent<VerticalLayoutGroup>();
        stack.padding = new RectOffset((int)UITheme.Dp(12f), (int)UITheme.Dp(12f), (int)UITheme.Dp(10f), (int)UITheme.Dp(10f));
        stack.spacing = UITheme.Dp(6f);
        stack.childControlWidth = true;
        stack.childControlHeight = true;
        stack.childForceExpandWidth = true;
        stack.childForceExpandHeight = false;

        var header = UIFactory.Panel("Header", _root.transform, Color.clear);
        header.GetComponent<Image>().raycastTarget = false;
        Prefer(header, UITheme.Dp(40f));
        var title = UIFactory.Text("Title", "LIGHT CONTROL", header.transform, UITheme.Sp(18), TextAnchor.MiddleLeft, FontStyle.Bold);
        title.color = GamePalette.TextPrimary;
        title.raycastTarget = false;
        Place(title.gameObject, 0f, 0.42f, 0.62f, 1f);
        _status = UIFactory.Text("Status", string.Empty, header.transform, UITheme.Sp(11), TextAnchor.MiddleLeft);
        _status.color = GamePalette.TextMuted;
        _status.horizontalOverflow = HorizontalWrapMode.Wrap;
        _status.verticalOverflow = VerticalWrapMode.Truncate;
        _status.raycastTarget = false;
        Place(_status.gameObject, 0f, 0f, 1f, 0.42f);

        var body = UIFactory.Panel("Body", _root.transform, Color.clear);
        body.GetComponent<Image>().raycastTarget = false;
        var bodyHeight = body.AddComponent<LayoutElement>();
        bodyHeight.flexibleHeight = 1f;
        bodyHeight.flexibleWidth = 1f;
        var split = body.AddComponent<HorizontalLayoutGroup>();
        split.spacing = UITheme.Dp(8f);
        split.childControlWidth = true;
        split.childControlHeight = true;
        split.childForceExpandWidth = false;
        split.childForceExpandHeight = true;

        var listPanel = UIFactory.Panel("List", body.transform, Color.clear);
        listPanel.GetComponent<Image>().raycastTarget = false;
        var listWidth = listPanel.AddComponent<LayoutElement>();
        listWidth.flexibleWidth = 43f;
        listWidth.flexibleHeight = 1f;
        if (listPanel.GetComponent<RectMask2D>() == null) listPanel.AddComponent<RectMask2D>();
        var content = UIFactory.ScrollableVerticalList("LampScroll", listPanel.transform, out var scroll);
        _listContent = content;
        _scroll = scroll;
        var contentLayout = content.GetComponent<VerticalLayoutGroup>();
        contentLayout.spacing = UITheme.Dp(4f);
        contentLayout.padding = new RectOffset(0, 0, 0, (int)UITheme.Dp(4f));
        contentLayout.childControlWidth = true;
        contentLayout.childForceExpandWidth = true;
        contentLayout.childControlHeight = true;
        contentLayout.childForceExpandHeight = false;
        if (scroll != null)
        {
            scroll.vertical = true;
            scroll.movementType = ScrollRect.MovementType.Clamped;
        }
        _empty = UIFactory.Text("Empty", "No supported grow lamps.", content, UITheme.Sp(13), TextAnchor.MiddleLeft);
        _empty.color = GamePalette.TextMuted;
        _empty.raycastTarget = false;
        Prefer(_empty.gameObject, UITheme.Dp(28f));

        var detail = UIFactory.Panel("Detail", body.transform, GamePalette.Card);
        detail.GetComponent<Image>().raycastTarget = false;
        var detailWidth = detail.AddComponent<LayoutElement>();
        detailWidth.flexibleWidth = 57f;
        detailWidth.flexibleHeight = 1f;
        var detailStack = detail.AddComponent<VerticalLayoutGroup>();
        detailStack.padding = new RectOffset((int)UITheme.Dp(8f), (int)UITheme.Dp(8f), (int)UITheme.Dp(6f), (int)UITheme.Dp(6f));
        detailStack.spacing = UITheme.Dp(4f);
        detailStack.childControlWidth = true;
        detailStack.childControlHeight = true;
        detailStack.childForceExpandWidth = true;
        detailStack.childForceExpandHeight = false;
        _detailName = UIFactory.Text("DetailName", "No lamp selected", detail.transform, UITheme.Sp(14), TextAnchor.MiddleLeft, FontStyle.Bold);
        _detailName.color = GamePalette.TextPrimary;
        _detailName.raycastTarget = false;
        Prefer(_detailName.gameObject, UITheme.Dp(22f));
        _detailProperty = UIFactory.Text("DetailProperty", string.Empty, detail.transform, UITheme.Sp(11), TextAnchor.MiddleLeft);
        _detailProperty.color = GamePalette.TextMuted;
        _detailProperty.raycastTarget = false;
        Prefer(_detailProperty.gameObject, UITheme.Dp(16f));

        _palette = UIFactory.Panel("Palette", detail.transform, Color.clear);
        _palette.GetComponent<Image>().raycastTarget = false;
        Prefer(_palette, UITheme.Dp(32f));
        var paletteRow = _palette.AddComponent<HorizontalLayoutGroup>();
        paletteRow.spacing = UITheme.Dp(3f);
        paletteRow.childControlWidth = true;
        paletteRow.childControlHeight = true;
        paletteRow.childForceExpandWidth = true;
        paletteRow.childForceExpandHeight = false;
        for (int i = (int)ColorMode.White; i < UiSelectionPolicy.ColorCount - 1; i++)
            CreateSwatch(_palette.transform, (ColorMode)i);

        var modeRow = UIFactory.Panel("Modes", detail.transform, Color.clear);
        modeRow.GetComponent<Image>().raycastTarget = false;
        Prefer(modeRow, UITheme.Dp(28f));
        var modes = modeRow.AddComponent<HorizontalLayoutGroup>();
        modes.spacing = UITheme.Dp(4f);
        modes.childControlWidth = true;
        modes.childControlHeight = true;
        modes.childForceExpandWidth = true;
        modes.childForceExpandHeight = false;
        MakeAction(modeRow.transform, "OriginalColor", "Original color", GamePalette.CardAlt, GamePalette.TextPrimary, () => Act(key => LightRuntime.SetColor(key, ColorMode.Original)));
        MakeAction(modeRow.transform, "Rainbow", "Rainbow", GamePalette.CardAlt, GamePalette.TextPrimary, () => Act(key => LightRuntime.SetColor(key, ColorMode.Rainbow)));

        var brightRow = UIFactory.Panel("Brightness", detail.transform, Color.clear);
        brightRow.GetComponent<Image>().raycastTarget = false;
        Prefer(brightRow, UITheme.Dp(28f));
        var brights = brightRow.AddComponent<HorizontalLayoutGroup>();
        brights.spacing = UITheme.Dp(3f);
        brights.childControlWidth = true;
        brights.childControlHeight = true;
        brights.childForceExpandWidth = true;
        brights.childForceExpandHeight = false;
        for (int i = 0; i < UiSelectionPolicy.IntensityCount; i++)
        {
            var level = UiSelectionPolicy.IntensityAt(i);
            var label = MakeAction(brightRow.transform, "Bright" + (int)level, ((int)level).ToString(), GamePalette.Card, GamePalette.TextPrimary, () => Act(key => LightRuntime.SetIntensity(key, level)));
            var mark = label.transform.parent.GetComponent<Image>();
            if (mark != null) _intensityMarks.Add(mark);
        }

        var note = UIFactory.Text("BrightNote", "100% is this lamp's original brightness.", detail.transform, UITheme.Sp(10), TextAnchor.MiddleLeft);
        note.color = GamePalette.TextMuted;
        note.raycastTarget = false;
        Prefer(note.gameObject, UITheme.Dp(14f));

        _actions = UIFactory.Panel("Actions", detail.transform, Color.clear);
        _actions.GetComponent<Image>().raycastTarget = false;
        Prefer(_actions, UITheme.Dp(64f));
        var actionStack = _actions.AddComponent<VerticalLayoutGroup>();
        actionStack.spacing = UITheme.Dp(4f);
        actionStack.childControlWidth = true;
        actionStack.childControlHeight = true;
        actionStack.childForceExpandWidth = true;
        actionStack.childForceExpandHeight = false;
        MakeAction(_actions.transform, "Reset", "Reset lamp", GamePalette.CardAlt, GamePalette.TextPrimary, () => Act(key => LightRuntime.ResetLamp(key)));
        _propertyApply = MakeAction(_actions.transform, "ApplyProperty", "Apply to property", GamePalette.Green, Color.white, ApplyProperty);
        _worldApply = MakeAction(_actions.transform, "ApplyWorld", "All properties", GamePalette.Card, GamePalette.TextPrimary, ApplyWorld);

        _action = UIFactory.Text("ActionMessage", string.Empty, _root.transform, UITheme.Sp(11), TextAnchor.MiddleLeft);
        _action.color = GamePalette.TextPrimary;
        _action.raycastTarget = false;
        Prefer(_action.gameObject, UITheme.Dp(18f));
    }

    protected override void OnDestroyed()
    {
        MelonEvents.OnUpdate.Unsubscribe(Update);
        // Release the exact wired handler/event pairs without a button liveness gate: a destroyed
        // button is Unity fake-null, so a `button != null` guard would skip the removal entirely and
        // leak the app graph. The UnityEvent captured at wiring time is used instead.
        foreach (var (unityEvent, handler) in _wired)
        {
            try { S1API.Utils.EventHelper.RemoveListener(handler, unityEvent); }
            catch { /* the panel is going away anyway */ }
        }
        _wired.Clear();
        _headers.Clear();
        _rows.Clear();
        _swatches.Clear();
        _root = null;
        _destroyed = true;
        base.OnDestroyed();
    }

    /// <summary>Polls only while the app is open; drives root visibility from IsOpen().</summary>
    private void Update()
    {
        if (_destroyed || _status == null) return;
        try
        {
            bool open = IsOpen();
            if (_root != null && _root.activeSelf != open) _root.SetActive(open);
            if (open) Refresh();
        }
        catch (Exception ex)
        {
            if (_updateErrorLogged) return;
            _updateErrorLogged = true;
            Mod.Log.Warn("Phone UI update failed", ex);
        }
    }

    private void Refresh()
    {
        bool ready = LightRuntime.Ready;
        Guid session = LightRuntime.SessionId;
        if (session != _seenSession)
        {
            _seenSession = session;
            _selectionKey = null;
            _actionPosted = double.NegativeInfinity;
            _seenCatalog = -1;
            _seenRevision = -1;
        }
        long catalog = LightRuntime.CatalogRevision;
        long revision = LightRuntime.Revision;
        var notice = UiSelectionPolicy.ClassifySave(ready, LightRuntime.StorageBlocked, LightRuntime.LastSaveFailed, revision, LightRuntime.SavedRevision, ready && !LightRuntime.SaveConfirmed);
        string noticeText = UiSelectionPolicy.SaveNoticeText(notice);
        if (LightRuntime.UnsupportedLampCount > 0) noticeText += $" {LightRuntime.UnsupportedLampCount} left untouched.";
        bool messageOn = _action != null && UiSelectionPolicy.ActionMessageVisible(_actionPosted, LightRuntime.ClockSeconds);
        if (_action != null) _action.gameObject.SetActive(messageOn);
        if (!ready || LightRuntime.LampCount == 0)
        {
            _status.text = ready
                ? "No supported grow lamps found. LED panel and emitted light use the same color."
                : noticeText;
            _selectionKey = null;
            ShowRows(0);
            ShowHeaders(0);
            if (_empty != null) _empty.gameObject.SetActive(true);
            PaintDetail(false, default);
            return;
        }
        if (catalog == _seenCatalog && revision == _seenRevision && noticeText == _seenNotice) return;
        bool structure = catalog != _seenCatalog;
        _seenCatalog = catalog;
        _seenRevision = revision;
        _seenNotice = noticeText;
        _status.text = $"{LightRuntime.LampCount} lights. {noticeText}";

        _visible.Clear();
        _rowCodes.Clear();
        _rowNames.Clear();
        var keys = new List<string>();
        for (int i = 0; i < LightRuntime.LampCount; i++)
        {
            if (!LightRuntime.TryGetLamp(i, out var lamp)) continue;
            _visible.Add(lamp.Index);
            _rowCodes.Add(lamp.PropertyCode);
            _rowNames.Add(lamp.PropertyName);
            keys.Add(lamp.Key);
        }
        _selectionKey = UiSelectionPolicy.ResolveSelection(_selectionKey, keys, false);
        if (structure)
        {
            UiSelectionPolicy.GroupByProperty(_visible, _rowCodes, _rowNames, _ordered, _groups);
            ShowRows(_ordered.Count);
            ShowHeaders(_groups.Count);
            if (_empty != null) _empty.gameObject.SetActive(_ordered.Count == 0);
            for (int i = 0; i < _groups.Count; i++)
            {
                _headers[i].Name.text = _groups[i].Name;
                _headers[i].Count.text = _groups[i].Count.ToString();
            }
            ArrangeGroups();
        }
        bool hasSelection = false;
        LightRuntime.LampView selected = default;
        for (int row = 0; row < _ordered.Count; row++)
        {
            if (!LightRuntime.TryGetLamp(_ordered[row], out var lamp)) continue;
            var ui = _rows[row];
            ui.LampIndex = lamp.Index;
            ui.Key = lamp.Key;
            ui.Name.text = lamp.Label;
            ui.Bright.text = $"{DescribeColor(lamp.Color)}  {DescribeIntensity(lamp.Intensity)}";
            if (lamp.Color == ColorMode.Rainbow)
            {
                ui.Swatch.sprite = RainbowDot();
                ui.Swatch.color = Color.white;
            }
            else
            {
                ui.Swatch.sprite = UISprites.Circle();
                ui.Swatch.color = Swatch(lamp.Color);
            }
            bool on = lamp.Key == _selectionKey;
            ui.Fill.color = on ? GamePalette.CardAlt : GamePalette.Card;
            if (on) { hasSelection = true; selected = lamp; }
        }
        PaintDetail(hasSelection, selected);
    }

    private void PaintDetail(bool hasSelection, LightRuntime.LampView selected)
    {
        bool show = hasSelection;
        if (_actions != null) _actions.SetActive(show);
        if (_palette != null) _palette.SetActive(show);
        _hasPropertyPreview = show && LightRuntime.TryBuildPreview(selected.Key, false, out _propertyPreview);
        _hasWorldPreview = show && LightRuntime.TryBuildPreview(selected.Key, true, out _worldPreview);
        if (!show)
        {
            _detailName.text = string.IsNullOrEmpty(_selectionKey) ? "No lamp selected" : "That lamp is not available right now.";
            _detailProperty.text = string.Empty;
            return;
        }
        _detailName.text = selected.Label;
        _detailProperty.text = string.IsNullOrWhiteSpace(selected.PropertyName) ? selected.PropertyCode : selected.PropertyName;
        PaintPalette(selected.Color);
        for (int i = 0; i < _intensityMarks.Count; i++)
        {
            var level = UiSelectionPolicy.IntensityAt(i);
            _intensityMarks[i].color = level == selected.Intensity ? GamePalette.Blue : GamePalette.Card;
        }
        if (_propertyApply != null)
            _propertyApply.text = _hasPropertyPreview
                ? $"Apply to {_detailProperty.text} ({_propertyPreview.TargetKeys.Length})"
                : "Apply to property";
        if (_worldApply != null)
            _worldApply.text = _hasWorldPreview ? $"All properties ({_worldPreview.TargetKeys.Length})" : "All properties";
    }

    private void ApplyProperty()
    {
        if (!_hasPropertyPreview) return;
        var shown = _propertyPreview;
        Act(_ => LightRuntime.ApplyPreview(shown, false));
    }

    private void ApplyWorld()
    {
        if (!_hasWorldPreview) return;
        var shown = _worldPreview;
        Act(_ => LightRuntime.ApplyPreview(shown, true));
    }

    private void OnRow(int slot)
    {
        if (slot < 0 || slot >= _rows.Count) return;
        _selectionKey = _rows[slot].Key;
        _seenRevision = -1;
        Refresh();
    }

    private void ShowRows(int count)
    {
        Transform? parent = _listContent != null ? _listContent : _empty != null ? _empty.transform.parent : null;
        while (parent != null && _rows.Count < count) _rows.Add(CreateRow(parent, _rows.Count));
        for (int i = 0; i < _rows.Count; i++) _rows[i].Root.SetActive(i < count);
    }

    private void ShowHeaders(int count)
    {
        while (_listContent != null && _headers.Count < count) _headers.Add(CreateHeader(_listContent, _headers.Count));
        for (int i = 0; i < _headers.Count; i++) _headers[i].Root.SetActive(i < count);
    }

    /// <summary>Header, then that property's lamps, then the next header. Inactive leftovers stay at the end.</summary>
    private void ArrangeGroups()
    {
        if (_listContent == null || _empty == null) return;
        _empty.transform.SetSiblingIndex(0);
        int sibling = 1;
        for (int group = 0; group < _groups.Count; group++)
        {
            _headers[group].Root.transform.SetSiblingIndex(sibling++);
            int start = _groups[group].Start;
            int end = start + _groups[group].Count;
            for (int row = start; row < end && row < _rows.Count; row++)
                _rows[row].Root.transform.SetSiblingIndex(sibling++);
        }
        if (_listContent is RectTransform rect)
            LayoutRebuilder.ForceRebuildLayoutImmediate(rect);
    }

    private PropertyHeader CreateHeader(Transform parent, int slot)
    {
        var go = UIFactory.Panel("Property" + slot, parent, GamePalette.Header);
        var fill = go.GetComponent<Image>();
        fill.sprite = UISprites.Rounded(6f);
        fill.type = Image.Type.Sliced;
        fill.raycastTarget = false;
        Prefer(go, UITheme.Dp(26f));
        var name = UIFactory.Text("Name", string.Empty, go.transform, UITheme.Sp(12), TextAnchor.MiddleLeft, FontStyle.Bold);
        name.color = GamePalette.TextPrimary;
        name.raycastTarget = false;
        Place(name.gameObject, 0.03f, 0.08f, 0.78f, 0.92f);
        var count = UIFactory.Text("Count", string.Empty, go.transform, UITheme.Sp(11), TextAnchor.MiddleRight);
        count.color = GamePalette.TextMuted;
        count.raycastTarget = false;
        Place(count.gameObject, 0.78f, 0.08f, 0.97f, 0.92f);
        return new PropertyHeader { Root = go, Name = name, Count = count };
    }

    private void CreateSwatch(Transform parent, ColorMode mode)
    {
        var go = UIFactory.Panel("Swatch" + (int)mode, parent, new Color(1f, 1f, 1f, 0f));
        var ring = go.GetComponent<Image>();
        ring.sprite = UISprites.Circle();
        var layout = go.GetComponent<LayoutElement>() ?? go.AddComponent<LayoutElement>();
        layout.flexibleWidth = 1f;
        layout.minHeight = UITheme.Dp(28f);
        layout.preferredHeight = UITheme.Dp(28f);
        layout.flexibleHeight = 0f;
        var button = go.AddComponent<Button>();
        button.transition = Selectable.Transition.None;
        var dotGo = UIFactory.Panel("Dot", go.transform, Color.white);
        var dot = dotGo.GetComponent<Image>();
        dot.raycastTarget = false;
        if (mode == ColorMode.Rainbow)
        {
            dot.sprite = RainbowDot();
            dot.color = Color.white;
        }
        else
        {
            dot.sprite = UISprites.Circle();
            dot.color = Swatch(mode);
        }
        Place(dotGo, 0.08f, 0.08f, 0.92f, 0.92f);
        ColorMode captured = mode;
        Wire(button, () => Act(key => LightRuntime.SetColor(key, captured)));
        _swatches.Add(new SwatchButton { Ring = ring, Mode = mode });
    }

    private void PaintPalette(ColorMode current)
    {
        for (int i = 0; i < _swatches.Count; i++)
        {
            bool on = _swatches[i].Mode == current;
            _swatches[i].Ring.color = on ? GamePalette.Blue : new Color(1f, 1f, 1f, 0f);
        }
    }

    private LampRow CreateRow(Transform parent, int slot)
    {
        var go = UIFactory.Panel($"Lamp{slot}", parent, GamePalette.Card);
        var fill = go.GetComponent<Image>();
        fill.sprite = UISprites.Rounded(8f);
        fill.type = Image.Type.Sliced;
        Prefer(go, UITheme.Dp(40f));
        var button = go.AddComponent<Button>();
        button.transition = Selectable.Transition.None;
        var swatchGo = UIFactory.Panel("Swatch", go.transform, Color.white);
        var swatch = swatchGo.GetComponent<Image>();
        swatch.sprite = UISprites.Circle();
        swatch.raycastTarget = false;
        Place(swatchGo, 0.02f, 0.28f, 0.07f, 0.72f);
        var name = UIFactory.Text("Name", string.Empty, go.transform, UITheme.Sp(13), TextAnchor.MiddleLeft, FontStyle.Bold);
        name.color = GamePalette.TextPrimary;
        name.raycastTarget = false;
        Place(name.gameObject, 0.09f, 0.12f, 0.74f, 0.88f);
        var bright = UIFactory.Text("Bright", string.Empty, go.transform, UITheme.Sp(12), TextAnchor.MiddleRight, FontStyle.Bold);
        bright.color = GamePalette.TextPrimary;
        bright.raycastTarget = false;
        Place(bright.gameObject, 0.74f, 0.15f, 0.97f, 0.85f);
        int captured = slot;
        Wire(button, () => OnRow(captured));
        return new LampRow { Root = go, Fill = fill, Swatch = swatch, Name = name, Bright = bright };
    }

    private Text MakeAction(Transform parent, string name, string label, Color fill, Color ink, Action handler)
    {
        var (go, button, text) = UIFactory.ButtonWithLabel(name + "Button", label, parent, fill, 180f, 64f);
        text.fontSize = UITheme.Sp(12);
        text.color = ink;
        var layout = go.GetComponent<LayoutElement>() ?? go.AddComponent<LayoutElement>();
        layout.flexibleWidth = 1f;
        layout.minHeight = UITheme.Dp(28f);
        layout.preferredHeight = UITheme.Dp(28f);
        layout.flexibleHeight = 0f;
        Wire(button, handler);
        return text;
    }

    private void Wire(Button button, Action handler)
    {
        var unityEvent = button.onClick;
        S1API.Utils.EventHelper.AddListener(handler, unityEvent);
        _wired.Add((unityEvent, handler));
    }

    /// <summary>
    /// Runs one lamp action and never presents a failed or storage-blocked action as a success.
    /// </summary>
    private void Act(Func<string, LightRuntime.LampActionResult> action)
    {
        if (string.IsNullOrEmpty(_selectionKey))
        {
            PostAction("Pick a lamp first.");
            return;
        }
        LightRuntime.LampActionResult result;
        try { result = action(_selectionKey); }
        catch (Exception ex) { Mod.Log.Warn("Phone action failed", ex); result = LightRuntime.LampActionResult.Rejected; }
        PostAction(result.Status switch
        {
            CommandStatus.Applied => $"Applied to {result.AppliedCount} lamps.",
            CommandStatus.Unchanged => "Already set.",
            CommandStatus.StaleRevision => "The lamp list changed. Check the targets and try again.",
            _ => "That change was not applied."
        });
        _seenRevision = -1;
        Refresh();
    }

    private void PostAction(string message)
    {
        _actionPosted = LightRuntime.ClockSeconds;
        if (_action == null) return;
        _action.text = message;
        _action.gameObject.SetActive(true);
    }

    private static void Prefer(GameObject go, float height)
    {
        var layout = go.GetComponent<LayoutElement>() ?? go.AddComponent<LayoutElement>();
        layout.minHeight = height;
        layout.preferredHeight = height;
        layout.flexibleHeight = 0f;
        layout.flexibleWidth = 1f;
    }

    /// <summary>Fills an explicit normalized anchor rect, so no layout group can resize the control.</summary>
    private static void Place(GameObject go, float minX, float minY, float maxX, float maxY)
    {
        var rect = go.GetComponent<RectTransform>();
        if (rect == null) rect = go.AddComponent<RectTransform>();
        rect.anchorMin = new Vector2(minX, minY);
        rect.anchorMax = new Vector2(maxX, maxY);
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;
        rect.pivot = new Vector2(0.5f, 0.5f);
    }

    private static string DescribeColor(ColorMode color) => color switch
    {
        ColorMode.Original => "Original",
        ColorMode.White => "White",
        ColorMode.Red => "Red",
        ColorMode.Orange => "Orange",
        ColorMode.Yellow => "Yellow",
        ColorMode.Green => "Green",
        ColorMode.Cyan => "Cyan",
        ColorMode.Blue => "Blue",
        ColorMode.Purple => "Purple",
        ColorMode.Pink => "Pink",
        ColorMode.Rainbow => "Rainbow",
        _ => "Unknown"
    };

    private static string DescribeIntensity(IntensityLevel level) => $"{(int)level}%";

    /// <summary>The same primaries the lamp actually emits. UI palette colors are too dull here.</summary>
    private static Color Swatch(ColorMode color) => color switch
    {
        ColorMode.White => Color.white,
        ColorMode.Red => new Color(1f, 0f, 0f),
        ColorMode.Orange => new Color(1f, 0.35f, 0f),
        ColorMode.Yellow => new Color(1f, 1f, 0f),
        ColorMode.Green => new Color(0f, 1f, 0f),
        ColorMode.Cyan => new Color(0f, 1f, 1f),
        ColorMode.Blue => new Color(0f, 0f, 1f),
        ColorMode.Purple => new Color(0.55f, 0f, 1f),
        ColorMode.Pink => new Color(1f, 0.15f, 0.55f),
        _ => GamePalette.TextMuted
    };

    private static Sprite? _rainbow;

    /// <summary>A hue wheel, so rainbow is not a second purple dot.</summary>
    private static Sprite RainbowDot()
    {
        if (_rainbow != null) return _rainbow;
        const int size = 32;
        var pixels = new Color32[size * size];
        float center = (size - 1) * 0.5f;
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                float dx = x - center;
                float dy = y - center;
                float radius = Mathf.Sqrt((dx * dx) + (dy * dy)) / center;
                if (radius > 1f)
                {
                    pixels[(y * size) + x] = new Color32(0, 0, 0, 0);
                    continue;
                }
                float hue = Mathf.Atan2(dy, dx) / (Mathf.PI * 2f);
                if (hue < 0f) hue += 1f;
                Color rgb = Hue(hue);
                byte alpha = (byte)Mathf.RoundToInt(Mathf.Clamp01((1f - radius) / 0.08f) * 255f);
                pixels[(y * size) + x] = new Color32(
                    (byte)Mathf.RoundToInt(rgb.r * 255f),
                    (byte)Mathf.RoundToInt(rgb.g * 255f),
                    (byte)Mathf.RoundToInt(rgb.b * 255f),
                    alpha);
            }
        }
        var texture = new Texture2D(size, size, TextureFormat.RGBA32, false);
        texture.SetPixels32(pixels);
        texture.Apply(false, true);
        _rainbow = Sprite.Create(texture, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f), 100f);
        _rainbow.name = "LightControlRainbow";
        return _rainbow;
    }

    private static Color Hue(float hue)
    {
        float scaled = hue * 6f;
        int sector = Mathf.FloorToInt(scaled);
        float fraction = scaled - sector;
        return sector switch
        {
            0 => new Color(1f, fraction, 0f),
            1 => new Color(1f - fraction, 1f, 0f),
            2 => new Color(0f, 1f, fraction),
            3 => new Color(0f, 1f - fraction, 1f),
            4 => new Color(fraction, 0f, 1f),
            _ => new Color(1f, 0f, 1f - fraction)
        };
    }

    /// <summary>Procedural bulb icon; no new asset is shipped for this build.</summary>
    private static Sprite GetIcon()
    {
        if (_icon != null) return _icon;
        const int size = 64;
        var pixels = new Color32[size * size];
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                float dx = x + 0.5f - (size * 0.5f);
                float dy = y + 0.5f - (size * 0.62f);
                float glass = Mathf.Clamp01(16.5f - Mathf.Sqrt((dx * dx) + (dy * dy)));
                bool screw = y > (size * 0.14f) && y < (size * 0.32f) && Mathf.Abs(dx) < 9f;
                float alpha = screw ? 1f : glass;
                pixels[(y * size) + x] = new Color32(255, 255, 255, (byte)Mathf.RoundToInt(Mathf.Clamp01(alpha) * 255f));
            }
        }
        var texture = new Texture2D(size, size, TextureFormat.RGBA32, false);
        texture.SetPixels32(pixels);
        texture.Apply(false, true);
        _iconTexture = texture;
        _icon = Sprite.Create(texture, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f), 100f);
        _icon.name = "LightControlIcon";
        return _icon;
    }

    private sealed class SwatchButton
    {
        public Image Ring = null!;
        public ColorMode Mode;
    }

    private sealed class PropertyHeader
    {
        public GameObject Root = null!;
        public Text Name = null!;
        public Text Count = null!;
    }

    private sealed class LampRow
    {
        public GameObject Root = null!;
        public Image Fill = null!;
        public Image Swatch = null!;
        public Text Name = null!;
        public Text Bright = null!;
        public string Key = string.Empty;
        public int LampIndex;
    }
}
