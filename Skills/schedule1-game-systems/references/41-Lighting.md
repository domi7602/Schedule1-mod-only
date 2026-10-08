# Lighting (Schedule I)
> verified: classes + fields + methods + save-participation re-checked 2026-10-05 against decompiles (generation 2026-10-02; game v0.4.7f9). Anchor: 0.4.7f9-era evidence; runtime 0.4.7f11 per workspace AGENTS.md.

Namespace: `Il2CppScheduleOne.Lighting` — plain MonoBehaviour components, **no manager/singleton**.

## Core Classes

| Class | Base | Purpose |
|-------|------|---------|
| `UsableLightSource` | `MonoBehaviour` | Data holder consumed by plant exposure: `GrowSpeedMultiplier`, `isEmitting` |
| `LightExposureNode` | `MonoBehaviour` | Aggregates light for plants: `ambientExposure`, `sources` |
| `LightTimer` | `MonoBehaviour` | Day-time on/off window for light groups |
| `BlinkingLight` | `MonoBehaviour` | Blink pattern light |
| `FlickeringLight` | `MonoBehaviour` | Intensity/color/position flicker |
| `PoliceLight` | `MonoBehaviour` | Red/blue strobe + siren (police vehicles) |
| `VolumetricLightTracker` | `MonoBehaviour` | Beam/dust volumetric effect state |
| `LensFlareDisabler` | `MonoBehaviour` | Disables lens flares beyond `RefreshDistance`/`threshold` |
| `ShadowLODController` | `MonoBehaviour` | Soft/hard shadow distance LOD for `_lights` |
| `ReflectionProbeUpdater` | `MonoBehaviour` | Queued, throttled reflection-probe rendering |

## Key APIs (verified)
- `LightExposureNode`: `GetTotalExposure(out float growSpeedMultiplier) → float`, `AddSource(UsableLightSource, float lightAmount)`, `RemoveSource(UsableLightSource)`, `OnDrawGizmos()` — the plant-growth light pipeline; `UsableLightSource` only carries `GrowSpeedMultiplier`/`isEmitting`.
- `LightTimer`: `StartTime`, `EndTime`, `StartTimeOffset`, `toggleableLights` (`List<Misc.ToggleableLight>`); `Awake()`, `Start()`, virtual `UpdateState()`, `SetState(bool on)`.
- `PoliceLight`: `SetIsOn(bool)`, `FixedUpdate()`, `CycleCoroutine()`; fields `IsOn`, `RedMeshes`/`BlueMeshes`, `RedLights`/`BlueLights`, `Siren`, `CycleDuration`, on/off materials (`RedOnMat` …), `RedBrightnessCurve`/`BlueBrightnessCurve`, `LightBrightness`.
- `BlinkingLight`: `IsOn`, `OnTime`, `OffTime`, `light`, `blinkRoutine` (+ `Blink` coroutine).
- `FlickeringLight`: `minIntensity`/`maxIntensity`, `enableColorShift` (`minColor`/`maxColor`), `flickerSpeed`, optional movement (`_enableFlickerMovement`, `_flickerMovementRange`, `_flickerMovementSpeed`), `lightSource`; `UpdateTargetValues()`, `UpdateMovement()`.
- `VolumetricLightTracker`: `Override`/`Enabled` props, `light`, `optimizedLight`, `beam`, `beamHD`, `dust`; `UpdateEffectsState()`.
- `ShadowLODController`: `RefreshMovementThreshold`, `_softShadowEnabled`, `_softShadowDistance`/`_hardShadowDistance` (+sqr), `UpdateShadows()`, `RecalculateDistances()`.
- `ReflectionProbeUpdater`: `Probe`, `renderQueue`, `RenderRoutine`, `UpdateProbe()`, `ProcessQueue()` coroutine.

## Namespace Corrections (vs. older docs)
- `ToggleableLight` is **not** in Lighting — it's `Il2CppScheduleOne.Misc.ToggleableLight` (`isOn` prop, `TurnOn()`/`TurnOff()`/`SetLights()`, `lightSources` list of `UsableLightSource`, on/off materials, `state`).
- `GrowLight` is `Il2CppScheduleOne.ObjectScripts.GrowLight : ProceduralGridItem` (`Light`, `usableLightSource` fields, `SetIsOn(bool)`, `InitializeProceduralGridItem(...)`) — furniture/save side lives there, not here.

## Events / UnityEvents
- **None.** No UnityEvents, no C#-event fields, no `Il2CppSystem.Action` fields in the namespace (grep). State changes are polled (`Update`/`FixedUpdate`) or pushed via `SetState`-style methods.

## Save Participation
- **No `ISaveable` / `SaveData` in the Lighting namespace** (grep). Light on/off state persists via the owning buildables (e.g. `GrowLight` as `ProceduralGridItem`, doors/lamps as `ToggleableLight` consumers), not via these components.

## Hook Points
1. **Postfix `LightExposureNode.GetTotalExposure(out float)`** — modify computed grow-light exposure (custom lamps, buff/debuff growth) — note the `out float` is filled before the postfix runs, so overwrite the outer arg; plain method, patchable.
2. **Postfix `LightExposureNode.AddSource(...)` / `RemoveSource(...)`** — track which lamps feed which plant nodes (light auditing, quest logic).
3. **Prefix `LightTimer.SetState(bool)`** — force/override scheduled light windows (curfew blackouts, custom timers).
- **S1API (3.2.1-beta.8): no Lighting wrapper** (grep over S1API source — no `UsableLightSource`/`LightExposureNode` references).

## Not Implemented / Unverified
- No "UV spectrum" system found in the decompiles — old doc claim removed; exposure is a single float pipeline (`unverified` whether art-side shaders add more).
- Weather/environment integration with these components: `unverified` (no references inside the namespace).
- `UsableLightSource` has no logic methods beyond its ctor in the wrapper — emission toggling likely happens via the field from consumers (e.g. `ToggleableLight.lightSources`); exact writers `unverified`.

## Cross-links
17-TimeManager · 12-Heat-Pursuit-Law · 47-ObjectStations · 09-Inventory-ItemFramework
