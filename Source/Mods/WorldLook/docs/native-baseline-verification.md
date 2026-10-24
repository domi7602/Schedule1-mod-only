# WorldLook native-sky baseline — verification record

This record describes **0.3.0-dev.1** and its observed runtime. The subsequently approved **0.3.0-dev.2 water-surface refinement** has its own [verification record](water-surface-verification.md); these results do not automatically verify that candidate.

**Installed candidate: 0.3.0-dev.1, 2026-10-22.** Full Unity/C# build passed, 1,293 focused tests passed, independent final delta review PASS, installed artifacts/profile read back and verified. **First candidate runtime observed 2026-10-23:** startup/water application and one OFF transition are recorded below. **In-Game-Verify open** for full visual and lifecycle acceptance.

## Approved outcome

Canonical repository build: native sky/clouds/celestial bodies, mild GoldenHour color adjustment with corrected HHMM input, current clear water driven by native environment values, one F7 session toggle and a small F9 status. GraphicsProbe and the custom sky prototype are archived.

## Preserved starting points

- Local backup: `.backups/WorldLook/native-baseline-20261022-171619/manifest.json`, **216 files copied and SHA-256 verified** before implementation. Contains repository sources/tests/docs, standalone 0.2.0 sources/build inputs/artifacts and installed WorldLook/config state.
- Previous installed DLL: `aebd2b81a0afe143f95e8e54c9e8d06caca30fb52f810da048f3d9f910932515` (0.2.0 prototype).
- Previous water-on profile: `b2699b0245318c91612d86064483af7c005a03e5151f2f476809b87f9d9cff33`.
- `Source/Archive/WorldLook/Standalone-0.2.0/`: preserved source/build-input reference; original external working copy retained.
- `Source/Archive/WorldLook/GraphicsProbe/` and `Source/Archive/Tests/GraphicsProbe.Tests/`: retired full-scene diagnostic sources/tests, outside the active solution.
- Mild light recipe from the historical owner-accepted 0.1.x look: sun `(1.06,1,.94)`, sky sun glow `(1.04,.97,.90)`, ambient `(1.03,.995,.96)`. Historical acceptance did not verify the incorrect clock conversion; the HHMM error existed in both the earlier GoldenHour time filter and the later standalone sky clock.

## Implementation evidence

- Correct HHMM validation/conversion and clock-weighted mild light. Five observed native color channels; no URP volume-profile instantiation, fog enabling or skybox replacement.
- Separate desired/active lighting state; native color drift and setter readback are tracked. Pending restoration retains the original target and is bounded. Readiness, destroyed targets and partial failures are covered by native-free tests.
- Environment-only water: no reflection camera, render texture, render callback or global reflection uniforms. Native SkyState colors with finite ambient fallback and native weather wind; original material/owned clone lifetime retained.
- Water shader source ported byte-identically from the installed prototype: SHA-256 `2ce7e343c100f2f9d6f93aef3380039d5ee34d2787b283dc1dde6d46ac952a47`. The shader builder is restricted to this asset and its variant collection.
- Runtime uses repository Shared services. Existing JSON is read without automatic migration/overwrite; a corrupt/unreadable profile disables this session's look. New `Warmth` does not reinterpret the old cloud-light setting.

## Final automated checks and review

- Parent rerun, lighting: **104 passed**, 0 failed/skipped; two coder-observed RED/GREEN regressions for native drift and setter rounding. Native IL2CPP behavior is not exercised by these tests.
- Parent rerun, water: **1,166 passed**, 0 failed/skipped; own material/session and shader-assignment policies plus limited source contracts.
- Parent settings suite: **23 passed**, 0 failed/skipped; JSON compatibility, bounds, absent/corrupt/unreadable inputs and write boundaries. TRX files under the local backup's `checks/` directory. Total **1,293**; these are not native/GPU visual tests.
- Initial independent runtime review: **FAIL**, one P2. F7 OFF stopped the update pump before a pending water restoration could retry. Five new regressions were observed failing before repair. The fix permits restore-only disabled updates, at most three retries five seconds apart, with no discovery/allocation/reapplication while disabled. Successful cleanup becomes Stopped; exhaustion retains ownership/error for explicit recovery.
- Independent final delta review: **PASS**. The reviewer checked the repaired ordinary host update path and the finished shader/build/provenance/deployment contracts. Reviewer performed no native game test or build.
- Full solution `dotnet format --verify-no-changes --no-restore`: exit 0. Version-sync: 21 mods, 0 drift. Doc paths: pass. `git diff --check`: pass (line-ending notices for existing working files are not whitespace failures). Solution regeneration was byte-identical, **39 projects**; GraphicsProbe.Tests retired and WorldLook.Tests included.
- Active hand-maintained production input count: **13 C# files + one project = 14**. No standalone support copy or custom sky/full probe is compiled.

## Build, deployment and runtime

Full build: `build/logs/20261022-175611-837-10c3ef00/`, via the canonical `build/Build.ps1`. Unity editor **2022.3.62f3**, SDK **8.0.425**, net6.0, installed S1API FileVersion **3.2.1.0** (SHA-256 `79d7af288b605f8fcd4d2582a59636670b6cafd6a8847bc31c21b2278a9f2bf3`). C# **0 warnings / 0 errors**. Both restore/build used `S1NoDeploy=true`. All **42 recorded source/build inputs** matched after compilation; actual embedded resource bytes matched the freshly generated water-only bundle. One shader, 17 properties, eight requested/present SVC descriptors; this is not proof that all eight GPU combinations execute. Existing four-variant compiler filtering is documented in `build/verification.md`.

| Installed/build artifact | SHA-256 |
| --- | --- |
| `WorldLook.dll` | `79721251dd863862b027d08d4064b47a197dd8a9527c98f84bf43f6457391c27` |
| Embedded water bundle | `24748b573273914b6a21b170d698d80ace7a6c7e0da153623353bf901a892391` |
| `WorldLook.pdb` | `4f75d54010253d70e641f57c82deeabd1741db3338ff46ced65ba42f8b2e05e8` |
| Installed `mod.json` | `5044d095763568d6e2164f1761d13cdfc3439f0aaee7ed1b6c97877511c77cee` |
| Installed four-setting profile | `73b8e6dc25029a817bdcddac5487a1a8fc254597f43d57a1745cf07a559a1d65` |

Deployment used `Tools/deploy-mod.ps1`, after enumerating exactly DLL/PDB/metadata and verifying the game was closed. Receipt: `C:\Program Files (x86)\Steam\steamapps\common\Schedule I\UserData\.deploy\receipts\WorldLook.20261022T180442038Z-df2af53500de.json`. Previous artifacts backed up under `UserData/.deploy/backups/20261022T180442082Z-WorldLook-28c2c19b/` in the game install. The prior profile is separately hash-verified in the pre-task local backup.

The canonical profile now has `Enabled=true`, `LightingEnabled=true`, `WaterEnabled=true`, `Warmth=1.0` (full strength of the **mild** recipe). Post-install comparison checked **34 existing game files**: only WorldLook DLL/PDB/metadata/profile changed; 30 others matched. Shared built and installed hashes are identical (`087b8768…`). The old external `Mods/s1reference.bundle` is reported as stale by the helper and is not read by this embedded-resource candidate; it was not deleted. Machine-readable final check: `.backups/WorldLook/native-baseline-20261022-171619/installed-candidate-verification.json`.

At the end of the 2026-10-22 installation, no game launch, time/weather mutation, save load or visual acceptance had been performed for the new candidate. The then-existing Latest.log/F9 files belonged to the prior prototype. The following owner-run evidence is newer.

## First candidate run — 2026-10-23

Read-only follow-up after the owner supplied two screenshots. By collection time the game process had exited, so no live MCP request was sent. Preserved evidence: `.backups/WorldLook/native-baseline-20261022-171619/runtime-20261023-033632/` (`Latest.log`, `status.json`, `WorldLook.json`, `identity.json`). Screenshot observations below refer to chat attachments; their original image files were not exported to this folder.

- The log reports game **0.4.7f12** and WorldLook **0.3.0-dev.1**. Its loaded DLL hash is the installed `79721251…` (lines 119–120); a fresh on-disk hash matches. Startup profile hash remains `73b8e6dc…`.
- At **05:32:38 local**, the embedded-memory loader reports bundle `24748b57…` and supported `S1Mods/ReferenceWater`. At **05:33:00**, the exact ocean material is prepared/applied with the intended `.10/.28/.22/.12` clarity values and shallow tint `.46/.52/.55` (lines 393–396). This is runtime application evidence, not a current-frame GPU texture readback.
- At **05:33:58**, the log reports water `Stopped`, material `released`, then global OFF with both module fault flags false (lines 533–534). This captured run contains one OFF request and no subsequent ON request.
- F9 at **03:34:29 UTC / 05:34:29 local** reports `lookRequested=false`, `mainLoaded=true`, native `SkyMat / Custom/ScheduleOneSky`, both module fault flags false, lighting `pendingRestores=0`/`errors=0`, water `Stopped`/`released` with no last error. OFF clears the lighting clock/channel snapshot by design, so its null time fields do not diagnose a clock failure or prove an active HHMM read. No WorldLook error/exception is present in this captured log.
- Screenshot 1: **Wednesday 07:25**, underwater pilings/cliff profiles and green plants are more visible; the surface appears very smooth and subdued. Screenshot 2: **Wednesday 07:44**, more pronounced blue-gray surface/reflection patterns, less visible submerged detail. Both views are very dark. Startup-ON then logged OFF is consistent with supplied AN/AUS order, but the owner did not label either image and no screenshot wall-clock metadata was captured. The 19 in-game minutes and slightly changed camera pitch prevent an equal-time brightness comparison.
- Other mods remain present, including Greenhouse Island, whose log reports an ocean patch. WorldLook OFF is therefore a WorldLook-disabled comparison, not a fully unmodded-game baseline; this observation does not identify another mod as a cause.

**Acceptance at this checkpoint:** correct candidate startup, reference-water application and one clean reported OFF/release are observed. Owner visual acceptance, active-light HHMM/channel readback, ON-after-OFF, daylight/night appearance and menu/reload roundtrip remain open. A noon comparison and an F9 snapshot while ON were requested; the subsequent evidence follows below. No source, shader, installed profile or game state was changed during this follow-up.

## Midday comparison — 2026-10-23

The owner supplied a second pair: **Wednesday 13:55 / 14:08**. Read-only evidence is preserved under `.backups/WorldLook/native-baseline-20261022-171619/runtime-20261023-034923/` with the same four capture files. The game had exited before inspection; no live MCP request was sent. Image originals remain chat attachments rather than local screenshot backups.

- This is a fresh game process/run: the log starts at 05:46:34 local, loads the same DLL `79721251…`, loads bundle `24748b57…`, and applies the reference-water recipe at 05:46:55. It reports one OFF/material-release at **05:47:58.041**, then F9 at **05:48:20.449**. There is no subsequent ON request or reported WorldLook exception in this capture.
- F9 is again **OFF**: `lookRequested=false`, native `SkyMat / Custom/ScheduleOneSky`, both module fault flags false, water `Stopped`/`released`, lighting `pendingRestores=0`/`errors=0`. Active HHMM conversion and readable lighting-channel counts remain unobserved. The null OFF-time fields are expected cleanup state.
- At **13:55**, submerged pilings, rock faces, bottom and plants are visible. A conspicuous soft, elongated reflection/highlight patch on the right is broken into broad, roughly parallel bands; most of the remaining surface appears smooth. At **14:08**, the surface is much more distinct, with scattered sharp bright glints and stronger wave distortion, while submerged detail is less visible. Both images are substantially more legible than the earlier morning pair and retain a blue-gray cast. The owner did not explicitly label ON/OFF; supplied order remains consistent with startup-ON then the single logged OFF. The 13-minute gap and changed view prevent exact photometric comparison.
- Source inspection identifies a plausible surface-detail cause, not a confirmed GPU diagnosis: `WaveGradient` in `ReferenceWater.shader` combines four periodic directional cosine slopes with slow domain warp; those slopes directly supply both refraction and the sun-highlight normal. Shader defaults imply nominal wavelengths of roughly 0.6–4 m before warp. The `pow(N·H, _SunSharpness)` term uses default sharpness 220. Actual live values for these unwritten shader properties were not captured. The environment reflection interpolates two native colors and does not reproduce scene detail.
- The existing `ShaderBendsTheWaveFieldSoItDoesNotReadAsAComb` test checks source tokens/directions, not the rendered absence of bands. Its passing result cannot accept this visible surface pattern. The lighting policy is neutral at both screenshot times by source design; an active runtime light snapshot is still missing.

**Recommendation, awaiting owner approval:** focused water-normal/sun-highlight refinement to obtain smaller, less regular glints while preserving the current clarity recipe. Retain distance filtering and check motion for shimmer; use the existing water tests, shader/full build, independent review and a backed-up candidate installation. This is a proposed visual correction, not an implemented change or owner rejection/acceptance of the current appearance. Findings and captures are the only changes from this inspection.

Manual acceptance after installation: same harbor view at noon and early evening; native night sky; F7 off/on including water; return to menu and reload once. F9 and scoped log/MCP reads support the state check. Screenshots/owner observations establish visual acceptance. Full scene probes are retired.
