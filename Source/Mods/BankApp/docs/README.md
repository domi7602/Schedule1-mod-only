# BankApp — Mobile Banking Smartphone Mod

**BankApp** is a full in-game smartphone banking app for **Schedule I** (v0.4.7 / IL2CPP), built on the **S1API PhoneApp** architecture with a modern fintech dark theme.

Since **v0.5.0** the app is **one single screen** — no tabs, no back button, no second page. Everything the player needs is on one scrollable phone page, top to bottom.

---

## The single screen (v0.5.0)

| Order | Section | Content |
|---|---|---|
| 1 | **Header** | "BankApp" plus the current in-game day and clock (`BankService.GetCurrentInGameDay` / `GetCurrentInGameTimeString`) |
| 2 | **Balance hero** | "Online balance" (large) and "Cash on hand" (smaller) — same data sources as before |
| 3 | **Mode switcher** | **Deposit** / **Withdraw** buttons; one is active. Switching only changes the mode and the action label — it never navigates |
| 4 | **Amount** | The guarded input field (decimal-only, validated by `TransferMath.TryParseAmount`), with chips for `$100`, `$500`, `$1,000` and `MAX`. The chips *set* the field value; `MAX` uses the existing `GetMaxDepositableCash` / `GetMaxWithdrawableCash` helper |
| 5 | **Preview** | Fee and resulting balance, computed by `TransferMath.ComputeQuote` exactly like before. While depositing with the vanilla limit enabled it also shows "Weekly limit left" and "resets in N days" (`TransferMath.DaysUntilWeeklyReset`) |
| 6 | **Primary action** | "Deposit $X" / "Withdraw $X" — same disabled state and same error messages as before |
| 7 | **Recent activity** | The newest five transactions, newest first, grouped by day with `HistoryGrouping` (`TODAY` / `YESTERDAY` / `DAY N`) |

Everything below the header lives in the existing scroll helper, so the page scrolls as a whole. **No code path opens a second screen.** Close and Escape behave exactly as before — they are driven by the phone itself.

---

## Configuration (`UserData/BankApp/config.json`)

```toml
[BankApp]
ServiceFeePercent = 0.0
RespectVanillaAtmLimit = true
EnableSoundEffects = true
```
