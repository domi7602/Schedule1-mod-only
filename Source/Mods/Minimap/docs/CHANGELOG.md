# Changelog

## 2.0.2 (2026-09-13) — Bug Report Round 6
Bug Report Round 6: M-05 - ContentSizeFitter (PreferredSize) on settings scroll content; Save&Apply footer is now reachable on long content (scroll never worked correctly before).


## 2.0.1 (2026-09-12) — Bug Audit Fixes (Audit 2026-09-12)
- `ResolveSlotSuffix` → `Save()` endless recursion fixed (crash): `_dirty` is now consumed BEFORE `FlushToPath(oldPath)` is called; new `FlushToPath(string)` writes to the explicit old path without going through `ResolveSlotSuffix` again. Prevents an uncatchable `StackOverflowException` on slot switch after a failed save (AV/Readonly).
- Health bar gets a `sizeDelta` (`MapSize * 0.92` width, 6 height). Previously Unity default 100x100 — square block instead of slim bar below the map.

## 2.0.0 (2026-09-12) — Marker & Settings Edition
- **M1 Dealer Marker:** Live positions of recruited dealers (purple, with name) via DealerManagementApp.dealers; always clamped to the edge, independent of MaxEntityRange.
- **M2 Heat Ring:** Pulsating ring around the minimap by EPursuitLevel (None→off, Investigating→amber, Arresting→orange, NonLethal→red, Lethal→dark red).
- **M3 Customer separation:** Dealer vs. customers cleanly separated (own BlipTypes + filter).
- **M4 Waypoints:** Own waypoints as pulsating diamond blips — `minimap wp add <name> [hex]`, `wp del`, `wp list`, `wp clear`; slot-isolated persistence via SafeStorage (waypoints_slot_{n}.json, slot_-1 guard), limit 16.
- **M5 Health Bar:** Slim HP bar below the minimap (polling 0.25s, 0-allocation), colour ramp green→amber→red, switchable via config.
- **M6 Minimap Settings PhoneApp:** Toggles for all blip types + hex colour fields per category (blip_colors.json sidecar), Save&Apply + Reset buttons; lives in the Minimap assembly (S1API auto-discovery).
- New files: MinimapWaypoints.cs, MinimapSettingsApp.cs.
- All blip colours now via central palette (PaletteColor lookup with fallback to defaults).

## 1.0.3 (2026-09-11)
- Critical-First Partition: quest/deal blips survive pool overflow (instead of the other way around).
- Raycast sync in ApplyLayout (mask eats no more clicks when AllowDragging=false).

## 1.0.2 (2026-09-10)
- Config save debounced (only on toggle/scene-unload instead of per zoom key).

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
