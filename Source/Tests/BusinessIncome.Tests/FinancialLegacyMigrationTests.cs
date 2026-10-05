using BusinessIncome.Core;
using Xunit;

namespace BusinessIncome.Tests;

/// <summary>Legendary-migration decision: never delete, adopt only exact identity match.</summary>
public class FinancialLegacyMigrationTests
{
    [Fact]
    public void NoLegacy_None()
    {
        Assert.Equal(LegacyMigrationDecision.None,
            LegacyMigration.Decide(legacyIdentity: null, targetIdentity: "slot_1", targetExists: false, legacyExists: false));
    }

    [Fact]
    public void TargetAlreadyExists_TargetExists()
    {
        Assert.Equal(LegacyMigrationDecision.TargetExists,
            LegacyMigration.Decide("slot_1", "slot_1", targetExists: true, legacyExists: true));
    }

    [Fact]
    public void MatchingIdentity_Adopt()
    {
        Assert.Equal(LegacyMigrationDecision.AdoptMatching,
            LegacyMigration.Decide("slot_1", "slot_1", targetExists: false, legacyExists: true));
    }

    [Fact]
    public void MismatchedIdentity_BlockAmbiguous()
    {
        Assert.Equal(LegacyMigrationDecision.BlockAmbiguous,
            LegacyMigration.Decide("default", "slot_1", targetExists: false, legacyExists: true));
    }

    [Fact]
    public void UnknownLegacyIdentity_BlockAmbiguous()
    {
        Assert.Equal(LegacyMigrationDecision.BlockAmbiguous,
            LegacyMigration.Decide("", "slot_1", targetExists: false, legacyExists: true));
    }
}
