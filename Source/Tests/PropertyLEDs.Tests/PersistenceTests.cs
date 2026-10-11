using Xunit;
using PropertyLEDs.Core;
namespace PropertyLEDs.Tests;
public class PersistenceTests
{
    [Fact]
    public void Json_load_boundary_distinguishes_missing_from_corrupt_and_future_files()
    {
        var type = typeof(PersistenceTests).Assembly.GetType("PropertyLEDs.Core.StateCodec");
        Assert.NotNull(type);
        var method = type!.GetMethod("Read");
        Assert.NotNull(method);
        foreach (var example in new (string? Json, bool CanWrite, string Status)[]
        {
            (null, true, "Missing"), ("", false, "Invalid"), ("null", false, "Invalid"),
            ("{broken", false, "Invalid"), ("{}", false, "Invalid"),
            ("{\"FormatVersion\":1,\"Properties\":null}", true, "Loaded"),
            ("{\"FormatVersion\":7,\"FutureData\":{\"anything\":true}}", false, "UnsupportedVersion"),
            ("{\"FormatVersion\":1,\"Properties\":12}", false, "Invalid"),
            ("{\"FormatVersion\":7,\"FormatVersion\":1}", false, "Invalid")
        })
        {
            var result = method!.Invoke(null, new object?[] { example.Json })!;
            Assert.Equal(example.CanWrite, result.GetType().GetProperty("CanWrite")!.GetValue(result));
            Assert.Equal(example.Status, result.GetType().GetProperty("Status")!.GetValue(result)!.ToString());
            var state = (LEDState)result.GetType().GetProperty("State")!.GetValue(result)!;
            Assert.False(state.GetEffective("p").Enabled);
        }
    }
    [Fact]
    public void Debounce_reschedules_and_requires_successful_revision_acknowledgement()
    {
        var type = typeof(PersistenceTests).Assembly.GetType("PropertyLEDs.Core.SaveDebounce");
        Assert.NotNull(type);
        dynamic debounce = Activator.CreateInstance(type!, new object[] { 0.3 })!;
        Assert.False(debounce.ShouldSave(0.0, false));
        Assert.True(debounce.MarkDirty(1.0));
        Assert.False(debounce.ShouldSave(1.2, false));
        Assert.True(debounce.MarkDirty(1.2));
        Assert.False(debounce.ShouldSave(1.49, false));
        Assert.True(debounce.ShouldSave(1.5, false));
        long saving = debounce.Revision;
        Assert.True(debounce.MarkDirty(1.6));
        Assert.False(debounce.MarkSaved(saving));
        Assert.True(debounce.ShouldSave(1.6, true));
        Assert.True(debounce.MarkSaved((long)debounce.Revision));
        Assert.False(debounce.ShouldSave(5.0, true));
        Assert.False(debounce.MarkDirty(double.NaN));
        Assert.False(debounce.MarkDirty(double.PositiveInfinity));
        Assert.False(debounce.MarkDirty(-1.0));
        Assert.False(debounce.ShouldSave(double.NaN, true));
    }
    [Fact]
    public void Independent_slot_sessions_clone_loads_and_only_acknowledge_their_own_edits()
    {
        var type = typeof(PersistenceTests).Assembly.GetType("PropertyLEDs.Core.SlotSession");
        Assert.NotNull(type);
        var load = StateCodec.Read("{\"FormatVersion\":1}");
        dynamic a = Activator.CreateInstance(type!, new object[] { 1, load, 0.3 })!;
        dynamic b = Activator.CreateInstance(type!, new object[] { 2, load, 0.3 })!;
        Assert.True(a.SetGlobal("p", LEDSettings.White(), 1.0));
        Assert.True(((LEDState)a.Snapshot()).GetEffective("p").Enabled);
        Assert.False(((LEDState)b.Snapshot()).GetEffective("p").Enabled);
        Assert.False(load.State.GetEffective("p").Enabled);
        var snapshot = (LEDState)a.Snapshot(); snapshot.SetGlobal("p", new());
        Assert.True(((LEDState)a.Snapshot()).GetEffective("p").Enabled);
        Assert.True(a.ShouldSave(2.0, false));
        Assert.False(b.ShouldSave(2.0, true));
        Assert.True(a.MarkSaved((long)a.Revision));
        Assert.False(a.ShouldSave(3.0, true));
        Assert.False(a.SetGlobal("p", new LEDSettings(), double.NaN));
        Assert.True(((LEDState)a.Snapshot()).GetEffective("p").Enabled);
        foreach (var item in new[] { (-1, StateCodec.Read(null)), (3, StateCodec.Read("bad")), (4, StateCodec.Read("{\"FormatVersion\":999}")) })
        {
            dynamic blocked = Activator.CreateInstance(type!, new object[] { item.Item1, item.Item2, 0.3 })!;
            Assert.False(blocked.CanWrite);
            Assert.False(blocked.SetGlobal("p", LEDSettings.White(), 0.0));
            Assert.False(blocked.ShouldSave(10.0, true));
        }
    }
    [Fact]
    public void Rejected_backward_clock_never_mutates_session_state()
    {
        var session = new SlotSession(1, StateCodec.Read(null));
        Assert.True(session.SetGlobal("p", LEDSettings.White(), 10));
        Assert.False(session.SetGlobal("p", new LEDSettings(), 9));
        Assert.True(session.Snapshot().GetEffective("p").Enabled);
        Assert.Equal(1, session.Revision);
    }
    [Fact]
    public void Session_room_actions_follow_inheritance_and_do_not_dirty_rejected_actions()
    {
        var session = new SlotSession(3, StateCodec.Read(null));
        Assert.True(session.SetGlobal("p", new LEDSettings { Brightness = 2 }, 1));
        var room = typeof(SlotSession).GetMethod("SetRoom");
        var inherit = typeof(SlotSession).GetMethod("UseProperty");
        Assert.NotNull(room); Assert.NotNull(inherit);
        Assert.Equal(true, room!.Invoke(session, new object?[] { "p", "a", LEDSettings.White(), 2.0 }));
        Assert.True(session.Snapshot().GetEffective("p", "a").Enabled);
        Assert.Equal(true, inherit!.Invoke(session, new object?[] { "p", "a", 3.0 }));
        Assert.Equal(2, session.Snapshot().GetEffective("p", "a").Brightness);
        long revision = session.Revision;
        Assert.Equal(false, inherit.Invoke(session, new object?[] { "p", "a", 4.0 }));
        Assert.Equal(false, room.Invoke(session, new object?[] { "p", "a", LEDSettings.White(), 1.0 }));
        Assert.Equal(revision, session.Revision);
    }
    [Fact]
    public void Valid_state_roundtrips_and_known_null_graphs_normalize_without_slot_crosstalk()
    {
        var state = new LEDState();
        state.SetGlobal("property:1", new LEDSettings { Enabled = true, Hue = 0.75f, Saturation = 0.8f, ColorIntensity = 0.4f, Brightness = 4 });
        state.SetRoom("property:1", "room:2", new LEDSettings { Brightness = 2 });
        var json = System.Text.Json.JsonSerializer.Serialize(state);
        var result = StateCodec.Read(json);
        Assert.True(result.CanWrite);
        Assert.Equal(StateLoadStatus.Loaded, result.Status);
        Assert.Equal(json, System.Text.Json.JsonSerializer.Serialize(result.State));
        Assert.Equal(json, result.OriginalJson);
        var nulls = StateCodec.Read("{\"FormatVersion\":1,\"Properties\":{\"p\":{\"Global\":null,\"Rooms\":{\"r\":null}},\"bad\":null}}");
        Assert.True(nulls.CanWrite);
        Assert.Empty(nulls.State.Properties["p"].Rooms);
        Assert.False(nulls.State.GetEffective("p", "r").Enabled);
        Assert.False(StateCodec.Read("{\"FormatVersion\":\"1\"}").CanWrite);
        Assert.False(StateCodec.Read("{\"FormatVersion\":1,\"Properties\":{\"p\":{\"Global\":{\"Hue\":\"NaN\"}}}}").CanWrite);
    }

    [Fact]
    public void Debounce_clock_rejections_and_failed_writes_preserve_pending_changes()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new SaveDebounce(double.NaN));
        Assert.Throws<ArgumentOutOfRangeException>(() => new SaveDebounce(-1));
        var save = new SaveDebounce(0);
        Assert.True(save.MarkDirty(10));
        Assert.False(save.MarkDirty(9));
        Assert.True(save.ShouldSave(10));
        Assert.False(save.MarkSaved(0));
        Assert.True(save.ShouldSave(11));
        Assert.True(save.MarkSaved(1));
        Assert.False(save.MarkSaved(1));
    }
    // NEXT TEST
}
