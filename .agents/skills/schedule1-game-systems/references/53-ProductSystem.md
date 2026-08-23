# Product System (Schedule I)

## Core Classes

| Class | Purpose |
|-------|---------|
| `ProductManager` | Global product manager (NetworkSingleton) |
| `ProductDefinition` | Product definition |
| `ProductItemInstance` | Product item instance |
| `EDrugType` | Drug types |

## Drug Types

| Type | Definition | Visual Settings |
|------|------------|-----------------|
| Marijuana | `WeedDefinition` | `WeedAppearanceSettings` |
| Meth | `MethDefinition` | `MethAppearanceSettings` |
| Cocaine | `CocaineDefinition` | `CocaineAppearanceSettings` |
| Shrooms | `ShroomDefinition` | `ShroomAppearanceSettings` |
| MDMA | (via Mixing) | - |
| Heroin | (via Mixing) | - |
| Liquid Meth | `LiquidMethDefinition` | `LiquidMethVisuals` |

## Product Properties

- `EProperty`: Product properties
- `PropertyContainer`: Container for properties
- `PropertyMethods`: Utility methods
- `PropertyUtility`: Property utilities
- `ProductQuantities`: Quantity management

## Product Manager

- `ProductList`: Product list (by DrugType)
- `ProductEntry`: Product entry
- `ProductIconManager`: Icon management
- `FunctionalProduct`: Physical product

## Mix Recipes

- `MixRecipeData`: Recipe data
- `NewMixOperation`: New mixing operation
- `NewMixDiscoveryBox`: Discovery box
- `DrugTypeContainer`: Type container

## Visual System

- `ProductVisualsSetter`: Base visuals setter
- `WeedVisualsSetter` / `MethVisualsSetter` / `CocaineVisualsSetter` / `ShroomVisualsSetter`
- `MultiTypeVisualsSetter`: Multi-type support
- Visual appearance based on quality and properties
