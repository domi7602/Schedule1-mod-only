using System.Text.Json;
using GraphicsProbe;
using Xunit;

public class ReportPolicyTests
{
    [Fact]
    public void ByteLimitFailureLeavesObservationWindowAvailableWithoutPublishingPartialJson()
    {
        var report = new ObservedRecord
        {
            ["schemaVersion"] = 2,
            ["modVersion"] = "test",
            ["captureReason"] = "manual",
            ["captureStartedAtUtc"] = "2026-10-16T01:00:00Z",
            ["captureEndedAtUtc"] = "2026-10-16T01:00:01Z",
            ["collectionTiming"] = new { activeWorkMilliseconds = 10, wallMilliseconds = 1000, frames = 3 },
            ["rows"] = new string('x', ReportPolicy.MaxBytes)
        };
        Assert.Throws<InvalidOperationException>(() => ReportPolicy.Serialize(report));
        Assert.Equal("2026-10-16T01:00:00Z", report["captureStartedAtUtc"]);
        Assert.Equal("2026-10-16T01:00:01Z", report["captureEndedAtUtc"]);
        Assert.True(report.ContainsKey("collectionTiming"));
    }

    [Fact]
    public void OnlyMatchedRowsAreKeptAndNoGetterRunsAfterCap()
    {
        var section = new BoundedSection<string>(2);
        int calls = 0;
        section.Consider(false, () => throw new Exception());
        for (int i = 0; i < 5; i++) section.Consider(true, () => { calls++; return "row"; });
        Assert.Equal(6, section.Scanned);
        Assert.Equal(5, section.Matched);
        Assert.Equal(2, section.Kept);
        Assert.Equal(3, section.LimitOmissions);
        Assert.True(section.Truncated);
        Assert.Equal(2, calls);
    }

    [Fact]
    public void BadObjectIsIsolatedAndDoesNotConsumeSuccessfulRowBudget()
    {
        var section = new BoundedSection<string>(1);
        section.Consider(true, () => throw new InvalidOperationException("stale"));
        section.Consider(true, () => "next");
        Assert.Equal(2, section.Scanned);
        Assert.Equal(1, section.Failed);
        Assert.Equal("next", Assert.Single(section.Items));
        Assert.False(section.Truncated);
    }

    [Fact]
    public void NonFiniteNumbersSerializeAsNullWithoutPermittingInvalidJson()
    {
        string json = ReportPolicy.Serialize(new { Values = new[] { double.NaN, double.PositiveInfinity, double.NegativeInfinity, 1.25 }, Float = float.NaN });
        using var parsed = JsonDocument.Parse(json);
        Assert.Equal("null", parsed.RootElement.GetProperty("values")[0].GetRawText());
        Assert.Equal(1.25, parsed.RootElement.GetProperty("values")[3].GetDouble());
        Assert.Equal(JsonValueKind.Null, parsed.RootElement.GetProperty("float").ValueKind);
    }

    [Fact]
    public void SerializationHasAHardUtf8ByteLimitAndStringsHaveExplicitCutMarker()
    {
        Assert.Equal("abc…", ReportPolicy.Text("abcdef", 3));
        Assert.Equal("", ReportPolicy.Text(null, 3));
        Assert.Throws<InvalidOperationException>(() => ReportPolicy.Serialize(new { A = new string('界', 100) }, 64));
        Assert.NotEmpty(ReportPolicy.Serialize(new { A = "ok" }, 64));
    }

    [Fact]
    public void TextCapDoesNotSplitUnicodeSurrogatePairs()
    {
        Assert.Equal("a…", ReportPolicy.Text("a😀b", 2));
    }

    [Fact]
    public void ZeroCapacityStillReportsMatchesAndTruncation()
    {
        var section = new BoundedSection<string>(0);
        section.Consider(true, () => throw new Exception());
        Assert.Equal(1, section.LimitOmissions);
        Assert.Equal(0, section.Failed);
        Assert.True(section.Truncated);
    }
}
