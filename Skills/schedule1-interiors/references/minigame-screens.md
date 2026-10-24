# In-World Screens, Minigame Focus, and Spatial Audio

> verified: static identifier sweep 2026-10-08 against the freshly regenerated f12 decompile (`GameReferences/decompiled/Assembly-CSharp`), replacing the earlier decompile-generation check — 3 of 3 identifier-shaped tokens resolve (0 documented as absent; 0 lowercase parameter tokens are out of scope). Static coverage only; runtime behaviour still needs an in-game session.

## Evidence and use

Use for arcade cabinets, terminals, and small generated textures on world props. Unity texture-upload
semantics were checked on 2026-10-21. The original 60-FPS example was a design target, not a measured
performance guarantee. Verify the target runtime's texture and input/interop APIs before implementation.

## Separate simulation and presentation

Keep game state and simulation independent of Unity texture objects. Choose a timestep/pause policy
and bound catch-up work after a stall. Render from a coherent simulation snapshot. For a shared game,
define host authority and replication separately from each client's visual display.

Allocate one bounded pixel buffer and one owned texture per display or intentional shared display
set. Validate positive dimensions and checked width × height before allocating. Clip drawing
operations safely, including rectangle endpoint overflow, and use the correct pixel origin/UV
orientation. Reuse buffers; do not allocate a fresh array for every frame.

## Upload budget

Batch drawing in memory, then use the installed bulk pixel API, historically `SetPixels32`, followed
by the required upload. Unity's `Texture2D.Apply` uploads texture data and can be expensive even when
mipmap regeneration is disabled. `Apply(false)` does not make an upload free or prove 60 FPS.

Track whether the display changed. Upload at a measured cadence and suspend or reduce work for
hidden, distant, unfocused, paused, or inactive cabinets according to the feature's behavior. If the
texture remains mutable, do not discard its CPU-readable backing data prematurely. Profile the
intended number of simultaneous screens, not only one small prototype.

Point filtering and clamp wrapping are useful for crisp pixel art when the visual design calls for
it. Pick texture size, mipmap policy, and sampling for actual viewing distance; these are not universal
settings for every video or terminal screen.

## Screen material and resource owner

Resolve a shader supported by the active render pipeline before constructing a material. An Unlit
shader can keep the surface readable in darkness; it does not automatically cast light onto the
cabinet or nearby room. Emission and scene-light contribution need a separate verified lighting path.

Assign the texture through the shader's actual property contract and retain material ownership.
Do not mutate a shared native material to attach one cabinet's unique framebuffer. On disposal,
stop updates and input, detach/restore the display assignment if still owned, then destroy the owned
material/texture after their last consumer is gone. Borrowed mesh/material assets remain borrowed.

## Focus transaction

Resolve the intended cabinet and active player, then acquire the supported UI/input ownership for
that cabinet. Capture the relevant previous camera/selection state; do not invent a global movement
field or force it true on exit. Route controls only while the cabinet owns focus and no higher-priority
modal has taken it.

Use current bindings and controller support. Process exit once, and cancel on Escape/back, focus
loss, menu interruption, cabinet disable/destruction, player replacement, and scene unload. Release
only this owner's restriction, restore still-valid prior state, and prevent the same input from also
activating gameplay. Raw A/D polling from an old sample is not a complete focus implementation.

## Spatial audio lifecycle

Resolve only intended local/embedded audio resources and validate path, byte budget, supported format,
and decode result. Associate asynchronous loads with an owner generation/cancellation token so a late
result cannot resurrect a destroyed cabinet. Dispose request/stream resources and destroy unused
owned clips on failure, cancellation, replacement, and teardown through the verified API.

Configure spatial blend, attenuation/range, looping, and volume for the intended room. Bound concurrent
sources and avoid starting another loop on every focus/open callback. Pause/stop according to the game
state and feature policy. An invalid audio file should disable that sound with a bounded diagnostic,
not break the room's exit or leave a load coroutine running forever.

## Verification

Measure simulation and upload time, allocation rate, and texture/material/audio counts across repeated
focus cycles and scene reloads. Test multiple screens, pause/time-scale changes, focus loss, controller
input, missing/corrupt audio, cancelled loads, destroyed cabinets, and input restoration. Report actual
frame timing and hardware/runtime context instead of an unmeasured FPS claim.

## Primary sources

- [Unity Texture2D.Apply](https://docs.unity3d.com/ScriptReference/Texture2D.Apply.html).
- [Unity Renderer.material](https://docs.unity3d.com/ScriptReference/Renderer-material.html).
- [Unity Object.Destroy](https://docs.unity3d.com/ScriptReference/Object.Destroy.html).

Read [Rendering and materials](../../schedule1-3d-assets/references/urp-rendering-materials.md) and
[Input ownership](../../schedule1-phoneapp/references/input-focus-and-controls.md) for shared contracts.
