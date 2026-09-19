# Decision Log — Schedule1-mod-only

Chronologisches Protokoll getroffener Entscheidungen mit Begründung. **Neueste oben.**
Diese Datei ergänzt `AGENTS.md` (operatives Inventar) um das *Warum*.

---

## 2026-09-19 — Release-Zips nur lokal, kein Nexus-Upload

**Entscheidung:** Release-Packaging (`Tools/package-release.ps1`) dient ausschließlich der lokalen Bereitstellung. Kein Upload zu NexusMods.
**Begründung:** Nutzer-Entscheidung (Dominik). Mods bleiben privat/lokal.
**Konsequenz:** `Release/` ist gitignored; kein Publishing-Schritt in Tools oder CI.

---

## 2026-09-19 — s1interop-CI-Job aktiviert, aber advisory

**Entscheidung:** Der `s1interop`-Job in `.github/workflows/ci.yml` ist aktiv, aber **niemals blockierend**. Er installiert `S1Interop` 0.1.0-alpha.1 (dotnet Global Tool, Quelle `ifBars/S1Interop`) und analysiert alle Mod-csproj, gated auf vorhandene Spiel-Assemblies.
**Begründung:**
- `s1interop analyze` liefert **immer Exit 0** — selbst bei gemeldeten Fehlern. Ein blockierendes Gate ist mit diesem Toolstand technisch nicht umsetzbar.
- Alpha-Tool mit bekannten False Positives (`wrong_target_framework`, `global_usings_require_langversion`, weil `Directory.Build.props` nicht ausgewertet wird).
- Nutzen bleibt: Frühwarnung bei echten IL2CPP-Fehlern (z. B. fehlende `IntPtr`-Ctors) direkt im CI-Log.

**Verworfene Alternative:** Job deaktiviert lassen, bis s1interop stabiler ist — verworfen, weil der Report auch im Alpha-Stadium Nutzen bringt und kein Risiko für die Pipeline besteht.
**Bewertete, aber nicht behobene Findings:** `ManagedCollectionSignatureInterop` in BusinessIncome (mod-interne API, kein Game-Callback → False Positive); Reflection in HitmanPhone (`S1Quest` ist internal in S1API; defensiv mit try-catch + Fallback → akzeptiert). **Keine Code-Änderungen.**

**Ergänzung (gleicher Tag):** s1interop zusätzlich als **lokales, optionales** Pre-Flight in `CONTRIBUTING.md` („IL2CPP Pflichten") verankert — mit Installationsbefehl, `doctor`-Hinweis und expliziter Warnung, dass `analyze` ein *Report* und kein Gate ist. Die DoD-Checkbox „`analyze` clean" war sachlich falsch (Exit-Code ist immer 0) und wurde auf „Report gelesen, keine unerwarteten Einträge" korrigiert. Damit kein Beitragender ein grünes Exit-Signal als Freigabe fehlinterpretiert.

---

## 2026-09 (Reinstall) — Deploy-Konvention: `Mods\` vs. `UserData\<Mod>\`

**Entscheidung:** `Directory.Build.targets` deployt getrennt: DLL + PNGs + Bundles → `<GameDir>\Mods\`; `mod.json` + `<Mod>.pdb` → `<GameDir>\UserData\<Mod>\`. JSON/PDB **niemals** nach `Mods\`.
**Begründung:** MelonLoader lädt aus `Mods\` — alles andere dort erzeugt unnötige Ladeversuche/Log-Rauschen. Metadaten und Symbole gehören in den UserData-Bereich.
**Konsequenz:** `Tools/package-release.ps1` spiegelt exakt diese Struktur in jedem Zip.

---

## 2026-09 — Repo liegt außerhalb des Spielverzeichnisses

**Entscheidung:** Workspace unter `C:\Users\pc\Schedule1-mod-only`, nicht im Spielordner.
**Begründung:** Dritter Umzug (Historie: `Desktop\Schedule1-mod-only` → `<GameDir>\...` → User-Profile). Ein Repo im Spielverzeichnis überlebt keine Spiel-Neuinstallation/Steam-Reparatur.
**Konsequenz:** Spielpfad wird zur Build-Zeit über `$env:SCHEDULE1_PATH` (bzw. Default-Fallback) aufgelöst — muss **vor** MSBuild-Start gesetzt sein, da die Property einmalig evaluiert wird.

---

## 2026-09-16 — Doku-Pfad-Guard (`check-doc-paths.ps1`)

**Entscheidung:** Alle in Markdown referenzierten Repo-Pfade müssen existieren; Prüfung läuft in CI **und** Pre-Commit.
**Begründung:** Repo-Audit 2026-09-16 fand unbemerkt gedriftete Pfade (`Knowledge/`, Archive-Pfade, `.agents/`).
**Konsequenz:** Doku-Pfade sind die zweite bewachte Drift-Klasse neben Versionen. Gitignored-aber-dokumentierte Pfade (z. B. `GameReferences/decompiled`) sind whitelisted.

---

## 2026-09-14 — Version-Sync-Guard: Code ist Single Source of Truth

**Entscheidung:** `Tools/check-version-sync.ps1` vergleicht die Code-Version (MelonInfo / `Constants.ModVersion`) mit `docs/mod.json`, `README.md`, AGENTS.md-Matrix-Zeile und Detail-Header. Exit 1 bei Drift; läuft in CI + Pre-Commit. Bumps nur über `Tools/bump-version.ps1`.
**Begründung:** Fünf Orte tragen dieselbe Versionsnummer — manuelles Synchronhalten driftet garantiert.
**Konsequenz:** Manuelle Versions-Edits sind ein Anti-Pattern; `bump-version.ps1` schreibt alle Orte atomar.

---

## 2026-09-14 — Deterministische Solution-Generierung

**Entscheidung:** `Tools/gen-sln.ps1` vergibt GUIDs deterministisch (`Get-DeterministicGuid "Project:<rel>"`) statt zufällig.
**Begründung:** Jede Regeneration erzeugte vorher zufällige GUID-Diffs, die die CI-Determinismus-Prüfung sinnlos machten.
**Konsequenz:** CI vergleicht zeilenenden-normalisiert (LF/CRLF-unabhängig) — es zählt der Inhalt.

---

## 2026-09 — IL2CPP primär, Mono als Alt-Branch

**Entscheidung:** Primäres Ziel ist IL2CPP (Default Steam Branch). Mono ist dokumentierter Alternativpfad, nicht aktiv gepflegt.
**Begründung:** Default-Branch des Spiels ist IL2CPP; doppelte Runtime-Pflege kostet mehr, als sie in diesem Workspace einbringt.
**Konsequenz:** IL2CPP-Disziplin ist verbindlich (IntPtr-Ctor, EventHelper/ButtonUtils, WasCollected-Guards). `s1interop doctor` meldet den Mono-Zweig als `[missing]` — erwartet und irrelevant.

---

## Dauerhaft / ohne Datum

| Entscheidung | Begründung |
|---|---|
| **Neue Patterns gehören in `Shared`, nicht als Mod-Duplikat** | `docs/architecture.md` definiert die Dependency Direction; Duplikate driften und erzeugen N-fache Bugfix-Kosten. |
| **`Shared` und `_DiagPerfCounter` kommen nicht in Releases** | Keine eigenständigen Spieler-Mods; `package-release.ps1` schließt sie explizit aus. |
| **`SkipUnchangedFiles=false`** (2026-08-20) | Jeder Build force-deployt — verhindert Stale-DLL-Fallen beim Testen. |
| **`ThirdParty/` ist read-only** | Gepinnte Dependencies (S1API 3.2.0, S1MCP); Modifikation würde die Reproduzierbarkeit brechen. |
| **Archivierte Mods werden nicht gebaut** | Kein Wartungsaufwand, keine Fehl-Signale in CI. |
| **Keine Commits/Pushes ohne explizite Aufforderung** | Nutzer behält Kontrolle über Historie und Remote. |
| **Sideload & hash nicht mehr deployt** (2026-09-10) | Deprecated/ungenutzt; Sideload war in keinem Mod eingebunden, hash nur Referenz. |


---

## 2026-09-17 — Archivierungs-Politik

**Entscheidung:** Mods auf Nutzerwunsch archivieren statt löschen: `Source/Archive/<Mod>/`, DLLs nach `Mods\_archived\`.
**Begründung:** Code bleibt als Referenz und für spätere Reaktivierung erhalten, ohne Build-/Wartungslast.
**Konsequenz:** `Source/Archive/` wird **nie** gebaut oder deployt. Betroffen: HomelessMod, SnackVendor (2026-09-17), Minimap (Nutzer spielt lieber ohne), DayCounter, ProfitTracker, TVBrowser, BackpackMod.

---

## 2026-09-16 — Status-Konvention `Verified <Datum>` vs. `In-Game-Verify offen`

**Entscheidung:** Zwei getrennte Stände werden bewusst geführt. `Verified` = Funktionsnachweis in einer Echtspiel-Session, belegt über Fix-/Audit-Commit + CHANGELOG. `In-Game-Verify offen` = Code vollständig, aber unbestätigt.
**Begründung:** Ehrlichkeit über vorgetäuschter Perfektion; kein Greenwashing von Build-Erfolgen als Funktionsnachweis.
**Konsequenz:** Kein Artefakt im Repo nötig — die Assertion wird über Commit-Historie und CHANGELOG nachvollziehbar gemacht.
