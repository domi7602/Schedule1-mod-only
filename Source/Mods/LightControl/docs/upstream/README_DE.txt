LIGHTCONTROL 0.1.0-dev.1 – ENTWICKLUNGSSTAND
Stand: 18. Oktober 2026

DIESES ARCHIV IST NOCH KEINE INSTALLIERBARE LICHTSTEUERUNGSMOD.
Es enthält eigenen C#-Quelltext, Buildwerkzeuge, Diagnosequelltext,
Sprachressourcen und nachvollziehbare Nachweise. Es enthält keine erzeugte
LightControl.dll. Kopiere dieses Projektarchiv nicht in den Mods-Ordner.

ZIEL
Eine eigene Lichtsteuerung für Schedule I, Windows/IL2CPP, mit Farben,
Helligkeit, Telefon-App, Speicherung, Multiplayer und Bereichsdisco.
Der vollständige Auftrag bleibt in docs/IMPLEMENTATION_BRIEF.txt erhalten.
Die Zielversion 1.0.0 wird erst nach den dort geforderten Prüfungen vergeben.

JETZT VORHANDEN
- Eigenständiger .NET-6-Kern für Lampenzustände, Instanzidentitäten,
  Gruppenänderungen, Sitzungstrennung, Effekte und Protokolldaten.
- Versionierte JSON-Datenverwaltung mit Schreibsperre nach Ladefehlern.
- C#-Konsolenprüfungen für fachliche Randfälle; noch nicht ausgeführt.
- Nur lesender MelonLoader-Diagnosehelfer als Quelltext; unkompiliert.
- Prüffähiger Build-/Paketablauf, ausdrückliche Release-Sperre.
- Acht Sprachdateien mit 42 Schlüsseln; sprachliche/UI-Abnahme offen.
- Versionsgebundene Quellbelege, Anforderungskatalog und Testplan.

AKTUELLE GRENZE
Die Erstellungsumgebung enthält weder .NET-SDK noch Spielinstallation.
Der Versuch, das offizielle SDK herunterzuladen, wurde mit HTTP 403
abgewiesen. Das ist kein erfolgreicher C#-Build. Die Lampenadapter,
native Interaktion, Telefonoberfläche, Save-Anbindung und der reale
Multiplayertransport sind noch zu implementieren. Die Kernklassen
ersetzen diese Integrationen nicht.

DEIN NÄCHSTER SCHRITT
1. Entpacke das Archiv in einen eigenen Entwicklungsordner.
2. Starte Schedule I mit dem aktuell eingesetzten MelonLoader und S1API
   einmal und beende das Spiel anschließend.
3. Öffne tools/COLLECT_DIAGNOSTICS.cmd, gib den konkreten Spielordner und
   den tatsächlich verwendeten Steam-Zweig an. Das Skript liest gezielt
   Versionshinweise und eine Referenzdateiliste und kopiert keine Spiel-DLLs.
4. Prüfe die erzeugten Text-/JSON-Dateien vor der Weitergabe. Sende das
   kleine Diagnoseergebnis zurück. Falls Windows das Skript aufgrund
   seiner Richtlinie sperrt, gib stattdessen den aktuellen Startlog und
   eine Dateiliste der relevanten Ordner weiter; siehe docs/LOCAL_TEST_INPUTS.md.

Für diesen ersten Schritt ist das Diagnose-Modul nicht zu bauen oder zu
installieren. Erst nach Sichtung der Versions- und Referenzliste werden
die tatsächlich benötigten einzelnen Dateien festgelegt.

KERN LOKAL BAUEN UND PRÜFEN
Erforderlich ist das .NET SDK 6.0.428; die reine .NET-Runtime genügt nicht.
Offizielle Bezugsquelle: https://dotnet.microsoft.com/en-us/download/dotnet/6.0
Nach Installation startet tools/BUILD_CORE.cmd den frischen Kernbuild und
den C#-Prüflauf. Ein tatsächlich erfolgreicher Lauf erzeugt einen eigenen
Buildbericht. Dies baut noch keine produktive LightControl-Mod.
Details, Argumente und alternative Dotnet-Pfade: docs/BUILD.md.

DIAGNOSE-MODUL
Nur für einen späteren gezielten Integrationstest:
src/LightControl.Diagnostics/LightControl.Diagnostics.csproj benötigt die
echte net6-MelonLoader.dll aus MelonLoader 0.7.3. Ein erfolgreicher Build
erzeugt LightControl.Diagnostics.dll. Die Datei ist ein Beobachtungswerkzeug,
keine Lichtsteuerung. Anleitung und Grenzen: docs/DIAGNOSTICS.md.

PAKETSTATUS UND PRÜFUNGEN
docs/STATUS.md beschreibt den tatsächlichen Bearbeitungsstand.
docs/TESTREPORT.md trennt ausgeführte lokale Prüfungen von offenen C#-
und Spieltests. docs/ACCEPTANCE.md enthält T01 bis T18.
docs/API_EVIDENCE.md enthält Quellen und fehlende Laufzeitnachweise.
docs/COMPATIBILITY.md unterscheidet Zielversionen und tatsächlich Geprüftes.
docs/REQUIREMENTS.md ordnet jede Pflichtanforderung einem Arbeitsteil zu.

ABHÄNGIGKEITEN UND AUSLIEFERUNG
MelonLoader und S1API werden später separat installiert. Spiel- und
Interop-Assemblies dienen nur als Entwicklungsreferenzen und werden nicht
weitergegeben. Dieses Entwicklungsarchiv enthält keine solchen Dateien.
Ein Transportkandidat ist dokumentiert, aber noch keine Laufzeitabhängigkeit.

LIZENZ
Der neu erstellte Projektquelltext steht unter der beigefügten MIT-Lizenz.
Es wurden keine Binärdateien oder Assets der Referenzmod übernommen.
Für extern installierte Voraussetzungen gelten deren eigene Lizenzen.
