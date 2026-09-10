using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using Il2CppScheduleOne.Persistence;
using MelonLoader;
using S1Mods.Shared;

namespace CalculatorApp;

/// <summary>
/// Represents a single completed calculation record in the history ("suchverlauf").
/// </summary>
public sealed class CalculationRecord
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = Guid.NewGuid().ToString("N");

    [JsonPropertyName("expression")]
    public string Expression { get; set; } = string.Empty;

    [JsonPropertyName("result")]
    public string Result { get; set; } = string.Empty;

    [JsonPropertyName("timestamp")]
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}

/// <summary>
/// Persistent state of the calculator to preserve entered digits, active operators,
/// repeated operation operands, and calculation history across app closes and game restarts.
/// </summary>
public sealed class CalculatorState
{
    [JsonPropertyName("displayText")]
    public string DisplayText { get; set; } = "0";

    [JsonPropertyName("expressionText")]
    public string ExpressionText { get; set; } = string.Empty;

    [JsonPropertyName("firstOperand")]
    public decimal? FirstOperand { get; set; }

    [JsonPropertyName("pendingOperator")]
    public string? PendingOperator { get; set; }

    [JsonPropertyName("lastSecondOperand")]
    public decimal? LastSecondOperand { get; set; }

    [JsonPropertyName("lastOperator")]
    public string? LastOperator { get; set; }

    [JsonPropertyName("isNewEntry")]
    public bool IsNewEntry { get; set; } = true;

    [JsonPropertyName("history")]
    public List<CalculationRecord> History { get; set; } = new();

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true,
        PropertyNameCaseInsensitive = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    private static string _lastKnownSlot = "default";

    public static string GetActiveSlotSuffix()
    {
        try
        {
            var info = LoadManager.Instance?.ActiveSaveInfo;
            if (info != null && info.Pointer != IntPtr.Zero && !info.WasCollected)
            {
                string slot = info.SaveSlotNumber.ToString();
                _lastKnownSlot = slot;
                return slot;
            }
        }
        catch { }
        return _lastKnownSlot;
    }

    public static string GetStateFilePath()
    {
        string slotSuffix = GetActiveSlotSuffix();
        string path = SafeStorage.GetUserDataPath("CalculatorApp", $"calculator_state_slot_{slotSuffix}.json");
        TryMigrateLegacy(path);
        return path;
    }

    private static void TryMigrateLegacy(string slotPath)
    {
        try
        {
            string legacyPath = SafeStorage.GetUserDataPath("CalculatorApp", "calculator_state.json");
            if (!File.Exists(legacyPath)) return;
            if (File.Exists(slotPath)) { File.Delete(legacyPath); return; }
            File.Move(legacyPath, slotPath);
        }
        catch (Exception ex)
        {
            MelonLoader.MelonLogger.Warning($"Legacy calculator_state migration failed: {ex.Message}");
        }
    }

    /// <summary>
    /// Loads the persisted calculator state from UserData using SafeStorage with .bak recovery.
    /// </summary>
    public static CalculatorState Load()
    {
        try
        {
            string path = GetStateFilePath();
            var fallback = new CalculatorState();
            var loaded = SafeStorage.LoadSafe<CalculatorState>(path, fallback, options: JsonOptions);
            return loaded ?? fallback;
        }
        catch (Exception ex)
        {
            MelonLogger.Warning($"Failed to load saved state: {ex.Message}. Using default state.");
            return new CalculatorState();
        }
    }

    /// <summary>
    /// Saves the calculator state atomically using SafeStorage.
    /// </summary>
    public void Save()
    {
        try
        {
            string path = GetStateFilePath();
            SafeStorage.SaveAtomic(path, this, options: JsonOptions);
        }
        catch (Exception ex)
        {
            MelonLogger.Error($"Failed to save state: {ex.Message}");
        }
    }
}
