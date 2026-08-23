# Passive Revenue (BusinessIncome Reference)

Source: `BusinessIncome/src/Services/IncomeEngine.cs` + `PayoutStateStore.cs` + `RevenueCalculator.cs` (v0.1.0, host-authoritative, slot-idempotent).

### Revenue Formula (per business)
```
gross = BaseIncome * PropertyMultiplier[BusinessId] * variance±15% hash(ElapsedDays, BusinessId)
        * (1 + 0.05*employees capped 0.25)
        * (isWeekend && WeekendBonusCategories.Contains(type) ? 1.25 : 1.0)
costs = gross * 0.10
net   = gross - costs
```
TotalNet = sum(net). If totalNet<=0 → CommitPayout(empty) + no transaction (IncomeEngine.cs:107).

### Execution (IncomeEngine.cs:82 TryExecuteDailyPayout)
```csharp
if (!IsHostOrSingleplayer()) return false; //:84
if (!force && IsDayPaid(day)) return false; //:90
var (lines, totalNet) = GetDailyRevenuePreview(day, config);
if (!lines.Any()) return false;
if (isDryRun) return true;

var ids = lines.Select(l=>l.BusinessId).ToList();
if (commit) PayoutStateStore.MarkInMemoryPaid(day, ids); // snapshots prev:20
try {
  Money.CreateOnlineTransaction("Business Revenue", +totalNet, 1f, $"Daily Revenue ({lines.Count} businesses): +${totalNet:N0}");
} catch {
  if (commit) PayoutStateStore.RevertInMemoryPaid(day, ids); // restores snapshot:149
  return false;
}
if (commit) PayoutStateStore.CommitPayout(day, ids); // SaveAtomic payout_state_slot_{n}.json:60 + clears snapshot
if (config.EnableNotifications) SendNotification(totalNet, lines.Count);
```

### State Store (PayoutStateStore.cs)
* `GetActiveSlotSuffix():23` → LoadManager.ActiveSaveInfo.SaveSlotNumber → `payout_state_slot_{n}.json` (slot-isolated)
* `MarkInMemoryPaid`: snapshots `_pendingPrevLastPaid` + `_pendingPrevPerBusiness` Dict:20 before overwrite
* `RevertInMemoryPaid`: restores snapshot (NOT -1) :149 — fixes 2026-08-21 double-payout
* `CommitPayout`: SaveAtomic + clears snapshot :183
* `IsDayPaid:93` + `IsBusinessPaid:100` per-business guard

Notification: `NotificationsManager.Instance.SendNotification("Business Revenue", "+$X from N businesses", null, 5f, playSound)` + `PlayCashSound` null-checked.
