using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LightControl.Core;

namespace LightControl.Core.Tests;

/// <summary>
/// LC08-LC10 phone-UI checks. Every case is either a pure decision of <see cref="UiSelectionPolicy"/>
/// or a source-wiring assertion. There is deliberately no fake Unity visual test here: it would prove
/// only the fake. Actual PhoneApp measurement and in-game fit remain OPEN in ui-repair-evidence.md.
/// </summary>
internal static class UiRepairChecks
{
    public static IEnumerable<(string Name, Action Run)> Cases => new (string, Action)[]
    {
        ("UI01 Temporary unload keeps the selected key, including an empty list", TemporarySelectionKept),
        ("UI02 Only a completed scan may treat an unknown key as permanent removal", PermanentRemovalGate),
        ("UI03 A filtered or unloaded selection keeps state but is not editable", FilteredSelectionNotEditable),
        ("UI04 Original, fixed color and rainbow are the only three modes", ModeClassification),
        ("UI05 Fixed mode remembers the last solid color and falls back safely", FixedColorMemory),
        ("UI06 Copy labels and the global confirmation name the source and exact count", CopyWording),
        ("UI07 An unchanged action never claims that lamps changed", ActionResultWording),
        ("UI08 Status keeps the actual reason, scan state and persistence even when empty", StatusComposition),
        ("UI09 The legacy compact-layout policy keeps its bounded width decision", CompactDecision),
        ("UI10 Phone app wires LC08 selection, LC09 availability state and LC10 status", AppWiring),
        ("UI11 Phone app exposes ten palette choices and seven immediate brightness steps", ControlWiring),
        ("UI12 Phone app keeps the clipping fix, stable labels, copy confirmation and cleanup", PreservedWiring)
    };

    public static int Run()
    {
        int failures = 0;
        foreach (var (name, run) in Cases)
        {
            try { run(); Console.WriteLine($"PASS {name}"); }
            catch (Exception exception)
            {
                failures++;
                Console.WriteLine($"FAIL {name}: {exception.GetType().Name}: {exception.Message}");
            }
        }
        return failures;
    }

    private static void Equal<T>(T expected, T actual)
    {
        if (!EqualityComparer<T>.Default.Equals(expected, actual))
            throw new InvalidOperationException($"Expected [{expected}]; actual [{actual}].");
    }

    private static void True(bool condition, string message)
    {
        if (!condition) throw new InvalidOperationException(message);
    }

    private static void Throws<T>(Action action) where T : Exception
    {
        try { action(); }
        catch (T) { return; }
        throw new InvalidOperationException($"Expected {typeof(T).Name}.");
    }

    private static void TemporarySelectionKept()
    {
        var empty = Array.Empty<string>();
        Equal("b", UiSelectionPolicy.ResolveSelection("b", empty, false));
        Equal("b", UiSelectionPolicy.ResolveSelection("b", new[] { "a", "c" }, false));
        Equal(null, UiSelectionPolicy.ResolveSelection("b", empty, true));
        Equal(null, UiSelectionPolicy.ResolveSelection(null, empty, false));
        Equal("a", UiSelectionPolicy.ResolveSelection(null, new[] { "a" }, false));
    }

    private static void PermanentRemovalGate()
    {
        True(!UiSelectionPolicy.IsPermanentlyRemoved("b", known: true, scanInProgress: false), "A known, temporarily unloaded lamp was treated as removed.");
        True(!UiSelectionPolicy.IsPermanentlyRemoved("b", known: false, scanInProgress: true), "An unknown key was removed while the scan was still running.");
        True(UiSelectionPolicy.IsPermanentlyRemoved("b", known: false, scanInProgress: false), "A completed scan that no longer knows the key must resolve the selection.");
        True(!UiSelectionPolicy.IsPermanentlyRemoved(null, known: false, scanInProgress: false), "No selection cannot be a removal.");
        True(!UiSelectionPolicy.IsPermanentlyRemoved("", known: false, scanInProgress: false), "An empty key cannot be a removal.");
    }

    private static void FilteredSelectionNotEditable()
    {
        Equal(UiSelectionPolicy.EditorAvailability.None, UiSelectionPolicy.ClassifyEditor(null, known: true, visible: true));
        Equal(UiSelectionPolicy.EditorAvailability.None, UiSelectionPolicy.ClassifyEditor("", known: true, visible: true));
        Equal(UiSelectionPolicy.EditorAvailability.Editable, UiSelectionPolicy.ClassifyEditor("b", known: true, visible: true));
        Equal(UiSelectionPolicy.EditorAvailability.Unavailable, UiSelectionPolicy.ClassifyEditor("b", known: true, visible: false));
        Equal(UiSelectionPolicy.EditorAvailability.Unavailable, UiSelectionPolicy.ClassifyEditor("b", known: false, visible: false));
        True(UiSelectionPolicy.CanEditSelection("b", known: true, visible: true), "A visible known lamp must be editable.");
        True(!UiSelectionPolicy.CanEditSelection("b", known: true, visible: false), "A filtered-out lamp must not be editable.");
        True(!UiSelectionPolicy.CanEditSelection("b", known: false, visible: true), "An unknown lamp must not be editable.");
        True(!UiSelectionPolicy.CanEditSelection(null, known: true, visible: true), "No selection must not be editable.");
    }

    private static void ModeClassification()
    {
        Equal(UiSelectionPolicy.LightMode.Original, UiSelectionPolicy.ModeOf(ColorMode.Original));
        Equal(UiSelectionPolicy.LightMode.Rainbow, UiSelectionPolicy.ModeOf(ColorMode.Rainbow));
        foreach (ColorMode fixedColor in new[]
                 { ColorMode.White, ColorMode.Red, ColorMode.Orange, ColorMode.Yellow, ColorMode.Green,
                   ColorMode.Cyan, ColorMode.Blue, ColorMode.Purple, ColorMode.Pink })
        {
            Equal(UiSelectionPolicy.LightMode.Fixed, UiSelectionPolicy.ModeOf(fixedColor));
            True(UiSelectionPolicy.IsFixedColor(fixedColor), $"{fixedColor} is a fixed color.");
        }
        True(!UiSelectionPolicy.IsFixedColor(ColorMode.Original), "Original is not a fixed color.");
        True(!UiSelectionPolicy.IsFixedColor(ColorMode.Rainbow), "Rainbow is not a fixed color.");
        Throws<ArgumentOutOfRangeException>(() => UiSelectionPolicy.ModeOf((ColorMode)99));
        Equal("Original", UiSelectionPolicy.ColorName(ColorMode.Original));
        Equal("Rainbow", UiSelectionPolicy.ColorName(ColorMode.Rainbow));
        Equal("10%", UiSelectionPolicy.IntensityLabel(IntensityLevel.Percent10));
        Equal("200%", UiSelectionPolicy.IntensityLabel(IntensityLevel.Percent200));
    }

    private static void FixedColorMemory()
    {
        Equal(ColorMode.Blue, UiSelectionPolicy.FixedColor(ColorMode.Blue, ColorMode.Red));
        Equal(ColorMode.Red, UiSelectionPolicy.FixedColor(ColorMode.Original, ColorMode.Red));
        Equal(ColorMode.Red, UiSelectionPolicy.FixedColor(ColorMode.Rainbow, ColorMode.Red));
        Equal(ColorMode.White, UiSelectionPolicy.FixedColor(ColorMode.Original, ColorMode.Original));
        Equal(ColorMode.White, UiSelectionPolicy.FixedColor(ColorMode.Rainbow, ColorMode.Rainbow));
    }

    private static void CopyWording()
    {
        string property = UiSelectionPolicy.PropertyCopyLabel("Motel Room", 4);
        True(property.Contains("Motel Room", StringComparison.Ordinal), "Property copy label lost the property name.");
        True(property.Contains("4", StringComparison.Ordinal), "Property copy label lost the exact target count.");
        True(property.Contains("source", StringComparison.OrdinalIgnoreCase), "Property copy label must say the count includes the source.");

        string confirmation = UiSelectionPolicy.WorldCopyConfirmation("Grow lamp 1", "Motel Room", ColorMode.Red, IntensityLevel.Percent100, 3);
        True(confirmation.Contains("Grow lamp 1", StringComparison.Ordinal), "Global confirmation lost the source lamp.");
        True(confirmation.Contains("Motel Room", StringComparison.Ordinal), "Global confirmation lost the source property.");
        True(confirmation.Contains("Red", StringComparison.Ordinal), "Global confirmation lost the source color.");
        True(confirmation.Contains("100%", StringComparison.Ordinal), "Global confirmation lost the source brightness.");
        True(confirmation.Contains("3", StringComparison.Ordinal), "Global confirmation lost the exact target count.");
        True(confirmation.Contains("including the source", StringComparison.OrdinalIgnoreCase), "Global confirmation must state that the source is included.");
    }

    private static void ActionResultWording()
    {
        string applied = UiSelectionPolicy.ActionMessage(CommandStatus.Applied, 2);
        True(applied.Contains("2", StringComparison.Ordinal), "Applied message lost the changed count.");
        string none = UiSelectionPolicy.ActionMessage(CommandStatus.Applied, 0);
        True(!none.Contains("Applied", StringComparison.OrdinalIgnoreCase), "Zero changed lamps were reported as applied.");
        True(none.Contains("No lamps changed", StringComparison.Ordinal), "Zero changed lamps need an explicit no-change message.");
        string unchanged = UiSelectionPolicy.ActionMessage(CommandStatus.Unchanged, 0);
        True(unchanged.Contains("Already set", StringComparison.Ordinal), "Unchanged result lost its wording.");
        True(!unchanged.Contains("Applied", StringComparison.OrdinalIgnoreCase), "Unchanged result claimed an apply.");
        string failed = UiSelectionPolicy.ActionMessage(CommandStatus.StaleRevision, 0);
        True(failed.Contains("not applied", StringComparison.Ordinal), "Rejected result claimed success.");
    }

    private static void StatusComposition()
    {
        string waiting = UiSelectionPolicy.StatusLine(false, "No active save slot yet.", "Saved.", false, false, 0, 0);
        True(waiting.Contains("No active save slot yet.", StringComparison.Ordinal), "The actual session reason was replaced.");
        True(!waiting.Contains("Saved.", StringComparison.Ordinal), "A persistence notice was shown before a session existed.");

        string empty = UiSelectionPolicy.StatusLine(true, "Host session ready.", "Saved.", false, false, 0, 0);
        True(empty.Contains("No supported grow lamps found.", StringComparison.Ordinal), "The empty list lost its reason.");
        True(empty.Contains("Saved.", StringComparison.Ordinal), "The empty list lost its persistence state.");
        True(!empty.Contains("Host session ready.", StringComparison.Ordinal), "The generic ready line crowded out scan and persistence.");

        string scanning = UiSelectionPolicy.StatusLine(true, "Host session ready.", "Save pending.", true, false, 0, 0);
        True(scanning.Contains("Scanning", StringComparison.Ordinal), "An active scan was not reported.");
        True(scanning.Contains("Save pending.", StringComparison.Ordinal), "The scan message replaced the persistence state.");

        string truncated = UiSelectionPolicy.StatusLine(true, "Host session ready.", "Saved.", false, true, 2, 1);
        True(truncated.Contains("incomplete", StringComparison.OrdinalIgnoreCase), "A truncated scan was reported as complete.");
        True(truncated.Contains("1", StringComparison.Ordinal), "Unsupported lamp count was dropped.");
        True(truncated.Contains("Saved.", StringComparison.Ordinal), "Persistence was dropped from a truncated scan line.");
    }

    private static void CompactDecision()
    {
        True(UiSelectionPolicy.UseCompactLayout(320f), "A narrow canvas must use the compact pane.");
        True(!UiSelectionPolicy.UseCompactLayout(400f), "A normal phone canvas must keep list and editor side by side.");
        True(!UiSelectionPolicy.UseCompactLayout(0f), "An unmeasured canvas is not compact.");
        True(!UiSelectionPolicy.UseCompactLayout(float.NaN), "A nonfinite canvas width is not compact.");
    }

    private static void AppWiring()
    {
        string app = ReadSource("LightControlApp.cs");
        True(app.Contains("LightRuntime.IsKnownLamp", StringComparison.Ordinal), "Selection does not consult the known-lamp set.");
        True(app.Contains("UiSelectionPolicy.IsPermanentlyRemoved", StringComparison.Ordinal), "Selection does not gate permanent removal.");
        True(app.Contains("UiSelectionPolicy.ClassifyEditor", StringComparison.Ordinal), "The modal does not use the shared availability decision.");
        True(app.Contains("LightRuntime.ScanInProgress", StringComparison.Ordinal), "The status omits the live scan state.");
        True(app.Contains("LightRuntime.ScanTruncated", StringComparison.Ordinal), "The status omits scan truncation.");
        True(app.Contains("LightRuntime.StatusMessage", StringComparison.Ordinal), "The status omits the runtime reason.");
        True(app.Contains("UiSelectionPolicy.StatusLine", StringComparison.Ordinal), "The status is not composed by the tested policy.");
        True(app.Contains("interactable", StringComparison.Ordinal), "The modal controls are never disabled.");
        True(app.Contains("SetModalEnabled", StringComparison.Ordinal), "No single modal enable/disable path exists.");
        True(app.Contains("Reserve", StringComparison.Ordinal) || app.Contains("reserved", StringComparison.Ordinal), "The message area is not marked as reserved.");
    }

    private static void ControlWiring()
    {
        string app = ReadSource("LightControlApp.cs");
        // Original and Rainbow stay reachable in the separate actions panel, which also marks the
        // active mode, so the mode decision is still painted outside the color-only modal.
        True(app.Contains("UiSelectionPolicy.ModeOf", StringComparison.Ordinal), "No active-mode marker is painted.");
        True(app.Contains("ColorMode.Original", StringComparison.Ordinal), "The Original action is missing.");
        True(app.Contains("ColorMode.Rainbow", StringComparison.Ordinal), "The Rainbow action is missing.");
        True(app.Contains("ColorMode.White", StringComparison.Ordinal) && app.Contains("ColorMode.Pink", StringComparison.Ordinal),
            "The nine real colors are not bounded White..Pink.");
        True(app.Contains("UiSelectionPolicy.PickerColorCount", StringComparison.Ordinal)
             && app.Contains("UiSelectionPolicy.PickerColorAt(i)", StringComparison.Ordinal),
            "The palette does not consume the approved ten-choice reading order.");
        True(app.Contains("UiSelectionPolicy.IntensityCount", StringComparison.Ordinal), "Brightness is not built from the seven-step source of truth.");
        True(app.Contains("UiSelectionPolicy.IntensityLabel", StringComparison.Ordinal), "Brightness labels are not the readable percent labels.");
        True(app.Contains("lamp.Label", StringComparison.Ordinal), "Rows do not use the stable runtime label.");
        True(app.Contains("UiSelectionPolicy.ReferenceCanvasScale", StringComparison.Ordinal), "The owned reference-canvas fit is not wired.");
        True(app.Contains("UiSelectionPolicy.BrightnessFill", StringComparison.Ordinal), "The bar does not consume the tested 200-percent scale.");
    }

    private static void PreservedWiring()
    {
        string app = ReadSource("LightControlApp.cs");
        True(app.Contains("PinListWidth", StringComparison.Ordinal), "The confirmed list-width clipping fix was removed.");
        True(app.Contains("fitter.horizontalFit", StringComparison.Ordinal), "The list content fitter fix was removed.");
        True(app.Contains("EventHelper.RemoveListener(handler, unityEvent)", StringComparison.Ordinal), "Exact UnityEvent cleanup was changed.");
        True(app.Contains("MeasureLayout", StringComparison.Ordinal), "Layout is not measured once per size change.");
        True(app.Contains("UiSelectionPolicy.PropertyCopyLabel", StringComparison.Ordinal), "Property copy text no longer uses the counted label.");
        True(app.Contains("UiSelectionPolicy.WorldCopyConfirmation", StringComparison.Ordinal), "Global copy has no explicit source confirmation.");
        True(app.Contains("UiSelectionPolicy.ActionMessage", StringComparison.Ordinal), "Action results are not the tested wording.");
        // All bounded modal contents fit together on a uniformly mapped reference canvas. The
        // unbounded pooled table still scrolls; this source check is not a native fit assertion.
        True(app.Contains("NewCard(\"ColorCard\", 600f, 456f", StringComparison.Ordinal), "The approved color-card reference bounds are missing.");
        True(app.Contains("UIFactory.ScrollableVerticalList(", StringComparison.Ordinal), "The lamp table no longer scrolls.");
        True(app.Contains("_referenceCanvas.localScale = new Vector3(scale, scale, 1f)", StringComparison.Ordinal),
            "The measured container does not map the entire owned reference composition.");
        True(app.Contains("CanvasGroup", StringComparison.Ordinal), "The list has no single blocked visual state while a modal is open.");
        True(app.Contains("MelonEvents.OnUpdate.Unsubscribe(Update)", StringComparison.Ordinal), "Final teardown lost update cleanup.");
        True(app.Contains("_ownedAssets.Clear()", StringComparison.Ordinal), "The view lost its owned-resource cleanup.");
    }

    private static string ReadSource(string name)
    {
        string? configured = Environment.GetEnvironmentVariable("LIGHTCONTROL_SRC");
        if (!string.IsNullOrWhiteSpace(configured))
        {
            string direct = Path.Combine(configured!, name);
            if (File.Exists(direct)) return File.ReadAllText(direct);
        }
        for (DirectoryInfo? dir = new DirectoryInfo(AppContext.BaseDirectory); dir != null; dir = dir.Parent)
        {
            string path = Path.Combine(dir.FullName, "Source", "Mods", "LightControl", "src", name);
            if (File.Exists(path)) return File.ReadAllText(path);
        }
        throw new FileNotFoundException("LightControl source root not found", name);
    }
}
