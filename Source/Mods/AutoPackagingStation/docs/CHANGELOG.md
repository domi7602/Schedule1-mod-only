# Changelog - AutoPackagingStation

## 0.4.7 (2026-10-15) — Retire interaction diagnostics after verified native E/canvas fix

- Owner-reported gameplay success (2026-10-15 morning session) is corroborated by `Latest.log` 07:27–07:28 against the deployed 0.4.6 DLL: two custom stations completed setup (`[autopack-setup] native interaction resolved on 'IntObj'`, GUIDs `68b74a59…` and `0420e054…`), `[autopack-ui] native canvas opened` fired twice per station GUID, `[AutoPack Engine] Native PackSingleInstance executed 8x` in two cycles, `Native Unpack() executed 8x` in one cycle, and canvas mode mirroring UNPACKAGE→PACKAGE. A vanilla comparison dump (`stage=vanilla-start`) was captured.
- The 0.4.6 collider A/B resolves the E-prompt defect: 0.4.5 with the placed root collider — no E prompt; 0.4.6 with all inherited native colliders retained and no root-collider manipulation — E prompt and native canvas work. The removed root collider was the selection blocker, consistent with native `InteractionManager.CheckHover` resolving hits via `GetComponentInParent` and cutting the candidate search at the nearest blocking hit.
- Retire the experiment scaffolding: delete `AutoPackInteractionDiagnostics.cs` and `AutoPackDiagnosticState.cs`; remove the read-only `PackagingStation.Awake` observer patch, the once-per-instance interaction/collider/serialized-UnityEvent dumps, the vanilla comparison dump, the Main-unload instance bookkeeping (`OnSceneWasUnloaded` override) and the deinit `Shutdown` call. No functional behavior changes beyond the breadcrumb, which now reads `Configured AutoPackagingStation [GUID] in world (controller + visuals + native interaction).`
- Tests: delete `InteractionDiagnosticStateTests.cs` (13 cases) and the three diagnostic wiring contracts; add `InteractionDiagnosticScaffoldingIsFullyRetired` (asserts absence against active code AND comments plus file non-existence) and the updated setup-breadcrumb contract. Observed RED (2 failures), then 66/66 pass.
- Final compile gate caught two stale `using AutoPackagingStation.Diagnostics;` imports after the file deletion. Extended the retirement contract to forbid that namespace in both entry/factory files; observed the targeted RED, removed the imports, then all 66 tests passed and compile-only Release succeeded with 0 warnings/errors. Version-sync and doc-path checks passed.
- Local deployment (2026-10-15): independent retirement-scoped review PASS; all 15 reviewed source/test fingerprints matched before release. Release rebuild/deploy succeeded with 0 warnings/errors and 66/66 tests passed. Installed DLL, PDB and mod.json matched their build/source hashes; installed MelonInfo reports 0.4.7, and ILSpy confirms both retired diagnostic types are absent. DLL SHA-256: `c942fd65d8a8c2a50ad3adad79c27c903d66a1938e4193bbf6b60ccd99fe6580`; Shared.dll remained `b108404d828216aca47bd59a7a0adef4cf114be183839257deb14327035f2ddc`. Prior DLL/PDB/metadata are backed up locally. The review could not certify byte-for-byte preservation against v0.4.6 because HEAD is an older WIP baseline; gameplay acceptance was still open at deployment; the later smoke-test result is recorded below.
- Historical gameplay acceptance on deployed v0.4.6 (log-backed, 2026-10-15): 4x4 placement on two stations, native E prompt + canvas open (4 canvas breadcrumbs), Package and Unpackage conversions with batch quantities, mode mirroring, instance-specific canvas targets across two stations. This historical session does not establish v0.4.7 acceptance; see the subsequent v0.4.7 smoke test below. Still open: F Pack-Up, save→reload persistence, collision walk-around, vanilla E-prompt comparison, multiplayer, 0.4.1 cauldron-save migration.

- Partial v0.4.7 gameplay verification (2026-10-15, owner-supplied MelonLoader excerpt 09:01:53–09:02:49): loaded assembly SHA-256 matches the reviewed built/installed DLL `c942fd65d8a8c2a50ad3adad79c27c903d66a1938e4193bbf6b60ccd99fe6580`. Initialization and PatchGuard 1/1 succeed; native prefab/4x4 footprint registration succeeds. Fresh station `41e512d0-f101-4710-b685-c35830f8002b` completes setup through both triggers at 09:02:36 (the same GUID, not two stations). Its native canvas opens at 09:02:39 with inputSlots=2 and outputSlotPresent=True. Native PackSingleInstance executes 8x at 09:02:44 and 09:02:49; native Unpack executes 8x at 09:02:47, with canvas mode mirroring UNPACKAGE then PACKAGE. No AutoPackagingStation error appears in the supplied excerpt. F Pack-Up/refunds, save→reload, collision walk-around, vanilla comparison, prompt appearance, close/reopen, simultaneous stations, multiplayer and legacy migration are not established by this session. Loading an empty station save before fresh placement is not a persistence test.

- Owner-confirmed gameplay acceptance (2026-10-15, after the log-backed v0.4.7 smoke test): the owner reports that everything tested in-game worked. This is firsthand acceptance for the tested session; additional actions were not itemized. No separate multiplayer or 0.4.1 cauldron-save migration result was supplied. The preceding check-specific limits describe the earlier log-only evidence stage, not a later reported failure.

## 0.4.6 (2026-10-14) — Native interaction diagnostics and collider-only A/B test

- The correctly loaded 0.4.5 session at 19:57–19:58 completes setup on two stations, but neither native hover nor canvas opening is established by the old `E-interact wired` message. The missing E prompt remains owner-reported; the root-collider selection hypothesis is not yet live-proven.
- Remove only the root collider creation/resize/enable block in `SetupPlacedStation`. Retain every inherited native collider unchanged. No replacement collision geometry, custom E input, native listener repair, manual Awake call or FishNet component change.
- Add `[autopack-interaction]` dumps once per native instance: native station/IntObj identity and complete path, active/enabled/layer/range/state/type/angle, display references, ancestor transforms/scales, and all root/child colliders with local shape and world bounds. Vanilla packaging stations get the same read-only comparison dump through the existing Start hook.
- A read-only `PackagingStation.Awake` postfix records the concrete native pointer + Unity instance ID. `PostfixObserved` proves a callback was observed, not listener execution; `NotObserved` is not proof Awake was skipped; `ObserverUnavailable` records a failed hook. Scalar bookkeeping clears on Main unload, not the later OnPreLoad event; the observer is removed on deinitialization.
- Replace the unsupported setup success claim with an explicit native-interaction-retained / prompt-and-canvas-unverified message. `HideBaseRenderers`, F-Pack-Up, ghost, 4x4 composition, processing and save/refund code remain unchanged.
- Independent scoped review PASS (no security/logic findings). Its two P3 diagnostic-resilience notes were applied with observed RED → GREEN: interaction/ancestor reads, collider enumeration and selection snapshot are exception-isolated so stale native references cannot erase the collider A/B evidence, and an incomplete dump releases its one-shot key so the duplicate setup trigger can retry.
- Native tracing (read-only, GameAssembly instruction level) disproves the Awake-listener suspicion for this build: the `PackagingStation` UserLogic body (RVA `0x8911A0`) initializes light/switch/slots/filter/callbacks but registers no `IntObj` hover/start listener and never reads `IntObj` (+0x240). Serialized persistent UnityEvents are the remaining wiring hypothesis, and the dump now reports `onHovered`/`onInteractStart`/`onInteractEnd` persistent counts/targets/methods/state for vanilla and custom comparison. Native `InteractionManager.CheckHover` resolves hits via `GetComponentInParent` (no child lookup) and cuts the sphere candidate search by nearest blocking ray distance, making a nearer root collider a mechanical suppression path for child IntObj. Selection hit and Awake execution remain gameplay evidence, not native proof.
- Regression gates observed RED, then 79 tests pass (pure diagnostic bookkeeping and supplementary source contracts). Compile-only Release: 0 warnings/errors. Deployed 2026-10-14: built=installed hashes match (`AutoPackagingStation.dll` SHA-256 `6939aeaa5f3ffd16c0f725322ceb0cbca3caf010151a70ee15ead8dbcbcd937c`, `Shared.dll` SHA-256 `b108404d828216aca47bd59a7a0adef4cf114be183839257deb14327035f2ddc`), installed PDB/mod.json read back (`version` 0.4.6), and ILSpy lists `AutoPackagingStation.Diagnostics.AutoPackInteractionDiagnostics`/`AutoPackDiagnosticState` in the installed DLL. Gameplay verification open. See `interaction-diagnostics.md` for the full acceptance checklist and evidence interpretation.

## 0.4.5 (2026-10-14) — Resolve the native child interaction before visual setup

- The supplied 19:38 v0.4.4 log confirms successful native-prefab composition, 16 footprint tiles and shop registration. Both placed-setup triggers then aborted on `native PackagingStation interaction is missing`, before `BuildStationVisuals()` could load the GLB.
- Resolve the existing interaction from `PackagingStation.IntObj`, with an inactive-child lookup fallback. Require the resolved interaction to belong to the placed object before changing its range. Do not create components or add listeners; native networking/wiring remains unchanged.
- New evidence line: `[autopack-setup] native interaction resolved on '...'`. Existing `Loaded custom AutoPackagingStation GLB model successfully (cached).` and `[autopack-ui] native canvas opened` distinguish model creation from actual menu opening.
- Regression observed RED before repair, then 63 tests passed; Release built/deployed 0 warnings/errors; scoped review PASS; installed DLL/PDB/metadata hashes matched (DLL SHA-256 `92355f4a5b01438d7a106347ee48820d6e60af968500c811209019f2f3adf697`). Visual rendering, E-menu and broader gameplay acceptance remain open.
- Asset check: `Mods/AutoPackagingStation/model.glb` is absent, and no AutoPack GLB was found in the working repository or the user home. The existing procedural model fallback remains available; this patch does not restore a missing GLB asset.
- Owner runtime acceptance (19:48): machine model is visible again. Native IntObj resolution and completed setup are logged. E prompt is reported missing; its raycast/collider/state path remains under investigation. This is not menu or full-mod verification.

## 0.4.4 (2026-10-14) — Accept the native footprint ghost-material marker

- The 19:30 v0.4.3 session loaded the correct DLL, but registration aborted before publication: `FootprintTiles` contains the native `ScheduleOne.Building.OverrideGhostMaterial` presentation marker. The geometry allowlist incorrectly rejected it as machine behaviour.
- Preserve that specific typed marker during footprint transplantation. FishNet `NetworkBehaviour`/`NetworkObject` rejection and rejection of unknown scripts remain unchanged; no blanket allowance for MonoBehaviours.
- Source regression observed RED, then 62 tests passed; Release built/deployed 0 warnings/errors; independent scoped review PASS; installed DLL/PDB/metadata hashes matched (DLL SHA-256 `4b27ce4f84daff4f3d4706d9d8d5c1f6c48eed5cc48d4d35cea8f1ba65d0906b`).
- This addresses the proven registration blocker only. Successful registration, 4x4 placement, E-menu/native networking and persistence still require an in-game check.

## 0.4.3 (2026-10-14) — Complete publication before registration; bounded prefab ownership

- Includes the native PackagingStation + cloned cauldron 4x4 footprint repair developed in the compile-only 0.4.2 iteration below.
- Independent review found that an exception after `builder.Build()` could leave a default packaging prefab registered. Assign the owned prefab and prepare the ghost BEFORE publication; require exact prepared definition/prefab identity on retries, shop injection and placed setup. Never accept a conflicting ID just because it exists.
- S1API 3.2.1 compatibility bridge uses the private `BuildableDefinition` getter and `BuildableGhostRuntime.RegisterVisual` signature. Both were confirmed in the installed DLL via ilspycmd. Missing signatures abort before publication; no source dependency on internal S1API types.
- A completed prefab is reserved once and reused across publication failures/registry resets. Its inactive cache has an explicit deinitialization owner, released even if store cleanup throws.
- Regression tests observed RED before repair, then 61 tests passed (pure policies + source contracts); Release build/deploy 0 warnings/errors. Independent scoped re-review PASS; parent reran 61 tests. Installed DLL/PDB/metadata hashes matched; DLL SHA-256 `1daa954acf4f8bb54453dfafd38b8837a0c94b0e25db453a73b242273aa02563`. No in-game acceptance claimed.
- Native clone remapping, Awake-created footprint dependencies, physical grid/BuildPoint alignment, E-menu, conversions, old cauldron save-type migration and multiplayer still require live acceptance.

## 0.4.2 (2026-10-14) — Compile-only iteration (not deployed): native packaging + cauldron footprint

- Root cause: v0.4.1 cloned the cauldron and added `PackagingStation` after spawn. The supplied 18:35 log shows `IsClientInitialized -> SetPlayerUser -> Use` throwing; the added component lacked the native packaging network graph.
- Clone the complete native packaging prefab under an inactive persistent cache; transplant only the cauldron clone's existing footprint subtrees and private coordinate mapping. No fabricated tiles, detector injection or scene-active template priming.
- Require a complete 16-cell 4x4 mapping, private tile/dependency ownership, geometry-only transplanted subtrees and unchanged network-behaviour counts. Abort composition before registration on an unsafe donor. Original donor prefabs are never edited.
- Keep native interaction wiring; remove the late native `AddComponent` and duplicated anonymous E listeners. Preserve transplanted footprint renderers when hiding the donor model.
- New runtime evidence: `[autopack-prefab]` reports composition and inactive-template state; `[autopack-ui] native canvas opened` reports an actually visible canvas, not merely an attempted interaction.
- Automated checks: 51 tests (pure policies and source contracts); compile-only Release passed with 0 warnings/errors. Deployment pending independent review. Native remapping, placement, E menu, conversion, persistence and multiplayer remain in-game-verify open for this build.
- Migration from stations saved with the v0.4.1 cauldron native data type has not been tested. Back up the test save before acceptance; do not infer save compatibility from a newly placed station.

## 0.4.1 (2026-10-14) — Basiswechsel auf Cauldron (4×4-Blueprint nativ)
Der elegantere Weg, den der Maintainer vorschlug: statt selbstgeklonter Tiles den Vanilla-**Cauldron** als Basis nehmen, der bereits korrekt initialisierte 4×4-Footprint-Tiles hat.

- **Basis-Priorität:** `candidateBases` beginnt jetzt mit `cauldron` — die 4×4-Tiles werden geerbt und sind nativ vollständig (Detector + Appearance + Blueprint).
- **`NormalizeFootprint` deaktiviert:** Kein Tile-Klonen mehr, keine Footprint-Mutation nötig.
- **Blueprint-Fix-Hook entfernt** (`AutoPackGhostBlueprintFix`): Bei Vanilla-Tiles initialisiert das Spiel sie selbst.
- **Prefab-Isolation bleibt** im Quellcode (Vanilla-Schutz), wird aber ohne Footprint-Änderung nur noch passiv gebraucht.
- Erwartet: 4×4-Grid-Blueprint (blau/rot) + Kollision + keinerlei Kollisions-Überschneidung.
- Gameplay-Verifikation offen.

## 0.4.0 (2026-10-14) — Footprint 2×2 + Blueprint-Reparatur
Footprint-Fix wieder aktiviert mit gezielter Blueprint-Reparatur.

- **Footprint:** `NormalizeFootprint` wieder aktiv — 4×4 Tiles (2×2 Meter). Station blockiert korrekt 2×2.
- **Blueprint-Reparatur:** Neuer Postfix auf `BuildStart_Grid.CreateGhostModel` ruft für unsere Station nachträglich `Initialize(closestTile)` pro FootprintTile auf (falls `MatchedStandardTile` fehlt) und schaltet das Footprint sichtbar. Das behebt das fehlende Grid-Blueprint bei eigenen geklonten Tiles.
- **Log-Marker:** `[AutoPack][blueprint-fix] CreateGhostModel: N tiles — X initialized, Y already OK, Z no detector.`
- Gameplay-Verifikation offen.

## 0.3.9 (2026-10-14) — Footprint-Fix zurückgenommen (Grid-Blueprint zurück)
Der BUG-009-Fix (Footprint-Expansion auf 2×2) hat das Grid-Blueprint gekapert, weil eigene geklonte FootprintTiles nicht die vollständige native Tile-Struktur (TileDetector + Initialize + TileAppearance) replizieren. Mehrere Reparaturversuche (Detector-Klon, Appearance-Klon, S1API-Muster-Umbau) haben das Blueprint nicht wiederhergestellt.

- **Footprint-Fix deaktiviert:** `NormalizeFootprint` wird nicht mehr aufgerufen. Vanilla-1×1-Footprint bleibt intakt.
- **Ghost-Diagnose-Hook entfernt** (war nur für den Footprint-Fix).
- **Trade-off:** Station ist jetzt 1×1 platzierbar statt 2×2 — dafür geht das Grid-Blueprint (blau/rot) und die Kollisionsprüfung wieder korrekt.
- Gameplay-Verifikation offen.

## 0.3.8 (2026-10-14) — Setup-Hook feuert nicht (Diagnose + zweiter Auslösungspfad)
Fix für den Live-Befund aus dem In-Game-Test (0.3.7): Kaufbar, aber kein Grid-Blueprint beim Platzieren und danach Vanilla-Optik — im Log fehlte `Configured AutoPackagingStation [GUID]`, d.h. `SetupPlacedStation` lief nie (fertig).

- **Diagnose:** `SetupPlacedStation` loggt jetzt jeden Entry (`SetupPlacedStation entry: object='…'`), sodass Hook-Feuer und Abbruchstelle im Log sichtbar sind.
- **Zweiter Auslösungspfad:** zusätzlich zum `BuildableItem.Start`-Postfix hängt das Setup an S1API's `BuildEvents.OnGridItemCreated` (liefert ItemId + GameObject direkt, robust gegen Start-Reihenfolge und abweichende Prefab-Pfade). Idempotent — Doppel-Auslösung ist harmlos.
- Gameplay-Verifikation offen.

## 0.3.7 (2026-10-14) — Platzierungs-Ghost + falsches Platzierungs-Objekt behoben
Fix für zwei Live-Befunde aus dem In-Game-Test (0.3.6): Die Station war kaufbar, aber der Platzierungs-Ghost zeigte kein Grid-Feld, und beim Platzieren erschien optisch eine normale Vanilla-Packstation.

- **Ghost:** `replaceExistingVisual` ist jetzt `false` — die geerbte GridItem-Footprint-Anzeige (das „Grid-Feld") bleibt sichtbar, unsere Mod-Visuals werden zusätzlich aufgebaut. Der doppelte BoxCollider auf dem Ghost-Root ist entfernt (der Native-Ghost bringt eigene Collider/Tasks mit).
- **Setup-Guard:** `SetupPlacedStation` verifiziert per Item-ID, dass das übergebene GameObject wirklich unsere Station ist, und bricht sonst ab (statt versehentlich eine Vanilla-Station einzurichten). Erfolgreicher Abschluss wird als `Configured AutoPackagingStation [GUID] in world (controller + visuals + E-interact wired)` geloggt.
- Isolation (0.3.6) und Vanilla-Schutz bleiben unverändert.
- Gameplay-Verifikation offen.

## 0.3.6 (2026-10-14) — Vanilla-Prefab-Schaden behoben (Prefab-Isolation)
Kritischer Fix für einen Seiteneffekt des BUG-009-Footprint-Fixes: S1API's `CloneFrom` teilt das BuiltItem per Referenz, sodass `NormalizeFootprint` fremde FootprintTiles in das Vanilla-Spenderprefab ('packagingstation') geklont hat — Vanilla-Packstation ließ sich in derselben Sitzung nicht mehr korrekt platzieren. Das betraf nur den RAM der Sitzung (kein Save-Schaden).

- **Prefab-Isolation (`EnsureOwnBuiltItem`):** vor jeder Footprint-Mutation wird das BuiltItem einmalig in eine eigene, inaktive, `DontDestroyOnLoad`-Kopie geklont (Marker: Name endet auf `_AutoPackIsolated`) und nur unsere Definition umgehängt. Scheitert der Klon, wird der Footprint unangetastet gelassen (Vanilla-first).
- **Doppelter Vanilla-Schutz:** `NormalizeFootprint` verifiziert vor der Mutation, dass es auf der isolierten Kopie arbeitet, und bricht sonst ab — das Vanilla-Spenderprefab wird garantiert nie angefasst.
- **Bessere Breadcrumbs:** Vorher/Nachher-Tile-Anzahl und Prefab-Name im `NormalizeFootprint`-Log.
- Gameplay-Verifikation offen.

## 0.3.5 (2026-10-13) — Re-review findings (footprint anchor, bonus population, refund cap)
Follow-up to the 0.3.4 bug-audit build, from an independent re-review. Unit tests still 45/45; gameplay verification open.

- **Footprint anchor fix (re-review finding 1):** `NormalizeFootprint` built its N×N grid from coordinate (0,0) while measuring world positions from the template tile — inheriting a base with an offset footprint (e.g. tile at (5,5)) drifted grid coordinates off the placement raster. Coordinates are now built relative to the template tile (`templateX+dx`, `templateY+dy`), so coord↔position pairing holds for any base anchor.
- **Bonus population unified (re-review finding 3):** the legacy rData fallback committed output at the *input* tier and rolled the bonus pre-commit, while the native path rolls against the *produced output stack*. The fallback now commits at the input tier first and rolls `ApplyFreshnessBonusToBuffer` against the final output stack tier — same population as native.
- **Refund delivery capped (re-review finding 4):** `DeliverPendingRefunds` materializes at most 20 unit instances per load (rest stays queued for the next load) to avoid a frame spike from a large backlog; the re-queue log now distinguishes "per-load budget exhausted" from "inventory full".
- **Documented, not code-fixed (re-review finding 2):** pending refunds survive *successful* saves only — a save abort (partial data rejected) keeps refunds queued in this session in RAM until the next successful save. Noted in the store comment and the mod README.
- AGENTS.md AutoPackagingStation entries corrected (v0.3.4 date/status drift from the re-review).

## 0.3.4 (2026-10-13) — Bug-audit: item integrity, save safety, native packaging path
Fixes from the 2026-10-13 bug audit (19 findings). Unit-tested pure policies (`AutoPackPolicies.cs`, 45 tests); gameplay verification open.

- **BUG-001 (P0, saved items wiped at load):** `RestoreNativeSlots()` cleared every native slot whose record was empty — and `Start()` ran it a second time after the staged restore had already consumed the runtime copies, wiping freshly restored items. Restore now follows unit-tested `RestorePolicy`: a filled native slot always wins, records only fill EMPTY slots, absent/empty records never clear anything. Failed materialization keeps the runtime copy for the refund fallback.
- **BUG-002 (P0, item loss on destroy with full inventory):** `OnDestroy()` refunded only what fit and dropped the rest. Un-deliverable items are now persisted as `PendingRefunds` in the save file (SaveVersion 2) and re-offered on every load until taken (`DeliverPendingRefunds`). Collection also runs when no inventory is available; a completed PackUp skips the refund pass entirely (no double refund).
- **BUG-003 (P0, dismantle not transaction-safe):** PackUp marks the station `_packUpHandled` before teardown (OnDestroy can no longer refund a second time) and falls back `Destroy_Server()` → `BuildableItem.Destroy()` → raw `Destroy()`.
- **BUG-004 (P0, packaging transaction could return success without output):** the hand-rolled native-slot transaction is GONE — the packaging step now delegates to the native vanilla conversion `PackagingStation.PackSingleInstance()` (batched, `GetState(EMode.Package)==CanBegin` re-checked before every call, symmetric to `ExecuteNativeUnpack`). The native path owns the transaction, the output instance and stacking; the old failure paths (silent null instance, rollback without output-quality restore) cannot occur anymore. The rData fallback keeps its snapshot rollback.
- **BUG-005 (P0, partial save overwrote good data):** `OnSaveComplete` now aborts the write (previous file kept) on any serialization failure or duplicate/empty GUID, and merges staged records (loaded, controller not yet matched) into every save so mid-restore saves can no longer drop stations. `RemoveRuntimeData` also drops the staged record (no resurrection of dismantled stations).
- **BUG-006 (P1, unpackage mode lost on save/load):** `UnpackageMode` is persisted and restored (SaveVersion 2; old saves default to Package).
- **BUG-008 (P1, invalid combos consumed packaging):** heuristic ID resolution (`{raw}_{pkg}`, `_jar`, `_brick`, `_baggie`, raw-ID fallback) removed. Native path validates via the vanilla state machine (`EState.Mismatch`); the fallback path requires a real `PackagingDefinition` + `ProductDefinition` and `ValidPackaging` membership (`ValidatePackagingCombo`). A packaged product keeps its product ID and carries the packaging on the instance (native item model).
- **BUG-009 (P1, footprint ≠ model footprint):** footprint normalized to a complete N×N grid over the 2×2 m base with tile size and count derived from the native `Grid.TileSize` at runtime (logged); inherited tiles kept (over-blocks rather than under-blocks).
- **BUG-010 (P1, quality bonus was a no-op):** quality-value × 1.05 never crossed a tier boundary (0.21/0.3675/0.5775/0.84/0.9975 map back to their own tiers). New mechanic (`QualityBonusPolicy`, unit-tested): each successful cycle rolls once and with probability = `FreshnessBonusMultiplier` (default 5%) the output quality rises one tier (capped at Heavenly). Consistent in native and fallback paths; item description updated.
- **BUG-011 (P1, inconsistent station lookup):** one canonical `ResolveNativeStation()` (self → children → parents) used by controller, engine and store; the per-frame `Update()` uses a 2 s cached variant (BUG-015 partial).
- **BUG-012 (P1, registration too late):** item registration also runs on `GameLifecycle.OnPreLoad` (the documented pre-save-data point); existing hooks remain as fallbacks.
- **BUG-013 (P1, ghost lost its materials):** the placement ghost no longer attaches a full controller (whose `OnDestroy` destroyed the instanced materials the ghost renderers used) — `BuildStationVisuals(root, null)` builds visuals only.
- **BUG-014 (P2, duplicate visuals on rebuild):** `BuildStationVisuals` is idempotent — re-runs clear previously generated children of `AutoPack_VisualRoot` first.
- **BUG-017 (P2, shop retry incomplete):** retry budget (10×2 s) is now driven by TARGET-shop success (hardware shop processed) instead of "any shop exists"; re-armed per load; last reason logged.
- **BUG-018 (P2, cursor state clobbered):** the cursor state from the frame the station UI opened is restored exactly on close (was: force-locked + hidden).
- **BUG-019 (P2, no config validation):** `ConfigPolicy.Sanitize` (unit-tested) validates every loaded value at startup — non-finite → default, out-of-range → clamped, empty item ID → default, every correction logged.
- **Not fixed (documented scope):** BUG-007 (multiplayer sync of the unpackage mode — needs a custom FishNet RPC/state, own work package), BUG-016 (unused `TryDeposit*`/`TryExtract*` rData inventory paths kept, removal is the maintainer's decision), Architecture B (mod JSON and vanilla save are not one atomic save).

## 0.3.3 (2026-09-20) — Fix: Baggie clones floating above the machine during unpack
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