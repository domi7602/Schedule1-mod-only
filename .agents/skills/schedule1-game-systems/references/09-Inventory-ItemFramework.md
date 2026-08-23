# Inventory/ItemFramework (Schedule I)

## Hierarchy

### Definitions
```
BaseItemDefinition (Core)
  └── ItemDefinition (abstract)
        ├── StorableItemDefinition
        │     ├── QualityItemDefinition
        │     │     └── ProductDefinition (ISaveable)
        │     ├── PropertyItemDefinition
        │     ├── ClothingDefinition
        │     ├── PackagingDefinition
        │     ├── CashDefinition
        │     ├── IntegerItemDefinition
        │     ├── AdditiveDefinition
        │     ├── WaterContainerDefinition
        │     ├── SoilDefinition
        │     ├── ShroomSpawnDefinition
        │     └── SporeSyringeDefinition
        └── BuildableItemDefinition
```

### Instances
```
BaseItemInstance (Core)
  └── ItemInstance (abstract)
        ├── StorableItemInstance
        │     ├── QualityItemInstance
        │     │     └── ProductItemInstance (PackagingID)
        │     ├── CashInstance (Balance)
        │     ├── IntegerItemInstance (Value)
        │     ├── WaterContainerInstance (FillAmount)
        │     └── ClothingInstance (Color)
```

## Quality (EQuality)
| Value | Threshold | Color |
|-------|-----------|-------|
| Trash | < 0.25 | #7D3232 |
| Poor | > 0.25 | #509132 |
| Standard | > 0.4 | #64BEFF |
| Premium | > 0.75 | #E14BFF |
| Heavenly | > 0.9 | #FFC832 |

## PlayerInventory
| Slot | Index | Type |
|------|-------|------|
| Hotbar | 0-7 | HotbarSlot (Items) |
| Clipboard | 8 | ClipboardSlot (Management) |
| Cash | 9 | CashSlot (max 1000) |

- `AddItemToInventory()` → checks stack first, then empty slot
- `StackLimit` (default 10), checks matching ID + Quality + Packaging

## Registry
- `PersistentSingleton<Registry>` with `Dictionary<int, ItemRegister>`
- Hash via `ID.ToLower().GetHashCode()`
- Aliases: `"viagra" → "viagor"`

## ItemFilter (9 Subclasses)
| Filter | Checks |
|--------|--------|
| IDs | Whitelist/Blacklist of IDs |
| Category | EItemCategory |
| LegalStatus | Legal/Illegal |
| ClothingSlot | EClothingSlot |
| Dryable | Products (Weed/Shrooms) |
| MixingIngredient | PropertyItemDefinition |
| PackagedProduct | ProductItemInstance with packaging |
| UnpackagedProduct | ProductItemInstance without packaging |
| ClothingSlot | Clothing slot |

## Equippable Variants
- `Equippable` (base), `Equippable_Viewmodel`, `Equippable_AvatarViewmodel`
- `Equippable_MeleeWeapon`, `Equippable_RangedWeapon`
- `Equippable_BuildableItem`, `Equippable_SurfaceItem`
- `Equippable_Pourable`, `Product_Equippable`

## Storage (World Storage)
- `StorageEntity` with `StorageGrid` (2D grid)
- `StoredItem` (MonoBehaviour) with footprint tiles
