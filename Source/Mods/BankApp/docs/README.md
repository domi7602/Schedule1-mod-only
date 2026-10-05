# BankApp — Mobile Banking Smartphone Mod

**BankApp** is a full in-game smartphone banking app for **Schedule I** (v0.4.6f13 / IL2CPP), built on the **S1API PhoneApp** architecture with a modern fintech dark theme.

In-game screenshot (v0.3.0 UI): [`assets/bankapp/dashboard-v0.3.0.png`](https://github.com/domi7602/Schedule1-mod-only/blob/main/assets/bankapp/dashboard-v0.3.0.png) · design mock-up: [`mockup-target-v0.3.0.png`](mockup-target-v0.3.0.png).

---

## Features

- **Digital Accounts Overview**:
  - Large digital checking-account card design with live balance (`$ 12,450.00`).
  - Companion cards for available pocket cash (`💵 Cash on Hand`) and net worth (`🏛 Net Worth`).
- **Mobile Transfers (ATM)**:
  - **Deposits (Cash → Bank)**: Atomically deducts cash and credits the checking account.
  - **Withdrawals (Bank → Cash)**: Limited only by bank balance and the weekly ATM limit — the engine cash path (`MoneyManager.ChangeCashBalance` → balance float + single `CashInstance`) never touches hotbar slots, so a full inventory can neither block nor lose a withdrawal.
  - **Weekly ATM Limits**: By default bound to the vanilla weekly limit of **$10,000 / week** (`RespectVanillaAtmLimit`), so as not to undermine vanilla money-laundering and ATM gameplay (configurable).
  - Quick chips: `[+$100]`, `[+$500]`, `[+$1,000]`, `[MAX]` (computes the absolute maximum from limits and balance).
- **Savegame Slot Isolation & SafeStorage**:
  - Transaction history and weekly limits are stored slot-isolated under `UserData/BankApp/bank_slot_{slotId}.json`.
  - Atomic writes and automatic `.bak` backups protect against file corruption.
- **Audio Feedback & Input Focus Protection**:
  - Real cash register chime on successful transactions (`MoneyManager.Instance.PlayCashSound()`).
  - Procedural click and error-buzzer tones.
  - `BankAppInputFocus` (IL2CPP MonoBehaviour) disables character movement (WASD) while typing in amount fields.
- **Method 3 Responsive UI**:
  - Dynamic UI scaling (`UITheme.Dp` / `UITheme.Sp`) on high-resolution screens.

---

## Configuration (`UserData/BankApp/config.json`)

```toml
[BankApp]
ServiceFeePercent = 0.0
RespectVanillaAtmLimit = true
EnableSoundEffects = true
```
