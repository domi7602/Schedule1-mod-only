# Architecture

## Dependency Direction

```text
Mods -> Shared -> S1API / game assemblies
Mods -> optional S1MAPI / game assemblies
```

- A mod may reference `S1Mods.Shared`, S1API, S1MAPI, and installed game assemblies.
- A mod must not reference another mod. Reusable code belongs in `Source/Mods/Shared/`.
- `ThirdParty/` is not a general source dependency. Only documented APIs or explicitly linked compatibility shims may be used.
- `GameReferences/` is generated research output. It must never be a build dependency.

## Mod Layout

```text
Source/Mods/<ModName>/
  src/       Project and production code
  docs/      mod.json, README, CHANGELOG
  assets/    Runtime assets copied by Directory.Build.targets when present
  tests/     Optional isolated tests; integration tests live in Source/Tests/
```

`Directory.Build.props` owns shared compiler settings and game references. Individual project files contain only identity and genuinely mod-specific inputs.

## Runtime Boundaries

- Persistence is slot-aware and uses `SafeStorage` plus `SaveSlots`.
- Multiplayer mutations must use `NetworkGuard` authority checks.
- Harmony patches use `PatchGuard` and must tolerate game-version signature drift.
- IL2CPP `MonoBehaviour` types require the public `IntPtr` constructor.

## AutoPackagingStation native prefab composition

AutoPackagingStation clones the complete native packaging prefab to retain its serialized station/network graph, and transplants only footprint geometry from an owned cauldron clone before any live placement. It does not add a native `PackagingStation` after spawn. Templates remain active-self beneath an inactive persistent cache, reused across registration retries and released on mod deinitialization.

S1API 3.2.1's public buildable builder exposes neither a BuiltItem setter nor pre-publication ghost registration. The mod's narrowly scoped reflection compatibility bridge therefore uses `BuildableItemDefinitionBuilder.BuildableDefinition` (private getter) and `S1API.Internal.Building.BuildableGhostRuntime.RegisterVisual(string, Func<Transform, GameObject>, bool)` (internal static). These signatures were checked in the installed S1API DLL with ilspycmd. Missing signatures abort registration before publishing; the mod does not compile against or link internal S1API source. Prefer a public pre-publication API when one is available. Physical placement/setup, native canvas and processing were exercised in the v0.4.7 session on 2026-10-15, followed by the owner's confirmation that everything tested worked. FishNet multiplayer initialization remains unverified; source signatures and component counts do not prove it.

Placed setup retains native collider geometry instead of adding or resizing a full-model root collider. The 2026-10-15 session on deployed v0.4.6 confirmed native E/canvas interaction on two stations after removing the placed root-collider manipulation. Version 0.4.7 removes the temporary read-only Awake observer, interaction/collider dumps and diagnostic bookkeeping; it does not add listeners or call Awake. The v0.4.6 evidence remains historical. The separate v0.4.7 log matches the installed DLL hash and establishes native setup/canvas/Pack/Unpack/mode mirroring; the owner then confirmed the tested gameplay session worked. Additional checks were not itemized; separate multiplayer and legacy migration coverage remain unverified. Source/policy tests are not runtime evidence.
