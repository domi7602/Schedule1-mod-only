# AutoPackagingStation (v0.1.0)

Automated 4x4 industrial packaging station for *Schedule I*.

## Features
- **4x4 Industrial Visuals:** Heavy steel chassis, moving pneumatic compression pistons, and animated UV-scrolling conveyor belt.
- **Multi-State Status LEDs:** Real-time visual feedback (Green = Ready, Orange Pulsing = Packaging, Blue = Waiting for Input, Red = Blocked / Full).
- **2-Second Packaging Cycle:** High-efficiency automated packaging with procedural pneumatic hiss and mechanical compression sound effects.
- **+5% Freshness Quality Bonus:** Packaged goods automatically receive a +5% quality freshness bonus while copying all mix-effects 1:1.
- **TOCTOU Safe 2-Phase Transaction:** Atomic validation and deduction ensuring zero lost items and zero item duplication exploits.
- **Slot-Isolated Atomic Persistence:** Uses `SafeStorage.SaveAtomic` with `autopack_slot_{slotId}.json` strictly synchronized with `GameLifecycle.OnSaveComplete`.
- **Hardware Store Availability:** Purchasable at Handy Hank's Hardware ($3,500).

## Controls
- **[E]**: Quick-load held item / packaging container into station.
- **[R]**: Collect finished packaged goods from output tray.

> **Pick up / dismantle:** Use Schedule I's native right-mouse-button pickup on the placed station. All buffered input/packaging/output is refunded to your inventory automatically.
