# Changelog

All notable changes to **Weather** are documented in this file.

## 0.1.0 (2026-09-24)
- **Initial release** — read-only weather dashboard PhoneApp (`WeatherApp : PhoneApp`, `EOrientation.Vertical`).
- **Dominant-condition hero readout:** the highest-weighted component is rendered large (`RAINY  62%`) with its own accent colour, an intensity line (`Heavy` > 0.66 / `Moderate` 0.33–0.66 / `Light` > 0 / `Clear` at 0) and a wide progress bar.
- **All nine components listed** (Sunny, Cloudy, Rainy, Stormy, Snowy, Foggy, Windy, Hail, Sleet) with name, horizontal progress bar and percentage; zero-weight rows are dimmed so active conditions stand out.
- **Live rendering** on `WeatherManager.OnWeatherChanged` (cache-while-closed, render-on-open) plus a per-frame `WeatherManager.Current` comparison as a safety net.
- **Graceful empty state** when `WeatherManager.Current` is `null` ("No weather data available" + in-game hint).
- **Responsive layout** via `S1Mods.Shared.UITheme` (`Sp`/`Dp`); the nine rows share the available height, so no scrolling is needed at any resolution.
- **Lifecycle per runbook:** isolated `_mainBG` (`fullAnchor: true`, starts inactive), `OnUpdate` subscribed exactly once (defensive `Unsubscribe`-before-`Subscribe` in `OnCreated`), nothing destroyed/unsubscribed in `OnPhoneClosed` (Rules 2/3/10), single static dispatcher for `WeatherManager.OnWeatherChanged` (no subscriber leaks per scene load), <kbd>Escape</kbd> closes the app.
- No persistence, no gameplay influence — pure status display.
