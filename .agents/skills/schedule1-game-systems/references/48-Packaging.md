# Packaging (Schedule I)

## Core Classes

| Class | Purpose |
|-------|---------|
| `FunctionalPackaging` | Base packaging |
| `FunctionalBaggie` | Baggie |
| `FunctionalJar` | Jar |
| `PackagingTool` | Packaging tool |

## Packaging Types

- `PackagingDefinition`: Definition in item system
- `EStealthLevel`: Stealth level (Low, Medium, High)
- Different packaging for different products
- Packaging quality influences customer satisfaction

## Stations

- `PackagingStation`: Base packaging station
- `PackagingStationMk2`: Advanced version (faster, more options)
- `FilledPackaging_Equippable`: Filled packaging (Equippable)
- `FilledPackaging_StoredItem`: Filled packaging (Stored)

## Process

1. Produce product (Mixing/Chemistry)
2. Bring to Packaging Station
3. Select packaging type
4. Product gets packaged
5. Ready for sale

## Item Definitions

- `PackagingDefinition`: Defines packaging type
- Products receive packaging as a property
- Different package sizes
