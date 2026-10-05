# Clothing & Fashion (Schedule I)

> verified: classes, enums, save paths and wrapper surface re-checked 2026-10-05 against decompiles (generation 2026-10-02; game v0.4.7f9). Anchor: game v0.4.7f9 / S1API 3.2.1-beta.8.

Namespace: `Il2CppScheduleOne.Clothing` — exactly **7 types** in the decompile dump.

## Core Classes (verified)

| Class | Base | Purpose |
|-------|------|---------|
| `ClothingDefinition` | `StorableItemDefinition` | Wearable item definition: `Slot` (EClothingSlot), `Colorable` (bool), `DefaultColor` (EClothingColor) |
| `ClothingInstance` | `StorableItemInstance` | Wearable instance: `Color` (EClothingColor), ctor `(ItemDefinition, int qty, EClothingColor)`, `GetCopy`, `GetItemData`, `Write(Writer)`/`Read(Reader)` (save), `GetSerializedAvatarObject()` |
| `ClothingConfiguration` | `Configuration<ClothingSettings>` | Per-character clothing config: nested `ColorData {ColorType, ActualColor, LabelColor}`, `ClothingSlotData` |
| `ClothingSettings` | `Settings` | Settings container: `GetSettingsObjects()` → flows through ConfigurationService (32) |
| `ClothingColorExtensions` | static class | EClothingColor helpers |
| `EClothingSlot` | enum | **10 values**: Feet, Bottom, Waist, Top, Outerwear, Hands, Neck, Eyes, Head, Wrist |
| `EClothingColor` | enum | **27 values**: White, LightGrey, DarkGrey, Charcoal, Black, LightRed, Red, Crimson, Orange, Tan, Brown, Coral, Beige, Yellow, Lime, LightGreen, DarkGreen, Cyan, SkyBlue, Blue, DeepBlue, Navy, DeepPurple, Purple, Magenta, BrightPink, HotPink |

**Corrections vs. the old class-list (2026-10-05):**
- `PlayerClothing` does **NOT** live in `Clothing` — it is `Il2CppScheduleOne.PlayerScripts.PlayerClothing` (NetworkBehaviour).
- `ClothingUtility` does **not exist** anywhere in the decompiles (removed).
- Shops: `ClothingShopInterface` / `ClothingShopListing` live in `Il2CppScheduleOne.UI.Shop`.

## PlayerClothing (PlayerScripts — the runtime equipment surface)

`NetworkBehaviour` over the avatar:
- Fields: `List<ItemSlot> ItemSlots`, `Dictionary<EClothingSlot, ItemSlot> _clothingSlots`, `AvatarFramework.Avatar _avatar`
- API: `InsertClothingItem(ClothingInstance)`, `GetClothingSlot(EClothingSlot)`, `RefreshAppearance()`
- Networking: `OnSpawnServer(conn)` + `SetStoredInstance(conn, itemSlotIndex, instance)` → `SetStoredInstance_Internal` (server-authoritative appearance sync)

## Save Participation

- `ClothingInstance.Write/Read` persists color + quantity through the standard item-data path (09-Inventory-ItemFramework).
- Appearance/config state travels via `ClothingSettings`/`ClothingConfiguration` through ConfigurationService (32), and via `SetStoredInstance` RPCs on join.
- No `ISaveable` in this namespace (grep).

## Hook Points

1. **Prefix `PlayerClothing.InsertClothingItem(ClothingInstance)`** — intercept/reject/gate clothing equips (dress codes, custom slots).
2. **Postfix `PlayerClothing.RefreshAppearance()`** — react to visual changes without patching each caller.
3. **Postfix `ClothingInstance..ctor(ItemDefinition, int, EClothingColor)`** — tag/redirect instances by color at creation.
- **S1API wrapper (3.2.1-beta.8): strong.** `S1API.Items.Clothing` + `S1API.Items` provide `ClothingItemDefinition` / `ClothingItemDefinitionBuilder` / `ClothingItemInstance`, `ClothingSlot` / `ClothingColor` + `ClothingSlotMetadata` / `ClothingColorMetadata` / `ClothingMetadataCatalog`, `ClothingApplicationType`, `ClothingItemCreator` — **prefer the wrapper** over raw `Il2CppScheduleOne.Clothing`.

## Cross-links

24-Avatar-CharacterCreator (creator slots/blendshapes) · 32-Configuration (Settings flow) · 09-Inventory-ItemFramework (item hierarchy) · 31-related shops live in `UI/Shop/`
