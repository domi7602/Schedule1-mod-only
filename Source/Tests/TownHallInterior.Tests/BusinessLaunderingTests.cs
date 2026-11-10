using System;
using System.IO;
using TownHallInterior;
using Xunit;

namespace TownHallInterior.Tests;

public sealed class BusinessLaunderingTests
{
    private sealed class MockLaunderingPort : ILaunderingPort
    {
        public float LaunderCapacity { get; set; } = 20_000f;
        public float CurrentLaunderTotal { get; set; } = 0f;
        public float PlayerCash { get; set; } = 15_000f;
        public float PaidToBank { get; private set; }

        public int DeductCashCalls { get; private set; }
        public int RefundCashCalls { get; private set; }
        public int NativeStartCalls { get; private set; }
        public int PayoutCalls { get; private set; }

        public bool LoseDeductionResult { get; set; }
        public bool FailRefundCash { get; set; }
        public bool FailNativeStart { get; set; }
        public bool ThrowNativeStart { get; set; }
        public bool ThrowAfterPayout { get; set; }
        public Func<float, bool>? RejectPayout { get; set; }

        public float GetLaunderCapacity() => LaunderCapacity;
        public float GetCurrentLaunderTotal() => CurrentLaunderTotal;
        public float GetPlayerCash() => PlayerCash;

        public MoneyMutationOutcome DeductCash(float amount)
        {
            DeductCashCalls++;
            if (PlayerCash < amount) return MoneyMutationOutcome.NotApplied;
            PlayerCash -= amount;
            return LoseDeductionResult ? MoneyMutationOutcome.Unknown : MoneyMutationOutcome.Applied;
        }

        public MoneyMutationOutcome RefundCash(float amount)
        {
            RefundCashCalls++;
            if (FailRefundCash) return MoneyMutationOutcome.NotApplied;
            PlayerCash += amount;
            return MoneyMutationOutcome.Applied;
        }

        public bool StartNativeLaundering(float amount)
        {
            NativeStartCalls++;
            if (ThrowNativeStart) throw new InvalidOperationException("simulated-native-laundering-crash");
            if (FailNativeStart) return false;
            CurrentLaunderTotal += amount;
            return true;
        }

        public MoneyMutationOutcome PayoutCleanFunds(float amount, string label, string note)
        {
            PayoutCalls++;
            if (RejectPayout?.Invoke(amount) == true) return MoneyMutationOutcome.NotApplied;
            PaidToBank += amount;
            if (ThrowAfterPayout) throw new InvalidOperationException("simulated balance callback failure");
            CurrentLaunderTotal = MathF.Max(0f, CurrentLaunderTotal - amount);
            return MoneyMutationOutcome.Applied;
        }
    }

    private static ConstructionContext CreateValidContext(string org = "TownHallTest", string slot = "SaveGame_2")
    {
        string dir = Path.Combine(Path.GetTempPath(), "TownHallLaunderingTests", slot);
        Directory.CreateDirectory(dir);
        return new ConstructionContext(
            LoadComplete: true,
            Singleplayer: true,
            HallReady: true,
            DependenciesMatch: true,
            ActiveSavePath: dir,
            LoadedSavePath: dir,
            OrganisationName: org);
    }

    [Fact]
    public void AccessDeniedWhenUnowned()
    {
        var purchaseManager = new BusinessPurchaseManager(initiallyOwned: false);
        var launderingManager = new BusinessLaunderingManager(purchaseManager);
        var port = new MockLaunderingPort();
        var context = CreateValidContext();

        var receipt = launderingManager.TryStartLaundering(context, port, 5_000f);

        Assert.False(receipt.Success);
        Assert.Equal("locked:unowned", receipt.StatusMessage);
        Assert.Equal(LaunderingTerminalState.LockedUnowned, launderingManager.State);
        Assert.Equal(0, port.DeductCashCalls);
        Assert.Equal(0, port.NativeStartCalls);
    }

    [Fact]
    public void SuccessfulLaunderingStartWhenOwned()
    {
        var purchaseManager = new BusinessPurchaseManager(initiallyOwned: true);
        var launderingManager = new BusinessLaunderingManager(purchaseManager, capacity: 20_000f);
        var port = new MockLaunderingPort { PlayerCash = 10_000f, CurrentLaunderTotal = 0f };
        var context = CreateValidContext();

        var receipt = launderingManager.TryStartLaundering(context, port, 5_000f);

        Assert.True(receipt.Success);
        Assert.Equal("laundering-started", receipt.StatusMessage);
        Assert.Equal(LaunderingTerminalState.Processing, launderingManager.State);

        Assert.Equal(1, port.DeductCashCalls);
        Assert.Equal(1, port.NativeStartCalls);
        Assert.Equal(5_000f, port.PlayerCash); // 10k - 5k
        Assert.Equal(5_000f, port.CurrentLaunderTotal);
        Assert.Equal(1440, launderingManager.RemainingMinutes);
    }

    [Fact]
    public void CapacityExceeded_BlocksWithoutCashDeduction()
    {
        var purchaseManager = new BusinessPurchaseManager(initiallyOwned: true);
        var launderingManager = new BusinessLaunderingManager(purchaseManager, capacity: 20_000f);
        var port = new MockLaunderingPort { LaunderCapacity = 20_000f, CurrentLaunderTotal = 18_000f, PlayerCash = 10_000f };
        var context = CreateValidContext();

        // 5k exceeds remaining 2k capacity
        var receipt = launderingManager.TryStartLaundering(context, port, 5_000f);

        Assert.False(receipt.Success);
        Assert.Equal("capacity-exceeded", receipt.StatusMessage);
        Assert.Equal(LaunderingTerminalState.CapacityExceeded, launderingManager.State);
        Assert.Equal(0, port.DeductCashCalls);
        Assert.Equal(0, port.NativeStartCalls);
        Assert.Equal(10_000f, port.PlayerCash); // Untouched
    }

    [Fact]
    public void InsufficientCash_BlocksWithoutDeduction()
    {
        var purchaseManager = new BusinessPurchaseManager(initiallyOwned: true);
        var launderingManager = new BusinessLaunderingManager(purchaseManager, capacity: 20_000f);
        var port = new MockLaunderingPort { PlayerCash = 2_000f, CurrentLaunderTotal = 0f };
        var context = CreateValidContext();

        var receipt = launderingManager.TryStartLaundering(context, port, 5_000f);

        Assert.False(receipt.Success);
        Assert.Equal("insufficient-cash", receipt.StatusMessage);
        Assert.Equal(LaunderingTerminalState.InsufficientCash, launderingManager.State);
        Assert.Equal(0, port.DeductCashCalls);
        Assert.Equal(0, port.NativeStartCalls);
    }

    [Fact]
    public void SlotGuard_StrictlyProtectsDominikSlot1()
    {
        var purchaseManager = new BusinessPurchaseManager(initiallyOwned: true);
        var launderingManager = new BusinessLaunderingManager(purchaseManager);
        var port = new MockLaunderingPort();
        var context = CreateValidContext(org: "Dominik", slot: "SaveGame_1");

        var receipt = launderingManager.TryStartLaundering(context, port, 5_000f);

        Assert.False(receipt.Success);
        Assert.StartsWith("skipped:test-world-mismatch", receipt.StatusMessage);
        Assert.Equal(0, port.DeductCashCalls);
        Assert.Equal(0, port.NativeStartCalls);
    }

    [Fact]
    public void CompensatingRefund_WhenNativeStartFails()
    {
        var purchaseManager = new BusinessPurchaseManager(initiallyOwned: true);
        var launderingManager = new BusinessLaunderingManager(purchaseManager);
        var port = new MockLaunderingPort { PlayerCash = 10_000f, FailNativeStart = true };
        var context = CreateValidContext();

        var receipt = launderingManager.TryStartLaundering(context, port, 4_000f);

        Assert.False(receipt.Success);
        Assert.StartsWith("refunded:native-start-failed", receipt.StatusMessage);
        Assert.Equal(LaunderingTerminalState.Refunded, launderingManager.State);

        Assert.Equal(1, port.DeductCashCalls);
        Assert.Equal(1, port.RefundCashCalls);
        Assert.Equal(10_000f, port.PlayerCash); // Restored!
    }

    [Fact]
    public void CompensatingRefund_WhenNativeThrowsException()
    {
        var purchaseManager = new BusinessPurchaseManager(initiallyOwned: true);
        var launderingManager = new BusinessLaunderingManager(purchaseManager);
        var port = new MockLaunderingPort { PlayerCash = 10_000f, ThrowNativeStart = true };
        var context = CreateValidContext();

        var receipt = launderingManager.TryStartLaundering(context, port, 4_000f);

        Assert.False(receipt.Success);
        Assert.Contains("refunded:native-exception", receipt.StatusMessage);
        Assert.Equal(LaunderingTerminalState.Refunded, launderingManager.State);

        Assert.Equal(1, port.DeductCashCalls);
        Assert.Equal(1, port.RefundCashCalls);
        Assert.Equal(10_000f, port.PlayerCash); // Restored!
    }

    [Fact]
    public void CriticalRefundFailureHandling()
    {
        var purchaseManager = new BusinessPurchaseManager(initiallyOwned: true);
        var launderingManager = new BusinessLaunderingManager(purchaseManager);
        var port = new MockLaunderingPort { PlayerCash = 10_000f, FailNativeStart = true, FailRefundCash = true };
        var context = CreateValidContext();

        var receipt = launderingManager.TryStartLaundering(context, port, 4_000f);

        Assert.False(receipt.Success);
        Assert.True(receipt.RequiresReconciliation);
        Assert.StartsWith("critical-refund-failed", receipt.StatusMessage);
        Assert.Equal(LaunderingTerminalState.Faulted, launderingManager.State);
        Assert.Equal(receipt, launderingManager.TryStartLaundering(context, port, 4_000f));
        Assert.Equal(1, port.DeductCashCalls);
        Assert.Equal(1, port.RefundCashCalls);
    }

    [Fact]
    public void UncertainDeductionBlocksRetryWithoutRefundOrCreatingJob()
    {
        var purchaseManager = new BusinessPurchaseManager(initiallyOwned: true);
        var launderingManager = new BusinessLaunderingManager(purchaseManager);
        var port = new MockLaunderingPort { PlayerCash = 10_000f, LoseDeductionResult = true };
        var context = CreateValidContext();

        var receipt = launderingManager.TryStartLaundering(context, port, 4_000f);

        Assert.False(receipt.Success);
        Assert.True(receipt.RequiresReconciliation);
        Assert.Equal("cash-deduct-unresolved", receipt.StatusMessage);
        Assert.Equal(receipt, launderingManager.TryStartLaundering(context, port, 4_000f));
        Assert.Equal(6_000f, port.PlayerCash);
        Assert.Equal(1, port.DeductCashCalls);
        Assert.Equal(0, port.RefundCashCalls);
        Assert.Empty(launderingManager.GetActiveOperationsSnapshot());
    }

    [Fact]
    public void RestoreDueOperation_PaysOutAndUpdatesStats()
    {
        var purchaseManager = new BusinessPurchaseManager(initiallyOwned: true);
        var launderingManager = new BusinessLaunderingManager(purchaseManager);
        var port = new MockLaunderingPort();
        var stats = new TownHallBusinessData { TotalLaundered = 10_000f, CompletedLaunderingOperations = 2 };
        launderingManager.RestoreActiveOperations(
            new[] { new ActiveLaunderingOperationRecord { OperationId = "due", Amount = 5_000f, RemainingMinutes = 0, TotalDurationMinutes = 1440 } },
            port);

        bool success = launderingManager.TrySettleDue(port, stats, out float completed);

        Assert.True(success);
        Assert.Equal(5_000f, completed);
        Assert.Equal(LaunderingTerminalState.Completed, launderingManager.State);
        Assert.Equal(1, port.PayoutCalls);
        Assert.Equal(0f, port.CurrentLaunderTotal);
        Assert.Equal(15_000f, stats.TotalLaundered);
        Assert.Equal(3, stats.CompletedLaunderingOperations);
    }

    [Fact]
    public void InvalidAmountRejected()
    {
        var purchaseManager = new BusinessPurchaseManager(initiallyOwned: true);
        var launderingManager = new BusinessLaunderingManager(purchaseManager);
        var port = new MockLaunderingPort();
        var context = CreateValidContext();

        var receiptZero = launderingManager.TryStartLaundering(context, port, 0f);
        Assert.False(receiptZero.Success);
        Assert.Equal("invalid-amount", receiptZero.StatusMessage);

        var receiptNegative = launderingManager.TryStartLaundering(context, port, -100f);
        Assert.False(receiptNegative.Success);
        Assert.Equal("invalid-amount", receiptNegative.StatusMessage);
    }

    [Fact]
    public void AdvanceMinutes_CompletesOnlyAfterTheFullGameDuration()
    {
        var purchaseManager = new BusinessPurchaseManager(initiallyOwned: true);
        var launderingManager = new BusinessLaunderingManager(purchaseManager);
        var port = new MockLaunderingPort { PlayerCash = 8_000f, CurrentLaunderTotal = 0f };
        var context = CreateValidContext();
        var stats = new TownHallBusinessData();

        var receipt = launderingManager.TryStartLaundering(context, port, 6_000f);
        Assert.True(receipt.Success);
        Assert.Equal(6_000f, launderingManager.PendingAmount);
        Assert.Equal(1440, launderingManager.RemainingMinutes);

        launderingManager.AdvanceMinutes(1439);
        Assert.False(launderingManager.TrySettleDue(port, stats, out float completedEarly));
        Assert.Equal(0f, completedEarly);
        Assert.Equal(6_000f, launderingManager.PendingAmount);
        Assert.Equal(1, launderingManager.RemainingMinutes);

        launderingManager.AdvanceMinutes(1);
        Assert.Equal(0, port.PayoutCalls);
        Assert.True(launderingManager.TrySettleDue(port, stats, out float completed));
        Assert.Equal(6_000f, completed);
        Assert.Equal(0f, launderingManager.PendingAmount);
        Assert.Equal(0, launderingManager.RemainingMinutes);
        Assert.Equal(LaunderingTerminalState.Completed, launderingManager.State);
        Assert.Equal(6_000f, stats.TotalLaundered);
        Assert.Equal(1, stats.CompletedLaunderingOperations);
    }

    [Fact]
    public void MultipleConcurrentOperations_TrackIndependentTimelineProgressAndSettleInOrder()
    {
        var purchaseManager = new BusinessPurchaseManager(initiallyOwned: true);
        var launderingManager = new BusinessLaunderingManager(purchaseManager);
        var port = new MockLaunderingPort { PlayerCash = 20_000f, CurrentLaunderTotal = 0f };
        var context = CreateValidContext();
        var stats = new TownHallBusinessData();

        var r1 = launderingManager.TryStartLaundering(context, port, 5_000f);
        Assert.True(r1.Success);

        launderingManager.AdvanceMinutes(720);
        Assert.False(launderingManager.TrySettleDue(port, stats, out _));

        var r2 = launderingManager.TryStartLaundering(context, port, 8_000f);
        Assert.True(r2.Success);

        var snapshot = launderingManager.GetActiveOperationsSnapshot();
        Assert.Equal(2, snapshot.Count);
        Assert.Equal(13_000f, launderingManager.PendingAmount);
        Assert.Equal(720, launderingManager.RemainingMinutes);
        Assert.Equal(720, snapshot[0].GetMinutesSinceStarted());
        Assert.Equal(0, snapshot[1].GetMinutesSinceStarted());

        launderingManager.AdvanceMinutes(720);
        Assert.True(launderingManager.TrySettleDue(port, stats, out float completedFirst));
        Assert.Equal(5_000f, completedFirst);
        Assert.Equal(8_000f, launderingManager.PendingAmount);
        Assert.Equal(720, launderingManager.RemainingMinutes);
        Assert.Equal(LaunderingTerminalState.Processing, launderingManager.State);
        Assert.Equal(r2.OperationId, Assert.Single(stats.ActiveOperations).OperationId);

        launderingManager.AdvanceMinutes(720);
        Assert.True(launderingManager.TrySettleDue(port, stats, out float completedSecond));
        Assert.Equal(8_000f, completedSecond);
        Assert.Equal(0f, launderingManager.PendingAmount);
        Assert.Empty(stats.ActiveOperations);
        Assert.Equal(13_000f, stats.TotalLaundered);
        Assert.Equal(2, stats.CompletedLaunderingOperations);
    }

    [Fact]
    public void SuccessfulPayoutRemovesOnlyItsJobAfterOlderPayoutsWereRejected()
    {
        var purchaseManager = new BusinessPurchaseManager(initiallyOwned: true);
        var launderingManager = new BusinessLaunderingManager(purchaseManager, durationMinutes: 20);
        var port = new MockLaunderingPort { RejectPayout = amount => amount != 3_000f };
        var context = CreateValidContext();
        var stats = new TownHallBusinessData();

        var first = launderingManager.TryStartLaundering(context, port, 1_000f);
        var second = launderingManager.TryStartLaundering(context, port, 2_000f);
        launderingManager.AdvanceMinutes(1);
        var third = launderingManager.TryStartLaundering(context, port, 3_000f);
        Assert.True(first.Success && second.Success && third.Success);

        launderingManager.AdvanceMinutes(19);
        Assert.False(launderingManager.TrySettleDue(port, stats, out _));
        Assert.Equal(2, port.PayoutCalls);

        launderingManager.AdvanceMinutes(1);
        Assert.True(launderingManager.TrySettleDue(port, stats, out float paid));
        Assert.Equal(3_000f, paid);
        Assert.Equal(3_000f, port.PaidToBank);
        Assert.Collection(stats.ActiveOperations,
            job => Assert.Equal(first.OperationId, job.OperationId),
            job => Assert.Equal(second.OperationId, job.OperationId));
        Assert.Equal(1, stats.CompletedLaunderingOperations);
    }

    [Fact]
    public void ExceptionAfterPayoutHoldsJobAndDoesNotRepeatAfterReload()
    {
        var purchaseManager = new BusinessPurchaseManager(initiallyOwned: true);
        var launderingManager = new BusinessLaunderingManager(purchaseManager);
        var port = new MockLaunderingPort { ThrowAfterPayout = true };
        var context = CreateValidContext();
        var stats = new TownHallBusinessData();
        var receipt = launderingManager.TryStartLaundering(context, port, 100f);
        Assert.True(receipt.Success);

        launderingManager.AdvanceMinutes(1440);
        Assert.False(launderingManager.TrySettleDue(port, stats, out _));
        Assert.False(launderingManager.TrySettleDue(port, stats, out _));
        Assert.Equal(100f, port.PaidToBank);
        Assert.Equal(1, port.PayoutCalls);
        var held = Assert.Single(stats.ActiveOperations);
        Assert.Equal(receipt.OperationId, held.OperationId);
        Assert.True(held.PayoutUnresolved);
        Assert.Equal(1, held.PayoutAttempts);
        Assert.Equal(0, held.RemainingMinutes);

        var reloadedManager = new BusinessLaunderingManager(purchaseManager);
        var reloadedPort = new MockLaunderingPort();
        reloadedManager.RestoreActiveOperations(stats.ActiveOperations, reloadedPort);
        Assert.False(reloadedManager.TrySettleDue(reloadedPort, stats, out _));
        Assert.Equal(0, reloadedPort.PayoutCalls);
        Assert.Equal(100f, reloadedManager.PendingAmount);
    }

    [Fact]
    public void ConfirmedRejectionsAreRetriedAtMostThreeTimes()
    {
        var purchaseManager = new BusinessPurchaseManager(initiallyOwned: true);
        var launderingManager = new BusinessLaunderingManager(purchaseManager);
        var port = new MockLaunderingPort { RejectPayout = _ => true };
        var context = CreateValidContext();
        var stats = new TownHallBusinessData();
        var receipt = launderingManager.TryStartLaundering(context, port, 500f);
        Assert.True(receipt.Success);

        launderingManager.AdvanceMinutes(1440);
        for (int i = 0; i < 4; i++)
        {
            Assert.False(launderingManager.TrySettleDue(port, stats, out _));
            launderingManager.AdvanceMinutes(1);
        }

        Assert.Equal(3, port.PayoutCalls);
        Assert.Equal(0f, port.PaidToBank);
        Assert.Equal(LaunderingTerminalState.Faulted, launderingManager.State);
        var held = Assert.Single(stats.ActiveOperations);
        Assert.Equal(receipt.OperationId, held.OperationId);
        Assert.False(held.PayoutUnresolved);
        Assert.Equal(3, held.PayoutAttempts);
    }

    [Fact]
    public void RestoreActiveOperations_RehydratesPendingOperationsAndPortTotal()
    {
        var purchaseManager = new BusinessPurchaseManager(initiallyOwned: true);
        var launderingManager = new BusinessLaunderingManager(purchaseManager);
        var port = new MockLaunderingPort { CurrentLaunderTotal = 0f };

        var savedOps = new[]
        {
            new ActiveLaunderingOperationRecord
            {
                OperationId = "op-1",
                Amount = 4_000f,
                RemainingMinutes = 360,
                TotalDurationMinutes = 1440
            },
            new ActiveLaunderingOperationRecord
            {
                OperationId = "op-2",
                Amount = 6_000f,
                RemainingMinutes = 1080,
                TotalDurationMinutes = 1440
            }
        };

        launderingManager.RestoreActiveOperations(savedOps, port);

        Assert.Equal(10_000f, launderingManager.PendingAmount);
        Assert.Equal(10_000f, port.CurrentLaunderTotal);
        Assert.Equal(360, launderingManager.RemainingMinutes);
        Assert.Equal(LaunderingTerminalState.Processing, launderingManager.State);

        var snapshot = launderingManager.GetActiveOperationsSnapshot();
        Assert.Equal(2, snapshot.Count);
        Assert.Equal(1080, snapshot[0].GetMinutesSinceStarted()); // 75% elapsed
        Assert.Equal(360, snapshot[1].GetMinutesSinceStarted());  // 25% elapsed
    }
}
