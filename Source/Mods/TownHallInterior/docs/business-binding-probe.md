# TownHall Business: erste Anbindungsprobe

Stand: 2026-11-03. **Beobachtende Diagnose geprüft: Schema-2-Erstladen und Menü-Reload
auf TownHallTest bestanden. Phase 1 des Fertigstellungsplans ist nicht erfüllt.**
Nächster Schritt: [inaktive native Business-Konstruktion](business-construction-probe.md).

Kandidat `a34e3b4a…` erfasste am 2026-11-03 um 13:12 und 13:16 CET im selben
Prozess je 90 Ereignisse/45 vollständige Aufrufpaare, keine verworfenen Ereignisse
und je zwei vollständige Inventuren. Alle 104 normalisierten Datensätze blieben
gleich; nach Reload wechselten alle 63 Pointer. g0001 behält seine früheren Daten
und ergänzt nur drei Szenenwechsel-Marker; g0002 enthält eigene Puffer.
[Erstladen](evidence/live-slot2-schema2-review.json),
[Reload](evidence/live-slot2-reload-review.json),
[Originalbericht g0002](evidence/live-slot2-reload-g0002.json),
[Logauszug](evidence/live-slot2-reload-log-excerpt.txt).
Unabhängige Prüfung durch Hauptagent und `probe_review`. BusinessLoader.Load ohne
Sichtung bleibt eine Beobachtungsgrenze; native Business-Erzeugung ist nicht bewiesen.

## Bisher beobachtet und gezielte Korrektur

Kandidat `48002119…` wurde auf Dominiks Auftrag installiert. Im bestätigten Testslot
TownHallTest / SaveGame_2 blieben fünf Businesses, sieben Grids, fünf Stationen und
14 Property-Mitgliedschaften über einen vollständigen Prozessneustart semantisch
gleich. Die 104 Datensätze je Inventur enthalten 63 unterschiedliche Pointer; alle
Pointer wechselten zwischen Prozessen. Der Vergleich normalisiert Codes, GUIDs,
Pfade und Rollen, ohne Pointergleichheit zu verlangen. ItemInstance-Inhalte wurden
nicht erfasst. [Vergleichsbeleg](evidence/business-probe-slot2-restart-comparison.json).

Der Vorgänger beschränkte Queue-Ereignisse auf 24, andere Loader-Ereignisse auf 40
und den gesamten Prozess auf 160 Snapshots. Im Slot-2-Lauf wurden 64 Ereignisse
gespeichert und 26 weitere verworfen; beim nächsten Laden war der gemeinsame
Snapshot-Puffer beinahe voll. Der aktuelle Code behebt diese Diagnosebegrenzung:
separate Loader-Aufzeichnung, getrennte Budgets je Ladegeneration und vorherige
Berichte bleiben erhalten. Ein höheres Budget allein ist kein Vollständigkeitsbeweis;
echte Überläufe und fehlende Call-Paare bleiben sichtbar.

## Ergebnis und nächste technische Frage

Der erste ausführbare Schritt beobachtet den nativen Bestand und den Ladeablauf.
Er erzeugt kein Business, Grid oder Stationsobjekt und verändert weder Besitz noch
Geldwäsche, Save-Daten oder die bestehenden Business-Listen. Der reguläre
TownHall-Innenraum bleibt Teil der DLL; diese Aussage über Schreibfreiheit gilt
für den neuen Beobachter, nicht pauschal für sämtliche Innenraumfunktionen.

Der anschließende Adapter muss ein natives Business rechtzeitig registrieren und
seine Referenzen eindeutig auf das Rathaus ausrichten. Die bisherigen
IL2CPP-Proxy-Signaturen beweisen weder einen sicheren Awake-/Aktivierungspfad noch
die Reihenfolge des nativen Ladecodes. Ohne diese Evidenz wäre ein neu erzeugtes
Business ein ungesicherter Eingriff in die persistenten Spielobjekte.

## Quellbefunde

- MapExpansions 1.0.1, Commit `9af7e60f3ba7e2fc3fc55de66de945f805d9d5ec`:
  `Api/PlaceDefinitions.cs`, `World/Properties.cs` und
  `Api/PropertyExtensionDefinition.cs` bieten Vorlagenklone und Erweiterungen einer
  bereits bekannten Property. Ein direkter Anschluss an das vorhandene Rathaus ist
  nicht nachgewiesen. Die Vorlagenlogik warnt bei externen Referenzen, bindet sie
  damit aber nicht automatisch neu. Daher keine neue Framework-Abhängigkeit für
  diese Probe; ein eigener nativer Adapter ist der untersuchte Weg.
- S1API-Submodul `6b6d7bada39054822732224508cf0f4f45ee7c26`,
  `S1API/Internal/Patches/GenericSaveables.Patches.cs`, `BeforeBaseLoaders`:
  vorhandene Mod-Save-Ordner werden vor Base-Loadern geladen. Fehlt der Ordner,
  wird `CreateInternal` erst an `onLoadComplete` gehängt. Das Attribut für frühes
  Laden ist deshalb allein kein garantierter Erzeugungszeitpunkt beim ersten Laden.
- Derselbe Patch speichert Moddaten nach `SaveManager.Save(string)`. Daraus folgt
  kein gemeinsamer atomarer Commit mit dem nativen Business-Save.
- Installierte Proxy-Assembly `Assembly-CSharp.dll`:
  SHA-256 `346205306273805a78b01a917f14b94518498af1361595c4c417baa0b32b4bd8`.
  Business-/Property-Listen, Grid-/Stationsreferenzen und die unten genannten
  Load-Signaturen wurden gegen diese Fassung abgeglichen. Das sind Metadaten und
  Proxy-Feldzugriffe, kein dekompilierter nativer Kontrollfluss.
- Installierte S1API-DLL:
  SHA-256 `d751a3cb66003f7a616eb9f8054daf34da0d9d7f23416a5005a586738a680852`.

## Was die Diagnose erfasst

`BusinessBindingProbe.cs` wird nur mit `S1TownHallBusinessProbe=true` eingebunden.
Der Harmony-Eigentümer `TownHallInterior.BusinessBindingProbe` installiert
beobachtende void-Prefixe/-Postfixe an exakt geprüften Überladungen:

- `LoadManager.QueueLoadRequest(LoadRequest)`
- `PropertiesLoader.Load(string)` und `BusinessesLoader.Load(string)`
- `BusinessLoader.Load(string)`
- `PropertyLoader.Load(PropertyData, string)` und `Business.Load(PropertyData, string)`

Die Beobachter ändern keine Argumente, liefern keine Ersatzresultate und
unterdrücken die Originalmethode nicht. Queue-Sichtungen sind **keine** Zuordnung
konkreter Queueeinträge zu späteren Loader-Aufrufen. Ein fehlender Postfix kann
unter anderem durch eine Ausnahme im Original entstehen und bedeutet keinen
erfolgreichen Abschluss. Harmony-Metadaten belegen keine tatsächliche IL2CPP-Ausführung.

Hinzu kommen die tatsächlich empfangenen S1API-Ereignisse `OnPreLoad`,
`OnLoadComplete`, `OnPreSceneChange` sowie Melon-Main-Szenenmarker.
Die Generation bezeichnet einen lokalen Markerzyklus, keine bestätigte
Spielstandidentität. Unklare Ereignisfolgen werden als unvollständig markiert.

Bei `OnLoadComplete` und nach fünf Sekunden in Main erfasst die Probe:

- Business-Identität, Code, Besitz-/Ladeflags und Listenmitgliedschaften;
- Container, Worldspace-UI, Spawn-/NPC-/Interior-Punkte, Bounds, Idle-Punkte;
- Zielpfad, Szene und interne/externe Hierarchiezuordnung der Business-Referenzen;
- Grids mit GUID und ParentProperty sowie Stationen mit GUID, ParentProperty,
  OwnerGrid, ItemInstance und `Interface.Business`;
- das vorhandene Rathausobjekt am bekannten Szenenpfad.

Prefab-/Asset-Stationen ohne geladene Szene sind ausgeschlossen. Nullziele werden
als solche ausgewiesen; ihre Zulässigkeit wird nicht erfunden. Lesefehler sind von
einer lesbaren leeren Liste getrennt. Es werden keine nativen Objektreferenzen über
Frames hinweg gehalten. Pointer sind nur innerhalb einer Bestandsaufnahme vergleichbar.

Die Schema-2-Berichte liegen unter
`UserData/TownHallInterior/BusinessProbe/<run>-g<generation>.json`;
`g0000` enthält die Setup-Phase, jede folgende Ladegeneration erhält eine eigene
Datei. Ihr Pfad steht im MelonLoader-Log. Die Berichte enthalten DLL-/Proxy-/S1API-Hashes,
Hook-Metadaten, `LoaderEvents` und getrennte `Snapshots` für Marker und Inventuren.
Eine gemeinsame `Sequence` erhält die Reihenfolge auch innerhalb desselben Frames.
`PreviousReport` verknüpft die Dateien desselben Prozesses. Sie exportieren keine
Save-Pfade oder Geldbeträge. Fehler im Beobachter dürfen die bestehende Hall-Erzeugung und
deren Cleanup nicht verhindern.

## Grenzen und Prüfung

Je Ladegeneration sind höchstens 64 Snapshots und separat 4.096 Loader-Ereignisse
vorgesehen. Ein Snapshot enthält höchstens 512 Entitäten und 32 Fehler; native
Listen werden höchstens bis 128 Einträge gelesen. Neue Generationen beginnen mit
leeren Puffern. Dadurch können die Ereignisse eines früheren Ladevorgangs die
Inventuren des nächsten nicht verdrängen. Jedes tatsächliche Limit bleibt explizit
`incomplete`; `DroppedLoaderEvents` ist dem einzelnen Bericht zugeordnet.

Vor dem Wechsel wird der alte Bericht erfolgreich geschrieben. Scheitert dies,
bleiben seine Daten erhalten und nur der Beobachter wird deaktiviert. Call-IDs und
Sequenzen laufen innerhalb des Prozesses weiter. Ein über den Wechsel hinaus
offener Call bleibt im alten Bericht unvollständig; ein späteres Exit ohne Enter
im neuen Bericht wird ebenfalls beanstandet. Diagnosefehler blockieren keine
bestehende Hall-Erzeugung oder deren Cleanup.

`ProbeTrace.Analyze` meldet nur `observed` oder `incomplete`. Geprüft werden
Leselücken, Grenzen, fehlende Business-Codes, Grid-/Stations-GUIDs und sichtbare
Code-/GUID-Kollisionen. `ProbeLoaderTrace.Analyze` prüft zusätzlich Reihenfolge,
Call-ID, Methode, Generation und passende Enter-/Exit-Paare; offene, doppelte oder
unpassende Paare sind unvollständig. `SnapshotAssessment` und `LoaderAssessment`
bleiben im Bericht getrennt, `Assessment` fasst beide zusammen. Im Setup-Bericht
sind fehlende Loader-Ereignisse erwartbar; das ist kein behaupteter Spielfehler.
`RecordedEnters`/`RecordedExits` je Hook zählen gespeicherte Sichtungen. Null bedeutet
„nicht beobachtet“, auch bei installiertem Owner. **`observed` ist keine Bindungsfreigabe**: die Auswertung
beweist weder Hook-Abdeckung noch Registrierung, vollständige Referenzgraphen,
native Save-Reihenfolge oder einen sicheren Erzeugungsweg.

Die gamefreien Tests prüfen die Auswertung, Alias-/Duplikatfälle, Fehlergrenzen,
Markerzyklen und erfolgreichen/fehlgeschlagenen Berichtswechsel. Eine synthetische
90-Ereignis-Regression bildet die gemessene Gesamtmenge ab (48 Queue-, 42 andere
Ereignisse); sie rekonstruiert nicht die im Original ausgelassenen Aufrufe.
Drei aufeinanderfolgende Läufe behalten getrennt je 90 Ereignisse und zwei
Inventuren. Ein No-Deploy-Build prüft die Kompatibilität mit den lokalen
Referenzassemblies. Beides ersetzt keinen nativen Test. Prüfprotokolle und
Artefakthashes werden beim Kandidaten separat festgehalten.

Build bei geschlossenem Spiel:

```powershell
dotnet build Source/Mods/TownHallInterior/src/TownHallInterior.csproj -c Release -p:S1NoDeploy=true -p:S1TownHallBusinessProbe=true
dotnet test Source/Tests/TownHallInterior.Tests/TownHallInterior.Tests.csproj -c Release
```

Ohne den Probe-Schalter werden weder Probe-Hooks noch ihre Lifecycle-Abonnements
installiert. Die Entwicklungsversion bleibt `0.1.0`; Kandidaten werden über ihre
Hashes unterschieden. Kein Release-Paket mit den lokalen extrahierten Assets.

## Begrenzter Ingame-Ablauf nach vereinbarter Installation

1. Aktuellen Testslot und tatsächlichen Cloud-/Backup-Zustand feststellen; alte
   SaveGame-1/2-Angaben sind keine aktuelle Freigabe. Spiel schließen, installierte
   DLL sichern, den konkret gehashten Kandidaten installieren und nachprüfen.
2. Dominik steuert das Spiel. Einen bestätigten Testslot laden und nach dem
   vollständigen Laden mindestens fünf Sekunden warten. Die Probe löst selbst
   keinen Speichervorgang aus; das Spiel kann seine üblichen Autosaves ausführen.
3. Neuesten Schema-2-Ladebericht und Log prüfen: richtige Hashes, bestätigter
   Testslot, beide Inventuren, `DroppedLoaderEvents=0`, passende Call-Paare und
   keine Diagnose-Ausnahmen. Ein Setup-Bericht oder Bericht nur mit Szenenmarkern
   genügt nicht; ein nicht gesichteter Hook ist gesondert einzuordnen.
4. Erst bei brauchbarem ersten Bericht Menü und erneutes Laden desselben Testslots
   untersuchen. Es muss eine neue Generationsdatei entstehen und die vorherige
   erhalten bleiben. Markerzyklen und GUIDs vergleichen; wechselnde Pointer sind normal.
   Kein Kauf und keine Geldwäsche sind Gegenstand dieses Schritts.
5. Aus dem nativen Befund den kleinsten Erzeugungs-/Registrierungsadapter ableiten.
   Dann genau eine Rathaus-Business-Instanz, Station sowie Save-/Reload-Weg prüfen.
   Erst dieser separate Nachweis kann Phase 1 abschließen.

Verbindungen: [Fertigstellungsplan](business-completion-plan.md),
[Innenraumstatus](README.md), [Projektregeln](../../../../AGENTS.md).
