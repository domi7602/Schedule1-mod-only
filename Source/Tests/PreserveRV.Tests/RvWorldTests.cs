using Il2CppScheduleOne.Quests;
using Xunit;

namespace PreserveRV.Tests;

public class RvWorldTests
{
    internal static Quest_WelcomeToHylandPoint Fixture()
    {
        var quest = new Quest_WelcomeToHylandPoint();
        quest.RV.transform.Add("RV");
        var wreck = quest.RV.transform.Add("Destroyed RV", false);
        wreck.Add("CartelNote", false);
        wreck.Add("FX", false);
        quest.Entries.Add(new() { EntryTitle = "Investigate the explosion" });
        quest.Entries.Add(new() { EntryTitle = "Read the note" });
        return quest;
    }

    [Fact]
    public void Repeated_loads_reparent_once_and_never_toggle_visibility()
    {
        var quest = Fixture();
        var world = new RvWorld();
        var note = quest.RV.transform.Children[1].Children[0];
        Assert.True(world.Prepare(quest));
        Assert.True(world.Prepare(quest));
        Assert.True(world.Prepare(quest));
        Assert.Equal(1, note.ParentWrites);
        Assert.Equal(0, note.gameObject.ActiveWrites);
    }

    [Fact]
    public void Native_activation_of_the_note_is_preserved_not_overridden()
    {
        var quest = Fixture();
        var note = quest.RV.transform.Children[1].Children[0];
        note.gameObject.SetActive(true);                // e.g. Investigate.onStart already ran

        Assert.True(new RvWorld().Prepare(quest));
        Assert.True(note.gameObject.activeSelf);        // the mod must not hide a native-visible note.
        Assert.Equal(1, note.gameObject.ActiveWrites);  // only the native call wrote.
    }

    [Fact]
    public void Ambiguous_root_note_is_refused_without_moving_anything()
    {
        var quest = Fixture();
        quest.RV.transform.Add("CartelNote", false);    // second note at the root: ambiguous
        var nested = quest.RV.transform.Children[1].Children[0];

        Assert.False(new RvWorld().Prepare(quest));
        Assert.Equal(0, nested.ParentWrites);
        Assert.Same(quest.RV.transform.Children[1], nested.parent);
    }

    [Fact]
    public void Preparation_survives_the_wreck_disappearing_afterwards()
    {
        var quest = Fixture();
        var root = quest.RV.transform;
        var note = root.Children[1].Children[0];
        var world = new RvWorld();
        Assert.True(world.Prepare(quest));

        // Native code may tear the wreck down once it is no longer needed.
        root.Children.Remove(root.Children[1]);
        Assert.True(world.Prepare(quest));   // still prepared, not a new failure
        Assert.Same(root, note.parent);
        Assert.Equal(1, note.ParentWrites);
    }

    [Fact]
    public void Missing_wreck_or_note_reports_failure_instead_of_claiming_preparation()
    {
        var noWreck = Fixture();
        noWreck.RV.transform.Children[1].name = "something else";
        Assert.False(new RvWorld().Prepare(noWreck));

        var noNote = Fixture();
        noNote.RV.transform.Children[1].Children[0].name = "some other pickable";
        Assert.False(new RvWorld().Prepare(noNote));

        var dead = Fixture();
        dead.RV.transform.Children[1].Alive = false;
        Assert.False(new RvWorld().Prepare(dead));
    }
}
