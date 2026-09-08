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
}
