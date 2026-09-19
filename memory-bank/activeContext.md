# Active Context — Schedule1-mod-only

**Stand:** 2026-09-19
**Branch:** `main` (synchron mit `origin/main`)

**Dateien in dieser Memory Bank:**

| Datei | Zweck |
|---|---|
| `activeContext.md` (diese) | Aktueller Arbeitsstand, letzte Session, Pitfalls, Umgebung |
| `productContext.md` | Repo-Zweck, Struktur, Mod-Inventar-Snapshot, Constraints |
| `decisionLog.md` | Getroffene Entscheidungen mit Begründung (neueste oben) |

> **Versions-Wahrheit bleibt `AGENTS.md`** — die Snapshots hier sind Session-Orientierung, keine Autorität.

---

## Letzte Session (2026-09-19)

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

---

## Umgebung (lokal, Stand 2026-09-19)

| Tool | Version | Installationsweg |
|---|---|---|
| .NET SDKs | 6.0.428, 8.0.425, 10.0.401 | System |
| S1Interop | 0.1.0-alpha.1 | `dotnet tool install --global S1Interop --version 0.1.0-alpha.1` (Quelle: GitHub `ifBars/S1Interop`, GPL-3.0) |
| Schedule I | v0.4.6f13 | `C:\Program Files (x86)\Steam\steamapps\common\Schedule I` — IL2CPP-Referenzen ready, Mono-Zweig fehlt (für diesen Workspace irrelevant) |

---

## Offene Punkte

| Thema | Details |
|---|---|
| **s1interop in CONTRIBUTING.md verankern** | CI-Job ist advisory; ob s1interop zusätzlich als dokumentiertes **lokales** Gate in CONTRIBUTING.md "IL2CPP Pflichten" gehört, ist offen (Abwägung: Alpha-Tool mit bekannten False Positives vs. Nutzen als Frühwarnung) |
| **Memory Bank** | ✅ etabliert: `activeContext.md`, `productContext.md`, `decisionLog.md`. Pflege-Regel: Session-Ende aktualisiert `activeContext.md`; Entscheidungen wandern nach `decisionLog.md`. |

## Verifikations-Stand (AGENTS.md)

Alle 11 aktiven Mods + Shared auf `Verified`-Stand. Zuletzt verifiziert 2026-09-19:
- AutoPackagingStation v0.2.8 (Bugfix-Runde 7)
- PocketShop v0.2.7 (Level-Lock & Inline-Qty)
- BusinessIncome v0.1.6 (0-Business Backlog-Fix)
- StackLimitMod v0.1.4 (Agriculture-Only & Weapon-Shield)

## Entscheidungen

- Release-Zips werden **nur lokal** bereitgestellt, kein Nexus-Upload (Dominik, 2026-09-19).
