# Active Context — Schedule1-mod-only

**Stand:** 2026-09-20
**Branch:** `main` (synchron mit `origin/main`; **Arbeitskopie dirty** — 18 modifizierte + 1 neue Datei aus der Session vom 19.09., nicht committet)

**Dateien in dieser Memory Bank:**

| Datei | Zweck |
|---|---|
| `activeContext.md` (diese) | Aktueller Arbeitsstand, letzte Session, Pitfalls, Umgebung |
| `productContext.md` | Repo-Zweck, Struktur, Mod-Inventar-Snapshot, Constraints |
| `decisionLog.md` | Getroffene Entscheidungen mit Begründung (neueste oben) |

> **Versions-Wahrheit bleibt `AGENTS.md`** — die Snapshots hier sind Session-Orientierung, keine Autorität.

---

## Letzte Session (2026-09-19, 12:42–14:49 — Hermes-Session `20260919_124204_512782`)

Titel „Stackgrößen in Mod erhöhen", 543 Messages (17 user / 265 assistant / 261 tool), rekonstruiert aus `%LOCALAPPDATA%\hermes\state.db`. **Aus dieser Session ist nichts committet.**

### Ergebnisse

| Mod | Version | Inhalt | Status |
|---|---|---|---|
| **StackLimitMod** | 0.1.4 → 0.1.5 → 0.1.6 | v0.1.5 = Timing-Fix (`OnLoadComplete`-Dedupe verschluckte den Registry-Scan) + `apply_report.json` + `stack check` / `stack report` / `LogDecisions`; v0.1.6 = **IL2CPP-TryCast-Fix** | deployt (13:54), **In-Game-Verify offen** |
| **PocketShop** | 0.2.7 → 0.3.0 → 0.3.1 | v0.3.0 = pauschal Card-only (auf User-Hinweis, war **falsch**); v0.3.1 = **Vanilla-`ShopInterface.PaymentType` pro Shop**, neue Konsole `pshop shops` | deployt (14:48), **In-Game-Verify offen** |
| **Shared** | — | neu deployt (Build-Abhängigkeit) | ok |

### Der eigentliche Bug hinter „verpackte Produkte stapeln nicht" (v0.1.6)

`is`-Typ-Checks funktionieren auf **IL2CPP-Proxy-Objekten nicht** — der Managed-Wrapper ist immer vom deklarierten Typ (`BaseItemDefinition`). Folge laut `apply_report.json`: `ogkush`/`sourdiesel`/`greencrack`/`granddaddypurple` (`WeedDefinition`), `meth`, `cocaine` wurden als `not-agriculture` klassifiziert und nie angehoben; `cash` (`CashDefinition`) lief ungeschützt durch, `mushroomhat` (`ClothingDefinition`) wurde fälschlich gestackt. Alle 23 „modified"-Items matchten nur die **ID-Keyword-Heuristik** (`soil`, `baggie`, `jar`, `defaultweed` → „weed"). Fix: `TryCast<T>()` (identisches Pattern wie AutoPackagingStation). Der Nachweis kam aus dem Report, nicht aus dem Log — die v0.1.5-Diagnosewerkzeuge haben ihren Zweck bewiesen.

Nebenbei verifiziert: Verpackung ist **kein eigener Item-Typ** (Instanz-Feld `ProductItemInstance.PackagingID`), und es gibt **kein** `get_StackLimit`-Override in `ItemInstance`/`QualityItemInstance`/`ProductItemInstance` — der Postfix auf `BaseItemInstance` deckt verpackte Produkte ab.

### Umgebungs-/Systemereignisse (Laptop-Session)

- **MelonLoader-Zombie:** Nach Spiel-Quit blieb `Schedule I.exe` (Konsolen-Host) in ununterbrechbarem Kernel-Wait — nicht killbar (Taskmanager, `taskkill`, `Stop-Process`), DLL-Locks blieben bestehen. Workaround: **Rename-Trick** (`<Mod>.dll` → `<Mod>.dll.zombie`, neue DLL kopieren) — Deploy lief trotzdem. Aktuell keine `.zombie`-Reste im Game-Dir (geprüft 2026-09-20).
- **Crash/Hard-Reset:** Bugcheck `0x12B` / `c00002c4` (ZEROED_PAGE_CORRUPTION) — Ursache war Fast-Startup-Hibernate, der den Zombie ins Hiberfile schrieb. `powercfg /h off` gesetzt. Memtest clean, WHEA 0 Events, 13,75 von 16 GB nutzbar (iGPU-Shared) → **kein** Hardwaredefekt.
- **Hardware (Laptop):** ACEMAGIC RX16 (nicht MEDION), Ryzen 7 7735HS, 16 GB DDR5-4800, Radeon iGPU 2 GB Shared.

### v0.4.7 Open Beta — Impact-Analyse (existiert nur hier, in keiner Repo-Datei)

Empfehlung: auf Default **v0.4.6f13** bleiben. Risiken bei Default-Wechsel: Avatar-Refactor (`AvatarObjects`, `NakedAppearance` + `Outfit` ersetzen `AvatarSettings`) → PocketShop + CustomSkateboard (**hoch**, Compile-Bruch); NPC-Ragdoll-Refactor → HitmanPhone (**hoch**); Walk-Speed-Refactor → BusinessIncome/PocketShop (mittel); Skateboard-Input-Fix → CustomSkateboard (mittel); Mixed-Product-Icons → PotScanner/AutoPackagingStation (niedrig); Laundering-Kapazitäten → BankApp (Werte-Drift). Appearance-Save-Konvertierung ist **one-way** → Beta nur mit Save-Backup; Beta-Test = `dotnet build -c Release` als API-Drift-Detektor.

---

## Session davor (2026-09-19, vormittags — Push, Releases, s1interop)

### Erledigt

1. **Push:** 2 lokale Commits auf `origin/main` gepusht
   - `dc5022d` docs(repo): 5-Star-Polish — THIRD-PARTY-NOTICES.md, assets/ Screenshot-Konventionen, Test/License-Badges, s1interop CI-Aktivierungs-Checkliste
   - `1d40500` fix(AutoPackagingStation): v0.2.8 — 2x2-Footprint, InteractableObject für E-Taste, Hustler-I-Rank-Gate

2. **Release-Packaging (nur lokal, kein Nexus-Upload):** 4 Zips in `Release/`
   - `AutoPackagingStation-v0.2.8.zip` (59,2 KB)
   - `PocketShop-v0.2.7.zip` (87,1 KB)
   - `BusinessIncome-v0.1.6.zip` (37,8 KB)
   - `StackLimitMod-v0.1.4.zip` (27,2 KB)
   - Deploy-Konvention in allen Zips verifiziert (DLL/PNG → `Mods/`, mod.json + pdb → `UserData/<Mod>/`, Docs im Root)
   - Version-Sync vor Lauf geprüft: 11 Mods, 0 Drift

3. **s1interop-CI-Aktivierung** (`0b0c52d`) — Checkliste aus `dc5022d` abgearbeitet:
   - `S1Interop` 0.1.0-alpha.1 (GitHub `ifBars/S1Interop`) lokal als dotnet Global Tool installiert
   - CI-Job in `.github/workflows/ci.yml` aktiviert: Detection-Probe dupliziert, `has-game`-Gate, Analyse über alle 12 Mod-csproj
   - **Advisory by design:** `analyze` liefert immer Exit 0 (auch bei Findings) — der Job kann die Pipeline nie rot machen
   - AGENTS.md §6 CI-Zeile synchronisiert

### s1interop-Analyse — Findings (2026-09-19)

| Mod | Finding | Bewertung |
|---|---|---|
| BusinessIncome | `ManagedCollectionSignatureInterop` (`IncomeEngine.cs:31`, medium) | **False Positive** — mod-interne Berechnungs-API mit mod-eigenem `BusinessRevenueLine`, kein Game-Callback über die Il2Cpp-Grenze |
| HitmanPhone | `DirectMemberReflectionLookup` ×2, `FieldPropertyReflectionFallback` ×1 (low/medium) | **Akzeptiert** — `S1Quest` ist internal in S1API; Code defensiv (try-catch + Fallback), als "best-effort, cosmetic" dokumentiert |
| Alle Projekte | `wrong_target_framework`, `global_usings_require_langversion` | **Analyzer-Limitation** — TFM (`net6.0`) + `LangVersion=12` stehen in `Directory.Build.props`, die 0.1.0-alpha.1 nicht auswertet (liest nur die csproj) |
| AutoPackagingStation, CalculatorApp, NotesApp, PocketShop | IntPtr-Ctor + derived-body erkannt | ✅ sauber, 0 Risks |

**Konsequenz: keine Code-Änderungen** — der Code ist in besserem Zustand als der Alpha-Analyzer darstellen kann.

### Gelernt / Pitfall

- `package-release.ps1` **nicht parallel** für mehrere Mods ausführen — File-Locks auf geteilten `Shared`-Build-Outputs lassen Builds fehlschlagen. Entweder seriell pro Mod oder `-Mod All` in einem Lauf (intern seriell).
- `s1interop analyze` gibt **immer Exit 0** — als blockierendes Quality-Gate ungeeignet, nur als Report nutzbar.
- `s1interop doctor` ohne Pfad-Argument läuft im aktuellen Verzeichnis und meldet `[missing] project`; bei Bedarf `s1interop doctor <csproj|dir>`.
- PowerShell: `cd` ändert nicht das .NET-Arbeitsverzeichnis — `[System.IO.File]::ReadAllBytes` braucht absolute Pfade.
- **`Tools/*.ps1` beginnen mit `#Requires -Version 7`** — in Windows PowerShell 5.1 (Default-Shell) brechen sie mit „cannot be run because it contained a #requires statement" ab. Immer `pwsh -NoProfile -File Tools/<script>.ps1` aufrufen.
- **`dotnet format` nimmt genau ein Projekt pro Aufruf** — mehrere csproj-Argumente enden in „Unrecognized command or argument".
- **Hermes-Sessions liegen in `%LOCALAPPDATA%\hermes\state.db`** (SQLite, Tabellen `sessions`/`messages`). Die Session-ID existiert nur als `sessions.id` (Dateiname-Suche bringt nichts); read-only Kopie + `sqlite3` genügt. `messages.tool_calls` nur über `json_extract(tool_calls,'$[0].function.name')` lesbar machen, sonst ist der Rohdump unbrauchbar.
- Bei lokalen Markdown-Pfaden in neuen Doku-Texten auf `Tools/check-doc-paths.ps1` achten — nicht existierende Repo-Pfade lassen den Gate fehlschlagen.

---

## Umgebung (lokal, Stand 2026-09-19)

| Tool | Version | Installationsweg |
|---|---|---|
| .NET SDKs | 6.0.428, 8.0.425, 10.0.401 | System |
| S1Interop | 0.1.0-alpha.1 | `dotnet tool install --global S1Interop --version 0.1.0-alpha.1` (Quelle: GitHub `ifBars/S1Interop`, GPL-3.0) |
| Schedule I | v0.4.6f13 | `C:\Program Files (x86)\Steam\steamapps\common\Schedule I` — IL2CPP-Referenzen ready, Mono-Zweig fehlt (für diesen Workspace irrelevant) |
| Laptop (sekundär) | ACEMAGIC RX16, Ryzen 7 7735HS, 16 GB DDR5-4800 | Hibernation deaktiviert (`powercfg /h off`) nach `0x12B`-Bugcheck durch MelonLoader-Zombie |
| Hermes Desktop | Provider `zai`, Modell `glm-5.3-flash` | Session-DB `%LOCALAPPDATA%\hermes\state.db` — Quelle für Session-Rekonstruktion |

---

## Offene Punkte

| Thema | Details |
|---|---|
| **WIP nicht committet** | 18 modifizierte Dateien (StackLimitMod 0.1.5/0.1.6, PocketShop 0.3.0/0.3.1, `AGENTS.md`, `README.md`, Skill-Referenz) + untracked `Source/Mods/PocketShop/src/PocketShopCommand.cs`. Letzter Commit bleibt `6a06cc4`. Commits nur auf Zuruf (decisionLog „dauerhaft"). |
| **In-Game-Verify 0.1.6 + 0.3.1** | Offen. Beleg: `apply_report.json` = `2026-09-19T11:25:53Z` mit `LastRegistryCount: 0` (= v0.1.5-Fehllauf), DLL-Deploy 13:54/14:48 — seither keine Spiel-Session. Testbefehle: `stack check <itemId>`, `stack stats`, `stack report`, `pshop shops`. |
| **AGENTS-Matrix vs. Realität** | Matrix führte „Verified 2026-09-19" für v0.1.6/v0.3.1, obwohl die CHANGELOGs „In-Game-Verify offen" sagen → am 2026-09-20 korrigiert (Feature-Verify datiert, Build-Verify offen ausgewiesen) + Hinweisblock über der Matrix. |
| **v0.4.7 Open Beta** | Impact-Analyse steht nur in dieser Memory Bank. Beta-Umstieg erst nach Save-Backup + Drift-Pass. Optional geplant (nicht angelegt): Deploy-Guard-Skript, das die Spielversion vor dem Deploy prüft. |
| ~~s1interop in CONTRIBUTING.md verankern~~ | ✅ erledigt 2026-09-19 — Installationsbefehl, `doctor`-Hinweis, Report-statt-Gate-Warnung und bekannte False Positives in `CONTRIBUTING.md` „IL2CPP Pflichten" + DoD-Checkbox korrigiert; `AGENTS.md §6` Pre-Flight synchron |
| **CI-Verifikation s1interop-Job** | Hosted-Runner: Install + Gate laufen (`success`), Analyze-Step wird ohne Spiel-Assemblies korrekt geskippt — Analyze-Pfad bisher nur lokal verifiziert |
| **Memory Bank** | ✅ etabliert: `activeContext.md`, `productContext.md`, `decisionLog.md`. Pflege-Regel: Session-Ende aktualisiert `activeContext.md`; Entscheidungen wandern nach `decisionLog.md`. |

## Verifikations-Stand (AGENTS.md)

Alle 11 aktiven Mods + Shared auf `Verified`-Stand. Zuletzt verifiziert 2026-09-19:
- AutoPackagingStation v0.2.8 (Bugfix-Runde 7)
- PocketShop v0.2.7 (Level-Lock & Inline-Qty)
- BusinessIncome v0.1.6 (0-Business Backlog-Fix)
- StackLimitMod v0.1.4 (Agriculture-Only & Weapon-Shield)

## Entscheidungen

- Release-Zips werden **nur lokal** bereitgestellt, kein Nexus-Upload (Dominik, 2026-09-19).
