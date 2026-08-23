---
name: schedule1-s1api
description: >-
  Author-reference for the S1API framework (ifBars fork, v3.2.0 deployed, 961 .cs files in 114 namespaces) for Schedule I v0.4.6f13 (IL2CPP/Mono).
  Use this skill whenever you need to find the right S1API namespace for a task, write a Saveable, build a PhoneApp, register a Quest, create a custom NPC, register a console command, hook a lifecycle event, or decide whether to use a S1API wrapper vs direct Il2CppScheduleOne interop.
  Keywords: S1API, Folders, Money, GameTime, Lifecycle, PhoneApp, Quests, NPCs, Saveables, Items, Products, Building, Law, Vehicles, GameLifecycle, OnSaveLoaded, OnSaveLoaded, OnSaveLoaded, Property, Il2CppScheduleOne, MelonLoader.
---

# Schedule I — S1API Author Reference (ifBars fork, v3.2.0)

This skill is the **API-catalog map** for S1API. Use it when you need to know which namespace / class to reach for, what the wired lifecycle hooks are, which patterns are safe via S1API wrapper vs which require direct IL2CPP interop, and how to handle the runtime-neutral contract.

> **Version check (last verified: 2026-08-21):** S1API 3.2.0 deployed in this workspace (`Plugins\S1APILoader.MelonLoader.dll` + `Mods\S1API.Il2Cpp.MelonLoader.dll`). If the game version or S1API version changed, verify before relying on the API surface here.

---

## 1. Workspace Anchors

| | |
|---|---|
| Workspace Root | `C:\Program Files (x86)\Steam\steamapps\common\Schedule I\Schedule 1 Modding` |
| Game Path | `C:\Program Files (x86)\Steam\steamapps\common\Schedule I` (override via `$env:SCHEDULE1_PATH`) |
| Runtime | MelonLoader 0.7.3 (IL2CPP, Unity 2022.3) |
| S1API DLLs (deployed) | `Plugins\S1APILoader.MelonLoader.dll` + `Mods\S1API.Il2Cpp.MelonLoader.dll` |
| S1API decompile (read-only) | `Knowledge/Frameworks/S1API/Decompiles/3.2.0/` (961 .cs files) |
| S1API source/origin | `Knowledge/Frameworks/S1API/VERSIONS.md` + GitHub `ifBars/S1API` |
| Knowledge snapshot | `Knowledge/Analysis/APIs/S1API.md` + `Knowledge/Analysis/s1api-reference/` (13 .md deep-dives) |

> **Branch compatibility:** This workspace is on the **IL2CPP standard branch**. The same S1API source code also supports Mono (`alternate` branch) — see [cross-compat.md](references/cross-compat.md).

---

## 2. The 5 Author Rules (Mandatory)

1. **Reference `S1API.Forked`** — never `Assembly-CSharp.dll`.
2. **Expose public mod interfaces only with S1API or pure .NET types** — no `Il2CppScheduleOne.*` in your public API.
3. **Use lifecycle events** instead of assuming objects are ready.
4. **Isolate runtime-specific code** behind internal boundaries.
5. **Test both runtime builds** before release.

> S1API exposes the game through **stable wrappers** that handle Mono/IL2CPP delegate and member differences. The whole framework is registered with the in-game `HomeScreen`/etc. via `S1APILoader` at startup. Mods that follow these rules automatically become branch-portable.

---

## 3. Decision Tree — Which Namespace Do I Need?

| Question | Namespace | Read |
|---|---|---|
| "When is the save game ready?" | `S1API.Lifecycle` | [lifecycle.md](references/lifecycle.md) |
| "Persist a custom class in the save game" | `S1API.Saveables` (+ `S1API.Internal.Abstraction`) | [saveables.md](references/saveables.md) |
| "Read or change the in-game time" | `S1API.GameTime` | [money-economy.md](references/money-economy.md) |
| "Add or remove money / bank transactions" | `S1API.Money` | [money-economy.md](references/money-economy.md) |
| "Build a phone app (call, ledger, …)" | `S1API.PhoneApp` (+ `S1API.UI`) | [phoneapp.md](references/phoneapp.md) |
| "Create a quest with steps and rewards" | `S1API.Quests` | [quests.md](references/quests.md) |
| "Add a custom NPC to the world" | `S1API.Entities` (+ `S1API.Entities.NPCs.*`) | [entities.md](references/entities.md) |
| "Register a custom console command" | `S1API.Console` | inline below |
| "Custom item / product / packaging" | `S1API.Items.*`, `S1API.Products` | [items-products.md](references/items-products.md) |
| "Wanted level / police behaviour" | `S1API.Law` | [game-systems.md](references/game-systems.md) |
| "Door / building / vehicle" | `S1API.Doors`, `S1API.Building`, `S1API.Vehicles` | [game-systems.md](references/game-systems.md) |
| "Plant / grow / pot" | `S1API.Growing` | [game-systems.md](references/game-systems.md) |
| "Property / business / owned" | `S1API.Property` | [game-systems.md](references/game-systems.md) |
| "Get the Harmonica-loadable UI / panels" | `S1API.UI` (`UIFactory`) | [phoneapp.md](references/phoneapp.md) |
| "Tooltip / HUD / shop / dialogue hooks" | `S1API.Internal.*` (read-only) | inline below |
| "I need to bypass S1API for an edge case" | `Il2CppScheduleOne.*` + Reflection | [cross-compat.md](references/cross-compat.md) |

---

## 4. Namespace Map (114 Namespaces)

### Core (10)
`S1API` · `S1API.Misc` · `S1API.Logging` · `S1API.Internal` · `S1API.Properties` · `S1API.Properties.Interfaces` · `S1API.Properties.Tokens` · `S1API.Saveables` · `S1API.Constants`

### Phone (3)
`S1API.PhoneApp` · `S1API.PhoneCalls` · `S1API.PhoneCalls.Constants` · `S1API.TVApp`

### Game Systems (14)
`S1API.Money` · `S1API.GameTime` · `S1API.Property` · `S1API.Properties` · `S1API.Growing` · `S1API.Items` (+ Additive/Buildable/Clothing/Ingredient/Quality/Storable) · `S1API.Products` (+ Packaging) · `S1API.Stations` · `S1API.Storage(s)` · `S1API.Trash`

### Entities (47)
`S1API.Entities` (+ Actions/Behaviour/Customer/Dealer/Dialogue/Employees/Equippables/Impostors/Interfaces/Relation/Schedule/Supplier/Voices) · `S1API.Entities.Appearances.*` (Avatar) · `S1API.Entities.NPCs` (+ Docks/Downtown/Northtown/PoliceOfficers/Suburbia/Uptown/Westville)

### Quest System (3)
`S1API.Quests` · `S1API.Quests.Constants` · `S1API.Quests.Identifiers`

### Lifecycle & Saves (3)
`S1API.Lifecycle` · `S1API.Internal.Lifecycle` · `S1API.Saveables`

### UI (4)
`S1API.UI` · `S1API.Rendering` · `S1API.Avatar` · `S1API.AssetBundles`

### Audio & FX (1)
`S1API.Audio`

### World (8)
`S1API.Map` (+ Buildings/DeliveryLocations/ParkingLots) · `S1API.Vehicles` · `S1API.Economy` · `S1API.Building` · `S1API.Doors`

### Conditions & State (5)
`S1API.Conditions` · `S1API.Weather` · `S1API.Temperature` · `S1API.Law` · `S1API.Leveling`

### Delivery & Drops (4)
`S1API.Deliveries` · `S1API.DeadDrops` (+ Native) · `S1API.Messaging`

### Dialog & Cutscene (5)
`S1API.Dialogues` · `S1API.Cutscenes` · `S1API.Interaction`

### Shops (3)
`S1API.Shops` · `S1API.Cartel` · `S1API.Casino`

### Avatar (4)
`S1API.Avatar` · `S1API.Graffiti`

### Console (1)
`S1API.Console`

### Utils (1)
`S1API.Utils` (+ `S1API.ReflectionUtils`)

### Input (1)
`S1API.Input` (`Controls.IsTyping` etc.)

### Internal (15+)
`S1API.Internal.*` — **mostly `internal`**, not for mods. See `S1API.Internal.Patches.HomeScreen_Start_Patch` for the auto-discovery mechanism.

---

## 5. Wiring Up The Basics (Every Mod)

```csharp
using MelonLoader;
using S1API.Lifecycle;
using S1API.Logging;

public sealed class MyMod : MelonMod
{
    public override void OnInitializeMelon()
    {
        GameLifecycle.OnSaveLoaded += OnSaveLoaded;
    }

    public override void OnApplicationQuit()
    {
        GameLifecycle.OnSaveLoaded -= OnSaveLoaded;  // defensive unsubscribe
    }

    private static void OnSaveLoaded()
    {
        Log.Info("S1API is loaded and the save is ready.");
        // Now safe to access NPCs, Properties, Businesses, etc.
    }
}
```

> **Why the unsubscribe?** Migrating this skill from the legacy `OnSaveInfoLoaded` namespace to the newer `OnSaveLoaded` — both still exist in 3.2.0. When in doubt, prefer `OnSaveLoaded` for new mods.

---

## 6. Console Command Quickstart

```csharp
using S1API.Console;

[ConsoleCommand("mycmd", "My mod commands")]
public sealed class MyModCommand : BaseConsoleCommand
{
    public override string[] Aliases => new[] { "mycmd", "mymod" };
    public override string Description => "My mod commands";
    public override string Example => "mycmd hello";

    public override void Execute(string[] args)
    {
        if (args.Length == 0)
        {
            ConsoleHelper.Print("Usage: mycmd <subcommand>");
            return;
        }
        switch (args[0].ToLowerInvariant())
        {
            case "hello": ConsoleHelper.Print("Hello!"); break;
            default:      ConsoleHelper.Print($"Unknown: {args[0]}"); break;
        }
    }
}
```

Auto-discovery: any `public class : BaseConsoleCommand` with a parameterless constructor is automatically registered — no manual `Register` call.

---

## 7. Save-Load Timing — The Number-1 Gotcha

**Problem:** Static lists like `Property.OwnedProperties`, `NPCManager.Registered`, `Business.OwnedBusinesses` are often **empty** at `OnGameplaySceneLoaded`.

**Solution:** Use the S1API lifecycle hooks — they fire AFTER the list is populated:

```csharp
GameLifecycle.OnSaveLoaded    += OnSaveLoaded;   // after save-info + property cache
GameLifecycle.OnLoadComplete  += OnLoadComplete; // after scene build
```

**Pitfall:** Never cache vanilla `Quest` / `QuestEntry` references across save loads — S1API replaces them on restore, cached references become stale. Always do fresh lookups via `QuestManager.GetQuestByName(name)`.

See [lifecycle.md](references/lifecycle.md) and [saveables.md](references/saveables.md) for the full patterns.

---

## 8. Common IDE / Discoverability Recipes

```pwsh
# List all S1API public classes
rg -l "public (class|sealed class|abstract class|static class) " "Knowledge/Frameworks/S1API/Decompiles/3.2.0/" --type cs

# Find a specific namespace
ls "Knowledge/Frameworks/S1API/Decompiles/3.2.0/S1API.Quests/"

# Find a method signature
rg -n "public static.*CreateQuest" "Knowledge/Frameworks/S1API/Decompiles/3.2.0/S1API.Quests/"

# Verify a decompile against the running game
& "C:\Users\pc\.dotnet\tools\ilspycmd.exe" -t "S1API.Money.Money" "$env:SCHEDULE1_PATH\Mods\S1API.Il2Cpp.MelonLoader.dll"
```

---

## 9. When NOT to Use S1API

| Scenario | Reason | Alternative |
|---|---|---|
| Render pipeline tweaks | No wrapper exists | Direct `Il2CppScheduleOne.Rendering` + Harmony, isolated to one branch |
| Exotic internal manager | `S1API.Internal.*` is mostly `internal` | Reflection on `Il2CppScheduleOne.*` |
| Frequent deep game state | Many wrapper hops | Bypass S1API, use Harmony on vanilla |
| You only need vanilla Mesh + RuntimeMeshData | S1API is overkill | UnityEngine primitives directly |

For map geometry / buildings / terrain / GLTF models, **S1MAPI** is the right tool (see the `schedule1-s1mapi` skill).

---

## 10. References

* [lifecycle.md](references/lifecycle.md) — `GameLifecycle` hooks + save-load timing
* [saveables.md](references/saveables.md) — `Saveable` base, `[SaveableField]`, JSON persistence
* [phoneapp.md](references/phoneapp.md) — `PhoneApp` base, `UIFactory`, IL2CPP button wiring
* [quests.md](references/quests.md) — `Quest` + `QuestEntry` + `QuestManager`
* [entities.md](references/entities.md) — `NPC` + `NPCPrefabBuilder` + `Entities.NPCs.*`
* [items-products.md](references/items-products.md) — `Items.Storable`, `Items.Buildable`, `Products`
* [money-economy.md](references/money-economy.md) — `Money`, `GameTime`, `Property`, `Law`
* [game-systems.md](references/game-systems.md) — Doors, Vehicles, Growing, Building, Weather, Law, Leveling
* [cross-compat.md](references/cross-compat.md) — IL2CPP vs Mono, when to bypass wrappers
* External: [S1API Docs](https://ifbars.github.io/S1API/) · [API Reference](https://ifbars.github.io/S1API/api/S1API.html) · [GitHub](https://github.com/ifBars/S1API)
* Local mirrors: `Knowledge/Analysis/APIs/S1API.md` · `Knowledge/Analysis/s1api-reference/` (13 .md)
