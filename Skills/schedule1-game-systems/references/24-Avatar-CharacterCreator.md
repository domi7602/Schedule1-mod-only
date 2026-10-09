# Avatar/Character Creator (Schedule I)
> UNVERIFIED against the installed runtime. Static identifier sweep 2026-10-08 against the freshly regenerated f12 decompile (`GameReferences/decompiled/Assembly-CSharp`), replacing the earlier decompile-generation check: 5/7 identifier-shaped tokens resolve (1 documented as absent). Unresolved identifiers are listed at the end of this file. Runtime behaviour is not covered by this sweep.


## Character Creator (Game Start)
- 7 Categories: Body → Hair → Face → Eyes → Eyebrows → Clothing → Accessories

### Customization Options
| Category | Properties |
|----------|------------|
| Body | Gender (Male/Female), Weight, SkinColor (20 colors) |
| Hair | HairStyle (AssetPath), HairColor, FacialHair |
| Face | Mouth, FacialDetails + Intensity |
| Eyes | EyeballColor, UpperEyelidRestingPosition, LowerEyelidRestingPosition, PupilDilation |
| Eyebrows | Scale, Thickness, RestingHeight, RestingAngle |
| Clothing | Top, Bottom, Shoes (each AssetPath + ColorPicker) |
| Accessories | Headwear, Eyewear (AssetPath + ColorPicker) |

## Avatar System (Modular)
- **Single mesh body** with blend shapes: Gender (0-100), Weight (0-100), Feet_Shrink
- **BodyLayers** (6 slots): Texture + Color + Normal, sorted by Order
- **FaceLayers** (6 slots): FaceTexture + FacialHair + Tattoos + Details
- **Accessories** (max 9): Separate GameObjects attached to body bones
- **Hair**: `Hair` inherits from `Accessory`, can be suppressed by hats
- **Eyes**: `EyeController` controlling pupil size and eyelid positions
- **Eyebrows**: Scale, Thickness, RestingHeight, RestingAngle

## In-Game Customization
- **Clothing**: 10 clothing slots (Feet, Bottom, Waist, Top, Outerwear, Hands, Neck, Eyes, Head, Wrist) via inventory
- **Barber / Hair Salon**: Modify hair style + hair color (for a fee)
- **Tattoo Parlor**: Purchase face/body layer tattoos
- **NO mirror, NO phone app** for full physical customization

## Dev/NPC Editor (`CustomizationManager`)
- Unity Editor only (`if (DevOnly && !Application.isEditor)`)
- `.asset` files stored under `Assets/Data/Appearances/`
- Used by TVGS to author NPCs without requiring Blender

## Clothing Colors (27 Colors)
White, LightGrey, DarkGrey, Charcoal, Black, LightRed, Red, Crimson, Orange, Tan, Brown, Coral, Beige, Yellow, Lime, LightGreen, DarkGreen, Cyan, SkyBlue, Blue, DeepBlue, Navy, DeepPurple, Purple, Magenta, BrightPink, HotPink

---

---

---

---

---

---

## Unresolved identifiers (f12 static check 2026-10-08)

These documented identifiers were not found in the f12 game assemblies, the checked-in S1API/S1MAPI source, or the workspace source. Treat them as drift candidates and re-derive them from the current decompiles before relying on this document.

- `CustomizationManager`
