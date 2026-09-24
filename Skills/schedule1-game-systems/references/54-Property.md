# Property (Schedule I)

## Core Classes

| Class | Purpose |
|-------|---------|
| `PropertyManager` | Global property manager (NetworkSingleton) |
| `Property` | Base property class |
| `Business` | Business property |
| `BusinessManager` | Business manager |

## Property Types

| Type | Class | Description |
|------|-------|-------------|
| RV | `RV` | Starter RV |
| Motel Room | `MotelRoom` | Motel room |
| Bungalow | `Bungalow` | Bungalow |
| Manor | `Manor` | Large manor |
| Sewer Office | `SewerOffice` | Sewer office |
| Sweatshop | `Sweatshop` | Sweatshop |

## Property Functions

- `PropertyContentsContainer`: Container for contents
- `PropertyDisposalArea`: Disposal area
- `Tap`: Water tap
- Each property has its own loading dock

## Business

- `Business`: Business property (front business)
- `BusinessManager`: Business management
- `LaunderingOperation`: Money laundering operation
- Generate income through legal businesses

## Purchase & Rent

- Properties are purchased or rented
- `PropertyDropdown`: Property selection UI
- `PropertySelector`: Property selector
- Different prices depending on type/location

## Hyland Manor — rebuild after Cartel defeat ("Finishing the Job")

Source of truth: `global-metadata.dat` (installed version) field-default-values + `GameReferences` decompile.

- `ScheduleOne.Property.Manor : Property`, nested `enum EManorState { Original, Destroyed, Rebuilt }`
- Static int constants (default values read out of the metadata blob): `Manor.REBUILD_AFTER_DAYS = 2`, `Manor.REBUILD_DURATION_DAYS = 3` → **5 in-game days total** until the manor is rebuilt and purchasable
- Instance state: `ManorState`, `DaysSinceStateChange` (persisted via `ScheduleOne.Persistence.Datas.ManorData`), `TunnelDug`
- Flow: `Quest_DefeatCartel.Explode`/`Manor.Explode()` → `SetManorState(Destroyed, resetStateChangeTimer: true)` → `Manor.OnSleepEnd()` increments `DaysSinceStateChange` per day transition → `Manor.Rebuild()` (construction container) → `SetManorState(Rebuilt)` → `Ray.NotifyPlayerOfManorRebuild()` sends the "exciting opportunity" text → `Manor.CanBePurchased()` turns true ($250,000 at Ray's Realty)
- Reference values in the same metadata region (sanity checks): `Quest_DefeatCartel.DIG_TUNNEL_COST = 10000f`, `IdealCullsPerSecond = 10`
- Known beta bug: if nothing happens after the wait, Thomas Benzies was never actually killed by the explosion (he can get stuck in the driveway) — save → main menu → reload fixes the NPC placement

## Integration

- Each property has a building grid for placement
- Employee management per property
- Power/Water supply
- Security (Alarm, Doors)
