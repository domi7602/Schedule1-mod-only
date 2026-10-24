# TaxiDriver visual wheel verification

## Scope — 0.8.20

Visual-only rolling and steering for the imported taxi GLB. No asset, physics, collider, navigation, fare, traffic-light or recovery-policy change. Suspension transfer is deliberately omitted. The existing 19 C# files plus project budget is retained.

## Evidence and implementation

- Source and installed taxi.glb SHA-256: `caf44f9ca230a430dfc07a04cb4cf6ca033f20b925ac9d8600e325f6b6164e65`. Four direct wheel nodes have centred mesh bounds (axle X, radial extent 0.34 m) and child hubs. No Blender/model edit was needed.
- S1MAPI preserves node hierarchy and mirrors translation X; the existing visual root has a 180-degree Y rotation. Pairing therefore compares final imported/native wheel positions in the same model frame rather than trusting FL/FR labels. At most one native wheel drives each matched node; missing links remain static.
- Rolling integrates native Rigidbody point velocity projected onto the imported axle using native collider radius. This is a no-slip visual approximation, not a copy of WheelCollider RPM: wheelspin, skidding and the 180-degree/frame hitch cap can differ from actual wheel rotation. Native steerAngle applies only to native steered-wheel members. Mounting positions, body model, materials and all native physics state remain untouched.
- Link caches survive ordinary ride end/park/reuse so an animated pose is never re-captured as the mounting pose. Scene change, shutdown, confirmed vehicle death and rollback release them without dereferencing Unity objects.
- Only a new visual swap arms initial binding and up to three total attempts, one second apart. Successful binding disarms retries; errors/stale identities cancel them. A whole visual-tick exception boundary prevents visual read failures from aborting the driving update.
- Expected default-log breadcrumb: `[visual] visual wheels bound: 4 of 4 native wheels (4 imported nodes).` A partial count is not full wheel acceptance.

## Executed technical checks

- Baseline: 605 tests. Parent reran final suite: 649 passed, 0 failed/skipped. New coverage consists of executable Unity-free mapping/rotation math and supplementary source contracts; it does not instantiate Unity/native wheels.
- Parent observed lifecycle-contract failures before correcting ride-end retention and explicit retry arming. A prior independent review identified that automatic rebind after ride end would recapture animated rotation; the candidate was not deployed.
- Forced Release compile: 0 warnings/errors. Scoped production/test format verification passed.
- Final independent delta review PASS. Forced Release deployment succeeded with 0 warnings/errors. Installed DLL/PDB/metadata match the built artifacts; metadata is 0.8.20 and both new wheel types were confirmed in the installed assembly. DLL SHA-256: `f36af4381042266a85b1737de61c07928b10705054e6ac769f17b1c93fc31847`. Shared and GLB hashes are unchanged. Gameplay acceptance remains open.

## In-game acceptance — open

1. CALL TAXI: confirm the four-link breadcrumb and usable model.
2. Watch an unobstructed straight drive forward, a reverse manoeuvre and a turn: wheels roll in the appropriate direction, and only the physical front/steered pair turns. The model's historical orientation is not changed by this update.
3. Park/arrival: wheels stop with the vehicle. Start another trip on the same taxi: no doubled/offset steering or mounting-pose drift.
4. Pause/resume: no free-running wheel animation during pause.
5. Exit then STOP, spawn another taxi, and menu/load roundtrip: no stale links, automatic retry loop or native visual errors.

The owner reported testing 0.8.19 separately: its log contained Barn arrival and eventual Sweatshop arrival, two Sweatshop give-ups and recurring road corrections. That evidence does not verify 0.8.20 wheel visuals or close the known navigation/road-jitter issues. Traffic-light ignoring remains disabled.
