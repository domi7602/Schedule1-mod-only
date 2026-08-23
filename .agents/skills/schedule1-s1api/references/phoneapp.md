# S1API — PhoneApp (In-Game Smartphone UI)

The `PhoneApp` base class turns your mod into a fully integrated app on the in-game phone home screen. Auto-discovered, lifecycle-managed, and branch-portable.

For the full PhoneApp **runbook** (lifecycle, UI patterns, InputFocus, lifecycle sync), see the `schedule1-phoneapp` skill. This reference is the **S1API-specific catalog** of the PhoneApp base class.

---

## 1. The Class

```csharp
using MelonLoader;
using S1API.PhoneApp;
using S1API.UI;
using S1API.Utils;
using UnityEngine;

public sealed class MyApp : PhoneApp
{
    protected override string AppName      => "MyApp";        // unique ID
    protected override string AppTitle     => "My App";       // display name
    protected override string IconLabel    => "My";           // text below icon
    protected override string IconFileName => "myapp_icon.png";  // in Mods/Plugins folder
    protected override EOrientation Orientation => EOrientation.Vertical;

    protected override void OnCreated()
    {
        base.OnCreated();
        // Subscribe to MelonEvents.OnUpdate here — idempotent (OnCreated fires ONCE per scene):
        // MelonEvents.OnUpdate.Unsubscribe(Update);
        // MelonEvents.OnUpdate.Subscribe(Update);
    }

    protected override void OnCreatedUI(GameObject container)
    {
        var panel = UIFactory.Panel("MainPanel", container.transform,
            new Color(0.1f, 0.1f, 0.1f), fullAnchor: true);

        UIFactory.Text("Title", "📱 Hello, S1API!", panel.transform, 22,
            TextAnchor.MiddleCenter);

        var (maskGO, button, label) = UIFactory.RoundedButtonWithLabel(
            "MyButton", "Click Me", panel.transform,
            new Color(0.2f, 0.5f, 0.3f), 140, 40, 18, Color.white);

        ButtonUtils.AddListener(button, () => Log.Msg("Clicked!"));
    }

    protected override void OnPhoneClosed()
    {
        base.OnPhoneClosed();
        // Cleanup here — never destroy UI!
    }
}
```

---

## 2. Required Properties

| Property | Type | Required | Notes |
|---|---|---|---|
| `AppName` | `string` | ✅ | Unique ID (used in auto-discovery) |
| `AppTitle` | `string` | ✅ | Displayed name on the phone |
| `IconLabel` | `string` | ✅ | Short text under the home icon |
| `IconFileName` | `string` | ✅ | PNG filename in `Mods/` (S1API loads it) |
| `Orientation` | `EOrientation` | ⚠ often | `Vertical` for portrait, `Horizontal` for landscape |

### Icon Override (NPE-safe)

```csharp
protected override string IconFileName => "";  // disable S1API file lookup
protected override Sprite? IconSprite
{
    get
    {
        // Programmatically build a 64x64 sprite
        var tex = new Texture2D(64, 64);
        // ... fill pixels
        return Sprite.Create(tex, new Rect(0, 0, 64, 64), Vector2.one * 0.5f);
    }
}
```

S1API prefers `IconSprite` over `IconFileName` — checked first.

---

## 3. Lifecycle Hooks

```csharp
protected override void OnCreated()          // register hooks, subscribe events
protected override void OnCreatedUI(GameObject container)  // build UI here (once)
protected override void OnPhoneClosed()      // cleanup, never destroy UI
protected override void OnDestroyed()        // final cleanup
```

> **Golden Rule:** NEVER destroy UI GameObjects in `OnPhoneClosed()`. The "Transparent Phone" bug occurs when the user re-opens the app and the 3D phone model renders with empty children.
>
> **Rule 10 (2026-08-20):** NEVER `Unsubscribe(MelonEvents.OnUpdate)` / `-=` static events in `OnPhoneClosed()` — `OnCreated` fires only once per scene, the app goes blank after the first close. Teardown goes in `OnDestroyed()`.

---

## 4. `IsOpen()` + Manual Close

```csharp
private void Update()
{
    bool open = IsOpen();   // checks PlayerSingleton<Phone>.Instance.IsOpen
    // ...
}

protected override void OnPhoneClosed()
{
    base.OnPhoneClosed();
    // Don't destroy UI here!
}

public void CloseApp()
{
    Exit(ExitAction.Default);   // or ExitAction.ReturnToHomeScreen
}
```

---

## 5. `UIFactory` Cheat Sheet

| Method | Returns | Notes |
|---|---|---|
| `Panel(name, parent, color)` | `GameObject` | uGUI panel with `Image` |
| `Panel(name, parent, color, fullAnchor:true)` | `GameObject` | Full-screen-anchored panel |
| `Text(name, parent, fontSize, color, alignment)` | `GameObject` | Text mesh |
| `RoundedButtonWithLabel(name, label, parent, color, w, h, fontSize, labelColor)` | `(GameObject, Button, Text)` | Pill-shaped button |
| `InputField(name, parent, w, h, placeholder)` | `InputField` | Text input |
| `ScrollableVerticalList(name, parent, spacing)` | `ScrollRect` | Pre-wired scroll rect |

**Pitfall:** `UIFactory.Panel` already sets an `Image` — `AddComponent<Image>()` afterwards creates a duplicate and triggers an NPE in IL2CPP. Use `GetComponent<Image>() ?? AddComponent<Image>()`.

For the full UI patterns + InputFocus + canvas, see the `schedule1-phoneapp` skill.

---

## 6. Auto-Discovery Mechanism

S1API uses `S1API.Internal.Patches.HomeScreen_Start_Patch` (a Harmony patch) to:
1. Reflect over all loaded assemblies for `PhoneApp` subclasses
2. Instantiate one of each
3. Call `OnCreated()` → register with `PhoneAppRegistry`
4. Build the home-icon via `SpawnIcon()` / `SpawnUI()`

You don't need to register manually — just place your class in the mod DLL.

---

## 7. S1API vs ScheduleOne-Hash

If you want to register a `phone` command in `hash` (DooDesch's terminal), it lives in a parallel plugin-system, **not** in S1API:

```csharp
public override void OnInitializeMelon()
{
    if (HashCommands.Available)
    {
        HashCommands.Add("mycmd", "My commands", "mycmd hello");
    }
}
```

For the full hash integration, see `Knowledge/Frameworks/ScheduleOne-Hash/Architecture.md`.
