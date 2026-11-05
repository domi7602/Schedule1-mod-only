using Xunit;
namespace PoliceTestMode.Tests;

public class LeaseFailureTests
{
    [Fact]
    public void Failed_write_after_mutation_keeps_original_for_restore()
    {
        bool value = true, fail = true;
        var lease = new BoolLease(() => true, () => value, v => { value = v; if (fail) throw new InvalidOperationException(); }, false);
        Assert.Throws<InvalidOperationException>(() => lease.Apply());
        Assert.False(value);
        fail = false;
        Assert.True(lease.Restore());
        Assert.True(value);
    }

    [Fact]
    public void Restore_mismatch_keeps_snapshot_for_next_attempt()
    {
        bool value = true, ignoreWrite = false;
        var lease = new BoolLease(() => true, () => value, v => { if (!ignoreWrite) value = v; }, false);
        lease.Apply();
        ignoreWrite = true;
        Assert.False(lease.Restore());
        ignoreWrite = false;
        Assert.True(lease.Restore());
        Assert.True(value);
    }

    [Fact]
    public void Destroyed_object_is_not_written_on_restore()
    {
        bool alive = true, value = true;
        int writes = 0;
        var lease = new BoolLease(() => alive, () => value, v => { value = v; writes++; }, false);
        lease.Apply();
        alive = false;
        Assert.True(lease.Restore());
        Assert.Equal(1, writes);
    }

    [Fact]
    public void Reapplication_does_not_write_when_already_suppressed()
    {
        bool value = true;
        int writes = 0;
        var lease = new BoolLease(() => true, () => value, v => { value = v; writes++; }, false);
        lease.Apply(); lease.Apply(); lease.Apply();
        Assert.Equal(1, writes);
    }
}
