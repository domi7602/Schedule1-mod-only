using System.Collections;
using S1MCPServer.Handlers;
using S1MCPServer.Models;
using S1MCPServer.Core;
using Xunit;

public class ScreenshotHandlerTests
{
    [Fact]
    public async Task CapturesOnlyAtEndOfFrameAndDestroysOwnedTextureBeforeReply()
    {
        UnityEngine.ScreenCapture.Calls = 0;
        UnityEngine.Object.Destroyed = false;
        var queue = new ResponseQueue();
        var handler = new ScreenshotCommandHandler(queue);
        handler.Handle(new Request { Id = 45, Params = new() });
        Assert.Empty(queue.Results);
        var routine = MelonLoader.MelonCoroutines.Routine!;
        Assert.True(routine.MoveNext());
        Assert.IsType<UnityEngine.WaitForEndOfFrame>(routine.Current);
        Assert.Equal(0, UnityEngine.ScreenCapture.Calls);
        Assert.False(routine.MoveNext());
        Assert.True(UnityEngine.Object.Destroyed);
        for (int i = 0; i < 100 && queue.Results.IsEmpty; i++) await Task.Delay(10);
        Assert.True(queue.Results.TryDequeue(out var response));
        Assert.Equal(45, response!.Id);
        Assert.Equal(0, response.Error);
        Assert.Equal(1, UnityEngine.ScreenCapture.Calls);
    }

    [Fact]
    public async Task ShutdownDrainsPendingFrameWithoutCapture()
    {
        var queue = new ResponseQueue();
        var handler = new ScreenshotCommandHandler(queue);
        handler.Handle(new Request { Id = 47, Params = new() });
        var disposable = Assert.IsAssignableFrom<IDisposable>(handler);
        disposable.Dispose();
        for (int i = 0; i < 100 && queue.Results.IsEmpty; i++) await Task.Delay(10);
        Assert.True(queue.Results.TryDequeue(out var response));
        Assert.NotEqual(0, response!.Error);
        handler.Handle(new Request { Id = 48 });
        Assert.True(queue.Results.TryDequeue(out response));
        Assert.NotEqual(0, response!.Error);
    }

    [Fact]
    public void RejectsCallerPathsBeforeStartingCapture()
    {
        var queue = new ResponseQueue();
        new ScreenshotCommandHandler(queue).Handle(new Request { Id = 46, Params = new() { ["path"] = "../escape.png" } });
        Assert.True(queue.Results.TryDequeue(out var response));
        Assert.Equal(-32602, response!.Error);
    }
}

namespace S1MCPServer.Core
{
    public interface ICommandHandler { void Handle(Request request); }
    public record TestResponse(int Id, int Error, object? Result);
    public class ResponseQueue
    {
        public System.Collections.Concurrent.ConcurrentQueue<TestResponse> Results = new();
        public void EnqueueResponse(TestResponse result) => Results.Enqueue(result);
    }
    public static class ProtocolHandler
    {
        public static TestResponse CreateSuccessResponse(int id, object result) => new(id, 0, result);
        public static TestResponse CreateErrorResponse(int id, int code, string message, object? data = null) => new(id, code, message);
    }
}
namespace MelonLoader
{
    public static class MelonCoroutines
    {
        public static IEnumerator? Routine;
        public static object Start(IEnumerator routine) { Routine = routine; return routine; }
        public static void Stop(object routine) { }
    }
}
namespace MelonLoader.Utils
{
    public static class MelonEnvironment
    {
        public static string UserDataDirectory = Path.Combine(Path.GetTempPath(), "s1capture-handler-" + Guid.NewGuid().ToString("N"));
    }
}
namespace UnityEngine
{
    public class WaitForEndOfFrame { }
    public class Object { public static bool Destroyed; public static void Destroy(Object obj) => Destroyed = true; }
    public class Texture2D : Object { public int width => 1; public int height => 1; }
    public static class Screen { public static int width => 1; public static int height => 1; }
    public static class Time { public static int frameCount => 123; }
    public static class ScreenCapture { public static int Calls; public static Texture2D CaptureScreenshotAsTexture(int size) { Calls++; return new(); } }
    public static class ImageConversion
    {
        public static byte[] EncodeToPNG(Texture2D texture) => Convert.FromBase64String("iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAQAAAC1HAwCAAAAC0lEQVR42mP8/x8AAwMCAO+jRZkAAAAASUVORK5CYII=");
    }
}
