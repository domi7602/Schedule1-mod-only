using System.Text.Json;
using Il2CppScheduleOne.Persistence;
using Il2CppScheduleOne.Networking;
using Il2CppScheduleOne.PlayerScripts;
using S1Mods.Shared;
using UnityEngine;
using UnityEngine.SceneManagement;
using Xunit;

namespace WorldLook.Tests;

[CollectionDefinition("Automation", DisableParallelization = true)]
public sealed class AutomationCollection { }

[Collection("Automation")]
public sealed class AutomationTests : IDisposable
{
    private readonly WorldLookMod _mod;

    public AutomationTests()
    {
        SceneManager.Active = new("Main");
        SceneManager.Reads = 0;
        LoadManager.Instance = new();
        Lobby.Instance = new();
        WaterShader.WaterShaderMod.ThrowInitialize = false;
        Player.PlayerList = new() { new object() };
        NetworkGuard.Host = true;
        Input.Shortcut = false;
        _mod = new();
        _mod.OnInitializeMelon();
        _mod.OnLateInitializeMelon();
    }

    private JsonElement Request(string action, bool? value = null) => JsonDocument.Parse(_mod.AutomationRequest(action, value)).RootElement;

    [Fact]
    public void ParameterlessEndpointHasExplicitReadbackAndBecomesUnavailableAfterTeardown()
    {
        var endpoint = GameObject.Find("WorldLook.Automation")!.GetComponent<WorldLookAutomation>();
        var off = JsonDocument.Parse(endpoint.DisableLook()).RootElement;
        Assert.True(off.GetProperty("accepted").GetBoolean());
        Assert.False(off.GetProperty("requestedEnabled").GetBoolean());
        Assert.False(JsonDocument.Parse(endpoint.GetStatus()).RootElement.GetProperty("requestedEnabled").GetBoolean());
        Assert.True(JsonDocument.Parse(endpoint.EnableLook()).RootElement.GetProperty("requestedEnabled").GetBoolean());
        _mod.OnDeinitializeMelon();
        var stale = JsonDocument.Parse(endpoint.DisableLook()).RootElement;
        Assert.False(stale.GetProperty("accepted").GetBoolean());
        Assert.Equal("endpoint-unavailable-or-wrong-thread", stale.GetProperty("error").GetString());
    }

    [Fact]
    public void RepeatedExplicitStateDoesNotToggleOrReapply()
    {
        int writes = SafeStorage.Writes;
        var changed = Request("disable", false);
        Assert.True(changed.GetProperty("accepted").GetBoolean());
        Assert.True(changed.GetProperty("changed").GetBoolean());
        int lightCalls = GoldenHour.Mod.Latest.SetCalls;
        int waterCalls = WaterShader.WaterShaderMod.Latest.SetCalls;
        var repeated = Request("disable", false);
        Assert.True(repeated.GetProperty("accepted").GetBoolean());
        Assert.False(repeated.GetProperty("changed").GetBoolean());
        Assert.False(repeated.GetProperty("requestedEnabled").GetBoolean());
        Assert.Equal(lightCalls, GoldenHour.Mod.Latest.SetCalls);
        Assert.Equal(waterCalls, WaterShader.WaterShaderMod.Latest.SetCalls);
        Assert.Equal(writes, SafeStorage.Writes);
    }

    [Theory]
    [InlineData("menu")]
    [InlineData("loading")]
    [InlineData("unloaded")]
    [InlineData("client")]
    [InlineData("multiplayer")]
    [InlineData("no-player")]
    [InlineData("missing-save")]
    [InlineData("one-player-lobby-host")]
    [InlineData("missing-lobby")]
    public void UnsafeWorldRejectsMutationWithoutChangingRequest(string condition)
    {
        switch (condition)
        {
            case "menu": SceneManager.Active = new("Menu"); break;
            case "loading": LoadManager.Instance.IsLoading = true; break;
            case "unloaded": LoadManager.Instance.IsGameLoaded = false; break;
            case "client": NetworkGuard.Host = false; break;
            case "multiplayer": Player.PlayerList.Add(new object()); break;
            case "no-player": Player.PlayerList.Clear(); break;
            case "missing-save": LoadManager.Instance.LoadedGameFolderPath = ""; break;
            case "one-player-lobby-host": Lobby.Instance.IsInLobby = true; break;
            case "missing-lobby": Lobby.Instance.Alive = false; break;
        }
        int calls = GoldenHour.Mod.Latest.SetCalls;
        var result = Request("disable", false);
        Assert.False(result.GetProperty("accepted").GetBoolean());
        Assert.False(result.GetProperty("ready").GetBoolean());
        Assert.True(result.GetProperty("requestedEnabled").GetBoolean());
        Assert.Equal(calls, GoldenHour.Mod.Latest.SetCalls);
    }

    [Fact]
    public void ManualOffStillDisablesLightingWhenWaterInitializationFailed()
    {
        _mod.OnDeinitializeMelon();
        WaterShader.WaterShaderMod.ThrowInitialize = true;
        var partial = new WorldLookMod();
        try
        {
            partial.OnInitializeMelon();
            partial.OnLateInitializeMelon();
            var initial = JsonDocument.Parse(partial.AutomationRequest("status", null)).RootElement;
            Assert.False(initial.GetProperty("ready").GetBoolean());
            Assert.True(initial.GetProperty("lightingInitialized").GetBoolean());
            Assert.False(initial.GetProperty("waterInitialized").GetBoolean());
            Assert.True(GoldenHour.Mod.Latest.Enabled);
            Input.Shortcut = true;
            partial.OnUpdate();
            Assert.False(GoldenHour.Mod.Latest.Enabled);
            var after = JsonDocument.Parse(partial.AutomationRequest("status", null)).RootElement;
            Assert.False(after.GetProperty("requestedEnabled").GetBoolean());
        }
        finally { Input.Shortcut = false; WaterShader.WaterShaderMod.ThrowInitialize = false; partial.OnDeinitializeMelon(); }
    }

    [Fact]
    public void OffPersistsAcrossMenuAndReloadWithoutWritingProfile()
    {
        Request("disable", false);
        int writes = SafeStorage.Writes;
        _mod.OnSceneWasUnloaded(1, "Main");
        SceneManager.Active = new("Menu");
        LoadManager.Instance.IsGameLoaded = false;
        var menu = Request("status");
        Assert.True(menu.GetProperty("accepted").GetBoolean());
        Assert.False(menu.GetProperty("ready").GetBoolean());
        Assert.False(menu.GetProperty("requestedEnabled").GetBoolean());
        SceneManager.Active = new("Main");
        LoadManager.Instance.IsGameLoaded = true;
        _mod.OnSceneWasLoaded(1, "Main");
        var loaded = Request("status");
        Assert.True(loaded.GetProperty("ready").GetBoolean());
        Assert.False(loaded.GetProperty("requestedEnabled").GetBoolean());
        Assert.Equal(writes, SafeStorage.Writes);
    }

    [Fact]
    public void SameStateDoesNotClearFaultButExplicitOffOnRecovers()
    {
        GoldenHour.Mod.Latest.ThrowUpdate = true;
        _mod.OnUpdate();
        Assert.True(Request("status").GetProperty("lightingFaulted").GetBoolean());
        Assert.False(Request("enable", true).GetProperty("changed").GetBoolean());
        Assert.True(Request("status").GetProperty("lightingFaulted").GetBoolean());
        Input.Shortcut = true;
        _mod.OnUpdate();
        Assert.False(Request("status").GetProperty("requestedEnabled").GetBoolean());
        _mod.OnUpdate();
        Assert.True(Request("status").GetProperty("requestedEnabled").GetBoolean());
        Assert.False(Request("status").GetProperty("lightingFaulted").GetBoolean());
        Assert.True(GoldenHour.Mod.Latest.Enabled);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void MenuRequestIsLatchedEvenWhenNativeOutcomeIsUnknown(bool throws)
    {
        LoadManager.Instance.ThrowMenu = throws;
        var first = Request("return-to-menu");
        Assert.Equal(!throws, first.GetProperty("accepted").GetBoolean());
        Assert.True(first.GetProperty("menuRequested").GetBoolean());
        Assert.False(Request("return-to-menu").GetProperty("accepted").GetBoolean());
        Assert.Equal(1, LoadManager.Instance.MenuCalls);
        Assert.False(Request("disable", false).GetProperty("accepted").GetBoolean());
        _mod.OnSceneWasLoaded(1, "Main");
        Assert.True(Request("disable", false).GetProperty("accepted").GetBoolean());
    }

    [Fact]
    public void ShutdownRejectsActionsAndDestroysOnlyOwnedEndpoint()
    {
        Assert.NotNull(GameObject.Find("WorldLook.Automation"));
        var foreign = new GameObject("Foreign");
        _mod.OnDeinitializeMelon();
        Assert.Null(GameObject.Find("WorldLook.Automation"));
        Assert.True(foreign.Alive);
        var result = Request("disable", false);
        Assert.False(result.GetProperty("accepted").GetBoolean());
        Assert.True(result.GetProperty("requestedEnabled").GetBoolean());
        UnityEngine.Object.Destroy(foreign);
    }

    [Fact]
    public async Task WrongThreadRejectsBeforeTouchingUnity()
    {
        int reads = SceneManager.Reads;
        var result = await Task.Run(() => Request("disable", false));
        Assert.False(result.GetProperty("accepted").GetBoolean());
        Assert.Equal("wrong-thread", result.GetProperty("error").GetString());
        Assert.Equal(reads, SceneManager.Reads);
    }

    public void Dispose() { Input.Shortcut = false; _mod.OnDeinitializeMelon(); }
}
