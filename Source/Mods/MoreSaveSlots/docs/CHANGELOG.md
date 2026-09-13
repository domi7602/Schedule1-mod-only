# Changelog

## 1.0.12 (2026-09-13) — S-05: tote DEL/EDIT-Buttons (unsichtbare Modals) behoben
S-05-Fix "tote DEL/EDIT-Buttons": Die Rename-/Delete-Modals waehlten per FindObjectsByType<Canvas>()[0] einen beliebigen, unsortierten Canvas — landeten sie hinter dem Menue-Canvas, oeffneten sie unsichtbar und ihr Vollbild-Dimmer frass alle Klicks (Menue wirkte tot, ESC heilte). Jetzt: (1) UIHelper.FindDialogCanvas() waehlt den Canvas der SaveDisplay-UI-Stacks (Fallback: Root-Canvas mit hoechstem sortingOrder), (2) jedes Modal bekommt ein eigenes Sorting-Overlay (overrideSorting, sortingOrder 1000) + eigenen GraphicRaycaster, (3) beim Re-Open SetAsLastSibling. Klick-Handler waren nie defekt — das Routing dahinter war es.

## 1.0.11 (2026-09-13) — Savegame-Dupe-Fix (Geisterkarten)
Bug-Report-Runde 6 / "Savegame-Dupe"-Fix: (1) SaveDisplay.Awake-Path behandelt leere Slots jetzt wie der Refresh-Pfad (UpdateEmptyState + UpdateSlotNumberText) — der Prefab-Platzhaltertext ('Organisation', '$0', 'More than a year ago', 'v0.1.0') bleibt nicht mehr als Geisterkarte stehen, wenn Awake vor dem Registry-Scan laeuft. (2) RefreshActiveScreen refreshed jetzt ALLE SaveDisplays inkl. inaktiver (FindObjectsInactive.Include) — der Post-Scan-Refresh uebersprang das noch geschlossene Continue-Panel, wodurch beim ersten Oeffnen veraltete Karten sichtbar waren. Klicks auf leere Slots waren und sind weiterhin sicher (ContinueScreen-Guard).


## 1.0.10 (2026-09-13) — Bug-Audit-Fixes Runde 5 (Audit 2026-09-13)
- EventTrigger-Pointer-Leak gefixt: `CleanupOwnedTriggersForSlot()` entfernt beim Page-Switch alle eigenen EventTrigger-Entries aus `_ownedTriggers` — vorher wuchs das Dictionary mit toten IntPtrs unbeschränkt.
- `RefreshActiveScreen()` ruft jetzt vor dem Refresh alle stale Keys über `CleanupOwnedTriggersForSlot` auf (statt erst beim nächsten Page-Switch).

## 1.0.9 (2026-09-12) — Bug-Audit-Fixes Runde 3 (Audit 2026-09-12)
- EventTrigger-Cleanup (`PaginationController.AttachHoverTracker`) entfernt nur noch **eigene** Entries (per `_ownedTriggers`-Dictionary getrackt) statt alle Pointer-Einträge — vorher wurden Vanilla-Hover/Click-Handler der Save-Slot-Cards mit-entfernt (Hover-Highlight verloren).

## 1.0.6 (2026-09-11)
- Diagnose-Zeile pro Refresh (Seite, Array-Länge, erster Name) zur leeren-Namen-Fehlersuche.

## 1.0.5 (2026-09-11)
- Nach Save-Scan aktiv refreshen (RefreshActiveScreen): Awake läuft vor dem Scan und zeigte leere Namen, obwohl Saves da sind.

## 1.0.4 (2026-09-11)
- SaveDisplay.Awake mit Prefix abgefangen (Vanilla loopt 0..SAVE_SLOT_COUNT-1 bei nur 5 Slot-Karten -> IndexOutOfRange). Paginierte Init, Vanilla wird geskippt.
- Stale Init-Log v1.0.2 auf v1.0.4 korrigiert.

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
