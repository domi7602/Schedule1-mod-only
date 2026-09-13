# Property (Schedule I)

## Core Classes

| Class | Purpose |
|-------|---------|
| `PropertyManager` | Global property manager (NetworkSingleton) |
| `Property` | Base property class |
| `Business` | Business property |
| `BusinessManager` | Business manager |

## Property Types

| Type | Class | Description |
|------|-------|-------------|
| RV | `RV` | Starter RV |
| Motel Room | `MotelRoom` | Motel room |
| Bungalow | `Bungalow` | Bungalow |
| Manor | `Manor` | Large manor |
| Sewer Office | `SewerOffice` | Sewer office |
| Sweatshop | `Sweatshop` | Sweatshop |

## Property Functions

- `PropertyContentsContainer`: Container for contents
- `PropertyDisposalArea`: Disposal area
- `Tap`: Water tap
- Each property has its own loading dock

## Business

- `Business`: Business property (front business)
- `BusinessManager`: Business management
- `LaunderingOperation`: Money laundering operation
- Generate income through legal businesses

## Purchase & Rent

- Properties are purchased or rented
- `PropertyDropdown`: Property selection UI
- `PropertySelector`: Property selector
- Different prices depending on type/location

## Integration

- Each property has a building grid for placement
- Employee management per property
- Power/Water supply
- Security (Alarm, Doors)
