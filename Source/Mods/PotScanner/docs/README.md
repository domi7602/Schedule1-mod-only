# PotScanner

**PotScanner** is a comprehensive plant pot monitoring and management mod for *Schedule I* (v0.4.7, IL2CPP). It equips players with an in-game phone application and console terminal bridge to track moisture, soil levels, plant strains, and growth progress across all properties, complete with bulk watering and automated maintenance services.

---

## Key Features

- **In-Game Smartphone Application:** Dedicated `PotScanner` phone app with dynamic responsive UI scaling (`UITheme`) designed for maximum legibility and density.
- **Single-Property Focus Mode (Accordion View):** Tap any property card to expand and inspect only its pots; tap again to collapse back to the clean multi-property overview.
- **Real-Time Multi-Metric Tracking:** High-contrast visual gauges for:
  - **Moisture (W):** Real-time water percentage with low-moisture alerts (< 30%).
  - **Growth (G):** Normalized growth progress percentage and gold harvest-ready highlights.
  - **Soil (S):** Soil availability indicators.
- **One-Click "Water All":** Instantly waters all thirsty player-owned pots for 50g per pot. A pot qualifies (`PotInfo.NeedsWater`) only if it is owned, has a plant, is not fully grown, and sits below the 30% moisture skip threshold — empty and fully-grown pots are skipped and never billed, unowned pots are never touched, and already-moist pots cost nothing.
- **Automated Background Watering (Auto-Water):** Periodic background maintenance service that monitors and waters dry owned pots on a 2-second polling interval with built-in per-pot cooldowns and negative cash balance guards. Same target rules and price as "Water All" (empty and fully-grown pots are never billed). State persists across game sessions.
- **Terminal & Console Integration:** Full integration with the in-game developer console and DooDesch's `hash` phone terminal app via `pot` subcommands (`list`, `summary`, `stats`, `water-all`, `auto`, `refresh`, `help`).

---

## In-Game Phone UI Guide

Open your in-game smartphone and tap the **Pots** icon:

The UI is the flat, dark `S1Mods.Shared.GamePalette` style (BankApp-verified colours and `UISprites`, no emoji glyphs):

```
┌───────────────────────────────────────────────────────────┐
│ SCAN SUMMARY                               ╭────────╮    │
│ 23 POTS                                   │  (donut)│    │
│ 4 ready · 2 thirsty · 0 empty             │  gauge  │    │
│ AVG WATER 82%                             ╰────────╯    │
├───────────────────────────────────────────────────────────┤
│ ┌───────────────────────────┐ ┌─────────────────────────┐ │
│ │        Water All          │ │       Auto-Water        │ │
│ │   2 x 100g                │ │          ON             │ │
│ └───────────────────────────┘ └─────────────────────────┘ │
├───────────────────────────────────────────────────────────┤
│ (All)  Thirsty  Ready  Empty                              │
├───────────────────────────────────────────────────────────┤
│ ▸ Barn                                        4 ready    │
│   ● OG Kush                 Q: 87%                       │
│   W [████████░░] 80%   (tap the water bar to water)      │
│   G [██████████] 100%                                    │
│   S [██████████] 100%                                    │
│ ▸ Bungalow                                       8 pots  │
└───────────────────────────────────────────────────────────┘
```

### Controls & Interactions
- **Hero Summary Card:** Top card with the `SCAN SUMMARY` overline, total pot count, `X ready · Y thirsty · Z empty` meta line, average water level and a donut moisture gauge — all view-only, derived from the same scan data as the list.
- **Water All Card:** Left action card. The sub-label shows the thirsty-pot count and the exact total cost (`N x TOTALg`, 50g per pot). Disabled automatically with the reason as sub-label (`All Moist`, `No Pots`, `No Cash`) or while a watering run is in progress; the solid green fill marks the enabled state.
- **Auto-Water Toggle:** Right action card. The sub-label shows `ON` (green accent) or `OFF` (dim, neutral fill). Click to toggle background automated watering.
- **Filter Chips:** `(All) Thirsty Ready Empty` — the active chip is the blue accent pill; the filter narrows the pot rows below.
- **Property Header:** Displays the property name (e.g., `Barn`, `Motel Room`) and a monochrome status badge (`N ready`, else `N thirsty`, else `N pots`). Clicking toggles between single-property focus and global overview; the chevron rotates to show the state.
- **Individual Pot Row:** Shows the status dot (orange = fully grown, green = growing, muted = empty), plant strain, quality (`Q: n%`), and three progress bars (W water, G growth, S soil) with % badges. Clicking the water bar on a qualifying pot allows direct single-pot watering (same rules and 50g price as Water All).

---

## Terminal & Console Commands

PotScanner registers the `pot` command word with the in-game console and `hash` terminal application:

| Command | Aliases | Description |
| :--- | :--- | :--- |
| `pot summary` | `status`, `sum`, `s` | Prints a clean, compact 1-line overview per property (e.g. `Barn: 12 pots (4 ready, 2 dry, 6 growing) \| Bungalow: 8 pots (8 moist)`). |
| `pot stats` | `stat`, `info`, `st` | Displays global statistics: total pots, owned pots, empty pots, active crops, harvestable plants, thirsty pots, total watering cost, and auto-water status. |
| `pot list` | `ls`, `l` | Displays a detailed list of all pots grouped by property with status, strain name, water %, growth %, and soil %. |
| `pot water-all` | `water`, `w` | Triggers immediate bulk watering for all owned dry pots (50g/pot). |
| `pot auto` | `a` | Toggles the automated background watering service. |
| `pot auto on` / `off` | `enable`, `disable` | Explicitly enables or disables automated background watering. |
| `pot refresh` | `r` | Forces an immediate scene scan and updates the cache. |
| `pot help` | `h`, `?` | Displays the list of available commands and usage examples. |

### Command Examples

```text
> pot summary
PotScanner: Barn: 12 pots (4 ready, 2 dry, 6 growing) | Bungalow: 8 pots (8 moist) | Motel Room: 3 pots (3 ready)

> pot stats
PotScanner Statistics:
  Total pots:           23
  Owned pots:           23 (100%)
  Empty pots:           0
  Plants growing:       14
  Ready to harvest:     7
  Thirsty pots (owned): 2
  Total cost to water:  100g (2 x 50g)
  Auto-Water service:   ENABLED

> pot water-all
PotScanner Water-All: 2/2 pot(s) watered for 100g (21 already watered, skipped)
```

---

## Configuration

Settings are managed via `MelonPreferences` and stored in `UserData/MelonPreferences.cfg`:

```ini
[PotScanner]
AutoWaterEnabled = false
```

- **`AutoWaterEnabled` (bool):** Controls whether background auto-watering is enabled upon game startup. Can be toggled at runtime via the Phone App, console commands, or config edits.

---

## Technical Architecture & Design

1. **Native IL2CPP Pointer Tracking (`IntPtr`):**
   - IL2CPP managed wrapper references are subject to garbage collection decoupling. PotScanner stores the underlying native pointer `GrowContainer.Pointer` in `PotInfo.NativePtr` and compares native addresses, preventing stale object crashes across scene transitions.
2. **Dynamic Responsive Canvas Engine (`UITheme`):**
   - Phone container canvases in *Schedule I* undergo dynamic rotation and variable pixel densities. `UITheme` initializes from container dimensions, calculating `Sp(pt)` font points and `Dp(px)` layouts with dampening (`Scale = Mathf.Clamp(ActualHeight / 900f, 0.75f, 1.20f)`) to guarantee a crisp, non-bloated interface.
3. **0-Allocation In-Place UI Updates:**
   - Background 2-second scans (`PotTracker.Refresh`) detect matching pointer sets and perform in-place updates on cached `PotRowUIRef` structures (`anchorMax` fills and text values), eliminating garbage collection pressure and frame hitches.
4. **Resilient S1API Lifecycle Integration:**
   - Subscribes to `GameLifecycle.OnSaveInfoLoaded` (ensuring `Property.OwnedProperties` is fully parsed prior to scene initialization) and `GameLifecycle.OnLoadComplete` for instant state synchronization.
5. **Exact-Once Financial Validation:**
   - `WaterAllService` executes an atomic validation pipeline, deducting cash per successfully watered pot to prevent duplicate billing.
