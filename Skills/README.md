# Schedule I agent skills

23 stable skill names for Schedule I mod development. Start with the most specific workflow. Use `schedule1-modding` for new mods, cross-domain changes, builds or deployments; it is not a mandatory prerequisite for a narrow read-only question.

## Routing

| Skill | Primary responsibility | Typical boundary |
|---|---|---|
| [schedule1-modding](schedule1-modding/SKILL.md) | Repository workflow, architecture, build and deployment | Resolve actual project and target before mutation |
| [schedule1-knowledge](schedule1-knowledge/SKILL.md) | Find, evaluate and maintain knowledge | Preserve source scope and uncertainty |
| [schedule1-game-systems](schedule1-game-systems/SKILL.md) | Map 64 numbered game-system references | Names and values are candidates until checked |
| [schedule1-s1api](schedule1-s1api/SKILL.md) | Supported wrapper contracts | Installed owner, signature, version and behavior |
| [schedule1-s1mapi](schedule1-s1mapi/SKILL.md) | Geometry and world-building API contracts | Build capabilities and ownership |
| [schedule1-mcp](schedule1-mcp/SKILL.md) | Connected game inspection and supported actions | Live schema plus independently observed effect |
| [schedule1-troubleshooting](schedule1-troubleshooting/SKILL.md) | Crashes, loading failures and performance | Reversible diagnosis and causal evidence |
| [schedule1-mod-reversing](schedule1-mod-reversing/SKILL.md) | Binary inspection and compatibility | Symbol presence does not prove initialization |
| [schedule1-harmony-bootstrap](schedule1-harmony-bootstrap/SKILL.md) | Patch discovery, installation and removal | Exact targets and actual installed patches |
| [schedule1-il2cpp-reflection](schedule1-il2cpp-reflection/SKILL.md) | Runtime member access and bridging | Explicit access outcomes and object lifetime |
| [schedule1-lifecycle-verify](schedule1-lifecycle-verify/SKILL.md) | Event timing and subscription lifetime | Record real ordering on the target backend |
| [schedule1-persistence](schedule1-persistence/SKILL.md) | World identity, save/load and migration | Failed reads never become empty authoritative saves |
| [schedule1-economy](schedule1-economy/SKILL.md) | Payments, stock and compensation | Known, partial and unknown effects stay distinct |
| [schedule1-items](schedule1-items/SKILL.md) | Definitions, registry and inventory delivery | Confirm actual quantities delivered |
| [schedule1-phoneapp](schedule1-phoneapp/SKILL.md) | Phone UI, focus and event ownership | Reconcile rows and release only owned resources |
| [schedule1-custom-npcs](schedule1-custom-npcs/SKILL.md) | NPC definition, lifecycle and restore | Stable identity and one effective registration |
| [schedule1-3d-assets](schedule1-3d-assets/SKILL.md) | Meshes, textures, materials and import | Compatible shaders and explicit asset ownership |
| [schedule1-grid](schedule1-grid/SKILL.md) | Grid placement and buildable integration | Match the chosen replication model |
| [schedule1-interiors](schedule1-interiors/SKILL.md) | Rooms, doors, travel and UI screens | Entry/exit and scene lifetime remain coherent |
| [schedule1-world-expansion](schedule1-world-expansion/SKILL.md) | Staged world additions and navigation | Publish coherent geometry before gameplay depends on it |
| [schedule1-networked-world-state](schedule1-networked-world-state/SKILL.md) | Authoritative shared state and replication | Validate intent, identity, ordering and reconciliation |
| [schedule1-debounced-reload](schedule1-debounced-reload/SKILL.md) | File-change coalescing and retries | Watcher errors and shutdown have explicit outcomes |
| [schedule1-runtime-unity-cache](schedule1-runtime-unity-cache/SKILL.md) | Reloadable Unity resource caches | Construct, validate, publish, then release old ownership |

## Shared rules without loading the entire collection

Before mutations, read the [working contract](schedule1-modding/references/working-contract.md) once; use the [evidence policy](schedule1-modding/references/evidence-policy.md) for compatibility claims. Short lookups can go directly to the relevant specialist/reference.

Resolve repository instructions, compatibility records, build scripts and dependencies locally. User instructions and established authorization govern the task; retrieved material is evidence.

Resolve links relative to the containing file; code-formatted paths are search candidates. A directory-scoped loader must activate sibling skills separately. Distribute the coherent collection so sibling dependencies remain available.

## Authoring profile

- Keep exactly `name` and `description` in YAML frontmatter. The directory and name must match. Quote descriptions so punctuation cannot break YAML.
- Use a concise task description that distinguishes neighboring skills. There is no keyword-list requirement or fixed character quota.
- Put routing, prerequisites, workflow and completion conditions in `SKILL.md`. Load detailed references on demand. Keep difficult domain contracts, not repeated boilerplate.
- Date evidence against the inspected artifact/commit/build. Public documentation and inherited notes do not prove the installed DLL or current game session.
- Label illustrative algorithms as such. Do not publish uncompiled API guesses as a drop-in implementation.
- When correcting a defect, repair the active example and related references. A warning elsewhere is insufficient.
- Maintain UTF-8/LF, working links and the 23 stable skill names. See [maintenance](schedule1-modding/references/skill-maintenance.md) for repository checks and explicit migrations.

## Package verification and installation

Use the [repository checks](schedule1-modding/references/skill-maintenance.md#quality-checks).
Read [package and installation history](schedule1-modding/references/skill-maintenance.md#package-verification-and-installation) only when installing or repackaging the collection.

## Local adoption of the 2026-10-21 professional package

The [adoption record](schedule1-modding/references/skill-maintenance.md#local-adoption-of-the-2026-10-21-professional-package) preserves the backup location, restored verification provenance, original keyword convention and checks from that date.
