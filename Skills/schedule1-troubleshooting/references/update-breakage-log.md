# Update Breakage Log - What changed in the game or API?

> verified: compiled 2026-10-05 from the cited evidence files; no new in-game tests were run for this log.
> verified: static identifier sweep 2026-10-08 against the freshly regenerated f12 decompile (`GameReferences/decompiled/Assembly-CSharp`), replacing the earlier decompile-generation check — 18 of 18 identifier-shaped tokens resolve (0 documented as absent; 0 lowercase parameter tokens are out of scope). Static coverage only; runtime behaviour still needs an in-game session.

This is an evidence-based record of **observed** mod breakage and compatibility risks, not a release matrix. Confirmed rows cite evidence and an observation date. Expected rows come from [`docs/compatibility.md`](../../../docs/compatibility.md) and are review triggers, not established facts. Check current assemblies before applying any historical observation to the installed runtime.

## Confirmed breakages

| Change or symptom | Observed impact | Evidence | Severity |
|---|---|---|---|
| `DialogueHandler.get_activeDialogue()` was removed | A patch targeting a method whose body references the removed accessor can fail during patch installation with `MissingMethodException`. Verify the target and its callers against current decompiles. | `references/common-errors.md` section 1 | high |
| `OnSaveInfoLoaded` callback behavior | In the instrumented load recorded 2026-09-29, two subscribed mods observed **0 firings** through boot, save-menu open, and load. Prefer `OnPreLoad` / `OnLoadComplete` for the documented reset/refresh pattern, but verify current runtime behavior before treating the observation as universal. | `../schedule1-lifecycle-verify/SKILL.md` section 7 | high |
| `OnSaveLoaded` API assumption | The checked-in `GameLifecycle` source exposes six events and does not define `OnSaveLoaded`; recipes using that name are invalid for the checked source. | `../schedule1-lifecycle-verify/SKILL.md` section 7 | medium |
| Framework/runtime assembly mismatch | A framework binary that lacks the game's renamed members can cause missing-method failures during startup. Keep the source checkout and deployed DLL aligned; canonical dependency selection lives in `AGENTS.md` / `docs/compatibility.md`. | `docs/compatibility.md` (toolchain and known drift notes) | medium |
| Excessive mod class volume | A very large mod can exhaust IL2CPPInterop class initialization signatures, fall back to a substitute, then crash natively. | `SKILL.md` section 8; `references/common-errors.md` section 4 | critical |
| Field-accessor patch target | `BaseItemDefinition.get_DefaultStackLimit` is a field accessor that cannot be Harmony-patched; the attempted patch was removed and the limit is resolved by engine scanning instead. | `../schedule1-items/references/stacklimit-engine.md`; `SKILL.md` section 8 | medium |

## Expected risks (not yet confirmed)

| Change or risk | Potential impact | Evidence | Severity if real |
|---|---|---|---|
| Avatar object-model refactor (`AvatarSettings` to `AvatarObjects` / `Outfit`) | May affect NPC portraits and visual patches. | `docs/compatibility.md` (known drift risks) | medium |
| NPC ragdoll / `NPCHealth` changes | Existing mitigation was exercised in one earlier session; other mods touching NPC death/ragdoll remain unchecked. | `docs/compatibility.md`; verification matrix | medium |
| One-way appearance-save conversion | Back up saves before switching game branches; this is a data-loss risk, not a code breakage. | `docs/compatibility.md` (known drift risks) | high (data loss) |

## Update-response runbook

Full runbook: [`schedule1-modding`](../../schedule1-modding/SKILL.md) section 5. Short form:

1. Launch the game once so MelonLoader regenerates `MelonLoader/Il2CppAssemblies/`; stale proxies prove nothing.
2. Run `pwsh Tools/bootstrap-game-references.ps1` to regenerate local decompiles.
3. Run `dotnet build -c Release -p:S1NoDeploy=true` as an API-drift check.
4. Run the available pre-flight/static analysis; if `s1interop` is unavailable, use the documented build-and-test substitute.
5. For each patched class, inspect it with `ilspycmd` against the regenerated proxies and compare signatures.
6. Check mod verification dates in `AGENTS.md` section 2 / `docs/compatibility.md`; re-verify affected mods against the installed runtime.
7. Wrap risky patches in `PatchGuard` (`S1Mods.Shared`) for graceful degradation.

After an update pass, add evidence-backed observations here. Keep unconfirmed risks explicitly marked until tested.
