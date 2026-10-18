# Reference-driven water target

## Current owner feedback

In the current MCP-connected 0.1.3 session the owner reports no stutters after the
startup phase, then explicitly redirects the work to the supplied visual references.
This is owner-confirmed smooth play for that observed scope, not GPU/frametime or
startup-capture attribution evidence. The reference appearance is still not delivered.

## Observable visual acceptance

| Scope | Daylight reference | Low-sun reference | Required result in Schedule I |
|---|---|---|---|
| Shallow view | Submerged ground and bank steps remain recognizable through near water | Less bottom visibility under darker light | Actual existing submerged detail, not a brighter opaque tint |
| Depth transition | Cyan/blue-green shallows transition into deeper blue | Deep water is darker without becoming a flat gray/brown sheet | Depth-dependent attenuation, not camera-distance coloring |
| Reflections | Recognizable shore/trees/sky are reflected and distorted by waves | Shore silhouettes and sun reflection remain visible | Real scene/sky reflection input, not a constant Fresnel color |
| Surface | Fine ripples break up reflections without a uniform foam blanket | Soft waves and a controlled light streak | Consistent animated detail and credible highlights |
| View angle | Downward view reveals bottom; grazing view reflects more | Low-angle reflection dominates | Independent transmission/reflection balance with angle |

The first image is the primary clear-water target. The second constrains low-sun
behavior; it is not proof of underwater visibility. These describe water appearance,
not a request to reproduce Minecraft terrain, vegetation, clouds or its exact shader
implementation. A pixel-identical cross-engine result is not promised.

## Confirmed implementation gap

Current 0.1.3 applies ten material parameters to the game's existing shader. It does
not add a reflection producer, verify usable scene-depth/color buffers or establish
submerged geometry at the test location. A property recipe, activation breadcrumb
and passing unit tests cannot close the visual acceptance above.

The parked original custom shader also falls short even independently of its bundle
loading problem:

- `unity/Assets/S1Mods/Water.shader:144–148` uses camera-to-surface distance as a
  depth proxy. It does not sample scene depth or calculate water thickness above
  the submerged surface.
- Lines 162–164 blend a constant Fresnel sky tint. No scene-reflection or sky
  texture is sampled there, so this cannot reproduce recognizable bank/tree
  reflections from the reference.

Do not revive that shader unchanged and present it as the reference implementation.
The currently active native Stylized Water 2 shader may have a richer feature surface;
its capability is not disproved by the limited current recipe. Choose native feature
integration versus an owned shader from tested inputs, not from the mod's name.

## Authoring lead, not a resolved compiler diagnosis

The inspected authoring project's GraphicsSettings asset has
`m_CustomRenderPipeline: {fileID: 0}`. This deserves a fresh build/stripping audit
before reusing the earlier claim that the editor universally fails to compile fragment
programs. GraphicsSettings alone does not establish the quality override, active
editor pipeline, shader stripping cause or final bundle support. No editor rebuild
or live replacement was performed in this follow-up.

## Scope gate for the next implementation

A reference-oriented prototype needs actual depth/transmission and reflection inputs,
reversible ownership and a before/after visual comparison. Adding reflection rendering
is a deliberate expansion from the preceding material-only releases. Establish that
scope before changing the live render graph. Keep the accepted cached discovery and
avoid diagnostic scans while evaluating appearance. Do not trade away smooth play
silently or call the full look complete after one isolated optical effect works.
