# Save/Persistence (Schedule I)

> Evidence: inherited notes from the supplied Skills.zip. The reported 2026-10-08 static scan was not rerun for this package. Treat symbols, values and behavior as lookup candidates until checked against the target build; [scope and workflow](README.md).
> UNVERIFIED against the installed runtime. Static identifier sweep 2026-10-08 against the freshly regenerated f12 decompile (`GameReferences/decompiled/Assembly-CSharp`), replacing the earlier decompile-generation check: 27/28 identifier-shaped tokens resolve (0 documented as absent). Unresolved identifiers are listed at the end of this file. Runtime behaviour is not covered by this sweep.

## Format
- **JSON** (Unity JsonUtility, `SAVE_FILE_EXTENSION = ".json"`)
- **No compression**, non-binary
- Pretty-Print enabled by default (`PRETTY_PRINT = true`)

## Save Location
```
<PersistentDataPath>/Saves/<SteamID>/SaveGame_<1-5>/
```

## Save Folder Structure
```
SaveGame_X/
├── Metadata.json              # Creation date, version, tutorial flag
├── Game.json                  # OrganizationName, Seed, GameSettings
├── Money.json                 # OnlineBalance, Networth, LifetimeEarnings
├── Time.json                  # TimeOfDay, ElapsedDays, Playtime
├── Player.json                # PlayerCode, Position, Rotation
├── PlayerInventory.json       # ItemSet
├── GenericSaveables.json      # GUID-based mini objects
├── Properties/                # One folder per property
│   ├── Property_1.json
│   └── Property_2.json
├── Trash.json                 # TrashItemData[] + TrashGeneratorData[]
├── Graffiti.json
├── Businesses.json
├── NPCs.json
├── Cartel.json
├── Law.json
├── Vehicles.json
├── Quests.json
├── ProductManager.json
├── Suppliers.json
├── Deliveries.json
├── Messages.json
├── Variables.json
├── Relationships.json
└── <BuildableObject_Folder>/   # One folder PER BuildableItem
    └── Data.json
```

## Scale / Size
- 1–5 MB save folder with 50 plants + 30 crates + 4 dealers + 200 trash
- Each `BuildableItem` = separate folder (`ItemID_<6 GUID characters>/Data.json`)

## Interfaces

| Interface | Purpose |
|-----------|---------|
| `ISaveable` | Main interface. Defines `SaveFolderName`, `SaveFileName`, `Loader`, `GetSaveString()`, `Save()` |
| `IBaseSaveable : ISaveable` | Additionally `LoadOrder` (managers load first) |
| `IGenericSaveable` | GUID-based, key-value storage via `GenericSaveData` |
| `IGUIDRegisterable` | Base: `GUID` field + `SetGUID()` |

## Versioning
- `SaveData.GameVersion` = `Application.version` (the game build string stored in save metadata)
- `SaveData.DataVersion` = overridable via `GetDataVersion()` (default 0)
- Backup for old save: `CreateSaveBackup()` before loading

## Auto-Save
- **NO automatic periodic auto-save**
- Only via bed (SavePoint, 60s cooldown) or F5 (Debug/Editor)

## Loading (3 Phases)
1. **Host**: Load scene → start server → spawn player → load data
2. **Client**: Connect → wait for replication from host (`ReplicationQueue`)
3. `CleanUp()` before every load: `GUIDManager.Clear()`, clear all global lists

## Loader Types
| Type | Examples |
|------|----------|
| `ItemLoader` | ItemLoader, QualityItemLoader, ProductItemLoader, CashLoader, ClothingLoader, etc. |
| `BuildableItemLoader` | PotLoader, MixingStationLoader, TrashLoader, etc. |
| `NPCLoader` | NPCsLoader, EmployeeLoader, BotanistLoader, etc. |

---

## Unresolved identifiers in the imported 2026-10-08 record

The imported scan did not resolve the following tokens within its recorded search scope. These may be placeholders, attributes from another assembly or drift candidates. Search the actual declaring assembly and version before concluding that an API was removed.

- `ItemID_`
