namespace TaxiDriver;

/// <summary>
/// Typed hero change snapshot (finding 7). Replaces the bit-packed int
/// signature, whose 5 fare bits wrapped every $32
/// (<c>32 &lt;&lt; 27 == 0</c> mod 2^32) and froze the fare display.
/// Exact field comparison — no hashes. Pure (no Unity), unit-tested;
/// the builder lives in <c>TaxiApp.RefreshHero</c> (game state).
/// </summary>
internal readonly record struct HeroSnapshot(
    bool RideActive,
    bool AwaitingDestination,
    bool Arrived,
    bool GaveUp,
    bool Picked,
    bool AwaitingBoard,
    bool ToPlayer,
    bool AutoRunning,
    bool PendingSpawn,
    bool Vehicle,
    bool ToPlayerPolling,
    string Destination,
    int Fare,
    string Override);
