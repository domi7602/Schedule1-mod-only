# Tatsächlich ausgeführte Prüfungen

Stand: 18. Oktober 2026. Quelltextversion: **0.1.0-dev.1**.

**Gesamtergebnis: Entwicklungszwischenstand. Keine Mod-DLL erzeugt; keine Spielabnahme.**

## Ergebnisübersicht

| Prüfung | Tatsächliches Ergebnis | Aussagegrenze |
|---|---|---|
| Verfügbarkeit von dotnet/csc/mcs/msbuild | Keine der vier Compiler-/Buildanwendungen gefunden | Kein C#-Build möglich |
| Beschaffung des offiziellen SDK 6.0.428 | Nicht erfolgreich; HTTP 403 | Kein SDK installiert, keine Buildausführung |
| Primärquellenprüfung S1API 3.2.1 / MelonLoader 0.7.3 | Relevante Typen und Member am festgehaltenen Quellstand geprüft | Keine Bestätigung der Nutzerinstallation oder Laufzeitwirkung |
| Projekt-XML / JSON-Ressourcen | Syntaxprüfung erfolgreich | Kein Ersatz für Compiler-/Typprüfung |
| Sprachressourcen | Acht Sprachen, jeweils 42 identische Schlüssel; Texte nicht leer, Platzhalter konsistent | Übersetzungsqualität, UI-Platzbedarf und Laufzeitbindung offen |
| Statischer C#-Review | Konkrete Befunde behoben; Details unten | Kein ausführbarer Funktionsnachweis |
| C#-Kernprüfkatalog | 62 fortlaufende, eindeutige verfasste Fälle; Methoden zugeordnet | **Nicht ausgeführt: 0 als bestanden gewertet** |
| Python-Pakettests | **26/26 bestanden; 0 Fehler, 0 übersprungen; Python 3.12.14** | Isolierte Dateien und synthetische Belege; keine echte Mod-/Spielprüfung |
| Paketwerkzeuge über CLI | Dateiliste → Manifest → DEVELOPMENT-ZIP → Verifikation mit vier ausgewählten Quellen erfolgreich | Getrennter Werkzeugtest, noch nicht das vollständige Downloadarchiv |
| PowerShell 5.1 / Windows-Starter | Statisch geprüft; UTF-8-BOM und Arbeitsverzeichnis berücksichtigt | **Nicht unter Windows ausgeführt** |
| Diagnose-DLL kompilieren und laden | Nicht ausgeführt | Quelltext allein vorhanden |
| Produktive Mod-DLL kompilieren und laden | Nicht ausgeführt; produktive Integration fehlt | Kein Releasekandidat |
| Spieltests T01–T18 und Performance | **Alle offen** | Keine Zusicherung von Spiel-/Multiplayerkompatibilität |

## Python-Testumfang

Der abschließende Lauf des Standardbibliothek-Unittest-Runners umfasst
`tests/packaging/test_packaging.py` mit 26 ausgeführten Tests. Der vollständige
Wiederholungsaufruf und die Toolargumente stehen in `BUILD.md`.

Geprüft wurden insbesondere:

- Vollständiger Archivdurchlauf und deterministisches Ergebnis für dieselben synthetischen Quellbytes.
- Änderung von Quelle oder Kandidat nach Festhalten der Prüfsumme.
- Relative Pfadtraversierung auf Quell- und Archivseite, Groß-/Kleinschreibungs- sowie Datei-/Ordnerkollisionen unter Windows.
- Symbolische Links, unerlaubte Quellbereiche, Binärdateien und NUL-Zeichen in vermeintlichen Textquellen.
- Versuch, eine Textdatei als DLL oder in einen Installationsbereich des Entwicklungsarchivs umzubenennen.
- Schutz einer vorhandenen ZIP und einer konkurrierend angelegten temporären Datei.
- Verwechslung von Core-/Diagnoseartefakt mit der produktiven Mod.
- Ungültige/native PE-Dateien, mitkopierte Spielreferenzen und nicht zugeordnete Fremd-DLLs.
- Fehlende Pflichtprüfungen, simulierte statt tatsächlicher Spielabnahme und falscher DLL-Bezug.
- Unzulässige vorgezogene T18-Freigabe und abweichender endgültiger ZIP-Hash.
- Fälschliche Hochstufung allein durch Änderung eines Statusmarkers ohne passende interne Abnahme.
- Fehlende Laufzeit-Dateizuordnung selbst bei nachträglich neu berechneten Archivprüfsummen.

Die Testfixtures erzeugen ausdrücklich künstliche PE-/CLR-Header in
temporären Verzeichnissen. **Diese Bytes sind keine kompilierte Mod.** Sie
prüfen die Reaktion der Paketregeln auf isolierte Eingaben und werden nicht
als DLL im Entwicklungsarchiv ausgeliefert. Die Werkzeuge validieren
eingereichte Belege; sie können keine nicht stattgefundene Spielhandlung
beobachten oder einen unwahren Bericht zu echter Evidenz machen.

## Statische C#-Befunde und Korrekturen

Der Diagnosequelltext verwendet jetzt den in MelonLoader 0.7.3 belegten
Namespace `MelonLoader.Utils` für `MelonEnvironment`. Die SHA-256-Berechnung
arbeitet mit dem unter .NET 6 vorhandenen `SHA256.Create().ComputeHash(Stream)`.
Der zuvor mögliche Gebrauch einer erst später verfügbaren statischen
Stream-Überladung wurde entfernt. Der Loader-Versionsstring und die
Projektversion sind auf `0.1.0-dev.1` abgestimmt.

Im Kern wurde der Fall doppelter erfolgreicher Initiallade-Benachrichtigungen
korrigiert: Eine solche Wiederholung verschlechtert eine bestehende
Schreibfreigabe nicht; ein tatsächlicher später gemeldeter Lesefehler sperrt
weiterhin. C61 und C62 beschreiben diese beiden unterschiedlichen Fälle.
Snapshot-/Delta-Rennen, Sender-/Sitzungsgrenzen und reentrante Autorisierung
wurden unabhängig statisch gegengelesen. Unnötige Enum- und UTF-8-Array-
Allokationen aus der Effektberechnung wurden entfernt. Daraus wird keine
gemessene Framerate oder bestandene Laufzeitprüfung abgeleitet.

## DLL- und Archivzuordnung

Für Core, Diagnose und Hauptmod gibt es in diesem Stand **keinen erzeugten
DLL-Hash**. Deshalb wird kein Prüfergebnis einer vermeintlichen DLL zugeordnet.
`BUILD_ENVIRONMENT.json` dokumentiert den fehlenden Build und die Umgebung.

Das endgültige DEVELOPMENT-Archiv wird nach Fertigstellung dieses Dokuments
aus einer ausdrücklichen Dateiliste erzeugt, mit `SHA256SUMS.txt` versehen,
auf Integrität geprüft und in ein frisches Verzeichnis entpackt. Sein
tatsächlicher Dateiname, seine Größe und der abschließende SHA-256 werden
außerhalb der ZIP bei der Übergabe genannt. Die Archivprüfung ist eine
Dateiprüfung und **nicht T18**, denn es gibt noch keine installierbare Mod.

Das Zielpaket `LightControl-1.0.0-IL2CPP.zip` bleibt bis zur tatsächlichen
Implementierung und Abnahme aller Pflichtfunktionen offen.
