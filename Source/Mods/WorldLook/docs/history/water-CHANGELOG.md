# Changelog

## 0.2.0 (2026-10-17)
- Reference water shader prototype `S1Mods/ReferenceWater`: an authored URP shader with a real vertex **and** fragment program (D3D11) that composes scene-depth transmission, scene-color refraction and a sampled planar-reflection input. This supersedes the material-only recipe, which could not reach the reference look (0.1.2/0.1.3 were honest but visually close to vanilla).
- Real planar reflection: one mod-owned disabled camera plus one aspect-preserving bounded render target (long side ≤ 512 px), rendered from the single main gameplay camera through `RenderPipelineManager.beginCameraRendering`. The reentrancy gate closes before the nested `UniversalRenderPipeline.RenderSingleCamera`; no world or renderer scans are reintroduced.
- Binding mechanism measured in the editor instead of assumed (`BuildReference.ProbeBinding`): `Material.HasProperty` reports **false** for the CBUFFER-only `_ReflectionVP`/`_ReflectionAvailable` uniforms while `SetMatrix`/`GetMatrix` and `SetFloat`/`GetFloat` round-trip them. The material property sheet is therefore the primary binding, with a `Shader.SetGlobal*` write kept as redundancy.
- Fixed a review-caught culling defect before release: the mirrored-pose camera is a proper rotation (`LookRotation`), so the global culling sense must **not** be flipped — doing so rendered the reflection inside-out. The reentrancy gate release stays in a `finally` and is now pinned by a test. Stale global reflection inputs are cleared when the session destroys the bound clone.
- Fail-closed to the vanilla recipe on every failure path: the clone's original shader is restored before falling back, and the reference path never destroys the session-owned material or writes the native slot. `WaterSession.cs` remains byte-identical (SHA-256 `51eb6e18b8421595b4a68a936f186e237aa5cc279b54c1ce5313646a71cc55d6`).
- 1,124 tests pass (1,077 prior + 47 new policy/wiring tests, RED observed before each implementation). Release build 0 warnings/errors; `dotnet format` clean; `WaterShader.csproj` gains the `Unity.RenderPipelines.Universal.Runtime` reference. Deployed 2026-10-17: DLL SHA-256 `7d38407aabf124dfe5104caf904fe6fed9fcdc7403f067c546a6c85d708ac3a2`, `s1reference.bundle` 9,104 bytes SHA-256 `5283b1b358003b2fca1fdde80817e862103473c844e5c9ad5c0cd9e4a20c5a29`, and `mod.json`/PDB hash-matched in `UserData/WaterShader/`.
- Independent review of the pre-fix stand returned **FAIL** with one fix-now finding (the culling defect above); that fix and the review hardening are applied, and the delta re-review of the changed region returned **PASS** with no fix-now findings. Deferred (documented, non-blocking): the stale-bind branch clears the reflection silently and should log one state transition to match the mod's logging convention.
- **In-Game-Verify open.** Shader/fragment compilation and the AssetBundle round-trip are proven (`isSupported=True`, `passCount=1`, bundle SHA-256 `5283b1b358003b2fca1fdde80817e862103473c844e5c9ad5c0cd9e4a20c5a29`), but reflection appearance, shoreline clipping, oblique-clip behaviour, measured reflection cost and FPS impact are unverified until a real game session.

## 0.1.3 (2026-10-17)
- Clear Shallows transparency candidate: set shallow RGBA (0.025, 0.055, 0.060, 0.15), intentionally superseding the prior preserve-alpha recipe. Native Deep/Base RGBA remains untouched.
- Restore declared native shader defaults for View Depth/Vertical Height Depth (4/1) and refraction strength (0.1); suppress chromatic aberration (0). These are supported parameters and test candidates, not proven physical visibility distances or guaranteed pixel transparency. Existing waves/normals/foam, textures, keywords, absorption, depth exponent, ZWrite, pipeline and reflection state remain unchanged.
- Preserve byte-identical WaterSession scheduling/cache/ownership logic; configure/log once per clone. 1,077 tests passed, including all 1,024 support subsets and 20 before/after setter-failure cases. Release 0 warnings/errors; independent exact-release review PASS with no blocking findings. Deployed DLL/PDB/metadata hash-matched; installed assembly version/diagnostic confirmed. DLL SHA-256 `91aa5eecc8b173b17248a47245ce5e5878eee49c6f39ac938c4bd9c066517c31`.
- Real bottom visibility, actual camera buffers, reference fidelity, frametimes and reload/shutdown remain In-Game-Verify open. No underwater geometry or new render passes were added. See [Clear Shallows scope/evidence](clear-shallows.md).

## 0.1.2 (2026-10-17)
- Natural-coast material recipe: shallow RGB (0.025, 0.055, 0.060), preserving the cloned alpha and the native deep/base color. Slightly stronger existing normals (0.18), reduced base foam (0.25), wave count 4 within the declared 1–5 range; accepted wave height/speed 0.35/1.1 retained.
- Remove unsupported guessed property aliases; apply six supported-property checks once per clone and log the supported applied values using invariant formatting. No texture/keyword/reflection/tessellation changes or new render passes.
- WaterSession scheduling, caching and ownership logic unchanged. 29 tests pass (including property subsets, alpha preservation and partial failures); Release 0 warnings/errors, independent release review PASS with no blocking findings. Deployed DLL/PDB/metadata hash-matched, and installed metadata contains WaterLookRecipe plus the native adapter. DLL SHA-256 `7a621c85c90645347e423c756da1b4638e6990edee5aba1a50ec398da8bd25eb`. New-look In-Game-Verify open. Prior 0.1.1 smooth-play acceptance is historical, not new-build verification.
- Runtime follow-up 2026-10-17: the loaded session hash and recipe/Applied breadcrumbs confirm activation, but the owner's screenshot still appeared close to vanilla: dark flat water and strong solar glare. No improved-realism acceptance was granted. The next authorized scope is shallow-water transparency; see [Clear Shallows](clear-shallows.md).
- See [native material evidence](material-evidence.md) for installed asset provenance and the distinction between native values and artistic candidates.

## 0.1.1 (2026-10-17)
- Performance repair: remove the recurring full-scene mesh-renderer scan (captured ~67,700 renderers every 5 s) that could cause microstutter. Resolve only Main/Map/StylizedWater2_Ocean through the Map singleton plus one direct-child lookup, then cache the renderer.
- Retain the exact turquoise color and wave recipe, native StylizedWater2 shader and HasProperty guards; no look change.
- Evidence: 17/17 tests pass, Release 0 warnings/0 errors, independent release review PASS with non-blocking notes. Built/installed DLL SHA-256: `8719debc877fed54c27e01dfae9fb6504f2e9a9090b1b521434e1f06c06f53a4`.
- Owner-confirmed 2026-10-17: stutters are gone. The matching 0.1.1 session logged `Applied: Main/Map/StylizedWater2_Ocean; last discovery=0.660 ms`. This closes observed smooth-play acceptance, not quantitative frametimes, menu/reload or shutdown coverage.

### Performance repair details

- Remove recurring global mesh-renderer enumeration (captured log: approximately
  67,700 scene renderers). Resolve only Main/Map/StylizedWater2_Ocean through the
  Map singleton's raw static field and one direct-child lookup; cache the renderer.
- Retain the exact 0.1.0 turquoise color and wave recipe, native shader and guards.
- Add monotonic, bounded late-load retries and cached health checks, transition-only
  logging with measured discovery cost, invalidation/reload and ownership-safe
  restoration/destruction. Never fight another writer or destroy on uncertain restore.
- Add runtime-used pure policy regression tests and supplementary source guards:
  17 tests passed after observed RED-to-GREEN cycles. No-deploy Release compile:
  zero warnings/errors. Owner-confirmed smooth play is recorded above; additional visual/lifecycle coverage remains open.
- Released as 0.1.1 after independent review; no commit or shader-asset cleanup.

## 0.1.0 — 2026-10-17

- Existing initial material-clone approach tunes the game's StylizedWater2 shader.
  Custom bundled shader assets are not loaded by the runtime implementation.
