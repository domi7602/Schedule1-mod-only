# S1MAPI Geometry Persistence — Making Buildings/Meshes/GLTF Survive Save/Reload

> verified: research against S1MAPI source + workspace mods 2026-10-05. S1MAPI.
> UNVERIFIED against the installed runtime. Static identifier sweep 2026-10-08 against the freshly regenerated f12 decompile (`GameReferences/decompiled/Assembly-CSharp`), replacing the earlier decompile-generation check: 59/62 identifier-shaped tokens resolve (0 documented as absent). Unresolved identifiers are listed at the end of this file. Runtime behaviour is not covered by this sweep.

## 1. The Problem — S1MAPI Builds GameObjects, Nothing Else

Everything S1MAPI produces — `ProceduralMeshBuilder` shapes, `BuildingBuilder` buildings, `FurnitureBuilder` furniture, `GltfLoader` models — is a **plain runtime GameObject**. The game's save system never sees it.

Verified against the full S1MAPI source (`ThirdParty/S1MAPI/`, 2026-10-05): a grep for `save|persist|Saveable|ISaveable|DontDestroyOnLoad` across all `.cs` files returns **zero persistence hits** — the only "Saved" matches are runtime NPC chase positions (`InteriorNavigatorCore.cs`). `GltfImporter` parses the JSON fresh on every call (`GltfImporter.cs:294`) with no cache. **Nothing S1MAPI builds survives a save→load cycle unless your mod persists it.** (`world-tools.md` once said "use `networked: true` + S1API `Saveable`" — see §3 for what that actually covers.)

## 2. The 3-Layer Pattern (workspace-canonical)

Proven by `AutoPackagingStation` (`AutoPackStore.cs`) and `HomelessMod` (`StreetPropertyManager.cs`):

**Layer A — Placement registry (in-memory, GUID-keyed).** During play, only track placements in a dictionary: `Dictionary<string guid, PlacementEntry>` (position, rotation, building/prefab ID, live `GameObject` reference). Add on place, remove on dismantle/pack-up. No disk writes during gameplay — this is the anti-dupe rule (`schedule1-grid/references/build-update-patching.md` §6: in-memory only, serialize only on save).

**Layer B — Slot-isolated JSON, written only on `GameLifecycle.OnSaveComplete`.** Flush the registry atomically to `UserData/<ModName>/<state>_slot_{n}.json` via `SafeStorage.SaveAtomic` (workspace shared helper). Reserved details:
- **Slot suffix**: triple-guard derivation (`LoadManager.Instance` → `ActiveSaveInfo` → `SaveSlotNumber >= 0`, fallback last-known/`"default"`); skip saving when the suffix resolves to `"default"` — see `schedule1-persistence/references/slot-isolation.md`.
- **Guards before write** (AutoPackStore.cs:476-495): skip when not in Main scene / scene changing; skip on MP clients — host owns world state (Bug-Audit 2026-09-13: a client write overwrites the host's file with an incomplete replication view).
- **Host-only note**: geometry itself is the host's truth; clients re-receive it via your spawn path on join.

**Layer C — Respawn in `OnLoadComplete`.** Read the JSON back and rebuild. Reserved details:
- **GUID matching, not `Guid.NewGuid()` per session.** For S1MAPI geometry you own the GUID lifecycle: generate once *at placement time*, persist it in the JSON, reuse it on reload. Only for `BuildableItem`s the game restores: use the persistent `BuildableItem.GUID` — `Guid.NewGuid()` in `Awake()` regenerates every session and breaks matching (`schedule1-persistence/references/buildable-restore.md`).
- **BuildableItem restore rule**: if the placed object is a registered `BuildableItem`, **never spawn it yourself in `OnLoadComplete`** — the game restores placed buildables via its own persistence; your extra spawn duplicates it (2 → 3 → 4 … after each cycle). Instead pre-stage save data in a dictionary keyed by GUID and let `BuildableItem.Start()` (Harmony-postfixed) pick it up (AutoPackStore.cs:306-309: "SaveData staged for GUID … awaiting buildable restore").
- **Virtual root + DontDestroyOnLoad** (grid Golden Rule 4): parent mod geometry to a dedicated virtual root (e.g. `StreetNomad_WorldRoot`), `DontDestroyOnLoad` it so registrations survive scene transitions, and **never attach vanilla `Property` components to it**. Treat children as disposable: clear state in `OnPreLoad`, rebuild in `OnLoadComplete`.
- **Never clear state in `OnSceneWasUnloaded`** — it fires for any scene unload (menus) and wipes state before `OnSaveComplete`. Clear only in `OnPreLoad`.
- **Timing**: `OnPreLoad` → `OnLoadComplete` → `OnSaveComplete`; **`OnSaveInfoLoaded` fires 0× in the instrumented session** (verified 2026-09-29, `schedule1-troubleshooting/references/save-load-timing.md` §1). Mods that still hook it defensively reset state at a second guard (scene leave) — belt and braces, not reliance.

## 3. Networked Prefabs (`PrefabPlacer`, `networked: true`)

What the S1MAPI source actually shows (`PrefabPlacer.cs:64`, `NetworkedPrefabLinker.cs:23-27`):

- `networked: true` (default): the prefab is spawned **via FishNet on the server** and replicated to clients; on a client the call returns `null` and queues a deferred link (`NetworkedPrefabLinkerQueue`: 0.5 s poll, max 60 attempts, 1.0 m tolerance) so the replicated object is parented/customized when it arrives. `networked: false` / `PlaceItem` = plain local `Instantiate`, no networking.
- This is **session networking, not persistence**: FishNet replicates a live spawn to clients; it does not write to the game save. S1MAPI contains no save integration for placed prefabs.
- To persist a networked prefab's placement: record it in your Layer-A registry (prefab name + local position/rotation) and re-`Place` it in `OnLoadComplete` — server spawns, clients re-link via the same deferred mechanism.
- **unverified**: whether the vanilla save system would independently restore a FishNet-spawned object across a reload. Plan as if it does not (registry + respawn covers both cases).

## 4. What Does NOT Survive (verify in-game; S1MAPI records nothing)

| Effect | Source evidence | After reload |
|---|---|---|
| `TerrainFlattener` | Mutates `UnityEngine.TerrainData` via `Internal_Get/SetHeights` + `Get/SetDetailLayer` ICalls (`TerrainFlattener.cs:22-43`) — in-memory heightmap/detail edits, no record kept | **unverified** but expected: scene reload on save-load rebuilds the terrain from build data → your flatten reverts. Re-run `FlattenTerrain` in your `OnLoadComplete` rebuild path |
| `TerrainClearer` | Destroys terrain trees/scene objects in the footprint at runtime (`TerrainClearer.cs:62-72`) | **unverified**: destroyed vanilla props are not tracked by S1MAPI; whether the game's save records the removals is unconfirmed — assume you may need to re-clear (or re-spawn) on load |
| `NavigationBuilder` | Carves the baked NavMesh with runtime `NavMeshObstacle`s + custom interior A* + Harmony on `NPCMovement.SetDestination` (`NavigationBuilder.cs:71-86`) | Runtime-only by design — no navmesh data is persisted. Call `Build()`/`Rebuild()` again after every load, from the same rebuild path as the geometry |

Common rule: **make everything S1MAPI-built deterministic from your own state file** — terrain edits, clears, and navmesh are re-applied in the same `OnLoadComplete` rebuild that respawns the geometry.

## 5. Worked Example — Persistable S1MAPI Building

Skeleton compiled from the verified patterns (see AutoPackStore.cs for the production version):

```csharp
using S1Mods.Shared;                 // SafeStorage, SceneGate, NetworkGuard, SaveSlots
using S1MAPI.Building;

public class BuildingSave { public string Guid=""; public string BuildingId=""; public float[] Pos=new float[3]; public float[] Rot=new float[4]; }
public class SaveRoot { public int SaveVersion=1; public List<BuildingSave> Buildings=new(); }

public static class BuildingStore
{
    // Layer A: in-memory registry, GUID-keyed. No disk writes during play.
    static readonly Dictionary<string, (BuildingSave data, Transform live)> _active = new();
    static readonly Dictionary<string, BuildingSave> _staged = new();

    public static void Place(string buildingId, Vector3 pos, Quaternion rot)
    {
        string guid = Guid.NewGuid().ToString();          // once at placement — then persisted (never in Awake)
        var go = new BuildingBuilder("MyBuilding").Build(); // S1MAPI geometry = plain GameObject
        go.transform.position = pos; go.transform.rotation = rot;
        _active[guid] = (new BuildingSave { Guid=guid, BuildingId=buildingId, Pos=V3(pos), Rot=Q(rot) }, go.transform);
    }

    public static void OnPreLoad() { _active.Clear(); _staged.Clear(); }   // reset state — never in OnSceneWasUnloaded

    public static void OnSaveComplete()                                    // Layer B: only here, atomic
    {
        if (!SceneGate.IsInMainScene) return;                              // guards: AutoPackStore.cs:476-495
        string slot = GetSlotSuffix(); if (slot == "default") return;
        if (!NetworkGuard.IsHostOrSingleplayer()) return;                  // host owns world state
        var root = new SaveRoot();
        foreach (var (d, _) in _active.Values) root.Buildings.Add(d);
        SafeStorage.SaveAtomic(SafeStorage.GetUserDataPath("MyBuildingMod", $"buildings_slot_{slot}.json"), root, Mod.Log);
    }

    public static void OnLoadComplete()                                    // Layer C: GUID matching, rebuild
    {
        var root = SafeStorage.LoadSafe<SaveRoot>(SavePath(), new SaveRoot(), Mod.Log);
        foreach (var s in root.Buildings)
            if (_active.ContainsKey(s.Guid)) Apply(s);                     // already live (timing race)
            else _staged[s.Guid] = s;                                      // pre-stage; spawn via rebuild path below
        RebuildAll(root);   // re-spawn geometry + TerrainFlattener + NavigationBuilder.Build() (see §4)
    }
}
// Mod wiring: GameLifecycle.OnPreLoad += BuildingStore.OnPreLoad; same for OnLoadComplete / OnSaveComplete.
```

## 6. Cross-References

- `schedule1-persistence/references/slot-isolation.md` — triple-guard slot suffix + legacy migration (canonical)
- `schedule1-persistence/references/buildable-restore.md` — BuildableItem pre-stage rule + GUID rule
- `schedule1-troubleshooting/references/save-load-timing.md` + `schedule1-persistence/references/safestorage-atomic.md` — verified hook order, atomic writes
- `schedule1-s1api/references/saveables.md` — S1API `Saveable`/`[SaveableField]` alternative (persist inside the save game; constraint table: no Il2Cpp refs in fields)
- `schedule1-grid/SKILL.md` + `references/build-update-patching.md` — virtual root, DontDestroyOnLoad, anti-dupe, Harmony guards
- Real implementations: `Source/Mods/AutoPackagingStation/src/AutoPackStore.cs` (full 3-layer), `Source/Mods/TaxiDriver/` (deterministic GLB rebuild on scene load, no geometry persistence needed), `Source/Archive/HomelessMod/src/StreetPropertyManager.cs` (street items + virtual root)

---

---

---

---

---

---

---

## Unresolved identifiers (f12 static check 2026-10-08)

These documented identifiers were not found in the f12 game assemblies, the checked-in S1API/S1MAPI source, or the workspace source. Treat them as drift candidates and re-derive them from the current decompiles before relying on this document.

- `PlacementEntry`
- `NetworkedPrefabLinker`
- `Internal_Get`
