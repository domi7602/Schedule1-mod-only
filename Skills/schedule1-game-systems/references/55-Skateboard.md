# Skateboard (Schedule I)

> **Redirect stub** (consolidated 2026-10-05): the deep, live-verified skateboard analysis (PID values, API signatures, Harmony hook point, TaxiDriver spike results) lives in **[`07-Vehicle.md`](07-Vehicle.md)** (skateboard section, "Live-Verified API observations (2026-09-25)" block). This file keeps only the class register. Class-list only — not yet re-verified.
> verified: static identifier sweep 2026-10-08 against the freshly regenerated f12 decompile (`GameReferences/decompiled/Assembly-CSharp`), replacing the earlier decompile-generation check — 14 of 14 identifier-shaped tokens resolve (0 documented as absent; 0 lowercase parameter tokens are out of scope). Static coverage only; runtime behaviour still needs an in-game session.

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
