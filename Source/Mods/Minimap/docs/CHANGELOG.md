# Changelog

## 2.3.4-dev.1 (2026-10-18) — Waypoint safety and opt-in search investigation
- Development preparation, NOT a playable sewer-key search release. No Hints tab, search-area overlay, production world/session binding or search-store IO is installed. Existing marker/configuration behavior remains unchanged.
- Repair field-only waypoint JSON loss with a property-based DTO and production roundtrip tests. Lost `[{}]` entries are not reconstructed. Trim and validate names before lookup, reject markup/control characters/nonfinite positions, cap labels and usable rows, and canonicalize colors.
- Bind pending waypoint snapshots to their original paths. Failed writes remain dirty; switching A -> B -> A restores newer retained data rather than stale disk state. CRUD resolves context before mutation; unknown sessions reject edits. Bound retries and refuse further edits safely at capacity until IO recovers.
- Validate primary and backup before use. Invalid/missing/unreadable sources are distinguished; a recovered backup remains read-only while a corrupt primary exists, so automatic backup rotation cannot destroy the good recovery copy. Explicit `wp clear` discards all rows to recover; manual file repair is also supported. Console messages report unsaved changes rather than false save success.
- Add opt-in, read-only `minimap searchdev <watch|stop|status|sites [offset]>`. Watch is OFF at startup, runs independently of HUD visibility, has time/report budgets, logs changed observations and load baselines only, and defers key reads without a valid current world candidate. Only the separately authorized `sites` command exposes exact development coordinates; it is not a gameplay hint.
- Prepare an unwired Unity-free search controller with stable-area/intent contracts, unknown/load baselines, explicit accept/cancel, retained intent and generic confirmed-transition completion. No native completion flag is persisted by this core.
- Parent observed further RED/GREEN regressions after independent review. All 186 tests pass; Release compilation has 0 warnings/errors; format checks pass. DLL/PDB/metadata were deployed and hash-matched; the live mod registry and startup log confirm this version loaded in the menu and disposable test world, with HUD/four settings pages initialized. Manual waypoint roundtrip was postponed by the owner, so gameplay verification remains open. Native-world investigation results from the prior installed build are scoped separately in `search-mode-phase0.md`; they do not verify this development DLL.

## 2.3.3 (2026-10-12) — Remove keyboard visibility toggle
- Latest.log recorded N at 11:40:34 changing visible true to false via save:ToggleMinimap, followed by M without re-enabling. Remove keyboard visibility toggling entirely, including its parsed key and settings rebind row; zoom input and explicit settings/console visibility controls remain.
- Keep ToggleKey only as a legacy schema/migration sentinel (None). On first load with any legacy key, set None, recover MinimapVisible=true and save atomically; later deliberate off choices with None remain unchanged. Existing hidden N configuration will recover on the next launch.
- Visibility diagnostics now report toggle=disabled. Regression source guards observed RED (3 failures) then GREEN; full suite 61 passed. Console help no longer advertises the removed M visibility hotkey (additional guard observed RED then GREEN). Source guards do not execute Unity input or establish in-game verification.

## 2.3.2 (2026-10-12) — Conservative blip-render workload reduction
- The renderer previously projected every unchanged 1-second entity snapshot each frame and recalculated player-yaw sine/cosine for every marker. Cache projected positions for the existing 64 slots, invalidating on every snapshot rebuild, native converter identity/fallback transition, and same-instance origin/edge/dimension/conversion-factor changes. Observe converter calibration once per frame; continue calculating player offset, zoom, shape/clamping and rotation each frame.
- Cache successfully applied per-slot sprite type, colours, dimensions, enabled/child-active state, portrait sprite, glyph/name text and geometry, and alpha. Skip identical native UI writes without suppressing pulse changes; retain actual child state over culling/hide/reopen and compare it on pool reuse. Resolve text/portrait RectTransforms once with GetComponent, replacing the unsafe portrait-parent managed cast. White portrait/glyph tint is initialized once. Live IL2CPP Color/Vector2 proxy declarations lack managed IEquatable<T>; cache their scalar tuples instead to avoid boxed managed equality. A dedicated source guard was observed RED then GREEN.
- Preserve all marker categories, selection/priority, range rules, 64-marker capacity, 1-second dynamic / 10-second landmark cadence, visibility diagnostics/toggle/menu policy and compact two-column settings. Customer scanning remains unchanged.
- Pure applied-state/cache regression tests were observed RED (5 failures) then GREEN; source-wiring guards were observed RED (3 failures) then GREEN. Full Minimap suite: 58 passed, 0 failed/skipped. Release build: 0 warnings/errors. Built/deployed DLL hashes match; runtime visual/lifecycle verification is pending. No live FPS, Unity CPU/GPU or GC performance measurement was performed; workload reductions are not an FPS claim.

## 2.3.1 (2026-10-12) — Visibility diagnostics (not a disappearance fix)
- Investigate the reported permanent disappearance after opening the phone map: stored config was false while its previous backup was true; ToggleKey was N and HideInMenus false. Pressing N restored visibility. These facts establish a persisted off state, not the original trigger.
- Add change-only `[MinimapVisibility]` breadcrumbs with source, frame, parsed toggle key, phone/menu/map object state and HUD state. Trace settings switches by label, save callers via CallerMemberName, startup state and otherwise-unattributed update changes. Log M-key and configured-toggle-key presses separately.
- Diagnostics do not change visibility decisions, key bindings or stored settings. Guards cover wiring only; diagnosis remains pending a live reproduction.

## 2.3.0 (2026-10-12) — Compact two-column settings
- Approved organization pass: Map has APPEARANCE and BEHAVIOR columns; Markers uses equal-width bands, Colors has two columns of seven pickers (including border color), Advanced separates keyboard and HUD controls. Existing settings callbacks and palette persistence are retained.
- Anchor the header and tabs at the top, footer at the bottom, and scroll region between them. Remove root preferred-size negotiation that previously left a 238.3-high viewport and a large unused lower area.
- Rows remain compact at 28 dp with fixed label/value/action bands; section headers remain 22 dp. Priority-100 height contracts prevent nested controls' preferred sizes from inflating rows. Expand content only when an inline palette actually needs more space.
- Reset Colors appears only on the Colors tab. Tab switches collapse the open palette, rebuild the active page and reset scroll to the top. Preserve the neutral/emerald palette without new icons or visual effects.
- Add source-wiring regression guards (red before implementation); live rendering and interaction verification remain pending. Runtime breadcrumb: `Compact layout: page=..., viewport=..., natural=..., target=..., columns=2`.

## 2.2.3 (2026-10-12) — Fix IL2CPP child enumeration in settings stretch
- Runtime v2.2.2 still failed: `Il2CppSystem.Object` could not be cast to `UnityEngine.Transform`. The typed foreach over a RectTransform inserts that managed cast even though the explicit RectTransform casts were removed.
- Traverse children with childCount/GetChild(index) instead; retain the existing stretch calculations. Log `Stretch applied: page=..., viewport=..., natural=..., target=...` for runtime verification.
- Add a source-level regression guard for this IL2CPP enumeration pattern (observed failing before the fix). Pure tests do not substitute for a live UI check.

## 2.2.2 (2026-10-12) — Fix page-stretch IL2CPP cast crash (found via live S1MCP log capture)
- **Bug (live-verified via S1MCP `capture_logs`):** the v2.2.1 stretch threw every frame — `Unable to cast object of type 'UnityEngine.Transform' to type 'UnityEngine.RectTransform'` — because IL2CPP wrapper objects do not support a direct `(RectTransform)transform` cast. The exception aborted `ApplyPageStretch` before the viewport height could be applied, so the stretch never happened.
- **Fix:** resolve the transforms via `GetComponent<RectTransform>()` (page + child) instead of casting. Same one-shot S1MCP connection pattern used for the diagnosis: the bridge's stale client slot and the queued duplicate response from a timed-out attempt desync the ack framing — fresh connection per query avoids it.

## 2.2.1 (2026-10-12) — Settings pages fill the viewport (PocketShop-style stretch)
- **No more dead black space below the settings rows (user request):** every tab page now stretches to the scroll viewport height like the PocketShop store directory — rows register as vertically flexible (`RegisterStretchable`), and `ApplyPageStretch` sets each page's preferred height to the viewport (never below its natural content height), so the page's VerticalLayoutGroup distributes the extra space across the rows. Map / Markers / Advanced now fit one screen without scrolling; Colors (14 rows) keeps scrolling naturally.
- Same self-healing pattern as PocketShop v0.3.4: the per-frame `Update` check re-applies whenever the measured viewport height changes (first measurement after open can be a stale pre-layout value). Palette open/close re-runs the stretch so the dropdown panel stays reachable.

## 2.2.0 (2026-10-12) — Settings app: unified control palette, English tabs
- **Unified colour scheme (user choice, "dezent"):** one control palette replaces the mixed steel-blue/green/brown controls — inactive tabs and all Change/Set/▼ buttons are now the same neutral dark (`ControlColor`), the active tab and Save & Apply stay emerald, and Reset Colors is a muted warm dark instead of saturated brown. The palette is defined once at class level instead of per-row literals.
- **Tab names in English** (`Map / Markers / Colors / Advanced`) — the tabs were German while every other label in the app is English.

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
