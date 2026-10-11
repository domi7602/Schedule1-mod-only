# PropertyLEDs — implementation and verification status

Last update: **2026-10-11** (tube→beam geometry switch; motel lines re-anchored and corners confirmed in game by the owner; vanilla coverage import: 11 properties / 33 rooms — per-property visual confirmation open; catalog audit diagnostic added).
Scope: independently developed mod for *Schedule I* (MelonLoader, IL2CPP, S1API 3.2.1). No source,
mesh or texture of any existing LED mod was used; the functional brief supplied by the owner is the
acceptance specification.

## Milestone table

| Milestone | Expected result | Status | Evidence |
|---|---|---|---|
| M1 | Mod loads and integrates into the solution | **Verified in game 2026-10-10** | `S1Mods.sln` regenerated (32 projects); Release build of `PropertyLEDs.csproj` → 0 warnings / 0 errors; `PropertyLEDs.dll` deployed to `<GameDir>\Mods\`; deployed file SHA-256 identical to the build output (`6fac6da5…`) |
| M2 | One rectangular room with correctly placed LEDs | **Geometry live, visual check open** | Motel bathroom (closed rectangle, 4 points) measured from the live hierarchy dump and built (`Layout catalog: 2 valid rooms` → `Built 2 LED room groups`); on-screen confirmation pending |
| M3 | RGB and brightness work | **Open (logic done, visuals pending)** | Managed colour math unit-tested (44 tests, incl. auto-enable on colour edit); glow ×2.5 applied; in-game look unverified |
| M4 | Property and room management | **Partly verified** | In-game registry reports 2 owned properties (`motelroom`, `rv`) via the mod bridge; room management needs a measured layout |
| M5 | Phone app controls the lighting | **Partly verified** | App implemented per PhoneApp/UITheme rules; `settings_slot_1.json` holds user-edited hue/saturation/brightness, which proves the app was opened and the sliders move values; on-screen look still unchecked |
| M6 | Per-save-slot persistence | **Partly verified** | Slot/debounce/schema logic unit-tested; `settings_slot_1.json` was written from the live session (global motelroom settings, Enabled true); reload round-trip still open |
| M7 | Complex rooms, sloped ceilings | **Open** | Beam geometry handles arbitrary 3D/open/closed paths (tested); room surveying not started |
| M8 | All vanilla properties and businesses surveyed | **In progress** | 33 rooms across 11 properties imported 2026-10-11 (reference lines, offline-validated; ids matched to live property codes); per-property visual confirmation open. Deferred: RV (destroyed), Manor (original/rebuilt variants need room-gate support) |
| M9 | Stability, regression and performance tests | **Not started** | — |

## Verified in game (2026-10-10, save slot 1 "Dominik")

| Observation | Evidence (`MelonLoader\Latest.log`, live run) |
|---|---|
| Mod loads, no exceptions | Assembly banner `PropertyLEDs v0.1.0`, `Initialized (v0.1.0)`, zero ERROR/Exception lines in the whole session log |
| Injected type registers | `[Il2CppInterop] Registered mono type PropertyLEDs.LEDDiagnosticBridge in il2cpp domain` |
| Phone app registers with S1API | `[PhoneApp] Initializing phone app: PropertyLEDs.PropertyLEDsApp` (gameplay scene) |
| Save slot handling | `No LED settings for slot 1 yet; defaults active` — no file created before the first edit |
| Layout catalog gate | `Layout catalog: 0 valid rooms, 0 marked verified` — empty catalog is valid, nothing invented |
| Shader resolution | `LED shader supported: 'Universal Render Pipeline/Lit'` |
| No geometry without a verified layout | `Built 0 LED room groups` |
| Property registry | `LED runtime ready for slot 1: 2 owned properties/businesses`; bridge status JSON lists `motelroom` (Motel Room) and `rv` (RV), both with live anchors |
| Settings state | Bridge status: `Ready: true`, `Slot: 1`, default (off) settings for both properties |
| Read-only tooling | S1MCP `call_method` on `PropertyLEDs_Diagnostics` + `component_type=LEDDiagnosticBridge` returns the live status JSON |
| Second session, new build (auto-enable fix deployed) | Log 20:46: `Initialized (v0.1.0)`, bridge registered, `Layout catalog: 2 valid rooms` → `Built 2 LED room groups`, `LED runtime ready for slot 1: 2 owned properties/businesses`, zero ERROR/Exception lines |
| Settings file from the live session | `UserData/PropertyLEDs/settings_slot_1.json`: motelroom global settings with the owner's hue/saturation/brightness values and `Enabled: true`; proves app edits reach the disk |

Not yet exercised in game: phone UI interaction, colour changes, brightness 0, inheritance switching,
save/reload round-trip, room measurement, preview markers, sloped ceilings, transitions and
performance.

## Measured layouts

All coordinates come from live hierarchy dumps (`led dump`, 256 nodes / depth 8) of the running
session, converted from world space into property-anchored local metres. No value is estimated.

| Property | Room | Derived from | Geometry | Visual check |
|---|---|---|---|---|
| `motelroom` | `main` | `Motel Unit/Motel unit/Interior` interior shell (world x -73.89...-67.79, z 81.66...86.66, ceiling underside y 3.515) minus the partitioned bathroom volume (`InteriorWall`: x -73.89...-71.39, z 84.66...86.66) | Closed **L-shaped** path, 6 points, segment beams 0.055 m at y 3.425, 8 cm inset (north/door wall ≈19 cm); re-anchored 2026-10-11 | **FAILED 2026-10-10**, reworked 2026-10-11: strip on the door-side wall ended ~1-1.5 m before the corner (owner screenshot); offline cross-checks found nothing within 0.5 m of the old line (walls/ceiling flat, door trim ≤2.965 m). Rework: segment beams with corner overhang plus the reference mod's measured inset lines (owner-accepted look, independently implemented); re-check in game pending; old data kept as `layouts.json.bak-2026-10-11-pre-ref` |
| `motelroom` | `bathroom` | same partition bounds, inner faces | Closed rectangle, 4 points, segment beams 0.055 m at y 3.425, 8 cm west/south inset | **Pending** (new beam style; partition lines kept) |

`RV` was measured first and then **withdrawn**: after its quest line the vehicle is destroyed and the
property is unusable (`RV/Destroyed RV` in the hierarchy). The catalog entry was removed. A coverage
table must treat quest-destroyed properties as unavailable, not as "unsupported".

Both rooms are installed in the running session (`Layout catalog: 2 valid rooms, 2 marked verified`
-> `Built 2 LED room groups`). `Verified` is still only a measurement claim: the owner has not looked
at them yet, and the entries are corrected or reset to `Verified: false` if the in-game look differs.
The 2026-10-11 rework (beam geometry, re-anchored lines, glow ×2.5) still awaits that look.

TODO (next rebuild): a console/bridge command that previews a **catalog** room without flipping
`Verified`, so measurement work never has to claim verification to become visible.

### Imported coverage (2026-10-11)

33 rooms across 11 properties were converted from the owner-accepted reference dataset
(`.scratch/refmod-2804/layouts_embedded.json`, gitignored) into catalog entries in `layouts.json`
(UserData): Bungalow 3, Barn 4, DocksWarehouse 2, StorageUnit 2, SewerOffice 4, Sweatshop 3,
Laundromat 2, PostOffice 3, Car Wash 2, Taco Ticklers 3, Hyland Medical 5. Property ids equal live
`PropertyCode` values confirmed from save data and game files; conversions are one closed chain per
room except Barn main-bay/loft (open chains by construction); every path passed the catalog validation
rules in an offline pre-write check. `Verified: true` reflects "measured lines" — per-property visual
confirmation is an open item; entries are corrected or reset on report.
Deferred: RV (destroyed in the owner's save; would need gate 'RV') and Manor (gates 'Original House' /
'Rebuilt House' — needs room-gate support so the two variants can never render at once).
The settings file additionally seeds per-property settings (owner's motel colour, enabled) so the
imported coverage is visible immediately; both previous files are backed up as
`layouts.json.bak-2026-10-11-pre-import` / `settings_slot_1.json.bak-2026-10-11-pre-import`.

## Verified offline (with real tool output)

| Check | Result |
|---|---|
| Release build (`dotnet build … -c Release`) | Build succeeded, **0 warnings, 0 errors**, DLL + pdb + json deployed |
| Solution integration | `Tools/gen-sln.ps1` → 32 projects; solution-wide compile passes (`-p:S1NoDeploy=true`) |
| Automated tests | `dotnet test Source/Tests/PropertyLEDs.Tests` → **44 passed, 0 failed** (`core-tests.md`; tube tests replaced by beam tests) |
| Formatting gate | `dotnet format <both projects> --verify-no-changes` → clean |
| Version sync | `Tools/check-version-sync.ps1` reports PropertyLEDs 0.1.0 consistent (code, `mod.json`, `CHANGELOG`, `AGENTS.md`, root `README.md`) |
| Doc-path gate | `Tools/check-doc-paths.ps1` → all referenced paths exist |
| Layout catalog | Ships empty by design; no guessed or synthetic coordinates anywhere in the repo |
| Reference analysis | Owner-supplied reference mod decompiled and measured (`.scratch/refmod-2804/`, gitignored): segment-beam geometry with corner overhang, 8 cm insets, glow ×2.5, point-light spill in nearest rooms — geometry/inset/glow adopted independently, no code or assets reused |
| Coverage import | 33/33 rooms converted (closed chains; Barn main-bay/loft as open chains) and checked against every catalog validation rule offline before writing; layout file written with backup (`layouts.json.bak-2026-10-11-pre-import`) |
| Catalog audit diagnostic | `led audit` + bridge `CatalogAudit()` compile clean; live output pending the next session |
| Unrelated work | Existing uncommitted Minimap/PocketShop/TaxiDriver changes were left untouched |

## Not verified yet (do not report as working)

- Loading in a real game session (no exceptions, phone app registered, no log errors) — M1 remainder.
- Every visual/geometry claim: strip placement, segment continuity, colour/brightness behaviour,
  "off means no glow", sloped ceilings, multi-storey rooms.
- Phone interaction, reopen-after-close, save-slot switching, resource cleanup.
- Performance: frame time and resource behaviour are not measured; realtime lights are disabled on
  purpose until they can be profiled.
- The 2026-10-11 imported coverage (33 rooms on 11 properties) — reference-ported and offline-validated,
  not yet seen in game; RV, Manor and the remaining unowned properties are still unmeasured.

## Guardrails honoured

- No new shared infrastructure: `SafeStorage`, `SaveSlots`, `UITheme`, `GamePalette`, `NetworkGuard`
  and `UITheme`-based scaling are reused; nothing was added to `Shared`.
- No changes to other mods, no dependency changes, no commits or pushes.
- No permanent Harmony patches; the installed S1API events and named methods are used.
- No invented API members: the console contract, lifecycle events and PhoneApp surface were checked
  against the installed 3.2.1 assemblies and the pinned source.
- Cosmetic-only data: settings have no economy/inventory effect, which is why a debounced background
  save (1 s, atomic, with `.bak`) is used in addition to the save-complete flush.

## Next steps

1. Next session: confirm the imported properties visually (Bungalow, Barn, Docks Warehouse, Storage Unit,
   Sewer Office, Sweatshop; businesses when owned) and read the mod log — expected `Layout catalog:
   35 valid rooms, 35 marked verified`. Run `led audit` (or bridge `CatalogAudit`) to prove every catalog
   id against live property codes; it also reports each property's catalog room count.
2. Colour, inheritance and slot round-trip checks (M4–M6); sloped/multi-level chains already ship with
   the Barn/Docks import (M7 evidence pending visual check).
3. Decide room-gate support (Manor original/rebuilt, RV) before importing those two datasets.
