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
