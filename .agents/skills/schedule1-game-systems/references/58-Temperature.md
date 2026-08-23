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
