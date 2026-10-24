# WorldLook 0.2.0 — Build-Paket

> **WOLKENBERECHNUNG AUF NATIVEM SKYBOX-PASS WIEDERHERGESTELLT – installiert am 22.10.2026 um 13:40 UTC:** Aktuelle DLL `aebd2b81…`, Bundle `d31739f1…`. Wolken, Tageszeitpalette, Sonne, Mond und Sterne sind wieder im Shader enthalten; UnityCG/`UnityObjectToClipPos`, ungetaggter Pass und fehlender manueller Far-Z-Override bleiben erhalten. **Erster Besitzer-Screenshot zeigt blauen Himmel und Wolken ohne vorherige orangefarbene Szeneumrisse; aktueller Kandidat im Log bestätigt.** Bewegungsnachbilder, Optikabnahme und vollständige Feature-/Lifecycleprüfung bleiben offen. Wasser bleibt aus, direkte Lichtanpassung 0. [Aktueller Lauf und Grenzen](docs/cloud-restore-20261022-133344/runtime-20261022-144019/README.md), [Build und Backup](docs/cloud-restore-20261022-133344/README.md). Kein Spielstart durch den Agenten; `inGameVerified=false`.

> **Historischer Reparatur-Build vom 22.10.2026, 09:19 UTC:** Der vorangegangene vollständige Neubau mit Unity 2022.3.62f3 ist im [Buildnachweis](docs/full-build-20261022-091929/README.md) dokumentiert. Dieser Kandidat wurde später getestet, der Orange-Fehler blieb bestehen. Er und der ursprüngliche Skyshader sind vor dem jetzigen Diagnosebuild gesichert. [Reparaturhistorie](docs/repair-2026-10-22/README.md) und [Bugreport](WorldLook-Bugreport.md) bleiben erhalten. Die alte isolierte DLL unter `docs/repair-2026-10-22/compile-only` bleibt ausdrücklich nicht zur Installation geeignet.

Dieses Paket enthält das eigenständige WorldLook-Projekt für deinen lokalen Windows-Build. Ziel ist die natürliche Bildrichtung aus Mockup 3: weichere Wolken, zurückhaltendes Tageslicht und dazu passendes Wasser. Der erste Stand ist ein **Prototyp für den anschließenden Test im Spiel**.

**Start:** ZIP vollständig in einen eigenen Ordner entpacken und `BUILD.cmd` doppelklicken. Bei einer üblichen Steam- und Unity-Hub-Installation findet das Skript die Pfade selbst. Nach einem erfolgreichen Durchlauf liegt die neue Mod unter **`dist/WorldLook.dll`**.

Die ursprüngliche Vorbereitungsumgebung konnte nur statisch prüfen. Später gelangen ein vollständiger lokaler Build und mehrere separate C#-Builds, die Spieltests zeigten jedoch Grafik- und Wiederherstellungsfehler. Nach den Quellreparaturen gelang nun ein neuer vollständiger Unity-/DLL-Build; dessen Spieltest steht noch aus. `docs/VALIDIERUNG.md`, der Bugreport und die aktuellen Nachweise trennen diese Stände ausdrücklich.

## Voraussetzungen

| Bestandteil | Erwarteter Stand |
| --- | --- |
| Spiel | Windows-Version von Schedule I, IL2CPP; vorbereitet anhand deiner Daten aus 0.4.7f12 |
| MelonLoader | Deine vorhandene IL2CPP-Installation; Ausgangsdaten: 0.7.3, Bibliotheken unter `MelonLoader/net6` |
| S1 API | `Mods/S1API.Il2Cpp.MelonLoader.dll`, Dateiversion 3.2.1.0 |
| Unity Editor | **2022.3.62f3**, im lokalen Build nach ausdrücklicher Freigabe gepinnt; Spielruntime weiterhin 2022.3.62f2; funktionierende Lizenz und Windows-Buildunterstützung erforderlich |
| .NET | Ein installiertes **SDK ab Version 6**, nicht nur eine Runtime; das Projekt zielt auf `net6.0` |
| Windows PowerShell | 5.1 oder neuer; `BUILD.cmd` verwendet die Windows-PowerShell |

Starte das Spiel mindestens einmal mit MelonLoader, damit `MelonLoader/Il2CppAssemblies` vollständig erzeugt wird. Das Paket referenziert diese Dateien direkt aus deiner Installation. Spiel-, Unity- und S1API-Bibliotheken werden nicht mitgeliefert oder in `dist` kopiert.

Der erste Unity-Import benötigt Zugriff auf die Unity-Paketquellen. Die Projektvorgabe ist URP/Core 14.0.11; der Buildbericht hält die tatsächlich von Unity aufgelösten Paketversionen fest und verlangt URP 14.x. Falls dem installierten .NET SDK die Referenzpakete für `net6.0` fehlen, stellt der Restore sie über die offizielle Quelle nuget.org bereit. Das Skript installiert weder einen Unity Editor noch ein SDK automatisch.

## Bauen

1. Entpacke alles beispielsweise nach `C:\ModBuilds\WorldLook`. Starte den Build aus diesem eigenen Ordner. Die ZIP muss nicht über dein bisheriges Mod-Repository kopiert werden.
2. Schließe einen eventuell geöffneten Unity Editor, der denselben enthaltenen `unity`-Projektordner verwendet.
3. Doppelklicke `BUILD.cmd` und warte auf **BUILD ERFOLGREICH**. Der erste Unity-Paketimport kann einige Zeit dauern.
4. Das Skript erstellt zuerst das Shaderbundle, prüft den neuen Bundle-Hash und baut anschließend die DLL mit den Spielreferenzen.
5. Das fertige Ergebnis findest du in `dist`. Nur `WorldLook.dll` ist für die Installation der Mod nötig. PDB und JSON-Dateien sind Diagnose-/Buildnachweise.

Es gibt keinen manuellen Schritt zum Erstellen oder Kopieren des Shaderbundles. Das Bundle wird als Ressource `WorldLook.Shaders.bundle` in die neue DLL eingebettet. Eine alte Bundle-Datei aus dem Ausgangspaket wird nicht verwendet.

### Wenn die Pfade nicht automatisch gefunden werden

Öffne PowerShell im entpackten Ordner und verwende:

```powershell
powershell -NoProfile -ExecutionPolicy Bypass -File .\build\Build.ps1 -GamePath "D:\SteamLibrary\steamapps\common\Schedule I" -UnityPath "D:\Unity\Hub\Editor\2022.3.62f3\Editor\Unity.exe"
```

Die Pfade müssen auf deine tatsächlichen Installationen zeigen. Alternativ liest das Skript die Umgebungsvariablen `SCHEDULE_I_GAME_DIR` und `UNITY_EDITOR_PATH`. Mit `-DotNetPath` kannst du eine bestimmte `dotnet.exe` auswählen.

Für eine Vorprüfung ohne Kompilierung kannst du `-CheckOnly` ergänzen. Diese Prüfung bestätigt vorhandene Dateien und SDK-Erreichbarkeit; sie bestätigt weder die Editorlizenz noch erfolgreiche Shaderkompilierung oder Laufzeitkompatibilität.

Für den vollständigen Build bitte den Starter verwenden. Ein einzelnes `dotnet build` erstellt das Shaderbundle nicht und kann einen vorhandenen Bundle-Stand wiederverwenden.

## Was dieser Stand verändert

### Himmel und Wolken

Der neue Shader enthält zwei weiche prozedurale Wolkenschichten mit unterschiedlicher Struktur, Bewegung, heller Oberseite und gedämpfter Unterseite. Es handelt sich um eine Darstellung auf dem Himmel, nicht um einen volumetrischen Wolken-Raymarcher. Das vorhandene Stadtmodell und seine Materialien bleiben die Grundlage der Welt.

Die Runtime bindet den Shader nur an zwei ausdrücklich bekannte Ziele: eine passende `RenderSettings.skybox` und die vom nativen `DayNightController` referenzierten Sky-Materialslots. Das ursprüngliche Material muss den belegten Shadernamen `Custom/ScheduleOneSky` und dessen zwölf erwartete Properties besitzen. Andere Wolkenrenderer werden nicht gesucht oder abgeschaltet.

Vor dem Austausch entsteht eine eigene native Sicherungskopie. Abschalten bevorzugt das gültige Original und verwendet bei dessen Verlust das gültige Backup, aber niemals absichtlich einen leeren Slot. Unbestätigte Freigaben bleiben gespeichert; fremde Übernahmen bleiben innerhalb derselben Main-Szene auch über F7 gesperrt. Der tatsächlich beobachtete Shader Graph `Shader Graphs/Schedule1Sky` erfüllt den zwölfteiligen Legacy-Vertrag nicht und wird nicht blind übernommen oder abgeschaltet.

Tageszeit und Sonnenrichtung kommen aus dem Spiel. Wetterwerte werden über `S1API.Weather.WeatherManager.Current` gelesen, ohne Wetter auszulösen oder umzuschalten. Ist dieser Snapshot noch nicht verfügbar, wird vorübergehend eine moderate Wolkenvorgabe verwendet. Sonnen-, Mond- und Sternparameter sowie die vorhandenen Sonnen-/Mondtexturen werden zwischen Original und eigenen Materialkopien weitergereicht. Die visuelle Interpretation dieser Parameter muss im Spiel geprüft werden.

### Tageslicht

Eine kleine, einstellbare Anpassung nimmt tagsüber etwas Farbsättigung aus der Sonne und dämpft bei stärkerer Bedeckung direkten Kontrast. Bei nativem Trilight-Ambientmodus wird die Umgebungsbeleuchtung entsprechend leicht abgestimmt. Der ältere GoldenHour-Schreiber wird nicht zusätzlich gestartet. Seine Quellen bleiben zur Nachvollziehbarkeit enthalten.

Die Anpassung merkt sich ihren letzten eigenen Schreibwert, übernimmt erkennbare native Änderungen als neue Ausgangswerte und stellt beim Abschalten nur noch selbst kontrollierte Werte zurück.

### Wasser

Der vorhandene Vier-Wellen-Ansatz bleibt erhalten. Wind verändert die Amplitude, ohne den bereits verstrichenen Zeitwert zu skalieren. Kleine Wellen werden in der Ferne gedämpft. Die Wasserkomposition verwendet vormultiplizierte Farbe, damit der Reflexionsanteil nicht nochmals pauschal mit der Oberflächen-Alpha abgeschwächt wird. Tiefen-/Uferauswertung und gebrochene Bildkoordinaten werden geprüft.

Der Reparaturquellstand erstellt keine eigene Spiegelkamera, kein Spiegel-Renderziel und keinen zusätzlichen Kamerarender. Ein schwacher Blickwinkelanteil verwendet nur aktuelle Himmels-/Horizontfarben und kann keine Gebäude oder Wolken spiegeln. Die Mindestdeckung des Wasserkörpers wird unabhängig davon eingestellt; fehlt ein gültiger Bodentiefenwert, bleibt ein beleuchteter Tiefwasseranteil statt einer verschwundenen Oberfläche. Darstellung und Abstimmung müssen nach dem vollständigen Neubau im Spiel geprüft werden.

Die benötigten Depth-/Opaque-Anforderungen werden anhand der aktiven URP-/Kameraeinstellungen geprüft. Das belegt die Konfiguration, nicht den Inhalt eines tatsächlich gerenderten Texturbildes. Kann der Referenzpfad nicht verwendet werden, bleibt der vorhandene native Wasserpfad verfügbar. Sonnenreflexe berücksichtigen den URP-Hauptlichtschatten, soweit dieser vom Spiel bereitgestellt wird.

## Installation nach erfolgreichem Build

1. Spiel schließen.
2. Die bisherige `WorldLook.dll` außerhalb des `Mods`-Ordners sichern.
3. **`dist/WorldLook.dll` nach `Schedule I/Mods/WorldLook.dll` kopieren.**
4. Keine zweite WorldLook-Version parallel laden. Alte separate GoldenHour-/WaterShader-Module, die dieselben Werte bearbeiten, dürfen beim Vergleich nicht zusätzlich aktiv sein.
5. Spiel starten und einen Spielstand laden.

Der Build installiert nichts automatisch. Zurück zur vorherigen Version geht es durch Schließen des Spiels und Wiederherstellen der gesicherten DLL.

Für die Weitergabe reicht die gebaute DLL. Beim Laden versucht WorldLook, das eingebettete Bundle direkt aus dem Speicher zu öffnen. Falls die erzeugten IL2CPP-Signaturen das nicht unterstützen, legt es seine eigenen, per SHA-256 geprüften Bundle-Bytes unter `UserData/WorldLook/ShaderCache` ab. Es muss kein separates Bundle manuell verteilt werden; Laufzeit-Konfiguration und Cache-Dateien können dennoch entstehen.

## Bedienung und Konfiguration

| Tastenkombination | Funktion |
| --- | --- |
| Strg + Umschalt + F7 | Gesamten WorldLook-Look für die aktuelle Sitzung an-/abschalten |
| Strg + Umschalt + F8 | Vorhandenen ausführlichen manuellen Grafik-Probe starten |
| Strg + Umschalt + F9 | Gezielten Status nach `UserData/WorldLook/status.json` schreiben |

Die Tastenkombinationen werden bei Texteingabe oder fehlendem Anwendungsfokus nicht verarbeitet. F7 verändert die gespeicherte Konfiguration nicht.

Beim ersten Start wird **`UserData/WorldLook/WorldLook.json`** angelegt. Änderungen werden beim nächsten Spielstart gelesen. `docs/WorldLook.example.json` zeigt die Ausgangswerte.

| Einstellung | Bedeutung / Bereich |
| --- | --- |
| `Enabled` | Gesamten Look beim Start aktivieren |
| `WaterEnabled` | Wasser separat aktivieren |
| `Clouds.Enabled` | Himmel und seine Tageslichtabstimmung aktivieren |
| `Clouds.Coverage` | Grundbedeckung, Ausgangswert 0,55; Wetter verschiebt die effektive Bedeckung |
| `Clouds.Density` | Wolkendichte, Ausgangswert 0,75; effektiver Wert wird begrenzt |
| `Clouds.Scale` | Strukturmaßstab, 0,25 bis 4; größere Werte erzeugen kleinere sichtbare Strukturen |
| `Clouds.Speed` | Grundgeschwindigkeit, 0 bis 3; Ausgangswert 0,35 |
| `Clouds.Opacity` | Sichtbarkeit der Wolken, 0 bis 1; Ausgangswert 0,88 |
| `Clouds.LightingStrength` | Tageslichtanpassung, 0 bis 1; Ausgangswert 0,35; 0 lässt diese Anpassung aus |

Die neue Konfiguration ist unabhängig von älteren GoldenHour-Einstellungen. Eine beschädigte oder nicht lesbare JSON-Konfiguration wird nicht überschrieben; für diese Sitzung gelten dann Standardwerte und eine Logmeldung erklärt den Grund.

## Erster Vergleich im Spiel

Vergleiche an derselben Stelle und möglichst zur selben Uhrzeit mit F7: einmal der Blick über die Stadt und einmal Hafen/Wasser. Prüfe anschließend Sonnenuntergang, Nacht, einen Wetterwechsel und einen Wechsel zurück ins Hauptmenü mit erneutem Laden. Achte auf doppelte Wolken, schwarze/pinke Flächen, unpassende Sonnen-/Mondgröße, flackernde Fernwellen und Veränderungen nach dem Abschalten.

Schreibe mit F9 einen Status, während der Look aktiv ist. Hilfreich für die nächste Abstimmung sind dieser Status, `MelonLoader/Latest.log` und zwei Vergleichsscreenshots. Ein erfolgreicher Materialtausch oder ein Reflexions-Framezähler bestätigt allein noch kein korrektes sichtbares Bild.

Nach Installation eines frisch vollständig gebauten Reparaturstands muss F9 bei Wasser `reflectionMode=environment-only` und `ownedReflectionCameras=0` zeigen; die alten Reflexionszähler bleiben null. Beim Himmel zusätzlich `skyPendingRestores` und den tatsächlich gebundenen Skybox-Namen prüfen. Ein optisch sauberer Zustand mit leerer Skybox gilt nicht als erfolgreicher Restore.

## Automatisierte Reparaturprüfungen

Die zusätzliche Konsolen-Testsuite benötigt .NET SDK 8 und referenziert die tatsächlichen Unity-freien Produktionsrichtlinien, keine erfundenen Unity-APIs. Sie prüft Besitz-/Restore-Übergänge mit Test-Delegates, Quellverträge und ein numerisches Wasser-Mischmodell. Das ist kein Shadercompiler oder Spieltest. Aufruf aus dem Projektordner: `dotnet run --project tests/WorldLook.Repair.Tests.csproj -c Release -- .`

## Fehler finden

- **Spiel nicht gefunden:** einmal mit MelonLoader starten und anschließend `-GamePath` angeben.
- **S1API fehlt oder falsche Version:** Dateiversion der installierten `S1API.Il2Cpp.MelonLoader.dll` prüfen; Ziel dieses Pakets ist 3.2.1.
- **Falscher Unity Editor:** im lokal freigegebenen Stand exakt 2022.3.62f3 auswählen. Der Shaderbuilder bricht bei einer anderen Version ab. Die Spielruntime wird dadurch nicht geändert.
- **Unity-Import/-Build scheitert:** Log unter `build/logs/<Zeitstempel>/unity-build.log` öffnen. Paket-, Lizenz- und Shaderfehler stehen dort.
- **C#-Build scheitert:** `dotnet-build.log` im selben Logordner prüfen. Insbesondere nach einem Spielupdate können sich Interop-Signaturen ändern.
- **Kein neuer Look im Spiel:** F9-Status und MelonLoader-Log prüfen. Bei einem unbekannten Skyshader oder fehlenden Shaderassets bleibt die native Darstellung aktiv.
- **Build abgebrochen, aber `dist` enthält noch eine DLL:** Diese kann vom letzten erfolgreichen Build stammen. Nur ein neuer Durchlauf mit `BUILD ERFOLGREICH` bestätigt ein frisches Ergebnis.

## Paketstruktur

| Pfad | Inhalt |
| --- | --- |
| `BUILD.cmd` | Start per Doppelklick |
| `build/Build.ps1` | Pfadprüfung, Unity-Build, .NET-Build und Buildnachweise |
| `WorldLook.csproj` | Eigenständiges Mod-Projekt mit eingebetteter Shaderressource |
| `src` | Mod-, Himmel-, Wasser- und Diagnosequellen |
| `unity` | Minimales Shaderprojekt und Editor-Builder |
| `assets` | Beim Build erzeugtes Bundle und Shaderbericht |
| `dist` | Wird nach erfolgreichem Build erzeugt |
| `docs` | Konfigurationsbeispiel, Änderungen und Prüfstatus |

Die ergänzten Hilfsfunktionen sind im Projekt enthalten. Ein externes `Shared.dll` oder ein übergeordnetes Mod-Repository ist nicht erforderlich. Die ursprüngliche ZIP wurde für dieses Paket nicht verändert.
