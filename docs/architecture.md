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
