# Archived TownHallInterior development sources

These inputs are preserved for history and targeted debugging. They are outside the active mod
and the solution, and they are never compiled by `Tools/gen-sln.ps1` or `Tools/test-all.ps1`.

- `DoorDialog/src/`: the staged native door-choice subsystem, moved out of
  `Source/Mods/TownHallInterior/src/` on 2026-10-29. It has no production instantiation:
  `DoorHook` hard-wires `UnavailableDoorSelector` and always passes the native interaction, so
  the selector, the modal host and the context-menu runtime were game-free code that could not
  affect the game.
- `DoorDialog/tests/`: the matching unit tests. They need `DoorChoice.cs` and `DoorAdapter.cs`
  from the active mod plus the archived `DoorDialog/src/` files to compile; they are preserved as
  source only and are not part of any test run.

Current source: `Source/Mods/TownHallInterior/`. Current verification record:
`Source/Mods/TownHallInterior/docs/enclosure-verification.md`.

## Why it was archived

The subsystem depends on an `ExclusiveModalFocus` lease that requires a native observation layer
on the screen stack, cursor, movement and typing mutations. That layer does not exist, so
exclusivity could not be proven in game; the staged code could only maintain a lease given
honest reports. Keeping it inside the active build cost roughly 550 production lines and 54 unit
tests that proved pure policy, not gameplay, while the accepted deliverable is the geometric
public hall.

## Restoring it

Move `DoorDialog/src/*.cs` back into `Source/Mods/TownHallInterior/src/`, re-add the matching
`<Compile Include=...>` entries to `Source/Tests/TownHallInterior.Tests/TownHallInterior.Tests.csproj`
and move `DoorDialog/tests/*.cs` back into that test project. Restoring the files alone changes
nothing at runtime: a real door choice additionally requires the native observation layer for
`ExclusiveModalFocus`, a wired `DoorSelector` instead of `UnavailableDoorSelector`, and the
`NativeDoorModalHost`/`NativeContextMenuRuntime` lifetime rules documented in
`Source/Mods/TownHallInterior/docs/README.md`.
