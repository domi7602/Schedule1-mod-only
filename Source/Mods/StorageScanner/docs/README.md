# StorageScanner

**StorageScanner** is a read-only phone app for *Schedule I* that summarizes items stored across player-owned properties.

## Features

- Scans world storage that belongs to owned properties.
- Aggregates total quantity and distinct container count per item.
- Property filter chips and text search across item, property and container names.
- Sort modes for name, quantity and container count.
- Tap an item to see the exact property/container breakdown.
- Uses a 2.5-second automatic refresh while the app is open, plus manual refresh.
- Keeps a last-known snapshot for properties whose content is currently culled by the game and marks those values as cached.
- Shows live/cached property count, container count, distinct item count and scan duration.

## Cached data

Schedule I can cull remote property interiors. When a property is culled, StorageScanner cannot read a fresh live container state. If that property was scanned previously, v0.2.0 keeps the last-known result and clearly marks it as cached. Cached values are informational and may be stale until the property becomes live again.

## Controls

- **Property chips:** filter inventory to one property or all properties.
- **Search:** searches item, property and container names.
- **SORT:** cycles A-Z, Z-A, quantity descending/ascending and container-count descending/ascending.
- **REFRESH:** forces an immediate scan.
- **Item row:** opens the location breakdown.
- **Escape:** closes item details first, then closes the app.

## Architecture

The implementation separates game access from pure logic:

- `GameStorageSource.cs` reads S1API / Schedule I storage state and maintains the per-property last-known cache.
- `StorageScannerCore.cs` contains pure models, aggregation, sorting, cache projection and item detail projection.
- `StorageScannerController.cs` owns UI state (property filter, search, sort and selected item).
- `StorageScannerApp.cs` is the S1API PhoneApp view.

Pure logic is covered by `Source/Tests/StorageScanner.Tests` and does not require game assemblies.

## Status

v0.2.0 is code-complete but still requires an in-game verification pass on the current Schedule I / S1API beta stack before being marked verified.
