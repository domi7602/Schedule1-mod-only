extern alias il2cpp;

using System;
using System.Collections.Generic;
using System.Linq;
using AutoPackagingStation.Engine;
using AutoPackagingStation.Persistence;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.Attributes;
using Il2CppScheduleOne.DevUtilities;
using Il2CppScheduleOne.ItemFramework;
using Il2CppScheduleOne.ObjectScripts;
using Il2CppScheduleOne.PlayerScripts;
using Il2CppScheduleOne.UI.Stations;
using MelonLoader;
using S1Mods.Shared;
using UnityEngine;
using GameRegistry = Il2CppScheduleOne.Registry;
using NativeQualityItemInst = Il2CppScheduleOne.ItemFramework.QualityItemInstance;

namespace AutoPackagingStation.Entities;

public enum StationState
{
    Idle,
    Packaging,
    Complete,
    Blocked,
    NoPackaging
}

/// <summary>
/// IL2CPP-registered MonoBehaviour controlling a 4x4 Auto-Packaging Station entity.
/// Manages buffer slots, animated UV scrolling conveyor, pneumatic piston movement, LED beacons, and player interactions.
/// </summary>
[RegisterTypeInIl2Cpp]
public class AutoPackStationController : MonoBehaviour
{
    public AutoPackStationController(IntPtr ptr) : base(ptr) { }

    private string _stationGuid = string.Empty;
    public string StationGuid
    {
        get => _stationGuid;
        set => _stationGuid = value;
    }

    // Visual Component Bindings
    private Renderer? _conveyorRenderer;
    private Material? _conveyorMaterial;
    private Transform? _pistonTransform;
    private Renderer? _ledRenderer;
    private Material? _ledMaterial;
    private Light? _ledLight;
    private AudioSource? _audioSource;

    public AudioSource? AudioSource => _audioSource;

    // Canvas & Interaction State
    private bool _wasCanvasOpen = false;
    private float _ledPulseTimer = 0f;
    private bool _savedBeginButtonActive = false;
    private string _savedInstructionText = string.Empty;


    public void EnsureGuid()
    {
        var buildable = gameObject.GetComponent<Il2CppScheduleOne.EntityFramework.BuildableItem>() ?? gameObject.GetComponentInParent<Il2CppScheduleOne.EntityFramework.BuildableItem>();

        if (string.IsNullOrEmpty(_stationGuid) || _stationGuid == "00000000-0000-0000-0000-000000000000")
        {
            if (buildable != null && buildable.Pointer != IntPtr.Zero)
            {
                string bGuid = buildable.GUID.ToString();
                if (!string.IsNullOrEmpty(bGuid) && bGuid != "00000000-0000-0000-0000-000000000000")
                {
                    _stationGuid = bGuid;
                }
            }
        }

        if (string.IsNullOrEmpty(_stationGuid) || _stationGuid == "00000000-0000-0000-0000-000000000000")
        {
            _stationGuid = System.Guid.NewGuid().ToString();
        }

        if (buildable != null && buildable.Pointer != IntPtr.Zero)
        {
            try
            {
                string bGuid = buildable.GUID.ToString();
                if (string.IsNullOrEmpty(bGuid) || bGuid == "00000000-0000-0000-0000-000000000000")
                {
                    buildable.GUID = new il2cpp::Il2CppSystem.Guid(_stationGuid);
                }
            }
            catch (Exception ex)
            {
                Mod.Log.Debug($"EnsureGuid assign to BuildableItem failed: {ex.Message}");
            }
        }
    }

    private void Awake()
    {
        EnsureGuid();
        HideBaseRenderers();

        // Setup 3D Spatial Audio Source
        _audioSource = gameObject.GetComponent<AudioSource>() ?? gameObject.AddComponent(Il2CppType.Of<AudioSource>()).Cast<AudioSource>();
        _audioSource.playOnAwake = false;
        _audioSource.spatialBlend = 1.0f; // Full 3D audio
        _audioSource.minDistance = 2.0f;
        _audioSource.maxDistance = 18.0f;
        _audioSource.rolloffMode = AudioRolloffMode.Linear;
    }

    private void Start()
    {
        EnsureGuid();
        HideBaseRenderers();
        AutoPackStore.RegisterStation(this);
        RestoreNativeSlots();
        UpdateLedVisuals();
    }

    /// <summary>
    /// Managed-only call path (Start, ItemFactory, AutoPackStore) — never invoked from IL2CPP.
    /// HideFromIl2Cpp keeps AutoPackStationSaveData (a plain C# type) out of the Il2CppInterop
    /// native-slot dump, which otherwise warns about the unsupported parameter at every startup.
    /// </summary>
    [HideFromIl2Cpp]
    public void RestoreNativeSlots(AutoPackStationSaveData? data = null)
    {
        try
        {
            var station = GetComponent<PackagingStation>() ?? GetComponentInParent<PackagingStation>();
            if (station == null || station.Pointer == IntPtr.Zero) return;

            var inputSlots = station.InputSlots;
            var outputSlots = station.OutputSlots;
            if (inputSlots == null || inputSlots.Count < 2 || outputSlots == null || outputSlots.Count < 1) return;

            var rData = AutoPackStore.GetRuntimeData(_stationGuid);
            var pkgData = data?.InputPackaging ?? rData.InputPackaging;
            var prodData = data?.InputProduct ?? rData.InputProduct;
            var outData = data?.OutputProduct ?? rData.OutputProduct;

            var pkgSlot = inputSlots[0];
            var prodSlot = inputSlots[1];
            var outSlot = outputSlots[0];

            // 1. Input Packaging — clear if data empty to avoid ghost items (H3)
            if (pkgSlot != null && pkgSlot.Pointer != IntPtr.Zero)
            {
                if (pkgData != null && pkgData.Quantity > 0 && !string.IsNullOrEmpty(pkgData.ItemId))
                {
                    var pkgDef = GameRegistry.GetItem(pkgData.ItemId);
                    if (pkgDef != null && pkgDef.Pointer != IntPtr.Zero)
                    {
                        var inst = pkgDef.GetDefaultInstance(pkgData.Quantity);
                        if (inst != null && inst.Pointer != IntPtr.Zero)
                        {
                            pkgSlot.SetStoredItem(inst);
                            pkgSlot.onItemDataChanged?.Invoke();
                            pkgSlot.onItemInstanceChanged?.Invoke();
                            // Duplication fix: native restore succeeded — drop the rData copy so stale runtime data can't pay out twice
                            rData.InputPackaging = null;
                        }
                    }
                }
                else
                {
                    pkgSlot.ClearStoredInstance();
                    pkgSlot.onItemDataChanged?.Invoke();
                    pkgSlot.onItemInstanceChanged?.Invoke();
                }
            }

            // 2. Input Product
            if (prodSlot != null && prodSlot.Pointer != IntPtr.Zero)
            {
                if (prodData != null && prodData.Quantity > 0 && !string.IsNullOrEmpty(prodData.ItemId))
                {
                    var prodDef = GameRegistry.GetItem(prodData.ItemId);
                    if (prodDef != null && prodDef.Pointer != IntPtr.Zero)
                    {
                        var inst = prodDef.GetDefaultInstance(prodData.Quantity);
                        if (inst != null && inst.Pointer != IntPtr.Zero)
                        {
                            var qInst = inst.TryCast<NativeQualityItemInst>();
                            if (qInst != null && qInst.Pointer != IntPtr.Zero)
                            {
                                qInst.Quality = (EQuality)prodData.QualityTier;
                            }
                            prodSlot.SetStoredItem(inst);
                            prodSlot.onItemDataChanged?.Invoke();
                            prodSlot.onItemInstanceChanged?.Invoke();
                            // Duplication fix: native restore succeeded — drop the rData copy so stale runtime data can't pay out twice
                            rData.InputProduct = null;
                        }
                    }
                }
                else
                {
                    prodSlot.ClearStoredInstance();
                    prodSlot.onItemDataChanged?.Invoke();
                    prodSlot.onItemInstanceChanged?.Invoke();
                }
            }

            // 3. Output Product
            if (outSlot != null && outSlot.Pointer != IntPtr.Zero)
            {
                if (outData != null && outData.Quantity > 0 && !string.IsNullOrEmpty(outData.ItemId))
                {
                    var outDef = GameRegistry.GetItem(outData.ItemId);
                    if (outDef != null && outDef.Pointer != IntPtr.Zero)
                    {
                        var inst = outDef.GetDefaultInstance(outData.Quantity);
                        if (inst != null && inst.Pointer != IntPtr.Zero)
                        {
                            var qInst = inst.TryCast<NativeQualityItemInst>();
                            if (qInst != null && qInst.Pointer != IntPtr.Zero)
                            {
                                qInst.Quality = (EQuality)outData.QualityTier;
                            }
                            var pInst = inst.TryCast<Il2CppScheduleOne.Product.ProductItemInstance>();
                            if (pInst != null && pInst.Pointer != IntPtr.Zero)
                            {
                                if (!string.IsNullOrEmpty(outData.PackagingId))
                                {
                                    var appliedPkg = GameRegistry.GetItem(outData.PackagingId)?.TryCast<Il2CppScheduleOne.Product.Packaging.PackagingDefinition>();
                                    if (appliedPkg != null && appliedPkg.Pointer != IntPtr.Zero)
                                    {
                                        pInst.SetPackaging(appliedPkg);
                                    }
                                    else
                                    {
                                        pInst.PackagingID = outData.PackagingId;
                                    }
                                }
                            }
                            outSlot.SetStoredItem(inst);
                            outSlot.onItemDataChanged?.Invoke();
                            outSlot.onItemInstanceChanged?.Invoke();
                            // Duplication fix: native restore succeeded — drop the rData copy so stale runtime data can't pay out twice
                            rData.OutputProduct = null;
                        }
                    }
                }
                else
                {
                    outSlot.ClearStoredInstance();
                    outSlot.onItemDataChanged?.Invoke();
                    outSlot.onItemInstanceChanged?.Invoke();
                }
            }

            try
            {
                station.UpdatePackagingVisuals();
                station.UpdateProductVisuals();
            }
            catch { }
        }
        catch (Exception ex)
        {
            Mod.Log.Warn($"RestoreNativeSlots failed for [{_stationGuid}]: {ex.Message}");
        }
    }

    public void HideBaseRenderers()
    {
        try
        {
            var station = GetComponent<PackagingStation>() ?? GetComponentInParent<PackagingStation>();
            if (station != null && station.Pointer != IntPtr.Zero)
            {
                if (station.PackagingSlotPosition != null && station.PackagingSlotPosition.Pointer != IntPtr.Zero)
                {
                    station.PackagingSlotPosition.localScale = Vector3.zero;
                    station.PackagingSlotPosition.localPosition = new Vector3(0, -0.5f, 0);
                }
                if (station.ProductSlotPosition != null && station.ProductSlotPosition.Pointer != IntPtr.Zero)
                {
                    station.ProductSlotPosition.localScale = Vector3.zero;
                    station.ProductSlotPosition.localPosition = new Vector3(0, -0.5f, 0);
                }
                if (station.OutputSlotPosition != null && station.OutputSlotPosition.Pointer != IntPtr.Zero)
                {
                    station.OutputSlotPosition.localScale = Vector3.zero;
                    station.OutputSlotPosition.localPosition = new Vector3(0, -0.5f, 0);
                }
            }

            var lods = gameObject.GetComponentsInChildren<LODGroup>(true);
            if (lods != null)
            {
                for (int i = 0; i < lods.Length; i++)
                {
                    if (lods[i] != null && lods[i].Pointer != IntPtr.Zero) lods[i].enabled = false;
                }
            }

            var renderers = gameObject.GetComponentsInChildren<Renderer>(true);
            if (renderers == null) return;
            var visualRoot = transform.Find("AutoPack_VisualRoot");
            for (int i = 0; i < renderers.Length; i++)
            {
                var r = renderers[i];
                if (r == null || r.Pointer == IntPtr.Zero) continue;

                // Wenn der Renderer Teil unserer Mod-Visuals ist -> anlassen
                if (visualRoot != null && visualRoot.Pointer != IntPtr.Zero && r.transform.IsChildOf(visualRoot))
                    continue;
                if (r.gameObject.name.StartsWith("AutoPack"))
                    continue;
                if (r == _conveyorRenderer || r == _ledRenderer)
                    continue;

                // Item-Visualizer / dynamische Produkte der Slots verschonen (wenn child von ProductSlotPosition etc.)
                if (r.gameObject.name.Contains("ProductVisual") || r.gameObject.name.Contains("PackagingVisual") || r.gameObject.name.Contains("SlotPosition"))
                    continue;

                // Vanilla Base-Renderer deaktivieren (NICHT SetActive(false) auf GameObject, um Collider/Slots nicht zu killen!)
                r.enabled = false;
                r.forceRenderingOff = true;
            }
        }
        catch (Exception ex)
        {
            Mod.Log.Warn($"HideBaseRenderers failed: {ex.Message}");
        }
    }

    private void OnDestroy()
    {
        // Cleanup instanced materials to prevent leak (H12)
        try
        {
            if (_conveyorMaterial != null && _conveyorMaterial.Pointer != IntPtr.Zero) UnityEngine.Object.Destroy(_conveyorMaterial);
            if (_ledMaterial != null && _ledMaterial.Pointer != IntPtr.Zero) UnityEngine.Object.Destroy(_ledMaterial);
        }
        catch { }
        _conveyorMaterial = null;
        _ledMaterial = null;

        try
        {
            if (!SceneGate.IsChangingScenes)
            {
                var inv = PlayerInventory.Instance;
                var rData = AutoPackStore.GetRuntimeData(_stationGuid);
                var station = GetComponent<PackagingStation>() ?? GetComponentInParent<PackagingStation>();

                if (inv != null && inv.Pointer != IntPtr.Zero)
                {
                    var itemsToRefund = new List<Il2CppScheduleOne.ItemFramework.ItemInstance>();
                    bool hadNativePackaging = false;
                    bool hadNativeProduct = false;
                    bool hadNativeOutput = false;

                    // 1. Native slots refund — authoritative if station exists (H2 fix: avoid double refund with rData)
                    if (station != null && station.Pointer != IntPtr.Zero)
                    {
                        if (station.InputSlots != null)
                        {
                            for (int s = 0; s < station.InputSlots.Count; s++)
                            {
                                var slot = station.InputSlots[s];
                                if (slot != null && slot.Pointer != IntPtr.Zero && slot.ItemInstance != null && slot.ItemInstance.Pointer != IntPtr.Zero && slot.Quantity > 0 && slot.ItemInstance.Definition != null)
                                {
                                    int qty = slot.Quantity;
                                    var def = slot.ItemInstance.Definition;
                                    var qInst = slot.ItemInstance.TryCast<NativeQualityItemInst>();
                                    var qual = qInst != null && qInst.Pointer != IntPtr.Zero ? qInst.Quality : EQuality.Standard;

                                    for (int i = 0; i < qty; i++)
                                    {
                                        var inst = def.GetDefaultInstance(1);
                                        if (inst != null && inst.Pointer != IntPtr.Zero)
                                        {
                                            var q = inst.TryCast<NativeQualityItemInst>();
                                            if (q != null && q.Pointer != IntPtr.Zero)
                                            {
                                                q.Quality = qual;
                                            }
                                            itemsToRefund.Add(inst);
                                        }
                                    }
                                    if (s == 0) hadNativePackaging = true;
                                    else if (s == 1) hadNativeProduct = true;
                                    slot.ClearStoredInstance();
                                }
                            }
                        }

                        if (station.OutputSlots != null)
                        {
                            for (int s = 0; s < station.OutputSlots.Count; s++)
                            {
                                var slot = station.OutputSlots[s];
                                if (slot != null && slot.Pointer != IntPtr.Zero && slot.ItemInstance != null && slot.ItemInstance.Pointer != IntPtr.Zero && slot.Quantity > 0 && slot.ItemInstance.Definition != null)
                                {
                                    int qty = slot.Quantity;
                                    var inst = slot.ItemInstance;
                                    var def = inst.Definition;
                                    var prodInst = inst.TryCast<Il2CppScheduleOne.Product.ProductItemInstance>();
                                    var pkgDef = prodInst?.AppliedPackaging ?? (!string.IsNullOrEmpty(prodInst?.PackagingID) ? GameRegistry.GetItem(prodInst.PackagingID)?.TryCast<Il2CppScheduleOne.Product.Packaging.PackagingDefinition>() : null);
                                    var qual = inst.TryCast<NativeQualityItemInst>()?.Quality ?? EQuality.Standard;

                                    for (int i = 0; i < qty; i++)
                                    {
                                        var newInst = def.GetDefaultInstance(1);
                                        if (newInst != null && newInst.Pointer != IntPtr.Zero)
                                        {
                                            var p = newInst.TryCast<Il2CppScheduleOne.Product.ProductItemInstance>();
                                            if (p != null && p.Pointer != IntPtr.Zero)
                                            {
                                                if (pkgDef != null && pkgDef.Pointer != IntPtr.Zero)
                                                {
                                                    p.SetPackaging(pkgDef);
                                                }
                                                else if (!string.IsNullOrEmpty(prodInst?.PackagingID))
                                                {
                                                    p.PackagingID = prodInst.PackagingID;
                                                }
                                            }
                                            var q = newInst.TryCast<NativeQualityItemInst>();
                                            if (q != null && q.Pointer != IntPtr.Zero)
                                            {
                                                q.Quality = qual;
                                            }
                                            itemsToRefund.Add(newInst);
                                        }
                                    }
                                    hadNativeOutput = true;
                                    slot.ClearStoredInstance();
                                }
                            }
                        }
                    }

                    // 2. Fallback runtime data — only if no native item of that type was refunded (prevents double H2)
                    // Duplication fix: with a LIVE native station the slots above are the single source of truth —
                    // rData can be stale (already consumed via the vanilla UI) and must never pay out a second refund.
                    bool hasLiveNativeStation = (station != null && station.Pointer != IntPtr.Zero);
                    if (!hasLiveNativeStation && !hadNativeProduct && rData.InputProduct != null && rData.InputProduct.Quantity > 0)
                    {
                        var pDef = GameRegistry.GetItem(rData.InputProduct.ItemId);
                        if (pDef != null && pDef.Pointer != IntPtr.Zero)
                        {
                            for (int i = 0; i < rData.InputProduct.Quantity; i++)
                            {
                                var inst = pDef.GetDefaultInstance(1);
                                if (inst != null && inst.Pointer != IntPtr.Zero)
                                {
                                    var qInst = inst.TryCast<NativeQualityItemInst>();
                                    if (qInst != null && qInst.Pointer != IntPtr.Zero)
                                    {
                                        qInst.Quality = (EQuality)rData.InputProduct.QualityTier;
                                    }
                                    itemsToRefund.Add(inst);
                                }
                            }
                        }
                    }

                    if (!hasLiveNativeStation && !hadNativePackaging && rData.InputPackaging != null && rData.InputPackaging.Quantity > 0)
                    {
                        var pkgDef = GameRegistry.GetItem(rData.InputPackaging.ItemId);
                        if (pkgDef != null && pkgDef.Pointer != IntPtr.Zero)
                        {
                            for (int i = 0; i < rData.InputPackaging.Quantity; i++)
                            {
                                var inst = pkgDef.GetDefaultInstance(1);
                                if (inst != null && inst.Pointer != IntPtr.Zero)
                                {
                                    itemsToRefund.Add(inst);
                                }
                            }
                        }
                    }

                    if (!hasLiveNativeStation && !hadNativeOutput && rData.OutputProduct != null && rData.OutputProduct.Quantity > 0)
                    {
                        var outDef = GameRegistry.GetItem(rData.OutputProduct.ItemId);
                        if (outDef != null && outDef.Pointer != IntPtr.Zero)
                        {
                            var pkgDef = !string.IsNullOrEmpty(rData.OutputProduct.PackagingId)
                                ? GameRegistry.GetItem(rData.OutputProduct.PackagingId)?.TryCast<Il2CppScheduleOne.Product.Packaging.PackagingDefinition>()
                                : null;

                            for (int i = 0; i < rData.OutputProduct.Quantity; i++)
                            {
                                var inst = outDef.GetDefaultInstance(1);
                                if (inst != null && inst.Pointer != IntPtr.Zero)
                                {
                                    var pInst = inst.TryCast<Il2CppScheduleOne.Product.ProductItemInstance>();
                                    if (pInst != null && pInst.Pointer != IntPtr.Zero)
                                    {
                                        if (!string.IsNullOrEmpty(rData.OutputProduct.PackagingId))
                                        {
                                            pInst.PackagingID = rData.OutputProduct.PackagingId;
                                        }
                                        if (pkgDef != null && pkgDef.Pointer != IntPtr.Zero)
                                        {
                                            pInst.SetPackaging(pkgDef);
                                        }
                                    }
                                    var qInst = inst.TryCast<NativeQualityItemInst>();
                                    if (qInst != null && qInst.Pointer != IntPtr.Zero)
                                    {
                                        qInst.Quality = (EQuality)rData.OutputProduct.QualityTier;
                                    }
                                    itemsToRefund.Add(inst);
                                }
                            }
                        }
                    }

                    // Add items if inventory fits
                    for (int i = 0; i < itemsToRefund.Count; i++)
                    {
                        var item = itemsToRefund[i];
                        if (inv.CanItemFitInInventory(item, 1))
                        {
                            inv.AddItemToInventory(item);
                        }
                        else
                        {
                            Mod.Log.Warn($"Dismantle OnDestroy: Inventory full, could not return item '{item.Definition?.ID}'.");
                        }
                    }
                }

                rData.InputProduct = null;
                rData.InputPackaging = null;
                rData.OutputProduct = null;
                AutoPackStore.RemoveRuntimeData(_stationGuid);
            }
        }
        catch (Exception ex)
        {
            Mod.Log.Error($"OnDestroy dismantle refund error: {ex.Message}");
        }
        finally
        {
            AutoPackStore.UnregisterStation(this);
        }
    }

    public void RegisterConveyorVisual(Renderer renderer, Material material)
    {
        _conveyorRenderer = renderer;
        _conveyorMaterial = material;
    }

    public void RegisterPistonVisual(Transform pistonTransform)
    {
        _pistonTransform = pistonTransform;
    }

    public void RegisterLedVisual(Renderer renderer, Material material, Light light)
    {
        _ledRenderer = renderer;
        _ledMaterial = material;
        _ledLight = light;
        UpdateLedVisuals();
    }

    public void UpdateLedVisuals()
    {
        if (!Mod.CurrentConfig.ShowStatusLeds) return;
        if (_ledMaterial == null || _ledMaterial.Pointer == IntPtr.Zero) return;

        var rData = AutoPackStore.GetRuntimeData(_stationGuid);

        Color targetColor = rData.State switch
        {
            StationState.Idle => (rData.InputProduct != null) ? new Color(0.1f, 0.9f, 0.2f) : new Color(0.2f, 0.6f, 1.0f),
            StationState.Packaging => new Color(1.0f, 0.65f, 0.05f),
            StationState.Complete => new Color(0.0f, 1.0f, 0.5f),
            StationState.Blocked => new Color(0.95f, 0.15f, 0.1f),
            StationState.NoPackaging => new Color(0.95f, 0.4f, 0.1f),
            _ => Color.white
        };

        try
        {
            _ledMaterial.SetColor("_EmissionColor", targetColor * 2.5f);
            if (_ledLight != null && _ledLight.Pointer != IntPtr.Zero)
            {
                _ledLight.color = targetColor;
                _ledLight.intensity = 1.4f;
            }
        }
        catch { }
    }

    private void Update()
    {
        try
        {
            float dt = Time.deltaTime;
            var rData = AutoPackStore.GetRuntimeData(_stationGuid);
            var station = GetComponent<PackagingStation>() ?? GetComponentInParent<PackagingStation>();
            bool isNativeStation = (station != null && station.Pointer != IntPtr.Zero);

            bool canStart = isNativeStation
                ? AutoPackEngine.CanStationPackage(station!, out _, out _)
                : rData.CanStartPackaging();

            // 1. Process Packaging State Machine — host-authoritative only (H7)
            if (AutoPackEngine.IsHostOrSingleplayer())
            {
                if (rData.State == StationState.Packaging)
                {
                    AutoPackEngine.ProcessPackagingStep(this, dt);
                }
                else if (rData.State == StationState.Complete)
                {
                    if (canStart)
                    {
                        rData.State = StationState.Packaging;
                        UpdateLedVisuals();
                        AutoPackEngine.PlayCompressorStroke(_audioSource);
                    }
                    else
                    {
                        rData.State = StationState.Idle;
                        UpdateLedVisuals();
                    }
                }
                else if (rData.State == StationState.Idle || rData.State == StationState.Blocked || rData.State == StationState.NoPackaging)
                {
                    // Check if ready to auto-start packaging
                    if (canStart)
                    {
                        rData.State = StationState.Packaging;
                        UpdateLedVisuals();
                        AutoPackEngine.PlayCompressorStroke(_audioSource);
                    }
                }
            }

            // 2. Cursor, UI Override & Escape Handling — per-station guard + restore (H9)
            var canvas = PackagingStationCanvas.Instance;
            bool isCanvasOpen = canvas != null && canvas.Pointer != IntPtr.Zero && canvas.gameObject.activeSelf && isNativeStation && canvas.Station == station;

            if (isCanvasOpen && canvas != null)
            {
                if (!_wasCanvasOpen)
                {
                    // Save original state on first open frame
                    try
                    {
                        if (canvas.BeginButton != null && canvas.BeginButton.Pointer != IntPtr.Zero)
                            _savedBeginButtonActive = canvas.BeginButton.gameObject.activeSelf;
                        if (canvas.InstructionLabel != null && canvas.InstructionLabel.Pointer != IntPtr.Zero)
                            _savedInstructionText = canvas.InstructionLabel.text ?? string.Empty;
                    }
                    catch { }
                }

                // 1. Ensure Cursor is unlocked & visible inside the station UI
                if (Cursor.lockState != CursorLockMode.None)
                {
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                }

                // 2. Hide BeginButton and customize label for automation
                if (canvas.BeginButton != null && canvas.BeginButton.Pointer != IntPtr.Zero && canvas.BeginButton.gameObject.activeSelf)
                {
                    canvas.BeginButton.gameObject.SetActive(false);
                }
                if (canvas.InstructionLabel != null && canvas.InstructionLabel.Pointer != IntPtr.Zero && canvas.InstructionLabel.text != "AUTOMATED PACKING STATION - Feeds & packages in background")
                {
                    canvas.InstructionLabel.text = "AUTOMATED PACKING STATION - Feeds & packages in background";
                }

                // 3. Handle Escape key to close the station menu cleanly — guard typing & pause (H9/M12)
                bool isTyping = false;
                try { isTyping = S1Mods.Shared.HotkeyManager.IsInputFieldFocused(); } catch { }
                if (!isTyping && Input.GetKeyDown(KeyCode.Escape))
                {
                    try
                    {
                        canvas.Close();
                    }
                    catch
                    {
                        if (station != null && station.Pointer != IntPtr.Zero)
                        {
                            station.OnEndUse();
                        }
                    }
                    // Don't force-lock here — let close branch handle restore
                }
            }
            else if (_wasCanvasOpen)
            {
                // Canvas just closed: restore BeginButton/label and cursor only if no other UI needs it
                try
                {
                    if (canvas != null && canvas.Pointer != IntPtr.Zero)
                    {
                        if (canvas.BeginButton != null && canvas.BeginButton.Pointer != IntPtr.Zero)
                            canvas.BeginButton.gameObject.SetActive(_savedBeginButtonActive);
                        if (canvas.InstructionLabel != null && canvas.InstructionLabel.Pointer != IntPtr.Zero && !string.IsNullOrEmpty(_savedInstructionText))
                            canvas.InstructionLabel.text = _savedInstructionText;
                    }
                }
                catch { }
                // Only relock if no other station canvas or menu is keeping cursor unlocked
                bool shouldRelock = true;
                try
                {
                    var nextCanvas = PackagingStationCanvas.Instance;
                    if (nextCanvas != null && nextCanvas.Pointer != IntPtr.Zero && nextCanvas.gameObject.activeSelf)
                        shouldRelock = false;
                }
                catch { }
                if (shouldRelock)
                {
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                }
            }

            _wasCanvasOpen = isCanvasOpen;

            // 2b. Strict raycast-guarded interactions — PackUp via F (H1)
            if (!isCanvasOpen && AutoPackEngine.IsHostOrSingleplayer())
            {
                // Only when cursor locked (no UI) and not typing
                bool canInteract = false;
                try { canInteract = Cursor.lockState == CursorLockMode.Locked && !S1Mods.Shared.HotkeyManager.IsInputFieldFocused(); } catch { canInteract = Cursor.lockState == CursorLockMode.Locked; }
                if (canInteract)
                {
                    var cam = Camera.main;
                    if (cam != null && cam.Pointer != IntPtr.Zero)
                    {
                        float maxDist = Mathf.Clamp(Mod.CurrentConfig.InteractionRange, 1f, 5f);
                        float distToStation = Vector3.Distance(cam.transform.position, transform.position);
                        if (distToStation <= maxDist + 0.5f)
                        {
                            Ray ray = new Ray(cam.transform.position, cam.transform.forward);
                            if (Physics.Raycast(ray.origin, ray.direction, out RaycastHit hit, maxDist, Physics.DefaultRaycastLayers))
                            {
                                bool isHitThisStation = false;
                                try
                                {
                                    var hitGo = hit.collider != null && hit.collider.Pointer != IntPtr.Zero ? hit.collider.gameObject : null;
                                    if (hitGo != null && hitGo.Pointer != IntPtr.Zero)
                                    {
                                        if (hitGo == gameObject || hitGo.transform.IsChildOf(transform) || transform.IsChildOf(hitGo.transform))
                                            isHitThisStation = true;
                                        else
                                        {
                                            // Also check parent BuildableItem root
                                            var hitBuildable = hitGo.GetComponentInParent<Il2CppScheduleOne.EntityFramework.BuildableItem>();
                                            var myBuildable = gameObject.GetComponentInParent<Il2CppScheduleOne.EntityFramework.BuildableItem>();
                                            if (hitBuildable != null && hitBuildable.Pointer != IntPtr.Zero && myBuildable != null && myBuildable.Pointer != IntPtr.Zero && hitBuildable.Pointer == myBuildable.Pointer)
                                                isHitThisStation = true;
                                        }
                                    }
                                }
                                catch { }

                                if (isHitThisStation)
                                {
                                    // PackUp via F (strict)
                                    if (Input.GetKeyDown(KeyCode.F))
                                    {
                                        PackUpStation();
                                    }
                                }
                            }
                        }
                    }
                }
            }

            // 3. Animate LEDs and Idle Visuals
            _ledPulseTimer += dt * 3.5f;
            if (rData.State == StationState.Packaging)
            {
                UpdatePackagingLedPulse();
            }
        }
        catch (Exception ex)
        {
            Mod.Log.Debug($"AutoPackStationController Update error: {ex.Message}");
        }
    }



    public void AnimateCycle(float dt, float progress)
    {
        // 1. UV Scrolling Conveyor
        if (_conveyorMaterial != null && _conveyorMaterial.Pointer != IntPtr.Zero)
        {
            float speed = Mod.CurrentConfig.ConveyorScrollSpeed;
            Vector2 offset = _conveyorMaterial.mainTextureOffset;
            offset.y = (offset.y + dt * speed) % 1.0f;
            _conveyorMaterial.mainTextureOffset = offset;
        }

        // 2. Pneumatic Piston Reciprocating Stroke
        //    Tied to packaging progress (0→1 over PackagingDurationSeconds, default 2s):
        //      0.00 → 0.40  : Press down (ease-in, gravity acceleration)
        //      0.40 → 0.60  : Dwell at bottom (compressing the contents in the Kessel)
        //      0.60 → 1.00  : Retract up (ease-out, motor-controlled)
        if (_pistonTransform != null && _pistonTransform.Pointer != IntPtr.Zero)
        {
            const float PRESS_DOWN_END = 0.40f;
            const float DWELL_END = 0.60f;
            const float TOP_Y = 1.10f;
            const float BOTTOM_Y = 0.80f;

            float y;
            if (progress < PRESS_DOWN_END)
            {
                // Press down with ease-in (accelerate like gravity)
                float u = progress / PRESS_DOWN_END;
                y = Mathf.Lerp(TOP_Y, BOTTOM_Y, u * u);
            }
            else if (progress < DWELL_END)
            {
                // Dwell at the bottom — stamp rests inside the Kessel
                y = BOTTOM_Y;
            }
            else
            {
                // Retract with ease-out (motor-controlled)
                float u = Mathf.Clamp01((progress - DWELL_END) / (1.0f - DWELL_END));
                float eased = 1f - (1f - u) * (1f - u);
                y = Mathf.Lerp(BOTTOM_Y, TOP_Y, eased);
            }

            _pistonTransform.localPosition = new Vector3(0f, y, 0f);

            // Clank when entering the dwell (transition from press-down to dwell)
            bool inDwell = progress >= PRESS_DOWN_END && progress < DWELL_END;
            bool wasInDwell = _lastProgress >= PRESS_DOWN_END && _lastProgress < DWELL_END;
            if (inDwell && !wasInDwell)
            {
                AutoPackEngine.PlayMechanicalClank(_audioSource);
            }
            _lastProgress = progress;
        }
    }

    private float _lastProgress = -1f;

    private void UpdatePackagingLedPulse()
    {
        if (_ledLight != null && _ledLight.Pointer != IntPtr.Zero)
        {
            float pulse = 0.9f + Mathf.Sin(_ledPulseTimer) * 0.5f;
            _ledLight.intensity = pulse;
        }
    }



    public bool TryExtractInputProduct()
    {
        var rData = AutoPackStore.GetRuntimeData(_stationGuid);
        if (rData.InputProduct == null || rData.InputProduct.Quantity <= 0)
        {
            AudioHelper.PlayDenySound();
            return false;
        }

        var inv = PlayerInventory.Instance;
        if (inv == null || inv.Pointer == IntPtr.Zero) return false;

        try
        {
            var def = GameRegistry.GetItem(rData.InputProduct.ItemId);
            if (def == null || def.Pointer == IntPtr.Zero)
            {
                AudioHelper.PlayDenySound();
                return false;
            }

            int qty = rData.InputProduct.Quantity;
            var probe = def.GetDefaultInstance(1);
            if (probe == null || probe.Pointer == IntPtr.Zero)
            {
                AudioHelper.PlayDenySound();
                return false;
            }

            if (!inv.CanItemFitInInventory(probe, qty))
            {
                AudioHelper.PlayDenySound();
                return false;
            }

            var instances = new List<Il2CppScheduleOne.ItemFramework.ItemInstance>();
            instances.Add(probe);
            for (int i = 1; i < qty; i++)
            {
                var inst = def.GetDefaultInstance(1);
                if (inst != null && inst.Pointer != IntPtr.Zero)
                {
                    instances.Add(inst);
                }
            }

            for (int i = 0; i < instances.Count; i++)
            {
                var inst = instances[i];
                var qInst = inst.TryCast<NativeQualityItemInst>();
                if (qInst != null && qInst.Pointer != IntPtr.Zero)
                {
                    qInst.Quality = (EQuality)rData.InputProduct.QualityTier;
                }
                inv.AddItemToInventory(inst);
            }

            Mod.Log.Info($"Player extracted {instances.Count}x raw '{rData.InputProduct.ItemId}' from AutoPackagingStation.");
            rData.InputProduct = null;
            if (rData.State == StationState.Blocked || rData.State == StationState.NoPackaging)
            {
                rData.State = StationState.Idle;
            }
            UpdateLedVisuals();
            AudioHelper.PlayClickSound();
            return true;
        }
        catch (Exception ex)
        {
            Mod.Log.Error($"Failed to extract input product: {ex}");
            AudioHelper.PlayDenySound();
            return false;
        }
    }

    public bool TryExtractInputPackaging()
    {
        var rData = AutoPackStore.GetRuntimeData(_stationGuid);
        if (rData.InputPackaging == null || rData.InputPackaging.Quantity <= 0)
        {
            AudioHelper.PlayDenySound();
            return false;
        }

        var inv = PlayerInventory.Instance;
        if (inv == null || inv.Pointer == IntPtr.Zero) return false;

        try
        {
            var def = GameRegistry.GetItem(rData.InputPackaging.ItemId);
            if (def == null || def.Pointer == IntPtr.Zero)
            {
                AudioHelper.PlayDenySound();
                return false;
            }

            int qty = rData.InputPackaging.Quantity;
            var probe = def.GetDefaultInstance(1);
            if (probe == null || probe.Pointer == IntPtr.Zero)
            {
                AudioHelper.PlayDenySound();
                return false;
            }

            if (!inv.CanItemFitInInventory(probe, qty))
            {
                AudioHelper.PlayDenySound();
                return false;
            }

            var instances = new List<Il2CppScheduleOne.ItemFramework.ItemInstance>();
            instances.Add(probe);
            for (int i = 1; i < qty; i++)
            {
                var inst = def.GetDefaultInstance(1);
                if (inst != null && inst.Pointer != IntPtr.Zero)
                {
                    instances.Add(inst);
                }
            }

            for (int i = 0; i < instances.Count; i++)
            {
                inv.AddItemToInventory(instances[i]);
            }

            Mod.Log.Info($"Player extracted {instances.Count}x packaging '{rData.InputPackaging.ItemId}' from AutoPackagingStation.");
            rData.InputPackaging = null;
            if (rData.State == StationState.Blocked || rData.State == StationState.NoPackaging)
            {
                rData.State = StationState.Idle;
            }
            UpdateLedVisuals();
            AudioHelper.PlayClickSound();
            return true;
        }
        catch (Exception ex)
        {
            Mod.Log.Error($"Failed to extract input packaging: {ex}");
            AudioHelper.PlayDenySound();
            return false;
        }
    }

    public bool TryExtractOutputProduct()
    {
        var inv = PlayerInventory.Instance;
        if (inv == null || inv.Pointer == IntPtr.Zero) return false;

        var station = GetComponent<PackagingStation>() ?? GetComponentInParent<PackagingStation>();
        if (station != null && station.Pointer != IntPtr.Zero)
        {
            var outputSlots = station.OutputSlots;
            if (outputSlots != null && outputSlots.Count > 0)
            {
                var outSlot = outputSlots[0];
                if (outSlot != null && outSlot.Pointer != IntPtr.Zero && outSlot.ItemInstance != null && outSlot.ItemInstance.Pointer != IntPtr.Zero && outSlot.Quantity > 0)
                {
                    var inst = outSlot.ItemInstance;
                    int totalQty = outSlot.Quantity;

                    if (!inv.CanItemFitInInventory(inst, totalQty))
                    {
                        AudioHelper.PlayDenySound();
                        return false;
                    }

                    var prodInst = inst.TryCast<Il2CppScheduleOne.Product.ProductItemInstance>();
                    var pkgDef = prodInst?.AppliedPackaging ?? (!string.IsNullOrEmpty(prodInst?.PackagingID) ? GameRegistry.GetItem(prodInst.PackagingID)?.TryCast<Il2CppScheduleOne.Product.Packaging.PackagingDefinition>() : null);
                    var qual = inst.TryCast<NativeQualityItemInst>()?.Quality ?? EQuality.Standard;

                    for (int i = 0; i < totalQty; i++)
                    {
                        var newInst = inst.Definition.GetDefaultInstance(1);
                        if (newInst != null && newInst.Pointer != IntPtr.Zero)
                        {
                            var q = newInst.TryCast<NativeQualityItemInst>();
                            if (q != null && q.Pointer != IntPtr.Zero)
                            {
                                q.Quality = qual;
                            }
                            var p = newInst.TryCast<Il2CppScheduleOne.Product.ProductItemInstance>();
                            if (p != null && p.Pointer != IntPtr.Zero)
                            {
                                if (pkgDef != null && pkgDef.Pointer != IntPtr.Zero)
                                {
                                    p.SetPackaging(pkgDef);
                                }
                                else if (!string.IsNullOrEmpty(prodInst?.PackagingID))
                                {
                                    p.PackagingID = prodInst.PackagingID;
                                }
                            }
                            inv.AddItemToInventory(newInst);
                        }
                    }

                    outSlot.ClearStoredInstance();
                    outSlot.onItemDataChanged?.Invoke();
                    outSlot.onItemInstanceChanged?.Invoke();
                    try { station.UpdatePackagingVisuals(); } catch { }
                    AudioHelper.PlayCashSound();
                    return true;
                }
            }
        }

        var rData = AutoPackStore.GetRuntimeData(_stationGuid);
        if (rData.OutputProduct == null || rData.OutputProduct.Quantity <= 0)
        {
            AudioHelper.PlayDenySound();
            return false;
        }

        try
        {
            var def = GameRegistry.GetItem(rData.OutputProduct.ItemId);
            if (def == null || def.Pointer == IntPtr.Zero)
            {
                Mod.Log.Warn($"[AutoPackagingStation] Output definition '{rData.OutputProduct.ItemId}' not found in registry.");
                AudioHelper.PlayDenySound();
                return false;
            }

            int qty = rData.OutputProduct.Quantity;
            var probe = def.GetDefaultInstance(1);
            if (probe == null || probe.Pointer == IntPtr.Zero)
            {
                Mod.Log.Warn($"[AutoPackagingStation] Failed to create probe instance for '{rData.OutputProduct.ItemId}'.");
                AudioHelper.PlayDenySound();
                return false;
            }

            // Apply packaging to probe before testing inventory fit!
            var pkgDef = !string.IsNullOrEmpty(rData.OutputProduct.PackagingId)
                ? GameRegistry.GetItem(rData.OutputProduct.PackagingId)?.TryCast<Il2CppScheduleOne.Product.Packaging.PackagingDefinition>()
                : null;

            var probeProd = probe.TryCast<Il2CppScheduleOne.Product.ProductItemInstance>();
            if (probeProd != null && probeProd.Pointer != IntPtr.Zero)
            {
                if (!string.IsNullOrEmpty(rData.OutputProduct.PackagingId))
                {
                    probeProd.PackagingID = rData.OutputProduct.PackagingId;
                }
                if (pkgDef != null && pkgDef.Pointer != IntPtr.Zero)
                {
                    probeProd.SetPackaging(pkgDef);
                }
            }

            var probeQual = probe.TryCast<NativeQualityItemInst>();
            if (probeQual != null && probeQual.Pointer != IntPtr.Zero)
            {
                probeQual.Quality = (EQuality)rData.OutputProduct.QualityTier;
            }

            if (!inv.CanItemFitInInventory(probe, qty))
            {
                AudioHelper.PlayDenySound();
                return false;
            }

            // Pre-create all instances before modifying inventory
            var instances = new List<Il2CppScheduleOne.ItemFramework.ItemInstance>();
            instances.Add(probe);
            for (int i = 1; i < qty; i++)
            {
                var inst = def.GetDefaultInstance(1);
                if (inst != null && inst.Pointer != IntPtr.Zero)
                {
                    var pInst = inst.TryCast<Il2CppScheduleOne.Product.ProductItemInstance>();
                    if (pInst != null && pInst.Pointer != IntPtr.Zero)
                    {
                        if (!string.IsNullOrEmpty(rData.OutputProduct.PackagingId))
                        {
                            pInst.PackagingID = rData.OutputProduct.PackagingId;
                        }
                        if (pkgDef != null && pkgDef.Pointer != IntPtr.Zero)
                        {
                            pInst.SetPackaging(pkgDef);
                        }
                    }
                    var qInst = inst.TryCast<NativeQualityItemInst>();
                    if (qInst != null && qInst.Pointer != IntPtr.Zero)
                    {
                        qInst.Quality = (EQuality)rData.OutputProduct.QualityTier;
                    }
                    instances.Add(inst);
                }
            }

            for (int i = 0; i < instances.Count; i++)
            {
                inv.AddItemToInventory(instances[i]);
            }

            Mod.Log.Info($"Player collected {instances.Count}x '{rData.OutputProduct.ItemId}' from AutoPackagingStation.");
            rData.OutputProduct = null;
            rData.State = StationState.Idle;
            UpdateLedVisuals();
            AudioHelper.PlayCashSound();
            return true;
        }
        catch (Exception ex)
        {
            Mod.Log.Error($"Failed to transfer output to inventory: {ex}");
            AudioHelper.PlayDenySound();
            return false;
        }
    }

    public bool TryExtractAll()
    {
        var rData = AutoPackStore.GetRuntimeData(_stationGuid);
        bool extractedAny = false;

        if (rData.OutputProduct != null && rData.OutputProduct.Quantity > 0)
        {
            if (TryExtractOutputProduct()) extractedAny = true;
        }
        if (rData.InputProduct != null && rData.InputProduct.Quantity > 0)
        {
            if (TryExtractInputProduct()) extractedAny = true;
        }
        if (rData.InputPackaging != null && rData.InputPackaging.Quantity > 0)
        {
            if (TryExtractInputPackaging()) extractedAny = true;
        }

        if (!extractedAny)
        {
            AudioHelper.PlayDenySound();
        }
        return extractedAny;
    }

    public bool TryDepositProduct()
    {
        var inv = PlayerInventory.Instance;
        if (inv == null || inv.Pointer == IntPtr.Zero) return false;

        var rData = AutoPackStore.GetRuntimeData(_stationGuid);
        int currentQty = rData.InputProduct?.Quantity ?? 0;
        if (currentQty >= 20)
        {
            AudioHelper.PlayDenySound();
            return false;
        }

        try
        {
            var slots = inv.GetAllInventorySlots();
            if (slots != null)
            {
                for (int i = 0; i < slots.Count; i++)
                {
                    var slot = slots[i];
                    if (slot == null || slot.Pointer == IntPtr.Zero) continue;
                    var inst = slot.ItemInstance;
                    if (inst == null || inst.Pointer == IntPtr.Zero || inst.Definition == null) continue;

                    string id = inst.Definition.ID ?? string.Empty;
                    if (string.IsNullOrEmpty(id) || id == Mod.CurrentConfig.StationItemId) continue;

                    var def = GameRegistry.GetItem(id);
                    if (def == null || def.Pointer == IntPtr.Zero) continue;

                    // Raw product check via Registry
                    var prodDef = def.TryCast<Il2CppScheduleOne.Product.ProductDefinition>();
                    if (prodDef != null && prodDef.ValidPackaging != null && prodDef.ValidPackaging.Length > 0)
                    {
                        if (rData.InputProduct == null || (rData.InputProduct.ItemId == id && rData.InputProduct.Quantity < 20))
                        {
                            int availableCapacity = 20 - (rData.InputProduct?.Quantity ?? 0);
                            int takeQty = Mathf.Min(inst.Quantity, Mathf.Min(10, availableCapacity));
                            float quality = 0.55f;
                            int tier = 2;

                            var qInst = inst.TryCast<NativeQualityItemInst>();
                            if (qInst != null && qInst.Pointer != IntPtr.Zero)
                            {
                                tier = (int)qInst.Quality;
                                quality = tier switch { 0 => 0.2f, 1 => 0.35f, 2 => 0.55f, 3 => 0.80f, _ => 0.95f };
                            }

                            if (rData.InputProduct == null)
                            {
                                rData.InputProduct = new SlotItemData
                                {
                                    ItemId = id,
                                    ItemName = inst.Definition.Name ?? id,
                                    Quantity = takeQty,
                                    QualityValue = quality,
                                    QualityTier = tier,
                                };
                            }
                            else
                            {
                                int oldQty = rData.InputProduct.Quantity;
                                float oldQual = rData.InputProduct.QualityValue;
                                rData.InputProduct.Quantity += takeQty;
                                rData.InputProduct.QualityValue = ((oldQty * oldQual) + (takeQty * quality)) / (oldQty + takeQty);
                            }

                            inv.RemoveAmountOfItem(id, (uint)takeQty);

                            if (rData.State == StationState.Blocked || rData.State == StationState.NoPackaging)
                            {
                                rData.State = StationState.Idle;
                            }
                            UpdateLedVisuals();
                            AudioHelper.PlayClickSound();
                            return true;
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Mod.Log.Debug($"TryDepositProduct error: {ex.Message}");
        }

        AudioHelper.PlayDenySound();
        return false;
    }

    public bool TryDepositPackaging()
    {
        var inv = PlayerInventory.Instance;
        if (inv == null || inv.Pointer == IntPtr.Zero) return false;

        var rData = AutoPackStore.GetRuntimeData(_stationGuid);
        int currentQty = rData.InputPackaging?.Quantity ?? 0;
        if (currentQty >= 20)
        {
            AudioHelper.PlayDenySound();
            return false;
        }

        try
        {
            var slots = inv.GetAllInventorySlots();
            if (slots != null)
            {
                for (int i = 0; i < slots.Count; i++)
                {
                    var slot = slots[i];
                    if (slot == null || slot.Pointer == IntPtr.Zero) continue;
                    var inst = slot.ItemInstance;
                    if (inst == null || inst.Pointer == IntPtr.Zero || inst.Definition == null) continue;

                    string id = inst.Definition.ID ?? string.Empty;
                    if (string.IsNullOrEmpty(id) || id == Mod.CurrentConfig.StationItemId) continue;

                    var def = GameRegistry.GetItem(id);
                    if (def == null || def.Pointer == IntPtr.Zero) continue;

                    // Packaging container check via Registry
                    bool isPackaging = def.TryCast<Il2CppScheduleOne.Product.Packaging.PackagingDefinition>() != null;

                    if (isPackaging)
                    {
                        if (rData.InputPackaging == null || (rData.InputPackaging.ItemId == id && rData.InputPackaging.Quantity < 20))
                        {
                            int availableCapacity = 20 - (rData.InputPackaging?.Quantity ?? 0);
                            int takeQty = Mathf.Min(inst.Quantity, Mathf.Min(10, availableCapacity));
                            if (rData.InputPackaging == null)
                            {
                                rData.InputPackaging = new SlotItemData { ItemId = id, ItemName = id, Quantity = takeQty };
                            }
                            else
                            {
                                rData.InputPackaging.Quantity += takeQty;
                            }
                            inv.RemoveAmountOfItem(id, (uint)takeQty);

                            if (rData.State == StationState.Blocked || rData.State == StationState.NoPackaging)
                            {
                                rData.State = StationState.Idle;
                            }
                            UpdateLedVisuals();
                            AudioHelper.PlayClickSound();
                            return true;
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Mod.Log.Debug($"TryDepositPackaging error: {ex.Message}");
        }

        AudioHelper.PlayDenySound();
        return false;
    }

    private void PackUpStation()
    {
        var inv = PlayerInventory.Instance;
        if (inv == null || inv.Pointer == IntPtr.Zero) return;

        var rData = AutoPackStore.GetRuntimeData(_stationGuid);
        var station = GetComponent<PackagingStation>() ?? GetComponentInParent<PackagingStation>();
        // Duplication fix: with a LIVE native station the slots are the single source of truth — never fall back to stale rData
        bool hasLiveNativeStation = (station != null && station.Pointer != IntPtr.Zero);

        // 1. Resolve Station item definition and probe
        var stationDef = GameRegistry.GetItem(Mod.CurrentConfig.StationItemId);
        if (stationDef == null || stationDef.Pointer == IntPtr.Zero)
        {
            Mod.Log.Warn($"Cannot pack up: '{Mod.CurrentConfig.StationItemId}' not found in registry.");
            AudioHelper.PlayDenySound();
            return;
        }

        var stationProbe = stationDef.GetDefaultInstance(1);
        if (stationProbe == null || stationProbe.Pointer == IntPtr.Zero)
        {
            AudioHelper.PlayDenySound();
            return;
        }

        // 2. Pre-create all 1-unit instances BEFORE modifying state (station item last, so a
        // partial abort below can never duplicate it)
        var itemsToAdd = new List<Il2CppScheduleOne.ItemFramework.ItemInstance>();
        int pkgItemCount = 0, prodItemCount = 0, outItemCount = 0;
        int countMark = itemsToAdd.Count;

        // Input Packaging (InputSlots[0] vs rData.InputPackaging)
        var pkgSlot = (station != null && station.Pointer != IntPtr.Zero && station.InputSlots != null && station.InputSlots.Count > 0)
            ? station.InputSlots[0] : null;
        if (pkgSlot != null && pkgSlot.Pointer != IntPtr.Zero && pkgSlot.ItemInstance != null && pkgSlot.ItemInstance.Pointer != IntPtr.Zero && pkgSlot.Quantity > 0 && pkgSlot.ItemInstance.Definition != null)
        {
            int qty = pkgSlot.Quantity;
            var def = pkgSlot.ItemInstance.Definition;
            for (int i = 0; i < qty; i++)
            {
                var inst = def.GetDefaultInstance(1);
                if (inst != null && inst.Pointer != IntPtr.Zero)
                {
                    itemsToAdd.Add(inst);
                }
            }
        }
        else if (!hasLiveNativeStation && rData.InputPackaging != null && rData.InputPackaging.Quantity > 0)
        {
            var pkgDef = GameRegistry.GetItem(rData.InputPackaging.ItemId);
            if (pkgDef != null && pkgDef.Pointer != IntPtr.Zero)
            {
                for (int i = 0; i < rData.InputPackaging.Quantity; i++)
                {
                    var inst = pkgDef.GetDefaultInstance(1);
                    if (inst != null && inst.Pointer != IntPtr.Zero)
                    {
                        itemsToAdd.Add(inst);
                    }
                }
            }
        }

        pkgItemCount = itemsToAdd.Count - countMark; countMark = itemsToAdd.Count;

        // Input Product (InputSlots[1] vs rData.InputProduct)
        var prodSlot = (station != null && station.Pointer != IntPtr.Zero && station.InputSlots != null && station.InputSlots.Count > 1)
            ? station.InputSlots[1] : null;
        if (prodSlot != null && prodSlot.Pointer != IntPtr.Zero && prodSlot.ItemInstance != null && prodSlot.ItemInstance.Pointer != IntPtr.Zero && prodSlot.Quantity > 0 && prodSlot.ItemInstance.Definition != null)
        {
            int qty = prodSlot.Quantity;
            var def = prodSlot.ItemInstance.Definition;
            var qInst = prodSlot.ItemInstance.TryCast<NativeQualityItemInst>();
            var qual = qInst != null && qInst.Pointer != IntPtr.Zero ? qInst.Quality : EQuality.Standard;

            for (int i = 0; i < qty; i++)
            {
                var inst = def.GetDefaultInstance(1);
                if (inst != null && inst.Pointer != IntPtr.Zero)
                {
                    var q = inst.TryCast<NativeQualityItemInst>();
                    if (q != null && q.Pointer != IntPtr.Zero)
                    {
                        q.Quality = qual;
                    }
                    itemsToAdd.Add(inst);
                }
            }
        }
        else if (!hasLiveNativeStation && rData.InputProduct != null && rData.InputProduct.Quantity > 0)
        {
            var inDef = GameRegistry.GetItem(rData.InputProduct.ItemId);
            if (inDef != null && inDef.Pointer != IntPtr.Zero)
            {
                for (int i = 0; i < rData.InputProduct.Quantity; i++)
                {
                    var inst = inDef.GetDefaultInstance(1);
                    if (inst != null && inst.Pointer != IntPtr.Zero)
                    {
                        var qInst = inst.TryCast<NativeQualityItemInst>();
                        if (qInst != null && qInst.Pointer != IntPtr.Zero)
                        {
                            qInst.Quality = (EQuality)rData.InputProduct.QualityTier;
                        }
                        itemsToAdd.Add(inst);
                    }
                }
            }
        }

        prodItemCount = itemsToAdd.Count - countMark; countMark = itemsToAdd.Count;

        // Output Product (OutputSlots[0] vs rData.OutputProduct)
        var outSlot = (station != null && station.Pointer != IntPtr.Zero && station.OutputSlots != null && station.OutputSlots.Count > 0)
            ? station.OutputSlots[0] : null;
        if (outSlot != null && outSlot.Pointer != IntPtr.Zero && outSlot.ItemInstance != null && outSlot.ItemInstance.Pointer != IntPtr.Zero && outSlot.Quantity > 0 && outSlot.ItemInstance.Definition != null)
        {
            int qty = outSlot.Quantity;
            var inst = outSlot.ItemInstance;
            var def = inst.Definition;
            var prodInst = inst.TryCast<Il2CppScheduleOne.Product.ProductItemInstance>();
            var pkgDef = prodInst?.AppliedPackaging ?? (!string.IsNullOrEmpty(prodInst?.PackagingID) ? GameRegistry.GetItem(prodInst.PackagingID)?.TryCast<Il2CppScheduleOne.Product.Packaging.PackagingDefinition>() : null);
            var qual = inst.TryCast<NativeQualityItemInst>()?.Quality ?? EQuality.Standard;

            for (int i = 0; i < qty; i++)
            {
                var newInst = def.GetDefaultInstance(1);
                if (newInst != null && newInst.Pointer != IntPtr.Zero)
                {
                    var p = newInst.TryCast<Il2CppScheduleOne.Product.ProductItemInstance>();
                    if (p != null && p.Pointer != IntPtr.Zero)
                    {
                        if (pkgDef != null && pkgDef.Pointer != IntPtr.Zero)
                        {
                            p.SetPackaging(pkgDef);
                        }
                        else if (!string.IsNullOrEmpty(prodInst?.PackagingID))
                        {
                            p.PackagingID = prodInst.PackagingID;
                        }
                    }
                    var q = newInst.TryCast<NativeQualityItemInst>();
                    if (q != null && q.Pointer != IntPtr.Zero)
                    {
                        q.Quality = qual;
                    }
                    itemsToAdd.Add(newInst);
                }
            }
        }
        else if (!hasLiveNativeStation && rData.OutputProduct != null && rData.OutputProduct.Quantity > 0)
        {
            var outDef = GameRegistry.GetItem(rData.OutputProduct.ItemId);
            if (outDef != null && outDef.Pointer != IntPtr.Zero)
            {
                var pkgItemDef = !string.IsNullOrEmpty(rData.OutputProduct.PackagingId)
                    ? GameRegistry.GetItem(rData.OutputProduct.PackagingId)?.TryCast<Il2CppScheduleOne.Product.Packaging.PackagingDefinition>()
                    : null;

                for (int i = 0; i < rData.OutputProduct.Quantity; i++)
                {
                    var inst = outDef.GetDefaultInstance(1);
                    if (inst != null && inst.Pointer != IntPtr.Zero)
                    {
                        var prodInst = inst.TryCast<Il2CppScheduleOne.Product.ProductItemInstance>();
                        if (prodInst != null && prodInst.Pointer != IntPtr.Zero)
                        {
                            if (!string.IsNullOrEmpty(rData.OutputProduct.PackagingId))
                            {
                                prodInst.PackagingID = rData.OutputProduct.PackagingId;
                            }
                            if (pkgItemDef != null && pkgItemDef.Pointer != IntPtr.Zero)
                            {
                                prodInst.SetPackaging(pkgItemDef);
                            }
                        }
                        var qInst = inst.TryCast<NativeQualityItemInst>();
                        if (qInst != null && qInst.Pointer != IntPtr.Zero)
                        {
                            qInst.Quality = (EQuality)rData.OutputProduct.QualityTier;
                        }
                        itemsToAdd.Add(inst);
                    }
                }
            }
        }

        outItemCount = itemsToAdd.Count - countMark;
        // Station item last: a partial abort below can never duplicate it (buffers deduct, station stays)
        itemsToAdd.Add(stationProbe);

        // Interleaved fit-check + add — each 1-unit probe is validated against the CURRENT inventory
        // (earlier adds already applied), so a near-full tail can't silently vanish (item-loss fix)
        int addedCount = 0;
        for (int i = 0; i < itemsToAdd.Count; i++)
        {
            var item = itemsToAdd[i];
            if (!inv.CanItemFitInInventory(item, 1))
            {
                Mod.Log.Warn($"PackUp: Inventory full, could not return item '{item.Definition?.ID}'. ({addedCount}/{itemsToAdd.Count} returned)");
                break;
            }
            try
            {
                inv.AddItemToInventory(item);
                addedCount++;
            }
            catch (Exception addEx)
            {
                Mod.Log.Warn($"PackUp: failed to return item '{item.Definition?.ID}': {addEx.Message}");
                break;
            }
        }

        if (addedCount < itemsToAdd.Count)
        {
            // Only what was actually added leaves the station buffers — deduct exactly those
            // quantities so nothing is lost and a retry can't duplicate anything.
            int remaining = addedCount;
            int takePkg = Mathf.Min(pkgItemCount, remaining); remaining -= takePkg;
            int takeProd = Mathf.Min(prodItemCount, remaining); remaining -= takeProd;
            int takeOut = Mathf.Min(outItemCount, remaining);
            try
            {
                if (takePkg > 0)
                {
                    if (pkgSlot != null && pkgSlot.Pointer != IntPtr.Zero && pkgSlot.ItemInstance != null && pkgSlot.ItemInstance.Pointer != IntPtr.Zero)
                    {
                        if (takePkg >= pkgSlot.Quantity) { pkgSlot.ClearStoredInstance(); }
                        else { pkgSlot.ChangeQuantity(-takePkg); }
                        pkgSlot.onItemDataChanged?.Invoke();
                        pkgSlot.onItemInstanceChanged?.Invoke();
                    }
                    else if (rData.InputPackaging != null)
                    {
                        rData.InputPackaging.Quantity -= takePkg;
                        if (rData.InputPackaging.Quantity <= 0) rData.InputPackaging = null;
                    }
                }
                if (takeProd > 0)
                {
                    if (prodSlot != null && prodSlot.Pointer != IntPtr.Zero && prodSlot.ItemInstance != null && prodSlot.ItemInstance.Pointer != IntPtr.Zero)
                    {
                        if (takeProd >= prodSlot.Quantity) { prodSlot.ClearStoredInstance(); }
                        else { prodSlot.ChangeQuantity(-takeProd); }
                        prodSlot.onItemDataChanged?.Invoke();
                        prodSlot.onItemInstanceChanged?.Invoke();
                    }
                    else if (rData.InputProduct != null)
                    {
                        rData.InputProduct.Quantity -= takeProd;
                        if (rData.InputProduct.Quantity <= 0) rData.InputProduct = null;
                    }
                }
                if (takeOut > 0)
                {
                    if (outSlot != null && outSlot.Pointer != IntPtr.Zero && outSlot.ItemInstance != null && outSlot.ItemInstance.Pointer != IntPtr.Zero)
                    {
                        if (takeOut >= outSlot.Quantity) { outSlot.ClearStoredInstance(); }
                        else { outSlot.ChangeQuantity(-takeOut); }
                        outSlot.onItemDataChanged?.Invoke();
                        outSlot.onItemInstanceChanged?.Invoke();
                    }
                    else if (rData.OutputProduct != null)
                    {
                        rData.OutputProduct.Quantity -= takeOut;
                        if (rData.OutputProduct.Quantity <= 0) rData.OutputProduct = null;
                    }
                }
                if (station != null && station.Pointer != IntPtr.Zero)
                {
                    try { station.UpdatePackagingVisuals(); station.UpdateProductVisuals(); } catch { }
                }
            }
            catch (Exception deductEx)
            {
                Mod.Log.Warn($"PackUp partial deduct failed: {deductEx.Message}");
            }
            AudioHelper.PlayDenySound();
            return;
        }

        Mod.Log.Info($"Successfully packed up AutoPackagingStation and returned {itemsToAdd.Count} items to inventory.");
        AudioHelper.PlayCashSound();

        // Mitigation 3: Unregister outdoor street item if HomelessMod is active (M9: log on fail)
        try
        {
            var streetManagerType = TypeResolver.Find("HomelessMod.Building.StreetPropertyManager", "HomelessMod");
            if (streetManagerType == null)
            {
                Mod.Log.Debug("PackUp: HomelessMod not present, skip UnregisterStreetItem.");
            }
            else
            {
                var unregisterMethod = streetManagerType.GetMethods().FirstOrDefault(m => m.Name == "UnregisterStreetItem");
                if (unregisterMethod == null) Mod.Log.Warn("PackUp: UnregisterStreetItem method not found on StreetPropertyManager.");
                else unregisterMethod.Invoke(null, new object?[] { gameObject });
            }
        }
        catch (Exception ex)
        {
            Mod.Log.Warn($"PackUp HomelessMod unregister failed: {ex.Message}");
        }

        // Clear native slots
        if (station != null && station.Pointer != IntPtr.Zero)
        {
            if (station.InputSlots != null)
            {
                for (int s = 0; s < station.InputSlots.Count; s++)
                {
                    var slot = station.InputSlots[s];
                    if (slot != null && slot.Pointer != IntPtr.Zero)
                    {
                        slot.ClearStoredInstance();
                        slot.onItemDataChanged?.Invoke();
                        slot.onItemInstanceChanged?.Invoke();
                    }
                }
            }
            if (station.OutputSlots != null)
            {
                for (int s = 0; s < station.OutputSlots.Count; s++)
                {
                    var slot = station.OutputSlots[s];
                    if (slot != null && slot.Pointer != IntPtr.Zero)
                    {
                        slot.ClearStoredInstance();
                        slot.onItemDataChanged?.Invoke();
                        slot.onItemInstanceChanged?.Invoke();
                    }
                }
            }
        }

        // Mitigation 2: Clear runtime data BEFORE Destroy() so OnDestroy does not duplicate items
        rData.InputProduct = null;
        rData.InputPackaging = null;
        rData.OutputProduct = null;
        AutoPackStore.RemoveRuntimeData(_stationGuid);
        AutoPackStore.UnregisterStation(this);

        GameObject.Destroy(gameObject);
    }
}
