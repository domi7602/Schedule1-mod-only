# MoreSaveSlots

**MoreSaveSlots** is a quality-of-life mod for *Schedule I* (v0.4.6f13, IL2CPP) that expands the save game slots beyond the vanilla 5-slot limitation (default 25 slots across 5 pages, configurable) and adds an inline Save Game Renaming feature.

---

## Features

- 🎮 **25+ Save Game Slots:** Configurable total slots (default 25, up to 100) via `UserData/MoreSaveSlots/config.json`.
- 📑 **Sleek Page Navigation:** Native `[ ◄ PREV ]` / `[ NEXT ► ]` pagination bar integrated directly into Continue, New Game, and Import/Export screens.
- ✏️ **Save Game Renaming:** Rename any existing save slot on the fly with <kbd>F2</kbd>, <kbd>R</kbd>, or the `[ ✏️ RENAME ]` UI button. Updates `Game.json` and memory instantly.
- ⌨️ **Keyboard Navigation:**
  - <kbd>←</kbd> / <kbd>→</kbd>, <kbd>PageUp</kbd> / <kbd>PageDown</kbd>, <kbd>Q</kbd> / <kbd>E</kbd>: Turn pages.
  - <kbd>F2</kbd> / <kbd>R</kbd>: Rename hovered or selected save.
  - <kbd>Enter</kbd> / <kbd>Escape</kbd>: Confirm or close rename dialog.
- 🛡️ **100% Backward Compatible:** Existing vanilla saves (`SaveGame_1` through `SaveGame_5`) stay intact on Page 1.

---

## Configuration

Located at `UserData/MoreSaveSlots/config.json`:
```json
{
  "TotalSlots": 25,
  "SlotsPerPage": 5,
  "EnableKeyboardNavigation": true,
  "EnableRenameFeature": true
}
```
