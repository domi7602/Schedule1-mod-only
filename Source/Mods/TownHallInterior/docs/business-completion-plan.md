# TownHall Business Fertigstellungsplan

Stand: 2026-11-04. Status: Business-Integration in diagnostischen Teilschritten.

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
freigegeben). Hinweis auf den Arbeitsbaum: `BusinessConstruction.cs` (Stand
2026-11-04, uncommitted) verdrahtet `TOWNHALL_BUSINESS_MANAGER` fest mit
`ExpectedOrganisation="Dominik"`, `ExpectedSlot="SaveGame_1"`,
`ProbeSave=true` und `ProbeMode="inactive-saveable-save"` — das ist die
vorbereitete **Save-Probe** (tatsächlicher `Save()`-Aufruf auf dem echten
Slot 1), nicht der Build des beobachteten Registrierungslaufs (`473064e0…`).
Dieser Stand ist nicht gebaut, installiert oder ausgeführt worden und darf ohne
ausdrücklichen Go und frische Sicherung nicht gebaut oder gestartet werden.
Statische Arbeit kann weitergehen.

Der konkrete Ausgangstest vor dem nächsten Live-Schritt (Dominik): 1) Spiel über
Steam starten (nie `Schedule I.exe` direkt), 2) TownHallTest / SaveGame_2
unverändert laden, 3) Handytest, 4) ohne weitere Eingriffe beenden, Befund an den
nächsten Agenten melden. Vor Builds und Deployment ist das Spiel gemäß
Projektregeln geschlossen.

Der [S1API-3.2.2-Nachweis](../../../../docs/s1api-upgrade-3.2.2.md) dokumentiert
Installation und Offline-Prüfungen, aber noch keinen Spielstart mit dieser Fassung.
Ein unveränderter Start-/Lade-/Handytest bildet deshalb die Ausgangslage für spätere
TownHall-Tests. Vor Builds und Deployment ist das Spiel gemäß Projektregeln geschlossen.

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

**Nächster Nachweis:** Die inaktive native Business-Shell im bestätigten Testslot
(TownHallTest / SaveGame_2) konstruieren, ihre eigene Serialisierung und
vollständige Bereinigung beobachten — als [Konstruktionsprobe](business-construction-probe.md)
bereits am 2026-11-03 um 14:04:58 CET bestanden. Stand 2026-11-04: Der inaktive
ISaveable-Aufruf ist beobachtet (`a8d45f2a…`, 06:58:38 UTC,
[Bericht](evidence/business-interface-live-observed-20261104.json)), die
Einmalversuchsperre im Menü-Reload um 07:00:49 UTC bestätigt (`skipped /
attempt-already-consumed`,
[Bericht](evidence/business-interface-live-reload-20261104.json)), und eine
umkehrbare `SaveManager.RegisterSaveable`-Registrierung ist beobachtet
(`473064e0…`, 07:11:05 UTC, Slot 1 „Dominik“ / SaveGame_1; `Save()` unaufgerufen,
[Bericht](evidence/business-manager-live-register-saveable-20261104.json)). Danach
gezielt Awake/Start, die frühe Registrierung vor dem nativen Wiederherstellen und
native Speicherung (Save/WriteData) entwickeln; der Managerpfad-Nachweis und die
Persistenz bleiben Phasen-1-Kriterien. Ein eigener Adapter bleibt der untersuchte
Weg; keine zusätzliche Framework-Abhängigkeit und kein Abschluss von Phase 1
allein durch diese Proben.
