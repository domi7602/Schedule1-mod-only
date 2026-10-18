namespace TaxiDriver;

/// <summary>Authority/control gate only. No obstacle classification or recovery timers.</summary>
internal static class TrafficLightPolicy
{
    internal static bool ShouldIgnore(bool host, bool exactOwnedTaxi, bool activeOrder,
        bool autoDriving, bool playerDriver)
        => host && exactOwnedTaxi && activeOrder && autoDriving && !playerDriver;
}
