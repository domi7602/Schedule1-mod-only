using System;

namespace TownHallInterior;

// Implementations own an inactive root, its children and any created materials.
public interface IRoomSink : IDisposable
{
    void Add(RoomBox box);
}

public static class RoomBuilder
{
    public static IRoomSink Build(Func<IRoomSink> create)
    {
        var sink = create();
        if (sink == null) throw new InvalidOperationException("Room sink was null");
        try
        {
            foreach (var box in RoomPlan.Create()) sink.Add(box);
            return sink;
        }
        catch
        {
            sink.Dispose();
            throw;
        }
    }
}
