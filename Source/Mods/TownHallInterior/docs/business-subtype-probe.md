# TownHall: inaktive eigene Business-Untertyp-Probe

Stand 2026-11-04. Die korrigierte Untertyp-/Property-Probe `59484a66…` wurde im
Spiel erfolgreich beobachtet. Die zusätzliche [Interfaceprobe](business-interface-probe.md)
`cb98bc95…` scheiterte am 2026-11-04 um 04:53:58 UTC vor dem Interface-Aufruf.

## Beobachteter Klassen-Dispatch und Reload

Der [Originalbericht vom 04:04:16 UTC](evidence/business-subtype-live-observed-report-20261104.json),
SHA256 `1021e4a2596ff39219531a4e6b93b8520b7168b61671650f35c714bdb648e782`,
meldet observed ohne Findings: ein registrierter eigener Untertyp, vier eng
begrenzte Mapping-Hits und bestätigter verwalteter Patchrückbau, beide nativen
Property-Rückgaben false mit je einem Callback. Genau Transform+Business,
zwölf eigene Methodenziele, keine unerwarteten Lifecycleaufrufe, bestätigter
eigener Unity-Rückbau. Zehn Register und 14 fremde VTables stimmen in allen
fünf Snapshotstufen überein.
[Review durch Root und unabhängigen Leser](evidence/business-subtype-live-observed-review-20261104.json)
und [native Saveprüfung](evidence/business-subtype-live-observed-save-readback-20261104.json)
belegen diesen begrenzten Lauf und unveränderte 39 Slot2-/38 Slot1-Dateien.

Der [Reloadbericht vom 04:08:48 UTC](evidence/business-subtype-live-reload-report-20261104.json),
SHA256 `8d180dfb54a6377dad024c5cdb8c07e8b22d7edb7f89033dd2737a365f474bff`,
meldet im selben Prozess `skipped / attempt-already-consumed` ohne neue
Registrierung oder Trial-Snapshots. Der Native-Block behält die Details des
ersten Laufs; er ist kein frischer Readback. CleanupComplete=false im skipped-
Result bedeutet keinen neuen Cleanupfehler.
[Reloadreview](evidence/business-subtype-live-reload-review-20261104.json),
[erneute Saveprüfung](evidence/business-subtype-live-reload-save-readback-20261104.json).
Diese Beobachtungen prüfen weder Interface- noch Managerpfade oder spielbares Business.

## Historischer erster Fehlversuch und Korrektur

Der erste Untertyp-Lauf vom 2026-11-04, 03:18:27 UTC, scheiterte vor jeder
Instanzerzeugung bei der Registrierung: Der ClassInjector fand
`FishNet.Connection.NetworkConnection, FishNet.Runtime` nicht als CLR-Typ.
Zehn Register und 13 fremde VTable-Fingerprints waren vorher/nachher identisch;
Root-, Business- und OwnClass-Pointer blieben null. Das ist keine bestandene
Untertyp-/Dispatch-Abnahme. Teilweise registrierte Metadaten sind damit nicht
ausgeschlossen; der nächste Versuch braucht einen neuen Prozess.
[Originalbericht](evidence/business-subtype-live-failed-20261104.json), SHA256
`0e2c63ef0b811c9491bb07c8f40cba24e540c440fd44cb779184a8287619d27a`.

Die [zurückgezogene Aktivierungsprobe](business-activation-probe.md) bleibt per
`#error` gesperrt. Der erste Untertyp-Kandidat hatte DLL
`659ee6a2fe2a3e12bb8dd8e61c15b48e673c5a54cdcf652167bda4035de96078`, PDB
`9384eb201f31d9ccd12f4c60787b13f539f25d0f83ff3511a889c77e71424777`.

Die am 2026-11-04 vorbereitete, anschließend installierte Korrekturfassung hat DLL
`59484a666e510a1fe48cbd5cec7277d1fa50df434a90433c74ff5d4fa7d1a794` und PDB
`4ae949089b16ad997bb6ff2edb4f21a7f559a5578a49b149c9e082c489aef67f`.
Normal- und Untertypmodus wurden ohne Deployment mit jeweils null Warnungen/
Fehlern gebaut; der Aktivierungsmodus scheitert weiterhin gezielt an CS1029.
Die gezielte Formatprüfung der veränderten Quelle und neuen Tests ist sauber.
Installation und erneuter Spieltest sind getrennte Ergebnisnachweise; der
beobachtete Spieltest steht im Abschnitt oben.

## Begrenzte Korrektur der Namensauflösung

Der installierte Interop-Code setzt beim zweiten CLR-Suchversuch `Il2Cpp` vor
den Typnamen, lässt aber den nativen Assemblynamen `FishNet.Runtime` unverändert.
Die generierte Assembly heißt `Il2CppFishNet.Runtime`. Die korrigierte Probe
übersetzt deshalb ausschließlich diesen vorher validierten NetworkConnection-
Namen während der eigenen Registrierung in die exakte Proxy-Assemblyidentität.

Ein temporärer **verwalteter** Postfix am ClassInjector-Namenshelfer erhält
dessen originale Generic-/ByRef-/RewriteType-Verarbeitung. Ein zweiter
verwalteter Guard begrenzt die Übersetzung auf den eigenen Typ, dessen Thread
und Registrierungstiefe eins; reentrante fremde Registrierungen und andere
Threads behalten das Originalergebnis. Es gibt keinen neuen Hook an einer
nativen Spielmethode, keine AssemblyResolve-Aliasbindung und keinen Austausch
von Abhängigkeiten.

Vor Registrierung: genaue Methodensignaturen und gepinnte Interop-/Harmony-/
FishNet-Assemblies prüfen, NetworkConnection-Namen und native Klassenidentität
validieren, die Parameter aller begrenzt gelesenen Basis-VTable-Slots inventarisieren.
Unbekannte FishNet-Signaturen sperren den Versuch. Eigene RegisterTypeOptions
verwenden keine Interfaces und keine Erfolgs-Logging-Callbacks.

Nach Erfolg oder Fehler wird der Scope zuerst deaktiviert, dann werden genau
die eigenen verwalteten Patches mit höchstens zwei Rückbauversuchen entfernt.
Fremde Patch-Owner, Methoden, Prioritäten und Before-/After-Reihenfolgen müssen
unverändert bleiben. Unbestätigter Patchrückbau beendet den Versuch vor
Instanzerzeugung. Bericht: echte Mapping-Hits, Registrierungstiefe, Scopeende,
Patchbestand und Restored getrennt von Unity-Objektrückbau. Die Registration
selbst wird nie automatisch wiederholt.

## Ziel und Zustandsfolge

Ein eigener IL2CPP-Untertyp soll `ShouldSave` und `CanBePurchased` nativ mit
`false` beantworten, ohne gemeinsam benutzte native Spielmethoden zu patchen.
Die Probe läuft einmal pro Prozess nach bestätigtem Main-Load, sechs Sekunden
Wartezeit, bereiter Halle, Singleplayer ohne Lobby, genau einem Spieler und
übereinstimmenden absoluten Savepfaden für `SaveGame_2` / `TownHallTest`.
Ein aktiver SaveManager-Speichervorgang sperrt den Versuch ebenfalls.

`gated → baseline → register-type → construct-inactive → validate-dispatch →
invoke-two-bools → cleanup → final-readback`. Der Versuch ist vor dem ersten
Snapshot verbraucht. Kontextablehnung berührt den Port nicht. Ein Fehler beendet
den Versuch; eigener Unity-Rückbau und eigener verwalteter Patchrückbau dürfen
jeweils höchstens zweimal versucht werden.
Unbekannter Rückbau bleibt ein Fehler und erlaubt keinen neuen Versuch.

## Grenzen und Akzeptanz

- Explizite ClassInjector-Registrierung erst hinter dem Kontextgate, ohne
  automatisches RegisterType-Attribut. Registrierung ist eine **prozessweite,
  nicht rücknehmbare Metadatenwirkung**, getrennt vom Unity-Objektrückbau.
- Genau ein eigener GameObject, vor AddComponent deaktiviert; die eigene
  Business-Komponente wird nie aktiviert. Kein NetworkObject und keine Kinder. Tatsächliche begrenzte
  Komponenteninventur mit Typ und Pointer; unerwartete Komponenten sind Fehler.
- Eigene no-base Lifecycle-Overrides inklusive Reset/OnValidate. Eigene
  Eligibility-Overrides zählen Aufrufe und liefern false. Keine originale
  Business-Initialisierung, native Serialisierung, Kauf-, Save- oder Spawnaufrufe.
- Vor Aufrufen prüft der native Resolver die eigene deklarierende Klasse und
  eigene MethodPointer. Frische konkrete Property-Basiswrapper führen die
  beiden virtuellen Aufrufe aus; ein bloßer CLR-Cast wäre kein Nachweis.
  Dies prüft den Property-Proxy-/Klassen-Virtualdispatch. ISaveable- oder andere
  Interface-Aufrufe und die tatsächlichen nativen Managerpfade bleiben ungeprüft.
  Bereits vor AddComponent müssen zwölf eigene Lifecycle-/Eligibility-Slots
  anhand der Basis-MethodInfo-Slotnummer auf eigene Trampoline zeigen. Die
  injizierte MethodInfo selbst hat Slot 65535; deren Slotnummer wäre kein Beweis.
- Vorinitialisierte fremde Property-/Business-/NetworkBehaviour-Klassen und
  Klassen der registrierten fremden Instanzen behalten ihre vollständigen
  VTable-Fingerprints. Zehn Property-/Business-/SaveManager-Register bleiben
  pointer- und reihenfolgengleich vor, während und nach der Probe.
  Die Korrektur nimmt zusätzlich die vorinitialisierte NetworkConnection-Klasse
  in den ersten Fingerprint auf; der ältere Fehlbericht mit 13 Klassen ist historisch.
  Der Klassenfingerprint umfasst VTable-Adresse, jeden Slot-MethodPtr,
  MethodInfo-Adresse und deren MethodPointer.
- Kein Aktivieren, Veröffentlichen in Registern oder Aufruf nativer Savepfade.
  Keine neuen Detours an Spielmethoden. ClassInjector-interne Bridge-Infrastruktur
  ist davon zu unterscheiden. Der vorhandene read-only Beobachter bleibt separat.
- Eigene native-null-Prüfung nach DestroyImmediate; Lifecycle-Zähler vor/nach
  Rückbau separat berichten. Nur der eigene no-base OnDestroy-Zähler darf beim
  Rückbau um null oder eins steigen; jede andere Änderung ist ein Fehler.
  RootDestroyed und bestätigter vollständiger Rückbau sind getrennte Zustände.
  Zähler belegen gesichtete eigene Overrides, nicht
  universell das Ausbleiben jedes denkbaren nativen Seiteneffekts.

Implementierung und Tests: Coding-Agent `construction_policy`; unabhängige
statische Prüfung `probe_review` und installierte API-Prüfung
`native_business_contract`. Produktionsdateibudget unverändert 20 einschließlich
Projektdatei; Erweiterung gehört in die bestehende BusinessConstruction-Domäne.

Die Generation und der vollständig erneut gelesene Kontext werden unmittelbar
vor Registrierung, Konstruktion/AddComponent und jedem bool-Aufruf geprüft.
Reentrantes PreLoad/Leaving sperrt die alte Generation. Abgewiesene Kontexte
binden keinen Port und verbrauchen keinen Versuch. Ein fehlgeschlagener Versuch
bleibt prozessweit endgültig; unbekannte eigene Handles werden gehalten.

## Installierter API-Vertrag

Geprüft am 2026-11-03 gegen die installierten Proxys und Il2CppInterop Runtime
`1.5.1-ci.845+f03c8f4ae507d47ea814f3d11d1ec6b0391c1576`, SHA256
`3878ce0ba0c60385f76f2ba37e07ad0ee3649d019703aec8695db3402abeedd7`.
`UnityVersionHandler.Wrap` liest versionsgerecht MethodInfo/VTable, ohne
geratene native Offsets. Die Property-Proxys verwenden für beide bool-Methoden
`il2cpp_object_get_virtual_method` und `il2cpp_runtime_invoke`; der eigene
IntPtr-Konstruktor ruft nur den Basiswrapper auf. In Business/Property/
NetworkBehaviour wurde kein RequireComponent(NetworkObject) gefunden; das
ersetzt ausdrücklich nicht die tatsächliche Komponentenprüfung im Spiel.
Die Native-/FishNet-/S1API-Hashes entsprechen dem Konstruktionsvertrag und werden
zusammen mit dem zusätzlichen Interop-Hash vor dem Scharfschalten geprüft.
Für die verwalteten Patches kommt Harmony `2.10.2` hinzu, SHA256
`6c898933b52149e8bcf6722305c7e4d94add47c9e53b364096f91721624c0877`.
Vor und nach dem Registrierungsscope werden beide bisherigen CLR-Suchnamen und
der tatsächlich installierte `SystemTypeFromIl2CppType` ohne künstlichen Scope
geprüft. Eine schon funktionierende korrekte Fremdauflösung bleibt unberührt;
ein falscher Rückgabetyp sperrt den Versuch.

Offline-Korrekturstand vom 2026-11-04: **352/352** reine TownHall-Tests bestanden,
darunter 34 neue Fälle. Die Regression reproduziert den exakten fehlenden
Assemblyalias, die begrenzte Reparatur und das ursprüngliche Verhalten außerhalb
des Scopes. Getrennte Fälle prüfen vorhandene korrekte Fallbacks, Threads,
Fremd-/Reentranzgrenzen, Teilinstallation, fremde Patchänderungen, fehlende Hits
und maximal zwei eigene Rückbauversuche. Dies ersetzt keinen neuen Spielbericht.

## Build und späterer Spieltest

Spiel geschlossen halten; der neue Build-Schalter ist unabhängig:

```powershell
dotnet build Source/Mods/TownHallInterior/src/TownHallInterior.csproj -c Release -p:S1NoDeploy=true -p:S1TownHallBusinessSubtype=true
```

Vor Installation: normale und Subtype-NoDeploy-Builds, reine Failure-Injection-
Tests, unabhängiges Review und isolierter Pure-Checkout. Danach separat durch
den Hauptagenten hashgeprüfte Installation. Kein Build ist Gameplay-Abnahme.

Tatsächlich geprüft am 2026-11-03 durch `construction_policy`: normaler und
Subtype-NoDeploy-Build jeweils ohne Warnungen/Fehler; Aktivierungsbuild scheitert
weiter gezielt an CS1029. 318/318 reine TownHall-Tests bestanden, einschließlich
45 neuer Fälle zu Gateablehnung, einmaligem/reentrantem Versuch, Teilfehlern,
Null-/Alias-Snapshots, fremden Registry-/VTable-Änderungen, Lifecycle-Sichtungen,
nativen Rückgabe-/Zählerfehlern, Kontextwechsel und begrenztem Rückbau.
Die isolierte Pure-Prüfung folgt nach dem Dokumentationsfreeze als letzter
Repo-Prüfschritt; ihr Ergebnis wird separat im Abschlussbericht festgehalten.

Der oben belegte Live-Lauf erfüllte diese Kriterien: kalter Start mit TownHallTest / Slot 2, Bericht unter
`UserData/TownHallInterior/BusinessSubtype`, Status observed, beide Rückgaben false,
je genau ein eigener Eligibility-Aufruf, bestätigte eigene Native-Dispatch-Ziele,
exakte zwei Komponenten, unveränderte Register und fremde VTables, CleanupComplete
true. Menü-Reload erzeugte keinen zweiten mutierenden Versuch. Native
Save-Dateien wurden gegenüber dem Checkpoint geprüft. Aktivierung, vollständige
Initialisierung, Kauf, Station und Persistenz bleiben spätere Arbeit.
