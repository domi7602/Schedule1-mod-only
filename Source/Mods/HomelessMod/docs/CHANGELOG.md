# Changelog

## [0.1.1] - 2026-08-17

### Added
* **Smart Ground Detector Assistant**: Introduced zero-allocation 5-point terrain sampling with auto-elevation, normal slope detection, and smart player collider filtering in `GroundPlacementAssistant`.
* **Dual-Grid Free-Placement Mode**: Holding <kbd>Shift</kbd> during build mode enables 0.05m ultra-fine placement.

### Fixed
* **IL2CPP Type Registration**: Fixed missing `ClassInjector.RegisterTypeInIl2Cpp<OutdoorItemInteractable>()` registration in `Mod.cs`, resolving runtime `ArgumentException` when packing up outdoor items.
* **Savegame-Slot Isolation**: Implemented slot-based save file naming (`street_items_slot_{slotId}.json`) with seamless migration from legacy `street_items.json` to prevent cross-save contamination.
* **OnGUI Garbage Collection**: Cached static `GUIStyle` instances and colors in `SleepingBagItem` and `OutdoorItemInteractable` to eliminate GC allocation spikes during OnGUI rendering.
* **Ghost Rotation Synchronization**: Synchronized ghost rotation explicitly with `__instance._rotation` in `BuildUpdate_Grid` patch for smooth outdoor rotation.
* **Scene Unload & Native Pointer Safety**: Added defensive `Pointer != IntPtr.Zero` checks to `SleepingBagMeshGenerator` to prevent `Il2CppException` after scene transitions.
* **Documentation**: Added comprehensive `README.md` and updated `mod.json`.

## [0.1.0] - 2026-08-17
* Initial release of HomelessMod ("Street Nomad").
