using System.Runtime.CompilerServices;
using Xunit;
namespace TaxiDriver.Tests;

public class CallbackLeaseTests
{
    [MethodImpl(MethodImplOptions.NoInlining)]
    private static (CallbackLease<string> Lease, WeakReference Root) Create()
    {
        object app = new();
        return (new CallbackLease<string>(_ => GC.KeepAlive(app)), new WeakReference(app));
    }
    [Fact]
    public void RetainedEventDelegateDoesNotKeepAppAliveAfterRelease()
    {
        var fixture = Create();
        Action<string> nativeDelegate = fixture.Lease.Invoke;
        nativeDelegate("search");
        fixture.Lease.Release();
        fixture.Lease.Release();
        GC.Collect(); GC.WaitForPendingFinalizers(); GC.Collect();
        Assert.False(fixture.Root.IsAlive);
        nativeDelegate("event after native removal failure");
        GC.KeepAlive(nativeDelegate);
    }
    [Fact]
    public void NoArgumentButtonsUseTheSameReleasableTarget()
    {
        int calls = 0;
        var lease = new CallbackLease<bool>(_ => calls++);
        Action button = lease.Invoke;
        button();
        lease.Release();
        button();
        Assert.Equal(1, calls);
    }

    [Fact]
    public void ReleaseSuppressesLaterCallbacks()
    {
        int calls = 0;
        var lease = new CallbackLease<string>(_ => calls++);
        lease.Invoke("first");
        lease.Release();
        lease.Invoke("stale");
        Assert.Equal(1, calls);
    }
}
