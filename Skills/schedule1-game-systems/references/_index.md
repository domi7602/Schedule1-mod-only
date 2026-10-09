# Systems — Quick Reference Index (single authority)

> verified: index consolidated 2026-10-05; f12 static identifier sweep executed 2026-10-08 against the regenerated `GameReferences/decompiled/Assembly-CSharp` (2 234 files, 56 741 identifiers). Per-file verification state is in each file's header; files with drift candidates carry an `Unresolved identifiers` section.

Quick access to all 64 game-system analyses. Each file documents one gameplay subsystem with core classes, events, and entry points.

> **Workflow:** Find the system in the table → read the MD → open vanilla class in `GameReferences/decompiled/Assembly-CSharp/Il2CppScheduleOne/` → check wrapper in `ThirdParty/S1API/S1API/`.

> **Reading the verification headers:** every reference file carries an f12 static identifier sweep dated 2026-10-08, run against `GameReferences/decompiled/Assembly-CSharp` (regenerated from the installed runtime) plus the checked-in S1API/S1MAPI source and the workspace source. `> verified: …` means every identifier-shaped token in the file resolves. `> UNVERIFIED …` means some do not, and those are listed in an `Unresolved identifiers` section at the end of the file — drift **candidates**, not confirmed breakage. Identifiers a file explicitly documents as *absent* are counted as resolved and named in the header, so a file can document a removal without being flagged. Tokens starting lowercase (parameter and variable names) are out of scope. None of this is runtime verification.

> **Depth legend:** 🟢 = deep (formulas/events/hook points/verification) · 🟡 = medium (facts + constants, no hooks) · 🔵 = class-list only (navigation register; deepen on demand) · ↪️ = redirect stub (canonical content merged into another file).

| # | System | Depth | Core Topics |
|---|--------|-------|-------------|
| 01 | [FishNet-Networking](01-FishNet-Networking.md) | 🟢 | SyncVars, RPCs, NetworkSingleton, Server Authority |
| 02 | [Save-Persistence](02-Save-Persistence.md) | 🟡 | JSON saves, GUID Manager, ISaveable, Loader (game-version example outdated — flagged in file) |
| 03 | [Dialogue-Quest](03-Dialogue-Quest.md) | 🟢 | Node graph, Quest subclasses, Journal |
| 04 | [DarkMarket](04-DarkMarket.md) | 🟡 | Night shop, Igor + Oscar, Rank Lock |
| 05 | [Achievement-Steam](05-Achievement-Steam.md) | 🟢 | Steamworks.NET, 13-Achievement table, VariableDatabase |
| 06 | [UI-Phone-HUD](06-UI-Phone-HUD.md) | 🟡 | 3D worldspace phone, App\<T\>, UIScreenManager |
| 07 | [Vehicle](07-Vehicle.md) | 🟢 | WheelCollider, PID, skateboard + map-POI section (Live-verified API observations, 2026-09-25) |
| 08 | [Plant-Growing](08-Plant-Growing.md) | 🟢 | MinPass formula, Soil, Water, Additives, Quality |
| 09 | [Inventory-ItemFramework](09-Inventory-ItemFramework.md) | 🟢 | Item hierarchy, Slots, Registry, Quality, Filters |
| 10 | [Building-Construction](10-Building-Construction.md) | 🟢 | 2D Grid (0.5u), Ghost Preview, BuildManager flow |
| 11 | [Business-Laundering](11-Business-Laundering.md) | 🟡 | Laundering process (24h, free, 1000 cap), Businesses |
| 12 | [Heat-Pursuit-Law](12-Heat-Pursuit-Law.md) | 🟢 | LE_Intensity, 4 Pursuit Levels, 16-Crime fines |
| 13 | [Regions](13-Regions.md) | 🟡 | 6 Regions, Cartel influence (canonical for Cartel), Unlocking |
| 14 | [NPC-Behaviour](14-NPC-Behaviour.md) | 🟡 | NavMesh, Daily routines, Emotions, VO types (canonical for VO) |
| 15 | [Sewer](15-Sewer.md) | 🟡 | Sewer, Goblin probabilities, King, Office, Mushrooms |
| 16 | [Graffiti](16-Graffiti.md) | 🟢 | DecalProjector, 8 colors / 4 brushes, XP/Influence table |
| 17 | [TimeManager](17-TimeManager.md) | 🟢 | Constants, Events table, Sleep flow, Curfew, **Stamina** (renamed from "Energy" after the f12 check) |
| 18 | [Trash-Recycling](18-Trash-Recycling.md) | 🟡 | Global limit 2000, Recycler math, Cleaner priorities |
| 19 | [Customer-Budget](19-Customer-Budget.md) | 🟢 | AdjustedWeeklySpend formula, Bonus table |
| 20 | [XP-Leveling](20-XP-Leveling.md) | 🟡 | 15 XP sources, 11 Ranks |
| 21 | [Casino-Gambling](21-Casino-Gambling.md) | 🟡 | Slots 131.9% RTP (verification date open), Blackjack |
| 22 | [DevConsole](22-DevConsole.md) | 🟡 | Activation via save edit, 8 commands, ~50 utilities |
| 23 | [Economy-Money](23-Economy-Money.md) | 🟡 | Money types, 6 income sources, 4 exploits |
| 24 | [Avatar-CharacterCreator](24-Avatar-CharacterCreator.md) | 🟡 | Creator, Blendshapes, 10 Clothing Slots (verified), 27 colors |
| 25 | [Mixing-Production](25-Mixing-Production.md) | 🟡 | MixMap combinatorics, 35 Effects, 16 Ingredients |
| 26 | [PlayerCamera-SelfDamageBug](26-PlayerCamera-SelfDamageBug.md) | 🟢 | Known bug with root cause + fix snippet |
| 27 | [Tutorial-Prolog](27-Tutorial-Prolog.md) | 🟢 | 16-Quest chain, IntroManager cutscene, RV explosion |
| 28 | [Audio](28-Audio.md) | 🔵 | Sound system |
| 29 | [Combat-Weapons](29-Combat-Weapons.md) | 🔵 | Combat, weapon mechanics |
| 30 | [Cartel](30-Cartel.md) | ↪️ | → canonical: **13-Regions.md** (class register kept) |
| 31 | [Clothing](31-Clothing.md) | 🟢 | 7-type namespace verified; PlayerClothing in PlayerScripts; 10 slots + 27 colors verified; strong S1API.Items.Clothing wrapper |
| 32 | [Configuration](32-Configuration.md) | 🔵 | Config system (+ ModConfig\<T\> mixed in) |
| 33 | [Cutscenes](33-Cutscenes.md) | 🔵 | Cutscene system |
| 34 | [Delivery](34-Delivery.md) | 🔵 | Delivery system |
| 35 | [Doors](35-Doors.md) | 🔵 | Door types, access enums |
| 36 | [Dragging](36-Dragging.md) | 🟢 | Drag mechanics (Taste G) |
| 37 | [DrugEffects](37-DrugEffects.md) | 🟡 | 35-Effect table with types/impact |
| 38 | [Employees](38-Employees.md) | 🟢 | Employee management (overlaps 11/42) |
| 39 | [Equipping](39-Equipping.md) | 🟢 | Equipment system (definitions cross-linked to 09) |
| 40 | [Interaction](40-Interaction.md) | 🟢 | Interaction framework |
| 41 | [Lighting](41-Lighting.md) | 🟢 | Lighting system |
| 42 | [ManagementUI](42-ManagementUI.md) | 🟡 | Clipboard, 10-entity config panel, 9 field-UI types |
| 43 | [MapAndLocations](43-MapAndLocations.md) | ↪️ | → canonical: **13-Regions.md** + 07 map section (class register kept) |
| 44 | [Messaging](44-Messaging.md) | 🟢 | In-game messaging |
| 45 | [Multiplayer](45-Multiplayer.md) | ↪️ | → canonical: **01-FishNet-Networking.md** (lobby register kept) |
| 46 | [Noise](46-Noise.md) | 🟢 | EmitNoise chain, 3 ENoiseType values, Listener/Awareness events, S1API noise events |
| 47 | [ObjectStations](47-ObjectStations.md) | 🟡 | Comprehensive station table (no recipes/events) |
| 48 | [Packaging](48-Packaging.md) | 🟢 | Station modes/states, PackagingTool minigame, EStealthLevel (mixing context → 25) |
| 49 | [Player](49-Player.md) | 🟢 | Player API, events, PlayerHealth, save pattern (details → 17/14) |
| 50 | [PlayerTasks](50-PlayerTasks.md) | 🟢 | Task = State.State, 22 task classes + hierarchy, TaskManager events |
| 51 | [Police](51-Police.md) | ↪️ | → canonical: **12-Heat-Pursuit-Law.md** (class register kept) |
| 52 | [Polling](52-Polling.md) | 🟢 | Web-poll HTTP client (speculation resolved), Steam ticket, PollPanel |
| 53 | [ProductSystem](53-ProductSystem.md) | 🟢 | ProductManager API, EDrugType/EProperty complete, save pattern, S1API Products |
| 54 | [Property](54-Property.md) | 🔵→🟢 | Class register + deep Hyland-Manor section (metadata-derived constants) |
| 55 | [Skateboard](55-Skateboard.md) | ↪️ | → canonical: **07-Vehicle.md** skateboard section (class register kept) |
| 56 | [StationFramework](56-StationFramework.md) | 🔵 | Station base framework |
| 57 | [Storage](57-Storage.md) | 🔵 | Storage system |
| 58 | [Temperature](58-Temperature.md) | 🔵 | Temperature system |
| 59 | [TilesGrid](59-TilesGrid.md) | ↪️ | → canonical: **10-Building-Construction.md** (class register kept) |
| 60 | [TVSystem](60-TVSystem.md) | 🔵 | TV system |
| 61 | [Variables](61-Variables.md) | 🔵 | Variable database |
| 62 | [Vision](62-Vision.md) | 🔵 | Vision/sight system |
| 63 | [VoiceOver](63-VoiceOver.md) | ↪️ | → canonical: **14-NPC-Behaviour.md** (class register kept) |
| 64 | [Weather](64-Weather.md) | 🔵 | Weather system |

## Categories (single source — do not fork this table)

| Category | Systems |
|----------|---------|
| **Core Gameplay** | Growing (08), Mixing (25), Products (53), DrugEffects (37) |
| **Economy** | Economy (23), Business (11), Customers (19), XP (20) |
| **World** | Regions (13), Map (43→13/07), Property (54), Building (10), Doors (35), Tiles (59→10) |
| **NPCs** | NPC Behaviour (14), Employees (38), Dialogue (03), Delivery (34), VoiceOver (63→14) |
| **Law & Order** | Heat/Pursuit (12), Police (51→12), Cartel (30→13), Noise (46) |
| **Player** | Player (49), Avatar (24), Inventory (09), Equipping (39), Skateboard (55→07) |
| **UI & Phone** | UI/Phone/HUD (06), Management UI (42), Dev Console (22), Messaging (44) |
| **Infrastructure** | Networking (01), Multiplayer (45→01), Save (02), Time (17), Config (32), Variables (61) |
| **Environment** | Weather (64), Lighting (41), Audio (28), Temperature (58), Trash (18) |
| **Misc** | Casino (21), Sewer (15), Graffiti (16), TV (60), Cutscenes (33), Tutorial (27), Polling (52), Interaction (40), Dragging (36), Vision (62), Storage (57), Stations (47/56/48) |
| **Known Bugs** | 26-PlayerCamera-SelfDamageBug |
