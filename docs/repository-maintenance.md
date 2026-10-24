# Repository maintenance checklist

Durable tracker for repository clean-up and remaining native (in-game) checks. This is not a
gameplay acceptance record and it does not replace `docs/compatibility.md`.

**Snapshot:** 2026-10-24. Status labels used below:

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
- [ ] Complete the native checks below with the owner; no new gameplay session was run by the agent.
- **Working tree state (2026-10-24):** clean and synced with `origin/main` at the start of this
  pass; the S1MCP clock fix, the Minimap `SearchStore`, the retired-probe clean-up and the
  documentation updates are new local changes. No commit, push or history rewrite is authorized.

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

Installed DLL fingerprints for the consolidated test round (**2026-10-19 snapshot — historical;
current versions live in the per-mod CHANGELOGs and the compatibility matrix**):

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
acceptance**. Treat every checkbox as open until a real session is recorded. Tick a box only with
the artifact hash, the log excerpt and the exact slot/checkpoint that produced it.

**Multiplayer is not a check.** Dominik does not play MP (2026-10-24). Host-authority and
MP-refusal guards stay in the code and fail closed, but they are not acceptance criteria and are
excluded from every list below.

### S1MCP 1.0.5 — clock units (smallest check, do this first)

- **Evidence:** 50 game-free tests in `Source/Tests/S1MCP.Tests`; Release IL2CPP 0 errors, no new
  warnings; deployed DLL `03DE264F…` matches the build output.
- **Why:** 1.0.4 treated `TimeManager.CurrentTime` as minutes. A 19:54 clock reported
  `32h 34m`, and `{"time": 12}` landed on 07:20.
- **Open native checks:**
  - [ ] `get_time_of_day` returns `time_hhmm` matching the HUD, `time_minutes` = hh*60+mm, and a
    24-hour `display` (`19:54`, not `32h 34m`).
  - [ ] `set_time_of_day {"time": 12}` reaches 12:00 (`readback_hhmm: 1200`, `changed: true`).
    A first call that reports `changed:false` with a stale readback is the known async-apply
    race — retry once before calling it a failure.
  - [ ] `hhmm` (e.g. `1954`) and `minutes` (e.g. `720`) reach the same clock value.

### HitmanPhone 0.3.0-dev.1

- **Evidence:** 148 pure/delegate/source checks; Release/Debug compile clean; deployed
  DLL/PDB/mod.json hash-matched. First native checkpoint evidence 2026-10-24: `Latest.log`
  08:22:55 restores checkpoint `56391B779CBE` for the confirmed world on a real save reload.
  Details: `Source/Mods/HitmanPhone/docs/audit-repair-verification.md`.
- **Limitation:** the restore path ran; registry ordering, GUID persistence and the cash paths
  did not.
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
  - [x] ~~MP lobby host/client: explicit unsupported status, no offers/mutation/payout~~
    **Not applicable:** multiplayer is out of scope (owner decision 2026-10-24). The refusal
    guard stays in the code, unverified.
  - [ ] Controlled checkpoint/native-save failure pauses mutations and preserves frozen state.

### LightControl 0.1.0-dev.19

- **Evidence:** 264 core/adapter/source checks; forced Release 0 warnings/errors; scoped format
  clean; independent delta review PASS; three installed targets hash-verified; native
  list/picker screenshots checked 2026-10-22. Details:
  `Source/Mods/LightControl/docs/graphite-ui-verification.md`.
- **Open native checks:**
  - [ ] Tap/hit-testing across the four-column table, sticky heading and both footer buttons.
  - [ ] Colour picker (ten choices incl. Rainbow) and the seven brightness steps apply correctly.
  - [ ] Long-list scrolling, reopen and scene/lifecycle transitions.
  - [ ] Exact font parity (Arial fallback is active because the Segoe UI array overload throws).
  - [ ] Save/reload restores slot-scoped state and original lamp values.

### PropertyLEDs 0.1.8

- **Evidence:** 115 tests; first-actual-open property selection and property-named power status
  2026-10-20. Details: `Source/Mods/PropertyLEDs/docs/implementation-status.md`.
- **Open native checks:**
  - [ ] Native location/status fitting for every owned property.
  - [ ] Toggle and colour application on real lamp geometry.
  - [ ] Save/reload roundtrip per slot (including per-room overrides).
  - [ ] Historical 0.1.4 UI acceptance does **not** cover 0.1.8.

### Minimap 2.3.5-dev.1

- **Evidence:** 215 tests; Release build 0 warnings/errors. The versioned `SearchStore` is new
  and **not runtime-wired**. Details: `Source/Mods/Minimap/docs/search-mode-phase0.md`.
- **Open native checks:**
  - [ ] Waypoint create/rename/colour/delete and A → B → A retention; the slot file survives a
    reload.
  - [ ] HUD markers, police heat ring, health bar and portrait/name settings at real range.
  - [ ] Settings persistence and per-slot applied state.
  - [ ] `minimap searchdev` stays opt-in/read-only.
  - [ ] Search mode itself is **not** testable yet: production adapter/world binding, curated
    area, Hints page, overlay and marker suppression are unimplemented. The Phase-0 experiments
    R1/R2/R3 are prerequisites, not checkbox items.

### WorldLook 0.3.0-dev.2

- **Evidence:** 1,167 water tests, full build 0/0, controlled GPU comparison and independent
  shader/fixture reviews PASS; DLL `410fecf7…` + bundle `29557b87…` match the owner run; morning
  motion owner-confirmed ("ruhig"). Details:
  `Source/Mods/WorldLook/docs/water-surface-verification.md`.
- **Open native checks:**
  - [ ] Overall/direct-sun appearance (the morning pair showed no strong direct-sun glint).
  - [ ] Positive warmth (strength 0 is expected after 07:30 — test before that).
  - [ ] ON after OFF (water re-applies cleanly).
  - [ ] Night look and save/scene reload.
  - [ ] Measured performance; no FPS measurement exists for this build.

### AutoPackagingStation 0.4.7

- **Evidence:** 66/66 tests; Release 0/0; retirement-scoped review PASS; owner-confirmed tested
  session 2026-10-15 (Pack/Unpack/mode mirroring). Details:
  `Source/Mods/AutoPackagingStation/docs/interaction-diagnostics.md`.
- **Open native checks:**
  - [x] ~~Separate multiplayer acceptance (host + client)~~ **not applicable** — multiplayer is
    out of scope (owner decision 2026-10-24); the host-authority guards stay in the code, unverified.
  - [ ] 0.4.1 cauldron-save migration (optional, only if such a save is still relevant).

### MessagesPlus 0.4.6

- **Evidence:** 25 tests; Release clean; popup/response repair owner-confirmed 2026-10-15.
- **Open native checks:**
  - [ ] Vanilla order menus (`MSGConversation.responseContainer`, `DealWindowSelector.Container`)
    stay undarkened while the rest of the app keeps the permanent dark theme.

### Closed — no action

- **TaxiDriver 1.0.0** — **verified, owner-confirmed 2026-10-24** (Dominik confirmed the work is
  finished; individual actions and the exact session were not itemized). 1.0.0 is a version-only
  milestone of the accepted 0.8.22 code (only the `MelonInfo` string and metadata changed), built
  and deployed with hash-matched DLL/PDB/metadata (`64CE7256…`). Per-finding record:
  `Source/Mods/TaxiDriver/docs/driver-binding-verification.md`. Known and accepted, not blockers:
  road-correction/jitter on some routes; multiplayer is out of scope (owner does not play MP),
  so the seat/STOP guards stay unverified in code.
- **PocketShop 0.3.9** — verified in-game 2026-10-24 (owner-confirmed; directory + item grid
  opened, no `ShopCatalog subscriber failed`). Last itemized purchase acceptance remains 0.3.2
  (2026-09-20) if a payment re-check is ever wanted; nothing is blocked.
- **S1MCP transport/routing** — 10/10 menu integration checks 2026-10-17, unchanged by 1.0.5.

## 3. Merged and retired graphics mods are not separate releases

- `GoldenHour` and `WaterShader` were merged into **WorldLook**; their code now lives in
  `Source/Mods/WorldLook/src/` and the modules ship inside `WorldLook.dll`. They are **not**
  separate active releases - do not package, deploy or list them independently.
- `GraphicsProbe` is **retired**: its source is archived under
  `Source/Archive/WorldLook/GraphicsProbe/`, its tests under `Source/Archive/Tests/GraphicsProbe.Tests/`,
  and its empty mirrored build-output folder under `Source/Tests/` was deleted on 2026-10-24.
  The "still open" probe items in older notes are not outstanding work.
- Only `WorldLook.dll` is installed; no standalone `WaterShader.dll`, `GoldenHour.dll` or
  `GraphicsProbe.dll` belongs in `Mods\`.
- Release notes and the compatibility matrix describe the merged WorldLook artifact only.

## 4. Proposed verification order (not executed here)

1. **Completed:** final `Tools/test-all.ps1 -Selection pure` run — 17 projects, 0 failures
   (`Shared.Tests` is game-required).
2. **Completed with fixtures:** deploy preflight rejects running-game and invalid-target cases.
   This is tooling evidence, not a real game-launch/close session.
3. **Proposed (cheapest first):** S1MCP clock read + one write in any loaded world.
4. **Proposed:** Minimap waypoint roundtrip and HUD/settings checks; then LightControl dev.19
   UI/persistence (do not reuse a dev.18 session).
5. **Proposed:** PropertyLEDs layout/rainbow, MessagesPlus order menus, WorldLook water/night.
6. **Proposed:** HitmanPhone full Release chain on a backed-up disposable slot, then save/reload.
7. **Out of scope:** any multiplayer check (owner decision 2026-10-24). TaxiDriver is closed
   (owner-confirmed 2026-10-24).

Record each result with the artifact hash, log excerpt and exact checkpoint/slot used. Do not
promote a checkbox on compile or unit-test evidence alone.
