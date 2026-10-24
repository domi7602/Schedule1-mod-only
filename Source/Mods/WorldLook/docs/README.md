# WorldLook

WorldLook combines a mild native-light adjustment with clearer coastal water. The game owns its sky, clouds, sun, moon and stars.

- **Ctrl+Shift+F7:** light and water ON/OFF together for this session; the profile is not rewritten.
- **Ctrl+Shift+F9:** small targeted state report at `UserData/WorldLook/status.json`.
- Light: correct native HHMM conversion; mild warmth peaks at 06:00/18:00 and is neutral at noon/night. Five evidenced color channels, native drift retained, owned writes restored.
- Water: exact `Main/Map/StylizedWater2_Ocean`, native sky/weather inputs, an owned material clone and weak environment-color reflection. The water shader is embedded in `WorldLook.dll`.
- Full GraphicsProbe and the custom cloud-sky prototype are preserved under `Source/Archive/WorldLook/`; F8 has no WorldLook binding.

## Profile

`UserData/WorldLook/WorldLook.json` is read at startup:

| Setting | Default | Meaning |
| --- | --- | --- |
| `Enabled` | `true` | Initial global look state |
| `LightingEnabled` | `true` | Include mild native-light adjustment |
| `WaterEnabled` | `true` | Include clear-water adjustment |
| `Warmth` | `1.0` | Strength of the mild recipe, clamped to 0–1; 0 is neutral |

`Warmth` is a new control; the prototype's `Clouds.LightingStrength` controlled a different effect. Legacy cloud settings are ignored. Existing valid profiles and unknown keys are retained by runtime reads; corrupt/unreadable profiles disable the session's look instead of overwriting the input. The installation procedure preserves the prior profile before supplying the canonical four-setting profile.

## Build

The canonical project is `Source/Mods/WorldLook/src/WorldLook.csproj`. The repository Shared library supplies logging, file storage and scene/input guards; the old standalone helper copies are not compiled.

Full shader + C# build, game closed (compile-only):

```text
pwsh -NoProfile -ExecutionPolicy Bypass -File Source/Mods/WorldLook/build/Build.ps1
```

Use the installed Unity 2022.3.62f3 editor and SDK 8 through the script's path parameters when auto-discovery is unavailable. The game itself runs Unity 2022.3.62f2. The environment-only water shader derives from the preserved 0.2.0 source; dev.2 refines surface detail and sun glints. The build contains only the water asset and its variant collection. Build receipts distinguish compilation from native/GPU acceptance.

The build does not install. Deployment uses `Tools/deploy-mod.ps1` for the candidate DLL/PDB/metadata after a game-closed check. The embedded asset directory is not a loose bundle deployment input.

## Verification and history

- [Prepared in-game acceptance](in-game-acceptance.md): step-by-step dev.2 session, evidence capture, expected state transitions and remaining measurement prerequisites; not an executed test.
- [Water-surface verification](water-surface-verification.md): current dev.2 implementation/check status, backup and manual acceptance.
- [Native-baseline verification](native-baseline-verification.md): historical dev.1 artifact/check status and observed morning/midday comparisons.
- `Source/Tests/GoldenHour.Tests`, `Source/Tests/WaterShader.Tests`, `Source/Tests/WorldLook.Tests`: native-free logic/configuration checks and limited adapter contracts. Their success is not an in-game visual result.
- [Changelog](CHANGELOG.md) and `docs/history/`: historical 0.1.x observations. The standalone 0.2.0 prototype is an archived source reference, not the current build root.

Current candidate remains **In-Game-Verify open** until the native light, water, toggles and load transition have been exercised together.
