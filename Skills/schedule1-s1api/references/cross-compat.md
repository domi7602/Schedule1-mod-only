# S1API — Cross-Compatibility (IL2CPP vs Mono)

> verified: loader mechanism checked against S1API 3.2.1-beta.8 source 2026-10-05; §6 runtime check corrected 2026-10-05 (was an OS check that proved nothing). Anchor: 0.4.7f9-era evidence; runtime 0.4.7f11 per workspace AGENTS.md.

S1API's main promise: **one mod, one build, both branches** (IL2CPP standard + Mono `alternate`). This reference explains how that works, when it breaks, and how to handle the exceptions.

---

## 1. The Two Runtimes

| Branch | Engine | Game code exposed as | Who's on it |
|---|---|---|---|
| **Standard** (this PC) | IL2CPP | `Il2CppScheduleOne.*` (interop proxies) | Default Steam release, ~99% of players |
| **alternate** | Mono | `ScheduleOne.*` (regular .NET) | Beta branch opt-in |

The same game code appears differently in each runtime. **Member signatures, delegate types, and IL stubs differ** — a mod built directly against one runtime will not run on the other.

---

## 2. How S1API Solves This

```
┌─────────────────────┐
│  Your Mod.dll       │  references S1API.Forked (NuGet)
└──────────┬──────────┘
           │
           ▼
┌─────────────────────┐
│  S1API (compiled)   │  wraps the game in stable surfaces
└──────────┬──────────┘
           │
           ▼ runtime-detection (at game start)
┌─────────────────────┐                          ┌─────────────────────┐
│  S1API.Il2Cpp.dll    │  loaded on IL2CPP branch  │  S1API.Mono.dll    │  loaded on Mono branch
└──────────┬──────────┘                          └──────────┬──────────┘
           │                                                │
           ▼                                                ▼
┌─────────────────────┐                          ┌─────────────────────┐
│  Il2CppScheduleOne.* │                          │  ScheduleOne.*       │
└─────────────────────┘                          └─────────────────────┘
```

The `S1APILoader.MelonLoader.dll` plugin detects the runtime and loads the correct S1API build. Your mod only ever sees the unified surface.

---

## 3. The 5 Author Rules (Recap)

1. **Reference `S1API.Forked`** — never `Assembly-CSharp.dll`.
2. **Expose public mod interfaces only with S1API or pure .NET types** — no `Il2CppScheduleOne.*` in your public API.
3. **Use lifecycle events** instead of assuming objects are ready.
4. **Isolate runtime-specific code** behind internal boundaries.
5. **Test both runtime builds** before release.

> S1API's documentation explicitly warns: "Directly using IL2CPP-only or Mono-only game assembly types in your mod API breaks the cross-compatible contract."

---

## 4. When S1API Is Enough

Most use cases are covered by S1API wrappers:

| Task | Wrapper |
|---|---|
| Phone App | `S1API.PhoneApp` |
| NPC | `S1API.Entities` |
| Quest | `S1API.Quests` |
| Item | `S1API.Items` |
| Saveable | `S1API.Saveables` |
| Money | `S1API.Money` |
| Game Time | `S1API.GameTime` |
| Property | `S1API.Property` |
| Saving | `S1API.Lifecycle` (`OnPreLoad`/`OnLoadComplete`/`OnSaveComplete` — **`OnSaveLoaded` does NOT exist** in 3.2.1-beta.8; `OnSaveInfoLoaded` fires 0× on game 0.4.7f6+, verified 2026-09-29) |
| Console | `S1API.Console` |
| Geometry | `S1MAPI` (separate skill) |

---

## 5. When You Must Bypass S1API

| Scenario | Reason |
|---|---|
| Render pipeline tweaks | No wrapper exists |
| Exotic internal manager | `S1API.Internal.*` is mostly `internal` |
| Pre-S1API-version game state | Some legacy methods |
| Frequent deep game state | Many wrapper hops; direct is faster |

### How to Bypass Safely

```csharp
using Il2CppScheduleOne.SomeSystem;   // ❌ in your public API
                                       // ✅ in private/internal code only

public class MyMod
{
    private void DirectInteropExample()
    {
        var vanillaThing = Il2CppScheduleOne.SomeSystem.Instance;
        // ... work with it directly ...
    }

    // Public API returns S1API or pure .NET only
    public int GetThingCount() => /* ... */;
}
```

The `Il2CppScheduleOne.*` access compiles **only** against the IL2CPP build. For Mono-branch support, you'd need to wrap:

```csharp
#if IL2CPP
    using Il2CppScheduleOne.SomeSystem;
#else
    using ScheduleOne.SomeSystem;
#endif
```

But this is exactly what S1API does internally — let S1API handle the abstraction.

---

## 6. The Branch-Pinning Trick (When You Need It)

For mods that genuinely need IL2CPP-specific behavior (e.g., IL2CPP-only optimization):

```csharp
public override void OnInitializeMelon()
{
    bool isIl2Cpp = AppDomain.CurrentDomain.GetAssemblies()
        .Any(a => string.Equals(a.GetName().Name, "Il2CppInterop.Runtime", StringComparison.Ordinal));
    if (isIl2Cpp)
    {
        // IL2CPP-only fast path
    }
}
```

The runtime check is rare; usually S1API handles it. (Canonical check = the `Il2CppInterop.Runtime` assembly probe from `schedule1-il2cpp-reflection` §2 — the older `OSPlatform.Windows` example here proved nothing about the runtime and was fixed 2026-10-05.)

---

## 7. Testing Both Branches

`S1API.Forked` ships both Mono and IL2CPP builds. To test your mod on both:

```pwsh
# IL2CPP build (default in this workspace)
dotnet build Source/Mods/MyMod/src/MyMod.csproj -c Release
# Verify deployment to Game/Mods/, launch game, test

# Mono build (requires alternate branch)
# Switch Steam to "alternate" beta branch
dotnet build Source/Mods/MyMod/src/MyMod.csproj -c MonoRelease
# Verify deployment, launch game, test
```

Most workspace mods are IL2CPP-only by deployment choice (current inventory: [`AGENTS.md`](../../../AGENTS.md) §2). `S1API` 3.2.1-beta.8 supports both via the loader mechanism.

---

## 8. Related: S1MAPI

For **map/building geometry** (procedural meshes, GLTF loading, terrain), the sister framework **S1MAPI** by the same author exists. It is **also branch-portable** (Mono + IL2CPP DLLs) but lives in `UserLibs/` rather than `Plugins/`. See the `schedule1-s1mapi` skill.

---

## 9. Quick Reference

| Concern | Tool |
|---|---|
| Where's the S1API namespace for X? | `ThirdParty/S1API/` (source tree) |
| How does S1API X.X.Y work? | `ThirdParty/S1API/` (source of the wrapper in question) |
| What's the actual signature? | `ThirdParty/S1API/` (namespace folder) |
| Is the runtime branch compatible? | S1API repo README / release notes (Submodule-Pin) |
| Vanilla class signature? | `GameReferences/` (locally generated decompiles, see GameReferences/README.md) |
| Bridging wrapper from S1API to vanilla? | That's literally what S1API does internally |
