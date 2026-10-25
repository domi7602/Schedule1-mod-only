# Product System (Schedule I)

> Inherited, not rerun; [evidence scope](README.md#source-history).

> UNVERIFIED against the installed runtime. Static identifier sweep 2026-10-08 against the freshly regenerated f12 decompile (`GameReferences/decompiled/Assembly-CSharp`), replacing the earlier decompile-generation check: 184/190 identifier-shaped tokens resolve (0 documented as absent). Unresolved identifiers are listed at the end of this file. Runtime behaviour is not covered by this sweep. Decompiles are IL2CPP interop stubs — hierarchy/signatures verified, method bodies not readable.

## Core Classes (previously reported)

| Class | Hierarchy | Purpose |
|-------|-----------|---------|
| `ProductManager` | `NetworkSingleton<ProductManager>` | Central registry: recipes, discovery, prices, mix state |
| `ProductDefinition` | `PropertyItemDefinition` | Product definition (save participant) |
| `ProductItemInstance` | `QualityItemInstance` | Concrete product stack in an item slot |
| `ProductList` / nested `Entry` | plain objects | Per-product quantity aggregation |

## EDrugType (complete — 6 values)

`Marijuana`, `Methamphetamine`, `Cocaine`, `MDMA`, `Shrooms`, `Heroin`

**Correction:** there is **no** `LiquidMeth` EDrugType value. `LiquidMethDefinition` + `LiquidMethVisuals` exist as classes, but liquid meth is not an enum entry. MDMA/Heroin have no dedicated `*Definition` classes (mixed-only products, see [`25-Mixing-Production.md`](25-Mixing-Production.md)).

## EProperty (complete — 17 values)

`Mild`, `Potent`, `Overwhelming`, `Sedating`, `Calming`, `Refreshing`, `Stimulating`, `Cerebral`, `Physical`, `Psychedelic`, `Dissociative`, `Hallucinogenic`, `Focused`, `Uplifting`, `Euphoric`, `Addictive`, `HighlyAddictive`

## ProductDefinition (previously reported)

Fields: `BasePrice`, `MarketValue`, `BaseAddictiveness`, `PlayerEffectDuration`, `NPCEffectDuration`, `LawIntensityChange`, `ConsumeAnimation`, `DrugTypes`, `FunctionalProduct`, `ValidPackaging`, `Recipes`. Methods: `GetAddictiveness()`, `GetDefaultInstance()`, `GetPrice()`, `GenerateAppearanceSettings()`, `AddRecipe`, `CleanRecipes`, plus save members (`GetSaveData`, `GetSaveString`, `InitializeSaveable`, `SaveFolderName/FileName`).

## ProductItemInstance (previously reported)

`ProductID`, `Quality`, `Quantity`, `packaging`/`PackagingID`, `AppliedPackaging`. On consumption: `ApplyEffectsToPlayer(Player)` / `ApplyEffectsToNPC(NPC)` (+ `ClearEffectsFrom*`), `GetMonetaryValue()`, `GetAddictiveness()`, `CanStackWith()`, `GetCopy()`. Physical forms: `Product_Equippable : Equippable_Viewmodel`, `Product_Stored : StoredItem`, `FunctionalProduct : Draggable`. Per-type instances: `WeedInstance`, `MethInstance`, `CocaineInstance`, `ShroomInstance` (all `: ProductItemInstance`).

## ProductManager API (previously reported highlights)

- Lookup: `GetRecipe(string product, string mixer)`, `GetRecipe(List<Effect> productProperties, Effect mixerProperty)`, `GetMixerMap(EDrugType)`, `GetKnownProduct(EDrugType, List<Effect>)`, `GetContractReceipts(EMapRegion, List<EContractParty>, int maxMinsAgo)`.
- Pricing: `GetPrice(ProductDefinition)`, `static CalculateProductValue(ProductDefinition, float baseValue)`, `static CalculateProductValue(float baseValue, List<Effect> properties)`, `SetPrice` (RPC), `RefreshHighestValueProduct`.
- Discovery: `DiscoverProduct(string productID)`, `static CheckDiscovery(ItemInstance)`, `SetProductDiscovered(conn, productID, autoList)`; discovery flags exist **only** for `MethDiscovered`, `CocaineDiscovered`, `ShroomsDiscovered` — no weed flag.
- Mixing state: `CurrentMixOperation`, `IsMixingInProgress`, `IsMixComplete`, `FinishAndNameMix`/`SendFinishAndNameMix`, `IsMixNameValid`, `MakeIDFileSafe`, `SetIsAcceptingOrder(bool)`, `TimeSinceProductListingChanged`.
- Listing: `SetProductListed`, `SetProductFavourited` (RPCs).
- Creation (server RPCs): `CreateWeed_Server` / `CreateMeth_Server` / `CreateCocaine_Server` / `CreateShroom_Server` + client-targeted variants, each taking `(name, id, EDrugType, List<string> properties, *AppearanceSettings)`.
- FishNet surface: Server/Observers/Target RPCs for all the above (`RpcWriter/RpcReader/RpcLogic_*`) — server-authoritative ([`01-FishNet-Networking.md`](01-FishNet-Networking.md)).

## Mix & Data Classes

`MixRecipeData` (`Product`, `Mixer`, `Output` — StationRecipe persisted by ProductManager), `NewMixOperation`, `NewMixDiscoveryBox`, `DrugTypeContainer`, `ProductRecipe : MonoBehaviour`, `ProductQuantities` (quantity helpers). Combinatorics/effects live in [`25-Mixing-Production.md`](25-Mixing-Production.md) — not duplicated here.

## Support Systems (previously reported)

- **Visuals:** `ProductVisualsSetter` base; `WeedVisualsSetter`, `MethVisualsSetter`, `CocaineVisualsSetter`, `ShroomVisualsSetter`, `MultiTypeVisualsSetter`; appearance settings `Weed/Meth/Cocaine/ShroomAppearanceSettings`, `LiquidMethVisuals`, `ProductConsumeAnimation`.
- **Icons:** `ProductIconManager : Singleton<ProductIconManager>` — nested `ProductIcon` (`ProductID`, `PackagingID`, `Icon`), `IconGenerator`, `RuntimeProductIconSize`, `Products`/`Packaging` lists.
- **UI:** `ProductEntry : MonoBehaviour` — list tile with `FavouriteButton`, `ListingButton`, `onHovered`, `onListed`, color states.
- **Properties:** `PropertyContainer`, `PropertyMethods`, `PropertyItemDefinition`, `PropertyUtility : Singleton<PropertyUtility>` (`GetPropertyData(EProperty)`, `GetDrugTypeData(EDrugType)`, `GetOrderedPropertyColors(List<Effect>)`), `static DrugTypeMethods` extension methods (`GetName`, `GetColor`, `GetNameWithRichTextColor`).

## Save participation (previously reported)

`ProductManager` and `ProductDefinition` both implement the `InitializeSaveable()`/`GetSaveString()`/`SaveFolderName`/`SaveFileName` pattern (`Loader`, `LoadOrder`, `ShouldSaveUnderFolder`, `LocalExtraFiles/Folders`) — see [`02-Save-Persistence.md`](02-Save-Persistence.md). Instance data travels inside item-stack save data.

## Candidate Hook Points

1. **Postfix `ProductManager.CalculateProductValue`** (static, two overloads) — price/value manipulation for all products.
2. **Prefix `ProductManager.GetPrice`** or postfix `ProductDefinition.GetPrice` — per-product price override.
3. **Postfix `ProductManager.CheckDiscovery` / `DiscoverProduct`** — observe completed discovery. To gate discovery, locate and validate a boundary before the state mutation; a postfix cannot undo already emitted rewards, saves or network events.
4. **S1API:** `S1API.Products.ProductManager` (static: `GetPrice`, `CalculateProductValue`, `SetEffectCallback`/`SetNpcEffectCallback` + clear variants), `ProductDefinition`/`WeedDefinition`/`MethDefinition`/`ShroomDefinition`/`CocaineDefinition`, `WeedDefinitionBuilder`, `CustomProductDefinition` (`Discover`, `SetListed`, `SupportsPackaging`, `CreateInstance(quantity, quality)`), `CustomProductSaveDescriptor`/`CustomProductSaveProviderRegistry` (custom-product save integration), `ProductInstance`, `DrugType`, `Quality`, `MixReactions`, `ProductPopulator.GetPackaging(string)`, `PackagingDefinition` + `StealthLevel`.

---

## Unresolved identifiers in the imported 2026-10-08 record

See [scan limits](README.md#source-history); unresolved does not mean removed.

- `IsMixNameValid`
- `RpcWriter`
- `ClearEffectsFrom`
- `LiquidMeth`
- `RpcReader`
- `RpcLogic_`
