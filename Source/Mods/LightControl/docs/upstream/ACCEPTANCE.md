# Abnahmekatalog T01–T18

**Alle nachfolgenden Spielprüfungen sind offen.** Es existiert noch keine erzeugte produktive LightControl-DLL, der ein Prüfergebnis zugeordnet werden könnte. Der Quelltextstand heißt `0.1.0-dev.1`. Ein Console-Kerntest oder synthetischer Pakettest besteht keinen dieser Spieltests.

## Protokollfelder je tatsächlichem Durchlauf

- Testnummer, Datum, Prüfer und konkrete Installation.
- Modversion, Quellstand, geprüfter DLL-Dateiname und SHA-256.
- Spielbuild, Zweig, Unity-Version, Loader-, S1API- und gegebenenfalls Transportversion.
- Tatsächliche Schritte und Beobachtung; relevante bereinigte Logs oder Aufnahme.
- Ergebnis `bestanden`, `fehlgeschlagen` oder `offen`; bei Fehler reproduzierbare Abweichung.

Kein leeres Feld und keine geplante Erwartung werden als Beobachtung behandelt. Ein neuer DLL-Hash verlangt einen neuen Kandidaten und die erneute Abnahme gemäß Auftrag.

## T01 – Echte Release-DLL laden

**Voraussetzungen:** Saubere Installation mit bestätigten Versionen; frischer Releasekandidat und SHA-256 bekannt.

**Schritte:** DLL und benötigte eigene Laufzeitdateien installieren, Spiel starten, Save laden, ins Menü zurückkehren und erneut laden.

**Erwartung:** Loader erkennt richtige Version, keine Initialisierungsfehler und keine versteckten Entwicklerdateien nötig.

**Tatsächlich:** Nicht ausgeführt. Mod-DLL und DLL-Hash fehlen. **Status: offen.**

## T02 – Genau eine Aktion pro Tastendruck

**Voraussetzungen:** Mindestens eine Pflanzenlampe und schaltbare Lampe, Interaktion/Sprint auch anders belegt.

**Schritte:** Einzeldruck und Halten, Sprintmodifikator, native Schaltung, verschiedene Collider, Textfeld, Telefon, Menü, Wand dazwischen und Entfernungsgrenze testen.

**Erwartung:** Genau eine Mod-Aktion bei zulässigem Kontext; native Aktion nur im eindeutigen Mod-Fall unterdrückt; Sperrkontexte bleiben ohne Farbwechsel.

**Tatsächlich:** Nicht ausgeführt. Mod-DLL und DLL-Hash fehlen. **Status: offen.**

## T03 – Originalwerte nach Farbzyklus

**Voraussetzungen:** Zwei Lampen mit unterschiedlichen nativen Farben/Helligkeiten.

**Schritte:** Alle elf Farbmodi durchlaufen, wieder Original wählen; währenddessen native Ein/Aus-Zustände ändern.

**Erwartung:** Jede Leuchte erhält ihren aktuell gültigen Originaleinfluss zurück; kein fremder Baselinewert, kein veralteter Einschaltzustand.

**Tatsächlich:** Nicht ausgeführt. Mod-DLL und DLL-Hash fehlen. **Status: offen.**

## T04 – Alle Lampenfamilien und Emission

**Voraussetzungen:** Je ein benanntes Exemplar jeder der 14 Pflichtfamilien; Shader-/Materialzuordnung dokumentiert.

**Schritte:** Farbe und Helligkeit ändern, Lichtabstrahlung und leuchtende Oberfläche aufnehmen; ausgeschlossene Objekte mitprüfen.

**Erwartung:** Unterstützte Echtzeitanteile und Emission ändern sich passend, Texturen bleiben erhalten; Ausschlüsse unbeeinflusst. Gebackene Anteile als konkrete Grenze erfassen.

**Tatsächlich:** Nicht ausgeführt. Mod-DLL und DLL-Hash fehlen. **Status: offen.**

## T05 – Intensität, Flackern, Ein/Aus

**Voraussetzungen:** Statische und flackernde Leuchte, Zugriff auf unveränderte Vergleichswerte.

**Schritte:** Intensitätsstufen mehrfach vor/zurück wechseln, mindestens 30 Sekunden konstant halten, Ein/Aus und Animation beobachten.

**Erwartung:** Kein exponentielles Aufhellen/Abdunkeln; Faktor pro frischem nativen Basiswert; Flackern und Originalverhältnisse bleiben erhalten.

**Tatsächlich:** Nicht ausgeführt. Mod-DLL und DLL-Hash fehlen. **Status: offen.**

## T06 – Gameplayneutralität

**Voraussetzungen:** Gleicher Testsave, gleiche Pflanze, Beleuchtung/Versorgung, Zeit, Wachstumsbedingungen und Spielbuild.

**Schritte:** Vergleichslauf ohne visuelle Änderung und mit mehreren Modfarben/Intensitäten; relevante Werte protokollieren.

**Erwartung:** Keine durch visuelle Einstellungen verursachte Änderung von Wachstum, Qualität, Ertrag oder Versorgung. Native Schaltaktionen separat bewerten.

**Tatsächlich:** Nicht ausgeführt. Mod-DLL und DLL-Hash fehlen. **Status: offen.**

## T07 – Instanzidentität und Speicherung

**Voraussetzungen:** Zwei identische platzierte Lampen mit unterschiedlicher Auswahl und belegten Instanz-IDs.

**Schritte:** Farben setzen, dieselbe Instanz bewegen, speichern, mindestens dreimal laden und erneut beitreten.

**Erwartung:** Keine vertauschten/zusammengefallenen Zustände; dieselbe Instanz bleibt eindeutig.

**Tatsächlich:** Nicht ausgeführt. Mod-DLL und DLL-Hash fehlen. **Status: offen.**

## T08 – Entfernung, Neuerzeugen und Entladen

**Voraussetzungen:** Lampenidentitäten, eine zeitweise ausgeblendete/entladene Immobilie und aktive Disco.

**Schritte:** Lampe aufnehmen/neu platzieren, andere Instanz bewegen, Immobilie verlassen/zurückkehren, unter Disco platzieren und entfernen.

**Erwartung:** Neuerzeugte Instanz Standard plus aktiver Effekt; fortbestehende Instanz behält Grundzustand; vorübergehende Abwesenheit löscht keine gespeicherten Werte.

**Tatsächlich:** Nicht ausgeführt. Mod-DLL und DLL-Hash fehlen. **Status: offen.**

## T09 – Save- und Menüwechsel

**Voraussetzungen:** Spielstände A/B mit unterscheidbaren Lampenwerten.

**Schritte:** A laden, Menü, B laden, Menü, A laden; Vorgang wiederholen und Ladevorgang gegebenenfalls abbrechen.

**Erwartung:** Keine übergreifenden Einstellungen, doppelten Ereignisbindungen oder veralteten UI-/Objektreferenzen.

**Tatsächlich:** Nicht ausgeführt. Mod-DLL und DLL-Hash fehlen. **Status: offen.**

## T10 – Telefon aus Nähe und Entfernung

**Voraussetzungen:** Eigene Telefon-App und bestätigte Immobilien-/Gruppenregister.

**Schritte:** Einzel-/Gruppen-/Immobilienauswahl, Farbe/Intensität/Ein/Aus, gemischte Gruppen, Entfernung und erneutes Öffnen testen.

**Erwartung:** Telefon und Direktbedienung lesen denselben maßgeblichen Zustand; keine doppelten Einträge; geschlossen keine Vollaktualisierung.

**Tatsächlich:** Nicht ausgeführt. Mod-DLL und DLL-Hash fehlen. **Status: offen.**

## T11 – Gruppen, Räume und Besitz

**Voraussetzungen:** Dokumentierte native Schalterbeziehungen, besessene und unbesessene Immobilie.

**Schritte:** Gruppenänderungen, späteres Platzieren, Erwerb, Raumwechsel und Bereichsgrenzen prüfen.

**Erwartung:** Richtige eingefrorene Zielmenge; neue Lampe erbt keine frühere Gruppenänderung; keine aus Besitz-Flag erfundenen individuellen Rechte.

**Tatsächlich:** Nicht ausgeführt. Mod-DLL und DLL-Hash fehlen. **Status: offen.**

## T12 – Mod-Host/Client und Beitrittsabgleich

**Voraussetzungen:** Zwei reale Modinstallationen auf dem vorgesehenen Steam-Verbindungsweg; DLL-/Protokollhashes bekannt.

**Schritte:** Gleichzeitig ändern, neu beitreten/wiederbeitreten, Gruppe ändern, ältere Snapshots/fehlende Deltas gezielt simulieren und reale Sitzung prüfen.

**Erwartung:** Hostzustand konvergiert; ältere Daten überschreiben nichts Neues; unbekannte Objekte werden vorgemerkt; keine Rückkopplung oder falsche Absenderautorität.

**Tatsächlich:** Nicht ausgeführt. Mod-DLL und DLL-Hash fehlen. **Status: offen.**

## T13 – Vanilla-Mischbetrieb und Host ohne Mod

**Voraussetzungen:** Mod-Host mit Vanilla-Client sowie Vanilla-Host mit Mod-Client.

**Schritte:** Verbinden, native Schaltung/Farbe testen; danach in Mod-Host-Sitzung wechseln, Protokollabweichung testen.

**Erwartung:** Vanilla bleibt kompatibel; Mod-Client ohne Mod-Host zeigt lokalen Sitzungsmodus, schreibt keine gemeinsamen Lampendaten und überträgt lokale Altwerte später nicht automatisch.

**Tatsächlich:** Nicht ausgeführt. Mod-DLL und DLL-Hash fehlen. **Status: offen.**

## T14 – Überlagerte Effekte

**Voraussetzungen:** Welt- und mindestens zwei Grundstückseffekte, ausgeschaltete sowie neu platzierte Lampen.

**Schritte:** Disco starten/stoppen, gleichzeitig Grundfarbe und Helligkeit ändern, Bereich wechseln, spät beitreten.

**Erwartung:** Grundstück gewinnt; andere Grundstücke unabhängig; Helligkeit sofort; neue Grundfarbe nach Effektende sichtbar; ausgeschaltete Lampen bleiben aus; keine Effektdauerpersistenz.

**Tatsächlich:** Nicht ausgeführt. Mod-DLL und DLL-Hash fehlen. **Status: offen.**

## T15 – Ungültige und beschädigte Daten

**Voraussetzungen:** Kopie des Saves; Backup-/Wiederherstellungsweg; beschädigte, doppelte und zukünftige Datenschemata.

**Schritte:** Fehler beim Lesen, abgeschnittene JSON-Datei, falschen Save, doppelte IDs, ungültige Zahlen und unbekannte Version laden; danach Save auslösen.

**Erwartung:** Keine Überschreibung gültiger/unklarer Daten durch Defaults; explizite Schreibsperre; vollständige sichere Wiederherstellung; falsche Sitzung abgewiesen.

**Tatsächlich:** Nicht ausgeführt. Mod-DLL und DLL-Hash fehlen. **Status: offen.**

## T16 – Deaktivierung, Update und Deinstallation

**Voraussetzungen:** Originalzustände und eigene Ressourcen bekannt; ausdrücklich unterstütztes Vorgängerformat vorhanden, falls zugesagt.

**Schritte:** Mod deaktivieren, wieder aktivieren, unterstützte Testversion aktualisieren, DLL entfernen und Save normal laden.

**Erwartung:** Eigener Einfluss sauber entfernt, keine fremden Materialien verändert, keine doppelten DLLs; zugesagte Migration funktioniert; keine Modpflicht für normalen Save-Ladevorgang.

**Tatsächlich:** Nicht ausgeführt. Mod-DLL und DLL-Hash fehlen. **Status: offen.**

## T17 – Ressourcen und Performance

**Voraussetzungen:** Definierte 250-Leuchten-Szene; sichtbare Lichter, Schatten, Zeit, Kamera, Grafikoptionen und Hardware festgehalten.

**Schritte:** Baseline, statische Modwerte und Animation messen; mindestens zehn Lade-/Platzierungszyklen; Hauptthread, GPU, eigene Materialien, Events, Netzwerk und Savefrequenz erfassen.

**Erwartung:** Keine monotonen eigenen Ressourcenverluste; keine Vollsuche/Materialerzeugung je Frame oder bildratenproportionaler Netzwerk-/Saveverkehr. P95-Budgets getrennt auswerten.

**Tatsächlich:** Nicht ausgeführt. Mod-DLL und DLL-Hash fehlen. **Status: offen.**

## T18 – Installation aus endgültiger ZIP

**Voraussetzungen:** T01–T17 mit eingefrorenem DLL-Hash; endgültige ZIP und SHA-256; frischer leerer Installationszustand plus Voraussetzungen.

**Schritte:** ZIP frisch entpacken; ausschließlich beiliegende Anleitung/Dateien verwenden; Direktbedienung, Telefon, Save/Laden, Multiplayer, Update und Deinstallation durchlaufen.

**Erwartung:** Alle zugesagten Abläufe funktionieren ohne Projektdateien. Externes Ergebnis nennt exakt den abschließenden ZIP-Hash; interne ZIP-Dateien werden danach nicht mehr geändert.

**Tatsächlich:** Nicht ausgeführt. Mod-DLL und DLL-Hash fehlen. **Status: offen.**

## Performancebudgets

Die anfänglichen Projektziele aus dem Auftrag sind bei 250 registrierten Leuchten höchstens ungefähr 0,5 ms zusätzliche Hauptthreadzeit im 95. Perzentil für statische Werte und ungefähr 1 ms für Animation. Es handelt sich um ungemessene Ziele. Ein kontrollierter Ausgangslauf muss zuerst festlegen, wie Messfenster, Hauptthreadkosten, GPU-Kosten und Warm-up getrennt erfasst werden. Kosten absichtlich eingeschalteter nativer Lichter werden separat ausgewiesen. Budgetänderungen benötigen eine nachvollziehbare Begründung und dürfen kein nachträgliches Bestehen erzeugen.

## Bindung des letzten Installationstests

Der interne Testbericht kann T18 als ausstehende Archivprüfung führen. Nach Erstellung der endgültigen ZIP wird T18 ausschließlich außerhalb des Archivs protokolliert und an dessen SHA-256 gebunden. Änderungen an einer Archivdatei erfordern eine neue ZIP und eine neue T18-Prüfung. Änderungen an der DLL erfordern zusätzlich einen neuen Releasekandidaten und die erneute Abnahme. `SHA256SUMS.txt` enthält weder seine eigene Prüfsumme noch einen zirkulären Hash der ZIP.

