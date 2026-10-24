# Registered driver-controller repair — TaxiDriver 0.8.22

## Approved boundary

Use the driver's existing NPCBehaviour, verify exact native ownership and initialized registration, never add networking components after spawn, preserve existing components and use the existing direct Navigate fallback if a safe patrol is unavailable. Do not change S1API, prefab/network construction, saves, traffic-light behavior, obstacle geometry, road recovery, fares or recovery budgets. Native gameplay acceptance remains open.

## Evidence and scope of diagnosis

The 0.8.21 test log reaches Barn (2.1 m) and Sweatshop (5.7 m), then the Docks Warehouse attempts report three NullReferenceExceptions in NavigationCalculationCallback -> NPCBehaviour.DeactivateBehaviour_Server -> NetworkBehaviour.get_IsClientInitialized. The installed native getter at RVA 0xCCAFD0 reads its NetworkBehaviour's _networkObjectCache (offset 0x30), checks for null and throws when absent. GameAssembly SHA-256 was checked by the existing analysis script: 61829050e56ca180d67be592e4bfe61ec97d2e9573f720d90a54e318a7e4381d.

S1API NPC initialization uses NPC.Behaviour and child controller discovery/ownership repair. The old TaxiAI instead searched only the driver's root, could add a duplicate NPCBehaviour and linked a runtime-added VehiclePatrolBehaviour to it. This is an unsafe source-level construction path, not live identification of the exact throwing instance. The first failed departure also reports BrickWall 0.2 m ahead; repairing the controller cannot establish that this geometry/path problem is resolved.

## Implementation

- Select only an existing patrol beneath driver.Behaviour; do not create NPCBehaviour or VehiclePatrolBehaviour, repair network registrations, write owners, or append controller stack entries.
- Validate driver/controller ownership, shared NetworkObject cache, exact ComponentIndex entries in NetworkBehaviours, exact BehaviourIndex entry in the controller stack and server initialization. Only an idle registered patrol may be borrowed.
- If unavailable, return false before activation and use SpikeCommands' existing Go/VehicleAgent.Navigate path. Standard S1API custom NPCs need not contain a registered vehicle-patrol component; direct navigation is intentional, not failed initialization.
- Preserve the original borrowed route, vehicle, aggression and component enabled state. Publish cleanup state before native writes and retain it, including waypoint objects, until release is confirmed. A pure release gate claims cleanup before native work; a still-active/asynchronously deactivating patrol cannot be resnapshotted or compete with replacement navigation. Pending release fences Go, ride fallback, ReDispatch, polling retry, reverse and relocation; deadlines still run. Only mod-created waypoint objects are destroyed, after confirmed hand-back.
- Permanent ownership/binding/network-readiness loss can leave the lease pinned until the native instance is destroyed or a scene reload. Player takeover quiesces only the borrowed component and cannot trigger vehicle/seat actuation; restoration requires safe ownership again. STOP can retry observations but cannot guarantee recovery from every invalid condition.
- Keep the prior 0.8.21 scoped traffic-light bypass. Its owner's successful red-light report is specific acceptance of that tested behavior, not blanket acceptance of all routes or multiplayer.

## Executed verification

Parent observed the original controller source contract fail, the positive registration policy fail, two initial lease/cleanup contracts fail, the retained-release gate/contract fail, then recovery and polling-fence contracts fail before their repairs. Final 683 tests pass (670 prior plus 13 new pure policy/lifecycle-gate/source cases). These do not execute native FishNet initialization, activation, callbacks or deactivation. Forced compile-only Release and scoped production/test formatting pass with 0 warnings/errors.

Full independent review identified early discarded lease/snapshots and precondition/fallback competition; repairs retain ownership until confirmed and fence all actuation entry points. Bounded delta re-review returned PASS, with documented fail-closed availability limitations. Whole-solution formatting reports out-of-scope whitespace in LightControl VisualResolver.cs:99–101; that file was already modified outside this task and is left untouched. Version sync, doc paths and Skills health pass. No in-game claim is made for 0.8.22.

Release deployed with the game closed and BuildProjectReferences=false after confirming Shared is already in sync. Installed DLL/PDB/mod.json were read back and hash-matched; installed metadata reports 0.8.22. DLL SHA-256 `9851f23974ac54509aca229d51c9019e92fd9075d410d54f904d2714079d8e0c`; PDB `2c295b1fb99cc41db0ee1ce06a6b2863b2ecacc6f0ad0aa9a6f5bd8d7ee62017`; metadata `ba9bc13b85dd5a08916a61115ecdc9b0abd27de8185f6d0c76ef3d1f360a8257`. Shared and GLB remain byte-identical. Deployment receipt `20261020T190315922Z-a34395bc56da`, backup `20261020T190315961Z-TaxiDriver-6795c426`. No commit/push.

## In-game checks — open

1. Start a backed-up test slot and confirm 0.8.22 loads.
2. If there is no registered patrol, expect `no idle, registered patrol ... using direct Navigate; no runtime network components added`, followed by the existing fallback dispatch.
3. Ride to Sweatshop, halt, then choose Docks Warehouse and attempt another trip with the same taxi.
4. Check for absence of the prior IsClientInitialized/deactivation exception, correct arrival or bounded honest failure, and unchanged red-light bypass plus real-obstacle braking.
5. STOP, failed start, re-open and scene teardown must not create duplicate controllers, destroy existing behavior components or revive stale route work.

A failed route in front of the wall remains an honest failure requiring separate investigation; do not hide it with globally suppressed exceptions, unlimited retries or collision removal.
