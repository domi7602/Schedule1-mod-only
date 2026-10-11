using PropertyLEDs.Core;
using Xunit;
namespace PropertyLEDs.Tests;
public class StateTests
{
    [Fact]
    public void Empty_state_returns_detached_disabled_defaults()
    {
        var type = typeof(StateTests).Assembly.GetType("PropertyLEDs.Core.LEDState");
        Assert.NotNull(type);
        var state = Activator.CreateInstance(type!);
        Assert.Equal(1, type!.GetProperty("FormatVersion")!.GetValue(state));
        var method = type.GetMethod("GetEffective");
        Assert.NotNull(method);
        var result = (LEDSettings)method!.Invoke(state, new object?[] { "p", null })!;
        Assert.False(result.Enabled);
        result.Enabled = true;
        Assert.False(((LEDSettings)method.Invoke(state, new object?[] { "p", null })!).Enabled);
    }
    [Fact]
    public void Room_override_changes_only_that_room_and_detaches_input()
    {
        var state = new LEDState();
        var method = typeof(LEDState).GetMethod("SetRoom");
        Assert.NotNull(method);
        var settings = LEDSettings.White();
        settings.Brightness = 8;
        Assert.Equal(true, method!.Invoke(state, new object?[] { "p", "a", settings }));
        settings.Enabled = false;
        Assert.True(state.GetEffective("p", "a").Enabled);
        Assert.Equal(5, state.GetEffective("p", "a").Brightness);
        Assert.False(state.GetEffective("p", "b").Enabled);
        Assert.False(state.GetEffective("other", "a").Enabled);
    }
    [Fact]
    public void Global_edit_clears_all_overrides_for_only_target_property()
    {
        var state = new LEDState();
        state.SetRoom("p", "a", LEDSettings.White());
        state.SetRoom("p", "b", LEDSettings.White());
        state.SetRoom("q", "a", LEDSettings.White());
        var method = typeof(LEDState).GetMethod("SetGlobal");
        Assert.NotNull(method);
        var global = new LEDSettings { Hue = 0.25f, Brightness = 2 };
        Assert.Equal(true, method!.Invoke(state, new object?[] { "p", global }));
        global.Hue = 0;
        Assert.Empty(state.Properties["p"].Rooms);
        Assert.Equal(0.25f, state.GetEffective("p", "a").Hue);
        Assert.True(state.GetEffective("q", "a").Enabled);
    }
    [Fact]
    public void Use_property_removes_only_target_override()
    {
        var state = new LEDState();
        state.SetGlobal("p", new LEDSettings { Brightness = 3 });
        state.SetRoom("p", "a", LEDSettings.White());
        state.SetRoom("p", "b", LEDSettings.White());
        var method = typeof(LEDState).GetMethod("UseProperty");
        Assert.NotNull(method);
        Assert.Equal(true, method!.Invoke(state, new object?[] { "p", "a" }));
        Assert.Equal(3, state.GetEffective("p", "a").Brightness);
        Assert.True(state.GetEffective("p", "b").Enabled);
        Assert.Equal(false, method.Invoke(state, new object?[] { "p", "missing" }));
    }
    [Fact]
    public void Normalize_repairs_null_graphs_and_ranges_without_inventing_overrides()
    {
        var state = new LEDState
        {
            Properties = new()
            {
                [" "] = new(),
                ["nil"] = null!,
                ["p"] = new()
                {
                    Global = null!,
                    Rooms = new() { ["a"] = null!, [""] = LEDSettings.White(), ["b"] = new() { Hue = float.NaN, Brightness = 10 } }
                },
                ["q"] = new() { Rooms = null! }
            }
        };
        var method = typeof(LEDState).GetMethod("Normalize");
        Assert.NotNull(method);
        Assert.Equal(true, method!.Invoke(state, null));
        Assert.Equal(new[] { "p", "q" }, state.Properties.Keys.OrderBy(x => x));
        Assert.False(state.GetEffective("p").Enabled);
        Assert.Single(state.Properties["p"].Rooms);
        Assert.Equal(5, state.GetEffective("p", "b").Brightness);
        Assert.Empty(state.Properties["q"].Rooms);
        state.Properties = null!;
        Assert.Equal(true, method.Invoke(state, null));
        Assert.Empty(state.Properties);
    }
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(2)]
    [InlineData(int.MaxValue)]
    public void Unknown_version_clone_preserves_data_but_refuses_edits_and_effective_lights(int version)
    {
        var state = new LEDState { FormatVersion = version, Properties = new() { ["p"] = new() { Global = new() { Enabled = true, Brightness = 99 }, Rooms = null! } } };
        var before = System.Text.Json.JsonSerializer.Serialize(state);
        Assert.False(state.Normalize());
        Assert.False(state.SetGlobal("p", LEDSettings.White()));
        Assert.False(state.SetRoom("p", "a", LEDSettings.White()));
        Assert.False(state.UseProperty("p", "a"));
        Assert.False(state.GetEffective("p").Enabled);
        Assert.Equal(before, System.Text.Json.JsonSerializer.Serialize(state));
        var method = typeof(LEDState).GetMethod("Clone");
        Assert.NotNull(method);
        var clone = (LEDState)method!.Invoke(state, null)!;
        Assert.Equal(before, System.Text.Json.JsonSerializer.Serialize(clone));
        clone.Properties["p"].Global.Brightness = 3;
        Assert.Equal(99, state.Properties["p"].Global.Brightness);
    }
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Invalid_ids_are_rejected_without_creating_data(string? id)
    {
        var state = new LEDState();
        Assert.False(state.SetGlobal(id, LEDSettings.White()));
        Assert.False(state.SetRoom("p", id, LEDSettings.White()));
        Assert.False(state.UseProperty("p", id));
        Assert.False(state.GetEffective(id).Enabled);
        Assert.Empty(state.Properties);
        state.SetGlobal(" p ", LEDSettings.White());
        Assert.False(state.GetEffective("p").Enabled);
        Assert.True(state.GetEffective(" p ").Enabled);
    }
    // NEXT TEST
}
