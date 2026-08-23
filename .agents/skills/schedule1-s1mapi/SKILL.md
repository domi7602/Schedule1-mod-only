---
name: schedule1-s1mapi
description: >-
  Author-reference for the S1MAPI framework (ifBars, v2.0.0, deployed as UserLibs\S1MAPI_Il2Cpp.dll) for Schedule I v0.4.6f13 (IL2CPP/Mono).
  Use this skill whenever you need to procedurally generate 3D meshes at runtime, build entire buildings or rooms, load external GLTF/GLB models, clear or flatten terrain, build NPC navigation meshes, or place in-game prefabs with network synchronization.
  Keywords: S1MAPI, ProceduralMesh, MeshBuilder, BuildingBuilder, InteriorBuilder, GltfLoader, glb, TerrainClearer, FlattenTerrain, NavigationBuilder, PrefabPlacer, NetworkPrefab, MaterialPresets, URP, UnityEngine, no AssetBundle, no Assembly-CSharp.
---

# Schedule I — S1MAPI Author Reference (ifBars, v2.0.0)

This skill is the **API catalog** for S1MAPI. S1MAPI is a **mapping and construction library**: generates procedural meshes at runtime, constructs full buildings, loads external 3D models (GLTF/GLB) — **without AssetBundles and without dependency on `Assembly-CSharp`**.

> **Version check (last verified: 2026-08-21):** S1MAPI 2.0.0 deployed in this workspace (`UserLibs\S1MAPI_Il2Cpp.dll`). If the version changed, verify before relying on the API surface here.

---

## 1. Workspace Anchors

| | |
|---|---|
| Workspace Root | `C:\Users\pc\Desktop\Schedule1-mod-only` |
| Game Path | `C:\Program Files (x86)\Steam\steamapps\common\Schedule I` (override via `$env:SCHEDULE1_PATH`) |
| Runtime | MelonLoader 0.7.3 (IL2CPP, Unity 2022.3) |
| S1MAPI DLL (deployed) | `UserLibs\S1MAPI_Il2Cpp.dll` |
| Knowledge snapshot | `Knowledge/Analysis/APIs/S1MAPI.md` (overview), `Knowledge/Analysis/Learnings/ThirdParty/S1MAPI_Il2Cpp.md` (workspace integration) |
| Author | ifBars (same as S1API fork) — GitHub `ifBars/S1MAPI` |
| License | GPL v3 |

> **Branch compatibility:** This workspace is on the **IL2CPP standard branch**. S1MAPI ships DLLs for both (`S1MAPI_Il2Cpp.dll` and `S1MAPI_Mono.dll`) — pick the matching one at runtime. Reference **only the Mono DLL** during compilation; the runtime DLL is the user's responsibility.

---

## 2. Why S1MAPI Exists

Two problems with traditional Schedule I modding:

1. **AssetBundles are fragile** — version-locked, IL2CPP/Mono differ, require Unity Editor roundtrip.
2. **Game code changes break mods** — `Assembly-CSharp` references pin to a specific version.

S1MAPI's design principle: **reference only Unity APIs, not `Assembly-CSharp`**.
- ✅ Update-safe (game patches rarely break it)
- ✅ Branch-neutral (one DLL per runtime, identical API)
- ✅ No AssetBundle pipeline — generate or load models at runtime

---

## 3. What's Complemented by S1API

S1MAPI builds **geometry**. S1API fills **gameplay systems** (NPCs, quests, money).

Typical content mod:
```csharp
using S1MAPI.ProceduralMesh;   // build the building
using S1API.Quests;             // place a quest inside
using S1API.Entities;           // spawn the NPC
```

See the `schedule1-s1api` skill for the S1API side.

---

## 4. Decision Tree — Which Module Do I Need?

| Goal | Module | Read |
|---|---|---|
| Drop a cube/sphere/cylinder in the world | `ProceduralMeshBuilder` | [procedural-mesh.md](references/procedural-mesh.md) |
| Apply a URP-compatible material (opaque, glass, metallic, emissive) | `MaterialPresets` | [materials.md](references/materials.md) |
| Build an entire room/building with floor, walls, ceiling, doors, windows, roof | `BuildingBuilder` | [building.md](references/building.md) |
| Place in-game furniture (desks, chairs, …) inside a custom building | `InteriorBuilder` + `FurnitureType` | [interior.md](references/interior.md) |
| Load a `.glb` / `.gltf` model from disk or embedded resource | `GltfLoader` | [gltf-loading.md](references/gltf-loading.md) |
| Clear vegetation/objects around a build site | `TerrainClearer` | [world-tools.md](references/world-tools.md) |
| Flatten terrain as a building foundation | `FlattenTerrain` | [world-tools.md](references/world-tools.md) |
| Build NPC pathfinding for new areas | `NavigationBuilder` | [world-tools.md](references/world-tools.md) |
| Place in-game prefabs (multiplayer-sync) | `PrefabPlacer` | [world-tools.md](references/world-tools.md) |

---

## 5. Quickstart

```csharp
using MelonLoader;
using S1MAPI.ProceduralMesh;
using UnityEngine;

public class YourMod : MelonMod
{
    public override void OnInitializeMelon()
    {
        GameObject cube = new ProceduralMeshBuilder("MyRedCube")
            .AddBox(new Vector3(0, 1, 0), Vector3.one)
            .SetColor(Color.red)
            .Build();

        LoggerInstance.Msg("Created a red cube!");
    }
}
```

> **Timing:** For objects meant to live in the saved game world, place them only after the world has loaded. Use `S1API.Lifecycle.OnSaveLoaded` or MelonLoader scene callbacks.

---

## 6. Module Overview

### 6.1 ProceduralMesh — Shapes at Runtime

Fluent builder for primitive shapes (box, sphere, cylinder, capsule) as ready-made GameObjects with mesh, collider, and material. See [procedural-mesh.md](references/procedural-mesh.md).

### 6.2 MaterialPresets — URP Materials

Pre-configured URP-compatible material types: opaque, transparent, glass, metallic, emissive. Saves error-prone runtime creation of URP materials (especially under IL2CPP). See [materials.md](references/materials.md).

### 6.3 BuildingBuilder — Entire Buildings

Declarative construction of rooms: floor, ceiling, walls, doors, windows, roof; complex structures with interior walls, trim, and foundation. Output is a walkable building with collision. See [building.md](references/building.md).

### 6.4 InteriorBuilder & FurnitureType — Furnishings

Place in-game furniture (desks, chairs, … via `FurnitureType` enums) inside custom buildings. See [interior.md](references/interior.md).

### 6.5 GltfLoader — External 3D Models

Load `.glb` / `.gltf` models exported from Blender at runtime. Replaces AssetBundles. See [gltf-loading.md](references/gltf-loading.md).

### 6.6 World Tools (Advanced)

`TerrainClearer` (remove surroundings), `FlattenTerrain` (foundation), `NavigationBuilder` (NPC pathfinding), `PrefabPlacer` (in-game prefabs / network-synced interactables). See [world-tools.md](references/world-tools.md).

---

## 7. Installation & DLL Pinning

### As a Player

```pwsh
# Drop into Game/UserLibs/
S1MAPI_Il2Cpp.dll    # for IL2CPP standard branch (this PC)
# or
S1MAPI_Mono.dll      # for Mono "alternate" branch
```

S1MAPI loads automatically **before** mods.

### As a Developer

There is **no NuGet package**. Reference the DLL directly from S1MAPI releases:

```xml
<Reference Include="S1MAPI">
  <HintPath>libs\S1MAPI_Mono.dll</HintPath>
  <Private>false</Private>   <!-- do not copy — located in user's UserLibs -->
</Reference>
```

> Per S1MAPI docs: "It is on the users of the mod to have the correct Mono/Il2Cpp dll installed." Always compile against Mono (the more permissive surface), ship a single DLL that works on both runtimes.

---

## 8. Workflow

```text
1. Conceptualize       (what you want to build: room, terrain, GLTF model)
2. Pick the module     (table in §4)
3. Build hierarchy     (ProceduralMeshBuilder → BuildingBuilder → InteriorBuilder)
4. Apply materials     (MaterialPresets)
5. Position            (Vector3 + Quaternion)
6. Wire navigation     (NavigationBuilder — required for NPCs to walk into new areas)
7. Place interactables (PrefabPlacer for in-game doors, NPC spawn points, …)
8. Save state          (S1API.Saveables + Property / Business binding)
```

For step-by-step recipes: see [recipes.md](references/recipes.md).

---

## 9. Constraints & Pitfalls

| Constraint | Why |
|---|---|
| **No `Assembly-CSharp` references** | S1MAPI's design principle — keeps it branch-portable |
| **No game-system hooks** | Use S1API for NPCs, quests, money, save state |
| **Materials must be URP-compatible** | S1API uses URP shaders; legacy diffuse breaks |
| **NavigationBuilder for NPCs** | Without it, NPCs won't walk into the new area |
| **Build after world load** | Use `S1API.Lifecycle.OnSaveLoaded` — placing objects before world load causes them to be orphaned |

---

## 10. References

* [procedural-mesh.md](references/procedural-mesh.md) — `ProceduralMeshBuilder` for shapes
* [materials.md](references/materials.md) — `MaterialPresets` for URP materials
* [building.md](references/building.md) — `BuildingBuilder` for entire buildings
* [interior.md](references/interior.md) — `InteriorBuilder` + `FurnitureType`
* [gltf-loading.md](references/gltf-loading.md) — `GltfLoader` for `.glb`/`.gltf` models
* [world-tools.md](references/world-tools.md) — Terrain, Navigation, Prefab placement
* [recipes.md](references/recipes.md) — Step-by-step end-to-end examples
* External: [GitHub](https://github.com/ifBars/S1MAPI) · [Thunderstore](https://thunderstore.io/c/schedule-i/p/ifBars/S1MAPI/) · [Nexus Mods](https://www.nexusmods.com/schedule1/mods/1447)
* Local mirror: `Knowledge/Analysis/APIs/S1MAPI.md`
