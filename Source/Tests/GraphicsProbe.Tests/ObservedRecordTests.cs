using GraphicsProbe;
using Xunit;

public class ObservedRecordTests
{
    [Fact]
    public void ThrowingTopLevelProviderPreservesEvidenceBeforeNotifyingOnce()
    {
        var record = new ObservedRecord();
        var settings = new ObservedRecord { ["quality"] = "observed" };
        var environment = new ObservedRecord { ["focused"] = true };
        record.Read("settings", () => settings);
        var failure = new InvalidOperationException("scene count unavailable");
        var notifications = new List<Exception>();

        record.Read("scenes", () => throw failure, ex =>
        {
            Assert.Null(record["scenes"]);
            Assert.Same(record.Errors, record["fieldErrors"]);
            Assert.Equal("InvalidOperationException: scene count unavailable", record.Errors["scenes"]);
            notifications.Add(ex);
        });
        record.Read("environment", () => environment);

        Assert.Same(failure, Assert.Single(notifications));
        Assert.Null(record["scenes"]);
        Assert.Single(record.Errors);
        Assert.Same(settings, record["settings"]);
        Assert.Same(environment, record["environment"]);
        using var report = System.Text.Json.JsonDocument.Parse(ReportPolicy.Serialize(record));
        Assert.Equal(System.Text.Json.JsonValueKind.Null, report.RootElement.GetProperty("scenes").ValueKind);
        Assert.Equal("InvalidOperationException: scene count unavailable",
            report.RootElement.GetProperty("fieldErrors").GetProperty("scenes").GetString());
    }

    [Fact]
    public void SuccessfulProviderDoesNotNotifyErrorCallback()
    {
        var record = new ObservedRecord();
        var scenes = new ObservedRecord { ["totalObserved"] = 0 };
        int notifications = 0;

        record.Read("scenes", () => scenes, _ => notifications++);

        Assert.Same(scenes, record["scenes"]);
        Assert.Equal(0, notifications);
        Assert.Empty(record.Errors);
        Assert.False(record.ContainsKey("fieldErrors"));
    }

    [Fact]
    public void ThrowingNativeLikeGetterBecomesFieldErrorAndLaterFieldsRemainReadable()
    {
        var record = new ObservedRecord();
        record.Read("bad", () => throw new InvalidOperationException("stale"));
        record.Read("good", () => 42);
        Assert.Null(record["bad"]);
        Assert.Equal(42, record["good"]);
        Assert.Contains("InvalidOperationException", record.Errors["bad"]);
        Assert.Equal("stale", record.Errors["bad"].Split(": ")[1]);
    }
}
