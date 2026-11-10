# Hallenobjekte und Cabinet-Inhalte — Datenverlustprüfung 2026-11-10

## Meldung und beobachteter Stand

Dominik meldet nach Save/Reload: ursprünglich zehn große Lagerregale, eines in der Halle platziert, eines in einem Cabinet abgelegt; danach acht im Inventar und beide außerhalb des Inventars nicht sichtbar.

Der gelesene native Slot-2-Save enthält acht `largestoragerack` im Inventar und keinen weiteren solchen Gegenstand in seinen übrigen JSON-Dateien. Der passende Rathaus-Checkpoint `57CDC2FF…` enthält weiterhin eine Hallenplatzierung mit leerem `Contents`. Ein fehlender sichtbarer Gegenstand ist damit nicht automatisch aus allen Datensätzen gelöscht. Eine Rückbuchung wurde nicht vorgenommen.

Der private Beobachtungsstand liegt separat unter `.backups/townhall-item-loss-20261110T184253`. Die Kopie wurde lesend erstellt und ist keine atomare Live-Sicherung. Der anschließende Hashvergleich der nativen Dateien meldet keine Änderungen. Save-Recovery und Kompensation sind nicht Teil dieser Korrektur.

## Belegte Ursachen

1. **Veraltete Weltbindung:** `HallGrid.Dispose`, `Bind`, `TryPublish` und `TryRestore` ließen die alte `_records`-Liste über den Weltwechsel bestehen. Im Log wird um 18:40:47 ein Hallenobjekt materialisiert, die neue Weltbindung erfolgt erst um 18:40:55. Das belegt vorzeitigen Restore; die genaue native Entfernung des sichtbaren Objekts wurde nicht beobachtet.
2. **Cabinets ohne angeschlossenen Inhaltswriter:** `TownHallCabinetStorage.SpawnOne` erzeugte nur einen Prefab-Klon. Weder der native Property-/Buildable-Loader noch der Rathauscheckpoint erfassten dessen Inhalte. `Mod.OnNativeSaveStart` erfasste ausschließlich Hallenplatzierungen. `AssignStableGuid` suchte CLR-Stringfelder, ist aber kein Nachweis für eine native GUID-/Saveable-Registrierung.
3. **Verlust individueller Itemdaten:** Das bisherige `HallGrid.ReadContents`/`FillContents` speicherte nur Item-ID, Menge und Fach; `GetDefaultInstance` rekonstruiert keine individuellen Attribute.

Native Verträge wurden gegen die vorhandenen IL2CPP-Proxys und durch den Build gegen die Spielreferenzen geprüft. Die Proxys zeigen Signaturen, keine native Implementierung. Der konkrete Platzierbares-Lager-Loader ist in `ThirdParty/S1API/S1API/Internal/Patches/StoragePatches.cs`, `PlaceableStorageEntityLoader_Load_Prefix`, nachvollziehbar: Platzierung erzeugen, anschließend `Contents.LoadTo(storageEntity.ItemSlots)`.

## Offline-Korrekturkandidat

- `HallGrid.Dispose` trennt die alte Checkpoint-Liste. Vor neuer Weltbindung wird nichts daraus wiederhergestellt; ungebundenes Platzieren wird verweigert. `RememberPlacement` muss einen gültigen Record erzeugen, bevor Inventar abgezogen wird.
- `TownHallBusinessData.CabinetContents` ist eine optionale, standardmäßig leere Sammlung; Schema 2 bleibt erhalten. Cabinet-Identitäten sind die stabilen Namen des Möbelplans, nicht vermeintlich gesetzte native GUIDs.
- `NativeHallStorage` nutzt den nativen `ItemSet`-Codec einschließlich individueller Itemdaten und Fächer. Leere native Einträge sind reguläre `ItemData` mit leerer ID und Menge 0; diese Form wurde im tatsächlichen Spielsave gefunden und in einem zunächst roten Regressionstest berücksichtigt.
- `NativeHallStorage.Read` prüft Anzahl/Initialisierung der Slots und vergleicht belegte Slots unabhängig vom Serializer auf ID und Menge. `Restore` prüft Deserialisierung und liest den geladenen Zustand zurück.
- `Mod.BeforeNativeSave` erfasst die Lager und führt die dauerhafte `TownHallSaveSession.Prepare` vollständig aus, bevor der native Writer zugelassen wird. Auch ein Sidecar-Schreibfehler verweigert damit den nativen Save. `OnNativeSaveStart` bestätigt nur noch diesen vorbereiteten Auftrag; der bestätigte native Abschluss veröffentlicht ihn über `Complete`. Überlappende Aufrufe werden abgewiesen; fehlt der Start-Handshake, bleibt der Pending-Stand geschützt.
- Unbekannte Cabinet-Records bleiben bei Capture erhalten. Ein fehlgeschlagener Restore darf weder durch Pickup noch durch einen neuen Capture seinen gespeicherten Inhalt verlieren. Betroffene Projektionen bleiben deaktiviert und der neue Save wird verweigert.
- Cabinets werden vor dem Spawn als checkpoint-eigen registriert; der erfolgreiche `InitializeSaveable`-Ausschlusshook ist Voraussetzung. Ein paralleler nativer Inhaltswriter ist nicht beabsichtigt; das tatsächliche Hook-/Codec-Verhalten ist noch nicht nativ beobachtet.

## Prüfungen und Identität

[Maschinenlesbarer Nachweis und Quellhashes](evidence/item-loss-20261110/verification.json), [TRX](evidence/item-loss-20261110/townhall-item-loss.trx).

- 1.248/1.248 TownHall-Unit-Tests bestanden, einschließlich eingefrorenem Cabinet-Snapshot mit Item-Metadaten, Savefehler, Rollback, fehlenden neuen Feldern in alten Checkpoints, doppelter Cabinet-ID, nativer Leerslot-Repräsentation und echtem Pending-Schreibfehler im Testdateisystem.
- Release-Build mit `S1NoDeploy=true`: 0 Warnungen, 0 Fehler. Whitespace-Prüfung bestanden.
- Separat im abschließenden isolierten Snapshot 1.248/1.248 TownHall-Tests bestanden ([TRX](evidence/item-loss-20261110/townhall-final-isolated.trx)); kopierte Produktionsdateien und geänderter Test hashgleich zum finalen Arbeitsstand. Die Submodulinitialisierung des Snapshot-Helfers lief in den Timeout; diese gezielten game-free Tests benötigen die Submodule nicht. Das vollständige isolierte Pure-Gate ist **nicht bestanden**: erster Lauf wegen TEMP-/ScratchRoot-Überlappung abgewiesen; zweiter Lauf nach 420 Sekunden Tool-Timeout beendet. Erfasste Einzelchecks melden vorhandene Formatstellen in `RoomPlan.cs` und Versionsdrift in HitmanPhone/Minimap/PoliceTestMode. Die bestandene Whitespace-Prüfung ist `git diff --check`, keine bestandene vollständige Formatprüfung.
- Exakte Produktionsmethoden wurden in kleinen Replay-Hosts ausgeführt: Dispose/Bind/Restore-Reihenfolge, Save-Prefix-Verweigerung, Erhalt unbekannter Cabinets bei Capture sowie nativer Lageradapter mit vier Szenarien (Metadaten/Fach, fälschlich leere Serialisierung, fehlendes Item, wirkungsloser Load). Die ersten drei Fehler wurden vor ihrer jeweiligen Korrektur rot beobachtet.
- **Replay-Grenze:** Unity, Spawn und nativer ItemSet-Codec werden durch Testgrenzen ersetzt. Das ist keine Unity-/IL2CPP- oder Gameplay-Abnahme.
- Installiert seit 2026-11-10, 19:26 CET: `9238c44efb1804a3f63393cff85f5915f032b17a5732c20b9b610601a0fd372c`, **noch nicht gestartet oder im Spiel geprüft**. Nur TownHall-DLL/PDB ersetzt; Metadaten/Icon unverändert. Vier Ziele und zwei Backupdateien unabhängig gelesen und hashgeprüft; Slot 2, Shared und andere beobachtete Moddateien unverändert. [Installationsnachweis](evidence/item-loss-20261110/deployment-verification.json), [Quittung](evidence/item-loss-20261110/deploy-receipt.json), [Backupmanifest](evidence/item-loss-20261110/deploy-backup-manifest.json).
- Vorherige DLL `8af979a80e89c4824f812c0528bcdf769da2669cef4635543f5e162ef6f8ca64` im verifizierten Helper-Backup `20261110T182612541Z-TownHallInterior-6a560bf1`. Kandidatenkopie unter `.backups/townhall-item-loss-candidate-final-20261110`. `d219cc06…` ist überholt und nicht zur Installation bestimmt.
- Der unabhängige Delta-Review benannte im Vorläufer als letzten Blocker die zu späte dauerhafte `Prepare`. Der finale Mod-Delta verschiebt sie in den abbrechbaren Prefix. Dessen exakte Produktionsmethoden wurden für Capture-/Prepare-Fehler, erfolgreiche Vorbereitung, Doppelaufruf, Start-/Finalizer-Handshake und fehlenden Callback geprüft; der Prepare-Fehler war vorher rot. Kein erneuter unabhängiger Review dieses letzten Deltas und kein nativer Save-Test behauptet.

## Eigentümerabnahme und Prüfgrenzen

**Teilabnahme, Dominik, 2026-11-10:** „Das cabinet hat diesmal das regal gespeichert“. Cabinet-Inhalt nach dem vereinbarten Save-/Menü-Reload damit eigentümerbestätigt. Log 19:34:18 erfasst native Halleninhalte und elf Cabinets; nach neuer Weltregistrierung 19:34:50 werden Hallenobjekt und dessen native Inhalte wiederhergestellt. Der letzte Logeintrag beweist nicht deren sichtbare Vollständigkeit; diese Rückmeldung bleibt offen. [Gesicherter Logauszug](evidence/item-loss-20261110/cabinet-live-acceptance.log).

**Nachfolgende Bestätigung, Dominik, 2026-11-10:** Auf die ausdrückliche Frage, ob der Gegenstand im Fach des selbst aufgestellten Hallen-Regals nach dem Neuladen erhalten blieb: „ja ist erhalten geblieben“. Damit sind Cabinet- und Hallenfach-Inhalt nach Save/Menü-Reload eigentümerbestätigt und der vereinbarte Test der Lagerkorrektur abgeschlossen. Die vorangehende offene Rückfrage ist hiermit erledigt.

Vollständiger Prozessneustart, individuelle Itemdaten und wiederholte Zyklen sind nicht separat geprüfte Evidenzgrenzen, kein neuer Auftrag. Abgenommene Arbeit nicht daraus wieder öffnen. Keine neuen Code-, Installations- oder Save-Eingriffe für diese Dokumentation.
