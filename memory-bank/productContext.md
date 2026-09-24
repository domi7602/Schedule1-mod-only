# Product Context — Schedule1-mod-only

> **Single source of truth is `AGENTS.md`.** This file is session-oriented orientation for agents, **not** an authoritative inventory. In case of conflict `AGENTS.md` (or the code itself) wins. Versions here are only a snapshot (2026-09-19).

---

## What is this?

Private modding workspace for **Schedule I** (TVGS) — a collection of own MelonLoader mods plus a shared library, managed as a single Git repo (`github.com/domi7602/Schedule1-mod-only`).

| Property | Value |
|---|---|
| **Game** | Schedule I v0.4.6f13 (Steam, default branch) |
| **Runtime** | Unity 2022.3, **IL2CPP** (primary), Mono (legacy branch — see AGENTS.md §4) |
| **Loader** | MelonLoader 0.7.3 |
| **TFM / language** | `net6.0`, `LangVersion` 12, `Nullable` enabled |
| **Repo location** | `C:\Users\pc\Schedule1-mod-only` — **outside** the game directory |
| **Game path resolution** | `$env:SCHEDULE1_PATH`, fallback `C:\Program Files (x86)\Steam\steamapps\common\Schedule I` |

## Who works with it?

**Dominik** (owner/developer) — solo developer, no team. The agent (Cline) is the executing engineer. Language: German. Quality bar: "5/5 stars, no half measures", but honesty about "in-game verify open" instead of fake perfection.

## Why (vision)

Stable, maintainable, and **in-game verified** mods. Not maximum mod count, but clean architecture (shared library instead of mod duplicates), defensive IL2CPP practice, and documented drift guards.

## Repository structure (summary)

| Path | Purpose |
|---|---|
| `Source/Mods/` | 15 active mods + `Shared` + `S1Mods.sln` |
| `Source/Archive/` | Archived mods — **do not build, do not deploy** |
| `Source/Tests/` | xUnit projects (Shared, AutoPackagingStation, CalculatorApp, BackpackMod) |
| `GameReferences/` | Local decompiles (gitignored, only present locally) |
| `ThirdParty/` | Pinned dependencies (S1API, S1MCP, Archive) — **read-only for mod code** |
| `Tools/` | 14 PowerShell scripts (build, release, guards, setup) |
| `Skills/` | 20 agent skills (load per task, `schedule1-modding` always first) |
| `docs/` | `architecture.md`, `release-process.md`, `pitfalls.md` |
| `memory-bank/` | Session context (these files) |

## Active mods (snapshot 2026-09-19)

| Mod | Version | Domain |
|---|---|---|
| NotesApp | 1.0.3 | PhoneApp (notes) |
| PotScanner | 0.5.4 | PhoneApp (property/growing) |
| CalculatorApp | 0.2.3 | PhoneApp (calculator) |
| CustomSkateboard | 1.1.5 | Gameplay (skating) |
| MoreSaveSlots | 1.0.12 | QoL (save games) |
| PocketShop | 0.2.7 | PhoneApp (shop, multi-payment) |
| BankApp | 0.4.4 | PhoneApp (banking) |
| BusinessIncome | 0.1.6 | Economy (passive income) |
| StackLimitMod | 0.1.4 | Items (stack limits) |
| AutoPackagingStation | 0.2.8 | Gameplay (industrial packaging line) |
| HitmanPhone | 0.2.9 | PhoneApp (contracts/quests) |
| _DiagPerfCounter | 0.3.2 | Dev tool (not in releases) |
| Shared | — | Workspace library (not in releases) |
| S1MCP | 1.0.1 | Third-party infrastructure (TCP :8765) |

Archived/inactive: DayCounter, HomelessMod, Minimap, ProfitTracker, TVBrowser, BackpackMod, SnackVendor. Removed: PhoneScroll, MoreDrugs, Herer's Minimap.

## Shared library (`Source/Mods/Shared/`)

Central building blocks that **every mod** should use instead of rolling their own:

`PatchGuard` · `SafeStorage` (atomic + `.bak`) · `SafeInvoker` · `GameObjectResolver` · `HotkeyManager` · `ModConfig<T>` · `ModLogger` · `NetworkGuard` · `SceneGate` · `TypeResolver` · `UITheme` (Method 3, `Sp`/`Dp`)

## Non-negotiable constraints (summary)

1. **Deploy convention:** DLL/PNG/bundle → `<GameDir>\Mods\`; `mod.json` + `.pdb` → `<GameDir>\UserData\<Mod>\`. **Never** JSON/PDB in `Mods\`.
2. **IL2CPP discipline:** `IntPtr` ctor for `[RegisterTypeInIl2Cpp]`, `EventHelper`/`ButtonUtils` instead of `UnityAction`, no `foreach`/LINQ on Il2Cpp lists, `WasCollected`/`IsAlive` guards.
3. **Host authority:** MP-relevant economy changes only with `NetworkGuard.IsHostOrSingleplayer`.
4. **Slot isolation:** `slot_{n}`, no `slot_-1.json`.
5. **Archived mods** (`Source/Archive/`) are not built. `Shared`/`_DiagPerfCounter` don't ship in releases.
6. **ThirdParty/** is read-only without explicit instruction.
7. **No** commits/pushes without explicit request; no secrets; no CI guard bypasses.