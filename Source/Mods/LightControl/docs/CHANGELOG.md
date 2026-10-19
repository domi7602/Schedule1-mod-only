# LightControl

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
