using System;
using System.Collections.Generic;
using System.Reflection;
using MelonLoader;

namespace S1Mods.Shared;

/// <summary>
/// Persists properties of a config class (T) via MelonPreferences.
/// Usage: ModConfig&lt;T&gt;.Initialize("Category", log) in OnInitializeMelon, then
/// ModConfig&lt;T&gt;.Instance.X / .SetAndSave("X", v) / .Save().
/// </summary>
public static class ModConfig<T> where T : class, new()
{
    private static MelonPreferences_Category? _cat;
    private static T? _instance;
    private static ModLogger? _logger;
    private static string _categoryId = "";
    private static readonly Dictionary<string, MelonPreferences_Entry> _entryCache = new();
    private static readonly Dictionary<string, PropertyInfo> _propertyCache = new();
    private static PropertyInfo[] _cachedProperties = Array.Empty<PropertyInfo>();

    public static event Action<string, object?>? OnChanged;

    public static ModLogger? Logger
    {
        get => _logger;
        set => _logger = value;
    }

    public static T Instance
    {
        get
        {
            EnsureInitialized();
            if (_instance == null)
                Load();
            return _instance!;
        }
    }

    public static void Initialize(string categoryId = "", ModLogger? log = null)
    {
        _logger = log;
        _categoryId = string.IsNullOrEmpty(categoryId) ? typeof(T).Name : categoryId;
        _entryCache.Clear();
        _propertyCache.Clear();
        _cachedProperties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
        foreach (var p in _cachedProperties)
        {
            _propertyCache[p.Name] = p;
        }

        try
        {
            _cat = MelonPreferences.CreateCategory(_categoryId);
        }
        catch (Exception ex) when (ex.Message.Contains("already exists") || ex.Message.Contains("existiert"))
        {
            try
            {
                // Fallback: try to get existing category via reflection (API varies across ML versions)
                var getCat = typeof(MelonPreferences).GetMethod("GetCategory", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
                if (getCat != null)
                {
                    _cat = (MelonPreferences_Category?)getCat.Invoke(null, new object[] { _categoryId });
                }
            }
            catch { }
            if (_cat == null)
            {
                LogWarn($"CreateCategory '{_categoryId}' failed (already exists) and GetCategory unavailable: {ex.Message}. Reusing previous category if any.");
                // If we already had one from previous init, keep it; otherwise try to continue with null and Load will early-return
            }
        }
        Load();
    }

    public static void Save()
    {
        EnsureInitialized();
        if (_instance == null)
            Load();

        foreach (PropertyInfo prop in _cachedProperties)
        {
            if (!prop.CanRead || !prop.CanWrite)
                continue;

            MelonPreferences_Entry? entry = GetOrCreateEntry(prop);
            if (entry == null)
                continue;

            try
            {
                entry.BoxedValue = prop.GetValue(_instance);
            }
            catch (Exception ex)
            {
                LogError($"'{prop.Name}' could not be saved", ex);
            }
        }

        _cat!.SaveToFile(true);
    }

    public static void SetAndSave(string propertyName, object? value)
    {
        if (!_propertyCache.TryGetValue(propertyName, out PropertyInfo? prop) || !prop.CanWrite)
            return;

        object? convertedVal = value;
        try
        {
            Type targetType = prop.PropertyType;
            convertedVal = value;

            if (value != null && !targetType.IsAssignableFrom(value.GetType()))
            {
                if (targetType.IsEnum)
                {
                    convertedVal = value is string s ? Enum.Parse(targetType, s, true) : Enum.ToObject(targetType, value);
                }
                else
                {
                    convertedVal = Convert.ChangeType(value, targetType);
                }
            }

            prop.SetValue(Instance, convertedVal);
        }
        catch (Exception ex)
        {
            LogError($"'{propertyName}' could not be set", ex);
            return;
        }

        Save();
        OnChanged?.Invoke(propertyName, convertedVal);
    }

    public static void ResetToDefaults()
    {
        EnsureInitialized();
        _instance = new T();
        Save();
    }

    private static void Load()
    {
        var fresh = new T();
        _instance = fresh;
        if (_cat == null)
            return;

        foreach (PropertyInfo prop in _cachedProperties)
        {
            if (!prop.CanRead || !prop.CanWrite)
                continue;

            MelonPreferences_Entry? entry = GetOrCreateEntry(prop);
            if (entry?.BoxedValue == null)
                continue;

            try
            {
                object? stored = entry.BoxedValue;
                if (stored != null)
                {
                    Type targetType = prop.PropertyType;
                    Type storedType = stored.GetType();

                    if (targetType.IsAssignableFrom(storedType))
                    {
                        prop.SetValue(_instance, stored);
                    }
                    else if (targetType.IsEnum)
                    {
                        try
                        {
                            object enumVal = stored is string str
                                ? Enum.Parse(targetType, str, true)
                                : Enum.ToObject(targetType, stored);
                            prop.SetValue(_instance, enumVal);
                        }
                        catch
                        {
                            LogWarn($"Enum mismatch for '{prop.Name}': '{stored}' ({storedType.Name}) does not match {targetType.Name}. Default value retained.");
                        }
                    }
                    else
                    {
                        try
                        {
                            object converted = Convert.ChangeType(stored, targetType);
                            prop.SetValue(_instance, converted);
                        }
                        catch (Exception ex)
                        {
                            LogWarn($"Type mismatch for '{prop.Name}': Stored={storedType.Name}, Expected={targetType.Name}. Conversion failed ({ex.Message}). Default value retained.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogError($"Stored value for '{prop.Name}' is not applicable (default retained)", ex);
            }
        }
    }

    private static bool IsNonMappableType(Type t)
    {
        if (typeof(System.Collections.IDictionary).IsAssignableFrom(t)) return true;
        if (t.IsGenericType)
        {
            var def = t.GetGenericTypeDefinition();
            if (def == typeof(Dictionary<,>) || def == typeof(List<>) || def == typeof(HashSet<>)) return true;
        }
        return false;
    }

    private static MelonPreferences_Entry? GetOrCreateEntry(PropertyInfo prop)
    {
        if (_entryCache.TryGetValue(prop.Name, out MelonPreferences_Entry? cached))
            return cached;

        if (IsNonMappableType(prop.PropertyType))
        {
            LogWarn($"'{prop.Name}' ({prop.PropertyType.Name}) is not TOML-mappable (Dictionary/List) — skipped, use SafeStorage JSON sidecar (ConfigJsonStore).");
            return null;
        }

        if (_cat!.HasEntry(prop.Name))
        {
            MelonPreferences_Entry entry = _cat.GetEntry(prop.Name);
            _entryCache[prop.Name] = entry;
            return entry;
        }

        try
        {
            object? val = prop.GetValue(_instance);
            MelonPreferences_Entry entry = _cat.CreateEntry(prop.Name, val ?? Activator.CreateInstance(prop.PropertyType)!, null, false);
            _entryCache[prop.Name] = entry;
            return entry;
        }
        catch (Exception ex)
        {
            LogError($"'{prop.Name}' ({prop.PropertyType.Name}) is not TOML-mappable and will not be persisted", ex);
            return null;
        }
    }

    private static void EnsureInitialized()
    {
        if (_cat == null)
            throw new InvalidOperationException($"ModConfig<{typeof(T).Name}>.Initialize() must be called first.");
    }

    private static void LogError(string msg, Exception? ex = null)
    {
        if (_logger != null)
        {
            if (ex != null)
                _logger.Error(msg, ex);
            else
                _logger.Error(msg);
        }
        else
        {
            string cat = string.IsNullOrEmpty(_categoryId) ? typeof(T).Name : _categoryId;
            MelonLogger.Error($"[ModConfig:{cat}] {msg}" + (ex != null ? $": {ex.InnerException?.Message ?? ex.Message}" : ""));
        }
    }

    private static void LogWarn(string msg)
    {
        if (_logger != null)
        {
            _logger.Warn(msg);
        }
        else
        {
            string cat = string.IsNullOrEmpty(_categoryId) ? typeof(T).Name : _categoryId;
            MelonLogger.Warning($"[ModConfig:{cat}] {msg}");
        }
    }
}
