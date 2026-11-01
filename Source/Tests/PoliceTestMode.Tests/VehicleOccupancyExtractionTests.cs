using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Xunit;
namespace PoliceTestMode.Tests;

// Executes the unchanged production adapter with native getter boundaries substituted.
// Not a native/Unity simulation. The no-deploy build checks installed API compatibility.
public class VehicleOccupancyExtractionTests
{
    private static readonly Lazy<MethodInfo> Replay = new(Compile);
    [Theory]
    [InlineData("npc-only", "Clear")]
    [InlineData("empty", "Clear")]
    [InlineData("driver", "Player")]
    [InlineData("passenger", "Player")]
    [InlineData("missing", "Unknown")]
    [InlineData("zero-seats", "Unknown")]
    [InlineData("null-seat", "Unknown")]
    [InlineData("dead-seat", "Unknown")]
    [InlineData("dead-player", "Unknown")]
    [InlineData("throw-seats", "Unknown")]
    [InlineData("throw-occupant", "Unknown")]
    public void Actual_adapter_distinguishes_player_from_NPC_and_unknown(string scenario, string expected)
        => Assert.Equal(expected, Replay.Value.Invoke(null, new object[] { scenario }));

    private static MethodInfo Compile()
    {
        var root = CSharpSyntaxTree.ParseText(RuntimeContractTests.Source("PoliceWorld.cs")).GetRoot();
        var methods = root.DescendantNodes().OfType<MethodDeclarationSyntax>()
            .Where(m => m.Identifier.Text == "ReadPlayerOccupancy").ToArray();
        Assert.Single(methods);
        string source = Harness.Replace("/* ADAPTER */", methods[0].ToString());
        var references = ((string)AppContext.GetData("TRUSTED_PLATFORM_ASSEMBLIES")!).Split(Path.PathSeparator)
            .Select(p => MetadataReference.CreateFromFile(p));
        var compilation = CSharpCompilation.Create("OccupancyReplay", new[] {
            CSharpSyntaxTree.ParseText(source), CSharpSyntaxTree.ParseText(RuntimeContractTests.Source("SessionPolicy.cs"))
        }, references, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
        using var stream = new MemoryStream();
        var emitted = compilation.Emit(stream);
        Assert.True(emitted.Success, string.Join("\n", emitted.Diagnostics));
        return Assembly.Load(stream.ToArray()).GetType("PoliceTestMode.Replay")!.GetMethod("Run")!;
    }

    private const string Harness = """
        using System;
        namespace PoliceTestMode;
        public class Native { public bool Alive = true; }
        public class Player : Native { }
        public class VehicleSeat : Native {
            public Player Value;
            public bool Throw;
            public Player Occupant => Throw ? throw new Exception("unreadable occupant") : Value;
        }
        public class LandVehicle : Native {
            public VehicleSeat[] Values;
            public bool Throw;
            public bool IsOccupied; // NPC-only reproduction: true despite all Player occupants null.
            public VehicleSeat[] Seats => Throw ? throw new Exception("unreadable seats") : Values;
        }
        public static class NetworkGuard {
            public static bool IsAlive(Native value) => value != null && value.Alive;
        }
        public class Replay {
            /* ADAPTER */
            public static string Run(string scenario) {
                var vehicle = new LandVehicle { Values = new[] { new VehicleSeat(), new VehicleSeat() } };
                switch (scenario) {
                    case "npc-only": vehicle.IsOccupied = true; break;
                    case "driver": vehicle.Values[0].Value = new Player(); break;
                    case "passenger": vehicle.Values[1].Value = new Player(); break;
                    case "missing": vehicle.Values = null; break;
                    case "zero-seats": vehicle.Values = Array.Empty<VehicleSeat>(); break;
                    case "null-seat": vehicle.Values[1] = null; break;
                    case "dead-seat": vehicle.Values[1].Alive = false; break;
                    case "dead-player": vehicle.Values[1].Value = new Player { Alive = false }; break;
                    case "throw-seats": vehicle.Throw = true; break;
                    case "throw-occupant": vehicle.Values[1].Throw = true; break;
                }
                return ReadPlayerOccupancy(vehicle).ToString();
            }
        }
        """;
}
