using System;

namespace BankApp.Config;

/// <summary>
/// Persisted configuration for the BankApp smartphone mod.
/// Stored under UserData/BankApp/config.json.
/// </summary>
public sealed class BankAppConfig
{
    /// <summary>
    /// Optional service fee (0.0% to 10.0%) deducted on mobile transactions.
    /// Default: 0.0 (Pure QoL mobile ATM).
    /// </summary>
    public float ServiceFeePercent { get; set; } = 0.0f;

    /// <summary>
    /// When true, respects the vanilla weekly ATM deposit limit ($10,000 / week)
    /// to preserve vanilla laundering & ATM gameplay balance.
    /// Default: true.
    /// </summary>
    public bool RespectVanillaAtmLimit { get; set; } = true;

    /// <summary>
    /// Maximum cash allowed per inventory slot (Vanilla = 1,000$).
    /// Default: 1000.
    /// </summary>
    public int MaxCashPerSlot { get; set; } = 1000;

    /// <summary>
    /// Enables native cash register sounds and transaction feedback.
    /// Default: true.
    /// </summary>
    public bool EnableSoundEffects { get; set; } = true;
}
