# Active Context — Schedule1-mod-only

**State:** 2026-09-20 (afternoon)
**Branch:** `main` — base HEAD: `8945236 docs: Verified-Status 2026-09-20 + _DiagPerfCounter archived`. Working copy after this commit is **clean** (5 files cleaned up: `ci.yml`, `AGENTS.md`, `CONTRIBUTING.md`, `activeContext.md`, `decisionLog.md`).

**Files in this memory bank:**

| File | Purpose |
|---|---|
| `activeContext.md` (this one) | Current work state, last session, pitfalls, environment |
| `productContext.md` | Repo purpose, structure, mod inventory snapshot, constraints |
| `decisionLog.md` | Decisions made with rationale (newest on top) |

> **Version truth remains `AGENTS.md`** — the snapshots here are session orientation, not authority.

---
## Last session (2026-09-23/24, Hermes — S1API 3.2.1-beta.5, HitmanPhone Beta done)

**Outcome:** HitmanPhone Beta test is COMPLETE (offer -> accept -> receipt -> payout, slot JSON + .bak, 0 warnings). Workspace committed clean.

- **S1API upgraded beta.2 -> 3.2.1-beta.5** (2026-09-23 22:01, prebuilt zip from ifBars/S1API releases, sha256-verified; backups `S1API.Il2Cpp.MelonLoader.dll.20260923.bak`). Root cause of the "no framework data object" error was upstream ifBars/S1API **#309** (fix `90a43df`, shipped in beta.3); beta.5 also carries the NPC postload crash fix (#317), f6 consume-event tolerance (#311), gamepad hang fix (#316). `hitman_unknown_caller` verified live via S1MCP `list_npcs` (107 NPCs).
- **Offer now hosts on the real "Unknown Number" caller thread** (no DEBUG tier-1 fallback needed — `SendOffer[DEBUG]: caller wrapper missing` never fired). Full chain verified in-game 2026-09-24 05:27: `Accepted: id=hitman-c21f60ddc15 reward=$323 deadline=5` -> receipt -> payout -> Journal Completed -> `bounties_slot_1.json` + `.bak` written, ghost cooldown persisted.
- **HitmanPhone commits:** `50bb1bb` fix(hitman): v0.2.9 (0.4.7f6 NPCDeathPatch via GetComponent<S1NPC>, DEBUG host fallback tier1/tier2, slash aliases, force-offer bypass, DE->EN docs, CHANGELOG filled) + `5d7ea3b` refactor(hitman): review follow-ups (DEBUG fallback wrapped in try/catch, SendOfferExpiredNotice deliberately fallback-less with rationale, Dominik's original German quote restored). Review-subagent verdict after 3 doc fixes: gates green (Debug+Release build, format, version-sync, doc-paths).
- **Repo cleanup (this session):** the long-standing 95-file DE->EN translation wave committed (Source/ + Skills/ + root docs + memory-bank), stale Release zips repacked (StackLimitMod v0.1.6, PocketShop v0.3.2, AutoPackagingStation v0.3.3; old zips deleted, gitignored), Archivierungsfrage decided: plan files & beta artifacts are KEPT.
- **Open (backlog):** S1API submodule still on v3.2.0+4 without the #309 fix and 2 UNCOMMITTED console files (`CustomConsoleRegistry.cs`, `ConsolePatches.cs`) — move to origin/beta + merge locals before the next S1API source build. Optional review leftovers: try/catch around OnAccept/OnMoreInfo DEBUG NPC.Get (only SendOffer got one). Abo downgrade check ~2026-10-05 (MiMo Lite vs Standard after 1-2 weeks).
- **Pitfalls (carry-over):** Release build overwrites the debug DLL in GameDir (never `-c Release` without `-p:S1NoDeploy=true`); `Il2CppSystem.Collections.List` needs `extern alias il2cpp`; console key `^` (VK 220/0xDC, dead key: `^`+Space -> Backspace); S1MCP bridge :8765 speaks FLAT JSON-RPC method names (`list_npcs`, not `tools/call`) and gets flaky (resets/timeouts) after a handful of calls — handshake first, don't rely on it for critical sequences.
- Detailplan (archived, kept): `C:\Users\pc\.hermes\plans\2026-09-22_125640-hitman-sms-offer-host-fallback.md`.

---

## Last session (2026-09-22, Hermes — HitmanPhone Beta test, plan mode until 14:07)

**Topic:** `/hitman_force_offer` produced no SMS on beta 0.4.7f6. **State 14:07: green up to SMS send**, accept test + commit still open.

- **Root cause:** S1API 3.2.1-beta.2 could not instantiate `HitmanCallerNPC` (upstream #309, "no framework data object" in `NPCDataAccess.InitializeCurrentDataForConstruction`) -> no caller wrapper in `NPC.All` -> `SendOffer` aborted. Bonus finding: `NPC.All` contains base-game wrappers only **lazy** (materialized by `S1API.Entities.NPC.Get(id)`), which is why the first fallback scan failed.
- **Implemented (committed this session as 50bb1bb/5d7ea3b):** DEBUG-only fallback chain in `BountyConversationRouter` — tier 1 `NPC.Get(target.ID)`, tier 2 native `MSGConversation.SendMessage`+`ShowResponses`, `DescribeWrapperIds()` diagnostics, fallbacks in `OnMoreInfo`/`OnAccept`. Release path untouched. Debug DLL deployed (DEBUG required by `#if DEBUG` in `Mod.cs:107-109`).
- **Accepted cosmetic (decision):** SMS ran on the TARGET thread ("Unknown Number" was broken by beta.2). DEBUG-only, disappeared once beta.5 landed. Not reworked further.
- **Done since:** accept click verified, journal + `bounties_slot_1.json` verified, upstream report to Bars unnecessary (#309 already fixed by ifBars), Archivierungsfrage -> KEEP.
- Detailplan: `C:\Users\pc\.hermes\plans\2026-09-22_125640-hitman-sms-offer-host-fallback.md`.

---

**Thema:** `/hitman_force_offer` bringt auf Beta 0.4.7f6 keine SMS. **Stand 14:07: grün bis SMS-Versand**, Accept-Test + Commit offen.

- **Root Cause:** S1API 3.2.1-beta.2 (Issue #305) kann `HitmanCallerNPC` nicht instanziieren ("no framework data object", `NPCDataAccess.InitializeCurrentDataForConstruction`) -> kein Caller-Wrapper in `NPC.All` -> `SendOffer` bricht ab. Zusatz-Fund: `NPC.All` enthaelt Basisgame-NPCs nur **lazy** (nach `S1API.Entities.NPC.Get(id)`), deshalb scheiterte der erste Fallback-Scan.
- **Implementiert (uncommitted!)** in `Source/Mods/HitmanPhone/src/BountyConversationRouter.cs`: DEBUG-only Fallback-Kette — Tier 1 `NPC.Get(target.ID)` (funktioniert, 14:07 per Log bewiesen), Tier 2 nativer `MSGConversation.SendMessage`+`ShowResponses` (nie noetig gewesen), `DescribeWrapperIds()`-Diagnose, Fallback auch in `OnMoreInfo`/`OnAccept`. Release-Pfad unveraendert. Debug-DLL deployed (DEBUG noetig wegen `#if DEBUG` in `Mod.cs:107-109`).
- **Akzeptierte Kosmetik (Entscheidung):** SMS laeuft im **Target-Thread** statt "Unbekannte Nummer" (Test-Default-Target `ludwig_meyer` -> wirkt wie Selbstmord-Auftrag). DEBUG-only, verschwindet mit upstream #305. Nicht weiter gebastelt.
- **Offen:** More-info/Accept-Klick -> Journal-Quest + `UserData/HitmanPhone/bounties_slot_1.json` verifizieren; `git commit` (nur die eine Datei!); Archivierungsfrage HitmanPhone fuer Beta entscheiden (Dominik); Upstream-Report #305 an Bars mit Stacktrace 13:57:48.
- **Pitfalls:** Release-Build ueberschreibt die Debug-DLL im GameDir (nie `-c Release` ohne `-p:S1NoDeploy=true`); `Il2CppSystem.Collections.List` braucht `extern alias il2cpp`; Konsolen-Tippfehler bleiben in der deployten beta.2-DLL unsichtbar (kein Unrouted-Logging) — `hitman_status` als Kanarienvogel nutzen. Konsolen-Key `^` (VK 220/0xDC, Dead-Key: `^`+Space -> Backspace).
- Detailplan: `C:\Users\pc\.hermes\plans\2026-09-22_125640-hitman-sms-offer-host-fallback.md`.

---

## Last session (2026-09-20, Cline Desktop — Dominik)

1. **Open points clarified:** Memory bank was stale — since 20.09 morning 4 commits came in:
   - `284bb0b` feat(StackLimitMod): v0.1.6 — IL2CPP TryCast fix + diagnostics expansion
   - `8ec6433` feat(PocketShop): v0.3.2 — vanilla payment type per shop + cash HUD feedback
   - `bd64287` feat(AutoPackagingStation): v0.3.3 — E-prompt fix + auto-UNPACK (vanilla delegation)
   - `8945236` docs: Verified-Status 2026-09-20 + _DiagPerfCounter archived
2. **In-game verify completed (real-game session 2026-09-20, 13:42):** StackLimitMod v0.1.6 ✅, PocketShop v0.3.2 ✅, AutoPackagingStation v0.3.3 ✅. Evidence: `apply_report.json` 2026-09-20T11:42:59Z (42 items modified, ogkush/sourdiesel/meth/cocaine correctly, cash/mushroomhat protected via weapon guard), Latest.log 13:42:59, user feedback "functional without errors".
3. **s1interop CI job removed:** On GitHub-hosted runners (`windows-latest`) Schedule I is never installed → analyze step was permanently skipped, the job was a no-op that only burned CI minutes. Removed from `.github/workflows/ci.yml` (left a hint comment there), AGENTS.md §6 CI line synced. **s1interop remains local pre-flight** (AGENTS.md §6, CONTRIBUTING.md) — advisory, always exit 0, never a gate.

---



## Session of 2026-09-19 (12:42–14:49 — Hermes session `20260919_124204_512782`)

Title "Increase stack sizes in mod", 543 messages (17 user / 265 assistant / 261 tool), reconstructed from `%LOCALAPPDATA%\hermes\state.db`. **Nothing from this session has been committed.**

### Results

| Mod | Version | Content | Status |
|---|---|---|---|
| **StackLimitMod** | 0.1.4 → 0.1.5 → 0.1.6 | v0.1.5 = timing fix (`OnLoadComplete` dedupe swallowed the registry scan) + `apply_report.json` + `stack check` / `stack report` / `LogDecisions`; v0.1.6 = **IL2CPP TryCast fix** | deployed (13:54), **in-game verify open** |
| **PocketShop** | 0.2.7 → 0.3.0 → 0.3.1 | v0.3.0 = blanket card-only (per user hint, was **wrong**); v0.3.1 = **vanilla `ShopInterface.PaymentType` per shop**, new console `pshop shops` | deployed (14:48), **in-game verify open** |
| **Shared** | — | newly deployed (build dependency) | ok |

### The actual bug behind "packaged products don't stack" (v0.1.6)

`is` type checks do not work on **IL2CPP proxy objects** — the managed wrapper is always of the declared type (`BaseItemDefinition`). Consequence per `apply_report.json`: `ogkush`/`sourdiesel`/`greencrack`/`granddaddypurple` (`WeedDefinition`), `meth`, `cocaine` were classified as `not-agriculture` and never raised; `cash` (`CashDefinition`) ran through unprotected, `mushroomhat` (`ClothingDefinition`) was falsely stacked. All 23 "modified" items matched only the **ID keyword heuristic** (`soil`, `baggie`, `jar`, `defaultweed` → "weed"). Fix: `TryCast<T>()` (same pattern as AutoPackagingStation). The proof came from the report, not the log — the v0.1.5 diagnostic tools proved their purpose.

Incidentally verified: packaging is **not its own item type** (instance field `ProductItemInstance.PackagingID`), and there is **no** `get_StackLimit` override in `ItemInstance`/`QualityItemInstance`/`ProductItemInstance` — the postfix on `BaseItemInstance` covers packaged products.

### Environment/system events (laptop session)

- **MelonLoader zombie:** After game quit `Schedule I.exe` (console host) stayed in uninterruptible kernel wait — not killable (Task Manager, `taskkill`, `Stop-Process`), DLL locks remained. Workaround: **rename trick** (`<Mod>.dll` → `<Mod>.dll.zombie`, copy new DLL) — deploy ran anyway. Currently no `.zombie` residue in game dir (checked 2026-09-20).
- **Crash/hard reset:** Bugcheck `0x12B` / `c00002c4` (ZEROED_PAGE_CORRUPTION) — cause was Fast Startup hibernate, which wrote the zombie into the hiberfile. `powercfg /h off` set. Memtest clean, WHEA 0 events, 13.75 of 16 GB usable (iGPU shared) → **no** hardware defect.
- **Hardware (laptop):** ACEMAGIC RX16 (not MEDION), Ryzen 7 7735HS, 16 GB DDR5-4800, Radeon iGPU 2 GB shared.

### v0.4.7 Open Beta — impact analysis (exists only here, in no repo file)

Recommendation: stay on default **v0.4.6f13**. Risks on default switch: Avatar refactor (`AvatarObjects`, `NakedAppearance` + `Outfit` replace `AvatarSettings`) → PocketShop + CustomSkateboard (**high**, compile break); NPC ragdoll refactor → HitmanPhone (**high**); walk-speed refactor → BusinessIncome/PocketShop (medium); skateboard input fix → CustomSkateboard (medium); mixed product icons → PotScanner/AutoPackagingStation (low); laundering capacities → BankApp (value drift). Appearance save conversion is **one-way** → beta only with save backup; beta test = `dotnet build -c Release` as API drift detector.

---

## Previous session (2026-09-19, morning — push, releases, s1interop)

### Done

1. **Push:** 2 local commits pushed to `origin/main`
   - `dc5022d` docs(repo): 5-star polish — THIRD-PARTY-NOTICES.md, assets/ screenshot conventions, test/license badges, s1interop CI activation checklist
   - `1d40500` fix(AutoPackagingStation): v0.2.8 — 2x2 footprint, InteractableObject for E-key, Hustler-I rank gate

2. **Release packaging (local only, no Nexus upload):** 4 zips in `Release/`
   - `AutoPackagingStation-v0.2.8.zip` (59.2 KB)
   - `PocketShop-v0.2.7.zip` (87.1 KB)
   - `BusinessIncome-v0.1.6.zip` (37.8 KB)
   - `StackLimitMod-v0.1.4.zip` (27.2 KB)
   - Deploy convention verified in all zips (DLL/PNG → `Mods/`, mod.json + pdb → `UserData/<Mod>/`, docs in root)
   - Version sync checked before run: 11 mods, 0 drift

3. **s1interop CI activation** (`0b0c52d`) — checklist from `dc5022d` worked off:
   - `S1Interop` 0.1.0-alpha.1 (GitHub `ifBars/S1Interop`) installed locally as dotnet global tool
   - CI job in `.github/workflows/ci.yml` activated: detection probe duplicated, `has-game` gate, analysis across all 12 mod csproj
   - **Advisory by design:** `analyze` always returns exit 0 (even with findings) — the job can never make the pipeline red
   - AGENTS.md §6 CI line synced

### s1interop analysis — findings (2026-09-19)

| Mod | Finding | Assessment |
|---|---|---|
| BusinessIncome | `ManagedCollectionSignatureInterop` (`IncomeEngine.cs:31`, medium) | **False positive** — mod-internal calculation API with mod's own `BusinessRevenueLine`, no game callback across the Il2Cpp boundary |
| HitmanPhone | `DirectMemberReflectionLookup` ×2, `FieldPropertyReflectionFallback` ×1 (low/medium) | **Accepted** — `S1Quest` is internal in S1API; code defensive (try-catch + fallback), documented as "best-effort, cosmetic" |
| All projects | `wrong_target_framework`, `global_usings_require_langversion` | **Analyzer limitation** — TFM (`net6.0`) + `LangVersion=12` are in `Directory.Build.props`, which 0.1.0-alpha.1 doesn't evaluate (only reads the csproj) |
| AutoPackagingStation, CalculatorApp, NotesApp, PocketShop | IntPtr ctor + derived body detected | ✅ clean, 0 risks |

**Consequence: no code changes** — the code is in better shape than the alpha analyzer can represent.

### Learned / pitfall

- Do not run `package-release.ps1` **in parallel** for multiple mods — file locks on shared `Shared` build outputs cause builds to fail. Either serially per mod or `-Mod All` in one run (internal serial).
- `s1interop analyze` always returns **exit 0** — unsuitable as a blocking quality gate, only usable as a report.
- `s1interop doctor` without a path argument runs in the current directory and reports `[missing] project`; if needed `s1interop doctor <csproj|dir>`.
- PowerShell: `cd` does not change the .NET working directory — `[System.IO.File]::ReadAllBytes` needs absolute paths.
- **`Tools/*.ps1` start with `#Requires -Version 7`** — in Windows PowerShell 5.1 (default shell) they break with "cannot be run because it contained a #requires statement". Always call via `pwsh -NoProfile -File Tools/<script>.ps1`.
- **`dotnet format` takes exactly one project per call** — multiple csproj arguments end in "Unrecognized command or argument".
- **Hermes sessions live in `%LOCALAPPDATA%\hermes\state.db`** (SQLite, tables `sessions`/`messages`). The session ID only exists as `sessions.id` (filename search is useless); read-only copy + `sqlite3` suffices. `messages.tool_calls` only readable via `json_extract(tool_calls,'$[0].function.name')`, otherwise the raw dump is unusable.
- For local markdown paths in new doc text watch `Tools/check-doc-paths.ps1` — non-existent repo paths cause the gate to fail.

---

## Environment (local, state 2026-09-19)

| Tool | Version | Install path |
|---|---|---|
| .NET SDKs | 6.0.428, 8.0.425, 10.0.401 | System |
| S1Interop | 0.1.0-alpha.1 | `dotnet tool install --global S1Interop --version 0.1.0-alpha.1` (source: GitHub `ifBars/S1Interop`, GPL-3.0) |
| Schedule I | v0.4.6f13 | `C:\Program Files (x86)\Steam\steamapps\common\Schedule I` — IL2CPP references ready, Mono branch missing (irrelevant for this workspace) |
| Laptop (secondary) | ACEMAGIC RX16, Ryzen 7 7735HS, 16 GB DDR5-4800 | Hibernation disabled (`powercfg /h off`) after `0x12B` bugcheck from MelonLoader zombie |
| Hermes Desktop | Provider `zai`, model `glm-5.3-flash` | Session DB `%LOCALAPPDATA%\hermes\state.db` — source for session reconstruction |

---

## Open points

| Topic | Details |
|---|---|
| **Uncommitted files** | `.github/workflows/ci.yml` (s1interop job removed), `AGENTS.md` (§6 CI line sync), `memory-bank/activeContext.md` (this catch-up). Commits only on request (decisionLog "permanent"). |
| ~~WIP not committed~~ | ✅ done 2026-09-20 — 19er-WIP committed: `284bb0b` (StackLimitMod v0.1.6), `8ec6433` (PocketShop v0.3.2), `bd64287` (AutoPackagingStation v0.3.3), `8945236` (docs/Verified-Status). |
| ~~In-game verify 0.1.6 + 0.3.1~~ | ✅ done 2026-09-20, 13:42 — real-game session: StackLimitMod v0.1.6 ✅, PocketShop v0.3.2 ✅, AutoPackagingStation v0.3.3 ✅. Evidence: `apply_report.json` 2026-09-20T11:42:59Z, Latest.log, user feedback. |
| ~~AGENTS matrix vs. reality~~ | ✅ done 2026-09-20 — feature verify dated, build verify shown, hint block above the matrix. |
| **v0.4.7 Open Beta** | Impact analysis only lives in this memory bank. Beta switch only after save backup + drift pass. Optionally planned (not created): deploy guard script that checks game version before deploy. |
| ~~Anchor s1interop in CONTRIBUTING.md~~ | ✅ done 2026-09-19 — install command, `doctor` hint, report-not-gate warning and known false positives in `CONTRIBUTING.md` "IL2CPP Obligations" + DoD checkbox corrected; `AGENTS.md §6` pre-flight synced |
| ~~CI verify s1interop job~~ | ✅ obsolete 2026-09-20 — job removed: never runnable on GitHub-hosted runners (no game installed → analyze permanently skipped). s1interop remains local pre-flight (AGENTS.md §6). |
| **Memory bank** | ✅ established: `activeContext.md`, `productContext.md`, `decisionLog.md`. Maintenance rule: end of session updates `activeContext.md`; decisions move to `decisionLog.md`. |

## Verification state (AGENTS.md)

All 11 active mods + Shared at `Verified` state. Last verified in the real-game session 2026-09-20, 13:42:
- StackLimitMod v0.1.6 (IL2CPP TryCast fix, 42 items correctly classified)
- PocketShop v0.3.2 (vanilla payment type per shop, cash HUD feedback)
- AutoPackagingStation v0.3.3 (E-prompt, Unpackage-mode mirror, 10-unit unpack batching)

## Decisions

- Release zips are provided **only locally**, no Nexus upload (Dominik, 2026-09-19).