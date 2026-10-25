using UnityEngine;
using WorldLook;
using Xunit;

namespace WaterShader.Tests;

/// <summary>Real cache control flow with native boundary doubles; no gameplay/render proof.</summary>
public sealed class ShaderCacheTests : IDisposable
{
    private const string AssetPath = "Assets/S1Mods/ReferenceWater.shader";
    private readonly List<string> _messages = new();

    public ShaderCacheTests()
    {
        ShaderAssets.Dispose();
        AssetBundle.MemoryLoads = AssetBundle.FileLoads = 0;
        AssetBundle.MemoryLoader = AssetBundle.FileLoader = () => null;
    }

    public void Dispose()
    {
        ShaderAssets.Dispose();
        string directory = S1Mods.Shared.SafeStorage.CacheDirectory;
        if (Directory.Exists(directory))
        {
            foreach (string file in Directory.GetFiles(directory)) File.Delete(file);
            Directory.Delete(directory);
        }
    }

    private AssetBundle Serve(Shader shader)
    {
        var bundle = new AssetBundle { AssetLoader = path => path == AssetPath ? shader : null };
        AssetBundle.MemoryLoader = () => bundle;
        return bundle;
    }

    private Shader? Load() => ShaderAssets.LoadShader(AssetPath, _messages.Add);

    [Fact]
    public void CachedShaderSurvivesUnusedCleanupBetweenSceneConsumers()
    {
        var shader = new Shader { hideFlags = HideFlags.HideInInspector };
        AssetBundle bundle = Serve(shader);
        Assert.Same(shader, Load());
        shader.SimulateUnusedAssetCleanup();
        Assert.True(shader.NativeAlive);
        Assert.Same(shader, Load());
        Assert.Equal(HideFlags.HideInInspector | HideFlags.DontUnloadUnusedAsset, shader.hideFlags);
        Assert.Equal(1, bundle.AssetLoads);
        Assert.Equal(1, AssetBundle.MemoryLoads);
        Assert.Empty(_messages);
    }

    [Theory]
    [InlineData("destroyed")]
    [InlineData("collected")]
    [InlineData("zero-pointer")]
    [InlineData("unsupported")]
    [InlineData("native-read-throws")]
    public void StaleCachedShaderIsReacquiredFromTheLiveBundle(string failure)
    {
        var original = new Shader();
        AssetBundle bundle = Serve(original);
        Assert.Same(original, Load());
        switch (failure)
        {
            case "destroyed": original.NativeAlive = false; break;
            case "collected": original.WasCollected = true; break;
            case "zero-pointer": original.Pointer = IntPtr.Zero; break;
            case "unsupported": original.Supported = false; break;
            case "native-read-throws": original.ThrowOnSupportRead = true; break;
        }
        var replacement = new Shader();
        bundle.AssetLoader = _ => replacement;
        Assert.Same(replacement, Load());
        Assert.Equal(HideFlags.DontUnloadUnusedAsset, replacement.hideFlags);
        Assert.Equal(2, bundle.AssetLoads);
        Assert.Equal(1, AssetBundle.MemoryLoads);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void LostBundleAndShaderCanBeReloadedWithoutRestart(bool collected)
    {
        var original = new Shader();
        AssetBundle originalBundle = Serve(original);
        Assert.Same(original, Load());
        original.NativeAlive = false;
        if (collected) originalBundle.WasCollected = true;
        else originalBundle.NativeAlive = false;
        var replacement = new Shader();
        AssetBundle replacementBundle = Serve(replacement);
        Assert.Same(replacement, Load());
        Assert.Equal(2, AssetBundle.MemoryLoads);
        Assert.Equal(1, replacementBundle.AssetLoads);
        Assert.Empty(originalBundle.Unloads);
    }

    [Fact]
    public void FailedBundleRequestIsBoundedAndNextExplicitRequestCanRecover()
    {
        Assert.Null(Load());
        Assert.Equal(1, AssetBundle.MemoryLoads);
        Assert.Equal(1, AssetBundle.FileLoads);
        Assert.Single(_messages);
        var shader = new Shader();
        Serve(shader);
        Assert.Same(shader, Load());
        Assert.Equal(2, AssetBundle.MemoryLoads);
        Assert.Equal(1, AssetBundle.FileLoads);
    }

    [Theory]
    [InlineData("missing")]
    [InlineData("unsupported")]
    [InlineData("throws")]
    [InlineData("retention-rejected")]
    public void FailedAssetRequestDoesNotPoisonLaterInitialization(string failure)
    {
        var original = new Shader();
        AssetBundle bundle = Serve(original);
        switch (failure)
        {
            case "missing": bundle.AssetLoader = _ => null; break;
            case "unsupported": original.Supported = false; break;
            case "throws": bundle.AssetLoader = _ => throw new InvalidOperationException("native load failed"); break;
            case "retention-rejected": original.IgnoreFlagWrite = true; break;
        }
        Assert.Null(Load());
        Assert.Equal(1, bundle.AssetLoads);
        Assert.Single(_messages);
        var replacement = new Shader();
        bundle.AssetLoader = _ => replacement;
        Assert.Same(replacement, Load());
        Assert.Equal(2, bundle.AssetLoads);
        Assert.Equal(1, AssetBundle.MemoryLoads);
    }

    [Fact]
    public void ShutdownRemovesOnlyOwnedRetentionAndUnloadsWithoutDestroyingBorrowers()
    {
        var shader = new Shader { hideFlags = HideFlags.HideInInspector };
        AssetBundle bundle = Serve(shader);
        Assert.Same(shader, Load());
        shader.hideFlags |= HideFlags.DontSaveInEditor;
        ShaderAssets.Dispose();
        Assert.Equal(HideFlags.HideInInspector | HideFlags.DontSaveInEditor, shader.hideFlags);
        Assert.True(shader.NativeAlive);
        Assert.Equal(new[] { false }, bundle.Unloads);
        ShaderAssets.Dispose();
        Assert.Single(bundle.Unloads);
    }

    [Fact]
    public void ShutdownPreservesRetentionThatAlreadyExistedOnTheAsset()
    {
        var shader = new Shader { hideFlags = HideFlags.HideInHierarchy | HideFlags.DontUnloadUnusedAsset };
        Serve(shader);
        Assert.Same(shader, Load());
        ShaderAssets.Dispose();
        Assert.Equal(HideFlags.HideInHierarchy | HideFlags.DontUnloadUnusedAsset, shader.hideFlags);
        Assert.True(shader.NativeAlive);
    }

    [Fact]
    public void CacheCanInitializeAgainAfterIdempotentCleanup()
    {
        var shader = new Shader();
        Serve(shader);
        Assert.Same(shader, Load());
        ShaderAssets.Dispose();
        ShaderAssets.Dispose();
        var replacement = new Shader();
        AssetBundle replacementBundle = Serve(replacement);
        Assert.Same(replacement, Load());
        Assert.Equal(1, replacementBundle.AssetLoads);
        Assert.Equal(2, AssetBundle.MemoryLoads);
    }
}
