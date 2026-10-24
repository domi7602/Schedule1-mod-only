// ---------------------------------------------------------------------------
// Probe/ProbeReportPolicies.cs
//
// Mechanically consolidated from the former per-type source files. Each original
// file keeps its own block namespace with its own using directives, so name
// binding is unchanged; the bodies differ from the pre-consolidation sources only
// by the block-namespace indent level. Machine markers delimit the section that
// came from each original file so the source-contract checks and an independent
// reconstruction proof can still read that file's exact content.
//
//   Probe/ReportPolicy.cs              -> section ReportPolicy.cs
//   Probe/IntrospectionPolicy.cs       -> section IntrospectionPolicy.cs
//   Probe/WaterHeuristics.cs           -> section WaterHeuristics.cs
// ---------------------------------------------------------------------------

namespace GraphicsProbe
{
    // ==== WorldLook section: ReportPolicy.cs ====
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Text.Json;
    using System.Text.Json.Serialization;


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
    // ==== end WorldLook section: ReportPolicy.cs ====
}

namespace GraphicsProbe
{
    // ==== WorldLook section: IntrospectionPolicy.cs ====
    using System;
    using System.Collections.Generic;


    /// <summary>
    /// Unity-free policy for bounded material/volume introspection: property read
    /// strategy by shader property kind, property-count caps and non-finite value
    /// normalization. Wired by ObjectReader/EnvironmentReader in the mod assembly.
    /// </summary>
    internal static class IntrospectionPolicy
    {
        public const int MaxShaderProperties = 64;

        /// <summary>
        /// Maps a UnityEngine.Rendering.ShaderPropertyType name to a read strategy:
        /// "color" (GetColor), "vector" (GetVector), "float" (GetFloat, covers Range),
        /// "texture" (GetTexture identity only) and "unknown" (name kept, value not read).
        /// </summary>
        public static string ShapeOf(string? kind) => kind switch
        {
            "Color" => "color",
            "Vector" => "vector",
            "Float" or "Range" => "float",
            "Texture" => "texture",
            _ => "unknown"
        };

        /// <summary>Keeps the prefix of property indices within the cap.</summary>
        public static IReadOnlyList<int> PlanIndices(int totalCount, int cap)
        {
            int kept = Math.Clamp(cap, 0, Math.Max(0, totalCount));
            var indices = new List<int>(kept);
            for (int i = 0; i < kept; i++) indices.Add(i);
            return indices;
        }

        /// <summary>Finite values pass through; NaN/Infinity become null.</summary>
        public static double? Normalize(double value) => double.IsFinite(value) ? value : null;

        /// <summary>
        /// Normalizes a component tuple. JSON validity at serialization is guaranteed
        /// by ReportPolicy's finite converters; rejecting the whole tuple when one
        /// component is non-finite is a semantic choice: a color/vector with a broken
        /// component is not meaningful evidence, so the record carries a clean null.
        /// Empty or missing tuples are rejected as well.
        /// </summary>
        public static IReadOnlyList<double>? NormalizeComponents(IReadOnlyList<double> components)
        {
            if (components == null || components.Count == 0) return null;
            var kept = new List<double>(components.Count);
            foreach (double component in components)
            {
                if (!double.IsFinite(component)) return null;
                kept.Add(component);
            }
            return kept;
        }
    }
    // ==== end WorldLook section: IntrospectionPolicy.cs ====
}

namespace GraphicsProbe
{
    // ==== WorldLook section: WaterHeuristics.cs ====
    using System;
    using System.Collections.Generic;


    internal static class WaterHeuristics
    {
        // Deliberately lexical. These clues are NOT identification of actual water.
        private static readonly string[] Terms = { "water", "ocean", "river", "lake", "pond", "sea", "canal" };

        public static List<string> Evidence(string? rendererName, string? materialName, string? shaderName)
        {
            var result = new List<string>();
            Add(result, "renderer-name", rendererName);
            Add(result, "material-name", materialName);
            Add(result, "shader-name", shaderName);
            return result;
        }

        private static void Add(List<string> result, string source, string? text)
        {
            if (text == null) return;
            foreach (string term in Terms)
            {
                int start = 0;
                while (start < text.Length)
                {
                    int index = text.IndexOf(term, start, StringComparison.OrdinalIgnoreCase);
                    if (index < 0) break;
                    int end = index + term.Length;
                    bool seaToken = term != "sea" || ((index == 0 || !char.IsLetterOrDigit(text[index - 1])) && (end == text.Length || !char.IsLetterOrDigit(text[end])));
                    if (seaToken) { result.Add(source + ":" + term); break; }
                    start = end;
                }
            }
        }
    }
    // ==== end WorldLook section: WaterHeuristics.cs ====
}
