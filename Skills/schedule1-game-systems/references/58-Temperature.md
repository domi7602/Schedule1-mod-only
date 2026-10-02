# Temperature (Schedule I)

## Core Classes

| Class | Purpose |
|-------|---------|
| `TemperatureUtility` | Temperature utilities |
| `TemperatureAlgorithm` | Temperature algorithm |
| `TemperatureEmitter` | Heat / cold source |
| `TemperatureEmitterInfo` | Emitter information |

## Temperature System

- Ambient temperature affects plant growth
- `TemperatureEmitter`: Generates heat/cold
- `AirConditioner`: Air conditioner as an emitter
- Temperature is calculated per grid tile

## Applications

- Grow containers require optimal temperature
- Air conditioner cools rooms
- Temperature affects plant growth speed
- Too hot / cold → Plant suffers

## Algorithm

- `TemperatureAlgorithm`: Calculates temperature changes
- Takes into account emitters, insulation, time of day
- Multi-sampling for stability
- Performance-optimized for many tiles

## Devices

- `AirConditioner`: Cooling / heating device
- Placeable in the building grid
- Energy consumption
- Configurable target temperature

## Verified anchors (2026-10-01, live 0.4.7f7 assembly)

- `Il2CppScheduleOne.Temperature.AirConditioner : GridItem` — placeable, network-synced (`SetCooling/SetHeating/SetOff/SetMode_Server`, `ApplyMode`, `OnModeChanged`), persisted (`AirConditionerData{Mode}`, `AirConditionerLoader`), owns a `TemperatureEmitter` + `TemperatureDisplay` + heat/cool particles and sounds. Static targets `CoolingTemperature` / `HeatingTemperature` (exact °C values not readable from interop stubs — read them off the in-game `TemperatureDisplay`).
- Emitters feed `TemperatureAlgorithm.GetTemperatureAtPoint(ambient, origin, point, emitters)` (S1API: `S1API.Temperature.TemperatureAlgorithm`); beds listen via `MushroomBed.OnTileTemperatureChanged`, pots via `Pot.GetTemperatureGrowthMultiplier()` (`WarmthMinThreshold` / `WarmthMaxThreshold` / `MaxWarmthGrowthMultiplier`). In-world temperature is observable (`GrowContainer._TemperatureDisplay`, heatmap toggle, `MushroomBed.CheckShowTemperatureHint`).
- Pots (weed/coca): growth = base × `TemperatureMultiplier` × `AverageLightExposure` (`GrowContainer.GetAverageLightExposure`) × `GrowSpeedMultiplier` × moisture gate (`Moisture <= 0` → 0). Warmth raises the multiplier within the thresholds.
- Mushrooms gate on cold, not on light: `ShroomColony.MaxTemperatureForGrowth` (= 15, Celsius scale — native temp is Celsius-based with `ToFahrenheit` converters and ambient 20; the "15°F" in S1API's `ShroomColonyInstance` comment is a unit doc bug), `IsTooHotToGrow`, `CheckTemperature`, `MinSoilMoistureForGrowth`, `MushroomBed._mushroomBedColdAtLeastOnce`. `ShroomColony`, `GrowingMushroom` and `MushroomBed` have **zero light members** — no darkness bonus is wired on the mushroom path (verified member lists, live assembly).
- Native temp statics are ints (`TemperatureEmitter.DefaultAmbientTemperature/Min/MaxTemperature`); S1API clamps managed floats 0–40 °C, range 0.1–100 units.
