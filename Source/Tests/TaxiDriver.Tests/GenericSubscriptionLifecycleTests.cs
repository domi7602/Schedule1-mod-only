using Xunit;
namespace TaxiDriver.Tests;

// Models the installed generic S1API remover's delete-after-native-remove ordering.
// This executes the runtime registry and lease, not Unity or the S1API DLL.
public class GenericSubscriptionLifecycleTests
{
    private sealed class FakeEvent
    {
        internal bool Dead;
        internal readonly HashSet<Action<string>> Native = new();
        public static bool operator ==(FakeEvent? a, FakeEvent? b) =>
            ReferenceEquals(a, b) || (ReferenceEquals(b, null) && a is { Dead: true });
        public static bool operator !=(FakeEvent? a, FakeEvent? b) => !(a == b);
        public override bool Equals(object? obj) => ReferenceEquals(this, obj);
        public override int GetHashCode() => System.Runtime.CompilerServices.RuntimeHelpers.GetHashCode(this);
    }
    private sealed class FakeGenericHelper
    {
        internal readonly HashSet<Action<string>> StaticRoots = new();
        internal void Add(Action<string> action, FakeEvent evt)
        {
            evt.Native.Add(action);
            StaticRoots.Add(action);
        }
        internal void Remove(Action<string> action, FakeEvent evt)
        {
            if (!StaticRoots.Contains(action)) return;
            if (evt.Dead) throw new InvalidOperationException("native wrapper died");
            evt.Native.Remove(action);
            StaticRoots.Remove(action);
        }
    }
    [Fact]
    public void FakeNullDeadEventUsesCapturedIdentityAndNeutralHelperRemoval()
    {
        var helper = new FakeGenericHelper();
        var evt = new FakeEvent();
        int calls = 0;
        var lease = new CallbackLease<string>(_ => calls++);
        Action<string> listener = lease.Invoke;
        var registry = new OwnedSubscriptions();
        registry.Add(listener, evt, helper.Add,
            (action, source) => { Assert.Same(evt, source); lease.Release(); helper.Remove(action, source); },
            action => { lease.Release(); helper.Remove(action, new FakeEvent()); });
        listener("search");
        evt.Dead = true;
        Assert.True(evt == null); // overloaded Unity-like null must never bypass registry cleanup
        registry.Clear();
        Assert.Empty(helper.StaticRoots);
        Assert.Equal(0, registry.Count);
        listener("late callback");
        Assert.Equal(1, calls);
        registry.Clear();
    }
    [Fact]
    public void OrdinaryTeardownRemovesExactDelegateFromNativeEventAndStaticHelper()
    {
        var helper = new FakeGenericHelper();
        var evt = new FakeEvent();
        var lease = new CallbackLease<string>(_ => { });
        Action<string> listener = lease.Invoke;
        var registry = new OwnedSubscriptions();
        registry.Add(listener, evt, helper.Add,
            (action, source) => { lease.Release(); helper.Remove(action, source); },
            action => helper.Remove(action, new FakeEvent()));
        registry.Clear();
        Assert.Empty(evt.Native);
        Assert.Empty(helper.StaticRoots);
    }
}
