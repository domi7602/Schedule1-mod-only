# Changelog - AutoPackagingStation

## 0.2.7 (2026-09-13) — Bug-Report-Runde 6
Bug-Report-Runde 6 (Audit 2026-09-13): Host-Guards fuer PackUp, OnDestroy-Refund, alle TryExtract*/TryDeposit*-Pfade und OnSaveComplete (9 neue IsHostOrSingleplayer-Guards). MP-Clients koennen Stationen nicht mehr lokal auszahlen/zerstoeren (Dupe/Desync, kritisch) und ueberschreiben die Slot-Datei des Hosts nicht mehr. Supersedes 0.2.4 (Client lief dort bewusst in den Fallback-Destroy).

## 0.2.6 (2026-09-12) — Bug-Audit-Fixes Runde 4 (Audit 2026-09-12)
- **F-Taste-PackUp Reentrancy-Schutz:** Neues `_packingUp`-Flag plus `try/finally`-Reset in `PackUpStation` verhindert Doppel-Auszahlung, falls der FishNet-Layer `Destroy_Server` ein zweites Dispatch im selben Frame anstößt.

## 0.2.5 (2026-09-12) — Bug-Audit-Fixes Runde 3 (Audit 2026-09-12)
- **Quality-Mixing nativ (HIGH):** Wenn der Output-Slot bereits einen Stack derselben Definition hat und ein neuer Batch dazukommt, mischt der Mod jetzt gewichtet (Tier-Via-Wert via neuer `PackagingMath.TierToQualityValue`-Helper) und mappt zurück auf den nächsten Tier. Vorher erbte der Stack die Qualität des ersten Batchs — Standard-Buds auf Premium-Stack wurden zu Premium verkauft und umgekehrt.
- **ObjLoader-Datei-Limit (LOW):** `LoadMeshFromObj` prueft jetzt die Dateigroesse vor `File.ReadAllLines` (50 MB Cap) und bricht mit Warnung ab, wenn die Vertex-Anzahl 250 000 ueberschreitet. Verhindert Frame-Spike bei versehentlich riesigen OBJ-Files.

## 0.2.4 (2026-09-12) — Bug-Audit-Fixes Runde 2 (Audit 2026-09-12)
- **PackUpStation CRITICAL-Fix:** Statt rohem `GameObject.Destroy(gameObject)` ruft der Mod jetzt `BuildableItem.Destroy_Server()` (FishNet ServerRpc, der Vanilla-Dismantle-Flow). Damit wird die Buildable-Registry, Grid-Belegung und der Netzwerk-State sauber abgebaut. Verhindert Ghost-Platzierungen und Item-Duplikation nach Save/Load. Fallback auf `Destroy(gameObject)` bleibt, falls kein `BuildableItem` an dem GameObject haengt (z. B. Editor-Spawn).
- **Output-Pre-Flight (HIGH):** Vor dem Phase-2-Deduct wird geprueft, ob die Output-Definition + `GetDefaultInstance(1)` tatsaechlich aufloesbar sind. Wenn nein, wird der Cycle vor dem Input-Deduct sauber abgebrochen (kein stiller Item-Verlust mehr).
- **Host-Guard:** `Destroy_Server()`-Aufruf ist hinter `IsHostOrSingleplayer()` gehaengt — auf einem MP-Client ruft der Mod weiterhin den Vanilla-Fallback auf, weil der Server-RPC sonst stumm no-op't.

## 0.2.3 (2026-09-11)
- TryDepositProduct/Packaging: Remove-before-Credit (kein Gratis-Item bei Remove-Fehlschlag mehr).
- Engine: feasible<=0 bricht Batch-Rechnung ab (statt batchSize=1-Coerce).
- Store: Warnung bei unaufgeloestem Slot-Fallback '0' (statt stiller Fehlleitung).

## [0.2.2] (2026-09-10)
- RestoreNativeSlots raeumt rData-Buffer auf plus Live-Native-Gate in Refund-Pfaden (Fix Item-Dupe nach Save-Load).
- Snapshot/Revert in ExecutePackagingTransaction (beide Overloads).
- PackUpStation: Fit-Check und Add interleaved; Teilerfolg behaelt Restbestand.
- Slot--1-Guard (kein autopack_slot_-1.json); TryExtractOutputProduct zahlt exakt N.

## [0.2.0] (2026-08-24)
- Version sync: bring `MelonInfo`, `mod.json`, AGENTS.md, and CHANGELOG into agreement at 0.2.0.
- No code changes since 0.1.0; the "0.2.1 verified 2026-08-23" claim in AGENTS.md was a documentation drift, not a release.
- Hardware store listing integration, 2-second packaging cycle, and atomic 2-phase engine remain as documented in 0.1.0.

## [0.1.0] - 2026-08-22
### Initial Release
- Implemented 4x4 industrial Auto-Packaging Station with procedural 3D chassis, overhead arch, and dual pneumatic pistons.
- Added animated UV-scrolling conveyor belt with configurable speed.
- Integrated multi-state status LEDs (Green/Orange/Blue/Red) with dynamic pulsing.
- Built atomic 2-phase packaging engine with +5% quality freshness bonus and 1:1 mix-effect replication.
- Added procedural sound synthesis for pneumatic hiss, compressor pump, and mechanical stamp impacts.
- Implemented slot-isolated atomic persistence (`autopack_slot_{slotId}.json`) via `SafeStorage.SaveAtomic` synchronized exclusively with `GameLifecycle.OnSaveComplete`.
- Integrated with S1API `BuildableItemCreator` and Handy Hank's hardware store listing injection.
