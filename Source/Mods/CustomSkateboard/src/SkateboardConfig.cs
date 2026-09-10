using System;
using UnityEngine;

namespace CustomSkateboard.Config;
/// <summary>
/// Configuration for the Custom Skateboard Mod.
/// Stored in UserData/CustomSkateboard/config.json.
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
    public void Validate()
    {
        if (TopSpeed_Kmh <= 0f)
            TopSpeed_Kmh = 100f;
        TopSpeed_Kmh = Mathf.Clamp(TopSpeed_Kmh, 5f, 300f);
        PushForceMultiplier = Mathf.Clamp(PushForceMultiplier, 0.1f, 20f);
        PushCooldown = Mathf.Clamp(PushCooldown, 0.05f, 2f);
        TurnForce = Mathf.Clamp(TurnForce, 0.1f, 50f);
        TurnChangeRate = Mathf.Clamp(TurnChangeRate, 1f, 200f);
        TurnReturnToRestRate = Mathf.Clamp(TurnReturnToRestRate, 1f, 200f);
        JumpForce = Mathf.Clamp(JumpForce, 0.1f, 50f);
        JumpDuration_Min = Mathf.Clamp(JumpDuration_Min, 0.05f, 2f);
        JumpDuration_Max = Mathf.Clamp(JumpDuration_Max, JumpDuration_Min, 2f);
        Price = Mathf.Max(0f, Price);
    }
}
