using System;

namespace S1Mods.Shared;

/// <summary>
/// Isolated action and callback execution.
/// Safely catches exceptions in event handlers and game hooks so that
/// individual mod errors after game updates never stop the Unity main loop or other mods.
/// </summary>
public static class SafeInvoker
{
    /// <summary>
    /// Executes an action encapsulated in a try-catch block.
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
            if (log != null) log.Error($"Exception in {context} caught (game loop stays stable)", ex);
            else try { MelonLoader.MelonLogger.Error($"[SafeInvoker:{context}] {ex}"); } catch { }
            return false;
        }
    }

    /// <summary>
    /// Executes a function with return value encapsulated and returns a fallback on error.
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
            if (log != null) log.Error($"Exception in {context} caught", ex);
            else try { MelonLoader.MelonLogger.Error($"[SafeInvoker:{context}] {ex}"); } catch { }
            return fallback;
        }
    }

    /// <summary>
    /// Executes a parameterized action encapsulated.
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
            if (log != null) log.Error($"Exception in {context} caught", ex);
            else try { MelonLoader.MelonLogger.Error($"[SafeInvoker:{context}] {ex}"); } catch { }
            return false;
        }
    }
}
