# ATM Double-Entry (BankApp Reference)

Source: `BankApp/src/Services/BankService.cs` (v0.1.0 verified) + `TransactionHistoryService.cs`.

### Read Balances
```csharp
float cash = MoneyManager.Instance.cashBalance;   // GetCashBalance():19
float bank = MoneyManager.Instance.onlineBalance; // GetOnlineBalance():18
```

### Inventory Capacity (Slot-Aware)
```csharp
int free = EconomyHelper.GetFreeInventorySlotsCount(); // :51
float capacity = EconomyHelper.GetMaxHoldableCashCapacity(maxPerSlot); // 1000 default:57
float maxWithdrawable = Mathf.Min(onlineBalance, capacity); // :60
float maxDepositable = Mathf.Min(cash, RemainingWeeklyLimit); // :80
```

### Weekly Limit
```csharp
bool respect = ModConfig<BankAppConfig>.Instance.RespectVanillaAtmLimit; // default true
int week = ElapsedDays / 7; // GetCurrentInGameWeek:34
float deposited = TransactionHistoryService.GetWeeklyDeposits(week); // slot_{n}.json
float remaining = Mathf.Max(0, 10000 - deposited); // VanillaWeeklyAtmLimit:16
```

### Deposit (Cash → Bank)
```csharp
ChangeCashBalance(-amount, true, false); //:119
try { CreateOnlineTransaction("ATM Deposit", amount-fee, 1f, "Mobile ATM Cash Deposit"); } //:120
catch { ChangeCashBalance(+amount, false,false); return false; } // refund:124
RecordWeeklyDeposit(amount, week); AddTransaction(Deposit, netCredited); PlayCashSuccess();
```

### Withdraw (Bank → Cash)
```csharp
CreateOnlineTransaction("ATM Withdrawal", -(amount+fee), 1f, "Mobile ATM Cash Withdrawal"); //:188
try { ChangeCashBalance(+amount, true,false); } //:189
catch { CreateOnlineTransaction("ATM Withdrawal Refund", +(amount+fee), 1f, "Rollback"); return false; } //:193
AddTransaction(Withdrawal, -amount); PlayCashSuccess();
```

Persistence: `bank_slot_{n}.json` via `BankState` (SafeStorage SaveAtomic) — slot-isolated.
