namespace TaxiDriver;

internal readonly record struct DestinationPoint(float X, float Y, float Z);
internal readonly record struct DestinationSelection(string Name, string Kind, DestinationPoint Goal);
internal enum DestinationPickMode { NoOp, MetadataOnly, Dispatch }
internal readonly record struct DestinationPickResult(DestinationPickMode Mode,
    DestinationSelection Selection, DestinationPoint RoutePoint, string DropOff, string Lot, string ActiveName);

/// <summary>Logical picks and physical route identity are independent. Exact points,
/// not a proximity radius: a nearby different route must still be dispatched.</summary>
internal static class DestinationSelectionPolicy
{
    internal static bool IsSelected(DestinationSelection? current, DestinationSelection candidate) =>
        current.HasValue && current.Value == candidate;

    internal static DestinationPickResult Select(DestinationSelection? current, DestinationPoint? resolved,
        DestinationPoint? activePoint, string activeDropOff, string activeLot,
        DestinationSelection next, DestinationPoint arrival, string dropOff, string lot,
        bool riding, bool driving, bool gaveUp)
    {
        bool keepRoute = riding && driving && !gaveUp && activePoint.HasValue &&
            (arrival == activePoint.Value || (resolved.HasValue && arrival == resolved.Value));
        var mode = keepRoute ? (current == next ? DestinationPickMode.NoOp : DestinationPickMode.MetadataOnly)
            : DestinationPickMode.Dispatch;
        // A reachable/fallback rebound remains the truth while retaining the route.
        bool rebound = keepRoute && activePoint!.Value != arrival;
        return new(mode, next, keepRoute ? activePoint!.Value : arrival,
            rebound ? activeDropOff : dropOff, rebound ? activeLot : lot, riding ? next.Name : string.Empty);
    }
}
