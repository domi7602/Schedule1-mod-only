using LightControl.Core;
using S1Mods.Shared;

namespace LightControl.Core.Tests;

internal static class DiskAdapterChecks
{
    public static IEnumerable<(string Name, Action Run)> Cases => new (string Name, Action Run)[]
    {
        ("Disk01 First-use missing mod directory on reachable storage is allowed", FirstUse),
        ("Disk02 Unreachable drive is not confirmed missing data", UnreachableDrive),
        ("Disk03 Directory in place of a file blocks and never overwrites", DirectoryIsNotFile),
        ("Disk04 Validated backup recovery preserves the good backup on save", RecoverAndSave),
        ("Disk05 Foreign identity and future schema never recover over main", IntactConflicts),
        ("Disk06 Locked main stays blocked after access returns", AccessReturns),
        ("Disk07 Slot drift and thrown slot probe never write", SlotErrors),
        ("Disk08 Ordinary saves execute the real Shared writer and rotate valid backup", OrdinarySave),
        ("Disk09 Absent main uses fully validated backup", MissingMainBackup),
        ("Disk10 Repeated load cannot discard recovered backup protection", RepeatedRecoveryLoad),
        ("Disk11 Foreign identity remains a conflict with unexpected root fields", ForeignIdentityWithExtraField)
    }.Where(test => OperatingSystem.IsWindows() || !test.Name.StartsWith("Disk02 ", StringComparison.Ordinal));

    private sealed class Fixture : IDisposable
    {
        public string Root { get; } = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "hermes", "cache", "scratch", "lightcontrol-disk-tests", Guid.NewGuid().ToString("N"));
        public string MainPath => Path.Combine(Root, "LightControl", "lamps_slot-0.json");
        public Fixture()
        {
            Directory.CreateDirectory(Root);
            MelonLoader.Utils.MelonEnvironment.UserDataDirectory = Root;
            SaveSlots.ActiveSlot = 0;
            SaveSlots.ThrowOnProbe = false;
        }
        public LightPersistence Adapter() => new(0, "slot-0", new ModLogger("LightControl.Tests"));
        public void Write(string text, bool backup = false)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(MainPath)!);
            File.WriteAllText(MainPath + (backup ? ".bak" : ""), text);
        }
        public void Dispose()
        {
            SaveSlots.ThrowOnProbe = false;
            Directory.Delete(Root, true);
            MelonLoader.Utils.MelonEnvironment.UserDataDirectory = string.Empty;
        }
    }
    private static JsonStatePersistence Empty(string saveId = "slot-0")
    {
        var store = new LightStateStore();
        store.BeginSession(Guid.NewGuid(), saveId, SessionMode.Host);
        return new(store);
    }
    private static string ValidJson(string saveId = "slot-0")
    {
        var store = new LightStateStore();
        store.BeginSession(Guid.NewGuid(), saveId, SessionMode.Host);
        var persistence = new JsonStatePersistence(store);
        persistence.ConfirmDataMissing();
        var identity = LampIdentity.Persistent(new(PersistentIdentityKind.VerifiedNativeInstance, "disk-test", 1, "fixture"));
        store.Register(new(identity, "fixture-runtime", "A"), 0);
        store.ExecuteHost(new(LightLimits.ProtocolVersion, store.SessionId, Guid.NewGuid(), store.Revision,
            ControlCommand.Set(LampSelection.Lamp(identity), new(ColorMode.Purple, IntensityLevel.Percent150))), "fixture-host", _ => true, 1);
        return persistence.CreateJsonForSave();
    }
    private static void Require(bool value, string message)
    {
        if (!value) throw new InvalidOperationException(message);
    }
    private static void FirstUse()
    {
        using var fixture = new Fixture();
        var state = Empty();
        Require(fixture.Adapter().Load(state) && state.Gate == PersistenceGate.Ready, "Genuine first use did not initialize.");
    }
    private static void UnreachableDrive()
    {
        using var fixture = new Fixture();
        var drives = DriveInfo.GetDrives().Select(drive => drive.Name.Substring(0, 1)).ToHashSet(StringComparer.OrdinalIgnoreCase);
        string? unused = Enumerable.Range('D', 'Z' - 'D' + 1).Select(x => ((char)x).ToString()).FirstOrDefault(x => !drives.Contains(x));
        if (unused is null) throw new InvalidOperationException("No unused drive is available for the read-only missing-root test.");
        // No directory is created or deleted on this drive. Only the approved scratch fixture is written.
        MelonLoader.Utils.MelonEnvironment.UserDataDirectory = unused + ":/LightControl-unreachable-" + Guid.NewGuid().ToString("N");
        var state = Empty();
        Require(!fixture.Adapter().Load(state) && state.Gate == PersistenceGate.Blocked, "Unavailable drive incorrectly opened the write gate.");
    }
    private static void DirectoryIsNotFile()
    {
        using var fixture = new Fixture();
        Directory.CreateDirectory(fixture.MainPath);
        var state = Empty();
        var adapter = fixture.Adapter();
        Require(!adapter.Load(state) && state.Gate == PersistenceGate.Blocked, "Directory-as-file was treated as absence.");
        Require(!adapter.Save(state) && Directory.Exists(fixture.MainPath), "Blocked data was overwritten.");
    }
    private static void RecoverAndSave()
    {
        using var fixture = new Fixture();
        string backup = ValidJson();
        fixture.Write("{broken");
        fixture.Write(backup, true);
        var adapter = fixture.Adapter();
        var state = Empty();
        Require(adapter.Load(state) && adapter.LoadedFromBackup, "Valid backup was not recovered.");
        Require(File.ReadAllText(fixture.MainPath) == "{broken", "Load rewrote main.");
        Require(adapter.Save(state), "Recovered save failed.");
        Require(File.ReadAllText(fixture.MainPath + ".bak") == backup, "Good backup was replaced by corrupt main.");
        Require(JsonStatePersistence.Inspect(File.ReadAllText(fixture.MainPath), "slot-0").Status == PersistenceDataStatus.Valid, "Saved main failed validation.");
    }
    private static void IntactConflicts()
    {
        using var fixture = new Fixture();
        foreach (string main in new[] { ValidJson("slot-1"), ValidJson().Replace("\"schemaVersion\": 1", "\"schemaVersion\": 999") })
        {
            fixture.Write(main);
            fixture.Write(ValidJson(), true);
            var state = Empty();
            var adapter = fixture.Adapter();
            Require(!adapter.Load(state) && state.Gate == PersistenceGate.Blocked, "Intact identity/schema conflict used backup.");
            Require(!adapter.Save(state) && File.ReadAllText(fixture.MainPath) == main, "Conflict main was overwritten.");
        }
    }
    private static void AccessReturns()
    {
        using var fixture = new Fixture();
        string json = ValidJson();
        fixture.Write(json);
        var state = Empty();
        var adapter = fixture.Adapter();
        using (var locked = new FileStream(fixture.MainPath, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
            Require(!adapter.Load(state) && state.Gate == PersistenceGate.Blocked, "Locked main was not blocked.");
        Require(!adapter.Save(state) && File.ReadAllText(fixture.MainPath) == json, "Returning access silently opened gate.");
    }
    private static void OrdinarySave()
    {
        using var fixture = new Fixture();
        string original = ValidJson();
        fixture.Write(original);
        var adapter = fixture.Adapter();
        var state = Empty();
        Require(adapter.Load(state) && !adapter.LoadedFromBackup, "Ordinary primary load failed.");
        Require(adapter.Save(state), "Normal Shared writer path failed.");
        Require(File.ReadAllText(fixture.MainPath + ".bak") == original, "Normal save did not rotate valid primary bytes.");
        Require(JsonStatePersistence.Inspect(File.ReadAllText(fixture.MainPath), "slot-0").Status == PersistenceDataStatus.Valid, "Ordinary save wrote invalid data.");
    }
    private static void MissingMainBackup()
    {
        using var fixture = new Fixture();
        fixture.Write(ValidJson(), true);
        var state = Empty();
        var adapter = fixture.Adapter();
        Require(adapter.Load(state) && adapter.LoadedFromBackup && !File.Exists(fixture.MainPath), "Absent primary did not load validated backup read-only.");
    }
    private static void RepeatedRecoveryLoad()
    {
        using var fixture = new Fixture();
        string backup = ValidJson();
        fixture.Write("{broken");
        fixture.Write(backup, true);
        var state = Empty();
        var adapter = fixture.Adapter();
        Require(adapter.Load(state), "Initial recovery failed.");
        adapter.Load(state);
        Require(adapter.Save(state) && File.ReadAllText(fixture.MainPath + ".bak") == backup, "Repeated load lost recovery protection and rotated corruption over good backup.");
    }
    private static void ForeignIdentityWithExtraField()
    {
        using var fixture = new Fixture();
        string foreign = ValidJson("slot-1").Replace("\"lamps\":", "\"unexpected\": 1, \"lamps\":");
        fixture.Write(foreign);
        fixture.Write(ValidJson(), true);
        var state = Empty();
        var adapter = fixture.Adapter();
        Require(!adapter.Load(state) && state.Gate == PersistenceGate.Blocked && !adapter.Save(state), "Root-shape error hid a foreign save-ID conflict.");
        Require(File.ReadAllText(fixture.MainPath) == foreign, "Foreign primary was overwritten.");
    }

    private static void SlotErrors()
    {
        using var fixture = new Fixture();
        var adapter = fixture.Adapter();
        var state = Empty();
        Require(adapter.Load(state), "Fixture initial load failed.");
        SaveSlots.ActiveSlot = -1;
        Require(!adapter.Save(state), "Inactive slot wrote data.");
        SaveSlots.ActiveSlot = 1;
        Require(!adapter.Save(state), "Other slot wrote data.");
        SaveSlots.ThrowOnProbe = true;
        Require(!adapter.Save(state), "Throwing slot probe wrote data.");
        Require(!File.Exists(fixture.MainPath), "Rejected writes created main.");
    }
}
