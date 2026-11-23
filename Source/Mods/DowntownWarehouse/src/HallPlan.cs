using System;
using System.Collections.Generic;

namespace DowntownWarehouse;

/// <summary>
/// Game-free layout for the hall behind Handy Hank's Hardware.
/// Shell identity and bounds were confirmed by the in-game probe on 2026-11-17
/// (UserData/DowntownWarehouse/hall-probe.txt): native renderer
/// HardwardStore/hardwarestore/Main, collider center (115.75,4.1,25.5), size 14x8x18.
/// </summary>
internal static class HallPlan
{
    internal const string Code = "downtown_warehouse";
    internal const string Name = "Downtown Warehouse";
    internal const string ChoiceLabel = Code;
    internal const float Price = 45000f;
    internal static string PriceLabel => "$" + Price.ToString("N0", System.Globalization.CultureInfo.InvariantCulture);
    internal const int EmployeeCapacity = 10;
    internal const int Schema = 1;
    internal const string GridGuidString = "a1b2c3d4-1111-4a2b-9c3d-000000000001";

    internal const string StoreRootPath = "Map/Hyland Point/Region_Downtown/HardwardStore";
    internal const string HallMainPath = StoreRootPath + "/hardwarestore/Main";
    internal const string BlueDoorPath = StoreRootPath + "/hardwarestore/DoorFrame (1)/Industrial Metal Door";
    internal const string WhiteboardPath = "Map/Hyland Point/Region_Downtown/RE Office/Interior/Whiteboard";

    // Confirmed native shell.
    internal const float CenterX = 115.75f;
    internal const float CenterY = 4.1f;
    internal const float CenterZ = 25.5f;
    internal const float WidthX = 14f;
    internal const float HeightY = 8f;
    internal const float LengthZ = 18f;
    internal const float FloorTopY = 0.1f;
    internal const float EaveY = 6.10f;
    internal const float RidgeY = CenterY + HeightY * 0.5f;
    internal const float CeilingGridY = 4.80f;
    internal const float GridTile = 0.5f;
    internal const float WallSurfaceInset = 0.125f;
    internal const float WeatherInsetXZ = 0.04f;
    internal const float WeatherCenterX = CenterX;
    internal const float WeatherCenterY = CenterY;
    internal const float WeatherCenterZ = CenterZ;
    internal const float WeatherSizeX = WidthX - WeatherInsetXZ * 2f;
    internal const float WeatherSizeY = HeightY;
    internal const float WeatherSizeZ = LengthZ - WeatherInsetXZ * 2f;

    // Buildable floor inset right up to the 0.12 m collision walls (13 cm clearance per side).
    internal const float Inset = 0.13f;
    internal const float InteriorMinX = CenterX - WidthX * 0.5f + Inset;   // 108.88
    internal const float InteriorMaxX = CenterX + WidthX * 0.5f - Inset;   // 122.62
    internal const float InteriorMinZ = CenterZ - LengthZ * 0.5f + Inset;  // 16.63
    internal const float InteriorMaxZ = CenterZ + LengthZ * 0.5f - Inset;  // 34.37

    // Approach in front of the blue door inside Hank's shop, and a safe interior stand.
    internal const float SpawnX = 107.4f;
    internal const float SpawnZ = 22.5f;
    internal const float InteriorSpawnX = CenterX;
    internal const float InteriorSpawnZ = CenterZ;
    internal const float NpcSpawnX = 108.0f;
    internal const float NpcSpawnZ = 22.5f;

    internal static bool ContainsInterior(float x, float z) =>
        x >= InteriorMinX && x <= InteriorMaxX && z >= InteriorMinZ && z <= InteriorMaxZ;

    internal static bool IsOverInterior(float x, float y, float z, float yTolerance = 2.2f) =>
        float.IsFinite(x) && float.IsFinite(y) && float.IsFinite(z)
        && MathF.Abs(y - FloorTopY) < yTolerance
        && ContainsInterior(x, z);

    internal static bool ContainsWeatherPoint(float x, float y, float z) =>
        float.IsFinite(x) && float.IsFinite(y) && float.IsFinite(z)
        && MathF.Abs(x - WeatherCenterX) <= WeatherSizeX * 0.5f
        && MathF.Abs(y - WeatherCenterY) <= WeatherSizeY * 0.5f
        && MathF.Abs(z - WeatherCenterZ) <= WeatherSizeZ * 0.5f;

    internal static bool ContainsPlacement(float x, float y, float z)
    {
        if (!float.IsFinite(x) || !float.IsFinite(y) || !float.IsFinite(z))
            return false;
        const float margin = 0.05f;
        float minX = CenterX - WidthX * 0.5f + margin;
        float maxX = CenterX + WidthX * 0.5f - margin;
        float minZ = CenterZ - LengthZ * 0.5f + margin;
        float maxZ = CenterZ + LengthZ * 0.5f - margin;
        return x >= minX && x <= maxX
            && z >= minZ && z <= maxZ
            && y >= FloorTopY - 0.5f && y <= EaveY + 0.5f;
    }

    internal static IReadOnlyList<HallWallSurfaceSpec> WallSurfaces()
    {
        const float outerWest = CenterX - WidthX * 0.5f;
        const float outerEast = CenterX + WidthX * 0.5f;
        const float outerSouth = CenterZ - LengthZ * 0.5f;
        const float outerNorth = CenterZ + LengthZ * 0.5f;
        const float minY = 0.20f;
        const float maxY = EaveY - 0.12f;
        const float height = maxY - minY;
        const float centerY = (minY + maxY) * 0.5f;

        const float wsMinZ = outerSouth + 0.15f;
        const float wsMaxZ = 21.15f;
        const float wnMinZ = 23.25f;
        const float wnMaxZ = outerNorth - 0.15f;
        const float eMinZ = outerSouth + 0.15f;
        const float eMaxZ = outerNorth - 0.15f;
        const float snMinX = outerWest + 0.15f;
        const float snMaxX = outerEast - 0.15f;

        return new HallWallSurfaceSpec[]
        {
            new("WestSouth", "a1b2c3d4-1111-4a2b-9c3d-000000000101", outerWest + WallSurfaceInset, centerY, (wsMinZ + wsMaxZ) * 0.5f, wsMaxZ - wsMinZ, height, 1f, 0f),
            new("WestNorth", "a1b2c3d4-1111-4a2b-9c3d-000000000102", outerWest + WallSurfaceInset, centerY, (wnMinZ + wnMaxZ) * 0.5f, wnMaxZ - wnMinZ, height, 1f, 0f),
            new("East", "a1b2c3d4-1111-4a2b-9c3d-000000000103", outerEast - WallSurfaceInset, centerY, CenterZ, eMaxZ - eMinZ, height, -1f, 0f),
            new("South", "a1b2c3d4-1111-4a2b-9c3d-000000000104", CenterX, centerY, outerSouth + WallSurfaceInset, snMaxX - snMinX, height, 0f, 1f),
            new("North", "a1b2c3d4-1111-4a2b-9c3d-000000000105", CenterX, centerY, outerNorth - WallSurfaceInset, snMaxX - snMinX, height, 0f, -1f)
        };
    }

    internal static bool TryWorldToTileCoord(float worldX, float worldZ, float originX, float originZ, float tileSize, out int x, out int z)
    {
        x = 0;
        z = 0;
        if (!float.IsFinite(worldX) || !float.IsFinite(worldZ) || !float.IsFinite(originX) || !float.IsFinite(originZ) || !float.IsFinite(tileSize) || tileSize <= 0.05f)
            return false;

        x = (int)MathF.Round((worldX - originX) / tileSize);
        z = (int)MathF.Round((worldZ - originZ) / tileSize);
        float maxDelta = tileSize * 0.55f;
        return MathF.Abs(originX + x * tileSize - worldX) <= maxDelta
            && MathF.Abs(originZ + z * tileSize - worldZ) <= maxDelta;
    }

    internal static IReadOnlyList<HallTile> Tiles(float tileSize)
    {
        if (tileSize < 0.2f || tileSize > 1f)
            return Array.Empty<HallTile>();

        int countX = (int)MathF.Floor((InteriorMaxX - InteriorMinX + 0.001f) / tileSize);
        int countZ = (int)MathF.Floor((InteriorMaxZ - InteriorMinZ + 0.001f) / tileSize);
        if (countX <= 0 || countZ <= 0)
            return Array.Empty<HallTile>();

        float startX = CenterX - countX * tileSize * 0.5f + tileSize * 0.5f;
        float startZ = CenterZ - countZ * tileSize * 0.5f + tileSize * 0.5f;
        var tiles = new List<HallTile>(countX * countZ);
        for (int ix = 0; ix < countX; ix++)
        {
            float x = startX + ix * tileSize;
            for (int iz = 0; iz < countZ; iz++)
            {
                float z = startZ + iz * tileSize;
                if (!ContainsInterior(x, z))
                    continue;
                tiles.Add(new HallTile(ix, iz, x, z));
            }
        }

        return tiles;
    }

    internal static (float OriginX, float OriginZ) GridOrigin(IReadOnlyList<HallTile> tiles)
    {
        if (tiles == null || tiles.Count == 0)
            throw new ArgumentException("hall-grid-empty", nameof(tiles));

        float originX = float.MaxValue;
        float originZ = float.MaxValue;
        for (int i = 0; i < tiles.Count; i++)
        {
            originX = Math.Min(originX, tiles[i].WorldX);
            originZ = Math.Min(originZ, tiles[i].WorldZ);
        }

        return (originX, originZ);
    }
}

internal readonly record struct HallTile(int X, int Z, float WorldX, float WorldZ);

internal readonly record struct HallWallSurfaceSpec(
    string Name,
    string Guid,
    float CenterX,
    float CenterY,
    float CenterZ,
    float Width,
    float Height,
    float NormalX,
    float NormalZ);
