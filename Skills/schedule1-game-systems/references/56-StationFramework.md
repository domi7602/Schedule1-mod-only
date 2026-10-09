# Station Framework (Schedule I)
> verified: static identifier sweep 2026-10-08 against the freshly regenerated f12 decompile (`GameReferences/decompiled/Assembly-CSharp`), replacing the earlier decompile-generation check — 105 of 105 identifier-shaped tokens resolve (0 documented as absent; 0 lowercase parameter tokens are out of scope). Static coverage only; runtime behaviour still needs an in-game session.

Namespace: `Il2CppScheduleOne.StationFramework` — 17 types in the decompile dump.

## Core Classes

| Class | Base | Purpose |
|-------|------|---------|
| `StationItem` | MonoBehaviour | Base for station **items**. Fields: `Modules` (`List<ItemModule>`), `TrashPrefab`. `ActiveModules` property. Methods: `Initialize(StorableItemDefinition)`, `ActivateModule<T>()`, `HasModule<T>()`, `GetModule<T>()`, `Destroy()` |
| `StationRecipe` | ScriptableObject | Recipe asset: inputs, product, cooking parameters (see below) |
| `ItemModule` | MonoBehaviour | Module on a station item: `Item` (owner), `IsModuleActive`, virtual `ActivateModule(StationItem)` |
| `Fillable` | MonoBehaviour | Liquid reservoir: `contents` (`List<Content>`), `AddLiquid(name, volume, color)`, `GetLiquidVolume(name)`, `GetTotalLiquidVolume()`, `LiquidCapacity_L` |
| `BoilingFlask` | Fillable | Cook vessel: `Burner` (`BunsenBurner`, ObjectScripts), temperature sim (`CurrentTemperature` + velocity, `TEMPERATURE_MAX`, `OverheatScale`), `Recipe` property, canvas + smoke visuals |

## StationRecipe API
- Fields: `RecipeTitle`, `IsDiscovered`, `Unlocked`, `Ingredients` (`List<IngredientQuantity>`), `Product` (`ItemQuantity`), `FinalLiquidColor`, `CookTime_Mins`, `CookTemperature`, `CookTemperatureTolerance`, `QualityCalculationMethod`
- `EQualityCalculationMethod` = `{ Additive }` — only one value exists
- Bounds: `CookTemperatureLowerBound` / `CookTemperatureUpperBound` (derived from `CookTemperature` ± tolerance; exact formula unverified)
- Nested types: `ItemQuantity { Item, Quantity }`, `IngredientQuantity { Items (List<ItemDefinition>), Quantity, Item getter }`
- Methods: `DoIngredientsSuffice(List<ItemInstance>)`, `CalculateQuality(List<ItemInstance>)` → `EQuality`, `GetProductInstance(List<ItemInstance>)`, `GetProductInstance(EQuality)`, `RecipeID` getter (derivation unverified)

## Module Classes
| Module | Fields of interest |
|--------|--------------------|
| `IngredientModule` | `Pieces`; `ActivateModule` spawns ingredient pieces |
| `PourableModule` | `LiquidType`, `PourRate`, `LiquidCapacity_L`, `LiquidContainer`, `OnlyEmptyOverFillable`; virtual `CanPour()`, `PourAmount()`, `ChangeLiquidLevel()` |
| `CookableModule` | `CookTime`, `CookType` (`ECookableType`), `Product`, `ProductQuantity`, `ProductShardPrefab`, `LiquidColor`/`SolidColor` |

Supporting: `LiquidContainer` (level + color mesh control), `LiquidLevelVisuals`, `LiquidVolumeCollider`, `IngredientPiece` (dissolve-over-time in liquid), `PourableAngleLimit`.

## Station Types
| Station | Class | Base |
|---------|-------|------|
| Mushroom spawn (container) | `MushroomSpawnStation` | **GridItem**, not StationItem |
| Mushroom spawn (bag item) | `MushroomSpawnStationItem` | StationItem |
| Spore syringe | `SporeSyringeStationItem` | StationItem |
| Liquid meth flask | `LiquidMeth_StationItem` | StationItem |
| Product wrapping station | `ProductStationItem` | StationItem |

`MushroomSpawnStation` is the networked container behind the mushroom growing workflow: `GrainBagSlot`/`SyringeSlot`/`OutputSlot`, `Use()`/`OnEndUse()`, `SetPlayerUser`/`SetNPCUser`/`SetConfigurer` (server RPCs), `SetItemSlotQuantity`/`SetSlotLocked`/`SetSlotFilter` (server RPCs), `GetSaveData()`.

## Events / UnityEvents
| Event | Type | Raised by |
|-------|------|-----------|
| `onUse`, `onUseEnded` | `UnityEvent` (`MushroomSpawnStation`) | `Use()` / `OnEndUse()` |
| `onCapRemoved`, `onInserted` | `UnityEvent` (`SporeSyringeStationItem`) | `RemoveCap()` / `InsertSyringe()` |
| `onPlungerMoved` | `UnityEvent<float>` (`SporeSyringeStationItem`) | plunger input |

No UnityEvents on `StationItem` / `StationRecipe` / `BoilingFlask`.

## Save Participation
- `MushroomSpawnStation` saves via its GridItem/property data path (`GetBaseData`/`GetSaveData`) — see 02-Save-Persistence.
- `StationRecipe.IsDiscovered`/`Unlocked`: no ISaveable on `StationRecipe` in this dump; persistence path unverified (S1API wraps both flags via `SetAvailability`).

## Hook Points
1. **Prefix `StationRecipe.CalculateQuality(List<ItemInstance>)`** — inject custom quality formulas (S1API mirrors this via `QualityCalculationMethod`).
2. **Prefix `StationItem.Initialize(StorableItemDefinition)`** — inject custom modules into stations after item init.
3. **Prefix `BoilingFlask.SetTemperature(float)`** — override the flask heat curve (pairs with 58-Temperature).
- **S1API wrapper (verified against checked-in source)**: `S1API.Stations.ChemistryStationRecipes.CreateAndRegister(builder => ...)` / `Register(recipe)` / `GetAll()` — wraps a native `StationRecipe`; `ChemistryStationRecipe.SetAvailability(isDiscovered, isUnlocked)`.

## Not Implemented / Unverified
- No station "progress" or "process time" manager class exists in StationFramework — cooking time is owned by `BoilingFlask`/`CookableModule` consumers (25-Mixing-Production).
- `RecipeID` derivation and recipe discovery persistence: unverified.

## Cross-links
08-Plant-Growing · 25-Mixing-Production · 47-ObjectStations · 09-Inventory-ItemFramework · 58-Temperature · 02-Save-Persistence
