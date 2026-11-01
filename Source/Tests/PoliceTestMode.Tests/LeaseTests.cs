using Xunit;
namespace PoliceTestMode.Tests;
public class LeaseTests
{
    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void Lease_restores_first_value_after_reapplication(bool original)
    {
        bool value = original;
        var type = typeof(LeaseTests).Assembly.GetType("PoliceTestMode.BoolLease");
        Assert.NotNull(type);
        dynamic lease = Activator.CreateInstance(type, new object[] { (Func<bool>)(() => true), (Func<bool>)(() => value), (Action<bool>)(v => value = v), false })!;
        lease.Apply();
        Assert.False(value);
        value = true;
        lease.Apply();
        Assert.False(value);
        Assert.True((bool)lease.Restore());
        Assert.Equal(original, value);
    }
}
