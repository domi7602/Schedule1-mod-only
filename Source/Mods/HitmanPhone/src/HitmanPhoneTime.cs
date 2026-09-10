using System;
using System.Reflection;
using MelonLoader;
using UnityEngine;

#if (IL2CPPMELON)
using S1Time = Il2CppScheduleOne.GameTime.TimeManager;
#elif MONOMELON
using S1Time = ScheduleOne.GameTime.TimeManager;
#endif

namespace HitmanPhone;

/// <summary>
/// Wrappers around Schedule I's <see cref="S1Time"/> for hitman-phase time
/// queries. Keeps reflection in one place so callers can ask for the current
/// in-game day without learning the stub layout.
/// </summary>
public static class HitmanPhoneTime
{
    // Audit L3 (2026-09-01): CurrentDay is called from every scheduler tick —
    // resolve the member infos ONCE instead of per call (per-frame reflection
    // churn). The IL2CPP stub type never changes within a session.
    private static PropertyInfo? _elapsedDaysProp;
    private static FieldInfo? _elapsedDaysField;
    private static bool _membersResolved;

    // Audit (2026-09-10): CurrentDay is called from every scheduler/expiry tick
    // (i.e. every OnUpdate frame) — cache the resolved day and refresh at most
    // once per second so the hot loop avoids interop reflection + boxing per
    // frame (same _nextTick throttle shape as BountyTargetWatchdog). No
    // OnDayPass-style event exists in this mod to hook instead; day-granular
    // bounty deadlines tolerate a 1s-stale day. First call resolves immediately.
    private static int _cachedDay;
    private static float _nextRefreshRealtime;
    private static bool _hasCachedDay;

    private static void ResolveMembers()
    {
        if (_membersResolved) return;
        _membersResolved = true;
        try
        {
            var type = typeof(S1Time);
            _elapsedDaysProp = type.GetProperty("ElapsedDays",
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            _elapsedDaysField = type.GetField("_ElapsedDays_k__BackingField",
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        }
        catch (Exception ex)
        {
            Mod.Log.Warn($"HitmanPhoneTime member resolution failed: {ex.Message}");
        }
    }

    /// <summary>
    /// Elapsed in-game days (whole days). Falls back to 0 when TimeManager is
    /// not yet initialised (e.g. before the player enters the gameplay scene).
    /// Throttled: the interop read runs at most once per second, otherwise the
    /// cached value is returned.
    /// </summary>
    public static int CurrentDay()
    {
        try
        {
            float now = Time.realtimeSinceStartup;
            if (_hasCachedDay && now < _nextRefreshRealtime) return _cachedDay;
            int day = ResolveDayUncached();
            _cachedDay = day;
            _hasCachedDay = true;
            _nextRefreshRealtime = now + 1.0f;
            return day;
        }
        catch (Exception ex)
        {
            Mod.Log.Warn($"HitmanPhoneTime.CurrentDay failed: {ex.Message}");
            return _hasCachedDay ? _cachedDay : 0;
        }
    }

    /// <summary>
    /// Uncached interop read of ElapsedDays (property, else backing field).
    /// </summary>
    private static int ResolveDayUncached()
    {
        try
        {
            var tm = S1Time.Instance;
            if (tm == null) return 0;
            ResolveMembers();
            // ElapsedDays is exposed via property in newer patches; the backing
            // field is the fallback.
            if (_elapsedDaysProp != null && _elapsedDaysProp.CanRead)
            {
                var v = _elapsedDaysProp.GetValue(tm);
                if (v is int d) return d;
            }
            if (_elapsedDaysField != null)
            {
                var v = _elapsedDaysField.GetValue(tm);
                if (v is int d2) return d2;
            }
            return 0;
        }
        catch (Exception ex)
        {
            Mod.Log.Warn($"HitmanPhoneTime.ResolveDayUncached failed: {ex.Message}");
            return 0;
        }
    }
}
