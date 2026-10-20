> verified 2026-10-20 — three worksteps recorded from actual decompilations; the sizes, hook counts and code references were read out of the decompiled sources. Behaviour claims were not tested in-game.

# Case Notes — three mods, three quality tiers

Worked examples for the runbook. Useful as calibration: the same procedure produces very different
verdicts depending on how the author built the mod.

Artifacts (local, never committed): `Documents/Schedule1-Mod-Reversing/<Mod>-<Version>/`
with `src/` (decompile), the original ZIP and `ANALYSIS.md`.

| | RealEstonia.Roguelike | FaleHouseKeys | CustomBrands |
|---|---|---|---|
| Version analysed | 0.2.1.0 | 1.0.0.0 | 1.1.1.0 |
| DLL size | 72 KB | 192 KB | 1.87 MB |
| Lines of C# | 3.078 | 4.165 | 21.414 |
| TFM | net472 | netcoreapp6.0 | netcoreapp6.0 |
| Framework | raw IL2CPP + Harmony | Il2CppInterop + Harmony | Il2CppInterop + Harmony |
| Hook count | 8 fixed | dynamic (name-pattern scan) | 35 fixed + 3 generated |
| Obfuscated | no | no | no |
| Error reporting | good | **none in release** | exemplary |
| Installed | no | no | no |

## RealEstonia.Roguelike — a small mod with a clean core

Game-mode layer over vanilla: 12 modes, perks, random events, wave combat against Cartel goons.

**What made it easy:** domain rules live in pure C# (`RunRules`, `CombatRules`, `RunOptions`) with
no Unity types — the formulas were readable and testable without the game. Only `GameSession`
(718 lines) touches the engine.

**Transferable patterns:**
- Deterministic pricing: `FNV-1a(itemId)` as RNG salt → the same seed yields the same price list
  across reloads, no shared RNG state, no desync.
- `SHA256(saveFolder|gameSeed|mode)` as the state filename → one run per save × mode, free.
- Borrowing pool objects (`GoonPool.SpawnGoon` → reset → `Despawn`) instead of instantiating NPCs.
- NavMesh spawn ring: radius 12–20 m, min 9 m apart, every point reachable via
  `NavMesh.CalculatePath` → waves only where the player can be reached.

**Risks found:** `ListingUI.Update` postfix runs per frame per listing; the perk-offer state
blocks wave start; a day-jump check kills the run on any day change that is not exactly +1;
goons are taken from the same pool the real cartel uses.

## FaleHouseKeys — the silent one

Physical property keys as items for seven owned properties, with `R` toggling the lock.

**Key technique:** it does not patch fixed types. It scans every assembly for
`Il2CppScheduleOne.*` types whose name contains `.Doors.` / `Gate` / `.Property.` and patches
every method named `CanPlayerAccess`, `SetIsOpen(_Server)`, `SetGate1Open`, `SetGate2Open`,
`AutoOpenForPlayer`, `OpenGate(s)` — with a single `object __instance, object[] __args` prefix.
Robust against renaming; it also hooks doors it does not know about.

**Second technique:** gate locking by disabling trigger colliders named `sensor`/`detector` inside
the gate root, remembering each collider's original `enabled` value. No game API needed.

**The problem:** diagnostics are compiled away. `ReleaseLog` is `[Conditional("FALEHOUSEKEYS_DIAGNOSTICS")]`,
`WarnPatchOnce` writes to a `HashSet` and logs nothing, `OnUpdate` swallows every exception. After a
game update the mod can be half-dead with a clean log — this is the exact failure mode that
`S1Mods.Shared.Diagnostics` exists to prevent.

**Also:** custom items cannot live in the game save, so inventory counts are snapshotted every 10 s
to `UserData/FaleHouseKeys/slot_<n>.json` and keys are re-created after load. No `.bak`, and a
slot-detection fallback named `unknown_slot` merges all saves.

## CustomBrands — the professional build

Branding system: design editor, label printer, custom packaging, sale bonuses, NPC animations.
21k lines across 9 namespaces; the largest single block is NPC animation work.

**Transferable patterns:**
- **Lazy item registration** via a prefix on `Registry._GetItem`: the custom definition is created
  at the moment somebody looks up a `cb_*` id. No migration, works in old saves.
- **Runtime-generated assets**: 3D meshes built in code, icons rendered through a `RenderTexture`
  (256×256, `ReadPixels`), 12 `AudioClip`s synthesised — the DLL ships no media at all.
- **Config with a migration chain**: a `ConfigVersion` entry (6 at analysis time) that raises
  individual values on upgrade instead of resetting the file.
- **Visible diagnostics**: `Safe.Run(where, …)` + `Log.ErrorOnce`, patch counters exposed in a
  startup banner, `HooksOk`/`HooksFailed`.

**Risks found:** three hooks on compiler-generated iterator types (customer consume routine, meth
animation, dealer handover) — they matched the current runtime then and break first on any rebuild;
hot-path postfixes on `get_Name` / `ApplyVisuals`; a 60 s refresh loop re-injecting shops; NPC
joint manipulation that can collide with other animation mods.

**Privacy check:** the only network path is the user-initiated logo import (`HttpClient`, 20 s
timeout, imgur page links rewritten to `i.imgur.com`, Google Drive links to `uc?export=download`).
No telemetry, no update check.

## Lessons that went into the workspace

1. `Diagnostics.ErrorOnce` / `StartupReport` + `PatchGuard.FailedTargets` — the missing half of
   "fail visible" (`docs/pitfalls.md` §1.4).
2. Prefer lazy registration over cloned definitions plus sidecar restore when adding items.
3. A `.bak` is not optional; `File.Move(overwrite: true)` is not an atomic replace.
4. Deterministic seeding beats shared RNG state for anything that has to survive a reload.
