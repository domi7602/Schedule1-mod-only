# Real-Time In-World Minigames & CRT Screens — Reference Guide

This reference details how to implement 60 FPS pixel-buffered minigames and interactive video/terminal screens rendered directly onto 3D props in the game world.

---

## 1. High-Performance Pixel Buffer Architecture

Avoid calling `Texture2D.SetPixel` in loops. Instead, operate on a contiguous `Color32[]` buffer in memory:

```csharp
public class CanvasDisplay
{
    private readonly int _width;
    private readonly int _height;
    private readonly Texture2D _texture;
    private readonly Color32[] _pixels;

    public Texture2D Texture => _texture;

    public CanvasDisplay(int width, int height)
    {
        _width = width;
        _height = height;
        _pixels = new Color32[width * height];
        _texture = new Texture2D(width, height, TextureFormat.RGBA32, false)
        {
            filterMode = FilterMode.Point, // Sharp retro arcade pixel art
            wrapMode = TextureWrapMode.Clamp
        };
    }

    public void Clear(Color32 clearColor)
    {
        for (int i = 0; i < _pixels.Length; i++)
            _pixels[i] = clearColor;
    }

    public void SetPixel(int x, int y, Color32 color)
    {
        if (x >= 0 && x < _width && y >= 0 && y < _height)
            _pixels[y * _width + x] = color;
    }

    public void FillRect(int x, int y, int w, int h, Color32 color)
    {
        int xMax = Math.Min(x + w, _width);
        int yMax = Math.Min(y + h, _height);
        for (int py = Math.Max(0, y); py < yMax; py++)
        {
            int rowOffset = py * _width;
            for (int px = Math.Max(0, x); px < xMax; px++)
            {
                _pixels[rowOffset + px] = color;
            }
        }
    }

    public void UploadToGpu()
    {
        _texture.SetPixels32(_pixels);
        _texture.Apply(false); // Do not generate mipmaps!
    }
}
```

---

## 2. In-World Screen Shader & Material

To ensure the screen glows bright even in dark interiors:

```csharp
public static Material CreateScreenMaterial(Texture2D screenTexture)
{
    var shader = Shader.Find("Universal Render Pipeline/Unlit");
    var mat = new Material(shader);
    mat.mainTexture = screenTexture;
    if (mat.HasProperty("_BaseColor"))
        mat.SetColor("_BaseColor", Color.white);
    return mat;
}
```

---

## 3. Mini-Game Lifecycle & Input Capture

```csharp
public void UpdateGame(float dt)
{
    if (!_isFocused) return;

    // 1. Capture Player Input
    if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        MovePaddleLeft();
    if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        MovePaddleRight();

    // 2. Step Physics & AI
    SimulateBall(dt);

    // 3. Redraw
    _display.Clear(Color.black);
    DrawGameElements();
    _display.UploadToGpu();

    // 4. Exit on Escape
    if (Input.GetKeyDown(KeyCode.Escape))
    {
        ExitFocusMode();
    }
}
```
