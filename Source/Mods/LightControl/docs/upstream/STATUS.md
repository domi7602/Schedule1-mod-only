# Tatsächlicher Projektstand

Stand: 18. Oktober 2026. Arbeitsversion: **0.1.0-dev.1**.

**Die Umsetzung wurde begonnen. Der fertige Releaseabschluss ist offen.**
Das DEVELOPMENT-Archiv ist ein weiterbearbeitbares Projektpaket. Es enthält
keine produktive Mod-DLL und ist nicht in Schedule I installierbar.

## Fertig ausgearbeiteter Quelltext

`src/LightControl.Core` enthält das eigene Zustands-, Identitäts-, Gruppen-,
Effekt-, Protokoll- und JSON-Datenmodell. `tests/LightControl.Core.Tests` enthält
62 verfasste Verhaltenstests ohne zusätzliche Testbibliothek. Es handelt sich
um C#-Quelltext; ein Compilerlauf und die Ausführung dieser Tests stehen aus.

`src/LightControl.Diagnostics` enthält einen getrennten, nur lesenden
MelonLoader-Diagnosehelfer. Loaderverträge wurden gegen 0.7.3 geprüft. Der
Helfer ist weder gebaut noch im Spiel geladen worden und übernimmt keine
produktive Lampensteuerung.

`tools` enthält den frischen Buildablauf, eine gezielte Windows-Diagnoseabfrage
sowie Paketierung und Prüfung anhand ausdrücklicher Dateilisten und Hashes.
Die Python-Paketlogik wurde mit isolierten Negativ- und Grenzfällen ausgeführt.
Die genaue Anzahl und Ergebnisse stehen in `TESTREPORT.md`. PowerShell und
die Windows-Starter wurden hier nicht ausgeführt.

`assets` enthält acht Sprachfassungen mit jeweils 42 Schlüsseln und
Konfigurationsvorgaben. Die Ressourcen sind strukturell geprüft, aber noch
nicht an eine Telefonoberfläche oder Laufzeitkonfiguration angebunden.

## Arbeitspakete des Auftrags

| Paket | Tatsächlicher Stand | Fehlender Abschluss |
|---|---|---|
| AP-00 | Auftrag gelesen, öffentliche Versionen/Verträge und Referenzbedarf untersucht; Diagnose vorbereitet | Startlog, tatsächlicher Zweig, Referenzinventar und lokale Lampenbeobachtung |
| AP-01 | SDK/Framework/Version festgelegt, Projekte und frischer Buildablauf angelegt | Compiler, konkrete Referenzen, erzeugte DLL und Ladeprobe |
| AP-02 | Registermetadaten und Identitäts-/Lebenszyklusregeln im Kern | Tatsächliche native IDs, Komponenten, Weltlocator und Ereignisanbindung |
| AP-03 | Grundzustand/Effekt/Originaleinfluss und Intensitätsrechnung im Kern | Licht-/Materialadapter und Gameplayvergleich |
| AP-04 | Controls- und Interaktionsgrenzen quellengeprüft | Spielinteraktion mit korrekter Aktion, Ziel, Priorität und Kontext |
| AP-05 | JSON-Schema, Session-/Hostgrenzen und Fehler-Schreibsperre im Kern | Tatsächlicher Savepfad, I/O, atomarer Schreib-/Wiederherstellungsweg und Save-Ladezyklen |
| AP-06 | Immobilien-/Telefonverträge untersucht, Sprachtexte vorbereitet | Alle Lampenfamilien, Räume, native Schaltung und Telefonoberfläche |
| AP-07 | Transportneutrale Protokollregeln und Snapshotbehandlung im Kern; separaten Transportkandidaten belegt | Mod-Erkennung, sichere Absenderbindung, echter Transport und Steamtests |
| AP-08 | Regenbogen/Disco-/Prioritätsregeln im Kern; Konfigurationsressourcen | Gemeinsame Zeitbasis, Laufzeitkonfiguration, Teilnehmererkennung und Rückbau |
| AP-09 | T01–T18 und Performanceziele dokumentiert | Frischer Releasekandidat und sämtliche Spielabnahmen |
| AP-10 | Paketwerkzeug mit Hash-/Allowlist-/Statusprüfungen vorhanden | Exakt abgenommene produktive DLL und vollständig geprüfte Laufzeitabhängigkeiten |
| AP-11 | Verfahren für externe T18-Zuordnung vorbereitet | Installation und Abschlussabnahme aus exakt der endgültigen ZIP |

## Blockierende Eingaben

1. **Buildumgebung:** Hier sind weder `dotnet`, `csc`, `mcs` noch `msbuild`
   vorhanden. Der Abruf des offiziellen .NET SDK 6.0.428 wurde mit HTTP 403
   blockiert. Ein lokaler erfolgreicher Build oder ein verfügbar gemachter
   passender Compiler ist erforderlich. Es wurde keine Ersatz-DLL konstruiert.
2. **Zielinstallation:** Spielbuild, tatsächlich gestarteter Zweig, exakte
   Unity-Version, Loader und installierte S1API-Assembly müssen anhand deiner
   aktuellen Installation bestätigt werden.
3. **Referenzen und Spielzugriff:** Die minimal nötigen Build-DLLs werden aus
   dem Referenzinventar ausgewählt. Lampe/Shader/Interaktion/Save/Netzwerk
   müssen an echten Objekten beobachtet werden; diese Informationen sind
   aus einer API-Funktionsliste allein nicht ableitbar.

## Nächste konkrete Rückmeldung

Zunächst reichen die zwei Dateien, die `tools/COLLECT_DIAGNOSTICS.cmd`
nach deiner Pfad- und Zweigangabe erzeugt:

- `reference-inventory.json`
- `log-excerpt-redacted.txt`

Gib dafür die aktuelle `MelonLoader/Latest.log` ausdrücklich an. Die
Dateiliste enthält Hashes und Versionsmetadaten, keine kopierten DLLs.
Ein vollständiger Spielordner, Save oder zusätzliche Mods sind nicht
erforderlich. Falls das Skript nicht ausführbar ist, genügt zunächst der
bereinigte relevante Startlog plus Angabe des Zweigs; die gezielte
Dateiliste folgt dann manuell nach `LOCAL_TEST_INPUTS.md`.

## Freigabeentscheidung

`tools/integration-status.json` hält die produktive Mod ausdrücklich auf
nicht implementiert. Das lässt Core- und Diagnosearbeit zu, verhindert aber
eine versehentliche Umbenennung dieses Zwischenstands in einen 1.0-Release.
Diese Sperre ist ein technischer Schutz gegen falsche Auslieferung, keine
zusätzliche Benutzerfreigabe. Nach tatsächlicher Integration werden die
Nachweise aktualisiert; nicht bestandene Pflichttests bleiben offen.
