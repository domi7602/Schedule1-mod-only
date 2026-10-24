using System;
using System.Collections.Generic;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using LightControl.Core;
using MelonLoader;
using S1API.PhoneApp;
using S1API.UI;
using S1Mods.Shared;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace LightControl;

/// <summary>Graphite reference-canvas view. Runtime retains all native state and command ownership.</summary>
public sealed class LightControlApp : PhoneApp
{
    private const int ColorColumns = 5;
    private const float LightColumnMinWidth = 180f;
    private const float ColorColumnWidth = 224f;
    private const float BrightnessColumnWidth = 182f;
    private const float MoreColumnWidth = 64f;
    private const float ColumnGap = 24f;
    private const string UnavailableMessage = "That lamp is temporarily unavailable or hidden. Its settings are kept.";
    private const string SessionChangedMessage = "The save changed. That action was not applied.";

    // Local view colors; GamePalette and other apps retain their own theme.
    private static readonly Color Background = Hex(0x15171a);
    private static readonly Color Table = Hex(0x1b1d21);
    private static readonly Color TableBorder = Hex(0x32363c);
    private static readonly Color Line = Hex(0x30343a);
    private static readonly Color Heading = Hex(0x212429);
    private static readonly Color Dialog = Hex(0x24272c);
    private static readonly Color DialogBorder = Hex(0x4b5058);
    private static readonly Color Control = Hex(0x292d33);
    private static readonly Color ControlBorder = Hex(0x41464e);
    private static readonly Color ButtonFill = Hex(0x30353d);
    private static readonly Color Primary = Hex(0xe6e9ee);
    private static readonly Color Ink = Hex(0xeef0f3);
    private static readonly Color Muted = Hex(0xa1a7b0);
    private static readonly Color Subtle = Hex(0x787f89);
    private static readonly Color MeterTrack = Hex(0x484e57);
    private static readonly Color MeterFill = Hex(0xc7cdd5);

    private readonly List<(UnityEvent Event, Action Handler)> _wired = new();
    private readonly List<UiSelectionPolicy.ClickBinding> _clickBindings = new();
    private readonly List<UnityEngine.Object> _ownedAssets = new();
    private readonly Dictionary<Glyph, Sprite> _glyphs = new();
    private readonly List<ColorTile> _tiles = new();
    private readonly List<IntensityStep> _steps = new();
    private readonly List<Button> _modalButtons = new();
    private readonly List<ModalFooter> _footers = new();
    private readonly List<LampRow> _rows = new();
    private readonly List<PropertyHeader> _headers = new();
    private readonly List<int> _visible = new();
    private readonly List<int> _ordered = new();
    private readonly List<string?> _rowCodes = new();
    private readonly List<string?> _rowNames = new();
    private readonly List<string> _keys = new();
    private readonly List<UiSelectionPolicy.PropertyGroup> _groups = new();
    private readonly UiSelectionPolicy.ColorPickerState _colorPicker = new();

    private GameObject? _root;
    private RectTransform? _referenceCanvas;
    private Transform? _listContent;
    private CanvasGroup? _contentGroup;
    private GameObject? _columnHeader;
    private GameObject? _columnHeaderSpacer;
    private float _lastHeadingTop = float.NaN;
    private GameObject? _toast;
    private GameObject? _modalRoot;
    private GameObject? _modalLayer;
    private GameObject? _colorCard;
    private GameObject? _brightCard;
    private GameObject? _actionsCard;
    private GameObject? _copyCard;
    private CanvasGroup? _modalGroup;
    private Text _summary = null!;
    private Text _availability = null!;
    private Text _status = null!;
    private Text _action = null!;
    private Text _empty = null!;
    private Text _pickerTarget = null!;
    private Text _pickerPrefix = null!;
    private Text _pickerName = null!;
    private Text _pickerDetail = null!;
    private Text _pickerNote = null!;
    private Text _brightTarget = null!;
    private Text _brightValue = null!;
    private Text _brightNote = null!;
    private RectTransform _brightFill = null!;
    private Text _actionsTarget = null!;
    private Text _copySource = null!;
    private Text _copyProperty = null!;
    private Text _copyColor = null!;
    private Text _copyIntensity = null!;
    private Text _copyCount = null!;
    private Text _confirmText = null!;
    private Text _copyButtonText = null!;
    private Text _applyProperty = null!;
    private Text _applyWorld = null!;
    private Image _copySwatch = null!;
    private GameObject _originalCheck = null!;
    private GameObject _rainbowCheck = null!;
    private Button? _pickerSelect;
    private Button? _worldSelect;
    private Button? _propertyButton;
    private Button? _worldButton;
    private Font? _regularFont;
    private Font? _semiboldFont;
    private Sprite? _rainbow;
    private Sprite? _icon;

    private ModalKind _modal;
    private string _modalKey = string.Empty;
    private Guid _modalSession;
    private string _pickerSignature = string.Empty;
    private string _brightSignature = string.Empty;
    private string _actionsSignature = string.Empty;
    private string? _selectionKey;
    private Guid _seenSession;
    private long _seenCatalog = -1;
    private long _seenRevision = -1;
    private string _seenNotice = string.Empty;
    private bool _seenScan;
    private bool _seenTruncated;
    private int _seenUnsupported = -1;
    private int _seenConflicts = -1;
    private int _seenRestores = -1;
    private string _seenStatusReason = string.Empty;
    private double _actionPosted = double.NegativeInfinity;
    private LightRuntime.CopyPreview _propertyPreview;
    private LightRuntime.CopyPreview _worldPreview;
    private LightRuntime.CopyPreview _pendingWorldCopy;
    private bool _hasPropertyPreview;
    private bool _hasWorldPreview;
    private bool _worldConfirm;
    private bool _destroyed;
    private bool _updateErrorLogged;
    private bool _layoutLogged;
    private bool _modalGeometryPending;
    private float _lastLayoutWidth = -1f;
    private float _lastLayoutHeight = -1f;
    private float _initialThemeScale;

    protected override string AppName => "LightControl";
    protected override string AppTitle => "Light Control";
    protected override string IconLabel => "Lights";
    protected override string IconFileName => string.Empty;
    protected override EOrientation Orientation => EOrientation.Horizontal;
    protected override Sprite IconSprite => GetIcon();

    protected override void OnCreated()
    {
        base.OnCreated();
        MelonEvents.OnUpdate.Unsubscribe(Update);
        MelonEvents.OnUpdate.Subscribe(Update);
    }

    protected override void OnCreatedUI(GameObject container)
    {
        UITheme.InitializeForDashboard(container.GetComponent<RectTransform>());
        _initialThemeScale = UITheme.Scale;
        // UITheme normalizes landscape as portrait and clamps its global scale. Capture that
        // initialization for diagnostics, then normalize ALL geometry/font metrics back to the
        // approved reference units. One owned transform maps them uniformly to the real rect.
        // Lazy rows and reusable modals never read UITheme.Scale/Sp/Dp after this point.
        try { BuildView(container); }
        catch
        {
            _destroyed = true;
            MelonEvents.OnUpdate.Unsubscribe(Update);
            ReleaseView();
            throw;
        }
    }

    private void BuildView(GameObject container)
    {
        LoadFonts();
        _root = UIFactory.Panel("LightControl_Root", container.transform, Background, fullAnchor: true);
        _root.SetActive(false);
        _root.AddComponent<RectMask2D>();
        var reference = UIFactory.Panel("GraphiteReferenceCanvas", _root.transform, Background);
        _referenceCanvas = reference.GetComponent<RectTransform>();
        _referenceCanvas.anchorMin = _referenceCanvas.anchorMax = new Vector2(0.5f, 0.5f);
        _referenceCanvas.pivot = new Vector2(0.5f, 0.5f);
        _referenceCanvas.anchoredPosition = Vector2.zero;
        _referenceCanvas.sizeDelta = new Vector2(UiSelectionPolicy.GraphiteReferenceWidth, UiSelectionPolicy.GraphiteReferenceHeight);
        _referenceCanvas.localScale = Vector3.zero;
        var header = Panel("Header", reference.transform, Color.clear);
        At(header, 36f, 32f, 1129f, 84f);
        Label("Title", "Lights", header.transform, 34, 0f, 0f, 500f, 42f, bold: true);
        _summary = Label("Summary", string.Empty, header.transform, 16, 0f, 48f, 520f, 24f, Muted);
        _availability = Label("Availability", string.Empty, header.transform, 16, 530f, 6f, 599f, 25f,
            Hex(0xd2d6dc), TextAnchor.MiddleRight);
        // Reserved header area for truthful scan/persistence/error state, with no fake save check.
        _status = Label("Status", string.Empty, header.transform, 14, 530f, 36f, 599f, 48f,
            Subtle, TextAnchor.UpperRight);
        var body = Bordered("LampTable", reference.transform, Table, TableBorder, 13f);
        At(body, 36f, 140f, 1129f, 487f);
        _contentGroup = body.AddComponent<CanvasGroup>();
        var content = UIFactory.ScrollableVerticalList("LampScroll", body.transform.Find("Surface"), out var scroll);
        PinScrollWidth(content);
        _listContent = content;
        var viewport = scroll.viewport.GetComponent<Image>();
        viewport.sprite = UISprites.Rounded(12f);
        viewport.type = Image.Type.Sliced;
        viewport.color = Color.white;
        var contentLayout = content.GetComponent<VerticalLayoutGroup>();
        contentLayout.spacing = 0f;
        contentLayout.padding = new RectOffset(0, 0, 0, 0);
        contentLayout.childControlWidth = true;
        contentLayout.childForceExpandWidth = true;
        contentLayout.childControlHeight = true;
        contentLayout.childForceExpandHeight = false;
        scroll.vertical = true;
        scroll.horizontal = false;
        scroll.inertia = false;
        scroll.movementType = ScrollRect.MovementType.Clamped;
        scroll.scrollSensitivity = 36f;
        _empty = Text("Empty", "No supported grow lamps found.", content, 18, Muted, TextAnchor.MiddleCenter);
        Prefer(_empty.gameObject, 126f);
        _columnHeaderSpacer = Panel("ColumnHeaderSpace", content, Color.clear);
        Prefer(_columnHeaderSpacer, 36f);
        _columnHeader = CreateColumnHeader(scroll.viewport);
        // The opaque heading consumes pointer hits over the rows it covers, while scroll events
        // still reach its ScrollRect ancestor. The spacer retains its one place in list layout.
        _columnHeader.GetComponent<Image>().raycastTarget = true;
        TopBand(_columnHeader, 52f, 36f);
        _toast = Bordered("ActionMessage", reference.transform, Hex(0x11151b), Hex(0x5b646f), 9f);
        At(_toast, 210.5f, 574f, 780f, 64f);
        _action = Label("Message", string.Empty, _toast.transform, 15, 17f, 10f, 746f, 44f,
            Ink, TextAnchor.MiddleCenter);
        _toast.SetActive(false);
        BuildModal();
        MeasureLayout();
    }

    /// <summary>The same four-column definition is used by headings and pooled rows.</summary>
    private static HorizontalLayoutGroup Columns(GameObject go)
    {
        var layout = go.AddComponent<HorizontalLayoutGroup>();
        layout.padding = new RectOffset(16, 16, 0, 0);
        layout.spacing = ColumnGap;
        layout.childControlWidth = true;
        layout.childControlHeight = true;
        layout.childForceExpandWidth = false;
        layout.childForceExpandHeight = false;
        layout.childAlignment = TextAnchor.MiddleLeft;
        return layout;
    }

    private GameObject CreateColumnHeader(Transform parent)
    {
        var columns = Panel("LampColumns", parent, Table);
        Columns(columns);
        Prefer(columns, 36f);
        var lightColumn = Panel("LightColumn", columns.transform, Color.clear);
        var lightLayout = lightColumn.AddComponent<LayoutElement>();
        lightLayout.flexibleWidth = 1f;
        lightLayout.minWidth = LightColumnMinWidth;
        lightLayout.preferredHeight = 36f;
        Stretch(Label("Light", "Light", lightColumn.transform, 14, 0f, 0f, 1f, 1f, Muted).gameObject, 58f, 0f, 0f, 0f);
        foreach (var (label, width) in new[] { ("Color", ColorColumnWidth), ("Brightness", BrightnessColumnWidth), ("More", MoreColumnWidth) })
        {
            var text = Text(label + "Column", label, columns.transform, 14, Muted,
                label == "More" ? TextAnchor.MiddleCenter : TextAnchor.MiddleLeft);
            SetButtonSize(text.gameObject, width, 36f);
        }
        Separator(columns.transform);
        return columns;
    }

    // Fixed reference geometry keeps every modal and its footer together inside the owned canvas.
    private void BuildModal()
    {
        _modalRoot = UIFactory.Panel("LightControl_Modal", _referenceCanvas!.transform, Color.clear, fullAnchor: true);
        _modalRoot.GetComponent<Image>().raycastTarget = false;
        _modalRoot.AddComponent<LayoutElement>().ignoreLayout = true;
        var backdrop = UIFactory.Panel("Backdrop", _modalRoot.transform, new Color(8f / 255f, 9f / 255f, 11f / 255f, 0.75f), fullAnchor: true);
        var dismiss = backdrop.AddComponent<Button>();
        dismiss.transition = Selectable.Transition.None;
        Wire(dismiss, CloseModal);
        _modalLayer = UIFactory.Panel("CardLayer", _modalRoot.transform, Color.clear, fullAnchor: true);
        var blocker = _modalLayer.GetComponent<Image>();
        blocker.raycastTarget = false;
        _modalGroup = _modalLayer.AddComponent<CanvasGroup>();
        BuildColorCard();
        BuildBrightnessCard();
        BuildActionsCard();
        BuildCopyCard();
        _modalRoot.SetActive(false);
    }

    private ModalCard NewCard(string name, float width, float height, string heading, float padding = 28f)
    {
        var card = Bordered(name, _modalLayer!.transform, Dialog, DialogBorder, 15f);
        var rect = card.GetComponent<RectTransform>();
        rect.anchorMin = rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.pivot = new Vector2(0.5f, 0.5f);
        rect.anchoredPosition = Vector2.zero;
        rect.sizeDelta = new Vector2(width, height);
        var fill = card.GetComponent<Image>();
        fill.raycastTarget = true;
        var cardButton = card.AddComponent<Button>();
        cardButton.transition = Selectable.Transition.None;
        Wire(cardButton, NoOpCardClick);
        Label("Title", heading, card.transform, 27, padding, padding, width - padding * 2f, 33f, bold: true);
        var target = Label("Target", string.Empty, card.transform, 16, padding, padding + 39f,
            width - padding * 2f, 22f, Hex(0xadb4bf));
        return new ModalCard { Root = card, Target = target };
    }

    private GameObject FooterRow(Transform parent, string backLabel, string selectLabel, Action confirm, out Button select)
    {
        var row = FooterStrip(parent);
        var backButton = FooterButton(row.transform, "CancelButton", backLabel, false, CloseModal);
        select = FooterButton(row.transform, "ApplyButton", selectLabel, true, confirm);
        _footers.Add(new ModalFooter { Card = parent.gameObject, BackButton = backButton, SelectButton = select });
        return row;
    }

    private GameObject FooterRow(Transform parent, string label, Action dismiss)
    {
        var row = FooterStrip(parent);
        var button = FooterButton(row.transform, "CloseButton", label, false, dismiss);
        _footers.Add(new ModalFooter { Card = parent.gameObject, BackButton = button });
        return row;
    }

    private static GameObject FooterStrip(Transform parent)
    {
        var row = Panel("Footer", parent, Color.clear);
        var rect = row.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0f, 0f);
        rect.anchorMax = new Vector2(1f, 0f);
        rect.pivot = new Vector2(0.5f, 0f);
        rect.offsetMin = new Vector2(28f, 28f);
        rect.offsetMax = new Vector2(-28f, 82f);
        var strip = row.AddComponent<HorizontalLayoutGroup>();
        strip.spacing = 12f;
        strip.childControlWidth = true;
        strip.childControlHeight = true;
        strip.childForceExpandWidth = true;
        strip.childForceExpandHeight = false;
        strip.childAlignment = TextAnchor.MiddleCenter;
        return row;
    }

    private Button FooterButton(Transform parent, string name, string label, bool primary, Action action)
    {
        var go = Bordered(name, parent, primary ? Primary : ButtonFill, primary ? Primary : Hex(0x4c535d), 10f);
        var buttonLayout = go.AddComponent<LayoutElement>();
        buttonLayout.minHeight = 54f;
        buttonLayout.preferredHeight = 54f;
        buttonLayout.flexibleHeight = 0f;
        buttonLayout.minWidth = 0f;
        buttonLayout.flexibleWidth = 1f;
        var labelText = Text("Label", label, go.transform, 18, primary ? Background : Ink, TextAnchor.MiddleCenter, true);
        Stretch(labelText.gameObject);
        return Clickable(go, action);
    }

    private void BuildColorCard()
    {
        var card = NewCard("ColorCard", 600f, 456f, "Light color");
        _colorCard = card.Root;
        _pickerTarget = card.Target;
        var grid = Panel("Palette", card.Root.transform, Color.clear);
        At(grid, 72f, 111f, 456f, 174f);
        for (int i = 0; i < UiSelectionPolicy.PickerColorCount; i++)
            CreateColorTile(grid.transform, UiSelectionPolicy.PickerColorAt(i), i);
        var pending = Panel("PendingColor", card.Root.transform, Color.clear);
        At(pending, 28f, 301f, 544f, 29f);
        var strip = pending.AddComponent<HorizontalLayoutGroup>();
        strip.spacing = 12f;
        strip.childControlWidth = true;
        strip.childControlHeight = true;
        strip.childForceExpandWidth = false;
        strip.childForceExpandHeight = true;
        strip.childAlignment = TextAnchor.MiddleCenter;
        _pickerPrefix = Text("Prefix", "Current", pending.transform, 15, Hex(0xa7b0bc));
        _pickerName = Text("ColorName", "Original", pending.transform, 20, Ink, bold: true);
        _pickerDetail = Text("ColorDetail", "Choose a color", pending.transform, 14, Hex(0xa7b0bc));
        _pickerNote = Label("PickerNote", string.Empty, card.Root.transform, 14, 28f, 334f, 544f, 34f,
            Muted, TextAnchor.MiddleCenter);
        FooterRow(card.Root.transform, "Cancel", "Apply", ConfirmColor, out _pickerSelect);
    }

    private void CreateColorTile(Transform parent, ColorMode color, int index)
    {
        var go = Bordered("ColorTile" + (int)color, parent, Dialog, Hex(0x434a54), 12f, 2f);
        At(go, (index % ColorColumns) * 94f, (index / ColorColumns) * 94f, 80f, 80f);
        var paint = Panel("Paint", go.transform, Swatch(color), 7f);
        At(paint, 7f, 7f, 66f, 66f);
        PaintSwatch(paint.GetComponent<Image>(), color, 7f);
        var check = Bordered("SelectedCheck", go.transform, Hex(0x14171b), Hex(0xb1b2b4), 7f);
        At(check, 46f, 46f, 23f, 23f);
        DrawGlyph(check.transform, Glyph.Check, 4f, 4f, 15f, 15f, Color.white);
        check.SetActive(false);
        var button = Clickable(go, () =>
        {
            if (_modal == ModalKind.Color && _colorPicker.Choose(color)) PaintPicker();
        });
        _tiles.Add(new ColorTile { Ring = go.GetComponent<Image>(), Check = check, Mode = color });
        _modalButtons.Add(button);
    }

    private void BuildBrightnessCard()
    {
        var card = NewCard("BrightnessCard", 600f, 395f, "Brightness");
        _brightCard = card.Root;
        _brightTarget = card.Target;
        _brightValue = Label("BrightnessValue", string.Empty, card.Root.transform, 40, 28f, 113f, 270f, 46f, bold: true);
        Label("Maximum", "200% maximum", card.Root.transform, 15, 300f, 131f, 272f, 25f, Muted, TextAnchor.MiddleRight);
        _brightFill = Meter(card.Root.transform, "BrightnessMeter", 28f, 171f, 544f, 6f);
        var choices = Panel("BrightnessChoices", card.Root.transform, Color.clear);
        At(choices, 28f, 199f, 544f, 54f);
        // All seven documented levels are on ONE reference row. They apply immediately.
        const float stepWidth = (544f - 6f * 8f) / 7f;
        for (int i = 0; i < UiSelectionPolicy.IntensityCount; i++)
        {
            var level = UiSelectionPolicy.IntensityAt(i);
            var go = Bordered("Step" + (int)level, choices.transform, ButtonFill, Hex(0x505762), 9f);
            At(go, i * (stepWidth + 8f), 0f, stepWidth, 54f);
            var text = Text("Label", UiSelectionPolicy.IntensityLabel(level), go.transform, 18, Ink, TextAnchor.MiddleCenter, true);
            Stretch(text.gameObject);
            var button = go.AddComponent<Button>();
            button.transition = Selectable.Transition.None;
            go.GetComponent<Image>().raycastTarget = true;
            IntensityLevel captured = level;
            Wire(button, () => ApplyBrightnessStep(captured));
            _steps.Add(new IntensityStep { Root = go, Text = text, Level = level });
            _modalButtons.Add(button);
        }
        _brightNote = Label("BrightNote", "Applies immediately. Try another level to compare.", card.Root.transform,
            15, 28f, 267f, 544f, 37f, Hex(0xaeb7c4));
        FooterRow(card.Root.transform, "Close", CloseModal);
    }

    private void BuildActionsCard()
    {
        var card = NewCard("ActionsCard", 530f, 554f, "Light actions", 26f);
        _actionsCard = card.Root;
        _actionsTarget = card.Target;
        var list = Panel("ActionList", card.Root.transform, Color.clear);
        At(list, 26f, 107f, 478f, 342f);
        _originalCheck = MakeAction(list.transform, "Original", "Original", "Game color · keep brightness", Glyph.Original, 0f,
            () => RunModalAction(key => LightRuntime.SetColor(key, ColorMode.Original))).Check;
        _rainbowCheck = MakeAction(list.transform, "Rainbow", "Rainbow", "Cycle colors · keep brightness", Glyph.Rainbow, 65f,
            () => RunModalAction(key => LightRuntime.SetColor(key, ColorMode.Rainbow))).Check;
        MakeAction(list.transform, "Reset", "Reset", "Original color · 100% brightness", Glyph.Reset, 130f,
            () => RunModalAction(key => LightRuntime.ResetLamp(key)));
        var divider = Panel("ActionDivider", list.transform, Hex(0x40464f));
        At(divider, 10f, 203f, 458f, 1f);
        var property = MakeAction(list.transform, "ApplyProperty", "Copy to property", string.Empty, Glyph.Copy, 212f, ApplyPropertyCopy);
        _applyProperty = property.Detail;
        _propertyButton = property.Button;
        var world = MakeAction(list.transform, "ApplyWorld", "Copy to all properties", string.Empty, Glyph.Properties, 277f, BeginWorldCopy);
        _applyWorld = world.Detail;
        _worldButton = world.Button;
        FooterRow(card.Root.transform, "Close", CloseModal);
    }

    private void BuildCopyCard()
    {
        var card = NewCard("WorldCopyCard", 560f, 499f, "Copy to all properties?");
        _copyCard = card.Root;
        card.Target.text = "Copy this light's color and brightness.";
        Label("SourceHeading", "Source", card.Root.transform, 16, 28f, 113f, 110f, 25f, Muted);
        _copySource = Label("Source", string.Empty, card.Root.transform, 18, 154f, 113f, 378f, 25f);
        _copyProperty = Label("Property", string.Empty, card.Root.transform, 14, 154f, 141f, 378f, 21f, Muted);
        Label("ColorHeading", "Color", card.Root.transform, 16, 28f, 180f, 110f, 25f, Muted);
        var swatch = Panel("ColorSwatch", card.Root.transform, Color.clear, 4f);
        At(swatch, 154f, 182f, 22f, 22f);
        _copySwatch = swatch.GetComponent<Image>();
        _copyColor = Label("Color", string.Empty, card.Root.transform, 18, 186f, 180f, 346f, 25f);
        Label("BrightnessHeading", "Brightness", card.Root.transform, 16, 28f, 222f, 110f, 25f, Muted);
        _copyIntensity = Label("Brightness", string.Empty, card.Root.transform, 18, 154f, 222f, 378f, 25f);
        var targets = Bordered("CopyTargets", card.Root.transform, Hex(0x2c323a), Hex(0x444c58), 10f);
        At(targets, 28f, 271f, 504f, 84f);
        _copyCount = Label("Count", string.Empty, targets.transform, 22, 18f, 13f, 468f, 28f, bold: true);
        Label("Scope", "Includes the source light", targets.transform, 15, 18f, 46f, 468f, 25f, Hex(0xb8c1ce));
        _confirmText = Label("Confirmation", string.Empty, card.Root.transform, 14, 28f, 368f, 504f, 42f,
            Hex(0xa9b3c0), TextAnchor.UpperLeft);
        FooterRow(card.Root.transform, "Cancel", "Copy lights", ConfirmWorldCopy, out _worldSelect);
        _copyButtonText = _worldSelect.GetComponentInChildren<Text>();
    }

    private ActionControl MakeAction(Transform parent, string name, string label, string detail, Glyph glyph, float top, Action action)
    {
        var go = Panel(name, parent, Color.clear, 9f);
        At(go, 0f, top, 478f, 65f);
        DrawGlyph(go.transform, glyph, 10f, 22f, 21f, 21f, Hex(0xb9c2cf));
        Label("Label", label, go.transform, 18, 45f, 8f, 390f, 24f, bold: true);
        var subtitle = Label("Detail", detail, go.transform, 14, 45f, 34f, 405f, 22f, Hex(0xa8b1be));
        var check = DrawGlyph(go.transform, Glyph.Check, 450f, 23f, 18f, 18f, Hex(0xd8dfe7));
        check.SetActive(false);
        var button = Clickable(go, action);
        _modalButtons.Add(button);
        return new ActionControl { Detail = subtitle, Button = button, Check = check };
    }

    private void OnRow(int slot)
    {
        if (!TryRowKey(slot, out var view)) return;
        if (!string.Equals(_selectionKey, view.Key, StringComparison.Ordinal)) CancelWorldCopy();
        _selectionKey = view.Key;
        OpenColorPicker(slot);
    }

    private void OpenColorPicker(int slot)
    {
        if (!TryRowKey(slot, out var view)) return;
        if (!_colorPicker.Open(LightRuntime.SessionId, view.Key, view.Color)) return;
        _selectionKey = view.Key;
        _modalKey = view.Key;
        _modalSession = LightRuntime.SessionId;
        _pickerSignature = string.Empty;
        ShowModal(ModalKind.Color);
        PaintPicker();
    }

    private void OpenBrightness(int slot)
    {
        if (!TryRowKey(slot, out var view)) return;
        if (!UiSelectionPolicy.IsIntensityStep(view.Intensity)) return;
        _modalKey = view.Key;
        _modalSession = LightRuntime.SessionId;
        _brightSignature = string.Empty;
        ShowModal(ModalKind.Brightness);
        PaintBrightness();
    }

    private void OpenActions(int slot)
    {
        if (!TryRowKey(slot, out var view)) return;
        _modalKey = view.Key;
        _modalSession = LightRuntime.SessionId;
        _actionsSignature = string.Empty;
        _worldConfirm = false;
        _hasPropertyPreview = LightRuntime.TryBuildPreview(view.Key, false, out _propertyPreview);
        _hasWorldPreview = LightRuntime.TryBuildPreview(view.Key, true, out _worldPreview);
        ShowModal(ModalKind.Actions);
        PaintActions();
    }

    private bool TryRowKey(int slot, out LightRuntime.LampView view)
    {
        view = default;
        if (_destroyed || !IsOpen() || _modal != ModalKind.None || slot < 0 || slot >= _rows.Count) return false;
        var row = _rows[slot];
        if (!row.Root.activeSelf || row.Session != LightRuntime.SessionId)
        {
            PostAction(SessionChangedMessage);
            return false;
        }
        string key = row.Key;
        bool known = LightRuntime.IsKnownLamp(key);
        bool visible = known && UiSelectionPolicy.SelectionVisible(key, _keys);
        if (UiSelectionPolicy.ClassifyEditor(key, known, visible) != UiSelectionPolicy.EditorAvailability.Editable)
        {
            PostAction(UnavailableMessage);
            return false;
        }
        view = FindView(key);
        return string.Equals(view.Key, key, StringComparison.Ordinal);
    }

    private void ConfirmColor()
    {
        if (_modal != ModalKind.Color) return;
        string sourceKey = _colorPicker.SourceKey;
        bool known = LightRuntime.IsKnownLamp(sourceKey);
        bool visible = known && UiSelectionPolicy.SelectionVisible(sourceKey, _keys);
        var view = known ? FindView(sourceKey) : default;
        var outcome = _colorPicker.Confirm(LightRuntime.SessionId, sourceKey, known, visible, view.Color, out var commit);
        if (outcome != UiSelectionPolicy.PickerOutcome.Committed)
        {
            PaintPicker();
            if (outcome == UiSelectionPolicy.PickerOutcome.Unavailable) PostAction(UnavailableMessage);
            return;
        }
        // Consume the staged claim before native mutation; a repeated click cannot apply twice.
        CloseModal();
        ActOn(commit.SessionId, commit.SourceKey, key => LightRuntime.SetColor(key, commit.Color));
    }

    private void ApplyBrightnessStep(IntensityLevel level)
    {
        if (_modal != ModalKind.Brightness) return;
        if (!UiSelectionPolicy.IsIntensityStep(level)) return;
        Guid session = _modalSession;
        string key = _modalKey;
        if (session == Guid.Empty || string.IsNullOrEmpty(key)) return;
        ActOn(session, key, exact => LightRuntime.SetIntensity(exact, level));
    }

    private void ShowModal(ModalKind kind)
    {
        if (kind != ModalKind.Color) _colorPicker.Cancel();
        if (kind != ModalKind.Actions && kind != ModalKind.WorldCopy) CancelWorldCopy();
        _modal = kind;
        if (_colorCard != null) _colorCard.SetActive(kind == ModalKind.Color);
        if (_brightCard != null) _brightCard.SetActive(kind == ModalKind.Brightness);
        if (_actionsCard != null) _actionsCard.SetActive(kind == ModalKind.Actions);
        if (_copyCard != null) _copyCard.SetActive(kind == ModalKind.WorldCopy);
        if (_modalRoot != null) _modalRoot.SetActive(true);
        _modalGeometryPending = true;
        _actionPosted = double.NegativeInfinity;
        if (_toast != null) _toast.SetActive(false);
        if (_contentGroup != null) _contentGroup.interactable = false;
    }

    private void CloseModal()
    {
        _colorPicker.Cancel();
        CancelWorldCopy();
        _hasPropertyPreview = _hasWorldPreview = false;
        _propertyPreview = _worldPreview = default;
        _modal = ModalKind.None;
        _modalKey = string.Empty;
        _modalSession = Guid.Empty;
        _pickerSignature = _brightSignature = _actionsSignature = string.Empty;
        _modalGeometryPending = false;
        if (_modalRoot != null) _modalRoot.SetActive(false);
        if (_contentGroup != null) _contentGroup.interactable = true;
    }

    private void PaintPicker()
    {
        if (_pickerTarget == null || _colorCard == null) return;
        var view = FindView(_colorPicker.SourceKey);
        bool known = LightRuntime.IsKnownLamp(_colorPicker.SourceKey);
        bool visible = known && UiSelectionPolicy.SelectionVisible(_colorPicker.SourceKey, _keys);
        string signature = $"{_colorPicker.SourceKey}|{_colorPicker.HasPendingColor}|{_colorPicker.PendingColor}|{_colorPicker.Stale}|{known}|{visible}";
        if (signature == _pickerSignature) return;
        _pickerSignature = signature;
        _pickerTarget.text = known ? $"{view.Label} · {PropertyOf(view)}" : "That lamp is no longer available.";
        foreach (var tile in _tiles)
        {
            bool on = _colorPicker.HasPendingColor && tile.Mode == _colorPicker.PendingColor;
            tile.Ring.color = on ? Color.white : Hex(0x434a54);
            tile.Check.SetActive(on);
        }
        _pickerPrefix.text = _colorPicker.HasPendingColor ? "Selected" : "Current";
        _pickerName.text = UiSelectionPolicy.ColorName(_colorPicker.HasPendingColor ? _colorPicker.PendingColor : _colorPicker.SourceColor);
        _pickerDetail.text = _colorPicker.HasPendingColor ? ColorDetail(_colorPicker.PendingColor) : "Choose a color";
        SetModalEnabled(visible);
        SetConfirm(_pickerSelect, visible && _colorPicker.HasPendingColor);
        _pickerNote.text = _colorPicker.Stale ? "The lamp's color changed. Choose again."
            : visible ? "LED panel and emitted light use the same color." : "The lamp is unavailable. Its settings are kept.";
    }

    private void PaintBrightness()
    {
        if (_brightTarget == null || _brightCard == null) return;
        var view = FindView(_modalKey);
        bool known = LightRuntime.IsKnownLamp(_modalKey);
        bool visible = known && UiSelectionPolicy.SelectionVisible(_modalKey, _keys);
        bool readable = known && UiSelectionPolicy.IsIntensityStep(view.Intensity);
        string signature = $"{_modalKey}|{view.Intensity}|{known}|{visible}";
        if (signature == _brightSignature) return;
        _brightSignature = signature;
        _brightTarget.text = readable ? $"{view.Label} · {PropertyOf(view)}" : "That lamp is no longer available.";
        _brightValue.text = readable ? UiSelectionPolicy.IntensityLabel(view.Intensity) : "—";
        SetMeter(_brightFill, readable ? UiSelectionPolicy.BrightnessFill(view.Intensity) : 0f);
        foreach (var step in _steps)
        {
            bool selected = readable && step.Level == view.Intensity;
            step.Root.GetComponent<Image>().color = selected ? Primary : Hex(0x505762);
            step.Root.transform.Find("Surface").GetComponent<Image>().color = selected ? Primary : ButtonFill;
            step.Text.color = selected ? Background : Ink;
        }
        SetModalEnabled(visible && readable);
        _brightNote.text = visible ? "Applies immediately. Try another level to compare."
            : "The lamp is unavailable. Its settings are kept.";
    }

    private void PaintActions()
    {
        if (_actionsTarget == null) return;
        var view = FindView(_modalKey);
        bool known = LightRuntime.IsKnownLamp(_modalKey);
        bool visible = known && UiSelectionPolicy.SelectionVisible(_modalKey, _keys);
        int propertyCount = _hasPropertyPreview ? _propertyPreview.TargetKeys.Length : 0;
        int worldCount = _hasWorldPreview ? _worldPreview.TargetKeys.Length : 0;
        string signature = $"{_modalKey}|{view.Color}|{view.Intensity}|{known}|{visible}|{propertyCount}|{worldCount}|{_worldConfirm}";
        if (signature == _actionsSignature) return;
        _actionsSignature = signature;
        _actionsTarget.text = known ? $"{view.Label} · {PropertyOf(view)}" : "That lamp is no longer available.";
        var mode = UiSelectionPolicy.ModeOf(view.Color);
        _originalCheck.SetActive(known && mode == UiSelectionPolicy.LightMode.Original);
        _rainbowCheck.SetActive(known && mode == UiSelectionPolicy.LightMode.Rainbow);
        _applyProperty.text = _hasPropertyPreview ? UiSelectionPolicy.PropertyCopyLabel(PropertyOf(view), propertyCount) : "No property preview available";
        _applyWorld.text = _hasWorldPreview ? $"{worldCount} lights across all properties · incl. source" : "No copy preview available";
        SetModalEnabled(visible);
        if (_propertyButton != null) _propertyButton.interactable = visible && _hasPropertyPreview;
        if (_worldButton != null) _worldButton.interactable = visible && _hasWorldPreview;
        SetConfirm(_worldSelect, visible && _worldConfirm);
    }

    private void SetModalEnabled(bool enabled)
    {
        for (int i = 0; i < _modalButtons.Count; i++)
            if (_modalButtons[i] != null) _modalButtons[i].interactable = enabled;
        if (_modalGroup != null) _modalGroup.alpha = enabled ? 1f : 0.7f;
    }

    private static void SetConfirm(Button? button, bool enabled)
    {
        if (button == null) return;
        button.interactable = enabled;
        button.GetComponent<Image>().color = enabled ? Primary : ControlBorder;
        button.transform.Find("Surface").GetComponent<Image>().color = enabled ? Primary : Hex(0x353b43);
        var text = button.GetComponentInChildren<Text>();
        if (text != null) text.color = enabled ? Background : Hex(0x8f97a3);
    }

    private void RunModalAction(Func<string, LightRuntime.LampActionResult> action)
    {
        if (_modal != ModalKind.Actions || _worldConfirm) return;
        if (string.IsNullOrEmpty(_modalKey)) return;
        Act(action);
        CloseModal();
    }

    private void ApplyPropertyCopy()
    {
        if (!_hasPropertyPreview) return;
        var shown = _propertyPreview;
        RunModalAction(_ => LightRuntime.ApplyPreview(shown, false));
    }

    private void BeginWorldCopy()
    {
        if (_modal != ModalKind.Actions) return;
        if (!_hasWorldPreview || string.IsNullOrEmpty(_modalKey)) return;
        var view = FindView(_modalKey);
        _pendingWorldCopy = _worldPreview;
        _worldConfirm = true;
        _copySource.text = view.Label;
        _copyProperty.text = PropertyOf(view);
        _copyColor.text = UiSelectionPolicy.ColorName(_pendingWorldCopy.Color);
        PaintSwatch(_copySwatch, _pendingWorldCopy.Color, 4f);
        _copyIntensity.text = UiSelectionPolicy.IntensityLabel(_pendingWorldCopy.Intensity);
        int count = _pendingWorldCopy.TargetKeys.Length;
        _copyCount.text = $"{count} lights · all properties";
        _copyButtonText.text = $"Copy to {count} lights";
        _confirmText.text = UiSelectionPolicy.WorldCopyConfirmation(view.Label, PropertyOf(view),
            _pendingWorldCopy.Color, _pendingWorldCopy.Intensity, count);
        ShowModal(ModalKind.WorldCopy);
        _actionsSignature = string.Empty;
        PaintActions();
    }

    private void ConfirmWorldCopy()
    {
        if (_modal != ModalKind.WorldCopy) return;
        if (!_worldConfirm || !_hasWorldPreview) return;
        var shown = _pendingWorldCopy;
        _worldConfirm = false;
        if (string.IsNullOrEmpty(_modalKey)) return;
        Act(_ => LightRuntime.ApplyPreview(shown, true));
        CloseModal();
    }

    private void CancelWorldCopy()
    {
        _worldConfirm = false;
        _pendingWorldCopy = default;
        if (_copyCard != null) _copyCard.SetActive(false);
    }

    /// <summary>
    /// Runs a captured lamp action through the policy wording. An unclassified failure reads
    /// "That change was not applied"; unchanged or zero-change results cannot claim success.
    /// </summary>
    private void Act(Func<string, LightRuntime.LampActionResult> action) => ActOn(_modalSession, _modalKey, action);

    private void ActOn(Guid session, string key, Func<string, LightRuntime.LampActionResult> action)
    {
        if (_destroyed || !IsOpen()) return;
        var gate = UiSelectionPolicy.ClassifyAction(session, key, LightRuntime.Ready, LightRuntime.SessionId,
            LightRuntime.IsKnownLamp(key), !string.IsNullOrEmpty(key) && UiSelectionPolicy.SelectionVisible(key, _keys));
        if (gate != UiSelectionPolicy.ActionGate.Allowed)
        {
            PostAction(gate switch
            {
                UiSelectionPolicy.ActionGate.NoKey => "Pick an available lamp first.",
                UiSelectionPolicy.ActionGate.WrongSession => SessionChangedMessage,
                UiSelectionPolicy.ActionGate.NotReady => SessionChangedMessage,
                _ => UnavailableMessage
            });
            return;
        }
        LightRuntime.LampActionResult result;
        try { result = action(key); }
        catch (Exception ex) { Mod.Log.Warn("Phone action failed", ex); result = LightRuntime.LampActionResult.Rejected; }
        PostAction(UiSelectionPolicy.ActionMessage(result.Status, result.AppliedCount));
        _seenRevision = -1;
        Refresh();
    }

    private void PostAction(string message)
    {
        _actionPosted = LightRuntime.ClockSeconds;
        if (_action == null || _toast == null) return;
        _action.text = message;
        _toast.SetActive(true);
        _toast.transform.SetAsLastSibling();
    }

    private void Update()
    {
        if (_destroyed || _status == null) return;
        try
        {
            bool open = IsOpen();
            if (_root != null && _root.activeSelf != open) _root.SetActive(open);
            if (!open)
            {
                if (_modal != ModalKind.None) CloseModal();
                return;
            }
            Refresh();
            MeasureLayout();
            UpdateColumnHeading();
            MeasureModalGeometry();
        }
        catch (Exception ex)
        {
            if (_updateErrorLogged) return;
            _updateErrorLogged = true;
            Mod.Log.Warn("Phone UI update failed", ex);
        }
    }

    protected override void OnPhoneClosed()
    {
        base.OnPhoneClosed();
        CloseModal();
    }

    /// <summary>Only an actual container resize changes the owned reference mapping.</summary>
    private void MeasureLayout()
    {
        var root = _root != null ? _root.GetComponent<RectTransform>() : null;
        if (root == null || _referenceCanvas == null) return;
        float width = root.rect.width;
        float height = root.rect.height;
        float scale = UiSelectionPolicy.ReferenceCanvasScale(width, height);
        if (scale <= 0f) return;
        bool resized = Mathf.Abs(width - _lastLayoutWidth) >= 0.5f || Mathf.Abs(height - _lastLayoutHeight) >= 0.5f;
        if (resized)
        {
            _lastLayoutWidth = width;
            _lastLayoutHeight = height;
            _referenceCanvas.localScale = new Vector3(scale, scale, 1f);
            PinListWidth();
        }
        if (!_layoutLogged && _root!.activeInHierarchy)
        {
            _layoutLogged = true;
            Mod.Log.Info($"[ui-layout] root={width:F0}x{height:F0}, reference={_referenceCanvas.rect.width:F0}x{_referenceCanvas.rect.height:F0}, scale={scale:F3}, initialTheme={_initialThemeScale:F3}. Native appearance remains unverified.");
        }
    }

    /// <summary>One actual card/footer breadcrumb per open; no per-frame forced canvas work.</summary>
    private void MeasureModalGeometry()
    {
        if (!_modalGeometryPending || _modal == ModalKind.None) return;
        GameObject? card = _modal switch
        {
            ModalKind.Color => _colorCard,
            ModalKind.Brightness => _brightCard,
            ModalKind.WorldCopy => _copyCard,
            _ => _actionsCard
        };
        if (card == null) return;
        var footer = _footers.Find(item => ReferenceEquals(item.Card, card));
        var cardRect = card.GetComponent<RectTransform>();
        if (footer == null || cardRect == null) return;
        LayoutRebuilder.ForceRebuildLayoutImmediate(cardRect);
        var backRect = footer.BackButton.GetComponent<RectTransform>();
        var selectRect = footer.SelectButton != null ? footer.SelectButton.GetComponent<RectTransform>() : null;
        bool backInside = IsInsideCard(cardRect, backRect);
        bool selectInside = selectRect == null || IsInsideCard(cardRect, selectRect);
        bool valid = backRect.rect.width > 1f && backRect.rect.height > 1f && backInside;
        if (selectRect != null) valid &= selectRect.rect.width > 1f && selectRect.rect.height > 1f && selectInside;
        string select = selectRect == null ? "none" : $"{selectRect.rect.width:F0}x{selectRect.rect.height:F0},inside={selectInside}";
        string message = $"[ui-layout] modal={_modal}, card={cardRect.rect.width:F0}x{cardRect.rect.height:F0}, Back={backRect.rect.width:F0}x{backRect.rect.height:F0},inside={backInside}, Select={select}";
        if (valid) Mod.Log.Info(message);
        else Mod.Log.Warn(message + "; required footer geometry is zero or outside the card.");
        _modalGeometryPending = false;
    }

    private static bool IsInsideCard(RectTransform card, RectTransform child)
    {
        var corners = new Vector3[4];
        child.GetWorldCorners(corners);
        Rect bounds = card.rect;
        foreach (var corner in corners)
        {
            Vector3 local = card.InverseTransformPoint(corner);
            if (!bounds.Contains(new Vector2(local.x, local.y))) return false;
        }
        return true;
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
            _seenCatalog = _seenRevision = -1;
            if (_modal != ModalKind.None) CloseModal();
        }
        long catalog = LightRuntime.CatalogRevision;
        long revision = LightRuntime.Revision;
        var notice = UiSelectionPolicy.ClassifySave(ready, LightRuntime.StorageBlocked, LightRuntime.LastSaveFailed,
            revision, LightRuntime.SavedRevision, ready && !LightRuntime.SaveConfirmed);
        string noticeText = UiSelectionPolicy.SaveNoticeText(notice);
        bool statusChanged = _seenScan != LightRuntime.ScanInProgress || _seenTruncated != LightRuntime.ScanTruncated ||
            _seenUnsupported != LightRuntime.UnsupportedLampCount || _seenStatusReason != LightRuntime.StatusMessage ||
            _seenConflicts != LightRuntime.MaterialConflictCount ||
            _seenRestores != LightRuntime.PendingLightRestoreCount + LightRuntime.RetainedSurfaceCount;
        if (statusChanged || catalog != _seenCatalog || revision != _seenRevision || noticeText != _seenNotice)
        {
            _status.text = UiSelectionPolicy.StatusLine(ready, LightRuntime.StatusMessage, ready ? noticeText : null,
                LightRuntime.ScanInProgress, LightRuntime.ScanTruncated, LightRuntime.LampCount, LightRuntime.UnsupportedLampCount,
                LightRuntime.MaterialConflictCount, LightRuntime.PendingLightRestoreCount + LightRuntime.RetainedSurfaceCount);
            _seenScan = LightRuntime.ScanInProgress;
            _seenTruncated = LightRuntime.ScanTruncated;
            _seenUnsupported = LightRuntime.UnsupportedLampCount;
            _seenStatusReason = LightRuntime.StatusMessage;
            _seenConflicts = LightRuntime.MaterialConflictCount;
            _seenRestores = LightRuntime.PendingLightRestoreCount + LightRuntime.RetainedSurfaceCount;
        }
        bool messageOn = UiSelectionPolicy.ActionMessageVisible(_actionPosted, LightRuntime.ClockSeconds);
        if (_toast != null && _toast.activeSelf != messageOn) _toast.SetActive(messageOn);
        if (!ready)
        {
            _seenNotice = noticeText;
            _selectionKey = null;
            _keys.Clear();
            ShowRows(0);
            ShowHeaders(0);
            if (_columnHeader != null) _columnHeader.SetActive(false);
            if (_columnHeaderSpacer != null) _columnHeaderSpacer.SetActive(false);
            _summary.text = "Waiting for a loaded save";
            _availability.text = "Lights unavailable";
            _empty.text = LightRuntime.StatusMessage;
            _empty.gameObject.SetActive(true);
            if (_modal != ModalKind.None) CloseModal();
            return;
        }
        // Preserve revision fast path and pools: no rebuilding or asset creation every frame.
        if (!statusChanged && catalog == _seenCatalog && revision == _seenRevision && noticeText == _seenNotice)
        {
            SyncModal(session);
            return;
        }
        _visible.Clear();
        _rowCodes.Clear();
        _rowNames.Clear();
        _keys.Clear();
        for (int i = 0; i < LightRuntime.LampCount; i++)
        {
            if (!LightRuntime.TryGetLamp(i, out var lamp)) continue;
            _visible.Add(lamp.Index);
            _rowCodes.Add(lamp.PropertyCode);
            _rowNames.Add(lamp.PropertyName);
            _keys.Add(lamp.Key);
        }
        bool known = !string.IsNullOrEmpty(_selectionKey) && LightRuntime.IsKnownLamp(_selectionKey!);
        bool removed = UiSelectionPolicy.IsPermanentlyRemoved(_selectionKey, known, LightRuntime.ScanInProgress);
        _selectionKey = UiSelectionPolicy.ResolveSelection(_selectionKey, _keys, removed);
        bool structure = catalog != _seenCatalog;
        _seenCatalog = catalog;
        _seenRevision = revision;
        _seenNotice = noticeText;
        _empty.text = UiSelectionPolicy.EmptyCatalogText(LightRuntime.ScanInProgress);
        _empty.gameObject.SetActive(_keys.Count == 0);
        if (structure)
        {
            UiSelectionPolicy.GroupByProperty(_visible, _rowCodes, _rowNames, _ordered, _groups);
            ShowRows(_ordered.Count);
            ShowHeaders(_groups.Count);
            for (int i = 0; i < _groups.Count; i++)
            {
                _headers[i].Name.text = _groups[i].Name;
                _headers[i].Count.text = LightCount(_groups[i].Count);
            }
            ArrangeGroups();
        }
        _summary.text = $"{(_groups.Count == 1 ? _groups[0].Name : $"{_groups.Count} properties")} · {LightCount(_keys.Count)}";
        // Readable catalog entries, not proof that every native light is currently loaded.
        _availability.text = LightRuntime.ScanInProgress ? $"Scanning · {_keys.Count} lights found"
            : LightRuntime.ScanTruncated ? $"{_keys.Count} lights listed · scan incomplete" : $"{_keys.Count} of {LightRuntime.LampCount} lights readable";
        for (int row = 0; row < _ordered.Count; row++)
        {
            var ui = _rows[row];
            if (!LightRuntime.TryGetLamp(_ordered[row], out var lamp))
            {
                ui.Key = string.Empty;
                ui.Root.SetActive(false);
                continue;
            }
            ui.Key = lamp.Key;
            ui.Session = session;
            if (!ui.Root.activeSelf) ui.Root.SetActive(true);
            ui.Name.text = lamp.Label;
            ui.Detail.text = $"{lamp.Kind} · {PropertyOf(lamp)}";
            ui.Color.text = UiSelectionPolicy.ColorName(lamp.Color);
            ui.Percent.text = UiSelectionPolicy.IntensityLabel(lamp.Intensity);
            PaintSwatch(ui.Swatch, lamp.Color, 4f);
            SetMeter(ui.Meter, UiSelectionPolicy.BrightnessFill(lamp.Intensity));
        }
        SyncModal(session);
    }

    private void SyncModal(Guid session)
    {
        if (_modal == ModalKind.None || _modalRoot == null || !_modalRoot.activeSelf) return;
        if (!LightRuntime.Ready || session != _modalSession ||
            (!LightRuntime.IsKnownLamp(_modalKey) && !LightRuntime.ScanInProgress))
        {
            CloseModal();
            return;
        }
        switch (_modal)
        {
            case ModalKind.Color: PaintPicker(); break;
            case ModalKind.Brightness: PaintBrightness(); break;
            default: PaintActions(); break;
        }
    }

    private LightRuntime.LampView FindView(string key)
    {
        for (int i = 0; i < LightRuntime.LampCount; i++)
            if (LightRuntime.TryGetLamp(i, out var lamp) && string.Equals(lamp.Key, key, StringComparison.Ordinal)) return lamp;
        return default;
    }

    private static string PropertyOf(LightRuntime.LampView view) =>
        string.IsNullOrWhiteSpace(view.PropertyName) ? view.PropertyCode : view.PropertyName;

    private static string LightCount(int count) => count == 1 ? "1 light" : $"{count} lights";

    private void ShowRows(int count)
    {
        while (_listContent != null && _rows.Count < count) _rows.Add(CreateRow(_listContent, _rows.Count));
        for (int i = 0; i < _rows.Count; i++) _rows[i].Root.SetActive(i < count);
    }

    private void ShowHeaders(int count)
    {
        while (_listContent != null && _headers.Count < count) _headers.Add(CreateHeader(_listContent, _headers.Count));
        for (int i = 0; i < _headers.Count; i++) _headers[i].Root.SetActive(i < count);
    }

    private void ArrangeGroups()
    {
        if (_listContent == null) return;
        _empty.transform.SetSiblingIndex(0);
        int sibling = 1;
        if (_columnHeader != null) _columnHeader.SetActive(_groups.Count > 0);
        if (_columnHeaderSpacer != null) _columnHeaderSpacer.SetActive(_groups.Count > 0);
        for (int group = 0; group < _groups.Count; group++)
        {
            _headers[group].Root.transform.SetSiblingIndex(sibling++);
            if (group == 0 && _columnHeaderSpacer != null) _columnHeaderSpacer.transform.SetSiblingIndex(sibling++);
            int end = _groups[group].Start + _groups[group].Count;
            for (int row = _groups[group].Start; row < end && row < _rows.Count; row++)
                _rows[row].Root.transform.SetSiblingIndex(sibling++);
        }
        var rect = _listContent.GetComponent<RectTransform>();
        if (rect != null) LayoutRebuilder.ForceRebuildLayoutImmediate(rect);
    }

    private void UpdateColumnHeading()
    {
        if (_columnHeader == null || !_columnHeader.activeSelf || _listContent == null) return;
        var content = _listContent.GetComponent<RectTransform>();
        if (content == null) return;
        float top = UiSelectionPolicy.ColumnHeadingTop(content.anchoredPosition.y);
        if (float.IsFinite(_lastHeadingTop) && Mathf.Abs(top - _lastHeadingTop) < 0.05f) return;
        _lastHeadingTop = top;
        TopBand(_columnHeader, top, 36f);
    }

    private PropertyHeader CreateHeader(Transform parent, int slot)
    {
        var go = Panel("Property" + slot, parent, Table);
        float gap = slot == 0 ? 0f : 20f;
        Prefer(go, 52f + gap);
        var band = Panel("Heading", go.transform, Heading);
        Stretch(band, 0f, gap, 0f, 0f);
        var name = Label("Name", string.Empty, band.transform, 18, 16f, 0f, 850f, 52f, bold: true);
        var count = Label("Count", string.Empty, band.transform, 15, 900f, 0f, 210f, 52f, Muted, TextAnchor.MiddleRight);
        return new PropertyHeader { Root = go, Name = name, Count = count };
    }

    private LampRow CreateRow(Transform parent, int slot)
    {
        var root = Panel($"Lamp{slot}", parent, Table);
        Prefer(root, 126f);
        Columns(root);
        var main = Panel("Main", root.transform, Color.clear);
        var mainLayout = main.AddComponent<LayoutElement>();
        mainLayout.minWidth = LightColumnMinWidth;
        mainLayout.preferredHeight = 100f;
        mainLayout.flexibleWidth = 1f;
        mainLayout.flexibleHeight = 0f;
        var symbol = Bordered("LampSymbol", main.transform, Control, Hex(0x353a42), 10f);
        At(symbol, 0f, 26f, 42f, 48f);
        DrawGlyph(symbol.transform, Glyph.Lamp, 9f, 12f, 24f, 24f, Hex(0xafb6c0));
        var labels = Panel("LampLabels", main.transform, Color.clear);
        Stretch(labels, 58f, 18f, 0f, 18f);
        labels.AddComponent<RectMask2D>();
        var name = Text("Name", string.Empty, labels.transform, 24, Ink, bold: true);
        TopBand(name.gameObject, 0f, 31f);
        var detail = Text("Detail", string.Empty, labels.transform, 15, Hex(0xa0a7b0));
        TopBand(detail.gameObject, 36f, 24f);
        var colorGo = Bordered("ColorButton", root.transform, Control, ControlBorder, 10f);
        SetButtonSize(colorGo, ColorColumnWidth, 54f);
        var swatchGo = Panel("Swatch", colorGo.transform, Color.clear, 4f);
        At(swatchGo, 14f, 16f, 22f, 22f);
        var color = Label("Color", string.Empty, colorGo.transform, 18, 48f, 0f, 140f, 54f, bold: true);
        DrawGlyph(colorGo.transform, Glyph.Chevron, 193f, 18.5f, 17f, 17f, Hex(0xb2b9c3));
        var brightColumn = Panel("BrightnessColumn", root.transform, Color.clear);
        SetButtonSize(brightColumn, BrightnessColumnWidth, 60f);
        var brightGo = Bordered("BrightButton", brightColumn.transform, Hex(0x24282e), ControlBorder, 10f);
        At(brightGo, 0f, 0f, 174f, 60f);
        var percent = Label("Percent", string.Empty, brightGo.transform, 18, 13f, 10f, 120f, 23f, bold: true);
        DrawGlyph(brightGo.transform, Glyph.Chevron, 144f, 13f, 17f, 17f, Hex(0xb2b9c3));
        var meter = Meter(brightGo.transform, "Level", 13f, 42f, 148f, 3f);
        var moreColumn = Panel("MoreColumn", root.transform, Color.clear);
        SetButtonSize(moreColumn, MoreColumnWidth, 52f);
        var moreGo = Panel("MoreButton", moreColumn.transform, Color.clear, 10f);
        At(moreGo, 6f, 0f, 52f, 52f);
        DrawGlyph(moreGo.transform, Glyph.More, 13.5f, 13.5f, 25f, 25f, Hex(0xb5bdc7));
        int captured = slot;
        Clickable(main, () => OnRow(captured));
        Clickable(colorGo, () => OpenColorPicker(captured));
        Clickable(brightGo, () => OpenBrightness(captured));
        Clickable(moreGo, () => OpenActions(captured));
        Separator(root.transform);
        return new LampRow
        {
            Root = root,
            Swatch = swatchGo.GetComponent<Image>(),
            Name = name,
            Detail = detail,
            Color = color,
            Percent = percent,
            Meter = meter
        };
    }

    private static void PinScrollWidth(RectTransform? content)
    {
        if (content == null) return;
        var fitter = content.GetComponent<ContentSizeFitter>();
        if (fitter != null) fitter.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
        content.anchorMin = new Vector2(0f, 1f);
        content.anchorMax = new Vector2(1f, 1f);
        content.pivot = new Vector2(0.5f, 1f);
        content.sizeDelta = new Vector2(0f, content.sizeDelta.y);
    }

    private void PinListWidth()
    {
        if (_listContent != null) PinScrollWidth(_listContent.GetComponent<RectTransform>());
    }

    private static GameObject Panel(string name, Transform parent, Color color, float radius = 0f)
    {
        var go = UIFactory.Panel(name, parent, color);
        var image = go.GetComponent<Image>();
        image.raycastTarget = false;
        if (radius > 0f)
        {
            image.sprite = UISprites.Rounded(radius, radius >= 15f ? 64 : 32);
            image.type = Image.Type.Sliced;
        }
        return go;
    }

    private static GameObject Bordered(string name, Transform parent, Color fill, Color border, float radius, float thickness = 1f)
    {
        var go = Panel(name, parent, border, radius);
        var surface = Panel("Surface", go.transform, fill, Mathf.Max(1f, radius - thickness));
        Stretch(surface, thickness, thickness, thickness, thickness);
        return go;
    }

    private Text Text(string name, string value, Transform parent, int size, Color color,
        TextAnchor alignment = TextAnchor.MiddleLeft, bool bold = false)
    {
        var text = UIFactory.Text(name, value, parent, size, alignment, bold ? FontStyle.Bold : FontStyle.Normal);
        var chosen = bold ? _semiboldFont : _regularFont;
        if (chosen != null)
        {
            text.font = chosen;
            text.fontStyle = FontStyle.Normal;
        }
        // UIFactory's borrowed Arial stays in place if the optional OS font cannot be created.
        text.color = color;
        text.raycastTarget = false;
        text.supportRichText = false;
        text.resizeTextForBestFit = false;
        // Respect the allocated width. Single-line rects truncate additional lines; larger
        // status/confirmation rects wrap. Never shrink font sizes to hide overflowing content.
        text.horizontalOverflow = HorizontalWrapMode.Wrap;
        text.verticalOverflow = VerticalWrapMode.Truncate;
        return text;
    }

    private Text Label(string name, string value, Transform parent, int size, float x, float y, float width, float height,
        Color? color = null, TextAnchor alignment = TextAnchor.MiddleLeft, bool bold = false)
    {
        var text = Text(name, value, parent, size, color ?? Ink, alignment, bold);
        At(text.gameObject, x, y, width, height);
        return text;
    }

    private static void At(GameObject go, float x, float y, float width, float height)
    {
        var rect = go.GetComponent<RectTransform>();
        rect.anchorMin = rect.anchorMax = new Vector2(0f, 1f);
        rect.pivot = new Vector2(0f, 1f);
        rect.anchoredPosition = new Vector2(x, -y);
        rect.sizeDelta = new Vector2(width, height);
    }

    private static void Stretch(GameObject go, float left = 0f, float top = 0f, float right = 0f, float bottom = 0f)
    {
        var rect = go.GetComponent<RectTransform>();
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = new Vector2(left, bottom);
        rect.offsetMax = new Vector2(-right, -top);
    }

    private static void TopBand(GameObject go, float top, float height)
    {
        var rect = go.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0f, 1f);
        rect.anchorMax = new Vector2(1f, 1f);
        rect.offsetMin = new Vector2(0f, -top - height);
        rect.offsetMax = new Vector2(0f, -top);
    }

    private static void Prefer(GameObject go, float height)
    {
        var layout = go.GetComponent<LayoutElement>() ?? go.AddComponent<LayoutElement>();
        layout.minHeight = layout.preferredHeight = height;
        layout.flexibleHeight = 0f;
        layout.flexibleWidth = 1f;
    }

    private static void SetButtonSize(GameObject go, float width, float height)
    {
        var layout = go.GetComponent<LayoutElement>() ?? go.AddComponent<LayoutElement>();
        layout.minWidth = layout.preferredWidth = width;
        layout.minHeight = layout.preferredHeight = height;
        layout.flexibleWidth = layout.flexibleHeight = 0f;
    }

    private static void Separator(Transform parent)
    {
        var line = Panel("Separator", parent, Line);
        line.AddComponent<LayoutElement>().ignoreLayout = true;
        var rect = line.GetComponent<RectTransform>();
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = new Vector2(1f, 0f);
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = new Vector2(0f, 1f);
    }

    private static RectTransform Meter(Transform parent, string name, float x, float y, float width, float height)
    {
        var track = Panel(name, parent, MeterTrack, height / 2f);
        At(track, x, y, width, height);
        var fill = Panel("Fill", track.transform, MeterFill, height / 2f);
        Stretch(fill);
        return fill.GetComponent<RectTransform>();
    }

    private static void SetMeter(RectTransform rect, float fraction)
    {
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = new Vector2(fraction, 1f);
        rect.offsetMin = rect.offsetMax = Vector2.zero;
    }

    private Button Clickable(GameObject go, Action handler)
    {
        var image = go.GetComponent<Image>();
        image.raycastTarget = true;
        var button = go.AddComponent<Button>();
        button.targetGraphic = image;
        button.transition = Selectable.Transition.None;
        Wire(button, handler);
        return button;
    }

    private void Wire(Button button, Action handler)
    {
        var unityEvent = button.onClick;
        var binding = new UiSelectionPolicy.ClickBinding(handler);
        _clickBindings.Add(binding);
        _wired.Add((unityEvent, binding.Callback));
        S1API.Utils.EventHelper.AddListener(binding.Callback, unityEvent);
    }

    private static void NoOpCardClick() { }

    private void LoadFonts()
    {
        // Installed wrapper evidence: string overload throws "Method unstripping failed";
        // Il2CppStringArray overload calls the native dynamic-font constructor.
        try
        {
            var installed = Font.GetOSInstalledFontNames();
            if (installed == null) return;
            bool regular = false, semibold = false;
            for (int i = 0; i < installed.Length; i++)
            {
                regular |= string.Equals(installed[i], "Segoe UI", StringComparison.OrdinalIgnoreCase);
                semibold |= string.Equals(installed[i], "Segoe UI Semibold", StringComparison.OrdinalIgnoreCase);
            }
            if (regular) _regularFont = CreateFont("Segoe UI");
            if (semibold) _semiboldFont = CreateFont("Segoe UI Semibold");
        }
        catch (Exception ex) { Mod.Log.Warn("Graphite font lookup unavailable; using the built-in Arial fallback", ex); }
    }

    private Font? CreateFont(string name)
    {
        try
        {
            var font = Font.CreateDynamicFontFromOSFont(new Il2CppStringArray(new[] { name, "Arial" }), 24);
            if (font == null) return null;
            _ownedAssets.Add(font);
            return font;
        }
        catch (Exception ex)
        {
            Mod.Log.Warn($"Could not create {name}; using built-in Arial", ex);
            return null;
        }
    }

    private static Color Hex(int value) => new(((value >> 16) & 255) / 255f, ((value >> 8) & 255) / 255f, (value & 255) / 255f);

    /// <summary>Exact runtime emitted colors, independent of the dark UI palette.</summary>
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
        _ => Muted
    };

    private static string ColorDetail(ColorMode color)
    {
        if (color == ColorMode.Rainbow) return "Color cycle";
        Color rgb = Swatch(color);
        return $"#{Mathf.RoundToInt(rgb.r * 255f):X2}{Mathf.RoundToInt(rgb.g * 255f):X2}{Mathf.RoundToInt(rgb.b * 255f):X2}";
    }

    private void PaintSwatch(Image image, ColorMode color, float radius)
    {
        if (color == ColorMode.Original)
        {
            image.sprite = GlyphSprite(Glyph.Original);
            image.type = Image.Type.Simple;
            image.color = Hex(0xc8cdd5);
        }
        else if (color == ColorMode.Rainbow)
        {
            image.sprite = RainbowSprite();
            image.type = Image.Type.Simple;
            image.color = Color.white;
        }
        else
        {
            image.sprite = UISprites.Rounded(radius);
            image.type = Image.Type.Sliced;
            image.color = Swatch(color);
        }
    }

    private Sprite RainbowSprite()
    {
        if (_rainbow != null) return _rainbow;
        const int size = 64;
        var pixels = new Color32[size * size];
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                int index = Mathf.Min(7, x * 8 / size);
                var color = Swatch((ColorMode)((int)ColorMode.Red + index));
                color.a = UISprites.RoundedRectAlpha(x + 0.5f, y + 0.5f, size, size, 7f);
                pixels[y * size + x] = color;
            }
        }
        _rainbow = CreateSprite("LightControlRainbow", pixels, size);
        return _rainbow;
    }

    private Sprite CreateSprite(string name, Color32[] pixels, int size)
    {
        var texture = new Texture2D(size, size, TextureFormat.RGBA32, false);
        _ownedAssets.Add(texture); // Track immediately: partial construction is still owned.
        texture.name = name + "Texture";
        texture.filterMode = FilterMode.Bilinear;
        texture.wrapMode = TextureWrapMode.Clamp;
        texture.SetPixels32(pixels);
        texture.Apply(false, true);
        var sprite = Sprite.Create(texture, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f), 100f);
        _ownedAssets.Add(sprite);
        sprite.name = name;
        return sprite;
    }

    private GameObject DrawGlyph(Transform parent, Glyph glyph, float x, float y, float width, float height, Color color)
    {
        var go = Panel(glyph.ToString(), parent, color);
        At(go, x, y, width, height);
        var image = go.GetComponent<Image>();
        image.sprite = GlyphSprite(glyph);
        image.preserveAspect = true;
        return go;
    }

    /// <summary>Small outlined raster glyphs, generated once per app instance, never per row/frame.</summary>
    private Sprite GlyphSprite(Glyph glyph)
    {
        if (_glyphs.TryGetValue(glyph, out var cached)) return cached;
        var lines = new List<(Vector2 A, Vector2 B)>();
        void Path(params float[] xy)
        {
            for (int i = 2; i < xy.Length; i += 2)
                lines.Add((new Vector2(xy[i - 2], xy[i - 1]), new Vector2(xy[i], xy[i + 1])));
        }
        void Arc(float x, float y, float radius, float start, float end)
        {
            const int parts = 32;
            for (int i = 0; i < parts; i++)
            {
                float a = (start + (end - start) * i / parts) * Mathf.Deg2Rad;
                float b = (start + (end - start) * (i + 1) / parts) * Mathf.Deg2Rad;
                lines.Add((new Vector2(x + Mathf.Cos(a) * radius, y + Mathf.Sin(a) * radius),
                    new Vector2(x + Mathf.Cos(b) * radius, y + Mathf.Sin(b) * radius)));
            }
        }
        switch (glyph)
        {
            case Glyph.Lamp:
                Path(12, 3, 12, 7); Path(7, 7, 17, 7, 21, 14, 3, 14, 7, 7);
                Path(7, 18, 6, 20); Path(12, 18, 12, 21); Path(17, 18, 18, 20);
                break;
            case Glyph.Chevron: Path(7, 10, 12, 15, 17, 10); break;
            case Glyph.More:
                Path(5, 12, 5, 12); Path(12, 12, 12, 12); Path(19, 12, 19, 12);
                break;
            case Glyph.Check: Path(5, 12, 9, 16, 19, 6); break;
            case Glyph.Original:
                Arc(12, 12, 3, 0, 360);
                for (int i = 0; i < 8; i++)
                {
                    float angle = i * Mathf.PI / 4f;
                    Path(12 + Mathf.Cos(angle) * 6, 12 + Mathf.Sin(angle) * 6,
                        12 + Mathf.Cos(angle) * 9, 12 + Mathf.Sin(angle) * 9);
                }
                break;
            case Glyph.Rainbow:
                foreach (float radius in new[] { 9f, 5f, 1f })
                {
                    Arc(12, 14, radius, 180, 360);
                    Path(12 - radius, 14, 12 - radius, 19);
                    Path(12 + radius, 14, 12 + radius, 19);
                }
                break;
            case Glyph.Reset:
                Path(4, 5, 4, 10, 9, 10); Arc(12, 12, 8, 205, 525);
                break;
            case Glyph.Copy:
                Path(8, 8, 20, 8, 20, 21, 8, 21, 8, 8);
                Path(15, 8, 15, 3, 3, 3, 3, 15, 8, 15);
                break;
            case Glyph.Properties:
                Path(3, 10, 10, 4, 17, 10, 17, 20, 3, 20, 3, 10);
                Path(7, 20, 7, 14, 13, 14, 13, 20); Path(15, 3, 21, 8, 21, 18);
                break;
        }
        const int size = 64;
        var pixels = new Color32[size * size];
        float stroke = glyph is Glyph.More or Glyph.Check ? 1.2f : 0.85f;
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                var p = new Vector2((x + 0.5f) * 24f / size, (size - y - 0.5f) * 24f / size);
                float distance = float.MaxValue;
                foreach (var (a, b) in lines)
                {
                    var delta = b - a;
                    float t = delta.sqrMagnitude > 0f ? Mathf.Clamp01(Vector2.Dot(p - a, delta) / delta.sqrMagnitude) : 0f;
                    distance = Mathf.Min(distance, (p - a - delta * t).magnitude);
                }
                byte alpha = (byte)Mathf.RoundToInt(Mathf.Clamp01((stroke - distance) * size / 24f + 0.5f) * 255f);
                pixels[y * size + x] = new Color32(255, 255, 255, alpha);
            }
        }
        var sprite = CreateSprite("LightControl" + glyph, pixels, size);
        _glyphs.Add(glyph, sprite);
        return sprite;
    }

    private Sprite GetIcon()
    {
        if (_icon != null) return _icon;
        const int size = 64;
        var pixels = new Color32[size * size];
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                float dx = x + 0.5f - size * 0.5f;
                float dy = y + 0.5f - size * 0.62f;
                float glass = Mathf.Clamp01(16.5f - Mathf.Sqrt(dx * dx + dy * dy));
                bool screw = y > size * 0.14f && y < size * 0.32f && Mathf.Abs(dx) < 9f;
                pixels[y * size + x] = new Color32(255, 255, 255, (byte)Mathf.RoundToInt((screw ? 1f : glass) * 255f));
            }
        }
        _icon = CreateSprite("LightControlIcon", pixels, size);
        return _icon;
    }

    protected override void OnDestroyed()
    {
        if (_destroyed)
        {
            base.OnDestroyed();
            return;
        }
        _destroyed = true;
        MelonEvents.OnUpdate.Unsubscribe(Update);
        try { base.OnDestroyed(); }
        finally { ReleaseView(); }
    }

    private void ReleaseView()
    {
        foreach (var binding in _clickBindings) binding.Release();
        _clickBindings.Clear();
        // The saved event/handler pair is removed even when the Button is already Unity fake-null.
        foreach (var (unityEvent, handler) in _wired)
        {
            try { S1API.Utils.EventHelper.RemoveListener(handler, unityEvent); }
            catch { /* Keep releasing the remaining exact subscriptions after native teardown. */ }
        }
        _wired.Clear();
        _colorPicker.Close();
        try
        {
            if (_root != null)
            {
                _root.SetActive(false);
                UnityEngine.Object.Destroy(_root);
            }
        }
        catch { /* The framework may have already destroyed the owned subtree. */ }
        _root = null;
        // Reverse creation order releases sprites before their textures. Borrowed UISprites and
        // UIFactory's Arial are never added here and must never be destroyed by this view.
        for (int i = _ownedAssets.Count - 1; i >= 0; i--)
        {
            try { if (_ownedAssets[i] != null) UnityEngine.Object.Destroy(_ownedAssets[i]); }
            catch { /* Partial construction/destruction cannot strand later owned assets. */ }
        }
        _ownedAssets.Clear();
        _glyphs.Clear();
        _regularFont = _semiboldFont = null;
        _rainbow = _icon = null;
        _rows.Clear();
        _headers.Clear();
        _tiles.Clear();
        _steps.Clear();
        _modalButtons.Clear();
        _footers.Clear();
    }

    private enum ModalKind { None, Color, Brightness, Actions, WorldCopy }
    private enum Glyph { Lamp, Chevron, More, Check, Original, Rainbow, Reset, Copy, Properties }

    private sealed class ModalCard
    {
        public GameObject Root = null!;
        public Text Target = null!;
    }

    private sealed class ModalFooter
    {
        public GameObject Card = null!;
        public Button BackButton = null!;
        public Button? SelectButton;
    }

    private sealed class ActionControl
    {
        public Text Detail = null!;
        public Button Button = null!;
        public GameObject Check = null!;
    }

    private sealed class ColorTile
    {
        public Image Ring = null!;
        public GameObject Check = null!;
        public ColorMode Mode;
    }

    private sealed class IntensityStep
    {
        public GameObject Root = null!;
        public Text Text = null!;
        public IntensityLevel Level;
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
        public Image Swatch = null!;
        public Text Name = null!;
        public Text Detail = null!;
        public Text Color = null!;
        public Text Percent = null!;
        public RectTransform Meter = null!;
        public string Key = string.Empty;
        public Guid Session;
    }
}
