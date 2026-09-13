# Mixing/Production (Schedule I)

## Drug Types (EDrugType)
6 Types (4 of which feature a MixMap): Marijuana, Methamphetamine, Cocaine, MDMA, Shrooms, Heroin

## Effects (35)
5 Tiers with 7 effects each: e.g., Balding, Toxic, Lethal, Seizure, LongFaced, Zombifying

## Mixing Ingredients (PropertyItemDefinition)
16 Items: Chili, Donut, Gasoline, HorseSemen, Viagor, Paracetamol, Iodine, Batteries, FluMedicine, EnergyDrink, MotorOil, etc.

## MixMap (2D Grid)
- Combines drugs + effects via a grid raster system
- Max 8 effects per product
- Theoretically: ~129 million combinations (35 effects, up to 8 slots)
- Practically: ~62 million per drug due to MixMap grid constraints
- Meaningfully distinct mixtures: lower 4-digit range

## Production Stations
| Station | Function |
|---------|----------|
| MixingStation | Mix drugs + ingredients |
| ChemistryStation | Chemical processes (Meth/Coke) |
| LabOven | Heating / baking |
| DryingRack | Drying (Weed/Shrooms) |
| PackagingStation | Package products |
| BrickPress | Pressing (e.g. into brick form) |
| Cauldron | Cooking (for special recipes) |

## Recipe System
- **NO predefined recipes** – recipes are discovered during mixing
- New discovery reward: **80 XP** (one-time)
- `MixRecipeData` saves discovered recipes
- `ProductManager` manages `mixRecipes`

## Packaging
- `PackagingDefinition`: Quantity, StealthLevel (EStealthLevel)
- `Equippable_Filled` + `StoredItem_Filled` depending on packaging type
- Increases stealth (reduces police risk during transit)
