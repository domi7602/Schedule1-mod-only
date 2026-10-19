# LightControl.Core: implementierter, spielunabhängiger Modellkern

## Status und Grenze

Dieses Projekt enthält C#-Quelltext für .NET 6 ohne Unity-, MelonLoader-, S1API-, Steam- oder sonstige Paketabhängigkeit. Es ist **keine installierbare Spielmod**. In der bereitgestellten Umgebung stand kein ausführbarer C#-Compiler zur Verfügung; eine erzeugte Assembly und ausgeführte C#-Tests werden deshalb nicht behauptet.

Der separate Console-Prüfharness enthält 62 verfasste Verhaltensprüfungen. Sie prüfen ausschließlich das reine Modell und seine simulierten Nachrichten. Ein erfolgreicher späterer Lauf würde weder eine reale Transportverbindung noch Rendering-, Spielstand-, Telefon-, Input- oder Lampenadapter nachweisen.

## Zuständigkeiten und öffentliche API

| Bereich | Öffentliche Einstiegspunkte | Verbindlicher Vertrag |
|---|---|---|
| Sitzung | `LightStateStore.BeginSession`, `EndSession` | Verbindungs-, Spielstand- und Moduswechsel beginnen mit einer neuen Sitzungs-GUID. Alle lokalen Zustände, Effekte, Bindungen und Replay-Daten werden verworfen. |
| Identität | `PersistentLampId`, `LampIdentity.Persistent`, `LampIdentity.Ephemeral` | Der Adapter liefert eine nachweislich eindeutige Instanzkennung. Itemdefinitionen werden weder erkannt noch automatisch in Instanzkennungen umgewandelt. |
| Laufzeitregister | `Register`, `Move`, `MarkTemporarilyUnloaded`, `RebindTemporarilyUnloaded`, `RemovePermanently` | Mehrere Lichtquellen/Materialflächen einer Leuchte werden vom Adapter unter einer logischen Identität zusammengefasst. Jeder Eintrag bindet genau eine vom Adapter vergebene Laufzeitkennung. |
| Lesen | `GetRegistrations`, `GetBaseState`, `Effects`, `Revision` | Rückgaben enthalten unveränderliche Daten beziehungsweise defensive Kopien. UI und Interaktion lesen diesen gemeinsamen Zustand. |
| Befehle | `ControlCommand.Set`, `Cycle`, `StartDisco`, `StopDisco`; `ExecuteHost`, `ExecuteLocal` | Alle Gruppenmitglieder werden zum Ausführungszeitpunkt erfasst. Der Host autorisiert die ganze Änderung; Clients verändern keine maßgeblichen Grundwerte direkt. |
| Synchronisation | `CreateHostSnapshot`, `ApplyHostSnapshot`, `ApplyHostDelta` | Nur Datenprotokoll, Protokollversion 1. Authentifizierung, Mod-Erkennung, tatsächlicher Transport und Uhrabgleich müssen extern nachgewiesen werden. |
| Darstellung | `ResolveVisual`, `VisualResolver.Resolve`, `VisualInstruction` | Ergebnis sind eine optionale RGB-Farbvorgabe und ein Helligkeitsfaktor. Es werden keine nativen Schaltaktionen, Materialänderungen oder Gameplayänderungen ausgeführt. |
| Speicherdaten | `JsonStatePersistence.ConfirmDataMissing`, `LoadJson`, `ReportReadFailure`, `CreateJsonForSave` | Initialer Host-Ladevorgang vor Registeraufbau; Schema 1; Schreibsperre nach Lesefehlern. Die API erzeugt JSON im Speicher und schreibt keine Datei. |

## Identitäten und Lebenszyklus

`PersistentLampId` verlangt Identitätsart, Namensschema, Schemaversion und Wert. Die zulässigen Arten heißen `VerifiedNativeInstance` und `VerifiedWorldLocator`. Diese Namen sind eine Anforderung an den integrierenden Adapter; der Kern kann den Nachweis einer nativen Kennung nicht selbst erbringen. Ein Weltlokator benötigt außerhalb dieses Projekts ein nachgewiesenes Kollisions- und Versionsverfahren. Die Feldcodierung verhindert lediglich eigene Trennzeichenkollisionen; sie erfindet keinen sicheren Weltlokator.

Ohne bestätigte dauerhafte Kennung wird eine ausdrücklich vergängliche `LampIdentity.Ephemeral` verwendet. Ihre Identität enthält die Sitzung und einen eindeutigen Token. Host und Mod-Clients müssen den gleichen Token für dieselbe Instanz bekommen; lokale Unity-Objektnummern sind nicht automatisch ein gemeinsamer Token. Vergängliche Kennungen werden nicht gespeichert.

Ein zweites gleichzeitig registriertes Objekt mit derselben logischen Kennung und anderer Laufzeitkennung wird abgelehnt. Ebenso wird eine Laufzeitkennung für zwei logische Leuchten abgelehnt. Eine bestätigte vorübergehend entladene Leuchte kann explizit neu gebunden werden; ihr Grundzustand bleibt erhalten. Bewegung verändert ausschließlich Grundstücks- und Gruppenzuordnung. Tatsächliche Entfernung entfernt auch vorgemerkte dauerhafte Einstellungen. Eine anschließend neu registrierte Ersatzinstanz beginnt mit Originalfarbe und 100 Prozent.

Vorübergehend entladene, bereits bekannte Leuchten bleiben vorhandene Gruppenmitglieder. Noch nie registrierte, aus Speicherdaten vorgemerkte Kennungen werden von Gruppenänderungen nicht erfasst. Neu platzierte oder später einer Gruppe zugeordnete Leuchten erben frühere Gruppenbefehle nicht.

Laufzeitkennungen, Grundstücke, Schaltgruppen, Lampenfamilien, Fähigkeiten und tatsächliche Objektreferenzen werden durch eine spätere nachgewiesene Spielintegration bestimmt. Der Kern enthält kein allgemeines Raum-API und keine Objektsuche.

## Host, lokaler Modus und Protokoll

Ein `ClientCommand` enthält Protokollversion, Sitzung, eindeutige Befehls-ID, erwartete Revision und den eigentlichen Änderungswunsch. Die verifizierte Absenderkennung wird durch den Transport separat an `ExecuteHost` übergeben; sie darf nicht aus einem unbestätigten Nutzdatenfeld übernommen werden.

`ExecuteHost` prüft Modus, Protokoll, Sitzung, Replay und erwartete Revision, erfasst die gegenwärtigen Zielmitglieder und ruft den verpflichtenden Autorisierungsdelegaten auf. Dieser erhält Auswahl und unveränderliche Zielkopie. Er muss echte Zielzulässigkeit, unterstützte Fähigkeiten und das bestätigte gemeinsame Grundstücksmodell prüfen. Er darf ein Grundstücksbesitz-Flag nicht ungeprüft als personenbezogenes Zugriffsrecht interpretieren. Effekte können bewusst für ein noch leeres Grundstück autorisiert werden, damit später platzierte Leuchten teilnehmen.

Der Autorisierungsdelegat muss kurz, synchron und ohne blockierende externe I/O arbeiten. Er läuft innerhalb der Modell-Transaktion. Er darf den gelieferten Kontext und Modellgetter lesen, aber keine Modellmutation, Persistenzoperation, Transportaktion oder wartende Aufgabe starten. Reentrante Modellmutationen werden abgelehnt; Ausnahmen des Delegaten führen zu `AuthorizationFailed`, ohne die angeforderte Gruppenänderung teilweise auszuführen.

Jeder angenommene Befehl erzeugt genau eine zusammenhängende Revision und ein Delta; auch ein Befehl mit bereits gewähltem Wert wird eindeutig abgeschlossen. Gleichzeitige Befehle mit derselben erwarteten Revision haben höchstens einen erfolgreichen Ausführungsstand. Der andere erhält `StaleRevision` und benötigt eine bewusste Aktualisierung beziehungsweise erneute Nutzerentscheidung. Es gibt keine verdeckte Endlosschleife zum Wiederholen von Befehlen. Erfolgreiche Befehls-IDs werden je Absender begrenzt gespeichert; ältere Wiedergaben scheitern danach an ihrer veralteten Revision.

Neue Hostregistrierungen und echte Löschungen liefern ebenfalls Deltas, sofern sich der Zustand verändert. Die Integration muss diese Rückgaben ebenso zustellen wie Befehlsdeltas. Das Register selbst enthält lokale Objektbindungen und wird nicht als vorgetäuschte native Welttopologie übertragen.

Ein Snapshot enthält alle Grundzustände und aktiven Effekte. Er kann vor den lokalen Objekten ankommen; die Einstellungen bleiben bis zur Registrierung vorgemerkt. Deltas enthalten ausschließlich neue Grundzustände, bestätigte Entfernungen und gegebenenfalls eine vollständige neue Effektmenge. Die Empfangsseite ruft keine Sendefunktion auf; daraus entsteht keine automatische Echo-Schleife.

Ein Client nimmt zunächst einen vollständigen Snapshot an. Ein zu früh oder mit einer Lücke eingetroffenes gültiges Delta erhöht die mindestens benötigte Snapshotrevision. Ein danach eintreffender älterer Snapshot wird nicht als vollständiger Abgleich akzeptiert. `NeedSnapshot` und `NeedsSnapshot` verlangen einen neu angeforderten vollständigen Abgleich; der Kern puffert keine unbeschränkt wachsende Nachrichtenfolge. Aufeinanderfolgende später zugestellte fehlende Deltas können eine bereits initialisierte Revisionslücke ebenfalls schließen. Alte Snapshots und Deltas überschreiben keinen neueren Stand.

Ein lokaler Darstellungsmodus ist ausdrücklich `SessionMode.Local`. Er darf `ExecuteLocal` verwenden, aber keine maßgeblichen Snapshots oder dauerhafte Lampendaten exportieren. Beim Wechsel auf einen Mod-Host beginnt eine frische Sitzung; die lokalen Lampenwerte werden nicht übertragen. Eine anzuzeigende Kennzeichnung dieses Modus ist Aufgabe der noch offenen Oberfläche.

## Darstellung und Effekte

Die Reihenfolge der elf Farbmodi ist Original, Weiß, Rot, Orange, Gelb, Grün, Cyan, Blau, Lila, Rosa, Regenbogen. Die sieben erlaubten Intensitätsstufen sind 10, 25, 50, 75, 100, 150 und 200 Prozent.

Grundfarbe und temporäre Disco sind getrennt. Grundstücksdisco überlagert Weltdisco. Das Ende der Grundstücksdisco legt eine weiterhin laufende Weltdisco frei; das Ende aller Effekte legt die aktuelle Grundfarbe frei. Eine Farbänderung während Disco verändert genau diese Grundfarbe. Eine Intensitätsänderung wirkt sofort auch unter einem Effekt.

Disco verwendet gemeinsame sitzungsrelative Zeit, Startzeit, Schrittweite, Seed und einen fest definierten UTF-8-FNV-1a-Hash der logischen Kennung. Die Hashberechnung verwendet einen begrenzten Stackpuffer und keine pro Aufruf erzeugten UTF-8-Bytearrays. Eine nachfolgende feste Ganzzahlmischung bestimmt die Farbe des aktuellen Zeitschritts. Es werden keine Framefarben serialisiert. Der Regenbogen verwendet ebenfalls die gemeinsame Zeit. Standard sind zehn Sekunden pro Zyklus; zulässig sind ein bis 120 Sekunden, entsprechend 60 bis 0,5 Zyklen pro Minute.

Die generischen Disco-Schutzgrenzen liegen bei 0,1 bis 60 Sekunden pro Schritt. Diese Grenzen sind absichtlich weiter als gegebenenfalls engere Benutzerkonfigurationsgrenzen. Effektzeit muss endlich, nicht negativ und innerhalb einer begrenzten sitzungsrelativen Skala liegen. Vom Host erzeugte Ereigniszeiten dürfen nicht rückwärts laufen. Das tatsächlich abgestimmte Zeitmodell mit Netzwerklatenzkorrektur bleibt Integrationsaufgabe.

`ColorOverride == null` bedeutet, die gerade gültige unveränderte Spielfarbe zu erhalten. Ein Intensitätsfaktor von eins bedeutet, keine Helligkeitsskalierung hinzuzufügen. Bei deaktivierter Mod gibt die Auflösung beide Vorgaben für vollständigen Rückbau zurück. `ScaleCurrentNativeIntensity` erwartet bei jedem Aufruf einen frisch erfassten unveränderten Spielwert; ein vorheriges Mod-Ausgabeergebnis ist keine zulässige neue Basis. Der Kern kann die Herkunft des übergebenen Basiswerts nicht selbst feststellen. Null bleibt null; keine Farb- oder Effektoperation schaltet eine Lampe ein.

RGB-Werte sind normierte Entwurfsfarben. Die zu verwendende Unity-Farbraumkonvention, HDR-Emission, Shaderparameter, Materialtexturen, gebackene Beleuchtung, native Animation und Ressourcenfreigabe sind offen. Es wurden keine Laufzeitkosten, Hauptthreadbudgets oder visuellen Wirkungen im Spiel gemessen.

## Persistenz und Fehlerschutz

Für jede frische Host-Sitzung wird genau ein `JsonStatePersistence` an den konkreten Store gebunden. Vor dem Registeraufbau muss der verifizierte Speicherweg genau einen dieser Zustände melden:

1. Vorhandenes JSON: `LoadJson` prüft und übernimmt es vollständig.
2. Nachweislich keine Mod-Daten vorhanden: `ConfirmDataMissing` initialisiert leere Daten.
3. Lesefehler beziehungsweise unklarer Zugriff: `ReportReadFailure` sperrt spätere Lampendatenexporte.

Beschädigtes JSON, doppelte Felder oder Identitäten, unbekannte Schemafassungen, andere Spielstandkennung, ungültige Zahlen, fehlende Felder, mehr als 10.000 Leuchten, mehr als vier MiB JSON oder zu tiefe Strukturen werden kontrolliert zurückgewiesen. Es gibt kein stilles Ersetzen durch eine leere Struktur. Schema 1 ist das erste unterstützte Format; für andere Fassungen existiert keine behauptete Migration.

Eine fehlgeschlagene Initialladung darf nach tatsächlicher Wiederherstellung mit gültigem JSON erneut versucht werden. Das nachträgliche Behaupten einer fehlenden Datei hebt die Fehlersperre nicht auf. Eine doppelte Benachrichtigung nach bereits erfolgreichem Initialload meldet `AlreadyLoaded` und erhält die gültige Schreibfreigabe. Ein ausdrücklich gemeldeter späterer echter Lesefehler sperrt die Freigabe. Ein erneuter vollständiger Ladevorgang nach schon aufgebautem Zustand benötigt eine neue Sitzung; aktive Lampenwerte werden nicht still überschrieben.

`CreateJsonForSave` liefert deterministisch nach Identität sortiertes JSON mit Schema, Spielstandkennung, dauerhafter Identität, Farbmodus und Intensitätsprozent. Temporäre Effekte, Framefarben, Laufzeitobjekte, Clientdaten und vergängliche Kennungen fehlen absichtlich. Vorgemerkte, momentan nicht registrierte dauerhafte Leuchten bleiben gespeichert. Tatsächliche Entfernung löscht ihren Eintrag.

**Dateiatomizität ist durch diese API nicht implementiert.** Der noch zu bestätigende Speicheradapter muss die vollständige Zeichenfolge im exakt aktiven Spielstand ablegen, gescheiterte Schreibvorgänge erkennen, bestehende gültige Daten erhalten und einen überprüften Sicherungs-/Wiederherstellungsablauf anbieten. Ein erfolgreich erzeugter JSON-String ist kein erfolgreich geschriebener Spielstand. Vor einem Schreiben ist der aktuelle Sitzungs-/Spielstandbezug erneut zu prüfen; auch danach muss der bestätigte Speicherweg verhindern, dass ein späterer Sitzungswechsel einen falschen Dateipfad verwendet.

## Notwendige Anschlussarbeiten

Erst ein tatsächlich verfügbarer .NET-6-Build und der Console-Testlauf können die Kompilierbarkeit und Modellprüfungen belegen. Danach fehlen weiterhin die gegen die konkrete Zielinstallation geprüften Lampenadapter, dauerhaften nativen Identitäten, Lebenszyklusereignisse, nativen Schaltaktionen, UI- und Eingabeeinbindung, Dateiablage und reale Host-/Client-Verbindung. Der eigene Kernname, seine Protokolle und sein Speicherformat sagen keine Kompatibilität mit GrowLightColors-Daten zu.
