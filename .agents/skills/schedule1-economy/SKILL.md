---
name: schedule1-economy
description: >-
  Economy runbook for Schedule I v0.4.6f13 (Money, Businesses, Shops, Customers, Laundering). Use when implementing purchases, bank transfers, passive income, inventory capacity, weekly ATM limits, or multiplayer-safe economy logic. Covers BankApp double-entry, PocketShop multi-payment + atomic purchase, BusinessIncome host authority + snapshot revert.
  Keywords: Money, MoneyManager, onlineBalance, cashBalance, Business, OwnedBusinesses, ShopInterface, PurchaseService, BankService, TransactionHistory, weekly limit, cash slot, Auto payment, CreateOnlineTransaction, ChangeCashBalance.
---

# Schedule I — Economy Skill (Money / Business / Shop)

> **Knowledge guard (mod-only):** `Knowledge/` is absent in this workspace. Before using `Knowledge/...` paths, `Test-Path Knowledge/` — fallback is `D:\Backup\game source` (`bundleVersion 0.4.5f2 Alternate`, ~1 version behind `v0.4.6f13`, 66k files, structure-only). Verify any decompile hit against live `Assembly-CSharp.dll` via `ilspycmd` / S1MCP before patching.

This skill is the **runbook for every economy interaction** in Schedule I — cash vs bank, price + fees, inventory capacity, weekly ATM limits, passive daily payouts, and multiplayer-safe transaction ordering. It codifies the patterns verified in `BankApp v0.1.0`, `PocketShop v0.2.1`, `BusinessIncome v0.1.0`.

> **Version check (last verified: 2026-08-21):** Game v0.4.6f13, S1API 3.2.0, `S1API.Money` + `Il2CppScheduleOne.Money.MoneyManager`. If the game patched, verify `MoneyManager` signatures with `ilspycmd`.

---

## 1. Two Ledgers (Core Mental Model)

| Ledger | Where | Access | Slot |
|---|---|---|---|
| **Cash** (physical) | `PlayerInventory → CashSlot (idx 9, max 1000 per slot) + hotbar overflow` | `MoneyManager.Instance.cashBalance` / `ChangeCashBalance(delta, visualizeChange, playSound)` | Needs free slots for `ChangeCashBalance(+amount)` |
| **Bank** (online) | `MoneyManager.onlineBalance` | `CreateOnlineTransaction(title, amount, days, note)` | Unlimited, but weekly ATM deposit limit applies |

**Truth:** Cash is *inventory*, not a float. `BankApp` `BankService.cs:55` calculates holdable capacity via `GetMaxHoldableCashCapacity(maxPerSlot)`. Forgetting the slot check = **cash loss** (withdraw destroys bills).

---

## 2. Decision Tree — What Pattern Do I Need?

```
Need economy feature?
│
├─ Player buys something in a phone/app shop?
│  → PocketShop atomic purchase: multi-payment + pre-create instances + refund-rollback (see §4)
│
├─ Player moves cash ↔ bank (ATM)?
│  → BankApp double-entry: cash/bank duality + weekly limit + slot capacity (see §5)
│
├─ Passive daily revenue for owned businesses?
│  → BusinessIncome: host authority + snapshot revert + deterministic variance (see §6)
│
├─ Auto-pay supplier debt (Cash / Bank / Both)?
│  → AutoPaySuppliers: SupplierDebtAccess.ChangeDebt + Re-entry guard + Relationship gain (see §7)
│
├─ Price display with fees?
│  → perUnit = basePrice * (1 + fee%/100); total = perUnit*qty + deliveryFlat (PocketShop PurchaseService.cs:62)
│
└─ Customer / laundering / business unlock?
   → Knowledge/Analysis/Systems/23-Economy-Money.md + 11-Business-Laundering.md + 19-Customer-Budget.md
```

---

## 3. The 4 Non-Negotiable Rules

1. **Never refund if payment never executed.** PocketShop `PurchaseService.cs:243` — payment `try/catch` returns without refund if `ChangeCashBalance/CreateOnlineTransaction` threw before deduction.
2. **Refund on undelivered inventory.** Transfer `AddItemToInventory` in own `try/catch` → on fail, **refund** via opposite ledger (`+total` cash or `+total` bank) `PurchaseService.cs:264`.
3. **Host-authority for passive income.** `BusinessIncome/Services/IncomeEngine.cs:24` `IsHostOrSingleplayer()` — clients must never book; else 2-4× payout in co-op.
4. **Weekly ATM limit is vaulted separately.** `BankApp` `BankService.cs:17` `VanillaWeeklyAtmLimit=10000`, enforced only if `RespectVanillaAtmLimit`, tracked per in-game week `GetCurrentInGameWeek() = ElapsedDays/7` + `TransactionHistoryService.GetWeeklyDeposits(week)`.

---

## 4. Atomic Purchase (PocketShop — Reference Pattern)

**Order (see `references/atomic-purchase.md`):**

```
Validate → Price → CanAfford(Cash|Bank|Auto) → CanItemFitInInventory → Pre-create ALL ItemInstances
→ Pay (cash ChangeCashBalance(-total) OR bank CreateOnlineTransaction(-total)) → Transfer AddItemToInventory loop
→ On transfer fail: refund via opposite ledger → Fail → Decrement Stock → Success
```

* **Multi-Payment Switcher:** `PocketShop/UI` chips `[💵 Cash] / [💳 Bank Card] / [⚡ Auto]` — Auto prefers cash, falls back to bank `PurchaseService.cs:92`.
* **Stock Sentinel:** `UnlimitedStockSentinel = -1` `PurchaseService.cs:49` — unlimited stock skips decrement.
* **Sound + UI:** `SoundService.PlayPurchaseSuccess/Denied`, `PlayCashSuccess` from `MoneyManager.Instance.PlayCashSound()` (null-checked).

---

## 5. ATM & Double-Entry (BankApp — Reference Pattern)

**Read balances:**
```csharp
float cash = MoneyManager.Instance.cashBalance;        // physical
float bank = MoneyManager.Instance.onlineBalance;      // bank
float net  = cash + bank + BusinessAssets;             // net worth (BankApp dashboard)
```

**Capacity before withdraw:**
```csharp
int free = EconomyHelper.GetFreeInventorySlotsCount(); // hotbar empty + CashSlot space
float capacity = EconomyHelper.GetMaxHoldableCashCapacity(maxPerSlot); // 1000 per slot default
float maxWithdrawable = Mathf.Min(onlineBalance, capacity); // BankService.cs:60
```

**Deposit (`BankService.cs:118`):** `ChangeCashBalance(-amount)` **then** `CreateOnlineTransaction("ATM Deposit", netCredited)` — on exception, refund `ChangeCashBalance(+amount)`.
**Withdraw (`BankService.cs:188`):** `CreateOnlineTransaction("ATM Withdrawal", -totalDeducted)` **then** `ChangeCashBalance(+amount)` — on exception, refund `CreateOnlineTransaction(+totalDeducted)`.
Fee `fee = amount * ServiceFeePercent/100`, `netCredited = amount-fee`.

**Weekly limit:** `Remaining = 10000 - GetWeeklyDeposits(currentWeek)` `BankService.cs:72` — tracked via `TransactionHistoryService: slot_{n}.json` (`BankState`).

---

## 6. Passive Revenue (BusinessIncome — Reference Pattern)

**Deterministic net:** `gross = BaseIncome * Multiplier(prop)` * `variance ±15% hash(ElapsedDays, BusinessId)` + `employeeBonus 5%/emp cap 25%` + `weekendBonus 25%` * `OperatingCosts 10%` → `net = gross - costs` (see `BusinessIncome/src/Services/RevenueCalculator.cs`).

**Ordering + Snapshot (2026-08-21 fix):**
```csharp
var ids = lines.Select(l=>l.BusinessId).ToList();
PayoutStateStore.MarkInMemoryPaid(day, ids); // snapshots prevLastPaid + per-business Dict:20
try { Money.CreateOnlineTransaction("Business Revenue", +net, 1f, note); }
catch { PayoutStateStore.RevertInMemoryPaid(day, ids); return false; } // restores snapshot:149, NOT -1
PayoutStateStore.CommitPayout(day, ids); // SaveAtomic slot_{n}.json + clears snapshot:183
```

* **Idempotency:** `IsDayPaid(day) => LastPaidElapsedDay >= day` + `LastPaidDayByBusiness[id]` per-business. Slot-isolated `payout_state_slot_{n}.json` `PayoutStateStore.cs:60`.
* **Notification:** `NotificationsManager.Instance.SendNotification("Business Revenue", "+$X from N businesses", null, 5f, playSound)` + optional `PlayCashSound`.

---

## 7. Supplier Payments (AutoPaySuppliers — Reference Pattern)

> **Source:** distilled from decompiling `AutoPaySuppliers 1.0.0` (Nexus). Verified 2026-08-26. No workspace mod currently implements this — pattern parked here for future use.

**The flow** (`AutoPaySuppliers/AutoPaySuppliersMod.cs:60-111` `TryPaySupplierDebt`):

```
IsEnabled && Debt > 0 && no re-entry guard (isAutoPaying)
  → pick source (Cash / Card / Both)
  → read cashBalance + onlineBalance from MoneyManager
  → amount = min(Debt, availableFunds)
  → set re-entry flag isAutoPaying = true
  → try {
      ApplyPayment(source, amount, cash, supplier);   // cash first, then bank
      SupplierDebtAccess.ChangeDebt(supplier, -amount);
    } finally { isAutoPaying = false; }
  → if RelationData != null && MaxOrderLimit > 0:
      RelationData.ChangeRelationship(amount / MaxOrderLimit * 0.5f, true);
```

**Multi-source payment** (`ApplyPayment`):

```csharp
float remaining = amount;
if (source != Card) {
    float fromCash = Mathf.Min(remaining, cashBalance);
    if (fromCash > 0) {
        MoneyManager.Instance.ChangeCashBalance(-fromCash, showCashChange, playSound);
        remaining -= fromCash;
    }
}
if (remaining > 0f && source != Cash) {
    MoneyManager.Instance.CreateOnlineTransaction("Supplier Payment", -remaining, 1f, supplier.fullName);
}
```

Cash first, then bank — preserves physical cash for emergencies, uses the bank to top up.

**Key APIs not in S1API directly:**

| API | Source | Purpose |
|---|---|---|
| `MoneyManager.Instance.cashBalance` | `Il2CppScheduleOne.Money.MoneyManager` | Read physical cash |
| `MoneyManager.Instance.onlineBalance` | same (use `sync___get_value_onlineBalance()` in IL2CPP) | Read bank balance |
| `MoneyManager.Instance.ChangeCashBalance(delta, visualize, playSound)` | same | Adjust physical cash |
| `MoneyManager.Instance.CreateOnlineTransaction(title, amount, days, note)` | same | Adjust bank + log transaction |
| `Supplier.Debt` (get/set) | `Il2CppScheduleOne.Economy.Supplier` | Outstanding debt |
| `Supplier.MaxOrderLimit` | same | Order cap (denominator for relationship gain) |
| `SupplierDebtAccess.ChangeDebt(supplier, delta)` | extension on Supplier | Safe debt mutation (no direct setter on Supplier) |
| `((NPC)supplier).RelationData.ChangeRelationship(delta, notify)` | `Il2CppScheduleOne.NPCs.NPC` + `RelationData` | Affect NPC mood/disposition |
| `NPCManager.NPCRegistry` | `Il2CppScheduleOne.NPCs.NPCManager` | Iterate all NPCs, filter for `Supplier` |

**Relationship gain rule of thumb:** gain = `(paidAmount / MaxOrderLimit) * 0.5f`. Caps at 0.5 per full MaxOrderLimit payment — never gives a huge mood swing per transaction.

**Re-entry guard:** `isAutoPaying` flag is critical — `TryPaySupplierDebt` can be called from multiple paths (auto-trigger, manual click, config-change hook). Without the guard, debt is paid N times for one trigger.

**Preference pattern** (`AutoPaySuppliersMod.cs:142-156`): `SubscribeToPreferenceChanges` + `RefreshCachedPreferences` — cache enum/string values as their parsed type, not raw string, so the hot path doesn't re-parse on every call. Mirror `ModConfig<T>` from `Shared` if available.

**Hook points to call `TryPaySupplierDebt`:**
- On `OnUpdate` poll (every 1-2s) if `IsEnabled`
- On player NPC interaction end
- On day tick (ElapseDay callback)
- On `MelonPreferences` change for `Enabled` (auto-pay-all on enable: `PayAllLoadedSupplierDebts` enumerates `NPCManager.NPCRegistry` and casts each to `Supplier`)

**Always null-check `Supplier.Debt` and `MoneyManager.Instance` — they may be null early in scene load.**

---

## 8. Common Pitfalls

| Symptom | Cause | Fix |
|---|---|---|
| Cash lost on withdraw | No slot check → bills deleted | `GetMaxWithdrawableCash()` before tx `BankService.cs:55` |
| Double payout in co-op | No `IsHostOrSingleplayer()` | Wrap in `IncomeEngine.IsHostOrSingleplayer()` `IncomeEngine.cs:24` |
| Day 5 re-pays after Day 6 fail | `Revert` set `LastPaid=-1` (pre-2026-08-21) | Snapshot restore `PayoutStateStore.cs:149` |
| Refund creates money | Refunded before payment ran | Guard: only refund if payment succeeded `PurchaseService.cs:243` |
| Weekly limit ignored | `RespectVanillaAtmLimit` false or week calc off | `ElapsedDays/7` + `GetWeeklyDeposits(week)` `BankService.cs:34` |
| Debt paid N× for one trigger | No re-entry guard | `isAutoPaying` flag around the apply block (§7) |
| `onlineBalance` reads as 0 | IL2CPP auto-property needs sync method | `sync___get_value_onlineBalance()` instead of direct property (§7) |
| NullRef on Supplier.Debt | NPC not fully initialized early in scene | Null-check + retry on next poll tick |

---

## 9. References

* `references/atomic-purchase.md` — PocketShop full purchase flow with `BuyResult` enum
* `references/atm-double-entry.md` — BankApp cash↔bank, fees, limits, history
* `references/passive-revenue.md` — BusinessIncome snapshot + variance + host authority
* External: `Knowledge/Game-Reference/Analysis/Systems/23-Economy-Money.md`, `11-Business-Laundering.md`, `19-Customer-Budget.md`, `45-Multiplayer.md`; `Knowledge/Analysis/APIs/S1API.md` §Money; `S1API.Money` decompile `Knowledge/Frameworks/S1API/Decompiles/3.2.0/S1API.Money/`
* Live code: `Source/Mods/BankApp/src/Services/BankService.cs`, `Source/Mods/PocketShop/src/Services/PurchaseService.cs`, `Source/Mods/BusinessIncome/src/Services/IncomeEngine.cs` + `PayoutStateStore.cs`
* Supplier reference (decompiled): `.scratch/mod-decompile/_decompiled/AutoPaySuppliers/AutoPaySuppliers/AutoPaySuppliersMod.cs` (key APIs: `Supplier.Debt`, `SupplierDebtAccess.ChangeDebt`, `RelationData.ChangeRelationship`)
