# Business/Laundering (Schedule I)

## Business
- **Only class**: `Business` (inherits Property)
- **No subclasses** for different front shop types
- `EmployeeCapacity` (default 10)
- `LaunderCapacity` (default 1000)

## Laundering (Money Laundering)
### Process
1. Place `LaunderingStation` inside a Business (GridItem)
2. Open interface → select amount (min 10, max `min(LaunderCapacity, Cash)`)
3. `ChangeCashBalance(-amount)` → cash is deducted
4. `StartLaunderingOperation(amount)` → 24h timer (1440 minutes)
5. After 24h: `CreateOnlineTransaction("Money laundering", amount)`
6. Money appears as **Online Balance**

### Important Facts
| Aspect | Value |
|--------|-------|
| Duration | 1440 minutes = 1 game day |
| Fee | **None** (100% of amount) |
| Max per Business | 1000 (simultaneously in laundering) |
| Min Amount | 10 |
| Simultaneous Operations | Unlimited (as long as capacity permits) |

### Law/Heat Interaction
- **NO** direct interaction with Law/Heat
- No crime type "MoneyLaundering"
- Only indirectly: drug dealing (which generates the cash) increases risk

### Persistence
- `BusinessManager` saves in `/Businesses/<PropertyName>/`
- `LaunderOperationData`: `Amount` + `MinutesSinceStarted`
- Upon loading, pending operations are restored with their current progress

## Employee Management
- Employees: Botanist, Chemist, Packager, Cleaner
- `SigningFee` (500) + `DailyWage` (100)
- Require a paid locker (`EmployeeHome`)
- Can be transferred (`SendTransfer()`) or fired (`SendFire()`)

## Passive Income
- **NO** direct passive income
- Only: Laundering (one-time payouts after 24h) + Dealer sales
