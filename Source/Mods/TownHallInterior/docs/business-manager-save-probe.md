# TownHall: sichere inaktive Speicherprobe

Stand: 2026-11-06. Die korrigierte inaktive No-IO-Probe wurde im Spiel beobachtet;
anschließend wurde der Normalbuild wiederhergestellt. Phase 1 bleibt offen.
Die [historische Registrierungsprobe](business-manager-registration-probe.md)
bleibt ein eigener, nicht schreibender Nachweis. Der Innenausbau bleibt abgenommen.

## Technische Teilabnahme: synchroner No-IO-Pfad und Rückbau

Der Lauf `20261106T085601025Z.json` vom **6. November, 09:56:01 CET
(08:56:01 UTC)** auf `TownHallTest / SaveGame_2` verwendete den Kandidaten
`38bd7da3159c4ab9363695e0f9717e659322ca68e852d316a13181fa9eaab72a`.
Der [unveränderte Rohbericht](evidence/business-save-live-success-20261106/live-report.json)
hat SHA-256 `d8dc3fc70e33bfbd1f114492b997c0403bd2590b3c331b0a5f4e67a1d2c7fc38`.

- `Result.Status=observed`, Findings leer; `Save.Status=observed` mit bestätigter
  Registrierung, einem Save-Versuch, Rückkehr, `CompletionConfirmed` und `SafeToDestroy`.
- Eigene Saveables-Einträge vor/nach dem Versuch 0; Start/Completion 0/0 → 1/1.
  Der Fingerprint der 404 fremden Saveables sowie Register und fremde VTables
  blieben unverändert.
- `GetSaveString` und `WriteData` jeweils einmal, Getter-Diagnose achtmal.
  `RootDestroyed` und `CleanupVerified` sind true; die Callbackzähler blieben
  nach Cleanup unverändert.
- Fortschrittsmarker 2694 → 2698 stammen aus der synthetisch fortgeschriebenen
  Lesereihenfolge. Sie belegen keine vier vergangenen Unity-Frames.

Das beobachtet einen eigenen synchronen `ISaveable.Save`-Pfad ohne Dateischreiben.
Es wurde kein vollständiger `SaveManager.Save`-Durchlauf nachgewiesen.
`NativeSaveWriteAllowed=false` bleibt bestehen; Persistenz und Reload, Kauf,
Geldwäsche sowie Teilnehmer-/Codelebensdauer und Last-Use über Weltwechsel oder
Shutdown sind damit nicht abgenommen. Der generische `Limitation`-Text im
Rohbericht enthält ältere Aussagen über fehlende Verdrahtung; die konkreten
Result-/Save-Felder und der aktuelle Code beschreiben diesen späteren Versuch.
[Auswertung und Grenzen](evidence/business-save-live-success-20261106/observation.json).

**Abschluss:** Alle 39 nativen Testdateien waren gegenüber dem frischen Checkpoint
bytegleich, keine fehlenden oder neuen Dateien. Leere Baseline-Verzeichnisse waren
nicht inventarisiert; der Datei-Abgleich beweist deshalb keinen vollständigen
Verzeichnisvergleich. Nach geschlossenem Spiel wurden um **10:02:17 CET
(09:02:17 UTC)** Normalbuild `614b81fd…` und PDB `8126ae65…` wiederhergestellt;
der unabhängige Hash-/Quittungs-/Backup-Readback bestand. Kein erneuter
Laufzeittest des wiederhergestellten Normalbuilds.
[Save-Abgleich](evidence/business-save-live-success-20261106/save-verification.json),
[Rollback-Verifikation](evidence/business-save-live-success-20261106/rollback-verification.json),
[Deploymentquittung](evidence/business-save-live-success-20261106/rollback-receipt.json),
[Hashmanifest der Belege](evidence/business-save-live-success-20261106/manifest.json).

## Offline-Korrektur: Getter-Diagnose und Cleanup bei unklarer Wirkung

Die Statusprüfung des Kandidaten `3ffbc248…` fand zwei Integrationsfehler:
`ReadSaveState()` zählte den zur Vertragsprüfung gelesenen Getter
`ShouldSaveUnderFolder` beim folgenden Readback als Save-Start; außerdem zerstörte
`BusinessSubtypeTrial.Run()` das Probeobjekt im `finally` auch ohne bestätigte
Freigabe. Die Korrektur erhält die native Schreibsperre und den bestehenden
Einmalversuch. Sie erweitert die Probe nicht zu einem Business-Schreiber.

- `BusinessSaveCallbackLedger` bewahrt alle Callbackzähler. Nur `GetSaveString`
  und `WriteData` liefern Save-Callback-Evidenz; Getter-Diagnose bleibt sichtbar,
  ohne einen Save-Start zu behaupten. `ProbeBusinessSubtype` und der native Port
  verwenden diesen tatsächlich getesteten Produktionsbaustein.
- Die Cleanup-Entscheidung liest den aktuellen Save-Trial-Zustand. Ab dem vor
  nativer Registrierung gesetzten Intent bleibt das Objekt erhalten, solange
  nicht **CompletionConfirmed und SafeToDestroy** bestätigt sind. Das umfasst
  eine werfende Registrierung, auch wenn `Save()` noch nie aufgerufen wurde.
- Nach Eintritt in die Save-Vorprüfung erlauben Vorversuchsfehler Cleanup nur bei
  lesbarer, referenzfreier Vorprüfung.
  Ein fehlgeschlagener frischer Read verwirft den früheren sauberen Zustand.
  Unlesbare Daten, fremde laufende Speicherung, eigene Einträge oder Save-Signale
  halten die Freigabe geschlossen. Direkt vor jedem Cleanupversuch wird der
  Kontext erneut geprüft; die Diagnose nennt `save-cleanup-deferred`.
- Der vorhandene statische Runtime-Port hält die Referenzen. Es gibt weder
  spekulatives Abmelden noch einen erneuten Save-/Registrierungsversuch. Dieses
  Unterlassen eigenen Cleanups beweist keine Unity-Weltwechsel-, Codeunload-
  oder native Last-Use-Garantie.

**Prüfung:** Ausgangsstand 860/860 TownHall-Tests. Von 18 neuen Cleanup-/Ablauffällen
scheiterten 15 gegen die unveränderte alte Produktionsquelle. Zwei weitere
gezielte Prüfungen scheiterten bei separat wieder eingeführter alter Getter-
Klassifikation (Mutationsprüfung, kein unveränderter Altcode). Mit der Korrektur
bestehen **884/884 Tests**, darunter 24 neue Fälle. Der alte Context-Loss-Test
verlangt jetzt unterlassenes Cleanup statt Zerstörung trotz Unsicherheit.
Normal-, Subtype-, Interface- und Manager-Build bestehen ohne Deployment,
jeweils 0 Warnungen/0 Fehler. Unabhängiger Code-/Testreview akzeptiert den Diff.
Die Tests verwenden die Produktionslogik mit künstlichem Port und künstlichen
Completiondaten; sie sind keine native Laufzeitfreigabe.

[Prüfwerte, Quell-/Artefakthashes und isolierter Prüfstatus](evidence/business-save-integration-fix-20261106/verification.json),
[unabhängiger Review](evidence/business-save-integration-fix-20261106/independent-review.json),
[finaler TownHall-Lauf](evidence/business-save-integration-fix-20261106/townhall-green.trx).
Der korrigierte Manager-Build `38bd7da3…` wurde nach dem Offline-Auftrag am
6. November um 08:48:53 UTC mit frischem Checkpoint installiert und unabhängig
hashgeprüft. Die vorherige Probe `3ffbc248…` zeigte um 08:41 UTC gültige eigene
Virtual-Ziele, danach jedoch `manager-save-preflight-invalid` ohne SaveManager-
Registrierung oder Save-Versuch. Dieser Installationsschritt lag vor dem oben
ausgewerteten erfolgreichen Lauf; Phase 1 bleibt trotz der Teilabnahme offen.
[Installationsnachweis](evidence/business-save-fixed-deployment-20261106/deployment-verification.json),
[vorheriger Lauf](evidence/business-save-fixed-deployment-20261106/previous-live-report.json).

Die folgenden Abschnitte bewahren den Entwurfs-/Auditstand vom 5. November.
Seine Aussagen über damals fehlende Verdrahtung beschreiben nicht die spätere
Implementierung; der aktuelle Lauf, seine Grenzen und die Korrektur stehen oben.

## Historischer Auftrag und Abnahmegrenze vom 5. November

Die vorbereitete Manager-Variante zielte auf Dominik / SaveGame_1 und entfernte
ihren Eintrag unmittelbar nach `Save()`. Ein bedingter zweiter `Save()`-Aufruf
war kein zuverlässiger Abschlussnachweis. Diese Route darf nicht unverändert
installiert werden.

Für die neue Probe gelten folgende Kriterien:

1. Ausschließlich die vollständig geladene Singleplayer-Welt **TownHallTest /
   SaveGame_2** ist zulässig. Aktiver und geladener absoluter Speicherpfad müssen
   übereinstimmen. Explizite Argumente dürfen den schreibenden Pfad nicht auf
   Slot 1 oder eine andere Welt umleiten.
2. Genau ein tatsächlicher Speicheraufruf pro Prozessversuch. Ein Fehler,
   Timeout oder unbekanntes Ergebnis erlaubt keine Wiederholung.
3. Speicheraufruf, bestätigtes Ende und vollständiger eigener Rückbau sind
   getrennte Zustände. Die Rückkehr aus `Save()` oder ein einzelnes Idle-Flag
   sind allein kein bestätigter Abschluss.
4. Eigene Registrierung und Objektlebensdauer bleiben erhalten, solange der
   Speicherpfad noch darauf zugreifen kann. Bei unbekannter Wirkung bleibt ein
   klar ausgewiesener gesperrter Zustand; kein zerstörtes Objekt in einer
   noch benutzten nativen Liste.
5. Kontextwechsel, unlesbarer Zustand, doppelte Ereignisse und Teilfehler
   dürfen weder einen zweiten Save noch eine Mutation im nächsten Slot auslösen.
6. Der normale Modbuild enthält keinen Laufzeit-Speicheradapter. Kein neuer
   Detour auf gemeinsam genutzte native Spielmethoden; Aktivierung bleibt gesperrt.

## Implementierter Schutz und Zustände

`ConstructionGate.CheckSave` hat keine frei wählbare Weltfreigabe. Ein explizit
passender Hauptslot wird ebenso abgewiesen wie eine falsche Organisation oder
unterschiedliche Active-/Loaded-Pfade. Der alte synchrone `probeSave`-Pfad im
`BusinessSubtypeTrial` liefert im richtigen Testslot **`blocked /
native-save-completion-contract-unverified`**, bevor ein Port gelesen, ein Typ
registriert oder ein Save aufgerufen wird. Der Modus verbraucht dabei keinen
nativen Versuch. Im Bericht Schema 3 bleibt `NativeSaveWriteAllowed=false`.
Die zwei bisherigen `save.Save()`-Aufrufstellen und `SaveRegistered()` sind
vollständig entfernt; es gibt keinen globalen Save-Blockierhook.

Die reine `BusinessManagerSaveTrial` modelliert den später benötigten Besitzer:

| Zustand | Erlaubte Folge |
|---|---|
| `skipped` / `blocked` | Keine Registrierung oder Speicherung. Ohne bestätigten Abschlussvertrag nicht starten; keine Erlaubnis, vorhandene oder unbekannte Referenzen zu zerstören. |
| `failed` vor dem Versuch | Ausgangszustand nicht lesbar. Derselbe Besitzer bleibt terminal; es wurde noch kein nativer Versuch verbraucht. `SafeToDestroy=false`. |
| `pending` | Genau ein Speicheraufruf wurde versucht; Registrierung nicht entfernen, Objekt nicht freigeben. |
| `observed` | Nur im Modell: unter einem ausdrücklich vorausgesetzten Abschlussvertrag wurden Start/Complete, Ruhe, leere Queue, kein aktiver Callback und ein Frame nach der ersten Completionbeobachtung geprüft; Entfernen und Readback bestätigt. |
| `quarantined` | Mögliche Teilwirkung, unklarer Abschluss, Fehler oder Timeout. Kein zweiter Save, keine spekulative Entfernung, `SafeToDestroy=false`. |

Manager, Listenidentität, absolute normalisierte Weltpfade, Generation und fremder
Saveables-Fingerprint werden eingefroren. Beobachtungsframe und Ereigniszähler
dürfen nicht zurücklaufen. Dispatchframe und erste Completionbeobachtung werden
getrennt ausgewiesen; im ersten Completion-Snapshot wird noch nicht entfernt.
`SafeToDestroy` beginnt false und wird ausschließlich nach bestätigter Entfernung
und abschließendem Readback true. Reentranz kann keinen zweiten Versuch starten;
Zeitüberschreitung ist terminal. Auch die Entfernung wird nicht bei unbekanntem
Ergebnis wiederholt. **Das Modell ist nicht an einen nativen Save-Aufruf
angeschlossen.** Kein Laufzeitadapter setzt `CompletionContractVerified=true`.
Eine wahre Modellannahme im Test ist weder eine Spielbeobachtung noch eine
Freigabe, die Sperre zu entfernen.

## Statischer Quellenbefund und technische Lücke

Der frühere unabhängige Quellencheck vom 2026-11-05 unterscheidet
(Erkenntnisstand vor dem nachfolgenden Native-Audit):

- `ThirdParty/S1API/S1API/Lifecycle/GameLifecycle.cs:190,282–290` bindet das
  native `SaveManager.onSaveComplete` und leitet es direkt weiter. Das ist **kein
  Save-Postfix**. Die passende installierte S1API-3.2.2-Assembly wurde vom Prüfer
  separat statisch abgeglichen.
- `ThirdParty/S1API/S1API/Internal/Patches/GenericSaveables.Patches.cs:40–71`
  ist dagegen ein Postfix auf `Save(string)` und schreibt zusätzliche Moddaten
  nach dessen Rückkehr. Daraus folgt keine gemeinsame atomare Speicherung.
- Die nativen `SaveManager`-/`SaveRoutine`-Rümpfe sind in den vorhandenen
  IL2CPP-Proxys nicht sichtbar. Reihenfolge von Queueverarbeitung, Dateiarbeit,
  Idle-Flag, Completion-Event und letzter Referenznutzung blieb in diesem
  damaligen Proxy-/Managed-Check unbelegt. Der spätere Abschnitt unten ergänzt
  echte native Teilfolgen, nicht jedoch einen vollständigen Freigabevertrag.
- Das parameterlose Completion-Event trägt weder Request-ID noch Zielpfad.
  Complete + Idle + leere Queue + Folgeframe sind daher allein kein etablierter
  nativer Freigabevertrag. Auch `CompletedSaveables` ist keine dokumentierte
  per-Request-Erfolgsquittung.
- Bei Main-Unload setzt S1API den Lifecycle zurück und entfernt die nativen
  Listener (`S1API.cs:93–95`, `GameLifecycle.cs:210–218`). Ein werfender früherer
  Teilnehmer kann außerdem spätere verwaltete Teilnehmer überspringen. Eine
  retained Root allein verhindert keinen Zugriff des persistenten Managers aus
  einer späteren Welt und wäre keine Slotisolation.

**Konsequenz, weiterhin gültig:** Fail-Closed vor Registrierung und Save bleibt
der sichere Laufzeitpfad; keinen Eventhandler allein als Freigabe verdrahten.

## Späterer Native-Audit vom 2026-11-05

Nach dem abgeschlossenen Start-/Lade-/Handy-Basistest wurde auf „bereit“ rein
lesend der tatsächliche Maschinencode untersucht. Passendes GameAssembly-/
Metadatenpaar, frisches Mapping und Byteprüfungen ersetzen für die ausgewählten
Methoden die frühere reine Proxygrenze. [Vollständiger Audit und Belege](native-save-completion-audit.md).

- `Save()` springt auf dem sichtbaren Pfad zu `Save(string)`; der zulässige Pfad
  startet eine Coroutine, liefert aber keine eigene Abschlussquittung.
- `SaveRequest.Complete` entfernt seinen Queueeintrag **vor** dem
  `WriteBaseData`-Interfacedispatch. Der Batchsnapshot hält Requests unabhängig
  davon weiter. Leere Queue bedeutet nicht Ende des gerade laufenden Requests.
- Die Coroutine kann mit Restqueue abschließen. Der Warnzweig leert
  **CompletedSaveables**, nicht QueuedSaveRequests. Bei queueleerem Abschluss
  wird Completed in diesem Zweig nicht geleert; eigene Referenzen können bleiben.
- `IsSaving=false` und `onSaveComplete` liegen **vor** der Fehlerauswertung.
  Das Ereignis ist keine Erfolgsquittung. Reentranz kann außerdem alte und neue
  Zähler-/Fehlerzustände vermischen; Welt-/Requestkorrelation bleibt offen.

**Offen bleibt der eigene Last-Use-/Entfernungsvertrag**, einschließlich aller
relevanten Halter, aktiven Callbacks und Weltwechsel. Die untersuchten Fenster
beweisen keine vollständige Exception-/Transitivabdeckung oder korrekten
Dispatch aller Slots der injizierten Klasse. `NativeSaveWriteAllowed=false`
bleibt unverändert; keine Produktions-/Teständerung, Builds, Installation,
Spielaktionen, Speicheraufrufe oder nativen Save-Dateizugriffe in diesem Audit.

## Weltwechsel-Nachtrag vom 2026-11-05

Der anschließende [Offline-Weltwechselaudit](native-world-exit-audit.md) ist
abgeschlossen. `Clean` hängt als instanzgebundener Delegate an PreSceneChange,
leert aber nur Saveables/BaseSaveables. Exit invalidiert die aktuelle Ladeidentität
vor dem Event; die Exit-Waits betreffen feste Zeit beziehungsweise Playerantwort
oder Timeout, nicht den eigenen Save-Drain. Der native SaveManager folgt einem
persistenten Singleton-Pfad; eigene `Leaving`/`Stop` liefern keine Altauftrag-
Übergabe oder Last-Use-Quittung. Unity-interner Coroutineabbruch bleibt ungeklärt.

**Architekturentscheidung, keine Umsetzung:** Die synchrone transiente Probe
nicht lediglich durch Event-/Folgeframewarten zum asynchronen Schreiber erweitern.
Zunächst eigenen Operationsbesitz mit eingefrorener Altidentität, Widerruf und
ausdrücklich geprüfter Teilnehmerlebensdauer entwerfen; auch dauerhafter Retain
allein wäre keine Lösung. Kein RuntimeSaveAdapter und keine Lockerung der Sperre.

## Besitzentwurf und nachfolgender reiner Datenbaustein vom 2026-11-05

Der [nichtschreibende Auftragsvertrag](business-save-operation-owner.md) ist
spezifiziert und unabhängig auf Entwurfskohärenz geprüft: neun `accepted`, ein
`limited`, keine Pflichtkorrektur. Optionale Präzisierungen zu Effekt-/Widerrufs-
ordnung und Gegenfällen sind übernommen; konkrete Synchronisation bleibt offen.
Der nachfolgende reine Datenbesitzer ist jetzt implementiert: 128 neue Fälle und
abschließend 809 TownHall-Offlinetests bestanden. Separater Semantikreview und
33 eigene Probegruppen, einzige Pflichtkorrektur nur Testwhitespace danach
separat geprüft. Isoliertes Pure bleibt insgesamt fehlgeschlagen; eigene
Formatstelle korrigiert, bekannter fremder Versionsdrift nicht Teil dieses Auftrags.
Nur künstliches Prozessmodell mit gemeinsamem Slot je Modellobjekt, kein
Runtime-Port, Unity-/Dateizugriff oder Modverdrahtung. Der bestehende Save-Trial
und seine bedingten Fakebelege bleiben unverändert; kein neues
`CompletionContractVerified=true`, kein nativer Einstieg oder Installation.

## Offline-Prüfung und Artefaktgrenze

Ausgangsstand `b2c6c7e`: 416 reine TownHall-Tests bestanden. Acht neue konkrete
Abweisungsregressionen scheiterten zuerst mit `Expected skipped / Actual
observed`; 20 weitere Kontrollen bestanden. Der ursprüngliche RED-Lauf gehörte
zur Test-Coder-Arbeit, nicht zu einem Live-Spieltest. Die erlaubten Testslotfälle
erwarten nach der Fail-Closed-Entscheidung ausdrücklich `blocked`, keinen
Save-Erfolg. Historische nichtschreibende Manager-Tests bleiben erhalten.

**Abschließende fokussierte Prüfung:** 681/681 TownHall-Tests bestanden
(265 zusätzliche Fälle gegenüber der Baseline). Normal-, Manager- und
Interfacebuild jeweils 0 Warnungen/Fehler, ausschließlich `S1NoDeploy=true` bei
geschlossenem Spiel. Die alte Aktivierung scheitert weiterhin erwartungsgemäß
an CS1029. Scoped Format bestanden. Testpfade sind künstliche Zeichenketten,
keine echten Save-Verzeichnisse.

Der getrennte Coderlauf fand zunächst drei weitere Fehler (Wiederaufnahme nach
unlesbarem Preflight und Widerruf während Save); nach Korrektur bestehen 185/185
Save-/Gate-Fälle. Der unabhängige Audit reproduzierte anschließend acht konkrete
Assertions zu zu großzügigem `SafeToDestroy` und zu früher Framefreigabe.
Nach Korrektur bestehen **dieselben 80 Auditfälle unverändert**. Die positiven
Codertests wurden der strengeren Folgeframegrenze angepasst, nicht die
unabhängigen Angriffsassertions abgeschwächt.
[Ursprünglicher Audit](evidence/business-save-safety-review-20261105.json),
[unabhängiger Abschlussreview](evidence/business-save-safety-review-closed-20261105.json),
[Test-/Buildbelege und Quellhashes](evidence/business-save-offline-20261105.json).

Ein erster Gesamttest mit eigenem `--artifacts-path` bestand 672 Fälle und
scheiterte in neun vorhandenen Quelltext-Fixtures wegen deren festen relativen
Rootannahmen. Ohne dieses abweichende Ausgabelayout bestanden alle 681 Fälle;
keine sachfremde Fixturekorrektur. Der Fehlaufruf bleibt separat erhalten.

Repository-Quick: sechs Schritte bestanden, nur vorbestehender `versions`-Drift
bei HitmanPhone, Minimap und PoliceTestMode fehlgeschlagen. Die separate
isolierte Pure-Prüfung und ihre Nachträge werden im Evidence-Verzeichnis
ausgewiesen; keine pauschale Repository- oder Gameplay-Freigabe durch diese
fokussierten Ergebnisse.

Historisch in diesem früheren Offline-Schritt am Installationsort gelesen: TownHall-DLL SHA-256
`cff67a85c0d922ea0aec7bd8ffc0affcf9fbb23a1576d1cd57c908fda165fa6a`.
Das ist nicht der historische Registrierungsbeleg `473064e0…`. Dieser Auftrag
hat sie nicht installiert und erklärt weder ihre Herkunft noch Laufzeitverhalten
oder Gleichheit mit dem aktuellen Source-Checkout. Auch PDB, Metadaten, Shared
und S1API werden für den Nicht-Deployment-Abgleich separat gehasht; native
Spielstanddateien werden dafür nicht gelesen.

Der spätere [Startvorbereitungsauftrag](launch-preparation.md) identifizierte
diese DLL als alte verdrahtete Slot-1-/Doppel-Save-Probe und ersetzte sie auf
ausdrückliche Auswahl durch Normalbuild `614b81fd…` ohne Business-Proben.
Der oben beschriebene Native-Audit verändert diese Installation nicht.

## Spätere Live-Arbeit (nicht Teil von Schritt 1)

Ein Live-Versuch braucht einen separaten Auftrag und einen frischen geprüften Checkpoint einschließlich
relevanter Moddaten sowie dokumentierten Cloud-Zustand und Rückweg. Ein nativer
Save kann auch fremde Saveables und Mod-Save-Handler ausführen; die eigene
no-base Business-Shell macht einen Save der ganzen Testwelt nicht schreibfrei.
Der begrenzte Start-/Lade-/Handy-Ausgangstest ist inzwischen abgeschlossen
([datierter Nachweis](launch-preparation.md)); er wird hier nicht erneut geöffnet.

Ein bestätigter Managerdurchlauf ist noch kein Nachweis gespeicherter und
wiederhergestellter Business-Daten, kein Kauf und keine Geldwäsche. Frühe
Business-Registrierung und genau ein Wiederherstellungsweg bleiben Kriterien
von Phase 1 des [Fertigstellungsplans](business-completion-plan.md).
