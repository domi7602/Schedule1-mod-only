using System;

namespace S1Mods.Shared;

/// <summary>
/// Isolierte Aktions- und Callback-Ausführung.
/// Fängt Exceptions in Event-Handlern und Game-Hooks sicher ab, sodass
/// einzelne Mod-Fehler nach Spiel-Updates niemals die Unity-Hauptschleife oder andere Mods stoppen.
/// </summary>
public static class SafeInvoker
{
    /// <summary>
    /// Führt eine Action gekapselt in einem Try-Catch-Block aus.
    /// </summary>
    public static bool Execute(Action action, ModLogger? log = null, string context = "Action")
    {
        if (action == null)
            return false;

        try
        {
            action();
            return true;
        }
        catch (Exception ex)
        {
            if (log != null) log.Error($"Exception in {context} abgefangen (Game-Loop bleibt stabil)", ex);
            else try { MelonLoader.MelonLogger.Error($"[SafeInvoker:{context}] {ex}"); } catch { }
            return false;
        }
    }

    /// <summary>
    /// Führt eine Funktion mit Rückgabewert gekapselt aus und gibt bei Fehlern einen Fallback zurück.
    /// </summary>
    public static TResult Execute<TResult>(Func<TResult> func, TResult fallback, ModLogger? log = null, string context = "Function")
    {
        if (func == null)
            return fallback;

        try
        {
            return func();
        }
        catch (Exception ex)
        {
            if (log != null) log.Error($"Exception in {context} abgefangen", ex);
            else try { MelonLoader.MelonLogger.Error($"[SafeInvoker:{context}] {ex}"); } catch { }
            return fallback;
        }
    }

    /// <summary>
    /// Führt eine parametrisierte Action gekapselt aus.
    /// </summary>
    public static bool Execute<T>(Action<T> action, T parameter, ModLogger? log = null, string context = "Action")
    {
        if (action == null)
            return false;

        try
        {
            action(parameter);
            return true;
        }
        catch (Exception ex)
        {
            if (log != null) log.Error($"Exception in {context} abgefangen", ex);
            else try { MelonLoader.MelonLogger.Error($"[SafeInvoker:{context}] {ex}"); } catch { }
            return false;
        }
    }
}
