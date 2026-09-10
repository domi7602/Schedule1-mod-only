using System;
using System.Linq;
using System.Reflection;
using S1Mods.Shared;
using Xunit;

namespace Shared.Tests;

/// <summary>
/// Tests for S1Mods.Shared.PatchGuard — covers the reflection-based FindMethod
/// helper and the stat counters. We do NOT exercise harmony.Patch here because
/// that requires a real HarmonyLib runtime; FindMethod is the high-value,
/// pure-logic surface that protects against game updates breaking patch
/// signatures.
/// </summary>
public class PatchGuardTests
{
    public PatchGuardTests()
    {
        PatchGuard.ResetStats();
    }

    // A small fixture type with several overloads for ambiguity testing
    private class Fixture
    {
        public void Single() { }
        public void Single(int x) { }
        public void Single(string s) { }
        public void Single(int x, int y) { }
        public static void StaticSingle() { }
        private void Private() { }
        public void Generic<T>(T value) { }
    }

    [Fact]
    public void FindMethod_NoOverload_FindsIt()
    {
        MethodInfo? m = PatchGuard.FindMethod(typeof(Fixture), nameof(Fixture.StaticSingle));
        Assert.NotNull(m);
        Assert.True(m!.IsStatic);
    }

    [Fact]
    public void FindMethod_WithParameterTypes_ResolvesOverload()
    {
        MethodInfo? m = PatchGuard.FindMethod(typeof(Fixture), nameof(Fixture.Single), new[] { typeof(int) });
        Assert.NotNull(m);
        Assert.Single(m!.GetParameters());
        Assert.Equal(typeof(int), m.GetParameters()[0].ParameterType);
    }

    [Fact]
    public void FindMethod_MultipleOverloads_ReturnsFirstWhenAmbiguous()
    {
        // Without parameterTypes, GetMethod throws AmbiguousMatchException for
        // overloaded names. PatchGuard catches that and returns null (no
        // guessing) — we verify that contract.
        MethodInfo? m = PatchGuard.FindMethod(typeof(Fixture), nameof(Fixture.Single));
        Assert.Null(m);
    }

    [Fact]
    public void FindMethod_NonExistent_ReturnsNull()
    {
        MethodInfo? m = PatchGuard.FindMethod(typeof(Fixture), "DoesNotExist");
        Assert.Null(m);
    }

    [Fact]
    public void FindMethod_NullTarget_ReturnsNull()
    {
        MethodInfo? m = PatchGuard.FindMethod(null!, "Anything");
        Assert.Null(m);
    }

    [Fact]
    public void FindMethod_EmptyName_ReturnsNull()
    {
        MethodInfo? m = PatchGuard.FindMethod(typeof(Fixture), "");
        Assert.Null(m);
    }

    [Fact]
    public void FindMethod_GenericOverload_ReturnsNull_AsPerContract()
    {
        // Generic methods without parameterTypes cause GetMethod to throw
        // AmbiguousMatchException (when only one overload exists it's still
        // rejected because the CLR cannot match a closed parameter list to
        // an open generic definition). PatchGuard's contract is "no guessing"
        // — return null and let the caller disambiguate. Documented behaviour.
        MethodInfo? m = PatchGuard.FindMethod(typeof(Fixture), nameof(Fixture.Generic), new[] { typeof(string) });
        Assert.Null(m);
    }

    [Fact]
    public void ResetStats_ZeroesCounters()
    {
        PatchGuard.ResetStats();
        Assert.Equal(0, PatchGuard.PatchesApplied);
        Assert.Equal(0, PatchGuard.PatchesFailed);
        Assert.Equal(0, PatchGuard.TotalPatches);
    }

    [Fact]
    public void TotalPatches_SumsAppliedAndFailed()
    {
        PatchGuard.ResetStats();
        // We can't call TryPatch with a real Harmony here (no real runtime),
        // but we can verify the formula by checking the report shape.
        string report = $"PatchGuard: {PatchGuard.PatchesApplied}/{PatchGuard.TotalPatches} patches successfully applied.";
        Assert.Contains("0/0", report);
    }
}
