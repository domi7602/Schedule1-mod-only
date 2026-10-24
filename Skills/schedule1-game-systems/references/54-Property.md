# Property (Schedule I)

> Evidence: inherited notes from the supplied Skills.zip. The reported 2026-10-08 static scan was not rerun for this package. Treat symbols, values and behavior as lookup candidates until checked against the target build; [scope and workflow](README.md).
> UNVERIFIED against the installed runtime. Static identifier sweep 2026-10-08 against the freshly regenerated f12 decompile (`GameReferences/decompiled/Assembly-CSharp`), replacing the earlier decompile-generation check: 37/38 identifier-shaped tokens resolve (0 documented as absent). Unresolved identifiers are listed at the end of this file. Runtime behaviour is not covered by this sweep.

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

Original source note (not re-opened for this package): `global-metadata.dat` (installed version) field-default-values + `GameReferences` decompile.

- `ScheduleOne.Property.Manor : Property`, nested `enum EManorState { Original, Destroyed, Rebuilt }`
- Static int constants (default values read out of the metadata blob): `Manor.REBUILD_AFTER_DAYS = 2`, `Manor.REBUILD_DURATION_DAYS = 3` → **5 in-game days total** until the manor is rebuilt and purchasable
- Instance state: `ManorState`, `DaysSinceStateChange` (persisted via `ScheduleOne.Persistence.Datas.ManorData`), `TunnelDug`
- Flow: `Quest_DefeatCartel.Explode`/`Manor.Explode()` → `SetManorState(Destroyed, resetStateChangeTimer: true)` → `Manor.OnSleepEnd()` increments `DaysSinceStateChange` per day transition → `Manor.Rebuild()` (construction container) → `SetManorState(Rebuilt)` → `Ray.NotifyPlayerOfManorRebuild()` sends the "exciting opportunity" text → `Manor.CanBePurchased()` turns true ($250,000 at Ray's Realty)
- Reference values in the same metadata region (sanity checks): `Quest_DefeatCartel.DIG_TUNNEL_COST = 10000f`, `IdealCullsPerSecond = 10`
- Inherited incident report: a stalled rebuild was associated with Thomas Benzies remaining near the driveway. Verify quest state, NPC state and elapsed transitions before choosing a fix. A menu/reload comparison may help reproduce placement behavior on a test copy, but is not proof of the cause or a guaranteed repair.

## Integration

- Each property has a building grid for placement
- Employee management per property
- Power/Water supply
- Security (Alarm, Doors)

---

## Unresolved identifiers in the imported 2026-10-08 record

The imported scan did not resolve the following tokens within its recorded search scope. These may be placeholders, attributes from another assembly or drift candidates. Search the actual declaring assembly and version before concluding that an API was removed.

- `PropertySelector`
