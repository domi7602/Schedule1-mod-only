# TownHall: Weltwechsel und Abbruch offener Speicherarbeit

Prüfung und Aufnahme: 2026-11-05. **Offline-Audit abgeschlossen; keine Save-,
Unregister- oder Destroyfreigabe.** Nur Analyse und Dokumentation, keine Änderung
an Produktions-/Testquellen oder Installation. Der
[vorige Speicherabschlussaudit](native-save-completion-audit.md) und seine Belege
bleiben unverändert; dieser Bericht ergänzt ausschließlich Weltwechsel und Besitz.

## Ergebnis und Architekturentscheidung

Die untersuchten nativen Weltwechselpfade liefern **keine belastbare letzte
Nutzungs-/Entfernungsquittung für unsere transiente Business-Probe**:

- `SaveManager.Clean` ist an `LoadManager.onPreSceneChange` gebunden, leert lokal
  aber nur Saveables/BaseSaveables. Das ist kein Drain sämtlicher Speicherhalter.
- Der geladene Exit-Pfad verwirft die aktive Ladeidentität vor diesem Event.
  Die Exit-Wartebedingungen betreffen Zeit beziehungsweise Playerdaten, nicht
  den Abschluss sämtlicher SaveManager-Requests und eigener Callbacks.
- Der SaveManager verwendet den persistenten Singleton-Pfad. Szenenwechsel
  bedeutet deshalb nicht automatisch Zerstörung des Managers oder seiner Halter.
- `SaveRoutine.Dispose` enthält keine eigene Abschlusslogik. Die Unity-interne
  Abbruch-/Zerstörungssemantik ist aus der untersuchten Aufrufhülle nicht bewiesen.
- Unsere eigenen `Leaving`-/`Stop`-Pfade invalidieren nur; sie besitzen noch
  keinen eingefrorenen Altauftrag mit geprüfter nativer Lebensdauer.

**Entscheidung für die weitere Entwicklung:** Die synchrone, kurzlebige Probe
nicht durch Entfernen der Sperre, einen Eventhandler oder einen zusätzlichen
Warteframe zum asynchronen Schreiber machen. `NativeSaveWriteAllowed=false`
bleibt die vorhandene sichere Grenze **vor Registrierung und Save**. Der nächste
Baustein ist ein gesonderter, zunächst nichtschreibender Entwurf des eigenen
Operationsbesitzes, nicht ein weiterer unkorrelierter Completion-Hook.

Ein dauerhaft gültiger, nach Widerruf inaktiver Teilnehmer wäre eine zu prüfende
Architekturalternative; bloßes `DontDestroyOnLoad`, Retain bis Prozessende oder
ein eigener Callbackzähler reicht dafür nicht. Auch diese Alternative ist weder
implementiert noch freigegeben. Für eine weiterhin transiente Instanz wäre
stattdessen ihre vollständige sichere Entfernung vor möglicher weiterer Nutzung
zu beweisen. **Eigene Freigabe, nativer Schreiberfolg und Konsistenz zusätzlicher
Moddaten bleiben drei verschiedene Beweisziele.**

## Eingaben und Aussageumfang

Basisrevision `b2c6c7e6c6ecc778da9624cdb68a6578b9db471a`, Arbeitsstand `dirty`.
Frische Eingabeaufnahme: `2026-11-05T16:35:51.543004+00:00`; kein Spielprozess,
`DOTNET_ROOT` nicht gesetzt. Die sieben benannten Eingaben und 41 geschützten
Produktions-/Testquellen waren gegenüber dem vorherigen Audit hashgleich, ebenso
dessen 135 archivierte Belege und fünf vorhandene Analysebibliotheken.
[Eingabeaufnahme](evidence/native-world-exit-audit-20261105/input-manifest.json).

| Byteinput | SHA-256 |
|---|---|
| GameAssembly.dll | `61829050e56ca180d67be592e4bfe61ec97d2e9573f720d90a54e318a7e4381d` |
| global-metadata.dat | `e775c97da277da8eb1a90265478f3b65c9c3d6464aff4f7255d45280a1d1181f` |
| installierter Normalbuild | `614b81fd584040d4cebe7a18929bca09eabae478a6487ec6d846ea01d6062003` |
| installierte S1API 3.2.2 | `d751a3cb66003f7a616eb9f8054daf34da0d9d7f23416a5005a586738a680852` |

Das passende Metadatenpaar wurde erneut mit vorhandenen gehashten LibCpp2IL-/
Iced-Bibliotheken gelesen; Spielassemblies blieben Byteinputs, nicht geladen oder
ausgeführt. Parserfassung 31.1, ImageBase `0x180000000`; 149.349 Methoden global,
zuletzt 979 Methoden/446 Felder im gezielten Inventar.
[Frisches Mapping](evidence/native-world-exit-audit-20261105/native-map-pass4.json).

24 native Fenster in Pass 2/3/4 umfassen **19.856 Bytes**, 4.621 lineare
Decoderzeilen und 1.416 relative Transfers. Das sind **keine 4.621 bewiesenen
ausgeführten Codeinstruktionen**: Padding und Daten sind enthalten. Insbesondere
neun Zeilen liegen in der StartGame-Sprungtabelle; 23 Suffixbytes bleiben in der
Originalausgabe ausdrücklich undekodiert. Der unabhängige Bytecheck ordnet die
elf RVA-Tabelleneinträge ab `0x592560` über den Dispatcher bei `0x590DD9`,
`0x590DF2`, `0x590DFC` zu, ohne die Originalausgabe umzuschreiben.
[Bytecheck und Datenergänzung](evidence/native-world-exit-audit-20261105/native-byte-integrity.json).

Der erste Export scheiterte bei `0x180592579`; Fehlerprotokoll und ursprünglicher
Exporter sind erhalten. Der erfolgreiche Folgeexport archiviert den Suffix als
Rohbytes. Nächster Methodenpointer und primäre `.pdata`-Grenzen sind nur lokale
Analysefenster, **keine vollständige Funktions-, EH-, CFG- oder Transitivabdeckung**.
Offene generische Feldoffsets können unaufgelöst/0 sein; daraus wird kein gültiges
Singleton-Feldlayout abgeleitet.

## Native Befunde mit Fundstellen

Alle folgenden Adressen sind RVAs des oben gehashten GameAssembly-Inputs.
Die IDs entsprechen den eingefrorenen
[Reviewbehauptungen WX-01 bis WX-08](evidence/native-world-exit-audit-20261105/world-exit-claims.json).

### Clean ist ein Eventteilnehmer, keine Drainquittung — WX-01/02

`SaveManager.Start` liest bei `0x5AAD59` das LoadManager-Event `+0x78`, erzeugt
einen UnityAction-Delegate für `Clean` mit **seiner SaveManager-Instanz als Ziel**
(`0x5AAD69`, `0x5AAD73`) und ruft bei `0x5AAD8F` `UnityEvent.AddListener` auf.
[Start](evidence/native-world-exit-audit-20261105/native-disassembly-pass2/0x06001824-SaveManager-Start.txt).
Null direkte Roh-Xrefs zu `Clean` bedeutet daher nicht „wird nicht aufgerufen“.
Die Registrierung ist ein statischer Pfadbeleg, keine beobachtete Zustellung
oder Reihenfolge aller Listener in jeder Sitzung.

Der lokale `Clean`-Rumpf leert `Saveables +0x48` (`0x5A9511` ff.) und
`BaseSaveables +0x50` (`0x5A953B` ff.). Er leert dort weder
`CompletedSaveables +0x60` noch `QueuedSaveRequests +0x68` und setzt nicht
`SaveManager.IsSaving +0x29` zurück.
[Clean](evidence/native-world-exit-audit-20261105/native-disassembly-pass2/0x06001834-SaveManager-Clean.txt).
Das schließt andere Bereinigung an anderer Stelle nicht aus. Es liefert aber
keine Quittung für Requests, unabhängige Batchreferenzen oder laufenden Dispatch.

### Exit invalidiert die aktuelle Identität vor PreSceneChange — WX-03

Auf dem geladenen Pfad von `LoadManager.ExitToMenu`:

1. `0x586BCB`: `IsGameLoaded +0x28 = false`.
2. `0x586BD1`: `ActiveSaveInfo +0x40 = null`.
3. `0x586BE8`: **LoadManager.IsLoading** `+0x29 = true`.
4. `0x586C5C`: Start seiner Exit-Coroutine.
5. Erst diese ruft bei `0x5AE418` `onPreSceneChange +0x78` auf.

[ExitToMenu](evidence/native-world-exit-audit-20261105/native-disassembly-pass2/0x060017BC-LoadManager-ExitToMenu.txt),
[Exit-Coroutine](evidence/native-world-exit-audit-20261105/native-disassembly-pass2/0x06001806-__ExitToMenu_g__Load_0_d-MoveNext.txt).
Das gleich nummerierte `SaveManager.IsSaving +0x29` ist ein anderes Objektfeld.
Ein eigener PreSceneChange-Handler darf die alte aktive Identität deshalb nicht
erst durch Abfrage des dann aktuellen Kontextes rekonstruieren.

Die Coroutine stoppt danach Netzwerkverbindungen (`0x5AE4DA`, `0x5AE4FE`), lädt
`Menu` asynchron (`0x5AE5E0`) und ruft SceneChangeDone bei `0x5AE670` auf. Der
sichtbare Fehlerlogpfad zum PreSceneChange-Invoke führt ebenfalls vor dem späteren
„Pre scene change event invoked“-Log zusammen: Diese Logzeile allein wäre keine
Quittung erfolgreicher Listenerausführung. Vollständige EH-Zustellung bleibt offen.

### Exit wartet auf Zeit/Playerantwort, nicht auf unseren Save-Drain — WX-04

Im `IsServer`-Zweig wird `WaitForSecondsRealtime(1.25f)` erzeugt
(`0x5AE945` bis `0x5AE96D`; Konstante bei `0x2E5CC60`). Der andere Zweig ruft
`Player.RequestSavePlayer` (`0x5AEA07`) und setzt `maxWait=3.0f`
(`0x5AEA11`) sowie `timeOnWaitStart` (`0x5AEA1F`). Auf dem nichtnullen normalen
Playerpfad prüft sein WaitUntil-Delegate:

`Player.Local.playerSaveRequestReturned || realtimeSinceStartup - timeOnWaitStart > maxWait`.

Das Playerfeld liegt bei `+0x330`; der Vergleich bei `0x5B3AB5` ist strikt `>`.
[Predicate](evidence/native-world-exit-audit-20261105/native-disassembly-pass3/0x0600180B-__c__DisplayClass81_1-_ExitToMenu_b__1.txt),
Feldzuordnung und Konstanten im Mapping/Bytecheck oben. Ein Timeout kann die
Bedingung erfüllen, ohne eine erfolgreiche Playerantwort zu beweisen. Keine der
beiden lokalen Bedingungen bestätigt den letzten Nutzer unserer Business-Instanz
oder leere Request-/Batch-/Completed-Halter. Das ist keine Multiplayer-Abnahme.
Nach dem normalen Client-WaitUntil-Resume führt `0x5AE91A` ebenfalls zu
`0x5AE950`, also durch die gemeinsame feste 1,25-Sekunden-Realtimewartezeit.
Sie ist nicht exklusiv serverseitig. Der spätere 0,5-Sekunden-Wait nach
RefreshSaveInfo ist ein weiterer getrennter Schritt, ebenfalls kein Drainnachweis.

### Persistenter Manager und wechselnder Kontext — WX-05/06

`SaveManager.Awake` ruft bei `0x5A8FEB` den gemeinsamen
`PersistentSingleton<SaveManager>.Awake`-Code auf. Dieser ruft `Singleton.Awake`
(`0x10709F2`) und prüft danach das native Duplikatflag bei `+0x20`; auf dem
nicht als Duplikat markierten Pfad folgen `SetParent(null)` (`0x1070A14`) und
`DontDestroyOnLoad(gameObject)` (`0x1070A40`).
[SaveManager.Awake](evidence/native-world-exit-audit-20261105/native-disassembly-pass4/0x06001823-SaveManager-Awake.txt),
[persistente Basis](evidence/native-world-exit-audit-20261105/native-disassembly-pass3/0x060030C9-PersistentSingleton_1-Awake.txt).
Das ist kein Beweis, dass jede konkrete Instanz überlebt; es widerlegt aber die
Freigabeannahme „Menüszene geladen, also Managerreferenzen sicher weg“.
`Singleton.OnDestroy` setzt nur bedingt seine eigene statische Instanz auf null
(`0x1135AE9` bis `0x1135B1B`), ohne eigene Save-Drainlogik.
[OnDestroy](evidence/native-world-exit-audit-20261105/native-disassembly-pass3/0x06003100-Singleton_1-OnDestroy.txt).

Tatsächliche direkte Menüaufrufer sind unter anderem
`ConfirmExitScreen.ConfirmExit` (`0xA680AD`) und `Tools.ExitToMenu.Exit`
(`0x7DFC58`), jeweils mit null Save/Popup und `preventLeaveLobby=false`.
[UI](evidence/native-world-exit-audit-20261105/native-disassembly-pass3/0x06006A54-ConfirmExitScreen-ConfirmExit.txt),
[Tool](evidence/native-world-exit-audit-20261105/native-disassembly-pass3/0x06003CFE-ExitToMenu-Exit.txt).
In diesen lokalen Rümpfen liegt keine SaveManager-Drainprüfung.
`ConfirmExitScreen.Update` formatiert die Zeit seit dem letzten Save, keine
Freigabequittung. Daraus wird **nicht** gefolgert, dass sämtliche UI-Eingaben
während eines Saves möglich seien oder alle Aufrufer untersucht wären.

Die Exit-Coroutine kann den erfassten `autoLoadSave` an `StartGame` weitergeben
(`0x5AE79F`, `0x5AE88A`). `StartGame` weist eine bereits geladene Welt **nur bei
`allowLoadStacking=false`** ab (`0x5882AD` bis `0x5882B6`). Genau false setzen
die beiden betrachteten Exit→StartGame-Aufrufer; über Nutzung oder Sicherheit
des True-Pfades wird nichts behauptet. Auf dem weiterführenden Pfad veröffentlicht
es neue `ActiveSaveInfo`
(`0x588385`) und `LoadedGameFolderPath` (`0x5883AA`) **vor** seiner Coroutine.
Diese ruft lokal PreSceneChange (`0x591153`), `LoadManager.CleanUp`
(`0x59116F`) und `LoadSceneAsync` (`0x591335`) auf.
[StartGame](evidence/native-world-exit-audit-20261105/native-disassembly-pass2/0x060017B5-LoadManager-StartGame.txt),
[Laderoutine](evidence/native-world-exit-audit-20261105/native-disassembly-pass2/0x060017E4-__StartGame_g__LoadRoutine_0_d-MoveNext.txt).
`LoadManager.CleanUp` ist eine getrennte globale Register-/Cachebereinigung,
nicht `SaveManager.Clean` und keine eigene Save-Drainquittung. Die beiden
Client-Ladewrapper rufen bei bereits geladener Welt ebenfalls ExitToMenu auf;
ihre weitergehende Netzwerklogik wurde nicht abgenommen.

### Dispose und Unity-Aufrufhülle liefern keinen Abbruchvertrag — WX-07

Der ausgewählte `SaveRoutine.System.IDisposable.Dispose`-Rumpf bei `0x43FEC0`
ist `C2 00 00` (`ret 0`) plus Padding, ohne eigene Abschluss-/Cleanupquittung.
[Dispose](evidence/native-world-exit-audit-20261105/native-disassembly-pass2/0x0600183B-__Save_g__SaveRoutine_0_d-System_IDisposable_Dispose.txt).
Das vielfach geteilte Ziel ist **kein vorgeschlagener Hook**. Es beweist nicht,
ob oder wann Unity es bei einem konkreten Abbruch aufruft.

`MonoBehaviour.StopAllCoroutines` bei `0x2B79B50` löst einen gecachten indirekten
Unity-Internalcall auf. Die dahinterliegende Engineimplementierung wurde nicht
untersucht.
[Aufrufhülle](evidence/native-world-exit-audit-20261105/native-disassembly-pass2/0x06000A53-MonoBehaviour-StopAllCoroutines.txt).
Die Rohsuche in `.text`/`il2cpp` prüfte 1.627.611 E8/E9-Bytes und fand für die
ausgewählten Ziele 23 Kandidaten; sechs sind in den neuen Fenstern als Transfers
bestätigt. Die übrigen bleiben Kandidaten. Weder fehlende direkte Clean-/Stop-
Treffer noch nächster Methodenpointer beweisen vollständige Aufrufer- oder
Abbruchabdeckung; Delegates, indirekte Aufrufe und Unity liegen außerhalb dieses
Negativschlusses.
[Rohsuche](evidence/native-world-exit-audit-20261105/world-exit-reference-candidates.json).

## Eigener Besitz und Bedingungen eines späteren Entwurfs — WX-08

Der getrennte
[eigene Lifecycle-Review](evidence/native-world-exit-audit-20261105/own-lifecycle-review.json)
belegt am unveränderten Quellstand:

- Heute verhindert die vorgeschaltete Sperre den asynchronen nativen Einstieg;
  es gibt keinen RuntimeSaveAdapter. Die reine Zustandsmaschine hat keinen
  Runtimebesitzer. Das ist keine neu gefundene aktive Save-Fehlfunktion.
- `Leaving` erhöht Generation, schließt Bereitschaft und setzt Fälligkeit zurück;
  `Stop` entfernt Abonnements. Weder Altmanagerübergabe noch Drainquittung.
- Der statische Port hält Wrapper/alte Closure, aber keine bewiesene native
  Objektfortlebensfähigkeit. Seine inaktive Root ist nicht ausdrücklich persistent.
- Eigene Overrides zählen nur Aufrufe; keine Operations-/Weltbindung oder echte
  Eintritt-/Austrittsbuchführung. Der heutige einmalige Tick ist keine Save-Pumpe.
- Synchrones Probe-finally und Hallen-Cleanup dürfen nicht als Rückbau künftig
  offener Savearbeit umgedeutet werden. Frühere bestandene Abnahmen bleiben zu.

**Anforderungen, keine Umsetzung:** Ein eigener äußerer Besitzer müsste schon
vor Registrierung eine unveränderliche Operationskennung, bestätigte Ladeepoche,
Zielpfade und konkrete alte Manager-/Listen-/Instanzbindung festhalten. Widerruf
schließt neue Mutation und Retargeting; unbekannte Wirkung bleibt terminal, ohne
Blindretry. Verwaltungsreichweite und geprüfte native Lebensdauer dürfen beim
Hallenabbau, Menü oder Ersetzen des aktuellen Kontextes nicht verloren gehen.

Eigene per-Instanz-Gates und Callbackbuchführung können ohne allgemeine GameHooks
entworfen werden, beweisen aber nur die tatsächlich umschlossenen eigenen Rümpfe,
nicht den umgebenden nativen Dispatch, spätere Wiederaufrufe oder alle Default-
Interfacepfade. Ohne deren nachgewiesenen Nicht-IO-/Nicht-Retarget-Vertrag ist
auch eine dauerhaft gehaltene, vermeintlich inerte Shell nicht freigegeben.

## Prüfungen und Archiv

Der unabhängige Standardbibliothekscheck gleicht Bytehashes, PE/RVA-Zuordnung,
jede exportierte Bytezeile, relative Sprung-/Callziele, `.pdata`-Vorhandensein/
Bounds, 14 fokussierte Feldoffsets, Rohsuffix und Sprungtabellendaten ab. Er ist
kein zweiter vollständiger x64-Decoder oder semantischer Beweis.

Der separate [Native-Gegenreview](evidence/native-world-exit-audit-20261105/native-world-exit-review.json)
vom `2026-11-05T17:04:16.1468933Z` liefert **7× agree, 1× limited**. Semantisch
17 Fenster vollständig beziehungsweise gezielt geprüft; alle 24 Fenster auf
Byte-/Textkonsistenz, 86 fokussierte Originalbyteanker und 35 relative Ziele
eigenständig abgeglichen. Kein zweites vollständiges Dekodieren aller Zeilen;
die 135 alten Belege bleiben hier dem Parent-Check zugeordnet.

Seine Pflichtpräzisierung zu WX-06 (`allowLoadStacking=false`) und die ergänzende
gemeinsame Client-Nachwartezeit sind oben übernommen. Originalbehauptungen und
Review bleiben unverändert, mit separatem
[Korrektur-/Dispositionseintrag](evidence/native-world-exit-audit-20261105/world-exit-claim-corrections.json).
Kein neuer unabhängiger Nachreview der korrigierten Dokumentation behauptet und
keine Freigabeentscheidung geändert.

Das dauerhafte [Archivmanifest](evidence/native-world-exit-audit-20261105/manifest.json)
identifiziert ausgewählte Text-/JSON-Belege und Analyseskripte. Keine Spiel- oder
Werkzeugbinärdateien, Metadaten, Saves oder privaten Konfigurationsinhalte kopiert.
Ursprüngliche Arbeitsorte in unveränderten Maschinenberichten sind Provenienz;
die Belegziele dieses Berichts liegen im dauerhaften Archiv.

Nicht ausgeführt: Builds, Testsuiten einschließlich isoliertem Pure, Installation,
Downloads, Spielstart/-steuerung, Bridge/Konsole, native Methoden-/Save-Aufrufe und
native Spielstanddateizugriffe. Kein neuer Save benötigt; TownHallTest /
SaveGame_2 wurde durch diesen Auftrag nicht gelesen oder verändert. Frühere
681 Tests/80 Auditfälle und der abgeschlossene Start-/Lade-/Handy-Basistest wurden
nicht wiederholt. Kein Commit oder Push.
