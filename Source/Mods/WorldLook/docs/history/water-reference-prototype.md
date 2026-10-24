# Reference water prototype — approved implementation slice

The owner explicitly approved moving beyond material-only tuning after reviewing
[the reference target](water-reference-target.md). Schedule I was closed by the owner and
confirmed absent before authoring/build work. The installed 0.1.3 DLL/PDB/metadata
are hash-checked under `.backups/WaterShader/0.1.3-before-reference/`.

## Gates in execution order

1. Prove the authored URP shader compiles with fragment output and survives an
   AssetBundle roundtrip. Audit active authoring pipeline and stripping; do not
   repeat the old universal compiler-failure claim without reproducing it.
2. Implement actual scene-depth thickness and opaque-scene-color transmission,
   animated surface detail and a sampled scene-reflection input. The prior fixed
   Fresnel sky tint and camera-distance depth proxy are not accepted substitutes.
3. Bind the renderer reversibly, with bounded owned reflection resources and an
   immediate comparison/disable path. Run executable policy/math/lifetime tests,
   compile against installed IL2CPP/URP references and obtain independent review.
4. Build/deploy only the exact reviewed WaterShader stand with the game closed;
   compare DLL/PDB/metadata/bundle hashes. Actual shader support, reflection input,
   visible underwater geometry and final appearance need a new real-game session.

A failed authoring gate blocks the new runtime deployment. Do not replace the
working native shader with an unsupported shader or fabricate a visual preview.

## Rendering scope

- Keep the evidenced exact Map/ocean discovery and WaterSession ownership/scheduler
  byte-identical where possible. No global or recurring scene/renderer scans.
- No pipeline migration, fake seabed, copied/extracted game assets, vegetation,
  clouds, world/terrain changes or indiscriminate lighting overrides.
- One self-owned shader/bundle, one owned disabled reflection camera and one bounded
  reflection RenderTexture are the intended topology. No per-camera growing cache.
- Use the proven active local gameplay camera. Reject recursive/reflection, overlay,
  editor and unsupported contexts. Do not infer the gameplay camera from a truncated
  discovery report or configure every camera in the world.
- Use the existing depth/opaque input requests when available. Do not claim a request
  proves usable buffers. Unsupported shader/inputs must fail closed to native water.
- A reflection render may temporarily change global culling and suppress the owned
  water draw. Save exact prior state, publish restoration ownership before mutation,
  and restore in finally; never leave gameplay cameras, native materials or global
  render state altered when a call fails.
- Keep reflection work explicitly lower-resolution/bounded and distinguish its CPU/
  GPU cost from the removed renderer search. A bounded texture is not a millisecond
  guarantee. Actual image quality and frame times remain acceptance gates.

## Teardown and comparison

Quiesce callbacks before releasing resources. Detach only a material still owned by
the prototype, restore the inherited native slot, destroy only owned resources and
unsubscribe exact event/delegate pairs. Scene changes, callback exceptions, partial
initialization and repeated shutdown must not retain a live render callback, destroy
another writer's material or destroy a still-bound shader resource. Expose a guarded
comparison control without colliding with GoldenHour F7 or GraphicsProbe F8 chords;
verify availability before choosing its key. Re-enabling must not leak cameras,
textures or material instances.

## Verification boundaries

The existing 1,077 tests passed before this slice, and Shared build/install hashes
match. Their old material-recipe coverage is not proof of this new shader or render
path. Pure tests are appropriate for reflection math, resource/scheduling policies,
state restoration and callback lifetime; only real rendering can validate buffers,
clipping, supported shader programs, reflected objects, underwater detail and the
reference appearance. The owner-confirmed smooth 0.1.3 session remains historical.
