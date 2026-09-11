# Changelog


## 0.1.7 (2026-09-11)
- Place-Prefix mit Host-Guard (Clients fallen auf Vanilla zurueck, kein lokaler Desync-Fork).
- Wipe-if-Switched nach OnSaveInfoLoaded verlegt (kein Full-Wipe bei Same-Slot-Reload mehr).
- Sleep-Flag mit 5-Min-Stale-Guard (kein Quest-Credit durchs naechste Vanilla-Bett).

## [0.1.6] - 2026-09-10
* Place-Prefix-Catch weist __result=null zu (nativer Crash-Vektor geschlossen).
* Quest-Completion bleibt in RAM, Flush auf OnSaveComplete (kein Save-Rollback-Drift mehr).
* SleepingBagItemFactory re-verifiziert Registry.ItemExists (Fix stale _isRegistered nach Menue-Reload).

## [0.1.5] - 2026-09-09

### Fixed (Gatekeeper-Review-Runde, 5 Befunde)
* **Kritisch — Item-Verlust bei PackUp ohne ItemID**: `OutdoorItemInteractable.PackUp()` zerstörte das Objekt und deregistrierte es, wenn `_itemId` leer und nicht rekonstruierbar war — der Spieler verlor das platzierte Objekt ersatzlos. Jetzt: früher `return` **ohne** Destroy/Unregister (gleiches Muster wie `SleepingBagInteractable.PackUp`). Objekt bleibt in der Welt, Log-Warn zeigt den Grund.
* **Hoch — Quest 2 zählte jedes Outdoor-Item**: „Place a workstation or grow container outdoors" schloss auch beim Platzieren des Sleeping Bags (Quest-1-Item) ab. Neu: `IsProductionGear()` prüft zuerst die Welt-Komponenten des platzierten Objekts (Pot/MixingStation/Mk2/Cauldron/ChemistryStation/PackagingStation/BrickPress/DryingRack — alle im Live-Decompile verifiziert), dann String-Fallback („station"/„press"/„cauldron"/„rack"/„…pot") für modded Gear. Sleeping Bag zählt nicht mehr.
* **Mittel — Quest-Credit beim Einschlafen statt Aufwachen** (altes M3-TODO): `NotifyPlayerSlept()` feuerte bei `StartSleep()`. Abgebrochener Schlaf zählte trotzdem. Neu: Flag `_sleepStartedInBag` bei Start, Credit erst im `S1API.GameTime.TimeManager.OnSleepEnd`-Wake-Hook (Action&lt;int&gt;, im Live-Decompile verifiziert). Vanilla-Bett-Schlaf zählt weiterhin nicht.
* **Niedrig — Rückseiten-Beleuchtung des Sleeping-Bag-Mesh**: `AddQuad()` nutzte für die doppelseitigen Rückseiten-Dreiecke die Front-Normalen → falsche Schattierung von unten. Neu: eigene Rückseiten-Vertices mit `-normal` und umgekehrter Wickelreihenfolge.
* **Niedrig — ResetState-Asymmetrie**: `HomelessQuestManager.ResetState()` hatte keinen `keepSlot`-Parameter (anders als `StreetPropertyManager`) und löschte den Slot-Cache auch bei reinem Menü→Spiel-Reload. Neu: `ResetState(bool keepSlot = false)` + `ResetForSceneUnload()`; `OnSceneWasUnloaded` nutzt jetzt keepSlot:true.

## [0.1.4] - 2026-09-09

### Fixed
* **Quest 'Alley Operations' sprang nach Save-Reload auf 'offen' zurück** (Root Cause: Reihenfolge-Abhängigkeit). Der Beide-Ziele-Check (`MarkQuestCompleted`) lag ausschließlich im Geld-Zweig von `CheckCashProgress` hinter `if (earn500.State != Completed)`. Schloss das Geld-Ziel VOR dem Gear-Ziel ab (z. B. ≥500 $ auf Konto, Workstation später platziert), lief dieser Pfad nie wieder → Quest wurde nie in `quest_progress_slot_N.json` persistiert → nach Neustart erzeugte `InitializeQuests` sie frisch mit offenen Entries. Neu: `TryFinalizeQuest2()` prüft beide Entries bei **jedem** Trigger (Platzieren + Cash-Poll) — Reihenfolge egal.
* **Quest 'Street Sovereign' konnte verfrüht abschließen** (umgekehrtes Muster): `MarkQuestCompleted` feuerte beim $5.000-Ziel allein, ohne das Camp-Entry zu prüfen. Neu: `TryFinalizeQuest3()` verlangt beide Entries.

### Notes
* Bestehende Saves heilen selbstständig: nach dem Update einmalig Gear draußen platzieren → Quest schließt dauerhaft ab.
* Versions-Drift repariert: `mod.json` hing auf 0.1.2 zurück (Mod.cs war 0.1.3) — beide jetzt 0.1.4.

## [0.1.2] - 2026-09-08

### Fixed
* **Sleeping Bag schwebte ~1,5m über dem Boden**: Die Item-Definition wird per `CloneFrom("bed")` vom Bett geklont — Vanilla `BuildUpdate_Grid` leitet daraus einen bett-hohen `verticalOffset` ab, der den flachen prozeduralen Schlafsack in die Luft hob.
  * Ghost-Fix: Für den Sleeping Bag wird der Vanilla-`verticalOffset` jetzt ignoriert (nur Pivot-Korrektur bleibt) — der Ghost liegt beim Platzieren direkt auf dem Boden (`BuildingPatches.BuildUpdate_Grid_CheckIntersections_Patch`).
  * Save-Heilung: Beim Restore wird die gespeicherte Position per Boden-Raycast nach unten gesnappt (`GroundPlacementAssistant.SnapToGround`). Alte Saves mit schwebenden Bags werden automatisch korrigiert; Bags auf Dächern/Brücken (≤0,5m über Fläche) bleiben korrekt liegen. Log-Zeile `[GroundFix]` zeigt die Korrektur an.
