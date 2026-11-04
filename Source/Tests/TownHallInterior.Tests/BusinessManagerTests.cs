using TownHallInterior;
using Xunit;

namespace TownHallInterior.Tests;

public sealed class BusinessManagerTests
{
    private static ConstructionContext World(string slot, string organisation) => new(true, true, true, true,
        Path.Combine(Path.GetTempPath(), "TownHallManagerTests", slot),
        Path.Combine(Path.GetTempPath(), "TownHallManagerTests", slot), organisation);

    [Fact]
    public void Default_gate_rejects_Dominik_and_accepts_only_the_explicit_slot()
    {
        Assert.Equal("test-world-mismatch", ConstructionGate.Check(World("SaveGame_1", "Dominik")));
        Assert.Null(ConstructionGate.Check(World("SaveGame_1", "Dominik"), "Dominik", "SaveGame_1"));
        Assert.Equal("test-world-mismatch", ConstructionGate.Check(World("SaveGame_2", "TownHallTest"), "Dominik", "SaveGame_1"));
        Assert.Equal("test-slot-mismatch", ConstructionGate.Check(World("SaveGame_2", "Dominik"), "Dominik", "SaveGame_1"));
    }

    [Fact]
    public void Reversed_registration_is_observed_only_on_Dominik_slot_1()
    {
        var port = new Port();
        var result = new BusinessSubtypeTrial().Run(World("SaveGame_1", "Dominik"), port, probeManager: true, organisation: "Dominik", slot: "SaveGame_1");
        Assert.Equal("observed", result.Status);
        Assert.Equal(1, port.Registrations);
        Assert.False(result.Manager!.SaveCalled);
        Assert.Equal(1, result.Manager.Added);
        Assert.Equal(0, result.Manager.Remaining);
        Assert.Equal("skipped", new BusinessSubtypeTrial().Run(World("SaveGame_2", "TownHallTest"), port, probeManager: true, organisation: "Dominik", slot: "SaveGame_1").Status);
        Assert.Equal(1, port.Registrations);
    }

    [Fact]
    public void Settled_save_is_observed_and_an_unfinished_save_fails()
    {
        var port = new Port();
        var result = new BusinessSubtypeTrial().Run(World("SaveGame_1", "Dominik"), port, organisation: "Dominik", slot: "SaveGame_1", probeSave: true);
        Assert.Equal("observed", result.Status);
        Assert.True(result.Manager!.SaveCalled);
        Assert.False(result.Manager.SavingAfter);
        port = new Port { SavingAfter = true };
        var failed = new BusinessSubtypeTrial().Run(World("SaveGame_1", "Dominik"), port, organisation: "Dominik", slot: "SaveGame_1", probeSave: true);
        Assert.Equal("failed", failed.Status);
        Assert.Contains("manager-save-not-settled", failed.Findings[0]);
    }

    [Fact]
    public void Unreversed_registration_fails_without_a_save_call()
    {
        var port = new Port { Remaining = 1, Removed = false };
        var result = new BusinessSubtypeTrial().Run(World("SaveGame_1", "Dominik"), port, probeManager: true, organisation: "Dominik", slot: "SaveGame_1");
        Assert.Equal("failed", result.Status);
        Assert.Contains("manager-registration-not-reversed", result.Findings[0]);
        Assert.False(result.Manager!.SaveCalled);
        Assert.Equal(1, port.Registrations);
    }

    private sealed class Port : IBusinessSubtypePort, IBusinessManagerPort
    {
        public int Registrations, Remaining = 0;
        public bool Removed = true;
        private readonly Dictionary<string, string> _registries = new[] { "properties", "properties-owned", "properties-unowned", "businesses", "businesses-owned", "businesses-unowned", "saveables", "base-saveables", "completed-saveables", "save-requests" }.ToDictionary(k => k, _ => "initial");
        public SubtypeSnapshot Capture() => new(_registries, new Dictionary<string, string> { ["ISaveable"] = "initial" });
        public void RegisterType() { }
        public void Prepare() { }
        public SubtypeReadback Inspect() => new(true, true, true, true, true, 0);
        public SubtypeDispatch InvokeThroughNativeBase() => new(false, false, 1, 1);
        public ManagerRegistration RegisterOnce()
        {
            Registrations++;
            return new(5, 1, Remaining, Removed, false);
        }
        public ManagerRegistration SaveRegistered()
        {
            Registrations++;
            return new(5, 1, Remaining, Removed, true, SavingAfter: SavingAfter);
        }
        public bool SavingAfter;
        public void Cleanup() { }
        public bool IsClean => true;
    }
}
