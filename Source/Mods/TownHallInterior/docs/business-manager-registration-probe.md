# TownHall: inaktive Speicherlisten-Registrierungsprobe (`RegisterSaveable`)

Stand 2026-11-04. Die Probe gehört zur
[ISaveable-Interfaceprobe](business-interface-probe.md) und benutzt denselben
inaktiven Business-Untertyp ohne NetworkObject. Sie prüft ausschließlich, ob die
eigene Business-Shell **einmalig und umkehrbar** in der nativen
`SaveManager`-Speicherliste registriert und anschließend wieder vollständig
entfernt werden kann. Kein Save, keine Aktivierung, kein Save-/Load-Nachweis.

## Beobachteter Live-Lauf 2026-11-04, 07:11:05 UTC

DLL `473064e0…`, Prozess ab 07:10:27 UTC, Ziel **Dominik / SaveGame_1** (Slot 1,
freigegeben), Mode `inactive-saveable-registration`, Schema 2. Ergebnis
`observed`, keine Befunde:

- `SaveManager.RegisterSaveable(…)` genau einmal aufgerufen; Saveables
  Before 404, Added 1, Remaining 0, Removed true; `Save()`, `WriteData`,
  `GetSaveString` und die Manager-Initialisierung unaufgerufen.
- `Interface` null, `InterfaceEnabled` false, `InterfaceStages` leer,
  `InitializeSaveableCalls` 0, `UnexpectedLifecycleCalls` 0.
- Zehn Register und 14 fremde VTables in allen fünf Stufen
  (Before/Registered/Constructed/Dispatched/After) identisch;
  `TypeSlotsValidated` true; `RootDestroyed`/`CleanupVerified` true;
  `TypeRegistrationReversible` false (prozessweite Registrierung bleibt).
- `WorldPublicationAllowed`, `NativeSaveWriteAllowed`, `GameMethodDetoursAdded`
  alle false; Singleplayer, `LoadComplete`, `HallReady`, `DependenciesMatch`;
  aktiver und geladener Save-Pfad identisch; Organisation `Dominik`;
  Generation 2; `AttemptConsumed` true, `CleanupComplete` true.
- Limitation (Wortlaut des Berichts, gekürzt): Die Typregistrierung bleibt bis
  zum Prozessende bestehen; Cleanup betrifft den eigenen Unity-Root und die
  erworbene Business-Komponente; Zähler sind begrenzte Beobachtungen; die
  eingebrachte Route ist genau ein `RegisterSaveable`-Aufruf mit In-Memory-
  Entfernung. `Save`, `WriteData`, `GetSaveString` und Manager-Init werden
  nicht aufgerufen.

[Bericht](evidence/business-manager-live-register-saveable-20261104.json)
(SHA-256 a23caf50…; Original unter `UserData/TownHallInterior/BusinessManager`).
Der Lauf hinterlässt den Slot laut Fertigstellungsplan unverändert; der Bericht
selbst weist keine Dateizählungen aus.

## Herkunft und aktueller Quellstand

Die zum Lauf gehörende Buildvariante (`TOWNHALL_BUSINESS_MANAGER` mit
`ProbeManager=true`/`ProbeSave=false`, `ProbeMode="inactive-saveable-registration"`)
ist im aktuellen Arbeitsbaum **nicht mehr enthalten**. Der Stand seit
2026-11-04 08:15 verdrahtet stattdessen die **Save-Probe**:
`ProbeMode="inactive-saveable-save"`, `ProbeSave=true`,
`ExpectedOrganisation="Dominik"`, `ExpectedSlot="SaveGame_1"`. `SaveRegistered()`
in `BusinessConstruction.cs` registriert, ruft tatsächlich `save.Save()` auf
(danach erneutes `if (!save.IsSaving) save.Save();`) und entfernt den Eintrag im
`finally`. Dazu `BusinessManagerTests.cs` (Richtlinie: das Gate akzeptiert
Dominik/Slot 1 nur bei expliziter Angabe). Dieser Stand ist **nicht gebaut,
installiert oder ausgeführt** worden; er setzt vor jeder Ausführung eine
frische, geprüfte Sicherung von SaveGame_1 und einen ausdrücklichen Go voraus.

## Ablauf und Grenzen (`RegisterOnce()`)

`Context-Gate → Baseline → eigene Typregistrierung (prozessweit, irreversibel) →
inaktive Komponente → Property-Dispatch → … → RegisterOnce():
PersistentSingleton<SaveManager>.Instance prüfen (NetworkGuard.IsAlive,
`!save.IsSaving`) → Saveables zählen → `CountPointer == 0` erwartet (sonst
`already-registered`) → `RegisterSaveable` → RequireContext erneut →
`!save.IsSaving` prüfen → `Added == 1 && RemovePointer` → `Remaining == 0`
prüfen → Rückbau → Endvergleich`.

- Die Registrierung ist **in-memory**: Der Eintrag wird sofort wieder entfernt;
  Saveables-Register und Vtables sind vor/nach identisch.
- Kein `Save()`, kein Serialize, keine Aktivierung, keine Manager-Initialisierung,
  keine zweite Instanz, keine fremden Native-Detours in diesem Pfad.
- Ein Teilfehler wird als `failed` protokolliert, auch wenn der Rückbau gelingt.
- Das Gate kennt zwei bewusst getrennte Freigaben: den Default
  (TownHallTest/SaveGame_2) und die explizite Angabe (Dominik/SaveGame_1). Der
  Live-Lauf vom 2026-11-04 benutzte die explizite Freigabe.

## Was diese Probe nicht beweist

- Kein Save-/Load-, kein `WriteData`-/`GetSaveString`-Nachweis; keine native
  Business-Initialisierung, keine Aktivierung; keine Kauf-/Geldwäsche-Wirkung.
- Kein Beweis, dass `Save()` mit registriertem eigenem Saveable fehlerfrei
  läuft — die Save-Probe steht aus.
- Der Aufruf lief auf Slot 1 des Eigentümers; freigegeben, aber die Route bleibt
  eine Testoperation und wird niemals publiziert (`WorldPublicationAllowed` false).

## Prüfstand und nächste Schritte

Die Offline-Tests für den Manager-Zustand liegen in `BusinessManagerTests.cs`
(Stand 2026-11-04, uncommitted); ihr Ergebnis ist nicht dokumentiert. Der
Live-Lauf ist der einzige Ausführungsnachweis dieser Probe; ein unabhängiger
Review für die Manager-Variante liegt nicht vor. Ohne ausdrücklichen Go des
Eigentümers: kein Build und kein Start der Save-Probe (`inactive-saveable-save`).

Der nächste Live-Ausgangstest ist der unveränderte Start-/Lade-/Handytest mit
S1API 3.2.2 auf TownHallTest / SaveGame_2 (siehe
[Fertigstellungsplan](business-completion-plan.md#voraussetzungen-für-spätere-ingame-prüfungen)).
