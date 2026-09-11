# Changelog


## 1.0.3 (2026-09-11)
- NewGame-SlotSelected mit Bounds-Guard (kein OOB auf unvollstaendiger letzter Seite).
- Hover-Tracker via EventHelper (IL2CPP-sicher) ohne Vanilla-Trigger-Clear.
- Font-Cache mit Liveness-Check; Rename ohne Regex-Fallback (+ .pre-rename.bak).

## 1.0.2 (2026-09-10)
- 1-based Slot-Nummern im Save-Scan (Active-Save-Schutz bei Rename/Delete greift).
- Toter _dialogRootMissing-Block und ungenutztes SlotsPerPage-Config entfernt.

## 1.0.1 (2026-08-14)
- **UI & Button Fix**: Replaced custom click handlers with standard `UnityEngine.UI.Button` components and solid styling, completely eliminating hollow wireframe ("empty skeleton") button artifacts.
- **Font & Material Resolution**: Implemented robust `TMP_FontAsset` and `fontSharedMaterial` auto-detection from scene canvas and game resources, ensuring all text labels render clearly.
- **Interactive State**: Added automatic interactive enabling/disabling for `◄ PREV` and `NEXT ►` buttons depending on the active page.
- **EventTrigger Hover**: Migrated slot hover tracking to Unity's native `EventTrigger` for IL2CPP reliability.
- **Slot Renumbering Safety**: Protected internal save metrics (organisation name, net worth, dates) from slot renumbering routines.
- **Empty Slot Handling**: Prevented invalid null game loads on empty slots in `ContinueScreen`.

## 1.0.0 (2026-08-14)
- Initial release for *Schedule I* v0.4.6f13 (IL2CPP).
- Expand save slots from vanilla 5 to 25+ slots with 5-slot page navigation.
- In-menu & in-game save game renaming feature (<kbd>F2</kbd>/<kbd>R</kbd> / UI Button).
- Harmony patches for `SaveDisplay.Refresh`, `ContinueScreen.LoadGame`, `NewGameScreen.SlotSelected`, `MenuScreen.OpenScreen`, and `SaveManager.Awake`.
- Configurable settings via `UserData/MoreSaveSlots/config.json`.
