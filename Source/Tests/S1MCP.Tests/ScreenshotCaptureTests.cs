using S1MCPServer.Utils;
using Xunit;

public class ScreenshotCaptureTests
{
    [Fact]
    public async Task RejectsReparseEvidenceDirectory()
    {
        var parent = Path.Combine(Path.GetTempPath(), "s1capture-link-" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(parent);
        var target = Directory.CreateDirectory(Path.Combine(parent, "target")).FullName;
        var link = Path.Combine(parent, "link");
        if (OperatingSystem.IsWindows())
        {
            using var process = System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo("cmd.exe", $"/c mklink /J \"{link}\" \"{target}\"") { CreateNoWindow = true, UseShellExecute = false });
            process!.WaitForExit();
            Assert.Equal(0, process.ExitCode);
        }
        else Directory.CreateSymbolicLink(link, target);
        try
        {
            var service = new ScreenshotCapture(link);
            var png = Convert.FromBase64String("iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAQAAAC1HAwCAAAAC0lEQVR42mP8/x8AAwMCAO+jRZkAAAAASUVORK5CYII=");
            await Assert.ThrowsAsync<IOException>(() => service.CaptureAsync(_ => Task.FromResult(new CapturedFrame(png, 1, 1, 1, DateTime.UtcNow)), 5000));
            Assert.Empty(Directory.GetFiles(target));
        }
        finally { Directory.Delete(link); Directory.Delete(parent, true); }
    }

    [Theory]
    [InlineData(0, 1, 5000)]
    [InlineData(8193, 1, 5000)]
    [InlineData(8192, 8192, 5000)]
    [InlineData(1, 1, 0)]
    [InlineData(1, 1, 15001)]
    public async Task RejectsUnboundedCapture(int width, int height, int timeout)
    {
        var service = new ScreenshotCapture(Path.Combine(Path.GetTempPath(), "s1capture-" + Guid.NewGuid().ToString("N")));
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => service.CaptureAsync(
            _ => Task.FromResult(new CapturedFrame(new byte[0], width, height, 0, DateTime.UtcNow)), timeout));
    }

    [Fact]
    public async Task RejectsInvalidPngWithoutLeavingAFile()
    {
        var root = Path.Combine(Path.GetTempPath(), "s1capture-" + Guid.NewGuid().ToString("N"));
        try
        {
            var service = new ScreenshotCapture(root);
            await Assert.ThrowsAsync<InvalidDataException>(() => service.CaptureAsync(
                _ => Task.FromResult(new CapturedFrame(new byte[25], 1, 1, 0, DateTime.UtcNow)), 100));
            Assert.False(Directory.Exists(root));
        }
        finally { if (Directory.Exists(root)) Directory.Delete(root, true); }
    }

    [Fact]
    public async Task TimeoutKeepsGuardUntilLateFrameDrainsWithoutPublishing()
    {
        var root = Path.Combine(Path.GetTempPath(), "s1capture-" + Guid.NewGuid().ToString("N"));
        var frame = new TaskCompletionSource<CapturedFrame>(TaskCreationOptions.RunContinuationsAsynchronously);
        var service = new ScreenshotCapture(root);
        var pending = service.CaptureAsync(_ => frame.Task, 100);
        await Assert.ThrowsAsync<InvalidOperationException>(() => service.CaptureAsync(_ => frame.Task, 100).WaitAsync(TimeSpan.FromSeconds(1)));
        await Assert.ThrowsAsync<TimeoutException>(() => pending.WaitAsync(TimeSpan.FromSeconds(2)));
        await Assert.ThrowsAsync<InvalidOperationException>(() => service.CaptureAsync(_ => frame.Task, 100).WaitAsync(TimeSpan.FromSeconds(1)));
        frame.SetException(new IOException("late frame failure"));
        await Task.Delay(100);
        Assert.False(Directory.Exists(root));
        await Assert.ThrowsAsync<IOException>(() => service.CaptureAsync(_ => Task.FromException<CapturedFrame>(new IOException("new request")), 100));
    }

    [Fact]
    public async Task PublishesUniquePngOnlyAfterFrameCompletes()
    {
        var root = Path.Combine(Path.GetTempPath(), "s1capture-" + Guid.NewGuid().ToString("N"));
        var frame = new TaskCompletionSource<CapturedFrame>();
        try
        {
            var service = new ScreenshotCapture(root);
            var pending = service.CaptureAsync(_ => frame.Task, 5000);
            Assert.False(pending.IsCompleted);
            Assert.False(Directory.Exists(root));
            var png = Convert.FromBase64String("iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAQAAAC1HAwCAAAAC0lEQVR42mP8/x8AAwMCAO+jRZkAAAAASUVORK5CYII=");
            frame.SetResult(new CapturedFrame(png, 1, 1, 42, DateTime.UtcNow));
            var result = await pending;
            Assert.True(Path.IsPathFullyQualified(result.Path));
            Assert.Equal(png, File.ReadAllBytes(result.Path));
            Assert.Equal(1, result.Width);
            Assert.Equal(42, result.Frame);
            var second = await service.CaptureAsync(_ => frame.Task, 5000);
            Assert.NotEqual(result.Path, second.Path);
        }
        finally { if (Directory.Exists(root)) Directory.Delete(root, true); }
    }
}
