using WaterShader;
using Xunit;

namespace WaterShader.Tests;

public sealed class WaterShaderAssignmentTests
{
    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void MissingOrDeadCandidateCannotWriteAnEmptyShader(bool missing)
    {
        var material = new FakeMaterial();
        Assert.Throws<InvalidOperationException>(() => material.Assign(missing ? null! : new Shader { Alive = false }));
        Assert.Equal(0, material.Writes);
    }

    [Fact]
    public void SilentSetterFailureCannotReportASuccessfulFallback()
    {
        var material = new FakeMaterial { IgnoreWrite = true };
        Assert.Throws<InvalidOperationException>(() => material.Assign(new Shader()));
    }

    [Fact]
    public void MutationThenExceptionRemainsAFailureUntilAnExplicitConfirmedRestore()
    {
        var material = new FakeMaterial { ThrowAfterWrite = true };
        var original = material.Current;
        var reference = new Shader();
        Assert.Throws<InvalidOperationException>(() => material.Assign(reference));
        Assert.Same(reference, material.Current);
        material.ThrowAfterWrite = false;
        material.Assign(original);
        Assert.Same(original, material.Current);
    }

    [Fact]
    public void FailedReadbackCannotConfirmAShaderTransition()
    {
        var material = new FakeMaterial { ThrowRead = true };
        Assert.Throws<InvalidOperationException>(() => material.Assign(new Shader()));
    }

    private sealed class Shader { internal bool Alive = true; }
    private sealed class FakeMaterial
    {
        internal Shader Current = new();
        internal bool IgnoreWrite, ThrowAfterWrite, ThrowRead;
        internal int Writes;
        internal void Assign(Shader value)
            => WaterShaderAssignment.AssignConfirmed(value, shader => shader.Alive,
                shader =>
                {
                    Writes++;
                    if (!IgnoreWrite) Current = shader;
                    if (ThrowAfterWrite) throw new InvalidOperationException();
                },
                () => ThrowRead ? throw new InvalidOperationException() : Current,
                ReferenceEquals);
    }
}
