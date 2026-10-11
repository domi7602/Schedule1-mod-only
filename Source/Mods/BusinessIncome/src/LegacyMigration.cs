using System;

namespace BusinessIncome.Core;

/// <summary>
/// Pure legacy-migration decision. The legacy file is NEVER deleted, and it is only
/// auto-adopted when its SaveIdentity matches the explicit migration target exactly.
/// Anything ambiguous is surfaced loudly (BlockAmbiguous) instead of speculatively
/// counting saves.
/// </summary>
public static class LegacyMigration
{
    public static LegacyMigrationDecision Decide(
        string? legacyIdentity,
        string targetIdentity,
        bool targetExists,
        bool legacyExists)
    {
        if (!legacyExists) return LegacyMigrationDecision.None;
        if (targetExists) return LegacyMigrationDecision.TargetExists;

        if (!string.IsNullOrEmpty(legacyIdentity)
            && !string.IsNullOrEmpty(targetIdentity)
            && string.Equals(legacyIdentity, targetIdentity, StringComparison.OrdinalIgnoreCase))
            return LegacyMigrationDecision.AdoptMatching;

        return LegacyMigrationDecision.BlockAmbiguous;
    }
}
