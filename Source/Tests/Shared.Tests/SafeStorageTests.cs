using System;
using System.IO;
using System.Text.Json;
using S1Mods.Shared;
using Xunit;

namespace Shared.Tests;

/// <summary>
/// Tests for S1Mods.Shared.SafeStorage: atomic JSON IO, backup-on-corruption,
/// fallback safety. Pure filesystem behaviour — no MelonLoader runtime needed
/// because SafeStorage catches all logger exceptions internally.
/// </summary>
public class SafeStorageTests
{
    private static string TempPath(string label) =>
        Path.Combine(Path.GetTempPath(), $"S1Test_{label}_{Guid.NewGuid():N}.json");

    private static void TryDelete(string path)
    {
        try { if (File.Exists(path)) File.Delete(path); } catch { /* ignore */ }
    }

    [Fact]
    public void SaveAtomic_CreatesFileAndContent()
    {
        string path = TempPath("create");
        try
        {
            var data = new TestData { Value = 42, Name = "hello" };
            Assert.True(SafeStorage.SaveAtomic(path, data));
            Assert.True(File.Exists(path));
            string content = File.ReadAllText(path);
            Assert.Contains("\"Value\": 42", content);
            Assert.Contains("\"Name\": \"hello\"", content);
        }
        finally
        {
            TryDelete(path);
            TryDelete(path + ".bak");
            TryDelete(path + ".tmp");
        }
    }

    [Fact]
    public void SaveAtomic_Overwrite_ProducesBackup()
    {
        string path = TempPath("overwrite");
        try
        {
            // First write — no backup yet (no prior file)
            Assert.True(SafeStorage.SaveAtomic(path, new TestData { Value = 1, Name = "v1" }));
            Assert.False(File.Exists(path + ".bak"));

            // Second write — backup of v1 should appear
            Assert.True(SafeStorage.SaveAtomic(path, new TestData { Value = 2, Name = "v2" }));
            Assert.True(File.Exists(path + ".bak"));
            Assert.Contains("\"Value\": 1", File.ReadAllText(path + ".bak"));
            Assert.Contains("\"Value\": 2", File.ReadAllText(path));
        }
        finally
        {
            TryDelete(path);
            TryDelete(path + ".bak");
        }
    }

    [Fact]
    public void SaveAtomic_CleansUpTempFile()
    {
        string path = TempPath("cleanup");
        try
        {
            SafeStorage.SaveAtomic(path, new TestData { Value = 7, Name = "x" });
            // Atomic write path creates .tmp then moves it — ensure no leftover
            Assert.False(File.Exists(path + ".tmp"));
        }
        finally
        {
            TryDelete(path);
            TryDelete(path + ".bak");
        }
    }

    [Fact]
    public void SaveAtomic_EnsuresDirectory()
    {
        string dir = Path.Combine(Path.GetTempPath(), $"S1TestNested_{Guid.NewGuid():N}");
        string path = Path.Combine(dir, "deep", "nested", "data.json");
        try
        {
            Assert.False(Directory.Exists(Path.GetDirectoryName(path)!));
            Assert.True(SafeStorage.SaveAtomic(path, new TestData { Value = 1, Name = "deep" }));
            Assert.True(File.Exists(path));
        }
        finally
        {
            try { Directory.Delete(dir, recursive: true); } catch { }
        }
    }

    [Fact]
    public void LoadSafe_MissingFile_ReturnsFallback()
    {
        string path = TempPath("missing");
        try
        {
            var fallback = new TestData { Value = -1, Name = "fallback" };
            var loaded = SafeStorage.LoadSafe<TestData>(path, fallback);
            Assert.Same(fallback, loaded);
        }
        finally
        {
            TryDelete(path);
        }
    }

    [Fact]
    public void LoadSafe_ValidFile_ReturnsContent()
    {
        string path = TempPath("valid");
        try
        {
            SafeStorage.SaveAtomic(path, new TestData { Value = 99, Name = "real" });
            var fallback = new TestData { Value = -1, Name = "fallback" };
            var loaded = SafeStorage.LoadSafe<TestData>(path, fallback);
            Assert.Equal(99, loaded.Value);
            Assert.Equal("real", loaded.Name);
        }
        finally
        {
            TryDelete(path);
            TryDelete(path + ".bak");
        }
    }

    [Fact]
    public void LoadSafe_CorruptFile_FallsBackToBackup()
    {
        string path = TempPath("corrupt");
        try
        {
            // Create v1, then v2 (so .bak = v1), then corrupt the main file
            SafeStorage.SaveAtomic(path, new TestData { Value = 10, Name = "v1" });
            SafeStorage.SaveAtomic(path, new TestData { Value = 20, Name = "v2" });
            Assert.True(File.Exists(path + ".bak"));

            File.WriteAllText(path, "{ this is not valid json !!!");

            var fallback = new TestData { Value = -1, Name = "fallback" };
            var loaded = SafeStorage.LoadSafe<TestData>(path, fallback);
            Assert.Equal(10, loaded.Value);
            Assert.Equal("v1", loaded.Name);
        }
        finally
        {
            TryDelete(path);
            TryDelete(path + ".bak");
        }
    }

    [Fact]
    public void LoadSafe_EmptyFile_ReturnsFallback()
    {
        string path = TempPath("empty");
        try
        {
            File.WriteAllText(path, "");
            var fallback = new TestData { Value = -1, Name = "fb" };
            var loaded = SafeStorage.LoadSafe<TestData>(path, fallback);
            Assert.Equal(-1, loaded.Value);
        }
        finally
        {
            TryDelete(path);
        }
    }

    [Fact]
    public void LoadSafe_BothCorrupt_ReturnsFallback()
    {
        string path = TempPath("both-bad");
        try
        {
            File.WriteAllText(path, "garbage main");
            File.WriteAllText(path + ".bak", "garbage bak");

            var fallback = new TestData { Value = -1, Name = "fb" };
            var loaded = SafeStorage.LoadSafe<TestData>(path, fallback);
            Assert.Same(fallback, loaded);
        }
        finally
        {
            TryDelete(path);
            TryDelete(path + ".bak");
        }
    }

    [Fact]
    public void LoadTextSafe_MissingFile_ReturnsFallback()
    {
        string path = TempPath("text-missing");
        try
        {
            string result = SafeStorage.LoadTextSafe(path, "default-value");
            Assert.Equal("default-value", result);
        }
        finally
        {
            TryDelete(path);
        }
    }

    [Fact]
    public void LoadTextSafe_ValidFile_ReturnsContent()
    {
        string path = TempPath("text-valid");
        try
        {
            File.WriteAllText(path, "hello world");
            Assert.Equal("hello world", SafeStorage.LoadTextSafe(path, "default"));
        }
        finally
        {
            TryDelete(path);
        }
    }

    [Fact]
    public void SaveTextAtomic_Overwrite_ProducesBackup()
    {
        string path = TempPath("text-overwrite");
        try
        {
            Assert.True(SafeStorage.SaveTextAtomic(path, "first"));
            Assert.True(SafeStorage.SaveTextAtomic(path, "second"));
            Assert.Equal("first", File.ReadAllText(path + ".bak"));
            Assert.Equal("second", File.ReadAllText(path));
        }
        finally
        {
            TryDelete(path);
            TryDelete(path + ".bak");
        }
    }

    [Fact]
    public void EnsureDirectoryForFile_EmptyPath_NoOp()
    {
        // Should not throw on null/empty input
        SafeStorage.EnsureDirectoryForFile("");
        SafeStorage.EnsureDirectoryForFile(null!);
    }

    [Fact]
    public void EnsureDirectory_CreatesNestedDir()
    {
        string root = Path.Combine(Path.GetTempPath(), $"S1TestEnsure_{Guid.NewGuid():N}");
        string dir = Path.Combine(root, "a", "b");
        try
        {
            Assert.False(Directory.Exists(dir));
            SafeStorage.EnsureDirectory(dir);
            Assert.True(Directory.Exists(dir));
        }
        finally
        {
            try { Directory.Delete(root, recursive: true); } catch { }
        }
    }

    private class TestData
    {
        public int Value { get; set; }
        public string Name { get; set; } = "";
    }
}
