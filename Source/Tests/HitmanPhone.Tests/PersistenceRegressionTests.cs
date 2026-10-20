using System.Text.Json;
using HitmanPhone.Persistence;
using Xunit;

namespace HitmanPhone.Tests;

public class PersistenceRegressionTests
{
    private static readonly JsonSerializerOptions Options = new() { IncludeFields = true };

    [Fact]
    public void DebugResetKeepsBoundIdentityAndTokenHighWaterWithoutRetainingContracts()
    {
        var previous = new BountySaveData { SaveIdentity = "bound-world", NextEvidenceToken = 57 };
        previous.Active.Add(new BountyContract { Id = "old" });
        var reset = BountySaveValidation.CreateReset(previous);
        Assert.Equal("bound-world", reset.SaveIdentity);
        Assert.Equal(57, reset.NextEvidenceToken);
        Assert.Empty(reset.Active);
        Assert.Single(previous.Active);
    }

    [Theory]
    [InlineData("A", "world-before", false)]
    [InlineData("B", "world-after", false)]
    [InlineData("", "", false)]
    [InlineData("A", "world-after", true)]
    public void PrimaryFallbackRequiresIdentityAndExactWorldCheckpoint(string identity, string hash, bool expected)
    {
        var data = new BountySaveData { SaveIdentity = identity, WorldCheckpoint = hash };
        Assert.Equal(expected, BountySaveValidation.MatchesCheckpoint(data, "A", "world-after"));
    }

    [Fact]
    public void WorldPathAcceptsExistingWorldFolderOrItsGameJsonNotAnArbitraryAncestor()
    {
        string root = Path.Combine(Environment.GetEnvironmentVariable("TMPDIR") ?? Path.GetTempPath(), "hitman-world-" + Guid.NewGuid());
        Directory.CreateDirectory(root);
        try
        {
            string game = Path.Combine(root, "Game.json");
            File.WriteAllText(game, "world");
            Assert.Equal(BountyWorldCheckpoint.ResolveDirectory(root), BountyWorldCheckpoint.ResolveDirectory(game));
            Assert.Throws<InvalidDataException>(() => BountyWorldCheckpoint.ResolveDirectory(Path.Combine(root, "missing.json")));
        }
        finally { Directory.Delete(root, true); }
    }

    [Fact]
    public void FingerprintIgnoresMetadataTimestampButTracksMoney()
    {
        string root = Path.Combine(Environment.GetEnvironmentVariable("TMPDIR") ?? Path.GetTempPath(), "hitman-checkpoint-" + Guid.NewGuid());
        Directory.CreateDirectory(root);
        try
        {
            File.WriteAllText(Path.Combine(root, "Game.json"), "world");
            File.WriteAllText(Path.Combine(root, "Metadata.json"), "old timestamp");
            File.WriteAllText(Path.Combine(root, "Money.json"), "100");
            string before = BountyWorldCheckpoint.Fingerprint(root);
            File.WriteAllText(Path.Combine(root, "Metadata.json"), "new timestamp");
            Assert.Equal(before, BountyWorldCheckpoint.Fingerprint(root));
            File.WriteAllText(Path.Combine(root, "Money.json"), "200");
            Assert.NotEqual(before, BountyWorldCheckpoint.Fingerprint(root));
        }
        finally { Directory.Delete(root, true); }
    }

    [Fact]
    public void FingerprintTracksWorldRollbackButIgnoresModdedWrites()
    {
        string root = System.IO.Path.Combine((Environment.GetEnvironmentVariable("TMPDIR") ?? Path.GetTempPath()), "hitman-checkpoint-" + Guid.NewGuid());
        System.IO.Directory.CreateDirectory(root);
        try
        {
            System.IO.File.WriteAllText(System.IO.Path.Combine(root, "Game.json"), "before");
            string before = BountyWorldCheckpoint.Fingerprint(root);
            Assert.NotEmpty(before);
            System.IO.Directory.CreateDirectory(System.IO.Path.Combine(root, "Modded"));
            System.IO.File.WriteAllText(System.IO.Path.Combine(root, "Modded", "other.json"), "mod");
            Assert.Equal(before, BountyWorldCheckpoint.Fingerprint(root));
            System.IO.File.WriteAllText(System.IO.Path.Combine(root, "Game.json"), "after");
            Assert.NotEqual(before, BountyWorldCheckpoint.Fingerprint(root));
            System.IO.File.WriteAllText(System.IO.Path.Combine(root, "Game.json"), "before");
            Assert.Equal(before, BountyWorldCheckpoint.Fingerprint(root));
        }
        finally { System.IO.Directory.Delete(root, true); }
    }

    [Theory]
    [InlineData("B", true)]
    [InlineData("A", false)]
    public void CheckpointRejectsWrongContextOrFailedWorldSave(string identity, bool success)
    {
        var session = new BountyCheckpointSession();
        session.Bind("A", "slotA");
        session.MarkDirty();
        session.Prepare("frozen");
        int writes = 0;
        Assert.False(session.Commit(identity, success, (_, _) => { writes++; return true; }));
        Assert.Equal(0, writes);
        Assert.True(session.Dirty);
    }

    [Fact]
    public void CheckpointFailureRetainsFrozenSnapshotForOriginalDestination()
    {
        var session = new BountyCheckpointSession();
        session.Bind("A", "slotA");
        session.MarkDirty();
        session.Prepare("before");
        Assert.False(session.Commit("A", true, (_, _) => false));
        Assert.True(session.Dirty);
        Assert.True(session.Commit("A", true, (path, json) => path == "slotA" && json == "before"));
        Assert.False(session.Dirty);
        Assert.False(session.Saving);
    }

    [Fact]
    public void ExplicitNullCollections_AreNormalizedBeforeIteration()
    {
        var data = JsonSerializer.Deserialize<BountySaveData>(
            """{"Active":null,"History":null,"CallerCooldowns":null,"KillEvents":null,"HeatState":null,"CallSchedule":{"Slots":null}}""", Options)!;
        BountySaveValidation.Normalize(data);
        Assert.NotNull(data.Active);
        Assert.NotNull(data.History);
        Assert.NotNull(data.CallerCooldowns);
        Assert.NotNull(data.KillEvents);
        Assert.NotNull(data.HeatState);
        Assert.NotNull(data.CallSchedule.Slots);
    }
}
