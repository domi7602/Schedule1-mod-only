using System;
using System.IO;
using Xunit;

namespace HitmanPhone.Tests;

// ---------------------------------------------------------------------------
// HP-06 / HP-07 / R07 source contracts.
//
// The journal bridge and the phone app are Unity/S1API dependent and cannot be
// linked into this Unity-free suite, so their repairs are pinned as source
// contracts (the same technique the TaxiDriver suite uses for native-facing
// code). These assertions fail on the pre-repair sources: AdvanceToDeadDrop
// matches entry titles by literal string, CleanupOrphanBountyQuests fabricates
// success with Complete(), and ResolveQuest adopts the first unbound quest.
// ---------------------------------------------------------------------------

public class JournalTrackerSourceContractTests
{
    // ---- HP-06: structural objective advance, never a display-text key -----

    [Fact]
    public void AdvanceToDeadDrop_UsesStructuralIndicesNotDisplayText()
    {
        string bridge = ReadSource("BountyJournalBridge.cs");

        // The first objective carries the target name ("Eliminate <NPC> and
        // collect the polaroid"), so an exact literal match can never succeed.
        Assert.DoesNotContain("Eliminate the target and collect the polaroid", bridge);
        Assert.DoesNotContain("entry.Title ==", bridge);

        // Objectives are addressed structurally: index 0 = elimination, 1 = drop.
        Assert.Contains("QuestEntries[0]", bridge);
        Assert.Contains("QuestEntries[1]", bridge);
        Assert.Contains("ApplyJournalPhase", bridge);
    }

    // ---- HP-07: stable deterministic binding, no arbitrary adoption --------

    [Fact]
    public void ResolveQuest_DoesNotAdoptArbitraryUnboundQuest()
    {
        string bridge = ReadSource("BountyJournalBridge.cs");

        // The "adopt the first unbound BountyQuest" fallback is the wrong-binding bug.
        Assert.DoesNotContain("IsQuestBoundToAnyContract(candidate)", bridge);
        Assert.DoesNotContain("CollectUnboundActiveContracts", bridge);

        // Binding resolves through the persisted quest GUID (S1API 3.2.1
        // GetQuestByGuid matches S1Quest.StaticGUID, which QuestPatches restores).
        Assert.Contains("GetQuestByGuid(", bridge);
        Assert.Contains("ComputeContractQuestGuid(", bridge);
        Assert.Contains("StaticGUID", bridge);
    }

    [Fact]
    public void CreatedQuest_BindsDeterministicPersistentGuid()
    {
        string bridge = ReadSource("BountyJournalBridge.cs");

        // The GUID is derived deterministically from the contract id (no new save
        // field) and written to the quest so it survives save/load.
        Assert.Contains("TryBindPersistentGuid", bridge);
        Assert.Contains("new Guid(", bridge);
    }

    // ---- HP-07: orphan cleanup must stay truthful --------------------------

    [Fact]
    public void CleanupOrphanBountyQuests_NeverFabricatesSuccess()
    {
        string bridge = ReadSource("BountyJournalBridge.cs");
        string cleanup = MethodBody(bridge, "void CleanupOrphanBountyQuests(");

        // An unbound quest is not automatically an orphan, and a genuine orphan
        // must never be reported as a successful completion.
        Assert.DoesNotContain(".Complete()", cleanup);
        Assert.DoesNotContain("Complete();", cleanup);

        // Cleanup may only act on a quest it can map to a real contract, and it
        // must delegate the state change (no invented success in the sweep itself).
        Assert.Contains("FindContractByQuestGuid", cleanup);
        Assert.Contains("ApplyContractState", cleanup);
    }

    // ---- HP-07: full phase restore from the contract -----------------------

    [Fact]
    public void Rebind_RestoresPhaseFromContract()
    {
        string bridge = ReadSource("BountyJournalBridge.cs");

        Assert.Contains("AwaitingDrop", bridge);
        Assert.Contains("DropDueStarted", bridge);
        // The phase is reconstructed when a quest is (re)bound, not only on kill.
        string rebind = MethodBody(bridge, "bool RebindActiveQuests(");
        Assert.Contains("ApplyJournalPhase", rebind);
    }

    // ---- R07: render key committed only after a successful rebuild ---------

    [Fact]
    public void TrackerApp_CommitsRenderKeyOnlyAfterSuccessfulRebuild()
    {
        string app = ReadSource("BountyTrackerApp.cs");

        // Old bug: _renderedKey was adopted before the guarded rebuild ran.
        Assert.DoesNotContain("_renderedKey = BuildRenderKey();", app);
        Assert.Contains("TrackerRenderGate", app);

        string rebuild = MethodBody(app, "void Rebuild(");
        int tryIdx = rebuild.IndexOf("try", StringComparison.Ordinal);
        int commitIdx = rebuild.IndexOf(".Commit(", StringComparison.Ordinal);
        Assert.True(tryIdx >= 0, "Rebuild must guard its UI work with try/catch");
        Assert.True(commitIdx > tryIdx,
            "the render key must be committed only after the guarded rebuild succeeds");
    }

    [Fact]
    public void TrackerApp_DisplayUsesThreeWayEliminationState()
    {
        string app = ReadSource("BountyTrackerApp.cs");
        Assert.Contains("HasElimination", app);
    }

    // ------------------------------------------------------------------ helpers

    private static string ReadSource(string name)
    {
        for (DirectoryInfo? dir = new DirectoryInfo(AppContext.BaseDirectory); dir != null; dir = dir.Parent)
        {
            string root = Path.Combine(dir.FullName, "Source", "Mods", "HitmanPhone", "src");
            if (!Directory.Exists(root)) continue;

            string direct = Path.Combine(root, name);
            if (File.Exists(direct)) return File.ReadAllText(direct);

            string[] matches = Directory.GetFiles(root, name, SearchOption.AllDirectories);
            if (matches.Length == 1) return File.ReadAllText(matches[0]);
            if (matches.Length > 1)
                throw new IOException($"Ambiguous HitmanPhone source name '{name}': {string.Join(", ", matches)}");
            throw new FileNotFoundException("HitmanPhone source file not found", name);
        }
        throw new FileNotFoundException("HitmanPhone source root not found", name);
    }

    /// <summary>
    /// Returns the brace-delimited body that follows <paramref name="signature"/>,
    /// skipping braces inside string/char literals and comments so interpolated
    /// strings in the bridge do not unbalance the scan.
    /// </summary>
    private static string MethodBody(string source, string signature)
    {
        int sig = source.IndexOf(signature, StringComparison.Ordinal);
        Assert.True(sig >= 0, $"signature not found: {signature}");
        int open = source.IndexOf('{', sig);
        Assert.True(open >= 0, $"no body after: {signature}");

        int depth = 0;
        bool inStr = false, inChar = false, inLine = false, inBlock = false;
        for (int i = open; i < source.Length; i++)
        {
            char c = source[i];
            char n = i + 1 < source.Length ? source[i + 1] : '\0';
            if (inLine) { if (c == '\n') inLine = false; continue; }
            if (inBlock) { if (c == '*' && n == '/') { inBlock = false; i++; } continue; }
            if (inStr) { if (c == '\\') { i++; continue; } if (c == '"') inStr = false; continue; }
            if (inChar) { if (c == '\\') { i++; continue; } if (c == '\'') inChar = false; continue; }
            if (c == '/' && n == '/') { inLine = true; i++; continue; }
            if (c == '/' && n == '*') { inBlock = true; i++; continue; }
            if (c == '"') { inStr = true; continue; }
            if (c == '\'') { inChar = true; continue; }
            if (c == '{') depth++;
            else if (c == '}')
            {
                depth--;
                if (depth == 0) return source.Substring(open, i - open + 1);
            }
        }
        throw new InvalidOperationException("unbalanced body after " + signature);
    }
}
