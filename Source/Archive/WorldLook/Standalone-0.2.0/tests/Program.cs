using System.Text.RegularExpressions;
using WaterShader;
using WorldLook;

int checks = 0;
int failed = 0;
string root = args.Length == 1 ? Path.GetFullPath(args[0])
    : throw new ArgumentException("Pass the standalone WorldLook project directory.");

void Check(bool condition, string message)
{
    checks++;
    if (!condition) throw new InvalidOperationException(message);
}

void Run(string name, Action test)
{
    try { test(); Console.WriteLine("PASS " + name); }
    catch (Exception ex) { failed++; Console.WriteLine("FAIL " + name + ": " + ex.Message); }
}

string Source(string path) => File.ReadAllText(Path.Combine(root, path));
string Active(string text) => Regex.Replace(text, @"/\*[\s\S]*?\*/|//[^\r\n]*", "");

Run("recipe keeps a visible water body without a hard mirror", () =>
{
    var material = new FakeLookMaterial();
    ReferenceWaterLook.Apply(material);
    Check(material.Floats.TryGetValue("_SurfaceOpacity", out float opacity) && opacity >= .1f,
        "A positive independent _SurfaceOpacity must be applied.");
    Check(material.Floats.TryGetValue("_ReflectionStrength", out float reflection)
          && reflection > 0f && reflection <= .25f, "Keep only a weak environment-color highlight.");
    Check(material.Floats["_Absorption"] == .1f, "Existing shallow absorption tuning changed.");
    Check(material.Floats["_RippleStrength"] == .28f, "Existing wave-detail tuning changed.");
});

Run("recipe respects absent properties", () =>
{
    var material = new FakeLookMaterial { AllowProperties = false };
    string log = ReferenceWaterLook.Apply(material);
    Check(material.Floats.Count == 0 && material.Colors.Count == 0, "Wrote an absent property.");
    Check(log.EndsWith("none", StringComparison.Ordinal), "No-op recipe must say none.");
});

Run("shader and C# water property contracts match", () =>
{
    string shader = Active(Source("unity/Assets/S1Mods/ReferenceWater.shader"));
    foreach (string property in ReferenceWaterPolicy.RequiredShaderProperties)
        Check(Regex.IsMatch(shader, @"\b" + Regex.Escape(property) + @"\s*\("), "Undeclared property: " + property);
    Check(!ReferenceWaterPolicy.RequiredShaderProperties.Contains("_ReflectionTex"), "Retired object texture still required.");
    Check(ReferenceWaterPolicy.RequiredShaderProperties.Contains("_SurfaceOpacity"), "Old shader bundle would silently pass the new ABI.");
    Check(ReferenceWaterPolicy.MissingRequiredProperties(_ => true).Length == 0, "Complete contract rejected.");
    Check(ReferenceWaterPolicy.MissingRequiredProperties(p => p != "_SurfaceOpacity").SequenceEqual(new[] { "_SurfaceOpacity" }),
        "Stale bundles must be rejected by their missing body-opacity property.");
});

Run("water runtime creates no reflection camera or render target", () =>
{
    string runtime = Active(Source("src/ReferenceWaterRuntime.cs"));
    Check(!Regex.IsMatch(runtime, @"\bRenderSingleCamera\s*\("), "Nested reflection rendering still present.");
    Check(!Regex.IsMatch(runtime, @"\bnew\s+RenderTexture\b"), "An object-reflection render target is still allocated.");
    Check(!Regex.IsMatch(runtime, @"\bAddComponent\s*<\s*Camera\s*>"), "An owned camera is still allocated.");
    Check(!runtime.Contains("add_beginCameraRendering", StringComparison.Ordinal), "Retired render callback is still subscribed.");
    Check(!runtime.Contains("SetGlobal", StringComparison.Ordinal), "Water must not write global reflection state.");
});

Run("water shader cannot sample object-reflection images", () =>
{
    string shader = Active(Source("unity/Assets/S1Mods/ReferenceWater.shader"));
    Check(!shader.Contains("_ReflectionTex", StringComparison.Ordinal), "Object-reflection texture still present.");
    Check(!shader.Contains("_ReflectionVP", StringComparison.Ordinal), "Reflection projection still present.");
    Check(!shader.Contains("_ReflectionAvailable", StringComparison.Ordinal), "Old planar availability still present.");
    Check(shader.Contains("_SurfaceOpacity", StringComparison.Ordinal), "Independent body opacity is missing.");
    Check(!Regex.IsMatch(shader, @"bodyOpacity\s*=\s*[^;]*\*\s*depthValid"), "Invalid depth still erases the water body.");
});

Run("water environment updates are driven by the mod update", () =>
{
    string mod = Active(Source("src/WaterShaderMod.cs"));
    Check(Regex.IsMatch(mod, @"_reference\?\.Tick\s*\("), "No update remains after removal of the camera callback.");
    Check(mod.Contains("reflectionMode", StringComparison.Ordinal), "Diagnostics must identify the environment-only path.");
});

Run("sky restore cannot deliberately assign null", () =>
{
    string sky = Active(Source("src/CloudSkyRuntime.cs"));
    Check(!Regex.IsMatch(sky, @"Assign\([^;]*\?[^;]*:\s*null\s*\)"), "Restore still writes null when the original dies.");
    Check(sky.Contains("backup", StringComparison.OrdinalIgnoreCase), "No retained native-shader backup exists.");
});

Run("reference eligibility remains fail-closed", () =>
{
    for (int bits = 0; bits < 8; bits++)
    {
        bool loaded = (bits & 1) != 0, supported = (bits & 2) != 0, contract = (bits & 4) != 0;
        Check(ReferenceWaterPolicy.CanApplyReference(loaded, supported, contract) == (bits == 7), "Reference eligibility changed.");
    }
});

Run("water session restores its own material and preserves a foreign takeover", () =>
{
    var runtime = new FakeWaterRuntime();
    var session = new WaterSession<object, FakeMaterial>(runtime);
    var native = runtime.Current;
    session.Start(0); session.Tick(0);
    Check(session.State == WaterState.Applied, "Not applied.");
    Check(!ReferenceEquals(native, runtime.Current), "No owned material created.");
    session.Stop(); session.Stop();
    Check(ReferenceEquals(native, runtime.Current), "Original was not restored.");
    Check(runtime.Destroyed.Count == 1, "Repeated stop destroyed more than once.");
    session.Start(1); session.Tick(1);
    var foreign = new FakeMaterial(); runtime.Current = foreign;
    session.Stop();
    Check(ReferenceEquals(foreign, runtime.Current), "Foreign material was overwritten.");
});

Run("water session retains material when restoration cannot be confirmed", () =>
{
    var runtime = new FakeWaterRuntime();
    var session = new WaterSession<object, FakeMaterial>(runtime);
    session.Start(0); session.Tick(0);
    var owned = runtime.Current;
    runtime.FailWrite = true;
    session.Stop();
    Check(session.State == WaterState.Faulted, "Restore failure not reported.");
    Check(ReferenceEquals(owned, runtime.Current), "Failed restore lost the visible material.");
    Check(runtime.Destroyed.Count == 0, "Possibly attached material destroyed.");
    runtime.FailWrite = false;
    session.Stop();
    Check(session.State == WaterState.Stopped && runtime.Destroyed.Count == 1, "Retained cleanup cannot recover.");
});

Run("sky lease restores live original and releases exactly once", () =>
{
    var slot = new FakeSlot(); var lease = slot.CreateLease();
    Check(lease.Publish(), "Publish failed.");
    Check(lease.IsApplied, "Lease not applied.");
    Check(lease.Restore() == MaterialLeaseState.RestoredOriginal, "Live original not preferred.");
    Check(ReferenceEquals(slot.Current, slot.Original), "Wrong restore target.");
    Check(lease.ReleaseOwned() && lease.ReleaseOwned(), "Release is not idempotent.");
    Check(slot.DestroyCount == 1, "Repeated release destroys twice.");
    Check(slot.Backup.Alive, "Lease must not destroy a shared backup.");
});

Run("water restore cannot clear the slot when native original dies", () =>
{
    var runtime = new FakeWaterRuntime();
    var original = runtime.Current!;
    var session = new WaterSession<object, FakeMaterial>(runtime);
    session.Start(0); session.Tick(0);
    var owned = runtime.Current;
    original.Alive = false;
    session.Stop();
    Check(session.State == WaterState.Faulted, "Missing native original not reported.");
    Check(ReferenceEquals(owned, runtime.Current) && runtime.Destroyed.Count == 0, "Missing original erased water.");
});

Run("sky lease uses a retained backup when original dies", () =>
{
    var slot = new FakeSlot(); var lease = slot.CreateLease(); Check(lease.Publish(), "Publish failed.");
    slot.Original.Alive = false;
    Check(lease.Restore() == MaterialLeaseState.RestoredBackup, "Backup not used.");
    Check(ReferenceEquals(slot.Current, slot.Backup), "Backup not attached.");
    Check(lease.ReleaseOwned() && slot.Backup.Alive, "Attached backup was destroyed.");
    Check(slot.NullWrites == 0, "Wrote null.");
});

Run("unreadable original candidate cannot erase the sky", () =>
{
    var slot = new FakeSlot(); var lease = slot.CreateLease(); Check(lease.Publish(), "Publish failed.");
    slot.Original.Unreadable = true;
    Check(lease.Restore() == MaterialLeaseState.RestoredBackup, "Known valid backup should replace unreadable candidate.");
    Check(lease.ReleaseOwned() && slot.NullWrites == 0, "Invalid release.");
});

Run("missing original and backup retain a visible owned sky", () =>
{
    var slot = new FakeSlot(); var lease = slot.CreateLease(); Check(lease.Publish(), "Publish failed.");
    slot.Original.Alive = slot.Backup.Alive = false;
    Check(lease.Restore() == MaterialLeaseState.Deferred, "Restore should defer.");
    Check(!lease.ReleaseOwned(), "Possibly attached material released.");
    Check(ReferenceEquals(slot.Current, slot.Owned) && slot.DestroyCount == 0 && slot.NullWrites == 0, "Visible sky was erased.");
});

Run("failed publish after setter mutation remains recoverable", () =>
{
    var slot = new FakeSlot { ThrowAfterWrite = true }; var lease = slot.CreateLease();
    Check(!lease.Publish() && ReferenceEquals(slot.Current, slot.Owned), "Injected partial publication not observed.");
    slot.ThrowAfterWrite = false;
    Check(lease.Restore() == MaterialLeaseState.RestoredOriginal && lease.ReleaseOwned(), "Partial publish cannot restore.");
});

Run("failed restore after setter mutation reconciles by readback", () =>
{
    var slot = new FakeSlot(); var lease = slot.CreateLease(); Check(lease.Publish(), "Publish failed.");
    slot.ThrowAfterWrite = true;
    Check(lease.Restore() == MaterialLeaseState.Deferred, "Unknown write must defer.");
    Check(ReferenceEquals(slot.Current, slot.Original), "Injected setter did not change the slot.");
    slot.ThrowAfterWrite = false;
    Check(lease.Restore() == MaterialLeaseState.RestoredOriginal && lease.ReleaseOwned(), "Readback did not reconcile the partial restore.");
});

Run("no-op setter and unreadable slot never permit destruction", () =>
{
    var slot = new FakeSlot(); var lease = slot.CreateLease(); Check(lease.Publish(), "Publish failed.");
    slot.IgnoreWrites = true;
    Check(lease.Restore() == MaterialLeaseState.Deferred && !lease.ReleaseOwned(), "No-op restore accepted.");
    slot.IgnoreWrites = false; slot.FailRead = true;
    Check(lease.Restore() == MaterialLeaseState.Deferred && !lease.ReleaseOwned(), "Uncertain slot treated as detached.");
    Check(slot.DestroyCount == 0, "Destroyed on unreadable slot.");
    slot.FailRead = false;
    Check(lease.Restore() == MaterialLeaseState.RestoredOriginal && lease.ReleaseOwned(), "Deferred restore cannot recover.");
});

Run("foreign native material and a foreign shader are preserved", () =>
{
    var slot = new FakeSlot(); var lease = slot.CreateLease(); Check(lease.Publish(), "Publish failed.");
    var foreign = new FakeSlotMaterial { Native = true }; slot.Current = foreign;
    Check(!lease.ValidateOwnership() && lease.Relinquished, "Same native shader incorrectly proves ownership.");
    Check(lease.Restore() == MaterialLeaseState.ForeignPreserved && lease.ReleaseOwned(), "Foreign material not preserved.");
    Check(ReferenceEquals(slot.Current, foreign), "Overwrote foreign native material.");
    slot = new FakeSlot(); lease = slot.CreateLease(); Check(lease.Publish(), "Publish failed.");
    slot.Owned.OwnedShader = false;
    Check(!lease.ValidateOwnership() && lease.Relinquished, "Foreign shader not recognized.");
    lease.Restore();
    Check(lease.ReleaseOwned() && slot.DestroyCount == 0 && ReferenceEquals(slot.Current, slot.Owned), "Foreign-shader material was changed/destroyed.");
});

Run("old world and another known consumer block unsafe cleanup", () =>
{
    var slot = new FakeSlot(); var lease = slot.CreateLease(); Check(lease.Publish(), "Publish failed.");
    slot.SameWorld = false;
    Check(lease.Restore() == MaterialLeaseState.Deferred && slot.WriteCount == 1, "Old generation wrote into new world.");
    slot.SameWorld = true;
    Check(lease.Restore() == MaterialLeaseState.RestoredOriginal, "Restore did not recover.");
    slot.KnownAlias = true;
    Check(!lease.ReleaseOwned() && slot.DestroyCount == 0, "Known alias was destroyed.");
    slot.KnownAlias = false;
    Check(lease.ReleaseOwned(), "Detached alias cannot be collected.");
});

Run("publication validates before seizing a changed source", () =>
{
    var slot = new FakeSlot(); var lease = slot.CreateLease();
    var foreign = new FakeSlotMaterial { Native = true }; slot.Current = foreign;
    Check(!lease.Publish() && slot.WriteCount == 0 && ReferenceEquals(slot.Current, foreign), "Publication seized foreign source.");
    Check(lease.Restore() == MaterialLeaseState.ForeignPreserved && lease.ReleaseOwned(), "Unpublished clone cannot be released.");
    slot = new FakeSlot(); slot.Backup.Native = false; lease = slot.CreateLease();
    Check(!lease.Publish() && slot.WriteCount == 0, "Invalid native backup was published.");
});

Run("sky adapter preserves generation fences and backup reuse", () =>
{
    string sky = Active(Source("src/CloudSkyRuntime.cs"));
    string reset = Regex.Match(sky, @"internal void Reset\(\)\s*\{([\s\S]*?)\n    \}").Groups[1].Value;
    Check(reset.Length > 0 && !reset.Contains("_bindings.Clear", StringComparison.Ordinal), "Reset loses pending leases.");
    Check(!reset.Contains("_ownershipFence.BeginWorld", StringComparison.Ordinal), "F7 resets a foreign-owner fence.");
    Check(sky.Contains("group.AdoptNativeSource(source)", StringComparison.Ordinal), "Native changes on a backup are not adopted.");
    Check(sky.Contains("candidate.MatchesSource(source)", StringComparison.Ordinal), "Reenable builds backup chains.");
    Check(sky.IndexOf("_bindings.Add(binding)", StringComparison.Ordinal) < sky.IndexOf("binding.Lease.Publish(", StringComparison.Ordinal), "Publication is not tracked first.");
    Check(!sky.Contains("renderer.enabled = false", StringComparison.Ordinal), "Unproven dome suppression remains.");
});

Run("water weight oracle stays visible and conserves composition", () =>
{
    var material = new FakeLookMaterial(); ReferenceWaterLook.Apply(material);
    double minimum = material.Floats["_SurfaceOpacity"];
    string shader = Active(Source("unity/Assets/S1Mods/ReferenceWater.shader"));
    Check(shader.Contains("lerp(1.0, saturate(pow(absorb, 2.2)), depthValid)", StringComparison.Ordinal), "Shader/oracle depth rule drifted.");
    Check(shader.Contains("lerp(clamp(_SurfaceOpacity, 0.1, 0.65), 0.89, dye)", StringComparison.Ordinal), "Shader/oracle body rule drifted.");
    for (int valid = 0; valid < 2; valid++)
        for (int d = 0; d <= 20; d++)
            for (int f = 0; f <= 20; f++)
                foreach (double strength in new[] { 0.0, .12, .25 })
                {
                    double dye = valid == 0 ? 1 : d / 20.0;
                    double reflection = (f / 20.0) * strength;
                    double bodyOpacity = minimum + (.89 - minimum) * dye;
                    double body = (1 - reflection) * bodyOpacity;
                    double alpha = reflection + body;
                    Check(bodyOpacity >= minimum - 1e-6 && alpha >= .1, "Missing depth or zero reflection erased water.");
                    Check(alpha <= 1 && body >= 0 && reflection >= 0, "Out-of-range composition.");
                    Check(Math.Abs((1 - alpha) + body + reflection - 1) < 1e-9, "Weights do not sum to one.");
                }
});

Run("review: relinquished lease cannot reclaim a reused clone", () =>
{
    var slot = new FakeSlot(); var lease = slot.CreateLease(); Check(lease.Publish(), "Publish failed.");
    slot.Current = new FakeSlotMaterial { Native = true };
    Check(!lease.ValidateOwnership() && lease.Relinquished, "Foreign takeover not recognized.");
    slot.KnownAlias = true; lease.Restore();
    Check(!lease.ReleaseOwned(), "Injected known alias did not defer release.");
    slot.Current = slot.Owned; slot.KnownAlias = false;
    int writes = slot.WriteCount;
    lease.Restore();
    Check(slot.WriteCount == writes && ReferenceEquals(slot.Current, slot.Owned), "Relinquished lease reclaimed the reused clone.");
    Check(!lease.ReleaseOwned() && slot.DestroyCount == 0, "Reused clone was destroyed.");
});

Run("review: sky alias registry covers originally unbound native slots", () =>
{
    string sky = Active(Source("src/CloudSkyRuntime.cs"));
    Check(sky.Contains("KnownMaterialConsumers.Contains", StringComparison.Ordinal), "Cleanup only sees registered binding indices.");
    Check(sky.Contains("_knownRenderers", StringComparison.Ordinal), "Skipped native renderer slots are not tracked.");
});

Run("review: generation-specific fences and transactional initialization", () =>
{
    string sky = Active(Source("src/CloudSkyRuntime.cs"));
    Check(sky.Contains("MaterialSlotFence", StringComparison.Ordinal), "Foreign fence has no world generation.");
    Check(sky.Contains("current.WorldHandle == _worldHandle", StringComparison.Ordinal), "Old pending key blocks a new-world binding.");
    Check(sky.Contains("Lease.Publish(() => group.Add(binding))", StringComparison.Ordinal), "Native bridge initialization precedes lease registration.");
    Check(sky.Contains("NativeMaterialBridgePolicy.Decide", StringComparison.Ordinal), "No tested source/own-write tracking seam.");
});

Run("review: all known native slots protect clones and backups", () =>
{
    var native = new FakeSlotMaterial { Native = true };
    var clone = new FakeSlotMaterial { OwnedShader = true };
    var backup = new FakeSlotMaterial { Native = true };
    FakeSlotMaterial?[] slots = { native, clone, null, backup };
    Check(KnownMaterialConsumers.Contains(clone, slots.Length, i => slots[i], ReferenceEquals), "Clone alias in originally unbound slot missed.");
    Check(KnownMaterialConsumers.Contains(backup, slots.Length, i => slots[i], ReferenceEquals), "Backup alias in originally unbound slot missed.");
    slots[1] = null; slots[3] = null;
    Check(!KnownMaterialConsumers.Contains(clone, slots.Length, i => slots[i], ReferenceEquals), "Detached clone still reported attached.");
    bool oversized = false, unknown = false;
    try { KnownMaterialConsumers.Contains(clone, KnownMaterialConsumers.MaximumSlots + 1, _ => null, ReferenceEquals); }
    catch (InvalidOperationException) { oversized = true; }
    try { KnownMaterialConsumers.Contains(clone, 2, _ => throw new IOException("Unreadable native material slot."), ReferenceEquals); }
    catch (IOException) { unknown = true; }
    Check(oversized && unknown, "Oversized/unreadable consumers incorrectly became a negative alias result.");
});

Run("review: late cleanup cannot fence the next world", () =>
{
    var fence = new MaterialSlotFence();
    fence.BeginWorld(-14464); fence.ObserveForeign(-14464, "RenderSettings.skybox");
    Check(fence.IsBlocked(-14464, "RenderSettings.skybox"), "Foreign ownership not fenced.");
    fence.BeginWorld(200); fence.ObserveForeign(-14464, "RenderSettings.skybox");
    Check(!fence.IsBlocked(200, "RenderSettings.skybox"), "Old pending release fenced the new scene.");
    fence.ObserveForeign(200, "RenderSettings.skybox"); fence.BeginWorld(200);
    Check(fence.IsBlocked(200, "RenderSettings.skybox"), "Same world/F7 reset the fence.");
    fence.BeginWorld(201);
    Check(!fence.IsBlocked(201, "RenderSettings.skybox"), "New actual scene remained fenced.");
});

Run("review: property initialization failure retains tracked allocations", () =>
{
    var slot = new FakeSlot(); var lease = slot.CreateLease();
    bool initialized = false;
    Check(!lease.Publish(() => { initialized = true; throw new IOException("Injected native bridge property setter."); }), "Initialization failure was hidden.");
    Check(initialized && slot.WriteCount == 0 && ReferenceEquals(slot.Current, slot.Original), "Failed initialization published a partial clone.");
    Check(lease.Restore() == MaterialLeaseState.RestoredOriginal && lease.ReleaseOwned(), "Unpublished allocation could not be cleaned up.");
    Check(slot.DestroyCount == 1 && slot.Backup.Alive, "Owned clone lost or shared backup destroyed.");
});

Run("review: own backup writes cannot erase the next native update", () =>
{
    var first = NativeMaterialBridgePolicy.Decide(0, 0, 0, 1, _ => 1, (int a, int b) => a == b, true);
    Check(first.value == 1 && first.sourceSnapshot == 1, "Own backup write was not tracked as the new source snapshot.");
    var second = NativeMaterialBridgePolicy.Decide(first.sourceSnapshot, first.value, 1, 1, _ => 2,
        (int a, int b) => a == b, true);
    Check(second.value == 2 && second.sourceSnapshot == 2, "Echo of first write overwrote the second native clone update.");
    var native = NativeMaterialBridgePolicy.Decide(2, 2, 3, 1,
        _ => throw new InvalidOperationException("A changed native source must not inspect clone getters."),
        (int a, int b) => a == b, false);
    Check(native.value == 3 && native.sourceSnapshot == 3, "Actual native-source drift lost priority.");
});

Run("repeated backup restores reuse a single native backup", () =>
{
    var slot = new FakeSlot(); slot.Original.Alive = false;
    slot.Current = slot.Backup;
    for (int i = 0; i < 50; i++)
    {
        var owned = new FakeSlotMaterial { OwnedShader = true };
        var lease = new MaterialSlotLease<FakeSlotMaterial>(slot, slot.Backup, slot.Original, slot.Backup, owned);
        Check(lease.Publish(), "Backup-source reenable failed.");
        Check(lease.Restore() == MaterialLeaseState.RestoredBackup, "Backup not reused.");
        Check(lease.ReleaseOwned() && ReferenceEquals(slot.Current, slot.Backup), "Reused backup detached or destroyed.");
        Check(slot.Backup.Alive, "Repeated toggle destroyed native backup.");
    }
    Check(slot.DestroyCount == 50, "Clone cleanup count differs across toggles.");
});

Run("review: unreadable old consumer does not block new discovery", () =>
{
    string sky = Active(Source("src/CloudSkyRuntime.cs"));
    Check(sky.Contains("KnownMaterialConsumers.Register", StringComparison.Ordinal), "Strict old-renderer comparison can abort new discovery.");
    var old = new FakeSlotMaterial { Unreadable = true };
    var dead = new FakeSlotMaterial { Alive = false };
    var fresh = new FakeSlotMaterial();
    var registry = new List<FakeSlotMaterial> { old, dead };
    bool Alive(FakeSlotMaterial material) => material.Unreadable ? throw new IOException("Unknown old proxy.") : material.Alive;
    Check(KnownMaterialConsumers.Register(registry, fresh, ReferenceEquals, Alive), "New valid consumer was not registered.");
    Check(registry.Count == 2 && registry.Contains(old) && registry.Contains(fresh) && !registry.Contains(dead),
        "Unknown consumer was discarded or a confirmed dead one was retained.");
    Check(!KnownMaterialConsumers.Register(registry, fresh, ReferenceEquals, Alive) && registry.Count == 2,
        "Repeated discovery duplicated the known consumer.");
    bool retained = false;
    try { foreach (var consumer in registry) if (Alive(consumer)) KnownMaterialConsumers.Contains(fresh, 0, _ => null, ReferenceEquals); }
    catch (IOException) { retained = true; }
    Check(retained, "Unknown retained consumer would not keep cleanup conservative.");
});

Console.WriteLine($"RESULT: checks={checks}, failures={failed}. Pure policy/delegate/source contracts only; no Unity rendering verified.");
return failed == 0 ? 0 : 1;

sealed class FakeLookMaterial : IWaterLookMaterial
{
    public bool AllowProperties = true;
    public Dictionary<string, float> Floats { get; } = new();
    public Dictionary<string, (float, float, float, float)> Colors { get; } = new();
    public bool HasProperty(string name) => AllowProperties;
    public void SetFloat(string name, float value) => Floats[name] = value;
    public void SetColor(string name, float r, float g, float b, float alpha) => Colors[name] = (r, g, b, alpha);
}

sealed class FakeMaterial { public bool Alive = true; }

sealed class FakeWaterRuntime : IWaterRuntime<object, FakeMaterial>
{
    private readonly object _target = new();
    public FakeMaterial? Current = new();
    public bool FailWrite;
    public List<FakeMaterial> Destroyed { get; } = new();
    public object Discover() => _target;
    public bool TargetAlive(object target) => true;
    public bool MaterialAlive(FakeMaterial material) => material.Alive;
    public FakeMaterial? ReadMaterial(object target) => Current;
    public FakeMaterial CreateMaterial(FakeMaterial original) => new();
    public void ConfigureMaterial(FakeMaterial material) { }
    public void WriteMaterial(object target, FakeMaterial? material)
    {
        if (FailWrite) throw new IOException("Injected failed material assignment.");
        Current = material;
    }
    public bool SameMaterial(FakeMaterial? a, FakeMaterial b) => ReferenceEquals(a, b);
    public void DestroyMaterial(FakeMaterial material) { Destroyed.Add(material); material.Alive = false; }
}

sealed class FakeSlotMaterial
{
    public bool Alive = true, Native, OwnedShader, Unreadable;
}

sealed class FakeSlot : IMaterialSlotRuntime<FakeSlotMaterial>
{
    public readonly FakeSlotMaterial Original = new() { Native = true };
    public readonly FakeSlotMaterial Backup = new() { Native = true };
    public readonly FakeSlotMaterial Owned = new() { OwnedShader = true };
    public FakeSlotMaterial? Current;
    public bool SameWorld = true, FailRead, ThrowAfterWrite, IgnoreWrites, KnownAlias;
    public int WriteCount, DestroyCount, NullWrites;
    public FakeSlot() { Current = Original; }
    public MaterialSlotLease<FakeSlotMaterial> CreateLease() => new(this, Original, Original, Backup, Owned);
    public bool CanRestore => SameWorld;
    public FakeSlotMaterial? ReadSlot() => FailRead ? throw new IOException("Injected slot read failure.") : Current;
    public void WriteSlot(FakeSlotMaterial material)
    {
        WriteCount++;
        if (material == null) { NullWrites++; throw new InvalidOperationException("Null assignment."); }
        if (!IgnoreWrites) Current = material;
        if (ThrowAfterWrite) throw new IOException("Injected exception after assignment.");
    }
    public bool IsAlive(FakeSlotMaterial material)
    {
        if (material.Unreadable) throw new IOException("Injected candidate read failure.");
        return material.Alive;
    }
    public bool IsOwnedShader(FakeSlotMaterial material) => IsAlive(material) && material.OwnedShader;
    public bool IsNativeShader(FakeSlotMaterial material) => IsAlive(material) && material.Native;
    public bool Same(FakeSlotMaterial? a, FakeSlotMaterial b)
    {
        if (a?.Unreadable == true || b.Unreadable) throw new IOException("Injected unreadable identity.");
        return ReferenceEquals(a, b);
    }
    public void DestroyOwned(FakeSlotMaterial material)
    {
        if (KnownAlias || ReferenceEquals(Current, material)) throw new InvalidOperationException("Material still consumed.");
        DestroyCount++; material.Alive = false;
    }
}
