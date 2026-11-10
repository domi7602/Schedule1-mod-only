using System;
using System.Collections.Generic;
using System.IO;
using TownHallInterior;
using Xunit;

namespace TownHallInterior.Tests;

public sealed class BusinessWorldRegistrationTests
{
    private sealed class MockWorldPort : IBusinessWorldPort
    {
        public bool InBusinessList { get; set; }
        public bool InUnownedBusinessList { get; set; }
        public bool InOwnedBusinessList { get; set; }
        public bool InPropertyList { get; set; }
        public bool InUnownedPropertyList { get; set; }
        public bool InOwnedPropertyList { get; set; }
        public bool InSaveablesList { get; set; }

        public int RegisterCalls { get; private set; }
        public int DeregisterCalls { get; private set; }
        public bool FailOnRegister { get; set; }
        public bool ThrowOnRegister { get; set; }
        public bool FailOnDeregister { get; set; }
        public bool ThrowOnDeregister { get; set; }

        public WorldRegistrationSnapshot CaptureSnapshot() =>
            new(InBusinessList, InUnownedBusinessList, InOwnedBusinessList,
                InPropertyList, InUnownedPropertyList, InOwnedPropertyList, InSaveablesList);

        public bool RegisterInWorld(WorldRegistrationIdentity identity, bool isOwned)
        {
            RegisterCalls++;
            if (ThrowOnRegister) throw new InvalidOperationException("simulated-register-crash");
            if (FailOnRegister) return false;

            InBusinessList = true;
            InUnownedBusinessList = !isOwned;
            InOwnedBusinessList = isOwned;
            InPropertyList = true;
            InUnownedPropertyList = !isOwned;
            InOwnedPropertyList = isOwned;
            InSaveablesList = true;
            return true;
        }

        public bool DeregisterFromWorld(WorldRegistrationIdentity identity)
        {
            DeregisterCalls++;
            if (ThrowOnDeregister) throw new InvalidOperationException("simulated-deregister-crash");
            if (FailOnDeregister) return false;

            InBusinessList = false;
            InUnownedBusinessList = false;
            InOwnedBusinessList = false;
            InPropertyList = false;
            InUnownedPropertyList = false;
            InOwnedPropertyList = false;
            InSaveablesList = false;
            return true;
        }
    }

    private static ConstructionContext CreateValidContext(string org = "TownHallTest", string slot = "SaveGame_2")
    {
        string dir = Path.Combine(Path.GetTempPath(), "TownHallRegistrationTests", slot);
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
    public void InitialStateIsUnregistered()
    {
        var manager = new BusinessWorldRegistrationManager();
        Assert.Equal(WorldRegistrationState.Unregistered, manager.State);
        Assert.False(manager.IsRegistered);
        Assert.Equal("Hyland Town Hall", manager.Identity.PropertyName);
        Assert.Equal("hyland_town_hall", manager.Identity.PropertyCode);
    }

    [Fact]
    public void SuccessfulUnownedRegistration_AndDeregistration()
    {
        var manager = new BusinessWorldRegistrationManager();
        var port = new MockWorldPort();
        var context = CreateValidContext();

        // 1. Register unowned
        var regResult = manager.Register(context, port, isOwned: false);

        Assert.Equal(WorldRegistrationState.Registered, manager.State);
        Assert.True(manager.IsRegistered);
        Assert.Equal("registered", regResult.Status);
        Assert.Null(regResult.Reason);
        Assert.Equal(1, port.RegisterCalls);

        Assert.NotNull(regResult.BeforeSnapshot);
        Assert.False(regResult.BeforeSnapshot.InBusinessList);

        Assert.NotNull(regResult.AfterSnapshot);
        Assert.True(regResult.AfterSnapshot.InBusinessList);
        Assert.True(regResult.AfterSnapshot.InUnownedBusinessList);
        Assert.False(regResult.AfterSnapshot.InOwnedBusinessList);
        Assert.True(regResult.AfterSnapshot.InPropertyList);
        Assert.True(regResult.AfterSnapshot.InUnownedPropertyList);
        Assert.False(regResult.AfterSnapshot.InOwnedPropertyList);
        Assert.True(regResult.AfterSnapshot.InSaveablesList);

        // 2. Deregister
        var deregResult = manager.Deregister(port);

        Assert.Equal(WorldRegistrationState.Unregistered, manager.State);
        Assert.False(manager.IsRegistered);
        Assert.Equal("deregistered", deregResult.Status);
        Assert.Null(deregResult.Reason);
        Assert.Equal(1, port.DeregisterCalls);

        Assert.NotNull(deregResult.AfterSnapshot);
        Assert.False(deregResult.AfterSnapshot.InBusinessList);
        Assert.False(deregResult.AfterSnapshot.InPropertyList);
        Assert.False(deregResult.AfterSnapshot.InSaveablesList);
    }

    [Fact]
    public void SuccessfulOwnedRegistration()
    {
        var manager = new BusinessWorldRegistrationManager();
        var port = new MockWorldPort();
        var context = CreateValidContext();

        var result = manager.Register(context, port, isOwned: true);

        Assert.Equal(WorldRegistrationState.Registered, manager.State);
        Assert.True(manager.IsRegistered);
        Assert.Equal("registered", result.Status);

        Assert.NotNull(result.AfterSnapshot);
        Assert.True(result.AfterSnapshot.InBusinessList);
        Assert.False(result.AfterSnapshot.InUnownedBusinessList);
        Assert.True(result.AfterSnapshot.InOwnedBusinessList);
        Assert.True(result.AfterSnapshot.InPropertyList);
        Assert.False(result.AfterSnapshot.InUnownedPropertyList);
        Assert.True(result.AfterSnapshot.InOwnedPropertyList);
        Assert.True(result.AfterSnapshot.InSaveablesList);
    }

    [Fact]
    public void SlotGuard_BlocksDominikSlot1Strictly()
    {
        var manager = new BusinessWorldRegistrationManager();
        var port = new MockWorldPort();

        // Target slot 1 "Dominik" / "SaveGame_1"
        var context = CreateValidContext(org: "Dominik", slot: "SaveGame_1");

        var result = manager.Register(context, port);

        Assert.Equal(WorldRegistrationState.Unregistered, manager.State);
        Assert.False(manager.IsRegistered);
        Assert.Equal("skipped", result.Status);
        Assert.Equal("test-world-mismatch", result.Reason);
        Assert.Equal(0, port.RegisterCalls);
    }

    [Fact]
    public void GateCheck_RejectsWhenContextIncomplete()
    {
        var manager = new BusinessWorldRegistrationManager();
        var port = new MockWorldPort();
        string dir = Path.Combine(Path.GetTempPath(), "TownHallRegistrationTests", "SaveGame_2");

        var incompleteContext = new ConstructionContext(
            LoadComplete: false,
            Singleplayer: true,
            HallReady: true,
            DependenciesMatch: true,
            ActiveSavePath: dir,
            LoadedSavePath: dir,
            OrganisationName: "TownHallTest");

        var result = manager.Register(incompleteContext, port);

        Assert.Equal(WorldRegistrationState.Unregistered, manager.State);
        Assert.Equal("skipped", result.Status);
        Assert.Equal("load-incomplete", result.Reason);
        Assert.Equal(0, port.RegisterCalls);
    }

    [Fact]
    public void IdempotentAlreadyRegistered()
    {
        var manager = new BusinessWorldRegistrationManager();
        var port = new MockWorldPort();
        var context = CreateValidContext();

        manager.Register(context, port);
        Assert.Equal(1, port.RegisterCalls);

        var secondResult = manager.Register(context, port);
        Assert.Equal("already-registered", secondResult.Status);
        Assert.Equal(1, port.RegisterCalls);
    }

    [Fact]
    public void IdempotentDeregistration()
    {
        var manager = new BusinessWorldRegistrationManager();
        var port = new MockWorldPort();

        var result = manager.Deregister(port);
        Assert.Equal("noop", result.Status);
        Assert.Equal(0, port.DeregisterCalls);
    }

    [Fact]
    public void FailClosedOnPortFailure()
    {
        var manager = new BusinessWorldRegistrationManager();
        var port = new MockWorldPort { FailOnRegister = true };
        var context = CreateValidContext();

        var result = manager.Register(context, port);

        Assert.Equal(WorldRegistrationState.Faulted, manager.State);
        Assert.False(manager.IsRegistered);
        Assert.Equal("failed", result.Status);
        Assert.Equal(1, port.RegisterCalls);
        Assert.Equal(1, port.DeregisterCalls); // fail-closed cleanup invoked
    }

    [Fact]
    public void FailClosedOnException()
    {
        var manager = new BusinessWorldRegistrationManager();
        var port = new MockWorldPort { ThrowOnRegister = true };
        var context = CreateValidContext();

        var result = manager.Register(context, port);

        Assert.Equal(WorldRegistrationState.Faulted, manager.State);
        Assert.False(manager.IsRegistered);
        Assert.Equal("faulted", result.Status);
        Assert.Contains("simulated-register-crash", result.Reason);
        Assert.Equal(1, port.RegisterCalls);
        Assert.Equal(1, port.DeregisterCalls); // fail-closed cleanup invoked
    }

    [Fact]
    public void ResetRestoresUnregisteredState()
    {
        var manager = new BusinessWorldRegistrationManager();
        var port = new MockWorldPort { FailOnRegister = true };
        var context = CreateValidContext();

        manager.Register(context, port);
        Assert.Equal(WorldRegistrationState.Faulted, manager.State);

        manager.Reset();
        Assert.Equal(WorldRegistrationState.Unregistered, manager.State);
        Assert.False(manager.IsRegistered);
    }
}
