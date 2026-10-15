using Xunit;

namespace AutoPackagingStation.Tests;

public sealed class RegistrationPolicyTests
{
    [Theory]
    [InlineData(true, true, true, true)]
    [InlineData(false, true, true, false)]
    [InlineData(true, false, true, false)]
    [InlineData(true, true, false, false)]
    [InlineData(false, false, true, false)]
    [InlineData(false, true, false, false)]
    [InlineData(true, false, false, false)]
    [InlineData(false, false, false, false)]
    public void ExistingIdNeedsExactPreparedOwnership(bool sameDefinition, bool samePrefab, bool ghostPrepared, bool expected)
    {
        Assert.Equal(expected, RegistrationPolicy.CanReuse(sameDefinition, samePrefab, ghostPrepared));
    }
}
