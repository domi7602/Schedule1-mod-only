using System;
using System.Reflection;
using MelonLoader;

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
    /// </summary>
    public static int CurrentDay()
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
            Mod.Log.Warn($"HitmanPhoneTime.CurrentDay failed: {ex.Message}");
            return 0;
        }
    }
}
