# PhoneScroll — ThirdParty Mod

**Version:** 1.4
**Author:** V4LEXL (NexusMods)
**Source:** NexusMods — external download (closed source)

## Purpose

PhoneScroll fixes the vanilla Phone HomeScreen, which **was not built for lists**: it injects a `ScrollViewport`/`Mask` component around `HomeScreen/AppIcons` and scrolls the app icons via the mouse wheel.

## Behavior (Decompile Highlights, ilspycmd 8.2.0)

- **Hook:** Harmony postfix on `HomeScreen.Start` (Priority 800 / `Priority.First`)
- **Setup:** builds `ScrollViewport` as a full-screen RectTransform with `Mask`, reparents `AppIcons` into it, sets `localPosition.y += 120f`
- **Hardening:** every frame (throttled to 1s) sets `MaskableGraphic.maskable = true` and `Canvas.overrideSorting = false` on all icon children — so the mask clips cleanly
- **Scroll:** `OnUpdate` reads `Input.mouseScrollDelta.y * ScrollSpeed` (default **30**), clamps to `[0, maxOffset]`, sets `AppIcons.localPosition.y = originalY + scrollOffset`

## Known Issues

### Vibrating Scroll

`ScrollSpeed = 30` is too high for a 30-pixel mouse-wheel tick. **Recommended adjustment** in `<GameDir>\UserData\MelonPreferences.cfg` (section `[PhoneScroll]`):

```ini
[PhoneScroll]
Enabled = True
ScrollSpeed = 5
DebugLog = False
```

5–10 is a good starting value. Lower = smoother scrolling, larger jump per tick. Try what feels best for you.

### Conflict with S1API PhoneApps

PhoneScroll only listens to `Input.mouseScrollDelta.y` — it does not filter by focus. When an S1API PhoneApp is open (NotesApp, CalculatorApp, PotScanner, PocketShop, BankApp) and operates its own `ScrollRect`, **both systems react to the same mouse-wheel tick**. Result: the S1API PhoneApp scrolls internally while PhoneScroll moves the HomeScreen app icons — perceived as "vibration".

Workaround: set each mod's PhoneApp ScrollSensitivity low (see in-game findings from 2026-09-10).

## Deploy

PhoneScroll is **automatically** copied via `Tools/deploy-thirdparty.ps1` (invoked from `Source/Mods/Directory.Build.targets` → `DeployThirdParty`) to `<GameDir>\Mods\PhoneScroll.dll`. No manual steps required — a `dotnet build` of the solution is enough.

Whitelist via `ThirdParty/.deployignore`: everything in `ThirdParty/S1API/`, `S1MAPI/`, `S1MCPServer-master/`, `ScheduleOne-Sideload/`, `ScheduleOne-Hash/`, `MoreDrugs/` is explicitly excluded from deploy.

## License / Attribution

V4LEXL on NexusMods. No source code is available in the repo; `PhoneScroll.decompiled.cs` (locally in the temp directory during decompile, not in the repo) serves as a reference for behavior analysis. The repo only contains the `.dll` (binary deployment).