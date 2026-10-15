# AutoPackagingStation

Automated industrial packaging station for *Schedule I* (2×2 m footprint, Hustler I rank).

## Features
- **Industrial Visuals:** Heavy steel chassis, moving pneumatic compression pistons, and animated UV-scrolling conveyor belt.
- **Multi-State Status LEDs:** Real-time visual feedback (Green = Ready, Orange Pulsing = Packaging, Blue = Waiting for Input, Red = Blocked / Full).
- **Status readout:** the station instruction label shows the live state and buffer counts (e.g. `Packing 40% | In 12 | Pkg 5 | Out 3`), updated only when the values change.
- **2-Second Packaging Cycle:** High-efficiency automated packaging with procedural pneumatic hiss and mechanical compression sound effects.
- **Native Packaging Engine:** Each cycle delegates to the vanilla `PackagingStation.PackSingleInstance()` conversion (batched up to `MaxBatchSize`), so item compatibility, quantities and output capacity follow the game's own rules. Auto-Unpack uses the native `Unpack()` symmetrically.
- **Freshness Quality Bonus:** Each successful packaging cycle rolls once and with probability = `FreshnessBonusMultiplier` (default 5%) the output quality rises by one tier (capped at Heavenly).
- **Item-Safe Persistence:** Slot-isolated saves via `SafeStorage.SaveAtomic` (`autopack_slot_{slotId}.json`, schema v2) synchronized with `GameLifecycle.OnSaveComplete`; saved items can never be wiped by a restore pass, and items that do not fit into a full inventory are persisted as pending refunds and re-offered on the next load.
- **Hardware Store Availability:** Purchasable at Handy Hank's Hardware ($3,500).

## Native prefab repair (current)

The placed template keeps the complete native PackagingStation component and networking graph. Only an owned clone of the cauldron's existing 4x4 footprint hierarchy is transplanted. No native station component is added after placement. Unsafe donor mappings abort registration rather than falling back to an unrelated machine.

The template stays active-self beneath an inactive persistent cache, so native initialization runs on the real placed instance, not on a scene-active template. `[autopack-prefab]` reports successful composition; `[autopack-ui] native canvas opened` reports an actually visible station canvas. Native E prompt, canvas and pack/unpack conversion were gameplay-verified on deployed v0.4.6 on 2026-10-15 (log-backed, see below). The subsequent v0.4.7 log confirms fresh placement/setup, native canvas opening, Package/Unpackage conversion and mode mirroring for one station. On 2026-10-15 the owner subsequently confirmed that everything tested in-game worked. This is owner-confirmed gameplay acceptance for that session, not an independently logged per-check certification. No separate multiplayer or 0.4.1 cauldron-save migration result was supplied.

Prior installed build evidence (historical): 0.4.3 (2026-10-14), 61 automated tests passed, independent scoped review PASS, Release built/deployed with 0 warnings/errors; installed DLL/PDB/metadata hashes matched. Its 19:30 runtime check exposed a registration blocker: the geometry guard rejected the native `OverrideGhostMaterial` marker. The current repair accepts that specific marker while preserving network/unknown-script rejection. Tests do not exercise Unity/FishNet initialization.

Prior installed build evidence (historical): 0.4.4 (2026-10-14), 62 tests passed, scoped review PASS, Release built/deployed 0 warnings/errors; DLL/PDB/metadata hashes matched. The 19:38 session confirmed registration but placed setup aborted on a root-only interaction lookup before visual construction. The current repair resolves the native station's IntObj reference or an owned child interaction; GLB rendering and gameplay acceptance remain open.

Prior installed build evidence (historical): 0.4.5 (2026-10-14), 63 tests passed, scoped review PASS, Release built/deployed 0 warnings/errors; DLL/PDB/metadata hashes matched. No AutoPack model.glb exists in the installed data folder or working repository, so the existing procedural model fallback is used unless an actual GLB asset is supplied. Model display was accepted by the owner in the 19:48 session; IntObj resolution and completed setup are logged. The E prompt was reported missing; this is not menu or full-mod verification.

Verified build evidence (2026-10-15, against the deployed 0.4.6 DLL): the `Latest.log` 07:27–07:28 session records two completed station setups (`native interaction resolved on 'IntObj'`), four `[autopack-ui] native canvas opened` breadcrumbs (two per station GUID), `Native PackSingleInstance executed 8x` in two cycles, `Native Unpack() executed 8x` in one cycle, canvas mode mirroring UNPACKAGE→PACKAGE and a vanilla comparison dump. The 0.4.6 collider A/B resolved the E-prompt defect: the removed placed root collider was intercepting native selection (0.4.5 with collider: no E; 0.4.6 without: E + canvas work).

Current installed build: 0.4.7 (2026-10-15), 66/66 tests pass; independent retirement-scoped review PASS, Release built/deployed with 0 warnings/errors. Installed DLL/PDB/metadata hashes match the build/source; Shared.dll is unchanged. Installed assembly metadata confirms MelonInfo 0.4.7 and absence of the retired diagnostic types. In-game acceptance: owner-confirmed 2026-10-15 for the tested session. The supplied 09:01–09:02 log independently confirms this exact DLL loaded, one station completed setup, its native canvas opened and native pack/unpack cycles ran. The owner subsequently reported that everything worked in-game; individual additional checks were not itemized, so multiplayer and legacy migration are not certified by that general report. The experiment scaffolding is retired: `AutoPackInteractionDiagnostics.cs`, `AutoPackDiagnosticState.cs`, the read-only Awake observer patch and all per-instance dumps are removed; placed setup still adds no root collider and the breadcrumb reads `(controller + visuals + native interaction)`. [Historical diagnostic interpretation and remaining gameplay checks](interaction-diagnostics.md).

## Controls
- **[E]**: Open the station UI (native PackagingStation canvas; use the red mode arrow to switch Package/Unpackage).
- **[F]**: Pack up the station while aiming at it — all buffered items plus the station item are returned to your inventory (host-only in multiplayer).

> **Dismantle:** Press **F** while looking at the station. If your inventory is full, the remaining items are stored as pending refunds and returned on the next load — nothing is lost. (Pending refunds survive successful saves only: if the game crashes before the next completed save, session-queued refunds are lost with it.)
