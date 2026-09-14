# Schedule I — Modding Workspace (Developers Guide)

*Note: This is the technical documentation for building and contributing to the mods in this repository. For the player-facing mod list and installation guide, please see [README.md](README.md).*

MelonLoader modding workspace for *Schedule I* v0.4.6f13 (TVGS). Fully built on IL2CPP + S1API 3.2.0 + MelonLoader 0.7.3.

## What This Is

- MelonLoader / IL2CPP mods (TFM `net6.0`) targeting Schedule I **v0.4.6f13**
- Mod source plus in-repo decompiled assemblies under `GameReferences/` (Assembly-CSharp, firstpass)
- AI agent skills under `Skills/` (see below)

## Layout

```
Source/Mods/                  Mods + shared lib + Directory.Build.props/targets
Source/Mods/S1Mods.sln        Solution (regeneratable via Tools/gen-sln.ps1)
Source/Archive/               Archived mods (DayCounter, ProfitTracker, TVBrowser)
Source/Tests/                 xUnit tests (Shared, AutoPackagingStation, BackpackMod)
GameReferences/               In-repo decompiled assemblies (Assembly-CSharp, firstpass)
Skills/                       20 AI-agent skills (`Skills/<name>/SKILL.md` + `references/`; index: Skills/README.md)
ThirdParty/                   External frameworks & mod sources (S1API, S1MCP, MoreDrugs, PhoneScroll; hash/Sideload reference-only)
Tools/                        build-all, gen-sln, new-mod, bump-version, check-version-sync, package-release, deploy-thirdparty, backup-to-d
Release/                      Release packages
.github/                      CI (workflows/ci.yml, workflows/release.yml) + issue/PR templates
memory/                       Daily logs `memory/YYYY-MM-DD.md` + `memory-protocol.md`
AGENTS.md                     Workspace conventions & mod inventory for AI agents
README.md                     Player-facing mod overview
DEVELOPERS.md                 This file
```

## AI-Agent Skills

Twenty skills orchestrate mod work. Load via the `skill` tool (do not open manually). Index: `Skills/README.md`.

| Skill | Purpose |
|---|---|
| **`schedule1-modding`** | **Primary skill.** Runbook: scaffold a mod, build, deploy. Architecture patterns (PhoneApp/HUD/Saveable/Harmony/Polling/ModConfig/Console). S1API/UI/IL2CPP cheat sheet. |
| **`schedule1-phoneapp`** | PhoneApp runbook: S1API PhoneApp development, Method 3 responsive UI, InputFocus protection, lifecycle stability (Rule 10/11). |
| **`schedule1-grid`** | Grid & building: outdoor/unrestricted placement, BuildUpdate_Grid patching, ghost positioning, 7 Golden Rules. |
| **`schedule1-s1api`** | S1API framework reference: Saveables, PhoneApp base, Quests, NPCs, Items, Money, GameTime, Lifecycle, cross-branch compatibility. |
| **`schedule1-s1mapi`** | S1MAPI framework reference: ProceduralMesh, BuildingBuilder, GltfLoader, InteriorBuilder, World tools. |
| `schedule1-knowledge` | Research: in-repo decompiles (`GameReferences/`), S1API source (`ThirdParty/S1API/`), 64 curated systems (`Skills/schedule1-game-systems/references/`). |
| `schedule1-troubleshooting` | Diagnostics: native PowerShell `Latest.log` triage, crash patterns, save-load timing, IL2CPP pitfalls, slot_-1 recovery. |
| `schedule1-game-systems` | Game systems: 64 systems (Growing, Inventory, Property, etc.), decision tree, recipes. |
| **`schedule1-economy`** | Economy: Money (cash/bank), Business revenue, Shop multi-payment, Customers, Laundering. |
| **`schedule1-persistence`** | Persistence: SafeStorage atomic .bak, slot_{n}.json, GameLifecycle timing, ModConfig TOML sidecar. |
| **`schedule1-items`** | Items: BaseItemDefinition/Registry/StackLimit, Inventory slots, Buildable injection. |
| **`schedule1-interiors`** | Interiors & Minigames: Door hooking, Procedural room shells, In-world screens, 3D spatial ambience. |
| **`schedule1-3d-assets`** | 3D Assets & Blender: Blender pipeline, URP shader resolution, PBR materials, bone rigging, zero-collider rule. |
| **`schedule1-mcp`** | S1MCP & Live Debugging: Live game introspection, TCP :8765 bridge, log capturing, object reflection. |
| **`schedule1-custom-npcs`** | Custom NPCs: NPCPrefabBuilder, appearances, dialogue node graphs, daily schedules, custom clothing. |
| **`schedule1-debounced-reload`** | Debounced live-reload: FileSystemWatcher debouncer (150–250ms), main-thread pump via OnUpdate, config hot-reload. |
| **`schedule1-harmony-bootstrap`** | Harmony bootstrap: assembly-wide patch discovery, PatchTargetGuard pre-flight checks, applied/skipped/failed counters. |
| **`schedule1-il2cpp-reflection`** | IL2CPP runtime reflection: array bridging, missing overloads, namespace fallback, dynamic member access. |
| **`schedule1-lifecycle-verify`** | Lifecycle verification: ILSpycmd runbook for S1API and native lifecycle event ordering. |
| **`schedule1-runtime-unity-cache`** | Runtime Unity cache: leak prevention for runtime Texture2D, Sprite, AudioClip and Material objects. |

Skills at `Skills/<skill-name>/SKILL.md` (YAML frontmatter + decision tree + references). Sub-topics under `references/`.

## Build & Deploy

```pwsh
# Build the full solution
pwsh Tools/build-all.ps1

# Or directly via dotnet
dotnet build Source/Mods/S1Mods.sln -c Release

# GameDir override:
$env:SCHEDULE1_PATH = "D:\path\to\Schedule I"

# Quality gates (identical in CI + pre-commit)
dotnet format Source/Mods/S1Mods.sln --verify-no-changes   # formatting
pwsh Tools/gen-sln.ps1                                    # SLN determinism
pwsh Tools/check-version-sync.ps1                         # code <-> mod.json <-> README/AGENTS
dotnet test Source/Tests/Shared.Tests/Shared.Tests.csproj -c Release   # needs game assemblies
```

A successful build deploys automatically (via `Directory.Build.targets`), with split targets since the 2026-09 reinstall:

- `<GameDir>\Mods\` — DLLs, icons (PNG), bundles (only MelonLoader-loadable files)
- `<GameDir>\UserData\<ModName>\` — `mod.json` (metadata) + `<ModName>.pdb` (portable debug symbols, `<DebugType>portable</DebugType>`)

**Never place json/pdb files into `Mods\`.** Default GameDir: `C:\Program Files (x86)\Steam\steamapps\common\Schedule I`.

`SkipUnchangedFiles` is set to `false` (since 2026-08-20) — every `dotnet build` force-deploys, eliminating stale-DLL traps.

> **Post-reinstall status (2026-09-04):** The repo now lives **inside the game directory** (`<GameDir>\Schedule1-mod-only-main`). Setup restored: .NET SDK 8.0.424 installed, S1API 3.2.0 rebuilt from `ThirdParty/S1API/` and deployed (`local.build.props` created from `example.build.props`), NotesApp + Shared built as verification. Update 2026-09-14: alle Mods gebaut/deployed; `Tools/` reaktiviert (9 Helper, inkl. `check-version-sync.ps1`); 20 AI-Skills unter `Skills/`; In-Repo-Decompiles unter `GameReferences/`; Tests unter `Source/Tests/` (Shared + AutoPackagingStation + BackpackMod); CI validiert Format + SLN-Determinismus + Versions-Sync immer, Build/Tests nur mit Spiel-Assemblies.

## Active Mods (as of 2026-09-14)

### 📱 Phone Apps (S1API PhoneApp)

| Mod | Version | Details |
|-----|---------|---------|
| **NotesApp** | v1.0.3 | SafeStorage persistence, real-time search, pinning, quick-stamp, 5 shortcuts |
| **PotScanner** | v0.5.3 | Quick filter tabs, quality rating, focus mode, auto-water, live-cache water threshold |
| **CalculatorApp** | v0.2.3 | Decimal arithmetic, cash/bank integration, clipboard, searchable history |
| **BankApp** | v0.4.4 | Chip-based single-screen ATM UI, weekly limit progress, double-entry booking, slot-isolated |
| **PocketShop** | v0.2.5 | Multi-payment (Cash/Bank/Auto), ItemDetailModal, 2-level navigation, SFX |

### 🎮 Gameplay & QoL

| Mod | Version | Details |
|-----|---------|---------|
| **CustomSkateboard** | v1.1.5 | Ultra-carving, instant-jump, high-speed push, anti-gravel, Nexus ready |
| **HomelessMod** | v0.1.9 | Everywhere building, procedural sleeping bag, Street Nomad questline |
| **BusinessIncome** | v0.1.5 | Daily passive revenue, multiplayer host authority, deterministic variance |
| **Minimap** | v2.0.2 | Dual-shape Radar/Tactical, integrated DayCounter, pooled blips, waypoints, heat ring |
| **MoreSaveSlots** | v1.0.12 | 25+ save slots, paginated navigation, inline renaming, overlay-safe modals |
| **StackLimitMod** | v0.1.3 | Configurable stack limits, runtime registry hook, console commands |
| **BackpackMod** | v1.2.3 | 3D wearable backpacks, tier-based storage, .obj loader, atomic B1 sort |
| **AutoPackagingStation** | v0.2.7 | 4×4 industrial packaging, UV-scroll conveyor, atomic 2-phase engine, host guards |
| **HitmanPhone** | v0.2.7 | Bounty contracts via Messages app, Polaroid dead-drops, heat, quests |
| **_DiagPerfCounter** | v0.3.2 | Dev-tool: StorageEntity hook-target dump to UserData (DEBUG builds only) |

###  Work in Progress

| Mod | Version | Details |
|-----|---------|---------|
| **SnackVendor** | v0.0.2-mvp | Player-stocked vending machine (MVP). Build + placement wired; NPC purchase routing and payout still need in-game verification — see `Source/Mods/SnackVendor/README.md`. |

### 📚 Archived (`Source/Archive/`)

| Mod | Version | Note |
|-----|---------|------|
| **DayCounter** | v1.0.0 | HUD functionality integrated into Minimap → archived |
| **ProfitTracker** | v1.1.0 | Discontinued |
| **TVBrowser** | v0.1.0 | Discontinued |

Archived mods are **not** included in `S1Mods.sln` and are not built. MelonLoader does not load `*.dll.bak` files.

## Status

- [x] Phase 0 — Git baseline (empty workspace committed)
- [x] Phase 1 — Project scaffold + build pipeline
- [x] Phase 2 — UI framework analysis (v0.4.6)
- [x] Phase 3 — Mod selection & architecture
- [x] Phase 4 — Development (active, as of 2026-08-23)

Currently **17 projects** in `S1Mods.sln` (16 mods incl. `SnackVendor` MVP and the `_DiagPerfCounter` dev-tool, plus the `Shared` library; `MoreDrugs` remains third-party), 3 archived. `AGENTS.md` §2 is the single source of truth for mod inventory, versions and day-to-day status — `Tools/check-version-sync.ps1` enforces that the code version matches `mod.json`, `README.md` and `AGENTS.md`.
