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

        var station = AutoPackStationController.ResolveNativeStation(controller.gameObject);
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
            // v0.3.1: the mirrored vanilla canvas mode decides — Unpackage mode auto-unpacks
            // (native Unpack(), readiness via vanilla GetState), Package mode auto-packs.
            // No priority guessing: both flows are mutually exclusive by the player's mode.
            bool success;
            if (isNativeStation && rData.UnpackageMode)
            {
                success = CanStationUnpack(station!, rData) && ExecuteNativeUnpack(station!);
            }
            else if (isNativeStation)
            {
                // Bug-audit 2026-10-13 (BUG-004/BUG-008): native PackSingleInstance path.
                success = ExecuteNativePack(station!);
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
    /// Bug-audit 2026-10-13 (BUG-008): readiness is delegated to the NATIVE state machine
    /// (GetState(EMode.Package) == CanBegin) — exactly the check the vanilla Begin button
    /// performs. It validates product/packaging compatibility (EState.Mismatch), quantities
    /// (InsufficentProduct) and output capacity (OutputSlotFull) with the game's own rules,
    /// so invalid combinations can no longer consume packaging material. The out-IDs are
    /// informational (readout/logging).
    /// </summary>
    public static bool CanStationPackage(PackagingStation station, out string outputItemId, out string packagingId)
    {
        outputItemId = string.Empty;
        packagingId = string.Empty;

        if (station == null || station.Pointer == IntPtr.Zero) return false;
        try
        {
            if (station.GetState(PackagingStation.EMode.Package) != PackagingStation.EState.CanBegin)
                return false;
        }
        catch (Exception ex)
        {
            Mod.Log.Debug($"CanStationPackage GetState threw: {ex.Message}");
            return false;
        }

        try
        {
            var inputSlots = station.InputSlots;
            if (inputSlots != null && inputSlots.Count >= 2)
            {
                if (inputSlots[0] != null && inputSlots[0].ItemInstance != null && inputSlots[0].ItemInstance.Definition != null)
                    packagingId = inputSlots[0].ItemInstance.Definition.ID ?? string.Empty;
                if (inputSlots[1] != null && inputSlots[1].ItemInstance != null && inputSlots[1].ItemInstance.Definition != null)
                    outputItemId = inputSlots[1].ItemInstance.Definition.ID ?? string.Empty;
            }
        }
        catch { }

        return true;
    }

    /// <summary>
    /// Checks if a native PackagingStation can UNPACKAGE right now.
    /// v0.3.1: Two conditions —
    /// 1. MODE: the runtime data mirrors the vanilla canvas mode (player toggles the red
    ///    UNPACKAGE arrow in the station UI). Auto-unpack runs ONLY in Unpackage mode.
    /// 2. READINESS: the vanilla's own state machine (GetState(EMode.Unpackage) == CanBegin) —
    ///    exactly the check the native Begin button performs. Covers slot layout (packaged
    ///    goods in the OUTPUT slot — the reverse flow), item match and output capacity.
    /// </summary>
    public static bool CanStationUnpack(PackagingStation station, AutoPackStationRuntimeData rData)
    {
        if (station == null || station.Pointer == IntPtr.Zero) return false;
        if (rData == null || !rData.UnpackageMode) return false;

        try
        {
            return station.GetState(PackagingStation.EMode.Unpackage) == PackagingStation.EState.CanBegin;
        }
        catch (Exception ex)
        {
            Mod.Log.Debug($"CanStationUnpack GetState threw: {ex.Message}");
            return false;
        }
    }

    /// <summary>
    /// v0.3.2: Executes unpack cycles by delegating to the NATIVE vanilla implementation
    /// (PackagingStation.Unpack) — one native call per unit, batched up to MaxBatchSize per
    /// auto-cycle (symmetric to auto-pack's batch size). The vanilla readiness state
    /// (GetState(EMode.Unpackage) == CanBegin) is re-checked before EVERY call, so the loop
    /// stops naturally when the stack is empty or a return slot fills up.
    /// Note: each unpacked unit returns its empty packaging (vanilla pickup notification
    /// shows "N Baggies" in the HUD — materials are conserved, that is vanilla behavior,
    /// not a dupe).
    /// </summary>
    public static bool ExecuteNativeUnpack(PackagingStation station)
    {
        if (!IsHostOrSingleplayer()) return false;
        if (station == null || station.Pointer == IntPtr.Zero) return false;

        int batch = Mathf.Clamp(Mod.CurrentConfig.MaxBatchSize, 1, 20);
        int executed = 0;

        for (int i = 0; i < batch; i++)
        {
            try
            {
                if (station.GetState(PackagingStation.EMode.Unpackage) != PackagingStation.EState.CanBegin)
                    break;
            }
            catch (Exception ex)
            {
                Mod.Log.Warn($"Native unpack pre-check threw: {ex.Message}");
                break;
            }

            try
            {
                station.Unpack();
                executed++;
            }
            catch (Exception ex)
            {
                Mod.Log.Error($"[AutoPack Engine] Native Unpack() threw: {ex.Message}");
                break;
            }
        }

        if (executed > 0)
        {
            Mod.Log.Info($"[AutoPack Engine] Native Unpack() executed {executed}x this cycle (batch max {batch}).");
            return true;
        }
        return false;
    }

    /// <summary>
    /// Re-review 2026-10-13 (Befund 3): legacy rData fallback — rolls the freshness bonus
    /// against the committed OUTPUT stack tier, so both paths share the same population.
    /// </summary>
    private static void ApplyFreshnessBonusToBuffer(SlotItemData? output)
    {
        try
        {
            float p = Mathf.Clamp(Mod.CurrentConfig.FreshnessBonusMultiplier, 0f, 1f);
            if (p <= 0f || output == null || output.Quantity <= 0) return;
            int oldTier = (output.QualityTier >= 0 && output.QualityTier <= QualityBonusPolicy.MaxTier) ? output.QualityTier : 2;
            float roll = UnityEngine.Random.value;
            int newTier = QualityBonusPolicy.ApplyFreshnessUpgrade(oldTier, p, roll);
            if (newTier != oldTier)
            {
                output.QualityTier = newTier;
                output.QualityValue = PackagingMath.TierToQualityValue(newTier);
                Mod.Log.Info($"[AutoPack Engine] Freshness bonus (buffer): output quality tier {oldTier} -> {newTier} (roll {roll:F3} < {p:F3}).");
            }
        }
        catch (Exception ex)
        {
            Mod.Log.Debug($"ApplyFreshnessBonusToBuffer skipped: {ex.Message}");
        }
    }

    /// <summary>
    /// Bug-audit 2026-10-13 (BUG-004/BUG-008): the packaging step delegates to the NATIVE
    /// vanilla conversion (PackagingStation.PackSingleInstance — the server-side step the
    /// vanilla Mk1 station uses) — one native call per unit, batched up to MaxBatchSize,
    /// with the vanilla readiness state re-checked before EVERY call (symmetric to
    /// ExecuteNativeUnpack).
    /// The old hand-rolled transaction could return success without producing output
    /// (second GetDefaultInstance failure after deducting inputs), relied on heuristic
    /// item-ID resolution, and its rollback did not restore a blended output quality.
    /// The native path owns the transaction, the output instance and the
    /// product/packaging compatibility rules — none of those failure modes remain.
    /// </summary>
    public static bool ExecuteNativePack(PackagingStation station)
    {
        if (!IsHostOrSingleplayer()) return false;
        if (station == null || station.Pointer == IntPtr.Zero) return false;

        int batch = Mathf.Clamp(Mod.CurrentConfig.MaxBatchSize, 1, 20);
        int executed = 0;
        for (int i = 0; i < batch; i++)
        {
            try
            {
                if (station.GetState(PackagingStation.EMode.Package) != PackagingStation.EState.CanBegin)
                    break;
            }
            catch (Exception ex)
            {
                Mod.Log.Warn($"Native pack pre-check threw: {ex.Message}");
                break;
            }

            try
            {
                station.PackSingleInstance();
                executed++;
            }
            catch (Exception ex)
            {
                Mod.Log.Error($"[AutoPack Engine] Native PackSingleInstance() threw: {ex.Message}");
                break;
            }
        }

        if (executed > 0)
        {
            ApplyFreshnessBonus(station);
            try { station.UpdatePackagingVisuals(); station.UpdateProductVisuals(); } catch { }
            Mod.Log.Info($"[AutoPack Engine] Native PackSingleInstance executed {executed}x this cycle (batch max {batch}).");
            return true;
        }
        return false;
    }

    /// <summary>
    /// Bug-audit 2026-10-13 (BUG-010): chance-based one-tier freshness upgrade on the
    /// output stack (QualityBonusPolicy is unit-tested). Rolls once per successful cycle.
    /// </summary>
    private static void ApplyFreshnessBonus(PackagingStation station)
    {
        try
        {
            float p = Mathf.Clamp(Mod.CurrentConfig.FreshnessBonusMultiplier, 0f, 1f);
            if (p <= 0f) return;

            var outputSlots = station.OutputSlots;
            if (outputSlots == null || outputSlots.Count < 1) return;
            var outSlot = outputSlots[0];
            if (outSlot == null || outSlot.Pointer == IntPtr.Zero || outSlot.ItemInstance == null || outSlot.ItemInstance.Pointer == IntPtr.Zero) return;

            var qInst = outSlot.ItemInstance.TryCast<QualityItemInstance>();
            if (qInst == null || qInst.Pointer == IntPtr.Zero) return;

            int oldTier = (int)qInst.Quality;
            float roll = UnityEngine.Random.value;
            int newTier = QualityBonusPolicy.ApplyFreshnessUpgrade(oldTier, p, roll);
            if (newTier != oldTier)
            {
                qInst.Quality = (EQuality)newTier;
                Mod.Log.Info($"[AutoPack Engine] Freshness bonus: output quality tier {oldTier} -> {newTier} (roll {roll:F3} < {p:F3}).");
                try { station.UpdateProductVisuals(); } catch { }
            }
        }
        catch (Exception ex)
        {
            Mod.Log.Debug($"ApplyFreshnessBonus skipped: {ex.Message}");
        }
    }

    /// <summary>
    /// Consolidated 2026-09-15 in S1Mods.Shared.NetworkGuard.IsHostOrSingleplayer
    /// (IL2CPP-safe pointer/WasCollected checks, fail-closed on exceptions).
    /// </summary>
    internal static bool IsHostOrSingleplayer() => NetworkGuard.IsHostOrSingleplayer();

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

        // Bug-audit 2026-10-13 (BUG-008): strict combo validation for the legacy rData
        // fallback path. The old code fell back to the raw product ID and a guessed
        // quantity of 1 when no PackagingDefinition existed, so invalid combinations
        // consumed packaging material. A packaged product keeps its product item ID and
        // carries the packaging on the instance (native item model) — no ID heuristics.
        if (!ValidatePackagingCombo(inputProd.ItemId, packagingId, out var pkgDefObj))
        {
            rData.State = StationState.Blocked;
            Mod.Log.Warn($"Phase 1 abort: invalid product/packaging combination '{inputProd.ItemId}' + '{packagingId}'.");
            return false;
        }
        int requiredProductQty = (pkgDefObj!.Quantity > 0) ? pkgDefObj.Quantity : 1;
        string outputItemId = inputProd.ItemId;

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
            var defProbe = GameRegistry.GetItem(outputItemId);
            if (defProbe != null && defProbe.Pointer != IntPtr.Zero) maxByOutputCap = defProbe.StackLimit;
        }
        catch { }
        if (outputSlot != null && outputSlot.Quantity > 0)
            maxByOutputCap = maxByOutputCap - outputSlot.Quantity;
        int feasible = Math.Min(Math.Min(maxByProduct, maxByPackaging), maxByOutputCap);
        // feasible <= 0 means output-full or no input: abort here like the native path
        // above instead of coercing to batchSize=1 and relying on a later capacity abort.
        if (feasible <= 0) return false;
        int batchSize = Math.Max(1, Math.Min(configuredBatch, feasible));

        // Check output slot capacity (outputItemId was resolved above: the packaged
        // product keeps its product item ID and carries the packaging on the instance —
        // native item model, no ID heuristics)
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
        // Transaction snapshot: restore buffers on a mid-commit failure (item-loss fix)
        int snapProdQty = inputProd.Quantity;
        var snapProdRef = inputProd;
        int snapPkgQty = inputPkg.Quantity;
        var snapPkgRef = inputPkg;
        var snapOutRef = outputSlot;
        int snapOutQty = outputSlot?.Quantity ?? 0;
        float snapOutQual = outputSlot?.QualityValue ?? 0f;
        int snapOutTier = outputSlot?.QualityTier ?? 2;
        bool snapOutWasEmpty = (outputSlot == null || outputSlot.Quantity <= 0);

        try
        {
            // 1. Base quality (re-review 2026-10-13, Befund 3): the buffer is committed
            // with the INPUT batch tier; the freshness bonus is rolled AFTERWARDS against
            // the FINAL output stack tier — exactly like the native path rolls against the
            // produced output stack. (The old "qualityValue * (1 + bonus)" value math never
            // crossed a tier boundary, and rolling pre-commit against the input tier skewed
            // the bonus distribution vs native.)
            int inputTier = (inputProd.QualityTier >= 0 && inputProd.QualityTier <= QualityBonusPolicy.MaxTier) ? inputProd.QualityTier : 2;
            float baseQuality = PackagingMath.TierToQualityValue(inputTier);

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
                    QualityValue = baseQuality,
                    QualityTier = inputTier,
                    PackagingId = packagingId,
                    MixEffects = clonedEffects
                };
            }
            else
            {
                int oldQty = outputSlot.Quantity;
                float oldQual = outputSlot.QualityValue;
                outputSlot.Quantity += batchSize;
                outputSlot.QualityValue = ((oldQty * oldQual) + (batchSize * baseQuality)) / (oldQty + batchSize);
                outputSlot.QualityTier = PackagingMath.ComputeQualityTier(outputSlot.QualityValue);
            }

            Mod.Log.Info($"[AutoPack Atomic Commit] Packaged {deductProduct}x '{inputProd.ItemId}' -> {batchSize}x '{outputItemId}' (base tier {inputTier}, Effects: {clonedEffects.Count}).");

            // 6. Freshness bonus against the final output stack (same population as native)
            ApplyFreshnessBonusToBuffer(rData.OutputProduct);
            return true;
        }
        catch (Exception ex)
        {
            // Revert to the snapshotted buffer state so inputs aren't lost without output
            try
            {
                if (snapProdRef != null)
                {
                    snapProdRef.Quantity = snapProdQty;
                    rData.InputProduct = snapProdRef;
                }
                if (snapPkgRef != null)
                {
                    snapPkgRef.Quantity = snapPkgQty;
                    rData.InputPackaging = snapPkgRef;
                }
                if (snapOutWasEmpty)
                {
                    rData.OutputProduct = null;
                }
                else if (snapOutRef != null)
                {
                    snapOutRef.Quantity = snapOutQty;
                    snapOutRef.QualityValue = snapOutQual;
                    snapOutRef.QualityTier = snapOutTier;
                    rData.OutputProduct = snapOutRef;
                }
            }
            catch (Exception revertEx)
            {
                Mod.Log.Warn($"Packaging commit revert failed: {revertEx.Message}");
            }
            Mod.Log.Error($"Fatal exception during packaging atomic commit: {ex}");
            return false;
        }
    }

    // ComputeQualityTier moved to PackagingMath.cs (testable, no S1API deps).

    /// <summary>
    /// Strict product+packaging validation (bug-audit 2026-10-13, BUG-008). The old
    /// heuristic name resolver ("{raw}_{pkg}", "_jar", "_brick", "_baggie") fell back to
    /// the raw product ID and thereby accepted invalid combinations, consuming packaging
    /// material for nothing. Per the native item model a packaged product keeps its
    /// product item ID and carries the packaging on the instance — so there is nothing
    /// to "resolve" beyond validation:
    ///  1. the packaging MUST be a real PackagingDefinition,
    ///  2. the product MUST be a real ProductDefinition,
    ///  3. when the product declares ValidPackaging, the packaging must be part of it.
    /// Fallback path only — the native path delegates to GetState/PackSingleInstance.
    /// </summary>
    public static bool ValidatePackagingCombo(string rawItemId, string packagingId, out PackagingDefinition? packagingDef)
    {
        packagingDef = null;
        if (string.IsNullOrEmpty(rawItemId) || string.IsNullOrEmpty(packagingId)) return false;

        var pkg = GameRegistry.GetItem(packagingId)?.TryCast<PackagingDefinition>();
        if (pkg == null || pkg.Pointer == IntPtr.Zero) return false;

        var prod = GameRegistry.GetItem(rawItemId)?.TryCast<Il2CppScheduleOne.Product.ProductDefinition>();
        if (prod == null || prod.Pointer == IntPtr.Zero) return false;

        try
        {
            var valid = prod.ValidPackaging;
            if (valid != null && valid.Length > 0)
            {
                bool compatible = false;
                for (int i = 0; i < valid.Length; i++)
                {
                    var candidate = valid[i];
                    if (candidate == null || candidate.Pointer == IntPtr.Zero) continue;
                    if (candidate.Pointer == pkg.Pointer || string.Equals(candidate.ID, packagingId, StringComparison.OrdinalIgnoreCase))
                    {
                        compatible = true;
                        break;
                    }
                }
                if (!compatible) return false;
            }
        }
        catch (Exception ex)
        {
            Mod.Log.Debug($"ValidatePackagingCombo ValidPackaging check threw: {ex.Message}");
            return false;
        }

        packagingDef = pkg;
        return true;
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
