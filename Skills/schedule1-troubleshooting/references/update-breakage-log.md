# Update Breakage Notes and Reverification

> verified: compiled 2026-10-05 from the cited evidence files; no new in-game tests were run for this log.
> verified: static identifier sweep 2026-10-08 against the freshly regenerated f12 decompile (`GameReferences/decompiled/Assembly-CSharp`), replacing the earlier decompile-generation check — 18 of 18 identifier-shaped tokens resolve (0 documented as absent; 0 lowercase parameter tokens are out of scope). Static coverage only; runtime behaviour still needs an in-game session.

> Evidence scope: historical observations inherited from the uploaded skills. The supporting
> original logs, decompiles, and repository documents are not bundled. This file is a list
> of targeted checks, not a compatibility release matrix or new confirmation of those cases.

## Historical leads

| Historical lead | What to inspect after an update | Limit |
|---|---|---|
| Removed `DialogueHandler` accessor | Exact declaring type, patch code references, and dependent callers | Do not assume it is absent in every game branch |
| `OnSaveInfoLoaded` not observed in a 2026-09-29 trace | Current subscription, callback order, required readiness | Zero observations do not prove a globally dead event |
| Assumed `OnSaveLoaded` name | Actual S1API lifecycle event surface | Do not manufacture an equivalent event |
| Source/deployed-framework mismatch | Source commit, DLL identity/hash, loaded dependency | Source checkout alone is not the running framework |
| Native crash after class-initialization warning | Crash diagnostics, interop compatibility, minimal reproduction | No universal DLL-size/class-count threshold established |
| `DefaultStackLimit` accessor patch rejected | Current proxy/native binding and supported alternative | A similarly named property is not automatically patchable |
| Appearance or NPC object-model change | Current data types, migration behavior, ragdoll/appearance call paths | Historical compatibility notes are not current declarations |
| Branch-specific save conversion | Actual migration direction and backup/recovery contract | Preserve the original before any destructive conversion test |

## Reverification workflow

1. Preserve the installed artifact manifest, logs, and test-save copy before changing inputs.
2. Identify the actual new game/backend/framework versions and matched proxy generation.
3. Regenerate references only through the workspace's inspected tooling when authorized.
4. Build affected projects without deployment using the current build contract.
5. Check each required anchor by declaring type and full signature, not global token counts.
6. Verify patch installation and invocation in a controlled session.
7. Reproduce gameplay, save/load, teardown, and relevant host/client behavior.
8. Record per-feature results and unresolved checks; do not update a verification date alone.

When optional static tools are missing, use available build/source checks and report the gap.
Do not automatically roll back a game version or overwrite a save from a `.bak` merely
because a version changed. Establish compatibility and the exact backup identity first.

## Add a new record

Record observed date, game/backend, dependency identities, artifact hashes, reproduction,
old/new behavior, concrete evidence path, affected scope, and whether the result is static,
build-only, or runtime-tested. Mark hypotheses separately from confirmed changes.
A future documentation edit must update examples and quick-reference tables with the record.

## Related references

- [Troubleshooting workflow](../SKILL.md)
- [Common error families](common-errors.md)
- [Lifecycle verification](../../schedule1-lifecycle-verify/SKILL.md)
- [Build/deploy contract](../../schedule1-modding/references/build-and-deploy.md)
