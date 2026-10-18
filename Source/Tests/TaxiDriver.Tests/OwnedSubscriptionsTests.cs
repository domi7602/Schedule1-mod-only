using Xunit;
namespace TaxiDriver.Tests;

public class OwnedSubscriptionsTests
{
    private sealed class FakeHelper
    {
        internal readonly HashSet<Delegate> Roots = new();
        internal void Add(Action listener, object evt) => Roots.Add(listener);
        internal void Remove(Action listener, object evt) => Roots.Remove(listener);
        internal void Forget(Action listener) => Roots.Remove(listener);
    }
    [Fact]
    public void PartialRegistrationFailureReleasesTheHelperRoot()
    {
        var helper = new FakeHelper();
        var owned = new OwnedSubscriptions();
        Action listener = () => { };
        Assert.Throws<InvalidOperationException>(() => owned.Add(listener, new object(),
            (action, evt) => { helper.Add(action, evt); throw new InvalidOperationException(); },
            helper.Remove, helper.Forget));
        Assert.Empty(helper.Roots);
        Assert.Equal(0, owned.Count);
    }

    [Fact]
    public void DeadEventFailureStillForgetsStaticRootAndContinuesDisposal()
    {
        var helper = new FakeHelper();
        var owned = new OwnedSubscriptions();
        object deadWrapper = new();
        Action first = () => GC.KeepAlive(deadWrapper);
        Action second = () => { };
        owned.Add(first, deadWrapper, helper.Add,
            (_, _) => throw new InvalidOperationException("native event died"), helper.Forget);
        owned.Add(second, new object(), helper.Add, helper.Remove, helper.Forget);
        owned.Clear();
        owned.Clear();
        Assert.Empty(helper.Roots);
        Assert.Equal(0, owned.Count);
    }

    [Fact]
    public void ThrowingForgetDoesNotBlockOtherOwnedRegistrations()
    {
        var helper = new FakeHelper();
        var owned = new OwnedSubscriptions();
        Action first = () => { };
        Action second = () => { };
        owned.Add(first, new object(), helper.Add, helper.Remove,
            _ => throw new InvalidOperationException());
        owned.Add(second, new object(), helper.Add, helper.Remove, helper.Forget);
        owned.Clear();
        Assert.Empty(helper.Roots);
        Assert.Equal(0, owned.Count);
    }

    [Fact]
    public void RebuildCyclesKeepRootsBoundedAndReleaseAppRoots()
    {
        var helper = new FakeHelper();
        var rows = new OwnedSubscriptions();
        var app = new OwnedSubscriptions();
        for (int i = 0; i < 9; i++)
        {
            object root = new();
            Action callback = () => GC.KeepAlive(root);
            app.Add(callback, root, helper.Add, helper.Remove, helper.Forget);
        }
        for (int cycle = 0; cycle < 100; cycle++)
        {
            rows.Clear();
            Assert.Equal(9, helper.Roots.Count); // before any row destroy
            for (int i = 0; i < 30; i++)
            {
                object root = new();
                Action callback = () => GC.KeepAlive(root);
                rows.Add(callback, root, helper.Add, helper.Remove, helper.Forget);
            }
            Assert.Equal(39, helper.Roots.Count);
        }
        rows.Clear(); app.Clear(); app.Clear(); rows.Clear();
        Assert.Empty(helper.Roots);
        Assert.Equal(0, rows.Count);
        Assert.Equal(0, app.Count);
    }
}
