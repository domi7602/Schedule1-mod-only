---
name: schedule1-il2cpp-reflection
description: "Resolve IL2CPP members and verify reflection access. Keywords: Il2CppStructArray, Il2CppReferenceArray, Il2CppInterop, RuntimeReflection, byte[] overload, Texture2D.LoadImage, Sprite array, type cache, namespace fallback."
---

> Runtime and dependency details are maintained in workspace [AGENTS.md](../../AGENTS.md). Verification notes in this skill describe evidence scope; they do not imply current-runtime verification.

# Schedule I — IL2CPP Reflection

## Scope and prerequisites

Follow the [working contract](../schedule1-modding/references/working-contract.md).
Identify the backend, proxy assembly identity, and installed Il2CppInterop version.
Prefer an available, verified S1API wrapper or typed binding over reflection.
Do not infer cross-backend compatibility merely because one code branch is not executed;
assembly loading and JIT resolution can still require referenced types.

## Resolution workflow

1. Define the operation and its expected result. Distinguish missing, null, empty, and failed.
2. Locate the exact declaring type in the actual loaded or deployed assembly.
3. Match member name, static/instance status, parameter types, return type, and access mode.
4. Resolve inherited private members by walking the declaring-type chain when necessary.
5. Validate the target instance and its lifecycle generation before native access.
6. Convert arguments to the actual declared types, invoke on the required thread, and
   preserve meaningful exceptions and their inner causes.
7. For writes, read back the specific target; do not report success on a silent no-op.

## Arrays and collections

| Need | Required check |
|---|---|
| Managed array to proxy array | Inspect the destination type and available constructors/conversions |
| Image bytes to a texture | Resolve the exact `ImageConversion.LoadImage` overload and its result |
| Collection iteration | Inspect supported enumeration, `Count`/`Length`, and indexer surface |
| Private sprite-array assignment | Verify member mutability, element type, ownership, and conversion |

Do not state that all IL2CPP collections lack managed enumeration, or that one reflected
`Count`/`Item` recipe works for every array and list. Use the installed wrapper's contract.
A missing length member is an unsupported surface, not an empty collection.
Bound traversal and detect invalid or changing sizes before indexing.
Cache verified method metadata rather than scanning every assembly for every loaded image.

## Member lookup and cache policy

- Prefer full type names and the expected assembly. A namespace fallback is an explicit
  compatibility list, not permission to select an unrelated type with a matching short name.
- Record why each fallback is supported; validate its signature and behavior separately.
- Cache successful metadata by runtime type and full member signature.
- Decide when negative lookups expire if assemblies can appear later.
- Handle ambiguous/indexed properties and read-only fields explicitly.
- Getter invocation can have side effects. Read named, justified members; avoid sweeping
  every property merely to produce a diagnostic dump.
- Reflection does not bypass native object lifetime or thread restrictions.

## Failure handling

Return an explicit unsupported/read-failed/write-failed result with target context.
Do not convert lookup failure to zero, false, an empty list, or apparent success.
If a required API is absent, stop the feature or select a verified alternative.
For optional integration, isolate dependency loading rather than relying on a catch around
code whose types require the missing assembly before that catch can run.
Generated IL2CPP proxy bodies expose bindings, not the native implementation's control flow.
A missing managed invocation site cannot establish that a native event never fires.

## Verification and delivery

Verify successful access, missing and ambiguous members, expected null results, stale
instances, scene changes, empty collections, and each supported backend independently.
For writes, verify exact readback and restoration; for images, verify load failure cleanup.
Report the observed signature, assembly identity, chosen conversion, and unsupported cases.
Do not claim a recipe is compiled or runtime verified without the actual corresponding test.

## References and read triggers

- [S1API catalog](../schedule1-s1api/SKILL.md): before adding raw interop.
- [Knowledge navigation](../schedule1-knowledge/SKILL.md): locating deployed signatures.
- [Harmony bootstrap](../schedule1-harmony-bootstrap/SKILL.md): reflection for patch targets.
- [Runtime cache](../schedule1-runtime-unity-cache/SKILL.md): ownership after creating assets.
- [Il2CppInterop upstream](https://github.com/BepInEx/Il2CppInterop): locate the exact installed source revision; this link is not version evidence.
