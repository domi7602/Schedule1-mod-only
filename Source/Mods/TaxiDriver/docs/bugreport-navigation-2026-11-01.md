# Bugreport: TaxiDriver kommt nicht zuverlässig am Ziel an

Stand: 2026-11-01, 19:37 Uhr (Europe/Berlin). Das ist ein Befundbericht, keine Abnahme
und kein Fix. Spielversion und geladene DLL sind aus dem Log gelesen, nicht geschätzt.

## Kurz

Das Taxi fährt manche Ziele an und gibt andere auf, obwohl eine Straßenroute
existiert oder kurz vorher berechnet wurde. Zwei verschiedene Fehler stecken
dahinter. Der erste ist in der jetzt installierten Fassung für die Barn nicht
mehr aufgetreten. Der zweite ist offen: eine Weiterfahrt vom Ankunftspunkt
aus bricht ab, ohne das neue Ziel zu verlassen.

## Umgebung

| | |
|---|---|
| Mod | TaxiDriver 1.0.5 |
| Geladene DLL | `012330b16b8fc6470c7bc0c299f1f43146821c63d232e807fdbd36d6e2528b42` |
| Build-Kennung im Log | `[shortcut] mode=Guard compatible=True` |
| Spiel | Schedule I 0.4.7f12, Unity 2022.3.62f2 |
| Loader | MelonLoader 0.7.3 Open-Beta |
| OS | Windows 11 |
| Speicherstand | Slot 1, „Dominik“ |
| Start | über Steam, nicht die EXE direkt |

Die Versionsnummer 1.0.5 allein sagt nicht, welcher Build läuft. Dieselbe Version
existiert als Observe-Build (`8153b7b6…`, Guard aus) und als Guard-Build. Ohne
die Zeile `mode=Guard` und den DLL-Hash ist ein Fahrtergebnis nicht zuordenbar.

## Erwartet

Nach normalem Einsteigen fährt das Taxi das in der App gewählte Ziel an, hält
in der Nähe des gewählten Straßenpunkts und meldet Ankunft. Eine Weiterfahrt
zu einem zweiten Ziel vom selben Taxi aus kommt ebenfalls an oder meldet einen
klaren, frühen Abbruch, ohne minutenlang zu rechnen und dann stehen zu bleiben.
Ein fehlgeschlagener leerer Direktpfad darf eine bereits gefundene Straßenroute
nicht verwerfen.

## Tatsächlich

### 1. Leerer Direktpfad ersetzt die Straßenroute — in dieser Fassung abgefangen

Beim Ziel Barn berechnet das Spiel eine gültige Straßengruppe und danach einen
optionalen Direktpfad. Dieser Direktpfad ist leer und im Fehlerzustand, Länge 0.
Die native Längenwahl behandelt 0 als kürzer und ersetzt damit die gültige
Straße. Sichtbar war das als komplette Berechnungsfehler und Abbruch etwa 108 m
vor dem Straßenpunkt, später auch als Fahrt bis auf etwa 5 m und dann `GAVE UP`.

Die installierte Fassung ersetzt genau diesen leeren Fehlerpfad einmal durch
unendliche Länge und behält die Straßengruppe. In Dominiks Sitzung vom
2026-11-01, 19:36 Uhr, steht dafür `override=True`, dieselbe Gruppe vorher und
nachher, `nativeResult=Success`. Das ist eine Mod-Abfangung, keine Reparatur
des Spiels.

### 2. Barn-Ankunft — in dieser Sitzung erreicht

Dieselbe Sitzung, normales Einsteigen erst nach dem Halt der Abholung:

- Abholung hielt 4,6 m vor dem Spieler, 0,9 km/h.
- Ziel Barn, Straßenpunkt `(165.3, 0.0, 25.1)`, Zielgebäude `(180.1, 1.0, -11.0)`.
- Supervisor schloss bei lesbarem Halt, 5,0 m, Geschwindigkeit −0,1 km/h.
- Log: `ARRIVED at Barn`, Fahrpreis 17 $.

Das deckt diese eine eigene Barn-Fahrt. Es ist keine Aussage über Wiederholung,
Untergrund oder andere Ziele.

### 3. Weiterfahrt Bungalow vom Barn-Ende — offen

Ohne Aussteigen wurde im selben Taxi Bungalow gewählt, Straßenpunkt
`(-156.0, -4.0, 113.7)`. Das Taxi stand noch am Barn-Straßenende
`(161.1, 0.7, 22.4)`.

- Die erste Berechnung schlug fehl. Die allgemeinen Suchen meldeten
  `Searched all reachable nodes, but could not find target.` Eine bestimmte
  blockierende Tag-Ursache ist damit nicht belegt.
- Recovery fuhr 3,3 m rückwärts. Die zweite Berechnung gelang, Guard griff
  nicht ein (`override=False`), Pfad bis `(-152.99, -4.00, 113.69)`.
- Danach blieb das Taxi bei `(164.6, 0.8, 19.6)`. Ein freier Rettungspunkt
  wurde nicht gefunden. Spur-Rücksetzung wurde übersprungen
  (`snap-back skipped: no free road spot`).
- Nach drei Recovery-Episoden und 9,1 s in diesem Auftrag:
  `GAVE UP at Bungalow — 334.1 m from the target`. Fahrpreis 6 $.

Das Taxi hat das Barn-Ende praktisch nicht verlassen. Der berechnete Pfad und
die körperliche Fahrt sind hier nicht dasselbe. Eine frühere Bungalow-Fahrt
mit derselben DLL, frisch vom Rathaus, kam 3,5 m vor dem Ziel an. Der Abbruch
gilt deshalb für die Weiterfahrt vom Barn-Ende, nicht für Bungalow allgemein.

## Schritte für die offene Weiterfahrt

1. Spiel über Steam starten, Slot 1 laden.
2. Im Log prüfen, dass TaxiDriver `012330b1…` und `mode=Guard` geladen sind.
3. Taxi rufen, erst einsteigen, wenn es steht.
4. Barn wählen und die Ankunft abwarten.
5. Ohne auszusteigen Bungalow wählen.
6. Beobachten, ob es losfährt oder am Straßenende stehen bleibt und aufgibt.

Früheinsteigen und Docks sind in dieser Sitzung nicht geprüft und gehören nicht
zu diesem Befund.

## Log, gekürzt

Sitzung 19:35:35 bis 19:37:35. Vollständiges Log, 112454 Bytes, SHA-256
`b2b6cfe51163c5d47799a6e088ca51b7ce28954b890b022328e05ce740d0a0af`,
Kopie im Vault unter
`raw/attachments/taxi-navigation-settings-2026-10-30/owner-barn-20261101/Latest-owner-19-35.log`.

```text
[19:35:37.340] Melon Assembly loaded: '.\Mods\TaxiDriver.dll'
[19:35:37.340] SHA256 Hash: '012330B16B8FC6470C7BC0C299F1F43146821C63D232E807FDBD36D6E2528B42'
[19:35:41.276] [shortcut] mode=Guard compatible=True registrations=True
[19:36:45.739] [nav] arrival confirmed — readable halt 0.9 km/h, 4.6 m
[19:36:47.570] [ride] passenger seat fix applied
[19:36:49.257] destination = Barn … drop-off roadside candidate at (165.3, 0.0, 25.1)
[19:36:49.565] seeker=general pathID=38 error=True state=Error count=0
[19:36:49.577] native-getter stage=4 … original=0 effective=Infinity override=True
[19:36:49.578] state2-post mode=Guard override=True … nativeResult=Success
[19:37:08.029] [nav] supervisor … trigger=pre-deadline dist=5.0m speed=-0.1 … action=completed
[19:37:08.042] [ride] ARRIVED at Barn — 5.0 m from the target (165.3, 0.0, 25.1)
[19:37:19.663] destination = Bungalow … roadside candidate at (-156.0, -4.0, 113.7)
[19:37:19.779] calculation-post result=Failed
[19:37:21.674] directed reverse 3.3 m confirmed
[19:37:21.920] state2-post mode=Guard override=False … nativeResult=Success
[19:37:30.799] [nav] GAVE UP … 334.1 m left … position (164.6, 0.8, 19.6)
[19:37:30.812] [ride] GAVE UP at Bungalow — 334.1 m from the target
```

## Einordnung

| Punkt | Stand |
|---|---|
| Barn, normale Abholung, diese DLL | angekommen, eine eigene Fahrt |
| Leerer Direktpfad | abgefangen, Spiel selbst unverändert |
| Bungalow direkt vom Rathaus, ältere Beobachtung derselben DLL | angekommen, 3,5 m, nicht diese Sitzung |
| Bungalow als Weiterfahrt vom Barn-Ende | offen, `GAVE UP` bei 334,1 m |
| Docks | nicht in dieser Sitzung geprüft |
| Früheinsteigen | nicht geprüft |
| Kollisionspartner am Straßenende | nicht gemessen |
| Speicherspielstand vor/nach dieser Sitzung | nicht byteweise verglichen |

Mitgeladen und für die Navigation nicht als Ursache belegt: PoliceTestMode war
in dieser Sitzung an. Die Meldung `patrol driver is unavailable` kam bei der
erfolgreichen Barn und bei der gescheiterten Weiterfahrt. Sie erklärt den
Abbruch allein nicht.

Der Fahrpreis von 6 $ für die abgebrochene Weiterfahrt folgt der bestehenden
Regel, eine bewegte Spielminute kostet 1 $. Das ist hier eine Folge, kein
eigener nachgewiesener Abrechnungsfehler.

## Quelle

Gelesen am 2026-11-01 gegen Repository-Stand `bcd54e9ace58dbe1a615b44f319f96efd7902fc5`.
Die genannten C#-Dateien waren dabei unverändert. Der Bericht selbst ist noch nicht
committet. Zeilen können sich verschieben.

### Spiel, nicht der Mod

Die Längenwahl ohne Fehlerprüfung liegt in `GameAssembly.dll`, SHA-256
`61829050e56ca180d67be592e4bfe61ec97d2e9573f720d90a54e318a7e4381d`.
Dokumentiert in `Source/Mods/TaxiDriver/docs/native-shortcut-verification.md`:
`MoveNext`, Resume-State 2, RVA `0xAD0D1D–0xAD0DB3`, vergleicht die
Einstiegslänge mit `Path.GetTotalLength` (`0xB5BE60`) und prüft den Fehler
des Direktpfads nicht. Ein leerer Fehlerpfad hat Länge 0 und kann die gültige
Straßengruppe ersetzen. Das ist Spielcode. Dieser Bericht enthält keine
neue Disassembly.

### Mod, Abfangung des Direktpfads

| Datei | Stelle | Was sie tut |
|---|---|---|
| `Source/Mods/TaxiDriver/src/TaxiDriver.csproj` | Zeile 8–11 | `TaxiShortcutGuard=true` setzt `TAXI_SHORTCUT_GUARD`. Ohne das Flag bleibt der Build Observe. |
| `Source/Mods/TaxiDriver/src/DrivingPatches.cs` | `NativeShortcutPatch`, Zeile 31–43 und 86–110 | Hängt sich an `VehicleAgent.Navigate`, `MoveNext`, `Path.GetTotalLength` und `NavigationCalculationCallback`. Die Logzeile `mode=Guard` entsteht in Zeile 110. |
| `Source/Mods/TaxiDriver/src/DrivingPolicies.cs` | `ShortcutComparison.After`, Zeile 89–110 | Nur der zweite Längenaufruf, Stage 4, und nur wenn das Ergebnis genau 0 ist und die Einstiegslänge größer ist, wird im Guard-Build durch `float.PositiveInfinity` ersetzt. Observe ändert nichts. |

### Mod, Ankunft und Aufgabe

| Datei | Stelle | Was sie tut |
|---|---|---|
| `Source/Mods/TaxiDriver/src/DrivingPolicies.cs` | `NavigationArrivalPolicy`, Zeile 217–253 | Ankunft nur innerhalb 10 m und bei lesbarem Halt bis 1 km/h, nicht bei Rückwärtsfahrt. |
| `Source/Mods/TaxiDriver/src/DrivingPolicies.cs` | `SupervisedArrivalPolicy.IsConfirmedArrival`, Zeile 270–275 | Zusätzlich braucht es einen aktuellen Complete oder Fortschritt derselben Fahrt. Nähe allein reicht nicht. |
| `Source/Mods/TaxiDriver/src/SpikeRunner.cs` | `TryCompleteBeforeRecovery`, Zeile 442–524 | Schließt die Fahrt vor Recovery ab. Der Barn-Log `trigger=pre-deadline` kommt aus `TickNavigationPolling`, Zeile 2701–2702. |
| `Source/Mods/TaxiDriver/src/SpikeRunner.cs` | `TryBeginRecovery`, Zeile 536–672 | Gibt Recovery frei, sucht bei wiederholtem Stillstand einen freien Punkt und ruft sonst `GiveUpNavigation` auf, Zeile 659. |
| `Source/Mods/TaxiDriver/src/SpikeRunner.cs` | `GiveUpNavigation`, Zeile 2426–2441 | Schreibt die Zeile `GAVE UP after …`. Die sichtbare `GAVE UP at Bungalow` kommt aus der Ride-Meldung um Zeile 1677. |

Die offene Weiterfahrt hat in diesem Stand keine eigene neue Ursache im Quelltext.
Der berechnete Pfad war gültig, der Guard hat nicht eingegriffen, und die
bestehende Recovery hat nach dem Stillstand am Barn-Ende aufgegeben.

## Umsetzung, nicht installiert

Die erste Recovery hat zurückgesetzt, obwohl diese Fahrt noch nie losgefahren war.
Das ist der Rückwärtsstoß vom Barn-Ende. `InitialDispatchRecoveryPolicy` in
`DrivingPolicies.cs` schickt genau diesen ersten Fall neu los, ohne Reverse.
Eine Fahrt, die schon gefahren ist und dann feststeckt, behält Reverse und
Ausweichziel.

Getestet: TaxiDriver **1.378/1.378**. Guard-Build ohne Deployment, 0 Warnungen,
DLL `3f70dd3cd8e8ebf5083caf565ed6a17ef7fa6c1cd71511e4f2edd1ce3f08d1f8`,
**nicht installiert**. Installiert bleibt `012330b1…`. Das beweist keine
Bungalow-Ankunft.

## Owner retest 2026-11-02

Dominik supplied the full run through 06:22:54. Loaded DLL `42b7e219…`, version
1.0.6, `mode=Guard`. Pickup halted 4.3 m away before normal boarding.

| Time | Evidence |
|---|---|
| 06:22:26.817 | Barn ARRIVED at 5.0 m; fare $18 |
| 06:22:33.698 | Bungalow whole-route Success, 285 points; individual seeker errors did not make this dispatch fail |
| 06:22:35.932 | First recovery at unchanged 329.7 m / 0 km/h / AutoDriving=true; reverse instead of the advertised no-drive redispatch |
| 06:22:37.438 | Directed reverse 3.2 m, another whole-route Success afterward |
| 06:22:43.247 | Lateral 5.1 m; checked road snap found no supported free point |
| 06:22:46.022 | New recurring reverse actually granted, 3-second window, attempt 2 |
| 06:22:49.027 | Backed 2.3 m, front still blocked; checked placement refused |
| 06:22:49.029 | GAVE UP at Bungalow, 332.4 m short, fare $7 |

### Confirmed decision defect in the owner-tested build

Line references in this diagnosis name the owner-tested `42b7e219…` source snapshot;
the later follow-up candidate below changes the relevant gate and diagnostics.

`SpikeRunner.cs:2774-2775` sets `NavEverAutoDriving=true` from the native flag,
independently of speed or displacement. `RecoverStuck`, `SpikeRunner.cs:1771-1772`,
passes it to `InitialDispatchRecoveryPolicy.PreferRedispatch` and skips the first
reverse only when it is false. `DrivingPolicies.cs:291` merely negates that input.
Thus an enabled but stationary native dispatch bypasses the intended no-start branch.
The existing two policy tests cover flag values, not enabled-versus-physical-motion.

Executed the unchanged production method and policy with native boundaries replaced
by counters: never-enabled and later-recovery controls pass; AutoDriving=true at
zero motion makes one reverse and no redispatch, violating the first-recovery contract.
Diagnostic result: **2 passed / 1 failed as expected**, not a green fix claim.

### Terminal and remaining uncertainty

`TickReDispatch`, `SpikeRunner.cs:2290-2303`, refuses forward dispatch when the front
remains blocked. At reverse attempt 2 it cannot retry reverse; refused free placement
then directly calls `GiveUpNavigation`, even with only two operation episodes spent.
The new reverse did run, but did not prove escape. Removing the obstacle gate or
blindly teleporting is not a justified repair. `RoadNavigation.cs:503-525` checks
coarse overlap/spherecasts; it does not log the blocking collider or an alternative exit.

At 06:22:33.425 handbrake=true and rear brake torque=1200 are confirmed. This is
the immediate post-dispatch sample, not proof of a latch throughout the 2.5-second
wait. The focused independent review completed: `ReleasePark` only clears intent,
but this sample is logged in the **prefix before native consumption**, not after it
(`DrivingPatches.cs:718-721`). Mode=None creates no state, so no corresponding
postfix readback exists (`DrivingPatches.cs:748-754`). The review of hash-matched
`GameAssembly.dll` (`61829050…`) reports that native ApplyThrottle consumes the
current handbrake input and rewrites its output (RVAs 0xAB320E, 0xAB34A3, 0xAB3668).
The parent independently recaptured the assembly hash and bytes at those RVAs;
native field identification remains attributed to the reviewer, not a runtime test.
Thus the first sample does not justify a new brake-release mutation. A bounded
Mode=None post-consumption sample after path Success and before recovery is the
missing measurement; this diagnostic extension has not been implemented yet.

Owner log and red diagnostic report are retained in the shared vault under
`raw/attachments/taxi-navigation-settings-2026-10-30/owner-20261102/`.
Log SHA-256 `886d7be93e2a78604e8227d87eb02cca37bd5a98ca6303efb3b84bdc58e21ac9`.
No new production edit, mod build, deployment, game control or save access during
this diagnosis. Barn-to-Bungalow acceptance remains failed.

## Owner acceptance — 2026-11-02 12:53 run

Dominik reports “Diesmal ging es” after the requested same-taxi Barn → Bungalow test.
The full supplied log has 853 lines, SHA-256
`2781f6458ef2efee5af5163f4adf3f57fb70c4f279e15253dd88edf82bdd8bc9`.
Loaded TaxiDriver 1.0.6 Guard `9d897a86…`; current installed hash still matches and
all 109 code/test hashes remain identical to the tested/reviewed candidate.

| Log lines | Observation |
|---|---|
| 640 | Barn ARRIVED at 5.0 m |
| 658–659 | Same op/order/frame: native None consumer clears hand true → false and rear brake torque 1200 → 0 |
| 678–679 | Initial calculation Failed; first recovery redispatches without reverse |
| 720, 732 | Later startup stall triggers a real directed reverse, 3.2 m confirmed, front clear |
| 785 | Back on the route after 21 nudges / 0 snaps |
| 848 | Bungalow ARRIVED at 3.4 m, callback Complete |

Two recovery episodes, zero GAVE UP lines in the complete supplied run. Later
post-consumption startup samples show hand=false and wheel brake=0 at zero speed,
so a sustained handbrake latch did not cause this run's startup stall. This does
not retrospectively supply the missing measurement in earlier builds.

The requested chain and successful recovery are **owner-confirmed and closed at
this scope**. One accepted journey does not prove every route, early boarding,
all recovery failure branches or multiplayer. No speculative follow-up patch or
repeat acceptance request. Raw source, line-numbered analysis and manifest retained
under the shared vault's `raw/attachments/taxi-navigation-settings-2026-10-30/owner-success-20261102-1253/`.
No production source/mod-build/deploy/game/save changes for this evidence update;
protected AGENTS row remains untouched. Acceptance-record isolated Pure
`abee387f8b9a` / `f8e82eeece98`: 19 passed / 3 failed (format, versions,
workspace-setup Git fixture), with docs and TaxiDriver 1,402/1,402 passing.
All 109 source/test hashes and installed artifact remained unchanged; no global pass claim.

## Physical-start follow-up candidate — 2026-11-02

On the continuation request, reproduced the physical-evidence contract in repository
tests before editing production: **4 failed / 3 controls**. `RecoverStuck` now calls
`InitialDispatchRecoveryPolicy.PreferRedispatch(HasCurrentForwardEvidence())`.
Evidence from another operation or target is rejected; native flag resets do not
erase genuine current-trip evidence. The skip still applies only to recovery count 0.

Separately reproduced the missing consumer outputs (**2 failed / 11 controls**) and
added passive Mode=None pre/post sampling. It runs only for an authorized active
operation in its first three seconds, 0.5 s apart with an eight-pair hard cap.
It creates no native control scope and writes no inputs. A stale order/operation/
vehicle, STOP, teardown, authority loss or player takeover rejects the postfix;
optional diagnostic state does not change exception propagation or hook health.
Readback includes native path count/calculation and target-speed context.

GREEN: **1,402/1,402**, Release Guard rebuild **0 warnings/errors**, scoped whitespace
checks clean. Decompiled candidate confirms Guard, physical-evidence call and startup
readback. DLL `9d897a865992506e87656b21876702668b43be91cb2e579e6d3327231c072d3a`,
still 1.0.6, **installed after independent delta review passed**. The reviewer checked
all 109 source/test hashes and both artifacts unchanged and ran 104 focused no-build
tests. No security or logic concerns; extra diagnostic getter/cleanup tests suggested
as non-blocking. This is not proof of native start/turn/escape or arrival.

Deployment at `2026-11-02T06:20:37.9787975Z`, attempt
`20261102T062037797Z-ef54ac60ea3a`: receipt status Deployed, five target hashes checked,
only DLL/PDB copied. Metadata, icon, GLB unchanged and Shared still byte-identical.
Installed DLL decompiled again: `Observe=false`. Backup
`20261102T062037834Z-TaxiDriver-14e8f1a8` verified against both before-install hashes
using its explicit `000-TaxiDriver.dll` / `001-TaxiDriver.pdb` mapping. Game closed;
no launch or save access. The protected AGENTS inventory row remains unchanged.

Isolated Pure snapshot `0ace9b534135`, verifier `a987d1927378`: TaxiDriver again
**1,402/1,402 passed**. Overall **18 passed / 4 failed**, with format, versions and
two Git fixture steps (`tool-clean-checkout.Tests`, `tool-workspace-setup.Tests`).
The clean-checkout error is additional in this run; do not claim an unchanged
three-failure total or a passing global gate. No tooling source changed.

## Historical candidate preparation — Build-Korrektur 2026-11-02

Der erste 1.0.6-Build wurde irrtümlich als Guard bezeichnet. Direkte Dekompilierung
der installierten `75a6ab63…`-DLL ergab `NativeShortcutPatch.Observe=true`.
Mit geschlossenem Spiel wurde dieselbe Quelle mit `-p:TaxiShortcutGuard=true`
ohne Deployment neu gebaut und danach nur TaxiDriver DLL/PDB installiert.
Kandidat und Ziel wurden als `Observe=false` geprüft; Ziel-SHA-256:
`42b7e219a0062758dedb2e33272e02fab9f462367cfc53280801658f3af914b9`.
Deploy-Quittung: `2026-11-02T05:04:53.6162072Z`, Backup
`20261102T050453430Z-TaxiDriver-75b369ca`. Erneut 1.382/1.382 Tests,
Release-Rebuild 0 Warnungen/Fehler. Zu diesem Zeitpunkt noch nicht gestartet;
der spätere fehlgeschlagene Eigentümerlauf ist oben erfasst.

## Historischer Nachtrag 2026-11-02, 05:35 Uhr — zweite Sitzung und Reparatur

### Neue Sitzung, gleiche installierte DLL

Sitzung 20:21:26 bis 20:23:25, Log
`26-11-1_20-21-26.log`, 108605 Bytes. Geladen war weiterhin `012330b1…`
mit `mode=Guard`, also der Stand **vor** `InitialDispatchRecoveryPolicy`.

| Zeit | Ereignis |
|---|---|
| 20:22:42 | Barn gewählt |
| 20:23:01 | `ARRIVED at Barn` — 5,0 m, Supervisor-Geschwindigkeit 0,8 km/h, Fahrpreis 18 $ |
| 20:23:04 | Bungalow als Weiterfahrt, erste Berechnung `Failed` |
| 20:23:05 | Recovery #1: 1,5 s Reverse, 3,4 m bestätigt |
| 20:23:06 | Re-Dispatch, Pfad gültig (283 Punkte, Ende `(-152.99, -4.00, 113.69)`) |
| 20:23:10 | `drifting off the road` lateral 3,1 m |
| 20:23:12 | `snap-back skipped: no free road spot`, lateral 5,0 m |
| 20:23:16 | Vollgas (`throttle=1.00`), `speed=0.0`, yaw 144,1; Kollisionspartner nicht bestimmt |
| 20:23:17 | Recovery #2: recurring stall, kein Freipunkt |
| 20:23:19 | Recovery #3: wieder kein Freipunkt → `GAVE UP` bei 334,3 m, Fahrpreis 6 $ |

Damit ist das Muster reproduziert: Barn kommt an, die Weiterfahrt vom
Barn-Straßenende scheitert an derselben Stelle wie am 19:37-Lauf.

### Ursache der Aufgabe

Ab Recovery #2 greift `RecoveryRecurrence.RegisterStall`: Die Position liegt
innerhalb von 6 m zum ersten Stall, also gilt der Stall als Wiederholung. Der
Zweig macht dann nur noch `TryFreeSpotRescue` und wartet auf einen freien
Straßenpunkt. Findet er keinen, folgt nach einer Nachprüfung `GiveUpNavigation`.
Ein physisches Manöver gibt es in diesem Zweig nicht mehr — obwohl das Auto mit
Vollgas stillstand. Eine Böschung als Kollisionspartner ist nicht belegt. Das ist die Stelle, an der die Meldung
„gibt auf, statt dass ein Recovery triggert" zutrifft: Es werden Episoden
gezählt, aber keine davon bewegt das Auto.

### Reparatur in 1.0.6

`SpikeRunner.TryBeginRecovery`, recurring-Zweig: Nach einer abgelehnten
Freipunkt-Rettung und **vor** der Nachprüfung wird genau **ein** gerichteter
Reverse pro Operation versucht (`_recurringReverseUsed`). Gelingt er, übernimmt
er den Re-Dispatch über `NavReDispatchAt`; ein zweiter Dispatch im selben
Zweig findet nicht statt. Ist das Heck blockiert, läuft es wie bisher in die
Nachprüfung und danach in die Aufgabe. Das Flag wird pro Operation
(`BeginNavigationOperation`) und pro Fahrt (`ResetDrivingState`) zurückgesetzt.

Zusätzlich ist `InitialDispatchRecoveryPolicy` jetzt mit installiert: Der erste
Recovery-Fall einer noch nie gefahrenen Fahrt setzt nicht mehr rückwärts.

### Prüfstand

| Punkt | Ergebnis |
|---|---|
| TaxiDriver-Tests | **1.382/1.382** bestanden |
| Erster Release-Build (nachträglich Observe bestätigt) | 0 Warnungen, 0 Fehler |
| Damals installierte DLL (inzwischen ersetzt) | `75a6ab63b3ae33385e515c62d8667800c85767da02a70cc00d594bb173f89fdb` |
| Version | 1.0.6; damalige Guard-Angabe war falsch, siehe aktuelle Korrektur oben |
| Vorherige DLL | `012330b1…` im Deploy-Backup `20261102T043456544Z-TaxiDriver-1724a9e8` |
| `verify.ps1 -Mode Quick` | 5 bestanden, 2 fehlgeschlagen — Format (TownHallInterior/WaterShader/WorldLook) und Versions-Drift (HitmanPhone/Minimap/PoliceTestMode), nicht TaxiDriver |

### Was das nicht belegt

- Keine Ankunft der Weiterfahrt. Der Reverse ist eine Rettung, keine
  reparierte Wende. Die frühere Fahrt driftete beim Wenden ab; der genaue
  Kollisionspartner und das Verhalten des neuen Builds sind nicht nachgewiesen.
- Kein Ingame-Lauf mit 1.0.6. Die Tests prüfen die Entscheidungsreihenfolge,
  nicht die native Fahrphysik.

### Nächster Test

1. Spiel über Steam starten, Slot 1 laden.
2. Im Log prüfen: `TaxiDriver` `1.0.6`, DLL `42b7e219…`, `mode=Guard`.
3. Taxi rufen, erst einsteigen, wenn es steht.
4. Barn wählen und Ankunft abwarten.
5. Ohne Aussteigen Bungalow wählen.
6. Beobachten, ob nach dem Stillstand ein `[reverse] recurring stall: requested …`
   erscheint und das Taxi zurücksetzt, statt `GAVE UP` zu melden.

Erwartete neue Logzeilen: `[nav] supervisor … trigger=recurring-stall-reverse
action=grant-reverse` und `[reverse] recurring stall: requested …` (nur bei erteiltem sicheren Reverse).


## Nicht behauptet

- Dass jede Barn-Fahrt jetzt ankommt.
- Dass Bungalow grundsätzlich unerreichbar ist.
- Dass ein bestimmter Navmesh-Tag die erste Fehlberechnung verursacht.
- Dass der Abbruch ein Speicher- oder Cloud-Fehler ist.
- Dass die sichtbare Halteposition auf der Straße oder im Bewuchs lag.
