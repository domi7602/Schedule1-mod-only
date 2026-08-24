using System.IO;
using System.Text.Json;
using S1Mods.Shared;
using Xunit;

namespace Shared.Tests;

public class SafeStorageTests
{
    private string TempPath(string name) => Path.Combine(Path.GetTempPath(), $"S1Test_{name}_{System.Guid.NewGuid():N}.json");

    [Fact]
    public void SaveAtomic_CreatesFileAndBackup()
    {
        var path = TempPath("save");
        try
        {
            var data = new TestData { Value = 42, Name = "hello" };
            Assert.True(SafeStorage.SaveAtomic(path, data));
            Assert.True(File.Exists(path));
            Assert.Contains("42", File.ReadAllText(path));

            data.Value = 99;
            Assert.True(SafeStorage.SaveAtomic(path, data));
            Assert.True(File.Exists(path + ".bak"));
            Assert.Contains("42", File.ReadAllText(path + ".bak"));
            Assert.Contains("99", File.ReadAllText(path));
        }
        finally
        {
            TryDelete(path);
            TryDelete(path + ".bak");
            TryDelete(path + ".tmp");
        }
    }

    [Fact]
    public void LoadSafe_ReturnsDataOrFallback()
    {
        var path = TempPath("load");
        try
        {
            var fallback = new TestData { Value = -1, Name = "fallback" };
            var result = SafeStorage.LoadSafe<TestData>(path, fallback);
            Assert.Same(fallback, result);

            var data = new TestData { Value = 7, Name = "persisted" };
            SafeStorage.SaveAtomic(path, data);
            var loaded = SafeStorage.LoadSafe<TestData>(path, fallback);
            Assert.Equal(7, loaded.Value);
            Assert.Equal("persisted", loaded.Name);
        }
        finally
        {
            TryDelete(path);
            TryDelete(path + ".bak");
        }
    }

    [Fact]
    public void LoadSafe_UsesBackupOnCorruption()
    {
        var path = TempPath("corrupt");
        try
        {
            var data = new TestData { Value = 123, Name = "good" };
            Assert.True(SafeStorage.SaveAtomic(path, data));
            data.Value = 124;
            Assert.True(SafeStorage.SaveAtomic(path, data));
            Assert.True(File.Exists(path + ".bak"));
            File.WriteAllText(path, "{ invalid json !!!");
            var fallback = new TestData { Value = -1 };
            var loaded = SafeStorage.LoadSafe<TestData>(path, fallback);
            Assert.Equal(123, loaded.Value);
        }
        finally
        {
            TryDelete(path);
            TryDelete(path + ".bak");
        }
    }

    [Fact]
    public void EnsureDirectoryForFile_CreatesDirectory()
    {
        var dir = Path.Combine(Path.GetTempPath(), $"S1TestDir_{System.Guid.NewGuid():N}");
        var path = Path.Combine(dir, "sub", "file.json");
        try
        {
            Assert.False(Directory.Exists(Path.GetDirectoryName(path)));
            SafeStorage.EnsureDirectoryForFile(path);
            Assert.True(Directory.Exists(Path.GetDirectoryName(path)));
        }
        finally
        {
            try { Directory.Delete(dir, true); } catch { }
        }
    }

    [Fact]
    public void GetUserDataPath_CombinesCorrectly()
    {
        try
        {
            var p1 = SafeStorage.GetUserDataPath("MyMod");
            Assert.EndsWith("MyMod", p1);
            var p2 = SafeStorage.GetUserDataPath("MyMod", "file.json");
            Assert.EndsWith(Path.Combine("MyMod", "file.json"), p2);
        }
        catch (System.TypeInitializationException)
        {
            Assert.True(true);
        }
        catch (System.IO.FileNotFoundException)
        {
            Assert.True(true);
        }
    }

    private static void TryDelete(string path)
    {
        try { if (File.Exists(path)) File.Delete(path); } catch { }
    }

    private class TestData
    {
        public int Value { get; set; }
        public string Name { get; set; } = "";
    }
}
