using System;
using MelonLoader;
using S1API.Input;
using S1API.TVApp;
using TVBrowser.Config;
using TVBrowser.UI;
using TVBrowser.UWB;
using UnityEngine;
using UnityEngine.UI;

namespace TVBrowser;

public sealed class TVBrowserApp : S1API.TVApp.TVApp
{
    private static TVBrowserApp? _instance;
    public static TVBrowserApp Instance => _instance ??= new TVBrowserApp();

    private readonly TVBrowserConfig _config;
    private readonly UwbBridge _bridge;

    private GameObject? _containerGo;
    private RawImage? _viewportImage;
    private BrowserNavBar? _navBar;
    private RectTransform? _viewportRectTransform;

    protected override string AppName => "TVBrowser";
    protected override string AppTitle => "Internet & YouTube";
    protected override Sprite Icon => BrowserIconFactory.GetOrCreateIcon();

    public TVBrowserApp()
    {
        _instance = this;
        _config = Mod.Config ?? new TVBrowserConfig();
        _bridge = new UwbBridge(_config.ResolutionWidth, _config.ResolutionHeight);
    }

    protected override void OnCreatedUI(GameObject container)
    {
        _containerGo = container;

        // Initialize responsive TVTheme scaling
        if (container.TryGetComponent<RectTransform>(out var containerRt))
        {
            TVTheme.Initialize(containerRt);
        }

        // 1. App Background
        var bg = container.AddComponent<Image>();
        bg.color = new Color(0.08f, 0.09f, 0.12f, 1f);

        // 2. Top Navigation Bar (0.85 .. 1.0)
        _navBar = new BrowserNavBar(container, _bridge, _config);

        // 3. Viewport (0.0 .. 0.85)
        var viewportGo = new GameObject("TVBrowser_Viewport");
        viewportGo.transform.SetParent(container.transform, false);

        _viewportRectTransform = viewportGo.AddComponent<RectTransform>();
        _viewportRectTransform.anchorMin = new Vector2(0f, 0f);
        _viewportRectTransform.anchorMax = new Vector2(1f, 0.85f);
        _viewportRectTransform.offsetMin = Vector2.zero;
        _viewportRectTransform.offsetMax = Vector2.zero;

        _viewportImage = viewportGo.AddComponent<RawImage>();
        _viewportImage.texture = _bridge.ViewportTexture;
        _viewportImage.color = Color.white;

        MelonLogger.Msg("TV UI initialized successfully.");
    }

    protected override void OnOpened()
    {
        MelonLogger.Msg("TV App opened by player.");

        if (_viewportImage != null)
        {
            _viewportImage.texture = _bridge.ViewportTexture;
        }

        if (_config.AutoLoadHomePage)
        {
            _bridge.StartEngine(_config.HomePage);
        }
        else
        {
            _bridge.StartEngine("https://www.youtube.com");
        }
    }

    protected override void OnClosed()
    {
        MelonLogger.Msg("TV App closed.");
        Controls.IsTyping = false;
        _bridge.StopEngine();
    }

    protected override void OnPaused()
    {
        MelonLogger.Msg("TV App paused.");
    }

    protected override void OnResumed()
    {
        MelonLogger.Msg("TV App resumed.");
    }

    protected override void OnUpdate()
    {
        if (!IsOpen) return;

        // Check for back navigation on Escape (when player is not typing in the URL bar)
        if (UnityEngine.Input.GetKeyDown(KeyCode.Escape))
        {
            if (!Controls.IsTyping)
            {
                Close();
            }
        }

        // Mouse wheel scrolling over viewport with hit-test guard
        float scroll = UnityEngine.Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scroll) > 0.01f && _viewportRectTransform != null)
        {
            if (RectTransformUtility.RectangleContainsScreenPoint(_viewportRectTransform, UnityEngine.Input.mousePosition))
            {
                _bridge.SendScroll(scroll);
            }
        }
    }
}
