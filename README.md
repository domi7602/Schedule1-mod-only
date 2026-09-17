<div align="center">
  <h1>🛹 Schedule I Mods Collection</h1>
  <p>A high-quality collection of QoL, gameplay, and UI mods for <strong>Schedule I</strong>.</p>
  
  ![Game Version](https://img.shields.io/badge/Game_Version-v0.4.6f13-blue)
  ![MelonLoader](https://img.shields.io/badge/MelonLoader-0.7.3-red)
  ![S1API](https://img.shields.io/badge/S1API-3.2.0-orange)
</div>

## ✨ Featured Mods

This repository contains several active mods, built from the ground up for stability and seamless integration into the game:

### 📱 Phone Apps
* **NotesApp** (v1.0.3): A fully functional notepad on your in-game phone. Features safe persistence, real-time search, pinning, and quick-stamps for keeping track of your business.
* **CalculatorApp** (v0.2.3): High-precision decimal calculator integrated into your phone. Features in-game money integration (Cash & Bank) and a searchable history.
* **PotScanner** (v0.5.4): The ultimate tool for your grow ops. Provides quick-filter tabs (Thirsty, Ready, Empty), plant quality ratings, and single-property focus modes.
* **BankApp** (v0.4.4): Digital account dashboard for tracking your cash flow and net worth. Features slot-aware cash deposits, weekly ATM limits, and chip-based UI.
* **PocketShop** (v0.2.7): An advanced online shopping interface with multi-payment switching (Cash/Bank/Auto), level-lock enforcement, direct quantity picker modal, and item inspection modals.

### 🎮 Gameplay & QoL
* **CustomSkateboard** (v1.1.5): Overhauls the skateboarding experience with ultra-responsive carving, instant jumping, high-speed pushes, and anti-gravel suspension. Visually protects your avatar.
* **BusinessIncome** (v0.1.6): Earn daily passive revenue for your owned businesses, complete with multiplayer host authority and deterministic variance.
* **StackLimitMod** (v0.1.4): Configurable inventory and storage stack limits (1–9999) restricted to agriculture and farming items (soil, seeds, baggies, jars, fertilizers, harvested crops) with permanent weapon and ammo protection.
* **MoreSaveSlots** (v1.0.12): Expands vanilla save slots from 5 to 25+ with paginated navigation and inline save renaming.
* **AutoPackagingStation** (v0.2.7): 4×4 industrial automated packaging line with UV-scroll conveyor belt, atomic 2-phase engine, weighted quality mixing, and native slot sync.
* **HitmanPhone** (v0.2.9): Bounty/contract gameplay via the phone Messages app — anonymous callers, Polaroid evidence dead-drops, police heat, and journal quests.
* **_DiagPerfCounter** (v0.3.2): Dev-tool that dumps StorageEntity hook targets to UserData for mod development (no gameplay effect).

> Archived mods (source preserved, not built/deployed): BackpackMod, DayCounter, HomelessMod, Minimap, ProfitTracker, SnackVendor, TVBrowser — see `Source/Archive/` and [AGENTS.md](AGENTS.md).

## 🧠 AI Agent Skills

The repository includes **20 specialized AI Agent Skills** under `Skills/` (index: [`Skills/README.md`](Skills/README.md)) providing architecture guides, runbooks, and reverse-engineering references:
* **Core Modding:** `schedule1-modding`, `schedule1-phoneapp`, `schedule1-grid`, `schedule1-economy`, `schedule1-persistence`, `schedule1-items`, `schedule1-troubleshooting`, `schedule1-knowledge`, `schedule1-game-systems`
* **World & Visuals:** `schedule1-interiors`, `schedule1-3d-assets`, `schedule1-custom-npcs`, `schedule1-s1mapi`
* **Runtime & Frameworks:** `schedule1-s1api`, `schedule1-mcp`, `schedule1-harmony-bootstrap`, `schedule1-debounced-reload`, `schedule1-il2cpp-reflection`, `schedule1-lifecycle-verify`, `schedule1-runtime-unity-cache`

## 🛠️ Installation

1. Install **[MelonLoader](https://melonwiki.xyz/#/?id=readme)** (version `0.7.3` or newer) into your Schedule I game directory.
2. Ensure you have **S1API** (v3.2.0) installed in your game directory (`Mods\S1API.Il2Cpp.MelonLoader.dll` and `Plugins\S1APILoader.dll`).
3. Download the latest `.dll` files from the [Releases](../../releases) page.
4. Drop the `.dll` files you want to use into the `Mods` folder of your game directory (`<YourGameDir>\Mods\`).
5. If a release contains `mod.json` or `.pdb` files, place them into `UserData\<ModName>\` (e.g. `UserData\NotesApp\mod.json`) — **never** into `Mods\`.
6. Launch the game! The mods will automatically generate their configuration files in `UserData/`.

## 📸 Screenshots

*(Hey there! Add your in-game screenshots to an `assets/` folder and link them here to show off the mods!)*

## 💻 For Developers

Are you a modder looking to contribute, explore the architecture, or use the AI-Agent frameworks?
Please refer to the **[Developers Guide](DEVELOPERS.md)** and **[AGENTS.md](AGENTS.md)** for details on the workspace layout, AI skills, and build pipeline.

- **Contributing:** See [`CONTRIBUTING.md`](CONTRIBUTING.md) for setup, `bump-version.ps1`, `dotnet format`, and PR checklist.
- **Architecture:** [`docs/architecture.md`](docs/architecture.md) defines dependency direction, mod layout, and runtime boundaries.
- **References:** [`GameReferences/README.md`](GameReferences/README.md) explains how to generate local decompiles; [`ThirdParty/README.md`](ThirdParty/README.md) documents pinned external dependencies.
- **Releases:** [`docs/release-process.md`](docs/release-process.md) documents versioning, packaging, and verification.
- **CI:** GitHub Actions (`.github/workflows/ci.yml`) validates format, version sync, build, tests, and SLN determinism on every push/PR.
- **Tests:** `Source/Tests/` (xUnit) — `Shared.Tests` (SafeStorage, UITheme, PatchGuard, ModLogger, SafeInvoker, TypeResolver), `AutoPackagingStation.Tests` (pure math) and `CalculatorApp.Tests` (decimal math, precision, overflow, history). Run all tests via `dotnet test Source/Mods/S1Mods.sln`.
- **License:** [`LICENSE`](LICENSE) — MIT for workspace code, Third-Party notices for S1API/S1MAPI.
