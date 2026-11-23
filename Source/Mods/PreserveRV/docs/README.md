# PreserveRV

Behält das Wohnmobil (RV) aus „Willkommen in Hyland Point", obwohl die Quest es
ursprünglich sprengen lässt. Der Originalzettel (`CartelNote`) bleibt erhalten und
wird für „Lies den Zettel" weiterverwendet; es gibt keinen Ersatzzettel.

## Stand

**Begrenzte Ingame-Abnahme am 12.11.2026, DLL `5CC97AEE…`.** Dominik hat in einem
frischen Tag-0-Stand das intakte Wohnmobil und den lesbaren Originalzettel bestätigt.
`Latest.log` 17:38–17:41: Hooks 2/2, Notiz nach dem Laden an die Wurzel, um 17:41:23
„original RV destruction blocked; original note prepared“. Screenshot 17:41: intaktes RV,
Quest „Read the note“, Zettel auf dem Boden, Tag 0 / Montag 9:42, 375 Dollar.
Nicht belegt: Speichern/Neuladen danach, der Folgeauftrag, bereits zerstörte Stände.

## Verhalten

- Hängt sich als eigener Harmony-Eigentümer `Dominik.PreserveRV` explizit an
  `Quest_WelcomeToHylandPoint.BlowupRV` und `Quest_WelcomeToHylandPoint.SetRVDestroyed`
  (kein `PatchAll`, keine Harmony-Attribute). Beide Methoden verändern nativ RV-Zustand
  inline, deshalb werden beide vor dem Laden der Szene blockiert und nicht nur
  Aliasnamen gepatcht.
- Der Block gilt nur in der Hauptszene, für Host/Einzelspieler, bei lebendigem
  `Lobby`-Singleton außerhalb einer Lobby und mit höchstens einem Spieler. Unbekannte
  oder werfende Prüfungen blockieren nichts. Ein früher Restore benötigt bewusst
  keinen `Player.Local`.
- Beim Blockieren wird zusätzlich `RV._exploded = true` gesetzt. Beleg: `RV.BlowUp()`
  liest in der installierten `GameAssembly.dll` (f12, SHA-256 `61829050…`) das Bool bei
  Offset `0x289` und kehrt bei `true` sofort zurück. Damit ist auch jeder andere Aufrufer
  neutralisiert; `IsDestroyed` bleibt `false`, sodass das intakte Property weiter
  aktualisiert und gespeichert wird.
- Der Originalzettel wird mit `SetParent(RV.transform, true)` an die Property-Wurzel
  gehängt; Weltposition und `activeSelf` bleiben unverändert. Kein neues Objekt, keine
  gelöschten Listener, keine manuellen Quest-Events.
- Nach `OnLoadComplete` wird die Notiz erneut aus dem Wrack geholt (ein Reload baut die
  Szene aus dem Prefab neu auf). Der Abgleich ist idempotent und fasst die Sichtbarkeit
  nicht an.
- **Scheitert die Zettel-Vorbereitung, wird nicht blockiert.** Dann läuft der native Pfad,
  damit die Quest nicht in einer Sackgasse endet; das Mod meldet
  „native RV destruction left enabled". Ebenso bei einer Ausnahme im Präfix.
- Sichtbarkeit bleibt nativ: `Investigate`/`Read` rufen `SetActive` auf der Notiz auf.
  Nach dem Umhängen unter die aktive Wurzel wirkt genau dieser native Aufruf. Das Mod
  erzwingt weder Sichtbarkeit noch `IsDestroyed` und wertet keine Quest-Eintragstitel aus.

## Grenzen (offen)

- Abgenommen am 12.11.2026 nur das intakte Wohnmobil und das Lesen des Originalzettels.
  Speichern und Neuladen danach, der Folgeauftrag und bereits zerstörte Stände bleiben offen.
  Der Zettel behält seine Original-Weltpose.
- Gemessene Zettelposition (statisch aus `level1`): Welt `(15,250 / 0,283 / −80,290)`,
  4,211 m von der RV-Wurzel, 0,272 m von der Karosserie-MeshCollider `Main.007`.
  Die Notiz liegt **außerhalb** aller intakten RV-BoxCollider und außerhalb des
  Enclosure-Triggers; nur ihre 0,30-m-Interaktionssphäre überschneidet die Karosserie
  um rund 0,03 m. Erwartung: sichtbar und bedienbar, aber am hinteren Chassis knapp.
- Der Questablauf selbst bleibt nativ und ist nicht von RV-Zustand abhängig: nativ
  `Quest_WelcomeToHylandPoint.Update` (RVA `0x55D620`) liest keinen RV-Zustand; die
  Abschlussbedingung von `ReturnToRVQuest` hängt an Quest- und Eintragsstatus.
  Wegen `IsDestroyed = false` setzt das Property weiterhin Speicherung und
  `UpdateVariables` fort. Belegt aus der Binary, nicht aus einem Spiellauf.
- Alte Speicherstände mit bereits zerstörtem RV bekommen keine verlorenen Inhalte
  zurück; das Mod meldet diesen Zustand nur.
- Multiplayer ist nicht unterstützt und nicht geprüft.

## Nachweise

Ausgeführt und bestanden: 21/21 Tests, No-Deploy-Build 0 Warnungen/0 Fehler,
`verify.ps1 -Mode Quick` 7/7, Installation mit Quittung, installierte DLL byte-identisch
zum geprüften Kandidaten (`5CC97AEE…`).

Statische Gegenprüfung der beiden Kernannahmen in Binary und Szenenassets:

- `BlowupRV` ruft **nicht** `RV.BlowUp()` auf, sondern wiederholt dessen Befehlsfolge
  inline (`_exploded`, Log, `onExplode`, Explosion bei Server, Shake, `IsDestroyed`,
  `onDestroyedState`) — ohne Questzustände, Variablen oder Notizaktivierung. Bei
  `_exploded == true` (Offset `0x289`) kehrt die Methode sofort zurück.
- `Quest_WelcomeToHylandPoint.Update` (RVA `0x55D620`) liest keinen RV-Zustand; das
  Beibehalten von `IsDestroyed = false` kann den Abschluss von `ReturnToRVQuest` daher
  nicht erneut auslösen.
- `_exploded` ist unbedenklich: Im gesamten RV-Methodenbereich `0x55D8C0–0x55E400`
  berührt nur `BlowUp` den Offset `0x289` (Prüfung `0x55D91B`, Setzen `0x55D927`).
  Kein Save-Datentyp führt das Feld; es wird nicht persistiert und nirgends sonst gelesen.

## Tests

`dotnet test Source/Tests/PreserveRV.Tests/PreserveRV.Tests.csproj -c Release`

Die Tests linken die Produktionsdateien gegen minimale verwaltete Attrappen
(kein Unity-/IL2CPP-Runtime-Ersatz) und decken Zettel-Reparenting mit Welterhalt,
Idempotenz, mehrdeutige/fehlende/tote Kinder, Fenster- und Endzustände der
Quest-Einträge, Lade-Abgleich, Präfix-Gate, Hook-Auswahl, PatchGuard-Ablehnung und
Rollback ab.

## Build

`dotnet build Source/Mods/PreserveRV/src/PreserveRV.csproj -c Release -p:S1NoDeploy=true`

Vorher `pwsh -NoProfile -File Tools/check-deploy-preflight.ps1` ausführen. Ohne
`S1NoDeploy=true` würde der Build installieren.
