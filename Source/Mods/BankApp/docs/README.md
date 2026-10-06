# BankApp — Mobile Banking Smartphone Mod

**BankApp** is a full in-game smartphone banking app for **Schedule I** (v0.4.6f13 / IL2CPP), built on the **S1API PhoneApp** architecture with a modern fintech dark theme.

In-game screenshot (v0.3.0 UI): [`assets/bankapp/dashboard-v0.3.0.png`](https://github.com/domi7602/Schedule1-mod-only/blob/main/assets/bankapp/dashboard-v0.3.0.png) · design mock-up: [`mockup-target-v0.3.0.png`](mockup-target-v0.3.0.png).

---

## Features

- **Overview / Transaction shell**: *Overview* holds balances, the weekly limit and two quick actions (**Deposit** / **Withdraw**); each opens a dedicated **Transaction** screen (back header, guarded amount field with presets, live preview, action button and a short-lived confirmation line). There is no Activity page or top-level tab strip. Back or Escape returns from Transaction to Overview before the phone itself closes (via S1API's native exit chain, `PhoneApp.Exit(ExitAction)`).
- **Digital Accounts Overview**:
  - Large digital checking-account card design with live balance (`$ 12,450.00`).
  - Companion cash-on-hand row and an online-balance headline, plus a weekly-limit card with usage bar, remaining amount and a live reset countdown (`7 - day % 7` days).
- **Mobile Transfers (ATM)**:
  - **Deposits (Cash → Bank)**: Atomically deducts cash and credits the checking account.
  - **Withdrawals (Bank → Cash)**: Limited only by bank balance and the weekly ATM limit — the engine cash path (`MoneyManager.ChangeCashBalance` → balance float + single `CashInstance`) never touches hotbar slots, so a full inventory can neither block nor lose a withdrawal. The MAX amount is fee-aware (gross + fee ≤ balance).
  - **Mobile weekly deposit cap**: BankApp enforces its **own** per-save-slot weekly deposit counter against a **$10,000 / week** ceiling (`RespectVanillaAtmLimit`, configurable). The counter lives in BankApp's persisted history (`TransactionHistoryService.GetWeeklyDeposits`); it does **not** read or mutate the vanilla ATM's own weekly-sum bookkeeping, so toggling it never alters base-game money-laundering limits.
  - **Amount entry**: fixed set-amount presets `$100`, `$500`, `$1,000`, `$2,500`, `$5,000`, `$10,000` (they *set* the amount), relative `25%` / `50%` / `MAX` of the allowed maximum, a Clear button, and a guarded direct-input field (decimal-only; non-finite/non-positive input is rejected).
  - **Live preview**: fee, net (`You receive`), source debit (`Cash out` / `Bank debit`) and balance-after update as the amount changes. If the transfer is invalid the action button is disabled and an inline reason is shown; success gives a short confirmation line — no extra confirm dialog.
  - All figures come from one Unity-free seam (`TransferMath` in `src/TransferMath.cs`), reused by the preview, the MAX buttons and `BankService` execution revalidation so the UI cannot accept a quote the service would recompute differently. Fee math preserves the existing float formula (`amount * (pct/100)`, pct clamped to 0–10), no invented rounding.
- **Persisted transaction records**: deposits and withdrawals remain stored for save-slot isolation and weekly deposit bookkeeping. Removing the Activity page does not delete existing records or change transfer execution.
- **Savegame Slot Isolation & SafeStorage**:
  - Transaction history and weekly limits are stored slot-isolated under `UserData/BankApp/bank_slot_{slotId}.json`.
  - Atomic writes and automatic `.bak` backups protect against file corruption.
- **Audio Feedback & Input Focus Protection**:
  - Real cash register chime on successful transactions (`MoneyManager.Instance.PlayCashSound()`).
  - Procedural click and error-buzzer tones.
  - `BankAppInputFocus` (IL2CPP MonoBehaviour) disables character movement (WASD) while typing in amount fields.
- **Method 3 Responsive UI**:
  - Dynamic UI scaling (`UITheme.Dp` / `UITheme.Sp`) on high-resolution screens.
- **Layout invariant (fixed-height chrome, single flexible viewport)**:
  - Every fixed band — the header, the transaction drill-in head and every row — is sized through `SetHeight`, which pins `LayoutElement.flexibleHeight = 0` at `layoutPriority = 1`. Only the content viewport (`ContentRoot`, and the scroll body inside the transaction pane) reports positive height flexibility.
  - Why: a `LayoutElement` that leaves `flexibleHeight` unset (-1) stops reporting it, and the layout system falls through to a sibling `HorizontalLayoutGroup` with `childForceExpandHeight = true`, which promotes each child to flexible ≥ 1 and reports a positive flexible height — so the parent vertical group hands that band the spare space and it balloons (header/tabs grew to ~140px each on a 560px phone). Pinning `flexibleHeight = 0` with priority 1 wins against the sibling group.
  - Overview order matches the mock-up: balance hero (~150dp) → two large equal-width Deposit/Withdraw tiles → weekly card. Transaction pane keeps a compact back/title head, a dominant amount field, a 3×2 preset grid plus 25% / 50% / MAX, one unified preview, and a primary action pinned in a fixed footer beneath the scroll body.

---

## Configuration (`UserData/BankApp/config.json`)

```toml
[BankApp]
ServiceFeePercent = 0.0
RespectVanillaAtmLimit = true
EnableSoundEffects = true
```
