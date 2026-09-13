# Knowledge-Base & Code Reference Inventory

The inventory of code references, decompiles, framework sources, and game system analyses available directly in the repository workspace. **Use this when researching game mechanics, APIs, and vanilla behavior.**

---

## 1. Frameworks & Third-Party Libraries (In-Repo Sources)

| Path | Contents | Purpose & Usage |
|------|----------|-----------------|
| `ThirdParty/S1API/S1API/` | Full C# Source of S1API 3.2.0 | Reference for PhoneApp, Saveables, Registry, Items, UI, NPCs, Quests, GameTime |
| `ThirdParty/MoreDrugs/` | C# Source / Plugin files | Reference for Custom Product Save Provider & Item Expansion |
| `ThirdParty/S1MCPServer-master/` | C# Source of S1MCP | Reference for Live Runtime Introspection & TCP Server :8765 |
| `ThirdParty/PhoneScroll/` | DLL + Documentation | Reference for Phone Home Screen scrolling (Closed Source, V4LEXL) |
| `<GameDir>/Mods/` | Runtime deployed DLLs | Live runtime binaries loaded by MelonLoader 0.7.3 |
| `<GameDir>/Plugins/` | `S1APILoader.dll` | S1API lifecycle bootstrap plugin |

---

## 2. Game-Reference — Decompiles (Vanilla Schedule I Source)

### 2.1. Assembly-CSharp (★ In-Repo Decompile: ~2100+ files)

`GameReferences/decompiled/Assembly-CSharp/` contains the full IL2CPP-aware decompile of *Schedule I* v0.4.6f13. Organized by namespace:

```
GameReferences/decompiled/Assembly-CSharp/
├── Assembly-CSharp.csproj               (for IDE navigation)
├── Il2Cpp/                              (interop primitives)
├── Il2CppScheduleOne/                   (★ ~69 sub-namespaces; game core)
│   ├── Audio/ AvatarFramework/ Building/ Calling/ Cartel/ Casino/ Clothing/
│   ├── Combat/ Configuration/ Dialogue/ Doors/ Dragging/ Economy/ Effects/
│   ├── Employees/ EntityFramework/ Equipping/ Events/ Gamepad/ GameTime/
│   ├── Growing/ Heatmap/ Input/ Interaction/ ItemFramework/ Law/ Levelling/
│   ├── Lighting/ Management/ Map/ Materials/ Math/ Messaging/ Misc/ Money/
│   ├── Networking/ NPCs/ Noise/ ObjectScripts/ Packaging/ Persistence/
│   ├── PlayerScripts/ PlayerTasks/ Police/ Polling/ Product/ Property/
│   ├── Quests/ Reporting/ ScriptableObjects/ Skating/ State/ Storage/
│   ├── StationFramework/ Temperature/ Tiles/ Tools/ Trash/ TV/ UI/
│   ├── Variables/ Vehicles/ Vision/ VoiceOver/ Weather/
├── Il2CppFishNet/                       (FishNet networking core)
├── Il2CppFishySteamworks/               (Steam transport layer)
└── Il2CppLiquidVolumeFX/ ...            (visual rendering components)
```

**Quick-Lookup tips:**
- Search via ripgrep (`grep_search` or `rg`):
  ```pwsh
  rg "class GrowContainer" "GameReferences/decompiled/Assembly-CSharp/Il2CppScheduleOne/Growing"
  ```
- Game namespaces use the IL2CPP prefix `Il2CppScheduleOne.<Area>` (e.g. `Il2CppScheduleOne.Growing`, `Il2CppScheduleOne.Persistence`).
- `Assembly-CSharp-firstpass` contains native plugins and first-pass Unity assemblies.

---

## 3. Game Systems Architecture (64 In-Repo Analyses)

Curated architecture reference documents located in `Skills/schedule1-game-systems/references/`:

- **Index & Quick Reference:** `Skills/schedule1-game-systems/references/_index.md`
- **System Guide:** `Skills/schedule1-game-systems/references/README.md`

| Category | Reference Files |
|----------|-----------------|
| **Core Gameplay** | `08-Plant-Growing.md`, `25-Mixing-Production.md`, `53-ProductSystem.md`, `37-DrugEffects.md` |
| **Economy & Trade** | `23-Economy-Money.md`, `11-Business-Laundering.md`, `19-Customer-Budget.md`, `04-DarkMarket.md` |
| **World & Map** | `13-Regions.md`, `43-MapAndLocations.md`, `54-Property.md`, `10-Building-Construction.md`, `35-Doors.md` |
| **NPCs & Dialogue** | `14-NPC-Behaviour.md`, `38-Employees.md`, `03-Dialogue-Quest.md`, `34-Delivery.md` |
| **Law & Enforcement** | `12-Heat-Pursuit-Law.md`, `51-Police.md`, `30-Cartel.md`, `46-Noise.md` |
| **Player & Character** | `49-Player.md`, `24-Avatar-CharacterCreator.md`, `09-Inventory-ItemFramework.md`, `39-Equipping.md`, `55-Skateboard.md` |
| **UI & Phone** | `06-UI-Phone-HUD.md`, `42-ManagementUI.md`, `22-DevConsole.md`, `44-Messaging.md` |
| **Infrastructure** | `01-FishNet-Networking.md`, `02-Save-Persistence.md`, `17-TimeManager.md`, `32-Configuration.md`, `61-Variables.md` |
| **Environment & Audio** | `64-Weather.md`, `41-Lighting.md`, `28-Audio.md`, `58-Temperature.md`, `18-Trash-Recycling.md` |
| **Progression & Minigames** | `20-XP-Leveling.md`, `21-Casino-Gambling.md`, `15-Sewer.md`, `16-Graffiti.md`, `60-TVSystem.md` |

---

## 4. Live Runtime Tools & Diagnostics

| Tool | Location / Usage | Purpose |
|------|------------------|---------|
| **S1MCP Bridge** | `ThirdParty/S1MCPServer-master/` (TCP port 8765) | Live game inspection, scene hierarchy queries, object reflection, log streaming |
| **ILSpy CLI (`ilspycmd`)** | Global dotnet tool: `ilspycmd` | Decompile live game assemblies in `<GameDir>/MelonLoader/Il2CppAssemblies/` |
| **MelonLoader Logs** | `<GameDir>/MelonLoader/Latest.log` | Real-time diagnostic log stream with exception backtraces and warning patterns |
| **DiagPerfCounter** | `Source/Mods/_DiagPerfCounter/` | In-game diagnostic reflection dump |

---

## Quick-Reference: "I need to …"

- **Find a vanilla game class or method** → `GameReferences/decompiled/Assembly-CSharp/Il2CppScheduleOne/<Area>/`
- **Understand a game system's logic and fields** → `Skills/schedule1-game-systems/references/XX-<System>.md`
- **Check S1API public interfaces or implementations** → `ThirdParty/S1API/S1API/`
- **Inspect live game state or spawn items** → `Skills/schedule1-mcp/SKILL.md` (S1MCP TCP :8765)
- **Check save-game format & persistence events** → `Skills/schedule1-game-systems/references/02-Save-Persistence.md` + `Skills/schedule1-persistence/SKILL.md`
- **Check MelonLoader log for errors or GC issues** → Run pwsh: `Get-Content "$env:SCHEDULE1_PATH\MelonLoader\Latest.log" -Tail 200 | Select-String -Pattern '\[ERROR\]|\[WARNING\]|Exception|WasCollected'`
