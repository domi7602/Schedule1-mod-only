using System.Collections.Generic;

namespace HitmanPhone.Bounty;

/// <summary>
/// Static catalog of the three caller styles the bounty workflow uses.
/// Cold (business), Threatening (pressure), Desperate (vendetta).
/// Each style produces slightly different narrative tone in the message body,
/// but mechanically the caller pays the same and rolls the same target pool.
/// </summary>
public static class BountyDialogTemplates
{
    public const string StyleCold = "cold";
    public const string StyleThreatening = "threatening";
    public const string StyleDesperate = "desperate";

    public const string AcceptLabel = "Take the job";
    public const string AcceptText = "I'll do it.";
    public const string DeclineLabel = "Decline";
    public const string DeclineText = "Not interested.";
    public const string MoreInfoLabel = "Tell me more";
    public const string MoreInfoText = "Who is the target?";

    /// <summary>
    /// Build the body text for a bounty offer shown to the player.
    /// Caller is asked to provide caller name (anonymised) and the target npc
    /// display name; reward is shown explicitly because the worker-stage is
    /// post-cartel and the player has to weigh reward vs. police heat.
    /// NOTE (audit H2, 2026-09-01): the reward travels structurally to the
    /// accept handler — the body is display-only, never parsed back.
    /// </summary>
    public static string BuildColdOffer(string caller, string target, string region, float r)
        => $"[{caller}]: I have a job for you. Clean.\n\n" +
           $"Target <b>{target}</b> in {region}. {r:N0} on confirmation. No questions.";

    public static string BuildThreateningOffer(string caller, string target, string region, float r)
        => $"[{caller}]: Listen. {region} has been rough on me.\n\n" +
           $"Target <b>{target}</b>. I want them gone. {r:N0} when they're gone. Don't test me.";

    public static string BuildDesperateOffer(string caller, string target, string region, float r)
        => $"[{caller}]: Please. <b>{target}</b> took something from me. " +
           "I can't do this myself. They hang out in " + region + ".\n" +
           $"I have {r:N0} for you if you do it. Please.";

    /// <summary>
    /// Style rotation: pick one in stable order — caller index → style.
    /// Caller index 0 = cold, 1 = threatening, 2 = desperate, then wrap.
    /// </summary>
    public static string StyleForCallerIndex(int callerIndex)
    {
        return (callerIndex % 3) switch
        {
            0 => StyleCold,
            1 => StyleThreatening,
            _ => StyleDesperate
        };
    }

    /// <summary>
    /// Pick the message-builder for the chosen style.
    /// </summary>
    public static string BuildForStyle(string style, string caller, string target, string region, float r)
    {
        return style switch
        {
            StyleThreatening => BuildThreateningOffer(caller, target, region, r),
            StyleDesperate => BuildDesperateOffer(caller, target, region, r),
            _ => BuildColdOffer(caller, target, region, r)
        };
    }

    /// <summary>
    /// Names for the anonymous caller pool. These are read by the player as
    /// unknown persons — the underlying NPC identity is not exposed because the
    /// conversation lives on a generic NPC shell (Phase D does not yet bind
    /// callers to specific NPCs; the conversation is sent on the player's
    /// behalf via MessagingManager).
    /// </summary>
    public static readonly IReadOnlyList<string> CallerPool = new[]
    {
        "Ghost",        // index 0
        "Jackal",       // index 1
        "Magpie",       // index 2
        "Viper",        // index 3
        "Crow"          // index 4
    };

    public static string GetCallerName(int index) =>
        CallerPool[((index % CallerPool.Count) + CallerPool.Count) % CallerPool.Count];
}
