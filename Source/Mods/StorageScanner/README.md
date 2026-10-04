# StorageScanner scaffold

Status: core scaffold only. Not a playable mod, registered phone app, or verified build.
No existing mod, solution, or build configuration is changed.

## Intended gameplay

Portrait phone app showing total item quantities across all shelves of a selected property.
Example: Sweatshop has 240 Baggies across 3 shelves. All properties can be selected too.
The numbers above are illustrative, not game data.

## Included

- Immutable storage snapshot with capture timestamp and completeness status.
- Aggregation by stable item ID and optional property ID.
- Search by display name, total quantity, and count of containers containing the item.
- Controller for open, manual refresh, property selection, search, and save reset.
- Interfaces for the game scanner and portrait phone view.
- No automatic purchasing, inventory changes, Harmony patches, or background scanning.

## Integration still required

1. Read PotScanner's actual project file, mod entry point, phone registration, and UI code.
2. Add a project using the repository's verified framework, references, and build conventions.
3. Implement IStorageSource using verified game APIs. Count each physical stack exactly once.
4. Resolve shelves to properties using verified IDs. Do not infer ownership solely from names.
5. Mark snapshots incomplete when unloaded or inaccessible shelves prevent a complete scan.
6. Implement IStorageScannerView as a portrait phone app with property selector, search,
   item rows, refresh button, timestamp, and visible incomplete-data warning.
7. Connect scene/save lifecycle; reset controller and clear the rendered view on save changes.
8. Add solution/CI integration and tests before describing this scaffold as buildable or playable.

## Required tests

- Baggies quantities 80, 120, 40 in three Sweatshop shelves => 240 and 3 containers.
- Another property containing 110 => all-property total 350.
- Property filter excludes the other property's 110.
- Different item IDs with identical display names remain separate.
- Multiple stacks in the same shelf do not inflate container count.
- An incomplete scan is visibly labelled and never presented as all shelves scanned.
- A failed scan reports an error instead of pretending the inventory is empty.
- A save change clears previous snapshot and view.

## Technical limitation during preparation

The GitHub connector returned file-download acknowledgements rather than source bodies
for requested existing files. Therefore game APIs, project references, and phone orientation
were not guessed. These source files intentionally contain no unverified game integration.
