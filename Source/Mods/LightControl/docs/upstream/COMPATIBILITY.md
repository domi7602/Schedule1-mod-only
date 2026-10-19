# LightControl – Kompatibilität und Referenzmatrix

Stand: 18. Oktober 2026.

**Es liegt noch keine im Zielspiel abgenommene LightControl-Version 1.0.0 vor.** Die folgende Matrix beschreibt bestätigte Quellen, die gewählte Buildgrundlage und fehlende Laufzeitbelege. Ein erfolgreiches Kompilieren allein bestätigt keine Spielfunktion.

## 1. Versionsmatrix

| Bestandteil | Vorgabe / recherchierter Stand | Tatsächlich bestätigt | Noch offen |
|---|---|---|---|
| Betriebssystem | Windows x64 | Auslieferungsziel aus Auftrag | Start der konkreten DLL auf Nutzerrechner. |
| Schedule I | IL2CPP-Zweig `public` | S1API-Releasebeschreibung benennt diesen Zweig | Tatsächlich gestarteter Zweig und Build des Nutzers. |
| Spielversion | S1API 3.2.1 zielt auf 0.4.7; letzter genannter Releasepass 0.4.7f12 | Primärer S1API-Releasebeleg | LightControl-Test auf genau diesem bzw. dem abweichenden Nutzerbuild. |
| Unity | Aus Zielinstallation zu bestimmen | Keine aktuelle lokale Spielinstallation vorhanden | Exakte Unity-Version, Renderpipeline, tatsächliche Materialshader. |
| MelonLoader | 0.7.3; Quellcommit `982ed9981ac5b0619648a2e79bcc02fd8861db33` | Öffentliche Callback-, Attribut-, Logging- und Pfadverträge; öffentliches net6.0-Referenzpaket | Laden der Diagnose und späteren Mod im Spiel. |
| S1API Forked | 3.2.1; Quellcommit `a4cbdd9e0ec54895e64fd6be0e91874ba6649c9b` | Öffentliche relevante API-Verträge gegen Tagquelle geprüft | Laufzeitassembly der Installation; jede konkrete LightControl-Anbindung. |
| Zielframework | net6.0 für IL2CPP-Arbeit | S1API-IL2CPP-Konfiguration und MelonLoader-Net6-Paket belegt | Konkrete Buildwerkzeuge und Ergebnisdateien siehe Buildbericht. |
| S1API-NuGet | S1API.Forked 3.2.1, netstandard2.1 | Paketmetadaten und MonoMelon-Veröffentlichungsworkflow | Kein vollständiges IL2CPP-Spielreferenzbundle. |
| SteamNetworkLib | 1.6.0 als späterer Kandidat | IL2CPP-Release und konkrete öffentliche Member quellengeprüft | Kein aktueller Pflichtbestandteil; Integration und Steamtests offen. |

Quellen: [S1API 3.2.1 Release](https://github.com/ifBars/S1API/releases/tag/v3.2.1), [S1API-Projektdatei](https://github.com/ifBars/S1API/blob/a4cbdd9e0ec54895e64fd6be0e91874ba6649c9b/S1API/S1API.csproj), [MelonLoader 0.7.3](https://github.com/LavaGang/MelonLoader/releases/tag/v0.7.3), [MelonLoader-Paket](https://www.nuget.org/packages/LavaGang.MelonLoader/0.7.3), [S1API-Paket](https://www.nuget.org/packages/S1API.Forked/3.2.1).

## 2. Entwicklungsreferenzen und Laufzeitbereitstellung

| Datei / Komponente | Verwendung | Bereitstellung | In LightControl-Installationspaket? |
|---|---|---|---|
| MelonLoader.dll 0.7.3, net6 | Compile-Referenz und Loader-API | Separat installierter MelonLoader | Nein. |
| IL2CPP-S1API-Assembly 3.2.1 | Künftige typisierte Standardintegrationen; Diagnose kann installierte Metadaten lesen | Offizielle S1API-ZIP separat installieren | Nein. |
| S1APILoader.MelonLoader.dll | Auswahl des passenden API-Runtimes durch S1API-Paket | Mit der separaten S1API-Installation | Nein. |
| Generierte Unity-/Spiel-/FishNet-/Steamworks-Interop-DLLs | Künftige konkrete Adapter kompilieren | Passende Nutzerinstallation oder ausdrücklich freigegebene Referenzen | Nein. |
| .NET-Standardbibliotheken | Verwalteter Kern und Diagnose | Geeignete .NET-Laufzeit des Loaders | Nein. |
| Eigene LightControl-Kerndatei | Unabhängig testbare Zustandsregeln | Eigener Build; nur mitliefern, wenn der konkrete Mod-Build sie referenziert | Nur bei tatsächlicher Runtime-Abhängigkeit. |
| Eigene Diagnose-DLL | Begrenzte technische Beobachtung | Eigenes, deutlich gekennzeichnetes Diagnosepaket | Ausschließlich im Diagnosepaket, kein Bestandteil des späteren 1.0.0-Releasepakets. |
| SteamNetworkLib-IL2Cpp.dll | Geprüfter Transportkandidat | Erst nach tatsächlich gewählter Integration eindeutig festlegen | Derzeit nein. |

S1API referenziert Newtonsoft.Json 13.0.2; MelonLoader 0.7.3 deklariert Newtonsoft.Json 13.0.4. Daraus folgt keine zusätzliche LightControl-Bündelung. Eine spätere direkte Verwendung muss gegen die tatsächlich vorhandene Loader-Assembly aufgelöst und im Abhängigkeitsbericht dokumentiert werden. Kein gesamter Ausgabeordner wird ungeprüft in die ZIP kopiert. Quellen: die oben verlinkte S1API-Projektdatei und [MelonLoader-Projektdatei](https://github.com/LavaGang/MelonLoader/blob/982ed9981ac5b0619648a2e79bcc02fd8861db33/MelonLoader/MelonLoader.csproj).

## 3. Aktuelle Funktionsgrenzen

Die unabhängige Kernlogik und eine technische Diagnose dürfen als Entwicklungszwischenstand ausgeliefert werden. Damit sind direkte Lichtbedienung, native Ein/Aus-Schaltung, Shaderemission, Telefon-App, persistente Lampeninstanzzuordnung, Immobilien-/Raumsteuerung oder Multiplayer noch nicht im Spiel nachgewiesen.

Der geplante Diagnose-Build verändert keine Lampenfarben und ist keine funktionsreduzierte Ausgabe des geforderten Lichtsteuerungsreleases. Er soll fehlende technische Belege für die weitere Implementierung liefern. Ob alle vorgesehenen Diagnoseabfragen auf dem konkreten generierten IL2CPP-Oberflächenstand funktionieren, muss ebenfalls beobachtet werden.

Eine Unity-Engineversion aus einer früheren Unterhaltung wird nicht ungeprüft als aktuelle Zielversion übernommen. Ebenso wird aus einer vorhandenen Pipeline-Assembly nicht auf die tatsächlich aktive Renderpipeline geschlossen. Echtzeit-Lichtwerte und Materialemission sind getrennte Integrationen; gebackene Beleuchtung muss separat bewertet werden. Grundlagen: [Unity 2022.3 Light](https://docs.unity3d.com/2022.3/Documentation/ScriptReference/Light.html), [Mixed Lights](https://docs.unity3d.com/2022.3/Documentation/Manual/LightMode-Mixed.html).

## 4. Nächste kleine Belegpakete

1. **Zielsystem:** Relevanter Anfang des aktuellen `MelonLoader/Latest.log` mit Spiel-, Loader-, Unity-, API-Version sowie Angabe des tatsächlich gestarteten Steamzweigs. Private Pfade und Teilnehmerinformationen können entfernt werden.
2. **Ladeprobe:** Diagnose-DLL installieren, einen Testspielstand laden, ins Menü zurückkehren und denselben Save erneut laden. Den Diagnosebericht und relevante Fehlermeldungen zurückgeben. Dies ist zunächst eine Loader-/Lifecycleprüfung, keine Modfunktionsabnahme.
3. **Lampenprobe:** Je eine platzierte Pflanzenlampe, eine schaltbare Steh-/Wandlampe und eine feste Weltleuchte in ihrer tatsächlichen Komponentenzusammensetzung erfassen; Zustand und Darstellung dokumentieren.
4. **Gezielte Referenzen:** Nach der vorhandenen Dateiliste nur die tatsächlich für den nächsten Adapter benötigten DLLs anfordern. Ein Startsatz wird voraussichtlich die passende `UnityEngine.CoreModule.dll`, je nach Adapter weitere Unity-Module, `Il2CppInterop.Runtime.dll`, `Il2Cppmscorlib.dll`, `Assembly-CSharp.dll` und gegebenenfalls `Il2CppScheduleOne.Core.dll` umfassen. Das ist eine vorläufige Bedarfsliste, kein bestätigter vollständiger Buildsatz. Für den separaten Steamtransport käme bei dessen Übernahme die passende `Il2Cppcom.rlabrecque.steamworks.net.dll` hinzu.

Ein vollständiger Spielordner und persönliche Spielstände werden für die erste Referenzprüfung nicht benötigt. Spiel- und generierte Interop-Dateien bleiben Entwicklungsreferenzen und werden nicht veröffentlicht.

## 5. Voraussetzungen für eine spätere Freigabe

- Exakte Referenzdateien, Dateiversionen und Hashes festhalten.
- Frische LightControl-Release-DLL erzeugen und ihren Hash festhalten.
- Pflichtanforderungen und Spieltests aus dem Auftrag ausführen; offene Tests bleiben offen.
- Bestätigte DLL ohne Neubuild paketieren, entpacken, Dateiliste und Hashes prüfen.
- Aus genau dieser ZIP sauber installieren und den Installationstest außerhalb der ZIP mit deren endgültigem Hash dokumentieren.
- Jede Änderung des DLL-Hashes erzeugt einen neuen Abnahmekandidaten; jede Änderung der ZIP erfordert eine neue Archivzuordnung und Installationsprüfung.

Die S1API-eigene Releasevalidierung, lokale Kerntests und statische Archivprüfungen sind unterschiedliche Belege. Sie dürfen nicht als ersetzbare Nachweise einer im Spiel fertigen Mod zusammengezählt werden.
