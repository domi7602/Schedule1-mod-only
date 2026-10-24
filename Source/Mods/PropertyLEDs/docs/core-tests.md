# Automated test report — PropertyLEDs.Core

## Current 0.1.8 evidence (2026-10-20)

Parent-run Release suite: 115 passed, 0 failed/skipped, using `-p:S1NoDeploy=true -p:BuildProjectReferences=false`.
Selection/status policies cover exact ordinal matching, current-property adoption, fallback, manual selection retention, missing ownership and load invalidation. Source contracts additionally pin native identity access, named status, close/load wiring and the first-actual-open gate. The hidden-construction contract failed before the `IsOpen()` guard was added, then passed. These tests do not exercise native player location, Unity labels, pointer input or renderer state; in-game acceptance remains open.

## Historical evidence

Command:

```pwsh
dotnet test Source/Tests/PropertyLEDs.Tests/PropertyLEDs.Tests.csproj -c Release
```

Result on 2026-10-17 after removing the room bar and color preview (0.1.6):

```text
Passed!  - Failed: 0, Passed: 90, Skipped: 0, Total: 90
```

Release build 0 warnings / 0 errors, then deployed. Native rendering is not covered.

Result on 2026-10-10 (local .NET SDK 8.0.425, `net6.0`):

```text
Passed!  - Failed: 0, Passed: 44, Skipped: 0, Total: 44, Duration: ~125 ms
```

Result on 2026-10-11 after the `src/Core/` flatten (test links updated to explicit includes):

```text
Passed!  - Failed: 0, Passed: 45, Skipped: 0, Total: 45, Duration: ~145 ms
```

Result on 2026-10-11 after the tube→beam geometry switch (`GenerateTube` removed; seven tube tests replaced by six beam tests):

```text
Passed!  - Failed: 0, Passed: 44, Skipped: 0, Total: 44, Duration: ~136 ms
```

The test project links the Unity-free sources directly (an explicit `<Compile Include>` list — the
files sit flat in `src/` since the `src/Core/` flatten, there is no `Core/` subfolder), so it runs
everywhere — no game installation, no IL2CPP, no MelonLoader.

Result on 2026-10-12 after the UI structure update (0.1.1):

```text
Passed!  - Failed: 0, Passed: 51, Skipped: 0, Total: 51, Duration: 108 ms
```

Seven additional cases followed observed RED -> GREEN cycles: three pure layout-budget cases
(750, 1201 and 1500-unit phones) plus four source-wiring contracts for cards, factory mask-root
arrow bounds, grouped slider typography and measured-layout wiring. The tests do not instantiate
Unity components, render a phone or exercise pointer input.

Result on 2026-10-12 after the approved simplified colour UI (0.1.2):

```text
Passed!  - Failed: 0, Passed: 58, Skipped: 0, Total: 58, Duration: 101 ms
```

Vertical RED -> GREEN evidence: the three default budget cases first failed with 576 dp instead
of 560 dp; three expanded cases failed because the state-aware budget was absent; each of four
new source contracts failed before its native wiring was implemented (advanced panel, plain English
labels/no degrees, rainbow track/cleanup, saturation track/cache). The final budgets are 560/620 dp
at scale 1, with ten/eleven active outer children. Source contracts verify built-once SetActive,
fresh-open collapse, retained intensity slots, no Apply in the toggle, rebuild/measurement wiring,
unchanged ranges and cached gradient resources. These do not prove Unity rendering or pointer input.
Compile-only Release build: 0 warnings / 0 errors, no deployment.

Result on 2026-10-13 after the English mockup correction (0.1.3):

```text
Passed! - Failed: 0, Passed: 64, Skipped: 0, Total: 64
```

English-label and Advanced tests were observed failing before localization correction. New mockup
contracts failed before the corresponding view work: stateful single power action, drawn icons/cards,
new compact geometry, hidden factory mask/rounded gradient alpha, readable disabled inheritance.
Pure budgets now use 524/580 dp, with nine/ten outer children. The earlier draft expectation of
594/647 was a test arithmetic error (100 dp too large), corrected against the explicit component sum.
After rendering, selector height/gaps were increased from 60/5 to 63/8 dp in another observed
seven-case RED -> GREEN cycle, bringing the top cards and preview closer to reference measurements.
Existing value/range, auto-enable/zero-brightness, persistence and inheritance tests still pass.
Browser verification adds seven passing demonstration interaction checks and zero JS exceptions;
it does not exercise the native PhoneApp. Compile-only build: 0 warnings / 0 errors, no deployment.

Result on 2026-10-13 after footer removal and responsive full-height layout (0.1.4):

```text
Passed! - Failed: 0, Passed: 80, Skipped: 0, Total: 80
```

Observed RED -> GREEN: 1201-unit measured sizing failed at 839.1 instead of 1062.9; the footer
removal, cached sizing/state application and constrained circular-handle source contracts each
failed before their implementation. Additional regression coverage checks 750/1201/1500 phones,
collapsed/expanded sums, small-screen overflow, conditional inline-state height, extreme growth
caps, stale-first-open convergence and stable no-rebuild decisions. Replaced obsolete footer
contracts rather than reintroducing status text to satisfy them. One assertion initially straddled
float rounding at 1062.885; it now uses a 0.01-unit absolute tolerance.

80 tests pass; compile-only Release build has 0 warnings/errors. 15 browser demonstration checks
pass with zero JS exceptions. These tests link pure sizing code and inspect native source wiring;
they do not create Unity controls or prove pointer input, masks, native layout or persistence.

## What is covered

| Area | Representative tests | Expectation |
|---|---|---|
| Colour conversion | `Rgb_is_normalized_hsv_independent_of_enabled_and_brightness` (8 data rows), `White_is_enabled_and_clones_are_independent`, `Default_settings_are_disabled_neutral_white` | Hue/saturation/intensity map to bounded RGB; NaN/∞ inputs fall back to defined values; a fresh setting is off, neutral and does not mutate shared state |
| Parameter validation | `Normalize_repairs_finite_ranges` (4 rows) | Out-of-range and non-finite values are clamped to 0..1 (brightness 0..5), never propagated |
| Property inheritance | `Global_edit_clears_all_overrides_for_only_target_property`, `Room_override_changes_only_that_room_and_detaches_input` | A property edit replaces the default and removes that property's room overrides only; a room edit affects one room and never the input object |
| Reset to property colour | `Use_property_removes_only_target_override` | Removing an override restores inheritance without touching other rooms |
| Invalid data | `Invalid_ids_are_rejected_without_creating_data` (null/empty/blank), `Normalize_repairs_null_graphs_and_ranges_without_inventing_overrides`, `Empty_state_returns_detached_disabled_defaults` | Rejected edits create no state; malformed graphs are repaired or dropped; reads return detached defaults |
| Schema versions | `Json_load_boundary_distinguishes_missing_from_corrupt_and_future_files`, `Unknown_version_clone_preserves_data_but_refuses_edits_and_effective_lights` (0/2/±) | Missing → writable; corrupt/duplicate-key → not writable; future version → data preserved, edits refused, no file rewrite |
| Persistence round-trip | `Valid_state_roundtrips_and_known_null_graphs_normalize_without_slot_crosstalk` | JSON survives a load/serialize round-trip byte-identically; the original text is retained for anti-overwrite decisions |
| Slot session isolation | `Independent_slot_sessions_clone_loads_and_only_acknowledge_their_own_edits`, `Session_room_actions_follow_inheritance_and_do_not_dirty_rejected_actions` | Two sessions over the same loaded data never share mutations; only the editing session becomes dirty and acknowledges its own revision; blocked slots (no/invalid/future slot) never write |
| Clock safety | `Rejected_backward_clock_never_mutates_session_state`, `Debounce_clock_rejections_and_failed_writes_preserve_pending_changes` | Backwards/NaN/negative timestamps are rejected before any state change; a failed write never acknowledges its revision; the debounce timer is re-armed by new edits |
| Debounce | `Debounce_reschedules_and_requires_successful_revision_acknowledgement` | Edits coalesce inside the window, a forced flush bypasses it, and only a matching revision may be marked saved |
| Geometry generation | `Straight_beam_is_a_finite_box_with_overhang_and_outward_triangles`, `Adjacent_beams_extend_half_width_past_every_corner_so_corners_stay_covered`, `Open_and_closed_paths_emit_one_beam_per_segment`, `Singular_u_turns_return_empty_instead_of_degenerate_geometry`, `Unrepresentable_float_coordinates_fail_safe_instead_of_emitting_collapsed_triangles`, `Malformed_paths_and_widths_return_empty_without_throwing` | One outward-wound box per segment, each reaching `width/2` past both endpoints so adjacent beams overlap at every corner; correct segment counts for open/closed paths; degenerate input fails closed with an empty mesh |
| Layout data | `Catalog_defaults_to_empty_supported_version_without_shipped_coordinates`, `Catalog_validation_rejects_unknown_versions_and_ambiguous_room_ids_without_mutation`, `Room_validation_refuses_unverified_missing_ids_invalid_width_and_null_paths`, `Paths_validate_finite_3d_coordinates_distinct_vertices_and_bounded_counts` | An empty catalog is valid (no invented coordinates); duplicate/ambiguous rooms, unverified rooms, invalid widths, non-finite points, duplicate vertices, near-180° reversals and oversized paths are rejected without mutating the input |

## What is NOT covered by these tests

The following require the real game and are therefore tracked as unverified until checked in a live
session (see `implementation-status.md`):

- Unity rendering (mesh upload, URP material/emission behaviour, visibility, "no glow when off").
- Phone app behaviour (layout, input handling, reopen after close, save-slot switch).
- Lifecycle and ownership: property registry contents, anchor resolution, scene transitions, object
  cleanup, absence of duplicates after repeated load/unload.
- Physics/performance impact on the game (frame time, allocation/leak behaviour).

A green test run and a warning-free build prove the managed logic and the API surface, not gameplay
behaviour.
