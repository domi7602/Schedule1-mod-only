# Changelog

## 0.1.0 (2026-11-10) — Lokaler Abschluss und Eigentümerabnahme

- Dominik hat TownHallInterior nach den Kauf-/Geldwäschearbeiten als soweit fertig
  bezeichnet. Diese Eigentümerabnahme ergänzt die früheren datierten Probe- und
  Offline-Nachweise; sie behauptet keine neue Einzelprüfung ihrer sämtlichen Grenzen.
- Auch die anschließende Lagerkorrektur ist nach der Bestätigung von Cabinet- und
  Hallenfach-Inhalt über Save/Menü-Reload abgeschlossen. [Abnahme und Prüfgrenzen](item-loss-fix-2026-11-10.md#eigentümerabnahme-und-prüfgrenzen).
- Der lokale Abschluss ist keine öffentliche Releasefreigabe. Die hashgebundenen
  nativen Ausschnitte bleiben nicht verteilbar; Paketierung und Veröffentlichung
  bleiben gesperrt. Die dokumentierten Grenzen bei Spielabbruch während eines Saves
  werden durch die Abschlussmeldung nicht als geprüft ausgegeben.

## 0.1.0 (2026-11-10) — Lagerverlust: Korrektur installiert und eigentümerbestätigt

- Hallen-Restore erst mit neuer Weltbindung; ungebundenes Platzieren und ungültige Record-Insertion dürfen kein Inventar verbrauchen.
- Cabinet-Inhalte erhalten optionale Checkpoint-Records unter Schema 2; Hallenlager und Cabinets verwenden vollständige native ItemSet-Payloads statt nur Default-Items. Native Leerslots (`ID=""`, Menge 0) werden berücksichtigt.
- Lager-Capture und dauerhafte Pending-Vorbereitung erfolgen vor dem nativen Save; bei Fehler wird der Aufruf abgewiesen. Unbekannte Cabinet-Records und fehlgeschlagene/teilweise Restores bleiben geschützt, einschließlich Pickup-/Interaktionssperre.
- 1.248 Tests lokal und isoliert und vier Produktionsmethoden-Replaygruppen bestanden; No-Deploy-Release-Build mit 0 Warnungen/0 Fehlern. Replays ersetzen native Grenzen und sind kein Gameplay-Nachweis. Vollständiges isoliertes Pure-Gate nicht bestanden (Timeout und vorhandene Format-/Versionsbefunde).
- In-Game-Verify: **Cabinet- und Hallenfach-Inhalt nach Save/Menü-Reload von Dominik bestätigt; vereinbarter Lagerkorrektur-Test abgeschlossen**, `9238c44e…` installiert. Prozessneustart und individuelle Itemdaten bleiben nicht separat geprüfte Grenzen, kein neuer Auftrag. Nur DLL/PDB ersetzt; vier Ziele und zwei Backups hashgeprüft. Backup `20261110T182612541Z-TownHallInterior-6a560bf1` enthält vorherige `8af979a8…`. Keine Kompensation. [Befund und Nachweise](item-loss-fix-2026-11-10.md), [Installation](evidence/item-loss-20261110/deployment-verification.json).

## 0.1.0 (2026-11-10) — Hallen-Grid auf beiden Böden

- Erdgeschoss und Obergeschoss bekommen ein Grid aus einer vorhandenen Spielkachel. Es hängt nicht an einer Property. Gesetzte Teile stehen im Rathaus-Checkpoint und kommen nur mit dem passenden Spielstand zurück.
- Die Treppenöffnung bleibt ohne Kacheln. Die Kachel-Collider haben die volle Rastergröße, damit die Stellfläche nicht zur Hälfte rot bleibt.
- Der Hover auf diesem Grid ruft die Heatmap mit einer leeren Property auf. Der Null-Aufruf wird übersprungen, sonst bricht die Schnittprüfung ab und der Klick kommt nicht an.
- Belegte Kacheln bleiben rot. Ein gesetztes Teil wird als Occupant eingetragen, damit nichts ineinander gestellt wird. Schränke und Tische blocken über ihre Collider. Dominik hat am 10. November bestätigt, dass ein gesetztes Regal wieder ins Inventar geht. Der Fachinhalt wird beim Speichern mitgeschrieben und in dieselben Slots zurückgelegt.

## 0.1.0 (2026-11-10) — Laufweg frei, Aktenschränke öffenbar

- Das Info-Pult gegenüber dem Eingang und der Papierkorb im Erdgeschoss sind entfernt. Sie standen im Durchgang.
- Die Deko-Aktenschränke werden durch das vorhandene Prefab `FilingCabinet_Built` ersetzt. Es wird kein neues Item und keine eigene Fächerliste erzeugt.
- Ingame-Platzierung und Öffnen wie im Sewer stehen aus.

## 0.1.0 (2026-11-09) — Rathauskauf und Geldwäsche an Spielsave gebunden

- Besitz, Aufträge und offene Buchungsfälle werden dem passenden nativen Save
  zugeordnet. Ein Ladefehler sperrt das Angebot bei Ray. Das alte Schema
  kann nach bestätigter Zuordnung übernommen werden.
- Auszahlungen schließen gezielt den bezahlten Auftrag ab. Bei unklarer
  Buchung bleibt der Auftrag zur Klärung offen. Bestätigte Ablehnungen
  werden auf drei Versuche begrenzt. Die Laufzeit beträgt 1.440 Spielminuten.
- Der Paketlauf lässt die lokale Rathaus-DLL aus. Das explizite Paketieren und
  Veröffentlichen sind gesperrt. Der Formatbefund im TownHall-Test ist behoben.
- Der Release-Build und die Tests für Kauf, Geldwäsche, Save/Reload und
  Paketwahl liefen offline. Zum damaligen Stand vom 9. November stand die
  Ingame-Abnahme noch aus; der spätere lokale Abschluss ist oben festgehalten.
  [Speichervertrag und Übernahme alter Daten](business-persistence-plan.md).

## 0.1.0 (2026-11-06) — Eigene Daten-/Abfrageprobe bestanden, Normalbuild wiederhergestellt

- Dritter Slot-2-Lauf mit Kandidat `b449c1c1…` um 15:49:12 CET: **`observed`,
  keine Findings**. Listener-Snapshot `size=889`, `capacity=1024`, `nodes=889`,
  `maxChildren=0`; Datenvorbereitung und `SetProperty` bestätigt; native
  Abfragen `CurrentLaunderTotal=0`, `AppliedLaunderLimit=0`, `Inside=true`,
  `Outside=false`; Cleanup bestätigt, Root zerstört; Vorher-/Nachher-Snapshots
  identisch (32 Register, 19 fremde VTables).
- 39/39 native Slot-2-Dateien nach regulärem Alt+F4 bytegleich; Normalbuild
  `614b81fd…` / PDB `8126ae65…` um 15:50:44 CET wiederhergestellt.
- Das ist die eigene leere Daten-/Abfrageprobe, nicht die vollständige
  Business-Initialisierung. Offen: Originalinitialisierung, Kauf, echte
  Geldwäsche, Persistenz/Reload.
  [Live-Beleg](evidence/business-data-capacity-live-20261106/observation.json),
  [Savevergleich](evidence/business-data-capacity-live-20261106/save-verification.json),
  [Rollback](evidence/business-data-capacity-live-20261106/rollback-verification.json).

## 0.1.0 (2026-11-06) — Kapazitätsgebundener Kandidat installiert, Slot-2-Test offen

- Kandidat `b449c1c1…` / PDB `943b9176…` um 15:43:53 CET (14:43:53 UTC) über
  den Projekt-Helper installiert. Zwei Dateien ersetzt, `mod.json` bytegleich;
  39/39 native Slot-2-Dateien unverändert. Keine neuen Builds/Tests, kein
  Spielstart, keine Save-Zugriffe.
- Nächster Schritt: TownHallTest / Slot 2 laden und den frischen Bericht prüfen,
  zuerst `Native.DataListenerDiagnostics["minute-list"]` mit `size`, `capacity`,
  `nodes` und `maxChildren`.
  [Installationsbeleg](evidence/business-data-listener-capacity-deployment-20261106/deployment-verification.json).

## 0.1.0 (2026-11-06) — Listener-Bestand 889 belegt, kapazitätsgebundener Nachfolger vorbereitet

- Zweiter Slot-2-Lauf mit Kandidat `4ae6509a…`: `size=889`, `capacity=1024`,
  `version=891`, `countProperty=889`, `countPropertyMatchesSize=true`. Der
  generische Getter las korrekt; der alte 512er-Guard war zu niedrig. Status
  `quarantined / data-minute-list-over-budget:889`, keine Datenabfragen.
- 39/39 native Slot-2-Dateien nach regulärem Alt+F4 bytegleich; Normalbuild
  `614b81fd…` / PDB `8126ae65…` um 15:31:02 CET wiederhergestellt.
- Kapazitätsgebundene Korrektur: Eintragsgrenze folgt `size <= capacity`,
  Kapazität <= 4096; Graphgrenze 16.384 Knoten; `nodes`/`maxChildren` im Bericht.
  Kandidat `b449c1c1…` / PDB `943b9176…` offline geprüft (1.166/1.166 Tests,
  sieben No-Deploy-Builds, neun CS1029-Sperren), **nicht installiert**.
  [Live-Beleg](evidence/business-data-listener-count-live-20261106/observation.json),
  [Korrektur](business-data-probe.md#kapazitätsgebundene-korrektur-offline-nicht-installiert).

## 0.1.0 (2026-11-06) — Korrigierter Datenkandidat installiert, Slot-2-Test offen

- Auf ausdrückliche Freigabe Kandidat `4ae6509a…` / PDB `47592f81…` um
  15:23:34 CET (14:23:34 UTC) über den Projekt-Helper installiert. Zwei Dateien
  ersetzt, `mod.json` bytegleich; vorherige normale DLL/PDB im Helper-Backup.
- 39/39 native Slot-2-Dateien und 181 UserData-Dateien (ohne `.deploy`) gegenüber
  dem Nachtest-Checkpoint unverändert, nur die PDB geändert. 38 Moddateien
  unverändert, nur die TownHall-DLL ersetzt. Spiel geschlossen, keine
  Save-Zugriffe, keine neuen Builds/Tests.
- Nächster Schritt: Spiel über Steam starten, TownHallTest / Slot 2 laden und den
  frischen Bericht prüfen, zuerst `Native.DataListenerDiagnostics["minute-list"]`.
  [Installationsbeleg](evidence/business-data-listener-fix-deployment-20261106/deployment-verification.json),
  [Vertrag und Korrektur](business-data-probe.md#installation-des-korrigierten-kandidaten).

## 0.1.0 (2026-11-06) — Listener-Count korrigiert, Datenkandidat offline neu geprüft

- Ursache des `data-minute-list-over-budget`-Abbruchs eingegrenzt: Der Guard las
  den generischen IL2CPP-`Count`-Getter; `_size`/`_items`/`_version` sind direkt
  lesbare Felder. Der Snapshot liest jetzt diese Felder, protokolliert den Getter
  separat als `DataListenerDiagnostics["minute-list"]` und nennt den tatsächlichen
  Wert im Fehler. Die 512er-Schranke bleibt bestehen.
- 1.166/1.166 TownHall-Tests, sieben No-Deploy-Builds ohne Warnungen/Fehler,
  neun CS1029-Sperren und saubere Formatierung bestanden. Neuer Kandidat
  `4ae6509a…` / PDB `47592f81…` in `.backups/` vorbereitet, **nicht installiert**.
- Normalbuild `614b81fd…` bleibt aktiv; kein Spielstart, keine Save-Zugriffe,
  keine Commits/Pushes. Nächster Schritt: Installation und erneuter Slot-2-Test
  nach Freigabe.
  [Korrektur und Belege](business-data-probe.md#listener-korrektur-offline-nicht-installiert).

## 0.1.0 (2026-11-06) — Datenprobe vor Vorbereitung abgebrochen, Normalbuild wiederhergestellt

- Datenkandidat `2e519ccf…` um 14:13:53 CET installiert und Slot 2 über Steam/CUA
  geladen. Frischer Bericht um 14:18:16 CET: `data-minute-list-over-budget`.
  Erster Listener-Snapshot scheitert vor Registrierung und Datenaufbau;
  tatsächlicher Count fehlt, keine nativen Datenabfragen oder Cleanup-Abnahme.
- Spiel regulär geschlossen; 39/39 native Testdateien bytegleich.
  Normalbuild `614b81fd…` / PDB `8126ae65…` um 14:22:17 CET wiederhergestellt,
  unabhängig geprüft. Keine neuen Builds/Unit-Tests, Commits oder Pushes.
- Nächster Schritt: begründete Listener-Erfassung und Count-Diagnose korrigieren.
  [Ergebnis und Belege](business-data-probe.md#installation-und-gescheiterter-slot-2-datentest).

## 0.1.0 (2026-11-06) — Eigene Business-Daten und native Abfragen offline vorbereitet

- Separater Datenmodus auf der eigenen Business-/NetworkObject-Shell: 15 leere
  Collections (Bounds mit einem eigenen Collider), getrennte Business-/Property-
  Loader und inaktive Container-/Bounds-Kinder. Tatsächliches `SetProperty`,
  beide S1API-Wrapperreferenzen sowie native Summen-, Limit- und Raumabfragen.
- Abfrageabsichten, bekannte Rückgaben, frische Identitäten und vier Listenergruppen
  werden festgehalten. Unbekannte Teilwirkung oder Kontextverlust sperren den
  Versuch; Ressourcenfreigabe verlangt bestätigte native Entfernung.
- 89 neue Tests, 1.166/1.166 TownHall-Tests und sieben No-Deploy-Builds
  ohne Warnungen/Fehler bestanden. Neun gesperrte Buildkombinationen mit CS1029
  bestätigt; unabhängiger Review abgeschlossen. Produktionsbudget bleibt 20 Dateien.
- Kandidat `2e519ccf…` vorbereitet, keine Installation oder native Laufzeitprobe.
  Installierte normale Fassung und Quittung unverändert; keine Save-Dateizugriffe.
  Originalinitialisierung, Registrierung, Kauf, Geldwäsche und Persistenz offen.
  [Vertrag und Nachweise](business-data-probe.md).

## 0.1.0 (2026-11-06) — Business-/NetworkObject-Bindung beobachtet, Normalbuild wiederhergestellt

- Kandidat `beb088f2…` um 13:05:17 CET auf TownHallTest / SaveGame_2 beobachtet:
  gegenseitige Bindung, echte Unity-Frames 1361 → 1362, beide eigenen Callbackketten
  und Rückbau bestätigt. `observed`, keine Findings; unabhängiger Live-Review.
- 28 Register-/Halterfingerprints und 15 fremde VTables unverändert. Nach Spielende
  39/39 native Testdateien bytegleich, keine hinzugefügt/gelöscht; keine vollständige
  Dateisystem-Unverändertheit behauptet.
- Normalbuild `614b81fd…` / PDB `8126ae65…` um 13:09:54 CET über den Helper
  wiederhergestellt und unabhängig geprüft. Keine neuen Builds/Tests.
- Originalinitialisierung, Weltregistrierung, Kauf, Geldwäsche und Persistenz offen.
  [Live-Nachweis](evidence/business-network-subtype-live-20261106/observation.json),
  [Save-Abgleich](evidence/business-network-subtype-live-20261106/save-verification.json),
  [Rollback](evidence/business-network-subtype-live-20261106/rollback-verification.json).

## 0.1.0 (2026-11-06) — Business-/NetworkObject-Testfassung installiert

- Auf ausdrücklichen Installationsauftrag Kandidat `beb088f2…` / PDB `84fa0b0f…`
  um 13:02:12 CET ohne Neubuild über den Helper installiert. Zwei Dateien ersetzt,
  Metadaten identisch; normale Vorgängerfassung und Testslot frisch gesichert.
- 39 native TownHallTest-/Slot-2-Dateien bytegleich; nur TownHall-DLL/PDB geändert.
  Spiel geschlossen, keine Spielsteuerung oder Speicherprobe. Liveprüfung offen.
  [Installationsnachweis](evidence/business-network-subtype-deployment-20261106/deployment-verification.json).

## 0.1.0 (2026-11-06) — Eigene Business-/NetworkObject-Bindung offline vorbereitet

- Neuer isolierter Modus `S1TownHallBusinessNetworkSubtype`: eigene no-base
  NetworkObject-Unterklasse, genau eine eigene Business-Zuordnung und eigener
  Aktivierungs-/Rückbauablauf. Bestehende Runtime-Anschlüsse weiterverwendet.
- Native Manager, bekannte Halter, Verbindungen und leere Client-Zwischenspeicher
  frisch prüfen; Identität, Zähler und Frames binden. Unklare Wirkung sperrt
  Wiederholung und spekulativen Rückbau. Eigene Unity-Namen ersetzen keine direkten
  Aufrufe nichtvirtueller nativer Basismethoden.
- 104 unabhängig geschriebene neue Fälle und 1.077/1.077 TownHall-Tests bestanden.
  Sechs No-Deploy-Varianten mit jeweils null Warnungen/Fehlern; alte Aktivierung
  und beide widersprüchlichen Network-Kombinationen erwartungsgemäß gesperrt.
  Native Reviewkorrekturen betreffen unmittelbare Mutationsgrenzen, echte leere
  Backlinks, zerstörte Unity-Wrapper und Widerruf nach Destroy.
- Allgemeine Diagnoseberichte unterscheiden jetzt aktive Business- und neue
  NetworkObject-Proben von der älteren inaktiven Subtype-Route.
- Keine Installation, kein Save-Zugriff oder Spielstart. Normale Installation
  bleibt `614b81fd…`; Originalinitialisierung, Weltregistrierung, Kauf, Geldwäsche
  und Persistenz bleiben offen. [Vertrag und Belege](business-network-subtype-probe.md).

## 0.1.0 (2026-11-06) — Aktive eigene Shell beobachtet, Normalbuild wiederhergestellt

- Kandidat `c07ada69…` bestand um 11:55:28 CET auf TownHallTest / SaveGame_2
  die begrenzte aktive Untertyp-Probe: `observed`, Findings leer; unabhängiger
  Live-Review bestätigt diesen Umfang.
- Echte Unity-Frames 2243 → 2244; Awake, OnEnable, Start, OnDisable jeweils 1,
  FixedUpdate 10 Aufrufe, OnDestroy 1. Die FixedUpdate-Zahl zählt keine Frames.
  Cleanup und Root-Zerstörung bestätigt, zehn Register und 14 fremde VTables
  unverändert. Kein NetworkObject, keine Weltpublikation und kein Save-Aufruf.
- 39/39 native Slot-2-Dateien nach Spielende bytegleich, keine neuen oder
  fehlenden Dateien; Business-Dateibestand unverändert. Leere Baseline-
  Verzeichnisse waren nicht inventarisiert, keine globale Schreibabwesenheit
  daraus abgeleitet.
- Nach Dominiks „Geschlossen“ Normalbuild `614b81fd…` / PDB `8126ae65…` um
  11:57:33 CET über den Helper wiederhergestellt und unabhängig geprüft;
  Versuch `20261106T105733407Z-22ae4cab2444`. Keine neuen Builds oder Tests.
- Eigene no-base Shell beobachtet; native Business-Initialisierung, echte
  Persistenz und Weltwechsel-/Last-Use-Garantien bleiben offen.
  [Laufzeitbeleg](evidence/business-active-subtype-live-20261106/observation.json),
  [Save-Abgleich](evidence/business-active-subtype-live-20261106/save-verification.json),
  [Rollback](evidence/business-active-subtype-live-20261106/rollback-verification.json).

## 0.1.0 (2026-11-06) — Aktiven eigenen Untertyp installiert, Live-Prüfung offen

- Auf Dominiks Installationsauftrag Kandidat `c07ada69…` / PDB `711863c0…`
  um 11:00:03 CET (10:00:03.1679217 UTC) über `deploy-mod.ps1` installiert;
  kein Neubuild. Versuch `20261106T100002932Z-77c756180c8f` kopierte DLL/PDB,
  identische Metadaten blieben ungeschrieben (`Unchanged` / `NotAttempted`).
- Frischer privater Checkpoint: 39 native Slot-2-Dateien, 172 UserData-Dateien
  und Normal-Rollback `614b81fd…` / PDB `8126ae65…`. Helperbackup
  `20261106T100002974Z-TownHallInterior-905551cc` erhalten.
- Installationsabgleich und unabhängiger Review bestanden. Alle 39 nativen
  Testdateien bytegleich; in 38 Moddateien nur TownHall-DLL, in 172 UserData-
  Dateien nur TownHall-PDB geändert (Deploymentquittungen ausgenommen).
  Spiel vor/nach Installation geschlossen; Slot 1 nicht gelesen.
- 973/973 Tests und fünf No-Deploy-Builds bleiben die vorausgehenden Offline-
  Nachweise; beim Deployment wurden keine neuen Tests oder Builds ausgeführt.
  Aktivierung im Spiel, native Initialisierung und Persistenz bleiben offen.
- Nächster Live-Schritt: Steam-Start, TownHallTest / Slot 2 laden, 20 Sekunden
  warten und „geladen“ melden. Frischen `BusinessActiveSubtypeReport` auswerten;
  nach regulärem Spielende den gesicherten Normalbuild per Helper wiederherstellen.
  [Installation und Grenzen](evidence/business-active-subtype-deployment-20261106/deployment-verification.json),
  [Deploymentquittung](evidence/business-active-subtype-deployment-20261106/deployment-receipt.json).

## 0.1.0 (2026-11-06) — Aktiven eigenen Untertyp vorbereitet, nicht installiert

- Neuer isolierter Buildmodus `S1TownHallBusinessActiveSubtype`: tatsächliche
  Begin/Tick/Leave-Anbindung an den vorhandenen Runtime-Port, eigene Unity-
  Nachrichtenziele geprüft, echte Framefortschritte und begrenzte Wartezeit.
- Gebundene Welt-/Objektidentität, Referenzfreiheit und monotone Readbacks vor
  Mutation; unklare Teilwirkungen erlauben weder erneute Aktivierung noch
  spekulatives Destroy. Berichte nach Widerruf verwenden gespeicherte Framewerte.
- 89 neue Fälle, 973/973 TownHall-Tests, fünf No-Deploy-Buildvarianten ohne
  Warnungen/Fehler und unabhängiger Review bestanden. Legacy-Aktivierung und
  gemischte ActiveSubtype-/Manager-Konfiguration werden beim Build abgewiesen.
- Kandidat `c07ada69…` nicht installiert; Normalbuild `614b81fd…` unverändert.
  Kein NetworkObject, keine Weltregistrierung, kein Speicheraufruf und keine
  native Aktivierungs- oder Business-Abnahme.
  [Vertrag und Belege](business-active-subtype-probe.md).

## 0.1.0 (2026-11-06) — Inaktive No-IO-Speicherprobe bestanden, Normalbuild wiederhergestellt

- Kandidat `38bd7da3…` lief auf `TownHallTest / SaveGame_2` um 09:56:01 CET
  (08:56:01 UTC): `Result.Status` und `Save.Status` sind `observed`, Findings leer;
  Registrierung, einzelner Save-Versuch, Rückkehr, Completion und Freigabe bestätigt.
- Eigener synchroner `ISaveable.Save`-Pfad ohne Dateischreiben: `GetSaveString=1`,
  `WriteData=1`, diagnostischer Getterzähler 8. Eigene Einträge vorher/nachher 0,
  Start/Completion 0/0 → 1/1; Register und fremde VTables unverändert. Eigene
  Objekte zerstört und Cleanup bestätigt; Callbackzähler nach Cleanup unverändert.
- 39/39 native Testdateien gegen den frischen Checkpoint bytegleich, keine neuen
  oder fehlenden Dateien. Leere Baseline-Verzeichnisse waren nicht inventarisiert.
  Anschließend Normalbuild `614b81fd…` / PDB `8126ae65…` wiederhergestellt.
- `NativeSaveWriteAllowed=false` bleibt bestehen. Kein vollständiger
  `SaveManager.Save`, kein Persistenz-/Reload-, Kauf-, Geldwäsche- oder
  Weltwechsel-/Last-Use-Nachweis; Phase 1 bleibt offen. Fortschrittsmarker
  2694 → 2698 belegen keine vier vergangenen Unity-Frames.
  [Laufzeitbeleg](evidence/business-save-live-success-20261106/observation.json),
  [Save-Abgleich](evidence/business-save-live-success-20261106/save-verification.json),
  [Rollback](evidence/business-save-live-success-20261106/rollback-verification.json).

## 0.1.0 (2026-11-06) — Korrigierte Save-Probe installiert, Laufzeitprüfung offen

- Nach Dominiks Rückmeldung „geschlossen“ Kandidat `38bd7da3…` / PDB `109bdd92…`
  um 08:48:53 UTC über den Deploymenthelfer installiert; kein Neubuild.
- Frischer privater Checkpoint: 39 native Slot-2-Dateien, 169 UserData-Dateien und
  bisherige Payload hashgeprüft. Nach Installation native Testdateien unverändert;
  nur TownHall-DLL/PDB ersetzt, Metadaten und andere Mods unverändert.
- Unabhängiger Installations-/Vorgängerbackup-Readback bestanden. Kein Spielstart
  oder neuer Live-Erfolg; Normal-Rollback `614b81fd…` bleibt gesichert vorhanden.
  [Installation und Grenzen](evidence/business-save-fixed-deployment-20261106/deployment-verification.json).

## 0.1.0 (2026-11-06) — Save-Vorprüfung und Cleanup korrigiert, nicht installiert

- Getter-Diagnose bleibt gezählt, wird aber nicht mehr als Save-Start gewertet.
  Teilnehmer und nativer Port verwenden denselben getesteten Callback-Baustein.
- Nach möglicher Registrierungs-/Save-Teilwirkung wird Cleanup bis bestätigtem
  Abschluss und Abmeldung unterlassen. Unlesbare Vorprüfungen und Kontextverlust
  erlauben ebenfalls keine Freigabe; bestehende Referenzhaltung bleibt erhalten.
- 24 neue Regressionen; 884/884 TownHall-Tests bestanden. Gegen den Altcode
  reproduzierten 15 von 18 Ablauffällen den Fehler. Normal/Subtype/Interface/
  Manager ohne Deployment jeweils 0 Warnungen/Fehler; unabhängiger Review akzeptiert.
- Korrigierter Kandidat `38bd7da3…` nur gebaut. Installierte Probe `3ffbc248…`
  bleibt unverändert; kein Spielstart, Savezugriff oder neuer nativer Nachweis.
  [Korrektur, Prüfbelege und Grenzen](business-manager-save-probe.md).

## 0.1.0 (2026-11-06) — Phase 1 Schritte 1–3 & Übergangsplan zur Ingame-Save-Probe

- **Schritt 1 (ISaveable-Inventar):** 23-Slot-Vertragsinventar (`BusinessSaveableContractInventory`),
  Vor-Konstruktions-Armierung (`TryArmParticipantConstruction`), Konstruktor-Callback-Bindung und
  15 gebundene Vtable-Slots auf `ProbeBusinessSubtype` implementiert. 846 Tests grün.
- **Schritt 2 (Non-IO Save-Completion):** 16. virtueller Slot (`ShouldSaveUnderFolder = false`)
  gebunden und überschrieben; schließt Ordneranlage und rekursive Dateilöschung im nativen
  `CompleteSave` (`0x582F00`) deterministisch aus. `IBusinessSavePort` auf `NativeSubtypePort`
  implementiert. 850 Tests grün.
- **Schritt 3 (Trial-Verknüpfung & Frame-Progression):** `BusinessSubtypeTrial.Run` führt No-IO
  Save-Trial aus; atomare CAS-Frame-Progression (`NextFrame()`) verhindert Stillstand bei gleichen
  Unity-Frames. 854/854 Tests bestanden (0 Fehler, 0 übersprungen).
- **Übergangsplan aufgesetzt & Ingame-Test ausgeführt:** [business-save-transition-plan.md](business-save-transition-plan.md)
  definiert die Vorflug-Prüfungen, privaten Checkpoint von `SaveGame_2`, strikten Slot-1-Schutz,
  die exakten Binärhashes für Normalbuild (`614B81FD…`) und Manager-Kandidat (`F11DBF50…`), das
  Ingame-Testprotokoll für Dominik und den sofortigen Rollback-Pfad. Ingame-Lauf (05:48 UTC)
  auf `TownHallTest / SaveGame_2` durchgeführt: kontrollierter fail-closed Abbruch bei `Inspect()`
  (`subtype-readback-invalid`) wegen IL2CPP `METHOD_ATTRIBUTE_FINAL` auf `Property.get_ShouldSaveUnderFolder`;
  keine neuen/gelöschten Save-Dateien, fünf Dateien durch regulären Autosave geändert;
  Normalbuild damals unmittelbar wiederhergestellt. Das spätere Deployment der
  Probe `3ffbc248…` und die folgende Offline-Korrektur stehen oben.

## 0.1.0 (2026-11-05) — Reinen Speicherauftragsbesitzer implementiert

- Ein neuer zusammenhängender Datenblock in `BusinessConstruction.cs`, keine
  Altcode-/Projekt-/Modverdrahtungsänderung. Ownerfassaden teilen einen Slot ihres
  ausdrücklich synthetischen Prozessmodells; kein schon verdrahtetes OS-Singleton.
- Unveränderliche Bindung, geordnete monotone Effekte, Widerruf, begrenzte Callback-
  und Diagnosebuchführung. Späte Return-/Fehlermeldungen getrennt erhalten; erster
  120/121-Fehlerstand archiviert und korrigiert, keine alte Assertion abgeschwächt.
- 128/128 neue Modellfälle, abschließend 809/809 TownHall-Offlinetests. Separater
  Review akzeptiert die Semantik, 33 eigene Probegruppen inklusive 200 Modellrennen
  bestanden. Einzige Pflichtkorrektur Testwhitespace token-/assertionsgleich
  nachgeprüft; V3 nicht erneut unabhängig semantisch geprüft.
- Isoliertes Pure: 20 pass/2 fail (eigene Formatstelle danach korrigiert und
  Formatter bestanden; bekannter fremder Versionsdrift bleibt). Alle 4.740 xUnit-
  Fälle, 265 Konsolenchecks und 14 Toolsuiten bestanden; kein grünes Gesamtgate.
- Keine Runtime-Ports, Unity-/Dateiarbeit im Besitzer oder Installation/Spiel-/
  Saveaktionen. Native Freigaben false, drei Proofs unknown; alte Abnahmen zu.
  [Umfang, Quellenidentität und Nachweise](business-save-operation-owner.md).

## 0.1.0 (2026-11-05) — Speicherauftragsbesitz offline spezifiziert

- [Besitz-/Widerrufsvertrag](business-save-operation-owner.md) mit eingefrorener
  Welt-/Manager-/Instanzbindung, Prozessslot, monotonem Wirkungsprotokoll und
  getrennten Referenz-/Write-/Konsistenzbeweisen entworfen; keine Implementierung.
- Konstruktorcallbacks vor Bindung, teilwirksame Registrierung, fremde/automatische
  Saves, Reentranz und Codeentladung ausdrücklich als Vertragsgrenzen behandelt.
  Quarantäne/Retain allein heilt keinen nativen Lebensdauerfehler.
- Nächster Implementierungsbaustein nur datenbasierter Besitzer mit künstlichen
  Nachrichten, ohne Runtime-Port oder Modverdrahtung. Native Anbindung weiter
  gesperrt; keine Produktions-/Teständerungen, Builds/Testsuiten oder Spielaktionen.

## 0.1.0 (2026-11-05) — Weltwechsel und Speicherabbruch offline eingegrenzt

- 24 zusätzliche native Fenster bytegeprüft. Clean-Delegate am PreSceneChange-
  Event, Identitätsverlust vor Event, zeit-/playerbezogene Exit-Waits und
  persistenter SaveManager-Pfad belegt; kein eigener Last-Use-/Drainvertrag.
- Eigener unabhängiger Lifecycle-Review trennt heutige Sperre von fehlendem
  zukünftigem Altauftragbesitz. Transiente Probe nicht durch bloßes Event-/Frame-
  warten zum asynchronen Schreiber machen; zunächst eigenen Besitz/Lebensdauer
  entwerfen. Keine Implementierung und keine Freigabe durch Retain/Dispose.
- Nur Analyse/Dokumentation; frühere Belege erhalten, keine Produktions-/Test-
  änderungen, Builds, Installation oder Spiel-/Saveaktionen. Basistest bleibt zu.
  [Weltwechselaudit und Beweisgrenzen](native-world-exit-audit.md).

## 0.1.0 (2026-11-05) — Nativer Speicherabschluss offline untersucht

- Passende GameAssembly und Metadaten frisch zugeordnet, 23 gezielte native
  Fenster gelesen und gegen die tatsächlichen Bytes geprüft; keine bloßen
  Proxy-/Dummy-Rümpfe als Implementierung ausgegeben.
- Queueentnahme liegt vor Schreibdispatch, Completion vor Fehlerauswertung.
  Der Restqueue-Zweig leert Completed statt Queue; weitere Referenzhalter und
  Reentranz verhindern eine Freigabe allein aus Event/Idle/Folgeframe.
- Kein eigener Last-Use-/Weltwechselvertrag bewiesen. Schreibsperre bleibt;
  keine Produktions-/Teständerungen, Builds, Installation oder Spiel-/Saveaktionen.
  Frühere Tests und abgeschlossener Start-/Lade-/Handy-Basistest nicht wiederholt.
- [Audit und dauerhafte Belege](native-save-completion-audit.md).

## 0.1.0 (2026-11-05) — Start-/Lade-/Handy-Basistest abgeschlossen

- Dominik bestätigt ausdrücklich problemloses Handy-Öffnen/-Schließen. Zusammen
  mit dem vorherigen Lognachweis ist der Normalbuild-Basistest auf TownHallTest /
  Slot 2 abgeschlossen; kein weiterer Handy-Abnahmeschritt offen.
- Bestätigung separat zugeordnet; der frühere Logbericht bleibt unverändert.
  Keine Einzelapp-, API- oder Business-Save-/Load-Abnahme, Schreibsperre unverändert.
- Nur Dokumentation, kein Build/Deployment oder Spieleingriff.
  [Bestätigung und Grenzen](evidence/launch-phone-owner-confirmation-20261105.json).

## 0.1.0 (2026-11-05) — Normalbuild auf TownHallTest geladen

- Dominik meldet „geladen“. Frischer Prozess seit 07:45:03 UTC; Loader- und
  Dateihashes bestätigen Normalbuild `614b81fd…`, S1API 3.2.2 und passenden Loader.
- Log bestätigt SaveGame_2/TownHallTest, Main, OnLoadComplete und die
  veröffentlichte Halle. Elf PhoneApp-Initialisierungsaufrufe, aber noch keine
  einzeln bestätigte Handybedienung oder neue Sicht-/Gameplay-Abnahme.
- Im erfassten Logpräfix keine ERROR-/Fatal-Meldung oder TownHall-Exception.
  Abgefangene LightControl-Schriftwarnung mit Arial-Fallback getrennt erfasst;
  weitere Warnungen bleiben dokumentiert, keine pauschale Fehlerfreiheitsbehauptung.
- Nur Log-, Prozess- und Artefaktlesen; keine Spielsteuerung, nativen Save-Aufrufe,
  Save-Dateizugriffe, Builds oder Installation. Pure für diesen Dokumentations-
  nachtrag wegen laufendem Spiel nicht erneut ausgeführt.
- [Beobachtung und Nachweisgrenzen](launch-preparation.md),
  [Laufzeitbericht](evidence/launch-live-loaded-20261105.json).

## 0.1.0 (2026-11-05) — Spielstart mit Normalbuild vorbereitet

- Installierte DLL `cff67a85…` unabhängig als alte, verdrahtete Slot-1-/Doppel-
  Save-Probe identifiziert; reguläre Quittung und passender Kandidat gefunden.
- Auf ausdrückliche Eigentümerauswahl durch den geprüften Normalbuild `614b81fd…`
  ohne Business-Proben ersetzt. Kein Neubuild; nur DLL/PDB kopiert, Metadaten,
  Shared und andere Mods unverändert. Installation bleibt ohne neue Ladeabnahme.
- Frischen TownHallTest-/UserData-Checkpoint hashgeprüft angelegt, lokale
  Cloud-Deaktivierung geprüft. Kein Spielstart, kein nativer Save und kein
  Zugriff auf den nativen Hauptslot. Normale Autosaves werden nicht deaktiviert.
- [Vorbereiteter Ablauf und Nachweisgrenzen](launch-preparation.md).

## 0.1.0 (2026-11-05) — Speicherprobe offline abgesichert, native Schreibroute gesperrt

- Manager-Modus nur TownHallTest / SaveGame_2; der Save-Pfad lehnt Slot 1 und
  andere Ziele auch mit passenden expliziten Argumenten ab. Historische
  nichtschreibende Registrierungsnachweise bleiben getrennt erhalten.
- Alte native SaveRegistered-Route einschließlich Sofort-Abmeldung im finally
  und bedingtem zweitem Save entfernt. Ohne geprüften nativen Abschlussvertrag
  wird vor Portzugriff und Registrierung `blocked` gemeldet; keine neue native
  Save- oder globale Sperrhook-Route.
- Reine tickbare Einzelversuchslogik für eingefrorene Identität, bestätigten
  Abschluss und unbekannte Wirkung. Kein Runtime-Adapter bestätigt derzeit den
  benötigten Vertrag; Offline-Tests sind kein Engine-/Persistenznachweis.
- Keine Installation, kein Spielstart, keine nativen Save-Dateioperationen,
  keine Versionsänderung. [Prüfstand und nächste technische Lücke](business-manager-save-probe.md).

## 0.1.0 (2026-11-04) — Interfaceprobe beobachtet, Speicherlisten-Registrierung beobachtet

- Folge-Lauf der ISaveable-Interfaceprobe `853a1178…` am 2026-11-04 um 05:24:22 UTC
  auf TownHallTest / SaveGame_2: `failed` in der Vorprüfung, Befund
  `interface-resolved-flags` (eigene injizierte Methode zeigt auf denselben Pointer
  und dieselbe Klasse, ist aber nicht als virtuell markiert; Slot 65535, Token 0).
  Der Aufruf unterblieb; der Versuch ist verbraucht.
  [Bericht](evidence/business-interface-live-flags-20261104.json).
- Flaggenkorrektur `a8d45f2a…` am 2026-11-04 um 06:58:38 UTC auf TownHallTest /
  SaveGame_2: `observed` — genau ein inaktiver `ISaveable.InitializeSaveable`-Aufruf
  (Zähler 0→1), Property-Zähler 1/1, zehn Register und 15 fremde VTables über alle
  Stufen unverändert, eigener Rückbau bestätigt.
  [Bericht](evidence/business-interface-live-observed-20261104.json). Der Menü-Reload
  um 07:00:49 UTC lieferte `skipped / attempt-already-consumed` ohne neue
  Registrierung. [Bericht](evidence/business-interface-live-reload-20261104.json).
- Inaktiver Speicherlisten-Registrierungsmodus (`473064e0…`,
  `inactive-saveable-registration`) am 2026-11-04 um 07:11:05 UTC auf Slot 1
  (Organisation Dominik / SaveGame_1): `observed`, keine Befunde —
  `SaveManager.RegisterSaveable` genau einmal aufgerufen; Saveables Before 404,
  Added 1, Removed true, Remaining 0; `Save()`/`WriteData` unaufgerufen; zehn
  Register, 14 fremde VTables und native Dateien unverändert;
  `WorldPublicationAllowed` und `NativeSaveWriteAllowed` false; eigener Rückbau
  (CleanupVerified, RootDestroyed) bestätigt. Das ist eine umkehrbare
  Speicherlisten-Registrierung, kein Save-/Load-Nachweis und keine Aktivierung.
  [Bericht](evidence/business-manager-live-register-saveable-20261104.json).
  [Vertrag und Grenzen](business-manager-registration-probe.md).
- Phase 1 (Business-Anbindung nachweisen) bleibt offen;
  [Fertigstellungsplan](business-completion-plan.md). Diese Berichte wurden aus
  `UserData/TownHallInterior` hashidentisch nach `evidence/` übernommen.

## 0.1.0 (2026-11-04) — Klassenroute beobachtet, Interfaceprobe vorbereitet

- Korrekturkandidat `59484a66…` besteht den nativen Untertyp-/Property-Dispatch
  um 04:04:16 UTC und die Einmaligkeitssperre beim Menü-Reload um 04:08:48 UTC.
  Eigener Rückbau bestätigt; zehn Register/14 fremde VTables sowie lokale native
  Dateien beider Slots unverändert. [Datierte Originalberichte](business-subtype-probe.md).
- Separater `S1TownHallBusinessInterface`-Modus: genau ein frischer
  ISaveable-Proxyaufruf des vorhandenen eigenen no-base InitializeSaveable-Overrides
  nach Token-, Interface-, Offset-, Target- und Assignabilityprüfung.
  Init-Zähler ausdrücklich0→1; bisheriger Untertypmodus bleibt bei0.
- 411/411 reine Tests und drei NoDeploy-Builds ohne Warnungen/Fehler; Aktivierung
  bleibt an CS1029 gesperrt. [Vertrag und Abnahmegrenzen](business-interface-probe.md).
- Interface-Live-Lauf `cb98bc95…` um 04:53:58 UTC: `failed` vor dem Aufruf,
  Befund `interface-target-or-counter-invalid`, kein gespeicherter Einzelwert.
  Property-Dispatch und Rückbau bestätigt; keine native Businessinitialisierung.
- Folge-Kandidat `853a1178…` benennt die erste abweichende Gate-Bedingung und
  behält den abgelehnten Readback. Der Interface-Aufruf bleibt gesperrt, bis die
  Prüfung passt. Live-Lauf steht aus.

## 0.1.0 (2026-11-04) — Begrenzte ClassInjector-Typauflösung vorbereitet

- Erster Untertyp-Live-Lauf `659ee6a2…`: Registrierung scheitert bei
  `FishNet.Connection.NetworkConnection, FishNet.Runtime`, keine Instanz;
  zehn Register und 13 fremde VTable-Fingerprints unverändert.
- Temporäre verwaltete Namenskorrektur für genau den validierten NetworkConnection-
  Proxy, begrenzt auf eigenen Typ, Thread und Registrierungstiefe eins. Originale
  Generic-/ByRef-Auflösung bleibt erhalten. Keine neuen nativen Spielmethodenhooks.
- Eigene Hooks werden auch bei Teilfehlern ownergebunden entfernt und gegen
  fremde Patchbestände geprüft. Unbestätigter Rückbau sperrt die Instanzerzeugung.
  Unveränderte Originalauflösung wird vor/nach dem Scope tatsächlich aufgerufen.
- 352/352 reine Tests bestanden, davon 34 neue Regressionen/Fehlerfälle.
  Aktivierung bleibt compile-blockiert; neuer Live-Nachweis offen.
  [Datierter Vertrag und ursprünglicher Fehlbericht](business-subtype-probe.md).

## 0.1.0 (2026-11-03) — Eigene inaktive IL2CPP-Untertyp-Probe vorbereitet

- Separater Schalter `S1TownHallBusinessSubtype`; Aktivierung bleibt compile-blockiert.
  Eigener Business-Untertyp ohne NetworkObject, keine neuen Spielmethoden-Detours,
  kein Aktivieren, Registrieren in Spielregistern, Speichern oder Spawnen.
- Prozessweite Type-Registrierung nach exaktem Slot-/Loadgate, eigene no-base
  Lifecycle-Overrides und native VTable-/Resolver-Prüfung vor gezielten false-
  Antworten auf ShouldSave/CanBePurchased über frische Property-Basiswrapper.
- Zehn Register und fremde VTables bleiben die gemessenen Vergleichsgrenzen;
  echte Komponentenliste und eigener nativer Rückbau werden separat gelesen.
  Unbekannter Rückbau bleibt ein Fehler, Type-Registrierung bleibt prozessweit.
- 318/318 reine Tests bestanden, darunter 45 neue Failure-Injection-Fälle.
  Kein neuer Spieltest oder Gameplay-Abschluss. [Vertrag und Live-Abnahme](business-subtype-probe.md).

## 0.1.0 (2026-11-03) — Aktivierungskandidat nach Startabsturz zurückgezogen

- Kandidat `54d37fe5…` scheitert vor dem Slot-Load: NullReferenceException und
  rekursive `CanBePurchased`-Trampolinaufrufe bis zum Stackoverflow. Aktuelle
  native Adressmetadaten belegen drei Doppelziele der neuen Hooks; der true-Stub
  wird außerdem von fremden und statischen Methoden geteilt.
- Geprüfte inaktive Diagnose `52bb577a…` samt PDB wieder installiert und direkt
  hashgeprüft. 39 native Slot-2- und 38 Slot-1-Dateien gegenüber Checkpoint unverändert.
- Aktivierungs-Buildsymbol durch `#error` gesperrt. Kein neuer Aktivierungskandidat
  und kein Spielstart. Eigene IL2CPP-Untertypen müssen zuerst separat inaktiv auf
  sicheren Dispatch geprüft werden. [Befund und nächster Schritt](business-activation-probe.md).

## 0.1.0 (2026-11-03) — Kontrollierte Aktivierungs- und Registrierungsprobe

- Inaktive Konstruktion auf TownHallTest bestanden: eigene native BusinessData-
  Serialisierung, bestätigter Rückbau und zehn unveränderte Register (14:04:58 CET).
- Neue Diagnose aktiviert eine eigene Business-/NetworkObject-Instanz und trägt
  denselben Business-Pointer genau einmal in vier native Register ein. Getrennte
  Generationen, Folgeframe-/Guardprüfung, vollständiger eigener Rückbau vor Save
  oder Weltwechsel. Unklarer Rückbau sperrt die nächste Generation.
- Native Awake-/Start-/Netzwerkinitialisierung und Zerstörungsrümpfe bleiben für die
  eigene Instanz gesperrt; der Adapter besitzt die Registrierung. Kein Kauf,
  Geldwäsche- oder nativer Persistenznachweis. [Vertrag und Test](business-activation-probe.md).

## 0.1.0 (2026-11-03) — Business-Diagnose und inaktive Konstruktion

- Schema-2-Inventur besteht Erstladen und Menü-Reload auf TownHallTest: getrennte
  Berichte mit je 90 Ereignissen/45 vollständigen Paaren, keine verworfenen Daten;
  normalisierte Inventuren unverändert. Keine Business-Erzeugung daraus abgeleitet.
- Separater Kandidat `52bb577a…`: echte inaktive Business-/NetworkObject-Shell mit
  eigenen Referenzen, begrenzter GetSaveString-Auswertung und geprüftem Rückbau.
  Genau ein Versuch pro Prozess, nur bestätigter Testslot, keine Registrierung,
  Aktivierung oder native Speicherung. [Vertrag](business-construction-probe.md).
- 229 Tests, Kandidat-/Normal-Build jeweils 0 Warnungen/Fehler; unabhängiger
  Quellreview. Zum damaligen Buildzeitpunkt war der native Konstruktionstest offen;
  sein späterer Erfolg ist im neueren Eintrag dokumentiert. 20 Produktionsdateien.

## 2026-10-30 — Innenausbau abgenommen

Dominik hat den Innenausbau insgesamt bestätigt („ja die interior ist fertig“): Eingang,
Erdgeschoss und Obergeschoss. Installierter Stand `7681d319…`, 145/145 Tests.
Offene Erweiterungen: Aushang-Beschriftung, Möbel außerhalb des Vanilla-Satzes,
NPC-Navigation, Deckenwirkung im OG; Keller verworfen.

## 0.1.0 (2026-10-30) — Pulte mit der offenen Seite zum Stuhl

- Nach dem Drehen der Stühle blieb der eigentliche Fehler: Die Stühle standen auf der
  **geschlossenen** Seite des Pultes. Am Asset gemessen: bei Kniehöhe liegen auf lokal +X nur
  10 Vertices (nur an den Enden), auf −X 240 über die ganze Länge → +X ist der Knieraum, −X die
  Blende; yaw bildet lokal +X auf (cos yaw, −sin yaw) ab.
- Bürozeile auf yaw 0 und z 30,6/33,2/35,8, Stühle bei x 57,9 (yaw 180), Besprechung als zwei
  gespiegelte Pulte mit je einem Stuhl auf der offenen Seite. **Empfangstresen im EG ebenfalls
  gedreht** (yaw 180 → 0), damit der Knieraum zum Personal zeigt.
- Neuer Test `EveryChairSitsInAKneeSpaceAndFacesThatDesk` (Arbeitsplatzstuhl vor offener Seite
  und zurückblickend; freistehende Wartestühle ausgenommen). 145/145, installiert `7681d319…`,
  Backup `20261030T140642055Z-TownHallInterior-650a4ec4`.

## 0.1.0 (2026-10-30) — Stühle schauen zum Tisch

- Die Blickrichtung des Stuhl-Assets war nie abgeleitet worden; die OG-Stühle standen 180° verdreht
  und schauten vom Schreibtisch weg. Konvention aus dem abgenommenen Erdgeschoss hergeleitet
  (Empfangsstuhl yaw 180 → zum Tisch, Formularstuhl yaw 270 → zum Tisch): **0 = +X, 90 = −Z,
  180 = −X, 270 = +Z**. Bürostühle 270 → 180, Besprechungsstühle 90/270 → 0/180, Südstühle 0 → 270.
- Neuer Test `EveryChairFacesTheDeskItBelongsTo`: für jeden Stuhl muss das Skalarprodukt aus
  Blickrichtung und Richtung zum nächstgelegenen Tisch desselben Geschosses > 0,35 sein.
  145/145, installiert `a341a42b…`, Backup `20261030T135521813Z-TownHallInterior-3198f58c`.

## 0.1.0 (2026-10-30) — Obergeschoss nachgelegt

- Zwei Teppiche (`upper-rug-south`, `upper-rug-meeting`), vier Aktenschränke und zwei Stühle an
  der Südwand, Papierkorb, zwei große Eckpflanzen sowie zwei Monitore auf dem Besprechungstisch.
  Gesamt 57 Platzierungen, 2 Teppiche.
- Tests prüfen jetzt beide Geschosse (Hüllgrenzen, Standhöhe, Desktop-Objekte, Treppenloch und
  Portal frei): 144/144, installiert `b826848d…`, Backup `20261030T062905165Z-TownHallInterior-6d2ea472`.

## 0.1.0 (2026-10-30) — Keller verworfen, Obergeschoss eingerichtet

- **Keller zurückgebaut.** Der Test am Morgen zeigte die Gelände-Mesh im Treppenschacht
  (RGB ≈ 5/34/32): Hallenboden 1,53 entspricht Blender 0,65, also Versatz ≈ +0,88 → Gelände bei
  y ≈ 0,69, nur 84 cm unter dem Boden. Eine offene Treppe kann diese Fläche nicht umgehen;
  Verdecken würde die Treppe blockieren, Ausschneiden hieße ein geteiltes Native-Mesh ändern.
  `BasementPlan`/`BasementPlanTests` entfernt, Boden wieder eine Platte, Kellerlichter und
  Keller-Einrichtung raus. Umfang laut Dominik: Erdgeschoss und Obergeschoss.
- **Obergeschoss eingerichtet** (48 Platzierungen): drei Büroarbeitsplätze (Schreibtisch, Stuhl,
  Monitor, Tischpflanze) an der Westwand, Besprechungstisch aus zwei Schreibtischen mit vier
  Stühlen, fünf Aktenschränke als Archiv an der Ostwand nördlich des Treppenlochs, zwei große
  Eckpflanzen, Papierkorb und Wanduhr. Einbauten des EG unverändert.
- **Tests:** 144 Fälle bestehen (Standhöhe EG/OG, Desktop-Objekte auf Tischen, keine
  Überlappungen, Portal und Treppenloch frei). Installiert `abaa9c2f…`, Backup
  `20261030T044742763Z-TownHallInterior-325e2698`. Sichtprüfung des OG offen.

## 0.1.0 (2026-10-29) — Keller unter dem Erdgeschoss

- **Machbarkeit:** Sonden in der privaten Blender-Map (read-only) zeigen unter der Südhälfte
  des Rathauses freien Raum zwischen ≈ −0,28 und −5,14 (Blender-Z); die Sewer-Flächen liegen
  bei −5,14/−5,54/−6,48 bzw. −16,2. Die Gelände-Mesh liegt über dem Keller und schneidet ihn nicht.
- **Keller** (`BasementPlan.cs`): x 55,0–62,2 · z 22,0–34,0, Boden −2,90, Deckenunterkante −0,55
  (2,35 m licht), Wände 0,20 m. Gerade Treppe mit 15 Tritten an der Westwand, Öffnung im
  EG-Boden x 55,0–56,35 · z 23,0–27,5; der Boden ist dafür in `public-floor-south/north/east`
  geteilt. Schachtwände gegen Einblick, vier Kellerlichter, sechs Aktenschränke, zwei Körbe.
- **Tests:** 157 Fälle, elf neue in `BasementPlanTests` (Sewer-Abstand, lichte Höhe, Stufenmaße,
  freies Treppenloch, Dichtheit, Schachtwände, Portal/Treppenloch frei) plus Läufer-Freigang.
  Installiert `211f112a…`, Backup `20261029T204537778Z-TownHallInterior-1f232624`.
- **Offen:** Sichtabnahme, Gelände-Kollision beim Abstieg, NPC-Navigation, Sewer-Anbindung.

## 0.1.0 (2026-10-29) — Erdgeschoss-Ausbau, Runde 1

- **Neue Einbauten** (`HallFittings.cs`, prozedural, keine neuen Vanilla-Assets):
  Info-Tresen gegenüber dem Eingang (x 57,2–58,1 · z 29,6–32,4, 2,50 m + 7 cm Platte,
  Sichtblende bis 3,05 m), drei Sitzbänke an der Südwand (je 2 m, Sitzhöhe 0,43 m),
  bordeauxfarbener Läufer ohne Kollision, Aushangkasten rechts neben der Tür, zwei auf
  1,8-fach skalierte Eckpflanzen.
- **Wartebereich umgestellt:** die acht losen Stühle entfallen, die Bänke ersetzen sie
  (Dominik-Entscheidung B). Vier Stühle bleiben an Empfang und Formularen.
- **Technik:** `FurniturePlacement` hat ein `Scale`-Feld (Platzierung skaliert jetzt den
  Wurzelknoten); die gemeinsamen Paints umfassen fünf Farben (dunkles Holz, Bordeaux);
  `DecorativeBox` behält den Collider für solide Einbauten und entfernt ihn nur bei Hüllflächen.
- **Tests:** 144 Fälle, davon sieben neue in `HallFittingsTests` (Passung, freies Portal,
  freies Treppenloch, Kollisionsflags, Laufwege am Tresen, Bankhöhe). Installiert
  `73cf3bdf…`, Backup `20261029T202623707Z-TownHallInterior-395a8f33`. Sichtprüfung offen.

## 0.1.0 (2026-10-29) — Nachtrag 5: Natives Klopfen bleibt erhalten

- **Befund (live gelesen):** An Jens `StaticDoor` stand `CanKnock = false` (von der Mod) und
  `Usable = true` (Fix aus Nachtrag 3). Dominik erklärt die Spielmechanik: NPCs despawnen,
  sobald sie ein Gebäude betreten, und ein Klopfen holt sie zurück. Unsere Knock-Unterdrückung
  hat damit den einzigen Weg zur Schalterin entfernt.
- **Änderung:** `SuppressPlayerKnock` ersetzt durch `HideNativeDoorSurface`. Die Mod versteckt
  nur noch die Native-Tür-Renderer; `CanKnock`, `Usable` und das `IntObj` bleiben unverändert,
  es wird nichts mehr restauriert, weil nichts mehr verändert wird.
- **Tests:** 138 Fälle; der Quellwächter verbietet jetzt sowohl `.Usable =` als auch
  `.CanKnock =` im Host. Installiert `6c39bd36…`, Backup `20261029T195551234Z-TownHallInterior-337a010c`.
- **Folge:** Am Rathaus erscheint wieder der native „E Knock"-Prompt. Die Halle bleibt frei
  begehbar; beides läuft nebeneinander.

## 0.1.0 (2026-10-29) — Nachtrag 4: Schiebetür blockiert keine NPCs mehr

- **Zweite Ursache für Jens Problem:** Der Näherungssensor liest nur `Player.Local`. Für NPCs
  bleiben die Flügel geschlossen, und das Panel hatte einen Collider – Jen stand vor einer
  zugezogenen Tür.
- **Änderung:** Kein Blattteil ist mehr solid; die Flügel sind reine Ersatzgrafik für die
  versteckte Native-Tür. Der Test `LeafPartFacesNeverShareAPlane…` prüft jetzt zusätzlich, dass
  kein Teil kollidiert.
- **Tests:** 138 Fälle bestehen. Installiert `6393fddf…`, Backup `20261029T194158847Z-TownHallInterior-50a27802`.
- **Offen:** Eine sichtbare Öffnung für NPCs (Sensor auf Nicht-Spieler erweitern) ist bewusst
  nicht enthalten; erst die Live-Prüfung soll zeigen, ob Jen ohne sie hineinkommt.

## 0.1.0 (2026-10-29) — Nachtrag 3: Türbenutzbarkeit für NPCs erhalten

- **Befund:** In der Sitzung um 20:29 warf `NPCEvent_StayInBuilding.OnActiveTick` 50-mal in
  zwölf Sekunden eine `NullReferenceException`. Die Mod setzte an Jens `StaticDoor` bisher
  `CanKnock=false` **und** `Usable=false`; `Usable` steuert, ob die Tür überhaupt funktioniert,
  und Jen wohnt in diesem Gebäude.
- **Änderung:** `Usable` wird nicht mehr angefasst und nicht mehr restauriert. Unterdrückt
  bleiben nur der Spieler-Knock (`CanKnock`) und der Interaktions-Prompt (`IntObj`), Renderer
  der Native-Tür bleiben versteckt.
- **Tests:** 138 Fälle, neu ein Quellwächter, der eine erneute `Usable`-Zuweisung im Host
  verhindert. Installiert `0ed42b5c…`, Backup `20261029T193738624Z-TownHallInterior-6bc522de`.
- **Grenze:** Der Zusammenhang ist aus Log und Code abgeleitet, nicht live bestätigt. Die
  nächste Sitzung muss zeigen, ob die Meldung ausbleibt und Jen wieder ins Gebäude kommt.

## 0.1.0 (2026-10-29) — Nachtrag 2: Restspalt über der Tür und flackernder Rahmen

- **Kopf-Laibung:** `inner-lintel` endet jetzt bei `HeadLiningBottom` (4,54) und beginnt bei
  `LiningFrontX` (54,16), überlappt also die Blattoberkante und -ebene um je 1 cm. Das schließt
  das letzte 1-cm-Fenster, durch das man beim Aufblicken Himmel sah.
- **Z-Fighting:** Die Blattteile liegen als testbare Daten in `PublicEntrance.LeafParts`.
  Rahmenprofile sind 1 mm dünner als die Stile, Stile und Panel enden unter der Rahmenoberkante,
  keine zwei sichtbaren Flächen teilen sich eine Ebene (Ursache des Flackerns).
- **Tests:** 137 Fälle bestehen (neu: Ebenen-Regression für die Blattteile). Installiert
  `3a82f9e8…`, Backup `20261029T173935879Z-TownHallInterior-fc354400`.
- **Abnahme:** Dominik hat den Eingang nach diesem Deploy optisch abgenommen („tür sitzt jetzt
  perfekt“). Gilt für den Sichtfit des Eingangs, nicht für Decke, obere Hülle, Treppe, Möbel,
  LOD, NPC-Navigation oder Multiplayer.

## 0.1.0 (2026-10-29) — Nachtrag: Laibungstiefe und Innenzarge

- **Sealed entrance:** Die Flügel sind auf die sichtbare Laibung bemessen (je 1,00 m, Abdeckung
  30,00–32,00, Berührung bei 31,00) und tragen ein Rahmenprofil; eine render-only Innenzarge
  fasst den Durchgang.
- **Deep reveal:** `inner-lintel` und `reveal-south`/`reveal-north` reichen bis zur
  Fassadenebene (x 54,18), weil der native Fassadenschnitt dreiecksbedingt über das geplante
  Rechteck hinausgeht und sonst ein Aufwärtsblick über die Blattoberkante die Außenwelt zeigt.
- **Tests:** 136 Fälle bestehen; installiert `28efce74…`, Backup
  `20261029T173104129Z-TownHallInterior-af82f3e5`. Ingame-Sichtprüfung offen.

## 0.1.0 (2026-10-29) — Local experimental public hall

Initial version of the local staging build. The Town Hall facade opens into a walkable
public reception hall: geometric portal, proximity-driven sliding entrance, vanilla
reception furniture and eight local lights. No furniture interactions, no upper-floor
furniture and no business. The embedded native slices are local, hash-pinned extractions
of the f12 scene and are **not distributable**.

- **In-Game-Verify open:** the geometric public hall and the sliding entrance are
  implemented and game-free tested, but the in-game visual acceptance (outside/inside,
  door open/closed, day/night, entering and leaving without jitter, sealed leaf/reveal
  joint) is still open and lies with Dominik. Candidate `5efb8650…` did load on
  2026-10-29 16:57 and published the hall and the reception (26 instances, 8 lights) per
  the MelonLoader log; that is a load signal, not a visual acceptance. The current
  candidate `7ffcf24b…` (sealed sliding leaves plus inner casing) was installed at
  17:53 local with the game closed and has not been launched yet.
- **Repository integration:** the mod and its test project are now part of
  `Source/Mods/S1Mods.sln`, regenerated with `Tools/gen-sln.ps1`. Before that they were
  invisible to the solution build, to the repository format gate and to CI. `RoomPlan.cs`
  and `RoomPlanTests.cs` are whitespace-normalized (space after comma, space around
  binary operators); no coordinate, box, name or collision behaviour changed.
- **Door dialog archived:** the staged, never-wired door-choice subsystem
  (`DoorSelector`, `DoorModalHost`, `ExclusiveModalFocus`, `NativeContextMenuRuntime`)
  and its four test files moved to `Source/Archive/TownHallInterior/DoorDialog/`. It had
  no production instantiation: `DoorHook` hard-wires `UnavailableDoorSelector` and always
  passes the native interaction. Runtime behaviour is unchanged.
- **Tests:** 128 game-free test cases remain in the active suite; 169 before the archive
  move. No-Deploy Release build of the solution: 0 errors, 0 warnings.
  `Tools/gen-sln.ps1 -Check` and `dotnet format --verify-no-changes` pass.
- **Release path:** `Tools/package-release.ps1` selects this mod by directory, not through
  the solution, and would package the embedded native slices. No release archive may be
  produced from this mod.
