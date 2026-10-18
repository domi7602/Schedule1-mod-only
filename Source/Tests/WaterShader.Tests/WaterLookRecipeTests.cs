using System.Globalization;
using WaterShader;
using Xunit;

namespace WaterShader.Tests;

public sealed class WaterLookRecipeTests
{
    private static readonly (string name, float value)[] ApprovedFloats =
    {
        ("_WaveHeight", 0.35f),
        ("_WaveSpeed", 1.1f),
        ("_WaveCount", 4f),
        ("_NormalStrength", 0.18f),
        ("_FoamBaseAmount", 0.25f),
        ("_DepthVertical", 4f),
        ("_DepthHorizontal", 1f),
        ("_RefractionStrength", 0.1f),
        ("_RefractionChromaticAberration", 0f),
    };

    [Fact]
    public void AppliesOnlyApprovedFloatsIncludingNativeRangeWaveCount()
    {
        var material = new TestMaterial();

        WaterLookRecipe.Apply(material);

        var expected = ApprovedFloats.ToDictionary(pair => pair.name, pair => pair.value);
        Assert.Equal(expected.OrderBy(pair => pair.Key), material.FloatWrites.OrderBy(pair => pair.Key));
        Assert.InRange(material.Floats["_WaveCount"], 1f, 5f);
    }

    // Matching native shader declarations; these ranges/defaults do not establish physical units.
    [Theory]
    [InlineData("_DepthVertical", 4f, 0.01f, 16f)]
    [InlineData("_DepthHorizontal", 1f, 0.01f, 8f)]
    [InlineData("_RefractionStrength", 0.1f, 0f, 1f)]
    public void UsesEvidencedNativeDepthAndRefractionDefaultsWithinDeclaredRanges(string name, float declaredDefault, float minimum, float maximum)
    {
        var material = new TestMaterial();

        WaterLookRecipe.Apply(material);

        Assert.True(material.FloatWrites.TryGetValue(name, out float value), $"Missing approved native-default write for {name}");
        Assert.Equal(declaredDefault, value);
        Assert.InRange(value, minimum, maximum);
    }

    [Fact]
    public void DisablesChromaticAberrationWithinDeclaredNativeRange()
    {
        var material = new TestMaterial();

        WaterLookRecipe.Apply(material);

        Assert.Equal(0f, material.FloatWrites["_RefractionChromaticAberration"]);
        Assert.InRange(material.FloatWrites["_RefractionChromaticAberration"], 0f, 1f);
    }

    [Theory]
    [InlineData(0f)]
    [InlineData(0.37f)]
    [InlineData(1f)]
    public void AppliesExplicitShallowOpacityInsteadOfClonedAlphaPreservingOtherColors(float alpha)
    {
        var material = new TestMaterial();
        material.Colors["_ShallowColor"] = (0.1f, 0.2f, 0.3f, alpha);
        var untouched = material.Colors.Where(pair => pair.Key != "_ShallowColor").ToArray();

        WaterLookRecipe.Apply(material);

        var shallow = Assert.Single(material.ColorWrites);
        Assert.Equal("_ShallowColor", shallow.Key);
        Assert.Equal((0.025f, 0.055f, 0.060f, 0.15f), shallow.Value);
        foreach (var (name, color) in untouched) Assert.Equal(color, material.Colors[name]);
    }

    [Theory]
    [InlineData("en-US")]
    [InlineData("de-DE")]
    public void DiagnosticListsOnlyAppliedSupportedValuesWithInvariantFormatting(string cultureName)
    {
        var material = new TestMaterial();
        material.Floats.Clear();
        material.Floats["_WaveCount"] = 4f;
        material.Floats["_RefractionStrength"] = 0.474f;
        material.Colors.Clear();
        material.Colors["_ShallowColor"] = (0.1f, 0.2f, 0.3f, 0.37f);
        var previous = CultureInfo.CurrentCulture;
        try
        {
            CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo(cultureName);

            string diagnostic = WaterLookRecipe.Apply(material);

            Assert.Equal("[water-shader] clear-shallows applied: _WaveCount=4 _RefractionStrength=0.1 _ShallowColor=(0.025,0.055,0.06,0.15)", diagnostic);
            Assert.Equal(2, material.FloatWrites.Count);
            Assert.Single(material.ColorWrites);
        }
        finally { CultureInfo.CurrentCulture = previous; }
    }

    public static IEnumerable<object[]> AllSupportedPropertySubsets()
        => Enumerable.Range(0, 1 << 10).Select(subset => new object[] { subset });

    [Theory]
    [MemberData(nameof(AllSupportedPropertySubsets))]
    public void EverySupportedSubsetWritesOnlyGuardedApprovedValues(int subset)
    {
        var material = new TestMaterial();
        material.Floats.Clear();
        material.Colors.Clear();
        var expected = new Dictionary<string, float>();
        for (int index = 0; index < ApprovedFloats.Length; index++)
        {
            if ((subset & (1 << index)) == 0) continue;
            var (name, value) = ApprovedFloats[index];
            material.Floats[name] = -1f;
            expected[name] = value;
        }
        bool shallowSupported = (subset & (1 << ApprovedFloats.Length)) != 0;
        if (shallowSupported) material.Colors["_ShallowColor"] = (0.1f, 0.2f, 0.3f, 0.37f);

        string diagnostic = WaterLookRecipe.Apply(material);

        Assert.Equal(expected.OrderBy(pair => pair.Key), material.FloatWrites.OrderBy(pair => pair.Key));
        Assert.Equal(shallowSupported ? 1 : 0, material.ColorWrites.Count);
        if (shallowSupported) Assert.Equal((0.025f, 0.055f, 0.060f, 0.15f), material.ColorWrites["_ShallowColor"]);
        Assert.Equal(ApprovedFloats.Select(pair => pair.name).Append("_ShallowColor"), material.Checked);
        var tokens = ApprovedFloats.Where(pair => expected.ContainsKey(pair.name))
            .Select(pair => pair.name + "=" + pair.value.ToString(CultureInfo.InvariantCulture)).ToList();
        if (shallowSupported) tokens.Add("_ShallowColor=(0.025,0.055,0.06,0.15)");
        Assert.Equal("[water-shader] clear-shallows applied: " + (tokens.Count == 0 ? "none" : string.Join(" ", tokens)), diagnostic);
    }

    [Fact]
    public void LeavesDeepColorDynamicParametersFoamAlphaTexturesAndKeywordsUntouched()
    {
        var material = new TestMaterial();
        var preservedFloats = material.Floats.Where(pair => !ApprovedFloats.Any(tuning => tuning.name == pair.Key)).ToArray();
        var preservedColors = material.Colors.Where(pair => pair.Key != "_ShallowColor").ToArray();
        var textures = material.Textures.ToArray();
        var keywords = material.Keywords.ToArray();

        WaterLookRecipe.Apply(material);

        foreach (var (name, value) in preservedFloats) Assert.Equal(value, material.Floats[name]);
        foreach (var (name, value) in preservedColors) Assert.Equal(value, material.Colors[name]);
        Assert.Equal(textures, material.Textures);
        Assert.Equal(keywords, material.Keywords);
    }

    [Fact]
    public void DiagnosticExplicitlyReportsNoAppliedValuesWhenNothingIsSupported()
    {
        var material = new TestMaterial();
        material.Floats.Clear();
        material.Colors.Clear();

        Assert.Equal("[water-shader] clear-shallows applied: none", WaterLookRecipe.Apply(material));
        Assert.Empty(material.FloatWrites);
        Assert.Empty(material.ColorWrites);
    }

    [Fact]
    public void CachedSessionAppliesAndSummarizesRealRecipeOnlyOnce()
    {
        var runtime = new RecipeRuntime();
        var session = new WaterSession<TestTarget, TestMaterial>(runtime);
        session.Start(0);
        session.Tick(0);
        session.Tick(0.5);
        session.Tick(1);
        session.Tick(30);
        session.Start(31);
        session.Tick(31);

        Assert.Equal("[water-shader] clear-shallows applied: _WaveHeight=0.35 _WaveSpeed=1.1 _WaveCount=4 _NormalStrength=0.18 _FoamBaseAmount=0.25 _DepthVertical=4 _DepthHorizontal=1 _RefractionStrength=0.1 _RefractionChromaticAberration=0 _ShallowColor=(0.025,0.055,0.06,0.15)", Assert.Single(runtime.Diagnostics));
        var clone = Assert.Single(runtime.Clones);
        Assert.Equal(1, runtime.Configurations);
        Assert.Equal(10, clone.Checked.Count);
        Assert.Equal(ApprovedFloats.Select(pair => pair.name).Append("_ShallowColor"), clone.SetterCalls);
        Assert.Empty(runtime.Original.Checked);
        Assert.Empty(runtime.Original.FloatWrites);
        Assert.Empty(runtime.Original.ColorWrites);
        Assert.Equal(WaterState.Applied, session.State);
    }

    [Fact]
    public void EachNewCloneGetsOneRealRecipeConfigurationAndOneDiagnostic()
    {
        var runtime = new RecipeRuntime();
        var session = new WaterSession<TestTarget, TestMaterial>(runtime);
        session.Start(0);
        session.Tick(0);
        var first = Assert.Single(runtime.Clones);
        session.Stop();
        session.Stop();
        session.Tick(100);
        Assert.Same(runtime.Original, runtime.Target.Material);
        Assert.False(first.Alive);
        Assert.Single(runtime.Diagnostics);

        session.Start(101);
        session.Start(101);
        session.Tick(101);
        session.Tick(101.5);
        session.Tick(102);

        Assert.Equal(2, runtime.Clones.Count);
        Assert.NotSame(first, runtime.Clones[1]);
        Assert.Same(runtime.Clones[1], runtime.Target.Material);
        Assert.Equal(2, runtime.Configurations);
        Assert.Equal(2, runtime.Diagnostics.Count);
        Assert.Equal(runtime.Diagnostics[0], runtime.Diagnostics[1]);
        Assert.All(runtime.Clones, clone => Assert.Equal(10, clone.SetterCalls.Count));
        session.Stop();
        Assert.Same(runtime.Original, runtime.Target.Material);
        Assert.Equal(2, runtime.Destroys);
    }

    [Fact]
    public void RealRecipeSessionIdleTicksPerformNoRuntimeOrMaterialWorkAndAllocateNothing()
    {
        var runtime = new RecipeRuntime();
        var session = new WaterSession<TestTarget, TestMaterial>(runtime);
        session.Start(0);
        session.Tick(0);
        session.Tick(0.5);
        var clone = Assert.Single(runtime.Clones);
        int nativeCalls = runtime.Calls;
        int checks = clone.Checked.Count;
        int setters = clone.SetterCalls.Count;
        long before = GC.GetAllocatedBytesForCurrentThread();

        for (int i = 0; i < 10000; i++) session.Tick(0.5);

        long allocated = GC.GetAllocatedBytesForCurrentThread() - before;
        Assert.Equal(0, allocated);
        Assert.Equal(nativeCalls, runtime.Calls);
        Assert.Equal(checks, clone.Checked.Count);
        Assert.Equal(setters, clone.SetterCalls.Count);
        Assert.Equal(1, runtime.Configurations);
        Assert.Single(runtime.Diagnostics);
    }

    public static IEnumerable<object[]> EverySetterFailure()
    {
        foreach (string name in ApprovedFloats.Select(pair => pair.name).Append("_ShallowColor"))
        {
            yield return new object[] { name, false };
            yield return new object[] { name, true };
        }
    }

    [Theory]
    [MemberData(nameof(EverySetterFailure))]
    public void RealRecipeFailureRetainsUnattachedCloneUntilExistingCleanupDeadline(string property, bool afterWrite)
    {
        var runtime = new RecipeRuntime { ThrowOnProperty = property, ThrowAfterWrite = afterWrite };
        var session = new WaterSession<TestTarget, TestMaterial>(runtime);
        var originalFloats = runtime.Original.Floats.ToArray();
        var originalColors = runtime.Original.Colors.ToArray();
        string[] names = ApprovedFloats.Select(pair => pair.name).Append("_ShallowColor").ToArray();
        int failureIndex = Array.IndexOf(names, property);
        session.Start(0);

        Assert.Null(Record.Exception(() => session.Tick(0)));

        var failedClone = Assert.Single(runtime.Clones);
        Assert.Equal(WaterState.Faulted, session.State);
        Assert.IsType<InvalidOperationException>(session.LastError);
        Assert.Same(runtime.Original, runtime.Target.Material);
        Assert.True(failedClone.Alive);
        Assert.Equal(0, runtime.Destroys);
        Assert.Equal(names.Take(failureIndex + 1), failedClone.Checked);
        Assert.Equal(names.Take(failureIndex + 1), failedClone.SetterCalls);
        int completedWrites = failureIndex + (afterWrite ? 1 : 0);
        Assert.Equal(ApprovedFloats.Take(completedWrites).OrderBy(pair => pair.name)
            .Select(pair => new KeyValuePair<string, float>(pair.name, pair.value)), failedClone.FloatWrites.OrderBy(pair => pair.Key));
        Assert.Equal(completedWrites > ApprovedFloats.Length ? 1 : 0, failedClone.ColorWrites.Count);
        if (failedClone.ColorWrites.Count != 0) Assert.Equal((0.025f, 0.055f, 0.060f, 0.15f), failedClone.Colors["_ShallowColor"]);
        Assert.Empty(runtime.Diagnostics);
        int calls = runtime.Calls;
        session.Tick(4.999);
        Assert.Equal(calls, runtime.Calls);
        Assert.Single(runtime.Clones);
        session.Tick(5);
        Assert.Equal(1, runtime.Destroys);
        Assert.False(failedClone.Alive);
        Assert.Single(runtime.Clones);
        Assert.Same(runtime.Original, runtime.Target.Material);
        Assert.Equal(WaterState.Waiting, session.State);
        calls = runtime.Calls;
        session.Tick(9.999);
        Assert.Equal(calls, runtime.Calls);

        runtime.ThrowOnProperty = null;
        session.Tick(10);
        Assert.Equal(WaterState.Applied, session.State);
        Assert.Same(runtime.Clones[1], runtime.Target.Material);
        Assert.Equal(2, runtime.Clones.Count);
        Assert.Equal(2, runtime.Configurations);
        Assert.Single(runtime.Diagnostics);
        Assert.Equal(ApprovedFloats.ToDictionary(pair => pair.name, pair => pair.value).OrderBy(pair => pair.Key),
            runtime.Clones[1].FloatWrites.OrderBy(pair => pair.Key));
        Assert.Equal((0.025f, 0.055f, 0.060f, 0.15f), runtime.Clones[1].Colors["_ShallowColor"]);
        session.Stop();
        Assert.Same(runtime.Original, runtime.Target.Material);
        Assert.Equal(2, runtime.Destroys);
        Assert.Equal(originalFloats, runtime.Original.Floats);
        Assert.Equal(originalColors, runtime.Original.Colors);
        Assert.Empty(runtime.Original.Checked);
    }

    private sealed class TestTarget
    {
        public TestMaterial? Material;
    }

    // Executes the linked production session and recipe, counting every simulated native call.
    private sealed class RecipeRuntime : IWaterRuntime<TestTarget, TestMaterial>
    {
        public TestMaterial Original { get; } = new();
        public TestTarget Target { get; }
        public List<TestMaterial> Clones { get; } = new();
        public List<string> Diagnostics { get; } = new();
        public int Calls, Configurations, Destroys;
        public string? ThrowOnProperty;
        public bool ThrowAfterWrite;

        public RecipeRuntime() { Target = new TestTarget { Material = Original }; }
        public TestTarget Discover() { Calls++; return Target; }
        public bool TargetAlive(TestTarget value) { Calls++; return true; }
        public bool MaterialAlive(TestMaterial value) { Calls++; return value.Alive; }
        public TestMaterial? ReadMaterial(TestTarget target) { Calls++; return target.Material; }
        public TestMaterial CreateMaterial(TestMaterial template)
        {
            Calls++;
            var clone = new TestMaterial(template) { ThrowOnProperty = ThrowOnProperty, ThrowAfterWrite = ThrowAfterWrite };
            Clones.Add(clone);
            return clone;
        }
        public void ConfigureMaterial(TestMaterial material)
        {
            Calls++;
            Configurations++;
            Diagnostics.Add(WaterLookRecipe.Apply(material));
        }
        public void WriteMaterial(TestTarget target, TestMaterial? material) { Calls++; target.Material = material; }
        public bool SameMaterial(TestMaterial? left, TestMaterial right) { Calls++; return ReferenceEquals(left, right); }
        public void DestroyMaterial(TestMaterial material) { Calls++; Destroys++; material.Alive = false; }
    }

    private sealed class TestMaterial : IWaterLookMaterial
    {
        public Dictionary<string, float> Floats { get; } = new()
        {
            ["_WaveHeight"] = 1f,
            ["_WaveSpeed"] = 1.3f,
            ["_WaveCount"] = 4f,
            ["_NormalStrength"] = 0.165f,
            ["_FoamBaseAmount"] = 0.317f,
            ["_DepthVertical"] = 0.75f,
            ["_DepthHorizontal"] = 3.5f,
            ["_RefractionStrength"] = 0.474f,
            ["_RefractionChromaticAberration"] = 0.2f,
            // Deliberately support old aliases too: HasProperty alone cannot authorize them.
            ["_WaveFrequency"] = 0.7f,
            ["_Absorption"] = 0.91f,
            ["_DepthExp"] = 1.75f,
            ["_ZWrite"] = 1f,
            ["native-reflection-state-sentinel"] = 0.62f,
            ["native-dynamic-sentinel"] = 123.456f,
        };
        public Dictionary<string, (float r, float g, float b, float alpha)> Colors { get; } = new()
        {
            ["_ShallowColor"] = (0.019607844f, 0.036572091f, 0.039215688f, 1f),
            ["_BaseColor"] = (0.010234951f, 0.014551431f, 0.018867910f, 1f),
            ["_DeepColor"] = (0.1f, 0.2f, 0.3f, 0.4f),
            ["_WaterColorShallow"] = (0.2f, 0.3f, 0.4f, 0.5f),
            ["_WaterColorDeep"] = (0.3f, 0.4f, 0.5f, 0.6f),
            ["native-foam-alpha-sentinel"] = (0.4f, 0.5f, 0.6f, 0.7f),
        };
        public Dictionary<string, float> FloatWrites { get; } = new();
        public Dictionary<string, (float r, float g, float b, float alpha)> ColorWrites { get; } = new();
        public Dictionary<string, object> Textures { get; } = new() { ["native-texture-sentinel"] = new object() };
        public HashSet<string> Keywords { get; } = new() { "native-keyword-sentinel" };
        public List<string> Checked { get; } = new();
        public List<string> SetterCalls { get; } = new();
        public bool Alive = true;
        public string? ThrowOnProperty;
        public bool ThrowAfterWrite;
        private readonly HashSet<string> _checked = new();

        public TestMaterial(TestMaterial? template = null)
        {
            if (template == null) return;
            Floats.Clear();
            foreach (var (name, value) in template.Floats) Floats.Add(name, value);
            Colors.Clear();
            foreach (var (name, value) in template.Colors) Colors.Add(name, value);
            Textures.Clear();
            foreach (var (name, value) in template.Textures) Textures.Add(name, value);
            Keywords.Clear();
            Keywords.UnionWith(template.Keywords);
        }

        public bool HasProperty(string name)
        {
            Checked.Add(name);
            _checked.Add(name);
            return Floats.ContainsKey(name) || Colors.ContainsKey(name);
        }

        private void RequireSupported(string name, bool present)
        {
            Assert.Contains(name, _checked);
            Assert.True(present, $"Access to unsupported property {name}");
        }

        public void SetFloat(string name, float value)
        {
            RequireSupported(name, Floats.ContainsKey(name));
            BeforeSet(name);
            Floats[name] = FloatWrites[name] = value;
            AfterSet(name);
        }

        public void SetColor(string name, float r, float g, float b, float alpha)
        {
            RequireSupported(name, Colors.ContainsKey(name));
            BeforeSet(name);
            Colors[name] = ColorWrites[name] = (r, g, b, alpha);
            AfterSet(name);
        }

        private void BeforeSet(string name)
        {
            SetterCalls.Add(name);
            if (name == ThrowOnProperty && !ThrowAfterWrite) throw new InvalidOperationException("Native material setter failed before writing.");
        }

        private void AfterSet(string name)
        {
            if (name == ThrowOnProperty && ThrowAfterWrite) throw new InvalidOperationException("Native material setter failed after writing.");
        }
    }
}
