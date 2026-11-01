using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Xunit;

namespace TaxiDriver.Tests;

/// <summary>Executes Cleanup's actual local-player exit/recovery block with native boundaries
/// substituted. Does not execute the full STOP entry point, destruction, or Unity ground probes.</summary>
public class StopExitExecutionTests
{
    private static readonly Lazy<MethodInfo> Replay = new(CompileReplay);

    [Fact]
    public void StopOutsideTaxi_DoesNotRecoverPlayerToTaxi()
    {
        Assert.Equal(new[] { 0, 0, 1 }, Run("outside"));
    }

    [Theory]
    [InlineData("already-exited", 0, 0)]
    [InlineData("other-vehicle", 0, 0)]
    [InlineData("stale-flag-other-vehicle", 0, 0)]
    [InlineData("unreadable-before", 0, 0)]
    [InlineData("invalid-network", 0, 0)]
    [InlineData("foreign-taxi", 0, 0)]
    [InlineData("passenger", 1, 1)]
    [InlineData("exit-noop", 1, 0)]
    [InlineData("exit-throws", 1, 0)]
    [InlineData("exit-clears-flag-only", 1, 0)]
    [InlineData("exit-enters-other", 1, 0)]
    [InlineData("exit-replaces-player", 1, 0)]
    [InlineData("unreadable-after", 1, 0)]
    public void StopRecoversOnlyExactPassengerAfterVerifiedExit(string scenario, int exits, int snaps)
    {
        int[] result = Run(scenario);
        Assert.Equal(exits, result[0]);
        Assert.Equal(snaps, result[1]);
    }

    private static int[] Run(string scenario) =>
        (int[])Replay.Value.Invoke(null, new object[] { scenario })!;

    private static MethodInfo CompileReplay()
    {
        var tree = CSharpSyntaxTree.ParseText(DriverSafetySourceContractTests.ReadSource("SpikeCommands.cs"));
        var cleanup = tree.GetRoot().DescendantNodes().OfType<MethodDeclarationSyntax>()
            .Single(m => m.Identifier.Text == "Cleanup");
        // Keep the actual statements up to the independent navigation-stop/destruction stage.
        var vehicleBlock = cleanup.Body!.Statements.OfType<IfStatementSyntax>()
            .Single(s => s.Condition.ToString() == "veh != null");
        var statements = ((BlockSyntax)vehicleBlock.Statement).Statements
            .TakeWhile(s => !s.DescendantNodes().OfType<VariableDeclarationSyntax>()
                .Any(v => v.Type.ToString() == "VehicleAgent?"));
        string source = Harness.Replace("/* EXIT_BLOCK */", string.Join("\n", statements));
        var references = ((string)AppContext.GetData("TRUSTED_PLATFORM_ASSEMBLIES")!).Split(Path.PathSeparator)
            .Select(p => MetadataReference.CreateFromFile(p));
        var compilation = CSharpCompilation.Create("StopExitReplay", new[] { CSharpSyntaxTree.ParseText(source) },
            references, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
        using var stream = new MemoryStream();
        var emitted = compilation.Emit(stream);
        Assert.True(emitted.Success, string.Join("\n", emitted.Diagnostics));
        return Assembly.Load(stream.ToArray()).GetType("Replay")!.GetMethod("Run")!;
    }

    private const string Harness = """
        using System;
        public class NativeObject {
            public IntPtr Pointer = new(1);
            public bool WasCollected;
        }
        public sealed class Player : NativeObject {
            public static Player Local;
            public NativeObject Current;
            public bool ThrowRead;
            public NativeObject CurrentVehicle {
                get { if (ThrowRead) throw new Exception("unreadable"); return Current; }
                set { Current = value; }
            }
        }
        public sealed class LandVehicle : NativeObject {
            public NativeObject NetworkObject = new() { Pointer = new(2) };
            public bool LocalPlayerIsInVehicle;
            public void ExitVehicle() {
                Replay.Exits++;
                if (Replay.Scenario == "exit-noop") return;
                LocalPlayerIsInVehicle = false;
                if (Replay.Scenario == "exit-clears-flag-only") return;
                Player.Local.CurrentVehicle = null;
                if (Replay.Scenario == "exit-throws") throw new Exception("exit failed after mutation");
                if (Replay.Scenario == "exit-enters-other") Player.Local.CurrentVehicle = new() { Pointer = new(3) };
                if (Replay.Scenario == "exit-replaces-player") Player.Local = new() { Pointer = new(9) };
                if (Replay.Scenario == "unreadable-after") Player.Local.ThrowRead = true;
            }
        }
        public sealed class Logger { public void Error(string s) { } public void Warn(string s) { } }
        public static class Mod { public static Logger Log = new(); }
        public static class Replay {
            public static int Exits, Snaps;
            public static string Scenario;
            private static void Print(string s) { }
            private static bool SafePlayerInVehicle(LandVehicle v) => v.LocalPlayerIsInVehicle;
            private static bool IsOwnedTaxi(LandVehicle v) => Scenario != "foreign-taxi";
            private static void SafeExitGroundSnap(LandVehicle v) { Snaps++; }
            public static int[] Run(string scenario) {
                Scenario = scenario;
                Exits = Snaps = 0;
                Player.Local = new();
                var veh = new LandVehicle();
                bool passenger = scenario != "outside" && scenario != "already-exited" && scenario != "other-vehicle";
                veh.LocalPlayerIsInVehicle = passenger;
                // Distinct wrappers for the same native network object: identity is not CLR reference equality.
                Player.Local.Current = passenger ? new() { Pointer = new(2) } : null;
                if (scenario.Contains("other-vehicle")) Player.Local.Current = new() { Pointer = new(3) };
                if (scenario == "invalid-network") veh.NetworkObject.Pointer = IntPtr.Zero;
                if (scenario == "unreadable-before") Player.Local.ThrowRead = true;
                bool ok = true;
                /* EXIT_BLOCK */
                return new[] { Exits, Snaps, ok ? 1 : 0 };
            }
        }
        """;
}
