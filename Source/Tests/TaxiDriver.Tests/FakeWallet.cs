using System;
using System.Collections.Generic;

namespace TaxiDriver.Tests;

/// <summary>
/// Scriptable <see cref="IGameWallet"/> fake for the payment tests. The
/// mutation order mirrors the real hazard shape: the "AfterMutation" throw
/// modes mutate the balance FIRST and then throw (S1API fires its event after
/// the manager call, and the native methods can plausibly mutate before
/// raising - outcome unknown). "SilentNoOp" mirrors MoneyManager.Instance ==
/// null: clean return, no mutation. A fake proves how a modeled scenario is
/// HANDLED, not that the scenario occurs in the game.
/// </summary>
internal sealed class FakeWallet : IGameWallet
{
    public enum ThrowMode
    {
        None,
        BeforeMutation,
        AfterMutation,
    }

    public float CashBalance = 100f;
    public float BankBalance = 100f;
    public bool ThrowOnGet;
    public ThrowMode ChangeThrow = ThrowMode.None;
    public ThrowMode TransactionThrow = ThrowMode.None;
    public bool SilentNoOp;

    public int GetCalls;
    public int ChangeCalls;
    public int TransactionCalls;
    public readonly List<string> CallOrder = new();
    public float LastChangeDelta;
    public string LastTransactionName = string.Empty;
    public float LastUnitAmount;
    public float LastQuantity;
    public string LastNote = string.Empty;

    public float GetCashBalance()
    {
        GetCalls++;
        CallOrder.Add("get");
        if (ThrowOnGet)
            throw new InvalidOperationException("fake: cash read failed");
        return CashBalance;
    }

    public void ChangeCashBalance(float delta, bool visualizeChange, bool playCashSound)
    {
        ChangeCalls++;
        CallOrder.Add("change");
        LastChangeDelta = delta;
        if (ChangeThrow == ThrowMode.BeforeMutation)
            throw new InvalidOperationException("fake: change threw before mutating");
        if (!SilentNoOp)
            CashBalance += delta;
        if (ChangeThrow == ThrowMode.AfterMutation)
            throw new InvalidOperationException("fake: change threw after mutating");
    }

    public void CreateOnlineTransaction(string transactionName, float unitAmount, float quantity, string transactionNote)
    {
        TransactionCalls++;
        CallOrder.Add("transaction");
        LastTransactionName = transactionName;
        LastUnitAmount = unitAmount;
        LastQuantity = quantity;
        LastNote = transactionNote;
        if (TransactionThrow == ThrowMode.BeforeMutation)
            throw new InvalidOperationException("fake: transaction threw before mutating");
        if (!SilentNoOp)
            BankBalance += unitAmount * quantity;
        if (TransactionThrow == ThrowMode.AfterMutation)
            throw new InvalidOperationException("fake: transaction threw after mutating");
    }
}
