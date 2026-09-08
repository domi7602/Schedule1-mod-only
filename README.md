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
* **NotesApp** (v1.0.0): A fully functional notepad on your in-game phone. Features safe persistence, real-time search, pinning, and quick-stamps for keeping track of your business.
* **CalculatorApp** (v0.2.0): High-precision decimal calculator integrated into your phone. Features in-game money integration (Cash & Bank) and a searchable history.
* **PotScanner** (v0.5.0): The ultimate tool for your grow ops. Provides quick-filter tabs (Thirsty, Ready, Empty), plant quality ratings, and single-property focus modes.
* **BankApp** (v0.1.0): Digital account dashboard for tracking your cash flow and net worth. Features slot-aware cash deposits and weekly ATM limits.
* **PocketShop** (v0.2.1): An advanced online shopping interface with multi-payment switching (Cash/Bank/Auto), item inspection modals, and zero-overlay clean shopping.

### 🎮 Gameplay & QoL
* **CustomSkateboard** (v1.0.2): Overhauls the skateboarding experience with ultra-responsive carving, instant jumping, high-speed pushes, and anti-gravel suspension. Visually protects your avatar.
* **HomelessMod** (v0.1.1): Live the street nomad life! Build anywhere, deploy procedural 3D sleeping bags, dismantle outdoor items, and follow the Street Nomad questline.
* **BusinessIncome** (v0.1.0): Earn daily passive revenue for your owned businesses, complete with multiplayer host authority and deterministic variance.
* **Minimap** (v1.0.1): A unified dual-shape viewport (Radar vs Tactical GPS) with dynamic rotation, zoom, pooled blips for tracking points of interest, and an integrated day/time HUD.
* **StackLimitMod** (v0.1.0): Configurable inventory and storage stack limits (1–9999) with runtime registry patching and console commands.
* **MoreSaveSlots** (v1.0.1): Expands vanilla save slots from 5 to 25+ with paginated navigation and inline save renaming.
* **BackpackMod** (v1.0.0): 3D wearable backpacks with realistic harness straps, tier-based storage scaling (+4/+8/+16 slots), runtime `.obj` model loading, and 360° mannequin rotation.
* **AutoPackagingStation** (v0.2.1): 4×4 industrial automated packaging line with UV-scroll conveyor belt, atomic 2-phase engine, weighted quality mixing, and native slot sync.

## 🛠️ Installation

1. Install **[MelonLoader](https://melonwiki.xyz/#/?id=readme)** (version `0.7.3` or newer) into your Schedule I game directory.
2. Ensure you have the **S1API** (Schedule 1 API) installed in your `Mods` folder.
3. Download the latest `.dll` files from the [Releases](../../releases) page.
4. Drop the `.dll` files you want to use into the `Mods` folder of your game directory (`<YourGameDir>\Mods\`).
5. If a release contains `mod.json` or `.pdb` files, place them into `UserData\<ModName>\` (e.g. `UserData\NotesApp\mod.json`) — **never** into `Mods\`.
6. Launch the game! The mods will automatically generate their configuration files in `UserData/`.

## 📸 Screenshots

*(Hey there! Add your in-game screenshots to an `assets/` folder and link them here to show off the mods!)*

## 💻 For Developers

Are you a modder looking to contribute, explore the architecture, or use the AI-Agent frameworks?
Please refer to the **[Developers Guide](DEVELOPERS.md)** for details on the workspace layout, AI skills, and build pipeline.

- **Contributing:** See [`CONTRIBUTING.md`](CONTRIBUTING.md) for setup, `bump-version.ps1`, `dotnet format`, and PR checklist.
- **CI:** GitHub Actions (`.github/workflows/ci.yml`) validates format, build, tests, and SLN determinism on every push/PR.
- **Tests:** `Source/Archive/Tests/Shared.Tests/` (xUnit) covers `Shared` core (`SafeStorage`, `UITheme`, `PatchGuard`).
- **License:** [`LICENSE`](LICENSE) — MIT for workspace code, Third-Party notices for S1API/S1MAPI/Sideload.
