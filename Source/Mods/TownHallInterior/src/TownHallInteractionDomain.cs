using System;

namespace TownHallInterior;

/// <summary>
/// Identification of interactive 3D spatial zones in and around the Town Hall.
/// </summary>
public enum InteractionZoneKind
{
    None,
    EntrancePurchase,
    LaunderingTerminal,
    WhiteboardPurchase
}

/// <summary>
/// Active interaction prompt to display on the player's HUD.
/// </summary>
public sealed record InteractionPromptInfo(
    InteractionZoneKind Zone,
    string Header,
    string Detail,
    bool CanInteract);

/// <summary>
/// Port abstraction for player spatial position, input detection, and HUD notifications.
/// </summary>
public interface ITownHallPlayerPort
{
    bool TryGetPlayerPosition(out float x, out float y, out float z);
    bool IsInteractKeyPressed();
    void ShowNotification(string title, string message, bool playSound = true);
}

/// <summary>
/// Immutable 3D spatial coordinate for domain zone evaluations without Unity dependencies.
/// </summary>
public sealed record SpatialPoint(float X, float Y, float Z);

/// <summary>
/// Coordinates player interaction zones (Entrance, Laundering Terminal, and Real Estate Whiteboard),
/// managing cooldowns, prompt resolution, and purchase/laundering command dispatch.
/// </summary>
public sealed class TownHallInteractionCoordinator
{
    private readonly BusinessPurchaseManager _purchaseManager;
    private readonly BusinessLaunderingManager _launderingManager;
    private readonly ITownHallPlayerPort _playerPort;
    private readonly Func<bool> _isAvailable;
    private readonly Func<ILaunderingPort?> _launderingPortProvider;
    private readonly Func<float, LaunderingOperationReceipt> _startLaundering;
    private readonly Func<SpatialPoint?>? _whiteboardPositionProvider;

    private float _cooldownTimer;
    public const float CooldownDuration = 0.8f;

    // Terminal coordinates: upper floor desk 0 (X=56.4f, Y=5.5f, Z=30.25f)
    public const float TerminalX = 56.4f;
    public const float TerminalY = 5.5f;
    public const float TerminalZ = 30.25f;
    public const float TerminalMaxRange = 2.4f;

    // Entrance coordinates: portal approach (X=54.0f, Y=2.0f, Z=30.5f)
    public const float EntranceX = 54.0f;
    public const float EntranceY = 2.0f;
    public const float EntranceZ = 30.5f;
    public const float EntranceMaxRange = 2.8f;

    // Whiteboard coordinates: dynamic in Ray's office
    public const float WhiteboardMaxRange = 3.2f;

    public InteractionPromptInfo? CurrentPrompt { get; private set; }
    public float CooldownRemaining => _cooldownTimer;

    public TownHallInteractionCoordinator(
        BusinessPurchaseManager purchaseManager,
        BusinessLaunderingManager launderingManager,
        ITownHallPlayerPort playerPort,
        Func<bool> isAvailable,
        Func<ILaunderingPort?> launderingPortProvider,
        Func<float, LaunderingOperationReceipt> startLaundering,
        Func<SpatialPoint?>? whiteboardPositionProvider = null)
    {
        _purchaseManager = purchaseManager ?? throw new ArgumentNullException(nameof(purchaseManager));
        _launderingManager = launderingManager ?? throw new ArgumentNullException(nameof(launderingManager));
        _playerPort = playerPort ?? throw new ArgumentNullException(nameof(playerPort));
        _isAvailable = isAvailable ?? throw new ArgumentNullException(nameof(isAvailable));
        _launderingPortProvider = launderingPortProvider ?? throw new ArgumentNullException(nameof(launderingPortProvider));
        _startLaundering = startLaundering ?? throw new ArgumentNullException(nameof(startLaundering));
        _whiteboardPositionProvider = whiteboardPositionProvider;
    }

    public static float DistanceSquared(float x1, float y1, float z1, float x2, float y2, float z2)
    {
        float dx = x1 - x2;
        float dy = y1 - y2;
        float dz = z1 - z2;
        return (dx * dx) + (dy * dy) + (dz * dz);
    }

    public InteractionZoneKind EvaluateZone(float playerX, float playerY, float playerZ)
    {
        // 1. Check Terminal (upper floor)
        // Upper floor is elevated (Y >= 4.0f), avoiding overlap with ground floor entrance
        if (playerY >= 4.0f)
        {
            float distSqTerminal = DistanceSquared(playerX, playerY, playerZ, TerminalX, TerminalY, TerminalZ);
            if (distSqTerminal <= TerminalMaxRange * TerminalMaxRange)
                return InteractionZoneKind.LaunderingTerminal;
        }

        // 2. Check Entrance (ground floor)
        if (playerY < 4.0f)
        {
            float distSqEntrance = DistanceSquared(playerX, playerY, playerZ, EntranceX, EntranceY, EntranceZ);
            if (distSqEntrance <= EntranceMaxRange * EntranceMaxRange)
                return InteractionZoneKind.EntrancePurchase;
        }

        // 3. Check Whiteboard (in Ray's Real Estate Office)
        if (_whiteboardPositionProvider != null)
        {
            var wb = _whiteboardPositionProvider();
            if (wb != null)
            {
                float distSqWb = DistanceSquared(playerX, playerY, playerZ, wb.X, wb.Y, wb.Z);
                if (distSqWb <= WhiteboardMaxRange * WhiteboardMaxRange)
                    return InteractionZoneKind.WhiteboardPurchase;
            }
        }

        return InteractionZoneKind.None;
    }

    public void Tick(float deltaSeconds)
    {
        if (deltaSeconds > 0f)
            _cooldownTimer = MathF.Max(0f, _cooldownTimer - deltaSeconds);

        if (!_playerPort.TryGetPlayerPosition(out float px, out float py, out float pz))
        {
            CurrentPrompt = null;
            return;
        }

        var zone = EvaluateZone(px, py, pz);
        CurrentPrompt = ResolvePrompt(zone);

        if (zone != InteractionZoneKind.None && _cooldownTimer <= 0f && _playerPort.IsInteractKeyPressed())
        {
            _cooldownTimer = CooldownDuration;
            ExecuteInteraction(zone);
        }
    }

    private InteractionPromptInfo? ResolvePrompt(InteractionZoneKind zone)
    {
        bool isOwned = _purchaseManager.IsOwned;

        return zone switch
        {
            InteractionZoneKind.EntrancePurchase => isOwned
                ? new InteractionPromptInfo(zone, "Rathaus Hyland", "Im Besitz · Verwaltungssitz", false)
                : new InteractionPromptInfo(zone, "Rathaus Hyland", "Kauf bei Makler Ray ($250.000)", false),

            InteractionZoneKind.WhiteboardPurchase => isOwned
                ? new InteractionPromptInfo(zone, "Rathaus Hyland", "Im Besitz · Verkauft", false)
                : new InteractionPromptInfo(zone, "Rathaus Hyland", "Kauf bei Makler Ray ($250.000)", false),

            InteractionZoneKind.LaunderingTerminal => isOwned
                ? new InteractionPromptInfo(
                    zone,
                    "Verwaltungs-Terminal",
                    _launderingManager.PendingAmount > 0f
                        ? $"[E] Geldwäsche einleiten (${_launderingManager.PendingAmount:N0} aktiv · {_launderingManager.RemainingMinutes} Spielmin.)"
                        : "[E] Geldwäsche einleiten",
                    true)
                : new InteractionPromptInfo(zone, "Verwaltungs-Terminal", "Zugriff gesperrt · Nur für Eigentümer", false),

            _ => null
        };
    }

    private void ExecuteInteraction(InteractionZoneKind zone)
    {
        if (!_isAvailable()) return;

        switch (zone)
        {
            case InteractionZoneKind.EntrancePurchase:
            case InteractionZoneKind.WhiteboardPurchase:
                // Direct [E] purchase is completely disabled: real estate purchases now
                // happen exclusively via Ray's native estate agent dialogue menu.
                if (!_purchaseManager.IsOwned)
                {
                    _playerPort.ShowNotification(
                        "Rathaus Hyland",
                        "Gewerbeimmobilie kann bei Makler Ray im Maklerbüro erworben werden ($250.000).",
                        playSound: false);
                }
                break;
            case InteractionZoneKind.LaunderingTerminal:
                HandleTerminalInteraction();
                break;
        }
    }

    private void HandleTerminalInteraction()
    {
        if (!_purchaseManager.IsOwned)
        {
            _playerPort.ShowNotification(
                "Zugriff verweigert",
                "Terminal gesperrt. Dieses System steht nur dem autorisierten Eigentümer des Rathauses zur Verfügung.",
                playSound: false);
            return;
        }

        var port = _launderingPortProvider();
        if (port == null)
        {
            _playerPort.ShowNotification("Geldwäsche-Terminal", "Fehler: Geldwäsche-Schnittstelle nicht bereit.", playSound: false);
            return;
        }

        float availableCap = _launderingManager.GetAvailableCapacity(port);
        if (availableCap <= 0f)
        {
            _playerPort.ShowNotification(
                "Geldwäsche-Terminal",
                "Die heutige Geldwäsche-Kapazität von $20.000 ist vollständig erschöpft.",
                playSound: false);
            return;
        }

        float playerCash = port.GetPlayerCash();
        if (playerCash <= 0f)
        {
            _playerPort.ShowNotification(
                "Geldwäsche-Terminal",
                $"Kein Schwarzgeld im Inventar. Bereit für bis zu ${availableCap:N0}.",
                playSound: false);
            return;
        }

        float depositAmount = MathF.Min(playerCash, availableCap);
        var receipt = _startLaundering(depositAmount);

        if (receipt.Success)
        {
            float remCap = _launderingManager.GetAvailableCapacity(port);
            _playerPort.ShowNotification(
                "Geldwäsche eingeleitet",
                $"+${receipt.Amount:N0} Schwarzgeld zur Säuberung übergeben. Verbleibende Tageskapazität: ${remCap:N0}.",
                playSound: true);
        }
        else
        {
            _playerPort.ShowNotification(
                "Geldwäsche fehlgeschlagen",
                $"Operation abgebrochen: {receipt.StatusMessage ?? "Unbekannter Fehler."}",
                playSound: false);
        }
    }
}
