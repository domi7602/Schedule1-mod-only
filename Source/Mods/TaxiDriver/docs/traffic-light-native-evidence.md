# Taxi-only native IgnoreTrafficLights evidence

Initially implemented compile-only. Parent reviewed the code, reran native instruction assertions and 324 tests, then built/deployed the version recorded in mod.json on 2026-10-13 (0 warnings/errors; installed DLL/PDB/metadata hash-matched). DLL SHA-256 `54598bbbfc3b5c4632fb6323927512653a704993406b3e555c9088e7aa3c36b5`. No commit or game launch was performed. Runtime hook execution and actual signal crossing remain unverified.

## Installed binary identity

Read-only local inspection of installed f12:

- GameAssembly.dll SHA-256: `61829050e56ca180d67be592e4bfe61ec97d2e9573f720d90a54e318a7e4381d`
- global-metadata.dat SHA-256: `e775c97da277da8eb1a90265478f3b65c9c3d6464aff4f7255d45280a1d1181f`
- Installed Cpp2IL Assembly-CSharp.dll SHA-256: `bf737dd7f3a5700527d42bee8d29ac4e7b7feecdea6a3bd01781a44ea93b23c4`
- Installed interop Assembly-CSharp.dll SHA-256: `38f45597a972887abb9219315bce41ac7666854096d76372accac4f9267f8771`

The generated interop files establish signatures only. Native instruction assertions
were executed against GameAssembly.dll with pefile/capstone; no native assets are
included in this repository.

## Proven consumer and excluded hit

Cpp2IL tokens/attributes establish:

- VehicleAgent.Flags at instance offset `0x30`.
- DriveFlags.IgnoreTrafficLights at object offset `0x20`.
- VehicleObstacle.type at object offset `0x2C`; Generic=0, TrafficLight=1.
- VehicleAgent.GetWheelSweepCollision, token `0x6000F57`, RVA `0xAD42D0`, length `0xFAB`.

Installed instructions:

- `0xAD4C03`: reads agent+0x30 (Flags).
- `0xAD4C10`: compares Flags+0x20 (IgnoreTrafficLights) with zero.
- `0xAD4C14`: when false, continues normal hit filtering at `0xAD4C5B`.
- `0xAD4C51`: when true and an obstacle component exists, compares type+0x2C with 1.
- `0xAD4C55`: only type=1 branches to `0xAD4EFA`.
- `0xAD4EFA`/`0xAD4EFC`: increment hit index and return to the hit loop; do not
  globally return no-collision. Generic hits keep their existing native filtering.

GetWheelSweepCollision is called twice by UpdateSweep (`0xAD95E6`, `0xAD9629`),
and twice by the wheel-sweep branch of UpdateSpeedReduction (`0xAD806A`, `0xAD80AC`).
InfrequentUpdate runs base speed, steering, UpdateSweep, then UpdateSpeedReduction
(`0xAD5587`, `0xAD5592`, `0xAD559C`, `0xAD55A6`). Turn (`Flags+0x29`, `0xAD8296`)
and destination (`Flags+0x28`, `0xAD83BA`) decisions and final reduced target write
(`0xAD85F9`) remain separate and are not patched.

## Choice and limits

The hook scopes only the proven synchronous GetWheelSweepCollision consumer:
physically new DriveFlags, every other installed field copied verbatim, only
IgnoreTrafficLights=true, exact agent/vehicle ownership, host, active order,
AutoDriving and no player driver. Postfix restores the exact original pointer;
finalizer retries a failed restoration without re-evaluating control/ownership.
Partial install errors restore immediately and retain scope state for finalizer retry.
No persistent agent/flags cache means native patrol resets and redispatch replacements
are read afresh at each call; teardown unpatches the complete hook.

**Important native branch limit:** UpdateSpeedReduction also has a separate
GetForwardObstacle raycast branch (`0xAD3960`). Its compiler-generated IsValidHit
helper (`0xAD6EF0`, length `0x5E`, token `0x6000F63`) was inspected and only rejects
self-child hits; it does not consume IgnoreTrafficLights. That raycast branch is
intentionally untouched to preserve native obstacle detection. Whether a specific
signal's collider/layer is encountered by that branch is not established by binary
inspection. This implementation supplies the native ignore-lights flag at its proven
consumer; it is not proof that every real signal is ignored in every simulation mode.
Do not broaden collider filtering without separate evidence/authorization.

Unrecoverable writes to a destroyed native agent cannot be guaranteed to restore;
errors disable the hook and produce a breadcrumb. Other Harmony mods can alter patch
ordering; successful compile/source contracts do not prove IL2CPP hook dispatch.
Each eligible sweep allocates a private native flags object plus managed scope;
allocation/GC cost has not been measured in gameplay.

## Verification

- Observed RED then GREEN: absent consumer wiring; missing runtime-used pure policy;
  missing pure scope; partial install restoration; native alias rejection; published
  pre-write state allowing finalizer restoration retry.
- Full TaxiDriver suite: **324 passed, 0 failed, 0 skipped** (316 prior + 8 added).
  Pure scope tests additionally cover ordinary/idempotent restore, native-consumer
  exception/control changes, nested scopes, and refreshed redispatch settings.
- Release compile-only: `dotnet build Source/Mods/TaxiDriver/src/TaxiDriver.csproj -c Release -p:S1NoDeploy=true -p:BuildProjectReferences=false`, **0 warnings, 0 errors**.
- Runtime breadcrumbs: `[traffic-lights] ... healthy=...`, `phase=consume-private-copy`
  and `phase=restored-original`, with order, agent and original/current flags pointers.
- Local evidence script/output: `C:/Users/pc/AppData/Local/hermes/cache/scratch/taxi-lights-native-evidence.py` / `.txt`.
  Full test TRX: `C:/Users/pc/AppData/Local/hermes/cache/scratch/taxi-lights-results/taxi-lights-full.trx`.

CruiseSpeedPatch.cs, SpikeRunner.cs recovery logic and fare code were not edited.
