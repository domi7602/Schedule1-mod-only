using System;
using System.Collections.Generic;

namespace NorthIsland;

/// <summary>
/// Game-free layout for the Northshore Hideout purchase, sign and build grid.
/// Coordinates match the accepted Phase 1 warehouse in <c>TownBuilder</c>.
/// </summary>
internal static class HideoutPropertyPlan
{
    internal const string Code = "northshore_hideout";
    internal const string Name = "Northshore Hideout";
    internal const string ChoiceLabel = Code;
    internal const float Price = 20000f;
    internal static string PriceLabel => "$" + Price.ToString("N0", System.Globalization.CultureInfo.InvariantCulture);
    internal const int EmployeeCapacity = 1;
    internal const int Schema = 1;

    internal const float CenterX = -98f;
    internal const float CenterY = 8.08f;
    internal const float CenterZ = 330f;
    internal const float WidthX = 14f;
    internal const float LengthZ = 18f;
    internal const float AntiTunnel = 1.40f;
    internal const float DoorWidth = 5f;
    internal const float FloorTopY = 8.18f;
    internal const float Inset = 1.55f;

    // Pole foot in the open, east of the wall and north of the ramp.
    // The Barn plants the sign root 0.70 m above the ground; the pole mesh hangs to the soil.
    internal const float SignX = -88.2f;
    internal const float SignY = 8.08f;
    internal const float SignZ = 334.8f;
    internal const float SignRootLift = 0.70f;

    // Door approach, clear of the post so a reload does not drop the player into the sign.
    internal const float SpawnX = -89.4f;
    internal const float SpawnZ = 331.2f;

    internal static float ExteriorMinX => CenterX - WidthX * 0.5f;
    internal static float ExteriorMaxX => CenterX + WidthX * 0.5f;
    internal static float ExteriorMinZ => CenterZ - LengthZ * 0.5f;
    internal static float ExteriorMaxZ => CenterZ + LengthZ * 0.5f;

    internal static float InteriorMinX => ExteriorMinX + Inset;
    internal static float InteriorMaxX => ExteriorMaxX - Inset;
    internal static float InteriorMinZ => ExteriorMinZ + Inset;
    internal static float InteriorMaxZ => ExteriorMaxZ - Inset;

    internal static float DoorMinZ => CenterZ - DoorWidth * 0.5f;
    internal static float DoorMaxZ => CenterZ + DoorWidth * 0.5f;

    internal static bool ContainsInterior(float x, float z) =>
        x >= InteriorMinX && x <= InteriorMaxX && z >= InteriorMinZ && z <= InteriorMaxZ;

    internal static IReadOnlyList<HideoutTile> Tiles(float tileSize)
    {
        if (tileSize < 0.2f || tileSize > 1f)
            return Array.Empty<HideoutTile>();

        var tiles = new List<HideoutTile>();
        float firstX = AlignCenter(InteriorMinX, tileSize);
        float firstZ = AlignCenter(InteriorMinZ, tileSize);
        int ix = 0;
        for (float x = firstX; x <= InteriorMaxX + 0.001f; x += tileSize, ix++)
        {
            int iz = 0;
            for (float z = firstZ; z <= InteriorMaxZ + 0.001f; z += tileSize, iz++)
            {
                if (!ContainsInterior(x, z))
                    continue;
                tiles.Add(new HideoutTile(ix, iz, x, z));
            }
        }

        return tiles;
    }

    private static float AlignCenter(float min, float size)
    {
        float n = MathF.Ceiling((min - size * 0.5f) / size);
        return n * size + size * 0.5f;
    }
}

internal readonly record struct HideoutTile(int X, int Z, float WorldX, float WorldZ);
