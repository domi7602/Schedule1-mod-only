# Traffic-light repair investigation (read-only production scope)

## Initial investigation gate (historical)

This initial read-only investigation made no TaxiDriver production/test changes or deployment. The subsequent approved 0.8.21 candidate and executed checks are recorded in [traffic-light repair verification](traffic-light-repair-verification.md); the constraints below describe the initial blockers, not the later implementation state. The previously approved scope is taxi-only red-light bypass plus preventing explicitly recognized traffic waits from spending recovery budget; generic obstacles, turn/destination braking and unknown stalls must remain unchanged. The old GetWheelSweepCollision hook remains disabled.

## Executed evidence

Tools capstone 5.0.9 and pefile 2024.8.26 were installed with uv into `C:/Users/pc/AppData/Local/hermes/cache/scratch/taxi-native-tools`, with owner approval. System Python packages were not modified. The initial `python -m pip` attempt failed because this Python has no pip; uv succeeded. The local disassembly script is `C:/Users/pc/AppData/Local/hermes/cache/scratch/taxi-native-disasm.py`.

Installed GameAssembly.dll SHA-256 was recomputed and equals the earlier evidence: `61829050e56ca180d67be592e4bfe61ec97d2e9573f720d90a54e318a7e4381d`. Disassembly was read from this exact binary, not from interop thunk bodies.

- UpdateSpeedReduction RVA 0xAD7D20, length 0x97A: reads LandVehicle.Rb at offset 0x188 and calls Rigidbody.get_isKinematic (RVA 0x2BF04C0) at 0xAD8023. The conditional branch at 0xAD802A selects the forward-raycast path at 0xAD8148 if true. The nonkinematic path calls GetWheelSweepCollision at 0xAD806A and 0xAD80AC. The kinematic path calls GetForwardObstacle at 0xAD8169.
- VehicleAgent.InfrequentUpdate RVA 0xAD5280, length 0x377: synchronous calls UpdateSweep at 0xAD559C and UpdateSpeedReduction at 0xAD55A6. These higher-level void/no-argument methods are candidates for a scoped flags override without patching a method containing native out parameters. No candidate hook has been installed or exercised.
- GetForwardObstacle RVA 0xAD3960, length 0x3D3, and its IsValidHit helper RVA 0xAD6EF0, length 0x5E were disassembled. The helper rejects self-child hits, with no IgnoreTrafficLights consumption. Its native name is `<GetForwardObstacle>g__IsValidHit|148_0`, token 0x6000F63; this helper is absent from the generated interop VehicleAgent surface, so AccessTools cannot simply find it there.
- Installed Il2CppInterop.HarmonySupport.Il2CppDetourMethodPatcher was decompiled to inspect its native-to-managed bridge. It converts every original method parameter before calling the patched managed method, regardless of the prefix's own signature, with a special by-ref conversion path. Therefore omitting output parameters from the prefix does not remove the old GetWheelSweepCollision ABI/conversion exposure. This identifies a concrete risk, not a reproduced exact cause of the historical NullReferenceException.
- Intersection.SetPath1Lights / SetPath2Lights exist at RVA 0x5F27F0 / 0x5F2B80 (length 0x38B each), with separate serialized light and obstacle lists. Their live associations and the active stop obstacle at the owner's reported halt have not been inspected.

## Blockers before production repair

The latest log does not record the actual light state/stop obstacle when recovery begins. It cannot distinguish a legitimate red stop from a generic/native navigation stall. Existing gameplay evidence showed physical simulation but does not prove which path is active at each reported light. No live world was available: the game-process check reported closed and the existing MCP bridge was not reachable. Subagent review dispatch also failed because the configured clinepass provider has no usable credentials; no provider/config change was made.

A read-only live observation at the reported red stop is needed before claiming that the stuck timer is misclassifying traffic. Observe exact owned vehicle, active order, Rigidbody.isKinematic, native target speed, flags, current stop obstacle and linked intersection state without creating material/profile instances. Only after that should the higher-level hook candidate and explicit wait classification be finalized. No global light/collider mutation, blanket obstacle bypass or forced throttle is an acceptable substitute.
