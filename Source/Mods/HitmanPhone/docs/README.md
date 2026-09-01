# HitmanPhone 0.2.0 (2026-09-01)

Lategame bounty mechanic for Schedule I 0.4.6f13 (TVGS).
Anonyme Kopfgeld-Anrufe über das Nachrichten-System, Polaroid-Beweiskette,
Dead-Drop-Einzug, schmutziges Bargeld, Journal-Quests und Police Heat.

1. **Caller System** — Anonymous callers (Ghost/Jackal/Magpie/Viper/Crow) reach
   out via the in-game Messages app. Three styles: cold, threatening,
   desperate. Multiple bounties can be active. Since 0.2.0 the caller is drawn
   randomly from the pool of callers off cooldown.
2. **Polaroid Evidence** — On killing a generic NPC flagged as a bounty target,
   a polaroid stamped with the target's Unity InstanceID spawns in the player's
   inventory. KO counts as a kill by design (watchdog tolerates OnDie/KnockOut).
3. **DeadDrop Receipt** — Depositing the polaroid in any dead drop completes the
   contract. Since v0.1.7 the hooks ride `SetStoredInstance_Internal` +
   `SetItemSlotQuantity_Internal` (the dead-drop UI never fires ContentsChanged).
4. **Dirty-Cash Payout** — Since v0.1.9 the bounty pays physical cash via
   `ChangeCashBalance` (no bank ledger record — no paper trail). Since 0.2.0
   the reward is rolled per style (8k–45k) and travels structurally into the
   contract; a failed payout leaves the contract active and retries.
5. **Police Heat** — Accepting a contract raises `CrimeData.PursuitLevel` to
   `Investigating`; completing it drops pursuit to `None`. The grace window is
   session-local (not persisted).

## Console Commands (S1API console)

- `/hitman_force_offer [callerIdx=0..4] [targetNpcId]` — manually fire a call (default: caller 0, target `ludwig_meyer`).
- `/hitman_status` — dump all observability counters (matches found, polaroids spawned, payouts issued, etc.).
- `/hitman_kill <npcId>` — simulate a kill and spawn a polaroid for testing.
- `/hitman_cleanup` — cancel orphaned generic-titled "Hitman Contract" journal quests (`confirm` required when contracts are active).
- `/hitman_reset` — wipe the per-slot `BountySaveData` (cannot be undone).

## Save Format

Per-slot JSON in `UserData\HitmanPhone\bounties_slot_{n}.json` (field-aware
`System.Text.Json` options — `IncludeFields` is mandatory, see
`BountyPersistence.FieldJsonOptions`). Migrates from pre-slot
`<ModFolder>/bounties.json` if present. Atomic write + `.bak` via SafeStorage.

## Known Limitations

- The polaroid's encoded target id (`IntegerItemInstance.Value`) is wiped by
  the dead-drop UI transfer; a single-awaiting-contract fallback resolves it,
  2+ simultaneous awaiting contracts are refused fail-safe.
- Cross-session receipts rely on that fallback (persisted InstanceIDs are
  invalidated on every load — audit M2).
- 65 generic NPC IDs hardcoded in `TargetSelector.cs` — must be maintained on
  game updates.
