# Changelog

## 0.1.5 (2026-09-13) — Bug-Report-Runde 6
Bug-Report-Runde 6: BIZ-01 Catch-up-Loop nach Cap neu eingelesen + hart auf MaxCatchupDays gedeckelt + LastPaid-Validierung beim Laden (Freeze bei korruptem State behoben); BIZ-02 'biz trigger --commit' verlangt --force, wenn der Tag bereits bezahlt ist (Money-Printer geschlossen); BIZ-03 float.IsFinite-/Upper-Bound-Guards in Sanitize inkl. MaxCatchupDays-Clamp (NaN-Economy-Brick verhindert); BIZ-04 'biz pending confirm' committet nur vorwaerts (stale Marker keine State-Regression mehr); BIZ-05 OnPreLoad behaelt den Slot (keepSlot:true, kein *_default.json mehr im Lade-Fenster).



## 0.1.4 (2026-09-12) — Bug-Audit-Fixes (Audit 2026-09-12)
- `IsHostOrSingleplayer`: fail-**closed** bei Exceptions (vorher `return true` erlaubte Doppelbuchungen, wenn der IsServer-Marshalling-Aufruf auf einem MP-Client fehlschlug).
- Bei `commit && !committed` (Geld gebucht aber Save fehlgeschlagen): eigene Notification „booked — save FAILED, run `biz pending confirm|resolve`“ statt der irrefuehrenden Erfolgsmeldung. Pending-Marker bleibt auf Disk; manueller `biz pending` Konsolen-Resolve-Pfad war bereits vorhanden.

## 0.1.2 (2026-09-11)
- Windfall-Seed wird persistiert (kein Re-Seed + kein biz-stats-Flip nach Restart).

## 0.1.1 (2026-09-10)
- Erstinstallations-Windfall geseedet (keine Historie seit Tag 0).
- Catch-up laeuft auch via OnDayPass (idempotent); Display-Strings invariant.
- Vorlauf 2026-09-01: Version bump.

## 0.1.0 (2026-08-17)

- **Initial version**:
  - Daily passive income for all owned `Business.OwnedBusinesses`.
  - Multiplayer host-authority protection via `NetworkGuard.IsHostOrSingleplayer()`.
  - Save-slot-isolated `PayoutStateStore` with atomic writes (`SafeStorage`) and crash protection.
  - Deterministic revenue model with hash variance (±15%), employee bonus and weekend factors.
  - Automatic online transactions via S1API `Money.CreateOnlineTransaction`.
  - In-game HUD notifications via `NotificationsManager` with sound effect.
  - Extensive dev console and DooDesch `hash` terminal integration (`biz`).
