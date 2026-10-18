# WorldLook

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
