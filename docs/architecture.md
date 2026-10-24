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
  tests/     Optional existing notes/fixtures; all executable tests live in Source/Tests/
```

New scaffolds create src, docs and assets only. Add executable test projects under
`Source/Tests/<Mod>.Tests/` so solution generation and the shared test runner discover them.

`Directory.Build.props` owns shared compiler settings and game references. Individual project files contain only identity and genuinely mod-specific inputs.

## Runtime Boundaries

- Persistence is slot-aware and uses `SafeStorage` plus `SaveSlots`.
- Multiplayer mutations must use `NetworkGuard` authority checks.
- Harmony patches use `PatchGuard` and must tolerate game-version signature drift.
- Hook bodies report failures through `Diagnostics.ErrorOnce` (first hit logged, repeats counted)
  and each mod prints one `Diagnostics.StartupReport` block after patching, so a half-broken
  post-update state is visible in the first lines of `Latest.log` instead of being silent.
- IL2CPP `MonoBehaviour` types require the public `IntPtr` constructor.

## TaxiDriver traffic-light boundary

The 0.8.22 driver-controller boundary uses only the driver's existing registered NPCBehaviour/patrol. Runtime AddComponent is not network initialization. Missing registration uses existing direct Navigate; an unconfirmed prior lease blocks every replacement/recovery entry point while retaining original state and bounded deadlines. No S1API, network prefab, save, player-seat or collision changes are made. See [driver binding verification](../Source/Mods/TaxiDriver/docs/driver-binding-verification.md).

TaxiDriver 0.8.21 introduced private DriveFlags around parameterless UpdateSweep/UpdateSpeedReduction for the exact owned host autonomous physical taxi. Native TrafficLight filtering changes; generic obstacles and native turn/destination braking do not. Kinematic/unknown modes retain vanilla, and the unsafe historical GetWheelSweepCollision detour is not installed. Stationary native sweep comparisons can hold recovery time only for readable exclusive traffic blocking; they do not refill attempts or infer arrival. TaxiDriver 1.0.0 is the version-only milestone of the owner-accepted 0.8.22 code (2026-10-24); individual actions were not itemized. The earlier [traffic-light verification](../Source/Mods/TaxiDriver/docs/traffic-light-repair-verification.md) remains historical evidence, not a reopened acceptance gate.

## TaxiDriver visual wheel boundary

The visual boundary introduced in TaxiDriver 0.8.20 writes only imported wheel-node rotations. It reads cached native wheel/rigidbody state and uses a no-slip rolling approximation plus native steering angle/membership; it does not alter wheel physics, colliders, vehicle transforms or navigation. Initial binding is bounded, and caches belong to the visual model lifetime (not each ride). See the historical [wheel verification](../Source/Mods/TaxiDriver/docs/wheel-visual-verification.md); the later owner confirmation is recorded separately in the current compatibility matrix.

## HitmanPhone audit-repair boundary

HitmanPhone's repair is explicitly singleplayer-only, not a host-authoritative
multiplayer feature. A confirmed load binds identity and write destination; gameplay only
marks bounty state dirty. Save-start freezes a snapshot and save-complete checks the native
result/destination before checkpoint-bound sidecar IO through SafeStorage. Unknown world
fingerprints pause the mod rather than adopting the newest bounty file. Compatibility and
backup copies require both matching identity and content fingerprint. This coordinates the
engine and sidecar checkpoints; it is not an atomic cross-filesystem transaction.
Item registration, evidence delivery/receipts and journal rebind
remain local to HitmanPhone; no mod-to-mod dependencies are introduced. See the
[verification record](../Source/Mods/HitmanPhone/docs/audit-repair-verification.md) for
tested pure/source behavior, the matching native checkpoint restore and the 2026-10-24 owner
confirmation of 0.3.0. Individual gameplay actions were not itemized in that confirmation.

## LightControl runtime and phone UI boundary

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
untouched. Plants naturally appearing color-lit is intended. The current supported surface families
are regular LED and the verified grow-tent halogen pair; FullSpectrum, other unsupported panels,
fixed world lamps, room/group control, direct interaction and multiplayer transport remain outside
this slice.
Build and pure-policy checks do not verify visible effects, native material swaps, discovery,
reload persistence or performance. Consult the installed-version record rather than assuming a
source change has already been deployed.
The graphite phone UI uses an owned 1201x655 reference canvas, a four-column table with a
sticky heading, and a pending-color chooser with Cancel/Apply and ten choices including
Rainbow. Seven brightness steps apply immediately; More owns mode/reset and counted copy
actions. Every item action binds the captured session and exact key and revalidates before
the runtime call. Unique forwarding callbacks retain per-widget listener identity.

Version 0.1.0 was owner-confirmed finished on 2026-10-24; native interaction/lifecycle and
font parity were accepted without itemized actions. Arial remains the active fallback.
Version 0.1.1 is the later log-only Rainbow-tick repair; its deployment does not create a new
gameplay evidence claim. See [LightControl docs](../Source/Mods/LightControl/docs/README.md),
[graphite verification](../Source/Mods/LightControl/docs/graphite-ui-verification.md) and the
[changelog](../Source/Mods/LightControl/docs/CHANGELOG.md). Earlier picker/deployment records
remain historical, including the dev.16 boundaries.

## WorldLook native-sky baseline

The active WorldLook build is canonical under `Source/Mods/WorldLook/`. One MelonMod owns startup settings, the F7 session state, a mild native-light module and the exact ocean material session. The game owns the skybox and its celestial/cloud rendering. The light module adjusts five evidenced native color channels, converts native HHMM explicitly, tracks game drift versus its own readbacks and retains bounded restore-only work after uncertain cleanup.

The water shader is embedded, environment-only and uses native SkyState colors with finite ambient fallback plus native wind. It owns one material clone on the exact ocean renderer; uncertain restoration retains the clone instead of clearing the slot. Camera depth/opaque request checks are not proof of actual texture contents. The shared bundle is released without destroying borrowed shader objects still referenced by uncertain consumers.

Dev.3 removes the F9 snapshot and normal diagnostic chatter; F7, concise startup/toggle messages and error/fallback warnings remain. The water shader, recipe and ownership/restore guards are unchanged. Full GraphicsProbe and the standalone custom cloud-sky prototype are archived outside the active build. The [water-surface record](../Source/Mods/WorldLook/docs/water-surface-verification.md) preserves dev.2 runtime evidence and the owner's closed water/ON-OFF acceptance; [native-baseline verification](../Source/Mods/WorldLook/docs/native-baseline-verification.md) preserves dev.1 history.

## Historical GraphicsProbe rendering discovery boundary (archived)

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
does not verify the repaired traversal or its frame-time behavior. Those acceptance gaps
belong to the retired diagnostic and are not current work.

## Historical WaterShader render-path boundary (0.2.0 prototype)

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
acceptance. The prototype's unresolved look, shoreline clipping and reflection-cost checks
remain historical; the current environment-only WorldLook candidate has its own record above.

## AutoPackagingStation native prefab composition

AutoPackagingStation clones the complete native packaging prefab to retain its serialized station/network graph, and transplants only footprint geometry from an owned cauldron clone before any live placement. It does not add a native `PackagingStation` after spawn. Templates remain active-self beneath an inactive persistent cache, reused across registration retries and released on mod deinitialization.

S1API 3.2.1's public buildable builder exposes neither a BuiltItem setter nor pre-publication ghost registration. The mod's narrowly scoped reflection compatibility bridge therefore uses `BuildableItemDefinitionBuilder.BuildableDefinition` (private getter) and `S1API.Internal.Building.BuildableGhostRuntime.RegisterVisual(string, Func<Transform, GameObject>, bool)` (internal static). These signatures were checked in the installed S1API DLL with ilspycmd. Missing signatures abort registration before publishing; the mod does not compile against or link internal S1API source. Prefer a public pre-publication API when one is available. Physical placement/setup, native canvas and processing were exercised in the v0.4.7 session on 2026-10-15, followed by the owner's confirmation that everything tested worked. FishNet multiplayer initialization remains unverified; source signatures and component counts do not prove it.

Placed setup retains native collider geometry instead of adding or resizing a full-model root collider. The 2026-10-15 session on deployed v0.4.6 confirmed native E/canvas interaction on two stations after removing the placed root-collider manipulation. Version 0.4.7 removes the temporary read-only Awake observer, interaction/collider dumps and diagnostic bookkeeping; it does not add listeners or call Awake. The v0.4.6 evidence remains historical. The separate v0.4.7 log matches the installed DLL hash and establishes native setup/canvas/Pack/Unpack/mode mirroring; the owner then confirmed the tested gameplay session worked. Additional checks were not itemized; separate multiplayer and legacy migration coverage remain unverified. Source/policy tests are not runtime evidence.
