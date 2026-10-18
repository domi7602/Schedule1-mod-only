using System.Globalization;
using System.Text.RegularExpressions;
using WaterShader;
using Xunit;

namespace WaterShader.Tests;

/// <summary>
/// Pins the reference-water clarity step. The owner approved the palette, so these
/// tests also pin that the colour, reflection and sun values are NOT touched — only
/// the four clarity parameters may move, and only toward clearer.
/// </summary>
public sealed class ReferenceWaterLookTests
{
    private static readonly (string name, float value)[] ApprovedFloats =
    {
        ("_Absorption", 0.38f),
        ("_RippleStrength", 0.45f),
        ("_FoamDepth", 0.28f),
        ("_FoamStrength", 0.68f),
    };

    /// <summary>
    /// Shader-declared defaults and ranges from ReferenceWater.shader. The clarity step
    /// must move every value TOWARD clearer (lower absorption, less distortion, less
    /// foam) while staying inside the declared Range() so the material sheet accepts it.
    /// </summary>
    private static readonly (string name, float shaderDefault, float minimum, float maximum)[] ClarityRanges =
    {
        ("_Absorption", 0.55f, 0.05f, 4.0f),
        ("_RippleStrength", 0.55f, 0f, 2.0f),
        ("_FoamDepth", 0.35f, 0.02f, 2.0f),
        ("_FoamStrength", 0.80f, 0f, 2.0f),
    };

    [Fact]
    public void AppliesOnlyApprovedClarityFloats()
    {
        var material = new TestMaterial();

        ReferenceWaterLook.Apply(material);

        var expected = ApprovedFloats.ToDictionary(pair => pair.name, pair => pair.value);
        Assert.Equal(expected.OrderBy(pair => pair.Key), material.FloatWrites.OrderBy(pair => pair.Key));
        Assert.Equal(ApprovedFloats.Select(pair => pair.name), material.Checked);
    }

    [Theory]
    [InlineData("_Absorption", 0.55f, 0.05f, 4.0f)]
    [InlineData("_RippleStrength", 0.55f, 0f, 2.0f)]
    [InlineData("_FoamDepth", 0.35f, 0.02f, 2.0f)]
    [InlineData("_FoamStrength", 0.80f, 0f, 2.0f)]
    public void MovesEveryClarityValueTowardClearerInsideItsDeclaredRange(
        string name, float shaderDefault, float minimum, float maximum)
    {
        var material = new TestMaterial();

        ReferenceWaterLook.Apply(material);

        Assert.True(material.FloatWrites.TryGetValue(name, out float value), $"Missing clarity write for {name}");
        Assert.True(value < shaderDefault, $"{name}={value} must be below the shader default {shaderDefault} to read clearer");
        Assert.InRange(value, minimum, maximum);
    }

    [Fact]
    public void ClarityRangesCoverExactlyTheApprovedFloats()
    {
        // Guards the semantic table against drifting away from the applied recipe.
        Assert.Equal(ApprovedFloats.Select(pair => pair.name), ClarityRanges.Select(entry => entry.name));
        foreach (var (name, shaderDefault, _, _) in ClarityRanges)
        {
            Assert.Equal(shaderDefault, TestMaterial.ShaderDefaults[name]);
        }
    }

    [Fact]
    public void ClarityRangesMatchTheShaderDeclarationsOnDisk()
    {
        // The defaults/ranges above are the "toward clearer" baseline. Reading them out of the
        // shader means an edited Range() or default cannot silently drift away from the pin.
        string shader = RepoFile("Source/Mods/WorldLook/unity/Assets/S1Mods/ReferenceWater.shader");
        foreach (var (name, shaderDefault, minimum, maximum) in ClarityRanges)
        {
            var declaration = Regex.Match(
                shader,
                Regex.Escape(name) + @"\s*\(\s*""[^""]*""\s*,\s*Range\s*\(\s*(-?[\d.]+)\s*,\s*(-?[\d.]+)\s*\)\s*\)\s*=\s*(-?[\d.]+)");
            Assert.True(declaration.Success, $"{name} is not declared with a Range() in ReferenceWater.shader");
            Assert.Equal(minimum, float.Parse(declaration.Groups[1].Value, CultureInfo.InvariantCulture));
            Assert.Equal(maximum, float.Parse(declaration.Groups[2].Value, CultureInfo.InvariantCulture));
            Assert.Equal(shaderDefault, float.Parse(declaration.Groups[3].Value, CultureInfo.InvariantCulture));
        }
    }

    private static string RepoFile(string relativePath)
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);
        while (directory != null && !File.Exists(Path.Combine(directory.FullName, "AGENTS.md"))) directory = directory.Parent;
        Assert.NotNull(directory);
        return File.ReadAllText(Path.Combine(directory!.FullName, relativePath));
    }

    [Fact]
    public void LeavesTheApprovedPaletteReflectionSunAndWaveValuesUntouched()
    {
        var material = new TestMaterial();
        var preserved = material.Floats.Where(pair => !ApprovedFloats.Any(approved => approved.name == pair.Key)).ToArray();
        var colors = material.Colors.ToArray();

        ReferenceWaterLook.Apply(material);

        foreach (var (name, value) in preserved) Assert.Equal(value, material.Floats[name]);
        foreach (var (name, value) in colors) Assert.Equal(value, material.Colors[name]);
        Assert.Empty(material.ColorWrites);
    }

    [Theory]
    [InlineData("en-US")]
    [InlineData("de-DE")]
    public void DiagnosticListsOnlyAppliedSupportedValuesWithInvariantFormatting(string cultureName)
    {
        var material = new TestMaterial();
        material.Floats.Clear();
        material.Floats["_Absorption"] = 0.91f;
        material.Floats["_FoamDepth"] = 0.44f;
        var previous = CultureInfo.CurrentCulture;
        try
        {
            CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo(cultureName);

            string diagnostic = ReferenceWaterLook.Apply(material);

            Assert.Equal("[water-shader] reference clarity applied: _Absorption=0.38 _FoamDepth=0.28", diagnostic);
            Assert.Equal(2, material.FloatWrites.Count);
        }
        finally { CultureInfo.CurrentCulture = previous; }
    }

    [Fact]
    public void DiagnosticExplicitlyReportsNoAppliedValuesWhenNothingIsSupported()
    {
        var material = new TestMaterial();
        material.Floats.Clear();

        Assert.Equal("[water-shader] reference clarity applied: none", ReferenceWaterLook.Apply(material));
        Assert.Empty(material.FloatWrites);
    }

    public static IEnumerable<object[]> AllSupportedPropertySubsets()
        => Enumerable.Range(0, 1 << 4).Select(subset => new object[] { subset });

    [Theory]
    [MemberData(nameof(AllSupportedPropertySubsets))]
    public void EverySupportedSubsetWritesOnlyGuardedApprovedValues(int subset)
    {
        var material = new TestMaterial();
        material.Floats.Clear();
        var expected = new Dictionary<string, float>();
        for (int index = 0; index < ApprovedFloats.Length; index++)
        {
            if ((subset & (1 << index)) == 0) continue;
            var (name, value) = ApprovedFloats[index];
            material.Floats[name] = -1f;
            expected[name] = value;
        }

        string diagnostic = ReferenceWaterLook.Apply(material);

        Assert.Equal(expected.OrderBy(pair => pair.Key), material.FloatWrites.OrderBy(pair => pair.Key));
        Assert.Equal(ApprovedFloats.Select(pair => pair.name), material.Checked);
        var tokens = ApprovedFloats.Where(pair => expected.ContainsKey(pair.name))
            .Select(pair => pair.name + "=" + pair.value.ToString(CultureInfo.InvariantCulture)).ToList();
        Assert.Equal("[water-shader] reference clarity applied: " + (tokens.Count == 0 ? "none" : string.Join(" ", tokens)), diagnostic);
    }

    private sealed class TestMaterial : IWaterLookMaterial
    {
        /// <summary>Shader-declared defaults, used to pin "toward clearer" and to seed the fake.</summary>
        public static readonly Dictionary<string, float> ShaderDefaults = new()
        {
            ["_Absorption"] = 0.55f,
            ["_RippleStrength"] = 0.55f,
            ["_FoamDepth"] = 0.35f,
            ["_FoamStrength"] = 0.80f,
        };

        public Dictionary<string, float> Floats { get; } = new(ShaderDefaults)
        {
            // Palette / reflection / sun / wave values the clarity step must not touch.
            ["_ReflectionStrength"] = 0.85f,
            ["_ReflectionSharpness"] = 24f,
            ["_WaveAmp"] = 0.15f,
            ["_WaveScale"] = 1.6f,
            ["_WaveSpeed"] = 0.8f,
            ["_SunStrength"] = 0.9f,
            ["_SunSharpness"] = 220f,
        };

        public Dictionary<string, (float r, float g, float b, float alpha)> Colors { get; } = new()
        {
            ["_ShallowTint"] = (0.35f, 0.75f, 0.75f, 1f),
            ["_DeepTint"] = (0.03f, 0.23f, 0.38f, 1f),
            ["_FoamColor"] = (0.90f, 0.95f, 0.97f, 1f),
            ["_SkyColor"] = (0.55f, 0.70f, 0.85f, 1f),
            ["_AmbientColor"] = (0.12f, 0.22f, 0.30f, 1f),
        };

        public Dictionary<string, float> FloatWrites { get; } = new();
        public Dictionary<string, (float r, float g, float b, float alpha)> ColorWrites { get; } = new();
        public List<string> Checked { get; } = new();

        public bool HasProperty(string name)
        {
            Checked.Add(name);
            return Floats.ContainsKey(name) || Colors.ContainsKey(name);
        }

        public void SetFloat(string name, float value)
        {
            Assert.Contains(name, Checked);
            Assert.True(Floats.ContainsKey(name), $"Access to unsupported property {name}");
            Floats[name] = FloatWrites[name] = value;
        }

        public void SetColor(string name, float r, float g, float b, float alpha)
        {
            Assert.Contains(name, Checked);
            Assert.True(Colors.ContainsKey(name), $"Access to unsupported property {name}");
            Colors[name] = ColorWrites[name] = (r, g, b, alpha);
        }
    }
}
