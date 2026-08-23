using System;
using System.Collections.Generic;
using Il2CppTMPro;
using S1API.Utils;
using TVBrowser.Config;
using TVBrowser.Input;
using TVBrowser.UWB;
using UnityEngine;
using UnityEngine.UI;

namespace TVBrowser.UI;

public sealed class BrowserNavBar : IDisposable
{
    private readonly GameObject _root;
    private readonly UwbBridge _bridge;
    private readonly TVBrowserConfig _config;
    private bool _disposed;

    private TMP_InputField _urlInput = null!;
    private Button _backBtn = null!;
    private Button _forwardBtn = null!;
    private Button _reloadBtn = null!;
    private Button _goBtn = null!;
    private TextMeshProUGUI _statusText = null!;
    private Image _statusLed = null!;

    public TVBrowserInputFocus? InputFocusHook { get; private set; }

    public BrowserNavBar(GameObject parent, UwbBridge bridge, TVBrowserConfig config)
    {
        _bridge = bridge;
        _config = config;

        _root = new GameObject("TVBrowser_NavBar");
        _root.transform.SetParent(parent.transform, false);

        var rt = _root.AddComponent<RectTransform>();
        rt.anchorMin = new Vector2(0f, 0.82f);
        rt.anchorMax = new Vector2(1f, 1f);
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;

        // Background (Solid Dark Navy)
        var bg = _root.AddComponent<Image>();
        bg.color = new Color(0.10f, 0.12f, 0.18f, 1f);

        // Bottom Accent Border Line
        var borderGo = new GameObject("BorderLine");
        borderGo.transform.SetParent(_root.transform, false);
        var borderRt = borderGo.AddComponent<RectTransform>();
        borderRt.anchorMin = new Vector2(0f, 0f);
        borderRt.anchorMax = new Vector2(1f, 0.025f);
        borderRt.offsetMin = Vector2.zero;
        borderRt.offsetMax = Vector2.zero;
        var borderImg = borderGo.AddComponent<Image>();
        borderImg.color = new Color(0.25f, 0.55f, 0.98f, 1f);

        BuildNavRow();
        BuildBookmarkRow();

        // Subscribe to bridge events
        _bridge.OnUrlChanged += HandleUrlChanged;
        _bridge.OnStateChanged += HandleStateChanged;
    }

    public void Dispose()
    {
        if (_disposed) return;
        _disposed = true;
        _bridge.OnUrlChanged -= HandleUrlChanged;
        _bridge.OnStateChanged -= HandleStateChanged;
    }

    private void BuildNavRow()
    {
        var navRow = new GameObject("NavRow");
        navRow.transform.SetParent(_root.transform, false);
        var rt = navRow.AddComponent<RectTransform>();
        rt.anchorMin = new Vector2(0f, 0.48f);
        rt.anchorMax = new Vector2(1f, 1f);
        rt.offsetMin = new Vector2(12f, 4f);
        rt.offsetMax = new Vector2(-12f, -6f);

        // 1. Back Button
        _backBtn = CreateNavButton(navRow, "BackBtn", "◄", 0f, 0.045f, () => _bridge.GoBack());

        // 2. Forward Button
        _forwardBtn = CreateNavButton(navRow, "FwdBtn", "►", 0.050f, 0.095f, () => _bridge.GoForward());

        // 3. Reload Button
        _reloadBtn = CreateNavButton(navRow, "ReloadBtn", "🔄", 0.100f, 0.145f, () => _bridge.Reload());

        // 4. Home Button
        CreateNavButton(navRow, "HomeBtn", "🏠", 0.150f, 0.195f, () => _bridge.Navigate(_config.HomePage));

        // 5. URL Input Field
        _urlInput = CreateTmpUrlInputField(navRow, 0.205f, 0.81f);

        // 6. Go Button
        _goBtn = CreateNavButton(navRow, "GoBtn", "➔ GO", 0.815f, 0.895f, () =>
        {
            if (_urlInput != null && !string.IsNullOrWhiteSpace(_urlInput.text))
            {
                _bridge.Navigate(_urlInput.text);
            }
        }, new Color(0.20f, 0.55f, 0.98f, 1f));

        // 7. Status Pill / LED
        CreateStatusIndicator(navRow, 0.905f, 0.995f);

        // Setup input focus hook
        InputFocusHook = _root.AddComponent<TVBrowserInputFocus>();
        InputFocusHook.tmpUrlInput = _urlInput;
        InputFocusHook.OnSubmitUrl = url => _bridge.Navigate(url);
        InputFocusHook.OnEscapePressed = () =>
        {
            if (_urlInput != null)
            {
                _urlInput.DeactivateInputField();
            }
            if (UnityEngine.EventSystems.EventSystem.current != null)
            {
                UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);
            }
        };
    }

    private void BuildBookmarkRow()
    {
        var bookmarkRow = new GameObject("BookmarkRow");
        bookmarkRow.transform.SetParent(_root.transform, false);
        var rt = bookmarkRow.AddComponent<RectTransform>();
        rt.anchorMin = new Vector2(0f, 0f);
        rt.anchorMax = new Vector2(1f, 0.48f);
        rt.offsetMin = new Vector2(12f, 4f);
        rt.offsetMax = new Vector2(-12f, -4f);

        // Label: Quick Bookmarks
        var labelGo = new GameObject("Label");
        labelGo.transform.SetParent(bookmarkRow.transform, false);
        var labelRt = labelGo.AddComponent<RectTransform>();
        labelRt.anchorMin = new Vector2(0f, 0f);
        labelRt.anchorMax = new Vector2(0.10f, 1f);
        labelRt.offsetMin = Vector2.zero;
        labelRt.offsetMax = Vector2.zero;

        var labelText = labelGo.AddComponent<TextMeshProUGUI>();
        labelText.font = FontHelper.GetFont(labelGo.transform);
        labelText.text = "⭐ <b>QUICK:</b>";
        labelText.fontSize = TVTheme.Sp(20);
        labelText.enableWordWrapping = false;
        labelText.color = new Color(0.75f, 0.82f, 0.95f, 1f);
        labelText.alignment = TextAlignmentOptions.MidlineLeft;

        // Bookmark Chips
        var bookmarks = _config.Bookmarks;
        float startX = 0.105f;
        float chipWidth = 0.16f;
        float gap = 0.015f;

        for (int i = 0; i < bookmarks.Count; i++)
        {
            float x0 = startX + (i * (chipWidth + gap));
            float x1 = x0 + chipWidth;
            if (x1 > 0.99f) break;

            var item = bookmarks[i];
            string chipText = $"{item.Icon} <b>{item.Title}</b>";
            string targetUrl = item.Url;

            Color chipBg = item.Title.ToLowerInvariant() switch
            {
                "youtube" => new Color(0.85f, 0.18f, 0.18f, 1f),
                "twitch" => new Color(0.55f, 0.25f, 0.88f, 1f),
                "google" => new Color(0.20f, 0.50f, 0.90f, 1f),
                "wikipedia" => new Color(0.18f, 0.60f, 0.40f, 1f),
                _ => new Color(0.22f, 0.26f, 0.36f, 1f)
            };

            CreateBookmarkChip(bookmarkRow, $"Bookmark_{item.Title}", chipText, x0, x1, () =>
            {
                _bridge.Navigate(targetUrl);
            }, chipBg);
        }
    }

    private Button CreateNavButton(GameObject parent, string name, string label, float anchorLeft, float anchorRight, Action onClick, Color? customBg = null)
    {
        var btnGo = new GameObject(name);
        btnGo.transform.SetParent(parent.transform, false);

        var rt = btnGo.AddComponent<RectTransform>();
        rt.anchorMin = new Vector2(anchorLeft, 0.05f);
        rt.anchorMax = new Vector2(anchorRight, 0.95f);
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;

        var img = btnGo.AddComponent<Image>();
        img.color = customBg ?? new Color(0.22f, 0.26f, 0.36f, 1f);

        var btn = btnGo.AddComponent<Button>();
        btn.targetGraphic = img;
        EventHelper.AddListener(onClick, btn.onClick);

        var textGo = new GameObject("Text");
        textGo.transform.SetParent(btnGo.transform, false);
        var textRt = textGo.AddComponent<RectTransform>();
        textRt.anchorMin = Vector2.zero;
        textRt.anchorMax = Vector2.one;
        textRt.offsetMin = Vector2.zero;
        textRt.offsetMax = Vector2.zero;

        var txt = textGo.AddComponent<TextMeshProUGUI>();
        txt.font = FontHelper.GetFont(textGo.transform);
        txt.text = label;
        txt.fontSize = TVTheme.Sp(24);
        txt.fontStyle = FontStyles.Bold;
        txt.enableWordWrapping = false;
        txt.color = Color.white;
        txt.alignment = TextAlignmentOptions.Center;

        return btn;
    }

    private void CreateBookmarkChip(GameObject parent, string name, string label, float anchorLeft, float anchorRight, Action onClick, Color bgCol)
    {
        var chipGo = new GameObject(name);
        chipGo.transform.SetParent(parent.transform, false);

        var rt = chipGo.AddComponent<RectTransform>();
        rt.anchorMin = new Vector2(anchorLeft, 0.05f);
        rt.anchorMax = new Vector2(anchorRight, 0.95f);
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;

        var img = chipGo.AddComponent<Image>();
        img.color = bgCol;

        var btn = chipGo.AddComponent<Button>();
        btn.targetGraphic = img;
        EventHelper.AddListener(onClick, btn.onClick);

        var textGo = new GameObject("Text");
        textGo.transform.SetParent(chipGo.transform, false);
        var textRt = textGo.AddComponent<RectTransform>();
        textRt.anchorMin = Vector2.zero;
        textRt.anchorMax = Vector2.one;
        textRt.offsetMin = Vector2.zero;
        textRt.offsetMax = Vector2.zero;

        var txt = textGo.AddComponent<TextMeshProUGUI>();
        txt.font = FontHelper.GetFont(textGo.transform);
        txt.text = label;
        txt.fontSize = TVTheme.Sp(20);
        txt.enableWordWrapping = false;
        txt.color = Color.white;
        txt.alignment = TextAlignmentOptions.Center;
    }

    private TMP_InputField CreateTmpUrlInputField(GameObject parent, float anchorLeft, float anchorRight)
    {
        var inputGo = new GameObject("UrlInput");
        inputGo.transform.SetParent(parent.transform, false);

        var rt = inputGo.AddComponent<RectTransform>();
        rt.anchorMin = new Vector2(anchorLeft, 0.05f);
        rt.anchorMax = new Vector2(anchorRight, 0.95f);
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;

        var bg = inputGo.AddComponent<Image>();
        bg.color = new Color(0.06f, 0.07f, 0.10f, 1f);

        var inputField = inputGo.AddComponent<TMP_InputField>();

        // Text Area
        var textAreaGo = new GameObject("TextArea");
        textAreaGo.transform.SetParent(inputGo.transform, false);
        var textAreaRt = textAreaGo.AddComponent<RectTransform>();
        textAreaRt.anchorMin = Vector2.zero;
        textAreaRt.anchorMax = Vector2.one;
        textAreaRt.offsetMin = new Vector2(14f, 2f);
        textAreaRt.offsetMax = new Vector2(-14f, -2f);

        // Text Component
        var textGo = new GameObject("Text");
        textGo.transform.SetParent(textAreaGo.transform, false);
        var textRt = textGo.AddComponent<RectTransform>();
        textRt.anchorMin = Vector2.zero;
        textRt.anchorMax = Vector2.one;
        textRt.offsetMin = Vector2.zero;
        textRt.offsetMax = Vector2.zero;

        var textComp = textGo.AddComponent<TextMeshProUGUI>();
        textComp.font = FontHelper.GetFont(textGo.transform);
        textComp.fontSize = TVTheme.Sp(22);
        textComp.enableWordWrapping = false;
        textComp.overflowMode = TextOverflowModes.Ellipsis;
        textComp.color = Color.white;
        textComp.alignment = TextAlignmentOptions.MidlineLeft;

        // Placeholder
        var placeholderGo = new GameObject("Placeholder");
        placeholderGo.transform.SetParent(textAreaGo.transform, false);
        var phRt = placeholderGo.AddComponent<RectTransform>();
        phRt.anchorMin = Vector2.zero;
        phRt.anchorMax = Vector2.one;
        phRt.offsetMin = Vector2.zero;
        phRt.offsetMax = Vector2.zero;

        var phComp = placeholderGo.AddComponent<TextMeshProUGUI>();
        phComp.font = FontHelper.GetFont(placeholderGo.transform);
        phComp.text = "Search YouTube or enter URL...";
        phComp.fontSize = TVTheme.Sp(22);
        phComp.fontStyle = FontStyles.Italic;
        phComp.enableWordWrapping = false;
        phComp.color = new Color(0.55f, 0.62f, 0.75f, 1f);
        phComp.alignment = TextAlignmentOptions.MidlineLeft;

        inputField.textViewport = textAreaRt;
        inputField.textComponent = textComp;
        inputField.placeholder = phComp;
        inputField.text = _bridge.CurrentUrl;

        return inputField;
    }

    private void CreateStatusIndicator(GameObject parent, float anchorLeft, float anchorRight)
    {
        var statGo = new GameObject("StatusPill");
        statGo.transform.SetParent(parent.transform, false);

        var rt = statGo.AddComponent<RectTransform>();
        rt.anchorMin = new Vector2(anchorLeft, 0.08f);
        rt.anchorMax = new Vector2(anchorRight, 0.92f);
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;

        var bg = statGo.AddComponent<Image>();
        bg.color = new Color(0.16f, 0.19f, 0.28f, 1f);

        // LED dot
        var ledGo = new GameObject("LED");
        ledGo.transform.SetParent(statGo.transform, false);
        var ledRt = ledGo.AddComponent<RectTransform>();
        ledRt.anchorMin = new Vector2(0.08f, 0.30f);
        ledRt.anchorMax = new Vector2(0.24f, 0.70f);
        ledRt.offsetMin = Vector2.zero;
        ledRt.offsetMax = Vector2.zero;

        _statusLed = ledGo.AddComponent<Image>();
        _statusLed.color = new Color(0.25f, 0.88f, 0.45f, 1f);

        // Text
        var textGo = new GameObject("Text");
        textGo.transform.SetParent(statGo.transform, false);
        var textRt = textGo.AddComponent<RectTransform>();
        textRt.anchorMin = new Vector2(0.28f, 0f);
        textRt.anchorMax = new Vector2(0.95f, 1f);
        textRt.offsetMin = Vector2.zero;
        textRt.offsetMax = Vector2.zero;

        _statusText = textGo.AddComponent<TextMeshProUGUI>();
        _statusText.font = FontHelper.GetFont(textGo.transform);
        _statusText.text = "<b>ONLINE</b>";
        _statusText.fontSize = TVTheme.Sp(18);
        _statusText.enableWordWrapping = false;
        _statusText.color = new Color(0.85f, 0.92f, 1f, 1f);
        _statusText.alignment = TextAlignmentOptions.MidlineLeft;
    }

    private void HandleUrlChanged(string newUrl)
    {
        if (_urlInput != null && _urlInput.Pointer != IntPtr.Zero && !_urlInput.WasCollected && !_urlInput.isFocused && _urlInput.text != newUrl)
        {
            _urlInput.text = newUrl;
        }
    }

    private void HandleStateChanged()
    {
        if (_statusText == null || _statusLed == null) return;

        switch (_bridge.State)
        {
            case BrowserEngineState.Running:
                _statusText.text = "<b>ONLINE</b>";
                _statusLed.color = new Color(0.25f, 0.88f, 0.45f, 1f); // Green
                break;
            case BrowserEngineState.Connecting:
                _statusText.text = "<b>CONNECTING</b>";
                _statusLed.color = new Color(0.95f, 0.75f, 0.20f, 1f); // Yellow
                break;
            case BrowserEngineState.Error:
                _statusText.text = "<b>ERROR</b>";
                _statusLed.color = new Color(0.92f, 0.25f, 0.25f, 1f); // Red
                break;
            default:
                _statusText.text = "<b>STANDBY</b>";
                _statusLed.color = new Color(0.55f, 0.60f, 0.70f, 1f); // Gray
                break;
        }
    }
}
