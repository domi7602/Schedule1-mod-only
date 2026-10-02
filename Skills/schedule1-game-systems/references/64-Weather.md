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

## Gameplay Effects (verified 2026-10-01 against live 0.4.7f7 + wiki)

Rain is the only weather state with an **economic** effect:

| Weather | Effect | Code anchor |
|---|---|---|
| Light Rain | NPCs open umbrellas | `Il2CppScheduleOne.NPCs.Framework.WeatherBehaviour.UseUmbrellaChance` |
| Heavy Rain | wet areas, weather-exclusive NPC dialogue, **"Rainy Weather" bonus added to ALL sales** | `DialogueController.RainyGreetingChance/RainyGreetingThreshold` (key `rainy_greeting`), bonus surfaces as `Contract.BonusPayment{Title, Amount}` through `DealCompletionPopup.PlayPopup(...)` |
| Any rain | NPCs walk slower | `WeatherBehaviour.RainTolerance`, `RainWalkSpeedMultiplier`, `NPCActions._rainySpeedControl`, `WeatherVolume.GetRainAmount` |
| Any rain | ground/soil wetness (cosmetic) | `MaskController._wetGrowthRate/_wetDecayRate`, `SoilDefinition.WetSoilMat` |

Weather sequence ids present in `global-metadata.dat`: `clear`, `lightrain`, `heavyrain`, `lightning` (console command `setweather` selects them).

**Rain does NOT water plants.** `Il2CppScheduleOne.Growing.GrowContainer` has no weather member (verified in the live assembly) — moisture changes only inside `GrowContainer` itself. Do not assume a free-watering loop.

**Temperature has no player-visible effect** (in-game observation + code: the player-facing plumbing is cosmetic/gated). `Il2CppScheduleOne.Temperature` has real members — `AirConditioner : GridItem`, `TemperatureEmitter`, `_cookTemperature` fields, `MushroomBedMoistureDisplay._tooHotIndicator/_temperatureBoostIndicator`, `TemperatureUtility.TemperatureSystemEnabled` — but a large part is explicitly cosmetic (`_cosmeticTemperatureEmitters`, `_cosmeticTileTemperature`, `AllowToggleShowTemperatures`, `DispatchHeatmap`). Never build gameplay on temperature; use rain.

**Weather consumers that react natively** (from `IWeatherEntity` / `OnWeatherChange`): `NPC`, `LandVehicle`, `Wheel`, `Skateboard` — nothing else. Plants, buildings and economy objects do not subscribe to weather.

> Interop-assembly caveat: the generated `Il2Cpp*` assemblies expose *member lists* only — method bodies are native `il2cpp_runtime_invoke` stubs, so call graphs cannot be read from them. Verify behaviour in-game or via `global-metadata.dat` name search.

## Integration

> The bullets below were the earlier curated claims — rows marked ❌ were **refuted** on 2026-10-01 by inspecting the live assembly.

- ❌ "Weather affects plant growth" — no weather member on `GrowContainer`/`Plant`; growth uses moisture + light + temperature only.
- ✅ "Rain makes wet" — but on **NPCs** (`NPC._wetness`, `UpdateWetness`) and the ground mask, not on plants/vision/noise.
- ✅ Night reduces visibility (day-night cycle, `DayNightController`).
- ✅ Weather affects NPC behaviour — rain only: umbrella chance, walk-speed multiplier, rainy greetings.
- 〰 Indoor/outdoor tracking exists via `WeatherEnclosure`/`IsPositionUnderCover(Vector3)`.
