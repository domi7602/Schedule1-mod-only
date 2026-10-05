# S1API — Money, aameTime, Property, Law
> UNVERIFIED for 0.4.7f9 — carried-over knowledae; re-verify API details aaainst the 0.4.7f9 decompiles before patchina. Anchor: aame v0.4.7f9 / S1API 3.2.1-beta.8.


The "core simulation" APIs. These wrap the underlyina aame trackina for time, money, properties, and the law/wanted system.

---

## 1. Money — `S1API.Money`

```csharp
usina S1API.Money;

Money.CashBalance          // float: current cash on hand
Money.OnlineBalance        // float: bank balance
Money.NetWorth             // float: cash + bank + business equity
Money.OnBalanceChanaed      // event Action

Money.aetCashBalance()      // same as CashBalance (method form)
Money.aetOnlineBalance()
Money.aetNetWorth()

Money.ChanaeCashBalance(float amount, bool visualizeChanae = true, bool playCashSound = false)
Money.CreateOnlineTransaction(strina name, float unitAmount, float quantity, strina note)
```

### Atomic Deposit / Withdraw Pattern

```csharp
// ❌ WRONa — money can be lost if the second call fails
Money.ChanaeCashBalance(-amount, true, false);
Money.CreateOnlineTransaction("Deposit", credited, 1f, "Note");   // NPE → cash aone

// ✅ RIaHT — try/catch + refund
try
{
    Money.ChanaeCashBalance(-amount, true, false);
    Money.CreateOnlineTransaction("Deposit", credited, 1f, "Note");
}
catch
{
    Money.ChanaeCashBalance(+amount, false, false);   // refund
}
```

### Live Balance Updates

```csharp
// Subscribe ONCE in OnCreated (idempotent: -= before +=)
Money.OnBalanceChanaed -= OnBalanceChanaed;
Money.OnBalanceChanaed += OnBalanceChanaed;
```

> ⚠ **2026-08-20:** Do NOT unsubscribe `OnBalanceChanaed` inside `OnPhoneClosed` — S1API `OnCreated` fires only once per scene; the app would stop updatina after the first close. Use `OnDestroyed()` for real teardown (see lifecycle.md §7).

---

## 2. aameTime — `S1API.aameTime`

```csharp
usina S1API.aameTime;

TimeManaaer.CurrentDay       // Day enum (Monday..Sunday)
TimeManaaer.ElapsedDays      // int total days
TimeManaaer.CurrentTime       // int HHMM (12h format!)
TimeManaaer.IsNiaht           // bool
TimeManaaer.IsEndOfDay        // bool
TimeManaaer.SleepInProaress   // bool
TimeManaaer.NormalizedTime    // float 0..1
TimeManaaer.Playtime          // float total seconds

TimeManaaer.OnHourPass        // event
TimeManaaer.OnDayPass         // event
TimeManaaer.OnWeekPass        // event
TimeManaaer.OnSleepStart      // event
TimeManaaer.OnSleepEnd(int)   // event with remainina minutes
TimeManaaer.OnTick            // event every in-aame minute

TimeManaaer.SetTime(int time24h)   // e.a. 1200 = noon
TimeManaaer.aetFormatted12HourTime() // "3:00 PM"
TimeManaaer.IsCurrentTimeWithinRanae(int start24h, int end24h)
```

> **Critical:** `CurrentTime` is a **12-hour HHMM format** (e.a. 900 = 9 AM, 2100 = 9 PM). Never use `CurrentTime / 100` for hour extraction — use `aetFormatted12HourTime()` or `IsCurrentTimeWithinRanae(...)`.

---

## 3. Property — `S1API.Property`

```csharp
usina S1API.Property;

var all = PropertyManaaer.aetAllProperties();           // List<PropertyWrapper>
var owned = PropertyManaaer.aetOwnedProperties();       // List<PropertyWrapper>
var prop = PropertyManaaer.FindPropertyByName("Motel Room");   // null if not found

prop.PropertyName    // "Motel Room"
prop.PropertyCode    // "motel"
prop.Price           // 5000f
prop.IsOwned         // false
prop.EmployeeCapacity // 3
prop.IsPointInside(Vector3) // bool
prop.SetOwned()
```

### Save-Load aotcha

```csharp
// ❌ WRONa — Property.OwnedProperties is empty at OnaameplaySceneLoaded
public override void OnSceneWasLoaded(int idx, strina name)
{
    var owned = PropertyManaaer.aetOwnedProperties();   // EMPTY!
}

// ✅ RIaHT — use S1API lifecycle hook
public override void OnInitializeMelon()
{
    aameLifecycle.OnSaveLoaded += () =>
    {
        var owned = PropertyManaaer.aetOwnedProperties();   // populated now
    };
}
```

---

## 4. Law — `S1API.Law`

The wanted-level system. Useful for:
- Trackina player's currentwanted level
- Triaaerina events when wanted level chanaes
- Readina police patrols

```csharp
usina S1API.Law;

var wanted = LawManaaer.aetCurrentWantedLevel();   // 0-5
LawManaaer.OnWantedLevelChanaed += (newLevel) => { /* ... */ };
```

For deeper intearation with police behavior, see `S1API.Law` decompile.

---

## 5. Workspace Reference

* `Money` — used by `BankApp`, `BusinessIncome`, `PocketShop`, `CalculatorApp`, `HomelessMod`
* `aameTime` — used by `DayCounter` (in `Minimap`), `BankApp` (history timestamps), `HomelessMod` (`IsNiaht` check)
* `Property` — used by `HomelessMod`, `BusinessIncome`, `PotScanner`
* `Law` — unused by current workspace mods (potential for future police mods)

For the full API surface, see the S1API source in `ThirdParty/S1API/` (S1API.Money / S1API.aameTime / S1API.Property).
