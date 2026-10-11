using System;
using System.Collections.Generic;

namespace PropertyLEDs.Core
{
    public sealed class LEDPoint
    {
        public LEDPoint() { }
        public LEDPoint(float x, float y, float z) { X = x; Y = y; Z = z; }
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
    }

    public sealed class LEDPath
    {
        public bool Closed { get; set; }
        public List<LEDPoint> Points { get; set; } = new List<LEDPoint>();

        public bool Validate(out string error)
        {
            if (Points == null || Points.Count < (Closed ? 3 : 2) || Points.Count > 4096)
            { error = "Path needs 2 open/3 closed vertices, at most 4096."; return false; }
            foreach (var point in Points)
            {
                if (point == null || !float.IsFinite(point.X) || !float.IsFinite(point.Y) || !float.IsFinite(point.Z) ||
                    Math.Abs(point.X) > 10000 || Math.Abs(point.Y) > 10000 || Math.Abs(point.Z) > 10000)
                { error = "Path contains null, non-finite or out-of-bounds coordinates."; return false; }
            }
            int segments = Closed ? Points.Count : Points.Count - 1;
            for (int i = 0; i < segments; i++)
            {
                var a = Points[i]; var b = Points[(i + 1) % Points.Count];
                double x = (double)b.X - a.X, y = (double)b.Y - a.Y, z = (double)b.Z - a.Z;
                if (x * x + y * y + z * z < 1e-10)
                { error = "Consecutive vertices (including the closing edge) must be distinct."; return false; }
            }
            int firstJoin = Closed ? 0 : 1;
            int lastJoin = Closed ? Points.Count : Points.Count - 1;
            for (int i = firstJoin; i < lastJoin; i++)
            {
                var a = Points[(i + Points.Count - 1) % Points.Count];
                var b = Points[i]; var c = Points[(i + 1) % Points.Count];
                double ax = (double)b.X - a.X, ay = (double)b.Y - a.Y, az = (double)b.Z - a.Z;
                double bx = (double)c.X - b.X, by = (double)c.Y - b.Y, bz = (double)c.Z - b.Z;
                double cosine = (ax * bx + ay * by + az * bz) /
                    Math.Sqrt((ax * ax + ay * ay + az * az) * (bx * bx + by * by + bz * bz));
                if (cosine < -0.9999)
                { error = "A near-180-degree reversal is not a valid path."; return false; }
            }
            error = "";
            return true;
        }
    }

    public sealed class RoomLayout
    {
        public string PropertyId { get; set; } = "";
        public string RoomId { get; set; } = "";
        public string DisplayName { get; set; } = "";
        public string AnchorPath { get; set; } = "";
        public bool Verified { get; set; }
        public float Width { get; set; } = 0.035f;
        public List<LEDPath> Paths { get; set; } = new List<LEDPath>();

        public bool Validate(out string error)
        {
            if (!Verified) { error = "Layout has not been measured/verified."; return false; }
            if (string.IsNullOrWhiteSpace(PropertyId) || string.IsNullOrWhiteSpace(RoomId) ||
                string.IsNullOrWhiteSpace(DisplayName) || string.IsNullOrWhiteSpace(AnchorPath))
            { error = "Room identifiers, display name and anchor path are required."; return false; }
            if (!float.IsFinite(Width) || Width < 0.001f || Width > 1f)
            { error = "Beam width must be finite and between 0.001 and 1 metres."; return false; }
            if (Paths == null || Paths.Count == 0 || Paths.Count > 256 || Paths.Contains(null!))
            { error = "Room must have 1..256 non-null paths."; return false; }
            foreach (var path in Paths)
                if (!path.Validate(out error)) return false;
            error = "";
            return true;
        }
    }

    public sealed class LayoutCatalog
    {
        public const int CurrentFormatVersion = 1;
        public int FormatVersion { get; set; } = CurrentFormatVersion;
        public List<RoomLayout> Rooms { get; set; } = new List<RoomLayout>();

        /// <summary>Validation never mutates input, including unsupported versions.</summary>
        public bool Validate(out string error)
        {
            if (FormatVersion != CurrentFormatVersion)
            { error = "Unsupported layout format version; preserve the original file."; return false; }
            if (Rooms == null || Rooms.Count > 1024)
            { error = "Catalog rooms are null or exceed 1024."; return false; }
            var keys = new HashSet<(string, string)>();
            foreach (var room in Rooms)
            {
                if (room == null) { error = "Catalog contains a null room."; return false; }
                if (!room.Validate(out error)) return false;
                if (!keys.Add((room.PropertyId, room.RoomId)))
                { error = "Duplicate property/room identity."; return false; }
            }
            error = "";
            return true;
        }
    }
}
