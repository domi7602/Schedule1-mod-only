# CustomSkateboard

Adds the **Pro Cyber Skateboard** to *Schedule I*: a custom high-performance board with its own tuned physics (carving, push, jump), anti-gravel suspension, a custom deck model with neon underglow, and purchase through **Jeff Gilmore**'s skateboard shop dialogue. Vanilla boards are left untouched.

## Features

- **Store integration:** the board is injected into Jeff Gilmore's `DialogueController_SkateboardSeller` options (default price `$1,500`, configurable). Injection is idempotent — reloads do not create duplicate entries.
- **Tuned riding physics (runtime, per board instance):** higher top speed and progressive push acceleration, sharper carving (`TurnForce`, `TurnChangeRate`, `TurnReturnToRestRate`, lateral friction), configurable board lean, instant jump with a short, clamped jump duration and a forward boost on take-off. Applied in `OnMount` to the custom board only; vanilla boards (Golden Skateboard, Cruiser, …) keep their values.
- **Anti-gravel suspension:** the custom board's `SlowOnTerrain` flag is set from `DisableTerrainSlowdown` (default on), so grass, gravel and dirt no longer slow it down. (Earlier Harmony prefixes on terrain smoothness were removed in 2026-08 because the flag alone has the same effect.)
- **Custom visuals:** deck mesh swapped from `assets/models/custom_skateboard.obj` (generated with `assets/generate_skateboard.py` in Blender; `.blend` and `.mtl` included) plus a cyan neon underglow. Renderer filtering is strict (deck/board meshes only), so player hair, eyes and clothing are never touched.
- **Console commands** (S1API console, command word `skate`): `skate give` (adds the board to the inventory after verifying the item is registered), `skate stats` (prints the live board parameters), `skate help`.
- **Resilience:** `PatchGuard`-guarded patches, cached `AnimationCurve`/`Gradient`/material instances (no per-frame allocations, `sharedMaterial` to avoid VRAM leaks), configuration validation with clamping (e.g. `TopSpeed_Kmh` is clamped to 5–300 km/h and reset to 100 if invalid).

## Requirements

- Schedule I (IL2CPP) — verified on v0.4.6f13 (2026-09-15)
- MelonLoader 0.7.3
- S1API 3.2.x (`Mods\S1API.Il2Cpp.MelonLoader.dll`)
- `Shared.dll` (`S1Mods.Shared`, included in the release ZIP)

## Installation

Extract the release ZIP over the game directory: `Mods\CustomSkateboard.dll`, `Mods\icon.png` and `Mods\Shared.dll`; `UserData\CustomSkateboard\mod.json` (+ `.pdb`). Never place `mod.json` or `.pdb` files in `Mods\`. See the [repository README](https://github.com/domi7602/Schedule1-mod-only#installation) for the generic steps.

## How to get the board in-game

- **Buy it:** talk to Jeff Gilmore (Downtown skateboard seller) and pick the *Pro Cyber Skateboard* option in his shop dialogue.
- **Console:** open the in-game console and run `skate give`.

## Configuration

Settings are stored through MelonPreferences in the `[CustomSkateboard]` category of `<GameDir>\UserData\MelonPreferences.cfg` (created on first start). Defaults from `SkateboardConfig.cs`:

| Setting | Default | Meaning |
|---|---|---|
| `SkateboardId` / `SkateboardName` | `custom_skateboard` / `Pro Cyber Skateboard` | Item id and display name |
| `Price` | `1500` | Shop price at Jeff Gilmore |
| `BaseItemIdOverride` | `""` | Force a specific vanilla base item id (empty = auto-detect) |
| `TopSpeed_Kmh` | `140` | Top speed; clamped to 5–300 |
| `PushForceMultiplier` / `PushForceDuration` / `PushCooldown` | `6.5` / `0.35` / `0.18` | Push strength, duration and cooldown |
| `LongitudinalFrictionMultiplier` / `BrakeForce` | `0.13` / `2.5` | Rolling resistance and braking |
| `TurnForce` / `TurnChangeRate` / `TurnReturnToRestRate` | `20` / `85` / `75` | Carving response |
| `TurnSpeedBoost` / `LateralFrictionForceMultiplier` | `2.5` / `1.85` | Turn boost and lateral grip |
| `MaxBoardLean` / `BoardLeanRate` | `33` / `75` | Visual deck lean (degrees / rate) |
| `JumpForce` / `JumpDuration_Min` / `JumpDuration_Max` / `JumpForwardBoost` | `18` / `0.45` / `0.70` / `2.4` | Jump tuning |
| `AirMovementEnabled` / `AirMovementForce` | `false` / `0` | Optional air control |
| `DisableTerrainSlowdown` | `true` | Anti-gravel suspension |
| `AutoInjectToJeffGilmore` | `true` | Store injection on/off |

Values are validated on load; out-of-range numbers are clamped or reset (see `SkateboardConfig.Validate`).

## Compatibility

Verified in-game on Schedule I v0.4.6f13 (2026-09-15, v1.1.5). Not yet re-verified on the v0.4.7 Open Beta; the beta's avatar refactor may affect the visual patches (see [`docs/compatibility.md`](https://github.com/domi7602/Schedule1-mod-only/blob/main/docs/compatibility.md)).

## Known issues

- None open for v1.1.5. Earlier audit rounds (20 findings) are documented in [`CHANGELOG.md`](CHANGELOG.md).

## Development

```pwsh
dotnet build Source/Mods/CustomSkateboard/src/CustomSkateboard.csproj -c Release
```

Source layout: `src/Mod.cs` (entry, config bootstrap), `SkateboardConfig.cs`, `SkateboardItemFactory.cs` (item definition), `SkateboardSellerInjector.cs` (dialogue injection), `SkateboardVisualPatches.cs` + `CyberSkateboardVisualizer.cs` (deck swap, underglow), `ObjLoader.cs` (runtime OBJ loading with winding correction), `SkateboardConsoleCommand.cs`. The 3D model is regenerated with `assets/generate_skateboard.py` (Blender).

Author: Dominik · License: MIT (see repository `LICENSE`).
