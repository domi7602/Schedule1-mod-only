using System;
using System.IO;
using Xunit;

namespace TaxiDriver.Tests;

/// <summary>Wiring contracts only: these checks do not execute Unity or native NPC behavior.</summary>
public class DriverSafetySourceContractTests
{
    [Fact]
    public void DriverSelection_DoesNotSearchTheBystanderRegistry()
    {
        string source = ReadSource("SpikeCommands.cs");
        string selection = Section(source, "internal static bool Npc()", "internal static bool OursBoardable");
        Assert.DoesNotContain("NPCManager.NPCRegistry", selection);
        Assert.Contains("TaxiDriverNPC.TryGetVanilla()", selection);
        Assert.Contains("return BoardDriver(veh, ours)", selection);
    }

    [Fact]
    public void GenuineEntry_IsIdentityCheckedAndCommittedBeforeNativeCall()
    {
        string source = ReadSource("SpikeCommands.cs");
        string board = Section(source, "internal static bool BoardDriver", "internal static void TickSeatVerify");
        Assert.Contains("TaxiDriverNPC.IsOurVanilla(best)", board);
        int gate = board.IndexOf("DriverBoarding.TryBeginEntry", StringComparison.Ordinal);
        int enter = board.IndexOf("best.EnterVehicle(null, veh)", StringComparison.Ordinal);
        Assert.True(gate >= 0 && enter > gate, "Cooldown must be committed before the actual native entry call.");
        Assert.DoesNotContain("LastBoardedAt =", board);
    }

    [Fact]
    public void PickupDriverFailure_UsesOwnershipCheckedCleanup()
    {
        string source = ReadSource("SpikeRunner.cs");
        string run = Section(source, "private static void TickAutoRun()", "internal static void TickRide()");
        Assert.Contains("SpikeCommands.Cleanup(\"automatic pickup failed\"", run);
    }

    [Fact]
    public void Protection_IsReappliedOnLoadAndReadsBackTheActualHealthField()
    {
        string source = ReadSource("TaxiDriverNPC.cs");
        Assert.Contains("protected override void OnLoaded()", source);
        Assert.Contains("health._invincible = true", source);
        Assert.Contains("return health._invincible", source);
        Assert.Contains("health.Revive()", source);
    }

    [Fact]
    public void DriverRetentionFailure_StopsInsteadOfRepeatingEntryEveryFrame()
    {
        string commands = ReadSource("SpikeCommands.cs");
        string retention = Section(commands, "internal static bool EnsureDriverSeat", "internal static VehicleSeat? FindSeat");
        Assert.Contains("AbortDriverRecovery", retention);
        Assert.Contains("if (!BoardDriver(veh, npc))", retention);
        string runner = ReadSource("SpikeRunner.cs");
        string reverse = Section(runner, "private static bool TryStartReverse", "private static bool TickStartupRecovery");
        Assert.Contains("if (!SpikeState.PollingActive)", reverse);
    }

    /// <summary>
    /// Resolves a mod source file by name from the src root, searching the domain
    /// subfolders (src/&lt;Domain&gt;/&lt;File&gt;.cs) as well as the root itself. Keeping the
    /// lookup layout-independent means a source reorganization does not invalidate
    /// every contract test that reads a file.
    /// </summary>
    internal static string ReadSource(string name)
    {
        for (DirectoryInfo? dir = new DirectoryInfo(AppContext.BaseDirectory); dir != null; dir = dir.Parent)
        {
            string root = Path.Combine(dir.FullName, "Source", "Mods", "TaxiDriver", "src");
            if (!Directory.Exists(root)) continue;

            string direct = Path.Combine(root, name);
            if (File.Exists(direct)) return File.ReadAllText(direct);

            string[] matches = Directory.GetFiles(root, name, SearchOption.AllDirectories);
            if (matches.Length == 1) return File.ReadAllText(matches[0]);
            if (matches.Length > 1)
                throw new IOException($"Ambiguous TaxiDriver source name '{name}': {string.Join(", ", matches)}");
            throw new FileNotFoundException("TaxiDriver source file not found", name);
        }
        throw new FileNotFoundException("TaxiDriver source root not found", name);
    }

    internal static string Section(string source, string start, string end)
    {
        int first = source.IndexOf(start, StringComparison.Ordinal);
        Assert.True(first >= 0, $"Missing section: {start}");
        int last = source.IndexOf(end, first + start.Length, StringComparison.Ordinal);
        Assert.True(last > first, $"Missing section end: {end}");
        return source.Substring(first, last - first);
    }
}
