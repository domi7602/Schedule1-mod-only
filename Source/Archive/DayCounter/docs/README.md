# DayCounter for Schedule I

A simple, sleek on-screen day counter and clock overlay for **Schedule I** (v0.4.6f13+).
Because the game does not have a persistent HUD clock by default, **DayCounter** keeps you informed of the current in-game day, day of the week, and time at a glance.

---

## ✨ Features

- **Persistent HUD Overlay**: Shows `DAY 14`, day name (e.g. `Wednesday`), and clock (e.g. `15:30`).
- **2 Layout Modes**:
  - **Standard Badge (2 Rows)**: Prominent day header + day name & time subtitle with an emerald accent strip.
  - **Compact Pill (1 Row)**: Ultra-compact horizontal pill (`DAY 14 │ Wed 15:30`) for minimal screen footprint.
- **Drag & Drop Repositioning**: Click and drag the widget anywhere on your screen whenever your cursor is free (menus, phone, pause, inventory). Position is saved automatically.
- **Corner Anchors**: Easily anchor to `TopLeft`, `TopRight`, `BottomLeft`, or `BottomRight`.
- **Time & Day Customization**:
  - 24-Hour (`15:30`) or 12-Hour AM/PM (`03:30 PM`) format.
  - Full (`Wednesday`) or short (`Wed`) day names.
  - Customizable background opacity and scale factor (0.5× to 2.5×).
  - Toggle individual components (day number, day name, clock, accent strip).
- **Hotkey Support**: Toggle the HUD instantly with <kbd>F7</kbd> (protected against typing in input fields).
- **Console & Terminal Integration**: Full support for vanilla Dev-Console (`day` / `daycounter`) and the **hash** terminal.
- **0-Allocation Rendering**: Only updates TextMeshPro text strings when values actually change, ensuring zero GC pressure.

---

## ⌨️ Controls & Shortcuts

| Key / Action | Description |
|---|---|
| <kbd>F7</kbd> | Toggle HUD overlay visibility |
| **Left Click + Drag** | Drag & drop widget across the screen (when cursor is unlocked) |

---

## 💬 Console Commands

Commands work in both the vanilla game console (`~` or `F1`) and the **hash** terminal (`#`):

| Command | Description |
|---|---|
| `day status` | Show current day, time, and overlay configuration |
| `day toggle` | Toggle HUD overlay visibility |
| `day compact` | Switch between Standard 2-Row badge and Slim 1-Row Pill |
| `day format <24h\|12h>` | Switch time format |
| `day shortday` | Toggle short day names (Wed vs Wednesday) |
| `day anchor <corner>` | Set corner anchor (`topleft`, `topright`, `bottomleft`, `bottomright`) |
| `day pos <x> <y>` | Set custom anchored coordinates |
| `day scale <0.5-2.5>` | Adjust HUD scale multiplier |
| `day opacity <0.0-1.0>` | Adjust background transparency |
| `day strip` | Toggle the emerald accent indicator strip |
| `day autohide` | Toggle auto-hiding when phone/menus open |
| `day reset` | Reset all settings and position to defaults |

---

## ⚙️ Configuration

Settings are saved in `UserData/MelonPreferences.cfg` under `[DayCounter]`:

```ini
[DayCounter]
Visible = true
ToggleHotkey = "F7"
Anchor = "TopLeft"
PositionX = 24.0
PositionY = -24.0
Scale = 1.0
BackgroundOpacity = 0.75
CompactMode = false
TimeFormat24H = true
ShortDayNames = false
ShowDayNumber = true
ShowDayName = true
ShowTime = true
ShowAccentStrip = true
AllowDragging = true
AutoHideInMenus = false
```
