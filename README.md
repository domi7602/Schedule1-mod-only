<div align="center">

# Schedule I Mods

**A curated collection of phone, gameplay, QoL and utility mods for *Schedule I* (MelonLoader / IL2CPP).**

[![CI](https://github.com/domi7602/Schedule1-mod-only/actions/workflows/ci.yml/badge.svg)](https://github.com/domi7602/Schedule1-mod-only/actions/workflows/ci.yml)
[![Latest release](https://img.shields.io/github/v/release/domi7602/Schedule1-mod-only?include_prereleases&label=release)](https://github.com/domi7602/Schedule1-mod-only/releases)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE)
![Schedule I](https://img.shields.io/badge/Schedule_I-0.4.6f13_·_0.4.7f6_beta-blue)
![MelonLoader](https://img.shields.io/badge/MelonLoader-0.7.3-red)
![S1API](https://img.shields.io/badge/S1API-3.2.x-orange)

[Mods](#mods) · [Installation](#installation) · [Download](#download) · [Screenshots](#screenshots) · [Documentation](#documentation) · [Contributing](#contributing)

</div>

## Overview

This repository hosts a set of independently installable mods for the game *Schedule I* by TVGS. All mods are written in C# for **MelonLoader 0.7.3** on the game's **IL2CPP** (default Steam) branch and build on the community **S1API** framework.

- **For players:** pick the mods you want, drop the DLLs into your game and play. Each mod ships on its own; there is no "all-in-one" package.
- **For modders:** every mod lives in its own project, shares a small common library (`S1Mods.Shared`) and is built from one solution with CI, unit tests, version-drift guards and documented IL2CPP conventions.

The mods are kept in a single repository so they can share the common library, build tooling, tests and documentation while remaining independent at runtime — no mod depends on another mod.

## Compatibility

| Component | Version | Notes |
|---|---|---|
| **Schedule I** | **v0.4.6f13** (Steam default branch) | Most mods were verified in-game on this version (September 2026). |
| **Schedule I** | v0.4.7f6 (Open Beta) | Current development target. `HitmanPhone`, `TaxiDriver` and `MessagesPlus` (v0.4.0, 2026-09-30) are verified on the beta; the other mods have not been re-verified yet. |
| **MelonLoader** | 0.7.3 | IL2CPP build, `net6` runtime. |
| **S1API** | 3.2.1-beta.7 (deployed runtime) | Required by every mod in this repository. Stay on the 3.2.1-beta line for game v0.4.7f6. |
| **S1MAPI** | 2.0.1 (source pin) | Required only by `AutoPackagingStation` and `TaxiDriver` (GLB model loading). |
| **.NET** | `net6.0` (mod target framework) | .NET 6 SDK to build; see [DEVELOPERS.md](DEVELOPERS.md). |

The per-mod verification matrix (which mod was verified on which game version, and when) lives in [`docs/compatibility.md`](docs/compatibility.md).

## Mods

Status legend: **Active** = released and verified in-game · **Active ¹** = code complete, in-game verification still open · **Experimental** = feasibility spike / developer tool.

| Mod | Category | Version | Status | Description |
|---|---|---|---|---|
| [**NotesApp**](Source/Mods/NotesApp/) | Phone | 1.0.4 | Active | In-game notepad with real-time search, pinning, quick in-game timestamps and keyboard shortcuts. Notes are saved atomically per save slot. |
| [**CalculatorApp**](Source/Mods/CalculatorApp/) | Phone | 0.2.4 | Active | Decimal-precision calculator with live cash/bank quick-insert chips, clipboard support and a searchable history. |
| [**PotScanner**](Source/Mods/PotScanner/) | Phone | 0.7.1 | Active | Monitors every grow pot across your properties: filter tabs (thirsty / ready / empty), quality ratings, Water-All and Auto-Water, plus `pot` console commands — in the shared BankApp-style palette. |
| [**BankApp**](Source/Mods/BankApp/) | Phone | 0.4.5 | Active | Mobile banking dashboard: deposit and withdraw cash via quick-amount chips, weekly ATM-limit progress and slot-isolated transaction history. |
| [**PocketShop**](Source/Mods/PocketShop/) | Phone | 0.3.10 | Active | Shop from your phone. Follows each shop's vanilla payment type (cash vs. card), enforces level locks and offers a quantity picker and item detail view. |
| [**Weather**](Source/Mods/Weather/) | Phone | 0.4.2 | Active ¹ | Read-only weather dashboard: accent-bordered hero card with an intensity pill and ring gauge over nine live condition rows that tint themselves while active. |
| [**MessagesPlus**](Source/Mods/MessagesPlus/) | Phone / QoL | 0.5.0 | Active ¹ | Sticky search band under the vanilla title (live name search, category chips, unread counter) plus a "⋯" menu with Clear Read / Clear All and a **permanent whole-app dark mode**. |
| [**HitmanPhone**](Source/Mods/HitmanPhone/) | Gameplay | 0.3.0 | Active | Bounty contracts via the Messages app: anonymous callers, Polaroid evidence dead-drops, police heat and journal quests. |
| [**CustomSkateboard**](Source/Mods/CustomSkateboard/) | Gameplay | 1.1.6 | Active | Adds the *Pro Cyber Skateboard* with tuned carving and jump physics, anti-gravel suspension and purchase through Jeff Gilmore. |
| [**BusinessIncome**](Source/Mods/BusinessIncome/) | Gameplay | 0.1.8 | Active | Daily passive income for owned businesses with multiplayer host authority, deterministic variance and a `biz` console dashboard. |
| [**AutoPackagingStation**](Source/Mods/AutoPackagingStation/) | Gameplay | 0.3.4 | Active | Placeable automated packaging line (conveyor belt, weighted quality mixing, auto-unpack) with a 2×2 footprint, unlocked at Hustler I rank. |
| [**MoreSaveSlots**](Source/Mods/MoreSaveSlots/) | QoL | 1.0.13 | Active | Raises the save-slot count from 5 to 25 (configurable) with paginated menus and inline save renaming. |
| [**StackLimitMod**](Source/Mods/StackLimitMod/) | QoL | 0.1.8 | Active | Configurable stack limits (default 40, 1–9999) for agriculture items (soil, seeds, baggies, jars, fertiliser, harvested crops); weapons and ammo always keep their vanilla limits. |
| [**TaxiDriver**](Source/Mods/TaxiDriver/) | Developer | 0.8.2 | Experimental | Orderable taxi from the in-game phone with its own NPC driver: vehicle spawn, autonomous A→B navigation, player ride, GLB visual swap, road-corridor assistance and an in-game fare meter. Still a spike — the current test round is open. |

Archived mods (source preserved under [`Source/Archive/`](Source/Archive/), not built or shipped): BackpackMod, DayCounter, HomelessMod, Minimap, ProfitTracker, SnackVendor, TVBrowser, `_DiagPerfCounter`. Each mod folder contains its own documentation (`Source/Mods/<Mod>/docs/README.md`) and version history (`Source/Mods/<Mod>/docs/CHANGELOG.md`).

## Featured Mods

<table>
  <tr>
    <td align="center" width="25%">
      <img src="Source/Mods/NotesApp/assets/notiz_app_lowpoly_fancy.png" width="64" alt="NotesApp icon"><br>
      <b>NotesApp</b> · v1.0.4<br>
      <sub>Notepad on your phone with search, pins and in-game timestamps.</sub><br>
      <a href="Source/Mods/NotesApp/docs/README.md">Docs</a> · <a href="Source/Mods/NotesApp/docs/CHANGELOG.md">Changelog</a>
    </td>
    <td align="center" width="25%">
      <img src="Source/Mods/PotScanner/assets/PotScannerIcon.png" width="64" alt="PotScanner icon"><br>
      <b>PotScanner</b> · v0.7.1<br>
      <sub>All grow pots at a glance, Water-All and Auto-Water.</sub><br>
      <a href="Source/Mods/PotScanner/docs/README.md">Docs</a> · <a href="Source/Mods/PotScanner/docs/CHANGELOG.md">Changelog</a>
    </td>
    <td align="center" width="25%">
      <img src="Source/Mods/BankApp/assets/bank_icon.png" width="64" alt="BankApp icon"><br>
      <b>BankApp</b> · v0.4.5<br>
      <sub>Deposit and withdraw from anywhere, weekly limit tracking.</sub><br>
      <a href="Source/Mods/BankApp/docs/README.md">Docs</a> · <a href="Source/Mods/BankApp/docs/CHANGELOG.md">Changelog</a>
    </td>
    <td align="center" width="25%">
      <img src="Source/Mods/PocketShop/assets/pocketshop_icon.png" width="64" alt="PocketShop icon"><br>
      <b>PocketShop</b> · v0.3.10<br>
      <sub>Order from every shop through the phone, vanilla payment rules.</sub><br>
      <a href="Source/Mods/PocketShop/docs/README.md">Docs</a> · <a href="Source/Mods/PocketShop/docs/CHANGELOG.md">Changelog</a>
    </td>
  </tr>
  <tr>
    <td align="center" width="25%">
      <b>AutoPackagingStation</b> · v0.3.4<br>
      <sub>Automated packaging line with conveyor and quality mixing.</sub><br>
      <a href="Source/Mods/AutoPackagingStation/docs/README.md">Docs</a> · <a href="Source/Mods/AutoPackagingStation/docs/CHANGELOG.md">Changelog</a>
    </td>
    <td align="center" width="25%">
      <b>HitmanPhone</b> · v0.3.0<br>
      <sub>Bounty contracts, evidence dead-drops and police heat.</sub><br>
      <a href="Source/Mods/HitmanPhone/docs/README.md">Docs</a> · <a href="Source/Mods/HitmanPhone/docs/CHANGELOG.md">Changelog</a>
    </td>
    <td align="center" width="25%">
      <img src="Source/Mods/CustomSkateboard/assets/icon.png" width="64" alt="CustomSkateboard icon"><br>
      <b>CustomSkateboard</b> · v1.1.6<br>
      <sub>High-performance skateboard sold by Jeff Gilmore.</sub><br>
      <a href="Source/Mods/CustomSkateboard/docs/README.md">Docs</a> · <a href="Source/Mods/CustomSkateboard/docs/CHANGELOG.md">Changelog</a>
    </td>
    <td align="center" width="25%">
      <b>MoreSaveSlots</b> · v1.0.13<br>
      <sub>25 save slots with pagination and renaming.</sub><br>
      <a href="Source/Mods/MoreSaveSlots/docs/README.md">Docs</a> · <a href="Source/Mods/MoreSaveSlots/docs/CHANGELOG.md">Changelog</a>
    </td>
  </tr>
</table>

Pre-built packages, when published, are available on the [Releases page](https://github.com/domi7602/Schedule1-mod-only/releases).

## Screenshots

### Phone Apps

Screenshots for the phone apps (NotesApp, CalculatorApp, PotScanner, PocketShop, Weather) and the gameplay mods (AutoPackagingStation, CustomSkateboard, HitmanPhone, MoreSaveSlots) are still being collected. The folder layout and naming rules for adding them are documented in [`assets/README.md`](assets/README.md) — contributions are welcome.

## Installation

### Requirements

1. **Schedule I** (Steam, IL2CPP default branch) — see [Compatibility](#compatibility).
2. **[MelonLoader](https://melonwiki.xyz/#/?id=readme) 0.7.3** installed into the game directory.
3. **[S1API](https://github.com/ifBars/S1API)** 3.2.x — provides `Mods\S1API.Il2Cpp.MelonLoader.dll` and `Plugins\S1APILoader.dll`. Needed by every mod in this repository.
4. **S1MAPI** (`UserLibs\S1MAPI_Il2Cpp.dll`) — only if you install `AutoPackagingStation` or `TaxiDriver`.

### Install a mod

1. Download the ZIP for the mod you want (see [Download](#download)).
2. Open the ZIP. It mirrors the game folder layout:
   ```text
   <ModName>-vX.Y.Z.zip
   ├── Mods/
   │   ├── <ModName>.dll
   │   ├── Shared.dll            (common library used by all mods here)
   │   └── <icon>.png            (phone apps only)
   ├── UserData/<ModName>/
   │   ├── mod.json
   │   └── <ModName>.pdb
   ├── README.md
   └── CHANGELOG.md
   ```
3. Copy the contents of `Mods/` into `<GameDir>\Mods\`.
4. Copy the `UserData/<ModName>/` folder into `<GameDir>\UserData\`.
5. Start the game. Mods create their configuration files under `UserData\<ModName>\` on first launch.

`<GameDir>` is normally `C:\Program Files (x86)\Steam\steamapps\common\Schedule I`.

### Important

- Only `.dll` and `.png` files belong in `Mods\`. **Never** put `mod.json` or `.pdb` files into `Mods\` — MelonLoader will try to load them and log errors.
- Mods are independent: install only the ones you want. Nothing here requires another mod from this repository.
- `Shared.dll` is the common library (`S1Mods.Shared`) that every mod here uses. Keep exactly one copy in `Mods\` and do not delete it when removing a single mod.
- Phone apps add an icon to the in-game phone home screen; `MessagesPlus` and `MoreSaveSlots` patch existing screens and have no icon.
- Problems? See [`docs/troubleshooting.md`](docs/troubleshooting.md) (log location, common mistakes, how to report a bug).

## Download

Release ZIPs are published per mod on the **[Releases page](https://github.com/domi7602/Schedule1-mod-only/releases)**, named `<ModName>-vX.Y.Z.zip`. The game version each mod was verified against, and the frameworks it needs (MelonLoader, S1API and — where applicable — S1MAPI), are listed in [`docs/compatibility.md`](docs/compatibility.md).

If a mod you want has no published release yet, you can build it from source; see [DEVELOPERS.md](DEVELOPERS.md) and [`docs/release-process.md`](docs/release-process.md). Release packaging requires a Windows machine with Schedule I installed, so releases are produced manually rather than by GitHub-hosted CI.

## Project Structure

```text
Schedule1-mod-only/
├── Source/
│   ├── Mods/              Active mods, one folder each (src/, docs/, assets/)
│   │   ├── Shared/        S1Mods.Shared library used by all mods
│   │   ├── NotesApp/
│   │   ├── PotScanner/
│   │   └── ...
│   ├── Tests/             xUnit test projects
│   └── Archive/           Archived mods (not built)
├── Skills/                AI-agent skills (runbooks and reference material)
├── Tools/                 PowerShell build, release and repository-check scripts
├── GameReferences/        Locally generated game decompiles (ignored by git)
├── ThirdParty/            Pinned dependencies (S1API, S1MAPI submodules) and archives
├── docs/                  Architecture, compatibility, release process, troubleshooting
├── assets/                Screenshots and icon sources for this README
└── .github/               CI workflows, issue and PR templates
```

## Documentation

| Audience | Document |
|---|---|
| Players | This README · [`docs/troubleshooting.md`](docs/troubleshooting.md) · [`docs/compatibility.md`](docs/compatibility.md) · per-mod `docs/README.md` |
| Developers | [`DEVELOPERS.md`](DEVELOPERS.md) · [`CONTRIBUTING.md`](CONTRIBUTING.md) · [`docs/architecture.md`](docs/architecture.md) · [`docs/pitfalls.md`](docs/pitfalls.md) · [`docs/release-process.md`](docs/release-process.md) · [`Source/Mods/README.md`](Source/Mods/README.md) · [`Tools/README.md`](Tools/README.md) |
| AI coding agents | [`AGENTS.md`](AGENTS.md) · [`Skills/README.md`](Skills/README.md) |
| Licensing | [`LICENSE`](LICENSE) (MIT) · [`THIRD-PARTY-NOTICES.md`](THIRD-PARTY-NOTICES.md) |

## AI Agent Skills

The repository includes 20 skills under [`Skills/`](Skills/README.md) — structured runbooks and reference notes that AI coding agents (and humans) can load when working on a mod. They document the project's IL2CPP conventions, S1API/S1MAPI usage and game systems, and are kept in sync with the mods.

| Skill | Purpose |
|---|---|
| [`schedule1-modding`](Skills/schedule1-modding/SKILL.md) | Core mod development runbook (scaffold, build, deploy, architecture). Load first. |
| [`schedule1-phoneapp`](Skills/schedule1-phoneapp/SKILL.md) | Phone applications: responsive UI, input focus, lifecycle rules. |
| [`schedule1-persistence`](Skills/schedule1-persistence/SKILL.md) | Save/data persistence: SafeStorage, slot isolation, lifecycle timing. |
| [`schedule1-grid`](Skills/schedule1-grid/SKILL.md) | Grid-based building and placement systems. |
| [`schedule1-economy`](Skills/schedule1-economy/SKILL.md) | Money, business revenue, shop payments, host authority. |
| [`schedule1-items`](Skills/schedule1-items/SKILL.md) | Item definitions, registry, stack limits, inventory slots. |
| [`schedule1-s1api`](Skills/schedule1-s1api/SKILL.md) | S1API framework reference. |
| [`schedule1-s1mapi`](Skills/schedule1-s1mapi/SKILL.md) | S1MAPI framework reference (meshes, GLTF, world tools). |
| [`schedule1-game-systems`](Skills/schedule1-game-systems/SKILL.md) | Curated notes on 64 game systems and their hook points. |
| [`schedule1-knowledge`](Skills/schedule1-knowledge/SKILL.md) | Research workflow across decompiles and framework sources. |
| [`schedule1-troubleshooting`](Skills/schedule1-troubleshooting/SKILL.md) | Log triage, crash patterns, IL2CPP pitfalls. |
| [`schedule1-mcp`](Skills/schedule1-mcp/SKILL.md) | Live game introspection via the S1MCP bridge. |
| [`schedule1-interiors`](Skills/schedule1-interiors/SKILL.md) | Interiors, doors, procedural rooms, in-world screens. |
| [`schedule1-3d-assets`](Skills/schedule1-3d-assets/SKILL.md) | Blender/3D asset pipeline and URP shader fixes. |
| [`schedule1-custom-npcs`](Skills/schedule1-custom-npcs/SKILL.md) | Custom NPCs, dialogue graphs, schedules. |
| [`schedule1-harmony-bootstrap`](Skills/schedule1-harmony-bootstrap/SKILL.md) | Harmony patch discovery and guarded patching. |
| [`schedule1-il2cpp-reflection`](Skills/schedule1-il2cpp-reflection/SKILL.md) | IL2CPP runtime reflection patterns. |
| [`schedule1-debounced-reload`](Skills/schedule1-debounced-reload/SKILL.md) | Debounced file watching and config hot reload. |
| [`schedule1-lifecycle-verify`](Skills/schedule1-lifecycle-verify/SKILL.md) | Verifying lifecycle event ordering against game assemblies. |
| [`schedule1-runtime-unity-cache`](Skills/schedule1-runtime-unity-cache/SKILL.md) | Leak-free caching of runtime Unity objects. |

## Contributing

Bug reports and feature requests are welcome via the [issue templates](https://github.com/domi7602/Schedule1-mod-only/issues/new/choose). For code contributions read [`CONTRIBUTING.md`](CONTRIBUTING.md) (setup, quality gates, version bumps, PR checklist) and [`DEVELOPERS.md`](DEVELOPERS.md) (build, test, debugging). CI runs formatting, pure-logic tests, solution determinism, version-sync and documentation-path checks on every push.

## License

The code in this repository is licensed under the [MIT License](LICENSE). Third-party components (S1API, S1MAPI, MelonLoader and archived reference material) remain under their own licenses — see [`THIRD-PARTY-NOTICES.md`](THIRD-PARTY-NOTICES.md). This is an unofficial fan project and is not affiliated with TVGS.
