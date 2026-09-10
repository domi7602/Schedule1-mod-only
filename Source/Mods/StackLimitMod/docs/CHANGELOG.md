# Changelog

## 0.1.1 (2026-09-10)

- Decision-Cache (IntPtr-Key) im get_StackLimit-Hot-Path (0 Allokationen).
- OverrideNonStackable=false stellt Original-Limits wieder her.

## 0.1.0 (2026-08-20)

- **Initial release**: StackLimitMod for *Schedule I* (v0.4.6f13).
- **Engine**: Implemented `StackLimitEngine` scanning both `Resources.FindObjectsOfTypeAll<BaseItemDefinition>()` and `Registry.GetAllItems()`.
- **Runtime Hook**: Harmony postfix on `Registry.AddToRegistry` to dynamically apply stack limit rules to runtime-registered items.
- **Instance Patch**: Harmony postfix on `BaseItemInstance.get_StackLimit` with fallback to original limits and exclusion support.
- **Console & Terminal Integration**: Added `stack` and `stacklimit` commands supporting `stats`, `set <amount>`, `reload`, and `help`. Registered with DooDesch's `hash` terminal bridge.
- **Resilience**: Integrated `PatchGuard` and `SafeStorage` atomic persistence (`UserData/StackLimitMod/config.json`).
