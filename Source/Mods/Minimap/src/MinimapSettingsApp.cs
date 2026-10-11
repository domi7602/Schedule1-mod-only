using System;
using System.Globalization;
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
/// M6 Settings PhoneApp: "Minimap Settings" — toggles per blip type + palette colour pickers
/// (per user decision 2026-09-12: NO sliders, NO presets).
///
/// Follows the PhoneApp Golden Rules (schedule1-phoneapp skill):
/// 1. Explicit Orientation (horizontal/landscape). 2. Never Destroy UI on close — only SetActive(false).
/// 3. Isolated _mainBG panel, starts hidden.      4. UITheme Sp/Dp (Shared single source).
/// 5. InputFocus guard covers the compact text fields. 6. EventHelper/defensive AddListener for IL2CPP buttons.
/// 7. Config persistence via the mod's existing SafeStorage-backed SaveConfig.
/// 10. Never unsubscribe MelonEvents.OnUpdate in OnPhoneClosed.
///
/// NOTE on colour rows: each row shows a live preview swatch plus a ▼ dropdown that opens the
/// shared 16-swatch palette (MinimapFeatureRules.ColorPalette); picking a swatch applies the
/// colour live. Full hex control stays available via the blip palette sidecar / config.json.
/// </summary>
public sealed class MinimapSettingsApp : PhoneApp
{
    protected override string AppName => "MinimapSettings";
    protected override string AppTitle => "Minimap";
    protected override string IconLabel => "Map";
    protected override string IconFileName => string.Empty;
    protected override EOrientation Orientation => EOrientation.Horizontal;

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
    private ScrollRect _settingsScroll = null!;

    // Rows built once in OnCreatedUI; state re-synced from Config on every open.
    private readonly System.Collections.Generic.List<ToggleRow> _toggleRows = new();
    private readonly System.Collections.Generic.List<ColorRow> _colorRows = new();
    private readonly System.Collections.Generic.List<Action> _inputSyncActions = new();

    // Palette dropdowns: one collapsible panel per colour row (built once, toggled via SetActive).
    private readonly System.Collections.Generic.List<GameObject> _palettePanels = new();
    private GameObject? _openPalette;

    // Phase 2 tab navigation: pages built once, switched via SetActive (never destroyed).
    private int _activeTab;
    private readonly System.Collections.Generic.List<GameObject> _tabPages = new();
    private readonly System.Collections.Generic.List<Image> _tabButtonImages = new();

    private MinimapMod Mod => MinimapMod.Instance ?? throw new InvalidOperationException("Minimap mod not running");

    // ── Color defaults per category (hex, mirrors MinimapBlips defaults) ──
    private static readonly (string Key, string Label, string DefaultHex)[] ColorDefs =
    {
        ("Police",     "Police",        "#338CF2"),
        ("Dealer",     "Dealer",        "#A44DE3"),
        ("Waypoint",   "Waypoint",      "#E14BFF"),
        ("Deal",       "Active Deal",   "#4DA3FF"),
        ("DealClock",  "Accepted Deal", "#4DA3FF"),
        ("Potential",  "Potential Cust","#A44DE3"),
        ("Customer",   "Customer",      "#2ECC70"),
        ("Property",   "Property",      "#F29C12"),
        ("PurchaseCheck", "Purchase Check", "#2ECC70"),
        ("RegionLock", "Regional Lock", "#9AA0A6"),
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
        // UIFactory.Text creates a default 100x100 RectTransform. Stretch the title
        // across the header; otherwise it wraps into tiny stacked fragments.
        title.rectTransform.anchorMin = Vector2.zero;
        title.rectTransform.anchorMax = Vector2.one;
        title.rectTransform.offsetMin = Vector2.zero;
        title.rectTransform.offsetMax = Vector2.zero;
        title.raycastTarget = false;
        title.color = new Color(0.59f, 0.85f, 0.51f, 1f); // emerald accent

        // 2b. Tab bar (Phase 2) — sits between header and scroll content.
        BuildTabBar(_mainBG.transform);

        // 3. Scrollable content – S1API 3.2.0
        _scrollContent = UIFactory.ScrollableVerticalList(
            "ScrollZone",
            _mainBG.transform,
            out _settingsScroll
        );

        // Fix (Log: Content 1301 statt Viewport 1201): Content horizontal exakt
        // auf den Viewport pinnen. Vertikale Anker, dynamische Höhe,
        // ContentSizeFitter und Scroll-Verhalten bleiben unangetastet.
        _scrollContent.anchorMin = new Vector2(0f, 1f);
        _scrollContent.anchorMax = new Vector2(1f, 1f);
        _scrollContent.pivot = new Vector2(0.5f, 1f);
        var pinMin = _scrollContent.offsetMin; pinMin.x = 0f; _scrollContent.offsetMin = pinMin;
        var pinMax = _scrollContent.offsetMax; pinMax.x = 0f; _scrollContent.offsetMax = pinMax;
        var pinSize = _scrollContent.sizeDelta; pinSize.x = 0f; _scrollContent.sizeDelta = pinSize;
        var pinPos = _scrollContent.anchoredPosition; pinPos.x = 0f; _scrollContent.anchoredPosition = pinPos;

        var scrollObj = _settingsScroll.gameObject;

        var scrollLe = scrollObj.AddComponent<LayoutElement>();
        scrollLe.minHeight = 1f;
        scrollLe.flexibleHeight = 1f;

        _settingsScroll.vertical = true;
        _settingsScroll.horizontal = false;
        _settingsScroll.scrollSensitivity = 24f;

        _scrollContent.anchoredPosition = Vector2.zero;

        var contentVlg = _scrollContent.GetComponent<VerticalLayoutGroup>();

        contentVlg.childControlWidth = true;
        contentVlg.childControlHeight = true;
        contentVlg.childForceExpandWidth = true;
        contentVlg.childForceExpandHeight = false;

        // Content now holds the 4 tab pages (each with its own padding) —
        // so no extra padding/spacing here.
        contentVlg.padding = new RectOffset(0, 0, 0, 0);

        contentVlg.spacing = 0f;

        string stage = "BuildMapPage";
        try
        {
            BuildMapPage();

            stage = "BuildMarkerPage";
            BuildMarkerPage();

            stage = "BuildColorsPage";
            BuildColorsPage();

            stage = "BuildAdvancedPage";
            BuildAdvancedPage();

            stage = "BuildFooter";
            BuildFooter(_mainBG.transform);

            stage = "ShowPage";
            ShowPage(0);

            MelonLogger.Msg(
                $"[MinimapSettings] UI completed. " +
                $"Pages: {_scrollContent.childCount}"
            );
        }
        catch (Exception ex)
        {
            MelonLogger.Error(
                $"[MinimapSettings] UI FAILED at {stage}: {ex}"
            );
        }
    }

    private void AddChoiceRow(Transform parent, string label, Func<string> get, Action cycle)
    {
        var row = UIFactory.Panel($"Choice_{label}", parent, new Color(0.11f, 0.13f, 0.18f, 1f));
        row.AddComponent<LayoutElement>().preferredHeight = UITheme.Dp(30f);
        var layout = row.AddComponent<HorizontalLayoutGroup>();
        layout.spacing = UITheme.Dp(6f);
        layout.childAlignment = TextAnchor.MiddleLeft;
        layout.childForceExpandWidth = false;
        layout.childControlWidth = true;
        layout.childControlHeight = false;
        var title = UIFactory.Text("Label", label, row.transform, UITheme.Sp(10), TextAnchor.MiddleLeft);
        title.color = Color.white;
        title.gameObject.AddComponent<LayoutElement>().preferredWidth = UITheme.Dp(92f);
        var value = UIFactory.Text("Value", get(), row.transform, UITheme.Sp(10), TextAnchor.MiddleLeft);
        value.color = new Color(0.65f, 0.86f, 0.69f, 1f);
        value.gameObject.AddComponent<LayoutElement>().flexibleWidth = 1f;
        var (mask, button, _) = UIFactory.RoundedButtonWithLabel($"Change_{label}", "Change", row.transform,
            new Color(0.13f, 0.30f, 0.42f, 1f), UITheme.Dp(64f), UITheme.Dp(24f), (int)UITheme.Sp(10), Color.white);
        var changeLe = mask.GetComponent<LayoutElement>();
        if (changeLe == null) changeLe = mask.AddComponent<LayoutElement>();
        changeLe.preferredWidth = UITheme.Dp(64f);
        ButtonUtils.AddListener(button, () =>
        {
            cycle();
            value.text = get();
            Mod.SaveAndApply();
        });
        _inputSyncActions.Add(() => value.text = get());
    }

    private void AddFloatRow(Transform parent, string label, Func<float> get, Action<float> set, float min, float max, string format)
    {
        var row = UIFactory.Panel($"Number_{label}", parent, new Color(0.11f, 0.13f, 0.18f, 1f));
        row.AddComponent<LayoutElement>().preferredHeight = UITheme.Dp(30f);
        var layout = row.AddComponent<HorizontalLayoutGroup>();
        layout.spacing = UITheme.Dp(5f);
        layout.childAlignment = TextAnchor.MiddleLeft;
        layout.childForceExpandWidth = false;
        layout.childControlWidth = true;
        layout.childControlHeight = false;
        var title = UIFactory.Text("Label", label, row.transform, UITheme.Sp(9), TextAnchor.MiddleLeft);
        title.color = Color.white;
        title.gameObject.AddComponent<LayoutElement>().preferredWidth = UITheme.Dp(112f);
        var input = CreateHexInput($"Value_{label}", row.transform, Color.white);
        input.text = get().ToString(format, CultureInfo.InvariantCulture);
        input.gameObject.AddComponent<LayoutElement>().preferredWidth = UITheme.Dp(72f);
        input.gameObject.GetComponent<LayoutElement>().preferredHeight = UITheme.Dp(24f);
        var (mask, button, _) = UIFactory.RoundedButtonWithLabel($"Set_{label}", "Set", row.transform,
            new Color(0.13f, 0.30f, 0.42f, 1f), UITheme.Dp(48f), UITheme.Dp(24f), (int)UITheme.Sp(10), Color.white);
        var setLe = mask.GetComponent<LayoutElement>();
        if (setLe == null) setLe = mask.AddComponent<LayoutElement>();
        setLe.preferredWidth = UITheme.Dp(48f);
        ButtonUtils.AddListener(button, () =>
        {
            if (!float.TryParse(input.text, NumberStyles.Float, CultureInfo.InvariantCulture, out float parsed)
                || float.IsNaN(parsed) || float.IsInfinity(parsed)) return;
            set(Mathf.Clamp(parsed, min, max));
            input.text = get().ToString(format, CultureInfo.InvariantCulture);
            Mod.SaveAndApply();
        });
        _inputSyncActions.Add(() => input.text = get().ToString(format, CultureInfo.InvariantCulture));
    }

    private void AddKeybindRow(Transform parent, string label, Func<string> get, Action<string> set)
    {
        var row = UIFactory.Panel($"Key_{label}", parent, new Color(0.11f, 0.13f, 0.18f, 1f));
        row.AddComponent<LayoutElement>().preferredHeight = UITheme.Dp(30f);
        var layout = row.AddComponent<HorizontalLayoutGroup>();
        layout.spacing = UITheme.Dp(5f);
        layout.childAlignment = TextAnchor.MiddleLeft;
        layout.childForceExpandWidth = false;
        layout.childControlWidth = true;
        layout.childControlHeight = false;
        var title = UIFactory.Text("Label", label, row.transform, UITheme.Sp(10), TextAnchor.MiddleLeft);
        title.color = Color.white;
        title.gameObject.AddComponent<LayoutElement>().preferredWidth = UITheme.Dp(82f);
        var input = CreateHexInput($"KeyInput_{label}", row.transform, Color.white);
        input.text = get();
        input.gameObject.AddComponent<LayoutElement>().preferredWidth = UITheme.Dp(90f);
        input.gameObject.GetComponent<LayoutElement>().preferredHeight = UITheme.Dp(24f);
        var (mask, button, _) = UIFactory.RoundedButtonWithLabel($"SetKey_{label}", "Set", row.transform,
            new Color(0.13f, 0.30f, 0.42f, 1f), UITheme.Dp(48f), UITheme.Dp(24f), (int)UITheme.Sp(10), Color.white);
        var setLe = mask.GetComponent<LayoutElement>();
        if (setLe == null) setLe = mask.AddComponent<LayoutElement>();
        setLe.preferredWidth = UITheme.Dp(48f);
        ButtonUtils.AddListener(button, () =>
        {
            string value = input.text.Trim();
            if (!Enum.TryParse(value, true, out KeyCode code) || code == KeyCode.None) return;
            set(code.ToString());
            input.text = code.ToString();
            Mod.SaveAndApply();
        });
        _inputSyncActions.Add(() => input.text = get());
    }

    private void AddBorderColorRow(Transform parent)
    {
        BuildColorPickerRow(
            parent,
            "MapBorderColor",
            "Border Color",
            () => Get().BorderColorHex,
            hex =>
            {
                Get().BorderColorHex = hex;
                Mod.SaveAndApply();
            });
    }

    // ─────────────────────────────────────────────────────────
    // Colour pickers: preview swatch + collapsible palette dropdown.
    // Tapping ▼ opens the shared 16-swatch palette (MinimapFeatureRules.ColorPalette);
    // tapping a swatch applies the colour live. Full hex control stays available via the
    // blip palette sidecar / config.json for power users.
    // ─────────────────────────────────────────────────────────

    private void BuildColorPickerRow(
        Transform parent,
        string rowName,
        string label,
        Func<string> getHex,
        Action<string> commitHex)
    {
        var row = UIFactory.Panel(rowName, parent, new Color(0.11f, 0.13f, 0.18f, 1f));
        row.AddComponent<LayoutElement>().preferredHeight = UITheme.Dp(30f);
        var layout = row.AddComponent<HorizontalLayoutGroup>();
        layout.spacing = UITheme.Dp(6f);
        layout.childAlignment = TextAnchor.MiddleLeft;
        layout.childForceExpandWidth = false;
        layout.childControlWidth = true;
        layout.childControlHeight = true;
        layout.childForceExpandHeight = false;
        layout.padding = new RectOffset((int)UITheme.Dp(6f), (int)UITheme.Dp(6f), 0, 0);

        var text = UIFactory.Text("Label", label, row.transform, UITheme.Sp(10), TextAnchor.MiddleLeft, FontStyle.Normal);
        text.color = new Color(0.92f, 0.93f, 0.95f, 1f);
        text.gameObject.AddComponent<LayoutElement>().flexibleWidth = 1f;

        // Current-colour preview swatch.
        var preview = UIFactory.Panel("Preview", row.transform, Color.white);
        var previewLe = preview.AddComponent<LayoutElement>();
        previewLe.preferredWidth = UITheme.Dp(22f);
        previewLe.preferredHeight = UITheme.Dp(22f);
        previewLe.flexibleWidth = 0f;
        var previewImage = preview.GetComponent<Image>();
        previewImage.raycastTarget = false;

        // Dropdown toggle.
        var (dropMask, dropBtn, _) = UIFactory.RoundedButtonWithLabel($"Dropdown_{rowName}", "\u25BC", row.transform,
            new Color(0.13f, 0.30f, 0.42f, 1f), UITheme.Dp(44f), UITheme.Dp(24f), (int)UITheme.Sp(10), Color.white);
        var dropLe = dropMask.GetComponent<LayoutElement>();
        if (dropLe == null) dropLe = dropMask.AddComponent<LayoutElement>();
        dropLe.preferredWidth = UITheme.Dp(44f);
        dropLe.preferredHeight = UITheme.Dp(24f);

        // Collapsible palette panel (created once, hidden until the dropdown opens).
        const int perRow = 8;
        int rows = (MinimapFeatureRules.ColorPalette.Length + perRow - 1) / perRow;
        var panel = UIFactory.Panel($"Palette_{rowName}", parent, new Color(0.08f, 0.10f, 0.14f, 1f));
        panel.AddComponent<LayoutElement>().preferredHeight =
            UITheme.Dp(12f) + rows * UITheme.Dp(28f) + Math.Max(0, rows - 1) * UITheme.Dp(4f);
        var panelVlg = panel.AddComponent<VerticalLayoutGroup>();
        panelVlg.childControlWidth = true;
        panelVlg.childControlHeight = true;
        panelVlg.childForceExpandWidth = true;
        panelVlg.childForceExpandHeight = false;
        panelVlg.childAlignment = TextAnchor.UpperLeft;
        panelVlg.padding = new RectOffset((int)UITheme.Dp(8f), (int)UITheme.Dp(8f), (int)UITheme.Dp(6f), (int)UITheme.Dp(6f));
        panelVlg.spacing = UITheme.Dp(4f);
        panel.SetActive(false);
        _palettePanels.Add(panel);

        for (int r = 0; r < rows; r++)
        {
            var strip = UIFactory.Panel($"PaletteRow{r}", panel.transform, Color.clear);
            strip.AddComponent<LayoutElement>().preferredHeight = UITheme.Dp(28f);
            var stripHlg = strip.AddComponent<HorizontalLayoutGroup>();
            stripHlg.spacing = UITheme.Dp(5f);
            stripHlg.childAlignment = TextAnchor.MiddleLeft;
            stripHlg.childForceExpandWidth = false;
            stripHlg.childControlWidth = true;
            stripHlg.childControlHeight = true;
            stripHlg.childForceExpandHeight = false;

            int start = r * perRow;
            int end = Math.Min(start + perRow, MinimapFeatureRules.ColorPalette.Length);
            for (int i = start; i < end; i++)
            {
                string hex = MinimapFeatureRules.ColorPalette[i];
                var (swMask, swBtn, _) = UIFactory.RoundedButtonWithLabel($"Swatch_{rowName}_{i}", string.Empty, strip.transform,
                    ParseHexOr(hex, Color.white), UITheme.Dp(26f), UITheme.Dp(26f), (int)UITheme.Sp(10), Color.white);
                var swLe = swMask.GetComponent<LayoutElement>();
                if (swLe == null) swLe = swMask.AddComponent<LayoutElement>();
                swLe.preferredWidth = UITheme.Dp(26f);
                swLe.preferredHeight = UITheme.Dp(26f);
                ButtonUtils.AddListener(swBtn, () =>
                {
                    try
                    {
                        commitHex(hex);
                        SyncPreview(previewImage, getHex());
                        ClosePalette(panel);
                    }
                    catch (Exception ex)
                    {
                        MelonLogger.Error($"[MinimapSettings] Colour pick failed: {ex.Message}");
                    }
                });
            }
        }

        ButtonUtils.AddListener(dropBtn, () => TogglePalette(panel));
        SyncPreview(previewImage, getHex());
        _inputSyncActions.Add(() => SyncPreview(previewImage, getHex()));
    }

    private static void SyncPreview(Image preview, string hex)
    {
        try
        {
            if (preview != null && !preview.WasCollected)
                preview.color = ParseHexOr(hex, Color.white);
        }
        catch { }
    }

    private static Color ParseHexOr(string hex, Color fallback)
    {
        try
        {
            if (!string.IsNullOrWhiteSpace(hex) && ColorUtility.TryParseHtmlString(hex.Trim(), out var parsed))
                return parsed;
        }
        catch { }
        return fallback;
    }

    private static string SafeGetBlipColor(string key)
    {
        try { return MinimapMod.Instance?.GetBlipColor(key) ?? string.Empty; }
        catch { return string.Empty; }
    }

    private void TogglePalette(GameObject panel)
    {
        try
        {
            bool opening = !panel.activeSelf;
            CloseOpenPalette();
            panel.SetActive(opening);
            if (opening) _openPalette = panel;
            RefreshSettingsLayout();
        }
        catch (Exception ex)
        {
            MelonLogger.Warning($"[MinimapSettings] Palette toggle failed: {ex.Message}");
        }
    }

    private void ClosePalette(GameObject panel)
    {
        try
        {
            panel.SetActive(false);
            if (_openPalette == panel) _openPalette = null;
            RefreshSettingsLayout();
        }
        catch { }
    }

    private void CloseOpenPalette()
    {
        if (_openPalette == null) return;
        try
        {
            if (!_openPalette.WasCollected) _openPalette.SetActive(false);
        }
        catch { }
        _openPalette = null;
    }

    /// <summary>Immediate scroll-content rebuild after a palette expands or collapses.</summary>
    private void RefreshSettingsLayout()
    {
        try
        {
            Canvas.ForceUpdateCanvases();
            if (_scrollContent != null && !_scrollContent.WasCollected)
                LayoutRebuilder.ForceRebuildLayoutImmediate(_scrollContent);
        }
        catch { }
    }

    private void BuildFooter(Transform parent)
    {
        var footer = UIFactory.Panel("FooterActions", parent, Color.clear);
        var le = footer.AddComponent<LayoutElement>();
        le.preferredHeight = UITheme.Dp(40f);

        var hlg = footer.AddComponent<HorizontalLayoutGroup>();
        hlg.spacing = UITheme.Dp(8f);
        hlg.childAlignment = TextAnchor.MiddleCenter;
        hlg.childForceExpandWidth = false;
        hlg.childControlWidth = true;
        hlg.childControlHeight = false;

        var (saveMask, saveBtn, _) = UIFactory.RoundedButtonWithLabel("SaveBtn", "Save & Apply", footer.transform,
            new Color(0.16f, 0.42f, 0.24f, 1f), UITheme.Dp(120f), UITheme.Dp(30f), (int)UITheme.Sp(12), new Color(0.92f, 0.95f, 0.93f, 1f));
        var saveLe = saveMask.GetComponent<LayoutElement>();
        if (saveLe == null) saveLe = saveMask.AddComponent<LayoutElement>();
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
        var resetLe = resetMask.GetComponent<LayoutElement>();
        if (resetLe == null) resetLe = resetMask.AddComponent<LayoutElement>();
        resetLe.preferredWidth = UITheme.Dp(110f);
        resetLe.preferredHeight = UITheme.Dp(30f);
        ButtonUtils.AddListener(resetBtn, () =>
        {
            try
            {
                foreach (var cr in _colorRows) cr.ResetToDefault();
                ApplyColorsToBlips();
                MinimapMod.Instance?.SaveBlipPalette();
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"[MinimapSettings] Reset failed: {ex.Message}");
            }
        });
    }

    // ─────────────────────────────────────────────────────────
    // Phase 2: tab pages (each short enough for ~one landscape screen)
    // ─────────────────────────────────────────────────────────

    private static readonly string[] TabNames = { "Karte", "Marker", "Farben", "Erweitert" };

    private void BuildTabBar(Transform parent)
    {
        var bar = UIFactory.Panel("TabBar", parent, new Color(0.10f, 0.12f, 0.16f, 1f));
        bar.AddComponent<LayoutElement>().preferredHeight = UITheme.Dp(36f);
        var hlg = bar.AddComponent<HorizontalLayoutGroup>();
        hlg.spacing = UITheme.Dp(6f);
        hlg.padding = new RectOffset((int)UITheme.Dp(8f), (int)UITheme.Dp(8f), (int)UITheme.Dp(4f), (int)UITheme.Dp(4f));
        hlg.childAlignment = TextAnchor.MiddleCenter;
        hlg.childControlWidth = true;
        hlg.childForceExpandWidth = true;
        hlg.childControlHeight = true;
        hlg.childForceExpandHeight = false;

        for (int i = 0; i < TabNames.Length; i++)
        {
            int index = i;
            var (mask, button, _) = UIFactory.RoundedButtonWithLabel($"Tab_{TabNames[i]}", TabNames[i], bar.transform,
                new Color(0.11f, 0.13f, 0.18f, 1f), 100f, UITheme.Dp(28f), (int)UITheme.Sp(11), Color.white);
            var tabLe = mask.GetComponent<LayoutElement>();
            if (tabLe == null) tabLe = mask.AddComponent<LayoutElement>();
            tabLe.flexibleWidth = 1f;
            tabLe.preferredHeight = UITheme.Dp(28f);
            var bgImage = button.GetComponent<Image>();
            _tabButtonImages.Add(bgImage);
            ButtonUtils.AddListener(button, () => ShowPage(index));
        }
    }

    private void ShowPage(int index)
    {
        if (index < 0 || index >= _tabPages.Count) return;
        _activeTab = index;
        for (int i = 0; i < _tabPages.Count; i++)
        {
            var page = _tabPages[i];
            if (page != null && !page.WasCollected)
                page.SetActive(i == index);
        }
        for (int b = 0; b < _tabButtonImages.Count; b++)
        {
            var img = _tabButtonImages[b];
            if (img != null && !img.WasCollected)
                img.color = b == index
                    ? new Color(0.16f, 0.42f, 0.24f, 1f)   // active tab: green
                    : new Color(0.13f, 0.30f, 0.42f, 1f);  // inactive tab: steel blue
        }
        if (_settingsScroll != null && !_settingsScroll.WasCollected)
            _settingsScroll.verticalNormalizedPosition = 1f;
    }

    /// <summary>Creates one tab page panel (vertical stack with its own padding).</summary>
    private Transform CreatePage(string name)
    {
        var page = UIFactory.Panel(name, _scrollContent.transform, Color.clear);
        var vlg = page.AddComponent<VerticalLayoutGroup>();
        vlg.childControlWidth = true;
        vlg.childControlHeight = true;
        vlg.childForceExpandWidth = true;
        vlg.childForceExpandHeight = false;
        vlg.padding = new RectOffset((int)UITheme.Dp(8f), (int)UITheme.Dp(8f), (int)UITheme.Dp(6f), (int)UITheme.Dp(20f));
        vlg.spacing = UITheme.Dp(4f);
        page.SetActive(false);
        _tabPages.Add(page);
        return page.transform;
    }

    private static Transform CreateColumn(Transform parent, string name)
    {
        var col = UIFactory.Panel(name, parent, Color.clear);
        var colLe = col.AddComponent<LayoutElement>();
        colLe.flexibleWidth = 1f;
        var vlg = col.AddComponent<VerticalLayoutGroup>();
        vlg.childControlWidth = true;
        vlg.childControlHeight = true;
        vlg.childForceExpandWidth = true;
        vlg.childForceExpandHeight = false;
        vlg.spacing = UITheme.Dp(4f);
        return col.transform;
    }

    private void BuildMapPage()
    {
        var page = CreatePage("Page_Map");
        AddSectionLabel(page, "MAP");
        AddToggle(page, "Minimap Visible", () => Get().MinimapVisible, v => Get().MinimapVisible = v);
        AddChoiceRow(page, "Shape", () => Get().Shape.ToString(), () =>
        {
            Get().Shape = (MinimapShape)(((int)Get().Shape + 1) % 3);
        });
        AddChoiceRow(page, "Screen Corner", () => Get().AnchorPosition.ToString(), () =>
        {
            Get().HasCustomPosition = false;
            Get().AnchorPosition = (MinimapAnchor)(((int)Get().AnchorPosition + 1) % 4);
        });
        AddChoiceRow(page, "Map Rotation", () => Get().RotationMode.ToString(), () =>
        {
            Get().RotationMode = Get().RotationMode == MinimapRotation.FollowPlayer
                ? MinimapRotation.NorthUp : MinimapRotation.FollowPlayer;
        });

        AddFloatRow(page, "Map Size (140-350)", () => Get().MapSize, v => Get().MapSize = v, 140f, 350f, "0");
        AddFloatRow(page, "Opacity (0-1)", () => Get().BackgroundOpacity, v => Get().BackgroundOpacity = MinimapFeatureRules.ClampOpacity(v),
            0f, 1f, "0.00");
        AddFloatRow(page, "Border Width (0-12)", () => Get().BorderThickness, v => Get().BorderThickness = MinimapFeatureRules.ClampBorderThickness(v),
            0f, MinimapFeatureRules.MaximumBorderThickness, "0.0");
        AddToggle(page, "Map Border", () => Get().ShowMapBorder, v => Get().ShowMapBorder = v);
    }

    private void BuildMarkerPage()
    {
        var page = CreatePage("Page_Marker");
        AddSectionLabel(page, "MARKER FILTERS");

        var cols = UIFactory.Panel("MarkerColumns", page, Color.clear);
        var colsHlg = cols.AddComponent<HorizontalLayoutGroup>();
        colsHlg.spacing = UITheme.Dp(8f);
        colsHlg.childAlignment = TextAnchor.UpperLeft;
        colsHlg.childControlWidth = true;
        colsHlg.childForceExpandWidth = true;
        colsHlg.childControlHeight = true;
        colsHlg.childForceExpandHeight = false;

        var colA = CreateColumn(cols.transform, "MarkerColA");
        AddToggle(colA, "Police", () => Get().ShowPoliceBlips, v => Get().ShowPoliceBlips = v);
        AddToggle(colA, "Dealers", () => Get().ShowDealerBlips, v => Get().ShowDealerBlips = v);
        AddToggle(colA, "Active Deals", () => Get().ShowActiveDeals, v => Get().ShowActiveDeals = v);
        AddToggle(colA, "Potential Cust", () => Get().ShowPotentialCustomers, v => Get().ShowPotentialCustomers = v);
        AddToggle(colA, "Customers", () => Get().ShowKnownCustomers, v => Get().ShowKnownCustomers = v);
        AddToggle(colA, "Properties", () => Get().ShowPropertyBlips, v => Get().ShowPropertyBlips = v);

        var colB = CreateColumn(cols.transform, "MarkerColB");
        AddToggle(colB, "Shops", () => Get().ShowShopBlips, v => Get().ShowShopBlips = v);
        AddToggle(colB, "Quests", () => Get().ShowQuestBlips, v => Get().ShowQuestBlips = v);
        AddToggle(colB, "Vehicles", () => Get().ShowVehicleBlips, v => Get().ShowVehicleBlips = v);
        AddToggle(colB, "Sewer Keys", () => Get().ShowSewerKeyBlips, v => Get().ShowSewerKeyBlips = v);
        AddToggle(colB, "Waypoints", () => Get().ShowWaypoints, v => Get().ShowWaypoints = v);

        AddToggle(page, "Purchase Checks", () => Get().ShowPurchaseChecks, v => Get().ShowPurchaseChecks = v);
        AddToggle(page, "Regional Locks", () => Get().ShowRegionalLocks, v => Get().ShowRegionalLocks = v);
    }

    private void BuildColorsPage()
    {
        var page = CreatePage("Page_Colors");
        AddSectionLabel(page, "BLIP COLORS");

        foreach (var def in ColorDefs)
        {
            AddColorRow(page, def.Label, def.DefaultHex, def.Key);
        }
        AddBorderColorRow(page);
    }

    private void BuildAdvancedPage()
    {
        var page = CreatePage("Page_Advanced");
        AddSectionLabel(page, "KEYBOARD SHORTCUTS (Unity KeyCode names)");
        AddKeybindRow(page, "Toggle", () => Get().ToggleKey, v => Get().ToggleKey = v);
        AddKeybindRow(page, "Zoom In", () => Get().ZoomInKey, v => Get().ZoomInKey = v);
        AddKeybindRow(page, "Zoom Out", () => Get().ZoomOutKey, v => Get().ZoomOutKey = v);

        AddSectionLabel(page, "HUD");
        AddToggle(page, "Health Bar", () => Get().ShowHealthBar, v => Get().ShowHealthBar = v);
        AddToggle(page, "Compass Outline", () => Get().ShowCompassRing, v => Get().ShowCompassRing = v);
        AddToggle(page, "Drag to Reposition", () => Get().AllowDragging, v => Get().AllowDragging = v);
        AddToggle(page, "Hide in Menus", () => Get().HideInMenus, v => Get().HideInMenus = v);
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
        hlg.childControlWidth = true;
        hlg.childForceExpandWidth = false;
        hlg.childControlHeight = true;
        hlg.childForceExpandHeight = false;

        var text = UIFactory.Text("Label", label, row.transform, UITheme.Sp(11), TextAnchor.MiddleLeft, FontStyle.Normal);
        text.color = new Color(0.92f, 0.93f, 0.95f, 1f);
        var textLe = text.gameObject.AddComponent<LayoutElement>();
        textLe.flexibleWidth = 1f;

        var tg = row.AddComponent<Toggle>();
        var checkBg = UIFactory.Panel("CheckBg", row.transform, new Color(0.2f, 0.24f, 0.3f, 1f));
        var checkLe = checkBg.AddComponent<LayoutElement>();
        checkLe.preferredWidth = UITheme.Dp(22f);
        checkLe.preferredHeight = UITheme.Dp(22f);
        checkLe.flexibleWidth = 0f;
        checkLe.flexibleHeight = 0f;
        // Toggle wiring: background stays visible in both states (targetGraphic),
        // a separate checkmark shows the ON state (graphic auto-toggled by Toggle).
        tg.targetGraphic = checkBg.GetComponent<Image>();
        var checkMark = UIFactory.Text("CheckMark", "✓", checkBg.transform, UITheme.Sp(13), TextAnchor.MiddleCenter, FontStyle.Bold);
        checkMark.color = Color.white;
        checkMark.raycastTarget = false;
        var checkRt = checkMark.rectTransform;
        checkRt.anchorMin = Vector2.zero;
        checkRt.anchorMax = Vector2.one;
        checkRt.offsetMin = Vector2.zero;
        checkRt.offsetMax = Vector2.zero;
        tg.graphic = checkMark;
        tg.isOn = get();

        var row2 = new ToggleRow(label, tg, get, set);
        _toggleRows.Add(row2);

        // Safe IL2CPP wiring (Rule 6): Toggle.onValueChanged is a UnityEvent<bool>.
        S1API.Utils.EventHelper.AddListener(_ => row2.Commit(), tg.onValueChanged);
    }

    private void AddColorRow(Transform parent, string label, string defaultHex, string key)
    {
        BuildColorPickerRow(
            parent,
            $"Color_{key}",
            label,
            () => SafeGetBlipColor(key),
            hex =>
            {
                MinimapMod.Instance?.SetBlipColor(key, hex);
                ApplyColorsToBlips();
                // Persist immediately — every other row type also auto-saves on interaction.
                MinimapMod.Instance?.SaveBlipPalette();
            });

        _colorRows.Add(new ColorRow(key, defaultHex));
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
                if (open)
                {
                    RebuildSettingsLayout();
                    SyncFromConfig();
                }
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

    /// <summary>
    /// S1API creates the app container inactive, so rebuild the nested layout after
    /// activation before the first frame is drawn. Reset the scroll to its top edge.
    /// </summary>
    private void RebuildSettingsLayout()
    {
        try
        {
            Canvas.ForceUpdateCanvases();
            var rootRt = _mainBG.GetComponent<RectTransform>();
            if (rootRt != null)
                LayoutRebuilder.ForceRebuildLayoutImmediate(rootRt);
            if (_scrollContent != null)
                LayoutRebuilder.ForceRebuildLayoutImmediate(_scrollContent);
            Canvas.ForceUpdateCanvases();
            if (_settingsScroll != null)
                _settingsScroll.verticalNormalizedPosition = 1f;

            if (_settingsScroll != null && _scrollContent != null && _settingsScroll.viewport != null)
            {
                var scrollRt = _settingsScroll.GetComponent<RectTransform>();
                var viewportRt = _settingsScroll.viewport;

                MelonLogger.Msg(
                    $"[MinimapSettings] Layout: " +
                    $"Tab={_activeTab}, " +
                    $"Scroll={scrollRt.rect.size}, " +
                    $"Viewport={viewportRt.rect.size}, " +
                    $"Content={_scrollContent.rect.size}, " +
                    $"Children={_scrollContent.childCount}"
                );
            }
        }
        catch (Exception ex)
        {
            MelonLogger.Warning($"[MinimapSettings] Layout refresh failed: {ex.Message}");
        }
    }

    /// <summary>Refresh toggle states from live config each time the app is opened.</summary>
    private void SyncFromConfig()
    {
        // Fresh open: collapse any palette dropdown left open last time.
        try
        {
            foreach (var panel in _palettePanels)
            {
                if (panel != null && !panel.WasCollected) panel.SetActive(false);
            }
            _openPalette = null;
        }
        catch { }

        foreach (var row in _toggleRows)
        {
            row.SyncFromGetter();
        }
        foreach (var sync in _inputSyncActions)
        {
            try { sync(); } catch { }
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

        public ColorRow(string key, string defaultHex)
        {
            _key = key; _defaultHex = defaultHex;
        }

        /// <summary>Restores this category's documented default colour.</summary>
        public void ResetToDefault()
        {
            try { MinimapMod.Instance?.SetBlipColor(_key, _defaultHex); } catch { }
        }
    }
}
