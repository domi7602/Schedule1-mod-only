# Research map for additional S1API systems

> verified: static identifier sweep 2026-10-08 against the freshly regenerated f12 decompile (`GameReferences/decompiled/Assembly-CSharp`), replacing the earlier decompile-generation check — 17 of 17 identifier-shaped tokens resolve (0 documented as absent; 0 lowercase parameter tokens are out of scope). Static coverage only; runtime behaviour still needs an in-game session.

Inherited source notes; not revalidated against the installed game in this package.
This page provides lookup targets and contracts to establish, not callable example
signatures. Check the [official API index](https://ifbars.github.io/S1API/) and the
exact target source/assembly before implementing a member.

## Choose the system and boundary

| Task | Search concepts | Contract to establish |
|---|---|---|
| Open or lock a door | Door, access, ownership, interaction | State mutation versus interaction animation, authority, persistence |
| Work with buildings/interiors | Building, property, entrance | Spatial identity, ownership, lifecycle, interior transition |
| Control vehicles | Vehicle, driver, occupant, navigation | Server ownership, driver/seat state, physics timing, path readiness |
| Observe or change growing | Pot, plant, growth, harvest | Definition versus instance, elapsed time, accepted output, inventory capacity |
| Operate stations | Station, recipe, task, configuration | Valid operating state, reserved inputs, output ownership |
| Handle dead drops/deliveries | DeadDrop, delivery, contract | Actor identity, contents, completion, duplicate processing |
| Add cartel or casino behavior | Cartel, region, casino, minigame | Progression and transaction ownership; supported extension points |
| Change weather/temperature | Weather, temperature, environment | Host authority, transitions, native visual ownership, save behavior |
| Change XP or level | Leveling, XP, reward | Per-player authority, caps, one-time reward state |
| Show dialogue or messages | Dialogue, messaging, contact | Correct NPC/contact identity, loaded conversation, callback restoration |

Use [the game-system index](../../schedule1-game-systems/SKILL.md) to select the
relevant native-system analysis. Use [world expansion](../../schedule1-world-expansion/SKILL.md)
for connected areas and [S1MAPI](../../schedule1-s1mapi/SKILL.md) for supported map
construction. A concept existing in the game does not guarantee a wrapper manager
with a predictable `Get...ByID` or `Set...` method.

## Verification workflow

1. Identify the target branch, assembly, namespace, and actual owner type.
2. Find the intended member and inspect visibility, overload, value semantics,
   readiness, authority, and effects. Check decompiled bodies only where they contain
   real managed implementation rather than IL2CPP forwarding thunks.
3. Trace current production callsites or framework examples from the same version.
4. Wrap the required behavior in the mod's own domain boundary without exposing
   runtime-specific types unnecessarily.
5. Compile the real callsites and test the success, absent-object, transition,
   duplicate-action, and relevant network failure paths.

## Console and optional integrations

For a console command, inspect the target's actual base class, registration
mechanism, constructor requirements, and argument/return contract. Do not assume
an attribute or automatic discovery rule from another framework.
Keep optional integrations such as a third-party terminal behind a verified
availability boundary. Their local helper names are not S1API APIs.

## When wrappers do not meet the requirement

Check current wrapper coverage before deciding it is absent or too slow. Measure
hot-path cost if performance is the reason to bypass it. Isolate direct native
access in a clearly owned branch adapter; preserve lifecycle and authority checks.
Publish shared helpers under the mod's own namespace, not a fabricated
`S1API.Internal.*` namespace that could collide with the framework.

Report the exact unsupported boundary and remaining verification instead of
filling the gap with a guessed method name.
