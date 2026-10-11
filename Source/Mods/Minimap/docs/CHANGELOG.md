# Changelog

## Unreleased
- **Settings phone app: user-verified 2026-10-11** — the tabbed layout (Karte / Marker / Farben / Erweitert) works in-game. Two same-day follow-ups from that test (deployed 2026-10-11, look pending):
  - colour rows are now **palette pickers** — a preview swatch plus a ▼ dropdown opening the shared 16-colour palette (`MinimapFeatureRules.ColorPalette`) instead of manual hex entry; picked colours apply and persist immediately (full hex stays editable via `blip_colors.json` / `config.json`);
  - the **Zoom value row was removed** from the settings — zoom stays on the keyboard keys and the `minimap zoom` console command.
- **Settings app rebuild (unreleased):** one tab page per area replaces the long single scroll (the pre-tab section builders are deleted); customer markers use portraits automatically — the Customer Names / Portraits app toggles were removed, and the config fields remain only so old saves still load.
- **Fixed: HUD position collapsed toward the screen centre on every layout apply.** The clamp used the phone `UITheme.ActualWidth/Height` (0 before any phone app was built) instead of the HUD's own canvas size, so a stored top-right position was crushed on each scene load. The card is now clamped against its own canvas (fallback: 1920x1080 reference); while unmeasured, the stored position is kept. **User-verified 2026-10-10:** the minimap loads in the corner again.
- **Fixed: map surface stayed visible over the loading screen.** The spawn/loading guard hid only the blips while the map image and player marker kept showing a stale default crop. Both are now hidden under the same readiness guard and re-shown with the first real player position.

## 2.1.1 (2026-10-10) — NPC portraits
- **Customer markers now show the NPC's portrait image instead of a text name** (user request: "lieber Bilder vom NPC statt Schrift"). The round mugshot badge comes from `NPC.MugshotSprite` (same sprite the game uses in its own UI); the marker keeps its colour (purple = potential, green = known) and the text name only appears as a fallback when no portrait is available (early save load).
- New setting **Customer Portraits** (default **on**); **Customer Names** now defaults to **off**. In the phone app under HUD, or `minimap blips portraits <on|off>`.
- Portrait images are lazy-resolved per customer and drawn in the existing 64-blip pool (no per-frame allocations); the pool got a small round mask badge per blip.


## 2.1.0 (2026-10-10) — "Nav & Marker Edition"
- **Zoom range widened to 0.1–12** (was 0.75–4.0); keyboard zoom, the `minimap zoom` command and the settings field all clamp through one shared rule (`MinimapFeatureRules.ClampZoom`).
- **Settings phone app expanded** with a MAP APPEARANCE section: shape, screen corner, rotation mode, map size, zoom, opacity, border width and border colour — plus a KEYBOARD SHORTCUTS section to rebind Toggle / Zoom In / Zoom Out (Unity `KeyCode` names).
- **New HUD toggles:** Customer Names, Purchase Checks, Regional Locks, Map Border, Compass Outline, Hide in Menus (hides the minimap while the phone or gameplay menu is open).
- **Customer markers:**
  - **Purple** highlight for available potential customers (was turquoise).
  - **Blue clock** (⏱) for accepted deals / awaiting delivery (was gold "Deal Meeting").
  - Name labels under customer, deal and lock markers.
- **Green purchase checks (✓)** for unowned but purchasable properties (`Property.UnownedProperties` + `CanBePurchased()`).
- **Gray regional locks (🔒)** for regions the player has not unlocked (`Map.Instance.Regions` + `IsUnlocked`).
- **Border width / on-off** now applied from config (sprite thickness no longer hard-coded).
- New file `MinimapFeatureRules.cs` (Unity-free decision helpers) with a `Minimap.Tests` xUnit project covering zoom/opacity/border clamping and marker resolution.
- Runtime visibility now evaluated in one place so `HideInMenus`, the toggle key and config edits stay consistent.

## 2.0.3 (2026-10-10) — "Visible only while the map is open" (key collision)
- **Fixed: the minimap appeared only while the big map was open.** Root cause: the default `ToggleKey` was **"M"** — the game's own map key. Every map open/close also toggled the minimap, and the flipped `MinimapVisible` state was persisted on scene unload, so the minimap ended up in lockstep with the map (reported state: `MinimapVisible: false` + `ToggleKey: "M"` in the user's config.json).
- **New default toggle key: "N"** (next to M, "Navi"; the game's shortcut keys are only M=map, T=messages, J=journal — N is unbound). Rebindable via `UserData/Minimap/config.json` (`ToggleKey`, any `UnityEngine.KeyCode` name).
- **Config migration on load:** a saved `ToggleKey: "M"` is moved to `"N"` and `MinimapVisible` is re-enabled (the stuck `false` is what made the minimap look map-bound). One-time, logged, then saved back.
- Expectation after update: the minimap is permanently visible at its position (default top-right) again; pressing M only opens the map.
- **In-game verified 2026-10-10:** the oversized green dot during startup is gone. The health-bar fill now starts at zero width until the first health update.

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
