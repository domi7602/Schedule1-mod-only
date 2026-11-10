# TownHall: nativer Speicherabschluss und Referenzfreigabe

Stand und Prüfung: 2026-11-05. **Rein lesender Offline-Audit; keine Save-Freigabe.**
Die eigene Route bleibt vor Registrierung und Speicheraufruf gesperrt. Es wurde
kein Runtime-Adapter ergänzt. Der abgeschlossene Start-/Lade-/Handy-Basistest und
die früheren Modelltests bleiben getrennte, nicht erneut geöffnete Nachweise.

## Ergebnis

Der tatsächliche Maschinencode der passenden `GameAssembly.dll` ist jetzt gezielt
untersucht, nicht nur die IL2CPP-Aufrufhüllen. Er belegt mehrere Gründe, warum
Rückkehr, Idle, Completion, leere Queue und Folgeframe allein **weder erfolgreichen
Save noch sichere Freigabe der eigenen Business-Instanz** bestätigen:

- Der zulässige `Save()`-Pfad führt zu `Save(string)`, das eine Coroutine startet.
- Der Queue-Remove-Aufruf kommt vor dem `WriteBaseData`-Dispatch des Requests.
- Die Coroutine kann trotz verbleibender Queue in den Abschlusszweig wechseln.
  Dieser Zweig warnt und leert gegebenenfalls **CompletedSaveables**, nicht die Queue.
- `IsSaving=false` und `onSaveComplete` kommen vor der abschließenden `SaveError`-
  Auswertung. Auch nach Fehlerpfaden ist dieses Ereignis erreichbar.
- Bei queueleerem Abschluss wird `CompletedSaveables` in diesem Zweig nicht geleert.
  Darin können weiterhin Referenzen auf Teilnehmer liegen.

Das ist ein statischer Kontrollflussbefund dieser Binärfassung, keine beobachtete
Spielausführung und kein vollständiger Last-Use-/Destroy-Vertrag.

## Herkunft und Reproduzierbarkeit

Projekt `schedule1-mod-only`, Basisrevision
`b2c6c7e6c6ecc778da9624cdb68a6578b9db471a`, Arbeitsbaum **dirty**.
Die [Eingabeaufnahme][inputs] enthält Größen, Zeitpunkte und Hashes sowie 41 vor
dem Audit eingefrorene Produktions-/Testquelldateien. Ein Commit allein würde
diesen uncommitteten Stand nicht identifizieren.
Die [Dokumentfassungen][sourcesnapshot] und das [Archivmanifest][archive]
identifizieren die abschließende Dokumentation und die erhaltenen Textbelege.

| Original am Spielinstallationsort | SHA-256 |
|---|---|
| `GameAssembly.dll` | `61829050e56ca180d67be592e4bfe61ec97d2e9573f720d90a54e318a7e4381d` |
| `Schedule I_Data/il2cpp_data/Metadata/global-metadata.dat` | `e775c97da277da8eb1a90265478f3b65c9c3d6464aff4f7255d45280a1d1181f` |
| Cpp2IL-Dummy `cpp2il_out/Assembly-CSharp.dll` | `1aec8c918c39ff75f0e4d8e2c38bd11752c593119701d7e524a4ee82fdc30f21` |
| Interop-Proxy `MelonLoader/Il2CppAssemblies/Assembly-CSharp.dll` | `346205306273805a78b01a917f14b94518498af1361595c4c417baa0b32b4bd8` |

Die vorhandene Generator-Konfiguration enthält exakt den SHA-512 der aktuellen
GameAssembly. Zusätzlich wurden Binärdatei und Metadaten frisch mit der vorhandenen
`LibCpp2IL.dll` geparst: 149.349 Methoden, davon 70 gezielt zugeordnet. Deklarierte
Metadatenversion 31, Parserfassung 31.1, Unity 2022.3.62f2, x64-ImageBase
`0x180000000`. 50 Adresstokens der drei ergänzend dekompilierten Dummy-Typen
stimmen mit dem frischen Mapping überein. Leere Dummy-Rümpfe sind **nur** Adress-/
Signaturdaten, kein Implementierungsnachweis. [Methoden, Felder und Slots][mapping].

Das installierte `Cpp2IL.exe` wurde statisch mit vorhandenem `ilspycmd 8.2.0.7535`
entpackt. Nur seine Analysebibliotheken wurden geladen; Spiel-/Modassemblies
wurden weder geladen noch aufgerufen. `Iced.dll` dekodierte die als Bytes gelesenen
nativen Ausschnitte. Keine Downloads oder Installationen. Werkzeugidentitäten
stehen im Mapping; das verwendete [Analyseskript][exporter] ist erhalten.

### Fenstergrenzen und Symbolgrenzen

Die ersten V2-Ausgaben beschränkten sich auf den jeweiligen primären x64-`.pdata`-
Eintrag. Das ist nicht immer ein vollständiger Methodenkörper: Bei
`SaveRequest.Complete` endet dieser Eintrag schon bei RVA `0x5ABB31`, der
entscheidende Dispatch liegt aber erst bei `0x5ABB95`.

Die maßgebliche V4-Ausgabe reicht deshalb jeweils bis zum nächsten **unterschiedlichen
Managed-Methodenpointer**. Diese 23 gezielt ausgewählten Fenster enthalten auch
Padding und gegebenenfalls weitere Fragmente. Sie sind keine Behauptung exakter,
vollständiger Funktions-, Exception- oder transitiver Grenzen. Außerhalb liegende
Helfer und indirekte Ziele werden nicht allein durch ihre Namen als geprüft erklärt.
Geteilte direkte Ziele führen bis zu acht Aliasnamen plus Gesamtzahl auf; ein
Aliasname allein bestimmt nicht die konkrete generische Spezialisierung.

V2 hatte unaufgelöste generische Symbolhinweise; V3 konnte außerdem Byteflags
fälschlich als Metadaten annotieren. V4 verwendet passende Generic-MethodRef-
Auflösung und nur ausgerichtete Pointer-/LEA-Operanden für solche Hinweise.
**Null Auflösungsfehler bedeutet nicht vollständige oder unabhängig bewiesene
Symbolabdeckung.** Entscheidende Aussagen stützen sich zusätzlich auf Rohbytes,
Feldoffsets, konkrete Übergaben und Kontrollfluss. Historische Revieweingaben
bleiben unverändert neben V4 erhalten.

## Belegte native Reihenfolge

Alle Adressen unten sind RVAs relativ zur genannten ImageBase. Die verlinkten
Textfenster enthalten Rohbytes; die gleichnamigen JSONs enthalten zusätzlich
Bytehash, primäre `.pdata`-Grenzen und Instruktionsdaten.

### 1. Einstieg und Start der Coroutine

`Save()` (`0x06001828`, RVA `0x5AA800`) übergibt einen aus dem LoadManager-
Singleton gelesenen String und
springt bei `0x5AA87B` nach `Save(string)` (`0x06001829`, `0x5AA890`).
Das belegt den nativen Übergang, **nicht** die erfolgreiche Ausführung eines
installierten Harmony-Postfixes. [Parameterloser Einstieg][save0].

`Save(string)` besitzt Host-, Leerpfad-, Busy- und Tutorialabweisungen. Der
Tutorialzweig hat eine Editor-Ausnahme mit `DevSave`-Umleitung; das Leerpfadgate
ist kein sichtbarer `IsGameLoaded`-/Weltidentitätsnachweis. Sein zulässiger Pfad
setzt `IsSaving` bei `0x5AAB6C` true, ruft `onSaveStart` bei
`0x5AAB78`, leert danach `CompletedSaveables` und die Liste genehmigter
Basispfade, setzt das statische `SaveError` zurück und startet bei `0x5AAC4E`
die erfasste SaveRoutine. Rückkehr liefert keine korrelierte Abschlussquittung.
Welche Coroutine-Arbeit Unity vor der Rückkehr bereits ausführt, ist hier nicht
als Laufzeitreihenfolge bewiesen. [Save(string)][save1].

### 2. Queue, Snapshot und vorgezogene Entnahme

Die Coroutine `MoveNext` (`0x0600183C`, `0x5B0C20`) verarbeitet Requests nur,
solange `CompletedSaveables.Count < Saveables.Count` **und** die Queue nicht leer
ist (`0x5B1027–0x5B1062`). Bei `0x5B1081` bildet `GetRange(0, min(15, Count))`
eine eigene Requestliste. Ein Entfernen aus der Managerqueue beseitigt weder
diesen Snapshot noch den gerade laufenden Request.

Bei `0x5B10CD` wird `SaveRequest.Complete` aufgerufen. Dort ruft
`0x5ABB13` `QueuedSaveRequests.Remove(request)` auf; der boolesche Rückgabewert
wird hier nicht ausgewertet. Erst anschließend werden
Saveable, Pfad und SaveString geladen und bei `0x5ABB95` über ISaveable-Slot 13
(`WriteBaseData`) weitergereicht. **Queueleer kann innerhalb dieses Aufrufs vor
dem eigentlichen Schreib-/Nacharbeitsdispatch gelten.**
[Coroutine][routine], [volleres Requestfenster][request].

Nach dem Request-Aufruf beziehungsweise dem sichtbaren Fehlerlogpfad folgen in
`MoveNext` eine gegebenenfalls zusätzliche Queueentnahme (`0x5B1278`) und die
Contains-/Add-Buchführung für `CompletedSaveables` (`0x5B128A–0x5B130A`). Dieses
Zählen ist damit keine per-Teilnehmer-Schreiberfolgsquittung. Nach einem Batch
wird ein `WaitForEndOfFrame` ausgegeben (`0x5B131D–0x5B134E`). Daraus folgt kein
zusätzlicher allgemeiner Freigabevertrag für einen beliebigen späteren Frame.

### 3. Abschluss, verbleibende Queue und Fehlerauswertung

Der Abschlusszweig ruft zuerst `ClearBaseLevelOutdatedSaves` bei `0x5B1374` auf;
der transitive Rumpf dieses Helfers gehört nicht zum untersuchten Umfang.
Bei nichtleerer Queue folgen Warnung `0x5B13BA` und das Leeren der Liste an
Manageroffset **`+0x60` = CompletedSaveables** (`0x5B13CC–0x5B13F2`). Die Queue
liegt an **`+0x68`** und wird in diesem Warnzweig nicht geleert. Bei queueleerem
Abschluss wird der komplette Warn-/Clear-Block übersprungen.

Anschließend: `IsSaving=false` bei `0x5B1404`, Zeitrücksetzung, optional
`onSaveComplete.Invoke` bei **`0x5B1439`**, **danach** Test von `SaveError` bei
**`0x5B144C`** und Fehler-/Erfolgslog bei `0x5B1472` beziehungsweise `0x5B14A3`.
Das Ereignis wird nicht durch einen vorherigen erfolgreichen Fehlertest bedingt.
Der sichtbare Fehlerlogpfad setzt beispielsweise bei `0x5B11D4` SaveError und
führt anschließend in die gemeinsame Buchführung zurück. [Coroutine][routine].

**Ableitung, keine provozierte Spielbeobachtung:** Dieser Kontrollfluss lässt
Completion mit Fehlerstatus oder Restqueue zu. Die sichtbare Restqueue-Warnung
allein setzt in diesem Block SaveError nicht. Ein erfolgreicher Logtext wäre
ebenfalls kein Ersatz für einen vollständigen Daten-/Durabilitätsnachweis.

### 4. Default-Interface-Nacharbeiten

`ISaveable.WriteBaseData` (`0x06001785`, `0x583E70`) enthält einen
`File.WriteAllText`-Aufruf bei `0x584009` und sichtbare Fehlerlogzweige. Der
Nachlauf ab `0x584168` leitet sein boolesches Argument aus der Nichtleere des
SaveStrings ab, nicht aus einer bestätigten erfolgreichen Dateischreibung; bei
`0x5841D2` folgt ISaveable-Slot 15 (`CompleteSave`). Die vollständige
Exception-Zuordnung wird damit nicht als rekonstruiert behauptet. [WriteBaseData][writebase].

`CompleteSave` (`0x06001787`, `0x582F00`) benutzt weiterhin den Teilnehmer für
Extra-Dateien/-Ordner, `WriteData` (Slot 16), abhängig vom Pfad Datei-/Ordnerarbeit
und `DeleteUnapprovedFiles` (Slot 17). Erst bei `0x5837E4` ruft sein sichtbarer
normaler Nachlauf `SaveManager.CompleteSaveable(this)` auf. [CompleteSave][complete].
Ein false-Argument aus dem vorherigen Stringtest überspringt nur einen Teil der
Basissave-Dateibehandlung (`0x5831E0` → `0x58335A`), nicht pauschal `WriteData`,
bedingte Cleanup-Arbeit oder die Completed-Buchführung. Ein leerer SaveString
wäre daher für einen künftigen Adapter kein allgemeiner No-IO-Vertrag:
- `SaveManager.RegisterSaveable` (`0x0600182E`, RVA `0x5AA5C0`) fügt eine
  `Business`-Instanz nur in `Saveables` (`+0x48`) ein, nicht in `BaseSaveables`
  (`+0x50`, weil `Property.get_ShouldSaveUnderFolder` `0x060013FE` `true` liefert).
  Ohne Eintrag in `BaseSaveables` oder einem Treiber wie `Business.Businesses`
  ruft `SaveRoutine.MoveNext` (`0x0600183C`, RVA `0x5B0C20`) niemals `Save` auf
  dem Teilnehmer auf, wartet aber in der Schleife auf
  `CompletedSaveables.Count < Saveables.Count` (Deadlock).
- `SaveRequest..ctor` (`0x0600184A`, RVA `0x5ABBC0`) ruft `GetSaveString()` bei
  `0x5ABC5A` auf (mit Exception-Fallback auf `string.Empty` bei `0x5ABC77–0x5ABD5B`)
  und prüft `System.String::op_Inequality(this.SaveString, string.Empty)` bei
  `0x5ABD7B`: bei leerem `SaveString` reiht der Konstruktor den Request **nicht**
  in `QueuedSaveRequests` ein, sondern springt synchron per Tail-Jump bei `0x5ABDDC`
  direkt in `ISaveable.CompleteSave(parentFolderPath, false)` (Slot 15).
- `ISaveable.CompleteSave` (`0x06001787`, RVA `0x582F00`) ruft in `0x583010–0x583190`
  Slot 4 (`get_LocalExtraFiles`, bei `0x58301F`, `0x58303C`, `0x5830B0`, `0x583182`)
  auf und hängt `<entry> + ".json"` an die Liste `approved` (`0x582FF2–0x58300B`) an,
  ruft in `0x583195–0x5831D8` Slot 6 (`get_LocalExtraFolders`, bei `0x5831A4`,
  `0x5831BD`) auf und übergibt das Ergebnis an `List<string>.AddRange` bei `0x5831D8`
  (sind `_LocalExtraFiles_k__BackingField` oder `_LocalExtraFolders_k__BackingField`
  `null`, führt dies dort unmittelbar zur Exception), ruft in `0x58335A–0x5833D3`
  Slot 16 (`WriteData(parentFolderPath)` bei `0x5833BB`, `List<string>.AddRange` bei
  `0x5833D3`) auf und führt in `0x5833D8–0x583795` nach Prüfung von Slot 3
  (`get_ShouldSaveUnderFolder`, `0x5833E7`) den Aufruf von Slot 18
  (`GetContainerFolder`, `0x0600178A`, RVA `0x583840`, Aufruf bei `0x58340B`,
  inklusive `Directory.CreateDirectory`), `Directory.GetFiles` (`0x583418`),
  `Directory.GetDirectories` (`0x583425`), die Dateilöschschleife (`0x583440–0x5835C0`,
  `File.Delete` bei `0x5834C6`) und die Ordnerlöschschleife (`0x5835D0–0x583731`,
  `Directory.Delete(dir, true)` bei `0x583659`) aus, **bevor** Slot 17
  (`DeleteUnapprovedFiles`, `0x583778–0x583795`) und `SaveManager.CompleteSaveable(this)`
  (`0x5837E4`) erreicht werden.
Diese Default-Rümpfe beweisen nicht, dass die eigene injizierte Business-Klasse
in allen betreffenden Slots korrekt dorthin oder in ihre Overrides dispatcht.
Im Phase-1-Schritt-1-Baustein (`BusinessSaveableContractInventory` und
15-Slot-Bindung im `NativeSubtypePort`, 2026-11-06) sind alle 23 `ISaveable`-Slots
(`0x06001778`..`0x0600178E`, `Business`-Klassen-Vtable-Offset `23`) und diese
Gefahren als expliziter Modellvertrag erfasst.
Im Phase-1-Schritt-2-Baustein (2026-11-06) wurde `ShouldSaveUnderFolder` auf
`ProbeBusinessSubtype` überschrieben (liefert `false`) und als 16. virtueller Slot in
`NativeSubtypePort.RegisterType()` gebunden; damit wird der gefährliche Block
`0x5833D8–0x583795` (`GetContainerFolder`, Directory-/File-Deletes) in `CompleteSave`
garantiert übersprungen und synchron direkt nach `SaveManager.CompleteSaveable(this)`
gesprungen. Zudem implementiert `NativeSubtypePort` nun `IBusinessSavePort` mit
`ClearCompletedSaveable`-Bereinigung.

## Konkrete Referenzhalter und verbleibende Lücke

Die frischen Feldoffsets stammen aus demselben Binär-/Metadatenpaar, nicht nur aus
einer älteren Proxy-Datei. [Feld- und Slotmapping][mapping].

| Halter | Beleg und Bedeutung |
|---|---|
| `Saveables +0x48`, gegebenenfalls `BaseSaveables +0x50` | Manager hält Teilnehmer; die passende eigene Abmeldung muss beide tatsächlichen Mitgliedschaften berücksichtigen. |
| `QueuedSaveRequests +0x68` | Hält Requests; `SaveRequest.Saveable +0x10`, `ParentFolderPath +0x18`, `SaveString +0x20` halten deren Kontext. Der Konstruktor speichert diese Werte. |
| Eigene Batchliste und laufender Dispatch | `GetRange`-Snapshot und lokale Referenzen bestehen unabhängig von der Queueentnahme. |
| `CompletedSaveables +0x60` | `CompleteSaveable` fügt Referenzen hinzu. Bei queueleerem Abschluss kein Clear in diesem Zweig; der nächste zulässige Save leert die Liste am Start. `ClearCompletedSaveable` (`0x5A9A40`) bietet eine gesonderte einzelne Entfernung. |
| Coroutine-Capture, Events und fremde Teilnehmer | Capture hält Manager/Pfad; spätere oder reentrante Teilnehmer und Weltwechsel sind damit nicht als vollständig stillgelegt bewiesen. |

[Request-Konstruktor][requestctor], [CompleteSaveable][markcomplete],
[ClearCompletedSaveable][clearcomplete]. `Clean()` ist ebenfalls kein Ersatz für
eine gezielte Freigabequittung des eigenen Requests. Eine gespeicherte Referenz
beweist nicht automatisch einen späteren Dereferenzierungsfehler; ihre letzte
mögliche Nutzung muss jedoch vor einer Destroy-Freigabe gesondert geklärt sein.

**Noch nicht bewiesen:** ein zur konkreten eigenen Instanz, Manager-/Listenidentität,
Weltgeneration und Zielpfad gehörender Quieszenz-/Entfernungsvertrag über alle
relevanten Halter und aktiven Callbacks, einschließlich Fehlern, Reentranz und
Weltwechsel. Idle und parameterloses Event leisten diese Korrelation nicht.
Ein Root, der nur bis zum nächsten Frame oder Prozessende gehalten wird, beweist
insbesondere keine sichere Weiterverwendung durch einen persistenten Manager
in einer späteren Welt.

## Managed-Grenze und getrennte Beweisziele

Der unabhängige [Managed-Review][managed] prüfte aktuelle S1API-3.2.2-Originalquellen
und vier statisch dekompilierte Typen der installierten DLL. Der direkte
GameLifecycle-Relay ist kein Postfix. Der gesonderte GenericSaveables-Postfix
schreibt nach Rückkehr von `Save(string)` und isoliert weder jeden Teilnehmer noch
alle Teilfehler. `OnSaved` und `RequestGameSave` liefern keine globale Quittung.
Main-Unload-/Reset kann Listener entfernen, ohne einen nativen Drain zu bestätigen.
Tatsächliche fremde Patch-/Listenerreihenfolgen wurden nicht inventarisiert.

Der neue native Übergang `Save()` → `Save(string)` schließt die entsprechende
**statische Callchain-Frage** des früheren Reviews. Er beweist keinen tatsächlichen
Postfixaufruf und legt ohne Unity-/Harmony-Laufzeitnachweis nicht pauschal dessen
Reihenfolge zu Coroutineabschluss und Fremdteilnehmern fest. Weil `IsSaving` vor
dem Completion-Event false wird, muss auch reentrante Arbeit aus Teilnehmern
ausdrücklich vom künftigen eigenen Vertrag erfasst werden.

Ein solcher neuer Save kann Completed und den gemeinsamen SaveError zurücksetzen,
bevor spätere Teilnehmer des alten Events oder die alte Routine fortfahren.
Auch das Start-Event liegt vor dem Leeren alter Buchführung und vor SaveError=0;
ein dort gesetzter Fehlerstatus oder genehmigter Pfad kann im normalen weiteren
Startpfad überschrieben werden. Diese konkreten Reihenfolgengefahren sind
Ableitungen, keine beobachteten Listener-/Fehlersitzungen.

Drei Ziele sind getrennt zu behandeln:

1. **Eigene Referenzfreigabe:** letzte relevante Nutzung und gezielte Entfernung
   beweisen; derzeit offen.
2. **Nativer Speichererfolg:** Fehler, tatsächliche Daten und Ziel zuordnen;
   durch Ereignis/Buchführung nicht bewiesen.
3. **Konsistenz zusätzlicher Moddaten:** gesonderte Teilnehmer-/Dateiverträge;
   keine gemeinsame atomare oder dauerhafte Transaktion aus dem Postfix ableiten.

Eine begrenzte, später bewiesene eigene Lifetimegrenze müsste nicht automatisch
Erfolg aller fremden Moddateien garantieren. Umgekehrt darf fehlender globaler
Schreiberfolg nicht durch eine behauptete sichere Destroy-Grenze verdeckt werden.

## Unabhängige Gegenprüfung

Der separat beauftragte Prüfer bestätigte im [V2-Native-Review][reviewv2] alle
2.978 Instruktionsbytezeilen der 23 primären Fenster und berechnete 831 relative
Sprung-/Callziele unabhängig nach. 52 Revieweingaben blieben während seiner
Prüfung unverändert. Fünf Aussagen sind `agree`, drei `limited`; die drei
`reject` verwerfen gerade vollständige Methoden-/Transitiv-/Freigabeclaims,
nicht die oben belegten lokalen Reihenfolgen. Insbesondere blieb der verkürzte
V2-Complete-Rumpf unvollständig. Dieser historische Bericht wird durch die
V4-Erweiterung nicht nachträglich zu einer V4-Prüfung umgedeutet.

Der gesonderte [V4-Nachtrag][reviewv4] um 09:03:02 UTC prüft genau die drei neuen
Request-/WriteBaseData-/CompleteSave-Fenster: 874 Instruktionsbytezeilen, 217
relative Transfers und zwölf stabile Eingaben; sechs Aussagen `agree`, zwei
`limited`, kein neuer Einwand gegen deren begrenzten Umfang. Default-Dispatch,
Fenster-/EH-Grenzen und Symbolvollständigkeit bleiben ausdrücklich eingeschränkt.
Beide alten Reviews sind unverändert; **keine Runtime-/Write-/Unregister-/Destroyfreigabe**.

## Tatsächlich ausgeführte Prüfungen und nächster Schritt

- Frisches Mapping, 23 native Fenster, 3.285 Instruktionen / 13.376 Byte einschließlich
  Padding. Alle Rohbytes, zusammenhängenden RVAs, Fensterhashes und primären
  `.pdata`-Einträge unabhängig vom PowerShell-Export gegen die PE-Datei abgeglichen.
  863 relative direkte Kontrolltransfers zusätzlich aus ihren Bytes berechnet.
- Zehn fokussierte Feldoffsets und zwei entscheidende Interfaceslots gegen das
  frische Mapping kontrolliert; V4 ohne Metadaten-Auflösungsfehler. Das ist keine
  zweite vollständige x64-Dekodierung oder unabhängige Metadatenimplementierung.
- Acht eingefrorene Installations-/Analyseeingaben und 41 Produktions-/Testquellen
  unverändert; installierte S1API separat hashgleich nachgeprüft.
  [Integritätsprüfung][integrity], [lesendes Prüfsystem][validator].
- Keine Produktions-/Teständerung, Builds, Testsuiten, Installation, Spielstart,
  Spielsteuerung, Bridge-/Konsolenbefehle, Save-Aufrufe oder nativen Save-Dateizugriffe.
  Kein Commit oder Push. Frühere 681 Tests, drei Builds und 80 Auditfälle nicht wiederholt.

Dokumentationspfade, UTF-8-/Whitespaceprüfung der betroffenen Dokumente und
`git diff --check` bestanden. Vault-Strukturprüfung mit Projektpfaden: 46
Markdown-Dateien, 0 Fehler, 0 Warnungen. Die isolierte Pure-/Build-/Testsuite
wurde auftragsgemäß nicht erneut ausgeführt; frühere fehlgeschlagene Gesamtgates
bleiben ihrem datierten Snapshot zugeordnet, keine neue Repository-Gesamtfreigabe.

Als nächste Entwicklungsentscheidung ist der fehlende **eigene** Lifetime-/
Weltwechselvertrag gezielt zu schließen oder die Probenarchitektur entsprechend
zu ändern. Kein bloßes Anschließen des Completion-Events, kein zusätzliches
Warteframe als Ersatz und kein neuer globaler Detour auf geteilte Spielmethoden.
Bis dahin bleibt `NativeSaveWriteAllowed=false`; ein Live-Save würde außerdem
einen gesonderten Auftrag und frischen passenden Checkpoint benötigen.

Der frühere Satz „native Rümpfe fehlen in den Proxys“ bleibt für diese Proxys
richtig und als historischer Erkenntnisstand erhalten. Die oben belegten nativen
Teilfolgen sind neu; die verbleibende Freigabelücke ist nicht mehr bloß mit
fehlenden C#-Rümpfen zu begründen. [Speicherprobe](business-manager-save-probe.md),
[Fertigstellungsplan](business-completion-plan.md).

[inputs]: evidence/native-save-audit-20261105/input-manifest.json
[sourcesnapshot]: evidence/native-save-audit-20261105/source-documents.json
[archive]: evidence/native-save-audit-20261105/manifest.json
[mapping]: evidence/native-save-audit-20261105/native-map-v4.json
[exporter]: evidence/native-save-audit-20261105/map-native.ps1
[integrity]: evidence/native-save-audit-20261105/native-integrity-final-20261105.json
[validator]: evidence/native-save-audit-20261105/validate-native-evidence.py
[managed]: evidence/native-save-audit-20261105/managed-boundary-review.json
[reviewv2]: evidence/native-save-audit-20261105/native-claims-review.json
[reviewv4]: evidence/native-save-audit-20261105/native-v4-review.json
[save0]: evidence/native-save-audit-20261105/native-disassembly-v4/0x06001828-Save.txt
[save1]: evidence/native-save-audit-20261105/native-disassembly-v4/0x06001829-Save.txt
[routine]: evidence/native-save-audit-20261105/native-disassembly-v4/0x0600183C-MoveNext.txt
[request]: evidence/native-save-audit-20261105/native-disassembly-v4/0x0600184B-Complete.txt
[requestctor]: evidence/native-save-audit-20261105/native-disassembly-v4/0x0600184A-_ctor.txt
[writebase]: evidence/native-save-audit-20261105/native-disassembly-v4/0x06001785-WriteBaseData.txt
[complete]: evidence/native-save-audit-20261105/native-disassembly-v4/0x06001787-CompleteSave.txt
[markcomplete]: evidence/native-save-audit-20261105/native-disassembly-v4/0x0600182B-CompleteSaveable.txt
[clearcomplete]: evidence/native-save-audit-20261105/native-disassembly-v4/0x0600182C-ClearCompletedSaveable.txt
