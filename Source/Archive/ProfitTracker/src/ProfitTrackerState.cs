using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using MelonLoader.Utils;
using S1Mods.Shared;

namespace ProfitTracker;

/// <summary>
/// Serializable state data for a specific save game slot.
/// Persisted in UserData/ProfitTracker/<saveName>.json.
/// </summary>
public sealed class SaveStateData
{
    [JsonPropertyName("saveKey")]
    public string SaveKey { get; set; } = "default";

    [JsonPropertyName("lastSampledDay")]
    public int LastSampledDay { get; set; } = -1;

    [JsonPropertyName("dayStartNetWorth")]
    public float DayStartNetWorth { get; set; }

    [JsonPropertyName("dayStartLiquid")]
    public float DayStartLiquid { get; set; }

    /// <summary>
    /// Daily snapshots for rolling calculations: Day -> Snapshot (NetWorth / Liquid).
    /// </summary>
    [JsonPropertyName("dailyNetWorthHistory")]
    public Dictionary<string, float> DailyNetWorthHistory { get; set; } = new();

    [JsonPropertyName("dailyLiquidHistory")]
    public Dictionary<string, float> DailyLiquidHistory { get; set; } = new();

    [JsonPropertyName("lastKnownNetWorth")]
    public float LastKnownNetWorth { get; set; }

    [JsonPropertyName("lastKnownLiquid")]
    public float LastKnownLiquid { get; set; }
}

public sealed class ProfitTrackerState
{
    private readonly ModLogger _log;
    private SaveStateData _data = new();
    private string _currentSaveKey = "default";

    // Session tracking (since mod / game load)
    public float SessionStartNetWorth { get; private set; }
    public float SessionStartLiquid { get; private set; }
    public bool HasSessionStart { get; private set; }

    public ProfitTrackerState(ModLogger log)
    {
        _log = log;
    }

    public static string StateDirectory => Path.Combine(MelonEnvironment.UserDataDirectory, "ProfitTracker");

    public string StatePathFor(string saveKey) => Path.Combine(StateDirectory, MakeSafeFileName(saveKey) + ".json");
    public string LegacyPathFor(string saveKey) => Path.Combine(StateDirectory, MakeSafeFileName(saveKey) + ".txt");

    public void LoadForSave(string saveKey)
    {
        _currentSaveKey = string.IsNullOrWhiteSpace(saveKey) ? "default" : saveKey;
        string jsonPath = StatePathFor(_currentSaveKey);

        if (File.Exists(jsonPath))
        {
            var fallback = new SaveStateData { SaveKey = _currentSaveKey };
            var loaded = SafeStorage.LoadSafe<SaveStateData>(jsonPath, fallback, _log);
            if (loaded != null && loaded.LastSampledDay >= 0)
            {
                _data = loaded;
                _data.SaveKey = _currentSaveKey;
                _log.Info($"Loaded state for '{_currentSaveKey}' (Day {_data.LastSampledDay}, History: {_data.DailyNetWorthHistory.Count} days)");
                return;
            }
        }

        // Check for legacy .txt file migration
        string legacyPath = LegacyPathFor(_currentSaveKey);
        if (!File.Exists(legacyPath) && _currentSaveKey == "default")
        {
            legacyPath = Path.Combine(StateDirectory, "state.txt");
        }

        if (File.Exists(legacyPath))
        {
            _data = MigrateLegacyState(legacyPath, _currentSaveKey);
            SaveCurrent();
            _log.Info($"Migrated legacy .txt state for '{_currentSaveKey}' to JSON");
            return;
        }

        // New fresh save state
        _data = new SaveStateData { SaveKey = _currentSaveKey };
        _log.Info($"Initialized fresh state for '{_currentSaveKey}'");
    }

    public void UpdateEconomy(int currentDay, float netWorth, float liquid, bool useNetWorth)
    {
        if (!HasSessionStart)
        {
            SessionStartNetWorth = netWorth;
            SessionStartLiquid = liquid;
            HasSessionStart = true;
        }

        _data.LastKnownNetWorth = netWorth;
        _data.LastKnownLiquid = liquid;

        string dayKey = currentDay.ToString(CultureInfo.InvariantCulture);

        // Initial setup for this save or new day rollover
        if (_data.LastSampledDay < 0)
        {
            _data.LastSampledDay = currentDay;
            _data.DayStartNetWorth = netWorth;
            _data.DayStartLiquid = liquid;
            _data.DailyNetWorthHistory[dayKey] = netWorth;
            _data.DailyLiquidHistory[dayKey] = liquid;
            SaveCurrent();
        }
        else if (currentDay < _data.LastSampledDay)
        {
            // Time regression (loaded older savegame)
            _log.Info($"Time regression detected (Day {currentDay} < LastSampledDay {_data.LastSampledDay}). Re-anchoring baselines.");
            _data.LastSampledDay = currentDay;
            _data.DayStartNetWorth = netWorth;
            _data.DayStartLiquid = liquid;
            _data.DailyNetWorthHistory[dayKey] = netWorth;
            _data.DailyLiquidHistory[dayKey] = liquid;
            SaveCurrent();
        }
        else if (currentDay != _data.LastSampledDay)
        {
            // Day rolled over! Record start of new day
            _data.LastSampledDay = currentDay;
            _data.DayStartNetWorth = netWorth;
            _data.DayStartLiquid = liquid;
            _data.DailyNetWorthHistory[dayKey] = netWorth;
            _data.DailyLiquidHistory[dayKey] = liquid;

            PruneOldHistory(currentDay);
            SaveCurrent();
            _log.Info($"Day rollover to Day {currentDay} recorded. DayStart: Net=${netWorth:N0}, Liq=${liquid:N0}");
        }
        else
        {
            // Same day: Ensure today's snapshot exists in history
            if (!_data.DailyNetWorthHistory.ContainsKey(dayKey))
                _data.DailyNetWorthHistory[dayKey] = _data.DayStartNetWorth;
            if (!_data.DailyLiquidHistory.ContainsKey(dayKey))
                _data.DailyLiquidHistory[dayKey] = _data.DayStartLiquid;
        }
    }

    public (float todayDelta, float weekDelta, float sessionDelta) GetDeltas(int currentDay, float currentNet, float currentLiq, bool useNetWorth)
    {
        float activeCurrent = useNetWorth ? currentNet : currentLiq;
        float activeDayStart = useNetWorth ? _data.DayStartNetWorth : _data.DayStartLiquid;
        float activeSessionStart = useNetWorth ? SessionStartNetWorth : SessionStartLiquid;

        float todayDelta = activeCurrent - activeDayStart;
        float sessionDelta = activeCurrent - activeSessionStart;

        // Rolling 7-day week profit calculation
        float weekDelta = todayDelta;
        int oldestTargetDay = Math.Max(0, currentDay - 6);
        string oldestKey = oldestTargetDay.ToString(CultureInfo.InvariantCulture);

        var history = useNetWorth ? _data.DailyNetWorthHistory : _data.DailyLiquidHistory;
        if (history.TryGetValue(oldestKey, out float oldestVal))
        {
            weekDelta = activeCurrent - oldestVal;
        }
        else if (history.Count > 0)
        {
            // Fallback: earliest recorded day baseline
            int minDay = int.MaxValue;
            float earliestVal = activeDayStart;
            foreach (var kvp in history)
            {
                if (int.TryParse(kvp.Key, NumberStyles.Integer, CultureInfo.InvariantCulture, out int d))
                {
                    if (d < minDay)
                    {
                        minDay = d;
                        earliestVal = kvp.Value;
                    }
                }
            }
            weekDelta = activeCurrent - earliestVal;
        }

        return (todayDelta, weekDelta, sessionDelta);
    }

    public void ResetBaselines(int currentDay, float netWorth, float liquid)
    {
        SessionStartNetWorth = netWorth;
        SessionStartLiquid = liquid;

        _data.LastSampledDay = currentDay;
        _data.DayStartNetWorth = netWorth;
        _data.DayStartLiquid = liquid;

        _data.DailyNetWorthHistory.Clear();
        _data.DailyLiquidHistory.Clear();

        string dayKey = currentDay.ToString(CultureInfo.InvariantCulture);
        _data.DailyNetWorthHistory[dayKey] = netWorth;
        _data.DailyLiquidHistory[dayKey] = liquid;

        SaveCurrent();
        _log.Info($"ProfitTracker baselines reset for Day {currentDay}: Net=${netWorth:N0}, Liq=${liquid:N0}");
    }

    private void PruneOldHistory(int currentDay)
    {
        int keepAfter = currentDay - 14;
        List<string> toRemove = new();
        foreach (var k in _data.DailyNetWorthHistory.Keys)
        {
            if (int.TryParse(k, NumberStyles.Integer, CultureInfo.InvariantCulture, out int d) && d < keepAfter)
                toRemove.Add(k);
        }
        foreach (var k in toRemove)
        {
            _data.DailyNetWorthHistory.Remove(k);
            _data.DailyLiquidHistory.Remove(k);
        }
    }

    public void SaveCurrent()
    {
        try
        {
            string path = StatePathFor(_currentSaveKey);
            SafeStorage.SaveAtomic(path, _data, _log);
        }
        catch (Exception ex)
        {
            _log.Error($"Failed to save state for '{_currentSaveKey}'", ex);
        }
    }

    private SaveStateData MigrateLegacyState(string legacyPath, string saveKey)
    {
        var data = new SaveStateData { SaveKey = saveKey };
        try
        {
            int day = -1;
            float dayStart = 0f;

            foreach (string line in File.ReadAllLines(legacyPath))
            {
                int eq = line.IndexOf('=');
                if (eq <= 0) continue;

                string k = line.Substring(0, eq).Trim();
                string v = line.Substring(eq + 1).Trim();
                if (k == "day" && int.TryParse(v, NumberStyles.Integer, CultureInfo.InvariantCulture, out int parsedDay))
                    day = parsedDay;
                else if (k == "dayStart" && float.TryParse(v, NumberStyles.Float, CultureInfo.InvariantCulture, out float parsedVal))
                    dayStart = parsedVal;
            }

            if (day >= 0)
            {
                data.LastSampledDay = day;
                data.DayStartLiquid = dayStart;
                data.DayStartNetWorth = dayStart;
                string dayKey = day.ToString(CultureInfo.InvariantCulture);
                data.DailyLiquidHistory[dayKey] = dayStart;
                data.DailyNetWorthHistory[dayKey] = dayStart;
            }
        }
        catch (Exception ex)
        {
            _log.Warn($"Error during legacy state migration: {ex.Message}");
        }
        return data;
    }

    private static string MakeSafeFileName(string value)
    {
        foreach (char c in Path.GetInvalidFileNameChars())
            value = value.Replace(c, '_');
        return value.Length > 80 ? value.Substring(0, 80) : value;
    }
}
