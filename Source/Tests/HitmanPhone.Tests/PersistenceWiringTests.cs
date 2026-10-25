using Xunit;

namespace HitmanPhone.Tests;

// Supplementary source contracts. These do not execute the native save pipeline.
public class PersistenceWiringTests
{
    private static string Source(string name)
    {
        var dir = new DirectoryInfo(AppContext.BaseDirectory);
        while (dir != null)
        {
            string file = Path.Combine(dir.FullName, "Source", "Mods", "HitmanPhone", "src", name);
            if (File.Exists(file)) return File.ReadAllText(file);
            dir = dir.Parent;
        }
        string? root = Environment.GetEnvironmentVariable("HITMANPHONE_REPO");
        if (root != null) return File.ReadAllText(Path.Combine(root, "Source", "Mods", "HitmanPhone", "src", name));
        throw new InvalidOperationException("Repository root not found.");
    }

    [Fact]
    public void PendingLoadRetryDoesNotRestartTheLoadDeadline()
    {
        string source = Source("BountyRuntime.cs");
        int start = source.IndexOf("public static void Tick()", source.IndexOf("public static class SaveStateGuard", StringComparison.Ordinal), StringComparison.Ordinal);
        string tick = source[start..source.IndexOf("public static void Suspend()", start, StringComparison.Ordinal)];
        Assert.DoesNotContain("OnLoadComplete()", tick);
        Assert.Contains("TryInstallContext()", tick);
    }

    [Fact]
    public void NativeSaveCaptureIncludesTheParameterlessOverload()
    {
        string source = Source("Mod.cs");
        Assert.Contains("nameof(NativeSaveCheckpointPatch.PrefixDefault)", source);
        Assert.Contains("parameterTypes: Type.EmptyTypes", source);
    }

    [Fact]
    public void RegistrationAndUnsupportedStatusRemainReachableBeforeGameplayReadiness()
    {
        string mod = Source("Mod.cs");
        Assert.Contains("OnSceneWasLoaded(int buildIndex, string sceneName)", mod);
        Assert.Contains("BountyEvidenceItemRegistry.OnMainSceneLoaded()", mod);
        string update = mod[mod.IndexOf("public override void OnUpdate()", StringComparison.Ordinal)..];
        Assert.Contains("BountyEvidenceItemRegistry.TickRegistration()", update);
        Assert.True(update.IndexOf("BountyEvidenceItemRegistry.TickRegistration()", StringComparison.Ordinal) <
                    update.IndexOf("if (!SaveStateGuard.IsReady)", StringComparison.Ordinal));
        Assert.Contains("NotifyMultiplayerUnsupportedOnce()", update);
        string runtime = Source("BountyRuntime.cs");
        int preload = runtime.IndexOf("private static void OnPreLoad()", StringComparison.Ordinal);
        Assert.Contains("BountyEvidenceItemRegistry.InvalidateRegistration()", runtime[preload..runtime.IndexOf("private static void OnLoadComplete()", preload, StringComparison.Ordinal)]);
        Assert.Contains("BountyEvidenceItemRegistry.EnsureRegistered()", runtime[preload..runtime.IndexOf("private static void OnLoadComplete()", preload, StringComparison.Ordinal)]);
    }

    [Fact]
    public void PhoneHomeDoesNotDiscoverAHitmanTrackerApp()
    {
        var dir = new DirectoryInfo(AppContext.BaseDirectory);
        while (dir != null)
        {
            string src = Path.Combine(dir.FullName, "Source", "Mods", "HitmanPhone", "src");
            if (Directory.Exists(src))
            {
                foreach (string file in Directory.GetFiles(src, "*.cs", SearchOption.AllDirectories))
                {
                    if (file.Contains($"{Path.DirectorySeparatorChar}bin{Path.DirectorySeparatorChar}") ||
                        file.Contains($"{Path.DirectorySeparatorChar}obj{Path.DirectorySeparatorChar}"))
                        continue;
                    string text = File.ReadAllText(file);
                    Assert.DoesNotContain(": PhoneApp", text);
                    Assert.DoesNotContain("\"Hitman Tracker\"", text);
                }
                return;
            }
            dir = dir.Parent;
        }
        throw new InvalidOperationException("Repository root not found.");
    }

    [Fact]
    public void RegistrationBindsTheDefinitionResolvedByTheActualRegistry()
    {
        string source = Source("BountyEvidence.cs");
        int start = source.IndexOf("private static bool TryCreateAndRegister(", StringComparison.Ordinal);
        string create = source[start..source.IndexOf("public static S1ItemFramework.IntegerItemInstance? Spawn", start, StringComparison.Ordinal)];
        Assert.Contains("S1Registry.GetItem(id)", create);
        Assert.Contains("registered.TryCast<S1ItemFramework.IntegerItemDefinition>()", create);
        int spawnStart = source.IndexOf("public static S1ItemFramework.IntegerItemInstance? Spawn", StringComparison.Ordinal);
        string spawn = source[spawnStart..source.IndexOf("// --- helpers ---", spawnStart, StringComparison.Ordinal)];
        Assert.Contains("if (!EnsureRegistered())", spawn);
    }

    [Fact]
    public void ExpiryPollerUsesTheSameSampleAndDeadlinePolicyAsRecognitionAndPayout()
    {
        string source = Source("BountyContracts.cs");
        string expiry = source[..source.IndexOf("public static class BountyHeatService", StringComparison.Ordinal)];
        Assert.Contains("TryGetSnapshot(", expiry);
        Assert.Contains("BountyDeadlinePolicy.IsEligible(c, now)", expiry);
        Assert.DoesNotContain("HitmanPhoneTime.MinutesUntil(", expiry);
    }

    [Fact]
    public void NativeConsumersUseTheTestedPresenceAndSettlementSeams()
    {
        string evidence = Source("BountyEvidence.cs");
        Assert.Contains("BountyRewardSettlement.TryPay(contract,", evidence);
        Assert.Contains("BountyEvidenceMatcher.ObserveForContract(save, contract, value, legacy)", evidence);
        Assert.Contains("BountyDeadlinePolicy.IsEligible(match, liveNow)", evidence);
        string flow = Source("BountyQuestFlow.cs");
        Assert.Contains("presence == EvidencePresence.Unknown", flow);
        Assert.Contains("_evidenceReconciliationPending = unknown > 0", flow);
        string contracts = Source("BountyContracts.cs");
        int expiry = contracts.IndexOf("c.Status != EBountyStatus.Active || c.RewardPaymentUncertain", StringComparison.Ordinal);
        int death = contracts.IndexOf("if (c.RewardPaymentUncertain) continue", StringComparison.Ordinal);
        Assert.True(expiry >= 0 && death > expiry);
    }

    [Fact]
    public void PlayerDeathPollingUsesTimeAndInitialLoadedBaseline()
    {
        string source = Source("BountyContracts.cs").Split("public static class PlayerDeathWatchdog", StringSplitOptions.None)[1];
        Assert.DoesNotContain("_aliveFrameCounter", source);
        Assert.Contains("Environment.TickCount64", source);
        Assert.Contains("_wasAlive = null", source);
    }
}
