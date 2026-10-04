# Changelog

## 0.2.0 (2026-10-04)
- Integrated StorageScanner into the repository as a first-class S1API PhoneApp mod.
- Auto-refresh interval raised from 0.5 s to 2.5 s and centralized in `Constants.AutoRefreshIntervalSeconds`; manual refresh remains immediate.
- Removed the duplicate owned-property refresh within one scan cycle (`IsReadyToScan` refreshes once; `Capture` reuses that cache).
- Added per-property last-known storage cache for culled interiors; cached values are explicitly marked in rows, details and scan status.
- Added scan duration, live/cached property counts, distinct item count and container count to the status display and structured scan/cache log lines.
- Added sort modes for name, quantity and container count.
- Search now matches item, property and container names.
- Added item detail navigation with exact property/container quantity breakdown.
- Inventory rows/chips are only rebuilt when their content or selection changes instead of being destroyed and recreated on every periodic scan.
- Added pure core models suitable for unit testing without Schedule I assemblies.

## 0.1.1
- Fixed readiness logic so culled remote properties no longer leave the app stuck on `Loading storage...`.
- Culled property data was reported as incomplete rather than blocking all scans.

## 0.1.0
- Initial StorageScanner prototype: owned-property storage scan, aggregation, property filtering and item-name search.
