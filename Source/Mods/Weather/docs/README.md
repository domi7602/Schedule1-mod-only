# Weather (v0.4.0)

A read-only weather dashboard for the in-game smartphone in *Schedule I* (v0.4.7, IL2CPP).

The app visualises the nine weather components that the game's own weather system publishes through
`S1API.Weather.WeatherManager` — Sunny, Cloudy, Rainy, Stormy, Snowy, Foggy, Windy, Hail and Sleet.

---

## Features

- **Dominant-Condition Hero Card:** An accent-bordered rounded card with a soft accent halo presents the highest-weighted component: the condition name large and uppercase (`MIXED` on a tie for the top weight, `CLEAR` when all weights are 0), its percentage, a solid accent pill with dark text (`HEAVY` > 0.66 / `MODERATE` 0.33–0.66 / `LIGHT` below that / `CLEAR` at 0), the `N OF 9 ACTIVE` meta line, and a ring gauge on the right carrying the same accent arc (rounded cap) around the shape-drawn condition icon.
- **All Nine Components Listed:** Every condition gets a rounded row with shape icon, name, horizontal bar (0–1) and percentage. Active rows tint their row background, border, icon and bar with the condition's accent colour; inactive rows stay neutral grey-blue.
- **Live Updates:** `WeatherManager.OnWeatherChanged` re-renders the view instantly. While the app is closed the snapshot is only cached and rendered on the next open; a cheap per-frame comparison of `WeatherManager.Current` additionally covers changes that happen before the app instance subscribes.
- **Graceful Empty State:** When `WeatherManager.Current` is `null` (outside gameplay or while the native services initialise) a friendly card is shown: *"No weather data available"* plus a hint that weather data becomes available in-game.
- **Responsive uGUI (Method 3 `UITheme`):** All fonts use `UITheme.Sp(...)` and all dimensions `UITheme.Dp(...)` from `S1Mods.Shared.UITheme`. The nine rows share the available height (`VerticalLayoutGroup` with `childForceExpandHeight`), so the whole condition set is visible at any resolution — no scrolling required.
- **Keyboard:** <kbd>Escape</kbd> closes the app.
- **Zero Side Effects:** Pure status display — no persistence, no save files, no gameplay or money influence, no buttons.

---

## Layout

```
+------------------------------------------------------+
| WEATHER                               ● LIVE CONDITIONS
+------------------------------------------------------+
| +--------------------------------------------------+ |  <- hero card (accent border + halo)
| | RAINY                             .--------.     | |
| | 62%                               ( ring   )     | |  <- accent arc + condition icon
| | [MODERATE]   3 OF 9 ACTIVE        '--------'     | |
| +--------------------------------------------------+ |
|                                                      |
| ALL CONDITIONS                                       |
| +--------------------------------------------------+ |
| | (i) CLOUDY                                  28% | |
| |     [=============                               | |
| | (i) RAINY                                   62% | |
| |     [=======================                     | |
| |      ... (all nine rows; active rows accent-tinted)
| +--------------------------------------------------+ |
+------------------------------------------------------+
```

---

## Weather API Used

| Member | Purpose |
|---|---|
| `WeatherManager.Current` (`WeatherState?`) | Immutable snapshot of the nine condition weights (each normalised 0–1). `null` outside gameplay / during native service initialisation. |
| `WeatherManager.OnWeatherChanged` (`Action<WeatherState>`) | Raised when weather first becomes available and on every change to a distinct snapshot. The dispatcher catches subscriber exceptions internally. |

`WeatherState` is a readonly struct with the nine float properties in the order
`Sunny, Cloudy, Rainy, Stormy, Snowy, Foggy, Windy, Hail, Sleet`.

---

## Technical Stack

- **Target Framework:** `net6.0` (C# 12)
- **Mod Loader:** MelonLoader 0.7.3 (IL2CPP)
- **Dependencies:** `S1API` (3.2.1-beta.8), `S1Mods.Shared`
- **UI Engine:** Unity uGUI (`UIFactory`, `VerticalLayoutGroup`, `LayoutElement`, `Image`)
- **Lifecycle:** `MelonEvents.OnUpdate` is subscribed exactly once with the defensive
  `Unsubscribe`-before-`Subscribe` pattern; it is **never** unsubscribed in `OnPhoneClosed()`
  (Rule 10 — otherwise the app stays blank after the first phone close). The static
  `WeatherManager.OnWeatherChanged` handler dispatches through a single `_active` instance, so a
  scene reload never stacks subscribers.

---

## Build & Installation

```pwsh
# Build single mod (auto-deploys to <GameDir>\Mods\ + <GameDir>\UserData\Weather\)
dotnet build Source\Mods\Weather\src\Weather.csproj -c Release

# Or build the entire solution
pwsh Tools\build-all.ps1
```

Release layout:

| File | Target |
|---|---|
| `Weather.dll` | `<GameDir>\Mods\Weather.dll` |
| `weather_icon.png` | `<GameDir>\Mods\weather_icon.png` (deployed from `assets/` by the build) |
| `mod.json` | `<GameDir>\UserData\Weather\mod.json` |
| `Weather.pdb` | `<GameDir>\UserData\Weather\Weather.pdb` |

---

## Verification Checklist

- [x] Compiles with 0 errors / 0 warnings (`dotnet build ... -c Release`).
- [x] `Orientation` explicitly `EOrientation.Vertical`.
- [x] `_mainBG` created with `fullAnchor: true` and starts `SetActive(false)`.
- [x] `OnPhoneClosed()` destroys nothing and unsubscribes nothing (Rule 2 / Rule 10).
- [x] All fonts/dimensions via `S1Mods.Shared.UITheme` (`Sp`/`Dp`) — no local theme class.
- [x] No input field → no `InputFocus` hook required (no WASD lock needed).
- [x] <kbd>Escape</kbd> closes the app.
- [ ] Re-open test in a real game session (open → close phone → re-open → still renders).
