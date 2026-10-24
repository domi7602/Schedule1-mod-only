# WorldLook dev.2 — prepared in-game acceptance

Prepared 2026-10-24 for **0.3.0-dev.2**. This is a runbook and blank result checklist,
not a completed game test. The [water-surface record](water-surface-verification.md)
owns prior observations; [compatibility](../../../../docs/compatibility.md#verification-matrix)
owns current acceptance. Calm morning motion was already owner-confirmed. The work below
targets direct sun, positive warmth, state transitions and measured performance.

## 1. Establish the candidate and session

Use the installed candidate; preparing this run does not require a rebuild or deployment.

| Identity | Expected value |
|---|---|
| WorldLook version | `0.3.0-dev.2` |
| Installed `Mods/WorldLook.dll` SHA-256 | `410fecf7dda5f079e86807942eb18ef98168201c4a9dddea124391e3d5bd1645` |
| Embedded bundle SHA-256, to confirm in a fresh F9 report | `29557b87609c367e3b568d84d10400d67c980581a1e3491fbd455def9794c8ef` |
| Startup profile | `Enabled=true`, `LightingEnabled=true`, `WaterEnabled=true`, `Warmth=1.0` |

The read-only preparation on 2026-10-24 found the game closed, matching DLL/PDB/metadata
and unchanged profile bytes. Ten existing files were copied and hash-verified under the
optional local evidence folder `.scratch/worldlook-acceptance-prep-20261024-2dec1423/`;
`preflight.json` records identities and `before/` preserves the old log and status files.
The existing `status.json` is from **2026-10-23**, not a fresh test result. Shared's current
hash differs from the older deployment record; retain the freshly captured dependency
identity rather than attributing a new session to the old environment. No dependency was replaced.

Before the actual session, recheck candidate/profile identity and record current game build,
loader/framework versions, dependency hashes, selected singleplayer slot, resolution,
quality/render scale, VSync/FPS limit, location and camera framing. Choose an owner-designated
test slot. Native saves are separate from `UserData`; preserve the selected save with the
game closed before any deliberate save/sleep/time-change scenario. Do not overwrite another slot.
Use an existing recorder for screenshots and, when available, raw frame timings.

Launch through Steam only:

```pwsh
Start-Process "steam://rungameid/3164500"
```

Load the selected singleplayer slot and move to a reproducible shoreline view with visible
submerged geometry and direct sunlight. Record starting time/weather. This runbook does not
require teleporting, setting time/weather through MCP or installing a diagnostic mod. Any
later controlled-world changes must be explicitly recorded with their before/after state.

## 2. Capture evidence without losing the preceding step

- **Ctrl+Shift+F7:** toggle light and water for this process/session, only in Main.
- **Ctrl+Shift+F9:** write `UserData/WorldLook/status.json`; usable in Main and the menu.
- Both shortcuts require focused gameplay/application input and no focused text field.
- After a load/toggle, allow state to settle before F9. The hotkey handler runs before
  that frame's light update, so the same toggle-frame report is not an end-state check.
  Water discovery retries at five-second intervals; record the actual wait. If still
  unready after a bounded observation window, retain the report as **INCONCLUSIVE** and
  inspect the log rather than waiting or toggling indefinitely.
- Copy each fresh status to a unique step filename **before the next F9**. Only one prior
  report survives as `status.json.bak`. Confirm `capturedUtc` belongs to this step and
  record its SHA-256. Capture labelled screenshots with the same step ID and ON/OFF state.
- Copy `MelonLoader/Latest.log` after the session and before another game launch. Preserve
  full exception context; do not discard evidence of a fault by repeatedly toggling/restarting.

Use a new session evidence directory under `.scratch/`, distinct from the prepared `before/`
copies. Example step IDs: `01-start-on`, `02-sun-off`, `03-sun-on`, `04-warm-on`,
`05-night-on`, `06-menu-on`, `07-reload-on`, `08-menu-off`, `09-reload-off`.
Each result needs UTC/local time, game HHMM, location/camera, state, files, observation,
owner judgment where applicable, and **PASS / FAIL / INCONCLUSIVE / NOT RUN**.

## 3. Verify the active starting state

Read the fresh loader log and F9 report together. In settled Main, expect:

| Area | Expected observation |
|---|---|
| Identity | Matching `version` and `shaderBundleHash`; fresh `capturedUtc`; record `shaderLoadRoute` (`embedded-memory` or the hash-matched `embedded-cache` fallback) |
| Coordinator | `mainLoaded=true`, `lookRequested=true`, `skyMode="native"`; both module fault flags false |
| Lighting | `desiredEnabled=true`, `loading=false`, `timeReadable=true`, `active=true`; five readable channels in the documented native setup; `pendingRestores=0` |
| Water | `enabled=true`, `state="Applied"`, `materialStatus="reference active"`, `referenceReady=true`, `ownedReflectionCameras=0` |

Record the initial `lighting.errors` count and `lastError`; investigate existing errors as
well as later increments. A constant nonzero counter does not establish an error-free run.
Shader load routes `failed` or `not-loaded` are not valid active reference-water evidence.
`Applied` alone can also describe fallback water. `referenceReady` alone is shader readiness,
not material binding. `sceneInputsRequested` only reports camera requests, not correct GPU
texture contents. Missing readiness or mismatched identity cannot establish visual acceptance.

## 4. Compare direct sunlight and exercise OFF → ON

1. From the fixed shoreline view, record ON with visible direct-sun glints.
2. Toggle OFF once, settle, save a fresh F9 report and an explicitly labelled OFF image.
   Expect `lookRequested=false`; lighting `desiredEnabled=false`, `active=false`,
   `appliedStrength=0`, `readableChannels=0`, null time fields and `pendingRestores=0`.
   Water should be disabled, `Stopped`, `released` and `referenceReady=false`.
   Inspect the image for restoration of the native appearance as well as the flags.
3. Toggle ON once, settle and capture again. Expect the active state from step 3 and a
   visible return of the effect, with no new faults or restore errors.
4. Ask the owner to judge overall appearance, smaller/irregular sun glints and retained
   underwater clarity. Record the actual verdict. Move the camera slowly for the new
   direct-sun condition; still images alone cannot establish motion/shimmer behavior.

Keep framing/settings fixed and record elapsed game time/weather for each image. No direct
sun or materially changed conditions means **INCONCLUSIVE** for the glint comparison.

## 5. Observe positive warmth and night

At a natural or separately agreed test-world warm peak, capture ON and a comparable OFF/ON
view. Exact **06:00** means `timeHhmm=600`, `minutesSinceMidnight=360`; **18:00** means
`1800` and `1080`. At either exact peak `appliedStrength` should match `lighting.warmth`
(normally 1.0); near the peak it should be positive when warmth is positive. Judge the
mild visible effect as well as the numeric state. Do not call a neutral daytime sample a
positive-warmth test: zero strength from 07:30 through 16:30 is expected.

At night, for example 21:00 or 00:00, confirm correct clock conversion and
`appliedStrength=0`, plausible native night appearance and active reference water when ON.
`lighting.active=true` is allowed at night: readable channels do not imply positive warmth.
Record how the time was reached. Sleeping or saving has game effects; simply preparing
this checklist has not performed or authorized an unrecorded time/save mutation.

## 6. Check menu and reload behavior

1. With ON established, capture before leaving Main, then in the main menu and after
   reloading the selected slot. In the menu, active resources should be released.
   `lookRequested`, `lighting.desiredEnabled` and `water.enabled` may remain **true**;
   they are requested state. Expect lighting inactive and water stopped/released.
2. After loading settles, expect the active state from step 3 again. Compare errors and
   visible results. This observes the transition; it does not prove every callback order.
3. Repeat the menu/reload roundtrip with OFF selected. Within the same process, OFF should
   remain requested and no visual effect should be reapplied.
4. If including a save/scene roundtrip, name the exact slot and transition and confirm save
   completion rather than treating a save request as completion. Mark unperformed cases
   individually **NOT RUN**; a menu reload does not stand in for every scene transition.

A full process restart is a separate optional check: startup reads the profile again;
session-only OFF is not expected to persist when the profile has `Enabled=true`. Preserve
the previous log first. Compare profile bytes after the session; toggling should not rewrite it.

## 7. Measure performance when the recorder is ready

**Prepared, not yet executable with a verified recorder.** The bounded local check on
2026-10-24 found no usable frame-time recorder in PATH, usual relevant installation paths
or running processes. A portable installation elsewhere may exist. Record its actual path,
version, per-frame metric and unit before measuring. Agree the target FPS/frame budget and
acceptable median/p95 regression with the owner; no percentage threshold has been accepted.

Use raw per-frame data from the same metric throughout. Present intervals are not GPU
execution time. F9 state, discovery milliseconds and an average FPS counter are not substitutes.

1. Keep the same session, fixed camera, scene and graphics settings. Warm up both states
   outside capture and wait out visible loading. Record clock/weather at each segment's ends.
2. Run **A1 OFF → B1 ON → B2 ON → A2 OFF**. A proposed protocol is ten seconds settling,
   then thirty seconds recorded per segment. B2 stays ON. Keep equal durations and retain
   separate raw files. Perform F9/screenshots/log copying outside measurement windows.
3. Repeat one full block under comparable conditions. Native time continues advancing;
   ABBA reduces drift but does not remove it. Changed weather or inconsistent OFF baselines
   makes the comparison **INCONCLUSIVE**, requiring a comparable repeat.
4. Report frame count/duration, median and p95 milliseconds, spikes and ON–OFF differences
   in ms and percent. Keep raw data and exclusions; do not derive percentiles from averaged FPS.

ON/OFF changes lighting and water together. This compares WorldLook's combined effect,
not isolated shader cost or a proven CPU/GPU cause. Until a recorder and acceptance budget
are established, keep quantitative performance **NOT RUN**; visual/lifecycle work can proceed.

## 8. Record the verdict and hand off

| Acceptance item | Result | Evidence / owner verdict |
|---|---|---|
| Direct-sun glints and overall appearance | NOT RUN | |
| Positive warmth | NOT RUN | |
| OFF restoration and ON-after-OFF | NOT RUN | |
| Native night appearance | NOT RUN | |
| Menu/reload while ON | NOT RUN | |
| Menu/reload while OFF | NOT RUN | |
| Named save/scene roundtrip | NOT RUN | |
| Comparable frame timings | NOT RUN — recorder and budget pending | |

Preserve evidence before another launch. Recheck installed/profile hashes and record any
differences without silently restoring unrelated state. A fault or persistent fallback means
stop that case, retain its log/status/images and diagnose; it is not an instruction to rebuild
or deploy. Update the canonical water-surface record and compatibility matrix only with actual
observations and attributed owner decisions. Untested cases remain explicit. Earlier owner
acceptance of calm morning motion and other mods stays closed; multiplayer is outside this run.
