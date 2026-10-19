using System.Globalization;
using System.Text.RegularExpressions;
using WaterShader;
using Xunit;

namespace WaterShader.Tests;

/// <summary>
/// Pins the reference-water look step. Two things are protected here: the values only ever
/// move toward "clearer", and the parts the owner approved — the deep tint, the rest of the
/// palette, reflection, sun and the shore foam — are NOT written, so this stays a clarity
/// change and not a recolour.
/// </summary>
public sealed class ReferenceWaterLookTests
{
    private static readonly (string name, float value)[] ApprovedFloats =
    {
        ("_Absorption", 0.38f),
        ("_RippleStrength", 0.45f),
    };

    private static readonly (string name, float r, float g, float b)[] ApprovedColors =
    {
        ("_ShallowTint", 0.50f, 0.75f, 0.75f),
    };

    /// <summary>
    /// Shader-declared defaults and ranges from ReferenceWater.shader. Each written float must
    /// move TOWARD clearer (lower absorption, less distortion) while staying inside the declared
    /// Range(). The foam names are deliberately absent from every table here — see
    /// <see cref="LeavesShoreFoamAtTheShaderDefaults"/>.
    /// </summary>
    private static readonly (string name, float shaderDefault, float minimum, float maximum)[] ClarityRanges =
    {
        ("_Absorption", 0.55f, 0.05f, 4.0f),
        ("_RippleStrength", 0.55f, 0f, 2.0f),
    };

    /// <summary>Shader-declared Color defaults for the one tint that is moved.</summary>
    private static readonly (string name, float r, float g, float b)[] TintDefaults =
    {
        ("_ShallowTint", 0.35f, 0.75f, 0.75f),
    };

    /// <summary>
    /// Foam is deliberately left at the shader's own defaults, so those defaults are part of the
    /// contract: if someone edits them in the shader, "we reverted the foam tweak" quietly stops
    /// being true even though nothing in the recipe changed. Pinned against the shader file below.
    /// </summary>
    private static readonly (string name, float shaderDefault, float minimum, float maximum)[] UnwrittenShaderDefaults =
    {
        ("_FoamDepth", 0.35f, 0.02f, 2.0f),
        ("_FoamStrength", 0.80f, 0f, 2.0f),
    };

    private const string ShaderPath = "Source/Mods/WorldLook/unity/Assets/S1Mods/ReferenceWater.shader";

    [Fact]
    public void AppliesOnlyApprovedFloatsAndColors()
    {
        var material = new TestMaterial();

        ReferenceWaterLook.Apply(material);

        var expected = ApprovedFloats.ToDictionary(pair => pair.name, pair => pair.value);
        Assert.Equal(expected.OrderBy(pair => pair.Key), material.FloatWrites.OrderBy(pair => pair.Key));
        Assert.Equal(ApprovedColors.Select(entry => entry.name), material.ColorWrites.Keys);
        Assert.Equal(
            ApprovedFloats.Select(pair => pair.name).Concat(ApprovedColors.Select(entry => entry.name)),
            material.Checked);
    }

    [Theory]
    [InlineData("_Absorption", 0.55f, 0.05f, 4.0f)]
    [InlineData("_RippleStrength", 0.55f, 0f, 2.0f)]
    public void MovesEveryClarityFloatTowardClearerInsideItsDeclaredRange(
        string name, float shaderDefault, float minimum, float maximum)
    {
        var material = new TestMaterial();

        ReferenceWaterLook.Apply(material);

        Assert.True(material.FloatWrites.TryGetValue(name, out float value), $"Missing clarity write for {name}");
        Assert.True(value < shaderDefault, $"{name}={value} must be below the shader default {shaderDefault} to read clearer");
        Assert.InRange(value, minimum, maximum);
    }

    [Fact]
    public void RaisesTheShallowTintRedAndKeepsGreenAndBlue()
    {
        // Everything seen THROUGH the surface is multiplied by lerp(_ShallowTint, _DeepTint,
        // absorb) * 2.0. At the old red the gain kept red well below green, which is what made the
        // bottom flatten into teal and read as "not clear" rather than as an absorption problem.
        // Raising red restores the bottom's own colour; green and blue stay, so the palette
        // character is preserved.
        var material = new TestMaterial();

        ReferenceWaterLook.Apply(material);

        var shaderDefault = TestMaterial.ShaderTintDefaults["_ShallowTint"];
        var written = material.ColorWrites["_ShallowTint"];
        Assert.True(written.r > shaderDefault.r,
            $"shallow tint red {written.r} must be above the shader default to read clearer");
        Assert.True(written.r <= written.g,
            $"shallow tint red {written.r} must not overtake green {written.g} - less red-crushed, not red-dominant");
        Assert.Equal(shaderDefault.g, written.g);
        Assert.Equal(shaderDefault.b, written.b);
        Assert.Equal(1f, written.alpha);
    }

    [Fact]
    public void LeavesShoreFoamAtTheShaderDefaults()
    {
        // 0.1.1 narrowed the foam band as a side effect of a clarity change; 0.1.2 reverted that.
        // The shader gates foam through smoothstep(0.35, 0.9, foamBand), so the old reduction only
        // moved a hairline by ~4.5 cm of eye-space thickness and was never the reason the shore
        // looked bare.
        var material = new TestMaterial();

        ReferenceWaterLook.Apply(material);

        Assert.False(material.FloatWrites.ContainsKey("_FoamDepth"));
        Assert.False(material.FloatWrites.ContainsKey("_FoamStrength"));

        // Not writing foam is only half the promise: the shader defaults the clone falls back to
        // must stay the ones the decision was made against, or the revert quietly stops being a
        // revert. This also pins the fake's seeded values against the shader instead of against
        // themselves.
        string shader = RepoFile(ShaderPath);
        foreach (var (name, shaderDefault, minimum, maximum) in UnwrittenShaderDefaults)
        {
            var (declaredMinimum, declaredMaximum, declaredDefault) = DeclaredRange(shader, name);
            Assert.Equal(minimum, declaredMinimum);
            Assert.Equal(maximum, declaredMaximum);
            Assert.Equal(shaderDefault, declaredDefault);
            Assert.Equal(shaderDefault, material.Floats[name]);
        }
    }

    [Fact]
    public void LeavesDeepTintRestOfPaletteReflectionSunAndWaveValuesUntouched()
    {
        var material = new TestMaterial();
        var preservedFloats = material.Floats
            .Where(pair => !ApprovedFloats.Any(approved => approved.name == pair.Key)).ToArray();
        var preservedColors = material.Colors
            .Where(pair => !ApprovedColors.Any(approved => approved.name == pair.Key)).ToArray();

        ReferenceWaterLook.Apply(material);

        foreach (var (name, value) in preservedFloats) Assert.Equal(value, material.Floats[name]);
        foreach (var (name, value) in preservedColors) Assert.Equal(value, material.Colors[name]);
    }

    [Fact]
    public void ClarityRangesAndTintDefaultsMatchTheShaderDeclarationsOnDisk()
    {
        // The defaults/ranges above are the "toward clearer" baseline. Reading them out of the
        // shader means an edited Range() or default cannot silently drift away from the pin.
        string shader = RepoFile(ShaderPath);
        foreach (var (name, shaderDefault, minimum, maximum) in ClarityRanges)
        {
            var (declaredMinimum, declaredMaximum, declaredDefault) = DeclaredRange(shader, name);
            Assert.Equal(minimum, declaredMinimum);
            Assert.Equal(maximum, declaredMaximum);
            Assert.Equal(shaderDefault, declaredDefault);
        }
        foreach (var (name, r, g, b) in TintDefaults)
        {
            var declaration = Regex.Match(
                shader,
                Regex.Escape(name) + @"\s*\(\s*""[^""]*""\s*,\s*Color\s*\)\s*=\s*\(\s*(-?[\d.]+)\s*,\s*(-?[\d.]+)\s*,\s*(-?[\d.]+)");
            Assert.True(declaration.Success, $"{name} is not declared as a Color in ReferenceWater.shader");
            Assert.Equal(r, float.Parse(declaration.Groups[1].Value, CultureInfo.InvariantCulture));
            Assert.Equal(g, float.Parse(declaration.Groups[2].Value, CultureInfo.InvariantCulture));
            Assert.Equal(b, float.Parse(declaration.Groups[3].Value, CultureInfo.InvariantCulture));
        }
    }

    /// <summary>Reads <c>Name ("label", Range(min, max)) = default</c> out of the shader source.</summary>
    private static (float minimum, float maximum, float declaredDefault) DeclaredRange(string shader, string name)
    {
        var declaration = Regex.Match(
            shader,
            Regex.Escape(name) + @"\s*\(\s*""[^""]*""\s*,\s*Range\s*\(\s*(-?[\d.]+)\s*,\s*(-?[\d.]+)\s*\)\s*\)\s*=\s*(-?[\d.]+)");
        Assert.True(declaration.Success, $"{name} is not declared with a Range() in ReferenceWater.shader");
        return (float.Parse(declaration.Groups[1].Value, CultureInfo.InvariantCulture),
                float.Parse(declaration.Groups[2].Value, CultureInfo.InvariantCulture),
                float.Parse(declaration.Groups[3].Value, CultureInfo.InvariantCulture));
    }

    [Theory]
    [InlineData("en-US")]
    [InlineData("de-DE")]
    public void DiagnosticListsOnlyAppliedSupportedValuesWithInvariantFormatting(string cultureName)
    {
        var material = new TestMaterial();
        material.Floats.Clear();
        material.Colors.Clear();
        material.Floats["_Absorption"] = 0.91f;
        material.Colors["_ShallowTint"] = (0.11f, 0.22f, 0.33f, 1f);
        var previous = CultureInfo.CurrentCulture;
        try
        {
            CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo(cultureName);

            string diagnostic = ReferenceWaterLook.Apply(material);

            Assert.Equal("[water-shader] reference clarity applied: _Absorption=0.38 _ShallowTint=(0.5,0.75,0.75)", diagnostic);
            Assert.Single(material.FloatWrites);
            Assert.Single(material.ColorWrites);
        }
        finally { CultureInfo.CurrentCulture = previous; }
    }

    [Fact]
    public void DiagnosticExplicitlyReportsNoAppliedValuesWhenNothingIsSupported()
    {
        var material = new TestMaterial();
        material.Floats.Clear();
        material.Colors.Clear();

        Assert.Equal("[water-shader] reference clarity applied: none", ReferenceWaterLook.Apply(material));
        Assert.Empty(material.FloatWrites);
        Assert.Empty(material.ColorWrites);
    }

    public static IEnumerable<object[]> AllSupportedPropertySubsets()
        => Enumerable.Range(0, 1 << 3).Select(subset => new object[] { subset });

    [Theory]
    [MemberData(nameof(AllSupportedPropertySubsets))]
    public void EverySupportedSubsetWritesOnlyGuardedApprovedValues(int subset)
    {
        var material = new TestMaterial();
        material.Floats.Clear();
        material.Colors.Clear();
        var expectedFloats = new Dictionary<string, float>();
        var expectedColors = new List<string>();
        for (int index = 0; index < ApprovedFloats.Length; index++)
        {
            if ((subset & (1 << index)) == 0) continue;
            var (name, value) = ApprovedFloats[index];
            material.Floats[name] = -1f;
            expectedFloats[name] = value;
        }
        for (int index = 0; index < ApprovedColors.Length; index++)
        {
            if ((subset & (1 << (ApprovedFloats.Length + index))) == 0) continue;
            var (name, r, g, b) = ApprovedColors[index];
            material.Colors[name] = (-1f, -1f, -1f, 1f);
            expectedColors.Add(name);
        }

        string diagnostic = ReferenceWaterLook.Apply(material);

        Assert.Equal(expectedFloats.OrderBy(pair => pair.Key), material.FloatWrites.OrderBy(pair => pair.Key));
        Assert.Equal(expectedColors, material.ColorWrites.Keys);
        var tokens = ApprovedFloats.Where(pair => expectedFloats.ContainsKey(pair.name))
            .Select(pair => pair.name + "=" + pair.value.ToString(CultureInfo.InvariantCulture))
            .Concat(ApprovedColors.Where(entry => expectedColors.Contains(entry.name))
                .Select(entry => entry.name + "=(" + entry.r.ToString(CultureInfo.InvariantCulture) + ","
                    + entry.g.ToString(CultureInfo.InvariantCulture) + ","
                    + entry.b.ToString(CultureInfo.InvariantCulture) + ")"))
            .ToList();
        Assert.Equal("[water-shader] reference clarity applied: " + (tokens.Count == 0 ? "none" : string.Join(" ", tokens)), diagnostic);
    }

    private static string RepoFile(string relativePath)
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);
        while (directory != null && !File.Exists(Path.Combine(directory.FullName, "AGENTS.md"))) directory = directory.Parent;
        Assert.NotNull(directory);
        return File.ReadAllText(Path.Combine(directory!.FullName, relativePath));
    }

    private sealed class TestMaterial : IWaterLookMaterial
    {
        /// <summary>Shader-declared float defaults, used to pin "toward clearer" and to seed the fake.</summary>
        public static readonly Dictionary<string, float> ShaderDefaults = new()
        {
            ["_Absorption"] = 0.55f,
            ["_RippleStrength"] = 0.55f,
            ["_FoamDepth"] = 0.35f,
            ["_FoamStrength"] = 0.80f,
        };

        /// <summary>Shader-declared Color defaults, used to pin "toward clearer" and to seed the fake.</summary>
        public static readonly Dictionary<string, (float r, float g, float b)> ShaderTintDefaults = new()
        {
            ["_ShallowTint"] = (0.35f, 0.75f, 0.75f),
        };

        public Dictionary<string, float> Floats { get; } = new(ShaderDefaults)
        {
            // Reflection / sun / wave values the look step must not touch.
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
