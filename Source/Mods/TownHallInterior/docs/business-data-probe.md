# TownHall: eigene Business-Daten und native Abfragen

Stand: 2026-11-06. Nach dem beobachteten
[Business-/NetworkObject-Bindungsbaustein](business-network-subtype-probe.md)
offline implementiert und in Slot 2 getestet. Der erste Datentest scheiterte am
Listener-Snapshot. Der zweite Lauf zeigte 889 Einträge bei Kapazität 1024; der
alte 512er-Guard war zu niedrig, der Getter las korrekt. Der kapazitätsgebundene
Nachfolger bestand den dritten Slot-2-Lauf mit `observed`, keinen Findings und
den erwarteten nativen Abfragen 0/0/true/false. Normalbuild wiederhergestellt;
alle 39 nativen Testdateien blieben unverändert. Keine vollständige
Business-Initialisierung.

## Problem und Ziel

Die eigene no-base Business-/NetworkObject-Bindung wurde im Testslot beobachtet.
Sie stellt keine nativen Business-Konstruktorzustände her. Native Abfragen benötigen
eigene gültige Collections, Referenzen und räumliche Daten. Die originalen Awake-
und Start-Methoden publizieren Instanzen beziehungsweise abonnieren globale
Ereignisse; sie sind keine isolierten Feldinitialisierer.

Die nächste optionale Probe bereitet ausschließlich eigene native Business-/
Property-Daten vor. Sie prüft den tatsächlichen Containeranschluss und native
Abfragen über den S1API-Wrapper. Diagnosewerte wie Preis/Kapazität null sind keine
Balancingentscheidung für ein später kaufbares Rathaus.

## Vertrag und Zuständigkeiten

- Neuer eigenständiger Buildmodus `S1TownHallBusinessData` auf dem bestehenden
  eigenen Business-/NetworkObject-Lebenszyklus; bisherige Modi bleiben erhalten.
  Widersprüchliche Kombinationsmodi müssen beim Build scheitern.
- Ein Versuch pro Prozess in vollständig geladenem Singleplayer TownHallTest /
  SaveGame_2. Gebundene Weltgeneration, native Identitäten, Abhängigkeiten und
  Besitzerbeziehungen gelten unmittelbar vor jedem nativen Zugriff/Eingriff.
- Eigene leere Business-/Property-Collections und erforderliche Loaderzustände;
  eigene Container-/Bounds-/Hilfsobjekte nur im belegten Umfang. Kein Aufruf eines
  nativen Component-Konstruktors auf einer bereits injizierten Komponente.
- Tatsächliches `PropertyContentsContainer.SetProperty` ausführen, beide
  Referenzrichtungen bestätigen und denselben nativen Business-Zeiger wrappen.
- Native `CurrentLaunderTotal` und `AppliedLaunderLimit` müssen für leere Daten
  und Kapazität null endlich und null sein. Tatsächliche Bounds-Abfrage für einen
  klar inneren und äußeren Punkt muss true beziehungsweise false liefern.
  Die Bounds-Methode verwendet lokale Transformation, center und size; der eigene
  BoxCollider kann deaktiviert bleiben. Ein eigener LINQ-Selectorcache kann beim
  ersten nativen Getterzugriff initialisiert werden; keine völlige Abwesenheit
  globaler Laufzeiteffekte behaupten.
- Gemeinsame Register/Halter, fremde VTables sowie relevante MoneyManager-/
  TimeManager-Ereignisse müssen in diesem begrenzten Ablauf unverändert bleiben.
  Lesefehler oder unbekannte Identität sind keine bestätigte Unverändertheit.
- Die bekannte eigene Lifecycle-Kette bleibt erforderlich. Eigene Ressourcen
  samt Kindobjekten/Komponenten müssen vor Cleanup genau identifiziert und danach
  nativ als entfernt bestätigt werden. Typregistrierung bleibt prozessweit bestehen.

Root führt Vertrag, Integration, Build-/Quellnachweise und Dokumentation;
separate Agenten implementieren Produktionscode, schreiben Tests und prüfen
native Anschlussstellen. Dateizuständigkeiten sind getrennt. Produktionsbudget
bleibt bei 20 handgepflegten Produktions-/Projektdateien.

## Übergänge und unbekannte Wirkung

Gültigen Kontext und Fremdzustand erfassen → eigene inaktive Daten/Objekte
vorbereiten → Referenzen und native Abfrageziele prüfen → tatsächliche Abfragen
mit Ergebnisreadback → eigenen Lifecycle beobachten → nur bestätigte eigene
Ressourcen zurückbauen → Entfernung und Fremdzustand bestätigen.

Ein begonnener fehlbarer nativer Schritt kann Teilwirkung haben. Kontextwiderruf,
Identitätswechsel, fremde Referenzen oder unbestätigte Wirkung verbrauchen den
Versuch und sperren weitere Nutzung beziehungsweise spekulativen Rückbau.
Diagnostik behält bekannte Beobachtungen, ohne nach Widerruf erneut nativ zu lesen.
Keine fremden Listen oder Listener auf einen alten Snapshot zurücksetzen.

## Prüfkriterien und Grenzen

Vor Änderungen bestanden 1.077/1.077 TownHall-Tests. Neue unabhängige Tests prüfen
den tatsächlichen Produktionsablauf mit künstlichem Port einschließlich partieller
Vorbereitung, ungültiger Ergebnisse, Referenzwechsel, Reentranz, Kontextverlust und
unbestätigtem Cleanup prüfen. Sie ersetzen keine native Laufzeitbeobachtung.
No-Deploy-Builds bestätigen konkrete API-Verwendung; nativer Quellenreview bestätigt
nur den belegten statischen Umfang. Endgültige API-/Feldmenge folgt den aktuellen
Assembly-/Native-Belegen, nicht einer älteren Wrapperbeschreibung.

Keine Originalinitialisierung, RPCs, Spawn, Weltpublikation, SetOwned, Load,
Serialisierung, Save, Kauf oder laufende Geldwäsche. Kein automatischer Spielstart
oder Deployment in dieser Entwicklung. Originale Initialisierung, frühe
Weltregistrierung und Persistenz bleiben separate offene Integrationsschritte.

## Konkrete Implementierung und vorherige Offline-Nachweise

Der Adapter hält elf eigene Listen, vier Arrays, getrennte Business-/Property-Loader
und zwei inaktive Kinder mit PropertyContentsContainer beziehungsweise deaktiviertem
BoxCollider. Das Bounds-Array enthält genau diesen Collider; übrige Collections
sind leer. `SetProperty` wird tatsächlich aufgerufen und in beide Richtungen gelesen.
Sowohl `BusinessWrapper.InnerBusiness` als auch `PropertyWrapper.InnerProperty`
müssen auf dieselbe eigene Instanz zeigen. Die vier nativen Abfragen sind
`CurrentLaunderTotal`, `AppliedLaunderLimit` und `IsPointInside` für innen und außen.
Native Methodentoken/-signaturen und die tatsächlich auditierten Binärdateien werden
gebunden; die vier Listenergruppen erfassen auch Inhalte mutabler ActionLists und
Delegate-Ziele. Jeder Abfrage geht eine frische Besitz-/Kontext-/Fremdzustandsprüfung
voraus. Bekannte skalare Rückgaben bleiben auch bei unbestätigter Nachprüfung erhalten.

- 89 neue Fälle und **1.166/1.166 TownHall-Tests bestanden**;
  zuvor 1.077/1.077 im unveränderten Ausgangsstand.
- Sieben Release-No-Deploy-Varianten bestanden mit jeweils 0 Warnungen/Fehlern:
  Normal, Subtype, Interface, Manager, ActiveSubtype, NetworkSubtype und Data.
  Neun gesperrte Kombinationen/Routen liefern erwartungsgemäß CS1029.
- Unabhängiger Quellenreview abgeschlossen; **keine native Data-Laufzeitbeobachtung**.
  Produktionsbudget weiterhin 20 Dateien. Keine Installation, Save-Dateizugriffe,
  Spielstarts, Commits oder Pushes. Vorbereiteter DLL-SHA-256: `2e519ccfdc58053281dc67ffd91e2c1b53d523aa2941657e7b8e1fcd7dd81357`.
- [Quell-/Build-/Testnachweis](evidence/business-data-20261106/verification.json),
  [nativer Vertrag](evidence/business-data-20261106/audit/native-data-contract.json),
  [abschließender Review](evidence/business-data-20261106/audit/final-code-review.json).
  Der isolierte Gesamtprüfstatus wird separat im gleichen Belegverzeichnis geführt;
  dieser fokussierte Nachweis behauptet keinen grünen Repository-Gesamtstatus.

Der native Audit bindet aktuelle Binärhashes und begrenzte Dekodierfenster; er ist
kein vollständiger transitiver Kontrollfluss-/Exception-/Lebenszeitnachweis.
Bestätigtes Verschwinden eigener Unity-Objekte ist getrennt von der späteren
GC-Sammlung ihrer IL2CPP-Datenobjekte. Keine sofortige Heapfreigabe behaupten.

## Installation und gescheiterter Slot-2-Datentest

Auf Dominiks Auftrag „installieren und Slot 2 testen“ wurde der unveränderte
Kandidat `2e519ccf…` um 14:13:53 CET mit dem offiziellen Helper installiert.
Frischer Checkpoint: 39 native Testdateien, 178 UserData-Dateien und drei
Vorgängerartefakte. Nur TownHall-DLL/PDB ersetzt, Metadaten unverändert.
[Installationsprüfung](evidence/business-data-deployment-20261106/deployment-verification.json),
[Quittung](evidence/business-data-deployment-20261106/deployment-receipt.json).

Start über Steam, ausdrückliche Auswahl TownHallTest / Slot 2 per CUA;
Prozess 15940 ab 14:16:31 CET, Slot-2-Mapping im Log um 14:17:51 CET.
Die geladene Halle war sichtbar. Bericht `20261106T131816973Z.json` um
14:18:16 CET bindet denselben DLL-Hash und vollständig gültigen Ladekontext.
Er endet mit **`quarantined / data-minute-list-over-budget`**.

`BusinessConstruction.cs` ruft im Anfangspfad `Capture()` vor `RegisterType()`
und `Prepare()` auf. Darin scheitert `CaptureDataListeners()` an
`count < 0 || count > 512`, bevor der Snapshot fertiggestellt wird.
Der tatsächliche Count wird nicht protokolliert: nur außerhalb 0..512 ist
ableitbar, keine genaue Anzahl und keine Unterscheidung negativ/zu groß.
Eigene Zeiger bleiben null; Registrierung, Vorbereitung, `SetProperty`,
Abfragen, Aktivierung und Cleanup wurden nicht erreicht. Acht Einträge in
`DataNativeMethods` belegen Metadatenprüfungen, keine nativen Getteraufrufe.
Beide Snapshots fehlen: keine bestätigte Unverändertheit fremder Zustände und
kein erfolgreicher Cleanup-Nachweis aus diesem Lauf.
[Originalbericht](evidence/business-data-live-20261106/live-report.json),
[Auswertung mit Quellhash](evidence/business-data-live-20261106/observation.json),
[unabhängiger Review](evidence/business-data-live-20261106/live-review.json),
[Logauszug mit Originalzeilen](evidence/business-data-live-20261106/live-log-excerpt.txt).

Das Spiel lief nach dem Abbruch weiter und wurde regulär per Alt+F4 geschlossen;
kein erzwungenes Prozessende. Nach frischer Sicherung waren **39/39 native
Slot-2-Dateien bytegleich**, keine hinzugefügt/entfernt. Leere Verzeichnisse
waren nicht inventarisiert; daraus folgt kein universeller Nachweis fehlender
Dateisystemoperationen. Nativer Slot 1 wurde nicht geöffnet.
Um **14:22:17 CET** Normalbuild `614b81fd…` / PDB `8126ae65…` wiederhergestellt.
Helperquittung, direkte Hashes und unabhängiger Review stimmen überein;
nur TownHall-DLL/PDB geändert, alle 37 anderen Moddateien unverändert.
Spiel geschlossen, Cloud-Einstellungen unverändert; Cloud-Konflikte nicht aufgelöst.
[Savevergleich](evidence/business-data-live-20261106/save-verification.json),
[Rollbackprüfung](evidence/business-data-live-20261106/rollback-verification.json),
[Rollbackreview](evidence/business-data-live-20261106/rollback-review.json),
[Hashmanifest](evidence/business-data-live-20261106/manifest.json).

Der darauffolgende Entwicklungsschritt war: den abgewiesenen Count diagnostisch
bewahren und die Listener-Erfassung begründet begrenzen. Keine blinde Erhöhung
oder Entfernung der Schutzprüfung. Danach dieselben nativen Daten-/Abfragekriterien
erneut prüfen. Dieser Installations-/Liveabschluss änderte keinen Produktionscode;
keine neuen Builds, Unit-Tests, Commits oder Pushes. Die vorherigen Offline-Nachweise
bleiben historisch erhalten; native Business-Daten und Originalinitialisierung sind offen.

## Listener-Korrektur (offline, nicht installiert)

Der alte Guard prüfte `time.onMinutePass.list.Count` über den generischen
IL2CPP-Getter der nativen `ActionList`. Dessen Rückgabe lag außerhalb 0..512;
die exakte alte Zahl wurde nicht protokolliert. Die dekompilierte `ActionList`
und die Feldmetadaten belegen `_items`, `_size` und `_version` als direkt lesbare
Felder. Ein fehlerhafter generischer Getter ist damit als plausible Ursache
eingegrenzt, nicht als einzige Möglichkeit bewiesen.

Die Korrektur in `CaptureDataListeners()` liest für Fingerprint, Schranke und
Kapazitätsprüfung jetzt `_size`, `_version` und `_items` direkt. Der Getter
`Count` wird weiterhin in einem isolierten `try`/`catch` ausgeführt und nur
diagnostisch erfasst. Die 512er-Schranke bleibt bestehen und nennt den
tatsächlichen Wert (`data-minute-list-over-budget:<size>`); eine unlesbare
Item-Referenz und eine ungültige Kapazität haben eigene Fehler. Der Bericht
enthält den neuen Eintrag `Native.DataListenerDiagnostics["minute-list"]` mit
Listenzeiger, Klasse, `size`, `capacity`, Itemzeiger, `version`,
`countProperty` (oder `unreadable:…`) und `countPropertyMatchesSize`. Er bleibt
auch bei einem Abbruch im Listener-Snapshot erhalten. Die Data-Mode-Grenze
beschreibt die Getter-Abweichung ausdrücklich.

- Änderung: `BusinessConstruction.cs`, SHA-256
  `d54ab55edfa7dd369e0711855885288a019e8e0b41f1581afb75fe75c520c979`.
- Neuer Kandidat: `4ae6509a2d8501176ee8b9dca7e3c8b6481382d0f6f30fa73857506c9cf304f6`,
  PDB `47592f81d0471844256ae91cd521a715f76a96c9fe27f7ffdc2bfa5567bc0a3d`;
  Ablage `.backups/townhall-business-data-prepared/20261106T141106231Z/`.
- Offline: Data-Build mit `-warnaserror` 0 Warnungen/0 Fehler;
  `dotnet format whitespace` formatiert 0 von 21 Dateien; **1.166/1.166
  TownHall-Tests**; sieben No-Deploy-Modi bestanden, neun Kombinationen/Routen
  weiterhin mit CS1029 gesperrt.
- Der alte Kandidat `2e519ccf…` und der gescheiterte Lauf bleiben historisch
  erhalten; die Korrektur hat ein eigenes Belegverzeichnis.
- [Offline-Nachweis](evidence/business-data-listener-fix-20261106/verification.json),
  [Buildmatrix](evidence/business-data-listener-fix-20261106/build-results.json),
  [Tests](evidence/business-data-listener-fix-20261106/townhall-tests.log.txt).

Zum Zeitpunkt der Offline-Prüfung nicht ausgeführt: Installation, Spielstart,
Slot-/Save-Zugriff, native Laufzeitbeobachtung, Commits oder Pushes. Damals blieb
der Normalbuild `614b81fd…` / PDB `8126ae65…` installiert; das Spiel war
geschlossen. Die zwischenzeitliche Installation ist im folgenden Abschnitt
dokumentiert. Ein erfolgreicher erneuter Datentest muss den Getterwert und die
Feldwerte im neuen Bericht zeigen.

## Installation des korrigierten Kandidaten

Auf ausdrückliche Freigabe („Ja, installieren“) wurde der Kandidat `4ae6509a…` /
PDB `47592f81…` am 6. November um **15:23:34 CET (14:23:34 UTC)** über den
Projekt-Helper installiert. Zwei Dateien wurden ersetzt (DLL/PDB), `mod.json`
blieb bytegleich; die Quittung meldet `Copied 2, Unchanged 1`. Der Helper sicherte
die vorherige normale DLL `614b81fd…` / PDB `8126ae65…` in seinem Backup.
Gegenüber dem Nachtest-Checkpoint `20261106T132157195259Z-af2660f4` blieben
**39/39 native Slot-2-Dateien bytegleich**; von 181 UserData-Dateien (ohne
`.deploy`) änderte sich nur die PDB. 38 Moddateien blieben gleich, nur die
TownHall-DLL wurde ersetzt. Spiel geschlossen, kein Spielstart, keine
Save-Zugriffe, keine neuen Builds/Tests. Die Prüfung ist ein skriptgestützter
Datei-/Hashabgleich, kein unabhängiger Review.
[Installationsbeleg](evidence/business-data-listener-fix-deployment-20261106/deployment-verification.json),
[Quittung](evidence/business-data-listener-fix-deployment-20261106/deployment-receipt.json),
[Baseline](evidence/business-data-listener-fix-deployment-20261106/baseline-checkpoint.json).

## Live-Test der Listener-Diagnose

Der korrigierte Kandidat wurde am 6. November um 15:28:10 CET über Steam geladen;
TownHallTest / SaveGame_2 war vollständig geladen (`LoadComplete`, `HallReady`,
`DependenciesMatch`). Der Bericht `20261106T142853400Z.json` um 15:28:53 CET
bindet DLL `4ae6509a…` und endet mit
`quarantined / data-minute-list-over-budget:889`. Die neue Diagnose zeigt
`size=889`, `capacity=1024`, `version=891`, `countProperty=889`,
`countPropertyMatchesSize=True`. Der generische Getter las also korrekt; die
Liste ist tatsächlich größer als 512. Eigene Registrierung, Datenaufbau,
`SetProperty`, Abfragen und Cleanup wurden nicht erreicht.
Nach regulärem Alt+F4 waren 39/39 native Slot-2-Dateien bytegleich. Um
**15:31:02 CET** wurde der Normalbuild `614b81fd…` / PDB `8126ae65…` über den
Helper wiederhergestellt; nur DLL/PDB ersetzt, `mod.json` bytegleich.
[Bericht](evidence/business-data-listener-count-live-20261106/live-report.json),
[Auswertung](evidence/business-data-listener-count-live-20261106/observation.json),
[Savevergleich](evidence/business-data-listener-count-live-20261106/save-verification.json),
[Rollback](evidence/business-data-listener-count-live-20261106/rollback-verification.json).

## Kapazitätsgebundene Korrektur (offline geprüft, installiert)

Der feste 512er-Grenzwert war die falsche Schranke. Die Liste hat 889 von 1024
Plätzen belegt; ihre Kapazität ist der natürliche, belegte Rahmen. Die Korrektur
entfernt den willkürlichen Eintragsgrenzwert und prüft stattdessen: `items`
lesbar, `size >= 0`, `size <= capacity`, `capacity <= 4096`. Damit bleibt eine
harte Obergrenze bestehen; die tatsächliche Größe wird im Bericht genannt. Der
Graph-Lauf ist auf 16.384 Knoten begrenzt; `nodes` und `maxChildren` werden in
`Native.DataListenerDiagnostics["minute-list"]` festgehalten, auch wenn der Lauf
abbricht. Die 512er-Grenze für einzelne Multicast-Arrays bleibt.

- Änderung: `BusinessConstruction.cs`, SHA-256
  `266cbe3fa9a275cc3f254998858c53d3e02a6091900a0bc74171bae035e96c24`.
- Neuer Kandidat: `b449c1c18062edff3729483b020eca578c7483ad958ef3d7c3ad72a57f9cce01`,
  PDB `943b9176dadc0e72690a0b2861b442649fc8b0e099b4a6ea6b7a6beec142078a`;
  Ablage `.backups/townhall-business-data-prepared/20261106T143520573Z/`.
- Offline: Data-Build mit `-warnaserror` 0 Warnungen/0 Fehler;
  `dotnet format whitespace` formatiert 0 von 21 Dateien; **1.166/1.166
  TownHall-Tests**; sieben No-Deploy-Modi bestanden, neun Kombinationen/Routen
  weiterhin mit CS1029 gesperrt.
- Nach Freigabe am 6. November um **15:43:53 CET (14:43:53 UTC)** über den
  Projekt-Helper installiert; nur DLL/PDB ersetzt, `mod.json` bytegleich,
  39/39 native Slot-2-Dateien unverändert. Kein Spielstart, keine Save-Zugriffe,
  keine Commits/Pushes.
- [Offline-Nachweis](evidence/business-data-listener-capacity-20261106/verification.json),
  [Buildmatrix](evidence/business-data-listener-capacity-20261106/build-results.json),
  [Tests](evidence/business-data-listener-capacity-20261106/townhall-tests.log.txt),
  [Installationsbeleg](evidence/business-data-listener-capacity-deployment-20261106/deployment-verification.json).

Der Lauf ist im folgenden Abschnitt dokumentiert; die Abnahmekriterien sind
danach zusammengefasst.

## Live-Test der kapazitätsgebundenen Probe (bestanden)

Der Ladevorgang begann am 6. November um 15:48:13 CET; TownHallTest / SaveGame_2
war vollständig geladen. Der Bericht `20261106T144912409Z.json` um 15:49:12 CET
endet mit **`observed`, keine Findings**. Der Listener-Snapshot zeigt
`size=889`, `capacity=1024`, `nodes=889`, `maxChildren=0`. Datenvorbereitung und
`SetProperty` sind bestätigt; `CurrentLaunderTotal=0`, `AppliedLaunderLimit=0`,
`Inside=true`, `Outside=false`. Cleanup bestätigt, Root zerstört, eigene Handles
freigegeben. Vorher-/Nachher-Snapshots identisch: 32 Register, 19 fremde VTables.
Nach regulärem Alt+F4 waren 39/39 native Slot-2-Dateien bytegleich. Um
**15:50:44 CET** wurde der Normalbuild `614b81fd…` / PDB `8126ae65…` über den
Helper wiederhergestellt; nur DLL/PDB ersetzt, `mod.json` bytegleich.
[Bericht](evidence/business-data-capacity-live-20261106/live-report.json),
[Auswertung](evidence/business-data-capacity-live-20261106/observation.json),
[Savevergleich](evidence/business-data-capacity-live-20261106/save-verification.json),
[Rollback](evidence/business-data-capacity-live-20261106/rollback-verification.json).

## Abnahmekriterien der Datenprobe

Die folgenden Kriterien wurden mit dem kapazitätsgebundenen Kandidaten
`b449c1c1…` im Lauf vom 6. November 2026 geprüft und erfüllt: Listener-Snapshot
mit `size=889`, `capacity=1024`, `nodes=889`, `maxChildren=0`; vollständige
Datenidentitäten; `SetProperty`-Rückverweise; native Ergebnisse 0/0/true/false;
bestätigter Cleanup; unveränderte beobachtete Fremdzustände; 39/39 native
Testdateien nach dem Schließen bytegleich; Normalbuild wiederhergestellt.
[Bericht](evidence/business-data-capacity-live-20261106/live-report.json),
[Auswertung](evidence/business-data-capacity-live-20261106/observation.json),
[Savevergleich](evidence/business-data-capacity-live-20261106/save-verification.json),
[Rollback](evidence/business-data-capacity-live-20261106/rollback-verification.json).

Offen bleiben Originalinitialisierung, frühe Weltregistrierung, Kauf, echte
Geldwäsche und Persistenz/Reload; diese Probe belegt sie nicht.
