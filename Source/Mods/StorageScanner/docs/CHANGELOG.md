# Changelog — StorageScanner

All notable changes to the StorageScanner mod will be documented in this file.

## 0.2.0 (2026-10-04) — Scan efficiency, cached data, sorting & item details
- **Scan interval:** automatic refresh while the app is open is now every 2.5 s (`Constants.AutoRefreshInterval`) instead of 0.5 s — about 4 full scans per 10 s instead of ~20. Opening the app and `REFRESH` still scan immediately.
- **Single property refresh per cycle:** `IsReadyToScan()` primes the owned-property cache and `Capture()` reuses it (the previous duplicate refresh is gone).
- **Snapshot statistics:** snapshots now carry `ContainerCount`, `PropertyCount`, `LivePropertyCount` and `CachedPropertyCount`.
- **Last-known cache per property:** when a property's contents are culled, its last live scan is shown with a `cached` marker and its capture time instead of the data disappearing; the status line reports `X/Y properties live, N using cached data`. The cache is cleared on save switch.
- **Sorting:** new `SORT` button cycles name A-Z / Z-A, quantity and container-count orders (`StockSortMode`).
- **Item details:** tapping an item row opens a per-property, per-container breakdown of the total (`TOTAL`, then quantities per container); `< BACK` returns to the list.
- **Fewer UI rebuilds:** item rows, chips and detail locations are only rebuilt when their data actually changed.
- **Structured scan logging:** one `[SCAN]` summary line per pass (properties live/cached, containers, stacks, milliseconds) plus `[CACHE]` lines for culled properties.
- Read-only behaviour unchanged: no inventory changes, no Harmony patches, no background scanning outside the open app.

## 0.1.1 (2026-10-04) — Scan blocked by cull-gate fix
- **Root cause:** `IsReadyToScan()` returned `false` as long as any owned property had `IsContentCulled == true`. The game culls property contents by distance/state, so with more than one owned property — or from anywhere outside a property — the gate blocked every scan; the app showed "Loading storage..." forever and no scan ever ran (zero `[StorageScanner] Scan:` lines in every session).
- **Fix:** the readiness gate now only waits for the owned-property list to be populated. Culled containers no longer block scanning: they are skipped and the snapshot is marked incomplete ("N content culled") as designed.
- Scan log line now reports `world=`/`skipped=` counters for diagnosis.
- Full source tree restored to match the deployed 0.1.0 behaviour.

## 0.1.0 (2026-10-04) — Initial
- Portrait phone app: total item quantities across the storage containers of owned properties, property filter chips, name search, manual + periodic refresh, completeness banner with capture time. Read-only: no inventory changes, no Harmony patches, no background scanning.
