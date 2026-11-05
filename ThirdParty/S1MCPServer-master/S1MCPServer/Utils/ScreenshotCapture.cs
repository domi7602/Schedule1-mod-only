using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace S1MCPServer.Utils;

public sealed record CapturedFrame(byte[] Png, int Width, int Height, int Frame, DateTime CapturedUtc);
public sealed record ScreenshotResult(string Path, int Width, int Height, int Frame, DateTime CapturedUtc,
    DateTime StartedUtc, DateTime CompletedUtc, double ElapsedMs, long Bytes);

public sealed class ScreenshotCapture
{
    private readonly string _root;
    public ScreenshotCapture(string root) { _root = System.IO.Path.GetFullPath(root); }
    public static void ValidateDimensions(int width, int height)
    {
        if (width < 1 || height < 1 || width > 8192 || height > 8192 || (long)width * height > 16777216)
            throw new ArgumentOutOfRangeException(nameof(width), "Capture supports at most 8192 per edge and 16 megapixels.");
    }

    private void EnsureSafeDirectory()
    {
        if (_root.StartsWith(@"\\\\", StringComparison.Ordinal)) throw new IOException("Network capture paths are not supported.");
        for (var directory = new DirectoryInfo(_root); directory != null; directory = directory.Parent)
            if (directory.Exists && (directory.Attributes & FileAttributes.ReparsePoint) != 0)
                throw new IOException("Capture directory ancestors must not be reparse points.");
    }

    private int _busy;
    public async Task<ScreenshotResult> CaptureAsync(Func<CancellationToken, Task<CapturedFrame>> capture, int timeoutMs)
    {
        if (timeoutMs < 100 || timeoutMs > 15000) throw new ArgumentOutOfRangeException(nameof(timeoutMs));
        if (Interlocked.CompareExchange(ref _busy, 1, 0) != 0)
            throw new InvalidOperationException("A screen capture is already in progress (possibly draining after timeout).");
        var cancellation = new CancellationTokenSource();
        var operation = CaptureCoreAsync(capture, cancellation.Token);
        try
        {
            var deadline = Task.Delay(timeoutMs);
            if (await Task.WhenAny(operation, deadline).ConfigureAwait(false) != operation)
            {
                cancellation.Cancel();
                throw new TimeoutException("Screen capture timed out; late work is discarded and guard remains until cleanup.");
            }
            return await operation.ConfigureAwait(false);
        }
        finally
        {
            // Never release the guard while a native capture/file operation may still be alive.
            _ = ReleaseAfterDrainAsync(operation, cancellation);
        }
    }

    private async Task ReleaseAfterDrainAsync(Task operation, CancellationTokenSource cancellation)
    {
        try { await operation.ConfigureAwait(false); }
        catch { /* The caller already received the failure/timeout. Observe late exceptions. */ }
        finally { cancellation.Dispose(); Interlocked.Exchange(ref _busy, 0); }
    }

    private async Task<ScreenshotResult> CaptureCoreAsync(Func<CancellationToken, Task<CapturedFrame>> capture, CancellationToken token)
    {
        var started = DateTime.UtcNow;
        var watch = System.Diagnostics.Stopwatch.StartNew();
        var frame = await capture(token).ConfigureAwait(false);
        token.ThrowIfCancellationRequested();
        ValidateDimensions(frame.Width, frame.Height);
        var png = frame.Png;
        if (png == null || png.Length < 45 || png.Length > 64 * 1024 * 1024 ||
            !png.AsSpan(0, 8).SequenceEqual(new byte[] { 137, 80, 78, 71, 13, 10, 26, 10 }) ||
            System.Buffers.Binary.BinaryPrimitives.ReadInt32BigEndian(png.AsSpan(16, 4)) != frame.Width ||
            System.Buffers.Binary.BinaryPrimitives.ReadInt32BigEndian(png.AsSpan(20, 4)) != frame.Height ||
            !png.AsSpan(png.Length - 12).SequenceEqual(new byte[] { 0, 0, 0, 0, 73, 69, 78, 68, 174, 66, 96, 130 }))
            throw new InvalidDataException("Capture returned an invalid, incomplete, oversized or mismatched PNG.");
        return await Task.Run(async () =>
        {
            token.ThrowIfCancellationRequested();
            EnsureSafeDirectory();
            Directory.CreateDirectory(_root);
            EnsureSafeDirectory();
            var path = System.IO.Path.Combine(_root, $"capture-{Guid.NewGuid():N}.png");
            try
            {
                using (var stream = new FileStream(path, FileMode.CreateNew, FileAccess.Write, FileShare.None))
                    await stream.WriteAsync(frame.Png, token).ConfigureAwait(false);
                token.ThrowIfCancellationRequested();
                return new ScreenshotResult(path, frame.Width, frame.Height, frame.Frame, frame.CapturedUtc,
                    started, DateTime.UtcNow, watch.Elapsed.TotalMilliseconds, new FileInfo(path).Length);
            }
            catch { if (File.Exists(path)) File.Delete(path); throw; }
        }, token).ConfigureAwait(false);
    }
}
