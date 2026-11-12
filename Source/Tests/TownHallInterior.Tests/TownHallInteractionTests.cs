using System;
using System.Collections.Generic;
using System.Globalization;
using TownHallInterior;
using Xunit;

namespace TownHallInterior.Tests;

public sealed class TownHallInteractionTests
{
    private sealed class MockPlayerPort : ITownHallPlayerPort
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public bool PlayerAvailable { get; set; } = true;
        public bool KeyPressed { get; set; }
        public List<(string Title, string Message, bool PlaySound)> Notifications { get; } = new();

        public bool TryGetPlayerPosition(out float x, out float y, out float z)
        {
            x = X;
            y = Y;
            z = Z;
            return PlayerAvailable;
        }

        public bool IsInteractKeyPressed() => KeyPressed;

        public void ShowNotification(string title, string message, bool playSound = true)
        {
            Notifications.Add((title, message, playSound));
        }
    }

    private sealed class MockLaunderingPort : ILaunderingPort
    {
        public float Capacity { get; set; } = 20_000f;
        public float CurrentTotal { get; set; }
        public float PlayerCash { get; set; } = 5_000f;
        public List<string> History { get; } = new();

        public float GetLaunderCapacity() => Capacity;
        public float GetCurrentLaunderTotal() => CurrentTotal;
        public float GetPlayerCash() => PlayerCash;

        public MoneyMutationOutcome DeductCash(float amount)
        {
            if (PlayerCash < amount) return MoneyMutationOutcome.NotApplied;
            PlayerCash -= amount;
            History.Add($"deduct:{amount}");
            return MoneyMutationOutcome.Applied;
        }

        public MoneyMutationOutcome RefundCash(float amount)
        {
            PlayerCash += amount;
            History.Add($"refund:{amount}");
            return MoneyMutationOutcome.Applied;
        }

        public bool StartNativeLaundering(float amount)
        {
            CurrentTotal += amount;
            History.Add($"native-start:{amount}");
            return true;
        }

        public MoneyMutationOutcome PayoutCleanFunds(float amount, string label, string note)
        {
            History.Add($"payout:{amount}");
            return MoneyMutationOutcome.Applied;
        }
    }

    private static ConstructionContext CreateContext(string org = "TownHallTest", string slot = "SaveGame_2")
    {
        string dir = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "TownHallInteractionTests", slot);
        System.IO.Directory.CreateDirectory(dir);
        return new ConstructionContext(
            LoadComplete: true,
            Singleplayer: true,
            HallReady: true,
            DependenciesMatch: true,
            ActiveSavePath: dir,
            LoadedSavePath: dir,
            OrganisationName: org);
    }

    private static LaunderingOperationReceipt UnexpectedLaundering(float amount) =>
        throw new InvalidOperationException("Interaction must not start laundering.");

    [Fact]
    public void EvaluateZone_OutsideAllRanges_ReturnsNone()
    {
        var purchase = new BusinessPurchaseManager();
        var laundering = new BusinessLaunderingManager(purchase);
        var player = new MockPlayerPort { X = 0f, Y = 0f, Z = 0f };

        var coordinator = new TownHallInteractionCoordinator(
            purchase, laundering, player,
            () => true, () => null, UnexpectedLaundering);

        var zone = coordinator.EvaluateZone(0f, 0f, 0f);
        Assert.Equal(InteractionZoneKind.None, zone);
    }

    [Fact]
    public void EvaluateZone_WithinEntranceRange_ReturnsEntrancePurchase()
    {
        var purchase = new BusinessPurchaseManager();
        var laundering = new BusinessLaunderingManager(purchase);
        var player = new MockPlayerPort();

        var coordinator = new TownHallInteractionCoordinator(
            purchase, laundering, player,
            () => true, () => null, UnexpectedLaundering);

        // Entrance is around X=54.0f, Y=2.0f, Z=30.5f
        var zone = coordinator.EvaluateZone(54.0f, 2.0f, 30.5f);
        Assert.Equal(InteractionZoneKind.EntrancePurchase, zone);
    }

    [Fact]
    public void EvaluateZone_WithinTerminalRange_ReturnsLaunderingTerminal()
    {
        var purchase = new BusinessPurchaseManager();
        var laundering = new BusinessLaunderingManager(purchase);
        var player = new MockPlayerPort();

        var coordinator = new TownHallInteractionCoordinator(
            purchase, laundering, player,
            () => true, () => null, UnexpectedLaundering);

        // Terminal is around X=56.4f, Y=5.5f, Z=30.25f (upper floor)
        var zone = coordinator.EvaluateZone(56.4f, 5.5f, 30.25f);
        Assert.Equal(InteractionZoneKind.LaunderingTerminal, zone);
    }

    [Fact]
    public void EvaluateZone_WithinWhiteboardRange_ReturnsWhiteboardPurchase()
    {
        var purchase = new BusinessPurchaseManager();
        var laundering = new BusinessLaunderingManager(purchase);
        var player = new MockPlayerPort();

        var coordinator = new TownHallInteractionCoordinator(
            purchase, laundering, player,
            () => true, () => null, UnexpectedLaundering,
            whiteboardPositionProvider: () => new SpatialPoint(100f, 1f, 200f));

        var zone = coordinator.EvaluateZone(100.5f, 1.2f, 200.5f);
        Assert.Equal(InteractionZoneKind.WhiteboardPurchase, zone);
    }

    [Fact]
    public void Tick_WhenUnownedEntrance_ResolvesPurchasePrompt()
    {
        var purchase = new BusinessPurchaseManager();
        var laundering = new BusinessLaunderingManager(purchase);
        var player = new MockPlayerPort { X = 54.0f, Y = 2.0f, Z = 30.5f };

        var coordinator = new TownHallInteractionCoordinator(
            purchase, laundering, player,
            () => true, () => null, UnexpectedLaundering);

        coordinator.Tick(0.1f);

        Assert.NotNull(coordinator.CurrentPrompt);
        Assert.Equal(InteractionZoneKind.EntrancePurchase, coordinator.CurrentPrompt.Zone);
        Assert.Contains("Ray", coordinator.CurrentPrompt.Detail);
        Assert.False(coordinator.CurrentPrompt.CanInteract);
    }

    [Fact]
    public void Tick_WhenOwnedEntrance_ResolvesOwnedPrompt()
    {
        var purchase = new BusinessPurchaseManager();
        purchase.RestoreOwnership(true);
        var laundering = new BusinessLaunderingManager(purchase);
        var player = new MockPlayerPort { X = 54.0f, Y = 2.0f, Z = 30.5f };

        var coordinator = new TownHallInteractionCoordinator(
            purchase, laundering, player,
            () => true, () => null, UnexpectedLaundering);

        coordinator.Tick(0.1f);

        Assert.NotNull(coordinator.CurrentPrompt);
        Assert.Equal(InteractionZoneKind.EntrancePurchase, coordinator.CurrentPrompt.Zone);
        Assert.Contains("Im Besitz", coordinator.CurrentPrompt.Detail);
        Assert.False(coordinator.CurrentPrompt.CanInteract);
    }

    [Fact]
    public void Tick_WhenUnownedTerminal_ResolvesLockedPrompt()
    {
        var purchase = new BusinessPurchaseManager();
        var laundering = new BusinessLaunderingManager(purchase);
        var player = new MockPlayerPort { X = 56.4f, Y = 5.5f, Z = 30.25f };

        var coordinator = new TownHallInteractionCoordinator(
            purchase, laundering, player,
            () => true, () => null, UnexpectedLaundering);

        coordinator.Tick(0.1f);

        Assert.NotNull(coordinator.CurrentPrompt);
        Assert.Equal(InteractionZoneKind.LaunderingTerminal, coordinator.CurrentPrompt.Zone);
        Assert.Contains("gesperrt", coordinator.CurrentPrompt.Detail);
        Assert.False(coordinator.CurrentPrompt.CanInteract);
    }

    [Fact]
    public void Tick_WhenOwnedTerminal_ResolvesActivePrompt()
    {
        var purchase = new BusinessPurchaseManager();
        purchase.RestoreOwnership(true);
        var laundering = new BusinessLaunderingManager(purchase);
        var player = new MockPlayerPort { X = 56.4f, Y = 5.5f, Z = 30.25f };

        var coordinator = new TownHallInteractionCoordinator(
            purchase, laundering, player,
            () => true, () => null, UnexpectedLaundering);

        coordinator.Tick(0.1f);

        Assert.NotNull(coordinator.CurrentPrompt);
        Assert.Equal(InteractionZoneKind.LaunderingTerminal, coordinator.CurrentPrompt.Zone);
        Assert.Contains("Geldwäsche", coordinator.CurrentPrompt.Detail);
        Assert.True(coordinator.CurrentPrompt.CanInteract);
    }

    [Fact]
    public void Tick_WhenKeyPressedAtUnownedEntrance_DoesNotPurchaseDirectly()
    {
        var purchase = new BusinessPurchaseManager();
        var laundering = new BusinessLaunderingManager(purchase);
        var player = new MockPlayerPort
        {
            X = 54.0f,
            Y = 2.0f,
            Z = 30.5f,
            KeyPressed = true
        };
        var coordinator = new TownHallInteractionCoordinator(
            purchase, laundering, player,
            () => true, () => null, UnexpectedLaundering);

        coordinator.Tick(0.01f);

        Assert.False(purchase.IsOwned);
        Assert.Single(player.Notifications);
        Assert.Contains("Ray", player.Notifications[0].Message);
        Assert.True(coordinator.CooldownRemaining > 0f);
    }

    [Fact]
    public void Tick_WhenKeyPressedAtUnownedTerminal_ShowsDenialNotification()
    {
        var purchase = new BusinessPurchaseManager();
        var laundering = new BusinessLaunderingManager(purchase);
        var player = new MockPlayerPort
        {
            X = 56.4f,
            Y = 5.5f,
            Z = 30.25f,
            KeyPressed = true
        };

        var coordinator = new TownHallInteractionCoordinator(
            purchase, laundering, player,
            () => true, () => null, UnexpectedLaundering);

        coordinator.Tick(0.01f);

        Assert.Single(player.Notifications);
        Assert.Equal("Zugriff verweigert", player.Notifications[0].Title);
        Assert.Contains("gesperrt", player.Notifications[0].Message);
    }

    [Theory]
    [InlineData("de-DE", "3.500")]
    [InlineData("en-US", "3,500")]
    public void Tick_WhenKeyPressedAtOwnedTerminal_WithCash_ExecutesLaundering(string cultureName, string formattedAmount)
    {
        var previousCulture = CultureInfo.CurrentCulture;
        try
        {
            CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo(cultureName);
            var purchase = new BusinessPurchaseManager();
            purchase.RestoreOwnership(true);
            var laundering = new BusinessLaunderingManager(purchase);
            var player = new MockPlayerPort
            {
                X = 56.4f,
                Y = 5.5f,
                Z = 30.25f,
                KeyPressed = true
            };
            var launderingPort = new MockLaunderingPort { PlayerCash = 3_500f };

            var coordinator = new TownHallInteractionCoordinator(
                purchase, laundering, player,
                () => true, () => launderingPort,
                amount => laundering.TryStartLaundering(CreateContext(), launderingPort, amount));

            coordinator.Tick(0.01f);

            Assert.Single(player.Notifications);
            Assert.Equal("Geldwäsche eingeleitet", player.Notifications[0].Title);
            Assert.Contains($"+${formattedAmount} Schwarzgeld", player.Notifications[0].Message);
            Assert.Equal(3_500f, launderingPort.CurrentTotal);
            Assert.Equal(0f, launderingPort.PlayerCash);
        }
        finally
        {
            CultureInfo.CurrentCulture = previousCulture;
        }
    }

    [Fact]
    public void Tick_CooldownPreventsImmediateRepeatInteraction()
    {
        var purchase = new BusinessPurchaseManager();
        var laundering = new BusinessLaunderingManager(purchase);
        var player = new MockPlayerPort
        {
            X = 56.4f,
            Y = 5.5f,
            Z = 30.25f,
            KeyPressed = true
        };

        var coordinator = new TownHallInteractionCoordinator(
            purchase, laundering, player,
            () => true, () => null, UnexpectedLaundering);

        // First press triggers denial
        coordinator.Tick(0.01f);
        Assert.Single(player.Notifications);

        // Immediate second frame while key is still down
        coordinator.Tick(0.01f);
        Assert.Single(player.Notifications); // Still 1 notification due to cooldown!

        // Advance past cooldown
        coordinator.Tick(TownHallInteractionCoordinator.CooldownDuration + 0.1f);
        Assert.Equal(2, player.Notifications.Count); // Now fired again
    }

    [Fact]
    public void Tick_SlotGuard_SuppressesInteractionsInDominikSlot1()
    {
        var purchase = new BusinessPurchaseManager();
        var laundering = new BusinessLaunderingManager(purchase);
        var player = new MockPlayerPort
        {
            X = 54.0f,
            Y = 2.0f,
            Z = 30.5f,
            KeyPressed = true
        };

        var coordinator = new TownHallInteractionCoordinator(
            purchase, laundering, player,
            () => ConstructionGate.Check(CreateContext(org: "Dominik", slot: "SaveGame_1")) == null,
            () => null, UnexpectedLaundering);

        coordinator.Tick(0.01f);

        Assert.Empty(player.Notifications);
    }
}
