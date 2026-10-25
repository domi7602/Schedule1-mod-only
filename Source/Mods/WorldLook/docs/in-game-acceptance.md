# WorldLook — owner acceptance and evidence

## Current status — 2026-10-25

**WorldLook is complete — Verified, owner-confirmed 2026-10-25.** Dominik clarified:
**"wordlook haben wir letzte session fertig gemacht"**. This records overall acceptance of
the completed work, currently version `0.3.0-dev.5`. The exact acceptance time and individual
actions from that prior session were not supplied. There is no remaining WorldLook acceptance
backlog; unitemized checks and unmeasured frame times remain evidence limits.

**Water and ON/OFF are accepted by Dominik and closed.** His request to mark water finished
supersedes the open water items in the historical protocol linked below. The
[water-surface record](water-surface-verification.md#owner-acceptance-and-session--2026-10-24)
preserves the exact owner statements, candidate identity, screenshots, log and F9 hashes.

| Item | Current result and scope |
| --- | --- |
| Overall WorldLook | Completed; Verified, owner-confirmed 2026-10-25, referring to the prior session without itemized actions |
| Water appearance, including supplied sun view | Accepted by owner on dev.2, 2026-10-24 |
| Calm morning motion | Owner-confirmed 2026-10-23; remains closed |
| ON/OFF including re-enable | Owner-confirmed without errors, 2026-10-24; captured ON→OFF additionally has clean state reports |
| Startup ON | Expected from profile `Enabled=true`, owner-confirmed; session OFF is not persisted across a process restart |
| Positive warmth | dev.2 numeric strength 0.9045084971874737 at 17:42; dev.3 same-session MCP ON/OFF/ON at 18:00 shows repeatable controller-Sun red +6% / blue -6%; the overall owner confirmation did not itemize a separate visible-warmth test |
| Native night | Owner-confirmed normal appearance without unusual coloration on dev.3, 2026-10-25, after MCP set time to 21:00 |
| Menu/reload | dev.5, 2026-10-25: two ON reloads and two OFF reloads followed by ON passed in Dominik; actual renderer shader/support readbacks confirm the reference-water reload fix |
| Automated control/screenshots | dev.4 control/idempotence evidence retained; dev.5 adds four completed reloads, eight renderer samples and ten agent-captured game/menu screenshots |
| Comparable frame timings | Not measured; no quantitative performance result or accepted frame-time budget is claimed |

The dev.3 cleanup removes F9 and normal diagnostic chatter. **Do not request fresh F9 reports
from dev.3** or reinterpret retained dev.2 `status.json` files as current state. Keep the old
reports and backups as evidence. The water shader/recipe and F7 behavior are unchanged;
the cleanup build was installed and hash-verified on 2026-10-24, with no new gameplay acceptance.

The dated observations below retain their original candidate, slot and session scope.
Overall owner acceptance does not turn unitemized visual checks into separately observed
passes or provide frame-time measurements. Preserve the logs, screenshots and earlier failure
records; their evidence limits do not reopen completed work. The old protocol is historical,
and multiplayer remains out of scope.

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
the scoped live checks above. At this checkpoint, visible warmth and measured performance had
no separate itemized acceptance. The later owner confirmation above closes overall acceptance
without adding frame-time measurements; multiplayer remains out of scope.

## Historical dev.2 protocol — prepared before the owner session

<a id="1-establish-the-candidate-and-session"></a>
<a id="2-capture-evidence-without-losing-the-preceding-step"></a>
<a id="3-verify-the-active-starting-state"></a>
<a id="4-compare-direct-sunlight-and-exercise-off--on"></a>
<a id="5-observe-positive-warmth-and-night"></a>
<a id="6-check-menu-and-reload-behavior"></a>
<a id="7-measure-performance-when-the-recorder-is-ready"></a>
<a id="8-record-the-verdict-and-hand-off"></a>

The [original dev.2 protocol and blank checklist](history/in-game-acceptance-dev2-20261024.md)
are preserved with their candidate hashes, dates and evidence limits. Their F9 instructions
apply only to the older diagnostic build; the current results above supersede the original
blank statuses. This historical protocol is **not the current backlog**.
