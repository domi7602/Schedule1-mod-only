# Weather & Environment (Schedule I)

> Inherited, not rerun; [evidence scope](README.md#source-history).
> UNVERIFIED against the installed runtime. Static identifier sweep 2026-10-08 against the freshly regenerated f12 decompile (`GameReferences/decompiled/Assembly-CSharp`), replacing the earlier decompile-generation check: 137/142 identifier-shaped tokens resolve (3 documented as absent). Unresolved identifiers are listed at the end of this file. Runtime behaviour is not covered by this sweep.

Namespace: `Il2CppScheduleOne.Weather` — 21 types in the decompile dump.

## Core Classes

| Class | Base | Purpose |
|-------|------|---------|
| `EnvironmentManager` | `NetworkSingleton<EnvironmentManager>` | Weather sequences, volumes, wind, sky, fog, thunder — the whole outdoor environment |
| `WeatherVolume` | NetworkBehaviour | Movable weather zone with blend edges; drives its effect controllers |
| `DayNightController` | MonoBehaviour | Sun/moon/ambient lights, sky evaluation, day-night phases |
| `WeatherEffectController` | EffectController | Base for per-volume effects (blend, audio, particles, shaders) |
| `RainController` / `CloudController` / `ThunderController` | WeatherEffectController | Precipitation, cloud cover, lightning |

Supporting (previously reported): `PuddleVolume`, `EnclosureVisualiser`, `HeightMaskGenerator` (terrain height map), `MaskController` (rain wetness mask + terrain modifications), `MaskModificationData`/`MaskModificationDataGPU`/`MaskModificationCreator`/`EModificationSize`, `WeatherProbe`, `WeatherBasedObjectProvider`, `LensFlareSettings`, `WeatherProfileNetworkSerializer` (FishNet writer/reader), `ParticleEffectHandler`/`ShaderEffectHandler` (EffectHandler base).

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
- The original notes found no direct `ISaveable` implementation in the inspected Weather namespace. That does not exclude persistence through another manager, a base class or saved inputs. Trace the actual load/save path before deciding how chosen weather survives a reload.

## Data Types Not In This Dump
`WeatherProfile`, `WeatherConditions`, `WeatherSequence`, `WeightedWeatherSequence`, `SkyState`, `SkySettings`, `WorldEnclosure`, `WeatherEnclosure`, `SkyOverrideEnclosure`, `IWeatherEntity` are **referenced in EnvironmentManager/WeatherVolume signatures but have no interop wrapper files** in the 2026-10-02 dump — the names appeared in recorded signatures, while their declaring assemblies and member APIs remain to be resolved for the target. Fog has no controller class — handled by an SRP `_fogFeature` inside EnvironmentManager. Old `ShadderEffectHandler` was a typo for `ShaderEffectHandler`; `EnvironmentHandler`/`EnvironmentProfile`/`BasicEnclosure`/`SkyProfileFrame` were **not found** (not located in that snapshot; removal not established).

## Candidate Hook Points
1. **Postfix `EnvironmentManager.SetWeather(string)`** — observe the completed call and inspect resulting state. A postfix cannot prevent side effects that already ran. A veto needs a verified pre-mutation boundary, an explicit decision rule and the correct authority path.
2. **Prefix `EnvironmentManager.SetRandomWeatherSequence()`** — bias or replace the daily weather roll.
3. **Prefix `ThunderController.TriggerRandomPlayerLightningStrike()`** — tune/prevent lightning hitting players.
- **S1API wrapper (previously reported against checked-in source)**: `S1API.Weather.WeatherManager` (static, **read-only**): `Current` (`WeatherState?` snapshot), `OnWeatherChanged` event, `KnownSequenceIds`; `WeatherState` = 9 weights (`Sunny, Cloudy, Rainy, Stormy, Snowy, Foggy, Windy, Hail, Sleet`, 0–1). S1API explicitly does **not** set weather.

## Cross-links
08-Plant-Growing · 62-Vision (night/rain sight) · 58-Temperature · 61-Variables (weather globals) · 17-TimeManager · 13-Regions · 01-FishNet-Networking

---

## Unresolved identifiers in the imported 2026-10-08 record

See [scan limits](README.md#source-history); unresolved does not mean removed.

- `ToHitPlayer`
- `ToHitNPC`
