# Golden Hour — Graphics Overhaul Slice 2 (sky & atmosphere)

> **Status 2026-10-16 (implementation, v0.1.3 after the first live session):** slice 2
> implemented as part of `WorldLook` (`Source/Mods/WorldLook/`): multiplicative warm bias over
> SkyMat `_SunShine`, sun light color (`RenderSettings.sun`), ambient Trilight colors, fog color
> and GlobalVolume `ColorAdjustments.colorFilter` (volume runtime profile instance, snapshot +
> restore — no new volume, so "bias, not fight" holds). Ctrl+Shift+F7 A/B toggle, first-wins
> snapshot/restore, bounded reapply (≥0.5 s), fail-soft. TDD: 48 policy tests RED→GREEN + 10
> source-contract facts = 58 in `Source/Tests/GoldenHour.Tests`; full suite 1,410 green; Release
> build 0 W / 0 E. Pre-deploy review FAIL (2 blocking: write-without-snapshot, scene-exit restore
> targeting) → fix round → **delta re-review PASS-with-nits, deploy gate cleared** (0 blocking) →
> **0.1.1 hardening**: volume `active`/`overrideState` forcing (a colorFilter write with either
> flag off never reaches the screen), `DIAG` dump on activation, scene-change log line → **0.1.1
> review PASS-with-nits** (0 blocking; blocker candidate F1) → **0.1.2**: the volume profile
> instance is only used when the game itself instantiated it (`HasInstantiatedProfile`) —
> otherwise the clone would decouple the game's own per-frame day/night grading writes. Rejected
> from the external fix proposal: baseline replacement, restore-skipping, per-frame default.
> **0.1.3 (first live session, S1MCP):** the volume key never applied because the resolver
> filtered scene *roots* on `EnvironmentFX`, but the live path is
> `@Managers/@EnvironmentFX/GlobalVolume` — the filter now accepts `Managers` or `Environment`.
> The same session answered F1 with hard evidence: `m_InternalProfile == null` and
> `profile == sharedProfile`, so the game never instantiates its profile and the volume
> colorFilter bias stays **intentionally skipped**; fog is disabled in the Main scene
> (`RenderSettings.fog == false`), so the 0.1.2 run's `5/7` is literal: 7 offered, 5 written
> (sky `_SunShine`, sun color, three ambient Trilight colors), 2 skipped (volume key, fog).
> In-Game-Verify on 0.1.3 **open** (deploy + re-verify). Fog density intentionally untouched
> (visibility risk).

## Look target (owner decision, 2026-10-16)

Golden-hour mood: warm evening light. Warm horizon gradient (amber/orange) into a
deeper blue zenith, warm sun tint, atmospheric haze in the distance, ambient light
biased warm. Realistic direction, not stylized saturation.

The look is a **color/atmosphere bias**, not a time freeze: the game's day/night cycle
stays functional and readable (night remains night). A config toggle may force
golden-hour colors at all times later; the default preserves time-of-day semantics.

## Approved scope

Slice 2 changes sky appearance and atmosphere grading only:

- `SkyMat` (`Custom/ScheduleOneSky`, RenderQueue 1000) parameter overrides and/or a
  replacement skybox material.
- `@EnvironmentFX/GlobalVolume` atmosphere grading: fog color/density bias, ambient
  (Trilight) warm bias, sun light color/intensity bias via
  `EnvironmentManager/Lighting/Pivot/Sun`.
- A/B toggle (hotkey) between original and golden-hour state; original material/volume
  parameters are snapshotted before first change and restored on demand and on shutdown.

All changes are client-side cosmetic material/lighting state. No meshes, no water, no
vegetation, no pipeline asset, no camera, no save data and no network-relevant state is
changed. The mod must fail soft when the game updates change object paths, shader
property names or volume profiles: a failed lookup is skipped, reported once and shows
up in the applied-parameter count (partial apply continues with the readable targets;
it does not patch blindly). Implemented 0.1.0: unreadable keys are skipped and warned
once, missing ColorAdjustments on GlobalVolume skips only the volume key.

## Deliberately outside this slice

- Water, grass, trees, post-processing stack changes beyond fog/ambient/sun bias.
- AssetBundle shader authoring (only needed if `Custom/ScheduleOneSky` parameters
  cannot express the look; if so, this plan is revised before implementation).
- Moving the sun or changing the in-game clock, weather system or time scale.
- Extracted game assets are never redistributed; runtime reference only.

## Required first evidence (before any visual change)

Runtime-introspect the actual targets in a real session and record them:

1. Enumerate `SkyMat` shader property names/types and current values (material
   introspection at runtime; no asset extraction).
2. Record `GlobalVolume` profile components actually present (fog/tonemapping/color
   adjustment or none) and their runtime weights.
3. Record Sun light color/intensity/temperature as driven by the game at several
   in-game times (the day/night cycle writes these; the mod must cooperate).

The look knobs chosen for implementation derive from this evidence, not assumptions.

## Acceptance gates

### Build and deployment

- Pure grading/policy logic (color math, bias mapping, config, snapshot/restore
  decisions) lives in Unity-free source linked into a test host; observe failing
  regressions before implementing each behavior.
- Full mod test suite green; compile-only Release build 0 warnings/0 errors.
- Independent scoped review before deployment; fixes get a delta re-review.
- Deploy only with the game closed and built/installed Shared DLL hashes agreeing;
  verify installed DLL hash/identity after deploy.
- Version inventory and compatibility matrix updated honestly; status stays
  **In-Game-Verify open** until an actual session shows the look.

### Runtime session

1. Launch via Steam; confirm the mod loads without errors and the ORIGINAL look is
   unchanged before the toggle is used (no passive drift).
2. Enable golden hour via hotkey: sky gradient, sun tint, fog/ambient bias visibly
   match the look target. Screenshot before/after at the same in-game time.
3. Toggle back: the exact original state is restored (no cumulative drift over
   several toggles).
4. Sit through an in-game day/night transition: night stays readable, no purple/broken
   sky, no NaN/black materials; the bias cooperates with the cycle.
5. Owner observation: overall frame feel unchanged on the RX 9060 XT (1080p144 Ultra);
   this is a judgment, not a GPU measurement.
6. Change game version (or simulate property drift by renaming in a test): the mod
   reports the mismatch and leaves rendering untouched.
7. Shutdown with golden hour active: no deferred work; removing the DLL requires no
   visual restoration beyond a game restart.

## Risks (known)

- `Custom/ScheduleOneSky` is an unknown custom shader; its properties may not express
  a full gradient look. Mitigation: evidence step first; AssetBundle fallback only
  after revising this plan.
- The day/night cycle may overwrite sun/sky state every frame; overrides may need to
  reapply per frame or hook the environment update. Bounded reapply, not polling the
  world.
- URP volume blending: a global volume with priority 0 may be blended with others
  (e.g. `Sewer` at priority 2). The mod must bias, not fight, active volume weights.
