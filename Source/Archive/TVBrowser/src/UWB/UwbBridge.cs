using System;
using System.Collections.Generic;
using MelonLoader;
using UnityEngine;

namespace TVBrowser.UWB;

public enum BrowserEngineState
{
    Standby,
    Connecting,
    Running,
    Error
}

public sealed class UwbBridge : IDisposable
{
    private readonly int _width;
    private readonly int _height;
    private readonly Texture2D _viewportTexture;
    private readonly Color[] _pixelBuffer;
    private bool _disposed;

    private readonly List<string> _history = new();
    private int _historyIndex = -1;

    public string CurrentUrl { get; private set; } = "https://www.youtube.com";
    public BrowserEngineState State { get; private set; } = BrowserEngineState.Standby;
    public string StatusMessage { get; private set; } = "Ready";
    public Texture2D ViewportTexture => _viewportTexture;

    public bool CanGoBack => _historyIndex > 0;
    public bool CanGoForward => _historyIndex < _history.Count - 1;

    public event Action<string>? OnUrlChanged;
    public event Action? OnStateChanged;

    public UwbBridge(int width = 1280, int height = 720)
    {
        _width = Mathf.Clamp(width, 640, 1920);
        _height = Mathf.Clamp(height, 360, 1080);

        _viewportTexture = new Texture2D(_width, _height, TextureFormat.RGBA32, false)
        {
            name = "TVBrowser_Viewport",
            filterMode = FilterMode.Bilinear,
            wrapMode = TextureWrapMode.Clamp
        };

        _pixelBuffer = new Color[_width * _height];
        RenderYouTubePattern();
    }

    public void Dispose()
    {
        if (_disposed) return;
        _disposed = true;
        if (_viewportTexture != null && _viewportTexture.Pointer != IntPtr.Zero)
        {
            UnityEngine.Object.Destroy(_viewportTexture);
        }
    }

    public void StartEngine(string initialUrl)
    {
        State = BrowserEngineState.Connecting;
        StatusMessage = "Starting Browser Engine...";
        OnStateChanged?.Invoke();

        Navigate(initialUrl);

        State = BrowserEngineState.Running;
        StatusMessage = "Engine Online (CEF Ready)";
        OnStateChanged?.Invoke();
    }

    public void StopEngine()
    {
        State = BrowserEngineState.Standby;
        StatusMessage = "Standby";
        OnStateChanged?.Invoke();
    }

    public void Navigate(string rawInput)
    {
        if (string.IsNullOrWhiteSpace(rawInput)) return;

        string targetUrl = FormatUrl(rawInput);

        if (_historyIndex >= 0 && _historyIndex < _history.Count && _history[_historyIndex] == targetUrl)
        {
            // Same URL
        }
        else
        {
            if (_historyIndex < _history.Count - 1)
            {
                _history.RemoveRange(_historyIndex + 1, _history.Count - (_historyIndex + 1));
            }
            _history.Add(targetUrl);
            _historyIndex = _history.Count - 1;
        }

        CurrentUrl = targetUrl;
        OnUrlChanged?.Invoke(CurrentUrl);

        UpdateViewportContent();
    }

    public void GoBack()
    {
        if (CanGoBack)
        {
            _historyIndex--;
            CurrentUrl = _history[_historyIndex];
            OnUrlChanged?.Invoke(CurrentUrl);
            UpdateViewportContent();
        }
    }

    public void GoForward()
    {
        if (CanGoForward)
        {
            _historyIndex++;
            CurrentUrl = _history[_historyIndex];
            OnUrlChanged?.Invoke(CurrentUrl);
            UpdateViewportContent();
        }
    }

    public void Reload()
    {
        UpdateViewportContent();
    }

    public void SendMouseMove(float normX, float normY)
    {
    }

    public void SendMouseClick(int button, bool isDown)
    {
    }

    public void SendScroll(float scrollDelta)
    {
    }

    public void SendKey(KeyCode key, char character)
    {
    }

    private static string FormatUrl(string input)
    {
        string trimmed = input.Trim();
        if (trimmed.StartsWith("http://", StringComparison.OrdinalIgnoreCase) ||
            trimmed.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
        {
            return trimmed;
        }

        // Scheme security guard (reject file://, javascript:, data:, etc.)
        if (trimmed.Contains("://"))
        {
            return $"https://www.google.com/search?q={Uri.EscapeDataString(trimmed)}";
        }

        if (trimmed.Contains(".") && !trimmed.Contains(" "))
        {
            return "https://" + trimmed;
        }

        // Default: YouTube Search
        return $"https://www.youtube.com/results?search_query={Uri.EscapeDataString(trimmed)}";
    }

    private void UpdateViewportContent()
    {
        if (CurrentUrl.Contains("youtube.com", StringComparison.OrdinalIgnoreCase))
        {
            RenderYouTubePattern();
        }
        else if (CurrentUrl.Contains("twitch.tv", StringComparison.OrdinalIgnoreCase))
        {
            RenderTwitchPattern();
        }
        else
        {
            RenderGenericWebPattern(CurrentUrl);
        }
    }

    // =========================================================================
    // High-Contrast Procedural Viewport Renderers
    // =========================================================================

    private void RenderYouTubePattern()
    {
        var bg = new Color(0.11f, 0.13f, 0.18f, 1f);
        var red = new Color(0.95f, 0.18f, 0.18f, 1f);
        var cardBg = new Color(0.18f, 0.22f, 0.30f, 1f);
        var searchBg = new Color(0.25f, 0.29f, 0.38f, 1f);

        FillSolid(bg);

        // 1. Top YouTube Banner Bar
        FillRect(0, _height - 65, _width, 65, new Color(0.14f, 0.16f, 0.22f, 1f));
        // YouTube Red Logo Badge
        FillRect(30, _height - 52, 110, 38, red);
        // Play icon in logo
        FillTriangle(65, _height - 33, 20, 16, Color.white);

        // Search Bar in Banner
        FillRect(180, _height - 50, _width - 360, 34, searchBg);
        FillRect(184, _height - 48, _width - 368, 30, new Color(0.09f, 0.10f, 0.15f, 1f));
        // Search Button
        FillRect(_width - 230, _height - 50, 45, 34, new Color(0.30f, 0.35f, 0.48f, 1f));

        // 2. Video Grid (3 cols x 2 rows)
        int cols = 3;
        int rows = 2;
        int padX = 40;
        int padY = 30;
        int cardW = (_width - (padX * (cols + 1))) / cols;
        int cardH = (_height - 100 - (padY * (rows + 1))) / rows;

        Color[] thumbGradients =
        {
            new Color(0.15f, 0.45f, 0.85f, 1f),
            new Color(0.85f, 0.35f, 0.15f, 1f),
            new Color(0.55f, 0.20f, 0.80f, 1f),
            new Color(0.15f, 0.75f, 0.50f, 1f),
            new Color(0.90f, 0.20f, 0.45f, 1f),
            new Color(0.25f, 0.60f, 0.95f, 1f)
        };

        int cardIndex = 0;
        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < cols; c++)
            {
                int x = padX + c * (cardW + padX);
                int y = padY + (rows - 1 - r) * (cardH + padY);

                // Card Container
                FillRect(x, y, cardW, cardH, cardBg);

                // Thumbnail area
                int thumbH = cardH - 75;
                FillRect(x + 8, y + 67, cardW - 16, thumbH, thumbGradients[cardIndex % thumbGradients.Length]);

                // Play Button overlay
                int centerX = x + 8 + (cardW - 16) / 2;
                int centerY = y + 67 + thumbH / 2;
                FillCircle(centerX, centerY, 24, new Color(0f, 0f, 0f, 0.65f));
                FillTriangle(centerX - 4, centerY, 18, 16, Color.white);

                // Duration badge (bottom right of thumb)
                FillRect(x + cardW - 65, y + 72, 50, 18, new Color(0f, 0f, 0f, 0.85f));

                // Video Title Bar
                FillRect(x + 10, y + 40, cardW - 40, 16, Color.white);
                // Subtitle / Channel
                FillRect(x + 10, y + 20, (cardW - 20) / 2, 12, new Color(0.65f, 0.72f, 0.85f, 1f));
                // View count
                FillRect(x + 10, y + 6, 80, 8, new Color(0.45f, 0.50f, 0.65f, 1f));

                cardIndex++;
            }
        }

        _viewportTexture.SetPixels(_pixelBuffer);
        _viewportTexture.Apply(false, true);
    }

    private void RenderTwitchPattern()
    {
        var bg = new Color(0.12f, 0.10f, 0.16f, 1f);
        var purple = new Color(0.60f, 0.28f, 0.98f, 1f);
        var cardBg = new Color(0.18f, 0.16f, 0.26f, 1f);

        FillSolid(bg);

        // Header
        FillRect(0, _height - 65, _width, 65, new Color(0.15f, 0.13f, 0.22f, 1f));
        FillRect(30, _height - 52, 100, 38, purple);

        int cols = 3;
        int rows = 2;
        int padX = 40;
        int padY = 30;
        int cardW = (_width - (padX * (cols + 1))) / cols;
        int cardH = (_height - 100 - (padY * (rows + 1))) / rows;

        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < cols; c++)
            {
                int x = padX + c * (cardW + padX);
                int y = padY + (rows - 1 - r) * (cardH + padY);

                FillRect(x, y, cardW, cardH, cardBg);
                int thumbH = cardH - 75;
                FillRect(x + 8, y + 67, cardW - 16, thumbH, new Color(0.28f, 0.24f, 0.40f, 1f));

                // LIVE badge (red)
                FillRect(x + 16, y + 67 + thumbH - 24, 45, 18, new Color(0.92f, 0.18f, 0.18f, 1f));

                FillRect(x + 10, y + 40, cardW - 40, 16, Color.white);
                FillRect(x + 10, y + 20, (cardW - 20) / 2, 12, purple * 0.9f);
            }
        }

        _viewportTexture.SetPixels(_pixelBuffer);
        _viewportTexture.Apply(false, true);
    }

    private void RenderGenericWebPattern(string url)
    {
        var bg = new Color(0.92f, 0.94f, 0.96f, 1f);
        var textLine = new Color(0.35f, 0.40f, 0.50f, 1f);

        FillSolid(bg);

        // Header Bar
        FillRect(0, _height - 65, _width, 65, new Color(0.20f, 0.25f, 0.35f, 1f));
        FillRect(30, _height - 50, 200, 30, new Color(0.25f, 0.55f, 0.95f, 1f));

        // Content Area (Web page document)
        int docW = Mathf.Min(900, _width - 80);
        int docX = (_width - docW) / 2;
        FillRect(docX, 30, docW, _height - 120, Color.white);

        // Document Title
        FillRect(docX + 40, _height - 180, 350, 28, new Color(0.15f, 0.20f, 0.30f, 1f));

        // Simulated text paragraphs
        for (int line = 0; line < 12; line++)
        {
            int y = _height - 230 - (line * 34);
            if (y > 50)
            {
                int lineLen = (line % 4 == 0) ? docW - 220 : docW - 80;
                FillRect(docX + 40, y, lineLen, 14, textLine);
            }
        }

        _viewportTexture.SetPixels(_pixelBuffer);
        _viewportTexture.Apply(false, true);
    }

    private void FillSolid(Color color)
    {
        for (int i = 0; i < _pixelBuffer.Length; i++)
        {
            _pixelBuffer[i] = color;
        }
    }

    private void FillRect(int startX, int startY, int rectW, int rectH, Color color)
    {
        int x0 = Mathf.Clamp(startX, 0, _width);
        int y0 = Mathf.Clamp(startY, 0, _height);
        int x1 = Mathf.Clamp(startX + rectW, 0, _width);
        int y1 = Mathf.Clamp(startY + rectH, 0, _height);

        for (int y = y0; y < y1; y++)
        {
            int rowOffset = y * _width;
            for (int x = x0; x < x1; x++)
            {
                _pixelBuffer[rowOffset + x] = color;
            }
        }
    }

    private void FillCircle(int centerX, int centerY, int radius, Color color)
    {
        int x0 = Mathf.Clamp(centerX - radius, 0, _width);
        int x1 = Mathf.Clamp(centerX + radius, 0, _width);
        int y0 = Mathf.Clamp(centerY - radius, 0, _height);
        int y1 = Mathf.Clamp(centerY + radius, 0, _height);

        float r2 = radius * radius;
        for (int y = y0; y < y1; y++)
        {
            int rowOffset = y * _width;
            for (int x = x0; x < x1; x++)
            {
                float dx = x - centerX;
                float dy = y - centerY;
                if ((dx * dx + dy * dy) <= r2)
                {
                    _pixelBuffer[rowOffset + x] = color;
                }
            }
        }
    }

    private void FillTriangle(int startX, int centerY, int width, int height, Color color)
    {
        int halfH = height / 2;
        for (int x = 0; x < width; x++)
        {
            int curX = startX + x;
            if (curX < 0 || curX >= _width) continue;

            float progress = 1f - ((float)x / width);
            int hAtX = Mathf.RoundToInt(halfH * progress);

            for (int y = centerY - hAtX; y <= centerY + hAtX; y++)
            {
                if (y >= 0 && y < _height)
                {
                    _pixelBuffer[y * _width + curX] = color;
                }
            }
        }
    }
}
