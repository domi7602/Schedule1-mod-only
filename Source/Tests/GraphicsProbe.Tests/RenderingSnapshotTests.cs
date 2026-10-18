using System.Text.Json;
using GraphicsProbe;
using GraphicsProbe.Tests.Adapters;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using UnityEngine;
using UnityEngine.SceneManagement;
using Xunit;

[CollectionDefinition("Managed RenderingSnapshot", DisableParallelization = true)]
public sealed class ManagedRenderingSnapshotCollection { }

// Executes the linked production collector/controller/report code with managed adapters.
// This is fault-injection orchestration coverage, NOT Unity or native-runtime evidence.
[Collection("Managed RenderingSnapshot")]
public sealed class RenderingSnapshotTests : IDisposable
{
    public RenderingSnapshotTests() => ManagedNativeReads.Reset();
    public void Dispose() => ManagedNativeReads.Reset();

    [Fact]
    public void ComponentLengthFailureDoesNotPublishFailedBatchOrReusePreviousLimit()
    {
        var first = new Il2CppArrayBase<Component>(new Component(), new Component(), new Component());
        var unreadable = new Il2CppArrayBase<Component>
        {
            LengthGetter = () => throw new InvalidOperationException("component length unavailable"),
            ItemGetter = _ => throw new InvalidOperationException("failed batch must not be indexed")
        };
        var camera = new Camera { InstanceId = 40 };
        var last = new Il2CppArrayBase<Component>(camera);
        var scene = new Scene();
        scene.Roots = new Il2CppReferenceArray<GameObject>(
            new GameObject(1) { scene = scene, ComponentsGetter = () => first },
            new GameObject(2) { scene = scene, ComponentsGetter = () => unreadable },
            new GameObject(3) { scene = scene, ComponentsGetter = () => last });
        SceneManager.Scenes = new[] { scene };
        var snapshot = new RenderingSnapshot("managed component failure", "test");

        using var report = Complete(snapshot);

        Assert.Equal(3, first.IndexReads);
        Assert.Equal(1, unreadable.LengthReads);
        Assert.Equal(0, unreadable.IndexReads);
        Assert.Equal(1, last.IndexReads);
        Assert.Equal(3, snapshot.NodesVisited);
        Assert.Equal(40, Assert.Single(snapshot.Cameras.Items)["instanceId"]);
        var discovery = report.RootElement.GetProperty("discovery");
        Assert.Equal(1, discovery.GetProperty("errors").GetInt32());
        Assert.True(discovery.GetProperty("discoveryTruncated").GetBoolean());
        Assert.Equal(0, discovery.GetProperty("pendingComponentsAtRelease").GetInt32());
        var error = Assert.Single(discovery.GetProperty("errorSamples").EnumerateArray());
        Assert.Contains("component length unavailable", error.GetString());
    }

    [Fact]
    public void SceneCountFailureReportsUnknownUnavailableTotalInsteadOfSuccessfulEmptyScenes()
    {
        SceneManager.CountGetter = () => throw new InvalidOperationException("scene count unavailable");
        var snapshot = new RenderingSnapshot("managed scene count failure", "test");

        using var report = Complete(snapshot);

        var root = report.RootElement;
        Assert.Equal(JsonValueKind.Null, root.GetProperty("sceneCount").ValueKind);
        Assert.Equal("InvalidOperationException: scene count unavailable",
            root.GetProperty("fieldErrors").GetProperty("sceneCount").GetString());
        var scenes = root.GetProperty("scenes");
        Assert.Equal(JsonValueKind.Null, scenes.GetProperty("totalObserved").ValueKind);
        Assert.False(scenes.GetProperty("available").GetBoolean());
        Assert.True(scenes.GetProperty("truncated").GetBoolean());
        Assert.Equal(0, scenes.GetProperty("scanned").GetInt32());
        Assert.Equal(0, scenes.GetProperty("kept").GetInt32());
        Assert.Empty(scenes.GetProperty("items").EnumerateArray());
        var discovery = root.GetProperty("discovery");
        Assert.True(discovery.GetProperty("discoveryTruncated").GetBoolean());
        Assert.Equal(1, discovery.GetProperty("errors").GetInt32());
        Assert.Contains("scenes: InvalidOperationException: scene count unavailable",
            Assert.Single(discovery.GetProperty("errorSamples").EnumerateArray()).GetString());
    }

    [Fact]
    public void SuccessfulZeroSceneCountRemainsAvailableEmptyAndNotTruncated()
    {
        var snapshot = new RenderingSnapshot("managed empty scene set", "test");

        using var report = Complete(snapshot);

        var root = report.RootElement;
        Assert.Equal(0, root.GetProperty("sceneCount").GetInt32());
        Assert.False(root.TryGetProperty("fieldErrors", out _));
        var scenes = root.GetProperty("scenes");
        Assert.Equal(0, scenes.GetProperty("totalObserved").GetInt32());
        Assert.True(scenes.GetProperty("available").GetBoolean());
        Assert.False(scenes.GetProperty("truncated").GetBoolean());
        Assert.Equal(0, scenes.GetProperty("scanned").GetInt32());
        Assert.Equal(0, scenes.GetProperty("kept").GetInt32());
        Assert.Empty(scenes.GetProperty("items").EnumerateArray());
        Assert.False(root.GetProperty("discovery").GetProperty("discoveryTruncated").GetBoolean());
        Assert.Equal(0, root.GetProperty("discovery").GetProperty("errors").GetInt32());
    }

    private static JsonDocument Complete(RenderingSnapshot snapshot)
    {
        using var capture = new CooperativeCapture(snapshot, () => 0);
        for (int frame = 0; frame < 100 && capture.Status == "running"; frame++) capture.StepFrame();
        Assert.Equal("completed", capture.Status);
        Assert.Null(capture.Failure);
        int readsBeforeReport = ManagedNativeReads.Count;
        var report = snapshot.FinishReport(capture);
        var json = JsonDocument.Parse(ReportPolicy.Serialize(report));
        Assert.Equal(readsBeforeReport, ManagedNativeReads.Count);
        Assert.False(snapshot.Advance());
        return json;
    }
}
