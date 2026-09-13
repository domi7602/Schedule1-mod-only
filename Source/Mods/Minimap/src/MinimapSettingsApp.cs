using System;
using System.IO;
using MelonLoader;
using S1API.PhoneApp;
using S1API.UI;
using S1API.Utils;
using S1Mods.Shared;
using UnityEngine;
using UnityEngine.UI;

namespace Minimap;

/// <summary>
/// M6 Settings PhoneApp: "Minimap Settings" — toggles per blip type + hex color fields
/// (per user decision 2026-09-12: NO sliders, NO presets).
///
/// Follows the PhoneApp Golden Rules (schedule1-phoneapp skill):
/// 1. Explicit Orientation (vertical).            2. Never Destroy UI on close — only SetActive(false).
/// 3. Isolated _mainBG panel, starts hidden.      4. UITheme Sp/Dp (Shared single source).
/// 5. InputFocus guard on the hex InputField.     6. EventHelper/defensive AddListener for IL2CPP buttons.
/// 7. Config persistence via the mod's existing SafeStorage-backed SaveConfig.
/// 10. Never unsubscribe MelonEvents.OnUpdate in OnPhoneClosed.
///
/// NOTE on color rows: each blip category has a hex InputField + Apply button. Colors are
/// stored in MinimapConfig per-category hex fields and applied live to the blip renderer.
/// </summary>
public sealed class MinimapSettingsApp : PhoneApp
{
    protected override string AppName => "MinimapSettings";
    protected override string AppTitle => "Minimap";
    protected override string IconLabel => "Map";
    protected override string IconFileName => string.Empty;
    protected override EOrientation Orientation => EOrientation.Vertical;

    private Sprite? _cachedIconSprite;

    /// <summary>
    /// Icon for the "Map" app. Uses the same pattern as PotScannerApp:
    /// optional <c>minimap_icon.png</c> next to Minimap.dll, otherwise a fully
    /// procedural mini-map glyph is generated (no shipped asset required).
    /// Returning a valid sprite here prevents S1API's "Icon file not found"
    /// fallback (empty IconFileName used to resolve to the Mods folder itself).
    /// </summary>
    protected override Sprite IconSprite
    {
        get
        {
            if (_cachedIconSprite != null) return _cachedIconSprite;
            _cachedIconSprite = BuildIconSprite();
            return _cachedIconSprite;
        }
    }

    private static Sprite BuildIconSprite()
    {
        // Optional external PNG — lets users drop in their own icon.
        try
        {
            string path = System.IO.Path.Combine(MelonLoader.Utils.MelonEnvironment.ModsDirectory, "minimap_icon.png");
            if (System.IO.File.Exists(path))
            {
                byte[] data = System.IO.File.ReadAllBytes(path);
                var tex = new Texture2D(2, 2, TextureFormat.RGBA32, false);
                if (UnityEngine.ImageConversion.LoadImage(tex, data))
                {
                    var s = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
                    s.name = "MinimapSettingsIcon";
                    return s;
                }
                UnityEngine.Object.Destroy(tex);
            }
        }
        catch { }

        // Procedural fallback: dark rounded tile + ring + emerald center blip.
        try
        {
            const int size = 96;
            const float corner = 18f;
            var tex = new Texture2D(size, size, TextureFormat.RGBA32, false);
            var clear = new Color(0f, 0f, 0f, 0f);
            var center = new Vector2((size - 1) * 0.5f, (size - 1) * 0.5f);
            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    bool inside = x >= corner && y >= corner && x < size - corner && y < size - corner;
                    if (!inside)
                    {
                        // Soft rounded-corner rejection (distance to nearest corner).
                        float nx = Mathf.Max(corner - x, 0f, x - (size - 1 - corner));
                        float ny = Mathf.Max(corner - y, 0f, y - (size - 1 - corner));
                        if (nx * nx + ny * ny > corner * corner)
                        {
                            tex.SetPixel(x, y, clear);
                            continue;
                        }
                    }
                    var c = new Color(0.07f, 0.09f, 0.13f, 1f);
                    float d = Vector2.Distance(new Vector2(x, y), center);
                    if (d <= 9f) c = new Color(0.42f, 0.88f, 0.46f, 1f);                     // player blip
                    else if (d <= 20f && Mathf.Abs(x - center.x) > 2f) c = new Color(0.24f, 0.48f, 0.64f, 1f); // compass ring
                    tex.SetPixel(x, y, c);
                }
            }
            tex.Apply();
            var sprite = Sprite.Create(tex, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f));
            sprite.name = "MinimapSettingsIconProcedural";
            return sprite;
        }
        catch { return null!; }
    }

    private GameObject _mainBG = null!;
    private RectTransform _scrollContent = null!;

    // Rows built once in OnCreatedUI; state re-synced from Config on every open.
    private readonly System.Collections.Generic.List<ToggleRow> _toggleRows = new();
    private readonly System.Collections.Generic.List<ColorRow> _colorRows = new();

    private MinimapMod Mod => MinimapMod.Instance ?? throw new InvalidOperationException("Minimap mod not running");

    // ── Color defaults per category (hex, mirrors MinimapBlips defaults) ──
    private static readonly (string Key, string Label, string DefaultHex)[] ColorDefs =
    {
        ("Police",     "Police",        "#338CF2"),
        ("Dealer",     "Dealer",        "#A44DE3"),
        ("Waypoint",   "Waypoint",      "#E14BFF"),
        ("Deal",       "Active Deal",   "#FAC40F"),
        ("Potential",  "Potential Cust","#1ABD9C"),
        ("Customer",   "Customer",      "#2ECC70"),
        ("Property",   "Property",      "#F29C12"),
        ("Shop",       "Shop",          "#E84D3D"),
        ("Quest",      "Quest",         "#E64C3C"),
        ("Vehicle",    "Vehicle",       "#3399DB"),
    };

    protected override void OnCreated()
    {
        base.OnCreated();
        MelonEvents.OnUpdate.Unsubscribe(Update);
        MelonEvents.OnUpdate.Subscribe(Update);
        MelonLogger.Msg("[MinimapSettings] Registered with S1API PhoneApp system.");
    }

    protected override void OnCreatedUI(GameObject container)
    {
        var containerRt = container.GetComponent<RectTransform>();
        if (containerRt != null)
            S1Mods.Shared.UITheme.InitializeForTextApp(containerRt);

        // 1. Isolated background panel (starts hidden — Rule 3)
        _mainBG = UIFactory.Panel("MinimapSettings_BG", container.transform, new Color(0.07f, 0.08f, 0.11f, 1f), fullAnchor: true);
        _mainBG.SetActive(false);

        var vlg = _mainBG.AddComponent<VerticalLayoutGroup>();
        vlg.childControlWidth = true;
        vlg.childControlHeight = true;
        vlg.childForceExpandWidth = true;
        vlg.childForceExpandHeight = false;

        // 2. Header
        var header = UIFactory.Panel("Header", _mainBG.transform, new Color(0.10f, 0.12f, 0.16f, 1f));
        var headerLe = header.AddComponent<LayoutElement>();
        headerLe.preferredHeight = UITheme.Dp(36f);
        headerLe.minHeight = UITheme.Dp(36f);
        var title = UIFactory.Text("Title", "MINIMAP SETTINGS", header.transform, UITheme.Sp(13), TextAnchor.MiddleCenter, FontStyle.Bold);
        title.color = new Color(0.59f, 0.85f, 0.51f, 1f); // emerald accent

        // 3. Scrollable content
        var scrollObj = UIFactory.Panel("ScrollZone", _mainBG.transform, new Color(0.05f, 0.06f, 0.09f, 1f));
        var scrollLe = scrollObj.AddComponent<LayoutElement>();
        scrollLe.flexibleHeight = 1f;

        var scroll = scrollObj.AddComponent<ScrollRect>();
        scroll.vertical = true;
        scroll.horizontal = false;
        scroll.scrollSensitivity = 24f;

        var viewport = UIFactory.Panel("Viewport", scrollObj.transform, Color.clear);
        var vpRt = viewport.GetComponent<RectTransform>();
        vpRt.anchorMin = Vector2.zero;
        vpRt.anchorMax = Vector2.one;
        vpRt.pivot = new Vector2(0.5f, 1f);
        viewport.AddComponent<RectMask2D>();
        var vpLe = viewport.AddComponent<LayoutElement>();
        vpLe.flexibleHeight = 1f;

        var content = UIFactory.Panel("Content", viewport.transform, Color.clear);
        _scrollContent = content.GetComponent<RectTransform>();
        _scrollContent.anchorMin = new Vector2(0f, 1f);
        _scrollContent.anchorMax = new Vector2(1f, 1f);
        _scrollContent.pivot = new Vector2(0.5f, 1f);
        var contentLe = content.AddComponent<LayoutElement>();
        contentLe.minHeight = UITheme.Dp(400f);

        var contentVlg = content.AddComponent<VerticalLayoutGroup>();
        contentVlg.childControlWidth = true;
        contentVlg.childControlHeight = true;
        contentVlg.childForceExpandWidth = true;
        contentVlg.childForceExpandHeight = false;
        contentVlg.padding = new RectOffset((int)UITheme.Dp(8f), (int)UITheme.Dp(8f), (int)UITheme.Dp(6f), (int)UITheme.Dp(20f));
        contentVlg.spacing = UITheme.Dp(4f);

        // Audit 2026-09-13 (M-05): without a ContentSizeFitter the content rect keeps its
        // anchor height (top-anchors => 0) — the VLG lays children into a zero-height strip,
        // nothing below the first rows is reachable and the Save&Apply footer is cut off.
        // PreferredSize feeds the VLG's measured height back into the rect (min 400 preserved
        // via LayoutElement.minHeight).
        var contentCsf = content.AddComponent<ContentSizeFitter>();
        contentCsf.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
        contentCsf.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;

        scroll.content = _scrollContent;
        scroll.viewport = vpRt;

        BuildToggleSection(content.transform);
        BuildColorSection(content.transform);
        BuildFooter(content.transform);
    }

    // ─────────────────────────────────────────────────────────
    // Section 1: Toggles (one per blip category / HUD element)
    // ─────────────────────────────────────────────────────────

    private void BuildToggleSection(Transform parent)
    {
        AddSectionLabel(parent, "BLIP FILTERS");

        AddToggle(parent, "Police", () => Get().ShowPoliceBlips, v => Get().ShowPoliceBlips = v);
        AddToggle(parent, "Dealers", () => Get().ShowDealerBlips, v => Get().ShowDealerBlips = v);
        AddToggle(parent, "Active Deals", () => Get().ShowActiveDeals, v => Get().ShowActiveDeals = v);
        AddToggle(parent, "Potential Cust", () => Get().ShowPotentialCustomers, v => Get().ShowPotentialCustomers = v);
        AddToggle(parent, "Customers", () => Get().ShowKnownCustomers, v => Get().ShowKnownCustomers = v);
        AddToggle(parent, "Properties", () => Get().ShowPropertyBlips, v => Get().ShowPropertyBlips = v);
        AddToggle(parent, "Shops", () => Get().ShowShopBlips, v => Get().ShowShopBlips = v);
        AddToggle(parent, "Quests", () => Get().ShowQuestBlips, v => Get().ShowQuestBlips = v);
        AddToggle(parent, "Vehicles", () => Get().ShowVehicleBlips, v => Get().ShowVehicleBlips = v);
        AddToggle(parent, "Waypoints", () => Get().ShowWaypoints, v => Get().ShowWaypoints = v);
        AddToggle(parent, "Health Bar", () => Get().ShowHealthBar, v => Get().ShowHealthBar = v);

        AddSectionLabel(parent, "HUD");
        AddToggle(parent, "Minimap Visible", () => Get().MinimapVisible, v => Get().MinimapVisible = v);
    }

    // ─────────────────────────────────────────────────────────
    // Section 2: Hex color fields
    // ─────────────────────────────────────────────────────────

    private void BuildColorSection(Transform parent)
    {
        AddSectionLabel(parent, "BLIP COLORS (HEX)");

        foreach (var def in ColorDefs)
        {
            AddColorRow(parent, def.Label, def.DefaultHex, def.Key);
        }
    }

    private void BuildFooter(Transform parent)
    {
        var footer = UIFactory.Panel("FooterActions", parent, Color.clear);
        var le = footer.AddComponent<LayoutElement>();
        le.preferredHeight = UITheme.Dp(40f);

        var hlg = footer.AddComponent<HorizontalLayoutGroup>();
        hlg.spacing = UITheme.Dp(8f);
        hlg.childAlignment = TextAnchor.MiddleCenter;
        hlg.childForceExpandWidth = true;
        hlg.childControlWidth = false;
        hlg.childControlHeight = false;

        var (saveMask, saveBtn, _) = UIFactory.RoundedButtonWithLabel("SaveBtn", "Save & Apply", footer.transform,
            new Color(0.16f, 0.42f, 0.24f, 1f), UITheme.Dp(120f), UITheme.Dp(30f), (int)UITheme.Sp(12), new Color(0.92f, 0.95f, 0.93f, 1f));
        var saveLe = saveMask.AddComponent<LayoutElement>();
        saveLe.preferredWidth = UITheme.Dp(120f);
        saveLe.preferredHeight = UITheme.Dp(30f);
        ButtonUtils.AddListener(saveBtn, () =>
        {
            try
            {
                Mod.SaveAndApply();
                Mod.SaveBlipPalette();
                ApplyColorsToBlips();
                MelonLogger.Msg("[MinimapSettings] Config saved & applied.");
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"[MinimapSettings] Save failed: {ex.Message}");
            }
        });

        var (resetMask, resetBtn, _) = UIFactory.RoundedButtonWithLabel("ResetColorsBtn", "Reset Colors", footer.transform,
            new Color(0.35f, 0.18f, 0.10f, 1f), UITheme.Dp(110f), UITheme.Dp(30f), (int)UITheme.Sp(12), new Color(0.98f, 0.90f, 0.85f, 1f));
        var resetLe = resetMask.AddComponent<LayoutElement>();
        resetLe.preferredWidth = UITheme.Dp(110f);
        resetLe.preferredHeight = UITheme.Dp(30f);
        ButtonUtils.AddListener(resetBtn, () =>
        {
            try
            {
                foreach (var cr in _colorRows) cr.ResetToDefault();
                ApplyColorsToBlips();
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"[MinimapSettings] Reset failed: {ex.Message}");
            }
        });
    }

    // ─────────────────────────────────────────────────────────
    // Row builders
    // ─────────────────────────────────────────────────────────

    private void AddSectionLabel(Transform parent, string text)
    {
        var lbl = UIFactory.Text($"Section_{text}", text, parent, UITheme.Sp(11), TextAnchor.MiddleLeft, FontStyle.Bold);
        lbl.color = new Color(0.55f, 0.60f, 0.70f, 1f);
        var le = lbl.gameObject.AddComponent<LayoutElement>();
        le.preferredHeight = UITheme.Dp(22f);
    }

    private void AddToggle(Transform parent, string label, Func<bool> get, Action<bool> set)
    {
        var row = UIFactory.Panel($"Toggle_{label}", parent, new Color(0.11f, 0.13f, 0.18f, 1f));
        var le = row.AddComponent<LayoutElement>();
        le.preferredHeight = UITheme.Dp(28f);

        var hlg = row.AddComponent<HorizontalLayoutGroup>();
        hlg.childAlignment = TextAnchor.MiddleLeft;
        hlg.childForceExpandWidth = true;
        hlg.childControlHeight = false;

        var text = UIFactory.Text("Label", label, row.transform, UITheme.Sp(11), TextAnchor.MiddleLeft, FontStyle.Normal);
        text.color = new Color(0.92f, 0.93f, 0.95f, 1f);
        var textLe = text.gameObject.AddComponent<LayoutElement>();
        textLe.flexibleWidth = 1f;

        var tg = row.AddComponent<Toggle>();
        var checkBg = UIFactory.Panel("CheckBg", row.transform, new Color(0.2f, 0.24f, 0.3f, 1f));
        var checkLe = checkBg.AddComponent<LayoutElement>();
        checkLe.preferredWidth = UITheme.Dp(22f);
        checkLe.preferredHeight = UITheme.Dp(22f);
        tg.graphic = checkBg.GetComponent<Image>();
        tg.isOn = get();

        var row2 = new ToggleRow(label, tg, get, set);
        _toggleRows.Add(row2);

        // Safe IL2CPP wiring (Rule 6): Toggle.onValueChanged is a UnityEvent<bool>.
        S1API.Utils.EventHelper.AddListener(_ => row2.Commit(), tg.onValueChanged);
    }

    private void AddColorRow(Transform parent, string label, string defaultHex, string key)
    {
        var row = UIFactory.Panel($"Color_{key}", parent, new Color(0.11f, 0.13f, 0.18f, 1f));
        var le = row.AddComponent<LayoutElement>();
        le.preferredHeight = UITheme.Dp(30f);

        var hlg = row.AddComponent<HorizontalLayoutGroup>();
        hlg.spacing = UITheme.Dp(6f);
        hlg.childAlignment = TextAnchor.MiddleLeft;
        hlg.childForceExpandWidth = false;
        hlg.childControlWidth = false;
        hlg.childControlHeight = false;
        hlg.padding = new RectOffset((int)UITheme.Dp(6f), 0, 0, 0);

        var text = UIFactory.Text("Label", label, row.transform, UITheme.Sp(10), TextAnchor.MiddleLeft, FontStyle.Normal);
        text.color = new Color(0.92f, 0.93f, 0.95f, 1f);
        var textLe = text.gameObject.AddComponent<LayoutElement>();
        textLe.preferredWidth = UITheme.Dp(105f);

        // Hex input field (NotesApp CreateInputField pattern, compact single-line)
        var input = CreateHexInput($"HexInput_{key}", row.transform, GetCategoryColor(key));
        var inputLe = input.gameObject.AddComponent<LayoutElement>();
        inputLe.preferredWidth = UITheme.Dp(90f);
        inputLe.preferredHeight = UITheme.Dp(24f);

        // Apply button
        var (applyMask, applyBtn, _) = UIFactory.RoundedButtonWithLabel($"Apply_{key}", "Set", row.transform,
            new Color(0.13f, 0.30f, 0.42f, 1f), UITheme.Dp(48f), UITheme.Dp(24f), (int)UITheme.Sp(10), new Color(0.90f, 0.95f, 1f, 1f));
        var applyLe = applyMask.AddComponent<LayoutElement>();
        applyLe.preferredWidth = UITheme.Dp(48f);
        applyLe.preferredHeight = UITheme.Dp(24f);

        var colorRow = new ColorRow(key, defaultHex, input);
        _colorRows.Add(colorRow);

        ButtonUtils.AddListener(applyBtn, () =>
        {
            try
            {
                colorRow.Commit(Mod);
                ApplyColorsToBlips();
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"[MinimapSettings] Color '{key}' failed: {ex.Message}");
            }
        });
    }

    /// <summary>
    /// Compact single-line InputField (Rule 5 note: the phone input focus guard lives in
    /// Shared HotkeyManager.IsInputFieldFocused — consumed by MinimapMod.OnUpdate; this
    /// field is standard uGUI InputField so the guard covers it).
    /// </summary>
    private static InputField CreateHexInput(string name, Transform parent, Color initial)
    {
        var go = new GameObject(name);
        go.transform.SetParent(parent, false);
        go.AddComponent<RectTransform>();

        var inputField = go.AddComponent<InputField>();

        var bg = UIFactory.Panel("Background", go.transform, new Color(0.05f, 0.06f, 0.09f, 1f));
        var bgRt = bg.GetComponent<RectTransform>();
        bgRt.anchorMin = Vector2.zero;
        bgRt.anchorMax = Vector2.one;
        bgRt.offsetMin = Vector2.zero;
        bgRt.offsetMax = Vector2.zero;

        var text = UIFactory.Text("Text", "", go.transform, UITheme.Sp(10), TextAnchor.MiddleLeft);
        var textRt = text.rectTransform;
        textRt.anchorMin = Vector2.zero;
        textRt.anchorMax = Vector2.one;
        textRt.offsetMin = new Vector2(8f, 0f);
        textRt.offsetMax = new Vector2(-8f, 0f);
        text.supportRichText = false;
        text.raycastTarget = false;

        var placeholder = UIFactory.Text("Placeholder", "#RRGGBB", go.transform, UITheme.Sp(10), TextAnchor.MiddleLeft);
        var phRt = placeholder.rectTransform;
        phRt.anchorMin = Vector2.zero;
        phRt.anchorMax = Vector2.one;
        phRt.offsetMin = new Vector2(8f, 0f);
        phRt.offsetMax = new Vector2(-8f, 0f);
        placeholder.supportRichText = false;
        placeholder.color = new Color(1f, 1f, 1f, 0.35f);
        placeholder.raycastTarget = false;

        inputField.textComponent = text;
        inputField.placeholder = placeholder;
        inputField.text = ColorToHex(initial);

        return inputField;
    }

    // ─────────────────────────────────────────────────────────
    // Config plumbing
    // ─────────────────────────────────────────────────────────

    private static MinimapConfig Get() => MinimapMod.Instance?.Config ?? new MinimapConfig();

    private static Color GetCategoryColor(string key)
    {
        var mod = MinimapMod.Instance;
        if (mod == null) return Color.white;
        try
        {
            if (UnityEngine.ColorUtility.TryParseHtmlString(mod.GetBlipColor(key), out var c)) return c;
        }
        catch { }
        return Color.white;
    }

    private static string ColorToHex(Color c)
        => $"#{ColorUtility.ToHtmlStringRGB(c).ToUpperInvariant()}";

    /// <summary>Pushes current hex values into the live blip renderer palette.</summary>
    private static void ApplyColorsToBlips()
    {
        var mod = MinimapMod.Instance;
        if (mod == null) return;
        try { mod.ApplyBlipPalette(); } catch { }
    }

    // ─────────────────────────────────────────────────────────
    // Lifecycle (Rules 2 & 10)
    // ─────────────────────────────────────────────────────────

    private void Update()
    {
        try
        {
            if (_mainBG == null || _mainBG.WasCollected) return;

            bool open = IsOpen();
            if (_mainBG.activeSelf != open)
            {
                _mainBG.SetActive(open);
                if (open) SyncFromConfig();
            }

            if (!open) return;

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                CloseApp();
            }
        }
        catch
        {
            // Rule 10 / stale-subscription guard: after a scene reload S1API re-registers
            // the app and the PREVIOUS instance's MelonEvents.OnUpdate subscription keeps
            // ticking with a destroyed _mainBG. Accessing it can throw — that exception
            // must never abort the shared Update dispatch chain (breaks other apps' toggles,
            // symptom: blank app UI on the freshly opened instance).
        }
    }

    /// <summary>Refresh toggle states from live config each time the app is opened.</summary>
    private void SyncFromConfig()
    {
        foreach (var row in _toggleRows)
        {
            row.SyncFromGetter();
        }
    }

    // ─────────────────────────────────────────────────────────
    // Row abstractions
    // ─────────────────────────────────────────────────────────

    private sealed class ToggleRow
    {
        private readonly string _label;
        private readonly Toggle _toggle;
        private readonly Func<bool> _get;
        private readonly Action<bool> _set;

        public ToggleRow(string label, Toggle toggle, Func<bool> get, Action<bool> set)
        {
            _label = label; _toggle = toggle; _get = get; _set = set;
        }

        public void SyncFromGetter()
        {
            if (_toggle != null && !_toggle.WasCollected) _toggle.SetIsOnWithoutNotify(_get());
        }

        public void Commit()
        {
            if (_toggle == null || _toggle.WasCollected) return;
            _set(_toggle.isOn);
            try { MinimapMod.Instance?.SaveAndApply(); } catch { }
        }
    }

    private sealed class ColorRow
    {
        private readonly string _key;
        private readonly string _defaultHex;
        private readonly InputField _input;

        public ColorRow(string key, string defaultHex, InputField input)
        {
            _key = key; _defaultHex = defaultHex; _input = input;
        }

        public void ResetToDefault()
        {
            if (_input != null && !_input.WasCollected) _input.text = _defaultHex;
            CommitTo(_defaultHex);
        }

        public void Commit(MinimapMod mod)
        {
            if (_input == null || _input.WasCollected) return;
            CommitTo(_input.text);
        }

        private void CommitTo(string hex)
        {
            try
            {
                MinimapMod.Instance?.SetBlipColor(_key, hex);
            }
            catch { }
        }
    }
}
