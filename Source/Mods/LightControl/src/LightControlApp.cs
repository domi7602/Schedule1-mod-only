using System;
using System.Collections.Generic;
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
/// The LightControl phone app. Minimal and clean: dark <see cref="GamePalette"/> surfaces,
/// <see cref="UITheme"/> spacing, an explicit vertical orientation, one isolated full-anchor root
/// that is only visible while the app is open, and fixed persistent controls. No sliders, no list
/// rebuilds, no decorative animation, and no fake defaults when nothing is controllable.
/// </summary>
public sealed class LightControlApp : PhoneApp
{
    protected override string AppName => "LightControl";

    protected override string AppTitle => "Light Control";

    protected override string IconLabel => "Lights";

    protected override string IconFileName => string.Empty;

    protected override EOrientation Orientation => EOrientation.Vertical;

    protected override Sprite IconSprite => GetIcon();

    private readonly List<GameObject> _gated = new();
    private readonly List<(UnityEvent Event, Action Handler)> _wired = new();
    private GameObject? _root;
    private Text _status = null!, _lampLabel = null!, _colorValue = null!, _intensityValue = null!;
    private int _selection;
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
        UITheme.InitializeForTextApp(container.GetComponent<RectTransform>());

        // A single isolated, inactive root. Visibility is driven exclusively by IsOpen().
        _root = UIFactory.Panel("LightControl_Root", container.transform, GamePalette.Bg, fullAnchor: true);
        _root.SetActive(false);

        var title = UIFactory.Text("Title", "LIGHT CONTROL", _root.transform, UITheme.Sp(24), TextAnchor.MiddleLeft, FontStyle.Bold);
        title.color = GamePalette.TextPrimary;
        Place(title.gameObject, 0.06f, 0.915f, 0.94f, 0.985f);

        var subtitle = UIFactory.Text("Subtitle", "GROW LAMP LED SURFACE", _root.transform, UITheme.Sp(12), TextAnchor.MiddleLeft);
        subtitle.color = GamePalette.TextMuted;
        Place(subtitle.gameObject, 0.06f, 0.875f, 0.94f, 0.912f);

        _status = UIFactory.Text("Status", string.Empty, _root.transform, UITheme.Sp(13), TextAnchor.UpperLeft);
        _status.color = GamePalette.TextMuted;
        _status.verticalOverflow = VerticalWrapMode.Truncate;
        Place(_status.gameObject, 0.06f, 0.765f, 0.94f, 0.868f);

        _lampLabel = UIFactory.Text("LampLabel", string.Empty, _root.transform, UITheme.Sp(17), TextAnchor.MiddleLeft, FontStyle.Bold);
        _lampLabel.color = GamePalette.TextPrimary;
        Place(_lampLabel.gameObject, 0.06f, 0.695f, 0.94f, 0.755f);
        _gated.Add(_lampLabel.gameObject);

        var previous = MakeButton(_root.transform, "Previous", "\u25c0  Prev lamp", GamePalette.Card, GamePalette.TextPrimary, () => Step(-1));
        Place(previous.gameObject, 0.06f, 0.605f, 0.47f, 0.683f);
        var next = MakeButton(_root.transform, "Next", "Next lamp  \u25b6", GamePalette.Card, GamePalette.TextPrimary, () => Step(1));
        Place(next.gameObject, 0.53f, 0.605f, 0.94f, 0.683f);

        var colorHeading = UIFactory.Text("ColorHeading", "COLOR", _root.transform, UITheme.Sp(11), TextAnchor.MiddleLeft);
        colorHeading.color = GamePalette.TextMuted;
        Place(colorHeading.gameObject, 0.06f, 0.53f, 0.94f, 0.566f);
        _gated.Add(colorHeading.gameObject);

        _colorValue = UIFactory.Text("ColorValue", string.Empty, _root.transform, UITheme.Sp(18), TextAnchor.MiddleLeft, FontStyle.Bold);
        _colorValue.color = GamePalette.TextPrimary;
        Place(_colorValue.gameObject, 0.06f, 0.45f, 0.54f, 0.522f);
        _gated.Add(_colorValue.gameObject);
        var colorButton = MakeButton(_root.transform, "CycleColor", "Cycle color", GamePalette.Blue, Color.white, () => Act(() => LightRuntime.CycleColor(_selection)));
        Place(colorButton.gameObject, 0.58f, 0.45f, 0.94f, 0.522f);

        var intensityHeading = UIFactory.Text("IntensityHeading", "LAMP BRIGHTNESS", _root.transform, UITheme.Sp(11), TextAnchor.MiddleLeft);
        intensityHeading.color = GamePalette.TextMuted;
        Place(intensityHeading.gameObject, 0.06f, 0.375f, 0.94f, 0.411f);
        _gated.Add(intensityHeading.gameObject);

        _intensityValue = UIFactory.Text("IntensityValue", string.Empty, _root.transform, UITheme.Sp(18), TextAnchor.MiddleLeft, FontStyle.Bold);
        _intensityValue.color = GamePalette.TextPrimary;
        Place(_intensityValue.gameObject, 0.06f, 0.295f, 0.54f, 0.367f);
        _gated.Add(_intensityValue.gameObject);
        var intensityButton = MakeButton(_root.transform, "CycleIntensity", "Cycle brightness", GamePalette.Blue, Color.white, () => Act(() => LightRuntime.CycleIntensity(_selection)));
        Place(intensityButton.gameObject, 0.58f, 0.295f, 0.94f, 0.367f);

        var original = MakeButton(_root.transform, "Original", "Original", GamePalette.CardAlt, GamePalette.TextPrimary, () => Act(() => LightRuntime.ResetOriginal(_selection)));
        Place(original.gameObject, 0.06f, 0.195f, 0.47f, 0.273f);
        var applyAll = MakeButton(_root.transform, "ApplyAll", "Apply to all", GamePalette.Green, Color.white, () => Act(() => LightRuntime.ApplyToAll(_selection)));
        Place(applyAll.gameObject, 0.53f, 0.195f, 0.94f, 0.273f);

        var hint = UIFactory.Text("Hint",
            "Prev/Next picks a grow lamp. Color and brightness drive that lamp's own LED panel and the light it emits together, so both always show the same color.\nOriginal restores the lamp's own color and 100%. Apply to all copies this lamp onto every supported lamp.",
            _root.transform, UITheme.Sp(11), TextAnchor.UpperLeft);
        hint.color = GamePalette.TextDim;
        hint.verticalOverflow = VerticalWrapMode.Truncate;
        Place(hint.gameObject, 0.06f, 0.075f, 0.94f, 0.185f);

        var limitation = UIFactory.Text("Limitation", "LED panel and emitted light use the same color. A lit lamp now tints what it shines on; plant materials themselves are never modified.", _root.transform, UITheme.Sp(10), TextAnchor.UpperLeft);
        limitation.color = GamePalette.TextDim;
        limitation.verticalOverflow = VerticalWrapMode.Truncate;
        Place(limitation.gameObject, 0.06f, 0.028f, 0.94f, 0.070f);
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
        _gated.Clear();
        _root = null;
        _destroyed = true;
        base.OnDestroyed();
    }

    /// <summary>Polls only while the app is open; drives root visibility from IsOpen().</summary>
    private void Update()
    {
        if (_destroyed) return;
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
        int count = LightRuntime.LampCount;
        if (!ready || count == 0)
        {
            _status.text = ready
                ? "No supported grow lamps found.\nPlace a grow lamp, then reopen this app. A lamp whose light has not loaded yet is picked up automatically."
                : LightRuntime.StatusMessage;
            SetGated(false);
            _selection = 0;
            return;
        }

        if (!LightRuntime.TryGetLamp(_selection, out var view))
        {
            _status.text = "Grow lamp state is not available right now.";
            SetGated(false);
            return;
        }

        _selection = view.Index;
        // A lamp that is switched on without a readable native light is never driven panel-only, so the
        // gap is stated instead of being presented as working control.
        string unsupported = LightRuntime.UnsupportedLampCount > 0
            ? $"\n{LightRuntime.UnsupportedLampCount} grow lamp(s) are on without a readable light source and are left untouched."
            : string.Empty;
        _status.text = LightRuntime.StorageBlocked
            ? $"Host session active. {count} grow lamp(s). Lamp data could not be read; changes are not saved this session."
            : $"Host session active. {count} grow lamp(s) registered.\nLED panel and emitted light use the same color.{unsupported}";
        _lampLabel.text = view.Label;
        _colorValue.text = DescribeColor(view.Color);
        _intensityValue.text = DescribeIntensity(view.Intensity);
        SetGated(true);
    }

    private void Step(int delta)
    {
        int count = LightRuntime.LampCount;
        if (count <= 0) return;
        _selection = UiSelectionPolicy.WrapIndex(_selection + delta, count);
        Refresh();
    }

    /// <summary>
    /// Runs one lamp action and never presents a failed or storage-blocked action as a success.
    /// </summary>
    private void Act(Func<bool> action)
    {
        bool applied;
        try { applied = action(); }
        catch (Exception ex) { Mod.Log.Warn("Phone action failed", ex); applied = false; }
        if (!applied)
        {
            _status.text = LightRuntime.StorageBlocked
                ? "Lamp data could not be read; changes are not saved this session."
                : "That change was not applied. Pick a grow lamp and try again.";
            return;
        }
        Refresh();
    }

    private void SetGated(bool visible)
    {
        foreach (var target in _gated)
            if (target != null) target.SetActive(visible);
    }

    private Button MakeButton(Transform parent, string name, string label, Color fill, Color ink, Action handler)
    {
        var (go, button, text) = UIFactory.ButtonWithLabel(name + "Button", label, parent, fill, 240f, 72f);
        text.fontSize = UITheme.Sp(14);
        text.color = ink;
        // IL2CPP-safe subscription (S1API wraps the Action). The exact delegate instance and the
        // exact UnityEvent are captured at wiring time so OnDestroyed can release precisely this
        // handler/event pair without a Unity fake-null liveness gate on the button.
        var unityEvent = button.onClick;
        S1API.Utils.EventHelper.AddListener(handler, unityEvent);
        _wired.Add((unityEvent, handler));
        _gated.Add(go);
        return button;
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
}
