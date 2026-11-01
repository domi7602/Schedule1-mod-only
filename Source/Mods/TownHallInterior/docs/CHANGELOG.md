# Changelog

## 2026-10-30 — Innenausbau abgenommen

Dominik hat den Innenausbau insgesamt bestätigt („ja die interior ist fertig“): Eingang,
Erdgeschoss und Obergeschoss. Installierter Stand `7681d319…`, 145/145 Tests.
Offene Erweiterungen: Aushang-Beschriftung, Möbel außerhalb des Vanilla-Satzes,
NPC-Navigation, Deckenwirkung im OG; Keller verworfen.

## 0.1.0 (2026-10-30) — Pulte mit der offenen Seite zum Stuhl

- Nach dem Drehen der Stühle blieb der eigentliche Fehler: Die Stühle standen auf der
  **geschlossenen** Seite des Pultes. Am Asset gemessen: bei Kniehöhe liegen auf lokal +X nur
  10 Vertices (nur an den Enden), auf −X 240 über die ganze Länge → +X ist der Knieraum, −X die
  Blende; yaw bildet lokal +X auf (cos yaw, −sin yaw) ab.
- Bürozeile auf yaw 0 und z 30,6/33,2/35,8, Stühle bei x 57,9 (yaw 180), Besprechung als zwei
  gespiegelte Pulte mit je einem Stuhl auf der offenen Seite. **Empfangstresen im EG ebenfalls
  gedreht** (yaw 180 → 0), damit der Knieraum zum Personal zeigt.
- Neuer Test `EveryChairSitsInAKneeSpaceAndFacesThatDesk` (Arbeitsplatzstuhl vor offener Seite
  und zurückblickend; freistehende Wartestühle ausgenommen). 145/145, installiert `7681d319…`,
  Backup `20261030T140642055Z-TownHallInterior-650a4ec4`.

## 0.1.0 (2026-10-30) — Stühle schauen zum Tisch

- Die Blickrichtung des Stuhl-Assets war nie abgeleitet worden; die OG-Stühle standen 180° verdreht
  und schauten vom Schreibtisch weg. Konvention aus dem abgenommenen Erdgeschoss hergeleitet
  (Empfangsstuhl yaw 180 → zum Tisch, Formularstuhl yaw 270 → zum Tisch): **0 = +X, 90 = −Z,
  180 = −X, 270 = +Z**. Bürostühle 270 → 180, Besprechungsstühle 90/270 → 0/180, Südstühle 0 → 270.
- Neuer Test `EveryChairFacesTheDeskItBelongsTo`: für jeden Stuhl muss das Skalarprodukt aus
  Blickrichtung und Richtung zum nächstgelegenen Tisch desselben Geschosses > 0,35 sein.
  145/145, installiert `a341a42b…`, Backup `20261030T135521813Z-TownHallInterior-3198f58c`.

## 0.1.0 (2026-10-30) — Obergeschoss nachgelegt

- Zwei Teppiche (`upper-rug-south`, `upper-rug-meeting`), vier Aktenschränke und zwei Stühle an
  der Südwand, Papierkorb, zwei große Eckpflanzen sowie zwei Monitore auf dem Besprechungstisch.
  Gesamt 57 Platzierungen, 2 Teppiche.
- Tests prüfen jetzt beide Geschosse (Hüllgrenzen, Standhöhe, Desktop-Objekte, Treppenloch und
  Portal frei): 144/144, installiert `b826848d…`, Backup `20261030T062905165Z-TownHallInterior-6d2ea472`.

## 0.1.0 (2026-10-30) — Keller verworfen, Obergeschoss eingerichtet

- **Keller zurückgebaut.** Der Test am Morgen zeigte die Gelände-Mesh im Treppenschacht
  (RGB ≈ 5/34/32): Hallenboden 1,53 entspricht Blender 0,65, also Versatz ≈ +0,88 → Gelände bei
  y ≈ 0,69, nur 84 cm unter dem Boden. Eine offene Treppe kann diese Fläche nicht umgehen;
  Verdecken würde die Treppe blockieren, Ausschneiden hieße ein geteiltes Native-Mesh ändern.
  `BasementPlan`/`BasementPlanTests` entfernt, Boden wieder eine Platte, Kellerlichter und
  Keller-Einrichtung raus. Umfang laut Dominik: Erdgeschoss und Obergeschoss.
- **Obergeschoss eingerichtet** (48 Platzierungen): drei Büroarbeitsplätze (Schreibtisch, Stuhl,
  Monitor, Tischpflanze) an der Westwand, Besprechungstisch aus zwei Schreibtischen mit vier
  Stühlen, fünf Aktenschränke als Archiv an der Ostwand nördlich des Treppenlochs, zwei große
  Eckpflanzen, Papierkorb und Wanduhr. Einbauten des EG unverändert.
- **Tests:** 144 Fälle bestehen (Standhöhe EG/OG, Desktop-Objekte auf Tischen, keine
  Überlappungen, Portal und Treppenloch frei). Installiert `abaa9c2f…`, Backup
  `20261030T044742763Z-TownHallInterior-325e2698`. Sichtprüfung des OG offen.

## 0.1.0 (2026-10-29) — Keller unter dem Erdgeschoss

- **Machbarkeit:** Sonden in der privaten Blender-Map (read-only) zeigen unter der Südhälfte
  des Rathauses freien Raum zwischen ≈ −0,28 und −5,14 (Blender-Z); die Sewer-Flächen liegen
  bei −5,14/−5,54/−6,48 bzw. −16,2. Die Gelände-Mesh liegt über dem Keller und schneidet ihn nicht.
- **Keller** (`BasementPlan.cs`): x 55,0–62,2 · z 22,0–34,0, Boden −2,90, Deckenunterkante −0,55
  (2,35 m licht), Wände 0,20 m. Gerade Treppe mit 15 Tritten an der Westwand, Öffnung im
  EG-Boden x 55,0–56,35 · z 23,0–27,5; der Boden ist dafür in `public-floor-south/north/east`
  geteilt. Schachtwände gegen Einblick, vier Kellerlichter, sechs Aktenschränke, zwei Körbe.
- **Tests:** 157 Fälle, elf neue in `BasementPlanTests` (Sewer-Abstand, lichte Höhe, Stufenmaße,
  freies Treppenloch, Dichtheit, Schachtwände, Portal/Treppenloch frei) plus Läufer-Freigang.
  Installiert `211f112a…`, Backup `20261029T204537778Z-TownHallInterior-1f232624`.
- **Offen:** Sichtabnahme, Gelände-Kollision beim Abstieg, NPC-Navigation, Sewer-Anbindung.

## 0.1.0 (2026-10-29) — Erdgeschoss-Ausbau, Runde 1

- **Neue Einbauten** (`HallFittings.cs`, prozedural, keine neuen Vanilla-Assets):
  Info-Tresen gegenüber dem Eingang (x 57,2–58,1 · z 29,6–32,4, 2,50 m + 7 cm Platte,
  Sichtblende bis 3,05 m), drei Sitzbänke an der Südwand (je 2 m, Sitzhöhe 0,43 m),
  bordeauxfarbener Läufer ohne Kollision, Aushangkasten rechts neben der Tür, zwei auf
  1,8-fach skalierte Eckpflanzen.
- **Wartebereich umgestellt:** die acht losen Stühle entfallen, die Bänke ersetzen sie
  (Dominik-Entscheidung B). Vier Stühle bleiben an Empfang und Formularen.
- **Technik:** `FurniturePlacement` hat ein `Scale`-Feld (Platzierung skaliert jetzt den
  Wurzelknoten); die gemeinsamen Paints umfassen fünf Farben (dunkles Holz, Bordeaux);
  `DecorativeBox` behält den Collider für solide Einbauten und entfernt ihn nur bei Hüllflächen.
- **Tests:** 144 Fälle, davon sieben neue in `HallFittingsTests` (Passung, freies Portal,
  freies Treppenloch, Kollisionsflags, Laufwege am Tresen, Bankhöhe). Installiert
  `73cf3bdf…`, Backup `20261029T202623707Z-TownHallInterior-395a8f33`. Sichtprüfung offen.

## 0.1.0 (2026-10-29) — Nachtrag 5: Natives Klopfen bleibt erhalten

- **Befund (live gelesen):** An Jens `StaticDoor` stand `CanKnock = false` (von der Mod) und
  `Usable = true` (Fix aus Nachtrag 3). Dominik erklärt die Spielmechanik: NPCs despawnen,
  sobald sie ein Gebäude betreten, und ein Klopfen holt sie zurück. Unsere Knock-Unterdrückung
  hat damit den einzigen Weg zur Schalterin entfernt.
- **Änderung:** `SuppressPlayerKnock` ersetzt durch `HideNativeDoorSurface`. Die Mod versteckt
  nur noch die Native-Tür-Renderer; `CanKnock`, `Usable` und das `IntObj` bleiben unverändert,
  es wird nichts mehr restauriert, weil nichts mehr verändert wird.
- **Tests:** 138 Fälle; der Quellwächter verbietet jetzt sowohl `.Usable =` als auch
  `.CanKnock =` im Host. Installiert `6c39bd36…`, Backup `20261029T195551234Z-TownHallInterior-337a010c`.
- **Folge:** Am Rathaus erscheint wieder der native „E Knock"-Prompt. Die Halle bleibt frei
  begehbar; beides läuft nebeneinander.

## 0.1.0 (2026-10-29) — Nachtrag 4: Schiebetür blockiert keine NPCs mehr

- **Zweite Ursache für Jens Problem:** Der Näherungssensor liest nur `Player.Local`. Für NPCs
  bleiben die Flügel geschlossen, und das Panel hatte einen Collider – Jen stand vor einer
  zugezogenen Tür.
- **Änderung:** Kein Blattteil ist mehr solid; die Flügel sind reine Ersatzgrafik für die
  versteckte Native-Tür. Der Test `LeafPartFacesNeverShareAPlane…` prüft jetzt zusätzlich, dass
  kein Teil kollidiert.
- **Tests:** 138 Fälle bestehen. Installiert `6393fddf…`, Backup `20261029T194158847Z-TownHallInterior-50a27802`.
- **Offen:** Eine sichtbare Öffnung für NPCs (Sensor auf Nicht-Spieler erweitern) ist bewusst
  nicht enthalten; erst die Live-Prüfung soll zeigen, ob Jen ohne sie hineinkommt.

## 0.1.0 (2026-10-29) — Nachtrag 3: Türbenutzbarkeit für NPCs erhalten

- **Befund:** In der Sitzung um 20:29 warf `NPCEvent_StayInBuilding.OnActiveTick` 50-mal in
  zwölf Sekunden eine `NullReferenceException`. Die Mod setzte an Jens `StaticDoor` bisher
  `CanKnock=false` **und** `Usable=false`; `Usable` steuert, ob die Tür überhaupt funktioniert,
  und Jen wohnt in diesem Gebäude.
- **Änderung:** `Usable` wird nicht mehr angefasst und nicht mehr restauriert. Unterdrückt
  bleiben nur der Spieler-Knock (`CanKnock`) und der Interaktions-Prompt (`IntObj`), Renderer
  der Native-Tür bleiben versteckt.
- **Tests:** 138 Fälle, neu ein Quellwächter, der eine erneute `Usable`-Zuweisung im Host
  verhindert. Installiert `0ed42b5c…`, Backup `20261029T193738624Z-TownHallInterior-6bc522de`.
- **Grenze:** Der Zusammenhang ist aus Log und Code abgeleitet, nicht live bestätigt. Die
  nächste Sitzung muss zeigen, ob die Meldung ausbleibt und Jen wieder ins Gebäude kommt.

## 0.1.0 (2026-10-29) — Nachtrag 2: Restspalt über der Tür und flackernder Rahmen

- **Kopf-Laibung:** `inner-lintel` endet jetzt bei `HeadLiningBottom` (4,54) und beginnt bei
  `LiningFrontX` (54,16), überlappt also die Blattoberkante und -ebene um je 1 cm. Das schließt
  das letzte 1-cm-Fenster, durch das man beim Aufblicken Himmel sah.
- **Z-Fighting:** Die Blattteile liegen als testbare Daten in `PublicEntrance.LeafParts`.
  Rahmenprofile sind 1 mm dünner als die Stile, Stile und Panel enden unter der Rahmenoberkante,
  keine zwei sichtbaren Flächen teilen sich eine Ebene (Ursache des Flackerns).
- **Tests:** 137 Fälle bestehen (neu: Ebenen-Regression für die Blattteile). Installiert
  `3a82f9e8…`, Backup `20261029T173935879Z-TownHallInterior-fc354400`.
- **Abnahme:** Dominik hat den Eingang nach diesem Deploy optisch abgenommen („tür sitzt jetzt
  perfekt“). Gilt für den Sichtfit des Eingangs, nicht für Decke, obere Hülle, Treppe, Möbel,
  LOD, NPC-Navigation oder Multiplayer.

## 0.1.0 (2026-10-29) — Nachtrag: Laibungstiefe und Innenzarge

- **Sealed entrance:** Die Flügel sind auf die sichtbare Laibung bemessen (je 1,00 m, Abdeckung
  30,00–32,00, Berührung bei 31,00) und tragen ein Rahmenprofil; eine render-only Innenzarge
  fasst den Durchgang.
- **Deep reveal:** `inner-lintel` und `reveal-south`/`reveal-north` reichen bis zur
  Fassadenebene (x 54,18), weil der native Fassadenschnitt dreiecksbedingt über das geplante
  Rechteck hinausgeht und sonst ein Aufwärtsblick über die Blattoberkante die Außenwelt zeigt.
- **Tests:** 136 Fälle bestehen; installiert `28efce74…`, Backup
  `20261029T173104129Z-TownHallInterior-af82f3e5`. Ingame-Sichtprüfung offen.

## 0.1.0 (2026-10-29) — Local experimental public hall

Initial version of the local staging build. The Town Hall facade opens into a walkable
public reception hall: geometric portal, proximity-driven sliding entrance, vanilla
reception furniture and eight local lights. No furniture interactions, no upper-floor
furniture and no business. The embedded native slices are local, hash-pinned extractions
of the f12 scene and are **not distributable**.

- **In-Game-Verify open:** the geometric public hall and the sliding entrance are
  implemented and game-free tested, but the in-game visual acceptance (outside/inside,
  door open/closed, day/night, entering and leaving without jitter, sealed leaf/reveal
  joint) is still open and lies with Dominik. Candidate `5efb8650…` did load on
  2026-10-29 16:57 and published the hall and the reception (26 instances, 8 lights) per
  the MelonLoader log; that is a load signal, not a visual acceptance. The current
  candidate `7ffcf24b…` (sealed sliding leaves plus inner casing) was installed at
  17:53 local with the game closed and has not been launched yet.
- **Repository integration:** the mod and its test project are now part of
  `Source/Mods/S1Mods.sln`, regenerated with `Tools/gen-sln.ps1`. Before that they were
  invisible to the solution build, to the repository format gate and to CI. `RoomPlan.cs`
  and `RoomPlanTests.cs` are whitespace-normalized (space after comma, space around
  binary operators); no coordinate, box, name or collision behaviour changed.
- **Door dialog archived:** the staged, never-wired door-choice subsystem
  (`DoorSelector`, `DoorModalHost`, `ExclusiveModalFocus`, `NativeContextMenuRuntime`)
  and its four test files moved to `Source/Archive/TownHallInterior/DoorDialog/`. It had
  no production instantiation: `DoorHook` hard-wires `UnavailableDoorSelector` and always
  passes the native interaction. Runtime behaviour is unchanged.
- **Tests:** 128 game-free test cases remain in the active suite; 169 before the archive
  move. No-Deploy Release build of the solution: 0 errors, 0 warnings.
  `Tools/gen-sln.ps1 -Check` and `dotnet format --verify-no-changes` pass.
- **Release path:** `Tools/package-release.ps1` selects this mod by directory, not through
  the solution, and would package the embedded native slices. No release archive may be
  produced from this mod.
