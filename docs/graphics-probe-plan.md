# GraphicsProbe — read-only rendering discovery

## Approved scope

The first graphics-overhaul slice is a diagnostic DLL and a runtime report, not a
water shader or a visual overhaul. No active render pipeline, renderer asset,
camera, material, light, reflection probe, scene object or save data is changed.
Writing diagnostic files beneath the mod's UserData directory is intentional.

Runtime values take priority over community project assumptions. An installed URP
assembly is evidence that a type is available, not that URP is the active pipeline.
A material/name match identifies a **water candidate**, not a confirmed water draw.
Camera settings express requests; they do not prove a depth/opaque buffer was
produced or sampled in the rendered frame.

## Acceptance gates

### Build and deployment

- Pure policies used by production are linked into a Unity-free test host.
- Observe a failing regression before implementing its corresponding behavior.
- Run the full GraphicsProbe test suite and a compile-only Release build.
- Obtain an independent scoped review; review fixes require a delta re-review.
- Build and auto-deploy only GraphicsProbe after confirming the game is closed
  and that the existing built/installed Shared DLL hashes agree.
- Verify built/installed DLL, symbols and metadata; check installed type metadata.
- Keep the version inventory and compatibility matrix at **In-Game-Verify open**
  until an actual game session supplies evidence.

### Runtime session — pending until exercised

1. Launch through Steam, never by directly starting the game executable.
2. Confirm the deployed GraphicsProbe build loaded without initialization errors.
3. Inspect its first report for the observed active pipeline and device settings.
4. Load a save, wait for its delayed snapshot, then inspect scene coverage,
   inactive/disabled cameras, renderer counts, water candidates, lights/probes and
   environment settings. Explicit limits/errors must remain visible in the report.
5. Near the intended water area, request a manual capture using the documented
   mod controls. Confirm the report updates and the previous report is backed up.
6. Return to the menu and load again. Confirm pending scene work is coalesced or
   cancelled appropriately and no stale-object error loop appears.
7. Confirm rendering and controls appear unchanged. This is an owner observation,
   not a GPU timing result or proof that every native getter is side-effect-free.
8. Confirm shutdown leaves no deferred capture running. Removing the diagnostic
   DLL with the game closed must require no visual-state restoration.

Scene discovery must be bounded and must not poll the entire world each frame.
If a persistent/native scene cannot be enumerated safely, report that coverage
limitation instead of asserting that every game object was inspected. Captures
may cause a one-off hitch; no frametime/FPS guarantee is made by a passing unit
suite or successful compile.

## Diagnostic coverage-repair acceptance

The first 0.1.1 gameplay reports (2026-10-16) reached 50,000 nodes in only two
Main-scene roots, discovered no camera and filled the retained lexical candidate
list with particles. Repair that coverage defect before choosing a water renderer.
The original reports remain evidence of loading, file IO and active URP, not proof
that the upcoming traversal works.

- Admit roots fairly across enumerated loaded scenes before descendants; prevent
  one high-fanout parent or deep NPC hierarchy monopolizing the budget. Preserve
  inactive/disabled-object discovery and explicit finite frontier/node/depth caps.
- Advance only an active capture across bounded main-thread slices. Record wall
  duration separately from active work, the largest observed cooperative slice and
  finalization cost. A native call, serialization or IO may exceed a time target;
  unit tests cannot establish native frame-time performance.
- Cancel and release native references on scene transitions, failure and shutdown;
  discard cancelled captures without replacing the existing JSON. Test queued
  requests, retry and no idle discovery through production-used pure policies.
- Give surface-like and particle/line/trail lexical matches separate scan and record
  budgets so a particle flood cannot consume the surface allowance. Retain omitted,
  failed and scanned counts; a mesh-like renderer is still only a heuristic candidate.
- If reading one tagged camera early, label the hint and deduplicate it when reached
  by scene traversal. Do not use global camera/object arrays or infer gameplay-camera
  identity from its tag, enabled state or existence.
- Exercise starvation, breadth fairness, category saturation, slice/node/time/frontier
  bounds and cancellation/retry regressions before native compilation. Obtain scoped
  independent review and re-review changed fixes before deployment.
- Fresh native acceptance must establish the new version actually loaded, real scene
  and root coverage, camera provenance, separated candidates, report/backup behavior
  and measured capture timings. No confirmed water or usable depth/opaque buffer can
  be inferred from a lexical match or an asset's requests alone.

## Deliberately outside this slice

- Shader compilation, Unity Editor setup and embedded AssetBundles.
- Water meshes, replacement materials, reflections, refraction and depth effects.
- Camera patches, pipeline changes, renderer features and additional render views.
- Lighting/material/atmosphere overrides or distributing extracted game assets.
- RenderDoc captures, GPU pass measurements, PresentMon data and visual baselines.

The next slice is selected from the **actual report**. It must have its own
reversible integration and visual/performance acceptance criteria. A successful
diagnostic report does not itself prove the proposed water shader is feasible.
