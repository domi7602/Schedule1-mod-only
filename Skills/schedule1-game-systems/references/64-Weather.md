# Weather & Environment (Schedule I)
> verified: EnvironmentManager, WeatherVolume, DayNightController, effect controllers re-checked 2026-10-05 against decompiles (generation 2026-10-02; game v0.4.7f9). Anchor: game v0.4.7f9 / S1API 3.2.1-beta.8.

Namespace: `Il2CppScheduleOne.Weather` — 21 types in the decompile dump.

## Core Classes

| Class | Base | Purpose |
|-------|------|---------|
| `EnvironmentManager` | `NetworkSingleton<EnvironmentManager>` | Weather sequences, volumes, wind, sky, fog, thunder — the whole outdoor environment |
| `WeatherVolume` | NetworkBehaviour | Movable weather zone with blend edges; drives its effect controllers |
| `DayNightController` | MonoBehaviour | Sun/moon/ambient lights, sky evaluation, day-night phases |
| `WeatherEffectController` | EffectController | Base for per-volume effects (blend, audio, particles, shaders) |
| `RainController` / `CloudController` / `ThunderController` | WeatherEffectController | Precipitation, cloud cover, lightning |

Supporting (verified): `PuddleVolume`, `EnclosureVisualiser`, `HeightMaskGenerator` (terrain height map), `MaskController` (rain wetness mask + terrain modifications), `MaskModificationData`/`MaskModificationDataGPU`/`MaskModificationCreator`/`EModificationSize`, `WeatherProbe`, `WeatherBasedObjectProvider`, `LensFlareSettings`, `WeatherProfileNetworkSerializer` (FishNet writer/reader), `ParticleEffectHandler`/`ShaderEffectHandler` (EffectHandler base).

## EnvironmentManager API
- Weather control (server): `SetWeather(string)` (virtual final), `SetWeatherSequence(string)`, `SetRandomWeatherSequence()`, `ClearWeather()`, `GetCurrentWeather(out string, out string, out float)`, `GetWeatherProfile(string)` → `WeatherProfile`, `GetWeatherProfileFromPosition(pos)`, `GetActiveWeatherConditionsFromPosition(pos)` → `WeatherConditions`, `HasActiveWeatherVolumes()`
- Data: `_weatherSequences` (`List<WeatherSequence>`), `_dailyWeatherSequences`, `_weatherProfiles`, `_currentWeatherSequence`, `_currentWeatherConditions`, `_currentSkyState`
- Volumes: `CreateWeatherVolumes()`, `MoveWeatherVolumes()`, `DetermineWeatherVolumeWithTarget()`, `CalculateWeatherBlendsFromVolumes()`, `BlendWeatherProfiles()`, blend size + `_blendCurve`, `_defaultWeatherVolumeMoveSpeed`
- Wind: `UpdateWind()`, `ChangeWindDirection()`, `ShiftWindDirection()`, `GetNewWindDirection(angle, range, dir)` (change/shift intervals + angles fields)
- Thunder: `TriggerLightningEvent()`, `TriggerTargetedLightningEvent(Vector3)`, `TriggerDistantThunder()`, `GetActiveThunderController()`
- Sky: `OverrideTimeOfDay(int)` / `ClearTimeOfDayOverride()` (delegates to `DayNightController`); explicit `IEnvironmentManager.SkyState` impl
- Enclosures: `RegisterEnclosure(WorldEnclosure)` / `RegisterWeatherEnclosure(WeatherEnclosure)` / `RegisterOverrideEnclosure(SkyOverrideEnclosure)`, `IsPositionUnderCover(Vector3)`
- Entities: `OnWeatherEntityRegistered/Unregistered(IWeatherEntity)`, `UpdateWeatherEntities()` (tick rate field) — registered entities get weather applied
- Terrain: `GetHeightFromPosition(pos)` / `GetRawHeightFromPosition(pos)`; masks via `SetMaskMapModificationState(MaskModificationData, bool)`; `MaskController.AddHippieModification()`/`RemoveHippieModification()`
- TimeManager subscriptions (17-TimeManager): `OnMinutePass`, `OnTick`, `OnTimeSet`, `OnSleepEnd`; `InitialiseGlobalVariables()` writes weather state into `VariableDatabase` (61-Variables)

## DayNightController & Phases
- `_timeInHours`, sun/moon/ambient lights, `SUN_SHADOW_STRENGTH`/`MOON_SHADOW_STRENGTH`, `MAX_LIGHT_INTENSITY`
- `EvaluateSky(SkyState, SkySettings, ..., blend)` / `BlendSky` / `UpdateSky` / `SetLights(isDay)` / `IsDay(...)`
- Gradient helpers: `EvaluateFloatByTimeOfDay(DynamicGradient)`, `EvaluateColorByTimeOfDay(DynamicGradient)`
- `DayNightPhaseTimes` = `{ MinDawnHour, SunRiseHour, MaxDawnHour, MinDuskHour, SunSetHour, MaxDuskHour }`
- TimeManager handlers: `OnUpdateTime(float)`, `OnTick()`, `OnTimeSet(float)`; time override: `OverrideTimeOfDay(int)` / `ClearTimeOfDayOverride()`

## WeatherVolume & Effects
- `WeatherVolume`: `Initialise(WeatherProfile, sizes..., blend)` (observers RPC), `UpdateVolume()`, `SetAnchor()`, `SetNeighbourVolume()`, `BlendEffects(blend, curve)`, shader/visual-effect parameter setters, corner helpers (`IsInRightHalf`, closest-point queries)
- `ThunderController`: chances `_chanceForLightingStrike` / `...ToHitPlayer` / `...ToHitNPC`, `_maxThunderDelay`, `_timeBetweenThunders`, NPC strike distance; server/client strike RPCs, `TriggerRandomPlayerLightningStrike()`, `TriggerRandomNPCLightningStrike()`
- Handlers: `ParticleEffectHandler` (activate/deactivate, color/numeric/vector params), `ShaderEffectHandler` (same param surface on mesh renderers); audio via `AudioSourceController` wrappers (28-Audio)

## Events / UnityEvents
- **No UnityEvents** in the Weather namespace. Effect changes propagate through `WeatherVolume`/controller methods; thunder uses FishNet RPCs.

## Save Participation
- **None**: no ISaveable in the Weather namespace (EnvironmentManager has no save members). Weather state is re-established on scene load; persistence of chosen weather: unverified.

## Data Types Not In This Dump
`WeatherProfile`, `WeatherConditions`, `WeatherSequence`, `WeightedWeatherSequence`, `SkyState`, `SkySettings`, `WorldEnclosure`, `WeatherEnclosure`, `SkyOverrideEnclosure`, `IWeatherEntity` are **referenced in EnvironmentManager/WeatherVolume signatures but have no interop wrapper files** in the 2026-10-02 dump — names are runtime-real (typed fields/RPC signatures), but member APIs: `unverified`. Fog has no controller class — handled by an SRP `_fogFeature` inside EnvironmentManager. Old `ShadderEffectHandler` was a typo for `ShaderEffectHandler`; `EnvironmentHandler`/`EnvironmentProfile`/`BasicEnclosure`/`SkyProfileFrame` were **not found** (removed).

## Hook Points
1. **Postfix `EnvironmentManager.SetWeather(string)`** — react to / veto weather changes (server-side).
2. **Prefix `EnvironmentManager.SetRandomWeatherSequence()`** — bias or replace the daily weather roll.
3. **Prefix `ThunderController.TriggerRandomPlayerLightningStrike()`** — tune/prevent lightning hitting players.
- **S1API wrapper (verified in 3.2.1-beta.8)**: `S1API.Weather.WeatherManager` (static, **read-only**): `Current` (`WeatherState?` snapshot), `OnWeatherChanged` event, `KnownSequenceIds`; `WeatherState` = 9 weights (`Sunny, Cloudy, Rainy, Stormy, Snowy, Foggy, Windy, Hail, Sleet`, 0–1). S1API explicitly does **not** set weather.

## Cross-links
08-Plant-Growing · 62-Vision (night/rain sight) · 58-Temperature · 61-Variables (weather globals) · 17-TimeManager · 13-Regions · 01-FishNet-Networking
