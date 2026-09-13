---
name: schedule1-items
description: >-
  Item Framework runbook for Schedule I v0.4.6f13 (BaseItemDefinition/Instance, Registry hashing, StackLimit, Inventory slots, ItemFilters). Use when registering custom items, injecting shop listings, patching StackLimit, scanning definitions, or handling inventory capacity (CashSlot, hotbar, StoredItem). Covers StackLimitMod dual-layer scan + Registry hook + field-accessor trap, HomelessMod sleeping-bag injection, PocketShop fit-check.
  Keywords: ItemDefinition, BaseItemDefinition, BaseItemInstance, Registry, GetAllItems, StackLimit, Inventory, PlayerInventory, CashSlot, StackLimitMod, HomelessMod, sleepingbag, GetDefaultInstance, BuildableItemDefinition.
---

> Version anchor: Game v0.4.6f13 / S1API 3.2.0 / MelonLoader 0.7.3 (verified 2026-09-03). Re-check after any game or S1API update.

# Schedule I — Items Skill (ItemFramework / Registry / Inventory)

This skill maps **how items exist, register, stack, and fit** — hierarchy, Registry hashing, stack limits (non-patchable accessor), inventory slots vs world storage, and buildable injections verified in `StackLimitMod`, `HomelessMod`, `PocketShop`, and `BackpackMod`.

> **Version check (verified 2026-09-11):** `Il2CppScheduleOne.ItemFramework`, `Il2CppScheduleOne.Core.Items.Framework`, `Registry` (`PersistentSingleton<Registry>`), `PlayerInventory`. Incorporates dual-layer scan and WasCollected safety rules.

---

## 1. Hierarchy (from Systems/09 + Source-Export/13)

```
Definitions (ScriptableObject):              Instances (runtime):
BaseItemDefinition (StackLimit, ID)          BaseItemInstance (ID, Quantity)
 └─ ItemDefinition (abstract)                  └─ ItemInstance
     ├─ StorableItemDefinition                  ├─ StorableItemInstance
     │   ├─ QualityItemDefinition              │   ├─ QualityItemInstance (Quality)
     │   │   └─ ProductDefinition (ISaveable)  │   │   └─ ProductItemInstance (PackagingID)
     │   ├─ PropertyItemDefinition            │   ├─ CashInstance (Balance)
     │   ├─ PackagingDefinition               │   ├─ IntegerItemInstance (Value)
     │   └─ Cash/Integer/Additive/Water…      │   └─ ClothingInstance (Color)
     └─ BuildableItemDefinition (BuiltItem)    └─ BuildableItemInstance
```

Quality `EQuality`: Trash<0.25 / Poor>0.25 / Standard>0.4 / Premium>0.75 / Heavenly>0.9 `Systems/09:38`.

---

## 2. Registry — Hash + Aliases + Two Layers

```csharp
var reg = Registry.Instance; // PersistentSingleton<Registry>, Pointer!=0 check
// Hash: ID.ToLower().GetHashCode():79 + aliases ("viagra"→"viagor"):60
bool exists = reg.ItemExists(id);           // via hash
ItemDefinition def = reg.GetItem(id);       // null if missing
var all = reg.GetAllItems();                // Il2Cpp IList<ItemDefinition>, for-loop only
```

* **Layer 1 — Resources:** `Resources.FindObjectsOfTypeAll<BaseItemDefinition>()` finds ScriptableObjects in memory (197 items in StackLimitMod log).
* **Layer 2 — Registry:** `registry.GetAllItems()` finds runtime-registered dynamic items (198 after mod items, `Latest.log:17:43:35.170`→`17:43:35.170` 197→198). `StackLimitEngine.ApplyStackLimits:46/69` scans **both** — else newly injected items (e.g. `sleepingbag`) missed.
* **Injection hook:** `StackLimitPatches.Registry_AddToRegistry_Postfix(ItemDefinition item)` `StackLimitPatches.cs:11` — applies limit immediately on `Registry.AddToRegistry` (runtime registration).
* **TOCTOU:** Don't `if (ItemExists) GetItem` — atomic `GetItem` + null check (CustomSkateboard candidate-ID collapse `StackLimitPatches.cs:11` pattern).

---

## 3. Inventory Slots & Capacity

| Slot | Index | Type | Notes |
|---|---|---|---|
| Hotbar | 0-7 | HotbarSlot | General items, respects `StackLimit` |
| Clipboard | 8 | ClipboardSlot | Management UI |
| Cash | 9 | CashSlot | **Max 1000 per stack** — overflow spills to hotbar free slots → capacity math in `BankApp` |
| World Storage | — | `StorageEntity` + `StorageGrid` + `StoredItem` footprint tiles | `StorageGrid` 2D grid |

```csharp
var inv = PlayerInventory.Instance;
bool canFit = inv.CanItemFitInInventory(itemInstance, qty); // PocketShop:203 — probe GetDefaultInstance(1)
inv.AddItemToInventory(instance);        // after payment, inside own try/catch → refund on fail
inv.RemoveAmountOfItem(id, 1u);          // HomelessMod Place deducts source
```

* **Cash withdraw capacity:** `GetMaxHoldableCashCapacity(1000) = CashSlot remaining + freeHotbarSlots*1000` `BankService.cs:57`. Withdraw must check `CanItemFitInInventory` first or bills vanish.
* **Fit probe:** Always `GetDefaultInstance(1)` then `CanItemFitInInventory(probe, qty)` — don't estimate via arithmetic.

---

## 4. StackLimit — Dual-Layer Engine + Not Patchable Accessor

**Goal:** Configurable global limit (1-9999, default 40) across player, storage, shelves, trunks `StackLimitMod/src/StackLimitConfig.cs`.

**Engine `StackLimitEngine.cs:37 ApplyStackLimits`:** scans Resources + Registry `for` loops, calls `ApplyToDefinition:112` which snapshots `_originalLimits[id] = def.StackLimit` once (so `OverrideNonStackable` can check `orig==1` vs new limit), respects `ExcludedItemIds` + `OverrideNonStackable` toggle, then `def.StackLimit = config.StackLimit` inside try.

**Runtime hook + instance guard:**
```csharp
// Postfix after new item registers
[HarmonyPostfix] Registry_AddToRegistry_Postfix(ItemDefinition item) => ApplyToItem(item, Config);

// Instance fallback (always runs, even if definition missed)
[HarmonyPostfix] BaseItemInstance_GetStackLimit_Postfix(BaseItemInstance __instance, ref int __result) {
  if (Excluded.Contains(id)) return; if (!OverrideNonStackable && GetOriginalLimit(id)==1) return;
  __result = Config.StackLimit;
}
```

**Trap 2026-08-21:** `BaseItemDefinition.get_DefaultStackLimit` is a **field accessor** → `Il2CppInterop "can't be patched"` `Latest.log:17:43:04.438` → postfix never fires, 16/16 false-positive. Removed `Mod.cs:109` + `StackLimitPatches.cs:54`. Use scan + instance postfix instead. Verify before patch: `ilspycmd -t BaseItemDefinition`.

---

## 5. Custom Item Injection (HomelessMod Sleeping Bag)

```csharp
// StreetPropertyManager: clone from base "bed" to keep GridItem+footprint
var baseDef = Registry.Instance.GetItem("bed") as BuildableItemDefinition;
var clone = UnityEngine.Object.Instantiate(baseDef); clone.ID = "sleepingbag";
Registry.Instance.AddToRegistry(clone); // triggers StackLimit hook above
// Vendor listing
Handy Hanks Hardware injection: listing.AddItem("sleepingbag");
// Buildable prefab handling: Instantiate(builtItem.gameObject) with wasActive toggle + parent to StreetRoot + DisableNetworking/Navigation: see schedule1-grid 7 Golden Rules
```

* **ID conventions:** lowercase `sleepingbag` / `custom_skateboard` — Registry hash is case-insensitive but canonical is lower.

---

## 6. ItemFilters (9 subclasses, Systems/09:62)

`IDs` (whitelist/blacklist), `Category` (EItemCategory), `LegalStatus`, `ClothingSlot`, `Dryable` (Weed/Shrooms), `MixingIngredient`, `PackagedProduct`/`UnpackagedProduct`. Use for shop filtering, storage rules, recipe matching.

---

## 7. Decision Tree

```
Need to… ?
├─ Change how many fit in a slot? → StackLimit dual-layer (see §4) — don't patch get_DefaultStackLimit
├─ Add new item to game? → Instantiate(BuildableItemDefinition) + Registry.AddToRegistry + vendor listing + snapshot originalLimits
├─ Check if player can carry? → GetDefaultInstance(1) + CanItemFitInInventory(item, qty) + cash-slot math
├─ Find item at runtime? → Registry.GetItem(id) atomic (not Exists→Get)
└─ Understand packaging/quality? → ProductDefinition → Quality + PackagingID → Filter PackagedProduct
```

---

## 8. Verification Checklist

- [ ] Both scans active (Resources + Registry.GetAllItems) for ApplyStackLimits?
- [ ] `Registry_AddToRegistry` postfix subscribed via `PatchGuard.TryPatch` `Mod.cs:95`?
- [ ] `BaseItemInstance.get_StackLimit` postfix as safety net, `get_DefaultStackLimit` **not** patched?
- [ ] `ExcludedItemIds` respected before overwrite? `OverrideNonStackable` prevents 1→40?
- [ ] `CanItemFitInInventory(GetDefaultInstance(1), qty)` before pay?
- [ ] Custom item cloned before modify? `AddToRegistry` after clone?
- [ ] Inventory `RemoveAmountOfItem` / `AddItemToInventory` inside try/catch with rollback?

---

## 9. References

* `references/registry-and-scan.md` — Resources+Registry scan + hook + TOCTOU
* `references/stacklimit-engine.md` — ApplyToDefinition + originalLimits snapshot + prefs
* `references/inventory-capacity.md` — CashSlot 1k, hotbar overflow, fit probe
* In-Repo Architecture: `Skills/schedule1-game-systems/references/09-Inventory-ItemFramework.md`
* Decompiled Game Reference: `GameReferences/decompiled/Assembly-CSharp/Il2CppScheduleOne/ItemFramework/`
* Live Mod Implementations:
  - `Source/Mods/StackLimitMod/src/StackLimitEngine.cs`
  - `Source/Mods/BankApp/src/Services/BankService.cs`
  - `Source/Mods/PocketShop/src/Services/PurchaseService.cs`
  - `Source/Mods/HomelessMod/src/Building/StreetPropertyManager.cs`
  - `Source/Mods/BackpackMod/src/Storage/BackpackStorageManager.cs`
