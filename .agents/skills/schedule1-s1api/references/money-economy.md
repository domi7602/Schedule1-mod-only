# S1API — Money, GameTime, Property, Law

The "core simulation" APIs. These wrap the underlying game tracking for time, money, properties, and the law/wanted system.

---

## 1. Money — `S1API.Money`

```csharp
using S1API.Money;

Money.CashBalance          // float: current cash on hand
Money.OnlineBalance        // float: bank balance
Money.NetWorth             // float: cash + bank + business equity
Money.OnBalanceChanged      // event Action

Money.GetCashBalance()      // same as CashBalance (method form)
Money.GetOnlineBalance()
Money.GetNetWorth()

Money.ChangeCashBalance(float amount, bool visualizeChange = true, bool playCashSound = false)
Money.CreateOnlineTransaction(string name, float unitAmount, float quantity, string note)
```

### Atomic Deposit / Withdraw Pattern

```csharp
// ❌ WRONG — money can be lost if the second call fails
Money.ChangeCashBalance(-amount, true, false);
Money.CreateOnlineTransaction("Deposit", credited, 1f, "Note");   // NPE → cash gone

// ✅ RIGHT — try/catch + refund
try
{
    Money.ChangeCashBalance(-amount, true, false);
    Money.CreateOnlineTransaction("Deposit", credited, 1f, "Note");
}
catch
{
    Money.ChangeCashBalance(+amount, false, false);   // refund
}
```

### Live Balance Updates

```csharp
// Subscribe ONCE in OnCreated (idempotent: -= before +=)
Money.OnBalanceChanged -= OnBalanceChanged;
Money.OnBalanceChanged += OnBalanceChanged;
```

> ⚠ **2026-08-20:** Do NOT unsubscribe `OnBalanceChanged` inside `OnPhoneClosed` — S1API `OnCreated` fires only once per scene; the app would stop updating after the first close. Use `OnDestroyed()` for real teardown (see lifecycle.md §7).

---

## 2. GameTime — `S1API.GameTime`

```csharp
using S1API.GameTime;

TimeManager.CurrentDay       // Day enum (Monday..Sunday)
TimeManager.ElapsedDays      // int total days
TimeManager.CurrentTime       // int HHMM (12h format!)
TimeManager.IsNight           // bool
TimeManager.IsEndOfDay        // bool
TimeManager.SleepInProgress   // bool
TimeManager.NormalizedTime    // float 0..1
TimeManager.Playtime          // float total seconds

TimeManager.OnHourPass        // event
TimeManager.OnDayPass         // event
TimeManager.OnWeekPass        // event
TimeManager.OnSleepStart      // event
TimeManager.OnSleepEnd(int)   // event with remaining minutes
TimeManager.OnTick            // event every in-game minute

TimeManager.SetTime(int time24h)   // e.g. 1200 = noon
TimeManager.GetFormatted12HourTime() // "3:00 PM"
TimeManager.IsCurrentTimeWithinRange(int start24h, int end24h)
```

> **Critical:** `CurrentTime` is a **12-hour HHMM format** (e.g. 900 = 9 AM, 2100 = 9 PM). Never use `CurrentTime / 100` for hour extraction — use `GetFormatted12HourTime()` or `IsCurrentTimeWithinRange(...)`.

---

## 3. Property — `S1API.Property`

```csharp
using S1API.Property;

var all = PropertyManager.GetAllProperties();           // List<PropertyWrapper>
var owned = PropertyManager.GetOwnedProperties();       // List<PropertyWrapper>
var prop = PropertyManager.FindPropertyByName("Motel Room");   // null if not found

prop.PropertyName    // "Motel Room"
prop.PropertyCode    // "motel"
prop.Price           // 5000f
prop.IsOwned         // false
prop.EmployeeCapacity // 3
prop.IsPointInside(Vector3) // bool
prop.SetOwned()
```

### Save-Load Gotcha

```csharp
// ❌ WRONG — Property.OwnedProperties is empty at OnGameplaySceneLoaded
public override void OnSceneWasLoaded(int idx, string name)
{
    var owned = PropertyManager.GetOwnedProperties();   // EMPTY!
}

// ✅ RIGHT — use S1API lifecycle hook
public override void OnInitializeMelon()
{
    GameLifecycle.OnSaveLoaded += () =>
    {
        var owned = PropertyManager.GetOwnedProperties();   // populated now
    };
}
```

---

## 4. Law — `S1API.Law`

The wanted-level system. Useful for:
- Tracking player's currentwanted level
- Triggering events when wanted level changes
- Reading police patrols

```csharp
using S1API.Law;

var wanted = LawManager.GetCurrentWantedLevel();   // 0-5
LawManager.OnWantedLevelChanged += (newLevel) => { /* ... */ };
```

For deeper integration with police behavior, see `S1API.Law` decompile.

---

## 5. Workspace Reference

* `Money` — used by `BankApp`, `BusinessIncome`, `PocketShop`, `CalculatorApp`, `HomelessMod`
* `GameTime` — used by `DayCounter` (in `Minimap`), `BankApp` (history timestamps), `HomelessMod` (`IsNight` check)
* `Property` — used by `HomelessMod`, `BusinessIncome`, `PotScanner`
* `Law` — unused by current workspace mods (potential for future police mods)

For the full API surface, see `Knowledge/Analysis/s1api-reference/S1API.Money.md`, `S1API.GameTime.md`, `S1API.Property.md`.
