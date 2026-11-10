# TownHall Business Fertigstellungsplan

Stand: 2026-11-06. Status: Business-Integration in diagnostischen Teilschritten.

**Aktueller Stand, 2026-11-06, 15:50 CET:** Die kapazitätsgebundene Datenprobe
bestand den dritten Slot-2-Lauf: **`observed`, keine Findings**. Der
Listener-Snapshot zeigt `size=889`, `capacity=1024`, `nodes=889`,
`maxChildren=0`; Datenvorbereitung und `SetProperty` sind bestätigt, die nativen
Abfragen liefern **0/0/true/false**. Cleanup bestätigt, Root zerstört;
Vorher-/Nachher-Snapshots identisch (32 Register, 19 fremde VTables).
39/39 native Slot-2-Dateien nach regulärem Alt+F4 bytegleich; Normalbuild
`614b81fd…` / PDB `8126ae65…` um **15:50:44 CET** wiederhergestellt und geprüft.
Das ist der technische Erfolg der eigenen leeren Daten-/Abfrageprobe, nicht die
vollständige Business-Initialisierung. Offen bleiben Originalinitialisierung,
Kauf, echte Geldwäsche und Persistenz/Reload.
[Live-Beleg](evidence/business-data-capacity-live-20261106/observation.json),
[Savevergleich](evidence/business-data-capacity-live-20261106/save-verification.json),
[Rollback](evidence/business-data-capacity-live-20261106/rollback-verification.json).

Der gescheiterte Lauf bleibt historisch dokumentiert: 39/39 native
Slot-2-Dateien nach regulärem Spielende bytegleich; Normalbuild seit 14:22:17 CET
wiederhergestellt und unabhängig geprüft. Der vorherige Offline-Schritt bestand
1.166/1.166 TownHall-Tests, sieben No-Deploy-Builds und unabhängigen
Quellenreview. Die Listener-Korrektur wurde erneut mit diesem Umfang geprüft.

**Vorangegangener Live-Stand:** Eigene no-base NetworkObject-Unterklasse und
Business-Bindung implementiert, mit frischen Halter-/Identitätsprüfungen und
bestätigungsabhängigem Rückbau. 104 neue Fälle, 1.077/1.077 TownHall-Tests bestanden.
Kandidat `beb088f2…` um 13:05:17 CET in TownHallTest / Slot 2 erfolgreich im
begrenzten Bindungs-/Lifecycle-Umfang beobachtet. 28 Fingerprints/15 fremde VTables
unverändert, beide Callbackketten/Rückbau bestätigt, 39 native Testdateien bytegleich.
Normalbuild seit 13:09:54 CET wiederhergestellt. Dieser Lauf prüfte noch keine
eigenen Daten oder nativen Business-Abfragen; Originalinitialisierung offen.
[Bindungsbaustein und native Initialisierungsbefunde](business-network-subtype-probe.md).

**Vorangegangene technische Teilabnahme, 11:55:28 CET:** Kandidat `c07ada69…` bestand
auf TownHallTest / SaveGame_2 die begrenzte aktive Untertyp-Probe mit `observed`
und leeren Findings. Echte Unity-Frames 2243 → 2244, eigene Lifecycle-Callbacks
und Rückbau bestätigt; zehn Register und 14 fremde VTables unverändert.
Kein NetworkObject, keine Weltpublikation und kein Save-Aufruf. Nach Spielende
39/39 native Testdateien bytegleich, keine Dateien hinzugefügt oder gelöscht;
leere Baseline-Verzeichnisse waren nicht inventarisiert. Um **11:57:33 CET
wurde Normalbuild `614b81fd…` / PDB `8126ae65…` wiederhergestellt**, unabhängig geprüft.
973/973 Tests, fünf No-Deploy-Builds und Code-/Testreview bleiben die vorherigen
Offline-Nachweise; für den Liveabschluss wurden keine neuen Tests oder Builds
ausgeführt. [Aktiver Untertyp: Vertrag und Grenzen](business-active-subtype-probe.md),
[Laufzeitbeleg](evidence/business-active-subtype-live-20261106/observation.json),
[Save-Abgleich](evidence/business-active-subtype-live-20261106/save-verification.json),
[Rollback](evidence/business-active-subtype-live-20261106/rollback-verification.json).

**Nächster offener Schritt in Phase 1:** Die eigene leere Daten-/Abfrageprobe ist
bestanden; Normalbuild 614b81fd… ist wiederhergestellt. Der technische Entwurf
für die anschließende Welt- und Listenanbindung ist im [Plan zur Weltregistrierung](business-world-registration-plan.md)
ausgearbeitet. Als Nächstes folgen
getrennt: Originalinitialisierung, rechtzeitige Weltregistrierung, Kauf, echte
Geldwäsche und Persistenz/Reload. Die Diagnosegrenzen bleiben bestehen; keine
unbelegte Erhöhung oder Umgehung.
Originale Business-/FishNet-Initialisierung, rechtzeitige Weltregistrierung vor
abhängiger Wiederherstellung und stabiler nativer Save-/Load-Weg bleiben offen.
Eigene no-base Lifecycle-Methoden ersetzen diese Verträge nicht; Kauf und
Geldwäsche folgen erst auf die erfolgreiche Business-Anbindung.

**Vorherige technische Teilabnahme:** Kandidat `38bd7da3…` bestand am 6. November
um 09:56:01 CET (08:56:01 UTC) auf `TownHallTest / SaveGame_2` genau einen eigenen
synchronen `ISaveable.Save`-Aufruf ohne Dateischreiben einschließlich Completion,
Entfernung eigener Listenreferenzen und bestätigtem Rückbau. Register und fremde
VTables unverändert, 39/39 native Testdateien bytegleich. Anschließend wurde
Normalbuild `614b81fd…` / PDB `8126ae65…` wiederhergestellt.
[Laufzeitbeleg](evidence/business-save-live-success-20261106/observation.json),
[Save-Abgleich](evidence/business-save-live-success-20261106/save-verification.json),
[Rollback](evidence/business-save-live-success-20261106/rollback-verification.json).

Der vorausgehende Offline-Stand bestand 884/884 Tests, vier No-Deploy-Builds und
unabhängigen Review. `NativeSaveWriteAllowed=false` bleibt bestehen. Der begrenzte
Lauf ersetzt weder einen vollständigen `SaveManager.Save` noch Persistenz/Reload,
Kauf, Geldwäsche oder einen Weltwechsel-/Last-Use-Vertrag. **Phase 1 bleibt offen.**
[Aktueller Vertrag, historische Schritte und Prüfgrenzen](business-manager-save-probe.md).

Die folgende Chronologie bewahrt den jeweiligen damaligen Nachweisstand;
damals ungerufene Save-Methoden sind keine Aussage über den jüngsten Lauf.

Das vorhandene Rathaus erhält ein kaufbares Business mit nativer Geldwäsche und
zuverlässiger Speicherung. Eingang, Erdgeschoss und Obergeschoss bleiben die
abgenommene Grundlage. Zuerst entsteht ein vollständig spielbarer Business-Kern;
Verwaltungsaufträge und größere Ausbaustufen folgen als eigenständige Erweiterungen.

Die Umsetzung wurde am 2026-11-03 mit einer [beobachtenden Anbindungsprobe](business-binding-probe.md)
begonnen. Sie klärt die noch offenen Laufzeitverträge. Die erste Diagnose wurde installiert und der erfasste native
Bestand auf TownHallTest (Slot 2) über einen Prozessneustart verglichen. Die korrigierte
Schema-2-Aufzeichnung besteht nun Erstladen und Menü-Reload mit je 90 Ereignissen,
45 vollständigen Aufrufpaaren und unveränderten Inventuren. Als nächster konkreter
Adapterbaustein hat die [inaktive native Konstruktionsprobe](business-construction-probe.md)
Erzeugung, Referenzen, Serialisierung und Rückbau im Testslot bestanden.
Die anschließende [Aktivierungs-/Registrierungsprobe](business-activation-probe.md)
`54d37fe5…` ist nach einem Startabsturz vor dem Slot-Load zurückgezogen: gemeinsame
native Codeadressen machen die neuen globalen Instanzguards ungeeignet. Zunächst
wurde `52bb577a…` wiederhergestellt. Vor dem nächsten Aktivierungsversuch müssen ein eigener
inaktiver IL2CPP-Untertyp, sein nativer Dispatch und der getrennte NetworkObject-
Lifecycle nachgewiesen werden. Die [separate inaktive Untertyp-Probe](business-subtype-probe.md)
enthält ausdrücklich keinen NetworkObject. Ihr erster Live-Lauf scheiterte am
2026-11-04 an der FishNet-Assemblyauflösung, noch vor einer Instanz. Dafür ist nun
eine eng begrenzte verwaltete Namenskorrektur implementiert. Mit `59484a66…`
bestanden am 2026-11-04 der inaktive Untertyp-/Property-Dispatch und der
anschließende Menü-Reload mit Einmaligkeitssperre. Die neue
[inaktive ISaveable-Interfaceprobe](business-interface-probe.md) bestand mit
`a8d45f2a…` am 2026-11-04 um 06:58:38 UTC genau einen eigenen no-base
`InitializeSaveable`-Aufruf. Der Menü-Reload um 07:00:49 UTC bestätigte die
Einmalversuchsperre. Das ist kein SaveManager-Nachweis: die zehn Register blieben
unverändert, und der eigene Override ruft die originale Initialisierung nicht auf.
Auf dem freigegebenen Slot 1 „Dominik“ hat `473064e0…` am 2026-11-04 um
07:11:05 UTC `RegisterSaveable` genau einmal aufgerufen und den Eintrag sofort
aus der Speicherliste entfernt. `Save()` wurde nicht aufgerufen. Die 38
Slot-1-Dateien und TownHallTest blieben gegenüber der vorherigen Sicherung
unverändert. [Bericht](evidence/business-manager-live-register-saveable-20261104.json).
[Vertrag und Herkunft](business-manager-registration-probe.md). Das ist eine umkehrbare Speicherlisten-Registrierung, kein
Save-/Load-Nachweis. `Save`, `WriteData`, `GetSaveString` und die originale
Manager-Initialisierung bleiben ungerufen. Phase 1 ist nicht erfüllt.

Am 2026-11-05 wurde die vorbereitete Save-Probe offline abgesichert: Ziel nur
TownHallTest / SaveGame_2, alte Sofort-Abmeldung und Doppel-Save entfernt.
Der native Schreibpfad bleibt vor Registrierung gesperrt, bis ein belastbarer
Abschluss-/Freigabevertrag vorhanden ist. Eine reine Zustandsmaschine behandelt
Einzelversuch, verzögerten Abschluss und unbekannte Wirkung; sie ist kein nativer
Abschlussbeleg. [Arbeitsstand und Offline-Prüfung](business-manager-save-probe.md).

Der nachfolgende [Native-Audit vom 5. November](native-save-completion-audit.md)
belegt echte Maschinencode-Teilfolgen: Queueentnahme vor Schreibdispatch,
Completion vor Fehlerauswertung und mögliche verbleibende Completed-/Request-
Referenzen. Ein sicherer eigener Last-Use-/Weltwechselvertrag ist weiterhin
nicht bewiesen. Kein Speicheradapter, Build, Deployment oder Live-Save in diesem Audit.

## Ausgangspunkt

- **Abgeschlossen:** Eingang und Innenausbau von EG und OG, von Dominik am
  2026-10-30 abgenommen. Die alte Innenraumabnahme wird nicht erneut geöffnet.
- **Verworfen:** Keller wegen des Geländes unter dem Gebäude. Er gehört nicht zum
  Fertigstellungsumfang; Buchhaltung und Business-Bedienung liegen im OG.
- **Neu:** Business-Registrierung, Kauf, Besitz, Geldwäsche, nutzbare Bedienung
  und deren Save-/Reload-Verhalten. Dekorative Möbel sind noch keine Business-Stationen.
- **Rahmen:** IL2CPP und Singleplayer. Bestehende Autoritäts-/MP-Abweisungen bleiben
  erhalten. Keine Multiplayer-Abnahme und kein neuer Spielzweig.
- **Veröffentlichung:** Die vorhandenen lokalen Fassaden-/Möbelextraktionen sind
  nicht verteilbar. Ein lokal fertiger Mod ist deshalb noch kein veröffentlichbares Paket.

Quellen: [aktuelle Innenraumabnahme und Veröffentlichungspfad](README.md),
[Einrichtungsnachweis](furniture-verification.md) und
[Projektregeln](../../../../AGENTS.md).

## Ziel der ersten fertigen Fassung

Der Spieler kann das Rathaus besuchen, bei Ray als Business kaufen und anschließend
an einem Verwaltungscomputer im Obergeschoss die native Geldwäsche bedienen.
Das Business erscheint mit eigenem Namen und stabiler Identität in den passenden
nativen Übersichten. Besitz, laufende Vorgänge und Ergebnisse überstehen Speichern,
Neuladen und einen Spielneustart ohne Verlust oder doppelte Auszahlung.

Der bereits freie Zugang zu EG und OG bleibt bestehen. Besitz schaltet die
Geschäftsfunktionen frei. Neue Zugangssperren, verschlossene Büroräume oder geänderte
NPC-Wohn-/Arbeitsroutinen sind keine stillschweigende Folge des Kaufs.

### Arbeitsannahmen für die Planung

| Punkt | Vorgeschlagener Startwert oder Ansatz | Vor Umsetzung festzulegen |
|---|---|---|
| Name | Hyland Town Hall | Anzeige- und unveränderliche technische Identität getrennt festlegen. |
| Kauf | Einmaliger Business-Kauf bei Ray | Tatsächlichen nativen Zahlungsweg und Geldquelle prüfen; Besitz setzen allein ist kein Kauf. |
| Kaufpreis | 150.000 $ | Früherer Ideenwert, keine bestätigte Balanceentscheidung; konfigurierbar vorsehen. |
| Geldwäsche | 20.000 $ Kapazität | Früherer Ideenwert; tatsächliche native Periode, Gebühr und Ablauf zuerst feststellen. |
| Bedienung | Ein Verwaltungscomputer im OG | Geeignete native Station samt Interaktion und Speicherung ermitteln. |
| Freischaltung | Kauf als einzige zusätzliche Zugangsvoraussetzung zum Business-Kern | Keine neue Quest oder Rangbedingung für die erste Fassung. |
| Zusatzeinnahmen | Keine zweite passive Auszahlung durch TownHall | Bei aktivem BusinessIncome dessen Erfassung und Balancing getrennt prüfen. |

Die Zahlen stammen aus dem früheren [Ideenplan im Chat Townhall Property Oder Business](https://chatgpt.com/c/6ae23c2c-a120-83eb-9ffa-2a7831e482f5).
Sie sind Planungsvorschläge. Technische Machbarkeit wird unabhängig von diesen
Balancewerten untersucht; für einen späteren Testbuild werden die verwendeten Werte
ausdrücklich dokumentiert.

## Architektur und Entscheidung über MapExpansions

Der erste Umsetzungsschritt bleibt im vorhandenen TownHallInterior-Mod. Innenraum,
Business-Regeln, native Anbindung, Bedienung und Speicherung erhalten klar getrennte
Zuständigkeiten. Erst wenn eine konkrete gemeinsame Funktion gebraucht wird, gehört
sie nach Shared. Es gibt keine direkte Referenz auf unseren BusinessIncome-Mod.
Siehe [Abhängigkeitsgrenzen](../../../../docs/architecture.md).

**MapExpansions bleibt bis zum technischen Nachweis optional.** Seine
[`PropertyDefinition`](https://github.com/DooDesch-Mods/ScheduleOne-MapExpansions/blob/db8c73a3a1b15d1c2c0e6c09f325bcaed34bd9fd/Api/PlaceDefinitions.cs#L76-L124)
kopiert eine native Property oder ein Business einschließlich Gebäude und weiterer
Bestandteile. Ein direkter Anschluss unseres bestehenden Rathaus-Innenraums ist
dadurch noch nicht bewiesen. Seine
[`PropertyExtensionDefinition`](https://github.com/DooDesch-Mods/ScheduleOne-MapExpansions/blob/db8c73a3a1b15d1c2c0e6c09f325bcaed34bd9fd/Api/PropertyExtensionDefinition.cs#L5-L29)
setzt bereits eine registrierte Property voraus.

Für die Business-Anbindung werden zwei Wege begrenzt verglichen:

1. MapExpansions nur dann verwenden, wenn die benötigte native Business-Struktur
   vollständig und ohne zweites Gebäude, fremde Komponentenreste oder konkurrierende
   Restore-Pfade an unser Rathaus angebunden werden kann.
2. Andernfalls einen schmalen eigenen Adapter auf die tatsächlich geprüften nativen
   Business-Verträge aufbauen. S1API-Wrapper allein sind kein nachgewiesener Erzeugungsweg.

Beide Wege müssen dieselben Abnahmekriterien erfüllen. Es werden nicht zwei komplette
Implementierungen gebaut. Ergibt keiner einen belastbaren Weg, wird die konkrete
technische Lücke dokumentiert; ein eigenes Einnahmenmenü ersetzt nicht stillschweigend
das geplante native Business.

Based dient als Entwurfsbeispiel für spätere Bauaufträge, nicht als notwendige
Abhängigkeit der ersten Fassung. MapExpansions 1.0.1 und Based 1.1.0 wurden für die
Recherche gelesen, nicht für TownHall installiert oder laufzeitgeprüft. API-Nutzung
und eigene Implementierung beachten die jeweiligen Lizenztexte; fremde interne
Implementierungen oder Assets werden nicht übernommen.

## Reihenfolge und Abschlusskriterien

| Phase | Arbeit | Ergebnis, bevor die nächste Phase beginnt |
|---|---|---|
| 0. Ausgangslage festhalten | Installierte Spiel-/API-Versionen und TownHall-Artefakte identifizieren; aktive lokale Änderungen erhalten. Native Business-, Kauf-, Station- und Save-Verträge gezielt prüfen. | Quellen-/Signaturübersicht, Schutz bestehender NPC-/Gebäudeidentität und eindeutige Zuständigkeiten sind dokumentiert. |
| 1. Business-Anbindung nachweisen | Einen minimalen Adapter im vorhandenen Rathaus erproben; die beiden Architekturwege anhand der Kriterien unten vergleichen. | Genau eine TownHall-Business-Instanz, stabile Kennungen und funktionierender nativer Save-/Load-Weg; nachvollziehbare Entscheidung über MapExpansions. |
| 2. Kauf und Besitz umsetzen | Ray-Angebot, Preis, fehlendes Guthaben, Kaufabschluss, Besitzanzeige und erneuten Kauf behandeln. | Ein Kauf bewirkt genau eine bestätigte Zahlung und genau einen Besitzwechsel; Wiederholung und Fehler verändern kein weiteres Geld. |
| 3. Geldwäsche und Bedienung umsetzen | OG-Computer anbinden, vor Besitz sperren, Kapazität/Gebühr/Periode anzeigen und native Vorgänge bedienen. | Ein vollständiger nativer Geldwäschezyklus ist beobachtet; Grenzen, Fehler und wiederholte Eingaben sind behandelt. |
| 4. Lebenszyklus und Zusammenspiel absichern | Besitz, Station, laufende Geldwäsche und zugehörige Moddaten über Reload, Neustart und Slotwechsel prüfen; BusinessIncome gezielt einbeziehen. | Keine Duplikate, kein Slotübertrag, keine zusätzliche Auszahlung und keine beschädigten fremden Business-Daten. |
| 5. Lokal fertigstellen und abnehmen | Texte und Fehlermeldungen prüfen, Diagnose begrenzen, gezielte Regressionen und einen zusammenhängenden Eigentümertest durchführen. | Alle Kernkriterien unten erfüllt, Artefaktidentität und Nachweise dokumentiert, Dominik bestätigt den spielbaren Business-Kern. |

Persistenz wird bereits in Phase 1 nachgewiesen und bei Kauf und Geldwäsche jeweils
mitentwickelt. Phase 4 prüft das vollständige Zusammenspiel; sie ist nicht der erste
Zeitpunkt, an dem gespeichert und geladen wird.

### Die erste technische Probe

Die Probe beantwortet nur: **Kann unser bestehendes Rathaus als eindeutiges natives
Business mit passender Station aufgebaut und wieder geladen werden?** Sie benötigt
noch keine Korruptionsquests, neue Möbel, Bauphasen oder zusätzlichen Mitarbeiter.

- Business-, Property- und gegebenenfalls Grid-/Stationsidentitäten kollidieren
  nicht mit Vanilla oder anderen Mods. Keine parallele unabhängige Property plus
  Business für denselben Ort ohne nachgewiesenen nativen Bedarf.
- Native Registrierung ist rechtzeitig vor abhängiger nativer Objektladung vorhanden.
  Fehlende Voraussetzungen erzeugen keinen halbfertigen Eintrag.
- Gebäude, originale NPC-Zuordnung, Eingang und bestehender Innenausbau bleiben nutzbar.
  Keine unsichtbare zweite Hülle, doppelten Collider oder fremden Save-Komponenten.
- Eine Station wird genau einmal erzeugt beziehungsweise nativ wiederhergestellt.
  Der Mod spawnt keine zweite Kopie über die native Wiederherstellung.
- Menüwechsel, erneutes Laden und ein frischer Prozess ergeben jeweils genau eine
  gültige Instanz; fremde Registrierungen und geliehene Ressourcen bleiben erhalten.

Ein erfolgreicher Build oder ein sichtbarer Business-Name erfüllt diese Probe nicht.
Scheitert sie, wird vor Kauf- und Geldwäscheimplementierung die Ursache geklärt.

## Zustände, Geld und Speicherung

| Vorgang | Normaler Übergang | Bei unklarem oder teilweisem Ergebnis |
|---|---|---|
| Welt laden | Unbereit → Identität bestätigt → native Instanz vorhanden → Daten geladen → betriebsbereit | Geschäftsfunktionen geschlossen halten; vorhandene Dateien nicht als leeren erfolgreichen Stand überschreiben. |
| Kauf | Unbesessen → Auftrag validiert → Zahlung/Besitz nativ ausgeführt → Ergebnis bestätigt | Native Effekte getrennt feststellen; keine blinde Wiederholung, keine pauschale Rückzahlung bei unbekanntem Abzug. |
| Geldwäsche | Auftrag validiert → nativ angenommen → läuft → nativ abgeschlossen | Vor erneutem Auftrag den nativen Vorgang und tatsächliche Geldbewegung abgleichen. |
| Speichern | Bestätigter Weltkontext → gemeinsamer passender Speicherzeitpunkt → Ergebnis bekannt | Native Daten und Moddaten nicht unabhängig als konsistent erklären; Wiederherstellung für den ursprünglichen Slot erhalten. |
| Welt verlassen | Neue Aktionen sperren → betroffene Szene/Abonnements abbauen → nächsten Kontext prüfen | Ausstehende Vorgänge behalten ihre ursprüngliche Weltidentität; nicht in den nächsten Slot übertragen. |

Native Besitz- und Geldwäschedaten haben jeweils einen maßgeblichen Speicherweg.
Eigene Moddaten speichern nur zusätzlich benötigte Einstellungen, Freischaltungen
oder eindeutig zugeordnete Vorgangsdaten. Für jede Station und jedes native Objekt
wird genau ein Wiederherstellungsverantwortlicher festgelegt.

Bestehende Helfer werden gegen ihre tatsächlichen Verträge geprüft. Ein atomarer
JSON-Schreibvorgang macht native Saves und Moddateien nicht automatisch zu einer
gemeinsamen Transaktion. Unbekannte native Effekte werden abgeglichen, nicht durch
ein Zurücksetzen des gesamten Kontostands verdeckt.

Quellen: [Zahlungsregeln](../../../../Skills/schedule1-economy/SKILL.md),
[Speicherung](../../../../Skills/schedule1-persistence/SKILL.md) und
[BusinessIncome-Erfassung](../../BusinessIncome/src/BusinessResolver.cs).

## Prüfungen für den Business-Kern

Die Implementierung erhält gezielte Offline-Tests für Übergänge und Fehlerfälle.
Diese ersetzen die folgenden Prüfungen im Spiel nicht. Ergebnisse bleiben bis zur
tatsächlichen Ausführung offen.

| Bereich | Erforderliche Prüfung |
|---|---|
| Registrierung | Ohne Besitz genau ein Angebot und eine Business-Identität; fehlende Voraussetzung hinterlässt keinen Teilzustand. |
| Kauf | Zu wenig Guthaben, Abbruch, erfolgreicher Kauf, Doppelklick und erneuter Aufruf nach Besitz. Geld und Besitz gemeinsam auswerten. |
| Geldwäsche | Vor Kauf gesperrt; gültiger Betrag, Kapazitätsgrenze und ungültiger Betrag; tatsächliche Gebühr, Dauer und Gutschrift messen. |
| Laufender Vorgang | Während der Geldwäsche speichern, Spiel neu starten und den Vorgang bis zum Abschluss verfolgen; keine doppelte Annahme/Gutschrift. |
| Speicherung | Gekauft/ungekauft, Menü-Reload und Prozessneustart; zwei eindeutig isolierte Testwelten; beschädigte/neuere Moddaten und fehlgeschlagenes Laden offline simulieren. |
| Rücksetzung | Einen älteren Testcheckpoint mit den dazugehörigen Moddaten wiederherstellen; Besitz und Geld müssen denselben Stand beschreiben. |
| Integration | Bei aktivem BusinessIncome genau eine passende Geschäftserfassung und dessen vorgesehenes Einkommen; keine zusätzliche TownHall-Auszahlung. Namens-/ID-Kollisionen ausschließen. |
| Regression | Eingang, EG/OG, vorhandene NPC-Zuordnung und andere Businesses bleiben funktionsfähig; gezielte Prüfung berührter Funktionen statt Wiederholung des alten Innenraumprojekts. |
| Fehlende Abhängigkeit | Klarer nicht betriebsbereiter Zustand; keine Zahlung, doppelten Objekte oder Überschreibung unbekannter Daten. |
| Rückweg | Business-kritische Daten und Abhängigkeiten nicht einfach entfernen. Rückkehr über den vollständigen Vorher-Checkpoint und passende Artefakte dokumentieren. |

### Voraussetzungen für spätere Ingame-Prüfungen

Dominik übernimmt Spielstart, Bedienung und Gameplay-Abnahme. Vor einer später
beauftragten Installation und Testreihe werden konkreter Testspielstand, Sicherung,
Rückweg und Steam-Cloud-Zustand aktuell festgehalten. Historisch nannten die [README](README.md) einen isolierten SaveGame_2 und das
ältere [Automationsrunbook](live-automation-runbook.md) dagegen SaveGame_1. Der
Widerspruch ist am 2026-11-04 unter Dominiks Auftrag („das was du empfiehlst“)
aufgelöst: **TownHallTest / SaveGame_2 ist der verbindliche Live-Testslot für
alle Business-Proben.** Slot 1 (Dominik / SaveGame_1) bleibt ausschließlich für
eine ausdrücklich freigegebene Probe mit frischer, geprüfter Sicherung zulässig
(wie am 2026-11-04 um 07:11:05 UTC für `inactive-saveable-registration`
freigegeben). **Die neue Save-Probe weist Slot 1 auch mit expliziten Argumenten
ab.** Die am 2026-11-04 vorbereitete Slot-1-/Doppel-Save-Route ist am
2026-11-05 entfernt worden, nicht mehr eine startbare Alternative. Der Manager-
Build ist nun auf TownHallTest / SaveGame_2 festgelegt und meldet vor jeder
Registrierung `blocked / native-save-completion-contract-unverified`.
Die [Offline-Sicherheitsarbeit](business-manager-save-probe.md) erlaubt weder
eine Installation der Speicherprobe noch einen Live-Save. Die anschließend
identifizierte alte installierte `cff67a85…` war tatsächlich verdrahtet; am
5. November wurde sie auf ausdrückliche Auswahl durch Normalbuild `614b81fd…`
ohne Business-Proben ersetzt. [Identität und Grenzen](launch-preparation.md).

**Ausgangstest abgeschlossen, 2026-11-05:** Das Log bestätigt Normalbuild,
S1API 3.2.2 und TownHallTest / SaveGame_2. Dominik bestätigt anschließend
problemloses Handy-Öffnen/-Schließen. [Start-/Lade-/Handy-Nachweis](launch-preparation.md).
Kein erneuter Basistest als offene Voraussetzung; keine Business-Save-/Load-
oder breite API-Abnahme daraus ableiten. Der frühere
[S1API-Upgradebericht](../../../../docs/s1api-upgrade-3.2.2.md) bleibt seinem
damaligen Offline-/Installationsumfang zugeordnet. Vor Builds und Deployment
ist das Spiel gemäß Projektregeln geschlossen.

## Wann das Rathaus lokal fertig ist

- [x] Eingang, EG und OG sind als Innenausbau abgenommen (Dominik, 2026-10-30).
- [ ] Die native Business-Anbindung einschließlich Wiederherstellung ist nachgewiesen.
- [ ] Kauf und Besitz funktionieren mit bestätigter, einmaliger Geldwirkung.
- [ ] Der Verwaltungscomputer bedient einen vollständigen nativen Geldwäschezyklus.
- [ ] Speichern, Reload, Neustart und Slotwechsel bestehen die relevanten Prüfungen.
- [ ] Bestehende Businesses, Rathauszugang und betroffene NPC-Funktionen bleiben nutzbar.
- [ ] Tatsächliche Testwerte, Abhängigkeiten, DLL-Identität und Rückweg sind dokumentiert.
- [ ] Dominik hat den zusammenhängenden Business-Ablauf im Spiel abgenommen.

Erst danach wird der Business-Kern als fertig bezeichnet. Geplante Tests sind keine
bestandenen Tests. Gesamtprüfungen des Repositorys und deren fremde bestehende Fehler
werden getrennt von den TownHall-Ergebnissen ausgewiesen.

## Erweiterungen nach dem Business-Kern

1. Verwaltungsaufträge und Bestechungsdialoge mit eigener Belohnungslogik.
2. Kaufbare Büro-/Ausstattungsstufen und Baufortschritt nach dem Based-Prinzip;
   MapExpansions nur bei konkret benötigten Weltzuständen/Flächen.
3. Mitarbeiter und Sicherheitspersonal mit geprüften Wegen und Tagesabläufen.
4. Zusätzliche Schilder, Aushangtexte und Ausstattung nach Bedarf.

Diese Erweiterungen sind keine Voraussetzung, um den ersten Business-Kern abzunehmen.
Keller und Multiplayer bleiben außerhalb des Plans.

## Separater Weg zur Veröffentlichung

Eine Veröffentlichung benötigt zusätzlich eine verteilbare Assetstrategie als Ersatz
für die lokalen Extraktionen, nachvollziehbare Lizenzen, Paket-/Abhängigkeitsprüfung
und einen Test auf sauberer Installation. Bis dahin kein Release-Archiv für diese Mod.
Die lokale Fertigstellung wird dadurch nicht als fehlgeschlagen bewertet.

## Zuständigkeiten und nächster Arbeitsschritt

Der implementierende Agent bearbeitet jeweils eine Phase und hält Quelltext,
Offline-Prüfungen und Nachweise zusammen. Ein unabhängiger Prüfer kontrolliert vor
Installation die geänderten Verträge und Fehlerfälle. Dominik bewertet das Gameplay.
Bestehende fremde Änderungen werden erhalten; je Datei schreibt nur ein Agent.

**Weltwechselaudit abgeschlossen:** Clean ist ein PreSceneChange-Listener, aber
kein Drain; Exit verwirft die aktive Identität vorher und wartet nicht auf einen
belegten eigenen Last-Use. Der SaveManager folgt dem persistenten Singleton-Pfad.
Eigene Leaving-/Stop-Aufrufer quittieren keine Altauftragfreigabe.
[Belege und begrenzte Architekturentscheidung](native-world-exit-audit.md).

**Besitzentwurf und nachfolgender reiner Datenbaustein abgeschlossen:**
[Vertrag für einen Speicherauftrag](business-save-operation-owner.md) mit
unveränderlicher Bindung, Intent vor Teilwirkung, monotoner Widerrufsordnung,
Prozessslot und unabhängiger Verwaltungsreichweite. Unabhängiger Entwurfsreview:
neun `accepted`, ein `limited`, keine Pflichtkorrektur; optionale Präzisierungen
übernommen. Konkrete native Effekt-/Widerrufssynchronisation weiterhin nachzuweisen.

**Reine Implementierung:** Datenbesitzer mit künstlichen Nachrichten/Identitäten,
einem gemeinsamen Slot je synthetischem Prozessmodell und Modell-Lock umgesetzt.
128 neue Fälle und abschließend 809 TownHall-Offlinetests bestanden, Semantik
unabhängig akzeptiert; 33 eigene Probegruppen des Prüfers bestanden. Einzige
Pflichtkorrektur Testformat separat token-/assertionsgleich behoben. Isoliertes
Pure nicht insgesamt bestanden (Formatstelle inzwischen korrigiert, fremder
Versionsdrift bleibt). Kein globaler OS-Prozessbesitz oder nativer Lockvertrag.
Keine Runtime-Ports, Unity-Objekte, Dateioperationen, alte Save-Trial-Aufrufe oder
Mod-/Lifecycleverdrahtung; kein Deployment oder Modbuild mit Spielreferenzen.

**Phase 1 Schritt 1 (2026-11-06) — Vor-Konstruktions-Teilnehmerbindung & 23-Slot-`ISaveable`-Vertragsinventar:**
- `BusinessSaveProcessModel` und `BusinessSaveOperationOwner` um Vor-Konstruktions-
  Armierung (`TryArmParticipantConstruction`), Konstruktor-Callback-Bindung
  (`EnterConstructorCallback`) und instanzgebundene Teilnehmer-Callbacks
  (`EnterParticipantCallback`) erweitert; Rückkehr von `Construction` ohne
  beobachteten Konstruktoreintritt widerruft mit
  `construction-returned-without-constructor-bind`.
- Vollständiges 23-Slot-`ISaveable`-Dispatch- und Save-Completion-Vertragsinventar
  (`BusinessSaveableContractInventory`, Slots `0..22`, Tokens `0x06001778`..`0x0600178E`,
  Klassen-Vtable-Slots `23..45`) samt deterministischem Gefahrenevaluator
  (`Evaluate`) implementiert: erfasst u. a. den `SaveRoutine.MoveNext`-Deadlock
  (`0x0600183C`, RVA `0x5B0C20`) bei `SaveManager.RegisterSaveable` (`0x0600182E`,
  RVA `0x5AA5C0`) in `Saveables` ohne `BaseSaveables`/`Business.Businesses`-Treiber,
  den synchronen Tail-Jump aus `SaveRequest..ctor` (`0x0600184A`, RVA `0x5ABBC0`,
  `op_Inequality` bei `0x5ABD7B`, Tail-Jump bei `0x5ABDDC`) nach
  `ISaveable.CompleteSave(parentFolderPath, false)` bei leerem `GetSaveString()`,
  die Null-Backing-Field-Gefahr von `LocalExtraFiles` (`0x583010–0x583190`) und
  `LocalExtraFolders` (`0x583195–0x5831D8`) sowie Ordneranlage und Datei-/Ordnerlöschung
  durch Default-`CompleteSave` (`0x06001787`, RVA `0x582F00`, `0x5833D8–0x583795`)
  und `GetContainerFolder` (`0x0600178A`, RVA `0x583840`) vor `DeleteUnapprovedFiles`
  (`0x583778–0x583795`).
- `ProbeBusinessSubtype` und `NativeSubtypePort` prüfen nun die bewaffnete
  Konstruktion (`ArmNextConstruction`/`ConstructedWhileArmed`), binden alle 15
  überschriebenen virtuellen Methoden (einschließlich `GetSaveString`,
  `WriteData(string)` und `DeleteUnapprovedFiles(string)`) und initialisieren
  `propertyName = "Hyland Town Hall"`, `_LocalExtraFiles_k__BackingField` sowie
  `_LocalExtraFolders_k__BackingField`.

**Phase 1 Schritt 2 (2026-11-06) — 16. Virtueller Slot (`ShouldSaveUnderFolder`), non-IO Save-Completion-Pfad & `IBusinessSavePort`-Implementierung:**
- `ProbeBusinessSubtype.ShouldSaveUnderFolder` überschrieben (liefert `false`, erfasst
  `get_ShouldSaveUnderFolder`-Aufrufe); schließt Ordneranlage (`GetContainerFolder`)
  sowie rekursive Datei- und Ordnerlöschschleifen in nativem `CompleteSave` (`0x582F00`)
  zuverlässig aus.
- `NativeSubtypePort.RegisterType()` bindet `get_ShouldSaveUnderFolder` auf
  `Il2CppScheduleOne.Property.Property` (Gesamtzahl gebundener virtueller Methoden:
  exakt **16**); `NativeSubtypePort.Inspect()` validiert alle 16 Slots.
- `BusinessSaveableContractInventory`: Slot 3 (`get_ShouldSaveUnderFolder`) auf
  `OverriddenOnProbeSubtype = true` gesetzt, `ExpectedProbeSubtypeBoundVirtualSlots = 16`,
  Gefahrenevaluator `Evaluate` validiert die 16 Slots deterministisch.
- `NativeSubtypePort` implementiert `IBusinessSavePort`:
  - `ReadSaveState()`: berechnet `CompletionContractVerified` deterministisch aus
    vollständiger 16-Slot-Bindung, armed Konstruktion, leeren Extra-Listen und
    `!ShouldSaveUnderFolder`.
  - `RegisterSaveable()`: registriert den Teilnehmer typsicher in `SaveManager.Saveables`.
  - `SaveOnce()`: führt den synchronen No-IO-Tail-Jump über `ISaveable.Save(folder)` aus.
  - `RemoveSaveable()`: bereinigt `Saveables`, ruft `ClearCompletedSaveable` auf und
    bereinigt Restreferenzen in `CompletedSaveables`.
- 4 neue dedizierte Unittests in `BusinessSaveOperationOwnerTests.cs`, Suite wächst
  auf **850/850 bestandene Tests** (0 failed, 0 skipped); alle 4 No-Deploy-Build-
  Konfigurationen (Normal, Subtype, Interface, Manager) fehler- und warnungsfrei.

**Phase 1 Schritt 3 (2026-11-06) — `BusinessSubtypeTrial.Run`-Verknüpfung mit `IBusinessSavePort`, atomare Frame-Progression & Save-Probe-Absicherung:**
- `BusinessSubtypeTrial.Run` um die Save-Probe-Ausführung erweitert:
  - Gate-Schutz: Ports ohne `IBusinessSavePort` werden sofort vor jeglichem Zugriff mit
    `blocked / native-save-completion-contract-unverified` abgewiesen (0 Port-Zugriffe).
  - Wenn `probeSave == true` und `port is IBusinessSavePort`: Prüfung von
    `preflight.CompletionContractVerified`, Ausführung des Save-Prozesses über
    `BusinessManagerSaveTrial.Begin`, deterministisches Vorantreiben der Ticks bis
    `save.SafeToDestroy && save.CompletionConfirmed` und Snapshot-Readback-Verifikation.
  - `SubtypeResult` um optionales `ManagerSaveResult? Save = null` erweitert;
    `BusinessSubtypeRuntime.Tick()` setzt `SaveCompletionContractVerified = result.Save?.CompletionConfirmed == true`.
- `NativeSubtypePort`: Atomare CAS-Frame-Progression (`NextFrame()`) implementiert,
  wodurch mehrfache Aufrufe innerhalb desselben Unity-Frames strikt um +1 inkrementieren
  und bei fortschreitenden Unity-Frames synchron nachführen (`Frame > _dispatchFrame && Frame > _completionFrame`).
- 4 neue Unittests (3 in `BusinessSubtypeTests.cs` für positiven No-IO-Ablauf, unverified-Vertragsabbruch
  und Context-Loss; 1 in `BusinessSaveOperationOwnerTests.cs` für atomare Frame-Progression).
  Suite wächst auf **854/854 bestandene Tests** (0 failed, 0 skipped). Alle 4 No-Deploy-Builds grün.

**Nächste Grenze nach der begrenzten No-IO-Teilabnahme:**
Der einzelne synchrone eigene Save-/Completion-Pfad und sein Rückbau wurden im
Lauf um 08:56:01 UTC beobachtet. Die native Schreibanbindung bleibt bis zu belegter
Teilnehmer-/Codelebensdauer über Weltwechsel/Shutdown und einem korrelierten
Last-Use-Vertrag für die tatsächliche Schreibroute gesperrt
(`NativeSaveWriteAllowed=false`). Retain oder eine persistente Shell ist kein
Ersatz. Der beobachtete begrenzte Rückbau gibt keine allgemeine transiente
Entfernung während fremder Saves oder Weltwechsel frei. Phase 1 bleibt offen.

Queueentnahme, Completion, Idle, leere Queue und Folgeframe ersetzen den eigenen
Vertrag nicht; [vorheriger Speicherabschlussaudit](native-save-completion-audit.md).
Native Referenzfreigabe, erfolgreicher Save und Konsistenz zusätzlicher Moddaten
getrennt entscheiden. Die vorhandene begrenzte No-IO-Verdrahtung erteilt keine
Freigabe für native Dateischreibpfade oder deren Save-/Destroy-Lebensdauer;
globale Saves anderer Systeme werden nicht abgefangen.

Die Konstruktion/Serialisierung vom 3. November, der inaktive Interface-Aufruf
mit Reloadsperre und die umkehrbare Registrierung vom 4. November bleiben
bestanden; ihre datierten Belege oben werden nicht erneut geöffnet. Anschließend
stehen die originale native Business-Initialisierung, frühe Registrierung vor
nativer Wiederherstellung und echte Speicherung/Wiederherstellung an. Der
eigene no-base Awake-/Start-Lifecycle ist inzwischen begrenzt beobachtet, siehe
jüngste Teilabnahme oben. Keine zusätzliche Framework-
Abhängigkeit und kein Abschluss von Phase 1 allein durch Offline-Tests.

**Übergang zum kontrollierten Ingame-Test vorbereitet und durchgeführt (2026-11-06):**
Für die Verifikation der No-IO Save-Completion im echten Spiel wurde der
[Übergangsplan](business-save-transition-plan.md) aufgesetzt und auf Dominiks Freigabe
durchgeführt (Stufen 1–4). Der Testslot `TownHallTest / SaveGame_2` und `UserData` wurden
in `20261106T054537590245Z-9fbfdbeb` gesichert, der Test-Kandidat (`F11DBF50…`) bereitgestellt
und im Spiel ausgeführt (`20261106T054820764Z.json`). Die Probe brach sicher fail-closed
vor Save-Aufrufen bei `Inspect()` mit `subtype-readback-invalid` ab: Grund ist, dass
`Property.get_ShouldSaveUnderFolder` das IL2CPP-Attribut `METHOD_ATTRIBUTE_FINAL` trägt,
wodurch `il2cpp_object_get_virtual_method` die Basismethode statt des abgeleiteten
VTable-Slots zurückgibt (obwohl native Aufrufe direkt über `[vtable + 0xB0]` die
Überschreibung ansteuern). Keine nativen Dateien wurden hinzugefügt oder gelöscht;
fünf Dateien änderten sich durch regulären Autosave. Normalbuild (`614b81fd…`)
wurde damals unmittelbar wiederhergestellt; der jüngere Stand steht oben.
