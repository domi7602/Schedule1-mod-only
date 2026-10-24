# WorldLook water-surface refinement — 0.3.0-dev.2

**Water accepted by Dominik on 2026-10-24; closed.** ON/OFF is also owner-confirmed without errors. This acceptance concerns the dev.2 water candidate used in that session (DLL `410fecf7…`, bundle `29557b87…`); it is not an itemized night/reload or performance result. The later diagnostic cleanup preserves this water acceptance and does not change the shader or water recipe. The dated implementation, build, deployment and runtime evidence below remains intact.

## Diagnostic cleanup deployment — dev.3, 2026-10-24

The owner authorized installation, commit and push after the cleanup passed Release/Full
verification (20/20 steps, 18 test projects including 1,167 water / 104 light / 23 settings
tests) and isolated Pure verification (19/19). The verified DLL was reused without rebuilding
or regenerating the unchanged shader bundle. Metadata wording was clarified to distinguish
installation/startup from gameplay acceptance.

`Tools/deploy-mod.ps1` installed exactly three files with the game closed at **19:06:17 UTC /
21:06:17 Europe/Berlin**. Source, receipt and installed hashes match:

| Installed artifact | SHA-256 |
| --- | --- |
| `Mods/WorldLook.dll` | `cc9a206a2e13b3824ff25e7d8d36e4d73114eefae9beff78cfc9b1bd610ae356` |
| `UserData/WorldLook/WorldLook.pdb` | `060cadd73216cbae5b1c4b703025315385088c4910c271092df1cb7980b19edb` |
| `UserData/WorldLook/mod.json` | `bcd72fef3bb5d333fca2d9797a121fd1a7cd36bbb3af1e0d12224f3a7e1b319a` |

Receipt: `UserData/.deploy/receipts/WorldLook.20261024T190616870Z-46f33609b7ae.json`.
The three previous dev.2 files are retained under
`UserData/.deploy/backups/20261024T190616915Z-WorldLook-ef9a7ea7/` and match the pre-install
hashes. Of 45 checked installed files, exactly DLL/PDB/metadata changed; 42 remained identical,
including Shared, the startup profile and the old F9 reports. The helper reported the legacy
external `Mods/s1reference.bundle` as stale and left it untouched; dev.3 uses its embedded bundle.
Local plan/receipt/hash comparisons are in `.scratch/worldlook-dev3-install-20261024/`.
The receipt truthfully records parent Git HEAD `993c867…` with a dirty working tree at installation;
it does not claim the later commit was already present. Installation is not a new gameplay result.

## Owner acceptance and session — 2026-10-24

Dominik explicitly requested **"Wasser fertig abhacken und die debugs entfernen"** after confirming
**"on off funktioniert bereits ohne fehler"**. Water appearance, including the supplied sun view,
and normal ON/OFF use are therefore closed by owner acceptance. Do not reopen these items
because an earlier checklist lacked a separate capture. The earlier calm morning-motion
confirmation remains valid; this decision does not manufacture unrecorded test actions.

The Steam-launched singleplayer session used the owner-selected **Dominik / SaveGame_1**
on Schedule I **0.4.7f12**, MelonLoader **0.7.3**, WorldLook **0.3.0-dev.2**.
The native save was backed up before launch: 44 files, 1,021,017 bytes, hashes checked.
DLL `410fecf7dda5f079e86807942eb18ef98168201c4a9dddea124391e3d5bd1645` and embedded bundle
`29557b87609c367e3b568d84d10400d67c980581a1e3491fbd455def9794c8ef` match the accepted candidate.
Shared was `3520cd6719b435312f4ec2545422c5bf767d773d9ccba03bc03c7f1446381430`, rather than the
older Shared identity in the 2026-10-23 record. Profile bytes remained
`73b8e6dc25029a817bdcddac5487a1a8fc254597f43d57a1745cf07a559a1d65` (all enabled, Warmth 1).

- **ON, 18:36:35.928 UTC / 20:36:35 local:** HHMM 1742 → 1062 minutes,
  positive applied light strength `0.9045084971874737`, five channels, no errors or pending restores;
  reference water active, native sky, bundle loaded through `embedded-memory`.
- **OFF request, 20:36:42.336 local; report 18:36:47.409 UTC / 20:36:47 local:**
  light inactive/strength 0, water `Stopped/released`, no faults or pending restores.
  Successful re-enabling is owner-confirmed, not a second independently captured ON report.
- Dominik explicitly labelled the **12:02 image ON** and **17:59 image OFF**, and reported
  no visible change across the intervening hours relevant to his comparison. ON shows submerged
  plants and a broad broken reflection; OFF shows a darker, more uniform foreground surface.
  Time and framing differ; these are owner-contextualized pictures, not controlled GPU measurements.
- Dominik confirmed that he closed the game himself. The log ends with deinitialization and
  `Preferences Saved!` at 20:37:18 local. This is not a menu/reload observation.

Local raw evidence remains under `.scratch/WorldLook/acceptance-20261024-182045-2ac05e86/`;
the native-save backup remains under the matching `.backups/WorldLook/` run directory.
These local artifacts are not promised as files in a fresh clone.

| Captured artifact | SHA-256 |
| --- | --- |
| `03-on-off-20261024-183807/on-status.json` | `3f05e720aaffa35baae1857dbdfb26a7fd508b95f933a519b21f1c5fe6046ca3` |
| `03-on-off-20261024-183807/off-status.json` | `6d921f2574bb3ff40d452e1f344ee5101f4ac9f89d07c91ddb2a60f180ff45ba` |
| `03-on-off-20261024-183807/Latest.log` | `75622a874fe5f550b956889147a9e664c8fe664938ed12e803eab5f2d624be0f` |
| `04-owner-image-labels-20261024-184116/01-owner-labelled-on-1202.png` | `1a8765a53b545d91882c1d2baafccf9245842984ebdbf22690013d1989c7aa03` |
| `04-owner-image-labels-20261024-184116/02-owner-labelled-off-1759.png` | `9fe1c3ca109bee268adf52a9519dd6d55eeab47f2515bce2aa567d890d57aaeb` |

Remaining WorldLook evidence limits: visible warmth has not received a separate owner verdict;
night/menu/reload and quantitative frame times were not measured in this session. Positive numeric
warmth was observed. These limits do not reopen the accepted water work. The sections below describe
the earlier state of knowledge at their dates; the owner decision above supersedes their open water items.

## Approved scope — 2026-10-23

The owner approved finer, less regular water surface normals, smaller clearer sun glints with bounded intensity, retained bottom clarity and distance filtering; focused tests, shader/mod build, independent review and backed-up installation. Native motion/shimmer and final appearance require the next owner run. No owner visual acceptance of this candidate is implied by approval to implement it.

The dev.1 morning and midday comparisons are preserved in the [native-baseline record](native-baseline-verification.md). At 13:55 the apparent ON image showed visible submerged geometry but broad soft glint bands; the apparent OFF image at 14:08 had much stronger surface detail. The supplied images were not explicitly labelled ON/OFF. Both fresh logs loaded dev.1 and recorded one subsequent OFF; both F9 reports were OFF, so active-light HHMM readback was still open at that checkpoint. The shader's periodic normal field was a plausible cause of the visible bands, not a complete GPU diagnosis.

## Preserved baseline

- `.backups/WorldLook/water-detail-20261023-035705/manifest.json`: **87 hash-verified files**, including current sources, tests, shader/build inputs, generated bundle, docs and installed WorldLook state. The manifest also records **34 installed files** for later scoped deployment comparison.
- Installed pre-task DLL: `79721251dd863862b027d08d4064b47a197dd8a9527c98f84bf43f6457391c27` (dev.1).
- Pre-task embedded bundle: `24748b573273914b6a21b170d698d80ace7a6c7e0da153623353bf901a892391`.
- Pre-task shader: `2ce7e343c100f2f9d6f93aef3380039d5ee34d2787b283dc1dde6d46ac952a47`.
- Profile: `73b8e6dc25029a817bdcddac5487a1a8fc254597f43d57a1745cf07a559a1d65`; global/light/water enabled, mild Warmth 1.0.
- Version advanced through `Tools/bump-version.ps1`; no manual version edit. Existing unrelated working-tree changes are retained.

## Implementation

- Surface normals now use three continuously advected gradient-noise layers with analytical derivatives and quintic interpolation. Frequency multipliers `1.35 / 3.10 / 6.70`, slope weights `1.35 / .85 / .45`, independent rotations/travel directions. With authored WaveScale 1.6, nominal cell widths are about 46 / 20 / 9.3 cm.
- Pixel footprint fades each detail band across `.12–.55` cells/pixel; unresolved slope variance remains in the filtered sun lobe (heuristic factor .5), with a screen-derivative contribution (.25). The authored SunSharpness default is 512 rather than 220. Filtering broadens the lobe and reduces its peak; SunStrength .9 and contribution factor .6 remain. This does not establish measured game GPU cost or flicker-free motion.
- The original refraction gradient is retained separately for refraction and shore foam. Late dye, clear-water recipe, premultiplied composition, native main light/shadow inputs and the 17-property interface are retained.
- Parent hash comparison: **13 of 14 hand-maintained runtime source/project inputs byte-identical** to the new backup; the sole other file is `WorldLookMod.cs`, whose only change is the tool-generated version string. Source count remains **13 C# + project = 14**. Machine-readable check: `runtime-scope-verification.json` in the new backup.
- The old source-token test named as though it proved absence of comb bands was replaced by a deliberately limited surface/refraction/foam separation check. The native-light/shadow wiring check was extended. Neither test claims rendered visual acceptance.

## Executed checks

- Parent full WaterShader suite: **1,167 passed**, 0 failed/skipped; `checks/water-surface.trx` in the new backup.
- Parent baseline lighting suite: **104 passed**, 0 failed/skipped; `checks/lighting-baseline.trx`. The unchanged lighting logic was not retested after shader-only work. The prior dev.1 1,293-test total is historical, not a new total for this candidate.
- Full shader/C# build: **PASS, C# 0 warnings/errors**. Run `Source/Mods/WorldLook/build/logs/20261023-041153-490-d22f14f5/`, Unity 2022.3.62f3 / URP 14.0.12 / SDK 8.0.425, explicit `S1NoDeploy=true`. Bundle rebuilt and loaded, 17-property/eight-SVC-entry contract passed; no shader compiler errors/warnings found in the targeted log scan. This is not all-variant GPU execution coverage.
- Parent verified **42 production/build inputs**, recorded DLL identity and embedded bundle hash; no source drift. Built Shared matches installed `087b8768…`.
- Whole-solution format: exit 0. Version synchronization: 21 mods, 0 drift. Solution regeneration: byte-identical, 39 projects. Documentation paths and scoped `git diff --check` passed (an existing test-project line-ending notice was not a whitespace failure).
- Independent shader/test delta review: **PASS**, no blocking findings. The reviewer verified analytical derivative continuity/rotation, bounded variance-filtered lobe, byte-identical refraction function body, separate retained foam lighting, material/CBUFFER/pragmas and limited test claims. Factors .5/.25 remain filter heuristics rather than proven unbiased physical pixel integration. Twelve added corner hashes per fragment are bounded work, not a measured GPU-performance result. A targeted native material/shader-data check found no SunSharpness collision; native WaveSpeed does exist, so the fixture's default .8 is not an asserted game value.

| Built/installed artifact | SHA-256 |
| --- | --- |
| Water shader source | `2c53e90c374288d4216099816d15ef6ac051b177a496306639fa8231de25b84e` |
| Water bundle | `29557b87609c367e3b568d84d10400d67c980581a1e3491fbd455def9794c8ef` |
| `WorldLook.dll` | `410fecf7dda5f079e86807942eb18ef98168201c4a9dddea124391e3d5bd1645` |
| `WorldLook.pdb` | `e46712a29a36e0a03986cf657e5e543cd2bcb5454c08e53be005c46ff32f3fc3` |
| `mod.json` | `f1ce20e6bade1fbbe82b67ad511d196ec3667092c4c2424cbef4e5aa17db48db` |

## Controlled GPU comparison

Test-only fixture: `Source/Mods/WorldLook/build/preview/`. It loads the supplied binary bundle into an isolated Unity project and renders through URP SingleCameraRequest to a linear floating-point target. A fixed-time render pass precedes transparent drawing; near/far views at 0 / 7.5 / 31 seconds, dry controls and repeated t=0 frames are saved with pixel readback and metadata. The scene is a submerged checker floor and colored blocks under synthetic daylight, not Schedule I.

- Baseline executed by fixture coder on **AMD Radeon RX 9060 XT / D3D11**, ten images, repeated t=0 hashes identical, no observed nonfinite/error-magenta/uniform frames; water differs from the dry controls. Path: `.backups/WorldLook/water-detail-20261023-035705/preview/baseline-20261023-041104-443/`.
- Baseline fixture hash: `1250d29c1c14f8ef69af8d30c040960237e04f29f1593ef2a03606b52a14e0e0`; rendered bundle `24748b57…`. Parent inspected the runner and fixture and viewed the baseline near image, which shows broad soft glint bands.
- Parent candidate run: **PASS**, ten images, matching repeated t=0 hashes, no observed nonfinite/error-magenta/uniform frames and visible water contribution. Path: `.backups/WorldLook/water-detail-20261023-035705/preview/candidate-20261023-041451-317/`. It renders the built `29557b87…` bundle directly.
- Parent comparison of the receipts confirms matching fixture/GPU/pipeline/scene/light/cameras/times, **pixel-identical dry controls**, and exactly one changed material input: SunSharpness 220 → 512. All six sampled water views differ. `preview/comparison.json` preserves all ten matched pairs and hashes; `preview/comparison-t0.png` is a labelled, unscaled near/far contact sheet.
- Parent visually inspected matching near t=0, near t=7.5 and far t=0 pairs. The candidate replaces broad blurred glint bands with smaller irregular detail and a more subdued far-field highlight; floor/block visibility remains readily apparent. This is a controlled-image judgment, not a quantitative clarity measure or owner acceptance. Other fixed-time frames passed the automated output checks.
- Fixed-scene render checks cannot establish game motion/performance, camera texture contents, shadow-variant coverage or owner acceptance. The native runtime may retain a different WaveSpeed when cloning the native material; the fixture intentionally retains each shader's defaults and records them.
- Independent fixture/evidence follow-up: **PASS**, no blocking finding. The reviewer checked actual URP clock-reset ordering, binary-bundle identities, float-readback and depth/opaque setup, isolated outputs/cleanup, receipt consistency and all **20 PNG hashes**. The review executed no new render and retained the stated synthetic-scene limitations.

## Deployment and rollback

The approved deployment used `Tools/deploy-mod.ps1` after the game-closed check and an exact three-target plan. At 04:22:53 UTC it replaced `Mods/WorldLook.dll`, `UserData/WorldLook/WorldLook.pdb` and `UserData/WorldLook/mod.json`.

- Receipt: `C:\Program Files (x86)\Steam\steamapps\common\Schedule I\UserData\.deploy\receipts\WorldLook.20261023T042253544Z-e3065c22f2c7.json`.
- Dev.1 rollback artifacts: `UserData/.deploy/backups/20261023T042253589Z-WorldLook-2e2ba848/` in the game install. All three backup hashes match the pre-task targets recorded in the 87-file snapshot.
- Parent post-install readback: all three source/receipt/destination hashes match. Of **34 checked pre-existing installed files**, exactly these three changed; **31 remain byte-identical**, including the startup profile and Shared. Full record: `.backups/WorldLook/water-detail-20261023-035705/installed-candidate-verification.json`.
- Profile stays global/light/water enabled and mild Warmth 1.0, hash `73b8e6dc…`. The legacy external `s1reference.bundle` was reported stale and retained; this candidate uses its embedded bundle.
- The game was closed at final deployment readback. No game launch/save/time/weather mutation occurred in this implementation; at that point Latest.log/status still described dev.1. No commit or push was made. The owner's later dev.2 run follows below.

## First dev.2 runtime — 2026-10-23

The owner supplied morning screenshots at **Wednesday 07:59 / 08:17**. By collection time the game had exited, so no live MCP call was sent. Read-only evidence is under `.backups/WorldLook/water-detail-20261023-035705/runtime-20261023-051741/`: Latest.log, startup profile, identity capture and both F9 reports. The active report was retained by SafeStorage as `status.json.bak`; it was copied and SHA-256 verified (`d18b795de131d8a01898ee4f3bf69d5e7c152273ec220226e534726b35e98a3b`). Images remain chat attachments, not locally exported backups.

- The loader log identifies **0.4.7f12**, WorldLook **0.3.0-dev.2**, DLL `410fecf7…` (lines 119–120) and embedded bundle `29557b87…`. The on-disk DLL/profile hashes still match the installed candidate. Reference water is prepared/applied at **07:14:43 local** with the retained clarity recipe (lines 394–397).
- **ON F9, 05:15:48 UTC / 07:15:48 local:** `lookRequested=true`, native `SkyMat / Custom/ScheduleOneSky`, lighting `active=true`, `timeHhmm=740`, `minutesSinceMidnight=460`, `timeReadable=true`, `readableChannels=5`, `appliedStrength=0`, no pending restores/errors. This is a real active clock/channel observation. Zero warmth is expected at 07:40: the morning lobe ends at 07:30. It does not verify positive-strength tinting at a warm peak or the whole clock cycle.
- The same ON report has water `Applied`, `reference active`, `referenceReady=true`, `environmentSource=native SkyState`, camera instance `-290942`, depth/opaque requests true from the main-camera settings, and zero owned reflection cameras. These request flags still do not independently validate current-frame GPU texture contents.
- At **07:16:20 local**, one OFF request logs water `Stopped` and material `released` (lines 537–538). **OFF F9 at 05:16:33 UTC** reports both module fault flags false, lighting pending restores/errors zero, and water `Stopped`/`released`. There is no subsequent ON request in this run. No WorldLook exception/error is present in the captured log.
- In the 07:59 image, submerged plants/pilings remain more visible and the dark water surface is subdued. The 08:17 image shows stronger surface patterning with less submerged detail. The log sequence supports apparent ON then OFF, but the images have no explicit state labels. Camera pitch and time differ; neither image contains the strong direct-sun reflection seen in the previous midday pair. These stills therefore do not establish glint-size improvement or motion/shimmer acceptance by themselves.
- **Owner-confirmed 2026-10-23:** in response to whether slow camera movement looked calm or visibly flickering, Dominik answered **"ruhig"**. This accepts calm surface motion/no noticed shimmer for the reported morning test. It is owner-reported observation, not a measured frame-time result or acceptance of strong-sun glints.

## Runtime acceptance at the 2026-10-23 checkpoint

Observed: correct candidate/bundle startup, active reference-water state with native sky input, one valid HHMM-to-minutes sample and five readable light channels, followed by one reported clean OFF release. Owner confirms calm camera-motion behavior in the morning test. Pending: overall surface/direct-sun-glint judgment, positive-strength warmth, ON-after-OFF, night and menu/reload roundtrip; GPU performance is unmeasured. The captured active F9 closes the earlier missing-ON-status item; another identical capture is not required just to repeat it.
