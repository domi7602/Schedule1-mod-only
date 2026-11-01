# Rathaus: geschlossene Backsteinfläche hinter der Schiebetür

## Befund und Grenzen

Dominik meldete nach dem ersten Umbau: Schiebetüren vorhanden, Flackern weg,
Klopf-Prompt nach einer weiteren Änderung entfernt, die Backsteinfläche bleibt aber
sichtbar und lässt sich durchlaufen. Sein Innenraumbild zeigt außerdem ein nicht
vollständig verkleidetes Obergeschoss. Das ist keine fertige Interior-Abnahme.

Die Offline-Prüfung der tatsächlich installierten Szene vom 2026-10-28 findet drei
unabhängige Wandbeiträge hinter dem bereits ausgeblendeten Türblatt:

| Pfad relativ zu TownHall | Renderer-ID im Asset | Mesh/Submesh in level1 | Materialslots | LOD |
|---|---:|---|---:|---|
| `townhall/Walls` | 302660 | 41 / 84 | 1 | 0 und 1 |
| `Cube` | 284066 | 125 / 127 | 1 | unabhängig |
| `townhall/Cube` | 294387 | 95 / 84–86 | 3 | 1 und 2 |

Asset-IDs sind keine Laufzeitidentitäten. Referenz: [vollständiger Offline-Befund](evidence/facade-analysis.json),
SHA-256 `d5fc434f23ba0c1e9d95da152a70211fdfc0631a4d6511a94241951014d58f58`.
Die Meldung „Tür ausgeblendet“ betraf die Door-Unterhierarchie, nicht diese Wände.
Die frühere Behauptung, Static-Batches würden `Renderer.enabled=false` ignorieren,
ist damit nicht belegt und wurde zurückgenommen.

Die Trim-Collider belegen den gesamten Grundriss, die zugehörigen Meshes dagegen
nur Randstreifen. Insbesondere existiert kein vollflächiger oberer Trim-Deckel als
nachgewiesene Ursache eines flackernden oberen Bodens. Außenorientierte Wand- und
Dachflächen ersetzen keine Innenwände und Decke.

## Umsetzung

- `Source/Mods/TownHallInterior/build/extract-native-facade.py` extrahiert nur die attribuierten Originalbeiträge
  aus der hashgebundenen Installation: 264 Vertices, 180 Dreiecke, fünf Submeshes.
  Keine private Blender-Datei, keine Verbindung zum Spiel.
- `Source/Mods/TownHallInterior/assets/native-facade.json` enthält ursprüngliche, ungeschnittene Positionen,
  Normalen, Tangenten samt Vorzeichen und UV0/UV1. SHA-256:
  `588d18a4d71360f7c7e68ffa0d0d6bc42c2d3cee0ed72e17a44701e09ef17553`.
- `FacadePortal.Cut` bildet die Polygondifferenz zum Eingangsvolumen:
  x 54.18–54.56, y 1.41–3.95, z 30.04–31.96. Die konkrete Schwelle bleibt erhalten.
  Kreuzende Dreiecke werden geteilt, nicht pauschal gelöscht; Attribute werden
  an den Schnittpunkten interpoliert. Unbetroffene Eingabedreiecke bleiben erhalten.
- `NativeFacadeSwap` baut eigene Renderer/Meshes unter dem zunächst inaktiven
  Raum-Root. Originalmaterialien werden nur ausgeliehen. Statische Vertices liegen
  bereits in Weltkoordinaten; der native Objekttransform wird nicht erneut angewendet.
- Native Combined-Meshbuffer bleiben unangetastet. Nur die drei Originalbeiträge
  werden nach erfolgreicher Vorbereitung über `forceRenderingOff` ausgeblendet;
  Ersatzrenderer erhalten dieselbe LOD-Mitgliedschaft. Keine Garantie, dass die drei
  Ersatzrenderer Teil desselben statischen Batches bleiben.
- Renderer-/Material-/Bounds-/LOD-Prüfungen und der Szenenhash sind Fail-Closed-
  Voraussetzungen. Vor Publikation entstehen keine begehbaren Teiländerungen.
- Die ursprünglichen Door-, Glass- und Quad-Renderer werden über genaue Pfade
  adressiert, nicht über eine Suche nach ungefähr türgroßen Renderern.
- Der vorhandene freie Schiebeeingang und der entfernte Spieler-Klopf-Prompt
  bleiben. Das NPC-Gebäude wird nicht in ein Business umgeschrieben.
- Ein Review fand einen blockierenden Restore-Fehler: eine LOD-Ausnahme übersprang
  Collider-/Tür-Restauration. `CleanupJournal` isoliert nun die einzelnen Schritte,
  behält Fehlschläge offen und verschiebt Meshfreigabe bis nach dem LOD-Detach.
  Der Host restauriert die genaue alte Türinstanz; zwei begrenzte Durchgänge
  melden bleibende Fehler ausdrücklich, ohne vollständigen Restore zu behaupten.

**Nur lokale Verwendung:** Die Extraktion enthält native Spielgeometrie. Weder
Fixture noch die DLL mit eingebetteter Fixture ohne gesonderte Rechte veröffentlichen.
Eine portable Mod müsste die Daten lokal erzeugen oder einen anderen Assetweg nutzen.

## Tatsächlich ausgeführte Prüfungen

- Exporter `--check` gegen die installierten Originaldateien bestanden. Originale
  indexierte Attribute und Dreiecksreihenfolge/Winding werden bitgenau gegengeprüft.
- xUnit: 92 Tests bestanden, darunter geometrischer Ausschnitt, Flächenerhalt,
  UV-/Tangenten-/Winding-Erhalt, unberührte Nachbarflächen, ungültige Eingaben sowie
  Portalproben in allen drei tatsächlichen f12-Wandbeiträgen.
- Vier Tests des verlinkten Produktions-Cleanup-Kerns: Fehler isolieren, nur
  offene Schritte wiederholen, Loggerfehler abfangen, Meshfreigabe nach LOD-Detach.
  Die Abbruchfälle wurden erst rot und nach der Korrektur grün beobachtet. Das ist
  verwaltete Policy-Ausführung, kein injizierter Unity-/IL2CPP-Fehlerlauf.
- Verdrahtungsprüfung: Ersatzvorbereitung vor Collideränderungen, Publikation vor
  Aktivierung des Raum-Roots. Diese Prüfung liest Code und führt Unity nicht aus.
- Release-Build mit `-p:S1NoDeploy=true`: bestanden, keine Warnungen/Fehler.
- Fokussierte Whitespace-Prüfung des geänderten Produktions-/Testcodes bestanden.
- Repository-Quick: nicht bestanden wegen vorhandener Format-, Solution- und
  Versionsabweichungen. Ein neu aufgefallener relativer Pfad in dieser Notiz wurde
  korrigiert; fremde Dateien wurden nicht mitrepariert.
- Dokumentationspfade nach der Korrektur separat bestanden. TownHall fehlen auch
  noch Inventar-/Versionszeilen und Changelog. Die dafür versuchte Änderung der
  geschützten Agentendatei wurde nicht freigegeben; kein alternativer Schreibweg.
- Isoliertes Pure: ausgeführt, aber nach 360 Sekunden abgebrochen. Bereits zuvor
  vorhandene Format-/Solution-/Versionsfehler und ein Git/MSYS-Fehler der
  Workspace-Setup-Tests wurden gemeldet; kein bestandenes Gesamtresultat.

**Noch nicht nachgewiesen:** Laufzeit des neuen Adapters in IL2CPP, tatsächlicher
LOD-Wechsel, native Rollback-/Reload-Pfade, sichtbarer Durchblick und Traversal im
Spiel. Dominik führt sämtliche Ingame-Schritte selbst aus; kein Computer-Use,
kein Start/Schließen und keine Tastatureingaben durch Eve in dieser Korrekturrunde.
Obergeschoss-Innenhaut/Decke, vollständiges Interior, NPC-Navigation und Business-
Integration sind mit diesem Ausschnitt nicht erledigt.

## Lokale Installation nach Abschluss des Reviews

Der abschließende unabhängige statische Delta-Review fand keinen weiteren belastbaren
Blocker im korrigierten Restore. Keine native Ausführung durch den Reviewer.

Am `2026-10-28T20:32:01.9803941Z` wurde ausschließlich TownHallInterior mit geschlossenem
Spiel über den regulären Deploy-Helper installiert. DLL, PDB und Metadaten wurden
anschließend unabhängig zurückgelesen: alle drei Hashes stimmen mit den Buildquellen
überein. Shared war bereits bytegleich und wurde nicht verändert. Auch die drei
vorherigen Dateien im Deploy-Backup stimmen mit dessen Hashmanifest überein.

- DLL SHA-256: `73e7ed65f5a58f3afd0e6daf64cc03365ef9129936dd88c30fceb3c01d2003c4`.
- Vorherige DLL im geprüften Backup: `5d6fa6fd9286e694d7d5a8274382b4360798262fa33d53504abc5a50c028955a`.
- [Archivierte Deploy-Quittung](evidence/facade-deploy-20261028.json), Versuch
  `20261028T203201788Z-ac3258d86c33`; SHA-256
  `bea145872c3eca1ebe6c67dfda3eec376cea570c4467f82f89059164aaa60f7b`.
- Backup-Kennung: `20261028T203201826Z-TownHallInterior-2fd24c54`.

**Installiert ist nicht geladen oder visuell akzeptiert.** Dominik startet selbst
über Steam und lädt ausschließlich den Teststand TownHallTest (Slot 2), nicht den
geschützten Dominik-Spielstand. Prüfen: automatischer Eingang, Durchblick vor dem
Betreten, hinein/hinaus und Fassade aus größerer Entfernung. Obergeschoss, NPCs
und native Restore-Fehlerläufe bleiben eigenständige unbestätigte Punkte.

## Historischer Versuch 2026-10-29: sichtbare Öffnung angehoben

Dominik meldete nach dem ersten Fassadenfix, dass er zum Eintritt ducken muss.
Die damalige Erklärung eines „gemessenen Zugangsbodens y 2,04“ und daraus
berechneter 1,9 m Kopffreiheit wird **zurückgenommen**: Die Zahl war nur eine
Teleportposition. Sie belegt weder Fuß- noch Bodenhöhe. Auch die damalige Aussage,
native Collider als mögliche Mitursache ausgeschlossen zu haben, war zu stark.

Geändert wurden damals die Portal-Oberkante 3,95 → **4,55**, der sichtbare innere
Türsturz auf y 4,55–4,85 und die Schiebeblätter auf 3,2 Höhe (1,35–4,55).
Drei neue bzw. verschärfte Tests schlugen zuerst fehl; danach bestanden insgesamt
94 Tests. Diese Tests erfassten den zweiten, unsichtbaren `west-lintel` nicht.

Neu installiert am `2026-10-29T04:01:56Z` (Spiel geschlossen): DLL
`641511a94e11f314f0c8df3e7fd0a00b69c452f76d3d4c8766bda5b4b20a29bf`;
die Fassung `73e7ed65…` vom Vorabend liegt im geprüften Deploy-Backup. Quittung
`20261029T040156284Z-cbccc2e18141`, archiviert als
[historische Quittung](evidence/facade-deploy-raised-20261029.json) (SHA-256
`a465f48a873813a0215e864350f6f14b710f72a23e760308773759d85f8d842a`).
Dominik meldete nach diesem Versuch erneut das Höhenproblem; keine Abnahme.

## Korrektur 2026-10-29: übersehener unsichtbarer Eingangscollider

`RoomPlan.Create` erzeugte weiterhin `west-lintel` ab y **3,35** mit `Solid=true`.
`UnityRoomSink.Add` blendet dessen Renderer aus, behält aber den Cube-Collider.
Der eigene öffentliche Boden liegt bei y **1,53** (1,48 + 0,1 / 2); damit blieben
im geometrischen Modell nur **1,82** Einheiten unter diesem Collider. Die alte
Kapselprüfung nutzte dagegen `RoomPlan.Ground=1,25` und kannte die zusätzliche
Innenhaut des Laufzeithosts nicht. Die größere sichtbare Öffnung reparierte diese
Kollision nicht.

Fünf neue Regressionen wurden vor der Korrektur tatsächlich rot beobachtet:
ein Höhenvergleich und vier Durchgangsfälle (Ein-/Austritt, Kapseln 1,85/0,35 und
1,95/0,40) meldeten ausdrücklich `west-lintel`. Nach Anheben **dieses** Colliders
auf die bereits bestehende Portalhöhe sind sie grün. Kein weiteres Vergrößern
der Fassade, kein Absenken oder Schrumpfen des Spielers.

- `PublicEntrance.PortalTop` verbindet nun Fassadenschnitt, beide Türstürze und
  Flügelhöhe. `ApproachGroundY` und die unbewiesene Bodenbehauptung wurden entfernt.
- Die sieben bestehenden Innenhaut-/Bodenboxen liegen mit gleichen Maßen in
  `PublicEntrance.CreateInterior`; Host und Tests verwenden dieselben Daten.
- Geometrische Durchgangsproben prüfen alle festen Boxen aus beiden Plänen,
  einschließlich verborgener Deckenplatten, an drei seitlichen Spuren in beiden
  Richtungen. Eine alte Collider-Fixture reproduziert die Standblockade und lässt
  einen ausdrücklich hypothetischen kürzeren Körper passieren.
- [Rote Tests](evidence/clearance-red.trx): 5 fehlgeschlagen; [finale Suite](evidence/clearance-final.trx):
  **100 bestanden**, 0 fehlgeschlagen/übersprungen. No-Deploy-Release: 0 Warnungen,
  0 Fehler. Fokussierte Formatprüfung von PublicEntrance, PublicHallHost,
  FacadePortal und FacadePortalTests bestanden; kein pauschales Repo-Grün.
- [Quell-/Artefaktmanifest](evidence/clearance-source-hashes.json): 33 Quellen,
  DLL `72bbc8073db2d8240998a5a536b6a1023b65b7b2e317cd03fc0161ab1ca5fc87`.
- Isoliertes Pure für diesen Stand vollständig ausgeführt: Rathaus **100/100**
  bestanden. [Gesamtbericht](evidence/clearance-pure-report.json): 17 Prüfschritte
  bestanden, 5 fehlgeschlagen, Exit 1. Offen sind Format-/Solution-/Versionsdrift
  und Git-Fehler in `tool-clean-checkout.Tests` / `tool-workspace-setup.Tests`;
  keine Reparatur fremder Werkzeuge/Mods in dieser Aufgabe. Ein erster Snapshot
  hatte parallel laufende Dokumentationsänderungen korrekt erkannt und wurde
  abgebrochen; nach Ende der Schreibarbeiten wurde der isolierte Lauf wiederholt.

Unabhängiger Read-only-Delta-Review `deleg_b1e52166`: kein weiterer belastbarer
Blocker. Der Prüfer bestätigte den vorherigen unsichtbaren Collider, den exakten
Backup→Arbeitsstand-Diff, 33/33 Quellhashes sowie DLL-/TRX-Identität. Die neuen
Tests wurden dabei gelesen und deren Artefakte geprüft, nicht erneut ausgeführt.

Installiert am `2026-10-29T04:33:23.7478612Z` bei geschlossenem Spiel:
DLL `72bbc8073db2d8240998a5a536b6a1023b65b7b2e317cd03fc0161ab1ca5fc87`.
DLL und PDB ersetzt, Metadaten bytegleich belassen; alle drei Zielhashes unabhängig
zurückgelesen. Shared unverändert. Vorherige DLL `641511a9…` und PDB liegen im
hashgeprüften Backup `20261029T043323595Z-TownHallInterior-e4bf0d3f`.
[Unveränderliche Deploy-Quittung](evidence/clearance-deploy-20261029.json), Versuch
`20261029T043323556Z-adeb5a1fd0c1`, SHA-256
`bb87d76e5bf4ae5560d40b79a967d5d9506d6c9d32cd17f7e68942a01ef95560`.
Keine Spielbedienung, keine Änderungen an Spielständen, kein Commit/Push.

Grenze: Diese Proben sind reine diskrete Kapsel-/Boxgeometrie, keine native
CharacterController-Stufensimulation. Der aktuelle Spielermaßstab, native
Zugangskollision und einzelne Bewegungsabläufe sind damit nicht vermessen.

### Eigentümerbestätigung 2026-10-29

Dominik bestätigte nach dieser Installation im Gespräch: **„Ja man kann durchlaufen“**.
Damit ist das gemeldete Durchgangsproblem eigentümerbestätigt abgeschlossen.
Die Rückmeldung itemisiert keine separaten Ein-/Austritts-, LOD-, Reload- oder
NPC-Tests; keine zusätzlichen Messwerte oder Einzelabnahmen daraus ableiten.
Präzisierung von Dominik: **„Der Rahmen geht zwar noch bis zur Stirn, aber man kann
ohne probleme durchlaufen rein visuell“**. Der Durchgang bleibt funktional
akzeptiert. Offen ist nur die sichtbare Rahmenhöhe; die Optik ist nicht abgenommen.
Beim Innenausbau den sichtbaren Rahmen an die Öffnung anpassen, ohne den
funktionierenden Kollisionsdurchgang oder die Spielergröße zu verändern.
Nächster besprochener Ausbau: Erdgeschoss-Innenhülle, danach Empfang/Wartebereich
und Beleuchtung. Dieser Vermerk allein behauptet noch keine Umsetzung des Ausbaus.

Basisrevision: `94a42c5b41abc9e403793de238a75688726681db`, Arbeitsstand dirty/untracked.
Die konkrete Quellfassung ist nicht durch den Basis-Commit allein identifiziert.
