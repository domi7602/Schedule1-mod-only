# Native direct-shortcut comparison — 2026-10-31

## Scope and current artifact

The owner approved the next investigation with **"los"**: repeat Barn before the
04:00 clock cap, identify the failing calculation stage, and derive a targeted repair.
During the source-investigation phase the installed build was **1.0.5**, SHA-256
`7ec8773abf897bd29a3e85d6d995cdae9dbec44a079bee5a519970cd98d44aa7`.
The owner later authorized the separate Observe→Guard runtime sequence recorded below;
the currently installed Guard artifact is `4d195888…`, still 1.0.5.
The working tree is dirty `main`, based on `94a42c5b41abc9e403793de238a75688726681db`.
Before this continuation, 134 selected files matched the prior runtime manifest and
were preserved under `.opencode/taxi-daytime-20261031-135015/baseline/`; the reviewed
DLL/PDB were also preserved. Prior [street-selection evidence](street-dropoff-verification.md)
and owner acceptance of v1.0.0 retain their separate scopes.

## Fresh comparison before 04:00

The agent launched through Steam and loaded **Dominik, slot 1**, at the same Town Hall
entrance. CUA 0.21.0 controlled menus and captured the window; bounded Win32 scan-code
movement/E and relative mouse input supplied the previously established Unity input
fallback. No clock, graph, destination or navigation settings were changed externally.

- Pickup again targeted `(52.5, 0.0, 12.1)`. The player boarded after the taxi was
  parked, with 0 km/h and **ON BOARD** visible.
- Screenshot `19-barn-dispatched.png` shows **21:39**, before the clock cap; the
  selected Barn street point remains `(165.3, 0.0, 25.1)`, logical goal
  `(180.1, 1.0, -11.0)`. The initial taxi root was `(54.1, 0.6, 9.2)`, close to but
  not byte-identical with the earlier `(53.9, 0.6, 9.2)` context.
- Four whole calculations failed again, operation 2, orders 4/6/7/8; their requested
  and effective settings remained `true/true/false`. The final general-seeker errors
  have path IDs **79/98/117/130**.
- GAVE UP at `(58.2, 0.6, 9.5)`, **108.2 m** from the street point. The app showed
  **COULDN'T GET THERE**, FARE $2; the meter reported payment calls returned $2 with
  balance movement unverified. Fare behavior was not changed by this investigation.

Immutable log: `.opencode/taxi-daytime-20261031-135015/Latest-before4-barn.log`, SHA-256
`d4a96b18d5939928a3cf4f3e95fdd4e07d4e5773e79807301930824c352bd382`.
Loaded identity: lines 111–112/226; Barn selection/dispatch: 680/688–689; full failures:
707/744/773/797; terminal result: 803/809. The native-investigation agent independently
rechecked this log and the two result screenshots. **04:00 is not necessary for the
failure.** This is an evening comparison, not a daytime-route acceptance result.

After the terminal result the player exited and paused. Read-only bridge inspection
verified the listener belonged to this game process (PID 13900, started
`2026-10-31T12:51:35.1626018Z`), scene Main and clock 22:34. Through the registered
`taxi_driver`'s CurrentVehicle, the final general path still had ID **130**, error=true,
CompleteState=Error, a **non-null vectorPath with Count=0**, enabledTags=-1 and
nnConstraint.graphMask.value=1; seeker traversableTags=-1. These scalar reads are
retained in `mcp-taxi-seeker.json` and `mcp-failed-path.json`. ABPath original endpoints
were unavailable through the base Path wrapper; "member not found" is not a null
endpoint observation. No MCP setter or path query was invoked.

The test was closed through the visible Quit confirmation and main-menu Quit; process
absence was confirmed at **14:09:43 local (UTC+01:00)**. No explicit save/sleep or native
save-file operation was performed; save-byte preservation was not measured. The installed
DLL stayed `7ec8773a…`. Exit log SHA-256:
`3638cf16cb4e72a6fb6d21afba20faa8aaa94b32a5021248ed30c4ad5579e934`.

## Native evidence and missing causal observation

Independent reconstruction from the installed **GameAssembly.dll** machine instructions,
SHA-256 `61829050e56ca180d67be592e4bfe61ec97d2e9573f720d90a54e318a7e4381d`,
is retained in `.opencode/taxi-next-native-20261031/findings.md`, `identity.json`,
`native/` and fresh `api/` dumps. Dummy/interop assemblies establish bindings and RVAs;
their empty bodies were not treated as implementations.

1. Successful native groups consist of general start→entry, road entry→exit and general
   exit→destination legs. Every leg's error is checked when constructing the group.
2. With at least one group, native code selects a group, applies endAtRoad, then runs
   an **optional direct general-path comparison** from the vehicle's front-offset
   calculation start to the original requested street coordinate.
3. At generated routine `MoveNext` **resume state 2** (RVA `0xAD0D1D–0xAD0DB3`), it
   compares the selected **start-to-entry leg's** length with the direct path's length,
   **without checking the direct path's error**. A shorter direct path replaces the group.
4. Native `Path.GetTotalLength` (`0xB5BE60`) gives a non-null zero/one-point list length
   **0**. A failed empty direct path can therefore replace a valid group; the subsequent
   fewer-than-two-points check yields Failed/null.

This missing error guard is a concrete native defect. The retained failing current
path supplies its dangerous zero-point input in the actual Barn session. However, the
discarded selected group's identities/counts/lengths are not retained after termination.
The before-04:00 log did not prove that exact replacement in that run; a later owned
state-2 Observe/Guard session, documented below, measured it in a fresh reproduction.
Existing seeker logs are prefixes,
before native modifiers; a logged one-point successful subpath is not its final length.
No blocking-tag or road-disconnection diagnosis follows from the generic error text.

## Repair contract

Exclude an errored direct shortcut from this exact owned native comparison, preserving
the already-selected native road group. Ownership, calculation identity and lifecycle
must be positively established; uncertain reads leave native behavior untouched.
Record bounded scalar before/after observations, clear any synchronous scope on normal
and exceptional exits, and verify both patch registration and actual hook invocation.
Do not mutate borrowed path data or global A* semantics, add a competing seeker query,
change targets/tags, widen arrival tolerance or increase recovery budgets. Compilation
and managed replay tests do not establish native group retention or a completed ride.

The independent design review (`.opencode/taxi-next-design-review-20261031/review.md`)
gave a **conditional source-candidate pass**, not deployment or gameplay approval.
It requires dispatch-creation nonce binding at state 0, rather than attaching current
labels to a potentially stale routine at state 2. The override must consume only the
ordered entry-leg/native-length then failed-direct comparison, once; the native state
has already changed from 2 to -1 during those calls. Nested/foreign calls mask the
outer scope, partial patch installation stays inert, and exceptional cleanup preserves
the original exception. The next native test must first capture the destructive choice
in Observe mode, then demonstrate retained group identity and actual arrival in Repair.

## Initial instrumented candidate — rejected by implementation review

The coder implemented the guard in the existing `DrivingPatches.cs` and pure call
protocol/bootstrap seams in `DrivingPolicies.cs`, with lifecycle wiring in `Mod.cs`.
The production budget remains **19 C# files plus the project = 20**. No existing
production files were consolidated or moved. The candidate is **not installed**.

Four dependent native originals cover dispatch creation, generated MoveNext,
GetTotalLength and calculation-result observation. Compatibility is hash-pinned to
the inspected native binary/metadata, interop and Harmony bridge; required registrations
and interfering patches are checked. Only an originally bound state-0 receipt can
authorize state 2. The comparison consumes permission once after its actual finite-
positive entry-length call. Borrowed paths, vectors, closure slots, targets, tags and
settings are not rewritten. Logs are capped at 32 lines per operation.

There are two explicitly distinct compile-time artifacts, both version **1.0.5**:

| Build | Selection | SHA-256 |
|---|---|---|
| **Observe (default)** | normal build or `-p:TaxiShortcutGuard=false` | `dbc14e2b428dc1c08f231cf8e77ef0a11c711e5e230e2bf4865de0adece33ac8` |
| **Guard** | `-p:TaxiShortcutGuard=true` | `5d3a71dca6a2dfac1df3e17543499b6c0c490a61086249b5c31068c8ebf25653` |

Use `-p:S1NoDeploy=true` for either verification build. There is no runtime mode
setting or new gameplay command. A subsequent ordinary build produces **Observe**,
even if the previous output was Guard; version alone does not identify the variant.
Frozen DLL/PDB pairs, five source hashes and receipts are retained under
`.opencode/taxi-shortcut-coder-20261031/`, including `candidate-identity.json` and
`handoff.md`. The installed DLL at that source-review point was `7ec8773a…`.

Coder evidence: **75/75** focused execution cases, both Release no-deploy builds
**0 warnings/errors**, scoped formatting and whitespace checks passed. The executable
native-comparison replay discards a healthy group for the Error/empty/zero direct path
in Observe, and retains the original group in Guard. A labeled test-only compile-symbol
bypass gives **1 assertion failure / 6 passing controls**, not a compiler failure and
not an assertion that the historical build contained these new hooks. Native boundaries
are substituted in the replay. The later authorized native Observe→Guard sequence
measured creation binding, getter hits and group retention; ride outcomes are below.

Parent full-suite verification passed **1,344/1,344**, zero failed/skipped, in
`.opencode/taxi-shortcut-parent-20261031/tests/taxi-shortcut-parent-full.trx`.
Parent independently rehashed all five handed-off source/test files and four frozen
DLL/PDB artifacts; they matched `candidate-identity.json`. The installed DLL remained
`7ec8773a…` and the game was closed. This broadens managed regression coverage, not
native execution evidence.

The independent implementation review subsequently returned **CHANGES REQUIRED**.
Its isolated harness executed the unchanged production bodies and injected five
revocations inside the final native-read validation: epoch invalidation, nested
state-1 MoveNext, changed order, changed dispatch nonce and the production Fault path.
All five still applied the infinity override, despite clean final stack cleanup.
The validation callback could revoke the frame after its pre-check, and the original
code could then advance/consume that stale frame; native receipt reads also lacked a
final managed-only currency check. This is a demonstrated managed boundary violation,
not evidence that the native Path.error getter normally calls those injections.

The two DLLs above are **rejected history, not installation candidates**. Their five
source/test files and four artifacts are frozen under
`.opencode/taxi-shortcut-parent-20261031/rejected-initial/`. Exact review, eight probe
outcomes (five failed safety checks, three controls) and harness are under
`.opencode/taxi-shortcut-verifier-20261031/`. The correction must prevent validation
reentry, recheck frame/stage/epoch after it, then check managed receipt currency after
the final native read before consuming permission. Assertions must test zero override
at revocation, rather than only later cleanup. At that review point the installed
`7ec8773a…` was unchanged; the later installation is recorded below.

## Corrected R1 candidate — bounded closure PASS

The narrow correction changes only `DrivingPolicies.cs`, `DrivingPatches.cs` and the
focused execution test relative to the rejected candidate. Each validation invocation
has an exclusive reentry marker, with the expected frame/epoch/stage checked again
after its external callback. The last native-backed receipt read is followed by a
managed-only reference/fault/readiness/epoch/nonce/lifecycle/operation/order/generation
and scalar-target check; no Unity getter, logging, delegate or native Vector3.Equals
call is used in that final predicate. Non-finite getter returns persistently fault the
protocol; ordinary eligibility loss leaves the native return intact.

The coder first ported the review's five exact native-Path.error revocation callbacks
and additional validation/last-GetCurrentPath/numeric cases. The unchanged rejected
production bodies gave **22 assertion failures / 7 passing controls**, without compiler
failure. The identical set then passed **29/29**; the expanded focused class passed
**97/97**. Parent full suite passed **1,366/1,366**, zero failed/skipped:
`.opencode/taxi-shortcut-parent-20261031/fixed-tests/taxi-shortcut-parent-fixed.trx`.
These tests assert zero override during revocation, not merely clean later teardown.

New frozen artifacts, still **1.0.5**, under
`.opencode/taxi-shortcut-coder-fix-20261031/`:

| Mode | DLL SHA-256 |
|---|---|
| Observe (default) | `74c8c3fded5ea8c97122557851e88da712b6ae0223d467317173f6a3bc4612a4` |
| Guard (`-p:TaxiShortcutGuard=true`) | `4d1958880781575a66b12b53dbf82cfb2529cba1c67c5303aa12847f2328cd21` |

Both Release no-deploy builds passed with **0 warnings/errors**; scoped formatting and
whitespace checks passed. `fix-identity.json` identifies all 20 production files, the
focused test and four DLL/PDB files; rejected artifacts remain unchanged. Neither new
variant was installed at source-verification time. The later live sequence below gives
actual native binding and comparison observations; its route results are separate.

Independent bounded closure **PASS**, retained under
`.opencode/taxi-shortcut-verifier-fix-20261031/`: **24/24** independently executed
scenarios, including the original five revocations with zero override, nine old
positive/native controls, four validation-recursion stages, two validation exceptions,
late order/nonce changes and two NaN returns. The reviewer used corrected frozen
production bodies with the original frozen boundary fixture, and independently
inspected the retained Guard DLL IL for the managed-only final gate/consumption.
This closes R1 and adjacent reviewed behavior, not native detour or gameplay acceptance.

The reviewer also verified RED provenance: 22 assertions comprise **18 unauthorized-
override failures, three fault-policy failures and one marker-specific assertion**.
They must not all be described as 22 independent unsafe overrides. The seven old
controls passed; GREEN uses the same test-source hash as RED. Source/DLL/PDB hashes
matched the frozen identities, and rejected history was retained.

Repository Quick completed with **5 steps passed / 2 failed**, the pre-existing
global format/version drift outside TaxiDriver. Documentation paths, skills, solution
determinism and MSBuild XML passed. Full isolated Pure and final source/binary identity
records are retained separately under `.opencode/taxi-shortcut-parent-20261031/`;
no whole-repository green claim is made.

## Owner-authorized Observe → Guard test — 2026-10-31

The owner answered **"ja"** to installing and testing Observe, then installing Guard
and testing Barn/Bungalow/Docks if the native cause was confirmed. The game was closed
through each deployment; no rebuild or version bump occurred. All **136 selected files**
matched the final source manifest before staging both artifacts. Both five-file payloads
were hash-checked; only the DLL and PDB changed. Metadata, icon, GLB and Shared stayed
byte-identical. Independent installation audits verified actual destinations, receipts,
and both overwritten backups for each attempt.

| Variant | DLL SHA-256 | Deployment UTC / attempt | Backup of prior DLL/PDB |
|---|---|---|---|
| Observe | `74c8c3fded5ea8c97122557851e88da712b6ae0223d467317173f6a3bc4612a4` | 2026-10-31T17:10:57Z / `20261031T171056983Z-30a6bf0c8bd5` | `20261031T171057022Z-TaxiDriver-4d3ff816` (`7ec8773a…`) |
| Guard | `4d1958880781575a66b12b53dbf82cfb2529cba1c67c5303aa12847f2328cd21` | 2026-10-31T17:18:55Z / `20261031T171855324Z-b37a4be52686` | `20261031T171855361Z-TaxiDriver-c24a275f` (`74c8c3fd…`) |

Receipt, before-state, parent readback, installation and runtime reviews are retained
under `.opencode/taxi-shortcut-live-20261031-181047/{observe,guard}/`. MelonLoader
reported the exact Observe hash at **18:11:48 local** and Guard hash at **18:19:47 local**
(UTC+01:00), both v1.0.5. Both logged `compatible=True registrations=True`; actual
hook execution below is separate from registration.

### Observe: native causal replacement confirmed

The agent used Steam, CUA 0.21.0 and bounded Win32 scan-code/mouse supplementation
to load **Dominik slot 1**, call a new taxi at Town Hall, wait for pickup halt and
board normally. The Barn logical goal was `(180.1, 1.0, -11.0)` and its requested
roadside point `(165.3, 0.0, 25.1)`.

In all four operation-2 orders **4/6/7/8**, the Observe trace showed an owned state-0
binding, a selected group with successful positive-length entry and road legs, and
an exact distinct failed direct path with non-null empty vector and native length 0.
The two actual native length getter hits were unchanged; after comparison the group
identity changed to one containing **that same failed direct path**, with no road leg,
and the native callback returned **Failed/null**. Example order 4: valid entry length
`0.29101568`, road length `112.82171`, failed direct ID 60 / length 0. Comparison
cleanup was clear. The ride GAVE UP **107.7 m** from the roadside point; this was a
deliberate diagnostic baseline, not a repaired ride.

Frozen `observe/game/Latest-observe-barn.log`, SHA-256
`b8288a3a1a14dce5bbac142e76c6c7848d0c0ab0c8fed7c30de7ecc373d49083`; independent
`observe/causal-verifier/audit.md` verified the pointer/ID/timestamp sequence. This
closes the earlier native comparison *causal* gap for this instrumented session only.

### Guard: comparison corrected; three separate ride outcomes

Guard was tested in one game process, with each route starting after a separate menu
reload of the same Dominik slot/Town Hall view and a fresh called taxi. Each pickup
had a confirmed halt, followed by ordinary E boarding. These are three individual
observations, not repeated trials or a chained route.

| Destination | Calculation / intervention | Actual route result |
|---|---|---|
| **Barn** | First dispatch and one recovery both had the failed empty direct path. Guard changed only its comparison return `0 → Infinity`, retained the same valid group/entry/road pointers and the native callback returned **Success**; first 86-point path ended `(165.30, 0.00, 25.12)`. | Taxi progressed from 112.4 m to the destination area, then stalled/recovered and **GAVE UP**. Navigation decision logged **4.9 m**; later ride status **5.5 m**, `callback=-`, app **COULDN'T GET THERE**. Subsequent 0 km/h is a failure halt, **not confirmed arrival**. Cause of the stall is not established. |
| **Bungalow** | Logical goal `(-158.1, -3.9, 113.7)`, selected street point `(-156.0, -4.0, 113.7)`. The direct candidate was successful, so **override=False**; native 375-point road path ended `(-152.99, -4.00, 113.69)`, about 3 m from selected target. | Callback Complete while moving, then **confirmed halt 1.0 km/h, 3.5 m from selected target**, app ARRIVED; later screenshot 0 km/h. |
| **Docks Warehouse** | Logical goal `(-76.6, -2.5, -59.9)`, selected street point `(-72.2, -2.5, -47.9)`. The direct candidate was successful, so **override=False**; native 243-point road path ended `(-69.34, -2.50, -46.32)`. | Callback Complete while moving, then **confirmed halt 0.9 km/h, 3.1 m from selected target**, app ARRIVED; later screenshot 0 km/h. |

Guard frozen logs: Barn `4083060f0e1ac36637bb3f92ace0ac43e00257b433c8ca51a576d81ccf1a53db`,
Bungalow `a011196652a7e4bd601b8f0dae3dce7f3f4191c468ff321caa73e1cfb9733048`,
Docks `756d74f2ab7216be094a3bcc8c7f2edbe83458b008e214863c7ec415628fad72`.
Independent route audits are in `guard/{barn,bungalow,docks}-verifier/`. Guard's
effect is directly observed for the two Barn dispatches; the successful Bungalow
and Docks runs are **pass-through observations**, not proof the guard changed them.
All seven observed comparison scopes in the three Guard rides closed cleanly, with
no shortcut-disabled/fault diagnostic; foreign/out-of-context behavior was not
independently exercised. Early boarding, a chain, multiplayer and broad reliability
remain open.

The app displayed fares $19 (Barn), $64 (Bungalow), $41 (Docks). Individual balance
readbacks and HUD values supported those reductions in the temporary sessions; final
aggregate settlement logs label balance movement unverified. No payment/persistence
acceptance is asserted. The agent requested no explicit save/sleep; each route was
left through the UI's unsaved-progress warning, and no native save files were read,
written or hash-compared by the agent. The final game and CUA driver were closed;
Guard remained installed. Final exit log SHA-256
`afab9d94019e529842a7920b2b95e07d2916f97f3561bfc77c9ae0e2dc807619`.
No commit, push, version change, or post-approval code rebuild occurred.

## Barn recheck and supervisor findings — 2026-10-31

The owner requested **"Kannst du das nochmal machen?"** after the proposed Barn
investigation. The agent repeated one Barn ride on the **same installed Guard**
`4d1958880781575a66b12b53dbf82cfb2529cba1c67c5303aa12847f2328cd21`, v1.0.5,
and independently reviewed the existing supervisor. No production-code change,
new mod build or deployment occurred during that recheck. The later uninstalled
repair candidate is recorded separately below.

Evidence is retained under `.opencode/taxi-barn-recheck-20261031/`. Before launch
and after shutdown, all **136 selected files** matched the preceding final manifest
`74f05852cdfbf65bffe930ac742575eb9d9a8603ad97ab913338be4c26db3c78` on dirty
`main`, revision `94a42c5b41abc9e403793de238a75688726681db`. These checks predate
this documentation addition and do not identify the entire repository.

### Repeated route: still failed

Steam launched the game; MelonLoader recorded the exact Guard hash at **19:45:06
local (UTC+01:00)**. CUA 0.21.0 plus bounded foreground Win32 input loaded **Dominik
slot 1** at Town Hall, called a fresh taxi, observed its pickup halt, and boarded
normally. Player walking/boarding timing and the taxi's precise dispatch pose were
not controlled to be identical to the preceding run. No pause occurred during the ride.

- Same Barn logical goal `(180.1, 1.0, -11.0)` and selected street point
  `(165.3, 0.0, 25.1)`. First dispatch and recovery again preserved the valid road
  group through Guard and returned native calculation Success.
- After real travel, the taxi became stationary near the target, reversed once,
  and became stationary again near `(160.8, 0.7, 23.0)`. Refused checked placement
  and its one delayed re-probe ended in **GAVE UP at 19:52:00**, **3 recovery
  episodes**, **5.0 m** remaining, `callback=-`. This run did **not** reproduce the
  older fourth episode/17.4 km/h terminal sequence; do not merge the two timelines.
- `game/32-barn-result.png` shows **COULDN'T GET THERE / FARE $16**. The preceding
  run's $19/5.5 m remain historical. Neither the later 0 km/h nor the 10 m radius
  converts the failed operation into an arrival.
- The 15 bounded ride captures and on-foot `36-scene-ground.png` show the taxi at
  the rounded paved-road end, extending into the vegetated edge. A separately
  labelled RGB-times-six derivative aids inspection; the original is preserved.
  This is **scene/placement evidence, not a measured collision or an identified
  blocking collider**. Target clearance and the exact first-stall physical cause
  remain unproven.

Frozen terminal log: `game/Latest-barn-terminal.log`, SHA-256
`6308594d106148ba2efdfa823ce5a51a6513aeb78af3e9c71a94e3f08b2d0926`.
The earlier capture-series log is
`85168fcf1aaa9c64346aefac28a106ad14a2f05b6f68c467a1532c019f0e37a6`.
Read-only S1MCP inspection matched the listener to the CUA-pinned game process;
`taxi_driver.CurrentVehicle` resolved to `Shitbox(Clone)` and later position
`(160.80916, 0.6996801, 23.013592)`. The prior name scan's zero results covered
only 2,000 objects and did not establish absence. These reads made no game-state
writes. UIA capture/input-report failures were followed by fresh observations;
unknown input results were not treated as successful actions.

### Source findings at recheck time

The independent `supervisor-audit/audit.md` identifies two concrete control-flow
gaps in the then hash-matched `SpikeRunner.cs`. At that point they were inspected
findings, not executable regression results or a completed repair:

1. **Deferred recovery is not revalidated against resumed motion.** The pending
   probe branch calls `TryBeginRecovery` before ordinary progress evaluation and
   discards its `true` result. A due probe can spend an episode, clear progress
   state and return permission without a manoeuvre; the following heartbeat then
   sees the old progress window. This strongly fits the earlier episode-3/4
   sequence, but the missing episode-3 position/recurrence snapshot prevents a
   complete runtime reconstruction.
2. **Direct driving omits the pre-recovery progress-backed halt gate.** Patrol
   calls `SupervisedArrivalPolicy`; the direct path runs recovery and returns on
   `AutoDriving=true` before its terminal check. A genuinely driven, already-halted
   taxi within 10 m can therefore miss that decision. The old drive-start proof
   also accepts absolute reverse speed, so it cannot simply be reused as durable
   forward-travel evidence without checking its provenance.

The subsequent uninstalled candidate below implements the then-next contract: share a current-operation, readable-halt arrival
gate before recovery, quiesce owned native driving with callback/identity fencing,
and revalidate delayed-probe eligibility using recent physical motion. Canceling an
obsolete probe must not replenish budgets or forgive reverse/artificial movement.
If braking while moving is added, use a distinct bounded settle intent and confirm
the subsequent halt; never synthesize a native Complete. `RequestPark` alone is
insufficient while polling owns an active actuator order. Regression coverage must
execute the actual polling caller together with progress/recovery decisions.
Keep the 3D 60 m goal budget, strict selected street point, 10 m plus confirmed-halt
verdict, native settings and finite recovery budgets. Physical clearance remains a
separate open observation; source corrections alone will not establish it.

Game and CUA driver were closed at **19:58 local** after normal UI Quit. The agent
requested no explicit save/sleep and did not inspect/hash-compare native saves;
save immutability is not claimed. Exit log SHA-256
`75f030390d77326888fe41baa9ac2b467c5d53584b7aeeda283118d080107a88`.
Bungalow/Docks retain their earlier single observations; early boarding and chained
travel were not exercised. No new gameplay acceptance, version change, commit or push.

## Uninstalled supervisor repair candidate — 2026-10-31

After the owner said **"weiter"**, the two source gaps above were repaired without
changing the installed Guard artifact. The candidate remains version **1.0.5** and
is **not installed**. Local no-deploy DLL/PDB:

| Artifact | SHA-256 |
|---|---|
| `TaxiDriver.dll` | `8153b7b6863c39c78e3e0fb0a0368fc6054115cd22eb55651b6ecec7ed68ad0a` |
| `TaxiDriver.pdb` | `8ffea14f67491c60c3fb14815db111711f31aab764dbfa45c295dd467d155730` |

Frozen copies and parent identity are under `.opencode/taxi-supervisor-fix-20261031/`.
The installed DLL remains `4d195888…`. Basis revision is still dirty `main`
`94a42c5b41abc9e403793de238a75688726681db`.

- Direct, patrol and delayed-probe recovery now share the existing supervised
  arrival decision before spending an episode. Completion requires a readable halt
  within the unchanged **10 m**, no reverse and current-operation forward evidence.
  The latter requires positive throttle plus signed speed or net approach; absolute
  reverse speed and the old drive-start flag are not reused as arrival proof.
- Completion invalidates the current order and suppresses the controlled-stop
  callback before stopping owned navigation, then rechecks identity. It does not
  synthesize native Complete, call `RequestPark`, widen the radius or change the
  60 m goal budget, settings, selected street point or finite recovery budgets.
- A due delayed probe is canceled when recent physical forward motion makes the
  stale stall unjustified. Cancellation spends no episode and does not dispatch,
  clear history, refill budgets or reset the deadline. A granted recovery is no
  longer discarded. A genuinely stationary refusal still gets one delayed probe
  and the existing terminal. Moving inside 10 m does not become arrival.
- Decision-scoped diagnostics record schedule, due, cancel, recovery grant and
  arrival hold/complete. They add no scene scan.

The new replay executes the actual polling caller with recovery and arrival policy.
Against the unrepaired caller it failed **7 / passed 3**; that RED log is retained.
After repair, the parent ran the full TaxiDriver suite: **1,376 passed, 0 failed,
0 skipped**. Release no-deploy build: **0 warnings/errors**. Scoped formatting
verification passed without edits. Independent source review is **PASS**, with no
installation blocker and explicit non-blocking residuals; it did not rerun tests or
authorize installation. [Review](../../../../.opencode/taxi-supervisor-fix-20261031/verifier/audit.md).

This is managed replay and compiler evidence only. It does not identify the Barn
collider, prove that either physical stall would now arrive, or replace the required
fresh in-game Barn retest. No deployment, launch, save access, version bump, commit
or push occurred.

The subsequent isolated Pure run of this documentation state finished **20 passed /
2 failed**. The failures are the existing unrelated global `format` and `versions`
steps: TaxiDriver itself is version-sync **True**, and its isolated suite passed
**1,376/1,376**. The snapshot remains Failed; this is not a whole-repository green
result.

## Guard + supervisor live Barn run — 2026-11-01

Owner said **"ja"** to installing the supervisor candidate and driving Barn, with Bungalow
and Docks as follow-ups. Game was closed for every copy; no version bump, no rebuild of the
reviewed supervisor sources, no commit or push. Revision remains dirty `main`
`94a42c5b41abc9e403793de238a75688726681db`.

### Selected-artifact trap: the frozen candidate is Observe

The frozen supervisor artifact `8153b7b6…` was built with the default selection, i.e. the
native shortcut guard **off**. Installing it alone would have silently removed the Guard fix
that the Barn investigation depends on. It was deployed first at `04:15:15Z`
(attempt `20261101T041515662Z-9e0c0ce3daba`) and launched at **05:15:41 local**; the loaded
assembly then logged `[shortcut] mode=Observe compatible=True registrations=True` (line 322)
and `state2-post mode=Observe override=False` on the pickup dispatch. No ride was completed
in that run; it was closed normally at 05:29. Its log is retained as
`run1-observe-26-11-1_5-15-41.log`, SHA-256
`f5ce0ab18aebe350340e7252de3182dad3ac7dcff795476a9d636b8381e6b6f0`. This is why version
`1.0.5` alone is not a build identity.

For the actual test the same reviewed supervisor sources (SpikeRunner `8aa30193…`,
`SupervisorArrivalRecoveryTests` `b25b1e3a…`) were built with the guard enabled:

```text
dotnet build Source/Mods/TaxiDriver/src/TaxiDriver.csproj -c Release -p:S1NoDeploy=true -p:TaxiShortcutGuard=true
```

Result **0 warnings / 0 errors**. Combined artifact, still **1.0.5**:

| Artifact | SHA-256 |
|---|---|
| `TaxiDriver.dll` | `012330b16b8fc6470c7bc0c299f1f43146821c63d232e807fdbd36d6e2528b42` |
| `TaxiDriver.pdb` | `69ee0e22b84945c1056b55292306f2409d0250efbb18c132258266028acc0c43` |

Deployed `04:30:44Z`, attempt `20261101T043044673Z-ded9db5c5779`, backup
`20261101T043044709Z-TaxiDriver-a7f07701` (the Observe DLL/PDB above). The helper copied the
DLL and PDB only; metadata, icon, GLB and Shared were reported `NotAttempted`/unchanged.
Destination readback matched `012330b1…`. This exact combined binary was built for this test;
the earlier independent review covers the supervisor **source**, not this variant.

### Result: Barn arrived

Launched through Steam at **05:30:58 local**, loaded **Dominik slot 1**, walked out of the
Town Hall interior and called the taxi with destination **Barn** at
`(48.0, 1.06, 16.75)`. Pickup target `(48.0, 0.1, 15.9)`; pickup navigation started
05:33:58 and parked at 05:34:18 (`[park] pickup finished`). The agent needed several minutes
to find and board the parked vehicle; the app showed `BOARD NOW` meanwhile. Boarding was the
vanilla E entry at ~05:48:28 (`[ride] passenger seat fix applied`).

Ride dispatch `op=2 order=4` to the selected street point `(165.3, 0.0, 25.1)`:

- **Guard active on the Barn comparison.** `native-getter stage=4 … original=0
  effective=Infinity override=True` (line 1350) and `state2-post mode=Guard override=True`
  with `groupBefore == groupAfter` (line 1353): the native road group survived the failed
  empty direct path. The `mode=Guard` string is the build-level distinction run 1 lacked.
- **Supervisor completion on a real halt.** `[nav] supervisor op=2 order=5
  trigger=pre-deadline dist=5.0m speedReadable=True speed=1.0 recentMotion=False …
  action=completed` (line 1360), i.e. a readable halt inside the unchanged 10 m radius.
- **Physical arrival.** `[ride] ARRIVED at Barn — 5.0 m from the target (165.3, 0.0, 25.1)
  … press E to exit` (line 1362). After exiting, the bridge read
  `(160.6472, 1.0061, 23.8349)`, **4.8 m** from that street point. The first post-boarding
  read still showed the pickup position because the ride had just begun.
- **Fare:** `$0` (`0 moving in-game minute(s)`, line 1361). Payment semantics were not
  evaluated and are not changed.
- In-game clock at arrival was **04:00** (day 2). This run therefore does not demonstrate
  anything about the earlier 04:00-cap hypothesis; the 21:39 run already showed the cap is
  not required for the native failure.

After exiting, selecting **Bungalow** in the app changed the destination, but no second ride
was completed: the taxi could not be re-boarded in the dark field. Bungalow and Docks remain
their earlier one-observation results and are **not** re-verified here. The app continued to
show `BOARD NOW` with the destination retained after the ride ended; that state was observed
but not investigated.

The game was closed through the visible Quit confirmation and main-menu Quit at
**05:57:45 local**. No explicit save or sleep was requested. The 79 native save files were
byte-identical before and after (`saves-before.json` / `saves-after.json`, zero diffs).
Run-2 log retained as `run2-guard-26-11-1_5-30-58.log`, SHA-256
`6fc70a37b0b667ce7e9778c4f181e4fffc113ca4d58b24ef86b6f829017d2794`; screenshots
`r2-01…r2-75` and both deploy receipts are in
`.opencode/taxi-supervisor-live-20261101/`.

### Bungalow ride and the aborted Docks attempt — second launch

The same installed artifact `012330b1…` was used for a second launch at **07:14:32 local**
(no rebuild, no redeploy). Logged startup shows `mode=Guard` (line 322). The town-hall exit,
taxi call and boarding needed the same manual approach as before.

**Bungalow arrived.** Pickup target `(48.1, 0.1, 15.9)`; the supervised gate completed the
pickup at **8.6 m** (`[nav] supervisor op=1 order=2 … action=completed`, line 544), so the
taxi parked noticeably short of the player and had to be located on foot. The ride
`op=2 order=5` to `(-156.0, -4.0, 113.7)` logged `state2-post mode=Guard override=False`
(line 1456): the direct candidate was successful, so this is a **pass-through** observation,
not guard evidence. Result: `ARRIVED at Bungalow — 3.5 m from the target (callback=Complete)`
(line 1480), fare `$0` (no full moving minute). Post-exit position
`(-154.3635, -3.0250, 115.7725)` is **2.8 m** (3D) from the target.

**Docks did not happen.** After exiting at Bungalow, selecting Docks Warehouse and pressing
`CALL TAXI` produced a user-visible `Taxi dispatch failed at step 1 … cleanup pending or
uncertain`. The log shows why: the previous taxi's destroy was still unconfirmed
(`Cleanup of the previous spike vehicle failed — refusing to spawn a second one (freeze
guard)`, line 1933), and the same tick confirmed the death 87 ms later (line 1937). A second
CALL then spawned normally (line 1955). Its pickup exercised the repaired decision path
cleanly: `hold-arrival` at **13.0 m**, then `grant-maneuver` (`budget 0->1`), recovery
episode 1, and finally `pre-deadline … dist=3.3m … action=completed` (lines 2010, 2012,
2056). The agent could not re-enter that parked taxi in the dark field; the attempt was
therefore ended without a Docks ride. **Docks Warehouse keeps its 2026-10-31 observation
only.**

The game was closed through the UI at **07:50 local**; the 79 native save files were again
byte-identical before and after. Run-3 log `run3-guard-bungalow-26-11-1_7-14-32.log`,
SHA-256 `c61e82ffc95ae8d0fe15859577ba828de02f54eee9515248cc2154101dd7543f`.

### Limits

Two rides (Barn 2026-11-01 morning, Bungalow 2026-11-01 evening) and one supervised pickup
recovery on one combined local build. At the time of those agent runs this did not establish
repeatability, Docks behavior, early boarding, chained rides, the physical collider at the
street point, or owner acceptance. Dominik's own later session is recorded below and is not
covered by that sentence. The Barn taxi stopped 4.8 m and the Bungalow taxi 3.5 m from their selected
points; whether they stopped on paved ground or in the verge was not measured. The
cleanup-pending dispatch failure above is a user-visible consequence of the pre-existing
fail-closed guard, not a supervisor change. Fare, payment and save semantics are unchanged
and untested. A fresh install is not implied by this file alone.

## Owner Slot-1 Barn ride — 2026-11-01 19:35

Dominik loaded Slot 1 himself. `Latest.log` for that process loaded TaxiDriver.dll
`012330B16B8FC6470C7BC0C299F1F43146821C63D232E807FDBD36D6E2528B42` and logged
`mode=Guard compatible=True`. Pickup halted 4.6 m from `(34.4, 0.0, 30.7)` at 0.9 km/h;
boarding was the later passenger-seat fix, after the park. Barn dispatch `op=2 order=4`
to `(165.3, 0.0, 25.1)` overrode only the empty Error direct path (`original=0
effective=Infinity override=True`, group unchanged, native Success, 115-point path ending
at the street point). Supervisor `trigger=pre-deadline dist=5.0m speed=-0.1 action=completed`,
then `ARRIVED at Barn`, fare $17.

Still seated, he selected Bungalow. The first calculation from `(161.1, 0.7, 22.4)` failed
with no road path. Recovery reversed 3.3 m and the redispatch succeeded with
`override=False`. The taxi then stalled at `(164.6, 0.8, 19.6)`; free-spot rescue found
nothing, and the ride `GAVE UP` 334.1 m from `(-156.0, -4.0, 113.7)` after three episodes.
Fare $6. This is a chained re-route from the Barn street end, not a fresh Town Hall
Bungalow and not a reopening of the Barn arrival. Docks and early boarding were not
tested. The game was closed afterward. Copied log SHA-256
`b2b6cfe51163c5d47799a6e088ca51b7ce28954b890b022328e05ce740d0a0af` (112454 bytes).
No new build, deploy, save-byte comparison, commit or push.
