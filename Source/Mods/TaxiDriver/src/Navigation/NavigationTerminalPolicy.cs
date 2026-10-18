namespace TaxiDriver;

/// <summary>Clear the previous verdict only at a genuinely new operation boundary, never on recovery.</summary>
internal static class NavigationTerminalPolicy
{
    internal static void Begin(ref bool gaveUp, ref bool arrived, ref string? callback)
    {
        gaveUp = false;
        arrived = false;
        callback = null;
    }
}
