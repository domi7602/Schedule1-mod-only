# Decision Log — Schedule1-mod-only

Chronological protocol of decisions made with rationale. **Newest on top.**
This file complements `AGENTS.md` (operational inventory) with the *why*.

---

## 2026-09-20 — s1interop CI job removed, local pre-flight remains

**Decision:** The `s1interop` job was removed from `.github/workflows/ci.yml` (one day after its activation on 2026-09-19). Local pre-flight (`s1interop analyze <csproj>`, see AGENTS.md §6 + CONTRIBUTING.md) remains unchanged.
**Rationale:** On GitHub-hosted runners (`windows-latest`) Schedule I is never installed — the `has-game` gate skipped the analyze step permanently, the job was a no-op that only burned CI minutes. Advisory nature (always exit 0) additionally makes it unsuitable as a gate.
**Consequence:** No analyze job in CI. With a later self-hosted runner with game installed the job can be restored from history (`git show 8945236:.github/workflows/ci.yml`).

---

## 2026-09-19 — IL2CPP type checks only via `TryCast<T>()`

**Decision:** In all mods IL2CPP type queries are made via `TryCast<T>()` (real Il2Cpp class hierarchy) — **no** `is`/`as` on interop objects.
**Rationale:** `is` checks the **managed wrapper**, not the Il2Cpp class. Proof in `apply_report.json` (2026-09-19): `ogkush`/`sourdiesel`/`greencrack`/`granddaddypurple` (`WeedDefinition`), `meth`, `cocaine` ran as "not-agriculture" and were never raised; `cash` (`CashDefinition`) ran through unprotected, `mushroomhat` (`ClothingDefinition`) was falsely stacked. The error was invisible as long as ID keyword heuristics happened to match.
**Consequence:** `StackLimitMod v0.1.6` fixes `IsAgricultureItem`/`IsWeaponOrAmmo`; the pattern should live near `docs/pitfalls.md` (AutoPackagingStation already uses it identically).

---

## 2026-09-19 — PocketShop pays per vanilla `ShopInterface.PaymentType`, not by shop name

**Decision:** The payment method is read at runtime from `ShopInterface.PaymentType` (`EPaymentType`: `Cash` / `Online` / `PreferCash` / `PreferOnline`), cached per shop/item in `ShopCatalog.Refresh` and used in afford check, payment, and all refund paths.
**Rationale:** Schedule I is not blanket card-based — black-market suppliers demand cash, legal shops card. `PaymentType` is a **serialized inspector field** in the Unity scene data: there is no code/metadata table, so any external "Shop X = Cash" list (wiki, AI answer) is reconstruction and potentially wrong. Additionally: mod-injected shops inherit their rule automatically.
**Discarded alternative:** Hardcoded shop name list (breaks on every content update and on mod-injected shops).
**Consequence:** `PocketShop v0.3.0` (blanket card-only) is thus superseded as a wrong assumption; diagnostic command `pshop shops` was added to verify the assignment live against any external sources.

---

## 2026-09-19 — Hibernation off + "restart" instead of "shut down" on hanging MelonLoader console

**Decision:** `powercfg /h off` is set on the laptop; on a hanging MelonLoader console we **restart**, not shut down.
**Rationale:** An IL2CPP process can stay in an uninterruptible kernel wait after game quit (not killable via Task Manager, `taskkill`, or `Stop-Process`; DLL locks stay active). Windows 11 "Shut down" with Fast Startup **hibernates** the kernel including this zombie → bugcheck `0x12B` / `c00002c4` (ZEROED_PAGE_CORRUPTION) on next boot. Memtest clean, WHEA 0 events → no hardware defect.
**Consequence (workaround, still valid):** Swap locked mod DLLs via rename trick (`<Mod>.dll` → `<Mod>.dll.zombie`, copy new DLL) — deploy works without reboot; clean up `.zombie` residue after reboot.

---

## 2026-09-19 — Schedule I v0.4.7 Open Beta: stay on default v0.4.6f13

**Decision:** No beta switch, no preemptive migration pass. Beta only for testing **after** save backup and with `dotnet build -c Release` as API drift detector.
**Rationale:** v0.4.7 is open beta (opt-in). Expected breaks on default switch: avatar refactor (`AvatarObjects`, `NakedAppearance` + `Outfit` replace `AvatarSettings`) → PocketShop, CustomSkateboard; NPC ragdoll refactor → HitmanPhone. `BankApp` only value drift. The new appearance save conversion is **one-way** — a save played in beta no longer loads cleanly on 0.4.6f13.
**Consequence:** All verifications continue against `v0.4.6f13`. Optionally planned (not created): deploy-guard script that checks game version against the expected one.

---

## 2026-09-19 — Release zips only local, no Nexus upload

**Decision:** Release packaging (`Tools/package-release.ps1`) serves only local distribution. No upload to NexusMods.
**Rationale:** User decision (Dominik). Mods remain private/local.
**Consequence:** `Release/` is gitignored; no publishing step in Tools or CI.

---

## 2026-09-19 — s1interop CI job activated, but advisory

**Decision:** The `s1interop` job in `.github/workflows/ci.yml` is active, but **never blocking**. It installs `S1Interop` 0.1.0-alpha.1 (dotnet global tool, source `ifBars/S1Interop`) and analyzes all mod csproj, gated on existing game assemblies.
**Rationale:**
- `s1interop analyze` always returns **exit 0** — even with reported errors. A blocking gate is technically not implementable with this tool version.
- Alpha tool with known false positives (`wrong_target_framework`, `global_usings_require_langversion`, because `Directory.Build.props` is not evaluated).
- Benefit remains: early warning on real IL2CPP errors (e.g. missing `IntPtr` ctors) directly in the CI log.

**Discarded alternative:** Leave job disabled until s1interop is more stable — discarded because the report is also useful in the alpha stage and poses no risk to the pipeline.
**Assessed but not fixed findings:** `ManagedCollectionSignatureInterop` in BusinessIncome (mod-internal API, no game callback → false positive); Reflection in HitmanPhone (`S1Quest` is internal in S1API; defensive with try-catch + fallback → accepted). **No code changes.**

**Addition (same day):** s1interop additionally anchored as **local, optional** pre-flight in `CONTRIBUTING.md` ("IL2CPP Obligations") — with install command, `doctor` hint, and explicit warning that `analyze` is a *report* and not a gate. The DoD checkbox "`analyze` clean" was factually wrong (exit code is always 0) and was corrected to "report read, no unexpected entries". So no contributor misinterprets a green exit signal as approval.

---

## 2026-09 (Reinstall) — Deploy convention: `Mods\` vs. `UserData\<Mod>\`

**Decision:** `Directory.Build.targets` deploys separately: DLL + PNGs + bundles → `<GameDir>\Mods\`; `mod.json` + `<Mod>.pdb` → `<GameDir>\UserData\<Mod>\`. JSON/PDB **never** in `Mods\`.
**Rationale:** MelonLoader loads from `Mods\` — anything else there causes unnecessary load attempts/log noise. Metadata and symbols belong in the UserData area.
**Consequence:** `Tools/package-release.ps1` mirrors exactly this structure in every zip.

---

## 2026-09 — Repo lives outside the game directory

**Decision:** Workspace under `C:\Users\pc\Schedule1-mod-only`, not in the game folder.
**Rationale:** Third move (history: `Desktop\Schedule1-mod-only` → `<GameDir>\...` → user profile). A repo in the game directory does not survive game reinstall/Steam repair.
**Consequence:** Game path is resolved at build time via `$env:SCHEDULE1_PATH` (or default fallback) — must be set **before** MSBuild starts, since the property is evaluated once.

---

## 2026-09-16 — Doc path guard (`check-doc-paths.ps1`)

**Decision:** All repo paths referenced in Markdown must exist; check runs in CI **and** pre-commit.
**Rationale:** Repo audit 2026-09-16 found unnoticed drifted paths (`Knowledge/`, archive paths, `.agents/`).
**Consequence:** Doc paths are the second guarded drift class alongside versions. Gitignored-but-documented paths (e.g. `GameReferences/decompiled`) are whitelisted.

---

## 2026-09-14 — Version-sync guard: code is the single source of truth

**Decision:** `Tools/check-version-sync.ps1` compares the code version (MelonInfo / `Constants.ModVersion`) with `docs/mod.json`, `README.md`, AGENTS.md matrix row and detail header. Exit 1 on drift; runs in CI + pre-commit. Bumps only via `Tools/bump-version.ps1`.
**Rationale:** Five places carry the same version number — manual synchronization is guaranteed to drift.
**Consequence:** Manual version edits are an anti-pattern; `bump-version.ps1` writes all places atomically.

---

## 2026-09-14 — Deterministic solution generation

**Decision:** `Tools/gen-sln.ps1` assigns GUIDs deterministically (`Get-DeterministicGuid "Project:<rel>"`) instead of randomly.
**Rationale:** Every regeneration previously produced random GUID diffs that made the CI determinism check pointless.
**Consequence:** CI compares line-ending-normalized (LF/CRLF-independent) — content is what counts.

---

## 2026-09 — IL2CPP primary, Mono as legacy branch

**Decision:** Primary target is IL2CPP (default Steam branch). Mono is a documented alternative path, not actively maintained.
**Rationale:** Default branch of the game is IL2CPP; dual runtime maintenance costs more than it brings to this workspace.
**Consequence:** IL2CPP discipline is mandatory (IntPtr ctor, EventHelper/ButtonUtils, WasCollected guards). `s1interop doctor` reports the Mono branch as `[missing]` — expected and irrelevant.

---

## Permanent / undated

| Decision | Rationale |
|---|---|
| **New patterns belong in `Shared`, not as mod duplicates** | `docs/architecture.md` defines the dependency direction; duplicates drift and cause N-fold bugfix costs. |
| **`Shared` and `_DiagPerfCounter` don't ship in releases** | No standalone player mods; `package-release.ps1` excludes them explicitly. |
| **`SkipUnchangedFiles=false`** (2026-08-20) | Every build force-deploys — prevents stale-DLL traps during testing. |
| **`ThirdParty/` is read-only** | Pinned dependencies (S1API 3.2.0, S1MCP); modification would break reproducibility. |
| **Archived mods are not built** | No maintenance overhead, no false signals in CI. |
| **No commits/pushes without explicit request** | User keeps control over history and remote. |
| **Sideload & hash no longer deployed** (2026-09-10) | Deprecated/unused; Sideload was never bound into any mod, hash only reference. |


---

## 2026-09-17 — Archiving policy

**Decision:** Archive mods on user request instead of deleting: `Source/Archive/<Mod>/`, DLLs to `Mods\_archived\`.
**Rationale:** Code stays as reference and for later reactivation, without build/maintenance load.
**Consequence:** `Source/Archive/` is **never** built or deployed. Affected: HomelessMod, SnackVendor (2026-09-17), Minimap (user prefers playing without), DayCounter, ProfitTracker, TVBrowser, BackpackMod.

---

## 2026-09-16 — Status convention `Verified <date>` vs. `In-Game-Verify open`

**Decision:** Two separate states are kept intentionally. `Verified` = functional proof in a real-game session, evidenced via fix/audit commit + CHANGELOG. `In-Game-Verify open` = code complete but unconfirmed.
**Rationale:** Honesty over fake perfection; no greenwashing of build successes as functional proof.
**Consequence:** No artifact needed in the repo — the assertion is made traceable via commit history and CHANGELOG.