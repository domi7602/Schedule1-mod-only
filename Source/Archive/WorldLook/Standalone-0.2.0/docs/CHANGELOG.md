# WorldLook — Änderungen dieses Build-Pakets

## Wassertest vorbereitet — 22.10.2026, 15:12 UTC

- Nach Freigabe und bei geschlossenem Spiel die bisherige Konfiguration gesichert und ausschließlich `WaterEnabled` auf `true` gesetzt. Gesamtlook und Wolken sind an, zusätzliche Lichtanpassung steht auf 0.
- JSON-Rücklesen, Vergleich auf genau eine inhaltliche Einstellungsänderung und Backup-/Nachher-Hashes geprüft. Installierte DLL weiterhin `aebd2b81…`; vorhandenes eingebettetes Bundle wird verwendet. [Testprofil, Rückweg und Prüfablauf](water-on-test-20261022-1510/README.md).
- Vorbereitung abgeschlossen; neuer Spielstart, Wasseroptik, tatsächlicher Referenzshader-Bind und F7-Rundwechsel mit Wasser noch offen. Die vorherige Himmels-/F7-Bestätigung stammt aus dem Wasser-aus-Profil.
- Nachfolgender Besitzerlauf: lesende S1MCP-Abfrage bestätigt aktiven Wasserrenderer mit unterstütztem `S1Mods/ReferenceWater`, Log bestätigt das vorgesehene Referenzrezept. Erstes Besitzerbild (HUD Mittwoch 11:37) zeigt sichtbare dunkle Wasserfläche und feine Wellen; Dunkelheit/Mattheit ist bislang die Einschätzung der Assistenz, keine Besitzer-Fehlermeldung. Uferklarheit, Bewegung und F7 mit Wasser offen. [Live-Daten und Bildbeurteilung](water-on-test-20261022-1510/README.md#erste-live-prüfung-und-screenshot).
- Zweites Bild (Donnerstag 15:25): Kaimauer durch Wasser erkennbar, Vordergrund beinahe schwarz; gleicher F7-OFF-Blick als nächster Vergleich. Spätere native Zeit-/Sonnenabfrage bestätigt außerdem den HHMM-/Minutenfehler in `ReadDayFraction()` und der MCP-Zeitanzeige. Ursache der Wasserverdunkelung dadurch nicht belegt; keine Quelländerung. [Beobachtung und Aussagegrenzen](water-on-test-20261022-1510/README.md#zweites-bild-und-prüfung-der-tageszeit).
- Nachfolgendes AN/AUS-Paar (Freitag 15:08/15:24) zeigt mit WorldLook bessere Sicht auf Grund/Pflanzen/Mauer und eine ruhigere Oberfläche; auch der native Vergleich ist dunkel. Die frühere Vermutung starker allein modbedingter Verdunkelung wird dadurch zurückgenommen, kein pauschaler Aufhellungsfix abgeleitet. Log bestätigt Wasser `Stopped`, Gesamtlook OFF und gültigen Sky-Backup-Restore; erneutes ON mit Wasser nicht aufgezeichnet. [Vergleich und Grenzen](water-on-test-20261022-1510/README.md#geliefertes-anaus-vergleichspaar).

## Wolkenberechnung auf nativem Skybox-Pass wiederhergestellt — 22.10.2026, 13:40 UTC

- Nach positivem Besitzer-Blautest ursprüngliche Wolken-/Tageszeit-/Sonnen-/Mond-/Sternberechnung aus der gehashten Referenz wiederhergestellt. Nur UnityCG-Syntax angepasst: native Richtungsmultiplikationen, `sampler2D`/`tex2D`, `UNITY_PI`/`UNITY_TWO_PI`. Bewährter Clip-/Passweg, Properties, Materialbuffer und Background-Zustände bleiben erhalten.
- Nur der Skyshader geändert; Runtime-C#, Editor-Normal-Registrierung, Wasser und Konfiguration unverändert. Wasser weiter aus, direkte Lichtanpassung 0. Keine globale HDR-Klemmung oder Kamera-/Dome-/Clear-Änderung.
- Vollständiger Build, 0 C#-Warnungen/Fehler, 32 Gruppen/8.283 Checks ohne Fehler, unabhängiger Quellreview PASS im Restore-Scope. Neue DLL `aebd2b81…` mit verifiziertem eingebettetem Bundle und Backup installiert. Kein Spielstart, vollständige Optik/Tag-Nacht/Lifecycle offen. [Nachweis und geerbte Grenzen](cloud-restore-20261022-133344/README.md).
- **Besitzerbestätigung am 22.10.2026:** „die skybox sah im spiel normal aus keine orangene schleier mehr“. Anschließend F7 aus/an ausdrücklich ohne Probleme bestätigt. Gesicherter Log 17:01:29–17:01:39 lokal: vier vollständige OFF/ON-Rundwechsel plus finales OFF, fünf gültige native Backup-Restores und vier erneute WorldLook-Skybox-Bindungen. F9 bestätigt gültige native Skybox, `skyFaulted=false`, `skyPendingRestores=0`; letzter Sitzungszustand OFF. [Log-/Statusnachweis](cloud-restore-20261022-133344/runtime-toggle-20261022-150146/README.md). Kamerabewegung, Tag/Nacht, Szene-/Savewechsel sowie Wasser und Licht wurden nicht separat bestätigt; keine vollständige Featureabnahme.

## Temporärer nativer Skybox-Kompatibilitätstest — 22.10.2026, 13:21 UTC

- Nach fehlgeschlagenem Konstantblau-Test wird ausschließlich die Skybox-Anbindung getestet: `CGPROGRAM` mit installiertem `UnityCG.cginc`, ungetaggter Pass und `UnityObjectToClipPos`, ohne manuellen Far-Z-Override. Properties, Materialbuffer, Background-/Depth-States und das exakt gleiche blaue Fragment bleiben erhalten. Ungenutzte URP-Helper werden in dieser Diagnose nicht mitkompiliert; Originale sind gesichert.
- Editor registriert für Sky nun `PassType.Normal`; tatsächlich erzeugte VariantCollection bestätigt Sky `passType: 0`. Acht angeforderte Wasserregistrierungen und alle bestehenden Gates unverändert. Keine Runtime-C#-, Kamera-, Clear- oder Dome-Änderung.
- Vollständiger Build, 0 C#-Warnungen/Fehler, 32 Testgruppen/8.283 Checks ohne Fehler, unabhängiger Quellreview PASS im Zwei-Dateien-Umfang. Neue DLL `71e2519b…` mit verifiziertem Bundle und Backup installiert. Konfiguration unverändert, kein Spielstart oder visueller PASS. [Nachweis](native-sky-20261022-131454/README.md).
- Nachfolgender Besitzer-Blautest positiv: Neuer Bundlehash und Skybox-Bind im Log, kein F7-OFF in diesem Lauf. Screenshot zeigt Blau ohne die vorherigen neonorangenen Szene-Nachbilder. Bestätigung nur für diese Diagnosevariante; ursprüngliche Wolken-/Himmelskörperberechnung, Wasser, Uniformbindung und vollständige Lebenszyklusabnahme bleiben offen. Einzelursache innerhalb Tag-/Matrix-/Tiefenänderungen nicht isoliert.

## Temporäre Konstantsky-Diagnose — 22.10.2026, 12:56 UTC

- Nach Besitzer-Tests mit Wasser aus und direkter Lichtanpassung 0 bleibt das orange/verzerrte Startbild. Für die ausdrücklich freigegebene Diagnose ist ausschließlich der Fragmentkörper des Skyshaders durch einen festen linearen Blauton ersetzt; Pass, Vertexcode, Properties, CBUFFER, C# und Restore bleiben unverändert.
- Vollständiger Unity-/Bundle-/DLL-Build erfolgreich, C# 0 Warnungen/Fehler, 32 Testgruppen mit 8.283 Checks ohne Fehler. Ressourcen-/Quellhashes passen; unabhängiger Quellreview PASS im begrenzten Delta.
- Nur die neue DLL mit Hashprüfung und Backup installiert. Konfiguration unverändert, kein Spielstart und keine Bildabnahme. Kein fertiger Look, nicht als Grafik-Fix veröffentlichen. [Diagnosebelege und Grenzen](constant-sky-20261022-125118/README.md).
- Nachfolgender Besitzer-Test: Trotz nachweislich geladenem Konstantblau-Bundle weiterhin Orange sowie mehrfach nachgezogene MOTEL-Buchstaben, Kabel und Brückenränder. Erwartete einfache blaue Ausgabe nicht sichtbar; Skybox-Draw/Abdeckung und folgende Bildverarbeitung sind zu prüfen. Das ist ein Hinweis auf behaltene Bildinhalte, kein GPU-seitiger Nachweis der konkreten Ursache. Wolkenberechnung, Wasser und direkte Lichtanpassung sind für dieses Testbild nicht erforderlich.

## Unveröffentlichter Reparaturquellstand — 22.10.2026

- Sky-Materialien erhalten vor Veröffentlichung eine eigene native Backup-Kopie. Wiederherstellung mit Readback, aufbewahrte unbestätigte Freigaben, Szenengrenzen und F7-beständige Fremdbesitzsperren ersetzen das bisherige Restore auf `null`.
- Der nicht als Ursache belegte Dome-Abschaltversuch wird nicht fortgeführt. Unpassende native Materialverträge bleiben unangetastet.
- Eigene Wasserspiegelkamera, RenderTexture, zusätzlicher Kamerarender, Eventregistrierung und globale Reflexionsuniforms entfernt. Farb-/Windupdates laufen begrenzt über den Mod-Tick.
- Schwacher Himmelsfarbanteil und unabhängige Wasser-Mindestdeckung statt pauschaler Nullreflexion. Ungültige Bodentiefe löscht den Wasserkörper nicht mehr. `_SurfaceOpacity` ist Pflicht im neuen Shadervertrag; alte Bundles werden für diesen Referenzpfad abgelehnt.
- Wasser-Restore schreibt bei verlorenem Original ebenfalls keinen leeren Materialslot, sondern meldet den zurückbehaltenen Zustand.
- Unity-freie Richtlinien-/Delegate-Tests, Quellvertragsprüfungen und numerisches Mischmodell ergänzt. Genaue Prüfergebnisse und Grenzen: [Reparaturnachweis](repair-2026-10-22/README.md).
- **Vollständiger Unity-/DLL-Neubau am 22.10.2026 um 09:19 UTC erfolgreich:** Nach Wiederinstallation des freigegebenen Editors 2022.3.62f3 wurde das Bundle frisch gebaut und in die neue DLL eingebettet. Ressourcen-/Datei-/Quellhashes passen; C# 0 Warnungen/Fehler, 32 Testgruppen mit 8.283 Checks ohne Fehler, scoped Formatprüfungen erfolgreich. [Aktueller Buildnachweis](full-build-20261022-091929/README.md). Alte Artefakte und fehlgeschlagene Versuche bleiben erhalten. Nichts aus diesem Reparaturstand installiert; keine visuelle Abnahme, orangefarbener Himmel weiterhin ungeklärt. Versionsnummer 0.2.0 unverändert.

## 0.2.0 — Prototyp, 21.10.2026

Dieser Quellstand wurde vorbereitet, aber in der Vorbereitungsumgebung **nicht kompiliert und nicht im Spiel ausgeführt**. Die Freigaben und Testzahlen des historischen Changelogs gelten nicht automatisch für diesen Stand.

- Eigenständiges `net6.0`-Projekt; fehlende kleine Shared-Hilfsfunktionen und Probe-Quellen ergänzt. Spiel-/Loader-/S1API-Referenzen kommen aus der lokalen Installation.
- Gemeinsamer Build-Starter für Unity und C#. Shaderbundle wird neu gebaut, gegen seinen SHA-256-Bericht geprüft und als Ressource in `WorldLook.dll` eingebettet.
- Gemeinsamer Shader-Lader für Himmel und Wasser. Speicherroute mit geprüftem Datei-Cache als Fallback; keine externe Bundle-Verteilpflicht.
- Neuer Skyshader mit zwei prozeduralen Wolkenschichten, Tageszeit-/Wetterabstimmung und einer auf bekannte native Ziele begrenzten Anwendung.
- Eigene Sky-Materialkopien mit Brücke für die zwölf nachgewiesenen nativen Sonnen-, Mond-, Stern- und Fog-Properties. Abschalten stellt kontrollierte Slots zurück; spätere fremde Bindungen bleiben erhalten.
- Kleine reversible Anpassung von Sonnenfarbe/-intensität, Schattenstärke und gegebenenfalls Trilight-Ambient. Der alte GoldenHour-Schreiber bleibt deaktiviert.
- Wasser übernimmt dieselben aktuellen Himmelsfarben und Wetter-Windgewichte. Die vier vorhandenen Wellen bleiben erhalten; kleine entfernte Komponenten werden gedämpft.
- Vormultiplizierte Wasserkomposition, getrennte Reflexions- und Körpergewichte, kontrollierte Depth-/Refraction-Auswertung, beleuchteter Uferschaum und URP-Hauptlichtschatten für Sonnenreflexe.
- Reflexionslaufzeit mit nativer Kameraidentität, Rücksetzen der Verfügbarkeit pro relevantem Hauptkameraframe, kontrollierter RT-Größe und Anforderungsprüfung für Depth/Opaque.
- Strg+Umschalt+F7 schaltet den gesamten Look für die Sitzung um. F8 bleibt der manuelle Voll-Probe. F9 schreibt einen gezielten Status.
- Neue JSON-Konfiguration und deutsche Build-/Installationsanleitung. Kein automatisches Deployment ins Spiel.

Die ursprüngliche Historie ist unverändert als `CHANGELOG-0.1.x-original.md` enthalten. Die dort genannten erfolgreichen Builds, Tests und Nutzerfreigaben stammen aus dem gelieferten Ausgangsstand.
