using LightControl.Core;

namespace LightControl.Core.Tests;

internal static class GraphiteUiChecks
{
    private static readonly Guid Session = Guid.Parse("6aef78e1-e911-424b-9d3b-cf6a42e95dcc");

    public static IEnumerable<(string Name, Action Run)> Cases => new (string, Action)[]
    {
        ("G01 Rainbow is staged and committed once to the captured lamp", RainbowCommit),
        ("G02 Reopening Rainbow selects Rainbow rather than a fixed color", RainbowReopen),
        ("G03 A stale Rainbow pick requires a fresh choice", StaleRainbow),
        ("G04 Original stays unselected and cancelled Rainbow cannot commit", OriginalCancel),
        ("G05 Rainbow obeys the existing session and availability gates", RainbowGates),
        ("G06 The reference canvas fits wide, short and small hosts without overflow", ReferenceFit),
        ("G07 The palette exposes nine fixed colors followed by Rainbow", PaletteOrder),
        ("G08 Brightness bars show the documented 200-percent scale", BrightnessMeter),
        ("G09 Equal actions remain distinct subscriptions at S1API's delegate-keyed boundary", DistinctClickBindings),
        ("G10 Released click bindings reject late callbacks independently", ReleasedClickBindings),
        ("G11 The column heading follows its initial position then stays visible while scrolling", StickyHeading)
    };

    private static void Require(bool value, string message)
    {
        if (!value) throw new InvalidOperationException(message);
    }

    private static UiSelectionPolicy.ColorPickerState Open(ColorMode color)
    {
        var picker = new UiSelectionPolicy.ColorPickerState();
        Require(picker.Open(Session, "lamp-a", color), "Available source did not open.");
        return picker;
    }

    private static void RainbowCommit()
    {
        var picker = Open(ColorMode.Blue);
        Require(picker.Choose(ColorMode.Rainbow), "Rainbow was not accepted in the tenth slot.");
        Require(picker.SourceColor == ColorMode.Blue && picker.PendingColor == ColorMode.Rainbow,
            "Picking Rainbow mutated the source instead of staging it.");
        Require(picker.Confirm(Session, "lamp-a", true, true, ColorMode.Blue, out var commit)
            == UiSelectionPolicy.PickerOutcome.Committed, "Apply rejected Rainbow.");
        Require(commit.Color == ColorMode.Rainbow && commit.SourceKey == "lamp-a" && commit.SessionId == Session,
            "Apply lost the Rainbow mode or exact source identity.");
        Require(picker.Confirm(Session, "lamp-a", true, true, ColorMode.Blue, out _)
            == UiSelectionPolicy.PickerOutcome.AlreadyConsumed, "Repeated Apply was not consumed.");
    }

    private static void RainbowReopen()
    {
        var picker = Open(ColorMode.Rainbow);
        Require(picker.HasPendingColor && picker.PendingColor == ColorMode.Rainbow,
            "An existing Rainbow lamp did not highlight Rainbow.");
        Require(!picker.Choose(ColorMode.Original) && !picker.Choose((ColorMode)99),
            "The picker accepted an unsupported mode.");
        picker.Cancel();
        Require(picker.Open(Session, "lamp-b", ColorMode.Pink) && picker.PendingColor == ColorMode.Pink,
            "A different lamp inherited the previous Rainbow selection.");
    }

    private static void StaleRainbow()
    {
        var picker = Open(ColorMode.Blue);
        Require(picker.Choose(ColorMode.Rainbow), "Rainbow was rejected.");
        Require(picker.Confirm(Session, "lamp-a", true, true, ColorMode.Red, out _)
            == UiSelectionPolicy.PickerOutcome.StaleSource, "A stale choice overwrote the current color.");
        Require(!picker.HasPendingColor && picker.Stale, "Stale selection was not visibly invalidated.");
        Require(picker.Choose(ColorMode.Rainbow), "A fresh Rainbow selection was rejected.");
        Require(picker.Confirm(Session, "lamp-a", true, true, ColorMode.Red, out var commit)
            == UiSelectionPolicy.PickerOutcome.Committed && commit.Color == ColorMode.Rainbow,
            "A refreshed choice could not be applied.");
    }

    private static void OriginalCancel()
    {
        var picker = Open(ColorMode.Original);
        Require(!picker.HasPendingColor, "Original silently preselected a picker mode.");
        Require(picker.Choose(ColorMode.Rainbow), "Rainbow was rejected from Original.");
        picker.Cancel();
        Require(picker.Confirm(Session, "lamp-a", true, true, ColorMode.Original, out _)
            == UiSelectionPolicy.PickerOutcome.NotOpen, "Cancel left a live Rainbow commit.");
        Require(!picker.Choose(ColorMode.Rainbow), "A late callback reopened a cancelled picker.");
    }

    private static void RainbowGates()
    {
        var picker = Open(ColorMode.Blue);
        Require(picker.Choose(ColorMode.Rainbow), "Rainbow was rejected.");
        Require(picker.Confirm(Guid.NewGuid(), "lamp-a", true, true, ColorMode.Blue, out _)
            == UiSelectionPolicy.PickerOutcome.WrongSession, "Rainbow bypassed the session gate.");
        Require(picker.Confirm(Session, "lamp-b", true, true, ColorMode.Blue, out _)
            == UiSelectionPolicy.PickerOutcome.WrongSource, "Rainbow retargeted another lamp.");
        Require(picker.Confirm(Session, "lamp-a", true, false, ColorMode.Blue, out _)
            == UiSelectionPolicy.PickerOutcome.Unavailable, "Rainbow applied to a hidden source.");
        Require(picker.Confirm(Session, "lamp-a", true, true, ColorMode.Blue, out _)
            == UiSelectionPolicy.PickerOutcome.Committed, "Rejected attempts consumed the valid choice.");
    }

    private static void ReferenceFit()
    {
        Require(UiSelectionPolicy.ReferenceCanvasScale(1201f, 655f) == 1f, "The accepted reference no longer maps 1:1.");
        foreach (var (width, height) in new[] { (800f, 600f), (1600f, 400f), (320f, 180f), (2402f, 1310f) })
        {
            float scale = UiSelectionPolicy.ReferenceCanvasScale(width, height);
            float fittedWidth = UiSelectionPolicy.GraphiteReferenceWidth * scale;
            float fittedHeight = UiSelectionPolicy.GraphiteReferenceHeight * scale;
            Require(scale > 0 && fittedWidth <= width + 0.01f && fittedHeight <= height + 0.01f,
                "The owned reference canvas extends beyond its measured host.");
            Require(Math.Abs(fittedWidth - width) < 0.01f || Math.Abs(fittedHeight - height) < 0.01f,
                "The reference canvas leaves unused space on both axes.");
        }
        foreach (var (width, height) in new[] { (0f, 655f), (-1f, 655f), (1201f, 0f), (float.NaN, 655f), (1201f, float.PositiveInfinity) })
            Require(UiSelectionPolicy.ReferenceCanvasScale(width, height) == 0f, "An invalid/unmeasured rect produced a live scale.");
    }

    private static void PaletteOrder()
    {
        var choices = Enumerable.Range(0, UiSelectionPolicy.PickerColorCount).Select(UiSelectionPolicy.PickerColorAt).ToArray();
        Require(choices.Length == 10 && choices.Distinct().Count() == 10, "The five-by-two palette has missing or duplicate entries.");
        Require(choices[0] == ColorMode.White && choices[4] == ColorMode.Green && choices[9] == ColorMode.Rainbow,
            "The accepted palette reading order changed.");
        Require(choices.Take(9).All(UiSelectionPolicy.IsFixedColor) && choices.All(UiSelectionPolicy.IsPickerColor),
            "A non-picker mode replaced a fixed color.");
        Require(!UiSelectionPolicy.IsPickerColor(ColorMode.Original) && !UiSelectionPolicy.IsPickerColor((ColorMode)99),
            "An invalid/Original mode became a selectable tile.");
    }

    private static void BrightnessMeter()
    {
        Require(Math.Abs(UiSelectionPolicy.BrightnessFill(IntensityLevel.Percent10) - 0.05f) < 0.0001f,
            "The minimum step does not use the 200-percent scale.");
        Require(UiSelectionPolicy.BrightnessFill(IntensityLevel.Percent100) == 0.5f
            && UiSelectionPolicy.BrightnessFill(IntensityLevel.Percent200) == 1f,
            "The meter confuses 100 percent baseline with the 200 percent maximum.");
    }

    private static void DistinctClickBindings()
    {
        int clicks = 0;
        Action sameAction = () => clicks++;
        var first = new UiSelectionPolicy.ClickBinding(sameAction);
        var second = new UiSelectionPolicy.ClickBinding(sameAction);
        var subscribed = new HashSet<Action>();
        Require(subscribed.Add(first.Callback) && subscribed.Add(second.Callback),
            "S1API's delegate-keyed subscription map would discard the second footer button.");
        first.Callback();
        second.Callback();
        Require(clicks == 2, "One of the independent buttons did not forward its click.");
    }

    private static void ReleasedClickBindings()
    {
        int clicks = 0;
        Action sameAction = () => clicks++;
        var first = new UiSelectionPolicy.ClickBinding(sameAction);
        var second = new UiSelectionPolicy.ClickBinding(sameAction);
        Action lateCallback = first.Callback;
        first.Release();
        first.Release();
        lateCallback();
        Require(clicks == 0, "A callback retained by a native event acted after release.");
        second.Callback();
        Require(clicks == 1, "Releasing one widget disabled another widget's action.");
        second.Release();
        second.Callback();
        Require(clicks == 1, "The second release left an active target.");
    }

    private static void StickyHeading()
    {
        Require(UiSelectionPolicy.ColumnHeadingTop(0f) == 52f
            && UiSelectionPolicy.ColumnHeadingTop(20f) == 32f,
            "The heading covers the initial property band instead of following its list position.");
        Require(UiSelectionPolicy.ColumnHeadingTop(52f) == 0f
            && UiSelectionPolicy.ColumnHeadingTop(500f) == 0f,
            "The heading scrolls out of view with the lamp rows.");
        Require(UiSelectionPolicy.ColumnHeadingTop(-20f) == 52f
            && UiSelectionPolicy.ColumnHeadingTop(float.NaN) == 52f,
            "Unmeasured/negative scrolling displaces the reference heading.");
    }
}
