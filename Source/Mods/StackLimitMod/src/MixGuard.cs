// PERMANENT fix (2026-10-05) - mixing-station item loss with StackLimitMod
// stack limits (reported with the MK2 mixing station, reproduced + traced).
//
// Cause chain (proven by [MixTrace] log 26-10-5_20-54-36, events #27-46):
//   1. Vanilla invariant: per-batch cap (MaxMixQuantity=20) equals the whole
//      input stack (vanilla stack limit 20), so Begin #1 empties both input
//      slots and CanStartMix() returns false until new items are inserted.
//   2. StackLimitMod breaks that invariant: 40-item stacks go in, Begin #1
//      consumes only the batch (40 -> 20) and leaves 20 in each slot.
//   3. CanStartMix() only inspects slots vs recipe - it never checks whether
//      an operation is already running - so it returns TRUE again mid-mix
//      (trace #37: second Begin with op running at t=4, canStart=True).
//   4. BeginMix #2 consumes the leftovers (trace #39-42) and SetMixOperation
//      OVERWRITES the running operation (trace #44: t=4 reset to t=0).
//   5. The overwritten operation's consumed inputs (20+20) and its output are
//      destroyed - with 40-item stacks that is exactly half the loaded
//      material ("20 ingredients and product gone").
//
// Fix - restore the vanilla invariant "one operation at a time":
//   1. CanStartMix        -> false while a mix is active (Begin greys out).
//   2. BeginMix           -> skip before any consumption happens.
//   3. SendMixingOperation-> skip for entry paths that bypass the UI.
//   4. SetMixOperation    -> never replace a not-yet-done operation (state
//      invariant; the operation is committed and MixingStart runs inside
//      SetMixOperation, so this also blocks the silent restart).
// Clearing (operation == null) and starting while the previous operation is
// done stay allowed, so multi-batch mixing with big stacks keeps working.

using System;
using HarmonyLib;
using Il2CppScheduleOne.ObjectScripts;
using Il2CppScheduleOne.UI;
using Il2CppScheduleOne.UI.Stations;
using S1Mods.Shared;
using UnityEngine;

namespace StackLimitMod;

internal static class MixGuard
{
    private const long NoticeIntervalMs = 3000;

    private static ModLogger? _log;
    private static long _lastNoticeTick = -NoticeIntervalMs;
    private static Sprite? _noticeIcon;

    public static void Apply(HarmonyLib.Harmony harmony, ModLogger log)
    {
        _log = log;
        int ok = 0;

        ok += P(harmony, typeof(MixingStation), nameof(MixingStation.CanStartMix),
            postfix: nameof(CanStartMix_Post))
            ? 1
            : 0;
        ok += P(harmony, typeof(MixingStationInterface), nameof(MixingStationInterface.BeginMix),
            prefix: nameof(BeginMix_Pre))
            ? 1
            : 0;
        ok += P(harmony, typeof(MixingStation), nameof(MixingStation.SendMixingOperation),
            prefix: nameof(SendOp_Pre))
            ? 1
            : 0;

        // SetMixOperation is matched without parameterTypes - identical to the
        // working trace round, where this resolution caught every observed
        // SetMixOperation call (the (conn, operation, mixTime) client-RPC form).
        ok += P(harmony, typeof(MixingStation), nameof(MixingStation.SetMixOperation),
            prefix: nameof(SetOp_Pre))
            ? 1
            : 0;

        log.Info($"MixGuard: {ok}/4 guard patches applied (blocks double mix starts).");
    }

    private static bool P(
        HarmonyLib.Harmony harmony,
        Type target,
        string methodName,
        string? prefix = null,
        string? postfix = null,
        Type[]? parameterTypes = null)
    {
        return PatchGuard.TryPatch(
            harmony,
            target,
            methodName,
            prefix: prefix == null ? null : new HarmonyLib.HarmonyMethod(typeof(MixGuard), prefix),
            postfix: postfix == null ? null : new HarmonyLib.HarmonyMethod(typeof(MixGuard), postfix),
            parameterTypes: parameterTypes,
            log: _log);
    }

    // ----- patch handlers -----

    public static void CanStartMix_Post(MixingStation __instance, ref bool __result)
    {
        try
        {
            if (__result && IsActive(__instance)) __result = false;
        }
        catch (Exception ex) { F(nameof(CanStartMix_Post), ex); }
    }

    public static bool BeginMix_Pre(MixingStationInterface __instance)
    {
        try
        {
            var station = __instance.Station;
            if (IsActive(station))
            {
                Blocked("BeginMix", station!);
                return false;
            }
        }
        catch (Exception ex) { F(nameof(BeginMix_Pre), ex); }
        return true;
    }

    public static bool SendOp_Pre(MixingStation __instance, MixOperation operation, int mixTime)
    {
        try
        {
            if (operation != null && operation.Pointer != IntPtr.Zero && IsActive(__instance))
            {
                Blocked("SendMixingOperation", __instance);
                return false;
            }
        }
        catch (Exception ex) { F(nameof(SendOp_Pre), ex); }
        return true;
    }

    public static bool SetOp_Pre(MixingStation __instance, MixOperation operation, int mixTime)
    {
        try
        {
            // Allow clearing (null) and any set while no operation is running
            // or the running one is done; block replacing an unfinished op.
            if (operation != null && operation.Pointer != IntPtr.Zero && IsActive(__instance))
            {
                Blocked("SetMixOperation", __instance);
                return false;
            }
        }
        catch (Exception ex) { F(nameof(SetOp_Pre), ex); }
        return true;
    }

    // ----- helpers -----

    private static bool IsActive(MixingStation? station)
    {
        if (station == null || station.Pointer == IntPtr.Zero || station.WasCollected) return false;
        var op = station.CurrentMixOperation;
        return op != null && op.Pointer != IntPtr.Zero && !station.IsMixingDone;
    }

    private static void Blocked(string where, MixingStation station)
    {
        _log?.Info($"MixGuard: blocked '{where}' - a mix is already running on this station.");
        ShowBlockedNotice();
    }

    private static void ShowBlockedNotice()
    {
        long now = Environment.TickCount64;
        if (now - _lastNoticeTick < NoticeIntervalMs) return;
        _lastNoticeTick = now;
        try
        {
            var notifMgr = NotificationsManager.Instance;
            if (notifMgr != null && (UnityEngine.Object)notifMgr != null)
                notifMgr.SendNotification("Mixing Station", "Mixing already running", GetNoticeIcon(), 3f, false);
        }
        catch (Exception ex) { _log?.Debug($"MixGuard: notice failed: {ex.Message}"); }
    }

    // Vanilla NotificationsManager draws a plain grey box when the icon is null.
    private static Sprite GetNoticeIcon()
    {
        if (_noticeIcon != null) return _noticeIcon;

        const int size = 64;
        var tex = new Texture2D(size, size, TextureFormat.RGBA32, false) { name = "StackLimitMod_MixNotice" };
        var amber = new Color32(196, 128, 32, 255);
        var dark = new Color32(60, 40, 10, 255);
        var px = new Color32[size * size];

        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                bool bar =
                    (x >= 14 && x < 50 && y >= 22 && y < 30) ||   // upper bar
                    (x >= 14 && x < 50 && y >= 34 && y < 42);     // lower bar
                px[y * size + x] = bar ? dark : amber;
            }
        }

        tex.SetPixels32(px);
        tex.Apply(false, true);
        _noticeIcon = Sprite.Create(tex, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f));
        return _noticeIcon;
    }

    private static void F(string where, Exception ex) =>
        _log?.Warn($"MixGuard: {where} failed: {ex.GetType().Name}: {ex.Message} - start allowed");
}
