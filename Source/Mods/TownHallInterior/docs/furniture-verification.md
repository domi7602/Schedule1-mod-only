# Vanilla-Einrichtung des Rathaus-Erdgeschosses

Prüfdatum: 2026-10-29. Lokaler Entwicklungskandidat, keine Veröffentlichung.
**Historischer Einrichtungsstand installiert und hashgeprüft; Teilrückmeldung erhalten.** Der bereits von Dominik
akzeptierte Durchgang bleibt funktional abgeschlossen; diese Erweiterung ist eine
separate Einrichtung und visuelle Rahmenkorrektur.

Dominiks spätere Screenshots bestätigen sichtbare Möbel und Lichtwirkung, zeigen
aber Hüllenfehler. Seine Meldung zur innen offenen Tür wurde als zu breite
Öffnungszone reproduziert. Die anschließende Korrektur hat einen eigenen
[Tür-/Hüllennachweis](enclosure-verification.md); die folgenden 117-Test-,
Vier-Licht- und Deploymentangaben bleiben dem damaligen Kandidaten zugeordnet.

## Umfang

- Zwei Empfangsarbeitsplätze mit originalem Ornate Desk, Chair_Business und
  OldComputer einschließlich Tastatur, Maus und Mauspad.
- Acht Wartestühle; ein weiterer Schreibtisch mit zwei Stühlen als Ausfüllplatz.
- Zwei Aktenschränke, drei kleine Tischpflanzen, zwei Papierkörbe, eine Wanduhr.
- Insgesamt 26 Modellinstanzen einschließlich des angepassten Original-Türrahmens.
- Vier eigene Punktlichter und eine separate, nicht kollidierende Decken-/Wandkrone.
  Decke und Lampengehäuse sind eigene einfache Geometrie; die Möbel sind Vanilla.
- Drei alte Blockmöbel entfernt. Die weitere Einrichtung des Obergeschosses ist
  nicht Teil dieses Schritts; Treppen-/Obergeschoss-Abnahme bleibt gesondert.

Die Modelle sind **Dekoration**: keine NPCs, Sitzfunktion, Inventare, Kaufoberflächen,
Businessregistrierung oder Spielstandeinträge. Die Uhr hat feste Originalzeiger.
Eingangs-/Spieler-/Treppenkollision wurde nicht umgebaut; nur Möbel bekommen eigene
konservative Boxcollider. Decke, Tischdeko, Uhr und Ersatzrahmen haben keine Collider.

## Herkunft und Besitz

`build/extract-native-furniture.py` extrahiert ausschließlich attribuierte
Rendererbeiträge aus der privaten lokalen Spielinstallation f12. Es prüft die
Quelldateien und einen externen Vertexbufferbereich vor und nach der Extraktion.

- Fixture: `Source/Mods/TownHallInterior/assets/native-furniture.json`, SHA-256
  `687fa231785b3be8938d8c080bddb1161ac468191ef26450b6ac0504769a79aa`.
- Acht Modelle, 19 Teile, 6.259 Vertices, 5.272 Dreiecke, 35 Materialslots.
- Schreibtisch, Aktenschrank und PC: `Map/Hyland Point/Region_Downtown/RE Office/Interior`.
- Stuhl und Pflanze: `@Businesses/Hyland Medical/Interior`.
- Papierkorb: `Map/Hyland Point/Region_Docks/Barbershop/Interior/Small Bin`.
- Uhr: `@Businesses/PostOffice/Interior/Wall Clock`.
- Rahmen: `Map/Hyland Point/Region_Downtown/TownCenter/TownHall/townhall/DoorFrame`,
  nativer Renderer 329674. Vollständige Pfade und Slots stehen in der Fixture.

`VanillaFurnishings` kopiert keine GameObjects oder Spielskripte. Die Mod erzeugt
19 eigene Meshes, teilt diese zwischen ihren Instanzen und leiht native Materialien
nur lesend. Donorpfad, Meshname, Vertexanzahl, Batchzuordnung und Materialslots werden
geprüft. Statische Quellen sowie der tatsächlich ersetzte Rahmen werden zusätzlich
über Bounds geprüft. Nur die zwei exakt benannten Uhrzeiger sind von dieser
Poseprüfung ausgenommen, weil ihre Lage zeitabhängig sein kann; Mesh-/Material-
Identität bleibt vorgeschrieben. Änderungen ihrer Batch-/Meshidentität bleiben
fail-closed. Quelle und Liveverhalten der Uhr sind nicht gleichzusetzen.

Verlegte Modelle übernehmen keine Quell-Lightmaps (`lightmapIndex=-1`). Der Rahmen
wird erst nach vollständigem Aufbau verborgen; seine genaue Originalinstanz und
vorherige Sichtbarkeit werden restauriert. Eigene Ressourcen sind unmittelbar
registriert, Verbraucher werden vor Mesh-/Materialfreigabe getrennt. Ein Fehler
im optionalen Einrichtungsaufbau behält den vorhandenen öffentlichen Eingang.

**Nicht verteilen:** Fixture und die DLL mit eingebetteter Originalspielgeometrie.
Keine privaten Blender-Dateien wurden gespeichert oder hochgeladen.

## Tatsächliche Prüfungen

- Parent und unabhängiger Reviewer führten den Extraktor mit `-B --check` aus:
  bytegleiche Regeneration. Der Extraktor-Agent verglich zusätzlich indizierte
  Attribute/Dreiecke gegen UnityPys separaten MeshHandler und prüfte beschädigte
  Quelle/Fixture; diese weitergehenden Ergebnisse bleiben dem Agenten zugeordnet.
- Rote Tests belegten fehlende Innenhülle, ungültige Datenannahme, den alten sichtbaren
  Rahmen, fehlendes Layout/Wiring und die verbliebenen Blockmöbel. Ein weiterer
  Test reproduzierte eine 5-cm-Deckenfuge: Deckenränder werden jetzt aus den tatsächlichen
  eigenen Innenwandflächen statt aus der etwas kleineren Bodenplatte abgeleitet.
- **117/117 gezielte verwaltete Tests bestanden**, Release-Build ohne Deployment,
  0 Warnungen/0 Fehler. Fokussierte Formatprüfung der neuen Dateien und des Host-Wirings
  bestanden. Vorhandene kompakte Formatierung anderer Dateien wurde nicht umgeschrieben.
- Der Layouttest transformiert sämtliche tatsächlichen Modellvertices, prüft Raumgrenzen,
  Überschneidungen, Auflage von PC/Pflanzen sowie freie Eingangs-/Treppenbereiche.
- Der originale Frame-Querbalken wurde im Test als visuelle Blockade reproduziert;
  das vorbereitete eigene Mesh gibt die Portalbreite und Höhe bis `PortalTop=4.55`
  frei. UVs/Normalen/Tangenten bleiben erhalten; die originale Fixture bleibt unverändert.
- Unabhängiger statischer Erstreview (`deleg_b02d54a5`): 37/37 Quellen/Assets und DLL
  bestätigt, kein belegter Sicherheits-/Logikblocker. Der separate Delta-Review
  (`deleg_cf35f244`) bestätigte anschließend 38/38 Quellen/Metadaten, DLL/PDB,
  die 117-Testquittung und beide Decken-/Uhr-Korrekturen, ebenfalls ohne neuen Blocker.
  Keine Unity-Abnahme daraus ableiten. Der Delta-Fingerprint verwendet Dateibytes,
  nicht den LF-normalisierten Text des ursprünglichen Reviewdiffs.
- Isoliertes Gesamt-Pure-Gate des Erstkandidaten: 17 Schritte bestanden, 5 fehlgeschlagen
  (Format, Solution, Versionsabgleich, Git-Werkzeugtests). Kein grünes Gesamtrepository.
- Das Pure-Gate des finalen Codekandidaten bestätigte erneut 117/117 Rathaus-Tests,
  endete aber mit 16 bestandenen/6 fehlgeschlagenen Schritten. Zusätzlich zu den fünf
  bekannten Gates waren zwei neu geschriebene Doku-Pfade falsch. Diese wurden korrigiert;
  der anschließende lokale Doku-Pfadcheck bestand. Der archivierte Pure-Bericht wird
  nicht nachträglich als erfolgreich umgedeutet.

## Verifizierte lokale Installation

- Zeitpunkt: `2026-10-29T06:10:05.7704352Z`.
- DLL-SHA-256: `bbe5a5c0137451ae3715a8f0ca5a022ca1d61159b1c527f82bc37887a29f4c0e`.
- Regulärer Deployment-Helper ersetzte ausschließlich DLL, PDB und mod.json.
  Alle drei Ziele wurden anschließend unabhängig gegen Quelle/Fingerprint gehasht.
- Vorheriger akzeptierter Stand `72bbc807…` sowie PDB/Metadaten wurden über das genaue
  Backupmanifest gegen die vorab gemessenen Bytes geprüft; Shared blieb unverändert.
- [Quellfingerprint](evidence/furniture-source-hashes.json),
  [Installationsquittung](evidence/furniture-deploy-20261029.json),
  [Backupmanifest](evidence/furniture-backup-manifest.json),
  [117 lokale Tests](evidence/furniture-tests.trx),
  [117 isolierte Tests](evidence/furniture-isolated-tests.trx) und
  [Pure-Gesamtbericht](evidence/furniture-pure-report.json).

Kein Spielstart, keine Ingame-Steuerung und keine Spielstandänderung durch den Agenten.
Installation ist kein Nachweis der Initialisierung oder visuellen Abnahme.

## Sichtprüfung und Grenzen

Eine separate Blender-Vorschauszene wurde direkt aus dem getesteten C#-Plan und
der extrahierten Geometrie erzeugt. Alle benötigten nativen Materialnamen waren
in der privaten Map vorhanden. Übersicht und Eingangsperspektive wurden tatsächlich
mit EEVEE gerendert; keine offensichtlichen Möbelüberschneidungen. Das ist eine
**Layout-/Modellvorschau**, keine Nachbildung von Unity-Licht, Probe-Verhalten oder
Interaktionen. Belege: [Übersicht](evidence/furniture-layout-cutaway.png) und
[Eingangsperspektive mit korrigierten Deckenrändern](evidence/furniture-entrance-final.png).

Beim späteren Entfernen der ausschließlich temporären Blender-Vorschauszene trat
ein MCP-Timeout auf. Der Abschluss dieser Aufräumaktion ist daher nicht bestätigt.
Blender war danach weiterhin vorhanden und laut Prozessabfrage ansprechbar; die
gespeicherte Original-Map wurde erneut SHA-256-identisch geprüft. Keine erneute
Löschaktion, kein erzwungenes Beenden und kein Speichern des Originals.

Sichtbare Möbel und Lichtwirkung sind durch die späteren Nutzerscreenshots belegt.
Nicht vollständig abgenommen: Materialdarstellung, Rahmen/LOD bei verschiedenen
Abständen, Bewegung zwischen allen Möbeln sowie Menü-/Save-Neuladen. Die zuvor akzeptierte
Durchgängigkeit wird dadurch nicht als altes offenes Problem neu aufgerollt.
Möglicherweise durchragende Zweige äußerer Bäume wurden nicht pauschal entfernt.
Native Fehler-/Unloadpfade und NPC-Navigation bleiben ohne separate Laufzeitprüfung.
