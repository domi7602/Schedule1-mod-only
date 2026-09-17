using System;
using System.Reflection;
using S1Mods.Shared;

namespace SnackVendor.World.Outdoor;

/// <summary>
/// Interop helper to detect if HomelessMod is installed and actively handling
/// custom outdoor placement for the current frame.
/// </summary>
public static class HomelessModInterop
{
    private static Type? _patchesType;
    private static FieldInfo? _isValidField;
    private static bool _checked;

    private static void EnsureReflected()
    {
        if (_checked) return;
        _checked = true;
        try
        {
            _patchesType = TypeResolver.Find("HomelessMod.Building.BuildingPatches", "HomelessMod");
            if (_patchesType != null)
            {
                _isValidField = _patchesType.GetField("IsCustomPlacementValid", BindingFlags.Public | BindingFlags.Static);
            }
        }
        catch (Exception ex)
        {
            Mod.Log.Debug($"HomelessModInterop reflection check: {ex.Message}");
        }
    }

    /// <summary>
    /// Returns true if HomelessMod is loaded and has validated custom placement for this frame.
    /// </summary>
    public static bool IsHomelessModHandlingPlacement()
    {
        EnsureReflected();
        if (_isValidField == null) return false;
        try
        {
            var val = _isValidField.GetValue(null);
            return val is true;
        }
        catch
        {
            return false;
        }
    }
}
