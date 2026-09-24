# Changelog



## 0.2.9 (2026-09-15)
- Host-authority check consolidated into `S1Mods.Shared.NetworkGuard.IsHostOrSingleplayer`; the previously local fail-open branch (catch => true) has been removed. When an authority exception occurs without an owned save, the payout is now aborted instead of blindly continuing. The MONOMELON dead branch was removed along with it (workspace builds exclusively IL2CPP).
- **Schedule I 0.4.7f6 (Open Beta) compatibility — `NPCDeathPatch`:** vanilla `NPCHealth.npc` no longer exists on the beta game assembly (CS1061 against the live 0.4.7f6). The patch now resolves the owning NPC via `GetComponent<S1NPC>()` on the health component — the same pattern S1API uses. Local decompiles in `GameReferences/` still reflect 0.4.6f13 and show the old field.
- **DEBUG-only offer host fallback (`BountyConversationRouter`):** while S1API 3.2.1-beta.2 could not instantiate `HitmanCallerNPC` (upstream ifBars/S1API issue #309 — "no framework data object", fixed in 3.2.1-beta.5), `SendOffer`/`OnAccept`/`OnMoreInfo` fall back under `#if DEBUG` to the target's thread (tier 1: `NPC.Get(target.ID)`) or, if that fails too, to the target's native `MSGConversation` (tier 2). Release builds stay caller-only. With S1API 3.2.1-beta.5 deployed the fallback is dormant — verified live 2026-09-24: offer (Unknown Number thread) → accept → receipt → payout, 0 warnings.
- **Test commands (DEBUG):** `/`-aliases for all five hitman console commands (`hitman_force_offer`, `hitman_status`, `hitman_kill`, `hitman_reset`, `hitman_cleanup`) plus a DEBUG-only eligibility bypass for `hitman_force_offer` (scheduler guard skipped).
- Documentation (CHANGELOG/README) translated to English.
- Version bump. Tested dependency: S1API 3.2.1-beta.5 (deployed 2026-09-23).

## 0.2.7 (2026-09-14) — Vanilla UI title sync for restore quests

- **Bug:** Restore bounty quests persistently render in the journal as "Hitman Contract" instead of "Hitman Contract: <NPC>" — even after the v0.2.6 reflection setter on `s1q.title`. The UI snapshot in the vanilla QuestComponent was set once in the ctor and never retriggered.
- **Fix:** `BountyQuest.SyncDisplayTitle()` runs automatically on every `InitContractId(...)`. Tries the public title setter first (if present — it fires the `_onTitleChanged` Unity event), otherwise writes directly to the title field plus re-`Begin()`.
- **Effect:** The same-target group (3 Ludwig quests in live test) now shows "Hitman Contract: Ludwig Meyer" 3×. Freshly accepted quests (Sam, Chloe) were already OK.
- **Bridge:** `BountyJournalBridge.TryRefreshDisplayTitle` is now a thin wrapper around `BountyQuest.SyncDisplayTitle`.

## 0.2.6 (2026-09-14) — Journal rebind for same-target groups (log spam after restart)

- **Group-aware quest rebind (MEDIUM):** The audit-M3 fail-safe refused any binding with 2+ restored contracts on the same NPC — consequence was the 40-line warning loop (8 retries × 5 lines) on every game start. Now: if ALL unbound active contracts share the same `TargetNpcId`, the oldest adopts the restored generic quest (`GetQuestByName("Hitman Contract")`), further restore quests are adopted via reflection over `QuestManager.Quests` (no zombie journal entries), and contracts without a persisted quest get FRESH quests via `RegisterBountyQuest`. Mixed-target groups stay refused (real ambiguity — wrong binding is worse than none).
- **Occupancy guard in title lookup (step 2):** In a same-target group, the owner quest's title becomes personalized through adoption — siblings reconstructed the same title and would have stolen the quest (M3 bug through the backdoor, latent since 0.2.5 also for third contracts in the same session). `IsQuestBoundToOtherContract` blocks that; SessionQuests is the occupancy source since only HitmanPhone calls `InitContractId`.
- **Display title refresh after adoption:** `InitContractId` only personalizes the managed `Title` getter; the vanilla snapshot `S1Quest.title` (journal UI) stayed generic. `TryRefreshDisplayTitle` writes the title again after adoption (reflection, best-effort, cosmetic).
- **Log dampening in the retry window:** `Could not rebind` warnings only on the first and last attempt (was: full 5-line blocks every 500 ms).
- **Cleaned up:** dead helper `CountActiveContractsWithoutSessionQuest` removed (replaced by `CollectUnboundActiveContracts`).

## 0.2.5 (2026-09-13) — Double-Ludwig fix (bug-ludwig-double)
- **Cross-session double-contract refuse resolved (MEDIUM):** `TryValidateAndPay` refused every polaroid drop when >1 contract was simultaneously `AwaitingDrop=true`. Crow however offers new bounties on the same NPC for days on end (e.g. ludwig_meyer day 5+6). After save reload the polaroid's `Value=0` (Unity InstanceID gone) and the fallback failed safely. Fix: when all waiting contracts target the same `TargetNpcId`, the whole group is paid out together (one kill = one piece of evidence = multiple bounties). For heterogeneous TargetNpcIds the old refuse log remains active. `OnStorageContentsChanged` now runs the side effects (reward, journal, heat, cooldown) per contract and consumes ONE polaroid for the whole group. Persist once per batch, unless a payout failed (retry path).
- **Skill note added:** `schedule1-il2cpp-sorting-patterns` now documents the `group-on-shared-target` match pattern as the canonical solution for same-NPC multi-contract cases.

## 0.2.4 (2026-09-12) — Bug-audit fixes round 2 (audit 2026-09-12)
- **Client payout deadlock resolved (MEDIUM):** `OnStorageContentsChanged` completely blocked the client player from polaroid payout (host-only gate + host-local save without the client's AwaitingDrop entry). Fix: client may validate and pay with its own save (`ChangeCashBalance` syncs player balance via FishNet; polaroid consume runs in the dead drop on the client). Host stays in charge of contract acceptance logic.
- **KO counted as kill prevented (MEDIUM):** `BountyTargetWatchdog` now checks `npc.Health.IsDead` (real death) instead of `!npc.IsConscious` (also true when KO'd). A stunned target no longer triggers Lethal Pursuit + no full reward. Reward remains on the `OnNpcDied` path.

## 0.2.3 (2026-09-11)
- Deadline boundary >= (exactly 3 days as promised in the journal).
- ConsumePolaroids now eats only 1 slot (second contract's evidence survives).
- OnDecline with stale-save guard (no cooldown in the wrong save).
- NPC death without contract downgraded to debug (no log spam).

## 0.2.2 (2026-09-10)
- Bounty payout host-only (fix multiplayer double reward).
- Blind PatchAll replaced with PatchGuard.TryPatch; CurrentDay() cached with 1s throttle; CooldownCaller persisted.
- Deadline boundary (full 3 days), evidence cache with liveness check, subscribe guard, log demote, NPC-alive check, test commands only in DEBUG.

## Unreleased

- **Hitman fix:** new offers pay randomly **200–500** cash.
- **Hitman targets:** an offer is now only generated for a living customer from
  `Dealer.AllPlayerDealers → AssignedCustomers`; generic civilians from a fixed
  ID list are excluded.
- **Hitman heat:** a confirmed kill immediately sets pursuit to
  `PursuitLevel.Lethal`. The old investigating-grace logic was removed.

- **Journal target after save reload:** Active hitman contracts are re-bound
  to the restored journal quest after load. As a result, the target name and
  elimination order appear correctly again after a restart.
- **New game in the same slot:** The mod now stores the vanilla save's
  identity and discards old hitman contracts when a new save reuses the
  same slot.
- **Late quest restoration:** The journal rebind is repeated up to eight
  times within a bounded time window in case S1API restores the quest
  later than `OnLoadComplete`.

## 0.2.1 (2026-09-07)

Dialog fixes for the bounty router (three remaining findings from the
0.2.0 audit round, found during the quest-dialog review on 2026-09-07):

- **Bug 1 — MoreInfo deadline hardcoded:** the info message always claimed
  "Three days window.", even when `ContractDeadlineDays` was set differently.
  The number is now derived (as L7 in OnAccept already did) from the
  constant — config and dialog can no longer drift apart.
- **Bug 2 — MoreInfo showed the wrong reward:** instead of the real rolled
  sum (`rewardCash`, the same number as in the offer and contract) the info
  message showed a static pay range per caller
  (`DefaultRewardFor`, e.g. "$18,000 – $45,000") — it could contradict the
  offer. `DefaultRewardFor` has been removed entirely.
- **Bug 3 — Silent expiry after reload:** the H3 stale-save guard silently
  voided Accept/MoreInfo after a reload (only a log line, buttons stayed
  visible). The caller now sends the inquiry message
  "[Ghost]: Forget it. The deal is off." (`SendOfferExpiredNotice`), so a
  dead button is never mistaken for a broken mod.

## 0.2.0 (2026-09-01)

Audit round (OpenCode bug audit, all 22 findings verified against the code;
21 confirmed and fixed, 1 documented as intentional).

**High (data loss / core loop):**
- **H1:** New `AwaitingDrop` field (persisted, survives reloads) replaces
  `EvidenceSpawned` as the gate and fallback basis of the dead-drop receipt.
  Previously every reload (also menu → resume) silenced the payout.
  `EvidenceSpawned` is still reset on every load — a lost polaroid instance
  can be earned again.
- **H2:** Reward is structurally passed to `OnAccept` instead of being parsed
  back from the message text. The old parser could NEVER match (the style
  templates contain no `$`), so every contract paid the $10k fallback — now
  it pays the rolled 8k–45k.
- **H3:** Accept/MoreInfo callbacks trigger `Mod.Instance.Save` at click time
  and discard the offer when the save object has changed between offer and
  click (reload / slot switch) — no more silent contract loss.
- **H4:** Payout BEFORE state transition. `IssueReward` returns bool; on
  failure the contract stays Active and the next storage event retries the
  attempt (previously the reward was permanently lost on failure).

**Medium:**
- **M1:** Heat-grace now only in RAM (in-memory dictionary), never persisted;
  old `hitman_grace_until_*` keys are cleaned out of the save on load.
- **M2:** Persisted `TargetNpcInstanceId`s are reset to 0 on every load —
  stale Unity IDs can no longer randomly hit a foreign NPC.
- **M3:** Generic title adoption now only with EXACTLY one orphaned active
  contract; with 2+ it is refused (no more incorrect cross-binding).
- **M4:** Random caller from the pool of cooldown-free callers (previously
  always Ghost / first free, style always "cold").
- **M5:** Day latches (`_lastFiredDay`, `_lastCheckedDay`) are reset on
  slot switch.
- **M6:** Expired/forfeited move to history (previously they stayed in active
  forever; list and save grew without bound).
- **M7:** Offer shows formatted name (`Ludwig Meyer`) instead of raw ID.
- **M8:** Spawn-fail warning throttled to 30s (watchdog polls every 1.5s).
  KO-counts-as-death remains INTENTIONAL (class comment Phase C alternative).
- **M9:** Drifting `_activeBounties` counter removed; `ActiveBounties`
  counts live from the save.

**Low:** README updated (L1), MONOMELON guards in
BountyPersistence/TestCommands (L2), reflection cache in `CurrentDay` (L3),
comment-rot corrected + dead defensive restore removed (L4), dead code
removed: `BuildOfferMessage`, `MaxConcurrentOffersPerDay`, private
cooldown constants, `HeatFlagKey`, `is var v ? v : v` (L5),
`TryMigrateLegacy` runs once per session (L6), "Three days." derived from the
deadline constant (L7), LoadManager `-1` path no longer wedges (L8),
`OnUpdate`-catch logs full stack trace (L9).

## 0.1.9 (2026-09-01)

Thematic change — request by Dominik: "Kopfgeld ist im echten Leben dreckiges
Geld" (bounty money is dirty money in real life).

- **Payout switched from online balance to physical cash:** `IssueReward` now
  calls `S1API.Money.Money.ChangeCashBalance(amount, visualizeChange: true,
  playCashSound: true)` — the HUD shows the cash change and the register chime
  plays. Fitting side effect of dirty money: cash leaves no record in the
  banking ledger, unlike the 0.1.8 `CreateOnlineTransaction` path.
- **Field rename `RewardOnline` → `RewardCash`:** contract persistence now
  stores the reward under its true meaning. Old saves stay loadable — a
  setter-only Newtonsoft shim reads the legacy `"RewardOnline"` JSON key into
  `RewardCash` (writes nothing back, so new saves carry only the new name).
- Caller-facing text was already neutral ("Pay: $10,000 — Drop a photo of the
  body in any dead-drop"), so no dialog changes were needed.
- **CRITICAL persistence fix (found during the rename):** every save since
  v0.1.3 wrote the literal `{}` — `BountySaveData` consists of public FIELDS,
  but System.Text.Json does not serialize fields unless `IncludeFields` is
  set, and BountyPersistence used SafeStorage's default options (proven by
  inspecting `bounties_slot_0.json`). It never surfaced because the bounty
  cycle ran within one session; a game reload mid-contract silently wiped all
  bounty state. BountyPersistence now passes field-aware options to BOTH save
  and load. Roundtrip-tested standalone: new saves carry every field
  (`RewardCash` etc.), the legacy `RewardOnline` key still loads, and the
  set-only legacy shim is never written back. Other mods are unaffected —
  their save models use properties (verified: HomelessMod/BusinessIncome/
  AutoPackagingStation files contain full data).

## 0.1.8 (2026-09-01)

Bugfix release — seventh live-test follow-up. The v0.1.7 hook fix WORKED
(live log: "DeadDrop 'Taco Ticklers exterior wall' contents changed — scanning
inventory" → "Receipt matched ... Reward=$10000" → quest Completed), but two
defects surfaced:

- **Payout never executed (critical):** `IssueReward` resolved
  `MoneyManager.Instance` via `Type.GetProperty` on the MoneyManager type — but
  the property is declared on the `NetworkSingleton<>` base class, so GetProperty
  on the derived type returned null and every payout died as
  "MoneyManager.Instance is null; skipping payout" while the contract was
  already marked Completed (live log 06:53:05). Replaced with the typed
  `S1API.Money.Money.CreateOnlineTransaction` wrapper — the same call
  BusinessIncome and PocketShop use live. A null-MoneyManager guard remains for
  observability.
- **Polaroid never destroyed:** evidence stayed in the dead drop forever and
  every later storage write re-scanned it (live log: repeat scans reporting
  "no payable polaroid (2 items)"). New `ConsumePolaroids` clears every
  polaroid slot via `ItemSlot.ClearStoredInstance()` — the same soft-removal
  path S1API's `RemoveAllOfDefinition` uses (no world item spawns).
- **Log-spam gate:** storage writes are frequent (every inventory move in any
  UI fires 2–3 of our hooks in singleplayer — server and observers RPCs both
  run locally). `OnStorageContentsChanged` now returns early when no active
  contract awaits evidence, silencing the no-op scan noise outside bounty
  phases. The one-shot "hook alive" diagnostic still fires first.
- **Observability note:** the polaroid's encoded `Value` (target instance id)
  arrives as 0 after the UI transfer — the item instance is re-created by the
  dead-drop UI and the integer value does not survive. The single-awaiting-
  contract evidence fallback covered it live; with 2+ simultaneous awaiting
  contracts the match stays refused (fail-safe, documented limitation).

## 0.1.7 (2026-09-01)

Bugfix release — sixth live-test follow-up. Symptom (Dominik, screenshot):
quest stays at "Drop the polaroid at any dead-drop" after inserting the
polaroid; the log contains NO `[Receipt]` line at all.

- **Root cause (proven by interop dump + log):** the v0.1.6 hook targeted
  `StorageEntity.ContentsChanged()`. The method exists (interop dump:
  `ContentsChanged() : Void`) and the patch applied cleanly — but the game
  never CALLS it for dead drops. `ContentsChanged` is serviced by
  `UpdateWhileOpen`, which only runs for storages opened through the regular
  storage UI (`Open()`); the dead-drop UI never opens the storage — it writes
  slots through the FishNet RPC chain
  (`SetStoredInstance` → `SetStoredInstance_Internal`,
  `SetItemSlotQuantity` → `SetItemSlotQuantity_Internal`).
- **Fix:** the three hooks are now registered explicitly via the house
  standard `PatchGuard.TryPatch` (Mod.cs): `ContentsChanged` (kept, catches
  local `InsertItem` writes), `SetStoredInstance_Internal` (full instance
  write — the dead-drop UI path) and `SetItemSlotQuantity_Internal`
  (quantity-only write). Every slot write in the game flows through one of
  the two `_Internal` methods regardless of which UI issued it. `DeadDropPatch`
  is now a plain postfix holder (no attribute patching); `PatchAll` still
  handles NPCDeathPatch.
- **Startup proof:** PatchGuard logs a warning the moment a target method is
  missing or a patch fails; three warnings ABSENT at startup = all three hooks
  armed. The one-shot "[Receipt] Storage write hook alive" line then proves the
  postfix chain end-to-end without log spam (v0.1.6 lesson: a silent hook is
  indistinguishable from a hook that was never applied — that ambiguity is now
  closed).
- **One-shot diagnostic:** the first storage-write event of a session logs
  "[Receipt] Storage write hook alive", proving the postfix chain end-to-end
  without log spam.

## 0.1.6 (2026-08-31)

Bugfix release — fifth live-test follow-up. Dialog fix proven by log
("More-info sent ... responses re-attached", accept worked). The receipt
chain stayed silent again: NO receipt log at drop time, proving the
dead-drop UI never calls InsertItem.

- **Hook switched from InsertItem to ContentsChanged (critical):** metadata
  analysis of StorageEntity shows the UI fills slots via SetStoredInstance /
  SetItemSlotQuantity, and the engine exposes a ContentsChanged() change
  signal. The patch now fires on ContentsChanged and — when the changed
  storage belongs to a registered dead drop — inventories the storage and
  matches any polaroid against active contracts. Scan-based payout is
  idempotent: the contract leaves Active on success, so repeated change
  events cannot double-pay.
- **Defensive inventory enumeration:** GetAllItems' wrapper type resists
  static naming (CS0234 despite Il2Cppmscorlib referenced); enumeration now
  duck-types via reflection (Count/Length + get_Item) and logs the real
  type name on mismatch.

## 0.1.5 (2026-08-31)

Bugfix release — fourth live-test follow-up.

- **More-info dialog swallowed the decision (bug report by Dominik):** after
  asking "Who is the target?", the follow-up message was sent WITHOUT the
  Response array. S1API attaches answer buttons to the latest message of a
  conversation, so the new message replaced the offer's Accept/Decline
  buttons and the player could neither accept nor decline. `OnMoreInfo` now
  re-attaches the full response set (accept/decline/more-info) to the
  follow-up.

## 0.1.4 (2026-08-31)

Bugfix release — third live-test follow-up. Progress proven by log: accept +
save file + kill + polaroid spawn + quest advance all work now. The receipt
chain stayed silent because the dead-drop filter could never match.

- **DeadDrop identification was structurally wrong (critical):** the filter
  assumed `Economy.DeadDrop` derives from `StorageEntity` and walked the
  inheritance chain. The decompiled stub proves `DeadDrop : MonoBehaviour`
  and it OWNS a `WorldStorageEntity` in its `Storage` field
  (Archive/Temp_DeadDrop.cs:16, :259). The test returned false for every
  object in the game — every insert was silently discarded since 0.1.0.
  Replaced with ownership matching: a storage belongs to a dead drop when
  `DeadDrop.DeadDrops[i].Storage` IS that entity (compared via Unity
  instance id). `GetGuidString` now reads the GUID from the DeadDrop
  component (it never lived on the storage).
- **Diagnostic:** world-storage inserts now log whether they are dead-drop
  owned, so if the UI bypasses `InsertItem` entirely the next test log
  proves it immediately.

## 0.1.3 (2026-08-31)

Bugfix release — second live-test follow-up. Root cause found in the log:
the mod started with `Active=0` and "No existing save found" even though a
contract had been accepted earlier, because NOTHING ever wrote the save file
(`UserData\HitmanPhone\` did not exist at all).

- **Persistence was write-never (critical):** the only `BountyPersistence.Save()`
  call site in the entire mod was the `/hitman_reset` test command. Accepted
  contracts, evidence spawns, completions, expiries and forfeits lived in RAM
  only — every game restart silently wiped them, leaving an orphaned journal
  quest behind while the mod side forgot the contract. New `PersistCurrent()`
  is now called at every state transition: contract accept
  (BountyConversationRouter), evidence spawn (BountyService.OnNpcDied),
  receipt/complete (BountyReceiptService), expiry (BountyExpiryService) and
  player-death forfeit (PlayerDeathWatchdog).
- **Kill with no matching contract is no longer silent:** `OnNpcDied` early-out
  now logs the dead NPC id and the active contract count (previously the exact
  scenario "Ludwig died, nothing happened" was invisible in the log).
- **Version log fix:** init message now reads the version from the MelonInfo
  attribute (logged "v1.0.0" before, because the assembly version is unrelated
  to the mod version and not touched by the bump script).

## 0.1.2 (2026-08-31)

Bugfix release — live game-test follow-up to 0.1.1: journal advance now works,
receipt chain fully observable.

- **Journal (wrong lookup key):** v0.1.1 resolved quests via
  `GetQuestByGuid(contract.Id)`, but S1API treats the CreateQuest string as an
  internal id only — the lookup never matched, even seconds after creation
  (proven by log: "Registered" → "no live quest" 0.8 s apart). Resolution now
  follows the proven HomelessMod pattern: reconstruct the quest TITLE from the
  contract and resolve via `GetQuestByName`, with a session dictionary for
  in-session disambiguation, generic-title re-adoption for restored quests
  (restored instances lose the transient contract id and render "Hitman
  Contract"), and re-creation as last resort.
- **Session cache:** the journal bridge's session dictionary is cleared on
  every completed save load (`ResetSessionCache`).
- **Receipt observability:** every insert into a DeadDrop is now logged with
  the item type; `IsPolaroid` compares case-insensitively and logs the found
  definition id on mismatch. Every rejection path now has a reason in the log.
- **Receipt robustness:** `Value==0` no longer aborts validation early — a
  save reload can lose the encoded instance id; the single-awaiting-contract
  evidence fallback now also covers this case.
- **New console command `/hitman_cleanup`:** cancels orphaned generic-titled
  "Hitman Contract" quests (e.g. after `/hitman_reset`). Guarded: with active
  contracts it requires a `confirm` argument.
- Mod init log now reports the real assembly version instead of a hardcoded
  "v0.1.0".

## 0.1.1 (2026-08-31)

Bugfix release — three fixes for the "polaroid never completes the quest" chain.

- **Journal (stale quest cache):** BountyJournalBridge no longer keeps static
  Quest references; every transition resolves the live quest via
  `QuestManager.GetQuestByGuid(contract.Id)` (skill rule: never cache Quest
  references across save loads). Fixes: contract paid out but the journal
  quest never advanced after a save reload.
- **Evidence latch:** `EvidenceSpawned` is released on every completed load
  (`BountyService.ResetEvidenceFlags()`, called from SaveStateGuard).
  Fixes: contracts bricked forever when the polaroid item was lost (inventory
  full at spawn / dropped / older save) — target kills then never spawned
  evidence again.
- **Cross-session receipt:** dead-drop validation gained an evidence fallback —
  if exactly one Active contract awaits its dead-drop, the deposited polaroid
  matches it even though Unity re-rolled InstanceIDs between sessions.
  Ambiguous (multiple awaiting contracts) is refused, fail-safe.
- On kill-by-id match, the contract's cached `TargetNpcInstanceId` is
  refreshed to the live session value.

## 0.1.0 (2026-08-30)

First public release of HitmanPhone.

- **A** Mod foundation: MelonLoader wiring, S1API GameLifecycle subscriptions,
  per-slot JSON persistence via SafeStorage (atomic + .bak).
- **B** Polaroid evidence item (custom `IntegerItemDefinition`) registered with
  the native Registry; PNG icon shipped as `polaroid.png`.
- **C** Harmony-Postfix on `Il2CppScheduleOne.NPCs.NPC.Die_Void_Private_Void()` —
  on matching kill, spawn the polaroid pinned to the NPC's Unity InstanceID
  into the player's inventory.
- **D** Caller service: five anonymised callers (`Ghost`, `Jackal`, `Magpie`,
  `Viper`, `Crow`) with three dialog styles (`Cold`, `Threatening`, `Desperate`),
  day-gated scheduling, and 3-day caller-cooldowns.
- **F** Harmony-Postfix on `StorageEntity.InsertItem` for DeadDrop validation;
  on matching receipt, `MoneyManager.CreateOnlineTransaction` issues the
  online-balance payout server-authoritatively.
- **G** Heat orchestration: raise / hold / drop `PlayerCrimeData.PursuitLevel` via
  S1API; post-kill 60-second grace window before escalation; failures escalate
  to `NonLethal`.
- **H** Journal integration: every accepted contract becomes a real
  `S1API.Quests.Quest` in `JournalApp`; matches the existing notification flow.
- **I** Auto-expiry: 3-day deadline per contract (Skill Rule §6 "timed").
- **J** Slot-load detection (Skill Rule #16): distinguishes real save-slot
  switches from same-slot Menu→Game scene reloads. Prevents `BountySaveData`
  wipe on every main-menu return.
- **K** Test scaffolding: four `/hitman_*` console commands for manual
  verification and integration tests.
- **L** Initial public release.

## Known Limitations

- Polaroid-item registry uses Reflection on `ID`, `Name`, `StackLimit`, etc.
  because the IL2CPP stub exposes only the `DefaultValue` accessor on
  `IntegerItemDefinition`. Game-version drift that renames these will break
  the `BountyEvidenceItemRegistry.TrySetReflected` set.
- DeadDrop validation falls through silently if the dropped item is not an
  `IntegerItemInstance` carrying the polaroid definition.
- Caller RNG uses `System.Random` per save-load; for true determinism we'd
  need to capture and replay the seed.
