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

    private static readonly Color Background = new(0.075f, 0.09f, 0.12f, 1f);
    private static readonly Color Surface = new(0.13f, 0.16f, 0.21f, 1f);
    private static readonly Color Accent = new(0.25f, 0.8f, 0.9f, 1f);
    private readonly List<Action> _unwire = new();
    private readonly List<Selectable> _controls = new();
    private readonly Slider[] _sliders = new Slider[4];
    private readonly Text[] _values = new Text[4];
    private GameObject _mainBG = null!;
    private CanvasGroup _controlGate = null!;
    private Text _propertyLabel = null!, _roomLabel = null!, _status = null!, _toggleLabel = null!;
    private Image _swatch = null!;
    private Button _inheritButton = null!;
    private Button _propertyPrev = null!, _propertyNext = null!, _roomPrev = null!, _roomNext = null!;
    private Sprite? _icon;
    private Texture2D? _iconTexture;
    private string? _propertyId, _roomId;
    private LEDSettings _shown = new();
    private int _shownRevision = -1;
    private bool _suppress, _destroyed, _lastReady, _updateErrorLogged;
    private float _lastPoll;

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
        _mainBG = UIFactory.Panel("PropertyLEDs_MainBG", container.transform, Background, fullAnchor: true);
        _mainBG.SetActive(false);
        var header = Label("Title", "PROPERTY LEDs", _mainBG.transform, 25, FontStyle.Bold);
        Rect(header.rectTransform, 0.05f, 0.93f, 0.95f, 0.99f);

        var content = UIFactory.ScrollableVerticalList("LEDControls", _mainBG.transform, out var scroll);
        Rect(scroll.GetComponent<RectTransform>(), 0.04f, 0.13f, 0.96f, 0.92f);
        scroll.scrollSensitivity = UITheme.Dp(24f);
        content.offsetMin = Vector2.zero;
        content.offsetMax = Vector2.zero;
        var layout = content.GetComponent<VerticalLayoutGroup>();
        layout.childControlWidth = true;
        layout.childForceExpandWidth = true;
        layout.childControlHeight = true;
        layout.childForceExpandHeight = false;
        layout.spacing = UITheme.Dp(8f);
        layout.padding = new RectOffset((int)UITheme.Dp(8), (int)UITheme.Dp(8), (int)UITheme.Dp(8), (int)UITheme.Dp(8));
        _controlGate = content.gameObject.AddComponent<CanvasGroup>();

        Caption(content, "PROPERTY");
        BuildSelector(content, "Property", () => StepProperty(-1), () => StepProperty(1),
            out _propertyLabel, out _propertyPrev, out _propertyNext);
        Caption(content, "ROOM / SCOPE");
        BuildSelector(content, "Room", () => StepRoom(-1), () => StepRoom(1),
            out _roomLabel, out _roomPrev, out _roomNext);

        var swatch = UIFactory.Panel("ColorPreview", content, Color.white);
        Height(swatch, 34);
        _swatch = swatch.GetComponent<Image>();
        _swatch.raycastTarget = false;
        BuildSlider(content, 0, "Hue", 1f);
        BuildSlider(content, 1, "Saturation", 1f);
        BuildSlider(content, 2, "Color intensity", 1f);
        BuildSlider(content, 3, "Brightness", 5f);
        var actions = Row(content, "Presets", 40);
        AddButton(actions.transform, "White", "White", () => Edit(settings =>
        {
            // Preset changes the colour only; brightness and the on/off intent stay untouched, and
            // the edit itself switches the LEDs on so the result is visible immediately.
            var white = LEDSettings.White();
            settings.Hue = white.Hue;
            settings.Saturation = white.Saturation;
            settings.ColorIntensity = white.ColorIntensity;
            settings.EnableForEdit();
        }), out _);
        AddButton(actions.transform, "Enabled", "LEDs OFF", () => Edit(s => s.Enabled = !s.Enabled), out _toggleLabel);
        var inherit = Row(content, "Inheritance", 40);
        _inheritButton = AddButton(inherit.transform, "UseProperty", "Use property color", UsePropertyColor, out _);
        var hint = Label("Help", "Colour edits switch the LEDs on. Room edits create an override; property edits affect rooms that inherit. Selecting a scope never changes its settings.", content, 12);
        Height(hint.gameObject, 54);

        _status = Label("Status", "Waiting for a loaded save...", _mainBG.transform, 13);
        Rect(_status.rectTransform, 0.06f, 0.015f, 0.94f, 0.12f);
        _status.verticalOverflow = VerticalWrapMode.Truncate;
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
            bool ready = LEDRuntime.Ready && NetworkGuard.IsInMainScene;
            if (!ready && _lastReady) Invalidate();
            if (open && (opened || ready != _lastReady || _shownRevision != LEDRuntime.Revision || Time.unscaledTime - _lastPoll > 0.5f))
            {
                _lastPoll = Time.unscaledTime;
                RefreshView();
            }
            _lastReady = ready;
        }
        catch (Exception ex)
        {
            Invalidate();
            if (!_updateErrorLogged)
            {
                _updateErrorLogged = true;
                Mod.Log.Warn("LED phone refresh failed: " + ex.Message);
            }
        }
    }

    private void Invalidate()
    {
        _shownRevision = -1;
        _propertyId = _roomId = null;
        _lastReady = false;
        SetEnabled(false);
        if (NetworkGuard.IsAlive(_status)) _status.text = "Unavailable during loading / scene transition.";
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
                _roomId = null;
            }
            var property = SelectedProperty();
            SetEnabled(property != null);
            if (property == null)
            {
                _propertyLabel.text = ready ? "No owned properties" : "Save not ready";
                _roomLabel.text = "Property default";
                _status.text = ready ? "No live property anchor available." : "Unavailable during loading / scene transition.";
                _shownRevision = LEDRuntime.Revision;
                return;
            }
            _propertyLabel.text = $"{property.DisplayName}\n{propertyIndex + 1}/{properties.Count} - {(property.IsBusiness ? "Business" : "Property")}";
            var rooms = LEDRuntime.GetRooms(property.Id);
            RoomLayout? selectedRoom = null;
            int roomIndex = 0;
            for (int i = 0; i < rooms.Count; i++)
                if (rooms[i].RoomId == _roomId) { selectedRoom = rooms[i]; roomIndex = i + 1; break; }
            if (selectedRoom == null) _roomId = null;
            _roomLabel.text = selectedRoom == null ? $"Property default\n1/{rooms.Count + 1}" :
                $"{selectedRoom.DisplayName}\n{roomIndex + 1}/{rooms.Count + 1} - {(selectedRoom.Verified ? "Verified layout" : "UNVERIFIED layout")}";
            _propertyPrev.interactable = _propertyNext.interactable = properties.Count > 1;
            _roomPrev.interactable = _roomNext.interactable = rooms.Count > 0;
            _inheritButton.interactable = _roomId != null && LEDRuntime.HasOverride(property.Id, _roomId);
            _shown = LEDRuntime.GetSettings(property.Id, _roomId).Clone();
            _shown.Normalize();
            _sliders[0].value = _shown.Hue;
            _sliders[1].value = _shown.Saturation;
            _sliders[2].value = _shown.ColorIntensity;
            _sliders[3].value = _shown.Brightness;
            _values[0].text = F(_shown.Hue * 360f) + " deg";
            _values[1].text = F(_shown.Saturation * 100f) + "%";
            _values[2].text = F(_shown.ColorIntensity * 100f) + "%";
            _values[3].text = F(_shown.Brightness) + " / 5";
            var rgb = _shown.ToRgb();
            _swatch.color = new Color(rgb.R, rgb.G, rgb.B, 1f);
            _toggleLabel.text = _shown.Enabled ? "Turn LEDs OFF" : "Turn LEDs ON";
            string mode = _roomId == null ? "Property default" : LEDRuntime.HasOverride(property.Id, _roomId) ? "Room override" : "Inherits property color";
            int verified = 0;
            for (int i = 0; i < rooms.Count; i++) if (rooms[i].Verified) verified++;
            string geometry = selectedRoom != null ? selectedRoom.Verified ? "Measured layout available" : "Unverified: normal LEDs not placed" : $"{verified}/{rooms.Count} verified room layouts";
            _status.text = $"{mode} - {(_shown.Enabled ? "Enabled" : "Disabled")}\n{geometry}. Settings are handled by the runtime.";
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
            var value = LEDRuntime.GetSettings(_propertyId!, _roomId).Clone();
            change(value);
            value.Normalize();
            LEDRuntime.Apply(_propertyId!, _roomId, value);
            RefreshView();
        }
        catch (Exception ex) { _status.text = "Edit failed: " + ex.Message; }
    }

    private void UsePropertyColor()
    {
        if (!CanEdit() || _roomId == null) return;
        try { LEDRuntime.UsePropertyColor(_propertyId!, _roomId); RefreshView(); }
        catch (Exception ex) { _status.text = "Inheritance edit failed: " + ex.Message; }
    }

    private void StepProperty(int delta)
    {
        if (!CanEdit()) return;
        var properties = LEDRuntime.Properties;
        int index = 0;
        for (int i = 0; i < properties.Count; i++) if (properties[i].Id == _propertyId) { index = i; break; }
        _propertyId = properties[(index + delta + properties.Count) % properties.Count].Id;
        _roomId = null;
        RefreshView(); // No Apply or file writes on navigation.
    }

    private void StepRoom(int delta)
    {
        if (!CanEdit()) return;
        var rooms = LEDRuntime.GetRooms(_propertyId!);
        int index = 0;
        for (int i = 0; i < rooms.Count; i++) if (rooms[i].RoomId == _roomId) { index = i + 1; break; }
        index = (index + delta + rooms.Count + 1) % (rooms.Count + 1);
        _roomId = index == 0 ? null : rooms[index - 1].RoomId;
        RefreshView();
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
        base.OnDestroyed();
        if (NetworkGuard.IsAlive(_icon)) Object.Destroy(_icon);
        if (NetworkGuard.IsAlive(_iconTexture)) Object.Destroy(_iconTexture);
        _icon = null;
        _iconTexture = null;
    }

    private void BuildSelector(Transform parent, string name, Action previous, Action next,
        out Text label, out Button prev, out Button following)
    {
        var row = Row(parent, name + "Selector", 70);
        prev = AddButton(row.transform, name + "Prev", "<", previous, out _, 38);
        label = Label(name + "Label", "...", row.transform, 15, FontStyle.Bold);
        label.gameObject.AddComponent<LayoutElement>().flexibleWidth = 1;
        following = AddButton(row.transform, name + "Next", ">", next, out _, 38);
    }

    private void BuildSlider(Transform parent, int index, string caption, float max)
    {
        var heading = Row(parent, caption + "Heading", 22);
        var label = Label("Caption", caption, heading.transform, 14);
        label.gameObject.AddComponent<LayoutElement>().flexibleWidth = 1;
        _values[index] = Label("Value", "...", heading.transform, 14);
        _values[index].alignment = TextAnchor.MiddleRight;
        _values[index].gameObject.AddComponent<LayoutElement>().preferredWidth = UITheme.Dp(95);
        var root = UIFactory.Panel("Slider" + index, parent, Color.clear);
        Height(root, 32);
        var track = UIFactory.Panel("Track", root.transform, Surface);
        Rect(track.GetComponent<RectTransform>(), 0.04f, 0.35f, 0.96f, 0.65f);
        var fillArea = UIFactory.Panel("FillArea", root.transform, Color.clear);
        Rect(fillArea.GetComponent<RectTransform>(), 0.04f, 0.35f, 0.96f, 0.65f);
        fillArea.GetComponent<Image>().raycastTarget = false;
        var fill = UIFactory.Panel("Fill", fillArea.transform, Accent, fullAnchor: true);
        fill.GetComponent<Image>().raycastTarget = false;
        var handleArea = UIFactory.Panel("HandleArea", root.transform, Color.clear);
        Rect(handleArea.GetComponent<RectTransform>(), 0.04f, 0.1f, 0.96f, 0.9f);
        handleArea.GetComponent<Image>().raycastTarget = false;
        var handle = UIFactory.Panel("Handle", handleArea.transform, Color.white);
        var handleRect = handle.GetComponent<RectTransform>();
        handleRect.anchorMin = new Vector2(0, 0);
        handleRect.anchorMax = new Vector2(0, 1);
        handleRect.sizeDelta = new Vector2(UITheme.Dp(18), 0);
        handleRect.anchoredPosition = Vector2.zero;
        var slider = root.AddComponent<Slider>();
        slider.minValue = 0;
        slider.maxValue = max;
        slider.wholeNumbers = false;
        slider.direction = Slider.Direction.LeftToRight;
        slider.fillRect = fill.GetComponent<RectTransform>();
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
        var (root, button, text) = UIFactory.RoundedButtonWithLabel(name, caption, parent, Surface,
            UITheme.Dp(width), UITheme.Dp(40), UITheme.Sp(13), Color.white);
        var le = root.GetComponent<LayoutElement>();
        le.minWidth = UITheme.Dp(width);
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

    private static void Caption(Transform parent, string text) => Height(Label(text, text, parent, 11).gameObject, 18);
    private static Text Label(string name, string text, Transform parent, int size, FontStyle style = FontStyle.Normal)
    {
        var label = UIFactory.Text(name, text, parent, UITheme.Sp(size), TextAnchor.MiddleLeft, style);
        label.raycastTarget = false;
        label.supportRichText = false;
        return label;
    }

    private static void Height(GameObject go, float height)
    {
        var le = go.GetComponent<LayoutElement>() ?? go.AddComponent<LayoutElement>();
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
