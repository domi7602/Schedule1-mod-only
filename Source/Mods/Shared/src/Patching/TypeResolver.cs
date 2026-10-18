using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace S1Mods.Shared;

/// <summary>
/// Resilient type resolution across all loaded assemblies.
/// Prevents binding breakage when developers or Unity game patches move classes
/// between assemblies (e.g. Assembly-CSharp vs S1API/Plugins).
/// </summary>
public static class TypeResolver
{
    private static readonly Dictionary<string, Type?> _typeCache = new(StringComparer.Ordinal);
    private static readonly object _lock = new();

    /// <summary>
    /// Late-loaded mod assemblies (S1API plugins, user mods) resolve types that were
    /// looked up — and cached as null — before they were loaded. Subscribing to
    /// AssemblyLoad lets us drop only the null entries on every new assembly; resolved
    /// hits stay valid (a type does not move once its assembly is loaded).
    /// Rooted in a static readonly field so the IL2CPP GC cannot collect the delegate.
    /// </summary>
    private static readonly AssemblyLoadEventHandler _onAssemblyLoad = (_, _) =>
    {
        lock (_lock)
        {
            foreach (var key in _typeCache.Where(kv => kv.Value == null).Select(kv => kv.Key).ToList())
                _typeCache.Remove(key);
        }
    };

    static TypeResolver()
    {
        AppDomain.CurrentDomain.AssemblyLoad += _onAssemblyLoad;
    }

    /// <summary>
    /// Finds a type by its full name (e.g. "ScheduleOne.Skateboarding.Skateboard")
    /// or its simple class name (e.g. "Skateboard") across all loaded assemblies.
    /// </summary>
    public static Type? Find(string typeName, ModLogger? log = null)
    {
        return Find(typeName, null, log);
    }

    /// <summary>
    /// Finds a type by its name with an optional assembly name hint (e.g. "Assembly-CSharp").
    /// </summary>
    public static Type? Find(string typeName, string? assemblyHint, ModLogger? log = null)
    {
        if (string.IsNullOrWhiteSpace(typeName))
            return null;

        string cacheKey = string.IsNullOrEmpty(assemblyHint) ? typeName : $"{assemblyHint}:{typeName}";

        lock (_lock)
        {
            if (_typeCache.TryGetValue(cacheKey, out Type? cached))
                return cached;
        }

        Type? resolved = ResolveTypeInternal(typeName, assemblyHint);

        lock (_lock)
        {
            _typeCache[cacheKey] = resolved;
        }

        if (resolved == null)
        {
            log?.Warn($"TypeResolver: Type '{typeName}' could not be found in any loaded assembly.");
        }
        else
        {
            log?.Debug($"TypeResolver: Type '{typeName}' resolved in assembly '{resolved.Assembly.GetName().Name}'.");
        }

        return resolved;
    }

    /// <summary>
    /// Clears the internal type cache (e.g. after dynamic loading of assemblies).
    /// </summary>
    public static void ClearCache()
    {
        lock (_lock)
        {
            _typeCache.Clear();
        }
    }

    private static Type? ResolveTypeInternal(string typeName, string? assemblyHint)
    {
        Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

        // 1. Prioritized search with assembly hint
        if (!string.IsNullOrEmpty(assemblyHint))
        {
            foreach (Assembly asm in assemblies)
            {
                if (asm.GetName().Name?.IndexOf(assemblyHint, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    Type? t = FindTypeInAssembly(asm, typeName);
                    if (t != null)
                        return t;
                }
            }
        }

        // 2. Direct Type.GetType check
        try
        {
            Type? direct = Type.GetType(typeName, false);
            if (direct != null)
                return direct;
        }
        catch
        {
            // Ignored
        }

        // 3. Breadth-first search across all loaded assemblies
        foreach (Assembly asm in assemblies)
        {
            Type? t = FindTypeInAssembly(asm, typeName);
            if (t != null)
                return t;
        }

        return null;
    }

    private static Type? FindTypeInAssembly(Assembly asm, string typeName)
    {
        try
        {
            // Direct attempt via Assembly.GetType
            Type? direct = asm.GetType(typeName, false, true);
            if (direct != null)
                return direct;

            // Fallback to separate scan (e.g. for simple class names without namespace)
            Type[] types = asm.GetTypes();
            for (int i = 0; i < types.Length; i++)
            {
                Type t = types[i];
                if (string.Equals(t.FullName, typeName, StringComparison.Ordinal) ||
                    string.Equals(t.Name, typeName, StringComparison.Ordinal))
                {
                    return t;
                }
            }
        }
        catch (ReflectionTypeLoadException ex)
        {
            // Query types that could be loaded
            if (ex.Types != null)
            {
                foreach (Type? t in ex.Types)
                {
                    if (t == null)
                        continue;

                    if (string.Equals(t.FullName, typeName, StringComparison.Ordinal) ||
                        string.Equals(t.Name, typeName, StringComparison.Ordinal))
                    {
                        return t;
                    }
                }
            }
        }
        catch
        {
            // Skip faulty/incompatible assembly
        }

        return null;
    }
}
