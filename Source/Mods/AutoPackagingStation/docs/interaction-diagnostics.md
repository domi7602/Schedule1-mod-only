# Native interaction diagnostic / collider A/B

## Resolved 2026-10-15 — this file is historical evidence

The 0.4.6 collider A/B resolved the E-prompt defect. The 2026-10-15 07:27–07:28 session against the deployed 0.4.6 DLL recorded: two custom station setups (`native interaction resolved on 'IntObj'`), four `[autopack-ui] native canvas opened` breadcrumbs (two per station GUID), `Native PackSingleInstance executed 8x` ×2, `Native Unpack() executed 8x` ×1, mode mirroring UNPACKAGE→PACKAGE and a vanilla comparison dump (`stage=vanilla-start`). Hypothesis 1 is supported: the removed placed root collider was intercepting selection before the native child IntObj. Build 0.4.7 retires all diagnostic scaffolding (both source files deleted, Awake observer unpatched, dumps and bookkeeping removed).

Checklist status for v0.4.6 (historical, 2026-10-15): 1 partial (vanilla comparison dump captured; vanilla E prompt itself not recorded) · 2 passed (two 4x4 placements, footprint=16 log-backed; blueprint colors owner-reported) · 3 passed (E prompt works) · 4 passed (canvas opens, station GUID matches) · 5 implied (canvas opened twice per station, so it closed in between; Escape itself not logged) · 6 passed (pack 8x ×2, unpack 8x ×1, mode mirroring) · 7 open (no Pack-Up breadcrumb) · 8 open (no save→reload cycle in the session) · 9 passed (two stations, instance-specific canvas targets) · 10 open (no second peer) · 11 open (walk-around not recorded; the collider dump confirms visual-root collision colliders exist).

## Subsequent v0.4.7 smoke test — partial verification, 2026-10-15

The owner-supplied 09:01:53–09:02:49 MelonLoader excerpt loads AutoPackagingStation 0.4.7 with SHA-256 `c942fd65d8a8c2a50ad3adad79c27c903d66a1938e4193bbf6b60ccd99fe6580`, matching the reviewed built and installed DLLs. Initialization, PatchGuard 1/1, native prefab registration and the 16-tile (4x4) footprint succeed.

- At 09:02:36, fresh station `41e512d0-f101-4710-b685-c35830f8002b` resolves native IntObj and completes setup. Both setup triggers report the same GUID; this is not evidence of two simultaneous stations.
- At 09:02:39, its native canvas opens with `inputSlots=2, outputSlotPresent=True`.
- At 09:02:44 and 09:02:49, native PackSingleInstance executes 8x per logged cycle. At 09:02:47, native Unpack executes 8x. Canvas mode mirrors UNPACKAGE then PACKAGE.
- No AutoPackagingStation error appears in the supplied excerpt. The retired diagnostic dumps are not present in it.

At the log-only stage, this established the observed single-station native canvas and processing path after diagnostic retirement, not complete mod acceptance. Prompt appearance, Escape/close/reopen, a second station, F Pack-Up/refunds, save→reload, collision walk-around, vanilla comparison, multiplayer and legacy migration were not established by the excerpt alone. `No saved AutoPackagingStations found` occurs before this fresh placement and does not exercise persistence.

### Subsequent owner confirmation — 2026-10-15

After supplying the log, the owner reported that everything tested in-game worked. Version 0.4.7 gameplay acceptance is owner-confirmed for that tested session. This firsthand report supplements the narrower log evidence above. Additional actions were not itemized; no separate multiplayer/0.4.1 migration result was supplied. The archived checklist and earlier log-only limits remain historical evidence, not a contradiction of the owner's later acceptance.

## Scope and current evidence (historical v0.4.6 experiment)

Build 0.4.6 removes only the placed root-collider creation/resize/enable block and adds read-only diagnostics. It does not implement a new E handler, activate IntObj, invoke Awake, add native interaction listeners, alter layers, add networking components or change the template/footprint/ghost. `HideBaseRenderers`, F-Pack-Up and the processing/save paths are unchanged.

The captured 0.4.5 log (2026-10-14, 19:57–19:58) loaded DLL SHA-256 `92355f4a5b01438d7a106347ee48820d6e60af968500c811209019f2f3adf697`. Two different station GUIDs each received both setup triggers and resolved `IntObj`. There is no native-canvas-open breadcrumb or interaction/collider/Awake dump. This confirms setup, not a working E prompt, and cannot establish the collider hypothesis by itself.

Earlier 0.4.4 evidence (19:38) is historical: composition/registration/shop injection succeeded, but root-only interaction lookup aborted visual setup. Model display on 0.4.5 was owner-accepted (19:48), not menu acceptance. The expected GLB is absent; existing procedural visuals are used. None of this establishes 0.4.6 gameplay acceptance.

Ranked hypotheses and falsifiers:

1. **The rewritten root collider intercepts selection before native child IntObj.** This is mechanically plausible: native `InteractionManager.CheckHover` (RVA `0x66B5A0`) resolves hit colliders to interactables with `GetComponentInParent` (generic call at `0x66B967`), which never descends to children, and cuts the sphere candidate search at `0x66BAC8` once `sphereHit.distance + 0.075 > nearestBlockingRayDistance + 0.01` (RayCastAll/SphereCastAll in `DoCasts` at `0x66C7A0`, `interaction_SearchMask`, 4 m, `QueryTriggerInteraction.Collide`, sphere radius 0.075 m). A nearer root hit without a resolvable Interactable in its parent path can therefore suppress the child IntObj; actual hits still depend on live layer/geometry. Test a fresh station in 0.4.6, where all inherited collider geometry is retained. Native E/Canvas returning supports this hypothesis; no return rejects it as the complete explanation. Do not add a replacement collision child during this test.
2. **Serialized UnityEvent wiring is missing or different on the custom IntObj.** The earlier suspicion that `PackagingStation.Awake` registers hover/start listeners is disproven for this build: the native UserLogic body at RVA `0x8911A0` (token 0x060049F8) initializes light, switch, slots, filter and slot callbacks but never touches `IntObj` (field +0x240) or its events. Serialized persistent listeners are therefore the remaining wiring mechanism to compare. The dump now reports `onHovered`/`onInteractStart`/`onInteractEnd` persistent counts, target names, method names and listener states for vanilla and custom stations. Persistent entries are serialized wiring evidence, not runtime listener execution proof. Do not add listeners manually based on this comparison.
3. **Different IntObj state, layers, transform ancestry or collider geometry prevents native selection.** Compare the vanilla/custom dumps and inspect the manager while actually aiming at the custom station. A setup-time selection snapshot is not an aimed reproduction. Native `InteractableObject.Hovered()` (`0x669B10`) invokes `onHovered` and only then suppresses display while `interactionState=Disabled`, and `PackagingStation.Hovered()` (`0x892500`) sets `IntObj.interactionState`, so capture state before and after hovering.

Awake observer context (updated after native tracing): `PostfixObserved` proves the native `PackagingStation.Awake` wrapper completed for that identity (slot/filter/callback initialization ran); it does **not** prove E-listener wiring, because this build's Awake path contains no listener registration. `NotObserved`/`ObserverUnavailable` are unchanged in meaning. Use this evidence for instance lifecycle only; use the event dumps for wiring.

## Diagnostic evidence

`[autopack-interaction]` identifies this probe. Full dumps are once per native pointer + Unity instance ID, so Start and BuildEvents cannot double-print the same station. Records contain scalar identities only and clear when Main unloads.

- `Awake observer installed=True`: the patch was accepted; this alone is not execution proof.
- `Awake postfix observed`: the native callback returned to the observer for that specific identity. It does not prove every user-logic/listener branch ran.
- `awakeEvidence=PostfixObserved`: matching callback observation exists for the world dump's identity.
- `awakeEvidence=NotObserved`: no callback was recorded; not an assertion that Awake was skipped.
- `awakeEvidence=ObserverUnavailable`: no usable hook; lifecycle outcome is unknown.
- `stage=setup-complete`: custom world snapshot after visual setup, hiding and configured interaction range.
- `stage=vanilla-start`: comparison snapshot for a native packaging item; no vanilla state is written.
- Station/IntObj full paths, activeSelf/activeInHierarchy/enabled/layer/range/state/type/angle, display references and ancestor local/world positions/scales are measured.
- All collider entries include scope (root, IntObj subtree, other child), full path, actual native type, enabled/isTrigger/active/layer, world bounds and local shape where applicable. Inactive objects are included; their world bounds may be empty.
- Event-object presence is explicitly not a listener-count or listener-execution assertion.
- The manager snapshot records its search mask and hovered/valid-hovered references **at setup only**. For live target-selection diagnosis, aim at an identified station and leave the scene running for read-only inspection.
- `[autopack-ui] native canvas opened for station ...` comes from actual controller observation of the open native canvas. The revised setup-complete message deliberately leaves prompt/canvas unverified.

Note: `HideBaseRenderers` also parks native product/packaging alignment transforms at scale zero; it is not exclusively renderer flags. Do not change it on suspicion. The ancestor-scale dump exists to establish whether those transforms actually affect IntObj or its colliders.

## Original pre-session acceptance checklist (archived)

Original deployment state: 0.4.6 deployed 2026-10-14 (built=installed hash verified; diagnostics classes present in that DLL). All items below were open before the 2026-10-15 morning session. The resolved section above records the later partial results; unchecked boxes below preserve the original experiment checklist, not the current acceptance status. The subsequent v0.4.7 smoke test above records its separate, partial verification scope.

Original test procedure: use a backed-up test save and launch via Steam. Confirm the log loads 0.4.6 and the installed DLL hash before interpreting this experiment. Record each outcome separately; do not convert a passed compile/source test into gameplay acceptance.

1. [ ] Place a vanilla PackagingStation beside the custom station; confirm its normal E prompt and menu and capture its comparison dump.
2. [ ] Place a fresh custom station; retain the 4x4 placement blueprint/ghost and measure expected grid occupancy/alignment.
3. [ ] Aim at the custom machine within range; the native E prompt appears.
4. [ ] Press E; the native PackagingStation canvas opens and its station identity matches the target.
5. [ ] Press Escape; the canvas closes and gameplay/cursor return correctly.
6. [ ] Exercise Package and Unpackage modes with compatible items; observe actual conversions and quantities.
7. [ ] Aim at the machine and press F; Pack-Up returns the station and contents once, frees placement occupancy and does not leave a native object behind.
8. [ ] Save, return/load, then aim and press E again; contents and mode retain their expected values.
9. [ ] Place two custom stations simultaneously; prompts/canvas targets/automation remain instance-specific.
10. [ ] If a second peer is available, exercise host/client native interaction and spawning without changes to the serialized FishNet graph. Existing host-only F authority and the separately documented unpackage-mode synchronization limitation remain unchanged.
11. [ ] Walk around/against the whole 2x2 model from all sides. Record any region that is passable after retaining native collider geometry; do not assume the inherited collider fits the larger model.

If E is restored, retain the removal and only discuss separate physical-collision geometry if check 11 proves it necessary. If E is still missing, use Awake evidence first, then compare IntObj/ancestors/layers/colliders/position and live manager selection. Investigate native listener wiring only after those structures and lifecycle are established. Never patch several hypotheses at once.
