# S1API & Responsive UI Frameworks Guide

---

## 1. S1API 3.2.0 Architecture

S1API provides high-level abstractions for Schedule I:
* **Phone Applications**: `PhoneApp`, `PhoneAppRegistry`
* **UI Components**: `UIFactory`, `TopBar`, `ScrollableVerticalList`
* **Game Services**: `Money`, `Property`, `Growing`, `Dialogue`, `Console / BaseConsoleCommand`

### Creating a Phone App via S1API:
```csharp
using S1API.PhoneApp;
using S1API.UI;

public sealed class MyPhoneApp : PhoneApp
{
    protected override string AppName => "my_phone_app";
    protected override string AppTitle => "My App";
    protected override string IconLabel => "My";
    protected override string IconFileName => "";

    protected override void OnCreatedUI(GameObject container)
    {
        S1Mods.Shared.UITheme.InitializeForTextApp(container.GetComponent<RectTransform>());
        // build UI ...
    }
}
```

> ⚠ **Lifecycle (2026-08-20):** `OnCreated` fires ONCE per scene. NEVER `Unsubscribe(MelonEvents.OnUpdate)` in `OnPhoneClosed` — app goes blank after 1st close. See `schedule1-phoneapp` Rule 10.

> ⚠ **Slot-Isolation (2026-08-21):** PhoneApps saving to `UserData/<Mod>/state.json` (global) leak Slot-A state into Slot-B. Use `LoadManager.Instance.ActiveSaveInfo.SaveSlotNumber` → `slot_{n}.json` + `TryMigrateLegacy` (see `CalculatorState.cs:65-93`).

---

## 2. Methode 3 Responsive UI & Scaling Engine

Schedule I phone screens and HUD overlays must scale gracefully across diverse screen resolutions (1080p, 1440p, 4K, ultrawide).

### The Dynamic Scaling Formula (single source of truth: `S1Mods.Shared.UITheme`):
```csharp
using S1Mods.Shared;

// Text-heavy app:      S1Mods.Shared.UITheme.InitializeForTextApp(containerRt);   // 750f, 0.85–2.0
// Dense dashboard:     S1Mods.Shared.UITheme.InitializeForDashboard(containerRt); // 900f, 0.75–1.20

float dp = S1Mods.Shared.UITheme.Dp(36f);   // heights, paddings, margins
int   sp = S1Mods.Shared.UITheme.Sp(14);    // fonts
```

* **Buttons & Row Heights**: Use `Dp(...)` for heights, paddings, margins.
* **Text / Fonts**: Use `Sp(...)` for `TMP_Text.fontSize`.
* **Clamping**: Clamping between `0.75f` and `1.20f` prevents giant UI elements on 4K screens while keeping elements readable on smaller displays.
* **Do NOT** re-implement the math in a local `UITheme` class — delegate to `S1Mods.Shared.UITheme` (wrappers may add palettes only).

---

## 3. Console & Hash Terminal Commands

Register commands both in S1API and Hash Terminal (DooDesch console):
```csharp
// S1API Console Command
[ConsoleCommand("mymod", "Control MyMod settings")]
public class MyModCommand : BaseConsoleCommand
{
    public override void Execute(string[] args)
    {
        // Execute command logic
    }
}
```
