# Delivery (Schedule I)

> Evidence: inherited notes from the supplied Skills.zip. The reported 2026-10-08 static scan was not rerun for this package. Treat symbols, values and behavior as lookup candidates until checked against the target build; [scope and workflow](README.md).
> UNVERIFIED against the installed runtime. Static identifier sweep 2026-10-08 against the freshly regenerated f12 decompile (`GameReferences/decompiled/Assembly-CSharp`), replacing the earlier decompile-generation check: 101/102 identifier-shaped tokens resolve (0 documented as absent). Unresolved identifiers are listed at the end of this file. Runtime behaviour is not covered by this sweep.

## Core Classes (`ScheduleOne.Delivery`)

| Class | Verified base type | Purpose |
|-------|--------------------|---------|
| `DeliveryManager` | `NetworkSingleton<DeliveryManager>` | Order tracking, delivery state machine, save participation |
| `DeliveryInstance` | plain object | One order: `DeliveryID`, `StoreName`, `DestinationCode`, `LoadingDockIndex`, `Items` (`StringIntPair` list), `Status`, `TimeUntilArrival`, `ActiveVehicle` |
| `DeliveryReceipt` | plain object | Receipt snapshot: same fields as instance minus status/time |
| `DeliveryVehicle` | `MonoBehaviour` (NOT a `LandVehicle` itself — holds `_Vehicle` backing + `GUID`) | `Activate(DeliveryInstance)`, `Deactivate()` |
| `DeliveryConfiguration` | `Configuration<DeliverySettings>` | Delivery tuning config |
| `LoadingDock` | `MonoBehaviour` | Physical dock per property: `InputSlots`/`OutputSlots`, `SetOccupant(LandVehicle)`, `SetStaticOccupant(LandVehicle)`, `RefreshOccupant()`, `ShowOutline(Color)`, `HideOutline()`, `ParentProperty`, `VehicleDetector`, `Parking`, `IsAcceptingItems`, `IsDestroyed`, `GUID`/`BakedGUID` |

`EDeliveryStatus` (previously reported values): `InTransit, Waiting, Arrived, Completed` — **not** "Pending/InTransit/Delivered" as previously listed.

## DeliveryManager API (previously reported)
- State: `Deliveries` (static list), `deliveryCache`, `_deliveryHistory` / `_displayedDeliveryHistory`, `_minsSinceVehicleEmpty`
- Queries: `GetDelivery(string deliveryID)`, `GetDelivery(Property destination)`, `GetActiveShopDelivery(DeliveryShop shop)`, `IsLoadingBayFree(Property destination, int loadingDockIndex)`
- Mutations: `SendDelivery(DeliveryInstance)` (server RPC funnel), `RecordDeliveryReceipt_Server(DeliveryReceipt, string originalOrderID = "")`, `ReceiveDelivery(NetworkConnection, DeliveryInstance)`, `SetDeliveryState(string, EDeliveryStatus)`
- Tick: `OnTimePass(int minutes)` — per-minute update hook (same pattern as other managers; exact subscriber wiring unverified). Progresses `TimeUntilArrival` and delivery completion
- Save: `InitializeSaveable()` (registers with the save system) + `Load(DeliveriesData data)` — deliveries **do** participate in the save file

## Events
| Event | Type | Raised by |
|-------|------|-----------|
| `DeliveryManager.onDeliveryCreated` | `Action<DeliveryInstance>` (add/remove verified) | order creation path |
| `DeliveryManager.onDeliveryCompleted` | `Action<DeliveryInstance>` (add/remove verified) | completion path (also mirrored per-instance as `DeliveryInstance.onDeliveryCompleted` field) |

## Phone UI (`ScheduleOne.UI.Phone.Delivery`)
- `DeliveryApp` — the phone app: `CanReorder(DeliveryReceipt, out string reason)`, `SetIsAvailable(ShopInterface, bool)`, `OnTabChange(int)`
- `DeliveryShop` — shop order screen: `SubmitOrder(string originalDeliveryID)`, `GetDeliveryTime(int itemCount)`, `CanOrder(out string reason)`, `CanReorder(...)`, `DestinationDropdownSelected(int)`, `LoadingDockDropdownSelected(int)`; fields `DeliveryFeeLabel`, `ItemTotalLabel`, `OrderTotalLabel`, `DeliveryTimeLabel`, `OrderButton`
- `ListingEntry`, `DeliveryStatusDisplay`, `DeliveryReceiptDisplay` — list/status/receipt widgets
- `ShopInterface` (matching shop interface) ties each delivery shop to an in-game store

## Flow
1. Player orders in `DeliveryApp`/`DeliveryShop` → `SubmitOrder` → server `SendDelivery`
2. `DeliveryInstance` created with `TimeUntilArrival`, vehicle activated (`DeliveryVehicle.Activate`)
3. `OnTimePass` counts down; status `InTransit → Arrived` (next morning / delivery time), player fills dock inputs
4. `RecordDeliveryReceipt_Server` → `Completed`, receipt persisted in `_deliveryHistory`

## Candidate Hook Points (Harmony)
1. `DeliveryManager.OnTimePass(int minutes)` — prefix/postfix for delivery timing control (accelerate/skip/queue custom deliveries); regular per-minute method, safe patch.
2. `DeliveryShop.SubmitOrder(string originalDeliveryID)` — intercept custom orders, or `CanOrder(out string)` prefix to override availability.
3. `LoadingDock.SetOccupant(LandVehicle)` — react to vehicle arrivals at docks.
- S1API: `S1API.Deliveries` exists — `Delivery`, `DeliveryStatus`, `DeliveryRegistry`, `DeliveryReceipt`, `DeliveryItem`; supported locations via `S1API.Map.DeliveryLocation` + dozens of `IDeliveryLocationIdentifier` classes (e.g. `DestroyedRV`, `BrickWarehouseDocks`). Internally S1API hooks delivery flow itself: `Internal/Deliveries/DeliveryEventBridge`, `SupplierDeliveryRecovery`/`SupplierDeliveryUiBridge`, and `Internal/Patches/DeliveryPatches` + `LoadingDockPatches`.

## Not Implemented / Notes
- `DeliveryVehicle` drives itself (vehicle AI on the wrapped `Vehicle` component) — no separate "AI driver NPC" class in this namespace.
- `DeliveryInstance.GetTimeStatus()` returns `int` (time-related status; exact unit semantics unverified); `OnTimePass(int minutes)` drives status progression; `AddItemsToDeliveryVehicle()` moves order items into the vehicle; `GetReceipt()` builds the `DeliveryReceipt`

---

## Unresolved identifiers in the imported 2026-10-08 record

The imported scan did not resolve the following tokens within its recorded search scope. These may be placeholders, attributes from another assembly or drift candidates. Search the actual declaring assembly and version before concluding that an API was removed.

- `_Vehicle`
