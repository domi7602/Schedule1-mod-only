# CustomSkateboard Mod (Schedule I)

Adds a custom high-performance skateboard (**Pro Cyber Skateboard**) with custom physics, Blender 3D model asset pipeline, dialogue injection into **Jeff Gilmore** (Downtown Skateboard Seller), anti-gravel frictionless terrain cruising, and console command support.

## Features
- **Jeff Gilmore Skateboard Shop Integration**: Injects the new skateboard seamlessly into Jeff Gilmore's dialogue shop in Downtown (`$1,500`).
- **Enhanced Physics & Tuning**:
  - Top Speed: 80 km/h (Standard ~25 km/h)
  - Push Force: x2.8 multiplier
  - Jump Force: x2.2 multiplier
  - Turn Force: x1.8 multiplier
  - Dynamic Lean: 26° max tilt
  - Longitudinal Friction: 0.35 (Smooth cruising glide)
- **Anti-Gravel & Terrain Slowdown Bypass**:
  - Disables the game's terrain / gravel drag (`SlowOnTerrain = false`, `GetSurfaceSmoothness = 1.0`, `IsOnTerrain = false`).
  - Glides across gravel, grass, dirt paths, and roads at full speed with 0 friction penalties.
- **Procedural IK Rigging**: Works with Schedule I's `SkateboardAnimation` IK system to align feet, hands, spine, and pelvis dynamically on the deck.
- **Blender 3D Asset Pipeline & OBJ Support**: Procedural Blender 5.2 Python generator script producing `.blend`, `.obj`, and `.mtl` models. Supports custom OBJ loading with automatic winding correction and caching.
- **Console Commands**:
  - `skate give` - Spawns the skateboard directly into inventory
  - `skate stats` - Shows current board parameters
  - `skate help` - Displays command list
