# GoldenHour — Verification trail

## 0.1.3 — live discovery fix — 2026-10-16

First real session with the mod loaded (0.1.2, S1MCP attached, scene `Main`). The toggle was
pressed at 18:33:49 and off again at 18:34:07. The DIAG block worked and restore was clean, but:

```
WARNING  no ColorAdjustments found on GlobalVolume — volume grading skipped.
DIAG volume: HasInstantiatedProfile=unknown
DIAG render: ambientMode=Trilight, fog=False, sun='Sun' (1, 0.89, 0.798, 1)
WARNING  fog is disabled in this scene — fog color bias skipped.
golden hour ON — bias applied to 5/7 parameters (strength 1.00).
golden hour OFF — restored 6 parameters to originals.
```

**Root cause, confirmed live via S1MCP `inspect_component` on `GlobalVolume`:**

| field | live value |
|---|---|
| `game_object_path` | `@Managers/@EnvironmentFX/GlobalVolume` |
| `isGlobal` / `weight` / `enabled` / `isActiveAndEnabled` | `true` / `1` / `true` / `true` |
| `m_InternalProfile` | `null` |
| `profile` / `sharedProfile` | both `VolumeProfile:Global Volume Profile` |

`EnvironmentFX` is a child of `@Managers`, not a scene root. The resolver filtered roots on
`"EnvironmentFX"`, so the filter never matched, `HasInstantiatedProfile()` was never called
(hence `unknown`) and the log blamed a missing `ColorAdjustments` instead of the unreachable
volume. **Fixed in 0.1.3:** the root filter accepts `Managers` or `Environment`, and the failure
message names the searched path.

**F1 is answered by the same evidence:** `m_InternalProfile == null` with
`profile == sharedProfile` means the game never instantiates its own volume profile. The 0.1.1
code would have created that clone by reading `volume.profile` and thereby decoupled the game's
per-frame colorFilter writes. The gate prevents exactly that, so the volume colorFilter bias
stays **intentionally skipped** — the safe path — and the other six parameters carry the look.

Fog was skipped correctly and the count is literal: **7 parameters are offered, 5 were written, 2
were skipped** — the volume key (the game never instantiates its profile) and fog
(`RenderSettings.fog == false` in the Main scene, so the fog color bias has nothing to write to).
That is a real limitation of the fog parameter in this scene, not a defect: the remaining five
(sky `_SunShine`, sun color, the three ambient Trilight colors) are the ones carrying the look.

Owner observation from the session: the toggle was pressed, the game stayed smooth. Visual
confirmation of the bias itself is still open — re-verify on 0.1.3.

**Deploy:** 2026-10-16, SHA-256 `6a7194297a7a4a37…` (built == installed), `mod.json` 0.1.3.
**Reviews:** delta re-review of 0.1.3 PASS-with-nits / 0 blocking (203 s) → nit round
(N-1/N-3/N-4 fixed, N-2 accepted) → nit delta re-review **PASS / 0 blocking** (102 s).
**In-Game-Verify open** — expected on the next session: no "no ColorAdjustments" line, either the
"shared profile" warning or none at all, and a visible warm bias without a frozen day/night cycle.

## 0.1.0 pre-deploy review round 1 — 2026-10-16

- Independent scoped review (deleg_f5c3749c, 359 s): **FAIL** — 2 blocking, 7 non-blocking.
- Tests at review time: 33/33 green; Release build 0 W / 0 E; version-sync 21 mods 0 drift.
- **B1 (blocking, fixed):** `SceneTargets.Handle` blended from an unrecorded `current` when the store had no entry — a key unreadable at activation (e.g. `RenderSettings.sun` null in early frames) would compound its bias on every reapply and never be restored. Fix: first write of such a key records the pristine value (`store.RecordOnce(key, current.Value)`) before blending.
- **B2 (blocking, fixed):** Main-scene-exit restore ran after the destination scene was already active: per-scene RenderSettings values were written into the menu/loading scene, and `_SunShine` went to whatever skybox was current instead of the snapshotted material — asset-level bias could survive and be re-snapshotted as "original". Fix: `RestoreAssetLevel` restores only asset-level targets — `_SunShine` into the recorded material instance (first-wins `_snapshottedSkyMat`, IsAlive-guarded) and the volume profile `colorFilter`; per-scene RenderSettings are deliberately skipped (they die with the scene).
- Non-blocking fixed in the same round: N2 `OnDeinitializeMelon` restore; N3 config-location doc corrected (global `UserData/MelonPreferences.cfg`); N4 both-side modifiers; N5 volume lookup miss logged once; N6 warn-once dedupe for per-key apply/restore failures; N1 CHANGELOG/plan wording aligned (reapply replaces, does not re-bias).
- Deferred (accepted, documented): N7 `TogglePolicy.SnapshotNeeded` is test-consumed state-machine API.
- After fixes: 37/37 tests (33 policy + 4 new source-contract facts pinning B1/B2/N2/N4), Release build 0 W / 0 E, deployed SHA256 built=installed `2fda4dc8e058e9e6bbceb0b2be8fb9ec2f6304b914a1c98bf81ba5daddff06b3`, mod.json present in `UserData/GoldenHour/`.

## 0.1.0 delta re-review — 2026-10-16

- Delta re-review (deleg_b2301cc4, 213 s): **PASS-with-nits — deploy gate cleared, 0 blocking.** Both blocking fixes confirmed (B1: `RecordOnce` before first blend, reapplies idempotent; B2: `RestoreAssetLevel` writes only into `_snapshottedSkyMat` + volume profile, per-scene targets excluded); new-invariant checks passed (material-ref vs store-Clear pairing, FailSoft inside Main, empty-store restore); live 37/37 tests, deterministic rebuild, SHA match verified by the reviewer.
- Nit disposition (all addressed in the same round):
  - N-1 shutdown/deinit path: `HandleSceneExit` now does a **full restore when still inside Main** (toggle-off, shutdown, mod unload) and asset-level only once Main is gone (scene exit); CHANGELOG + `Restore` doc comment aligned.
  - N-2 edge: `Handle`'s RecordOnce fallback now captures `RenderSettings.skybox` into `_snapshottedSkyMat` when recording `sky.sunShine`.
  - N-3 stale counts: README/plan/AGENTS rows updated to 37 tests / suite 1,389.
  - N-4 test style: global class kept (GraphicsProbe `RuntimeWiringTests` precedent); noted for the next hardening pass.
- Final artifacts after nits: 37/37 tests, Release build 0 W / 0 E, deployed SHA256 built=installed `bdca3dae5efc557c6732d1b20db00dd998cbc8ed09384bf9c8f717a1c21d7879`.

## 0.1.1 review (independent, scoped) — 2026-10-16

- Verdict: **PASS-with-nits, 0 blocking** (deleg_95661fec, 600 s, 39 api calls). Gates re-run
  by the reviewer: 55/55 tests, Release build 0 W / 0 E (`-p:S1NoDeploy=true`, no deploy).
- Invariants I1–I7 confirmed on code level: first-wins `RecordOnce` untouched, no `store.Set`
  anywhere, restore never skips, B1/B2 fixes intact, no `sharedProfile` mutation in live code,
  no double flag capture (cache-valid early return + restore-before-research), TryCast/IsAlive
  discipline, no per-frame work by default, fail-soft with WarnOnce dedupe.
- **F1 — blocker candidate (in code review undecidable, closed in 0.1.2):** reading
  `volume.profile` instantiates the URP profile; the game's `PostProcessingManager` caches a
  `ColorAdjustments` and drives `colorFilter` per frame via an `HDRColorSmoother` override
  system. If the game never instantiated the profile, the mod's clone would take over
  rendering and freeze the day/night grading. Fix in 0.1.2: only use the profile when
  `Volume.HasInstantiatedProfile()` is true, otherwise skip the volume key with a warning.
- Nits addressed: N2 (doc comment), N4 (volume cache no longer gated on `weight`), N5
  (`RestoreSun` over-count), N7 (counts: 10 contract facts, not 4), N8 (`.pre011.bak` removed).
  Documented as accepted: N3 (throttle can delay the volume key by ~2 s, bounded), N6
  (`InvalidateRuntimeCache` nulls without restoring — all current callers restore first),
  N9 (`active = true` enables the whole ColorAdjustments component, restored on exit).

## 0.1.2 — F1 fix — 2026-10-16

- `VolumeProfilePolicy.ShouldUseProfile(HasInstantiatedProfile())` gates the volume write; the
  DIAG dump now reports `HasInstantiatedProfile` so a session shows immediately which path is
  active. Rejected alternative (write into the shared asset anyway) would either be
  overwritten by the game's per-frame writes or mutate a shipped asset — both worse.
- **Delta re-review (deleg_ad779e2b, 244 s, 16 api calls): PASS-with-nits — 0 blocking.**
  Verified: the gate is read before the single `volume.profile` access (only one such read in
  the code), `HasInstantiatedProfile()` does not instantiate (`m_InternalProfile != null`), so
  the mod can never create the profile itself; the skip path leaves no dangling references and
  never writes the volume key; removing the `weight` gate strengthens "every written key is
  restorable"; `RestoreSun` still restores in the normal case; no new write without a baseline.
  Reviewer gates: 58/58 tests, Release 0 W / 0 E; `HasInstantiatedProfile` confirmed present in
  the installed `Unity.RenderPipelines.Core.Runtime.dll`.
- New nits from the delta review: **N-a** (an uninstantiated volume ended the whole search —
  fixed: `sawUninstantiated` + `continue`, warning after the loop), **N-b** (the DIAG can show
  a `_volumeInstantiated` value from an earlier resolve when the cache hits or the throttle
  applies — diagnostic only, accepted).
- Final artifacts: 58/58 tests, Release build 0 W / 0 E, deployed SHA256 built=installed
  `a97bb54b57edff7e…`. Still **not verified in game** — the DIAG dump is the intended evidence
  for the F1 question.

## 0.1.1 hardening — 2026-10-16

- Trigger: in-game report that nothing looked different, plus an external (ChatGPT) fix
  proposal. Log evidence first: the mod was loaded and initialized, and **no toggle
  activation line existed** — the A/B toggle is off by default, so "no visual change" was
  the designed state up to that point.
- Adopted from the proposal (all with tests): volume flag forcing (`active`,
  `colorFilter.overrideState` — without them a colorFilter write never reaches the screen);
  volume writes into the runtime `volume.profile` instance instead of the shared asset;
  `DIAG` dump; scene-change log line; `HasProperty` guard; ambient-mode/fog gates;
  redundant-write skipping via `Argb.IsCloseTo`; `ApplyEveryLateUpdate` and
  `DiagnosticStrongTint` as opt-in config flags.
- Rejected (with reasons, documented in the CHANGELOG): baseline replacement on external
  change, restore-skipping on external change (both weaken first-wins), and per-frame
  apply as a default.
- Not verified in game: the volume flag fix is a code-level hypothesis until a session
  shows a visible change — the DIAG dump is the intended evidence.

## 0.1.0 in-game session — 2026-10-16

- **Open.** Planned gates per docs/graphics-golden-hour-plan.md: load without passive drift; toggle on/off with before/after screenshots at the same in-game time; toggle-off restores exactly; day/night transition stays readable; owner reports frame feel unchanged on RX 9060 XT 1080p144 Ultra.
