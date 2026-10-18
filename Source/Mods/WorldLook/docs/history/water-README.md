# WaterShader

Reference water shader prototype. Release **0.2.0** ships an authored URP shader
(`S1Mods/ReferenceWater`) in `s1reference.bundle` with scene-depth transmission,
scene-color refraction and a real planar reflection rendered by one owned disabled
camera. 1,124 tests, Release 0 warnings/errors, independent delta re-review PASS with
no fix-now findings. **In-Game-Verify open**: reflection appearance/orientation,
shoreline and oblique-clip behaviour, measured reflection cost and FPS are not
established by these tests. Prior 0.1.1 smooth play was owner-confirmed 2026-10-17:
stutters gone. Reload/shutdown and quantitative frametimes remain open.

## Scope

The runtime-used Unity-free `WaterLookRecipe` checks ten supported properties once
per clone: retain wave height/speed 0.35/1.1, count 4, normals 0.18 and base foam 0.25;
set shallow RGBA (0.025, 0.055, 0.060, 0.15), depth parameters to declared defaults
4/1, refraction strength to declared default 0.1 and chromatic aberration to 0.
Native deep `_BaseColor` RGBA, absorption/depth exponent, ZWrite, textures and
keywords remain unchanged. **0.2.0 deliberately supersedes the earlier material-only
boundary**: it adds an authored shader and one reflection camera/render pass, approved
by the owner after the material-only recipes (0.1.2/0.1.3) stayed visually close to
vanilla. No underwater geometry or lighting changes. See [Clear Shallows](clear-shallows.md)
for the 0.1.3 evidence and its limitations, and [reference-target](reference-target.md)
for the 0.2.0 acceptance table.

The previous implementation enumerated every mesh renderer every five seconds.
The captured `Latest.log` recorded roughly 67,700 scene renderers per scan. This
is direct evidence of unnecessary work, not a measured attribution of all stutter.

The repair reads the generated `Map.instance` static field, validates the Main
scene/root, then resolves only its direct `StylizedWater2_Ocean` child and renderer.
GraphicsProbe's installed `latest.json` records `Map[0]/StylizedWater2_Ocean[2]`,
Main, one shared-material slot and `Stylized Water 2/Standard (Tessellation)`.
The local f12 interop decompile shows `Map : Singleton<Map>` and the available
`instance` accessor. Only the exact target is resolved; there is no global fallback.
Native compilation confirms availability, not a hard frame-time guarantee.
Shared `GameObjectResolver` was inspected but deliberately not used: its helpers
recurse or allocate all descendant components, contrary to this narrow scope.

The native property provenance and the historical natural-coast recipe are recorded
in [material-evidence.md](material-evidence.md). That document separates installed
asset values from artistic candidates and pending visual acceptance.

## Runtime policy

- No discovery in menus. On Main load, retry one targeted lookup every five real
  seconds until the singleton, child, renderer, material and shader are available.
  The direct-child lookup includes inactive children and does not enumerate the world.
- Cache one renderer, original shared material and owned clone. Every second check
  only that binding; normal idle frames do no native work or managed allocation.
- Fake-null destruction invalidates the cache; release before a delayed rediscovery.
  There is no catch-up loop or retry flood after a stalled frame.
- Preserve another writer's replacement: release the unused clone and yield until
  that renderer dies or Main unloads. Do not retint another mod's material repeatedly.
- Main unload/deinit restores slot zero only while it still points to our clone,
  verifies detachment, then destroys only our clone. Setup/restore exceptions retain
  the lease for bounded retry rather than destroying a possibly attached material.
- Log only state transitions, with the last measured targeted-discovery duration.
  A single native call cannot be interrupted: call-count bounds are **not** a hard
  frame-time guarantee. Cloning/tuning time is not included in discovery timing.

## Verification

Release 0.2.0: `WaterSession.cs` byte-identical (SHA-256
`51eb6e18b8421595b4a68a936f186e237aa5cc279b54c1ce5313646a71cc55d6`), and the
reference bundle `s1reference.bundle` is 9,104 bytes, SHA-256
`5283b1b358003b2fca1fdde80817e862103473c844e5c9ad5c0cd9e4a20c5a29`, proving
`S1Mods/ReferenceWater` compiles with real vertex and fragment programs and survives
the bundle round-trip (`isSupported=True`, `passCount=1`, `messages=0`). Deployed
2026-10-17: installed DLL SHA-256
`7d38407aabf124dfe5104caf904fe6fed9fcdc7403f067c546a6c85d708ac3a2`,
`s1reference.bundle` 9,104 bytes (SHA-256 `5283b1b3…c5a29`) and `mod.json`/PDB in
`UserData/WaterShader/` all hash-matched against the reviewed build. Actual new-build
rendering, reflection inputs and underwater visibility remain unverified.

Prior 0.1.3 installed evidence (historical), DLL SHA-256: `91aa5eecc8b173b17248a47245ce5e5878eee49c6f39ac938c4bd9c066517c31`. Activation was proven, but the screenshot stayed close to vanilla — no visible bottom.
Prior 0.1.2 installed evidence (historical), DLL SHA-256: `7a621c85c90645347e423c756da1b4638e6990edee5aba1a50ec398da8bd25eb`.
Its session log confirmed activation, but the supplied screenshot remained close to vanilla. It was not accepted as a convincing realistic-water overhaul.

Current candidate: 1,124 xUnit cases passed: runtime-used recipe and scheduler/ownership orchestration,
late loading, fake-null invalidation, retries, reload, foreign writes, partial setup,
failed/ignored writes, cleanup and an idle-allocation check; supplementary source
contracts pin exact discovery, lifecycle wiring and material-adapter delegation.
Recipe tests cover all 1,024 property subsets, explicit shallow opacity, preservation
of deep/other state, declared defaults/ranges, invariant diagnostics, one-time
configuration, 20 before/after setter failures and 10,000 idle ticks with zero modeled
runtime/material work or allocations. These are not Unity/GPU or rendering tests.
Release compile passed against installed references with zero warnings/errors.
Prior 0.1.1 deployed evidence (historical), DLL SHA-256 `8719debc877fed54c27e01dfae9fb6504f2e9a9090b1b521434e1f06c06f53a4`. The real 0.1.1 session logged `Applied: Main/Map/StylizedWater2_Ocean; last discovery=0.660 ms` (discovery only, not frame time).

Commands from repository root (Windows bash):

```sh
C:/Users/pc/.dotnet/dotnet.exe test Source/Tests/WaterShader.Tests/WaterShader.Tests.csproj -c Release -p:S1NoDeploy=true -p:BuildProjectReferences=false
C:/Users/pc/.dotnet/dotnet.exe build Source/Mods/WaterShader/src/WaterShader.csproj -c Release -p:S1NoDeploy=true -p:BuildProjectReferences=false
```

## Pending runtime acceptance / limitations

The 0.2.0 review of the pre-fix stand returned FAIL with one fix-now culling finding
(the mirrored-pose camera must not have its global culling sense flipped); that fix and
the review hardening are applied, and the delta re-review returned PASS with no fix-now
findings. The owner confirms stutters gone in 0.1.1.
Do not infer from that report that menu/reload, delayed loading or graceful
cleanup were exercised. Those lifecycle checks remain open.
Compare frametimes at the same location/camera/settings; tests do not prove FPS,
p99, GC behavior inside Unity or freedom from unrelated microstutter.

Renaming/reparenting the known object or a missing singleton deliberately leaves
vanilla water untouched; there is no global fallback. An unreadable collected proxy
can retain one lease and prevent reapplication until cleanup becomes safe. If this
persists through shutdown, native cleanup is unverified and may leave one material
until process exit. This conservative behavior is safer than destroying attached
state. Foreign material replacement intentionally wins over this mod's look.
