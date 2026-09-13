# Clothing & Fashion (Schedule I)

## Core Classes

| Class | Purpose |
|-------|---------|
| `ClothingDefinition` | Clothing item definition (ScriptableObject) |
| `ClothingInstance` | Clothing item instance (in inventory) |
| `ClothingUtility` | Utility methods for clothing |
| `PlayerClothing` | Player clothing manager component |

## Clothing Slots

- `EClothingSlot`: 10 Slots (Head, Face, Eyes, Torso, Hands, Legs, Feet, Neck, Wrist, Back)
- `EClothingColor`: Color palette (Black, White, Red, Blue, Green, Yellow, Pink, Purple, Orange, Brown, Grey, Camo)
- `EClothingApplicationType`: How clothing is applied (Mesh, Texture, Shader)

## Clothing System

- Clothing items are `ClothingInstance` (subclass of ItemInstance)
- Can be purchased in the `ClothingShopInterface`
- `PlayerClothing` synchronizes visual display across the network
- `ClothingColorExtensions`: Color utility methods

## Shops

- `ClothingShopInterface`: Clothing store UI
- `ClothingShopListing`: Store product listing
- `CartEntry_Clothing`: Cart entry for clothing items

## Character Creator

- `CharacterCreator`: Base character creation system
- Barber: `BarbershopUI`
- Tattoos: `TattooShopUI`
- `CharacterCustomizationUI`: Customization UI interface
