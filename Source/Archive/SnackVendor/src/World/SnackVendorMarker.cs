extern alias il2cpp;

using System;
using System.Collections.Generic;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.Attributes;
using Il2CppScheduleOne.ObjectScripts;
using Il2CppScheduleOne.Persistence;
using Il2CppScheduleOne.UI;
using MelonLoader;
using UnityEngine;
using S1Mods.Shared;
using GameObject = UnityEngine.GameObject;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace SnackVendor.World;

/// <summary>
/// MonoBehaviour marker attached to a runtime-cloned VendingMachine that
/// "belongs to us". Without this marker the Harmony patches below treat
/// the instance as vanilla Cuke-Machine and skip it — so the Cuke-machines
/// in the vanilla world (e.g. the gas-station spawn) keep their old
/// behaviour forever. Mirrors the schedule1-il2cpp-sorting-patterns
/// guideline: every mod-mutation gate the MUTATION, not the method, and
/// the marker is the cheapest possible gate.
/// </summary>
[RegisterTypeInIl2Cpp]
public sealed class SnackVendorMarker : MonoBehaviour
{
    public SnackVendorMarker(IntPtr ptr) : base(ptr) { }

    /// <summary>Stable instance id across save/load (pinned by controller).</summary>
    public string InstanceGuid = string.Empty;

    /// <summary>Pointer back to the vanilla NetworkBehaviour we cloned; null on host-originals.</summary>
    public VendingMachine? VanillaClone;
    public il2cpp::Il2CppSystem.Guid BakedGuid;
}
