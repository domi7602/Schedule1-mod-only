using System;
using System.Reflection;
using Xunit;

namespace TaxiDriver.Tests;

public class PrivateFlagsScopeTests
{
    private sealed class Flags { public bool Ignore; public int Setting; }

    [Fact]
    public void StateIsPublishedBeforeWriteSoFinalizerCanRetryFailedRestoration()
    {
        var method = typeof(PrivateFlagsScope<Flags>).GetMethod("Begin", BindingFlags.Static | BindingFlags.NonPublic)!;
        Assert.Equal(5, method.GetParameters().Length);
        var original = new Flags();
        Flags current = original;
        IDisposable? published = null;
        int writes = 0;
        Action<Flags> write = x => { current = x; if (++writes <= 2) throw new InvalidOperationException(); };
        Assert.Throws<TargetInvocationException>(() => method.Invoke(null, new object[] {
            (Func<Flags>)(() => current), write, (Func<Flags, Flags>)(x => new Flags()),
            (Func<Flags, Flags, bool>)ReferenceEquals,
            (Action<PrivateFlagsScope<Flags>>)(x => published = x) }));
        Assert.NotNull(published);
        published!.Dispose();
        Assert.Same(original, current);
    }

    [Fact]
    public void RejectsAliasedNativeCopyBeforePublishing()
    {
        var original = new Flags();
        bool wrote = false;
        Assert.Throws<InvalidOperationException>(() => PrivateFlagsScope<Flags>.Begin(
            () => original, x => wrote = true, x => x, (a, b) => true));
        Assert.False(wrote);
    }

    [Fact]
    public void PartialInstallWriteFailureRestoresOriginalBeforeRethrowing()
    {
        var original = new Flags();
        Flags current = original;
        bool fail = true;
        Assert.Throws<InvalidOperationException>(() => PrivateFlagsScope<Flags>.Begin(
            () => current, x => { current = x; if (fail) { fail = false; throw new InvalidOperationException(); } },
            x => new Flags { Ignore = true }, ReferenceEquals));
        Assert.Same(original, current);
    }

    [Fact]
    public void RestoreIgnoresControlChangesAndConsumerExceptions()
    {
        var original = new Flags { Setting = 42 };
        Flags current = original;
        bool host = true;
        var scope = PrivateFlagsScope<Flags>.Begin(() => current, x => current = x,
            x => new Flags { Ignore = true, Setting = x.Setting }, ReferenceEquals);
        try
        {
            host = false;
            throw new InvalidOperationException("native-consumer");
        }
        catch (InvalidOperationException) { scope.Dispose(); }
        Assert.False(host);
        Assert.Same(original, current);
        Assert.False(original.Ignore);
    }

    [Fact]
    public void EachRedispatchCopiesFreshNativeSettingsAndNestedCallsRestoreInOrder()
    {
        Flags current = new Flags { Setting = 1 };
        var firstOriginal = current;
        using (PrivateFlagsScope<Flags>.Begin(() => current, x => current = x,
            x => new Flags { Ignore = true, Setting = x.Setting }, ReferenceEquals))
        {
            var outer = current;
            using (PrivateFlagsScope<Flags>.Begin(() => current, x => current = x,
                x => new Flags { Ignore = true, Setting = x.Setting }, ReferenceEquals))
                Assert.NotSame(outer, current);
            Assert.Same(outer, current);
        }
        Assert.Same(firstOriginal, current);
        current = new Flags { Setting = 99 }; // native patrol/reset/redispatch replaced flags
        var nextOriginal = current;
        using (PrivateFlagsScope<Flags>.Begin(() => current, x => current = x,
            x => new Flags { Ignore = true, Setting = x.Setting }, ReferenceEquals))
            Assert.Equal(99, current.Setting);
        Assert.Same(nextOriginal, current);
    }

    [Fact]
    public void ConsumerSeesPrivateCopyAndSharedOriginalIsRestored()
    {
        var original = new Flags { Setting = 17 };
        Flags current = original;
        var scope = PrivateFlagsScope<Flags>.Begin(() => current, x => current = x,
            x => new Flags { Ignore = true, Setting = x.Setting }, ReferenceEquals);
        Assert.NotSame(original, current);
        Assert.True(current.Ignore);
        Assert.Equal(17, current.Setting);
        Assert.False(original.Ignore);
        scope.Dispose();
        Assert.Same(original, current);
        scope.Dispose();
        Assert.Same(original, current);
    }
}
