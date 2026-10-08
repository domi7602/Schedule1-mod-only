# S1API & Responsive UI Frameworks Guide

> verified: S1API section checked against 3.2.1-beta.8 2026-10-05 (icon policy + clamp curves); Method 3 section since 2026-08-20. Anchor: 0.4.7f9-era evidence; runtime 0.4.7f11 per workspace AGENTS.md.

---

## 1. S1API 3.2.1-beta.8 Architecture

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
    // Ship icon.png in the mod's assets/ folder (Directory.Build.targets must copy it — see phoneapp checklist).
    protected override string IconFileName => "icon.png";
    // No icon file? Override `IconSprite` to return a Sprite directly instead
    // (PotScanner fix — avoids the "Icon file not found" log at runtime).

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

## 2. Method 3 Responsive UI & Scaling Engine

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
* **Clamping**: the TextApp curve clamps `0.85–2.0` (RefHeight 750f), the Dashboard curve `0.75–1.20` (RefHeight 900f) — prevents giant UI on 4K while keeping small screens readable (curves: `schedule1-phoneapp/references/responsive-ui-theme.md`).
* **Do NOT** re-implement the math in a local `UITheme` class — delegate to `S1Mods.Shared.UITheme` (wrappers may add palettes only).

---

## 3. In-Game Console Commands (S1API)

Register custom commands via S1API `BaseConsoleCommand` (accessible in-game via the dev console `~`):
```csharp
using S1API.Console;

[ConsoleCommand("mymod", "Control MyMod settings")]
public class MyModCommand : BaseConsoleCommand
{
    public override void Execute(string[] args)
    {
        // Execute command logic
    }
}
```
