using System;

namespace TaxiDriver;

/// <summary>Consumes the meter end before IO; reporting failure never resubmits fares.</summary>
internal static class MeterSettlement
{
    internal static void Stop(ref bool running, Action report)
    {
        if (!running) return;
        running = false;
        report();
    }
}
