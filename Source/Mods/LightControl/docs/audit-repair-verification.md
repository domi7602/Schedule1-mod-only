# LightControl audit repair verification

## Authorized deployment — 2026-10-21

After a separate owner **go**, the exact retained `0.1.0-dev.16` DLL/PDB/metadata were installed without
a rebuild and read back against the frozen manifest. Repository deploy receipt
`20261021T041602745Z-78ebc0c6bfa0` reports **Copied 3, Unchanged 0**, `Status: Deployed`, no stale files
and no rollback. Preflight before/after confirmed the game closed; no game was launched.

- Verified backup directory: `C:/Users/pc/Documents/Schedule1-Save-Backups/2026-10-21_061601-LightControl-dev16-3b22b284/`.
- `Tests-native-save.zip`: complete native **Tests** save (`76561199109519851/SaveGame_2`, 38 files), read back and individually hash-checked.
- `LightControl-UserData.zip`: all existing LightControl sidecars, backups, symbols and metadata; `LightControl.previous.dll`: prior installed DLL. Original files were not modified by the backup.
- `backup-manifest.json` and `deployment-verification.json` preserve the individual hashes, source locations, receipt/rollback handles and unchanged-file checks. The repository also kept per-target rollback backups under its normal deployment directory.
- All **76** unrelated mod/library/plugin witness files were unchanged, as were the native Tests save and all LightControl persistence files. Only the intended DLL, PDB and metadata were replaced, plus LightControl deployment receipts/backups.
- Installed DLL SHA-256 remains `a0697ef72100891f31149dbd341af7f136e02d02678c05f696203a561791f344`; PDB/metadata match the retained hashes below. The metadata is deliberately byte-identical to the frozen build record, including its historical compile-only/not-deployed label; this deployment record is authoritative for installed state.

**In-Game-Verify remains open.** Installation and exact hash equality prove delivery, not runtime
loading, PhoneApp fit, rendering or material destruction. The owner will start through Steam and load
the backed-up **Tests** slot. The implementation/compile-only history below is preserved as history.

## Original implementation scope and baseline (historical)

Owner-approved source/test/documentation changes and compile-only builds. No deployment, game launch, live-game or save mutations, commits, pushes, dependencies, Shared edits or schema changes. Preserve existing dirty work. English UI; seven brightness step buttons. Bump once at implementation end using the inspected repository script.

Baseline before implementation:

- `dotnet run --project Source/Tests/LightControl.Tests/LightControl.Tests.csproj -c Release -p:S1NoDeploy=true -p:BuildProjectReferences=false`: **151/151 passed**, 0 failed. Existing NETSDK1138 warnings report the unsupported net6.0 target; these are not new code warnings.
- `dotnet build Source/Mods/LightControl/src/LightControl.csproj -c Release --no-incremental -p:S1NoDeploy=true -p:BuildProjectReferences=false`: **0 warnings, 0 errors**. SDK 8.0.425; test runtime 6.0.36.
- Current source baseline: 0.1.0-dev.15, branch main with pre-existing changes. Existing owner acceptance concerns that earlier UI, not these new repairs.
- Installed LightControl/Shared/S1API and LightControl metadata hashes captured in scratch `lightcontrol-audit-installed-baseline.json` for final no-deployment comparison.

## Shared contracts inspected, not changed

`SafeStorage.GetUserDataPath` uses MelonEnvironment.UserDataDirectory plus mod name; it does not add player/world identity. `SaveSlots.GetActiveSlotNumber` returns a nonnegative active slot or -1 and catches native probe failures. A new world in the same numeric slot is not distinguished by this format; no invented world ID or migration is added.

`SafeStorage.SaveTextAtomic` writes `.tmp`, attempts a best-effort backup copy (failure logs but does not veto the replacement), then calls overwrite `File.Move`. It catches errors and returns false, but does not fsync the file/directory. Its name is not proof of crash durability or backup success. `LoadTextSafe` returns raw readable primary text even if its JSON is corrupt; it does not validate schema, identity or contents. LightControl must perform its own validated read/recovery decisions without changing Shared.

### Scope incident: Shared deployment receipts

The native implementation subagent ran an early build without the required no-deploy flag. This was **outside the approval**, even though no binary copy occurred. Readback of `UserData/.deploy/receipts/Shared.json` confirms attempt `20261020T192720934Z-5bea34d69ea1` wrote deployment receipts; both DLL/PDB entries are `Action: Unchanged`, `State: NotAttempted`. The parent independently compared all initially captured installed hashes: LightControl DLL/PDB/mod.json, Shared DLL and S1API DLL were unchanged. The Shared PDB receipt also reports equal source/destination hashes. No receipt rollback or other install mutation was attempted. Later parent builds always explicitly use `S1NoDeploy=true` and `BuildProjectReferences=false`. Consequently the accurate statement is **no mod binaries deployed; unintended Shared receipt writes occurred**, not "nothing in the install was written".

## Evidence boundaries

Behavioral core/delegate/adapter tests do not prove native Unity setter timing, actual material destruction, rendering, gameplay or on-device UI fit. Actual PhoneApp-container measurement and screenshots require a separately approved game session and remain open. No game is started for this work.

## Final result

**0.1.0-dev.16**: **233/233** final checks passed; forced compile-only Release built with **0 warnings / 0 errors**. Full independent persistence/native/UI reviews and final scoped delta reviews completed; both final delta verdicts were **PASS**. The inspected version script ran once after implementation, then the final build/test/version checks ran against that version. No native gameplay acceptance is claimed.

### Final executed gates

- `dotnet run --project Source/Tests/LightControl.Tests/LightControl.Tests.csproj -c Release -p:S1NoDeploy=true -p:BuildProjectReferences=false -p:CheckEolTargetFramework=false`: **233 passed, 0 failed**. Core/delegate/source cases plus real fixture I/O through the production adapter and Shared writer; only loader logging/path and native slot probing are shimmed. The EOL switch suppresses the known net6.0 SDK support notice; it does not change the target or prove continued upstream support.
- `dotnet build Source/Mods/LightControl/src/LightControl.csproj -c Release --no-incremental -p:S1NoDeploy=true -p:BuildProjectReferences=false`: **0 warnings, 0 errors**. No Shared project build or deploy target is invoked by this final command.
- `dotnet format LightControl.csproj --verify-no-changes --no-restore --include ...` from the production project directory, with every changed C# file explicitly included: **exit 0**. The equivalent test-project command includes every changed/new test file: **exit 0**. Includes are project-relative, not accidentally empty repo-relative paths. No whole-solution format claim.
- `Tools/check-version-sync.ps1`: **21 mods, 0 drift**; LightControl code/JSON/README/AGENTS all `0.1.0-dev.16`.
- `Tools/check-doc-paths.ps1`: **PASS**. `git diff --check` for LightControl/tests and its shared documentation rows: **PASS**.
- `ilspycmd` against the actual final DLL: `[assembly: MelonInfo(typeof(Mod), "LightControl", "0.1.0-dev.16", ...)]`. The unchanged SDK-default `AssemblyVersion`/`AssemblyFileVersion` are `1.0.0.0`; MelonInfo and `mod.json` carry the mod version. `AssemblyInformationalVersion` includes the base HEAD, not a clean-working-tree assertion.
- Final source budget: **19 C# files + project = 20**. No production file/assembly/dependency added. No commit or push.

### Retained verified candidate (outside the game and scratch)

Directory: `C:/Users/pc/AppData/Local/Schedule1Tools/VerifiedBuilds/LightControl/0.1.0-dev.16/`.

| Artifact | SHA-256 |
|---|---|
| `LightControl.dll` | `a0697ef72100891f31149dbd341af7f136e02d02678c05f696203a561791f344` |
| `LightControl.pdb` | `53f7979cc1fdbec5be4ef03f84ad3e42551a40425e2515df76fa68fcf7648478` |
| `mod.json` | `a3ed89c3856ce6abb9687c2c9131aa293a2fe8325f0dfa23caa31181e66ab9ee` |

`verified-build.json` records artifact hashes, **184 resolved reference hashes**, source/build-input hashes and the exact command/scope. The source fingerprint is `904cf04e7807873d51242fbba5f7bbfb6931d6d75f26c011c0129cc7e122512b`. `source-snapshot.zip` contains **40** source/test/build/metadata/asset entries, each read back and hash-checked; it is not a standalone game/dependency distribution. `final-verification.log` and `compiled-version.txt` preserve the final tool results. The repository is dirty; HEAD alone does not identify this candidate.

Any later separately approved deployment must take these exact retained DLL/PDB/metadata hashes, not whatever happens to occupy `bin/` later. If rebuilding is necessary, compare the approved source and dependency manifests, rerun verification, and record the new hashes before deployment. No package in this directory authorizes installation or a game start.

## Per-finding disposition

All twelve findings were confirmed in the inspected baseline code (some need the report's conditional native setup). None was treated as already repaired merely because an old test was green.

| Finding | Current code disposition | Executed evidence / remaining boundary |
|---|---|---|
| LC01 | Corrected: own output cannot become a fresh baseline after missed undo; hand-back confirmation is per property; unresolved outputs remain owned/retained. | Production policy/delegate regressions and source wiring. Actual native getter/setter timing and reset still require game testing. |
| LC02 | Corrected: atomic rebind replacement includes property/group; active owned-parent changes reconcile core before runtime view. | Core A-to-B and rejected-transition tests. Same-GUID native movement remains unexercised. |
| LC03 | Corrected: both fixture types use a common rebind publication path; old resources retire before discovery, responsibility transfers to retained cleanup, and throwing preparation leaves the core unavailable/retryable. | Executed throwing-preparation regression plus supplementary source contracts. Fifty real tent rebinds and native clone accounting remain open. |
| LC04 | Corrected: readable null slots differ from failed reads; release requires a full post-setter disconnect proof; slot relocation hands back the prior installation. | Material-array/ownership decisions and wiring checks. Real Unity arrays, duplicate refs and actual Destroy acceptance remain open. |
| LC05 | Corrected: slot changes invalidate immediately; converged material state has bounded periodic validation; material conflicts remain visible. | Policy/source checks and status regression. Actual native material/emission replacement cadence remains open. |
| LC06 | Corrected: ownership reconciles before final preview comparison; exact key set and catalog pin are checked at the mutation boundary. Changed previews reject without partial application. | Executed lost-target, changed-catalog, equal-count/different-keys, duplicate-keys, happy-path and rejected-command retry checks. Gameplay still open. |
| LC07 | Corrected: the bounded known-fixture reconciliation queue includes inactive entries, and rediscovery handles positive deletion before initialization shortcuts. | Source wiring plus existing model deletion/unload cases. Streaming without proof retains state; a dead/unknown handle never establishes deletion. Real lifecycle sequence remains open. |
| LC08 | Corrected: temporary absence retains selection, confirmed absence resolves it, and session changes reset it. | Pure selection and app-wiring tests. Native deletion/reopen acceptance remains open. |
| LC09 | Corrected: one editability path covers modes, palette, brightness and actions; three modes have persistent markers. | Pure classification and wiring tests. Rendered disabled-state/interaction acceptance remains open. |
| LC10 | Corrected: actual session reason, scan/truncation, persistence and native conflicts/retained cleanup are distinct; empty catalogs do not erase storage warnings. | Pure status tests and actual disk-adapter failures/recovery tests. Live client/phone integration remains open. |
| LC11 | Corrected code costs: immutable static instructions are reused, logging eligibility precedes native readback/formatting, and unchanged open UI skips catalog projection/layout writes. | Isolated before/after resolver microbenchmark at 1/50/200 simulated lamps. Whole-runtime CPU/FPS, native calls and app-open/closed measurements remain open. |
| LC12 | Corrected: explicit read outcomes, ancestor reachability proof and identity/schema/content-aware backup recovery; error/detail retained; save exceptions cannot abort following teardown cleanup. | Real adapter + real Shared writer fixture tests, only loader/slot boundaries shimmed. Unavailable-drive RED observed before ancestor-proof repair; later access does not silently reopen a blocked gate. No game save was touched. |

## Integration repairs and review triage

- A first review found that cleanup after an already-committed core rebind could strand an Available core record. A runtime-used preparation callback now executes before publication, under the same mutation guard; an injected preparation exception was observed RED and then GREEN, with the old registration and retry preserved. The independent delta review confirmed this ordering is sound.
- A retained-restore cap must never discard already-owned output. The initial cap path was rejected by integration checks. Retention is now lossless for acquired claims; a soft threshold applies backpressure to new influence, and captured native identities reserve pending sources against a new binding. Unknown native identity is not proof of destruction.
- The initial absence proof still accepted `DirectoryNotFoundException` at an unavailable drive. The persistent real-disk test reproduced an incorrectly Ready gate; walking to a successfully listed ancestor fixes it while retaining genuine first-use initialization.
- The wide editor header must not be hidden with its compact-only Back button. The Back object, not the whole header, is now the compact toggle target. Two-line lamp rows, two-row palette and brightness controls give long values more room; actual layout acceptance is not claimed.
- The source-contract tests are supplementary and explicitly named as such. They are not simulations of actual native release. Core/delegate tests and real fixture I/O are reported separately from game verification.
- A review suggestion to infer permanent deletion from an unavailable wrapper was rejected: no native evidence establishes that inference. A fixture without both readable coupled outputs intentionally stays temporarily unavailable; no switch or native object is forced to make it controllable.
- Failed/unknown material references may remain tracked until readable proof arrives, including across teardown. This deliberately prefers a visible retained cleanup backlog over destroying an unproven resource. Process exit and permanently unreadable proxies remain a native acceptance boundary, not a claimed leak-free outcome.
- The final native review caught an unwired own-clone discovery guard and a mid-name `(Instance)` stripping hazard. The exact `ledlight_light_on (Instance) (LightControl)` case was observed RED as a falsely accepted native baseline; both native identity rejection at discovery and terminal-only instance-suffix matching now pass. Pending clones are never adopted as a new native clone source.
- Default Original/100% sources now skip native reads/writes when no prior property is owned; an owned/pending output still takes the restore path. The runtime-used `NeedsApply` decision has a targeted RED/GREEN regression. Material-conflict flags/counters are separated from cross-session retained cleanup status.
- Final disk coverage executes the normal real Shared save/backup-rotation path and missing-primary/valid-backup recovery, not just the special protected save. Repeated adapter loads retain good-backup protection until a successful protected save; a foreign save-ID with additional root fields remains an identity conflict instead of being recovered over. Both latent defects were reproduced in real fixture I/O before correction. An unconsulted backup now carries no absence proof.
- The empty-list message updates on scan completion even without a catalog revision; selecting another lamp dismisses the previous subject's world-copy confirmation. Pure state/text and supplementary wiring regressions cover these UI repairs; no artificial rendered-geometry test is used.

## Dependencies and API checks

The referenced installed S1API reports FileVersion `3.2.1.0`, ProductVersion `3.2.1+a4cbdd9e0ec54895e64fd6be0e91874ba6649c9b`; the source submodule matches the full commit. Independent UI review checked the actual pinned PhoneApp/UIFactory/EventHelper members used here. The compile gate uses the real installed reference assemblies. Evaluated MSBuild properties confirm `S1NoDeploy=true`, `BuildProjectReferences=false`, `net6.0`, C# 12. The local Shared build reference matches the installed Shared SHA-256 `087b876818beaffcc95d834a9b6d353d08d8c1c8e932ef0fe747e03d3e060335`; Shared source is unchanged.

## Isolated measurement scope

`resolver-microbenchmark.csv` compares the original HEAD resolver with the repaired resolver in one Release harness (1,000 simulated frames, 1/50/200 lamps; Original, Red, Rainbow). Both use the same model/dependency inputs. Static instruction/color allocations decrease; identifier-validation allocations and dynamic Rainbow allocations remain. Timings are a single microbenchmark observation, not a statistical runtime profile and not proof of FPS improvement. No UI, Unity object, native material count or open/closed app was measured.

## Deferred in-game acceptance

After separate deployment/game approval, use a backed-up disposable test slot and the exact retained candidate artifact. Measure the actual PhoneApp container at 720p/1080p/ultrawide and different UI scales; check all seven steps, nine colors, Original versus Reset, long names, compact Back, editor scrolling and repeated open/close. Exercise native getter/setter failures only with separately agreed instrumentation; inspect reset baseline, external writes, off/on state, null/duplicate material refs, slot movement, 50 tent rebinds, same-GUID property changes, inactive positive deletion, stale copy previews and teardown. Capture real screenshots and logs. Until then native rendering/material destruction/gameplay stay **In-Game-Verify open**.
