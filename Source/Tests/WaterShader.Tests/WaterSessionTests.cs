using WaterShader;
using Xunit;

namespace WaterShader.Tests;

public sealed class WaterSessionTests
{
    [Fact]
    public void CachedWaterNeverRepeatsDiscoveryOrMaterialCreation()
    {
        var runtime = new FakeRuntime();
        var session = new WaterSession<Handle, Handle>(runtime);
        session.Start(0);
        session.Tick(0);
        session.Tick(5);
        session.Tick(30);
        Assert.Equal(1, runtime.Discoveries);
        Assert.Equal(1, runtime.Clones);
        Assert.Same(runtime.Clone, runtime.Target.Material);
    }

    [Fact]
    public void MissingWaterRetriesOnlyAtDeadlineAndCanLoadLate()
    {
        var runtime = new FakeRuntime { Available = false };
        var session = new WaterSession<Handle, Handle>(runtime);
        session.Start(0);
        session.Tick(0);
        session.Tick(0.01);
        session.Tick(4.999);
        Assert.Equal(1, runtime.Discoveries);
        runtime.Available = true;
        session.Tick(5);
        Assert.Equal(2, runtime.Discoveries);
        Assert.Same(runtime.Clone, runtime.Target.Material);
    }

    [Fact]
    public void FakeNullRendererIsReleasedBeforeDelayedRediscovery()
    {
        var runtime = new FakeRuntime();
        var session = new WaterSession<Handle, Handle>(runtime);
        session.Start(0);
        session.Tick(0);
        runtime.Target.Alive = false;
        session.Tick(1);
        Assert.Equal(1, runtime.Destroys);
        Assert.Equal(1, runtime.Discoveries);
        runtime.Target = new Handle { Material = runtime.Original };
        session.Tick(5.999);
        Assert.Equal(1, runtime.Discoveries);
        session.Tick(6);
        Assert.Equal(2, runtime.Discoveries);
        Assert.Same(runtime.Clone, runtime.Target.Material);
    }

    [Fact]
    public void MissingTemplateDoesNotPoisonTheRendererCache()
    {
        var runtime = new FakeRuntime();
        runtime.Target.Material = null;
        var session = new WaterSession<Handle, Handle>(runtime);
        session.Start(0);
        session.Tick(0);
        runtime.Target.Material = runtime.Original;
        session.Tick(5);
        Assert.Equal(1, runtime.Clones);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void StopRestoresOnlyOurMaterialAndDestroysOnlyOurClone(bool foreignReplacement)
    {
        var runtime = new FakeRuntime();
        var session = new WaterSession<Handle, Handle>(runtime);
        session.Start(0);
        session.Tick(0);
        var expected = foreignReplacement ? new Handle() : runtime.Original;
        if (foreignReplacement) runtime.Target.Material = expected;
        session.Stop();
        session.Stop();
        session.Tick(100);
        Assert.Same(expected, runtime.Target.Material);
        Assert.Equal(1, runtime.Destroys);
        Assert.True(runtime.Original.Alive);
        Assert.Equal(1, runtime.Discoveries);
        session.Start(101);
        session.Tick(101);
        Assert.Equal(2, runtime.Discoveries);
    }

    [Fact]
    public void ForeignReplacementIsNotFoughtByPolling()
    {
        var runtime = new FakeRuntime();
        var session = new WaterSession<Handle, Handle>(runtime);
        session.Start(0);
        session.Tick(0);
        var foreign = new Handle();
        runtime.Target.Material = foreign;
        session.Tick(1);
        session.Tick(100);
        Assert.Equal(1, runtime.Destroys);
        Assert.Same(foreign, runtime.Target.Material);
        Assert.Equal(1, runtime.Discoveries);
        Assert.Equal(1, runtime.Clones);
    }

    [Fact]
    public void FailedWriteAfterAssignmentIsRestoredBeforeRetry()
    {
        var runtime = new FakeRuntime { ThrowAfterWrite = true };
        var session = new WaterSession<Handle, Handle>(runtime);
        session.Start(0);
        Assert.Null(Record.Exception(() => session.Tick(0)));
        Assert.Equal(1, runtime.Clones);
        session.Tick(4.99);
        Assert.Equal(1, runtime.Clones);
        runtime.ThrowAfterWrite = false;
        session.Tick(5);
        Assert.Same(runtime.Original, runtime.Target.Material);
        Assert.Equal(1, runtime.Destroys);
        session.Tick(10);
        Assert.Equal(2, runtime.Clones);
    }

    [Fact]
    public void UnreadableOwnershipNeverDestroysPossiblyAttachedClone()
    {
        var runtime = new FakeRuntime();
        var session = new WaterSession<Handle, Handle>(runtime);
        session.Start(0);
        session.Tick(0);
        runtime.ThrowRead = true;
        Assert.Null(Record.Exception(session.Stop));
        Assert.Equal(0, runtime.Destroys);
        Assert.True(runtime.Clone.Alive);
        runtime.ThrowRead = false;
        session.Start(10);
        session.Tick(10);
        Assert.Same(runtime.Original, runtime.Target.Material);
        Assert.Equal(1, runtime.Destroys);
        session.Tick(15);
        Assert.Equal(2, runtime.Clones);
    }

    [Fact]
    public void DeadOwnedMaterialInvalidatesBindingAndRestoresOriginal()
    {
        var runtime = new FakeRuntime();
        var session = new WaterSession<Handle, Handle>(runtime);
        session.Start(0);
        session.Tick(0);
        runtime.Clone.Alive = false;
        session.Tick(1);
        Assert.Same(runtime.Original, runtime.Target.Material);
        session.Tick(6);
        Assert.Equal(2, runtime.Clones);
    }

    [Fact]
    public void IdleTicksAllocateNothingAndPerformNoNativeReads()
    {
        var runtime = new FakeRuntime();
        var session = new WaterSession<Handle, Handle>(runtime);
        session.Start(0);
        session.Tick(0);
        session.Tick(0.5);
        int reads = runtime.Reads;
        long before = GC.GetAllocatedBytesForCurrentThread();
        for (int i = 0; i < 10000; i++) session.Tick(0.5);
        long allocated = GC.GetAllocatedBytesForCurrentThread() - before;
        Assert.Equal(0, allocated);
        Assert.Equal(reads, runtime.Reads);
    }

    [Fact]
    public void TuningFailureRetainsCloneForCleanupWithoutAttachingIt()
    {
        var runtime = new FakeRuntime { ThrowConfigure = true };
        var session = new WaterSession<Handle, Handle>(runtime);
        session.Start(0);
        session.Tick(0);
        Assert.Equal(WaterState.Faulted, session.State);
        Assert.Same(runtime.Original, runtime.Target.Material);
        session.Tick(5);
        Assert.Equal(1, runtime.Destroys);
    }

    [Fact]
    public void IgnoredRestoreDoesNotDestroyAttachedClone()
    {
        var runtime = new FakeRuntime();
        var session = new WaterSession<Handle, Handle>(runtime);
        session.Start(0);
        session.Tick(0);
        runtime.IgnoreWrites = true;
        session.Stop();
        Assert.Equal(0, runtime.Destroys);
        Assert.Equal(WaterState.Faulted, session.State);
        runtime.IgnoreWrites = false;
        session.Stop();
        Assert.Same(runtime.Original, runtime.Target.Material);
        Assert.Equal(1, runtime.Destroys);
    }

    [Fact]
    public void IgnoredApplicationIsNotReportedAsApplied()
    {
        var runtime = new FakeRuntime { IgnoreWrites = true };
        var session = new WaterSession<Handle, Handle>(runtime);
        session.Start(0);
        session.Tick(0);
        Assert.Equal(WaterState.Faulted, session.State);
        session.Tick(5);
        Assert.Equal(1, runtime.Destroys);
    }

    internal sealed class Handle
    {
        public bool Alive = true;
        public Handle? Material;
    }

    internal sealed class FakeRuntime : IWaterRuntime<Handle, Handle>
    {
        public Handle Original = new();
        public Handle Clone = new();
        public Handle Target;
        public bool Available = true;
        public bool ThrowRead, ThrowAfterWrite, ThrowConfigure, IgnoreWrites;
        public void ConfigureMaterial(Handle material)
        {
            if (ThrowConfigure) throw new InvalidOperationException();
        }
        public int Discoveries, Clones, Writes, Destroys, Reads;
        public FakeRuntime() { Target = new Handle { Material = Original }; }
        public Handle? Discover() { Discoveries++; return Available ? Target : null; }
        public bool TargetAlive(Handle value) => value.Alive;
        public bool MaterialAlive(Handle value) => value.Alive;
        public Handle? ReadMaterial(Handle target) { Reads++; if (ThrowRead) throw new InvalidOperationException(); return target.Material; }
        public Handle CreateMaterial(Handle template) { Clones++; Clone = new Handle(); return Clone; }
        public void WriteMaterial(Handle target, Handle? material) { Writes++; if (!IgnoreWrites) target.Material = material; if (ThrowAfterWrite) throw new InvalidOperationException(); }
        public bool SameMaterial(Handle? left, Handle right) => ReferenceEquals(left, right);
        public void DestroyMaterial(Handle material) { Destroys++; material.Alive = false; }
    }
}
