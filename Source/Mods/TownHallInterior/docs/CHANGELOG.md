# Changelog

## 0.1.0 (2026-11-04) — Interfaceprobe beobachtet, Speicherlisten-Registrierung beobachtet

- Folge-Lauf der ISaveable-Interfaceprobe `853a1178…` am 2026-11-04 um 05:24:22 UTC
  auf TownHallTest / SaveGame_2: `failed` in der Vorprüfung, Befund
  `interface-resolved-flags` (eigene injizierte Methode zeigt auf denselben Pointer
  und dieselbe Klasse, ist aber nicht als virtuell markiert; Slot 65535, Token 0).
  Der Aufruf unterblieb; der Versuch ist verbraucht.
  [Bericht](evidence/business-interface-live-flags-20261104.json).
- Flaggenkorrektur `a8d45f2a…` am 2026-11-04 um 06:58:38 UTC auf TownHallTest /
  SaveGame_2: `observed` — genau ein inaktiver `ISaveable.InitializeSaveable`-Aufruf
  (Zähler 0→1), Property-Zähler 1/1, zehn Register und 15 fremde VTables über alle
  Stufen unverändert, eigener Rückbau bestätigt.
  [Bericht](evidence/business-interface-live-observed-20261104.json). Der Menü-Reload
  um 07:00:49 UTC lieferte `skipped / attempt-already-consumed` ohne neue
  Registrierung. [Bericht](evidence/business-interface-live-reload-20261104.json).
- Inaktiver Speicherlisten-Registrierungsmodus (`473064e0…`,
  `inactive-saveable-registration`) am 2026-11-04 um 07:11:05 UTC auf Slot 1
  (Organisation Dominik / SaveGame_1): `observed`, keine Befunde —
  `SaveManager.RegisterSaveable` genau einmal aufgerufen; Saveables Before 404,
  Added 1, Removed true, Remaining 0; `Save()`/`WriteData` unaufgerufen; zehn
  Register, 14 fremde VTables und native Dateien unverändert;
  `WorldPublicationAllowed` und `NativeSaveWriteAllowed` false; eigener Rückbau
  (CleanupVerified, RootDestroyed) bestätigt. Das ist eine umkehrbare
  Speicherlisten-Registrierung, kein Save-/Load-Nachweis und keine Aktivierung.
  [Bericht](evidence/business-manager-live-register-saveable-20261104.json).
  [Vertrag und Grenzen](business-manager-registration-probe.md).
- Phase 1 (Business-Anbindung nachweisen) bleibt offen;
  [Fertigstellungsplan](business-completion-plan.md). Diese Berichte wurden aus
  `UserData/TownHallInterior` hashidentisch nach `docs/evidence/` übernommen.

## 0.1.0 (2026-11-04) — Klassenroute beobachtet, Interfaceprobe vorbereitet

- Korrekturkandidat `59484a66…` besteht den nativen Untertyp-/Property-Dispatch
  um 04:04:16 UTC und die Einmaligkeitssperre beim Menü-Reload um 04:08:48 UTC.
  Eigener Rückbau bestätigt; zehn Register/14 fremde VTables sowie lokale native
  Dateien beider Slots unverändert. [Datierte Originalberichte](business-subtype-probe.md).
- Separater `S1TownHallBusinessInterface`-Modus: genau ein frischer
  ISaveable-Proxyaufruf des vorhandenen eigenen no-base InitializeSaveable-Overrides
  nach Token-, Interface-, Offset-, Target- und Assignabilityprüfung.
  Init-Zähler ausdrücklich0→1; bisheriger Untertypmodus bleibt bei0.
- 411/411 reine Tests und drei NoDeploy-Builds ohne Warnungen/Fehler; Aktivierung
  bleibt an CS1029 gesperrt. [Vertrag und Abnahmegrenzen](business-interface-probe.md).
- Interface-Live-Lauf `cb98bc95…` um 04:53:58 UTC: `failed` vor dem Aufruf,
  Befund `interface-target-or-counter-invalid`, kein gespeicherter Einzelwert.
  Property-Dispatch und Rückbau bestätigt; keine native Businessinitialisierung.
- Folge-Kandidat `853a1178…` benennt die erste abweichende Gate-Bedingung und
  behält den abgelehnten Readback. Der Interface-Aufruf bleibt gesperrt, bis die
  Prüfung passt. Live-Lauf steht aus.

## 0.1.0 (2026-11-04) — Begrenzte ClassInjector-Typauflösung vorbereitet

- Erster Untertyp-Live-Lauf `659ee6a2…`: Registrierung scheitert bei
  `FishNet.Connection.NetworkConnection, FishNet.Runtime`, keine Instanz;
  zehn Register und 13 fremde VTable-Fingerprints unverändert.
- Temporäre verwaltete Namenskorrektur für genau den validierten NetworkConnection-
  Proxy, begrenzt auf eigenen Typ, Thread und Registrierungstiefe eins. Originale
  Generic-/ByRef-Auflösung bleibt erhalten. Keine neuen nativen Spielmethodenhooks.
- Eigene Hooks werden auch bei Teilfehlern ownergebunden entfernt und gegen
  fremde Patchbestände geprüft. Unbestätigter Rückbau sperrt die Instanzerzeugung.
  Unveränderte Originalauflösung wird vor/nach dem Scope tatsächlich aufgerufen.
- 352/352 reine Tests bestanden, davon 34 neue Regressionen/Fehlerfälle.
  Aktivierung bleibt compile-blockiert; neuer Live-Nachweis offen.
  [Datierter Vertrag und ursprünglicher Fehlbericht](business-subtype-probe.md).

## 0.1.0 (2026-11-03) — Eigene inaktive IL2CPP-Untertyp-Probe vorbereitet

- Separater Schalter `S1TownHallBusinessSubtype`; Aktivierung bleibt compile-blockiert.
  Eigener Business-Untertyp ohne NetworkObject, keine neuen Spielmethoden-Detours,
  kein Aktivieren, Registrieren in Spielregistern, Speichern oder Spawnen.
- Prozessweite Type-Registrierung nach exaktem Slot-/Loadgate, eigene no-base
  Lifecycle-Overrides und native VTable-/Resolver-Prüfung vor gezielten false-
  Antworten auf ShouldSave/CanBePurchased über frische Property-Basiswrapper.
- Zehn Register und fremde VTables bleiben die gemessenen Vergleichsgrenzen;
  echte Komponentenliste und eigener nativer Rückbau werden separat gelesen.
  Unbekannter Rückbau bleibt ein Fehler, Type-Registrierung bleibt prozessweit.
- 318/318 reine Tests bestanden, darunter 45 neue Failure-Injection-Fälle.
  Kein neuer Spieltest oder Gameplay-Abschluss. [Vertrag und Live-Abnahme](business-subtype-probe.md).

## 0.1.0 (2026-11-03) — Aktivierungskandidat nach Startabsturz zurückgezogen

- Kandidat `54d37fe5…` scheitert vor dem Slot-Load: NullReferenceException und
  rekursive `CanBePurchased`-Trampolinaufrufe bis zum Stackoverflow. Aktuelle
  native Adressmetadaten belegen drei Doppelziele der neuen Hooks; der true-Stub
  wird außerdem von fremden und statischen Methoden geteilt.
- Geprüfte inaktive Diagnose `52bb577a…` samt PDB wieder installiert und direkt
  hashgeprüft. 39 native Slot-2- und 38 Slot-1-Dateien gegenüber Checkpoint unverändert.
- Aktivierungs-Buildsymbol durch `#error` gesperrt. Kein neuer Aktivierungskandidat
  und kein Spielstart. Eigene IL2CPP-Untertypen müssen zuerst separat inaktiv auf
  sicheren Dispatch geprüft werden. [Befund und nächster Schritt](business-activation-probe.md).

## 0.1.0 (2026-11-03) — Kontrollierte Aktivierungs- und Registrierungsprobe

- Inaktive Konstruktion auf TownHallTest bestanden: eigene native BusinessData-
  Serialisierung, bestätigter Rückbau und zehn unveränderte Register (14:04:58 CET).
- Neue Diagnose aktiviert eine eigene Business-/NetworkObject-Instanz und trägt
  denselben Business-Pointer genau einmal in vier native Register ein. Getrennte
  Generationen, Folgeframe-/Guardprüfung, vollständiger eigener Rückbau vor Save
  oder Weltwechsel. Unklarer Rückbau sperrt die nächste Generation.
- Native Awake-/Start-/Netzwerkinitialisierung und Zerstörungsrümpfe bleiben für die
  eigene Instanz gesperrt; der Adapter besitzt die Registrierung. Kein Kauf,
  Geldwäsche- oder nativer Persistenznachweis. [Vertrag und Test](business-activation-probe.md).

## 0.1.0 (2026-11-03) — Business-Diagnose und inaktive Konstruktion

- Schema-2-Inventur besteht Erstladen und Menü-Reload auf TownHallTest: getrennte
  Berichte mit je 90 Ereignissen/45 vollständigen Paaren, keine verworfenen Daten;
  normalisierte Inventuren unverändert. Keine Business-Erzeugung daraus abgeleitet.
- Separater Kandidat `52bb577a…`: echte inaktive Business-/NetworkObject-Shell mit
  eigenen Referenzen, begrenzter GetSaveString-Auswertung und geprüftem Rückbau.
  Genau ein Versuch pro Prozess, nur bestätigter Testslot, keine Registrierung,
  Aktivierung oder native Speicherung. [Vertrag](business-construction-probe.md).
- 229 Tests, Kandidat-/Normal-Build jeweils 0 Warnungen/Fehler; unabhängiger
  Quellreview. Zum damaligen Buildzeitpunkt war der native Konstruktionstest offen;
  sein späterer Erfolg ist im neueren Eintrag dokumentiert. 20 Produktionsdateien.

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
