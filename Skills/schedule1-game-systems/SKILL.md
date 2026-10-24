---
name: schedule1-game-systems
description: "Find Schedule I game systems, classes and hook candidates. Keywords: game systems, FishNet, Save, Dialogue, Quest, Growing, Inventory, Building, Laundering, Heat, Pursuit, Regions, NPC, TimeManager, Economy, Avatar, Mixing, DrugEffects, Stations, Property, Skateboard, Tiles, Weather, hook points, Harmony."
---

> Runtime and dependency details are maintained in workspace [AGENTS.md](../../AGENTS.md). Verification notes in this skill describe evidence scope; they do not imply current-runtime verification.

# Schedule I game-system map

## When to use

Use for locating a game subsystem, understanding inherited research, finding likely native owners, or planning a hook. A focused API call belongs in `schedule1-s1api`; implementing persistence, payments, phone UI or geometry belongs in its specialist skill. Do not load all 64 references for one question.

## Prerequisites and evidence

The [reference scope](references/README.md) explains the imported 2026-10-08 scan. It was not reproduced here. Names, constants, formulas and hook points are lookup candidates until checked against the actual target. A signature scan is not a behavior test. Before repository mutations, read the shared [working contract](../schedule1-modding/references/working-contract.md) once.

## Workflow

1. Identify the user's subsystem and needed answer: class lookup, contract, behavioral explanation or implementation.
2. Open the closest topic below. Use the [detailed index](references/_index.md) for redirects and coverage; do not infer confidence from its depth rating.
3. Locate the actual repository, game/backend and dependency version. Search the named class in the resolved decompile/source and declaring assembly. Paths recorded in notes are candidates, not guaranteed local locations.
4. Resolve the exact owner, signature, accessibility and units. Check a supported wrapper before adding a native patch; wrappers still require version-specific verification.
5. For behavior or a hook, inspect readable implementation or design a bounded runtime check. Interop stubs do not reveal a native method body. Check host/client authority and pre-/post-mutation timing.
6. Return the relevant file and observed symbol evidence, then the smallest supported conclusion. If evidence is missing, state the specific missing artifact and continue with a concrete investigation plan.

## Direct topic navigation

| Topic | Reference files |
|---|---|
| Gameplay | [08-Plant-Growing](references/08-Plant-Growing.md) · [25-Mixing-Production](references/25-Mixing-Production.md) · [37-DrugEffects](references/37-DrugEffects.md) · [53-ProductSystem](references/53-ProductSystem.md) |
| Economy | [11-Business-Laundering](references/11-Business-Laundering.md) · [19-Customer-Budget](references/19-Customer-Budget.md) · [20-XP-Leveling](references/20-XP-Leveling.md) · [21-Casino-Gambling](references/21-Casino-Gambling.md) · [23-Economy-Money](references/23-Economy-Money.md) |
| World | [10-Building-Construction](references/10-Building-Construction.md) · [13-Regions](references/13-Regions.md) · [15-Sewer](references/15-Sewer.md) · [16-Graffiti](references/16-Graffiti.md) · [35-Doors](references/35-Doors.md) · [43-MapAndLocations](references/43-MapAndLocations.md) · [54-Property](references/54-Property.md) · [59-TilesGrid](references/59-TilesGrid.md) |
| NPCs and quests | [03-Dialogue-Quest](references/03-Dialogue-Quest.md) · [04-DarkMarket](references/04-DarkMarket.md) · [14-NPC-Behaviour](references/14-NPC-Behaviour.md) · [27-Tutorial-Prolog](references/27-Tutorial-Prolog.md) · [30-Cartel](references/30-Cartel.md) · [34-Delivery](references/34-Delivery.md) · [38-Employees](references/38-Employees.md) · [63-VoiceOver](references/63-VoiceOver.md) |
| Law and perception | [12-Heat-Pursuit-Law](references/12-Heat-Pursuit-Law.md) · [46-Noise](references/46-Noise.md) · [51-Police](references/51-Police.md) · [62-Vision](references/62-Vision.md) |
| Player and combat | [07-Vehicle](references/07-Vehicle.md) · [09-Inventory-ItemFramework](references/09-Inventory-ItemFramework.md) · [24-Avatar-CharacterCreator](references/24-Avatar-CharacterCreator.md) · [26-PlayerCamera-SelfDamageBug](references/26-PlayerCamera-SelfDamageBug.md) · [29-Combat-Weapons](references/29-Combat-Weapons.md) · [31-Clothing](references/31-Clothing.md) · [36-Dragging](references/36-Dragging.md) · [39-Equipping](references/39-Equipping.md) · [49-Player](references/49-Player.md) · [50-PlayerTasks](references/50-PlayerTasks.md) · [55-Skateboard](references/55-Skateboard.md) |
| UI and interaction | [06-UI-Phone-HUD](references/06-UI-Phone-HUD.md) · [22-DevConsole](references/22-DevConsole.md) · [40-Interaction](references/40-Interaction.md) · [42-ManagementUI](references/42-ManagementUI.md) · [44-Messaging](references/44-Messaging.md) · [60-TVSystem](references/60-TVSystem.md) |
| Infrastructure | [01-FishNet-Networking](references/01-FishNet-Networking.md) · [02-Save-Persistence](references/02-Save-Persistence.md) · [05-Achievement-Steam](references/05-Achievement-Steam.md) · [17-TimeManager](references/17-TimeManager.md) · [32-Configuration](references/32-Configuration.md) · [45-Multiplayer](references/45-Multiplayer.md) · [52-Polling](references/52-Polling.md) · [61-Variables](references/61-Variables.md) |
| Environment | [18-Trash-Recycling](references/18-Trash-Recycling.md) · [28-Audio](references/28-Audio.md) · [33-Cutscenes](references/33-Cutscenes.md) · [41-Lighting](references/41-Lighting.md) · [58-Temperature](references/58-Temperature.md) · [64-Weather](references/64-Weather.md) |
| Stations and storage | [47-ObjectStations](references/47-ObjectStations.md) · [48-Packaging](references/48-Packaging.md) · [56-StationFramework](references/56-StationFramework.md) · [57-Storage](references/57-Storage.md) |

## Hand off to the implementation owner

| Work | Skill |
|---|---|
| Save identity, corrupt input, restore or migration | [Persistence](../schedule1-persistence/SKILL.md) |
| Debit, delivery, stock, compensation | [Economy](../schedule1-economy/SKILL.md) |
| Supported wrapper signature | [S1API](../schedule1-s1api/SKILL.md) |
| Native compatibility and patch feasibility | [Reversing](../schedule1-mod-reversing/SKILL.md), [Harmony bootstrap](../schedule1-harmony-bootstrap/SKILL.md) |
| UI, focus and app lifetime | [Phone apps](../schedule1-phoneapp/SKILL.md) |
| Building placement and scene additions | [Grid](../schedule1-grid/SKILL.md), [world expansion](../schedule1-world-expansion/SKILL.md) |
| Evidence from a connected game | [MCP](../schedule1-mcp/SKILL.md) |

## Completion check

The answer distinguishes inherited notes, actual metadata/source findings and observed runtime results. It does not treat “not found” as removal without search scope, a candidate hook as a tested patch, or a numeric table as a current balance guarantee. Changes to a named member propagate to related references through the [maintenance workflow](../schedule1-modding/references/skill-maintenance.md).
