# Navigation settings consistency — 2026-10-30

## Scope and baseline

This continuation addresses settings drift in **mod-issued direct navigation**. It does
not change the native patrol controller's internal navigation, destination selection,
graph sampling, arrival radius, road placement, fares or the existing early-boarding fix.
It is not a Bungalow/Docks reachability repair or a new gameplay acceptance.

The starting working tree is dirty, based on `94a42c5b41abc9e403793de238a75688726681db`.
Before edits, the parent agent observed **767/767 TaxiDriver tests passing** and a focused
Release build with `-p:S1NoDeploy=true` passing with **0 warnings / 0 errors**, using SDK
8.0.426. The game was closed. Existing TaxiDriver source, tests and local documentation
were snapshotted with hashes before implementation.

Installed identity read on 2026-10-30 **before the later authorized deployment**:

| Artifact | SHA-256 |
|---|---|
| `Mods/TaxiDriver.dll` (1.0.5) | `f614acdd3a1dd1d3b99fca263967292df9ebbdc4b9cdea2a8a516e834b0514cc` |
| `UserData/TaxiDriver/mod.json` | `17c6f54d6f4d4675ad1619296523e8cf64ab9a9dab62a73efb83c7891df0d5f4` |
| `MelonLoader/Il2CppAssemblies/Assembly-CSharp.dll` | `346205306273805a78b01a917f14b94518498af1361595c4c417baa0b32b4bd8` |

The initial implementation phase authorized no deployment, version change, game
launch/control or native-save operation. The owner subsequently approved installation
of the exact tested artifact; see the deployment record below. Prior owner acceptance
remains attributed to its original version and scope.

## Confirmed defect, not a guessed graph diagnosis

The initial `SpikeCommands.Go` explicitly supplies:

```text
endAtRoad=True
ensureProximityToGraph=True
teleportToGraphIfCalculationFails=False
```

The baseline `SpikeRunner.ReDispatch` and its one-time polling retry instead pass `null`.
The preserved **2026-10-26, 1.0.4 session** directly records what that changes:

```text
16:55:40.979  op=2 order=6  requestedSettings=[endAtRoad=True,ensureProximityToGraph=True,teleportToGraphIfCalculationFails=False]
16:55:45.482  op=2 order=8  requestedSettings=[null]
16:55:45.483  op=2 order=8  currentNavigationSettings=[endAtRoad=False,ensureProximityToGraph=True,teleportToGraphIfCalculationFails=True]
```

These are abbreviated field extracts, not a newly executed game session. Source:
local `.scratch/taxi-navigation-repair/live104-native/route.log`, lines 60 and 83–84,
SHA-256 `93892c02261ff5a7a89e1a1d2a59dff2d31b4933d1b3517abb4426892401aa85`.
The matching preserved `Latest.log` has SHA-256
`b9230f6298403f58c7968bda20b8a7fc9f976a9baacff056c20e65d6db938dde`.
The earlier parent-session record is retained in [CHANGELOG 1.0.4](CHANGELOG.md).
An independent read-only evidence review confirmed this is a supported first repair.

Null is therefore **not** equivalent to the initial explicit settings: recovery changes
the endpoint policy and re-enables the native failure-teleport flag. The record proves
the flag change, not that every failed recovery actually teleported the vehicle.

## Bounded change contract

Every mod-issued direct `Navigate` invocation must receive a fresh explicit
`true/true/false` settings object, including initial dispatch, callback-less fallback,
recovery/patrol-to-direct fallback and the one-time polling retry. Settings allocation
failure must not fall back to native defaults. Existing callback/order bookkeeping,
retry timing, operation deadline and recovery budgets must remain intact.

No `endAtRoad=false`, new road-point coordinates, graph/tag modifications, arrival-radius
inflation or newly enabled native failure teleport belongs in this patch.

## Implementation and observed checks

- `SpikeCommands.CreateNavigationSettings` centralizes the initial explicit values.
  `Go`, its callback-less fallback, `ReDispatch` and the polling retry each obtain a
  fresh object; a throwing constructor or setter cannot dispatch null/default settings.
  The unused internal `useSettings` bypass was removed after confirming both service
  callers always passed `true`.
- Order/callback bookkeeping, target calculation, native patrol internals, retry timing,
  operation deadline and recovery budgets are unchanged. `SpikeState` changes only
  stale retry comments. The production file budget remains 19 C# files plus the project.
- **Meaningful RED/GREEN:** the coder's 28 new executable cases first produced 23
  assertion failures / 5 passes against the baseline, then 28/28 passes. The independent
  verifier checked the saved RED log: failures expose null dispatch, mutable-object reuse
  and skipped settings construction, not merely a missing-helper compilation failure.
- **Parent rerun:** 795/795 TaxiDriver tests passed, zero failures/skips; focused Release
  no-deploy build passed with 0 warnings / 0 errors. Parent scoped formatting checks for
  all five changed/new source/test files passed without modifying files.
- **Independent delta review: PASS**, no actionable blocker. Review compared against
  the pre-edit working-file snapshots rather than conflating earlier changes in Git.
  Parent also inspected the production/test delta and verified the retained baseline
  files: no unrelated TaxiDriver implementation, project or metadata was changed.

`NavigationSettingsExecutionTests` extracts actual production dispatch, callback,
polling, recovery and policy code into a Roslyn replay harness. Cases cover hostile
settings mutation, throwing Navigate, fresh object identity, constructor/all three
setter failures, synchronous/stale callbacks, patrol handoff and unchanged finite
operation limits. Unity/native boundaries and some supervision helpers are substituted;
this does not execute native pathfinding, physics or the complete Update/mailbox flow.

At the end of source-only verification, the **then not installed** candidate still
reported 1.0.5, with DLL SHA-256
`fb31c4e7881fce86b6e93e0de8a3e8986d9bbe449732065eae495c7b6a46d278`.
Installed DLL/metadata and the referenced game assembly were independently hash-checked
unchanged after verification. No version bump, deployment or new gameplay session occurred.
Local reports are retained under `.opencode/taxi-routing-20261030/`; repository-wide
green is not asserted by these focused checks. Whole-tree/isolated verification reports
are separate evidence, including any unrelated format/version/tool-fixture failures.

## Owner-authorized installation — 2026-10-30

After the owner's explicit approval, the exact tested DLL above was installed at
**2026-10-30T15:55:55Z**, still reporting **1.0.5**. No rebuild, version bump or further
production change was made. All 20 production/project files plus both central build
contracts matched the reviewed source manifest before deployment; installed Shared
also matched the build reference and was not copied.

`Tools/deploy-mod.ps1` copied **DLL and PDB only**; metadata, icon and GLB were identical
and left unchanged. Independent post-deploy readback matched all five planned hashes:
DLL `fb31c4e7…`, PDB `10078685…`. Shared remained unchanged. The game was closed at
preflight and post-deploy checks and was not launched or controlled by the agent.

- Attempt receipt: `UserData/.deploy/receipts/TaxiDriver.20261030T155555656Z-39c272b6e170.json`.
- Backup: `UserData/.deploy/backups/20261030T155555694Z-TaxiDriver-161bce18/`.
  It retains the previous DLL `f614acdd…` and PDB `8a97bcdb…`; both backups were
  independently checked against their manifest and the captured pre-deployment hashes.
- Installed does **not** mean loaded or gameplay-accepted. No new startup, route,
  early-boarding or save/reload result is claimed. No native-save operation, commit or push.

## Owner-run session inspected — 2026-10-31

The owner ran the installed `fb31c4e7…` build; the agent only inspected the closed
session's log, **04:06:14–04:14:36 local (+01:00)**. Loader lines 111–112 identify the
loaded DLL by its complete hash, matching the installed file. The preserved full log
has SHA-256 `d17e5f0d6207a39a3997a7eb48a4fbfa0da48d79cde48a7b791d4837274d0b5e`.
Local capture: `.opencode/taxi-log-review-20261031-041545/Latest.log`; a numbered
diagnostic extract and identity manifest were also retained in the shared vault.

- All **19 logged direct dispatches and their 19 immediate readbacks** retain
  `true/true/false`, including recovery. Operations 5/6/7 reach the 96-event cap;
  later suppressed calls are not separately verified. Cleared current settings after
  native failure are not a new null-settings dispatch.
- Normal pickup reached a confirmed halt at **04:11:38**, 4.2 m from its resolved
  target, before passenger boarding at 04:12:06. This did not exercise early boarding.
- **Barn:** native calculation succeeds but ends at `(154.0, 0.0, 18.6)`, not
  `(172.7, 0.0, -13.8)` (line 677). Despite native `Complete`, the ride stops **37.1 m**
  short at 04:12:31 (703). The historical endpoint mismatch is reproduced.
- **Bungalow:** the first attempt stalls near Barn and terminates **332.9 m** short
  (812). A second attempt changes its target to lot `(-166.1, -4.0, 109.3)` (897);
  its successful path ends at `(-153.0, -4.0, 109.3)` (959), and it terminates **12.2 m
  from that fallback target**, not the original direct target (1021). The next dispatch
  starts at `(-154.1, -3.4, 111.6)` (1038), approximately 4.6 m horizontally from the
  original direct target. That rounded calculation does not establish arrival at the
  different active target or justify changing the 10 m radius.
- **Docks Warehouse:** three attempts fail whole-route calculation after road-seeker
  successes; general-seeker errors still do not establish a particular blocking tag.
  Terminal distances are **185.3 / 181.4 / 177.4 m** (1171, 1316, 1461). Checked
  mod-side free-spot placements are logged; they are distinct from native failure teleport.

The settings repair is runtime-observed within those captured calls, **not** a route
acceptance. Exact save identity, obstacle cause and visual behavior are not established
by this log. No agent-controlled launch, live mutation, save access or deployment was
performed during the log review.

## Remaining gameplay questions

- **Barn endpoint:** with `endAtRoad=true`, the preserved successful calculation ended
  near `(154.0, 0.0, 18.6)` rather than the supervisor target `(172.7, 0.0, -13.8)`.
  Uniform settings do not solve that endpoint mismatch. Keep endpoint policy as a
  separate controlled investigation rather than changing multiple flags at once.
- **Bungalow:** the preserved route calculations could reach the selected target, but
  actual traversal/recovery ended 16.2 m short. Mathematical path existence is not an
  arrival or a successful physical drive.
- **Docks:** the whole calculation failed after road-seeker successes. The general
  seeker reported `Searched all reachable nodes, but could not find target`; its generic
  suggestion about tags does not establish a particular blocking tag.
- **Early boarding:** the 1.0.5 handoff was not exercised in the last recorded live
  session. Its acceptance remains separate and is not claimed by this repair.

The 2026-10-31 trace now provides runtime evidence for the logged settings contract,
but reproduces the route failures. Investigate endpoint selection and whole-path
reachability separately before repeating the same drive sequence. Arrival must still
use the existing confirmed-halt/current-operation rules, not native `Complete` alone.

The later [street-selection candidate](street-dropoff-verification.md) is a separate
2026-10-31 continuation. Its reviewed `7ec8773a…` DLL was subsequently installed with
owner approval, replacing `fb31c4e7…` while keeping version 1.0.5. That does not turn
the earlier failed owner-run routes into acceptance or establish a new loader result.
