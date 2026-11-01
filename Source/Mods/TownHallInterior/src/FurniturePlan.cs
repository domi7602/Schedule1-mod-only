using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace TownHallInterior;

public sealed record FurniturePart
{
    public string SourcePath { get; init; } = "";
    public string MeshName { get; init; } = "";
    public int SourceVertexCount { get; init; }
    public int FirstSubMesh { get; init; }
    public bool IsStaticBatch { get; init; }
    public double[] SourceBoundsMin { get; init; } = Array.Empty<double>();
    public double[] SourceBoundsMax { get; init; } = Array.Empty<double>();
    public string[] MaterialNames { get; init; } = Array.Empty<string>();
    public double[][] Vertices { get; init; } = Array.Empty<double[]>();
    public int[][] Submeshes { get; init; } = Array.Empty<int[]>();
}

public sealed record FurnitureAsset
{
    public string Key { get; init; } = "";
    public double[] BoundsMin { get; init; } = Array.Empty<double>();
    public double[] BoundsMax { get; init; } = Array.Empty<double>();
    public FurniturePart[] Parts { get; init; } = Array.Empty<FurniturePart>();
}

public sealed record FurnitureDocument
{
    public int Schema { get; init; }
    public string LevelSha256 { get; init; } = "";
    public FurnitureAsset[] Assets { get; init; } = Array.Empty<FurnitureAsset>();
}

public sealed record FurniturePlacement(string Name, string Key, float X, float Y, float Z, float Yaw,
    bool Solid, float Scale = 1f);
public sealed record InteriorLight(string Name, float X, float Y, float Z, float Range, float Intensity);

// Pure decorative layout. Accepted floor, stairs and entrance collision stay separate.
public static class FurniturePlan
{
    public static bool RequiresStableBounds(string key, FurniturePart part)
    {
        // Only material identity is borrowed from the two potentially moving hands.
        const string clock = "@Businesses/PostOffice/Interior/Wall Clock/";
        bool hand = key == "clock" && (part.SourcePath == clock + "Min/MinuteHand" || part.SourcePath == clock + "Hour/HourHand");
        return key == "door-frame" || (part.IsStaticBatch && !hand);
    }

    public static IReadOnlyList<FurniturePlacement> Create(FurnitureDocument document)
    {
        var required = new[] { "desk", "chair", "cabinet", "computer", "plant", "bin", "clock", "door-frame" };
        if (required.Any(key => document.Assets.Count(a => a.Key == key) != 1))
            throw new InvalidOperationException("Reception asset set is incomplete");
        float floor = PublicEntrance.FloorTop;
        float desktop = floor + (float)document.Assets.Single(a => a.Key == "desk").BoundsMax[1];
        var p = new List<FurniturePlacement>();
        void Add(string name, string key, float x, float y, float z, float yaw,
            bool solid = true, float scale = 1f) =>
            p.Add(new FurniturePlacement(name, key, x, y, z, yaw, solid, scale));
        for (int i = 0; i < 2; i++)
        {
            float z = 33.8f + i * 2.2f;
            Add("reception-desk-" + i, "desk", 60f, floor, z, 0);      // knee space faces the staff side (+X)
            Add("staff-chair-" + i, "chair", 61.2f, floor, z - .35f, 180);
            Add("reception-computer-" + i, "computer", 60f, desktop, z - .35f, 180, false);
            Add("reception-plant-" + i, "plant", 60f, desktop, z + .55f, 0, false);
        }
        // The waiting zone is furnished with built-in benches (HallFittings); the eight loose
        // chairs it replaced would read as a canteen. Two scaled plants frame the west corners.
        Add("corner-plant-south", "plant", 55.45f, floor, 21.95f, 0, false, 1.8f);
        Add("corner-plant-north", "plant", 55.45f, floor, 40f, 0, false, 1.8f);
        Add("files-north-a", "cabinet", 61.8f, floor, 38.8f, 180);
        Add("files-north-b", "cabinet", 61.8f, floor, 39.7f, 180);
        Add("forms-desk", "desk", 56.8f, floor, 39.4f, 90);
        Add("forms-chair-a", "chair", 56.2f, floor, 38f, 270);
        Add("forms-chair-b", "chair", 57.3f, floor, 38f, 270);
        Add("forms-plant", "plant", 56.3f, desktop, 39.4f, 0, false);
        Add("waiting-bin", "bin", 56f, floor, 28.8f, 0);
        Add("reception-bin", "bin", 61.8f, floor, 32.5f, 0);
        // ---- Obergeschoss: Büros, Besprechung, Archiv -------------------------------------
        float upper = RoomPlan.Upper;                                  // 4.75, design coordinate
        float upperDesktop = upper + (float)document.Assets.Single(a => a.Key == "desk").BoundsMax[1];

        // Drei Büroarbeitsplätze an der Westwand. Das Pult ist 1 m breit und 2 m tief; seine
        // offene Sitzseite liegt lokal bei +X, bei yaw 0 also nach Osten in den Raum.
        for (int i = 0; i < 3; i++)
        {
            float z = 30.6f + i * 2.6f;
            Add("office-desk-" + i, "desk", 56.4f, upper, z, 0);
            Add("office-chair-" + i, "chair", 57.9f, upper, z, 180);   // faces -X, into the knee space
            Add("office-computer-" + i, "computer", 56.4f, upperDesktop, z - 0.35f, 0, false);
            Add("office-plant-" + i, "plant", 56.4f, upperDesktop, z + 0.55f, 0, false);
        }

        // Besprechungstisch aus zwei gespiegelten Pulten, je ein Stuhl auf der offenen Seite.
        Add("meeting-desk-a", "desk", 59.2f, upper, 36.4f, 0);
        Add("meeting-desk-b", "desk", 59.2f, upper, 38.4f, 180); // mirrored, knee space faces -X
        Add("meeting-chair-b", "chair", 58.1f, upper, 38.4f, 0);    // faces +X into b's knee space
        Add("meeting-chair-c", "chair", 60.3f, upper, 36.4f, 180);  // faces -X into a's knee space

        // Archiv an der Ostwand, nördlich des Treppenlochs.
        for (int i = 0; i < 5; i++)
            Add("archive-cabinet-" + i, "cabinet", 62.6f, upper, 31.6f + i * 1.05f, 270);

        // Südgruppe: Schrankzeile an der Südwand, zwei Stühle, Pflanzen, Papierkorb.
        for (int i = 0; i < 4; i++)
            Add("south-cabinet-" + i, "cabinet", 56.3f + i * 0.95f, upper, 21.05f, 0);
        Add("south-chair-a", "chair", 60.15f, upper, 21.9f, 270);   // faces +Z, into the room
        Add("south-chair-b", "chair", 61.15f, upper, 21.9f, 270);
        Add("south-bin", "bin", 61.9f, upper, 22.3f, 0);
        Add("south-plant-a", "plant", 55.15f, upper, 21.15f, 0, false, 1.6f);
        Add("south-plant-b", "plant", 62.50f, upper, 21.15f, 0, false, 1.6f);

        // Zweiter Monitor-Satz für den Besprechungstisch.
        Add("meeting-computer-a", "computer", 59.2f, upperDesktop, 36.4f, 0, false);
        Add("meeting-computer-b", "computer", 59.2f, upperDesktop, 38.4f, 0, false);

        Add("upper-plant-north", "plant", 55.15f, upper, 40.5f, 0, false, 1.6f);
        Add("upper-bin", "bin", 55.15f, upper, 30.0f, 0);
        Add("upper-clock", "clock", 58.7f, 7.25f, 20.78f, 0, false);

        Add("hall-clock", "clock", 58.8f, 3.3f, 40.377f, 180, false);
        Add("raised-native-frame", "door-frame", 54.3f, (float)FacadePortal.Minimum[1], 31f, 0, false);
        return p;
    }

    public static FurnitureAsset PrepareAsset(FurnitureAsset asset)
    {
        if (asset.Key != "door-frame") return asset;
        // Exact pinned frame: old inner header Y=2.1 and inner half-width Z=.525.
        // Translate the header and side columns; preserve moulding thickness and UVs.
        double rise = PublicEntrance.PortalTop - FacadePortal.Minimum[1] - 2.1;
        double widen = (FacadePortal.Maximum[2] - FacadePortal.Minimum[2]) / 2 + .01 - .525;
        var parts = asset.Parts.Select(p => p with
        {
            Vertices = p.Vertices.Select(v =>
        {
            var copy = (double[])v.Clone();
            if (copy[1] > 1) copy[1] += rise;
            copy[2] += Math.Sign(copy[2]) * widen;
            return copy;
        }).ToArray()
        }).ToArray();
        var vertices = parts.SelectMany(p => p.Vertices).ToArray();
        return asset with
        {
            Parts = parts,
            BoundsMin = Enumerable.Range(0, 3).Select(i => vertices.Min(v => v[i])).ToArray(),
            BoundsMax = Enumerable.Range(0, 3).Select(i => vertices.Max(v => v[i])).ToArray()
        };
    }

    public static FurnitureDocument Read(byte[] bytes)
    {
        if (bytes.Length > 12_000_000) throw new InvalidOperationException("Furniture byte budget exceeded");
        var document = JsonSerializer.Deserialize<FurnitureDocument>(bytes,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
            ?? throw new InvalidOperationException("Furniture document missing");
        if (document.Schema != 1 || document.LevelSha256 != FacadePortal.LevelSha256 ||
            document.Assets == null || document.Assets.Length is < 1 or > 12)
            throw new InvalidOperationException("Unrecognized furniture extraction or game version");
        var keys = new HashSet<string>(StringComparer.Ordinal);
        int vertexCount = 0;
        foreach (var asset in document.Assets)
        {
            if (asset == null || string.IsNullOrWhiteSpace(asset.Key) || !keys.Add(asset.Key) ||
                asset.Parts == null || asset.Parts.Length is < 1 or > 12)
                throw new InvalidOperationException("Invalid furniture asset identity/parts");
            CheckBounds(asset.BoundsMin, asset.BoundsMax);
            var paths = new HashSet<string>(StringComparer.Ordinal);
            foreach (var part in asset.Parts)
            {
                if (part == null || string.IsNullOrWhiteSpace(part.SourcePath) ||
                    part.SourcePath.Split('/').Any(s => string.IsNullOrWhiteSpace(s) || s is "." or "..") ||
                    !paths.Add(part.SourcePath) || string.IsNullOrWhiteSpace(part.MeshName) ||
                    part.SourceVertexCount < 3 || part.FirstSubMesh < 0)
                    throw new InvalidOperationException("Invalid furniture source identity");
                FacadePortal.ValidateMesh(new FacadeMesh { Vertices = part.Vertices, Submeshes = part.Submeshes });
                CheckBounds(part.SourceBoundsMin, part.SourceBoundsMax);
                if (part.MaterialNames == null || part.MaterialNames.Length != part.Submeshes.Length ||
                    part.MaterialNames.Any(string.IsNullOrWhiteSpace) || part.Submeshes.All(s => s.Length == 0))
                    throw new InvalidOperationException("Invalid furniture material slots");
                vertexCount += part.Vertices.Length;
                if (vertexCount > 30000) throw new InvalidOperationException("Furniture vertex budget exceeded");
            }
            var vertices = asset.Parts.SelectMany(p => p.Vertices).ToArray();
            for (int axis = 0; axis < 3; axis++)
            {
                if (Math.Abs(vertices.Min(v => v[axis]) - asset.BoundsMin[axis]) > .0001 ||
                    Math.Abs(vertices.Max(v => v[axis]) - asset.BoundsMax[axis]) > .0001 ||
                    asset.BoundsMax[axis] - asset.BoundsMin[axis] > 6)
                    throw new InvalidOperationException("Furniture geometry/bounds mismatch: " + asset.Key);
            }
            if (Math.Abs(asset.BoundsMin[1]) > .0001 ||
                Math.Abs(asset.BoundsMin[0] + asset.BoundsMax[0]) > .0001 ||
                Math.Abs(asset.BoundsMin[2] + asset.BoundsMax[2]) > .0001)
                throw new InvalidOperationException("Furniture pivot must be bottom center: " + asset.Key);
        }
        return document;
    }

    private static void CheckBounds(double[] min, double[] max)
    {
        if (min == null || max == null || min.Length != 3 || max.Length != 3)
            throw new InvalidOperationException("Missing furniture bounds");
        for (int i = 0; i < 3; i++)
            if (!double.IsFinite(min[i]) || !double.IsFinite(max[i]) || max[i] < min[i])
                throw new InvalidOperationException("Invalid furniture bounds");
    }

    public static IReadOnlyList<InteriorLight> CreateLights()
    {
        var lights = new List<InteriorLight>();
        foreach (float y in new[] { 4.15f, 7.8f })
            foreach (float z in new[] { 24f, 28.5f, 33.5f, 38f })
                lights.Add(new InteriorLight((y < RoomPlan.Upper ? "reception-" : "upper-") + lights.Count,
                    58.7f, y, z, 5f, 1.1f));
        return lights;
    }

    public static IReadOnlyList<RoomBox> CreateShell()
    {
        var boxes = new List<RoomBox>();
        var skin = PublicEntrance.CreateInterior();
        var west = skin.Single(b => b.Name == "inner-west-south");
        var east = skin.Single(b => b.Name == "inner-east");
        var floor = skin.Single(b => b.Name == "public-floor");
        VisualBox(boxes, "floor-edge-west", west.X + west.Width / 2, floor.Y - floor.Height / 2, floor.Z - floor.Depth / 2,
            floor.X - floor.Width / 2, PublicEntrance.FloorTop, floor.Z + floor.Depth / 2, 0);
        VisualBox(boxes, "floor-edge-east", floor.X + floor.Width / 2, floor.Y - floor.Height / 2, floor.Z - floor.Depth / 2,
            east.X - east.Width / 2, PublicEntrance.FloorTop, floor.Z + floor.Depth / 2, 0);
        foreach (var slab in RoomPlan.Create().Where(b => b.Name.StartsWith("upper-")))
        {
            var x1 = Math.Max(west.X + west.Width / 2, slab.X - slab.Width / 2);
            var x2 = Math.Min(east.X - east.Width / 2, slab.X + slab.Width / 2);
            var z1 = Math.Max(21.6f, slab.Z - slab.Depth / 2);
            var z2 = Math.Min(40.4f, slab.Z + slab.Depth / 2);
            boxes.Add(new RoomBox("ceiling-" + slab.Name, (x1 + x2) / 2, (4.4f + RoomPlan.Upper) / 2, (z1 + z2) / 2,
                x2 - x1, RoomPlan.Upper - 4.4f, z2 - z1, false, 1));
        }
        foreach (var wall in PublicEntrance.CreateInterior().Where(b => b.Name.StartsWith("inner-") && b.Name != "inner-lintel"))
            boxes.Add(wall with { Name = "crown-" + wall.Name, Y = 4.34f, Height = .28f, Solid = false });
        foreach (var wall in skin.Where(b => b.Name is "inner-south" or "inner-north"))
        {
            float z1 = wall.Z - wall.Depth / 2, z2 = wall.Z + wall.Depth / 2;
            VisualBox(boxes, "corner-west-" + wall.Name, west.X + west.Width / 2, 1.5f, z1,
                wall.X - wall.Width / 2, 4.48f, z2, 2);
            VisualBox(boxes, "corner-east-" + wall.Name, wall.X + wall.Width / 2, 1.5f, z1,
                east.X - east.Width / 2, 4.48f, z2, 2);
        }
        // Slightly overlap frame backs; offset exposed wall faces and bury tops below the floor
        // to avoid coplanar surfaces. These small overlaps do not enter the accepted portal.
        VisualBox(boxes, "reveal-south", PublicEntrance.LiningFrontX, 1.41f, 29.95f, 54.951f, RoomPlan.Upper - .001f, 30.02f, 2);
        VisualBox(boxes, "reveal-north", PublicEntrance.LiningFrontX, 1.41f, 31.98f, 54.951f, RoomPlan.Upper - .001f, 32.05f, 2);
        // Finish the existing upper support footprint; the entry header fills the west-center notch.
        // This leaves the full-height portal and the existing stair void untouched.
        VisualBox(boxes, "floor-rim-west-south", 54.55f, 4.4f, 20.7f, 54.95f, RoomPlan.Upper, 30.04f, 1);
        VisualBox(boxes, "floor-rim-west-north", 54.55f, 4.4f, 31.98f, 54.95f, RoomPlan.Upper, 41.3f, 1);
        VisualBox(boxes, "floor-rim-east", 62.45f, 4.4f, 20.7f, 63.15f, RoomPlan.Upper, 41.3f, 1);
        VisualBox(boxes, "floor-rim-south", 54.95f, 4.4f, 20.7f, 62.45f, RoomPlan.Upper, 21.6f, 1);
        VisualBox(boxes, "floor-rim-north", 54.95f, 4.4f, 40.4f, 62.45f, RoomPlan.Upper, 41.3f, 1);
        // Thin inward skins follow the EXISTING upper perimeter collision, not the narrower EG room.
        // A deeply inset render-only wall would let the player walk through it before hitting collision.
        const float roofBottom = 8.05f, roofTop = 8.13f; // Chosen interior design heights.
        VisualBox(boxes, "upper-skin-west", 54.55f, RoomPlan.Upper, 20.7f, 54.57f, roofTop, 41.3f, 2);
        VisualBox(boxes, "upper-skin-east", 63.13f, RoomPlan.Upper, 20.7f, 63.15f, roofTop, 41.3f, 2);
        VisualBox(boxes, "upper-skin-south", 54.55f, RoomPlan.Upper, 20.7f, 63.15f, roofTop, 20.72f, 2);
        VisualBox(boxes, "upper-skin-north", 54.55f, RoomPlan.Upper, 41.28f, 63.15f, roofTop, 41.3f, 2);
        VisualBox(boxes, "upper-ceiling", 54.55f, roofBottom, 20.7f, 63.15f, roofTop, 41.3f, 1);
        return boxes;
    }

    private static void VisualBox(List<RoomBox> boxes, string name,
        float x1, float y1, float z1, float x2, float y2, float z2, int style)
    {
        if (!(x2 > x1 && y2 > y1 && z2 > z1)) throw new InvalidOperationException("Invalid visual enclosure: " + name);
        boxes.Add(new RoomBox(name, (x1 + x2) / 2, (y1 + y2) / 2, (z1 + z2) / 2,
            x2 - x1, y2 - y1, z2 - z1, false, style));
    }
}
