# WorldLook

WorldLook combines a mild native-light adjustment with clearer coastal water. The game owns its sky, clouds, sun, moon and stars.

- **Ctrl+Shift+F7:** light and water ON/OFF together for this session; the profile is not rewritten.
- [Direct MCP control](mcp-automation.md): explicit ON/OFF, status and menu transitions for automated acceptance with screenshots.
- The F9 development status dump is removed in dev.3. Existing reports are historical evidence and are retained.
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

- [Acceptance status and remaining checks](in-game-acceptance.md): owner-accepted water/ON-OFF, remaining WorldLook evidence limits and the preserved dev.2 diagnostic protocol.
- [Water-surface verification](water-surface-verification.md): current dev.2 implementation/check status, backup and manual acceptance.
- [Native-baseline verification](native-baseline-verification.md): historical dev.1 artifact/check status and observed morning/midday comparisons.
- `Source/Tests/GoldenHour.Tests`, `Source/Tests/WaterShader.Tests`, `Source/Tests/WorldLook.Tests`: native-free logic/configuration checks and limited adapter contracts. Their success is not an in-game visual result.
- [Changelog](CHANGELOG.md) and `docs/history/`: historical 0.1.x observations. The standalone 0.2.0 prototype is an archived source reference, not the current build root.

**Water and ON/OFF are owner-accepted and closed (2026-10-24, dev.2).** The dev.3 cleanup
removes debug output without changing the shader, recipe or normal toggle behavior. It was
installed and hash-verified on 2026-10-24; installation alone did not establish gameplay acceptance.
The dev.3 session on 2026-10-25 measured repeatable Sun tint and restored ON lighting after
reloading Dominik; the owner accepted normal night appearance. Dev.4 was installed on
2026-10-25 and its agent-run MCP checks preserved OFF and ON through menu/reload, including
measured light behavior and seven automatic screenshots; its shader fallback remains historical
evidence. Dev.5 repairs shader-cache lifetime with the same bundle/recipe. On 2026-10-25,
four automated Dominik reloads (two ON, two OFF followed by ON) passed actual shader/support
readbacks, with ten scene/menu screenshots and unchanged profile/native saves. The reload
defect is closed for these tested paths. Visible warmth and measured performance remain
separate limits; the accepted water appearance stays closed. See the acceptance record for
the initial cold-load transport failure and limits of the scene screenshots.
