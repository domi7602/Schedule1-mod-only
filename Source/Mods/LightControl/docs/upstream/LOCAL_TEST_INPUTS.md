# Genau benötigte lokale Eingaben

## Nächster Schritt: eine kleine Referenz- und Versionsauskunft

Für die nächste Integration werden zunächst **zwei selbst erzeugte Textdateien** aus deiner tatsächlichen Zielinstallation benötigt. Ein vollständiger Spielordner, komplette Original-Logs oder ein persönlicher Spielstand sind dafür nicht erforderlich.

1. Notiere den tatsächlich gestarteten Steam-Zweig. Die Einstellung ist eine Nutzerangabe und wird nicht aus vorhandenen Dateinamen erraten.
2. Wenn du einen aktuellen Startlog liefern möchtest, starte das Spiel mit deiner gegenwärtigen Installation einmal normal und beende es wieder. Wähle anschließend ausdrücklich die zu diesem Start gehörende `MelonLoader/Latest.log`.
3. Öffne `tools/COLLECT_DIAGNOSTICS.cmd`. Gib den konkreten Spielordner und den Zweignamen ein. Du kannst die Logfrage leer lassen; dann liest das Werkzeug keine Logdatei und die Versionsbestätigung bleibt entsprechend offen.
4. Öffne anschließend `reference-inventory.json` und `log-excerpt-redacted.txt` im neu angelegten Ausgabeordner und prüfe den Inhalt.
5. Lade nur diese beiden Dateien als Rückmeldung hoch. Das Werkzeug lädt selbst nichts hoch.

Die CMD-Datei startet das mitgelieferte PowerShell-Skript ohne Administratoranforderung und ohne Änderung deiner Ausführungsrichtlinie. Falls Windows oder deine Richtlinie es blockiert, sende die konkrete Fehlermeldung. Das Script wird nicht automatisch entsperrt oder mit einer Umgehungsoption gestartet.

### Was der Collector tatsächlich liest

| Auswahl | Umfang |
| --- | --- |
| Ausdrücklich angegebener Spielordner | Nur konventionelle unmittelbare Referenzorte unter genau diesem Ordner. Keine Suche in Steam-Bibliotheken, Benutzerordnern oder anderen Laufwerken. |
| Wurzel des Spielordners | Falls vorhanden: Metadaten zu `GameAssembly.dll` und `UnityPlayer.dll`. |
| `MelonLoader/net6` | Unmittelbar enthaltene DLL-Dateien. |
| `MelonLoader/Il2CppAssemblies` | Unmittelbar enthaltene DLL-Dateien zur späteren gezielten Auswahl der Buildreferenzen. |
| `Mods`, `Plugins` und `UserLibs` | Nur DLL-Dateinamen, die mit `S1API` beginnen. |
| Explizit ausgewählte Logdatei | Höchstens die ersten 5.000 Zeilen; übernommen werden nur erkannte Produktbezeichnungen, direkt zugehörige numerische Versionstoken und Mono-/IL2CPP-Kennzeichen. |

Von DLLs werden relative Dateinamen, Größe, SHA-256 und lesbare Assembly-/Dateiversionen erfasst. Der Collector lädt keine Assembly zur Ausführung und ruft keine Spielmethode auf. Native oder nicht lesbare Dateien werden entsprechend gekennzeichnet. Verzeichnislinks und Dateilinks werden nicht verfolgt.

Der Logauszug wird aus erlaubten Fakten neu zusammengesetzt. Er enthält **keine kopierten Originalzeilen**, keine absoluten Installationspfade und keine automatisch übernommenen Benutzer- oder Sitzungsangaben. Nicht erkannte Informationen gelten als fehlend. Eine gefundene Versionszahl beweist weder die vollständige Kompatibilität noch ein erfolgreiches Mod-Verhalten.

Der Ausgabeordner muss neu sein und außerhalb der Spielinstallation liegen. Das Spielverzeichnis bleibt unverändert. Es werden keine DLLs, Spielstände, Konfigurationen oder Original-Logs in diesen Diagnoseordner kopiert.

### Argumente für den direkten Aufruf

| Argument | Bedeutung |
| --- | --- |
| `-GamePath` | Konkreter Spielordner. |
| `-Branch` | Tatsächlich gestarteter kurzer Zweigname. |
| `-LogPath` | Optionaler, ausdrücklich ausgewählter aktueller Startlog. Ohne Angabe erfolgt kein Logzugriff. |
| `-OutputDirectory` | Optionaler neuer Ausgabeordner außerhalb des Spiels. |
| `-Interactive` | Fragt fehlende Werte einzeln ab; wird vom Doppelklick-Starter verwendet. |

Die genaue Syntax ist in `tools/collect-diagnostics.ps1` sichtbar. Das Script ist als lesendes Hilfswerkzeug implementiert, wurde in diesem Erstellungsumfeld jedoch noch nicht mit Windows PowerShell 5.1 ausgeführt. Ein Windows-Lauf und die dabei tatsächlich erkannten Felder bleiben zu bestätigen.

## Danach gezielt ausgewählte Dateien

Erst nach Sichtung der Referenzliste wird eine kleine, konkrete Dateiauswahl für den nächsten Integrationsschritt angefragt. Dabei werden benötigte Typen, Referenzversion, Dateiname und Zweck benannt. Eine generierte Spiel-/Interop-Assembly ist eine Entwicklungsreferenz; sie gehört nicht in die Auslieferungs-ZIP.

Für den bereits abgegrenzten Diagnosebuild ist nur die tatsächlich verwendete `MelonLoader.dll` als externe Buildreferenz vorgesehen. Die vollständige produktive Mod benötigt später weitere nachgewiesene Verträge. Diese Liste wird nicht durch Vermutungen über verfügbare Spielklassen ersetzt.

Wenn ein Dateitransfer nicht erforderlich oder nicht gewünscht ist, können gezielte Metadatenauszüge bzw. die Ergebnisse eines lokalen Builds ausreichen. Das wird anhand des nächsten konkreten fehlenden Nachweises entschieden.

## Rückmeldung von Build- und Spieltests

| Art des Nachweises | Kleine zweckmäßige Rückmeldung |
| --- | --- |
| Buildfehler | Gewählter Modus, erster konkreter Compiler-/Referenzfehler einschließlich Fehlercode und die direkt zugehörigen Meldungen. Persönliche absolute Pfade vor dem Senden entfernen. |
| Erfolgreicher Core-Build | `build-record.json` mit `artifact_kind: core-only`, Ergebnis des tatsächlich gestarteten Konsolentests und bei Bedarf die kleine Testzusammenfassung. Dies ist kein Spieltest. |
| Erfolgreicher Diagnosebuild | Buildkennung, Loader-Referenzversion, DLL-Hash und anschließend die gezielt vereinbarte Beobachtung im Spiel. Ein Build allein bestätigt noch keine Laufzeitfunktion. |
| Spätere Funktionsprüfung | Testnummer T01–T17, Kandidaten-/DLL-Hash, tatsächliche Schritte und beobachtetes Ergebnis; nur die relevanten redigierten Logstellen. |
| Visuelle Prüfung | Gezielt vereinbarter Screenshot oder kurzer Clip derselben Szene mit nachvollziehbaren Ausgangseinstellungen. |
| Abschlussprüfung T18 | Bericht außerhalb der ZIP mit deren endgültigem SHA-256 und den tatsächlichen Installations-, Funktions-, Update- und Deinstallationsprüfungen. |

Bis eine produktive Mod mit den nötigen Integrationen vorliegt, bleiben die Spielprüfungen T01–T18 offen. Simulierte Kerntests, erfolgreiche Paketprüfungen oder die bloße Existenz einer DLL dürfen nicht als bestandene Spielabnahme eingetragen werden.
