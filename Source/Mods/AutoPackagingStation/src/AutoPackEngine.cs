using System;
using System.Collections.Generic;
using AutoPackagingStation.Entities;
using AutoPackagingStation.Persistence;
using Il2CppInterop.Runtime;
using Il2CppScheduleOne.ItemFramework;
using Il2CppScheduleOne.ObjectScripts;
using Il2CppScheduleOne.Product;
using Il2CppScheduleOne.Product.Packaging;
using S1Mods.Shared;
using UnityEngine;
using GameRegistry = Il2CppScheduleOne.Registry;

namespace AutoPackagingStation.Engine;

/// <summary>
/// Core execution engine for automated packaging processing.
/// Implements 2-phase atomic transactions, quality freshness bonuses, and pneumatic audio synthesis.
/// Drives packaging directly from native PackagingStation input/output slots.
/// </summary>
public static class AutoPackEngine
{
    private static AudioClip? _pneumaticHissClip;
    private static AudioClip? _compressorStrokeClip;
    private static AudioClip? _mechanicalClankClip;

    /// <summary>
    /// Processes an active packaging time step for a station controller.
    /// Host-only tick; clients only animate via host sync (prevents client desync H7).
    /// </summary>
    public static void ProcessPackagingStep(AutoPackStationController controller, float deltaTime)
    {
        if (controller == null || controller.Pointer == IntPtr.Zero) return;

        var station = controller.GetComponent<PackagingStation>() ?? controller.GetComponentInParent<PackagingStation>();
        bool isNativeStation = (station != null && station.Pointer != IntPtr.Zero);

        var rData = AutoPackStore.GetRuntimeData(controller.StationGuid);

        if (!IsHostOrSingleplayer())
        {
            return;
        }

        float targetDuration = Mathf.Max(0.2f, Mod.CurrentConfig.PackagingDurationSeconds);
        rData.PackagingProgress += deltaTime / targetDuration;

        // Animate conveyor and pneumatic piston (host-authoritative)
        controller.AnimateCycle(deltaTime, rData.PackagingProgress);

        if (rData.PackagingProgress >= 1.0f)
        {
            // Execute atomic commit
            bool success;
            if (isNativeStation)
            {
                success = ExecutePackagingTransaction(station!);
            }
            else
            {
                success = ExecutePackagingTransaction(controller.StationGuid);
            }

            rData.PackagingProgress = 0f;
            if (success)
            {
                rData.State = StationState.Complete;
                controller.UpdateLedVisuals();
                PlayPneumaticHiss(controller.AudioSource);
            }
            else
            {
                if (rData.State != StationState.NoPackaging)
                {
                    rData.State = StationState.Blocked;
                }
                controller.UpdateLedVisuals();
                AudioHelper.PlayDenySound();
            }
        }
    }

    /// <summary>
    /// Checks if a native PackagingStation has valid inputs and output capacity to start packaging.
    /// Slot 0 = Packaging, Slot 1 = Product.
    /// </summary>
    public static bool CanStationPackage(PackagingStation station, out string outputItemId, out string packagingId)
    {
        outputItemId = string.Empty;
        packagingId = string.Empty;

        if (station == null || station.Pointer == IntPtr.Zero) return false;
        var inputSlots = station.InputSlots;
        var outputSlots = station.OutputSlots;
        if (inputSlots == null || inputSlots.Count < 2 || outputSlots == null || outputSlots.Count < 1) return false;

        var pkgSlot = inputSlots[0];
        var prodSlot = inputSlots[1];
        var outSlot = outputSlots[0];

        if (pkgSlot == null || pkgSlot.Pointer == IntPtr.Zero || pkgSlot.ItemInstance == null || pkgSlot.ItemInstance.Pointer == IntPtr.Zero || pkgSlot.Quantity <= 0)
            return false;

        if (prodSlot == null || prodSlot.Pointer == IntPtr.Zero || prodSlot.ItemInstance == null || prodSlot.ItemInstance.Pointer == IntPtr.Zero || prodSlot.Quantity <= 0)
            return false;

        var prodDef = prodSlot.ItemInstance.Definition;
        var pkgDef = pkgSlot.ItemInstance.Definition;
        if (prodDef == null || prodDef.Pointer == IntPtr.Zero || pkgDef == null || pkgDef.Pointer == IntPtr.Zero)
            return false;

        var pkgDefObj = pkgDef.TryCast<PackagingDefinition>();
        int requiredProductQty = (pkgDefObj != null && pkgDefObj.Quantity > 0) ? pkgDefObj.Quantity : 1;
        if (prodSlot.Quantity < requiredProductQty)
            return false;

        string rawItemId = prodDef.ID ?? string.Empty;
        packagingId = pkgDef.ID ?? string.Empty;
        if (string.IsNullOrEmpty(rawItemId) || string.IsNullOrEmpty(packagingId))
            return false;

        outputItemId = ResolvePackagedItemId(rawItemId, packagingId);
        if (string.IsNullOrEmpty(outputItemId))
            return false;

        // Check output slot capacity & matching item
        if (outSlot != null && outSlot.Pointer != IntPtr.Zero && outSlot.ItemInstance != null && outSlot.ItemInstance.Pointer != IntPtr.Zero && outSlot.Quantity > 0)
        {
            var outDef = outSlot.ItemInstance.Definition;
            if (outDef == null || outDef.Pointer == IntPtr.Zero || outDef.ID != outputItemId)
                return false;

            int stackLimit = outDef.StackLimit;
            if (outSlot.Quantity >= stackLimit)
                return false;
        }

        return true;
    }

    /// <summary>
    /// Executes atomic 2-phase packaging directly on the native PackagingStation slots:
    /// Deducts required product quantity from Product slot (InputSlots[1]), deducts 1 from Packaging slot (InputSlots[0]),
    /// and adds packaged product to Output slot (OutputSlots[0]).
    /// </summary>
    public static bool ExecutePackagingTransaction(PackagingStation station)
    {
        if (!IsHostOrSingleplayer()) return false;
        if (station == null || station.Pointer == IntPtr.Zero) return false;

        var inputSlots = station.InputSlots;
        var outputSlots = station.OutputSlots;
        if (inputSlots == null || inputSlots.Count < 2 || outputSlots == null || outputSlots.Count < 1) return false;

        var pkgSlot = inputSlots[0];
        var prodSlot = inputSlots[1];
        var outSlot = outputSlots[0];

        if (outSlot == null || outSlot.Pointer == IntPtr.Zero)
            return false;

        if (pkgSlot == null || pkgSlot.Pointer == IntPtr.Zero || pkgSlot.ItemInstance == null || pkgSlot.ItemInstance.Pointer == IntPtr.Zero || pkgSlot.Quantity <= 0)
            return false;

        if (prodSlot == null || prodSlot.Pointer == IntPtr.Zero || prodSlot.ItemInstance == null || prodSlot.ItemInstance.Pointer == IntPtr.Zero || prodSlot.Quantity <= 0)
            return false;

        var prodInst = prodSlot.ItemInstance;
        var pkgInst = pkgSlot.ItemInstance;
        var prodDef = prodInst.Definition;
        var pkgDef = pkgInst.Definition;
        if (prodDef == null || prodDef.Pointer == IntPtr.Zero || pkgDef == null || pkgDef.Pointer == IntPtr.Zero)
            return false;

        var packagingDefObj = pkgDef.TryCast<PackagingDefinition>();
        int requiredProductQty = (packagingDefObj != null && packagingDefObj.Quantity > 0) ? packagingDefObj.Quantity : 1;
        if (prodSlot.Quantity < requiredProductQty) return false;

        string rawItemId = prodDef.ID ?? string.Empty;
        string packagingId = pkgDef.ID ?? string.Empty;
        string outputItemId = ResolvePackagedItemId(rawItemId, packagingId);
        if (string.IsNullOrEmpty(outputItemId)) return false;

        // Compute feasible batch size respecting MaxBatchSize (H6)
        int configuredBatch = Mathf.Clamp(Mod.CurrentConfig.MaxBatchSize, 1, 20);
        int maxByProductNative = prodSlot!.Quantity / Math.Max(1, requiredProductQty);
        int maxByPackagingNative = pkgSlot!.Quantity;
        int currentOutQty = (outSlot != null && outSlot.Pointer != IntPtr.Zero && outSlot.ItemInstance != null && outSlot.ItemInstance.Pointer != IntPtr.Zero) ? outSlot.Quantity : 0;
        string currentOutId = (outSlot != null && outSlot.ItemInstance != null && outSlot.ItemInstance.Definition != null) ? outSlot.ItemInstance.Definition.ID : string.Empty;
        int outStackLimit = 20;
        try
        {
            var outDefProbe = GameRegistry.GetItem(outputItemId);
            if (outDefProbe != null && outDefProbe.Pointer != IntPtr.Zero) outStackLimit = outDefProbe.StackLimit;
            else if (!string.IsNullOrEmpty(currentOutId))
            {
                var curDef = GameRegistry.GetItem(currentOutId);
                if (curDef != null && curDef.Pointer != IntPtr.Zero) outStackLimit = curDef.StackLimit;
            }
        }
        catch { }
        int maxByOutputNative = outStackLimit - currentOutQty;
        // Validate output slot item match / capacity before batch calc
        if (currentOutQty > 0)
        {
            if (outSlot == null || outSlot.Pointer == IntPtr.Zero || outSlot.ItemInstance == null || outSlot.ItemInstance.Pointer == IntPtr.Zero) return false;
            var curOutDef = outSlot.ItemInstance.Definition;
            if (curOutDef == null || curOutDef.Pointer == IntPtr.Zero || curOutDef.ID != outputItemId)
                return false;
            if (currentOutQty >= outStackLimit)
                return false;
        }
        int feasibleNative = Math.Min(Math.Min(maxByProductNative, maxByPackagingNative), maxByOutputNative);
        if (feasibleNative <= 0) return false;
        int batchSizeNative = Math.Max(1, Math.Min(configuredBatch, feasibleNative));
        int deductProductNative = requiredProductQty * batchSizeNative;

        try
        {
            // Quality calculation with freshness bonus — unified with fallback path via PackagingMath (H5)
            EQuality upgradedQuality = EQuality.Standard;
            {
                float bonusMultiplier = Mathf.Max(0f, Mod.CurrentConfig.FreshnessBonusMultiplier);
                float baseQuality = 0.55f;
                var qInst = prodInst.TryCast<QualityItemInstance>();
                if (qInst != null && qInst.Pointer != IntPtr.Zero)
                {
                    baseQuality = (int)qInst.Quality switch { 0 => 0.20f, 1 => 0.35f, 2 => 0.55f, 3 => 0.80f, _ => 0.95f };
                }
                float upgradedVal = Mathf.Clamp01(baseQuality * (1f + bonusMultiplier));
                int upgradedTier = PackagingMath.ComputeQualityTier(upgradedVal);
                upgradedQuality = (EQuality)upgradedTier;
            }

            // Phase 2: Deduct inputs & add output — batch-aware (H6)
            // 1. Deduct Product
            if (prodSlot.Quantity > deductProductNative)
            {
                prodSlot.ChangeQuantity(-deductProductNative);
            }
            else
            {
                prodSlot.ClearStoredInstance();
            }

            // 2. Deduct Packaging
            if (pkgSlot.Quantity > batchSizeNative)
            {
                pkgSlot.ChangeQuantity(-batchSizeNative);
            }
            else
            {
                pkgSlot.ClearStoredInstance();
            }

            // 3. Add to Output slot
            if (outSlot!.ItemInstance == null || outSlot.Quantity <= 0)
            {
                var targetDef = GameRegistry.GetItem(outputItemId);
                if (targetDef != null && targetDef.Pointer != IntPtr.Zero)
                {
                    var newInst = targetDef.GetDefaultInstance(1);
                    if (newInst != null && newInst.Pointer != IntPtr.Zero)
                    {
                        var newQInst = newInst.TryCast<QualityItemInstance>();
                        if (newQInst != null && newQInst.Pointer != IntPtr.Zero)
                        {
                            newQInst.Quality = upgradedQuality;
                        }
                        var newProdInst = newInst.TryCast<ProductItemInstance>();
                        if (newProdInst != null && newProdInst.Pointer != IntPtr.Zero)
                        {
                            if (packagingDefObj != null && packagingDefObj.Pointer != IntPtr.Zero)
                            {
                                newProdInst.SetPackaging(packagingDefObj);
                            }
                            else
                            {
                                newProdInst.PackagingID = packagingId;
                            }
                        }
                        outSlot.SetStoredItem(newInst);
                        if (batchSizeNative > 1)
                        {
                            outSlot.ChangeQuantity(batchSizeNative - 1);
                        }
                    }
                }
            }
            else
            {
                outSlot.ChangeQuantity(batchSizeNative);
            }

            // Notify listeners
            prodSlot.onItemDataChanged?.Invoke();
            prodSlot.onItemInstanceChanged?.Invoke();
            pkgSlot.onItemDataChanged?.Invoke();
            pkgSlot.onItemInstanceChanged?.Invoke();
            outSlot.onItemDataChanged?.Invoke();
            outSlot.onItemInstanceChanged?.Invoke();

            try
            {
                station.UpdatePackagingVisuals();
                station.UpdateProductVisuals();
            }
            catch { }

            Mod.Log.Info($"[AutoPack Engine] Packaged {deductProductNative}x '{rawItemId}' + {batchSizeNative}x '{packagingId}' -> {batchSizeNative}x '{outputItemId}' (Quality: {upgradedQuality}).");
            return true;
        }
        catch (Exception ex)
        {
            Mod.Log.Error($"ExecutePackagingTransaction on station error: {ex}");
            return false;
        }
    }

    internal static bool IsHostOrSingleplayer()
    {
        try
        {
            var nm = Il2CppFishNet.InstanceFinder.NetworkManager;
            if (nm == null || nm.Pointer == IntPtr.Zero || nm.WasCollected || (UnityEngine.Object)nm == null)
                return true;

            return Il2CppFishNet.InstanceFinder.IsServer;
        }
        catch
        {
            return true;
        }
    }

    /// <summary>
    /// Critic Pillar 2: Atomic 2-Phase Transaction against TOCTOU Duplication.
    /// Deducts input in the exact same frame that the upgraded QualityItemInstance is placed into the Output slot.
    /// Quality value and mix-effects are copied 1:1 (+5% freshness bonus applied).
    /// </summary>
    public static bool ExecutePackagingTransaction(string stationGuid)
    {
        if (!IsHostOrSingleplayer()) return false;

        var rData = AutoPackStore.GetRuntimeData(stationGuid);
        var inputProd = rData.InputProduct;
        var inputPkg = rData.InputPackaging;
        var outputSlot = rData.OutputProduct;

        // =========================================================================
        // PHASE 1: PRE-FLIGHT VALIDATION & CAPACITY PROBE (No side-effects)
        // =========================================================================
        if (inputProd == null || inputProd.Quantity <= 0 || string.IsNullOrEmpty(inputProd.ItemId))
        {
            Mod.Log.Debug("Phase 1 abort: No input product in station buffer.");
            return false;
        }

        // Check packaging availability (either packaging item inserted, or auto-packaged)
        if (inputPkg == null || inputPkg.Quantity <= 0 || string.IsNullOrEmpty(inputPkg.ItemId))
        {
            rData.State = StationState.NoPackaging;
            Mod.Log.Debug("Phase 1 abort: No packaging material provided.");
            return false;
        }
        string packagingId = inputPkg.ItemId;

        var pkgDefObj = GameRegistry.GetItem(packagingId)?.TryCast<PackagingDefinition>();
        int requiredProductQty = (pkgDefObj != null && pkgDefObj.Quantity > 0) ? pkgDefObj.Quantity : 1;

        if (inputProd == null || inputProd.Quantity < requiredProductQty || string.IsNullOrEmpty(inputProd.ItemId))
        {
            Mod.Log.Debug("Phase 1 abort: Insufficient input product in station buffer.");
            return false;
        }

        // Determine units to package this cycle — respects MaxBatchSize (H6) but clamped to input/output capacity
        int configuredBatch = Mathf.Clamp(Mod.CurrentConfig.MaxBatchSize, 1, 20);
        int maxByProduct = inputProd.Quantity / Math.Max(1, requiredProductQty);
        int maxByPackaging = inputPkg.Quantity;
        int maxByOutputCap = 20;
        try
        {
            var defProbe = GameRegistry.GetItem(ResolvePackagedItemId(inputProd.ItemId, packagingId));
            if (defProbe != null && defProbe.Pointer != IntPtr.Zero) maxByOutputCap = defProbe.StackLimit;
        }
        catch { }
        if (outputSlot != null && outputSlot.Quantity > 0)
            maxByOutputCap = maxByOutputCap - outputSlot.Quantity;
        int feasible = Math.Min(Math.Min(maxByProduct, maxByPackaging), maxByOutputCap);
        int batchSize = Math.Max(1, Math.Min(configuredBatch, Math.Max(1, feasible)));
        if (batchSize > feasible) batchSize = Math.Max(1, feasible);

        // Resolve target output item ID
        string outputItemId = ResolvePackagedItemId(inputProd.ItemId, packagingId);

        if (string.IsNullOrEmpty(outputItemId))
        {
            Mod.Log.Debug($"Phase 1 abort: Packaged item ID for '{inputProd.ItemId}' + '{packagingId}' not found in registry.");
            return false;
        }

        // Check output slot capacity
        if (outputSlot != null && outputSlot.Quantity > 0)
        {
            if (outputSlot.ItemId != outputItemId || outputSlot.PackagingId != packagingId)
            {
                Mod.Log.Debug($"Phase 1 abort: Output slot contains mismatched item '{outputSlot.ItemId}' (pkg: '{outputSlot.PackagingId}') vs target '{outputItemId}' (pkg: '{packagingId}').");
                return false;
            }

            int stackLimit = 20;
            try
            {
                var def = GameRegistry.GetItem(outputItemId);
                if (def != null && def.Pointer != IntPtr.Zero)
                {
                    stackLimit = def.StackLimit;
                }
            }
            catch { }

            if (outputSlot.Quantity + batchSize > stackLimit)
            {
                Mod.Log.Debug("Phase 1 abort: Output slot at full capacity.");
                return false;
            }
        }

        // =========================================================================
        // PHASE 2: ATOMIC COMMIT (Executed synchronously in exact same frame)
        // =========================================================================
        try
        {
            // 1. Calculate Quality with +5% Freshness Bonus
            float bonusMultiplier = Mathf.Max(0f, Mod.CurrentConfig.FreshnessBonusMultiplier);
            float baseQuality = Mathf.Clamp01(inputProd.QualityValue);
            float upgradedQuality = Mathf.Clamp01(baseQuality * (1.0f + bonusMultiplier));

            // Derive quality tier
            int upgradedTier = PackagingMath.ComputeQualityTier(upgradedQuality);

            // 2. Clone mix-effects 1:1
            var clonedEffects = new List<string>();
            if (inputProd.MixEffects != null)
            {
                for (int i = 0; i < inputProd.MixEffects.Count; i++)
                {
                    clonedEffects.Add(inputProd.MixEffects[i]);
                }
            }

            // 3. Deduct Input Product (Atomic Phase 2A) — respects batchSize (H6)
            int deductProduct = requiredProductQty * batchSize;
            inputProd.Quantity -= deductProduct;
            if (inputProd.Quantity <= 0)
            {
                rData.InputProduct = null;
            }

            // 4. Deduct Packaging Material if used (Atomic Phase 2B)
            if (inputPkg != null && inputPkg.Quantity > 0)
            {
                inputPkg.Quantity -= batchSize;
                if (inputPkg.Quantity <= 0)
                {
                    rData.InputPackaging = null;
                }
            }

            // 5. Place Packaged Item into Output Slot (Atomic Phase 2C)
            if (outputSlot == null || outputSlot.Quantity <= 0)
            {
                rData.OutputProduct = new SlotItemData
                {
                    ItemId = outputItemId,
                    ItemName = PackagingMath.FormatPackagedName(inputProd.ItemName, packagingId),
                    Quantity = batchSize,
                    QualityValue = upgradedQuality,
                    QualityTier = upgradedTier,
                    PackagingId = packagingId,
                    MixEffects = clonedEffects
                };
            }
            else
            {
                int oldQty = outputSlot.Quantity;
                float oldQual = outputSlot.QualityValue;
                outputSlot.Quantity += batchSize;
                outputSlot.QualityValue = ((oldQty * oldQual) + (batchSize * upgradedQuality)) / (oldQty + batchSize);
                outputSlot.QualityTier = PackagingMath.ComputeQualityTier(outputSlot.QualityValue);
            }

            Mod.Log.Info($"[AutoPack Atomic Commit] Packaged {deductProduct}x '{inputProd.ItemId}' -> {batchSize}x '{outputItemId}' (Quality: {baseQuality:P0} -> {upgradedQuality:P0} [+{Mod.CurrentConfig.FreshnessBonusMultiplier:P0} bonus], Effects: {clonedEffects.Count}).");
            return true;
        }
        catch (Exception ex)
        {
            Mod.Log.Error($"Fatal exception during packaging atomic commit: {ex}");
            return false;
        }
    }

    // ComputeQualityTier moved to PackagingMath.cs (testable, no S1API deps).

    public static string ResolvePackagedItemId(string rawItemId, string packagingId)
    {
        if (string.IsNullOrEmpty(rawItemId)) return string.Empty;
        string raw = rawItemId.ToLowerInvariant().Trim();
        string pkg = (packagingId ?? string.Empty).ToLowerInvariant().Trim();

        // 1. Check direct combinations
        string combo = $"{raw}_{pkg}";
        if (GameRegistry.ItemExists(combo)) return combo;

        // 2. Check canonical packaging names
        if (pkg.Contains("jar"))
        {
            string jarId = $"{raw}_jar";
            if (GameRegistry.ItemExists(jarId)) return jarId;
        }
        if (pkg.Contains("brick") || pkg.Contains("box"))
        {
            string brickId = $"{raw}_brick";
            if (GameRegistry.ItemExists(brickId)) return brickId;
        }
        if (pkg.Contains("bag") || pkg.Contains("baggie"))
        {
            string bagId = $"{raw}_baggie";
            if (GameRegistry.ItemExists(bagId)) return bagId;
            string bagId2 = $"{raw}_bag";
            if (GameRegistry.ItemExists(bagId2)) return bagId2;
        }

        // 3. Fallback: Raw product item definition ID itself
        if (GameRegistry.ItemExists(rawItemId)) return rawItemId;
        if (GameRegistry.ItemExists(raw)) return raw;

        return rawItemId;
    }

    // FormatPackagedName moved to PackagingMath.cs (testable, no S1API deps).

    #region Audio Synthesis & SFX

    public static void PlayCompressorStroke(AudioSource? source)
    {
        if (source == null || source.Pointer == IntPtr.Zero || !Mod.CurrentConfig.EnablePneumaticSFX) return;
        try
        {
            if (_compressorStrokeClip == null) _compressorStrokeClip = GenerateCompressorStrokeClip();
            source.PlayOneShot(_compressorStrokeClip, 0.55f);
        }
        catch { }
    }

    public static void PlayPneumaticHiss(AudioSource? source)
    {
        if (source == null || source.Pointer == IntPtr.Zero || !Mod.CurrentConfig.EnablePneumaticSFX) return;
        try
        {
            if (_pneumaticHissClip == null) _pneumaticHissClip = GeneratePneumaticHissClip();
            source.PlayOneShot(_pneumaticHissClip, 0.70f);
        }
        catch { }
    }

    public static void PlayMechanicalClank(AudioSource? source)
    {
        if (source == null || source.Pointer == IntPtr.Zero || !Mod.CurrentConfig.EnablePneumaticSFX) return;
        try
        {
            if (_mechanicalClankClip == null) _mechanicalClankClip = GenerateMechanicalClankClip();
            source.PlayOneShot(_mechanicalClankClip, 0.50f);
        }
        catch { }
    }

    private static AudioClip GeneratePneumaticHissClip()
    {
        const int sampleRate = 44100;
        const float duration = 0.45f;
        int sampleCount = (int)(sampleRate * duration);
        var samples = new float[sampleCount];
        var rng = new System.Random(42);

        for (int i = 0; i < sampleCount; i++)
        {
            float t = (float)i / sampleRate;
            // White noise base modulated with resonant high-frequency hiss
            float noise = (float)(rng.NextDouble() * 2.0 - 1.0);
            float envelope = Mathf.Exp(-6f * t) * (1f - Mathf.Exp(-30f * t));
            float resonance = Mathf.Sin(2f * Mathf.PI * 3200f * t) * 0.25f;
            samples[i] = (noise * 0.75f + resonance) * envelope * 0.60f;
        }

        var clip = AudioClip.Create("AutoPack_PneumaticHiss", sampleCount, 1, sampleRate, false);
        clip.SetData(samples, 0);
        return clip;
    }

    private static AudioClip GenerateCompressorStrokeClip()
    {
        const int sampleRate = 44100;
        const float duration = 0.30f;
        int sampleCount = (int)(sampleRate * duration);
        var samples = new float[sampleCount];

        for (int i = 0; i < sampleCount; i++)
        {
            float t = (float)i / sampleRate;
            float freq = Mathf.Lerp(120f, 65f, (float)i / sampleCount);
            float envelope = Mathf.Exp(-8f * t);
            float val = Mathf.Sin(2f * Mathf.PI * freq * t) + 0.3f * Mathf.Sin(2f * Mathf.PI * freq * 2f * t);
            samples[i] = val * envelope * 0.65f;
        }

        var clip = AudioClip.Create("AutoPack_CompressorStroke", sampleCount, 1, sampleRate, false);
        clip.SetData(samples, 0);
        return clip;
    }

    private static AudioClip GenerateMechanicalClankClip()
    {
        const int sampleRate = 44100;
        const float duration = 0.12f;
        int sampleCount = (int)(sampleRate * duration);
        var samples = new float[sampleCount];

        for (int i = 0; i < sampleCount; i++)
        {
            float t = (float)i / sampleRate;
            float envelope = Mathf.Exp(-25f * t);
            float val = Mathf.Sin(2f * Mathf.PI * 850f * t) + 0.6f * Mathf.Sin(2f * Mathf.PI * 1450f * t);
            samples[i] = val * envelope * 0.50f;
        }

        var clip = AudioClip.Create("AutoPack_MechanicalClank", sampleCount, 1, sampleRate, false);
        clip.SetData(samples, 0);
        return clip;
    }

    #endregion
}
