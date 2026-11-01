#nullable enable
using System.Collections;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using MelonLoader;
using MelonLoader.Utils;
using S1MCPServer.Core;
using S1MCPServer.Models;
using S1MCPServer.Utils;
using UnityEngine;

namespace S1MCPServer.Handlers;

// NOT IThreadSafeCommandHandler: the coroutine must start on the game thread.
public sealed class ScreenshotCommandHandler : ICommandHandler, IDisposable
{
    private readonly ResponseQueue _queue;
    private readonly ScreenshotCapture _capture;
    private object? _routine;
    private TaskCompletionSource<CapturedFrame>? _pending;
    private bool _disposed;
    public ScreenshotCommandHandler(ResponseQueue queue)
    {
        _queue = queue;
        _capture = new ScreenshotCapture(Path.Combine(MelonEnvironment.UserDataDirectory, "S1MCP", "Evidence", "Screenshots"));
    }

    public void Handle(Request request)
    {
        if (_disposed)
        {
            _queue.EnqueueResponse(ProtocolHandler.CreateErrorResponse(request.Id, -32000, "Capture handler is shutting down."));
            return;
        }
        int timeout = 5000;
        foreach (var pair in request.Params ?? new Dictionary<string, object>())
        {
            if (pair.Key != "timeout_ms" ||
                !(pair.Value is int || pair.Value is JsonElement { ValueKind: JsonValueKind.Number }) ||
                !int.TryParse(pair.Value.ToString(), out timeout) || timeout < 100 || timeout > 15000)
            {
                _queue.EnqueueResponse(ProtocolHandler.CreateErrorResponse(request.Id, -32602,
                    "Only timeout_ms (integer 100..15000, default 5000) is accepted. Paths, scaling and UI overrides are not accepted."));
                return;
            }
        }
        _ = CompleteAsync(request.Id, timeout);
    }

    private async Task CompleteAsync(int id, int timeout)
    {
        try
        {
            var result = await _capture.CaptureAsync(StartFrameCapture, timeout).ConfigureAwait(false);
            _queue.EnqueueResponse(ProtocolHandler.CreateSuccessResponse(id, new
            {
                schema_version = 1, success = true, path = result.Path,
                width = result.Width, height = result.Height, frame = result.Frame,
                captured_utc = result.CapturedUtc, started_utc = result.StartedUtc,
                completed_utc = result.CompletedUtc, elapsed_ms = result.ElapsedMs, bytes = result.Bytes,
                includes_ui = true, capture_source = "Unity.ScreenCapture/end-of-frame"
            }));
        }
        catch (Exception ex)
        {
            int code = ex is TimeoutException ? -32002 : ex is InvalidOperationException ? -32001 : -32000;
            _queue.EnqueueResponse(ProtocolHandler.CreateErrorResponse(id, code, ex.Message));
        }
    }

    private Task<CapturedFrame> StartFrameCapture(CancellationToken token)
    {
        var completion = new TaskCompletionSource<CapturedFrame>(TaskCreationOptions.RunContinuationsAsynchronously);
        _pending = completion;
        _routine = MelonCoroutines.Start(CaptureFrame(completion, token));
        return completion.Task;
    }

    public void Dispose()
    {
        _disposed = true;
        if (_pending != null && !_pending.Task.IsCompleted)
        {
            if (_routine != null) MelonCoroutines.Stop(_routine);
            _pending.TrySetException(new OperationCanceledException("Game is shutting down."));
        }
        _pending = null;
        _routine = null;
    }

    private static IEnumerator CaptureFrame(TaskCompletionSource<CapturedFrame> completion, CancellationToken token)
    {
        yield return new WaitForEndOfFrame();
        Texture2D? texture = null;
        CapturedFrame? frame = null;
        Exception? failure = null;
        try
        {
            token.ThrowIfCancellationRequested();
            ScreenshotCapture.ValidateDimensions(Screen.width, Screen.height);
            var capturedUtc = DateTime.UtcNow;
            var frameNumber = Time.frameCount;
            texture = ScreenCapture.CaptureScreenshotAsTexture(1);
            if (texture == null) throw new IOException("Unity returned no final-frame texture.");
            ScreenshotCapture.ValidateDimensions(texture.width, texture.height);
            token.ThrowIfCancellationRequested();
            var encoded = ImageConversion.EncodeToPNG(texture);
            if (encoded == null || encoded.Length > 64 * 1024 * 1024) throw new IOException("PNG encoding failed or exceeded 64 MiB.");
            // Installed IL2CPP EncodeToPNG returns Il2CppStructArray<byte>; copy to managed ownership.
            frame = new CapturedFrame(encoded.ToArray(), texture.width, texture.height, frameNumber, capturedUtc);
        }
        catch (Exception ex) { failure = ex; }
        finally
        {
            try { if (texture != null) UnityEngine.Object.Destroy(texture); }
            catch (Exception ex) { failure = ex; }
        }
        if (failure != null) completion.TrySetException(failure);
        else completion.TrySetResult(frame!);
    }
}
