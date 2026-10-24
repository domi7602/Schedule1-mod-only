using Il2CppScheduleOne.DevUtilities;
using Il2CppScheduleOne.GameTime;
using S1API.Lifecycle;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using Xunit;

namespace GoldenHour.Tests;

// Executes the production managed module against RuntimeBoundary.cs. Real Unity/IL2CPP
// ordering, native setter precision and rendered appearance still require in-game checks.
public sealed class RuntimeWiringTests : IDisposable
{
    private readonly Mod _module = new();
    private static readonly Color Native = new(0.6f, 0.7f, 0.8f, 1f);

    public RuntimeWiringTests()
    {
        Assert.Equal(0, GameLifecycle.Subscribers);
        SceneManager.Active = new Scene(1, "Main");
        RenderSettings.skybox = new Material();
        RenderSettings.sun = new Light();
        RenderSettings.ambientMode = AmbientMode.Trilight;
        RenderSettings.Sky = new NativeColor();
        RenderSettings.Equator = new NativeColor();
        RenderSettings.Ground = new NativeColor();
        NetworkSingleton<TimeManager>.Instance = new TimeManager();
        Time.unscaledTime = 0;
    }

    public void Dispose()
    {
        _module.OnDeinitializeMelon();
        Assert.Equal(0, GameLifecycle.Subscribers);
    }

    private Dictionary<string, object?> Status => _module.GetDiagnosticsSnapshot();
    private void Start(double warmth = 1.0)
    {
        _module.OnInitializeMelon();
        _module.SetEnabled(true, warmth);
        Frame();
    }

    private void Frame()
    {
        Time.unscaledTime += 0.02f;
        _module.OnUpdate();
        _module.OnLateUpdate();
    }

    private static Color Biased(Color color, Argb tint, double warmth = 1.0)
    {
        Argb biased = BlendPolicy.Blend(new Argb(color.r, color.g, color.b, color.a), tint, warmth);
        return new Color(biased.R, biased.G, biased.B, biased.A);
    }

    [Fact]
    public void Startup_in_menu_defers_activation_until_readable_Main_targets()
    {
        SceneManager.Active = new Scene(5, "Menu");
        _module.SetEnabled(true, 1);
        Frame();
        Assert.Equal(0, RenderSettings.sun!.Slot.Writes);
        _module.OnInitializeMelon();
        Frame();
        Assert.False((bool)Status["active"]!);
        Assert.True((bool)Status["desiredEnabled"]!);

        SceneManager.Active = new Scene(10, "Main");
        RenderSettings.sun = null;
        RenderSettings.skybox = null;
        RenderSettings.ambientMode = AmbientMode.Flat;
        Frame();
        Assert.False((bool)Status["active"]!);
        RenderSettings.sun = new Light();
        Frame();
        Assert.True((bool)Status["active"]!);
        Assert.Equal(Biased(Native, GoldenHourTargets.SunLightTint), RenderSettings.sun.Slot.Value);
    }

    [Fact]
    public void Writes_happen_in_LateUpdate_and_native_drift_is_never_frozen_or_compounded()
    {
        _module.OnInitializeMelon();
        _module.SetEnabled(true, 1);
        _module.OnUpdate();
        Assert.Equal(0, RenderSettings.sun!.Slot.Writes);
        for (int frame = 0; frame < 240; frame++)
        {
            Color native = Native with { r = Native.r + frame * 0.00001f };
            RenderSettings.sun.Slot.Value = native;
            RenderSettings.Sky.Value = native;
            Frame();
            Assert.Equal(Biased(native, GoldenHourTargets.SunLightTint), RenderSettings.sun.Slot.Value);
            Assert.Equal(Biased(native, GoldenHourTargets.AmbientTint), RenderSettings.Sky.Value);
        }
        Color latestNative = Native with { r = Native.r + 239 * 0.00001f };
        _module.SetEnabled(false, 1);
        Assert.Equal(latestNative, RenderSettings.sun.Slot.Value);
        Assert.Equal(latestNative, RenderSettings.Sky.Value);
    }

    [Fact]
    public void Repeated_toggle_and_live_warmth_changes_restore_all_five_values()
    {
        Start();
        Material sky = RenderSettings.skybox!;
        Light sun = RenderSettings.sun!;
        foreach (double warmth in new[] { 0.2, 0.7, 1.0, 0.0, 0.9 })
        {
            _module.SetEnabled(true, warmth);
            Frame();
            Assert.Equal(Biased(Native, GoldenHourTargets.SunLightTint, warmth), sun.Slot.Value);
            Assert.Equal(Biased(Native, GoldenHourTargets.SkySunGlowTint, warmth), sky.Slot.Value);
            _module.SetEnabled(false, warmth);
            Frame();
            Assert.Equal(Native, sun.Slot.Value);
            Assert.Equal(Native, sky.Slot.Value);
            Assert.Equal(Native, RenderSettings.Sky.Value);
            Assert.Equal(Native, RenderSettings.Equator.Value);
            Assert.Equal(Native, RenderSettings.Ground.Value);
            Assert.Same(sky, RenderSettings.skybox);
            Assert.Same(sun, RenderSettings.sun);
        }
    }

    [Theory]
    [InlineData(600, 1.0)]
    [InlineData(1200, 0.0)]
    [InlineData(1800, 1.0)]
    [InlineData(2359, 0.0)]
    [InlineData(0, 0.0)]
    [InlineData(1260, 0.0)]
    public void Actual_module_uses_HHMM_and_releases_old_tint(int hhmm, double strength)
    {
        Start();
        NetworkSingleton<TimeManager>.Instance!.RawHhmm = hhmm;
        Frame();
        Assert.Equal(strength, Status["appliedStrength"]);
        Assert.Equal(Biased(Native, GoldenHourTargets.SunLightTint, strength), RenderSettings.sun!.Slot.Value);
        Assert.Equal(hhmm != 1260, Status["timeReadable"]);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void Missing_or_throwing_clock_is_neutral_and_can_recover(bool throwing)
    {
        Start();
        if (throwing) NetworkSingleton<TimeManager>.Instance!.ThrowOnRead = true;
        else NetworkSingleton<TimeManager>.Instance = null;
        Frame();
        Assert.False((bool)Status["timeReadable"]!);
        Assert.Equal(0.0, Status["appliedStrength"]);
        Assert.Equal(Native, RenderSettings.sun!.Slot.Value);
        NetworkSingleton<TimeManager>.Instance = new TimeManager();
        Frame();
        Assert.Equal(1.0, Status["appliedStrength"]);
    }

    [Theory]
    [InlineData(double.NaN, 0.0)]
    [InlineData(double.PositiveInfinity, 0.0)]
    [InlineData(-2.0, 0.0)]
    [InlineData(2.0, 1.0)]
    public void Coordinator_warmth_is_clamped_before_applying(double value, double expected)
    {
        Start(value);
        Assert.Equal(expected, Status["warmth"]);
        Assert.Equal(expected, Status["appliedStrength"]);
    }

    [Fact]
    public void Replacement_objects_never_receive_old_baselines_on_OFF()
    {
        Start();
        Material oldSky = RenderSettings.skybox!;
        Light oldSun = RenderSettings.sun!;
        Material newSky = new();
        Light newSun = new();
        Color foreign = new(0.2f, 0.3f, 0.4f, 1f);
        newSky.Slot.Value = newSun.Slot.Value = foreign;
        RenderSettings.skybox = newSky;
        RenderSettings.sun = newSun;
        _module.SetEnabled(false, 1);
        Assert.Equal(Native, oldSky.Slot.Value);
        Assert.Equal(Native, oldSun.Slot.Value);
        Assert.Equal(foreign, newSky.Slot.Value);
        Assert.Equal(foreign, newSun.Slot.Value);
        Assert.Equal(0, newSky.Slot.Writes);
        Assert.Equal(0, newSun.Slot.Writes);
    }

    [Fact]
    public void Replacement_while_enabled_restores_old_target_then_adopts_new_target()
    {
        Start();
        Material oldSky = RenderSettings.skybox!;
        Light oldSun = RenderSettings.sun!;
        RenderSettings.skybox = new Material();
        RenderSettings.sun = new Light();
        Color replacement = new(0.2f, 0.3f, 0.4f, 1f);
        RenderSettings.skybox.Slot.Value = RenderSettings.sun.Slot.Value = replacement;
        Frame();
        Assert.Equal(Native, oldSky.Slot.Value);
        Assert.Equal(Native, oldSun.Slot.Value);
        Assert.Equal(Biased(replacement, GoldenHourTargets.SunLightTint), RenderSettings.sun.Slot.Value);
        _module.SetEnabled(false, 1);
        Assert.Equal(replacement, RenderSettings.sun.Slot.Value);
        Assert.Equal(replacement, RenderSettings.skybox.Slot.Value);
    }

    [Fact]
    public void Dead_target_is_released_without_accessing_its_color()
    {
        Start();
        Light old = RenderSettings.sun!;
        old.Alive = false;
        int reads = old.Slot.Reads;
        int writes = old.Slot.Writes;
        RenderSettings.sun = new Light();
        Frame();
        Assert.Equal(reads, old.Slot.Reads);
        Assert.Equal(writes, old.Slot.Writes);
        Assert.Equal(Biased(Native, GoldenHourTargets.SunLightTint), RenderSettings.sun.Slot.Value);
    }

    [Fact]
    public void Tiny_external_changes_win_on_OFF()
    {
        Start();
        Color foreign = RenderSettings.sun!.Slot.Value with { r = RenderSettings.sun.Slot.Value.r + 0.00001f };
        RenderSettings.sun.Slot.Value = foreign;
        RenderSettings.skybox!.Slot.Value = foreign;
        RenderSettings.Sky.Value = foreign;
        _module.SetEnabled(false, 1);
        Assert.Equal(foreign, RenderSettings.sun.Slot.Value);
        Assert.Equal(foreign, RenderSettings.skybox.Slot.Value);
        Assert.Equal(foreign, RenderSettings.Sky.Value);
    }

    [Fact]
    public void Scene_exit_restores_borrowed_objects_but_never_the_destination_ambient()
    {
        Start();
        Light oldSun = RenderSettings.sun!;
        Material oldSky = RenderSettings.skybox!;
        SceneManager.Active = new Scene(2, "Menu");
        RenderSettings.sun = new Light();
        RenderSettings.skybox = new Material();
        RenderSettings.Sky = new NativeColor();
        RenderSettings.Equator = new NativeColor();
        RenderSettings.Ground = new NativeColor();
        _module.OnSceneWasUnloaded(1, "Main");
        Frame();
        Assert.Equal(Native, oldSun.Slot.Value);
        Assert.Equal(Native, oldSky.Slot.Value);
        Assert.Equal(0, RenderSettings.Sky.Writes);
        Assert.Equal(0, RenderSettings.sun.Slot.Writes);
        Assert.Equal(0, RenderSettings.skybox.Slot.Writes);
        Assert.True((bool)Status["desiredEnabled"]!);
        Assert.False((bool)Status["active"]!);
    }

    [Fact]
    public void Lifecycle_suspends_and_restores_then_reapplies_desired_state_on_new_world()
    {
        Start();
        _module.OnSceneWasUnloaded(8, "AdditiveInterior");
        Assert.True((bool)Status["active"]!);
        GameLifecycle.PreSceneChange();
        GameLifecycle.PreLoad();
        Assert.Equal(Native, RenderSettings.sun!.Slot.Value);
        Frame();
        Assert.False((bool)Status["active"]!);
        Assert.True((bool)Status["desiredEnabled"]!);
        SceneManager.Active = new Scene(11, "Main");
        RenderSettings.sun = new Light();
        GameLifecycle.LoadComplete();
        Frame();
        Assert.True((bool)Status["active"]!);
        Assert.Equal(Biased(Native, GoldenHourTargets.SunLightTint), RenderSettings.sun.Slot.Value);
    }

    [Fact]
    public void Repeated_initialize_and_shutdown_are_idempotent_and_unsubscribe_exactly()
    {
        Start();
        _module.OnInitializeMelon();
        Assert.Equal(3, GameLifecycle.Subscribers);
        _module.OnDeinitializeMelon();
        int writes = RenderSettings.sun!.Slot.Writes;
        _module.OnDeinitializeMelon();
        _module.SetEnabled(true, 1);
        GameLifecycle.LoadComplete();
        Frame();
        Assert.Equal(writes, RenderSettings.sun.Slot.Writes);
        Assert.Equal(Native, RenderSettings.sun.Slot.Value);
        Assert.Equal(0, GameLifecycle.Subscribers);
    }

    [Fact]
    public void Ambient_is_only_written_in_Trilight_and_waits_for_mode_to_return_on_restore()
    {
        RenderSettings.ambientMode = AmbientMode.Skybox;
        Start();
        Assert.Equal(0, RenderSettings.Sky.Writes);
        RenderSettings.ambientMode = AmbientMode.Trilight;
        Frame();
        Assert.Equal(1, RenderSettings.Sky.Writes);
        RenderSettings.ambientMode = AmbientMode.Flat;
        _module.SetEnabled(false, 1);
        for (int second = 1; second <= 10; second++)
        {
            Time.unscaledTime = second;
            _module.OnUpdate();
        }
        Assert.Equal(1, RenderSettings.Sky.Writes);
        Assert.Equal(3, Status["pendingRestores"]);
        RenderSettings.ambientMode = AmbientMode.Trilight;
        Time.unscaledTime = 12;
        _module.OnUpdate();
        Assert.Equal(Native, RenderSettings.Sky.Value);
        Assert.Equal(0, Status["pendingRestores"]);
    }

    [Fact]
    public void Missing_optional_sunshine_property_does_not_disable_other_channels()
    {
        RenderSettings.skybox!.HasSunShine = false;
        Start();
        Assert.Equal(0, RenderSettings.skybox.Slot.Writes);
        Assert.Equal(4, Status["readableChannels"]);
        Assert.Equal(Biased(Native, GoldenHourTargets.SunLightTint), RenderSettings.sun!.Slot.Value);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void Partial_setter_failure_restores_only_its_lane_and_does_not_retry_every_frame(bool after)
    {
        RenderSettings.sun!.Slot.ThrowAfterWrites = after ? 1 : 0;
        RenderSettings.sun.Slot.ThrowBeforeWrite = !after;
        Start();
        Frame();
        Assert.Equal(Native, RenderSettings.sun.Slot.Value);
        int sunWrites = RenderSettings.sun.Slot.Writes;
        for (int frame = 0; frame < 100; frame++) Frame();
        Assert.Equal(sunWrites, RenderSettings.sun.Slot.Writes);
        Assert.Equal(Biased(Native, GoldenHourTargets.SkySunGlowTint), RenderSettings.skybox!.Slot.Value);
        Assert.True((int)Status["errors"]! > 0);
        Assert.Equal(0, Status["pendingRestores"]);
        RenderSettings.sun.Slot.ThrowBeforeWrite = false;
        _module.SetEnabled(false, 1);
        _module.SetEnabled(true, 1);
        Frame();
        Assert.Equal(Biased(Native, GoldenHourTargets.SunLightTint), RenderSettings.sun.Slot.Value);
    }

    [Fact]
    public void Unresolved_restore_blocks_reactivation_until_native_drift_releases_it()
    {
        Start();
        Light sun = RenderSettings.sun!;
        sun.Slot.IgnoreWrite = true;
        _module.SetEnabled(false, 1);
        _module.SetEnabled(true, 1);
        for (int second = 1; second <= 10; second++)
        {
            Time.unscaledTime = second;
            Frame();
        }
        Assert.Equal(4, sun.Slot.Writes); // first apply + three restore attempts
        Assert.Equal(1, Status["pendingRestores"]);
        sun.Slot.IgnoreWrite = false;
        sun.Slot.Value = Native;
        Time.unscaledTime = 12;
        Frame();
        Assert.Equal(0, Status["pendingRestores"]);
        Assert.Equal(Biased(Native, GoldenHourTargets.SunLightTint), sun.Slot.Value);
    }

    [Fact]
    public void Diagnostics_are_cached_and_do_not_touch_native_resources()
    {
        Start(0.75);
        int sunReads = RenderSettings.sun!.Slot.Reads;
        int skyReads = RenderSettings.skybox!.Slot.Reads;
        int ambientReads = RenderSettings.Sky.Reads;
        for (int read = 0; read < 100; read++)
        {
            Assert.Equal(0.75, Status["appliedStrength"]);
            Assert.Equal(1800, Status["timeHhmm"]);
            Assert.Equal(1080, Status["minutesSinceMidnight"]);
        }
        Assert.Equal(sunReads, RenderSettings.sun.Slot.Reads);
        Assert.Equal(skyReads, RenderSettings.skybox.Slot.Reads);
        Assert.Equal(ambientReads, RenderSettings.Sky.Reads);
    }
}
