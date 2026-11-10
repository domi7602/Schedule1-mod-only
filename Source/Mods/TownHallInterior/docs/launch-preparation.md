# TownHall: vorbereiteter Start-/Lade-/Handytest

Stand: 2026-11-05. **Start-/Lade-/Handy-Basistest abgeschlossen: Start/Laden
logbelegt, Handy-Öffnen/-Schließen eigentümerbestätigt.** Dominik bedient das Spiel. Dieser Ablauf
prüft keine Business-Speicherung und öffnet die akzeptierte Innenraumabnahme
nicht erneut. Der folgende Sicherungsstand beschreibt die frühere Vorbereitung.

## Warum die installierte Fassung ersetzt werden musste

Der unabhängige statische DLL-/IL-Audit identifizierte `cff67a85…` als die alte,
tatsächlich verdrahtete Dominik-/SaveGame_1-Doppel-Save-Probe. Eine reguläre
Quittung vom 2026-11-04 um 07:16:09 UTC und das vorhandene Kandidatenpaar stimmen
per Hash/MVID/PDB-ID mit dieser Installation überein. Das belegt weder deren
damalige Ausführung noch den vollständigen damaligen dirty Quellstand. Die
frühere pauschale Annahme „Save-Vorbereitung nicht installiert“ galt damit nicht
für die tatsächlich vorgefundene DLL.

Auf Dominiks ausdrückliche Auswahl **„Normalbuild installieren“** wurde der
bereits geprüfte Normalbuild ohne Business-Proben installiert, ohne Neubuild:

- DLL SHA-256: `614b81fd584040d4cebe7a18929bca09eabae478a6487ec6d846ea01d6062003`
- PDB SHA-256: `8126ae65f5c12f6e8261af0a1734e622d8c0ab5522a91e6653c7b1ffd7d8c497`
- Metadaten weiter 0.1.0, unverändert. Regulärer Deploymenthelper, nur DLL/PDB
  ersetzt. Shared, andere Mods und S1API unverändert.

Die kompilierten Mod-Entry-Points dieses Normalbuilds rufen keine Business-
Proben auf; deren native Runtime-Typen fehlen. Das reine Zustandsmodell ist
kein nativer Adapter. Die [Speicherprobe](business-manager-save-probe.md)
bleibt ein getrenntes, noch nicht live-bereites Vorhaben.

## Sicherung und Grenzen

- Testziel anhand seiner gespeicherten Metadaten bestätigt: **TownHallTest /
  SaveGame_2**, gespeicherte Spielversion 0.4.7f12. Während der Vorbereitung kein Laden ausgeführt.
- Privater Checkpoint `20261105T062735613294Z-d8477f8e`: 39 native Testslotdateien,
  162 UserData-Dateien ohne Deploymenthistorie und die drei bisherigen TownHall-
  Payloaddateien. Kopien und Vorher-/Nachherhashes geprüft. Konfiguration und
  Save-Inhalte bleiben ausschließlich im Git-ignorierten lokalen Backup.
- Die vollständige UserData-Sicherung kann Mod-Sidecars anderer Slots enthalten;
  diese wurden nur kopiert. Der native Hauptslot wurde weder geöffnet noch
  verändert. Nach Installation sind die 39 Testslotdateien bytegleich; in den
  verglichenen Mods/UserData änderten sich nur TownHall-DLL/PDB.
- Steam Cloud ist laut lokalem `CloudEnabled=0` deaktiviert; Einstellungen
  unverändert, keine Konfliktlösung oder Aussage über den Remote-Stand.
- Normale Spiel-Autosaves sind **nicht** deaktiviert. Kein Versprechen einer
  schreibfreien Sitzung. Einen Rückweg nur bei geschlossenem Spiel und nach
  Prüfung der zusammengehörenden nativen und Moddaten anwenden; nicht blind
  die gesamte UserData-Sicherung über inzwischen neuere Daten kopieren.

Belege, genaue Beobachtungszeitpunkte, Quittung und getrennte Prüfzuständigkeiten:
[Startvorbereitungsnachweis](evidence/launch-preparation-20261105.json).
Die 681 Tests, drei Builds und 80 Auditfälle des vorherigen Offline-Schritts
bleiben dessen Nachweise; Installation ist keine neue Gameplay-Abnahme.

## Beobachteter Start-/Ladestand nach „geladen“

Dominik meldete am 2026-11-05 „geladen“. Der rein lesende Mitschnitt um
07:51:25 UTC ordnet den frischen Prozess 11888 seit 07:45:03 UTC derselben
Installation zu. Die Loader-Hashes bestätigen Normalbuild `614b81fd…`, S1API
3.2.2 `d751a3cb…` und Loader `b05187bd…`; die aktuellen Dateien stimmen überein.
Das Log ordnet Slot 2 TownHallTest zu, lädt SaveGame_2 um 08:46:06 CET und
erreicht Main sowie OnLoadComplete. TownHall meldet den veröffentlichten
Innenraum um 08:46:17 CET. Das ist ein Start-/Ladenachweis, keine neue Sichtabnahme.

Elf PhoneApp-Initialisierungsaufrufe sind protokolliert; zum Mitschnittzeitpunkt
war Handy-Öffnen/-Schließen noch nicht einzeln bestätigt. Im erfassten Logpräfix
keine ERROR-/Fatal-Meldung und keine TownHall-Exception/Probenmeldung. Fünf
Warnungen bleiben sichtbar: Interop-Ersatzsignatur, zwei TownHall-Staginghinweise,
Taxi-Parkplatzfallback im Menü und eine abgefangene LightControl-Schrift-
MissingMethodException mit protokolliertem Arial-Fallback. Die nachfolgenden
App-Initialisierungen laufen weiter; daraus folgt keine vollständige App-Abnahme.

[Laufzeitbericht mit Grenzen](evidence/launch-live-loaded-20261105.json),
[ausgewählter Logauszug mit Originalzeilen](evidence/launch-live-loaded-excerpt-20261105.txt).
Keine Bridge-/Konsolenbefehle, Spielsteuerung, Save-Aufrufe oder nativen
Save-Dateizugriffe durch den Agenten. Kein Build/Deployment im laufenden Spiel.
Native Business-Speicherprobe weiterhin nicht live-bereit; normale Autosaves
bleiben möglich. Die frühere 21/22-Pure-Prüfung gilt für den damaligen Snapshot;
für diesen Dokumentationsnachtrag wegen laufendem Spiel nicht erneut ausgeführt.

## Eigentümerbestätigung: Basistest abgeschlossen

Auf die ausdrückliche Frage, ob das Handy problemlos auf- und zuging, antwortete
Dominik am 2026-11-05: **„lies sich problemlos öffnen und schließen“**.
Damit ist der vorgesehene Start-/Lade-/Handy-Basistest auf TownHallTest / Slot 2
abgeschlossen. Die Handybedienung ist eine Eigentümerbestätigung, keine eigene
Bildschirmbeobachtung. Der vorherige Logbericht bleibt als damaliger Stand unverändert.
[Zugeordnete Bestätigung und Umfang](evidence/launch-phone-owner-confirmation-20261105.json).

Keine Einzelabnahme aller elf Apps, Käufe, Geldwäsche, neuer API-Funktionen oder
nativer Business-Speicherung. Die Schreibsperre und die erfassten Warnungen
bleiben unverändert; es werden keine weiteren Ingame-Aktionen daraus abgeleitet.

## Durchgeführter Eigentümerablauf (Referenz)

1. Schedule I **über Steam** starten, nicht die EXE direkt.
2. Im Laden-Menü ausdrücklich **TownHallTest / Slot 2** auswählen, nicht blind
   „Fortsetzen“ und nicht Dominik / Slot 1.
3. Nach vollständigem Laden etwa 20 Sekunden warten, Handy öffnen und schließen.
   Keine Käufe, Geldwäsche, Konsolenbefehle oder manuellen Speicherproben.
4. Melden, dass TownHallTest geladen ist, beziehungsweise die genaue Fehlermeldung
   nennen. Der neue Lauf kann dann anhand des frischen Logs zugeordnet werden.

Start/Laden sind wie oben belegt; Handy-Öffnen/-Schließen ist inzwischen
ausdrücklich bestätigt. Für diesen begrenzten Basistest bleibt keine Abnahme offen.

Bei falschem Slotnamen oder neuen Exceptions nicht mit weiteren Loads/Save-
Versuchen fortfahren. Ein erfolgreicher Handytest belegt nicht den nativen
Business-Abschlussvertrag und erlaubt keine Aktivierung der Save-Probe.
