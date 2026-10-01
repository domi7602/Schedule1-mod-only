# Changelog

## 1.0.12 (2026-09-13) — S-05: dead DEL/EDIT buttons (invisible modals) fixed
S-05 fix "dead DEL/EDIT buttons": The rename/delete modals selected an arbitrary, unsorted canvas via `FindObjectsByType<Canvas>()[0]` — if they landed behind the menu canvas they opened invisibly and their fullscreen dimmer swallowed all clicks (menu appeared dead, ESC healed it). Now: (1) `UIHelper.FindDialogCanvas()` selects the canvas of the SaveDisplay UI stack (fallback: root canvas with highest `sortingOrder`), (2) each modal gets its own sorting overlay (`overrideSorting`, `sortingOrder` 1000) + its own `GraphicRaycaster`, (3) `SetAsLastSibling` on re-open. Click handlers were never broken — the routing behind them was.

## 1.0.11 (2026-09-13) — Savegame dupe fix (ghost cards)
Bug-report round 6 / "Savegame-Dupe" fix: (1) SaveDisplay.Awake path now treats empty slots like the refresh path (`UpdateEmptyState` + `UpdateSlotNumberText`) — the prefab placeholder text ('Organisation', '$0', 'More than a year ago', 'v0.1.0') no longer lingers as a ghost card when Awake runs before the registry scan. (2) `RefreshActiveScreen` now refreshes ALL SaveDisplays including inactive ones (`FindObjectsInactive.Include`) — the post-scan refresh used to skip the still-closed Continue panel, so outdated cards were visible on first open. Clicks on empty slots were and remain safe (ContinueScreen guard).

## 1.0.10 (2026-09-13) — Bug-Audit-Fixes Runde 5 (Audit 2026-09-13)
- EventTrigger pointer leak fixed: `CleanupOwnedTriggersForSlot()` removes all owned EventTrigger entries from `_ownedTriggers` on page switch — previously the dictionary grew unbounded with dead IntPtrs.
- `RefreshActiveScreen()` now cleans up all stale keys via `CleanupOwnedTriggersForSlot` before the refresh (instead of waiting until the next page switch).

## 1.0.9 (2026-09-12) — Bug-Audit-Fixes Runde 3 (Audit 2026-09-12)
- EventTrigger cleanup (`PaginationController.AttachHoverTracker`) now only removes **owned** entries (tracked via the `_ownedTriggers` dictionary) instead of all pointer entries — previously the vanilla hover/click handlers of the save-slot cards were removed along with them (hover highlight lost).

## 1.0.6 (2026-09-11)
- Diagnostic line per refresh (page, array length, first name) for empty-name error hunting.

## 1.0.5 (2026-09-11)
- Refresh actively after save scan (RefreshActiveScreen): Awake ran before the scan and showed empty names even though saves existed.

## 1.0.4 (2026-09-11)
- SaveDisplay.Awake intercepted with a prefix (vanilla loops `0..SAVE_SLOT_COUNT-1` for only 5 slot cards → `IndexOutOfRange`). Paginated init, vanilla is skipped.
- Stale init log corrected from v1.0.2 to v1.0.4.

## 1.0.3 (2026-09-11)
- NewGame.SlotSelected with bounds guard (no OOB on an incomplete last page).
- Hover tracker via EventHelper (IL2CPP-safe) without vanilla trigger clear.
- Font cache with liveness check; rename without regex fallback (+ `.pre-rename.bak`).

## 1.0.2 (2026-09-10)
- 1-based slot numbers in the save scan (active-save protection on rename/delete takes effect).
- Dead `_dialogRootMissing` block and unused `SlotsPerPage` config removed.

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
