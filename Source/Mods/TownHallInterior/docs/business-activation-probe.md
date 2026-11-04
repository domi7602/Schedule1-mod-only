# TownHall: kontrollierte Aktivierung und Registrierung

Stand 2026-11-03. **Kandidat `54d37fe5…` nach Startabsturz zurückgezogen.**
Seit 14:53:53 CET ist wieder die zuvor auf TownHallTest bestandene
[inaktive Konstruktionsdiagnose](business-construction-probe.md) `52bb577a…`
installiert. Aktivierung und Registrierung sind nicht bestanden.

## Startabsturz und Rückbau am 3. November

Dominiks vollständiges Konsolenlog zeigt um 14:49:52.387 CET zuerst eine
NullReferenceException im nativen/verwalteten `Property.CanBePurchased`-Trampolin,
danach einen Stackoverflow mit wiederholten Originalaufrufen. Die Aktivierungsprobe
war nur vorgemerkt: kein bestätigter Slot-Load, kein Aktivierungsbericht, keine
eigene Business-Instanz. Der einzige neue Beobachterbericht ist g0000 mit einem
Setup-Snapshot und null Loader-Ereignissen. Der spätere Kontrollpunkt der Lease
konnte den Fehler nicht abfangen, weil die globalen Hooks schon beim Start wirken.

Der Abgleich der aktuellen GameAssembly mit dem Generator-SHA512 und dessen
Cpp2IL-Adressmetadaten belegt: `Property.ShouldSave` und `CanBePurchased` teilen
RVA `0x4574D0`, Länge drei Bytes (`B0 01 C3`, konstant true). Insgesamt 61 Methoden
in Assembly-CSharp nutzen diesen Einstieg, darunter fremde Typen und statische
Methoden; der Scan aller 111 generierten Assemblies findet insgesamt 357 Aliase
dieses Stubs. Zwei weitere Doppelziele innerhalb der neuen Hookmenge betreffen
`Property/Business.NetworkInitializeIfDisabled` sowie
`Property.ReceiveOwned_Networked` / `RpcLogic___SetOwned_Server_2166136261`.
Ein instanzprüfender Prefix isoliert den nativen Detour daher nicht. Nur einen
Bool-Hook zu entfernen oder gleiche Ziele zu deduplizieren wäre kein belegter Fix.
Der genaue erste native Aufrufer und die vollständige Laufzeit-Trampolinkette
wurden nicht instrumentiert. [Adressaudit](evidence/business-activation-native-aliases.json).

Der zuerst vermutete `Il2CppObjectBaseToPtrNotNull`-Matcherfehler wurde verworfen:
Der innere Virtual-Method-Resolver der aktuellen Proxys verwendet das passende
`Il2CppObjectBaseToPtr`. Der frühere Review der bool-Rückgabe-ABI war korrekt,
deckte jedoch gemeinsam verwendeten nativen Code und Originaldispatch nicht ab.
273 bestandene verwaltete Tests und die Builds bleiben historische Offline-Belege;
sie bewiesen diese native Grenze nicht.

Die DLL/PDB `52bb577a…` / `722623f1…` wurden bei geschlossenem Spiel aus ihrem
hashgeprüften Deployment-Backup wieder installiert. Der fehlerhafte Kandidat ist
im neuen Backup erhalten. Direktes Readback bestätigt beide installierten Hashes.
Alle 39 nativen Slot-2- und 38 Slot-1-Dateien stimmen mit dem Checkpoint überein;
kein Nachweis über fremde Mod-Sidecars oder entfernte Cloud-Daten. Kein Spielstart
durch den Agenten. [Fehleranalyse und Grenzen](evidence/business-activation-crash-review.json),
[Rollback-Receipt](evidence/business-activation-rollback-receipt.json).

`TOWNHALL_BUSINESS_ACTIVATION` ist im Quelltext durch `#error` gesperrt, auch bei
manuell gesetztem Symbol. Der dokumentierte Aktivierungsschalter darf damit keinen
neuen installierbaren Build ergeben. Normal- und inaktive Konstruktion bleiben
separate Buildpfade; der installierte Rückweg ist das geprüfte Originalartefakt.
Die gezielte Prüfung bestätigt den erwarteten CS1029-Abbruch, einen Normalbuild
mit 0 Warnungen/Fehlern sowie 273 bestandene TownHall-Tests. Installierte DLL und
sämtliche TownHall-Deployment-Receipts blieben dabei unverändert.
[Buildsperren-Prüfung](evidence/business-activation-containment.json).

## Nächster technischer Schritt

Zuerst einen eigenen registrierten IL2CPP-Business-Untertyp **inaktiv** untersuchen:
native Klassen-/VTable-Identität, Aufrufe über einen nachweislich nativen Basistyp-
Proxy oder nativen Virtual-Resolver und tatsächlich beobachtete eigene Overrides
ohne Änderung fremder MethodPointer. Ein C#-Cast mit anschließendem verwaltetem
virtuellen Aufruf reicht als Dispatch-Nachweis nicht aus. Vorher den
ClassInjector-Vertrag der installierten Bridge prüfen. Register müssen unverändert
bleiben und der eigene Rückbau muss bestätigt sein. Danach erst NetworkObject-
Lifecycle und nichtvirtuelle Pfade klären, bevor Aktivierung/Registrierung erneut
getestet werden. Das ist ein Untersuchungsweg, noch keine implementierte Lösung.

**Der folgende Vertrag beschreibt den zurückgezogenen Versuch. Seine Testanleitung
ist ausgesetzt und darf nicht als Freigabe zum erneuten Aktivierungstest gelten.**

## Ziel und klare Grenze

Genau ein echter nativer `Business`-Component mit eigenem `NetworkObject` wird
aktiv. Derselbe Business-Pointer steht jeweils einmal in `Property.Properties`,
`Property.UnownedProperties`, `Business.Businesses` und `Business.UnownedBusinesses`.
Die Owned-Listen sowie die vier überwachten Save-Listen enthalten keinen eigenen
Eintrag. Es gibt keinen zweiten Property-Component für denselben Ort.

Die nativen Awake-/Start-Rümpfe können externe Container und globale Abonnements
anlegen. Für diese begrenzte Probe bleibt der Adapter alleiniger Besitzer der
Registrierung: Eigene native Awake/Start, FixedUpdate, OnDisable/OnDestroy und
Netzwerkinitialisierung werden mit instanzgebundenen Guards unterdrückt. Die Root
ist tatsächlich aktiv; nach mindestens einem Folgeframe müssen die erwarteten
Business-/NetworkObject-Awake-/Start-Aufrufe gesehen worden sein. Das belegt
kontrollierte Aktivierung, **keine vollständige native Initialisierung**.

Es entstehen keine zusätzlichen Gebäude, Möbel, Stationen oder laufenden
Geldwäschevorgänge. Kauf und native Besitz-/Geldwäschemethoden sind für diese eigene
Instanz gesperrt. Die geometrische Halle und ihre NPC-Zuordnung sind unabhängig.
Die Reihenfolge eines vorgeschalteten nativen Manager-/UI-Kaufs vor einer
Geldabbuchung ist nicht bewiesen; diese Probe erteilt keine Kaufabnahme.
Die vier Registereinträge werden nach abgeschlossenem Load erzeugt; rechtzeitige
Registrierung vor nativer Wiederherstellung ist erst Teil des Persistenzschritts.

## Ablauf, Fehler und Rückbau

Bestätigter Load → sechs Sekunden warten → Kontext prüfen → eigene inaktive
Instanz vorbereiten → aktivieren → vier Registereinträge hinzufügen → Folgeframe
und Lifecycle-Sichtungen prüfen → aktive Diagnoseinstanz behalten → vor Save oder
Weltwechsel abmelden → eigene Objekte zerstören → Abwesenheit nachweisen.

Nur TownHallTest / SaveGame_2 bei bestätigtem Singleplayer, bereiter Halle,
passenden Abhängigkeitshashes und nicht laufendem Save. ActiveSave-/LoadedGame-Pfade
müssen absolut, normalisiert identisch und dem Testslot zugeordnet sein. Der
Kontext wird im Bericht exportiert. Jeder Ladevorgang erhält genau einen Versuch
und eine eigene Berichtsdatei. Doppelte LoadComplete-Meldungen öffnen keinen neuen
Versuch; eine neue Generation verlangt bestätigten alten Rückbau.

Alle zehn Register werden begrenzt gelesen. Für die fremden Einträge werden
Listenidentität und geordnete Pointerfolge gehasht; eigene Vorkommen werden
separat gezählt. Bei `QueuedSaveRequests` zählt zusätzlich das Ziel `Saveable`
des Requests. Nie ganze Listen leeren oder auf einen alten Gesamtstand zurücksetzen.
Der Rückbau vergleicht den unmittelbar davor gelesenen fremden Bestand, damit
legitime fremde Änderungen während der Sitzung erhalten bleiben.

Fehler nach jedem einzelnen Add bleiben rückbaubar. Eigene Einträge werden
rückwärts aus allen sechs Property-/Business-Listen und den vier Save-Listen
entfernt. Erst nach lesbar bestätigter Abmeldung werden eigene Objekte zerstört.
Maximal zwei Rückbauversuche. Ein unbekannter Zustand hält weitere Aktivierung
gesperrt und die eigenen Schutzpatches installiert; die Halle räumt sich separat auf.

## Speicherung und Bedienung während der Probe

`ShouldSave` und `CanBePurchased` liefern nur für die eigene Instanz false.
Eigene RegisterSaveable-/QueueSaveRequest-Aufrufe werden blockiert. Das allein
beweist keine Speicherisolierung: Native Manager könnten ihre Business-Listen
direkt durchlaufen. Deshalb beenden beide parameterlosen bzw. String-Überladungen
von `SaveManager.Save` die Probe synchron **vor** dem ursprünglichen Save-Aufruf.
Sie wird in derselben Generation nicht wieder veröffentlicht.

Ist eigene Abmeldung/Rückbau unbestätigt oder der Save reentrant während einer
laufenden Mutation, wird der native Save-Einstieg abgewiesen und sichtbar geloggt.
Andere Mods können dennoch eigene Save-Postfixes besitzen; dieser Fehlerfall ist
keine atomare Transaktion über sämtliche Moddaten. Der gesicherte Testcheckpoint
bleibt der Rückweg. Native Methoden mit IL2CPP-Objekt-Rückgabe wie Manager.WriteData
werden wegen ihres ungeprüften Trampolinvertrags nicht gepatcht. Direkte fremde
WriteData-Aufrufe außerhalb der normalen Save-Einstiege sind nicht abgedeckt.

Time-/Money-Abonnements und ungenannte native Register werden nicht inventarisiert.
Unterdrückte native Initialisierung und unveränderte zehn Register sind kein
umfassender Nebenwirkungsnachweis. Keine Veröffentlichung oder vollständige
Gameplay-Abnahme aus einem erfolgreichen Diagnosebericht ableiten.

## Artefakte und Prüfung

Build-Schalter `S1TownHallBusinessActivation=true` enthält die kombinierte Inventur
und kontrollierte Aktivierung. Die frühere einmalige inaktive Probe läuft dabei
nicht zusätzlich. Normale Builds enthalten keinen dieser Laufzeitadapter.
Die zusammengehörige Konstruktion, Lease-Logik und native Registrierung bleiben
in `src/BusinessConstruction.cs`; die Mod bleibt bei 20 Produktionsdateien
einschließlich Projektdatei. Es kommt keine neue Framework-Abhängigkeit hinzu.

```powershell
dotnet build Source/Mods/TownHallInterior/src/TownHallInterior.csproj -c Release -p:S1NoDeploy=true -p:S1TownHallBusinessActivation=true
dotnet test Source/Tests/TownHallInterior.Tests/TownHallInterior.Tests.csproj -c Release -p:S1NoDeploy=true
```

Die Lease-Tests prüfen unter anderem falsche Kontexte, Teilregistrierung, doppelte
Einträge, einen tatsächlich späteren Frame, Reentranz, Snapshot-Aliasing, fremde
Änderungen während der Sitzung, unlesbare Abmeldung und Übergabe an eine neue
Generation. Quellreview, Build, Installation und Laufzeitprüfung werden getrennt
mit Artefakthashes belegt.

**273 TownHall-Tests bestanden**, Kandidat- und Normal-Build mit jeweils
0 Warnungen und 0 Fehlern. Unabhängiger Source-Review durch `probe_review` ohne
verbleibenden blockierenden Befund im begrenzten Scope. Save-Start, geschützte
native Pointer beim Rückbau, Adresswiederverwendung nach Cleanup sowie korrekte
Cleanup-/Quarantäneberichte wurden dabei nachgeschärft.
[Artefakte und Quellhashes](evidence/business-activation-offline.json),
[Testbericht](evidence/business-activation-tests.trx). Nativer Aktivierungs- und
Menü-Reload-Test stehen aus; kein Spielstart durch den Agenten.

## Test durch Dominik

1. Über Steam starten, TownHallTest / Slot 2 laden, zehn Sekunden warten und offen lassen.
2. Den Bericht unter `UserData/TownHallInterior/BusinessActivation/` prüfen lassen.
   Erwartet: `active`, keine Findings, aktive eigene Instanz, vier eigene Einträge,
   alle anderen eigenen Zähler null und erwartete blockierte Awake-/Start-Aufrufe.
3. Nach bestätigtem Erstbefund ins Hauptmenü wechseln und denselben Slot erneut laden.
   Alter Bericht muss bestätigten Rückbau enthalten, neuer Bericht genau eine neue
   Instanz. Ein automatischer Save kann die Probe schon vor dem Menüwechsel schließen;
   das wird als `native-save-boundary` dokumentiert und ist beabsichtigt.

Kein Kauf oder Geldwäschetest in diesem Schritt. Der nächste Meilenstein ist die
native Initialisierung samt frühem Registrierungs- und Save-/Restore-Vertrag aus
dem [Fertigstellungsplan](business-completion-plan.md).
