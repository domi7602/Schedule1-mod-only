# Türsteuerung und geschlossene sichtbare Rathaushülle

Prüfdatum: 2026-10-29. Lokaler Entwicklungskandidat, nicht veröffentlichen.
**Gebaut, offline geprüft, unabhängig reviewt und installiert; Ingame-Abnahme offen.**
Die neue Ingame-Sichtprüfung macht Dominik; keine Spielsteuerung durch den Agenten.

## Ausgangsbefund und Abgrenzung

Dominik hatte den begehbaren Durchgang bereits akzeptiert. Seine anschließenden
Screenshots zeigen die Vanilla-Einrichtung und Lichtwirkung, aber auch Spalten am
Eingang, eine senkrechte Eckfuge und eine offene obere Innenansicht. Seine Rückmeldung
„von innen fehlt die Schiebe tür auch“ ist eine Beobachtung, keine gesicherte
Backface-Diagnose. Die damalige Öffnungsbedingung enthielt tatsächlich eine Zone für
fast die gesamte Halle; auch Raummitte und Empfang hielten die Flügel offen.

Belege des **vorherigen installierten** Möbelstands `bbe5a5c0…`:
[Eingang](evidence/enclosure-owner-entrance.png),
[Erdgeschoss](evidence/enclosure-owner-ground.png),
[Ecke/Treppe](evidence/enclosure-owner-corner.png),
[Obergeschoss](evidence/enclosure-owner-upper.png) und
[gefilterte Rathaus-Logzeilen](evidence/enclosure-owner-log-excerpt.txt).
Sie sind keine Abnahme des hier beschriebenen Reparaturstands.

## Änderungen

- **Türnähe statt ganzer Halle:** `PublicEntrance.WantsDoorOpen` wird vom echten
  Host aufgerufen. Die vorhandene Türzone auf beiden Seiten und im Durchgang bleibt;
  Aufenthalt weiter hinten fordert keine Öffnung mehr an. Flügelmaße, Bewegung und
  `PortalTop=4.55` bleiben bestehen. Die Tür ist weiterhin eine eigene lokale
  Näherungssteuerung, kein nachträglich behaupteter nativer Hospital-Controller.
- **Erdgeschoss:** Vier sichtbare Eckfüllungen schließen die jeweils 5 cm breiten
  Fugen auch über den Wandkronen. Zwei Bodenrandstreifen schließen die entsprechenden
  schmalen Seitenfugen. Neue seitliche Laibungen verbinden Rahmen und Innenwände.
- **Sturz:** Der vorhandene eigene `inner-lintel` verbindet nun X54.35–54.95.
  Seine Unterkante bleibt Y4.55; seine Oberkante endet auf Y4.75 statt Y4.85.
  Damit ragt er nicht mehr über den oberen Boden hinaus. Das ändert diesen eigenen
  Collider **oberhalb der erhaltenen Portalhöhe**; die pauschale Aussage „alle
  Collider unverändert“ wäre falsch. Native Fassadenschnitte, Spielermaße, Treppe,
  bestehende Perimetercollider und Möbelkollision sind unverändert.
- **Geschossdecke/Boden:** Die vier vorhandenen dekorativen Deckenboxen reichen jetzt
  von Y4.40 bis Y4.75. Ihre EG-Unterseite und die Treppenöffnung bleiben erhalten;
  zusätzliche Randstücke decken die volle vorhandene obere Auflage ab. Kein neuer
  kollidierender Boden und keine Auflage auf einer niedrigeren unsichtbaren Fläche.
- **Oben:** Dünne Innenflächen folgen den bestehenden oberen Perimetercollidern mit
  rund 2 cm Innenabstand, statt das schmalere EG mit unpassender Kollisionslage zu
  kopieren. Eine Decke schließt oben bei Y8.05–8.13 ab. Das sind gewählte
  Entwurfskoordinaten, keine Messung einer nativen Innenraumdecke. Oben gibt es noch
  keine Möbel oder Interaktionen.
- **Darstellung:** Vier zusätzliche obere Punktlichter, insgesamt acht. Die bisherigen
  vier Empfangslichter behalten Position, Reichweite, Intensität und Farbe. Drei
  eigene Materialien unterscheiden Bodenrand, Decke und Wandfüllungen. Registrierung
  vor weiteren nativen Settern, Verbrauchertrennung vor Materialfreigabe; geliehene
  Vanilla-Materialien werden weder verändert noch zerstört.
- **Überlappungen:** Laibungsstirnflächen stehen etwa 1 mm vor den angrenzenden
  Wandflächen, ihre Oberseiten liegen 1 mm unter dem oberen Boden. Das vermeidet
  dort gleichflächige sichtbare Überlagerungen. Ein geschlossener Teststrahl allein
  wäre kein Nachweis gegen Z-Fighting.

Alle Flächen aus `FurniturePlan.CreateShell()` sind render-only; die Primitive-
Collider werden vor ihrer verzögerten Zerstörung deaktiviert. Die 26 Vanilla-
Modellinstanzen und ihre ursprünglichen Meshdaten bleiben gleich. Keine gemeinsam
benutzten nativen Meshbuffer, Fremdbeiträge oder Bäume werden pauschal verändert.

## Geometrischer Nachweis

Ein unabhängiger, read-only Prüfer (`deleg_697324b9`) extrahierte die tatsächlichen
C#-Pläne sowie die exakten Fassaden-/Rahmendreiecke und reproduzierte 15 Sollfehler:
vier Eckstrahlen, sechs obere Sichtwege, vier Strahlen um den Sturz und die
abweichende sichtbare Bodenhöhe. Der Parent führte dessen Repro selbst aus und
prüfte die Datenbindung. Dessen ursprünglicher Korrekturvorschlag war nur ein
Modell; implementiert wurde insbesondere **nicht** seine tief insetzte obere Wand.

Die unveränderte Reprologik wurde danach in einem getrennten Snapshot auf den
finalen Produktionsplan angewendet: **15/15 Kriterien bestanden**, keine Quelldrift.
210 geschnittene Fassadendreiecke wurden mit ursprünglicher Windung/Normalen
untersucht. Rückseitige Treffer sind nur unter Backface-Culling ein Sichtbeleg;
Shader, LODs und Unity wurden dabei nicht ausgeführt.

- [Vorheriger Geometriebericht](evidence/enclosure-baseline-forensics.json)
- [Finaler Geometriebericht](evidence/enclosure-final-forensics.json)
- [Gepinnte Repro-/Quellsnapshots](evidence/enclosure-forensic-snapshots.zip)
- [Hashes der archivierten Nachweise](evidence/enclosure-evidence-hashes.json)

Die Repro-Snapshots sind privat und enthalten lokale Originalgeometrie. Zur
Wiederholung in eine Arbeitskopie entpacken, den lokalen Repositorypfad prüfen und
nicht im unveränderlichen Nachweisarchiv ausführen.

## Tatsächliche Codeprüfungen

- **163/163 verwaltete Tests bestanden**, einschließlich der bestehenden Tests zu
  Durchgang, Treppe, Möbeln, Parsing, Fassadenschnitt und Cleanup-Policy.
- Neue Fehlerfälle wurden vor der jeweiligen Korrektur rot ausgeführt: Türbelegung,
  Eck-/Kronenfugen, Bodenhöhe/-ränder, Sturzanschluss, Laibungen, obere Hülle,
  Lichtplan, Materialwiring und exponierte Laibungsüberlagerung. Die zusätzlichen
  finalen Schutzprüfungen übernehmen die zuvor roten Forensikstrahlen und prüfen
  das freie Portal-/Treppenvolumen sowie horizontale obere Rundblicke.
- Ein erster Bodenrastertest tastete nach Ergänzung der Wandhaut auch Punkte
  innerhalb dieser Haut ab. Der Test wurde auf Bodenflächen beschränkt; er prüft
  weiterhin die komplette vorhandene Auflage, nicht einen unmöglichen Kamerastand
  in einer Wand. Der ursprüngliche fehlende Randboden bleibt als roter Nachweis erhalten.
- Release-Build mit `S1NoDeploy=true`: **0 Fehler, 0 Warnungen**.
- Fokussierte Whitespace-Prüfung aller geänderten C#-Dateien: bestanden.
- Added-Line-Sicherheitsprüfung: kein Treffer. Produktionsbudget: **20 Dateien**
  einschließlich csproj; keine neue Produktionsdatei oder Assembly.

[Tests](evidence/enclosure-tests.trx),
[Testdefinitionen](../../../Tests/TownHallInterior.Tests/EnclosureTests.cs),
[39-Dateien-Fingerprint](evidence/enclosure-source-hashes.json) und
[genauer Reviewdiff](evidence/enclosure-reviewed.diff).
Die 38 Dateien der Backupbasis wurden gegen den Fingerprint des installierten
Vorgängers geprüft. Basisrevision `94a42c5b41abc9e403793de238a75688726681db`, Branch
`main`, Mod und Testordner weiterhin untracked; kein Commit identifiziert allein
mit diesem Arbeitsstand die tatsächlichen Quellen.

Kandidaten-DLL:
`932de88dc3acdee308e1315ce3a4e05f06a4abf8f8608924d2b3075b648794ed`.

## Unabhängiger Review und isolierte Prüfung

- [Review](evidence/enclosure-review.json) durch `deleg_26c560be`: keine belegten
  Sicherheits-/Logikblocker. 39/39 Eingabedateien, zwei Build-Kontextdateien,
  38 Baseline-Dateien, Diff sowie DLL/PDB/Metadaten und Testartefakt bestätigt.
- Der Reviewer führte zusätzlich [88 gezielte Tests](evidence/enclosure-review-tests.trx)
  aus einer bytegleichen isolierten Kopie aus. Der Parent hat diese TRX nachgelesen.
- Das [isolierte Pure-Gesamtgate](evidence/enclosure-pure-report.json) endete mit
  **17 bestandenen und 5 fehlgeschlagenen Schritten, Exit 1**. Innerhalb dieses
  Snapshots bestanden [163/163 Rathaus-Tests](evidence/enclosure-isolated-tests.trx).
  Alle 39 Kandidateneingaben und beide Build-Kontextdateien wurden gegen dessen
  Snapshotmanifest abgeglichen. Doku- und Skillprüfung bestanden.
- Fehlergruppen: vorhandene S1MCP-Formatierung, Solution-Drift, Versions-/Inventar-
  Abgleich sowie zwei Git-basierte Werkzeugtests (`clean-checkout`, `workspace-setup`).
  Die Git-Tests melden fehlendes Repository/Arbeitsverzeichnis in ihren Fixtures.
  Kein Rathaus-Formatfehler im Gesamtformatlog; keine sachfremde Reparatur und keine
  nachträgliche Behauptung eines grünen Gesamtrepositories.

## Verifizierte Installation

- Zeitpunkt: `2026-10-29T08:17:14.6080720Z`; Versuch
  `20261029T081714399Z-afcf8e52b002`.
- Unveränderten Kandidaten und geschlossenes Spiel unmittelbar vor dem regulären
  Deployment geprüft. Genau DLL, PDB und Metadaten kopiert, keine losen Assets.
- Alle drei Zieldateien anschließend unabhängig zurückgelesen und mit den
  Quell-/Reviewhashes verglichen. Shared blieb hashgleich und wurde nicht installiert.
- Alle drei vorherigen Dateien im expliziten Backupmanifest gegen die vorab
  erfassten Bytes geprüft. Vorgänger-DLL: `bbe5a5c0…`.
- [Installationsquittung](evidence/enclosure-deploy-20261029.json) und
  [Backupmanifest](evidence/enclosure-backup-manifest.json) bytegleich archiviert.

Installation ist kein Nachweis, dass die neue DLL schon im Spiel geladen oder ihre
Darstellung abgenommen wurde. Nachträgliche Doku-/Quittungsergänzungen sind nicht
Teil des zuvor geprüften Pure-Dokumentationssnapshots; Code und Metadaten bleiben
an den unveränderten 39-Dateien-Fingerprint gebunden.

## Noch benötigte Ingame-Sichtprüfung

Der geprüfte Reparaturstand ist installiert. Ausschließlich `SaveGame_2 / TownHallTest`:

1. Von draußen und innen zur Tür gehen; im Durchgang bleibt sie offen. Weiter in
   die Halle gehen und zurückblicken: geschlossene Flügel müssen erkennbar sein.
2. Eingang/Sturz und vier Ecken aus mehreren Blickwinkeln auf Himmelsspalten und
   flimmernde Anschlüsse prüfen. Der akzeptierte Durchgang darf nicht schlechter werden.
3. Treppe hinauf und wieder hinunter; oben Boden, vier Wände, Decke und Licht
   ansehen. Verbliebene Zweige wären getrennt zu attribuieren, nicht pauschal zu löschen.
4. Für eine spätere Lifecycle-Abnahme Menü/Neuladen und Sicht aus größerem Abstand
   prüfen. Dies ist noch keine bestätigte NPC-, LOD- oder Cleanup-Laufzeitabnahme.

Keine Spielstarts, Eingaben, Bridge-Aufrufe, Blender-Änderungen oder Spielstand-
Schreibzugriffe durch den Agenten in diesem Reparaturschritt. Kein aktueller
Blender-Render oder Ingame-Erfolg wird aus den geometrischen Tests abgeleitet.

## Schwelle (public-threshold) - Kandidat 2026-10-29 10:11 UTC

Problem: Der wanddicke Durchgang (x 54,34..54,951, z 30,02..31,98) war nicht
ausgelegt; natives Gelaende lag bei ca. y 1,40 und zeigte sich als gruene
Wiesenstrippe unter dem Durchgang. Fix: eine eigene solide Box in
PublicEntrance.CreateInterior() mit Oberkante exakt FloorTop (1,53), Stil 0
(Bodenfarbe der Inneneinrichtung), Unterseite bei 1,36 (4 cm unter den
Laibungen begraben). Anschluss an die Laibungen (z 30,02/31,98) und an den
Bodenrandstreifen (x 54,951) ist fugenlos; die Schiebetueren bei x 54,08..54,16
werden nicht beruehrt. Schritt hoehe bleibt 13 cm, nur die Stelle wandert 60 cm
nach aussen (natives Steigen nicht simuliert).

Aenderungen: PublicEntrance.cs (+1 Box), Tests: Portalvolumen-Untergrenze auf
FloorTop, FacadePortalTests/FurniturePlanTests 7 -> 8, neue Tests
EntranceThresholdFinishesTheRecessBetweenTheRevealLinings und
OwnedThresholdClosesTheRecessFloorAcrossTheOpening (5 Senkrecht-Rays),
Kapsel-Test filtert Gehflaechen. Teststand: 169/169 gruen, No-Deploy-Build gruen.

Deployment: DLL SHA-256 5efb8650156b13f852894c8cf26db2b71854ecb342b6efc42f96a7f7d1923641,
Quittung UserData/.deploy/receipts/TownHallInterior.json (2026-10-29T10:11:32Z).
Ingame-Abnahme ausstehend (SaveGame_2 / TownHallTest): Aussen/innen, Tuer
offen/geschlossen, Tag/Nacht, Ein-/Ausgehen ohne Ruckeln.

## Nachtrag: Repository-Integration und Archivierung (2026-10-29)

Nach der Schwelle-Installation am selben Tag hat sich der Quellensatz geändert. Die
vorstehenden Abschnitte bleiben unverändert und beschreiben den Kandidaten
`5efb8650…` mit dem 39-Dateien-Fingerprint; dieser Fingerprint gilt **nicht** mehr für
den Arbeitsbaum.

- `RoomPlan.cs` wurde whitespace-normalisiert (Space nach Komma, Space um binäre
  Operatoren). Keine Koordinate, Box, kein Name und kein Kollisionsverhalten wurde
  geändert; die Geometrie ist identisch.
- Der nie verdrahtete Tür-Dialog wurde nach
  `Source/Archive/TownHallInterior/DoorDialog/` verschoben (`DoorSelector`,
  `DoorModalHost`, `ExclusiveModalFocus`, `NativeContextMenuRuntime` und die vier
  zugehörigen Testdateien). Die Mod bleibt aktiv; `DoorHook` verdrahtet unverändert
  `UnavailableDoorSelector`, das Laufzeitverhalten ändert sich nicht.
- `Source/Mods/S1Mods.sln` wurde mit `Tools/gen-sln.ps1` neu erzeugt und enthält die
  Mod samt Testprojekt. `Tools/gen-sln.ps1 -Check` und
  `dotnet format --verify-no-changes` bestehen.

Stand des Arbeitsbaums danach: **128/128** verwaltete Tests, No-Deploy-Release-Build
der Solution mit 0 Fehlern und 0 Warnungen, DLL
`aad9113aec80bd8762b599a70264299cc48288fe0dc835d4f2cd30247151eec2`. Die 41 archivierten
Testfälle laufen nicht mehr mit; die Angabe 169/169 bleibt als historischer Wert des
vorstehenden Kandidaten gültig.

Nicht ausgeliefert und nicht neu installiert. Der installierte Stand bleibt
`5efb8650…` und ist weiterhin nicht visuell abgenommen; die Ingame-Sichtprüfung liegt
bei Dominik. Die 41 Testfälle und die Quellnormalisierung sind keine Gameplay-Belege.

