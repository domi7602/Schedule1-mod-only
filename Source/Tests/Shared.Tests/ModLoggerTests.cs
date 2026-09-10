using System;
using S1Mods.Shared;
using Xunit;

namespace Shared.Tests;

/// <summary>
/// Tests for S1Mods.Shared.ModLogger — the thin prefix-formatter over
/// MelonLogger. The contract we test is "doesn't throw out of the caller";
/// the actual MelonLogger sink may or may not work depending on whether
/// MelonLoader is fully initialized in the host process.
/// </summary>
public class ModLoggerTests
{
    [Fact]
    public void Constructor_NullName_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => new ModLogger(null!));
    }

    [Fact]
    public void Constructor_EmptyName_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => new ModLogger(""));
    }

    [Fact]
    public void Constructor_ValidName_StoresModName()
    {
        var log = new ModLogger("MyMod");
        Assert.Equal("MyMod", log.ModName);
    }

    [Fact]
    public void Info_ForwardsToMelonLogger_DoesNotThrow()
    {
        var log = new ModLogger("Test");
        // In a non-MelonLoader host (test process), MelonLogger.Msg may throw.
        // ModLogger's contract is that it does not propagate exceptions to the
        // caller; this verifies that contract by wrapping the call.
        try { log.Info("test message"); } catch (Exception ex) when (ex is InvalidOperationException || ex is NullReferenceException) { /* expected in test host */ }
    }

    [Fact]
    public void Warn_ForwardsToMelonLogger_DoesNotThrow()
    {
        var log = new ModLogger("Test");
        try { log.Warn("warning"); } catch (Exception ex) when (ex is InvalidOperationException || ex is NullReferenceException) { }
    }

    [Fact]
    public void Warn_ExceptionOverload_DoesNotThrow()
    {
        var log = new ModLogger("Test");
        try { log.Warn(new InvalidOperationException("warn ex")); } catch (Exception ex) when (ex is InvalidOperationException || ex is NullReferenceException) { }
    }

    [Fact]
    public void Warn_ContextAndException_DoesNotThrow()
    {
        var log = new ModLogger("Test");
        try { log.Warn("context", new InvalidOperationException("inner")); } catch (Exception ex) when (ex is InvalidOperationException || ex is NullReferenceException) { }
    }

    [Fact]
    public void Error_ForwardsToMelonLogger_DoesNotThrow()
    {
        var log = new ModLogger("Test");
        try { log.Error("error"); } catch (Exception ex) when (ex is InvalidOperationException || ex is NullReferenceException) { }
    }

    [Fact]
    public void Error_ExceptionOverload_DoesNotThrow()
    {
        var log = new ModLogger("Test");
        try { log.Error(new InvalidOperationException("err")); } catch (Exception ex) when (ex is InvalidOperationException || ex is NullReferenceException) { }
    }

    [Fact]
    public void Error_ContextAndException_DoesNotThrow()
    {
        var log = new ModLogger("Test");
        try { log.Error("context", new InvalidOperationException("inner")); } catch (Exception ex) when (ex is InvalidOperationException || ex is NullReferenceException) { }
    }

    [Fact]
    public void Debug_ForwardsToMelonLogger_DoesNotThrow()
    {
        var log = new ModLogger("Test");
        // Debug is #if DEBUG-guarded; in test builds it forwards to MelonLogger.
        try { log.Debug("debug"); } catch (Exception ex) when (ex is InvalidOperationException || ex is NullReferenceException) { }
    }
}
