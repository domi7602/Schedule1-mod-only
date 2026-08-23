# Delivery (Schedule I)

## Core Classes

| Class | Purpose |
|-------|---------|
| `DeliveryManager` | Global delivery manager (NetworkSingleton) |
| `DeliveryVehicle` | Delivery vehicle |
| `DeliveryInstance` | Individual delivery |
| `DeliveryReceipt` | Delivery receipt |
| `LoadingDock` | Loading dock station |

## Deliveries

- `DeliveryConfiguration`: Delivery configuration
- `EDeliveryStatus`: Status tracking (Pending, InTransit, Delivered)
- Deliveries are ordered via the Phone app
- `DeliveryApp`: Delivery UI on the phone

## Delivery Vehicles

- `DeliveryVehicle`: Dedicated vehicle type
- Drives automatically to the loading dock
- Controlled via AI

## UI

- `DeliveryApp`: Order UI
- `DeliveryStatusDisplay`: Delivery status
- `DeliveryReceiptDisplay`: Receipt display
- `ListingEntry`: Order listing

## Delivery Locations

- `LoadingDock`: Loading dock station at the property
- Each property has its own loading dock
- Deliveries appear the next morning
