# WorldLook

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
