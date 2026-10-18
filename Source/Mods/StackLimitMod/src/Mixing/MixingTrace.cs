// TEMPORARY diagnostic instrumentation (2026-10-05) - REMOVE after the fix.
//
// Purpose: pin the reported item loss in the mixing-station chain:
//   "With StackLimitMod active and the MK2 mixing station, pressing Begin twice
//    makes ingredients/product disappear - about half the batch (e.g. 20 of 40)."
// The loss session's Latest.log shows no exception and no mixing output at all,
// so this is instrumentation instead of a guessed fix (bug-hunt rule): every
// relevant entry point logs a compact before/after station snapshot at Info.
//
// Coverage (round 1):
//   UI chain:      MixingStationInterface.Open / BeginButtonPressed / BeginTask
//                  / BeginMix / MixNamed
//   Station flow:  StartButtonClicked, TryCreateOutputItems, MixingStart,
//                  MixingDone - on BOTH MixingStation and MixingStationMk2
//                  (the Mk2 overrides MixingStart/MixingDone, so patching only
//                  the base class would silently miss the MK2 flow).
//   Mutations:     SendMixingOperation, SetMixOperation, SetItemSlotQuantity,
//                  SetItemSlotQuantity_Internal, SetStoredInstance(_Internal)
//                  - each with before/after slot snapshots.
// Round 2 (only if state changes without a trace line): ItemSlot-level watchers.
//
// Log-only: no patch mutates game state. Remove this file and the
// MixingTrace.Apply call in Mod.ApplyHarmonyPatches once the loss path is
// pinned and fixed.

using System;
using System.Reflection;
using Il2CppScheduleOne.ItemFramework;
using Il2CppScheduleOne.ObjectScripts;
using Il2CppScheduleOne.UI.Stations;
using S1Mods.Shared;

namespace StackLimitMod;

internal static class MixingTrace
{
    private static ModLogger? _log;
    private static int _seq;
    private static MixingStation? _current;

    public static void Apply(HarmonyLib.Harmony harmony, ModLogger log)
    {
        _log = log;
        int ok = 0;

        // UI chain (one open interface at a time; state read from _current).
        ok += P(harmony, typeof(MixingStationInterface), nameof(MixingStationInterface.Open),
            postfix: nameof(Open_Post));
        ok += P(harmony, typeof(MixingStationInterface), nameof(MixingStationInterface.BeginButtonPressed),
            prefix: nameof(UiFlow_Pre));
        ok += P(harmony, typeof(MixingStationInterface), nameof(MixingStationInterface.BeginTask),
            prefix: nameof(UiFlow_Pre));
        ok += P(harmony, typeof(MixingStationInterface), nameof(MixingStationInterface.BeginMix),
            prefix: nameof(UiFlow_Pre));
        ok += P(harmony, typeof(MixingStationInterface), "MixNamed", prefix: nameof(UiFlow_Pre));

        // Station flow - base class.
        ok += P(harmony, typeof(MixingStation), nameof(MixingStation.StartButtonClicked),
            prefix: nameof(StationFlow_Pre));
        ok += P(harmony, typeof(MixingStation), nameof(MixingStation.MixingStart),
            prefix: nameof(StationFlow_Pre));
        ok += P(harmony, typeof(MixingStation), nameof(MixingStation.MixingDone),
            prefix: nameof(StationFlow_Pre));
        ok += P(harmony, typeof(MixingStation), nameof(MixingStation.TryCreateOutputItems),
            prefix: nameof(StationFlow_Pre), postfix: nameof(After_Post));

        // Station flow - MK2 overrides (the reported station type).
        ok += P(harmony, typeof(MixingStationMk2), nameof(MixingStationMk2.MixingStart),
            prefix: nameof(StationFlow_Pre));
        ok += P(harmony, typeof(MixingStationMk2), nameof(MixingStationMk2.MixingDone),
            prefix: nameof(StationFlow_Pre));

        // Mutations: before/after snapshots around the real state changes.
        ok += P(harmony, typeof(MixingStation), nameof(MixingStation.SendMixingOperation),
            prefix: nameof(Mut_Pre), postfix: nameof(After_Post));
        ok += P(harmony, typeof(MixingStation), nameof(MixingStation.SetMixOperation),
            prefix: nameof(Mut_Pre), postfix: nameof(After_Post));
        ok += P(harmony, typeof(MixingStation), nameof(MixingStation.SetItemSlotQuantity),
            prefix: nameof(Mut_Pre), postfix: nameof(After_Post));
        ok += P(harmony, typeof(MixingStation), nameof(MixingStation.SetItemSlotQuantity_Internal),
            prefix: nameof(Mut_Pre), postfix: nameof(After_Post));
        ok += P(harmony, typeof(MixingStation), "SetStoredInstance",
            prefix: nameof(Mut_Pre), postfix: nameof(After_Post));
        ok += P(harmony, typeof(MixingStation), "SetStoredInstance_Internal",
            prefix: nameof(Mut_Pre), postfix: nameof(After_Post));

        log.Info($"MixTrace: {ok}/17 diagnostic patches applied (log-only, temporary).");
    }

    private static int P(
        HarmonyLib.Harmony harmony,
        Type target,
        string methodName,
        string? prefix = null,
        string? postfix = null)
    {
        return PatchGuard.TryPatch(
            harmony,
            target,
            methodName,
            prefix: prefix == null ? null : new HarmonyLib.HarmonyMethod(typeof(MixingTrace), prefix),
            postfix: postfix == null ? null : new HarmonyLib.HarmonyMethod(typeof(MixingTrace), postfix),
            log: _log)
            ? 1
            : 0;
    }

    // ----- patch handlers (all log-only, never throw into the game) -----

    public static void Open_Post(MixingStation station)
    {
        try
        {
            _current = station;
            E($"UI-OPEN cfg {Cfg(station)} | {Snap(station)}");
        }
        catch (Exception ex) { F(nameof(Open_Post), ex); }
    }

    public static void UiFlow_Pre(object[] __args, MethodBase __originalMethod)
    {
        try { E($"{Tag(__originalMethod)} {A(__args)} | {Snap(_current)}"); }
        catch (Exception ex) { F(nameof(UiFlow_Pre), ex); }
    }

    public static void StationFlow_Pre(MixingStation __instance, MethodBase __originalMethod)
    {
        try { E($"{Tag(__originalMethod)} | {Snap(__instance)}"); }
        catch (Exception ex) { F(nameof(StationFlow_Pre), ex); }
    }

    public static void Mut_Pre(MixingStation __instance, object[] __args, MethodBase __originalMethod)
    {
        try { E($"{Tag(__originalMethod)} {A(__args)} | before {Snap(__instance)}"); }
        catch (Exception ex) { F(nameof(Mut_Pre), ex); }
    }

    public static void After_Post(MixingStation __instance, MethodBase __originalMethod)
    {
        try { L($"   after {Tag(__originalMethod)} | {Snap(__instance)}"); }
        catch (Exception ex) { F(nameof(After_Post), ex); }
    }

    // ----- formatting helpers -----

    private static string Cfg(MixingStation? s)
    {
        try
        {
            if (!Alive(s)) return "<no station>";
            return $"max={s!.MaxMixQuantity} mixTimePerItem={s.MixTimePerItem} autoInsert={s.RequiresIngredientInsertion}";
        }
        catch (Exception ex) { return $"<cfg fail: {ex.GetType().Name}>"; }
    }

    private static string Snap(MixingStation? s)
    {
        try
        {
            if (!Alive(s)) return "<no station>";
            return $"P={Slot(s!.ProductSlot)} M={Slot(s.MixerSlot)} O={Slot(s.OutputSlot)}" +
                   $" | {Op(s.CurrentMixOperation)} t={s.CurrentMixTime} done={s.IsMixingDone}" +
                   $" mixQ={SafeMixQ(s)}/{s.MaxMixQuantity} canStart={s.CanStartMix()}";
        }
        catch (Exception ex) { return $"<snap fail: {ex.GetType().Name}>"; }
    }

    private static string SafeMixQ(MixingStation s)
    {
        try { return s.GetMixQuantity().ToString(); }
        catch { return "?"; }
    }

    private static string Slot(ItemSlot? slot)
    {
        try
        {
            if (slot == null || slot.Pointer == IntPtr.Zero) return "?";
            var inst = slot.ItemInstance;
            string id = (inst != null && inst.Pointer != IntPtr.Zero) ? inst.ID : "-";
            return $"{slot.Quantity}x{id}";
        }
        catch { return "!"; }
    }

    private static string Op(MixOperation? op)
    {
        try
        {
            if (op == null || op.Pointer == IntPtr.Zero) return "op=none";
            return $"op={op.ProductID}+{op.IngredientID}x{op.Quantity}";
        }
        catch { return "op=!"; }
    }

    private static string A(object[]? args)
    {
        if (args == null || args.Length == 0) return string.Empty;
        var parts = new string[args.Length];
        for (int i = 0; i < args.Length; i++) parts[i] = Arg(args[i]);
        return string.Join(" ", parts);
    }

    private static string Arg(object? a)
    {
        try
        {
            if (a == null) return "null";
            if (a is int i) return i.ToString();
            if (a is ItemInstance inst) return $"item({inst.ID} x{inst.Quantity})";
            if (a is MixOperation op) return Op(op);
            return a.ToString() ?? a.GetType().Name;
        }
        catch { return "?"; }
    }

    private static string Tag(MethodBase m) =>
        $"{m.DeclaringType?.Name ?? "?"}.{m.Name}";

    private static bool Alive(MixingStation? s) =>
        s != null && s.Pointer != IntPtr.Zero && !s.WasCollected;

    private static int E(string msg)
    {
        int seq = ++_seq;
        _log?.Info($"[MixTrace] #{seq} {msg}");
        return seq;
    }

    private static void L(string msg) => _log?.Info($"[MixTrace] {msg}");

    private static void F(string where, Exception ex) =>
        _log?.Warn($"[MixTrace] {where} failed: {ex.GetType().Name}: {ex.Message}");
}
