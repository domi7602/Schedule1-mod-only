# Minimap v2.0.2

High-performance customizable Minimap & Unified DayCounter HUD for **Schedule I v0.4.7** (TVGS, IL2CPP, MelonLoader 0.7.3).

## Features

- **Dual-Shape Viewport** — Circular Radar or Rounded-Square Tactical GPS with anti-aliased procedural textures.
- **Integrated DayCounter HUD** — Day number, weekday and 24h/12h time displayed as a header bar, footer bar, or standalone pill (map hidden).
- **Dynamic Rotation** — Follow-Player mode (map rotates, you see N at the top) or North-Up mode (fixed north, player arrow rotates).
- **Live Zoom** — 0.75×–4.0× via console or `[` / `]` hotkeys.
- **0-Allocation Pooled Blip Engine** — Pre-allocated pool of 64 blips with fast scene scans (Police, Active Deals, Potential Customers, Known Customers, Recruited Dealers, Properties, Shops, Quests, Vehicles, Sleeping Bags) plus user Waypoints.
- **Recruited Dealer Blips** — every recruited dealer (`DealerManagementApp.dealers`) is always shown with a dedicated dealer icon and clamped to the map edge regardless of range.
- **User Waypoints** — named waypoints with per-waypoint hex colour, drawn as pulsing diamond blips and always edge-clamped; managed via `minimap wp ...`, persisted per save slot (`waypoints_<slot>.json`, max 16).
- **Police Heat Ring** — second outline ring around the map, coloured and pulsing by the current pursuit level (None / Investigating / Arresting / Non-Lethal / Lethal).
- **Health Bar** — slim HP bar under the minimap, colour-coded green / amber / red.
- **Minimap Settings PhoneApp** — in-game settings app with a toggle per blip type (plus waypoints and health bar) and per-category hex colour fields, applied live to the blip renderer.
- **Range Filtering & Edge Clamping** — Per-config max entity range (default 140m). Critical blips (active deals, quests, properties) always clamp to the edge when out of view.
- **Drag & Drop** — Free positioning when cursor is unlocked (menu/phone/pause). Boundaries auto-clamped.
- **Persistent Configuration** — Anchor, position, blip visibility, shape, zoom, time format, all persisted in `UserData/Minimap/config.json` via `SafeStorage`.
- **Dev Console + Hash Terminal** — Full S1API `BaseConsoleCommand` integration (`minimap`/`map`) auto-discovered by DooDesch `hash` terminal.

## Hotkeys

| Key | Action |
|-----|--------|
| `M` | Toggle HUD visibility |
| `]` / `Keypad +` | Zoom in |
| `[` / `Keypad -` | Zoom out |
| Mouse drag (cursor unlocked) | Move HUD |

Hotkeys are skipped when an input field is focused.

## Console Commands

```
minimap toggle                          - Toggle HUD
minimap shape <circle|rounded|square>   - Change map shape
minimap rot <follow|north>              - Rotation mode
minimap zoom <0.75..4.0>                - Set zoom level
minimap size <140..350>                 - Set minimap dimension (px)
minimap day <header|footer|standalone|off>  - DayCounter mode
minimap pos <topright|topleft|bottomright|bottomleft|reset>
minimap blips <police|deals|potential|customers|properties|shops|quests|vehicles|clamp> <on|off>
minimap wp <add|del|list|clear> [name] [hex]  - Manage user waypoints
minimap status                          - Live settings dump
minimap help                            - Command list
```

Aliases: `map` is a shortcut for `minimap`.

## Configuration

Persisted in `UserData/Minimap/config.json` (atomic write + `.bak` backup).

Key fields:
- `Shape` — `Circle` / `RoundedSquare` (default) / `Square`
- `RotationMode` — `FollowPlayer` (default) / `NorthUp`
- `DayMode` — `Header` (default) / `Footer` / `Standalone` / `Hidden`
- `MapSize` — 220 px default
- `ZoomLevel` — 1.75× default
- `BorderColorHex` — `#3BD882` (Emerald accent)
- `MaxEntityRange` — 140 m
- `EnableEdgeClamping` / `ClampOnlyCritical` — clamp blips to map edge
- `ShowHealthBar` / `ShowCompassRing` — HUD extras
- 10 individual blip filter flags (including dealers and waypoints)

Related files in `UserData/Minimap/`: user waypoints (`waypoints_<slot>.json`, per save slot) and the blip colour palette (`blip_colors.json`).

## Cross-Mod Integration

- **HomelessMod** — Sleeping bag blips are resolved via reflection on `HomelessMod.Building.StreetPropertyManager.ActiveStreetObjects`. If HomelessMod is not loaded, the blip is silently skipped.
- **DooDesch hash** — `minimap` and `map` are registered via `Hash.Api.HashCommands.Add(...)` for tab-completion and help.

## Compatibility

- S1API 3.2.1-beta.8 (handled via `Directory.Build.props`)
- Inherits `Shared.dll` (SafeStorage, ModLogger, HotkeyManager)
- IL2CPP (Mono would require `Skill` plugins — not supported)
