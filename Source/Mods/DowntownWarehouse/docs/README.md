# DowntownWarehouse

Purchasable singleplayer warehouse behind Handy Hank's Hardware: interior shell, working
door, save point, build grid and placement persistence. The property is registered through
the S1API property path; the interior is a locally built shell with collision, door and
build-area handling.

## Status 0.1.0

Implemented and building. Only local build and unit-test results are recorded:

- `dotnet build Source/Mods/DowntownWarehouse/src/DowntownWarehouse.csproj -c Release -p:S1NoDeploy=true`
- `Source/Tests/DowntownWarehouse.Tests/`

Not owner-verified in-game: the Ray purchase menu on a save with unlocked real-estate
purchase, `E` door handling, save/reload persistence of placed objects, and the build grid
inside the hall.

## Controls

- Door: `E` (vanilla door interaction).
- Build mode: the vanilla property build mode.

## Limits

Singleplayer only; multiplayer is not supported. The warehouse geometry is local to this
mod and uses the extracted [`main-cut.json`](../assets/main-cut.json) layout. There is no release packaging.

See [CHANGELOG.md](CHANGELOG.md) for the current version.
