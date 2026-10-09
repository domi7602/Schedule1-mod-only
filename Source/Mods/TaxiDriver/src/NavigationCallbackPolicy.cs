namespace TaxiDriver;

/// <summary>Pure gate that accepts only a current, still-active navigation callback.</summary>
internal static class NavigationCallbackPolicy
{
    internal static bool ShouldAcceptResult(int callbackOrder, int currentOrder, bool pollingActive) =>
        pollingActive && callbackOrder == currentOrder;
}
