# Schedule I — Mods Source Directory

This directory contains the source code for all active **MelonLoader (IL2CPP)** mods and the core **`Shared`** library for *Schedule I* (v0.4.6f13).

---

## 📦 Directory Structure

```
Source/Mods/
├── S1Mods.sln                  # Main Visual Studio Solution (all active mods + Shared)
├── Directory.Build.props       # Central MSBuild properties (net6.0, game paths, MelonLoader refs)
├── Directory.Build.targets     # Auto-deployment targets to <GameDir>\Mods\
│
├── Shared/                     # Core library used by all mods (UITheme, SafeStorage, PatchGuard, etc.)
│
├── AutoPackagingStation/       # 4x4 automated packaging station (conveyor belt, quality mixing)
├── BackpackMod/                # 3D wearable backpacks (obj loader, tier storage, mannequin rotation)
├── BankApp/                    # In-game phone ATM & bank dashboard (slot-aware, weekly limit)
├── BusinessIncome/             # Daily passive revenue for owned businesses (host authority)
├── CalculatorApp/              # In-game phone decimal calculator (cash/bank sync, history)
├── CustomSkateboard/           # Skateboard overhaul (carving, instant jump, anti-gravel)
├── HomelessMod/                # Street nomad mod (build anywhere, procedural sleeping bag, quests)
├── Minimap/                    # Radar & Tactical GPS HUD (pooled blips, integrated day/time)
├── MoreSaveSlots/              # Expanded save slots (5 -> 25+, paginated navigation, rename)
├── NotesApp/                   # In-game notepad phone app (SafeStorage, real-time search, pinning)
├── PocketShop/                 # In-game phone online store (Cash/Bank/Auto, item detail modal)
├── PotScanner/                 # Grow-op monitor phone app (filter tabs, plant quality, focus view)
└── StackLimitMod/              # Global configurable stack limits (registry hook, console commands)
```

---

## 🛠️ Build System

### 1. Central Solution (`S1Mods.sln`)
All mods and `Shared` are managed in a single solution with deterministic project GUIDs.

```pwsh
# Build everything in Release mode
dotnet build S1Mods.sln -c Release

# Or use the workspace helper from root:
pwsh Tools/build-all.ps1
```

### 2. Building a Single Mod
```pwsh
dotnet build NotesApp/src/NotesApp.csproj -c Release
```

### 3. Automatic Deployment
`Directory.Build.targets` automatically copies built DLLs, assets (icons, bundles), and `mod.json` files to:
`C:\Program Files (x86)\Steam\steamapps\common\Schedule I\Mods`

> **Note:** `SkipUnchangedFiles` is set to `false` to guarantee that every build deploys immediately.

---

## 🧱 Architecture & Shared Core (`Shared/`)

All mods should leverage `S1Mods.Shared` instead of reinventing common boilerplate:

| Component | Namespace / Class | Purpose |
|---|---|---|
| **SafeStorage** | `S1Mods.Shared.SafeStorage` | Atomic JSON save/load with `.bak` crash recovery and slot isolation (`slot_{id}.json`). |
| **UITheme** | `S1Mods.Shared.UITheme` | Method 3 responsive uGUI scaling (`Sp`, `Dp`) for phone apps & HUDs. |
| **PatchGuard** | `S1Mods.Shared.PatchGuard` | Safe Harmony patching with signature validation and error isolation. |
| **ModConfig** | `S1Mods.Shared.ModConfig<T>` | Type-safe MelonPreferences wrapper with auto-persistence. |
| **ModLogger** | `S1Mods.Shared.ModLogger` | Standardized logging with `[ModName]` prefix. |
| **SafeInvoker** | `S1Mods.Shared.SafeInvoker` | Resilient callback execution protecting the Unity main loop. |

---

## 📋 Mod Inventory Summary

| Mod | Type | Primary Features |
|---|---|---|
| **NotesApp** | PhoneApp | Note taking, real-time search, pinning, quick timestamp stamps |
| **PotScanner** | PhoneApp | Grow container monitor, moisture alerts, quality rating, single-property focus |
| **CalculatorApp** | PhoneApp | High-precision decimal math, cash/bank chips, calculation history |
| **BankApp** | PhoneApp | Mobile banking dashboard, ATM deposit/withdrawal with slot safety |
| **PocketShop** | PhoneApp | Remote shopping with multi-payment (Cash/Bank/Auto) and item inspector |
| **CustomSkateboard** | Gameplay | Ultra-carving, instant jump, anti-gravel, cosmetic deck swapping |
| **HomelessMod** | Gameplay | Everywhere building, procedural 3D sleeping bag, dismantling, nomad quests |
| **BusinessIncome** | Gameplay / Economy | Passive daily payout for businesses with multiplayer host authority |
| **Minimap** | HUD | Circular Radar / Square GPS, pooled blips, merged DayCounter clock |
| **MoreSaveSlots** | QoL | 25+ save slots, paginated navigation, in-line save renaming |
| **StackLimitMod** | QoL / Items | Global stack limit override (1–9999), runtime item registry hook |
| **BackpackMod** | 3D / Equipment | Wearable 3D backpacks, obj loader, tier storage (+4/+8/+16), 360° rotation |
| **AutoPackagingStation** | 3D / Production | 4x4 industrial packaging line, conveyor belt, weighted quality mixing |
| **Shared** | Library | Core utilities, UI theme engine, persistence, patch guard |

---

## ⚡ Technical Guidelines

- **Target Framework:** `net6.0` (C# 12)
- **Mod Loader:** MelonLoader `0.7.3` (IL2CPP backend)
- **API Framework:** S1API `3.2.0`
- **IL2CPP Requirement:** All `[RegisterTypeInIl2Cpp]` classes **must** define a public `IntPtr` constructor:
  ```csharp
  public MyComponent(IntPtr ptr) : base(ptr) { }
  ```
- **Collections:** Never use `foreach` or LINQ over `Il2CppSystem.Collections.Generic.List<T>`. Always use standard `for (int i = 0; i < count; i++)`.
