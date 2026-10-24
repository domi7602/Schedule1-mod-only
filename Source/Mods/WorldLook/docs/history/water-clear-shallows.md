# Clear Shallows transparency candidate

## Owner's reference target

The two supplied Minecraft water images are visual direction, not an engine/shader
compatibility claim. The first shows submerged bank/ground detail through the near
water, a blue depth transition and surface scenery/sky reflections. The second is
darker in low sun with softer surface waves and glare. The immediate requested
scope is seeing a little into shallow water, not reproducing the entire lighting,
world, reflection system or underwater-camera renderer from those images.

The owner's preceding Schedule I screenshot showed dark, visually flat water and
a strong near-white solar streak. Version 0.1.2 was confirmed active in its session
log but was not accepted as a convincing realism overhaul. Its prior performance
repair remains separately owner-confirmed on 0.1.1.

## Observed contract

See [material-evidence.md](water-material-evidence.md) for exact installed asset record
provenance. The selected material references the native Stylized Water 2/Standard
(Tessellation) shader, not an embedded custom shader.

- `_ShallowColor` and `_BaseColor` (Deep) are declared RGBA properties. Installed
  shallow and deep alpha are both 1. The shader's declared shallow default alpha
  is approximately 0.02; the forward pass uses SrcAlpha/OneMinusSrcAlpha blending.
  These prove a transparency-capable material contract, not final pixel opacity.
- `_DepthVertical`, labeled View Depth: declared range 0.01–16, default 4; material 7.
- `_DepthHorizontal`, labeled Vertical Height Depth: range 0.01–8, default 1;
  material 0.01. Names and labels must not be assigned physical units by guesswork.
- `_RefractionStrength`: range 0–1, default 0.1; material approximately 0.474.
- `_RefractionChromaticAberration`: range 0–1, default/material 1.
- Refraction is enabled by the existing valid `_REFRACTION` keyword. The material
  has `_DisableDepthTexture=0`; `_DISABLE_DEPTH_TEX` is not enabled. `_DEPTHEXP_ON`
  and `_DEPTH_TEX` are invalid saved leftovers, not valid feature controls.
- `_ZWrite=1` is inherited. Do not disable it or change sorting/blend/pipeline state
  merely to lower the shallow color's alpha.

Parent independently decoded the declaration/default/range records for the five
shallow/depth/refraction properties above. Native shader source/fog math has not
been recovered, so neither depth parameter's density direction nor a visibility
in meters is established.

## Candidate, not a visual guarantee

| Property | Clear Shallows candidate | Reason |
|---|---|---|
| `_ShallowColor` | RGB (0.025, 0.055, 0.060), alpha **0.15** | Explicit transparency request supersedes old preserve-alpha requirement; RGB unchanged |
| `_DepthVertical` | **4** | Restore declared shader default, not a claimed metric depth |
| `_DepthHorizontal` | **1** | Restore declared shader default rather than extreme material value 0.01 |
| `_RefractionStrength` | **0.10** | Restore declared default to reduce distortion |
| `_RefractionChromaticAberration` | **0** | Remove color-channel separation for a clearer view |

Retain all five prior wave/normal/foam values, native Deep RGBA, absorption, depth
exponent, textures, reflection and other keyword state, ZWrite and tessellation.
Run recipe writes once on the owned clone, gated by HasProperty. Preserve the
cached exact lookup, WaterSession ownership and deadline gates byte-for-byte.

## Camera and world limitations

The available GraphicsProbe report requests depth and opaque textures on the
active URP asset, but its latest capture has no confirmed active player camera.
Raw camera backing fields are not resolved pipeline-effective settings. The
report does not prove usable depth/opaque buffers in the current rendered view.

The shader author's SW2 setup documentation requires a depth texture for depth
features and an opaque texture when refraction is enabled; its troubleshooting
page describes gray/nontransparent output if the opaque input is unavailable.
Transparent objects are not in URP's opaque texture. This candidate does not patch
camera/pipeline settings or promise refraction of every object.

Likewise, underwater ground geometry has not been verified at the owner's chosen
shoreline. A material cannot reveal a bottom that is absent, unrendered or far
below the view. No new seabed, coral, terrain, mesh or collision is part of this
candidate. If visual acceptance fails, inspect the exact shoreline/camera/inputs
before adding more guessed constants.

Primary author references:
- [SW2 Getting Started](https://staggart.xyz/unity/stylized-water-2/sws-2-docs/?section=getting-started-8)
- [SW2 Troubleshooting](https://staggart.xyz/unity/stylized-water-2/sws-2-docs/?section=troubleshooting-10)
- [SW2 Vertex Colors](https://staggart.xyz/unity/stylized-water-2/sws-2-docs/?section=vertex-colors-2)

## Session follow-up — activation confirmed, transparency not demonstrated

The matching 0.1.3 session loaded DLL `91aa5eec…`, logged all ten supported
parameters including shallow alpha 0.15, and reached Applied (target discovery
0.648 ms). It later logged Stopped on deinitialization; this is not an independent
readback of restored render state.

The owner's follow-up screenshot at Day 3, Thursday 07:28 still shows a mostly
opaque-looking brown/gray surface with fine ripple detail. No unambiguous submerged
bottom/stone contour is visible through the water. The foreground rocks alone do
not prove underwater visibility. Compared with the earlier 08:13 view, camera and
lighting differ, so tint/glare differences are not an A/B result. Do not mark the
transparency target accepted or infer that changing alpha had no effect anywhere.
Before another recipe adjustment, inspect actual camera scene-color/depth inputs,
shader blend/depth response and rendered submerged geometry at the test location.

## Acceptance — In-Game-Verify open

Later MCP-connected follow-up: the owner reports no stutters after the startup phase
in the currently loaded 0.1.3 world. This is scoped owner-confirmed smooth play, not
quantitative timing or attribution of prior hitches. The owner explicitly retains
the reference-look goal; see [Reference-driven target](water-reference-target.md). The
visual transparency/reflection target remains unmet, not accepted by this report.

Subsequent owner report: microstutters have returned during the water work while
the matching 0.1.3 release is installed/was loaded. The earlier 0.1.1 smooth-play
acceptance is historical, not acceptance of current performance. The preserved
latest session has no WaterShader fault/Exception/ERROR matches and no repeated
water application. GraphicsProbe also auto-captured during the Main load: collection
wall 22390.750 ms, cumulative active work 1027.810 ms, maximum observed step
18.043 ms, finalization 6.650 ms. These are diagnostic-work measurements and a
plausible hitch contributor, not a measured attribution of the user's stutters or
whole-frame/GPU times. Its last completion was 14:48:31.626. Game was closed when
the report was investigated; live MCP inspection awaits a user-started session.
Do not add another global scan or change material constants to guess at this issue.

Version 0.1.3 deployed after independent release review PASS. Parent reran 1,077
tests, compiled Release with zero warnings/errors and verified installed DLL/PDB/
metadata against release inputs. Installed DLL SHA-256: `91aa5eecc8b173b17248a47245ce5e5878eee49c6f39ac938c4bd9c066517c31`.
This is installation evidence only, not a rendered transparency result.

Build/tests/review/deployment establish only the candidate implementation. In one
fresh Steam-launched session, check the `clear-shallows applied` and `Applied`
breadcrumbs, then view a shallow bank looking downward away from solar glare.
Look for actual submerged scene detail (not only a brighter water tint). Check a
low viewing angle and deeper water to ensure the ocean does not appear uniformly
invisible, and verify normal play remains smooth. Keep screenshots from comparable
viewpoints/light. Actual ground visibility, pixel blend behavior, buffer inputs,
reflection fidelity and frame times remain unverified until exercised. Menu/reload
and shutdown are separate lifecycle acceptance scopes.
