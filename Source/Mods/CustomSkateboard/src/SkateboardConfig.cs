using System;
using UnityEngine;

namespace CustomSkateboard.Config;
/// <summary>
/// Configuration for the Custom Skateboard Mod.
/// Stored via MelonPreferences in the [CustomSkateboard] category (MelonPreferences.cfg),
/// managed by S1Mods.Shared.ModConfig.
/// </summary>
public sealed class SkateboardConfig
{
    public static SkateboardConfig Default() => new();

    public string SkateboardId { get; set; } = "custom_skateboard";
    public string SkateboardName { get; set; } = "Pro Cyber Skateboard";
    public string Description { get; set; } = "A custom aerodynamic cyberpunk skateboard crafted with neon underglow, carbon fiber deck, anti-gravel suspension, and extreme carving physics.";
    public float Price { get; set; } = 1500f;
    public string BaseItemIdOverride { get; set; } = "";

    // Speed & Acceleration (v1.1.0 tune: higher top speed, stronger push)
    public float TopSpeed_Kmh { get; set; } = 140f;
    public float PushForceMultiplier { get; set; } = 6.5f;
    public float PushForceDuration { get; set; } = 0.35f;
    public float PushCooldown { get; set; } = 0.18f;
    public float LongitudinalFrictionMultiplier { get; set; } = 0.13f;
    public float BrakeForce { get; set; } = 2.5f;

    // Steering & Handling (v1.1.0 tune: ultra-responsive carving & high-speed grip)
    public float TurnForce { get; set; } = 20.0f;
    public float TurnChangeRate { get; set; } = 85.0f;
    public float TurnReturnToRestRate { get; set; } = 75.0f;
    public float TurnSpeedBoost { get; set; } = 2.5f;
    public float LateralFrictionForceMultiplier { get; set; } = 1.85f;
    public float MaxBoardLean { get; set; } = 33f;
    public float BoardLeanRate { get; set; } = 75f;

    // Jump & Air Control — Gatekeeper-hotfix 2026-08-30: vanilla 6.5 = dead. Bumped to 9.5 for lift.
    // Gatekeeper-hotfix 2026-08-30: 9.5 only lifted slightly, player still couldn't actually jump. Bumped to 14.0 for real hop.
    // Gatekeeper-hotfix 2026-08-30: player confirmed lift but wants more — 2× → 28.0 for proper hop height.
    // Gatekeeper-hotfix 2026-08-30: 28.0 + ollie-curves launched player several meters high. Reduce to 18.0 (still 2.77× vanilla, controlled hop).
    public float JumpForce { get; set; } = 18.0f;
    public float JumpDuration_Min { get; set; } = 0.45f;
    public float JumpDuration_Max { get; set; } = 0.70f;
    public float JumpForwardBoost { get; set; } = 2.4f;
    public bool AirMovementEnabled { get; set; } = false;
    public float AirMovementForce { get; set; } = 0f;

    // Environment & Gameplay
    public bool DisableTerrainSlowdown { get; set; } = true;
    public bool AutoInjectToJeffGilmore { get; set; } = true;

    // Gatekeeper-fix B13: validate deserialized JSON values (TopSpeed 0/negative would break physics/curves).
    private static float SafeClamp(string fieldName, float value, float min, float max, float def)
    {
        if (float.IsNaN(value) || float.IsInfinity(value))
        {
            return def;
        }
        return Mathf.Clamp(value, min, max);
    }

    public void Validate()
    {
        var d = Default();

        TopSpeed_Kmh = SafeClamp("TopSpeed", TopSpeed_Kmh, 5f, 300f, d.TopSpeed_Kmh);
        PushForceMultiplier = SafeClamp("PushForceMult", PushForceMultiplier, 0.1f, 20f, d.PushForceMultiplier);
        PushCooldown = SafeClamp("PushCooldown", PushCooldown, 0.05f, 2f, d.PushCooldown);
        PushForceDuration = SafeClamp("PushDur", PushForceDuration, 0.05f, 2f, d.PushForceDuration);
        TurnForce = SafeClamp("TurnForce", TurnForce, 0.1f, 50f, d.TurnForce);
        TurnChangeRate = SafeClamp("TurnChangeRate", TurnChangeRate, 1f, 200f, d.TurnChangeRate);
        TurnReturnToRestRate = SafeClamp("TurnReturn", TurnReturnToRestRate, 1f, 200f, d.TurnReturnToRestRate);
        TurnSpeedBoost = SafeClamp("TurnSpeedBoost", TurnSpeedBoost, 0.1f, 20f, d.TurnSpeedBoost);
        LateralFrictionForceMultiplier = SafeClamp("LateralFric", LateralFrictionForceMultiplier, 0.01f, 20f, d.LateralFrictionForceMultiplier);
        LongitudinalFrictionMultiplier = SafeClamp("LongFric", LongitudinalFrictionMultiplier, 0.01f, 20f, d.LongitudinalFrictionMultiplier);
        MaxBoardLean = SafeClamp("MaxLean", MaxBoardLean, 1f, 90f, d.MaxBoardLean);
        BoardLeanRate = SafeClamp("LeanRate", BoardLeanRate, 1f, 200f, d.BoardLeanRate);

        JumpForce = SafeClamp("JumpForce", JumpForce, 0.1f, 50f, d.JumpForce);
        JumpDuration_Min = SafeClamp("JumpDurMin", JumpDuration_Min, 0.05f, 2f, d.JumpDuration_Min);
        float jDurMax = float.IsNaN(JumpDuration_Max) || float.IsInfinity(JumpDuration_Max) ? d.JumpDuration_Max : JumpDuration_Max;
        JumpDuration_Max = Mathf.Clamp(Mathf.Max(jDurMax, JumpDuration_Min), 0.05f, 2f);
        JumpForwardBoost = SafeClamp("JumpForward", JumpForwardBoost, 0.1f, 20f, d.JumpForwardBoost);

        BrakeForce = SafeClamp("BrakeForce", BrakeForce, 0f, 20f, d.BrakeForce);
        AirMovementForce = SafeClamp("AirMove", AirMovementForce, 0f, 50f, d.AirMovementForce);
        Price = Mathf.Max(0f, Price);
    }
}
