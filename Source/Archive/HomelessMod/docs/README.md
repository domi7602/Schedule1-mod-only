# HomelessMod — Street Nomad for Schedule I

Play **Schedule I** without ever buying or renting a property! Transform streets, alleys, and parks into your personal living and production headquarters.

---

## Features

* **Portable Sleeping Bag**:
  * Complete procedural 3D mesh & custom inventory sprite (zero asset bundle overhead).
  * Deploy anywhere in the world from your inventory.
  * **[E]** Sleep through the night (18:00 – 06:00, or anytime if configured).
  * **[F]** or **Hold [Right Mouse Button]** to pack up the sleeping bag back into your inventory.
* **Everywhere Building & Smart Ground Assistant**:
  * Unrestricted building mode everywhere in the city (streets, parks, dark alleys).
  * **5-Point Ground Detection**: Evaluates center and rotated corners with auto-elevation to prevent sinking or floating.
  * **Player & Debris Filtering**: Automatically filters out player capsule to prevent self-blocking when standing close.
  * **Dual-Grid Snapping**: Standard 0.5m grid raster + **Hold [Shift]** for 0.05m ultra-precise free placement.
* **Outdoor Item Dismantling**:
  * Generic outdoor setups (grow pots, drying racks, tables, packaging stations) can be packed up by pressing **[F]** or holding **[RMB]**.
* **Save-Slot Isolated Persistence**:
  * Placed outdoor items are saved atomically per save slot (`street_items_slot_X.json`) with `.bak` crash recovery.
* **Street Nomad Questline**:
  * **Cold Concrete**: Deploy your sleeping bag on the street and sleep through the night.
  * **Alley Operations**: Set up makeshift outdoor production gear and accumulate $500 cash.
  * **Street Sovereign**: Expand your street camp to 3+ outdoor items and amass $5,000 cash.

---

## Controls & Keybindings

| Key / Action | Action | Context |
|---|---|---|
| **[E]** | Sleep in Sleeping Bag | Looking at placed Sleeping Bag (18:00 - 06:00) |
| **[F]** | Instant Pack Up | Looking at placed Sleeping Bag or Outdoor Item |
| **Hold [RMB]** | Dismantle / Pack Up | Looking at placed Sleeping Bag or Outdoor Item |
| **[R] / Scroll** | Rotate Object | In Build Mode |
| **Hold [Shift]** | Free Placement (0.05m Fine Alignment) | In Build Mode |

---

## Console Commands

Open the in-game developer console and use:

* `homeless bag` — Adds 1x portable Sleeping Bag to your inventory.
* `homeless start` — Starts the Street Nomad survival challenge (gives sleeping bag & activates questline).
* `homeless stats` — Displays placed outdoor objects, current balance, and configuration.
* `homeless help` — Displays command overview.

---

## Configuration (`UserData/MelonPreferences.cfg` or `config.json`)

```json
{
  "SleepingBagItemId": "sleepingbag",
  "EnableEverywhereBuilding": true,
  "AllowAnytimeSleep": false,
  "OutdoorGridSnapSize": 0.5,
  "EnablePoliceRisk": false,
  "EnableHomelessQuests": true
}
```
