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
* **NotesApp** (v1.0.2): A fully functional notepad on your in-game phone. Features safe persistence, real-time search, pinning, and quick-stamps for keeping track of your business.
* **CalculatorApp** (v0.2.3): High-precision decimal calculator integrated into your phone. Features in-game money integration (Cash & Bank) and a searchable history.
* **PotScanner** (v0.5.3): The ultimate tool for your grow ops. Provides quick-filter tabs (Thirsty, Ready, Empty), plant quality ratings, and single-property focus modes.
* **BankApp** (v0.4.4): Digital account dashboard for tracking your cash flow and net worth. Features slot-aware cash deposits, weekly ATM limits, and chip-based UI.
* **PocketShop** (v0.2.4): An advanced online shopping interface with multi-payment switching (Cash/Bank/Auto), item inspection modals, and zero-overlay clean shopping.

### 🎮 Gameplay & QoL
* **CustomSkateboard** (v1.1.4): Overhauls the skateboarding experience with ultra-responsive carving, instant jumping, high-speed pushes, and anti-gravel suspension. Visually protects your avatar.
* **HomelessMod** (v0.1.9): Live the street nomad life! Build anywhere, deploy procedural 3D sleeping bags, dismantle outdoor items, and follow the Street Nomad questline.
* **BusinessIncome** (v0.1.4): Earn daily passive revenue for your owned businesses, complete with multiplayer host authority and deterministic variance.
* **Minimap** (v2.0.1): A unified dual-shape viewport (Radar vs Tactical GPS) with dynamic rotation, zoom, pooled blips, dealer markers, police-heat ring, slot-isolated waypoints, health bar, and a settings PhoneApp.
* **StackLimitMod** (v0.1.3): Configurable inventory and storage stack limits (1–9999) with runtime registry patching and console commands.
* **MoreSaveSlots** (v1.0.9): Expands vanilla save slots from 5 to 25+ with paginated navigation and inline save renaming.
* **BackpackMod** (v1.2.2): 3D wearable backpacks with realistic harness straps, tier-based storage scaling (+4/+8/+16 slots), runtime `.obj` model loading, 360° mannequin rotation, and button-only inventory sorting with stack-merge.
* **AutoPackagingStation** (v0.2.6): 4×4 industrial automated packaging line with UV-scroll conveyor belt, atomic 2-phase engine, weighted quality mixing, and native slot sync.
* **HitmanPhone** (v0.2.4): Bounty/contract gameplay via the phone Messages app — anonymous callers, Polaroid evidence dead-drops, police heat, and journal quests.
* **PhoneScroll** (v1.4): Third-party utility by V4LEXL enabling smooth home-screen scrolling for any number of installed PhoneApps.
* **_DiagPerfCounter** (v0.3.2): Dev-tool that dumps StorageEntity hook targets to UserData for mod development (no gameplay effect).

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
- **CI:** GitHub Actions (`.github/workflows/ci.yml`) validates format, build, tests, and SLN determinism on every push/PR.
- **Tests:** `Source/Archive/Tests/Shared.Tests/` (xUnit) covers `Shared` core (`SafeStorage`, `UITheme`, `PatchGuard`).
- **License:** [`LICENSE`](LICENSE) — MIT for workspace code, Third-Party notices for S1API/S1MAPI.
