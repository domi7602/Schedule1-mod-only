using Xunit;

namespace NorthIsland.Tests;

public class NorthIslandTests
{
    [Fact]
    public void TerrainGrid_DimensionsMatchAndSampleClampsCleanly()
    {
        Assert.Equal(
            NorthIslandTerrainData.CountX * NorthIslandTerrainData.CountZ,
            NorthIslandTerrainData.Heights.Length);

        float clampedWest = NorthIslandTerrainData.SampleHeight(-999f, 424f);
        float edgeWest = NorthIslandTerrainData.SampleHeight(NorthIslandTerrainData.MinX, 424f);
        Assert.InRange(MathF.Abs(clampedWest - edgeWest), 0f, 0.001f);

        float plateauH = NorthIslandTerrainData.SampleHeight(-98f, 330f);
        Assert.InRange(plateauH, 7.8f, 8.2f);
    }

    [Fact]
    public void BoulevardSurfaceY_NeverDropsBelowUnderlyingTerrain()
    {
        for (float x = NorthIslandGeometry.BoulevardMinX; x <= NorthIslandGeometry.BoulevardMaxX; x += 1.0f)
        {
            float blvdY = NorthIslandGeometry.GetBoulevardSurfaceY(x);
            float tSouth = NorthIslandTerrainData.SampleHeight(x, NorthIslandGeometry.BoulevardMinZ);
            float tMid = NorthIslandTerrainData.SampleHeight(x, 429.0f);
            float tNorth = NorthIslandTerrainData.SampleHeight(x, NorthIslandGeometry.BoulevardMaxZ);
            float maxTerrain = MathF.Max(tSouth, MathF.Max(tMid, tNorth));

            Assert.True(
                blvdY >= maxTerrain + 0.16f - 0.001f,
                $"Boulevard at x={x} dropped below terrain+0.16: {blvdY:F3} < {maxTerrain + 0.16f:F3}");
        }
    }

    [Fact]
    public void OffRamp_WeldsFlushAtOverpassTopAndClearsTerrainAcrossFullRamp()
    {
        float leftX = NorthIslandGeometry.OverpassX - NorthIslandGeometry.OverpassTotalWidth * 0.5f;
        float rightX = NorthIslandGeometry.OverpassX + NorthIslandGeometry.OverpassTotalWidth * 0.5f;

        foreach (float x in new[] { leftX, -22.0f, NorthIslandGeometry.OverpassX, -15.0f, rightX })
        {
            float topY = NorthIslandGeometry.GetOffRampPointY(x, NorthIslandGeometry.OffRampStartZ);
            Assert.InRange(MathF.Abs(topY - NorthIslandGeometry.OverpassDeckTopY), 0f, 0.0001f);

            float botY = NorthIslandGeometry.GetOffRampPointY(x, NorthIslandGeometry.OffRampEndZ);
            float expectedBlvdY = NorthIslandGeometry.GetBoulevardSurfaceY(x);
            Assert.InRange(MathF.Abs(botY - expectedBlvdY), 0f, 0.0001f);
        }

        // Verify monotonic descent and positive clearance above terrain across the entire 14m ramp surface
        float prevY = NorthIslandGeometry.GetOffRampPointY(NorthIslandGeometry.OverpassX, NorthIslandGeometry.OffRampStartZ);
        for (float z = NorthIslandGeometry.OffRampStartZ; z <= NorthIslandGeometry.OffRampEndZ; z += 0.5f)
        {
            float centerY = NorthIslandGeometry.GetOffRampPointY(NorthIslandGeometry.OverpassX, z);
            Assert.True(centerY <= prevY + 1e-5f, $"Expected monotonic descent at z={z}: prev={prevY}, cur={centerY}");
            prevY = centerY;

            float yL = NorthIslandGeometry.GetOffRampPointY(leftX, z);
            float yR = NorthIslandGeometry.GetOffRampPointY(rightX, z);
            for (float fx = 0f; fx <= 1f; fx += 0.25f)
            {
                float x = leftX + (rightX - leftX) * fx;
                float meshY = yL + (yR - yL) * fx;
                float terrainY = NorthIslandTerrainData.SampleHeight(x, z);
                Assert.True(
                    meshY >= terrainY + 0.12f,
                    $"OffRamp clipped close to/below terrain at ({x:F1}, {z:F1}): ramp={meshY:F3} < terrain+0.12 ({terrainY + 0.12f:F3})");
            }
        }
    }

    [Fact]
    public void HighlandDrive_WeldsEndpointsAndCurbsStayClearOfBoulevardAndCourtyard()
    {
        NorthIslandGeometry.SampleHighlandCenterline(0f, out _, out float startY, out float startZ);
        Assert.InRange(MathF.Abs(startY - NorthIslandGeometry.GetBoulevardSurfaceY(NorthIslandGeometry.HighlandStartX)), 0f, 0.0001f);
        Assert.Equal(NorthIslandGeometry.HighlandStartZ, startZ);

        NorthIslandGeometry.SampleHighlandCenterline(1f, out _, out float endY, out float endZ);
        Assert.Equal(NorthIslandGeometry.HighlandPlateauY, endY);
        Assert.Equal(NorthIslandGeometry.HighlandEndZ, endZ);

        NorthIslandGeometry.GetHighlandHorizontalFrame(out float dirX, out float dirZ, out float rightX, out float rightZ, out _);

        const int steps = 20;
        float prevX = 0f, prevZ = 0f;
        for (int i = 0; i <= steps; i++)
        {
            float t = NorthIslandGeometry.HighlandCurbedTMin +
                      (NorthIslandGeometry.HighlandCurbedTMax - NorthIslandGeometry.HighlandCurbedTMin) * ((float)i / steps);
            NorthIslandGeometry.SampleHighlandCenterline(t, out float cx, out float cy, out float cz);

            float terrainCenter = NorthIslandTerrainData.SampleHeight(cx, cz);
            Assert.True(cy >= terrainCenter + 0.14f - 0.001f, $"Highland centerline at t={t} below terrain+0.14");

            if (i > 0)
            {
                float midX = (prevX + cx) * 0.5f;
                float midZ = (prevZ + cz) * 0.5f;
                float segHorizLen = MathF.Sqrt((cx - prevX) * (cx - prevX) + (cz - prevZ) * (cz - prevZ)) + 0.12f;
                float halfLen = segHorizLen * 0.5f;
                float halfCurbW = NorthIslandGeometry.HighlandCurbWidth * 0.5f;

                foreach (float sideSign in new[] { -1f, +1f })
                {
                    float curbCenterX = midX + rightX * (sideSign * NorthIslandGeometry.HighlandCurbOffset);
                    float curbCenterZ = midZ + rightZ * (sideSign * NorthIslandGeometry.HighlandCurbOffset);

                    foreach (float sx in new[] { -1f, +1f })
                    {
                        foreach (float sz in new[] { -1f, +1f })
                        {
                            float cornerZ = curbCenterZ + rightZ * (sx * halfCurbW) + dirZ * (sz * halfLen);
                            Assert.True(
                                cornerZ < NorthIslandGeometry.BoulevardMinZ,
                                $"Highland curb corner protruded into Boulevard at seg {i - 1}: z={cornerZ:F3} (X~{curbCenterX:F2})");
                            Assert.True(
                                cornerZ > NorthIslandGeometry.HighlandEndZ,
                                $"Highland curb corner protruded into West Plateau Courtyard at seg {i - 1}: z={cornerZ:F3} (X~{curbCenterX:F2})");
                        }
                    }
                }
            }

            prevX = cx;
            prevZ = cz;
        }
    }

    [Fact]
    public void IsNorthMedianCubeMatch_MatchesOnlyOverpassMedianAndProtectsTownObjects()
    {
        Assert.True(BarrierFilterRules.IsNorthMedianCubeMatch("Cube (1)", "Overpass Segment (50)", -18.5f, 10.5f, 200f));
        Assert.True(BarrierFilterRules.IsNorthMedianCubeMatch("Divider", "AnyParent", -18.5f, 10.3f, 290f));
        Assert.True(BarrierFilterRules.IsNorthMedianCubeMatch("concrete_lod1 (3)", "AnyParent", -18.5f, 10.4f, 150f));

        // Reject town buildings/props named 'Cube (1)'
        Assert.False(BarrierFilterRules.IsNorthMedianCubeMatch("Cube (1)", "Northshore_GeneralStore", -33.0f, 10.2f, 415.0f));
        Assert.False(BarrierFilterRules.IsNorthMedianCubeMatch("Cube (1)", "Town_Props", -18.5f, 10.2f, 430.0f));
        Assert.False(BarrierFilterRules.IsNorthMedianCubeMatch("Cube (1)", "RandomParent", -18.5f, 10.5f, 200.0f));
        Assert.False(BarrierFilterRules.IsNorthMedianCubeMatch("Cube (1)", "Overpass Segment (50)", -24.0f, 10.5f, 200.0f));
        Assert.False(BarrierFilterRules.IsNorthMedianCubeMatch("Cube (1)", "Overpass Segment (50)", -18.5f, 2.5f, 200.0f));
    }

    [Fact]
    public void IsNorthernGapObstacleMatch_FiltersProtectedEssentialAndDynamicObjects()
    {
        Assert.True(BarrierFilterRules.IsNorthernGapObstacleMatch("ConstructionWall", 0, false, -21f, -16f, 9.9f, 12.2f, 138f, 140f));

        Assert.False(BarrierFilterRules.IsNorthernGapObstacleMatch("ConstructionWall", 6, false, -21f, -16f, 9.9f, 12.2f, 138f, 140f));
        Assert.False(BarrierFilterRules.IsNorthernGapObstacleMatch("ConstructionWall", 24, false, -21f, -16f, 9.9f, 12.2f, 138f, 140f));
        Assert.False(BarrierFilterRules.IsNorthernGapObstacleMatch("PlayerVehicle", 0, true, -21f, -16f, 9.9f, 12.2f, 138f, 140f));
        Assert.False(BarrierFilterRules.IsNorthernGapObstacleMatch("Concrete", 0, false, -25f, -12f, 9.8f, 10.5f, 138f, 145f));
        Assert.False(BarrierFilterRules.IsNorthernGapObstacleMatch("Fence_North", 0, false, -25f, -24.5f, 10.0f, 11.4f, 138f, 145f));
        Assert.False(BarrierFilterRules.IsNorthernGapObstacleMatch("WaterBelow", 0, false, -22f, -15f, -2.0f, 10.05f, 138f, 145f));
        Assert.False(BarrierFilterRules.IsNorthernGapObstacleMatch("DistrictMesh", 0, false, -60f, 20f, 9.9f, 12.5f, 135f, 150f));
    }

    [Fact]
    public void IsRoadwayAirspaceObstacleMatch_FocusesOnlyOnNativeRampTurnThroat()
    {
        Assert.True(BarrierFilterRules.IsRoadwayAirspaceObstacleMatch("BlockingBox", 0, false, -18f, -14f, 10.15f, 12.0f, -90f, -86f));
        Assert.False(BarrierFilterRules.IsRoadwayAirspaceObstacleMatch("GapAsset", 0, false, -22f, -14f, 10.5f, 12.0f, 215f, 235f));
        Assert.False(BarrierFilterRules.IsRoadwayAirspaceObstacleMatch("Overpass Segment (62)", 0, false, -22f, -14f, 10.2f, 12.0f, -92f, -86f));
        Assert.False(BarrierFilterRules.IsRoadwayAirspaceObstacleMatch("DeckTop", 0, false, -22f, -14f, 9.5f, 10.08f, -92f, -86f));
    }

    [Fact]
    public void IsInvisibleWallCandidate_PreservesEssentialGeometry()
    {
        Assert.True(BarrierFilterRules.IsInvisibleWallCandidate("InvisibleBoundary_North", false, -30f, 10f, 0f, 20f, 130f, 135f));
        Assert.True(BarrierFilterRules.IsInvisibleWallCandidate("GenericCollider", true, -20f, -15f, 0f, 15f, 250f, 260f));
        Assert.True(BarrierFilterRules.IsInvisibleWallCandidate("RoadBlocker4_LOD", false, -10f, 0f, 0f, 3f, -50f, -40f));

        Assert.False(BarrierFilterRules.IsInvisibleWallCandidate("Overpass Segment (10)", true, -25f, -12f, 0f, 12f, 100f, 110f));
        Assert.False(BarrierFilterRules.IsInvisibleWallCandidate("Sidewalk_Main", true, -10f, 10f, 0f, 2f, 420f, 425f));
        Assert.False(BarrierFilterRules.IsInvisibleWallCandidate("North Island Terrain", true, -100f, 100f, -10f, 10f, 300f, 450f));
    }

    [Fact]
    public void ExpandedBroadphaseBounds_CoverVanillaHylandPointAndNorthIsland()
    {
        Assert.InRange(NorthIslandGeometry.BroadphaseSubdivisions, 1, 16);
        Assert.True(NorthIslandGeometry.BroadphaseSizeX > 0f);
        Assert.True(NorthIslandGeometry.BroadphaseSizeY > 0f);
        Assert.True(NorthIslandGeometry.BroadphaseSizeZ > 0f);

        // Vanilla Hyland Point bounds [-250..250] x [-50..50] x [-250..250]
        foreach (float x in new[] { -250f, 0f, 250f })
            foreach (float y in new[] { -50f, 0f, 50f })
                foreach (float z in new[] { -250f, 0f, 250f })
                    Assert.True(NorthIslandGeometry.ContainsWorldPointInBroadphase(x, y, z), $"Missing vanilla corner ({x}, {y}, {z})");

        // North Island bridge, ramp, boulevard, plateau, and terrain north of z = 250
        Assert.True(NorthIslandGeometry.ContainsWorldPointInBroadphase(NorthIslandGeometry.OverpassX, NorthIslandGeometry.OverpassDeckTopY, 270f));
        Assert.True(NorthIslandGeometry.ContainsWorldPointInBroadphase(NorthIslandGeometry.OverpassX, NorthIslandGeometry.OverpassDeckTopY, NorthIslandGeometry.OffRampStartZ));
        Assert.True(NorthIslandGeometry.ContainsWorldPointInBroadphase(NorthIslandGeometry.BoulevardMinX, 0.5f, NorthIslandGeometry.BoulevardMaxZ));
        Assert.True(NorthIslandGeometry.ContainsWorldPointInBroadphase(NorthIslandGeometry.BoulevardMaxX, 5f, NorthIslandGeometry.BoulevardMaxZ));
        Assert.True(NorthIslandGeometry.ContainsWorldPointInBroadphase(NorthIslandGeometry.HighlandEndX, NorthIslandGeometry.HighlandPlateauY, NorthIslandGeometry.HighlandEndZ));
        Assert.True(NorthIslandGeometry.ContainsWorldPointInBroadphase(-220f, -5f, 560f));
        Assert.True(NorthIslandGeometry.ContainsWorldPointInBroadphase(220f, 80f, 560f));
    }

    [Fact]
    public void IsInstancedProxyName_FiltersInstancingManagerTreesWhileAllowingRealPrefabs()
    {
        Assert.True(NorthIslandGeometry.IsInstancedProxyName("InstanceWorldObject(Clone)"));
        Assert.True(NorthIslandGeometry.IsInstancedProxyName("@InstancingManager"));
        Assert.True(NorthIslandGeometry.IsInstancedProxyName("InstancedMapTrees"));

        Assert.False(NorthIslandGeometry.IsInstancedProxyName("tree_Pine_L2_LOD0"));
        Assert.False(NorthIslandGeometry.IsInstancedProxyName("Fir L - Variant 2"));
        Assert.False(NorthIslandGeometry.IsInstancedProxyName("SGS_Shrub_LOD0"));
        Assert.False(NorthIslandGeometry.IsInstancedProxyName(string.Empty));
        Assert.False(NorthIslandGeometry.IsInstancedProxyName(null));
    }

    [Fact]
    public void CoastalHouseWest_ClearsHighlandDriveRoadAndCurb()
    {
        NorthIslandGeometry.GetHighlandHorizontalFrame(out _, out _, out float rightX, out float rightZ, out _);

        float halfFoundW = NorthIslandGeometry.CoastalHouseBodyWidth * 0.5f + 0.4f;
        float halfFoundD = NorthIslandGeometry.CoastalHouseBodyDepth * 0.5f + 0.4f;
        float outerCurbLat = NorthIslandGeometry.HighlandHalfWidth + NorthIslandGeometry.HighlandCurbWidth;

        // House must stay cleanly within Blvd_SouthWalk West boundary and well West of Highland Drive's outer curb
        Assert.True(NorthIslandGeometry.CoastalHouseWestX - halfFoundW >= NorthIslandGeometry.BoulevardMinX);

        foreach (float dx in new[] { -halfFoundW, halfFoundW })
        {
            foreach (float dz in new[] { -halfFoundD, halfFoundD })
            {
                float cx = NorthIslandGeometry.CoastalHouseWestX + dx;
                float cz = NorthIslandGeometry.CoastalHouseWestZ + dz;
                float lat = (cx - NorthIslandGeometry.HighlandStartX) * rightX + (cz - NorthIslandGeometry.HighlandStartZ) * rightZ;
                Assert.True(
                    lat >= outerCurbLat + 2.0f,
                    $"CoastalHouse_West corner ({cx:F2}, {cz:F2}) too close to Highland curb: lat={lat:F2}m (outerCurb={outerCurbLat:F2}m)");
            }
        }
    }
}
