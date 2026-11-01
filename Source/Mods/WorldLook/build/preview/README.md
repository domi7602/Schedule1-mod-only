# Controlled water GPU preview

Test-only Unity 2022.3.62f3 / URP 14.0.12 / D3D11 scene. The runner copies this
fixture and the existing package pins into a new scratch project. It loads the
**supplied compiled bundle**, renders its actual shader through
`RenderPipeline.SubmitRenderRequest` / `UniversalRenderPipeline.SingleCameraRequest`,
and reads back a linear floating-point GPU target. No water HLSL is reproduced in C#.

From the repository root:

```powershell
pwsh -NoProfile -ExecutionPolicy Bypass -File Source/Mods/WorldLook/build/preview/Run-Preview.ps1 -Label baseline -Bundle .backups/WorldLook/water-detail-20261023-035705/repository/Source/Mods/WorldLook/assets/embedded/worldlook-shaders.bundle
pwsh -NoProfile -ExecutionPolicy Bypass -File Source/Mods/WorldLook/build/preview/Run-Preview.ps1 -Label candidate -Bundle Source/Mods/WorldLook/assets/embedded/worldlook-shaders.bundle
```

Run with labels `A`, `B`, and `C` against the **same** bundle for the controlled
experiment. A uses deep tint (.06,.27,.32) and reflection .12; B changes only
that tint to (.06,.34,.28); C changes only reflection to .138 (+15%). Other
labels use A's settings. The baseline/candidate commands above compare bundles,
not the B/C recipe parameters. Each invocation has its
own Unity process/project and copied bundle, avoiding shared bundle GUID conflicts.
Outputs default to `.backups/WorldLook/water-detail-20261023-035705/preview/<label>-<UTC>/`.
Scratch Unity projects use the shorter ignored `build/logs/p-<id>/` path to avoid
Windows package-import path limits; `inputs.json` records the exact directory.
Existing runs are retained. No production build/deployment is invoked.

## Evidence

- Eighteen 960x540 PNGs: near/far views at elapsed seconds 0, 7.5, 31; two water-disabled
  controls and two exact t=0 repeats; plus near and far night at t=0 and 7.5,
  a shallow water/dry pair, and a deep water/dry pair. The deep control uses a floor at y=-8 with colored cubes off.
  The night uses synthetic low light (sun .05, ambient .025/.03/.04, sky
  .025/.035/.06, horizon .015/.02/.03), not measured game lighting. The shallow
  control raises the checker floor to y=-.15 and disables the colored cubes.
  Same geometry, cameras, light, settings and
  times for every bundle. PNGs are sRGB-encoded/clamped from real linear readback.
- Baseline water uses absorption .12, ripple .28, opacity .22, environment strength .12,
  shallow tint (.46,.52,.55), deep tint (.06,.27,.32), wind .25, wave amplitude .15,
  scale 1.6, speed 1.3, and diagnostic mode 0, matching the recorded live material
  diagnostic. B and C differ only as above. Sun and foam values retain shader defaults;
  `images/preview.json` records every resulting material property.
- A test-only render pass at `BeforeRenderingTransparents - 1` supplies `_Time`
  and related clock globals **after** URP camera setup. Both t=0 repeats must have
  identical PNG hashes. The pass execution count is checked for every render.
- Float readback rejects observed NaN/Infinity, error-magenta and uniform frames.
  Each water view must differ from its dry control. These are execution checks,
  not aesthetic thresholds. Shader/render errors and logged compiler/import
  exceptions fail the run (licensing-client startup diagnostics remain in the log).
- `inputs.json` identifies the copied bundle and fixture/runner hashes. A sibling
  `shader-build.json`, when present, must identify that same bundle; its shader
  source hash is recorded as build provenance, not as a separately rendered asset.
  `images/preview.json` includes GPU/API/package/settings and PNG hashes;
  `unity-preview.log` retains compiler/render diagnostics.

The scene uses a submerged checker floor and colored cubes with synthetic neutral
daylight colors. It is **not the native game**, a game-sky reconstruction, a shader
benchmark, all-variant coverage, or proof of smooth movement/flicker-free play.
Owner in-game acceptance remains separate. Compare matching filenames only when
the fixture hash and recorded scene/settings agree.

For the ripple experiment, run `python Source/Tests/WaterShader.Tests/test_surface_shade.py`
separately: it interprets the production HLSL function's scalar expressions with
controlled derivatives; it does not compile the shader or model GPU derivative
quads. It is not currently included in `dotnet test`. The optional
`compare_wave_preview.py BASELINE_IMAGES CANDIDATE_IMAGES` requires Pillow 13+
for 8-bit sRGB signed and absolute differences in two fixed side rectangles. Those
rectangles avoid the central glint but are not a semantic water-only mask; images
and color-space clipping still require inspection. Finite patches, perspective,
nonuniform illumination and changing footprints do not guarantee zero mean in
final composed pixels, even if the analytic sine layers have zero mean over
complete wave periods at fixed cross-wave coordinate and footprint.
