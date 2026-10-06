using System;

namespace HitmanPhone.Bounty;

/// <summary>
/// Budget-derived bounty terms (package B): reward, tier, and post-photo drop
/// window computed from the target's weekly customer budget. Pure static math
/// (no Unity, no game types) so Source/Tests/HitmanPhone.Tests can cover it.
///
/// The weekly budget is the midpoint of the game's CustomerData
/// MinWeeklySpend/MaxWeeklySpend for the target (0 = unknown, e.g. legacy
/// contracts or NPCs without customer data).
/// </summary>
public static class BountyBudget
{
    public const string TierLow = "low";
    public const string TierMid = "mid";
    public const string TierHigh = "high";

    /// <summary>Weekly-spend at or below this is "low" tier ($/week).</summary>
    public const float LowTierMaxWeeklySpend = 400f;

    /// <summary>Weekly-spend at or above this is "high" tier ($/week).</summary>
    public const float HighTierMinWeeklySpend = 900f;

    /// <summary>Reward = weekly budget × this factor.</summary>
    public const float RewardPerWeekFactor = 0.35f;

    public const float MinReward = 150f;
    public const float MaxReward = 2500f;

    /// <summary>Reward granularity, so prices stay round.</summary>
    public const float RewardRounding = 50f;

    // Agreed deadline table (post-photo drop window): low = 4h, mid = 3h,
    // high = 2h. Unknown budget behaves like mid.
    public const int LowTierWindowMinutes = 4 * 60;
    public const int MidTierWindowMinutes = 3 * 60;
    public const int HighTierWindowMinutes = 2 * 60;
    public const int UnknownTierWindowMinutes = MidTierWindowMinutes;

    /// <summary>
    /// Classify the target budget at offer time. "" = unknown/legacy (the
    /// caller must then keep legacy reward and deadline semantics).
    /// </summary>
    public static string TierFor(float weeklySpend)
    {
        if (weeklySpend <= 0f) return string.Empty;
        if (weeklySpend <= LowTierMaxWeeklySpend) return TierLow;
        if (weeklySpend >= HighTierMinWeeklySpend) return TierHigh;
        return TierMid;
    }

    public static int WindowMinutesFor(string? tier) => tier switch
    {
        TierLow => LowTierWindowMinutes,
        TierHigh => HighTierWindowMinutes,
        TierMid => MidTierWindowMinutes,
        _ => UnknownTierWindowMinutes
    };

    public static int WindowHoursFor(string? tier) => WindowMinutesFor(tier) / 60;

    /// <summary>
    /// Budget-based reward (rounded to <see cref="RewardRounding"/>). Unknown
    /// budget (≤ 0) falls back to the legacy 200–500 roll so test/force offers
    /// and pre-budget contracts keep their old behaviour.
    /// </summary>
    public static float RewardForBudget(float weeklySpend, Random rng)
    {
        if (rng == null) throw new ArgumentNullException(nameof(rng));
        if (weeklySpend <= 0f) return LegacyReward(rng);
        float raw = weeklySpend * RewardPerWeekFactor;
        float clamped = Math.Clamp(raw, MinReward, MaxReward);
        return MathF.Round(clamped / RewardRounding) * RewardRounding;
    }

    /// <summary>Legacy reward band kept for unknown budgets: 200–500 cash, endpoints included.</summary>
    public static float LegacyReward(Random rng)
    {
        if (rng == null) throw new ArgumentNullException(nameof(rng));
        return rng.Next(200, 501);
    }
}
