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

    [Fact]
    public void MissingOriginalRetainsTheVisibleCloneWithoutWritingNull()
    {
        var runtime = new FakeRuntime();
        var session = new WaterSession<Handle, Handle>(runtime);
        session.Start(0);
        session.Tick(0);
        runtime.Original.Alive = false;
        int writes = runtime.Writes;
        session.Stop();
        Assert.Equal(WaterState.Faulted, session.State);
        Assert.Equal(writes, runtime.Writes);
        Assert.Same(runtime.Clone, runtime.Target.Material);
        Assert.Equal(0, runtime.Destroys);
        // A later explicit enable reconciles the retained ownership before allocation.
        runtime.Original.Alive = true;
        session.Start(10);
        session.Tick(10);
        Assert.Same(runtime.Original, runtime.Target.Material);
        Assert.Equal(1, runtime.Clones);
        Assert.Equal(1, runtime.Destroys);
        session.Tick(15);
        Assert.Equal(WaterState.Applied, session.State);
        Assert.Equal(2, runtime.Clones);
    }

    [Fact]
    public void RestoreSetterMutationThenExceptionRetainsOwnershipUntilConfirmedDetached()
    {
        var runtime = new FakeRuntime();
        var session = new WaterSession<Handle, Handle>(runtime);
        session.Start(0);
        session.Tick(0);
        runtime.ThrowAfterWrite = true;
        session.Stop();
        Assert.Equal(WaterState.Faulted, session.State);
        Assert.Same(runtime.Original, runtime.Target.Material);
        Assert.Equal(0, runtime.Destroys);
        runtime.ThrowAfterWrite = false;
        session.Stop();
        Assert.Equal(WaterState.Stopped, session.State);
        Assert.Equal(1, runtime.Destroys);
    }

    [Fact]
    public void UnreadableTargetLivenessDoesNotCountAsDestroyed()
    {
        var runtime = new FakeRuntime();
        var session = new WaterSession<Handle, Handle>(runtime);
        session.Start(0);
        session.Tick(0);
        runtime.ThrowTargetAlive = true;
        session.Stop();
        Assert.Equal(WaterState.Faulted, session.State);
        Assert.Equal(0, runtime.Destroys);
        Assert.Same(runtime.Clone, runtime.Target.Material);
    }

    [Fact]
    public void LiveShaderUpdateFailureUsesBoundedCleanupBeforeReallocation()
    {
        var runtime = new FakeRuntime();
        var session = new WaterSession<Handle, Handle>(runtime);
        session.Start(0);
        session.Tick(0);
        var failure = new InvalidOperationException("Native shader fallback failed");
        session.ReportFailure(failure, 0.5);
        Assert.Equal(WaterState.Faulted, session.State);
        Assert.Same(failure, session.LastError);
        session.Tick(1);
        session.Tick(5.499);
        Assert.Equal(0, runtime.Destroys);
        Assert.Equal(1, runtime.Clones);
        session.Tick(5.5);
        Assert.Equal(WaterState.Waiting, session.State);
        Assert.Same(runtime.Original, runtime.Target.Material);
        Assert.Equal(1, runtime.Destroys);
        Assert.Equal(1, runtime.Clones);
        session.Tick(10.5);
        Assert.Equal(WaterState.Applied, session.State);
        Assert.Equal(2, runtime.Clones);
    }

    [Fact]
    public void RepeatedEnableAndSceneReentryRestoreEachGeneration()
    {
        var runtime = new FakeRuntime();
        var session = new WaterSession<Handle, Handle>(runtime);
        for (int i = 0; i < 4; i++)
        {
            session.Start(i * 10);
            session.Start(i * 10);
            session.Tick(i * 10);
            Assert.Equal(WaterState.Applied, session.State);
            Assert.Same(runtime.Clone, runtime.Target.Material);
            session.Stop();
            Assert.Equal(WaterState.Stopped, session.State);
            Assert.Same(runtime.Original, runtime.Target.Material);
            if (i == 1) runtime.Target = new Handle { Material = runtime.Original };
        }
        Assert.Equal(4, runtime.Clones);
        Assert.Equal(4, runtime.Destroys);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void DisabledTicksRecoverTransientRestoreFailureWithoutReapplying(bool setterFailure)
    {
        var runtime = new FakeRuntime();
        var session = new WaterSession<Handle, Handle>(runtime);
        session.Start(0);
        session.Tick(0);
        runtime.ThrowRead = !setterFailure;
        runtime.ThrowBeforeWrite = setterFailure;
        session.Stop();
        Assert.Equal(WaterState.Faulted, session.State);
        Assert.Same(runtime.Clone, runtime.Target.Material);
        Assert.Equal(0, runtime.Destroys);
        var appliedWork = (runtime.Discoveries, runtime.Clones, runtime.Configurations);
        int reads = runtime.Reads;
        int writes = runtime.Writes;
        runtime.ThrowRead = runtime.ThrowBeforeWrite = false;
        session.Tick(0.1);
        session.Tick(4.999);
        Assert.Equal(reads, runtime.Reads);
        Assert.Equal(writes, runtime.Writes);
        session.Tick(5);
        Assert.Equal(WaterState.Stopped, session.State);
        Assert.Null(session.LastError);
        Assert.Same(runtime.Original, runtime.Target.Material);
        Assert.Equal(1, runtime.Destroys);
        Assert.Equal(appliedWork, (runtime.Discoveries, runtime.Clones, runtime.Configurations));
        var completedWork = (runtime.Reads, runtime.Writes, runtime.Destroys);
        session.Tick(5);
        session.Tick(10);
        session.Tick(10000);
        Assert.Equal(completedWork, (runtime.Reads, runtime.Writes, runtime.Destroys));
        Assert.Equal(appliedWork, (runtime.Discoveries, runtime.Clones, runtime.Configurations));
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void InactiveCleanupIsCadencedAndFiniteWithExplicitRecoveryAfterExhaustion(bool recoverByEnable)
    {
        var runtime = new FakeRuntime();
        var session = new WaterSession<Handle, Handle>(runtime);
        session.Start(0);
        session.Tick(0);
        runtime.ThrowRead = true;
        session.Stop();
        int reads = runtime.Reads;
        // Three automatic restore-only attempts, each at least five seconds apart.
        for (int retry = 1; retry <= 3; retry++)
        {
            double deadline = retry * 5;
            for (int frame = 0; frame < 100; frame++) session.Tick(deadline - 0.001);
            Assert.Equal(reads + retry - 1, runtime.Reads);
            session.Tick(deadline);
            Assert.Equal(reads + retry, runtime.Reads);
            session.Tick(deadline);
            Assert.Equal(reads + retry, runtime.Reads);
        }
        var exhaustedWork = (runtime.Reads, runtime.Writes, runtime.Destroys);
        var retainedError = session.LastError;
        Assert.NotNull(retainedError);
        for (int frame = 0; frame < 100; frame++) session.Tick(20 + frame * 100);
        runtime.ThrowRead = false;
        session.Tick(10000);
        Assert.Equal(exhaustedWork, (runtime.Reads, runtime.Writes, runtime.Destroys));
        Assert.Equal(WaterState.Faulted, session.State);
        Assert.Same(retainedError, session.LastError);
        Assert.Same(runtime.Clone, runtime.Target.Material);
        Assert.True(runtime.Clone.Alive);
        Assert.Equal((1, 1, 1), (runtime.Discoveries, runtime.Clones, runtime.Configurations));
        if (recoverByEnable)
        {
            session.Start(10001);
            session.Tick(10001);
            Assert.Equal(WaterState.Waiting, session.State);
        }
        else
        {
            session.Stop();
            Assert.Equal(WaterState.Stopped, session.State);
        }
        Assert.Same(runtime.Original, runtime.Target.Material);
        Assert.Equal(1, runtime.Destroys);
        Assert.Equal((1, 1, 1), (runtime.Discoveries, runtime.Clones, runtime.Configurations));
    }

    [Fact]
    public void InactiveCleanupDeadlineStartsAtTheActualStopEvent()
    {
        var runtime = new FakeRuntime();
        var session = new WaterSession<Handle, Handle>(runtime);
        session.Start(10);
        session.Tick(10);
        runtime.ThrowRead = true;
        session.Stop(50);
        int reads = runtime.Reads;
        runtime.ThrowRead = false;
        session.Tick(50);
        session.Tick(54.999);
        Assert.Equal(reads, runtime.Reads);
        session.Tick(55);
        Assert.Equal(WaterState.Stopped, session.State);
        Assert.Same(runtime.Original, runtime.Target.Material);
        Assert.Equal(1, runtime.Destroys);
        Assert.Equal((1, 1, 1), (runtime.Discoveries, runtime.Clones, runtime.Configurations));
    }

    [Fact]
    public void InactiveCleanupDoesNotCatchUpAfterAStalledFrame()
    {
        var runtime = new FakeRuntime();
        var session = new WaterSession<Handle, Handle>(runtime);
        session.Start(0);
        session.Tick(0);
        runtime.ThrowRead = true;
        session.Stop(0);
        int reads = runtime.Reads;
        session.Tick(10000);
        Assert.Equal(reads + 1, runtime.Reads);
        session.Tick(10000);
        session.Tick(10004.999);
        Assert.Equal(reads + 1, runtime.Reads);
        session.Tick(10005);
        Assert.Equal(reads + 2, runtime.Reads);
        Assert.Equal((1, 1, 1), (runtime.Discoveries, runtime.Clones, runtime.Configurations));
    }

    [Fact]
    public void InactiveRetriesRetainTheVisibleCloneIfOriginalMaterialIsUnavailable()
    {
        var runtime = new FakeRuntime();
        var session = new WaterSession<Handle, Handle>(runtime);
        session.Start(0);
        session.Tick(0);
        int writes = runtime.Writes;
        runtime.Original.Alive = false;
        session.Stop(0);
        session.Tick(5);
        session.Tick(10);
        session.Tick(15);
        session.Tick(20);
        Assert.Equal(WaterState.Faulted, session.State);
        Assert.NotNull(session.LastError);
        Assert.Same(runtime.Clone, runtime.Target.Material);
        Assert.True(runtime.Clone.Alive);
        Assert.Equal(writes, runtime.Writes);
        Assert.Equal(0, runtime.Destroys);
        runtime.Original.Alive = true;
        session.Tick(25);
        Assert.Same(runtime.Clone, runtime.Target.Material);
        session.Stop(30);
        Assert.Equal(WaterState.Stopped, session.State);
        Assert.Same(runtime.Original, runtime.Target.Material);
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
        public bool ThrowRead, ThrowBeforeWrite, ThrowAfterWrite, ThrowConfigure, IgnoreWrites, ThrowTargetAlive;
        public void ConfigureMaterial(Handle material)
        {
            Configurations++;
            if (ThrowConfigure) throw new InvalidOperationException();
        }
        public int Discoveries, Clones, Configurations, Writes, Destroys, Reads;
        public FakeRuntime() { Target = new Handle { Material = Original }; }
        public Handle? Discover() { Discoveries++; return Available ? Target : null; }
        public bool TargetAlive(Handle value) => ThrowTargetAlive ? throw new InvalidOperationException() : value.Alive;
        public bool MaterialAlive(Handle value) => value.Alive;
        public Handle? ReadMaterial(Handle target) { Reads++; if (ThrowRead) throw new InvalidOperationException(); return target.Material; }
        public Handle CreateMaterial(Handle template) { Clones++; Clone = new Handle(); return Clone; }
        public void WriteMaterial(Handle target, Handle? material)
        {
            Writes++;
            if (ThrowBeforeWrite) throw new InvalidOperationException();
            if (!IgnoreWrites) target.Material = material;
            if (ThrowAfterWrite) throw new InvalidOperationException();
        }
        public bool SameMaterial(Handle? left, Handle right) => ReferenceEquals(left, right);
        public void DestroyMaterial(Handle material) { Destroys++; material.Alive = false; }
    }
}
