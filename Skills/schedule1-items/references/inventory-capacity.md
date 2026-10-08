# Inventory Capacity
> UNVERIFIED for runtime 0.4.7f11 — slot indexes + capacity facts carried over (last reflected check 2026-08-22). Anchor: 0.4.7f9-era evidence; runtime 0.4.7f11 per workspace AGENTS.md.


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

StorageEntity world storage uses `StorageGrid` 2D + `StoredItem` footprint — not slot-limited, but `StorageGrid.CanFit(item, rotation)` analog.
