extern alias il2cpp;

using System;
using System.IO;
using HarmonyLib;
using Il2CppInterop.Runtime;
using Il2CppScheduleOne.DevUtilities;
using Il2CppScheduleOne.Persistence;
using Il2CppScheduleOne.PlayerScripts;
using MelonLoader;
using S1API.Lifecycle;
using S1Mods.Shared;
using UnityEngine;
using NativeClock = Il2CppScheduleOne.GameTime.TimeManager;

[assembly: MelonInfo(typeof(TownHallInterior.Mod), "TownHallInterior", "0.1.0", "Dominik")]
[assembly: MelonGame("TVGS", "Schedule I")]

namespace TownHallInterior;

/// <summary>Experimental local hall; only a fully prepared, identity-matched facade is published.</summary>
public sealed class Mod : MelonMod
{
    public static Mod? Instance { get; private set; }
    public bool IsTownHallOwned => IsBusinessAllowedInCurrentWorld && _purchaseManager.IsOwned;
    public bool IsBusinessAllowedInCurrentWorld =>
        _saveSession != null && !_persistenceFailed && _businessData.UnresolvedPayment == null;

    private IRoomSink? _staged;
    private PublicHallHost? _hall;
    private readonly BusinessWorldRegistrationManager _worldRegistration = new();
    private readonly BusinessPurchaseManager _purchaseManager = new();
    private readonly BusinessLaunderingManager _launderingManager;
    private readonly TownHallInteractionCoordinator _interactionCoordinator;
    private TownHallBusinessData _businessData = new();
    private NativeWorldPort? _worldPort;
    private NativePurchasePaymentPort? _paymentPort;
    private NativeLaunderingPort? _launderingPort;
    private readonly NativeTownHallInteractionPort _playerPort;
    private readonly TownHallCabinetStorage _cabinets = new();
    private float _whiteboardRetryTimer = 2f;
    private TownHallSaveSession? _saveSession;
    private bool _persistenceFailed;
    private bool _savePatchInstalled;
    private bool _waitingForBusiness;
    private float _loadWaitRemaining;
    private IntPtr _requestedSaveManager;
    private IntPtr _acceptedSaveManager;
    private NativeClock? _clock;
    private int _clockBinding;
    private il2cpp::Il2CppSystem.Action? _minuteHandler;
    private il2cpp::Il2CppSystem.Action<int>? _timeSkipHandler;
    private int _queuedGameMinutes;
    private bool _settlementRequested;

    public Mod()
    {
        _launderingManager = new BusinessLaunderingManager(_purchaseManager);
        _playerPort = new NativeTownHallInteractionPort();
        _interactionCoordinator = new TownHallInteractionCoordinator(
            _purchaseManager,
            _launderingManager,
            _playerPort,
            isAvailable: () => GetTransactionContext() != null,
            launderingPortProvider: () => _launderingPort,
            startLaundering: TryStartLaundering,
            whiteboardPositionProvider: () =>
            {
                var pos = TownHallWhiteboardIntegration.WhiteboardPosition;
                return pos.HasValue ? new SpatialPoint(pos.Value.x, pos.Value.y, pos.Value.z) : null;
            });
    }

    public PurchaseReceipt TryPurchaseTownHall(PurchasePaymentMode mode = PurchasePaymentMode.Auto)
    {
        var context = GetTransactionContext();
        if (context == null)
            return new PurchaseReceipt("", 0f, mode, DateTime.UtcNow, false, "Rathausstand derzeit gesperrt");

        _paymentPort ??= new NativePurchasePaymentPort();
        var receipt = _purchaseManager.TryPurchase(context, _paymentPort, _worldPort!, _worldRegistration.Identity, mode);
        if (receipt.Success)
        {
            _businessData.IsOwned = true;
            _businessData.PurchasePrice = receipt.Amount;
            _businessData.PurchaseTimestampUtc = receipt.TimestampUtc.ToString("o");
            _businessData.PaymentMode = receipt.ResolvedMode.ToString();
            TownHallWhiteboardIntegration.SyncOwnership(true);
            TownHallLaunderingDeskIntegration.SyncOwnership(true);
        }
        else if (receipt.RequiresReconciliation)
        {
            HoldPayment(receipt.TransactionId, receipt.Amount, receipt.StatusMessage);
        }
        return receipt;
    }

    public LaunderingOperationReceipt TryStartLaundering(float amount)
    {
        var context = GetTransactionContext();
        if (context == null)
            return new LaunderingOperationReceipt("", amount, DateTime.UtcNow, false, "Rathausstand derzeit gesperrt");

        AdvanceLaunderingClock();
        var receipt = _launderingManager.TryStartLaundering(context, _launderingPort!, amount);
        if (receipt.Success)
        {
            _launderingManager.SyncStatsActiveOperations(_businessData);
            TownHallLaunderingDeskIntegration.SyncOperations(_launderingManager.GetActiveOperationsSnapshot());
        }
        else if (receipt.RequiresReconciliation)
        {
            HoldPayment(receipt.OperationId, receipt.Amount, receipt.StatusMessage);
        }
        return receipt;
    }

    private void HoldPayment(string id, float amount, string? reason)
    {
        _businessData.UnresolvedPayment = new UnresolvedTownHallPayment(id, amount, reason ?? "unknown-wallet-effect");
        TownHallLaunderingDeskIntegration.SyncOwnership(false);
        MelonLogger.Warning($"TownHallInterior payment held for reconciliation ({id}, {reason}).");
    }

    public override void OnInitializeMelon()
    {
        Instance = this;
        MelonLogger.Msg("TownHallInterior: local hall and checkpoint-bound business for TownHallTest / Slot 2.");
        DoorHook.Install();
        TownHallDialoguePatches.Install();
        TownHallLaunderingDeskIntegration.InstallPatches();
        GameLifecycle.OnPreLoad += ResetBusinessState;
        GameLifecycle.OnLoadComplete += OnGameLoadComplete;
        GameLifecycle.OnSaveStart += OnNativeSaveStart;
        GameLifecycle.OnSaveComplete += OnNativeSaveComplete;
#if TOWNHALL_BUSINESS_PROBE
        BusinessBindingProbe.Start();
#endif
#if TOWNHALL_BUSINESS_ACTIVATION
        BusinessActivationRuntime.Start(() => _hall != null && _staged is UnityRoomSink room &&
            S1Mods.Shared.NetworkGuard.IsAlive(room.Root) && room.Root!.gameObject.activeInHierarchy);
#elif TOWNHALL_BUSINESS_SUBTYPE
        BusinessSubtypeRuntime.Start(() => _hall != null && _staged is UnityRoomSink room &&
            S1Mods.Shared.NetworkGuard.IsAlive(room.Root) && room.Root!.gameObject.activeInHierarchy);
#elif TOWNHALL_BUSINESS_CONSTRUCTION
        BusinessConstructionRuntime.Start(() => _hall != null && _staged is UnityRoomSink room &&
            S1Mods.Shared.NetworkGuard.IsAlive(room.Root) && room.Root!.gameObject.activeInHierarchy);
#endif
    }

    public override void OnSceneWasLoaded(int buildIndex, string sceneName)
    {
#if TOWNHALL_BUSINESS_PROBE
        BusinessBindingProbe.SceneLoaded(sceneName);
#endif
        if (sceneName != "Main" || _staged != null) return;
        DoorHook.NextGeneration();
        try
        {
            _staged = RoomBuilder.Build(() => new UnityRoomSink());
            _hall = PublicHallHost.TryOpen(_staged);
            if (_hall == null)
                MelonLogger.Warning("TownHallInterior: hall stayed inactive; any pending native restore is logged separately.");

        }
        catch (Exception ex) { MelonLogger.Warning("TownHallInterior staging failed: " + ex); }
    }

    private void OnGameLoadComplete()
    {
        // f12 emits the callback before clearing IsLoading and setting IsGameLoaded.
        _waitingForBusiness = true;
        _loadWaitRemaining = 15f;
    }

    private bool TryOpenBusinessWorld()
    {
        var context = NativeWorldPort.CaptureCurrentContext(_hall != null);
        if (context?.LoadComplete != true) return false;
        if (ConstructionGate.Check(context) != null) return true;
        if (_saveSession != null) return true;

        var clock = NativeClock.Instance;
        var player = Player.Local;
        if (!NetworkGuard.IsAlive(clock) || !NetworkGuard.IsAlive(player) || player!.SaveFolderName != "Player_0" ||
            !TownHallNativeMoney.TryReadWallet(out float bank, out float cash))
            return false;
        EnsureNativeSaveHook();

        string savesDir = Path.Combine(MelonLoader.Utils.MelonEnvironment.UserDataDirectory, "TownHallInterior", "Saves");
        var session = TownHallSaveSession.Open(context, savesDir, player.SaveFolderName);
        if (session.NativeValues.Bank != bank || session.NativeValues.Cash != cash)
            throw new InvalidDataException("Geladenes Spielgeld stimmt nicht mit dem gespeicherten Rathaus-Checkpoint überein.");
        _businessData = session.Data;
        _purchaseManager.RestoreOwnership(_businessData.IsOwned);
        _worldPort = new NativeWorldPort();
        var regResult = _worldRegistration.Register(context, _worldPort, isOwned: _purchaseManager.IsOwned);
        MelonLogger.Msg($"TownHallInterior: world registration status={regResult.Status} (owned={_purchaseManager.IsOwned}, reason={regResult.Reason ?? "none"})");
        if (!_worldRegistration.IsRegistered)
            throw new InvalidOperationException("Local Town Hall registration failed.");

        _launderingPort = new NativeLaunderingPort(_worldPort);
        _launderingManager.RestoreActiveOperations(_businessData.ActiveOperations, _launderingPort);
        BindClock(clock!);
        _saveSession = session;
        HallGrid.Bind(session.Data.PlacedItems);
        HallGrid.TryRestore();
        _cabinets.Bind(session.Data.CabinetContents);
        _settlementRequested = true;

        if (IsBusinessAllowedInCurrentWorld)
        {
            TryInstallDeskStation();
            TownHallWhiteboardIntegration.TryInstallListing(_purchaseManager.IsOwned);
        }
        else
        {
            MelonLogger.Warning("TownHallInterior payment requires reconciliation. Rathaus transactions are locked.");
        }
        return true;
    }

    private void EnsureNativeSaveHook()
    {
        if (_savePatchInstalled) return;
        if (!TownHallNativeMoney.MatchesAuditedGameAssembly())
            throw new InvalidOperationException("Native wallet/save contract has not been checked for this game binary.");

        _savePatchInstalled = PatchGuard.TryPatch(HarmonyInstance, typeof(SaveManager), nameof(SaveManager.Save),
            prefix: new HarmonyMethod(typeof(Mod), nameof(BeforeNativeSave)),
            finalizer: new HarmonyMethod(typeof(Mod), nameof(AfterNativeSave)),
            parameterTypes: new[] { typeof(string) });
        if (!_savePatchInstalled)
            throw new InvalidOperationException("Native Save(string) hook unavailable.");
    }

    private void TryInstallDeskStation()
    {
        if (_staged is not UnityRoomSink room || !S1Mods.Shared.NetworkGuard.IsAlive(room.Root))
            return;

        TownHallLaunderingDeskIntegration.TryInstallStation(
            room.Root,
            _purchaseManager.IsOwned,
            _launderingManager.GetActiveOperationsSnapshot());
    }

    private ConstructionContext? GetTransactionContext()
    {
        if (!IsBusinessAllowedInCurrentWorld || _saveSession!.HasPendingSave ||
            _purchaseManager.IsMutating || _launderingManager.IsMutating)
            return null;
        return GetBoundContext(allowSaving: false);
    }

    private ConstructionContext? GetBoundContext(bool allowSaving)
    {
        if (_saveSession == null) return null;
        var context = NativeWorldPort.CaptureCurrentContext(_hall != null, allowSaving);
        if (ConstructionGate.Check(context) != null ||
            !SameSlot(context!.ActiveSavePath!, _saveSession.SlotPath))
            return null;
        return context;
    }

    private static bool SameSlot(string path, string slotPath) =>
        string.Equals(TownHallSaveSession.ResolveSlotPath(path), slotPath, StringComparison.OrdinalIgnoreCase);

    private static bool BeforeNativeSave(SaveManager __instance, string __0, out bool __state)
    {
        __state = false;
        var mod = Instance;
        try
        {
            if (mod?._saveSession == null || !SameSlot(__0, mod._saveSession.SlotPath)) return true;
            if (__instance.IsSaving || mod._requestedSaveManager != IntPtr.Zero || mod._saveSession.HasPendingSave)
                return false;
            if (mod._persistenceFailed || mod.GetBoundContext(allowSaving: false) == null)
                throw new InvalidOperationException("Rathaus-Lagerstand ist nicht sicher speicherbar.");
            if (mod._purchaseManager.IsMutating || mod._launderingManager.IsMutating ||
                !NetworkGuard.IsAlive(mod._clock) || !TownHallNativeMoney.TryReadWallet(out float bank, out float cash))
                throw new InvalidOperationException("Rathaus-Geld-, Zeit- oder Mutationsstand ist ungeklärt.");

            // Refuse before the native writer can persist an inventory deduction without its storage.
            HallGrid.CaptureContents();
            mod._cabinets.CaptureContents();
            mod.AdvanceLaunderingClock();
            mod._launderingManager.SyncStatsActiveOperations(mod._businessData);
            mod._saveSession.Prepare(new TownHallNativeValues(bank, cash, mod._clock!.CurrentTime, mod._clock.ElapsedDays));
            mod._requestedSaveManager = __instance.Pointer;
            __state = true;
            return true;
        }
        catch (Exception ex)
        {
            MelonLogger.Warning($"TownHallInterior: native save refused before writing to protect storage: {ex.Message}");
            return false;
        }
    }

    private static void AfterNativeSave(SaveManager __instance, bool __state)
    {
        var mod = Instance;
        if (!__state || mod == null) return;
        mod._requestedSaveManager = IntPtr.Zero;
        try
        {
            if (!mod._persistenceFailed && mod._saveSession?.HasPendingSave == true &&
                mod._acceptedSaveManager != __instance.Pointer)
                mod.BlockForSaveFailure(new InvalidOperationException("Native save started without the Rathaus save-start callback."));
        }
        catch (Exception ex)
        {
            mod._persistenceFailed = true;
            MelonLogger.Warning($"TownHallInterior native save observation failed: {ex.Message}");
        }
    }

    private void OnNativeSaveStart()
    {
        if (_saveSession == null || _persistenceFailed) return;
        try
        {
            var manager = PersistentSingleton<SaveManager>.Instance;
            if (_requestedSaveManager == IntPtr.Zero || manager == null || manager.Pointer != _requestedSaveManager ||
                !manager.IsSaving || GetBoundContext(allowSaving: true) == null ||
                !_saveSession.HasPendingSave)
            {
                throw new InvalidOperationException("Native save destination or prepared Rathaus snapshot is unresolved.");
            }

            _acceptedSaveManager = manager.Pointer;
        }
        catch (Exception ex) { BlockForSaveFailure(ex); }
    }

    private void OnNativeSaveComplete()
    {
        if (_saveSession?.HasPendingSave != true || _persistenceFailed) return;
        try
        {
            var manager = PersistentSingleton<SaveManager>.Instance;
            bool succeeded = manager != null && manager.Pointer == _acceptedSaveManager &&
                !manager.IsSaving && !SaveManager.SaveError && manager.QueuedSaveRequests?.Count == 0 &&
                GetBoundContext(allowSaving: false) != null;
            _saveSession.Complete(succeeded);
            _settlementRequested = true;
        }
        catch (Exception ex) { BlockForSaveFailure(ex); }
        finally { _acceptedSaveManager = IntPtr.Zero; }
    }

    private void BlockForSaveFailure(Exception error)
    {
        _persistenceFailed = true;
        TownHallWhiteboardIntegration.Teardown();
        TownHallLaunderingDeskIntegration.Teardown();
        MelonLogger.Warning($"TownHallInterior Rathausstand gesperrt: {error.Message}");
    }

    private void BindClock(NativeClock clock)
    {
        _clock = clock;
        int binding = ++_clockBinding;
        _minuteHandler = DelegateSupport.ConvertDelegate<il2cpp::Il2CppSystem.Action>((Action)(() => QueueMinutes(binding, 1)));
        _timeSkipHandler = DelegateSupport.ConvertDelegate<il2cpp::Il2CppSystem.Action<int>>(
            (Action<int>)(minutes => QueueMinutes(binding, minutes)));
        try
        {
            if (clock.onMinutePass == null || _minuteHandler == null || _timeSkipHandler == null)
                throw new InvalidOperationException("Native game-minute callbacks are unavailable.");
            clock.onMinutePass.Add(_minuteHandler);
            clock.onTimeSkip += _timeSkipHandler;
        }
        catch
        {
            UnbindClock();
            throw;
        }
    }

    private void UnbindClock()
    {
        var clock = _clock;
        _clock = null;
        _clockBinding++;
        if (NetworkGuard.IsAlive(clock))
        {
            try { if (_minuteHandler != null) clock!.onMinutePass.Remove(_minuteHandler); }
            catch (Exception ex) { MelonLogger.Warning($"TownHallInterior minute listener cleanup: {ex.Message}"); }
            try { if (_timeSkipHandler != null) clock!.onTimeSkip -= _timeSkipHandler; }
            catch (Exception ex) { MelonLogger.Warning($"TownHallInterior sleep listener cleanup: {ex.Message}"); }
        }
        _minuteHandler = null;
        _timeSkipHandler = null;
        _queuedGameMinutes = 0;
        _settlementRequested = false;
    }

    private void QueueMinutes(int binding, int minutes)
    {
        if (binding == _clockBinding && minutes > 0)
            _queuedGameMinutes = (int)Math.Min(int.MaxValue, (long)_queuedGameMinutes + minutes);
    }

    private void AdvanceLaunderingClock()
    {
        if (_queuedGameMinutes == 0) return;
        _launderingManager.AdvanceMinutes(_queuedGameMinutes);
        _queuedGameMinutes = 0;
        _settlementRequested = true;
    }

    private void TickBusiness()
    {
        if (!_settlementRequested && _queuedGameMinutes == 0) return;
        if (GetTransactionContext() == null) return;

        AdvanceLaunderingClock();
        _settlementRequested = false;
        if (_launderingManager.PendingAmount <= 0f) return;

        bool completed = _launderingManager.TrySettleDue(_launderingPort!, _businessData, out float amount);
        TownHallLaunderingDeskIntegration.SyncOperations(_launderingManager.GetActiveOperationsSnapshot());
        if (completed)
        {
            _playerPort.ShowNotification("Geldwäsche abgeschlossen",
                $"+${amount:N0} auf dem Bankkonto. Gesamt im Rathaus gewaschen: ${_businessData.TotalLaundered:N0}.");
        }
    }

    public override void OnUpdate()
    {
        float dt = UnityEngine.Time.deltaTime;
        _hall?.Tick(dt);
        if (_waitingForBusiness)
        {
            try
            {
                _loadWaitRemaining -= UnityEngine.Time.unscaledDeltaTime;
                _waitingForBusiness = !TryOpenBusinessWorld() && _loadWaitRemaining > 0f;
                if (!_waitingForBusiness && _loadWaitRemaining <= 0f && _saveSession == null)
                    MelonLogger.Warning("TownHallInterior load readiness timed out.");
            }
            catch (Exception ex)
            {
                _waitingForBusiness = false;
                BlockForSaveFailure(ex);
            }
        }
        bool coordinatorRan = IsBusinessAllowedInCurrentWorld && !TownHallLaunderingDeskIntegration.IsInstalled;
        if (coordinatorRan)
            _interactionCoordinator.Tick(dt);
        if (_hall != null)
            _cabinets.TryPlace();

        TickBusiness();

        if (IsBusinessAllowedInCurrentWorld &&
            (!TownHallWhiteboardIntegration.IsCardInstalled || !TownHallLaunderingDeskIntegration.IsInstalled))
        {
            _whiteboardRetryTimer -= dt;
            if (_whiteboardRetryTimer <= 0f)
            {
                _whiteboardRetryTimer = 3f;
                if (!TownHallWhiteboardIntegration.IsCardInstalled)
                    TownHallWhiteboardIntegration.TryInstallListing(_purchaseManager.IsOwned);
                if (!TownHallLaunderingDeskIntegration.IsInstalled)
                    TryInstallDeskStation();
            }
        }
#if TOWNHALL_BUSINESS_PROBE
        BusinessBindingProbe.Tick();
#endif
#if TOWNHALL_BUSINESS_ACTIVATION
        BusinessActivationRuntime.Tick();
#elif TOWNHALL_BUSINESS_SUBTYPE
        BusinessSubtypeRuntime.Tick();
#elif TOWNHALL_BUSINESS_CONSTRUCTION
        BusinessConstructionRuntime.Tick();
#endif
    }

    public override void OnGUI()
    {
        if (!IsBusinessAllowedInCurrentWorld || TownHallLaunderingDeskIntegration.IsInstalled)
            return;

        var prompt = _interactionCoordinator.CurrentPrompt;
        if (prompt == null || prompt.Zone != InteractionZoneKind.LaunderingTerminal) return;

        float width = 460f;
        float height = 54f;
        float x = (Screen.width - width) * 0.5f;
        float y = Screen.height - 130f;

        GUI.Box(new Rect(x, y, width, height), $"{prompt.Header}\n{prompt.Detail}");
    }

    public override void OnSceneWasUnloaded(int buildIndex, string sceneName)
    {
#if TOWNHALL_BUSINESS_ACTIVATION
        if (sceneName == "Main") BusinessActivationRuntime.SceneUnloaded();
#elif TOWNHALL_BUSINESS_SUBTYPE
        if (sceneName == "Main") BusinessSubtypeRuntime.Leaving();
#endif
#if TOWNHALL_BUSINESS_PROBE
        BusinessBindingProbe.SceneUnloaded(sceneName);
#endif
        if (sceneName == "Main") { DoorHook.NextGeneration(); Cleanup(); }
    }

    public override void OnDeinitializeMelon()
    {
#if TOWNHALL_BUSINESS_ACTIVATION
        BusinessActivationRuntime.Stop();
#elif TOWNHALL_BUSINESS_SUBTYPE
        BusinessSubtypeRuntime.Stop();
#elif TOWNHALL_BUSINESS_CONSTRUCTION
        BusinessConstructionRuntime.Stop();
#endif
#if TOWNHALL_BUSINESS_PROBE
        BusinessBindingProbe.Stop();
#endif
        GameLifecycle.OnPreLoad -= ResetBusinessState;
        GameLifecycle.OnLoadComplete -= OnGameLoadComplete;
        GameLifecycle.OnSaveStart -= OnNativeSaveStart;
        GameLifecycle.OnSaveComplete -= OnNativeSaveComplete;
        HarmonyInstance.UnpatchSelf();
        TownHallLaunderingDeskIntegration.UninstallPatches();
        TownHallDialoguePatches.Uninstall();
        DoorHook.Uninstall();
        Cleanup();
        Instance = null;
    }

    private void ResetBusinessState()
    {
        _cabinets.Release();
        _waitingForBusiness = false;
        _saveSession = null;
        _requestedSaveManager = _acceptedSaveManager = IntPtr.Zero;
        _persistenceFailed = false;
        UnbindClock();
        _businessData = new TownHallBusinessData();
        _purchaseManager.RestoreOwnership(false);
        _launderingManager.Reset();
        _launderingPort?.Reset();
        TownHallLaunderingDeskIntegration.Teardown();
        TownHallWhiteboardIntegration.Teardown();
        if (_worldPort != null)
        {
            try { _worldRegistration.Deregister(_worldPort); }
            catch (Exception ex) { MelonLogger.Warning("TownHallInterior world deregistration failed: " + ex); }
            _worldPort = null;
        }
        _worldRegistration.Reset();
        _launderingPort = null;
        _paymentPort = null;
    }

    private void Cleanup()
    {
        ResetBusinessState();
        var hall = _hall;
        var old = _staged;
        _hall = null;
        _staged = null;
        for (int attempt = 0; attempt < 2; attempt++)
        {
            try { hall?.Dispose(); }
            catch (Exception ex) { MelonLogger.Warning("TownHallInterior hall restore failed: " + ex); }
            if (hall == null || hall.CleanupComplete) break;
        }
        if (hall != null && !hall.CleanupComplete)
            MelonLogger.Warning("TownHallInterior: native restoration remains incomplete after two passes; inspect the pending-step log.");
        try { old?.Dispose(); }
        catch (Exception ex) { MelonLogger.Warning("TownHallInterior cleanup failed: " + ex); }
    }
}
