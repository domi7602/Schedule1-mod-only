# WorldLook

One look mod for Schedule I: clock-weighted light, the reference water surface, and a manual rendering probe.

- Light: Ctrl+Shift+F7. Warm at 06:00 and 18:00, off at noon and at night.
- Water: applies to `Main/Map/StylizedWater2_Ocean` when the Main scene is loaded. The bundle `s1reference.bundle` deploys next to the DLL.
- Probe: Ctrl+Shift+F8 writes `UserData/GraphicsProbe/latest.json`. It does not scan on scene load.

Build, game closed:

```text
dotnet build Source/Mods/WorldLook/src/WorldLook.csproj -c Release
```

History of the three previous mods is under `docs/history/`.

## Source layout

The pure, Unity-free logic lives in six domain bundles; each former per-type file is preserved
verbatim in its own block namespace inside the bundle, so its types and using-directives are
unchanged and an independent reconstruction proof can recover each source byte-for-byte. The
Unity/native adapters stay as separate files.

| Bundle | Absorbed former files |
|---|---|
| `src/Lighting/LightingMath.cs` | `Argb.cs`, `BlendPolicy.cs`, `DaylightPolicy.cs` |
| `src/Lighting/LightingVolume.cs` | `VolumeFlagPolicy.cs`, `VolumeProfilePolicy.cs`, `SnapshotStore.cs` |
| `src/Lighting/LightingCadence.cs` | `ReapplyPolicy.cs`, `TogglePolicy.cs`, `GoldenHourTargets.cs` |
| `src/Probe/ProbeCapturePolicies.cs` | `CaptureSchedule.cs`, `CooperativeCapture.cs`, `FairTraversal.cs`, `RendererScanBudget.cs` |
| `src/Probe/ProbeReportPolicies.cs` | `ReportPolicy.cs`, `IntrospectionPolicy.cs`, `WaterHeuristics.cs` |
| `src/Water/WaterRecipePolicies.cs` | `WaterLookRecipe.cs`, `ReferenceWaterLook.cs`, `ReferenceWaterPolicy.cs`, `WaterSession.cs` |

Native adapters kept separate: `src/Lighting/BaselinePolicy.cs`, `GoldenHourConfig.cs`,
`Lighting/Mod.cs`, `Lighting/SceneTargets.cs`, `Probe/Mod.cs`, `Probe/ObjectReader.cs`,
`Probe/EnvironmentReader.cs`, `Probe/RenderingSnapshot.cs`, `Water/ReferenceWaterRuntime.cs`,
`Water/WaterShaderMod.cs`, `src/WorldLookMod.cs`. The three test projects
(`GoldenHour.Tests`, `GraphicsProbe.Tests`, `WaterShader.Tests`) link the same pure logic and
read the same adapter files as before.
