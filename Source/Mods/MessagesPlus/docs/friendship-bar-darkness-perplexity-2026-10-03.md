# Research pack: friendship status bar too dark in dark theme (MessagesPlus)

Answer ONLY the four numbered questions at the end, from general Unity / uGUI /
IL2CPP / C# knowledge. Do not invent behavior for the game-owned classes listed
under DO-NOT-GUESS - mark anything that would require them as unverifiable.

## Shared context

- Language/runtime: C# 12, .NET 6 mod assembly, Unity 2022.3 (IL2CPP game),
  MelonLoader 0.7.3, Harmony patches. UI is legacy uGUI (UnityEngine.UI),
  NOT UI Toolkit.
- The mod recolours the game's vanilla phone UI into a dark theme at runtime
  (colour writes only). Palette values used below are floats 0..1:
  Bg (0.043,0.055,0.078), Header (0.067,0.086,0.133), Card (0.082,0.114,0.165),
  CardAlt (0.110,0.149,0.220), TextMuted (0.545,0.580,0.620).
- DO-NOT-GUESS (game-owned IL2CPP types the mod only calls):
  Il2CppScheduleOne.Messaging.MSGConversation (fields used: `slider` (Slider),
  `sliderFill` (Image), `_bubbles`, `responseContainer`),
  Il2CppScheduleOne.UI.Phone.Messages.MessagesApp (`currentConversation`,
  `dialoguePage`, `ActiveConversations`),
  Il2CppScheduleOne.Messaging.MessageBubble.
- Time semantics: throttle/heartbeat code uses `Time.unscaledTime` (scaled
  `Time.time` is affected by the game's speed setting and is not used).
- IL2CPP interop note: property reads on game objects can throw; all access is
  wrapped in per-check try/catch. `GetInstanceID()` and `.Pointer` are both used
  as identity keys.

## Finding 1: the friendship bar renders dark-on-dark after theming

The dialogue header of the vanilla messages app contains a friendship/status
bar implemented as a standard UnityEngine.UI.Slider. Under the slider root the
game has exactly three images (from our runtime dump, verbatim names): the game
painted a five-segment relationship spectrum into the Fill, a marker (Handle),
and a Background/track. Mod policy deliberately keeps this bar vanilla because
it displays state (relationship level). Measured screenshot pixels of the bar
row (8-bit sRGB, decoded from PNG): five equal-width segments
(10,1,3), (16,10,5), (11,17,26), (2,14,34), (3,22,5) left to right, thin gaps
between some segments showing the surface behind at (21,29,41), header band
around it (17,22,33). The last two palette values equal our themed Card and
Header colours, so the SURFACES around the bar are our dark theme as intended,
while the segment pixels sit far below the surface in brightness. Hue order of
the segments (red, orange, neutral, blue, green) matches a relationship
spectrum (bad to good). User report: the bar is barely visible without zoom in
the dark theme; goal is to brighten the bar while keeping the state readable.

Runtime log evidence (one-time dump per conversation, logged at the FIRST
theme pass after the conversation opens, verbatim):

```
FriendshipBar: name='Background' w=650.0 colour=(1.00,1.00,1.00) a=1.00 rendererAlpha=1.00 groupAlpha=1.00 isFillRef=False
FriendshipBar: name='Fill' w=650.0 colour=(1.00,1.00,1.00) a=1.00 rendererAlpha=1.00 groupAlpha=1.00 isFillRef=True
FriendshipBar: name='Handle' w=20.0 colour=(1.00,1.00,1.00) a=1.00 rendererAlpha=1.00 groupAlpha=1.00 isFillRef=False
```

`colour` is `Image.color`, `rendererAlpha` is `CanvasRenderer.GetAlpha()` on
that image, `groupAlpha` is the nearest ancestor `CanvasGroup.alpha`. All three
images report white, fully opaque at dump time. The Fill's rect width equals
the Background's (650 canvas units) so the fill may span the full track.

Verbatim code excerpts from the mod (Source/Mods/MessagesPlus/src/AppTheme.cs,
line numbers match the working tree at pack time).

A) AppTheme.cs:668-684 - policy and slider resolution (called once per theme
pass while a conversation is open):

```csharp
    /// <summary>
    /// The chat header's friendship slider. Its images are never themed (state
    /// display); a track that would blend dark-on-dark is lifted to a readable
    /// grey while the game-coloured fill stays exactly as the game set it.
    /// Uses the conversation's own slider reference - the name/type guard can
    /// miss the bar depending on the prefab's object names (2026-10-03).
    /// </summary>
    private static void TintFriendshipBar(MSGConversation conv)
    {
        Slider? slider = null;
        try { slider = conv.slider; } catch { /* skip */ }
        if (slider == null || !NetworkGuard.IsAlive(slider))
        {
            return; // keep the last known root - a missing ref must not open the guard
        }

        try { _friendshipSliderRoot = slider.transform; } catch { /* keep previous root */ }
```

B) AppTheme.cs:724-739 - only the resolved track may be tinted, and only when
its Image.color is dark (the dump above shows (1,1,1), so this never fires for
the observed bar):

```csharp
            // Only the resolved track is themed - everything else stays
            // vanilla so the standing reads truthfully.
            if (track == null) continue;
            bool isTrack = false;
            try { isTrack = img.Pointer == track.Pointer; } catch { continue; }
            if (!isTrack) continue;

            Color c;
            try { c = img.color; } catch { continue; }
            if (c.a <= 0.02f) continue;
            if (MaxChannel(c) >= 0.35f) continue; // already readable on the dark bar

            // Track would blend into the dark header - theme it (tracked), so
            // the verify pass keeps it readable instead of fighting it.
            TintGraphic(img, GamePalette.TextMuted);
```

C) AppTheme.cs:883 and 1128-1148 - every generic sweep skips slider subtrees:

```csharp
                if (IsUnderSlider(img) || IsInFriendshipBar(img.transform)) continue; // identity/state sliders stay vanilla (friendship bar)
```

```csharp
    /// <summary>
    /// True when the image belongs to a Slider (e.g. the dialogue header's
    /// friendship bar). Sliders carry state - their track/fill/handle stay
    /// vanilla so the level stays readable on the dark surfaces (2026-10-03).
    /// </summary>
    private static bool IsUnderSlider(Image img)
    {
        try { if (img.GetComponentInParent<Slider>() != null) return true; } catch { /* fall through */ }

        try
        {
            Transform? t = img.transform;
            for (int i = 0; i < 4 && t != null; i++)
            {
                if (t.name.ToLowerInvariant().Contains("slider")) return true;
                t = t.parent;
            }
        }
        catch { /* fall through */ }

        return false;
    }
```

D) AppTheme.cs:1215-1237 - the mod's own tint helper, which already treats a
ColorTint Selectable specially (graphic white + palette in the ColorBlock),
because uGUI multiplies the two:

```csharp
    /// <summary>
    /// Applies the dark RGB (the graphic's alpha stays live) and darkens the
    /// button transitions when this graphic is the target of a colour-tint
    /// Selectable. uGUI MULTIPLIES the graphic colour with the state colour, so
    /// in that case the graphic is set white and the palette is carried entirely
    /// in the ColorBlock. Returns the colour actually written to the graphic.
    /// </summary>
    private static bool SetDark(Graphic graphic, Color target, out Color applied)
    {
        applied = target;
        try
        {
            Selectable? sel = graphic.GetComponent<Selectable>();
            if (sel != null
                && sel.transition == Selectable.Transition.ColorTint
                && sel.targetGraphic != null
                && sel.targetGraphic.Pointer == graphic.Pointer)
            {
                applied = Color.white;
                ApplyDarkTransitions(sel, target);
                LightenDarkIconsIn(graphic.gameObject);
            }
        }
        catch { /* transitions are polish, never fatal */ }
```

Unverifiable / game-specific (do NOT answer these): the actual sprite texture
texels of Background/Fill/Handle (dark jewel tones vs bright colours), whether
the game writes `sliderFill.color` or `slider.value` dynamically at runtime,
the prefab object names, and whether the Fill uses type=Filled or a sliced
sprite. An earlier build of this mod briefly ran aggressive tint rules
app-wide and visibly darkened this bar; the current build contains the
exclusions above, and the quoted dump + screenshot are both from the current
build.

## Finding 2: proposed fix under review

Goal: brighten ONLY the bar (Background, Fill, Handle) in the dark theme,
hue-preserving (red segment must stay red, green must stay green; the level and
marker position must stay exactly where the game puts them). Candidate
mechanisms:

- (a) a white Image overlay with alpha k above the bar (result = lerp(pixel,
  white, k)),
- (b) runtime sprite replacement: copy sprite texels via RenderTexture blit +
  ReadPixels (works for non-readable textures), lift RGB (gamma or scaling),
  assign as `Image.overrideSprite`,
- (c) same as (b) but with per-sprite max-channel normalization (scale all RGB
  by target / sprite max channel) instead of gamma,
- (d) insurance item: if the Slider's ColorBlock normal colour is dark (stale
  write from an earlier pass), reset the block toward white so any state-colour
  multiply stops crushing the bar.

## Questions (answer only these)

1. Inventory: with `Image.color = (1,1,1)`, `CanvasRenderer.GetAlpha() = 1`,
   and the nearest `CanvasGroup.alpha = 1` at measurement time, which uGUI
   mechanisms can still render that Image's pixels at RGB around 2..35 of 255?
   Rank the candidates (sprite texels themselves; `CanvasRenderer.SetColor`
   driven by Selectable.ColorTint via `CrossFadeColor`; material/shader tint;
   fillRect clipping revealing a dark backdrop; ancestor CanvasGroups beyond
   the nearest one; anything else). Also: does a stock UnityEngine.UI.Slider
   itself ever write colours onto fillRect/handleRect/targetGraphic during
   `UpdateVisualState`/`UpdateDrag`, or does it only move transforms?

2. Discrimination probe: design the minimal set of runtime log lines (values to
   read, once per bar) that in ONE in-game round distinguishes (i) dark sprite
   texels, (ii) dark state-colour multiply on the canvas renderer, (iii) a
   persisted tint write from the mod's own tint helper. Which properties to
   read (`canvasRenderer.GetColor()`, `Image.type`, `fillAmount`, sprite
   texture size/sampled texels via a temporary RenderTexture,
   `Selectable.colors`, `selectable.targetGraphic`), in what order, and which
   IL2CPP interop pitfalls to avoid while reading them?

3. Brightening quality for near-black jewel-tone art (segment RGB 2..34 of 255,
   distinct hues, flat colours with anti-aliased edges and alpha): compare (a)
   white alpha overlay, (b) gamma lift on copied texels, (c) max-channel
   normalization on copied texels, for hue preservation AND within-bar contrast
   (segment vs segment, filled vs empty track) on a near-black background.
   Show explicitly where (a) fails numerically for a pixel like (10,1,3).
   Recommend one, with the target brightness formula you would use.

4. Sprite replacement mechanics: when swapping `Image.overrideSprite` at
   runtime for an Image that may be type=Filled (horizontal fillAmount driven
   by the Slider) or type=Sliced, what must the replacement Sprite preserve
   (rect, textureRect offsets, pivot, pixelsPerUnit, border, mesh type) for the
   slider geometry to stay identical? What goes wrong when the source sprite is
   packed into a texture atlas and one blits `sprite.texture` wholesale
   (offsets, filtering, alpha fringe)? Is there a lower-risk alternative that
   brightens via `Image.material` with an additive-style UI shader, and what
   does it cost in an IL2CPP mod that ships no shader assets?
