namespace TaxiDriver;

/// <summary>Unity-free selection of the pre-reduction forward cruise target, in km/h.</summary>
internal static class CruiseSpeedPolicy
{
    internal const float CruiseKmh = 50f;

    internal static float BaseTargetKmh(float nativeBaseKmh, bool ownedTaxi, bool activeOrder,
        bool autoDriving, bool playerDriving, bool reversing, bool nativeOverride)
    {
        if (!ownedTaxi || !activeOrder || !autoDriving || playerDriving || reversing || nativeOverride ||
            !float.IsFinite(nativeBaseKmh) || nativeBaseKmh <= 0f)
            return nativeBaseKmh;
        return CruiseKmh;
    }
}
