using System;
using System.Collections.Generic;
using System.Text;
using MelonLoader;
using S1API.PhoneApp;
using S1API.UI;
using S1Mods.Shared;
using UnityEngine;
using UnityEngine.UI;

namespace HitmanPhone.Bounty;

/// <summary>
/// Phase E — "Hitman Tracker" phone app: save-scoped history of every hitman
/// contract target with an explicit three-way distinction:
///   • "Out of action"   (knockout/unconscious only),
///   • "Confirmed dead"  (game-confirmed death),
///   • contract state    ("contract completed" is tracked per target and is NOT
///                        treated as proof of death).
/// Read-only; data via <see cref="BountyTrackerService"/>.
/// </summary>
public sealed class BountyTrackerApp : PhoneApp
{
    protected override string AppName => "Hitman Tracker";
    protected override string AppTitle => "Hitman Tracker";
    protected override string IconLabel => "Tracker";
    // Optional drop-in override: place hitman_tracker_icon.png in the game's Mods
    // folder to replace the generated icon (CalculatorApp pattern).
    protected override string IconFileName => "hitman_tracker_icon.png";
    protected override EOrientation Orientation => EOrientation.Vertical;

    private Sprite? _cachedIconSprite;

    // PhoneApp.SpawnIcon prefers IconSprite over IconFileName, so the missing-PNG
    // case stays silent (fixes the "Icon file not found: ...\Mods" error from the
    // empty-filename fallback).
    protected override Sprite IconSprite
    {
        get
        {
            if (_cachedIconSprite != null) return _cachedIconSprite;

            string iconPath = System.IO.Path.Combine(
                MelonLoader.Utils.MelonEnvironment.ModsDirectory, "hitman_tracker_icon.png");
            if (System.IO.File.Exists(iconPath))
            {
                try
                {
                    byte[] data = System.IO.File.ReadAllBytes(iconPath);
                    var tex = new Texture2D(128, 128, TextureFormat.RGBA32, false);
                    if (ImageConversion.LoadImage(tex, data))
                    {
                        _cachedIconSprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
                        _cachedIconSprite.name = "HitmanTrackerIcon";
                        return _cachedIconSprite;
                    }
                }
                catch (System.Exception ex)
                {
                    MelonLoader.MelonLogger.Warning($"Failed to load hitman_tracker_icon.png: {ex.Message}");
                }
            }

            _cachedIconSprite = GenerateProgrammaticIcon();
            return _cachedIconSprite;
        }
    }

    /// <summary>Dark app tile with a hitman-style red crosshair (no asset needed).</summary>
    private static Sprite GenerateProgrammaticIcon()
    {
        const int size = 64;
        var tex = new Texture2D(size, size, TextureFormat.RGBA32, false);
        var px = new Color32[size * size];
        var bg = new Color32(24, 24, 30, 255);
        var fg = new Color32(200, 40, 40, 255);
        var dim = new Color32(90, 90, 100, 255);
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                int dx = x - size / 2, dy = y - size / 2;
                int r2 = dx * dx + dy * dy;
                bool ring = r2 >= 250 && r2 <= 340;
                bool cross = (System.Math.Abs(dx) <= 1 && System.Math.Abs(dy) >= 8 && System.Math.Abs(dy) <= 22)
                          || (System.Math.Abs(dy) <= 1 && System.Math.Abs(dx) >= 8 && System.Math.Abs(dx) <= 22);
                bool dot = r2 <= 4;
                px[y * size + x] = dot || ring ? fg : (cross ? dim : bg);
            }
        }
        tex.SetPixels32(px);
        tex.Apply();
        var sprite = Sprite.Create(tex, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f));
        sprite.name = "HitmanTrackerIcon";
        return sprite;
    }

    private GameObject _mainBG = null!;
    private Text _summaryLabel = null!;
    private Transform _listContent = null!;

    // Fix (Bug-Audit pattern, cf. CalculatorApp/BankApp/PotScanner): the phone
    // re-instantiates this app per scene load and per-instance MelonEvents.OnUpdate
    // handlers stay in the static invocation list forever. Dispatch through _active,
    // subscribed exactly once; Mod.OnSceneWasUnloaded clears _active on teardown.
    private static BountyTrackerApp? _active;
    private static bool _staticSubscribed;

    protected override void OnCreated()
    {
        base.OnCreated();
        _active = this;
        if (_staticSubscribed) return;
        _staticSubscribed = true;
        MelonEvents.OnUpdate.Subscribe(DispatchUpdate);
    }

    internal static void TearDownForSceneUnload()
    {
        _active = null;
    }

    private static void DispatchUpdate()
    {
        var a = _active;
        if (a != null)
        {
            try { a.Update(); } catch { /* UI may be torn down */ }
        }
    }

    protected override void OnCreatedUI(GameObject container)
    {
        var containerRt = container.GetComponent<RectTransform>();
        if (containerRt != null)
        {
            UITheme.InitializeForTextApp(containerRt);
        }

        // 1. Isolated background panel (Rule 3) — starts hidden.
        _mainBG = UIFactory.Panel("Tracker_MainBG", container.transform, GamePalette.Bg, fullAnchor: true);
        _mainBG.SetActive(false);

        var vlg = _mainBG.AddComponent<VerticalLayoutGroup>();
        vlg.childControlWidth = true;
        vlg.childControlHeight = true;
        vlg.childForceExpandWidth = true;
        vlg.childForceExpandHeight = false;
        vlg.padding = new RectOffset(
            (int)UITheme.Dp(12f), (int)UITheme.Dp(12f),
            (int)UITheme.Dp(10f), (int)UITheme.Dp(10f));
        vlg.spacing = UITheme.Dp(6f);

        // 2. Header
        var title = UIFactory.Text("Title", "HITMAN TRACKER", _mainBG.transform,
            UITheme.Sp(15), TextAnchor.MiddleLeft, FontStyle.Bold);
        title.color = GamePalette.TextPrimary;
        var tle = title.gameObject.AddComponent<LayoutElement>();
        tle.minHeight = UITheme.Dp(30f);
        tle.preferredHeight = UITheme.Dp(30f);
        tle.flexibleHeight = 0f;
        tle.layoutPriority = 1;

        // 3. Summary line (neutral metadata, Rule 20)
        _summaryLabel = UIFactory.Text("Summary", "No contract history yet.", _mainBG.transform,
            UITheme.Sp(11), TextAnchor.MiddleLeft, FontStyle.Normal);
        _summaryLabel.color = GamePalette.TextMuted;
        var sle = _summaryLabel.gameObject.AddComponent<LayoutElement>();
        sle.minHeight = UITheme.Dp(20f);
        sle.preferredHeight = UITheme.Dp(20f);
        sle.flexibleHeight = 0f;
        sle.layoutPriority = 1;

        // 4. Scrollable target list (Rule 13: fix content rect + control width)
        UIFactory.ScrollableVerticalList("Tracker_List", _mainBG.transform, out _);
        // The helper parents the content under its own scroll rect; find it.
        var scrollRect = _mainBG.GetComponentInChildren<ScrollRect>(true);
        if (scrollRect != null)
        {
            var viewport = scrollRect.viewport != null ? scrollRect.viewport : scrollRect.transform;
            _listContent = scrollRect.content != null ? scrollRect.content : viewport;
            var contentLayout = _listContent.GetComponent<VerticalLayoutGroup>();
            if (contentLayout == null) contentLayout = _listContent.gameObject.AddComponent<VerticalLayoutGroup>();
            contentLayout.childControlWidth = true;
            contentLayout.childControlHeight = true;
            contentLayout.childForceExpandWidth = true;
            contentLayout.childForceExpandHeight = false;
            contentLayout.spacing = UITheme.Dp(4f);

            var rect = _listContent as RectTransform;
            if (rect != null)
            {
                rect.anchorMin = new Vector2(0f, 1f);
                rect.anchorMax = new Vector2(1f, 1f);
                rect.pivot = new Vector2(0.5f, 1f);
                rect.sizeDelta = new Vector2(0f, 0f);
            }
            var fitter = _listContent.GetComponent<ContentSizeFitter>();
            if (fitter == null) fitter = _listContent.gameObject.AddComponent<ContentSizeFitter>();
            fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
            fitter.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
        }
    }

    private void Update()
    {
        if (_mainBG == null || _mainBG.WasCollected) return;

        bool open = IsOpen();
        if (_mainBG.activeSelf != open)
        {
            _mainBG.SetActive(open);
            if (open) OnAppOpened();
        }

        if (!open) return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CloseApp();
        }
    }

    private void OnAppOpened()
    {
        Rebuild();
    }

    /// <summary>Rebuild the summary and the target rows from the current save.</summary>
    private void Rebuild()
    {
        try
        {
            if (_listContent == null || _listContent.WasCollected) return;
            var save = Mod.Instance?.Save;

            // Clear previous rows (children only — never the container itself).
            for (int i = _listContent.childCount - 1; i >= 0; i--)
            {
                var child = _listContent.GetChild(i);
                if (child != null && !child.WasCollected) UnityEngine.Object.Destroy(child.gameObject);
            }

            if (save == null)
            {
                _summaryLabel.text = "No contract history yet.";
                return;
            }

            var snap = BountyTrackerService.Build(save);
            _summaryLabel.text = snap.Rows.Count == 0
                ? "No contract history yet."
                : $"{snap.Rows.Count} targets \u00b7 {snap.ConfirmedDead} confirmed dead \u00b7 " +
                  $"{snap.OnlyOut} out of action \u00b7 {snap.ContractsCompleted} contracts completed";

            for (int i = 0; i < snap.Rows.Count; i++)
            {
                BuildRow(snap.Rows[i]);
            }

            var rect = _listContent as RectTransform;
            if (rect != null) rect.sizeDelta = new Vector2(0f, rect.sizeDelta.y);
        }
        catch (Exception ex)
        {
            Mod.Log.Warn($"[TrackerApp] Rebuild failed: {ex.Message}");
            if (_summaryLabel != null) _summaryLabel.text = "History unavailable right now.";
        }
    }

    private void BuildRow(BountyTrackerService.Row row)
    {
        var card = UIFactory.Panel("Tracker_Row", _listContent, GamePalette.Card, fullAnchor: false);
        var hlg = card.AddComponent<HorizontalLayoutGroup>();
        hlg.childControlWidth = true;
        hlg.childControlHeight = true;
        hlg.childForceExpandWidth = false;
        hlg.childForceExpandHeight = true;
        hlg.padding = new RectOffset(
            (int)UITheme.Dp(8f), (int)UITheme.Dp(8f),
            (int)UITheme.Dp(5f), (int)UITheme.Dp(5f));
        hlg.spacing = UITheme.Dp(6f);
        var cle = card.AddComponent<LayoutElement>();
        cle.minHeight = UITheme.Dp(40f);
        cle.flexibleHeight = 0f;
        cle.layoutPriority = 1;

        // Left: target name + event time
        var left = new GameObject("Left");
        left.AddComponent<RectTransform>();
        left.transform.SetParent(card.transform, false);
        var llg = left.AddComponent<VerticalLayoutGroup>();
        llg.childControlWidth = true;
        llg.childControlHeight = true;
        llg.childForceExpandWidth = true;
        llg.childForceExpandHeight = false;
        var lle = left.AddComponent<LayoutElement>();
        lle.flexibleWidth = 1f;

        var name = UIFactory.Text("Name", row.TargetName, left.transform,
            UITheme.Sp(12), TextAnchor.MiddleLeft, FontStyle.Bold);
        name.color = GamePalette.TextPrimary;

        string timeText = row.Day > 0 || row.MinuteSum > 0
            ? BountyTrackerService.FormatEventTime(row.Day, row.MinuteSum)
            : "No elimination recorded";
        if (row.ContractCompleted) timeText += "  \u00b7  contract completed";
        var time = UIFactory.Text("Time", timeText, left.transform,
            UITheme.Sp(10), TextAnchor.MiddleLeft, FontStyle.Normal);
        time.color = GamePalette.TextMuted;

        // Right: the explicit status distinction (Rule 20: plain words, no debug)
        string statusText = row.Died ? "Confirmed dead" : "Out of action";
        var status = UIFactory.Text("Status", statusText, card.transform,
            UITheme.Sp(10), TextAnchor.MiddleRight, FontStyle.Bold);
        status.color = row.Died ? GamePalette.Red : GamePalette.Orange;
        var sre = status.gameObject.AddComponent<LayoutElement>();
        sre.preferredWidth = UITheme.Dp(96f);
        sre.flexibleWidth = 0f;
    }

    protected override void OnPhoneClosed()
    {
        base.OnPhoneClosed();
        if (_mainBG != null) _mainBG.SetActive(false);
        // Rule 10/2: never destroy UI or unsubscribe here.
    }
}
