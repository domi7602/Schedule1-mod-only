# Changelog

## 0.1.0
- Scaffolded standalone PoliceTestMode; default enabled for singleplayer test sessions.
- Added explicit police/curfew gates, bounded registry reapplication, exact boolean restoration,
  status logging and `policetest on|off|status` session commands.
- Thirty-six game-free tests pass; Release no-deploy build has zero warnings/errors.
- **Bounded in-game verification 2026-11-10** (`TownHallTest` / `SaveGame_2`, `Schedule I.exe` PID 10708, installed DLL SHA-256 `033DE3DF31A6C8FA21F917A0E2A04B70980B99DA5A476658096307C5DC05E740`):
  - Registered `43/43` native police/curfew hooks at startup (`hooks ready registered=43/43`).
  - Applied default ON suppression on initial load and after a full `Main -> Menu -> Main` reload (`state='ACTIVE - police suppressed (TEST)' hooks=43 loaded=True singleplayer=True attempts=1/30 leases=17 officers=10 vehicles=0 playerOccupiedSkipped=0 unknownSkipped=0 checkpoints=4`, `activeTargets=0/14 curfewRaw=False/False/False`).
  - Observed live blocked hook invocations across 7 native methods (`PoliceOfficer.OnTick=11355`, `PoliceOfficer.CanInvestigate=5710`, `CurfewManager.get_IsCurrentlyActive=568`, `CurfewManager.OnUncappedMinPass=563`, `LawActivitySettings.Evaluate=563`, `CurfewManager.get_IsEnabled=110`, `CurfewManager.RpcLogic___Enable_328543758=1`).
  - Verified clean lease restoration across `Main -> Menu` (`restore reason=lifecycle leaving pending=0`) and `Menu -> Main` (`restore reason=load boundary pending=0`), with all 39 `SaveGame_2` files byte-identical before and after (`diffCount=0`).
  - Interactive `policetest off|on` console toggling during gameplay and live player-occupied police vehicle exemption (`playerOccupiedSkipped > 0`) remain open. See [evidence.md](evidence.md) and [README.md](README.md) limits.
