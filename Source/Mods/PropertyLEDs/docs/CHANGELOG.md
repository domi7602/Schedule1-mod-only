# Changelog

All notable changes to PropertyLEDs are documented here. Format: Keep a Changelog;
versioning: SemVer.

## 0.1.10 (2026-10-25)
- Tighten the landscape phone insets from 4.5 percent to about 1.2 percent and move the title band up so the controls sit closer to the glass. Inner card padding, sliders, selection and lighting are unchanged.
- **Verified, owner-confirmed 2026-10-25.** Dominik confirmed the open landscape and tighter-inset look works. `Latest.log` 14:53:44–15:11:59 loaded v0.1.10, DLL `31E6E129…`, initialized `PropertyLEDsApp`, reported 35 verified rooms, built 5 LED groups and marked slot 1 ready for 3 owned properties. The log does not measure the 1.2 percent insets or edge clearance; those are owner-confirmed, not log-measured. An earlier screenshot the same day showed landscape only, not these insets.
- Deployed 2026-10-25 with the game closed: installed `PropertyLEDs.dll` SHA-256 `31E6E1299F3C40B3DD0A5853510E6FE8D2FD30AA6DDF7042E26FB52673F516CD` (125,952 bytes). Release build 0 warnings/errors; 116 tests pass.

## 0.1.9 (2026-10-25)
- The phone app opens landscape, same orientation contract as Minimap and LightControl (`EOrientation.Horizontal`, `Phone.SetIsHorizontal(true)`). Theme scale follows the visible short side so the portrait UITheme helper does not inflate controls on a wide canvas. Control stack, settings and lighting are unchanged. Native landscape appearance is not verified.
- Deployed 2026-10-25 with the game closed: installed `PropertyLEDs.dll` SHA-256 `75B57C386B26B44E4E2DB24D4D216C3640452CF26F8CE8823F311F5DCCBB851C` (125,952 bytes); installed metadata reports `0.1.9`. Release build 0 warnings/errors; 116 tests pass.

## 0.1.8 (2026-10-20)
- On the first actual app open after a load, select the local player's exact current owned property/business code when available; never infer it from distance. Preserve manual selection across close/reopen; outdoors or unavailable identity falls back to the existing/first owned selection.
- Name the selected property in the power status, with the enabled/disabled state first so long names cannot hide it. The reported motel/OFF mismatch showed Greenhouse Island selected while the motel settings were enabled; no renderer/toggle repair was justified. Existing room-override and property-edit semantics are unchanged.
- Preserve manual selection after transient refresh failures; only actual load/scene invalidation re-arms initial adoption.
- Prevent hidden UI construction from consuming the first-open selection. Log `Phone property selection: selected=..., current=...` when resolving it.
- 115 tests pass (pure selection/status decisions and supplementary source contracts, including observed failing hidden-construction/refresh-error guards and state-first text expectations before their fixes). Independent review and final delta re-review PASS. Forced Release build/deploy: 0 warnings/errors; installed DLL/PDB/metadata hashes match, installed metadata is 0.1.8, and both new policy/text types are present in the installed assembly. Shared and layout catalog are unchanged. Native location detection, status fitting, toggle/color/selection and save roundtrip acceptance remain In-Game-Verify open.

## 0.1.7 (2026-10-18)
- Centralize the phone typography ramp and increase selector names, slider labels/values, buttons, help text and subtitle. Increase the corresponding selector bands and control heights without changing palette, slider ranges, settings or lighting.
- Minimum content budget is now 472 dp collapsed / 538 dp expanded; small screens retain scrolling. Room bar and color preview remain removed.
- 94 tests pass (typography RED/GREEN observed). Independent scoped review PASS; Release deployed with 0 warnings/errors and DLL/PDB/metadata hashes matched; the installed layout catalog hash is unchanged. Native readability, clipping and pointer acceptance remain open.

## 0.1.6 (2026-10-17)

### Changed

- The phone no longer shows the Room / Scope card, the color preview bar, or the
  "Use property color" button. Slider edits apply to the property color. Saved
  per-room overrides are not deleted and still win on those rooms.

## 0.1.5 (2026-10-17)

### Changed

- Phone controls use the Phase-1 sizes: selector 68, slider heading/track 24/28, preview 58,
  actions 52, power 64, advanced 32, gap 12, padding 4. Slider indices, ranges and edit
  semantics are unchanged.
- Palette is the authorized deeper set: background `(0.035, 0.055, 0.075)`, surface
  `(0.07, 0.11, 0.145)`, accent `(0.15, 0.72, 0.88)`, border `(0.18, 0.28, 0.36)`.
- Each slider block has a translucent group fill `(0.06, 0.09, 0.12, 0.45)`. The power card
  fill is `(0.08, 0.13, 0.17)`, lifted off the normal surface. The power switch is still the
  only on/off control.
- Inline status color follows the message: unverified placement is warm yellow, a message
  containing "failed" is red, everything else stays neutral. Color is applied on every call.
- Selector text windows stay 36-56 / 14-37 / 1-15. Inflating every band by 3 dp per edge
  would overlap the caption and the name inside a card that only grew by 5 dp. The taller
  card is the extra air.
- Opening Advanced now exceeds the 88.5% scroll viewport by about 2-4 dp (minimum 666 at
  scale 1). That state scrolls. Collapsed content (602) still fits and still stretches.
  Controls are not shrunk below their minimum.

### Verification

- 90 tests passed. Compile-only Release build: 0 warnings / 0 errors. Independent scoped
  review PASS, no blocking findings.
- In-game visual acceptance is open. Tests do not prove native gradients, pointer input,
  reopen behavior or scrolling.
- Geometry, capture flow and settings schema were not changed.

## 0.1.4 (2026-10-13)

### Changed

- Remove the bottom information icon and normal coverage/scope footer. Extend the scroll viewport
  from 80% to 88.5% of the phone height, ending at a 1.5% bottom margin. Loading, unavailable anchors,
  unverified-placement warnings and actual edit/refresh errors remain in an unobtrusive inline row
  only when needed; inactive state messages reserve no height.
- Distribute real measured available height proportionally across selector cards, RGB preview,
  slider breathing room, Advanced, power and preset rows plus group gaps. Explicit priority-10
  minimum/preferred contracts drive cached native controls; content height is set directly with the
  factory ContentSizeFitter unconstrained, avoiding a feedback loop. Controls grow at most 1.5x and
  gaps at most 3x; exceptionally oversized viewports stop at those bounds rather than inventing spacers.
- Preserve usable minimum heights and scrolling when expanded or small-screen content exceeds the
  viewport. Reapply only on material measured width/height or state changes; stale first-open rects
  converge, without scene discovery, hierarchy scans or per-frame canvas force updates.
- Keep selector text centered and slider handles circular while controls stretch. Unity Slider drives
  cross-axis handle anchors to 0..1, so its parent now has a fixed 23 dp height and handle height delta
  is zero. No switch-silhouette redesign or runtime/control/slot/geometry changes are bundled.

### Verification

- Observed RED -> GREEN for measured 1201-unit sizing (839.1 -> 1062.9 content), footer removal,
  native sizing/state wiring and circular-handle constraints. 80 tests pass, including smaller/larger,
  collapsed/expanded, sum-of-contracts, overflow and convergence cases. Compile-only Release build:
  0 warnings / 0 errors, S1NoDeploy=true and BuildProjectReferences=false.
- Updated offline visual comparison uses the owner's current screenshot and the responsive native
  policy. 15 browser demonstration checks pass; CDP captured collapsed, expanded and small-expanded
  screenshots with zero JavaScript exceptions. Browser rendering is not Unity acceptance.
- Next runtime breadcrumb includes viewport/content/expected, Advanced/inline state, selector,
  preview, slider, power, action and gap sizes plus overflow. First-open/reopen, native circles,
  switch mask, pointer input and scrolling still need authorized deployment and in-game acceptance.
- No deploy, game start, commit, push or UserData writes. Catalog SHA-256 remains
  07bbc5620ea3eac1d45e736af5dd76c377565ca39dd2481bc6ee72a0d75f5f11.


## 0.1.3 (2026-10-13)

### Changed

- Correct the earlier localization mistake: all current UI labels, source and tests are English.
  Normal controls are Color, Saturation and Brightness; Color intensity remains under Advanced.
- Rebuild the phone view around the supplied mockup: bulb/title/subtitle, compact outlined navy
  selector cards and icon tiles, house/layers icons, rounded selected-color preview with a subtle
  selected-color border, rainbow/white-to-color tracks, round handles and drawn row/action icons.
- Preserve real previous/next scope selection rather than introducing a nonfunctional dropdown.
  Use one stateful power switch as the sole on/off action; no dead gear or duplicate power button.
  White and Use property color remain functional; disabled inheritance text/icons stay legible.
  The footer shows actual selected-property layout coverage and scope, not an unproven Saved claim.
- Preserve all four ranges/indices/settings, edit auto-enable, zero-brightness and inheritance rules.
  No runtime lighting, catalog, settings, other mods or Shared files changed. Native assets are
  destruction-owned; reopen sync, fresh-open Advanced collapse and immediate layout rebuild remain.
- Fixed layout budgets: 524 dp collapsed / 580 dp expanded, nine/ten active outer children.
  The viewport remains scrollable and measured until native first-open layout converges.

### Verification

- Observed RED -> GREEN for English labels/Advanced, mockup/power/layout contracts, round gradient
  alpha/hidden factory mask, and inheritance disabled-state wiring. 64 tests pass; compile-only
  Release build uses S1NoDeploy=true and BuildProjectReferences=false (0 warnings / 0 errors).
- Measured the supplied 1672 x 941 PNG and rendered the 291 x 592 phone crop beside a browser
  preview using the native sizing constants and icon predicates. Seven browser interaction checks
  pass with zero JavaScript exceptions. See visual-preview/README.md for geometry deltas and limits.
- HTML is not Unity evidence. The flatter palette, no glow, extra selector context, /5 readout position
  and intentional simplified controls still differ from the reference. Native masks, gradients,
  pointer input, first-open/reopen and persistence acceptance remain open. No deployment, game
  start, commit, push or UserData writes.

## 0.1.2 (2026-10-12)

### Changed

- Simplify the approved colour controls: **Color** replaces Hue with a full rainbow track
  (red at both endpoints, no degree readout); **Saturation** uses a
  white-to-selected-colour track and percentage; **Brightness** retains the 0–5 range/readout.
  The preview still shows the selected RGB. Property/scope cards, actions and footer are unchanged.
- Move the existing Color intensity control to **Color intensity** under **Advanced**.
  Build the panel once, toggle it with SetActive and rebuild cached scroll content immediately.
  Every fresh app open starts collapsed; toggling and scope navigation never apply settings.
  All four original slider indices, values, callbacks, inheritance, persistence and edit-auto-enable
  semantics remain intact. Procedural gradient sprites/textures are released only on destruction.
- Dynamic sizing is 560 dp collapsed / 620 dp expanded (10/11 active outer children).
  Layout breadcrumbs include the expanded state and matching expected budget; scrolling remains enabled.
  The previous-session log measured the compact viewport at 602.6 × 948.8, not this new UI.
- Strict vertical RED -> GREEN cycles: updated three collapsed-budget cases and added seven cases
  (three expanded-budget and four source-wiring cases). All 58 tests pass; no-deploy Release build
  reports 0 warnings/errors. Native rendering, pointer input, scrolling and reopen acceptance remain
  pending. No deployment, game start or runtime settings/catalog edits were performed.

## 0.1.1 (2026-10-12)

### Changed

- Phone UI structure only: property/room captions, names and context now share compact 68 dp
  cards with fixed anchored arrow hit boxes. Larger captions/names and slider labels/live values
  replace the tiny detached captions; palette, colour preview, actions and status semantics stay unchanged.
- Group each slider heading with its unchanged 32 dp track; use consistent 4 dp content gaps,
  6 dp padding and high-priority fixed heights to prevent preferred-text-size inflation.
  The 576 dp stack fits the existing 79% viewport at the measured 1201-unit phone baseline
  (sizing calculation, not native-rendering evidence). Scrolling remains available on smaller viewports.
- Rebuild on open and report cached measured viewport/content dimensions when they change via
  `Phone layout: viewport=...`, without scene discovery or per-frame rebuilding. Native visual,
  hit-test and reopen verification remain open; this build was not deployed.
- Seven test cases added using observed RED -> GREEN cycles; 51 tests pass, and compile-only
  Release build reports 0 warnings/errors. Settings, layouts, navigation and LED runtime are untouched.

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
