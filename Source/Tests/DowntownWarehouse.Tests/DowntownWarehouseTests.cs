using Xunit;

namespace DowntownWarehouse.Tests;

public class DowntownWarehouseTests
{
    [Fact]
    public void HallGrid_StaysInsideCollisionWallsAndGenerates945UniqueTiles()
    {
        const float wallThickness = 0.12f;
        float outerWest = HallPlan.CenterX - HallPlan.WidthX * 0.5f;
        float outerEast = HallPlan.CenterX + HallPlan.WidthX * 0.5f;
        float outerSouth = HallPlan.CenterZ - HallPlan.LengthZ * 0.5f;
        float outerNorth = HallPlan.CenterZ + HallPlan.LengthZ * 0.5f;

        Assert.True(HallPlan.Inset > wallThickness);

        var tiles = HallPlan.Tiles(HallPlan.GridTile);
        Assert.Equal(945, tiles.Count);

        var seen = new HashSet<(int X, int Z)>();
        foreach (var tile in tiles)
        {
            Assert.True(seen.Add((tile.X, tile.Z)));
            Assert.True(HallPlan.ContainsInterior(tile.WorldX, tile.WorldZ));
            Assert.True(tile.WorldX - HallPlan.GridTile * 0.5f >= outerWest + wallThickness);
            Assert.True(tile.WorldX + HallPlan.GridTile * 0.5f <= outerEast - wallThickness);
            Assert.True(tile.WorldZ - HallPlan.GridTile * 0.5f >= outerSouth + wallThickness);
            Assert.True(tile.WorldZ + HallPlan.GridTile * 0.5f <= outerNorth - wallThickness);
        }
    }

    [Fact]
    public void HallGrid_RejectsOutOfRangeTileSizes()
    {
        Assert.Empty(HallPlan.Tiles(0.1f));
        Assert.Empty(HallPlan.Tiles(1.5f));
    }

    [Fact]
    public void HallSpawns_PlaceApproachOutsideInShopAndInteriorInsideHall()
    {
        Assert.False(HallPlan.ContainsInterior(HallPlan.SpawnX, HallPlan.SpawnZ));
        Assert.False(HallPlan.ContainsInterior(HallPlan.NpcSpawnX, HallPlan.NpcSpawnZ));
        Assert.True(HallPlan.ContainsInterior(HallPlan.InteriorSpawnX, HallPlan.InteriorSpawnZ));
        Assert.True(HallPlan.SpawnX < HallPlan.CenterX - HallPlan.WidthX * 0.5f);
    }

    [Fact]
    public void HallPurchase_UsesBankBeforeCashAndRefusesInsufficientFunds()
    {
        var bank = HallPurchase.Evaluate(true, false, 45000f, 0f);
        Assert.Equal(HallPurchaseStatus.Purchased, bank.Status);
        Assert.True(bank.ChargeBank);
        Assert.Equal(45000f, bank.Amount);

        var cash = HallPurchase.Evaluate(true, false, 1000f, 45000f);
        Assert.Equal(HallPurchaseStatus.Purchased, cash.Status);
        Assert.False(cash.ChargeBank);
        Assert.Equal(45000f, cash.Amount);

        Assert.Equal(HallPurchaseStatus.InsufficientFunds, HallPurchase.Evaluate(true, false, 44999f, 44999f).Status);
        Assert.Equal(HallPurchaseStatus.InsufficientFunds, HallPurchase.Evaluate(true, false, float.NaN, float.PositiveInfinity).Status);
        Assert.Equal(HallPurchaseStatus.AlreadyOwned, HallPurchase.Evaluate(true, true, 100000f, 100000f).Status);
        Assert.Equal(HallPurchaseStatus.NotReady, HallPurchase.Evaluate(false, false, 100000f, 100000f).Status);
        Assert.Equal("$45,000", HallPlan.PriceLabel);
    }

    [Fact]
    public void HallListing_MatchesPropertiesAndRejectsBusinesses()
    {
        Assert.True(HallListing.IsPropertyMenu("DocksWarehouse", "Docks Warehouse"));
        Assert.True(HallListing.IsPropertyMenu("Bungalow", "Bungalow"));
        Assert.False(HallListing.IsPropertyMenu("Laundromat", "Laundromat"));
        Assert.False(HallListing.IsPropertyMenu("CarWash", "Car Wash"));
        Assert.False(HallListing.IsPropertyMenu("BusinessMenu", "Buy a business"));
    }

    [Fact]
    public void HallSave_RejectsForeignSlotCorruptAndUnownedPlacements()
    {
        var ready = new HallSaveFile
        {
            SaveFolder = @"C:\Saves\SlotA",
            Owned = false
        };
        Assert.Equal(HallSaveKind.Ready, HallSaveRules.Classify(ready, @"C:\Saves\SlotA", true));
        Assert.Equal(HallSaveKind.ForeignSlot, HallSaveRules.Classify(ready, @"C:\Saves\SlotB", true));
        Assert.Equal(HallSaveKind.Missing, HallSaveRules.Classify(null, @"C:\Saves\SlotA", false));
        Assert.Equal(HallSaveKind.Corrupt, HallSaveRules.Classify(null, @"C:\Saves\SlotA", true));

        ready.Placements.Add(new HallPlacementRecord
        {
            Id = "rack-1",
            ItemId = "storagerackmedium",
            X = HallPlan.CenterX,
            Y = HallPlan.FloorTopY,
            Z = HallPlan.CenterZ,
            RotW = 1f,
            Contents = new List<HallSlotRecord>
            {
                new() { Slot = 0, ItemId = "soil", Quantity = 10 }
            }
        });
        Assert.Equal(HallSaveKind.Corrupt, HallSaveRules.Classify(ready, @"C:\Saves\SlotA", true));

        ready.Owned = true;
        Assert.Equal(HallSaveKind.Ready, HallSaveRules.Classify(ready, @"C:/Saves/SlotA/", true));

        ready.Schema = 2;
        Assert.Equal(HallSaveKind.Unsupported, HallSaveRules.Classify(ready, @"C:\Saves\SlotA", true));
        ready.Schema = HallPlan.Schema;

        ready.PropertyCode = "other";
        Assert.Equal(HallSaveKind.Corrupt, HallSaveRules.Classify(ready, @"C:\Saves\SlotA", true));
    }

    [Fact]
    public void HallPlacementRecord_ValidatesBoundsQuaternionAndSlotContents()
    {
        static HallPlacementRecord Valid() => new()
        {
            Id = "p1",
            ItemId = "bed",
            X = HallPlan.CenterX,
            Y = HallPlan.FloorTopY,
            Z = HallPlan.CenterZ,
            RotW = 1f
        };

        Assert.True(Valid().IsPlausible());

        var outside = Valid();
        outside.X = HallPlan.CenterX - HallPlan.WidthX;
        Assert.False(outside.IsPlausible());

        var badPath = Valid();
        badPath.ItemId = "../evil";
        Assert.False(badPath.IsPlausible());

        var badQuat = Valid();
        badQuat.RotW = 0f;
        Assert.False(badQuat.IsPlausible());

        var dupSlot = Valid();
        dupSlot.Contents.Add(new HallSlotRecord { Slot = 1, ItemId = "seed", Quantity = 5 });
        dupSlot.Contents.Add(new HallSlotRecord { Slot = 1, ItemId = "seed", Quantity = 5 });
        Assert.False(dupSlot.IsPlausible());

        var zeroQty = Valid();
        zeroQty.Contents.Add(new HallSlotRecord { Slot = 0, ItemId = "seed", Quantity = 0 });
        Assert.False(zeroQty.IsPlausible());
    }

    [Fact]
    public void GridOrigin_MatchesTileZeroCenterAndSurvivesScaleDrift()
    {
        var tiles = HallPlan.Tiles(HallPlan.GridTile);
        var (originX, originZ) = HallPlan.GridOrigin(tiles);

        Assert.Equal(tiles[0].WorldX, originX, 4);
        Assert.Equal(tiles[0].WorldZ, originZ, 4);

        foreach (float drift in new[] { -0.005f, 0f, 0.005f })
        {
            foreach (var tile in tiles)
            {
                int mappedX = (int)Math.Round((tile.WorldX + drift - originX) / HallPlan.GridTile, MidpointRounding.ToEven);
                int mappedZ = (int)Math.Round((tile.WorldZ + drift - originZ) / HallPlan.GridTile, MidpointRounding.ToEven);
                Assert.Equal(tile.X, mappedX);
                Assert.Equal(tile.Z, mappedZ);
                Assert.True(HallPlan.TryWorldToTileCoord(tile.WorldX + drift, tile.WorldZ + drift, originX, originZ, HallPlan.GridTile, out int cx, out int cz));
                Assert.Equal(tile.X, cx);
                Assert.Equal(tile.Z, cz);
            }
        }
    }

    [Fact]
    public void HallInventoryPlanner_RejectsCombinedOverflowAndAllowsValidStacking()
    {
        var oneFreeSlot = new[]
        {
            new HallSlotBudget { Index = 0, Locked = false, IsEmpty = true },
            new HallSlotBudget { Index = 1, Locked = true, IsEmpty = true }
        };

        var rackAndContents = new[]
        {
            new HallReturnNeed
            {
                StackKey = "storagerackmedium",
                Quantity = 1,
                StackLimit = 10,
                AllowedEmptySlots = new[] { 0 }
            },
            new HallReturnNeed
            {
                StackKey = "soil",
                Quantity = 3,
                StackLimit = 20,
                AllowedEmptySlots = new[] { 0 }
            }
        };

        // Individually both fit into slot 0, together they require 2 distinct slots.
        Assert.False(HallInventoryPlanner.CanFitAll(oneFreeSlot, rackAndContents));

        // Partial existing rack stack + 1 empty slot allows both items.
        var onePartialOneEmpty = new[]
        {
            new HallSlotBudget { Index = 0, IsEmpty = false, Quantity = 2, Capacity = 10 },
            new HallSlotBudget { Index = 1, IsEmpty = true }
        };
        var withExistingRackStack = new[]
        {
            new HallReturnNeed
            {
                StackKey = "storagerackmedium",
                Quantity = 1,
                StackLimit = 10,
                ExistingStackCapacities = new Dictionary<int, int> { [0] = 8 },
                AllowedEmptySlots = new[] { 1 }
            },
            new HallReturnNeed
            {
                StackKey = "soil",
                Quantity = 3,
                StackLimit = 20,
                AllowedEmptySlots = new[] { 1 }
            }
        };
        Assert.True(HallInventoryPlanner.CanFitAll(onePartialOneEmpty, withExistingRackStack));

        // Two compatible stacks from storage share one empty slot if total <= StackLimit.
        var sharedStackSameKey = new[]
        {
            new HallReturnNeed
            {
                StackKey = "ogkush|Standard|",
                Quantity = 12,
                StackLimit = 20,
                AllowedEmptySlots = new[] { 0 }
            },
            new HallReturnNeed
            {
                StackKey = "ogkush|Standard|",
                Quantity = 8,
                StackLimit = 20,
                AllowedEmptySlots = new[] { 0 }
            }
        };
        Assert.True(HallInventoryPlanner.CanFitAll(oneFreeSlot, sharedStackSameKey));

        // Different qualities cannot stack onto the same newly claimed slot.
        var mixedQualities = new[]
        {
            new HallReturnNeed
            {
                StackKey = "ogkush|Standard|",
                Quantity = 12,
                StackLimit = 20,
                AllowedEmptySlots = new[] { 0 }
            },
            new HallReturnNeed
            {
                StackKey = "ogkush|Premium|",
                Quantity = 8,
                StackLimit = 20,
                AllowedEmptySlots = new[] { 0 }
            }
        };
        Assert.False(HallInventoryPlanner.CanFitAll(oneFreeSlot, mixedQualities));
    }

    [Fact]
    public void HallSlotRecord_SupportsOptionalItemJsonWhileRejectingOversizedPayloads()
    {
        var withMetadata = new HallPlacementRecord
        {
            Id = "p2",
            ItemId = "storagerackmedium",
            X = HallPlan.CenterX,
            Y = HallPlan.FloorTopY,
            Z = HallPlan.CenterZ,
            RotW = 1f,
            Contents = new List<HallSlotRecord>
            {
                new() { Slot = 0, ItemId = "ogkush", Quantity = 5, ItemJson = "{\"ID\":\"ogkush\",\"Quantity\":5,\"Quality\":\"Heavenly\"}" }
            }
        };
        Assert.True(withMetadata.IsPlausible());

        withMetadata.Contents[0].ItemJson = new string('x', 4097);
        Assert.False(withMetadata.IsPlausible());
    }

    [Fact]
    public void HallListing_MatchesWholeWordRvAndRejectsSubstringWords()
    {
        Assert.True(HallListing.IsPropertyMenu("rv", "RV"));
        Assert.True(HallListing.IsPropertyMenu("buy_rv", "Buy the RV"));
        Assert.False(HallListing.IsPropertyMenu("service_info", "What services do you offer?"));
        Assert.False(HallListing.IsPropertyMenu("reserve", "Reserve a spot"));
        Assert.False(HallListing.IsPropertyMenu("survey", "Take a survey"));
    }

    [Fact]
    public void HallPickupPrompt_MatchesPickupPromptsAndNeverHijacksUnrelatedPrompts()
    {
        Assert.True(HallPickupPrompt.IsPickupMessage("Pick up"));
        Assert.True(HallPickupPrompt.IsPickupMessage("Must be empty"));
        Assert.True(HallPickupPrompt.IsPickupMessage("Won't fit in inventory"));
        Assert.True(HallPickupPrompt.IsPickupMessage("Doesn't fit in inventory"));
        Assert.True(HallPickupPrompt.IsPickupMessage("Inhalt geschuetzt"));
        Assert.True(HallPickupPrompt.IsPickupMessage("Lager ist offen"));
        Assert.True(HallPickupPrompt.IsPickupMessage("Belegt"));
        Assert.True(HallPickupPrompt.IsPickupMessage("In Benutzung"));

        Assert.False(HallPickupPrompt.IsPickupMessage("Empty trash"));
        Assert.False(HallPickupPrompt.IsPickupMessage("Empty pot"));
        Assert.False(HallPickupPrompt.IsPickupMessage("Open"));
        Assert.False(HallPickupPrompt.IsPickupMessage("Change outfit"));
        Assert.False(HallPickupPrompt.IsPickupMessage("Check profit"));
        Assert.False(HallPickupPrompt.IsPickupMessage(null));

        Assert.True(HallPickupPrompt.IsEmptyOnlyBlockReason("Must be empty"));
        Assert.True(HallPickupPrompt.IsEmptyOnlyBlockReason("Contains items"));
        Assert.False(HallPickupPrompt.IsEmptyOnlyBlockReason("Currently in use"));
        Assert.False(HallPickupPrompt.IsEmptyOnlyBlockReason("Plant is growing"));
        Assert.False(HallPickupPrompt.IsEmptyOnlyBlockReason(""));
    }

    [Fact]
    public void HallWalletCheck_HandlesHighFloatBalancesAndRejectsMismatch()
    {
        Assert.True(HallWalletCheck.MatchesExpected(50000f, -45000f, 5000f));
        Assert.True(HallWalletCheck.MatchesExpected(2_000_000f, -45000f, 1_955_000f));
        Assert.True(HallWalletCheck.MatchesExpected(2_000_000f, -45000f, 1_955_000.125f));
        Assert.False(HallWalletCheck.MatchesExpected(50000f, -45000f, 50000f));
        Assert.False(HallWalletCheck.MatchesExpected(2_000_000f, -45000f, 1_954_990f));
        Assert.False(HallWalletCheck.MatchesExpected(float.NaN, -45000f, 0f));
    }

    [Fact]
    public void HallSaveRules_SkipsCleanUnownedSavesAndBlocksCorruptStates()
    {
        Assert.False(HallSaveRules.ShouldWriteSave(owned: false, HallSaveKind.Missing, placementCount: 0));
        Assert.True(HallSaveRules.ShouldWriteSave(owned: true, HallSaveKind.Missing, placementCount: 0));
        Assert.True(HallSaveRules.ShouldWriteSave(owned: false, HallSaveKind.Ready, placementCount: 0));
        Assert.False(HallSaveRules.ShouldWriteSave(owned: true, HallSaveKind.Corrupt, placementCount: 1));
        Assert.False(HallSaveRules.ShouldWriteSave(owned: true, HallSaveKind.ForeignSlot, placementCount: 1));
        Assert.False(HallSaveRules.ShouldWriteSave(owned: true, HallSaveKind.Unsupported, placementCount: 1));
    }

    [Fact]
    public void HallInventoryPlanner_RespectsAllowedEmptySlotsWhenReusingClaimedSlot()
    {
        var twoEmpty = new[]
        {
            new HallSlotBudget { Index = 0, IsEmpty = true },
            new HallSlotBudget { Index = 1, IsEmpty = true }
        };

        var restrictedSecond = new[]
        {
            new HallReturnNeed
            {
                StackKey = "soil",
                Quantity = 5,
                StackLimit = 20,
                AllowedEmptySlots = new[] { 0 }
            },
            new HallReturnNeed
            {
                StackKey = "soil",
                Quantity = 5,
                StackLimit = 20,
                AllowedEmptySlots = Array.Empty<int>()
            }
        };

        // Second need does not allow slot 0, so it must not stack onto the slot claimed by the first need.
        Assert.False(HallInventoryPlanner.CanFitAll(twoEmpty, restrictedSecond));
    }

    [Fact]
    public void HallWeatherEnclosure_ContainsAllGridTilesAndExcludesShopSpawn()
    {
        foreach (var tile in HallPlan.Tiles(HallPlan.GridTile))
            Assert.True(HallPlan.ContainsWeatherPoint(tile.WorldX, HallPlan.FloorTopY + 1f, tile.WorldZ));

        Assert.True(HallPlan.ContainsWeatherPoint(HallPlan.InteriorSpawnX, HallPlan.FloorTopY + 1f, HallPlan.InteriorSpawnZ));

        // Outside west into Hank's shop, NPC spawn, beyond east/south/north walls, underground, above ridge, and NaN.
        Assert.False(HallPlan.ContainsWeatherPoint(HallPlan.SpawnX, HallPlan.FloorTopY + 1f, HallPlan.SpawnZ));
        Assert.False(HallPlan.ContainsWeatherPoint(HallPlan.NpcSpawnX, HallPlan.FloorTopY + 1f, HallPlan.NpcSpawnZ));
        Assert.False(HallPlan.ContainsWeatherPoint(HallPlan.CenterX + HallPlan.WidthX * 0.5f + 0.1f, HallPlan.FloorTopY + 1f, HallPlan.CenterZ));
        Assert.False(HallPlan.ContainsWeatherPoint(HallPlan.CenterX, HallPlan.FloorTopY + 1f, HallPlan.CenterZ - HallPlan.LengthZ * 0.5f - 0.1f));
        Assert.False(HallPlan.ContainsWeatherPoint(HallPlan.CenterX, HallPlan.FloorTopY + 1f, HallPlan.CenterZ + HallPlan.LengthZ * 0.5f + 0.1f));
        Assert.False(HallPlan.ContainsWeatherPoint(HallPlan.CenterX, -0.5f, HallPlan.CenterZ));
        Assert.False(HallPlan.ContainsWeatherPoint(HallPlan.CenterX, 8.5f, HallPlan.CenterZ));
        Assert.False(HallPlan.ContainsWeatherPoint(float.NaN, HallPlan.FloorTopY + 1f, HallPlan.CenterZ));
    }

    [Fact]
    public void HallWallSurfaces_HaveUniqueValidGuidsDistinctFromGridAndAvoidDoorwayAndSavePoint()
    {
        var surfaces = HallPlan.WallSurfaces();
        Assert.Equal(5, surfaces.Count);

        var gridGuid = Guid.Parse(HallPlan.GridGuidString);
        var seenGuids = new HashSet<Guid> { gridGuid };
        const float doorMinZ = 21.86f;
        const float doorMaxZ = 23.14f;
        const float saveZ = 21.40f;

        foreach (var s in surfaces)
        {
            Assert.False(string.IsNullOrWhiteSpace(s.Name));
            Assert.True(Guid.TryParse(s.Guid, out var parsed));
            Assert.NotEqual(Guid.Empty, parsed);
            Assert.True(seenGuids.Add(parsed));

            Assert.True(s.Width > 1f);
            Assert.True(s.Height > 2f);
            Assert.True(HallPlan.ContainsPlacement(s.CenterX, s.CenterY, s.CenterZ));

            float normalMag = MathF.Sqrt(s.NormalX * s.NormalX + s.NormalZ * s.NormalZ);
            Assert.Equal(1f, normalMag, 4);
            Assert.True(MathF.Abs(s.NormalX) < 0.001f || MathF.Abs(s.NormalZ) < 0.001f);

            if (s.NormalX > 0.5f)
            {
                float minZ = s.CenterZ - s.Width * 0.5f;
                float maxZ = s.CenterZ + s.Width * 0.5f;
                Assert.True(maxZ < doorMinZ || minZ > doorMaxZ);
                Assert.True(maxZ < saveZ - 0.15f || minZ > saveZ + 0.15f);
            }
        }
    }

    [Fact]
    public void HallPlacementRecord_AcceptsWallSurfacePoseAndNativeDynamicSaveDataAndRejectsInvalidPayloads()
    {
        static HallPlacementRecord WallValid() => new()
        {
            Id = "a1b2c3d4-2222-4a2b-9c3d-000000000099",
            ItemId = "airconditioner",
            X = HallPlan.CenterX - HallPlan.WidthX * 0.5f + HallPlan.WallSurfaceInset,
            Y = 4.5f,
            Z = 28.0f,
            RotW = 1f,
            NativeDataType = "PotData",
            NativeDataJson = "{\"DataType\":\"PotData\",\"BaseData\":\"{}\"}",
            LoadOrder = 0
        };

        Assert.True(WallValid().IsPlausible());

        var procOrder = WallValid();
        procOrder.LoadOrder = 1;
        Assert.True(procOrder.IsPlausible());

        var missingType = WallValid();
        missingType.NativeDataType = "   ";
        Assert.False(missingType.IsPlausible());

        var oversizedJson = WallValid();
        oversizedJson.NativeDataJson = new string('x', HallPlacementRecord.MaxNativeDataJsonLength + 1);
        Assert.False(oversizedJson.IsPlausible());

        var traversalType = WallValid();
        traversalType.NativeDataType = "../BadType";
        Assert.False(traversalType.IsPlausible());

        var badLoadOrderLow = WallValid();
        badLoadOrderLow.LoadOrder = -101;
        Assert.False(badLoadOrderLow.IsPlausible());

        var badLoadOrderHigh = WallValid();
        badLoadOrderHigh.LoadOrder = 1001;
        Assert.False(badLoadOrderHigh.IsPlausible());

        var aboveRoof = WallValid();
        aboveRoof.Y = 7.5f;
        Assert.False(aboveRoof.IsPlausible());

        var outsideWall = WallValid();
        outsideWall.X = 108.5f;
        Assert.False(outsideWall.IsPlausible());
    }

    [Fact]
    public void HallPlacementOrdering_SortsByLoadOrderAscendingStably()
    {
        var r1 = new HallPlacementRecord { Id = "light-1", LoadOrder = 2 };
        var r2 = new HallPlacementRecord { Id = "pot-1", LoadOrder = 0 };
        var r3 = new HallPlacementRecord { Id = "rack-1", LoadOrder = 1 };
        var r4 = new HallPlacementRecord { Id = "pot-2", LoadOrder = 0 };

        var sorted = HallPlacementOrdering.SortForRestore(new HallPlacementRecord[] { r1, r2, r3, r4 });
        Assert.Equal(4, sorted.Count);
        Assert.Equal("pot-1", sorted[0].Id);
        Assert.Equal("pot-2", sorted[1].Id);
        Assert.Equal("rack-1", sorted[2].Id);
        Assert.Equal("light-1", sorted[3].Id);

        Assert.Empty(HallPlacementOrdering.SortForRestore(null));
        Assert.Empty(HallPlacementOrdering.SortForRestore(Array.Empty<HallPlacementRecord>()));
    }

    [Fact]
    public void HallDynamicDataPayload_ParsesValidNativeJsonAndRejectsMissingBaseData()
    {
        const string validJson = """
            {
              "DataType": "PotData",
              "DataVersion": 0,
              "GameVersion": "0.4.7f12",
              "BaseData": "{\"ID\":\"plasticpot\"}",
              "AdditionalDatas": [
                { "Name": "Soil", "Contents": "{}" }
              ]
            }
            """;

        var parsed = HallDynamicDataPayload.TryParse(validJson);
        Assert.NotNull(parsed);
        Assert.Equal("PotData", parsed!.DataType);
        Assert.Equal("{\"ID\":\"plasticpot\"}", parsed.BaseData);
        Assert.Single(parsed.AdditionalDatas!);
        Assert.Equal("Soil", parsed.AdditionalDatas![0].Name);

        Assert.Null(HallDynamicDataPayload.TryParse(null));
        Assert.Null(HallDynamicDataPayload.TryParse(""));
        Assert.Null(HallDynamicDataPayload.TryParse("{not-json"));
        Assert.Null(HallDynamicDataPayload.TryParse("{\"DataType\":\"PotData\"}"));
        Assert.Null(HallDynamicDataPayload.TryParse("{\"DataType\":\"PotData\",\"BaseData\":\"\"}"));
    }
}
