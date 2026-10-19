using TaxiDriver;
using Xunit;

namespace TaxiDriver.Tests;

/// <summary>
/// Brief §8.1 / §10.1: a single failed navigation run is not automatically the end of the
/// operation — exactly ONE bounded recovery decision, for a running ride AND for a taxi-app
/// pickup on a valid operation — while a current Complete asks the ride to confirm a halt and
/// an unexpected Stopped is a distinct, explicit failure (never a universal restart). A Stopped
/// produced by our OWN controlled transition is ignored (brief §7.4/§8.1).
/// </summary>
public class NavigationCallbackOutcomePolicyTests
{
    [Fact]
    public void NotAnAcceptedCurrentResult_DoesNothing()
    {
        Assert.Equal(NavigationOutcomeAction.Ignore,
            NavigationCallbackOutcomePolicy.Decide("Failed", navToPlayer: false, operationValid: false, controlledStop: false));
    }

    [Fact]
    public void CurrentRecoverableFailure_HandsOneBoundedRecoveryInsteadOfEndingTheRide()
    {
        Assert.Equal(NavigationOutcomeAction.BoundedRecovery,
            NavigationCallbackOutcomePolicy.Decide("Failed", navToPlayer: false, operationValid: true, controlledStop: false));
        // case-insensitive: the native enum text is not trusted for casing
        Assert.Equal(NavigationOutcomeAction.BoundedRecovery,
            NavigationCallbackOutcomePolicy.Decide("failed", navToPlayer: false, operationValid: true, controlledStop: false));
    }

    [Fact]
    public void PickupFailure_OnAValidOperation_IsAlsoBounded()
    {
        // Review D13: a pickup Failed on an active valid operation is bounded too — it is not a
        // silent terminal exception.
        Assert.Equal(NavigationOutcomeAction.BoundedRecovery,
            NavigationCallbackOutcomePolicy.Decide("Failed", navToPlayer: true, operationValid: true, controlledStop: false));
    }

    [Fact]
    public void RideComplete_ConfirmsArrivalInsteadOfClaimingSuccess()
    {
        // Review D13: a current Complete for a ride asks the ride to confirm a halt — it must not
        // be treated as an already-published success while the taxi may still be moving.
        Assert.Equal(NavigationOutcomeAction.ConfirmArrival,
            NavigationCallbackOutcomePolicy.Decide("Complete", navToPlayer: false, operationValid: true, controlledStop: false));
    }

    [Fact]
    public void PickupComplete_KeepsItsOwnTerminalHandling()
    {
        Assert.Equal(NavigationOutcomeAction.Finalize,
            NavigationCallbackOutcomePolicy.Decide("Complete", navToPlayer: true, operationValid: true, controlledStop: false));
    }

    [Fact]
    public void UnexpectedStopped_IsADistinctExplicitFailure()
    {
        Assert.Equal(NavigationOutcomeAction.ExplicitFailure,
            NavigationCallbackOutcomePolicy.Decide("Stopped", navToPlayer: false, operationValid: true, controlledStop: false));
    }

    [Fact]
    public void ControlledStop_FromOurOwnTransition_IsIgnored()
    {
        // Review D16: the Stopped our own StopReversing/StopNavigating produces must never be
        // treated as an authoritative terminal (that would recurse stop -> callback -> stop).
        Assert.Equal(NavigationOutcomeAction.Ignore,
            NavigationCallbackOutcomePolicy.Decide("Stopped", navToPlayer: false, operationValid: true, controlledStop: true));
        Assert.Equal(NavigationOutcomeAction.Ignore,
            NavigationCallbackOutcomePolicy.Decide("Failed", navToPlayer: false, operationValid: true, controlledStop: true));
        Assert.Equal(NavigationOutcomeAction.Ignore,
            NavigationCallbackOutcomePolicy.Decide("Complete", navToPlayer: true, operationValid: true, controlledStop: true));
    }

    [Fact]
    public void UnknownOrNullResult_DoesNotPublishABoundedRecovery()
    {
        Assert.Equal(NavigationOutcomeAction.Finalize,
            NavigationCallbackOutcomePolicy.Decide(null, navToPlayer: false, operationValid: true, controlledStop: false));
    }
}
