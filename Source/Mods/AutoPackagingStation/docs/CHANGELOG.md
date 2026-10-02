# Changelog - AutoPackagingStation

## 0.3.4 (2026-10-02) - internal cleanup: dead legacy buffer UI removed (-511 lines), perf

- Removed the unreachable custom-buffer API (`TryExtractInputProduct`/`TryExtractInputPackaging`/`TryExtractOutputProduct`, `TryExtractAll`, `TryDepositProduct`/`TryDepositPackaging` - superseded by the native canvas in v0.2.8) plus dead `SpawnStationAt`, `ActiveStations` and the `CreateSaveData(string, ...)` overload.
- Perf: the F-raycast + focus probe run only on F-keydown frames, the Escape focus probe only on Escape frames; the native `PackagingStation` component is cached; `ResolvePackagedItemId` is memoized (invalidated on save load).
- No behaviour change intended. Tests 28/28; 2026-10-02 session smoke: loads + patch clean (log-verified).

## 0.3.3 (2026-09-20) - Fix: Baggie clones floating above the machine during unpack
- **Root cause (live finding, user report):** During auto-unpack, vanilla renders the returned empty packages at the native `PackagingAlignments`/`ActivePackagingAlignent` points. These transforms are NOT part of the slot positions hidden in `HideBaseRenderers` — on our custom station (vanilla chassis hidden), up to 8 "baggie clones" floated freely above the housing and glitched into the machine when the UI opened. Visible only during unpack, since only this flow returns packages.
- **Fix:** `HideBaseRenderers` now additionally parks `ActivePackagingAlignent`, all `PackagingAlignments[]`, and `ActiveProductAlignments[]` invisibly (scale 0, moved below the base) — same technique as for the slot positions. Applies exclusively to AutoPack stations; vanilla machines are unaffected (their display remains intact).
- Storage/inventory data was correct at all times (purely visual issue).

## 0.3.2 (2026-09-20) — Unpack batching (symmetry with pack batch)
- **Unpack in batch:** v0.3.1 unpacked 1 unit per cycle (a single native `Unpack()` call), while auto-pack ran in batches of 10. `ExecuteNativeUnpack` now performs up to `MaxBatchSize` (default 10) native unpack calls per cycle — with a vanilla readiness check (`GetState(EMode.Unpackage) == CanBegin`) before **each** call, so the loop naturally stops when the stack is empty or a return slot is full.
- **Baggie HUD popups explained (no fix needed):** Each unpacked unit returns its empty packaging (vanilla conservation) — the pickup notification in the HUD ("1 Baggie", "2 Baggies", ...) is correct vanilla behavior and proof that materials are conserved. No popup during packing because nothing flows into the player's inventory there.
- Live-verified by user: Unpackage mode works (v0.3.1), E prompt works (v0.2.9).

## 0.3.1 (2026-09-20) — Unpack fix: vanilla mode mirror instead of slot heuristic
- **Root cause of "nothing happens on unpack" (live session 12:36):** v0.3.0 searched for the packaged product in the PRODUCT input slot — but vanilla Unpackage runs in **reverse**: packaged goods lie in the OUTPUT slot, raw product + packaging come out on the left (the red UNPACKAGE arrow in the canvas points left). The heuristic never matched.
- **Fix — two borrowings from vanilla instead of custom logic:**
  1. **Mode mirror:** The canvas mode (Package/Unpackage toggle, the red arrow) is mirrored into runtime data (`UnpackageMode`) while the station is open. After closing, the last selected mode remains authoritative — automation follows the player's choice.
  2. **Readiness via `GetState(EMode.Unpackage)`:** The eligibility check is exactly the vanilla state machine (== `CanBegin`), the same check as the native Begin button — automatically covers slot layout, item match, and output capacity.
- **Consequence:** Pack and unpack are now mode-exclusive (no more priority guessing): Unpackage mode → auto-unpack, Package mode → auto-pack.
- **Operation (still simple):** Open station (E) → switch arrow to UNPACKAGE → place packaged goods in OUTPUT slot → close → station automatically unpacks in cycles.
- Session finding: E prompt fix (v0.2.9) works — 3× `E-interact: calling PackagingStation.Interacted()` in log, canvas opened.

## 0.3.0 (2026-09-20) — Auto-UNPACK (native vanilla implementation)
- **Unpacking implemented — by delegating to TVGS code instead of reimplementing:** Cycle commit detects on its own: product slot holds a packaged `ProductItemInstance` (instance-level `PackagingID`/`AppliedPackaging`, exactly what the vanilla pack machine sets) and packaging slot is empty → auto-unpack. Execution is the native `PackagingStation.Unpack()` call (vanilla slot math, no duplicate on our side).
- **Priority chain in commit:** 1. Pack (when packaging material + raw product present) → 2. Unpack (when packaged product present, no material) → 3. Idle. Decision is made at cycle end based on live slots — self-correcting if the player switches slots mid-animation.
- **Chain runs through:** `CanStationPackage || CanStationUnpack` now drives the advancement logic (state Idle/Blocked/NoPackaging → Packaging) — a stack of 20 baggies is therefore unpacked in successive cycles, not just the first one.
- **Output capacity guard:** Unpack only starts when the raw product fits in the output (empty or same definition with stack room).
- **Diagnostics:** Each native-unpack commit logs before/after slot state (`prod[...] out[...] -> [...]`); if `Unpack()` does nothing (e.g. mode mismatch), a warning with report hint is in the log.
- UI: Canvas instruction label updated ("Auto packs & unpacks in background").
- Based on v0.2.9 (E prompt canvas guard fix).

## 0.2.9 (2026-09-20) — E prompt fix (canvas guard)
- **E prompt on the station was visible but dead:** The InteractableObject listener had a guard `PackagingStationCanvas.Instance != null && !activeSelf` — as long as the canvas singleton was still null (lazy init, before the first vanilla station UI opened), `Interacted()` was silently skipped. The new guard only checks "canvas open FOR THIS station" (Instance + activeSelf + station pointer comparison); null-instance no longer blocks.
- **Improved diagnostic logging:** `E-interact:` info on every `Interacted()` call, warning when canvas does not open after the call (Instance null / inactive), error instead of debug on exceptions — previously invisible in normal sessions.
- Side note: pre-existing CS8625 warning in `AutoPackStore.cs` legacy migration fixed (`null!` for LoadSafe fallback).

## 0.2.8 (2026-09-19) — Bugfix round 7: Placement, interaction, level gate
- **Grid-tile overlap (HIGH):** `ExpandFootprintTo2x2()` clones the footprint tile of the base item and creates 3 additional tiles for a real 2×2 grid (0.5m spacing). Prevents the station from sticking halfway into shelves or other buildables.
- **E interaction on the kettle (MEDIUM):** `SetupPlacedStation()` now adds an `Il2CppScheduleOne.Interaction.InteractableObject` (message, range, onInteractStart → `station.Interacted()`). The native `PackagingStationCanvas` opens reliably when hovering on the station (kettle).
- **Level gate (MEDIUM):** `WithRequiredRank(new FullRank(Rank.Hustler, 1))` — the station is purchasable from Hustler I (after Tier 1 PackagingStation and Tier 2 PackagingStationMk2). No longer available from level 1.

## 0.2.7 (2026-09-13) — Bug-report round 6
Bug-report round 6 (audit 2026-09-13): Host guards for PackUp, OnDestroy refund, all TryExtract*/TryDeposit* paths and OnSaveComplete (9 new IsHostOrSingleplayer guards). MP clients can no longer locally cash out/destroy stations (dupe/desync, critical) and no longer overwrite the host's slot file. Supersedes 0.2.4 (client deliberately fell back to Destroy there).

## 0.2.6 (2026-09-12) — Bug-audit fixes round 4 (audit 2026-09-12)
- **F-key PackUp reentrancy guard:** New `_packingUp` flag plus `try/finally` reset in `PackUpStation` prevents double payout if the FishNet layer's `Destroy_Server` triggers a second dispatch in the same frame.

## 0.2.5 (2026-09-12) — Bug-audit fixes round 3 (audit 2026-09-12)
- **Native quality mixing (HIGH):** When the output slot already has a stack of the same definition and a new batch is added, the mod now mixes weighted (tier via value via new `PackagingMath.TierToQualityValue` helper) and maps back to the nearest tier. Previously the stack inherited the quality of the first batch — standard buds on a premium stack were sold as premium and vice versa.
- **ObjLoader file limit (LOW):** `LoadMeshFromObj` now checks the file size before `File.ReadAllLines` (50 MB cap) and aborts with a warning when the vertex count exceeds 250,000. Prevents frame spikes on accidentally huge OBJ files.

## 0.2.4 (2026-09-12) — Bug-audit fixes round 2 (audit 2026-09-12)
- **PackUpStation CRITICAL fix:** Instead of raw `GameObject.Destroy(gameObject)`, the mod now calls `BuildableItem.Destroy_Server()` (FishNet ServerRpc, the vanilla dismantle flow). This cleanly tears down the buildable registry, grid occupancy, and network state. Prevents ghost placements and item duplication after save/load. Fallback to `Destroy(gameObject)` remains if no `BuildableItem` is attached to the GameObject (e.g. editor spawn).
- **Output pre-flight (HIGH):** Before the phase-2 deduct, it is checked whether the output definition + `GetDefaultInstance(1)` can actually be resolved. If not, the cycle is cleanly aborted before the input deduct (no more silent item loss).
- **Host guard:** The `Destroy_Server()` call is hung behind `IsHostOrSingleplayer()` — on an MP client, the mod still uses the vanilla fallback, because the server RPC would otherwise silently no-op.

## 0.2.3 (2026-09-11)
- TryDepositProduct/Packaging: remove-before-credit (no more free item on remove failure).
- Engine: feasible<=0 aborts batch calculation (instead of batchSize=1 coerce).
- Store: warning on unresolved slot fallback '0' (instead of silent misrouting).

## 0.2.2 (2026-09-10)
- RestoreNativeSlots cleans up rData buffer plus live-native gate in refund paths (fix item dupe after save/load).
- Snapshot/revert in ExecutePackagingTransaction (both overloads).
- PackUpStation: fit-check and add interleaved; partial success retains remaining stock.
- Slot--1 guard (no autopack_slot_-1.json); TryExtractOutputProduct pays out exactly N.

## 0.2.0 (2026-08-24)
- Version sync: bring `MelonInfo`, `mod.json`, AGENTS.md, and CHANGELOG into agreement at 0.2.0.
- No code changes since 0.1.0; the "0.2.1 verified 2026-08-23" claim in AGENTS.md was a documentation drift, not a release.
- Hardware store listing integration, 2-second packaging cycle, and atomic 2-phase engine remain as documented in 0.1.0.

## 0.1.0 (2026-08-22)
### Initial Release
- Implemented 4x4 industrial Auto-Packaging Station with procedural 3D chassis, overhead arch, and dual pneumatic pistons.
- Added animated UV-scrolling conveyor belt with configurable speed.
- Integrated multi-state status LEDs (Green/Orange/Blue/Red) with dynamic pulsing.
- Built atomic 2-phase packaging engine with +5% quality freshness bonus and 1:1 mix-effect replication.
- Added procedural sound synthesis for pneumatic hiss, compressor pump, and mechanical stamp impacts.
- Implemented slot-isolated atomic persistence (`autopack_slot_{slotId}.json`) via `SafeStorage.SaveAtomic` synchronized exclusively with `GameLifecycle.OnSaveComplete`.
- Integrated with S1API `BuildableItemCreator` and Handy Hank's hardware store listing injection.