# Lighting (Schedule I)

## Core Classes

| Class | Purpose |
|-------|---------|
| `UsableLightSource` | Usable light sources |
| `LightExposureNode` | Light exposure for plants |
| `PoliceLight` | Police strobe light |
| `VolumetricLightTracker` | Volumetric light |

## Light Types

| Type | Description |
|------|-------------|
| `BlinkingLight` | Blinking light |
| `FlickeringLight` | Flickering light |
| `LightTimer` | Timer-controlled light |
| `UsableLightSource` | Toggleable (On/Off) |
| `ToggleableLight` | UI-controlled light |

## Grow Lights

- `GrowLight`: Plant illumination
- `LightExposureNode`: Calculates light for plants
- Essential for indoor growing
- UV spectrum simulation

## Reflection Probes

- `ReflectionProbeUpdater`: Updates reflection probes
- Performance-optimized
- Only updated as needed

## Miscellaneous

- `PoliceLight`: Red/blue strobe lights on police vehicles
- `VolumetricLightTracker`: For god ray effects
- Integration with weather/environment
