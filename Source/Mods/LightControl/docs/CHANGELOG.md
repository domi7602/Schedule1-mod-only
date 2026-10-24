# LightControl

## 0.1.0-dev.19 (2026-10-22)
- Native port of the approved graphite HTML app area: owned 1201 x 655 reference canvas, larger Segoe UI typography with Arial fallback, outlined lamp icons, four aligned columns, explicit color controls, brightness meters and ellipsis actions. A single sticky column heading retains its list spacer and blocks clicks through to covered rows.
- Color chooser matches the 600 x 456 reference, with ten 80-unit fields in a 5 x 2 grid, Rainbow at bottom-right, selection checks and 54-unit Cancel/Apply buttons. Rainbow now stages through the exact-session/source picker; Original opens without a pending choice. Brightness retains seven immediate steps in one row. Property copy remains immediate; all-property copy has its own frozen source/settings/count confirmation card.
- Per-widget forwarding bindings avoid S1API's global Action-equality deduplication, which previously dropped repeated CloseModal registrations. Binding targets are released before exact native listener removal; cached owned fonts, glyphs and rainbow sprites are cleaned up with the view. Shared theme scale is not read by later row/modal creation.
- Parent-observed 264/264 core/adapter/source checks and forced compile-only Release 0 warnings/errors; scoped format checks passed. Five Rainbow behavioral regressions failed before policy repair; two delegate-identity/release regressions failed through the introduced legacy-compatible binding seam, then passed with unique forwarding. Final independent delta review PASS. Native rendering, font fit, hit testing and gameplay remain open; see graphite-ui-verification.md.
- Installed the checked Release on 2026-10-22 with the game closed: DLL/PDB/metadata and all three previous dev.18 backups independently hash-verified. Candidate DLL SHA-256 `d7af584c48869411515132d8bd4c332caa2fc0d3d16c94c98e5fbf5c00f97a6c`. Retained artifacts, source snapshot and evidence are under `VerifiedBuilds/LightControl/0.1.0-dev.19-graphite`; no game launch or native visual acceptance is claimed.
- Subsequent owner screenshots on 2026-10-22 establish the native three-lamp list and open 5 x 2 chooser rendering at 1920 x 1080, including Rainbow tenth and fully visible Cancel/Apply. The startup log confirms dev.19 and the 1201 x 655 reference layout, but also reports a missing downstream font constructor: Segoe UI creation fails and the Arial fallback is active. Exact font parity and interaction/lifecycle acceptance remain open. This review changed documentation only; see graphite-ui-verification.md for the captured log identity and evidence limits.

## 0.1.0-dev.18 (2026-10-21)
- Professional list pass on the compact picker, based on the owner's in-game screenshots and the accepted V4 preview: the large circular color dots and percentage-anchored pseudo-columns are gone. Each row is now one horizontal layout with a fixed 18 dp square real-color marker, the lamp name as the primary label, the color name below it, and right-aligned **Brightness** and **Actions** columns shared with a single column-heading row.
- Row selection is a restrained background tint instead of a text `>` prefix; property groups are plain headings, the mobile-style title sits beside the preserved status line, and the long standing instruction paragraph is replaced by one compact action-feedback line.
- The color dialog keeps nine staged real-color tiles with one white selection ring, a quiet empty center note, and a clearly reserved footer. Back/Select and the single Close button now declare their own preferred and minimum height, so the child-controlled footer layout can no longer collapse them - the reported symptom where only the hint text rendered.
- A one-shot geometry probe after each modal opens logs the measured card and footer-button rects (`[ui-layout] ...`), so the next session can distinguish a real layout failure from a rendering misread.
- Test-first: the two new contracts failed against the previous UI (251/253: rows still anchor-fractioned, footer buttons without reserved height), then passed after the change; a deliberate duplicate-heading injection made the same check fail again (252/253) before being reverted, confirming the assertion is sensitive.
- Independent review of the frozen delta found one blocking defect - a second, untracked column-heading row was created outside the scroll list and would never be hidden. The duplicate call was removed, heading and row share one column-minimum constant, the status band now reserves two lines so warnings cannot paint over the table, and the review's low-severity notes were addressed or recorded.
- Parent-verified at build completion: 253/253 core/adapter/source checks, forced Release `0 warnings / 0 errors` with `TreatWarningsAsErrors`, scoped `dotnet format` and diff checks clean. Runtime/storage/native-light/Shared/persistence untouched; that compile-only slice performed no deployment, game launch, save mutation or commit. Native rendering and in-game fit stay open. See phone-color-picker-verification.md.
- Installed 2026-10-22 after owner approval: exact retained dev.18 DLL/PDB/metadata, without rebuilding. All three artifact/source-to-target hashes and all three dev.17 backup files verified; game-closed preflight passed before and after, installed Shared hash unchanged. Receipt and backup paths are in phone-color-picker-deployment.md. No new build/test run or game launch; In-Game-Verify remains open.

## 0.1.0-dev.17 (2026-10-21)
- Compact-picker redesign: replace the permanent right-hand editor with a full-width property-grouped lamp list and an item-triggered color-only dialog matching the accepted compact v3 composition. Nine true-color square tiles, strong white selection ring, current/pending name and Back/Select; no extra colors or runtime-light tuning. Initially compile-only; later installation is recorded below.
- A tile stages color only; Select consumes the exact session/source-bound choice before invoking the existing runtime once. Back, outside dismissal, phone close, session change and removed selection discard it. Original/Rainbow never silently become White; unavailable/stale sources cannot apply an old pick.
- The row's percentage opens the existing seven intensity steps, which still apply immediately on click; no extra Select confirmation. More keeps Original/Rainbow, Reset (Original/100%), counted property copy and the frozen all-properties confirmation outside the color chooser.
- Review repairs: let outside clicks reach the dismiss backdrop while cards consume interior hits; pin every scroll-content width rather than repeating the previous 100-unit overhang; validate the captured session/key at every action boundary. Bounded compact card/footer sizing retains the measured reference composition without claiming universal tiny-container fit.
- Parent-observed 251/251 integrated core/adapter/source checks, forced final Release 0 warnings/errors with compiled MelonInfo version checked, scoped format/diff checks clean, final independent delta review PASS. Actual uGUI clipping/hit testing/reopen behavior and native rendering remain In-Game-Verify open. Runtime/storage/native policies, Shared and dependencies are unchanged; implementation performed no deployment, game launch, save mutation, commit or push. Historical dev.16 installation and dev.15 UI acceptance are not acceptance of this build. See phone-color-picker-verification.md.
- Installed 2026-10-21 after separate owner go: exact retained DLL/PDB/metadata, no rebuild. Three prior files backed up and hash-verified; all three installed targets/receipt and compiled identity read back. 76 unrelated mod/library/plugin files, four LightControl sidecars and 125 native save files unchanged. No game launch; frozen compile-only metadata is superseded by phone-color-picker-deployment.md for current installation status. In-Game-Verify remains open.

## 0.1.0-dev.16 (2026-10-20)
- Audit repair/UI candidate, compile-only: all LC01-LC12 findings reviewed against the real project. Explicit read outcomes and reachable-ancestor proof replace File.Exists gating; fully validated backup recovery keeps identity/schema conflicts blocked and protects the good backup through repeated loads and the first save. Save exceptions cannot skip native teardown cleanup.
- Native intensity/color retain their baseline and per-property hand-back responsibility after failed getters/setters. Pending restoration survives rebind/teardown without dropping owned output at a cap; new influence receives backpressure. Null material slots differ from failed reads; release requires a post-write disconnect proof. Retained clones cannot be adopted as native sources, and slot changes/periodic validation invalidate converged caches.
- Common lamp/tent rebind preparation is atomic with the model transition; live property changes reconcile core/runtime, inactive entries receive bounded positive-deletion checks, and copy commands pin exact targets/catalog after ownership checks.
- English landscape property list and selected editor: stable labels, two-line rows, Original/Color/Rainbow, nine real colors and seven readable brightness buttons. Original preserves intensity; Reset restores Original/100%. Whole-editor disabling, persistent scan/storage/conflict warnings and explicit global-copy confirmation; compact Back/editor scrolling and existing clipping/listener fixes preserved.
- Static visual instructions are reused; untouched Original/100% avoids native apply; log eligibility precedes readback/formatting; unchanged open UI skips projections/layout writes. Resolver microbenchmark recorded, not a game/FPS claim.
- 233/233 integrated core/delegate/real-disk-adapter/source checks, forced Release clean, scoped format checks and independent final delta reviews PASS. No new dependencies, Shared source edits, save format changes, commit or push. Native rendering/material destruction/actual phone-container fit/gameplay remain In-Game-Verify open.
- Scope incident: one child build unexpectedly wrote Shared deployment receipts; receipt readback and baseline hashes prove no binary copies. Installed LightControl/Shared/S1API and LightControl metadata/symbols stayed unchanged. See audit-repair-verification.md for exact evidence and the later deployment artifact policy.

## 0.1.0-dev.15 (2026-10-20)
- The lamp list was 100 units wider than the window, 50 clipped on each side. Names and the color label sat in that overhang, so the mask cut the letters. The list is now pinned to the visible width.
- **Owner-confirmed 2026-10-20.** Dominik accepted the app after this repair: every label renders complete, nothing clipped. He called LightControl done. Individual lamp actions were not itemized.

## 0.1.0-dev.14 (2026-10-20)
- Text bands are sized from the font, not from a short fixed strip. The title was 18pt bold in about 23 units of height, so the tops of the letters were sliced. Status no longer uses vertical truncate. In-Game-Verify open.

## 0.1.0-dev.13 (2026-10-19) — split controls

- The phone list stays on the left, under property headers. The selected lamp's color, brightness and copy actions stay visible on the right.
- A tap sets that lamp immediately. Brightness is a direct step, not a cycle. Original color keeps brightness. Reset lamp restores original color and 100%.
- Apply to property copies color and brightness onto the currently available lamps of that property. All properties is the separate world copy. A changed target list is rejected instead of applied.
- Temporarily unloaded lamps stay registered, but a state command no longer recolours them. That is a deliberate change from the old "every registration is a target" rule.
- Save pending, save failed and saved are separate from the action message. An unchanged open app no longer rebuilds its signature every frame.

## 0.1.0-dev.12 (2026-10-19) — property groups

- Tents and Lamps chips are gone. Every property is a header, and its lamps sit under that header, the same split PotScanner uses.
- A lamp row no longer repeats the property name. Apply still copies the selected color to every tracked lamp.

## 0.1.0-dev.11 (2026-10-19) — palette matches the lamp

- The dots use the same primaries the lamp emits. The old UI reds and the mustard yellow were duller than the light.
- The gray ring is gone. Only the selected dot keeps a blue ring. Rainbow is a hue wheel, not a second purple.

## 0.1.0-dev.10 (2026-10-19) — color palette

- The cycle button is a row of color dots. Tap one to set the selected lamp, then Apply copies that color.
- The active color has a blue ring. Original is the gray dot, rainbow is the violet one.

## 0.1.0-dev.9 (2026-10-19) — list gets the height

- Filter chips and the bottom buttons no longer stretch. The lamp list takes the leftover space.
- The brightness button shows the percent, not the long heading.

## 0.1.0-dev.8 (2026-10-19) — landscape lamp list

- The phone opens landscape, like PotScanner. Lamps are a scrolling list with a color swatch, not Prev/Next.
- Chips filter All / Tents / Lamps, plus a property chip when more than one property is owned. Apply then copies onto that property only.
- 148/148 console checks. In-game verify open.

## 0.1.0-dev.7 (2026-10-19) — prologue RV is not tracked

- The RV is prologue-only, so its lights are never listed. Exclusion is by property code `rv` and the RV type, not by the explosion flag.
- Motel and other owned properties stay tracked. 147/147 console checks. In-game verify open.

## 0.1.0-dev.6 (2026-10-19) — grow tents and exploded RV

- Grow tents are now controllable. Their built-in light is a child halogen toggle (`halogen light on/off mat`), not a placed LED `GrowLight`, so the old scan never listed them. The app shows them as "Grow tent".
- Full-spectrum lights stay untouched.
- The two quest LED lights were removed once the RV reported itself destroyed. Superseded by 0.1.0-dev.7.
- 147/147 console checks. In-game verify open.

## 0.1.0-dev.5 (2026-10-18) — synchronized LED panel + emitted light

Parent reran **143/143** console checks; independent targeted review **PASS**.
Gameplay acceptance remains open. This release adds the coupled light path described below;
dev.4 only shipped the material-identity repair, not this combined behavior.

### Unified scope (supersedes the surface-only contract)
- Owner requirement: the light a lamp casts must match the color chosen for its panel ("rote lampe =
  roter Schein"). The dev.4 native-material-identity repair is **kept**; the emitted-light path is
  **added beside** the LED panel, not reverted.
- The lamp's own native light sources (`ToggleableLight.lightSources[i]._Light`) are now driven from the
  exact same resolved `VisualInstruction` as the panel, so both always show the same color. Only `color`
  and `intensity` are written: no transform, position, rotation or parenting, no range, cone, cookie,
  shadows, culling mask, render mode or `enabled` flag, never the lamp's switch state, and no Harmony
  hook.
- Emitted intensity is *relative* to that source's own freshly sampled native intensity (never the
  panel's HDR magnitude): it cannot compound and a native intensity of 0 stays 0. Emitted color is
  *absolute*.
- Coupled mode **fails closed**: registration requires both a verified panel slot and a readable own
  light source. An off lamp whose `_Light` has not resolved yet is retained as *pending*, retried at a
  bounded cadence and never switched on by the mod; an on lamp without a readable source is reported and
  left completely untouched instead of being driven panel-only.
- The proven exact-output-guarded `NativeLightPolicy`/`NativeColorPolicy` are reused **unchanged**
  through the new Unity-free `NativeLightBinding`; the new `NativeCouplingPolicy` is the readiness gate.
  The update phase hands both native properties back, late-update samples and applies, and
  teardown/demote/removal/rebind restore only while the value still equals our own last write (foreign
  drift preserved). A setter that throws releases only its own property, so one failure cannot block the
  other output or another lamp's cleanup. `NativeMaterialIdentity`, `NativeMaterialArrayScan`,
  `LedSurfaceOwnership` and the surface release proof are untouched.
- Plant materials are untouched; because a real light colors what it illuminates, a lit lamp now tints
  the plants it shines on. That is intended, and the obsolete "LED surface only / room lighting
  unchanged" wording was removed from the code, the app strings and the docs. `SURFACE BRIGHTNESS` is now
  `LAMP BRIGHTNESS`, and the status states "LED panel and emitted light use the same color".
- The apply breadcrumb now also reads back the emitted light and is rate limited to one line per second
  per lamp, so a per-frame changing (rainbow) instruction cannot spam the log.
- The obsolete surface-only contracts were replaced by the unified scope and nine new behavioral checks
  (C135-C143) were added: **RED first at 132/143** (11 behavioral failures, no missing-type compile
  error), **GREEN at 143/143**. Every native identity and ownership check still passes.
- Verified locally: **143/143** console checks; Release compile of the production mod **0 warnings /
  0 errors** with the installed-Shared override and a dedicated scratch output. **Not deployed**
  (installed DLL mtime unchanged), version unchanged at `0.1.0-dev.4` (the parent owns the bump and the
  deploy), no root or global documentation touched, no commit made.

## 0.1.0-dev.4 (2026-10-18)
- Fix log-proven applies-once regression: the dev.3 session classified its own
  `ledlight_light_on (LightControl)` clone as unsupported after the first edit. Native material
  identity now uses guarded nonzero pointers consistently across clone detection/reuse and restore.
- Repeated color edits keep the same native baseline and clone; incomplete material-array reads
  cannot prove a clone disconnected. Deferred Original restores remain pending for retry.
- Added bounded successful-apply breadcrumbs with requested color and material emission readback.
- Parent reran 134 console checks (all passed); independent targeted delta review PASS.
  Gameplay acceptance remains open. (The surface-only scope recorded in this entry is superseded by the
  unified LED panel + emitted-light entry above.)

## Native identity repair implementation notes (shipped in 0.1.0-dev.4)

### LED surface native-material identity repair

- **Owner-confirmed dev.3 partial failure.** In-game, the LED surface took only the **first** colour and
  then stopped tracking the selected colour, while plant and room lighting stayed vanilla. That matches
  the corrected surface-only scope, so no physical-light write was reintroduced.
- **Root cause.** IL2CPP returns a **fresh managed wrapper** around the same native material on every
  `sharedMaterials` read, so `ReferenceEquals(current, surface.Clone)` was false even when the slot
  really held our own clone. The clone was then classified as a foreign material, and
  `LedSurfacePolicy` correctly refuses to touch a foreign slot — so after the first install nothing was
  ever written again. The dev.3 log proves it:
  `Skipping an unsupported grow-lamp surface material 'ledlight_light_on (LightControl)'`.
- **Repair.** New runtime-used, Unity-free `NativeMaterialIdentity` decides sameness by the **nonzero
  native pointer** (managed reference equality is only a fast path; a disposed/unreadable wrapper fails
  closed). The single `LightRuntime.SameNativeMaterial` helper now backs every ownership, source-reuse,
  restore, array-reference and renderer-reference decision, and `LedSurfaceOwnership.NeedsClone` takes
  the native-id reader so the clone is reused instead of re-cloned. A foreign material with an identical
  name is never adopted, restored or destroyed.
- **Unreadable handles are never a disconnect proof.** New `NativeMaterialArrayScan` marks a scan
  incomplete as soon as one element cannot be read, so a material the renderer still points at is never
  destroyed by an optimistic "disconnected" verdict.
- **Deferred Original retry.** A restore that returns `Deferred` is no longer reported as converged
  (`LedSurfaceOwnership.RestoreConverged`), so a pending `Original` restore is retried instead of being
  dropped forever.
- **Bounded apply breadcrumb.** One `[apply] lamp <key> slot <n> requested <colour> -> surface emission
  (r, g, b)` line per real instruction change, with the emission read back from our own material; a
  repeated identical line is suppressed, so there is no per-frame spam and no model-only success claim.
- Verified locally: **134/134** console checks (the nine new checks C126–C134 were written RED first and
  the host reported **125/134** before the implementation); Release compile of the production mod
  **0 warnings / 0 errors**; `dotnet format --verify-no-changes` clean for both projects. **Not
  deployed**, version unchanged, no root or global documentation touched.

## 0.1.0-dev.3 (2026-10-18)
- Owner-selected correction: color and brightness now affect only the verified LEDLight emitting
  panel material. No Unity light color/intensity writes remain, so natural plant/room illumination
  stays under game control. FullSpectrum and halogen panels are intentionally unsupported.
- Native material source remains stable across repeated edits; owned clones are reused and never
  destroyed while a renderer might still reference them. Unproven releases are retained and retried.
- 125 console checks passed; independent correction and lifecycle delta review PASS after two
  material ownership blockers were repaired. Gameplay acceptance is open.
- See `verification.md` for installed artifact verification and known limitations.

## 0.1.0-dev.2 (2026-10-18)
- First installed host/singleplayer grow-lamp test build: phone color/intensity controls,
  slot-scoped atomic persistence, original restoration and bounded owned-lamp discovery.
- Independent final delta review PASS. Gameplay acceptance remains open; emissive surfaces,
  world lamps and multiplayer transport are outside this test slice.

### LED-surface implementation notes (shipped in 0.1.0-dev.3, not dev.2)

- **The mod no longer writes any `UnityEngine.Light` color or intensity.** The previous
  `OptimizedLight._Light` path is gone from the runtime: `LightRuntime` has no light reference, no
  `NativeLightPolicy`/`NativeColorPolicy` usage and no `Light.color`/`Light.intensity` write. Plant and
  room illumination is now exactly what the game produces, which is the whole point of the correction
  (the owner saw the emitted light spilling red onto the plants).
- **Color and brightness now tint only the lamp's own emitting LED panel.** New runtime-used, Unity-free
  `LedSurfacePolicy` (`src/Core/LedSurfacePolicy.cs`) owns the ownership and colour model; the adapter
  installs an owned clone of the native LED material into the exact `ToggleableLight.MaterialIndex` slot
  of `ToggleableLight.lightSurfacesMeshes` and sets its HDR `_EmissionColor`. Only the
  reference-returning `Renderer.sharedMaterials` accessor is used — never `material`/`materials`, which
  would instantiate a material per call — and the slot array is written only when the slot changes. A
  converged surface (same switch state and same resolved instruction as the previous frame) skips the
  read/write path entirely, so the steady-state per-frame cost for the whole scene is zero allocations.
- **Grounding.** Read offline from the shipped `Schedule I_Data/sharedassets0.assets` with UnityPy:
  `LEDLight` (`GrowLight` mb 91645, `ToggleableLight` mb 94512) has one surface MeshRenderer (56640,
  material `ledlight_light_off`) and `lightOnMat`/`lightOffMat` = Material 637 `ledlight_light_on` /
  Material 636 `ledlight_light_off`. Both panel materials carry an HDR `_EmissionColor`
  (off `2.505, 0.1749, 12.358`; on `2.7175, 0.1941, 12.358`) — the violet panel in the report.
  `_EmissionColor`, not `_BaseColor`/`_Color`, is therefore the property this build drives.
- **Verified-family gate.** A lamp is only registered when its native `ToggleableLight` carries exactly
  that verified ON/OFF material pair. FullSpectrum (`fullspectrum_light_on`, not present in
  `sharedassets0.assets`) and halogen (`halogen light on mat`) are left completely untouched and
  reported once, rather than guessing at an unverified material.
- **Lifecycle.** The slot is restored to the exact native material only while it still holds our own
  clone (a native or foreign change always wins), the clone is destroyed only after the slot no longer
  points at it, and teardown/pre-load/pre-scene-change/scene-unload/quit/deinit release every owned
  resource. `RestoreOwned` (keep the clone for a rebind) and `ReleaseOwned` (restore then destroy) are
  distinct paths.
- **UI honesty.** The phone app now reads `SURFACE BRIGHTNESS` for the brightness level, states
  "LED surface only; room lighting unchanged", and no longer claims a non-existent emissive-surface
  control. The eleven colours, seven brightness levels, `Original` and `Apply to all` are unchanged, and
  no new setting or light mode was added.
- **Historical, not active.** `NativeLightPolicy`, `NativeColorPolicy` and their checks remain as pure
  historical model code; the runtime no longer references them, and supplementary source guards assert
  that no active light write survives in `LightRuntime.cs`.
- Verified locally: **120/120** pure-model console checks (the twenty new checks were written RED first
  and the host reported **103/120** before the implementation); Release compile of the production mod
  **0 warnings / 0 errors**; the decompiled production assembly contains **zero** `UnityEngine.Light`,
  `OptimizedLight`, `set_color` or `set_intensity` references; `dotnet format --verify-no-changes` clean
  for both projects. **Not deployed** and not verified in-game.

### LED-surface lifecycle repair notes (shipped in 0.1.0-dev.3)

- **Clone source no longer drifts onto the clone.** `LightRuntime.ApplySurface` chose the clone source
  as the slot's current material even when the slot already held our own clone, so an instruction
  change re-cloned from the previous clone: the clones chained (each copy inherited the previous
  scaled emission, compounding the colour), the old handle was overwritten without being destroyed,
  and the recorded native source was replaced by the clone, so the native material could no longer be
  restored. The source is now chosen through the new Unity-free `LedSurfaceOwnership.CloneSource`
  (`ownClone ? installedSource : currentSlot`), so it is always the native material and stays stable
  across changes.
- **A clone is destroyed only once its renderer is proven not to reference it.** `RestoreSurface`
  cleared the tracked slot (`InstalledSlot = -1`) before it had read or written anything, and
  `ReleaseOwned` then destroyed the clone unconditionally, so a failed restore (unreadable renderer,
  dead native source, failed setter) could destroy a material the renderer still pointed at. It now
  returns the `LedReleaseOutcome` proof from `LedSurfaceOwnership.DecideRelease`: the tracked slot is
  dropped only on `Disconnected` (the renderer references the clone nowhere) or `Restored` (the exact
  owned slot held the clone and was written back to a live native material). Every uncertainty defers.
- **Deferred releases are retained and retried, never destroyed while referenced.** An unproven
  release keeps its surface in a static `PendingReleases` list, retried at most once per second from
  `UpdatePhase` (even while no session is ready). The list is not cleared by teardown; a clone that can
  never be proven is intentionally left to the loader rather than destroyed and left dangling, and
  teardown logs the retained count. A duplicate retain is ignored, and a clone handle is never
  overwritten while the renderer may still reference it.
- **No foreign writes.** The release searches the renderer's whole material array for the clone, but
  writes only into the exact owned slot and only while it still holds our own clone; a slot that holds
  anything else means disconnected (destroy is safe) and is never overwritten.
- Verified locally: **125/125** pure-model console checks (the five new checks C121–C125 were written
  RED first and the host reported **120/125** before the fix); Release compile of the production mod
  **0 warnings / 0 errors**; the rebuilt assembly carries `LedSurfaceOwnership`/`LedReleaseOutcome` and
  still contains **zero** `UnityEngine.Light`, `OptimizedLight`, `set_color` or `set_intensity`
  references; `dotnet format --verify-no-changes` clean for both projects. **Not deployed**, version
  unchanged (`0.1.0-dev.2`), no root or global documentation touched.

- **Exact-owned-output restoration.** `NativeLightPolicy` and the new `NativeColorPolicy` now remember
  the exact float they wrote and restore the baseline only while the current native value still equals
  that write. An external/native change is preserved, ownership is released and nothing is written; the
  next apply adopts the external value as the new baseline. Intensity is compared as `float` so a
  float↔double round-trip is not misread as an external write. A throwing native setter calls
  `RejectWrite` and never claims ownership.
- **Strict slot gate + retryable I/O.** `LightPersistence.Save` evaluates
  `PersistenceSlotPolicy.CanWrite` unconditionally, so a `-1` active slot no longer bypasses the guard.
  `LightRuntime` advances its saved revision only after a successful write, so a failed I/O is retried
  at the next bounded autosave.
- **Symmetrical unload cleanup.** Added `OnDeinitializeMelon` (unsubscribe, then `LightRuntime.Teardown`)
  behind a one-shot `_deinitialized` gate shared with `OnApplicationQuit`.
- **Owned-property gate.** New `PropertyOwnershipPolicy` plus runtime enforcement: a lamp is only
  registered, commanded or driven while its live parent property is owned (live `IsOwned` or the native
  owned-property list). Ownership is rechecked before every command and at most once per second.
- **Cooperative bounded discovery.** Replaced the recursive `GetComponentsInChildren` walk with a
  budgeted traversal: owned-property roots plus a rotating fair-order snapshot of the scene roots, at
  most 64 nodes and 2 ms per frame up to a 200000-node cap, one `GetComponent<GrowLight>()` per visited
  node, loop-free via a pointer set, with real completion/elapsed/truncation logging. New
  `SceneScanBudget` (limits, truncation, fair root order) and `LightPointerDedupe` (shared light
  pointers are never scaled twice).
- **Unavailable/rebind instead of deletion.** A lamp with a temporarily missing renderer or parent
  property is marked *temporarily unloaded* (`MarkTemporarilyUnloaded`), leaves the active/UI list while
  keeping its state, and is rebound through `RebindTemporarilyUnloaded` when the same GUID reappears.
  Only a live `IsDestroyed` is permanent removal.
- **App honesty.** `LightControlApp.Act` reports a failed or storage-blocked action instead of
  refreshing as success; `OnDestroyed` removes the exact `UnityEvent`/handler pair captured at wiring
  time without a Unity fake-null button gate.
- Verified locally: **100/100** pure-model console checks (the new checks were written RED first and the
  host reported **85/100 passed; 15 failed** before the fixes); Release compile of the production mod
  **0 warnings / 0 errors**; `dotnet format --verify-no-changes` clean for both projects.
  See `verification.md` for the parent release and installation checks.

## 0.1.0-dev.1 (2026-10-18)

- Imported the `LightControl-0.1.0-dev.1-DEVELOPMENT` archive into the repository layout and pointed
  provenance at `C:/Users/pc/AppData/Local/hermes/attachments/LightControl-0.1.0-dev.1-DEVELOPMENT.zip`.
  The original archive is not duplicated in the tree.
- Relocated the pure model sources to `src/Core/` and merged everything into one project
  `src/LightControl.csproj` that uses the shared `Source/Mods/Directory.Build.props` (implicit usings
  enabled). Moved the console behavioural checks to `Source/Tests/LightControl.Tests` and switched
  them from a project reference to linked `Core` sources, so the test host carries no game dependency.
- Archived the previous read-only diagnostics entry point as `src/Diagnostics/DiagnosticMod.cs.txt`.
  It is no longer compiled: exactly one `MelonInfo`/`MelonMod` discovery point remains.
- **Replaced the diagnostic-only artifact with the production mod.** The assembly is now
  `LightControl` (root namespace `LightControl`) with a single entry point `src/Mod.cs`.
- **First in-game test slice implemented** (host/singleplayer only):
  - `src/LightRuntime.cs` — bounded discovery of placed, initialized grow lamps with a nonempty
    native GUID (`BuildableItem.GUID`), a two-phase visual pipeline (undo our own output in
    `OnUpdate`, rebase on a freshly sampled native value in `OnLateUpdate`), explicit removal only
    when a placed lamp reports `IsDestroyed`, and a teardown that restores every owned light.
  - `src/LightPersistence.cs` — slot-scoped atomic storage through the shared `SafeStorage`, with a
    read-failure gate that never clobbers existing data.
  - `src/LightControlApp.cs` — a vertical, dark-`GamePalette` phone app with fixed controls:
    previous/next lamp, color cycle over all eleven core modes, intensity cycle over all seven core
    levels, `Original` (color + 100%) and `Apply to all`. The empty state shows no fake values.
- Added five runtime-used, Unity-free policy files under `src/Core/` and linked them into the console
  test host, which now runs **79** behavioural checks (was 62):
  `NativeLightPolicy` (baseline ownership, no compounding, invalid-sample guard),
  `LampIdentityPolicy` (native GUID to persistent identity, canonical form),
  `PersistenceSlotPolicy` (save id / load decision / write guard),
  `ScanCadence` (bounded reconcile interval) and
  `UiSelectionPolicy` (color/intensity cycling and the original reset).
- `S1NoDeploy=true` remains a project default so an accidental solution build cannot deploy this
  build; `-p:S1NoDeploy=false` overrides it when separately authorized.
- Kept `assets/` unchanged.
- Verified locally: 79/79 pure-model console checks passed; Release compile of the production mod
  passed with 0 warnings/errors. Version-sync: 21 mods, 0 drift. See `verification.md`.
- **Not done / open:** no deploy, no gameplay run, no multiplayer check, no release approval. Lamp
  *emissive surface* control (owned per-renderer material clone) is deliberately not implemented in
  this slice, only the proven Unity light color/intensity path. Groups/rooms, disco effects,
  configuration and language binding, client transport and acceptance tests T01–T18 remain open.
