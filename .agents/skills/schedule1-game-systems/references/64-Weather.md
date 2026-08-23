# Weather & Environment (Schedule I)

## Core Classes

| Class | Purpose |
|-------|---------|
| `EnvironmentManager` | Global environment manager (NetworkSingleton) |
| `DayNightController` | Day-night cycle |
| `WeatherProfile` | Weather profile |
| `WeatherVolume` | Weather volume |

## Time of Day

- `DayNightController`: Controls day-night transition
- `DayNightPhaseTimes`: Phase times
- `SkySettings`: Sky settings
- `SkyProfileFrame`: Sky profile frame
- `SkyState`: Sky state

## Weather System

- `WeatherProfile`: Defines weather conditions
- `WeatherConditions`: Weather conditions
- `WeatherSequence`: Weather sequence
- `WeightedWeatherSequence`: Weighted sequence
- `WeatherVolume`: Weather area (Indoor/Outdoor)

## Weather Effects

| Effect | Controller | Description |
|--------|------------|-------------|
| Rain | `RainController` | Precipitation |
| Thunder | `ThunderController` | Lightning/Thunder |
| Clouds | `CloudController` | Cloud cover |
| Fog | - | Visibility |

## Environment

- `EnvironmentHandler`: Environment handler
- `EnvironmentProfile`: Environment profile
- `WeatherEffectController`: Weather effect controller
- `WeatherEntityHandler`: Entity weather handler
- `ParticleEffectHandler`: Particle effects

## Enclosures

- `BasicEnclosure`: Base enclosure
- `WeatherEnclosure`: Weather-protected area
- `WorldEnclosure`: World enclosure
- `SkyOverrideEnclosure`: Sky override
- `ShadderEffectHandler`: Shader effects

## Integration

- Weather affects plant growth
- Rain makes wet (Vision/Noise changes)
- Night reduces visibility
- Weather affects NPC behavior
- Indoor/outdoor tracking
