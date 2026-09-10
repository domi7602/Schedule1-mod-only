# Changelog

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
