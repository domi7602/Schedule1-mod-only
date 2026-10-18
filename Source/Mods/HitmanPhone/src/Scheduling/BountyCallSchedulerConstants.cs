namespace HitmanPhone.Bounty;

/// <summary>
/// Public constants used by the caller scheduler. Kept here to share values
/// between <see cref="BountyCallScheduler"/> and <see cref="BountyConversationRouter"/>
/// without exposing them as fields on the scheduler itself (which would clutter
/// its API for callers that only care about <c>Tick()</c>).
/// </summary>
public static class BountyCallSchedulerConstants
{
    /// <summary>Cooldown applied to a caller when the player declines the offer.</summary>
    public const int CallerCooldownDaysAfterDecline = 3;

    /// <summary>Cooldown applied to a caller when the player dies during the contract.</summary>
    public const int CallerCooldownDaysAfterPlayerDeath = 14;

    /// <summary>How many in-game days the player has to deliver the photo before the contract expires.</summary>
    public const int ContractDeadlineDays = 3;

    // ------------------------------------------------------------------
    // Call-plan cadence (package D). All times are in-game minutes/days and
    // live in the persisted save-level plan — see BountyCallSchedulePlan.
    // ------------------------------------------------------------------

    /// <summary>Hard cap of offers dispatched per in-game day.</summary>
    public const int MaxOffersPerDay = 2;

    /// <summary>Minimum in-game minutes between two calls on the same day.</summary>
    public const int MinGapMinutesBetweenSameDayCalls = 210;

    /// <summary>Longest stretch of silence: after this many idle days a call is forced.</summary>
    public const int MaxIdleDaysBeforeCall = 2;

    /// <summary>Chance (percent) that a second call joins an already planned call day.</summary>
    public const int DoubleCallChancePercent = 25;

    /// <summary>Quiet hours: no calls scheduled in [QuietHourStart, QuietHourEnd).</summary>
    public const int QuietHourStart = 1;

    /// <summary>End of the quiet window (inclusive-exclusive, see QuietHourStart).</summary>
    public const int QuietHourEnd = 7;

    /// <summary>The plan is always generated this many days ahead of the current day.</summary>
    public const int PlanLeadDays = 7;

    /// <summary>Hard cap on simultaneously active bounty contracts.</summary>
    public const int MaxActiveContracts = 3;
}
