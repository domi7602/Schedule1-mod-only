# Changelog

All notable changes to `ProductAppQoL` are documented here. Format: [Keep a Changelog](https://keepachangelog.com/en/1.1.0/), versioning: [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## 0.1.0 (2026-10-10)
- **Mouse/touch support for the asking-price −/+ buttons.** Vanilla wires `Products > AskingPrice/Container/{Add,Remove}` through `ScheduleOne.GamepadInput.InputValueRamp` (hold-to-accelerate gamepad adjustment), so plain mouse clicks do nothing and mouse users can only type the price (`PriceSubmitted`). `PriceButtonMouseInput` is added alongside the untouched vanilla Button and handles pointer input: tap = ±$1 (`ProductAppDetailPanel.AdjustPrice(float change)` + `UpdatePrice()`), hold = repeat that accelerates from 0.13 s to 0.05 s after a 0.4 s delay — mirroring the gamepad ramp feel. Gamepad and typed input stay unchanged (listeners only, no vanilla state replaced).
