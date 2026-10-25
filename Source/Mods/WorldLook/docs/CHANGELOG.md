# WorldLook

## 0.3.0-dev.5 (2026-10-25)

- Repair shader-cache lifetime across menu/reload: validate native shader and bundle objects, retain the owned shader with `DontUnloadUnusedAsset`, and allow one acquisition per explicit initialization after stale or failed loads. Existing flags are preserved; shutdown releases only the owned retention bit and keeps `Unload(false)` for borrowed material consumers.
- Keep the embedded water shader, bundle and recipe unchanged. The dev.3/dev.4 fallback evidence remains in the [acceptance record](in-game-acceptance.md); the exact native unload trigger is not established by those observations.
- Add native-free tests of the real cache implementation with Unity boundary doubles. These cover lifetime and recovery decisions; native reload verification is recorded separately.
- Installed DLL `3f46ae888db9b7dcc61db7a4c83a211274c849451056dae08292a730daebd8c9`; Full 21/21 passed (1,183 water tests, including 16 new cache cases; 41 WorldLook and 81 driver checks). Agent-run MCP tests in Dominik passed two ON reloads and two OFF reloads followed by ON, confirmed by eight actual renderer shader/support readbacks and ten screenshots. No shader warning; all 44 native-save files and the profile stayed unchanged. Initial cold-load polling rejected a stale response after a timeout; fresh readiness was confirmed without repeating the load. Earlier failure evidence and remaining visual/performance limits are retained in the acceptance record.

## 0.3.0-dev.4 (2026-10-25)

- Add an owned persistent `WorldLook.Automation` component for existing S1MCP reflection tools: parameterless status, explicit ON/OFF and native return-to-menu commands. No new S1MCP dependency/server, periodic diagnostics or profile writes.
- F7 and MCP share one idempotent session setter. Repeated same-state requests retain resource/fault state; OFF→ON remains deliberate recovery. Mutations require the game thread and a ready singleplayer save; status remains available in Menu. A requested state is not a claim that every render channel is applied.
- Add the bounded local MCP driver and synthetic transport/identity/readback tests, plus coordinator/adapter lifecycle tests. Screenshots remain agent-driven through supported Computer Use APIs, paired with timestamps and evidence hashes. [Automation contract](mcp-automation.md).
- Preserve the dev.3 session evidence from 2026-10-25: repeated Sun tint at 18:00, owner-accepted normal night, and restored ON lighting after Dominik menu/reload. The reference-water shader failed on that reload and remains a separate known defect; this control change does not alter shader assets or recipes. [Acceptance record](in-game-acceptance.md).
- Installed/hash-verified DLL `5522f394523dbaaba7eab675bb579df8523892b7db7890d7baed5201640949e4`; Full 21/21 and 41 WorldLook tests passed. Agent-run MCP acceptance in Dominik verified explicit/idempotent ON/OFF and both same-process menu/reload state/light paths, with seven screenshots. All 44 backed-up native-save files and the profile stayed byte-identical. Reference-water fallback after reload was reproduced and remains open; no full visual/performance acceptance is implied.
- A real initial loading-frame status timeout led to bounded read-only status retries; mutations remain once-only and failed evidence is retained. The revised driver passed 81/81 synthetic checks and independent review. Both complete reload runs then succeeded without a timeout.

## 0.3.0-dev.3 (2026-10-24)

- Remove Ctrl+Shift+F9, the runtime JSON status export and water diagnostic snapshots/timing. Remove successful shader/material/input/transition detail logs. Keep F7, concise startup/toggle confirmation, and errors/fallback/restore warnings. The internal light snapshot remains for the existing lifecycle tests, with no runtime export.
- Water shader, embedded bundle, recipes and restoration/ownership guards are unchanged. Update the existing water-wiring assertions for direct recipe calls without logging; material writes still execute.
- **Owner acceptance recorded 2026-10-24 for the installed dev.2:** Dominik confirmed ON/OFF without errors and explicitly requested water marked finished. Water appearance and toggling are closed; retain the earlier calm-motion acceptance. The session independently captured positive light strength at 17:42 and clean OFF restoration. Details, hashes and attribution are in the [water-surface record](water-surface-verification.md#owner-acceptance-and-session--2026-10-24).
- Installed/hash-verified 2026-10-24 with backed-up dev.2 DLL/PDB/metadata. DLL `cc9a206a2e13b3824ff25e7d8d36e4d73114eefae9beff78cfc9b1bd610ae356`; Release/Full verification 20/20 passed. No new gameplay acceptance is implied. Visible warmth, night/reload and measured performance remain separate WorldLook evidence limits, not reasons to reopen the accepted water work. Earlier F9 reports and backups are retained.

## 0.3.0-dev.2 (2026-10-23)

- Focused surface/glint refinement after the dev.1 midday comparison: three advected gradient-noise detail layers, footprint filtering with retained unresolved variance, and compact bounded sun glints (authored sharpness 512). Refraction/foam keep the prior gradient and the clarity recipe is retained.
- Parent ran 1,167 water tests (0 failed/skipped); full Unity/C# build passed with 0 C# warnings/errors, no shader compiler errors/warnings in the targeted scan. Runtime C# inputs are byte-identical except the tool-generated version. Independent shader and fixture reviews PASS. Controlled binary-bundle GPU comparison passed: identical dry controls/repeated time-zero frames; matched near/far pictures show finer glints and readable submerged blocks. These synthetic Editor checks do not accept game motion or performance.
- Installed with three hash-verified dev.1 rollback targets; DLL `410fecf7dda5f079e86807942eb18ef98168201c4a9dddea124391e3d5bd1645`, embedded bundle `29557b87609c367e3b568d84d10400d67c980581a1e3491fbd455def9794c8ef`. Post-install comparison: only DLL/PDB/metadata changed among 34 checked files; profile retained. **In-Game-Verify open** for full acceptance.
- **First dev.2 runtime, 2026-10-23:** loader/bundle hashes match. The ON F9 preserved in `status.json.bak` confirms native sky, active reference water/native SkyState, HHMM `740` → 460 minutes, five readable lighting channels and expected neutral strength at 07:40; no module faults/errors. Subsequent OFF F9 reports clean release. Owner images at 07:59/08:17 lack a strong direct-sun highlight; glint improvement is not yet accepted. Game was already closed at collection, so no live MCP query. Full details in the current verification record.
- **Owner-confirmed 2026-10-23:** surface stayed **"ruhig"** during slow camera movement in the reported morning test, with no noticed shimmer. This closes that scoped movement check; strong-sun appearance, measured performance and the remaining lifecycle checks are separate.
- The pre-change source/assets/installed state are preserved in an 87-file hash-verified snapshot. [Current verification and rollback record](water-surface-verification.md). Historical dev.1 runtime evidence below does not verify this candidate.

## 0.3.0-dev.1 (2026-10-22)

- Canonical repository baseline: native sky/clouds/sun/moon/stars, the mild historical GoldenHour tint recipe and the current environment-only clear-water shader. The former external standalone prototype is preserved under `Source/Archive/WorldLook/Standalone-0.2.0/`.
- Correct native HHMM conversion in the light controller. The older GoldenHour time filter and standalone sky reader both incorrectly treated HHMM as minutes; a historical visual acceptance was not a verification of that clock input.
- One host owns F7 and module lifetime; small F9 state replaces the full F8 GraphicsProbe. Scene scanning/probe sources and tests are archived. No native skybox material replacement, extra reflection camera or global reflection uniforms.
- Five evidenced light color channels with native drift/readback tracking and owned-write restoration. Water reads native SkyState/ambient/weather inputs on bounded cadences. Water recipe and shader composition retain the clearer 0.2.0 candidate values.
- One JSON startup profile: Enabled, LightingEnabled, WaterEnabled, Warmth. The new warmth strength is distinct from the retired cloud-light setting. Existing valid files are not auto-rewritten; invalid reads disable the session's look and preserve the input.
- Independent review found and closed a disabled-water cleanup defect: OFF now permits bounded restore-only retries after transient native failure, without allocating or reapplying. Five regressions observed RED before repair.
- Full Unity/C# build passed with 0 warnings/errors; parent ran 1,293 tests (104 lighting, 1,166 water, 23 settings), final independent delta review PASS, solution format/version/doc/SLN checks passed. Candidate installed with hash-verified backup; DLL SHA-256 `79721251dd863862b027d08d4064b47a197dd8a9527c98f84bf43f6457391c27`. Profile enables light/water with mild Warmth 1.0. [Evidence and rollback](native-baseline-verification.md). **In-Game-Verify open**; prior prototype screenshots do not accept this candidate.
- **2026-10-23, first candidate runtime:** loaded DLL/bundle identities match; log confirms reference-water application and one OFF/material-release transition. F9 reports native sky, both modules OFF, no module fault flags or pending lighting restores. Morning screenshots (07:25/07:44) show more visible submerged detail in the first and stronger surface reflections in the second; both are dark, with AN/AUS labels inferred from sequence rather than supplied by the owner. No live MCP capture (game already closed). Full visual acceptance, active-light readback, re-enable and reload checks remain open; see the verification record.
- **2026-10-23, midday follow-up:** 13:55/14:08 images reveal clear submerged geometry but broad soft highlight bands in the first, versus more distinct surface/glints in the second. A fresh run again confirms application and clean reported OFF; F9 was captured while OFF, so active lighting is still unverified. A focused surface-normal/highlight refinement is proposed pending approval; no code, shader or installed-state changes made. Details and limitations are in the verification record.

## 0.1.5 (2026-10-20)

- The comb stripes were two straight cosines. The wave field now bends on a slow warp and uses four directions whose frequencies do not lock into parallel teeth. Clarity, tint, absorption and foam are unchanged.
- Bundle rebuilt, 9 705 bytes, SHA-256 `04a2afb6321e06321869740541d19ea71d017b3148592a7971beb609ad4c3a6f`. Diagnose: `isSupported=True`, `passCount=1`, `messages=0`, fragment pass compiled.
- **Owner-confirmed 2026-10-20.** Dominik accepted the water look on 0.1.5 after a Main-scene session. `Latest.log` shows WorldLook v0.1.5 loaded and the reference shader bound on `Main/Map/StylizedWater2_Ocean` with `_Absorption=0.1`, `_RippleStrength=0.28`, `_ShallowTint=(0.46,0.52,0.55)`. Individual actions were not itemized. This closes the water acceptance item. The 2026-10-18 light look and manual probe records stay their own.

## 0.1.4 (2026-10-20)

- The blue plate was not the tint. The shader covered the water with `alpha = 0.35 + absorb * 0.65` and dyed with raw absorb, so a few metres were already an opaque sheet. Material values could not lift that. The fragment now delays the dye (`pow(absorb, 2.2)`) and keeps alpha near clear until that dye is high (`0.04 + dye * 0.85`). Refraction smear `0.035 → 0.012`, downward reflection bias `0.08 → 0.02`.
- Recipe follows the new curve: `_Absorption` 0.22 → 0.10, `_RippleStrength` 0.45 → 0.28. Shallow tint stays `(0.46, 0.52, 0.55)`. Foam, deep tint and sun are not written.
- Bundle rebuilt with Unity 2022.3.62f2. `s1reference.bundle` 9 510 bytes, SHA-256 `7bbc68ea52bc3fd3948c295051e9893ddd6baab73f0ab28205c74649b60479c0`. Diagnose: `isSupported=True`, `passCount=1`, `messages=0`, fragment pass compiled.
- WaterShader.Tests: 1 145 passed. In-Game-Verify open. Deep ocean should still darken; harbor water should show the bottom.

## 0.1.3 (2026-10-20)

- The 0.1.2 water still read creamy. The shader multiplies everything seen through the surface by `lerp(_ShallowTint, _DeepTint, absorb) * 2.0`. The old tint `(0.50, 0.75, 0.75)` therefore gained `(1.0, 1.5, 1.5)` and lifted the bottom toward pale cream. The new tint `(0.46, 0.52, 0.55)` keeps that gain near the bottom's own colour, with only a small cool bias.
- `_Absorption` 0.38 → 0.22, so the dark deep tint and the extra opacity arrive later. `_RippleStrength` stays 0.45. Deep tint, reflection, sun and shore foam are still not written.
- Still no shader or bundle rebuild. The shader's own alpha floor (`0.35 + absorb * 0.65`) remains, so ankle-deep water is not glass. This step removes the pale wash, not that floor.
- Tests first: the old pin that green and blue must stay at 0.75 failed on purpose, then the recipe moved. WaterShader.Tests: 1 144 passed.
- In-Game-Verify open.

## 0.1.2 (2026-10-18)

- **Reverts the shore-foam tweak from 0.1.1.** That was scope creep on my side: the owner asked for clarity, not less foam, and the change was nearly inert anyway. The shader gates foam through `smoothstep(0.35, 0.9, foamBand)`, so foam only *starts* below `0.65 × _FoamDepth` and only *saturates* below `0.10 × _FoamDepth` — dropping `_FoamDepth` 0.35 → 0.28 moved the onset from 22.7 cm to 18.2 cm of eye-space thickness, a hairline either way. `_FoamDepth` and `_FoamStrength` are no longer written at all and stay at the shader's own defaults.
- **Adds the real remaining clarity lever: `_ShallowTint` red 0.35 → 0.50.** Everything seen *through* the surface is `sceneColor * lerp(_ShallowTint, _DeepTint, absorb) * 2.0`. At the old red that gain kept red at only ~47 % of green in the shallows (~39 % at mid depth), which is why the bottom flattened into teal and read as "not clear" rather than as an absorption problem. Green and blue stay untouched so the palette keeps its character, and `_DeepTint` is not written at all.
- The recipe therefore writes exactly three values now: `_Absorption` 0.38, `_RippleStrength` 0.45, `_ShallowTint` (0.50, 0.75, 0.75). Reflection, sun, deep tint and foam remain the owner-approved shader defaults.
- Still no shader or bundle rebuild — material property values only. The colour write goes through the same `HasProperty`-guarded, Unity-free path as the floats.
- Tests were rewritten rather than merely extended: a pin that the foam values are *not* written (including the shader defaults it falls back to, parsed from the shader so they cannot drift), a semantic pin that the shallow-tint red rises without overtaking green and with green/blue/alpha unchanged, and the shader-declaration parse now covers the `Color` declaration too. 1 143 tests in the suite; the drop from 1 152 is the smaller tables (the float subset theory went 16 → 8 cases).
- **Open question, honestly flagged:** the percentages above are arithmetic on the shader's raw literals. The game runs a Linear colour space, so if Unity converts the un-annotated `Color` default sRGB→linear on this shader-swap path, the effective baseline is redder than 0.35 suggests and the figures shift. The direction of the change and its safety hold under either model — but the numbers do not, so they should be confirmed by reading the unwritten `_DeepTint` off the configured clone via S1MCP. Independent review: PASS-with-nits; both actionable nits fixed before deploy (the foam-defaults pin above, and an upper bound on the tint red).
- In-Game-Verify open.

## 0.1.1 (2026-10-18)

- The reference water reads clearer. `ReferenceWaterLook` writes four material properties onto the owned clone right after the reference shader is bound: `_Absorption` 0.55 → 0.38, `_RippleStrength` 0.55 → 0.45, `_FoamDepth` 0.35 → 0.28, `_FoamStrength` 0.8 → 0.68.
- `_Absorption` is the dominant lever: the shader attenuates the refracted bottom by `1 - exp(-thickness * _Absorption)` over the real water thickness, so a lower density is what makes the bottom read further out. It also lowers the composed alpha (`saturate(0.35 + absorb * 0.65)`), which compounds in the same direction.
- The palette is deliberately untouched — `_ShallowTint`, `_DeepTint`, `_ReflectionStrength` and the sun values are not written. The owner approved the colour, so this is a clarity change, not a recolour.
- No shader or asset-bundle rebuild: only material property values change, and all four names were already part of the reference contract, so the binding path is unchanged.
- Every write stays inside the `Range()` the shader declares for its property, is `HasProperty`-guarded, and is Unity-free like the sibling `WaterLookRecipe`. 27 new tests (1 151 in the suite) including a semantic pin that each value moved toward clearer and a guard that the palette stays untouched.
- In-Game-Verify open: whether the step is the right size is the owner's call.

## 0.1.0

- Merged GoldenHour, WaterShader and GraphicsProbe into one mod and one DLL. The old three must not stay loaded beside it.
- Light bias is a mild warm shift instead of the previous gold wash. The clock still peaks at 06:00 and 18:00 and releases at noon and at night.
- The rendering probe no longer captures on startup or scene changes. Ctrl+Shift+F8 still takes a manual report. The old automatic scans were the multi-second hitch.
- **2026-10-18: owner confirmed the new look in-game** (save slot 2, game 0.4.7f12). Log from that session: `5/7 parameters` biased (volume + fog skipped as designed), clock weight 0.31→0.88 across the evening, water bound on `Main/Map/StylizedWater2_Ocean` with the reference shader, clean teardown on quit.
- **2026-10-18, second session: the manual probe capture was exercised** (Ctrl+Shift+F8). `UserData\GraphicsProbe\latest.json` was written (8/8 cameras, 15 682 renderers, 8 retained water candidates of 208 matched, 32 of 119 particle/line/trail matches) and the log confirms the automatic scan stayed off. Cost of one manual capture: **14.64 s wall time for 1 130 ms of active work** (205 481 steps, 1 620 frames) — it is bounded by `stepsPerFrame` (128), not by the wall deadline, so it plays out as ~15 s of degraded frames. Raised `stepsPerFrame` would shorten it; left as-is because the capture is manual-only. The clock curve was re-verified against the policy: all nine logged weights match to two decimals.
- Water look (reflection/shore) remains unverified.
