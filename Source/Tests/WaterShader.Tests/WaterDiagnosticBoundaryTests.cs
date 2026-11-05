using System;
using System.Reflection;
using WaterShader;
using Xunit;

namespace WaterShader.Tests;

public sealed class WaterDiagnosticBoundaryTests
{
    [Fact]
    public void AppliedOwnedMaterialCanChangeModeAndRepeatIsIdempotent()
    {
        var runtime = new Runtime();
        var session = new WaterSession<Handle, Handle>(runtime);
        session.Start(0); session.Tick(0);
        var method = typeof(WaterSession<Handle, Handle>).GetMethod("SetDiagnosticMode", BindingFlags.Instance | BindingFlags.NonPublic);
        Assert.True(method != null, "Production session must expose the diagnostic transaction");
        bool Invoke(int mode) => (bool)method!.Invoke(session, new object[] { mode, (Func<Handle, bool>)(_ => true), (Func<Handle, bool>)(_ => true), (Func<Handle, float>)(m => m.Mode), (Action<Handle, float>)((m, v) => { m.Mode = v; runtime.Writes++; }), 1d })!;
        Assert.True(Invoke(1));
        Assert.False(Invoke(1));
        Assert.Equal(1, runtime.Writes);
        Assert.Equal(1f, runtime.Clone.Mode);
    }

    [Fact]
    public void ForeignReplacementAndShaderMismatchNeverWrite()
    {
        var runtime = new Runtime(); var session = Started(runtime);
        runtime.Target.Material = new Handle();
        Assert.Throws<InvalidOperationException>(() => Set(session, runtime));
        Assert.Equal(0, runtime.Writes);
        runtime.Target.Material = runtime.Clone;
        runtime.ShaderMatches = false;
        Assert.Throws<InvalidOperationException>(() => Set(session, runtime));
        Assert.Equal(0, runtime.Writes);
    }

    [Fact]
    public void MissingPropertyAndPrewriteReadFailureNeverWrite()
    {
        var runtime = new Runtime(); var session = Started(runtime);
        runtime.HasProperty = false;
        Assert.Throws<InvalidOperationException>(() => Set(session, runtime));
        runtime.HasProperty = true; runtime.ThrowRead = true;
        Assert.Throws<InvalidOperationException>(() => Set(session, runtime));
        Assert.Equal(0, runtime.Writes);
        Assert.Equal(WaterState.Applied, session.State);
    }

    [Theory]
    [InlineData("ignore")]
    [InlineData("after-write")]
    [InlineData("readback")]
    [InlineData("replace")]
    public void UncertainWriteFaultsAndCleanupNeverTouchesForeign(string failure)
    {
        var runtime = new Runtime { Failure = failure }; var session = Started(runtime);
        Assert.Throws<InvalidOperationException>(() => Set(session, runtime));
        Assert.Equal(WaterState.Faulted, session.State);
        Assert.Equal(1, runtime.Writes);
        Assert.Throws<InvalidOperationException>(() => Set(session, runtime));
        Assert.Equal(1, runtime.Writes);
        var foreign = runtime.Target.Material;
        session.Stop(2);
        Assert.Equal(WaterState.Stopped, session.State);
        Assert.Same(failure == "replace" ? foreign : runtime.Original, runtime.Target.Material);
        Assert.Equal(1, runtime.Destroys);
    }

    [Fact]
    public void StopAndRestartDisableAccessUntilFreshCloneIsApplied()
    {
        var runtime = new Runtime(); var session = Started(runtime);
        Assert.True(Set(session, runtime));
        session.Stop(2);
        Assert.Throws<InvalidOperationException>(() => Set(session, runtime));
        runtime.Clone = new Handle();
        session.Start(3); session.Tick(3);
        Assert.Equal(0f, runtime.Clone.Mode);
        Assert.True(Set(session, runtime));
    }

    private static WaterSession<Handle, Handle> Started(Runtime runtime)
    {
        var session = new WaterSession<Handle, Handle>(runtime);
        session.Start(0); session.Tick(0); return session;
    }
    private static bool Set(WaterSession<Handle, Handle> session, Runtime runtime)
        => session.SetDiagnosticMode(1, _ => runtime.ShaderMatches, _ => runtime.HasProperty,
            m => { if (runtime.ThrowRead || (runtime.Failure == "readback" && runtime.Writes != 0)) throw new InvalidOperationException("read failed"); return m.Mode; },
            (m, v) =>
            {
                runtime.Writes++; if (runtime.Failure != "ignore") m.Mode = v;
                if (runtime.Failure == "replace") runtime.Target.Material = new Handle();
                if (runtime.Failure == "after-write") throw new InvalidOperationException("unknown write");
            }, 1);

    private sealed class Handle { public Handle? Material; public float Mode; }
    private sealed class Runtime : IWaterRuntime<Handle, Handle>
    {
        public Handle Original = new();
        public Handle Clone = new();
        public Handle Target;
        public int Writes;
        public int Destroys;
        public bool ShaderMatches = true;
        public bool HasProperty = true;
        public bool ThrowRead;
        public string Failure = "";
        public Runtime() { Target = new Handle { Material = Original }; }
        public Handle Discover() => Target;
        public bool TargetAlive(Handle value) => true;
        public bool MaterialAlive(Handle value) => true;
        public Handle? ReadMaterial(Handle value) => value.Material;
        public Handle CreateMaterial(Handle value) => Clone;
        public void ConfigureMaterial(Handle value) { }
        public void WriteMaterial(Handle target, Handle? material) => target.Material = material;
        public bool SameMaterial(Handle? left, Handle right) => ReferenceEquals(left, right);
        public void DestroyMaterial(Handle material) { Destroys++; }
    }
}
