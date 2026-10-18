using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace GraphicsProbe;

internal sealed class BoundedSection<T>
{
    public int Limit { get; }
    public int Scanned { get; private set; }
    public int Matched { get; private set; }
    public int Failed { get; private set; }
    public int LimitOmissions { get; private set; }
    public int Kept => Items.Count;
    public bool Truncated => LimitOmissions > 0;
    public List<T> Items { get; } = new();

    public BoundedSection(int limit) => Limit = Math.Max(0, limit);

    public void Consider(bool matches, Func<T> read)
    {
        Scanned++;
        if (!matches) return;
        Matched++;
        if (Items.Count >= Limit) { LimitOmissions++; return; }
        try { Items.Add(read()); }
        catch { Failed++; }
    }
}

internal sealed class ObservedRecord : Dictionary<string, object?>
{
    public Dictionary<string, string> Errors { get; } = new();
    public void Read(string field, Func<object?> getter, Action<Exception>? onError = null)
    {
        try { this[field] = getter(); }
        catch (Exception ex)
        {
            this[field] = null;
            Errors[field] = ReportPolicy.Text(ex.GetType().Name + ": " + ex.Message);
            this["fieldErrors"] = Errors;
            onError?.Invoke(ex);
        }
    }
}

internal static class ReportPolicy
{
    public const int MaxBytes = 4 * 1024 * 1024;
    public const int MaxText = 256;
    public const int MaxPath = 2048;
    public const int MaxHierarchyDepth = 64;
    public const int MaxComponents = 32;
    public const int MaxMaterials = 16;
    public const int MaxNodes = 50000;
    public const int MaxScenes = 64;
    public const int MaxRootsPerScene = 4096;
    public const int MaxChildrenPerNode = 4096;
    public const int MaxComponentsPerNode = 128;
    public const int MaxRenderersScanned = 20000;
    public const int MaxSurfaceRenderersScanned = 19488;
    public const int MaxEffectRenderersScanned = 512;
    public const int MaxEffectMatches = 32;
    public const int MaxPendingRoots = 4096;
    public const int MaxFrontierCursors = 4096;
    public const int DiscoveryMilliseconds = 1500;

    private static readonly JsonSerializerOptions Options = MakeOptions();

    private static JsonSerializerOptions MakeOptions()
    {
        var options = new JsonSerializerOptions { WriteIndented = true, PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        options.Converters.Add(new FiniteDoubleConverter());
        options.Converters.Add(new FiniteFloatConverter());
        return options;
    }

    public static string Text(string? value, int limit = MaxText)
    {
        if (value == null) return "";
        limit = Math.Max(0, limit);
        if (value.Length <= limit) return value;
        if (limit > 0 && char.IsHighSurrogate(value[limit - 1])) limit--;
        return value.Substring(0, limit) + "…";
    }

    public static string Serialize<T>(T value, int maxBytes = MaxBytes)
    {
        string json = JsonSerializer.Serialize(value, Options);
        if (Encoding.UTF8.GetByteCount(json) > maxBytes)
            throw new InvalidOperationException("Report exceeds UTF-8 byte cap; previous latest.json retained.");
        return json;
    }

    private sealed class FiniteDoubleConverter : JsonConverter<double>
    {
        public override double Read(ref Utf8JsonReader reader, Type type, JsonSerializerOptions options) => reader.GetDouble();
        public override void Write(Utf8JsonWriter writer, double value, JsonSerializerOptions options)
        { if (double.IsFinite(value)) writer.WriteNumberValue(value); else writer.WriteNullValue(); }
    }

    private sealed class FiniteFloatConverter : JsonConverter<float>
    {
        public override float Read(ref Utf8JsonReader reader, Type type, JsonSerializerOptions options) => reader.GetSingle();
        public override void Write(Utf8JsonWriter writer, float value, JsonSerializerOptions options)
        { if (float.IsFinite(value)) writer.WriteNumberValue(value); else writer.WriteNullValue(); }
    }
}
