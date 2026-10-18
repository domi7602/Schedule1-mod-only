# GraphicsProbe

**Experimental rendering diagnostics, v0.1.3. In-Game-Verify: 0.1.3 evidence slice verified in-game 2026-10-16 on v0.4.7f12 (all 12 SkyMat shader properties, sun details, time anchor — two captures at 15h34m and 23h41m in-game); 0.1.2 core acceptance observed 2026-10-16; edge lifecycle checks open.** No water shader, scene additions or rendering changes. Pure tests/compilation do not verify Unity runtime behavior. The repaired traversal was verified in a real session (8 cameras, surface separation, prior-report backup, scene-transition cancellation); graceful shutdown with pending work, post-cooldown repeat with input suppression, menu round-trip and IO-failure retry remain open — see `verification.md`.

## Capture

- Automatic requests at initialization and after scene load, unload and initialization callbacks.
- Manual: hold **Left or Right Ctrl + Left or Right Shift, then press F8** while the game has focus. A selected standard uGUI `InputField` or TMP `TMP_InputField` suppresses the chord. Custom text/console widgets may not be recognized; no typing state is changed.
- Requests wait **3 seconds**, coalesce fixed reason labels without moving the first deadline, and respect a **5-second cooldown after completion**, successful or failed. No periodic scanning or automatic retry loop. Request another capture after failure.
- Only active requested captures discover objects: main-thread collection advances over frames with a **2 ms cooperative slice target**, **128 steps/update**, a **1,500 ms active-work budget** and a **30-second wall deadline**. A native call can overrun a slice; there is no hard frame-time guarantee.
- Scene transitions and deinitialization cancel/discard collection, release retained arrays/queues and preserve the previous report before rescheduling. No overlapping captures; budget-limited collection may write an explicitly incomplete report.
- Reports are **multi-frame observations, not atomic frozen-frame snapshots**. Objects/settings may change without scene events. Serialization/write/readback remain synchronous and can hitch; their cost is logged separately.

Reports use `S1Mods.Shared.SafeStorage.SaveTextAtomic`:

```text
<Game>/UserData/GraphicsProbe/latest.json
<Game>/UserData/GraphicsProbe/latest.json.bak
```

Default installation report:

```text
C:\Program Files (x86)\Steam\steamapps\common\Schedule I\UserData\GraphicsProbe\latest.json
```

SafeStorage attempts to back up an existing report before replacement and logs backup failures; its transient sibling is `latest.json.tmp`. There is no history or save-slot file. Success logs counts, truncation and path after exact file readback. Capture/serialization/IO failures log an error and release the schedule for retry. Oversized JSON is refused before touching the existing report.

## Schema version 2

- Capture reason, start/end UTC timestamps, scope and caps. Collection timing separates active/wall duration, work steps/frames and maximum observed step. Synchronous serialization/write/readback and total wall duration are logged separately.
- Graphics device/name/vendor/API/version, shader level/memory; Unity/game application versions; current screen/display resolution, full-screen mode, color space, quality index/name, quality MSAA and VSync.
- Existing runtime pipeline native type (direct field read), plus observed current, per-quality override and default pipeline asset names/native types. URP serialized depth/opaque/HDR/MSAA/render-scale/shadow/batching configuration and capped renderer-data name/type descriptors. Renderer/pass resolution is intentionally unresolved.
- One early `Camera.main` tagged-enabled hint, deduplicated by instance ID, supplements scene traversal. `discoverySource` labels the hint versus traversal; neither proves gameplay-camera identity. Cameras on discovered loaded-scene objects, including disabled cameras and inactive descendants: scene, indexed hierarchy path, activation, enabled state, native component names, depth, culling mask, clear mode, target-texture descriptor, HDR/MSAA/depth requests, projection basics and pixel size. URP additional-camera serialized fields include depth/opaque override requests, render type, renderer index, post-processing/AA, volume mask and stack count. Requests do not prove actual buffer production.
- Renderer inventory distinguishes encountered native types, heuristic scan attempts, budget omissions, read failures and incomplete material reads. Independent scan allowances sum to 20,000: surface-like 19,488; particle/line/trail 512. **`unconfirmedWaterCandidates`** retains non-particle/line/trail lexical matches only; **`unconfirmedParticleLineTrailMatches`** is a separately capped diagnostic section, not surface water. Non-particle type is not water proof. Renderer/material/shader names retain source/term evidence: `water`, `ocean`, `river`, `lake`, `pond`, `canal`; `sea` as a separate token. Generic transparency or blue appearance does not qualify. False positives (water-related props) and false negatives remain possible.
- Candidate renderer scene/path/type/layer/activation/enabled/bounds, shared-material slots/name/shader/render queue, shared-mesh name/vertex count/submesh count/bounds when available, renderer and immediate-parent component types. No geometry/pixels/assets extracted.
- Lights, reflection/light-probe-group descriptors, skybox shared material, fog/ambient/sun settings and Volume descriptors. Only shared Volume profile component types/active flags are read, not instanced profiles/evaluated runtime blends or bulk probe positions/SH.

Unavailable scene counts remain `totalObserved=null`, `available=false`, `truncated=true`; a successfully observed empty scene set is available, 0 and not truncated. Root admission is sequential per scene; the quotas protect memory capacity, not admission-time fairness. A budget exit during seeding can precede any root visits.

Lists expose scanned/matched/kept/failed/limit-omission counts as relevant. Candidate `matched` means lexical match, never confirmed water. Nested lists include observed totals and truncation. `fieldErrors` preserves individual getter failures without dropping sibling fields; discovery errors have bounded samples and mark discovery incomplete. Non-finite float/double values serialize as JSON `null`.

## Bounds and limitations

Discovery covers **SceneManager-enumerable loaded scenes**, including inactive descendants. Roots have bounded per-scene admission shares (unused shares are not redistributed); admitted roots are visited round-robin across scenes before descendants. At each breadth level parent cursors rotate after one child attempt. Reports expose root admission/visit attempts, known omissions and pending work; unknown descendants behind omitted roots/edges are not invented as known counts. The special DontDestroyOnLoad scene may not be enumerable: persistent objects outside these scenes are out of scope **except the single Camera.main hint**. This is **not a claim to inventory every loaded Unity object**. Caps/errors/time limits may omit cameras or other objects; inspect all counters before drawing conclusions.

| Limit | Cap |
|---|---:|
| Scenes / roots per scene | 64 / 4,096 |
| Retained admitted roots / frontier cursors | 4,096 / 4,096 |
| Hierarchy attempts / levels / children per node | 50,000 / 64 / 4,096 |
| Components classified per object | 128 |
| Surface / particle-line-trail heuristic scans | 19,488 / 512 |
| Camera / surface candidate / effect match records | 256 / 128 / 32 |
| Light / probe / Volume records | 256 / 128 / 128 |
| Component-name/profile-component descriptors | 32 |
| Material slots / renderer-data entries | 16 / 16 |
| Renderer-type keys | 32 plus overflow bucket |
| Discovery error samples | 64 |
| Text / path prefixes | 256 / 2,048 UTF-16 code units plus cut marker |
| JSON UTF-8 bytes | 4 MiB |

Text cuts do not split surrogate pairs. The **1,500 ms cooperative discovery budget** cannot interrupt an individual native call or guarantee a maximum frame stall. A capture can hitch; runtime performance is unmeasured. Unity returns per-scene root, per-object component and per-renderer shared-material arrays before item caps apply; no whole-world component arrays or global Resources discovery are used.

No Harmony patches, S1API calls, network/gameplay authority requirement, save-data changes, object/component creation/removal, rendering setters, render-texture allocation, material instantiation, pixel reads or extracted assets. Lazy URP renderer/stack resolution and lazy Volume profile accessors are not invoked. Observed camera settings do not identify the main/gameplay camera, actual water, active passes or available GPU depth/opaque buffers.

## Verify and remove

See `verification.md` for historical native evidence and the repaired gate, and `api-notes.md` for installed signatures. Version 0.1.1 loaded and wrote manual reports/backup; it confirmed active URP/URP_3Ultra but missed cameras and retained only particle false positives after exhausting the node cap. That evidence does **not** verify 0.1.2. Required new acceptance:

1. Launch normally via Steam, confirm **GraphicsProbe 0.1.2** in `Latest.log`, load the save and let scene loading settle.
2. Near water, close text inputs, press **Ctrl+Shift+F8**, then allow **up to 35 seconds** for the request delay and bounded collection. Check the final success/error and timing lines; the mere presence of `latest.json` may be an old report.
3. Confirm fresh schema/version/start/end times, manual reason, scene/root admissions and visits, camera provenance and independent surface/effect sections. Record truncation/errors/budget exits and active/max-step/finalization timing. Do not promote lexical matches or depth requests to confirmed water/buffer availability.
4. After cooldown, repeat once and inspect the prior `.bak`. Check selected uGUI/TMP input and unfocused suppression; scene-transition cancellation, shutdown with pending work and manual retry after IO failure remain native acceptance items. Pure policies, source contracts and managed collector-adapter tests do not execute the MelonLoader lifecycle or real Unity getters.

With the game closed, remove/disable `Mods/GraphicsProbe.dll` and its optional diagnostic PDB; optionally remove its owned `UserData/GraphicsProbe/` report/backup/tmp. Preserve any rollback backups you still need, Shared and all other mods. There are no owned game objects, shaders or save-state changes to undo. No deployment or in-game test was performed by the initial 0.1.0 implementation task. The parent subsequently reviewed and deployed 0.1.1; the 2026-10-16 real-game inspection established loading/report IO and active URP, but exposed incomplete gameplay discovery. It is partial runtime evidence, not full acceptance.
