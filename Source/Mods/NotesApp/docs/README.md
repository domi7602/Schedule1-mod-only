# NotesApp (v1.0.0)

A feature-rich in-game Notes application for the smartphone in *Schedule I* (v0.4.6f13+).

---

## Features

- **Responsive uGUI Design (Method 3 `UITheme`):** Scale-adjusted typography (`Sp`) and dimensions (`Dp`) that look crisp on any resolution and rotated phone canvas.
- **Real-Time Search & Filter:** Search bar at the top of the list to instantly filter notes by title or body content.
- **Pinning / Favorites (📌):** Pin your most critical notes (mix formulas, customer schedules, dealer rates) to the top of the list with a distinct amber accent.
- **Data Resilience (`SafeStorage`):** Atomic file writes with `.tmp` staging, automatic `.bak` backups, and recovery from corruption.
- **Power-User Actions in Detail View:**
  - **Edit:** Modify title and content seamlessly.
  - **Copy:** One-click copy to Windows clipboard (`GUIUtility.systemCopyBuffer`) with visual feedback.
  - **Clone:** Duplicate existing notes as templates.
  - **Delete:** Safe deletion with confirmation modal.
- **In-Game Quick-Stamp (`[ 🕒 Stamp ]`):** Stamp the current in-game Day, Weekday, and 24-hour Time (`[Day 14 (Wed), 15:30]`) directly into your notes.
- **Live Counter:** Real-time word and character counter (`{N} W · {M} C`) in the editor.
- **Keyboard Shortcuts:**
  - <kbd>Ctrl + S</kbd>: Save note in editor.
  - <kbd>Escape</kbd>: Go back / cancel current screen.
  - <kbd>Tab</kbd> / <kbd>Shift + Tab</kbd>: Switch focus between Title and Text inputs.
  - <kbd>Ctrl + N</kbd>: Create a new note from the list.
  - <kbd>Ctrl + F</kbd>: Focus the search bar.
- **Input Focus Protection:** Uses `NotesAppInputFocus` to prevent accidental player movement or game interaction while typing.

---

## Storage & Location

Notes are stored in JSON format at:
```
<GameDir>\UserData\NotesApp\notes.json
```
Older save files (`List<string>`) and notes without `UpdatedAt`/`IsPinned` properties are automatically migrated on load without data loss.

---

## Technical Stack

- **Target Framework:** `net6.0` (C# 12)
- **Mod Loader:** MelonLoader 0.7.3 (IL2CPP)
- **Dependencies:** `S1API` (3.1.15+), `S1Mods.Shared`
- **UI Engine:** Unity uGUI (`Canvas`, `ScrollRect`, `VerticalLayoutGroup`, `UIFactory`, `EventHelper`)

---

## Build & Installation

```pwsh
# Build single mod
dotnet build Source\Mods\NotesApp\src\NotesApp.csproj -c Release

# Or build entire solution
pwsh Tools\build-all.ps1
```
The build automatically deploys `NotesApp.dll`, `notiz_app_lowpoly_fancy.png`, and `mod.json` into `<GameDir>\Mods\`.
