# Changelog

All notable changes to PropertyLEDs are documented here. Format: Keep a Changelog;
versioning: SemVer.

## Unreleased

### Changed

- **Segment-beam geometry** replaces the swept tube: each path segment is an independent square beam
  with a `width/2` overhang past both endpoints, so neighbouring beams overlap at every corner and the
  strip stays visibly continuous around it. Independently implemented (aligns with the owner-accepted
  reference look, no code reused); `GenerateTube` and its tests were removed, beam-math tests added
  (44 tests, see `core-tests.md`).
- **Glow ×2.5**: the combined emission/base colour is multiplied by 2.5, matching the reference
  brightness.
- Layout data (UserData, not shipped): motel room and bathroom re-anchored to the reference lines —
  5.5 cm beams, ceiling height y 3.425 m, 8 cm wall inset (north/door wall ≈19 cm, clear of the door
  trim zone). Visual check pending.
- **Vanilla coverage import (2026-10-11)**: 33 rooms across 11 properties (Bungalow, Barn, DocksWarehouse,
  PostOffice, Laundromat, Taco Ticklers, Hyland Medical, Sewer office, Car Wash, StorageUnit, Sweatshop)
  converted from the owner-accepted reference dataset — property ids matched to live `PropertyCode` values,
  one closed path per room (Barn main-bay/loft remain open chains), width 0.055 m; every path validated
  offline. Motel lines unchanged (visually confirmed). RV and Manor deferred (state gates). Per-property
  visual confirmation open; entries corrected or reset on report.
- **`led audit` / bridge `CatalogAudit()`**: read-only diagnostic listing every native property/business
  (owned or not) against the catalog — code match, room counts, dangling ids; no scene scans, no state changes.

## 0.1.0 (2026-10-10) — first implementation (experimental)

Initial independently developed implementation. **No room coordinates ship with this version**;
rooms become visible only after they are measured in game and marked verified in `layouts.json`.

### Added

- **Settings model and inheritance** (`PropertyLEDs.Core`): property defaults, per-room overrides,
  white preset, clamped hue/saturation/colour-intensity/brightness, "use property color" and the
  rule that editing a property default clears all room overrides for that property.
- **Slot-isolated persistence**: `settings_slot_{n}.json` written atomically with `.bak` rotation,
  1 s debounce, no writes in menus or for an invalid/unknown slot, and a read-only guard that keeps
  corrupt or newer-schema files untouched instead of overwriting them.
- **Owned property/business registry** built from the live S1API/native owned lists, with stable
  property codes as the primary key, duplicate/ambiguous codes skipped, and no per-frame searches.
- **Runtime LED rendering**: one merged mesh and one material per room (URP Lit, emissive, no
  colliders, no realtime lights), rebuilt only on real layout/settings changes; brightness 0 or
  "off" fully disables the renderer instead of leaving a glow.
- **Geometry**: circle-swept tube meshes for open and closed 3D paths with parallel-transport
  frames, validated widths, rejection of degenerate/reversing paths and fail-closed mesh building.
- **Phone app "Property LEDs"**: property and room selectors, four sliders with live values, colour
  swatch, white preset, on/off toggle, "Use property color", status/geometry indicator, controls
  disabled during invalid lifecycle states, navigation without side effects, no UI destruction on
  phone close and no duplicate listeners on reopen. Colour edits switch the LEDs on automatically
  (brightness 0 stays off); the toggle reads "Turn LEDs ON/OFF".
- **Measurement and diagnostics tooling**: bounded hierarchy dump with local/world transforms and
  renderer bounds, manual and aimed point capture, temporary markers plus LED polyline preview,
  candidate layout export, explicit layout reload, and a primitive MCP-callable bridge.
- **Console command** `led` covering survey, capture, preview, export, reload and manual colour
  control (see `diagnostics.md`).
- **Automated tests** for colour conversion, inheritance, override/global-clear/reset semantics,
  persistence tri-state handling, slot-session isolation, debounce behaviour and geometry
  generation — 44 tests, see `core-tests.md`.

### Known limitations

- No built-in room layouts for any property; surveying in game is still required.
- Emissive strips only — no realtime lights yet (deliberately disabled until profiling).
- Unverified layout candidates are preview-only and must be checked visually before `Verified: true`.
- Multiplayer synchronisation and third-party property mods are out of scope for 0.1.0.
