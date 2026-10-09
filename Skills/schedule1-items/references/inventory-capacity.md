# Inventory Capacity
> verified: static identifier sweep 2026-10-08 against the freshly regenerated f12 decompile (`GameReferences/decompiled/Assembly-CSharp`), replacing the earlier decompile-generation check — 5 of 5 identifier-shaped tokens resolve (0 documented as absent; 0 lowercase parameter tokens are out of scope). Static coverage only; runtime behaviour still needs an in-game session.


Hotbar 0-7 + Cash 9 (1000 per stack) + Clipboard 8.

```csharp
var inv = PlayerInventory.Instance;
var probe = itemDef.GetDefaultInstance(1);
if(!inv.CanItemFitInInventory(probe, qty)) { SoundService.PlayPurchaseDenied(); return NoInventorySpace; }

// Cash capacity (BankApp.cs:57)
int freeSlots = EconomyHelper.GetFreeInventorySlotsCount(); // empty hotbar + CashSlot space
float capacity = EconomyHelper.GetMaxHoldableCashCapacity(1000); // per-slot max
float maxWithdraw = Mathf.Min(onlineBalance, capacity);
```

Always probe `GetDefaultInstance(1)` — don't arithmetic stack math. PocketShop atomic purchase refund handles full inventory: try AddItem → catch → refund: `PocketShop/PurchaseService.cs:264`.

StorageEntity world storage uses `StorageGrid` 2D + `StoredItem` footprint — not slot-limited, but `StorageGrid.TryFitItem(int sizeX, int sizeY, List<Coordinate> lockedCoordinates, out Coordinate originCoordinate, out float rotation)` — there is no `CanFit` member.

---

---
