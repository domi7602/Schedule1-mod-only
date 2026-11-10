using System.Security.Cryptography;
using System.Text.Json;
using TownHallInterior;
using Xunit;

namespace TownHallInterior.Tests;

public sealed class BusinessPersistenceTests : IDisposable
{
    private const string PlayerFolder = "Player_0";
    private const string Organisation = "TownHallTest";
    private static readonly TownHallNativeValues InitialWallet = new(500_000f, 300_000f, 1200, 0);
    private readonly string _fixture = Path.Combine(Path.GetTempPath(), "TownHallPersistenceTests_" + Guid.NewGuid().ToString("N"));

    private string Sidecars => Path.Combine(_fixture, "sidecars");
    private string CheckpointRoot => Path.Combine(Sidecars, "Checkpoints");

    [Fact]
    public void FirstOpenRecordsUnownedBaselineForThisNativeSave()
    {
        var context = CreateNativeSlot();
        var session = Open(context);

        Assert.Equal(ReadNative(context).Fingerprint, session.Fingerprint);
        Assert.Equal(InitialWallet, ReadNative(context).Values);
        Assert.Equal(TownHallBusinessData.CurrentSchemaVersion, session.Data.SchemaVersion);
        Assert.False(session.Data.IsOwned);
        Assert.Empty(session.Data.ActiveOperations);
        Assert.True(File.Exists(CheckpointFile(session)));
        Assert.False(Open(context).Data.IsOwned);
    }

    [Fact]
    public void UnsupportedWorldCannotCreateAnEmptyRathausState()
    {
        var context = CreateNativeSlot() with { OrganisationName = "Dominik" };

        Assert.Throws<InvalidDataException>(() => Open(context));
        Assert.False(Directory.Exists(CheckpointRoot));
    }

    [Fact]
    public void UnsavedPurchaseReloadsWithoutOwnershipOrDebit()
    {
        var context = CreateNativeSlot();
        var session = Open(context);
        var wallet = new Wallet(InitialWallet);

        var purchase = Buy(session, context, wallet, PurchasePaymentMode.Bank);
        Assert.True(purchase.IsOwned);
        Assert.Equal(250_000f, wallet.Bank);

        var reopened = Open(context);
        Assert.Equal(session.Fingerprint, reopened.Fingerprint);
        Assert.False(reopened.Data.IsOwned);
        Assert.Equal(InitialWallet, ReadNative(context).Values);
    }

    [Theory]
    [InlineData(PurchasePaymentMode.Bank, 250_000f, 300_000f)]
    [InlineData(PurchasePaymentMode.Cash, 500_000f, 50_000f)]
    public void NativeSaveReloadsPurchaseWithExactlyTheChargedBalance(
        PurchasePaymentMode mode, float expectedBank, float expectedCash)
    {
        var context = CreateNativeSlot();
        var session = Open(context);
        var wallet = new Wallet(InitialWallet);
        Buy(session, context, wallet, mode);
        var values = new TownHallNativeValues(expectedBank, expectedCash, 1201, 0);
        Assert.Equal(values, wallet.ValuesAt(1201, 0));

        Commit(session, context, values, revision: 1);

        var reopened = Open(context);
        Assert.Equal(values, ReadNative(context).Values);
        Assert.True(reopened.Data.IsOwned);
        Assert.Equal(250_000f, reopened.Data.PurchasePrice);
        Assert.Equal(mode.ToString(), reopened.Data.PaymentMode);
        Assert.Equal(session.Data.PurchaseTimestampUtc, reopened.Data.PurchaseTimestampUtc);
        var purchase = new BusinessPurchaseManager(initiallyOwned: reopened.Data.IsOwned);
        var secondPurchase = purchase.TryPurchase(context, wallet, new OwnedMarker(), new WorldRegistrationIdentity(), mode);
        Assert.Equal("already-owned", secondPurchase.StatusMessage);
        Assert.Equal(values, wallet.ValuesAt(1201, 0));
    }

    [Fact]
    public void DepositProgressAndPayoutFollowTheNativeSaveBeingLoaded()
    {
        var context = CreateNativeSlot();
        var session = Open(context);
        var wallet = new Wallet(InitialWallet);
        var purchase = Buy(session, context, wallet, PurchasePaymentMode.Bank);
        var laundering = new BusinessLaunderingManager(purchase);
        var deposit = laundering.TryStartLaundering(context, wallet, 2_000f);
        Assert.True(deposit.Success);
        laundering.AdvanceMinutes(180);
        laundering.SyncStatsActiveOperations(session.Data);
        var depositedWallet = new TownHallNativeValues(250_000f, 298_000f, 1500, 0);
        Assert.Equal(depositedWallet, wallet.ValuesAt(1500, 0));
        Commit(session, context, depositedWallet, revision: 1);

        var pending = Open(context);
        var job = Assert.Single(pending.Data.ActiveOperations);
        Assert.Equal(deposit.OperationId, job.OperationId);
        Assert.Equal(1_260, job.RemainingMinutes);
        Assert.Equal(180, job.GetMinutesSinceStarted());
        var pendingWallet = new Wallet(ReadNative(context).Values);
        var restored = RestoreLaundering(pending, pendingWallet);
        Assert.False(restored.TrySettleDue(pendingWallet, pending.Data, out _));
        Assert.Equal(depositedWallet, pendingWallet.ValuesAt(1500, 0));

        restored.AdvanceMinutes(job.RemainingMinutes);
        Assert.True(restored.TrySettleDue(pendingWallet, pending.Data, out float paid));
        Assert.Equal(2_000f, paid);
        Assert.False(restored.TrySettleDue(pendingWallet, pending.Data, out _));
        Assert.Equal(1, pendingWallet.PayoutCalls);

        // A payout that hasn't been saved restores both the pre-payout bank and the job.
        Assert.Equal(depositedWallet, ReadNative(context).Values);
        Assert.Equal(deposit.OperationId, Assert.Single(Open(context).Data.ActiveOperations).OperationId);

        var paidWallet = new TownHallNativeValues(252_000f, 298_000f, 1200, 1);
        Assert.Equal(paidWallet, pendingWallet.ValuesAt(1200, 1));
        Commit(pending, context, paidWallet, revision: 2);
        var settled = Open(context);
        Assert.Equal(paidWallet, ReadNative(context).Values);
        Assert.Empty(settled.Data.ActiveOperations);
        Assert.Equal(2_000f, settled.Data.TotalLaundered);
        Assert.Equal(1, settled.Data.CompletedLaunderingOperations);
        var settledWallet = new Wallet(ReadNative(context).Values);
        Assert.False(RestoreLaundering(settled, settledWallet).TrySettleDue(settledWallet, settled.Data, out _));
        Assert.Equal(0, settledWallet.PayoutCalls);

        WriteNative(context, depositedWallet, revision: 1);
        var rolledBack = Open(context);
        Assert.Equal(depositedWallet, ReadNative(context).Values);
        Assert.Equal(deposit.OperationId, Assert.Single(rolledBack.Data.ActiveOperations).OperationId);
        Assert.Equal(0f, rolledBack.Data.TotalLaundered);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void RejectedNativeSaveRetainsPendingAndNeverPublishesPurchase(bool partiallyWritten)
    {
        var context = CreateNativeSlot();
        var session = Open(context);
        var wallet = new Wallet(InitialWallet);
        Buy(session, context, wallet, PurchasePaymentMode.Bank);
        session.Prepare(wallet.ValuesAt(1201, 0));
        var pendingPath = Assert.Single(Directory.GetFiles(CheckpointRoot, "pending-*.json", SearchOption.AllDirectories));
        string frozen = File.ReadAllText(pendingPath);
        if (partiallyWritten)
            WriteJson(context.ActiveSavePath!, "Money.json", new { OnlineBalance = wallet.Bank });

        Assert.Throws<InvalidDataException>(() => session.Complete(nativeSaveSucceeded: false));

        Assert.Equal(frozen, File.ReadAllText(pendingPath));
        if (partiallyWritten)
            Assert.Throws<InvalidDataException>(() => Open(context));
        else
            Assert.False(Open(context).Data.IsOwned);

        WriteNative(context, InitialWallet, revision: 0);
        Assert.False(Open(context).Data.IsOwned);
    }

    [Theory]
    [InlineData(PurchasePaymentMode.Bank)]
    [InlineData(PurchasePaymentMode.Cash)]
    public void SuccessfulSignalWithMissingDebitCannotCommitPurchase(PurchasePaymentMode mode)
    {
        var context = CreateNativeSlot();
        var session = Open(context);
        string baselinePath = CheckpointFile(session);
        string baseline = File.ReadAllText(baselinePath);
        var wallet = new Wallet(InitialWallet);
        Buy(session, context, wallet, mode);
        session.Prepare(wallet.ValuesAt(1201, 0));

        WriteNative(context, InitialWallet with { TimeOfDay = 1201 }, revision: 1);

        Assert.Throws<InvalidDataException>(() => session.Complete(nativeSaveSucceeded: true));
        Assert.Throws<InvalidDataException>(() => Open(context));
        Assert.Equal(baseline, File.ReadAllText(baselinePath));
        Assert.Single(Directory.GetFiles(CheckpointRoot, "pending-*.json", SearchOption.AllDirectories));
    }

    [Theory]
    [InlineData("corrupt")]
    [InlineData("fingerprint")]
    [InlineData("newer-schema")]
    public void InvalidCheckpointClosesLoadAndPreservesItsBytes(string damage)
    {
        var context = CreateNativeSlot();
        var session = Open(context);
        string path = CheckpointFile(session);
        string invalid = damage == "corrupt" ? "{" : JsonSerializer.Serialize(new
        {
            Fingerprint = damage == "fingerprint" ? new string('0', 64) : session.Fingerprint,
            Data = new TownHallBusinessData
            {
                SchemaVersion = damage == "newer-schema" ? TownHallBusinessData.CurrentSchemaVersion + 1 : TownHallBusinessData.CurrentSchemaVersion
            }
        });
        File.WriteAllText(path, invalid);

        if (damage == "corrupt")
            Assert.ThrowsAny<JsonException>(() => Open(context));
        else
            Assert.Throws<InvalidDataException>(() => Open(context));

        Assert.Equal(invalid, File.ReadAllText(path));
        Assert.Single(Directory.GetFiles(CheckpointRoot, "*.json", SearchOption.AllDirectories));
    }

    [Fact]
    public void IdenticalNativeContentsInDifferentFullSlotPathsAreIsolated()
    {
        var first = CreateNativeSlot("profile-one");
        var second = CreateNativeSlot("profile-two");
        var session = Open(first);
        var wallet = new Wallet(InitialWallet);
        Buy(session, first, wallet, PurchasePaymentMode.Cash);
        var sameNative = wallet.ValuesAt(1201, 0);
        Commit(session, first, sameNative, revision: 1);
        WriteNative(second, sameNative, revision: 1);

        var other = Open(second);

        Assert.Equal(session.Fingerprint, other.Fingerprint);
        Assert.True(Open(first).Data.IsOwned);
        Assert.False(other.Data.IsOwned);
    }

    [Fact]
    public void UncertainPaymentBlocksOnlyTheNativeSaveThatContainsIt()
    {
        var context = CreateNativeSlot();
        var session = Open(context);
        var hold = new UnresolvedTownHallPayment("purchase-attempt", 250_000f, "payment-unresolved");
        session.Data.UnresolvedPayment = hold;

        Assert.Null(Open(context).Data.UnresolvedPayment);

        var afterDebit = InitialWallet with { Bank = InitialWallet.Bank - hold.Amount };
        Commit(session, context, afterDebit, revision: 1);
        Assert.Equal(hold, Open(context).Data.UnresolvedPayment);
        Assert.False(Open(context).Data.IsOwned);

        WriteNative(context, InitialWallet, revision: 0);
        Assert.Null(Open(context).Data.UnresolvedPayment);
    }

    [Fact]
    public void PaidLegacyStateNeedsBothExactHashesAndHoldsAlreadyDueJob()
    {
        var context = CreateNativeSlot();
        string legacyFolder = Path.Combine(Sidecars, "TownHallTest_SaveGame_2");
        Directory.CreateDirectory(legacyFolder);
        string legacy = """
            {
              "schemaVersion": 1,
              "isOwned": true,
              "purchasePrice": 250000,
              "purchaseTimestampUtc": "2026-11-09T06:00:00Z",
              "paymentMode": "Bank",
              "totalLaundered": 2500,
              "completedLaunderingOperations": 2,
              "activeOperations": [
                {"operationId":"partial","amount":1500,"remainingSeconds":30,"totalDurationSeconds":60},
                {"operationId":"due","amount":500,"remainingSeconds":0,"totalDurationSeconds":60}
              ]
            }
            """;
        string primary = Path.Combine(legacyFolder, "townhall_business.json");
        string backup = primary + ".bak";
        File.WriteAllText(primary, legacy);
        File.WriteAllText(backup, legacy + Environment.NewLine);
        string fingerprint = ReadNative(context).Fingerprint;
        string legacyHash = Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(primary)));
        string wrongHash = new string('0', 64);

        Assert.Throws<InvalidDataException>(() => Open(context));
        WriteJson(legacyFolder, "legacy-adoption.json", new { NativeFingerprint = wrongHash, LegacySha256 = legacyHash });
        Assert.Throws<InvalidDataException>(() => Open(context));
        WriteJson(legacyFolder, "legacy-adoption.json", new { NativeFingerprint = fingerprint, LegacySha256 = wrongHash });
        Assert.Throws<InvalidDataException>(() => Open(context));
        Assert.False(Directory.Exists(CheckpointRoot));

        WriteJson(legacyFolder, "legacy-adoption.json", new { NativeFingerprint = fingerprint, LegacySha256 = legacyHash });
        var adopted = Open(context);

        Assert.Equal(TownHallBusinessData.CurrentSchemaVersion, adopted.Data.SchemaVersion);
        Assert.True(adopted.Data.IsOwned);
        Assert.Equal(2_500f, adopted.Data.TotalLaundered);
        Assert.Equal(2, adopted.Data.CompletedLaunderingOperations);
        Assert.Collection(adopted.Data.ActiveOperations,
            partial =>
            {
                Assert.Equal("partial", partial.OperationId);
                Assert.Equal(720, partial.RemainingMinutes);
                Assert.Equal(1_440, partial.TotalDurationMinutes);
                Assert.False(partial.PayoutUnresolved);
            },
            due =>
            {
                Assert.Equal("due", due.OperationId);
                Assert.Equal(0, due.RemainingMinutes);
                Assert.True(due.PayoutUnresolved);
                Assert.Equal(1, due.PayoutAttempts);
            });
        var wallet = new Wallet(ReadNative(context).Values);
        Assert.False(RestoreLaundering(adopted, wallet).TrySettleDue(wallet, adopted.Data, out _));
        Assert.Equal(0, wallet.PayoutCalls);
        Assert.Equal(legacy, File.ReadAllText(primary));
        Assert.Equal(legacy + Environment.NewLine, File.ReadAllText(backup));
    }

    private ConstructionContext CreateNativeSlot(string profile = "profile")
    {
        string slot = Path.Combine(_fixture, profile, "SaveGame_2");
        Directory.CreateDirectory(Path.Combine(slot, "Players", PlayerFolder));
        var context = new ConstructionContext(true, true, true, true, slot, slot, Organisation);
        WriteNative(context, InitialWallet, revision: 0);
        return context;
    }

    private TownHallSaveSession Open(ConstructionContext context) =>
        TownHallSaveSession.Open(context, Sidecars, PlayerFolder);

    private string CheckpointFile(TownHallSaveSession session) =>
        Assert.Single(Directory.GetFiles(CheckpointRoot, session.Fingerprint + ".json", SearchOption.AllDirectories));

    private static TownHallNativeCheckpoint ReadNative(ConstructionContext context) =>
        TownHallNativeCheckpoint.Read(context.ActiveSavePath!, PlayerFolder, Organisation);

    private static void WriteNative(ConstructionContext context, TownHallNativeValues values, int revision)
    {
        string slot = context.ActiveSavePath!;
        WriteJson(slot, "Game.json", new { OrganisationName = Organisation });
        WriteJson(slot, "Metadata.json", new { CreationDate = new { Year = 2026 }, LastPlayedDate = new { revision } });
        WriteJson(slot, "Money.json", new { OnlineBalance = values.Bank });
        WriteJson(slot, "Time.json", new { values.TimeOfDay, values.ElapsedDays });
        string cash = JsonSerializer.Serialize(new { DataType = "CashData", ID = "cash", CashBalance = values.Cash });
        WriteJson(Path.Combine(slot, "Players", PlayerFolder), "Inventory.json", new { Items = new string?[] { null, cash } });
    }

    private static void WriteJson(string directory, string file, object value) =>
        File.WriteAllText(Path.Combine(directory, file), JsonSerializer.Serialize(value));

    private static void Commit(TownHallSaveSession session, ConstructionContext context, TownHallNativeValues values, int revision)
    {
        session.Prepare(values);
        WriteNative(context, values, revision);
        session.Complete(nativeSaveSucceeded: true);
    }

    private static BusinessPurchaseManager Buy(
        TownHallSaveSession session, ConstructionContext context, Wallet wallet, PurchasePaymentMode mode)
    {
        var purchase = new BusinessPurchaseManager(initiallyOwned: session.Data.IsOwned);
        var receipt = purchase.TryPurchase(context, wallet, new OwnedMarker(), new WorldRegistrationIdentity(), mode);
        Assert.True(receipt.Success, receipt.StatusMessage);
        session.Data.IsOwned = purchase.IsOwned;
        session.Data.PurchasePrice = receipt.Amount;
        session.Data.PurchaseTimestampUtc = receipt.TimestampUtc.ToString("o");
        session.Data.PaymentMode = receipt.ResolvedMode.ToString();
        return purchase;
    }

    private static BusinessLaunderingManager RestoreLaundering(TownHallSaveSession session, Wallet wallet)
    {
        var manager = new BusinessLaunderingManager(new BusinessPurchaseManager(initiallyOwned: session.Data.IsOwned));
        manager.RestoreActiveOperations(session.Data.ActiveOperations, wallet);
        return manager;
    }

    private sealed class OwnedMarker : IBusinessWorldPort
    {
        private bool _owned;

        public WorldRegistrationSnapshot CaptureSnapshot() => new(true, !_owned, _owned, true, !_owned, _owned, false);

        public bool RegisterInWorld(WorldRegistrationIdentity identity, bool isOwned)
        {
            _owned = isOwned;
            return true;
        }

        public bool DeregisterFromWorld(WorldRegistrationIdentity identity)
        {
            _owned = false;
            return true;
        }
    }

    private sealed class Wallet(TownHallNativeValues initial) : IPurchasePaymentPort, ILaunderingPort
    {
        private float _reserved;
        public float Bank { get; private set; } = initial.Bank;
        public float Cash { get; private set; } = initial.Cash;
        public int PayoutCalls { get; private set; }

        public TownHallNativeValues ValuesAt(int hhmm, int days) => new(Bank, Cash, hhmm, days);
        public float GetOnlineBalance() => Bank;
        public float GetCashBalance() => Cash;
        public float GetPlayerCash() => Cash;
        public float GetLaunderCapacity() => BusinessLaunderingManager.DefaultCapacity;
        public float GetCurrentLaunderTotal() => _reserved;

        public MoneyMutationOutcome DeductBank(float amount, string label, string note)
        {
            Bank -= amount;
            return MoneyMutationOutcome.Applied;
        }

        public MoneyMutationOutcome RefundBank(float amount, string label, string note)
        {
            Bank += amount;
            return MoneyMutationOutcome.Applied;
        }

        public MoneyMutationOutcome DeductCash(float amount)
        {
            Cash -= amount;
            return MoneyMutationOutcome.Applied;
        }

        public MoneyMutationOutcome RefundCash(float amount)
        {
            Cash += amount;
            return MoneyMutationOutcome.Applied;
        }

        public bool StartNativeLaundering(float amount)
        {
            _reserved += amount;
            return true;
        }

        public MoneyMutationOutcome PayoutCleanFunds(float amount, string label, string note)
        {
            PayoutCalls++;
            Bank += amount;
            _reserved -= amount;
            return MoneyMutationOutcome.Applied;
        }
    }

    public void Dispose()
    {
        string absolute = Path.GetFullPath(_fixture);
        string temp = Path.TrimEndingDirectorySeparator(Path.GetFullPath(Path.GetTempPath()));
        if (Path.GetDirectoryName(absolute) == temp && Directory.Exists(absolute))
            Directory.Delete(absolute, recursive: true);
    }
}
