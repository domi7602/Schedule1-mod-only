using System;
using System.IO;
using System.Reflection;
using S1Mods.Shared;
using UnityEngine;

#if (IL2CPPMELON)
using S1ItemFramework = Il2CppScheduleOne.ItemFramework;
using S1Registry = Il2CppScheduleOne.Registry;
#elif MONOMELON
using S1ItemFramework = ScheduleOne.ItemFramework;
using S1Registry = ScheduleOne.Registry;
#endif

namespace HitmanPhone.Items;

/// <summary>
/// Registers the Polaroid evidence item used by the Hitman-Phone bounty workflow.
///
/// We piggyback on Schedule I's <c>IntegerItemDefinition</c> (which already exists in
/// the engine) and store the bounty target's <c>UnityEngine.Object.GetInstanceID()</c>
/// in the <c>Value</c> field of the spawned <c>IntegerItemInstance</c>. Dead-drop
/// validation later reads <c>IntegerItemInstance.Value</c> and matches it back to the
/// active contract — no custom item subclass required (see Spec v3 §Phase B).
///
/// Caveat: the public surface of <c>StorableItemDefinition</c> in
/// <c>MelonLoader/Il2CppAssemblies/Assembly-CSharp.dll</c> is stub-only (C++ bodies
/// live in <c>GameAssembly.dll</c>), so we discover the inherited properties via
/// reflection at runtime. This means a future game version may rename them; the
/// registry tries each candidate and logs which one worked.
/// </summary>
public static class BountyEvidenceItemRegistry
{
    public const string ItemId = "bounty_evidence_polaroid";
    public const string ItemName = "Polaroid Evidence";
    public const string ItemDescription =
        "A photo taken at the scene of a bounty. The Unity instance id of the " +
        "target is encoded in the polaroid's data field. Drop it at any dead-drop " +
        "to collect the reward.";

    /// <summary>Cached sprite for the polaroid icon (loaded from disk in <see cref="Register"/>).</summary>
    public static Sprite? IconSprite { get; private set; }

    /// <summary>The registered definition. Null until <see cref="Register"/> succeeds.</summary>
    public static S1ItemFramework.IntegerItemDefinition? Definition { get; private set; }

    public static bool IsRegistered => Definition != null && IsAlive(Definition);

    /// <summary>
    /// IL2CPP-safe liveness check for the cached Unity objects. A scene change
    /// can destroy the native Definition/Sprite while the managed static stays
    /// non-null — using it then throws. Mirrors the Pointer/WasCollected
    /// pattern in <see cref="Bounty.BountyReceiptService"/>.
    /// </summary>
    private static bool IsAlive(UnityEngine.Object? obj)
    {
#if (IL2CPPMELON)
        return obj != null && obj.Pointer != System.IntPtr.Zero && !obj.WasCollected;
#else
        return obj != null;
#endif
    }

    /// <summary>
    /// Registers the polaroid in the game registry if it has not been registered yet.
    /// Idempotent — calling it twice is a no-op.
    /// </summary>
    public static void Register()
    {
        if (IsRegistered) return;
        Definition = null; // drop a scene-destroyed reference so the retry below starts clean

        if (!IsAlive(IconSprite))
        {
            IconSprite = LoadPolaroidSprite();
            if (IconSprite == null)
            {
                Mod.Log.Warn("Polaroid sprite could not be loaded; " +
                             "the item will be created with a fallback (null) icon.");
            }
        }

        try
        {
            var def = ScriptableObject.CreateInstance<S1ItemFramework.IntegerItemDefinition>();

            // Skill Rule: Prevent Unity from destroying our ScriptableObject 
            // when transitioning from the Main Menu to the Gameplay scene.
            def.hideFlags = HideFlags.HideAndDontSave;

            // Direct property on IntegerItemDefinition.
            def.DefaultValue = 0;

            // Inherited properties — set via reflection because the stub-decompile doesn't
            // expose them as typed accessors. We log every successful match.
            TrySetReflected(def, "ID", ItemId);
            TrySetReflected(def, "Name", ItemName);
            TrySetReflected(def, "Description", ItemDescription);
            TrySetReflected(def, "StackLimit", 1);
            TrySetReflected(def, "BasePurchasePrice", 0f);
            TrySetReflected(def, "ResellMultiplier", 0f);
            TrySetReflected(def, "Icon", IconSprite);

            // Best-effort category. S1API exposes S1API.Items.ItemCategory as a friendly wrapper;
            // we try setting it both as the wrapper type (cast to object) and as the underlying int.
            TrySetCategory(def, S1API.Items.ItemCategory.Decoration);

            // Best-effort native registry injection. If this fails, direct spawning still works.
            RegisterIntoNativeRegistry(def);

            Definition = def;
            Mod.Log.Info($"Polaroid '{ItemId}' registered " +
                          $"(stack=1, icon={(IconSprite != null ? "loaded" : "missing")}).");
        }
        catch (Exception ex)
        {
            Mod.Log.Error($"Failed to register Polaroid item: {ex}");
        }
    }

    /// <summary>
    /// Spawn a polaroid instance pointing at the given target's Unity InstanceID.
    /// Returns null if the definition has not been registered yet.
    /// </summary>
    public static S1ItemFramework.IntegerItemInstance? Spawn(int targetInstanceId)
    {
        if (Definition == null || !IsAlive(Definition))
        {
            // First spawn in this session, or the cached Definition died with a
            // scene change: register on demand. Expected path, not a failure.
            Mod.Log.Debug("Polaroid Definition was null or destroyed during Spawn() — attempting lazy re-registration.");
            Definition = null;
            Register();
        }

        if (Definition == null || !IsAlive(Definition))
        {
            Mod.Log.Warn("Polaroid Spawn() failed: Definition is still null or destroyed after re-registration.");
            return null;
        }
        var instance = new S1ItemFramework.IntegerItemInstance(Definition, 1, targetInstanceId);
        instance.SetValue(targetInstanceId);
        return instance;
    }

    // --- helpers ---

    private static void TrySetReflected(object target, string propName, object value)
    {
        try
        {
            var prop = target.GetType().GetProperty(propName,
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (prop != null && prop.CanWrite)
            {
                prop.SetValue(target, value);
                Mod.Log.Debug($"Set {target.GetType().Name}.{propName}={value} (via reflection).");
                return;
            }
            var field = target.GetType().GetField(propName,
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (field != null)
            {
                field.SetValue(target, value);
                Mod.Log.Debug($"Set {target.GetType().Name}.{propName}={value} (via reflection field).");
                return;
            }
            Mod.Log.Warn($"Polaroid property/field '{propName}' not found on {target.GetType().Name} — skipping.");
        }
        catch (Exception ex)
        {
            Mod.Log.Warn($"TrySetReflected failed for {propName}: {ex.Message}");
        }
    }

    private static void TrySetCategory(object def, S1API.Items.ItemCategory category)
    {
        try
        {
            // Try setting Category via reflection. S1API exposes ItemCategory as a managed enum,
            // but the engine uses its own enum — value identity may not match. Cast via ToString
            // would not work; we just send the int value.
            int v = (int)category;
            TrySetReflected(def, "Category", v);
        }
        catch (Exception ex)
        {
            Mod.Log.Warn($"Category assignment skipped: {ex.Message}");
        }
    }

    private static Sprite? LoadPolaroidSprite()
    {
        try
        {
            string modDir = Path.GetDirectoryName(typeof(BountyEvidenceItemRegistry).Assembly.Location)
                            ?? string.Empty;
            string iconPath = Path.Combine(modDir, "polaroid.png");
            if (!File.Exists(iconPath))
            {
                Mod.Log.Error($"Polaroid PNG missing at '{iconPath}'.");
                return null;
            }
            byte[] bytes = File.ReadAllBytes(iconPath);
            var tex = new Texture2D(2, 2, TextureFormat.RGBA32, mipChain: false);
            tex.hideFlags = HideFlags.HideAndDontSave; // Protect texture from scene unload
            tex.name = "HitmanPhone_PolaroidIcon";
            if (!tex.LoadImage(bytes))
            {
                Mod.Log.Error($"Texture2D.LoadImage failed for '{iconPath}'.");
                return null;
            }
            tex.filterMode = FilterMode.Bilinear;
            var sprite = Sprite.Create(
                tex,
                new Rect(0, 0, tex.width, tex.height),
                new Vector2(0.5f, 0.5f),
                pixelsPerUnit: 100f);
            sprite.hideFlags = HideFlags.HideAndDontSave; // Protect sprite from scene unload
            sprite.name = "HitmanPhone_PolaroidIcon";
            return sprite;
        }
        catch (Exception ex)
        {
            Mod.Log.Error($"Exception while loading polaroid sprite: {ex}");
            return null;
        }
    }

    /// <summary>
    /// Best-effort native registry injection. We try <c>Registry.AddItem</c> / <c>Register</c>
    /// via reflection because the accessor naming varies between patches. If nothing works
    /// the spec still falls back to manual spawning — the item is registered only so
    /// that ID-based lookups in <see cref="S1Registry"/> resolve; direct spawning works
    /// without registration.
    /// </summary>
    private static void RegisterIntoNativeRegistry(S1ItemFramework.IntegerItemDefinition def)
    {
        try
        {
            var registryType = typeof(S1Registry);
            string[] candidates = { "AddToRegistry", "AddItemToRegistry", "Register", "AddItem", "Add" };
            bool added = false;
            foreach (string name in candidates)
            {
                var mi = registryType.GetMethod(name,
                    BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static,
                    binder: null,
                    types: new[] { typeof(S1ItemFramework.ItemDefinition) },
                    modifiers: null);
                if (mi != null)
                {
                    object? instance = mi.IsStatic ? null : S1Registry.Instance;
                    if (!mi.IsStatic && instance == null) continue;

                    mi.Invoke(instance, new object[] { def });
                    added = true;
                    Mod.Log.Info($"Polaroid registered via {registryType.Name}.{name}().");
                    break;
                }
            }
            if (!added)
            {
                Mod.Log.Warn("No known Registry.Add method found — polaroid will still spawn " +
                             "directly via IntegerItemInstance(definition, qty, value).");
            }
        }
        catch (Exception ex)
        {
            Mod.Log.Warn($"Native registry injection failed (non-fatal): {ex.Message}");
        }
    }
}
