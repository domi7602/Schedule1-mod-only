# Changelog


## 0.2.3 (2026-09-11)
- Deadline-Boundary >= (exakt 3 Tage wie im Journal versprochen).
- ConsumePolaroids frisst nur noch 1 Slot (zweite Contracts-Evidence ueberlebt).
- OnDecline mit Stale-Save-Guard (kein Cooldown im falschen Save).
- NPC-Tod-ohne-Contract auf Debug (kein Log-Spam).

## 0.2.2 (2026-09-10)
- Bounty-Payout host-only (Fix Multiplayer-Doppel-Reward).
- Blindes PatchAll durch PatchGuard.TryPatch ersetzt; CurrentDay() cached mit 1s-Throttle; CooldownCaller persistiert.
- Deadline-Boundary (volle 3 Tage), Evidence-Cache mit Liveness-Check, Subscribe-Guard, Log-Demote, NPC-Alive-Check, Test-Commands nur noch in DEBUG.

## Unreleased

- **Hitman-Fix:** neue Angebote zahlen zufällig **200–500** Cash.
- **Hitman-Ziele:** ein Angebot wird nur noch für einen lebenden Kunden aus
  `Dealer.AllPlayerDealers → AssignedCustomers` erzeugt; allgemeine Zivilisten
  aus einer festen ID-Liste sind ausgeschlossen.
- **Hitman-Heat:** ein bestätigter Kill setzt die Verfolgung sofort auf
  `PursuitLevel.Lethal`. Die alte Investigating-Grace-Logik wurde entfernt.

- **Journal-Ziel nach Save-Reload:** Aktive Hitman-Verträge werden nach dem
  Laden wieder an die restaurierte Journal-Quest gebunden. Dadurch erscheinen
  Zielname und Eliminierungsauftrag nach einem Neustart wieder korrekt.
- **Neues Spiel im gleichen Slot:** Die Mod speichert jetzt die Identität des
  Vanilla-Spielstands und verwirft alte Hitman-Verträge, wenn ein neuer
  Spielstand denselben Slot wiederverwendet.
- **Späte Quest-Restaurierung:** Der Journal-Rebind wird bis zu achtmal in
  einem begrenzten Zeitfenster wiederholt, falls S1API die Quest später als
  `OnLoadComplete` wiederherstellt.

## 0.2.1 (2026-09-07)

Dialog-Fixes für den Bounty-Router (drei Restbefunde aus der 0.2.0-Audit-Runde,
gefunden bei der Quest-Dialog-Durchsicht am 2026-09-07):

- **Bug 1 — MoreInfo-Deadline hardcodiert:** die Info-Nachricht behauptete
  immer "Three days window.", selbst wenn `ContractDeadlineDays` anders
  steht. Jetzt wird die Zahl (wie schon L7 in OnAccept) aus der Konstante
  abgeleitet — Config und Dialog können nicht mehr auseinanderlaufen.
- **Bug 2 — MoreInfo zeigte falschen Reward:** statt der echten gewürfelten
  Summe (`rewardCash`, dieselbe Zahl wie im Angebot und im Contract) zeigte
  die Info-Nachricht einen statischen Pay-Range pro Caller
  (`DefaultRewardFor`, z.B. "$18 000 – $45 000") — konnte dem Angebot
  widersprechen. `DefaultRewardFor` ist komplett entfernt.
- **Bug 3 — Stilles Verfallen nach Reload:** der H3-Stale-Save-Guard machte
  Accept/MoreInfo nach einem Reload stumm zunichte (nur Log-Zeile, Buttons
  blieben sichtbar). Jetzt sendet der Caller die Nachfrage-Nachricht
  "[Ghost]: Forget it. The deal is off." (`SendOfferExpiredNotice`), damit ein
  toter Button nie als kaputtes Mod missverstanden wird.

## 0.2.0 (2026-09-01)

Audit-Runde (OpenCode bug audit, alle 22 Funde gegen den Code verifiziert;
21 bestätigt und gefixt, 1 als Absicht dokumentiert).

**Hoch (Datenverlust / Kern-Loop):**
- **H1:** Neues `AwaitingDrop`-Feld (persistiert, überlebt Reloads) ersetzt
  `EvidenceSpawned` als Gate und Fallback-Basis des Dead-Drop-Receipts. Vorher
  brachte jeder Reload (auch Menü→Resume) die Auszahlung zum Schweigen.
  `EvidenceSpawned` wird weiterhin bei jedem Load resettet — eine verlorene
  Polaroid-Instanz kann wieder verdient werden.
- **H2:** Reward wird strukturell an `OnAccept` übergeben statt aus dem
  Nachrichtentext zurückgeparsert. Der alte Parser konnte NIE matchen (die
  Style-Templates enthalten kein `$`), jeder Contract zahlte daher den
  $10k-Fallback — jetzt zahlen die gewürfelten 8k–45k.
- **H3:** Accept/MoreInfo-Callbacks lösen `Mod.Instance.Save` zur Klickzeit auf
  und verwerfen das Angebot, wenn das Save-Objekt zwischen Angebot und Klick
  gewechselt hat (Reload/Slot-Switch) — kein stiller Contract-Verlust mehr.
- **H4:** Payout VOR State-Transition. `IssueReward` liefert bool; bei
  Fehlschlag bleibt der Contract Active und der nächste Storage-Event wiederholt
  den Versuch (vorher war der Reward bei Fehlschlag dauerhaft weg).

**Mittel:**
- **M1:** Heat-Grace nur noch im RAM (in-memory Dictionary), nie persistiert;
  alte `hitman_grace_until_*`-Keys werden beim Load aus dem Save geputzt.
- **M2:** Persistierte `TargetNpcInstanceId`s werden bei jedem Load auf 0
  gesetzt — stale Unity-IDs können nicht mehr zufällig einen fremden NPC
  treffen.
- **M3:** Generische Titel-Adoption nur noch bei GENAU einem verwaisten
  Active-Contract; bei 2+ wird verweigert (kein falsches Cross-Binding mehr).
- **M4:** Zufälliger Caller aus dem Pool derCooldown-freien Caller (vorher
  immer Ghost/erster freier, Stil immer "cold").
- **M5:** Day-Latches (`_lastFiredDay`, `_lastCheckedDay`) werden beim
  Slot-Switch zurückgesetzt.
- **M6:** Expired/Forfeited wandern nach History (vorher blieben sie ewig in
  Active; Liste und Save wuchsen unbegrenzt).
- **M7:** Angebot zeigt formatierten Namen (`Ludwig Meyer`) statt roher ID.
- **M8:** Spawn-Fail-Warn auf 30s gedrosselt (Watchdog pollt alle 1,5s).
  KO-zählt-als-Tod bleibt ABSICHT (Klassenkommentar Phase C Alternative).
- **M9:** Driftender `_activeBounties`-Zähler entfernt; `ActiveBounties`
  zählt live aus dem Save.

**Niedrig:** README aktualisiert (L1), MONOMELON-Guards in
BountyPersistence/TestCommands (L2), Reflection-Cache in `CurrentDay` (L3),
Kommentar-Rot korrigiert + toter Defensiv-Restore entfernt (L4), toter Code
entfernt: `BuildOfferMessage`, `MaxConcurrentOffersPerDay`, private
Cooldown-Konstanten, `HeatFlagKey`, `is var v ? v : v` (L5),
`TryMigrateLegacy` läuft einmal pro Session (L6), "Three days." aus der
Deadline-Konstante (L7), LoadManager-`-1`-Pfad wedged nicht mehr (L8),
`OnUpdate`-Catch loggt vollen Stacktrace (L9).

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
