# StorageScanner — Phone Storage Overview

**StorageScanner** is a read-only phone app for **Schedule I** (IL2CPP / S1API `PhoneApp` architecture) that sums the contents of every storage container across your owned properties. Stop walking from shelf to shelf to count Baggies — open the app and read the totals.

---

## Features

- **Totals per item** across all containers of your owned properties (e.g. `Baggies — 240 (3 containers)`).
- **Collapsible filters**: open **Filters** to choose an item category and/or owned property. The current selection stays visible when the drawer is closed; **Reset** clears both filters and the search.
- **Live name search** for quickly finding one item.
- **Sort control**: shows the current name, quantity or container-count order; tap to cycle.
- **Item detail page**: tap an item row for its total plus a per-property, per-container breakdown (e.g. `Storage Shelf — 200`).
- **Last-known cache per property**: when a property is not loaded, its last live scan remains available. Affected item rows are labelled **last known stock**.
- **Manual refresh** (**Refresh** button) plus an automatic refresh every 2.5 s while the app is open.
- **Readable status**: availability notices are separate from the neutral footer (shown item types and last scan time). Incomplete stock is never presented as a complete scan; build versions and raw scan diagnostics remain in the log.
- **Read-only by design**: no inventory changes, no Harmony patches, no background scanning, no save data.

---

## Status

- v0.2.0 — **In-Game-Verify open** (2026-10-04): scan efficiency, per-property last-known cache, sorting, item detail page; see [`CHANGELOG.md`](CHANGELOG.md).

## Usage

1. Open the phone and tap the **STORAGE** app icon.
2. Tap **Filters** to expand the drawer. Choose a category, a property, or both; scroll the drawer for additional options. Tap **Filters** again to close it.
3. Type in the search field to filter by item name. Delete the text to clear only search, or use **Reset** in the drawer to clear search and both filters.
4. Tap an item row for its detail page; **Back to stock** returns. The sort control beside **Filters** cycles the sort order.
5. **Refresh** re-scans on demand.
6. See [UI verification checklist](UI-VERIFICATION.md) for the consolidated in-game test round.

## Notes

- Only storage on **owned** properties is counted; containers elsewhere are ignored.
- If containers are far away, the game may not have their contents loaded ("culled"). The app explains unavailable or last-known stock without exposing engine diagnostics. **Last scan** is the latest overall scan attempt that produced a snapshot, not the age of every cached item; cached rows are explicitly marked.
- The scan runs on the main thread only, in short bursts while the app is open.

History: [`CHANGELOG.md`](CHANGELOG.md).
