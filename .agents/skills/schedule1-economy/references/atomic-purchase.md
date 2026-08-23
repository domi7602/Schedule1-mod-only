# Atomic Purchase (PocketShop Reference)

Complete sequence from `PocketShop/src/Services/PurchaseService.cs:114` (verified v0.2.1, `PocketShop/UI/UITheme.cs` delegates to Shared).

```csharp
// 1. Validate definition + qty + stock
if (item.Definition == null) return DefinitionNull;
if (qty<1) return InvalidQuantity;
int stock = item.CurrentStock >0 ? item.CurrentStock : UnlimitedStockSentinel; // -1 = unlimited:49
if (stock==0) return StockEmpty; if (stock!=-1 && qty>stock) return InvalidQuantity;

// 2. Price
CalculatePricing(item, qty, out perUnit, out _, out _, out total); // perUnit = Price*(1+fee%/100):62
// 3. Affordability — Auto prefers Cash→Bank fallback
if (!CanAfford(total, mode, out effectiveMode)) return NotEnoughFunds; //:72

// 4. Fit check
var probe = item.Definition.GetDefaultInstance(1);
if (!inventory.CanItemFitInInventory(probe, qty)) return NoInventorySpace; //:203

// 5. Pre-create ALL instances BEFORE payment (PocketShop C2 fix)
var instances = new List<ItemInstance>();
for (int i=0;i<qty;i++) instances.Add(item.Definition.GetDefaultInstance(1)); //:218

// 6. Pay — visualizeChange=false to suppress big-screen animation
try {
  if (effectiveMode==Cash) money.ChangeCashBalance(-total,false,false); //:233
  else money.CreateOnlineTransaction($"PocketShop: {item.Name} x{qty}", -total, 1, "PocketShop Order"); //:238
} catch (Exception payEx) {
  // NEVER refund — payment didn't happen :243
  return Error;
}

// 7. Transfer — own try/catch → refund on fail
try { foreach(var inst in instances) inventory.AddItemToInventory(inst); } //:254
catch (Exception ex) {
  if (effectiveMode==Cash) money.ChangeCashBalance(+total,false,false); //:267
  else money.CreateOnlineTransaction("PocketShop Refund", +total, 1, "Rollback"); //:271
  return NoInventorySpace;
}

// 8. Stock decrement only after success
if (stock!=-1) { item.SourceListing.SetStock(Math.Max(0, stock-qty), true); }
return Success; //:297 + SoundService.PlayPurchaseSuccess()
```

BuyResult: Success, NotEnoughCash/Bank/Funds, NoInventorySpace, StockEmpty, InvalidQuantity, DefinitionNull, Error. See `BuyResult:14`.
