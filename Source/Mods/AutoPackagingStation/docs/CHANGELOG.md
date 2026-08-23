# Changelog - AutoPackagingStation

## [0.1.0] - 2026-08-22
### Initial Release
- Implemented 4x4 industrial Auto-Packaging Station with procedural 3D chassis, overhead arch, and dual pneumatic pistons.
- Added animated UV-scrolling conveyor belt with configurable speed.
- Integrated multi-state status LEDs (Green/Orange/Blue/Red) with dynamic pulsing.
- Built atomic 2-phase packaging engine with +5% quality freshness bonus and 1:1 mix-effect replication.
- Added procedural sound synthesis for pneumatic hiss, compressor pump, and mechanical stamp impacts.
- Implemented slot-isolated atomic persistence (`autopack_slot_{slotId}.json`) via `SafeStorage.SaveAtomic` synchronized exclusively with `GameLifecycle.OnSaveComplete`.
- Integrated with S1API `BuildableItemCreator` and Handy Hank's hardware store listing injection.
