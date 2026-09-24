# Changelog




## 0.1.12 (2026-09-17) — SnackVendor street pack-up fix
- **Interaction HUD default off (user request 2026-09-17):** The "Hold RMB — Pack Up" tooltip (including the orange progress bar) when hovering over outdoor items is no longer shown — `ShowInteractionHud` default `false` (user-config set). RMB pack-up still works unchanged; the toggle also affects the sleeping bag's sleep hint.
- **Bug (in-game report 2026-09-17):** `snackvendor` could not be packed up with RMB on the street — toast "won't fit in inventory". Cause: as a custom station it got no `OutdoorItemInteractable` (0.1.11), so the vanilla pack-up of the drying-rack base prefab fired into the void (street items have no `BuildableItem.ItemInstance`). Fix: `snackvendor` now receives the generic `OutdoorItemInteractable` in the place (`BuildingPatches`) and restore paths (`StreetPropertyManager.SpawnSavedStreetItem`) — same pack-up flow as `dryingrack` (refund 1x + unregister + destroy). `autopackagingstation` stays excluded (own F-key pack-up with native buffer refund). Orphan stock in the sidecar is cleaned up by SnackVendor v0.0.6 via SaveComplete prune.
- **Side effect:** The ThirdParty archive 2026-09-16 broke the `Hash.Api` source includes of PotScanner/BusinessIncome (path without `Archive/`) — csproj paths adjusted, solution green again.

## 0.1.11 (2026-09-16) — SnackVendor as custom station
- **Street placement handoff:** `snackvendor` is treated like `autopackagingstation` as a custom station (place prefix, RegisterStreetItem, save restore). HomelessMod instantiates the BuiltItem prefab directly and then disables `BuildableItem` — as a result the SnackVendor start postfix never fired and the station stayed a bare rack (spike 2026-09-15). The place path now calls `SnackVendor.Items.SnackVendorItemFactory.SetupPlacedStation(go, guid)` via reflection (same contract as AutoPack), the restore path does the same with the persisted street-item GUID.
- **No double interactable:** `RegisterStreetItem` detects the `SnackVendorController` (reflection) and no longer stacks a generic `OutdoorItemInteractable` on the station.

## 0.1.10 (2026-09-15)
- Host-authority check `IsHostOrSingleplayer` consolidated in `S1Mods.Shared.NetworkGuard` and corrected from fail-open to fail-closed: on an exception in the authority check, placement now falls back to vanilla handling instead of allowing a potentially client-side world mutation (affected place prefix, ghost colouring, sleeping bag and outdoor item dismantle).
- Version bump.

## 0.1.9 (2026-09-12) — Bug-audit fixes round 4 (audit 2026-09-12)
- **Ghost rotation restore in finally:** Previously `ghost.transform.rotation = originalRot` was only reached on the success path — if `EvaluatePlacement` or `ApplyMaterial` threw, the ghost stayed at `Quaternion.identity` (visually "spinning reset"). The restore now runs in a `finally` block and applies on every exception path.

## 0.1.8 (2026-09-12) — Bug-audit fixes round 3 (audit 2026-09-12)
- `BuildUpdate_Grid_CheckIntersections_Patch` (postfix) now has the same host guard as the place prefix. Previously MP clients saw the green ghost even though the place click was rejected (UX desync, possibly save drift).
- `ObstacleLayerMask` (GroundPlacementAssistant) extended with `Vehicle`/`NPC`/`Player`/`Item`/`Interactable`/`Navigation`/`NavigationRegion`. The original 4-layer mask treated vehicles and NPCs as "free" — sleeping bags landed on them. Unknown layers are reported with `Msg` (instead of `Warning`), since mod layers are usually missing.

## 0.1.7 (2026-09-11)
- Place prefix with host guard (clients fall back to vanilla, no local desync fork).
- Wipe-if-switched moved behind OnSaveInfoLoaded (no more full wipe on same-slot reload).
- Sleep flag with 5-minute stale guard (no quest credit through the next vanilla bed).

## [0.1.6] - 2026-09-10
- Place-prefix catch assigns __result=null (native crash vector closed).
- Quest completion stays in RAM, flushed on OnSaveComplete (no more save-rollback drift).
- SleepingBagItemFactory re-verifies Registry.ItemExists (fix stale _isRegistered after menu reload).

## [0.1.5] - 2026-09-09

### Fixed (Gatekeeper review round, 5 findings)
- **Critical — item loss on PackUp without ItemID**: `OutdoorItemInteractable.PackUp()` destroyed the object and unregistered it when `_itemId` was empty and unreconstructable — the player lost the placed object without replacement. Now: early `return` **without** destroy/unregister (same pattern as `SleepingBagInteractable.PackUp`). Object stays in the world, log warning shows the reason.
- **High — Quest 2 counted every outdoor item**: "Place a workstation or grow container outdoors" also completed when placing the sleeping bag (Quest 1 item). New: `IsProductionGear()` first checks the world components of the placed object (Pot/MixingStation/Mk2/Cauldron/ChemistryStation/PackagingStation/BrickPress/DryingRack — all verified in live decompile), then string fallback ("station"/"press"/"cauldron"/"rack"/"…pot") for modded gear. Sleeping bag no longer counts.
- **Medium — quest credit on falling asleep instead of waking up** (old M3 TODO): `NotifyPlayerSlept()` fired on `StartSleep()`. Aborted sleep still counted. New: `_sleepStartedInBag` flag on start, credit only in the `S1API.GameTime.TimeManager.OnSleepEnd` wake hook (Action&lt;int&gt;, verified in live decompile). Vanilla-bed sleep still doesn't count.
- **Low — back-side lighting of the sleeping-bag mesh**: `AddQuad()` used the front normals for the double-sided back triangles → wrong shading from below. New: dedicated back-side vertices with `-normal` and reversed winding order.
- **Low — ResetState asymmetry**: `HomelessQuestManager.ResetState()` had no `keepSlot` parameter (unlike `StreetPropertyManager`) and also cleared the slot cache on pure menu→game reload. New: `ResetState(bool keepSlot = false)` + `ResetForSceneUnload()`; `OnSceneWasUnloaded` now uses keepSlot:true.

## [0.1.4] - 2026-09-09

### Fixed
- **Quest 'Alley Operations' jumped back to 'open' after save reload** (root cause: order dependency). The both-goals check (`MarkQuestCompleted`) was only in the cash branch of `CheckCashProgress` behind `if (earn500.State != Completed)`. If the cash goal was completed BEFORE the gear goal (e.g. ≥500 $ in account, workstation placed later), this path never ran again → quest was never persisted in `quest_progress_slot_N.json` → after restart `InitializeQuests` created it fresh with open entries. New: `TryFinalizeQuest2()` checks both entries on **every** trigger (place + cash poll) — order independent.
- **Quest 'Street Sovereign' could complete prematurely** (inverse pattern): `MarkQuestCompleted` fired on the $5,000 goal alone, without checking the camp entry. New: `TryFinalizeQuest3()` requires both entries.

### Notes
- Existing saves self-heal: after the update, place gear outside once → quest permanently completes.
- Version drift fixed: `mod.json` was stuck at 0.1.2 (Mod.cs was 0.1.3) — both now 0.1.4.

## [0.1.2] - 2026-09-08

### Fixed
- **Sleeping bag floated ~1.5m above the ground**: the item definition is cloned from the bed via `CloneFrom("bed")` — vanilla `BuildUpdate_Grid` derives a bed-high `verticalOffset` from this, which lifted the flat procedural sleeping bag into the air.
  - Ghost fix: for the sleeping bag the vanilla `verticalOffset` is now ignored (only pivot correction remains) — the ghost lies directly on the ground when placing (`BuildingPatches.BuildUpdate_Grid_CheckIntersections_Patch`).
  - Save healing: on restore, the saved position is snapped down via ground raycast (`GroundPlacementAssistant.SnapToGround`). Old saves with floating bags are automatically corrected; bags on roofs/bridges (≤0.5m above surface) stay correctly placed. Log line `[GroundFix]` shows the correction.