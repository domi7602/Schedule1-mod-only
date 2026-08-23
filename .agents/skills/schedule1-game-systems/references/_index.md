# Systems — Quick Reference Index

Quick access to all 64 game-system analyses. Each file documents one gameplay subsystem with core classes, events, and entry points.

> **Workflow:** Find the system in the table → read the MD → locate core classes in `../Reference/ScheduleOne_CodeTree.md` → open the source in `Game-Reference/Decompiles/Assembly-CSharp/`.

| # | File | System | Core Topics |
|---|------|--------|-------------|
| 01 | `01-FishNet-Networking.md` | Networking | SyncVars, RPCs, NetworkSingleton, Server Authority |
| 02 | `02-Save-Persistence.md` | Save System | JSON saves, GUID Manager, ISaveable, Loader |
| 03 | `03-Dialogue-Quest.md` | Dialogue & Quests | Node graph, Quest subclasses, Journal |
| 04 | `04-DarkMarket.md` | Dark Market | Night shop, Igor + Oscar, Rank Lock |
| 05 | `05-Achievement-Steam.md` | Achievements | Steamworks.NET, VariableDatabase |
| 06 | `06-UI-Phone-HUD.md` | UI / Phone / HUD | 3D worldspace phone, App<T>, UIScreenManager |
| 07 | `07-Vehicle.md` | Vehicles | WheelCollider, Hover Skateboard, AI Patrol |
| 08 | `08-Plant-Growing.md` | Plant Growing | MinPass tick, Soil, Water, Additives |
| 09 | `09-Inventory-ItemFramework.md` | Inventory & Items | Item hierarchy, Slots, Registry, Quality |
| 10 | `10-Building-Construction.md` | Building | 2D Grid, Ghost Preview, Surface System |
| 11 | `11-Business-Laundering.md` | Business & Laundering | Money laundering, Businesses, Employees |
| 12 | `12-Heat-Pursuit-Law.md` | Law Enforcement | LE_Intensity, 4 Pursuit Levels, 16 Crimes |
| 13 | `13-Regions.md` | Regions | 6 Regions, Cartel influence, Unlocking |
| 14 | `14-NPC-Behaviour.md` | NPC Behaviour | NavMesh (A*), Daily routines, Emotions, Combat |
| 15 | `15-Sewer.md` | Sewer System | Sewer, Goblin, King, Office |
| 16 | `16-Graffiti.md` | Graffiti | DecalProjector, Freehand, Cartel Activity |
| 17 | `17-TimeManager.md` | Time & Sleep | Time, Sleeping, Curfew, Energy |
| 18 | `18-Trash-Recycling.md` | Trash & Recycling | Trash, Recycler, Cleaner |
| 19 | `19-Customer-Budget.md` | Customers | Customers, AdjustedWeeklySpend |
| 20 | `20-XP-Leveling.md` | XP & Leveling | XP sources, Ranks |
| 21 | `21-Casino-Gambling.md` | Casino | Slots 131.9% RTP, Blackjack |
| 22 | `22-DevConsole.md` | Dev Console | Console commands |
| 23 | `23-Economy-Money.md` | Economy | Money types, Income sources |
| 24 | `24-Avatar-CharacterCreator.md` | Avatar & Character | Creator, 10 Clothing Slots |
| 25 | `25-Mixing-Production.md` | Mixing & Production | MixMap, Effects, Stations |
| 26 | `26-PlayerCamera-SelfDamageBug.md` | Known Bug | PlayerCamera self-damage |
| 27 | `27-Tutorial-Prolog.md` | Tutorial | Intro, Starter quests, RV explosion |
| 28 | `28-Audio.md` | Audio | Sound system |
| 29 | `29-Combat-Weapons.md` | Combat & Weapons | Combat, weapon mechanics |
| 30 | `30-Cartel.md` | Cartel | Cartel system |
| 31 | `31-Clothing.md` | Clothing | Clothing system |
| 32 | `32-Configuration.md` | Configuration | Config system |
| 33 | `33-Cutscenes.md` | Cutscenes | Cutscene system |
| 34 | `34-Delivery.md` | Delivery | Delivery system |
| 35 | `35-Doors.md` | Doors | Door mechanics |
| 36 | `36-Dragging.md` | Dragging | Drag mechanics |
| 37 | `37-DrugEffects.md` | Drug Effects | Drug effect system |
| 38 | `38-Employees.md` | Employees | Employee management |
| 39 | `39-Equipping.md` | Equipping | Equipment system |
| 40 | `40-Interaction.md` | Interaction | Interaction framework |
| 41 | `41-Lighting.md` | Lighting | Lighting system |
| 42 | `42-ManagementUI.md` | Management UI | Management interface |
| 43 | `43-MapAndLocations.md` | Map & Locations | Map system, POIs |
| 44 | `44-Messaging.md` | Messaging | In-game messaging |
| 45 | `45-Multiplayer.md` | Multiplayer | Multiplayer system |
| 46 | `46-Noise.md` | Noise | Noise detection |
| 47 | `47-ObjectStations.md` | Object Stations | Station objects |
| 48 | `48-Packaging.md` | Packaging | Product packaging |
| 49 | `49-Player.md` | Player | Player controller |
| 50 | `50-PlayerTasks.md` | Player Tasks | Task system |
| 51 | `51-Police.md` | Police | Police AI & behavior |
| 52 | `52-Polling.md` | Polling | Polling system |
| 53 | `53-ProductSystem.md` | Product System | Product definitions |
| 54 | `54-Property.md` | Property | Property system |
| 55 | `55-Skateboard.md` | Skateboard | Skateboard mechanics |
| 56 | `56-StationFramework.md` | Station Framework | Station base framework |
| 57 | `57-Storage.md` | Storage | Storage system |
| 58 | `58-Temperature.md` | Temperature | Temperature system |
| 59 | `59-TilesGrid.md` | Tiles & Grid | Tile/grid placement |
| 60 | `60-TVSystem.md` | TV System | TV system |
| 61 | `61-Variables.md` | Variables | Variable database |
| 62 | `62-Vision.md` | Vision | Vision/sight system |
| 63 | `63-VoiceOver.md` | Voice Over | VO system |
| 64 | `64-Weather.md` | Weather | Weather system |

## Categories

| Category | Systems |
|----------|---------|
| **Core Gameplay** | Growing (08), Mixing (25), Products (53), DrugEffects (37) |
| **Economy** | Economy (23), Business (11), Customers (19), XP (20) |
| **World** | Regions (13), Map (43), Property (54), Building (10), Doors (35) |
| **NPCs** | NPC Behaviour (14), Employees (38), Dialogue (03), Delivery (34) |
| **Law & Order** | Heat/Pursuit (12), Police (51), Cartel (30), Noise (46) |
| **Player** | Player (49), Avatar (24), Inventory (09), Equipping (39), Skateboard (55) |
| **UI & Phone** | UI/Phone/HUD (06), Management UI (42), Dev Console (22) |
| **Infrastructure** | Networking (01), Save (02), Time (17), Config (32), Variables (61) |
| **Environment** | Weather (64), Lighting (41), Audio (28), Temperature (58), Trash (18) |
| **Misc** | Casino (21), Sewer (15), Graffiti (16), TV (60), Cutscenes (33), Tutorial (27) |
