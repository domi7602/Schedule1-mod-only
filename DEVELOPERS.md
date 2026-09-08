# Schedule I — Modding Workspace (Developers Guide)

*Note: This is the technical documentation for building and contributing to the mods in this repository. For the player-facing mod list and installation guide, please see [README.md](README.md).*

MelonLoader modding workspace for *Schedule I* v0.4.6f13 (TVGS). Fully built on IL2CPP + S1API 3.2.0 + MelonLoader 0.7.3.

## What This Is

- MelonLoader / IL2CPP mods (TFM `net6.0`) targeting Schedule I **v0.4.6f13**
- Mod source only — the decompiled game state is **not** in the repo (regenerate locally via AssetRipper/ilspycmd)
- AI agent skills under `.agents/skills/` (see below)

## Layout

```
Source/Mods/                  Mods + shared lib + Directory.Build.props/targets
Source/Mods/S1Mods.sln        Solution (regeneratable via Tools/gen-sln.ps1)
Source/Archive/               Archived mods (DayCounter, ProfitTracker, TVBrowser), Tests
ThirdParty/                   External frameworks & mod sources (Sideload, hash, MoreDrugs, S1MCP)
Tools/                        (empty after 2026-09 migration — scripts recoverable from git history)
Release/                      Release packages
.agents/skills/               (removed during 2026-09 migration — recoverable from git history)
memory/                       Daily logs `memory/YYYY-MM-DD.md` + `memory-protocol.md`
AGENTS.md                     Workspace conventions & mod inventory for AI agents
README.md                     Player-facing mod overview
DEVELOPERS.md                 This file
```

## AI-Agent Skills

Fourteen skills orchestrate mod work. Load via the `skill` tool (do not open manually):

> **Note:** `.agents/skills/` was intentionally removed during the 2026-09 migration — the table below is kept for reference (recoverable via `git checkout HEAD -- .agents/skills/`).

| Skill | Purpose |
|---|---|
| **`schedule1-modding`** | **Primary skill.** Runbook: scaffold a mod, build, deploy. Architecture patterns (PhoneApp/HUD/Saveable/Harmony/Polling/ModConfig/Console). S1API/UI/IL2CPP cheat sheet. |
| **`schedule1-phoneapp`** | PhoneApp runbook: S1API PhoneApp development, Method 3 responsive UI, InputFocus protection, lifecycle stability (Rule 10/11). |
| **`schedule1-grid`** | Grid & building: outdoor/unrestricted placement, BuildUpdate_Grid patching, ghost positioning, 7 Golden Rules. |
| **`schedule1-s1api`** | S1API framework reference: Saveables, PhoneApp base, Quests, NPCs, Items, Money, GameTime, Lifecycle, cross-branch compatibility. |
| **`schedule1-s1mapi`** | S1MAPI framework reference: ProceduralMesh, BuildingBuilder, GltfLoader, InteriorBuilder, World tools. |
| `schedule1-knowledge` | Research: efficiently find knowledge files, read decompiles, understand system architecture, search recipes. |
| `schedule1-troubleshooting` | Diagnostics: `logscan.py`, crash patterns, save-load timing, IL2CPP pitfalls (11 fragile areas). |
| `schedule1-game-systems` | Game systems: 64 systems (Growing, Inventory, Property, etc.), decision tree, recipes. |
| **`schedule1-economy`** | Economy: Money (cash/bank), Business revenue, Shop multi-payment, Customers, Laundering. |
| **`schedule1-persistence`** | Persistence: SafeStorage atomic .bak, slot_{n}.json, GameLifecycle timing, ModConfig TOML sidecar. |
| **`schedule1-items`** | Items: BaseItemDefinition/Registry/StackLimit, Inventory slots, Buildable injection. |
| **`schedule1-interiors`** | Interiors & Minigames: Door hooking, Procedural room shells, In-world screens, 3D spatial ambience. |
| **`schedule1-3d-assets`** | 3D Assets & Blender: Blender pipeline, URP shader resolution, PBR materials, bone rigging, zero-collider rule. |
| **`schedule1-mcp`** | S1MCP & Live Debugging: Live game introspection, TCP :8765 bridge, log capturing, object reflection. |

Skills at `.agents/skills/<skill-name>/SKILL.md` (YAML frontmatter + decision tree + references). Sub-topics under `references/`.

## Build & Deploy

```pwsh
# Build the full solution
pwsh Tools/build-all.ps1

# Or directly via dotnet
dotnet build Source/Mods/S1Mods.sln -c Release

# GameDir override:
$env:SCHEDULE1_PATH = "D:\path\to\Schedule I"
```

A successful build deploys automatically (via `Directory.Build.targets`), with split targets since the 2026-09 reinstall:

- `<GameDir>\Mods\` — DLLs, icons (PNG), bundles (only MelonLoader-loadable files)
- `<GameDir>\UserData\<ModName>\` — `mod.json` (metadata) + `<ModName>.pdb` (portable debug symbols, `<DebugType>portable</DebugType>`)

**Never place json/pdb files into `Mods\`.** Default GameDir: `C:\Program Files (x86)\Steam\steamapps\common\Schedule I`.

`SkipUnchangedFiles` is set to `false` (since 2026-08-20) — every `dotnet build` force-deploys, eliminating stale-DLL traps.

> **Post-reinstall status (2026-09-04):** The repo now lives **inside the game directory** (`<GameDir>\Schedule1-mod-only-main`). Setup restored: .NET SDK 8.0.424 installed, S1API 3.2.0 rebuilt from `ThirdParty/S1API/` and deployed (`local.build.props` created from `example.build.props`), NotesApp + Shared built as verification. Remaining mods: redeploy via `dotnet build` per mod. `Tools\` and `.agents/skills/` were intentionally removed during the migration (recoverable via `git checkout HEAD -- Tools/ .agents/`).

## Active Mods (as of 2026-08-23)

### 📱 Phone Apps (S1API PhoneApp)

| Mod | Version | Details |
|-----|---------|---------|
| **NotesApp** | v1.0.0 | SafeStorage persistence, real-time search, pinning, quick-stamp, 5 shortcuts |
| **PotScanner** | v0.5.0 | Quick filter tabs, quality rating, focus mode, auto-water, 0-GC polling |
| **CalculatorApp** | v0.2.0 | Decimal arithmetic, cash/bank integration, clipboard, searchable history |
| **BankApp** | v0.1.0 | Digital ATM dashboard, slot-aware deposits/withdrawals, weekly limits |
| **PocketShop** | v0.2.1 | Multi-payment (Cash/Bank/Auto), ItemDetailModal, 2-level navigation, SFX |

### 🎮 Gameplay & QoL

| Mod | Version | Details |
|-----|---------|---------|
| **CustomSkateboard** | v1.0.2 | Ultra-carving, instant-jump, high-speed push, anti-gravel, Nexus ready |
| **HomelessMod** | v0.1.1 | Everywhere building, procedural sleeping bag, Street Nomad questline |
| **BusinessIncome** | v0.1.0 | Daily passive revenue, multiplayer host authority, deterministic variance |
| **Minimap** | v1.0.1 | Dual-shape Radar/Tactical, integrated DayCounter, pooled blips, drag & drop |
| **MoreSaveSlots** | v1.0.1 | 25+ save slots, paginated navigation, inline renaming, Harmony patches |
| **StackLimitMod** | v0.1.0 | Configurable stack limits, runtime registry hook, console commands |
| **BackpackMod** | v1.0.0 | 3D wearable backpacks, spine rig alignment, tier-based storage, .obj loader |
| **AutoPackagingStation** | v0.2.1 | 4×4 industrial packaging, UV-scroll conveyor, atomic 2-phase engine |

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

Currently **13 active mods** in `S1Mods.sln` (plus Shared lib & MoreDrugs as third-party), 3 archived. The `AGENTS.md` is the single source of truth for the day-to-day status of all mods and frameworks.
