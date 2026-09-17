# Changelog

## 1.0.1 (2026-08-16)

- **TopLeft Default Position**: HUD now starts in the top-left corner (24, -24) by default.
- **Compact Text by Default**: `CompactMode` is now `true` — single-row slim pill is the default layout.
- **Time Display Off by Default**: `ShowTime` is now `false` — only `DAY <N> <Weekday>` is shown by default. Time can be re-enabled via config.
- **No More Label Separators**: Removed `│` and `•` separator glyphs from the text rendering. All elements are joined by a single space for a clean, compact look (e.g. `DAY 14 Wed`).

## 1.0.0 (2026-08-15)

- **Initial Release**:
  - Persistent on-screen HUD overlay showing Day, Day Name, and Time.
  - Standard 2-row badge mode and slim 1-row compact pill mode.
  - Interactive drag-and-drop repositioning with viewport boundary clamping.
  - 4 corner anchor presets (`TopLeft`, `TopRight`, `BottomLeft`, `BottomRight`).
  - 24-hour and 12-hour AM/PM time formatting.
  - Full and short weekday name formatting.
  - Custom background opacity and scaling.
  - Input-focus-protected <kbd>F7</kbd> toggle hotkey.
  - Full Dev-Console & Hash terminal command integration (`day` and `daycounter`).
  - 0-allocation update loop with TextMeshPro font caching.
