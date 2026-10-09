# Packaging (Schedule I)

> UNVERIFIED against the installed runtime. Static identifier sweep 2026-10-08 against the freshly regenerated f12 decompile (`GameReferences/decompiled/Assembly-CSharp`), replacing the earlier decompile-generation check: 64/65 identifier-shaped tokens resolve (0 documented as absent). Unresolved identifiers are listed at the end of this file. Runtime behaviour is not covered by this sweep. Decompiles are IL2CPP interop stubs — hierarchy/signatures verified, method bodies not readable.

## Core Classes (verified)

| Class | Namespace | Hierarchy / Purpose |
|-------|-----------|---------------------|
| `PackagingStation` | `ScheduleOne.ObjectScripts` | `GridItem` — base station, nested `EMode` (Package/Unpackage) + `EState` gate enum |
| `PackagingStationMk2` | `ScheduleOne.Packaging` | `: PackagingStation` — interactive variant, holds a `PackagingTool` |
| `PackagingTool` | `ScheduleOne.Packaging` | MonoBehaviour minigame controller (conveyor, hopper, kick, seal) |
| `FunctionalPackaging` | `ScheduleOne.Packaging` | `: Draggable` — physical packaging item in hand |
| `FunctionalBaggie` / `FunctionalJar` | `ScheduleOne.Packaging` | `: FunctionalPackaging` (each with its own radius/start-point fields on the tool) |
| `FilledPackaging_Equippable` | `ScheduleOne.Packaging` | `: Product_Equippable` — held packaged product |
| `FilledPackaging_StoredItem` | `ScheduleOne.Packaging` | `: StoredItem` — packaged product placed in the world |
| `PackagingDefinition` | `ScheduleOne.Product.Packaging` | `: StorableItemDefinition` — fields `Quantity`, `StealthLevel`, `FunctionalPackaging`, `Equippable_Filled`, `StoredItem_Filled` |

> Note: `PackagingDefinition`/`EStealthLevel` live in the **Product.Packaging** namespace, not `ScheduleOne.Packaging`.

## EStealthLevel (complete)

`None`, `Basic`, `Advanced`

## Process (verified classes)

1. Insert product + packaging into the station's item slots; `EState` gates the run: `CanBegin`, `MissingItems`, `InsufficentProduct` *(sic, game typo)*, `OutputSlotFull`, `Mismatch`, `PackageSlotFull`, `ProductSlotFull`.
2. Server-side conversion via `PackagingStation.PackSingleInstance` (Mk1 auto-packages; Mk2 runs the `PackagingTool` minigame: `InsertIntoHopper`, `UpdateConveyor`, `Kick`, `SealAnim`, `CheckFinalize`).
3. Output is a `FilledPackaging_Equippable` / `FilledPackaging_StoredItem`; the product instance carries `AppliedPackaging` (see [`53-ProductSystem.md`](53-ProductSystem.md)).
4. `EMode.Unpackage` reverses the process.

Stealth values, police-risk effects and mixing context: see [`25-Mixing-Production.md`](25-Mixing-Production.md) (packaging section) and [`51-Police.md`](51-Police.md) — not duplicated here.

## Events

- **None** in `ScheduleOne.Packaging` (no UnityEvents/delegates).
- Slot/config changes flow through FishNet RPCs on the station: `SetStoredInstance`, `SetItemSlotQuantity`, `SetSlotFilter`, `SetSlotLocked`, `SetPlayerUser`/`SetNPCUser` (Server/Observers/Target variants — see [`01-FishNet-Networking.md`](01-FishNet-Networking.md)).
- `ProductEntry` (UI tile in Product namespace) exposes `onHovered`, `onListed` delegates for the product-list UI.

## Save participation

- `PackagingStation` persists via the Persistence layer — `PackagingStationData`, `PackagingStationConfigurationData`, `PackagingStationLoader` exist in `ScheduleOne.Persistence` (station is not `ISaveable` itself; saves via `GridItem`/`BuildableItem` data pipeline, see [`02-Save-Persistence.md`](02-Save-Persistence.md)).
- `PackagingDefinition : StorableItemDefinition` participates in the item-definition save set.

## Hook Points

1. **Prefix `PackagingStation.PackSingleInstance`** — server-side interception: bypass minigame, alter output, force packaging.
2. **Prefix `PackagingTool.CheckFinalize`** — auto-pass the Mk2 minigame or log player input.
3. **S1API:** `S1API.Products.PackagingDefinition` (`Quantity`, `StealthLevel`), `S1API.Products.Packaging.StealthLevel` enum, `CustomProductDefinition.SupportsPackaging(PackagingDefinition)`, `ProductPackagingContentProfile*` APIs. **No station/tool wrapper exists** — patch the native classes for station behavior.

---

---

---

---

---

---

---

---

## Unresolved identifiers (f12 static check 2026-10-08)

These documented identifiers were not found in the f12 game assemblies, the checked-in S1API/S1MAPI source, or the workspace source. Treat them as drift candidates and re-derive them from the current decompiles before relying on this document.

- `Kick`
