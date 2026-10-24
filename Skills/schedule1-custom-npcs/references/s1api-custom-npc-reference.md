# Custom NPC API and Lifecycle Reference

> **Canonical home of the Custom-NPC API rules.** API surface checked against the in-repo S1API source on 2026-10-05. The source checkout and deployed DLL were not aligned at that time; compare them before relying on source-level claims. Current dependency selection lives in `AGENTS.md`.
> UNVERIFIED against the installed runtime. Static identifier sweep 2026-10-08 against the freshly regenerated f12 decompile (`GameReferences/decompiled/Assembly-CSharp`), replacing the earlier decompile-generation check: 82/84 identifier-shaped tokens resolve (0 documented as absent). Unresolved identifiers are listed at the end of this file. Runtime behaviour is not covered by this sweep.

## Evidence and navigation

The original workspace inspected `ThirdParty/S1API/S1API/Entities/` on 2026-10-05 and recorded a
source/deployed-DLL mismatch. Its 2026-10-08 identifier sweep was not runtime validation. This revision
checked public S1API documentation on 2026-10-21; that documentation is not a pinned 3.2.1 contract. <!-- skills-health: version-evidence (S1API target requested by the owner; installed build state lives in AGENTS.md) -->
Before implementation, compare the actual referenced DLL, source commit, and game/interop versions.
The member names below are lookup anchors unless the installed signature has been verified.

- [Prefab and instance phases](#prefab-and-instance-phases)
- [Role configuration](#role-configuration)
- [Schedule and action composition](#schedule-and-action-composition)
- [Dialogue, events, and load restoration](#dialogue-events-and-load-restoration)
- [Health compatibility](#health-compatibility)
- [Appearance and asset constraints](#appearance-and-asset-constraints)
- [Verification](#verification)

This file owns API/lifecycle guidance. [Implementation patterns](example-project-patterns.md) owns
role-level recipes. Avoid duplicating a full API recipe in both.

## Prefab and instance phases

| Phase | Responsibilities | Avoid |
|---|---|---|
| Type declaration | Stable physical/customer/dealer/supplier role declarations | Values depending on ordinary instance initialization |
| `ConfigurePrefab` | Identity, prefab components, appearance defaults, role defaults, relationship and schedule composition | Instance-dependent callbacks or side effects |
| Creation/finalization | Supported base initialization, actual appearance finalization, runtime wiring, schedule enablement | Rebuilding persistent defaults on every callback |
| Load restoration | Rebind runtime callbacks; restore derived state after the framework restores saved fields | Replaying one-time rewards or introductions |
| Final teardown | Remove the exact subscriptions and owned runtime resources | Clearing a newer instance's registration |

Public documentation explicitly treats customer/dealer/supplier declarations as type-level values.
Inspect the target runtime's allowed role combinations; the current supplier documentation requires a
physical NPC and excludes combining supplier and dealer. Do not assume older builds expose suppliers.

For a physical NPC, establish identity and valid spawn/navigation points before registration and
network spawn. For a non-physical contact, avoid unnecessary physical spawn and movement setup.
Do not use the obsolete constructor recipe merely because an older example compiles against a
different API build. A null icon is a placeholder requiring explicit product behavior, not a finished
contact icon.

## Role configuration

| Area | Inherited lookup candidates | Design and validation |
|---|---|---|
| Identity | `WithIdentity`, `WithIcon` | Stable unique ID; explicit icon; migration when an existing ID changes |
| Spawn | `WithSpawnPosition` | Verify overload, walkable position, orientation, collision and route |
| Customer | `WithCustomerDefaults` | Spending, order frequency/time, standards, affinity, dependence and unlock policy |
| Dealer | `WithDealerDefaults` | Signing fee, cut, dealer type, home, quality acceptance and completed-deal variable |
| Relationship | `WithRelationshipDefaults` | Delta, unlock mode, initial lock and valid connection IDs |
| Appearance | `WithAppearanceDefaults` | Valid asset paths and supported appearance fields |
| Schedule | `WithSchedule` | Valid points, timings, prerequisites and native action support |

Customer helper names in the inherited source include `WithSpending`, `WithOrdersPerWeek`,
`WithPreferredOrderDay`, `WithOrderTime`, `WithStandards`, `AllowDirectApproach`, `GuaranteeFirstSample`,
`WithCallPoliceChance`, `WithMutualRelationRequirement`, `WithDependence`, `WithAffinities`, and
`WithPreferredProperties`. Verify their argument types, units, accepted ranges, and fluent return
values. The old example's numbers were tuning choices, not canonical economy defaults.

Dealer lookup candidates include `WithSigningFee`, `WithCut`, `WithDealerType`, `WithHomeName`,
`AllowInsufficientQuality`, `AllowExcessQuality`, and `WithCompletedDealsVariable`. Resolve the home
and variable identifiers rather than copying an example's apartment name.

Role defaults belong in prefab configuration where the target API expects them. Runtime events such
as customer unlock/deal completion/contract assignment or dealer recruitment/contract acceptance
should drive current behavior without rebuilding those defaults. Verify whether a named callback is
an event, registration method, or delegate property before binding it.

## Schedule and action composition

Prefer supported schedule wrappers over constructing native action specs when the wrapper exposes
all required options. Inherited candidates are `WalkTo`, `StayInBuilding`, `UseVendingMachine`,
`LocationDialogue`, and `HandleDeal`. Advanced seat, parking, GUID-routing, or timing requirements may
need a supported spec; inspect its prerequisites and serialization behavior.

The old integration used `EnsureDealSignal` for active customer/dealer handling and required a positive
seat duration for `SitAtSeatSet`/`SitSpec`. Recheck these paths in the target version and exercise the
actual action; an NPC arriving at a location does not prove contract handling works.

Location actions may require matching prefab components before network registration. Historical
builder candidates are `EnsureSmokeBreak`, `EnsureGraffiti`, `EnsureDrinking`, and `EnsureItemHolding`,
paired with `LocationBased`, distance conditions, and arrival-action wrappers. Add components only
through the verified supported phase; do not patch them onto a spawned network prefab blindly.

**Do not call `NPCSchedule.InitializeActions()` from mod code.** The inherited 2026-10-05 source
inspection found it internal and framework-owned in `Entities/NPCSchedule.cs`. The old bundled example
calling it was removed. If the deployed implementation differs, document its actual supported public
initialization path; do not use reflection merely to recreate an obsolete docs recipe.

Verify the creation order for appearance finalization and `Schedule.Enable`. Avoid enabling movement
before the avatar/native instance, world geometry, and navigation dependencies are ready.

## Dialogue, events, and load restoration

Inherited dialogue anchors include `BuildAndRegisterContainer`, `AddNode`, `OnChoiceSelected`,
`JumpTo`, `UseContainerOnInteract`, and `StopOverride`. Use stable container/node/choice IDs and
validate all transitions. The old workspace found a recursion risk when stopping an override from a
node-displayed callback. Exit through a supported choice/close path and verify callback reentrancy;
do not reproduce that stop-on-display pattern.

Bind runtime events once per real instance/publisher. Cache the exact delegate and publisher, track
whether registration succeeded, and unsubscribe during final teardown. Subtract/add on the new
instance does not remove a delegate bound to an old instance. Cleanup must compare owner identity
before clearing shared registrations so an old teardown cannot erase a new NPC.

Use verified load hooks, historically `OnLoaded` and `OnResponseLoaded(Response)`, to reconnect runtime
callbacks that serialization cannot preserve. Saved message response IDs should map to idempotent
handlers. A one-time introduction, reward, or purchase needs persistent completion/operation state;
`OnCreated` can run again and is not a one-time-per-save guarantee.

The current public `SaveableField` attribute targets **fields**, not properties. Use supported field
types and stable save keys, validate schema/version transitions, and avoid the reserved field name
`QuestData` described by its documentation. Confirm the installed attribute/serializer before adding
it. Do not serialize Unity object references as a substitute for stable identity.

Runtime anchors such as `ID`, `FullName`, `Position`, `Aggressiveness`, `Region`, `SendTextMessage`,
`Goto`, and `ClearConversationCategories` need actual getter/setter and authority checks. A region
association does not necessarily mean the NPC is physically in that region. Clearing badges for a
story contact is a deliberate role decision, not a default for customer/dealer contacts.

## Health compatibility

The inherited workspace found a possible mismatch between an `IsInvincible` wrapper searching for
`Invincible`/backing-field spellings and an installed native proxy exposing `_invincible`. This is a
specific compatibility lead, not proof that every current wrapper is broken. Inspect the deployed
implementation and read back the native result after applying protection.

Apply service-NPC protection only to the exact registered instance and on the authoritative side.
If recovery of that instance is part of the feature, use the verified supported health/revive path
and read back its state before a service mutation. Do not spawn a replacement or revive an unrelated
NPC as an invisible fallback. A failed readback should stop the affected service operation clearly.

## Appearance and asset constraints

Inherited native anchors are under `GameReferences/decompiled/Assembly-CSharp/Il2CppScheduleOne/AvatarFramework/`,
including `Avatar.cs`, `AvatarSettings.cs`, `Customization/BasicAvatarSettings.cs`, `Eye.cs`,
`EyeController.cs`, and `Eyebrow.cs`. Some prior identifiers, including `ApplyBodySettings` and
`CustomizationManager`, were unresolved in the later sweep; rederive the real application path.

| Field or limit | Inherited observation | Use in new work |
|---|---|---|
| Gender | Normalized convention; male test historically below 0.5 | Use the actual asset convention and conservative 0–1 values |
| Weight | Historically converted to blendshape weight ×100 | Validate the target blendshapes; avoid unsupported extremes |
| Height | Applied as local scale without a clamp; random examples 0.8–1.2 | Measure navigation/collision/animation fit; this range is not a safety guarantee |
| Pupil/lid values | Inspector range annotations 0–1 | Respect intended range; attributes alone do not enforce runtime clamping |
| Eyebrow resting height | Historical application clamp −1.1 to 1.5 | Confirm current implementation |
| Eyebrow scale/thickness/angle | No useful hard clamp found in old path | Start from existing valid avatars and visually test |
| Face/body/accessory layers | Historical capacities 6 / 6 / 9 | Verify current constants/accessor counts before indexing |

Old fallback defaults included height 0.98, weight 0.4, gender 0, lid values 0.5, and differing pupil
values in different code paths. These observations are not universal target defaults and should not
override an existing authored appearance without a reason.

Historical resource-path examples are `Avatar/Hair/Spiky/Spiky`, `Avatar/Layers/Face/Face_Agitated`,
`Avatar/Layers/Top/T-Shirt`, `Avatar/Layers/Bottom/Jeans`, and
`Avatar/Accessories/Feet/Sneakers/Sneakers`. Resolve against the installed resource set, using the
supported resource-style path without an extension where the loader requires it. Embedded .NET
resources and Unity resources are different namespaces.

Verify colour types: a `Color32` uses byte channels while `Color` uses floating channels. Do not pass
0–255 values to a floating tint by copying a skin-colour example into a hair/eye field.

Inspect when `NPCAppearance.Build` or its current equivalent applies settings and creates a mugshot.
Call the supported finalization step at the appropriate lifecycle point; do not repeatedly rebuild
appearance on every frame or assume a mugshot proves the whole animated model is correct.

## Verification

Test registration uniqueness, host/client spawn, appearance and icon, dialogue transitions, route
completion, customer/dealer functions, health readback when used, load restoration, and saved response
callbacks. Include scene reload and save A→B→A. Confirm no duplicate messages/rewards/subscriptions and
no premature callbacks before world/navigation readiness.

## Primary sources

- [S1API NPC public API](https://ifbars.github.io/S1API/api/S1API.Entities.NPC.html).
- [S1API SaveableField public API](https://ifbars.github.io/S1API/api/S1API.Saveables.SaveableField.html).

The user-provided S1API 3.2.1 DLL was not included in this package review. Compilation and in-game <!-- skills-health: version-evidence (S1API target requested by the owner; installed build state lives in AGENTS.md) -->
behavior remain explicit implementation gates.
