# Skateboard (Schedule I)

> Evidence: inherited notes from the supplied Skills.zip. The reported 2026-10-08 static scan was not rerun for this package. Treat symbols, values and behavior as lookup candidates until checked against the target build; [scope and workflow](README.md).

> **Redirect stub** (consolidated 2026-10-05): the deep, previously reported skateboard analysis (PID values, API signatures, Harmony hook point, TaxiDriver spike results) lives in **[`07-Vehicle.md`](07-Vehicle.md)** (skateboard section, "previously reported API observations (2026-09-25)" block). This file keeps only the class register. Class-list only — not yet re-verified.

## Core Classes

| Class | Purpose |
|-------|---------|
| `Skateboard` | Main skateboard class |
| `Skateboard_Equippable` | Equippable skateboard (hotbar, right-click to mount) |
| `SkateboardAnimation` | Animations |
| `SkateboardCamera` | Camera mods (dynamic FOV, turn tilt) |
| `SkateboardVisuals` | Visual customizations |
| `SkateboardAudio` / `SkateboardEffects` | Sounds / particles |
| `SkateboardData` / `SkateboardOverrideData` / `SkateboardSettings` | Experimental prototype data (`Experimental/`) |

## Related

- Workspace reference mod: `Source/Mods/CustomSkateboard/` (Harmony + ModConfig + Console)
- Physics/PID + hook points: [`07-Vehicle.md`](07-Vehicle.md)
