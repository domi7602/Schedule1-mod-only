using TownHallInterior;
using Xunit;

namespace TownHallInterior.Tests;

public sealed class BusinessManagerSafetyTests
{
    private const string TestOrganisation = "TownHallTest";
    private const string TestSlot = "SaveGame_2";

    // Synthetic path strings only: no directory or save file is created or inspected.
    private static string SlotPath(string slot, string profile = "profile-a") =>
        Path.Combine(Path.GetTempPath(), "TownHallManagerSafetyTests", profile, slot);

    private static ConstructionContext World(string organisation = TestOrganisation, string slot = TestSlot) =>
        new(true, true, true, true, SlotPath(slot), SlotPath(slot), organisation);

    [Theory]
    [InlineData("Dominik", "SaveGame_1")]
    [InlineData("Dominik", "SaveGame_2")]
    [InlineData("OtherOrganisation", "SaveGame_2")]
    [InlineData("OtherOrganisation", "SaveGame_9")]
    [InlineData("TownHallTest", "SaveGame_1")]
    [InlineData("TownHallTest", "SaveGame_9")]
    [InlineData("townhalltest", "SaveGame_2")]
    public void Save_probe_cannot_be_retargeted_with_matching_explicit_arguments(string organisation, string slot)
    {
        var trial = new BusinessSubtypeTrial();
        var port = new Port();

        var result = trial.Run(World(organisation, slot), port,
            organisation: organisation, slot: slot, probeSave: true);

        AssertSkippedBeforePortTouched(trial, result, port);
    }

    [Theory]
    [InlineData("missing-context")]
    [InlineData("load-incomplete")]
    [InlineData("lobby-or-multiplayer")]
    [InlineData("hall-unready")]
    [InlineData("dependencies-mismatch")]
    [InlineData("different-absolute-paths")]
    [InlineData("loaded-other-slot")]
    [InlineData("active-path-missing")]
    [InlineData("loaded-path-missing")]
    [InlineData("active-path-relative")]
    [InlineData("loaded-path-relative")]
    [InlineData("organisation-missing")]
    [InlineData("organisation-mismatch")]
    [InlineData("organisation-case")]
    [InlineData("slot-mismatch")]
    public void Save_probe_rejects_unconfirmed_test_world_before_any_port_access(string cause)
    {
        var context = cause switch
        {
            "missing-context" => null!,
            "load-incomplete" => World() with { LoadComplete = false },
            "lobby-or-multiplayer" => World() with { Singleplayer = false },
            "hall-unready" => World() with { HallReady = false },
            "dependencies-mismatch" => World() with { DependenciesMatch = false },
            "different-absolute-paths" => World() with { LoadedSavePath = SlotPath(TestSlot, "profile-b") },
            "loaded-other-slot" => World() with { LoadedSavePath = SlotPath("SaveGame_1") },
            "active-path-missing" => World() with { ActiveSavePath = null },
            "loaded-path-missing" => World() with { LoadedSavePath = null },
            "active-path-relative" => World() with { ActiveSavePath = TestSlot },
            "loaded-path-relative" => World() with { LoadedSavePath = TestSlot },
            "organisation-missing" => World() with { OrganisationName = null },
            "organisation-mismatch" => World("Dominik"),
            "organisation-case" => World("townhalltest"),
            "slot-mismatch" => World(slot: "SaveGame_1"),
            _ => throw new ArgumentOutOfRangeException(nameof(cause))
        };
        var trial = new BusinessSubtypeTrial();
        var port = new Port();

        var result = trial.Run(context, port, probeSave: true);

        AssertSkippedBeforePortTouched(trial, result, port);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void Confirmed_test_world_is_blocked_without_a_verified_native_completion_contract(bool explicitTarget)
    {
        var trial = new BusinessSubtypeTrial();
        var port = new Port();

        var result = explicitTarget
            ? trial.Run(World(), port, organisation: TestOrganisation, slot: TestSlot, probeSave: true)
            : trial.Run(World(), port, probeSave: true);

        AssertBlockedBeforePortTouched(trial, result, port);
    }

    [Fact]
    public void Rejected_save_target_leaves_the_same_trial_available_for_the_confirmed_test_world()
    {
        var trial = new BusinessSubtypeTrial();
        var deniedPort = new Port();
        var denied = trial.Run(World("Dominik", "SaveGame_1"), deniedPort,
            organisation: "Dominik", slot: "SaveGame_1", probeSave: true);

        AssertSkippedBeforePortTouched(trial, denied, deniedPort);

        var allowedPort = new Port();
        var allowed = trial.Run(World(), allowedPort, probeSave: true);
        AssertBlockedBeforePortTouched(trial, allowed, allowedPort);
        Assert.Empty(deniedPort.Events);
    }

    [Theory]
    [InlineData("Dominik", "SaveGame_1")]
    [InlineData("OtherOrganisation", "SaveGame_9")]
    [InlineData("TownHallTest", "SaveGame_1")]
    public void Historical_non_writing_manager_probe_retains_the_general_explicit_gate(string organisation, string slot)
    {
        var context = World(organisation, slot);
        Assert.Null(ConstructionGate.Check(context, organisation, slot));
        var port = new Port();

        var result = new BusinessSubtypeTrial().Run(context, port,
            probeManager: true, organisation: organisation, slot: slot);

        Assert.Equal("observed", result.Status);
        Assert.Empty(result.Findings);
        Assert.True(result.CleanupComplete);
        Assert.False(result.Manager!.SaveCalled);
        Assert.Single(port.Events, e => e == "manager-register");
        Assert.DoesNotContain("manager-save", port.Events);
    }

    private static void AssertSkippedBeforePortTouched(BusinessSubtypeTrial trial, SubtypeResult result, Port port)
    {
        Assert.Equal("skipped", result.Status);
        Assert.False(result.AttemptConsumed);
        Assert.False(trial.AttemptConsumed);
        Assert.False(result.RegistrationAttempted);
        Assert.False(result.RegistrationCompleted);
        Assert.Null(result.Before);
        Assert.Null(result.Manager);
        Assert.NotEmpty(result.Findings);
        Assert.Empty(port.Events);
    }

    private static void AssertBlockedBeforePortTouched(BusinessSubtypeTrial trial, SubtypeResult result, Port port)
    {
        Assert.Equal("blocked", result.Status);
        Assert.Contains("native-save-completion-contract-unverified", result.Findings);
        Assert.False(result.AttemptConsumed);
        Assert.False(trial.AttemptConsumed);
        Assert.False(result.RegistrationAttempted);
        Assert.False(result.RegistrationCompleted);
        Assert.False(result.CleanupComplete);
        Assert.Null(result.Before);
        Assert.Null(result.Manager);
        Assert.Empty(port.Events);
    }

    private sealed class Port : IBusinessSubtypePort, IBusinessManagerPort
    {
        public readonly List<string> Events = new();
        private readonly Dictionary<string, string> _registries = new[]
        {
            "properties", "properties-owned", "properties-unowned", "businesses", "businesses-owned",
            "businesses-unowned", "saveables", "base-saveables", "completed-saveables", "save-requests"
        }.ToDictionary(key => key, _ => "initial");
        private readonly Dictionary<string, string> _classes = new() { ["Business"] = "initial" };

        public SubtypeSnapshot Capture() { Events.Add("capture"); return new(_registries, _classes); }
        public void RegisterType() => Events.Add("register");
        public void Prepare() => Events.Add("prepare");
        public SubtypeReadback Inspect() { Events.Add("inspect"); return new(true, true, true, true, true, 0); }
        public SubtypeDispatch InvokeThroughNativeBase() { Events.Add("invoke"); return new(false, false, 1, 1); }
        public ManagerRegistration RegisterOnce() { Events.Add("manager-register"); return new(5, 1, 0, true, false); }
        public void Cleanup() => Events.Add("cleanup");
        public bool IsClean { get { Events.Add("clean"); return true; } }
    }
}
