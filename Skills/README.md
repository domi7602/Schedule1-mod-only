# Schedule I — AI Agent Skills Index

20 Skills unter `Skills/<skill-name>/SKILL.md` — Runbooks, Framework-Referenzen und
Diagnose-Anleitungen für die MelonLoader-Mod-Entwicklung (Spiel v0.4.6f13, S1API 3.2.0).

> **Ladereihenfolge:** Immer zuerst [`schedule1-modding`](schedule1-modding/SKILL.md)
> (primärer Runbook-Skill), danach den passenden Spezial-Skill. Die Pfadangabe
> `Skills/...` ist relativ zum Workspace-Root.

## Kern-Runbooks

| Skill | Wann laden |
|---|---|
| [`schedule1-modding`](schedule1-modding/SKILL.md) | Mod-Runbook: Scaffold, Build, Deploy, Architektur, S1API/UI/Harmony. **Primär-Skill, immer zuerst.** |
| [`schedule1-phoneapp`](schedule1-phoneapp/SKILL.md) | PhoneApp-Entwicklung: Method-3-UI, Input-Focus-Schutz, Lifecycle-Stabilität, WasCollected-Guards. |
| [`schedule1-troubleshooting`](schedule1-troubleshooting/SKILL.md) | Diagnose: `Latest.log`-Triage, Crash-Patterns, Save-Load-Timing, IL2CPP-Pitfalls, Slot-Recovery. |
| [`schedule1-knowledge`](schedule1-knowledge/SKILL.md) | Recherche: Decompiles (`GameReferences/`), S1API-Source, 64 kuratierte Systeme finden statt blind suchen. |

## Framework-Referenzen

| Skill | Wann laden |
|---|---|
| [`schedule1-s1api`](schedule1-s1api/SKILL.md) | S1API-Katalog: Saveables, PhoneApp-Basis, Quests, NPCs, Items, Money, GameTime, Lifecycle. |
| [`schedule1-s1mapi`](schedule1-s1mapi/SKILL.md) | S1MAPI-Katalog: ProceduralMesh, BuildingBuilder, GltfLoader, InteriorBuilder, World-Tools. |
| [`schedule1-game-systems`](schedule1-game-systems/SKILL.md) | 64 Spielsysteme (Growing 08, Inventory 09, Property 54 …): Klassen, Events, Hook-Punkte. |
| [`schedule1-mcp`](schedule1-mcp/SKILL.md) | S1MCP Live-Debugging: TCP-:8765-Bridge, Spiel-State-Inspektion, Log-Capture, Item-Spawning. |

## Domänen-Skills

| Skill | Wann laden |
|---|---|
| [`schedule1-economy`](schedule1-economy/SKILL.md) | Money (Cash/Bank), Business-Revenue, Shop-Multi-Payment, Customers, Laundering — Host-Authority. |
| [`schedule1-persistence`](schedule1-persistence/SKILL.md) | SafeStorage atomic+.bak, slot-isolierte Saves, GameLifecycle-Timing, ModConfig-TOML-Sidecar. |
| [`schedule1-items`](schedule1-items/SKILL.md) | BaseItemDefinition/Registry/StackLimit, Inventory-Slots, Buildable-Injection. |
| [`schedule1-grid`](schedule1-grid/SKILL.md) | Grid & Bauen: Outdoor-Placement, BuildUpdate_Grid-Patching, Ghost-Positionierung. |
| [`schedule1-interiors`](schedule1-interiors/SKILL.md) | Interiors & Minigames: Tür-Hooking, prozedurale Raum-Shells, In-World-Screens, 3D-Ambience. |
| [`schedule1-custom-npcs`](schedule1-custom-npcs/SKILL.md) | Custom-NPCs: NPCPrefabBuilder, Dialogue-Graphen, Tagesabläufe, Custom-Clothing. |
| [`schedule1-3d-assets`](schedule1-3d-assets/SKILL.md) | 3D-Assets & Blender: Export-Pipeline, URP-Shader-Fix, PBR-Materials, Bone-Rigging. |

## Wiederverwendbare Patterns

| Skill | Wann laden |
|---|---|
| [`schedule1-harmony-bootstrap`](schedule1-harmony-bootstrap/SKILL.md) | Harmony-Auto-Discovery: Patch-Klassen finden, applied/skipped/failed zählen, sauber unpatchen. |
| [`schedule1-il2cpp-reflection`](schedule1-il2cpp-reflection/SKILL.md) | IL2CPP-Runtime-Reflection: Array-Bridging, fehlende Overloads, Namespace-Fallback. |
| [`schedule1-debounced-reload`](schedule1-debounced-reload/SKILL.md) | Debounced Live-Reload: FileSystemWatcher-Entprellung, Main-Thread-Pump, Config-Hot-Reload. |
| [`schedule1-runtime-unity-cache`](schedule1-runtime-unity-cache/SKILL.md) | Leak-freie Caches für Texture2D/Sprite/AudioClip/Material bei Runtime-Reloads. |
| [`schedule1-lifecycle-verify`](schedule1-lifecycle-verify/SKILL.md) | Lifecycle-Verifikation: ilspycmd-Runbook für S1API/native Event-Reihenfolge. |

## Konventionen (für Skill-Autoren)

- **Frontmatter:** Jede `SKILL.md` beginnt mit YAML (`name:` = Verzeichnisname, `description:` mit
  Trigger-Sätzen + `Keywords:`). Keine SKILL.md ohne Frontmatter.
- **Version-Anker:** Direkt unter dem Frontmatter steht der Verifikations-Stand, z. B.
  `Game v0.4.6f13 / S1API 3.2.0 / MelonLoader 0.7.3 (verified 2026-09-03)`.
  Nach Spiel- oder S1API-Updates re-verifizieren, nicht nur das Datum anfassen.
- **Detailtiefe:** `SKILL.md` = Decision-Tree + Quick-Refs (schlank halten);
  Details wandern nach `references/*.md` und werden per relativem Link eingebunden.
- **Link-Stile:**
  - Eigene Referenzen: `references/datei.md` (aus `SKILL.md`) bzw. `datei.md` (unter Referenzen).
  - Fremde Skills: `../<skill>/references/datei.md` (relativ, klickbar).
  - Repo-weite Pfade in Prosa: `Skills/<skill>/...` (relativ zum Workspace-Root).
  - Verboten: absolute `file:///`-URLs und bare `schedule1-x/...`-Pfade ohne Präfix.
- **Zeilenenden:** LF (kein CRLF) für alle Skill-Markdown-Dateien.
- **Skill-Count:** Diese Datei, [`AGENTS.md`](../AGENTS.md) (§0) und [`README.md`](../README.md)
  (§ AI Agent Skills) nennen alle dieselbe Skill-Liste — bei neuen Skills alle drei pflegen.
