# ProductAppQoL

**ProductAppQoL** — quality-of-life fixes for the vanilla Product app on the in-game phone. The first fix: the **asking-price −/+ buttons respond to mouse and touch**.

The mod is **patch-only**: it hooks the existing vanilla `ProductAppDetailPanel` (Harmony postfix on `Awake`) and adds its input helper next to the untouched vanilla Button components. It does **not** register a new PhoneApp or change gameplay values beyond the clicks you make.

## Why

Vanilla wires `Products > AskingPrice/Container/{Add,Remove}` through `ScheduleOne.GamepadInput.InputValueRamp` — the hold-to-accelerate adjustment made for gamepads (tap = one step, hold = the value ramps up/down). With mouse and keyboard those buttons do nothing; the price can only be typed into the field (`PriceSubmitted`).

## Features

- **Tap** the − or + button: price ±$1 per click (`ProductAppDetailPanel.AdjustPrice(float change)` + `UpdatePrice()`).
- **Hold** the button: the price repeats with acceleration (0.4 s delay, then 0.13 s → 0.05 s repeat interval) — the same feel as the gamepad ramp.
- Works for **touch** too (Unity simulates mouse input from touches).
- Gamepad and typed input keep working exactly as before — the mod adds listeners/polling only, no vanilla state is replaced.

## Implementation notes

- `PriceButtonMouseInput` is a `[RegisterTypeInIl2Cpp]` MonoBehaviour polled via `RectTransformUtility.RectangleContainsScreenPoint` + `Input` mouse state. It deliberately does NOT implement `IPointerDownHandler` etc.: Il2CppInterop exposes those EventSystem interfaces as classes (CS1721, multiple base classes) — the workspace's polling pattern (like `MessagesPlusInputFocus`) sidesteps that.
- The world-space phone canvas is handled: the rect check uses the parent canvas's world camera unless the canvas is Screen Space Overlay.
