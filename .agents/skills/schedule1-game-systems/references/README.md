# System Reference: Schedule I

Analysis of 64 gameplay systems, based on the decompiled C# source code (AssetRipper export, 22 GB).

## Overview

### Core Systems
| # | System | Description |
|---|--------|-------------|
| 01 | [FishNet-Networking](01-FishNet-Networking.md) | SyncVars, RPCs, Server authority, NetworkSingleton |
| 02 | [Save/Persistence](02-Save-Persistence.md) | JSON save, GUID manager, Loader, ISaveable |
| 03 | [Dialogue/Quest](03-Dialogue-Quest.md) | Custom node graph, Quest subclasses, Journal |
| 04 | [DarkMarket](04-DarkMarket.md) | Night shop, Igor + Oscar, Rank lock |
| 05 | [Achievement/Steam](05-Achievement-Steam.md) | Steamworks.NET, Event-based, VariableDatabase |
| 06 | [UI/Phone/HUD](06-UI-Phone-HUD.md) | 3D worldspace phone, App\<T\>, UIScreenManager |

### Gameplay Systems
| # | System | Description |
|---|--------|-------------|
| 07 | [Vehicle](07-Vehicle.md) | WheelCollider, Hover skateboard, AI patrol |
| 08 | [Plant/Growing](08-Plant-Growing.md) | MinPass tick, Soil, Water, Additives, Quality |
| 09 | [Inventory/ItemFramework](09-Inventory-ItemFramework.md) | Item hierarchy, Slot system, Registry, Quality |
| 10 | [Building/Construction](10-Building-Construction.md) | 2D grid, Ghost preview, Surface system |
| 11 | [Business/Laundering](11-Business-Laundering.md) | Money laundering, Business class, Employee management |
| 12 | [Heat/Pursuit/Law](12-Heat-Pursuit-Law.md) | LE_Intensity, 4 pursuit levels, 16 crimes, Body search |
| 13 | [Regions](13-Regions.md) | 6 regions, Cartel influence, Unlock mechanics |
| 14 | [NPC Behaviour](14-NPC-Behaviour.md) | NavMesh, Daily schedules, Emotions, Combat |
| 15 | [Sewer](15-Sewer.md) | Sewer system, Goblin, King, Office, Mushrooms |
| 16 | [Graffiti](16-Graffiti.md) | DecalProjector, Freehand painting, Cartel activity |
| 17 | [TimeManager](17-TimeManager.md) | Time system, Sleeping, Curfew, Energy |
| 18 | [Trash/Recycling](18-Trash-Recycling.md) | Trash generation, Recycler, Cleaner |
| 27 | [Tutorial/Prolog](27-Tutorial-Prolog.md) | Tutorial scene, IntroManager cutscene, Starter quests, RV explosion |

### Business & Economy Systems
| # | System | Description |
|---|--------|-------------|
| 19 | [Customer/Budget](19-Customer-Budget.md) | Customer calculation, AdjustedWeeklySpend |
| 20 | [XP/Leveling](20-XP-Leveling.md) | XP sources, Ranks, Level table |
| 21 | [Casino/Gambling](21-Casino-Gambling.md) | Slot 131.9% RTP, Blackjack, Save-scumming |
| 22 | [DevConsole](22-DevConsole.md) | Console commands, Activation |
| 23 | [Economy/Money](23-Economy-Money.md) | Money types, Income sources, Exploits |

### Avatar, Items & Audio
| # | System | Description |
|---|--------|-------------|
| 24 | [Avatar/CharacterCreator](24-Avatar-CharacterCreator.md) | Character Creator, 10 clothing slots, Hairdresser/Tattoo |
| 25 | [Mixing/Production](25-Mixing-Production.md) | MixMap, Effects, Stations, Packaging |
| 28 | [Audio](28-Audio.md) | AudioManager, MusicManager, SFXManager, Zones |
| 31 | [Clothing](31-Clothing.md) | ClothingDefinition, Slots, Colors, Shops |
| 37 | [Drug Effects](37-DrugEffects.md) | All 35+ effects, EffectController, MixMaps |
| 53 | [Product System](53-ProductSystem.md) | Drug definitions, Visuals, Properties |

### Combat, Crime & Law Enforcement
| # | System | Description |
|---|--------|-------------|
| 29 | [Combat & Weapons](29-Combat-Weapons.md) | CombatManager, Melee/Ranged combat, Explosions |
| 30 | [Cartel](30-Cartel.md) | Cartel activities, Goons, Influence, Ambush |
| 51 | [Police](51-Police.md) | PoliceOfficer, Investigations, Checkpoints |
| 62 | [Vision System](62-Vision.md) | Vision cone, EntityVisibility, Stealth |

### Player & Movement
| # | System | Description |
|---|--------|-------------|
| 49 | [Player](49-Player.md) | PlayerController, Movement, Camera, Health |
| 50 | [Player Tasks](50-PlayerTasks.md) | Task system, Minigames, Interactions |
| 55 | [Skateboard](55-Skateboard.md) | Skateboard mechanics, Tricks, Camera |

### Properties & Buildings
| # | System | Description |
|---|--------|-------------|
| 42 | [Management UI](42-ManagementUI.md) | Clipboard, Entity configuration, Transit routes |
| 47 | [Object Stations](47-ObjectStations.md) | All stations (Mixing, Chem, Press, etc.) |
| 54 | [Property](54-Property.md) | Property types, Business, Rent/Purchase |
| 59 | [Tiles & Grid](59-TilesGrid.md) | Grid system, Tile types, Coordinates |

### Storage & Items
| # | System | Description |
|---|--------|-------------|
| 39 | [Equipping](39-Equipping.md) | Equippable framework, Weapons, Viewmodel |
| 48 | [Packaging](48-Packaging.md) | Packaging types, Packaging Station |
| 56 | [Station Framework](56-StationFramework.md) | StationItem, Recipes, LiquidContainer |
| 57 | [Storage](57-Storage.md) | StorageEntity, Grid, Store/retrieve items |

### NPCs & Social
| # | System | Description |
|---|--------|-------------|
| 38 | [Employees](38-Employees.md) | Employee types, Configuration, Behavior |
| 44 | [Messaging](44-Messaging.md) | SMS system, Conversations, Responses |
| 63 | [VoiceOver](63-VoiceOver.md) | VODatabase, VO lines, Police Chatter |

### World & Environment
| # | System | Description |
|---|--------|-------------|
| 33 | [Cutscenes](33-Cutscenes.md) | CutsceneManager, Intro, End cutscene |
| 43 | [Map & Locations](43-MapAndLocations.md) | Map, POIs, Regions, Access control |
| 58 | [Temperature](58-Temperature.md) | Temperature system, AirConditioner, Plants |
| 64 | [Weather](64-Weather.md) | EnvironmentManager, Day/Night, Weather profiles |

### Full Inventory (all 64 systems)
The table above highlights a curated subset. The complete one-line-per-system index for quick lookup:

| # | File |
|---|------|
| 01 | [01-FishNet-Networking](01-FishNet-Networking.md) |
| 02 | [02-Save-Persistence](02-Save-Persistence.md) |
| 03 | [03-Dialogue-Quest](03-Dialogue-Quest.md) |
| 04 | [04-DarkMarket](04-DarkMarket.md) |
| 05 | [05-Achievement-Steam](05-Achievement-Steam.md) |
| 06 | [06-UI-Phone-HUD](06-UI-Phone-HUD.md) |
| 07 | [07-Vehicle](07-Vehicle.md) |
| 08 | [08-Plant-Growing](08-Plant-Growing.md) |
| 09 | [09-Inventory-ItemFramework](09-Inventory-ItemFramework.md) |
| 10 | [10-Building-Construction](10-Building-Construction.md) |
| 11 | [11-Business-Laundering](11-Business-Laundering.md) |
| 12 | [12-Heat-Pursuit-Law](12-Heat-Pursuit-Law.md) |
| 13 | [13-Regions](13-Regions.md) |
| 14 | [14-NPC-Behaviour](14-NPC-Behaviour.md) |
| 15 | [15-Sewer](15-Sewer.md) |
| 16 | [16-Graffiti](16-Graffiti.md) |
| 17 | [17-TimeManager](17-TimeManager.md) |
| 18 | [18-Trash-Recycling](18-Trash-Recycling.md) |
| 19 | [19-Customer-Budget](19-Customer-Budget.md) |
| 20 | [20-XP-Leveling](20-XP-Leveling.md) |
| 21 | [21-Casino-Gambling](21-Casino-Gambling.md) |
| 22 | [22-DevConsole](22-DevConsole.md) |
| 23 | [23-Economy-Money](23-Economy-Money.md) |
| 24 | [24-Avatar-CharacterCreator](24-Avatar-CharacterCreator.md) |
| 25 | [25-Mixing-Production](25-Mixing-Production.md) |
| 26 | [26-PlayerCamera-SelfDamageBug](26-PlayerCamera-SelfDamageBug.md) |
| 27 | [27-Tutorial-Prolog](27-Tutorial-Prolog.md) |
| 28 | [28-Audio](28-Audio.md) |
| 29 | [29-Combat-Weapons](29-Combat-Weapons.md) |
| 30 | [30-Cartel](30-Cartel.md) |
| 31 | [31-Clothing](31-Clothing.md) |
| 32 | [32-Configuration](32-Configuration.md) |
| 33 | [33-Cutscenes](33-Cutscenes.md) |
| 34 | [34-Delivery](34-Delivery.md) |
| 35 | [35-Doors](35-Doors.md) |
| 36 | [36-Dragging](36-Dragging.md) |
| 37 | [37-DrugEffects](37-DrugEffects.md) |
| 38 | [38-Employees](38-Employees.md) |
| 39 | [39-Equipping](39-Equipping.md) |
| 40 | [40-Interaction](40-Interaction.md) |
| 41 | [41-Lighting](41-Lighting.md) |
| 42 | [42-ManagementUI](42-ManagementUI.md) |
| 43 | [43-MapAndLocations](43-MapAndLocations.md) |
| 44 | [44-Messaging](44-Messaging.md) |
| 45 | [45-Multiplayer](45-Multiplayer.md) |
| 46 | [46-Noise](46-Noise.md) |
| 47 | [47-ObjectStations](47-ObjectStations.md) |
| 48 | [48-Packaging](48-Packaging.md) |
| 49 | [49-Player](49-Player.md) |
| 50 | [50-PlayerTasks](50-PlayerTasks.md) |
| 51 | [51-Police](51-Police.md) |
| 52 | [52-Polling](52-Polling.md) |
| 53 | [53-ProductSystem](53-ProductSystem.md) |
| 54 | [54-Property](54-Property.md) |
| 55 | [55-Skateboard](55-Skateboard.md) |
| 56 | [56-StationFramework](56-StationFramework.md) |
| 57 | [57-Storage](57-Storage.md) |
| 58 | [58-Temperature](58-Temperature.md) |
| 59 | [59-TilesGrid](59-TilesGrid.md) |
| 60 | [60-TVSystem](60-TVSystem.md) |
| 61 | [61-Variables](61-Variables.md) |
| 62 | [62-Vision](62-Vision.md) |
| 63 | [63-VoiceOver](63-VoiceOver.md) |
| 64 | [64-Weather](64-Weather.md) |

### Interaction & Physics
| # | System | Description |
|---|--------|-------------|
| 35 | [Doors](35-Doors.md) | All door types, Access control |
| 36 | [Dragging](36-Dragging.md) | DragManager, Draggable objects |
| 40 | [Interaction](40-Interaction.md) | InteractionManager, InteractableObject |
| 46 | [Noise](46-Noise.md) | Noise events, Listener, Stealth |

### Services & Infrastructure
| # | System | Description |
|---|--------|-------------|
| 32 | [Configuration](32-Configuration.md) | ConfigurationService, Settings |
| 34 | [Delivery](34-Delivery.md) | DeliveryManager, Delivery vehicles |
| 41 | [Lighting](41-Lighting.md) | Light types, Grow Lights, Timers |
| 45 | [Multiplayer](45-Multiplayer.md) | Lobby, Splitscreen, Transport |
| 52 | [Polling](52-Polling.md) | Community polls |
| 60 | [TV System](60-TVSystem.md) | TV apps, Pong, Snake, Runner |
| 61 | [Variables](61-Variables.md) | VariableDatabase, Conditions, Quest flags |

### Bugs & Exploits
| # | System | Description |
|---|--------|-------------|
| 26 | [SelfDamage Bug](26-PlayerCamera-SelfDamageBug.md) | Baseball bat + V key = Self-damage/Suicide |

## Source

Analyses based on ~1855 C# files from the ScheduleOne namespace.

## Code Tree

`Analysis/Reference/ScheduleOne_CodeTree.md` — full index of all 1855 C# files.

## Workflow Guidelines for Agents

- Locate target system using the table above and read the respective MD file
- Locate core classes mentioned there via CodeTree
- For Interop: Observe `Analysis/IL2CPP/IL2CPP-Interop-Rules.md`
