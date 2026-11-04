# TownHallInterior — local experimental public hall

## Nächste Entwicklungsphase: TownHall Business

Der [Fertigstellungsplan für das TownHall Business](business-completion-plan.md)
beschreibt Kauf, Besitz, native Geldwäsche und Speicherung auf Basis des fertigen
Innenausbaus. Stand 2026-11-04: Die [beobachtende Anbindungsprobe](business-binding-probe.md)
hat Erstladen, Prozessneustartvergleich und Menü-Reload auf TownHallTest (Slot 2)
bestanden. Schema 2 liefert je Ladevorgang 90 Ereignisse/45 vollständige Aufrufpaare
ohne verworfene Daten; die früheren Daten bleiben erhalten. Kandidat `52bb577a…`
bestand anschließend die [inaktive native Konstruktionsprobe](business-construction-probe.md):
eigene Business-Shell, native JSON-Serialisierung und bestätigter Rückbau bei zehn
unveränderten Registern. Die anschließende
[Aktivierungs-/Registrierungsprobe](business-activation-probe.md) `54d37fe5…`
stürzte vor dem Slot-Load durch Hooks auf gemeinsam genutzten nativen Code ab.
Sie ist zurückgezogen und gegen Neubauen gesperrt; am 2026-11-03 wurde zunächst
die geprüfte Diagnose `52bb577a…` wiederhergestellt. Die folgende
[inaktive Untertyp-/Dispatch-Probe](business-subtype-probe.md) `659ee6a2…`
scheiterte im Spiel am 2026-11-04 vor Instanzerzeugung an der FishNet-Typauflösung.
Zehn Register und 13 fremde VTables blieben unverändert. Die begrenzte verwaltete
Namenskorrektur für genau NetworkConnection (`59484a66…`) bestand anschließend
den Live-Lauf um 04:04:16 UTC: eigener Untertyp, beide Property-Aufrufe false,
eigener Rückbau und zehn Register/14 fremde VTables unverändert. Menü-Reload
um 04:08:48 UTC bestätigte die Einmaligkeit ohne neuen Versuch.
Die [zusätzliche inaktive ISaveable-Interfaceprobe](business-interface-probe.md)
`cb98bc95…` scheiterte am 2026-11-04 um 04:53:58 UTC vor dem Interface-Aufruf.
Die Flaggen-Korrektur `a8d45f2a…` bestand am 2026-11-04 um 06:58:38 UTC den
inaktiven Interface-Aufruf. Der Menü-Reload um 07:00:49 UTC bestätigte die Einmalversuchsperre.
Der inaktive Speicherlisten-Registrierungsmodus (`473064e0…`,
`inactive-saveable-registration`) lief am 2026-11-04 um 07:11:05 UTC auf Slot 1
(Organisation Dominik / SaveGame_1): `RegisterSaveable` genau einmal aufgerufen,
Eintrag sofort wieder aus der Speicherliste entfernt, `Save()` unaufgerufen;
Register, VTables und native Dateien unverändert. Kein Save-/Load-Nachweis.
[Bericht](evidence/business-manager-live-register-saveable-20261104.json),
[Vertrag und Herkunft](business-manager-registration-probe.md). **411 reine TownHall-Tests** bleiben der
Offline-Stand. Aktivierung, vollständige Initialisierung und Persistenz bleiben offen.
Keine MapExpansions-Abhängigkeit. Der abgenommene Innenraum bleibt abgeschlossen.

## Abgenommen: Innenausbau komplett (2026-10-30)

Eingang, Erdgeschoss und Obergeschoss sind abgenommen („ja die interior ist fertig“). Letzter Stand: DLL `7681d319…`, 145/145 Tests, Build 0/0, Backup `20261030T140642055Z-TownHallInterior-650a4ec4`. Offen als Erweiterung, nicht als Mangel: Aushang-Beschriftung, Möbel außerhalb des Vanilla-Satzes, NPC-Navigation, Deckenwirkung im OG; der Keller bleibt verworfen (Gelände-Mesh 84 cm unter dem Hallenboden).

## Vorher: Erdgeschoss und Obergeschoss (2026-10-30)

Der Keller wurde am 2026-10-30 verworfen: Die Gelände-Mesh liegt nur 84 cm unter dem Hallenboden (y ≈ 0,69) und steht im Treppenschacht; eine offene Treppe kann sie nicht umgehen. Umfang ist jetzt EG + OG. Das Obergeschoss hat drei Büroarbeitsplätze, einen Besprechungstisch mit vier Stühlen, fünf Archivschränke und zwei Eckpflanzen (48 Platzierungen insgesamt). 144 Tests, installiert `abaa9c2f…`, Backup `20261030T044742763Z-TownHallInterior-325e2698`. Sichtabnahme des OG offen.

## Verworfen: Keller (2026-10-29, Nacht)

Das Erdgeschoss hat Info-Tresen, drei Sitzbänke, Läufer, Aushangkasten und zwei Eckpflanzen (`HallFittings.cs`, abgenommen am 2026-10-29). Unter der Halle liegt jetzt ein Keller: Boden −2,90, Deckenunterkante −0,55, gerade Treppe mit 15 Tritten an der Westwand, vier Lichter, sechs Aktenschränke. Die Platzierung beruht auf Sonden in der privaten Blender-Map (freier Raum zwischen ≈ −0,28 und −5,14 in Blender-Z). 157 Tests, installiert `211f112a…`, Backup `20261029T204537778Z-TownHallInterior-1f232624`. Sichtabnahme des Kellers, Gelände-Kollision, NPC-Navigation und eine Sewer-Anbindung sind offen.

## Vorherige: Restspalt über der Tür und flackernder Rahmen (2026-10-29, Nachtrag 2)

Die erste Flügelkorrektur war wirksam (die 2-cm-Spalten sind weg), aber die Ingame-Aufnahme um
18:20 zeigte noch ein helles Band über den Blättern: RGB 159/191/239 gegen Himmel 119/145/203,
also echte Außenwelt. Der Native-Fassadenschnitt folgt Dreieckskanten und überschreitet das
geplante Rechteck; die Laibungen endeten bei x 54,34 und ließen einen 17 cm tiefen Aufwärtsweg
über die Blattoberkante (4,55) offen. `inner-lintel` und `reveal-south`/`reveal-north` reichen
jetzt bis zur Fassadenebene 54,18; von außen bleibt alles unverändert, weil die Flügel
(30,00–32,00) die Öffnung decken.

136 verwaltete Tests bestehen (vier neue: Laibungstiefe plus drei Sehstrahlen über die
Blattkante), Builds 0 Fehler / 0 Warnungen. Installiert ist `28efce74…` bei geschlossenem Spiel
(DLL-Hash am Ziel identisch, PDB mitgeprüft, Backup
`20261029T173104129Z-TownHallInterior-af82f3e5`). **Nach der Installation nicht gestartet**;
Sichtprüfung liegt bei Dominik.

Nachtrag 2 nach Dominiks Rückmeldung („über der Tür ist ein kleiner Spalt, und der Rahmen oben
flickert“): `inner-lintel` endet bei 4,54 statt 4,55 und beginnt bei x 54,16 statt 54,16–54,95
von 54,18 – damit überlappt die Kopf-Laibung die Blattoberkante und -ebene um je 1 cm. Das
Flackern kam von deckungsgleichen Flächen der Blattteile (Rahmen und Stile mit gleicher
X-Ausdehnung und gleicher Oberkante); die Teile liegen jetzt als `PublicEntrance.LeafParts`
vor, Rahmen 1 mm dünner, Stile/Panel unter der Rahmenkante, abgesichert durch einen
Ebenen-Test. 137 Tests, installiert `3a82f9e8…`, Backup
`20261029T173935879Z-TownHallInterior-fc354400`.

**Abnahme 2026-10-29:** Dominik hat den Eingang nach diesem Deploy optisch abgenommen
(„tür sitzt jetzt perfekt“) – Sichtfit von Laibung, Flügeln und Innenzarge, innen wie außen.
Decke, obere Hülle, Treppe, Möbel, LOD, NPC-Navigation und Multiplayer sind damit **nicht**
abgenommen; nächster Umfang ist der Innenausbau des Erdgeschosses.

## Vorherige: Schiebetür dicht am sichtbaren Rahmen (2026-10-29, abends)

Dominik meldete einen Innenraumscreenshot, auf dem neben beiden Flügeln die Außenwelt
durchscheint. Ursache war ein Maßkonflikt: Die Flügel waren auf das Fassadenloch
(z 30,04–31,96) bemessen, innen sichtbar ist aber die Laibungskante (z 30,02/31,98) —
je Seite blieb ein 2-cm-Spalt. Die Flügel sind jetzt je 1,00 m breit (Abdeckung
30,00–32,00, Berührung bei 31,00), tragen ein Rahmenprofil und werden von einer
render-only Innenzarge gefasst. Geometrie liegt als gemeinsame Konstanten in
`src/PublicEntrance.cs`.

132 verwaltete Tests bestehen; No-Deploy- und Deploy-Build liefen mit 0 Fehlern und
0 Warnungen. Installiert war `7ffcf24b…` (Backup `20261029T165314988Z-TownHallInterior-0f5f15b9`);
diese Fassung lief um 18:19 im Spiel und beseitigte die 2-cm-Spalten, ließ aber den
Himmelsstreifen über den Blättern offen (siehe oben).

## Repository-Integration und Archivierung (2026-10-29)

Die Mod ist jetzt in `S1Mods.sln` aufgenommen und damit von Solution-Build, Format-Gate
und CI erfasst. `RoomPlan.cs` und `RoomPlanTests.cs` sind whitespace-sauber. Der nie
verdrahtete Tür-Dialog (Selector, Modal-Host, Kontextmenü-Laufzeit, Fokus-Lease) ist als
Quellarchiv nach `Source/Archive/TownHallInterior/DoorDialog/` gewandert; die Mod
selbst bleibt aktiv. Mit dem Archiv gingen 41 Tests (169 → 128), danach kamen vier
Flügel-/Dichtheitstests hinzu.

Vorheriger, nicht mehr aktueller Stand: installierter Schwelle-Kandidat `5efb8650…`
lud am 2026-10-29 um 16:57 und veröffentlichte Halle und Empfangsmöbel laut MelonLoader-Log;
das ist ein Ladehinweis, keine Sichtabnahme. [Archivhinweis und Wiederherstellung](../../../Archive/TownHallInterior/README.md).

### Veröffentlichungspfad

`Tools/package-release.ps1` wählt Mods über das Verzeichnis `Source/Mods/<Name>`, nicht
über die Solution. Diese Mod erfüllt das Kriterium und würde damit in ein Release-Archiv
aufgenommen — die eingebetteten Schnitte [`../assets/native-facade.json`](../assets/native-facade.json) und
[`../assets/native-furniture.json`](../assets/native-furniture.json) sind lokale, hashgepinnte Extraktionen der f12-Szene und
ausdrücklich **nicht verteilbar**. Für diese Mod darf kein Release-Archiv erzeugt werden.

## Vorherige: Tür- und Hüllenkorrektur (2026-10-29)

Die Türlogik verwendet jetzt nur die beidseitige Türnähe statt der gesamten Halle.
Eckfugen, Bodenränder und die Eingangslaibung sind geschlossen; der eigene Sturz
verbindet Rahmen und Decke, ohne über den oberen Boden hinauszuragen. Oben ergänzen
sichtbarer Boden, perimeternahe Wände, Decke und vier weitere Lichter die Hülle.
Vanilla-Möbel, Spielermaße, Portalhöhe und Treppe bleiben erhalten.

169 verwaltete Tests (auch isoliert), No-Deploy-Build und fokussierte Formatprüfungen
bestanden. Unabhängiger Review ohne belegten Blocker; DLL `932de88d…`, PDB und
Metadaten waren installiert, Ziele und Backups hashgeprüft; dieser Stand wurde am
selben Tag durch den Schwelle-Kandidaten `5efb8650…` abgelöst. Das Gesamt-Pure-Gate
blieb mit fünf bekannten Fehlergruppen rot, nicht die gezielte Rathaus-Suite.
Ingame macht weiterhin Dominik; die bisherigen Bilder sind nur die Teilabnahme
von Möbeln/Licht mit gemeldeten Hüllenfehlern. Der neue Stand ist noch nicht visuell
abgenommen. [Reparaturnachweis und kurze Testfolge](enclosure-verification.md).

## Vorherige Vanilla-Empfangshalle (2026-10-29)

Der kostenlose Durchgang ist von Dominik akzeptiert. Die neue Einrichtung ergänzt
das Erdgeschoss um zwei Empfangsarbeitsplätze, acht Wartestühle, einen Ausfüllplatz,
Aktenschränke, Pflanzen, Papierkörbe und eine dekorative Uhr. Die Möbel verwenden
gezielt extrahierte Vanilla-Geometrie und geliehene Originalmaterialien, keine
kopierten Spielskripte. Eigene Deckenflächen und vier lokale Lichter ergänzen die
Hülle. Der separate sichtbare Rahmen wird passend zum Portal höher/breiter dargestellt;
Eingangs- und Spieler-Kollision bleiben unverändert. Alte Blockmöbel entfallen.

117 verwaltete Tests (auch isoliert) und der No-Deploy-Build bestanden für diesen
Vorgänger. Er wurde bei geschlossenem Spiel installiert und hashgeprüft.
Details, Herkunft, Besitz und Grenzen:
[Einrichtungsnachweis](furniture-verification.md). Obergeschossausbau und nutzbare
Möbelinteraktionen sind nicht enthalten. Originalgeometrie/DLL bleiben lokal.

## Vorherige Fassadenkorrektur (2026-10-28)

The previous builds opened collision but left **three independent opaque wall contributions**
behind the hidden door. This is not evidence that static batches ignore `Renderer.enabled`.
`FacadePortal` now cuts the measured entrance volume from local copies of the exact f12
`townhall/Walls`, root `Cube`, and distinct LOD `townhall/Cube` contributions. `NativeFacadeSwap`
prepares owned meshes with original UV0/UV1, normals, tangents and material slots, keeps their
LOD membership, then hides only the three original renderer contributions. Shared native
meshes, foreign batch contributions, door frame, windows, foundation and NPC-building identity
are not modified. The original Door/Glass/Quad renderers are addressed by exact path rather
than guessed door-sized bounds. The existing free sliding entrance and disabled player-knock
prompt remain; this is not a business, purchase or money-laundering implementation.

The local extraction and DLL embedding it are **not distributable**. The game scene hash,
renderer metadata, bounds and material names must match before publication; a mismatch leaves
the room inactive. See [facade evidence and verification](facade-portal-verification.md).
Geometry tests and a successful no-deploy build are not in-game visual acceptance. Dominik
handles all in-game launch, navigation and visual checks; do not use the old automation runbook
as permission to launch, drive or close his game.

The screenshot's missing upper skin is addressed by the current enclosure candidate above;
its in-game acceptance and NPC traversal/lifecycle acceptance remain unestablished.
The wall-portal correction alone does not establish those outcomes or provide the
hospital's native networked door controller.

## Historical staging notes — superseded by the current section

The remainder preserves earlier staging constraints and dated observations. In particular,
the old knock-choice gates are not the current automatic public-entrance design, and the
following collider-only description does not describe the new geometric facade replacement.

On `Main` scene load this project builds an owned room and, only if the live Town Hall colliders match the known identity, disables the Walls, Cube and door-quad colliders. It does not disable their renderers, does not edit shared meshes, and does not replace `StaticDoor`. Two owned leaves slide aside when the player is near the entrance. That is a collision opening, not a hospital-style mesh opening: the visible door can still be walked through. Unit tests cover the plan and the swap rule, not Unity, Harmony or traversal.

## Continuation safety and scope (2026-10-28)

The native context-menu probe received actual foreground clicks for all three test choices and demonstrated a movement lock/restoration in one bounded session; that was **not** a Town Hall first-E test and does not prove Escape, interleaved foreign owners or lifecycle safety. Its source and assertion evidence are preserved in `.backups/townhall-modal-save-20261028/continuation-probe/`. `NativeContextMenuRuntime` now compiles against the installed IL2CPP proxies and contains code to create the typed native payload and callbacks. `NativeDoorModalHost` defers selection until a later update, the native popup is closed and guarded release succeeds. Its automated tests substitute the runtime boundary; they do **not** exercise the native adapter, Unity or focus ownership. `DoorAdapter` scopes a native replay token to the captured door pointer; tests exercise the policy but not the native Harmony bridge.

Live testing of the protected save stays blocked, but an isolated slot now exists. MoreSaveSlots maps page position `CurrentPage * 5 + index` to `SaveGame_{n+1}` (decompiled from the installed 1.0.12 DLL; live log `mapped slot 2 -> 2 (SaveGame_2)`). On 2026-10-28 the game itself created `SaveGame_2` (organisation `TownHallTest`, version 0.4.7f12, `PlayTutorial` false after a skipped prologue; the first attempt had started the tutorial and was replaced). `SaveGame_1` stayed byte-identical (38/38 SHA-256). Steam Cloud for this app is disabled in the global setting (properties dialog plus `cloud_log.txt`: `Sync Disabled`, no upload/download of `SaveGame_2`). Evidence: `.backups/townhall-modal-save-20261028/isolation-run-20261028T172611Z/`. A copied folder is still not sufficient evidence; this slot was created by `SetupScreen.StartGame`, not by copying. The earlier 40-file inventory remains at `.backups/townhall-modal-save-20261028/continuation-live-hashes.json`.

The adapter retains native option/action payloads for the lifetime of its runtime instance to protect stale delegates, and refuses additional attempts after 128 retained payloads. A future integrator must retain that runtime through the native screen's scene lifetime, drive `Tick` from later game updates, and call `Shutdown` before invalidating its scene generation. None of that is wired in the staging mod.

Physical opening remains blocked: the known native shell contains solid full-footprint colliders and potentially static-batched renderers; inactive test geometry does not establish safe publication, exit, or NPC traversal. The native modal adapter and host remain **unwired**. `ExclusiveModalFocus` now implements `IExclusiveModalFocus` as game-free lease logic: the owner announces every mutation before performing it (`MutateOwned`) and a future native observation layer must report every observed change (`NotifyChanged`). Ownership is proven by attributed mutations, never by comparing focus values — an unannounced change poisons the lease until an authoritative reset, including interleavings that leave identical values behind. The observation layer (native hooks on the screen stack, cursor, movement and typing) is **not** part of this build and remains unverified; without it, exclusivity is not proven. A matching boolean snapshot is still not ownership proof. `DoorAdapter` now poisons the choice after every post-suppression failure (selector refusal, premature callback, exceptions, an action that threw): the native action is never retried blindly and the door stays suppressed until an authoritative reset (scene reload or authority loss); failures before a choice is made still pass native. An unidentifiable partially opened native popup is intentionally not closed, and cleanup may remain pending rather than mutate a foreign screen. If an ownership check or native cleanup fails after mutation, the cursor or movement may remain in the modified state and there is no verified recovery; do not wire or deploy the host on this evidence. No runtime result is claimed for the new adapter. Do not substitute a fake open option or deploy this staging DLL as the requested feature.

The plan relies on the native foundation for ground support, and includes a split upper floor, stairs, perimeter colliders and block furnishings. It is only a candidate: roof/headroom, exterior and all native geometry/collision interactions must be checked before publication. Its world-space boxes have not been validated under Unity physics or against the live scene.

## Native door baseline (live session 2026-10-28)

Setup: Steam was set to offline by the owner before launch; the game was launched
through Steam and the owner-declared test slot 1 (`SaveGame_1`, organisation name
"Dominik") was loaded. `TownHallInterior.dll` was **not** installed, so every
observation below is native-only. The session ran roughly 17:46–17:55 and was closed
by window close (graceful shutdown).

Owner observations (Dominik, attributed, not agent-reproduced): the hover prompt at
the Town Hall door reads exactly `E Knock`; pressing E opens only the native knock
dialog with no enter/open entry; with one NPC inside it performs a normal knock;
with two or more NPCs inside it offers a selection of which NPC to summon out.
No additional buttons appeared. The preserved screenshot
(`.backups/townhall-modal-save-20261028/baseline-native-door-20261028/door-dialog.png`,
SHA-256 `11d0579d22b7a1feb2438463ef909bd6b9f77f456369334d349c2772f37d46e2`, read by the
agent on 2026-10-28) shows the hover prompt `E Knock` on the closed door
(capture shows DAY 1, Tue 9:35 PM); it does not show an open dialog.

Not verified in this session: the zero-NPC E behavior (whether the knock dialog also
appears when nobody is inside), Escape/cancel behavior, and anything involving the
mod hook (not deployed). The save tree hashed identical to the pre-live backup
(`.backups/townhall-modal-save-20261028/pre-live-20261028/`, 40 files,
0 changed / 0 missing / 0 new), so the session produced no save writes; the log's
final `Preferences Saved!` is loader preferences, not a game save. No runtime result
is claimed for any TownHallInterior component from this session.

## Target evidence

The supplied `townhall_native.out` and `townhall_measure_world.out` (under Hermes scratch) identify `Map/Hyland Point/Region_Downtown/TownCenter/TownHall`, baked GUID `c747a750-98cb-47fc-9c40-cf5912acc390`. World shell bounds approximately x=54.35..63.35, z=20.50..41.50, y=1.25..8.25. Door is on x~54.3 at z~31. Path IDs are asset/session identifiers, not runtime identities. Preserve native `StaticDoor` and `IntObj`, NPC building and knock access. The cached f12 IL2CPP proxy has `StaticDoor.Interacted()` and `Knock()` (proxy thunks, not native control-flow evidence), `InteractableObject.StartInteract()`, and `NPCSummonMenu.Open`; installed patch target/bridge behavior still needs direct confirmation.

The installed proxy `Assembly-CSharp.dll` SHA-256 `346205306273805a78b01a917f14b94518498af1361595c4c417baa0b32b4bd8` was inspected via `System.Reflection.Metadata`: its managed proxy methods `StaticDoor.Hovered`, `Interacted`, `Knock` and `InteractableObject.StartInteract` are public virtual instance `void()` methods. Native method visibility can differ from proxy visibility. `GenericSelectionModule.Open` has a two-parameter signature; `Close` and `Cancel` have none; `isOpen` and `ChosenOptionIndex` getters are present. These are metadata only, not hook-installation or native-control-flow evidence. Installed `0Harmony.dll` SHA-256 is `6c898933b52149e8bcf6722305c7e4d94add47c9e53b364096f91721624c0877`. The current hook has not been verified in gameplay.

The `townhall/Walls` and `Cube` colliders are solid boxes covering the interior; `UpperBrickTrim` is a full-footprint slab. The entry `Door/Quad` has its own solid mesh collider. Their exact per-object ownership, any other blocking colliders (including lower trim, foundation and roof) and static-batched renderer behavior must be validated in-game before any replacement. Native static-batched door renderers cannot safely be assumed to follow transform changes. Disabling a shared static batch could affect unrelated map geometry. Preserve shell and native interaction; build exact guarded replacement collision and animated owned leaf; record and restore captured per-object state. Never open a hole until replacement geometry is fully ready.

## Required next implementation and review gates

1. Confirm actual installed Assembly-CSharp/IL2CPP/Harmony signatures and whether the earliest E callback occurs even when nobody can be knocked for; a prefix on the wrong method will fail the no-NPC entry requirement. Confirm callback invocation and bypass reentrancy in an isolated game session. The current guarded adapter has a native-pass-only selector, and its identity/lifecycle policy is not live-verified. In code, `PassNative`, `ShowChoice` and `Suppress` are now mutually exclusive: native passes only before a choice is made (foreign door, missing authority or an unready selector — the staging hook always reports unready), and every post-suppression failure poisons the choice until an authoritative reset instead of blindly retrying a native action whose outcome is unknown. None of that is live-verified. The 2026-10-28 baseline session confirmed the Knock-only native entry (no open/enter option) and the NPC-count-dependent summon selection; the zero-NPC E behavior remains unconfirmed and must still be checked in a live session.
2. The owned selector (German open/knock/cancel, ticket-guarded callback ownership) and the game-free `ExclusiveModalFocus` input/camera-lease logic exist in code and are unit-tested; the native observation hooks, their wiring into a real input/camera lease and cancel releasing only owned state are still open and not live-verified. Open must not knock; knock must replay the original native interaction once without opening. Teardown and late callbacks must honor the world generation and target identity. Return vanilla on unsupported multiplayer/unknown authority, no purchase, quest, money, ownership or NPC-eligibility gating on open.
3. Resolve exact scene hierarchy, GUID and collider/renderer identities by live objects, not baked path IDs. Plan 9x21x7 world shell, y~1.25 ground and y~4.75 upper floor; validate native foundation/floor tops, 2m+ player headroom and actual controller step size. Build inactive shaped walls around the exact entrance, split upper slab for stairs and guard exposed edges; furnishings are decorative only and use verified native resources read-only or safe primitives. Do not activate unvalidated partial geometry. Guarantee continuous floor support, no wall/terrain leaks and correct camera/roof behavior.
4. Implement reversible publication as a transaction: validate unique target and exact original states; build new geometry inactive, verify colliders, materials and escape; perform bounded mutations with per-object restoration journal. Preserve foreign changes, detect uncertain restoration and never destroy the room while a player remains inside without a verified evacuation policy. Scene unload, menu, destroy, repeat and authority loss must be exercised.
5. Run geometry and lifecycle/rollback tests against production implementations, then independent parent review **before deployment**. Keep the current real save untouched. Establish and verify an isolated test save and Steam-cloud boundary before any game launch; if that cannot be established, perform no live tests. Only after isolation and feature verification, deploy with rollback and verify free entry, first E with and without an eligible NPC, knock once, cancel, movement/focus, exit, collision seams, floor/headroom and reload. Compare complete current-save and cloud inventories, and keep WorldLook/other changes untouched.

## Build and tests

```
dotnet test Source/Tests/TownHallInterior.Tests/TownHallInterior.Tests.csproj -c Release -p:S1NoDeploy=true
dotnet build Source/Mods/TownHallInterior/src/TownHallInterior.csproj -c Release -p:S1NoDeploy=true
```

Live baseline runs are automated through `Tools/townhall-live-baseline.ps1`; the channel
roles, sequence, CUA handoff and abort rules are in the
[live automation runbook](live-automation-runbook.md).

The central props include a Shared project reference and optional installed API references. `mod.json` describes a non-playable staging build; this is not release-ready metadata. No S1MAPI calls are assumed. The adapter consumes a knock replay token and suppresses repeats using single-threaded, generation-scoped state and `finally` replay cleanup, and poisons the choice after every post-suppression failure until an authoritative reset, but its real bridge has not been verified. Automated tests do not validate mesh alignment, actual game behavior, save safety or correct dependency loading.
