using System.Reflection;
using Xunit;

namespace TaxiDriver.Tests;

public class TaxiLifecycleTests
{
    [Fact]
    public void ShutdownClosesGateBeforeFirstPhaseAndPreservesCleanupOrder()
    {
        var policy = new TaxiLifecycle();
        Type type = policy.GetType();
        // Reflection keeps the initial missing-API regression executable, not a compile error.
        var start = type.GetMethod("Initialize", BindingFlags.Instance | BindingFlags.NonPublic);
        Assert.NotNull(start);
        Assert.True((bool)start!.Invoke(policy, null)!);
        var shutdown = type.GetMethod("Shutdown", BindingFlags.Instance | BindingFlags.NonPublic);
        Assert.NotNull(shutdown);
        var accepting = type.GetProperty("AcceptingWork", BindingFlags.Instance | BindingFlags.NonPublic);
        Assert.NotNull(accepting);
        var order = new List<string>();
        Action phase(string label) => () =>
        {
            Assert.False((bool)accepting!.GetValue(policy)!);
            order.Add(label);
        };
        shutdown!.Invoke(policy, new object[]
        {
            new (string, Action)[] { ("quiesce", phase("quiesce")), ("meter", phase("meter")),
                ("patrol", phase("patrol")), ("vehicle", phase("vehicle")),
                ("trunk", phase("trunk")), ("passenger", phase("passenger")), ("managed", phase("managed")) },
            new (string, Action)[] { ("hooks", phase("hooks")) },
            (Action<string, Exception>)((_, ex) => throw ex)
        });
        Assert.Equal(new[] { "quiesce", "meter", "patrol", "vehicle", "trunk", "passenger", "managed", "hooks" }, order);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    [InlineData(6)]
    [InlineData(7)]
    [InlineData(8)]
    [InlineData(9)]
    [InlineData(10)]
    [InlineData(11)]
    public void EveryFailureIncludingReporterStillRunsLaterPhasesAndAllFinalizers(int throwingPhase)
    {
        var policy = new TaxiLifecycle();
        policy.Initialize();
        var visited = new List<int>();
        var failures = new List<string>();
        (string, Action) step(int index) => ($"phase-{index}", () =>
        {
            Assert.False(policy.AcceptingWork);
            visited.Add(index);
            if (index == throwingPhase) throw new InvalidOperationException("native phase failed");
        }
        );
        policy.Shutdown(Enumerable.Range(0, 7).Select(step).ToArray(),
            Enumerable.Range(7, 5).Select(step).ToArray(), (label, _) =>
            {
                failures.Add(label);
                throw new InvalidOperationException("logger failed too");
            });
        Assert.Equal(Enumerable.Range(0, 12), visited);
        Assert.Equal(new[] { $"phase-{throwingPhase}" }, failures);
        Assert.False(policy.AcceptingWork);
    }

    [Fact]
    public void RepeatedAndReentrantShutdownIsOneShotEvenBeforeInitialization()
    {
        var policy = new TaxiLifecycle();
        int settled = 0, finalized = 0;
        (string, Action)[] final = { ("hooks", () => finalized++) };
        (string, Action)[] phases = { ("meter", () =>
        {
            settled++;
            policy.Shutdown(Array.Empty<(string, Action)>(), final, (_, _) => { });
        }) };
        policy.Shutdown(phases, final, (_, _) => { });
        policy.Shutdown(phases, final, (_, _) => { });
        Assert.Equal(1, settled);
        Assert.Equal(1, finalized);
        Assert.False(policy.AcceptingWork);
    }

    [Fact]
    public void InitializationCannotReopenGateFromInsideCleanup()
    {
        var policy = new TaxiLifecycle();
        policy.Initialize();
        bool reopened = false;
        policy.Shutdown(new (string, Action)[] { ("restore", () => reopened = policy.Initialize()) },
            Array.Empty<(string, Action)>(), (_, _) => { });
        Assert.False(reopened);
        Assert.False(policy.AcceptingWork);
        Assert.True(policy.Initialize());
        Assert.True(policy.AcceptingWork);
    }

    [Fact]
    public void DeferredCallbacksStayInertAfterShutdownAndReinitialization()
    {
        var policy = new TaxiLifecycle();
        var method = typeof(TaxiLifecycle).GetMethod("Guard", BindingFlags.Instance | BindingFlags.NonPublic);
        Assert.NotNull(method);
        int calls = 0;
        Action<int> target = value => calls += value;
        var beforeInit = (Action<int>)method!.MakeGenericMethod(typeof(int)).Invoke(policy, new object[] { target })!;
        Assert.True(policy.Initialize());
        var pending = (Action<int>)method.MakeGenericMethod(typeof(int)).Invoke(policy, new object[] { target })!;
        pending(1);
        Assert.Equal(1, calls);
        policy.Shutdown(new (string, Action)[] { ("quiesce", () => pending(10)) },
            Array.Empty<(string, Action)>(), (_, _) => { });
        pending(10);
        Assert.Equal(1, calls);
        Assert.True(policy.Initialize());
        pending(100);
        beforeInit(100);
        Assert.Equal(1, calls);
        var current = (Action<int>)method.MakeGenericMethod(typeof(int)).Invoke(policy, new object[] { target })!;
        Assert.True(policy.Initialize()); // duplicate init must not invalidate current callbacks
        current(2);
        Assert.Equal(3, calls);
    }

    [Fact]
    public void RuntimeLifecyclePolicyIsAvailableToExecute()
    {
        Assert.NotNull(typeof(TaxiLifecycleTests).Assembly.GetType("TaxiDriver.TaxiLifecycle"));
    }
}
