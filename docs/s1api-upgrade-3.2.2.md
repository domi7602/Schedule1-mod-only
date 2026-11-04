# S1API 3.2.2 — lokales Upgrade vom 2026-11-02

## Ergebnis und Umfang

Offizielle Release-Artefakte bei geschlossenem Spiel installiert; noch **nicht im Spiel geladen**.
Nur diese beiden Dateien wurden ersetzt:

| Ziel relativ zum Spielordner | Vorher SHA-256 | Installiert SHA-256 |
|---|---|---|
| `Mods/S1API.Il2Cpp.MelonLoader.dll` | `85b218ff0ac3d18ec03e2a959f13ce13c596d38d46ab78f7908ec9fa7533c6e3` | `d751a3cb66003f7a616eb9f8054daf34da0d9d7f23416a5005a586738a680852` |
| `Plugins/S1APILoader.MelonLoader.dll` | `d58b1d3ceefa8863d627ed395c55d824faad8559e3614b19b1b5ebc92d271f60` | `b05187bd64be49acf5c5ac8f67a21a2f6605712afbc2d626c8ba55ccd89ac576` |

`ThirdParty/S1API` steht sauber auf Tag `v3.2.2`, Commit
`6b6d7bada39054822732224508cf0f4f45ee7c26`, zuvor
`a4cbdd9e0ec54895e64fd6be0e91874ba6649c9b` (`v3.2.1`). Die Änderung des Submodulstands
im übergeordneten Repository ist uncommittet; kein Commit/Push wurde ausgeführt.
Die IL2CPP-Assembly meldet `S1API, Version=3.2.2.0` und ProductVersion
`3.2.2+6b6d7bada39054822732224508cf0f4f45ee7c26`.

[Offizieller Release](https://github.com/ifBars/S1API/releases/tag/v3.2.2), veröffentlicht
2026-10-30. Das ZIP `S1API-Forked-3.2.2.zip` wurde gegen den GitHub-Asset-Digest
`ea81f1eae46f6a13a5228f3cec891e8e9529bde22db647aa33f3bb698a7c10b2`
geprüft; extrahierte DLLs stimmen mit den ZIP-Einträgen überein. Keine signierte
Tagprüfung und kein reproduzierbarer Eigenbuild der Release-DLLs behauptet.

## Tatsächlich ausgeführte Prüfungen

- Gesamte `Source/Mods/S1Mods.sln` mit SDK 8.0.426 gegen die konkrete Release-IL2CPP-DLL
  gebaut: **0 Warnungen / 0 Fehler**. Globales `ModsDir` auf die entpackten Kandidaten
  gesetzt, `S1NoDeploy=true`, `TaxiShortcutGuard=true`. `ResolveReferences` für Shared
  bestätigt Kandidatenpfad und AssemblyVersion 3.2.2.0.
- `dotnet test` auf der gebauten Solution ohne Neubuild: **4.157/4.157 bestanden**, 19
  TRX-Suiten, 0 fehlgeschlagen/übersprungen. Separater LightControl-Konsolenhost:
  **265/265 bestanden**. TaxiDriver darin **1.402/1.402**.
- Unabhängige API-/Artefaktprüfung: Microsoft ApiCompat 8.0.426 auf die **vorher wirklich
  installierte** IL2CPP-Assembly gegen den Release. Standardregeln sowie zusätzlich
  Parameternamen/Attribute ohne Diagnosen, ohne Suppressions. Negative Gegenrichtung
  erkennt 5 CP0001, 15 CP0002, 1 CP0003. Das Tool lieferte auch dort Exit 0; deshalb
  wurden beide Ausgabekanäle inhaltlich ausgewertet. Keine CI-Parität zur dortigen
  ApiCompat-Version 10.0.302 behauptet.
- Loader-Quellvergleich unverändert; konkrete alte/neue Loader dekompilieren gleich,
  alle **28 Methodenkörper-IL-Hashes** und Assemblyreferenzen stimmen überein. Der
  Loader meldet weiterhin MelonInfo 2.5.0; seine AssemblyVersion ist 1.0.0.0, nicht 3.2.2.
- Mono-DLL bewusst nicht installiert: `S1APILoader/S1APILoader.cs`, `NormalizeBuild`,
  behandelt fehlende Builddateien als No-op. Diese Installation bleibt IL2CPP.
- Vor Installationsschreibzugriffen Prozessprüfung und gemeinsamer Installationsmutex;
  beide Originaldateien gesichert und per Hash geprüft, anschließend beide Ziele und
  Backup-Dateien nochmals unabhängig gelesen. Kein modübergreifendes Deployment.
- Vergleich mit Vorhermanifest: Von **30 erfassten installierten DLL-/Taxi-Metadatendateien**
  änderten sich genau die beiden S1API-Ziele. TaxiDriver bleibt
  `9d897a865992506e87656b21876702668b43be91cb2e579e6d3327231c072d3a`;
  Shared, S1MAPI und übrige erfasste Moddateien unverändert.

Reproduktion der Kandidatenprüfung: globale Eigenschaft `ModsDir` auf den entpackten
Release-Ordner `Mods` setzen (keine Änderung an zentralen Build-Props nötig):

```powershell
dotnet build Source/Mods/S1Mods.sln -c Release -p:S1NoDeploy=true -p:TaxiShortcutGuard=true -p:ModsDir=<Release-Mods-Ordner>
dotnet test Source/Mods/S1Mods.sln -c Release --no-build --no-restore -p:S1NoDeploy=true --logger trx --results-directory <Ergebnisordner>
dotnet run --project Source/Tests/LightControl.Tests/LightControl.Tests.csproj -c Release --no-build --no-restore
```

## Rückweg und Nachweisablage

Deployment-Versuch: `20261102T143320833Z-S1API-3.2.2`, installiert am
`2026-11-02T14:33Z`. Relativ zum Spielordner:

- Quittung: `UserData/.deploy/receipts/20261102T143320833Z-S1API-3.2.2.json`.
- Geprüfte Originale: `UserData/.deploy/backups/20261102T143320833Z-S1API-3.2.2/`,
  darin dieselben relativen `Mods/`- und `Plugins/`-Pfade wie oben.

Bei einem begründeten Rückbau zuerst Spiel schließen, beide Backupdateien gegen die
Vorherhashes prüfen, beide Ziele wiederherstellen und nachprüfen. Passenden bisherigen
Submodulstand separat wiederherstellen; fremde Arbeitsänderungen erhalten. Nicht
vorsorglich zurückbauen und weder Spielstände noch Konfigurationen ersetzen.

Dauerhafte lokale Zusatzbelege liegen im verbundenen Vault, Projektkennung
`schedule1-mod-only`, unter `raw/attachments/s1api-3.2.2-upgrade-2026-11-02/`:
Release-Metadaten, Vorhermanifest, TRX-Zähler und Buildlog, Reviewbericht/-ausgaben,
Deployment-Quittung und Hashmanifest. Zuständigkeit bleibt dieses Repository;
[Vault-Anbindung](knowledge-vault.md).

## Grenzen und separate bestehende Fehler

- Kein Spielstart, kein Ingame-/Save-/Multiplayer-Test. Alte Gameplay-Abnahmen
  behalten Version und Zeitpunkt; sie sind kein Nachweis für diese neue Abhängigkeit.
  Als nächster Eigentümercheck genügen Start/Laden, Handy und eine normale Modaktion;
  neue API-Funktionen sind dadurch nicht pauschal geprüft.
- Upstream-S1API-Vertragssuiten nicht lokal wiederholt: deren Projekt zielt auf
  net9.0; installiert ist SDK 8.0.426. Die obigen Resultate sind unsere Modtests,
  nicht die upstream Mono-/IL2CPP-Vertragssuiten.
- Lokales Quick vor den Dokumentationsänderungen: **5 bestanden / 2 fehlgeschlagen**,
  Formatierung und vorhandener Versionsdrift (HitmanPhone/Minimap/PoliceTestMode).
  Nicht im Upgrade repariert und kein vollständig grünes Repository behauptet.
- Isolierter Arbeitssnapshot `f69959bb5af3`, Digest
  `14fc56d54ba4095f913a06387a1d1f7ff13524711a53739b10f7196c19ed5bf5`:
  erster Pure-Lauf nach 300 Sekunden abgebrochen, kein vollständiger Prüfbericht.
  Anschließend denselben unveränderten Checkout mit kürzerem Ergebnis-/Fixturepfad
  erneut geprüft: `verify-c5d90039d685`, **19 bestanden / 3 fehlgeschlagen**
  (`format`, `versions`, `tool-workspace-setup.Tests`). Dokumentation, Skills,
  ausgewählte Modtests und die Clean-Checkout-Tooltests bestanden. Der verbleibende
  Workspace-Setup-Test scheitert im synthetischen Git/Submodul-Fixture, nicht im
  echten S1API-Checkout. Kein globales Pass. Alle erfassten Originaldateien passten
  nach dem Lauf noch zum Snapshot; nur diese Ergebnisnotiz wurde danach ergänzt.
- Beim scratch-lokalen ApiCompat-Toolsetup meldete dotnet unerwartet ein installiertes
  ASP.NET-Core-HTTPS-Entwicklungszertifikat. Keine Vertrauensfreigabe ausgeführt;
  der Reviewer hatte den Speicher nicht untersucht. Die spätere rein lesende
  Elternprüfung in `Cert:/CurrentUser/My` fand nur ein so benanntes Zertifikat mit
  NotBefore 2026-10-25, kein neu datiertes. Mangels Vorherinventar kein absoluter
  Unverändertheitsnachweis; nichts bereinigt. Spätere Toolaufrufe unterdrückten die
  Zertifikaterzeugung.
