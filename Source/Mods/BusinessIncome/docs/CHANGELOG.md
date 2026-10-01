# Changelog

## 0.1.6 (2026-09-17)
- **Fix: 0-business backlog bug**: When the player owns no businesses yet (`lines.Count == 0`), the day is now properly marked as paid/settled in the `PayoutStateStore` on `commit && !isDryRun` (`LastPaidElapsedDay = elapsedDays`). Previously `TryExecuteDailyPayout` aborted without a state commit, so `LastPaidElapsedDay` got stuck on the seed day (e.g. day 3) and produced a growing backlog warning (> 7 days) plus endless catch-up loops in the log with every day change.

## 0.1.5 (2026-09-13) — Bug-report round 6
Bug-report round 6: BIZ-01 catch-up loop after cap re-read + hard-capped at MaxCatchupDays + LastPaid validation on load (freeze on corrupt state fixed); BIZ-02 `biz trigger --commit` requires `--force` when the day is already paid (money printer closed); BIZ-03 `float.IsFinite`/upper-bound guards in Sanitize including MaxCatchupDays clamp (prevents NaN-economy brick); BIZ-04 `biz pending confirm` only commits forward (stale marker no longer causes state regression); BIZ-05 OnPreLoad keeps the slot (`keepSlot:true`, no more `*_default.json` during the load window).

## 0.1.4 (2026-09-12) — Bug-audit fixes (audit 2026-09-12)
- `IsHostOrSingleplayer`: fail-**closed** on exceptions (previously `return true` allowed double-bookings when the IsServer marshalling call failed on an MP client).
- On `commit && !committed` (money booked but save failed): dedicated notification "booked — save FAILED, run `biz pending confirm|resolve`" instead of the misleading success message. Pending marker stays on disk; manual `biz pending` console resolve path already existed.

## 0.1.2 (2026-09-11)
- Windfall seed persisted (no re-seed + no biz-stats flip after restart).

## 0.1.1 (2026-09-10)
- First-install windfall seeded (no history since day 0).
- Catch-up also runs via OnDayPass (idempotent); display strings invariant.
- Lead 2026-09-01: Version bump.

## 0.1.0 (2026-08-17)

- **Initial version**:
  - Daily passive income for all owned `Business.OwnedBusinesses`.
  - Multiplayer host-authority protection via `NetworkGuard.IsHostOrSingleplayer()`.
  - Save-slot-isolated `PayoutStateStore` with atomic writes (`SafeStorage`) and crash protection.
  - Deterministic revenue model with hash variance (±15%), employee bonus and weekend factors.
  - Automatic online transactions via S1API `Money.CreateOnlineTransaction`.
  - In-game HUD notifications via `NotificationsManager` with sound effect.
  - Extensive dev console and DooDesch `hash` terminal integration (`biz`).