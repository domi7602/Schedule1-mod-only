using System;
using System.Collections.Generic;
using System.Reflection;

namespace S1Mods.Shared;

/// <summary>
/// Resiliente Typauflösung über alle geladenen Assemblies.
/// Verhindert Bindungsbrüche, wenn Entwickler oder Unity-Game-Patches Klassen
/// zwischen Assemblies verschieben (z.B. Assembly-CSharp vs S1API/Plugins).
/// </summary>
public static class TypeResolver
{
    private static readonly Dictionary<string, Type?> _typeCache = new(StringComparer.Ordinal);
    private static readonly object _lock = new();

    /// <summary>
    /// Sucht einen Typ anhand seines vollen Namens (z. B. "ScheduleOne.Skateboarding.Skateboard")
    /// oder seines einfachen Klassennamens (z. B. "Skateboard") über alle geladenen Assemblies.
    /// </summary>
    public static Type? Find(string typeName, ModLogger? log = null)
    {
        return Find(typeName, null, log);
    }

    /// <summary>
    /// Sucht einen Typ anhand seines Namens mit optionalem Assembly-Namenshinweis (z. B. "Assembly-CSharp").
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
            log?.Warn($"TypeResolver: Typ '{typeName}' konnte in keiner geladenen Assembly gefunden werden.");
        }
        else
        {
            log?.Debug($"TypeResolver: Typ '{typeName}' aufgelöst in Assembly '{resolved.Assembly.GetName().Name}'.");
        }

        return resolved;
    }

    /// <summary>
    /// Leert den internen Typ-Cache (z. B. nach dynamischem Nachladen von Assemblies).
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

        // 1. Priorisierte Suche mit Assembly-Hint
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

        // 2. Direkte Type.GetType-Prüfung
        try
        {
            Type? direct = Type.GetType(typeName, false);
            if (direct != null)
                return direct;
        }
        catch
        {
            // Ignorieren
        }

        // 3. Breitensuche über alle geladenen Assemblies
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
            // Direkter Versuch via Assembly.GetType
            Type? direct = asm.GetType(typeName, false, true);
            if (direct != null)
                return direct;

            // Fallback auf getrennten Scan (z.B. bei einfachem Klassennamen ohne Namespace)
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
            // Typen abfragen, die geladen werden konnten
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
            // Fehlerhafte/inkompatible Assembly überspringen
        }

        return null;
    }
}
