using System;
using System.Collections.Generic;
using System.Globalization;
using MelonLoader;
using PropertyLEDs.Core;
using S1API.Lifecycle;
using S1API.PhoneApp;
using S1API.UI;
using S1API.Utils;
using S1Mods.Shared;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;
using UITheme = S1Mods.Shared.UITheme;

namespace PropertyLEDs;

/// <summary>View and explicit user edits only. Runtime owns settings, persistence and geometry.</summary>
public sealed class PropertyLEDsApp : PhoneApp
{
    protected override string AppName => "PropertyLEDs";
    protected override string AppTitle => "Property LEDs";
    protected override string IconLabel => "LEDs";
    protected override string IconFileName => string.Empty;
    protected override EOrientation Orientation => EOrientation.Vertical;
    protected override Sprite IconSprite => GetIcon();

    private static readonly Color Background = new(0.035f, 0.055f, 0.075f, 1f);
    private static readonly Color Surface = new(0.07f, 0.11f, 0.145f, 1f);
    private static readonly Color Accent = new(0.15f, 0.72f, 0.88f, 1f);
    private static readonly Color Border = new(0.18f, 0.28f, 0.36f, 1f);
    private readonly List<Sprite> _drawnSprites = new();
    private readonly List<Texture2D> _drawnTextures = new();
    private Image _powerTrack = null!;
    private RectTransform _powerKnob = null!;
    private Text _powerDetail = null!, _advancedLabel = null!;
    private readonly List<Action> _unwire = new();
    private readonly List<Selectable> _controls = new();
    private readonly Slider[] _sliders = new Slider[4];
    private readonly Text[] _values = new Text[4];
    private GameObject _mainBG = null!;
    private GameObject _advancedPanel = null!;
    private bool _advancedExpanded;
    private CanvasGroup _controlGate = null!;
    private Text _propertyLabel = null!, _status = null!, _toggleLabel = null!;
    private Text _propertyContext = null!;
    private Button _propertyPrev = null!, _propertyNext = null!;
    private Sprite? _icon;
    private Texture2D? _iconTexture;
    private Sprite? _hueSprite;
    private Texture2D? _hueTexture;
    private Sprite? _saturationSprite;
    private Texture2D? _saturationTexture;
    private float _saturationHue = -1;
    private string? _propertyId, _editError;
    private LEDSettings _shown = new();
    private int _shownRevision = -1;
    private bool _suppress, _destroyed, _lastReady, _updateErrorLogged;
    private float _lastPoll;
    private RectTransform _scrollViewport = null!, _scrollContent = null!;
    private float _layoutWidth = -1, _layoutHeight = -1, _layoutScale;
    private bool _layoutDirty = true;
    private VerticalLayoutGroup _contentLayout = null!;
    private readonly List<LayoutElement> _selectorSizes = new(), _presetSizes = new();
    private readonly LayoutElement[] _sliderSizes = new LayoutElement[4], _trackSizes = new LayoutElement[4];
    private LayoutElement _advancedSize = null!, _advancedButtonSize = null!,
        _advancedPanelSize = null!, _powerSize = null!, _actionSize = null!;

    protected override void OnCreated()
    {
        base.OnCreated();
        MelonEvents.OnUpdate.Unsubscribe(Update);
        MelonEvents.OnUpdate.Subscribe(Update);
        GameLifecycle.OnPreLoad -= Invalidate;
        GameLifecycle.OnPreLoad += Invalidate;
        GameLifecycle.OnPreSceneChange -= Invalidate;
        GameLifecycle.OnPreSceneChange += Invalidate;
    }

    protected override void OnCreatedUI(GameObject container)
    {
        UITheme.InitializeForTextApp(container.GetComponent<RectTransform>());
        _layoutScale = UITheme.Scale;
        _mainBG = UIFactory.Panel("PropertyLEDs_MainBG", container.transform, Background, fullAnchor: true);
        _mainBG.SetActive(false);
        var header = Label("Title", "PROPERTY LEDs", _mainBG.transform, AppLayout.FontTitle, FontStyle.Bold);
        Rect(header.rectTransform, 0.17f, 0.94f, 0.95f, 0.99f);
        header.fontSize = UITheme.Sp(AppLayout.FontTitle);
        var subtitle = Label("Subtitle", "CONTROL THE LED LIGHTING FOR YOUR PROPERTIES", _mainBG.transform, AppLayout.FontSubtitle);
        subtitle.color = new Color(0.65f, 0.73f, 0.80f, 1);
        Rect(subtitle.rectTransform, 0.13f, 0.915f, 0.97f, 0.943f);
        var bulb = DrawIcon("Bulb", _mainBG.transform, new Color(1f, 0.84f, 0.25f, 1));
        Rect(bulb.rectTransform, 0.045f, 0.94f, 0.13f, 0.99f);

        var content = UIFactory.ScrollableVerticalList("LEDControls", _mainBG.transform, out var scroll);
        Rect(scroll.GetComponent<RectTransform>(), 0.045f, 0.015f, 0.955f, 0.90f);
        scroll.scrollSensitivity = UITheme.Dp(24f);
        _scrollViewport = scroll.viewport;
        _scrollContent = content;
        content.offsetMin = Vector2.zero;
        content.offsetMax = Vector2.zero;
        var fitter = content.GetComponent<ContentSizeFitter>();
        fitter.verticalFit = ContentSizeFitter.FitMode.Unconstrained;
        var layout = content.GetComponent<VerticalLayoutGroup>();
        _contentLayout = layout;
        layout.childControlWidth = true;
        layout.childForceExpandWidth = true;
        layout.childControlHeight = true;
        layout.childForceExpandHeight = false;
        layout.spacing = UITheme.Dp(AppLayout.Gap);
        layout.padding = new RectOffset((int)UITheme.Dp(AppLayout.Padding), (int)UITheme.Dp(AppLayout.Padding), (int)UITheme.Dp(AppLayout.Padding), (int)UITheme.Dp(AppLayout.Padding));
        _controlGate = content.gameObject.AddComponent<CanvasGroup>();
        _status = Label("InlineState", "", content, AppLayout.FontInlineState);
        _status.color = new Color(0.80f, 0.85f, 0.90f, 1);
        Height(_status.gameObject, AppLayout.InlineStateHeight);
        _status.verticalOverflow = VerticalWrapMode.Truncate;
        _status.gameObject.SetActive(false);

        BuildSelector(content, "Property", "Property", () => StepProperty(-1), () => StepProperty(1),
            out _propertyLabel, out _propertyContext, out _propertyPrev, out _propertyNext);
        BuildSlider(content, 0, "Color", 1f);
        BuildSlider(content, 1, "Saturation", 1f);
        BuildSlider(content, 3, "Brightness", 5f);
        var advanced = Row(content, "AdvancedToggle", AppLayout.AdvancedHeight);
        var advancedButton = AddButton(advanced.transform, "Advanced", "Advanced", ToggleAdvanced, out _advancedLabel);
        _advancedLabel.text = "Advanced  +";
        _advancedSize = advanced.GetComponent<LayoutElement>();
        _advancedButtonSize = advancedButton.transform.parent.GetComponent<LayoutElement>();
        _advancedPanel = UIFactory.Panel("AdvancedPanel", content, Color.clear);
        _advancedPanel.GetComponent<Image>().raycastTarget = false;
        Height(_advancedPanel, AppLayout.SliderHeight);
        _advancedPanelSize = _advancedPanel.GetComponent<LayoutElement>();
        var advancedLayout = _advancedPanel.AddComponent<VerticalLayoutGroup>();
        advancedLayout.childControlWidth = advancedLayout.childControlHeight = true;
        advancedLayout.childForceExpandWidth = true;
        advancedLayout.childForceExpandHeight = false;
        advancedLayout.spacing = 0;
        BuildSlider(_advancedPanel.transform, 2, "Color intensity", 1f);
        _advancedPanel.SetActive(false);
        BuildPowerCard(content);
        var actions = Row(content, "Presets", AppLayout.ActionHeight);
        _actionSize = actions.GetComponent<LayoutElement>();
        AddButton(actions.transform, "White", "White", () => Edit(settings =>
        {
            // Preset changes the colour only; brightness and the on/off intent stay untouched, and
            // the edit itself switches the LEDs on so the result is visible immediately.
            var white = LEDSettings.White();
            settings.Hue = white.Hue;
            settings.Saturation = white.Saturation;
            settings.ColorIntensity = white.ColorIntensity;
            settings.EnableForEdit();
        }), out var whiteLabel);
        ButtonIcon(whiteLabel, "Sun");
        _presetSizes.Add(whiteLabel.transform.parent.parent.GetComponent<LayoutElement>());

        RefreshView();
    }

    private void Update()
    {
        if (_destroyed || !NetworkGuard.IsAlive(_mainBG)) return;
        try
        {
            bool open = IsOpen();
            bool opened = open && !_mainBG.activeSelf;
            if (_mainBG.activeSelf != open) _mainBG.SetActive(open);
            if (opened) SetAdvancedExpanded(false);
            bool ready = LEDRuntime.Ready && NetworkGuard.IsInMainScene;
            if (!ready && _lastReady) Invalidate();
            if (open && (opened || ready != _lastReady || _shownRevision != LEDRuntime.Revision || Time.unscaledTime - _lastPoll > 0.5f))
            {
                _lastPoll = Time.unscaledTime;
                RefreshView();
            }
            if (open) LogMeasuredLayout(opened);
            _lastReady = ready;
        }
        catch (Exception ex)
        {
            Invalidate();
            SetInlineState("LED phone refresh failed: " + ex.Message);
            if (!_updateErrorLogged)
            {
                _updateErrorLogged = true;
                Mod.Log.Warn("LED phone refresh failed: " + ex.Message);
            }
        }
    }

    private void ToggleAdvanced()
    {
        if (!CanEdit()) return;
        SetAdvancedExpanded(!_advancedExpanded);
    }

    private void SetAdvancedExpanded(bool expanded)
    {
        if (!NetworkGuard.IsAlive(_advancedPanel)) return;
        if (_advancedExpanded == expanded) return;
        _advancedExpanded = expanded;
        _advancedPanel.SetActive(expanded);
        _advancedLabel.text = expanded ? "Advanced  -" : "Advanced  +";
        _layoutDirty = true;
        LogMeasuredLayout(false);
    }

    private void LogMeasuredLayout(bool opened)
    {
        if (!NetworkGuard.IsAlive(_scrollViewport) || !NetworkGuard.IsAlive(_scrollContent)) return;
        // Cheap cached-rect read: stale first-open measurements converge without repeated rebuilds.
        float width = _scrollViewport.rect.width, height = _scrollViewport.rect.height;
        if (!AppLayout.ShouldApply(width, height, _layoutWidth, _layoutHeight, _layoutDirty)) return;
        var sizing = AppLayout.Measure(height, _layoutScale, _advancedExpanded, _status.gameObject.activeSelf);
        _contentLayout.spacing = sizing.Gap;
        for (int i = 0; i < _selectorSizes.Count; i++) ApplyHeight(_selectorSizes[i], sizing.Selector);
        for (int i = 0; i < _sliderSizes.Length; i++)
        {
            ApplyHeight(_sliderSizes[i], sizing.Slider);
            ApplyHeight(_trackSizes[i], sizing.Slider - AppLayout.SliderHeadingHeight * _layoutScale);
        }
        ApplyHeight(_advancedSize, sizing.Advanced);
        ApplyHeight(_advancedButtonSize, sizing.Advanced);
        ApplyHeight(_advancedPanelSize, sizing.Slider);
        ApplyHeight(_powerSize, sizing.Power);
        ApplyHeight(_actionSize, sizing.Action);
        for (int i = 0; i < _presetSizes.Count; i++) ApplyHeight(_presetSizes[i], sizing.Action);
        _scrollContent.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, sizing.Content);
        LayoutRebuilder.ForceRebuildLayoutImmediate(_scrollContent);
        _layoutWidth = width;
        _layoutHeight = height;
        _layoutDirty = false;
        float contentHeight = _scrollContent.rect.height;
        Mod.Log.Info($"Phone layout: viewport={F(width)}x{F(height)}, content={F(contentHeight)}, " +
            $"expected={F(sizing.Content)}, advanced={_advancedExpanded}, inline={_status.gameObject.activeSelf}, " +
            $"selector={F(sizing.Selector)}, preview={F(sizing.Preview)}, slider={F(sizing.Slider)}, " +
            $"power={F(sizing.Power)}, actions={F(sizing.Action)}, gap={F(sizing.Gap)}, " +
            $"overflow={F(Mathf.Max(0, contentHeight - height))}; native visual verification pending.");
    }

    private void SetInlineState(string message)
    {
        if (!NetworkGuard.IsAlive(_status)) return;
        bool visible = !string.IsNullOrEmpty(message);
        bool changed = _status.gameObject.activeSelf != visible;
        var tone = AppLayout.InlineColor(message);
        _status.color = new Color(tone.R, tone.G, tone.B, tone.A);
        _status.text = message;
        _status.gameObject.SetActive(visible);
        if (changed) _layoutDirty = true;
    }

    private void Invalidate()
    {
        _shownRevision = -1;
        _propertyId = null;
        _lastReady = false;
        SetEnabled(false);
        SetInlineState("Unavailable during loading / scene transition.");
    }

    private PropertyEntry? SelectedProperty()
    {
        if (!LEDRuntime.Ready || !NetworkGuard.IsInMainScene) return null;
        var properties = LEDRuntime.Properties;
        for (int i = 0; i < properties.Count; i++)
            if (properties[i].Id == _propertyId && NetworkGuard.IsAlive(properties[i].Anchor)) return properties[i];
        return null;
    }

    private void RefreshView()
    {
        if (!NetworkGuard.IsAlive(_status)) return;
        _suppress = true;
        try
        {
            var properties = LEDRuntime.Properties;
            bool ready = LEDRuntime.Ready && NetworkGuard.IsInMainScene;
            int propertyIndex = -1;
            if (ready)
                for (int i = 0; i < properties.Count; i++)
                    if (properties[i].Id == _propertyId) { propertyIndex = i; break; }
            if (ready && propertyIndex < 0 && properties.Count > 0)
            {
                propertyIndex = 0;
                _propertyId = properties[0].Id;
            }
            var property = SelectedProperty();
            SetEnabled(property != null);
            if (property == null)
            {
                _propertyLabel.text = ready ? "No owned properties" : "Save not ready";
                _propertyContext.text = "";
                SetInlineState(ready ? "No live property anchor available." : "Unavailable during loading / scene transition.");
                _shownRevision = LEDRuntime.Revision;
                return;
            }
            _propertyLabel.text = property.DisplayName;
            _propertyContext.text = $"{propertyIndex + 1}/{properties.Count} - {(property.IsBusiness ? "Business" : "Property")}";
            _propertyPrev.interactable = _propertyNext.interactable = properties.Count > 1;
            _shown = LEDRuntime.GetSettings(property.Id, null).Clone();
            _shown.Normalize();
            _sliders[0].value = _shown.Hue;
            _sliders[1].value = _shown.Saturation;
            _sliders[2].value = _shown.ColorIntensity;
            _sliders[3].value = _shown.Brightness;
            _values[0].text = "";
            _values[1].text = F(_shown.Saturation * 100f) + "%";
            _values[2].text = F(_shown.ColorIntensity * 100f) + "%";
            _values[3].text = F(_shown.Brightness) + " / 5";
            UpdateSaturationTrack(_shown.Hue);
            _toggleLabel.text = _shown.Enabled ? "LEDs enabled" : "LEDs disabled";
            _powerDetail.text = _shown.Enabled ? "Property LEDs are currently on" : "Property LEDs are currently off";
            _powerTrack.color = _shown.Enabled ? Accent : Surface;
            _powerKnob.anchorMin = _powerKnob.anchorMax = new Vector2(_shown.Enabled ? 0.75f : 0.25f, 0.5f);
            SetInlineState(_editError ?? "");
            _shownRevision = LEDRuntime.Revision;
        }
        finally { _suppress = false; }
    }

    private bool CanEdit()
    {
        if (_suppress || _destroyed || !NetworkGuard.IsAlive(_mainBG) || !_mainBG.activeInHierarchy || !IsOpen()) return false;
        if (SelectedProperty() == null || _shownRevision != LEDRuntime.Revision) { RefreshView(); return false; }
        return true;
    }

    private void Edit(Action<LEDSettings> change)
    {
        if (!CanEdit()) return;
        try
        {
            var value = LEDRuntime.GetSettings(_propertyId!, null).Clone();
            change(value);
            value.Normalize();
            LEDRuntime.Apply(_propertyId!, null, value);
            _editError = null;
            RefreshView();
        }
        catch (Exception ex) { _editError = "Edit failed: " + ex.Message; SetInlineState("Edit failed: " + ex.Message); }
    }

    private void StepProperty(int delta)
    {
        if (!CanEdit()) return;
        var properties = LEDRuntime.Properties;
        int index = 0;
        for (int i = 0; i < properties.Count; i++) if (properties[i].Id == _propertyId) { index = i; break; }
        _propertyId = properties[(index + delta + properties.Count) % properties.Count].Id;
        RefreshView(); // No Apply or file writes on navigation.
    }

    private void SetEnabled(bool enabled)
    {
        if (NetworkGuard.IsAlive(_controlGate))
        {
            _controlGate.interactable = enabled;
            _controlGate.blocksRaycasts = enabled;
            _controlGate.alpha = enabled ? 1f : 0.45f;
        }
        for (int i = 0; i < _controls.Count; i++)
            if (NetworkGuard.IsAlive(_controls[i])) _controls[i].interactable = enabled;
    }

    protected override void OnPhoneClosed()
    {
        base.OnPhoneClosed();
        if (NetworkGuard.IsAlive(_mainBG)) _mainBG.SetActive(false);
        // No unsubscription or destruction here: S1API creates this app once per scene.
    }

    protected override void OnDestroyed()
    {
        if (_destroyed) return;
        _destroyed = true;
        MelonEvents.OnUpdate.Unsubscribe(Update);
        GameLifecycle.OnPreLoad -= Invalidate;
        GameLifecycle.OnPreSceneChange -= Invalidate;
        for (int i = 0; i < _unwire.Count; i++)
            try { _unwire[i](); } catch (Exception ex) { Mod.Log.Warn("LED UI listener cleanup: " + ex.Message); }
        _unwire.Clear();
        _controls.Clear();
        for (int i = 0; i < _drawnSprites.Count; i++)
            if (NetworkGuard.IsAlive(_drawnSprites[i])) Object.Destroy(_drawnSprites[i]);
        for (int i = 0; i < _drawnTextures.Count; i++)
            if (NetworkGuard.IsAlive(_drawnTextures[i])) Object.Destroy(_drawnTextures[i]);
        _drawnSprites.Clear();
        _drawnTextures.Clear();
        base.OnDestroyed();
        if (NetworkGuard.IsAlive(_icon)) Object.Destroy(_icon);
        if (NetworkGuard.IsAlive(_iconTexture)) Object.Destroy(_iconTexture);
        if (NetworkGuard.IsAlive(_hueSprite)) Object.Destroy(_hueSprite);
        if (NetworkGuard.IsAlive(_hueTexture)) Object.Destroy(_hueTexture);
        if (NetworkGuard.IsAlive(_saturationSprite)) Object.Destroy(_saturationSprite);
        if (NetworkGuard.IsAlive(_saturationTexture)) Object.Destroy(_saturationTexture);
        _icon = null;
        _iconTexture = null;
        _hueSprite = null;
        _hueTexture = null;
        _saturationSprite = null;
        _saturationTexture = null;
    }

    private void BuildSelector(Transform parent, string name, string caption, Action previous, Action next,
        out Text label, out Text context, out Button prev, out Button following)
    {
        var card = RoundedPanel(name + "Selector", parent, Surface);
        card.GetComponent<Image>().raycastTarget = false;
        Height(card, AppLayout.SelectorHeight);
        _selectorSizes.Add(card.GetComponent<LayoutElement>());
        var tile = RoundedPanel(name + "IconTile", card.transform, new Color(0.095f, 0.16f, 0.21f, 1), false);
        Rect(tile.GetComponent<RectTransform>(), 0, 0.12f, 0, 0.88f);
        tile.GetComponent<RectTransform>().offsetMin = new Vector2(UITheme.Dp(8), 0);
        tile.GetComponent<RectTransform>().offsetMax = new Vector2(UITheme.Dp(48), 0);
        var symbol = DrawIcon(name == "Property" ? "House" : "Layers", card.transform, Color.white);
        Rect(symbol.rectTransform, 0, 0.20f, 0, 0.80f);
        symbol.rectTransform.offsetMin = new Vector2(UITheme.Dp(12), 0);
        symbol.rectTransform.offsetMax = new Vector2(UITheme.Dp(42), 0);
        prev = AddButton(card.transform, name + "Prev", "", previous, out _, 24);
        var back = DrawIcon("Back", prev.transform, Color.white);
        Rect(back.rectTransform, 0.22f, 0.3f, 0.78f, 0.7f);
        following = AddButton(card.transform, name + "Next", "", next, out _, 24);
        var forward = DrawIcon("Next", following.transform, Color.white);
        Rect(forward.rectTransform, 0.22f, 0.3f, 0.78f, 0.7f);
        // Anchored hit boxes do not negotiate width with long property/room names.
        var prevRect = prev.transform.parent.GetComponent<RectTransform>();
        Rect(prevRect, 1, 0, 1, 1);
        prevRect.offsetMin = new Vector2(-UITheme.Dp(60), UITheme.Dp(8));
        prevRect.offsetMax = new Vector2(-UITheme.Dp(34), -UITheme.Dp(8));
        var nextRect = following.transform.parent.GetComponent<RectTransform>();
        Rect(nextRect, 1, 0, 1, 1);
        nextRect.offsetMin = new Vector2(-UITheme.Dp(30), UITheme.Dp(8));
        nextRect.offsetMax = new Vector2(-UITheme.Dp(4), -UITheme.Dp(8));
        var heading = Label(name + "Caption", caption, card.transform, AppLayout.FontSelectorCaption, FontStyle.Bold);
        label = Label(name + "Label", "...", card.transform, AppLayout.FontSelectorLabel, FontStyle.Bold);
        context = Label(name + "Context", "", card.transform, AppLayout.FontSelectorContext);
        heading.color = new Color(0.67f, 0.75f, 0.81f, 1);
        heading.fontStyle = FontStyle.Normal;
        SelectorBand(heading, AppLayout.SelectorCaptionBandBottom, AppLayout.SelectorCaptionBandTop);
        SelectorBand(label, AppLayout.SelectorLabelBandBottom, AppLayout.SelectorLabelBandTop);
        SelectorBand(context, AppLayout.SelectorContextBandBottom, AppLayout.SelectorContextBandTop);
    }

    private static void SelectorBand(Text text, float bottom, float top)
    {
        Rect(text.rectTransform, 0, 0.5f, 1, 0.5f);
        text.rectTransform.offsetMin = new Vector2(UITheme.Dp(54), UITheme.Dp(bottom - AppLayout.SelectorHeight / 2));
        text.rectTransform.offsetMax = new Vector2(-UITheme.Dp(64), UITheme.Dp(top - AppLayout.SelectorHeight / 2));
        text.horizontalOverflow = HorizontalWrapMode.Wrap;
        text.verticalOverflow = VerticalWrapMode.Truncate;
        // The card height is the extra air; the AppLayout band constants keep caption, name and context stacked without overlap.
    }

    private void BuildSlider(Transform parent, int index, string caption, float max)
    {
        var block = UIFactory.Panel(caption + "Control", parent, new Color(0.06f, 0.09f, 0.12f, 0.45f));
        block.GetComponent<Image>().raycastTarget = false;
        Height(block, AppLayout.SliderHeight);
        _sliderSizes[index] = block.GetComponent<LayoutElement>();
        var rowIcon = DrawIcon(index == 0 ? "Palette" : index == 1 ? "Drop" : "Sun", block.transform, Color.white);
        Rect(rowIcon.rectTransform, 0, 0.23f, 0, 0.77f);
        rowIcon.rectTransform.offsetMin = new Vector2(0, 0);
        rowIcon.rectTransform.offsetMax = new Vector2(UITheme.Dp(28), 0);
        rowIcon.gameObject.AddComponent<LayoutElement>().ignoreLayout = true;
        var layout = block.AddComponent<VerticalLayoutGroup>();
        layout.padding = new RectOffset((int)UITheme.Dp(46), 0, 0, 0);
        layout.childControlWidth = layout.childControlHeight = true;
        layout.childForceExpandWidth = true;
        layout.childForceExpandHeight = false;
        layout.spacing = 0;
        var heading = Row(block.transform, caption + "Heading", AppLayout.SliderHeadingHeight);
        var label = Label("Caption", caption, heading.transform, AppLayout.FontSliderCaption);
        label.gameObject.AddComponent<LayoutElement>().flexibleWidth = 1;
        _values[index] = Label("Value", "...", heading.transform, AppLayout.FontSliderValue, FontStyle.Bold);
        _values[index].alignment = TextAnchor.MiddleRight;
        _values[index].gameObject.AddComponent<LayoutElement>().preferredWidth = UITheme.Dp(95);
        // Keep every value slot populated for refresh, but Color has no numeric hue caption.
        if (index == 0) _values[index].gameObject.SetActive(false);
        var root = UIFactory.Panel("Slider" + index, block.transform, Color.clear);
        Height(root, AppLayout.SliderTrackHeight);
        _trackSizes[index] = root.GetComponent<LayoutElement>();
        var track = UIFactory.Panel("Track", root.transform, Surface);
        Rect(track.GetComponent<RectTransform>(), 0, 0.38f, 0.84f, 0.70f);
        track.GetComponent<Image>().sprite = UISprites.Capsule();
        track.GetComponent<Image>().type = Image.Type.Sliced;
        if (index == 0)
        {
            var trackImage = track.GetComponent<Image>();
            trackImage.sprite = GetHueTrack();
            trackImage.color = Color.white;
            trackImage.type = Image.Type.Simple;
        }
        else if (index == 1)
        {
            var trackImage = track.GetComponent<Image>();
            trackImage.sprite = GetSaturationTrack();
            trackImage.color = Color.white;
            trackImage.type = Image.Type.Simple;
        }
        var fillArea = UIFactory.Panel("FillArea", root.transform, Color.clear);
        Rect(fillArea.GetComponent<RectTransform>(), 0, 0.38f, 0.84f, 0.70f);
        fillArea.GetComponent<Image>().raycastTarget = false;
        var fill = UIFactory.Panel("Fill", fillArea.transform, Accent, fullAnchor: true);
        fill.GetComponent<Image>().raycastTarget = false;
        fill.GetComponent<Image>().sprite = UISprites.Capsule();
        fill.GetComponent<Image>().type = Image.Type.Sliced;
        if (index < 2) fill.SetActive(false);
        var handleArea = UIFactory.Panel("HandleArea", root.transform, Color.clear);
        // Slider.UpdateVisuals drives cross-axis anchors to 0..1: constrain the parent,
        // then use zero height delta so the native handle stays square after value changes.
        var handleAreaRect = handleArea.GetComponent<RectTransform>();
        Rect(handleAreaRect, 0, 0.54f, 0.84f, 0.54f);
        handleAreaRect.sizeDelta = new Vector2(0, UITheme.Dp(23));
        handleArea.GetComponent<Image>().raycastTarget = false;
        var handle = UIFactory.Panel("Handle", handleArea.transform, Color.white);
        handle.GetComponent<Image>().sprite = UISprites.Circle();
        handle.GetComponent<Image>().type = Image.Type.Simple;
        var handleRect = handle.GetComponent<RectTransform>();
        handleRect.anchorMin = handleRect.anchorMax = new Vector2(0, 0.5f);
        handleRect.sizeDelta = new Vector2(UITheme.Dp(23), 0);
        handle.GetComponent<Image>().preserveAspect = true;
        handleRect.anchoredPosition = Vector2.zero;
        var slider = root.AddComponent<Slider>();
        slider.minValue = 0;
        slider.maxValue = max;
        slider.wholeNumbers = false;
        slider.direction = Slider.Direction.LeftToRight;
        if (index >= 2) slider.fillRect = fill.GetComponent<RectTransform>();
        slider.handleRect = handleRect;
        slider.targetGraphic = handle.GetComponent<Image>();
        Action<float> listener = value => Edit(s =>
        {
            switch (index)
            {
                case 0: s.Hue = value; break;
                case 1: s.Saturation = value; break;
                case 2: s.ColorIntensity = value; break;
                case 3: s.Brightness = value; break;
            }
            // A colour edit is an effective-settings change: it ends an explicit "off" so the new
            // value is visible at once (brightness 0 stays off, see LEDSettings.EnableForEdit).
            s.EnableForEdit();
        });
        var evt = slider.onValueChanged;
        EventHelper.RemoveListener(listener, evt);
        EventHelper.AddListener(listener, evt);
        _unwire.Add(() => EventHelper.RemoveListener(listener, evt));
        _sliders[index] = slider;
        _controls.Add(slider);
    }

    private Button AddButton(Transform parent, string name, string caption, Action click, out Text label, float width = 0)
    {
        var (root, button, text) = UIFactory.RoundedButtonWithLabel(name, caption, parent, name == "White" ? new Color(0.17f, 0.20f, 0.23f, 1) : Surface,
            UITheme.Dp(width), UITheme.Dp(AppLayout.ActionHeight), UITheme.Sp(AppLayout.FontButton), Color.white);
        root.GetComponent<Mask>().showMaskGraphic = false;
        var le = root.GetComponent<LayoutElement>();
        le.layoutPriority = 10;
        le.minWidth = le.preferredWidth = UITheme.Dp(width);
        le.minHeight = le.preferredHeight = UITheme.Dp(name == "Advanced" ? AppLayout.AdvancedHeight : AppLayout.ActionHeight);
        le.flexibleHeight = 0;
        var colors = button.colors;
        colors.normalColor = Color.white;
        colors.highlightedColor = new Color(0.85f, 0.94f, 1f, 1);
        colors.pressedColor = new Color(0.65f, 0.79f, 0.87f, 1);
        colors.disabledColor = new Color(0.65f, 0.71f, 0.77f, 1);
        button.colors = colors;
        le.flexibleWidth = width > 0 ? 0 : 1;
        label = text;
        text.raycastTarget = false;
        EventHelper.RemoveListener(click, button.onClick);
        EventHelper.AddListener(click, button.onClick);
        var evt = button.onClick;
        _unwire.Add(() => EventHelper.RemoveListener(click, evt));
        _controls.Add(button);
        return button;
    }

    private static GameObject RoundedPanel(string name, Transform parent, Color color, bool outlined = true)
    {
        var panel = UIFactory.Panel(name, parent, outlined ? Border : color);
        var image = panel.GetComponent<Image>();
        image.sprite = UISprites.Rounded(9);
        image.type = Image.Type.Sliced;
        image.raycastTarget = false;
        if (outlined)
        {
            var inner = UIFactory.Panel(name + "Surface", panel.transform, color, fullAnchor: true);
            var rt = inner.GetComponent<RectTransform>();
            rt.offsetMin = new Vector2(UITheme.Dp(1.5f), UITheme.Dp(1.5f));
            rt.offsetMax = -rt.offsetMin;
            var fill = inner.GetComponent<Image>();
            fill.sprite = UISprites.Rounded(8);
            fill.type = Image.Type.Sliced;
            fill.raycastTarget = false;
        }
        return panel;
    }

    private void BuildPowerCard(Transform parent)
    {
        var card = RoundedPanel("PowerState", parent, new Color(0.08f, 0.13f, 0.17f, 1f));
        Height(card, AppLayout.PowerHeight);
        _powerSize = card.GetComponent<LayoutElement>();
        var icon = DrawIcon("Power", card.transform, Accent);
        Rect(icon.rectTransform, 0, 0.2f, 0, 0.8f);
        icon.rectTransform.offsetMin = new Vector2(UITheme.Dp(12), 0);
        icon.rectTransform.offsetMax = new Vector2(UITheme.Dp(44), 0);
        _toggleLabel = Label("PowerLabel", "LEDs disabled", card.transform, AppLayout.FontPowerLabel, FontStyle.Bold);
        _powerDetail = Label("PowerDetail", "Property LEDs are currently off", card.transform, AppLayout.FontPowerDetail);
        Rect(_toggleLabel.rectTransform, 0, 0.48f, 1, 0.84f);
        Rect(_powerDetail.rectTransform, 0, 0.18f, 1, 0.48f);
        _toggleLabel.rectTransform.offsetMin = _powerDetail.rectTransform.offsetMin = new Vector2(UITheme.Dp(60), 0);
        _toggleLabel.rectTransform.offsetMax = _powerDetail.rectTransform.offsetMax = new Vector2(-UITheme.Dp(78), 0);
        var button = AddButton(card.transform, "PowerToggle", "", () => Edit(s => s.Enabled = !s.Enabled), out _, 60);
        var root = button.transform.parent.GetComponent<RectTransform>();
        Rect(root, 1, 0.5f, 1, 0.5f);
        root.offsetMin = new Vector2(-UITheme.Dp(72), -UITheme.Dp(16));
        root.offsetMax = new Vector2(-UITheme.Dp(12), UITheme.Dp(16));
        _powerTrack = button.GetComponent<Image>();
        _powerTrack.sprite = UISprites.Capsule();
        _powerTrack.type = Image.Type.Sliced;
        var knob = UIFactory.Panel("PowerKnob", button.transform, Color.white);
        knob.GetComponent<Image>().sprite = UISprites.Circle();
        knob.GetComponent<Image>().raycastTarget = false;
        _powerKnob = knob.GetComponent<RectTransform>();
        _powerKnob.anchorMin = _powerKnob.anchorMax = new Vector2(0.25f, 0.5f);
        _powerKnob.sizeDelta = new Vector2(UITheme.Dp(26), UITheme.Dp(26));
        _powerKnob.anchoredPosition = Vector2.zero;
    }

    private Image ButtonIcon(Text text, string kind)
    {
        var icon = DrawIcon(kind, text.transform.parent, Color.white);
        Rect(icon.rectTransform, 0.06f, 0.25f, 0.24f, 0.75f);
        Rect(text.rectTransform, 0.26f, 0, 0.97f, 1);
        text.alignment = TextAnchor.MiddleCenter;
        text.fontSize = UITheme.Sp(AppLayout.FontButton);
        return icon;
    }

    // White-alpha raster shapes avoid emoji/font fallback and stay crisp at phone scale.
    private Image DrawIcon(string kind, Transform parent, Color tint)
    {
        var texture = new Texture2D(48, 48, TextureFormat.RGBA32, false);
        texture.wrapMode = TextureWrapMode.Clamp;
        texture.filterMode = FilterMode.Bilinear;
        for (int y = 0; y < 48; y++)
            for (int x = 0; x < 48; x++)
            {
                int hits = 0;
                for (int sy = 0; sy < 3; sy++)
                    for (int sx = 0; sx < 3; sx++)
                        if (IconPixel(kind, (x + (sx + 0.5f) / 3f) / 2f, (y + (sy + 0.5f) / 3f) / 2f)) hits++;
                texture.SetPixel(x, y, new Color(1, 1, 1, hits / 9f));
            }
        texture.Apply();
        var sprite = Sprite.Create(texture, new Rect(0, 0, 48, 48), new Vector2(0.5f, 0.5f));
        sprite.name = "PropertyLEDs_" + kind;
        _drawnTextures.Add(texture);
        _drawnSprites.Add(sprite);
        var go = UIFactory.Panel(kind + "Icon", parent, tint);
        var image = go.GetComponent<Image>();
        image.sprite = sprite;
        image.preserveAspect = true;
        image.raycastTarget = false;
        return image;
    }

    private static bool Stroke(float x, float y, float ax, float ay, float bx, float by, float width = 1.8f)
    {
        float dx = bx - ax, dy = by - ay;
        float t = Mathf.Clamp01(((x - ax) * dx + (y - ay) * dy) / (dx * dx + dy * dy));
        return (x - ax - t * dx) * (x - ax - t * dx) + (y - ay - t * dy) * (y - ay - t * dy) <= width * width / 4;
    }

    private static bool IconPixel(string kind, float x, float y)
    {
        float radius = Mathf.Sqrt((x - 12) * (x - 12) + (y - 12) * (y - 12));
        switch (kind)
        {
            case "Back": return Stroke(x, y, 15, 5, 8, 12) || Stroke(x, y, 8, 12, 15, 19);
            case "Next": return Stroke(x, y, 9, 5, 16, 12) || Stroke(x, y, 16, 12, 9, 19);
            case "Power": return (Mathf.Abs(radius - 8) < 1.2f && (y < 16 || Mathf.Abs(x - 12) > 4)) || Stroke(x, y, 12, 12, 12, 22, 2.5f);
            case "House":
                return Stroke(x, y, 3, 12, 12, 21, 2.5f) || Stroke(x, y, 12, 21, 21, 12, 2.5f) ||
                (x > 6 && x < 18 && y > 4 && y < 14 && !(x > 10 && x < 14 && y < 10));
            case "Layers":
                return Mathf.Abs(y - (17 - Mathf.Abs(x - 12) * 0.55f)) < 1.7f && Mathf.Abs(x - 12) < 9 ||
                Stroke(x, y, 3, 10, 12, 5) || Stroke(x, y, 12, 5, 21, 10) || Stroke(x, y, 3, 14, 12, 9) || Stroke(x, y, 12, 9, 21, 14);
            case "Drop": return y > 3 && y < 21 && Mathf.Abs(x - 12) < (y < 10 ? Mathf.Sqrt(Mathf.Max(0, 36 - (y - 9) * (y - 9))) : (21 - y) * 0.55f);
            case "Bulb":
                return (x - 12) * (x - 12) + (y - 15) * (y - 15) < 49 ||
                (x > 9 && x < 15 && y > 3 && y < 10);
            case "Info": return (Mathf.Abs(radius - 9) < 1.4f) || Stroke(x, y, 12, 5, 12, 12, 2) || (x - 12) * (x - 12) + (y - 16) * (y - 16) < 1.5f;
            case "Palette":
                return radius < 9 && !((x - 15) * (x - 15) + (y - 6) * (y - 6) < 12) &&
                !((x - 7) * (x - 7) + (y - 13) * (y - 13) < 2.5f) && !((x - 11) * (x - 11) + (y - 18) * (y - 18) < 2.5f) &&
                !((x - 17) * (x - 17) + (y - 15) * (y - 15) < 2.5f);
            default:
                if (radius < 4.5f) return true;
                for (int i = 0; i < 8; i++)
                {
                    float a = i * Mathf.PI / 4;
                    if (Stroke(x, y, 12 + Mathf.Cos(a) * 7, 12 + Mathf.Sin(a) * 7, 12 + Mathf.Cos(a) * 10, 12 + Mathf.Sin(a) * 10)) return true;
                }
                return false;
        }
    }

    private static GameObject Row(Transform parent, string name, float height)
    {
        var row = UIFactory.Panel(name, parent, Color.clear);
        row.GetComponent<Image>().raycastTarget = false;
        Height(row, height);
        var layout = row.AddComponent<HorizontalLayoutGroup>();
        layout.childControlWidth = layout.childControlHeight = true;
        layout.childForceExpandWidth = false;
        layout.childForceExpandHeight = true;
        layout.spacing = UITheme.Dp(8);
        return row;
    }

    private static Text Label(string name, string text, Transform parent, int size, FontStyle style = FontStyle.Normal)
    {
        var label = UIFactory.Text(name, text, parent, UITheme.Sp(size), TextAnchor.MiddleLeft, style);
        label.raycastTarget = false;
        label.supportRichText = false;
        return label;
    }

    private static void ApplyHeight(LayoutElement element, float nativeHeight)
    {
        element.layoutPriority = 10;
        element.flexibleHeight = 0;
        element.minHeight = element.preferredHeight = nativeHeight;
    }

    private static void Height(GameObject go, float height)
    {
        var le = go.GetComponent<LayoutElement>() ?? go.AddComponent<LayoutElement>();
        le.layoutPriority = 10;
        le.flexibleHeight = 0;
        le.preferredHeight = le.minHeight = UITheme.Dp(height);
        go.GetComponent<RectTransform>().sizeDelta = new Vector2(0, UITheme.Dp(height));
    }

    private static void Rect(RectTransform rt, float x0, float y0, float x1, float y1)
    {
        rt.anchorMin = new Vector2(x0, y0);
        rt.anchorMax = new Vector2(x1, y1);
        rt.offsetMin = rt.offsetMax = Vector2.zero;
    }
    private static string F(float value) => value.ToString("0.#", CultureInfo.InvariantCulture);

    private static void SetGradientColumn(Texture2D texture, int x, Color color)
    {
        for (int y = 0; y < 16; y++)
        {
            color.a = UISprites.RoundedRectAlpha(x + 0.5f, y + 0.5f, 256, 16, 8);
            texture.SetPixel(x, y, color);
        }
    }

    private Sprite GetHueTrack()
    {
        if (NetworkGuard.IsAlive(_hueSprite)) return _hueSprite!;
        _hueTexture = new Texture2D(256, 16, TextureFormat.RGBA32, false);
        _hueTexture.name = "PropertyLEDs_HueTrackTexture";
        _hueTexture.wrapMode = TextureWrapMode.Clamp;
        _hueTexture.filterMode = FilterMode.Bilinear;
        // Inclusive endpoints match the unchanged normalized hue range: red at 0 and 1.
        for (int x = 0; x < 256; x++)
            SetGradientColumn(_hueTexture, x, Color.HSVToRGB(x / 255f, 1f, 1f));
        _hueTexture.Apply();
        _hueSprite = Sprite.Create(_hueTexture, new Rect(0, 0, 256, 16), new Vector2(0.5f, 0.5f));
        _hueSprite.name = "PropertyLEDs_HueTrack";
        return _hueSprite;
    }

    private Sprite GetSaturationTrack()
    {
        if (NetworkGuard.IsAlive(_saturationSprite)) return _saturationSprite!;
        _saturationTexture = new Texture2D(256, 16, TextureFormat.RGBA32, false);
        _saturationTexture.name = "PropertyLEDs_SaturationTrackTexture";
        _saturationTexture.wrapMode = TextureWrapMode.Clamp;
        _saturationTexture.filterMode = FilterMode.Bilinear;
        _saturationHue = -1;
        UpdateSaturationTrack(0f);
        _saturationSprite = Sprite.Create(_saturationTexture, new Rect(0, 0, 256, 16), new Vector2(0.5f, 0.5f));
        _saturationSprite.name = "PropertyLEDs_SaturationTrack";
        return _saturationSprite;
    }

    private void UpdateSaturationTrack(float hue)
    {
        if (!NetworkGuard.IsAlive(_saturationTexture) || _saturationHue == hue) return;
        for (int x = 0; x < 256; x++)
            SetGradientColumn(_saturationTexture!, x, Color.HSVToRGB(hue, x / 255f, 1f));
        _saturationTexture!.Apply();
        _saturationHue = hue;
    }

    private Sprite GetIcon()
    {
        if (NetworkGuard.IsAlive(_icon)) return _icon!;
        _iconTexture = new Texture2D(64, 64, TextureFormat.RGBA32, false);
        _iconTexture.name = "PropertyLEDs_IconTexture";
        for (int y = 0; y < 64; y++)
            for (int x = 0; x < 64; x++)
            {
                bool strip = x >= 11 && x <= 52 && y >= 11 && y <= 52 && (x <= 16 || x >= 47 || y <= 16 || y >= 47);
                _iconTexture.SetPixel(x, y, strip ? Color.HSVToRGB((x + y) / 128f, 0.8f, 1f) : Background);
            }
        _iconTexture.Apply();
        _icon = Sprite.Create(_iconTexture, new Rect(0, 0, 64, 64), new Vector2(0.5f, 0.5f));
        _icon.name = "PropertyLEDs_Icon";
        return _icon;
    }
}
