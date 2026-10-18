# Changelog — GoldenHour

## 0.1.5 (2026-10-17)

Clock-weighted warm bias. A constant multiply was painting noon gold while the sky stayed blue.

### Changed
- `DaylightPolicy` maps minutes-since-midnight to a weight that peaks at 06:00 and 18:00 and is zero at noon, at night, and outside a 90-minute lobe. Applied strength is `config.Strength * weight`. At 12:22 the weight is 0, so the sun color — and the water specular that reads it — stay the game's own light.
- Unreadable time fails soft to weight 0 and logs once. It does not fall back to full gold.
- `DiagnosticStrongTint` still ignores the clock so a diagnostic tint stays visible.

### Fixed
- `BaselinePolicy` now recognizes the blend at the previous strength as our own write. A strength drop writes the new blend from the stored game baseline instead of adopting the old tint. Without that, noon would freeze the evening gold and restore a tinted value.

### Not in this build
- Distance haze is not enabled. `RenderSettings.fog` is false in Main, and SkyMat has no horizon-color property. Turning fog on would be a visibility change without a measured density hook.

### Tests
- 82 GoldenHour tests pass, including the noon/12:22 weight cases and the strength-drop release. In-game verify of 0.1.5 is open. The 0.1.4 sun-color measurement does not cover this clock.

## 0.1.4 (2026-10-17)

The sun color never showed the bias in game — measured ON and OFF values were bit-identical.
Root cause found and fixed via S1MCP live introspection (`get_component_property` on
`EnvironmentManager/Lighting/Pivot/Sun`, `set_time_of_day` A/B at 06:00/12:00/18:00).

### Fixed
- **The game rewrites `RenderSettings.sun.color` continuously** (its day/night grading drifts
  the value every frame), and the 0.1.3 default reapply cadence (once per second) lost every
  frame race — the bias was overwritten within ~16 ms and never showed. `ApplyEveryLateUpdate`
  is now **on by default** (0.1.3 assumed the game does not rewrite these values; that evidence
  was wrong). Verified 2026-10-17: with per-frame apply the sun color reads
  `(1.133296, 0.809716, 0.505238)` — matching vanilla `(0.985, 0.834, 0.701)` ×
  `SunLightTint (1.15, 0.97, 0.72)` within rounding of the drifting values.
- **The reapply froze the sun color at its toggle-time value.** `SceneTargets.Handle` blended
  from the first-wins activation snapshot, so the game's drift was never adopted. The new
  `BaselinePolicy.Evaluate` rebases onto the game's current value whenever it detects the game
  overwrote our write (own writes are detected by value and never re-adopted — no compounding
  under a constant recipe), and `SnapshotStore.Update` records the new baseline so restore
  returns the latest vanilla value instead of dragging the sun color back hours.
  **Known limitation (non-blocking, review NB 1):** changing `Strength`/`DiagnosticStrongTint`
  while the bias is active can tint once on top of a previous write (the own-write check
  compares against the recomputed blend of the new recipe). Self-heals in one frame on
  game-graded targets (sun/ambient/fog — and the volume colorFilter, which the game also
  rewrites per frame); sky `_SunShine` keeps the compounded value. The compounding event
  adopts the once-tinted value as baseline, so toggle-off restores a tinted value for that
  key (the "restore returns the latest vanilla value" property holds only for writes without
  a mid-activation recipe change). Not reachable in normal play (config changes take effect
  on restart, when the mod re-initializes); a per-key last-written-value comparison is the
  tracked follow-up.
  **Left open (review NB 3):** the "restore never skips on external changes" invariant is
  only pinned by a phrase-absence source contract (`DoesNotContain("changed externally")`),
  not by a positive assertion over the restore paths.

### Revised contracts (0.1.4 revises 0.1.1/0.1.3 decisions)
- `PerFrameApplyIsOptInNotDefault` → `PerFrameApplyIsTheDefault` (the "does not rewrite
  continuously" evidence was wrong).
- `FirstWinsBaselineIsNotReplacedAndRestoreNeverSkips` → `BaselineAdoptsDriftAndRestoreNeverSkips`
  (first-wins freezes the parameter and makes restore stale; restore is still never skipped).

### Live evidence (2026-10-17, Save 2, S1MCP)
- Baseline (OFF, 12:00): `Sun.color` drifts continuously `(0.985047→0.985153, …)` — the game
  writes every frame. `Sun.intensity` = 1.72 (0 at 06:00, 2.6 at 18:00 — the day/night curve).
- ON with per-frame apply (12:00): `(1.133296, 0.809716, 0.505238)` = vanilla × tint, stable
  across 10 samples. Toggle via Ctrl+Shift+F7 shows `bias applied to 5/7 parameters`.
- The old "an/aus bit-identisch (0.872414/0.64745/0.3946)" measurement was taken at ~06:00
  (intensity 0) with the losing 1 s cadence — no write ever survived to the sample.

## 0.1.3 (2026-10-16)

Found in the first live session (S1MCP, scene `Main`, 0.1.2 loaded): the volume key never applied
and the log blamed the wrong thing.

### Fixed
- **The GlobalVolume was never found.** The resolver filtered scene roots on `"EnvironmentFX"`,
  but the live path is `@Managers/@EnvironmentFX/GlobalVolume` — `EnvironmentFX` is a child of
  `@Managers`, not a root. The filter therefore never matched, `HasInstantiatedProfile` stayed
  unread and the log claimed "no ColorAdjustments found on GlobalVolume". The root filter now
  accepts `Managers` **or** `Environment`, and the failure message names the path it actually
  searched (`no GlobalVolume found under @Managers/@EnvironmentFX`).

### Live evidence (0.1.2, S1MCP `inspect_component` on `GlobalVolume`)
- `game_object_path: @Managers/@EnvironmentFX/GlobalVolume`, `isGlobal: true`, `weight: 1`,
  `enabled: true`, `isActiveAndEnabled: true` — the volume is live and rendering.
- **`m_InternalProfile: "null"`, `profile` == `sharedProfile` ("Global Volume Profile")** — the
  game never instantiates its own volume profile. That is the F1 scenario, confirmed: had the
  0.1.1 code read `volume.profile`, it would have created the clone and decoupled the game's
  own per-frame colorFilter writes. **The gate is doing real work.**
- Consequence: the volume colorFilter bias stays **intentionally skipped**. The 0.1.2 run
  reported `5/7` literally: 7 parameters are offered, 5 were written, 2 skipped — the volume key
  (no game-instantiated profile) and fog (`RenderSettings.fog == false` in the Main scene, so
  there is nothing to write to). The five written ones (sky `_SunShine`, sun color, three ambient
  Trilight colors) carry the look.

### Tests
- 58/58 (`Source/Tests/GoldenHour.Tests`), full suite 1,410 green over 15 projects, Release build
  0 W / 0 E.

### Review
- Delta re-review of 0.1.3 (read-only, 203 s / 13 api calls): **PASS-with-nits, 0 blocking** —
  root filter reachable, cheap and null-safe; the new warning no longer blames a missing
  component; every prior invariant intact (first-wins `RecordOnce`, no `store.Set`, restore never
  skipped, `HasInstantiatedProfile()` still immediately before the single `volume.profile` read,
  `sawUninstantiated`, fail-soft + WarnOnce, no per-frame default); the 10 contract facts match
  the source; tests 58/58 and full suite 1,410 both re-executed.
- Nit round: **N-1 fixed** — a volume that was *found* but yielded nothing writable was still
  reported as "no GlobalVolume found". Same class of mis-blame this release set out to remove,
  one level deeper; a `sawVolume` flag now selects an accurate message.
  **N-3 / N-4 fixed** (stale limitation framing in `mod.json`, missing test line here).
  **N-2 accepted** — the uninstantiated path re-walks the `@Managers` subtree every ~2 s while the
  toggle is on; bounded and throttled, no per-frame cost, and a discovery cache would add runtime
  state for no behavioural gain.
- Nit delta re-review (read-only, 102 s / 9 api calls): **PASS, 0 blocking** — `sawVolume` set
  before the component lookup so all three failure modes reach the accurate message,
  `sawUninstantiated` still wins ties, the flag is a method-local that cannot leak across calls,
  and the new contract assertion is a genuine regression guard (58/58 re-verified).
- **Deployed 2026-10-16: SHA-256 `6a7194297a7a4a37…` built == installed**; `mod.json` 0.1.3,
  PDB alongside. In-game re-verify open.

## 0.1.2 (2026-10-16)

Follow-up to the 0.1.1 review (PASS-with-nits, 0 blocking). The blocker candidate and the
non-blocking findings were addressed.

### Fixed
- **F1 (blocker candidate): the mod could decouple the game's own day/night grading.**
  Reading `Volume.profile` instantiates the profile (`m_InternalProfile`), and the
  VolumeManager then blends from that clone instead of the shared asset. If the game itself
  never instantiated the profile, that clone would freeze the game's per-frame colorFilter
  writes (day/night grading) at the values present on first activation. Fix: the volume is
  only used when `Volume.HasInstantiatedProfile()` is true — i.e. when the clone is the
  profile the game itself writes into. Otherwise the volume key is skipped (with a warning)
  and the other six parameters still apply (`VolumeProfilePolicy`).
- **N4:** the volume cache is no longer gated on `Volume.weight`, so a weight change can no
  longer drop the restore target while the profile instance still carries the tint.
- **N5:** `RestoreSun` no longer counts a restore when neither the recorded nor the current
  sun light is alive (the "restored N parameters" log was over-counting).
- **N2:** the `LogDiagnostics` doc comment no longer claims it writes nothing — it reads
  state and can resolve the volume cache (which may restore previously forced flags).
- **N8:** the temporary `SceneTargets.cs.pre011.bak` was removed from the source tree.

### Delta re-review
- Independent delta re-review (deleg_ad779e2b): **PASS-with-nits, 0 blocking.** F1 confirmed
  closed (the gate precedes the only `volume.profile` access; the mod can never instantiate the
  profile itself), all invariants intact. Follow-up: **N-a** — an uninstantiated volume no
  longer ends the search for a later instantiated one (`continue` + warning after the loop);
  **N-b** (DIAG may show a stale `HasInstantiatedProfile` on cache hit) accepted as diagnostic.

### Tests
- 58 tests (48 policy/unit + 10 source-contract facts), full suite 1,410 green.

## 0.1.1 (2026-10-16)

Hardening round after the in-game visibility question and an external (ChatGPT) fix
proposal. Only the parts that preserve the reviewed invariants were adopted; the parts
that weaken them were rejected and are listed below.

### Fixed
- **Volume write could be a silent no-op.** `colorFilter.value = x` never reaches the
  screen while `ColorAdjustments.active` or `colorFilter.overrideState` is off. The
  component/override flags are now forced before the write, captured first, and restored
  on the way out (`VolumeFlagPolicy`).
- **Volume writes no longer touch the shared profile asset.** They go into the volume's
  runtime profile instance (`volume.profile`), so the shipped asset is never mutated and
  nothing can leak across scene loads.

### Added
- `DIAG` dump on every activation: camera `renderPostProcessing` + `volumeLayerMask`,
  volume `active`/`overrideState` + `colorFilter`, sky name with `HasProperty("_SunShine")`
  check, `ambientMode`, `fog`, sun color — the evidence trail for the in-game verify.
- Scene-change log line (`scene '<name>' (Main guard: …)`), so an idle toggle is visible
  at a glance in the log.
- Config: `ApplyEveryLateUpdate` (opt-in per-frame apply, default **false**) and
  `DiagnosticStrongTint` (opt-in unmissable volume tint, default **false**).
- Gates and guards: ambient Trilight keys are only written when `ambientMode == Trilight`,
  fog color only when `fog` is on, sky access is `HasProperty`-guarded, and redundant
  writes are skipped via `Argb.IsCloseTo`.

### Not adopted (deliberately)
- Baseline replacement on external change and restore-skipping on external change: both
  weaken the first-wins snapshot/restore invariant confirmed by the 0.1.0 review — the
  bias could then no longer be fully undone.
- Per-frame apply as a **default** (7 IL2CPP read/write pairs per frame at 144 Hz); the
  0.1.3 evidence shows the game does not rewrite these values continuously.

### Tests
- 55 tests (51 policy/unit + 4 source-contract facts), full suite 1,407 green.

## 0.1.0 — 2026-10-16

Initial slice (graphics overhaul slice 2).

### Added
- Golden-hour look bias, client-side and cosmetic: warm multiplicative tint (hue shift only — the day/night cycle keeps magnitude control) over
  - SkyMat `_SunShine` HDR glow,
  - Sun directional light color,
  - Ambient Trilight sky/equator/ground colors,
  - fog color,
  - GlobalVolume `ColorAdjustments.colorFilter`.
- A/B toggle via Ctrl+Shift+F7 (Main scene only, uGUI input-field guard, game-focused guard).
- First-wins snapshot of all seven parameters on activation; restore on toggle-off (full) and on scene-exit/shutdown/unload (full while still in Main, asset-level only — skybox material instance + volume profile — once the Main scene is gone, because per-scene RenderSettings die with the scene); fail-soft: errors disable the feature and restore what was recorded.
- Bounded reapply while active (default 1 s, minimum 0.5 s): values written between reapplies are **replaced** by the snapshot-based bias (the look holds while active); how the cycle behaves under this is validated at the In-Game-Verify gate, not assumed.
- TOML-sidecar config (`Strength`, `ReapplyIntervalSeconds`) via S1Mods.Shared ModConfig.
- Pure policies unit-tested in `Source/Tests/GoldenHour.Tests` (37 tests: blend math incl. non-finite guards, tint-table bounds, snapshot first-wins semantics, toggle state machine, reapply cadence, plus 4 source-contract facts pinning the pre-deploy review fixes).
