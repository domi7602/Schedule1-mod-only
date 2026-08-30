using System;
using System.Collections.Generic;
using System.IO;
using CustomSkateboard.Config;
using Il2CppScheduleOne;
using Il2CppScheduleOne.DevUtilities;
using Il2CppScheduleOne.Equipping;
using Il2CppScheduleOne.ItemFramework;
using Il2CppScheduleOne.Skating;
using MelonLoader.Utils;
using S1API.Items;
using S1API.Items.Storable;
using UnityEngine;
using GameItemDef = Il2CppScheduleOne.ItemFramework.ItemDefinition;
using ItemInstance = Il2CppScheduleOne.ItemFramework.ItemInstance;

namespace CustomSkateboard.Items;

/// <summary>
/// Factory responsible for cloning, tuning, and registering the custom Skateboard ItemDefinition and prefabs.
/// </summary>
public static class SkateboardItemFactory
{
    private static GameItemDef? _registeredItemDef;
    private static readonly HashSet<int> _tunedInstanceIds = new();

    public static GameItemDef? CustomSkateboardItem => (_registeredItemDef != null && _registeredItemDef.Pointer != IntPtr.Zero) ? _registeredItemDef : null;

    public static void ClearTuningState() { _tunedInstanceIds.Clear(); }

    /// <summary>
    /// Hot-path check: is this board instance one of our custom (tuned) boards?
    /// O(1) HashSet lookup — safe to call every physics step.
    /// </summary>
    public static bool IsInstanceTuned(int instanceId)
    {
        return instanceId != 0 && _tunedInstanceIds.Contains(instanceId);
    }

    public static bool IsCustomItem(ItemInstance? item)
    {
        if (item == null || item.Pointer == IntPtr.Zero) return false;
        try
        {
            // Gatekeeper-fix 2026-08-30 B12: unify ID compare to OrdinalIgnoreCase
            return item.Definition != null && string.Equals(item.Definition.ID, Mod.CurrentConfig.SkateboardId, StringComparison.OrdinalIgnoreCase);
        }
        catch
        {
            return false;
        }
    }

    public static bool IsCustomSkateboard(Skateboard board)
    {
        if (board == null || board.Pointer == IntPtr.Zero) return false;
        try
        {
            var eq = board.Equippable;
            return eq != null && eq.Pointer != IntPtr.Zero && IsCustomItem(eq.itemInstance);
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Creates and registers the custom Skateboard into the game's item registry.
    /// Idempotent: returns early on subsequent calls without re-registering (audit 2026-08-29).
    /// </summary>
    public static bool CreateAndRegister(SkateboardConfig config)
    {
        try
        {
            // Guard 1: in-process cached def is still valid
            if (_registeredItemDef != null && _registeredItemDef.Pointer != IntPtr.Zero)
            {
                return true;
            }

            // Guard 2: another mod (or earlier lifecycle phase) already registered it via the game's Registry
            try
            {
                _registeredItemDef = Registry.GetItem(config.SkateboardId);
                if (_registeredItemDef != null && _registeredItemDef.Pointer != IntPtr.Zero)
                {
                    Mod.Log.Info($"Found existing item registration for '{config.SkateboardId}'.");
                    return true;
                }
            }
            catch (Exception ex)
            {
                Mod.Log.Warn($"Registry.GetItem failed: {ex.Message}");
            }

            // Check if base skateboard exists (prefer Gold Skateboard, then Cruiser, Skateboard, Cheap)
            string[] candidateBases = new[] { "goldenskateboard", "goldskateboard", "cruiser", "skateboard", "cheapskateboard" };
            string? baseId = null;
            if (!string.IsNullOrEmpty(config.BaseItemIdOverride) && Registry.ItemExists(config.BaseItemIdOverride))
            {
                baseId = config.BaseItemIdOverride;
            }
            else
            {
                foreach (var c in candidateBases)
                {
                    if (Registry.ItemExists(c))
                    {
                        baseId = c;
                        break;
                    }
                }
            }

            if (string.IsNullOrEmpty(baseId))
            {
                Mod.Log.Warn($"Base skateboard not found. Tried: {string.Join(", ", candidateBases)}");
                return false;
            }

            Mod.Log.Info($"Using base '{baseId}' (override='{config.BaseItemIdOverride}') for '{config.SkateboardId}'...");

            // Load custom icon if present
            Sprite? customIcon = LoadCustomIcon();

            // Use S1API ItemCreator for clean runtime registration (Equipment category)
            var builder = S1API.Items.Storable.ItemCreator.CloneFrom(baseId)
                .WithBasicInfo(config.SkateboardId, config.SkateboardName, config.Description, ItemCategory.Equipment)
                .WithPricing(config.Price, 0.5f);

            if (customIcon != null)
            {
                builder.WithIcon(customIcon);
            }

            var storableItem = builder.Build();
            if (storableItem != null)
            {
                _registeredItemDef = Registry.GetItem(config.SkateboardId);
                // Gatekeeper-fix 2026-08-30 B8: verify Registry actually returned def, fix false-success
                if (_registeredItemDef == null || _registeredItemDef.Pointer == IntPtr.Zero)
                {
                    Mod.Log.Warn($"Build succeeded but Registry.GetItem('{config.SkateboardId}') returned null.");
                    return false;
                }
                Mod.Log.Info($"Registered '{config.SkateboardId}' ({config.SkateboardName}) successfully!");
                return true;
            }

            return false;
        }
        catch (Exception ex)
        {
            Mod.Log.Error($"Error creating skateboard definition: {ex.Message}");
            return false;
        }
    }

    public static void TuneSkateboard(Skateboard board, SkateboardConfig config, bool forceRetune = false, bool logStats = true)
    {
        if (board == null || board.Pointer == IntPtr.Zero) return;
        // Gatekeeper-fix B13: clamp invalid JSON values (TopSpeed 0/negative) before applying.
        try { config.Validate(); } catch { }
        // Gatekeeper-fix 2026-08-30 B13: validate/clamp config values before tuning
        config.TopSpeed_Kmh = Mathf.Clamp(config.TopSpeed_Kmh, 1f, 200f);
        config.PushCooldown = Mathf.Clamp(config.PushCooldown, 0.05f, 2f);
        config.PushForceMultiplier = Mathf.Clamp(config.PushForceMultiplier, 0.1f, 20f);
        config.PushForceDuration = Mathf.Clamp(config.PushForceDuration, 0.05f, 2f);
        config.JumpForce = Mathf.Clamp(config.JumpForce, 0.1f, 20f);
        config.TurnForce = Mathf.Clamp(config.TurnForce, 0.1f, 100f);
        config.BrakeForce = Mathf.Clamp(config.BrakeForce, 0f, 20f);
        config.AirMovementForce = Mathf.Clamp(config.AirMovementForce, 0f, 50f);
        // Gatekeeper-fix B10: cache terrain flag for hot-path prefixes (avoids Mod.CurrentConfig try/catch per tick).
        try { Visuals.SkateboardVisualPatches.SetDisableTerrainSlowdownCached(config.DisableTerrainSlowdown); } catch { }
        int instId = 0;
        try { instId = board.GetInstanceID(); } catch { }
        // H5: Use only InstanceID (pointer recycled after Destroy). If ID unavailable, don't cache — always retune.
        if (!forceRetune && instId != 0 && _tunedInstanceIds.Contains(instId)) return;
        // Gatekeeper-fix 2026-08-30 B3: defer cache until tune succeeds
        bool tuneOk = true;

        // 1. Direct instance fields (only on this specific board instance)
        try
        {
            board.TopSpeed_Kmh = config.TopSpeed_Kmh;
            board.PushForceMultiplier = config.PushForceMultiplier;
            board.PushForceDuration = config.PushForceDuration;
            board.PushDelay = config.PushCooldown;

            board.JumpForce = config.JumpForce;
            board.JumpDuration_Min = Mathf.Clamp(config.JumpDuration_Min, 0.1f, 1.5f);
            board.JumpDuration_Max = Mathf.Clamp(Mathf.Max(config.JumpDuration_Max, config.JumpDuration_Min), 0.1f, 1.5f);
            board.JumpForwardBoost = config.JumpForwardBoost;

            board.TurnForce = config.TurnForce;
            board.TurnChangeRate = config.TurnChangeRate;
            board.TurnReturnToRestRate = config.TurnReturnToRestRate;
            board.TurnSpeedBoost = config.TurnSpeedBoost;

            board.LateralFrictionForceMultiplier = config.LateralFrictionForceMultiplier;
            board.LongitudinalFrictionMultiplier = config.LongitudinalFrictionMultiplier;
            board.BrakeForce = config.BrakeForce;

            // Gatekeeper-fix 2026-08-30 B1: wire AirMovement config
            board.AirMovementEnabled = config.AirMovementEnabled;
            board.AirMovementForce = config.AirMovementForce;

            board.SlowOnTerrain = !config.DisableTerrainSlowdown;
        }
        catch (Exception ex)
        {
            tuneOk = false;
            Mod.Log.Warn($"Direct board physics setup notice: {ex.Message}");
        }

        // 2. Apply cached high-speed curves (static readonly, allocated once per process)
        try
        {
            // Gatekeeper-fix 2026-08-30 B4: clone curve to avoid mutating static cache
            board.TurnForceMap = new AnimationCurve(_cachedTurnCurve.keys);
        }
        catch (Exception ex)
        {
            tuneOk = false;
            Mod.Log.Warn($"TurnForceMap curve assignment notice: {ex.Message}");
        }

        try
        {
            // Gatekeeper-fix 2026-08-30 B4: clone curve to avoid mutating static cache
            board.PushForceMultiplierMap = new AnimationCurve(_cachedPushCurve.keys);
        }
        catch (Exception ex)
        {
            tuneOk = false;
            Mod.Log.Warn($"PushForceMap curve assignment notice: {ex.Message}");
        }

        // 3. Tune ONLY the active runtime settings instance on this specific board (NEVER touch _defaultData ScriptableObject!)
        try
        {
            if (board._settings != null && board._settings.Pointer != IntPtr.Zero)
            {
                TuneSettingsObject(board._settings, config);
            }
        }
        catch (Exception ex)
        {
            tuneOk = false;
            Mod.Log.Warn($"_settings tune notice: {ex.Message}");
        }

        try
        {
            var curSettings = board.CurentSettings;
            if (curSettings != null && curSettings.Pointer != IntPtr.Zero && curSettings != board._settings)
            {
                TuneSettingsObject(curSettings, config);
            }
        }
        catch (Exception ex)
        {
            tuneOk = false;
            Mod.Log.Warn($"CurentSettings tune notice: {ex.Message}");
        }

        // Gatekeeper-fix 2026-08-30 B3: only cache after successful tune
        if (tuneOk && instId != 0)
        {
            _tunedInstanceIds.Add(instId);
        }

        if (logStats)
        {
            DumpSkateboardStats(board, "Tuned Board");
        }
    }

    public static void DumpSkateboardStats(Skateboard board, string context)
    {
        try
        {
            if (board == null || board.Pointer == IntPtr.Zero) return;
            Mod.Log.Info($"[{context}] TopSpeed={board.TopSpeed_Kmh} km/h, TurnForce={board.TurnForce}, TurnRate={board.TurnChangeRate}, TurnReturn={board.TurnReturnToRestRate}, JumpForce={board.JumpForce}, JumpMin={board.JumpDuration_Min:F2}s, JumpMax={board.JumpDuration_Max:F2}s, Boost={board.JumpForwardBoost}");
        }
        catch { }
    }

    private static void TuneSettingsObject(Il2CppScheduleOne.Experimental.SkateboardSettings settings, SkateboardConfig config)
    {
        if (settings == null || settings.Pointer == IntPtr.Zero) return;

        try
        {
            settings.TopSpeed_Kmh = config.TopSpeed_Kmh;
            settings.PushForceMultiplier = config.PushForceMultiplier;
            settings.PushForceDuration = config.PushForceDuration;
            settings.PushDelay = config.PushCooldown;

            settings.JumpForce = config.JumpForce;
            settings.JumpDuration_Min = Mathf.Clamp(config.JumpDuration_Min, 0.1f, 1.5f);
            settings.JumpDuration_Max = Mathf.Clamp(Mathf.Max(config.JumpDuration_Max, config.JumpDuration_Min), 0.1f, 1.5f);
            settings.JumpForwardBoost = config.JumpForwardBoost;

            settings.TurnForce = config.TurnForce;
            settings.TurnChangeRate = config.TurnChangeRate;
            settings.TurnReturnToRestRate = config.TurnReturnToRestRate;
            settings.TurnSpeedBoost = config.TurnSpeedBoost;

            // Gatekeeper-fix 2026-08-30 B1: wire AirMovement config
            settings.AirMovementEnabled = config.AirMovementEnabled;
            settings.AirMovementForce = config.AirMovementForce;

            settings.BrakeForce = config.BrakeForce;
            settings.LateralFrictionForceMultiplier = config.LateralFrictionForceMultiplier;
            settings.LongitudinalFrictionMultiplier = config.LongitudinalFrictionMultiplier;

            try
            {
                // Gatekeeper-fix 2026-08-30 B4: clone curve to avoid mutating static cache
                settings.TurnForceMap = new AnimationCurve(_cachedTurnCurve.keys);
            }
            catch (Exception ex)
            {
                Mod.Log.Warn($"Settings TurnForceMap assignment notice: {ex.Message}");
            }

            try
            {
                // Gatekeeper-fix 2026-08-30 B4: clone curve to avoid mutating static cache
                settings.PushForceMultiplierMap = new AnimationCurve(_cachedPushCurve.keys);
            }
            catch (Exception ex)
            {
                Mod.Log.Warn($"Settings PushForceMultiplierMap assignment notice: {ex.Message}");
            }
        }
        catch (Exception ex)
        {
            Mod.Log.Warn($"TuneSettingsObject exception: {ex.Message}");
        }
    }

    private static readonly AnimationCurve _cachedTurnCurve = BuildHighSpeedTurnCurve();
    private static readonly AnimationCurve _cachedPushCurve = BuildHighSpeedPushCurve();

    private static AnimationCurve BuildHighSpeedTurnCurve()
    {
        // Maintains extreme carving authority from 0 km/h up to 160 km/h (unlike vanilla which falls to 0 at ~25 km/h)
        return new AnimationCurve(
            new Keyframe(0f, 1.0f, 0f, -0.0005f),
            new Keyframe(45f, 0.96f, -0.001f, -0.001f),
            new Keyframe(85f, 0.90f, -0.0015f, -0.0015f),
            new Keyframe(125f, 0.82f, -0.002f, -0.002f),
            new Keyframe(160f, 0.75f, -0.002f, 0f)
        );
    }

    private static AnimationCurve BuildHighSpeedPushCurve()
    {
        // Delivers progressive push acceleration up to top speed (unlike vanilla which cuts off above 25 km/h)
        return new AnimationCurve(
            new Keyframe(0f, 1.0f, 0f, -0.001f),
            new Keyframe(45f, 0.90f, -0.002f, -0.003f),
            new Keyframe(85f, 0.72f, -0.005f, -0.006f),
            new Keyframe(115f, 0.45f, -0.008f, -0.008f),
            new Keyframe(150f, 0.15f, -0.006f, 0f)
        );
    }

    private static Sprite? _cachedIcon;
    private static string? _cachedIconPath;

    private static Sprite? LoadCustomIcon()
    {
        try
        {
            string[] searchPaths = new string[]
            {
                Path.Combine(MelonEnvironment.ModsDirectory, "CustomSkateboardIcon.png"),
                Path.Combine(MelonEnvironment.ModsDirectory, "icon.png")
            };

            foreach (string path in searchPaths)
            {
                if (File.Exists(path))
                {
                    if (_cachedIcon != null && _cachedIconPath == path) return _cachedIcon;
                    byte[] fileData = File.ReadAllBytes(path);
                    Texture2D tex = new Texture2D(128, 128, TextureFormat.RGBA32, false);
                    if (ImageConversion.LoadImage(tex, fileData))
                    {
                        // Destroy previous cached sprite+texture before replacing (B6 leak fix)
                        if (_cachedIcon != null)
                        {
                            try
                            {
                                var oldTex = _cachedIcon.texture;
                                if (oldTex != null && oldTex.Pointer != IntPtr.Zero) UnityEngine.Object.Destroy(oldTex);
                            }
                            catch { }
                            try { if (_cachedIcon.Pointer != IntPtr.Zero) UnityEngine.Object.Destroy(_cachedIcon); } catch { }
                        }
                        Sprite sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
                        _cachedIcon = sprite;
                        _cachedIconPath = path;
                        Mod.Log.Info($"Loaded custom icon ({tex.width}x{tex.height}).");
                        return sprite;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Mod.Log.Warn($"Could not load custom icon: {ex.Message}");
        }

        return null;
    }

    public static void InvalidateIconCache()
    {
        if (_cachedIcon != null)
        {
            try
            {
                var tex = _cachedIcon.texture;
                if (tex != null && tex.Pointer != IntPtr.Zero) UnityEngine.Object.Destroy(tex);
            }
            catch { }
            try { if (_cachedIcon.Pointer != IntPtr.Zero) UnityEngine.Object.Destroy(_cachedIcon); } catch { }
            _cachedIcon = null;
            _cachedIconPath = null;
            Mod.Log.Info("Custom icon cache invalidated.");
        }
    }
}
