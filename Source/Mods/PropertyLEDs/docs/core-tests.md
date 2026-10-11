# Automated test report — PropertyLEDs.Core

Command:

```pwsh
dotnet test Source/Tests/PropertyLEDs.Tests/PropertyLEDs.Tests.csproj -c Release
```

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
