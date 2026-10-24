# First test-slice verification

> Historical record (dev.2–dev.16 iteration). Current status: **0.1.0, owner-confirmed
> 2026-10-24** — see [graphite-ui-verification.md](graphite-ui-verification.md). The
> "open"/"candidate" statements below describe their original snapshot dates.

## Current candidate: 0.1.0-dev.16 — compile-only audit/UI repair

See [audit-repair-verification.md](audit-repair-verification.md) for the final 233/233 checks, clean
Release build, independent delta reviews, exact retained artifact/source hashes, all twelve finding
dispositions and remaining game acceptance. Installed dev.15 UI acceptance is historical. No mod
binaries were deployed; an unintended Shared receipt write is explicitly documented there.

The older slices below preserve their original test/design claims, including claims disproved by
the later audit. They do not establish current native safety, material destruction or PhoneApp fit.

## Historical source: 0.1.0-dev.4 unified LED panel + emitted-light scope (not deployed, not in-game verified)

Owner requirement: *"der Schein sollte sich zum farblicht anpassen also zb rote lampe = roter Schein"* —
the light a lamp casts must match the color chosen for its panel. The dev.4 native-material-identity
repair is **kept**; the emitted-light path is **added beside** the panel, not reverted.

- **Two coupled outputs, one resolved instruction.** `LightRuntime` resolves a single
  `VisualInstruction` per lamp in the late-update phase and feeds that exact instruction to the panel
  slot and to the lamp's own native light sources. Emitted intensity is *relative* (the chosen factor
  scales that source's own freshly sampled native intensity — never the panel's HDR magnitude, so it
  cannot compound and a native `0` stays `0`); emitted color is *absolute* (the chosen hue replaces the
  native color, because a color factor would be meaningless).
- **Proven policies reused, not re-invented.** The new Unity-free `Core/NativeLightBinding` sequences the
  existing exact-output-guarded `NativeLightPolicy` (intensity) and `NativeColorPolicy` (color) for one
  source; the new `Core/NativeCouplingPolicy` is the readiness gate. Neither replaces the identity fix:
  `NativeMaterialIdentity`, `NativeMaterialArrayScan`, `LedSurfaceOwnership`, `NativeLightPolicy` and
  `NativeColorPolicy` are unchanged, and every ownership test still passes.
- **Fail-closed coupling.** A lamp is registered only when a verified emitting panel slot **and** at
  least one readable own native light source both resolve. The shipped `LEDLight` prefab's `_Light` is
  null in the asset and resolves at runtime, so an off lamp may expose no source on a frame: that is
  `LampCouplingState.Pending`, retried at most once per 2 s (never per frame, capped at 256 entries) and
  **never** resolved by switching the lamp on. A lamp that is on without a readable source is
  `LampCouplingState.Unsupported`: reported once, counted per distinct lamp for the phone app, and left
  completely untouched — no panel-only fallback.
- **Native references.** Sources are collected only from the verified own
  `ToggleableLight.lightSources[i]._Light` (`Il2CppScheduleOne.DevUtilities.OptimizedLight`) and
  de-duplicated by native pointer through the existing `LightPointerDedupe`, across the lamp and every
  other active lamp. Collection happens in the bounded scan/register/rebind path only; there is no
  per-frame discovery. Zero, unreadable and dead pointers are skipped. No child is searched by name and
  nothing is relocated.
- **Two-phase writes and exact restore.** `OnUpdate` hands both native properties back to the game's own
  writer for every lamp that still owns an output (even if the lamp was just switched off or ownership
  was just lost); `OnLateUpdate` samples the native value and applies only while the lamp is on and the
  owner is verified. A switched-off lamp restores color and intensity but is never forced on. Demote,
  removal, rebind, pre-load, pre-scene-change, scene-unload, deinit and quit restore the native values
  **only** while the current value still equals our own exact last write, so foreign drift is preserved.
  Each property is released on its own when its setter throws (`RejectWrite`), so a failure on one
  property cannot block the other, the rest of the lamp, or another lamp's cleanup. The surface release
  proof and `PendingReleases` retention are untouched.
- **Nothing else about a light is written.** No transform/position/rotation/parenting, range, cone,
  cookie, shadows, culling mask, render mode or `enabled` flag, and never the lamp's switch state. No
  plant or world material is accessed, cloned or mutated; no shader object or shader keyword is touched.
  The only material write is still our own panel clone.
- **Plant illumination is now colored, on purpose.** A real light colors what it illuminates, so the
  plants a lit lamp shines on receive the chosen tint. That is the requested behavior and it is **not** a
  plant material change: plant materials are never modified. The previous "room lighting unchanged /
  LED surface only" wording is obsolete and has been removed from the code, the app strings and the docs.
- **UI wording.** `SURFACE BRIGHTNESS` → `LAMP BRIGHTNESS`; the status line and the app's limitation text
  now state "LED panel and emitted light use the same color", and the app additionally shows how many
  lamps are on without a readable light source instead of presenting them as working control.
- **Breadcrumb.** `[apply] lamp <key> slot <n> requested <colour> -> panel emission (r, g, b) emitted
  (r, g, b) x<i>` is logged only for a successful coupled write whose instruction actually changed. An
  identical line is suppressed **and** the line is rate-limited to at most one per second per lamp, so a
  per-frame changing instruction (a rainbow effect) cannot spam the log. Model state is never reported as
  a visual success.
- **RED first.** The obsolete surface-only contracts were replaced by the unified scope, and nine new
  behavioral checks (C135–C143) were added. Against the stub seams the host reported **132/143 passed;
  11 failed** — every failure behavioral (coupled contracts, readiness, hue equality, restore-both,
  switched-off, intensity levels, external-write preservation, failure isolation), not a missing-type
  compile error. **After the implementation: 143/143 passed, 0 failed.** The check count was read from
  the program's own `RESULT:` line.
- **Release compile (no deploy),**
  `dotnet build Source/Mods/LightControl/src/LightControl.csproj -c Release --no-incremental -p:S1NoDeploy=true -p:BuildProjectReferences=false -p:CustomAfterMicrosoftCommonTargets=<scratch installed-shared override> -p:OutputPath=<scratch lightcontrol-combined-verify>`:
  **Build succeeded, 0 warnings, 0 errors**. The rebuilt scratch `LightControl.dll` contains
  `UndoLights`, `NativeCouplingPolicy`, `NativeLightBinding`, `LampCouplingState`, `RestoreLightSource`,
  `CollectLightSources` and `UnsupportedLampCount`. The installed `Mods/LightControl.dll` mtime is
  unchanged (13:58:33, before the build), so nothing was deployed.
- **Not verified.** No game process was started, no save was touched and this build is **not deployed**.
  The artifact behavior — that a lamp's own `_Light` really resolves and that the emitted color visibly
  matches the panel in-game — is **unverified**; this physical-light write path has never run in-game in
  this form. The colored plant illumination is **expected, not observed**. Version stays `0.1.0-dev.4`
  (the parent owns the bump and the deploy), no root or global documentation was touched and no commit
  was made.

## Installed build: 0.1.0-dev.4 — native material identity repair (superseded in source by the section above)

- Parent reran **134/134** console checks; independent targeted delta review **PASS**.
- Repository version script applied before final nonincremental Release build: **0 warnings / 0 errors**.
- Game confirmed closed. Local Shared output changed during an independent source-folder migration,
  so the ordinary byte-match preflight correctly blocked the first deployment attempt.
- No Shared file was replaced. A scratch `CustomAfterMicrosoftCommonTargets` override removed the
  LightControl project reference and explicitly referenced the exact installed Shared.dll with
  `Private=false`. Evaluated items confirmed no ProjectReference and the installed Shared HintPath.
- Final build used this dependency override, `S1NoDeploy=false`, `BuildProjectReferences=false` and
  a dedicated scratch output directory `lightcontrol-release-dev4` to avoid concurrent bin output changes.
  Installed Shared SHA-256 stayed `7df38520bd509357d0998d71405029870e8b47516f6bebec7de33cfc7de6b912`.
- Installed LightControl DLL, PDB and mod.json were read back and matched the pinned release artifacts.
  DLL SHA-256 `d623313d839a7112674c39d832b105fbc0bd9069662704ec81837e8592b3dc71`;
  installed metadata version `0.1.0-dev.4`.
- Decompiled installed runtime contains `SameNativeMaterial`, `NativeMaterialId` and `[apply] lamp`;
  no `UnityEngine.Light`, `OptimizedLight` or `set_intensity` references.
- Version-sync **21 mods, 0 drift**, doc-path check passed. Native rendering acceptance remains open.
- Owner test: several different color edits on the same lamp, Original, then color again. The LED
  panel should follow each selection; successful changes log `[apply] lamp ... -> surface emission`.
  The native pink room/plant illumination is intentionally unchanged by this surface-only build.

## Native material identity repair (pre-deploy implementation evidence; shipped in dev.4)

Owner-confirmed in-game result for the installed `0.1.0-dev.3`: *the LED surface took only the first
colour and then stopped following the selected colour; the lamp was tinted, while the plant/wall
lighting stayed vanilla.* The vanilla plant/wall lighting is the intended surface-only scope — no
physical light was reintroduced for this repair.

- **Log-proven defect.** `Latest.log` repeats
  `[LightControl] Skipping an unsupported grow-lamp surface material 'ledlight_light_on (LightControl)'; only the verified LED panel is recoloured.`
  The quoted material is **our own clone** (`CreateClone` names it `<native name> (LightControl)`), yet
  the runtime classified it as foreign. Cause: IL2CPP returns a fresh managed wrapper for the same
  native material on every `sharedMaterials` read, so `ReferenceEquals(current, surface.Clone)` was
  false and `LedSurfacePolicy.Classify` reported `Foreign`. The policy then correctly refuses to write a
  foreign slot, so after the first successful install no later colour was ever applied.
- **Repair, one identity rule.** New runtime-used Unity-free `src/Core/NativeMaterialIdentity.cs`:
  sameness is the **nonzero native pointer**, managed reference equality is only a fast path, and a
  disposed/unreadable wrapper (`Pointer` throws) fails closed. `LightRuntime` gained the single
  `SameNativeMaterial`/`NativeMaterialId` helper pair, now used for the own-clone decision, the
  `NeedsClone` source comparison, the exact-slot array compare, the restore array scan and the
  renderer-reference proof. `LedSurfaceOwnership.NeedsClone` accepts the native-id reader, so the same
  native material read through two wrappers reuses the existing clone instead of re-cloning it.
- **A foreign material with an identical name is never adopted.** Identity is the native pointer, never
  a name suffix; a same-named foreigner is neither owned, restored nor destroyed.
- **Unreadable handles can never prove disconnection.** `NativeMaterialArrayScan` marks the scan
  incomplete as soon as one element is unreadable, so `DecideRelease` returns `Deferred` and the clone is
  retained rather than destroyed while the renderer may still point at it. Both the restore scan and
  `RendererNoLongerReferences` use it.
- **Deferred Original retry.** `LedSurfaceOwnership.RestoreConverged` replaces the unconditional
  `Converged = true` after a restore: a `Deferred` restore is retried instead of being silently dropped,
  which previously could leave `Original` never applied.
- **Bounded apply breadcrumb.** `[apply] lamp <stable key> slot <n> requested <model colour> -> surface
  emission (r, g, b)` is logged only for a successful install whose instruction actually changed, with
  the emission read back from our own material; a repeated identical line is suppressed. No per-frame
  spam, no new scan, and the phone UI still claims nothing beyond the model state.
- **RED first.** Nine new checks (C126–C134) were written against a deliberately broken stub of the new
  identity policy and the host reported **125/134 passed; 9 failed** — `C130` failed with
  `Expected Install; actual None`, i.e. exactly the reported "second colour is never applied" symptom.
  **After the implementation: 134/134 passed.**
- **Release compile (no deploy),**
  `dotnet build Source/Mods/LightControl/src/LightControl.csproj -c Release --no-incremental -p:S1NoDeploy=true -p:BuildProjectReferences=false`:
  **Build succeeded, 0 warnings, 0 errors**, no deploy line, no game process started or stopped, no save
  touched. `S1NoDeploy=true` remained the effective value.
- **Artifact check.** `ilspycmd -l c` lists `LightControl.Core.NativeMaterialIdentity` and
  `LightControl.Core.NativeMaterialArrayScan`; the decompiled assembly contains **no**
  `UnityEngine.Light`, `set_color`, `set_intensity` or `OptimizedLight` reference (the only
  `NativeLightPolicy`/`NativeColorPolicy` matches are their historical pure class declarations, which
  the runtime does not reference), and the new `SameNativeMaterial`/`NativeMaterialId`/`RestoreConverged`/
  `LastApplyLog` paths and the `[apply] lamp …` literal are present.
- **Formatting,** `dotnet format <project>.csproj --verify-no-changes --no-restore` from each project's
  own directory: **exit 0** for both `src/LightControl.csproj` and
  `Source/Tests/LightControl.Tests/LightControl.Tests.csproj`.
- **Not verified.** This repair is **not deployed** and has not been exercised in-game. Version stays
  `0.1.0-dev.3` (the parent owns the bump and the deploy), no root or global documentation was touched,
  and no commit was made. The repeated
  `Grow lamp is no longer controllable (its parent property is no longer owned)` lines in the same log
  window were **not** treated as a second defect: they coincide with placement/registration events and
  no evidence separates a genuine ownership loss from a placement event, so nothing in the ownership
  gate, the scan or the authority model was changed.

## Prior installed build: 0.1.0-dev.3 — LED surface only (historical; applies-once regression)

- Parent reran all **125/125** console checks. These are pure-model and supplementary source guards,
  not native rendering tests. Independent lifecycle delta review **PASS** after both blockers were fixed.
- Game closed and built/installed Shared byte-identical before the single-mod deploy.
- Version bump used the repository script; final nonincremental Release build explicitly used
  `-p:S1NoDeploy=false -p:BuildProjectReferences=false`: **0 warnings / 0 errors**, deploy line observed.
- Exact installed DLL, PDB and mod.json were read back and all matched their build/source bytes.
  Installed metadata version: `0.1.0-dev.3`. DLL SHA-256:
  `2cc75d5d55339e7e6ae090f389e76a284b0c66a715c56c76d1ba3326c14239da`.
- Decompiled the installed `LightControl.LightRuntime`: `LedSurfaceOwnership` and `_EmissionColor`
  present; no `UnityEngine.Light`, `OptimizedLight`, `set_color` or `set_intensity` references.
- Version-sync **21 mods, 0 drift** and doc-path check passed. Only `LightControl.dll` is loadable.
- Gameplay remains **unverified by this agent**. Test panel color, surface brightness, repeated edits,
  Original, native on/off and save/reload. Plants/room should retain the game's normal illumination.
- **Owner-confirmed dev.3 result (partial failure):** the surface took only the first colour and then
  stopped following the selected colour; plant and wall lighting stayed vanilla as intended. See the
  repair section above for the log-proven cause and the fix, which is **not** in this installed build.
- Known nonblocking review limitation: a deferred Original restore is safe (no referenced clone is
  destroyed), but may require another instruction/switch change before the active surface retries.
  A retained release with an unreadable/dead renderer is intentionally kept rather than unsafely destroyed.

## Prior installed build: 0.1.0-dev.2 (historical; owner rejected physical-light tint)

- Parent reran the console host: **100/100 passed**, pure-model and source-wiring checks only.
- Independent delta review: **PASS**, no fix-now findings for the declared grow-lamp test slice.
- Version synchronized using `Tools/bump-version.ps1` before the final nonincremental Release build.
- Preflight confirmed the game closed and built/installed Shared SHA-256 equal
  (`7df38520bd509357d0998d71405029870e8b47516f6bebec7de33cfc7de6b912`).
- Actual authorized build used `--no-incremental -p:S1NoDeploy=false -p:BuildProjectReferences=false`:
  **0 warnings / 0 errors**, deployment line observed. No dependency deployment.
- Read back exact installed DLL, PDB and mod.json: all byte-identical to their build/source targets.
  Installed metadata reports `0.1.0-dev.2`; only `LightControl.dll` is a loadable LightControl artifact.
- DLL SHA-256: `7b0a884980899e6b444e681d5c3c0302be782a45d1557dedd0aa187ae32f03cf`.
- Installed assembly metadata lists `LightControl.Mod`, `LightControl.LightControlApp`, runtime and
  ownership/scan policies. This proves artifact contents, not successful game activation.
- Parent version-sync: **21 mods, 0 drift**; doc-path check passed.
- **In-Game-Verify open**: no actual gameplay, save/reload, performance or multiplayer observation.

## LED-surface-only correction (implementation evidence before dev.3 deployment)

Owner-selected scope: *"Nur die LED-Leuchtfläche der Lampe färben; Pflanzen und Umgebung behalten die
normale Beleuchtung."* The runtime no longer writes any `UnityEngine.Light` color or intensity; color
and brightness now tint only the lamp's own emitting LED panel. Nothing below is a gameplay
observation — every line is a compile-time, offline-asset or pure-model result.

- **Offline grounding (UnityPy 1.25.4, read-only).** `Schedule I_Data/sharedassets0.assets`:
  `LEDLight` (`GrowLight` mb 91645, `ToggleableLight` mb 94512) exposes `lightSurfacesMeshes` =
  MeshRenderer 56640 on GameObject 21493 `Light`, whose material array is `ledlight_light_off`.
  `ToggleableLight.MaterialIndex` = 0, `lightOnMat` = Material 637 `ledlight_light_on`,
  `lightOffMat` = Material 636 `ledlight_light_off`. `ledlight_light_off` (shader 3780) has
  `_BaseColor` `(0.7925, 0.7925, 0.7925)` and **`_EmissionColor` `(2.505, 0.1749, 12.358)`**;
  `ledlight_light_on` (shader 3779) has `_EmissionColor` `(2.7175, 0.1941, 12.358)`. Both are HDR
  emission colours, which is why the panel glows violet in the report. **`_EmissionColor` — not
  `_BaseColor`/`_Color` — is the property this build drives.** The other `ledlight_*` materials
  (`body`, `bar`, `trim`, `button`) are the lamp housing and are never touched.
- **Unverified families are skipped, not guessed at.** `fullspectrum_light_on` is **not present** in
  `sharedassets0.assets` (only `ledlight_*` and `halogen *` are), and the halogen panel is
  `halogen light on mat`. Registration therefore requires the lamp's native `ToggleableLight` to carry
  exactly the verified `ledlight_light_on` / `ledlight_light_off` pair; a FullSpectrum or halogen grow
  lamp is discovered but left completely untouched and reported once.
- **RED first.** The twenty new checks (C101–C120) were written against the shipped sources and the
  host reported **103/120 passed; 17 failed** before the implementation (C101–C107, C109–C116 on the
  stub policy, C117–C120 on the unchanged runtime/app). **After the implementation: 120/120 passed.**
- **Release compile (no deploy),**
  `dotnet build Source/Mods/LightControl/src/LightControl.csproj -c Release --no-incremental -p:S1NoDeploy=true -p:BuildProjectReferences=false`:
  **Build succeeded, 0 warnings, 0 errors** → `src/bin/Release/net6.0/LightControl.dll`. No deploy
  line, no game process started or stopped, no save touched.
- **Artifact check.** The production assembly was decompiled (`ilspycmd`) and contains **zero**
  references to `UnityEngine.Light`, `OptimizedLight`, `set_color` or `set_intensity`. The only
  `Material`/emission writes present are `new Material(source)`, `SetColor("_EmissionColor", …)`,
  `renderer.sharedMaterials`, and `Object.Destroy(clone)`.
- **Formatting,** `dotnet format <project>.csproj --verify-no-changes --no-restore`: **exit 0** for both
  `src/LightControl.csproj` and `Source/Tests/LightControl.Tests/LightControl.Tests.csproj`.
- **What the surface path does.** `LedSurfacePolicy` (Unity-free, in `src/Core/`) owns every gate: an
  off lamp is never recoloured, a foreign slot is never replaced, an empty slot is never created, the
  value always scales a freshly sampled native baseline (never our own output), and model `Original` at
  100% removes all owned influence. The adapter caches renderer + slot + baseline material per lamp,
  reads the slot only through `Renderer.sharedMaterials` (never `material`/`materials`), writes the slot
  array only when the slot actually changes, restores the native material only while the slot still
  holds our own clone, and destroys the clone only after the slot no longer points at it. A converged
  surface (unchanged switch state and unchanged resolved instruction) skips the read/write path
  completely, so the steady-state per-frame cost for the whole scene is zero allocations.
  `RestoreOwned` (rebind: keep the clone) and `ReleaseOwned` (teardown: restore then destroy) are
  separate paths, and teardown/pre-load/pre-scene-change/scene-unload/quit/deinit all release every
  owned resource.
- **Unchanged.** Eleven colours, seven brightness levels, `Original`, `Apply to all`, the storage
  schema, the bounded 64-node/2 ms/200000-node scan, the GUID identity gate, the owned-property gate,
  the temporarily-unloaded/rebind semantics, `LightPointerDedupe` and the exact `UnityEvent` listener
  ownership. No new setting, toggle or native-light mode was added; `NativeLightPolicy` and
  `NativeColorPolicy` remain as pure historical model code that the runtime no longer references.
- **Not deployed, not verified in-game.** This round changed only LightControl sources, tests and its
  own `docs/`. Version stays `0.1.0-dev.2` (the parent owns the bump), and no root or global
  documentation was touched.

## LED-surface clone lifecycle repair (implementation evidence before dev.3 deployment)

Two confirmed pre-deploy blockers in the LED surface adapter, repaired with strict RED → GREEN. Every
line below is a compile-time, offline-artifact or pure-model result — no gameplay observation.

- **B1 — the clone source could drift onto the clone itself.** `LightRuntime.ApplySurface` set
  `Material? source = current;` unconditionally, so when the slot already held our own clone the
  replacement clone was created *from the clone*: `InstallSurface` then set `InstalledSource` to the
  clone, overwrote the old clone handle without destroying it, chained the colour (each copy inherited
  the previous scaled emission, compounding) and left `InstalledSource` no longer the native material,
  so the native restore failed. The source is now chosen by the new Unity-free
  `LedSurfaceOwnership.CloneSource(ownClone, currentSlot, installedSource)`
  (`ownClone ? installedSource : currentSlot`), so it is always the native material and is stable
  across changes; the same native source reuses the existing clone (`LedSurfaceOwnership.NeedsClone`).
- **B2 — a clone could be destroyed while the renderer still referenced it.** `RestoreSurface` cleared
  `surface.InstalledSlot = -1` before it read or wrote anything, and `ReleaseOwned` then destroyed the
  clone unconditionally, so an unreadable renderer, a dead native source or a failed setter left the
  renderer pointing at a destroyed material. `RestoreSurface` now returns the `LedReleaseOutcome` proof
  from `LedSurfaceOwnership.DecideRelease`: the tracked slot is dropped only on `Disconnected` (the
  renderer is readable and references the clone nowhere in its material array) or `Restored` (the exact
  owned slot held our clone and was written back to a live native material). Every uncertainty —
  unreadable renderer, dead renderer wrapper, dead source, failed setter, or a clone outside the exact
  owned slot — defers.
- **Retention instead of a dangling material.** An unproven release keeps its surface in a static
  `PendingReleases` list, retried at most once per second (`PendingReleaseRetrySeconds = 1`) from
  `UpdatePhase`, which runs the retry before the `Ready` gate so it still works after a teardown. The
  list is never cleared by `Teardown` (a deferred release must survive it) and teardown logs the
  retained count. A duplicate retain is ignored; a dead clone is released immediately; a clone that can
  never be proven is intentionally left to the loader rather than destroyed and left dangling. No busy
  retry, no blocking, no unbounded loop.
- **No foreign writes, no overwritten handles.** The release searches the renderer's whole material
  array for the clone but writes only into the exact owned slot and only while it still holds our own
  clone; a slot holding anything else means disconnected (destroy is safe) and is never overwritten. A
  replacement clone is only created once the previous one is proven unreferenced
  (`RendererNoLongerReferences`), so a live handle is never overwritten.
- **RED first.** Five new checks (C121–C125) were written against a deliberately buggy stub of the new
  helper and the host reported **120/125 passed; 5 failed** (`C121`/`C122` the clone source returned
  the slot's current material, `C123` an unproven release reported `Disconnected` instead of
  `Deferred`, `C124` a successful restore reported `Disconnected` instead of `Restored`, `C125` the
  runtime did not yet use the helper). **After the fix: 125/125 passed.**
- **Release compile (no deploy),**
  `dotnet build Source/Mods/LightControl/src/LightControl.csproj -c Release --no-incremental -p:S1NoDeploy=true -p:BuildProjectReferences=false`:
  **Build succeeded, 0 warnings, 0 errors** → `src/bin/Release/net6.0/LightControl.dll` (118784 bytes).
  No deploy line, no game process started or stopped, no save touched.
- **Artifact check.** `ilspycmd -l c` on the rebuilt assembly lists `LightControl.Core.LedSurfaceOwnership`;
  a full decompile contains **zero** `UnityEngine.Light`, `OptimizedLight`, `set_color` or
  `set_intensity` references (the only `NativeLightPolicy`/`NativeColorPolicy` matches are their pure
  historical class declarations, which the runtime does not reference).
- **Formatting,** `dotnet format <project>.csproj --verify-no-changes --no-restore`: **exit 0** for both
  `src/LightControl.csproj` and `Source/Tests/LightControl.Tests/LightControl.Tests.csproj`.
- **New runtime-used seam.** `src/Core/LedSurfaceOwnership.cs` (Unity-free, linked into the test host)
  owns the source choice and the release proof; `LightRuntime` uses `CloneSource`, `NeedsClone` and
  `DecideRelease`, so the repair is exercised by executable behaviour checks (C121–C124), not only by
  source text. C125 is a supplementary wiring contract that the runtime actually calls the helper, that
  `Material? source = current;` is gone, and that the tracked slot is only dropped through the proof;
  C117 still asserts no physical Unity light write survives.

## Prior 0.1.0-dev.1 repair evidence (historical)

LightControl remains at `0.1.0-dev.1`. This run repairs the pre-deploy review findings on the existing
test slice: exact-owned-output restoration that preserves native/external writes, a strict active-slot
write gate with retryable I/O, symmetrical unload cleanup, a live owned-property gate, a genuinely
cooperative bounded discovery traversal, and safe *temporarily unloaded* / rebind semantics instead of
deletion when a native reference goes missing. It is **not** a gameplay release and it has **not** been
run in-game by this agent. No version bump, no deploy and no global documentation were touched; those
belong to the parent review step.

## Executed checks

- Console test host, `dotnet run -c Release --project Source/Tests/LightControl.Tests/LightControl.Tests.csproj`:
  **100/100 passed, 0 failed**. This is a console host, not an xUnit project; `dotnet test` is not a
  substitute for running it.
- **RED first.** The new checks (C80–C100) were written against the shipped code before any fix and
  the host reported **85/100 passed; 15 failed**:
  `C80` native undo preserves an external write after late-update (expected `ShouldWrite = False`,
  got `ShouldWrite = True, Intensity = 10`), `C83` teardown preserves an external write,
  `C86` color undo preserves an external write (expected no write, got `R = 0.2, G = 0.3, B = 0.4`),
  `C88` owned-property gate (expected `False`, got `True`), `C89` scan budget bounds nodes and
  milliseconds per frame, `C91` fair root order (expected `[1,2,0]`, got `[0,1,2]`),
  `C92` light-pointer dedupe (expected `[0,1]`, got `[0,1,2]`), `C93` strict slot guard,
  `C94` persist retries a failed write, `C95` symmetrical deinitialization, `C96` ownership
  gate/unload/rebind wiring, `C97` cooperative bounded scan wiring (the shipped source still contained
  a recursive `GetComponentsInChildren`), `C98` exact `UnityEvent` removal without a button gate,
  `C99`/`C100` app action failure reporting. **After the fixes: 100/100 passed.**
- Release compile of the production mod,
  `dotnet build Source/Mods/LightControl/src/LightControl.csproj -c Release -t:Rebuild -p:S1NoDeploy=true -p:BuildProjectReferences=false`:
  **Build succeeded, 0 warnings, 0 errors**. Output `src/bin/Release/net6.0/LightControl.dll`,
  110080 bytes, sha256 `ec73d6e4a8d2a58b018c4a47a7af5be60e22bead8003f1a881c27519306266f6`. The build
  exercises the real game assemblies, so every game API used by the slice (`BuildableItem.GUID`,
  `IsDestroyed`, `Initialized`, `ParentProperty`, `Property.propertyCode`, `Property.IsOwned`,
  `Property.OwnedProperties`, `GrowLight.Light`, `ToggleableLight.lightSources`, `OptimizedLight._Light`,
  `Transform.GetChild`/`childCount`/`GetComponent<T>`, `Scene.GetRootGameObjects`, `BuildEvents`,
  `GameLifecycle.OnSaveStart`, `SaveSlots.GetActiveSlotNumber`, `SafeStorage`, `UITheme`/`GamePalette`/
  `UIFactory`, `PhoneApp`) resolves against the shipped interop.
- Artifact-to-source check: the rebuilt DLL contains the new members and literals
  (`NextOwnershipCheckAt`, `OwnershipRecheckIntervalSeconds`, `RebindTemporarilyUnloaded`,
  `MarkTemporarilyUnloaded`, `RemovePermanently`, and the UTF-16 literals `Scan complete`,
  `Bounded scan started`, `Rebound a previously unloaded grow lamp`, `changes are not saved`), so the
  compiled artifact matches the repaired sources.
- Formatting, `dotnet format <project>.csproj --verify-no-changes --no-restore` from each project's own
  directory: **exit 0, no changes reported** for both `src/LightControl.csproj` and
  `Source/Tests/LightControl.Tests/LightControl.Tests.csproj`.
- Exactly one `MelonInfo`/`MelonMod` remains; the previous diagnostics entry point is archived as
  `src/Diagnostics/DiagnosticMod.cs.txt` and is not compiled.
- No LightControl-prefixed file exists in the installed game's `Mods` folder. **No deployment was
  performed.**
- Not re-run in this round: the repository version-sync script. No version, `mod.json` or root
  documentation was changed by this repair, so the previously recorded `21 mods, 0 drift` is untouched
  but is not re-asserted here.

## What the slice actually does

- **Discovery is cooperative and bounded.** One cycle queues a snapshot of the owned-property roots plus
  the active scene roots in a rotating fair order (`SceneScanBudget.FairRootOrder`), then `UpdatePhase`
  steps the traversal with at most `SceneScanBudget.MaxNodesPerFrame` = 64 nodes and
  `MaxMillisecondsPerFrame` = 2 ms of measured work per frame, stopping at `TotalNodeCap` = 200000
  nodes. Each visited node is inspected with a single `GetComponent<GrowLight>()` and its children are
  enqueued by index; **no recursive `GetComponentsInChildren` call exists anywhere in the runtime**
  (asserted by test `C97`). A node-identity `HashSet` makes the walk loop-free. Completion is logged
  with the real registered count, visited-node count, total elapsed milliseconds and a truncation flag;
  `LoadComplete` never claims a completed scan. New placements still arrive through S1API `BuildEvents`,
  and a fresh walk is started at most once every 20 s (`ScanCadence`).
- **Identity.** Only a live, initialized placed lamp with a **nonempty native GUID** is registered; an
  item definition id or a runtime instance id is never used as an identity, and all textual forms of one
  GUID collapse to one canonical identity.
- **Owned-property gate.** A lamp is registered, commanded and driven only while its live parent
  property is owned: the live native `IsOwned` flag, or the property code appearing in the native
  owned-property list refreshed at the reconcile cadence (`PropertyOwnershipPolicy.IsOwnedProperty`
  requires a nonblank code and one of those two signals). The gate is re-evaluated before every command
  (`Execute` → `RecheckOwnership`) and at most once per second per lamp in `LateUpdatePhase`; a lamp
  that loses its property is demoted immediately and its visual influence is removed. No unowned or
  arbitrary NPC grow lamp is ever driven.
- **A missing reference is not a deletion.** `IsGenuinelyDestroyed` is the only permanent-removal path
  and requires a live `IsDestroyed` on the placed object (`RemovePermanently`). A lamp whose native
  reference, light source or parent property is temporarily unavailable is demoted to *temporarily
  unloaded* (`MarkTemporarilyUnloaded`, which preserves state), removed from the active/UI list, and
  rebound through the core `RebindTemporarilyUnloaded` API when the same GUID reappears — a new runtime
  instance id and a fresh `OwnedLight` set, with the old policies released first.
- **Exact-output restoration.** `NativeLightPolicy`/`NativeColorPolicy` remember the exact float they
  wrote. `UndoOwned` and `Restore` write the baseline **only while the current native value still equals
  our own last write** (`MatchesOwnedOutput`, compared as floats so a float↔double round-trip is not
  mistaken for an external write). If anything else changed the value, it is preserved, ownership is
  released and nothing is written; the next `Apply` adopts the external value as the new baseline.
  `RejectWrite` releases ownership when the native setter throws, so a failed write never claims
  ownership, and an invalid/negative/non-finite sample writes nothing.
- **Visuals never compound.** `OnUpdate` hands each owned light back to the game's own writer by
  restoring the last sampled baseline; `OnLateUpdate` samples a fresh native value and applies the
  resolved instruction. A static factor of two therefore stays two, native flicker and zero are
  preserved, external drift is picked up, and `Original` restores the latest native baseline. Shared
  Unity light pointers are de-duplicated (`LightPointerDedupe.SelectUnowned`) against both the lamp's
  own source list and every other active lamp, so no light is scaled twice.
- **Storage is strictly gated and retryable.** Slot-scoped path, atomic write via
  `SafeStorage.SaveTextAtomic`, and a write is attempted only while the captured slot still equals the
  game's active slot (`PersistenceSlotPolicy.CanWrite`, evaluated unconditionally — a `-1` active slot
  never authorizes a write). A read that cannot produce text blocks writes instead of clobbering the
  file. `_savedRevision` advances only after a successful write, so a failed I/O is retried at the next
  bounded autosave (2 s) rather than being silently skipped.
- **Unload cleanup is symmetrical.** `OnDeinitializeMelon` releases every subscription and then runs
  `LightRuntime.Teardown` behind a one-shot `_deinitialized` gate that `OnApplicationQuit` shares, so a
  repeated shutdown cannot write again and a re-initialization re-arms the hooks. The phone app keeps
  the exact `UnityEvent`/handler pair captured at wiring time and removes it on destroy **without** a
  Unity fake-null button gate, and it reports a failed action instead of refreshing as success.
- **Untouched game state.** Lamp switch state, `GrowSpeedMultiplier`, exposure, electricity and culling
  enable flags are never modified, and no `UnityEngine.Light` color or intensity is written. Shared
  materials are never mutated: the only material the mod changes is its **own clone** of the verified
  LED panel, installed into that lamp's own surface slot and restored/destroyed on teardown. The lamp
  housing, the plant geometry and every other renderer keep their original materials.

## Known limits / not verified

- **In-game behaviour is unverified.** No gameplay run, no screenshot, no save round-trip on disk, no
  multiplayer check, and no verification that the native owned-property list contains the expected
  codes at runtime. Acceptance tests T01–T18 remain open. Every statement above is a compile-time,
  pure-model or source-contract result, not a gameplay observation.
- **The 20 s reconcile walk and the per-frame steps are unmeasured at runtime.** The node/time budgets
  are unit-tested as pure arithmetic (`SceneScanBudget`); their real cost against a large scene has not
  been profiled.
- **World/plant illumination is deliberately not controlled.** This build writes no `UnityEngine.Light`
  color or intensity at all, so the lamp's light on the plants and the room stays exactly the game's
  own. That is the requested behaviour, not an omission.
- **Only the verified LEDLight panel family is recoloured.** FullSpectrum (`fullspectrum_light_on` is not
  present in `sharedassets0.assets`) and halogen (`halogen light on mat`) grow lamps are discovered but
  left completely untouched and reported once, because an unverifiable material clone could visibly
  break the lamp. Widening the family requires the same offline verification for its panel materials.
- **The surface clone path is compile-verified, not gameplay-verified.** That the game's on/off material
  swap lands in the same `MaterialIndex` slot the adapter owns, and that the HDR `_EmissionColor` change
  produces the expected glow, are read from the serialized assets and the shipped interop, not observed
  in a running game.
- No group/room/property selection, no disco effects, no configuration or language binding; `assets/`
  is unchanged and unbound.
- Multiplayer is host/singleplayer only. A client shows an unsupported/read-only status and writes
  nothing; no transport, snapshot or delta sync is exercised.
- The test host emits the SDK's `NETSDK1138` warning because net6.0 is out of support. The target
  remains net6.0 to match the game loader; the mod compile itself was warning-free.
- The pure-model console checks are behavioural simulations of the model and its policies; they prove
  no game hook, loader, rendering, save or transport behaviour.
- Original archive reports under `upstream/` describe the pre-migration environment, not this run.
