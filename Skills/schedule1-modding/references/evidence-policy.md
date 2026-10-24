# Evidence policy

> UNVERIFIED locally: this file was authored in the external skills package (2026-10-21) and adopted here without re-running the source checks it describes. Treat its rules as procedure, not as reproduced evidence.

## Classify the claim

| Status | Required evidence | Does not establish |
|---|---|---|
| Design requirement | Explicit project/user requirement or reasoned invariant | That a particular API implements it |
| Inherited note | Source path/date preserved from the uploaded collection | That its original check was independently reproduced |
| Symbol located | Exact containing type and member found in a named artifact | Full callable signature, native logic or runtime success |
| Signature checked | Owner, visibility, static/instance form, arguments and return type against identified artifact | Correct game state, ordering or effects |
| Source inspected | Matching implementation plus relevant callers/dependencies | That deployed bytes match or a native path behaved as expected |
| Compiled | Actual command/environment and successful compiler output | Gameplay correctness or deployment |
| Runtime observed | Reproduction steps, inputs, identity/role, logs/readbacks and observed result | Universal behavior outside that tested scope |
| Failure path tested | Injected failure, expected invariant and observed recovery | Other untested failure points |

Do not treat these as a single automatic ladder: a runtime observation can be valid for one path without proving a general source claim.

## Record enough to reproduce

For a substantial finding record: claim; owner/type/member or file/section; source kind; repository commit or DLL hash; game/runtime/branch when relevant; date; exact steps; observed result; limits; and next verification. Keep the current compatibility matrix in the repository's existing canonical location. Historical evidence keeps its own immutable provenance.

## Source order by question

- For installed callable metadata, inspect the deployed or exact build-reference assembly.
- For managed behavior, read source that matches that assembly; verify the alignment rather than assuming a checkout matches.
- For native IL2CPP behavior hidden behind invoke stubs, use appropriate runtime instrumentation or actual native implementation evidence.
- For a public framework concept, consult maintainer documentation. Record whether it follows a moving branch or a release tag.
- Use curated system notes and third-party mods to locate candidates. Do not copy their assumptions into current guarantees.

## Handling conflicting evidence

State the conflict close to the affected instruction. Replace an invalid active example or remove its executable form; a distant warning does not neutralize it. Preserve useful historical evidence with explicit scope, not as a second current standard.

If source and deployed DLL disagree, determine the requested target before changing code. Do not silently upgrade the user's dependencies to make an example compile.

## Missing names and negative evidence

A repository-wide identifier sweep is a navigation aid. It can match a name on the wrong type or flag a locally declared example name as absent. Verify owner/signature. Absence from an IL2CPP proxy call graph does not prove absence from native game code.

## Output wording

Use precise statements such as “the supplied snippet maps zero stock to the unlimited marker” or “this method signature exists in the inspected assembly.” Reserve “works in game,” “fixed,” “initializes successfully” and “compatible” for the observed scope that supports them.

Report unrun tests separately from failed tests. A proposed test, a tool's success flag, or a reference's historical verified label is not a completed test.
