# Native ocean material evidence and natural-coast recipe

The natural-coast recipe below is historical (0.1.2). Current transparency
candidate and acceptance limits: [Clear Shallows](clear-shallows.md).

## Evidence provenance — 2026-10-17

Read-only inspection of the installed f12 data, not a generic shader preset or a
fresh runtime material snapshot. No game source, shader program, texture or mesh
was exported for distribution. Stored components below have not been converted
between color spaces.

- Installed `Schedule I_Data/level1`: GameObject pathID 24704,
  `StylizedWater2_Ocean`; MeshRenderer pathID 323390 references fileID 3/pathID 837.
  Its external-file table resolves that fileID to `sharedassets1.assets`.
- `sharedassets1.assets`: Material pathID 837, `StylizedWater2_Ocean - Copy`,
  offset 2876744/length 5720. Shader pathID 4332, offset 130986064/length 217604,
  `Stylized Water 2/Standard (Tessellation)`.
- GraphicsProbe's installed report independently identifies
  `Map[0]/StylizedWater2_Ocean[2]` in Main and one shared-material slot.
- Parent independently read the selected serialized material records and confirmed
  the material/shader identities, palette, wave, normal and foam values below.
  Shader-declaration names were checked separately from saved material properties:
  saved obsolete entries are not proof that a shader supports a property.

| Property | Installed vanilla asset | Natural-coast candidate |
|---|---|---|
| `_BaseColor` (Deep) | RGBA (0.010234951, 0.014551431, 0.018867910, 1) | Inherit unchanged |
| `_ShallowColor` | RGBA (0.019607844, 0.036572091, 0.039215688, 1) | RGB (0.025, 0.055, 0.060), preserve existing alpha |
| `_WaveHeight` | 1 | 0.35, unchanged from accepted 0.1.1 |
| `_WaveSpeed` | 1.300000072 | 1.1, unchanged from accepted 0.1.1 |
| `_WaveCount` | 4; shader range 1–5 | 4; removes old mod's out-of-range 6 |
| `_NormalStrength` | 0.165 | 0.18 |
| `_FoamBaseAmount` | 0.317 | 0.25 |
| `_ColorAbsorption` | 0.016 | Inherit unchanged |
| `_DepthVertical` / `_DepthHorizontal` | 7 / 0.01 | Inherit unchanged |
| `_EnvironmentReflectionsOn` | 0 | Inherit unchanged |
| `_PlanarReflectionsEnabled` | 0 | Inherit unchanged |

`_DeepColor`, `_WaveFrequency`, `_WaterColorShallow` and `_WaterColorDeep` are
not declared by this shader; writing them conditionally did not create the claimed
features. Deep color is `_BaseColor`. The new candidate deliberately inherits the
native deep color instead of brightening the whole ocean.

## Natural-coast intent and boundary (historical, 0.1.2)

A restrained coastal look: reduce the prior bright cyan shallow tint, keep dark
water at depth, slightly strengthen existing surface normals, reduce the base foam
coverage, and use an in-range wave count. These are artistic candidate values,
not shader-author recommendations or independently verified visual improvements.

Only the owned material clone is configured once. Retain existing textures,
keywords, refraction/depth settings, foam alpha, wave direction/steepness/distance,
tessellation and lighting. Do not enable new reflection rendering, add cameras,
raise geometry detail or reintroduce global discovery. HasProperty guards remain
mandatory; preserve shallow alpha rather than assuming every target uses 1.

## Acceptance

The owner confirmed 0.1.1 smooth play on 2026-10-17; keep that historical acceptance
separate from the new look. New-look visual acceptance is **In-Game-Verify open**.

After deployment, inspect the `natural-coast` recipe breadcrumb and `Applied` line.
Near the same shoreline at comparable time/weather/camera/settings, check subdued
shallow tint, dark depth, waves, foam and continued smooth play. Also inspect the
horizon at a grazing angle and in sun glare; don't call it realistic based on log
values alone. Menu/reload and shutdown remain separate lifecycle checks. Tests do
not establish GPU/CPU frametimes; no quantitative performance gain is claimed.
