# Schedule I — AI Agent Skills Index

20 Skills under `Skills/<skill-name>/SKILL.md` — runbooks, framework references, and
diagnostic guides for MelonLoader mod development (game v0.4.7f6, S1API 3.2.1-beta.7).

> **Loading order:** Always first [`schedule1-modding`](schedule1-modding/SKILL.md)
> (primary runbook skill), then the matching specialty skill. The path
> `Skills/...` is relative to the workspace root.

## Core Runbooks

| Skill | When to load |
|---|---|
| [`schedule1-modding`](schedule1-modding/SKILL.md) | Mod runbook: scaffold, build, deploy, architecture, S1API/UI/Harmony. **Primary skill, always first.** |
| [`schedule1-phoneapp`](schedule1-phoneapp/SKILL.md) | PhoneApp development: Method-3 UI, input-focus protection, lifecycle stability, WasCollected guards. |
| [`schedule1-troubleshooting`](schedule1-troubleshooting/SKILL.md) | Diagnostics: `Latest.log` triage, crash patterns, save-load timing, IL2CPP pitfalls, slot recovery. |
| [`schedule1-knowledge`](schedule1-knowledge/SKILL.md) | Research: decompiles (`GameReferences/`), S1API source, 64 curated systems instead of blind search. |

## Framework References

| Skill | When to load |
|---|---|
| [`schedule1-s1api`](schedule1-s1api/SKILL.md) | S1API catalog: Saveables, PhoneApp base, Quests, NPCs, Items, Money, GameTime, Lifecycle. |
| [`schedule1-s1mapi`](schedule1-s1mapi/SKILL.md) | S1MAPI catalog: ProceduralMesh, BuildingBuilder, GltfLoader, InteriorBuilder, World tools. |
| [`schedule1-game-systems`](schedule1-game-systems/SKILL.md) | 64 game systems (Growing 08, Inventory 09, Property 54 …): classes, events, hook points. |
| [`schedule1-mcp`](schedule1-mcp/SKILL.md) | S1MCP live debugging: TCP :8765 bridge, game state inspection, log capture, item spawning. |

## Domain Skills

| Skill | When to load |
|---|---|
| [`schedule1-economy`](schedule1-economy/SKILL.md) | Money (cash/bank), business revenue, shop multi-payment, customers, laundering — host authority. |
| [`schedule1-persistence`](schedule1-persistence/SKILL.md) | SafeStorage atomic + .bak, slot-isolated saves, GameLifecycle timing, ModConfig TOML sidecar. |
| [`schedule1-items`](schedule1-items/SKILL.md) | BaseItemDefinition/Registry/StackLimit, inventory slots, buildable injection. |
| [`schedule1-grid`](schedule1-grid/SKILL.md) | Grid & building: outdoor placement, BuildUpdate_Grid patching, ghost positioning. |
| [`schedule1-interiors`](schedule1-interiors/SKILL.md) | Interiors & minigames: door hooking, procedural room shells, in-world screens, 3D ambience. |
| [`schedule1-custom-npcs`](schedule1-custom-npcs/SKILL.md) | Custom NPCs: NPCPrefabBuilder, dialogue graphs, daily schedules, custom clothing. |
| [`schedule1-3d-assets`](schedule1-3d-assets/SKILL.md) | 3D assets & Blender: export pipeline, URP shader fix, PBR materials, bone rigging. |

## Reusable Patterns

| Skill | When to load |
|---|---|
| [`schedule1-harmony-bootstrap`](schedule1-harmony-bootstrap/SKILL.md) | Harmony auto-discovery: find patch classes, count applied/skipped/failed, clean unpatch. |
| [`schedule1-il2cpp-reflection`](schedule1-il2cpp-reflection/SKILL.md) | IL2CPP runtime reflection: array bridging, missing overloads, namespace fallback. |
| [`schedule1-debounced-reload`](schedule1-debounced-reload/SKILL.md) | Debounced live reload: FileSystemWatcher debouncing, main-thread pump, config hot reload. |
| [`schedule1-runtime-unity-cache`](schedule1-runtime-unity-cache/SKILL.md) | Leak-free caches for Texture2D/Sprite/AudioClip/Material on runtime reloads. |
| [`schedule1-lifecycle-verify`](schedule1-lifecycle-verify/SKILL.md) | Lifecycle verification: ilspycmd runbook for S1API/native event ordering. |

## Conventions (for skill authors)

- **Frontmatter:** Every `SKILL.md` starts with YAML (`name:` = directory name, `description:` with
  trigger sentences + `Keywords:`). No SKILL.md without frontmatter.
- **Version anchor:** Directly below the frontmatter is the verification state, e.g.
  `Game v0.4.7f6 / S1API 3.2.1-beta.7 / MelonLoader 0.7.3 (versions verified 2026-09-28 against live install)`.
  After game or S1API updates re-verify, don't just touch the date.
- **Detail depth:** `SKILL.md` = decision tree + quick refs (keep slim);
  details move to `references/*.md` and are linked via relative link.
- **Link styles:**
  - Own references: `references/file.md` (from `SKILL.md`) or `file.md` (under references).
  - Foreign skills: `../<skill>/references/file.md` (relative, clickable).
  - Repo-wide paths in prose: `Skills/<skill>/...` (relative to workspace root).
  - Forbidden: absolute `file:///` URLs and bare `schedule1-x/...` paths without prefix.
- **Line endings:** LF (no CRLF) for all skill markdown files.
- **Skill count:** This file, [`AGENTS.md`](../AGENTS.md) (§0), and [`README.md`](../README.md)
  (§ AI Agent Skills) all name the same skill list — for new skills update all three.