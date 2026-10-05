# BusinessIncome

Automatic, daily passive income for all owned businesses (`Business.OwnedBusinesses`) in *Schedule I*.

---

## 🌟 Features

- **Daily Bank Transfer**: Credits revenue directly and multiplayer-safely to the in-game online account via S1API `MoneyManager`.
- **Multiplayer Host Authority**: Payouts are strictly calculated and posted on the host/server to prevent money duplication on clients.
- **Savegame-Safe Idempotency**: Slot-isolated `PayoutStateStore` (`payout_state_slot_X.json`) saves the day-state atomically with `.bak` protection against file corruption.
- **Deterministic Revenue Model**:
  - Base income (default: $500/day).
  - Custom multipliers per business (Laundromat, Car Wash, Nightclub, etc.).
  - Deterministic variance (±15% via in-game day hash).
  - Employee synergy (+5% per assigned employee, capped at 25%).
  - Weekend bonuses for bars, nightclubs and food venues (+25%).
  - Operating cost deduction (default: 10% of gross revenue).
- **In-Game HUD Notification**: Sleek notification on every payout via the `NotificationsManager` with cash register chime.
- **Full Console Dashboard (`biz`)**:
  - `biz stats`: Tabular daily overview of all branches.
  - `biz trigger`: Simulation / test payout (with `--commit` for an actual posting).
  - `biz config` / `biz set`: Dynamic runtime configuration.

---

## 💬 Console Commands

Commands work in the vanilla dev console (`~` or `F1`) as well as in the **hash** terminal (`#`):

| Command | Description |
|---|---|
| `biz` / `biz stats` | Shows the dashboard with all branches, revenues and payout status. |
| `biz trigger` | Runs a simulation (dry-run) without posting to the bank. |
| `biz trigger --commit` | Requests an authoritative payout if the day is eligible and no safety lock exists. |
| `biz trigger --commit --force` | Explicitly bypasses the paid-day check; never bypasses a pending/state safety lock. Can pay the same day again. |
| `biz pending` | Inspects the unresolved transfer and recovery status. |
| `biz pending confirm` | After checking bank history: confirms receipt and durably advances the ledger before clearing the marker. |
| `biz pending resolve` | Explicitly declares money missing and clears a readable marker; only an exact undurable session snapshot can be rolled back. |
| `biz config` | Lists all configured multipliers and settings. |
| `biz set base <val>` | Sets the base income (e.g. `biz set base 600`). |
| `biz set hour <0-23>` | Changes the payout hour (e.g. `biz set hour 0` for midnight). |
| `biz set costs <0.0-1.0>` | Adjusts the operating-cost rate (e.g. `biz set costs 0.15` for 15%). |
| `biz set notif <true/false>` | Toggles HUD notifications on/off. |
| `biz help` | Shows the command overview. |

---

## ⚙️ Configuration (`UserData/MelonPreferences.cfg`)

Scalar values live in `UserData/MelonPreferences.cfg` under `[BusinessIncome]`. Collection settings (business multipliers, display-name overrides and weekend categories) live in `UserData/BusinessIncome/business_config.json`. An explicitly empty collection stays empty; missing collection properties retain their defaults. Unsupported schemas are preserved rather than overwritten.

## Safety behavior and verification

A fresh ledger records the installation day as initialization, not as a payout. Exceptions after invoking the bank leave the outcome unknown and block retries with a pending marker. Corrupt state, backup-only state and unresolved marker artifacts fail closed. Never delete a ledger or marker as a generic repair: that can enable duplicate payouts.

The float-based revenue formula and deterministic variance remain unchanged. Feature additions such as payout history and a phone app are outside this safety/refactor batch.

See [safety verification and consolidated gameplay checklist](SAFETY-TESTS.md). The candidate remains compile-only until an explicit deployment; native lifecycle and actual settlement are not proven by unit tests.
