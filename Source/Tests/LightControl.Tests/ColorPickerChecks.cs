using System;
using System.Collections.Generic;
using System.IO;
using LightControl.Core;

namespace LightControl.Core.Tests;

/// <summary>
/// Graphite color picker. The staged edit state is Unity-free and is exercised directly: open,
/// stage, cancel, confirm, one-shot consumption and every fail-closed gate. The action gate and the
/// legacy card-width clamp are covered behaviorally. The app now uses uniform reference fitting. The
/// source contracts at the end only prove that the real PhoneApp drives that state, that every
/// scroll body is width-pinned and that the overlay routes outside clicks to the dismiss button.
/// They are not native hit-testing, rendered layout, PhoneApp measurement or native fit.
/// </summary>
internal static class ColorPickerChecks
{
    private static readonly Guid SessionA = Guid.Parse("0b8e4c1e-6a53-4b2e-9a3f-4f0d6a5c7e11");
    private static readonly Guid SessionB = Guid.Parse("5d1f0f77-2c2f-4a1e-8a44-9e1b3c2d4f55");

    public static IEnumerable<(string Name, Action Run)> Cases => new (string, Action)[]
    {
        ("CP01 Opening stages the source color and SELECT commits it once", StagedPickCommitsOnce),
        ("CP02 Original stays unselected and Rainbow stages itself without becoming White", NonFixedSourceNotMutated),
        ("CP03 BACK discards the staged pick without applying", CancelDiscards),
        ("CP04 A late confirmation fails closed on a different session", WrongSessionRejected),
        ("CP05 A different source key cannot retarget the open picker", WrongSourceRejected),
        ("CP06 An unloaded or unknown source cannot be confirmed", UnavailableRejected),
        ("CP07 A source changed while pending is rejected and requires a re-pick", StaleSourceRejected),
        ("CP08 Nine fixed colors and Rainbow can be staged; Original and invalid values cannot", PickerColors),
        ("CP09 Two lamps keep independent staged picks", DistinctLamps),
        ("CP10 Closing the phone or switching sessions drops the pending pick", ClosureClearsPending),
        ("CP11 Every documented brightness step still applies immediately", BrightnessSteps),
        ("CP12 The phone app wires the immediate brightness path and keeps other actions reachable", AppWiring),
        ("CP13 A modal action is bound to its captured session and exact visible lamp", ActionBoundToSession),
        ("CP14 A brightness step applies on click and never after another card opens", ImmediateBrightnessWiring),
        ("CP15 The legacy modal-width policy retains its bounded behavior", CardWidthClamped),
        ("CP16 Every scrollable body is width-pinned so the card never clips 50 units", EveryScrollListPinned),
        ("CP17 An outside click dismisses while a card click is consumed by the card", OutsideClickRouting),
        ("CP18 The app gates every modal action and carries the session into the commit", ActionGateWiring),
        ("CP19 Lamp rows use small square swatches and consistent aligned columns", ProfessionalRowWiring),
        ("CP20 Modal footer buttons reserve explicit rendered height", FooterButtonHeights)
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

    private static void StagedPickCommitsOnce()
    {
        var picker = new UiSelectionPolicy.ColorPickerState();
        True(picker.Open(SessionA, "lamp-a", ColorMode.Blue), "The picker refused an available lamp.");
        Equal(true, picker.IsOpen);
        Equal(SessionA, picker.SessionId);
        Equal("lamp-a", picker.SourceKey);
        // Opening stages the current color for display; it never applies anything.
        Equal(ColorMode.Blue, picker.PendingColor);
        Equal(true, picker.HasPendingColor);
        Equal(false, picker.Consumed);

        True(picker.Choose(ColorMode.Red), "A real emitted color was rejected as a pending pick.");
        Equal(ColorMode.Red, picker.PendingColor);

        var outcome = picker.Confirm(SessionA, "lamp-a", known: true, visible: true, sourceColor: ColorMode.Blue, out var commit);
        Equal(UiSelectionPolicy.PickerOutcome.Committed, outcome);
        Equal(ColorMode.Red, commit.Color);
        Equal("lamp-a", commit.SourceKey);
        Equal(SessionA, commit.SessionId);
        Equal(false, picker.IsOpen);
        Equal(true, picker.Consumed);
        Equal(false, picker.HasPendingColor);

        // The claim is consumed before the runtime call, so a repeated click cannot apply twice.
        Equal(UiSelectionPolicy.PickerOutcome.AlreadyConsumed,
            picker.Confirm(SessionA, "lamp-a", known: true, visible: true, sourceColor: ColorMode.Blue, out var second));
        Equal(default, second);
    }

    private static void NonFixedSourceNotMutated()
    {
        var picker = new UiSelectionPolicy.ColorPickerState();
        True(picker.Open(SessionA, "lamp-a", ColorMode.Original), "The picker refused Original.");
        Equal(false, picker.HasPendingColor);
        True(picker.PendingColor != ColorMode.White, "Opening Original silently turned it into White.");
        Equal(UiSelectionPolicy.PickerOutcome.NoPick,
            picker.Confirm(SessionA, "lamp-a", true, true, ColorMode.Original, out var commit));
        Equal(default, commit);
        Equal(true, picker.IsOpen);
        Equal(false, picker.Consumed);
        picker.Cancel();
        True(picker.Open(SessionA, "lamp-a", ColorMode.Rainbow), "The picker refused Rainbow.");
        Equal(true, picker.HasPendingColor);
        Equal(ColorMode.Rainbow, picker.PendingColor);
        Equal(ColorMode.Rainbow, picker.SourceColor);
        Equal(false, picker.Consumed);
    }

    private static void CancelDiscards()
    {
        var picker = new UiSelectionPolicy.ColorPickerState();
        True(picker.Open(SessionA, "lamp-a", ColorMode.Blue), "The picker refused an available lamp.");
        True(picker.Choose(ColorMode.Green), "A real emitted color was rejected as a pending pick.");
        Equal(ColorMode.Green, picker.PendingColor);
        picker.Cancel();
        Equal(false, picker.IsOpen);
        Equal(false, picker.HasPendingColor);
        Equal(false, picker.Consumed);
        Equal(UiSelectionPolicy.PickerOutcome.NotOpen,
            picker.Confirm(SessionA, "lamp-a", known: true, visible: true, sourceColor: ColorMode.Blue, out _));
        // A cancelled state cannot be resurrected by a late delegate.
        True(!picker.Choose(ColorMode.Red), "A closed picker still accepted a pick.");
    }

    private static void WrongSessionRejected()
    {
        var picker = new UiSelectionPolicy.ColorPickerState();
        picker.Open(SessionA, "lamp-a", ColorMode.Blue);
        picker.Choose(ColorMode.Red);
        Equal(UiSelectionPolicy.PickerOutcome.WrongSession,
            picker.Confirm(SessionB, "lamp-a", known: true, visible: true, sourceColor: ColorMode.Blue, out var commit));
        Equal(default, commit);
        Equal(false, picker.Consumed);
        // A load/session transition closes the modal instead of leaving a live confirmation behind.
        Equal(true, picker.CloseIfSessionChanged(SessionB));
        Equal(false, picker.IsOpen);
        Equal(false, picker.HasPendingColor);
        Equal(false, picker.CloseIfSessionChanged(SessionA));
    }

    private static void WrongSourceRejected()
    {
        var picker = new UiSelectionPolicy.ColorPickerState();
        picker.Open(SessionA, "lamp-a", ColorMode.Blue);
        picker.Choose(ColorMode.Red);
        // Clicking another row must not retarget the open picker.
        True(!picker.Open(SessionA, "lamp-b", ColorMode.Blue), "An open picker was retargeted to another lamp.");
        Equal("lamp-a", picker.SourceKey);
        Equal(UiSelectionPolicy.PickerOutcome.WrongSource,
            picker.Confirm(SessionA, "lamp-b", known: true, visible: true, sourceColor: ColorMode.Blue, out var commit));
        Equal(default, commit);
        Equal(false, picker.Consumed);
        Equal(UiSelectionPolicy.PickerOutcome.WrongSource,
            picker.Confirm(SessionA, null, known: true, visible: true, sourceColor: ColorMode.Blue, out _));
    }

    private static void UnavailableRejected()
    {
        var picker = new UiSelectionPolicy.ColorPickerState();
        picker.Open(SessionA, "lamp-a", ColorMode.Blue);
        picker.Choose(ColorMode.Red);
        Equal(UiSelectionPolicy.PickerOutcome.Unavailable,
            picker.Confirm(SessionA, "lamp-a", known: false, visible: false, sourceColor: ColorMode.Blue, out _));
        Equal(UiSelectionPolicy.PickerOutcome.Unavailable,
            picker.Confirm(SessionA, "lamp-a", known: true, visible: false, sourceColor: ColorMode.Blue, out _));
        Equal(false, picker.Consumed);
        Equal(true, picker.IsOpen);
        // A temporary disappearance keeps the staged pick visible but never applies it.
        Equal(ColorMode.Red, picker.PendingColor);
    }

    private static void StaleSourceRejected()
    {
        var picker = new UiSelectionPolicy.ColorPickerState();
        picker.Open(SessionA, "lamp-a", ColorMode.Blue);
        picker.Choose(ColorMode.Red);
        // Something else changed the lamp's color after the picker was opened.
        Equal(UiSelectionPolicy.PickerOutcome.StaleSource,
            picker.Confirm(SessionA, "lamp-a", known: true, visible: true, sourceColor: ColorMode.Green, out var commit));
        Equal(default, commit);
        Equal(false, picker.Consumed);
        Equal(true, picker.Stale);
        // The modal is refreshed visibly and the player must pick again; nothing was overwritten.
        Equal(false, picker.HasPendingColor);
        Equal(ColorMode.Green, picker.SourceColor);
        True(picker.Choose(ColorMode.Red), "A stale picker refused a fresh pick.");
        Equal(false, picker.Stale);
        Equal(UiSelectionPolicy.PickerOutcome.Committed,
            picker.Confirm(SessionA, "lamp-a", known: true, visible: true, sourceColor: ColorMode.Green, out var fresh));
        Equal(ColorMode.Red, fresh.Color);
    }

    private static void PickerColors()
    {
        var picker = new UiSelectionPolicy.ColorPickerState();
        picker.Open(SessionA, "lamp-a", ColorMode.Blue);
        True(!picker.Choose(ColorMode.Original), "The color modal accepted Original.");
        True(!picker.Choose((ColorMode)99), "The color modal accepted an invalid color.");
        Equal(ColorMode.Blue, picker.PendingColor);
        True(picker.Choose(ColorMode.Rainbow), "The approved tenth tile rejected Rainbow.");
        Equal(ColorMode.Rainbow, picker.PendingColor);
        True(!picker.Choose(ColorMode.Original), "Original replaced the staged Rainbow choice.");
        Equal(ColorMode.Rainbow, picker.PendingColor);
        // White is a real emitted color and stays available.
        True(picker.Choose(ColorMode.White), "The color modal rejected White.");
        Equal(ColorMode.White, picker.PendingColor);

        var empty = new UiSelectionPolicy.ColorPickerState();
        True(!empty.Open(Guid.Empty, "lamp-a", ColorMode.Blue), "The picker opened without a session.");
        True(!empty.Open(SessionA, "  ", ColorMode.Blue), "The picker opened without a source key.");
        Equal(false, empty.IsOpen);
    }

    private static void DistinctLamps()
    {
        var first = new UiSelectionPolicy.ColorPickerState();
        var second = new UiSelectionPolicy.ColorPickerState();
        first.Open(SessionA, "lamp-a", ColorMode.Blue);
        second.Open(SessionA, "lamp-b", ColorMode.Green);
        first.Choose(ColorMode.Red);
        second.Choose(ColorMode.Pink);
        Equal(UiSelectionPolicy.PickerOutcome.Committed,
            first.Confirm(SessionA, "lamp-a", known: true, visible: true, sourceColor: ColorMode.Blue, out var firstCommit));
        Equal("lamp-a", firstCommit.SourceKey);
        Equal(ColorMode.Red, firstCommit.Color);
        Equal(ColorMode.Pink, second.PendingColor);
        True(second.IsOpen, "Confirming one lamp closed the other lamp's picker.");
        Equal(UiSelectionPolicy.PickerOutcome.Committed,
            second.Confirm(SessionA, "lamp-b", known: true, visible: true, sourceColor: ColorMode.Green, out var secondCommit));
        Equal("lamp-b", secondCommit.SourceKey);
        Equal(ColorMode.Pink, secondCommit.Color);
    }

    private static void ClosureClearsPending()
    {
        foreach (var close in new Action<UiSelectionPolicy.ColorPickerState>[]
                 {
                     state => state.Close(),
                     state => state.Cancel()
                 })
        {
            var picker = new UiSelectionPolicy.ColorPickerState();
            picker.Open(SessionA, "lamp-a", ColorMode.Blue);
            picker.Choose(ColorMode.Red);
            close(picker);
            Equal(false, picker.IsOpen);
            Equal(false, picker.HasPendingColor);
            Equal(UiSelectionPolicy.PickerOutcome.NotOpen,
                picker.Confirm(SessionA, "lamp-a", known: true, visible: true, sourceColor: ColorMode.Blue, out _));
        }
        // A finished session also invalidates an open picker.
        var session = new UiSelectionPolicy.ColorPickerState();
        session.Open(SessionA, "lamp-a", ColorMode.Blue);
        Equal(true, session.CloseIfSessionChanged(Guid.Empty));
        Equal(false, session.IsOpen);
    }

    /// <summary>
    /// The approved brightness menu keeps its pre-redesign behavior: seven documented steps, each
    /// applied immediately through the existing intensity path. Moving the controls into their own
    /// card did not authorize a staged SELECT for brightness.
    /// </summary>
    private static void BrightnessSteps()
    {
        var seen = new HashSet<IntensityLevel>();
        for (int i = 0; i < UiSelectionPolicy.IntensityCount; i++)
        {
            var level = UiSelectionPolicy.IntensityAt(i);
            seen.Add(level);
            True(UiSelectionPolicy.IsIntensityStep(level),
                $"{UiSelectionPolicy.IntensityLabel(level)} is no longer a documented brightness step.");
        }
        Equal(7, UiSelectionPolicy.IntensityCount);
        Equal(7, seen.Count);
        Equal("10%", UiSelectionPolicy.IntensityLabel(UiSelectionPolicy.IntensityAt(0)));
        Equal("200%", UiSelectionPolicy.IntensityLabel(UiSelectionPolicy.IntensityAt(UiSelectionPolicy.IntensityCount - 1)));
        True(!UiSelectionPolicy.IsIntensityStep((IntensityLevel)99), "An undefined intensity was accepted as a step.");
        True(!UiSelectionPolicy.IsIntensityStep((IntensityLevel)((int)IntensityLevel.Percent10 + 1)),
            "An off-step intensity was accepted as a step.");
    }

    private static void AppWiring()
    {
        string app = ReadSource("LightControlApp.cs");
        True(app.Contains("UiSelectionPolicy.ColorPickerState", StringComparison.Ordinal), "The app does not own the staged color state.");
        True(!app.Contains("_brightPicker", StringComparison.Ordinal),
            "The unapproved staged brightness state is still wired.");
        True(app.Contains("_colorPicker.Confirm(", StringComparison.Ordinal), "SELECT does not go through the validated confirmation.");
        True(app.Contains("UiSelectionPolicy.PickerOutcome.Committed", StringComparison.Ordinal), "SELECT ignores the confirmation outcome.");
        True(app.Contains("ActOn(commit.SessionId, commit.SourceKey, key => LightRuntime.SetColor(key, commit.Color))", StringComparison.Ordinal),
            "The committed color is not applied to the captured session and source key.");
        True(app.Contains("ApplyBrightnessStep(captured)", StringComparison.Ordinal),
            "A brightness button no longer applies its own step on the click.");
        True(app.Contains("UiSelectionPolicy.IsIntensityStep(view.Intensity)", StringComparison.Ordinal),
            "Opening the brightness menu no longer requires a documented step.");

        // A tile stages; only SELECT reaches the runtime.
        string tile = Method(app, "private void CreateColorTile");
        True(tile.Contains("_colorPicker.Choose(color)", StringComparison.Ordinal), "A tile click does not stage the pending color.");
        Equal(true, !tile.Contains("LightRuntime.SetColor", StringComparison.Ordinal));
        True(app.Contains("private void ConfirmColor()", StringComparison.Ordinal), "The color modal has no confirmation path.");
        // The commit is consumed exactly once and the runtime call cannot be retried by a second click.
        True(app.Contains("CloseModal();", StringComparison.Ordinal), "The app never closes the modal path.");

        // The overlay is excluded from the root layout and the card layer blocks clicks behind it.
        True(app.Contains("ignoreLayout = true", StringComparison.Ordinal), "The overlay still participates in the root layout.");
        True(app.Contains("CardLayer", StringComparison.Ordinal), "No separate modal layer blocks clicks behind the card.");
        Equal(true, !app.Contains("GraphicRaycaster", StringComparison.Ordinal));

        // The other features stay reachable outside the color-only modal.
        True(app.Contains("\"More\"", StringComparison.Ordinal), "The per-row actions entry is gone.");
        True(app.Contains("ColorMode.Original", StringComparison.Ordinal) && app.Contains("ColorMode.Rainbow", StringComparison.Ordinal),
            "Original and Rainbow are not reachable outside the color modal.");
        True(app.Contains("UiSelectionPolicy.IntensityLabel(lamp.Intensity)", StringComparison.Ordinal),
            "The row does not show the lamp's current brightness percentage.");
        True(app.Contains("UiSelectionPolicy.PropertyCopyLabel", StringComparison.Ordinal), "Property copy lost its counted label.");
        True(app.Contains("UiSelectionPolicy.WorldCopyConfirmation", StringComparison.Ordinal), "All-properties copy lost its confirmation.");

        // No permanent editor and no new key handling.
        Equal(true, !app.Contains("_detailPanel", StringComparison.Ordinal));
        Equal(true, !app.Contains("_compactEditor", StringComparison.Ordinal));
        Equal(true, !app.Contains("protected override void Exit", StringComparison.Ordinal));
        Equal(true, !app.Contains("Input.GetKey", StringComparison.Ordinal));
    }

    /// <summary>
    /// Finding: More -> RunModalAction -> Act -> ActOn used to bind only the lamp identity key.
    /// A new session can keep that key, so a click on a stale card could still reach the runtime.
    /// The gate is pure and the phone app calls it, so both the decision and the wiring are covered.
    /// </summary>
    private static void ActionBoundToSession()
    {
        Equal(UiSelectionPolicy.ActionGate.Allowed,
            UiSelectionPolicy.ClassifyAction(SessionA, "lamp-a", ready: true, currentSession: SessionA, known: true, visible: true));

        // A reload can reuse the same lamp identity key: the old modal's session must not act.
        Equal(UiSelectionPolicy.ActionGate.WrongSession,
            UiSelectionPolicy.ClassifyAction(SessionA, "lamp-a", ready: true, currentSession: SessionB, known: true, visible: true));
        // A card captured before any session existed has nothing to act on.
        Equal(UiSelectionPolicy.ActionGate.WrongSession,
            UiSelectionPolicy.ClassifyAction(Guid.Empty, "lamp-a", ready: true, currentSession: SessionA, known: true, visible: true));
        Equal(UiSelectionPolicy.ActionGate.NotReady,
            UiSelectionPolicy.ClassifyAction(SessionA, "lamp-a", ready: false, currentSession: SessionA, known: true, visible: true));
        Equal(UiSelectionPolicy.ActionGate.UnknownKey,
            UiSelectionPolicy.ClassifyAction(SessionA, "lamp-a", ready: true, currentSession: SessionA, known: false, visible: false));
        Equal(UiSelectionPolicy.ActionGate.NotVisible,
            UiSelectionPolicy.ClassifyAction(SessionA, "lamp-a", ready: true, currentSession: SessionA, known: true, visible: false));
        Equal(UiSelectionPolicy.ActionGate.NoKey,
            UiSelectionPolicy.ClassifyAction(SessionA, "   ", ready: true, currentSession: SessionA, known: true, visible: true));
        Equal(UiSelectionPolicy.ActionGate.NoKey,
            UiSelectionPolicy.ClassifyAction(SessionA, null, ready: true, currentSession: SessionA, known: true, visible: true));
        // Unknown and invisible never act, even when the session matches.
        True(UiSelectionPolicy.ClassifyAction(SessionA, "lamp-a", true, SessionA, false, true) != UiSelectionPolicy.ActionGate.Allowed,
            "An unknown lamp was allowed to act.");
        True(UiSelectionPolicy.ClassifyAction(SessionA, "lamp-a", true, SessionA, true, false) != UiSelectionPolicy.ActionGate.Allowed,
            "A filtered-out lamp was allowed to act.");
    }

    /// <summary>
    /// The brightness menu is a move-only change: each of the seven steps applies on the button
    /// click through the existing intensity path, exactly as before the color dialog existed. This
    /// is a source contract for the native call wiring, not native execution or hit-testing.
    /// </summary>
    private static void ImmediateBrightnessWiring()
    {
        string app = ReadSource("LightControlApp.cs");
        string card = Method(app, "private void BuildBrightnessCard()");

        // The step callback itself applies; nothing is staged for a later confirmation.
        True(card.Contains("Wire(button, () => ApplyBrightnessStep(captured));", StringComparison.Ordinal),
            "A brightness step no longer applies itself on the button click.");
        True(!card.Contains("_brightPicker", StringComparison.Ordinal), "The brightness card still stages a pending step.");
        True(!card.Contains("ConfirmBrightness", StringComparison.Ordinal), "The brightness card kept its unapproved confirmation.");

        // One dismiss button, no SELECT placeholder: nothing here authorizes a second confirmation.
        True(card.Contains("FooterRow(card.Root.transform, \"Close\", CloseModal);", StringComparison.Ordinal),
            "The brightness footer is not a single dismiss button.");
        True(!card.Contains("\"SELECT\"", StringComparison.Ordinal), "The brightness footer still offers a SELECT.");
        True(!card.Contains("Applies after selecting", StringComparison.Ordinal),
            "The brightness card still announces a confirmation step.");

        // The handler is the sole SetIntensity path and is gated on the brightness card being open.
        True(app.Contains("private void ApplyBrightnessStep(IntensityLevel level)", StringComparison.Ordinal),
            "No immediate brightness handler exists.");
        string apply = Method(app, "private void ApplyBrightnessStep(");
        True(apply.Contains("if (_modal != ModalKind.Brightness) return;", StringComparison.Ordinal),
            "A brightness handler can still act after the color or More card opened.");
        True(apply.Contains("UiSelectionPolicy.IsIntensityStep(level)", StringComparison.Ordinal),
            "The brightness handler accepts a value that is not a documented step.");
        True(apply.Contains("ActOn(session, key, exact => LightRuntime.SetIntensity(exact, level))", StringComparison.Ordinal),
            "The step does not reach SetIntensity through the captured session and key gate.");
        True(apply.Contains("Guid session = _modalSession;", StringComparison.Ordinal)
             && apply.Contains("string key = _modalKey;", StringComparison.Ordinal),
            "The handler acts on mutable modal fields instead of the values captured at click time.");
        True(!apply.Contains("CloseModal", StringComparison.Ordinal),
            "Applying a step closes the menu, so brightness can no longer be previewed step by step.");
    }

    /// <summary>
    /// Finding: SizeModals used width * 0.5 / 0.4 with no floor, so a 320-wide container produced a
    /// 160-pixel card whose fixed footer overflowed. The approved normal composition must not move.
    /// </summary>
    private static void CardWidthClamped()
    {
        // Approved reference: 1201 x 655, measured container. Half-width card and 40% side cards.
        Close(600.5, UiSelectionPolicy.ModalCardWidth(1201f, 0.5f, 300f, 1157f));
        Close(480.4, UiSelectionPolicy.ModalCardWidth(1201f, 0.4f, 300f, 1157f));
        // Nothing may exceed the available width, even when the fraction would.
        Close(1157f, UiSelectionPolicy.ModalCardWidth(4000f, 0.5f, 300f, 1157f));
        // Compact container: the fraction alone is 160/128, far below a usable card. The floor wins.
        Close(300f, UiSelectionPolicy.ModalCardWidth(320f, 0.5f, 300f, 302f));
        Close(300f, UiSelectionPolicy.ModalCardWidth(320f, 0.4f, 300f, 302f));
        True(UiSelectionPolicy.ModalCardWidth(320f, 0.5f, 300f, 302f) >= 300f,
            "A compact card collapsed below the usable minimum width.");
        // A container shorter than the minimum still wins: the card never overflows the phone.
        Close(180f, UiSelectionPolicy.ModalCardWidth(180f, 0.5f, 300f, 180f));
        // Unmeasured or nonsensical input sizes nothing.
        Close(0f, UiSelectionPolicy.ModalCardWidth(0f, 0.5f, 300f, 0f));
        Close(0f, UiSelectionPolicy.ModalCardWidth(float.NaN, 0.5f, 300f, 100f));
        Close(0f, UiSelectionPolicy.ModalCardWidth(600f, 0f, 300f, 400f));
    }

    /// <summary>
    /// Finding: UIFactory.ScrollableVerticalList stretches its content but leaves the default
    /// 100-unit sizeDelta, so a content rect is 100 units wider than the viewport and 50 units are
    /// clipped on each side. This is a source contract: it proves the wiring, not native fit.
    /// </summary>
    private static void EveryScrollListPinned()
    {
        string app = ReadSource("LightControlApp.cs");

        // The helper itself must do the three things the clipping fix needs.
        string pin = Method(app, "private static void PinScrollWidth");
        True(pin.Contains("horizontalFit = ContentSizeFitter.FitMode.Unconstrained", StringComparison.Ordinal),
            "The pinned content still stretches horizontally.");
        True(pin.Contains("anchorMin = new Vector2(0f, 1f)", StringComparison.Ordinal)
             && pin.Contains("anchorMax = new Vector2(1f, 1f)", StringComparison.Ordinal),
            "The pinned content is not full-width stretched at the top edge.");
        True(pin.Contains("pivot = new Vector2(0.5f, 1f)", StringComparison.Ordinal), "The pinned content lost its top pivot.");
        True(pin.Contains("sizeDelta = new Vector2(0f, ", StringComparison.Ordinal),
            "The pinned content keeps the default 100-unit horizontal sizeDelta.");

        // Every ScrollableVerticalList result is pinned, not only the main lamp list.
        string[] lines = app.Replace("\r\n", "\n").Split('\n');
        int sites = 0;
        for (int i = 0; i < lines.Length; i++)
        {
            if (!lines[i].Contains("UIFactory.ScrollableVerticalList(", StringComparison.Ordinal)) continue;
            sites++;
            bool pinned = false;
            for (int j = i; j < Math.Min(lines.Length, i + 8); j++)
            {
                if (lines[j].Contains("PinScrollWidth(", StringComparison.Ordinal)) { pinned = true; break; }
            }
            True(pinned, $"ScrollableVerticalList at line {i + 1} is not width-pinned; its content keeps the default 100-unit sizeDelta.");
        }
        True(sites >= 1, "The pooled lamp table no longer has a scroll view.");
        // Fixed reference cards fit as a whole; only the unbounded lamp list requires scrolling.
        True(Method(app, "private void BuildView(").Contains("PinScrollWidth(content);", StringComparison.Ordinal),
            "The lamp list content is created without the width pin.");
        True(Method(app, "private void MeasureLayout()").Contains("UiSelectionPolicy.ReferenceCanvasScale", StringComparison.Ordinal),
            "The fixed reference cards are not fitted with their containing canvas.");
    }

    /// <summary>
    /// Finding: the full-root CardLayer Image answered every raycast above the backdrop Button, so
    /// an outside click never reached the dismiss handler. Sibling layers do not share an ancestor
    /// chain, so the layer must not be a raycast target and the card consumes its own clicks.
    /// Source contract: it proves the hierarchy wiring, not native hit-testing.
    /// </summary>
    private static void OutsideClickRouting()
    {
        string app = ReadSource("LightControlApp.cs");
        string build = Method(app, "private void BuildModal()");
        True(build.Contains("Wire(dismiss, CloseModal);", StringComparison.Ordinal), "The backdrop no longer dismisses the modal.");
        Equal(false, build.Contains("blocker.raycastTarget = true", StringComparison.Ordinal));
        True(build.Contains("blocker.raycastTarget = false;", StringComparison.Ordinal),
            "The full-root card layer still swallows every outside click before the dismiss button.");
        // The backdrop is still a full-root sibling of the card layer, built first.
        True(build.Contains("UIFactory.Panel(\"Backdrop\", _modalRoot.transform", StringComparison.Ordinal),
            "The backdrop is no longer a full-root panel of the modal.");
        True(build.Contains("UIFactory.Panel(\"CardLayer\", _modalRoot.transform", StringComparison.Ordinal),
            "The card layer is no longer a sibling of the backdrop.");

        // The card itself answers the interior hit, so a card click cannot bubble into a dismiss.
        string card = Method(app, "private ModalCard NewCard(");
        True(card.Contains("Wire(cardButton, NoOpCardClick);", StringComparison.Ordinal),
            "A click inside the card is not consumed by the card.");
        True(app.Contains("private static void NoOpCardClick()", StringComparison.Ordinal), "The consuming card handler is missing.");
        True(card.Contains("fill.raycastTarget = true;", StringComparison.Ordinal), "The card stopped answering its own interior hits.");
    }

    /// <summary>
    /// The four action entry points and the commit wiring must all pass through the session+key
    /// gate. Source contract: it proves the wiring, not a native session transition.
    /// </summary>
    private static void ActionGateWiring()
    {
        string app = ReadSource("LightControlApp.cs");
        True(app.Contains("UiSelectionPolicy.ClassifyAction(", StringComparison.Ordinal), "No modal action passes the session gate.");

        string act = Method(app, "private void ActOn(");
        True(app.Contains("private void ActOn(Guid session, string key,", StringComparison.Ordinal),
            "ActOn is not bound to the captured modal session.");
        True(act.Contains("LightRuntime.SessionId", StringComparison.Ordinal), "The action gate does not compare the live session.");
        True(act.Contains("LightRuntime.Ready", StringComparison.Ordinal), "The action gate ignores readiness.");
        True(act.Contains("LightRuntime.IsKnownLamp(key)", StringComparison.Ordinal), "The action gate ignores an unknown lamp.");

        string run = Method(app, "private void RunModalAction(");
        True(run.Contains("_modal != ModalKind.Actions", StringComparison.Ordinal),
            "A hidden actions card can still act while another modal is open.");

        // The captured session travels with the staged color commit.
        True(app.Contains("_colorPicker.Confirm(LightRuntime.SessionId", StringComparison.Ordinal), "The color SELECT lost its session check.");
        // Brightness has no staged commit: the immediate step handler passes the same gate.
        True(app.Contains("private void ApplyBrightnessStep(IntensityLevel level)", StringComparison.Ordinal),
            "No immediate brightness handler exists to gate.");
        string apply = Method(app, "private void ApplyBrightnessStep(");
        True(apply.Contains("ActOn(session, key, exact => LightRuntime.SetIntensity(exact, level))", StringComparison.Ordinal),
            "The immediate brightness path bypasses the session+key gate.");
        True(app.Contains("_modalSession = Guid.Empty;", StringComparison.Ordinal),
            "A closed modal keeps its old session identity.");
    }

    /// <summary>
    /// The graphite table uses one shared column definition and explicit color controls. This is
    /// source-wiring evidence only; native PhoneApp geometry remains an in-game acceptance item.
    /// </summary>
    private static void ProfessionalRowWiring()
    {
        string app = ReadSource("LightControlApp.cs");
        string row = Method(app, "private LampRow CreateRow(");
        True(row.Contains("Columns(root)", StringComparison.Ordinal), "The row does not use the shared column definition.");
        True(row.Contains("22f, 22f", StringComparison.Ordinal), "The color control lost its small square swatch.");
        True(!row.Contains("UISprites.Circle()", StringComparison.Ordinal),
            "The lamp marker is still a large circular dot.");
        True(row.Contains("flexibleWidth = 1f", StringComparison.Ordinal),
            "The lamp-name column does not receive the remaining row width.");
        True(row.Contains("SetButtonSize(colorGo, ColorColumnWidth", StringComparison.Ordinal)
             && row.Contains("SetButtonSize(brightColumn, BrightnessColumnWidth", StringComparison.Ordinal)
             && row.Contains("SetButtonSize(moreColumn, MoreColumnWidth", StringComparison.Ordinal),
            "The explicit Color, Brightness and More columns lost their shared widths.");
        True(row.Contains("Clickable(colorGo, () => OpenColorPicker(captured))", StringComparison.Ordinal),
            "The explicit Color action no longer opens the exact row's picker.");
        string ui = Method(app, "private void BuildView(");
        True(ui.Contains("CreateColumnHeader(scroll.viewport)", StringComparison.Ordinal)
            && ui.Contains("ColumnHeaderSpace", StringComparison.Ordinal),
            "The sticky heading lost its viewport owner or its list-layout spacer.");
        // Regression: a second, untracked heading row outside the list was created at build time and
        // would never be hidden, so the shipped app showed the column labels twice.
        int buildSites = CountCalls(ui, "CreateColumnHeader(");
        Equal(1, buildSites);
        // Heading and rows must share the flexible column minimum, otherwise the labels drift.
        True(app.Contains("private const float LightColumnMinWidth", StringComparison.Ordinal),
            "The flexible lamp column has no shared minimum width.");
        Equal(2, CountCalls(app, "minWidth = LightColumnMinWidth"));
        string columns = Method(app, "private GameObject CreateColumnHeader(");
        True(columns.Contains("Columns(columns)", StringComparison.Ordinal)
             && columns.Contains("ColorColumnWidth", StringComparison.Ordinal)
             && columns.Contains("BrightnessColumnWidth", StringComparison.Ordinal)
             && columns.Contains("MoreColumnWidth", StringComparison.Ordinal),
            "The heading row does not use the same four columns as the lamp rows.");
    }

    private static int CountCalls(string source, string fragment)
    {
        int count = 0;
        for (int at = source.IndexOf(fragment, StringComparison.Ordinal); at >= 0;
             at = source.IndexOf(fragment, at + fragment.Length, StringComparison.Ordinal))
            count++;
        return count;
    }

    /// <summary>
    /// Regression from the screenshots: the hint rendered but Back/Select did not. A footer's
    /// total preferred height does not guarantee its children survive child-controlled layout.
    /// Every button must therefore reserve a concrete nonzero height of its own.
    /// </summary>
    private static void FooterButtonHeights()
    {
        string app = ReadSource("LightControlApp.cs");
        string footer = Method(app, "private GameObject FooterRow(Transform parent, string backLabel");
        Equal(2, CountCalls(footer, "FooterButton("));
        string single = Method(app, "private GameObject FooterRow(Transform parent, string label, Action dismiss");
        Equal(1, CountCalls(single, "FooterButton("));
        string button = Method(app, "private Button FooterButton(");
        True(button.Contains("buttonLayout.preferredHeight = 54f", StringComparison.Ordinal)
             && button.Contains("buttonLayout.minHeight = 54f", StringComparison.Ordinal)
             && button.Contains("buttonLayout.flexibleHeight = 0f", StringComparison.Ordinal),
            "A footer button has no reserved nonzero height in the child-controlled layout.");
        True(button.Contains("Stretch(labelText.gameObject)", StringComparison.Ordinal),
            "The footer label is not bound to its actual button rectangle.");
    }

    private static void Close(double expected, double actual)
    {
        if (!(Math.Abs(expected - actual) <= 0.001))
            throw new InvalidOperationException($"Expected [{expected}]; actual [{actual}].");
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

    /// <summary>One method body, bounded by the next 4-space member so a later method cannot satisfy it.</summary>
    private static string Method(string source, string signature)
    {
        int at = source.IndexOf(signature, StringComparison.Ordinal);
        if (at < 0) throw new InvalidOperationException($"Missing method: {signature}");
        int end = source.IndexOf("\n    private ", at + signature.Length, StringComparison.Ordinal);
        return end < 0 ? source.Substring(at) : source.Substring(at, end - at);
    }
}
