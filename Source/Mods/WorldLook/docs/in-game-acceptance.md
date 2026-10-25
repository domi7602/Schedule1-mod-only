# WorldLook — acceptance status and remaining checks

## Current status — 2026-10-25

**Water and ON/OFF are accepted by Dominik and closed.** His request to mark water finished
supersedes the open water items in the original protocol below. The
[water-surface record](water-surface-verification.md#owner-acceptance-and-session--2026-10-24)
preserves the exact owner statements, candidate identity, screenshots, log and F9 hashes.

| Item | Current result and scope |
| --- | --- |
| Water appearance, including supplied sun view | Accepted by owner on dev.2, 2026-10-24 |
| Calm morning motion | Owner-confirmed 2026-10-23; remains closed |
| ON/OFF including re-enable | Owner-confirmed without errors, 2026-10-24; captured ON→OFF additionally has clean state reports |
| Startup ON | Expected from profile `Enabled=true`, owner-confirmed; session OFF is not persisted across a process restart |
| Positive warmth | dev.2 numeric strength 0.9045084971874737 at 17:42; dev.3 same-session MCP ON/OFF/ON at 18:00 shows repeatable controller-Sun red +6% / blue -6%; visible warmth is not separately accepted |
| Native night | Owner-confirmed normal appearance without unusual coloration on dev.3, 2026-10-25, after MCP set time to 21:00 |
| Menu/reload | dev.5, 2026-10-25: two ON reloads and two OFF reloads followed by ON passed in Dominik; actual renderer shader/support readbacks confirm the reference-water reload fix |
| Automated control/screenshots | dev.4 control/idempotence evidence retained; dev.5 adds four completed reloads, eight renderer samples and ten agent-captured game/menu screenshots |
| Comparable frame timings | Not measured; recorder and acceptance budget still pending |

The dev.3 cleanup removes F9 and normal diagnostic chatter. **Do not request fresh F9 reports
from dev.3** or reinterpret retained dev.2 `status.json` files as current state. Keep the old
reports and backups as evidence. The water shader/recipe and F7 behavior are unchanged;
the cleanup build was installed and hash-verified on 2026-10-24, with no new gameplay acceptance.

For remaining WorldLook work, use labelled visual observations and `MelonLoader/Latest.log`
with the actual candidate/slot/time. The scoped night/menu/reload results below do not establish
visual warmth or frame-time acceptance. Do not add hidden debug output again or reopen accepted water
merely to fill the original protocol. Measured performance requires a verified recorder and
an agreed budget; a screenshot, log timestamp or average FPS is not that measurement.

### dev.3 MCP session — 2026-10-25

Installed DLL `cc9a206a2e13b3824ff25e7d8d36e4d73114eefae9beff78cfc9b1bd610ae356`,
game v0.4.7f12, S1MCP 1.0.5, process 18124, owner-selected `Dominik` / `SaveGame_1`.
Local raw evidence is preserved under `.scratch/WorldLook/mcp-lightcheck-20261025/`:
`07-sun-comparison.json`, `09-night-owner-acceptance.json`, `20-reload-on-result.json`
and `25-automation-result.json`, with correlated requests, logs and screenshot hashes.
These optional local files are not tracked fixtures. The clock was changed with the owner's
explicit authorization; native-save backups and the unchanged startup profile were retained.

The ON→Menu→reload sequence used one confirmed `load_save` request, then independently
confirmed `Main`, `IsGameLoaded=true`, `IsLoading=false`, the exact save path/name and the
same process. The Sun color differed by less than 0.09% from the pre-menu ON sample near
18:00. This establishes that measured light behavior, not every render channel or resource
cleanup. Menu loaded three times during the native return sequence; that observation remains
part of the record.

At 08:55:39.731 local time, the reload logged `reference shader unavailable or unsupported`.
Subsequent renderer readback returned `Stylized Water 2/Standard (Tessellation)` instead of
`S1Mods/ReferenceWater`. The reference-shader reload path therefore remains a separate
defect; earlier owner acceptance of the water appearance stays closed. A stale shader cache
is a source-supported hypothesis, not a proven native unload cause.

The owner requested direct MCP control after synthetic Windows hotkeys failed to produce
an ON/OFF log entry. Screenshot capture and time/readback already work; see the
[automation contract](mcp-automation.md) for deterministic commands and evidence limits.

### dev.4 automated MCP session — 2026-10-25

Installed DLL SHA-256 `5522f394523dbaaba7eab675bb579df8523892b7db7890d7baed5201640949e4`;
game v0.4.7f12, S1MCP 1.0.5, process **17524**, started **08:18:29.2887241 UTC** through Steam.
All loads resolved the owner-selected `Dominik` / `SaveGame_1` from fresh save metadata.
The endpoint registered in IL2CPP and its parameterless methods returned JSON through the
existing bridge. No S1MCP binary or shader asset changed.

| Check | Observed result |
| --- | --- |
| Startup/status in Menu | ON from the unchanged profile; both modules initialized; status readable before a save is loaded |
| Explicit ON/OFF | Real ON→OFF and OFF→ON setter responses have `changed=true`; repeated ON and OFF have `changed=false`, with separate status readbacks |
| OFF→Menu→reload | One `load_save`, complete load of Dominik in the same process; OFF remains OFF, water `Stopped`, native shader retained |
| ON→Menu→reload | One `load_save`, complete load of Dominik in the same process; ON remains ON and measured light tint is retained |
| Images | Seven unedited game/menu captures with hashes and associated requests; no owner screenshot upload needed |
| Reference water after reload | **FAIL:** ON reports `referenceWaterReady=false` and the renderer uses the native shader; the prior reload defect is reproduced |

At matched **18:05–18:08** sampling windows before the first menu transition, Sun RGB was
ON `(0.73883194, 0.6216188, 0.5267627)` and OFF `(0.69736606, 0.62152785, 0.5598719)`:
approximately **+5.95% red / −5.91% blue**, with green nearly unchanged. The renderer changed
from `S1Mods/ReferenceWater` to `Stylized Water 2/Standard (Tessellation)` on OFF.
The maximum absolute RGB drift after OFF reload was **0.0005792** (18:04–18:07); after ON
reload it was **0.00001741** relative to the preceding ON sample. This verifies the measured
light channel and state preservation, not subjective warmth, every lighting channel or performance.

The reference-water warning appeared at **09:24:18.486** and **09:25:39.368 local time**.
Both subsequent ON readbacks showed native fallback despite `waterState=Applied` and false
coordinator fault flags. The automation correctly exposes this distinction; no shader fix is
included in dev.4. Each native menu transition again emitted three Menu-loaded callbacks;
the record therefore does not assert universally clean resource lifecycle behavior.

The first load was accepted but its subsequent status query timed out during a long loading
frame. A later independent query confirmed readiness; the load command was never repeated.
The driver was then hardened to retry **only** confirmed transport timeouts on read-only
status polling within the original deadline. Both complete OFF/ON reload runs finished
without a timeout. The initial failure and resulting bridge send error are preserved.

Local evidence is under `.scratch/WorldLook/mcp-control-20261025/`: `acceptance-result.json`,
action/readback JSONL files (notably `12-reload-off.jsonl` and `20-reload-on.jsonl`),
`Latest-after-test.log`, deployment receipt, installed identities and `screenshots/manifest.jsonl`.
These are optional local artifacts, not tracked CI fixtures. The first image's time is its
save time as explicitly corrected in the manifest; subsequent images have capture intervals.
The HUD/camera are visible, but moving traffic and small clock differences preclude exact-frame
pixel claims. A shifted Minimap after reload is recorded separately; no Minimap change is included.

The game was quit through its menu after testing. All **44 backed-up native save files** and
the WorldLook profile remained byte-identical. Profile SHA-256
`73b8e6dc25029a817bdcddac5487a1a8fc254597f43d57a1745cf07a559a1d65`; startup remains ON.
The game-closed backup and earlier dev.2/dev.3 evidence are retained. Release build and Full
verification passed **21/21** before deployment, including **41 WorldLook tests**; the later
read-only timeout improvement passed **81/81** driver checks and an independent code review.

### dev.5 shader-cache repair and automated reloads — 2026-10-25

**The separately observed reference-water reload defect is resolved in this tested session.**
Installed DLL SHA-256 `3f46ae888db9b7dcc61db7a4c83a211274c849451056dae08292a730daebd8c9`,
embedded bundle `29557b87609c367e3b568d84d10400d67c980581a1e3491fbd455def9794c8ef`
(unchanged from accepted dev.2). Game v0.4.7f12 / Unity 2022.3.62f2, S1MCP 1.0.5,
process **12360**, started through Steam at **08:52:01.9945477 UTC**. Each load freshly
resolved `Dominik` / `SaveGame_1`; all four roundtrips stayed in that same singleplayer process.

The cache now checks native shader/bundle validity and shader support before reuse. It retains
its owned shader with `DontUnloadUnusedAsset`, preserves other flags, and permits a new bounded
acquisition on the next explicit initialization after failure. There is no per-frame reload loop.
Shutdown clears only the retention bit this owner added and still uses `Unload(false)`.
Unity documents [the retention flag](https://docs.unity3d.com/2022.3/Documentation/ScriptReference/HideFlags.DontUnloadUnusedAsset.html)
and [non-destructive bundle unloading](https://docs.unity3d.com/2022.3/Documentation/ScriptReference/AssetBundle.Unload.html).
The unchecked stale-cache path was present in the source; the exact native unload trigger in
the earlier sessions remains unproven. Shader code, bundle and water recipe were not changed.

| Automated check | Result and independent readback |
| --- | --- |
| ON→Menu→reload, twice | Requested ON preserved; water `Applied`, `referenceWaterReady=true`; renderer shader `S1Mods/ReferenceWater`, `isSupported=true` |
| OFF→Menu→reload, twice | Requested OFF preserved; water `Stopped`, `referenceWaterReady=false`; renderer uses `Stylized Water 2/Standard (Tessellation)`, supported |
| ON after each OFF reload | Both re-enables restore the supported `S1Mods/ReferenceWater`; neither falls back to native water |
| Final repeated ON | Idempotent acknowledgement (`changed=false`), followed by independent state readback |
| Log and exit | No reference-shader warning in this process; final Menu reached and game quit through its native UI |
| Saved data | All 44 backed-up native-save files byte-identical, no new files in that slot; WorldLook profile unchanged |

The eight renderer samples read **`sharedMaterial`**, never the allocating `material` getter.
Ten unedited screenshots include Main and Menu, with actual capture intervals, process identity,
associated request files and SHA-256 hashes. They document the saved street viewpoint and HUD,
not a fresh close-up water appearance assessment. No water-appearance acceptance is inferred
from those screenshots; the owner's earlier acceptance remains closed. Sun readings are retained
as context, with differing clock/capture times, not as a new exact-frame visual comparison.

The initial cold load was accepted once. Its read-only status polling hit a long loading-frame
timeout; a later response carried the old request ID, which the driver rejected. A fresh query
then independently confirmed ready Main/Dominik in the pinned process. No load mutation was
repeated. **All four subsequent reload commands completed successfully**, each with exactly one
`load_save` request and independent renderer verification. The initial transport failure remains
in the evidence; this run does not claim that cold-load polling is free of bridge timing issues.
The previously observed three Menu callbacks per native transition and shifted Minimap after
reload remain separate observations, outside this cache repair.

Local evidence: `.scratch/WorldLook/reload-fix-20261025/acceptance-result.json`, the numbered
JSONL files (reloads `07`, `13`, `19`, `24`; renderer samples `05`, `09`, `11`, `15`, `17`, `21`,
`26`, `29`), `Latest-after-test.log`, installed/embedded identities, deployment receipts and
`screenshots/manifest.jsonl`. Backup: `.backups/WorldLook/reload-fix-dev5-20261025-07db56f6/`
with a hash manifest of the prior installation, profile, log and native save. These are optional
local artifacts, not tracked CI fixtures. Profile SHA-256 remains
`73b8e6dc25029a817bdcddac5487a1a8fc254597f43d57a1745cf07a559a1d65`.

Before installation, Full verification passed **21/21** including **1,183 water tests**
(16 new cache cases), **41 WorldLook tests** and **81 MCP-driver checks**. Independent cache
review found no blocking issue. The native-free cache tests execute production cache code
against explicit Unity boundary doubles; actual Unity lifetime behavior is evidenced only by
the scoped live checks above. Visible warmth and measured performance remain separate open
WorldLook acceptance items; multiplayer remains out of scope.

## Historical dev.2 protocol — prepared before the owner session

The following protocol and original blank checklist are retained to interpret the archived
dev.2 reports. They are **not the current backlog** and their F9 commands apply only to the
older dev.2 diagnostic build. Current results above supersede their original blank
statuses. The [compatibility matrix](../../../../docs/compatibility.md#verification-matrix)
owns the repository-wide summary.

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
