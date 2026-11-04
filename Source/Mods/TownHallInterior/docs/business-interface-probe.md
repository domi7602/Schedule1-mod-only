# TownHall: inaktive ISaveable-Interfaceprobe

Stand 2026-11-04. Die bestehende [Untertyp-/Property-Probe](business-subtype-probe.md)
ist mit DLL `59484a66…` im Spiel beobachtet. Die Interfaceprobe `cb98bc95…`
lief um 04:53:58 UTC auf TownHallTest / Slot 2 und scheiterte vor dem
Interface-Aufruf, ohne den abgelehnten Einzelwert zu speichern. Der Folge-Kandidat `853a1178…` speicherte den Einzelwert. Die Flaggen-Korrektur
`a8d45f2a…` bestand am 2026-11-04 um 06:58:38 UTC den inaktiven Interface-Aufruf.
Der Menü-Reload um 07:00:49 UTC bestätigte im selben Prozess die Einmalversuchsperre.

Die Probe prüft ausschließlich den nativen Interface-Dispatch von
`ISaveable.InitializeSaveable()` zum bereits vorhandenen eigenen **no-base**
Override. Sie initialisiert kein natives Business. `ISaveable` besitzt kein
`ShouldSave`; die vorher geprüfte Property-Methode ist kein Interface-Nachweis.
Der Getter `ShouldSaveUnderFolder` ist ein anderer Vertrag und bleibt unaufgerufen.

## Ablauf und Grenzen

Der separate Schalter `S1TownHallBusinessInterface` erweitert die inaktive
Untertypprobe um genau einen Aufruf nach den beiden Property-Aufrufen.
`RegisterTypeOptions.Interfaces` bleibt leer: Es werden keine zusätzlichen
oder doppelten Interfaceeinträge injiziert. Der ursprüngliche Untertypmodus
erwartet weiterhin null `InitializeSaveable`-Callbacks.

`Slot-/Kontextgate → Baseline → eigene Typregistrierung → inaktive Komponente →
Property-Dispatch → Interface-Vorprüfung → ein Interface-Aufruf → Readback →
eigener Rückbau → Endvergleich`.

Die bisherigen Gates bleiben erhalten: abgeschlossener Main-Load, bereite Halle,
Singleplayer ohne Lobby, genau ein Spieler, keine laufende Speicherung, gleiche
absolute aktive/geladene Pfade `SaveGame_2` und Organisation exakt `TownHallTest`.
Die Generation wird vor nativen Schritten erneut geprüft. Ablehnung berührt
den Port nicht. Ein zugelassener Versuch wird pro Prozess nur einmal verbraucht,
auch bei Fehlern oder teilweise registrierten Metadaten.

Vor dem Interface-Resolver und dem Aufruf müssen alle folgenden Nachweise passen:

- Vorinitialisierte native `ISaveable`-Klasse; exakte native Methode per Token
  `0x06001782`, gleicher Namefinder-Pointer und gleicher privater MethodInfo-Pointer
  des tatsächlich benutzten generierten Proxywrappers.
- Genau ein Eintrag in der begrenzten Methods-Liste mit 23 Methoden: Owner
  `ISaveable`, Name `InitializeSaveable`, Instanzmethode, virtuell und abstrakt,
  keine Parameter, Rückgabe `System.Void`. Der Interface-Slot ist gültig und
  wird gelesen, nicht aus dem Property-Klassenslot abgeleitet.
- Je genau ein passendes Interface-Offsetpaar in Business und eigenem Untertyp,
  gleiche nichtnegative Offsets und unveränderte geerbte ImplementedInterfaces.
  `offset + interfaceSlot` liegt in beiden konkreten VTables. Eine direkte
  Mitgliedschaft in Business.ImplementedInterfaces wird wegen Vererbung nicht verlangt.
- Das konkrete Ziel ist exakt die schon gebundene eigene InitializeSaveable-
  MethodInfo samt MethodPointer und eigener deklarierender Klasse. Das bekannte
  Property-Basisziel hat Token `0x06001420`; sein Methodenrumpf bleibt unaufgerufen.
- Nach diesem Metadatengate bestätigt `il2cpp_class_is_assignable_from`
  die Interfacezugehörigkeit für Business und eigenen Untertyp. Erst danach
  wird `il2cpp_object_get_virtual_method` aufgerufen und muss dasselbe eigene
  Ziel liefern. Ein frischer konkreter `ISaveable(ownPointer)`-Proxy ruft es auf.
- Vorher genau null, nachher genau ein eigener `InitializeSaveable`-Callback;
  `ShouldSave`/`CanBePurchased` bleiben jeweils genau eins. Andere Lifecycle-
  Zähler müssen null bleiben. Frühere, doppelte oder fehlende Aufrufe scheitern.
  Ziel, Objektzustand und Kontext werden nach dem Aufruf erneut geprüft.

Zehn Register und fremde native Metadaten werden vor und nach jeder Stufe
verglichen. Der erweiterte Fingerprint enthält VTable-Einträge/MethodPointer,
ImplementedInterfaces, InterfaceOffsets und Methodentabellen; ISaveable wird
vor der ersten Baseline vorinitialisiert. Fremder Zustand wird niemals zurückgerollt.

Ein Teilfehler wird als `failed` protokolliert, auch wenn eigener Rückbau danach
gelingt. Nur eigener Unity- und eigener verwalteter Patchrückbau sind auf jeweils
höchstens zwei Versuche begrenzt. Beim Unity-Rückbau darf ausschließlich der eigene
no-base OnDestroy-Zähler um null oder eins steigen; alle anderen Zähler bleiben
gleich. Root- und Component-Abwesenheit müssen bestätigt sein. Typregistrierung
bleibt eine nicht rücknehmbare prozessweite Metadatenwirkung.

Keine Aktivierung, Originalinitialisierung, Savewrites, Serialisierung, Käufe,
Spawns, Manageraufrufe oder Publikation in Property-/Business-/SaveManager-Registern.
Keine neuen nativen Spielmethoden-Detours; die bestehende eng begrenzte verwaltete
Typauflösung bleibt unverändert. Die alte Aktivierungsprobe bleibt compile-blockiert.
Ein Erfolg heißt **nativer Interface-Dispatch beobachtet**, nicht Save-Eignung,
Business-Initialisierung oder Managerintegration. Native Managerpfade bleiben
auf den statisch gelesenen Signaturbestand begrenzt.

## Belege und Prüfstand

Der [installierte API-Vertrag](evidence/business-interface-native-contract.json)
wurde durch `native_business_contract` aus den hashgepinnten installierten
Proxys/Interop-Structs geprüft. Reine Tests prüfen die Zustandsmaschine und
abgewiesene Metadaten, ersetzen aber keine tatsächlichen nativen Pointerreadbacks.
`construction_policy` implementiert; `probe_review` prüft unabhängig.

Vorbereiteter Kandidat vom 2026-11-04:

- Vorheriger Live-Kandidat DLL SHA256 `cb98bc95c7efcf947eaa268bbde742bffb467401fed6afb526e7cc26406ef207`.
- Folge-Kandidat DLL SHA256 `853a11784af61807190479a71860bc56c8806d07067ca733cbc2c92c8f34a220`,
  PDB `c0dbf3626a16ac55a05cab71c306dc17267ff09860fd115a50b8890428897c3c`.
  Installiert nach 411/411 Tests und Interface-/Normal-Build ohne Warnungen.
  Isoliertes Pure: 20 bestanden, bekannte Format-/Versionsfehler, keine neuen
  Formatbefunde in den geänderten Dateien. Live-Lauf am 2026-11-04 um 05:24:22 UTC:
  `failed` (`interface-resolved-flags`); Aufruf unterblieb.
  [Bericht](evidence/business-interface-live-flags-20261104.json).
- PDB SHA256 `51b90c4ce1cae26827e97c434a87e65fcd4725578e7d64015df8d9f948eed625`.
- **411/411** TownHall-Tests bestanden, davon 59 neue Fälle: falsche/null
  Methoden, Slots/Offsetgrenzen, eigene/fremde Zielklassen, Signaturen,
  Assignability, frühe/fehlende/doppelte Callbacks, Pointerwechsel,
  fremde Snapshotänderungen, Kontextverlust, Teilfehler, begrenzter Rückbau
  und unveränderte Einmaligkeit sowie bisheriger Untertypmodus.
- Normal-, Untertyp- und Interfacemodus ohne Deployment gebaut: jeweils
  null Warnungen/Fehler. Aktivierung endet weiterhin gezielt an `CS1029`.
- Gezielte Formatprüfung der veränderten Domain und neuen Testdatei bestanden.
  Unabhängiger Artefaktreview und isoliertes Pure werden im Abschlussbeleg
  getrennt dokumentiert. Produktionsdateibudget bleibt 20 inklusive csproj.

## Fehlgeschlagener Live-Lauf 2026-11-04, 04:53:58 UTC

DLL `cb98bc95…`, Prozess seit 04:53:06 UTC, exakt TownHallTest / SaveGame_2.
Bericht `failed` mit einzigem Befund
`interface-preflight:InvalidOperationException:interface-target-or-counter-invalid`.
`Interface.Before` ist null und `InvocationAttempted` false: die Vorprüfung warf
innerhalb von `InspectInterface`, bevor ein Readback gespeichert oder
`ISaveable.InitializeSaveable` aufgerufen wurde. Registrierung, beide
Property-Aufrufe (false, je einmal) und eigener Rückbau sind bestätigt. Zehn
Register und 15 fremde VTables blieben über alle fünf Stufen gleich. Native
Dateien: 39/39 Slot 2 und 38/38 Slot 1 unverändert.
[Bericht](evidence/business-interface-live-failed-report-20261104.json),
[Auswertung](evidence/business-interface-live-failed-review-20261104.json).
Ein Menü-Reload kann diesen Versuch nicht wiederholen.

Der Folge-Lauf von `853a1178…` um 05:24:22 UTC speichert den Einzelwert:
`interface-resolved-flags`. Die Interface-Methode selbst passt (Token
`0x06001782`, virtuell und abstrakt, Slot 10, Offset 19). Die eigene injizierte
Methode zeigt auf denselben Pointer und dieselbe Klasse, ist aber nicht als
virtuell markiert: Slot 65535, Token 0, nicht abstrakt, nicht statisch. Das ist
die installierte ClassInjector-Form, kein falsches Ziel. Der Aufruf unterblieb.
Kein Interface-Dispatch, keine Aktivierung und keine Persistenz nachgewiesen.
[Bericht](evidence/business-interface-live-flags-20261104.json).

Die Flaggenkorrektur `a8d45f2a…` bestand am 2026-11-04 um 06:58:38 UTC den inaktiven
Interface-Aufruf auf TownHallTest / SaveGame_2: `observed` ohne Findings;
`InitializeSaveableCalls` 0→1, `ShouldSave`/`CanBePurchased` je 1, zehn Register und
15 fremde VTables über alle Stufen unverändert, eigener Rückbau bestätigt.
[Bericht](evidence/business-interface-live-observed-20261104.json). Der Menü-Reload
um 07:00:49 UTC im selben Prozess (19672) lieferte `skipped /
attempt-already-consumed` ohne neue Registrierung; `NativeEvidenceFromThisAttempt`
false, `Result.CleanupComplete` false ist der Skip-Standard.
[Bericht](evidence/business-interface-live-reload-20261104.json).

Ein separater inaktiver Modus `inactive-saveable-registration` (`473064e0…`) lief am
2026-11-04 um 07:11:05 UTC auf Slot 1 (Organisation Dominik / SaveGame_1): `observed`
ohne Befunde; `SaveManager.RegisterSaveable` genau einmal aufgerufen, Eintrag sofort
wieder aus der Speicherliste entfernt (Before 404, Added 1, Remaining 0);
`Save()`/`WriteData`/Manager-Initialisierung unaufgerufen; zehn Register, 14 fremde
VTables und native Dateien unverändert; `WorldPublicationAllowed` und
`NativeSaveWriteAllowed` false; eigener Rückbau bestätigt. Das ist eine umkehrbare
Speicherlisten-Registrierung, kein Save-/Load-Nachweis und keine Aktivierung.
[Bericht](evidence/business-manager-live-register-saveable-20261104.json).

## Build und späterer Live-Test

Spiel geschlossen halten:

```powershell
dotnet build Source/Mods/TownHallInterior/src/TownHallInterior.csproj -c Release -p:S1NoDeploy=true -p:S1TownHallBusinessInterface=true -p:S1TownHallBusinessSubtype=false -p:S1TownHallBusinessConstruction=false -p:S1TownHallBusinessActivation=false
```

Erst nach getrennt dokumentierter Installation kalt starten, TownHallTest / Slot2
laden und die automatische Probe abwarten. Neuer Bericht liegt unter
`UserData/TownHallInterior/BusinessInterface`, Mode
`inactive-own-business-interface-dispatch`, Schema2. Eingetreten am 2026-11-04: der Bericht um 06:58:38 UTC meldet `observed` ohne
Findings, `InitializeSaveableCalls` 0→1, unveränderte Register/Metadaten und
bestätigten eigenen Rückbau; der Menü-Reload um 07:00:49 UTC meldete
`skipped / attempt-already-consumed`. Ein Bericht liegt unter
`UserData/TownHallInterior/BusinessInterface` (Mode
`inactive-own-business-interface-dispatch`, Schema2), Kopien hashidentisch in
`docs/evidence/`. Für `inactive-saveable-registration` liegt der Bericht unter
`UserData/TownHallInterior/BusinessManager`, Kopie in
`docs/evidence/business-manager-live-register-saveable-20261104.json`.

`NativeEvidenceFromThisAttempt=false` kennzeichnet
die behaltenen Details des ersten Versuchs; Mapping-Hits und Cleanupdaten
sind dann historisch. `Result.CleanupComplete=false` bei skipped ist kein
neuer Cleanupfehler. Native Saves werden separat gegen den Checkpoint geprüft.
