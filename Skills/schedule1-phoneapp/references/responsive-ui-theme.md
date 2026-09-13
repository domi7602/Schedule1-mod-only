# Methode 3: Responsive UI & Canvas-Scaling (`UITheme`)

In *Schedule I*, S1API instantiates phone app containers on high-resolution uGUI Canvases that are often rotated by 90° (`Quaternion.Euler(0, 0, 90)`). Using fixed integer pixel dimensions (e.g. `14pt` font, `40px` button) results in unreadable, microscopic, or overflowing layouts across different resolutions.

**Methode 3 (`UITheme`)** is the standard mathematical engine used across all verified workspace mods (`NotesApp`, `PotScanner`, `CalculatorApp`, `PocketShop`).

---

## 1. Core UITheme Implementation

**Single source of truth (since 2026-08-20):** `S1Mods.Shared.UITheme` (`Source/Mods/Shared/src/UITheme.cs`) — do NOT copy a local class into your mod. Mod-specific wrappers (e.g. `BankApp.UI.UITheme`, `PocketShop.UI.UITheme`) delegate to it and only add their own color palette:

```csharp
// Text-heavy app (NotesApp/CalculatorApp style):
S1Mods.Shared.UITheme.InitializeForTextApp(containerRt);      // 750f, clamp 0.85–2.0

// Dense dashboard (PotScanner style):
S1Mods.Shared.UITheme.InitializeForDashboard(containerRt);    // 900f, clamp 0.75–1.20

// Custom curve:
S1Mods.Shared.UITheme.Initialize(containerRt, refHeight: 750f, minScale: 0.85f, maxScale: 2.0f);

int sp = S1Mods.Shared.UITheme.Sp(14);
float dp = S1Mods.Shared.UITheme.Dp(36f);
```

For reference, the canonical engine (pre-delegation version) is:

```csharp
using UnityEngine;

public static class UITheme
{
    public const float RefHeight = 750f;
    public const float RefWidth = 400f;

    public static float ActualWidth { get; private set; } = RefWidth;
    public static float ActualHeight { get; private set; } = RefHeight;
    public static float Scale { get; private set; } = 1.0f;

    public static void Initialize(RectTransform containerRt)
    {
        Canvas.ForceUpdateCanvases();
        var r = containerRt.rect;
        float h = Mathf.Max(r.width, r.height);
        float w = Mathf.Min(r.width, r.height);

        if (h > 200f && w > 100f)
        {
            ActualHeight = h;
            ActualWidth = w;

            // Choose appropriate clamping curve for your app style (see §2)
            Scale = Mathf.Clamp(ActualHeight / RefHeight, 0.85f, 2.0f);
        }
        else
        {
            ActualHeight = RefHeight;
            ActualWidth = RefWidth;
            Scale = 1.0f;
        }
    }

    /// <summary>Calculates scale-adjusted font point size (Sp = Scale-adjusted Points).</summary>
    public static int Sp(float pt) => Mathf.RoundToInt(pt * Scale);

    /// <summary>Calculates scale-adjusted pixel dimension or padding (Dp = Density-adjusted Pixels).</summary>
    public static float Dp(float px) => px * Scale;
}
```

---

## 2. Choosing the Right Scale Dampening Factor

Different app types require different scaling curves:

| App Style | Clamping Formula | Why |
|---|---|---|
| **Text-Heavy & Utilities**<br>*(NotesApp, CalculatorApp)* | `Scale = Mathf.Clamp(ActualHeight / 750f, 0.85f, 2.0f)` | Prioritizes maximum readability of notes, numbers, and editor fields. |
| **Dense Dashboards & Grid Lists**<br>*(PotScanner, PocketShop)* | `Scale = Mathf.Clamp(ActualHeight / 900f, 0.75f, 1.20f)` | Prevents oversized rows/cards on high-DPI screens so many rows fit on one screen. |

---

## 3. Applying `UITheme` in UI Builders

### A. Initialization
Call `UITheme.Initialize(...)` at the very start of `OnCreatedUI`:
```csharp
protected override void OnCreatedUI(GameObject container)
{
    var containerRt = container.GetComponent<RectTransform>();
    if (containerRt != null)
    {
        UITheme.Initialize(containerRt);
    }
    // ...
}
```

### B. Standard Component Dimensions Cheat-Sheet
| Component | Height / Size | Font Size | Example Code |
|---|---|---|---|
| **App Header Bar** | `Dp(32f)` – `Dp(36f)` | `Sp(14)` Bold | `le.preferredHeight = UITheme.Dp(36f);` |
| **Top Action Buttons** | `Dp(28f)` – `Dp(32f)` | `Sp(11)` – `Sp(12)` | `le.preferredHeight = UITheme.Dp(30f);` |
| **Filter Pills / Chips** | `Dp(24f)` – `Dp(26f)` | `Sp(10)` Bold | `le.preferredHeight = UITheme.Dp(24f);` |
| **List Row** | `Dp(42f)` – `Dp(48f)` | `Sp(11)` – `Sp(13)` | `le.preferredHeight = UITheme.Dp(46f);` |
| **Grid Card** | `Dp(120f)` – `Dp(180f)` | `Sp(10)` – `Sp(12)` | `le.preferredHeight = UITheme.Dp(150f);` |
| **Modal Dialog** | `Dp(340f) x Dp(300f)` | `Sp(12)` – `Sp(16)` | `cardRt.sizeDelta = new Vector2(UITheme.Dp(340f), UITheme.Dp(300f));` |

### C. Margins & Layout Padding
Always scale `RectOffset` and `spacing`:
```csharp
var vlg = panel.AddComponent<VerticalLayoutGroup>();
vlg.spacing = UITheme.Dp(6f);
vlg.padding = new RectOffset(
    (int)UITheme.Dp(10f), 
    (int)UITheme.Dp(10f), 
    (int)UITheme.Dp(6f), 
    (int)UITheme.Dp(6f)
);
```
