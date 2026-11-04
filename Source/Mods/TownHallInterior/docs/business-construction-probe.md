# TownHall: inaktive native Business-Konstruktion

Stand 2026-11-03. Kandidat `52bb577a…`, lokale Entwicklungsfassung 0.1.0.
**Nativer Konstruktionstest auf TownHallTest bestanden (14:04:58 CET).** Dies ist ein
Schritt zum eigenen Adapter, noch keine registrierte Business-Instanz und kein
Abschluss von Phase 1 des [Fertigstellungsplans](business-completion-plan.md).

## Befund und enges Ziel

Die [beobachtende Probe](business-binding-probe.md) erfasst Erstladen und Menü-Reload
auf TownHallTest vollständig. Die installierten IL2CPP-Proxys belegen Signaturen,
aber keine vollständigen nativen Awake-/Start-/Speicherrümpfe. Ein nacktes Business
sofort zu aktivieren würde mehrere ungeprüfte Registrierungen verbinden.

Dieser Kandidat konstruiert deshalb einen echten nativen `Business` und einen
eigenen `NetworkObject` unter einer dauerhaft inaktiven Wurzel. Er konfiguriert
eigene Referenzen, liest sie zurück, fragt `GetSaveString` ab und entfernt alle
eigenen Objekte wieder synchron. Die zehn erfassten Registrierungslisten müssen
vorher, nach Konstruktion und nach Rückbau exakt dieselben geordneten Pointer
enthalten. Das ist mehr als eine Bestandsinventur und bleibt eine begrenzte Probe.

## Zuständigkeiten und Grenzen

- Implementierung: `src/BusinessConstruction.cs`, reine Orchestrierung und nativer
  Port derselben Konstruktionsdomäne; die Mod bleibt bei **20 Produktionsdateien**
  einschließlich Projektdatei. Tests, Dokumente und Assets zählen separat.
- Ausführung nur nach `OnLoadComplete` plus sechs Sekunden, in Main, mit bereiter
  Halle, bestätigtem Singleplayer ohne Lobby und genau einem Spieler.
- `ActiveSaveInfo.SavePath` und `LoadedGameFolderPath` müssen nach Normalisierung
  identisch und absolut sein, ihr letzter Ordner `SaveGame_2` heißen und
  `OrganisationName` genau `TownHallTest` sein. Ein Slotname allein genügt nicht.
  Andere Spielstände werden mit `skipped` protokolliert.
- Ein tatsächlicher Versuch pro Prozess; auch ein unbekannter Fehler verbraucht
  ihn. Fehlende Kontextvoraussetzungen erzeugen kein Objekt. Kein automatischer
  zweiter Konstruktionsversuch nach einem Fehler.
- Feste technische Identität `townhallinterior_townhall`, Anzeige `Hyland Town Hall`.
  Vor Konstruktion wird ein vorhandener gleichnamiger nativer Property-Code
  abgewiesen. Preis, Mitarbeiterzahl und Geldwäschekapazität sind in dieser
  unpublizierten Probe null; das sind keine Balanceentscheidungen.
- Eigene leere Collections/Ereignisse, Spawnpunkte, Bounds-Trigger,
  Contents-Container und Worldspace-RectTransform. Beide verschiedenen Loader-
  Felder (`Property.loader` und `Business.loader`) werden separat initialisiert.
  Der Contents-Container wird direkt über sein eigenes Feld gebunden; keine
  native `SetProperty`-Initialisierung. NetworkBehaviour-Felder werden direkt
  geschrieben; kein `NetworkInitializeIfDisabled` oder Prefab-/Server-Spawn.
- Keine zweite Gebäudegeometrie, keine Vanilla-Klone, keine Station, keine Tiles,
  kein Kauf oder Eigentumswechsel, keine Registrierung in Spiel- oder Save-Listen.
  `Load`, `InitializeSaveable`, `WriteData` und `Save` werden nicht aufgerufen.
- Sieben exakt aufgelöste void-Lifecycle-Ziele schützen ausschließlich die eigene
  Root-Identität: Business/Property Awake und Start, Property.InitializeSaveable,
  NetworkObject Awake und Start. Fremde Instanzen passieren unverändert. Ein
  unerwarteter eigener Aufruf wird blockiert und führt zu `failed`; Owner-Metadaten
  allein beweisen keine Wirksamkeit in IL2CPP.

Die gesetzten Felder, einschließlich FishNet-Caches, sind gegen die installierten
Proxy-Signaturen geprüft. Native Initialisierung, Serialisierung und Zerstörung
müssen erst im Spiel beobachtet werden. Das native Verhalten einer nie aktivierten
Instanz wird nicht aus erfolgreichen Offline-Tests abgeleitet.

## Ablauf und Fehlerfälle

Kontext bestätigt → Registerkopien → eigene inaktive Shell → Referenz-Readback →
erneuter Registervergleich → begrenzte native Serialisierung → eigener Rückbau →
abschließender Registervergleich. Nach jedem begonnenen Konstruktionsversuch läuft
Cleanup, auch bei fehlenden Referenzen, ungültigem JSON oder nativen Ausnahmen.

Das Ressourcenjournal erfasst die Wurzel vor `AddComponent` und jedes Kind vor dem
Parenting. Dadurch bleiben auch Kinder eines fehlgeschlagenen Parenting-Schritts
erreichbar. Rückbau betrifft ausschließlich diese Objekte. Eine unlesbare native
Abwesenheit gilt nicht als erfolgreiche Zerstörung. Maximal zwei Cleanup-Versuche,
danach bleiben Handles und Schutzpatches erhalten; die bestehende Halle wird
weiter unabhängig aufgeräumt. Es werden keine fremden Register zurückgesetzt.

Der Vergleich erfasst Property-/Business-Gesamt-, Owned- und Unowned-Listen sowie
Saveables, BaseSaveables, CompletedSaveables und QueuedSaveRequests. Pointerfolge,
Listenidentität und Anzahl werden begrenzt gelesen und gehasht. Der zusätzliche
Lifecycle-Indikator erfasst auch unerwartete Aufrufe während Serialisierung oder
Cleanup. Diese Auswahl beweist keine umfassende native Nebenwirkungsfreiheit.

Native JSON-Ausgabe ist auf 16.384 Zeichen begrenzt. Geprüft werden Objektform,
eindeutige Root-Schlüssel, eigener PropertyCode, `IsOwned=false` und eine leere
`LaunderingOperations`-Liste. JSON wird ausschließlich als Diagnose unter UserData
archiviert; es ist kein nativer Save- oder Wiederherstellungsnachweis.

## Artefakte und Offline-Prüfung

Build-Schalter `S1TownHallBusinessConstruction=true` aktiviert die Konstruktion und
die vorhandene Schema-2-Inventur. Normale Builds enthalten keinen dieser beiden
Laufzeitadapter. Der Inventurbericht weist den zusätzlichen Konstruktionstest als
`observation-with-inactive-construction` aus; seine frühere reine Beobachtungszusage
gilt nicht unverändert für den kombinierten Kandidaten.

```powershell
dotnet build Source/Mods/TownHallInterior/src/TownHallInterior.csproj -c Release -p:S1NoDeploy=true -p:S1TownHallBusinessConstruction=true
dotnet test Source/Tests/TownHallInterior.Tests/TownHallInterior.Tests.csproj -c Release -p:S1NoDeploy=true
```

**229 TownHall-Tests bestanden**, darunter Fehlerpfade, doppelte Versuche,
verwechselte Testwelten, Snapshot-Aliasing, späte Lifecycle-Signale und unlesbarer
Cleanup-Zustand. Kandidat und Normal-Build: jeweils 0 Warnungen/Fehler.
Unabhängiger Quellreview durch `probe_review`: die zwei gefundenen Probleme bei
Cleanup-Fehlerweitergabe und verspäteten Lifecycle-Signalen sind behoben.
[Offline-Artefakte und Quellhashes](evidence/business-construction-offline.json),
[Testbericht](evidence/business-construction-tests.trx).

Die vier gepinnten Abhängigkeiten sind im Offline-Beleg benannt. Insbesondere
FishNet `_componentIndexCache` wird direkt gesetzt; der alternative
`ComponentIndex`-Setter würde bereits eine native Methode aufrufen.

## Bestandener nativer Test und damaliger Ablauf

Der [Live-Bericht](evidence/live-slot2-construction.json),
[Logauszug](evidence/live-slot2-construction-log-excerpt.txt) und
[unabhängig bestätigte Auswertung](evidence/live-slot2-construction-review.json)
belegen `observed`, keine Findings, `CleanupComplete=true`, null unerwartete
Lifecycle-Aufrufe und zehn unveränderte Register. `GetSaveString` lieferte 282 Zeichen
gültiges natives BusinessData-JSON mit eigener Kennung, unbesessen und leeren Listen.
Die begleitende Inventur liegt zeitlich vor der Konstruktion; der Nachher-Nachweis
kommt aus `Result.After`. Damit ist diese begrenzte Probe abgeschlossen.
Nachfolger ist die [kontrollierte aktive Registrierungsprobe](business-activation-probe.md).

Vor einer Installation den geschlossenen Prozess, aktuelle Cloud-Einstellung und
den Vorher-Checkpoint festhalten. Dominik startet über Steam und lädt ausschließlich
TownHallTest / Slot 2. Nach zehn Sekunden Wartezeit stehen Inventur und ein separater
Bericht unter `UserData/TownHallInterior/BusinessConstruction/` bereit; dessen Pfad
und Status erscheinen im MelonLoader-Log. Keine zusätzliche Bedienung nötig.

Erwartet: `observed`, keine Findings, `CleanupComplete=true`,
`UnexpectedLifecycleCalls=0`, gleiche Registerfingerprints und eigene gültige
Business-JSON-Ausgabe. Das Rathaus bekommt durch diesen Test noch kein sichtbares
Kaufangebot oder Geldwäschemenü. Bei `failed` oder `skipped` erst den konkreten
Bericht prüfen; nicht durch weitere Loads einen zweiten Versuch erzwingen.

Erst der native Befund entscheidet über die anschließende Aktivierung und frühe
Registrierung. Station, native Speicherung, Kauf und Geldwäsche bleiben offen.
Der abgenommene Innenraum und die ursprüngliche NPC-Gebäudeidentität bleiben
eigenständige Zuständigkeiten. Kein Release-Paket mit den lokalen Extraktionen.
