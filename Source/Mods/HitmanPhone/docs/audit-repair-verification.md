# HitmanPhone audit repair — verification record

Current code is 0.3.2. The Hitman Tracker phone app was removed in 0.3.1, and 0.3.2 widened
offer targets to the player's own unlocked customers. Dominik confirmed 0.3.2 on 2026-10-25;
the session note below records what the log and end-of-session save actually show. The older
audit rows do not verify either change. Status of the audited 0.3.0 code: **Verified, owner-confirmed 2026-10-24**
(shipped as 0.3.0 with version metadata only). The confirmation had been missing from the record and was supplied
retroactively; individual actions and the exact session were not itemized. This record separates
executable model tests, source-wiring checks, builds and native gameplay observations.
The normal Release offer → elimination → photo → assigned drop → exactly one cash payout and
saved reload/slot roundtrip is owner-accepted as working. The 0.3.0 confirmation has no separate
log artifact for that chain. A session on 2026-10-19 observed an **empty-state** checkpoint restore → save
commit → reload restore with this artifact (see the final evidence section), and a 2026-10-24
log adds a real save-reload checkpoint restore. Those remain the stored evidence samples.

## 2026-10-25 session (0.3.2)

- Game 0.4.7f12, Unity 2022.3.62f2, MelonLoader 0.7.3. `Latest.log` 14:53:44–15:11:59.
  Loaded `HitmanPhone.dll` SHA-256 `B03899BD0ADA582AF87B0FFB70A07D40696911C3F5CFAA88BADD405B73AB2393`.
- Phone-app init listed no `BountyTrackerApp`. Registered apps were Bank, Calculator, LightControl,
  Minimap, Notes, PocketShop, PotScanner, PropertyLEDs, StorageScanner, SeedWorks, Taxi and Weather.
- Day 2 20:02 slot was dispatched: Jackal offered `donna_martin` ($150, Behind Thompson Construction),
  accepted as `hitman-f64d2e14270`, kill matched, cash payout $150, journal completed, polaroid consumed.
  Later offers: day 3 07:18 `geraldine_poon`, day 3 11:00 `peggy_myers`. Checkpoints `E6D2A61ECCE0`
  and `19A7DFA1DDE7` committed for `SaveGame_1`.
- End-of-session `NPCs.json`, read after `19A7DFA1DDE7`: only `benji_coleman` recruited among 11
  dealer-like records. `donna_martin` is in his assigned list. `geraldine_poon` and `peggy_myers`
  are not. The log does not print why a target was eligible. This file is not a snapshot from before
  the day-2 offer, so it does not independently prove the empty-network path.
- Geraldine's polaroid was kept at the wrong drops and that contract expired at 15:11. That is the
  assigned-drop guard. A load-window multiplayer notice fired before the save was ready; later offers
  show the singleplayer gate opened. MP remains out of scope. A quit-time `LandVehicle.SetVisible`
  null reference is not this mod.

## Baseline and scope

- Input: owner's `HitmanPhone_Bugreport_Hermes.txt`, 2026-10-19, HP-01–15 and R-01–07.
- Source baseline: `da19186`, HitmanPhone 0.2.9; clean working tree before repair.
- Existing pure suite: 33 passed, 0 failed/skipped. It did not establish native acceptance.
- Latest installed log reports game 0.4.7f12, Unity 2022.3.62f2, S1API 3.2.1;
  Steam manifest build 25796331. The game was closed during baseline inspection.
- Actual installed S1API SHA-256:
  `79d7af288b605f8fcd4d2582a59636670b6cafd6a8847bc31c21b2278a9f2bf3`.
  This differs from older inventory hashes. Live lifecycle and crime wrapper surfaces
  were decompiled; an assembly version/string is not proof of all API behavior.
- Owner decision: **singleplayer only** for this repair. Multiplayer host and client
  contract mutation/payout paths are unsupported and must be blocked, not advertised safe.
- KO remains a valid elimination; reward remains cash; existing item IDs, positive tokens
  and narrowly scoped legacy matching remain compatibility requirements.

## Save model and migration

Gameplay marks the bounty state dirty in memory. Save-start freezes the state; all bounty
mutations pause during that checkpoint. Save-complete commits only after native SaveError
is false, the bound identity still agrees, and the captured native destination matches
the active save path and normalized loaded-world directory. Both `Save()` and `Save(string)`
are instrumented. An existing `Game.json` path can normalize to its own world directory;
arbitrary ancestor/staging folders are not accepted. Native call routing remains unproven.
No additional unconfirmed Save request is used as a transaction.

Native world JSON files are content-fingerprinted at load/save boundaries only, with file
and byte limits. `Modded` and `Backups` folders and save-info `Metadata.json` are excluded.
Money, inventory, quest and other native world JSON remain in the fingerprint.
Each matching bounty snapshot
is archived under `UserData/HitmanPhone/checkpoints/<identity-hash>/<world-hash>.json`.
The slot JSON remains a compatibility copy. A known older native checkpoint selects its own
bounty snapshot, rather than the newest completion state. A missing/mismatched/corrupt
checkpoint pauses HitmanPhone and retains sources; it must not silently create a fresh save.
The primary slot copy or its backup is a recovery candidate only when both save identity
and world fingerprint match exactly. Identity alone is not rollback-safe.

The loaded baseline is retained in RAM and archived at the first explicit save, not while
loading. Legacy files are read without move/delete or immediate rewrite. Existing fields
and IDs are retained; explicit null collections/null rows are normalized before iteration.
An old write-through file cannot retroactively prove which native world checkpoint it
belongs to. Legacy alignment is therefore an explicitly unverified migration boundary.

This is **not an atomic transaction across the native engine and the mod filesystem**.
A process/disk failure between them can leave an unknown checkpoint; the safe response is
to pause, preserve evidence, and require recovery rather than pay twice or discard a claim.
Native event ordering, save destination capture and fingerprint stability across real loads
must still be exercised. Do not call HP-01 accepted on model/source tests alone.

## Preserved local recovery material

Before tests/deploy, a ZIP containing the old installed HitmanPhone DLL/PDB, its UserData
and the existing native Saves tree was created outside scratch. ZIP CRC check passed.
The absolute local recovery path is recorded in the work-session result, not redistributed
with source/release packages. No native save or original bounty JSON was edited by the repair tools.

## Required native Release acceptance (still open)

Use a backed-up disposable singleplayer slot. Use normal caller eligibility and messages;
DEBUG force-offer/kill commands cannot establish Release acceptance.

1. Normal offer → accept → KO → actual photo → first journal objective complete → assigned
   drop → exactly one cash increase and completed journal. Repeat the same storage event.
2. Two photos: wrong-drop photo first, correct photo later; both correct; failed first payout.
   Wrong-drop photos survive and cannot block another eligible receipt.
3. Full inventory during KO; keep it full until the target wakes, then free a slot. Save/load
   while delivery is pending. No second attack; short delivery window starts at handoff.
4. Cold restart and menu/slot changes with a photo in inventory and in a drop. Existing
   evidence does not spawn again or trigger new lethal heat merely because the world loaded.
5. Save before payment, receive payment, reload without saving; then repeat with a confirmed
   post-payment save. Money, photo, contract, tracker and journal select the same checkpoint.
6. KO → death, including after completion, inside/outside the old debounce. Tracker upgrades
   only a known target; no second photo/window/payment.
7. Historical and active journal quests, same target with different contracts/phases and
   varied restoration order. No arbitrary adoption or false-success orphan cleanup.
8. Call sender unavailable/throwing/partial message delivery; retry; one outstanding offer;
   warnings do not destroy its response path. Several blocked days respect delivery cadence.
9. Exact deadline boundaries (before, equal, after), second accept during lethal pursuit,
   and long cooldown followed by short cooldown.
10. Menu-only metadata scan, A → B → A, same-slot reload and new game in reused slot.
    Observe `[context]` and `[checkpoint]`; no foreign identity/path writes.
11. Registration temporarily unavailable, then available; both evidence IDs must be present
    before native item restoration. Log success must correspond to registry readback.
12. ~~Multiplayer lobby host/client: explicit unsupported status, no offers, evidence mutation,
    photo consumption or payouts.~~ **Not applicable (2026-10-24):** the owner does not play
    multiplayer, so this rejection check is out of scope. The SP-only readiness gate, tracker
    status and local notice remain in the code, unverified.
13. Controlled checkpoint IO/native-save failure: mutation pauses, original/frozen state is
    retained, failure is visible, retry does not write an old snapshot to a new slot.

## Evidence ledger

### Intermediate parent integration, 2026-10-19

- Independent static review exposed a pending-load retry which restarted its deadline.
  Retry now calls `TryInstallContext` without re-entering/resetting `OnLoadComplete`.
- Both native save overloads now capture destinations; normalized loaded/info/request
  directories must all agree. Missing live identity still refuses writes deliberately.
- Excluded metadata timestamp churn from the fingerprint. Added exact checkpoint checks
  for primary/backup recovery. A different world hash for the same identity stays rejected.
- Observed RED/GREEN for metadata/hash and fallback matching; source contracts cover the
  bounded retry wiring and both-overload registration, not native Harmony execution.
- Isolated parent suite: 24 passed, 0 failed/skipped. Integrated current suite: 121 passed,
  0 failed/skipped. Release compile succeeded against installed references, initially with
  five nullable warnings. After accurate nullability annotations and skipping null storage
  entries, Release compile passed with 0 warnings/errors and all 121 tests stayed green.
  Independent delta review closed B1–B6 at source level, with native path/event/fingerprint
  stability still open. A DEBUG reset identity regression found by that review was tested
  RED/GREEN using the runtime-used reset factory; identity and token high-water survive.
- Current integrated suite after that fix: 122 passed, 0 failed/skipped. Both Debug compile
  and scoped production `dotnet format --verify-no-changes --no-restore` passed. Final
  evidence/calls/journal integration reviews are in progress; this is not a final verdict.
- No deploy or new native gameplay acceptance at this intermediate point.

### Candidate integration checks, 2026-10-19

- Candidate: **0.3.0-dev.1**, versioned through the repository helper, not a stable release.
- Current full suite: **148 passed, 0 failed, 0 skipped**. Release and Debug compilation:
  **0 warnings, 0 errors**. Scoped production format and `git diff --check`: passed.
- RED/GREEN observed for parent checkpoint/null/time/pursuit tests and the added legacy-zero
  presence, incomplete-scan aggregation and settlement delegate scenarios. Wiring checks
  are supplementary text assertions, not execution of Harmony/Unity/native storage.
- An independent review found uncalled registry lifecycle helpers, unreachable MP notice
  and inconsistent legacy-zero presence. Parent wired Main/preload + bounded pre-ready
  retry, local MP notice + tracker status, and shared presence with Unknown/Missing/Present.
  Unknown reads/ambiguous legacy-zero scans defer photo repair and retry reconciliation;
  they cannot turn into fabricated missing evidence or a synthetic integer zero.
- Live installed Registry wrapper inspection confirms static `ItemExists(string)` and
  `GetItem(string)`, instance `AddToRegistry(ItemDefinition)`. Registry-ID lookup binds the
  cached definition; unverified IDs cannot spawn a new savable photo. Timing still open.
- Payment seam tests inject exceptions before/after simulated cash mutation and re-entry.
  They prove the runtime-used state seam, **not** native cash movement: unknown outcomes
  retain the claim/photo, disable retry/expiry/death forfeiture, and display manual review.
- Calls/journal/tracker read-only review: PASS at source level. Final independent integration
  delta review: **PASS / no hard source blocker**, including lifecycle/MP visibility,
  conservative presence and payment uncertainty. All native-only items remained open at this
  review point; the owner confirmation of 2026-10-24 later accepted them (see the status header).

### Deployed Release candidate identity

- Version: **0.3.0-dev.1**, game closed before auto-deploy; Shared build/installed SHA-256
  matched before deployment. Build references were not rebuilt or redeployed.
- Nonincremental Release build after versioning: **0 warnings, 0 errors**. Installed
  DLL/PDB/metadata were read back and byte-hash matched with the exact build/source outputs.
- DLL SHA-256: `aa48ca59b3df56368bb878d4837dbf4196c7fd8e1c599bed3f2b1b419eac4a95`.
- PDB SHA-256: `419652cf3e5ec83136d0a490dbd4255fe44a010e7f2523a35109375c2c1f3df3`.
- Metadata SHA-256: `20f35813cf8c865e19f98b4beaf7825728405e704835f71b13ca6c17508a6a0f`;
  installed metadata version reads `0.3.0-dev.1`.
- Shared remains `68bf1e033c164b529253ab7fc2b4ad7df86ba884f14d933909e4718f3ac13eeb`.
- Re-run full suite with persisted TRX: **148/148 passed**, 0 failed/not-executed. Production
  source budget: **18 files** (17 C# plus project). Both production and test format checks,
  version sync, documentation paths and whitespace-diff checks passed.
- No game start, native save mutation, normal Release chain test, commit or push by this
  repair **at the time this record was written (document mtime 2026-10-19 09:11)**. The
  later 16:14–16:37 session below started the deployed game and completed an empty-state
  native save; it did not mutate bounty state and does not establish the normal Release
  chain. Deployment is preparation for native acceptance, not evidence of it.

## Per-finding cause, repair and evidence

**Status convention:** every row below is a source/model repair or rejection boundary;
**none is a new native gameplay acceptance**. No original P1 runtime scenario is declared
widerlegt from compile success. HP-01/02/07/10 and R-01/02 remain native acceptance gates.

| ID | Actual source cause / implemented change | Executed evidence and outcome | Native acceptance still open |
|---|---|---|---|
| HP-01 | Immediate sidecar writes could precede world money/items. Dirty memory, frozen save snapshot, identity/path-bound commit and exact world-hash lookup replace write-through. Unknown checkpoint pauses, originals retained. | `PersistenceRegressionTests`: wrong context, failed world save/writer, frozen destination, rollback fingerprints and exact fallback; GREEN. | Payment/save/reload with and without a newer native save; native save ordering, engine/sidecar crash gap. |
| HP-02 | Creation/alive was mistaken for registration and first spawn was late. Main/preload invalidation + verified ID lookup, bounded retry before gameplay, reuse registered definition; no unverified spawn. | `PersistenceWiringTests`: lifecycle/callsite contracts GREEN; installed Registry API wrapper signatures decompiled. | Cold restart, photo in inventory/drop, registry first failure then recovery; actual registration before native restore. |
| HP-03 | Early container return after rejected/failed photo. Whole-container plan, reentry latch, exact per-photo consumption and continued eligible claims. Cash-call uncertainty does not stop another claim. | `EvidencePolicyTests` scan-plan cases + `RewardSettlementTests` independent next claim; GREEN. | Native slot mutation/repeated event/consumption and two actual cash payouts. |
| HP-04 | Full inventory returned before recognition. Persist recognized elimination + pending delivery, handoff starts drop clock without another attack. | Pending-state JSON roundtrip and phase/deadline model cases; GREEN. | Keep inventory full until KO ends, reload pending, disappeared target, actual native AddItem handoff. |
| HP-05 | Unconditional evidence flag reset re-spawned and re-applied heat. Actual presence reconciliation; delivery repair is separate from elimination. Unknown/ambiguous reads defer, not duplicate. | `EvidencePresenceTests` single-target legacy zero, ambiguity, terminal cleanup and completeness; pending roundtrip GREEN. | Existing inventory/drop photo, lost photo, repeated reload/full inventory, no new lethal heat. |
| HP-06 | Exact generic objective title could not match named text. Structural objective indices restore eliminate-complete/drop-active. | `JournalTrackerSourceContractTests` source checks GREEN. | Actual entry states on fresh/restored native quests and title/countdown visuals. |
| HP-07 | First-unbound adoption and no phase reconstruction. Deterministic contract GUID, exact resolution, structural phase restore, truthful terminal cleanup; unknown old quests untouched. | Journal source checks GREEN; installed S1API quest creation/GUID source inspected. | IL2CPP StaticGUID write/persist/restore, missing/reordered/history quests and exactly one active journal entry. |
| HP-08 | Contract-only rows counted as KO; completed target rows counted contracts. `HasElimination`, separate KO/death and actual per-contract completion count. | `JournalTrackerServiceTests`: untouched active/expired targets and repeated contracts on same NPC; GREEN. | Actual tracker UI with representative native history. |
| HP-09 | Death upgrade was hidden behind new-photo eligibility/debounce. Known target death upgrade runs independently, including completed history; unrelated NPCs excluded. | Tracker model upgrade/no-duplicate cases GREEN; hook/watchdog paths read statically. | Native KO-to-death events inside/outside debounce and after completion, no extra photo/reward. |
| HP-10 | Slot consumed before void sender. Typed send outcome, only Delivered consumes/counts; bounded deferred failure and single-use offer ID. | `CallsLifecycleTests` outcome/attempt/single-use cases GREEN. | Missing real caller, native SendTextMessage exception/partial delivery and later normal retry. |
| HP-11 | No-due return lost plan changes. Plan fingerprint marks dirty for generation, extension, cancellation and defer; unchanged ticks do not request persistence. | `CallsScheduleTests` mutation/serialization/fingerprint cases GREEN. | Native world-save/reload preserves identical plan without re-roll. |
| HP-12 | Blind reschedule ignored other slots and actual delivery history. Whole-plan free-slot search plus successful-send quota (2/day) and 210-minute spacing. | `CallsScheduleTests` full following days, spacing/quota and actual delayed sends GREEN. | Several actual blocked days/sleep/day transitions, no burst. |
| HP-13 | Poller-only expiry admitted late events. Shared strict `now < due` decision at recognition, delivery, immediate payout and expiry poller; valid coherent clock required. | Boundary cases before/equal/after plus source-wiring check; GREEN. | Native event/time ordering around both boundaries; no fresh window after expired execution. |
| HP-14 | Acceptance assigned Investigating absolutely. Current pursuit and other delivery-phase requirement form a monotonic minimum. | `RuntimeRulesTests` None/Investigating/NonLethal/Lethal combinations GREEN. | Actual law API effects with concurrent contracts and second acceptance during lethal. |
| HP-15 | New short cooldown overwrote longer existing end. Maximum of existing and newly calculated end; acceptance rechecks current availability. | `CallsLifecycleTests` 14→3, 3→14 and exact availability boundary GREEN. | Respawn/stale response/caller interaction; no natural exploit claimed. |
| R-01 | Save-info scan selected data and stale slot fallback could write foreign state. Metadata scan is not installation; bounded completed-load retry binds identity/path and validates at write. | Persistence model and source-wiring checks GREEN; retry deadline bug reproduced and repaired. | Menu-only scans, A→B→A, same-slot new world and actual path/identity logs. |
| R-02 | Host/client ownership protocol absent. Owner chose rejection instead: SP-only readiness gates, visible tracker status and local-only notice; no shared-service safety claim. | Readiness/status callsite checks GREEN; native lobby/player-count wrapper signatures inspected. | Host+client unsupported/rejection checks; never advertised as supported MP. |
| R-03 | Cached day plus fresh minute/error zero. Read day/minute/day consistently; reject mixed/invalid samples, never treat failed read as valid minute zero. | `RuntimeRulesTests` day-change, exception, invalid clock and valid sample GREEN. | Actual midnight, sleeping and slot transition timing. |
| R-04 | Callback-only offers and shared-thread overwrites. Stable single-use offer, one outstanding decision, preserved responses on notices and invalidation on suspend. | `CallsLifecycleTests` current/stale/resolved/superseded states GREEN; native callsites read. | Real S1API response-button retention and partial send behavior. |
| R-05 | Acceptance used stale reference/existence. Stable-ID re-resolution + current alive/conscious/eligibility/capacity/cooldown checks. | Acceptance policy tests GREEN; native caller route read. | Native state semantics (including sleep). After acceptance state-based recognition is retained; no exclusive attacker attribution was added. |
| R-06 | Explicit null collections/rows reached iteration. Central normalization before use; legacy reads do not rewrite/delete sources. | `PersistenceRegressionTests` explicit null collections/rows/string normalization GREEN. | Representative real old/corrupt files and controlled IO errors; this is not a general repair of arbitrary malformed numeric data. |
| R-07 | Frame-based death polling/early render-key commit. Monotonic elapsed polling + loaded baseline; commit render key only after successful rebuild; structural phase display refresh. | Death source contracts, tracker render-gate unit/source cases and Debug reset RED/GREEN; GREEN. | Actual low-FPS death/respawn, transient Unity UI failure and visible countdown. Long-save profiling unperformed; no unmeasured optimization claim. |

## Residual limitations and compatibility decision

- Compilation establishes compatibility with installed 0.4.7f12 / S1API 3.2.1 references,
  not correctness of their native hooks. MONO is not a supported output of this workspace.
- Native save-info/loaded/request directory equality, SaveError reset behavior, callback
  ordering and load-time native JSON rewriting remain unproven. A foreign root JSON sidecar
  or changed native content deliberately pauses instead of accepting an arbitrary snapshot.
- A legacy write-through source may already be ahead of the restored world. Initial import
  cannot repair that history automatically. No migration deletes old IDs/tokens or original
  files. New flags default false; pending delivery, checkpoint and uncertain-payment state
  use the existing field-aware JSON model.
- Unknown payment effect is an explicit unresolved state, not a successful payout or proof
  of zero movement. Claim/photo stays; manual review/recovery is necessary. No blind flag
  clear, automatic compensation, repay or forfeit is safe. No in-game override/resolver is
  shipped; this deliberately fail-closed state may occupy capacity until reviewed/recovered.
- The original completion criterion remains **open** until the native Release chain and
  saved reload/slot roundtrip above are executed. Use a disposable backed-up test world,
  not the owner's only production save. No commit/push or automatic game start performed.

## Observed session evidence — 2026-10-19 16:14–16:37 (read-only)

Scope: a real session with the deployed `0.3.0-dev.1` artifact. It is **not** the normal
Release chain and **not** evidence for payment, evidence restoration, slot switching or MP
rejection. The loaded bounty state was empty (`active=0`, `history=0`, no kill events), so
the only new native observation is an empty-state checkpoint restore → save commit → reload
restore. All earlier open items remained open at that point; the owner confirmation of
2026-10-24 later accepted them (see the status header).

### Loaded artifact identity

- Session: `Latest.log` 16:14:42.572–16:37:54.030; game **0.4.7f12**, Unity 2022.3.62f2,
  MelonLoader 0.7.3, S1API 3.2.1.
- `[16:14:44.482] Melon Assembly loaded: '.\Mods\HitmanPhone.dll'`
  `[16:14:44.482] SHA256 Hash: 'AA48CA59B3DF56368BB878D4837DBF4196C7FD8E1C599BED3F2B1B419EAC4A95'`
- Banner at `[16:14:45.123] HitmanPhone v0.3.0-dev.1`; initialized at
  `[16:14:48.827] ... initialized (v0.3.0-dev.1) — Phase J: slot-switch detection enabled.`
- Current installed `Mods\HitmanPhone.dll` re-hashes to
  `aa48ca59b3df56368bb878d4837dbf4196c7fd8e1c599bed3f2b1b419eac4a95` (123,904 bytes,
  mtime 2026-10-19 09:09). Installed PDB
  `419652cf3e5ec83136d0a490dbd4255fe44a010e7f2523a35109375c2c1f3df3` and
  `UserData\HitmanPhone\mod.json`
  `20f35813cf8c865e19f98b4beaf7825728405e704835f71b13ca6c17508a6a0f` also match this
  record; `mod.json` reads `0.3.0-dev.1`. This proves artifact identity, not native correctness.
- A later rebuild at `Source/Mods/HitmanPhone/src/bin/Release/net6.0/HitmanPhone.dll`
  (mtime 19:22) hashes to
  `aad5679242acd335eadd3b28058ca60bd5a754b06cc96f257028c3f86e770f95` — **different** from
  the loaded/installed artifact. Do not treat the current build output as byte-identical to
  what ran.

### Checkpoint restore, commit and reload (empty state)

- `[16:16:11.936] [HitmanPhone] [HitmanPhone] [checkpoint] restored 0F4E599ECDD4 for the confirmed world from 'C:\Program Files (x86)\Steam\steamapps\common\Schedule I\UserData\HitmanPhone\checkpoints\2858716C9BE214CC963BFC3B950ECF358E07D0E2323A77FDA6DE429D5ACDDCB0\0F4E599ECDD4680EC2044AEACA869A2B87DCDB49C2A32FBCD4E2173FA19847A7.json'.`
- `[16:16:11.945] ... Active quest rebind complete: 0 of 0 active contract(s).`
- `[16:16:11.946] ... [context] ready slot=1, active=0, history=0; checkpoint mode.`
- `[16:34:40.463]` and `[16:34:40.502] ... [checkpoint] native save destination verified:
  loaded='...\Saves\76561199109519851\SaveGame_1', info='...\SaveGame_1',
  request='...\SaveGame_1'.` Two destination-verified events occurred 39 ms apart,
  consistent with both instrumented `Save` overloads; the log alone does not attribute each
  line to a specific overload.
- `[16:34:40.505] ... [checkpoint] save-start snapshot frozen; bounty mutations paused.`
- `[16:34:40.921] ... [checkpoint] committed 864CED1C27E9; native save complete.`
- After reload: `[16:37:26.844] ... [checkpoint] restored 864CED1C27E9 for the confirmed
  world from '...\checkpoints\2858716C...\864CED1C27E9034532762EBAA76EE61B585F93337CDA11728164C0672591A3B4.json'.`
- `[16:37:26.845] ... Active quest rebind complete: 0 of 0 active contract(s).` and
  `[16:37:26.845] ... [context] ready slot=1, active=0, history=0; checkpoint mode.`
- Read-only file checks: `0F4E...json` is 1,795 bytes, mtime 15:26:16.608 (pre-session);
  `864C...json` is 2,010 bytes, mtime 16:34:40.919 (matches the commit within 2 ms). Both
  parse with `Active=[]`, `History=[]`, `KillEvents=[]`, `NextEvidenceToken=1`; their
  `WorldCheckpoint` values equal their filenames, and `LastSaveDay` is 1 and 2 respectively.
  The reload therefore selected the checkpoint created by this save, not the older
  pre-session snapshot.

### Observed but not acceptance evidence

- `[16:15:49.239] [WARNING] [HitmanPhone] ... [BountyRouter] Multiplayer is not supported —
  bounty calls are disabled in this session.` This came from the early
  `NetworkGuard.IsInMainScene && !SaveStateGuard.IsSingleplayer` notice path before the guard
  was ready. The log contains no lobby, player-count, host/client or rejection action, so
  this is **not** R-02 multiplayer rejection acceptance.
- `[16:26:18.889] ... [Receipt] Storage write hook alive — first storage event observed.`
  This is hook liveness only; no receipt, drop, photo or payout existed in the empty state.
- No HitmanPhone `ERROR`/exception lines appear among the 31 HitmanPhone log lines. Absence
  of an error does not prove any path that never ran.
- Not exercised: normal offer → accept → KO → photo → assigned drop → payout, wrong-drop /
  full-inventory cases, payment-uncertainty path, photo/registry restore, journal phase/GUID
  persistence, slot switching, and MP rejection. The empty checkpoint state limits this
  session to save-boundary behavior.
