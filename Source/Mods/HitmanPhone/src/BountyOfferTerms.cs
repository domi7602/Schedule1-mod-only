namespace HitmanPhone.Bounty;

/// <summary>
/// Immutable-ish transport of the rolled offer terms from the scheduler to the
/// accept handler. Written into the <see cref="BountyContract"/> at accept
/// time (audit H2 pattern: values travel structurally, the message body is
/// display-only and never parsed back). Not persisted itself.
/// </summary>
public class BountyOfferTerms
{
    public float RewardCash;
    public string DropId = "";        // stable dead-drop GUID; "" = any drop (legacy/test offers)
    public string DropName = "";
    public int DropWindowMinutes = BountyBudget.MidTierWindowMinutes;
    public float TargetWeeklySpend;   // 0 = unknown → legacy reward/deadline semantics
    public string RewardTier = "";    // "" = unknown (see BountyBudget.TierFor)
}
