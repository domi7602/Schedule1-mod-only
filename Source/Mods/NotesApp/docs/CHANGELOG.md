# Changelog

## 1.0.0 (2026-08-14)
- **SafeStorage Persistence Integration**:
  - Migrated save & load mechanisms to `S1Mods.Shared.SafeStorage` (`SaveAtomic` with `.tmp` staging, automatic `.bak` rotation, and fallback restoration).
  - Preserves 100% backward-compatibility and legacy migration for older save games.
- **Real-Time Search & Filter Bar**:
  - Integrated a search input bar (`🔍 Search notes...`) directly beneath the header in the List view.
  - Live filtering against both note titles and body content with zero-allocation query updates and dedicated empty state feedback (`No notes matching "{query}"`).
- **Pinning / Favorites System (📌)**:
  - Added `IsPinned` property to the data model.
  - Pinned notes are highlighted with an amber indicator bar and sorted to the very top of the list.
  - Quick `[📌 Pin]` / `[Unpin]` toggle directly in the Detail view header.
- **Detail View Power Actions**:
  - `[Copy]` Button: Copies the full note text (and title) directly to the Windows clipboard via `GUIUtility.systemCopyBuffer` with temporary `"✓ Copied!"` visual feedback.
  - `[Clone]` (Duplicate) Button: Instantly clones the current note with `"(Copy)"` suffix.
- **In-Game Quick-Stamp (`[ 🕒 Stamp ]`)**:
  - Header action in the Editor screen to inject the current in-game day, weekday, and 24-hour time (e.g. `[Day 14 (Wednesday), 15:30]`) or fallback real-world timestamp directly into the note.
- **Keyboard Shortcuts & Power-User Controls**:
  - <kbd>Ctrl + S</kbd>: Instant save in the Editor.
  - <kbd>Escape</kbd>: Navigate back / cancel from Confirm, Editor, or Detail views.
  - <kbd>Tab</kbd> / <kbd>Shift + Tab</kbd>: Switch input focus seamlessly between Title and Text fields.
  - <kbd>Ctrl + N</kbd>: Open a new note from the List screen.
  - <kbd>Ctrl + F</kbd>: Focus the search bar in the List screen.
- **Updated Timestamp Tracking**:
  - Added `UpdatedAt` to `Note` model, updated whenever a note is edited so the list displays the most recently modified notes first.
- **Enhanced Input Focus Tracking**:
  - `NotesAppInputFocus` now tracks Search, Title, and Text fields to prevent any accidental player movement or game interaction while typing.

## 0.9.0
- **UI Readability & Scaling**:
  - **Bigger Buttons**: `FontSizeButton` raised from 17pt to 24pt, `BtnHeight` from 40f to 56f. All action buttons ("Back", "Edit", "Delete", "Cancel", "Save", "New Note") are now significantly more readable and easier to click.
  - **Back Button & Header Layout**: Back-button width widened in Detail and Editor screens (0.05–0.42), title shifted next to it at 0.45, so "Back" with 24pt font is no longer clipped.
  - **Multi-Line Text Field Top-Aligned**: `TextAnchor.UpperLeft` and padding offset corrected for the editor text field (text now starts at the very top instead of centered).
  - **Delete Dialog Polished**: Dialog box enlarged, confirmation text with clean line breaks, red button labelled concisely as "Delete".
  - **"New Note" Button**: 320px wide, 24pt font, perfectly centred in the footer bar.

## 0.8.0
- **Editor Improvements**:
  - **Section Labels** "TITLE" and "CONTENT" as small-caps, 14pt, white at 40% alpha, above the respective inputs. Clear visual hierarchy — previously both inputs were equally weighted without structure.
  - **Larger Body Font**: New constant `FontSizeBodyInput = 30f` (previously 26pt). Automatically used for `multiLine: true`; Title input stays at 26pt. More comfortable for writing.
  - **Live Word Count** in the bottom-right of the text field: shows `{N} W · {M}` (words · chars), updates live via `InputField.onValueChanged` (hooked through `EventHelper.AddListener<string>` for IL2CPP safety). Hidden when the text is empty.
  - **Input Anchors Adjusted** to make room for the labels: Title input 0.72-0.81 (9% height) → 0.77-0.825 (5.5%, more compact), Text input 0.16-0.70 (54%) → 0.14-0.695 (55.5%, slightly larger for more writing comfort).
- New helpers `UpdateWordCount(string)` + `CountWords(string)` (editor helpers section at the end of the class).

## 0.7.3
- **Bug Fix: List Scrolling with Many Notes**. Previously, with ~5+ notes the lower rows vanished under the BottomBar without the list being scrollable. Root cause: `UIFactory.FitContentHeight` is a no-op (S1API already sets `ContentSizeFitter` correctly), and Unity updates the layout only at frame end — after `SaveEditor → ShowList → RefreshList` the screen showed stale layout values, ScrollRect didn't activate.
- **Fix**: `RebuildListLayout()` method after refresh calls `LayoutRebuilder.ForceRebuildLayoutImmediate(_listContent)` for immediate bottom-up rebuild. Plus three edge cases:
  1. ScrollRect config explicit (`vertical=true`, `movementType=Clamped` instead of Elastic default) — `ScrollRect` is now stored in `_listScrollRect` from the `out` param of `UIFactory.ScrollableVerticalList`.
  2. Save scroll position before rebuild and restore after rebuild — no "jumping" during the SaveEditor→ShowList cycle.
  3. When content ≤ viewport (shrunk list), `verticalNormalizedPosition = 1` (snap to top) — otherwise the content stays pinned above the viewport.

## 0.7.2
- **Custom App Icon**: `notiz_app_lowpoly_fancy.png` (256×256, low-poly style — folded note corner + pencil, generated with Meta AI) replaces the programmatic placeholder icon. Lives directly in the `Mods/` folder (not embedded in the DLL), S1API loads it via `IconFileName`. The build automatically deploys all `*.png` files from the `src/` folder to the `Mods/` directory (see `Directory.Build.targets`).
- Dead code removed: `CreateIconSprite` method, `_iconSprite` field and `IconSprite` override — no longer needed since `IconFileName` now loads the file.

## 0.7.1
- **"New Note" Button Enlarged**: Width 190px → 290px (significantly more space), height 44px → 40px (flatter pill shape), font 19pt → 22pt (more legible). Constants `AddBtnWidth` and `BtnHeight` are only used here, so no side-effects on other buttons.

## 0.7.0
- **List View Visual Upgrade**:
  - **Accent Stripe on the Left** of each row (5px wide, full height). Colour hashed from the title (HSV) — every note gets "its" colour, deterministic. Turns the uniform-grey rows into visually distinct cards.
  - **Timestamp on the Top-Right** of each row (relatively formatted: "today" / "yesterday" / "Aug 12" / "Aug 12 2025" for older notes). Brings visual balance (title left, date right) and gives each note a clear time reference.
  - Title and preview anchors adjusted to make room for accent + date (offsetMin.x 16 → 20, Title-anchorMax.x 1.0 → 0.7).
- **Data Model Extension**: `Note` now has `CreatedAt` (DateTime, UTC). Set explicitly to `DateTime.UtcNow` on creation; old notes without a value get the default value (UTC-now of load) on deserialise and are persisted on the next save.
- `using System.Globalization;` for `CultureInfo("de-DE")` in the date formatter.

## 0.6.0
- **Delete Confirmation**: Tapping "Delete" in the detail view now opens a confirmation dialog (semi-transparent dimmer + centred dialog box) before the note is permanently removed. The body text shows the title of the affected note. Prevents accidental deletion — no undo, but a real second chance. The Confirm screen is implemented as its own view (same pattern as List/Detail/Editor), no modal overlay, so it can be reused later for other confirm actions (e.g. "Discard editor changes?").
- `DeleteNote` cleaned up: navigation logic removed, leaving only remove + save + index fix-up. `ConfirmDelete` now handles navigation explicitly (always `ShowList`).

## 0.5.1
- **Version Badge Fix**: `MelonInfo` is an assembly-level attribute → `Attribute.GetCustomAttribute(typeof(Mod).Assembly, ...)` instead of `typeof(Mod)`. The badge now correctly shows "v0.5.1" instead of "v?".

## 0.5.0
- **Layout Fix (final)**: S1API's `UIFactory.ScrollableVerticalList` leaves `sizeDelta.x=100` of the default content GameObject behind → content 100px wider than the viewport → 50px mask clipping left + 50px right → text was clipped on the left. Workaround in `BuildListScreen`: `_listContent.sizeDelta = new Vector2(0, currentY)` right after the `UIFactory.ScrollableVerticalList` call. Details in `Knowledge/s1api-scrollableverticallist-bug.md`.
- **Diagnostic Code Removed**: `NotesAppDiag` MonoBehaviour + DBG logging removed (was a debug aid during bug hunting, clean 0.5.0 without debug noise).
- **Version Badge Dynamic**: Now read live from `MelonInfoAttribute` instead of hard-coded "v0.4".

## 0.4.0
- Initial NotesApp version with Name+Text notes, List/Detail/Editor views, JSON persistence.
- *0.4.1–0.4.5: Debug iterations for the layout problem (see git log `git log --oneline -- Source/Mods/NotesApp`).*

## 0.3.0
- Notes now have **Name + Text** instead of a single string.
- List lines show Name (bold) + Text preview; the whole line opens the **Detail View**.
- Detail view with scrollable full text, Back/Edit/Delete.
- Dedicated Editor screen for create/edit (Name field + multi-line text field).
- Old `notes.json` saves (`List<string>`) are auto-migrated (title from first line).
- Icon reworked (notepad look instead of broken single-line).

## 0.2.0
- Edit existing notes (Edit mode in the input area).

## 0.1.0
- Initial version: Phone app "Notes" with create/view/delete, JSON persistence, custom icon.
