# Changelog - AutoPackagingStation

## 0.3.3 (2026-09-20) — Fix: Baggie-Klone über der Maschine beim Unpacken
- **Root Cause (Live-Befund, User-Report):** Beim Auto-Unpack rendert Vanilla die zurückgegebenen leeren Verpackungen an den nativen `PackagingAlignments`/`ActivePackagingAlignent`-Punkten. Diese Transforms gehören NICHT zu den in `HideBaseRenderers` versteckten SlotPositions — auf unserer Custom-Station (Vanilla-Chassis ausgeblendet) schwebten so bis zu 8 „Baggie-Klone" frei über dem Gehäuse und glitchten beim UI-Öffnen in die Maschine. Nur beim Unpacken sichtbar, da nur dieser Flow Verpackungen zurückgibt.
- **Fix:** `HideBaseRenderers` parkt jetzt zusätzlich `ActivePackagingAlignent`, alle `PackagingAlignments[]` und `ActiveProductAlignments[]` unsichtbar (Scale 0, unter die Basis verlegt) — gleiche Technik wie bei den SlotPositions. Greift ausschließlich auf AutoPackStationen; Vanilla-Maschinen sind nicht betroffen (deren Anzeige bleibt intakt).
- Lager-/Inventar-Daten waren zu jeder Zeit korrekt (rein visuelles Problem).

## 0.3.2 (2026-09-20) — Unpack-Batching (Symmetrie zum Pack-Batch)
- **Unpack im Batch:** v0.3.1 entpackte 1 Einheit pro Zyklus (ein nativer `Unpack()`-Call), während Auto-Pack im 10er-Batch lief. `ExecuteNativeUnpack` führt jetzt bis zu `MaxBatchSize` (Default 10) native Unpack-Calls pro Zyklus aus — mit Vanilla-Readiness-Check (`GetState(EMode.Unpackage) == CanBegin`) vor **jedem** Call, sodass der Loop natürlich stoppt, wenn der Stapel leer oder ein Rückgabe-Slot voll ist.
- **Baggie-HUD-Popups eingehend erklärt (kein Fix nötig):** Jede entpackte Einheit gibt ihre leere Verpackung zurück (Vanilla-Konservation) — die Pickup-Notification im HUD („1 Baggie", „2 Baggies", …) ist korrektes Vanilla-Verhalten und ein Beweis, dass die Materialien konserviert werden. Beim Packen erscheint kein Popup, weil dort nichts in den Besitz des Spielers fließt.
- Live-Verifiziert durch User: Unpackage-Modus funktioniert (v0.3.1), E-Prompt funktioniert (v0.2.9).

## 0.3.1 (2026-09-20) — Unpack-Fix: Vanilla-Mode-Mirror statt Slot-Heuristik
- **Root Cause des "beim Unpacking passiert nichts" (Live-Session 12:36):** v0.3.0 suchte das verpackte Produkt im PRODUCT-Eingangsslot — Vanilla-Unpackage läuft aber **rückwärts**: verpackte Ware liegt im OUTPUT-Slot, Rohprodukt + Verpackung kommen links heraus (der rote UNPACKAGE-Pfeil im Canvas zeigt nach links). Die Heuristik traf nie zu.
- **Fix — zwei Anleihen beim Vanilla statt eigener Logik:**
  1. **Mode-Mirror:** Der Canvas-Modus (Package/Unpackage-Umschalter, der rote Pfeil) wird in die Runtime-Data gespiegelt (`UnpackageMode`), solange die Station geöffnet ist. Nach dem Schließen bleibt der zuletzt gewählte Modus maßgeblich — die Automation folgt der Spielerentscheidung.
  2. **Readiness via `GetState(EMode.Unpackage)`:** Berechtigungscheck ist exakt die Vanilla-Zustandsmaschine (== `CanBegin`), dieselbe Prüfung wie der native Begin-Button — deckt Slot-Layout, Item-Match und Output-Kapazität automatisch ab.
- **Konsequenz:** Pack und Unpack sind jetzt modus-exklusiv (kein Prioritäts-Raten mehr): Unpackage-Modus → Auto-Unpack, Package-Modus → Auto-Pack.
- **Bedienung (unverändert einfach):** Station öffnen (E) → mit dem Pfeil auf UNPACKAGE schalten → verpackte Ware in den OUTPUT-Slot legen → schließen → Station entpackt automatisch zyklusweise.
- Befund der Session: E-Prompt-Fix (v0.2.9) funktioniert — 3× `E-interact: calling PackagingStation.Interacted()` im Log, Canvas öffnete.

## 0.3.0 (2026-09-20) — Auto-UNPACK (native Vanilla-Implementierung)
- **Unpacking implementiert — per Delegation an TVGS-Code statt Reimplementierung:** Zyklus-Commit erkennt selbstständig: Produkt-Slot hält ein verpacktes `ProductItemInstance` (instanz-Level `PackagingID`/`AppliedPackaging`, exakt das, was die Vanilla-Packmaschine setzt) und Packaging-Slot ist leer → Auto-Unpack. Ausführung ist der native `PackagingStation.Unpack()`-Call (vanilla Slot-Mathematik, kein Duplikat unsererseits).
- **Prioritäten-Kette im Commit:** 1. Pack (wenn Packaging-Material + Rohprodukt liegt) → 2. Unpack (wenn verpacktes Produkt liegt, kein Material) → 3. Idle. Entscheidung fällt am Zyklus-Ende anhand der Live-Slots — selbstkorrigierend, wenn der Spieler mid-Animation die Slots wechselt.
- **Kette läuft durch:** `CanStationPackage || CanStationUnpack` steuert jetzt die Fortschaltungs-Logik (State Idle/Blocked/NoPackaging → Packaging) — ein Stapel von 20 Baggies wird also in aufeinanderfolgenden Zyklen komplett entpackt, nicht nur das erste.
- **Output-Capacity-Guard:** Unpack startet nur, wenn das Rohprodukt in den Output passt (leer oder gleiche Definition mit Stack-Spielraum).
- **Diagnose:** Jeder Native-Unpack-Commit loggt Vorher/Nachher-Slotstate (`prod[...] out[...] -> [...]`); wenn `Unpack()` gar nichts bewirkt (z. B. Mode-Mismatch), steht eine Warnung mit Report-Hinweis im Log.
- UI: Canvas-Instruction-Label aktualisiert ("Auto packs & unpacks in background").
- Basieren auf v0.2.9 (E-Prompt-Canvas-Guard-Fix).


## 0.2.9 (2026-09-20) — E-Prompt-Fix (Canvas-Guard)
- **E-Prompt auf der Station war sichtbar, aber tot:** Der InteractableObject-Listener hatte einen Guard `PackagingStationCanvas.Instance != null && !activeSelf` — solange der Canvas-Singleton noch null war (lazy Init, vor dem ersten Vanilla-Station-UI-Öffnen), wurde `Interacted()` stumm übersprungen. Neuer Guard prüft nur noch „Canvas offen FÜR DIESE Station" (Instance + activeSelf + Station-Pointer-Vergleich), null-Instance blockiert nicht mehr.
- **Diagnose-Logging aufgebessert:** `E-interact:`-Info bei jedem Interacted()-Call, Warn wenn der Canvas sich nach dem Call nicht öffnet (Instance null / inactive), Error statt Debug bei Exceptions — vorher in Normalsessions unsichtbar.
- Nebenher: pre-existing CS8625-Warning in `AutoPackStore.cs` Legacy-Migration behoben (`null!` für LoadSafe-Fallback).


## 0.2.8 (2026-09-19) — Bugfix-Runde 7: Placement, Interaktion, Level-Gate
- **Grid-Tile-Überschneidung (HIGH):** `ExpandFootprintTo2x2()` klont die FootprintTile des Basis-Items und erstellt 3 zusätzliche Tiles für ein echtes 2×2-Raster (0.5m-Spacing). Verhindert, dass die Station halb in Regale oder andere Buildables ragt.
- **E-Interaktion auf Kessel (MEDIUM):** `SetupPlacedStation()` fügt jetzt ein `Il2CppScheduleOne.Interaction.InteractableObject` hinzu (Message, Range, onInteractStart → `station.Interacted()`). Das native `PackagingStationCanvas` öffnet sich zuverlässig beim Hovern auf die Station (Kessel).
- **Level-Gate (MEDIUM):** `WithRequiredRank(new FullRank(Rank.Hustler, 1))` — die Station ist ab Hustler I kaufbar (nach Tier 1 PackagingStation und Tier 2 PackagingStationMk2). Nicht mehr ab Level 1 verfügbar.


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
