# Build, Prüfnachweise und Paketierung

## Aktueller Lieferstatus

Dieser Stand ist **DEVELOPMENT / Quellcode**. Er enthält keine kompilierte Release-Mod und erfüllt die Spielabnahme T01–T18 noch nicht. `tools/integration-status.json` hält die produktive Mod-Integration ausdrücklich gesperrt. Eine Core- oder Diagnoseassembly darf nicht in `LightControl.dll` umbenannt und als fertige Mod ausgeliefert werden.

Die Werkzeuge unterscheiden vier Ergebnisse:

| Ergebnis | Inhalt und Aussage |
| --- | --- |
| `Core` / `core-only` | Spielunabhängiger C#-Kern und zugehöriger Konsolentest. Kein MelonLoader-Modul. |
| `Diagnostics` / `diagnostics` | Eigenständige Diagnoseassembly mit tatsächlicher MelonLoader-Referenz. Keine produktive Lichtsteuerung. |
| `Mod` / `mod` | Erst nach vollständiger, belegter Integration zulässig. Im aktuellen Stand gesperrt. |
| `development` | Ausdrücklich ausgewählte Textquellen, Dokumentation und Werkzeuge. Keine DLLs oder Spielreferenzen. |

Im hier verfügbaren Erstellungsumfeld konnten weder das benötigte .NET-SDK noch Windows PowerShell ausgeführt werden. C#-Kompilierung, C#-Tests und ein Windows-PowerShell-Lauf bleiben deshalb offen. Die Python-Paketprüfungen wurden separat tatsächlich ausgeführt; deren Ergebnis steht weiter unten. Diese unterschiedlichen Prüfarten ersetzen einander nicht.

## Voraussetzungen für deinen lokalen Build

- Windows PowerShell 5.1 oder eine geeignete neuere PowerShell für die Windows-Skripte.
- Genau das in [global.json](../global.json) festgelegte .NET-SDK **6.0.428**. Eine installierte .NET-Laufzeit allein reicht zum Kompilieren nicht.
- Für `Diagnostics`: die konkrete `MelonLoader.dll` aus dem `net6`-Verzeichnis der zu prüfenden MelonLoader-Installation. Die Zielversion und ihr Beleg gehören in die Kompatibilitätsmatrix.
- Für die Python-Werkzeuge: Python 3.10 oder neuer. Sie verwenden ausschließlich die Standardbibliothek.

Die Skripte installieren nichts, suchen keine Spielinstallation, laden keine Werkzeuge herunter und benötigen keine pauschalen Administratorrechte. Die Doppelklick-Starter verändern auch die PowerShell-Ausführungsrichtlinie nicht. Wenn deine Richtlinie die Ausführung blockiert, bleibt der Fehler sichtbar und der Ablauf endet. Gib dann die konkrete Fehlermeldung zurück; eine Ausführung wird nicht stillschweigend umgangen.

Die Projektversion kommt aus [Directory.Build.props](../Directory.Build.props). Ein optionales `-Version` muss denselben Wert enthalten. Eine Versionsänderung gehört zuerst in den überprüfbaren Quellstand einschließlich der Loader-Attribute.

## Core bauen und die echten Kerntests ausführen

1. Entpacke das vollständige Entwicklungsprojekt in einen eigenen Ordner.
2. Prüfe, dass das festgelegte SDK verfügbar ist.
3. Öffne `tools/BUILD_CORE.cmd` per Doppelklick. Alternativ starte `tools/build.ps1` mit `-Mode Core` aus PowerShell.
4. Ein erfolgreicher Lauf nennt einen neuen Ordner unter `artifacts/builds` und erzeugt darin `build-record.json`.
5. Prüfe `status`, `artifact_kind` und `core_tests` im Beleg. Nur ein tatsächlich erfolgreich ausgeführter Konsolentest erhält dort `core_tests: passed`.

Das Script baut im Core-Modus das Testprojekt. Die getestete `LightControl.Core.dll` stammt dadurch aus demselben neuen Ausgabeordner wie der Test-Runner. Das Ergebnis ist eine Kernbibliothek; es wird nicht in den Spielordner installiert.

### Öffentliche Build-Argumente

| Argument | Bedeutung |
| --- | --- |
| `-Mode Core` | Standard: Kern einschließlich Konsolentests bauen und testen. |
| `-Mode Diagnostics` | Separates Diagnoseprojekt gegen die explizite Loader-Referenz bauen. Der Lauf im Spiel ist anschließend noch offen. |
| `-Mode Mod` | Im aktuellen Stand absichtlich gesperrt. |
| `-DotNet` | Optional ein ausdrücklich gewählter Pfad zur `dotnet`-Anwendung. Das aktive SDK wird trotzdem exakt geprüft. |
| `-ReferenceDirectory` | Expliziter lokaler Referenzordner; für Diagnostics muss er die wirkliche `MelonLoader.dll` enthalten. Kein automatisches Suchen. |
| `-OutputRoot` | Optionales Ziel unter dem `artifacts`-Unterbaum dieses Projekts. Quellverzeichnisse sind gesperrt. |
| `-Version` | Optionaler Abgleich mit der festgehaltenen Projektversion; kein abweichender Versions-Override. |

## Schutz vor alten oder uneindeutigen Builds

Jeder Lauf erhält eine neue Kennung aus UTC-Zeitpunkt und Zufallsanteil. Er erstellt einen eigenen Quell-Snapshot und leere Ausgabeordner. Bereits vorhandene `bin`-, `obj`- und `artifacts`-Verzeichnisse werden nicht als Quellen übernommen. Verzeichnislinks im Ausgabeweg werden abgelehnt; Links innerhalb der Quellsuche werden nicht verfolgt.

Der Snapshot enthält die ausgewählten Projektdefinitionen, C#-Quellen, Tests und benötigten Textassets. Jede Quelldatei wird gehasht; die geordnete Liste ergibt den `source_fingerprint`. Buildreferenzen werden aus dem ausdrücklich gewählten Ordner in einen eigenen lokalen Referenz-Snapshot kopiert und ebenfalls gehasht. Diese Entwicklungskopien verbleiben unter `artifacts` und gehören nicht in ein Auslieferungsarchiv.

Erst ein erfolgreicher Restore, Build und gegebenenfalls Core-Testlauf kann `status: succeeded` setzen. Die erwartete Assembly muss im neuen Ordner vorhanden sein. Ihre verwaltete Assemblyidentität, Größe und SHA-256 werden aufgezeichnet. Bei einem Fehler ist der Beleg `failed`, seine Liste freigegebener Artefakte bleibt leer und das Script liefert einen Fehlercode. Eine vorherige DLL wird nicht ersatzweise übernommen.

`restore.log`, `build.log` und `core-tests.log` bleiben lokale Arbeitsbelege. Sie können vollständige lokale Pfade enthalten und werden nicht automatisch in ein Paket übernommen. Der Buildbeleg dokumentiert eine wiederholbare Vorgehensweise; byteidentische C#-Wiederholungsbuilds wurden noch nicht nachgewiesen.

Der spätere Mod-Build benötigt zusätzlich eine bestätigte `tools/mod-build-references.json` mit einer ausdrücklichen DLL-Dateiliste. Diese Datei wird gegenwärtig bewusst nicht mit erfundenen Referenzen gefüllt. Ebenso ist eine vollständige konkrete Liste zusätzlicher produktiver Laufzeitdateien erst mit der tatsächlichen Mod-Integration festzulegen. Ein Buildordner darf dafür nicht pauschal kopiert werden.

## Entwicklungsarchiv erzeugen

Die Dateiauswahl ist eine überprüfbare Allowlist. Der Paketierer scannt keine Verzeichnisse, erweitert keine Wildcards und übernimmt keine zufällig gefundenen Dateien.

1. Lege eine UTF-8-Textdatei mit genau einer ausgewählten, projektrelativen Quelldatei pro Zeile an. Leere Zeilen und Zeilen mit vorangestelltem `#` sind Kommentare. Verwende `/` als Pfadtrenner. Änderungen der Dateiauswahl werden bewusst in dieser Liste vorgenommen.
2. `tools/create-development-manifest.py` erhält `--root`, `--file-list`, `--output` und optional `--prefix`. Es erzeugt daraus ein **neues** Entwicklungsmanifest mit SHA-256 für jede Datei. Der Standardpräfix innerhalb der ZIP ist `LightControl`.
3. `tools/package.py development` erhält `--root`, `--manifest` und `--output`. Der ZIP-Dateiname muss `DEVELOPMENT` enthalten, beispielsweise `LightControl-0.1.0-dev.1-DEVELOPMENT-SOURCE.zip`.
4. Jede Datei wird unmittelbar vor dem Paketieren gegen den im Manifest festgehaltenen Hash geprüft. Eine inzwischen geänderte Datei sperrt die Paketierung.
5. Der Paketierer prüft ZIP-Integrität, Dateiliste und Hashes und schreibt den Inhalt in einen frischen temporären Entpackordner zurück. Erst danach wird die endgültige neue ZIP bereitgestellt.

Ein Manifest ist ein JSON-Objekt mit `schema_version: 1`, `kind: development` und einer Liste `files`. Jeder Eintrag besitzt die Felder `source` (projektrelativer Eingabepfad), `archive` (genauer ZIP-Pfad), `role: source` und `sha256` (64 Kleinbuchstaben/Hexziffern). Das erzeugte Manifest darf sich nicht selbst mit einer eigenen Prüfsumme einschließen. Bewahre die Eingabe unter `artifacts` auf. Der Paketierer übernimmt die ursprüngliche Liste zusätzlich als `PACKAGE_MANIFEST.json` ins Archiv, ohne sie in ihre eigene Quelldateiliste einzutragen. Damit lässt sich dieselbe Auswahl nach dem Entpacken wiederverwenden.

Erlaubt sind die im Werkzeug definierten Textformate für C#, Projekte, XML/JSON-Assets, Dokumentation, Python, PowerShell und CMD. DLLs, ausführbare Binärdateien, Buildverzeichnisse, Referenzkopien, persönliche Sitzungslogs und Spielstände sind im Entwicklungsmodus gesperrt.

Die Dateien `PACKAGE_STATUS.txt`, `PACKAGE_METADATA.json`, `PACKAGE_MANIFEST.json` und `SHA256SUMS.txt` werden vom Paketierer selbst erzeugt. Das Archiv enthält keine Prüfsumme der Prüfsummen-Datei selbst. Der abschließende ZIP-Hash wird außerhalb des Archivs ausgegeben.

## Spätere Test- und Release-Gates

Diese Schnittstellen sind vorbereitet, im aktuellen Integrationsstand jedoch gesperrt. Sie stellen keine Behauptung einer vorhandenen Mod dar.

| Aufrufmodus | Voraussetzungen und resultierender Status |
| --- | --- |
| `candidate` | Wirkliche produktive Mod-Assembly aus erfolgreichem frischem Build; exakt zugehöriger Abnahmebericht; T01–T18 vollständig aufgeführt. Offene oder fehlgeschlagene Spielprüfungen sind sichtbar zulässig. Der Name endet auf `-IL2CPP-TEST.zip`. |
| `release` | Zusätzlich müssen T01–T17 im Zielspiel bestanden, alle Pflichtanforderungen abgeschlossen, Integrationsblocker leer und die konkrete Versionsmatrix bestätigt sein. Das Archiv ist zunächst ein Releasekandidat für T18. |
| `verify` | Prüft eine bestehende ZIP auf sichere Dateipfade, Integrität, vollständige Prüfsummen und Entpackbarkeit. Führt kein Spiel aus. |
| `verify-release` | Prüft zusätzlich einen externen T18-Bericht gegen den endgültigen ZIP-Hash und DLL-Hash. Verändert die ZIP nicht. |

Für `candidate` und `release` sind `--root`, `--manifest`, `--build-record`, `--acceptance-report` und `--output` erforderlich. Der Paketierungsschritt ruft keinen Compiler auf. Er akzeptiert nur die im Buildbeleg genannten Laufzeitdateien mit denselben Hashes. Der PE-/CLR-Strukturtest ist eine Plausibilitätskontrolle; nur T01 bestätigt das tatsächliche Laden als Mod im Zielspiel.

### Inhalt eines späteren Laufzeitmanifests

`kind` ist `mod`. `files` führt jede Eingabe mit Zielpfad und SHA-256 ausdrücklich auf. Genau ein Eintrag mit `role: mod` wird zu `Mods/LightControl.dll`. README, Änderungsübersicht und Lizenztext sind `documentation`. Zusätzliche Laufzeitdateien sind `runtime` und nennen eine zugehörige Abhängigkeitskennung.

`dependency_review_complete` muss `true` sein. In `dependencies` erhält jede direkte und indirekte Abhängigkeit eine Kennung, konkrete Version und einen Bereitstellungsweg: `game`, `loader`, `separate` oder `bundled`. Für tatsächlich mitgelieferte Abhängigkeiten sind zusätzlich Herkunft, Lizenzbeleg und die vollständige Liste ihrer Archivpfade erforderlich. Diese Liste muss exakt mit den `runtime`-Einträgen übereinstimmen. Eine fremde DLL ohne Zuordnung wird abgelehnt. Spiel-, Unity-, generierte Interop-, Loader- und S1API-Assemblies werden nicht mitgeliefert.

`BUILDINFO.txt`, `COMPATIBILITY.txt` und `TESTREPORT.txt` werden aus den validierten Belegen erzeugt. `ACCEPTANCE_REPORT.json` hält die vollständigen minimalen Abnahmedaten fest; das Laufzeit-`PACKAGE_MANIFEST.json` enthält die Rollen, Hashes und Abhängigkeitszuordnungen ohne lokale Quelldateipfade. Der spätere Archivprüfer kontrolliert diese Daten erneut, einschließlich aller T01–T17-Gates, der verwalteten PE-Struktur, verbotener bzw. nicht zugeordneter DLLs und der Übereinstimmung mit dem lesbaren Testbericht. Ein bloßes Release-Markerfeld reicht nicht zur Freigabe.

Vollständige rohe Build- oder Sitzungslogs werden dabei nicht kopiert. Kuratiere die Angaben des Abnahmeberichts vorab so, dass er keine persönlichen Pfade oder privaten Sitzungsdaten enthält.

### Abnahmebericht und externer T18-Bericht

Der Abnahmebericht nennt `schema_version`, `candidate_id`, `mod_version`, `dll_sha256`, `runtime_sha256`, `requirements_complete`, `blockers`, `compatibility` und `tests`. `runtime_sha256` ordnet jedem ausgelieferten Mod-/Laufzeit-Archivpfad seinen geprüften SHA-256 zu. Dadurch sind auch zusätzliche Laufzeitdateien an die Abnahme gebunden. Die Versionsmatrix benennt `game_version`, `game_branch`, `unity_version`, `melonloader_version` und `s1api_version`; `verified` darf erst nach tatsächlicher Prüfung wahr werden.

`tests` enthält T01–T18 jeweils genau einmal. Jeder Eintrag nennt `id`, `status`, `prerequisites`, `steps`, `expected`, `actual`, `mod_version` und `dll_sha256`. Erlaubte Zustände sind `open`, `blocked`, `passed`, `failed` sowie ausschließlich für T18 `pending-archive`. Eine bestandene Spielprüfung benötigt außerdem `execution: target-game`, einen tatsächlichen UTC-Testzeitpunkt und einen konkreten `evidence`-Text. Lokale Kernsimulationen dürfen diese Nachweise nicht ersetzen.

T18 bleibt im internen Bericht immer `pending-archive`. Nach dem Installationslauf aus genau der endgültigen ZIP wird ein **externer** JSON-Bericht erstellt. Er nennt `test_id: T18`, `status: passed`, dieselben Kandidaten-, Versions- und DLL-Kennungen sowie den endgültigen `archive_sha256`. `execution` muss `clean-target-game-installation` sein. Voraussetzungen, Schritte, Erwartung, tatsächliches Ergebnis, Belege und UTC-Testzeitpunkt sind Pflichtfelder.

Unter `tested_checks` werden Erststart, Versions-/Hashabgleich, Bedienung mit Telefon und Speichern/Laden, Multiplayer, Aktualisierung, Datenformatbehandlung und Deinstallation jeweils tatsächlich als bestanden dokumentiert. Die entsprechenden Feldnamen lauten `first_start`, `version_and_hash`, `interaction_phone_save_reload`, `multiplayer`, `upgrade`, `data_format_handling` und `uninstall`. Erst dann kann `verify-release` mit `--archive` und `--t18-report` das letzte formale Gate bestätigen.

Diese Werkzeuge prüfen die Konsistenz eingereichter Belege. Sie beobachten keine Spielhandlung und können einen bewusst unwahren Bericht nicht in echte Testevidenz verwandeln. Eine geänderte DLL ist ein neuer Kandidat. Eine geänderte endgültige ZIP benötigt einen erneuten T18-Lauf. Ein Abschlussbericht wird deshalb niemals nachträglich in die gerade geprüfte ZIP eingefügt.

## Tatsächlich ausgeführte Werkzeugtests

Am 18. Oktober 2026 wurden die **26 Python-Tests** unter `tests/packaging` mit Python 3.12.14 erfolgreich ausgeführt. Sie decken insbesondere Quell- und Kandidatenhashabweichungen, Pfadtraversal, Windows-Namenskollisionen, Symlinks, verbotene bzw. nicht zugeordnete Fremd-DLLs, fehlende Pflichtprüfungen, falsche Artefaktarten, vorzeitig bestandene T18-Angaben und einen abweichenden endgültigen ZIP-Hash ab. Regressionstests sichern außerdem die beidseitige Text-/Pfadprüfung im Entwicklungsarchiv, NUL-Bytes, einen fremden konkurrierenden `.partial`-Stand sowie Manipulationen am Release-Marker, internen Abnahmebericht und der finalen Laufzeit-Dateiliste ab.

Die Laufzeit-Gates verwenden im Test isolierte synthetische Belege und künstliche PE-/CLR-Header. Diese Fixtures sind ausdrücklich keine gebauten Mod-DLLs und keine ausgeführten Spieltests. Sie werden nur in temporären Testverzeichnissen erzeugt und danach gelöscht.

Zum Wiederholen dient der Python-Unittest-Runner mit `discover`, Testverzeichnis `tests/packaging` und Muster `test_*.py`. Die vollständige Aufrufzeile ist `python -B -m unittest discover -s tests/packaging -p "test_*.py" -v`, ausgeführt im Projektordner. Das `-B` vermeidet zusätzliche Python-Bytecode-Dateien im Quellbaum.
