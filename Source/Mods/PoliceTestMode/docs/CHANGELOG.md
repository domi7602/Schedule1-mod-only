# Changelog

## 0.1.0
- **Bounded in-game verification 2026-11-11** (`TownHallTest` / `SaveGame_2`, PID 5540, same installed DLL as below):
  - Entered `policetest off` and `policetest on` in the in-game console. At `05:59:46.622` CET restoration completed with `pending=0`, followed by OFF with `leases=0`. At `05:59:58.329` ON was effective again with `leases=17` and `activeTargets=0/14`.
  - With `Shitbox_Police (2)` occupied by the player, console ON reported `PARTIAL` with `playerOccupiedSkipped=1`, `unknownSkipped=0` and `leases=19` at `06:00:29.288`. S1MCP confirmed the player was still seated and the vehicle active. After exit, at `06:00:36.341` the skip count returned to zero and suppression covered the car. S1MCP confirmed it was inactive. [Readbacks and test setup](evidence.md#bounded-live-in-game-verification-2026-11-11).
- Scaffolded standalone PoliceTestMode; default enabled for singleplayer test sessions.
- Added explicit police/curfew gates, bounded registry reapplication, exact boolean restoration,
  status logging and `policetest on|off|status` session commands.
- Thirty-six game-free tests pass; Release no-deploy build has zero warnings/errors.
- **Bounded in-game verification 2026-11-10** (`TownHallTest` / `SaveGame_2`, `Schedule I.exe` PID 10708, installed DLL SHA-256 `033DE3DF31A6C8FA21F917A0E2A04B70980B99DA5A476658096307C5DC05E740`):
  - Registered `43/43` native police/curfew hooks at startup (`hooks ready registered=43/43`).
  - Applied default ON suppression on initial load and after a full `Main -> Menu -> Main` reload (`state='ACTIVE - police suppressed (TEST)' hooks=43 loaded=True singleplayer=True attempts=1/30 leases=17 officers=10 vehicles=0 playerOccupiedSkipped=0 unknownSkipped=0 checkpoints=4`, `activeTargets=0/14 curfewRaw=False/False/False`).
  - Observed live blocked hook invocations across 7 native methods (`PoliceOfficer.OnTick=11355`, `PoliceOfficer.CanInvestigate=5710`, `CurfewManager.get_IsCurrentlyActive=568`, `CurfewManager.OnUncappedMinPass=563`, `LawActivitySettings.Evaluate=563`, `CurfewManager.get_IsEnabled=110`, `CurfewManager.RpcLogic___Enable_328543758=1`).
  - Verified clean lease restoration across `Main -> Menu` (`restore reason=lifecycle leaving pending=0`) and `Menu -> Main` (`restore reason=load boundary pending=0`), with all 39 `SaveGame_2` files byte-identical before and after (`diffCount=0`).
  - This session covered default suppression and reload. The interactive console toggle and occupied-vehicle exemption were exercised in the 2026-11-11 follow-up above.
