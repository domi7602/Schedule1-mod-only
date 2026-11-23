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

    [Fact]
    public void HideoutGrid_StaysInsideWallsAndClearsTheDoorCorridor()
    {
        Assert.True(HideoutPropertyPlan.Inset > HideoutPropertyPlan.AntiTunnel);
        var tiles = HideoutPropertyPlan.Tiles(0.5f);
        Assert.InRange(tiles.Count, 400, 800);

        var seen = new HashSet<(int X, int Z)>();
        foreach (var tile in tiles)
        {
            Assert.True(seen.Add((tile.X, tile.Z)));
            Assert.True(HideoutPropertyPlan.ContainsInterior(tile.WorldX, tile.WorldZ));
            Assert.True(tile.WorldX >= HideoutPropertyPlan.ExteriorMinX + HideoutPropertyPlan.AntiTunnel);
            Assert.True(tile.WorldZ >= HideoutPropertyPlan.ExteriorMinZ + HideoutPropertyPlan.AntiTunnel);
            Assert.True(tile.WorldZ <= HideoutPropertyPlan.ExteriorMaxZ - HideoutPropertyPlan.AntiTunnel);
        }
    }

    [Fact]
    public void HideoutSign_StandsOutsideTheDoorAndOffTheRamp()
    {
        Assert.True(HideoutPropertyPlan.SignX > HideoutPropertyPlan.ExteriorMaxX + 2.2f);
        Assert.True(HideoutPropertyPlan.SignZ > HideoutPropertyPlan.DoorMaxZ + 1.5f);
        Assert.InRange(HideoutPropertyPlan.SignRootLift, 0.55f, 0.9f);
        Assert.False(HideoutPropertyPlan.ContainsInterior(HideoutPropertyPlan.SignX, HideoutPropertyPlan.SignZ));
        float dx = HideoutPropertyPlan.SpawnX - HideoutPropertyPlan.SignX;
        float dz = HideoutPropertyPlan.SpawnZ - HideoutPropertyPlan.SignZ;
        Assert.True(dx * dx + dz * dz > 1.5f);
    }

    [Fact]
    public void HideoutPurchase_UsesBankBeforeCashAndRefusesAShortWallet()
    {
        var bank = HideoutPurchase.Evaluate(true, false, 20000f, 0f);
        Assert.Equal(HideoutPurchaseStatus.Purchased, bank.Status);
        Assert.True(bank.ChargeBank);
        Assert.Equal(20000f, bank.Amount);

        var cash = HideoutPurchase.Evaluate(true, false, 100f, 20000f);
        Assert.Equal(HideoutPurchaseStatus.Purchased, cash.Status);
        Assert.False(cash.ChargeBank);

        Assert.Equal(HideoutPurchaseStatus.InsufficientFunds, HideoutPurchase.Evaluate(true, false, 19999f, 19999f).Status);
        Assert.Equal(HideoutPurchaseStatus.AlreadyOwned, HideoutPurchase.Evaluate(true, true, 50000f, 50000f).Status);
        Assert.Equal(HideoutPurchaseStatus.NotReady, HideoutPurchase.Evaluate(false, false, 50000f, 50000f).Status);
        Assert.Equal("$20,000", HideoutPropertyPlan.PriceLabel);
        Assert.True(HideoutListing.IsPropertyMenu("DocksWarehouse", "Docks Warehouse"));
        Assert.False(HideoutListing.IsPropertyMenu("Laundromat", "Laundromat"));
    }

    [Fact]
    public void HideoutSave_RejectsAnotherSlotAndAnUnownedPlacement()
    {
        var ready = new HideoutSaveFile
        {
            SaveFolder = @"C:\Saves\SlotA",
            Owned = false
        };
        Assert.Equal(HideoutSaveKind.Ready, HideoutSaveRules.Classify(ready, @"C:\Saves\SlotA", true));
        Assert.Equal(HideoutSaveKind.ForeignSlot, HideoutSaveRules.Classify(ready, @"C:\Saves\SlotB", true));
        Assert.Equal(HideoutSaveKind.Missing, HideoutSaveRules.Classify(null, @"C:\Saves\SlotA", false));

        ready.Placements.Add(new HideoutPlacementRecord
        {
            Id = "one",
            ItemId = "bed",
            X = HideoutPropertyPlan.CenterX,
            Y = HideoutPropertyPlan.FloorTopY,
            Z = HideoutPropertyPlan.CenterZ,
            RotW = 1f
        });
        Assert.Equal(HideoutSaveKind.Corrupt, HideoutSaveRules.Classify(ready, @"C:\Saves\SlotA", true));

        ready.Owned = true;
        Assert.Equal(HideoutSaveKind.Ready, HideoutSaveRules.Classify(ready, @"C:/Saves/SlotA/", true));
        ready.Schema = 2;
        Assert.Equal(HideoutSaveKind.Unsupported, HideoutSaveRules.Classify(ready, @"C:\Saves\SlotA", true));
    }

    [Fact]
    public void CoastRailClip_OpensExactlyThePierGap()
    {
        const float minX = NorthIslandGeometry.BoulevardMinX;
        const float maxX = NorthIslandGeometry.BoulevardMaxX;
        const int steps = 35;
        bool sawGapSegment = false;

        for (int i = 0; i < steps; i++)
        {
            float x0 = minX + (maxX - minX) * i / steps;
            float x1 = minX + (maxX - minX) * (i + 1) / steps;
            var spans = NorthIslandGeometry.ClipCoastRailSpan(x0, x1);

            float covered = 0f;
            foreach (var (a, b) in spans)
            {
                Assert.True(b > a, $"Empty span returned for segment {i}");
                Assert.True(a >= x0 - 0.001f && b <= x1 + 0.001f, $"Span ({a},{b}) left segment [{x0},{x1}]");
                Assert.True(b <= NorthIslandGeometry.PierMinX + 0.001f || a >= NorthIslandGeometry.PierMaxX - 0.001f,
                    $"Span ({a},{b}) intrudes into the pier entrance gap");
                covered += b - a;
            }

            float gapInside = MathF.Max(0f, MathF.Min(x1, NorthIslandGeometry.PierMaxX) - MathF.Max(x0, NorthIslandGeometry.PierMinX));
            Assert.InRange(MathF.Abs(covered + gapInside - (x1 - x0)), 0f, 0.01f);
            if (gapInside > 0.001f) sawGapSegment = true;
        }

        Assert.True(sawGapSegment, "No segment intersected the pier gap — constants drifted?");
        Assert.Empty(NorthIslandGeometry.ClipCoastRailSpan(15f, 19f));
        var partial = NorthIslandGeometry.ClipCoastRailSpan(10f, 15f);
        Assert.Single(partial);
        Assert.InRange(MathF.Abs(partial[0].B - NorthIslandGeometry.PierMinX), 0f, 0.001f);
    }

    [Fact]
    public void PierDeck_StaysAboveWaterAndReachableFromTheBoardwalk()
    {
        float walkTop = NorthIslandGeometry.GetPierWalkTopY();
        Assert.InRange(MathF.Abs(walkTop - (NorthIslandGeometry.GetBoulevardSurfaceY(NorthIslandGeometry.PierCenterX) + 0.10f)), 0f, 0.0001f);

        foreach (float waterY in new[] { -0.2f, 0f, 0.5f })
        {
            float deckTop = NorthIslandGeometry.GetPierDeckTopY(waterY);
            Assert.True(deckTop >= waterY + NorthIslandGeometry.PierFreeboard - 0.001f,
                $"Deck {deckTop:F3} washed over at waterY={waterY:F2}");
            Assert.True(deckTop >= walkTop - 0.001f, "Deck dropped below the boardwalk");
            Assert.True(deckTop - walkTop <= 1.0f, $"Entrance rise {deckTop - walkTop:F2}m too steep for the threshold ramp");
        }
    }

    [Fact]
    public void PierPilings_ReachTheSeabedAlongTheWholePier()
    {
        float deckTop = NorthIslandGeometry.GetPierDeckTopY(0f);
        for (float z = 438f; z <= NorthIslandGeometry.PierEndZ; z += 5f)
        {
            foreach (float x in new[] { NorthIslandGeometry.PierMinX + 0.3f, NorthIslandGeometry.PierMaxX - 0.3f })
            {
                float seabed = NorthIslandTerrainData.SampleHeight(x, z);
                Assert.True(seabed < deckTop - 0.8f, $"Piling at ({x},{z}) has no positive length: seabed={seabed:F2}");
            }
        }

        foreach (float x in new[] { NorthIslandGeometry.PierHeadMinX + 0.4f, NorthIslandGeometry.PierHeadMaxX - 0.4f })
        {
            float seabed = NorthIslandTerrainData.SampleHeight(x, NorthIslandGeometry.PierHeadMaxZ - 0.4f);
            Assert.True(seabed < deckTop - 0.8f, $"Head piling at ({x},{NorthIslandGeometry.PierHeadMaxZ}) floats: seabed={seabed:F2}");
        }
    }

    [Fact]
    public void PierEntrance_ClearsLampPolesBenchesAndTheWelcomeSign()
    {
        // The x=12 north lamp pole must stay outside the guardrail gap with margin.
        Assert.True(NorthIslandGeometry.PierMinX - 12f >= 1.5f);
        Assert.True(32f - NorthIslandGeometry.PierMaxX >= 1.5f);

        // Boardwalk bench at x=22 (seat half-width 0.9) must not collide with the wider head platform.
        Assert.True(22f - 0.9f - NorthIslandGeometry.PierHeadMaxX >= 0.1f);

        // Welcome sign posts (-21.2 / -15.8) stay far west of the pier.
        Assert.True(NorthIslandGeometry.PierHeadMinX - (-15.8f) > 20f);

        // Pedestrian-wide gap, but the entrance bollard pair (runtime) keeps cars out.
        Assert.InRange(NorthIslandGeometry.PierMaxX - NorthIslandGeometry.PierMinX, 4f, 8f);
    }

    [Fact]
    public void LighthouseMound_StandsInOpenWaterClearOfPierAndBridgeChannel()
    {
        float cx = NorthIslandGeometry.LighthouseX;
        float cz = NorthIslandGeometry.LighthouseZ;
        float half = NorthIslandGeometry.LighthouseMoundHalfWidth;

        foreach (float dx in new[] { -half, 0f, half })
        {
            foreach (float dz in new[] { -half, 0f, half })
            {
                float seabed = NorthIslandTerrainData.SampleHeight(cx + dx, cz + dz);
                Assert.True(seabed < 0.3f, $"Mound corner ({cx + dx},{cz + dz}) is not in open water: seabed={seabed:F2}");
            }
        }

        // Clear of the bridge approach channel (x = -25.5..-11.5) and north of the boardwalk rail.
        Assert.True(cx + half < -25.5f);
        Assert.True(cz - half > NorthIslandGeometry.PierStartZ);

        // Clear of the pier footprint including its head platform.
        bool overlapsX = cx + half > NorthIslandGeometry.PierHeadMinX && cx - half < NorthIslandGeometry.PierHeadMaxX;
        bool overlapsZ = cz + half > NorthIslandGeometry.PierStartZ && cz - half < NorthIslandGeometry.PierHeadMaxZ;
        Assert.False(overlapsX && overlapsZ, "Lighthouse mound overlaps the pier");

        float baseY = NorthIslandGeometry.GetLighthouseBaseY(NorthIslandGeometry.VisualSeaLevelY);
        Assert.True(baseY >= NorthIslandGeometry.VisualSeaLevelY + 2f, "Mound must break the visual waterline");
        Assert.True(NorthIslandTerrainData.SampleHeight(cx, cz) < baseY - 4f, "Mound is not standing in deep water");

        // Water must read between the shoreline and the islet, not a tower on the near grass.
        float shoreZ = 430f;
        for (float z = 432f; z < cz; z += 2f)
        {
            float h = NorthIslandTerrainData.SampleHeight(cx, z);
            if (h < -0.15f)
            {
                shoreZ = z;
                break;
            }
        }

        Assert.True(cz - half - shoreZ >= 35f, $"Islet too close to shore: shoreZ={shoreZ:F0}, moundSouth={cz - half:F0}");
    }

    [Fact]
    public void LighthouseBeam_IsNightOnlyAndRejectsABrokenClock()
    {
        Assert.True(NorthIslandGeometry.TryIsLighthouseNight(1800, out bool dusk) && dusk);
        Assert.True(NorthIslandGeometry.TryIsLighthouseNight(2359, out bool late) && late);
        Assert.True(NorthIslandGeometry.TryIsLighthouseNight(0, out bool midnight) && midnight);
        Assert.True(NorthIslandGeometry.TryIsLighthouseNight(559, out bool preDawn) && preDawn);

        Assert.True(NorthIslandGeometry.TryIsLighthouseNight(600, out bool dawn) && !dawn);
        Assert.True(NorthIslandGeometry.TryIsLighthouseNight(1129, out bool morning) && !morning);
        Assert.True(NorthIslandGeometry.TryIsLighthouseNight(1759, out bool evening) && !evening);

        Assert.False(NorthIslandGeometry.TryIsLighthouseNight(1860, out _));
        Assert.False(NorthIslandGeometry.TryIsLighthouseNight(2400, out _));
        Assert.False(NorthIslandGeometry.TryIsLighthouseNight(-1, out _));
        Assert.InRange(NorthIslandGeometry.LighthouseRevolutionSeconds, 8f, 30f);
    }

    [Fact]
    public void StringLights_KeepHeadroomOverTheBoardwalk()
    {
        var xs = NorthIslandGeometry.NorthLampXs;
        Assert.True(xs.Length >= 2);

        for (int i = 0; i + 1 < xs.Length; i++)
        {
            float yA = NorthIslandGeometry.GetBoulevardSurfaceY(xs[i]) + 0.10f + NorthIslandGeometry.StringLightAttachHeight;
            float yB = NorthIslandGeometry.GetBoulevardSurfaceY(xs[i + 1]) + 0.10f + NorthIslandGeometry.StringLightAttachHeight;
            float walkMid = NorthIslandGeometry.GetBoulevardSurfaceY((xs[i] + xs[i + 1]) * 0.5f) + 0.10f;

            float minY = float.MaxValue;
            for (float t = 0f; t <= 1.0001f; t += 0.05f)
            {
                float y = NorthIslandGeometry.GetStringLightY(yA, yB, MathF.Min(t, 1f));
                if (y < minY) minY = y;
            }

            Assert.True(minY >= walkMid + NorthIslandGeometry.StringLightMinHeadroom,
                $"String light span {xs[i]}..{xs[i + 1]} dips to {minY:F2}, headroom over walk {walkMid:F2} too low");
        }

        // Equal-height attachments sag exactly StringLightSag at mid-span.
        float mid = NorthIslandGeometry.GetStringLightY(5f, 5f, 0.5f);
        Assert.InRange(MathF.Abs(mid - (5f - NorthIslandGeometry.StringLightSag)), 0f, 0.0001f);
    }

    [Fact]
    public void GuardColliderSplit_CoversEverythingExceptThePierGap()
    {
        float westWidth = NorthIslandGeometry.PierMinX - NorthIslandGeometry.BoulevardMinX;
        float eastWidth = NorthIslandGeometry.BoulevardMaxX - NorthIslandGeometry.PierMaxX;
        float gap = NorthIslandGeometry.PierMaxX - NorthIslandGeometry.PierMinX;
        float total = NorthIslandGeometry.BoulevardMaxX - NorthIslandGeometry.BoulevardMinX;

        Assert.True(westWidth > 0f && eastWidth > 0f);
        Assert.InRange(MathF.Abs(westWidth + gap + eastWidth - total), 0f, 0.001f);
    }
}
