# Temperature (Schedule I)
> verified: static identifier sweep 2026-10-08 against the freshly regenerated f12 decompile (`GameReferences/decompiled/Assembly-CSharp`), replacing the earlier decompile-generation check — 65 of 65 identifier-shaped tokens resolve (0 documented as absent; 0 lowercase parameter tokens are out of scope). Static coverage only; runtime behaviour still needs an in-game session.

Namespace: `Il2CppScheduleOne.Temperature` — 5 types in the decompile dump. Small, purely mathematical system.

## Core Classes

| Class | Base | Purpose |
|-------|------|---------|
| `TemperatureEmitter` | MonoBehaviour | Heat/cold source. Fields: `DefaultAmbientTemperature`, `MinTemperature`, `MaxTemperature`, `Temperature`, `Range` |
| `TemperatureEmitterInfo` | struct | Snapshot handed to the algorithm: `Temperature`, `SqrRange`, `Position` (+ `SizeOf` static) |
| `TemperatureAlgorithm` | static class | `GetTemperatureAtPoint(float, float, Vector3, Vector3, TemperatureEmitterInfo[])` —the emission point is a `Vector3` **parameter**, not a type — single public method |
| `TemperatureUtility` | static class | Formatting/conversion helpers |
| `AirConditioner` | GridItem | Placeable heating/cooling device (networked) |

## How It Runs
- **Grid tiles, not rooms**: `Tiles\Tile` stores `_tileTemperature` with `TileTemperature` / `CosmeticTileTemperature` getters; `Tiles\Grid` keeps `_temperatureEmitters` / `_cosmeticTemperatureEmitters` arrays and pushes `TemperatureEmitterInfos` to consumers.
- `Tiles\Tile.onTileTemperatureChanged` (MulticastDelegate `TileChange`) fires when a tile's value changes; `Tiles\Grid` also exposes `OnTemperatureEmittersChanged` / `OnCosmeticTemperatureEmittersChanged`.
- `Heatmap\HeatmapManager` consumes emitters as well (heatmap visualization).
- Consumers query the algorithm per point; emitters must call `NotifyChanged()` after `SetTemperature`/`SetRange`/`SetPosition` so tiles re-cache.

## TemperatureEmitter
- Methods: `SetTemperature(float)`, `SetRange(float)`, `SetPosition(Vector3)`, `EmissionPoint` getter, `NotifyChanged()`
- `OnEmitterChanged` — field of type `Il2CppSystem.Action` (delegate, not UnityEvent), raised by `NotifyChanged()`
- `DefaultAmbientTemperature` / `MinTemperature` / `MaxTemperature` clamp emitter output
- `TemperatureEmitterInfo` order/stride matters for the array overload (`SizeOf`).

## AirConditioner (GridItem)
- Fields: `CoolingTemperature`, `HeatingTemperature`, `TemperatureEmitter`, `TemperatureDisplay`, lights/particles/sounds
- `EMode` = `{ Off, Cooling, Heating }`; `CurrentMode` is a FishNet syncvar
- Server flow: `SetMode(EMode)` → RPC → `SetMode_Server(EMode)` → `ApplyMode()` → `OnModeChanged(old, new, asServer)`
- Helpers: `SetOff()`, `SetCooling()`, `SetHeating()`; `InitializeGridItem(...)`, `GetBaseData()` (build-grid persistence)

## TemperatureUtility
- `TemperatureSystemEnabled` (static bool property)
- `ToFahrenheit(celsius)`, `FormatCelsiusTemperature(c, decimals)`, `FormatFahrenheitTemperature(f, decimals)`, `FormatTemperatureWithAppropriateUnit(...)`, `NormalizeTemperature(celsius)` — display helpers (BoilingFlask temperature canvas uses this style of formatting).

## Events / UnityEvents
| Event | Type | Raised by |
|-------|------|-----------|
| `OnEmitterChanged` (TemperatureEmitter) | `Il2CppSystem.Action` | `NotifyChanged()` |
| `onTileTemperatureChanged` (Tiles\Tile) | delegate (`TileChange`) | tile temperature update |

No UnityEvents in the Temperature namespace.

## Save Participation
- **None**: no ISaveable in the namespace. Temperature state is derived each frame from emitter positions/modes; `AirConditioner` persists only via its GridItem build data (02-Save-Persistence).

## Hook Points
1. **Prefix `TemperatureAlgorithm.GetTemperatureAtPoint(...)`** — global climate mods (seasons, weather chill; pairs with 64-Weather).
2. **Postfix `Tiles\Tile.TileTemperature` getter** — read or offset tile temperature (grow-room simulation).
3. **Prefix `AirConditioner.SetMode_Server(EMode)`** — restrict/log/extend device modes (server-authoritative).
- **S1API wrappers (verified against checked-in source)**: `S1API.Temperature.TemperatureEmitter` (`FromGameObject`, `GetOrAddComponent`, `SetTemperature/SetRange/SetPosition`, `ToInfo`), `TemperatureAlgorithm.GetTemperatureAtPoint`, `TemperatureUtility` formatting, `TemperatureEmitterInfo`.

## Not Implemented / Unverified
- No "insulation" or "time of day" terms exist in the algorithm — ambient value comes only from `TemperatureEmitter.DefaultAmbientTemperature` and neighbouring emitters (old file claim removed as unverifiable).
- No weather→temperature coupling visible in this namespace (unverified whether another system drives emitters from weather).

## Cross-links
56-StationFramework (BoilingFlask heat) · 08-Plant-Growing (grow rooms) · 10-Building-Construction (GridItem placement) · 64-Weather · 02-Save-Persistence

---
