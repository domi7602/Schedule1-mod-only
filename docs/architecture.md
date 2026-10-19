# Architecture

## Dependency Direction

```text
Mods -> Shared -> S1API / game assemblies
Mods -> optional S1MAPI / game assemblies
```

- A mod may reference `S1Mods.Shared`, S1API, S1MAPI, and installed game assemblies.
- A mod must not reference another mod. Reusable code belongs in `Source/Mods/Shared/`.
- `ThirdParty/` is not a general source dependency. Only documented APIs or explicitly linked compatibility shims may be used.
- `GameReferences/` is generated research output. It must never be a build dependency.

## Mod Layout

```text
Source/Mods/<ModName>/
  src/       Project and production code
  docs/      mod.json, README, CHANGELOG
  assets/    Runtime assets copied by Directory.Build.targets when present
  tests/     Optional isolated tests; integration tests live in Source/Tests/
```

`Directory.Build.props` owns shared compiler settings and game references. Individual project files contain only identity and genuinely mod-specific inputs.

## Runtime Boundaries

- Persistence is slot-aware and uses `SafeStorage` plus `SaveSlots`.
- Multiplayer mutations must use `NetworkGuard` authority checks.
- Harmony patches use `PatchGuard` and must tolerate game-version signature drift.
- IL2CPP `MonoBehaviour` types require the public `IntPtr` constructor.

## LightControl first gameplay test boundary

LightControl builds one `LightControl.dll` from its pure state model and a native grow-light
adapter, with a phone app for color/intensity, original reset and applying the selected state to
supported registered lamps. Storage is owned JSON through SafeStorage, isolated by active save
slot. The latest owner request supersedes the earlier surface-only selection: **the LED panel and
its emitted illumination must share the chosen color**. The source correction retains dev.4's
native-material identity and owned-clone panel path, and adds guarded color/intensity overrides
only for the exact lamp's own `ToggleableLight.lightSources` references. Both outputs use one resolved
model instruction. Native light baselines are handed back in Update and re-sampled in LateUpdate;
Original/teardown restore only still-owned outputs, preserving foreign native changes.
Plant materials, growth, electricity, switch state, transforms, cone/range and native culling remain
untouched. Plants naturally appearing color-lit is intended. FullSpectrum/halogen panels, fixed
world lamps, room/group control, direct interaction and multiplayer transport are outside this slice.
Build and pure-policy checks do not verify visible effects, native material swaps, discovery,
reload persistence or performance. Consult the installed-version record rather than assuming a
source change has already been deployed.
See the [LightControl docs](../Source/Mods/LightControl/docs/README.md) for current evidence.

## GraphicsProbe rendering discovery boundary

GraphicsProbe is a diagnostic-only first slice for the graphics-overhaul feasibility
study. Its only intended state change is writing its own report under UserData.
Runtime rendering configuration, camera requests and water-name/material heuristics
are observations, not proof of rendered passes or confirmed water identity. World
discovery remains capture-driven and bounded. The coverage repair must admit
roots across enumerated loaded scenes before descending through large hierarchies,
advance fairly within bounded breadth levels and spread native reads cooperatively
across frames. Idle updates do not discover scene objects. Scene transitions discard
in-flight work; only a completed capture may replace the persisted report. Separate
surface-like and particle/line/trail lexical buckets retain their own scan and record
budgets; neither category establishes actual water identity. A single tagged-camera
hint, if used, has explicit provenance and does not replace inactive-camera discovery.
No S1API gameplay API, Harmony hook, shader bundle or extracted game asset is needed
by the diagnostic itself; it uses the existing Shared dependency for safe report IO.
The scope, real-game acceptance gates and exclusions are recorded in
[`graphics-probe-plan.md`](graphics-probe-plan.md). Version 0.1.1 supplied partial
native evidence (report IO and active URP) but failed gameplay coverage; its evidence
does not verify the repaired traversal or its frame-time behavior. Overall acceptance
remains **In-Game-Verify open** pending a fresh real-game report and log.

## WaterShader render-path boundary (0.2.0)

WaterShader 0.1.x replaced only the evidenced Main/Map/StylizedWater2_Ocean material
slot, cloning the game's existing shader/material rather than transporting shader
programs. **That material-only boundary is deliberately superseded as of 0.2.0.** The
owner approved moving beyond material tuning after the material-only recipes (0.1.2
natural coast, 0.1.3 clear shallows) stayed visually close to vanilla and never
revealed the bottom near the shore. 0.2.0 ships an authored URP shader
(`S1Mods/ReferenceWater`) in `s1reference.bundle` together with a real planar-reflection
render path. See [the reference target](../Source/Mods/WorldLook/docs/history/water-reference-target.md)
and [the prototype slice](../Source/Mods/WorldLook/docs/history/water-reference-prototype.md).

Preserved unchanged: exact singleton/direct-child discovery (`Map.instance` →
`StylizedWater2_Ocean`), the cached binding, the owned-clone material slot,
ownership-aware restore, slow bounded retries, and byte-identical `WaterSession.cs`.
No world or renderer scans are reintroduced.

New and bounded: one mod-owned disabled reflection camera and one aspect-preserving
render target (long side ≤ 512 px), driven from the single main gameplay camera through
`RenderPipelineManager.beginCameraRendering`. The reentrancy gate closes before the
nested `UniversalRenderPipeline.RenderSingleCamera`. The global culling sense is
intentionally **not** flipped — the mirrored-pose camera is a proper rotation
(`LookRotation`), so flipping it renders the reflection inside-out (a review-caught
defect, fixed and test-pinned before release).

Every failure path restores the clone's original shader and falls back to the vanilla
recipe; the reflection path never destroys the session-owned material or writes the
native slot. Material inspection evidence is in
[WaterShader material evidence](../Source/Mods/WorldLook/docs/history/water-material-evidence.md).
A passing shader-property or AssetBundle test is not visual or GPU performance
acceptance: the look, shoreline clipping and reflection cost stay **In-Game-Verify
open**.

## AutoPackagingStation native prefab composition

AutoPackagingStation clones the complete native packaging prefab to retain its serialized station/network graph, and transplants only footprint geometry from an owned cauldron clone before any live placement. It does not add a native `PackagingStation` after spawn. Templates remain active-self beneath an inactive persistent cache, reused across registration retries and released on mod deinitialization.

S1API 3.2.1's public buildable builder exposes neither a BuiltItem setter nor pre-publication ghost registration. The mod's narrowly scoped reflection compatibility bridge therefore uses `BuildableItemDefinitionBuilder.BuildableDefinition` (private getter) and `S1API.Internal.Building.BuildableGhostRuntime.RegisterVisual(string, Func<Transform, GameObject>, bool)` (internal static). These signatures were checked in the installed S1API DLL with ilspycmd. Missing signatures abort registration before publishing; the mod does not compile against or link internal S1API source. Prefer a public pre-publication API when one is available. Physical placement/setup, native canvas and processing were exercised in the v0.4.7 session on 2026-10-15, followed by the owner's confirmation that everything tested worked. FishNet multiplayer initialization remains unverified; source signatures and component counts do not prove it.

Placed setup retains native collider geometry instead of adding or resizing a full-model root collider. The 2026-10-15 session on deployed v0.4.6 confirmed native E/canvas interaction on two stations after removing the placed root-collider manipulation. Version 0.4.7 removes the temporary read-only Awake observer, interaction/collider dumps and diagnostic bookkeeping; it does not add listeners or call Awake. The v0.4.6 evidence remains historical. The separate v0.4.7 log matches the installed DLL hash and establishes native setup/canvas/Pack/Unpack/mode mirroring; the owner then confirmed the tested gameplay session worked. Additional checks were not itemized; separate multiplayer and legacy migration coverage remain unverified. Source/policy tests are not runtime evidence.
