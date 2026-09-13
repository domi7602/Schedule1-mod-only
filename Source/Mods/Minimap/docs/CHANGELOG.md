# Changelog


## 2.0.1 (2026-09-12) — Bug-Audit-Fixes (Audit 2026-09-12)
- `ResolveSlotSuffix` → `Save()`-Endlos-Rekursion gefixt (Crash): `_dirty` wird jetzt konsumiert BEVOR `FlushToPath(oldPath)` aufgerufen wird; neue `FlushToPath(string)` schreibt an den expliziten alten Pfad ohne erneut `ResolveSlotSuffix` zu durchlaufen. Verhindert unkatchbaren `StackOverflowException` beim Slot-Switch nach fehlgeschlagenem Save (AV/Readonly).
- Health-Bar bekommt eine `sizeDelta` (`MapSize * 0.92` Breite, 6 Hoehe). Vorher Unity-Default 100x100 — quadratischer Block statt schlanker Leiste unter der Karte.

## 2.0.0 (2026-09-12) — Marker & Settings Edition
- **M1 Dealer-Marker:** Live-Positionen angeworbener Dealer (lila, mit Name) via DealerManagementApp.dealers; immer am Rand geclamped, unabhaengig von MaxEntityRange.
- **M2 Heat-Ring:** Pulsierender Ring um die Minimap nach EPursuitLevel (None→aus, Investigating→Bernstein, Arresting→Orange, NonLethal→Rot, Lethal→Dunkelrot).
- **M3 Customer-Trennung:** Dealer vs. Kunden sauber getrennt (eigene BlipTypes + Filter).
- **M4 Waypoints:** Eigene Wegpunkte als pulsierende Diamant-Blips — `minimap wp add <name> [hex]`, `wp del`, `wp list`, `wp clear`; slot-isolierte Persistence via SafeStorage (waypoints_slot_{n}.json, slot_-1-Guard), Limit 16.
- **M5 Health-Bar:** Schlanke HP-Bar unter der Minimap (Polling 0,25s, 0-Allocation), Farbrampe Gruen→Bernstein→Rot, per Config abschaltbar.
- **M6 Minimap-Settings-PhoneApp:** Toggles fuer alle Blip-Typen + Hex-Farbfelder pro Kategorie (blip_colors.json Sidecar), Save&Apply + Reset-Buttons; lebt im Minimap-Assembly (S1API Auto-Discovery).
- Neue Dateien: MinimapWaypoints.cs, MinimapSettingsApp.cs.
- Alle Blip-Farben jetzt ueber zentrale Palette (PaletteColor-Lookup mit Fallback auf Defaults).

## 1.0.3 (2026-09-11)
- Critical-First-Partition: Quest-/Deal-Blips ueberleben Pool-Overflow (statt umgekehrt).
- Raycast-Sync in ApplyLayout (Mask frisst keine Klicks mehr bei AllowDragging=false).

## 1.0.2 (2026-09-10)
- Config-Save entprellt (nur bei Toggle/Scene-Unload statt pro Zoom-Taste).

## 1.0.1
- **Bug:** Time-of-day display in the integrated DayCounter now uses vanilla `TimeManager.Get12HourTime(CurrentTime)` for 12-hour mode, fixing the parse logic. 24-hour mode preserves the verified HHMM math.
- **Bug:** Hash Terminal bridge is now actually wired. `Hash.Api.HashCommands.Add(...)` registers `minimap` and `map` for tab-completion and help. The legacy no-op log was removed.
- **Bug:** `MinimapCommands.Print` no longer double-prefixes the `[Minimap]` literal (MelonLogger sets the prefix automatically).
- **Feature:** `ShowVehicleBlips` is now functional — scans `VehicleManager.AllVehicles` with range filter.
- **Feature:** `ShowQuestBlips` is now functional — iterates `Quest.Quests`, then `QuestEntry.PoILocation` for active state POIs.
- **Feature:** `SleepingBag` blip cross-mod integration — resolved via reflection on `HomelessMod.Building.StreetPropertyManager.ActiveStreetObjects`. Skipped silently if HomelessMod is not loaded.
- **Feature:** `DayCounterMode.Standalone` is now correctly implemented — map viewport hidden, only the DayCounter bar shown.
- **Refactor:** `MinimapMapAliasCommand._mainCmd` is now `static readonly` (one allocation instead of per-call).
- **Refactor:** `IsGameplayScene` is now case-sensitive and matches exactly `"Main"` (matches workspace convention; avoids false positives on additive UI scenes).
- **Refactor:** `MinimapFont.ResetCache()` is called on `EnsureHUD` so font refs from a previous scene are not re-used after a scene transition.
- **Docs:** README + CHANGELOG filled out.

## 1.0.0
- Initial release: Dual-Shape Viewport, Integrated DayCounter HUD, Dynamic Rotation, 0-Allocation Pooled Blip Engine, Drag & Drop, S1API Console + Hash Terminal integration.
