using System;
using System.Collections.Generic;

namespace TownHallInterior;

// World coordinates, derived from the scene inventory. These boxes are owned by the mod,
// never edits to shared meshes. This plan is not evidence of player traversal.
//
// Replacement-candidate manifest (serialized level1 identities, NOT runtime handles):
// Root Map/Hyland Point/Region_Downtown/TownCenter/TownHall (GO 62727).
// - townhall/Walls GO 69766: BoxCollider 405121 spans x54.35..63.35,
//   y1.25..8.25, z20.50..41.50; MeshRenderer 302660, static batch 84/1.
// - Cube GO 28103: BoxCollider 409405 spans x54.40..63.30,
//   y0.65..9.65, z20.60..41.40; MeshRenderer 284066, batch 127/1.
// - townhall/UpperBrickTrim GO 50074: BoxCollider 425454 spans x54.30..63.40,
//   y4.60..4.90, z20.45..41.55; MeshRenderer 321047, batch 752/1. Its
//   collider fills the entire opening.
// - townhall/LowerBrickTrim GO 62216: BoxCollider 415744 spans x54.30..63.40,
//   y1.10..1.40, z20.45..41.55; MeshRenderer 272608, batch 621/1. Its
//   collider raises the ground by .15.
// - townhall/Door/Quad GO 5947: MeshCollider 399088 spans the entrance at
//   x54.192, y1.092..3.950, z30.039..31.961; renderer 320925 batch 447/1.
//   Door renderer 326533 batch 269/1 and Glass 315785 batch 71/1 are separate.
// These are candidates for component-specific replacement, NOT an authorization to
// disable them. Foundation 418467, roof, windows, door frame and StaticDoor/IntObj
// trigger 413831 must be preserved. Runtime paths, transforms, mesh contributions,
// enabled states, door behavior and any additional blockers must first be reconciled.
// In particular removing Walls/Cube/trim renderers may remove exterior surfaces: no
// activation until an exterior-preserving visual replacement is demonstrated. Never
// alter a shared combined mesh; snapshot exact owned component states and restore only
// one's own changes on failure/unload, with identity/ownership checks. This plan performs
// no native mutation and the room sink intentionally offers no publication method.
public readonly record struct RoomBox(string Name, float X, float Y, float Z, float Width, float Height, float Depth, bool Solid, int Style);

public static class RoomPlan
{
    public const float Ground = 1.25f, Upper = 4.75f, Slab = 0.25f;
    public const float StairRise = 0.175f, StairTread = 0.28f;
    public static IReadOnlyList<RoomBox> Create()
    {
        var b = new List<RoomBox>();
        void Box(string name, float x1, float y1, float z1, float x2, float y2, float z2, bool solid, int style)
        {
            if (!(x2 > x1 && y2 > y1 && z2 > z1)) throw new ArgumentException(name);
            b.Add(new RoomBox(name, (x1 + x2) / 2, (y1 + y2) / 2, (z1 + z2) / 2, x2 - x1, y2 - y1, z2 - z1, solid, style));
        }
        // The slab is split around the stair void. Ground support is the native foundation.
        Box("upper-west", 54.55f, 4.5f, 20.7f, 60.7f, Upper, 41.3f, true, 0);
        Box("upper-south", 60.7f, 4.5f, 20.7f, 63.15f, Upper, 24f, true, 0);
        Box("upper-north", 60.7f, 4.5f, 29.6f, 63.15f, Upper, 41.3f, true, 0);
        Box("upper-east", 62.1f, 4.5f, 24f, 63.15f, Upper, 29.6f, true, 0);
        // Match the visible portal even when UnityRoomSink hides the wall renderer.
        Box("west-south", 54.35f, Ground, 20.5f, 54.55f, 8.25f, 30.04f, true, 1);
        Box("west-north", 54.35f, Ground, 31.96f, 54.55f, 8.25f, 41.5f, true, 1);
        Box("west-lintel", 54.35f, PublicEntrance.PortalTop, 30.04f, 54.55f, 8.25f, 31.96f, true, 1);
        Box("east", 63.15f, Ground, 20.5f, 63.35f, 8.25f, 41.5f, true, 1);
        Box("south", 54.55f, Ground, 20.5f, 63.15f, 8.25f, 20.7f, true, 1);
        Box("north", 54.55f, Ground, 41.3f, 63.15f, 8.25f, 41.5f, true, 1);
        for (int i = 0; i < 20; i++)
            Box("step-" + i, 60.7f, Ground, 24f + i * StairTread, 62.1f, Ground + (i + 1) * StairRise, 24f + (i + 1) * StairTread, true, 0);
        // Guards are outside the opening; the north end remains the upper landing.
        Box("guard-west", 60.6f, Upper, 24f, 60.7f, 5.75f, 29.6f, true, 2);
        Box("guard-east", 62.1f, Upper, 24f, 62.2f, 5.75f, 29.6f, true, 2);
        Box("guard-south", 60.7f, Upper, 23.9f, 62.1f, 5.75f, 24f, true, 2);
        // Furniture is a separately staged vanilla-geometry layer, not solid placeholder boxes.
        return b;
    }
}

// Walkable tile centers for the two hall floors. Not a Property and not a save record.
public readonly record struct HallGridRect(float MinX, float MinZ, float MaxX, float MaxZ);

public static class HallGridPlan
{
    public const float TileSize = 0.5f;
    public const string GroundName = "TownHallInterior-grid-eg";
    public const string UpperName = "TownHallInterior-grid-og";

    // Public floor, inset so a tile does not sit inside the wall colliders.
    public static readonly HallGridRect Ground = new(55.15f, 21.75f, 62.25f, 40.25f);

    // Upper slabs from RoomPlan, inset at the outer walls. The stair void is excluded below.
    public static readonly HallGridRect[] Upper =
    {
        new(54.70f, 20.85f, 60.65f, 41.15f),
        new(60.70f, 20.85f, 63.00f, 23.85f),
        new(60.70f, 29.75f, 63.00f, 41.15f),
        new(62.15f, 24.00f, 63.00f, 29.60f),
    };

    public static readonly HallGridRect StairVoid = new(60.70f, 24.00f, 62.10f, 29.60f);

    public static IReadOnlyList<(int X, int Z, float WorldX, float WorldZ)> Tiles(
        IReadOnlyList<HallGridRect> floors, HallGridRect? blocked, float size)
    {
        if (size <= 0f) throw new ArgumentOutOfRangeException(nameof(size));
        var tiles = new List<(int, int, float, float)>();
        var seen = new HashSet<(int, int)>();
        foreach (var floor in floors)
        {
            int x0 = (int)Math.Ceiling((floor.MinX + size * 0.5f) / size);
            int x1 = (int)Math.Floor((floor.MaxX - size * 0.5f) / size);
            int z0 = (int)Math.Ceiling((floor.MinZ + size * 0.5f) / size);
            int z1 = (int)Math.Floor((floor.MaxZ - size * 0.5f) / size);
            for (int ix = x0; ix <= x1; ix++)
                for (int iz = z0; iz <= z1; iz++)
                {
                    float x = ix * size, z = iz * size;
                    if (blocked is HallGridRect block && Overlaps(x, z, size, block)) continue;
                    if (!seen.Add((ix, iz))) continue;
                    tiles.Add((ix, iz, x, z));
                }
        }
        return tiles;
    }

    public static bool Overlaps(float x, float z, float size, HallGridRect block)
    {
        float half = size * 0.5f;
        return x + half > block.MinX && x - half < block.MaxX
            && z + half > block.MinZ && z - half < block.MaxZ;
    }
}
