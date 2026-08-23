using System;

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
}
