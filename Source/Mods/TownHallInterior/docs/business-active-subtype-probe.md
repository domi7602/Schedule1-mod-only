# TownHall: Lebenszyklus des aktiven eigenen Business-Untertyps

Stand: 2026-11-06. Der Aktivierungsbaustein nach der bestandenen inaktiven
[No-IO-Speicherprobe](business-manager-save-probe.md) ist implementiert und offline
geprüft. **Begrenzter Lifecycle-Test um 11:55:28 CET bestanden; seit 11:57:33 CET
ist der Normalbuild wiederhergestellt.**

## Ziel und Abgrenzung

Der hier untersuchte Baustein aktiviert die bereits untersuchte eigene
`ProbeBusinessSubtype`-Instanz und beobachtet deren Unity-Lebenszyklus. Er hängt
an den tatsächlichen `BusinessSubtypeRuntime`-/`NativeSubtypePort`-Aufrufstellen.
Die zurückgezogene [globale Aktivierungsprobe](business-activation-probe.md)
bleibt durch `TOWNHALL_BUSINESS_ACTIVATION` / `#error` gesperrt.

Der neue optionale Buildschalter heißt `S1TownHallBusinessActiveSubtype`.
Eine eigene Root mit genau Transform und Business bleibt unveröffentlicht:
kein NetworkObject, keine Einträge in Property-/Business-/Save-Listen, kein
Spawn, Kauf, Speicheraufruf oder Geldwäsche. Die Abnahme dieses Bausteins lautet
„eigene aktive Business-Shell mit beobachtetem Unity-Lifecycle und Rückbau“.
Das ist eine Voraussetzung für die spätere Business-Anbindung, kein fertiges
natives Business und kein Ersatz für dessen Initialisierungsvertrag.

## Vertrag und Eigentümer

1. Ein Prozessversuch ausschließlich in vollständig geladenem Singleplayer
   `TownHallTest / SaveGame_2`, mit bereiter Halle und passenden Abhängigkeiten.
   Doppelte Ladeereignisse oder Fehler öffnen keinen zweiten Versuch.
2. Vor Konstruktion werden die eigenen virtuellen Ziele geprüft. Vor Aktivierung
   müssen zusätzlich die benannten Unity-Methoden auf eigene native MethodInfo-
   Einträge zeigen: eigene deklarierende Klasse, nicht statisch, `void()`, gültiger
   MethodPointer, VirtualMethodPointer und Invoker. Metadatenprüfung ersetzt
   keine spätere Beobachtung der Unity-Aufrufe.
3. Root-, Business-, Klassen- und Manageridentität sowie Weltgeneration werden
   gebunden. Vor jedem weiteren Eingriff sind frischer Weltkontext, dieselben
   Identitäten und das Fehlen eigener externer Registerreferenzen erforderlich.
4. `Begin` erstellt inaktiv, prüft den Zustand und aktiviert genau einmal.
   `Tick` wartet auf einen echten späteren `UnityEngine.Time.frameCount` und
   eigene `Awake`, `Start`, `OnEnable` sowie mindestens ein `FixedUpdate`.
   Die synthetischen Fortschrittsmarker der inaktiven Save-Probe werden dafür
   ausdrücklich nicht verwendet.
5. Der bestätigte Rückbau deaktiviert zuerst, beobachtet `OnDisable` und zerstört
   anschließend nur die eigene Root. Erfolg braucht native Abwesenheit, genau
   ein `OnDestroy` sowie passende Callback- und fremde Register-/VTable-Readbacks.
   Fremde Listen werden weder geleert noch auf einen alten Stand zurückgesetzt.
6. Unlesbare oder geänderte Identität, fremde Halter, unerwartete Callbacks oder
   unklare Teilwirkungen sperren weitere Aktionen. Kein blinder Wiederholungs-
   oder Destroy-Versuch. Reentrantes Verlassen invalidiert den Versuch, ohne
   innerhalb der laufenden Mutation nochmals einzugreifen. Der Runtime-Port
   bleibt für Diagnose und eindeutigen Besitz erhalten.
7. Die Wartezeit ist auf fünf Sekunden begrenzt. Ein Timeout erlaubt Rückbau
   nur bei weiterhin lesbar bestätigter eigener Identität und Referenzfreiheit.
   `Leaving`, Main-Unload und `Stop` sind echte Anschlusspunkte; ein fehlender
   Nachweis wird als solcher erhalten, nicht durch einen neuen Weltkontext ersetzt.

## Prüfungen und offene Grenzen

Ausgangsstand vor Änderungen: **884/884 TownHall-Offlinetests bestanden**.
Die **89 neuen Fälle** prüfen gleiche/rückläufige Frames, doppelte Aktivierung,
Kontextwechsel zwischen Teiloperationen, reentrantes Verlassen, fehlende oder
doppelte Callbacks, unerwartete Save-/Netzwerkaufrufe, fremde Instanzidentität,
Fehler nach Teilwirkung und fehlgeschlagene Belege nach Destroy. Dazu kommen
verspätete positive Beobachtungen nach der Deadline und Rückschritte innerhalb
frischer Rückbau-Readbacks. **973/973 TownHall-Tests bestanden**. Der Testport
simuliert diese Zustände; er beweist keine Unity-Ausführung.

Alle fünf Varianten `Normal`, `Subtype`, `Interface`, `Manager` und `ActiveSubtype`
bauen mit `S1NoDeploy=true` ohne Warnungen oder Fehler. Legacy-Aktivierung und
die Kombination ActiveSubtype + Manager scheitern erwartungsgemäß an `CS1029`.
Unabhängiger Code-/Testreview abgeschlossen, keine offenen Pflichtkorrekturen.
Ein Reviewbefund wurde korrigiert: Berichte nach Widerruf lesen den zuletzt
geprüften Frame aus dem Cache und rufen nicht erneut Unity auf.
[Prüfwerte und Quellidentitäten](evidence/business-active-subtype-20261106/verification.json),
[unabhängiger Review](evidence/business-active-subtype-20261106/audit/final-code-review.json).

Kandidat-DLL SHA-256:
`c07ada69d74f7491528efff2f4518319a9404ccd3a8e5ba948e876f8eaf38b34`;
PDB `711863c0c5833c69c6680fcb181804b26b0adb8837b16f34ed7f348d43417a5f`.
Produktionsquelle `BusinessConstruction.cs`:
`32724b5085d1d6ca6bb34340e6b95198f450c4f67a2c5e5bbb187b68286ad708`.
Nur diese Produktionsdatei und die Projektdatei wurden erweitert; `Mod.cs` nutzt
seine vorhandenen Anschlüsse unverändert. Dateibudget weiterhin 20 Produktions-/
Projektdateien. Basisrevision `b2c6c7e6c6ecc778da9624cdb68a6578b9db471a`, dirty.

Die frische Metadatenprüfung verwendet die installierten, hashpassenden FishNet-
und Il2CppInterop-Assemblies. Sie bestätigt eigene Methodentabelleneinträge und
Invoker sowie die untersuchte NetworkBehaviour-Methodenoberfläche, keine native
Unity-Ereigniszustellung. [Metadatenaudit](evidence/business-active-subtype-20261106/audit/metadata-audit.json).

Diese Offline-Prüfungen stammen aus der Implementierung vor der Installation.
Sie wurden für das anschließende Deployment nicht neu ausgeführt und belegen
keine Aktivierung im Spiel. Die normalen und bisherigen inaktiven Buildpfade
bleiben getrennt.

## Installation vor der Live-Prüfung

Auf Dominiks Auftrag wurde der geprüfte ActiveSubtype-Kandidat am 2026-11-06
um **11:00:03 CET (10:00:03.1679217 UTC)** über `Tools/deploy-mod.ps1` installiert,
ohne Neubuild. Versuch `20261106T100002932Z-77c756180c8f`: DLL `c07ada69…`
und PDB `711863c0…` kopiert; die Metadaten `65938002…` waren bereits identisch
und wurden nicht geschrieben (`Action=Unchanged`, `State=NotAttempted`).
Der Helper sicherte den Vorgänger unter
`20261106T100002974Z-TownHallInterior-905551cc`.
[Deploymentquittung](evidence/business-active-subtype-deployment-20261106/deployment-receipt.json),
[Installationsabgleich und Grenzen](evidence/business-active-subtype-deployment-20261106/deployment-verification.json).

Vorher wurde ein frischer privater Checkpoint mit 39 nativen Slot-2-Dateien,
172 UserData-Dateien und normaler Rollback-Payload angelegt. Die Payload liegt
repository-relativ unter
`.backups/townhall-launch-prep/20261106T095942344997Z-c3eb0025/payload`
und enthält den Normalbuild `614b81fd…` / PDB `8126ae65…`.
Der nachfolgende Abgleich bestätigt alle 39 nativen Testdateien bytegleich,
denselben Satz von 38 Moddateien mit ausschließlich geänderter TownHall-DLL
und denselben Satz von 172 UserData-Dateien mit ausschließlich geänderter
TownHall-PDB (Deploymentquittungen ausgenommen). Slot 1 wurde nicht gelesen.
Ein unabhängiger Installationsreview bestätigte Payload, Helperquittung,
Vorgängerbackup und das vor/nach der Installation geschlossene Spiel.
Installation und Sicherung sind keine neue Test-, Build- oder Gameplay-Abnahme.

## Begrenzter Live-Test und Wiederherstellung

Dominik lud TownHallTest / Slot 2 und meldete „geladen“. Der Prozess 9212 lief
seit 11:54:34 CET mit Kandidat `c07ada69…`; der frische Bericht
`20261106T105528196Z.json` entstand um **11:55:28 CET (10:55:28 UTC)** im
vollständig geladenen Singleplayer-Kontext `TownHallTest / SaveGame_2`.
SHA-256 des unveränderten Berichts:
`5770b58eb4c9b9f22db129b3d424e6aed426fde1ad35810968c8e66a3cad8602`.
[Beobachtung und unabhängiger Review](evidence/business-active-subtype-live-20261106/observation.json),
[Originalbericht](evidence/business-active-subtype-live-20261106/live-report.json).

Der Versuch meldet `observed` mit leeren Findings. Die echten Unity-Frames
wechselten von 2243 auf 2244. `Awake`, `OnEnable`, `Start` und `OnDisable`
wurden jeweils einmal beobachtet, `FixedUpdate` zehnmal und `OnDestroy`
einmal. Die zehn FixedUpdate-Aufrufe sind keine zehn vergangenen Unity-Frames.
`CleanupComplete` und `RootDestroyed` sind bestätigt. Alle zehn Register und
14 fremden VTables blieben gleich. Die eigene no-base Shell hatte keinen
NetworkObject, wurde nicht in Weltregistern veröffentlicht und rief keinen
Save-Pfad auf. Das belegt diesen begrenzten eigenen Unity-Lebenszyklus mit
Rückbau, keine originale native Business-Initialisierung.

Nach Dominiks „Geschlossen“ wurden **39/39 native Slot-2-Dateien bytegleich**
zum Checkpoint vorgefunden; keine Dateien hinzugefügt oder gelöscht und der
Business-Dateibestand unverändert. Leere Baseline-Verzeichnisse waren nicht
inventarisiert; daraus wird keine vollständige Abwesenheit von Schreibaktionen
abgeleitet. [Save-Abgleich](evidence/business-active-subtype-live-20261106/save-verification.json).

Bei geschlossenem Spiel stellte der Deploymenthelfer um **11:57:33 CET**
Normalbuild `614b81fd…` / PDB `8126ae65…` wieder her. Versuch
`20261106T105733407Z-22ae4cab2444`; Payload und installierter Stand wurden
unabhängig bestätigt. Keine neuen Builds oder Tests für diesen Liveabschluss.
Der wiederhergestellte Normalbuild wurde anschließend nicht erneut im Spiel getestet.
[Rollbackabgleich](evidence/business-active-subtype-live-20261106/rollback-verification.json),
[Rollbackquittung](evidence/business-active-subtype-live-20261106/rollback-receipt.json).

Offen bleiben NetworkObject-Lifecycle, nichtvirtuelle native Business-Pfade,
Weltregistrierung vor abhängiger Objektladung, echte Persistenz und Reload.
Die erfolgreiche begrenzte Aktivierungsprobe beweist keine allgemeine
Last-Use- oder Weltwechselgarantie bei ausstehenden nativen Speicheraufträgen.
Die nächste offene Entwicklungsstufe innerhalb Phase 1 ist die eigentliche
native Business-Initialisierung mit separat geklärtem NetworkObject-Lifecycle
und rechtzeitiger Weltregistrierung vor abhängiger Wiederherstellung. Ein
stabiler nativer Save-/Load-Weg bleibt Abschlusskriterium von Phase 1.
[Fertigstellungsplan](business-completion-plan.md),
[Weltwechselbefunde](native-world-exit-audit.md).
