using System;
using System.IO;
using TownHallInterior;
using Xunit;

namespace TownHallInterior.Tests;

public sealed class BusinessPurchaseTests
{
    private sealed class MockPaymentPort : IPurchasePaymentPort
    {
        public float OnlineBalance { get; set; } = 300_000f;
        public float CashBalance { get; set; } = 50_000f;

        public int BankDeductCalls { get; private set; }
        public int CashDeductCalls { get; private set; }
        public int BankRefundCalls { get; private set; }
        public int CashRefundCalls { get; private set; }

        public bool FailBankDeduct { get; set; }
        public bool FailCashDeduct { get; set; }
        public bool FailBankRefund { get; set; }
        public bool FailCashRefund { get; set; }
        public Action? AfterBankDeduction { get; set; }

        public float GetOnlineBalance() => OnlineBalance;
        public float GetCashBalance() => CashBalance;

        public MoneyMutationOutcome DeductBank(float amount, string label, string note)
        {
            BankDeductCalls++;
            if (FailBankDeduct || OnlineBalance < amount) return MoneyMutationOutcome.NotApplied;
            OnlineBalance -= amount;
            AfterBankDeduction?.Invoke();
            return MoneyMutationOutcome.Applied;
        }

        public MoneyMutationOutcome DeductCash(float amount)
        {
            CashDeductCalls++;
            if (FailCashDeduct || CashBalance < amount) return MoneyMutationOutcome.NotApplied;
            CashBalance -= amount;
            return MoneyMutationOutcome.Applied;
        }

        public MoneyMutationOutcome RefundBank(float amount, string label, string note)
        {
            BankRefundCalls++;
            if (FailBankRefund) return MoneyMutationOutcome.NotApplied;
            OnlineBalance += amount;
            return MoneyMutationOutcome.Applied;
        }

        public MoneyMutationOutcome RefundCash(float amount)
        {
            CashRefundCalls++;
            if (FailCashRefund) return MoneyMutationOutcome.NotApplied;
            CashBalance += amount;
            return MoneyMutationOutcome.Applied;
        }
    }

    private sealed class MockWorldPort : IBusinessWorldPort
    {
        public bool InOwnedList { get; private set; }
        public int RegisterCalls { get; private set; }
        public bool FailRegister { get; set; }
        public bool ThrowRegister { get; set; }

        public WorldRegistrationSnapshot CaptureSnapshot() =>
            new(true, !InOwnedList, InOwnedList, true, !InOwnedList, InOwnedList, true);

        public bool RegisterInWorld(WorldRegistrationIdentity identity, bool isOwned)
        {
            RegisterCalls++;
            if (ThrowRegister) throw new InvalidOperationException("simulated-world-crash");
            if (FailRegister) return false;
            InOwnedList = isOwned;
            return true;
        }

        public bool DeregisterFromWorld(WorldRegistrationIdentity identity)
        {
            InOwnedList = false;
            return true;
        }
    }

    private static ConstructionContext CreateValidContext(string org = "TownHallTest", string slot = "SaveGame_2")
    {
        string dir = Path.Combine(Path.GetTempPath(), "TownHallPurchaseTests", slot);
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
    public void InitialStateIsUnowned()
    {
        var manager = new BusinessPurchaseManager();
        Assert.False(manager.IsOwned);
        Assert.Equal(PurchaseState.Unowned, manager.State);
        Assert.Equal(250_000f, manager.Price);
        Assert.Null(manager.LastReceipt);
    }

    [Fact]
    public void SuccessfulBankPurchase()
    {
        var manager = new BusinessPurchaseManager();
        var payment = new MockPaymentPort { OnlineBalance = 300_000f, CashBalance = 10_000f };
        var world = new MockWorldPort();
        var context = CreateValidContext();
        var identity = new WorldRegistrationIdentity();

        var receipt = manager.TryPurchase(context, payment, world, identity, PurchasePaymentMode.Auto);

        Assert.True(receipt.Success);
        Assert.Equal(PurchasePaymentMode.Bank, receipt.ResolvedMode);
        Assert.Equal(250_000f, receipt.Amount);
        Assert.Equal("purchase-completed", receipt.StatusMessage);

        Assert.True(manager.IsOwned);
        Assert.Equal(PurchaseState.Completed, manager.State);
        Assert.Equal(50_000f, payment.OnlineBalance); // 300k - 250k
        Assert.Equal(1, payment.BankDeductCalls);
        Assert.Equal(0, payment.CashDeductCalls);
        Assert.Equal(1, world.RegisterCalls);
        Assert.True(world.InOwnedList);
    }

    [Fact]
    public void SuccessfulCashPurchaseWhenBankInsufficient()
    {
        var manager = new BusinessPurchaseManager();
        var payment = new MockPaymentPort { OnlineBalance = 5_000f, CashBalance = 260_000f };
        var world = new MockWorldPort();
        var context = CreateValidContext();
        var identity = new WorldRegistrationIdentity();

        var receipt = manager.TryPurchase(context, payment, world, identity, PurchasePaymentMode.Auto);

        Assert.True(receipt.Success);
        Assert.Equal(PurchasePaymentMode.Cash, receipt.ResolvedMode);
        Assert.Equal(250_000f, receipt.Amount);

        Assert.True(manager.IsOwned);
        Assert.Equal(PurchaseState.Completed, manager.State);
        Assert.Equal(10_000f, payment.CashBalance); // 260k - 250k
        Assert.Equal(0, payment.BankDeductCalls);
        Assert.Equal(1, payment.CashDeductCalls);
        Assert.True(world.InOwnedList);
    }

    [Fact]
    public void InsufficientFundsRejectsWithoutDeduction()
    {
        var manager = new BusinessPurchaseManager();
        var payment = new MockPaymentPort { OnlineBalance = 50_000f, CashBalance = 20_000f };
        var world = new MockWorldPort();
        var context = CreateValidContext();
        var identity = new WorldRegistrationIdentity();

        var receipt = manager.TryPurchase(context, payment, world, identity, PurchasePaymentMode.Auto);

        Assert.False(receipt.Success);
        Assert.Equal("insufficient-funds", receipt.StatusMessage);
        Assert.False(manager.IsOwned);
        Assert.Equal(PurchaseState.InsufficientFunds, manager.State);

        Assert.Equal(50_000f, payment.OnlineBalance);
        Assert.Equal(20_000f, payment.CashBalance);
        Assert.Equal(0, payment.BankDeductCalls);
        Assert.Equal(0, payment.CashDeductCalls);
        Assert.Equal(0, world.RegisterCalls);
    }

    [Fact]
    public void AlreadyOwnedRejectsWithoutDeduction()
    {
        var manager = new BusinessPurchaseManager(initiallyOwned: true);
        var payment = new MockPaymentPort();
        var world = new MockWorldPort();
        var context = CreateValidContext();
        var identity = new WorldRegistrationIdentity();

        var receipt = manager.TryPurchase(context, payment, world, identity);

        Assert.False(receipt.Success);
        Assert.Equal("already-owned", receipt.StatusMessage);
        Assert.Equal(0, payment.BankDeductCalls);
        Assert.Equal(0, world.RegisterCalls);
    }

    [Fact]
    public void SlotGuard_StrictlyProtectsDominikSlot1()
    {
        var manager = new BusinessPurchaseManager();
        var payment = new MockPaymentPort();
        var world = new MockWorldPort();
        var context = CreateValidContext(org: "Dominik", slot: "SaveGame_1");
        var identity = new WorldRegistrationIdentity();

        var receipt = manager.TryPurchase(context, payment, world, identity);

        Assert.False(receipt.Success);
        Assert.StartsWith("skipped:test-world-mismatch", receipt.StatusMessage);
        Assert.False(manager.IsOwned);
        Assert.Equal(0, payment.BankDeductCalls);
        Assert.Equal(0, world.RegisterCalls);
    }

    [Fact]
    public void PaymentDeclinedByPort_FailsWithoutWorldRegistration()
    {
        var manager = new BusinessPurchaseManager();
        var payment = new MockPaymentPort { FailBankDeduct = true };
        var world = new MockWorldPort();
        var context = CreateValidContext();
        var identity = new WorldRegistrationIdentity();

        var receipt = manager.TryPurchase(context, payment, world, identity, PurchasePaymentMode.Bank);

        Assert.False(receipt.Success);
        Assert.Equal("payment-declined", receipt.StatusMessage);
        Assert.Equal(PurchaseState.Faulted, manager.State);
        Assert.False(manager.IsOwned);
        Assert.Equal(0, world.RegisterCalls);
    }

    [Fact]
    public void CompensatingRefund_WhenWorldRegistrationFailsAfterDeduction()
    {
        var manager = new BusinessPurchaseManager();
        var payment = new MockPaymentPort { OnlineBalance = 300_000f };
        var world = new MockWorldPort { FailRegister = true };
        var context = CreateValidContext();
        var identity = new WorldRegistrationIdentity();

        var receipt = manager.TryPurchase(context, payment, world, identity, PurchasePaymentMode.Bank);

        Assert.False(receipt.Success);
        Assert.StartsWith("refunded:registration-failed", receipt.StatusMessage);
        Assert.Equal(PurchaseState.Refunded, manager.State);
        Assert.False(manager.IsOwned);

        // Verification of compensating refund
        Assert.Equal(1, payment.BankDeductCalls);
        Assert.Equal(1, payment.BankRefundCalls);
        Assert.Equal(300_000f, payment.OnlineBalance); // Balance fully restored!
    }

    [Fact]
    public void CompensatingRefund_WhenWorldRegistrationThrowsException()
    {
        var manager = new BusinessPurchaseManager();
        var payment = new MockPaymentPort { OnlineBalance = 5_000f, CashBalance = 300_000f };
        var world = new MockWorldPort { ThrowRegister = true };
        var context = CreateValidContext();
        var identity = new WorldRegistrationIdentity();

        var receipt = manager.TryPurchase(context, payment, world, identity, PurchasePaymentMode.Cash);

        Assert.False(receipt.Success);
        Assert.Contains("refunded:registration-exception", receipt.StatusMessage);
        Assert.Equal(PurchaseState.Refunded, manager.State);
        Assert.False(manager.IsOwned);

        // Verification of cash refund
        Assert.Equal(1, payment.CashDeductCalls);
        Assert.Equal(1, payment.CashRefundCalls);
        Assert.Equal(300_000f, payment.CashBalance); // Cash fully restored!
    }

    [Fact]
    public void CriticalRefundFailureHandling()
    {
        var manager = new BusinessPurchaseManager();
        var payment = new MockPaymentPort { FailBankRefund = true };
        var world = new MockWorldPort { FailRegister = true };
        var context = CreateValidContext();
        var identity = new WorldRegistrationIdentity();

        var receipt = manager.TryPurchase(context, payment, world, identity, PurchasePaymentMode.Bank);

        Assert.False(receipt.Success);
        Assert.True(receipt.RequiresReconciliation);
        Assert.StartsWith("critical-refund-failed", receipt.StatusMessage);
        Assert.Equal(PurchaseState.Faulted, manager.State);

        Assert.Equal(receipt, manager.TryPurchase(context, payment, world, identity, PurchasePaymentMode.Bank));
        Assert.Equal(1, payment.BankDeductCalls);
        Assert.Equal(1, payment.BankRefundCalls);
    }

    [Fact]
    public void ExceptionAfterDeductionHoldsTheSamePurchaseWithoutRefundOrRetry()
    {
        var manager = new BusinessPurchaseManager();
        var payment = new MockPaymentPort
        {
            OnlineBalance = 600_000f,
            AfterBankDeduction = () => throw new InvalidOperationException("simulated balance callback failure")
        };
        var world = new MockWorldPort();
        var context = CreateValidContext();
        var identity = new WorldRegistrationIdentity();

        var receipt = manager.TryPurchase(context, payment, world, identity);

        Assert.False(receipt.Success);
        Assert.True(receipt.RequiresReconciliation);
        Assert.Equal("payment-unresolved", receipt.StatusMessage);
        Assert.Equal(receipt, manager.TryPurchase(context, payment, world, identity));
        Assert.Equal(350_000f, payment.OnlineBalance);
        Assert.Equal(1, payment.BankDeductCalls);
        Assert.Equal(0, payment.BankRefundCalls);
        Assert.Equal(0, world.RegisterCalls);
        Assert.False(manager.IsOwned);
    }

    [Fact]
    public void ReentrantPurchaseCannotDebitWhileOriginalIsInProgress()
    {
        var manager = new BusinessPurchaseManager();
        var payment = new MockPaymentPort { OnlineBalance = 600_000f };
        var world = new MockWorldPort();
        var context = CreateValidContext();
        var identity = new WorldRegistrationIdentity();
        PurchaseReceipt? nested = null;
        bool wasMutating = false;
        payment.AfterBankDeduction = () =>
        {
            wasMutating = manager.IsMutating;
            nested = manager.TryPurchase(context, payment, world, identity);
        };

        var receipt = manager.TryPurchase(context, payment, world, identity);

        Assert.True(receipt.Success);
        Assert.True(wasMutating);
        Assert.NotNull(nested);
        Assert.Equal("blocked:transaction-in-progress", nested.StatusMessage);
        Assert.Equal(350_000f, payment.OnlineBalance);
        Assert.Equal(1, payment.BankDeductCalls);
        Assert.False(manager.IsMutating);
    }

    [Fact]
    public void RestoreOwnershipSetsStateCorrectly()
    {
        var manager = new BusinessPurchaseManager();
        Assert.False(manager.IsOwned);

        manager.RestoreOwnership(true);
        Assert.True(manager.IsOwned);
        Assert.Equal(PurchaseState.Completed, manager.State);

        manager.RestoreOwnership(false);
        Assert.False(manager.IsOwned);
        Assert.Equal(PurchaseState.Unowned, manager.State);
    }
}
