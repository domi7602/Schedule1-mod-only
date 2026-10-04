using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using MelonLoader;
using S1API.UI;
using S1API.Utils;
using S1Mods.Shared;
using UnityEngine;
using UnityEngine.UI;

namespace TaxiDriver;

/// <summary>
/// "Taxi" phone app — order the taxi from the in-game phone instead of the
/// keyboard: the CALL TAXI button runs the very same flow as the F5 hotkey
/// (<see cref="SpikeCommands.CallTaxi"/> — single source of truth) and STOP runs
/// the <c>taxi stop</c> console path (<see cref="SpikeCommands.Stop"/>). The
/// status label mirrors the live <see cref="SpikeState"/> (cheap string compare,
/// refreshed from <c>Update</c>).
/// S1API auto-discovers every <c>PhoneApp</c> subclass in this assembly when the
/// phone home screen starts — no manual registration.
/// Follows the workspace phone-app golden rules: explicit vertical orientation,
/// background panel deactivated (never destroyed) in <c>OnPhoneClosed</c>,
/// <c>UITheme</c> sizing, <c>ButtonUtils.AddListener</c> wiring (IL2CPP-safe),
/// idempotent <c>MelonEvents.OnUpdate</c> subscription, no text input fields and
/// no persistence (the spike state lives in <see cref="SpikeState"/>).
///
/// The deferred-close experiment (TaxiCloseExperiment, 45-frame hide delay) was
/// removed 2026-10: it made this app's ESC close lag ~0.75 s behind every other
/// phone app. Close is now immediate, same frame as the others.
/// </summary>
public sealed class TaxiApp : S1API.PhoneApp.PhoneApp
{
    // ---- identity -----------------------------------------------------------

    /// <summary>Internal S1API app key.</summary>
    protected override string AppName => "Taxi";

    /// <summary>App title shown by the phone.</summary>
    protected override string AppTitle => "Taxi";

    /// <summary>Icon caption on the home screen.</summary>
    protected override string IconLabel => "Taxi";

    /// <summary>
    /// No filename shortcut — the icon is resolved in <see cref="IconSprite"/>
    /// (the PotScanner pattern), so a missing PNG can never make the phone log
    /// load errors.
    /// </summary>
    protected override string IconFileName => string.Empty;

    /// <summary>Golden Rule 1: always vertical, always explicit.</summary>
    protected override EOrientation Orientation => EOrientation.Vertical;

    // ---- icon ---------------------------------------------------------------

    private Sprite? _cachedIconSprite;

    /// <summary>
    /// The app icon: <c>taxi_icon.png</c> from the game's <c>Mods</c> folder
    /// (deployed from <c>Source/Mods/TaxiDriver/assets/</c> by
    /// <c>Directory.Build.targets</c>), with a procedural fallback so a missing
    /// or broken file never logs errors.
    /// </summary>
    protected override Sprite? IconSprite
    {
        get
        {
            if (_cachedIconSprite != null)
                return _cachedIconSprite;

            try
            {
                string path = Path.Combine(MelonLoader.Utils.MelonEnvironment.ModsDirectory, "taxi_icon.png");
                if (File.Exists(path))
                {
                    var tex = new Texture2D(2, 2, TextureFormat.RGBA32, false);
                    if (ImageConversion.LoadImage(tex, File.ReadAllBytes(path)))
                    {
                        tex.name = "TaxiApp_Icon";
                        _cachedIconSprite = Sprite.Create(
                            tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
                        return _cachedIconSprite;
                    }

                    MelonLogger.Warning($"[TaxiApp] Icon '{path}' is not a decodable texture — using the procedural fallback.");
                }
                else
                {
                    MelonLogger.Warning($"[TaxiApp] Icon '{path}' not found — using the procedural fallback.");
                }
            }
            catch (Exception ex)
            {
                MelonLogger.Warning($"[TaxiApp] Icon load failed ({ex.Message}) — using the procedural fallback.");
            }

            _cachedIconSprite = CreateFallbackIconSprite();
            return _cachedIconSprite;
        }
    }

    /// <summary>Procedural yellow "T" icon — used when the PNG cannot be loaded.</summary>
    private static Sprite CreateFallbackIconSprite()
    {
        const int size = 64;
        var tex = new Texture2D(size, size, TextureFormat.RGBA32, false) { name = "TaxiApp_Icon_Fallback" };
        var yellow = new Color32(242, 194, 48, 255);
        var dark = new Color32(30, 34, 44, 255);
        var px = new Color32[size * size];
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                bool inT = (y >= 42 && y < 52 && x >= 14 && x < 50) ||   // top bar
                           (x >= 27 && x < 37 && y >= 14 && y < 52);    // stem
                px[y * size + x] = inT ? dark : yellow;
            }
        }

        tex.SetPixels32(px);
        tex.Apply(false, true);
        return Sprite.Create(tex, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f));
    }

    // ---- UI state -----------------------------------------------------------

    private GameObject? _mainBG;
    private Text? _statusLabel;
    private string _statusOverride = string.Empty;
    private float _statusOverrideUntil;

    /// <summary>How long a button outcome stays in the status label before the live state takes over again.</summary>
    private const float StatusOverrideSeconds = 4f;

    /// <summary>Destination picker button background/label colors (selected / unselected).</summary>
    private static readonly Color DestSelectedColor = new(0.95f, 0.76f, 0.10f, 1f);

    private static readonly Color DestUnselectedColor = new(0.16f, 0.20f, 0.31f, 1f);
    private static readonly Color DestSelectedLabelColor = new(0.10f, 0.11f, 0.14f, 1f);
    private static readonly Color DestUnselectedLabelColor = Color.white;

    /// <summary>Tag colour off the selected row, and on it (dark, or the tag is
    /// "ST…" in pale grey on bright yellow — unreadable, see the 0.5.4 screenshot).</summary>
    private static readonly Color DestUnselectedTagColor = new(0.70f, 0.76f, 0.88f, 1f);
    private static readonly Color DestSelectedTagColor = new(0.26f, 0.22f, 0.04f, 1f);

    /// <summary>
    /// Destination list rows keyed by their click key (catalog index / "STAND"),
    /// each carrying the name the highlight compares against (Stage 3d).
    /// </summary>
    private readonly Dictionary<string, (Image Image, Text Label, Text Tag, string Highlight)> _destRows = new();

    /// <summary>Scroll content of the destination list (rows are rebuilt on every app open).</summary>
    private RectTransform? _destContent;

    /// <summary>
    /// One-shot per session: the first app open writes the COMPLETE destination
    /// table into the log (<see cref="TaxiDestinations.DumpPois"/>). The DEAL names
    /// live in scene data (`DeliveryLocation.LocationName`), so the log dump is the
    /// only way to read the full list outside the app itself.
    /// </summary>
    private static bool _catalogDumpedThisSession;

    /// <summary>"DESTINATION: &lt;place&gt;" line above the list.</summary>
    private Text? _destHeader;

    /// <summary>Right-aligned place count in the same line ("79 places").</summary>
    private Text? _destCount;

    /// <summary>Theme scale the app was BUILT with (the global theme can be re-initialised later).</summary>
    private float _buildScale = 1f;

    /// <summary>
    /// Measured label widths (name, preferred width in units) collected while the list is
    /// built, so the [ui] summary can name the rows that have to shrink — measurement
    /// instead of guessing whether a name fits the phone's app container.
    /// </summary>
    private readonly List<(string Name, float Width)> _rowWidths = new();

    // ---- lifecycle ----------------------------------------------------------

    /// <summary>
    /// Golden Rules 6 + 10: defensive, idempotent update-hook wiring. The hook is
    /// NEVER unsubscribed in <c>OnPhoneClosed</c> (the app would stay blank after
    /// the first close).
    /// </summary>
    protected override void OnCreated()
    {
        base.OnCreated();

        MelonEvents.OnUpdate.Unsubscribe(Update);
        MelonEvents.OnUpdate.Subscribe(Update);
    }

    /// <summary>Builds the compact phone UI (background panel starts hidden).</summary>
    protected override void OnCreatedUI(GameObject container)
    {
        var containerRt = container.GetComponent<RectTransform>();
        if (containerRt != null)
            UITheme.InitializeForTextApp(containerRt);

        BuildUI(container);
    }

    /// <summary>IL2CPP liveness: managed wrappers survive scene unload while native objects are dead.</summary>
    private static bool IsAlive([NotNullWhen(true)] UnityEngine.Object? obj)
    {
        if (obj == null) return false;
        try { return obj.Pointer != IntPtr.Zero && !obj.WasCollected && (UnityEngine.Object)obj != null; }
        catch { return false; }
    }

<<<<<<< HEAD
    /// <summary>
    /// Golden Rule 2: deactivate the background panel ONLY — never
    /// <c>Object.Destroy</c>, never clear the hierarchy (the "Transparent Phone"
    /// bug).
    /// </summary>
    protected override void OnPhoneClosed()
    {
        TaxiLog.Verbose($"[close] OnPhoneClosed f={Time.frameCount} t={Time.unscaledTime:0.000}");
        // Hide immediately, like every other phone app. The deferred-close experiment
        // (patched S1API SetAppOpen, HideDelayFrames=45) made the Taxi app linger ~0.75 s
        // behind the others on ESC - removed, the close path is now the plain S1API one.
        base.OnPhoneClosed();
        if (IsAlive(_mainBG))
            _mainBG.SetActive(false);
    }

    /// <summary>
    /// Per-frame sync: background visibility follows <see cref="IsOpen"/>,
    /// Escape closes the app, and the status label is refreshed.
    /// </summary>
    private void Update()
    {
        if (!IsAlive(_mainBG))
            return;

        bool open = IsOpen();
        // Visibility follows IsOpen() in BOTH directions. _mainBG starts inactive
        // (Golden Rule 3) and this sync is the only thing that ever shows it, so the
        // OPEN direction must never be gated by a close-path experiment.
        if (_mainBG.activeSelf != open)
        {
            _mainBG.SetActive(open);

            // The destination catalog walks the scene (deal locations + lots), so it
            // is built on open — never per frame.
            if (open)
                RebuildDestinationList();
            else
                _closeTraceFrames = 8;   // [close] frame-gap trace
        }

        if (_uiDumpFrames > 0 && --_uiDumpFrames == 0)
        {
            RefitLabelsAtRender();
            DumpActualLayout();
        }

        if (_closeTraceFrames > 0)
        {
            _closeTraceFrames--;
            Mod.Log.Info($"[close] trace f={Time.frameCount} t={Time.unscaledTime:0.000} open={open} mainBG={_mainBG.activeSelf}");
        }

        if (!open)
            return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CloseApp();
            return;
        }

        RefreshStatus();
        RefreshDestinationHighlight();
    }

    // ---- UI construction ----------------------------------------------------

    private void BuildUI(GameObject container)
    {
        // The theme scale is GLOBAL and gets re-initialised by whichever phone app is
        // opened last (log: two "Responsive canvas initialized" lines 67 ms apart, 1.60
        // then 1.20). Every size in this app is therefore Dp/Sp() at BUILD time, and the
        // [ui] report has to use the same number — reporting the current one produced a
        // wrong "name font 22 pt / column 489 units" while the app was built at 29 pt.
        _buildScale = UITheme.Scale;
        _rowHeight = UITheme.Dp(48f);
        _rowPadH = UITheme.Dp(10f);
        _rowSpacing = UITheme.Dp(8f);
        _nameSize = UITheme.Sp(18);
        _nameMin = UITheme.Sp(12);
        _tagSize = UITheme.Sp(12);
        _tagMin = UITheme.Sp(9);
        _tagWidth = UITheme.Dp(78f);
        _tagMinWidth = UITheme.Dp(64f);
        _nameColumnWidth = UITheme.ActualWidth - 2f * ScreenSafeInset - 2f * (14f * _buildScale) - 2f * 10f - 2f * (10f * _buildScale) - (78f * _buildScale + 8f * _buildScale);
        // Golden Rule 3: isolated background panel, starts hidden.
        var bg = UIFactory.Panel("TaxiApp_MainBG", container.transform, new Color(0.078f, 0.102f, 0.18f, 1f), fullAnchor: true);
        _mainBG = bg;

        var layout = bg.AddComponent<VerticalLayoutGroup>();
        int safeInset = (int)ScreenSafeInset;
        layout.padding = new RectOffset(
            (int)UITheme.Dp(14) + safeInset, (int)UITheme.Dp(14) + safeInset, (int)UITheme.Dp(14), (int)UITheme.Dp(14));
        layout.spacing = UITheme.Dp(10);
        layout.childAlignment = TextAnchor.UpperCenter;
        layout.childControlWidth = true;
        layout.childControlHeight = true;
        layout.childForceExpandWidth = true;
        layout.childForceExpandHeight = false;

        // Header bar.
        var header = UIFactory.Panel("TaxiApp_Header", bg.transform, new Color(0.11f, 0.14f, 0.23f, 1f));
        AddFixedHeight(header, 56f);
        var title = UIFactory.Text("TaxiApp_Title", "TAXI", header.transform, UITheme.Sp(22), TextAnchor.MiddleCenter, FontStyle.Bold);
        Stretch(title.rectTransform);
        title.color = new Color(0.95f, 0.78f, 0.2f, 1f);

        // Primary action — orders the taxi (same flow as the F5 hotkey).
        MakeButton("TaxiApp_CallBtn", bg.transform, new Color(0.95f, 0.76f, 0.10f, 1f), 72f,
            "CALL TAXI", UITheme.Sp(20), FontStyle.Bold, new Color(0.10f, 0.11f, 0.14f, 1f), OnCallTaxiPressed);

        // Stage 3d: instead of three buttons (two of them hand-read coordinates) the
        // app now lists every place the GAME owns — deal locations first, then
        // parking lot entries (`taxi pois` dumps the same list to the log). Tapping
        // a row runs the shared SpikeCommands.SetRideDestination, which also
        // resolves the drop-off (a goal that is off the vehicle graph is served by
        // the nearest lot entry instead of being force-driven into geometry).
        var destHeaderPanel = UIFactory.Panel("TaxiApp_DestHeaderPanel", bg.transform, new Color(0.11f, 0.14f, 0.23f, 1f));
        AddFixedHeight(destHeaderPanel, 42f);
        // HorizontalLayoutGroup instead of hand-placed rects (BankApp's idiom): the
        // layout places the two texts, so nothing can slide out of the panel.
        var headerLayout = destHeaderPanel.AddComponent<HorizontalLayoutGroup>();
        headerLayout.padding = new RectOffset((int)UITheme.Dp(10f), (int)UITheme.Dp(10f), 0, 0);
        headerLayout.spacing = UITheme.Dp(8f);
        headerLayout.childAlignment = TextAnchor.MiddleLeft;
        headerLayout.childControlWidth = true;
        headerLayout.childControlHeight = true;
        headerLayout.childForceExpandWidth = true;
        headerLayout.childForceExpandHeight = true;

        int headerSize = UITheme.Sp(15);
        _destHeader = UIFactory.Text("TaxiApp_DestHeader", "DESTINATION: none picked", destHeaderPanel.transform,
            headerSize, TextAnchor.MiddleLeft, FontStyle.Bold);
        _destHeader.color = DestSelectedColor;
        _destHeader.gameObject.AddComponent<LayoutElement>().flexibleWidth = 1f;
        MakeSingleLineFitting(_destHeader, headerSize, UITheme.Sp(11));

        int countSize = UITheme.Sp(12);
        _destCount = UIFactory.Text("TaxiApp_DestCount", string.Empty, destHeaderPanel.transform,
            countSize, TextAnchor.MiddleRight, FontStyle.Normal);
        _destCount.color = new Color(0.62f, 0.68f, 0.80f, 1f);
        var countLe = _destCount.gameObject.AddComponent<LayoutElement>();
        countLe.preferredWidth = UITheme.Dp(84f);
        countLe.minWidth = UITheme.Dp(70f);
        countLe.flexibleWidth = 0f;
        MakeSingleLineFitting(_destCount, countSize, UITheme.Sp(9));

        _destCount.color = new Color(0.62f, 0.68f, 0.80f, 1f);

        var destPanel = UIFactory.Panel("TaxiApp_DestPanel", bg.transform, new Color(0.09f, 0.12f, 0.20f, 1f));
        AddFixedHeight(destPanel, 370f);
        _destContent = UIFactory.ScrollableVerticalList("TaxiApp_DestScroll", destPanel.transform, out ScrollRect destScroll);

        // Content width = VIEWPORT width (2026-09-29, the real clipping mechanism):
        // UIFactory's content keeps a width of its own (measured: 711 units inside a
        // 655-unit screen!), so rows laid out to the content hung ~28 units out at
        // BOTH sides - past the visible screen edge. That is why names lost their
        // first letters and tags their last while every rect "fit" its parent.
        // Force the content to the viewport's width (x only; y stays free for
        // FitContentHeight). Row/name math then matches _nameColumnWidth exactly.
        RectTransform contentRt = _destContent;
        contentRt.anchorMin = new Vector2(0f, contentRt.anchorMin.y);
        contentRt.anchorMax = new Vector2(1f, contentRt.anchorMax.y);
        contentRt.offsetMin = new Vector2(0f, contentRt.offsetMin.y);
        contentRt.offsetMax = new Vector2(0f, contentRt.offsetMax.y);
        // Scroll-content fix (2026-09-29 screenshot): UIFactory's content
        // VerticalLayoutGroup does NOT set childControlWidth, so rows kept their
        // PREFERRED width (long names = wide rows) and bled past BOTH mask edges -
        // names lost their first letters, tags their last. Width-control the rows to
        // the content. Headers never showed this: the app root VLG has
        // childControlWidth = true.
        var contentLayout = _destContent.GetComponent<VerticalLayoutGroup>();
        if (contentLayout != null)
        {
            contentLayout.childControlWidth = true;
            contentLayout.childForceExpandWidth = true;
        }
        UIFactory.FitContentHeight(_destContent);
        if (destScroll != null)
        {
            destScroll.vertical = true;
            destScroll.movementType = ScrollRect.MovementType.Clamped;
        }

        // Secondary action — same code path as the `taxi stop` console command.
        MakeButton("TaxiApp_StopBtn", bg.transform, new Color(0.62f, 0.17f, 0.17f, 1f), 62f,
            "STOP", UITheme.Sp(18), FontStyle.Bold, Color.white, OnStopPressed);

        // Live status.
        var statusPanel = UIFactory.Panel("TaxiApp_Status", bg.transform, new Color(0.11f, 0.14f, 0.23f, 1f));
        AddFixedHeight(statusPanel, 66f);
        var status = UIFactory.Text("TaxiApp_StatusLabel", "No taxi", statusPanel.transform, UITheme.Sp(15), TextAnchor.MiddleCenter);
        Stretch(status.rectTransform);
        _statusLabel = status;

        bg.SetActive(false);
    }

    /// <summary>
    /// Makes a text line that can never be cut off or wrap out of its row: the rect is
    /// exactly one line tall, best-fit shrinks the font between <paramref name="minSize"/>
    /// and <paramref name="maxSize"/> until the text fits, and Truncate is only the last
    /// resort. Dominik (2026-09-28): "Die Wege sind abgeschnitten, zu breit für die phone
    /// UI" — a place name like "Thompson Street Taxi Station" at Sp(18) is wider than
    /// the app container, and neither wrapping nor clipping is acceptable.
    /// </summary>
    private static void MakeSingleLineFitting(Text txt, int maxSize, int minSize)
    {
        txt.resizeTextForBestFit = true;
        txt.resizeTextMaxSize = maxSize;
        txt.resizeTextMinSize = minSize;
        txt.verticalOverflow = VerticalWrapMode.Truncate;
    }

    /// <summary>
    /// Single-line label with a COMPUTED-ONCE font size (2026-09-29). The earlier
    /// resizeTextForBestFit variant re-ran Unity's text generator on EVERY canvas
    /// rebuild - with 80 rows x 2 labels that made each close/open rebuild do ~160
    /// generator passes (the Taxi-only flicker while the phone folds). The fit ratio
    /// is the same math the [ui] report prints.
    /// </summary>
    private static void MakeFixedFitting(Text txt, int maxSize, int minSize, float availableWidth)
    {
        txt.resizeTextForBestFit = false;
        txt.verticalOverflow = VerticalWrapMode.Truncate;
        txt.horizontalOverflow = HorizontalWrapMode.Overflow;
        float natural = txt.preferredWidth;
        if (natural > availableWidth && natural > 1f)
            txt.fontSize = Mathf.Clamp(Mathf.FloorToInt(maxSize * availableWidth / natural), minSize, maxSize);
    }

    /// <summary>Builds a full-width button row and wires it IL2CPP-safely.</summary>
    private static void MakeButton(string name, Transform parent, Color bg, float height,
        string label, int fontSize, FontStyle style, Color labelColor, Action onClick)
    {
        var go = UIFactory.Panel(name, parent, bg);
        AddFixedHeight(go, height);

        var btn = go.AddComponent<Button>();
        btn.targetGraphic = go.GetComponent<Image>();

        var txt = UIFactory.Text(name + "_Label", label, go.transform, fontSize, TextAnchor.MiddleCenter, style);
        Stretch(txt.rectTransform);
        txt.color = labelColor;

        // Golden Rule 5: NEVER wire clicks via the raw onClick API directly
        // (IL2CPP crash) — always ButtonUtils.AddListener.
        ButtonUtils.AddListener(btn, onClick);
    }

    private static void AddFixedHeight(GameObject go, float height)
    {
        var le = go.AddComponent<LayoutElement>();
        le.preferredHeight = UITheme.Dp(height);
    }

    private static void Stretch(RectTransform rt)
    {
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;
    }

    // Row geometry baked at BUILD time (2026-09-29): the global UITheme scale is
    // re-initialised by whichever app builds last (log: 1.60 -> 1.20 within 67 ms),
    // so live Dp/Sp at OPEN time could size rows differently than this app's frame.
    // Same invariant as the [ui] report's _buildScale math.
    private float _rowHeight, _rowPadH, _rowSpacing, _tagWidth, _tagMinWidth;
    private int _nameSize, _nameMin, _tagSize, _tagMin;
    private int _uiDumpFrames;
    private float _nameColumnWidth;
    private int _closeTraceFrames;

    /// <summary>
    /// Screen-safe side inset (2026-09-29): the phone's VISIBLE screen is narrower
    /// than the app container - the bezel hides units at both edges (screenshot
    /// analysis: container ~755 units vs a ~655-unit screen). Text laid out to the
    /// full container ends up BEHIND the bezel: the row names lost their first
    /// letters and the tags their last even though every rect fit its parent.
    /// Every column now stays clear of the hidden edge zone.
    /// </summary>
    private static float ScreenSafeInset => UITheme.Dp(18f);

    /// <summary>
    /// One destination row (tap = pick). LayoutElement-driven height, IL2CPP-safe
    /// click wiring, highlight driven by <see cref="SpikeState.RideDestinationName"/>.
    /// </summary>
    private void MakeDestinationRow(string key, string name, string tag, string highlight)
    {
        if (_destContent == null)
            return;

        var go = UIFactory.Panel($"Dest_{key}", _destContent, DestUnselectedColor);
        var le = go.AddComponent<LayoutElement>();
        // Legibility (2026-09-28): the row used to be Dp(30) with Sp(11) text, which
        // rendered as 5 px on screen — half of the smallest text in the Weather app
        // (measured from Dominik's screenshots: phone 655x1201 units at 0.42 px/unit).
        // Row = Dp(48) with Sp(18) text now, and the "DEAL * " prefix moved into a
        // small right-aligned tag so the NAME gets the width.
        le.preferredHeight = _rowHeight;

        var btn = go.AddComponent<Button>();
        btn.targetGraphic = go.GetComponent<Image>();

        // BankApp's row idiom: a HorizontalLayoutGroup positions both texts itself, so no
        // RectTransform hand-math can push the name out of its row — exactly what clipped
        // the first letters ("xi-Stand", "eyway behind…") behind the scroll mask.
        var rowLayout = go.AddComponent<HorizontalLayoutGroup>();
        rowLayout.padding = new RectOffset((int)_rowPadH, (int)_rowPadH, 0, 0);
        rowLayout.spacing = _rowSpacing;
        rowLayout.childAlignment = TextAnchor.MiddleLeft;
        rowLayout.childControlWidth = true;
        rowLayout.childControlHeight = true;
        // 2026-09-29: force-expand made the FIXED tag expand too (50/50 split).
        // false = the tag keeps LayoutElement.preferredWidth, the name gets the rest.
        rowLayout.childForceExpandWidth = false;
        rowLayout.childForceExpandHeight = true;

        int nameSize = _nameSize;
        var txt = UIFactory.Text($"Dest_{key}_Name", name, go.transform, nameSize, TextAnchor.MiddleLeft, FontStyle.Bold);
        txt.color = DestUnselectedLabelColor;
        txt.gameObject.AddComponent<LayoutElement>().flexibleWidth = 1f;   // name takes the rest
        MakeFixedFitting(txt, nameSize, _nameMin, _nameColumnWidth);

        // Measurement instead of guessing (see the [ui] summary after the list build).
        _rowWidths.Add((name, txt.preferredWidth));

        int tagSize = _tagSize;
        var tagText = UIFactory.Text($"Dest_{key}_Tag", tag, go.transform, tagSize, TextAnchor.MiddleRight, FontStyle.Bold);
        tagText.color = DestUnselectedTagColor;
        var tagLe = tagText.gameObject.AddComponent<LayoutElement>();
        tagLe.preferredWidth = _tagWidth;
        tagLe.minWidth = _tagMinWidth;
        tagLe.flexibleWidth = 0f;                                          // tag keeps its width
        MakeFixedFitting(tagText, tagSize, _tagMin, _tagWidth);

        string captured = key;
        ButtonUtils.AddListener(btn, () => OnDestinationPressed(captured));
        _destRows[captured] = (go.GetComponent<Image>(), txt, tagText, highlight);
    }

    /// <summary>
    /// Rebuilds the destination list. Called once per app open because
    /// <see cref="TaxiDestinations.BuildCatalog"/> walks the scene (never per frame).
    /// </summary>
    private void RebuildDestinationList()
    {
        if (_destContent == null)
            return;

        UIFactory.ClearChildren(_destContent);
        _destRows.Clear();
        _rowWidths.Clear();

        // Paket D (2026-09-29, "nach Checkpoint-Auswahl bleibt immer der letzte
        // markiert"): explicit clear row on top. Its highlight key is NoDestinationName,
        // so it reads as "selected" exactly while nothing is picked.
        MakeDestinationRow("CLEAR", "✕ Clear selection", "NONE", SpikeState.NoDestinationName);

        // The taxi stand stays a first-class choice: proven routable, because the
        // taxi spawns on that street-side entry on every call-taxi run.
        int places = 1;
        MakeDestinationRow("STAND", $"Taxi-Stand ({TaxiStand.StandName})", "STAND", "Taxi-Stand");

        if (!TaxiDestinations.BuildCatalog(out List<TaxiDestinations.Destination> catalog, "TaxiApp list"))
        {
            MakeDestinationRow("NONE", "No places found — is a save loaded?", string.Empty, string.Empty);
            if (IsAlive(_destCount))
                _destCount.text = "—";
            UIFactory.FitContentHeight(_destContent);
            return;
        }

        if (!_catalogDumpedThisSession)
        {
            _catalogDumpedThisSession = true;
            TaxiDestinations.DumpPois("TaxiApp first open — full destination dump (one-shot per session)");
        }

        // The INDEX is the click key: lot names repeat in the scene ("Parking"),
        // and a name lookup would refuse an ambiguous hit.
        foreach (TaxiDestinations.Destination destination in catalog)
        {
            MakeDestinationRow(destination.Index.ToString(), destination.Name, destination.Tag, destination.Name);
            places++;
        }

        if (IsAlive(_destCount))
            _destCount.text = $"{places} places";

        UIFactory.FitContentHeight(_destContent);
        _uiDumpFrames = 3;   // [ui2] real-layout dump 3 frames later
        ReportLabelWidths();
    }

    /// <summary>
    /// Logs the label geometry that decides legibility: how wide the name column is
    /// (unit math, independent of the layout pass) and the widest names the list has to
    /// fit. This is what tells us whether best-fit has to shrink anything, without
    /// needing a screenshot.
    /// </summary>
    private void ReportLabelWidths()
    {
        float scale = _buildScale;
        float container = UITheme.ActualWidth;
        float appPadding = 14f * scale + ScreenSafeInset;
        float scrollPadding = 10f;                 // ScrollableVerticalList uses raw units
        float nameInset = 10f * scale;
        float tagColumn = 78f * scale + 8f * scale;
        float nameColumn = container - 2f * appPadding - 2f * scrollPadding - 2f * nameInset - tagColumn;
        int nameSize = Mathf.RoundToInt(18f * scale);

        Mod.Log.Info(
            $"[ui] app container {container:0} x {UITheme.ActualHeight:0} units, built at scale {scale:0.00}; " +
            $"name column {nameColumn:0} units, name font {nameSize} pt, min {Mathf.RoundToInt(12f * scale)} pt (best-fit), " +
            $"tag column {tagColumn:0} units.");

        if (_rowWidths.Count == 0)
            return;

        int over = 0;
        int cut = 0;
        float minSize = 12f * scale;
        foreach ((string name, float width) in _rowWidths)
        {
            if (width <= nameColumn)
                continue;

            over++;
            if (width * minSize / Math.Max(nameSize, 1) > nameColumn)
                cut++;
        }

        Mod.Log.Info(
            $"[ui] {_rowWidths.Count} rows measured: {over} wider than the name column, " +
            $"{cut} still too wide at the minimum font size (those would be cut).");

        foreach ((string name, float width) in _rowWidths.OrderByDescending(r => r.Width).Take(5))
            Mod.Log.Info($"[ui]   widest: '{name}' needs {width:0} units" +
                         $"{(width > nameColumn ? $" -> shrinks to {nameSize * nameColumn / Math.Max(width, 1f):0} pt" : " (fits)")}");
    }

    /// <summary>
    /// Render-time fit verification (2026-09-29): the build-time fit trusts
    /// _nameColumnWidth unit math; this pass re-checks every label against its REAL
    /// laid-out rect 3 frames after the build and shrinks the font of anything that
    /// would render wider than its box. One generator pass per label, only on list
    /// rebuild - not per canvas rebuild (that caused the fold flicker).
    /// </summary>
    private void RefitLabelsAtRender()
    {
        int over = 0;
        float worst = 0f;
        foreach (KeyValuePair<string, (Image Image, Text Label, Text Tag, string Highlight)> entry in _destRows)
        {
            over += RefitLabel(entry.Value.Label, _nameMin, ref worst);
            over += RefitLabel(entry.Value.Tag, _tagMin, ref worst);
        }
        Mod.Log.Info($"[ui2] render-fit: {over} label(s) were wider than their rect (worst +{worst:0}u) and were shrunk to fit.");
    }

    private static int RefitLabel(Text txt, int minSize, ref float worst)
    {
        if (!IsAlive(txt))
            return 0;
        float avail = txt.rectTransform.rect.width;
        if (avail < 1f)
            return 0;
        float natural = txt.preferredWidth;
        float overBy = natural - avail;
        if (overBy <= 0f)
            return 0;
        if (overBy > worst)
            worst = overBy;
        txt.fontSize = Mathf.Clamp(Mathf.FloorToInt(txt.fontSize * avail / natural), minSize, txt.fontSize);
        return 1;
    }

    /// <summary>
    /// [ui2] one-shot real-layout dump (3 frames after a list build): reports the
    /// ACTUAL row/name rects against the scroll viewport. The [ui] report is unit
    /// math and cannot see layout-pass positioning - the clipped first letters
    /// ("Ta" in "Taxi-Stand") are positional, and this dump names the mechanism.
    /// worldLeftInset &lt; 0 = the name starts left of the mask = clipped left.
    /// </summary>
    private void DumpActualLayout()
    {
        try
        {
            if (_destContent == null)
                return;

            RectTransform contentRt = _destContent;
            // Il2cpp-interop safe: an `as RectTransform` on Transform.parent can come
            // back null in MelonLoader even when the parent IS a RectTransform (that is
            // exactly why the 13:20 [ui2] dump printed worldLeft=worldRight=0.0).
            RectTransform? viewportRt = contentRt.parent != null ? contentRt.parent.GetComponent<RectTransform>() : null;
            Vector3[] corners = new Vector3[4];

            float vpLeft = 0f, vpRight = 0f;
            if (viewportRt != null)
            {
                viewportRt.GetWorldCorners(corners);
                vpLeft = corners[0].x;
                vpRight = corners[2].x;
            }

            Mod.Log.Info(
                $"[ui2] theme scale now {UITheme.Scale:0.00} (built at {_buildScale:0.00}); " +
                $"content anchoredX={contentRt.anchoredPosition.x:0.0} w={contentRt.rect.width:0.0} " +
                $"pivot=({contentRt.pivot.x:0.00},{contentRt.pivot.y:0.00}); " +
                $"viewport worldLeft={vpLeft:0.0} worldRight={vpRight:0.0}");

            int shown = 0;
            foreach (KeyValuePair<string, (Image Image, Text Label, Text Tag, string Highlight)> entry in _destRows)
            {
                if (!IsAlive(entry.Value.Label))
                    continue;

                RectTransform nameRt = entry.Value.Label.rectTransform;
                RectTransform? rowRt = nameRt.parent as RectTransform;
                nameRt.GetWorldCorners(corners);
                float inset = corners[0].x - vpLeft;

                Mod.Log.Info(
                    $"[ui2]   row '{entry.Key}': row anchoredX={(rowRt != null ? rowRt.anchoredPosition.x : -99999f):0.0} " +
                    $"w={(rowRt != null ? rowRt.rect.width : -1f):0.0} pivot=({(rowRt != null ? rowRt.pivot.x : -1f):0.00}); " +
                    $"name anchoredX={nameRt.anchoredPosition.x:0.0} w={nameRt.rect.width:0.0} " +
                    $"worldLeftInset={inset:0.0} u (NEGATIVE = first letters clipped)");

                if (++shown >= 4)
                    break;
            }
        }
        catch (Exception ex)
        {
            Mod.Log.Warn($"[ui2] layout dump failed: {ex.Message}");
        }
    }

    // ---- button handlers ----------------------------------------------------

    private void OnCallTaxiPressed()
    {
        if (!SceneGate.IsInMainScene)
        {
            SetStatusOverride("Only available in gameplay.");
            return;
        }

        bool started = SpikeCommands.CallTaxi("TaxiApp");
        SetStatusOverride(started
            ? "Taxi ordered — spawning at the stand, then driving to you."
            : "Could not order the taxi — see the log.");
    }

    /// <summary>
    /// STOP = cancel the ride AND despawn the taxi (Dominik: "Kündigen soll das
    /// Taxi despawnen lassen") — the proven cleanup order, with everyone verified
    /// out of the car before it is destroyed.
    /// </summary>
    private void OnStopPressed()
    {
        if (!SceneGate.IsInMainScene)
        {
            SetStatusOverride("Only available in gameplay.");
            return;
        }

        bool stopped = SpikeCommands.Stop();
        SetStatusOverride(stopped
            ? "Ride cancelled — the taxi has been despawned."
            : "Could not despawn the taxi — see the log.");
    }

    /// <summary>Destination row press → the shared SpikeCommands ride destination.</summary>
    private void OnDestinationPressed(string key)
    {
        if (!SceneGate.IsInMainScene)
        {
            SetStatusOverride("Only available in gameplay.");
            return;
        }

        // Paket D: the clear row is not a destination — drop the pick instead of
        // resolving a place.
        if (key == "CLEAR")
        {
            SpikeState.ClearDestination();
            SetStatusOverride("Destination cleared — tap a place to pick a new one.");
            Mod.Log.Info("[ride] destination cleared via the app (Paket D).");
            return;
        }

        bool ok = SpikeCommands.SetRideDestination(key, "TaxiApp");
        SetStatusOverride(ok
            ? $"Destination: {SpikeState.RideDestinationName} ({SpikeState.RideDropOff})"
            : $"Could not set '{key}' — see the log.");
    }

    // ---- status label -------------------------------------------------------

    /// <summary>Shows a button outcome for a few seconds, then the live state returns.</summary>
    private void SetStatusOverride(string text)
    {
        _statusOverride = text;
        _statusOverrideUntil = Time.unscaledTime + StatusOverrideSeconds;
    }

    private void RefreshStatus()
    {
        if (!IsAlive(_statusLabel))
            return;

        string desired = Time.unscaledTime < _statusOverrideUntil ? _statusOverride : BuildLiveStatus();
        if (_statusLabel.text != desired) // cheap string compare before assigning
            _statusLabel.text = desired;
    }

    /// <summary>
    /// Highlights the row matching <see cref="SpikeState.RideDestinationName"/> and
    /// keeps the "DESTINATION:" line in sync (cheap string compares, no per-frame
    /// allocations beyond the one label string).
    /// </summary>
    private void RefreshDestinationHighlight()
    {
        string selected = SpikeState.RideDestinationName;

        if (IsAlive(_destHeader))
        {
            // Paket A (2026-09-29): the header is honest about the pick state —
            // "none selected" with a hint instead of silently naming a default.
            string header;
            if (!SpikeState.RideDestinationPicked)
            {
                header = "DESTINATION: none selected — tap a place";
            }
            else
            {
                // The drop-off detail can be a whole sentence ("lot entry (nearest to the
                // goal, 43 m)") — the line only shows its kind, the log has the rest.
                string dropOff = SpikeState.RideDropOff ?? string.Empty;
                int paren = dropOff.IndexOf('(');
                if (paren > 0)
                    dropOff = dropOff.Substring(0, paren).Trim();

                header = string.IsNullOrEmpty(dropOff)
                    ? $"DESTINATION: {selected}"
                    : $"DESTINATION: {selected}  >  {dropOff}";
            }

            if (_destHeader.text != header)
                _destHeader.text = header;
        }

        foreach (KeyValuePair<string, (Image Image, Text Label, Text Tag, string Highlight)> entry in _destRows)
        {
            bool isSelected = string.Equals(entry.Value.Highlight, selected, StringComparison.Ordinal);

            // Change guards only (2026-09-29): an unconditional Set dirties the
            // graphics and forces a canvas rebuild EVERY frame for 80 rows x 3
            // graphics - prime suspect for the Taxi-only frame flicker.
            Color imageColor = isSelected ? DestSelectedColor : DestUnselectedColor;
            if (IsAlive(entry.Value.Image) && entry.Value.Image.color != imageColor)
                entry.Value.Image.color = imageColor;

            Color labelColor = isSelected ? DestSelectedLabelColor : DestUnselectedLabelColor;
            if (IsAlive(entry.Value.Label) && entry.Value.Label.color != labelColor)
                entry.Value.Label.color = labelColor;

            if (entry.Value.Tag != null && IsAlive(entry.Value.Tag))
            {
                Color tagColor = isSelected ? DestSelectedTagColor : DestUnselectedTagColor;
                if (entry.Value.Tag.color != tagColor)
                    entry.Value.Tag.color = tagColor;
            }
        }
    }

    /// <summary>
    /// Live spike state → status text (from the real <see cref="SpikeState"/> fields).
    /// Precedence follows the ride flow: riding beats awaiting-board beats arriving beats
    /// the plain run/vehicle states.
    /// </summary>
    private static string BuildLiveStatus()
    {
        if (SpikeState.RideActive)
        {
            if (SpikeState.RideAwaitingDestination)
                return "On board — pick a destination";

<<<<<<< HEAD
            if (SpikeState.RideArrived)
            {
                return SpikeState.NavGaveUp
                    ? "The driver cannot get there — press E to get out"
                    : "Arrived — press E to exit";
            }

            return SpikeState.RideDestinationPicked
                ? $"Riding to {SpikeState.RideDestinationName} — ${FareMeter.ChargedTotal}"
                : $"Riding — ${FareMeter.ChargedTotal}";
        }
        if (SpikeState.RideAwaitingBoard)
            return "Board to ride (E)";
        if (SpikeState.AutoToPlayer || (SpikeState.NavToPlayer && SpikeState.PollingActive))
            return "Taxi arriving";
        if (SpikeState.AutoRunning || SpikeState.PendingSpawnCode != null)
            return "Run in progress";
        if (SpikeState.Vehicle != null)
            return "Taxi active — press STOP or ride along";
        return "No taxi";
    }
}
