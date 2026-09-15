using S1Mods.Shared;
using Xunit;

namespace Shared.Tests;

/// <summary>
/// Tests for S1Mods.Shared.SaveSlots: pure path-token derivation (legacy saves
/// without slot number). The LoadManager probe itself needs the IL2CPP runtime
/// and stays out of scope here — its statement sequence is 1:1 the proven
/// per-mod code it replaced (Konsolidierung 2026-09-15).
/// </summary>
public class SaveSlotsTests
{
    [Theory]
    [InlineData("C:\\Saves\\Slot_5\\Game.json", "5")]          // digits in folder name
    [InlineData("C:\\Saves\\Save 12.json", "12")]              // digits in file name
    [InlineData("C:\\Games\\ScheduleI\\Saves\\7\\Game.json", "7")]
    [InlineData("/home/user/saves/quicksave.json", "quicksave")] // no digits anywhere -> file name
    [InlineData("C:\\Saves\\Slot3\\My Save 9.json", "9")]      // file name wins over folder
    public void TryExtractSlotTokenFromSavePath_DerivesExpectedToken(string path, string expected)
    {
        Assert.Equal(expected, SaveSlots.TryExtractSlotTokenFromSavePath(path));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void TryExtractSlotTokenFromSavePath_NullOrEmpty_ReturnsNull(string? path)
    {
        Assert.Null(SaveSlots.TryExtractSlotTokenFromSavePath(path));
    }

    [Fact]
    public void TryExtractSlotTokenFromSavePath_ExtensionIsStripped()
    {
        // The "no digits" fallback uses the file name WITHOUT extension —
        // mods prepend their own prefix (slot_{token}), so "Game.json" must
        // never leak the ".json" part into a state-file name.
        Assert.Equal("Game", SaveSlots.TryExtractSlotTokenFromSavePath("C:\\Saves\\Game.json"));
    }

    [Fact]
    public void TryExtractSlotTokenFromSavePath_RootOnlyPath_PrefersDigitsInFileName()
    {
        // Path.GetDirectoryName of a root-only file is empty; the filename probe
        // still runs first, so its digits remain the token.
        Assert.Equal("2", SaveSlots.TryExtractSlotTokenFromSavePath("save2.json"));
    }
}
