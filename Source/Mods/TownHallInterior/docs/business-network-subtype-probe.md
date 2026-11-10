# TownHall: eigene Business-/NetworkObject-Bindung

Stand: 2026-11-06. Bindungsbaustein nach dem bestandenen
[aktiven Business-Untertyp](business-active-subtype-probe.md) implementiert und
offline geprüft. Kandidat `beb088f2…` am 2026-11-06 um 13:05:17 CET im begrenzten
Bindungs-/Lifecycle-Umfang beobachtet. Normalbuild seit 13:09:54 CET wiederhergestellt.

## Beobachteter Bindungs-/Lifecycle-Lauf

Frischer Prozess 17496 seit 13:04:33 CET, TownHallTest / SaveGame_2, Bericht
`20261106T120517402Z.json`, SHA-256
`64f953b5728cc5046bf8639421ce97d47e64a9ef0588b8864e33c3f1a9542c83`.
Loader, installierte DLL und Bericht passen zum Kandidaten. `observed`, keine
Findings, `NativeEvidenceFromThisAttempt=true`; unabhängiger Live-Review bestätigt
den begrenzten Umfang.

- Business-Cache und NetworkObject zeigen auf dieselbe eigene Instanz;
  Komponentenindex 0, Behaviour-Array enthält genau die eigene Business-Instanz.
- Echte Unity-Frames 1361 → 1362. Beide Komponenten jeweils Awake/OnEnable/Start/
  OnDisable/OnDestroy 1, FixedUpdate 10. Zehn Aufrufe sind keine zehn Frames;
  die Reihenfolge zwischen den beiden Komponenten ist nicht nachgewiesen.
- 28 Register-/Halterfingerprints und 15 fremde VTables unverändert. ObjectId
  65535/Unset, SceneId 0, keine Client-/Serverinitialisierung, detached/unspawned.
  Eigener Rückbau und native Abwesenheit von Root/Business/NetworkObject bestätigt.
- Nach tatsächlich geschlossenem Spiel 39/39 native Testdateien bytegleich,
  keine Dateien hinzugefügt/gelöscht. Leere Baseline-Verzeichnisse waren nicht
  inventarisiert. Normalbuild `614b81fd…` / PDB `8126ae65…` um 13:09:54 CET
  über den Helper wiederhergestellt und unabhängig geprüft.

[Laufzeitnachweis](evidence/business-network-subtype-live-20261106/observation.json),
[Save-Abgleich](evidence/business-network-subtype-live-20261106/save-verification.json),
[Normalrollback](evidence/business-network-subtype-live-20261106/rollback-verification.json).
Keine neuen Builds/Tests im Liveabschluss. Originale Business-/FishNet-
Initialisierung, Spawn, Weltregistrierung, Kauf, Geldwäsche und Persistenz bleiben
offen. Folgeschritt ist die unten beschriebene eigene native Datenvorbereitung.

## Vorbereiteter und inzwischen abgeschlossener Testablauf

Auf Dominiks „dann installiere damit ich testen kann“ vorhandene geprüfte DLL/PDB
ohne Neubuild über den Deploymenthelper installiert. Zwei Dateien ersetzt,
Metadaten unverändert. Frischer privater Checkpoint
`20261106T120014681652Z-bbe38997`: 39 native TownHallTest-/Slot-2-Dateien,
175 UserData-Dateien und bisherige normale Mod-Fassung. Slot 1 nicht geöffnet.
Steam Cloud lokal deaktiviert; bestehende Konfliktmarker nicht aufgelöst.
[Installation und Hashabgleich](evidence/business-network-subtype-deployment-20261106/deployment-verification.json).

Der Nutzer startete über Steam und meldete **TownHallTest / Slot 2** geladen.
Frischer BusinessNetworkSubtype-Bericht und Spielprotokoll wurden geprüft.
Nach regulärem Spielende erfolgten Save-Abgleich und Normalrollback. Kein manueller
Save-/Kauf-/Geldwäschetest; normale Autosaves waren nicht deaktiviert.

## Problem und begrenztes Ziel

Die bisher aktive eigene Business-Shell besitzt kein NetworkObject. Die
S1API-Manager liefern Wrapper vorhandener nativer Businesses; daraus ergibt sich
keine Factory für das Rathaus. Native Kauf-/Geldwäschepfade benötigen weitere
Initialisierung. Das bloße Setzen eines Netzwerk-Caches beweist deren Vertrag nicht.

Dieser Baustein ergänzt eine eigene no-base NetworkObject-Unterklasse und die
gegenseitige Bindung zur eigenen Business-Instanz. Er prüft Konstruktion,
Referenzen, Unity-Aktivierung, spätere Frames und bestätigten eigenen Rückbau.
Die originalen FishNet-Lebenszyklusmethoden werden dabei nicht aufgerufen. Deren
Awake-/OnDisable-Pfade können weitere Listen und Despawn-Zustände berühren; sie
bleiben ein eigenständiger späterer Integrationsschritt.

## Zuständigkeit und Übergänge

- Optionaler Buildmodus `S1TownHallBusinessNetworkSubtype`, eigener Bericht unter
  `UserData/TownHallInterior/BusinessNetworkSubtype`. Normale und bisherige
  Business-Proben bleiben getrennt. Die alte globale Aktivierungsroute bleibt gesperrt.
- Nur ein Versuch pro Prozess in vollständig geladenem Singleplayer
  TownHallTest / SaveGame_2, mit bereiter Halle und passenden Abhängigkeiten.
  Ein Fehler mit möglicher Teilwirkung verbraucht den Versuch.
- Eigene Root mit genau Transform, eigener Business-Unterklasse und eigener
  NetworkObject-Unterklasse. Typregistrierung und Konstruktion werden vor
  folgenden fehlbaren Operationen als begonnen vermerkt; Typregistrierungen
  bleiben bis zum Prozessende bestehen.
- Vor Aktivierung eigene Klassen, virtuelle und benannte Unity-Methodenziele,
  Signaturen, MethodPointer, VirtualMethodPointer und Invoker bestätigen.
  Insbesondere sind NetworkObject.OnDisable/OnDestroy native nichtvirtuelle
  Methoden: eigene namensgleiche Unity-Methoden ersetzen keinen direkten Aufruf
  eines Basisproxies. Solche Basisaufrufe gehören nicht zu diesem Pfad.
- Business-Cache zeigt exakt auf das eigene NetworkObject, Komponentenindex ist
  `0`, dessen Behaviour-Array enthält genau die eigene Business-Instanz. Keine
  fremden Eltern, Kinder, Besitzer oder Initialisierungs-/Spawn-Zustände zulassen.
  Bindungen, Kontext und Identitäten vor Eingriffen frisch lesen.
- Bekannte FishNet-Halter werden über dieselben gebundenen Manager und Collections
  geprüft. Nicht lesbare oder mehrdeutige Instanzen blockieren den Eingriff.
  Für den Client-Objektcache ist eine streng lesbare und leere Ausgangslage
  zulässig; das beweist keine vollständige transitive Referenzfreiheit.
- Inaktiv vorbereiten → Bindung bestätigen → einmal aktivieren → echten späteren
  Unity-Frame und eigene erwartete Callbacks beider Komponenten beobachten →
  deaktivieren → jeweiliges OnDisable bestätigen → eigene Root zerstören →
  jeweiliges OnDestroy und native Abwesenheit beider Komponenten bestätigen.
  Keine Reihenfolge zwischen den beiden Komponenten aus bloßen Zählerwerten ableiten.
- Unlesbare Identität, fremde Referenzen, Kontextverlust oder unbekannte Teilwirkung
  sperren weitere Mutationen und Wiederholung. Keine fremden Listen zurücksetzen
  und kein spekulatives Destroy. Widerruf während eines Eingriffs wird vor dem
  nächsten Eingriff berücksichtigt; Berichte nach Widerruf verwenden gespeicherte
  Beobachtungen.

## Abnahme und Prüfungen

Die Offline-Prüfung umfasst den tatsächlichen Produktionsablauf mit künstlichem
Port: Bindungsfehler, Identitätswechsel, fremde Referenzen, unerwartete Callbacks,
fehlenden Framefortschritt, Timeout, Kontextverlust, Reentranz und unbestätigten
Rückbau. Ein künstlicher Port beweist keine native Unity-Ausführung.

Vor Änderungen bestanden 973/973 TownHall-Tests; danach **104 neue Fälle und
1.077/1.077 insgesamt bestanden**. Unabhängiger Testautor und Quellreview prüfen
Ablauf und tatsächliche native Anschlussstellen getrennt. Der native Review
präzisierte Halteridentitäten, erneute Zustandsprüfung unmittelbar vor Eingriffen,
echte leere Backlinks statt Unity-Nullvergleich und Kontextwiderruf nach Destroy.
Normale, Subtype-, Interface-, Manager-, ActiveSubtype- und NetworkSubtype-Builds
bestanden ohne Deployment mit jeweils null Warnungen und Fehlern. Die alte
Aktivierung sowie Network + Manager und Network + ActiveSubtype wurden
erwartungsgemäß durch `CS1029` gesperrt. Scoped Format und unabhängiger nativer
Review sind abgeschlossen; die korrigierten Mutationsgrenzen wurden erneut gelesen.

Genaue Quell-/Kandidathashes, Build- und Testausgaben sowie Reviewbelege stehen in
den [Offline-Belegen](evidence/business-network-subtype-20261106/). Der isolierte
Pure-Gesamtbericht wird dort gesondert archiviert; ein bestandenes TownHall-Ergebnis
ist keine Repository-Gesamtfreigabe. Keine Spiel- oder Save-Aktionen in diesem Schritt.
Während der Offline-Entwicklung blieb Normalbuild `614b81fd…` unangetastet.
Die anschließend beauftragte Installation ist oben getrennt dokumentiert.

## Native Initialisierung: geklärte Grenze und Folgeschritt

Der frische Audit der aktuellen nativen Bytes zeigt: Property-Awake veröffentlicht
die Instanz vor erforderlichen PoI-/Verkaufsschild-Zugriffen. Start abonniert globale
Geld-/Zeitereignisse, InitializeSaveable registriert beim SaveManager. Diese Rümpfe
sind keine isolierten Feldinitialisierer. Der aktuelle Native-Untertyp ruft sie
nicht auf. S1API-Wrapper kapseln bereits vorhandene native Instanzen.

Nach dem begrenzten NetworkObject-/Bindungsnachweis folgt die eigene native
Datenvorbereitung: leere Business-/Property-Collections, Container, Bounds und
weitere benötigte Referenzen. Native Mengen- und Bounds-Abfragen über den
tatsächlichen BusinessWrapper können diese Datenseite separat prüfen. Originale
RPC-/FishNet-Initialisierung, frühe Weltregistrierung und echter Save-/Load-Weg
bleiben eigenständige offene Schritte. Audit-Fundstellen und native Grenzen stehen
ebenfalls in den Offline-Belegen; lineare Disassembly-Fenster sind kein vollständiger
Kontrollfluss- oder Laufzeitnachweis.

Kein Kauf, Eigentumswechsel, Geldwäsche, Spawn, Weltregistereintrag oder Save-Aufruf.
Die Probe beweist weder originale native Business-/FishNet-Initialisierung noch
Persistenz, Reload, allgemeine Last-Use- oder Weltwechselsicherheit. Der nächste
Integrationsschritt wird aus den tatsächlichen Bindungs-/Lifecycle-Befunden und
dem getrennten nativen Initialisierungsaudit abgeleitet.
[Fertigstellungsplan](business-completion-plan.md).
