# Repository maintenance checklist

Durable tracker for repository clean-up and remaining native (in-game) checks. This is not a
gameplay acceptance record and it does not replace `docs/compatibility.md`.

**Snapshot:** 2026-10-19. Status labels used below:

- **Observed** — verified by reading the working tree, files, hashes or `Latest.log` during this snapshot.
- **Parent-reported** — reported by the coordinating agent's run; not independently repeated here.
- **Proposed** — a test to run; it has **not** been executed.
- **Open** — no evidence yet.

## 1. Repository clean-up

### Completed in the working tree

- **Existing source layout:** mod sources live under `Source/Mods/<Mod>/src/`; tests live
  under `Source/Tests/`. This maintenance pass does not reorganize mod sources.
- **Single test runner:** `Tools/test-all.ps1` runs xUnit suites and the LightControl console
  host. CI and pre-push use it, with deployment disabled. Reviewed zero-discovery and
  environment-override holes are fixed; xUnit TRX execution counters and the console result
  contract are checked. Final parent run: 2,956 xUnit tests plus 151 console checks passed.
- **Observed parent validation:** all 17 projects passed, none skipped; full solution
  `dotnet format --verify-no-changes --no-restore` passed. A forced Release rebuild with
  `S1NoDeploy=true` completed with 0 errors and 0 C# warnings; only the framework support
  warning below was emitted twice (restore/build). This is not gameplay acceptance.
- **Framework constraint (accepted, not suppressed):** `LightControl.Tests` deliberately targets
  `net6.0`, matching the MelonLoader runtime and the linked production core. The SDK's
  `NETSDK1138` end-of-support warning is retained. Moving just the test host to a newer
  runtime to silence this warning would weaken runtime parity; changing the game's mod
  runtime is outside repository maintenance. This warning is not a gameplay verification.
- [x] Nullable warnings reviewed and corrected in MoreSaveSlots, BusinessIncome and the
  Hash command shim shared with PotScanner. PotScanner's blanket suppression removed.
  BusinessIncome retains only a two-line CS8601 suppression documenting S1API's no-op
  delegate invariant; no replacement delegates or new lifecycle behavior. Invalid-null
  test inputs are explicitly marked. MoreSaveSlots removes a no-card call whose exception
  was swallowed locally; live-card behavior stays unchanged.
- **Documentation:** SDK/runtime/submodule guidance and native-save versus mod-sidecar paths
  reconciled; test commands now use the no-deploy runner.

### In progress / pending

- [x] Deploy preflight and explicit third-party allowlist implemented and reviewed.
  Destination-override finding fixed and exercised through the actual MSBuild target against
  a fake installation; final live-install deployment/readback is recorded below.
- [x] Version-bump acceptance safety implemented; prior evidence stays historical, already-open
  rows retain their original evidence, and ambiguous labels abort before writes.
- [x] Durable tool regression suites: version 72, per-mod deploy 131, third-party/preflight
  deploy 64, skills health 21, runner 62: 350 assertions passed in the parent integrated
  run. CI discovers these suites; remote GitHub execution was not triggered.
- [x] Version sync: 21 mods, 0 drift; documentation paths, solution determinism, format and
  diff whitespace gates passed. Skills health: clean; historical version evidence is retained with narrow reason-bearing annotations.
- [x] Packaging disables deployment and includes GLB models in the mod data subfolder. Actual
  TaxiDriver package entries matched built DLLs, model and metadata; installed artifacts stayed
  unchanged. The package is a scratch verification artifact, not a published release.
- [x] Per-project hash receipts, attempt history, verified overwritten-file backups and bounded
  in-process rollback implemented. Corrupt backups, reparse targets, copy/receipt failures,
  partial rollback, stale-file reporting and no-op reruns are fixture-tested. Normal and
  third-party deployment share a per-install mutex.
- **Explicit limits:** no solution-wide/crash rollback, backup pruning or automatic stale-file
  deletion. Third-party deployment installs new DLLs or accepts identical content; differing
  installed DLLs require an explicit owner-managed upgrade and are never silently overwritten.
- [ ] Game-launch race after preflight remains: keep the game closed until deployment finishes.
- [ ] Complete the native checks below with the owner; no new gameplay session was run.
- **Working tree is intentionally dirty.** Existing HitmanPhone/LightControl changes are
  preserved. No commit, push or history rewrite is authorized by this checklist.

### Guardrails

- Distinguish artifact identity (hash equality) from native correctness. A matching DLL hash
  proves what loaded, not that a gameplay path works.
- Read existing logs and save sidecars without modifying the game or saves.
- One session in an empty checkpoint state cannot close the full gameplay acceptance.

## Final technical handoff

The parent ran `Tools/build-all.ps1` against the closed game after the focused delta review
returned PASS. Release succeeded: 0 errors, only the retained NETSDK1138 warning (two SDK
emissions). Readback verified **22 projects / 78 artifacts**: 40 copied, 38 content-identical;
source SHA256, installed SHA256 and receipt SHA256 agreed for every artifact. Attempt
receipts matched latest receipts, retained backup hashes matched their manifests, and no
stale entries were reported. Third-party allowlist was empty: no third-party DLL changed.

Durable receipts and backups are in `<GameDir>/UserData/.deploy/`. This was an actual deployment,
not a native gameplay test. The owner's native acceptance remains deferred until this handoff.
No commit, staging, push, hook activation or remote CI run was performed. New tooling files,
including `ThirdParty/.deployallow`, must be included when a commit is explicitly authorized.
The pre-push hook remains opt-in; its syntax was checked, not activated.

Installed DLL fingerprints for the consolidated test round (SHA256 prefixes; complete hashes
are in the per-project receipts):

| Mod | Installed source version | SHA256 prefix |
|---|---|---|
| HitmanPhone | 0.3.0-dev.1 | `aad5679242acd335` |
| TaxiDriver | 0.8.19 | `608c8221a30fe362` |
| LightControl | 0.1.0-dev.13 | `329a34ae72cd330f` |
| PropertyLEDs | 0.1.7 | `5efe7f06dc0d8ad5` |
| PocketShop | 0.3.7 | `1d98bde22f2c2b9a` |
| Minimap | 2.3.4-dev.1 | `359c28f6ce0551a5` |
| WorldLook | 0.1.2 | `f68fe25784f12602` |
| MoreSaveSlots | 1.0.12 | `47be5ec9d7f52515` |
| BusinessIncome | 0.1.6 | `dca7673ae99044ba` |
| PotScanner | 0.7.0 | `6af0d6eee73df298` |

Also smoke-test save-slot pagination/labels and BusinessIncome's normal day rollover; their
maintenance edits received compile/review evidence, not a new gameplay acceptance.

## 2. Remaining native checks

Each item below has non-native evidence (tests/build/review) but **no current gameplay
acceptance**. Treat every checkbox as open until a real session is recorded.

### HitmanPhone 0.3.0-dev.1

- **Evidence:** 148 pure/delegate/source checks; Release/Debug compile clean; deployed
  DLL/PDB/mod.json hash-matched. **Observed:** an empty-state checkpoint restore → native save
  commit → reload restore with the deployed artifact. Exact log lines and hashes are in
  `Source/Mods/HitmanPhone/docs/audit-repair-verification.md`.
- **Limitation:** the session loaded `active=0`, `history=0` and no kill events. No offer,
  acceptance, elimination, photo, drop, payout, slot switch, journal phase or MP rejection ran.
- **Open native checks:**
  - [ ] Normal offer → accept → KO → actual photo → first journal objective → assigned drop →
    exactly one cash increase and completed journal.
  - [ ] Wrong-drop photo first, correct photo later; no false payout and no blocked second claim.
  - [ ] Full inventory through KO; pending delivery survives save/load; one handoff window.
  - [ ] Cold restart with photo in inventory/drop; no duplicate spawn or new lethal heat.
  - [ ] Save before/after payment; reload with and without a post-payment save; money, photo,
    contract, tracker and journal select the same checkpoint.
  - [ ] Registry failure then recovery; both evidence IDs present before native item restore.
  - [ ] Journal phase/GUID persistence across reload, slot change and history.
  - [ ] MP lobby host/client: explicit unsupported status, no offers/mutation/payout
    (rejection check only).
  - [ ] Controlled checkpoint/native-save failure pauses mutations and preserves frozen state.

### TaxiDriver 0.8.19

- **Evidence:** 605 tests; final delta review PASS; nonincremental Release compile clean.
  Source-policy, delegate-seam and source-contract evidence only.
- **Open native checks:**
  - [ ] Blocked-front departure, dynamic rear obstacle, ineffective short reverse, and normal
    pickup/ride/arrival on the current build.
  - [ ] STOP, reroute, pause and slot change; native callback ordering.
  - [ ] No `GetWheelSweepCollision` ERROR spam; player cannot drive the taxi during an active ride.
  - [ ] Fare balance readback matches the charged fare; teardown logs one meter settlement and
    no taxi despawn on quit.

### LightControl 0.1.0-dev.13

- **Evidence:** 151 tests; split-control UI and persistence model checks pass.
- **Caution:** the observed 2026-10-19 session loaded **v0.1.0-dev.12**, not dev.13. Do not use
  that session as dev.13 acceptance.
- **Open native checks:**
  - [ ] Property headers with the lamp list left and selected-lamp controls right.
  - [ ] Tap sets one lamp; direct brightness step; original color keeps brightness; reset restores.
  - [ ] Apply to property vs the separate all-properties action; changed target list is rejected.
  - [ ] Temporarily unloaded lamps stay registered but are not recolored by a state command.
  - [ ] Save pending / save failed / saved states remain distinct; no per-frame signature rebuild.
  - [ ] Save/reload restores slot-scoped state and original lamp values.

### PropertyLEDs 0.1.7

- **Evidence:** 94 tests; scoped review PASS; larger phone typography implemented.
- **Open native checks:**
  - [ ] Full rainbow visibility at hue 0/1 and middle hues; white-to-colour saturation.
  - [ ] Expanded Advanced panel opens/closes, collapses on reopen, and remains scroll-reachable.
  - [ ] Scope switching never enables LEDs or writes settings.
  - [ ] Edits, inheritance and save/reload persist per slot.
  - [ ] Only measured/marked rooms receive persistent LED geometry; inspect real layouts.

### PocketShop 0.3.7

- **Evidence:** 52 tests; independent scoped/name-band review PASS; Release deployed with hashes
  matched.
- **Open native checks:**
  - [ ] Four-column, three-visible-row grid with scroll overflow and trailing filler cells.
  - [ ] Two-line name band fits real shop names at UI scale; tile size stable across catalogs/search.
  - [ ] v0.3.3 payment rewrite: cash shop debits cash, online/card shop uses the bank path,
    locks and refunds behave (last real session was v0.3.2).
  - [ ] Purchase status/SFX and level-rank rejection are visible and correct.

### Minimap 2.3.4-dev.1

- **Evidence:** 186 tests; Release clean; HUD and four settings pages initialized in the menu /
  disposable test world. Manual waypoint roundtrip was postponed.
- **Open native checks:**
  - [ ] Waypoint create/rename/color/delete and A → B → A retention; slot file survives reload.
  - [ ] HUD markers, police heat ring, health bar and portrait/name settings at real range.
  - [ ] Settings persistence and per-slot applied state.
  - [ ] `minimap searchdev` stays opt-in/read-only; the prepared search core is not runtime-wired.

### WorldLook 0.1.2

- **Evidence:** merged GoldenHour + WaterShader + GraphicsProbe into one DLL; 1,143-test suite;
  light look owner-confirmed 2026-10-18; manual probe capture exercised 2026-10-18.
- **Open native checks:**
  - [ ] Water clarity step: `_Absorption=0.38`, `_RippleStrength=0.45`,
    `_ShallowTint=(0.50,0.75,0.75)`; foam values untouched.
  - [ ] Owner judges whether the clarity step is the right size.
  - [ ] Confirm the Linear-colour-space effect on `_ShallowTint` (the percentages in the
    changelog are arithmetic on raw shader literals).
  - [ ] Only `WorldLook.dll` is loaded beside the merged mod; Ctrl+Shift+F7/F8 work as documented.

## 3. Merged mods are not separate releases

- `GoldenHour`, `WaterShader` and `GraphicsProbe` were merged into **WorldLook 0.1.0** and are
  **not** separate active releases. Do not package, deploy or list them as independent mods.
- Only `WorldLook.dll` is installed at snapshot; no standalone `WaterShader.dll`,
  `GraphicsProbe.dll` or `GoldenHour.dll` is present in `Mods\`.
- Their test projects (`GoldenHour.Tests`, `WaterShader.Tests`, `GraphicsProbe.Tests`) remain in
  `Source/Tests` for the merged components. Test presence does not make them separate releases.
- Release notes and the compatibility matrix should describe the merged WorldLook artifact only.

## 4. Proposed verification order (not executed here)

1. **Completed:** final `Tools/test-all.ps1 -Selection all` run passed all 17 projects without deployment.
2. **Completed with fixtures:** deploy preflight rejects running-game and invalid-target cases.
   This is tooling evidence, not a real game-launch/close session.
3. **Proposed:** HitmanPhone full Release chain on a backed-up disposable slot, then save/reload
   and MP rejection checks.
4. **Proposed:** LightControl dev.13 fresh deploy and UI/persistence checks (do not reuse the
   dev.12 session).
5. **Proposed:** PocketShop visual/payment, PropertyLEDs layout/rainbow, Minimap waypoint/HUD.
6. **Proposed:** WorldLook water/one-DLL check, then TaxiDriver reverse/ride/teardown.

Record each result with the artifact hash, log excerpt and exact checkpoint/slot used. Do not
promote a checkbox on compile or unit-test evidence alone.
