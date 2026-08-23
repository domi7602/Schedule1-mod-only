# Graffiti (Schedule I)

## Spraying
- Item: `spraypaint` (IntegerItemInstance, 1 can per graffiti)
- Equip `Equippable_SprayBottle` → interact with `SpraySurface`
- `SpraySurfaceInteraction.Open()`: camera aligned to wall, FOV 70, unlocks mouse cursor

## Editor
- Freehand painting with mouse
- **8 colors**: Black, White, Red, Green, Blue, Yellow, Pink, Brown
- **4 brush sizes**: 10, 16, 24, 32 pixels
- **Undo**: 10 steps via History-TextureArray
- **Clear**: clear entire graffiti
- Pixel limit: 25,000 total, 1,000 per stroke
- **No text or image import**

## Technical Implementation
- `DecalProjector` (URP) on `SpraySurface`
- Texture: RGBA32, default 450x300 (upscaled to 512x512)
- 1 pixel = 0.006666671 world units
- Render: CPU-based (no compute shader active)

## Cartel Graffiti
- `SprayGraffiti` (CartelActivity): region → unoccupied surface → spawns CartelGoon
- Duration: 120-180 minutes of game time
- Cartel templates (`SerializedGraffitiDrawing`) loaded randomly
- `ContainsCartelGraffiti = true` (cannot be sprayed over, must be cleaned off)

## Removal / Rewards
| Action | XP | Influence |
|--------|----|-----------|
| Complete custom graffiti | 50 | -0.05 |
| Remove cartel graffiti | 25 | -0.05 |
| Interrupt cartel graffiti (attack NPC) | 50 | -0.1 |

## Achievement
- `URBAN_ARTIST`: 25 distinct `WorldSpraySurface` objects with `HasEverBeenMarkedByPlayer = true`

## NPC Reactions
- `EVisualState.Vandalizing` → police detects vandalism
- `NPCResponses_Police.NoticedVandalism()` → `CrimeData.AddCrime(new Vandalism())`

## Persistence
- `GraffitiData` → `WorldSpraySurfaceData[]` with strokes
- Network: `QueueSurfaceToReplicate()` prioritized by stroke count
- Per surface: `HasEverBeenMarkedByPlayer` + `DrawingStrokeCount`
