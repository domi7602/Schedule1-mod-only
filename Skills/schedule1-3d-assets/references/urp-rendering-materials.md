# Rendering, Materials, Textures, and Shader Ownership

> verified: static identifier sweep 2026-10-08 against the freshly regenerated f12 decompile (`GameReferences/decompiled/Assembly-CSharp`), replacing the earlier decompile-generation check — 12 of 12 identifier-shaped tokens resolve (0 documented as absent; 0 lowercase parameter tokens are out of scope). Static coverage only; runtime behaviour still needs an in-game session.

## Evidence and use

Use for pink assets, incorrect PBR appearance, runtime material changes, texture loading, and leaks.
Unity documentation was checked on 2026-10-21. Check the game's actual Unity version, render pipeline,
shader inventory, and interop bindings before selecting a call or property. Historical game identifiers
and URP assumptions do not establish that every branch, bundle, or shader variant is compatible.

## Resolve the shader before creating a material

1. Inspect a working material with the required visual behavior in the installed game.
2. Identify the active render pipeline and the shader/property contract.
3. Resolve the intended shader and check availability. `Shader.Find` can fail; finding a shader name
   alone does not establish that all required variants survived the player build.
4. Use a compatible, validated fallback only if its different appearance is acceptable.
5. If resolution fails, preserve the last working material or disable the affected visual feature
   with a bounded diagnostic. Do not call `new Material` with a null shader.

`Universal Render Pipeline/Lit`, `/Simple Lit`, and `/Unlit` are lookup candidates when the runtime
uses URP. `Standard` is not a reliable fallback for a URP material. Likewise, Unlit changes lighting
behavior and must not be substituted silently when the feature depends on lit surfaces.

Code can select existing shaders and available variants, create materials, and adjust exposed
properties. A new shader program needs a compatible shader build and asset delivery route, such as
an appropriate Unity asset build with the required variants retained. A C# mod DLL does not by itself
provide a general runtime compiler for arbitrary Unity shader source. Establish the supported game,
Unity, graphics API, and pipeline route before promising a custom WorldLook shader.

## Material ownership

| Resource relationship | Safe default | Cleanup |
|---|---|---|
| Borrowed game material | Read without mutation | Never destroy it |
| Intentionally shared mod material | Mutate only when every consumer should change | Release after its last consumer |
| Per-renderer owned material | Create/cache deliberately for isolated edits | Destroy on replacement or owner teardown |
| Property override | Use a tested `MaterialPropertyBlock` when supported | Clear/restore the owner's override on release |

`Renderer.material` can instantiate a renderer-specific material when the material is shared.
It does not necessarily create another copy on every access. The problem is uncontrolled ownership
and repeated creation/replacement. Cache deliberate instances and release them.

`Renderer.sharedMaterial` provides access to a shared object. Changing its colour or texture can
change every renderer using that object. It is not an isolation mechanism. For temporary overrides,
record and restore the previous assignment only if this owner still controls it; do not overwrite a
newer change made by another system.

A property block changes supported properties, not arbitrary render state. Unity documents that
property blocks are not compatible with the SRP Batcher; benchmark the actual workload rather than
claiming they are always the faster URP option.

## Property contract

For a verified URP Lit shader, common lookup candidates are `_BaseColor`, `_BaseMap`, `_Metallic`,
`_Smoothness`, `_BumpMap`, and `_EmissionColor`. Check property existence and meaning before setting
values; custom shaders and other pipelines can use different names or packed channels.

- Colour textures and data textures can require different colour-space handling.
- A normal-map texture, keyword state, and tangent basis must agree.
- Transparency can require surface mode, blend factors, depth-write state, render queue, and keywords;
  assigning an alpha value alone does not establish correct transparent rendering.
- Emission can make a surface appear bright without lighting nearby geometry. Light contribution
  depends on the lighting setup, renderer features, and supported GI path.
- Metallic and smoothness are visual design values. Test fabric, painted metal, and bare metal under
  the game's lighting instead of treating one example's numbers as universal defaults.

## Texture-load transaction

1. Validate the allowed path/resource identity, byte size, supported image format, and project limits.
2. Allocate a temporary owned texture with the intended colour-space/mipmap policy using the installed
   API signature. Do not overwrite the live texture reference yet.
3. Decode using the verified image API. Check its result and catch expected I/O/decode failures.
4. On false result or exception, destroy the temporary texture and retain the old usable material.
5. On success, validate dimensions and configure sampler settings; then publish the new texture.
6. Release the replaced owned texture when no renderer/material/UI consumer still needs it.

Unity's `ImageConversion.LoadImage` uploads decoded data automatically; an extra `Apply` is not
required for that operation. Constructor, format, readable-data, and managed/IL2CPP byte-array
signatures must still match the deployed runtime. Do not rely on a helper returning null while
leaking the texture allocated before the failed decode.

For generated pixel textures, batch CPU edits before `Apply`; repeated full uploads are expensive.
Drop CPU-readable data only when no later update, readback, or save operation needs it.

## Verify

Compare daylight/interior/night appearance, transparency sorting, shadows, normal-map direction,
missing-file behavior, unsupported shaders, and repeated reload memory. Count owned material and
texture instances before and after open/close, scene reload, and feature disable.

### Controlled shader comparison

WorldLook's 2026-10-23 surface-refinement check rendered the actual baseline/candidate bundles in an isolated Unity 2022.3.62f3 / URP 14.0.12 / D3D11 scene. The test-only fixture is under `Source/Mods/WorldLook/build/preview/` in this workspace; its evidence boundaries are recorded in `Source/Mods/WorldLook/docs/water-surface-verification.md`.

- Source-token tests cannot establish that a wave field looks natural or is free of visible bands. Compile the shader and inspect actual matched pixels as well as failure boundaries.
- Match fixture/bundle identities, camera, time, native-equivalent input values and pipeline settings. URP rewrites clock globals during camera setup: a fixed-time test pass before transparent rendering plus hash-identical repeated frames checks that the shader actually receives controlled time. Dry controls distinguish scene drift from water changes.
- If surface-detail normals also drive refraction or foam, a glint change can unintentionally change bottom distortion or shore appearance. Preserve or explicitly test those consumers when separating the normal fields.
- Float output checks detect observed nonfinite/error pixels, not every invalid intermediate. Fixed-frame Editor images do not measure native game texture bindings, motion shimmer, variant coverage or GPU cost. Record shader defaults separately from any values retained when cloning a native material.

## Primary sources

- [Renderer.material](https://docs.unity3d.com/ScriptReference/Renderer-material.html).
- [Renderer.sharedMaterial](https://docs.unity3d.com/ScriptReference/Renderer-sharedMaterial.html).
- [MaterialPropertyBlock](https://docs.unity3d.com/ScriptReference/MaterialPropertyBlock.html).
- [Shader.Find](https://docs.unity3d.com/ScriptReference/Shader.Find.html).
- [ImageConversion.LoadImage](https://docs.unity3d.com/ScriptReference/ImageConversion.LoadImage.html).
- [Texture2D.Apply](https://docs.unity3d.com/ScriptReference/Texture2D.Apply.html).
