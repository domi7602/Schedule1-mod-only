using System.Collections.Generic;

namespace TownHallInterior;

// Built-in public-hall fittings: info counter opposite the entrance, a waiting bench row along
// the south wall, a runner and the notice board. Pure geometry, so tests can check fit,
// collision and that the accepted portal and stair voids stay free.
public static class HallFittings
{
    // Style indices of the shared hall paints (0..2 are the existing shell colours).
    public const int DarkStyle = 3, LightStyle = 1, RugStyle = 4;

    public const float CounterHeight = 2.50f;
    public const float BenchSeatTop = 1.96f;

    public static IReadOnlyList<RoomBox> Create()
    {
        float floor = PublicEntrance.FloorTop;
        var boxes = new List<RoomBox>();

        void Box(string name, float x1, float y1, float z1, float x2, float y2, float z2,
            bool solid, int style) =>
            boxes.Add(new RoomBox(name, (x1 + x2) / 2f, (y1 + y2) / 2f, (z1 + z2) / 2f,
                x2 - x1, y2 - y1, z2 - z1, solid, style));

        // 1) Info counter: body, stone top with 5 cm overhang, and a low privacy screen.
        const float cx1 = 57.2f, cx2 = 58.1f, cz1 = 29.6f, cz2 = 32.4f;
        Box("info-counter", cx1, floor, cz1, cx2, CounterHeight, cz2, true, DarkStyle);
        Box("info-counter-top", cx1 - .05f, CounterHeight, cz1 - .05f,
            cx2 + .05f, CounterHeight + .07f, cz2 + .05f, true, LightStyle);
        Box("info-counter-screen", cx1 - .05f, CounterHeight + .07f, cz1,
            cx1 + .03f, CounterHeight + .55f, cz2, true, DarkStyle);

        // 2) Waiting bench row along the south wall, facing into the hall.
        for (int i = 0; i < 3; i++)
        {
            float x1 = 55.6f + i * 2.4f, x2 = x1 + 2f;
            Box("wait-bench-plinth-" + i, x1, floor, 21.88f, x2, 1.88f, 22.27f, true, DarkStyle);
            Box("wait-bench-seat-" + i, x1, 1.88f, 21.85f, x2, BenchSeatTop, 22.30f, true, LightStyle);
            Box("wait-bench-back-" + i, x1, BenchSeatTop, 21.80f, x2, 2.46f, 21.88f, true, DarkStyle);
        }

        // 3) Runner under the waiting zone: 1.5 cm thick, deliberately without collision.
        Box("wait-runner", 55.2f, floor, 22.6f, 59.0f, floor + .015f, 27.8f, false, RugStyle);

        // 4) Notice board on the west wall right of the entrance: frame plus light insert.
        Box("notice-frame", 54.96f, 2.10f, 33.2f, 55.04f, 3.10f, 34.6f, false, DarkStyle);
        Box("notice-insert", 55.04f, 2.18f, 33.28f, 55.06f, 3.02f, 34.52f, false, LightStyle);

        // 5) Upper storey rugs: they anchor the south seating group and the meeting table.
        float up = RoomPlan.Upper, rug = .015f;
        Box("upper-rug-south", 55.4f, up, 21.6f, 62.6f, up + rug, 23.85f, false, RugStyle);
        Box("upper-rug-meeting", 58.2f, up, 35.3f, 60.9f, up + rug, 39.5f, false, RugStyle);

        return boxes;
    }
}
