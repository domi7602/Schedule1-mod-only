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

    // Speed & Acceleration
    public float TopSpeed_Kmh { get; set; } = 100f;
    public float PushForceMultiplier { get; set; } = 5.2f;
    public float PushForceDuration { get; set; } = 0.35f;
    public float PushCooldown { get; set; } = 0.22f;
    public float LongitudinalFrictionMultiplier { get; set; } = 0.16f;
    public float BrakeForce { get; set; } = 2.5f;

    // Steering & Handling (Ultra-Responsive Carving & High-Speed Grip)
    public float TurnForce { get; set; } = 15.0f;
    public float TurnChangeRate { get; set; } = 64.0f;
    public float TurnReturnToRestRate { get; set; } = 56.0f;
    public float TurnSpeedBoost { get; set; } = 2.5f;
    public float LateralFrictionForceMultiplier { get; set; } = 1.60f;
    public float MaxBoardLean { get; set; } = 28f;
    public float BoardLeanRate { get; set; } = 60f;

    // Jump & Air Control
    public float JumpForce { get; set; } = 6.5f;
    public float JumpDuration_Min { get; set; } = 0.38f;
    public float JumpDuration_Max { get; set; } = 0.58f;
    public float JumpForwardBoost { get; set; } = 1.8f;
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
        JumpForce = Mathf.Clamp(JumpForce, 0.1f, 20f);
        JumpDuration_Min = Mathf.Clamp(JumpDuration_Min, 0.05f, 2f);
        JumpDuration_Max = Mathf.Clamp(JumpDuration_Max, JumpDuration_Min, 2f);
        Price = Mathf.Max(0f, Price);
    }
}
