# Changelog — BankApp

All notable changes to the BankApp mod will be documented in this file.

## [0.4.1] — 2026-09-10

### Fixed
- Statischer Event-Dispatcher (OnUpdate/Balance/History) gegen Subscriber-Leaks.

## [0.4.0] — 2026-09-09

### UI Fixes (user screenshot feedback)
- **Transform & Anchor Fix:** Removed flawed `PinParentPanelToStretch` and `EnsureConstraintLayout`. S1API vertically rotates the panel by 90° on the phone's canvas quad using center-anchored `sizeDelta`; stretch anchors broke this transform and warped the UI into a massive landscape overlay outside the phone. Layout now respects S1API canvas metrics.
- **MAX chip:** Mode-aware accent color (blue in Deposit, orange in Withdraw) while maintaining the clean, symmetric 2×5 chip grid (`✕ CLEAR` + `MAX`).
- **Uniform rows/columns:** Every interactive row (mode tabs + all chip rows) now has an identical fixed height of 38dp and a uniform 6dp grid gap — no more `flexibleHeight` stretching, no more uneven row heights or ragged column edges. Main layout spacing unified to 6dp (was 12dp).
- **Unchanged backend:** Double-entry transactions with rollback, slot capacity checks, weekly limit enforcement, slot-isolated persistence, audio feedback.

## [0.3.0] — 2026-09-09

### Mockup-Based UI Redesign (docs/mockup-target-v0.3.0.png)
- **Weekly Progress Card:** Deposit usage against $10k vanilla ATM limit with live progress bar (`$ deposited / $ limit`); shows `NO LIMIT` when `RespectVanillaAtmLimit` is disabled.
- **Two-Column Balances:** CASH BALANCE and ONLINE BALANCE side by side in teal (mockup style).
- **Chip Grid (2 columns × 5 rows):** $1, $5, $10, $25, $50, $100, $500, $1000 + ✕ CLEAR + MAX (accent blue). Chips ADD to the amount — replaces the squeezed single-row chips of v0.2.0.
- **Mode Tabs:** ⬇ DEPOSIT / ⬆ WITHDRAW toggle (selected = blue); switching resets the amount.
- **Single Confirm Button:** Full-width, green for deposit / orange for withdraw, label follows mode.
- **Removed:** Free-text amount input field + `BankAppInputFocus` component (no typing → no WASD protection needed; IL2Cpp registration removed from `Mod.cs`).
- **Unchanged Backend:** Double-entry transactions with rollback, slot capacity checks, weekly limit enforcement, slot-isolated persistence, audio feedback.

## [0.2.0] — 2026-09-09

### UI Simplification — Single-Screen Redesign
- **Removed**: Tab navigation (Accounts / Transfer / Statement), `DashboardPane`, `TransferPane`, `HistoryPane`, fake credit card, net worth card, recent activity feed, transfer mode switcher, limit info card.
- **New single-screen layout**: Header → Balance card (Bank + Cash) → Amount input → Quick chips (+$100 / +$500 / +$1,000 / MAX / CLR) → Deposit & Withdraw buttons → Feedback line.
- **MAX chip**: context-aware — fills max depositable cash if cash on hand, otherwise max withdrawable amount.
- **Unchanged backend**: `BankService` double-entry transactions, weekly ATM limit ($10k, config), slot capacity checks, `TransactionHistoryService` persistence, slot isolation, input focus protection, audio feedback — all still active, just no longer surfaced as UI clutter.

## [0.1.0] — 2026-08-17

### Initial Release
- **S1API PhoneApp Integration**: Full smartphone banking app registration under `BankApp`.
- **Digital Account Dashboard**: Large checking card, cash on hand and net worth overview.
- **Slot-Aware ATM Operations**: Precise double-entry cash deposit & withdrawal with slot capacity calculation.
- **Weekly ATM Limits**: Configurable $10,000 weekly deposit limit respecting vanilla game economy.
- **Save-Slot Isolation**: Independent JSON persistence per save slot with `.bak` safety backup.
- **Audio Feedback**: Native cash sounds + procedural UI clicks and deny buzzers.
- **Input Focus Protection**: `BankAppInputFocus` component prevents player movement while editing amount fields.
- **Responsive UI Theme**: Methode 3 dynamic scaling with modern Fintech dark palette.
