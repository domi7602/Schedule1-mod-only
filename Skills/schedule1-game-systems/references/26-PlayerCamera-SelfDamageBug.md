# Camera-dependent self-damage investigation (Schedule I)

> verified: static identifier sweep 2026-10-08 against the freshly regenerated f12 decompile (`GameReferences/decompiled/Assembly-CSharp`), replacing the earlier decompile-generation check — 15 of 15 identifier-shaped tokens resolve (0 documented as absent; 0 lowercase parameter tokens are out of scope). Static coverage only; runtime behaviour still needs an in-game session.

> Evidence: inherited incident hypothesis from Skills.zip. No game session, executable method body or reproduction log was supplied with this package. The old static-token scan does not establish a root cause.

## Report and competing explanations

The original note reports self-damage while using the baseball bat and the V-key avatar view. It proposes that a camera-origin cast intersects the local player's collider. Other explanations include repeated hit callbacks, another damage source, a collider/layer configuration difference, or a mod interaction. Keep these alternatives open until a reproduction distinguishes them.

Candidate owners are `Il2CppScheduleOne.Equipping.Equippable_MeleeWeapon`, `ExecuteHit(float power)`, `PlayerCamera` and the player damage receiver. Their exact current signatures, visibility and runtime path must be resolved on the target build.

## What the previous note cannot prove

- An `il2cpp_runtime_invoke` proxy body does not expose the native hit-selection logic. A local variable name and source line number from a different export are not a valid patch target.
- Finding identifiers does not prove that a self-hit guard is missing, that every melee weapon shares the same effective path, or that explosions exclude the owner.
- A single 60-point hit against 100 health leaves 40 health in a simple subtraction model. Death would need evidence of another hit, a multiplier, another source or a different initial health value.
- The inherited bat/pan range and radius estimates are hypotheses, not measured prefab values.

## Reproduction and instrumentation

1. Use a reversible test save and record game build, backend, weapon, health, active mods and camera mode.
2. Compare bat and one other melee weapon, normal view and avatar view, with identical health and target position. Repeat with the suspected mod disabled when appropriate.
3. Record one attack identifier, actual cast origin/direction and hit collider where observable, resolved damage receiver, damage amount and every health transition. Avoid per-frame log flooding.
4. Resolve the real damage/hit boundary and inspect a readable implementation or capture runtime evidence. Verify whether damage is host-authoritative, predicted locally or broadcast before choosing a hook.
5. Locate the narrow ownership check or collision filter that preserves ordinary hits and other damage sources. Do not suppress an entire attack merely because the camera points at the player.

## Acceptance for a proposed fix

The reproduction stops harming its owner, ordinary hits still work, unrelated environmental damage remains valid, and the relevant host/client path produces one outcome. Return to the menu and reload once to detect stale references or duplicate patches. If only metadata is available, deliver a hypothesis and instrumented test plan, not a claimed one-line fix.

Related: [combat](29-Combat-Weapons.md), [player](49-Player.md), [Harmony and IL2CPP boundaries](../../schedule1-modding/references/il2cpp-harmony-guide.md).
