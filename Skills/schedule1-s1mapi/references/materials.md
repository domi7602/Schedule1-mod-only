# S1MAPI — Materials and Rendering Ownership

> verified: static identifier sweep 2026-10-08 against the freshly regenerated f12 decompile (`GameReferences/decompiled/Assembly-CSharp`), replacing the earlier decompile-generation check — 14 of 14 identifier-shaped tokens resolve (0 documented as absent; 0 lowercase parameter tokens are out of scope). Static coverage only; runtime behaviour still needs an in-game session.

## Evidence boundary

The official S1MAPI API overview was consulted on 2026-10-21. It places material helpers in `S1MAPI.Utils` and demonstrates named helpers such as `Opaque`, `Transparent`, `Glass`, `Metal`, and `Emissive`, then passes the resulting material to `SetMaterial`. This is public documentation evidence, not proof that the deployed DLL exposes the same members.

The original package's invented enum-style preset recipes and builder-specific preset setters are removed. Resolve exact helpers and parameters from the target assembly.

## Choose an owner before choosing a color

| Material source | Allowed use | Cleanup responsibility |
|---|---|---|
| Borrowed native/shared material | Read/reuse without mutation unless coordinated ownership is established | Never destroy the borrowed asset |
| Explicitly cloned material for one renderer | Modify only the owned copy | Destroy the copy after its final consumer releases it |
| Mod-owned shared material | Share among objects with intentionally identical settings | A central owner releases it after all consumers |
| Per-renderer property override | Use only supported properties and preserve existing overrides | Restore/remove only this feature's overrides |

Use [the canonical rendering reference](../../schedule1-3d-assets/references/urp-rendering-materials.md) for the Unity `material`/`sharedMaterial` distinction. Do not turn a read-only native material into global mutable state to avoid making a copy.

## Resolve a shader and preset

1. Determine the target render pipeline and inspect a compatible existing material.
2. Verify the chosen S1MAPI helper, its shader resolution, required keywords, and returned ownership.
3. Confirm the shader exists and is supported before constructing a material.
4. Apply only properties supported by that shader. Check the actual material result instead of assuming a preset name implies a feature.
5. If no supported shader is available, keep a known-good appearance or disable that visual with a diagnostic. Do not fall through to a known incompatible shader.
6. Cache a reusable material by the actual visual parameters, including shader/texture/render state, and bound that cache.

## Property checklist

The following are common URP Lit lookup keys; verify each with the selected shader:

| Property | Purpose | Check |
|---|---|---|
| `_BaseColor`, `_BaseMap` | Tint and albedo | Correct color space and UVs |
| `_Metallic`, `_Smoothness` | Surface response | Texture channel convention and shader variant |
| `_BumpMap` | Normal texture | Linear data and normal-map keyword |
| `_EmissionColor` | Emission | Required keyword and scene/postprocessing behavior |

Alpha alone is not a complete transparency setup. Match surface mode, blending, depth writing, keywords, and render queue to the verified shader/helper. A preset called glass does not prove physical refraction or a particular performance cost.

## Application and replacement

Build the replacement material before changing renderers. Save only the original references that this feature actually replaces, track the target owner/generation, and restore conditionally so another mod's later assignment is not overwritten. On replacement failure, release the failed candidate and retain the last usable material.

A material does not automatically own every texture assigned to it. Track generated textures separately and release shared resources after their final consumer. Use the runtime cache skill for repeated reloads.

## Performance and verification

Reuse stable materials where appropriate, but test changes under the actual render pipeline. Unity documents that MaterialPropertyBlock can bypass SRP batching, so do not prescribe it as an unconditional optimization. Compare allocation counts, material instances, batching, and visible results before/after.

Test independent colors on two objects sharing the original material, repeated restyles/reloads, missing shader, transparency sorting, emission at day/night, and cleanup with surviving consumers.

## Sources

- [S1MAPI API overview](https://ifbars.github.io/S1MAPI/docs/api-overview.html) — public helper catalog; inspect the deployed member contracts.
- [Unity MaterialPropertyBlock](https://docs.unity3d.com/ScriptReference/MaterialPropertyBlock.html) — property overrides and SRP batching caveat.
- [Unity material ownership](../../schedule1-3d-assets/references/urp-rendering-materials.md).
