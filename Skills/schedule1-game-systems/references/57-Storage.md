# Storage (Schedule I)
> verified: static identifier sweep 2026-10-08 against the freshly regenerated f12 decompile (`GameReferences/decompiled/Assembly-CSharp`), replacing the earlier decompile-generation check — 101 of 101 identifier-shaped tokens resolve (0 documented as absent; 0 lowercase parameter tokens are out of scope). Static coverage only; runtime behaviour still needs an in-game session.

Namespace: `Il2CppScheduleOne.Storage` (+ subclasses in `ObjectScripts`, menu in `UI`).

## Core Classes

| Class | Base | Purpose |
|-------|------|---------|
| `StorageManager` | `NetworkSingleton<StorageManager>` | Global manager + `StoredItem` instance pool |
| `StorageEntity` | NetworkBehaviour | Any container with item slots |
| `WorldStorageEntity` | StorageEntity | World-placed, individually saved container |
| `StorageGrid` | MonoBehaviour | Tile grid with footprint placement |
| `StorageTile` | MonoBehaviour | One grid tile; `occupant` (StoredItem) |
| `StoredItem` | MonoBehaviour | Placed visual instance of a stored item (footprint X/Y, rotation, grid) |

`StorageEntity` fields: `MAX_SLOTS`, `SlotCount`, `ItemSlots` (`List<ItemSlot>`), `EmptyOnSleep`, `SlotsAreFilterable`, `DisplayRowCount`, `AccessSettings` (`EAccessSettings`), `MaxAccessDistance`, `CurrentPlayerAccessor`. Key methods: `Open()`, `CanItemFit(ItemInstance, int)`, `HowManyCanFit(ItemInstance)`, `InsertItem(ItemInstance, bool)`, `GetAllItems()`, `GetContentsDictionary()`, `ClearContents()`, `LoadFromItemSet(...)`, `CanBeOpened()`, `SetStoredInstance(...)` (server RPC).

Subclasses in `ObjectScripts`: `PlaceableStorageEntity`, `SurfaceStorageEntity` (both build-grid variants, base class verified in ObjectScripts folder).

`EAccessSettings` = `{ Closed, SinglePlayerOnly, Full }`.

## Storage Grid
- `StorageGrid`: `gridSize`, `RegisterTile`/`DeregisterTile`, `GetTile(coordinate)`, `TryFitItem(...)`, `GetUserEndCapacity()`, `UnoccupiedTileCount`
- `StorageTile`: `SetOccupant(StoredItem)`; `onOccupantChanged` (delegate)
- Items occupy 1..n tiles via footprint (`StoredItem.SizeX/SizeY`)
- `CoordinateStorageTilePair` / `CoordinateStorageFootprintTilePair` (structs): coordinate→tile mapping

## Stored Items
- `StoredItem`: `InitializeStoredItem(StorableItemInstance, StorageGrid, Vector2, float)`, `Destroy()`, `ClearFootprintOccupancy()`
- `StoredItem_GenericBox` (icon rendering), `StoredItemRandomRotation`, `LiquidMeth_Stored`
- `StorableItemInstance : ItemInstance` — item-instance side; `StoredItemPrefab` getter, `GetMonetaryValue()`

## Events / UnityEvents (C# Action events — raised by the listed methods)
| Event | Raised by |
|-------|-----------|
| `onOpened` / `onClosed` | `Open()` → `OnOpened()` / `OnClosed()` |
| `onContentsChanged` | `ContentsChanged()` (insert/load/clear) |
| `onOccupantChanged` (StorageTile) | `SetOccupant()` |

No UnityEvents in the Storage namespace.

## Save Participation
- `StorageManager` implements **ISaveable** (`InitializeSaveable`, `GetSaveString`, load-order member).
- `WorldStorageEntity` implements **ISaveable** per instance: `GUID` (+ `RegenerateGUID`), `GetSaveData()` → `WorldStorageEntityData`, `Load(...)`, `LastContentChangeTime`; static `All` list. Plain `StorageEntity` is **not** individually saveable.
- See 02-Save-Persistence for the folder/file layout.

## Hook Points
1. **Prefix `StorageEntity.CanItemFit(ItemInstance, int)` / `HowManyCanFit`** — custom capacity rules (e.g. backpacks, bigger containers).
2. **Prefix `StorageEntity.InsertItem(ItemInstance, bool)`** — audit/redirect/log stored contraband.
3. **Postfix `StorageEntity.ContentsChanged()`** — react to any storage mutation (robbery detection, quest counters).
- **S1API wrappers (verified against checked-in source)**:
  - `S1API.Storages.StorageManager` (static): `GetAll()`, `FindByName()`, `FindByPredicate()`
  - `S1API.Storages.StorageInstance`: `GetItems()`, `GetContentsDictionary()`, `CanItemFit()`, `AddItem()`, `RemoveItem()`, `TryRemoveQuantity()`, `RemoveAllOfDefinition()`
  - `S1API.Storage.StorageEntity`: `AddSlots()`/`RemoveSlots()`/`SetSlotCount()` (slot expansion, incl. load-time handling), `SyncCustomNameToDisplayName()`
  - `S1API.Storage.StorageEvents`: `StorageEventArgs` / `StorageLoadingEventArgs` (with `NeedsMoreSlots`, `AdditionalSlotsNeeded`)

## UI & Visuals
- `StorageMenu : Singleton<StorageMenu>` (UI namespace) — the open/close container UI.
- `StorageVisualizer` / `StorageEntityVisualizer` — spawn/despawn `StoredItem` visuals from slot contents (`RefreshVisuals`).
- `StorageEntityInteractable : InteractableObject` — opens the entity; `StorageDoorAnimation` — open/close anims + `SetIsOpen`/`OverrideState`.
- `StorageVisualizationUtility` — stacks identical `StorableItemInstance`s for display.

## Not Implemented / Unverified
- No global "automatic sorting" API found — sorting UI behavior lives in `StorageMenu` (unverified).
- No dedicated storage-save events beyond `onContentsChanged`.

## Cross-links
09-Inventory-ItemFramework · 02-Save-Persistence · 47-ObjectStations · 36-Dragging · 01-FishNet-Networking
