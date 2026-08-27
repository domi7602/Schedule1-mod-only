---
name: schedule1-il2cpp-reflection
description: >-
  Runtime-reflection recipes for MelonLoader IL2CPP mods in Schedule I v0.4.6f13. Use when IL2CPP bindings are missing or wrong (Il2CppStructArray vs byte[], Sprite[] vs Il2CppReferenceArray), when patching game types that may move namespaces, or when reading/writing private fields across Mono/IL2CPP runtime.
  Keywords: Il2CppStructArray, Il2CppReferenceArray, Il2CppInterop, RuntimeReflection, byte[] overload, Texture2D.LoadImage, Sprite array, type cache, namespace fallback.
---

# Schedule I — IL2CPP Runtime-Reflection Recipes

> **Source:** distilled from decompiling `CustomLoadingScreens 1.1.0`, `Unofficial Map Enhancer 1.0.0`, and the Shared `TypeResolver`. Last verified 2026-08-26.
>
> **Why this exists:** The S1API/Sideload ecosystem covers 95% of game interactions via pre-bound types. The remaining 5% — image loading, sprite arrays in private fields, dynamic UI discovery, namespace drift between game patches — all need **runtime reflection**. Our `Shared/TypeResolver` is a thin wrapper; this skill is the cookbook for what to do when the wrapper isn't enough.

---

## 1. When to Use (Decision Tree)

```
Need to call a Unity API or read/write a game-class field?
│
├─ S1API has it? → use S1API. Stop.
├─ Direct IL2CPP binding compiles cleanly? → use direct call. Stop.
│
└─ Need runtime reflection because...:
   ├─ Wrong array type (Il2CppStructArray<T> vs T[])?  → see §3 (Array Bridging)
   ├─ Missing overload (e.g. byte[] version of LoadImage)? → see §4 (Missing Overload)
   ├─ Namespace may drift between game patches?  → see §5 (Namespace Fallback)
   ├─ Reading/writing private members on IL2CPP objects? → see §6 (Member Access)
   └─ Materializing Il2CppList<T> / List<T> to managed list? → see §7 (Collection Materialization)
```

---

## 2. Core Setup

```csharp
using System.Reflection;
using System.Reflection.Emit;          // optional, for IL emit
using System.Collections.Concurrent;
using Il2CppInterop.Runtime;           // Il2CppInterop.Runtime assembly
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
```

**Critical first check** — is this an IL2CPP runtime?

```csharp
private static bool IsIl2CppRuntime() =>
    AppDomain.CurrentDomain.GetAssemblies()
        .Any(a => string.Equals(a.GetName().Name, "Il2CppInterop.Runtime", StringComparison.Ordinal));
```

If `false`, skip all `Il2CppStructArray`/`Il2CppReferenceArray` wrapping — just use `byte[]` and `T[]` directly.

---

## 3. Array Bridging (T[] ↔ Il2CppXxxArray<T>)

IL2CPP exposes game fields as `Il2CppReferenceArray<T>` (reference type) or `Il2CppStructArray<T>` (struct type). When you need to write a managed `T[]` into such a field, the **constructor of the Il2Cpp array takes the managed array directly**:

```csharp
// Pattern A: Sprite[] → Il2CppReferenceArray<Sprite>
Il2CppReferenceArray<Sprite> WrapRef(Sprite[] managed, Type targetType) {
    var ctor = targetType.GetConstructor(new[] { typeof(Sprite[]) });
    if (ctor == null) throw new InvalidOperationException($"No (Sprite[]) ctor on {targetType}");
    return (Il2CppReferenceArray<Sprite>)ctor.Invoke(new object[] { managed });
}

// Pattern B: byte[] → Il2CppStructArray<byte>  (REQUIRED for UnityEngine.ImageConversion.LoadImage in IL2CPP)
Il2CppStructArray<byte> WrapStruct(byte[] managed) {
    var arrayType = typeof(Il2CppStructArray<byte>);          // built-in
    var ctor = arrayType.GetConstructor(new[] { typeof(byte[]) });
    return (Il2CppStructArray<byte>)ctor.Invoke(new object[] { managed });
}
```

**Verify with `MemberType`** — don't assume, reflect the actual field type:

```csharp
MemberInfo member = type.GetField(name, BindingFlags.NonPublic | BindingFlags.Instance);
Type memberType = (member as FieldInfo)?.FieldType ?? (member as PropertyInfo)?.PropertyType;
if (memberType.FullName?.StartsWith("Il2CppInterop.Runtime.InteropTypes.Arrays.Il2CppReferenceArray`1", StringComparison.Ordinal) == true) {
    // wrap and assign via reflection
}
```

If you try to assign a managed `T[]` directly to an `Il2CppReferenceArray<T>` field, the IL2CPP bridge throws `InvalidCastException` at runtime.

---

## 4. Missing Overload (byte[] version of UnityEngine.ImageConversion.LoadImage)

**Problem:** In IL2CPP, `ImageConversion.LoadImage(Texture2D, byte[])` may not have a direct binding — only `(Texture2D, Il2CppStructArray<byte>)`. Compiling against `byte[]` throws `MissingMethodException`.

**Solution:** Discover the method at runtime and build a delegate wrapper. Verified recipe from `CustomLoadingScreens/Utils/RuntimeReflection.cs`:

```csharp
private static MethodInfo FindLoadImageMethod() {
    var allTypes = AppDomain.CurrentDomain.GetAssemblies()
        .SelectMany(a => { try { return a.GetTypes(); } catch (ReflectionTypeLoadException ex) { return ex.Types.OfType<Type>(); } });
    var imageConversion = allTypes.FirstOrDefault(t => t.Name == "ImageConversion" && t.Namespace == "UnityEngine");
    if (imageConversion == null) throw new InvalidOperationException("UnityEngine.ImageConversion not found");

    // IL2CPP signature uses Il2CppStructArray<byte>; Mono uses byte[].
    return imageConversion.GetMethods(BindingFlags.Public | BindingFlags.Static)
        .Where(m => m.Name == "LoadImage" && m.ReturnType == typeof(bool))
        .FirstOrDefault(m => {
            var p = m.GetParameters();
            return p.Length is 2 or 3
                && p[0].ParameterType == typeof(Texture2D)
                && (p[1].ParameterType == typeof(byte[]) || IsIl2CppByteArray(p[1].ParameterType))
                && (p.Length == 2 || p[2].ParameterType == typeof(bool));
        });
}

private static bool IsIl2CppByteArray(Type t) =>
    t.IsGenericType
    && t.GetGenericTypeDefinition().FullName == "Il2CppInterop.Runtime.InteropTypes.Arrays.Il2CppStructArray`1"
    && t.GenericTypeArguments.Length == 1
    && t.GenericTypeArguments[0] == typeof(byte);

public static bool TryLoadImage(Texture2D tex, byte[] bytes) {
    var m = FindLoadImageMethod();
    var p = m.GetParameters();
    object arg2 = IsIl2CppByteArray(p[1].ParameterType)
        ? typeof(Il2CppStructArray<byte>).GetConstructor(new[] { typeof(byte[]) })!.Invoke(new object[] { bytes })
        : (object)bytes;
    var result = p.Length == 2
        ? m.Invoke(null, new[] { tex, arg2 })
        : m.Invoke(null, new[] { tex, arg2, false });
    return Convert.ToBoolean(result);
}
```

**Use case:** When loading user-supplied PNG/JPG bytes into a runtime-created `Texture2D` (loading screens, save thumbnails, photo frames). Note that `Texture2D.LoadImage` itself also exists and is the preferred path when available — try it first.

---

## 5. Namespace Fallback (Resilient Type Resolution)

Game patches move types between namespaces. Rather than hardcoding `Il2CppScheduleOne.UI.LoadingScreen`, try a list:

```csharp
public static readonly string[] LoadingScreenCandidates = new[] {
    "ScheduleOne.UI.LoadingScreen",                  // decompiled C# name
    "Il2CppScheduleOne.UI.LoadingScreen",            // IL2CPP runtime name
    // add newer paths here as game patches ship
};

public static Type? ResolveGameType(IEnumerable<string> typeNames) {
    foreach (var name in typeNames) {
        if (TypeCache.TryGetValue(name, out var cached)) return cached;
        foreach (var asm in AppDomain.CurrentDomain.GetAssemblies()) {
            var t = asm.GetType(name, throwOnError: false, ignoreCase: false);
            if (t != null) {
                TypeCache[name] = t;
                return t;
            }
        }
    }
    return null;
}

// pair with HarmonyPrepare guard
[HarmonyPrepare]
private static bool Prepare() => ResolveGameType(GameTypeNames.LoadingScreenCandidates) != null
                              && AccessTools.Method(ResolveGameType(GameTypeNames.LoadingScreenCandidates), "Open", new[] { typeof(bool) }) != null;
```

Cache with `ConcurrentDictionary<string, Type?>` — see §6 of `Shared/TypeResolver` for the production version. **Never** use `Activator.CreateInstance` to discover types: IL2CPP instances need `Il2CppType.Of` registration.

---

## 6. Private Member Access (Field/Property by Name)

When the API surface is hostile but you know the internal field name:

```csharp
private static readonly ConcurrentDictionary<(Type, string), MemberInfo?> MemberCache = new();

public static object? GetMemberValue(object? instance, string name) {
    if (instance == null) return null;
    var member = ResolveMember(instance.GetType(), name);
    return member switch {
        PropertyInfo p => p.GetValue(instance),
        FieldInfo f    => f.GetValue(instance),
        _ => null
    };
}

public static void SetMemberValue(object instance, string name, object? value) {
    var member = ResolveMember(instance.GetType(), name);
    if (member is PropertyInfo { CanWrite: true } p) p.SetValue(instance, value);
    else if (member is FieldInfo f) f.SetValue(instance, value);
}

private static MemberInfo? ResolveMember(Type type, string name) =>
    MemberCache.GetOrAdd((type, name), key => FindMember(key.Item1, key.Item2));

private static MemberInfo? FindMember(Type type, string name) {
    for (var t = type; t != null; t = t.BaseType) {
        var p = t.GetProperty(name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
        if (p != null) return p;
        var f = t.GetField(name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
        if (f != null) return f;
    }
    return null;
}
```

**Always walk the base-class chain** — IL2CPP classes inherit proxies that hide inherited members in single-shot `GetProperty/GetField` calls. The `t.BaseType` loop is non-negotiable.

---

## 7. Materializing Il2Cpp Collections

`Il2CppList<T>` and `Il2CppReferenceArray<T>` are NOT `IEnumerable` in the managed sense. You need to use `Count` + indexer:

```csharp
public static List<object> Materialize(object il2cppCollection) {
    var result = new List<object>();
    var count = Convert.ToInt32(GetMemberValue(il2cppCollection, "Count"));
    var item = il2cppCollection.GetType().GetProperty("Item", new[] { typeof(int) });
    for (int i = 0; i < count; i++) result.Add(item.GetValue(il2cppCollection, new object[] { i }));
    return result;
}
```

**Better path:** use `Il2CppSystem.Collections.Generic.List<T>`'s `ToArray()` if available, or cast to `IEnumerable` and let `Il2CppInterop` marshal — but only when the runtime support is confirmed. The reflection path above always works.

---

## 8. Pitfalls

| Symptom | Cause | Fix |
|---|---|---|
| `MissingMethodException` on `ImageConversion.LoadImage(tex, bytes)` | IL2CPP only binds the `Il2CppStructArray<byte>` overload | Runtime discovery (§4) |
| `InvalidCastException` assigning `Sprite[]` to game field | Field is `Il2CppReferenceArray<Sprite>` | Reflect field type, wrap via ctor (§3) |
| Patch silently no-ops after game update | Type moved namespace | Namespace-fallback list (§5) |
| `ReflectionTypeLoadException` on `Assembly.GetTypes()` | Forward reference in a dependency | `try { GetTypes() } catch (ReflectionTypeLoadException ex) { return ex.Types.OfType<Type>(); }` |
| Inherited private field returns `null` | Base-class proxy hides it | Walk `t.BaseType` chain (§6) |
| `GetProperty("Item", int)` returns `null` | `Item` is on a base, not the concrete type | Same as above |

---

## 9. References

- `Source/Mods/Shared/src/TypeResolver.cs` — production wrapper (uses §5 + §6)
- Live source of patterns above: decompiled `CustomLoadingScreens/CustomLoadingScreens.Utils/RuntimeReflection.cs` and `CustomLoadingScreens/CustomLoadingScreens.Integrations.Interop/RuntimeInterop.cs` (kept under `.scratch/mod-decompile/_decompiled/` for reference)
- S1API 3.2.0 already wraps most of this — always check S1API's `Internal/Utils` before rolling your own
- Harmony 2.x docs: `[HarmonyPrepare]` + `[HarmonyTargetMethod]` for runtime target resolution (see `schedule1-modding` §5)
